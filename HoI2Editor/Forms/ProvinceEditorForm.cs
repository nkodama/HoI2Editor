﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using HoI2Editor.Models;
using HoI2Editor.Properties;

namespace HoI2Editor.Forms
{
    /// <summary>
    ///     プロヴィンスエディタのフォーム
    /// </summary>
    public partial class ProvinceEditorForm : Form
    {
        #region 内部フィールド

        /// <summary>
        ///     絞り込み後のプロヴィンスリスト
        /// </summary>
        private readonly List<Province> _list = new List<Province>();

        /// <summary>
        ///     世界全体ノード
        /// </summary>
        private readonly TreeNode _worldNode = new TreeNode {Text = Resources.World};

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public ProvinceEditorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProvinceEditorFormLoad(object sender, EventArgs e)
        {
            // プロヴィンスデータを初期化する
            Provinces.Init();

            // 編集項目を初期化する
            InitEditableItems();

            // プロヴィンスファイルを読み込む
            LoadFiles();
        }

        /// <summary>
        ///     閉じるボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloseButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region プロヴィンスデータ処理

        /// <summary>
        ///     再読み込みボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReloadButtonClick(object sender, EventArgs e)
        {
            // 文字列定義ファイルの再読み込みを要求する
            Config.RequireReload();

            // プロヴィンスファイルの再読み込みを要求する
            Provinces.RequireReload();

            // プロヴィンスファイルを読み込む
            LoadFiles();
        }

        /// <summary>
        ///     保存ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            SaveFiles();
        }

        /// <summary>
        ///     プロヴィンスファイルを読み込む
        /// </summary>
        private void LoadFiles()
        {
            // ゲーム設定ファイルを読み込む
            Misc.Load();

            // 文字列定義ファイルを読み込む
            Config.Load();

            // プロヴィンスファイルを読み込む
            Provinces.Load();

            // 海域の編集項目を更新する
            UpdateSeaZoneItems();

            // ワールドツリービューの表示を更新する
            UpdateWorldTree();
        }

        /// <summary>
        ///     プロヴィンスファイルを保存する
        /// </summary>
        private void SaveFiles()
        {
            // 文字列定義ファイルを保存する
            Config.Save();

            // プロヴィンスファイルを保存する
            Provinces.Save();

            // 文字列定義のみ保存の場合、プロヴィンス名の編集済みフラグがクリアされないためここで全クリアする
            foreach (Province province in Provinces.Items)
            {
                province.ResetDirtyAll();
            }

            // 編集項目を更新する
            UpdateEditableItems();
        }

        #endregion

        #region ワールドツリービュー

        /// <summary>
        ///     ワールドツリーの表示を更新する
        /// </summary>
        private void UpdateWorldTree()
        {
            worldTreeView.BeginUpdate();
            worldTreeView.Nodes.Clear();

            // 世界全体ノードを追加する
            worldTreeView.Nodes.Add(_worldNode);

            // 大陸ノードを順に追加する
            foreach (ContinentId continent in Enum.GetValues(typeof (ContinentId)))
            {
                AddContinentTreeItem(continent, _worldNode);
            }

            // 世界全体ノードを展開する
            _worldNode.Expand();

            // 世界全体ノードを選択する
            worldTreeView.SelectedNode = _worldNode;

            worldTreeView.EndUpdate();
        }

        /// <summary>
        ///     ワールドツリービューの選択ノード変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWorldTreeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            // プロヴィンスリストを絞り込む
            NarrowProvinceList();

