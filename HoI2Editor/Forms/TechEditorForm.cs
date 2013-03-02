using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HoI2Editor.Models;
using HoI2Editor.Properties;
using HoI2Editor.Utilities;

namespace HoI2Editor.Forms
{
    /// <summary>
    ///     技術ツリーエディタのフォーム
    /// </summary>
    public partial class TechEditorForm : Form
    {
        #region フィールド

        /// <summary>
        ///     技術アプリケーション項目とIDの対応付け
        /// </summary>
        private static readonly List<KeyValuePair<int, TechApplication>> TechApplicationMap =
            new List<KeyValuePair<int, TechApplication>>();

        /// <summary>
        ///     技術アプリケーションラベルの画像
        /// </summary>
        private static Bitmap _applicationLabelBitmap;

        /// <summary>
        ///     技術イベントラベルの画像
        /// </summary>
        private static Bitmap _eventLabelBitmap;

        /// <summary>
        ///     技術アプリケーションラベルの描画領域
        /// </summary>
        private static readonly Region ApplicationLabelRegion =
            new Region(new Rectangle(0, 0, ApplicationLabelWidth, ApplicationLabelHeight));

        /// <summary>
        ///     技術イベントラベルの描画領域
        /// </summary>
        private static readonly Region EventLabelRegion =
            new Region(new Rectangle(0, 0, EventLabelWidth, EventLabelHeight));

        /// <summary>
        ///     ドラッグアンドドロップの開始位置
        /// </summary>
        private static Point _dragPoint = Point.Empty;

        /// <summary>
        ///     ドラッグ中のカーソル
        /// </summary>
        private static Cursor _dragCursor;

        /// <summary>
        ///     技術アプリケーションラベルのANDマスク
        /// </summary>
        private static Bitmap _applicationLabelAndMask;

        /// <summary>
        ///     技術イベントラベルのANDマスク
        /// </summary>
        private static Bitmap _eventLabelAndMask;

        #endregion

        #region 定数

        /// <summary>
        ///     技術アプリケーションラベルの幅
        /// </summary>
        private const int ApplicationLabelWidth = 112;

        /// <summary>
        ///     技術アプリケーションラベルの高さ
        /// </summary>
        private const int ApplicationLabelHeight = 16;

        /// <summary>
        ///     技術イベントラベルの幅
        /// </summary>
        private const int EventLabelWidth = 112;

