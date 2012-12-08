using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HoI2Editor.Models;
using HoI2Editor.Properties;

namespace HoI2Editor.Forms
{
    /// <summary>
    ///     技術ツリーエディタのフォーム
    /// </summary>
    public partial class TechEditorForm : Form
    {
        #region フィールド

        /// <summary>
        ///     技術IDの対応付けテーブル
        /// </summary>
        private static readonly List<KeyValuePair<int, Tech>> TechIdMap = new List<KeyValuePair<int, Tech>>();

        /// <summary>
        ///     技術ラベルの画像
        /// </summary>
        private static Bitmap _techLabelBitmap;

        /// <summary>
        ///     イベントラベルの画像
        /// </summary>
        private static Bitmap _eventLabelBitmap;

        /// <summary>
        ///     技術ラベルの描画領域
        /// </summary>
        private static readonly Region TechLabelRegion = new Region(new Rectangle(0, 0, TechLabelWidth, TechLabelHeight));

        /// <summary>
        ///     イベントラベルの描画領域
        /// </summary>
        private static readonly Region EventLabelRegion =
            new Region(new Rectangle(0, 0, EventLabelWidth, EventLabelHeight));

        #endregion

        #region 定数

        /// <summary>
        ///     技術ラベルの幅
        /// </summary>
        private const int TechLabelWidth = 112;

        /// <summary>
        ///     技術ラベルの高さ
        /// </summary>
        private const int TechLabelHeight = 16;

        /// <summary>
        ///     イベントラベルの幅
        /// </summary>
        private const int EventLabelWidth = 112;

        /// <summary>
        ///     イベントラベルの高さ
        /// </summary>
        private const int EventLabelHeight = 24;

        /// <summary>
        ///     技術ツリー画像ファイル名テーブル
        /// </summary>
        private static readonly string[] TechTreeFileNames =
            {
                "techtree_infantry.bmp",
                "techtree_armor.bmp",
                "techtree_naval.bmp",
                "techtree_aircraft.bmp",
                "techtree_industry.bmp",
                "techtree_land_doctrine.bmp",
                "techtree_secret_weapons.bmp",
                "techtree_naval_doctrines.bmp",
                "techtree_air_doctrines.bmp"
            };

        #endregion

        #region コンストラクタ

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public TechEditorForm()
        {
            InitializeComponent();
        }

        #endregion

        #region 初期化

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechEditorFormLoad(object sender, EventArgs e)
        {
            // 研究特性を初期化する
            InitSpecialities();

            // 技術定義ファイルを読み込む
            LoadTechFiles();

            // ラベル画像を読み込む
            InitLabelBitmap();

            // 編集可能な項目を初期化する
            InitEditableItems();

            // カテゴリリストボックスを初期化する
            InitCategoryList();
        }

        /// <summary>
        ///     技術IDの対応付けテーブルを初期化する
        /// </summary>
        private static void InitTechIdMap()
        {
            TechIdMap.Clear();

            foreach (Tech item in Techs.List.SelectMany(group => group.Items.OfType<Tech>()))
            {
                TechIdMap.Add(new KeyValuePair<int, Tech>(item.Id, item));
            }
        }

        /// <summary>
        ///     編集項目を初期化する
        /// </summary>
        private void InitEditableItems()
        {
            // 小研究特性
            int maxSize = componentSpecialityComboBox.DropDownWidth;
            foreach (
                string name in
                    Techs.SpecialityTable.Select(
                        speciality => Config.GetText(Tech.SpecialityNameTable[(int) speciality])))
            {
                componentSpecialityComboBox.Items.Add(name);
                maxSize = Math.Max(maxSize,
                                   TextRenderer.MeasureText(name, componentSpecialityComboBox.Font).Width +
                                   SystemInformation.VerticalScrollBarWidth);
            }
            componentSpecialityComboBox.DropDownWidth = maxSize;

            // 技術効果種類
            maxSize = commandTypeComboBox.DropDownWidth;
            foreach (string name in Command.TypeStringTable)
            {
                commandTypeComboBox.Items.Add(name);
                maxSize = Math.Max(maxSize,
                                   TextRenderer.MeasureText(name, commandTypeComboBox.Font).Width +
                                   SystemInformation.VerticalScrollBarWidth);
            }
            commandTypeComboBox.DropDownWidth = maxSize;
        }

        /// <summary>
        ///     研究特性を初期化する
        /// </summary>
        private static void InitSpecialities()
        {
            // ゲームの種類に合わせて研究特性を初期化する
            Techs.InitSpecialities();
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

        #region 技術定義データ操作

        /// <summary>
        ///     再読み込みボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReloadButtonClick(object sender, EventArgs e)
        {
            LoadTechFiles();
        }

        /// <summary>
        ///     保存ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            SaveTechFiles();
        }

        /// <summary>
        ///     技術定義ファイルを読み込む
        /// </summary>
        private void LoadTechFiles()
        {
            // 技術定義ファイルを読み込む
            Techs.LoadTechFiles();

            // 技術IDの対応付けテーブルを初期化する
            InitTechIdMap();

            // 編集済みフラグがクリアされるため表示を更新する
            categoryListBox.Update();
        }

        /// <summary>
        ///     技術定義ファイルを保存する
        /// </summary>
        private void SaveTechFiles()
        {
            // 技術定義ファイルを保存する
            Techs.SaveTechFiles();

            // 編集済みフラグがクリアされるため表示を更新する
            categoryListBox.Update();
        }

        /// <summary>
        ///     編集済みフラグをセットする
        /// </summary>
        private void SetDirtyFlag()
        {
            var category = (TechCategory) categoryListBox.SelectedIndex;
            Techs.SetDirtyFlag(category);
        }

        #endregion

        #region カテゴリリスト

        /// <summary>
        ///     カテゴリリストボックスを初期化する
        /// </summary>
        private void InitCategoryList()
        {
            foreach (TechGroup group in Techs.List)
            {
                categoryListBox.Items.Add(Config.GetText(group.Name));
            }
            categoryListBox.SelectedIndex = 0;
        }

        /// <summary>
        ///     カテゴリリストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCategoryListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateItemList();
            UpdateCategoryTabItems();

            DisableEventItems();
            DisableLabelItems();
            DisableTechItems();

            editTabControl.SelectedIndex = 0;
        }

        #endregion

        #region 項目リスト

        /// <summary>
        ///     項目リストの表示を更新する
        /// </summary>
        private void UpdateItemList()
        {
            techListBox.BeginUpdate();

            techListBox.Items.Clear();
            treePictureBox.Controls.Clear();

            foreach (object item in Techs.List[categoryListBox.SelectedIndex].Items)
            {
                techListBox.Items.Add(item);
                AddTechTreeItems(item);
            }

            techListBox.EndUpdate();
        }

        /// <summary>
        ///     項目リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                DisableTechItems();
                DisableLabelItems();
                DisableEventItems();

                editTabControl.SelectedIndex = 0;

