﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HoI2Editor.Controls;
using HoI2Editor.Dialogs;
using HoI2Editor.Models;
using HoI2Editor.Properties;
using HoI2Editor.Utilities;

namespace HoI2Editor.Forms
{
    /// <summary>
    ///     閣僚エディタのフォーム
    /// </summary>
    public partial class MinisterEditorForm : Form
    {
        #region 内部フィールド

        /// <summary>
        ///     絞り込み後の閣僚リスト
        /// </summary>
        private readonly List<Minister> _list = new List<Minister>();

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
            Tag,
            Id,
            Name,
            StartYear,
            EndYear,
            Position,
            Personality,
            Ideology
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
        ///     閣僚リストビューの列の数
        /// </summary>
        public const int MinisterListColumnCount = 8;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public MinisterEditorForm()
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
            // 閣僚リストを絞り込む
            NarrowMinisterList();

            // 閣僚リストをソートする
            SortMinisterList();

            // 閣僚リストの表示を更新する
            UpdateMinisterList();

            // 編集済みフラグがクリアされるため表示を更新する
            countryListBox.Refresh();
        }

        /// <summary>
        ///     データ保存後の処理
        /// </summary>
        public void OnFileSaved()
        {
            // 編集済みフラグがクリアされるため表示を更新する
            countryListBox.Refresh();
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
            // 閣僚リストビュー
            countryColumnHeader.Width = HoI2EditorController.Settings.MinisterEditor.ListColumnWidth[0];
            idColumnHeader.Width = HoI2EditorController.Settings.MinisterEditor.ListColumnWidth[1];
            nameColumnHeader.Width = HoI2EditorController.Settings.MinisterEditor.ListColumnWidth[2];
            startYearColumnHeader.Width = HoI2EditorController.Settings.MinisterEditor.ListColumnWidth[3];
            endYearColumnHeader.Width = HoI2EditorController.Settings.MinisterEditor.ListColumnWidth[4];
            positionColumnHeader.Width = HoI2EditorController.Settings.MinisterEditor.ListColumnWidth[5];
            personalityColumnHeader.Width = HoI2EditorController.Settings.MinisterEditor.ListColumnWidth[6];
            ideologyColumnHeader.Width = HoI2EditorController.Settings.MinisterEditor.ListColumnWidth[7];

            // 国家リストボックス
            countryListBox.ColumnWidth = DeviceCaps.GetScaledWidth(countryListBox.ColumnWidth);
            countryListBox.ItemHeight = DeviceCaps.GetScaledHeight(countryListBox.ItemHeight);

            // ウィンドウの位置
            Location = HoI2EditorController.Settings.MinisterEditor.Location;
            Size = HoI2EditorController.Settings.MinisterEditor.Size;
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormLoad(object sender, EventArgs e)
        {
            // 国家データを初期化する
            Countries.Init();

            // 閣僚特性を初期化する
            Ministers.InitPersonality();

            // ゲーム設定ファイルを読み込む
            Misc.Load();

            // 文字列定義ファイルを読み込む
            Config.Load();

            // 編集項目を初期化する
            InitEditableItems();

            // 国家リストボックスを初期化する
            InitCountryListBox();

            // 閣僚ファイルを読み込む
            Ministers.Load();

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
            if (!HoI2EditorController.IsDirty())
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
                    HoI2EditorController.Save();
                    break;
                case DialogResult.No:
                    HoI2EditorController.SaveCanceled = true;
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
            HoI2EditorController.OnMinisterEditorFormClosed();
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
                HoI2EditorController.Settings.MinisterEditor.Location = Location;
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
                HoI2EditorController.Settings.MinisterEditor.Size = Size;
            }
        }

        /// <summary>
        ///     一括編集ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBatchButtonClick(object sender, EventArgs e)
        {
            MinisterBatchEditArgs args = new MinisterBatchEditArgs();
            args.TargetCountries.AddRange(from string name in countryListBox.SelectedItems
                select Countries.StringMap[name]);

            // 一括編集ダイアログを表示する
            MinisterBatchDialog dialog = new MinisterBatchDialog(args);
            if (dialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            // 終了年が未設定ならばMiscの値を変更する
            if (args.Items[(int) MinisterBatchItemId.EndYear] && !Misc.UseNewMinisterFilesFormat)
            {
                Misc.UseNewMinisterFilesFormat = true;
                HoI2EditorController.OnItemChanged(EditorItemId.MinisterEndYear, this);
            }

            // 引退年が未設定ならばMiscの値を変更する
            if (args.Items[(int) MinisterBatchItemId.RetirementYear] && !Misc.EnableRetirementYearMinisters)
            {
                Misc.EnableRetirementYearMinisters = true;
                HoI2EditorController.OnItemChanged(EditorItemId.MinisterRetirementYear, this);
            }

            // 一括編集処理
            Ministers.BatchEdit(args);

            // 閣僚リストを更新する
            NarrowMinisterList();
            UpdateMinisterList();

            // 国家リストボックスの項目色を変更するため描画更新する
            countryListBox.Refresh();
        }

        /// <summary>
        ///     再読み込みボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReloadButtonClick(object sender, EventArgs e)
        {
            // 編集済みならば保存するかを問い合わせる
            if (HoI2EditorController.IsDirty())
            {
                DialogResult result = MessageBox.Show(Resources.ConfirmSaveMessage, Text, MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);
                switch (result)
                {
                    case DialogResult.Cancel:
                        return;
                    case DialogResult.Yes:
                        HoI2EditorController.Save();
                        break;
                }
            }

            HoI2EditorController.Reload();
        }

        /// <summary>
        ///     保存ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            HoI2EditorController.Save();
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

        #region 閣僚リストビュー

        /// <summary>
        ///     閣僚リストの表示を更新する
        /// </summary>
        private void UpdateMinisterList()
        {
            ministerListView.BeginUpdate();
            ministerListView.Items.Clear();

            // 項目を順に登録する
            foreach (Minister minister in _list)
            {
                ministerListView.Items.Add(CreateMinisterListViewItem(minister));
            }

            if (ministerListView.Items.Count > 0)
            {
                // 先頭の項目を選択する
                ministerListView.Items[0].Focused = true;
                ministerListView.Items[0].Selected = true;

                // 編集項目を有効化する
                EnableEditableItems();
            }
            else
            {
                // 編集項目を無効化する
                DisableEditableItems();
            }

            ministerListView.EndUpdate();
        }

        /// <summary>
        ///     閣僚リストを国タグで絞り込む
        /// </summary>
        private void NarrowMinisterList()
        {
            _list.Clear();

            // 選択中の国家リストを作成する
            List<Country> tags = (from string s in countryListBox.SelectedItems select Countries.StringMap[s]).ToList();

            // 選択中の国家に所属する指揮官を順に絞り込む
            _list.AddRange(Ministers.Items.Where(minister => tags.Contains(minister.Country)));
        }

        /// <summary>
        ///     閣僚リストをソートする
        /// </summary>
        private void SortMinisterList()
        {
            switch (_key)
            {
                case SortKey.None: // ソートなし
                    break;

                case SortKey.Tag: // 国タグ
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((minister1, minister2) => minister1.Country - minister2.Country);
                    }
                    else
                    {
                        _list.Sort((minister1, minister2) => minister2.Country - minister1.Country);
                    }
                    break;

                case SortKey.Id: // ID
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((minister1, minister2) => minister1.Id - minister2.Id);
                    }
                    else
                    {
                        _list.Sort((minister1, minister2) => minister2.Id - minister1.Id);
                    }
                    break;

                case SortKey.Name: // 名前
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((minister1, minister2) => string.CompareOrdinal(minister1.Name, minister2.Name));
                    }
                    else
                    {
                        _list.Sort((minister1, minister2) => string.CompareOrdinal(minister2.Name, minister1.Name));
                    }
                    break;