        /// <summary>
        ///     技術イベントラベルの高さ
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

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public TechEditorForm()
        {
            InitializeComponent();

            // 技術ツリーピクチャーボックス上でドラッグアンドドロップを許可する
            // フォームデザイナで設定できないためコード上で記載する
            techTreePictureBox.AllowDrop = true;
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechEditorFormLoad(object sender, EventArgs e)
        {
            // 研究特性を初期化する
            Techs.InitSpecialities();

            // 技術定義ファイルを読み込む
            LoadTechFiles();

            // ラベル画像を読み込む
            InitLabelBitmap();

            // 編集可能な項目を初期化する
            InitEditableItems();

            // 必要技術タブの技術リストを更新する
            UpdateRequiredTechItems();

            // 技術イベントタブの技術リストを更新する
            UpdateEventTechItems();

            // カテゴリリストボックスを初期化する
            InitCategoryListBox();
        }

        /// <summary>
        ///     技術アプリケーション項目とIDの対応付けを初期化する
        /// </summary>
        private static void InitTechApplicationMap()
        {
            TechApplicationMap.Clear();

            foreach (TechApplication item in Techs.List.SelectMany(grp => grp.Items.OfType<TechApplication>()))
            {
                TechApplicationMap.Add(new KeyValuePair<int, TechApplication>(item.Id, item));
            }
        }

        /// <summary>
        ///     編集項目を初期化する
        /// </summary>
        private void InitEditableItems()
        {
            // 小研究特性
            int maxWidth = componentSpecialityComboBox.DropDownWidth;
            foreach (string s in Techs.Specialities
                                      .Select(speciality => Techs.SpecialityNames[(int) speciality])
                                      .Select(Config.GetText))
            {
                componentSpecialityComboBox.Items.Add(s);
                maxWidth = Math.Max(maxWidth,
                                    TextRenderer.MeasureText(s, componentSpecialityComboBox.Font).Width +
                                    SystemInformation.VerticalScrollBarWidth);
            }
            componentSpecialityComboBox.DropDownWidth = maxWidth;

            // 技術効果種類
            maxWidth = commandTypeComboBox.DropDownWidth;
            foreach (string s in Command.TypeStrings)
            {
                commandTypeComboBox.Items.Add(s);
                maxWidth = Math.Max(maxWidth,
                                    TextRenderer.MeasureText(s, commandTypeComboBox.Font).Width +
                                    SystemInformation.VerticalScrollBarWidth);
            }
            commandTypeComboBox.DropDownWidth = maxWidth;
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

        #region 技術定義データ処理

        /// <summary>
        ///     再読み込みボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReloadButtonClick(object sender, EventArgs e)
        {
            // 文字列定義ファイルを再読み込みする
            Config.RequireReload();
            Config.Load();

            // 技術定義ファイルを再読み込みする
            Techs.RequireReload();
            LoadTechFiles();

            // 必要技術タブの技術リストを更新する
            UpdateRequiredTechItems();

            // 技術イベントタブの技術リストを更新する
            UpdateEventTechItems();

            // カテゴリリストボックスを初期化する
            InitCategoryListBox();
        }

        /// <summary>
        ///     保存ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            // 文字列定義ファイルを保存する
            SaveConfigFiles();

            // 技術定義ファイルを保存する
            SaveTechFiles();
        }

        /// <summary>
        ///     技術定義ファイルを読み込む
        /// </summary>
        private void LoadTechFiles()
        {
            // 技術定義ファイルを読み込む
            Techs.Load();

            // 技術アプリケーション項目とIDの対応付けを初期化する
            InitTechApplicationMap();

            // 編集済みフラグが解除されるため表示を更新する
            categoryListBox.Refresh();
        }

        /// <summary>
        ///     技術定義ファイルを保存する
        /// </summary>
        private void SaveTechFiles()
        {
            // 技術定義ファイルを保存する
            Techs.Save();

            // 編集済みフラグが解除されるため表示を更新する
            categoryListBox.Refresh();
        }

        /// <summary>
        ///     文字列定義ファイルを保存する
        /// </summary>
        private static void SaveConfigFiles()
        {
            // 一時キーをIDに応じた値に変更する
            int no = 1;
            foreach (TechGroup grp in Techs.List)
            {
                foreach (ITechItem item in grp.Items)
                {
                    if (item is TechApplication)
                    {
                        var application = item as TechApplication;
                        application.RenameTempKey(grp.Category);
                    }
                    else if (item is TechLabel)
                    {
                        var label = item as TechLabel;
                        label.RenameTempKey(no.ToString(CultureInfo.InvariantCulture));
                        no++;
                    }
                }
            }

            // 文字列定義ファイルを保存する
            Config.Save();
        }

        #endregion

        #region カテゴリリストボックス

        /// <summary>
        ///     カテゴリリストボックスを初期化する
        /// </summary>
        private void InitCategoryListBox()
        {
            categoryListBox.Items.Clear();
            foreach (TechGroup grp in Techs.List)
            {
                categoryListBox.Items.Add(Config.GetText(grp.Name));
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
            // 技術項目リストボックスを更新する
            UpdateItemListBox();

            // 技術ツリー画像を更新する
            UpdateTechTreePicture();

            // カテゴリタブの項目を更新する
            UpdateCategoryTabItems();

            // 技術/必要技術/小研究/技術効果タブの項目を無効化する
            DisableApplicationItems();
            // ラベルタブの項目を無効化する
            DisableLabelItems();
            // イベントタブの項目を無効化する
            DisableEventItems();

            // カテゴリタブを選択する
            editTabControl.SelectedIndex = 0;

            cloneButton.Enabled = false;
            removeButton.Enabled = false;
            topButton.Enabled = false;
            upButton.Enabled = false;
            downButton.Enabled = false;
            bottomButton.Enabled = false;
        }

        /// <summary>
        ///     カテゴリリストボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCategoryListBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 背景を描画する
            e.DrawBackground();

            // 選択項目がない場合はスキップ
            if (e.Index != -1)
            {
                Brush brush;
                if ((e.State & DrawItemState.Selected) != DrawItemState.Selected)
                {
                    // 変更ありの項目は文字色を変更する
                    brush = Techs.IsDirty((TechCategory) e.Index)
                                ? new SolidBrush(Color.Red)
                                : new SolidBrush(categoryListBox.ForeColor);
                }
                else
                {
                    brush = new SolidBrush(SystemColors.HighlightText);
                }
                string s = categoryListBox.Items[e.Index].ToString();
                e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     選択中の技術カテゴリを取得する
        /// </summary>
        /// <returns></returns>
        private TechCategory GetSelectedCategory()
        {
            return (TechCategory) categoryListBox.SelectedIndex;
        }

        #endregion

        #region 項目リストボックス

        /// <summary>
        ///     項目リストボックスの表示を更新する
        /// </summary>
        private void UpdateItemListBox()
        {
            techListBox.BeginUpdate();

            techListBox.Items.Clear();
            techTreePictureBox.Controls.Clear();

            foreach (ITechItem item in Techs.List[categoryListBox.SelectedIndex].Items)
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
            // 選択項目がない場合
            if (techListBox.SelectedIndex == -1)
            {
                // 技術/必要技術/小研究/技術効果タブの項目を無効化する
                DisableApplicationItems();
                // ラベルタブの項目を無効化する
                DisableLabelItems();
                // イベントタブの項目を無効化する
                DisableEventItems();

                // カテゴリタブを選択する
                editTabControl.SelectedIndex = 0;

                return;
            }

            TechGroup grp = Techs.List[categoryListBox.SelectedIndex];
            ITechItem item = grp.Items[techListBox.SelectedIndex];

            if (item is TechApplication)
            {
                // 技術/必要技術/小研究/技術効果タブの項目を更新する
                UpdateTechItems(item as TechApplication);

                // 技術/必要技術/小研究/技術効果タブの項目を有効化する
                EnableApplicationItems();
                // ラベルタブの項目を無効化する
                DisableLabelItems();
                // イベントタブの項目を無効化する
                DisableEventItems();

                // 技術タブを選択する
                if (editTabControl.SelectedIndex < 1 || editTabControl.SelectedIndex > 4)
                {
                    editTabControl.SelectedIndex = 1;
                }
            }
            else if (item is TechLabel)
            {
                // ラベルタブの項目を更新する
                UpdateLabelItems(item as TechLabel);

                // ラベルタブの項目を有効化する
                EnableLabelItems();
                // 技術/必要技術/小研究/技術効果タブの項目を無効化する
                DisableApplicationItems();
                // イベントタブの項目を無効化する
                DisableEventItems();

                // ラベルタブを選択する
                editTabControl.SelectedIndex = 5;
            }
            else if (item is TechEvent)
            {
                // イベントタブの項目を更新する
                UpdateEventItems(item as TechEvent);

                // イベントタブの項目を有効化する
                EnableEventItems();
                // 技術/必要技術/小研究/技術効果タブの項目を無効化する
                DisableApplicationItems();
                // ラベルタブの項目を無効化する
                DisableLabelItems();

                // イベントタブを選択する
                editTabControl.SelectedIndex = 6;
            }

            cloneButton.Enabled = true;
            removeButton.Enabled = true;
            topButton.Enabled = (techListBox.SelectedIndex > 0);
            upButton.Enabled = (techListBox.SelectedIndex > 0);
            downButton.Enabled = (techListBox.SelectedIndex < techListBox.Items.Count - 1);
            bottomButton.Enabled = (techListBox.SelectedIndex < techListBox.Items.Count - 1);
        }

        /// <summary>
        ///     項目リストボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechListBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の種類によって背景色を変える
            if ((e.State & DrawItemState.Selected) == 0)
            {
                object item = techListBox.Items[e.Index];
                if (item is TechLabel)
                {
                    e.Graphics.FillRectangle(Brushes.AliceBlue,
                                             new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
                }
                else if (item is TechEvent)
                {
                    e.Graphics.FillRectangle(Brushes.Honeydew,
                                             new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
                }
            }

            // 項目の文字列を描画する
            Brush brush = new SolidBrush(techListBox.ForeColor);
            e.Graphics.DrawString(techListBox.Items[e.Index].ToString(), e.Font, brush,
                                  new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     項目リストの新規技術ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNewTechButtonClick(object sender, EventArgs e)
        {
            TechCategory category = GetSelectedCategory();
            var item = new TechApplication
                           {
                               Year = 1936,
                               Name = Config.GetTempKey(),
                               ShortName = Config.GetTempKey(),
                               Desc = Config.GetTempKey()
                           };

            // 文字列
            Config.SetText(item.Name, "", Game.TechTextFileName);
            Config.SetText(item.ShortName, "", Game.TechTextFileName);
            Config.SetText(item.Desc, "", Game.TechTextFileName);

            ITechItem selected = GetSelectedItem();
            if (selected != null)
            {
                // IDを引き継ぐ
                var appItem = selected as TechApplication;
                if (appItem != null)
                {
                    item.Id = appItem.Id + 10;
                    TechApplicationMap.Add(new KeyValuePair<int, TechApplication>(item.Id, item));
                }

                // 座標リスト
                item.Positions.Add(selected.Positions[0].Clone());

                // 技術項目リストに項目を挿入する
                int index = techListBox.SelectedIndex;
                Techs.InsertItem(category, item, index + 1);
                InsertListItem(item, index + 1);
            }
            else
            {
                // ID
                TechApplicationMap.Add(new KeyValuePair<int, TechApplication>(item.Id, item));

                // 座標リスト
                item.Positions.Add(new TechPosition());

                // 技術項目リストに項目を追加する
                Techs.AddItem(category, item);
                AddListItem(item);
            }

            // 技術ツリーにラベルを追加する
            foreach (TechPosition position in item.Positions)
            {
                AddTechTreeItem(item, position);
            }

            // 必要技術タブの技術リストを更新する
            UpdateRequiredTechItems();
            // 技術イベントタブの技術リストを更新する
            UpdateEventTechItems();

            // 編集済みフラグを設定する
            item.SetDirty();
            Techs.SetDirty(category);
        }

        /// <summary>
        ///     項目リストの新規ラベルボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNewLabelButtonClick(object sender, EventArgs e)
        {
            TechCategory category = GetSelectedCategory();
            var item = new TechLabel {Name = Config.GetTempKey()};

            // 文字列
            Config.SetText(item.Name, "", Game.TechTextFileName);

            ITechItem selected = GetSelectedItem();
            if (selected != null)
            {
                // 座標リスト
                item.Positions.Add(selected.Positions[0].Clone());

                // 技術項目リストに項目を挿入する
                int index = techListBox.SelectedIndex;
                Techs.InsertItem(category, item, index + 1);
                InsertListItem(item, index + 1);
            }
            else
            {
                // 座標リスト
                item.Positions.Add(new TechPosition());

                // 技術項目リストに項目を追加する
                Techs.AddItem(category, item);
                AddListItem(item);
            }

            // 技術ツリーにラベルを追加する
            foreach (TechPosition position in item.Positions)
            {
                AddTechTreeItem(item, position);
            }

            // 編集済みフラグを設定する
            Techs.SetDirty(category);
        }

        /// <summary>
        ///     項目リストの新規イベントボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNewEventButtonClick(object sender, EventArgs e)
        {
            TechCategory category = GetSelectedCategory();
            var item = new TechEvent();

            ITechItem selected = GetSelectedItem();
            if (selected != null)
            {
                // IDを引き継ぐ
                var eventItem = selected as TechEvent;
                if (eventItem != null)
                {
                    item.Id = eventItem.Id + 10;
                }

                // 座標リスト
                item.Positions.Add(selected.Positions[0].Clone());

                // 技術項目リストに項目を挿入する
                int index = techListBox.SelectedIndex;
                Techs.InsertItem(category, item, index + 1);
                InsertListItem(item, index + 1);
            }
            else
            {
                // 座標リスト
                item.Positions.Add(new TechPosition());

                // 技術項目リストに項目を追加する
                Techs.AddItem(category, item);
                AddListItem(item);
            }

            // 技術ツリーにラベルを追加する
            foreach (TechPosition position in item.Positions)
            {
                AddTechTreeItem(item, position);
            }

            // 編集済みフラグを設定する
            Techs.SetDirty(category);
        }

        /// <summary>
        ///     項目リストの複製ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloneButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            ITechItem selected = GetSelectedItem();
            if (selected == null)
            {
                return;
            }

            TechCategory category = GetSelectedCategory();
            ITechItem item = selected.Clone();

            // 技術項目リストに項目を挿入する
            int index = techListBox.SelectedIndex;
            Techs.InsertItem(category, item, index + 1);
            InsertListItem(item, index + 1);

            // 技術ツリーにラベルを追加する
            foreach (TechPosition position in item.Positions)
            {
                AddTechTreeItem(item, position);
            }

            if (selected is TechApplication)
            {
                // 必要技術タブの技術リストを更新する
                UpdateRequiredTechItems();
                // 技術イベントタブの技術リストを更新する
                UpdateEventTechItems();
            }

            // 編集済みフラグを設定する
            Techs.SetDirty(category);
        }

        /// <summary>
        ///     項目リストの削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRemoveButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            ITechItem item = GetSelectedItem();
            if (item == null)
            {
                return;
            }

            // 技術項目リストから項目を削除する
            TechCategory category = GetSelectedCategory();
            int index = techListBox.SelectedIndex;
            Techs.RemoveItem(category, index);
            RemoveTechListItem(index);

            // 技術ツリーからラベルを削除する
            RemoveTechTreeItems(item);

            if (item is TechApplication)
            {
                // 技術アプリケーション項目との対応付けを削除する
                List<KeyValuePair<int, TechApplication>> list =
                    TechApplicationMap.Where(pair => pair.Value == item).ToList();
                foreach (var pair in list)
                {
                    TechApplicationMap.Remove(pair);
                }

                // 必要技術タブの技術リストを更新する
                UpdateRequiredTechItems();
                // 技術イベントタブの技術リストを更新する
                UpdateEventTechItems();
            }

            // 技術項目がなくなれば編集項目を無効化してカテゴリタブを選択する
            if (techListBox.Items.Count == 0)
            {
                DisableApplicationItems();
                DisableLabelItems();
                DisableEventItems();

                editTabControl.SelectedIndex = 0;
            }

            // 編集済みフラグを設定する
            Techs.SetDirty(category);
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

            // 技術項目リストの項目を移動する
            TechCategory category = GetSelectedCategory();
            Techs.MoveItem(category, index, 0);
            MoveTechListItem(index, 0);

            ITechItem item = GetSelectedItem();
            if (item is TechApplication)
            {
                // 必要技術タブの技術リストを更新する
                UpdateRequiredTechItems();
                // 技術イベントタブの技術リストを更新する
                UpdateEventTechItems();
            }

            // 編集済みフラグを設定する
            Techs.SetDirty(category);
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

            // 技術項目リストの項目を移動する
            TechCategory category = GetSelectedCategory();
            Techs.MoveItem(category, index, index - 1);
            MoveTechListItem(index, index - 1);

            ITechItem item = GetSelectedItem();
            if (item is TechApplication)
            {
                // 必要技術タブの技術リストを更新する
                UpdateRequiredTechItems();
                // 技術イベントタブの技術リストを更新する
                UpdateEventTechItems();
            }

            // 編集済みフラグを設定する
            Techs.SetDirty(category);
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

            // 技術項目リストの項目を移動する
            TechCategory category = GetSelectedCategory();
            Techs.MoveItem(category, index, index + 1);
            MoveTechListItem(index, index + 1);

            ITechItem item = GetSelectedItem();
            if (item is TechApplication)
            {
                // 必要技術タブの技術リストを更新する
                UpdateRequiredTechItems();
                // 技術イベントタブの技術リストを更新する
                UpdateEventTechItems();
            }

            // 編集済みフラグを設定する
            Techs.SetDirty(category);
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

            // 技術項目リストの項目を移動する
            TechCategory category = GetSelectedCategory();
            Techs.MoveItem(category, index, techListBox.Items.Count - 1);
            MoveTechListItem(index, techListBox.Items.Count - 1);

            ITechItem item = GetSelectedItem();
            if (item is TechApplication)
            {
                // 必要技術タブの技術リストを更新する
                UpdateRequiredTechItems();
                // 技術イベントタブの技術リストを更新する
                UpdateEventTechItems();
            }

            // 編集済みフラグを設定する
            Techs.SetDirty(category);
        }

        /// <summary>
        ///     項目リストに項目を追加する
        /// </summary>
        /// <param name="item">追加対象の項目</param>
        private void AddListItem(ITechItem item)
        {
            techListBox.Items.Add(item);

            // 追加した項目を選択する
            techListBox.SelectedIndex = techListBox.Items.Count - 1;
        }

        /// <summary>
        ///     項目リストに項目を挿入する
        /// </summary>
        /// <param name="item">挿入対象の項目</param>
        /// <param name="index">挿入先の位置</param>
        private void InsertListItem(ITechItem item, int index)
        {
            techListBox.Items.Insert(index, item);

            // 挿入した項目を選択する
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
            else if (index > 0)
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

        /// <summary>
        ///     選択中の項目を取得する
        /// </summary>
        /// <returns>選択中の項目</returns>
        private ITechItem GetSelectedItem()
        {
            // 選択項目がない場合
            if (techListBox.SelectedIndex == -1)
            {
                return null;
            }

            return techListBox.SelectedItem as ITechItem;
        }

        #endregion

        #region 技術ツリー

        /// <summary>
        ///     技術ツリー画像を更新する
        /// </summary>
        private void UpdateTechTreePicture()
        {
            TechCategory category = GetSelectedCategory();
            techTreePictureBox.ImageLocation =
                Game.GetReadFileName(Game.PicturePathName, TechTreeFileNames[(int) category]);
        }

        /// <summary>
        ///     技術ツリーに項目群を追加する
        /// </summary>
        /// <param name="item">追加対象の項目</param>
        private void AddTechTreeItems(ITechItem item)
        {
            foreach (TechPosition position in item.Positions)
            {
                AddTechTreeItem(item, position);
            }
        }

        /// <summary>
        ///     技術ツリーに技術項目を追加する
        /// </summary>
        /// <param name="item">追加対象の項目</param>
        /// <param name="position">追加対象の位置</param>
        private void AddTechTreeItem(ITechItem item, TechPosition position)
        {
            var label = new Label
                            {
                                Location = new Point(position.X, position.Y),
                                BackColor = Color.Transparent,
                                Tag = new TechTreeLabelInfo {Item = item, Position = position},
                            };

            if (item is TechApplication)
            {
                label.Size = new Size(ApplicationLabelWidth, ApplicationLabelHeight);
                label.Image = _applicationLabelBitmap;
                label.Region = ApplicationLabelRegion;
                label.Paint += OnTechTreeApplicationPaint;
            }
            else if (item is TechLabel)
            {
                var labelItem = item as TechLabel;
                label.Size = TextRenderer.MeasureText(Config.GetText(labelItem.Name), label.Font);
                label.Paint += OnTechTreeLabelPaint;
            }
            else
            {
                label.Size = new Size(EventLabelWidth, EventLabelHeight);
                label.Image = _eventLabelBitmap;
                label.Region = EventLabelRegion;
            }

            label.MouseDown += OnTechTreeLabelMouseDown;
            label.MouseUp += OnTechTreeLabelMouseUp;
            label.MouseMove += OnTechTreeLabelMouseMove;
            label.GiveFeedback += OnTechTreeLabelGiveFeedback;

            techTreePictureBox.Controls.Add(label);
        }

        /// <summary>
        ///     技術ツリーの項目群を削除する
        /// </summary>
        /// <param name="item">削除対象の項目</param>
        private void RemoveTechTreeItems(ITechItem item)
        {
            Control.ControlCollection labels = techTreePictureBox.Controls;
            foreach (Label label in labels)
            {
                var info = label.Tag as TechTreeLabelInfo;
                if (info == null)
                {
                    continue;
                }

                // 削除対象の項目と対応するラベルを削除する
                if (info.Item == item)
                {
                    techTreePictureBox.Controls.Remove(label);
                }
            }
        }

        /// <summary>
        ///     技術ツリーの項目を削除する
        /// </summary>
        /// <param name="item">削除対象の項目</param>
        /// <param name="position">削除対象の位置</param>
        private void RemoveTechTreeItem(ITechItem item, TechPosition position)
        {
            Control.ControlCollection labels = techTreePictureBox.Controls;
            foreach (Label label in labels)
            {
                var info = label.Tag as TechTreeLabelInfo;
                if (info == null)
                {
                    continue;
                }

                // 削除対象の項目と対応するラベルを削除する
                if (info.Item == item && info.Position == position)
                {
                    techTreePictureBox.Controls.Remove(label);
                    break;
                }
            }
        }

        /// <summary>
        ///     技術ツリーの技術アプリケーション描画時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnTechTreeApplicationPaint(object sender, PaintEventArgs e)
        {
            var label = sender as Label;
            if (label == null)
            {
                return;
            }

            var info = label.Tag as TechTreeLabelInfo;
            if (info == null)
            {
                return;
            }

            var item = info.Item as TechApplication;
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
        ///     技術ツリーの技術ラベル描画時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnTechTreeLabelPaint(object sender, PaintEventArgs e)
        {
            var label = sender as Label;
            if (label == null)
            {
                return;
            }

            var info = label.Tag as TechTreeLabelInfo;
            if (info == null)
            {
                return;
            }

            var item = info.Item as TechLabel;
            if (item == null)
            {
                return;
            }

            string s = Config.GetText(item.Name);
            if (string.IsNullOrEmpty(s))
            {
                return;
            }

            Brush brush;
            if ((s[0] == '%' || s[0] == 'ｧ' || s[0] == '§') &&
                s.Length > 4 &&
                s[1] >= '0' && s[1] <= '9' &&
                s[2] >= '0' && s[2] <= '9' &&
                s[3] >= '0' && s[3] <= '9')
            {
                // 色指定文字列に従う
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
            // 技術アプリケーション
            var bitmap = new Bitmap(Game.GetReadFileName(Game.TechLabelPathName));
            _applicationLabelBitmap = bitmap.Clone(new Rectangle(0, 0, ApplicationLabelWidth, ApplicationLabelHeight),
                                                   bitmap.PixelFormat);
            bitmap.Dispose();
            _applicationLabelAndMask = new Bitmap(_applicationLabelBitmap.Width, _applicationLabelBitmap.Height);
            Color transparent = _applicationLabelBitmap.GetPixel(0, 0);
            for (int x = 0; x < _applicationLabelBitmap.Width; x++)
            {
                for (int y = 0; y < _applicationLabelBitmap.Height; y++)
                {
                    if (_applicationLabelBitmap.GetPixel(x, y) == transparent)
                    {
                        ApplicationLabelRegion.Exclude(new Rectangle(x, y, 1, 1));
                        _applicationLabelAndMask.SetPixel(x, y, Color.White);
                    }
                    else
                    {
                        _applicationLabelAndMask.SetPixel(x, y, Color.Black);
                    }
                }
            }
            _applicationLabelBitmap.MakeTransparent(transparent);

            // 技術イベント
            bitmap = new Bitmap(Game.GetReadFileName(Game.SecretLabelPathName));
            _eventLabelBitmap = bitmap.Clone(new Rectangle(0, 0, EventLabelWidth, EventLabelHeight), bitmap.PixelFormat);
            bitmap.Dispose();
            _eventLabelAndMask = new Bitmap(_eventLabelBitmap.Width, _eventLabelBitmap.Height);
            transparent = _eventLabelBitmap.GetPixel(0, 0);
            for (int x = 0; x < _eventLabelBitmap.Width; x++)
            {
                for (int y = 0; y < _eventLabelBitmap.Height; y++)
                {
                    if (_eventLabelBitmap.GetPixel(x, y) == transparent)
                    {
                        EventLabelRegion.Exclude(new Rectangle(x, y, 1, 1));
                        _eventLabelAndMask.SetPixel(x, y, Color.White);
                    }
                    else
                    {
                        _eventLabelAndMask.SetPixel(x, y, Color.Black);
                    }
                }
            }
            _eventLabelBitmap.MakeTransparent(transparent);
        }

        /// <summary>
        ///     技術ツリーラベルのマウスダウン時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechTreeLabelMouseDown(object sender, MouseEventArgs e)
        {
            // 左ボタン押下以外ならばドラッグ開始位置を初期化する
            if (e.Button != MouseButtons.Left)
            {
                _dragPoint = Point.Empty;
                return;
            }

            var label = sender as Label;
            if (label == null)
            {
                return;
            }

            var info = label.Tag as TechTreeLabelInfo;
            if (info == null)
            {
                return;
            }

            // ラベルに対応する技術項目を選択する
            techListBox.SelectedItem = info.Item;

            // ラベルに対応する座標リストビューの項目を選択する
            ITechItem item = info.Item;
            for (int i = 0; i < item.Positions.Count; i++)
            {
                if (item.Positions[i] == info.Position)
                {
                    if (item is TechApplication)
                    {
                        techPositionListView.Items[i].Focused = true;
                        techPositionListView.Items[i].Selected = true;
                    }
                    else if (item is TechLabel)
                    {
                        labelPositionListView.Items[i].Focused = true;
                        labelPositionListView.Items[i].Selected = true;
                    }
                    else if (item is TechEvent)
                    {
                        eventPositionListView.Items[i].Focused = true;
                        eventPositionListView.Items[i].Selected = true;
                    }
                    break;
                }
            }

            // ドラッグ開始位置を設定する
            _dragPoint = new Point(label.Left + e.X, label.Top + e.Y);
        }

        /// <summary>
        ///     技術ツリーラベルのマウスアップ時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnTechTreeLabelMouseUp(object sender, MouseEventArgs e)
        {
            // 左ボタン押下以外ならばドラッグ開始位置を初期化する
            _dragPoint = Point.Empty;

            // 通常カーソルに戻す
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        ///     技術ツリーラベルのマウス移動時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechTreeLabelMouseMove(object sender, MouseEventArgs e)
        {
            // ドラッグ中でなければ何もしない
            if (_dragPoint == Point.Empty)
            {
                return;
            }

            // クリック時のぶれをドラッグと検出しないように
            // 一定座標移動するまではドラッグ操作と認めない
            Size dragSize = SystemInformation.DragSize;
            var dragRect = new Rectangle(_dragPoint.X - dragSize.Width/2, _dragPoint.Y - dragSize.Height/2,
                                         dragSize.Width, dragSize.Height);
            if (dragRect.Contains(e.X, e.Y))
            {
                return;
            }

            var label = sender as Label;
            if (label == null)
            {
                return;
            }

            var info = label.Tag as TechTreeLabelInfo;
            if (info == null)
            {
                return;
            }

            // ドラッグ中のマウスカーソル画像を作成する
            var bitmap = new Bitmap(label.Width, label.Height);
            bitmap.MakeTransparent(bitmap.GetPixel(0, 0));
            label.DrawToBitmap(bitmap, new Rectangle(0, 0, label.Width, label.Height));

            if (info.Item is TechApplication)
            {
                _dragCursor = CursorFactory.CreateCursor(bitmap, _applicationLabelAndMask, _dragPoint.X - label.Left,
                                                         _dragPoint.Y - label.Top);
            }
            else if (info.Item is TechLabel)
            {
                _dragCursor = CursorFactory.CreateCursor(bitmap, _dragPoint.X - label.Left,
                                                         _dragPoint.Y - label.Top);
            }
            else
            {
                _dragCursor = CursorFactory.CreateCursor(bitmap, _eventLabelAndMask, _dragPoint.X - label.Left,
                                                         _dragPoint.Y - label.Top);
            }

            // ドラッグアンドドロップ処理を実行する
            label.DoDragDrop(sender, DragDropEffects.Move);

            _dragPoint = Point.Empty;
            _dragCursor.Dispose();
            bitmap.Dispose();
        }

        /// <summary>
        ///     技術ツリーラベルのカーソル更新処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechTreeLabelGiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if ((e.Effect & DragDropEffects.Move) != 0)
            {
                // ドラッグ移動中は作成済みのカーソル画像を使用する
                e.UseDefaultCursors = false;
                Cursor.Current = _dragCursor;
            }
            else
            {
                e.UseDefaultCursors = true;
            }
        }

        /// <summary>
        ///     技術ツリーピクチャーボックスにドラッグした時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTreePictureBoxDragOver(object sender, DragEventArgs e)
        {
            // ラベル以外をドラッグした時には処理しない
            if (!e.Data.GetDataPresent(typeof (Label)))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            var label = e.Data.GetData(typeof (Label)) as Label;
            if (label == null || label.Tag == null)
            {
                return;
            }

            var dragRect = new Rectangle(0, 0, techTreePictureBox.Image.Width, techTreePictureBox.Image.Height);
            Point p = techTreePictureBox.PointToClient(new Point(e.X, e.Y));
            e.Effect = dragRect.Contains(p) ? DragDropEffects.Move : DragDropEffects.None;
        }

        /// <summary>
        ///     技術ツリーピクチャーボックスにドロップした時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTreePictureBoxDragDrop(object sender, DragEventArgs e)
        {
            // ラベル以外をドロップした時には処理しない
            if (!e.Data.GetDataPresent(typeof (Label)))
            {
                return;
            }

            var label = e.Data.GetData(typeof (Label)) as Label;
            if (label == null)
            {
                return;
            }

            // 技術ツリー内のドロップ位置を計算する
            var p = new Point(e.X, e.Y);
            p = techTreePictureBox.PointToClient(p);

            p.X = label.Left + p.X - _dragPoint.X;
            p.Y = label.Top + p.Y - _dragPoint.Y;

            var info = label.Tag as TechTreeLabelInfo;
            if (info == null)
            {
                return;
            }

            // 技術ツリーラベル情報の座標を更新する
            info.Position.X = p.X;
            info.Position.Y = p.Y;

            // 座標リストビューの表示を更新する
            ITechItem item = info.Item;
            for (int i = 0; i < info.Item.Positions.Count; i++)
            {
                if (item.Positions[i] == info.Position)
                {
                    if (item is TechApplication)
                    {
                        techPositionListView.Items[i].Text = info.Position.X.ToString(CultureInfo.InvariantCulture);
                        techPositionListView.Items[i].SubItems[1].Text =
                            info.Position.Y.ToString(CultureInfo.InvariantCulture);
                    }
                    else if (item is TechLabel)
                    {
                        labelPositionListView.Items[i].Text = info.Position.X.ToString(CultureInfo.InvariantCulture);
                        labelPositionListView.Items[i].SubItems[1].Text =
                            info.Position.Y.ToString(CultureInfo.InvariantCulture);
                    }
                    else if (item is TechEvent)
                    {
                        eventPositionListView.Items[i].Text = info.Position.X.ToString(CultureInfo.InvariantCulture);
                        eventPositionListView.Items[i].SubItems[1].Text =
                            info.Position.Y.ToString(CultureInfo.InvariantCulture);
                    }
                    break;
                }
            }

            // ラベル画像を移動する
            label.Location = p;

            // 編集済みフラグを設定する
            Techs.SetDirty(GetSelectedCategory());
        }

        /// <summary>
        ///     技術ラベルに関連付けられる情報
        /// </summary>
        private class TechTreeLabelInfo
        {
            /// <summary>
            ///     技術項目
            /// </summary>
            internal ITechItem Item;

            /// <summary>
            ///     位置
            /// </summary>
            internal TechPosition Position;
        }

        #endregion

        #region カテゴリタブ

        /// <summary>
        ///     カテゴリタブの編集項目を更新する
        /// </summary>
        private void UpdateCategoryTabItems()
        {
            TechCategory category = GetSelectedCategory();
            TechGroup grp = Techs.List[(int) category];

            categoryNameTextBox.Text = Config.GetText(grp.Name);
            categoryDescTextBox.Text = Config.GetText(grp.Desc);
        }

        /// <summary>
        ///     技術グループ名変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCategoryNameTextBoxTextChanged(object sender, EventArgs e)
        {
            var category = (TechCategory) categoryListBox.SelectedIndex;
            TechGroup grp = Techs.List[(int) category];

            // 値に変化がなければ何もしない
            string name = categoryNameTextBox.Text;
            if (name.Equals(Config.GetText(grp.Name)))
            {
                return;
            }

            // 値を更新する
            Config.SetText(grp.Name, name, Game.TechTextFileName);

            // カテゴリリストボックスの項目を再設定することで表示更新している
            // この時再選択によりフォーカスが外れるので、イベントハンドラを一時的に無効化する
            categoryListBox.SelectedIndexChanged -= OnCategoryListBoxSelectedIndexChanged;
            categoryListBox.Items[(int) category] = Config.GetText(grp.Name);
            categoryListBox.SelectedIndexChanged += OnCategoryListBoxSelectedIndexChanged;

            // 編集済みフラグを設定する
            Techs.SetDirty(category);
            Config.SetDirty(Game.TechTextFileName, true);
        }

        /// <summary>
        ///     技術グループ説明変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCategoryDescTextBoxTextChanged(object sender, EventArgs e)
        {
            var category = (TechCategory) categoryListBox.SelectedIndex;
            TechGroup grp = Techs.List[(int) category];

            // 値に変化がなければ何もしない
            string desc = categoryDescTextBox.Text;
            if (desc.Equals(Config.GetText(grp.Desc)))
            {
                return;
            }

            // 値を更新する
            Config.SetText(grp.Desc, desc, Game.TechTextFileName);

            // 編集済みフラグを設定する
            Techs.SetDirty(category);
            Config.SetDirty(Game.TechTextFileName, true);
        }

        #endregion

        #region 技術タブ

        /// <summary>
        ///     技術の編集項目を有効化する
        /// </summary>
        private void EnableApplicationItems()
        {
            // タブの有効化
            editTabControl.TabPages[1].Enabled = true;
            editTabControl.TabPages[2].Enabled = true;
            editTabControl.TabPages[3].Enabled = true;
            editTabControl.TabPages[4].Enabled = true;

            // 無効化時にクリアした文字列を再設定する
            techIdNumericUpDown.Text = techIdNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            techYearNumericUpDown.Text = techYearNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            techXNumericUpDown.Text = techXNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            techYNumericUpDown.Text = techYNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            andIdNumericUpDown.Text = andIdNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            orIdNumericUpDown.Text = orIdNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            componentIdNumericUpDown.Text = componentIdNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            componentDifficultyNumericUpDown.Text =
                componentDifficultyNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     技術の編集項目を無効化する
        /// </summary>
        private void DisableApplicationItems()
        {
            // タブの無効化
            editTabControl.TabPages[1].Enabled = false;
            editTabControl.TabPages[2].Enabled = false;
            editTabControl.TabPages[3].Enabled = false;
            editTabControl.TabPages[4].Enabled = false;

            // 技術タブの設定項目初期化
            techNameTextBox.ResetText();
            techShortNameTextBox.ResetText();
            techIdNumericUpDown.ResetText();
            techYearNumericUpDown.ResetText();
            techPositionListView.Items.Clear();
            techXNumericUpDown.ResetText();
            techYNumericUpDown.ResetText();
            techPictureBox.ImageLocation = "";

            // 必要研究タブの設定項目初期化
            andRequiredListView.Items.Clear();
            orRequiredListView.Items.Clear();
            andIdNumericUpDown.ResetText();
            andTechComboBox.SelectedIndex = -1;
            orIdNumericUpDown.ResetText();
            orTechComboBox.SelectedIndex = -1;

            // 小研究タブの設定項目初期化
            componentListView.Items.Clear();
            componentIdNumericUpDown.ResetText();
            componentNameTextBox.ResetText();
            componentDifficultyNumericUpDown.ResetText();
            componentDoubleTimeCheckBox.Checked = false;
        }

        /// <summary>
        ///     技術座標の編集項目を有効化する
        /// </summary>
        private void EnableTechPositionItems()
        {
            techXNumericUpDown.Enabled = true;
            techYNumericUpDown.Enabled = true;
            techPositionRemoveButton.Enabled = true;

            // 無効化時にクリアした文字列を再設定する
            techXNumericUpDown.Text = techXNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            techYNumericUpDown.Text = techYNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     技術座標の編集項目を無効化する
        /// </summary>
        private void DisableTechPositionItems()
        {
            techXNumericUpDown.ResetText();
            techYNumericUpDown.ResetText();

            techXNumericUpDown.Enabled = false;
            techYNumericUpDown.Enabled = false;
            techPositionRemoveButton.Enabled = false;
        }

        /// <summary>
        ///     技術タブの項目を更新する
        /// </summary>
        /// <param name="item">技術アプリケーション</param>
        private void UpdateTechItems(TechApplication item)
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
        ///     技術座標リストを更新する
        /// </summary>
        /// <param name="item">技術アプリケーション</param>
        private void UpdateTechPositionList(TechApplication item)
        {
            techPositionListView.BeginUpdate();
            techPositionListView.Items.Clear();

            // 座標を順に登録する
            foreach (TechPosition position in item.Positions)
            {
                var listItem = new ListViewItem(position.X.ToString(CultureInfo.InvariantCulture));
                listItem.SubItems.Add(position.Y.ToString(CultureInfo.InvariantCulture));
                techPositionListView.Items.Add(listItem);
            }

            // 編集項目を更新する
            if (techPositionListView.Items.Count > 0)
            {
                techPositionListView.Items[0].Focused = true;
                techPositionListView.Items[0].Selected = true;
                EnableTechPositionItems();
            }
            else
            {
                DisableTechPositionItems();
            }

            techPositionListView.EndUpdate();
        }

        /// <summary>
        ///     技術画像を更新する
        /// </summary>
        /// <param name="item">技術</param>
        private void UpdateTechPicture(TechApplication item)
        {
            techPictureNameTextBox.Text = string.IsNullOrEmpty(item.PictureName) ? "" : item.PictureName;

            string fileName =
                Game.GetReadFileName(Game.TechPicturePathName,
                                     string.Format("{0}.bmp",
                                                   string.IsNullOrEmpty(item.PictureName)
                                                       ? item.Id.ToString(CultureInfo.InvariantCulture)
                                                       : item.PictureName));
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
        ///     技術名変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechNameTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechApplication;
            if (item == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            string name = techNameTextBox.Text;
            if (name.Equals(Config.GetText(item.Name)))
            {
                return;
            }

            // 値を更新する
            Config.SetText(item.Name, name, Game.TechTextFileName);

            // 項目リストボックスの項目を再設定することで表示更新している
            // この時再選択によりフォーカスが外れるので、イベントハンドラを一時的に無効化する
            techListBox.SelectedIndexChanged -= OnTechListBoxSelectedIndexChanged;
            techListBox.Items[techListBox.SelectedIndex] = item;
            techListBox.SelectedIndexChanged += OnTechListBoxSelectedIndexChanged;

            // 技術コンボボックスの項目を再設定することで表示更新している
            // この時再選択によりフォーカスが外れるので、イベントハンドラを一時的に無効化する
            andTechComboBox.SelectedIndexChanged -= OnAndTechComboBoxSelectedIndexChanged;
            orTechComboBox.SelectedIndexChanged -= OnOrTechComboBoxSelectedIndexChanged;
            eventTechComboBox.SelectedIndexChanged -= OnEventTechComboBoxSelectedIndexChanged;
            for (int i = 0; i < andTechComboBox.Items.Count; i++)
            {
                if (andTechComboBox.Items[i] == item)
                {
                    andTechComboBox.Items[i] = item;
                    orTechComboBox.Items[i] = item;
                    eventTechComboBox.Items[i] = item;
                }
            }
            andTechComboBox.SelectedIndexChanged += OnAndTechComboBoxSelectedIndexChanged;
            orTechComboBox.SelectedIndexChanged += OnOrTechComboBoxSelectedIndexChanged;
            eventTechComboBox.SelectedIndexChanged += OnEventTechComboBoxSelectedIndexChanged;

            // 編集済みフラグを設定する
            Techs.SetDirty(GetSelectedCategory());
            Config.SetDirty(Game.TechTextFileName, true);
        }

        /// <summary>
        ///     技術短縮名変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechShortNameTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechApplication;
            if (item == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            string shortName = techShortNameTextBox.Text;
            if (shortName.Equals(Config.GetText(item.ShortName)))
            {
                return;
            }

            // 値を更新する
            Config.SetText(item.ShortName, shortName, Game.TechTextFileName);

            // 技術ツリー上のラベル名を更新する
            foreach (Label label in techTreePictureBox.Controls)
            {
                var info = label.Tag as TechTreeLabelInfo;
                if (info != null && info.Item == item)
                {
                    label.Refresh();
                }
            }

            // 編集済みフラグを設定する
            Techs.SetDirty(GetSelectedCategory());
            Config.SetDirty(Game.TechTextFileName, true);
        }

        /// <summary>
        ///     技術ID変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechIdNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechApplication;
            if (item == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            var id = (int) techIdNumericUpDown.Value;
            if (id == item.Id)
            {
                return;
            }

            List<KeyValuePair<int, TechApplication>> list =
                TechApplicationMap.Where(pair => pair.Value == item).ToList();
            foreach (var pair in list)
            {
                TechApplicationMap.Remove(pair);
            }
            item.Id = id;
            TechApplicationMap.Add(new KeyValuePair<int, TechApplication>(item.Id, item));

            item.SetDirty();
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

            var item = techListBox.SelectedItem as TechApplication;
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

            item.SetDirty();
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

            var item = techListBox.SelectedItem as TechApplication;
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

            var item = techListBox.SelectedItem as TechApplication;
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

            // 値に変化がなければ何もせずに戻る
            var newX = (int) techXNumericUpDown.Value;
            if (newX == position.X)
            {
                return;
            }

            position.X = newX;

            techPositionListView.Items[index].Text = newX.ToString(CultureInfo.InvariantCulture);

            foreach (Label label in techTreePictureBox.Controls)
            {
                var info = label.Tag as TechTreeLabelInfo;
                if (info != null && info.Position == position)
                {
                    label.Location = new Point(position.X, position.Y);
                }
            }

            item.SetDirty();
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

            var item = techListBox.SelectedItem as TechApplication;
            if (item == null)
            {
                return;
            }

            if (techPositionListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = techPositionListView.SelectedIndices[0];

            TechPosition position = item.Positions[techPositionListView.SelectedIndices[0]];

            // 値に変化がなければ何もせずに戻る
            var newY = (int) techYNumericUpDown.Value;
            if (newY == position.Y)
            {
                return;
            }

            position.Y = newY;

            techPositionListView.Items[index].SubItems[1].Text = newY.ToString(CultureInfo.InvariantCulture);

            foreach (Label label in techTreePictureBox.Controls)
            {
                var info = label.Tag as TechTreeLabelInfo;
                if (info != null && info.Position == position)
                {
                    label.Location = new Point(position.X, position.Y);
                }
            }

            item.SetDirty();
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

            var item = techListBox.SelectedItem as TechApplication;
            if (item == null)
            {
                return;
            }

            var position = new TechPosition {X = 0, Y = 0};
            item.Positions.Add(position);

            var listItem = new ListViewItem {Text = position.X.ToString(CultureInfo.InvariantCulture)};
            listItem.SubItems.Add(position.Y.ToString(CultureInfo.InvariantCulture));
            techPositionListView.Items.Add(listItem);

            techPositionListView.Items[techPositionListView.Items.Count - 1].Focused = true;
            techPositionListView.Items[techPositionListView.Items.Count - 1].Selected = true;

            EnableTechPositionItems();

            AddTechTreeItem(item, position);

            item.SetDirty();
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

            var item = techListBox.SelectedItem as TechApplication;
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

            techPositionListView.Items.RemoveAt(index);

            if (index < techPositionListView.Items.Count)
            {
                techPositionListView.Items[index].Focused = true;
                techPositionListView.Items[index].Selected = true;
            }
            else if (index > 0)
            {
                techPositionListView.Items[techPositionListView.Items.Count - 1].Focused = true;
                techPositionListView.Items[techPositionListView.Items.Count - 1].Selected = true;
            }
            else
            {
                DisableTechPositionItems();
            }

            RemoveTechTreeItem(item, position);

            item.SetDirty();
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

            var item = techListBox.SelectedItem as TechApplication;
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

            item.SetDirty();
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

            var item = techListBox.SelectedItem as TechApplication;
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
        private void UpdateAndRequiredList(TechApplication item)
        {
            if (item == null)
            {
                return;
            }

            andRequiredListView.BeginUpdate();
            andRequiredListView.Items.Clear();

            foreach (int id in item.AndRequiredTechs)
            {
                var listItem = new ListViewItem {Text = id.ToString(CultureInfo.InvariantCulture)};
                foreach (var pair in TechApplicationMap)
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

                EnableAndRequiredItems();
            }
            else
            {
                DisableAndReuqiredItems();
            }

            andRequiredListView.EndUpdate();
        }


        /// <summary>
        ///     OR条件必要技術リストを更新する
        /// </summary>
        /// <param name="item">技術</param>
        private void UpdateOrRequiredList(TechApplication item)
        {
            if (item == null)
            {
                return;
            }

            orRequiredListView.BeginUpdate();
            orRequiredListView.Items.Clear();

            foreach (int id in item.OrRequiredTechs)
            {
                var listItem = new ListViewItem {Text = id.ToString(CultureInfo.InvariantCulture)};
                foreach (var pair in TechApplicationMap)
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

                EnableOrRequiredItems();
            }
            else
            {
                DisableOrReuqiredItems();
            }

            orRequiredListView.EndUpdate();
        }

        /// <summary>
        ///     必要技術タブの技術リストを更新する
        /// </summary>
        private void UpdateRequiredTechItems()
        {
            andTechComboBox.BeginUpdate();
            orTechComboBox.BeginUpdate();

            andTechComboBox.Items.Clear();
            orTechComboBox.Items.Clear();

            int maxSize = andTechComboBox.DropDownWidth;
            foreach (TechApplication item in Techs.List.SelectMany(grp => grp.Items.OfType<TechApplication>()))
            {
                andTechComboBox.Items.Add(item);
                orTechComboBox.Items.Add(item);
                maxSize = Math.Max(maxSize,
                                   TextRenderer.MeasureText(Config.GetText(item.Name), andTechComboBox.Font).Width +
                                   SystemInformation.VerticalScrollBarWidth);
            }
            andTechComboBox.DropDownWidth = maxSize;
            orTechComboBox.DropDownWidth = maxSize;

            andTechComboBox.EndUpdate();
            orTechComboBox.EndUpdate();

            if (techListBox.SelectedItem is TechApplication)
            {
                var techItem = techListBox.SelectedItem as TechApplication;
                UpdateAndRequiredList(techItem);
                UpdateOrRequiredList(techItem);
            }
        }

        /// <summary>
        ///     AND条件必要技術の編集項目を有効化する
        /// </summary>
        private void EnableAndRequiredItems()
        {
            andIdNumericUpDown.Enabled = true;
            andTechComboBox.Enabled = true;
            andRemoveButton.Enabled = true;
        }

        /// <summary>
        ///     AND条件必要技術の編集項目を無効化する
        /// </summary>
        private void DisableAndReuqiredItems()
        {
            andIdNumericUpDown.Value = 0;
            andTechComboBox.SelectedIndex = -1;

            andIdNumericUpDown.Enabled = false;
            andTechComboBox.Enabled = false;
            andRemoveButton.Enabled = false;
        }

        /// <summary>
        ///     OR条件必要技術の編集項目を有効化する
        /// </summary>
        private void EnableOrRequiredItems()
        {
            orIdNumericUpDown.Enabled = true;
            orTechComboBox.Enabled = true;
            orRemoveButton.Enabled = true;
        }

        /// <summary>
        ///     OR条件必要技術の編集項目を無効化する
        /// </summary>
        private void DisableOrReuqiredItems()
        {
            orIdNumericUpDown.Value = 0;
            orTechComboBox.SelectedIndex = -1;

            orIdNumericUpDown.Enabled = false;
            orTechComboBox.Enabled = false;
            orRemoveButton.Enabled = false;
        }

        /// <summary>
        ///     AND条件必要技術リストの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAndRequiredListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as TechApplication;
            if (item == null)
            {
                return;
            }

            if (andRequiredListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = andRequiredListView.SelectedIndices[0];

            andIdNumericUpDown.Value = item.AndRequiredTechs[index];

            andTechComboBox.SelectedIndex = -1;
            foreach (
                TechApplication techItem in
                    andTechComboBox.Items.Cast<TechApplication>()
                                   .Where(techItem => techItem.Id == item.AndRequiredTechs[index]))
            {
                andTechComboBox.SelectedItem = techItem;
            }
        }

        /// <summary>
        ///     OR条件必要技術リストの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOrRequiredListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as TechApplication;
            if (item == null)
            {
                return;
            }

            if (orRequiredListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = orRequiredListView.SelectedIndices[0];

            orIdNumericUpDown.Value = item.OrRequiredTechs[index];

            orTechComboBox.SelectedIndex = -1;
            foreach (
                TechApplication techItem in
                    orTechComboBox.Items.Cast<TechApplication>()
                                  .Where(techItem => techItem.Id == item.OrRequiredTechs[index]))
            {
                orTechComboBox.SelectedItem = techItem;
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

            var item = techListBox.SelectedItem as TechApplication;
            if (item == null)
            {
                return;
            }

            item.AndRequiredTechs.Add(0);

            AddAndRequiredListItem(0);

            item.SetDirty();
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

            var item = techListBox.SelectedItem as TechApplication;
            if (item == null)
            {
                return;
            }

            item.OrRequiredTechs.Add(0);

            AddOrRequiredListItem(0);

            item.SetDirty();
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

            var item = techListBox.SelectedItem as TechApplication;
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

            item.AndRequiredTechs.RemoveAt(index);

            item.SetDirty();
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

            var item = techListBox.SelectedItem as TechApplication;
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

            item.OrRequiredTechs.RemoveAt(index);

            item.SetDirty();
        }

        /// <summary>
        ///     AND条件必要技術ID変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAndIdNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as TechApplication;
            if (item == null)
            {
                return;
            }

            if (andRequiredListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = andRequiredListView.SelectedIndices[0];

            // 値に変化がなければ何もせずに戻る
            var newId = (int) andIdNumericUpDown.Value;
            if (newId == item.AndRequiredTechs[index])
            {
                return;
            }

            item.AndRequiredTechs[index] = newId;

            ModifyAndRequiredListItem(newId, index);

            andTechComboBox.SelectedIndex = -1;
            foreach (
                TechApplication techItem in
                    andTechComboBox.Items.Cast<TechApplication>().Where(techItem => techItem.Id == newId))
            {
                andTechComboBox.SelectedItem = techItem;
            }

            item.SetDirty();
        }

        /// <summary>
        ///     OR条件必要技術ID変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOrIdNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as TechApplication;
            if (item == null)
            {
                return;
            }

            if (orRequiredListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = orRequiredListView.SelectedIndices[0];

            // 値に変化がなければ何もせずに戻る
            var newId = (int) orIdNumericUpDown.Value;
            if (newId == item.OrRequiredTechs[index])
            {
                return;
            }

            item.OrRequiredTechs[index] = newId;

            ModifyOrRequiredListItem(newId, index);

            orTechComboBox.SelectedIndex = -1;
            foreach (
                TechApplication techItem in
                    orTechComboBox.Items.Cast<TechApplication>().Where(techItem => techItem.Id == newId))
            {
                orTechComboBox.SelectedItem = techItem;
            }

            item.SetDirty();
        }

        /// <summary>
        ///     AND必要条件技術変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAndTechComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as TechApplication;
            if (item == null)
            {
                return;
            }

            if (andRequiredListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = andRequiredListView.SelectedIndices[0];

            // 値に変化がなければ何もせずに戻る
            var techItem = andTechComboBox.SelectedItem as TechApplication;
            if (techItem == null)
            {
                return;
            }
            int newId = techItem.Id;
            if (newId == item.AndRequiredTechs[index])
            {
                return;
            }

            item.AndRequiredTechs[index] = newId;

            andIdNumericUpDown.Value = newId;
            ModifyAndRequiredListItem(newId, index);

            item.SetDirty();
        }

        /// <summary>
        ///     OR必要条件技術変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOrTechComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (techListBox.SelectedIndex == -1)
            {
                return;
            }

            var item = techListBox.SelectedItem as TechApplication;
            if (item == null)
            {
                return;
            }

            if (orRequiredListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = orRequiredListView.SelectedIndices[0];

            // 値に変化がなければ何もせずに戻る
            var techItem = orTechComboBox.SelectedItem as TechApplication;
            if (techItem == null)
            {
                return;
            }
            int newId = techItem.Id;
            if (newId == item.OrRequiredTechs[index])
            {
                return;
            }

            item.OrRequiredTechs[index] = newId;

            orIdNumericUpDown.Value = newId;
            ModifyOrRequiredListItem(newId, index);

            item.SetDirty();
        }

        /// <summary>
        ///     AND条件必要技術リストに項目を追加する
        /// </summary>
        /// <param name="id">必要技術ID</param>
        private void AddAndRequiredListItem(int id)
        {
            var listItem = new ListViewItem {Text = id.ToString(CultureInfo.InvariantCulture)};
            foreach (var pair in TechApplicationMap)
            {
                if (pair.Key == id)
                {
                    listItem.SubItems.Add(Config.GetText(pair.Value.Name));
                }
            }
            andRequiredListView.Items.Add(listItem);

            int index = andRequiredListView.Items.Count - 1;
            andRequiredListView.Items[index].Focused = true;
            andRequiredListView.Items[index].Selected = true;
            andRequiredListView.EnsureVisible(index);

            EnableAndRequiredItems();
        }

        /// <summary>
        ///     OR条件必要技術リストに項目を追加する
        /// </summary>
        /// <param name="id">必要技術ID</param>
        private void AddOrRequiredListItem(int id)
        {
            var listItem = new ListViewItem {Text = id.ToString(CultureInfo.InvariantCulture)};
            foreach (var pair in TechApplicationMap)
            {
                if (pair.Key == id)
                {
                    listItem.SubItems.Add(Config.GetText(pair.Value.Name));
                }
            }
            orRequiredListView.Items.Add(listItem);

            int index = orRequiredListView.Items.Count - 1;
            orRequiredListView.Items[index].Focused = true;
            orRequiredListView.Items[index].Selected = true;
            orRequiredListView.EnsureVisible(index);

            EnableOrRequiredItems();
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
            foreach (var pair in TechApplicationMap)
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
            foreach (var pair in TechApplicationMap)
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

            if (index < andRequiredListView.Items.Count)
            {
                andRequiredListView.Items[index].Focused = true;
                andRequiredListView.Items[index].Selected = true;
            }
            else if (index > 0)
            {
                andRequiredListView.Items[andRequiredListView.Items.Count - 1].Focused = true;
                andRequiredListView.Items[andRequiredListView.Items.Count - 1].Selected = true;
            }
            else
            {
                DisableAndReuqiredItems();
            }
        }

        /// <summary>
        ///     OR条件必要技術リストの項目を削除する
        /// </summary>
        /// <param name="index">削除対象の項目インデックス</param>
        private void RemoveOrRequiredListItem(int index)
        {
            orRequiredListView.Items[index].Remove();

            if (index < orRequiredListView.Items.Count)
            {
                orRequiredListView.Items[index].Focused = true;
                orRequiredListView.Items[index].Selected = true;
            }
            else if (index > 0)
            {
                orRequiredListView.Items[orRequiredListView.Items.Count - 1].Focused = true;
                orRequiredListView.Items[orRequiredListView.Items.Count - 1].Selected = true;
            }
            else
            {
                DisableOrReuqiredItems();
            }
        }

        #endregion

        #region 小研究タブ

        /// <summary>
        ///     小研究リストを更新する
        /// </summary>
        /// <param name="item">技術</param>
        private void UpdateComponentList(TechApplication item)
        {
            if (item == null)
            {
                return;
            }

            componentListView.BeginUpdate();
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

                EnableComponentItems();
            }
            else
            {
                DisableComponentItems();
            }

            componentListView.EndUpdate();
        }

        /// <summary>
        ///     小研究の編集項目を有効化する
        /// </summary>
        private void EnableComponentItems()
        {
            componentIdNumericUpDown.Enabled = true;
            componentNameTextBox.Enabled = true;
            componentSpecialityComboBox.Enabled = true;
            componentDifficultyNumericUpDown.Enabled = true;
            componentDoubleTimeCheckBox.Enabled = true;

            componentCloneButton.Enabled = true;
            componentRemoveButton.Enabled = true;
        }

        /// <summary>
        ///     小研究の編集項目を無効化する
        /// </summary>
        private void DisableComponentItems()
        {
            componentIdNumericUpDown.Value = 0;
            componentNameTextBox.Text = "";
            componentSpecialityComboBox.SelectedIndex = -1;
            componentDifficultyNumericUpDown.Value = 0;
            componentDoubleTimeCheckBox.Checked = false;

            componentIdNumericUpDown.Enabled = false;
            componentNameTextBox.Enabled = false;
            componentSpecialityComboBox.Enabled = false;
            componentDifficultyNumericUpDown.Enabled = false;
            componentDoubleTimeCheckBox.Enabled = false;

            componentCloneButton.Enabled = false;
            componentRemoveButton.Enabled = false;
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

            var item = techListBox.SelectedItem as TechApplication;
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

            componentUpButton.Enabled = index != 0;
            componentDownButton.Enabled = index != item.Components.Count - 1;
        }

        /// <summary>
        ///     小研究特性コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComponentSpecialityComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 背景を描画する
            e.DrawBackground();

            var combobox = sender as ComboBox;
            if (combobox != null && e.Index >= 0)
            {
                int imageIndex = e.Index - (string.IsNullOrEmpty(combobox.Items[0] as string) ? 1 : 0);
                if (imageIndex < Techs.SpecialityImages.Images.Count && imageIndex >= 0)
                {
                    var gr = new Rectangle(e.Bounds.X + 1, e.Bounds.Y + 1, 16, 16);
                    e.Graphics.DrawImage(Techs.SpecialityImages.Images[imageIndex], gr);
                }

                Brush brush = new SolidBrush(combobox.ForeColor);
                string s = combobox.Items[e.Index].ToString();
                var tr = new Rectangle(e.Bounds.X + 19, e.Bounds.Y + 3, e.Bounds.Width - 19, e.Bounds.Height);
                e.Graphics.DrawString(s, e.Font, brush, tr);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
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

            var item = techListBox.SelectedItem as TechApplication;
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

            item.SetDirty();
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

            var item = techListBox.SelectedItem as TechApplication;
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

            item.SetDirty();
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

            var item = techListBox.SelectedItem as TechApplication;
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

            item.SetDirty();
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

            var item = techListBox.SelectedItem as TechApplication;
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

            item.SetDirty();
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

            var item = techListBox.SelectedItem as TechApplication;
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

            item.SetDirty();
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

            var item = techListBox.SelectedItem as TechApplication;
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

            item.SetDirty();
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

            var item = techListBox.SelectedItem as TechApplication;
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

            Config.SetText(component.Name, newText, Game.TechTextFileName);

            componentListView.Items[index].SubItems[1].Text = newText;

            item.SetDirty();
            Config.SetDirty(Game.TechTextFileName, true);
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

            var item = techListBox.SelectedItem as TechApplication;
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
                Config.GetText(Techs.SpecialityNames[(int) newSpeciality]);
            UpdateComponentSpecialityComboBox(component);

            item.SetDirty();
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

            var item = techListBox.SelectedItem as TechApplication;
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

            item.SetDirty();
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

            var item = techListBox.SelectedItem as TechApplication;
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

            item.SetDirty();
        }

        /// <summary>
        ///     小研究リストの項目を作成する
        /// </summary>
        /// <param name="component">小研究</param>
        /// <returns>小研究リストの項目</returns>
        private static ListViewItem CreateComponentListItem(TechComponent component)
        {
            var listItem = new ListViewItem {Text = component.Id.ToString(CultureInfo.InvariantCulture)};
            listItem.SubItems.Add(Config.GetText(component.Name));
            listItem.SubItems.Add(Config.GetText(Techs.SpecialityNames[(int) component.Speciality]));
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

            int index = componentListView.Items.Count - 1;
            componentListView.Items[index].Focused = true;
            componentListView.Items[index].Selected = true;
            componentListView.EnsureVisible(index);

            EnableComponentItems();
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

            componentListView.Items[index].Focused = true;
            componentListView.Items[index].Selected = true;
            componentListView.EnsureVisible(index);
        }

        /// <summary>
        ///     小研究リストの項目を移動する
        /// </summary>
        /// <param name="src">移動元の位置</param>
        /// <param name="dest">移動先の位置</param>
        private void MoveComponentListItem(int src, int dest)
        {
            var listItem = componentListView.Items[src].Clone() as ListViewItem;
            if (listItem == null)
            {
                return;
            }

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
            componentListView.EnsureVisible(dest);
        }

        /// <summary>
        ///     小研究リストから項目を削除する
        /// </summary>
        /// <param name="index">削除する項目の位置</param>
        private void RemoveComponentListItem(int index)
        {
            componentListView.Items.RemoveAt(index);

            if (index < componentListView.Items.Count)
            {
                componentListView.Items[index].Focused = true;
                componentListView.Items[index].Selected = true;
            }
            else if (index > 0)
            {
                componentListView.Items[componentListView.Items.Count - 1].Focused = true;
                componentListView.Items[componentListView.Items.Count - 1].Selected = true;
            }
            else
            {
                DisableComponentItems();
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
        private void UpdateEffectList(TechApplication item)
        {
            if (item == null)
            {
                return;
            }

            effectListView.BeginUpdate();
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

                EnableEffectItems();
            }
            else
            {
                DisableEffectItems();
            }

            effectListView.EndUpdate();
        }

        /// <summary>
        ///     技術効果の編集項目を有効化する
        /// </summary>
        private void EnableEffectItems()
        {
            commandTypeComboBox.Enabled = true;
            commandWhichComboBox.Enabled = true;
            commandValueComboBox.Enabled = true;
            commandWhenComboBox.Enabled = true;
            commandWhereComboBox.Enabled = true;

            effectCloneButton.Enabled = true;
            effectRemoveButton.Enabled = true;
        }

        /// <summary>
        ///     技術効果の編集項目を無効化する
        /// </summary>
        private void DisableEffectItems()
        {
            commandTypeComboBox.Text = "";
            commandWhichComboBox.Text = "";
            commandValueComboBox.Text = "";
            commandWhenComboBox.Text = "";
            commandWhereComboBox.Text = "";

            commandTypeComboBox.Enabled = false;
            commandWhichComboBox.Enabled = false;
            commandValueComboBox.Enabled = false;
            commandWhenComboBox.Enabled = false;
            commandWhereComboBox.Enabled = false;

            effectCloneButton.Enabled = false;
            effectRemoveButton.Enabled = false;
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

            var item = techListBox.SelectedItem as TechApplication;
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

            UpdateCommandTypeComboBox(command);
            commandWhichComboBox.Text = command.Which != null ? command.Which.ToString() : "";
            commandValueComboBox.Text = command.Value != null ? command.Value.ToString() : "";
            commandWhenComboBox.Text = command.When != null ? command.When.ToString() : "";
            commandWhereComboBox.Text = command.Where != null ? command.Where.ToString() : "";

            effectUpButton.Enabled = index != 0;
            effectDownButton.Enabled = index != item.Effects.Count - 1;
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

            var item = techListBox.SelectedItem as TechApplication;
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

            item.SetDirty();
        }

        /// <summary>
        ///     技術効果の複製ボタン押下時の処理
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

            var item = techListBox.SelectedItem as TechApplication;
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

            item.SetDirty();
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

            var item = techListBox.SelectedItem as TechApplication;
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

            item.SetDirty();
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

            var item = techListBox.SelectedItem as TechApplication;
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

            item.SetDirty();
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

            var item = techListBox.SelectedItem as TechApplication;
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

            item.SetDirty();
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

            var item = techListBox.SelectedItem as TechApplication;
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

            // コマンドリストの選択項目がなければ何もしない
            if (commandTypeComboBox.SelectedIndex == -1)
            {
                return;
            }

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

            effectListView.Items[index].Text = Command.TypeStrings[(int) newType];
            UpdateCommandTypeComboBox(command);

            item.SetDirty();
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

            var item = techListBox.SelectedItem as TechApplication;
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

            item.SetDirty();
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

            var item = techListBox.SelectedItem as TechApplication;
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

            item.SetDirty();
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

            var item = techListBox.SelectedItem as TechApplication;
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

            item.SetDirty();
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

            var item = techListBox.SelectedItem as TechApplication;
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

            item.SetDirty();
        }

        /// <summary>
        ///     技術効果リストの項目を作成する
        /// </summary>
        /// <param name="command">技術効果</param>
        /// <returns>技術効果リストの項目</returns>
        private static ListViewItem CreateEffectListItem(Command command)
        {
            var listItem = new ListViewItem {Text = Command.TypeStrings[(int) command.Type]};
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

            int index = effectListView.Items.Count - 1;
            effectListView.Items[index].Focused = true;
            effectListView.Items[index].Selected = true;
            effectListView.EnsureVisible(index);

            EnableEffectItems();
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

            effectListView.Items[index].Focused = true;
            effectListView.Items[index].Selected = true;
            effectListView.EnsureVisible(index);
        }

        /// <summary>
        ///     技術効果リストの項目を移動する
        /// </summary>
        /// <param name="src">移動元の位置</param>
        /// <param name="dest">移動先の位置</param>
        private void MoveEffectListItem(int src, int dest)
        {
            var listItem = effectListView.Items[src].Clone() as ListViewItem;
            if (listItem == null)
            {
                return;
            }

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
            effectListView.EnsureVisible(dest);
        }

        /// <summary>
        ///     技術効果リストから項目を削除する
        /// </summary>
        /// <param name="index">削除する項目の位置</param>
        private void RemoveEffectListItem(int index)
        {
            effectListView.Items.RemoveAt(index);

            if (index < effectListView.Items.Count)
            {
                effectListView.Items[index].Focused = true;
                effectListView.Items[index].Selected = true;
            }
            else if (index > 0)
            {
                effectListView.Items[effectListView.Items.Count - 1].Focused = true;
                effectListView.Items[effectListView.Items.Count - 1].Selected = true;
            }
            else
            {
                DisableEffectItems();
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
            labelNameTextBox.Text = Config.GetText(item.Name);
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
        ///     技術ラベル座標の編集項目を有効化する
        /// </summary>
        private void EnableLabelPositionItems()
        {
            labelXNumericUpDown.Enabled = true;
            labelYNumericUpDown.Enabled = true;
            labelPositionRemoveButton.Enabled = true;
        }

        /// <summary>
        ///     技術ラベル座標の編集項目を無効化する
        /// </summary>
        private void DisableLabelPositionItems()
        {
            labelXNumericUpDown.Value = 0;
            labelYNumericUpDown.Value = 0;

            labelXNumericUpDown.Enabled = false;
            labelYNumericUpDown.Enabled = false;
            labelPositionRemoveButton.Enabled = false;
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

                EnableLabelPositionItems();
            }
            else
            {
                DisableLabelPositionItems();
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
            if (newText.Equals(Config.GetText(item.Name)))
            {
                return;
            }

            Config.SetText(item.Name, newText, Game.TechTextFileName);

            // 項目リストボックスの項目を再設定することで表示更新している
            // この時再選択によりフォーカスが外れるので、イベントハンドラを一時的に無効化する
            techListBox.SelectedIndexChanged -= OnTechListBoxSelectedIndexChanged;
            techListBox.Items[techListBox.SelectedIndex] = item;
            techListBox.SelectedIndexChanged += OnTechListBoxSelectedIndexChanged;

            // 技術ツリー上のラベル名を更新する
            foreach (Label label in techTreePictureBox.Controls)
            {
                var info = label.Tag as TechTreeLabelInfo;
                if (info != null && info.Item == item)
                {
                    label.Size = TextRenderer.MeasureText(Config.GetText(item.Name), label.Font);
                    label.Refresh();
                }
            }

            item.SetDirty();
            Config.SetDirty(Game.TechTextFileName, true);
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
            int index = labelPositionListView.SelectedIndices[0];

            TechPosition position = item.Positions[index];

            // 値に変化がなければ何もせずに戻る
            var newX = (int) labelXNumericUpDown.Value;
            if (newX == position.X)
            {
                return;
            }

            position.X = newX;

            labelPositionListView.Items[index].Text = newX.ToString(CultureInfo.InvariantCulture);

            foreach (Label label in techTreePictureBox.Controls)
            {
                var info = label.Tag as TechTreeLabelInfo;
                if (info != null && info.Position == position)
                {
                    label.Location = new Point(position.X, position.Y);
                }
            }

            item.SetDirty();
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
            int index = labelPositionListView.SelectedIndices[0];

            TechPosition position = item.Positions[index];

            // 値に変化がなければ何もせずに戻る
            var newY = (int) labelYNumericUpDown.Value;
            if (newY == position.Y)
            {
                return;
            }

            position.Y = newY;

            labelPositionListView.Items[index].SubItems[1].Text = newY.ToString(CultureInfo.InvariantCulture);

            foreach (Label label in techTreePictureBox.Controls)
            {
                var info = label.Tag as TechTreeLabelInfo;
                if (info != null && info.Position == position)
                {
                    label.Location = new Point(position.X, position.Y);
                }
            }

            item.SetDirty();
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

            var listItem = new ListViewItem {Text = position.X.ToString(CultureInfo.InvariantCulture)};
            listItem.SubItems.Add(position.Y.ToString(CultureInfo.InvariantCulture));
            labelPositionListView.Items.Add(listItem);

            labelPositionListView.Items[labelPositionListView.Items.Count - 1].Focused = true;
            labelPositionListView.Items[labelPositionListView.Items.Count - 1].Selected = true;

            EnableLabelPositionItems();

            AddTechTreeItem(item, position);

            item.SetDirty();
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

            labelPositionListView.Items.RemoveAt(index);

            if (index < labelPositionListView.Items.Count)
            {
                labelPositionListView.Items[index].Focused = true;
                labelPositionListView.Items[index].Selected = true;
            }
            else if (index > 0)
            {
                labelPositionListView.Items[labelPositionListView.Items.Count - 1].Focused = true;
                labelPositionListView.Items[labelPositionListView.Items.Count - 1].Selected = true;
            }
            else
            {
                DisableLabelPositionItems();
            }

            RemoveTechTreeItem(item, position);

            item.SetDirty();
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
            eventTechNumericUpDown.Value = item.TechId;
            eventTechComboBox.SelectedIndex = -1;
            foreach (TechApplication techItem in eventTechComboBox.Items)
            {
                if (techItem.Id == item.TechId)
                {
                    eventTechComboBox.SelectedItem = techItem;
                }
            }
            UpdateEventPositionList(item);
        }

        /// <summary>
        ///     イベントタブの技術リストを更新する
        /// </summary>
        private void UpdateEventTechItems()
        {
            eventTechComboBox.BeginUpdate();

            eventTechComboBox.Items.Clear();

            int maxSize = eventTechComboBox.DropDownWidth;
            foreach (TechApplication item in Techs.List.SelectMany(grp => grp.Items.OfType<TechApplication>()))
            {
                eventTechComboBox.Items.Add(item);
                maxSize = Math.Max(maxSize,
                                   TextRenderer.MeasureText(Config.GetText(item.Name), eventTechComboBox.Font).Width +
                                   SystemInformation.VerticalScrollBarWidth);
            }
            eventTechComboBox.DropDownWidth = maxSize;

            eventTechComboBox.EndUpdate();

            if (techListBox.SelectedItem is TechEvent)
            {
                var eventItem = techListBox.SelectedItem as TechEvent;
                UpdateEventItems(eventItem);
            }
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
            eventTechComboBox.SelectedIndex = -1;
            eventPositionListView.Items.Clear();
            eventXNumericUpDown.Value = 0;
            eventYNumericUpDown.Value = 0;
        }

        /// <summary>
        ///     技術イベント座標の編集項目を有効化する
        /// </summary>
        private void EnableEventPositionItems()
        {
            eventXNumericUpDown.Enabled = true;
            eventYNumericUpDown.Enabled = true;

            eventPositionRemoveButton.Enabled = true;
        }

        /// <summary>
        ///     技術イベント座標の編集項目を無効化する
        /// </summary>
        private void DisableEventPositionItems()
        {
            eventXNumericUpDown.Value = 0;
            eventYNumericUpDown.Value = 0;

            eventXNumericUpDown.Enabled = false;
            eventYNumericUpDown.Enabled = false;
            eventPositionRemoveButton.Enabled = false;
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

                EnableEventPositionItems();
            }
            else
            {
                DisableEventPositionItems();
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

            item.SetDirty();
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
            if (newTechnology == item.TechId)
            {
                return;
            }

            item.TechId = newTechnology;

            eventTechComboBox.SelectedIndex = -1;
            foreach (TechApplication techItem in eventTechComboBox.Items)
            {
                if (techItem.Id == newTechnology)
                {
                    eventTechComboBox.SelectedItem = techItem;
                }
            }

            item.SetDirty();
        }

        /// <summary>
        ///     技術イベント技術変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEventTechComboBoxSelectedIndexChanged(object sender, EventArgs e)
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
            var techItem = eventTechComboBox.SelectedItem as TechApplication;
            if (techItem == null)
            {
                return;
            }
            int newTechnology = techItem.Id;
            if (newTechnology == item.TechId)
            {
                return;
            }

            item.TechId = newTechnology;

            eventTechNumericUpDown.Value = newTechnology;

            item.SetDirty();
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
            int index = eventPositionListView.SelectedIndices[0];

            TechPosition position = item.Positions[index];

            // 値に変化がなければ何もせずに戻る
            var newX = (int) eventXNumericUpDown.Value;
            if (newX == position.X)
            {
                return;
            }

            position.X = newX;

            eventPositionListView.Items[index].Text = newX.ToString(CultureInfo.InvariantCulture);

            foreach (Label label in techTreePictureBox.Controls)
            {
                var info = label.Tag as TechTreeLabelInfo;
                if (info != null && info.Position == position)
                {
                    label.Location = new Point(position.X, position.Y);
                }
            }

            item.SetDirty();
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
            int index = eventPositionListView.SelectedIndices[0];

            TechPosition position = item.Positions[index];

            // 値に変化がなければ何もせずに戻る
            var newY = (int) eventYNumericUpDown.Value;
            if (newY == position.Y)
            {
                return;
            }

            position.Y = newY;

            eventPositionListView.Items[index].SubItems[1].Text = newY.ToString(CultureInfo.InvariantCulture);

            foreach (Label label in techTreePictureBox.Controls)
            {
                var info = label.Tag as TechTreeLabelInfo;
                if (info != null && info.Position == position)
                {
                    label.Location = new Point(position.X, position.Y);
                }
            }

            item.SetDirty();
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

            var listItem = new ListViewItem {Text = position.X.ToString(CultureInfo.InvariantCulture)};
            listItem.SubItems.Add(position.Y.ToString(CultureInfo.InvariantCulture));
            eventPositionListView.Items.Add(listItem);

            eventPositionListView.Items[eventPositionListView.Items.Count - 1].Focused = true;
            eventPositionListView.Items[eventPositionListView.Items.Count - 1].Selected = true;

            EnableEventPositionItems();

            AddTechTreeItem(item, position);

            item.SetDirty();
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

            eventPositionListView.Items.RemoveAt(index);

            if (index < techPositionListView.Items.Count)
            {
                eventPositionListView.Items[index].Focused = true;
                eventPositionListView.Items[index].Selected = true;
            }
            else if (index > 0)
            {
                eventPositionListView.Items[eventPositionListView.Items.Count - 1].Focused = true;
                eventPositionListView.Items[eventPositionListView.Items.Count - 1].Selected = true;
            }
            else
            {
                DisableEventPositionItems();
            }

            RemoveTechTreeItem(item, position);

            item.SetDirty();
        }

        #endregion
    }
}