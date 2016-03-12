using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HoI2Editor.Controllers;
using HoI2Editor.Controls;
using HoI2Editor.Models;
using HoI2Editor.Properties;
using HoI2Editor.Utilities;

namespace HoI2Editor.Forms
{
    /// <summary>
    ///     技術ツリーエディタフォーム
    /// </summary>
    internal partial class TechEditorForm : Form
    {
        #region 内部フィールド

        /// <summary>
        ///     技術ツリーエディタコントローラ
        /// </summary>
        private readonly TechEditorController _controller;

        /// <summary>
        ///     技術ツリーパネルのコントローラ
        /// </summary>
        private TechTreePanelController _techTreePanelController;

        #endregion

        #region 公開定数

        /// <summary>
        ///     必要技術リストビューの列の数
        /// </summary>
        internal const int RequiredListColumnCount = 2;

        /// <summary>
        ///     小研究リストビューの列の数
        /// </summary>
        internal const int ComponentListColumnCount = 5;

        /// <summary>
        ///     技術効果リストビューの列の数
        /// </summary>
        internal const int EffectListColumnCount = 5;

        /// <summary>
        ///     座標リストビューの列の数
        /// </summary>
        internal const int PositionListColumnCount = 2;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="controller">技術ツリーエディタコントローラ</param>
        internal TechEditorForm(TechEditorController controller)
        {
            InitializeComponent();

            _controller = controller;

            // フォームの初期化
            InitForm();
        }

        #endregion

        #region データ処理

        /// <summary>
        ///     データ読み込み後の処理
        /// </summary>
        internal void OnFileLoaded()
        {
            // 技術タブの編集項目を初期化する
            InitTechItems();

            // 小研究タブの編集項目を初期化する
            InitComponentItems();

            // 技術効果タブの編集項目を初期化する
            InitEffectItems();

            // 必要技術タブの技術リストを更新する
            UpdateRequiredTechListItems();

            // 技術イベントタブの技術リストを更新する
            UpdateEventTechListItems();

            // カテゴリリストボックスを初期化する
            InitCategoryList();
        }

        /// <summary>
        ///     データ保存後の処理
        /// </summary>
        internal void OnFileSaved()
        {
            // 編集済みフラグがクリアされるため表示を更新する
            categoryListBox.Refresh();
            techListBox.Refresh();
            UpdateCategoryItems();
            UpdateEditableItems();
        }

        /// <summary>
        ///     編集項目更新時の処理
        /// </summary>
        /// <param name="id">編集項目ID</param>
        internal void OnItemChanged(EditorItemId id)
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
            // 技術カテゴリリストボックス
            categoryListBox.ItemHeight = DeviceCaps.GetScaledHeight(categoryListBox.ItemHeight);

            // 技術項目リストボックス
            techListBox.ItemHeight = DeviceCaps.GetScaledHeight(techListBox.ItemHeight);

            // 技術座標リストビュー
            techXColumnHeader.Width = HoI2EditorController.Settings.TechEditor.TechPositionListColumnWidth[0];
            techYColumnHeader.Width = HoI2EditorController.Settings.TechEditor.TechPositionListColumnWidth[1];

            // AND条件必要技術リストビュー
            andIdColumnHeader.Width = HoI2EditorController.Settings.TechEditor.AndRequiredListColumnWidth[0];
            andNameColumnHeader.Width = HoI2EditorController.Settings.TechEditor.AndRequiredListColumnWidth[1];

            // OR条件必要技術リストビュー
            orIdColumnHeader.Width = HoI2EditorController.Settings.TechEditor.OrRequiredListColumnWidth[0];
            orNameColumnHeader.Width = HoI2EditorController.Settings.TechEditor.OrRequiredListColumnWidth[1];

            // 小研究リストビュー
            componentIdColumnHeader.Width = HoI2EditorController.Settings.TechEditor.ComponentListColumnWidth[0];
            componentNameColumnHeader.Width = HoI2EditorController.Settings.TechEditor.ComponentListColumnWidth[1];
            componentSpecialityColumnHeader.Width = HoI2EditorController.Settings.TechEditor.ComponentListColumnWidth[2];
            componentDifficultyColumnHeader.Width = HoI2EditorController.Settings.TechEditor.ComponentListColumnWidth[3];
            componentDoubleTimeColumnHeader.Width = HoI2EditorController.Settings.TechEditor.ComponentListColumnWidth[4];

            // 小研究特性コンボボックス
            componentSpecialityComboBox.ItemHeight = DeviceCaps.GetScaledHeight(componentSpecialityComboBox.ItemHeight);

            // 技術効果リストビュー
            commandTypeColumnHeader.Width = HoI2EditorController.Settings.TechEditor.EffectListColumnWidth[0];
            commandWhichColumnHeader.Width = HoI2EditorController.Settings.TechEditor.EffectListColumnWidth[1];
            commandValueColumnHeader.Width = HoI2EditorController.Settings.TechEditor.EffectListColumnWidth[2];
            commandWhenColumnHeader.Width = HoI2EditorController.Settings.TechEditor.EffectListColumnWidth[3];
            commandWhereColumnHeader.Width = HoI2EditorController.Settings.TechEditor.EffectListColumnWidth[4];

            // ラベル座標リストビュー
            labelXColumnHeader.Width = HoI2EditorController.Settings.TechEditor.LabelPositionListColumnWidth[0];
            labelYColumnHeader.Width = HoI2EditorController.Settings.TechEditor.LabelPositionListColumnWidth[1];

            // 発明イベント座標リストビュー
            eventXColumnHeader.Width = HoI2EditorController.Settings.TechEditor.EventPositionListColumnWidth[0];
            eventYColumnHeader.Width = HoI2EditorController.Settings.TechEditor.EventPositionListColumnWidth[1];

            // 技術ツリーパネル
            _techTreePanelController = new TechTreePanelController(treePictureBox) { AllowDragDrop = true };
            _techTreePanelController.ItemMouseDown += OnTechTreeLabelMouseDown;
            _techTreePanelController.ItemDragDrop += OnTreePictureBoxDragDrop;

            // ウィンドウの位置
            Location = HoI2EditorController.Settings.TechEditor.Location;
            Size = HoI2EditorController.Settings.TechEditor.Size;
        }

        /// <summary>
        ///     技術ツリー画像サイズに合わせてフォームのサイズを変更する
        /// </summary>
        private void UpdateFormSize()
        {
            // デスクトップのサイズを取得する
            Rectangle screenRect = Screen.GetWorkingArea(new Point(200, 200));

            int height = Height + (treePictureBox.Image.Height - treePanel.Height);

            Height = Math.Min(height, screenRect.Height);
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormLoad(object sender, EventArgs e)
        {
            // 研究特性を初期化する
            Techs.InitSpecialities();

            // ゲーム設定ファイルを読み込む
            Misc.Load();

            // 文字列定義ファイルを読み込む
            Config.Load();

            // 技術定義ファイルを読み込む
            Techs.Load();

            // データ読み込み後の処理
            OnFileLoaded();

            // 技術ツリー画像サイズに合わせてフォームのサイズを変更する
            UpdateFormSize();
        }

        /// <summary>
        ///     フォームクローズ時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = _controller.OnFormClosing();
        }

