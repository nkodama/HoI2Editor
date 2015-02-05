using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HoI2Editor.Models;
using HoI2Editor.Properties;
using HoI2Editor.Utilities;

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
        private readonly TreeNode _worldNode = new TreeNode { Text = Resources.World };

        /// <summary>
        ///     ソート対象
        /// </summary>
        private SortKey _key = SortKey.None;

        /// <summary>
        ///     ソート順
        /// </summary>
        private SortOrder _order = SortOrder.Ascendant;

        /// <summary>
        ///     ソート対象
        /// </summary>
        private enum SortKey
        {
            None,
            Name,
            Id,
            Sea,
            Port,
            Beach,
            Infrastructure,
            Ic,
            Manpower,
            Energy,
            Metal,
            RareMaterials,
            Oil
        }

        /// <summary>
        ///     ソート順
        /// </summary>
        private enum SortOrder
        {
            Ascendant,
            Decendant
        }

        #endregion

        #region 公開定数

        /// <summary>
        ///     プロヴィンスリストビューの列の数
        /// </summary>
        public const int ProvinceListColumnCount = 12;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public ProvinceEditorForm()
        {
            InitializeComponent();

            // フォームの初期化
            InitForm();
        }

        #endregion

        #region データ処理

        /// <summary>
        ///     データ読み込み後の処理
        /// </summary>
        public void OnFileLoaded()
        {
            // 海域の編集項目を更新する
            UpdateSeaZoneItems();

            // ワールドツリービューの表示を更新する
            UpdateWorldTree();
        }

        /// <summary>
        ///     データ保存後の処理
        /// </summary>
        public void OnFileSaved()
        {
            // 編集済みフラグがクリアされるため表示を更新する
            UpdateEditableItems();
        }

        /// <summary>
        ///     編集項目変更後の処理
        /// </summary>
        /// <param name="id">編集項目ID</param>
        public void OnItemChanged(EditorItemId id)
        {
            // 何もしない
        }

        #endregion

        #region フォーム

        /// <summary>
        ///     フォームの初期化
        /// </summary>
        private void InitForm()
        {
            // プロヴィンスリストビュー
            nameColumnHeader.Width = HoI2Editor.Settings.ProvinceEditor.ListColumnWidth[0];
            idColumnHeader.Width = HoI2Editor.Settings.ProvinceEditor.ListColumnWidth[1];
            seaColumnHeader.Width = HoI2Editor.Settings.ProvinceEditor.ListColumnWidth[2];
            portColumnHeader.Width = HoI2Editor.Settings.ProvinceEditor.ListColumnWidth[3];
            beachColumnHeader.Width = HoI2Editor.Settings.ProvinceEditor.ListColumnWidth[4];
            infraColumnHeader.Width = HoI2Editor.Settings.ProvinceEditor.ListColumnWidth[5];
            icColumnHeader.Width = HoI2Editor.Settings.ProvinceEditor.ListColumnWidth[6];
            manpowerColumnHeader.Width = HoI2Editor.Settings.ProvinceEditor.ListColumnWidth[7];
            energyColumnHeader.Width = HoI2Editor.Settings.ProvinceEditor.ListColumnWidth[8];
            metalColumnHeader.Width = HoI2Editor.Settings.ProvinceEditor.ListColumnWidth[9];
            rareMaterialsColumnHeader.Width = HoI2Editor.Settings.ProvinceEditor.ListColumnWidth[10];
            oilColumnHeader.Width = HoI2Editor.Settings.ProvinceEditor.ListColumnWidth[11];

            // ウィンドウの位置
            Location = HoI2Editor.Settings.ProvinceEditor.Location;
            Size = HoI2Editor.Settings.ProvinceEditor.Size;
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormLoad(object sender, EventArgs e)
        {
            // プロヴィンスデータを初期化する
            Provinces.Init();

            // ゲーム設定ファイルを読み込む
            Misc.Load();

            // 文字列定義ファイルを読み込む
            Config.Load();

            // 編集項目を初期化する
            InitEditableItems();

            // プロヴィンスファイルを読み込む
            Provinces.Load();

            // データ読み込み後の処理
            OnFileLoaded();
        }

        /// <summary>
        ///     フォームクローズ時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            // 編集済みでなければフォームを閉じる
            if (!HoI2Editor.IsDirty())
            {
                return;
            }

            // 保存するかを問い合わせる
            DialogResult result = MessageBox.Show(Resources.ConfirmSaveMessage, Text, MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);
            switch (result)
            {
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
                case DialogResult.Yes:
                    HoI2Editor.Save();
                    break;
                case DialogResult.No:
                    HoI2Editor.SaveCanceled = true;
                    break;
            }
        }

        /// <summary>
        ///     フォームクローズ後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            HoI2Editor.OnProvinceEditorFormClosed();
        }

        /// <summary>
        ///     フォーム移動時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormMove(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                HoI2Editor.Settings.ProvinceEditor.Location = Location;
            }
        }

        /// <summary>
        ///     フォームリサイズ時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormResize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                HoI2Editor.Settings.ProvinceEditor.Size = Size;
            }
        }

        /// <summary>
        ///     再読み込みボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReloadButtonClick(object sender, EventArgs e)
        {
            // 編集済みならば保存するかを問い合わせる
            if (HoI2Editor.IsDirty())
            {
                DialogResult result = MessageBox.Show(Resources.ConfirmSaveMessage, Text, MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);
                switch (result)
                {
                    case DialogResult.Cancel:
                        return;
                    case DialogResult.Yes:
                        HoI2Editor.Save();
                        break;
                }
            }

            HoI2Editor.Reload();
        }

        /// <summary>
        ///     保存ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            HoI2Editor.Save();
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
            _worldNode.Nodes.Clear();
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
            TreeNode node = new TreeNode { Text = Provinces.GetContinentName(continent), Tag = continent };
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
            TreeNode node = new TreeNode { Text = Provinces.GetRegionName(region), Tag = region };
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
            TreeNode node = new TreeNode { Text = Provinces.GetAreaName(area), Tag = area };
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
                ContinentId continent = (ContinentId) node.Tag;
                _list.AddRange(Provinces.Items.Where(province => province.Continent == continent));
                return;
            }

            // 地方
            if (node.Tag is RegionId)
            {
                RegionId region = (RegionId) node.Tag;
                _list.AddRange(Provinces.Items.Where(province => province.Region == region));
                return;
            }

            // 地域
            if (node.Tag is AreaId)
            {
                AreaId area = (AreaId) node.Tag;
                _list.AddRange(Provinces.Items.Where(province => province.Area == area));
                return;
            }

            _list.AddRange(Provinces.Items);
        }

        /// <summary>
        ///     プロヴィンスリストをソートする
        /// </summary>
        private void SortProvinceList()
        {
            switch (_key)
            {
                case SortKey.None: // ソートなし
                    break;

                case SortKey.Name: // 名前
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((province1, province2) => string.CompareOrdinal(province1.Name, province2.Name));
                    }
                    else
                    {
                        _list.Sort((province1, province2) => string.CompareOrdinal(province2.Name, province1.Name));
                    }
                    break;

                case SortKey.Id: // ID
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((province1, province2) => province1.Id - province2.Id);
                    }
                    else
                    {
                        _list.Sort((province1, province2) => province2.Id - province1.Id);
                    }
                    break;

                case SortKey.Sea: // 海洋プロヴィンスかどうか
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((province1, province2) =>
                        {
                            if (province1.Terrain == TerrainId.Ocean && province2.Terrain != TerrainId.Ocean)
                            {
                                return 1;
                            }
                            if (province2.Terrain == TerrainId.Ocean && province1.Terrain != TerrainId.Ocean)
                            {
                                return -1;
                            }
                            return 0;
                        });
                    }
                    else
                    {
                        _list.Sort((province1, province2) =>
                        {
                            if (province2.Terrain == TerrainId.Ocean && province1.Terrain != TerrainId.Ocean)
                            {
                                return 1;
                            }
                            if (province1.Terrain == TerrainId.Ocean && province2.Terrain != TerrainId.Ocean)
                            {
                                return -1;
                            }
                            return 0;
                        });
                    }
                    break;

                case SortKey.Port: // 港の有無
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((province1, province2) =>
                        {
                            if (province1.PortAllowed && !province2.PortAllowed)
                            {
                                return 1;
                            }
                            if (!province1.PortAllowed && province2.PortAllowed)
                            {
                                return -1;
                            }
                            return 0;
                        });
                    }
                    else
                    {
                        _list.Sort((province1, province2) =>
                        {
                            if (province2.PortAllowed && !province1.PortAllowed)
                            {
                                return 1;
                            }
                            if (!province2.PortAllowed && province1.PortAllowed)
                            {
                                return -1;
                            }
                            return 0;
                        });
                    }
                    break;

                case SortKey.Beach: // 砂浜の有無
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((province1, province2) =>
                        {
                            if (province1.Beaches && !province2.Beaches)
                            {
                                return 1;
                            }
                            if (!province1.Beaches && province2.Beaches)
                            {
                                return -1;
                            }
                            return 0;
                        });
                    }
                    else
                    {
                        _list.Sort((province1, province2) =>
                        {
                            if (province2.Beaches && !province1.Beaches)
                            {
                                return 1;
                            }
                            if (!province2.Beaches && province1.Beaches)
                            {
                                return -1;
                            }
                            return 0;
                        });
                    }
                    break;

                case SortKey.Infrastructure: // インフラ
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort(
                            (province1, province2) => Math.Sign(province1.Infrastructure - province2.Infrastructure));
                    }
                    else
                    {
                        _list.Sort(
                            (province1, province2) => Math.Sign(province2.Infrastructure - province1.Infrastructure));
                    }
                    break;

                case SortKey.Ic: // IC
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((province1, province2) => Math.Sign(province1.Ic - province2.Ic));
                    }
                    else
                    {
                        _list.Sort((province1, province2) => Math.Sign(province2.Ic - province1.Ic));
                    }
                    break;

                case SortKey.Manpower: // 労働力
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((province1, province2) => Math.Sign(province1.Manpower - province2.Manpower));
                    }
                    else
                    {
                        _list.Sort((province1, province2) => Math.Sign(province2.Manpower - province1.Manpower));
                    }
                    break;

                case SortKey.Energy: // エネルギー
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((province1, province2) => Math.Sign(province1.Energy - province2.Energy));
                    }
                    else
                    {
                        _list.Sort((province1, province2) => Math.Sign(province2.Energy - province1.Energy));
                    }
                    break;

                case SortKey.Metal: // 金属
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((province1, province2) => Math.Sign(province1.Metal - province2.Metal));
                    }
                    else
                    {
                        _list.Sort((province1, province2) => Math.Sign(province2.Metal - province1.Metal));
                    }
                    break;

                case SortKey.RareMaterials: // 希少資源
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort(
                            (province1, province2) => Math.Sign(province1.RareMaterials - province2.RareMaterials));
                    }
                    else
                    {
                        _list.Sort(
                            (province1, province2) => Math.Sign(province2.RareMaterials - province1.RareMaterials));
                    }
                    break;

                case SortKey.Oil: // 石油
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((province1, province2) => Math.Sign(province1.Oil - province2.Oil));
                    }
                    else
                    {
                        _list.Sort((province1, province2) => Math.Sign(province2.Oil - province1.Oil));
                    }
                    break;
            }
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
        ///     閣僚リストビューのカラムクリック時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLeaderListViewColumnClick(object sender, ColumnClickEventArgs e)
        {
            switch (e.Column)
            {
                case 0: // 名前
                    if (_key == SortKey.Name)
                    {
                        _order = (_order == SortOrder.Ascendant) ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.Name;
                    }
                    break;

                case 1: // ID
                    if (_key == SortKey.Id)
                    {
                        _order = (_order == SortOrder.Ascendant) ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.Id;
                    }
                    break;

                case 2: // 海洋プロヴィンスかどうか
                    if (_key == SortKey.Sea)
                    {
                        _order = (_order == SortOrder.Ascendant) ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.Sea;
                    }
                    break;

                case 3: // 港の有無
                    if (_key == SortKey.Port)
                    {
                        _order = (_order == SortOrder.Ascendant) ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.Port;
                    }
                    break;

                case 4: // 砂浜の有無
                    if (_key == SortKey.Beach)
                    {
                        _order = (_order == SortOrder.Ascendant) ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.Beach;
                    }
                    break;

                case 5: // インフラ
                    if (_key == SortKey.Infrastructure)
                    {
                        _order = (_order == SortOrder.Ascendant) ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.Infrastructure;
                    }
                    break;

                case 6: // IC
                    if (_key == SortKey.Ic)
                    {
                        _order = (_order == SortOrder.Ascendant) ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.Ic;
                    }
                    break;

                case 7: // 労働力
                    if (_key == SortKey.Manpower)
                    {
                        _order = (_order == SortOrder.Ascendant) ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.Manpower;
                    }
                    break;

                case 8: // エネルギー
                    if (_key == SortKey.Energy)
                    {
                        _order = (_order == SortOrder.Ascendant) ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.Energy;
                    }
                    break;

                case 9: // 金属
                    if (_key == SortKey.Metal)
                    {
                        _order = (_order == SortOrder.Ascendant) ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.Metal;
                    }
                    break;

                case 10: // 希少資源
                    if (_key == SortKey.RareMaterials)
                    {
                        _order = (_order == SortOrder.Ascendant) ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.RareMaterials;
                    }
                    break;

                case 11: // 石油
                    if (_key == SortKey.Oil)
                    {
                        _order = (_order == SortOrder.Ascendant) ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.Oil;
                    }
                    break;

                default:
                    // 項目のない列をクリックした時には何もしない
                    return;
            }

            // プロヴィンスリストをソートする
            SortProvinceList();

            // プロヴィンスリストを更新する
            UpdateProvinceList();
        }

        /// <summary>
        ///     プロヴィンスリストビューの列の幅変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTeamListViewColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if ((e.ColumnIndex >= 0) && (e.ColumnIndex < ProvinceListColumnCount))
            {
                HoI2Editor.Settings.ProvinceEditor.ListColumnWidth[e.ColumnIndex] =
                    provinceListView.Columns[e.ColumnIndex].Width;
            }
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

            ListViewItem item = new ListViewItem
            {
                Text = province.GetName(),
                Tag = province
            };
            item.SubItems.Add(IntHelper.ToString(province.Id));
            item.SubItems.Add(province.Terrain == TerrainId.Ocean ? Resources.Yes : Resources.No);
            item.SubItems.Add(province.PortAllowed ? Resources.Yes : Resources.No);
            item.SubItems.Add(province.Beaches ? Resources.Yes : Resources.No);
            item.SubItems.Add(DoubleHelper.ToString(province.Infrastructure));
            item.SubItems.Add(DoubleHelper.ToString(province.Ic));
            item.SubItems.Add(DoubleHelper.ToString(province.Manpower));
            item.SubItems.Add(DoubleHelper.ToString(province.Energy));
            item.SubItems.Add(DoubleHelper.ToString(province.Metal));
            item.SubItems.Add(DoubleHelper.ToString(province.RareMaterials));
            item.SubItems.Add(DoubleHelper.ToString(province.Oil));

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
            Graphics g = Graphics.FromHwnd(Handle);
            int margin = DeviceCaps.GetScaledWidth(2) + 1;

            // 大陸
            continentComboBox.BeginUpdate();
            continentComboBox.Items.Clear();
            int width = continentComboBox.Width;
            foreach (ContinentId continent in Provinces.Continents)
            {
                string s = Provinces.GetContinentName(continent);
                continentComboBox.Items.Add(s);
                width = Math.Max(width, (int) g.MeasureString(s, continentComboBox.Font).Width + margin);
            }
            continentComboBox.DropDownWidth = width;
            continentComboBox.EndUpdate();

            // 地方
            regionComboBox.BeginUpdate();
            regionComboBox.Items.Clear();
            width = regionComboBox.Width;
            foreach (RegionId region in Provinces.Regions)
            {
                string s = Provinces.GetRegionName(region);
                regionComboBox.Items.Add(s);
                width = Math.Max(width,
                    (int) g.MeasureString(s, regionComboBox.Font).Width + SystemInformation.VerticalScrollBarWidth +
                    margin);
            }
            regionComboBox.DropDownWidth = width;
            regionComboBox.EndUpdate();

            // 地域
            areaComboBox.BeginUpdate();
            areaComboBox.Items.Clear();
            width = areaComboBox.Width;
            foreach (AreaId area in Provinces.Areas)
            {
                string s = Provinces.GetAreaName(area);
                areaComboBox.Items.Add(s);
                width = Math.Max(width,
                    (int) g.MeasureString(s, areaComboBox.Font).Width + SystemInformation.VerticalScrollBarWidth +
                    margin);
            }
            areaComboBox.DropDownWidth = width;
            areaComboBox.EndUpdate();

            // 気候
            climateComboBox.BeginUpdate();
            climateComboBox.Items.Clear();
            width = climateComboBox.Width;
            foreach (ClimateId climate in Provinces.Climates)
            {
                string s = Provinces.GetClimateName(climate);
                climateComboBox.Items.Add(s);
                width = Math.Max(width, (int) g.MeasureString(s, climateComboBox.Font).Width + margin);
            }
            climateComboBox.DropDownWidth = width;
            climateComboBox.EndUpdate();

            // 地形
            terrainComboBox.BeginUpdate();
            terrainComboBox.Items.Clear();
            width = terrainComboBox.Width;
            foreach (TerrainId terrain in Provinces.Terrains)
            {
                string s = Provinces.GetTerrainName(terrain);
                terrainComboBox.Items.Add(s);
                width = Math.Max(width, (int) g.MeasureString(s, terrainComboBox.Font).Width + margin);
            }
            terrainComboBox.DropDownWidth = width;
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

            // プロヴィンス画像を更新する
            UpdateProvinceImage(province);
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
            infraTextBox.Text = DoubleHelper.ToString(province.Infrastructure);
            icTextBox.Text = DoubleHelper.ToString(province.Ic);
            manpowerTextBox.Text = DoubleHelper.ToString(province.Manpower);
            energyTextBox.Text = DoubleHelper.ToString(province.Energy);
            metalTextBox.Text = DoubleHelper.ToString(province.Metal);
            rareMaterialsTextBox.Text = DoubleHelper.ToString(province.RareMaterials);
            oilTextBox.Text = DoubleHelper.ToString(province.Oil);

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

            infraTextBox.ForeColor = province.IsDirty(ProvinceItemId.Infrastructure)
                ? Color.Red
                : SystemColors.WindowText;
            icTextBox.ForeColor = province.IsDirty(ProvinceItemId.Ic) ? Color.Red : SystemColors.WindowText;
            manpowerTextBox.ForeColor = province.IsDirty(ProvinceItemId.Manpower)
                ? Color.Red
                : SystemColors.WindowText;
            energyTextBox.ForeColor = province.IsDirty(ProvinceItemId.Energy) ? Color.Red : SystemColors.WindowText;
            metalTextBox.ForeColor = province.IsDirty(ProvinceItemId.Metal) ? Color.Red : SystemColors.WindowText;
            rareMaterialsTextBox.ForeColor = province.IsDirty(ProvinceItemId.RareMaterials)
                ? Color.Red
                : SystemColors.WindowText;
            oilTextBox.ForeColor = province.IsDirty(ProvinceItemId.Oil) ? Color.Red : SystemColors.WindowText;

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
            idNumericUpDown.Text = IntHelper.ToString((int) idNumericUpDown.Value);
            beachXNumericUpDown.Text = IntHelper.ToString((int) beachXNumericUpDown.Value);
            beachYNumericUpDown.Text = IntHelper.ToString((int) beachYNumericUpDown.Value);
            beachIconNumericUpDown.Text = IntHelper.ToString((int) beachIconNumericUpDown.Value);
            portXNumericUpDown.Text = IntHelper.ToString((int) portXNumericUpDown.Value);
            portYNumericUpDown.Text = IntHelper.ToString((int) portYNumericUpDown.Value);
            portSeaZoneNumericUpDown.Text = IntHelper.ToString((int) portSeaZoneNumericUpDown.Value);
            cityXNumericUpDown.Text = IntHelper.ToString((int) cityXNumericUpDown.Value);
            cityYNumericUpDown.Text = IntHelper.ToString((int) cityYNumericUpDown.Value);
            fortXNumericUpDown.Text = IntHelper.ToString((int) fortXNumericUpDown.Value);
            fortYNumericUpDown.Text = IntHelper.ToString((int) fortYNumericUpDown.Value);
            aaXNumericUpDown.Text = IntHelper.ToString((int) aaXNumericUpDown.Value);
            aaYNumericUpDown.Text = IntHelper.ToString((int) aaYNumericUpDown.Value);
            armyXNumericUpDown.Text = IntHelper.ToString((int) armyXNumericUpDown.Value);
            armyYNumericUpDown.Text = IntHelper.ToString((int) armyYNumericUpDown.Value);
            counterXNumericUpDown.Text = IntHelper.ToString((int) counterXNumericUpDown.Value);
            counterYNumericUpDown.Text = IntHelper.ToString((int) counterYNumericUpDown.Value);
            fillXNumericUpDown1.Text = IntHelper.ToString((int) fillXNumericUpDown1.Value);
            fillYNumericUpDown1.Text = IntHelper.ToString((int) fillYNumericUpDown1.Value);
            fillXNumericUpDown2.Text = IntHelper.ToString((int) fillXNumericUpDown2.Value);
            fillYNumericUpDown2.Text = IntHelper.ToString((int) fillYNumericUpDown2.Value);
            fillXNumericUpDown3.Text = IntHelper.ToString((int) fillXNumericUpDown3.Value);
            fillYNumericUpDown3.Text = IntHelper.ToString((int) fillYNumericUpDown3.Value);
            fillXNumericUpDown4.Text = IntHelper.ToString((int) fillXNumericUpDown4.Value);
            fillYNumericUpDown4.Text = IntHelper.ToString((int) fillYNumericUpDown4.Value);
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

            infraTextBox.ResetText();
            icTextBox.ResetText();
            manpowerTextBox.ResetText();
            energyTextBox.ResetText();
            metalTextBox.ResetText();
            rareMaterialsTextBox.ResetText();
            oilTextBox.ResetText();

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
            Graphics g = Graphics.FromHwnd(Handle);
            int margin = DeviceCaps.GetScaledWidth(2) + 1;

            portSeaZoneComboBox.BeginUpdate();
            portSeaZoneComboBox.Items.Clear();
            int maxWidth = portSeaZoneComboBox.Width;
            foreach (int id in Provinces.SeaZones)
            {
                Province province = Provinces.SeaZoneMap[id];
                string s = province.GetName();
                portSeaZoneComboBox.Items.Add(s);
                maxWidth = Math.Max(maxWidth,
                    (int) g.MeasureString(s, portSeaZoneComboBox.Font).Width + SystemInformation.VerticalScrollBarWidth +
                    margin);
            }
            portSeaZoneComboBox.DropDownWidth = maxWidth;
            portSeaZoneComboBox.EndUpdate();
        }

        /// <summary>
        ///     プロヴィンス画像を更新する
        /// </summary>
        /// <param name="province">プロヴィンス</param>
        private void UpdateProvinceImage(Province province)
        {
            string fileName = Game.GetReadFileName(Game.GetProvinceImageFileName(province.Id));
            provincePictureBox.ImageLocation = File.Exists(fileName) ? fileName : "";
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
            if (string.IsNullOrEmpty(name))
            {
                if (string.IsNullOrEmpty(province.Name))
                {
                    return;
                }
            }
            else
            {
                if (name.Equals(province.GetName()))
                {
                    return;
                }
            }

            Log.Info("[Province] name: {0} -> {1} ({2})", province.GetName(), name, province.Id);

            // 値を更新する
            Config.SetText(province.Name, name, Game.ProvinceTextFileName);

            // プロヴィンスリストビューの項目を更新する
            provinceListView.SelectedItems[0].SubItems[0].Text = province.GetName();

            // 編集済みフラグを設定する
            province.SetDirty(ProvinceItemId.Name);

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

            Log.Info("[Province] continent: {0} -> {1} ({2}: {3})", Provinces.GetContinentName(province.Continent),
                Provinces.GetContinentName(continent), province.Id, province.GetName());

            // 値を更新する
            Provinces.ModifyContinent(province, continent);

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

            Log.Info("[Province] region: {0} -> {1} ({2}: {3})", Provinces.GetRegionName(province.Region),
                Provinces.GetRegionName(region), province.Id, province.GetName());

            // 値を更新する
            Provinces.ModifyRegion(province, region);

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

            Log.Info("[Province] area: {0} -> {1} ({2}: {3})", Provinces.GetAreaName(province.Area),
                Provinces.GetAreaName(area), province.Id, province.GetName());

            // 値を更新する
            Provinces.ModifyArea(province, area);

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

            Log.Info("[Province] climate: {0} -> {1} ({2}: {3})", Provinces.GetClimateName(province.Climate),
                Provinces.GetClimateName(climate), province.Id, province.GetName());

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

            Log.Info("[Province] terrain: {0} -> {1} ({2}: {3})", Provinces.GetTerrainName(province.Terrain),
                Provinces.GetTerrainName(terrain), province.Id, province.GetName());

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
        private void OnInfraTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(infraTextBox.Text, out val))
            {
                infraTextBox.Text = DoubleHelper.ToString(province.Infrastructure);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, province.Infrastructure))
            {
                return;
            }

            Log.Info("[Province] infrastructure: {0} -> {1} ({2}: {3})", province.Infrastructure, val, province.Id,
                province.GetName());

            // 値を更新する
            province.Infrastructure = val;

            // プロヴィンスリストビューの項目を更新する
            provinceListView.SelectedItems[0].SubItems[5].Text = DoubleHelper.ToString(province.Infrastructure);

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.Infrastructure);

            // 文字色を変更する
            infraTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     IC変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIcTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(icTextBox.Text, out val))
            {
                icTextBox.Text = DoubleHelper.ToString(province.Ic);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, province.Ic))
            {
                return;
            }

            Log.Info("[Province] ic: {0} -> {1} ({2}: {3})", province.Ic, val, province.Id, province.GetName());

            // 値を更新する
            province.Ic = val;

            // プロヴィンスリストビューの項目を更新する
            provinceListView.SelectedItems[0].SubItems[6].Text = DoubleHelper.ToString(province.Ic);

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.Ic);

            // 文字色を変更する
            icTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     労働力変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnManpowerTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(manpowerTextBox.Text, out val))
            {
                manpowerTextBox.Text = DoubleHelper.ToString(province.Manpower);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, province.Manpower))
            {
                return;
            }

            Log.Info("[Province] manpower: {0} -> {1} ({2}: {3})", province.Manpower, val, province.Id,
                province.GetName());

            // 値を更新する
            province.Manpower = val;

            // プロヴィンスリストビューの項目を更新する
            provinceListView.SelectedItems[0].SubItems[7].Text = DoubleHelper.ToString(province.Manpower);

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.Manpower);

            // 文字色を変更する
            manpowerTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     エネルギー変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEnergyTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(energyTextBox.Text, out val))
            {
                energyTextBox.Text = DoubleHelper.ToString(province.Energy);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, province.Energy))
            {
                return;
            }

            Log.Info("[Province] energy: {0} -> {1} ({2}: {3})", province.Energy, val, province.Id, province.GetName());

            // 値を更新する
            province.Energy = val;

            // プロヴィンスリストビューの項目を更新する
            provinceListView.SelectedItems[0].SubItems[8].Text = DoubleHelper.ToString(province.Energy);

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.Energy);

            // 文字色を変更する
            energyTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     金属変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMetalTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(metalTextBox.Text, out val))
            {
                metalTextBox.Text = DoubleHelper.ToString(province.Metal);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, province.Metal))
            {
                return;
            }

            Log.Info("[Province] metal: {0} -> {1} ({2}: {3})", province.Metal, val, province.Id, province.GetName());

            // 値を更新する
            province.Metal = val;

            // プロヴィンスリストビューの項目を更新する
            provinceListView.SelectedItems[0].SubItems[9].Text = DoubleHelper.ToString(province.Metal);

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.Metal);

            // 文字色を変更する
            metalTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     希少資源変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRareMaterialsTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(rareMaterialsTextBox.Text, out val))
            {
                rareMaterialsTextBox.Text = DoubleHelper.ToString(province.RareMaterials);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, province.RareMaterials))
            {
                return;
            }

            Log.Info("[Province] rare materials: {0} -> {1} ({2}: {3})", province.RareMaterials, val, province.Id,
                province.GetName());

            // 値を更新する
            province.RareMaterials = val;

            // プロヴィンスリストビューの項目を更新する
            provinceListView.SelectedItems[0].SubItems[10].Text = DoubleHelper.ToString(province.RareMaterials);

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.RareMaterials);

            // 文字色を変更する
            rareMaterialsTextBox.ForeColor = Color.Red;
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

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(oilTextBox.Text, out val))
            {
                oilTextBox.Text = DoubleHelper.ToString(province.Oil);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, province.Oil))
            {
                return;
            }

            Log.Info("[Province] oil: {0} -> {1} ({2}: {3})", province.Oil, val, province.Id, province.GetName());

            // 値を更新する
            province.Oil = val;

            // プロヴィンスリストビューの項目を更新する
            provinceListView.SelectedItems[0].SubItems[11].Text = DoubleHelper.ToString(province.Oil);

            // 編集済みフラグを設定する
            Provinces.SetDirty();
            province.SetDirty(ProvinceItemId.Oil);

            // 文字色を変更する
            oilTextBox.ForeColor = Color.Red;
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

            Log.Info("[Province] beach: {0} -> {1} ({2}: {3})", BoolHelper.ToString(province.Beaches),
                BoolHelper.ToString(flag), province.Id, province.GetName());

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
            int x = (int) beachXNumericUpDown.Value;
            if (x == province.BeachXPos)
            {
                return;
            }

            Log.Info("[Province] beach position x: {0} -> {1} ({2}: {3})", province.BeachXPos, x, province.Id,
                province.GetName());

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
            int y = (int) beachYNumericUpDown.Value;
            if (y == province.BeachYPos)
            {
                return;
            }

            Log.Info("[Province] beach position y: {0} -> {1} ({2}: {3})", province.BeachYPos, y, province.Id,
                province.GetName());

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
            int icon = (int) beachIconNumericUpDown.Value;
            if (icon == province.BeachIcon)
            {
                return;
            }

            Log.Info("[Province] beach icon: {0} -> {1} ({2}: {3})", province.BeachIcon, icon, province.Id,
                province.GetName());

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

            Log.Info("[Province] port allowed: {0} -> {1} ({2}: {3})", BoolHelper.ToString(province.PortAllowed),
                BoolHelper.ToString(flag), province.Id, province.GetName());

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
            int x = (int) portXNumericUpDown.Value;
            if (x == province.PortXPos)
            {
                return;
            }

            Log.Info("[Province] port position x: {0} -> {1} ({2}: {3})", province.PortXPos, x, province.Id,
                province.GetName());

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
            int y = (int) portYNumericUpDown.Value;
            if (y == province.PortYPos)
            {
                return;
            }

            Log.Info("[Province] port position y: {0} -> {1} ({2}: {3})", province.PortYPos, y, province.Id,
                province.GetName());

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
            int seaZone = (int) portSeaZoneNumericUpDown.Value;
            if (seaZone == province.PortSeaZone)
            {
                return;
            }

            Province oldProv = Provinces.Items.Find(prov => prov.Id == province.PortSeaZone);
            Province newProv = Provinces.Items.Find(prov => prov.Id == seaZone);

            Log.Info("[Province] port sea zone: {0} [{1}] -> {2} [{3}] ({4}: {5})", province.PortSeaZone,
                (oldProv != null) ? oldProv.GetName() : "", seaZone, (newProv != null) ? newProv.GetName() : "",
                province.Id, province.GetName());

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
                    Province seaProvince = Provinces.Items.Find(prov => prov.Id == seaZone);
                    if (seaProvince != null)
                    {
                        portSeaZoneComboBox.Text = seaProvince.GetName();
                    }
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
            int x = (int) cityXNumericUpDown.Value;
            if (x == province.CityXPos)
            {
                return;
            }

            Log.Info("[Province] city position x: {0} -> {1} ({2}: {3})", province.CityXPos, x, province.Id,
                province.GetName());

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
            int y = (int) cityYNumericUpDown.Value;
            if (y == province.CityYPos)
            {
                return;
            }

            Log.Info("[Province] city position y: {0} -> {1} ({2}: {3})", province.CityYPos, y, province.Id,
                province.GetName());

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
            int x = (int) fortXNumericUpDown.Value;
            if (x == province.FortXPos)
            {
                return;
            }

            Log.Info("[Province] fort position x: {0} -> {1} ({2}: {3})", province.FortXPos, x, province.Id,
                province.GetName());

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
            int y = (int) fortYNumericUpDown.Value;
            if (y == province.FortYPos)
            {
                return;
            }

            Log.Info("[Province] fort position y: {0} -> {1} ({2}: {3})", province.FortYPos, y, province.Id,
                province.GetName());

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
            int x = (int) aaXNumericUpDown.Value;
            if (x == province.AaXPos)
            {
                return;
            }

            Log.Info("[Province] aa position x: {0} -> {1} ({2}: {3})", province.AaXPos, x, province.Id,
                province.GetName());

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
            int y = (int) aaYNumericUpDown.Value;
            if (y == province.AaYPos)
            {
                return;
            }

            Log.Info("[Province] aa position y: {0} -> {1} ({2}: {3})", province.AaYPos, y, province.Id,
                province.GetName());

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
            int x = (int) armyXNumericUpDown.Value;
            if (x == province.ArmyXPos)
            {
                return;
            }

            Log.Info("[Province] army position x: {0} -> {1} ({2}: {3})", province.ArmyXPos, x, province.Id,
                province.GetName());

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
            int y = (int) armyYNumericUpDown.Value;
            if (y == province.ArmyYPos)
            {
                return;
            }

            Log.Info("[Province] army position y: {0} -> {1} ({2}: {3})", province.ArmyYPos, y, province.Id,
                province.GetName());

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
            int x = (int) counterXNumericUpDown.Value;
            if (x == province.CounterXPos)
            {
                return;
            }

            Log.Info("[Province] counter position x: {0} -> {1} ({2}: {3})", province.CounterXPos, x, province.Id,
                province.GetName());

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
            int y = (int) counterYNumericUpDown.Value;
            if (y == province.CounterYPos)
            {
                return;
            }

            Log.Info("[Province] counter position y: {0} -> {1} ({2}: {3})", province.CounterYPos, y, province.Id,
                province.GetName());

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
            int x = (int) fillXNumericUpDown1.Value;
            if (x == province.FillCoordX1)
            {
                return;
            }

            Log.Info("[Province] fill coord position x1: {0} -> {1} ({2}: {3})", province.FillCoordX1, x, province.Id,
                province.GetName());

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
            int y = (int) fillYNumericUpDown1.Value;
            if (y == province.FillCoordY1)
            {
                return;
            }

            Log.Info("[Province] fill coord position y1: {0} -> {1} ({2}: {3})", province.FillCoordY1, y, province.Id,
                province.GetName());

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
            int x = (int) fillXNumericUpDown2.Value;
            if (x == province.FillCoordX2)
            {
                return;
            }

            Log.Info("[Province] fill coord position x2: {0} -> {1} ({2}: {3})", province.FillCoordX2, x, province.Id,
                province.GetName());

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
            int y = (int) fillYNumericUpDown2.Value;
            if (y == province.FillCoordY2)
            {
                return;
            }

            Log.Info("[Province] fill coord position y2: {0} -> {1} ({2}: {3})", province.FillCoordY2, y, province.Id,
                province.GetName());

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
            int x = (int) fillXNumericUpDown3.Value;
            if (x == province.FillCoordX3)
            {
                return;
            }

            Log.Info("[Province] fill coord position x3: {0} -> {1} ({2}: {3})", province.FillCoordX3, x, province.Id,
                province.GetName());

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
            int y = (int) fillYNumericUpDown3.Value;
            if (y == province.FillCoordY3)
            {
                return;
            }

            Log.Info("[Province] fill coord position y3: {0} -> {1} ({2}: {3})", province.FillCoordY3, y, province.Id,
                province.GetName());

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
            int x = (int) fillXNumericUpDown4.Value;
            if (x == province.FillCoordX4)
            {
                return;
            }

            Log.Info("[Province] fill coord position x4: {0} -> {1} ({2}: {3})", province.FillCoordX4, x, province.Id,
                province.GetName());

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
            int y = (int) fillYNumericUpDown4.Value;
            if (y == province.FillCoordY4)
            {
                return;
            }

            Log.Info("[Province] fill coord position y4: {0} -> {1} ({2}: {3})", province.FillCoordY4, y, province.Id,
                province.GetName());

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