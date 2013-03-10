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
    ///     閣僚エディタのフォーム
    /// </summary>
    public partial class MinisterEditorForm : Form
    {
        #region フィールド

        /// <summary>
        ///     絞り込み後の閣僚リスト
        /// </summary>
        private readonly List<Minister> _narrowedList = new List<Minister>();

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public MinisterEditorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterEditorFormLoad(object sender, EventArgs e)
        {
            // 国家データを初期化する
            Country.Init();

            // 閣僚特性を初期化する
            Ministers.InitPersonality();

            // 編集項目を初期化する
            InitEditableItems();

            // 国家リストボックスを初期化する
            InitCountryListBox();

            // 閣僚ファイルを読み込む
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

        #region 閣僚データ処理

        /// <summary>
        ///     再読み込みボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReloadButtonClick(object sender, EventArgs e)
        {
            // 閣僚ファイルの再読み込みを要求する
            Ministers.RequireReload();

            // 閣僚ファイルを読み込む
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
        ///     閣僚ファイルを読み込む
        /// </summary>
        private void LoadFiles()
        {
            // 閣僚ファイルを読み込む
            Ministers.Load();

            // 閣僚リストを絞り込む
            NarrowMinisterList();

            // 閣僚リストの表示を更新する
            UpdateMinisterList();

            // 編集済みフラグがクリアされるため表示を更新する
            countryListBox.Refresh();
        }

        /// <summary>
        ///     閣僚ファイルを保存する
        /// </summary>
        private void SaveFiles()
        {
            // 閣僚ファイルを保存する
            Ministers.Save();

            // 閣僚リストの表示を更新する
            UpdateMinisterList();

            // 編集済みフラグがクリアされるため表示を更新する
            countryListBox.Refresh();
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
            foreach (Minister minister in _narrowedList)
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
            _narrowedList.Clear();

            // 選択中の国家リストを作成する
            List<CountryTag> tags =
                (from string name in countryListBox.SelectedItems select Country.StringMap[name]).ToList();

            // 選択中の国家に所属する指揮官を順に絞り込む
            foreach (Minister minister in Ministers.List.Where(minister => tags.Contains(minister.Country)))
            {
                _narrowedList.Add(minister);
            }
        }

        /// <summary>
        ///     閣僚リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Minister minister = GetSelectedMinister();
            if (minister == null)
            {
                return;
            }

            // 編集項目を更新する
            countryComboBox.SelectedIndex = minister.Country != CountryTag.None ? (int) minister.Country - 1 : -1;
            idNumericUpDown.Value = minister.Id;
            nameTextBox.Text = minister.Name;
            startYearNumericUpDown.Value = minister.StartYear;
            endYearNumericUpDown.Value = minister.EndYear;
            retirementYearNumericUpDown.Value = minister.RetirementYear;
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

            // 項目移動ボタンの状態更新
            topButton.Enabled = ministerListView.SelectedIndices[0] != 0;
            upButton.Enabled = ministerListView.SelectedIndices[0] != 0;
            downButton.Enabled = ministerListView.SelectedIndices[0] != ministerListView.Items.Count - 1;
            bottomButton.Enabled = ministerListView.SelectedIndices[0] != ministerListView.Items.Count - 1;
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
                minister = new Minister
                               {
                                   Country = selected.Country,
                                   Id = selected.Id + 1,
                                   StartYear = 1936,
                                   EndYear = 1970,
                                   RetirementYear = 1999,
                                   Position = MinisterPosition.None,
                                   Personality = 0,
                                   Ideology = MinisterIdeology.None,
                                   Loyalty = MinisterLoyalty.None,
                               };

                // 閣僚ごとの編集済みフラグを設定する
                minister.SetDirty();

                // 閣僚リストに項目を挿入する
                Ministers.InsertItem(minister, selected);
                InsertListItem(minister, ministerListView.SelectedIndices[0] + 1);
            }
            else
            {
                // 新規項目を作成する
                minister = new Minister
                               {
                                   Country =
                                       countryListBox.SelectedItems.Count > 0
                                           ? (CountryTag) (countryListBox.SelectedIndex + 1)
                                           : CountryTag.None,
                                   Id = 0,
                                   StartYear = 1930,
                                   EndYear = 1970,
                                   RetirementYear = 1999,
                                   Position = MinisterPosition.None,
                                   Personality = 0,
                                   Ideology = MinisterIdeology.None,
                                   Loyalty = MinisterLoyalty.None,
                               };

                // 閣僚ごとの編集済みフラグを設定する
                minister.SetDirty();

                // 閣僚リストに項目を追加する
                Ministers.AddItem(minister);
                AddListItem(minister);

                // 編集項目を有効化する
                EnableEditableItems();
            }

            // 国家ごとの編集済みフラグを設定する
            Ministers.SetDirty(minister.Country);
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
            var minister = new Minister
                               {
                                   Country = selected.Country,
                                   Id = selected.Id + 1,
                                   Name = selected.Name,
                                   StartYear = selected.StartYear,
                                   EndYear = selected.EndYear,
                                   RetirementYear = selected.RetirementYear,
                                   Position = selected.Position,
                                   Personality = selected.Personality,
                                   Ideology = selected.Ideology,
                                   Loyalty = selected.Loyalty,
                                   PictureName = selected.PictureName
                               };

            // 閣僚ごとの編集済みフラグを設定する
            minister.SetDirty();

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

            var top = ministerListView.Items[0].Tag as Minister;
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

            var upper = ministerListView.Items[index - 1].Tag as Minister;
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

            var lower = ministerListView.Items[index + 1].Tag as Minister;
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

            var bottom = ministerListView.Items[ministerListView.Items.Count - 1].Tag as Minister;
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
            _narrowedList.Add(minister);

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
            _narrowedList.Insert(index, minister);

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
            _narrowedList.RemoveAt(index);

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
            Minister minister = _narrowedList[src];

            if (src > dest)
            {
                // 上へ移動する場合
                // 絞り込みリストの項目を移動する
                _narrowedList.Insert(dest, minister);
                _narrowedList.RemoveAt(src + 1);

                // 閣僚リストビューの項目を移動する
                ministerListView.Items.Insert(dest, CreateMinisterListViewItem(minister));
                ministerListView.Items.RemoveAt(src + 1);
            }
            else
            {
                // 下へ移動する場合
                // 絞り込みリストの項目を移動する
                _narrowedList.Insert(dest + 1, minister);
                _narrowedList.RemoveAt(src);

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

            var item = new ListViewItem
                           {
                               Text = Country.Strings[(int) minister.Country],
                               Tag = minister
                           };
            item.SubItems.Add(minister.Id.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(minister.Name);
            item.SubItems.Add(minister.StartYear.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(Misc.Mod.NewMinisterFormat ? minister.EndYear.ToString(CultureInfo.InvariantCulture) : "");
            item.SubItems.Add(Config.GetText(Ministers.PositionNames[(int) minister.Position]));
            item.SubItems.Add(Config.GetText(Ministers.PersonalityTable[minister.Personality].Name));
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
            foreach (string name in Country.Tags.Select(country => Country.Strings[(int) country]))
            {
                countryListBox.Items.Add(name);
            }
            countryListBox.SelectedIndex = 0;
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
                CountryTag country = Country.Tags[e.Index];
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
            // 選択数に合わせて全選択/全解除を切り替える
            countryAllButton.Text = countryListBox.SelectedItems.Count <= 1
                                        ? Resources.KeySelectAll
                                        : Resources.KeyUnselectAll;

            // 選択数がゼロの場合は新規追加ボタンを無効化する
            newButton.Enabled = countryListBox.SelectedItems.Count > 0;

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
            // 国タグ
            countryComboBox.Items.Clear();
            int maxWidth = countryComboBox.DropDownWidth;
            foreach (string s in Country.Tags
                                        .Select(country => Country.Strings[(int) country])
                                        .Select(name => Config.ExistsKey(name)
                                                            ? string.Format("{0} {1}", name, Config.GetText(name))
                                                            : name))
            {
                countryComboBox.Items.Add(s);
                maxWidth = Math.Max(maxWidth,
                                    TextRenderer.MeasureText(s, countryComboBox.Font).Width +
                                    SystemInformation.VerticalScrollBarWidth);
            }
            countryComboBox.DropDownWidth = maxWidth;

            // 地位
            positionComboBox.Items.Clear();
            maxWidth = positionComboBox.DropDownWidth;
            foreach (
                string s in Ministers.PositionNames.Where(name => !string.IsNullOrEmpty(name)).Select(Config.GetText))
            {
                positionComboBox.Items.Add(s);
                maxWidth = Math.Max(maxWidth, TextRenderer.MeasureText(s, positionComboBox.Font).Width);
            }
            positionComboBox.DropDownWidth = maxWidth;

            // 特性
            personalityComboBox.DropDownWidth =
                Ministers.PersonalityTable.Select(info => Config.GetText(info.Name))
                         .Select(s => TextRenderer.MeasureText(s, personalityComboBox.Font).Width)
                         .Concat(new[] {personalityComboBox.DropDownWidth})
                         .Max();

            // イデオロギー
            ideologyComboBox.Items.Clear();
            maxWidth = ideologyComboBox.DropDownWidth;
            foreach (
                string s in Ministers.IdeologyNames.Where(name => !string.IsNullOrEmpty(name)).Select(Config.GetText))
            {
                ideologyComboBox.Items.Add(s);
                maxWidth = Math.Max(maxWidth, TextRenderer.MeasureText(s, ideologyComboBox.Font).Width);
            }
            ideologyComboBox.DropDownWidth = maxWidth;

            // 忠誠度
            loyaltyComboBox.Items.Clear();
            maxWidth = loyaltyComboBox.DropDownWidth;
            foreach (string s in Ministers.LoyaltyNames.Where(name => !string.IsNullOrEmpty(name)))
            {
                loyaltyComboBox.Items.Add(s);
                maxWidth = Math.Max(maxWidth, TextRenderer.MeasureText(s, loyaltyComboBox.Font).Width);
            }
            loyaltyComboBox.DropDownWidth = maxWidth;
        }

        /// <summary>
        ///     編集可能な項目を有効化する
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
            pictureNameReferButton.Enabled = true;

            // 無効化時にクリアした文字列を再設定する
            idNumericUpDown.Text = idNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            startYearNumericUpDown.Text = startYearNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);

            if (Misc.Mod.NewMinisterFormat)
            {
                endYearNumericUpDown.Enabled = true;
                endYearNumericUpDown.Text = endYearNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            }
            if (Misc.Mod.RetirementYearMinister)
            {
                retirementYearNumericUpDown.Enabled = true;
                retirementYearNumericUpDown.Text =
                    retirementYearNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            }

            cloneButton.Enabled = true;
            removeButton.Enabled = true;
        }

        /// <summary>
        ///     編集可能な項目を無効化する
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
            pictureNameReferButton.Enabled = false;

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
                personalityComboBox.Items.Add(Config.GetText(Ministers.PersonalityTable[minister.Personality].Name));
                personalityComboBox.SelectedIndex = 0;
            }
            else if (!Ministers.PositionPersonalityTable[(int) minister.Position].Contains(minister.Personality))
            {
                // 閣僚特性が閣僚地位とマッチしない場合、ワンショットで候補に登録する
                personalityComboBox.Items.Add(Config.GetText(Ministers.PersonalityTable[minister.Personality].Name));
                personalityComboBox.SelectedIndex = 0;

                // 閣僚地位と対応する閣僚特性を順に登録する
                foreach (int personality in Ministers.PositionPersonalityTable[(int) minister.Position])
                {
                    personalityComboBox.Items.Add(Config.GetText(Ministers.PersonalityTable[personality].Name));
                }
            }
            else
            {
                // 閣僚地位と対応する閣僚特性を順に登録する
                foreach (int personality in Ministers.PositionPersonalityTable[(int) minister.Position])
                {
                    personalityComboBox.Items.Add(Config.GetText(Ministers.PersonalityTable[personality].Name));
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
                if ((Country.Tags[e.Index] == minister.Country) && minister.IsDirty(MinisterItemId.Country))
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
            if (!string.IsNullOrEmpty(minister.PictureName))
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
            CountryTag country = Country.Tags[countryComboBox.SelectedIndex];
            if (country == minister.Country)
            {
                return;
            }

            // 変更前の国タグの編集済みフラグを設定する
            Ministers.SetDirty(minister.Country);

            // 値を更新する
            minister.Country = country;

            // 閣僚リストビューの項目を更新する
            ministerListView.SelectedItems[0].Text = Country.Strings[(int) minister.Country];

            // 閣僚ごとの編集済みフラグを設定する
            minister.SetDirty(MinisterItemId.Country);

            // 変更後の国タグの編集済みフラグを設定する
            Ministers.SetDirty(minister.Country);

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
            var id = (int) idNumericUpDown.Value;
            if (id == minister.Id)
            {
                return;
            }

            // 値を更新する
            minister.Id = id;

            // 閣僚リストビューの項目を更新する
            ministerListView.SelectedItems[0].SubItems[1].Text = minister.Id.ToString(CultureInfo.InvariantCulture);

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
            if (name.Equals(minister.Name))
            {
                return;
            }

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
            var startYear = (int) startYearNumericUpDown.Value;
            if (startYear == minister.StartYear)
            {
                return;
            }

            // 値を更新する
            minister.StartYear = startYear;

            // 閣僚リストビューの項目を更新する
            ministerListView.SelectedItems[0].SubItems[3].Text =
                minister.StartYear.ToString(CultureInfo.InvariantCulture);

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
            var endYear = (int) endYearNumericUpDown.Value;
            if (endYear == minister.EndYear)
            {
                return;
            }

            // 値を更新する
            minister.EndYear = endYear;

            // 閣僚リストビューの項目を更新する
            ministerListView.SelectedItems[0].SubItems[4].Text =
                minister.EndYear.ToString(CultureInfo.InvariantCulture);

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
            var retirementYear = (int) retirementYearNumericUpDown.Value;
            if (retirementYear == minister.RetirementYear)
            {
                return;
            }

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
            var position = (MinisterPosition) (positionComboBox.SelectedIndex + 1);
            if (position == minister.Position)
            {
                return;
            }

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

            // 値を更新する
            minister.Personality = personality;

            // 閣僚リストビューの項目を更新する
            ministerListView.SelectedItems[0].SubItems[6].Text =
                Config.GetText(Ministers.PersonalityTable[minister.Personality].Name);

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
            if (pictureName.Equals(minister.PictureName))
            {
                return;
            }

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
            var dialog = new OpenFileDialog
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