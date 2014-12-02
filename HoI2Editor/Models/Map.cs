using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using HoI2Editor.Parsers;

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
        ///     プロヴィンスIDの配列
        /// </summary>
        public static MapProvinceIds Ids { get; private set; }

        /// <summary>
        ///     マップブロックの配列
        /// </summary>
        public static MapBlocks Blocks { get; private set; }

        /// <summary>
        ///     プロヴィンス境界の配列
        /// </summary>
        public static Rectangle[] BoundBoxes { get; private set; }

        /// <summary>
        ///     カラースケールテーブル
        /// </summary>
        public static Dictionary<string, ColorPalette> ColorScales { get; private set; }

        #endregion

        #region 内部フィールド

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
        private static byte[] _pics;

        /// <summary>
        ///     プロヴィンスID展開用バッファ
        /// </summary>
        private static ushort[] _provs;

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
        ///     マップブロック単位の幅
        /// </summary>
        private const int BlockWidth = 936;

        /// <summary>
        ///     マップブロック単位の高さ
        /// </summary>
        private const int BlockHeight = 360;

        /// <summary>
        ///     マップ断片の幅
        /// </summary>
        private const int PieceWidth = 117;

        /// <summary>
        ///     マップ断片の高さ
        /// </summary>
        private const int PieceHeight = 45;

        /// <summary>
        ///     最大プロヴィンス数
        /// </summary>
        private const int MaxProvinces = 10000;

        /// <summary>
        ///     マップブロック内のプロヴィンスの最大数
        /// </summary>
        private const int MaxBlockProvinces = 256;

        /// <summary>
        ///     カラースケールテーブルの要素数
        /// </summary>
        private const int MaxColorScales = 64;

        #endregion

        #region ファイル読み込み

        /// <summary>
        ///     マップファイルを読み込む
        /// </summary>
        public static void Load()
        {
            Width = BlockWidth;
            Height = BlockHeight;

            var sw = new Stopwatch();
            sw.Start();

            // マップデータを読み込む
            LoadLightMap();

            // プロヴィンス境界定義ファイルを読み込む
            LoadBoundBox();

            // カラースケールテーブルを読み込む
            LoadColorScales();

            sw.Stop();
            Log.Info("[Map] Load: {0}ms", sw.ElapsedMilliseconds);
        }

        /// <summary>
        ///     マップデータを読み込む
        /// </summary>
        private static void LoadLightMap()
        {
            // マップデータをメモリへ展開する
            LoadFile();

            // マップブロックのオフセット位置を読み込む
            int count = Width * Height;
            LoadOffsets(count);

            int offset = (count + 1) * 4;
            Blocks = new MapBlocks(Width, Height);
            _stack = new MapTreeNode[MapBlock.Width * MapBlock.Height * 2];

            // マップブロックを順に読み込む
            for (int i = 0; i < count; i++)
            {
                _index = offset + (int) _offsets[i];
                Blocks[i] = LoadBlock();
            }

            // 使用済みのバッファを解放する
            _data = null;
            _offsets = null;
            _stack = null;
        }

        /// <summary>
        ///     マップデータをメモリへ展開する
        /// </summary>
        private static void LoadFile()
        {
            string fileName = Game.GetReadFileName(Game.GetMapFolderName(), Game.LightMapFileName);
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
            var ids = new ushort[MaxBlockProvinces];
            int no = 0;

            ushort work;
            do
            {
                work = _data[_index++];
                work |= (ushort) (_data[_index++] << 8);
                ids[no++] = ((work & 0x4000) == 0) ? (ushort) (work & 0x7FFF) : ids[((work & 0x1E00) >> 9) - 4];
            } while ((work & 0x8000) == 0);

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
                    // プロヴィンス数が1の場合は省略
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

        /// <summary>
        ///     プロヴィンス境界定義ファイルを読み込む
        /// </summary>
        private static void LoadBoundBox()
        {
            // プロヴィンス境界データをメモリへ展開する
            string fileName = Game.GetReadFileName(Game.GetMapFolderName(), Game.LightMapFileName);
            int count;
            using (var reader = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                var len = (int) reader.Length;
                _data = new byte[len];
                count = len / 16;
                reader.Read(_data, 0, len);
                reader.Close();
            }
            _index = 0;

            if (count > MaxProvinces)
            {
                count = MaxProvinces;
            }
            BoundBoxes = new Rectangle[count];

            // プロヴィンス境界を順に読み込む
            for (int i = 0; i < count; i++)
            {
                int left = _data[_index] | (_data[_index + 1] << 8);
                BoundBoxes[i].X = left;
                BoundBoxes[i].Width = (_data[_index + 8] | (_data[_index + 9] << 8)) - left + 1;
                int top = _data[_index + 4] | (_data[_index + 5] << 8);
                BoundBoxes[i].Y = top;
                BoundBoxes[i].Height = (_data[_index + 12] | (_data[_index + 13] << 8)) - top + 1;
                _index += 16;
            }
        }

        /// <summary>
        ///     カラースケールテーブルを読み込む
        /// </summary>
        private static void LoadColorScales()
        {
            string fileName = Game.GetReadFileName(Game.GetMapFolderName(), Game.ColorScalesFileName);
            using (var lexer = new CsvLexer(fileName))
            {
                ColorScales = new Dictionary<string, ColorPalette>();

                // パレット作成用のダミービットマップ
                var bitmap = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);

                while (!lexer.EndOfStream)
                {
                    string[] tokens = lexer.GetTokens();

                    // カラースケール名が空ならば読み飛ばす
                    if (tokens == null || string.IsNullOrEmpty(tokens[0]))
                    {
                        lexer.SkipLine();
                        lexer.SkipLine();
                        lexer.SkipLine();
                        lexer.SkipLine();
                        lexer.SkipLine();
                        continue;
                    }
                    string name = tokens[0].ToLower();

                    // ヘッダ行を読み飛ばす
                    lexer.SkipLine();

                    // 色定義を読み込む
                    int index1;
                    Color color1 = ParseColorScale(lexer, out index1);
                    int index2;
                    Color color2 = ParseColorScale(lexer, out index2);
                    int index3;
                    Color color3 = ParseColorScale(lexer, out index3);
                    int index4;
                    Color color4 = ParseColorScale(lexer, out index4);
                    if (color1 == Color.Empty || color2 == Color.Empty || color3 == Color.Empty || color4 == Color.Empty)
                    {
                        continue;
                    }

                    // カラーパレットに値を設定する
                    ColorPalette palette = bitmap.Palette;
                    SetColorScales(palette, color1, color2, index1, index2);
                    SetColorScales(palette, color2, color3, index2, index3);
                    SetColorScales(palette, color3, color4, index3, index4);
                    palette.Entries[255] = Color.Lime;

                    // カラースケールテーブルに値を追加する
                    ColorScales.Add(name, palette);
                }
            }
        }

        /// <summary>
        ///     カラースケールを解釈する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <param name="index">カラーインデックス</param>
        /// <returns>カラースケール</returns>
        private static Color ParseColorScale(CsvLexer lexer, out int index)
        {
            string[] tokens = lexer.GetTokens();
            if (tokens == null || tokens.Length != 4)
            {
                index = 0;
                return Color.Empty;
            }

            int r;
            if (!int.TryParse(tokens[0], out r))
            {
                index = 0;
                return Color.Empty;
            }

            int g;
            if (!int.TryParse(tokens[1], out g))
            {
                index = 0;
                return Color.Empty;
            }

            int b;
            if (!int.TryParse(tokens[2], out b))
            {
                index = 0;
                return Color.Empty;
            }

            if (!int.TryParse(tokens[3], out index))
            {
                return Color.Empty;
            }

            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        ///     カラーパレットに値を設定する
        /// </summary>
        /// <param name="palette">カラーパレットに</param>
        /// <param name="color1">開始位置の色</param>
        /// <param name="color2">終了位置の色</param>
        /// <param name="index1">開始位置のインデックス</param>
        /// <param name="index2">終了位置のインデックス</param>
        private static void SetColorScales(ColorPalette palette, Color color1, Color color2, int index1, int index2)
        {
            int r = color1.R << 16;
            int g = color1.G << 16;
            int b = color1.B << 16;
            int width = index2 - index1;
            int stepR = ((color2.R - color1.R) << 16) / width;
            int stepG = ((color2.G - color1.G) << 16) / width;
            int stepB = ((color2.B - color1.B) << 16) / width;
            for (int i = index1; i < index2; i++)
            {
                palette.Entries[i * 4] = Color.FromArgb(r >> 16, g >> 16, b >> 16);
                r += stepR;
                g += stepG;
                b += stepB;
            }
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

            int hCount = Width / PieceWidth;
            int vCount = Height / PieceHeight;

            for (int i = 0; i < vCount; i++)
            {
                for (int j = 0; j < hCount; j++)
                {
                    SavePiece(Pixels, j * PieceWidth, i * PieceHeight, folderName);
                }
            }

            sw.Stop();
            Log.Info("[Map] Save: {0}ms", sw.ElapsedMilliseconds);
        }

        /// <summary>
        ///     マップ断片を保存する
        /// </summary>
        /// <param name="pixels">マップ画素の配列</param>
        /// <param name="x">ブロック単位のX座標</param>
        /// <param name="y">ブロック単位のY座標</param>
        /// <param name="folderName">保存先フォルダ名</param>
        public static void SavePiece(MapPixels pixels, int x, int y, string folderName)
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

        #region マップデータ展開

        /// <summary>
        ///     マップデータを展開する
        /// </summary>
        public static void Decode()
        {
            var sw = new Stopwatch();
            sw.Start();

            // マップ画像を展開する
            DecodePixels();

            // プロヴィンスIDの配列を展開する
            ExtractIds();

            sw.Stop();
            Log.Info("[Map] Decode: {0}ms", sw.ElapsedMilliseconds);
        }

        /// <summary>
        ///     マップ画像を展開する
        /// </summary>
        public static void DecodePixels()
        {
            var bitmap = new Bitmap(Width * MapBlock.Width, Height * MapBlock.Height, PixelFormat.Format8bppIndexed);

            // グレースケールのパレットを準備する
            ColorPalette palette = bitmap.Palette;
            for (int i = 0; i < 64; i++)
            {
                palette.Entries[i] = Color.FromArgb(255 - i * 4, 255 - i * 4, 255 - i * 4);
            }
            bitmap.Palette = palette;

            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly,
                PixelFormat.Format8bppIndexed);

            Pixels = new MapPixels(Width * MapBlock.Width, Height * MapBlock.Height);
            _pics = new byte[(MapBlock.Width + 1) * (MapBlock.Height + 1)];

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

            Marshal.Copy(Pixels.Data, 0, data.Scan0, Pixels.Data.Length);
            bitmap.UnlockBits(data);

            Image old = Image;
            Image = bitmap;
            if (old != null)
            {
                old.Dispose();
            }

            // 使用済みのバッファを解放する
            _pics = null;
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
                _pics, MapBlock.Height * (MapBlock.Width + 1), MapBlock.Width);

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
                _pics[pos] = Pixels[index];
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
            _pics[MapBlock.Height * (MapBlock.Width + 1) + MapBlock.Width] = (byte) (_block.NodeColors[node.No] << 2);
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
                _pics[pos] = (byte) (_block.NodeColors[node.No] << 2);
                return;
            }
            int height = 1 << node.Level;
            int top = _block.NodeColors[node.No] << 10;
            int bottom = _pics[pos + height * (MapBlock.Width + 1)] << 8;
            int delta = (bottom - top) >> node.Level;
            int color = top;
            for (int i = 0; i < height; i++)
            {
                _pics[pos] = (byte) (color >> 8);
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
                _pics[pos] = (byte) (_block.NodeColors[node.No] << 2);
                return;
            }
            int width = 1 << node.Level;
            int left = _block.NodeColors[node.No] << 10;
            int right = _pics[pos + width] << 8;
            int delta = (right - left) >> node.Level;
            int color = left;
            for (int i = 0; i < width; i++)
            {
                _pics[pos++] = (byte) (color >> 8);
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
                _pics[pos] = (byte) (_block.NodeColors[node.No] << 2);
                return;
            }
            int width = 1 << node.Level;
            int step = MapBlock.Width + 1 - width;
            int top = _block.NodeColors[node.No] << 10;
            int bottom = _pics[pos + width * (MapBlock.Width + 1)] << 8;
            int deltaY = (bottom - top) >> node.Level;
            int left = top;
            for (int i = 0; i < width; i++)
            {
                int right = _pics[pos + width] << 8;
                int deltaX = (right - left) >> node.Level;
                int color = left;
                for (int j = 0; j < width; j++)
                {
                    _pics[pos++] = (byte) (color >> 8);
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
                Buffer.BlockCopy(_pics, pos, Pixels.Data, index, MapBlock.Width);
                pos += MapBlock.Width + 1;
                index += step;
            }
        }

        /// <summary>
        ///     プロヴィンスIDの配列を展開する
        /// </summary>
        private static void ExtractIds()
        {
            Ids = new MapProvinceIds(Width * MapBlock.Width, Height * MapBlock.Height);
            _provs = new ushort[MaxProvinces];

            for (int i = 0; i < Blocks.Height; i++)
            {
                for (int j = 0; j < Blocks.Width; j++)
                {
                    ExtractBlock(j, i);
                }
            }

            // 使用済みのバッファを解放する
            _provs = null;
        }

        /// <summary>
        ///     マップブロックのプロヴィンスIDを展開する
        /// </summary>
        /// <param name="x">マップブロック単位のX座標</param>
        /// <param name="y">マップブロック単位のY座標</param>
        private static void ExtractBlock(int x, int y)
        {
            _block = Blocks[x, y];

            // プロヴィンスID展開用バッファを準備する
            for (int i = 0; i < _block.NodeCount; i++)
            {
                _provs[i] = _block.ProvinceIds[_block.NodeIds[i]];
            }

            // ツリーをデコードする
            _base = y * MapBlock.Height * Pixels.Width + x * MapBlock.Width;
            VisitTree(_block.Nodes, FillNode);
        }

        /// <summary>
        ///     ノードをプロヴィンスIDの配列に展開する
        /// </summary>
        /// <param name="node">対象ノード</param>
        private static void FillNode(MapTreeNode node)
        {
            int pos = _base + node.Y * Ids.Width + node.X;
            ushort[] ids = Ids.Data;
            ushort id = _provs[node.No];
            int width = 1 << node.Level;
            int step = Ids.Width - width + 1;
            switch (node.Level)
            {
                case 0:
                    ids[pos] = id;
                    break;

                case 1:
                    ids[pos++] = id;
                    ids[pos] = id;
                    pos += step;
                    ids[pos++] = id;
                    ids[pos] = id;
                    break;

                case 2:
                    ids[pos++] = id;
                    ids[pos++] = id;
                    ids[pos++] = id;
                    ids[pos] = id;
                    pos += step;
                    ids[pos++] = id;
                    ids[pos++] = id;
                    ids[pos++] = id;
                    ids[pos] = id;
                    pos += step;
                    ids[pos++] = id;
                    ids[pos++] = id;
                    ids[pos++] = id;
                    ids[pos] = id;
                    pos += step;
                    ids[pos++] = id;
                    ids[pos++] = id;
                    ids[pos++] = id;
                    ids[pos] = id;
                    break;

                case 3:
                    for (int i = 0; i < width; i++)
                    {
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos] = id;
                        pos += step;
                    }
                    break;

                case 4:
                    for (int i = 0; i < width; i++)
                    {
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos] = id;
                        pos += step;
                    }
                    break;

                case 5:
                    for (int i = 0; i < width; i++)
                    {
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos++] = id;
                        ids[pos] = id;
                        pos += step;
                    }
                    break;
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

    /// <summary>
    ///     プロヴィンスIDの配列
    /// </summary>
    public class MapProvinceIds
    {
        #region 公開プロパティ

        /// <summary>
        ///     プロヴィンスIDを取得する
        /// </summary>
        /// <param name="index">配列のインデックス</param>
        /// <returns>プロヴィンスID</returns>
        public ushort this[int index]
        {
            get { return Data[index]; }
            set { Data[index] = value; }
        }

        /// <summary>
        ///     プロヴィンスIDを取得する
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <returns>プロヴィンスID</returns>
        public ushort this[int x, int y]
        {
            get { return Data[y * Width + x]; }
            set { Data[y * Width + x] = value; }
        }

        /// <summary>
        ///     プロヴィンスIDの配列
        /// </summary>
        public ushort[] Data { get; private set; }

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
        public MapProvinceIds(int width, int height)
        {
            Width = width;
            Height = height;

            Data = new ushort[width * height];
        }

        #endregion
    }
}