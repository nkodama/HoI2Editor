using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

        /// <summary>
        ///     マップレベル
        /// </summary>
        public MapLevel Level { get; set; }

        /// <summary>
        ///     フィルターモード
        /// </summary>
        public MapFilterMode FilterMode
        {
            get { return _mode; }
            set
            {
                // プロヴィンス単位でマップ画像を更新する
                List<ushort> prev = GetHighlightedProvinces(_mode, _country);
                List<ushort> next = GetHighlightedProvinces(value, _country);
                UpdateProvinces(prev, next);

                // フィルターモードを更新する
                _mode = value;
            }
        }

        /// <summary>
        ///     選択国
        /// </summary>
        public Country SelectedCountry
        {
            get { return _country; }
            set
            {
                // プロヴィンス単位でマップ画像を更新する
                List<ushort> prev = GetHighlightedProvinces(_mode, _country);
                List<ushort> next = GetHighlightedProvinces(_mode, value);
                UpdateProvinces(prev, next);

                // 選択国を更新する
                _country = value;
            }
        }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     マップパネル
        /// </summary>
        private readonly Panel _panel;

        /// <summary>
        ///     マップパネルのピクチャーボックス
        /// </summary>
        private readonly PictureBox _pictureBox;

        /// <summary>
        ///     フィルターモード
        /// </summary>
        private MapFilterMode _mode;

        /// <summary>
        ///     選択国
        /// </summary>
        private Country _country;

        #endregion

        #region 公開定数

        /// <summary>
        ///     マップ表示のフィルターモード
        /// </summary>
        public enum MapFilterMode
        {
            None, // フィルターなし
            Core, // 中核プロヴィンス
            Owned, // 保有プロヴィンス
            Controlled, // 支配プロヴィンス
            Claimed // 領有権主張プロヴィンス
        }

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

        #region 公開イベント

        /// <summary>
        ///     プロヴィンスマウスクリック時のイベント
        /// </summary>
        public event EventHandler<ProvinceEventArgs> ProvinceMouseClick;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="panel">マップパネル</param>
        /// <param name="pictureBox">マップパネルのピクチャーボックス</param>
        public MapPanelController(Panel panel, PictureBox pictureBox)
        {
            _panel = panel;
            _pictureBox = pictureBox;

            Level = MapLevel.Level2;
        }

        #endregion

        #region マップ画像表示

        /// <summary>
        ///     マップ画像を表示する
        /// </summary>
        public void Show()
        {
            // カラーパレットを初期化する
            InitColorPalette();

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
                if (province.IsLand)
                {
                    continue;
                }
                if (province.IsSea)
                {
                    seaList.Add((ushort) province.Id);
                }
                else if (province.IsInvalid)
                {
                    invalidList.Add((ushort) province.Id);
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

            // マウスクリックイベントハンドラを登録する
            _pictureBox.MouseClick += OnPictureBoxMouseClick;
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

        /// <summary>
        ///     指定プロヴィンスが表示されるようにスクロールする
        /// </summary>
        /// <param name="id">プロヴィンスID</param>
        public void ScrollToProvince(int id)
        {
            // 未読み込みならば何もしない
            if (!Maps.IsLoaded[(int) Level])
            {
                return;
            }

            Rectangle rect = Maps.BoundBoxes[id];
            int provLeft = rect.Left >> 1;
            int provTop = rect.Top >> 1;
            int provWidth = rect.Width >> 1;
            int provHeight = rect.Height >> 1;
            int panelX = _panel.HorizontalScroll.Value;
            int panelY = _panel.VerticalScroll.Value;
            int panelWidth = _panel.Width;
            int panelHeight = _panel.Height;

            // 指定プロヴィンスの全体が表示されていれば何もしない
            if ((provLeft >= panelX) &&
                (provTop >= panelY) &&
                (provLeft + provWidth <= panelX + panelWidth) &&
                (provTop + provHeight <= panelY + panelHeight))
            {
                return;
            }

            // 指定プロヴィンスが中央に表示されるようにスクロールする
            int x = provLeft + provWidth / 2 - panelWidth / 2;
            int mapWidth = _pictureBox.Width;
            if (x < 0)
            {
                x = 0;
            }
            else if (x + panelWidth > mapWidth)
            {
                x = mapWidth - panelWidth;
            }
            int y = provTop + provHeight / 2 - panelHeight / 2;
            int mapHeight = _pictureBox.Height;
            if (y < 0)
            {
                y = 0;
            }
            else if (y + panelHeight > mapHeight)
            {
                y = mapHeight - panelHeight;
            }
            _panel.HorizontalScroll.Value = x;
            _panel.VerticalScroll.Value = y;
        }

        /// <summary>
        ///     プロヴィンス単位でマップ画像を更新する
        /// </summary>
        /// <param name="id">更新対象のプロヴィンスID</param>
        /// <param name="highlighted">強調表示の有無</param>
        public void UpdateProvince(ushort id, bool highlighted)
        {
            // 未読み込みならば何もしない
            if (!Maps.IsLoaded[(int) Level])
            {
                return;
            }

            // 対象プロヴィンスのカラーインデックスを変更する
            Maps.SetColorIndex(id, (int) (highlighted ? MapColorIndex.Highlighted : MapColorIndex.Land));

            // プロヴィンス単位でマップ画像を更新する
            Map map = Maps.Data[(int) Level];
            map.UpdateProvince(id);

            // ピクチャーボックスを再描画する
            _pictureBox.Refresh();
        }

        /// <summary>
        ///     プロヴィンス単位でマップ画像を更新する
        /// </summary>
        /// <param name="prev">更新前に強調表示していたプロヴィンス</param>
        /// <param name="next">更新後に強調表示するプロヴィンス</param>
        private void UpdateProvinces(List<ushort> prev, List<ushort> next)
        {
            // 通常表示対象のプロヴィンスリストを取得する
            List<ushort> normal = prev.Where(id => !next.Contains(id)).ToList();

            // 強調表示対象のプロヴィンスリストを取得する
            List<ushort> highlighted = next.Where(id => !prev.Contains(id)).ToList();

            // 通常表示プロヴィンスのカラーインデックスを変更する
            Maps.SetColorIndex(normal, (int) MapColorIndex.Land);

            // 強調表示プロヴィンスのカラーインデックスを変更する
            Maps.SetColorIndex(highlighted, (int) MapColorIndex.Highlighted);

            // プロヴィンス単位でマップ画像を更新する
            Map map = Maps.Data[(int) Level];
            map.UpdateProvinces(normal);
            map.UpdateProvinces(highlighted);

            // ピクチャーボックスを再描画する
            _pictureBox.Refresh();
        }

        /// <summary>
        ///     強調表示するプロヴィンスのリストを取得する
        /// </summary>
        /// <param name="mode">フィルターモード</param>
        /// <param name="country">対象国</param>
        /// <returns>強調表示するプロヴィンスのリスト</returns>
        private static List<ushort> GetHighlightedProvinces(MapFilterMode mode, Country country)
        {
            if (mode == MapFilterMode.None)
            {
                return new List<ushort>();
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);
            if (settings == null)
            {
                return new List<ushort>();
            }

            switch (mode)
            {
                case MapFilterMode.Core:
                    return Provinces.Items.Where(province => settings.NationalProvinces.Contains(province.Id))
                        .Select(province => (ushort) province.Id)
                        .ToList();

                case MapFilterMode.Owned:
                    return Provinces.Items.Where(province => settings.OwnedProvinces.Contains(province.Id))
                        .Select(province => (ushort) province.Id)
                        .ToList();

                case MapFilterMode.Controlled:
                    return Provinces.Items.Where(province => settings.ControlledProvinces.Contains(province.Id))
                        .Select(province => (ushort) province.Id)
                        .ToList();

                case MapFilterMode.Claimed:
                    return Provinces.Items.Where(province => settings.ClaimedProvinces.Contains(province.Id))
                        .Select(province => (ushort) province.Id)
                        .ToList();
            }

            return null;
        }

        #endregion

        #region マウスイベントハンドラ

        /// <summary>
        ///     ピクチャーボックスのマウスクリック時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPictureBoxMouseClick(object sender, MouseEventArgs e)
        {
            Log.Info("Click: ({0},{1})", e.X, e.Y);
            Log.Info("Scroll: ({0},{1})", _panel.HorizontalScroll.Value, _panel.VerticalScroll.Value);
            Log.Info("PictureBox: ({0},{1})", _pictureBox.Width, _pictureBox.Height);

            Map map = Maps.Data[(int) Level];
            ushort id = map.ProvinceIds[e.X, e.Y];
            if (ProvinceMouseClick != null)
            {
                ProvinceMouseClick(sender, new ProvinceEventArgs(id, e));
            }
        }

        #endregion

        #region 内部クラス

        /// <summary>
        ///     プロヴィンスイベントのパラメータ
        /// </summary>
        public class ProvinceEventArgs : EventArgs
        {
            /// <summary>
            ///     プロヴィンスID
            /// </summary>
            public int Id { get; private set; }

            /// <summary>
            ///     マウスイベントのパラメータ
            /// </summary>
            public MouseEventArgs MouseEvent;

            /// <summary>
            ///     コンストラクタ
            /// </summary>
            /// <param name="id">プロヴィンスID</param>
            /// <param name="e">マウスイベントのパラメータ</param>
            public ProvinceEventArgs(int id, MouseEventArgs e)
            {
                Id = id;
                MouseEvent = e;
            }
        }

        #endregion
    }
}