        /// <summary>
        ///     フォームクローズ後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _controller.OnFormClosed();
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
                HoI2EditorController.Settings.TechEditor.Location = Location;
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
                HoI2EditorController.Settings.TechEditor.Size = Size;
            }
        }

        /// <summary>
        ///     再読み込みボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReloadButtonClick(object sender, EventArgs e)
        {
            _controller.QueryReload();
        }

        /// <summary>
        ///     保存ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            _controller.Save();
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

            // 選択中のカテゴリを反映する
            int index = HoI2EditorController.Settings.TechEditor.Category;
            if ((index < 0) || (index >= categoryListBox.Items.Count))
            {
                index = 0;
            }
            categoryListBox.SelectedIndex = index;
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

            // 技術ツリーパネルを更新する
            _techTreePanelController.Category = (TechCategory) categoryListBox.SelectedIndex;
            _techTreePanelController.Update();

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

            // 選択中のカテゴリを保存する
            HoI2EditorController.Settings.TechEditor.Category = categoryListBox.SelectedIndex;
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
                _techTreePanelController.AddItem(item);
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
        ///     技術項目リストビューの項目入れ替え時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechListBoxItemReordered(object sender, ItemReorderedEventArgs e)
        {
            // 自前で項目を入れ替えるのでキャンセル扱いにする
            e.Cancel = true;

            int srcIndex = e.OldDisplayIndices[0];
            int destIndex = e.NewDisplayIndex;

            ITechItem src = techListBox.Items[srcIndex] as ITechItem;
            if (src == null)
            {
                return;
            }
            ITechItem dest = techListBox.Items[destIndex] as ITechItem;
            if (dest == null)
            {
                return;
            }


            // 技術項目リストの項目を移動する
            TechGroup grp = GetSelectedGroup();
            grp.MoveItem(src, dest);

            // 項目リストビューの項目を移動する
            MoveTechListItem(srcIndex, destIndex);

            // 編集済みフラグを設定する
            grp.SetDirty();
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
                TechItem applicationItem = item as TechItem;
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
                ITechItem item = techListBox.Items[e.Index] as ITechItem;
                brush = item != null && item.IsDirty()
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
            TechItem item = new TechItem
            {
                Name = Config.GetTempKey(),
                ShortName = Config.GetTempKey(),
                Desc = Config.GetTempKey(),
                Year = 1936
            };
            Config.SetText(item.Name, "", Game.TechTextFileName);
            Config.SetText(item.ShortName, "", Game.TechTextFileName);
            Config.SetText(item.Desc, "", Game.TechTextFileName);

            // 重複文字列リストに登録する
            Techs.AddDuplicatedListItem(item);

            // 編集済みフラグを設定する
            grp.SetDirty();
            item.SetDirtyAll();

            ITechItem selected = techListBox.SelectedItem as ITechItem;
            if (selected != null)
            {
                // 選択項目の先頭座標を引き継ぐ
                item.Positions.Add(new TechPosition { X = selected.Positions[0].X, Y = selected.Positions[0].Y });

                if (selected is TechItem)
                {
                    // 選択項目が技術アプリケーションならばIDを10増やす
                    TechItem selectedApplication = selected as TechItem;
                    item.Id = Techs.GetNewId(selectedApplication.Id + 10);
                }
                else
                {
                    // 未使用の技術IDを1010以降で検索する
                    item.Id = Techs.GetNewId(1010);
                }

                // 空の小研究を追加する
                item.CreateNewComponents();

                // 技術項目リストに項目を挿入する
                grp.InsertItem(item, selected);

                // 項目リストビューに項目を挿入する
                InsertTechListItem(item, techListBox.SelectedIndex + 1);
            }
            else
            {
                // 仮の座標を登録する
                item.Positions.Add(new TechPosition());

                // 未使用の技術IDを1010以降で検索する
                item.Id = Techs.GetNewId(1010);

                // 空の小研究を追加する
                item.CreateNewComponents();

                // 技術項目リストに項目を追加する
                grp.AddItem(item);

                // 項目リストビューに項目を追加する
                AddTechListItem(item);
            }

            // 技術ツリーにラベルを追加する
            _techTreePanelController.AddItem(item);

            // 技術項目とIDの対応付けを更新する
            Techs.UpdateTechIdMap();
            // 必要技術コンボボックスの項目を更新する
            UpdateRequiredTechListItems();
            // 技術イベントの技術IDコンボボックスの項目を更新する
            UpdateEventTechListItems();

            // 技術項目リストの更新を通知する
            _controller.NotifyItemChange(EditorItemId.TechItemList);

            Log.Info("[Tech] Added new tech: {0}", item.Id);
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
            TechLabel item = new TechLabel { Name = Config.GetTempKey() };
            Config.SetText(item.Name, "", Game.TechTextFileName);

            // 重複文字列リストに登録する
            Techs.AddDuplicatedListItem(item);

            // 編集済みフラグを設定する
            grp.SetDirty();
            item.SetDirtyAll();

            ITechItem selected = techListBox.SelectedItem as ITechItem;
            if (selected != null)
            {
                // 選択項目の先頭座標を引き継ぐ
                item.Positions.Add(new TechPosition { X = selected.Positions[0].X, Y = selected.Positions[0].Y });

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
            _techTreePanelController.AddItem(item);

            Log.Info("[Tech] Added new label");
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
            TechEvent item = new TechEvent();

            // 編集済みフラグを設定する
            grp.SetDirty();
            item.SetDirtyAll();

            ITechItem selected = techListBox.SelectedItem as ITechItem;
            if (selected != null)
            {
                // 選択項目の先頭座標を引き継ぐ
                item.Positions.Add(new TechPosition { X = selected.Positions[0].X, Y = selected.Positions[0].Y });

                // 選択項目が技術イベントならばIDを10増やす
                if (selected is TechEvent)
                {
                    TechEvent selectedEvent = selected as TechEvent;
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
            _techTreePanelController.AddItem(item);

            Log.Info("[Tech] Added new event: {0}", item.Id);
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

            // 重複文字列リストに登録する
            Techs.AddDuplicatedListItem(item);

            // 技術項目リストに項目を挿入する
            grp.InsertItem(item, selected);

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

            // 項目リストビューに項目を挿入する
            InsertTechListItem(item, techListBox.SelectedIndex + 1);

            // 技術ツリーにラベルを追加する
            _techTreePanelController.AddItem(item);

            if (item is TechItem)
            {
                // 技術項目リストの更新を通知する
                _controller.NotifyItemChange(EditorItemId.TechItemList);

                TechItem techItem = item as TechItem;
                Log.Info("[Tech] Added new tech: {0}", techItem.Id);
            }
            else if (item is TechLabel)
            {
                Log.Info("[Tech] Added new label");
            }
            else if (item is TechEvent)
            {
                TechEvent eventItem = item as TechEvent;
                Log.Info("[Tech] Added new event: {0}", eventItem.Id);
            }
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
            _techTreePanelController.RemoveItem(selected);

            if (selected is TechItem)
            {
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

            if (selected is TechItem)
            {
                // 技術項目リストの更新を通知する
                _controller.NotifyItemChange(EditorItemId.TechItemList);

                TechItem techItem = selected as TechItem;
                Log.Info("[Tech] Removed tech: {0} [{1}]", techItem.Id, techItem);
            }
            else if (selected is TechLabel)
            {
                TechLabel labelItem = selected as TechLabel;
                Log.Info("[Tech] Removed label: {0}", labelItem);
            }
            else if (selected is TechEvent)
            {
                TechEvent eventItem = selected as TechEvent;
                Log.Info("[Tech] Removed event: {0}", eventItem.Id);
            }
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
            ITechItem item = techListBox.Items[src] as ITechItem;
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
        ///     技術ツリーラベルのマウスダウン時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechTreeLabelMouseDown(object sender, TechTreePanelController.ItemMouseEventArgs e)
        {
            // 技術項目リストの項目を選択する
            techListBox.SelectedItem = e.Item;
        }

        /// <summary>
        ///     技術ツリーピクチャーボックスにドロップした時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTreePictureBoxDragDrop(object sender, TechTreePanelController.ItemDragEventArgs e)
        {
            // 座標リストビューの項目を更新する
            for (int i = 0; i < e.Item.Positions.Count; i++)
            {
                if (e.Item.Positions[i] == e.Position)
                {
                    if (e.Item is TechItem)
                    {
                        techPositionListView.Items[i].Text = IntHelper.ToString(e.Position.X);
                        techPositionListView.Items[i].SubItems[1].Text = IntHelper.ToString(e.Position.Y);
                        techXNumericUpDown.Value = e.Position.X;
                        techYNumericUpDown.Value = e.Position.Y;
                        techXNumericUpDown.ForeColor = Color.Red;
                        techYNumericUpDown.ForeColor = Color.Red;
                    }
                    else if (e.Item is TechLabel)
                    {
                        labelPositionListView.Items[i].Text = IntHelper.ToString(e.Position.X);
                        labelPositionListView.Items[i].SubItems[1].Text = IntHelper.ToString(e.Position.Y);
                        labelXNumericUpDown.Value = e.Position.X;
                        labelYNumericUpDown.Value = e.Position.Y;
                        labelXNumericUpDown.ForeColor = Color.Red;
                        labelYNumericUpDown.ForeColor = Color.Red;
                    }
                    else
                    {
                        eventPositionListView.Items[i].Text = IntHelper.ToString(e.Position.X);
                        eventPositionListView.Items[i].SubItems[1].Text = IntHelper.ToString(e.Position.Y);
                        eventXNumericUpDown.Value = e.Position.X;
                        eventYNumericUpDown.Value = e.Position.Y;
                        eventXNumericUpDown.ForeColor = Color.Red;
                        eventYNumericUpDown.ForeColor = Color.Red;
                    }
                    break;
                }
            }
            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            e.Item.SetDirty();
            e.Position.SetDirtyAll();
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

            Log.Info("[Tech] Changed category name: {0} -> {1} <{2}>", grp, name, grp.Name);

            // 値を更新する
            Config.SetText(grp.Name, name, Game.TechTextFileName);

            // カテゴリリストボックスの項目を再設定することで表示更新している
            // この時再選択によりフォーカスが外れるので、イベントハンドラを一時的に無効化する
            categoryListBox.SelectedIndexChanged -= OnCategoryListBoxSelectedIndexChanged;
            categoryListBox.Items[(int) grp.Category] = name;
            categoryListBox.SelectedIndexChanged += OnCategoryListBoxSelectedIndexChanged;

            // 編集済みフラグを設定する
            grp.SetDirty(TechGroupItemId.Name);

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

            Log.Info("[Tech] Changed category description: {0} -> {1} <{2}>", grp.GetDesc(), desc, grp.Desc);

            // 値を更新する
            Config.SetText(grp.Desc, desc, Game.TechTextFileName);

            // 編集済みフラグを設定する
            grp.SetDirty(TechGroupItemId.Desc);

            // 文字色を変更する
            categoryDescTextBox.ForeColor = Color.Red;
        }

        #endregion

        #region 技術タブ

        /// <summary>
        ///     技術タブの項目を初期化する
        /// </summary>
        private void InitTechItems()
        {
            // 画像ファイル名
            bool flag = Game.Type == GameType.DarkestHour;
            techPictureNameLabel.Enabled = flag;
            techPictureNameTextBox.Enabled = flag;
            techPictureNameBrowseButton.Enabled = flag;
        }

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
            techIdNumericUpDown.Text = IntHelper.ToString((int) techIdNumericUpDown.Value);
            techYearNumericUpDown.Text = IntHelper.ToString((int) techYearNumericUpDown.Value);
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
            Image prev = techPictureBox.Image;
            techPictureBox.Image = null;
            prev?.Dispose();
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
                ListViewItem li = new ListViewItem(IntHelper.ToString(position.X));
                li.SubItems.Add(IntHelper.ToString(position.Y));
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
            techXNumericUpDown.Text = IntHelper.ToString((int) techXNumericUpDown.Value);
            techYNumericUpDown.Text = IntHelper.ToString((int) techYNumericUpDown.Value);

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
            TechItem item = GetSelectedItem() as TechItem;
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

            // 重複文字列ならば定義名を再設定する
            if (Techs.IsDuplicatedName(item.Name))
            {
                Techs.DecrementDuplicatedListCount(item.Name);
                item.Name = Config.GetTempKey();
                Techs.IncrementDuplicatedListCount(item.Name);
            }

            Log.Info("[Tech] Changed tech name: {0} -> {1} <{2}>", item, name, item.Name);

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

            // 文字色を変更する
            techNameTextBox.ForeColor = Color.Red;

            // 技術項目名の更新を通知する
            _controller.NotifyItemChange(EditorItemId.TechItemName);
        }

        /// <summary>
        ///     技術短縮名変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechShortNameTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            TechItem item = GetSelectedItem() as TechItem;
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

            // 重複文字列ならば定義名を再設定する
            if (Techs.IsDuplicatedName(item.ShortName))
            {
                Techs.DecrementDuplicatedListCount(item.ShortName);
                item.ShortName = Config.GetTempKey();
                Techs.IncrementDuplicatedListCount(item.ShortName);
            }

            Log.Info("[Tech] Changed tech short name: {0} -> {1} <{2}>", item.GetShortName(), shortName, item.ShortName);

            // 値を更新する
            Config.SetText(item.ShortName, shortName, Game.TechTextFileName);

            // 技術ツリー上のラベル名を更新する
            _techTreePanelController.UpdateItem(item);

            // 編集済みフラグを設定する
            item.SetDirty(TechItemId.ShortName);

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
            TechItem item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            int id = (int) techIdNumericUpDown.Value;
            if (id == item.Id)
            {
                return;
            }

            Log.Info("[Tech] Changed tech id: {0} -> {1} [{2}]", item.Id, id, item.Name);

            // 値を更新する
            Techs.ModifyTechId(item, id);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty(TechItemId.Id);

            // 文字色を変更する
            techIdNumericUpDown.ForeColor = Color.Red;

            // 技術項目IDの更新を通知する
            _controller.NotifyItemChange(EditorItemId.TechItemId);
        }

        /// <summary>
        ///     史実年度変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechYearNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            TechItem item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            int year = (int) techYearNumericUpDown.Value;
            if (year == item.Year)
            {
                return;
            }

            Log.Info("[Tech] Changed tech year: {0} -> {1} [{2}]", item.Year, year, item);

            // 値を更新する
            item.Year = year;

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty(TechItemId.Year);

            // 文字色を変更する
            techYearNumericUpDown.ForeColor = Color.Red;

            // 技術項目の史実年度の更新を通知する
            _controller.NotifyItemChange(EditorItemId.TechItemYear);
        }

        /// <summary>
        ///     技術座標リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechPositionListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            TechItem item = GetSelectedItem() as TechItem;
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
        ///     技術座標リストビューの列の幅変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechPositionListViewColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if ((e.ColumnIndex >= 0) && (e.ColumnIndex < PositionListColumnCount))
            {
                HoI2EditorController.Settings.TechEditor.TechPositionListColumnWidth[e.ColumnIndex] =
                    techPositionListView.Columns[e.ColumnIndex].Width;
            }
        }

        /// <summary>
        ///     技術座標リストビューの項目編集前の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechPositionListViewQueryItemEdit(object sender, QueryListViewItemEditEventArgs e)
        {
            switch (e.Column)
            {
                case 0: // X
                    e.Type = ItemEditType.Text;
                    e.Text = techXNumericUpDown.Text;
                    break;

                case 1: // Y
                    e.Type = ItemEditType.Text;
                    e.Text = techYNumericUpDown.Text;
                    break;
            }
        }

        /// <summary>
        ///     技術座標リストビューの項目編集後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechPositionListViewBeforeItemEdit(object sender, ListViewItemEditEventArgs e)
        {
            switch (e.Column)
            {
                case 0: // X
                    techXNumericUpDown.Text = e.Text;
                    break;

                case 1: // Y
                    techYNumericUpDown.Text = e.Text;
                    break;
            }

            // 自前でリストビューの項目を更新するのでキャンセル扱いとする
            e.Cancel = true;
        }

        /// <summary>
        ///     技術座標リストビューの項目入れ替え時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechPositionListViewItemReordered(object sender, ItemReorderedEventArgs e)
        {
            // 選択項目がなければ何もしない
            TechItem item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            int srcIndex = e.OldDisplayIndices[0];
            int destIndex = e.NewDisplayIndex;

            // 技術座標を移動する
            TechPosition position = item.Positions[srcIndex];
            item.Positions.Insert(destIndex, position);
            if (srcIndex < destIndex)
            {
                item.Positions.RemoveAt(srcIndex);
            }
            else
            {
                item.Positions.RemoveAt(srcIndex + 1);
            }

            Log.Info("[Tech] Move tech position: {0} -> {1} ({2}, {3}) [{4}]", srcIndex, destIndex, position.X,
                position.Y, item);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
        }

        /// <summary>
        ///     技術X座標変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechXNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            TechItem item = GetSelectedItem() as TechItem;
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
            int x = (int) techXNumericUpDown.Value;
            if (x == position.X)
            {
                return;
            }

            Log.Info("[Tech] Changed tech position: ({0},{1}) -> ({2},{1}) [{3}]", position.X, position.Y, x, item);

            // 値を更新する
            position.X = x;

            // 座標リストビューの表示を更新する
            techPositionListView.Items[index].Text = IntHelper.ToString(x);

            // ラベルの位置を更新する
            _techTreePanelController.UpdateItem(item, position);

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
            TechItem item = GetSelectedItem() as TechItem;
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
            int y = (int) techYNumericUpDown.Value;
            if (y == position.Y)
            {
                return;
            }

            Log.Info("[Tech] Changed tech position: ({0},{1}) -> ({0},{2}) [{3}]", position.X, position.Y, y, item);

            // 値を更新する
            position.Y = y;

            // 座標リストビューの表示を更新する
            techPositionListView.Items[index].SubItems[1].Text = IntHelper.ToString(y);

            // ラベルの位置を更新する
            _techTreePanelController.UpdateItem(item, position);

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
            TechItem item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            // 座標をリストに追加する
            TechPosition position = new TechPosition { X = 0, Y = 0 };
            item.Positions.Add(position);

            Log.Info("[Tech] Added tech position: ({0}, {1}) [{2}]", position.X, position.Y, item);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            position.SetDirtyAll();

            // 座標リストビューの項目を追加する
            ListViewItem li = new ListViewItem { Text = IntHelper.ToString(position.X) };
            li.SubItems.Add(IntHelper.ToString(position.Y));
            techPositionListView.Items.Add(li);

            // 追加した項目を選択する
            techPositionListView.Items[techPositionListView.Items.Count - 1].Focused = true;
            techPositionListView.Items[techPositionListView.Items.Count - 1].Selected = true;
            techPositionListView.EnsureVisible(techPositionListView.Items.Count - 1);

            // 編集項目を有効化する
            EnableTechPositionItems();

            // 技術ツリーにラベルを追加する
            _techTreePanelController.AddItem(item, position);
        }

        /// <summary>
        ///     技術座標削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechPositionRemoveButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            TechItem item = GetSelectedItem() as TechItem;
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

            Log.Info("[Tech] Removed tech position: ({0}, {1}) [{2}]", position.X, position.Y, item);

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
            _techTreePanelController.RemoveItem(item, position);
        }

        /// <summary>
        ///     技術画像を更新する
        /// </summary>
        /// <param name="item">技術アプリケーション</param>
        private void UpdateTechPicture(TechItem item)
        {
            // 画像ファイル名テキストボックスの値を更新する
            if (Game.Type == GameType.DarkestHour)
            {
                techPictureNameTextBox.Text = item.PictureName ?? "";
            }

            // 編集項目の色を更新する
            techPictureNameTextBox.ForeColor = item.IsDirty(TechItemId.PictureName)
                ? Color.Red
                : SystemColors.WindowText;

            Image prev = techPictureBox.Image;
            string name = !string.IsNullOrEmpty(item.PictureName) &&
                          (item.PictureName.IndexOfAny(Path.GetInvalidPathChars()) < 0)
                ? item.PictureName
                : IntHelper.ToString(item.Id);
            string fileName = Game.GetReadFileName(Game.TechPicturePathName, $"{name}.bmp");
            if (File.Exists(fileName))
            {
                // 技術画像を更新する
                Bitmap bitmap = new Bitmap(fileName);
                bitmap.MakeTransparent();
                techPictureBox.Image = bitmap;
            }
            else
            {
                techPictureBox.Image = null;
            }
            prev?.Dispose();
        }

        /// <summary>
        ///     画像ファイル名変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechPictureNameTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            TechItem item = GetSelectedItem() as TechItem;
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

            Log.Info("[Tech] Changed tech picture: {0} -> {1} [{2}]", item.PictureName, pictureName, item);

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
            TechItem item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            OpenFileDialog dialog = new OpenFileDialog
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
            Graphics g = Graphics.FromHwnd(Handle);
            int margin = DeviceCaps.GetScaledWidth(2) + 1;

            andTechComboBox.BeginUpdate();
            orTechComboBox.BeginUpdate();

            andTechComboBox.Items.Clear();
            orTechComboBox.Items.Clear();

            int width = andTechComboBox.Width;
            foreach (TechItem item in Techs.TechIdMap.Select(pair => pair.Value))
            {
                andTechComboBox.Items.Add(item);
                orTechComboBox.Items.Add(item);
                width = Math.Max(width,
                    (int) g.MeasureString(item.ToString(), andTechComboBox.Font).Width +
                    SystemInformation.VerticalScrollBarWidth + margin);
            }
            andTechComboBox.DropDownWidth = width;
            orTechComboBox.DropDownWidth = width;

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
                ListViewItem li = new ListViewItem { Text = IntHelper.ToString(id) };
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
                ListViewItem li = new ListViewItem { Text = IntHelper.ToString(id) };
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
            andIdNumericUpDown.Text = IntHelper.ToString((int) andIdNumericUpDown.Value);

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
            orIdNumericUpDown.Text = IntHelper.ToString((int) orIdNumericUpDown.Value);

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
            TechItem item = GetSelectedItem() as TechItem;
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
            TechItem item = GetSelectedItem() as TechItem;
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
            TechItem item = GetSelectedItem() as TechItem;
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
            TechItem item = GetSelectedItem() as TechItem;
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
        ///     AND条件必要技術リストの列の幅変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAndRequiredListViewColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if ((e.ColumnIndex >= 0) && (e.ColumnIndex < RequiredListColumnCount))
            {
                HoI2EditorController.Settings.TechEditor.AndRequiredListColumnWidth[e.ColumnIndex] =
                    andRequiredListView.Columns[e.ColumnIndex].Width;
            }
        }

        /// <summary>
        ///     OR条件必要技術リストの列の幅変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOrRequiredListViewColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if ((e.ColumnIndex >= 0) && (e.ColumnIndex < RequiredListColumnCount))
            {
                HoI2EditorController.Settings.TechEditor.OrRequiredListColumnWidth[e.ColumnIndex] =
                    orRequiredListView.Columns[e.ColumnIndex].Width;
            }
        }

        /// <summary>
        ///     AND条件必要技術リストビューの項目編集前の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAndRequiredListViewQueryItemEdit(object sender, QueryListViewItemEditEventArgs e)
        {
            switch (e.Column)
            {
                case 0: // ID
                    e.Type = ItemEditType.Text;
                    e.Text = andIdNumericUpDown.Text;
                    break;

                case 1: // 名前
                    e.Type = ItemEditType.List;
                    e.Items = andTechComboBox.Items.Cast<string>();
                    e.Index = andTechComboBox.SelectedIndex;
                    e.DropDownWidth = andTechComboBox.DropDownWidth;
                    break;
            }
        }

        /// <summary>
        ///     AND条件必要技術リストビューの項目編集後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAndRequiredListViewBeforeItemEdit(object sender, ListViewItemEditEventArgs e)
        {
            switch (e.Column)
            {
                case 0: // ID
                    andIdNumericUpDown.Text = e.Text;
                    break;

                case 1: // 名前
                    andTechComboBox.SelectedIndex = e.Index;
                    break;
            }

            // 自前でリストビューの項目を更新するのでキャンセル扱いとする
            e.Cancel = true;
        }

        /// <summary>
        ///     OR条件必要技術リストビューの項目編集前の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOrRequiredListViewQueryItemEdit(object sender, QueryListViewItemEditEventArgs e)
        {
            switch (e.Column)
            {
                case 0: // ID
                    e.Type = ItemEditType.Text;
                    e.Text = orIdNumericUpDown.Text;
                    break;

                case 1: // 名前
                    e.Type = ItemEditType.List;
                    e.Items = orTechComboBox.Items.Cast<string>();
                    e.Index = orTechComboBox.SelectedIndex;
                    e.DropDownWidth = orTechComboBox.DropDownWidth;
                    break;
            }
        }

        /// <summary>
        ///     OR条件必要技術リストビューの項目編集後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOrRequiredListViewBeforeItemEdit(object sender, ListViewItemEditEventArgs e)
        {
            switch (e.Column)
            {
                case 0: // ID
                    orIdNumericUpDown.Text = e.Text;
                    break;

                case 1: // 名前
                    orTechComboBox.SelectedIndex = e.Index;
                    break;
            }

            // 自前でリストビューの項目を更新するのでキャンセル扱いとする
            e.Cancel = true;
        }

        /// <summary>
        ///     AND条件必要技術リストビューの項目入れ替え時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAndRequiredListViewItemReordered(object sender, ItemReorderedEventArgs e)
        {
            // 選択項目がなければ何もしない
            TechItem item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            int srcIndex = e.OldDisplayIndices[0];
            int destIndex = e.NewDisplayIndex;

            // 必要技術を移動する
            RequiredTech tech = item.AndRequiredTechs[srcIndex];
            item.AndRequiredTechs.Insert(destIndex, tech);
            if (srcIndex < destIndex)
            {
                item.AndRequiredTechs.RemoveAt(srcIndex);
            }
            else
            {
                item.AndRequiredTechs.RemoveAt(srcIndex + 1);
            }

            Log.Info("[Tech] Move and required tech: {0} -> {1} {2} [{3}]", srcIndex, destIndex, tech.Id, item);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            tech.SetDirty();
        }

        /// <summary>
        ///     OR条件必要技術リストビューの項目入れ替え時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOrRequiredListViewItemReordered(object sender, ItemReorderedEventArgs e)
        {
            // 選択項目がなければ何もしない
            TechItem item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            int srcIndex = e.OldDisplayIndices[0];
            int destIndex = e.NewDisplayIndex;

            // 必要技術を移動する
            RequiredTech tech = item.OrRequiredTechs[srcIndex];
            item.OrRequiredTechs.Insert(destIndex, tech);
            if (srcIndex < destIndex)
            {
                item.OrRequiredTechs.RemoveAt(srcIndex);
            }
            else
            {
                item.OrRequiredTechs.RemoveAt(srcIndex + 1);
            }

            Log.Info("[Tech] Move or required tech: {0} -> {1} {2} [{3}]", srcIndex, destIndex, tech.Id, item);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            tech.SetDirty();
        }

        /// <summary>
        ///     AND条件必要技術追加ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAndAddButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            TechItem item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            // AND条件必要技術リストに項目を追加する
            RequiredTech tech = new RequiredTech();
            item.AndRequiredTechs.Add(tech);

            Log.Info("[Tech] Added and required tech: {0} [{1}]", tech.Id, item);

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
            TechItem item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            // OR条件必要技術リストに項目を追加する
            RequiredTech tech = new RequiredTech();
            item.OrRequiredTechs.Add(tech);

            Log.Info("[Tech] Added or required tech: {0} [{1}]", tech.Id, item);

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
            TechItem item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            if (andRequiredListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = andRequiredListView.SelectedIndices[0];

            Log.Info("[Tech] Removed and required tech: {0} [{1}]", item.AndRequiredTechs[index].Id, item);

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
            TechItem item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            if (orRequiredListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = orRequiredListView.SelectedIndices[0];

            Log.Info("[Tech] Removed or required tech: {0} [{1}]", item.OrRequiredTechs[index].Id, item);

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
            TechItem item = GetSelectedItem() as TechItem;
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
            int id = (int) andIdNumericUpDown.Value;
            if (id == tech.Id)
            {
                return;
            }

            Log.Info("[Tech] Changed and required tech: {0} -> {1} [{2}]", tech.Id, id, item);

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
            TechItem item = GetSelectedItem() as TechItem;
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
            int id = (int) orIdNumericUpDown.Value;
            if (id == tech.Id)
            {
                return;
            }

            Log.Info("[Tech] Changed or required tech: {0} -> {1} [{2}]", tech.Id, id, item);

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
            TechItem item = GetSelectedItem() as TechItem;
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
            TechItem item = GetSelectedItem() as TechItem;
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
            ListViewItem li = new ListViewItem { Text = IntHelper.ToString(id) };
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
            ListViewItem li = new ListViewItem { Text = IntHelper.ToString(id) };
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
            andRequiredListView.Items[index].Text = IntHelper.ToString(id);
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
            orRequiredListView.Items[index].Text = IntHelper.ToString(id);
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
            componentSpecialityComboBox.BeginUpdate();
            componentSpecialityComboBox.Items.Clear();
            Graphics g = Graphics.FromHwnd(componentSpecialityComboBox.Handle);
            int width = componentSpecialityComboBox.Width;
            int additional = SystemInformation.VerticalScrollBarWidth + DeviceCaps.GetScaledWidth(16) + 3;
            foreach (string name in Techs.Specialities
                .Where(speciality => speciality != TechSpeciality.None)
                .Select(Techs.GetSpecialityName))
            {
                componentSpecialityComboBox.Items.Add(name);
                width = Math.Max(width,
                    (int) g.MeasureString(name, componentSpecialityComboBox.Font).Width + additional);
            }
            componentSpecialityComboBox.DropDownWidth = width;
            componentSpecialityComboBox.EndUpdate();
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
            componentIdNumericUpDown.Text = IntHelper.ToString((int) componentIdNumericUpDown.Value);
            componentDifficultyNumericUpDown.Text = IntHelper.ToString((int) componentDifficultyNumericUpDown.Value);

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
            TechItem item = GetSelectedItem() as TechItem;
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
            componentSpecialityComboBox.SelectedIndex = Array.IndexOf(Techs.Specialities, component.Speciality) - 1;
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
        ///     小研究リストビューの列の幅変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComponentListViewColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if ((e.ColumnIndex >= 0) && (e.ColumnIndex < ComponentListColumnCount))
            {
                HoI2EditorController.Settings.TechEditor.ComponentListColumnWidth[e.ColumnIndex] =
                    componentListView.Columns[e.ColumnIndex].Width;
            }
        }

        /// <summary>
        ///     小研究リストビューの項目編集前の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComponentListViewQueryItemEdit(object sender, QueryListViewItemEditEventArgs e)
        {
            switch (e.Column)
            {
                case 0: // ID
                    e.Type = ItemEditType.Text;
                    e.Text = componentIdNumericUpDown.Text;
                    break;

                case 1: // 小研究名
                    e.Type = ItemEditType.Text;
                    e.Text = componentNameTextBox.Text;
                    break;

                case 2: // 研究特性
                    e.Type = ItemEditType.List;
                    e.Items = componentSpecialityComboBox.Items.Cast<string>();
                    e.Index = componentSpecialityComboBox.SelectedIndex;
                    e.DropDownWidth = componentSpecialityComboBox.DropDownWidth;
                    break;

                case 3: // 難易度
                    e.Type = ItemEditType.Text;
                    e.Text = componentDifficultyNumericUpDown.Text;
                    break;

                case 4: // 2倍
                    e.Type = ItemEditType.Bool;
                    e.Flag = componentDoubleTimeCheckBox.Checked;
                    break;
            }
        }

        /// <summary>
        ///     小研究リストビューの項目編集後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComponentListViewBeforeItemEdit(object sender, ListViewItemEditEventArgs e)
        {
            switch (e.Column)
            {
                case 0: // ID
                    componentIdNumericUpDown.Text = e.Text;
                    break;

                case 1: // 小研究名
                    componentNameTextBox.Text = e.Text;
                    break;

                case 2: // 研究特性
                    componentSpecialityComboBox.SelectedIndex = e.Index;
                    break;

                case 3: // 難易度
                    componentDifficultyNumericUpDown.Text = e.Text;
                    break;

                case 4: // 2倍
                    componentDoubleTimeCheckBox.Checked = e.Flag;
                    break;
            }

            // 自前でリストビューの項目を更新するのでキャンセル扱いとする
            e.Cancel = true;
        }

        /// <summary>
        ///     小研究リストビューの項目入れ替え時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComponentListViewItemReordered(object sender, ItemReorderedEventArgs e)
        {
            // 選択項目がなければ何もしない
            TechItem item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            int srcIndex = e.OldDisplayIndices[0];
            int destIndex = e.NewDisplayIndex;

            // 小研究を移動する
            TechComponent component = item.Components[srcIndex];
            item.Components.Insert(destIndex, component);
            if (srcIndex < destIndex)
            {
                item.Components.RemoveAt(srcIndex);
            }
            else
            {
                item.Components.RemoveAt(srcIndex + 1);
            }

            Log.Info("[Tech] Move component: {0} -> {1} {2} [{3}]", srcIndex, destIndex, component.Id, item);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            component.SetDirty();

            // 小研究リストの更新を通知する
            _controller.OnItemChanged(EditorItemId.TechComponentList);
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
            int iconWidth = DeviceCaps.GetScaledWidth(16);
            int iconHeight = DeviceCaps.GetScaledHeight(16);
            if (e.Index < Techs.SpecialityImages.Images.Count &&
                !string.IsNullOrEmpty(componentSpecialityComboBox.Items[e.Index].ToString()))
            {
                e.Graphics.DrawImage(Techs.SpecialityImages.Images[e.Index],
                    new Rectangle(e.Bounds.X + 1, e.Bounds.Y + 1, iconWidth, iconHeight));
            }

            // 項目の文字列を描画する
            TechItem item = GetSelectedItem() as TechItem;
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
                    new Rectangle(e.Bounds.X + iconWidth + 3, e.Bounds.Y + 3, e.Bounds.Width - iconHeight - 3,
                        e.Bounds.Height));
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
            TechItem item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            TechComponent component = TechComponent.Create();

            // 重複文字列リストに登録する
            Techs.IncrementDuplicatedListCount(component.Name);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            component.SetDirtyAll();

            if (componentListView.SelectedIndices.Count > 0)
            {
                int index = componentListView.SelectedIndices[0];
                TechComponent selected = item.Components[index];
                component.Id = item.GetNewComponentId(selected.Id + 1);

                // 項目をリストに挿入する
                item.InsertComponent(component, index + 1);

                // 小研究リストビューに項目を挿入する
                InsertComponentListItem(component, index + 1);
            }
            else
            {
                component.Id = item.GetNewComponentId(item.Id + 1);

                // 項目をリストに追加する
                item.AddComponent(component);

                // 小研究リストビューに項目を追加する
                AddComponentListItem(component);
            }

            // 小研究リストの更新を通知する
            _controller.NotifyItemChange(EditorItemId.TechComponentList);

            Log.Info("[Tech] Added new tech component: {0} [{1}]", component.Id, item);
        }

        /// <summary>
        ///     小研究の複製ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComponentCloneButtonClick(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            TechItem item = GetSelectedItem() as TechItem;
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

            TechComponent selected = item.Components[index];
            TechComponent component = selected.Clone();
            component.Id = item.GetNewComponentId(selected.Id);

            Log.Info("[Tech] Added new tech component: {0} [{1}]", component.Id, item);

            // 重複文字列リストに登録する
            Techs.IncrementDuplicatedListCount(component.Name);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            component.SetDirtyAll();

            // 項目をリストに挿入する
            item.InsertComponent(component, index + 1);

            // 小研究リストビューに項目を挿入する
            InsertComponentListItem(component, index + 1);

            // 小研究リストの更新を通知する
            _controller.NotifyItemChange(EditorItemId.TechComponentList);
        }

        /// <summary>
        ///     小研究の削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComponentRemoveButtonClick(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            TechItem item = GetSelectedItem() as TechItem;
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

            Log.Info("[Tech] Removed new tech component: {0} [{1}]", item.Components[index], item);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();

            // 項目をリストから削除する
            item.RemoveComponent(index);

            // 小研究リストビューから項目を削除する
            RemoveComponentListItem(index);

            // 小研究リストの更新を通知する
            _controller.NotifyItemChange(EditorItemId.TechComponentList);
        }

        /// <summary>
        ///     小研究の上へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComponentUpButtonClick(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            TechItem item = GetSelectedItem() as TechItem;
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
            TechItem item = GetSelectedItem() as TechItem;
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
            TechItem item = GetSelectedItem() as TechItem;
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
            int id = (int) componentIdNumericUpDown.Value;
            if (id == component.Id)
            {
                return;
            }

            Log.Info("[Tech] Changed tech component id: {0} -> {1} [{2}]", component.Id, id, component);

            // 値を更新する
            component.Id = id;

            // 小研究リストビューの項目を更新する
            componentListView.Items[index].Text = IntHelper.ToString(id);

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
            TechItem item = GetSelectedItem() as TechItem;
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

            // 重複文字列ならば定義名を再設定する
            if (Techs.IsDuplicatedName(component.Name))
            {
                Techs.DecrementDuplicatedListCount(component.Name);
                component.Name = Config.GetTempKey();
                Techs.IncrementDuplicatedListCount(component.Name);
            }

            Log.Info("[Tech] Changed tech component name: {0} -> {1} <{2}>", component, name, component.Name);

            // 値を更新する
            Config.SetText(component.Name, name, Game.TechTextFileName);

            // 小研究リストビューの項目を更新する
            componentListView.Items[index].SubItems[1].Text = name;

            // 編集済みフラグを設定する
            item.SetDirty();
            component.SetDirty(TechComponentItemId.Name);

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
            TechItem item = GetSelectedItem() as TechItem;
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

            Log.Info("[Tech] Changed tech component speciality: {0} -> {1} [{2}]",
                Techs.GetSpecialityName(component.Speciality), Techs.GetSpecialityName(speciality), component);

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

            // 小研究の特性の更新を通知する
            _controller.NotifyItemChange(EditorItemId.TechComponentSpeciality);
        }

        /// <summary>
        ///     小研究難易度変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComponentDifficultyNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            TechItem item = GetSelectedItem() as TechItem;
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
            int difficulty = (int) componentDifficultyNumericUpDown.Value;
            if (difficulty == component.Difficulty)
            {
                return;
            }

            Log.Info("[Tech] Changed tech component difficulty: {0} -> {1} [{2}]", component.Difficulty, difficulty,
                component);

            // 値を更新する
            component.Difficulty = difficulty;

            // 小研究リストビューの項目を更新する
            componentListView.Items[index].SubItems[3].Text = IntHelper.ToString(difficulty);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            component.SetDirty(TechComponentItemId.Difficulty);

            // 文字色を変更する
            componentDifficultyNumericUpDown.ForeColor = Color.Red;

            // 小研究の難易度の更新を通知する
            _controller.NotifyItemChange(EditorItemId.TechComponentDifficulty);
        }

        /// <summary>
        ///     小研究2倍時間設定変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComponentDoubleTimeCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            TechItem item = GetSelectedItem() as TechItem;
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

            Log.Info("[Tech] Changed tech component double time: {0} -> {1} [{2}]", component.DoubleTime, doubleTime,
                component);

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

            // 小研究の難易度の更新を通知する
            _controller.NotifyItemChange(EditorItemId.TechComponentDoubleTime);
        }

        /// <summary>
        ///     小研究リストの項目を作成する
        /// </summary>
        /// <param name="component">小研究</param>
        /// <returns>小研究リストの項目</returns>
        private static ListViewItem CreateComponentListItem(TechComponent component)
        {
            ListViewItem li = new ListViewItem { Text = IntHelper.ToString(component.Id) };
            li.SubItems.Add(component.ToString());
            li.SubItems.Add(Techs.GetSpecialityName(component.Speciality));
            li.SubItems.Add(IntHelper.ToString(component.Difficulty));
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
            ListViewItem li = componentListView.Items[src].Clone() as ListViewItem;
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
                DisableComponentItems();
            }
        }

        #endregion

        #region 技術効果タブ

        /// <summary>
        ///     技術効果タブの編集項目を初期化する
        /// </summary>
        private void InitEffectItems()
        {
            Graphics g = Graphics.FromHwnd(Handle);
            int margin = DeviceCaps.GetScaledWidth(2) + 1;

            // 技術効果の種類
            commandTypeComboBox.BeginUpdate();
            commandTypeComboBox.Items.Clear();
            int width = commandTypeComboBox.Width;
            foreach (string name in Commands.Types.Select(type => Commands.Strings[(int) type]))
            {
                commandTypeComboBox.Items.Add(name);
                width = Math.Max(width,
                    (int) g.MeasureString(name, commandTypeComboBox.Font).Width +
                    SystemInformation.VerticalScrollBarWidth + margin);
            }
            commandTypeComboBox.DropDownWidth = width;
            commandTypeComboBox.EndUpdate();
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
            TechItem item = GetSelectedItem() as TechItem;
            if (item != null && effectListView.SelectedIndices.Count > 0)
            {
                Command command = item.Effects[effectListView.SelectedIndices[0]];
                Brush brush;
                if ((Commands.Types[e.Index] == command.Type) && command.IsDirty(CommandItemId.Type))
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
            TechItem item = GetSelectedItem() as TechItem;
            if (item != null && effectListView.SelectedIndices.Count > 0)
            {
                Command command = item.Effects[effectListView.SelectedIndices[0]];
                Brush brush = command.IsDirty(CommandItemId.Which)
                    ? new SolidBrush(Color.Red)
                    : new SolidBrush(SystemColors.WindowText);
                string s = ObjectHelper.ToString(commandWhichComboBox.Items[e.Index]);
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
            TechItem item = GetSelectedItem() as TechItem;
            if (item != null && effectListView.SelectedIndices.Count > 0)
            {
                Command command = item.Effects[effectListView.SelectedIndices[0]];
                Brush brush = command.IsDirty(CommandItemId.Value)
                    ? new SolidBrush(Color.Red)
                    : new SolidBrush(SystemColors.WindowText);
                string s = ObjectHelper.ToString(commandValueComboBox.Items[e.Index]);
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
            TechItem item = GetSelectedItem() as TechItem;
            if (item != null && effectListView.SelectedIndices.Count > 0)
            {
                Command command = item.Effects[effectListView.SelectedIndices[0]];
                Brush brush = command.IsDirty(CommandItemId.When)
                    ? new SolidBrush(Color.Red)
                    : new SolidBrush(SystemColors.WindowText);
                string s = ObjectHelper.ToString(commandWhenComboBox.Items[e.Index]);
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
            TechItem item = GetSelectedItem() as TechItem;
            if (item != null && effectListView.SelectedIndices.Count > 0)
            {
                Command command = item.Effects[effectListView.SelectedIndices[0]];
                Brush brush = command.IsDirty(CommandItemId.Where)
                    ? new SolidBrush(Color.Red)
                    : new SolidBrush(SystemColors.WindowText);
                string s = ObjectHelper.ToString(commandWhereComboBox.Items[e.Index]);
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
            TechItem item = GetSelectedItem() as TechItem;
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
                commandTypeComboBox.SelectedIndex = Commands.Types.IndexOf(command.Type);
            }
            else
            {
                commandTypeComboBox.SelectedIndex = -1;
                commandTypeComboBox.Text = "";
            }
            commandWhichComboBox.Text = ObjectHelper.ToString(command.Which);
            commandValueComboBox.Text = ObjectHelper.ToString(command.Value);
            commandWhenComboBox.Text = ObjectHelper.ToString(command.When);
            commandWhereComboBox.Text = ObjectHelper.ToString(command.Where);

            // コンボボックスの色を更新する
            commandTypeComboBox.Refresh();
            commandWhichComboBox.Refresh();
            commandValueComboBox.Refresh();
            commandWhenComboBox.Refresh();
            commandWhereComboBox.Refresh();
            commandWhichComboBox.ForeColor = command.IsDirty(CommandItemId.Which) ? Color.Red : SystemColors.WindowText;
            commandValueComboBox.ForeColor = command.IsDirty(CommandItemId.Value) ? Color.Red : SystemColors.WindowText;
            commandWhenComboBox.ForeColor = command.IsDirty(CommandItemId.When) ? Color.Red : SystemColors.WindowText;
            commandWhereComboBox.ForeColor = command.IsDirty(CommandItemId.Where) ? Color.Red : SystemColors.WindowText;

            // 編集項目を有効化する
            EnableEffectItems();

            effectUpButton.Enabled = index != 0;
            effectDownButton.Enabled = index != item.Effects.Count - 1;
        }

        /// <summary>
        ///     技術効果リストビューの列の幅変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEffectListViewColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if ((e.ColumnIndex >= 0) && (e.ColumnIndex < EffectListColumnCount))
            {
                HoI2EditorController.Settings.TechEditor.EffectListColumnWidth[e.ColumnIndex] =
                    effectListView.Columns[e.ColumnIndex].Width;
            }
        }

        /// <summary>
        ///     技術効果リストビューの項目編集前の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEffectListViewQueryItemEdit(object sender, QueryListViewItemEditEventArgs e)
        {
            switch (e.Column)
            {
                case 0: // 種類
                    e.Type = ItemEditType.List;
                    e.Items = commandTypeComboBox.Items.Cast<string>();
                    e.Index = commandTypeComboBox.SelectedIndex;
                    e.DropDownWidth = commandTypeComboBox.DropDownWidth;
                    break;

                case 1: // Which
                    e.Type = ItemEditType.Text;
                    e.Text = commandWhichComboBox.Text;
                    break;

                case 2: // Value
                    e.Type = ItemEditType.Text;
                    e.Text = commandValueComboBox.Text;
                    break;

                case 3: // When
                    e.Type = ItemEditType.Text;
                    e.Text = commandWhenComboBox.Text;
                    break;

                case 4: // Where
                    e.Type = ItemEditType.Text;
                    e.Text = commandWhereComboBox.Text;
                    break;
            }
        }

        /// <summary>
        ///     技術効果リストビューの項目編集後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEffectListViewBeforeItemEdit(object sender, ListViewItemEditEventArgs e)
        {
            switch (e.Column)
            {
                case 0: // 種類
                    commandTypeComboBox.SelectedIndex = e.Index;
                    break;

                case 1: // Which
                    commandWhichComboBox.Text = e.Text;
                    break;

                case 2: // Value
                    commandValueComboBox.Text = e.Text;
                    break;

                case 3: // When
                    commandWhenComboBox.Text = e.Text;
                    break;

                case 4: // Where
                    commandWhereComboBox.Text = e.Text;
                    break;
            }

            // 自前でリストビューの項目を更新するのでキャンセル扱いとする
            e.Cancel = true;
        }

        /// <summary>
        ///     技術効果リストビューの項目入れ替え時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEffectListViewItemReordered(object sender, ItemReorderedEventArgs e)
        {
            // 選択項目がなければ何もしない
            TechItem item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            int srcIndex = e.OldDisplayIndices[0];
            int destIndex = e.NewDisplayIndex;

            // 技術効果を移動する
            Command command = item.Effects[srcIndex];
            item.Effects.Insert(destIndex, command);
            if (srcIndex < destIndex)
            {
                item.Effects.RemoveAt(srcIndex);
            }
            else
            {
                item.Effects.RemoveAt(srcIndex + 1);
            }

            Log.Info("[Tech] Move effect: {0} -> {1} {2} [{3}]", srcIndex, destIndex,
                Commands.Strings[(int) command.Type], item);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            command.SetDirty();
        }

        /// <summary>
        ///     技術効果の新規ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEffectNewButtonClick(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            TechItem item = GetSelectedItem() as TechItem;
            if (item == null)
            {
                return;
            }

            Command command = new Command();

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

            Log.Info("[Tech] Added new effect: [{0}]", item);
        }

        /// <summary>
        ///     技術効果の複製ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEffectCloneButtonClick(object sender, EventArgs e)
        {
            // 項目リストの選択項目がなければ何もしない
            TechItem item = GetSelectedItem() as TechItem;
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

            Command command = new Command(item.Effects[index]);

            Log.Info("[Tech] Added new effect: {0} [{1}]", Commands.Strings[(int) command.Type], item);

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
            TechItem item = GetSelectedItem() as TechItem;
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

            Log.Info("[Tech] Removed effect: {0} [{1}]", Commands.Strings[(int) item.Effects[index].Type], item);

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
            TechItem item = GetSelectedItem() as TechItem;
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
            TechItem item = GetSelectedItem() as TechItem;
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
            TechItem item = GetSelectedItem() as TechItem;
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
            CommandType type = Commands.Types[commandTypeComboBox.SelectedIndex];
            if (type == command.Type)
            {
                return;
            }

            Log.Info("[Tech] Changed tech effect type: {0} -> {1} [{2}]", Commands.Strings[(int) command.Type],
                Commands.Strings[(int) type], item);

            // 値を更新する
            command.Type = type;

            // 技術効果リストビューの表示を更新する
            effectListView.Items[index].Text = Commands.Strings[(int) type];

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
            TechItem item = GetSelectedItem() as TechItem;
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

            double val;
            if (DoubleHelper.TryParse(commandWhichComboBox.Text, out val))
            {
                // 値に変化がなければ何もしない
                if (ObjectHelper.IsEqual(val, command.Which))
                {
                    return;
                }

                Log.Info("[Tech] Changed tech effect which: {0} -> {1} [{2}]", ObjectHelper.ToString(command.Which),
                    DoubleHelper.ToString(val), item);

                // 値を更新する
                command.Which = val;
            }
            else
            {
                // 値に変化がなければ何もしない
                string text = commandWhichComboBox.Text;
                if (ObjectHelper.IsEqual(text, command.Which))
                {
                    return;
                }

                Log.Info("[Tech] Changed tech effect which: {0} -> {1} [{2}]", ObjectHelper.ToString(command.Which),
                    text, item);

                // 値を更新する
                command.Which = text;
            }

            // 技術効果リストビューの表示を更新する
            effectListView.Items[index].SubItems[1].Text = ObjectHelper.ToString(command.Which);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            command.SetDirty(CommandItemId.Which);

            // 文字色を変更する
            commandWhichComboBox.ForeColor = Color.Red;

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
            TechItem item = GetSelectedItem() as TechItem;
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

            double val;
            if (DoubleHelper.TryParse(commandValueComboBox.Text, out val))
            {
                // 値に変化がなければ何もしない
                if (ObjectHelper.IsEqual(val, command.Value))
                {
                    return;
                }

                Log.Info("[Tech] Changed tech effect value: {0} -> {1} [{2}]", ObjectHelper.ToString(command.Value),
                    DoubleHelper.ToString(val), item);

                // 値を更新する
                command.Value = val;
            }
            else
            {
                // 値に変化がなければ何もしない
                string text = commandValueComboBox.Text;
                if (ObjectHelper.IsEqual(text, command.Value))
                {
                    return;
                }

                Log.Info("[Tech] Changed tech effect value: {0} -> {1} [{2}]", ObjectHelper.ToString(command.Value),
                    text, item);

                // 値を更新する
                command.Value = text;
            }

            // 技術効果リストビューの表示を更新する
            effectListView.Items[index].SubItems[2].Text = ObjectHelper.ToString(command.Value);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            command.SetDirty(CommandItemId.Value);

            // 文字色を変更する
            commandValueComboBox.ForeColor = Color.Red;

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
            TechItem item = GetSelectedItem() as TechItem;
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

            double val;
            if (DoubleHelper.TryParse(commandWhenComboBox.Text, out val))
            {
                // 値に変化がなければ何もしない
                if (ObjectHelper.IsEqual(val, command.When))
                {
                    return;
                }

                Log.Info("[Tech] Changed tech effect when: {0} -> {1} [{2}]", ObjectHelper.ToString(command.When),
                    DoubleHelper.ToString(val), item);

                // 値を更新する
                command.When = val;
            }
            else
            {
                // 値に変化がなければ何もしない
                string text = commandWhenComboBox.Text;
                if (ObjectHelper.IsEqual(text, command.When))
                {
                    return;
                }

                Log.Info("[Tech] Changed tech effect when: {0} -> {1} [{2}]", ObjectHelper.ToString(command.When), text,
                    item);

                // 値を更新する
                command.When = text;
            }

            // 技術効果リストビューの表示を更新する
            effectListView.Items[index].SubItems[3].Text = ObjectHelper.ToString(command.When);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            command.SetDirty(CommandItemId.When);

            // 文字色を変更する
            commandWhenComboBox.ForeColor = Color.Red;

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
            TechItem item = GetSelectedItem() as TechItem;
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

            double val;
            if (DoubleHelper.TryParse(commandWhereComboBox.Text, out val))
            {
                // 値に変化がなければ何もしない
                if (ObjectHelper.IsEqual(val, command.Where))
                {
                    return;
                }

                Log.Info("[Tech] Changed tech effect where: {0} -> {1} [{2}]", ObjectHelper.ToString(command.Where),
                    DoubleHelper.ToString(val), item);

                // 値を更新する
                command.Where = val;
            }
            else
            {
                // 値に変化がなければ何もしない
                string text = commandWhereComboBox.Text;
                if (ObjectHelper.IsEqual(text, command.Where))
                {
                    return;
                }

                Log.Info("[Tech] Changed tech effect where: {0} -> {1} [{2}]", ObjectHelper.ToString(command.Where),
                    text, item);

                // 値を更新する
                command.Where = text;
            }

            // 技術効果リストビューの表示を更新する
            effectListView.Items[index].SubItems[4].Text = ObjectHelper.ToString(command.Where);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            command.SetDirty(CommandItemId.Where);

            // 文字色を変更する
            commandWhereComboBox.ForeColor = Color.Red;

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
            ListViewItem li = new ListViewItem { Text = Commands.Strings[(int) command.Type] };
            li.SubItems.Add(ObjectHelper.ToString(command.Which));
            li.SubItems.Add(ObjectHelper.ToString(command.Value));
            li.SubItems.Add(ObjectHelper.ToString(command.When));
            li.SubItems.Add(ObjectHelper.ToString(command.Where));

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
            ListViewItem li = effectListView.Items[src].Clone() as ListViewItem;
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
            labelXNumericUpDown.Text = IntHelper.ToString((int) labelXNumericUpDown.Value);
            labelYNumericUpDown.Text = IntHelper.ToString((int) labelYNumericUpDown.Value);
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
            labelXNumericUpDown.Text = IntHelper.ToString((int) labelXNumericUpDown.Value);
            labelYNumericUpDown.Text = IntHelper.ToString((int) labelYNumericUpDown.Value);

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
                ListViewItem listItem = new ListViewItem(IntHelper.ToString(position.X));
                listItem.SubItems.Add(IntHelper.ToString(position.Y));
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
            TechLabel item = GetSelectedItem() as TechLabel;
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

            // 重複文字列ならば定義名を再設定する
            if (Techs.IsDuplicatedName(item.Name))
            {
                Techs.DecrementDuplicatedListCount(item.Name);
                item.Name = Config.GetTempKey();
                Techs.IncrementDuplicatedListCount(item.Name);
            }

            Log.Info("[Tech] Changed label name: {0} -> {1} <{2}>", item, text, item.Name);

            // 値を更新する
            Config.SetText(item.Name, text, Game.TechTextFileName);

            // 項目リストボックスの項目を再設定することで表示更新している
            // この時再選択によりフォーカスが外れるので、イベントハンドラを一時的に無効化する
            techListBox.SelectedIndexChanged -= OnTechListBoxSelectedIndexChanged;
            techListBox.Items[techListBox.SelectedIndex] = item;
            techListBox.SelectedIndexChanged += OnTechListBoxSelectedIndexChanged;

            // 技術ツリー上のラベル名を更新する
            _techTreePanelController.UpdateItem(item);

            // 編集済みフラグを設定する
            item.SetDirty(TechItemId.Name);

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
            TechLabel item = GetSelectedItem() as TechLabel;
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
        ///     ラベル座標リストビューの列の幅変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLabelPositionListViewColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if ((e.ColumnIndex >= 0) && (e.ColumnIndex < PositionListColumnCount))
            {
                HoI2EditorController.Settings.TechEditor.LabelPositionListColumnWidth[e.ColumnIndex] =
                    labelPositionListView.Columns[e.ColumnIndex].Width;
            }
        }

        /// <summary>
        ///     ラベル座標リストビューの項目編集前の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLabelPositionListViewQueryItemEdit(object sender, QueryListViewItemEditEventArgs e)
        {
            switch (e.Column)
            {
                case 0: // X
                    e.Type = ItemEditType.Text;
                    e.Text = labelXNumericUpDown.Text;
                    break;

                case 1: // Y
                    e.Type = ItemEditType.Text;
                    e.Text = labelYNumericUpDown.Text;
                    break;
            }
        }

        /// <summary>
        ///     ラベル座標リストビューの項目編集後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLabelPositionListViewBeforeItemEdit(object sender, ListViewItemEditEventArgs e)
        {
            switch (e.Column)
            {
                case 0: // X
                    labelXNumericUpDown.Text = e.Text;
                    break;

                case 1: // Y
                    labelYNumericUpDown.Text = e.Text;
                    break;
            }

            // 自前でリストビューの項目を更新するのでキャンセル扱いとする
            e.Cancel = true;
        }

        /// <summary>
        ///     ラベル座標リストビューの項目入れ替え時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLabelPositionListViewItemReordered(object sender, ItemReorderedEventArgs e)
        {
            // 選択項目がなければ何もしない
            TechLabel item = GetSelectedItem() as TechLabel;
            if (item == null)
            {
                return;
            }

            int srcIndex = e.OldDisplayIndices[0];
            int destIndex = e.NewDisplayIndex;

            // ラベル座標を移動する
            TechPosition position = item.Positions[srcIndex];
            item.Positions.Insert(destIndex, position);
            if (srcIndex < destIndex)
            {
                item.Positions.RemoveAt(srcIndex);
            }
            else
            {
                item.Positions.RemoveAt(srcIndex + 1);
            }

            Log.Info("[Tech] Move label position: {0} -> {1} ({2}, {3}) [{4}]", srcIndex, destIndex, position.X,
                position.Y, item);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
        }

        /// <summary>
        ///     ラベルX座標変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLabelXNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            TechLabel item = GetSelectedItem() as TechLabel;
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
            int x = (int) labelXNumericUpDown.Value;
            if (x == position.X)
            {
                return;
            }

            Log.Info("[Tech] Changed label position: ({0},{1}) -> ({2},{1}) [{3}]", position.X, position.Y, x, item);

            // 値を更新する
            position.X = x;

            // ラベル座標リストビューの項目を更新する
            labelPositionListView.Items[index].Text = IntHelper.ToString(x);

            // 技術ツリー上のラベルを移動する
            _techTreePanelController.UpdateItem(item, position);

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
            TechLabel item = GetSelectedItem() as TechLabel;
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
            int y = (int) labelYNumericUpDown.Value;
            if (y == position.Y)
            {
                return;
            }

            Log.Info("[Tech] Changed label position: ({0},{1}) -> ({0},{2}) [{3}]", position.X, position.Y, y, item);

            // 値を更新する
            position.Y = y;

            // ラベル座標リストビューの項目を更新する
            labelPositionListView.Items[index].SubItems[1].Text = IntHelper.ToString(y);

            // 技術ツリー上のラベルを移動する
            _techTreePanelController.UpdateItem(item, position);

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
            TechLabel item = GetSelectedItem() as TechLabel;
            if (item == null)
            {
                return;
            }

            // ラベル座標リストに項目を追加する
            TechPosition position = new TechPosition { X = 0, Y = 0 };
            item.Positions.Add(position);

            Log.Info("[Tech] Added label position: ({0},{1}) [{2}]", position.X, position.Y, item);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            position.SetDirtyAll();

            // ラベル座標リストビューの項目を追加する
            ListViewItem li = new ListViewItem { Text = IntHelper.ToString(position.X) };
            li.SubItems.Add(IntHelper.ToString(position.Y));
            labelPositionListView.Items.Add(li);

            // 追加した項目を選択する
            labelPositionListView.Items[labelPositionListView.Items.Count - 1].Focused = true;
            labelPositionListView.Items[labelPositionListView.Items.Count - 1].Selected = true;

            // ラベル座標の編集項目を有効化する
            EnableLabelPositionItems();

            // 技術ツリーにラベルを追加する
            _techTreePanelController.AddItem(item, position);
        }

        /// <summary>
        ///     ラベル座標削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLabelPositionRemoveButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            TechLabel item = GetSelectedItem() as TechLabel;
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

            Log.Info("[Tech] Removed label position: ({0},{1}) [{2}]", position.X, position.Y, item);

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
            _techTreePanelController.RemoveItem(item, position);
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
            Graphics g = Graphics.FromHwnd(Handle);
            int margin = DeviceCaps.GetScaledWidth(2) + 1;

            eventTechComboBox.BeginUpdate();
            eventTechComboBox.Items.Clear();

            int width = eventTechComboBox.Width;
            foreach (TechItem item in Techs.TechIdMap.Select(pair => pair.Value))
            {
                eventTechComboBox.Items.Add(item);
                width = Math.Max(width,
                    (int) g.MeasureString(item.ToString(), eventTechComboBox.Font).Width +
                    SystemInformation.VerticalScrollBarWidth + margin);
            }
            eventTechComboBox.DropDownWidth = width;

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
            eventIdNumericUpDown.Text = IntHelper.ToString((int) eventIdNumericUpDown.Value);
            eventTechNumericUpDown.Text = IntHelper.ToString((int) eventTechNumericUpDown.Value);
            eventXNumericUpDown.Text = IntHelper.ToString((int) eventXNumericUpDown.Value);
            eventYNumericUpDown.Text = IntHelper.ToString((int) eventYNumericUpDown.Value);
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
            eventXNumericUpDown.Text = IntHelper.ToString((int) eventXNumericUpDown.Value);
            eventYNumericUpDown.Text = IntHelper.ToString((int) eventYNumericUpDown.Value);

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
                ListViewItem listItem = new ListViewItem(IntHelper.ToString(position.X));
                listItem.SubItems.Add(IntHelper.ToString(position.Y));
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
            TechEvent item = GetSelectedItem() as TechEvent;
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
            TechEvent item = GetSelectedItem() as TechEvent;
            if (item == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            int id = (int) eventIdNumericUpDown.Value;
            if (id == item.Id)
            {
                return;
            }

            Log.Info("[Tech] Changed event id: {0} -> {1}", item.Id, id);

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
            TechEvent item = GetSelectedItem() as TechEvent;
            if (item == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            int id = (int) eventTechNumericUpDown.Value;
            if (id == item.TechId)
            {
                return;
            }

            Log.Info("[Tech] Changed event tech id: {0} -> {1}", item.TechId, id);

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
            TechEvent item = GetSelectedItem() as TechEvent;
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
            TechEvent item = GetSelectedItem() as TechEvent;
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
        ///     発明イベント座標リストビューの列の幅変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEventPositionListViewColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if ((e.ColumnIndex >= 0) && (e.ColumnIndex < PositionListColumnCount))
            {
                HoI2EditorController.Settings.TechEditor.EventPositionListColumnWidth[e.ColumnIndex] =
                    eventPositionListView.Columns[e.ColumnIndex].Width;
            }
        }

        /// <summary>
        ///     発明イベント座標リストビューの項目編集前の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEventPositionListViewQueryItemEdit(object sender, QueryListViewItemEditEventArgs e)
        {
            switch (e.Column)
            {
                case 0: // X
                    e.Type = ItemEditType.Text;
                    e.Text = eventXNumericUpDown.Text;
                    break;

                case 1: // Y
                    e.Type = ItemEditType.Text;
                    e.Text = eventYNumericUpDown.Text;
                    break;
            }
        }

        /// <summary>
        ///     発明イベント座標リストビューの項目編集後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEventPositionListViewBeforeItemEdit(object sender, ListViewItemEditEventArgs e)
        {
            switch (e.Column)
            {
                case 0: // X
                    eventXNumericUpDown.Text = e.Text;
                    break;

                case 1: // Y
                    eventYNumericUpDown.Text = e.Text;
                    break;
            }

            // 自前でリストビューの項目を更新するのでキャンセル扱いとする
            e.Cancel = true;
        }

        /// <summary>
        ///     発明イベント座標リストビューの項目入れ替え時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEventPositionListViewItemReordered(object sender, ItemReorderedEventArgs e)
        {
            // 選択項目がなければ何もしない
            TechEvent item = GetSelectedItem() as TechEvent;
            if (item == null)
            {
                return;
            }

            int srcIndex = e.OldDisplayIndices[0];
            int destIndex = e.NewDisplayIndex;

            // ラベル座標を移動する
            TechPosition position = item.Positions[srcIndex];
            item.Positions.Insert(destIndex, position);
            if (srcIndex < destIndex)
            {
                item.Positions.RemoveAt(srcIndex);
            }
            else
            {
                item.Positions.RemoveAt(srcIndex + 1);
            }

            Log.Info("[Tech] Move event position: {0} -> {1} ({2}, {3}) [{4}]", srcIndex, destIndex, position.X,
                position.Y, item);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
        }

        /// <summary>
        ///     発明イベントX座標変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEventXNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            TechEvent item = GetSelectedItem() as TechEvent;
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
            int x = (int) eventXNumericUpDown.Value;
            if (x == position.X)
            {
                return;
            }

            Log.Info("[Tech] Changed event position: ({0},{1}) -> ({2},{1}) [{3}]", position.X, position.Y, x, item);

            // 値を更新する
            position.X = x;

            // 発明イベント座標リストビューの項目を更新する
            eventPositionListView.Items[index].Text = IntHelper.ToString(x);

            // 技術ツリー上のラベルを移動する
            _techTreePanelController.UpdateItem(item, position);

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
            TechEvent item = GetSelectedItem() as TechEvent;
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
            int y = (int) eventYNumericUpDown.Value;
            if (y == position.Y)
            {
                return;
            }

            Log.Info("[Tech] Changed event position: ({0},{1}) -> ({0},{2}) [{3}]", position.X, position.Y, y, item);

            // 値を更新する
            position.Y = y;

            // 発明イベント座標リストビューの項目を更新する
            eventPositionListView.Items[index].SubItems[1].Text = IntHelper.ToString(y);

            // 技術ツリー上のラベルを移動する
            _techTreePanelController.UpdateItem(item, position);

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
            TechEvent item = GetSelectedItem() as TechEvent;
            if (item == null)
            {
                return;
            }

            // 発明イベント座標リストに項目を追加する
            TechPosition position = new TechPosition { X = 0, Y = 0 };
            item.Positions.Add(position);

            Log.Info("[Tech] Added event position: ({0},{1}) [{2}]", position.X, position.Y, item);

            // 編集済みフラグを設定する
            TechGroup grp = GetSelectedGroup();
            grp.SetDirty();
            item.SetDirty();
            position.SetDirtyAll();

            // 発明イベント座標リストビューに項目を追加する
            ListViewItem li = new ListViewItem { Text = IntHelper.ToString(position.X) };
            li.SubItems.Add(IntHelper.ToString(position.Y));
            eventPositionListView.Items.Add(li);

            // 追加した項目を選択する
            eventPositionListView.Items[eventPositionListView.Items.Count - 1].Focused = true;
            eventPositionListView.Items[eventPositionListView.Items.Count - 1].Selected = true;

            // 編集項目を有効化する
            EnableEventPositionItems();

            // 技術ツリーにラベルを追加する
            _techTreePanelController.AddItem(item, position);
        }

        /// <summary>
        ///     発明イベント座標削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEventPositionRemoveButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            TechEvent item = GetSelectedItem() as TechEvent;
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

            Log.Info("[Tech] Removed event position: ({0},{1}) [{2}]", position.X, position.Y, item);

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
            _techTreePanelController.RemoveItem(item, position);
        }

        #endregion
    }

    /// <summary>
    ///     技術ツリーエディタのタブ
    /// </summary>
    internal enum TechEditorTab
    {
        Category, // カテゴリ
        Tech, // 技術
        Required, // 必要技術
        Component, // 小研究
        Effect, // 技術効果
        Label, // 技術ラベル
        Event // 技術イベント
    }
}