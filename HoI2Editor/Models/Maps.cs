using System;
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

        #endregion

        #region 初期化

        /// <summary>
        ///     静的コンストラクタ
        /// </summary>
        static Maps()
        {
            Data = new Map[Enum.GetNames(typeof (MapLevel)).Length];
        }

        #endregion

        #region ファイル読み込み

        /// <summary>
        ///     マップファイルを読み込む
        /// </summary>
        /// <param name="level">マップレベル</param>
        public static void Load(MapLevel level)
        {
            var sw = new Stopwatch();
            sw.Start();

            var map = new Map(level);

            // マップデータを読み込む
            map.Load();
            Data[(int) level] = map;

            // プロヴィンス境界定義ファイルを読み込む
            if (BoundBoxes == null)
            {
                LoadBoundBox();
            }

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
            using (var reader = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                var len = (int) reader.Length;
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