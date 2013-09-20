using System;
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
        #region 内部フィールド

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

        /// <summary>
        ///     ドラッグアンドドロップの開始位置
        /// </summary>
        private static Point _dragPoint = Point.Empty;

        /// <summary>
        ///     ドラッグ中のカーソル
        /// </summary>
        private static Cursor _dragCursor;

        /// <summary>
        ///     技術ラベルのANDマスク
        /// </summary>
        private static Bitmap _applicationLabelAndMask;

        /// <summary>
        ///     イベントラベルのANDマスク
        /// </summary>
        private static Bitmap _eventLabelAndMask;

        #endregion

        #region 内部定数

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
        ///     技術ツリー画像ファイル名
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

            // 技術ツリーピクチャーボックスへのドラッグアンドドロップを許可する
            // プロパティに存在しないので初期化時に設定する
            treePictureBox.AllowDrop = true;
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechEditorFormLoad(object sender, EventArgs e)
        {
            // 画面解像度が十分に広い場合はツリー画像全体が入るように高さを調整する
            if (Screen.GetWorkingArea(this).Height >= 876)
            {
                Height = 876;
            }

            // 研究特性を初期化する
            Techs.InitSpecialities();

            // ゲーム設定ファイルを読み込む
            Misc.Load();

            // ラベル画像を読み込む
            InitLabelBitmap();

            // 技術定義ファイルを読み込む
            LoadFiles();
        }

        /// <summary>
        ///     編集項目を初期化する
        /// </summary>
        private void InitEditableItems()
        {
            // 小研究タブの編集項目を初期化する
            InitComponentItems();

            // 技術効果タブの編集項目を初期化する
            InitEffectItems();
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
            // 文字列定義ファイルの再読み込みを要求する
            Config.RequireReload();

            // 技術定義ファイルの再読み込みを要求する
            Techs.RequireReload();

            // 技術定義ファイルを読み込む
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
        ///     技術定義ファイルを読み込む
        /// </summary>
        private void LoadFiles()
        {
            // 文字列定義ファイルを読み込む
            Config.Load();

            // 技術定義ファイルを読み込む
            Techs.Load();

            // 編集項目を初期化する
            InitEditableItems();

            // 必要技術タブの技術リストを更新する
            UpdateRequiredTechListItems();

            // 技術イベントタブの技術リストを更新する
            UpdateEventTechListItems();

            // カテゴリリストボックスを初期化する
            InitCategoryList();
        }

        /// <summary>
        ///     技術定義ファイルを保存する
        /// </summary>
        private void SaveFiles()
        {
            // 文字列の一時キーを保存形式に変更する
            int no = 1;
            foreach (TechGroup grp in Techs.Groups)
            {
                foreach (ITechItem item in grp.Items)
                {
                    if (item is TechItem)
                    {
                        var techItem = item as TechItem;
                        techItem.RenameTempKey(Techs.CategoryNames[(int) grp.Category]);
                    }
                    else if (item is TechLabel)
                    {
                        var labelItem = item as TechLabel;
                        labelItem.RenameTempKey(no.ToString(CultureInfo.InvariantCulture));
                        no++;
                    }
                }
            }

            // 文字列定義ファイルを保存する
            Config.Save();

            // 技術定義ファイルを保存する
            Techs.Save();

            // 文字列定義のみ保存の場合、技術名などの編集済みフラグがクリアされないためここで全クリアする
            foreach (TechGroup grp in Techs.Groups)
            {
                grp.ResetDirtyAll();
            }

            // 編集済みフラグがクリアされるため表示を更新する
            categoryListBox.Refresh();
            techListBox.Refresh();
            UpdateEditableItems();
        }

        #endregion

        #region カテゴリリスト

        /// <summary>
        ///     カテゴリリストボックスを初期化する
        /// </summary>
        private void InitCategoryList()
        {
            categoryListBox.Items.Clear();
            foreach (TechGroup grp in Techs.Groups)
            {
                categoryListBox.Items.Add(grp);
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
            // 項目リストを更新する
            UpdateItemList();

            // 技術ツリー画像を更新する
            UpdateTechTreePicture();

            // カテゴリタブの項目を更新する
            UpdateCategoryItems();

            // 技術項目編集タブを無効化する
            DisableTechTab();
            DisableRequiredTab();
            DisableComponentTab();
            DisableEffectTab();
            DisableLabelTab();
            DisableEventTab();

            // カテゴリタブを選択する
            editTabControl.SelectedIndex = (int) TechEditorTab.Category;

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
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            if ((e.State & DrawItemState.Selected) != DrawItemState.Selected)
            {
                // 変更ありの項目は文字色を変更する
                TechGroup grp = Techs.Groups[e.Index];
                brush = grp.IsDirty() ? new SolidBrush(Color.Red) : new SolidBrush(categoryListBox.ForeColor);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            e.Graphics.DrawString(categoryListBox.Items[e.Index].ToString(), e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     選択中の技術グループを取得する
        /// </summary>
        /// <returns>選択中の技術グループ</returns>
        private TechGroup GetSelectedGroup()
        {
            return Techs.Groups[categoryListBox.SelectedIndex];
        }

        #endregion

        #region 技術項目リスト

        /// <summary>
        ///     技術項目リストの表示を更新する
        /// </summary>
        private void UpdateItemList()
        {
            techListBox.BeginUpdate();

            techListBox.Items.Clear();
            treePictureBox.Controls.Clear();

            foreach (ITechItem item in Techs.Groups[categoryListBox.SelectedIndex].Items)
            {
                techListBox.Items.Add(item);
                AddTechTreeItems(item);
            }

            techListBox.EndUpdate();
        }

        /// <summary>
        ///     技術項目リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 編集項目を更新する
            UpdateEditableItems();
        }

        /// <summary>
        ///     編集項目を更新する
        /// </summary>
        private void UpdateEditableItems()
        {
            // 選択項目がない場合
            ITechItem item = GetSelectedItem();
            if (item == null)
            {
                // 技術/必要技術/小研究/技術効果/技術ラベル/技術イベントタブを無効化する
                DisableTechTab();
                DisableRequiredTab();
                DisableComponentTab();
                DisableEffectTab();
                DisableLabelTab();
                DisableEventTab();

                // カテゴリタブを選択する
                editTabControl.SelectedIndex = (int) TechEditorTab.Category;

                return;
            }

            if (item is TechItem)
            {
                // 編集項目を更新する
                var applicationItem = item as TechItem;
                UpdateTechItems(applicationItem);
                UpdateRequiredItems(applicationItem);
                UpdateComponentItems(applicationItem);
                UpdateEffectItems(applicationItem);

                // 技術/必要技術/小研究/技術効果タブを有効化する
                EnableTechTab();
                EnableRequiredTab();
                EnableComponentTab();
                EnableEffectTab();

                // 技術ラベル/技術イベントタブを無効化する
                DisableLabelTab();
                DisableEventTab();

                // 技術/必要技術/小研究/技術効果タブ以外を選択していれば技術タブを選択する
                if (editTabControl.SelectedIndex != (int) TechEditorTab.Tech &&
                    editTabControl.SelectedIndex != (int) TechEditorTab.Required &&
                    editTabControl.SelectedIndex != (int) TechEditorTab.Component &&
                    editTabControl.SelectedIndex != (int) TechEditorTab.Effect)
                {
                    editTabControl.SelectedIndex = (int) TechEditorTab.Tech;
                }
            }
            else if (item is TechLabel)
            {
                // 編集項目を更新する
                UpdateLabelItems(item as TechLabel);

                // 技術ラベルタブを有効化する
                EnableLabelTab();

                // 技術/必要技術/小研究/技術効果/技術イベントタブを無効化する
                DisableTechTab();
                DisableRequiredTab();
                DisableComponentTab();
                DisableEffectTab();
                DisableEventTab();

                // 技術ラベルタブを選択する
                editTabControl.SelectedIndex = (int) TechEditorTab.Label;
            }
            else if (item is TechEvent)
            {
                // 編集項目を更新する
                UpdateEventItems(item as TechEvent);

                // 技術イベントタブを有効化する
                EnableEventTab();

                // 技術/必要技術/小研究/技術効果/技術ラベルタブを無効化する
                DisableTechTab();
                DisableRequiredTab();
                DisableComponentTab();
                DisableEffectTab();
                DisableLabelTab();

                // 技術イベントタブを選択する
                editTabControl.SelectedIndex = (int) TechEditorTab.Event;
            }

            cloneButton.Enabled = true;
            removeButton.Enabled = true;
            topButton.Enabled = techListBox.SelectedIndex != 0;
            upButton.Enabled = techListBox.SelectedIndex != 0;
            downButton.Enabled = techListBox.SelectedIndex != techListBox.Items.Count - 1;
            bottomButton.Enabled = techListBox.SelectedIndex != techListBox.Items.Count - 1;
        }

        /// <summary>
        ///     技術項目リストボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechListBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 背景色を変更する
            if ((e.State & DrawItemState.Selected) == 0)
            {
                if (techListBox.Items[e.Index] is TechLabel)
                {
                    e.Graphics.FillRectangle(Brushes.AliceBlue,
                                             new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
                }
                else if (techListBox.Items[e.Index] is TechEvent)
                {
                    e.Graphics.FillRectangle(Brushes.Honeydew,
                                             new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
                }
            }

            // 項目の文字列を描画する
            Brush brush;
            if ((e.State & DrawItemState.Selected) != DrawItemState.Selected)
            {
                // 変更ありの項目は文字色を変更する
                var item = techListBox.Items[e.Index] as ITechItem;
                brush = (item != null && item.IsDirty())
                            ? new SolidBrush(Color.Red)
                            : new SolidBrush(categoryListBox.ForeColor);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            e.Graphics.DrawString(techListBox.Items[e.Index].ToString(), e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     技術項目リストの新規技術ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNewTechButtonClick(object sender, EventArgs e)
        {
            TechGroup grp = GetSelectedGroup();

            // 項目を作成する
            var item = new TechItem
                {
                    Name = Config.GetTempKey(),
                    ShortName = Config.GetTempKey(),
                    Desc = Config.GetTempKey(),
                    Year = 1936,
                };
            Config.SetText(item.Name, "", Game.TechTextFileName);
            Config.SetText(item.ShortName, "", Game.TechTextFileName);
            Config.SetText(item.Desc, "", Game.TechTextFileName);

            if (techListBox.SelectedItem is ITechItem)
            {
                var selected = techListBox.SelectedItem as ITechItem;

                // 選択項目の先頭座標を引き継ぐ
                item.Positions.Add(new TechPosition {X = selected.Positions[0].X, Y = selected.Positions[0].Y});

                // 選択項目が技術アプリケーションならばIDを10増やす
                if (selected is TechItem)
                {
                    var selectedApplication = selected as TechItem;
                    item.Id = selectedApplication.Id + 10;
                }

                // 技術項目リストに項目を挿入する
                grp.InsertItem(item, selected);

                // 項目リストビューに項目を挿入する
                InsertTechListItem(item, techListBox.SelectedIndex + 1);
            }
            else
            {
                // 仮の座標を登録する
                item.Positions.Add(new TechPosition());

                // 技術項目リストに項目を追加する
                grp.AddItem(item);

                // 項目リストビューに項目を追加する
                AddTechListItem(item);
            }

            // 技術ツリーにラベルを追加する
            AddTechTreeItems(item);

            // 技術項目とIDの対応付けを更新する
            Techs.UpdateTechIdMap();
            // 必要技術コンボボックスの項目を更新する
            UpdateRequiredTechListItems();
            // 技術イベントの技術IDコンボボックスの項目を更新する
            UpdateEventTechListItems();

            // 編集済みフラグを設定する
            grp.SetDirty();
            item.SetDirtyAll();
        }

        /// <summary>
        ///     技術項目リストの新規ラベルボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNewLabelButtonClick(object sender, EventArgs e)
        {
            TechGroup grp = GetSelectedGroup();

            // 項目を作成する
            var item = new TechLabel {Name = Config.GetTempKey()};
            Config.SetText(item.Name, "", Game.TechTextFileName);

            if (techListBox.SelectedItem is ITechItem)
            {
                var selected = techListBox.SelectedItem as ITechItem;

                // 選択項目の先頭座標を引き継ぐ
                item.Positions.Add(new TechPosition {X = selected.Positions[0].X, Y = selected.Positions[0].Y});

                // 技術項目リストに項目を挿入する
                grp.InsertItem(item, selected);

                // 項目リストビューに項目を挿入する
                InsertTechListItem(item, techListBox.SelectedIndex + 1);
            }
            else
            {
                // 仮の座標を登録する
                item.Positions.Add(new TechPosition());

                // 技術項目リストに項目を追加する
                grp.AddItem(item);

                // 項目リストビューに項目を追加する
                AddTechListItem(item);
            }

            // 技術ツリーにラベルを追加する
            AddTechTreeItems(item);

            // 編集済みフラグを設定する
            grp.SetDirty();
            item.SetDirtyAll();
        }

        /// <summary>
        ///     技術項目リストの新規イベントボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNewEventButtonClick(object sender, EventArgs e)
        {
            TechGroup grp = GetSelectedGroup();

            // 項目を作成する
            var item = new TechEvent();

            if (techListBox.SelectedItem is ITechItem)
            {
                var selected = techListBox.SelectedItem as ITechItem;

                // 選択項目の先頭座標を引き継ぐ
                item.Positions.Add(new TechPosition {X = selected.Positions[0].X, Y = selected.Positions[0].Y});

                // 選択項目が技術イベントならばIDを10増やす
                if (selected is TechEvent)
                {
                    var selectedEvent = selected as TechEvent;
                    item.Id = selectedEvent.Id + 10;
                }

                // 技術項目リストに項目を挿入する
                grp.InsertItem(item, selected);

                // 項目リストビューに項目を挿入する
                InsertTechListItem(item, techListBox.SelectedIndex + 1);
            }
            else
            {
                // 仮の座標を登録する
                item.Positions.Add(new TechPosition());

                // 技術項目リストに項目を追加する
                grp.AddItem(item);

                // 項目リストビューに項目を追加する
                AddTechListItem(item);
            }

            // 技術ツリーにラベルを追加する
            AddTechTreeItems(item);

            // 編集済みフラグを設定する
            grp.SetDirty();
            item.SetDirtyAll();
        }

        /// <summary>
        ///     技術項目リストの複製ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloneButtonClick(object sender, EventArgs e)
        {
            TechGroup grp = GetSelectedGroup();

            // 選択項目がなければ何もしない
            ITechItem selected = GetSelectedItem();
            if (selected == null)
            {
                return;
            }

            // 項目を複製する
            ITechItem item = selected.Clone();

            // 技術項目リストに項目を挿入する
            grp.InsertItem(item, selected);

            // 項目リストビューに項目を挿入する
            InsertTechListItem(item, techListBox.SelectedIndex + 1);

            // 技術ツリーにラベルを追加する
            AddTechTreeItems(item);

            if (item is TechItem)
            {
                // 技術項目とIDの対応付けを更新する
                Techs.UpdateTechIdMap();
                // 必要技術コンボボックスの項目を更新する
                UpdateRequiredTechListItems();
                // 技術イベントの技術IDコンボボックスの項目を更新する
                UpdateEventTechListItems();
            }

            // 編集済みフラグを設定する
            grp.SetDirty();
            item.SetDirtyAll();
        }

        /// <summary>
        ///     技術項目リストの削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRemoveButtonClick(object sender, EventArgs e)
        {
            TechGroup grp = GetSelectedGroup();

            // 選択項目がなければ何もしない
            ITechItem selected = GetSelectedItem();
            if (selected == null)
            {
                return;
            }

            // 技術項目リストから項目を削除する
            grp.RemoveItem(selected);

            // 項目リストビューから項目を削除する
            RemoveTechListItem(techListBox.SelectedIndex);

            // 技術ツリーからラベルを削除する
            RemoveTechTreeItems(selected);

            if (selected is TechItem)
            {
                // 技術項目とIDの対応付けを更新する
                var item = selected as TechItem;
                Techs.TechIds.Remove(item.Id);
                Techs.TechIdMap.Remove(item.Id);
                // 必要技術コンボボックスの項目を更新する
                UpdateRequiredTechListItems();
                // 技術イベントの技術IDコンボボックスの項目を更新する
                UpdateEventTechListItems();
            }

            // 項目がなくなれば編集項目を無効化する
            if (techListBox.Items.Count == 0)
            {
                // 技術/必要技術/小研究/技術効果/技術ラベル/技術イベントタブを無効化する
                DisableTechTab();
                DisableRequiredTab();
                DisableComponentTab();
                DisableEffectTab();
                DisableLabelTab();
                DisableEventTab();

                // カテゴリタブを選択する
                editTabControl.SelectedIndex = (int) TechEditorTab.Category;
            }

            // 編集済みフラグを設定する
            grp.SetDirty();
        }

        /// <summary>
        ///     技術項目リストの先頭へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTopButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            ITechItem selected = GetSelectedItem();
            if (selected == null)
            {
                return;
            }

            // 選択項目がリストの先頭ならば何もしない
            int index = techListBox.SelectedIndex;
            if (index == 0)
            {
                return;
            }

            TechGroup grp = GetSelectedGroup();
            ITechItem top = grp.Items[0];
            if (top == null)
            {
                return;
            }

            // 技術項目リストの項目を移動する
            grp.MoveItem(selected, top);

            // 項目リストビューの項目を移動する
            MoveTechListItem(index, 0);

            if (selected is TechItem)
            {
                // 必要技術コンボボックスの項目を更新する
                UpdateRequiredTechListItems();
                // 技術イベントの技術IDコンボボックスの項目を更新する
                UpdateEventTechListItems();
            }

            // 編集済みフラグを設定する
            grp.SetDirty();
        }

        /// <summary>
        ///     技術項目リストの上へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            ITechItem selected = GetSelectedItem();
            if (selected == null)
            {
                return;
            }

            // 選択項目がリストの先頭ならば何もしない
            int index = techListBox.SelectedIndex;
            if (index == 0)
            {
                return;
            }

            TechGroup grp = GetSelectedGroup();
            ITechItem upper = grp.Items[index - 1];

            // 技術項目リストの項目を移動する
            grp.MoveItem(selected, upper);

            // 項目リストビューの項目を移動する
            MoveTechListItem(index, index - 1);

            if (selected is TechItem)
            {
                // 必要技術コンボボックスの項目を更新する
                UpdateRequiredTechListItems();
                // 技術イベントの技術IDコンボボックスの項目を更新する
                UpdateEventTechListItems();
            }

            // 編集済みフラグを設定する
            grp.SetDirty();
        }

        /// <summary>
        ///     技術項目リストの下へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDownButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            ITechItem selected = GetSelectedItem();
            if (selected == null)
            {
                return;
            }

            // 選択項目がリストの末尾ならば何もしない
            int index = techListBox.SelectedIndex;
            if (index == techListBox.Items.Count - 1)
            {
                return;
            }

            TechGroup grp = GetSelectedGroup();
            ITechItem lower = grp.Items[index + 1];

            // 技術項目リストの項目を移動する
            grp.MoveItem(selected, lower);

            // 項目リストビューの項目を移動する
            MoveTechListItem(index, index + 1);

            if (selected is TechItem)
            {
                // 必要技術コンボボックスの項目を更新する
                UpdateRequiredTechListItems();
                // 技術イベントの技術IDコンボボックスの項目を更新する
                UpdateEventTechListItems();
            }

            // 編集済みフラグを設定する
            grp.SetDirty();
        }

        /// <summary>
        ///     技術項目リストの末尾へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBottomButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            ITechItem selected = GetSelectedItem();
            if (selected == null)
            {
                return;
            }

            // 選択項目がリストの末尾ならば何もしない
            int index = techListBox.SelectedIndex;
            if (index == techListBox.Items.Count - 1)
            {
                return;
            }

            TechGroup grp = GetSelectedGroup();
            ITechItem bottom = grp.Items[techListBox.Items.Count - 1];

            // 技術項目リストの項目を移動する
            grp.MoveItem(selected, bottom);

            // 項目リストビューの項目を移動する
            MoveTechListItem(index, techListBox.Items.Count - 1);

            if (selected is TechItem)
            {
                // 必要技術コンボボックスの項目を更新する
                UpdateRequiredTechListItems();
                // 技術イベントの技術IDコンボボックスの項目を更新する
                UpdateEventTechListItems();
            }

            // 編集済みフラグを設定する
            grp.SetDirty();
        }

        /// <summary>
        ///     項目リストビューに項目を追加する
        /// </summary>
        /// <param name="item">追加対象の項目</param>
        private void AddTechListItem(ITechItem item)
        {
            // 項目リストビューに項目を追加する
            techListBox.Items.Add(item);

            // 追加した項目を選択する
            techListBox.SelectedIndex = techListBox.Items.Count - 1;
        }

        /// <summary>
        ///     項目リストビューに項目を挿入する
        /// </summary>
        /// <param name="item">挿入対象の項目</param>
        /// <param name="index">挿入先の位置</param>
        private void InsertTechListItem(object item, int index)
        {
            // 項目リストビューに項目を挿入する
            techListBox.Items.Insert(index, item);

            // 挿入した項目を選択する
            techListBox.SelectedIndex = index;
        }

        /// <summary>
        ///     項目リストビューの項目を削除する
        /// </summary>
        /// <param name="index">削除対象の位置</param>
        private void RemoveTechListItem(int index)
        {
            // 項目リストビューから項目を削除する
            techListBox.Items.RemoveAt(index);

            if (index < techListBox.Items.Count)
            {
                // 削除した項目の次の項目を選択する
                techListBox.SelectedIndex = index;
            }
            else if (index > 0)
            {
                // リストの末尾ならば、削除した項目の前の項目を選択する
                techListBox.SelectedIndex = index - 1;
            }
        }

        /// <summary>
        ///     項目リストビューの項目を移動する
        /// </summary>
        /// <param name="src">移動元の位置</param>
        /// <param name="dest">移動先の位置</param>
        private void MoveTechListItem(int src, int dest)
        {
            var item = techListBox.Items[src] as ITechItem;
            if (item == null)
            {
                return;
            }

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

            // 移動先の項目を選択する
            techListBox.SelectedIndex = dest;
        }

        /// <summary>
        ///     選択中の技術項目を取得する
        /// </summary>
        /// <returns>選択中の技術項目</returns>
        private ITechItem GetSelectedItem()
        {
            return techListBox.SelectedItem as ITechItem;
        }

        #endregion

        #region 技術ツリー

        /// <summary>
        ///     技術ツリー画像を更新する
        /// </summary>
        private void UpdateTechTreePicture()
        {
            TechGroup grp = GetSelectedGroup();
            treePictureBox.ImageLocation = Game.GetReadFileName(Game.PicturePathName,
                                                                TechTreeFileNames[(int) grp.Category]);
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
        ///     技術ツリーに項目を追加する
        /// </summary>
        /// <param name="item">追加対象の項目</param>
        /// <param name="position">追加対象の位置</param>
        private void AddTechTreeItem(ITechItem item, TechPosition position)
        {
            var label = new Label
                {
                    Location = new Point(position.X, position.Y),
                    BackColor = Color.Transparent,
                    Tag = new TechLabelInfo {Item = item, Position = position}
                };

            if (item is TechItem)
            {
                label.Size = new Size(TechLabelWidth, TechLabelHeight);
                label.Image = _techLabelBitmap;
                label.Region = TechLabelRegion;
                label.Paint += OnTechTreeLabelPaint;
            }
            else if (item is TechLabel)
            {
                var labelItem = item as TechLabel;
                label.Size = TextRenderer.MeasureText(labelItem.ToString(), label.Font);
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

            treePictureBox.Controls.Add(label);
        }

        /// <summary>
        ///     技術ツリーの項目群を削除する
        /// </summary>
        /// <param name="item">削除対象の項目</param>
        private void RemoveTechTreeItems(ITechItem item)
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
        private void RemoveTechTreeItem(ITechItem item, TechPosition position)
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
        ///     技術ツリーのラベル描画時の処理
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

            var info = label.Tag as TechLabelInfo;
            if (info == null)
            {
                return;
            }

            if (info.Item is TechItem)
            {
                var item = info.Item as TechItem;
                string s = item.GetShortName();
                if (string.IsNullOrEmpty(s))
                {
                    return;
                }
                Brush brush = new SolidBrush(Color.Black);
                e.Graphics.DrawString(s, label.Font, brush, 6, 2);
                brush.Dispose();
            }
            else if (info.Item is TechLabel)
            {
                var item = info.Item as TechLabel;
                string s = item.ToString();
                if (string.IsNullOrEmpty(s))
                {
                    return;
                }

                // 色指定文字列を解釈する
                Brush brush;
                if ((s[0] == '%' || s[0] == 'ｧ' || s[0] == '§') &&
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
        }

        /// <summary>
        ///     ラベル画像を初期化する
        /// </summary>
        private static void InitLabelBitmap()
        {
            // 技術
            var bitmap = new Bitmap(Game.GetReadFileName(Game.TechLabelPathName));
            _techLabelBitmap = bitmap.Clone(new Rectangle(0, 0, TechLabelWidth, TechLabelHeight),
                                            bitmap.PixelFormat);
            bitmap.Dispose();
            _applicationLabelAndMask = new Bitmap(_techLabelBitmap.Width, _techLabelBitmap.Height);
            Color transparent = _techLabelBitmap.GetPixel(0, 0);
            for (int x = 0; x < _techLabelBitmap.Width; x++)
            {
                for (int y = 0; y < _techLabelBitmap.Height; y++)
                {
                    if (_techLabelBitmap.GetPixel(x, y) == transparent)
                    {
                        TechLabelRegion.Exclude(new Rectangle(x, y, 1, 1));
                        _applicationLabelAndMask.SetPixel(x, y, Color.White);
                    }
                    else
                    {
                        _applicationLabelAndMask.SetPixel(x, y, Color.Black);
                    }
                }
            }
            _techLabelBitmap.MakeTransparent(transparent);

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
            // 左ボタンダウンでなければドラッグ状態を解除する
            if (e.Button != MouseButtons.Left)
            {
                _dragPoint = Point.Empty;
                Cursor.Current = Cursors.Default;
                return;
            }

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

            // 技術項目リストの項目を選択する
            techListBox.SelectedItem = info.Item;

            // 座標リストビューの項目を選択する
            for (int i = 0; i < info.Item.Positions.Count; i++)
            {
                if (info.Item.Positions[i] == info.Position)
                {
                    if (info.Item is TechItem)
                    {
                        techPositionListView.Items[i].Focused = true;
                        techPositionListView.Items[i].Selected = true;
                        techPositionListView.EnsureVisible(i);
                    }
                    else if (info.Item is TechLabel)
                    {
                        labelPositionListView.Items[i].Focused = true;
                        labelPositionListView.Items[i].Selected = true;
                        labelPositionListView.EnsureVisible(i);
                    }
                    else
                    {
                        eventPositionListView.Items[i].Focused = true;
                        eventPositionListView.Items[i].Selected = true;
                        eventPositionListView.EnsureVisible(i);
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
            // ドラッグ状態を解除する
            _dragPoint = Point.Empty;
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

            var label = sender as Label;
            if (label == null)
            {
                return;
            }

            // ドラッグ判定サイズを超えていなければ何もしない
            Size dragSize = SystemInformation.DragSize;
            var dragRect = new Rectangle(_dragPoint.X - dragSize.Width / 2, _dragPoint.Y - dragSize.Height / 2,
                                         dragSize.Width, dragSize.Height);
            if (dragRect.Contains(label.Left + e.X, label.Top + e.Y))
            {
                return;
            }

            var info = label.Tag as TechLabelInfo;
            if (info == null)
            {
                return;
            }

            // カーソル画像を作成する
            var bitmap = new Bitmap(label.Width, label.Height);
            bitmap.MakeTransparent(bitmap.GetPixel(0, 0));
            label.DrawToBitmap(bitmap, new Rectangle(0, 0, label.Width, label.Height));
            if (info.Item is TechItem)
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

            // ドラッグアンドドロップを開始する
            label.DoDragDrop(sender, DragDropEffects.Move);

            // ドラッグ状態を解除する
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
                // カーソル画像を設定する
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
            // ラベルでなければ何もしない
            if (!e.Data.GetDataPresent(typeof (Label)))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            var label = e.Data.GetData(typeof (Label)) as Label;
            if (label == null)
            {
                return;
            }

            // 技術ツリー画像の範囲外ならばドロップを禁止する
            var dragRect = new Rectangle(0, 0, treePictureBox.Image.Width, treePictureBox.Image.Height);
            Point p = treePictureBox.PointToClient(new Point(e.X, e.Y));
            e.Effect = dragRect.Contains(p) ? DragDropEffects.Move : DragDropEffects.None;
        }

        /// <summary>
        ///     技術ツリーピクチャーボックスにドロップした時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTreePictureBoxDragDrop(object sender, DragEventArgs e)
        {
            // ラベルでなければ何もしない
            if (!e.Data.GetDataPresent(typeof (Label)))
            {
                return;
            }

            var label = e.Data.GetData(typeof (Label)) as Label;
            if (label == null)
            {
                return;
            }

            // 技術ツリー上のドロップ座標を計算する
            var p = new Point(e.X, e.Y);
            p = treePictureBox.PointToClient(p);
            p.X = label.Left + p.X - _dragPoint.X;
            p.Y = label.Top + p.Y - _dragPoint.Y;

            // ラベル情報の座標を更新する
            var info = label.Tag as TechLabelInfo;
            if (info == null)
            {
                return;
            }
            info.Position.X = p.X;
            info.Position.Y = p.Y;

            // 座標リストビューの項目を更新する
            for (int i = 0; i < info.Item.Positions.Count; i++)
            {
                if (info.Item.Positions[i] == info.Position)
                {
                    if (info.Item is TechItem)
                    {
                        techPositionListView.Items[i].Text = info.Position.X.ToString(CultureInfo.InvariantCulture);
                        techPositionListView.Items[i].SubItems[1].Text =
                            info.Position.Y.ToString(CultureInfo.InvariantCulture);
                        techXNumericUpDown.Value = info.Position.X;
                        techYNumericUpDown.Value = info.Position.Y;
                    }
                    else if (info.Item is TechLabel)
                    {
                        labelPositionListView.Items[i].Text = info.Position.X.ToString(CultureInfo.InvariantCulture);
                        labelPositionListView.Items[i].SubItems[1].Text =
                            info.Position.Y.ToString(CultureInfo.InvariantCulture);
                        labelXNumericUpDown.Value = info.Position.X;
                        labelYNumericUpDown.Value = info.Position.Y;
                    }
                    else
                    {
                        eventPositionListView.Items[i].Text = info.Position.X.ToString(CultureInfo.InvariantCulture);
                        eventPositionListView.Items[i].SubItems[1].Text =
                            info.Position.Y.ToString(CultureInfo.InvariantCulture);
                        eventXNumericUpDown.Value = info.Position.X;
                        eventYNumericUpDown.Value = info.Position.Y;
                    }
                    break;
                }
            }

            // ラベルの座標を更新する
            label.Location = p;

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            info.Item.SetDirty();
            info.Position.SetDirtyAll();
        }

        /// <summary>
        ///     技術ラベルに関連付けられる情報
        /// </summary>
        private class TechLabelInfo
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
        ///     カテゴリタブの項目を更新する
        /// </summary>
        private void UpdateCategoryItems()
        {
            // 編集項目の値を更新する
            TechGroup grp = GetSelectedGroup();
            categoryNameTextBox.Text = grp.ToString();
            categoryDescTextBox.Text = grp.GetDesc();

            // 編集項目の色を更新する
            categoryNameTextBox.ForeColor = grp.IsDirty(TechGroupItemId.Name) ? Color.Red : SystemColors.WindowText;
            categoryDescTextBox.ForeColor = grp.IsDirty(TechGroupItemId.Desc) ? Color.Red : SystemColors.WindowText;
        }

        /// <summary>
        ///     技術グループ名変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCategoryNameTextBoxTextChanged(object sender, EventArgs e)
        {
            TechGroup grp = GetSelectedGroup();

            // 値に変化がなければ何もしない
            string name = categoryNameTextBox.Text;
            if (name.Equals(grp.ToString()))
            {
                return;
            }

            // 値を更新する
            Config.SetText(grp.Name, name, Game.TechTextFileName);

            // カテゴリリストボックスの項目を再設定することで表示更新している
            // この時再選択によりフォーカスが外れるので、イベントハンドラを一時的に無効化する
            categoryListBox.SelectedIndexChanged -= OnCategoryListBoxSelectedIndexChanged;
            categoryListBox.Items[(int) grp.Category] = name;
            categoryListBox.SelectedIndexChanged += OnCategoryListBoxSelectedIndexChanged;

            // 編集済みフラグを設定する
            Config.SetDirty(Game.TechTextFileName);

            // 文字色を変更する
            categoryNameTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     技術グループ説明変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCategoryDescTextBoxTextChanged(object sender, EventArgs e)
        {
            TechGroup grp = GetSelectedGroup();

            // 値に変化がなければ何もしない
            string desc = categoryDescTextBox.Text;
            if (desc.Equals(grp.GetDesc()))
            {
                return;
            }

            // 値を更新する
            Config.SetText(grp.Desc, desc, Game.TechTextFileName);

            // 編集済みフラグを設定する
            Config.SetDirty(Game.TechTextFileName);

            // 文字色を変更する
            categoryDescTextBox.ForeColor = Color.Red;
        }

        #endregion

        #region 技術タブ

        /// <summary>
        ///     技術タブの項目を更新する
        /// </summary>
        /// <param name="item">技術アプリケーション</param>
        private void UpdateTechItems(TechItem item)
        {
            // 編集項目の値を更新する
            techNameTextBox.Text = item.ToString();
            techShortNameTextBox.Text = item.GetShortName();
            techIdNumericUpDown.Value = item.Id;
            techYearNumericUpDown.Value = item.Year;
            UpdateTechPositionList(item);
            UpdateTechPicture(item);

            // 編集項目の色を更新する
            techNameTextBox.ForeColor = item.IsDirty(TechItemId.Name) ? Color.Red : SystemColors.WindowText;
            techShortNameTextBox.ForeColor = item.IsDirty(TechItemId.ShortName) ? Color.Red : SystemColors.WindowText;
            techIdNumericUpDown.ForeColor = item.IsDirty(TechItemId.Id) ? Color.Red : SystemColors.WindowText;
            techYearNumericUpDown.ForeColor = item.IsDirty(TechItemId.Year) ? Color.Red : SystemColors.WindowText;
            techPictureNameTextBox.ForeColor = item.IsDirty(TechItemId.PictureName)
                                                   ? Color.Red
                                                   : SystemColors.WindowText;
        }

        /// <summary>
        ///     技術タブを有効化する
        /// </summary>
        private void EnableTechTab()
        {
            // タブを有効化する
            editTabControl.TabPages[(int) TechEditorTab.Tech].Enabled = true;

            // 無効化時にクリアした値を再設定する
            techIdNumericUpDown.Text = techIdNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            techYearNumericUpDown.Text = techYearNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     技術タブを無効化する
        /// </summary>
        private void DisableTechTab()
        {
            // タブを無効化する
            editTabControl.TabPages[(int) TechEditorTab.Tech].Enabled = false;

            // 編集項目の値をクリアする
            techNameTextBox.ResetText();
            techShortNameTextBox.ResetText();
            techIdNumericUpDown.ResetText();
            techYearNumericUpDown.ResetText();
            techPositionListView.Items.Clear();
            techXNumericUpDown.ResetText();
            techYNumericUpDown.ResetText();
            techPictureBox.Image = null;
        }

        /// <summary>
        ///     技術座標リストを更新する
        /// </summary>
        /// <param name="item">技術</param>
        private void UpdateTechPositionList(TechItem item)
        {
            techPositionListView.BeginUpdate();
            techPositionListView.Items.Clear();

            foreach (TechPosition position in item.Positions)
            {
                var li = new ListViewItem(position.X.ToString(CultureInfo.InvariantCulture));
                li.SubItems.Add(position.Y.ToString(CultureInfo.InvariantCulture));
                techPositionListView.Items.Add(li);
            }

            if (techPositionListView.Items.Count > 0)
            {
                // 先頭の項目を選択する
                techPositionListView.Items[0].Focused = true;
                techPositionListView.Items[0].Selected = true;

                // 編集項目を有効化する
                EnableTechPositionItems();
            }
            else
            {
                // 編集項目を無効化する
                DisableTechPositionItems();
            }

            techPositionListView.EndUpdate();
        }

        /// <summary>
        ///     技術座標の編集項目を有効化する
        /// </summary>
        private void EnableTechPositionItems()
        {
            // 無効化時にクリアした値を再設定する
            techXNumericUpDown.Text = techXNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            techYNumericUpDown.Text = techYNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);

            // 編集項目を有効化する
            techXNumericUpDown.Enabled = true;
            techYNumericUpDown.Enabled = true;

            techPositionRemoveButton.Enabled = true;
        }

        /// <summary>
        ///     技術座標の編集項目を無効化する
        /// </summary>
        private void DisableTechPositionItems()
        {
            // 編集項目の値をクリアする
            techXNumericUpDown.ResetText();
            techYNumericUpDown.ResetText();

            // 編集項目を無効化する
            techXNumericUpDown.Enabled = false;
            techYNumericUpDown.Enabled = false;

            techPositionRemoveButton.Enabled = false;
        }

        /// <summary>
        ///     技術名変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechNameTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            string name = techNameTextBox.Text;
            if (name.Equals(item.ToString()))
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
            item.SetDirty(TechItemId.Name);
            Config.SetDirty(Game.TechTextFileName);

            // 文字色を変更する
            techNameTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     技術短縮名変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechShortNameTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            string shortName = techShortNameTextBox.Text;
            if (shortName.Equals(item.GetShortName()))
            {
                return;
            }

            // 値を更新する
            Config.SetText(item.ShortName, shortName, Game.TechTextFileName);

            // 技術ツリー上のラベル名を更新する
            foreach (Label label in treePictureBox.Controls)
            {
                var info = label.Tag as TechLabelInfo;
                if (info != null && info.Item == item)
                {
                    label.Refresh();
                }
            }

            // 編集済みフラグを設定する
            item.SetDirty(TechItemId.ShortName);
            Config.SetDirty(Game.TechTextFileName);

            // 文字色を変更する
            techShortNameTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     技術ID変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechIdNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var id = (int) techIdNumericUpDown.Value;
            if (id == item.Id)
            {
                return;
            }

            // 値を更新する
            Techs.ModifyTechId(item, id);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty(TechItemId.Id);

            // 文字色を変更する
            techIdNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     史実年度変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechYearNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var year = (int) techYearNumericUpDown.Value;
            if (year == item.Year)
            {
                return;
            }

            // 値を更新する
            item.Year = year;

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty(TechItemId.Year);

            // 文字色を変更する
            techYearNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     技術座標リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechPositionListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            // 技術座標リストの選択項目がなければ編集項目を無効化する
            if (techPositionListView.SelectedIndices.Count == 0)
            {
                DisableTechPositionItems();
                return;
            }

            // 編集項目を更新する
            TechPosition position = item.Positions[techPositionListView.SelectedIndices[0]];
            techXNumericUpDown.Value = position.X;
            techYNumericUpDown.Value = position.Y;

            // 編集項目の色を更新する
            techXNumericUpDown.ForeColor = position.IsDirty(TechPositionItemId.X) ? Color.Red : SystemColors.WindowText;
            techYNumericUpDown.ForeColor = position.IsDirty(TechPositionItemId.Y) ? Color.Red : SystemColors.WindowText;

            // 編集項目を有効化する
            EnableTechPositionItems();
        }

        /// <summary>
        ///     技術X座標変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechXNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
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

            // 値に変化がなければ何もしない
            var x = (int) techXNumericUpDown.Value;
            if (x == position.X)
            {
                return;
            }

            // 値を更新する
            position.X = x;

            // 座標リストビューの表示を更新する
            techPositionListView.Items[index].Text = x.ToString(CultureInfo.InvariantCulture);

            // ラベルの位置を更新する
            foreach (Label label in treePictureBox.Controls)
            {
                var info = label.Tag as TechLabelInfo;
                if (info != null && info.Position == position)
                {
                    label.Location = new Point(position.X, position.Y);
                }
            }

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            position.SetDirty(TechPositionItemId.X);

            // 文字色を変更する
            techXNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     技術Y座標変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechYNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
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

            // 値に変化がなければ何もしない
            var y = (int) techYNumericUpDown.Value;
            if (y == position.Y)
            {
                return;
            }

            // 値を更新する
            position.Y = y;

            // 座標リストビューの表示を更新する
            techPositionListView.Items[index].SubItems[1].Text = y.ToString(CultureInfo.InvariantCulture);

            // ラベルの位置を更新する
            foreach (Label label in treePictureBox.Controls)
            {
                var info = label.Tag as TechLabelInfo;
                if (info != null && info.Position == position)
                {
                    label.Location = new Point(position.X, position.Y);
                }
            }

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            position.SetDirty(TechPositionItemId.Y);

            // 文字色を変更する
            techYNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     技術座標追加ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechPositionAddButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            // 座標をリストに追加する
            var position = new TechPosition {X = 0, Y = 0};
            item.Positions.Add(position);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            position.SetDirtyAll();

            // 座標リストビューの項目を追加する
            var li = new ListViewItem {Text = position.X.ToString(CultureInfo.InvariantCulture)};
            li.SubItems.Add(position.Y.ToString(CultureInfo.InvariantCulture));
            techPositionListView.Items.Add(li);

            // 追加した項目を選択する
            techPositionListView.Items[techPositionListView.Items.Count - 1].Focused = true;
            techPositionListView.Items[techPositionListView.Items.Count - 1].Selected = true;
            techPositionListView.EnsureVisible(techPositionListView.Items.Count - 1);

            // 編集項目を有効化する
            EnableTechPositionItems();

            // 技術ツリーにラベルを追加する
            AddTechTreeItem(item, position);
        }

        /// <summary>
        ///     技術座標削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechPositionRemoveButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
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

            // 座標をリストから削除する
            item.Positions.RemoveAt(index);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();

            // 座標リストビューの項目を削除する
            techPositionListView.Items.RemoveAt(index);

            if (index < techPositionListView.Items.Count)
            {
                // 削除した項目の次の項目を選択する
                techPositionListView.Items[index].Focused = true;
                techPositionListView.Items[index].Selected = true;
            }
            else if (index > 0)
            {
                // リストの末尾ならば、削除した項目の前の項目を選択する
                techPositionListView.Items[techPositionListView.Items.Count - 1].Focused = true;
                techPositionListView.Items[techPositionListView.Items.Count - 1].Selected = true;
            }
            else
            {
                // 項目がなくなれば編集項目を無効化する
                DisableTechPositionItems();
            }

            // 技術ツリーからラベルを削除する
            RemoveTechTreeItem(item, position);
        }

        /// <summary>
        ///     技術画像を更新する
        /// </summary>
        /// <param name="item">技術アプリケーション</param>
        private void UpdateTechPicture(TechItem item)
        {
            // 画像ファイル名テキストボックスの値を更新する
            techPictureNameTextBox.Text = item.PictureName ?? "";

            // 編集項目の色を更新する
            techPictureNameTextBox.ForeColor = item.IsDirty(TechItemId.PictureName)
                                                   ? Color.Red
                                                   : SystemColors.WindowText;

            string fileName =
                Game.GetReadFileName(Game.TechPicturePathName,
                                     string.Format("{0}.bmp",
                                                   string.IsNullOrEmpty(item.PictureName)
                                                       ? item.Id.ToString(CultureInfo.InvariantCulture)
                                                       : item.PictureName));
            if (File.Exists(fileName))
            {
                // 技術画像を更新する
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
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            string pictureName = techPictureNameTextBox.Text;
            if (pictureName.Equals(item.PictureName))
            {
                return;
            }

            // 値を更新する
            item.PictureName = pictureName;

            // 技術画像を更新する
            UpdateTechPicture(item);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty(TechItemId.PictureName);

            // 文字色を変更する
            techPictureNameTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     画像ファイル名参照ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechPictureNameBrowseButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
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
                // 画像ファイル名テキストボックスの値を更新する
                techPictureNameTextBox.Text = Path.GetFileNameWithoutExtension(dialog.FileName);
            }
        }

        #endregion

        #region 必要技術タブ

        /// <summary>
        ///     必要技術タブの項目を更新する
        /// </summary>
        /// <param name="item">技術アプリケーション</param>
        private void UpdateRequiredItems(TechItem item)
        {
            UpdateAndRequiredList(item);
            UpdateOrRequiredList(item);
        }

        /// <summary>
        ///     必要技術タブを有効化する
        /// </summary>
        private void EnableRequiredTab()
        {
            // タブを有効化する
            editTabControl.TabPages[(int) TechEditorTab.Required].Enabled = true;
        }

        /// <summary>
        ///     必要技術タブを無効化する
        /// </summary>
        private void DisableRequiredTab()
        {
            // タブを無効化する
            editTabControl.TabPages[(int) TechEditorTab.Required].Enabled = false;

            // 必要技術リストをクリアする
            andRequiredListView.Items.Clear();
            orRequiredListView.Items.Clear();

            // 編集項目を無効化する
            DisableAndRequiredItems();
            DisableOrRequiredItems();
        }

        /// <summary>
        ///     必要技術タブの技術リストを更新する
        /// </summary>
        private void UpdateRequiredTechListItems()
        {
            andTechComboBox.BeginUpdate();
            orTechComboBox.BeginUpdate();

            andTechComboBox.Items.Clear();
            orTechComboBox.Items.Clear();

            int maxWidth = andTechComboBox.DropDownWidth;
            foreach (TechItem item in Techs.TechIdMap.Select(pair => pair.Value))
            {
                andTechComboBox.Items.Add(item);
                orTechComboBox.Items.Add(item);
                maxWidth = Math.Max(maxWidth,
                                    TextRenderer.MeasureText(item.ToString(), andTechComboBox.Font).Width +
                                    SystemInformation.VerticalScrollBarWidth);
            }
            andTechComboBox.DropDownWidth = maxWidth;
            orTechComboBox.DropDownWidth = maxWidth;

            andTechComboBox.EndUpdate();
            orTechComboBox.EndUpdate();
        }

        /// <summary>
        ///     AND条件必要技術リストを更新する
        /// </summary>
        /// <param name="item">技術アプリケーション</param>
        private void UpdateAndRequiredList(TechItem item)
        {
            andRequiredListView.BeginUpdate();
            andRequiredListView.Items.Clear();

            foreach (int id in item.AndRequiredTechs.Select(tech => tech.Id))
            {
                var li = new ListViewItem {Text = id.ToString(CultureInfo.InvariantCulture)};
                if (Techs.TechIdMap.ContainsKey(id))
                {
                    li.SubItems.Add(Techs.TechIdMap[id].ToString());
                }
                andRequiredListView.Items.Add(li);
            }

            if (andRequiredListView.Items.Count > 0)
            {
                // 先頭の項目を選択する
                andRequiredListView.Items[0].Focused = true;
                andRequiredListView.Items[0].Selected = true;

                // 編集項目を有効化する
                EnableAndRequiredItems();
            }
            else
            {
                // 編集項目を無効化する
                DisableAndRequiredItems();
            }

            andRequiredListView.EndUpdate();
        }


        /// <summary>
        ///     OR条件必要技術リストを更新する
        /// </summary>
        /// <param name="item">技術アプリケーション</param>
        private void UpdateOrRequiredList(TechItem item)
        {
            orRequiredListView.BeginUpdate();
            orRequiredListView.Items.Clear();

            foreach (int id in item.OrRequiredTechs.Select(tech => tech.Id))
            {
                var li = new ListViewItem {Text = id.ToString(CultureInfo.InvariantCulture)};
                if (Techs.TechIdMap.ContainsKey(id))
                {
                    li.SubItems.Add(Techs.TechIdMap[id].ToString());
                }
                orRequiredListView.Items.Add(li);
            }

            if (orRequiredListView.Items.Count > 0)
            {
                // 先頭の項目を選択する
                orRequiredListView.Items[0].Focused = true;
                orRequiredListView.Items[0].Selected = true;

                // 編集項目を有効化する
                EnableOrRequiredItems();
            }
            else
            {
                // 編集項目を無効化する
                DisableOrRequiredItems();
            }

            orRequiredListView.EndUpdate();
        }

        /// <summary>
        ///     AND条件必要技術の編集項目を有効化する
        /// </summary>
        private void EnableAndRequiredItems()
        {
            // 無効化時にクリアした値を再設定する
            andIdNumericUpDown.Text = andIdNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);

            // 編集項目を有効化する
            andIdNumericUpDown.Enabled = true;
            andTechComboBox.Enabled = true;

            andRemoveButton.Enabled = true;
        }

        /// <summary>
        ///     AND条件必要技術の編集項目を無効化する
        /// </summary>
        private void DisableAndRequiredItems()
        {
            // 編集項目の値をクリアする
            andIdNumericUpDown.ResetText();
            andTechComboBox.SelectedIndex = -1;
            andTechComboBox.ResetText();

            // 編集項目を無効化する
            andIdNumericUpDown.Enabled = false;
            andTechComboBox.Enabled = false;

            andRemoveButton.Enabled = false;
        }

        /// <summary>
        ///     OR条件必要技術の編集項目を有効化する
        /// </summary>
        private void EnableOrRequiredItems()
        {
            // 無効化時にクリアした値を再設定する
            orIdNumericUpDown.Text = orIdNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);

            // 編集項目を有効化する
            orIdNumericUpDown.Enabled = true;
            orTechComboBox.Enabled = true;

            orRemoveButton.Enabled = true;
        }

        /// <summary>
        ///     OR条件必要技術の編集項目を無効化する
        /// </summary>
        private void DisableOrRequiredItems()
        {
            // 編集項目の値をクリアする
            orIdNumericUpDown.ResetText();
            orTechComboBox.SelectedIndex = -1;
            orTechComboBox.ResetText();

            // 編集項目を無効化する
            orIdNumericUpDown.Enabled = false;
            orTechComboBox.Enabled = false;

            orRemoveButton.Enabled = false;
        }

        /// <summary>
        ///     AND条件必要技術コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAndTechComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            var item = GetSelectedItem() as TechItem;
            if (item != null && andRequiredListView.SelectedIndices.Count > 0)
            {
                RequiredTech tech = item.AndRequiredTechs[andRequiredListView.SelectedIndices[0]];
                Brush brush;
                if ((Techs.TechIds[e.Index] == tech.Id) && tech.IsDirty())
                {
                    brush = new SolidBrush(Color.Red);
                }
                else
                {
                    brush = new SolidBrush(SystemColors.WindowText);
                }
                string s = andTechComboBox.Items[e.Index].ToString();
                e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     OR条件必要技術コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOrTechComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            var item = GetSelectedItem() as TechItem;
            if (item != null && orRequiredListView.SelectedIndices.Count > 0)
            {
                RequiredTech tech = item.OrRequiredTechs[orRequiredListView.SelectedIndices[0]];
                Brush brush;
                if ((Techs.TechIds[e.Index] == tech.Id) && tech.IsDirty())
                {
                    brush = new SolidBrush(Color.Red);
                }
                else
                {
                    brush = new SolidBrush(SystemColors.WindowText);
                }
                string s = orTechComboBox.Items[e.Index].ToString();
                e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     AND条件必要技術リストの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAndRequiredListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 技術項目リストの選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            // 必要技術リストの選択項目がなければ編集項目を無効化する
            if (andRequiredListView.SelectedIndices.Count == 0)
            {
                DisableAndRequiredItems();
                return;
            }

            // 編集項目を更新する
            RequiredTech tech = item.AndRequiredTechs[andRequiredListView.SelectedIndices[0]];
            int id = tech.Id;
            andIdNumericUpDown.Value = id;

            // コンボボックスの色を更新する
            andTechComboBox.Refresh();

            // 編集項目の色を更新する
            andIdNumericUpDown.ForeColor = tech.IsDirty() ? Color.Red : SystemColors.WindowText;

            // AND条件必要技術コンボボックスの選択項目を更新する
            if (Techs.TechIds.Contains(id))
            {
                andTechComboBox.SelectedIndex = Techs.TechIds.IndexOf(id);
            }
            else
            {
                andTechComboBox.SelectedIndex = -1;
                andTechComboBox.ResetText();
            }

            // 編集項目を有効化する
            EnableAndRequiredItems();
        }

        /// <summary>
        ///     OR条件必要技術リストの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOrRequiredListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 技術項目リストの選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            // 必要技術リストの選択項目がなければ編集項目を無効化する
            if (orRequiredListView.SelectedIndices.Count == 0)
            {
                DisableOrRequiredItems();
                return;
            }

            // 編集項目を更新する
            RequiredTech tech = item.OrRequiredTechs[orRequiredListView.SelectedIndices[0]];
            int id = tech.Id;
            orIdNumericUpDown.Value = id;

            // コンボボックスの色を更新する
            orTechComboBox.Refresh();

            // 編集項目の色を更新する
            orIdNumericUpDown.ForeColor = tech.IsDirty() ? Color.Red : SystemColors.WindowText;

            // OR条件必要技術コンボボックスの選択項目を更新する
            if (Techs.TechIds.Contains(id))
            {
                orTechComboBox.SelectedIndex = Techs.TechIds.IndexOf(id);
            }
            else
            {
                orTechComboBox.SelectedIndex = -1;
                orTechComboBox.ResetText();
            }

            // 編集項目を有効化する
            EnableOrRequiredItems();
        }

        /// <summary>
        ///     AND条件必要技術追加ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAndAddButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            // AND条件必要技術リストに項目を追加する
            var tech = new RequiredTech();
            item.AndRequiredTechs.Add(tech);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            tech.SetDirty();

            // AND条件必要技術リストビューに項目を追加する
            AddAndRequiredListItem(0);
        }

        /// <summary>
        ///     OR条件必要技術追加ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOrAddButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            // OR条件必要技術リストに項目を追加する
            var tech = new RequiredTech();
            item.OrRequiredTechs.Add(tech);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            tech.SetDirty();

            // OR条件必要技術リストビューに項目を追加する
            AddOrRequiredListItem(0);
        }

        /// <summary>
        ///     AND条件必要技術削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAndRemoveButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            if (andRequiredListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = andRequiredListView.SelectedIndices[0];

            // AND条件必要技術リストから項目を削除する
            RemoveAndRequiredListItem(index);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();

            // AND条件必要技術リストビューから項目を削除する
            item.AndRequiredTechs.RemoveAt(index);
        }

        /// <summary>
        ///     OR条件必要技術削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOrRemoveButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            if (orRequiredListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = orRequiredListView.SelectedIndices[0];

            // OR条件必要技術リストから項目を削除する
            RemoveOrRequiredListItem(index);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();

            // OR条件必要技術リストビューから項目を削除する
            item.OrRequiredTechs.RemoveAt(index);
        }

        /// <summary>
        ///     AND条件必要技術ID変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAndIdNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            if (andRequiredListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = andRequiredListView.SelectedIndices[0];

            // 値に変化がなければ何もしない
            RequiredTech tech = item.AndRequiredTechs[index];
            var id = (int) andIdNumericUpDown.Value;
            if (id == tech.Id)
            {
                return;
            }

            // 値を更新する
            tech.Id = id;

            // AND条件必要技術コンボボックスの選択項目を更新する
            if (Techs.TechIds.Contains(id))
            {
                andTechComboBox.SelectedIndex = Techs.TechIds.IndexOf(id);
            }
            else
            {
                andTechComboBox.SelectedIndex = -1;
                andTechComboBox.ResetText();
            }

            // AND条件必要技術リストの項目を更新する
            ModifyAndRequiredListItem(id, index);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            tech.SetDirty();

            // 文字色を変更する
            andIdNumericUpDown.ForeColor = Color.Red;

            // AND条件必要技術コンボボックスの項目色を変更するため描画更新する
            andTechComboBox.Refresh();
        }

        /// <summary>
        ///     OR条件必要技術ID変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOrIdNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            if (orRequiredListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = orRequiredListView.SelectedIndices[0];

            // 値に変化がなければ何もしない
            RequiredTech tech = item.OrRequiredTechs[index];
            var id = (int) orIdNumericUpDown.Value;
            if (id == tech.Id)
            {
                return;
            }

            // 値を更新する
            tech.Id = id;

            // OR条件必要技術コンボボックスの選択項目を更新する
            orTechComboBox.SelectedIndex = -1;
            if (Techs.TechIds.Contains(id))
            {
                orTechComboBox.SelectedIndex = Techs.TechIds.IndexOf(id);
            }
            else
            {
                orTechComboBox.SelectedIndex = -1;
                orTechComboBox.ResetText();
            }

            // OR条件必要技術リストの項目を更新する
            ModifyOrRequiredListItem(id, index);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            tech.SetDirty();

            // 文字色を変更する
            orIdNumericUpDown.ForeColor = Color.Red;

            // OR条件必要技術コンボボックスの項目色を変更するため描画更新する
            orTechComboBox.Refresh();
        }

        /// <summary>
        ///     AND条件必要技術変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAndTechComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            if (andRequiredListView.SelectedIndices.Count == 0)
            {
                return;
            }

            // 値に変化がなければ何もしない
            int index = andRequiredListView.SelectedIndices[0];
            if (index == -1)
            {
                return;
            }
            RequiredTech tech = item.AndRequiredTechs[index];
            if (andTechComboBox.SelectedIndex == -1)
            {
                return;
            }
            int id = Techs.TechIds[andTechComboBox.SelectedIndex];
            if (id == tech.Id)
            {
                return;
            }

            // AND条件必要技術IDの値を更新する
            andIdNumericUpDown.Value = id;
        }

        /// <summary>
        ///     OR条件必要技術変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOrTechComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            if (orRequiredListView.SelectedIndices.Count == 0)
            {
                return;
            }

            // 値に変化がなければ何もしない
            int index = orRequiredListView.SelectedIndices[0];
            if (index == -1)
            {
                return;
            }
            RequiredTech tech = item.OrRequiredTechs[index];
            if (orTechComboBox.SelectedIndex == -1)
            {
                return;
            }
            int id = Techs.TechIds[orTechComboBox.SelectedIndex];
            if (id == tech.Id)
            {
                return;
            }

            // OR条件必要技術IDの値を更新する
            orIdNumericUpDown.Value = id;
        }

        /// <summary>
        ///     AND条件必要技術リストビューに項目を追加する
        /// </summary>
        /// <param name="id">必要技術ID</param>
        private void AddAndRequiredListItem(int id)
        {
            // リストに項目を追加する
            var li = new ListViewItem {Text = id.ToString(CultureInfo.InvariantCulture)};
            if (Techs.TechIdMap.ContainsKey(id))
            {
                li.SubItems.Add(Techs.TechIdMap[id].ToString());
            }
            andRequiredListView.Items.Add(li);

            // 追加した項目を選択する
            int index = andRequiredListView.Items.Count - 1;
            andRequiredListView.Items[index].Focused = true;
            andRequiredListView.Items[index].Selected = true;
            andRequiredListView.EnsureVisible(index);

            // 編集項目を有効化する
            EnableAndRequiredItems();
        }

        /// <summary>
        ///     OR条件必要技術リストビューに項目を追加する
        /// </summary>
        /// <param name="id">必要技術ID</param>
        private void AddOrRequiredListItem(int id)
        {
            // リストに項目を追加する
            var li = new ListViewItem {Text = id.ToString(CultureInfo.InvariantCulture)};
            if (Techs.TechIdMap.ContainsKey(id))
            {
                li.SubItems.Add(Techs.TechIdMap[id].ToString());
            }
            orRequiredListView.Items.Add(li);

            // 追加した項目を選択する
            int index = orRequiredListView.Items.Count - 1;
            orRequiredListView.Items[index].Focused = true;
            orRequiredListView.Items[index].Selected = true;
            orRequiredListView.EnsureVisible(index);

            // 編集項目を有効化する
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
            if (Techs.TechIdMap.ContainsKey(id))
            {
                andRequiredListView.Items[index].SubItems.Add(Techs.TechIdMap[id].ToString());
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
            if (Techs.TechIdMap.ContainsKey(id))
            {
                orRequiredListView.Items[index].SubItems.Add(Techs.TechIdMap[id].ToString());
            }
        }

        /// <summary>
        ///     AND条件必要技術リストの項目を削除する
        /// </summary>
        /// <param name="index">削除対象の項目インデックス</param>
        private void RemoveAndRequiredListItem(int index)
        {
            // リストから項目を削除する
            andRequiredListView.Items[index].Remove();

            if (index < andRequiredListView.Items.Count)
            {
                // 削除した項目の次の項目を選択する
                andRequiredListView.Items[index].Focused = true;
                andRequiredListView.Items[index].Selected = true;
            }
            else if (index > 0)
            {
                // リストの末尾ならば削除した項目の前の項目を選択する
                andRequiredListView.Items[andRequiredListView.Items.Count - 1].Focused = true;
                andRequiredListView.Items[andRequiredListView.Items.Count - 1].Selected = true;
            }
            else
            {
                // 項目がなくなれば編集項目を無効化する
                DisableAndRequiredItems();
            }
        }

        /// <summary>
        ///     OR条件必要技術リストの項目を削除する
        /// </summary>
        /// <param name="index">削除対象の項目インデックス</param>
        private void RemoveOrRequiredListItem(int index)
        {
            // リストから項目を削除する
            orRequiredListView.Items[index].Remove();

            if (index < orRequiredListView.Items.Count)
            {
                // 削除した項目の次の項目を選択する
                orRequiredListView.Items[index].Focused = true;
                orRequiredListView.Items[index].Selected = true;
            }
            else if (index > 0)
            {
                // リストの末尾ならば削除した項目の前の項目を選択する
                orRequiredListView.Items[orRequiredListView.Items.Count - 1].Focused = true;
                orRequiredListView.Items[orRequiredListView.Items.Count - 1].Selected = true;
            }
            else
            {
                // 項目がなくなれば編集項目を無効化する
                DisableOrRequiredItems();
            }
        }

        #endregion

        #region 小研究タブ

        /// <summary>
        ///     小研究タブの項目を初期化する
        /// </summary>
        private void InitComponentItems()
        {
            // 小研究特性
            componentSpecialityComboBox.Items.Clear();
            int maxWidth = componentSpecialityComboBox.DropDownWidth;
            foreach (string name in Techs.Specialities.Where(speciality => speciality != TechSpeciality.None)
                                         .Select(Techs.GetSpecialityName))
            {
                componentSpecialityComboBox.Items.Add(name);
                maxWidth = Math.Max(maxWidth,
                                    TextRenderer.MeasureText(name, componentSpecialityComboBox.Font).Width +
                                    SystemInformation.VerticalScrollBarWidth);
            }
            componentSpecialityComboBox.DropDownWidth = maxWidth;
        }

        /// <summary>
        ///     小研究タブの項目を更新する
        /// </summary>
        /// <param name="item">技術</param>
        private void UpdateComponentItems(TechItem item)
        {
            componentListView.BeginUpdate();
            componentListView.Items.Clear();

            foreach (TechComponent component in item.Components)
            {
                ListViewItem listItem = CreateComponentListItem(component);

                componentListView.Items.Add(listItem);
            }

            if (componentListView.Items.Count > 0)
            {
                // 先頭の項目を選択する
                componentListView.Items[0].Focused = true;
                componentListView.Items[0].Selected = true;

                // 編集項目を有効化する
                EnableComponentItems();
            }
            else
            {
                // 編集項目を無効化する
                DisableComponentItems();
            }

            componentListView.EndUpdate();
        }

        /// <summary>
        ///     小研究タブを有効化する
        /// </summary>
        private void EnableComponentTab()
        {
            // タブを有効化する
            editTabControl.TabPages[(int) TechEditorTab.Component].Enabled = true;
        }

        /// <summary>
        ///     小研究タブを無効化する
        /// </summary>
        private void DisableComponentTab()
        {
            // タブを無効化する
            editTabControl.TabPages[(int) TechEditorTab.Component].Enabled = false;

            // 小研究リストの項目をクリアする
            componentListView.Items.Clear();

            // 編集項目を無効化する
            DisableComponentItems();
        }

        /// <summary>
        ///     小研究タブの編集項目を有効化する
        /// </summary>
        private void EnableComponentItems()
        {
            // 無効化時にクリアした値を再設定する
            componentIdNumericUpDown.Text = componentIdNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            componentDifficultyNumericUpDown.Text =
                componentDifficultyNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);

            // 編集項目を有効化する
            componentIdNumericUpDown.Enabled = true;
            componentNameTextBox.Enabled = true;
            componentSpecialityComboBox.Enabled = true;
            componentDifficultyNumericUpDown.Enabled = true;
            componentDoubleTimeCheckBox.Enabled = true;

            componentCloneButton.Enabled = true;
            componentRemoveButton.Enabled = true;
        }

        /// <summary>
        ///     小研究タブの編集項目を無効化する
        /// </summary>
        private void DisableComponentItems()
        {
            // 編集項目の値をクリアする
            componentIdNumericUpDown.ResetText();
            componentNameTextBox.ResetText();
            componentSpecialityComboBox.SelectedIndex = -1;
            componentSpecialityComboBox.ResetText();
            componentDifficultyNumericUpDown.ResetText();
            componentDoubleTimeCheckBox.Checked = false;

            // 編集項目を無効化する
            componentIdNumericUpDown.Enabled = false;
            componentNameTextBox.Enabled = false;
            componentSpecialityComboBox.Enabled = false;
            componentDifficultyNumericUpDown.Enabled = false;
            componentDoubleTimeCheckBox.Enabled = false;

            componentCloneButton.Enabled = false;
            componentRemoveButton.Enabled = false;
            componentUpButton.Enabled = false;
            componentDownButton.Enabled = false;
        }

        /// <summary>
        ///     小研究リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComponentListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            // 小研究リストの選択項目がなければ編集項目を無効化する
            if (componentListView.SelectedIndices.Count == 0)
            {
                DisableComponentItems();
                return;
            }
            int index = componentListView.SelectedIndices[0];

            TechComponent component = item.Components[index];
            if (component == null)
            {
                return;
            }

            // 編集項目の値を更新する
            componentIdNumericUpDown.Value = component.Id;
            componentNameTextBox.Text = component.ToString();
            componentSpecialityComboBox.SelectedIndex = (int) component.Speciality - 1;
            componentDifficultyNumericUpDown.Value = component.Difficulty;
            componentDoubleTimeCheckBox.Checked = component.DoubleTime;

            // コンボボックスの色を更新する
            componentSpecialityComboBox.Refresh();

            // 編集項目の色を更新する
            componentIdNumericUpDown.ForeColor = component.IsDirty(TechComponentItemId.Id)
                                                     ? Color.Red
                                                     : SystemColors.WindowText;
            componentNameTextBox.ForeColor = component.IsDirty(TechComponentItemId.Name)
                                                 ? Color.Red
                                                 : SystemColors.WindowText;
            componentDifficultyNumericUpDown.ForeColor = component.IsDirty(TechComponentItemId.Difficulty)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
            componentDoubleTimeCheckBox.ForeColor = component.IsDirty(TechComponentItemId.DoubleTime)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;

            // 編集項目を有効化する
            EnableComponentItems();

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
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 研究特性アイコンを描画する
            if (e.Index < Techs.SpecialityImages.Images.Count && !string.IsNullOrEmpty(componentSpecialityComboBox.Text))
            {
                e.Graphics.DrawImage(Techs.SpecialityImages.Images[e.Index],
                                     new Rectangle(e.Bounds.X + 1, e.Bounds.Y + 1, 16, 16));
            }

            // 項目の文字列を描画する
            var item = GetSelectedItem() as TechItem;
            if (item != null && componentListView.SelectedIndices.Count > 0)
            {
                TechComponent component = item.Components[componentListView.SelectedIndices[0]];
                Brush brush;
                if ((Techs.Specialities[e.Index + 1] == component.Speciality) &&
                    component.IsDirty(TechComponentItemId.Specilaity))
                {
                    brush = new SolidBrush(Color.Red);
                }
                else
                {
                    brush = new SolidBrush(SystemColors.WindowText);
                }
                e.Graphics.DrawString(
                    componentSpecialityComboBox.Items[e.Index].ToString(), e.Font, brush,
                    new Rectangle(e.Bounds.X + 19, e.Bounds.Y + 3, e.Bounds.Width - 19, e.Bounds.Height));
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
            var item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            TechComponent component = TechComponent.Create();

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            component.SetDirtyAll();

            if (componentListView.SelectedIndices.Count > 0)
            {
                int index = componentListView.SelectedIndices[0];
                TechComponent selected = item.Components[index];
                component.Id = selected.Id + 1;

                // 項目をリストに挿入する
                item.InsertComponent(component, index + 1);

                // 小研究リストビューに項目を挿入する
                InsertComponentListItem(component, index + 1);
            }
            else
            {
                // 項目をリストに追加する
                item.AddComponent(component);

                // 小研究リストビューに項目を追加する
                AddComponentListItem(component);
            }
        }

        /// <summary>
        ///     小研究の複製ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComponentCloneButtonClick(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
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

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            component.SetDirtyAll();

            // 項目をリストに挿入する
            item.InsertComponent(component, index + 1);

            // 小研究リストビューに項目を挿入する
            InsertComponentListItem(component, index + 1);
        }

        /// <summary>
        ///     小研究の削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComponentRemoveButtonClick(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
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

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();

            // 項目をリストから削除する
            item.RemoveComponent(index);

            // 小研究リストビューから項目を削除する
            RemoveComponentListItem(index);
        }

        /// <summary>
        ///     小研究の上へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComponentUpButtonClick(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
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

            // 項目を移動する
            item.MoveComponent(index, index - 1);
            MoveComponentListItem(index, index - 1);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
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
            var item = GetSelectedItem() as TechItem;
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

            // 項目を移動する
            item.MoveComponent(index, index + 1);
            MoveComponentListItem(index, index + 1);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
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
            var item = GetSelectedItem() as TechItem;
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

            // 値に変化がなければ何もしない
            var id = (int) componentIdNumericUpDown.Value;
            if (id == component.Id)
            {
                return;
            }

            // 値を更新する
            component.Id = id;

            // 小研究リストビューの項目を更新する
            componentListView.Items[index].Text = id.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            component.SetDirty(TechComponentItemId.Id);

            // 文字色を変更する
            componentIdNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     小研究名変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComponentNameTextBoxTextChanged(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
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

            // 値に変化がなければ何もしない
            string name = componentNameTextBox.Text;
            if (name.Equals(component.ToString()))
            {
                return;
            }

            // 値を更新する
            Config.SetText(component.Name, name, Game.TechTextFileName);

            // 小研究リストビューの項目を更新する
            componentListView.Items[index].SubItems[1].Text = name;

            // 編集済みフラグを設定する
            item.SetDirty();
            component.SetDirty(TechComponentItemId.Name);
            Config.SetDirty(Game.TechTextFileName);

            // 文字色を変更する
            componentNameTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     小研究特性変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComponentSpecialityComboBoxSelectionChangeCommitted(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
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

            // 値に変化がなければ何もしない
            TechSpeciality speciality = Techs.Specialities[componentSpecialityComboBox.SelectedIndex + 1];
            if (speciality == component.Speciality)
            {
                return;
            }

            // 値を更新する
            component.Speciality = speciality;

            // 小研究リストビューの項目を更新する
            componentListView.Items[index].SubItems[2].Text = Techs.GetSpecialityName(speciality);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            component.SetDirty(TechComponentItemId.Specilaity);

            // 小研究特性コンボボックスの項目色を変更するため描画更新する
            componentSpecialityComboBox.Refresh();
        }

        /// <summary>
        ///     小研究難易度変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComponentDifficultyNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
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

            // 値に変化がなければ何もしない
            var difficulty = (int) componentDifficultyNumericUpDown.Value;
            if (difficulty == component.Difficulty)
            {
                return;
            }

            // 値を更新する
            component.Difficulty = difficulty;

            // 小研究リストビューの項目を更新する
            componentListView.Items[index].SubItems[3].Text = difficulty.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            component.SetDirty(TechComponentItemId.Difficulty);

            // 文字色を変更する
            componentDifficultyNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     小研究2倍時間設定変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComponentDoubleTimeCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
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

            // 値に変化がなければ何もしない
            bool doubleTime = componentDoubleTimeCheckBox.Checked;
            if (doubleTime == component.DoubleTime)
            {
                return;
            }

            // 値を更新する
            component.DoubleTime = doubleTime;

            // 小研究リストビューの項目を更新する
            componentListView.Items[index].SubItems[4].Text = doubleTime ? Resources.Yes : Resources.No;

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            component.SetDirty(TechComponentItemId.DoubleTime);

            // 文字色を変更する
            componentDoubleTimeCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     小研究リストの項目を作成する
        /// </summary>
        /// <param name="component">小研究</param>
        /// <returns>小研究リストの項目</returns>
        private static ListViewItem CreateComponentListItem(TechComponent component)
        {
            var li = new ListViewItem {Text = component.Id.ToString(CultureInfo.InvariantCulture)};
            li.SubItems.Add(component.ToString());
            li.SubItems.Add(Techs.GetSpecialityName(component.Speciality));
            li.SubItems.Add(component.Difficulty.ToString(CultureInfo.InvariantCulture));
            li.SubItems.Add(component.DoubleTime ? Resources.Yes : Resources.No);

            return li;
        }

        /// <summary>
        ///     小研究リストの項目を追加する
        /// </summary>
        /// <param name="component">追加対象の小研究</param>
        private void AddComponentListItem(TechComponent component)
        {
            // リストに項目を追加する
            ListViewItem li = CreateComponentListItem(component);
            componentListView.Items.Add(li);

            // 追加した項目を選択する
            int index = componentListView.Items.Count - 1;
            componentListView.Items[index].Focused = true;
            componentListView.Items[index].Selected = true;
            componentListView.EnsureVisible(index);

            // 編集項目を有効化する
            EnableComponentItems();
        }

        /// <summary>
        ///     小研究リストに項目を挿入する
        /// </summary>
        /// <param name="component">挿入対象の小研究</param>
        /// <param name="index">挿入する位置</param>
        private void InsertComponentListItem(TechComponent component, int index)
        {
            // リストに項目を追加する
            ListViewItem li = CreateComponentListItem(component);
            componentListView.Items.Insert(index, li);

            // 挿入した項目を選択する
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
            var li = componentListView.Items[src].Clone() as ListViewItem;
            if (li == null)
            {
                return;
            }

            if (src > dest)
            {
                // 上へ移動する場合
                componentListView.Items.Insert(dest, li);
                componentListView.Items.RemoveAt(src + 1);
            }
            else
            {
                // 下へ移動する場合
                componentListView.Items.Insert(dest + 1, li);
                componentListView.Items.RemoveAt(src);
            }

            // 移動先の項目を選択する
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
                // 削除した項目の次の項目を選択する
                componentListView.Items[index].Focused = true;
                componentListView.Items[index].Selected = true;
            }
            else if (index > 0)
            {
                // リストの末尾ならば削除した項目の前の項目を選択する
                componentListView.Items[componentListView.Items.Count - 1].Focused = true;
                componentListView.Items[componentListView.Items.Count - 1].Selected = true;
            }
            else
            {
                // 項目がなくなれば編集項目を無効化する
                DisableComponentTab();
            }
        }

        #endregion

        #region 技術効果タブ

        /// <summary>
        ///     技術効果タブの編集項目を初期化する
        /// </summary>
        private void InitEffectItems()
        {
            // 技術効果の種類
            commandTypeComboBox.Items.Clear();
            int maxWidth = commandTypeComboBox.DropDownWidth;
            foreach (string name in Command.TypeStringTable)
            {
                commandTypeComboBox.Items.Add(name);
                maxWidth = Math.Max(maxWidth,
                                    TextRenderer.MeasureText(name, commandTypeComboBox.Font).Width +
                                    SystemInformation.VerticalScrollBarWidth);
            }
            commandTypeComboBox.DropDownWidth = maxWidth;
        }

        /// <summary>
        ///     技術効果タブの項目を更新する
        /// </summary>
        /// <param name="item">技術</param>
        private void UpdateEffectItems(TechItem item)
        {
            effectListView.BeginUpdate();
            effectListView.Items.Clear();

            // 項目を順に追加する
            foreach (Command command in item.Effects)
            {
                ListViewItem listItem = CreateEffectListItem(command);

                effectListView.Items.Add(listItem);
            }

            if (effectListView.Items.Count > 0)
            {
                // 先頭の項目を選択する
                effectListView.Items[0].Focused = true;
                effectListView.Items[0].Selected = true;

                // 編集項目を有効化する
                EnableEffectItems();
            }
            else
            {
                // 編集項目を無効化する
                DisableEffectItems();
            }

            effectListView.EndUpdate();
        }

        /// <summary>
        ///     技術効果タブを有効化する
        /// </summary>
        private void EnableEffectTab()
        {
            // タブを有効化する
            editTabControl.TabPages[(int) TechEditorTab.Effect].Enabled = true;
        }

        /// <summary>
        ///     技術効果タブを無効化する
        /// </summary>
        private void DisableEffectTab()
        {
            // タブを無効化する
            editTabControl.TabPages[(int) TechEditorTab.Effect].Enabled = false;

            // 技術効果リストの項目をクリアする
            effectListView.Items.Clear();

            // 編集項目を無効化する
            DisableEffectItems();
        }

        /// <summary>
        ///     技術効果タブの編集項目を有効化する
        /// </summary>
        private void EnableEffectItems()
        {
            // 編集項目を有効化する
            commandTypeComboBox.Enabled = true;
            commandWhichComboBox.Enabled = true;
            commandValueComboBox.Enabled = true;
            commandWhenComboBox.Enabled = true;
            commandWhereComboBox.Enabled = true;

            effectCloneButton.Enabled = true;
            effectRemoveButton.Enabled = true;
        }

        /// <summary>
        ///     技術効果タブの編集項目を無効化する
        /// </summary>
        private void DisableEffectItems()
        {
            // 編集項目の値をクリアする
            commandTypeComboBox.SelectedIndex = -1;
            commandTypeComboBox.ResetText();
            commandWhichComboBox.SelectedIndex = -1;
            commandWhichComboBox.ResetText();
            commandValueComboBox.SelectedIndex = -1;
            commandValueComboBox.ResetText();
            commandWhenComboBox.SelectedIndex = -1;
            commandWhenComboBox.ResetText();
            commandWhereComboBox.SelectedIndex = -1;
            commandWhereComboBox.ResetText();

            // 編集項目を無効化する
            commandTypeComboBox.Enabled = false;
            commandWhichComboBox.Enabled = false;
            commandValueComboBox.Enabled = false;
            commandWhenComboBox.Enabled = false;
            commandWhereComboBox.Enabled = false;

            effectCloneButton.Enabled = false;
            effectRemoveButton.Enabled = false;
            effectUpButton.Enabled = false;
            effectDownButton.Enabled = false;
        }

        /// <summary>
        ///     技術効果種類コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCommandTypeComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            var item = GetSelectedItem() as TechItem;
            if (item != null && effectListView.SelectedIndices.Count > 0)
            {
                Command command = item.Effects[effectListView.SelectedIndices[0]];
                Brush brush;
                if ((e.Index == (int) command.Type) && command.IsDirty(CommandItemId.Type))
                {
                    brush = new SolidBrush(Color.Red);
                }
                else
                {
                    brush = new SolidBrush(SystemColors.WindowText);
                }
                string s = commandTypeComboBox.Items[e.Index].ToString();
                e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     技術効果whichパラメータコンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCommandWhichComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            var item = GetSelectedItem() as TechItem;
            if (item != null && effectListView.SelectedIndices.Count > 0)
            {
                Command command = item.Effects[effectListView.SelectedIndices[0]];
                Brush brush = command.IsDirty(CommandItemId.Which)
                                  ? new SolidBrush(Color.Red)
                                  : new SolidBrush(SystemColors.WindowText);
                string s = commandWhichComboBox.Items[e.Index].ToString();
                e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     技術効果valueパラメータコンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCommandValueComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            var item = GetSelectedItem() as TechItem;
            if (item != null && effectListView.SelectedIndices.Count > 0)
            {
                Command command = item.Effects[effectListView.SelectedIndices[0]];
                Brush brush = command.IsDirty(CommandItemId.Value)
                                  ? new SolidBrush(Color.Red)
                                  : new SolidBrush(SystemColors.WindowText);
                string s = commandValueComboBox.Items[e.Index].ToString();
                e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     技術効果whenパラメータコンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCommandWhenComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            var item = GetSelectedItem() as TechItem;
            if (item != null && effectListView.SelectedIndices.Count > 0)
            {
                Command command = item.Effects[effectListView.SelectedIndices[0]];
                Brush brush = command.IsDirty(CommandItemId.When)
                                  ? new SolidBrush(Color.Red)
                                  : new SolidBrush(SystemColors.WindowText);
                string s = commandWhenComboBox.Items[e.Index].ToString();
                e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     技術効果whereパラメータコンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCommandWhereComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            var item = GetSelectedItem() as TechItem;
            if (item != null && effectListView.SelectedIndices.Count > 0)
            {
                Command command = item.Effects[effectListView.SelectedIndices[0]];
                Brush brush = command.IsDirty(CommandItemId.Where)
                                  ? new SolidBrush(Color.Red)
                                  : new SolidBrush(SystemColors.WindowText);
                string s = commandWhereComboBox.Items[e.Index].ToString();
                e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     技術効果リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEffectListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            // 技術効果リストの選択項目がなければ編集項目を無効化する
            if (effectListView.SelectedIndices.Count == 0)
            {
                DisableEffectItems();
                return;
            }
            int index = effectListView.SelectedIndices[0];

            Command command = item.Effects[index];
            if (command == null)
            {
                return;
            }

            // 編集項目の値を更新する
            if (command.Type != CommandType.None)
            {
                commandTypeComboBox.SelectedIndex = (int) command.Type;
            }
            else
            {
                commandTypeComboBox.SelectedIndex = -1;
                commandTypeComboBox.Text = "";
            }
            commandWhichComboBox.Text = command.Which != null ? command.Which.ToString() : "";
            commandValueComboBox.Text = command.Value != null ? command.Value.ToString() : "";
            commandWhenComboBox.Text = command.When != null ? command.When.ToString() : "";
            commandWhereComboBox.Text = command.Where != null ? command.Where.ToString() : "";

            // コンボボックスの色を更新する
            commandTypeComboBox.Refresh();
            commandWhichComboBox.Refresh();
            commandValueComboBox.Refresh();
            commandWhenComboBox.Refresh();
            commandWhereComboBox.Refresh();

            // 編集項目を有効化する
            EnableEffectItems();

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
            var item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            var command = new Command();

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            command.SetDirtyAll();

            if (effectListView.SelectedIndices.Count > 0)
            {
                // リストに項目を挿入する
                int index = effectListView.SelectedIndices[0];
                item.InsertCommand(command, index + 1);

                // 技術効果リストビューに項目を挿入する
                InsertEffectListItem(command, index + 1);
            }
            else
            {
                // リストに項目を追加する
                item.AddCommand(command);

                // 技術効果リストビューに項目を追加する
                AddEffectListItem(command);
            }
        }

        /// <summary>
        ///     技術効果の複製ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEffectCloneButtonClick(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
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

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            command.SetDirtyAll();

            // リストに項目を挿入する
            item.InsertCommand(command, index + 1);

            // 技術効果リストビューに項目を挿入する
            InsertEffectListItem(command, index + 1);
        }

        /// <summary>
        ///     技術効果の削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEffectRemoveButtonClick(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
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

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();

            // リストから項目を削除する
            item.RemoveCommand(index);

            // 技術効果リストビューから項目を削除する
            RemoveEffectListItem(index);
        }

        /// <summary>
        ///     技術効果の上へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEffectUpButtonClick(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
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

            // 項目を移動する
            item.MoveCommand(index, index - 1);
            MoveEffectListItem(index, index - 1);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
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
            var item = GetSelectedItem() as TechItem;
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

            // 項目を移動する
            item.MoveCommand(index, index + 1);
            MoveEffectListItem(index, index + 1);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
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
            var item = GetSelectedItem() as TechItem;
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

            // 値に変化がなければ何もしない
            var type = (CommandType) commandTypeComboBox.SelectedIndex;
            if (type == command.Type)
            {
                return;
            }

            // 値を更新する
            command.Type = type;

            // 技術効果リストビューの表示を更新する
            effectListView.Items[index].Text = Command.TypeStringTable[(int) type];

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            command.SetDirty(CommandItemId.Type);

            // 技術効果種類コンボボックスの項目色を変更するため描画更新する
            commandTypeComboBox.Refresh();
        }

        /// <summary>
        ///     技術効果whichパラメータ変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCommandWhichComboBoxTextUpdate(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
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

            // 値に変化がなければ何もしない
            string text = commandWhichComboBox.Text;
            if (command.Which != null && text.Equals(command.Which.ToString()))
            {
                return;
            }

            // 値を更新する
            command.Which = text;

            // 技術効果リストビューの表示を更新する
            effectListView.Items[index].SubItems[1].Text = text;

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            command.SetDirty(CommandItemId.Which);

            // 技術効果whichパラメータコンボボックスの項目色を変更するため描画更新する
            commandWhichComboBox.Refresh();
        }

        /// <summary>
        ///     技術効果valueパラメータ変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCommandValueComboBoxTextUpdate(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
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

            // 値に変化がなければ何もしない
            string text = commandValueComboBox.Text;
            if (command.Value != null && text.Equals(command.Value.ToString()))
            {
                return;
            }

            // 値を更新する
            command.Value = text;

            // 技術効果リストビューの表示を更新する
            effectListView.Items[index].SubItems[2].Text = text;

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            command.SetDirty(CommandItemId.Value);

            // 技術効果valueパラメータコンボボックスの項目色を変更するため描画更新する
            commandValueComboBox.Refresh();
        }

        /// <summary>
        ///     技術効果whenパラメータ変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCommandWhenComboBoxTextUpdate(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
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

            // 値に変化がなければ何もしない
            string text = commandWhenComboBox.Text;
            if (command.When != null && text.Equals(command.When.ToString()))
            {
                return;
            }

            // 値を更新する
            command.When = text;

            // 技術効果リストビューの表示を更新する
            effectListView.Items[index].SubItems[3].Text = text;

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            command.SetDirty(CommandItemId.When);

            // 技術効果whenパラメータコンボボックスの項目色を変更するため描画更新する
            commandWhenComboBox.Refresh();
        }

        /// <summary>
        ///     技術効果whereパラメータ変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCommandWhereComboBoxTextUpdate(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            var item = GetSelectedItem() as TechItem;
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

            // 値に変化がなければ何もしない
            string newText = commandWhereComboBox.Text;
            if (command.Where != null && newText.Equals(command.Where.ToString()))
            {
                return;
            }

            // 値を更新する
            command.Where = newText;

            // 技術効果リストビューの表示を更新する
            effectListView.Items[index].SubItems[4].Text = newText;

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            command.SetDirty(CommandItemId.Where);

            // 技術効果whereパラメータコンボボックスの項目色を変更するため描画更新する
            commandWhereComboBox.Refresh();
        }

        /// <summary>
        ///     技術効果リストの項目を作成する
        /// </summary>
        /// <param name="command">技術効果</param>
        /// <returns>技術効果リストの項目</returns>
        private static ListViewItem CreateEffectListItem(Command command)
        {
            var li = new ListViewItem {Text = Command.TypeStringTable[(int) command.Type]};
            li.SubItems.Add(command.Which != null ? command.Which.ToString() : "");
            li.SubItems.Add(command.Value != null ? command.Value.ToString() : "");
            li.SubItems.Add(command.When != null ? command.When.ToString() : "");
            li.SubItems.Add(command.Where != null ? command.Where.ToString() : "");

            return li;
        }

        /// <summary>
        ///     技術効果リストの項目を追加する
        /// </summary>
        /// <param name="command">追加対象の技術効果</param>
        private void AddEffectListItem(Command command)
        {
            // リストに項目を追加する
            ListViewItem li = CreateEffectListItem(command);
            effectListView.Items.Add(li);

            // 追加した項目を選択する
            int index = effectListView.Items.Count - 1;
            effectListView.Items[index].Focused = true;
            effectListView.Items[index].Selected = true;
            effectListView.EnsureVisible(index);

            // 編集項目を有効化する
            EnableEffectItems();
        }

        /// <summary>
        ///     技術効果リストに項目を挿入する
        /// </summary>
        /// <param name="command">挿入対象の技術効果</param>
        /// <param name="index">挿入する位置</param>
        private void InsertEffectListItem(Command command, int index)
        {
            // リストに項目を挿入する
            ListViewItem li = CreateEffectListItem(command);
            effectListView.Items.Insert(index, li);

            // 挿入した項目を選択する
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
            var li = effectListView.Items[src].Clone() as ListViewItem;
            if (li == null)
            {
                return;
            }

            if (src > dest)
            {
                // 上へ移動する場合
                effectListView.Items.Insert(dest, li);
                effectListView.Items.RemoveAt(src + 1);
            }
            else
            {
                // 下へ移動する場合
                effectListView.Items.Insert(dest + 1, li);
                effectListView.Items.RemoveAt(src);
            }

            // 移動先の項目を選択する
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
            // リストから項目を削除する
            effectListView.Items.RemoveAt(index);

            if (index < effectListView.Items.Count)
            {
                // 削除した項目の次の項目を選択する
                effectListView.Items[index].Focused = true;
                effectListView.Items[index].Selected = true;
            }
            else if (index > 0)
            {
                // リストの末尾ならば削除した項目の前の項目を選択する
                effectListView.Items[effectListView.Items.Count - 1].Focused = true;
                effectListView.Items[effectListView.Items.Count - 1].Selected = true;
            }
            else
            {
                // 項目がなくなれば編集項目を無効化する
                DisableEffectItems();
            }
        }

        #endregion

        #region ラベルタブ

        /// <summary>
        ///     ラベルタブの項目を更新する
        /// </summary>
        /// <param name="item">技術ラベル</param>
        private void UpdateLabelItems(TechLabel item)
        {
            // 編集項目の値を更新する
            labelNameTextBox.Text = item.ToString();
            UpdateLabelPositionList(item);

            // 編集項目の色を更新する
            labelNameTextBox.ForeColor = item.IsDirty(TechItemId.Name) ? Color.Red : SystemColors.WindowText;
        }

        /// <summary>
        ///     ラベルタブを有効化する
        /// </summary>
        private void EnableLabelTab()
        {
            // タブを有効化する
            editTabControl.TabPages[5].Enabled = true;

            // 無効化時にクリアした値を再設定する
            labelXNumericUpDown.Text = labelXNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            labelYNumericUpDown.Text = labelYNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     ラベルタブを無効化する
        /// </summary>
        private void DisableLabelTab()
        {
            // タブを無効化する
            editTabControl.TabPages[5].Enabled = false;

            // 編集項目の値をクリアする
            labelNameTextBox.ResetText();
            labelPositionListView.Items.Clear();
            labelXNumericUpDown.ResetText();
            labelYNumericUpDown.ResetText();
        }

        /// <summary>
        ///     技術ラベル座標の編集項目を有効化する
        /// </summary>
        private void EnableLabelPositionItems()
        {
            // 無効化時にクリアした値を再設定する
            labelXNumericUpDown.Text = labelXNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            labelYNumericUpDown.Text = labelYNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);

            // 編集項目を有効化する
            labelXNumericUpDown.Enabled = true;
            labelYNumericUpDown.Enabled = true;

            labelPositionRemoveButton.Enabled = true;
        }

        /// <summary>
        ///     技術ラベル座標の編集項目を無効化する
        /// </summary>
        private void DisableLabelPositionItems()
        {
            // 編集項目の値をクリアする
            labelXNumericUpDown.ResetText();
            labelYNumericUpDown.ResetText();

            // 編集項目を無効化する
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
                // 先頭の項目を選択する
                labelPositionListView.Items[0].Focused = true;
                labelPositionListView.Items[0].Selected = true;

                // 編集項目を有効化する
                EnableLabelPositionItems();
            }
            else
            {
                // 編集項目を無効化する
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
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechLabel;
            if (item == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            string text = labelNameTextBox.Text;
            if (text.Equals(item.ToString()))
            {
                return;
            }

            // 値を更新する
            Config.SetText(item.Name, text, Game.TechTextFileName);

            // 項目リストボックスの項目を再設定することで表示更新している
            // この時再選択によりフォーカスが外れるので、イベントハンドラを一時的に無効化する
            techListBox.SelectedIndexChanged -= OnTechListBoxSelectedIndexChanged;
            techListBox.Items[techListBox.SelectedIndex] = item;
            techListBox.SelectedIndexChanged += OnTechListBoxSelectedIndexChanged;

            // 技術ツリー上のラベル名を更新する
            foreach (Label label in treePictureBox.Controls)
            {
                var info = label.Tag as TechLabelInfo;
                if (info != null && info.Item == item)
                {
                    label.Size = TextRenderer.MeasureText(item.ToString(), label.Font);
                    label.Refresh();
                }
            }

            // 編集済みフラグを設定する
            item.SetDirty(TechItemId.Name);
            Config.SetDirty(Game.TechTextFileName);

            // 文字色を変更する
            labelNameTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     ラベル座標リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLabelPositionListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            var item = GetSelectedItem() as TechLabel;
            if (item == null)
            {
                return;
            }

            // ラベル座標リストの選択項目がなければ編集項目を無効化する
            if (labelPositionListView.SelectedIndices.Count == 0)
            {
                DisableLabelPositionItems();
                return;
            }

            // 編集項目の値を更新する
            TechPosition position = item.Positions[labelPositionListView.SelectedIndices[0]];
            labelXNumericUpDown.Value = position.X;
            labelYNumericUpDown.Value = position.Y;

            // 編集項目の色を更新する
            labelXNumericUpDown.ForeColor = position.IsDirty(TechPositionItemId.X) ? Color.Red : SystemColors.WindowText;
            labelYNumericUpDown.ForeColor = position.IsDirty(TechPositionItemId.Y) ? Color.Red : SystemColors.WindowText;

            // 編集項目を有効化する
            EnableLabelPositionItems();
        }

        /// <summary>
        ///     ラベルX座標変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLabelXNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechLabel;
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

            // 値に変化がなければ何もしない
            var x = (int) labelXNumericUpDown.Value;
            if (x == position.X)
            {
                return;
            }

            // 値を更新する
            position.X = x;

            // ラベル座標リストビューの項目を更新する
            labelPositionListView.Items[index].Text = x.ToString(CultureInfo.InvariantCulture);

            // 技術ツリー上のラベルを移動する
            foreach (Label label in treePictureBox.Controls)
            {
                var info = label.Tag as TechLabelInfo;
                if (info != null && info.Position == position)
                {
                    label.Location = new Point(position.X, position.Y);
                }
            }

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            position.SetDirty(TechPositionItemId.X);

            // 文字色を変更する
            labelXNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     ラベルY座標変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLabelYNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechLabel;
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

            // 値に変化がなければ何もしない
            var y = (int) labelYNumericUpDown.Value;
            if (y == position.Y)
            {
                return;
            }

            // 値を更新する
            position.Y = y;

            // ラベル座標リストビューの項目を更新する
            labelPositionListView.Items[index].SubItems[1].Text = y.ToString(CultureInfo.InvariantCulture);

            // 技術ツリー上のラベルを移動する
            foreach (Label label in treePictureBox.Controls)
            {
                var info = label.Tag as TechLabelInfo;
                if (info != null && info.Position == position)
                {
                    label.Location = new Point(position.X, position.Y);
                }
            }

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            position.SetDirty(TechPositionItemId.Y);

            // 文字色を変更する
            labelYNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     ラベル座標追加ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLabelPositionAddButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechLabel;
            if (item == null)
            {
                return;
            }

            // ラベル座標リストに項目を追加する
            var position = new TechPosition {X = 0, Y = 0};
            item.Positions.Add(position);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            position.SetDirtyAll();

            // ラベル座標リストビューの項目を追加する
            var li = new ListViewItem {Text = position.X.ToString(CultureInfo.InvariantCulture)};
            li.SubItems.Add(position.Y.ToString(CultureInfo.InvariantCulture));
            labelPositionListView.Items.Add(li);

            // 追加した項目を選択する
            labelPositionListView.Items[labelPositionListView.Items.Count - 1].Focused = true;
            labelPositionListView.Items[labelPositionListView.Items.Count - 1].Selected = true;

            // ラベル座標の編集項目を有効化する
            EnableLabelPositionItems();

            // 技術ツリーにラベルを追加する
            AddTechTreeItem(item, position);
        }

        /// <summary>
        ///     ラベル座標削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLabelPositionRemoveButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechLabel;
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

            // ラベル座標リストから項目を削除する
            item.Positions.RemoveAt(index);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();

            // ラベル座標リストビューから項目を削除する
            labelPositionListView.Items.RemoveAt(index);

            if (index < labelPositionListView.Items.Count)
            {
                // 削除した項目の次の項目を選択する
                labelPositionListView.Items[index].Focused = true;
                labelPositionListView.Items[index].Selected = true;
            }
            else if (index > 0)
            {
                // リストの末尾ならば削除した項目の前の項目を選択する
                labelPositionListView.Items[labelPositionListView.Items.Count - 1].Focused = true;
                labelPositionListView.Items[labelPositionListView.Items.Count - 1].Selected = true;
            }
            else
            {
                // 項目がなくなれば編集項目を無効化する
                DisableLabelPositionItems();
            }

            // 技術ツリーからラベルを削除する
            RemoveTechTreeItem(item, position);
        }

        #endregion

        #region 発明イベントタブ

        /// <summary>
        ///     発明イベントタブの項目を更新する
        /// </summary>
        /// <param name="item">技術イベント</param>
        private void UpdateEventItems(TechEvent item)
        {
            // 編集項目の値を更新する
            eventIdNumericUpDown.Value = item.Id;
            eventTechNumericUpDown.Value = item.TechId;
            if (Techs.TechIds.Contains(item.TechId))
            {
                eventTechComboBox.SelectedIndex = Techs.TechIds.IndexOf(item.TechId);
            }
            else
            {
                eventTechComboBox.SelectedIndex = -1;
                eventTechComboBox.ResetText();
            }
            UpdateEventPositionList(item);

            // コンボボックスの色を更新する
            eventTechComboBox.Refresh();

            // 編集項目の色を更新する
            eventIdNumericUpDown.ForeColor = item.IsDirty(TechItemId.Id) ? Color.Red : SystemColors.WindowText;
            eventTechNumericUpDown.ForeColor = item.IsDirty(TechItemId.TechId) ? Color.Red : SystemColors.WindowText;
        }

        /// <summary>
        ///     発明イベントタブの技術リストを更新する
        /// </summary>
        private void UpdateEventTechListItems()
        {
            eventTechComboBox.BeginUpdate();
            eventTechComboBox.Items.Clear();

            int maxWidth = eventTechComboBox.DropDownWidth;
            foreach (TechItem item in Techs.TechIdMap.Select(pair => pair.Value))
            {
                eventTechComboBox.Items.Add(item);
                maxWidth = Math.Max(maxWidth,
                                    TextRenderer.MeasureText(item.ToString(), eventTechComboBox.Font).Width +
                                    SystemInformation.VerticalScrollBarWidth);
            }
            eventTechComboBox.DropDownWidth = maxWidth;

            eventTechComboBox.EndUpdate();
        }

        /// <summary>
        ///     発明イベントタブを有効化する
        /// </summary>
        private void EnableEventTab()
        {
            // タブを有効化する
            editTabControl.TabPages[6].Enabled = true;

            // 無効化時にクリアした値を再設定する
            eventIdNumericUpDown.Text = eventIdNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            eventTechNumericUpDown.Text = eventTechNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            eventXNumericUpDown.Text = eventXNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            eventYNumericUpDown.Text = eventYNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     発明イベントタブを無効化する
        /// </summary>
        private void DisableEventTab()
        {
            // タブを無効化する
            editTabControl.TabPages[6].Enabled = false;

            // 編集項目の値をクリアする
            eventIdNumericUpDown.ResetText();
            eventTechNumericUpDown.ResetText();
            eventTechComboBox.SelectedIndex = -1;
            eventTechComboBox.ResetText();
            eventPositionListView.Items.Clear();
            eventXNumericUpDown.ResetText();
            eventYNumericUpDown.ResetText();
        }

        /// <summary>
        ///     発明イベント座標の編集項目を有効化する
        /// </summary>
        private void EnableEventPositionItems()
        {
            // 無効化時にクリアした文字列を再設定する
            eventXNumericUpDown.Text = eventXNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            eventYNumericUpDown.Text = eventYNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);

            // 編集項目を有効化する
            eventXNumericUpDown.Enabled = true;
            eventYNumericUpDown.Enabled = true;

            eventPositionRemoveButton.Enabled = true;
        }

        /// <summary>
        ///     発明イベント座標の編集項目を無効化する
        /// </summary>
        private void DisableEventPositionItems()
        {
            // 編集項目をクリアする
            eventXNumericUpDown.ResetText();
            eventYNumericUpDown.ResetText();

            // 編集項目を無効化する
            eventXNumericUpDown.Enabled = false;
            eventYNumericUpDown.Enabled = false;

            eventPositionRemoveButton.Enabled = false;
        }

        /// <summary>
        ///     発明イベント座標リストを更新する
        /// </summary>
        /// <param name="item">発明イベント</param>
        private void UpdateEventPositionList(TechEvent item)
        {
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
                // 先頭の項目を選択する
                eventPositionListView.Items[0].Focused = true;
                eventPositionListView.Items[0].Selected = true;

                // 編集項目を有効化する
                EnableEventPositionItems();
            }
            else
            {
                // 編集項目を無効化する
                DisableEventPositionItems();
            }

            eventPositionListView.EndUpdate();
        }

        /// <summary>
        ///     発明イベント技術コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEventTechComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            var item = GetSelectedItem() as TechEvent;
            if (item != null)
            {
                Brush brush;
                if ((Techs.TechIds[e.Index] == item.TechId) && item.IsDirty(TechItemId.TechId))
                {
                    brush = new SolidBrush(Color.Red);
                }
                else
                {
                    brush = new SolidBrush(SystemColors.WindowText);
                }
                string s = eventTechComboBox.Items[e.Index].ToString();
                e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     発明イベントID変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEventIdNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechEvent;
            if (item == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var id = (int) eventIdNumericUpDown.Value;
            if (id == item.Id)
            {
                return;
            }

            // 値を更新する
            item.Id = id;

            // 項目リストボックスの項目を再設定することで表示更新している
            // この時再選択によりフォーカスが外れるので、イベントハンドラを一時的に無効化する
            techListBox.SelectedIndexChanged -= OnTechListBoxSelectedIndexChanged;
            techListBox.Items[techListBox.SelectedIndex] = item;
            techListBox.SelectedIndexChanged += OnTechListBoxSelectedIndexChanged;

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty(TechItemId.Id);

            // 文字色を変更する
            eventIdNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     発明イベント技術ID変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEventTechNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechEvent;
            if (item == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var id = (int) eventTechNumericUpDown.Value;
            if (id == item.TechId)
            {
                return;
            }

            // 値を更新する
            item.TechId = id;

            // 技術コンボボックスの選択項目を更新する
            if (Techs.TechIds.Contains(id))
            {
                eventTechComboBox.SelectedIndex = Techs.TechIds.IndexOf(id);
            }
            else
            {
                eventTechComboBox.SelectedIndex = -1;
                eventTechComboBox.ResetText();
            }

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty(TechItemId.TechId);

            // 文字色を変更する
            eventTechNumericUpDown.ForeColor = Color.Red;

            // 発明イベント技術コンボボックスの項目色を変更するため描画更新する
            eventTechComboBox.Refresh();
        }

        /// <summary>
        ///     発明イベント技術変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEventTechComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechEvent;
            if (item == null)
            {
                return;
            }

            if (eventTechComboBox.SelectedIndex == -1)
            {
                return;
            }

            // 発明イベント技術IDの値を更新する
            int id = Techs.TechIds[eventTechComboBox.SelectedIndex];
            eventTechNumericUpDown.Value = id;
        }

        /// <summary>
        ///     発明イベント座標リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEventPositionListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 技術リストの選択項目がなければ何もしない
            var item = GetSelectedItem() as TechEvent;
            if (item == null)
            {
                return;
            }

            // 発明イベント座標リストの選択項目がなければ編集項目を無効化する
            if (eventPositionListView.SelectedIndices.Count == 0)
            {
                DisableEventPositionItems();
                return;
            }

            // 編集項目の値を更新する
            TechPosition position = item.Positions[eventPositionListView.SelectedIndices[0]];
            eventXNumericUpDown.Value = position.X;
            eventYNumericUpDown.Value = position.Y;

            // 編集項目の色を更新する
            eventXNumericUpDown.ForeColor = position.IsDirty(TechPositionItemId.X) ? Color.Red : SystemColors.WindowText;
            eventYNumericUpDown.ForeColor = position.IsDirty(TechPositionItemId.Y) ? Color.Red : SystemColors.WindowText;

            // 編集項目を有効化する
            EnableEventPositionItems();
        }

        /// <summary>
        ///     発明イベントX座標変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEventXNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechEvent;
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

            // 値に変化がなければ何もしない
            var x = (int) eventXNumericUpDown.Value;
            if (x == position.X)
            {
                return;
            }

            // 値を更新する
            position.X = x;

            // 発明イベント座標リストビューの項目を更新する
            eventPositionListView.Items[index].Text = x.ToString(CultureInfo.InvariantCulture);

            // 技術ツリー上のラベルを移動する
            foreach (Label label in treePictureBox.Controls)
            {
                var info = label.Tag as TechLabelInfo;
                if (info != null && info.Position == position)
                {
                    label.Location = new Point(position.X, position.Y);
                }
            }

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            position.SetDirty(TechPositionItemId.X);

            // 文字色を変更する
            eventXNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     発明イベントY座標変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEventYNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechEvent;
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

            // 値に変化がなければ何もしない
            var y = (int) eventYNumericUpDown.Value;
            if (y == position.Y)
            {
                return;
            }

            // 値を更新する
            position.Y = y;

            // 発明イベント座標リストビューの項目を更新する
            eventPositionListView.Items[index].SubItems[1].Text = y.ToString(CultureInfo.InvariantCulture);

            // 技術ツリー上のラベルを移動する
            foreach (Label label in treePictureBox.Controls)
            {
                var info = label.Tag as TechLabelInfo;
                if (info != null && info.Position == position)
                {
                    label.Location = new Point(position.X, position.Y);
                }
            }

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            position.SetDirty(TechPositionItemId.Y);

            // 文字色を変更する
            eventYNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     技術イベント座標追加ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEventPositionAddButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechEvent;
            if (item == null)
            {
                return;
            }

            // 発明イベント座標リストに項目を追加する
            var position = new TechPosition {X = 0, Y = 0};
            item.Positions.Add(position);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            position.SetDirtyAll();

            // 発明イベント座標リストビューに項目を追加する
            var li = new ListViewItem {Text = position.X.ToString(CultureInfo.InvariantCulture)};
            li.SubItems.Add(position.Y.ToString(CultureInfo.InvariantCulture));
            eventPositionListView.Items.Add(li);

            // 追加した項目を選択する
            eventPositionListView.Items[eventPositionListView.Items.Count - 1].Focused = true;
            eventPositionListView.Items[eventPositionListView.Items.Count - 1].Selected = true;

            // 編集項目を有効化する
            EnableEventPositionItems();

            // 技術ツリーにラベルを追加する
            AddTechTreeItem(item, position);
        }

        /// <summary>
        ///     発明イベント座標削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEventPositionRemoveButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            var item = GetSelectedItem() as TechEvent;
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

            // 発明イベント座標リストから項目を削除する
            item.Positions.RemoveAt(index);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();

            // 発明イベント座標リストビューから項目を削除する
            eventPositionListView.Items.RemoveAt(index);

            if (index < techPositionListView.Items.Count)
            {
                // 削除した項目の次の項目を選択する
                eventPositionListView.Items[index].Focused = true;
                eventPositionListView.Items[index].Selected = true;
            }
            else if (index > 0)
            {
                // リストの末尾ならば削除した項目の前の項目を選択する
                eventPositionListView.Items[eventPositionListView.Items.Count - 1].Focused = true;
                eventPositionListView.Items[eventPositionListView.Items.Count - 1].Selected = true;
            }
            else
            {
                // 項目がなくなれば編集項目を無効化する
                DisableEventPositionItems();
            }

            // 技術ツリーのラベルを削除する
            RemoveTechTreeItem(item, position);
        }

        #endregion
    }

    /// <summary>
    ///     技術ツリーエディタのタブ
    /// </summary>
    public enum TechEditorTab
    {
        Category, // カテゴリ
        Tech, // 技術
        Required, // 必要技術
        Component, // 小研究
        Effect, // 技術効果
        Label, // 技術ラベル
        Event, // 技術イベント
    }
}