            // プロヴィンスリストの表示を更新する
            UpdateProvinceList();
        }

        /// <summary>
        ///     大陸ノードを追加する
        /// </summary>
        /// <param name="continent">大陸</param>
        /// <param name="parent">親ノード</param>
        private void AddContinentTreeItem(ContinentId continent, TreeNode parent)
        {
            // 大陸ノードを追加する
            var node = new TreeNode {Text = Provinces.GetContinentName(continent), Tag = continent};
            parent.Nodes.Add(node);

            // 地方ノードを順に追加する
            if (Provinces.ContinentRegionMap.ContainsKey(continent))
            {
                foreach (RegionId region in Provinces.ContinentRegionMap[continent])
                {
                    AddRegionTreeItem(region, node);
                }
            }
        }

        /// <summary>
        ///     地方ノードを追加する
        /// </summary>
        /// <param name="region">地方</param>
        /// <param name="parent">親ノード</param>
        private void AddRegionTreeItem(RegionId region, TreeNode parent)
        {
            // 地方ノードを追加する
            var node = new TreeNode {Text = Provinces.GetRegionName(region), Tag = region};
            parent.Nodes.Add(node);

            // 地域ノードを順に追加する
            if (Provinces.RegionAreaMap.ContainsKey(region))
            {
                foreach (AreaId area in Provinces.RegionAreaMap[region])
                {
                    AddAreaTreeItem(area, node);
                }
            }
        }

        /// <summary>
        ///     地域ノードを追加する
        /// </summary>
        /// <param name="area">地域</param>
        /// <param name="parent">親ノード</param>
        private void AddAreaTreeItem(AreaId area, TreeNode parent)
        {
            // 地域ノードを追加する
            var node = new TreeNode {Text = Provinces.GetAreaName(area), Tag = area};
            parent.Nodes.Add(node);
        }

        #endregion

        #region プロヴィンスリストビュー

        /// <summary>
        ///     プロヴィンスリストの表示を更新する
        /// </summary>
        private void UpdateProvinceList()
        {
            provinceListView.BeginUpdate();
            provinceListView.Items.Clear();

            // 項目を順に登録する
            foreach (Province province in _list)
            {
                provinceListView.Items.Add(CreateProvinceListViewItem(province));
            }

            if (provinceListView.Items.Count > 0)
            {
                // 先頭の項目を選択する
                provinceListView.Items[0].Focused = true;
                provinceListView.Items[0].Selected = true;

                // 編集項目を有効化する
                EnableEditableItems();
            }
            else
            {
                // 編集項目を無効化する
                DisableEditableItems();
            }

            provinceListView.EndUpdate();
        }

        /// <summary>
        ///     プロヴィンスリストを絞り込む
        /// </summary>
        private void NarrowProvinceList()
        {
            _list.Clear();

            TreeNode node = worldTreeView.SelectedNode;

            // 世界全体
            if (node.Tag == null)
            {
                _list.AddRange(Provinces.Items);
                return;
            }

            // 大陸
            if (node.Tag is ContinentId)
            {
                var continent = (ContinentId) node.Tag;
                _list.AddRange(Provinces.Items.Where(province => province.Continent == continent));
                return;
            }

            // 地方
            if (node.Tag is RegionId)
            {
                var region = (RegionId) node.Tag;
                _list.AddRange(Provinces.Items.Where(province => province.Region == region));
                return;
            }

            // 地域
            if (node.Tag is AreaId)
            {
                var area = (AreaId) node.Tag;
                _list.AddRange(Provinces.Items.Where(province => province.Area == area));
                return;
            }

            _list.AddRange(Provinces.Items);
        }

        /// <summary>
        ///     プロヴィンスリストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProvinceListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 編集項目を更新する
            UpdateEditableItems();
        }

        /// <summary>
        ///     プロヴィンスリストビューの項目を作成する
        /// </summary>
        /// <param name="province">プロヴィンスデータ</param>
        /// <returns>プロヴィンスリストビューの項目</returns>
        private static ListViewItem CreateProvinceListViewItem(Province province)
        {
            if (province == null)
            {
                return null;
            }

            var item = new ListViewItem
                           {
                               Text = province.GetName(),
                               Tag = province
                           };
            item.SubItems.Add(province.Id.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(province.Terrain == TerrainId.Ocean ? Resources.Yes : Resources.No);
            item.SubItems.Add(province.PortAllowed ? Resources.Yes : Resources.No);
            item.SubItems.Add(province.Beaches ? Resources.Yes : Resources.No);
            item.SubItems.Add(province.Infrastructure.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(province.Ic.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(province.Manpower.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(province.Energy.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(province.Metal.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(province.RareMaterials.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(province.Oil.ToString(CultureInfo.InvariantCulture));

            return item;
        }

        /// <summary>
        ///     選択中のプロヴィンスデータを取得する
        /// </summary>
        /// <returns>選択中のプロヴィンスデータ</returns>
        private Province GetSelectedProvince()
        {
            // 選択項目がない場合
            if (provinceListView.SelectedItems.Count == 0)
            {
                return null;
            }

            return provinceListView.SelectedItems[0].Tag as Province;
        }

        #endregion

        #region 編集項目

        /// <summary>
        ///     編集項目を初期化する
        /// </summary>
        private void InitEditableItems()
        {
            // 大陸
            continentComboBox.BeginUpdate();
            continentComboBox.Items.Clear();
            int maxWidth = continentComboBox.DropDownWidth;
            foreach (ContinentId continent in Provinces.Continents)
            {
                string s = Provinces.GetContinentName(continent);
                continentComboBox.Items.Add(s);
                maxWidth = Math.Max(maxWidth,
                                    TextRenderer.MeasureText(s, continentComboBox.Font).Width +
                                    SystemInformation.VerticalScrollBarWidth);
            }
            continentComboBox.DropDownWidth = maxWidth;
            continentComboBox.EndUpdate();

            // 地方
            regionComboBox.BeginUpdate();
            regionComboBox.Items.Clear();
            maxWidth = regionComboBox.DropDownWidth;
            foreach (RegionId region in Provinces.Regions)
            {
                string s = Provinces.GetRegionName(region);
                regionComboBox.Items.Add(s);
                maxWidth = Math.Max(maxWidth,
                                    TextRenderer.MeasureText(s, regionComboBox.Font).Width +
                                    SystemInformation.VerticalScrollBarWidth);
            }
            regionComboBox.DropDownWidth = maxWidth;
            regionComboBox.EndUpdate();

            // 地域
            areaComboBox.BeginUpdate();
            areaComboBox.Items.Clear();
            maxWidth = areaComboBox.DropDownWidth;
            foreach (AreaId area in Provinces.Areas)
            {
                string s = Provinces.GetAreaName(area);
                areaComboBox.Items.Add(s);
                maxWidth = Math.Max(maxWidth,
                                    TextRenderer.MeasureText(s, areaComboBox.Font).Width +
                                    SystemInformation.VerticalScrollBarWidth);
            }
            areaComboBox.DropDownWidth = maxWidth;
            areaComboBox.EndUpdate();

            // 気候
            climateComboBox.BeginUpdate();
            climateComboBox.Items.Clear();
            maxWidth = climateComboBox.DropDownWidth;
            foreach (ClimateId climate in Provinces.Climates)
            {
                string s = Provinces.GetClimateName(climate);
                climateComboBox.Items.Add(s);
                maxWidth = Math.Max(maxWidth,
                                    TextRenderer.MeasureText(s, climateComboBox.Font).Width +
                                    SystemInformation.VerticalScrollBarWidth);
            }
            climateComboBox.DropDownWidth = maxWidth;
            climateComboBox.EndUpdate();

            // 地形
            terrainComboBox.BeginUpdate();
            terrainComboBox.Items.Clear();
            maxWidth = terrainComboBox.DropDownWidth;
            foreach (TerrainId terrain in Provinces.Terrains)
            {
                string s = Provinces.GetTerrainName(terrain);
                terrainComboBox.Items.Add(s);
                maxWidth = Math.Max(maxWidth,
                                    TextRenderer.MeasureText(s, terrainComboBox.Font).Width +
                                    SystemInformation.VerticalScrollBarWidth);
            }
            terrainComboBox.DropDownWidth = maxWidth;
            terrainComboBox.EndUpdate();
        }

        /// <summary>
        ///     編集項目を更新する
        /// </summary>
        private void UpdateEditableItems()
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 編集項目の値を更新する
            UpdateEditableItemsValue(province);

            // 編集項目の色を更新する
            UpdateEditableItemsColor(province);
        }

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="province">プロヴィンスデータ</param>
        private void UpdateEditableItemsValue(Province province)
        {
            // 基本設定
            idNumericUpDown.Value = province.Id;
            nameTextBox.Text = province.GetName();
            if (Provinces.Continents.Contains(province.Continent))
            {
                continentComboBox.SelectedIndex = Provinces.Continents.IndexOf(province.Continent);
            }
            else
            {
                continentComboBox.SelectedIndex = -1;
                continentComboBox.Text = Provinces.GetContinentName(province.Continent);
            }
            if (Provinces.Regions.Contains(province.Region))
            {
                regionComboBox.SelectedIndex = Provinces.Regions.IndexOf(province.Region);
            }
            else
            {
                regionComboBox.SelectedIndex = -1;
                regionComboBox.Text = Provinces.GetRegionName(province.Region);
            }
            if (Provinces.Areas.Contains(province.Area))
            {
                areaComboBox.SelectedIndex = Provinces.Areas.IndexOf(province.Area);
            }
            else
            {
                areaComboBox.SelectedIndex = -1;
                areaComboBox.Text = Provinces.GetAreaName(province.Area);
            }
            if (Provinces.Climates.Contains(province.Climate))
            {
                climateComboBox.SelectedIndex = Provinces.Climates.IndexOf(province.Climate);
            }
            else
            {
                climateComboBox.SelectedIndex = -1;
                climateComboBox.Text = Provinces.GetClimateName(province.Climate);
            }
            if (Provinces.Terrains.Contains(province.Terrain))
            {
                terrainComboBox.SelectedIndex = Provinces.Terrains.IndexOf(province.Terrain);
            }
            else
            {
                terrainComboBox.SelectedIndex = -1;
                terrainComboBox.Text = Provinces.GetTerrainName(province.Terrain);
            }

            // 資源設定
            infraNumericUpDown.Value = province.Infrastructure;
            icNumericUpDown.Value = province.Ic;
            manpowerNumericUpDown.Value = (decimal) province.Manpower;
            energyNumericUpDown.Value = province.Energy;
            metalNumericUpDown.Value = province.Metal;
            rareMaterialsNumericUpDown.Value = province.RareMaterials;
            oilNumericUpDown.Value = province.Oil;

            // 座標設定
            beachCheckBox.Checked = province.Beaches;
            beachXNumericUpDown.Value = province.BeachXPos;
            beachYNumericUpDown.Value = province.BeachYPos;
            beachIconNumericUpDown.Value = province.BeachIcon;
            portCheckBox.Checked = province.PortAllowed;
            portXNumericUpDown.Value = province.PortXPos;
            portYNumericUpDown.Value = province.PortYPos;
            portSeaZoneNumericUpDown.Value = province.PortSeaZone;
            if (Provinces.SeaZones.Contains(province.PortSeaZone))
            {
                portSeaZoneComboBox.SelectedIndex = Provinces.SeaZones.IndexOf(province.PortSeaZone);
            }
            else
            {
                portSeaZoneComboBox.SelectedIndex = -1;
                if (province.PortSeaZone > 0)
                {
                    Province seaProvince = Provinces.Items.First(prov => prov.Id == province.PortSeaZone);
                    portSeaZoneComboBox.Text = Config.GetText(seaProvince.Name);
                }
            }
            cityXNumericUpDown.Value = province.CityXPos;
            cityYNumericUpDown.Value = province.CityYPos;
            fortXNumericUpDown.Value = province.FortXPos;
            fortYNumericUpDown.Value = province.FortYPos;
            aaXNumericUpDown.Value = province.AaXPos;
            aaYNumericUpDown.Value = province.AaYPos;
            armyXNumericUpDown.Value = province.ArmyXPos;
            armyYNumericUpDown.Value = province.ArmyYPos;
            counterXNumericUpDown.Value = province.CounterXPos;
            counterYNumericUpDown.Value = province.CounterYPos;
            fillXNumericUpDown1.Value = province.FillCoordX1;
            fillYNumericUpDown1.Value = province.FillCoordY1;
            fillXNumericUpDown2.Value = province.FillCoordX2;
            fillYNumericUpDown2.Value = province.FillCoordY2;
            fillXNumericUpDown3.Value = province.FillCoordX3;
            fillYNumericUpDown3.Value = province.FillCoordY3;
            fillXNumericUpDown4.Value = province.FillCoordX4;
            fillYNumericUpDown4.Value = province.FillCoordY4;
        }

        /// <summary>
        ///     編集項目の色を更新する
        /// </summary>
        /// <param name="province">プロヴィンスデータ</param>
        private void UpdateEditableItemsColor(Province province)
        {
            // コンボボックスの色を更新する
            continentComboBox.Refresh();
            regionComboBox.Refresh();
            areaComboBox.Refresh();
            climateComboBox.Refresh();
            terrainComboBox.Refresh();
            portSeaZoneComboBox.Refresh();

            // 編集項目の色を更新する
            idNumericUpDown.ForeColor = province.IsDirty(ProvinceItemId.Id) ? Color.Red : SystemColors.WindowText;
            nameTextBox.ForeColor = province.IsDirty(ProvinceItemId.Name) ? Color.Red : SystemColors.WindowText;

            infraNumericUpDown.ForeColor = province.IsDirty(ProvinceItemId.Infrastructure)
                                               ? Color.Red
                                               : SystemColors.WindowText;
            icNumericUpDown.ForeColor = province.IsDirty(ProvinceItemId.Ic) ? Color.Red : SystemColors.WindowText;
            manpowerNumericUpDown.ForeColor = province.IsDirty(ProvinceItemId.Manpower)
                                                  ? Color.Red
                                                  : SystemColors.WindowText;
            energyNumericUpDown.ForeColor = province.IsDirty(ProvinceItemId.Energy)
                                                ? Color.Red
                                                : SystemColors.WindowText;
            metalNumericUpDown.ForeColor = province.IsDirty(ProvinceItemId.Metal) ? Color.Red : SystemColors.WindowText;
            rareMaterialsNumericUpDown.ForeColor = province.IsDirty(ProvinceItemId.RareMaterials)
                                                       ? Color.Red
                                                       : SystemColors.WindowText;
            oilNumericUpDown.ForeColor = province.IsDirty(ProvinceItemId.Oil) ? Color.Red : SystemColors.WindowText;

            beachCheckBox.ForeColor = province.IsDirty(ProvinceItemId.Beaches) ? Color.Red : SystemColors.WindowText;
            beachXNumericUpDown.ForeColor = province.IsDirty(ProvinceItemId.BeachXPos)
                                                ? Color.Red
                                                : SystemColors.WindowText;
            beachYNumericUpDown.ForeColor = province.IsDirty(ProvinceItemId.BeachYPos)
                                                ? Color.Red
                                                : SystemColors.WindowText;
            beachIconNumericUpDown.ForeColor = province.IsDirty(ProvinceItemId.BeachIcon)
                                                   ? Color.Red
                                                   : SystemColors.WindowText;
            portCheckBox.ForeColor = province.IsDirty(ProvinceItemId.PortAllowed) ? Color.Red : SystemColors.WindowText;
            portXNumericUpDown.ForeColor = province.IsDirty(ProvinceItemId.PortXPos)
                                               ? Color.Red
                                               : SystemColors.WindowText;
            portYNumericUpDown.ForeColor = province.IsDirty(ProvinceItemId.PortYPos)
                                               ? Color.Red
                                               : SystemColors.WindowText;
            portSeaZoneNumericUpDown.ForeColor = province.IsDirty(ProvinceItemId.PortSeaZone)
                                                     ? Color.Red
                                                     : SystemColors.WindowText;
            cityXNumericUpDown.ForeColor = province.IsDirty(ProvinceItemId.CityXPos)
                                               ? Color.Red
                                               : SystemColors.WindowText;
            cityYNumericUpDown.ForeColor = province.IsDirty(ProvinceItemId.CityYPos)
                                               ? Color.Red
                                               : SystemColors.WindowText;
            fortXNumericUpDown.ForeColor = province.IsDirty(ProvinceItemId.FortXPos)
                                               ? Color.Red
                                               : SystemColors.WindowText;
            fortYNumericUpDown.ForeColor = province.IsDirty(ProvinceItemId.FortYPos)
                                               ? Color.Red
                                               : SystemColors.WindowText;
            aaXNumericUpDown.ForeColor = province.IsDirty(ProvinceItemId.AaXPos) ? Color.Red : SystemColors.WindowText;
            aaYNumericUpDown.ForeColor = province.IsDirty(ProvinceItemId.AaYPos) ? Color.Red : SystemColors.WindowText;
            armyXNumericUpDown.ForeColor = province.IsDirty(ProvinceItemId.ArmyXPos)
                                               ? Color.Red
                                               : SystemColors.WindowText;
            armyYNumericUpDown.ForeColor = province.IsDirty(ProvinceItemId.ArmyYPos)
                                               ? Color.Red
                                               : SystemColors.WindowText;
            counterXNumericUpDown.ForeColor = province.IsDirty(ProvinceItemId.CounterXPos)
                                                  ? Color.Red
                                                  : SystemColors.WindowText;
            counterYNumericUpDown.ForeColor = province.IsDirty(ProvinceItemId.CounterYPos)
                                                  ? Color.Red
                                                  : SystemColors.WindowText;
            fillXNumericUpDown1.ForeColor = province.IsDirty(ProvinceItemId.FillCoordX1)
                                                ? Color.Red
                                                : SystemColors.WindowText;
            fillYNumericUpDown1.ForeColor = province.IsDirty(ProvinceItemId.FillCoordY1)
                                                ? Color.Red
                                                : SystemColors.WindowText;
            fillXNumericUpDown2.ForeColor = province.IsDirty(ProvinceItemId.FillCoordX2)
                                                ? Color.Red
                                                : SystemColors.WindowText;
            fillYNumericUpDown2.ForeColor = province.IsDirty(ProvinceItemId.FillCoordY2)
                                                ? Color.Red
                                                : SystemColors.WindowText;
            fillXNumericUpDown3.ForeColor = province.IsDirty(ProvinceItemId.FillCoordX3)
                                                ? Color.Red
                                                : SystemColors.WindowText;
            fillYNumericUpDown3.ForeColor = province.IsDirty(ProvinceItemId.FillCoordY3)
                                                ? Color.Red
                                                : SystemColors.WindowText;
            fillXNumericUpDown4.ForeColor = province.IsDirty(ProvinceItemId.FillCoordX4)
                                                ? Color.Red
                                                : SystemColors.WindowText;
            fillYNumericUpDown4.ForeColor = province.IsDirty(ProvinceItemId.FillCoordY4)
                                                ? Color.Red
                                                : SystemColors.WindowText;
        }

        /// <summary>
        ///     編集項目を有効化する
        /// </summary>
        private void EnableEditableItems()
        {
            basicGroupBox.Enabled = true;
            resourceGroupBox.Enabled = true;
            positionGroupBox.Enabled = true;

            // 無効化時にクリアした文字列を再設定する
            idNumericUpDown.Text = idNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            infraNumericUpDown.Text = infraNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            icNumericUpDown.Text = icNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            manpowerNumericUpDown.Text = manpowerNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            energyNumericUpDown.Text = energyNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            metalNumericUpDown.Text = metalNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            rareMaterialsNumericUpDown.Text = rareMaterialsNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            oilNumericUpDown.Text = oilNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            beachXNumericUpDown.Text = beachXNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            beachYNumericUpDown.Text = beachYNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            beachIconNumericUpDown.Text = beachIconNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            portXNumericUpDown.Text = portXNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            portYNumericUpDown.Text = portYNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            portSeaZoneNumericUpDown.Text = portSeaZoneNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            cityXNumericUpDown.Text = cityXNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            cityYNumericUpDown.Text = cityYNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            fortXNumericUpDown.Text = fortXNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            fortYNumericUpDown.Text = fortYNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            aaXNumericUpDown.Text = aaXNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            aaYNumericUpDown.Text = aaYNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            armyXNumericUpDown.Text = armyXNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            armyYNumericUpDown.Text = armyYNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            counterXNumericUpDown.Text = counterXNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            counterYNumericUpDown.Text = counterYNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            fillXNumericUpDown1.Text = fillXNumericUpDown1.Value.ToString(CultureInfo.InvariantCulture);
            fillYNumericUpDown1.Text = fillYNumericUpDown1.Value.ToString(CultureInfo.InvariantCulture);
            fillXNumericUpDown2.Text = fillXNumericUpDown2.Value.ToString(CultureInfo.InvariantCulture);
            fillYNumericUpDown2.Text = fillYNumericUpDown2.Value.ToString(CultureInfo.InvariantCulture);
            fillXNumericUpDown3.Text = fillXNumericUpDown3.Value.ToString(CultureInfo.InvariantCulture);
            fillYNumericUpDown3.Text = fillYNumericUpDown3.Value.ToString(CultureInfo.InvariantCulture);
            fillXNumericUpDown4.Text = fillXNumericUpDown4.Value.ToString(CultureInfo.InvariantCulture);
            fillYNumericUpDown4.Text = fillYNumericUpDown4.Value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     編集項目を無効化する
        /// </summary>
        private void DisableEditableItems()
        {
            idNumericUpDown.ResetText();
            nameTextBox.ResetText();
            continentComboBox.SelectedIndex = -1;
            continentComboBox.ResetText();
            regionComboBox.SelectedIndex = -1;
            regionComboBox.ResetText();
            areaComboBox.SelectedIndex = -1;
            areaComboBox.ResetText();
            climateComboBox.SelectedIndex = -1;
            climateComboBox.ResetText();
            terrainComboBox.SelectedIndex = -1;
            terrainComboBox.ResetText();

            infraNumericUpDown.ResetText();
            icNumericUpDown.ResetText();
            manpowerNumericUpDown.ResetText();
            energyNumericUpDown.ResetText();
            metalNumericUpDown.ResetText();
            rareMaterialsNumericUpDown.ResetText();
            oilNumericUpDown.ResetText();

            beachCheckBox.Checked = false;
            beachXNumericUpDown.ResetText();
            beachYNumericUpDown.ResetText();
            beachIconNumericUpDown.ResetText();
            portCheckBox.Checked = false;
            portXNumericUpDown.ResetText();
            portYNumericUpDown.ResetText();
            portSeaZoneNumericUpDown.ResetText();
            portSeaZoneComboBox.SelectedIndex = -1;
            portSeaZoneComboBox.ResetText();
            cityXNumericUpDown.ResetText();
            cityYNumericUpDown.ResetText();
            fortXNumericUpDown.ResetText();
            fortYNumericUpDown.ResetText();
            aaXNumericUpDown.ResetText();
            aaYNumericUpDown.ResetText();
            armyXNumericUpDown.ResetText();
            armyYNumericUpDown.ResetText();
            counterXNumericUpDown.ResetText();
            counterYNumericUpDown.ResetText();
            fillXNumericUpDown1.ResetText();
            fillYNumericUpDown1.ResetText();
            fillXNumericUpDown2.ResetText();
            fillYNumericUpDown2.ResetText();
            fillXNumericUpDown3.ResetText();
            fillYNumericUpDown3.ResetText();
            fillXNumericUpDown4.ResetText();
            fillYNumericUpDown4.ResetText();

            basicGroupBox.Enabled = false;
            resourceGroupBox.Enabled = false;
            positionGroupBox.Enabled = false;
        }

        /// <summary>
        ///     港の海域コンボボックスの項目を更新する
        /// </summary>
        private void UpdateSeaZoneItems()
        {
            portSeaZoneComboBox.BeginUpdate();
            portSeaZoneComboBox.Items.Clear();
            int maxWidth = portSeaZoneComboBox.DropDownWidth;
            foreach (int id in Provinces.SeaZones)
            {
                Province province = Provinces.SeaZoneMap[id];
                string s = province.GetName();
                portSeaZoneComboBox.Items.Add(s);
                maxWidth = Math.Max(maxWidth,
                                    TextRenderer.MeasureText(s, portSeaZoneComboBox.Font).Width +
                                    SystemInformation.VerticalScrollBarWidth);
            }
            portSeaZoneComboBox.DropDownWidth = maxWidth;
            portSeaZoneComboBox.EndUpdate();
        }

        /// <summary>
        ///     大陸コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnContinentComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Province province = GetSelectedProvince();
            if (province != null)
            {
                Brush brush;
                if ((Provinces.Continents[e.Index] == province.Continent) && province.IsDirty(ProvinceItemId.Continent))
                {
                    brush = new SolidBrush(Color.Red);
                }
                else
                {
                    brush = new SolidBrush(SystemColors.WindowText);
                }
                string s = continentComboBox.Items[e.Index].ToString();
                e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     地方コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRegionComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Province province = GetSelectedProvince();
            if (province != null)
            {
                Brush brush;
                if ((Provinces.Regions[e.Index] == province.Region) && province.IsDirty(ProvinceItemId.Region))
                {
                    brush = new SolidBrush(Color.Red);
                }
                else
                {
                    brush = new SolidBrush(SystemColors.WindowText);
                }
                string s = regionComboBox.Items[e.Index].ToString();
                e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     地域コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAreaComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Province province = GetSelectedProvince();
            if (province != null)
            {
                Brush brush;
                if ((Provinces.Areas[e.Index] == province.Area) && province.IsDirty(ProvinceItemId.Area))
                {
                    brush = new SolidBrush(Color.Red);
                }
                else
                {
                    brush = new SolidBrush(SystemColors.WindowText);
                }
                string s = areaComboBox.Items[e.Index].ToString();
                e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     気候コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClimateComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Province province = GetSelectedProvince();
            if (province != null)
            {
                Brush brush;
                if ((Provinces.Climates[e.Index] == province.Climate) && province.IsDirty(ProvinceItemId.Climate))
                {
                    brush = new SolidBrush(Color.Red);
                }
                else
                {
                    brush = new SolidBrush(SystemColors.WindowText);
                }
                string s = climateComboBox.Items[e.Index].ToString();
                e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     地形コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTerrainComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Province province = GetSelectedProvince();
            if (province != null)
            {
                Brush brush;
                if ((Provinces.Terrains[e.Index] == province.Terrain) && province.IsDirty(ProvinceItemId.Terrain))
                {
                    brush = new SolidBrush(Color.Red);
                }
                else
                {
                    brush = new SolidBrush(SystemColors.WindowText);
                }
                string s = terrainComboBox.Items[e.Index].ToString();
                e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     港の海域コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPortSeaZoneComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Province province = GetSelectedProvince();
            if (province != null)
            {
                Brush brush;
                if ((Provinces.SeaZones[e.Index] == province.PortSeaZone) &&
                    province.IsDirty(ProvinceItemId.PortSeaZone))
                {
                    brush = new SolidBrush(Color.Red);
                }
                else
                {
                    brush = new SolidBrush(SystemColors.WindowText);
                }
                string s = portSeaZoneComboBox.Items[e.Index].ToString();
                e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     名前文字列変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNameTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            string name = nameTextBox.Text;
            if (name.Equals(province.GetName()))
            {
                return;
            }

            // 値を更新する
            Config.SetText(province.Name, name, Game.ProvinceTextFileName);

            // プロヴィンスリストビューの項目を更新する
            provinceListView.SelectedItems[0].SubItems[0].Text = province.GetName();

            // 編集済みフラグを設定する
            province.SetDirty(ProvinceItemId.Name);
            Config.SetDirty(Game.ProvinceTextFileName);

            // 文字色を変更する
            nameTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     大陸変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnContinentComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (continentComboBox.SelectedIndex == -1)
            {
                return;
            }
            ContinentId continent = Provinces.Continents[continentComboBox.SelectedIndex];
            if (continent == province.Continent)
            {
                return;
            }

            // 値を更新する
            province.Continent = continent;

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.Continent);

            // 大陸コンボボックスの項目色を変更するため描画更新する
            continentComboBox.Refresh();
        }

        /// <summary>
        ///     地方変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRegionComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (regionComboBox.SelectedIndex == -1)
            {
                return;
            }
            RegionId region = Provinces.Regions[regionComboBox.SelectedIndex];
            if (region == province.Region)
            {
                return;
            }

            // 値を更新する
            province.Region = region;

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.Region);

            // 地方コンボボックスの項目色を変更するため描画更新する
            regionComboBox.Refresh();
        }

        /// <summary>
        ///     地域変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAreaComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (areaComboBox.SelectedIndex == -1)
            {
                return;
            }
            AreaId area = Provinces.Areas[areaComboBox.SelectedIndex];
            if (area == province.Area)
            {
                return;
            }

            // 値を更新する
            province.Area = area;

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.Area);

            // 地域コンボボックスの項目色を変更するため描画更新する
            areaComboBox.Refresh();
        }

        /// <summary>
        ///     気候変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClimateComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (climateComboBox.SelectedIndex == -1)
            {
                return;
            }
            ClimateId climate = Provinces.Climates[climateComboBox.SelectedIndex];
            if (climate == province.Climate)
            {
                return;
            }

            // 値を更新する
            province.Climate = climate;

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.Climate);

            // 気候コンボボックスの項目色を変更するため描画更新する
            climateComboBox.Refresh();
        }

        /// <summary>
        ///     地形変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTerrainComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (terrainComboBox.SelectedIndex == -1)
            {
                return;
            }
            TerrainId terrain = Provinces.Terrains[terrainComboBox.SelectedIndex];
            if (terrain == province.Terrain || (terrain == TerrainId.Plains && province.Terrain == TerrainId.Clear))
            {
                return;
            }

            // 値を更新する
            province.Terrain = terrain;

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.Terrain);

            // 地形コンボボックスの項目色を変更するため描画更新する
            terrainComboBox.Refresh();
        }

        /// <summary>
        ///     インフラ変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnInfraNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var infra = (int) infraNumericUpDown.Value;
            if (infra == province.Infrastructure)
            {
                return;
            }

            // 値を更新する
            province.Infrastructure = infra;

            // プロヴィンスリストビューの項目を更新する
            provinceListView.SelectedItems[0].SubItems[5].Text =
                province.Infrastructure.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.Infrastructure);

            // 文字色を変更する
            infraNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     IC変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIcNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var ic = (int) icNumericUpDown.Value;
            if (ic == province.Ic)
            {
                return;
            }

            // 値を更新する
            province.Ic = ic;

            // プロヴィンスリストビューの項目を更新する
            provinceListView.SelectedItems[0].SubItems[6].Text = province.Ic.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.Ic);

            // 文字色を変更する
            icNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     労働力変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnManpowerNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var manpower = (double) manpowerNumericUpDown.Value;
            if (Math.Abs(manpower - province.Manpower) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            province.Manpower = manpower;

            // プロヴィンスリストビューの項目を更新する
            provinceListView.SelectedItems[0].SubItems[7].Text = province.Manpower.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.Manpower);

            // 文字色を変更する
            manpowerNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     エネルギー変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEnergyNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var energy = (int) energyNumericUpDown.Value;
            if (energy == province.Energy)
            {
                return;
            }

            // 値を更新する
            province.Energy = energy;

            // プロヴィンスリストビューの項目を更新する
            provinceListView.SelectedItems[0].SubItems[8].Text = province.Energy.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.Energy);

            // 文字色を変更する
            energyNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     金属変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMetalNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var metal = (int) metalNumericUpDown.Value;
            if (metal == province.Metal)
            {
                return;
            }

            // 値を更新する
            province.Metal = metal;

            // プロヴィンスリストビューの項目を更新する
            provinceListView.SelectedItems[0].SubItems[9].Text = province.Metal.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.Metal);

            // 文字色を変更する
            metalNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     希少資源変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRareMaterialsNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var rareMaterials = (int) rareMaterialsNumericUpDown.Value;
            if (rareMaterials == province.RareMaterials)
            {
                return;
            }

            // 値を更新する
            province.RareMaterials = rareMaterials;

            // プロヴィンスリストビューの項目を更新する
            provinceListView.SelectedItems[0].SubItems[10].Text =
                province.RareMaterials.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.RareMaterials);

            // 文字色を変更する
            rareMaterialsNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     石油変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOilNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var oil = (int) oilNumericUpDown.Value;
            if (oil == province.Oil)
            {
                return;
            }

            // 値を更新する
            province.Oil = oil;

            // プロヴィンスリストビューの項目を更新する
            provinceListView.SelectedItems[0].SubItems[11].Text = province.Oil.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.Oil);

            // 文字色を変更する
            oilNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     砂浜チェックボックスのチェック状態変化時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBeachCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            bool flag = beachCheckBox.Checked;
            if (flag == province.Beaches)
            {
                return;
            }

            // 値を更新する
            province.Beaches = flag;

            // プロヴィンスリストビューの項目を更新する
            provinceListView.SelectedItems[0].SubItems[4].Text = province.Beaches ? Resources.Yes : Resources.No;

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.Beaches);

            // 文字色を変更する
            beachCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     砂浜のX座標変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBeachXNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var x = (int) beachXNumericUpDown.Value;
            if (x == province.BeachXPos)
            {
                return;
            }

            // 値を更新する
            province.BeachXPos = x;

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.BeachXPos);

            // 文字色を変更する
            beachXNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     砂浜のY座標変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBeachYNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var y = (int) beachYNumericUpDown.Value;
            if (y == province.BeachYPos)
            {
                return;
            }

            // 値を更新する
            province.BeachYPos = y;

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.BeachYPos);

            // 文字色を変更する
            beachYNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     砂浜のアイコン変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBeachIconNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var icon = (int) beachIconNumericUpDown.Value;
            if (icon == province.BeachIcon)
            {
                return;
            }

            // 値を更新する
            province.BeachIcon = icon;

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.BeachIcon);

            // 文字色を変更する
            beachIconNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     港チェックボックスのチェック状態変化時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPortCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            bool flag = portCheckBox.Checked;
            if (flag == province.PortAllowed)
            {
                return;
            }

            // 値を更新する
            province.PortAllowed = flag;

            // プロヴィンスリストビューの項目を更新する
            provinceListView.SelectedItems[0].SubItems[3].Text = province.PortAllowed ? Resources.Yes : Resources.No;

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.PortAllowed);

            // 文字色を変更する
            portCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     港のX座標変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPortXNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var x = (int) portXNumericUpDown.Value;
            if (x == province.PortXPos)
            {
                return;
            }

            // 値を更新する
            province.PortXPos = x;

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.PortXPos);

            // 文字色を変更する
            portXNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     港のY座標変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPortYNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var y = (int) portYNumericUpDown.Value;
            if (y == province.PortYPos)
            {
                return;
            }

            // 値を更新する
            province.PortYPos = y;

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.PortYPos);

            // 文字色を変更する
            portYNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     港の海域変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPortSeaZoneNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var seaZone = (int) portSeaZoneNumericUpDown.Value;
            if (seaZone == province.PortSeaZone)
            {
                return;
            }

            // 値を更新する
            province.PortSeaZone = seaZone;

            // 港の海域コンボボックスの項目を更新する
            if (Provinces.SeaZones.Contains(seaZone))
            {
                portSeaZoneComboBox.SelectedIndex = Provinces.SeaZones.IndexOf(seaZone);
            }
            else
            {
                portSeaZoneComboBox.SelectedIndex = -1;
                if (seaZone > 0)
                {
                    Province seaProvince = Provinces.Items.First(prov => prov.Id == seaZone);
                    portSeaZoneComboBox.Text = Config.GetText(seaProvince.Name);
                }
            }

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.PortSeaZone);

            // 文字色を変更する
            portSeaZoneNumericUpDown.ForeColor = Color.Red;

            // 港の海域コンボボックスの項目色を変更するため描画更新する
            portSeaZoneComboBox.Refresh();
        }

        /// <summary>
        ///     港の海域コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPortSeaZoneComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (portSeaZoneComboBox.SelectedIndex == -1)
            {
                return;
            }
            int seaZone = Provinces.SeaZones[portSeaZoneComboBox.SelectedIndex];
            if (seaZone == province.PortSeaZone)
            {
                return;
            }

            // 港の海域の値を更新する
            portSeaZoneNumericUpDown.Value = seaZone;
        }

        /// <summary>
        ///     都市のX座標変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCityXNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var x = (int) cityXNumericUpDown.Value;
            if (x == province.CityXPos)
            {
                return;
            }

            // 値を更新する
            province.CityXPos = x;

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.CityXPos);

            // 文字色を変更する
            cityXNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     都市のY座標変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCityYNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var y = (int) cityYNumericUpDown.Value;
            if (y == province.CityYPos)
            {
                return;
            }

            // 値を更新する
            province.CityYPos = y;

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.CityYPos);

            // 文字色を変更する
            cityYNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     要塞のX座標変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFortXNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var x = (int) fortXNumericUpDown.Value;
            if (x == province.FortXPos)
            {
                return;
            }

            // 値を更新する
            province.FortXPos = x;

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.FortXPos);

            // 文字色を変更する
            fortXNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     要塞のY座標変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFortYNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var y = (int) fortYNumericUpDown.Value;
            if (y == province.FortYPos)
            {
                return;
            }

            // 値を更新する
            province.FortYPos = y;

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.FortYPos);

            // 文字色を変更する
            fortYNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     対空砲のX座標変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAaXNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var x = (int) aaXNumericUpDown.Value;
            if (x == province.AaXPos)
            {
                return;
            }

            // 値を更新する
            province.AaXPos = x;

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.AaXPos);

            // 文字色を変更する
            aaXNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     対空砲のY座標変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAaYNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var y = (int) aaYNumericUpDown.Value;
            if (y == province.AaYPos)
            {
                return;
            }

            // 値を更新する
            province.AaYPos = y;

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.AaYPos);

            // 文字色を変更する
            aaYNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     軍隊のX座標変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnArmyXNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var x = (int) armyXNumericUpDown.Value;
            if (x == province.ArmyXPos)
            {
                return;
            }

            // 値を更新する
            province.ArmyXPos = x;

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.ArmyXPos);

            // 文字色を変更する
            armyXNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     軍隊のY座標変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnArmyYNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var y = (int) armyYNumericUpDown.Value;
            if (y == province.ArmyYPos)
            {
                return;
            }

            // 値を更新する
            province.ArmyYPos = y;

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.ArmyYPos);

            // 文字色を変更する
            armyYNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     カウンターのX座標変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCounterXNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var x = (int) counterXNumericUpDown.Value;
            if (x == province.CounterXPos)
            {
                return;
            }

            // 値を更新する
            province.CounterXPos = x;

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.CounterXPos);

            // 文字色を変更する
            counterXNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     カウンターのY座標変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCounterYNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var y = (int) counterYNumericUpDown.Value;
            if (y == province.CounterYPos)
            {
                return;
            }

            // 値を更新する
            province.CounterYPos = y;

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.CounterYPos);

            // 文字色を変更する
            counterYNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     塗りつぶしX座標1変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFillXNumericUpDown1ValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var x = (int) fillXNumericUpDown1.Value;
            if (x == province.FillCoordX1)
            {
                return;
            }

            // 値を更新する
            province.FillCoordX1 = x;

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.FillCoordX1);

            // 文字色を変更する
            fillXNumericUpDown1.ForeColor = Color.Red;
        }

        /// <summary>
        ///     塗りつぶしY座標1変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFillYNumericUpDown1ValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var y = (int) fillYNumericUpDown1.Value;
            if (y == province.FillCoordY1)
            {
                return;
            }

            // 値を更新する
            province.FillCoordY1 = y;

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.FillCoordY1);

            // 文字色を変更する
            fillYNumericUpDown1.ForeColor = Color.Red;
        }

        /// <summary>
        ///     塗りつぶしX座標2変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFillXNumericUpDown2ValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var x = (int) fillXNumericUpDown2.Value;
            if (x == province.FillCoordX2)
            {
                return;
            }

            // 値を更新する
            province.FillCoordX2 = x;

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.FillCoordX2);

            // 文字色を変更する
            fillXNumericUpDown2.ForeColor = Color.Red;
        }

        /// <summary>
        ///     塗りつぶしY座標2変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFillYNumericUpDown2ValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var y = (int) fillYNumericUpDown2.Value;
            if (y == province.FillCoordY2)
            {
                return;
            }

            // 値を更新する
            province.FillCoordY2 = y;

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.FillCoordY2);

            // 文字色を変更する
            fillYNumericUpDown2.ForeColor = Color.Red;
        }

        /// <summary>
        ///     塗りつぶしX座標3変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFillXNumericUpDown3ValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var x = (int) fillXNumericUpDown3.Value;
            if (x == province.FillCoordX3)
            {
                return;
            }

            // 値を更新する
            province.FillCoordX3 = x;

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.FillCoordX3);

            // 文字色を変更する
            fillXNumericUpDown3.ForeColor = Color.Red;
        }

        /// <summary>
        ///     塗りつぶしY座標3変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFillYNumericUpDown3ValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var y = (int) fillYNumericUpDown3.Value;
            if (y == province.FillCoordY3)
            {
                return;
            }

            // 値を更新する
            province.FillCoordY3 = y;

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.FillCoordY3);

            // 文字色を変更する
            fillYNumericUpDown3.ForeColor = Color.Red;
        }

        /// <summary>
        ///     塗りつぶしX座標4変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFillXNumericUpDown4ValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var x = (int) fillXNumericUpDown4.Value;
            if (x == province.FillCoordX4)
            {
                return;
            }

            // 値を更新する
            province.FillCoordX4 = x;

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.FillCoordX4);

            // 文字色を変更する
            fillXNumericUpDown4.ForeColor = Color.Red;
        }

        /// <summary>
        ///     塗りつぶしY座標4変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFillYNumericUpDown4ValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var y = (int) fillYNumericUpDown4.Value;
            if (y == province.FillCoordY4)
            {
                return;
            }

            // 値を更新する
            province.FillCoordY4 = y;

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.FillCoordY4);

            // 文字色を変更する
            fillYNumericUpDown4.ForeColor = Color.Red;
        }

        #endregion
    }
}