                case SortKey.StartYear: // 開始年
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((minister1, minister2) => minister1.StartYear - minister2.StartYear);
                    }
                    else
                    {
                        _list.Sort((minister1, minister2) => minister2.StartYear - minister1.StartYear);
                    }
                    break;

                case SortKey.EndYear: // 終了年
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((minister1, minister2) => minister1.EndYear - minister2.EndYear);
                    }
                    else
                    {
                        _list.Sort((minister1, minister2) => minister2.EndYear - minister1.EndYear);
                    }
                    break;

                case SortKey.Position: // 地位
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((minister1, minister2) => minister1.Position - minister2.Position);
                    }
                    else
                    {
                        _list.Sort((minister1, minister2) => minister2.Position - minister1.Position);
                    }
                    break;

                case SortKey.Personality: // 特性
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((minister1, minister2) => minister1.Personality - minister2.Personality);
                    }
                    else
                    {
                        _list.Sort((minister1, minister2) => minister2.Personality - minister1.Personality);
                    }
                    break;

                case SortKey.Ideology: // イデオロギー
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((minister1, minister2) => minister1.Ideology - minister2.Ideology);
                    }
                    else
                    {
                        _list.Sort((minister1, minister2) => minister2.Ideology - minister1.Ideology);
                    }
                    break;
            }
        }

        /// <summary>
        ///     閣僚リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 編集項目を更新する
            UpdateEditableItems();
        }

        /// <summary>
        ///     閣僚リストビューの項目編集前の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterListViewQueryItemEdit(object sender, QueryListViewItemEditEventArgs e)
        {
            switch (e.Column)
            {
                case 0: // 国タグ
                    e.Type = ItemEditType.List;
                    e.Items = countryComboBox.Items.Cast<string>();
                    e.Index = countryComboBox.SelectedIndex;
                    e.DropDownWidth = countryComboBox.DropDownWidth;
                    break;

                case 1: // ID
                    e.Type = ItemEditType.Text;
                    e.Text = idNumericUpDown.Text;
                    break;

                case 2: // 名前
                    e.Type = ItemEditType.Text;
                    e.Text = nameTextBox.Text;
                    break;

                case 3: // 開始年
                    e.Type = ItemEditType.Text;
                    e.Text = startYearNumericUpDown.Text;
                    break;

                case 4: // 終了年
                    e.Type = ItemEditType.Text;
                    e.Text = endYearNumericUpDown.Text;
                    break;

                case 5: // 地位
                    e.Type = ItemEditType.List;
                    e.Items = positionComboBox.Items.Cast<string>();
                    e.Index = positionComboBox.SelectedIndex;
                    e.DropDownWidth = positionComboBox.DropDownWidth;
                    break;

                case 6: // 特性
                    e.Type = ItemEditType.List;
                    e.Items = personalityComboBox.Items.Cast<string>();
                    e.Index = personalityComboBox.SelectedIndex;
                    e.DropDownWidth = personalityComboBox.DropDownWidth;
                    break;

                case 7: // イデオロギー
                    e.Type = ItemEditType.List;
                    e.Items = ideologyComboBox.Items.Cast<string>();
                    e.Index = ideologyComboBox.SelectedIndex;
                    e.DropDownWidth = ideologyComboBox.DropDownWidth;
                    break;
            }
        }

        /// <summary>
        ///     閣僚リストビューの項目編集後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterListViewBeforeItemEdit(object sender, ListViewItemEditEventArgs e)
        {
            switch (e.Column)
            {
                case 0: // 国タグ
                    countryComboBox.SelectedIndex = e.Index;
                    break;

                case 1: // ID
                    idNumericUpDown.Text = e.Text;
                    break;

                case 2: // 名前
                    nameTextBox.Text = e.Text;
                    break;

                case 3: // 開始年
                    startYearNumericUpDown.Text = e.Text;
                    break;

                case 4: // 終了年
                    endYearNumericUpDown.Text = e.Text;
                    break;

                case 5: // 地位
                    positionComboBox.SelectedIndex = e.Index;
                    break;

                case 6: // 特性
                    personalityComboBox.SelectedIndex = e.Index;
                    break;

                case 7: // イデオロギー
                    ideologyComboBox.SelectedIndex = e.Index;
                    break;
            }

            // 自前でリストビューの項目を更新するのでキャンセル扱いとする
            e.Cancel = true;
        }

        /// <summary>
        ///     閣僚リストビューの項目入れ替え時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterListViewItemReordered(object sender, ItemReorderedEventArgs e)
        {
            // 自前で項目を入れ替えるのでキャンセル扱いにする
            e.Cancel = true;

            int srcIndex = e.OldDisplayIndices[0];
            int destIndex = e.NewDisplayIndex;
            if (srcIndex < destIndex)
            {
                destIndex--;
            }

            Minister src = ministerListView.Items[srcIndex].Tag as Minister;
            if (src == null)
            {
                return;
            }
            Minister dest = ministerListView.Items[destIndex].Tag as Minister;
            if (dest == null)
            {
                return;
            }

            // 閣僚リストの項目を移動する
            Ministers.MoveItem(src, dest);
            MoveListItem(srcIndex, destIndex);

            // 編集済みフラグを設定する
            Ministers.SetDirty(src.Country);
        }

        /// <summary>
        ///     閣僚リストビューのカラムクリック時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterListViewColumnClick(object sender, ColumnClickEventArgs e)
        {
            switch (e.Column)
            {
                case 0: // 国タグ
                    if (_key == SortKey.Tag)
                    {
                        _order = _order == SortOrder.Ascendant ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.Tag;
                    }
                    break;

                case 1: // ID
                    if (_key == SortKey.Id)
                    {
                        _order = _order == SortOrder.Ascendant ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.Id;
                    }
                    break;

                case 2: // 名前
                    if (_key == SortKey.Name)
                    {
                        _order = _order == SortOrder.Ascendant ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.Name;
                    }
                    break;

                case 3: // 開始年
                    if (_key == SortKey.StartYear)
                    {
                        _order = _order == SortOrder.Ascendant ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.StartYear;
                    }
                    break;

                case 4: // 終了年
                    if (_key == SortKey.EndYear)
                    {
                        _order = _order == SortOrder.Ascendant ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.EndYear;
                    }
                    break;

                case 5: // 地位
                    if (_key == SortKey.Position)
                    {
                        _order = _order == SortOrder.Ascendant ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.Position;
                    }
                    break;

                case 6: // 特性
                    if (_key == SortKey.Personality)
                    {
                        _order = _order == SortOrder.Ascendant ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.Personality;
                    }
                    break;

                case 7: // イデオロギー
                    if (_key == SortKey.Ideology)
                    {
                        _order = _order == SortOrder.Ascendant ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.Ideology;
                    }
                    break;

                default:
                    // 項目のない列をクリックした時には何もしない
                    return;
            }

            // 閣僚リストをソートする
            SortMinisterList();

            // 閣僚リストを更新する
            UpdateMinisterList();
        }

        /// <summary>
        ///     閣僚リストビューの列の幅変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterListViewColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if ((e.ColumnIndex >= 0) && (e.ColumnIndex < MinisterListColumnCount))
            {
                HoI2EditorController.Settings.MinisterEditor.ListColumnWidth[e.ColumnIndex] =
                    ministerListView.Columns[e.ColumnIndex].Width;
            }
        }

        /// <summary>
        ///     新規ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNewButtonClick(object sender, EventArgs e)
        {
            Minister minister;
            Minister selected = GetSelectedMinister();
            if (selected != null)
            {
                // 選択項目がある場合、国タグやIDを引き継いで項目を作成する
                minister = new Minister(selected)
                {
                    Id = Ministers.GetNewId(selected.Country),
                    Name = "",
                    PictureName = ""
                };

                // 閣僚ごとの編集済みフラグを設定する
                minister.SetDirtyAll();

                // 閣僚リストに項目を挿入する
                Ministers.InsertItem(minister, selected);
                InsertListItem(minister, ministerListView.SelectedIndices[0] + 1);
            }
            else
            {
                Country country = Countries.Tags[countryListBox.SelectedIndex];
                // 新規項目を作成する
                minister = new Minister
                {
                    Country = country,
                    Id = Ministers.GetNewId(country),
                    StartYear = 1930,
                    EndYear = 1970,
                    RetirementYear = 1999,
                    Position = MinisterPosition.None,
                    Personality = 0,
                    Ideology = MinisterIdeology.None,
                    Loyalty = MinisterLoyalty.None
                };

                // 閣僚ごとの編集済みフラグを設定する
                minister.SetDirtyAll();

                // 閣僚リストに項目を追加する
                Ministers.AddItem(minister);
                AddListItem(minister);

                // 編集項目を有効化する
                EnableEditableItems();
            }

            // 国家ごとの編集済みフラグを設定する
            Ministers.SetDirty(minister.Country);

            // ファイル一覧に存在しなければ追加する
            if (!Ministers.FileNameMap.ContainsKey(minister.Country))
            {
                Ministers.FileNameMap.Add(minister.Country, Game.GetMinisterFileName(minister.Country));
                Ministers.SetDirtyList();
            }
        }

        /// <summary>
        ///     複製ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloneButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Minister selected = GetSelectedMinister();
            if (selected == null)
            {
                return;
            }

            // 選択項目を引き継いで項目を作成する
            Minister minister = new Minister(selected)
            {
                Id = Ministers.GetNewId(selected.Country)
            };

            // 閣僚ごとの編集済みフラグを設定する
            minister.SetDirtyAll();

            // 閣僚リストに項目を挿入する
            Ministers.InsertItem(minister, selected);
            InsertListItem(minister, ministerListView.SelectedIndices[0] + 1);

            // 国家ごとの編集済みフラグを設定する
            Ministers.SetDirty(minister.Country);
        }

        /// <summary>
        ///     削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRemoveButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Minister selected = GetSelectedMinister();
            if (selected == null)
            {
                return;
            }

            // 閣僚リストから項目を削除する
            Ministers.RemoveItem(selected);
            RemoveItem(ministerListView.SelectedIndices[0]);

            // リストから項目がなくなれば編集項目を無効化する
            if (ministerListView.Items.Count == 0)
            {
                DisableEditableItems();
            }

            // 編集済みフラグを設定する
            Ministers.SetDirty(selected.Country);
        }

        /// <summary>
        ///     先頭へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTopButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Minister selected = GetSelectedMinister();
            if (selected == null)
            {
                return;
            }

            // 選択項目がリストの先頭ならば何もしない
            int index = ministerListView.SelectedIndices[0];
            if (ministerListView.SelectedIndices[0] == 0)
            {
                return;
            }

            Minister top = ministerListView.Items[0].Tag as Minister;
            if (top == null)
            {
                return;
            }

            // 閣僚リストの項目を移動する
            Ministers.MoveItem(selected, top);
            MoveListItem(index, 0);

            // 編集済みフラグを設定する
            Ministers.SetDirty(selected.Country);
        }

        /// <summary>
        ///     上へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Minister selected = GetSelectedMinister();
            if (selected == null)
            {
                return;
            }

            // 選択項目がリストの先頭ならば何もしない
            int index = ministerListView.SelectedIndices[0];
            if (index == 0)
            {
                return;
            }

            Minister upper = ministerListView.Items[index - 1].Tag as Minister;
            if (upper == null)
            {
                return;
            }

            // 閣僚リストの項目を移動する
            Ministers.MoveItem(selected, upper);
            MoveListItem(index, index - 1);

            // 編集済みフラグを設定する
            Ministers.SetDirty(selected.Country);
        }

        /// <summary>
        ///     下へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDownButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Minister selected = GetSelectedMinister();
            if (selected == null)
            {
                return;
            }

            // 選択項目がリストの末尾ならば何もしない
            int index = ministerListView.SelectedIndices[0];
            if (index == ministerListView.Items.Count - 1)
            {
                return;
            }

            Minister lower = ministerListView.Items[index + 1].Tag as Minister;
            if (lower == null)
            {
                return;
            }

            // 閣僚リストの項目を移動する
            Ministers.MoveItem(selected, lower);
            MoveListItem(index, index + 1);

            // 編集済みフラグを設定する
            Ministers.SetDirty(selected.Country);
        }

        /// <summary>
        ///     末尾へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBottomButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Minister selected = GetSelectedMinister();
            if (selected == null)
            {
                return;
            }

            // 選択項目がリストの末尾ならば何もしない
            int index = ministerListView.SelectedIndices[0];
            int bottomIndex = ministerListView.Items.Count - 1;
            if (ministerListView.SelectedIndices[0] == bottomIndex)
            {
                return;
            }

            Minister bottom = ministerListView.Items[ministerListView.Items.Count - 1].Tag as Minister;
            if (bottom == null)
            {
                return;
            }

            // 閣僚リストの項目を移動する
            Ministers.MoveItem(selected, bottom);
            MoveListItem(index, ministerListView.Items.Count - 1);

            // 編集済みフラグを設定する
            Ministers.SetDirty(selected.Country);
        }

        /// <summary>
        ///     閣僚リストに項目を追加する
        /// </summary>
        /// <param name="minister">挿入対象の項目</param>
        private void AddListItem(Minister minister)
        {
            // 絞り込みリストに項目を追加する
            _list.Add(minister);

            // 閣僚リストビューに項目を追加する
            ministerListView.Items.Add(CreateMinisterListViewItem(minister));

            // 追加した項目を選択する
            ministerListView.Items[ministerListView.Items.Count - 1].Focused = true;
            ministerListView.Items[ministerListView.Items.Count - 1].Selected = true;
            ministerListView.EnsureVisible(ministerListView.Items.Count - 1);
        }

        /// <summary>
        ///     閣僚リストに項目を挿入する
        /// </summary>
        /// <param name="minister">挿入対象の項目</param>
        /// <param name="index">挿入先の位置</param>
        private void InsertListItem(Minister minister, int index)
        {
            // 絞り込みリストに項目を挿入する
            _list.Insert(index, minister);

            // 閣僚リストビューに項目を挿入する
            ministerListView.Items.Insert(index, CreateMinisterListViewItem(minister));

            // 挿入した項目を選択する
            ministerListView.Items[index].Focused = true;
            ministerListView.Items[index].Selected = true;
            ministerListView.EnsureVisible(index);
        }

        /// <summary>
        ///     閣僚リストから項目を削除する
        /// </summary>
        /// <param name="index">削除対象の位置</param>
        private void RemoveItem(int index)
        {
            // 絞り込みリストから項目を削除する
            _list.RemoveAt(index);

            // 閣僚リストビューから項目を削除する
            ministerListView.Items.RemoveAt(index);

            // 削除した項目の次の項目を選択する
            if (index < ministerListView.Items.Count)
            {
                ministerListView.Items[index].Focused = true;
                ministerListView.Items[index].Selected = true;
            }
            else if (index - 1 >= 0)
            {
                // リストの末尾ならば、削除した項目の前の項目を選択する
                ministerListView.Items[index - 1].Focused = true;
                ministerListView.Items[index - 1].Selected = true;
            }
        }

        /// <summary>
        ///     閣僚リストの項目を移動する
        /// </summary>
        /// <param name="src">移動元の位置</param>
        /// <param name="dest">移動先の位置</param>
        private void MoveListItem(int src, int dest)
        {
            Minister minister = _list[src];

            if (src > dest)
            {
                // 上へ移動する場合
                // 絞り込みリストの項目を移動する
                _list.Insert(dest, minister);
                _list.RemoveAt(src + 1);

                // 閣僚リストビューの項目を移動する
                ministerListView.Items.Insert(dest, CreateMinisterListViewItem(minister));
                ministerListView.Items.RemoveAt(src + 1);
            }
            else
            {
                // 下へ移動する場合
                // 絞り込みリストの項目を移動する
                _list.Insert(dest + 1, minister);
                _list.RemoveAt(src);

                // 閣僚リストビューの項目を移動する
                ministerListView.Items.Insert(dest + 1, CreateMinisterListViewItem(minister));
                ministerListView.Items.RemoveAt(src);
            }

            // 移動先の項目を選択する
            ministerListView.Items[dest].Focused = true;
            ministerListView.Items[dest].Selected = true;
            ministerListView.EnsureVisible(dest);
        }

        /// <summary>
        ///     閣僚リストビューの項目を作成する
        /// </summary>
        /// <param name="minister">閣僚データ</param>
        /// <returns>閣僚リストビューの項目</returns>
        private static ListViewItem CreateMinisterListViewItem(Minister minister)
        {
            if (minister == null)
            {
                return null;
            }

            ListViewItem item = new ListViewItem
            {
                Text = Countries.Strings[(int) minister.Country],
                Tag = minister
            };
            item.SubItems.Add(IntHelper.ToString(minister.Id));
            item.SubItems.Add(minister.Name);
            item.SubItems.Add(IntHelper.ToString(minister.StartYear));
            item.SubItems.Add(Misc.UseNewMinisterFilesFormat ? IntHelper.ToString(minister.EndYear) : "");
            item.SubItems.Add(Config.GetText(Ministers.PositionNames[(int) minister.Position]));
            item.SubItems.Add(Ministers.Personalities[minister.Personality].NameText);
            item.SubItems.Add(Config.GetText(Ministers.IdeologyNames[(int) minister.Ideology]));

            return item;
        }

        /// <summary>
        ///     選択中の閣僚データを取得する
        /// </summary>
        /// <returns>選択中の閣僚データ</returns>
        private Minister GetSelectedMinister()
        {
            // 選択項目がない場合
            if (ministerListView.SelectedItems.Count == 0)
            {
                return null;
            }

            return ministerListView.SelectedItems[0].Tag as Minister;
        }

        #endregion

        #region 国家リストボックス

        /// <summary>
        ///     国家リストボックスを初期化する
        /// </summary>
        private void InitCountryListBox()
        {
            countryListBox.BeginUpdate();
            countryListBox.Items.Clear();
            foreach (Country country in Countries.Tags)
            {
                countryListBox.Items.Add(Countries.Strings[(int) country]);
            }

            // 選択イベントを処理すると時間がかかるので、一時的に無効化する
            countryListBox.SelectedIndexChanged -= OnCountryListBoxSelectedIndexChanged;
            // 選択中の国家を反映する
            foreach (Country country in HoI2EditorController.Settings.MinisterEditor.Countries)
            {
                int index = Array.IndexOf(Countries.Tags, country);
                if (index >= 0)
                {
                    countryListBox.SetSelected(Array.IndexOf(Countries.Tags, country), true);
                }
            }
            // 選択イベントを元に戻す
            countryListBox.SelectedIndexChanged += OnCountryListBoxSelectedIndexChanged;

            int count = countryListBox.SelectedItems.Count;
            // 選択数に合わせて全選択/全解除を切り替える
            countryAllButton.Text = count <= 1 ? Resources.KeySelectAll : Resources.KeyUnselectAll;
            // 選択数がゼロの場合は新規追加ボタンを無効化する
            newButton.Enabled = count > 0;

            countryListBox.EndUpdate();
        }

        /// <summary>
        ///     国家リストボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryListBoxDrawItem(object sender, DrawItemEventArgs e)
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
                Country country = Countries.Tags[e.Index];
                brush = Ministers.IsDirty(country) ? new SolidBrush(Color.Red) : new SolidBrush(SystemColors.WindowText);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            string s = countryListBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     国家リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            int count = countryListBox.SelectedItems.Count;

            // 選択数に合わせて全選択/全解除を切り替える
            countryAllButton.Text = count <= 1 ? Resources.KeySelectAll : Resources.KeyUnselectAll;

            // 選択数がゼロの場合は新規追加ボタンを無効化する
            newButton.Enabled = count > 0;

            // 選択中の国家を保存する
            HoI2EditorController.Settings.MinisterEditor.Countries =
                countryListBox.SelectedIndices.Cast<int>().Select(index => Countries.Tags[index]).ToList();

            // 閣僚リストを更新する
            NarrowMinisterList();
            UpdateMinisterList();
        }

        /// <summary>
        ///     国家リストボックスの全選択/全解除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryAllButtonClick(object sender, EventArgs e)
        {
            countryListBox.BeginUpdate();

            // 選択イベントを処理すると時間がかかるので、一時的に無効化する
            countryListBox.SelectedIndexChanged -= OnCountryListBoxSelectedIndexChanged;

            if (countryListBox.SelectedItems.Count <= 1)
            {
                // スクロール位置を先頭に設定するため、逆順で選択する
                for (int i = countryListBox.Items.Count - 1; i >= 0; i--)
                {
                    countryListBox.SetSelected(i, true);
                }
            }
            else
            {
                for (int i = 0; i < countryListBox.Items.Count; i++)
                {
                    countryListBox.SetSelected(i, false);
                }
            }

            // 選択イベントを元に戻す
            countryListBox.SelectedIndexChanged += OnCountryListBoxSelectedIndexChanged;

            // 閣僚リスト絞り込みのため、ダミーでイベント発行する
            OnCountryListBoxSelectedIndexChanged(sender, e);

            countryListBox.EndUpdate();
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

            // 国タグ
            countryComboBox.BeginUpdate();
            countryComboBox.Items.Clear();
            int width = countryComboBox.Width;
            foreach (string s in Countries.Tags
                .Select(country => Countries.Strings[(int) country])
                .Select(name => Config.ExistsKey(name)
                    ? $"{name} {Config.GetText(name)}"
                    : name))
            {
                countryComboBox.Items.Add(s);
                width = Math.Max(width,
                    (int) g.MeasureString(s, countryComboBox.Font).Width + SystemInformation.VerticalScrollBarWidth +
                    margin);
            }
            countryComboBox.DropDownWidth = width;
            countryComboBox.EndUpdate();

            // 地位
            positionComboBox.BeginUpdate();
            positionComboBox.Items.Clear();
            width = positionComboBox.Width;
            foreach (string s in Ministers.PositionNames.Where(id => id != TextId.Empty).Select(Config.GetText))
            {
                positionComboBox.Items.Add(s);
                width = Math.Max(width, (int) g.MeasureString(s, positionComboBox.Font).Width + margin);
            }
            positionComboBox.DropDownWidth = width;
            positionComboBox.EndUpdate();

            // 特性
            personalityComboBox.DropDownWidth =
                Ministers.Personalities
                    .Select(info => (int) g.MeasureString(info.NameText, personalityComboBox.Font).Width +
                                    SystemInformation.VerticalScrollBarWidth + margin)
                    .Concat(new[] { personalityComboBox.Width })
                    .Max();

            // イデオロギー
            ideologyComboBox.BeginUpdate();
            ideologyComboBox.Items.Clear();
            width = ideologyComboBox.Width;
            foreach (string s in Ministers.IdeologyNames.Where(id => id != TextId.Empty).Select(Config.GetText))
            {
                ideologyComboBox.Items.Add(s);
                width = Math.Max(width, (int) g.MeasureString(s, ideologyComboBox.Font).Width + margin);
            }
            ideologyComboBox.DropDownWidth = width;
            ideologyComboBox.EndUpdate();

            // 忠誠度
            loyaltyComboBox.BeginUpdate();
            loyaltyComboBox.Items.Clear();
            width = loyaltyComboBox.Width;
            foreach (string s in Ministers.LoyaltyNames.Where(name => !string.IsNullOrEmpty(name)))
            {
                loyaltyComboBox.Items.Add(s);
                width = Math.Max(width, (int) g.MeasureString(s, loyaltyComboBox.Font).Width + margin);
            }
            loyaltyComboBox.DropDownWidth = width;
            loyaltyComboBox.EndUpdate();
        }

        /// <summary>
        ///     編集項目を更新する
        /// </summary>
        private void UpdateEditableItems()
        {
            // 選択項目がなければ何もしない
            Minister minister = GetSelectedMinister();
            if (minister == null)
            {
                return;
            }

            // 編集項目を更新する
            UpdateEditableItemsValue(minister);

            // 編集項目の色を更新する
            UpdateEditableItemsColor(minister);

            // 項目移動ボタンの状態更新
            topButton.Enabled = ministerListView.SelectedIndices[0] != 0;
            upButton.Enabled = ministerListView.SelectedIndices[0] != 0;
            downButton.Enabled = ministerListView.SelectedIndices[0] != ministerListView.Items.Count - 1;
            bottomButton.Enabled = ministerListView.SelectedIndices[0] != ministerListView.Items.Count - 1;
        }

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="minister">閣僚データ</param>
        private void UpdateEditableItemsValue(Minister minister)
        {
            countryComboBox.SelectedIndex = minister.Country != Country.None ? (int) minister.Country - 1 : -1;
            idNumericUpDown.Value = minister.Id;
            nameTextBox.Text = minister.Name;
            startYearNumericUpDown.Value = minister.StartYear;
            if (Misc.UseNewMinisterFilesFormat)
            {
                endYearLabel.Enabled = true;
                endYearNumericUpDown.Enabled = true;
                endYearNumericUpDown.Value = minister.EndYear;
                endYearNumericUpDown.Text = IntHelper.ToString((int) endYearNumericUpDown.Value);
            }
            else
            {
                endYearLabel.Enabled = false;
                endYearNumericUpDown.Enabled = false;
                endYearNumericUpDown.ResetText();
            }
            if (Misc.EnableRetirementYearMinisters)
            {
                retirementYearLabel.Enabled = true;
                retirementYearNumericUpDown.Enabled = true;
                retirementYearNumericUpDown.Value = minister.RetirementYear;
                retirementYearNumericUpDown.Text = IntHelper.ToString((int) retirementYearNumericUpDown.Value);
            }
            else
            {
                retirementYearLabel.Enabled = false;
                retirementYearNumericUpDown.Enabled = false;
                retirementYearNumericUpDown.ResetText();
            }
            positionComboBox.SelectedIndex = minister.Position != MinisterPosition.None
                ? (int) minister.Position - 1
                : -1;
            UpdatePersonalityComboBox(minister);
            ideologyComboBox.SelectedIndex = minister.Ideology != MinisterIdeology.None
                ? (int) minister.Ideology - 1
                : -1;
            loyaltyComboBox.SelectedIndex = minister.Loyalty != MinisterLoyalty.None ? (int) minister.Loyalty - 1 : -1;
            pictureNameTextBox.Text = minister.PictureName;
            UpdateMinisterPicture(minister);
        }

        /// <summary>
        ///     編集項目の色を更新する
        /// </summary>
        /// <param name="minister">閣僚データ</param>
        private void UpdateEditableItemsColor(Minister minister)
        {
            // コンボボックスの色を更新する
            countryComboBox.Refresh();
            positionComboBox.Refresh();
            personalityComboBox.Refresh();
            ideologyComboBox.Refresh();
            loyaltyComboBox.Refresh();

            // 編集項目の色を更新する
            idNumericUpDown.ForeColor = minister.IsDirty(MinisterItemId.Id) ? Color.Red : SystemColors.WindowText;
            nameTextBox.ForeColor = minister.IsDirty(MinisterItemId.Name) ? Color.Red : SystemColors.WindowText;
            startYearNumericUpDown.ForeColor = minister.IsDirty(MinisterItemId.StartYear)
                ? Color.Red
                : SystemColors.WindowText;
            endYearNumericUpDown.ForeColor = minister.IsDirty(MinisterItemId.EndYear)
                ? Color.Red
                : SystemColors.WindowText;
            retirementYearNumericUpDown.ForeColor = minister.IsDirty(MinisterItemId.RetirementYear)
                ? Color.Red
                : SystemColors.WindowText;
            pictureNameTextBox.ForeColor = minister.IsDirty(MinisterItemId.PictureName)
                ? Color.Red
                : SystemColors.WindowText;
        }

        /// <summary>
        ///     編集項目を有効化する
        /// </summary>
        private void EnableEditableItems()
        {
            countryComboBox.Enabled = true;
            idNumericUpDown.Enabled = true;
            nameTextBox.Enabled = true;
            startYearNumericUpDown.Enabled = true;
            positionComboBox.Enabled = true;
            personalityComboBox.Enabled = true;
            ideologyComboBox.Enabled = true;
            loyaltyComboBox.Enabled = true;
            pictureNameTextBox.Enabled = true;
            pictureNameBrowseButton.Enabled = true;

            // 無効化時にクリアした文字列を再設定する
            idNumericUpDown.Text = IntHelper.ToString((int) idNumericUpDown.Value);
            startYearNumericUpDown.Text = IntHelper.ToString((int) startYearNumericUpDown.Value);

            if (Misc.UseNewMinisterFilesFormat)
            {
                endYearNumericUpDown.Enabled = true;
                endYearNumericUpDown.Text = IntHelper.ToString((int) endYearNumericUpDown.Value);
            }
            if (Misc.EnableRetirementYearMinisters)
            {
                retirementYearNumericUpDown.Enabled = true;
                retirementYearNumericUpDown.Text = IntHelper.ToString((int) retirementYearNumericUpDown.Value);
            }

            cloneButton.Enabled = true;
            removeButton.Enabled = true;
        }

        /// <summary>
        ///     編集項目を無効化する
        /// </summary>
        private void DisableEditableItems()
        {
            countryComboBox.SelectedIndex = -1;
            countryComboBox.ResetText();
            idNumericUpDown.ResetText();
            nameTextBox.ResetText();
            startYearNumericUpDown.ResetText();
            endYearNumericUpDown.ResetText();
            retirementYearNumericUpDown.ResetText();
            pictureNameTextBox.ResetText();
            ministerPictureBox.ImageLocation = "";

            countryComboBox.Enabled = false;
            idNumericUpDown.Enabled = false;
            nameTextBox.Enabled = false;
            startYearNumericUpDown.Enabled = false;
            endYearNumericUpDown.Enabled = false;
            retirementYearNumericUpDown.Enabled = false;
            positionComboBox.Enabled = false;
            personalityComboBox.Enabled = false;
            ideologyComboBox.Enabled = false;
            loyaltyComboBox.Enabled = false;
            pictureNameTextBox.Enabled = false;
            pictureNameBrowseButton.Enabled = false;

            cloneButton.Enabled = false;
            removeButton.Enabled = false;
            topButton.Enabled = false;
            upButton.Enabled = false;
            downButton.Enabled = false;
            bottomButton.Enabled = false;
        }

        /// <summary>
        ///     閣僚特性コンボボックスの項目を更新する
        /// </summary>
        /// <param name="minister">閣僚データ</param>
        private void UpdatePersonalityComboBox(Minister minister)
        {
            personalityComboBox.BeginUpdate();
            personalityComboBox.Items.Clear();
            if (minister.Position == MinisterPosition.None)
            {
                // 閣僚地位の値が不正な場合は、現在の閣僚特性のみ登録する
                personalityComboBox.Items.Add(Ministers.Personalities[minister.Personality].NameText);
                personalityComboBox.SelectedIndex = 0;
            }
            else if (!Ministers.PositionPersonalityTable[(int) minister.Position].Contains(minister.Personality))
            {
                // 閣僚特性が閣僚地位とマッチしない場合、ワンショットで候補に登録する
                personalityComboBox.Items.Add(Ministers.Personalities[minister.Personality].NameText);
                personalityComboBox.SelectedIndex = 0;

                // 閣僚地位と対応する閣僚特性を順に登録する
                foreach (int personality in Ministers.PositionPersonalityTable[(int) minister.Position])
                {
                    personalityComboBox.Items.Add(Ministers.Personalities[personality].NameText);
                }
            }
            else
            {
                // 閣僚地位と対応する閣僚特性を順に登録する
                foreach (int personality in Ministers.PositionPersonalityTable[(int) minister.Position])
                {
                    personalityComboBox.Items.Add(Ministers.Personalities[personality].NameText);
                    if (personality == minister.Personality)
                    {
                        personalityComboBox.SelectedIndex = personalityComboBox.Items.Count - 1;
                    }
                }
            }
            personalityComboBox.EndUpdate();
        }

        /// <summary>
        ///     国家コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Minister minister = GetSelectedMinister();
            if (minister != null)
            {
                Brush brush;
                if ((Countries.Tags[e.Index] == minister.Country) && minister.IsDirty(MinisterItemId.Country))
                {
                    brush = new SolidBrush(Color.Red);
                }
                else
                {
                    brush = new SolidBrush(SystemColors.WindowText);
                }
                string s = countryComboBox.Items[e.Index].ToString();
                e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     閣僚地位コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPositionComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Minister minister = GetSelectedMinister();
            if (minister != null)
            {
                Brush brush;
                if ((e.Index == (int) minister.Position - 1) && minister.IsDirty(MinisterItemId.Position))
                {
                    brush = new SolidBrush(Color.Red);
                }
                else
                {
                    brush = new SolidBrush(SystemColors.WindowText);
                }
                string s = positionComboBox.Items[e.Index].ToString();
                e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     閣僚特性コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPersonalityComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Minister minister = GetSelectedMinister();
            if (minister != null)
            {
                Brush brush;
                if ((minister.Position == MinisterPosition.None) ||
                    !Ministers.PositionPersonalityTable[(int) minister.Position].Contains(minister.Personality))
                {
                    // 閣僚地位の値が不正な場合は、現在の閣僚特性のみ登録されている
                    // 閣僚地位とマッチしない閣僚特性の場合、現在の閣僚特性が先頭に登録されている
                    if ((e.Index == 0) && minister.IsDirty(MinisterItemId.Personality))
                    {
                        brush = new SolidBrush(Color.Red);
                    }
                    else
                    {
                        brush = new SolidBrush(SystemColors.WindowText);
                    }
                }
                else
                {
                    if ((Ministers.PositionPersonalityTable[(int) minister.Position][e.Index] ==
                         minister.Personality) &&
                        minister.IsDirty(MinisterItemId.Personality))
                    {
                        brush = new SolidBrush(Color.Red);
                    }
                    else
                    {
                        brush = new SolidBrush(SystemColors.WindowText);
                    }
                }
                string s = personalityComboBox.Items[e.Index].ToString();
                e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     イデオロギーコンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIdeologyComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Minister minister = GetSelectedMinister();
            if (minister != null)
            {
                Brush brush;
                if ((e.Index == (int) minister.Ideology - 1) && minister.IsDirty(MinisterItemId.Ideology))
                {
                    brush = new SolidBrush(Color.Red);
                }
                else
                {
                    brush = new SolidBrush(SystemColors.WindowText);
                }
                string s = ideologyComboBox.Items[e.Index].ToString();
                e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     忠誠度コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoyaltyComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Minister minister = GetSelectedMinister();
            if (minister != null)
            {
                Brush brush;
                if ((e.Index == (int) minister.Loyalty - 1) && minister.IsDirty(MinisterItemId.Loyalty))
                {
                    brush = new SolidBrush(Color.Red);
                }
                else
                {
                    brush = new SolidBrush(SystemColors.WindowText);
                }
                string s = loyaltyComboBox.Items[e.Index].ToString();
                e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     閣僚画像ピクチャーボックスの項目を更新する
        /// </summary>
        /// <param name="minister">閣僚データ</param>
        private void UpdateMinisterPicture(Minister minister)
        {
            if (!string.IsNullOrEmpty(minister.PictureName) &&
                (minister.PictureName.IndexOfAny(Path.GetInvalidPathChars()) < 0))
            {
                string fileName = Game.GetReadFileName(Game.PersonPicturePathName,
                    Path.ChangeExtension(minister.PictureName, ".bmp"));
                ministerPictureBox.ImageLocation = File.Exists(fileName) ? fileName : "";
            }
            else
            {
                ministerPictureBox.ImageLocation = "";
            }
        }

        /// <summary>
        ///     国タグ変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Minister minister = GetSelectedMinister();
            if (minister == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            Country country = Countries.Tags[countryComboBox.SelectedIndex];
            if (country == minister.Country)
            {
                return;
            }

            // 変更前の国タグの編集済みフラグを設定する
            Ministers.SetDirty(minister.Country);

            Log.Info("[Minister] country: {0} -> {1} ({2}: {3})", Countries.Strings[(int) minister.Country],
                Countries.Strings[(int) country], minister.Id, minister.Name);

            // 値を更新する
            minister.Country = country;

            // 閣僚リストビューの項目を更新する
            ministerListView.SelectedItems[0].Text = Countries.Strings[(int) minister.Country];

            // 閣僚ごとの編集済みフラグを設定する
            minister.SetDirty(MinisterItemId.Country);

            // 変更後の国タグの編集済みフラグを設定する
            Ministers.SetDirty(minister.Country);

            // ファイル一覧に存在しなければ追加する
            if (!Ministers.FileNameMap.ContainsKey(minister.Country))
            {
                Ministers.FileNameMap.Add(minister.Country, Game.GetMinisterFileName(minister.Country));
                Ministers.SetDirtyList();
            }

            // 国家コンボボックスの項目色を変更するため描画更新する
            countryComboBox.Refresh();

            // 国家リストボックスの項目色を変更するため描画更新する
            countryListBox.Refresh();
        }

        /// <summary>
        ///     ID変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIdNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Minister minister = GetSelectedMinister();
            if (minister == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            int id = (int) idNumericUpDown.Value;
            if (id == minister.Id)
            {
                return;
            }

            Log.Info("[Minister] id: {0} -> {1} ({2})", minister.Id, id, minister.Name);

            // 値を更新する
            minister.Id = id;

            // 閣僚リストビューの項目を更新する
            ministerListView.SelectedItems[0].SubItems[1].Text = IntHelper.ToString(minister.Id);

            // 編集済みフラグを設定する
            minister.SetDirty(MinisterItemId.Id);
            Ministers.SetDirty(minister.Country);

            // 文字色を変更する
            idNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     名前文字列変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNameTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Minister minister = GetSelectedMinister();
            if (minister == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            string name = nameTextBox.Text;
            if (string.IsNullOrEmpty(name))
            {
                if (string.IsNullOrEmpty(minister.Name))
                {
                    return;
                }
            }
            else
            {
                if (name.Equals(minister.Name))
                {
                    return;
                }
            }

            Log.Info("[Minister] name: {0} -> {1} ({2})", minister.Name, name, minister.Id);

            // 値を更新する
            minister.Name = name;

            // 閣僚リストビューの項目を更新する
            ministerListView.SelectedItems[0].SubItems[2].Text = minister.Name;

            // 編集済みフラグを設定する
            minister.SetDirty(MinisterItemId.Name);
            Ministers.SetDirty(minister.Country);

            // 文字色を変更する
            nameTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     開始年変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStartYearNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Minister minister = GetSelectedMinister();
            if (minister == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            int startYear = (int) startYearNumericUpDown.Value;
            if (startYear == minister.StartYear)
            {
                return;
            }

            Log.Info("[Minister] start year: {0} -> {1} ({2}: {3})", minister.StartYear, startYear, minister.Id,
                minister.Name);

            // 値を更新する
            minister.StartYear = startYear;

            // 閣僚リストビューの項目を更新する
            ministerListView.SelectedItems[0].SubItems[3].Text = IntHelper.ToString(minister.StartYear);

            // 編集済みフラグを設定する
            minister.SetDirty(MinisterItemId.StartYear);
            Ministers.SetDirty(minister.Country);

            // 文字色を変更する
            startYearNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     終了年変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEndYearNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Minister minister = GetSelectedMinister();
            if (minister == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            int endYear = (int) endYearNumericUpDown.Value;
            if (endYear == minister.EndYear)
            {
                return;
            }

            Log.Info("[Minister] end year: {0} -> {1} ({2}: {3})", minister.EndYear, endYear, minister.Id, minister.Name);

            // 値を更新する
            minister.EndYear = endYear;

            // 閣僚リストビューの項目を更新する
            ministerListView.SelectedItems[0].SubItems[4].Text = IntHelper.ToString(minister.EndYear);

            // 編集済みフラグを設定する
            minister.SetDirty(MinisterItemId.EndYear);
            Ministers.SetDirty(minister.Country);

            // 文字色を変更する
            endYearNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     引退年変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRetirementYearNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Minister minister = GetSelectedMinister();
            if (minister == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            int retirementYear = (int) retirementYearNumericUpDown.Value;
            if (retirementYear == minister.RetirementYear)
            {
                return;
            }

            Log.Info("[Minister] retirement year: {0} -> {1} ({2}: {3})", minister.RetirementYear, retirementYear,
                minister.Id, minister.Name);

            // 値を更新する
            minister.RetirementYear = retirementYear;

            // 編集済みフラグを設定する
            minister.SetDirty(MinisterItemId.RetirementYear);
            Ministers.SetDirty(minister.Country);

            // 文字色を変更する
            retirementYearNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     閣僚地位変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPositionComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Minister minister = GetSelectedMinister();
            if (minister == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            MinisterPosition position = (MinisterPosition) (positionComboBox.SelectedIndex + 1);
            if (position == minister.Position)
            {
                return;
            }

            Log.Info("[Minister] position: {0} -> {1} ({2}: {3})",
                Config.GetText(Ministers.PositionNames[(int) minister.Position]),
                Config.GetText(Ministers.PositionNames[(int) position]), minister.Id, minister.Name);

            // 値を更新する
            minister.Position = position;

            // 閣僚リストビューの項目を更新する
            ministerListView.SelectedItems[0].SubItems[5].Text =
                Config.GetText(Ministers.PositionNames[(int) minister.Position]);

            // 地位に連動して特性の選択肢も変更する
            UpdatePersonalityComboBox(minister);

            // 編集済みフラグを設定する
            minister.SetDirty(MinisterItemId.Position);
            Ministers.SetDirty(minister.Country);

            // 閣僚地位コンボボックスの項目色を変更するため描画更新する
            positionComboBox.Refresh();
        }

        /// <summary>
        ///     閣僚特性変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPersonalityComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Minister minister = GetSelectedMinister();
            if (minister == null)
            {
                return;
            }

            // 閣僚地位が不定値の時には変更不可
            if (minister.Position == MinisterPosition.None)
            {
                return;
            }

            // 値に変化がなければ何もしない
            int personality;
            if (Ministers.PositionPersonalityTable[(int) minister.Position].Contains(minister.Personality))
            {
                personality =
                    Ministers.PositionPersonalityTable[(int) minister.Position][personalityComboBox.SelectedIndex];
            }
            else
            {
                if (personalityComboBox.SelectedIndex == 0)
                {
                    return;
                }
                personality =
                    Ministers.PositionPersonalityTable[(int) minister.Position][personalityComboBox.SelectedIndex - 1];
            }
            if (personality == minister.Personality)
            {
                return;
            }

            Log.Info("[Minister] personality: {0} -> {1} ({2}: {3})",
                Ministers.Personalities[minister.Personality].NameText, Ministers.Personalities[personality].NameText,
                minister.Id, minister.Name);

            // 値を更新する
            minister.Personality = personality;

            // 閣僚リストビューの項目を更新する
            ministerListView.SelectedItems[0].SubItems[6].Text = Ministers.Personalities[minister.Personality].NameText;

            // 編集済みフラグを設定する
            minister.SetDirty(MinisterItemId.Personality);
            Ministers.SetDirty(minister.Country);

            // 閣僚コンボボックスの項目を更新する
            UpdatePersonalityComboBox(minister);
        }

        /// <summary>
        ///     イデオロギー変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIdeologyComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Minister minister = GetSelectedMinister();
            if (minister == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            MinisterIdeology ideology = !string.IsNullOrEmpty(ideologyComboBox.Items[0].ToString())
                ? (MinisterIdeology) (ideologyComboBox.SelectedIndex + 1)
                : (MinisterIdeology) ideologyComboBox.SelectedIndex;
            if (ideology == minister.Ideology)
            {
                return;
            }

            Log.Info("[Minister] ideology: {0} -> {1} ({2}: {3})",
                Config.GetText(Ministers.IdeologyNames[(int) minister.Ideology]),
                Config.GetText(Ministers.IdeologyNames[(int) ideology]), minister.Id, minister.Name);

            // 値を更新する
            minister.Ideology = ideology;

            // 閣僚リストビューの項目を更新する
            ministerListView.SelectedItems[0].SubItems[7].Text =
                Config.GetText(Ministers.IdeologyNames[(int) minister.Ideology]);

            // 編集済みフラグを設定する
            minister.SetDirty(MinisterItemId.Ideology);
            Ministers.SetDirty(minister.Country);

            // イデオロギーコンボボックスの項目色を変更するため描画更新する
            ideologyComboBox.Refresh();
        }

        /// <summary>
        ///     忠誠度変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoyaltyComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Minister minister = GetSelectedMinister();
            if (minister == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            MinisterLoyalty loyalty = !string.IsNullOrEmpty(loyaltyComboBox.Items[0].ToString())
                ? (MinisterLoyalty) (loyaltyComboBox.SelectedIndex + 1)
                : (MinisterLoyalty) loyaltyComboBox.SelectedIndex;
            if (loyalty == minister.Loyalty)
            {
                return;
            }

            Log.Info("[Minister] loyalty: {0} -> {1} ({2}: {3})", Ministers.LoyaltyNames[(int) minister.Loyalty],
                Ministers.LoyaltyNames[(int) loyalty], minister.Id, minister.Name);

            // 値を更新する
            minister.Loyalty = loyalty;

            // 編集済みフラグを設定する
            minister.SetDirty(MinisterItemId.Loyalty);
            Ministers.SetDirty(minister.Country);

            // 忠誠度コンボボックスの項目色を変更するため描画更新する
            loyaltyComboBox.Refresh();
        }

        /// <summary>
        ///     画像ファイル名変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPictureNameTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Minister minister = GetSelectedMinister();
            if (minister == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            string pictureName = pictureNameTextBox.Text;
            if (string.IsNullOrEmpty(pictureName))
            {
                if (string.IsNullOrEmpty(minister.PictureName))
                {
                    return;
                }
            }
            else
            {
                if (pictureName.Equals(minister.PictureName))
                {
                    return;
                }
            }

            Log.Info("[Minister] picture name: {0} -> {1} ({2}: {3})", minister.PictureName, pictureName, minister.Id,
                minister.Name);

            // 値を更新する
            minister.PictureName = pictureName;

            // 閣僚画像を更新する
            UpdateMinisterPicture(minister);

            // 編集済みフラグを設定する
            minister.SetDirty(MinisterItemId.PictureName);
            Ministers.SetDirty(minister.Country);

            // 文字色を設定する
            pictureNameTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     画像ファイル名参照ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPictureNameReferButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Minister minister = GetSelectedMinister();
            if (minister == null)
            {
                return;
            }

            // ファイル選択ダイアログを開く
            OpenFileDialog dialog = new OpenFileDialog
            {
                InitialDirectory = Path.Combine(Game.FolderName, Game.PersonPicturePathName),
                FileName = minister.PictureName,
                Filter = Resources.OpenBitmapFileDialogFilter
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                pictureNameTextBox.Text = Path.GetFileNameWithoutExtension(dialog.FileName);
            }
        }

        #endregion
    }
}