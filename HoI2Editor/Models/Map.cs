using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     マップデータの管理
    /// </summary>
    public static class Map
    {
        #region 公開プロパティ

        /// <summary>
        ///     マップ画像
        /// </summary>
        public static Bitmap Image { get; private set; }

        /// <summary>
        ///     マップブロック単位の幅
        /// </summary>
        public static int Width { get; private set; }

        /// <summary>
        ///     マップブロック単位の高さ
        /// </summary>
        public static int Height { get; private set; }

        /// <summary>
        ///     マップ画素の配列
        /// </summary>
        public static MapPixels Pixels { get; private set; }

        /// <summary>
        ///     マップブロックの配列
        /// </summary>
        public static MapBlocks Blocks { get; private set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     マップファイル名
        /// </summary>
        private static string _fileName;

        /// <summary>
        ///     ファイル読み込みデータ
        /// </summary>
        private static byte[] _data;

        /// <summary>
        ///     読み込み位置
        /// </summary>
        private static int _index;

        /// <summary>
        ///     マップブロックのオフセット位置
        /// </summary>
        private static uint[] _offsets;

        /// <summary>
        ///     ツリー展開用スタック
        /// </summary>
        private static MapTreeNode[] _stack;

        /// <summary>
        ///     マップピクセル展開用バッファ
        /// </summary>
        private static byte[] _buffer;

        /// <summary>
        ///     ツリー巡回時の対象マップブロック
        /// </summary>
        private static MapBlock _block;

        /// <summary>
        ///     ツリー巡回時の基準ピクセル位置
        /// </summary>
        private static int _base;

        #endregion

        #region 内部定数

        /// <summary>
        ///     プロヴィンスIDの最大数
        /// </summary>
        private const int MaxProvinceId = 256;

        #endregion

        #region ファイル読み込み

        /// <summary>
        ///     マップファイルを読み込む
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="width">マップブロック単位の幅</param>
        /// <param name="height">マップブロック単位の高さ</param>
        public static void Load(string fileName, int width, int height)
        {
            _fileName = fileName;

            Width = width;
            Height = height;

            var sw = new Stopwatch();
            sw.Start();

            // マップデータをメモリへ展開する
            LoadFile(fileName);

            // マップブロックのオフセット位置を読み込む
            int count = width * height;
            LoadOffsets(count);

            int dataoffset = (count + 1) * 4;
            Blocks = new MapBlocks(width, height);
            _stack = new MapTreeNode[MapBlock.Width * MapBlock.Height * 2];

            for (int i = 0; i < count; i++)
            {
                _index = dataoffset + (int) _offsets[i];
                Blocks[i] = LoadBlock();
            }

            // 使用済みのバッファを解放する
            _data = null;
            _offsets = null;
            _stack = null;

            sw.Stop();
            Log.Info("[Map] Load: {0}ms", sw.ElapsedMilliseconds);
        }

        /// <summary>
        ///     マップデータをメモリへ展開する
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        private static void LoadFile(string fileName)
        {
            var reader = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            _data = new byte[reader.Length];
            reader.Read(_data, 0, (int) reader.Length);
            reader.Close();
        }

        /// <summary>
        ///     マップブロックのオフセット位置を読み込む
        /// </summary>
        /// <param name="count">マップブロック数</param>
        private static void LoadOffsets(int count)
        {
            _offsets = new uint[count + 1];

            _index = 0;
            for (int i = 0; i <= count; i++)
            {
                _offsets[i] = _data[_index++] |
                              (uint) (_data[_index++] << 8) |
                              (uint) (_data[_index++] << 16) |
                              (uint) (_data[_index++] << 24);
            }
        }

        /// <summary>
        ///     マップブロックを読み込む
        /// </summary>
        private static MapBlock LoadBlock()
        {
            var block = new MapBlock();

            LoadProvinceIds(block);
            LoadMapTree(block);
            LoadNodeIds(block);
            LoadNodeColors(block);

            return block;
        }

        /// <summary>
        ///     プロヴィンスIDリストを読み込む
        /// </summary>
        /// <param name="block">対象マップブロック</param>
        private static void LoadProvinceIds(MapBlock block)
        {
            var ids = new ushort[MaxProvinceId];
            int no = 0;

            const ushort terminator = 0x8000;
            const ushort mask = 0x7FFF;
            ushort work;
            do
            {
                work = _data[_index++];
                work |= (ushort) (_data[_index++] << 8);
                ids[no++] = (ushort) (work & mask);
            } while ((work & terminator) == 0);

            block.ProvinceIds = ids;
            block.ProvinceIdCount = no;
        }

        /// <summary>
        ///     マップツリーを読み込む
        /// </summary>
        /// <param name="block">対象マップブロック</param>
        private static void LoadMapTree(MapBlock block)
        {
            MapTreeNode[] stack = _stack;
            int sp = 0;

            var node = new MapTreeNode {Level = MapTreeNode.MaxLevel};
            block.Nodes = node;
            int no = 0;

            byte mask = 0x00;
            byte data = 0x00;
            while (true)
            {
                mask <<= 1;
                if (mask == 0x00)
                {
                    data = _data[_index++];
                    mask = 0x01;
                }

                if ((data & mask) == 0)
                {
                    node.No = no++;

                    if (sp == 0)
                    {
                        break;
                    }
                    node = stack[--sp];
                }
                else
                {
                    int level = node.Level - 1;
                    if (level > 0)
                    {
                        int width = 1 << level;

                        int left = node.X;
                        int right = left + width;
                        int top = node.Y;
                        int bottom = top + width;

                        var topLeft = new MapTreeNode {Level = level, X = left, Y = top};
                        node.TopLeftChild = topLeft;
                        stack[sp++] = topLeft;

                        var topRight = new MapTreeNode {Level = level, X = right, Y = top};
                        node.TopRightChild = topRight;
                        stack[sp++] = topRight;

                        var bottomLeft = new MapTreeNode {Level = level, X = left, Y = bottom};
                        node.BottomLeftChild = bottomLeft;
                        stack[sp++] = bottomLeft;

                        var bottomRight = new MapTreeNode {Level = level, X = right, Y = bottom};
                        node.BottomRightChild = bottomRight;
                        node = bottomRight;
                    }
                    else
                    {
                        int left = node.X;
                        int right = left + 1;
                        int top = node.Y;
                        int bottom = top + 1;

                        node.BottomRightChild = new MapTreeNode {No = no++, X = right, Y = bottom};
                        node.BottomLeftChild = new MapTreeNode {No = no++, X = left, Y = bottom};
                        node.TopRightChild = new MapTreeNode {No = no++, X = right, Y = top};
                        node.TopLeftChild = new MapTreeNode {No = no++, X = left, Y = top};

                        if (sp == 0)
                        {
                            break;
                        }
                        node = stack[--sp];
                    }
                }
            }

            block.NodeCount = no;
        }

        /// <summary>
        ///     ツリーノードごとのプロヴィンスIDを読み込む
        /// </summary>
        /// <param name="block">対象マップブロック</param>
        private static void LoadNodeIds(MapBlock block)
        {
            int count = block.NodeCount;
            var ids = new byte[count + 7];

            switch (block.ProvinceIdCount)
            {
                case 1:
                    //for (int i = 0; i < count; i++)
                    //{
                    //    ids[i] = 0;
                    //}
                    break;

                case 2:
                    for (int i = 0; i < count;)
                    {
                        const byte mask = 0x01;
                        byte data = _data[_index++];
                        ids[i++] = (byte) (data & mask);
                        data >>= 1;
                        ids[i++] = (byte) (data & mask);
                        data >>= 1;
                        ids[i++] = (byte) (data & mask);
                        data >>= 1;
                        ids[i++] = (byte) (data & mask);
                        data >>= 1;
                        ids[i++] = (byte) (data & mask);
                        data >>= 1;
                        ids[i++] = (byte) (data & mask);
                        data >>= 1;
                        ids[i++] = (byte) (data & mask);
                        data >>= 1;
                        ids[i++] = data;
                    }
                    break;

                case 3:
                case 4:
                    for (int i = 0; i < count;)
                    {
                        const byte mask = 0x03;
                        byte data = _data[_index++];
                        ids[i++] = (byte) (data & mask);
                        data >>= 2;
                        ids[i++] = (byte) (data & mask);
                        data >>= 2;
                        ids[i++] = (byte) (data & mask);
                        data >>= 2;
                        ids[i++] = data;
                    }
                    break;

                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                    for (int i = 0; i < count;)
                    {
                        const byte mask = 0x0F;
                        byte data = _data[_index++];
                        ids[i++] = (byte) (data & mask);
                        data >>= 4;
                        ids[i++] = data;
                    }
                    break;

                default:
                    for (int i = 0; i < count; i++)
                    {
                        ids[i] = _data[_index++];
                    }
                    break;
            }

            block.NodeIds = ids;
        }

        /// <summary>
        ///     ノードごとのカラーインデックスを読み込む
        /// </summary>
        /// <param name="block">対象マップブロック</param>
        private static void LoadNodeColors(MapBlock block)
        {
            int count = block.NodeCount;
            var colors = new byte[count + 3];
            const uint mask = 0x3F;

            for (int i = 0; i < count;)
            {
                var data = (uint) (_data[_index++] | (_data[_index++] << 8) | (_data[_index++] << 16));
                colors[i++] = (byte) (data & mask);
                colors[i++] = (byte) ((data >> 6) & mask);
                colors[i++] = (byte) ((data >> 12) & mask);
                colors[i++] = (byte) (data >> 18);
            }

            block.NodeColors = colors;
        }

        #endregion

        #region ファイル書き込み

        /// <summary>
        ///     マップ画像を保存する
        /// </summary>
        /// <param name="folderName">保存先フォルダ名</param>
        public static void Save(string folderName)
        {
            var sw = new Stopwatch();
            sw.Start();

            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            int hCount = Width / MapPiece.Width;
            int vCount = Height / MapPiece.Height;

            for (int i = 0; i < vCount; i++)
            {
                for (int j = 0; j < hCount; j++)
                {
                    MapPiece.Save(Pixels, j * MapPiece.Width, i * MapPiece.Height, folderName);
                }
            }

            sw.Stop();
            Log.Info("[Map] Save: {0}ms", sw.ElapsedMilliseconds);
        }

        #endregion

        #region マップ画像展開

        /// <summary>
        ///     マップ画像を展開する
        /// </summary>
        public static void Decode()
        {
            var sw = new Stopwatch();
            sw.Start();

            var bitmap = new Bitmap(Width * MapBlock.Width, Height * MapBlock.Height, PixelFormat.Format8bppIndexed);

            ColorPalette palette = bitmap.Palette;
            for (int i = 0; i < 256; i++)
            {
                palette.Entries[i] = Color.FromArgb(255 - i, 255 - i, 255 - i);
            }
            bitmap.Palette = palette;

            BitmapData image = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly,
                PixelFormat.Format8bppIndexed);

            Pixels = new MapPixels(Width * MapBlock.Width, Height * MapBlock.Height);
            _buffer = new byte[(MapBlock.Width + 1) * (MapBlock.Height + 1)];

            // 右下端のブロックをデコードする
            DecodeBlockBottomRight();

            // 下端のブロックをデコードする
            for (int j = Blocks.Width - 2; j >= 0; j--)
            {
                DecodeBlockBottom(j);
            }

            // 右端のブロックをデコードする
            for (int i = Blocks.Height - 2; i >= 0; i--)
            {
                DecodeBlockRight(i);
            }

            for (int i = Blocks.Height - 2; i >= 0; i--)
            {
                // 右端のブロックをデコードする
                DecodeBlockRight(i);

                // その他のブロックをデコードする
                for (int j = Blocks.Width - 2; j >= 0; j--)
                {
                    DecodeBlock(j, i);
                }
            }

            Marshal.Copy(Pixels.Data, 0, image.Scan0, Pixels.Data.Length);
            bitmap.UnlockBits(image);

            Image = bitmap;

            sw.Stop();
            Log.Info("[Map] Decode: {0}ms", sw.ElapsedMilliseconds);
        }

        /// <summary>
        ///     右下端のマップブロックをデコードする
        /// </summary>
        private static void DecodeBlockBottomRight()
        {
            // 右下端の領域外ピクセルを準備する
            _block = Blocks[0, Blocks.Height - 1];
            VisitTreeBottomLeft(_block.Nodes, PrepareBottomRight);

            // 右端の領域外ピクセルを準備する
            VisitTreeLeft(_block.Nodes, PrepareRight);

            // 下端の領域外ピクセルを準備する
            _block = Blocks[Blocks.Width - 1, Blocks.Height - 1];
            VisitTreeBottom(_block.Nodes, PrepareBottom);

            // ツリーをデコードする
            VisitTree(_block.Nodes, DrawNodeBuffer);

            // 展開用バッファからマップピクセルの配列へコピーする
            CopyBufferPixels(Blocks.Width - 1, Blocks.Height - 1);
        }

        /// <summary>
        ///     右端のマップブロックをデコードする
        /// </summary>
        /// <param name="y">Y座標</param>
        private static void DecodeBlockRight(int y)
        {
            // 右下端の領域外ピクセルを準備する
            _block = Blocks[0, y];
            VisitTreeBottomLeft(_block.Nodes, PrepareBottomRight);

            // 右端の領域外ピクセルを準備する
            VisitTreeLeft(_block.Nodes, PrepareRight);

            // 下端のピクセルを準備する
            Buffer.BlockCopy(Pixels.Data, (y + 1) * MapBlock.Height * Pixels.Width + (Pixels.Width - 1) * MapBlock.Width,
                _buffer, MapBlock.Height * (MapBlock.Width + 1), MapBlock.Width);

            // ツリーをデコードする
            _block = Blocks[Blocks.Width - 1, y];
            VisitTree(_block.Nodes, DrawNodeBuffer);

            // 展開用バッファからマップピクセルの配列へコピーする
            CopyBufferPixels(Blocks.Width - 1, y);
        }

        /// <summary>
        ///     下端のマップブロックをデコードする
        /// </summary>
        /// <param name="x">X座標</param>
        private static void DecodeBlockBottom(int x)
        {
            // 右下端の領域外ピクセルを準備する
            _block = Blocks[x + 1, Blocks.Height - 1];
            VisitTreeBottomLeft(_block.Nodes, PrepareBottomRight);

            // 右端の領域外ピクセルを準備する
            int pos = MapBlock.Width;
            int index = (Blocks.Height - 1) * MapBlock.Height * Pixels.Width + (x + 1) * MapBlock.Width;
            for (int i = 0; i < MapBlock.Height; i++)
            {
                _buffer[pos] = Pixels[index];
                pos += MapBlock.Width + 1;
                index += Pixels.Width;
            }

            // 下端のピクセルを準備する
            _block = Blocks[x, Blocks.Height - 1];
            VisitTreeBottom(_block.Nodes, PrepareBottom);

            // ツリーをデコードする
            VisitTree(_block.Nodes, DrawNodeBuffer);

            // 展開用バッファからマップピクセルの配列へコピーする
            CopyBufferPixels(x, Blocks.Height - 1);
        }

        /// <summary>
        ///     マップブロックをデコードする
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        private static void DecodeBlock(int x, int y)
        {
            // ツリーをデコードする
            _block = Blocks[x, y];
            _base = y * MapBlock.Height * Pixels.Width + x * MapBlock.Width;
            VisitTree(_block.Nodes, DrawNode);
        }

        /// <summary>
        ///     右下端の領域外ピクセルを準備する
        /// </summary>
        /// <param name="node">対象ノード</param>
        private static void PrepareBottomRight(MapTreeNode node)
        {
            _buffer[MapBlock.Height * (MapBlock.Width + 1) + MapBlock.Width] = (byte) (_block.NodeColors[node.No] << 2);
        }

        /// <summary>
        ///     右端の領域外ピクセルを準備する
        /// </summary>
        /// <param name="node">対象ノード</param>
        private static void PrepareRight(MapTreeNode node)
        {
            int pos = node.Y * (MapBlock.Width + 1) + MapBlock.Width;
            if (node.Level == 0)
            {
                _buffer[pos] = (byte) (_block.NodeColors[node.No] << 2);
                return;
            }
            int height = 1 << node.Level;
            int top = _block.NodeColors[node.No] << 10;
            int bottom = _buffer[pos + height * (MapBlock.Width + 1)] << 8;
            int delta = (bottom - top) >> node.Level;
            int color = top;
            for (int i = 0; i < height; i++)
            {
                _buffer[pos] = (byte) (color >> 8);
                pos += MapBlock.Width + 1;
                color += delta;
            }
        }

        /// <summary>
        ///     下端の領域外ピクセルを準備する
        /// </summary>
        /// <param name="node">対象ノード</param>
        private static void PrepareBottom(MapTreeNode node)
        {
            int pos = MapBlock.Height * (MapBlock.Width + 1) + node.X;
            if (node.Level == 0)
            {
                _buffer[pos] = (byte) (_block.NodeColors[node.No] << 2);
                return;
            }
            int width = 1 << node.Level;
            int left = _block.NodeColors[node.No] << 10;
            int right = _buffer[pos + width] << 8;
            int delta = (right - left) >> node.Level;
            int color = left;
            for (int i = 0; i < width; i++)
            {
                _buffer[pos++] = (byte) (color >> 8);
                color += delta;
            }
        }

        /// <summary>
        ///     ノードを展開用バッファに描画する
        /// </summary>
        /// <param name="node">対象ノード</param>
        private static void DrawNodeBuffer(MapTreeNode node)
        {
            int pos = node.Y * (MapBlock.Width + 1) + node.X;
            if (node.Level == 0)
            {
                _buffer[pos] = (byte) (_block.NodeColors[node.No] << 2);
                return;
            }
            int width = 1 << node.Level;
            int step = MapBlock.Width + 1 - width;
            int top = _block.NodeColors[node.No] << 10;
            int bottom = _buffer[pos + width * (MapBlock.Width + 1)] << 8;
            int deltaY = (bottom - top) >> node.Level;
            int left = top;
            for (int i = 0; i < width; i++)
            {
                int right = _buffer[pos + width] << 8;
                int deltaX = (right - left) >> node.Level;
                int color = left;
                for (int j = 0; j < width; j++)
                {
                    _buffer[pos++] = (byte) (color >> 8);
                    color += deltaX;
                }
                pos += step;
                left += deltaY;
            }
        }

        /// <summary>
        ///     ノードをマップピクセルの配列に展開する
        /// </summary>
        /// <param name="node">対象ノード</param>
        private static void DrawNode(MapTreeNode node)
        {
            int pos = _base + node.Y * Pixels.Width + node.X;
            byte[] pixels = Pixels.Data;
            if (node.Level == 0)
            {
                pixels[pos] = (byte) (_block.NodeColors[node.No] << 2);
                return;
            }
            int width = 1 << node.Level;
            int step = Pixels.Width - width + 1;
            int top = _block.NodeColors[node.No] << 10;
            int bottom = pixels[pos + width * Pixels.Width] << 8;
            int deltaY = (bottom - top) >> node.Level;
            int left = top;
            switch (node.Level)
            {
                case 1:
                    for (int i = 0; i < width; i++)
                    {
                        int right = pixels[pos + width] << 8;
                        if (left == right)
                        {
                            var color = (byte) (left >> 8);
                            pixels[pos++] = color;
                            pixels[pos] = color;
                        }
                        else
                        {
                            int deltaX = (right - left) >> node.Level;
                            int color = left;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos] = (byte) (color >> 8);
                        }
                        pos += step;
                        left += deltaY;
                    }
                    break;

                case 2:
                    for (int i = 0; i < width; i++)
                    {
                        int right = pixels[pos + width] << 8;
                        if (left == right)
                        {
                            var color = (byte) (left >> 8);
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos] = color;
                        }
                        else
                        {
                            int deltaX = (right - left) >> node.Level;
                            int color = left;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos] = (byte) (color >> 8);
                        }
                        pos += step;
                        left += deltaY;
                    }
                    break;

                case 3:
                    for (int i = 0; i < width; i++)
                    {
                        int right = pixels[pos + width] << 8;
                        if (left == right)
                        {
                            var color = (byte) (left >> 8);
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos] = color;
                        }
                        else
                        {
                            int deltaX = (right - left) >> node.Level;
                            int color = left;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos] = (byte) (color >> 8);
                        }
                        pos += step;
                        left += deltaY;
                    }
                    break;

                case 4:
                    for (int i = 0; i < width; i++)
                    {
                        int right = pixels[pos + width] << 8;
                        if (left == right)
                        {
                            var color = (byte) (left >> 8);
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos] = color;
                        }
                        else
                        {
                            int deltaX = (right - left) >> node.Level;
                            int color = left;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos] = (byte) (color >> 8);
                        }
                        pos += step;
                        left += deltaY;
                    }
                    break;

                case 5:
                    for (int i = 0; i < width; i++)
                    {
                        int right = pixels[pos + width] << 8;
                        if (left == right)
                        {
                            var color = (byte) (left >> 8);
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos++] = color;
                            pixels[pos] = color;
                        }
                        else
                        {
                            int deltaX = (right - left) >> node.Level;
                            int color = left;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos++] = (byte) (color >> 8);
                            color += deltaX;
                            pixels[pos] = (byte) (color >> 8);
                        }
                        pos += step;
                        left += deltaY;
                    }
                    break;
            }
        }

        /// <summary>
        ///     展開用バッファからマップピクセルの配列へコピーする
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        private static void CopyBufferPixels(int x, int y)
        {
            int pos = 0;
            int index = (y * Pixels.Width + x) * MapBlock.Width;
            int step = Pixels.Width;
            for (int i = 0; i < MapBlock.Width; i++)
            {
                Buffer.BlockCopy(_buffer, pos, Pixels.Data, index, MapBlock.Width);
                pos += MapBlock.Width + 1;
                index += step;
            }
        }

        #endregion

        #region ツリー巡回

        /// <summary>
        ///     マップブロック内のツリーを巡回する
        /// </summary>
        /// <param name="node">開始ノード</param>
        /// <param name="callback">葉ノードで呼び出す処理</param>
        private static void VisitTree(MapTreeNode node, VisitorCallback callback)
        {
            var stack = new Stack<MapTreeNode>();
            while (true)
            {
                if (node.TopLeftChild != null)
                {
                    stack.Push(node.TopLeftChild);
                    stack.Push(node.TopRightChild);
                    stack.Push(node.BottomLeftChild);
                    node = node.BottomRightChild;
                }
                else
                {
                    callback(node);
                    if (stack.Count == 0)
                    {
                        break;
                    }
                    node = stack.Pop();
                }
            }
        }

        /// <summary>
        ///     マップブロック内のツリーを巡回する(下側ノードのみ)
        /// </summary>
        /// <param name="node">開始ノード</param>
        /// <param name="callback">葉ノードで呼び出す処理</param>
        private static void VisitTreeBottom(MapTreeNode node, VisitorCallback callback)
        {
            var stack = new Stack<MapTreeNode>();
            while (true)
            {
                if (node.BottomLeftChild != null)
                {
                    stack.Push(node.BottomLeftChild);
                    node = node.BottomRightChild;
                }
                else
                {
                    callback(node);
                    if (stack.Count == 0)
                    {
                        break;
                    }
                    node = stack.Pop();
                }
            }
        }

        /// <summary>
        ///     マップブロック内のツリーを巡回する(左側ノードのみ)
        /// </summary>
        /// <param name="node">開始ノード</param>
        /// <param name="callback">葉ノードで呼び出す処理</param>
        private static void VisitTreeLeft(MapTreeNode node, VisitorCallback callback)
        {
            var stack = new Stack<MapTreeNode>();
            while (true)
            {
                if (node.TopLeftChild != null)
                {
                    stack.Push(node.TopLeftChild);
                    node = node.BottomLeftChild;
                }
                else
                {
                    callback(node);
                    if (stack.Count == 0)
                    {
                        break;
                    }
                    node = stack.Pop();
                }
            }
        }

        /// <summary>
        ///     マップブロック内のツリーを巡回する(左下ノードのみ)
        /// </summary>
        /// <param name="node">開始ノード</param>
        /// <param name="callback">葉ノードで呼び出す処理</param>
        private static void VisitTreeBottomLeft(MapTreeNode node, VisitorCallback callback)
        {
            while (node.BottomLeftChild != null)
            {
                node = node.BottomLeftChild;
            }
            callback(node);
        }

        /// <summary>
        ///     葉ノードで呼び出す処理のデリゲート
        /// </summary>
        /// <param name="node">対象ノード</param>
        private delegate void VisitorCallback(MapTreeNode node);

        #endregion
    }

    /// <summary>
    ///     マップ画素の配列
    /// </summary>
    public class MapPixels
    {
        #region 公開プロパティ

        /// <summary>
        ///     マップ画素を取得する
        /// </summary>
        /// <param name="index">配列のインデックス</param>
        /// <returns>マップ画素</returns>
        public byte this[int index]
        {
            get { return Data[index]; }
            set { Data[index] = value; }
        }

        /// <summary>
        ///     マップ画素を取得する
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <returns>マップ画素</returns>
        public byte this[int x, int y]
        {
            get { return Data[y * Width + x]; }
            set { Data[y * Width + x] = value; }
        }

        /// <summary>
        ///     マップ画素の配列
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        ///     マップ画素単位の幅
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        ///     マップ画素単位の高さ
        /// </summary>
        public int Height { get; private set; }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="width">マップ画素の配列の幅</param>
        /// <param name="height">マップ画素の配列の高さ</param>
        public MapPixels(int width, int height)
        {
            Width = width;
            Height = height;

            Data = new byte[width * height];
        }

        #endregion
    }

    /// <summary>
    ///     マップ断片
    /// </summary>
    public class MapPiece
    {
        #region 公開プロパティ

        /// <summary>
        ///     マップ断片の幅
        /// </summary>
        public const int Width = 117;

        /// <summary>
        ///     マップ断片の高さ
        /// </summary>
        public const int Height = 45;

        #endregion

        #region ファイル書き込み

        /// <summary>
        ///     マップ断片を保存する
        /// </summary>
        /// <param name="pixels">マップ画素の配列</param>
        /// <param name="x">ブロック単位のX座標</param>
        /// <param name="y">ブロック単位のY座標</param>
        /// <param name="folderName">保存先フォルダ名</param>
        public static void Save(MapPixels pixels, int x, int y, string folderName)
        {
            var bitmap = new Bitmap(Width * MapBlock.Width, Height * MapBlock.Width,
                PixelFormat.Format8bppIndexed);

            ColorPalette palette = bitmap.Palette;
            for (int i = 0; i < 256; i++)
            {
                palette.Entries[i] = Color.FromArgb(255 - i, 255 - i, 255 - i);
            }
            bitmap.Palette = palette;

            BitmapData image = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly,
                PixelFormat.Format8bppIndexed);
            int src = (y * pixels.Width + x) * MapBlock.Width;
            int dest = 0;
            for (int i = 0; i < Height * MapBlock.Width; i++)
            {
                Marshal.Copy(pixels.Data, src, (IntPtr) ((Int64) image.Scan0 + dest), image.Width);
                src += pixels.Width;
                dest += image.Width;
            }
            bitmap.UnlockBits(image);

            bitmap.Save(Path.Combine(folderName, string.Format("map_{0}_{1}.colorscales.png", x, y)), ImageFormat.Png);
        }

        #endregion
    }

    /// <summary>
    ///     マップブロック
    /// </summary>
    public class MapBlock
    {
        #region 公開プロパティ

        /// <summary>
        ///     プロヴィンスIDリスト
        /// </summary>
        public ushort[] ProvinceIds { get; set; }

        /// <summary>
        ///     プロヴィンスIDの数
        /// </summary>
        public int ProvinceIdCount { get; set; }

        /// <summary>
        ///     ツリーノード
        /// </summary>
        public MapTreeNode Nodes { get; set; }

        /// <summary>
        ///     ツリーノードの数
        /// </summary>
        public int NodeCount { get; set; }

        /// <summary>
        ///     ツリーノードごとのプロヴィンスID
        /// </summary>
        public byte[] NodeIds { get; set; }

        /// <summary>
        ///     ツリーノードごとのカラーインデックス
        /// </summary>
        public byte[] NodeColors { get; set; }

        #endregion

        #region 公開定数

        /// <summary>
        ///     マップブロックの幅
        /// </summary>
        public const int Width = 32;

        /// <summary>
        ///     マップブロックの高さ
        /// </summary>
        public const int Height = 32;

        #endregion
    }

    /// <summary>
    ///     マップブロックの配列
    /// </summary>
    public class MapBlocks
    {
        #region 公開プロパティ

        /// <summary>
        ///     マップブロックを取得する
        /// </summary>
        /// <param name="index">配列のインデックス</param>
        /// <returns>マップブロック</returns>
        public MapBlock this[int index]
        {
            get { return Data[index]; }
            set { Data[index] = value; }
        }

        /// <summary>
        ///     マップブロックを取得する
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <returns>マップブロック</returns>
        public MapBlock this[int x, int y]
        {
            get { return Data[y * Width + x]; }
            set { Data[y * Width + x] = value; }
        }

        /// <summary>
        ///     マップブロックの配列
        /// </summary>
        public MapBlock[] Data { get; private set; }

        /// <summary>
        ///     マップブロック単位の幅
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        ///     マップブロック単位の高さ
        /// </summary>
        public int Height { get; private set; }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="width">マップブロックの配列の幅</param>
        /// <param name="height">マップブロックの配列の高さ</param>
        public MapBlocks(int width, int height)
        {
            Width = width;
            Height = height;

            Data = new MapBlock[width * height];
        }

        #endregion
    }

    /// <summary>
    ///     マップツリーのノード
    /// </summary>
    public class MapTreeNode
    {
        #region 公開プロパティ

        /// <summary>
        ///     ノード番号
        /// </summary>
        public int No { get; set; }

        /// <summary>
        ///     ノードレベル
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        ///     マップブロック内のX座標
        /// </summary>
        public int X { get; set; }

        /// <summary>
        ///     マップブロック内のY座標
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        ///     右下の子ノード
        /// </summary>
        public MapTreeNode BottomRightChild { get; set; }

        /// <summary>
        ///     左下の子ノード
        /// </summary>
        public MapTreeNode BottomLeftChild { get; set; }

        /// <summary>
        ///     右上の子ノード
        /// </summary>
        public MapTreeNode TopRightChild { get; set; }

        /// <summary>
        ///     左上の子ノード
        /// </summary>
        public MapTreeNode TopLeftChild { get; set; }

        #endregion

        #region 公開定数

        /// <summary>
        ///     最大ノードレベル
        /// </summary>
        public const int MaxLevel = 5;

        #endregion
    }
}