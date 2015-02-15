using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     マップデータ群
    /// </summary>
    public static class Maps
    {
        #region 公開プロパティ

        /// <summary>
        ///     マップデータ
        /// </summary>
        public static Map[] Data;

        /// <summary>
        ///     プロヴィンス境界の配列
        /// </summary>
        public static Rectangle[] BoundBoxes { get; private set; }

        /// <summary>
        ///     カラースケールテーブル
        /// </summary>
        public static Dictionary<string, Color[]> ColorScales { get; private set; }

        /// <summary>
        ///     カラーパレット
        /// </summary>
        public static Color[] ColorPalette { get; private set; }

        /// <summary>
        ///     カラーマスクの配列
        /// </summary>
        public static byte[] ColorMasks { get; private set; }

        /// <summary>
        ///     読み込み済みフラグ
        /// </summary>
        public static bool[] IsLoaded;

        #endregion

        #region 公開定数

        /// <summary>
        ///     最大プロヴィンス数
        /// </summary>
        public const int MaxProvinces = 10000;

        #endregion

        #region 内部定数

        /// <summary>
        ///     カラーインデックスの最大数
        /// </summary>
        private const int MaxColorIndex = 4;

        /// <summary>
        ///     カラースケールの数
        /// </summary>
        private const int ColorScaleCount = 64;

        #endregion

        #region 初期化

        /// <summary>
        ///     静的コンストラクタ
        /// </summary>
        static Maps()
        {
            Data = new Map[Enum.GetNames(typeof (MapLevel)).Length];
            ColorMasks = new byte[MaxProvinces];
            IsLoaded = new bool[Enum.GetNames(typeof (MapLevel)).Length];

            InitColorPalette();
        }

        #endregion

        #region ファイル読み込み

        /// <summary>
        ///     マップファイルを読み込む
        /// </summary>
        /// <param name="level">マップレベル</param>
        public static void Load(MapLevel level)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Map map = new Map(level);

            // マップデータを読み込む
            map.Load();
            Data[(int) level] = map;

            // プロヴィンス境界定義ファイルを読み込む
            if (BoundBoxes == null)
            {
                LoadBoundBox();
            }

            // カラースケールテーブルを読み込む
            if (ColorScales == null)
            {
                LoadColorScales();
            }

            IsLoaded[(int) level] = true;

            sw.Stop();
            Log.Verbose("[Map] Load: {0} {1}ms", map.Level, sw.ElapsedMilliseconds);
        }

        /// <summary>
        ///     プロヴィンス境界定義ファイルを読み込む
        /// </summary>
        private static void LoadBoundBox()
        {
            // プロヴィンス境界データをメモリへ展開する
            string fileName = Game.GetReadFileName(Game.GetMapFolderName(), Game.BoundBoxFileName);
            byte[] data;
            int count;
            using (FileStream reader = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                int len = (int) reader.Length;
                data = new byte[len];
                count = len / 16;
                reader.Read(data, 0, len);
                reader.Close();
            }
            int index = 0;

            // プロヴィンス境界を順に読み込む
            BoundBoxes = new Rectangle[count];
            for (int i = 0; i < count; i++)
            {
                int left = data[index] | (data[index + 1] << 8);
                BoundBoxes[i].X = left;
                BoundBoxes[i].Width = (data[index + 8] | (data[index + 9] << 8)) - left + 1;
                int top = data[index + 4] | (data[index + 5] << 8);
                BoundBoxes[i].Y = top;
                BoundBoxes[i].Height = (data[index + 12] | (data[index + 13] << 8)) - top + 1;
                index += 16;
            }
        }

        /// <summary>
        ///     カラースケールテーブルを読み込む
        /// </summary>
        private static void LoadColorScales()
        {
            string fileName = Game.GetReadFileName(Game.GetMapFolderName(), Game.ColorScalesFileName);
            using (StreamReader reader = new StreamReader(fileName))
            {
                ColorScales = new Dictionary<string, Color[]>();
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (String.IsNullOrEmpty(line))
                    {
                        continue;
                    }
                    string[] tokens = line.Split(';');
                    if (tokens.Length == 0)
                    {
                        continue;
                    }
                    string name = tokens[0].ToLower().Trim('\"');
                    reader.ReadLine();
                    int[][] colors = new int[4][];
                    line = reader.ReadLine();
                    colors[0] = ParseColor(line);
                    if (colors[0] == null)
                    {
                        continue;
                    }
                    line = reader.ReadLine();
                    colors[1] = ParseColor(line);
                    if (colors[1] == null)
                    {
                        continue;
                    }
                    line = reader.ReadLine();
                    colors[2] = ParseColor(line);
                    if (colors[2] == null)
                    {
                        continue;
                    }
                    line = reader.ReadLine();
                    colors[3] = ParseColor(line);
                    if (colors[3] == null)
                    {
                        continue;
                    }
                    Color[] colorScale = GetColorScale(colors);
                    ColorScales.Add(name, colorScale);
                }
            }
        }

        /// <summary>
        ///     色定義を解析する
        /// </summary>
        /// <param name="s">解析対象の文字列</param>
        /// <returns>色定義</returns>
        private static int[] ParseColor(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return null;
            }
            string[] tokens = s.Split(';');
            if (tokens.Length < 4)
            {
                return null;
            }
            int[] color = new int[4];
            for (int i = 0; i < 4; i++)
            {
                if (!int.TryParse(tokens[i], out color[i]))
                {
                    return null;
                }
                if (color[i] > 255)
                {
                    color[i] = 255;
                }
            }
            return color;
        }

        /// <summary>
        ///     カラースケールを取得する
        /// </summary>
        /// <param name="colors">色定義の配列</param>
        /// <returns>カラースケール</returns>
        private static Color[] GetColorScale(int[][] colors)
        {
            Color[] colorScale = new Color[64];
            int width = colors[1][3] - colors[0][3];
            int deltaR = (width > 0) ? ((colors[1][0] - colors[0][0]) << 10) / width : 0;
            int deltaG = (width > 0) ? ((colors[1][1] - colors[0][1]) << 10) / width : 0;
            int deltaB = (width > 0) ? ((colors[1][2] - colors[0][2]) << 10) / width : 0;
            int r = colors[0][0] << 10;
            int g = colors[0][1] << 10;
            int b = colors[0][2] << 10;
            for (int i = colors[0][3]; i < colors[1][3]; i++)
            {
                colorScale[i] = Color.FromArgb(r >> 10, g >> 10, b >> 10);
                r += deltaR;
                g += deltaG;
                b += deltaB;
            }
            width = colors[2][3] - colors[1][3];
            deltaR = (width > 0) ? ((colors[2][0] - colors[1][0]) << 10) / width : 0;
            deltaG = (width > 0) ? ((colors[2][1] - colors[1][1]) << 10) / width : 0;
            deltaB = (width > 0) ? ((colors[2][2] - colors[1][2]) << 10) / width : 0;
            r = colors[1][0] << 10;
            g = colors[1][1] << 10;
            b = colors[1][2] << 10;
            for (int i = colors[1][3]; i < colors[2][3]; i++)
            {
                colorScale[i] = Color.FromArgb(r >> 10, g >> 10, b >> 10);
                r += deltaR;
                g += deltaG;
                b += deltaB;
            }
            width = colors[3][3] - colors[2][3];
            deltaR = (width > 0) ? ((colors[3][0] - colors[2][0]) << 10) / width : 0;
            deltaG = (width > 0) ? ((colors[3][1] - colors[2][1]) << 10) / width : 0;
            deltaB = (width > 0) ? ((colors[3][2] - colors[2][2]) << 10) / width : 0;
            r = colors[2][0] << 10;
            g = colors[2][1] << 10;
            b = colors[2][2] << 10;
            for (int i = colors[2][3]; i < colors[3][3]; i++)
            {
                colorScale[i] = Color.FromArgb(r >> 10, g >> 10, b >> 10);
                r += deltaR;
                g += deltaG;
                b += deltaB;
            }
            return colorScale;
        }

        #endregion

        #region カラースケール

        /// <summary>
        ///     プロヴィンスのカラーインデックスを取得する
        /// </summary>
        /// <param name="id">プロヴィンスID</param>
        /// <returns>カラーインデックス</returns>
        public static int GetColorIndex(ushort id)
        {
            return ColorMasks[id] >> 6;
        }

        /// <summary>
        ///     プロヴィンスのカラーインデックスを設定する
        /// </summary>
        /// <param name="ids">プロヴィンスIDの配列</param>
        /// <param name="index">カラーインデックス</param>
        public static void SetColorIndex(IEnumerable<ushort> ids, int index)
        {
            foreach (ushort id in ids)
            {
                ColorMasks[id] = (byte) (index << 6);
            }
        }

        /// <summary>
        ///     プロヴィンスのカラーインデックスを設定する
        /// </summary>
        /// <param name="id">プロヴィンスID</param>
        /// <param name="index">カラーインデックス</param>
        public static void SetColorIndex(ushort id, int index)
        {
            ColorMasks[id] = (byte) (index << 6);
        }

        /// <summary>
        ///     カラースケールを設定する
        /// </summary>
        /// <param name="index">カラーインデックス</param>
        /// <param name="color">カラースケール名</param>
        public static void SetColorPalette(int index, string color)
        {
            // 存在しない色名ならば何もしない
            if (!ColorScales.ContainsKey(color.ToLower()))
            {
                return;
            }

            Color[] colorScale = ColorScales[color];
            for (int i = 0; i < ColorScaleCount; i++)
            {
                ColorPalette[index * ColorScaleCount + i] = colorScale[i];
            }
        }

        /// <summary>
        ///     カラーパレットを初期化する
        /// </summary>
        private static void InitColorPalette()
        {
            ColorPalette = new Color[ColorScaleCount * MaxColorIndex];
            for (int i = 0; i < MaxColorIndex; i++)
            {
                for (int j = 0; j < ColorScaleCount; j++)
                {
                    ColorPalette[i * ColorScaleCount + j] = Color.FromArgb(255 - j * 4, 255 - j * 4, 255 - j * 4);
                }
            }
        }

        #endregion
    }

    /// <summary>
    ///     マップレベル
    /// </summary>
    public enum MapLevel
    {
        Level1, // 936x360
        Level2, // 468x180
        Level3, // 234x90
        Level4 // 117x45
    }
}