                return;
            }

            TechGroup group = Techs.List[categoryListBox.SelectedIndex];
            object item = group.Items[techListBox.SelectedIndex];

            if (item is Tech)
            {
                UpdateTechItems(item as Tech);

                EnableTechItems();
                DisableLabelItems();
                DisableEventItems();

                if (editTabControl.SelectedIndex != 1 &&
                    editTabControl.SelectedIndex != 2 &&
                    editTabControl.SelectedIndex != 3 &&
                    editTabControl.SelectedIndex != 4)
                {
                    editTabControl.SelectedIndex = 1;
                }
            }
            else if (item is TechLabel)
            {
                UpdateLabelItems(item as TechLabel);

                DisableTechItems();
                EnableLabelItems();
                DisableEventItems();

                editTabControl.SelectedIndex = 5;
            }
            else if (item is TechEvent)
            {
                UpdateEventItems(item as TechEvent);

                DisableTechItems();
                DisableLabelItems();
                EnableEventItems();

                editTabControl.SelectedIndex = 6;
            }
        }

        /// <summary>
        ///     項目リストの新規技術ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNewTechButtonClick(object sender, EventArgs e)
        {
            var category = (TechCategory) categoryListBox.SelectedIndex;
            var item = new Tech
                           {
                               Year = 1936,
                               Name = Config.GetTempKey(Game.TechTextFileName),
                               Desc = Config.GetTempKey(Game.TechTextFileName)
                           };
            item.ShortName = "SHORT_" + item.Name;
            Config.SetText(item.Name, "");
            Config.SetText(item.ShortName, "");
            Config.SetText(item.Desc, "");

            if (techListBox.SelectedIndex > 0 && techListBox.SelectedItem is Tech)
            {
                var selected = techListBox.SelectedItem as Tech;

                item.Id = selected.Id + 10;
                item.Positions.Add(new TechPosition {X = selected.Positions[0].X, Y = selected.Positions[0].Y});

                Techs.InsertItemNext(category, item, selected);
                TechIdMap.Add(new KeyValuePair<int, Tech>(item.Id, item));

                InsertTechListItem(item, techListBox.SelectedIndex + 1);
            }
            else
            {
                item.Id = 0;
                item.Positions.Add(new TechPosition {X = 0, Y = 0});

                Techs.AddItem(category, item);
                TechIdMap.Add(new KeyValuePair<int, Tech>(item.Id, item));

                AddTechListItem(item);
            }

            foreach (TechPosition position in item.Positions)
            {
                AddTechTreeTechItem(item, position);
            }

            SetDirtyFlag();
        }

        /// <summary>
        ///     項目リストの新規ラベルボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNewLabelButtonClick(object sender, EventArgs e)
        {
            var category = (TechCategory) categoryListBox.SelectedIndex;
            var item = new TechLabel {Tag = Config.GetTempKey(Game.TechTextFileName)};
            Config.SetText(item.Tag, "");
            if (techListBox.SelectedIndex > 0 && techListBox.SelectedItem is TechLabel)
            {
                var selected = techListBox.SelectedItem as TechLabel;

                item.Positions.Add(new TechPosition {X = selected.Positions[0].X, Y = selected.Positions[0].Y});

                Techs.InsertItemNext(category, item, selected);
                InsertTechListItem(item, techListBox.SelectedIndex + 1);
            }
            else
            {
                item.Positions.Add(new TechPosition {X = 0, Y = 0});

                Techs.AddItem(category, item);
                AddTechListItem(item);
            }

            foreach (TechPosition position in item.Positions)
            {
                AddTechTreeLabelItem(item, position);
            }

            SetDirtyFlag();
        }

        /// <summary>
        ///     項目リストの新規イベントボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNewEventButtonClick(object sender, EventArgs e)
        {
            var category = (TechCategory) categoryListBox.SelectedIndex;
            TechEvent item;
            if (techListBox.SelectedIndex > 0 && techListBox.SelectedItem is TechEvent)
            {
                var selected = techListBox.SelectedItem as TechEvent;

                item = new TechEvent
                           {
                               Id = selected.Id + 1,
                           };
                item.Positions.Add(new TechPosition {X = selected.Positions[0].X, Y = selected.Positions[0].Y});

                Techs.InsertItemNext(category, item, selected);

                InsertTechListItem(item, techListBox.SelectedIndex + 1);
            }
            else
            {
                item = new TechEvent
                           {
                               Id = 0,
                           };

                Techs.AddItem(category, item);

                AddTechListItem(item);
                techListBox.SelectedIndex = 0;
            }

            foreach (TechPosition position in item.Positions)
            {
                AddTechTreeEventItem(item, position);
            }

            SetDirtyFlag();
        }

        /// <summary>
        ///     項目リストの複製ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloneButtonClick(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var category = (TechCategory) categoryListBox.SelectedIndex;
            if (techListBox.SelectedItem is Tech)
            {
                var selected = techListBox.SelectedItem as Tech;
                Tech item = selected.Clone();

                Techs.InsertItemNext(category, item, selected);
                InsertTechListItem(item, techListBox.SelectedIndex + 1);

                foreach (TechPosition position in item.Positions)
                {
                    AddTechTreeTechItem(item, position);
                }
            }
            else if (techListBox.SelectedItem is TechLabel)
            {
                var selected = techListBox.SelectedItem as TechLabel;
                TechLabel item = selected.Clone();

                Techs.InsertItemNext(category, item, selected);
                InsertTechListItem(item, techListBox.SelectedIndex + 1);

                foreach (TechPosition position in item.Positions)
                {
                    AddTechTreeLabelItem(item, position);
                }
            }
            else if (techListBox.SelectedItem is TechEvent)
            {
                var selected = techListBox.SelectedItem as TechEvent;
                TechEvent item = selected.Clone();

                Techs.InsertItemNext(category, item, selected);
                InsertTechListItem(item, techListBox.SelectedIndex + 1);

                foreach (TechPosition position in item.Positions)
                {
                    AddTechTreeEventItem(item, position);
                }
            }

            SetDirtyFlag();
        }

        /// <summary>
        ///     項目リストの削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRemoveButtonClick(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var category = (TechCategory) categoryListBox.SelectedIndex;
            object item = techListBox.SelectedItem;
            Techs.RemoveItem(category, item);
            InitTechIdMap();
            if (item is Tech)
            {
                List<KeyValuePair<int, Tech>> list = TechIdMap.Where(pair => pair.Value == item).ToList();
                foreach (var pair in list)
                {
                    TechIdMap.Remove(pair);
                }
            }

            RemoveTechTreeItems(item);
            RemoveTechListItem(techListBox.SelectedIndex);

            if (techListBox.Items.Count == 0)
            {
                DisableEventItems();
                DisableLabelItems();
                DisableTechItems();

                editTabControl.SelectedIndex = 0;
            }

            SetDirtyFlag();
        }

        /// <summary>
        ///     項目リストの先頭へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTopButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            int index = techListBox.SelectedIndex;
            if (index == -1)
            {
                return;
            }

            // 選択項目がリストの先頭ならば何もしない
            if (index == 0)
            {
                return;
            }

            var category = (TechCategory) categoryListBox.SelectedIndex;
            TechGroup group = Techs.List[(int) category];
            object selected = group.Items[index];
            object top = group.Items[0];

            Techs.MoveItem(category, selected, top);

            MoveTechListItem(index, 0);

            SetDirtyFlag();
        }

        /// <summary>
        ///     項目リストの上へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            int index = techListBox.SelectedIndex;
            if (index == -1)
            {
                return;
            }

            // 選択項目がリストの先頭ならば何もしない
            if (index == 0)
            {
                return;
            }

            var category = (TechCategory) categoryListBox.SelectedIndex;
            TechGroup group = Techs.List[(int) category];
            object selected = group.Items[index];
            object upper = group.Items[index - 1];

            Techs.MoveItem(category, selected, upper);

            MoveTechListItem(index, index - 1);

            SetDirtyFlag();
        }

        /// <summary>
        ///     項目リストの下へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDownButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            int index = techListBox.SelectedIndex;
            if (index == -1)
            {
                return;
            }

            // 選択項目がリストの末尾ならば何もしない
            if (index == techListBox.Items.Count - 1)
            {
                return;
            }

            var category = (TechCategory) categoryListBox.SelectedIndex;
            TechGroup group = Techs.List[(int) category];
            object selected = group.Items[index];
            object lower = group.Items[index + 1];

            Techs.MoveItem(category, selected, lower);

            MoveTechListItem(index, index + 1);

            SetDirtyFlag();
        }

        /// <summary>
        ///     項目リストの末尾へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBottomButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            int index = techListBox.SelectedIndex;
            if (index == -1)
            {
                return;
            }

            // 選択項目がリストの末尾ならば何もしない
            if (index == techListBox.Items.Count - 1)
            {
                return;
            }

            var category = (TechCategory) categoryListBox.SelectedIndex;
            TechGroup group = Techs.List[(int) category];
            object selected = group.Items[index];
            object bottom = group.Items[techListBox.Items.Count - 1];

            Techs.MoveItem(category, selected, bottom);

            MoveTechListItem(index, techListBox.Items.Count - 1);

            SetDirtyFlag();
        }

        /// <summary>
        ///     項目リストに項目を追加する
        /// </summary>
        /// <param name="item">追加対象の項目</param>
        private void AddTechListItem(object item)
        {
            techListBox.Items.Add(item);

            techListBox.SelectedIndex = techListBox.Items.Count - 1;
        }

        /// <summary>
        ///     項目リストに項目を挿入する
        /// </summary>
        /// <param name="item">挿入対象の項目</param>
        /// <param name="index">挿入先の位置</param>
        private void InsertTechListItem(object item, int index)
        {
            techListBox.Items.Insert(index, item);

            techListBox.SelectedIndex = index;
        }

        /// <summary>
        ///     項目リストの項目を削除する
        /// </summary>
        /// <param name="index">削除対象の位置</param>
        private void RemoveTechListItem(int index)
        {
            techListBox.Items.RemoveAt(index);

            if (index < techListBox.Items.Count)
            {
                techListBox.SelectedIndex = index;
            }
            else if (index - 1 >= 0)
            {
                techListBox.SelectedIndex = index - 1;
            }
        }

        /// <summary>
        ///     項目リストの項目を移動する
        /// </summary>
        /// <param name="src">移動元の位置</param>
        /// <param name="dest">移動先の位置</param>
        private void MoveTechListItem(int src, int dest)
        {
            object item = techListBox.Items[src];

            if (src > dest)
            {
                // 上へ移動する場合
                techListBox.Items.Insert(dest, item);
                techListBox.Items.RemoveAt(src + 1);
            }
            else
            {
                // 下へ移動する場合
                techListBox.Items.Insert(dest + 1, item);
                techListBox.Items.RemoveAt(src);
            }

            techListBox.SelectedIndex = dest;
        }

        #endregion

        #region 技術ツリー

        /// <summary>
        ///     技術ツリーに項目群を追加する
        /// </summary>
        /// <param name="item">追加対象の項目</param>
        private void AddTechTreeItems(object item)
        {
            if (item is Tech)
            {
                var techItem = item as Tech;
                foreach (TechPosition position in techItem.Positions)
                {
                    AddTechTreeTechItem(techItem, position);
                }
            }
            else if (item is TechLabel)
            {
                var labelItem = item as TechLabel;
                foreach (TechPosition position in labelItem.Positions)
                {
                    AddTechTreeLabelItem(labelItem, position);
                }
            }
            else if (item is TechEvent)
            {
                var eventItem = item as TechEvent;
                foreach (TechPosition position in eventItem.Positions)
                {
                    AddTechTreeEventItem(eventItem, position);
                }
            }
        }

        /// <summary>
        ///     技術ツリーに技術項目を追加する
        /// </summary>
        /// <param name="item">追加対象の項目</param>
        /// <param name="position">追加対象の位置</param>
        private void AddTechTreeTechItem(Tech item, TechPosition position)
        {
            var label = new Label
                            {
                                Location = new Point(position.X, position.Y),
                                Size = new Size(TechLabelWidth, TechLabelHeight),
                                BackColor = Color.Transparent,
                                Image = _techLabelBitmap,
                                Region = TechLabelRegion,
                                Tag = new TechLabelInfo {Item = item, Position = position},
                            };
            label.Paint += OnTechLabelPaint;
            treePictureBox.Controls.Add(label);
        }

        /// <summary>
        ///     技術ツリーに技術ラベル項目を追加する
        /// </summary>
        /// <param name="item">追加対象の項目</param>
        /// <param name="position">追加対象の位置</param>
        private void AddTechTreeLabelItem(TechLabel item, TechPosition position)
        {
            var label = new Label
                            {
                                Location = new Point(position.X, position.Y),
                                BackColor = Color.Transparent,
                                Tag = new TechLabelInfo {Item = item, Position = position},
                            };
            label.Size = TextRenderer.MeasureText(Config.GetText(item.Tag), label.Font);
            label.Paint += OnLabelLabelPaint;
            treePictureBox.Controls.Add(label);
        }

        /// <summary>
        ///     技術ツリーに技術イベント項目を追加する
        /// </summary>
        /// <param name="item">追加対象の項目</param>
        /// <param name="position">追加対象の位置</param>
        private void AddTechTreeEventItem(TechEvent item, TechPosition position)
        {
            var label = new Label
                            {
                                Location = new Point(position.X, position.Y),
                                Size = new Size(EventLabelWidth, EventLabelHeight),
                                BackColor = Color.Transparent,
                                Image = _eventLabelBitmap,
                                Region = EventLabelRegion,
                                Tag = new TechLabelInfo {Item = item, Position = position},
                            };
            treePictureBox.Controls.Add(label);
        }

        /// <summary>
        ///     技術ツリーの項目群を削除する
        /// </summary>
        /// <param name="item">削除対象の項目</param>
        private void RemoveTechTreeItems(object item)
        {
            Control.ControlCollection labels = treePictureBox.Controls;
            foreach (Label label in labels)
            {
                var info = label.Tag as TechLabelInfo;
                if (info == null)
                {
                    continue;
                }
                if (info.Item == item)
                {
                    treePictureBox.Controls.Remove(label);
                }
            }
        }

        /// <summary>
        ///     技術ツリーの項目を削除する
        /// </summary>
        /// <param name="item">削除対象の項目</param>
        /// <param name="position">削除対象の位置</param>
        private void RemoveTechTreeItem(object item, TechPosition position)
        {
            Control.ControlCollection labels = treePictureBox.Controls;
            foreach (Label label in labels)
            {
                var info = label.Tag as TechLabelInfo;
                if (info == null)
                {
                    continue;
                }
                if (info.Item == item && info.Position == position)
                {
                    treePictureBox.Controls.Remove(label);
                }
            }
        }

        /// <summary>
        ///     技術ラベル描画時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechLabelPaint(object sender, PaintEventArgs e)
        {
            var label = sender as Label;
            if (label == null)
            {
                return;
            }

            var info = label.Tag as TechLabelInfo;
            if (info == null)
            {
                return;
            }

            var item = info.Item as Tech;
            if (item == null)
            {
                return;
            }

            string s = Config.GetText(item.ShortName);
            if (string.IsNullOrEmpty(s))
            {
                return;
            }

            Brush brush = new SolidBrush(Color.Black);
            e.Graphics.DrawString(s, label.Font, brush, 6, 2);
            brush.Dispose();
        }

        /// <summary>
        ///     ラベル描画時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnLabelLabelPaint(object sender, PaintEventArgs e)
        {
            var label = sender as Label;
            if (label == null)
            {
                return;
            }

            var info = label.Tag as TechLabelInfo;
            if (info == null)
            {
                return;
            }

            var item = info.Item as TechLabel;
            if (item == null)
            {
                return;
            }

            string s = Config.GetText(item.Tag);
            if (string.IsNullOrEmpty(s))
            {
                return;
            }

            Brush brush;
            if ((s[0] == 'ｧ' || s[0] == '§') &&
                s.Length > 4 &&
                s[1] >= '0' && s[1] <= '9' &&
                s[2] >= '0' && s[2] <= '9' &&
                s[3] >= '0' && s[3] <= '9')
            {
                brush = new SolidBrush(Color.FromArgb((s[3] - '0') << 5, (s[2] - '0') << 5, (s[1] - '0') << 5));
                s = s.Substring(4);
            }
            else
            {
                brush = new SolidBrush(Color.White);
            }
            e.Graphics.DrawString(s, label.Font, brush, -2, 0);
            brush.Dispose();
        }

        /// <summary>
        ///     ラベル画像を初期化する
        /// </summary>
        private static void InitLabelBitmap()
        {
            // 技術
            var bitmap = new Bitmap(Game.GetFileName(Game.TechLabelPathName));
            _techLabelBitmap = bitmap.Clone(new Rectangle(0, 0, TechLabelWidth, TechLabelHeight), bitmap.PixelFormat);
            bitmap.Dispose();
            Color transparent = _techLabelBitmap.GetPixel(0, 0);
            for (int x = 0; x < _techLabelBitmap.Width; x++)
            {
                for (int y = 0; y < _techLabelBitmap.Height; y++)
                {
                    if (_techLabelBitmap.GetPixel(x, y) == transparent)
                    {
                        TechLabelRegion.Exclude(new Rectangle(x, y, 1, 1));
                    }
                }
            }

            // 技術イベント
            bitmap = new Bitmap(Game.GetFileName(Game.SecretLabelPathName));
            _eventLabelBitmap = bitmap.Clone(new Rectangle(0, 0, EventLabelWidth, EventLabelHeight), bitmap.PixelFormat);
            bitmap.Dispose();
            _eventLabelBitmap.MakeTransparent(_eventLabelBitmap.GetPixel(0, 0));
            transparent = _eventLabelBitmap.GetPixel(0, 0);
            for (int x = 0; x < _eventLabelBitmap.Width; x++)
            {
                for (int y = 0; y < _eventLabelBitmap.Height; y++)
                {
                    if (_eventLabelBitmap.GetPixel(x, y) == transparent)
                    {
                        EventLabelRegion.Exclude(new Rectangle(x, y, 1, 1));
                    }
                }
            }
        }

        /// <summary>
        ///     技術ラベルに関連付けられる情報
        /// </summary>
        private class TechLabelInfo
        {
            /// <summary>
            ///     技術項目
            /// </summary>
            internal object Item;

            /// <summary>
            ///     位置
            /// </summary>
            internal TechPosition Position;
        }

        #endregion

        #region カテゴリタブ

        /// <summary>
        ///     カテゴリタブの項目を更新する
        /// </summary>
        private void UpdateCategoryTabItems()
        {
            // 技術ツリー画像
            int index = categoryListBox.SelectedIndex;
            treePictureBox.ImageLocation = Game.GetFileName(Path.Combine(Game.PicturePathName, TechTreeFileNames[index]));

            // カテゴリタブの編集項目
            TechGroup group = Techs.List[index];
            categoryNameTextBox.Text = Config.GetText(group.Name);
            categoryDescTextBox.Text = Config.GetText(group.Desc);
        }

        /// <summary>
        ///     技術グループ名変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCategoryNameTextBoxTextChanged(object sender, EventArgs e)
        {
            var category = (TechCategory) categoryListBox.SelectedIndex;
            TechGroup group = Techs.List[(int) category];

            Config.SetText(group.Name, categoryNameTextBox.Text);

            // カテゴリリストボックスの項目を再設定することで表示更新している
            // この時再選択によりフォーカスが外れるので、イベントハンドラを一時的に無効化する
            categoryListBox.SelectedIndexChanged -= OnCategoryListBoxSelectedIndexChanged;
            categoryListBox.Items[(int) category] = Config.GetText(group.Name);
            categoryListBox.SelectedIndexChanged += OnCategoryListBoxSelectedIndexChanged;

            SetDirtyFlag();
            Config.SetDirtyFlag(Game.TechTextFileName);
        }

        /// <summary>
        ///     技術グループ説明変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCategoryDescTextBoxTextChanged(object sender, EventArgs e)
        {
            var category = (TechCategory) categoryListBox.SelectedIndex;
            Config.SetText(Techs.List[(int) category].Desc, categoryDescTextBox.Text);

            SetDirtyFlag();
            Config.SetDirtyFlag(Game.TechTextFileName);
        }

        #endregion

        #region 技術タブ

        /// <summary>
        ///     技術タブの項目を更新する
        /// </summary>
        /// <param name="item">技術</param>
        private void UpdateTechItems(Tech item)
        {
            // 技術タブの編集項目
            techNameTextBox.Text = Config.GetText(item.Name);
            techShortNameTextBox.Text = Config.GetText(item.ShortName);
            techIdNumericUpDown.Value = item.Id;
            techYearNumericUpDown.Value = item.Year;
            UpdateTechPositionList(item);
            UpdateTechPicture(item);

            // 必要研究タブの編集項目
            UpdateAndRequiredList(item);
            UpdateOrRequiredList(item);

            // 小研究タブの編集項目
            UpdateComponentList(item);

            // 効果タブの編集項目
            UpdateEffectList(item);
        }

        /// <summary>
        ///     技術の編集項目を有効化する
        /// </summary>
        private void EnableTechItems()
        {
            // タブの有効化
            editTabControl.TabPages[1].Enabled = true;
            editTabControl.TabPages[2].Enabled = true;
            editTabControl.TabPages[3].Enabled = true;
            editTabControl.TabPages[4].Enabled = true;
        }

        /// <summary>
        ///     技術の編集項目を無効化する
        /// </summary>
        private void DisableTechItems()
        {
            // タブの無効化
            editTabControl.TabPages[1].Enabled = false;
            editTabControl.TabPages[2].Enabled = false;
            editTabControl.TabPages[3].Enabled = false;
            editTabControl.TabPages[4].Enabled = false;

            // 技術タブの設定項目初期化
            techNameTextBox.Text = "";
            techShortNameTextBox.Text = "";
            techIdNumericUpDown.Value = 0;
            techYearNumericUpDown.Value = 1936;
            techPositionListView.Items.Clear();
            techXNumericUpDown.Value = 0;
            techYNumericUpDown.Value = 0;
            techPictureBox.Image = null;

            // 必要研究タブの設定項目初期化
            andRequiredListView.Items.Clear();
            orRequiredListView.Items.Clear();
            andIdNumericUpDown.Value = 0;
            orIdNumericUpDown.Value = 0;

            // 小研究タブの設定項目初期化
            componentListView.Items.Clear();
            componentIdNumericUpDown.Value = 0;
            componentNameTextBox.Text = "";
            componentDifficultyNumericUpDown.Value = 0;
            componentDoubleTimeCheckBox.Checked = false;
        }

        /// <summary>
        ///     技術座標リストを更新する
        /// </summary>
        /// <param name="item">技術</param>
        private void UpdateTechPositionList(Tech item)
        {
            if (item == null)
            {
                return;
            }

            techPositionListView.BeginUpdate();
            techPositionListView.Items.Clear();

            foreach (TechPosition position in item.Positions)
            {
                var listItem = new ListViewItem(position.X.ToString(CultureInfo.InvariantCulture));
                listItem.SubItems.Add(position.Y.ToString(CultureInfo.InvariantCulture));
                techPositionListView.Items.Add(listItem);
            }

            if (techPositionListView.Items.Count > 0)
            {
                techPositionListView.Items[0].Focused = true;
                techPositionListView.Items[0].Selected = true;

                techXNumericUpDown.Enabled = true;
                techYNumericUpDown.Enabled = true;
            }
            else
            {
                techXNumericUpDown.Value = 0;
                techYNumericUpDown.Value = 0;

                techXNumericUpDown.Enabled = false;
                techYNumericUpDown.Enabled = false;
            }

            techPositionListView.EndUpdate();
        }

        /// <summary>
        ///     技術名変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechNameTextBoxTextChanged(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            string newText = techNameTextBox.Text;
            if (newText.Equals(Config.GetText(item.Name)))
            {
                return;
            }

            Config.SetText(item.Name, newText);

            // 項目リストボックスの項目を再設定することで表示更新している
            // この時再選択によりフォーカスが外れるので、イベントハンドラを一時的に無効化する
            techListBox.SelectedIndexChanged -= OnTechListBoxSelectedIndexChanged;
            techListBox.Items[techListBox.SelectedIndex] = item;
            techListBox.SelectedIndexChanged += OnTechListBoxSelectedIndexChanged;

            SetDirtyFlag();
            Config.SetDirtyFlag(Game.TechTextFileName);
        }

        /// <summary>
        ///     技術短縮名変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechShortNameTextBoxTextChanged(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            string newText = techShortNameTextBox.Text;
            if (newText.Equals(Config.GetText(item.ShortName)))
            {
                return;
            }

            Config.SetText(item.ShortName, newText);

            foreach (Label label in treePictureBox.Controls)
            {
                var info = label.Tag as TechLabelInfo;
                if (info != null && info.Item == item)
                {
                    label.Refresh();
                }
            }

            SetDirtyFlag();
            Config.SetDirtyFlag(Game.TechTextFileName);
        }

        /// <summary>
        ///     技術ID変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechIdNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            var newId = (int) techIdNumericUpDown.Value;
            if (newId == item.Id)
            {
                return;
            }

            List<KeyValuePair<int, Tech>> list = TechIdMap.Where(pair => pair.Value == item).ToList();
            foreach (var pair in list)
            {
                TechIdMap.Remove(pair);
            }
            item.Id = newId;
            TechIdMap.Add(new KeyValuePair<int, Tech>(item.Id, item));

            SetDirtyFlag();
        }

        /// <summary>
        ///     史実年度変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechYearNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            var newYear = (int) techYearNumericUpDown.Value;
            if (newYear == item.Year)
            {
                return;
            }

            item.Year = newYear;

            SetDirtyFlag();
        }

        /// <summary>
        ///     技術座標リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechPositionListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            if (techPositionListView.SelectedIndices.Count == 0)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            int index = techPositionListView.SelectedIndices[0];
            techXNumericUpDown.Value = item.Positions[index].X;
            techYNumericUpDown.Value = item.Positions[index].Y;
        }

        /// <summary>
        ///     技術X座標変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechXNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            if (techPositionListView.SelectedIndices.Count == 0)
            {
                return;
            }

            TechPosition position = item.Positions[techPositionListView.SelectedIndices[0]];

            // 値に変化がなければ何もせずに戻る
            var newX = (int) techXNumericUpDown.Value;
            if (newX == position.X)
            {
                return;
            }

            position.X = newX;

            UpdateTechPositionList(item);

            foreach (Label label in treePictureBox.Controls)
            {
                var info = label.Tag as TechLabelInfo;
                if (info != null && info.Position == position)
                {
                    label.Location = new Point(position.X, position.Y);
                }
            }

            SetDirtyFlag();
        }

        /// <summary>
        ///     技術Y座標変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechYNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            if (techPositionListView.SelectedIndices.Count == 0)
            {
                return;
            }

            TechPosition position = item.Positions[techPositionListView.SelectedIndices[0]];

            // 値に変化がなければ何もせずに戻る
            var newY = (int) techYNumericUpDown.Value;
            if (newY == position.Y)
            {
                return;
            }

            position.Y = newY;

            UpdateTechPositionList(item);

            foreach (Label label in treePictureBox.Controls)
            {
                var info = label.Tag as TechLabelInfo;
                if (info != null && info.Position == position)
                {
                    label.Location = new Point(position.X, position.Y);
                }
            }

            SetDirtyFlag();
        }

        /// <summary>
        ///     技術座標追加ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechPositionAddButtonClick(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            var position = new TechPosition {X = 0, Y = 0};
            item.Positions.Add(position);

            UpdateTechPositionList(item);

            AddTechTreeTechItem(item, position);

            SetDirtyFlag();
        }

        /// <summary>
        ///     技術座標削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechPositionRemoveButtonClick(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            if (techPositionListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = techPositionListView.SelectedIndices[0];
            TechPosition position = item.Positions[index];

            item.Positions.RemoveAt(index);

            UpdateTechPositionList(item);

            RemoveTechTreeItem(item, position);

            SetDirtyFlag();
        }

        /// <summary>
        ///     技術画像を更新する
        /// </summary>
        /// <param name="item">技術</param>
        private void UpdateTechPicture(Tech item)
        {
            techPictureNameTextBox.Text = string.IsNullOrEmpty(item.PictureName) ? "" : item.PictureName;

            string fileName =
                Game.GetFileName(Path.Combine(Game.TechPicturePathName,
                                              string.Format("{0}.bmp",
                                                            string.IsNullOrEmpty(item.PictureName)
                                                                ? item.Id.ToString(CultureInfo.InvariantCulture)
                                                                : item.PictureName)));
            if (File.Exists(fileName))
            {
                var bitmap = new Bitmap(fileName);
                bitmap.MakeTransparent();
                techPictureBox.Image = bitmap;
            }
            else
            {
                techPictureBox.Image = null;
            }
        }

        /// <summary>
        ///     画像ファイル名変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechPictureNameTextBoxTextChanged(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            string newPictureName = techPictureNameTextBox.Text;
            if (newPictureName.Equals(item.PictureName))
            {
                return;
            }

            item.PictureName = newPictureName;
            UpdateTechPicture(item);

            SetDirtyFlag();
        }

        /// <summary>
        ///     画像ファイル名参照ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechPictureNameBrowseButtonClick(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            var dialog = new OpenFileDialog
                             {
                                 InitialDirectory = Path.Combine(Game.FolderName, Game.TechPicturePathName),
                                 FileName = item.PictureName,
                                 Filter = Resources.OpenBitmapFileDialogFilter
                             };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                techPictureNameTextBox.Text = Path.GetFileNameWithoutExtension(dialog.FileName);
            }
        }

        #endregion

        #region 必要技術タブ

        /// <summary>
        ///     AND条件必要技術リストを更新する
        /// </summary>
        /// <param name="item">技術</param>
        private void UpdateAndRequiredList(Tech item)
        {
            if (item == null)
            {
                return;
            }

            andRequiredListView.Items.Clear();
            foreach (int id in item.Required)
            {
                var listItem = new ListViewItem {Text = id.ToString(CultureInfo.InvariantCulture)};
                foreach (var pair in TechIdMap)
                {
                    if (pair.Key == id)
                    {
                        listItem.SubItems.Add(Config.GetText(pair.Value.Name));
                    }
                }
                andRequiredListView.Items.Add(listItem);
            }

            if (andRequiredListView.Items.Count > 0)
            {
                andRequiredListView.Items[0].Focused = true;
                andRequiredListView.Items[0].Selected = true;
            }
        }


        /// <summary>
        ///     OR条件必要技術リストを更新する
        /// </summary>
        /// <param name="item">技術</param>
        private void UpdateOrRequiredList(Tech item)
        {
            if (item == null)
            {
                return;
            }

            orRequiredListView.Items.Clear();
            foreach (int id in item.OrRequired)
            {
                var listItem = new ListViewItem {Text = id.ToString(CultureInfo.InvariantCulture)};
                foreach (var pair in TechIdMap)
                {
                    if (pair.Key == id)
                    {
                        listItem.SubItems.Add(Config.GetText(pair.Value.Name));
                    }
                }
                orRequiredListView.Items.Add(listItem);
            }

            if (orRequiredListView.Items.Count > 0)
            {
                orRequiredListView.Items[0].Focused = true;
                orRequiredListView.Items[0].Selected = true;
            }
        }

        /// <summary>
        ///     AND条件必要技術追加ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAndAddButtonClick(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            var id = (int) andIdNumericUpDown.Value;
            item.Required.Add(id);

            AddAndRequiredListItem(id);

            SetDirtyFlag();
        }

        /// <summary>
        ///     OR条件必要技術追加ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOrAddButtonClick(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            var id = (int) orIdNumericUpDown.Value;
            item.OrRequired.Add(id);

            AddOrRequiredListItem(id);

            SetDirtyFlag();
        }

        /// <summary>
        ///     AND条件必要技術変更ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAndModifyButtonClick(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            if (andRequiredListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = andRequiredListView.SelectedIndices[0];

            var id = (int) andIdNumericUpDown.Value;
            item.Required[index] = id;

            ModifyAndRequiredListItem(id, index);

            SetDirtyFlag();
        }

        /// <summary>
        ///     OR条件必要技術変更ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOrModifyButtonClick(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            if (orRequiredListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = orRequiredListView.SelectedIndices[0];

            var id = (int) orIdNumericUpDown.Value;
            item.OrRequired[index] = id;

            ModifyOrRequiredListItem(id, index);

            SetDirtyFlag();
        }

        /// <summary>
        ///     AND条件必要技術削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAndRemoveButtonClick(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            if (andRequiredListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = andRequiredListView.SelectedIndices[0];

            RemoveAndRequiredListItem(index);

            item.Required.RemoveAt(index);

            SetDirtyFlag();
        }

        /// <summary>
        ///     OR条件必要技術削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOrRemoveButtonClick(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            if (orRequiredListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = orRequiredListView.SelectedIndices[0];

            RemoveOrRequiredListItem(index);

            item.OrRequired.RemoveAt(index);

            SetDirtyFlag();
        }

        /// <summary>
        ///     AND条件必要技術リストに項目を追加する
        /// </summary>
        /// <param name="id">必要技術ID</param>
        private void AddAndRequiredListItem(int id)
        {
            var listItem = new ListViewItem {Text = id.ToString(CultureInfo.InvariantCulture)};
            foreach (var pair in TechIdMap)
            {
                if (pair.Key == id)
                {
                    listItem.SubItems.Add(Config.GetText(pair.Value.Name));
                }
            }
            andRequiredListView.Items.Add(listItem);

            andRequiredListView.Items[andRequiredListView.Items.Count - 1].Focused = true;
            andRequiredListView.Items[andRequiredListView.Items.Count - 1].Selected = true;
        }

        /// <summary>
        ///     OR条件必要技術リストに項目を追加する
        /// </summary>
        /// <param name="id">必要技術ID</param>
        private void AddOrRequiredListItem(int id)
        {
            var listItem = new ListViewItem {Text = id.ToString(CultureInfo.InvariantCulture)};
            foreach (var pair in TechIdMap)
            {
                if (pair.Key == id)
                {
                    listItem.SubItems.Add(Config.GetText(pair.Value.Name));
                }
            }
            orRequiredListView.Items.Add(listItem);

            orRequiredListView.Items[orRequiredListView.Items.Count - 1].Focused = true;
            orRequiredListView.Items[orRequiredListView.Items.Count - 1].Selected = true;
        }

        /// <summary>
        ///     AND条件必要技術リストの項目を変更する
        /// </summary>
        /// <param name="id">必要技術ID</param>
        /// <param name="index">変更対象の項目インデックス</param>
        private void ModifyAndRequiredListItem(int id, int index)
        {
            andRequiredListView.Items[index].SubItems.Clear();
            andRequiredListView.Items[index].Text = id.ToString(CultureInfo.InvariantCulture);
            foreach (var pair in TechIdMap)
            {
                if (pair.Key == id)
                {
                    andRequiredListView.Items[index].SubItems.Add(Config.GetText(pair.Value.Name));
                }
            }
        }

        /// <summary>
        ///     OR条件必要技術リストの項目を変更する
        /// </summary>
        /// <param name="id">必要技術ID</param>
        /// <param name="index">変更対象の項目インデックス</param>
        private void ModifyOrRequiredListItem(int id, int index)
        {
            orRequiredListView.Items[index].SubItems.Clear();
            orRequiredListView.Items[index].Text = id.ToString(CultureInfo.InvariantCulture);
            foreach (var pair in TechIdMap)
            {
                if (pair.Key == id)
                {
                    orRequiredListView.Items[index].SubItems.Add(Config.GetText(pair.Value.Name));
                }
            }
        }

        /// <summary>
        ///     AND条件必要技術リストの項目を削除する
        /// </summary>
        /// <param name="index">削除対象の項目インデックス</param>
        private void RemoveAndRequiredListItem(int index)
        {
            andRequiredListView.Items[index].Remove();

            if (andRequiredListView.Items.Count > 0)
            {
                if (index < andRequiredListView.Items.Count)
                {
                    andRequiredListView.Items[index].Focused = true;
                    andRequiredListView.Items[index].Selected = true;
                }
                else
                {
                    andRequiredListView.Items[andRequiredListView.Items.Count - 1].Focused = true;
                    andRequiredListView.Items[andRequiredListView.Items.Count - 1].Selected = true;
                }
            }
        }

        /// <summary>
        ///     OR条件必要技術リストの項目を削除する
        /// </summary>
        /// <param name="index">削除対象の項目インデックス</param>
        private void RemoveOrRequiredListItem(int index)
        {
            orRequiredListView.Items[index].Remove();

            if (orRequiredListView.Items.Count > 0)
            {
                if (index < orRequiredListView.Items.Count)
                {
                    orRequiredListView.Items[index].Focused = true;
                    orRequiredListView.Items[index].Selected = true;
                }
                else
                {
                    orRequiredListView.Items[orRequiredListView.Items.Count - 1].Focused = true;
                    orRequiredListView.Items[orRequiredListView.Items.Count - 1].Selected = true;
                }
            }
        }

        #endregion

        #region 小研究タブ

        /// <summary>
        ///     小研究リストを更新する
        /// </summary>
        /// <param name="item">技術</param>
        private void UpdateComponentList(Tech item)
        {
            if (item == null)
            {
                return;
            }

            componentListView.Items.Clear();
            foreach (TechComponent component in item.Components)
            {
                ListViewItem listItem = CreateComponentListItem(component);

                componentListView.Items.Add(listItem);
            }

            if (componentListView.Items.Count > 0)
            {
                componentListView.Items[0].Focused = true;
                componentListView.Items[0].Selected = true;
            }
        }

        /// <summary>
        ///     小研究リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComponentListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            // 小研究リストの選択項目がなければ何もしない
            if (componentListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = componentListView.SelectedIndices[0];

            TechComponent component = item.Components[index];
            if (component == null)
            {
                return;
            }

            componentIdNumericUpDown.Value = component.Id;
            componentNameTextBox.Text = Config.GetText(component.Name);
            UpdateComponentSpecialityComboBox(component);
            componentDifficultyNumericUpDown.Value = component.Difficulty;
            componentDoubleTimeCheckBox.Checked = component.DoubleTime;
        }

        /// <summary>
        ///     小研究の新規ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComponentNewButtonClick(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            TechComponent component = TechComponent.Create();

            if (componentListView.SelectedIndices.Count > 0)
            {
                int index = componentListView.SelectedIndices[0];
                TechComponent selected = item.Components[index];
                component.Id = selected.Id + 1;

                item.InsertComponent(component, index + 1);

                InsertComponentListItem(component, index + 1);
            }
            else
            {
                item.AddComponent(component);

                AddComponentListItem(component);
            }

            SetDirtyFlag();
        }

        /// <summary>
        ///     小研究の複製ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComponentCloneButtonClick(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            // 小研究リストの選択項目がなければ何もしない
            if (componentListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = componentListView.SelectedIndices[0];

            TechComponent component = item.Components[index].Clone();

            item.InsertComponent(component, index + 1);

            InsertComponentListItem(component, index + 1);

            SetDirtyFlag();
        }

        /// <summary>
        ///     小研究の削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComponentRemoveButtonClick(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            // 小研究リストの選択項目がなければ何もしない
            if (componentListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = componentListView.SelectedIndices[0];

            item.RemoveComponent(index);

            RemoveComponentListItem(index);

            SetDirtyFlag();
        }

        /// <summary>
        ///     小研究の上へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComponentUpButtonClick(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            // 小研究リストの選択項目がなければ何もしない
            if (componentListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = componentListView.SelectedIndices[0];

            // リストの先頭ならば何もしない
            if (index == 0)
            {
                return;
            }

            item.MoveComponent(index, index - 1);

            MoveComponentListItem(index, index - 1);

            SetDirtyFlag();
        }

        /// <summary>
        ///     小研究の下へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComponentDownButtonClick(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            // 小研究リストの選択項目がなければ何もしない
            if (componentListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = componentListView.SelectedIndices[0];

            // リストの末尾ならば何もしない
            if (index == componentListView.Items.Count - 1)
            {
                return;
            }

            item.MoveComponent(index, index + 1);

            MoveComponentListItem(index, index + 1);

            SetDirtyFlag();
        }

        /// <summary>
        ///     小研究ID変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComponentIdNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            // 小研究リストの選択項目がなければ何もしない
            if (componentListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = componentListView.SelectedIndices[0];

            TechComponent component = item.Components[index];
            if (component == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            var newId = (int) componentIdNumericUpDown.Value;
            if (newId == component.Id)
            {
                return;
            }

            component.Id = newId;

            componentListView.Items[index].Text = newId.ToString(CultureInfo.InvariantCulture);

            SetDirtyFlag();
        }

        /// <summary>
        ///     小研究名変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComponentNameTextBoxTextChanged(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            // 小研究リストの選択項目がなければ何もしない
            if (componentListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = componentListView.SelectedIndices[0];

            TechComponent component = item.Components[index];
            if (component == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            string newText = componentNameTextBox.Text;
            if (newText.Equals(Config.GetText(component.Name)))
            {
                return;
            }

            Config.SetText(component.Name, newText);

            componentListView.Items[index].SubItems[1].Text = newText;

            SetDirtyFlag();
            Config.SetDirtyFlag(Game.TechTextFileName);
        }

        /// <summary>
        ///     小研究特性変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComponentSpecialityComboBoxSelectionChangeCommitted(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            // 小研究リストの選択項目がなければ何もしない
            if (componentListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = componentListView.SelectedIndices[0];

            TechComponent component = item.Components[index];
            if (component == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            TechSpeciality newSpeciality;
            if (component.Speciality != TechSpeciality.None)
            {
                newSpeciality = (TechSpeciality) (componentSpecialityComboBox.SelectedIndex + 1);
            }
            else
            {
                newSpeciality = (TechSpeciality) componentSpecialityComboBox.SelectedIndex;
            }
            if (newSpeciality == component.Speciality)
            {
                return;
            }

            component.Speciality = newSpeciality;

            componentListView.Items[index].SubItems[2].Text =
                Config.GetText(Tech.SpecialityNameTable[(int) newSpeciality]);
            UpdateComponentSpecialityComboBox(component);

            SetDirtyFlag();
        }

        /// <summary>
        ///     小研究難易度変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComponentDifficultyNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            // 小研究リストの選択項目がなければ何もしない
            if (componentListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = componentListView.SelectedIndices[0];

            TechComponent component = item.Components[index];
            if (component == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            var newDifficulty = (int) componentDifficultyNumericUpDown.Value;
            if (newDifficulty == component.Difficulty)
            {
                return;
            }

            component.Difficulty = newDifficulty;

            componentListView.Items[index].SubItems[3].Text = newDifficulty.ToString(CultureInfo.InvariantCulture);

            SetDirtyFlag();
        }

        /// <summary>
        ///     小研究2倍時間設定変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComponentDoubleTimeCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            // 小研究リストの選択項目がなければ何もしない
            if (componentListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = componentListView.SelectedIndices[0];

            TechComponent component = item.Components[index];
            if (component == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            bool newDoubleTime = componentDoubleTimeCheckBox.Checked;
            if (newDoubleTime == component.DoubleTime)
            {
                return;
            }

            component.DoubleTime = newDoubleTime;

            componentListView.Items[index].SubItems[4].Text = newDoubleTime ? Resources.Yes : Resources.No;

            SetDirtyFlag();
        }

        /// <summary>
        ///     小研究リストの項目を作成する
        /// </summary>
        /// <param name="component">小研究</param>
        /// <returns>小研究リストの項目</returns>
        private ListViewItem CreateComponentListItem(TechComponent component)
        {
            var listItem = new ListViewItem {Text = component.Id.ToString(CultureInfo.InvariantCulture)};
            listItem.SubItems.Add(Config.GetText(component.Name));
            listItem.SubItems.Add(Config.GetText(Tech.SpecialityNameTable[(int) component.Speciality]));
            listItem.SubItems.Add(component.Difficulty.ToString(CultureInfo.InvariantCulture));
            listItem.SubItems.Add(component.DoubleTime ? Resources.Yes : Resources.No);

            return listItem;
        }

        /// <summary>
        ///     小研究リストの項目を追加する
        /// </summary>
        /// <param name="component">追加対象の小研究</param>
        private void AddComponentListItem(TechComponent component)
        {
            ListViewItem listItem = CreateComponentListItem(component);

            componentListView.Items.Add(listItem);
        }

        /// <summary>
        ///     小研究リストに項目を挿入する
        /// </summary>
        /// <param name="component">挿入対象の小研究</param>
        /// <param name="index">挿入する位置</param>
        private void InsertComponentListItem(TechComponent component, int index)
        {
            ListViewItem listItem = CreateComponentListItem(component);

            componentListView.Items.Insert(index, listItem);
        }

        /// <summary>
        ///     小研究リストの項目を移動する
        /// </summary>
        /// <param name="src">移動元の位置</param>
        /// <param name="dest">移動先の位置</param>
        private void MoveComponentListItem(int src, int dest)
        {
            ListViewItem listItem = componentListView.Items[src];

            if (src > dest)
            {
                // 上へ移動する場合
                componentListView.Items.Insert(dest, listItem);
                componentListView.Items.RemoveAt(src + 1);
            }
            else
            {
                // 下へ移動する場合
                componentListView.Items.Insert(dest + 1, listItem);
                componentListView.Items.RemoveAt(src);
            }

            componentListView.Items[dest].Focused = true;
            componentListView.Items[dest].Selected = true;
        }

        /// <summary>
        ///     小研究リストから項目を削除する
        /// </summary>
        /// <param name="index">削除する項目の位置</param>
        private void RemoveComponentListItem(int index)
        {
            componentListView.Items.RemoveAt(index);

            if (componentListView.Items.Count > 0)
            {
                if (index < componentListView.Items.Count - 1)
                {
                    componentListView.Items[index].Focused = true;
                    componentListView.Items[index].Selected = true;
                }
                else
                {
                    componentListView.Items[componentListView.Items.Count - 1].Focused = true;
                    componentListView.Items[componentListView.Items.Count - 1].Selected = true;
                }
            }
        }

        /// <summary>
        ///     小研究特性コンボボックスを更新する
        /// </summary>
        /// <param name="component">小研究</param>
        private void UpdateComponentSpecialityComboBox(TechComponent component)
        {
            componentSpecialityComboBox.BeginUpdate();

            if (component.Speciality != TechSpeciality.None)
            {
                if (string.IsNullOrEmpty(componentSpecialityComboBox.Items[0].ToString()))
                {
                    componentSpecialityComboBox.Items.RemoveAt(0);
                }
                componentSpecialityComboBox.SelectedIndex = (int) (component.Speciality - 1);
            }
            else
            {
                if (!string.IsNullOrEmpty(componentSpecialityComboBox.Items[0].ToString()))
                {
                    componentSpecialityComboBox.Items.Insert(0, "");
                }
                componentSpecialityComboBox.SelectedIndex = 0;
            }

            componentSpecialityComboBox.EndUpdate();
        }

        #endregion

        #region 効果タブ

        /// <summary>
        ///     技術効果リストを更新する
        /// </summary>
        /// <param name="item">技術</param>
        private void UpdateEffectList(Tech item)
        {
            if (item == null)
            {
                return;
            }

            effectListView.Items.Clear();
            foreach (Command command in item.Effects)
            {
                ListViewItem listItem = CreateEffectListItem(command);

                effectListView.Items.Add(listItem);
            }

            if (effectListView.Items.Count > 0)
            {
                effectListView.Items[0].Focused = true;
                effectListView.Items[0].Selected = true;
            }
        }

        /// <summary>
        ///     技術効果リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEffectListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            // 技術効果リストの選択項目がなければ何もしない
            if (effectListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = effectListView.SelectedIndices[0];

            Command command = item.Effects[index];
            if (command == null)
            {
                return;
            }

            commandTypeComboBox.SelectedIndex = (int) command.Type;
            commandWhichComboBox.Text = command.Which != null ? command.Which.ToString() : "";
            commandValueComboBox.Text = command.Value != null ? command.Value.ToString() : "";
            commandWhenComboBox.Text = command.When != null ? command.When.ToString() : "";
            commandWhereComboBox.Text = command.Where != null ? command.Where.ToString() : "";
        }

        /// <summary>
        ///     技術効果の新規ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEffectNewButtonClick(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            var command = new Command();

            if (effectListView.SelectedIndices.Count > 0)
            {
                int index = effectListView.SelectedIndices[0];

                item.InsertCommand(command, index + 1);

                InsertEffectListItem(command, index + 1);
            }
            else
            {
                item.AddCommand(command);

                AddEffectListItem(command);
            }

            SetDirtyFlag();
        }

        /// <summary>
        ///     小研究の複製ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEffectCloneButtonClick(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            // 技術効果リストの選択項目がなければ何もしない
            if (effectListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = effectListView.SelectedIndices[0];

            Command command = item.Effects[index].Clone();

            item.InsertCommand(command, index + 1);

            InsertEffectListItem(command, index + 1);

            SetDirtyFlag();
        }

        /// <summary>
        ///     技術効果の削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEffectRemoveButtonClick(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            // 技術効果リストの選択項目がなければ何もしない
            if (effectListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = effectListView.SelectedIndices[0];

            item.RemoveCommand(index);

            RemoveEffectListItem(index);

            SetDirtyFlag();
        }

        /// <summary>
        ///     技術効果の上へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEffectUpButtonClick(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            // 技術効果リストの選択項目がなければ何もしない
            if (effectListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = effectListView.SelectedIndices[0];

            // リストの先頭ならば何もしない
            if (index == 0)
            {
                return;
            }

            item.MoveCommand(index, index - 1);

            MoveEffectListItem(index, index - 1);

            SetDirtyFlag();
        }

        /// <summary>
        ///     技術効果の下へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEffectDownButtonClick(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            // 技術効果リストの選択項目がなければ何もしない
            if (effectListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = effectListView.SelectedIndices[0];

            // リストの末尾ならば何もしない
            if (index == effectListView.Items.Count - 1)
            {
                return;
            }

            item.MoveCommand(index, index + 1);

            MoveEffectListItem(index, index + 1);

            SetDirtyFlag();
        }

        /// <summary>
        ///     技術効果種類変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCommandTypeComboBoxSelectionChangeCommitted(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            // 技術効果リストの選択項目がなければ何もしない
            if (effectListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = effectListView.SelectedIndices[0];

            Command command = item.Effects[index];
            if (command == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            CommandType newType;
            if (command.Type != CommandType.None)
            {
                newType = (CommandType) (commandTypeComboBox.SelectedIndex + 1);
            }
            else
            {
                newType = (CommandType) commandTypeComboBox.SelectedIndex;
            }
            if (newType == command.Type)
            {
                return;
            }

            command.Type = newType;

            effectListView.Items[index].Text = Command.TypeStringTable[(int) newType];
            UpdateCommandTypeComboBox(command);

            SetDirtyFlag();
        }

        /// <summary>
        ///     技術効果Whichパラメータ変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCommandWhichComboBoxTextUpdate(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            // 技術効果リストの選択項目がなければ何もしない
            if (effectListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = effectListView.SelectedIndices[0];

            Command command = item.Effects[index];
            if (command == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            string newText = commandWhichComboBox.Text;
            if (command.Which != null && newText.Equals(command.Which.ToString()))
            {
                return;
            }

            command.Which = newText;

            effectListView.Items[index].SubItems[1].Text = newText;

            SetDirtyFlag();
        }

        /// <summary>
        ///     技術効果Valueパラメータ変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCommandValueComboBoxTextUpdate(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            // 技術効果リストの選択項目がなければ何もしない
            if (effectListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = effectListView.SelectedIndices[0];

            Command command = item.Effects[index];
            if (command == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            string newText = commandValueComboBox.Text;
            if (command.Value != null && newText.Equals(command.Value.ToString()))
            {
                return;
            }

            command.Value = newText;

            effectListView.Items[index].SubItems[2].Text = newText;

            SetDirtyFlag();
        }

        /// <summary>
        ///     技術効果Whenパラメータ変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCommandWhenComboBoxTextUpdate(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            // 技術効果リストの選択項目がなければ何もしない
            if (effectListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = effectListView.SelectedIndices[0];

            Command command = item.Effects[index];
            if (command == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            string newText = commandWhenComboBox.Text;
            if (command.When != null && newText.Equals(command.When.ToString()))
            {
                return;
            }

            command.When = newText;

            effectListView.Items[index].SubItems[3].Text = newText;

            SetDirtyFlag();
        }

        /// <summary>
        ///     技術効果Whereパラメータ変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCommandWhereComboBoxTextUpdate(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as Tech;
            if (item == null)
            {
                return;
            }

            // 技術効果リストの選択項目がなければ何もしない
            if (effectListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = effectListView.SelectedIndices[0];

            Command command = item.Effects[index];
            if (command == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            string newText = commandWhereComboBox.Text;
            if (command.Where != null && newText.Equals(command.Where.ToString()))
            {
                return;
            }

            command.Where = newText;

            effectListView.Items[index].SubItems[4].Text = newText;

            SetDirtyFlag();
        }

        /// <summary>
        ///     技術効果リストの項目を作成する
        /// </summary>
        /// <param name="command">技術効果</param>
        /// <returns>技術効果リストの項目</returns>
        private ListViewItem CreateEffectListItem(Command command)
        {
            var listItem = new ListViewItem {Text = Command.TypeStringTable[(int) command.Type]};
            listItem.SubItems.Add(command.Which != null ? command.Which.ToString() : "");
            listItem.SubItems.Add(command.Value != null ? command.Value.ToString() : "");
            listItem.SubItems.Add(command.When != null ? command.When.ToString() : "");
            listItem.SubItems.Add(command.Where != null ? command.Where.ToString() : "");

            return listItem;
        }

        /// <summary>
        ///     技術効果リストの項目を追加する
        /// </summary>
        /// <param name="command">追加対象の技術効果</param>
        private void AddEffectListItem(Command command)
        {
            ListViewItem listItem = CreateEffectListItem(command);

            effectListView.Items.Add(listItem);
        }

        /// <summary>
        ///     技術効果リストに項目を挿入する
        /// </summary>
        /// <param name="command">挿入対象の技術効果</param>
        /// <param name="index">挿入する位置</param>
        private void InsertEffectListItem(Command command, int index)
        {
            ListViewItem listItem = CreateEffectListItem(command);

            effectListView.Items.Insert(index, listItem);
        }

        /// <summary>
        ///     技術効果リストの項目を移動する
        /// </summary>
        /// <param name="src">移動元の位置</param>
        /// <param name="dest">移動先の位置</param>
        private void MoveEffectListItem(int src, int dest)
        {
            ListViewItem listItem = effectListView.Items[src];

            if (src > dest)
            {
                // 上へ移動する場合
                effectListView.Items.Insert(dest, listItem);
                effectListView.Items.RemoveAt(src + 1);
            }
            else
            {
                // 下へ移動する場合
                effectListView.Items.Insert(dest + 1, listItem);
                effectListView.Items.RemoveAt(src);
            }

            effectListView.Items[dest].Focused = true;
            effectListView.Items[dest].Selected = true;
        }

        /// <summary>
        ///     技術効果リストから項目を削除する
        /// </summary>
        /// <param name="index">削除する項目の位置</param>
        private void RemoveEffectListItem(int index)
        {
            effectListView.Items.RemoveAt(index);

            if (effectListView.Items.Count > 0)
            {
                if (index < effectListView.Items.Count - 1)
                {
                    effectListView.Items[index].Focused = true;
                    effectListView.Items[index].Selected = true;
                }
                else
                {
                    effectListView.Items[effectListView.Items.Count - 1].Focused = true;
                    effectListView.Items[effectListView.Items.Count - 1].Selected = true;
                }
            }
        }

        /// <summary>
        ///     技術効果特性コンボボックスを更新する
        /// </summary>
        /// <param name="command">技術効果</param>
        private void UpdateCommandTypeComboBox(Command command)
        {
            commandTypeComboBox.BeginUpdate();

            if (command.Type != CommandType.None)
            {
                if (string.IsNullOrEmpty(commandTypeComboBox.Items[0].ToString()))
                {
                    commandTypeComboBox.Items.RemoveAt(0);
                }
                commandTypeComboBox.SelectedIndex = (int) (command.Type - 1);
            }
            else
            {
                if (!string.IsNullOrEmpty(commandTypeComboBox.Items[0].ToString()))
                {
                    commandTypeComboBox.Items.Insert(0, "");
                }
                commandTypeComboBox.SelectedIndex = 0;
            }

            commandTypeComboBox.EndUpdate();
        }

        #endregion

        #region ラベルタブ

        /// <summary>
        ///     ラベルタブの項目を更新する
        /// </summary>
        /// <param name="item">技術ラベル</param>
        private void UpdateLabelItems(TechLabel item)
        {
            // ラベルタブの編集項目
            labelNameTextBox.Text = Config.GetText(item.Tag);
            UpdateLabelPositionList(item);
        }

        /// <summary>
        ///     技術ラベルの編集項目を有効化する
        /// </summary>
        private void EnableLabelItems()
        {
            // タブの有効化
            editTabControl.TabPages[5].Enabled = true;
        }

        /// <summary>
        ///     技術ラベルの編集項目を無効化する
        /// </summary>
        private void DisableLabelItems()
        {
            // タブの無効化
            editTabControl.TabPages[5].Enabled = false;

            // 設定項目の初期化
            labelNameTextBox.Text = "";
            labelPositionListView.Items.Clear();
            labelXNumericUpDown.Value = 0;
            labelYNumericUpDown.Value = 0;
        }

        /// <summary>
        ///     技術ラベル座標リストを更新する
        /// </summary>
        /// <param name="item">技術ラベル</param>
        private void UpdateLabelPositionList(TechLabel item)
        {
            if (item == null)
            {
                return;
            }

            labelPositionListView.BeginUpdate();
            labelPositionListView.Items.Clear();

            foreach (TechPosition position in item.Positions)
            {
                var listItem = new ListViewItem(position.X.ToString(CultureInfo.InvariantCulture));
                listItem.SubItems.Add(position.Y.ToString(CultureInfo.InvariantCulture));
                labelPositionListView.Items.Add(listItem);
            }

            if (labelPositionListView.Items.Count > 0)
            {
                labelPositionListView.Items[0].Focused = true;
                labelPositionListView.Items[0].Selected = true;
            }

            labelPositionListView.EndUpdate();
        }

        /// <summary>
        ///     ラベル名変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLabelNameTextBoxTextChanged(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as TechLabel;
            if (item == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            string newText = labelNameTextBox.Text;
            if (newText.Equals(Config.GetText(item.Tag)))
            {
                return;
            }

            Config.SetText(item.Tag, newText);

            // 項目リストボックスの項目を再設定することで表示更新している
            // この時再選択によりフォーカスが外れるので、イベントハンドラを一時的に無効化する
            techListBox.SelectedIndexChanged -= OnTechListBoxSelectedIndexChanged;
            techListBox.Items[techListBox.SelectedIndex] = item;
            techListBox.SelectedIndexChanged += OnTechListBoxSelectedIndexChanged;

            SetDirtyFlag();
            Config.SetDirtyFlag(Game.TechTextFileName);
        }

        /// <summary>
        ///     ラベル座標リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLabelPositionListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            if (labelPositionListView.SelectedIndices.Count == 0)
            {
                return;
            }

            var item = techListBox.SelectedItem as TechLabel;
            if (item == null)
            {
                return;
            }

            int index = labelPositionListView.SelectedIndices[0];
            labelXNumericUpDown.Value = item.Positions[index].X;
            labelYNumericUpDown.Value = item.Positions[index].Y;
        }

        /// <summary>
        ///     ラベルX座標変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLabelXNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as TechLabel;
            if (item == null)
            {
                return;
            }

            if (labelPositionListView.SelectedIndices.Count == 0)
            {
                return;
            }

            TechPosition position = item.Positions[labelPositionListView.SelectedIndices[0]];

            // 値に変化がなければ何もせずに戻る
            var newX = (int) labelXNumericUpDown.Value;
            if (newX == position.X)
            {
                return;
            }

            position.X = newX;

            UpdateLabelPositionList(item);

            foreach (Label label in treePictureBox.Controls)
            {
                var info = label.Tag as TechLabelInfo;
                if (info != null && info.Position == position)
                {
                    label.Location = new Point(position.X, position.Y);
                }
            }

            SetDirtyFlag();
        }

        /// <summary>
        ///     ラベルY座標変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLabelYNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as TechLabel;
            if (item == null)
            {
                return;
            }

            if (labelPositionListView.SelectedIndices.Count == 0)
            {
                return;
            }

            TechPosition position = item.Positions[labelPositionListView.SelectedIndices[0]];

            // 値に変化がなければ何もせずに戻る
            var newY = (int) labelYNumericUpDown.Value;
            if (newY == position.Y)
            {
                return;
            }

            position.Y = newY;

            UpdateLabelPositionList(item);

            foreach (Label label in treePictureBox.Controls)
            {
                var info = label.Tag as TechLabelInfo;
                if (info != null && info.Position == position)
                {
                    label.Location = new Point(position.X, position.Y);
                }
            }

            SetDirtyFlag();
        }

        /// <summary>
        ///     ラベル座標追加ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLabelPositionAddButtonClick(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as TechLabel;
            if (item == null)
            {
                return;
            }

            var position = new TechPosition {X = 0, Y = 0};
            item.Positions.Add(position);

            UpdateLabelPositionList(item);

            AddTechTreeLabelItem(item, position);

            SetDirtyFlag();
        }

        /// <summary>
        ///     ラベル座標削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLabelPositionRemoveButtonClick(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as TechLabel;
            if (item == null)
            {
                return;
            }

            if (labelPositionListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = labelPositionListView.SelectedIndices[0];
            TechPosition position = item.Positions[index];

            item.Positions.RemoveAt(index);

            UpdateLabelPositionList(item);

            RemoveTechTreeItem(item, position);

            SetDirtyFlag();
        }

        #endregion

        #region イベントタブ

        /// <summary>
        ///     技術イベント関連項目を更新する
        /// </summary>
        /// <param name="item">技術イベント</param>
        private void UpdateEventItems(TechEvent item)
        {
            // イベントタブの編集項目
            eventIdNumericUpDown.Value = item.Id;
            eventTechNumericUpDown.Value = item.Technology;
            UpdateEventPositionList(item);
        }

        /// <summary>
        ///     技術イベントの編集項目を有効化する
        /// </summary>
        private void EnableEventItems()
        {
            // タブの有効化
            editTabControl.TabPages[6].Enabled = true;
        }

        /// <summary>
        ///     技術イベントの編集項目を無効化する
        /// </summary>
        private void DisableEventItems()
        {
            // タブの無効化
            editTabControl.TabPages[6].Enabled = false;

            // 設定項目の初期化
            eventIdNumericUpDown.Value = 0;
            eventTechNumericUpDown.Value = 0;
            eventPositionListView.Items.Clear();
            eventXNumericUpDown.Value = 0;
            eventYNumericUpDown.Value = 0;
        }

        /// <summary>
        ///     技術イベント座標リストを更新する
        /// </summary>
        /// <param name="item">技術イベント</param>
        private void UpdateEventPositionList(TechEvent item)
        {
            if (item == null)
            {
                return;
            }

            eventPositionListView.BeginUpdate();
            eventPositionListView.Items.Clear();

            foreach (TechPosition position in item.Positions)
            {
                var listItem = new ListViewItem(position.X.ToString(CultureInfo.InvariantCulture));
                listItem.SubItems.Add(position.Y.ToString(CultureInfo.InvariantCulture));
                eventPositionListView.Items.Add(listItem);
            }

            if (eventPositionListView.Items.Count > 0)
            {
                eventPositionListView.Items[0].Focused = true;
                eventPositionListView.Items[0].Selected = true;
            }

            eventPositionListView.EndUpdate();
        }

        /// <summary>
        ///     技術イベントID変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEventIdNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as TechEvent;
            if (item == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            var newId = (int) eventIdNumericUpDown.Value;
            if (newId == item.Id)
            {
                return;
            }

            item.Id = newId;

            // 項目リストボックスの項目を再設定することで表示更新している
            // この時再選択によりフォーカスが外れるので、イベントハンドラを一時的に無効化する
            techListBox.SelectedIndexChanged -= OnTechListBoxSelectedIndexChanged;
            techListBox.Items[techListBox.SelectedIndex] = item;
            techListBox.SelectedIndexChanged += OnTechListBoxSelectedIndexChanged;

            SetDirtyFlag();
        }

        /// <summary>
        ///     技術イベント技術ID変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEventTechNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as TechEvent;
            if (item == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            var newTechnology = (int) eventTechNumericUpDown.Value;
            if (newTechnology == item.Id)
            {
                return;
            }

            item.Technology = newTechnology;

            SetDirtyFlag();
        }

        /// <summary>
        ///     技術イベント座標リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEventPositionListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            if (eventPositionListView.SelectedIndices.Count == 0)
            {
                return;
            }

            var item = techListBox.SelectedItem as TechEvent;
            if (item == null)
            {
                return;
            }

            int index = eventPositionListView.SelectedIndices[0];
            eventXNumericUpDown.Value = item.Positions[index].X;
            eventYNumericUpDown.Value = item.Positions[index].Y;
        }

        /// <summary>
        ///     技術イベントX座標変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEventXNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as TechEvent;
            if (item == null)
            {
                return;
            }

            if (eventPositionListView.SelectedIndices.Count == 0)
            {
                return;
            }

            TechPosition position = item.Positions[eventPositionListView.SelectedIndices[0]];

            // 値に変化がなければ何もせずに戻る
            var newX = (int) eventXNumericUpDown.Value;
            if (newX == position.X)
            {
                return;
            }

            position.X = newX;

            UpdateEventPositionList(item);

            foreach (Label label in treePictureBox.Controls)
            {
                var info = label.Tag as TechLabelInfo;
                if (info != null && info.Position == position)
                {
                    label.Location = new Point(position.X, position.Y);
                }
            }

            SetDirtyFlag();
        }

        /// <summary>
        ///     技術イベントY座標変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEventYNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as TechEvent;
            if (item == null)
            {
                return;
            }

            if (eventPositionListView.SelectedIndices.Count == 0)
            {
                return;
            }

            TechPosition position = item.Positions[eventPositionListView.SelectedIndices[0]];

            // 値に変化がなければ何もせずに戻る
            var newY = (int) eventYNumericUpDown.Value;
            if (newY == position.Y)
            {
                return;
            }

            position.Y = newY;

            UpdateEventPositionList(item);

            foreach (Label label in treePictureBox.Controls)
            {
                var info = label.Tag as TechLabelInfo;
                if (info != null && info.Position == position)
                {
                    label.Location = new Point(position.X, position.Y);
                }
            }

            SetDirtyFlag();
        }

        /// <summary>
        ///     技術イベント座標追加ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEventPositionAddButtonClick(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as TechEvent;
            if (item == null)
            {
                return;
            }

            var position = new TechPosition {X = 0, Y = 0};
            item.Positions.Add(position);

            UpdateEventPositionList(item);

            AddTechTreeEventItem(item, position);

            SetDirtyFlag();
        }

        /// <summary>
        ///     技術イベント座標削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEventPositionRemoveButtonClick(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as TechEvent;
            if (item == null)
            {
                return;
            }

            if (eventPositionListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = eventPositionListView.SelectedIndices[0];
            TechPosition position = item.Positions[index];

            item.Positions.RemoveAt(index);

            UpdateEventPositionList(item);

            RemoveTechTreeItem(item, position);

            SetDirtyFlag();
        }

        #endregion
    }
}