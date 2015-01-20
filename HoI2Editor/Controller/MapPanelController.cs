using System.Collections.Generic;
using System.Windows.Forms;
using HoI2Editor.Models;

namespace HoI2Editor.Controller
{
    /// <summary>
    ///     マップパネルのコントローラクラス
    /// </summary>
    public class MapPanelController
    {
        #region 公開プロパティ

        public MapLevel Level { get; set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     マップパネルのピクチャーボックス
        /// </summary>
        private readonly PictureBox _pictureBox;

        #endregion

        #region 内部定数

        /// <summary>
        ///     カラーインデックス
        /// </summary>
        private enum MapColorIndex
        {
            Land, // 陸地
            Sea, // 海洋
            Highlighted, // 強調表示
            Invalid // 無効
        }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="pictureBox">マップパネルのピクチャーボックス</param>
        public MapPanelController(PictureBox pictureBox)
        {
            _pictureBox = pictureBox;

            Level = MapLevel.Level2;
            ;

            InitColorPalette();
        }

        /// <summary>
        ///     カラーパレットを初期化する
        /// </summary>
        private static void InitColorPalette()
        {
            Maps.SetColorPalette((int) MapColorIndex.Land, "orange");
            Maps.SetColorPalette((int) MapColorIndex.Sea, "water");
            Maps.SetColorPalette((int) MapColorIndex.Highlighted, "green");
            Maps.SetColorPalette((int) MapColorIndex.Invalid, "black");
        }

        #endregion

        /// <summary>
        ///     マップ画像を表示する
        /// </summary>
        public void ShowMapImage()
        {
            // 海洋/無効プロヴィンスリストを生成する
            List<ushort> seaList = new List<ushort>();
            List<ushort> invalidList = new List<ushort>();
            int maxId = Maps.BoundBoxes.Length;
            foreach (Province province in Provinces.Items)
            {
                if ((province.Id <= 0) || (province.Id >= maxId))
                {
                    continue;
                }
                switch (province.Terrain)
                {
                    case TerrainId.Ocean:
                    case TerrainId.River:
                        // 海洋
                        seaList.Add((ushort) province.Id);
                        Log.Info("Sea: {0}", province.Id);
                        break;

                    case TerrainId.TerraIncognito:
                    case TerrainId.Unknown:
                        // 無効
                        invalidList.Add((ushort) province.Id);
                        Log.Info("Invalid: {0}", province.Id);
                        break;
                }
            }

            // 海洋プロヴィンスのカラーインデックスを変更する
            Maps.SetColorIndex(seaList, (int) MapColorIndex.Sea);

            // 無効プロヴィンスのカラーインデックスを変更する
            Maps.SetColorIndex(invalidList, (int) MapColorIndex.Invalid);

            // プロヴィンス単位でマップ画像を更新する
            Map map = Maps.Data[(int) Level];
            map.UpdateProvinces(seaList);
            map.UpdateProvinces(invalidList);

            // カラーパレットを更新する
            map.UpdateColorPalette();

            // ピクチャーボックスに画像を設定する
            _pictureBox.Image = map.Image;
        }
    }
}