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
        /// <summary>
        ///     閣僚編集フラグ
        /// </summary>
        private readonly bool[] _dirtyFlags = new bool[Enum.GetValues(typeof (CountryTag)).Length];

        /// <summary>
        ///     絞り込み後の閣僚リスト
        /// </summary>
        private readonly List<Minister> _narrowedMinisterList = new List<Minister>();

        /// <summary>
        ///     マスター閣僚リスト
        /// </summary>
        private List<Minister> _masterMinisterList = new List<Minister>();

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public MinisterEditorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     閣僚ファイルを読み込む
        /// </summary>
        private void LoadMinisterFiles()
        {
            _masterMinisterList = Minister.LoadMinisterFiles();

            ClearDirtyFlags();

            NarrowMinisterList();
            UpdateMinisterList();
        }

        /// <summary>
        ///     編集フラグをセットする
        /// </summary>
        /// <param name="countryTag">国タグ</param>
        private void SetDirtyFlag(CountryTag countryTag)
        {
            _dirtyFlags[(int) countryTag] = true;
        }

        /// <summary>
        ///     編集フラグをクリアする
        /// </summary>
        /// <param name="countryTag">国タグ</param>
        private void ClearDirtyFlag(CountryTag countryTag)
        {
            _dirtyFlags[(int) countryTag] = false;
        }

        /// <summary>
        ///     編集フラグを全てクリアする
        /// </summary>
        private void ClearDirtyFlags()
        {
            foreach (CountryTag countryTag in Enum.GetValues(typeof (CountryTag)))
            {
                ClearDirtyFlag(countryTag);
            }
        }

        /// <summary>
        ///     閣僚リストを国タグで絞り込む
        /// </summary>
        private void NarrowMinisterList()
        {
            _narrowedMinisterList.Clear();
            List<CountryTag> selectedTagList = countryListBox.SelectedItems.Count == 0
                                                   ? new List<CountryTag>()
                                                   : (from string country in countryListBox.SelectedItems
                                                      select Country.CountryTextMap[country]).ToList();

            foreach (
                Minister minister in
                    _masterMinisterList.Where(
                        minister => minister.CountryTag != null && selectedTagList.Contains(minister.CountryTag.Value)))
            {
                _narrowedMinisterList.Add(minister);
            }
        }

        /// <summary>
        ///     閣僚リストの表示を更新する
        /// </summary>
        private void UpdateMinisterList()
        {
            ministerListView.BeginUpdate();
            ministerListView.Items.Clear();

            foreach (Minister minister in _narrowedMinisterList)
            {
                AddMinisterListViewItem(minister);
            }

            if (ministerListView.Items.Count > 0)
            {
                ministerListView.Items[0].Focused = true;
                ministerListView.Items[0].Selected = true;
                EnableEditableItems();
            }
            else
            {
                DisableEditableItems();
            }

            ministerListView.EndUpdate();
        }

        /// <summary>
        ///     編集項目を初期化する
        /// </summary>
        private void InitEditableItems()
        {
            // 国タグ
            int maxSize = countryComboBox.DropDownWidth;
            foreach (string s in Country.CountryTextTable.Select(
                country =>
                Config.Text.ContainsKey(country) ? string.Format("{0} {1}", country, Config.Text[country]) : country))
            {
                countryComboBox.Items.Add(s);
                maxSize = Math.Max(maxSize,
                                   TextRenderer.MeasureText(s, countryComboBox.Font).Width +
                                   SystemInformation.VerticalScrollBarWidth);
            }
            countryComboBox.DropDownWidth = maxSize;

            // 地位
            foreach (string position in Minister.PositionTextTable)
            {
                positionComboBox.Items.Add(Config.Text[position]);
            }

            // 特性
            foreach (string personality in Minister.PersonalityTextTable)
            {
                personalityComboBox.Items.Add(Config.Text[personality]);
            }

            // イデオロギー
            foreach (string ideology in Minister.IdeologyTextTable)
            {
                ideologyComboBox.Items.Add(Config.Text[ideology]);
            }

            // 忠誠度
            foreach (string loyalty in Minister.LoyaltyTextTable)
            {
                loyaltyComboBox.Items.Add(loyalty);
            }
        }

        /// <summary>
        ///     国家リストボックスを初期化する
        /// </summary>
        private void InitCountryList()
        {
            foreach (string country in Country.CountryTextTable)
            {
                countryListBox.Items.Add(country);
            }
            countryListBox.SelectedIndex = 0;
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterEditorFormLoad(object sender, EventArgs e)
        {
            InitEditableItems();
            InitCountryList();
            LoadMinisterFiles();
        }

        /// <summary>
        ///     閣僚リストビューの項目を追加する
        /// </summary>
        /// <param name="minister">追加する項目</param>
        private void AddMinisterListViewItem(Minister minister)
        {
            ministerListView.Items.Add(CreateMinisterListViewItem(minister));
        }

        /// <summary>
        ///     閣僚リストビューの項目を挿入する
        /// </summary>
        /// <param name="index">挿入する位置</param>
        /// <param name="minister">挿入する項目</param>
        private void InsertMinisterListViewItem(int index, Minister minister)
        {
            ministerListView.Items.Insert(index, CreateMinisterListViewItem(minister));
        }

        /// <summary>
        ///     閣僚リストビューの項目を削除する
        /// </summary>
        /// <param name="index">削除する位置</param>
        private void RemoveMinisterListViewItem(int index)
        {
            ministerListView.Items.RemoveAt(index);
        }

        /// <summary>
        ///     閣僚リストビューの項目を作成する
        /// </summary>
        /// <param name="minister">閣僚データ</param>
        /// <returns>閣僚リストビューの項目</returns>
        private ListViewItem CreateMinisterListViewItem(Minister minister)
        {
            if (minister == null)
            {
                return null;
            }

            var item = new ListViewItem
                           {
                               Text =
                                   minister.CountryTag != null
                                       ? Country.CountryTextTable[(int) minister.CountryTag]
                                       : "",
                               Tag = minister
                           };
            item.SubItems.Add(minister.Id.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(minister.Name);
            item.SubItems.Add(minister.StartYear.ToString(CultureInfo.InvariantCulture));
            //item.SubItems.Add(minister.EndYear.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add("");
            item.SubItems.Add(minister.Position != null
                                  ? Config.Text[Minister.PositionTextTable[(int) minister.Position]]
                                  : "");
            item.SubItems.Add(minister.Personality != null
                                  ? Config.Text[Minister.PersonalityTextTable[(int) minister.Personality]]
                                  : "");
            item.SubItems.Add(minister.Ideology != null
                                  ? Config.Text[Minister.IdeologyTextTable[(int) minister.Ideology]]
                                  : "");

            return item;
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
            //endYearNumericUpDown.Enabled = true;
            positionComboBox.Enabled = true;
            personalityComboBox.Enabled = true;
            ideologyComboBox.Enabled = true;
            loyaltyComboBox.Enabled = true;
            pictureNameTextBox.Enabled = true;
            pictureNameReferButton.Enabled = true;

            cloneButton.Enabled = true;
            deleteButton.Enabled = true;
            topButton.Enabled = true;
            upButton.Enabled = true;
            downButton.Enabled = true;
            bottomButton.Enabled = true;
        }

        /// <summary>
        ///     編集可能な項目を無効化する
        /// </summary>
        private void DisableEditableItems()
        {
            countryComboBox.Text = "";
            idNumericUpDown.Value = 0;
            nameTextBox.Text = "";
            startYearNumericUpDown.Value = 1930;
            endYearNumericUpDown.Value = 1970;
            pictureNameTextBox.Text = "";
            ministerPictureBox.ImageLocation = "";

            countryComboBox.Enabled = false;
            idNumericUpDown.Enabled = false;
            nameTextBox.Enabled = false;
            startYearNumericUpDown.Enabled = false;
            endYearNumericUpDown.Enabled = false;
            positionComboBox.Enabled = false;
            personalityComboBox.Enabled = false;
            ideologyComboBox.Enabled = false;
            loyaltyComboBox.Enabled = false;
            pictureNameTextBox.Enabled = false;
            pictureNameReferButton.Enabled = false;

            cloneButton.Enabled = false;
            deleteButton.Enabled = false;
            topButton.Enabled = false;
            upButton.Enabled = false;
            downButton.Enabled = false;
            bottomButton.Enabled = false;
        }

        /// <summary>
        ///     新規ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNewButtonClick(object sender, EventArgs e)
        {
            Minister minister;
            if (ministerListView.SelectedItems.Count > 0)
            {
                var selectedMinister = ministerListView.SelectedItems[0].Tag as Minister;
                if (selectedMinister == null)
                {
                    return;
                }

                minister = new Minister
                               {
                                   CountryTag = selectedMinister.CountryTag,
                                   Id = selectedMinister.Id + 1,
                                   StartYear = 1930,
                                   EndYear = 1970,
                                   Position = null,
                                   Personality = null,
                                   Ideology = null,
                                   Loyalty = null,
                               };

                int masterIndex = _masterMinisterList.IndexOf(selectedMinister);
                _masterMinisterList.Insert(masterIndex + 1, minister);

                int narrowedIndex = ministerListView.SelectedIndices[0] + 1;
                _narrowedMinisterList.Insert(narrowedIndex, minister);
                InsertMinisterListViewItem(narrowedIndex, minister);

                ministerListView.Items[narrowedIndex].Focused = true;
                ministerListView.Items[narrowedIndex].Selected = true;
                ministerListView.Items[narrowedIndex].EnsureVisible();
            }
            else
            {
                minister = new Minister
                               {
                                   CountryTag =
                                       countryListBox.SelectedItems.Count > 0
                                           ? (CountryTag?) (countryListBox.SelectedIndex)
                                           : null,
                                   Id = 0,
                                   StartYear = 1930,
                                   EndYear = 1970,
                                   Position = null,
                                   Personality = null,
                                   Ideology = null,
                                   Loyalty = null,
                               };

                _masterMinisterList.Add(minister);

                _narrowedMinisterList.Add(minister);
                AddMinisterListViewItem(minister);

                ministerListView.Items[0].Focused = true;
                ministerListView.Items[0].Selected = true;

                EnableEditableItems();
            }

            if (minister.CountryTag != null)
            {
                SetDirtyFlag(minister.CountryTag.Value);
            }
        }

        /// <summary>
        ///     複製ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloneButtonClick(object sender, EventArgs e)
        {
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }
            var selectedMinister = ministerListView.SelectedItems[0].Tag as Minister;
            if (selectedMinister == null)
            {
                return;
            }
            var minister = new Minister
                               {
                                   CountryTag = selectedMinister.CountryTag,
                                   Id = selectedMinister.Id + 1,
                                   Name = selectedMinister.Name,
                                   StartYear = selectedMinister.StartYear,
                                   EndYear = selectedMinister.EndYear,
                                   Position = selectedMinister.Position,
                                   Personality = selectedMinister.Personality,
                                   Ideology = selectedMinister.Ideology,
                                   Loyalty = selectedMinister.Loyalty,
                                   PictureName = selectedMinister.PictureName
                               };

            int masterIndex = _masterMinisterList.IndexOf(selectedMinister);
            _masterMinisterList.Insert(masterIndex + 1, minister);

            int narrowedIndex = ministerListView.SelectedIndices[0] + 1;
            _narrowedMinisterList.Insert(narrowedIndex, minister);
            InsertMinisterListViewItem(narrowedIndex, minister);

            ministerListView.Items[narrowedIndex].Focused = true;
            ministerListView.Items[narrowedIndex].Selected = true;
            ministerListView.Items[narrowedIndex].EnsureVisible();

            if (minister.CountryTag != null)
            {
                SetDirtyFlag(minister.CountryTag.Value);
            }
        }

        /// <summary>
        ///     削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDeleteButtonClick(object sender, EventArgs e)
        {
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }
            var selectedMinister = ministerListView.SelectedItems[0].Tag as Minister;
            if (selectedMinister == null)
            {
                return;
            }

            int masterIndex = _masterMinisterList.IndexOf(selectedMinister);
            _masterMinisterList.RemoveAt(masterIndex);

            int narrowedIndex = ministerListView.SelectedIndices[0];
            _narrowedMinisterList.RemoveAt(narrowedIndex);
            RemoveMinisterListViewItem(narrowedIndex);

            if (narrowedIndex < ministerListView.Items.Count)
            {
                ministerListView.Items[narrowedIndex].Focused = true;
                ministerListView.Items[narrowedIndex].Selected = true;
            }
            else if (narrowedIndex - 1 >= 0)
            {
                ministerListView.Items[narrowedIndex - 1].Focused = true;
                ministerListView.Items[narrowedIndex - 1].Selected = true;
            }
            else
            {
                DisableEditableItems();
            }

            if (selectedMinister.CountryTag != null)
            {
                SetDirtyFlag(selectedMinister.CountryTag.Value);
            }
        }

        /// <summary>
        ///     先頭へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTopButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }
            int selectedIndex = ministerListView.SelectedIndices[0];
            // 選択項目がリストの先頭ならば何もしない
            if (ministerListView.SelectedIndices[0] == 0)
            {
                return;
            }

            var selectedMinister = ministerListView.SelectedItems[0].Tag as Minister;
            if (selectedMinister == null)
            {
                return;
            }
            var topMinister = ministerListView.Items[0].Tag as Minister;
            if (topMinister == null)
            {
                return;
            }

            int masterSelectedIndex = _masterMinisterList.IndexOf(selectedMinister);
            int masterTopIndex = _masterMinisterList.IndexOf(topMinister);
            _masterMinisterList.Insert(masterTopIndex, selectedMinister);
            _masterMinisterList.RemoveAt(masterSelectedIndex + 1);

            _narrowedMinisterList.Insert(0, selectedMinister);
            _narrowedMinisterList.RemoveAt(selectedIndex + 1);
            InsertMinisterListViewItem(0, selectedMinister);
            RemoveMinisterListViewItem(selectedIndex + 1);

            ministerListView.Items[0].Focused = true;
            ministerListView.Items[0].Selected = true;

            if (selectedMinister.CountryTag != null)
            {
                SetDirtyFlag(selectedMinister.CountryTag.Value);
            }
        }

        /// <summary>
        ///     上へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }
            // 選択項目がリストの先頭ならば何もしない
            if (ministerListView.SelectedIndices[0] == 0)
            {
                return;
            }

            int selectedIndex = ministerListView.SelectedIndices[0];
            var selectedMinister = ministerListView.SelectedItems[0].Tag as Minister;
            if (selectedMinister == null)
            {
                return;
            }
            var upperMinister = ministerListView.Items[selectedIndex - 1].Tag as Minister;
            if (upperMinister == null)
            {
                return;
            }

            int masterSelectedIndex = _masterMinisterList.IndexOf(selectedMinister);
            int masterUpperIndex = _masterMinisterList.IndexOf(upperMinister);
            _masterMinisterList.Insert(masterUpperIndex, selectedMinister);
            _masterMinisterList.RemoveAt(masterSelectedIndex + 1);

            _narrowedMinisterList.Insert(selectedIndex - 1, selectedMinister);
            _narrowedMinisterList.RemoveAt(selectedIndex + 1);
            InsertMinisterListViewItem(selectedIndex - 1, selectedMinister);
            RemoveMinisterListViewItem(selectedIndex + 1);

            ministerListView.Items[selectedIndex - 1].Focused = true;
            ministerListView.Items[selectedIndex - 1].Selected = true;

            if (selectedMinister.CountryTag != null)
            {
                SetDirtyFlag(selectedMinister.CountryTag.Value);
            }
        }

        /// <summary>
        ///     下へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDownButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }
            // 選択項目がリストの末尾ならば何もしない
            if (ministerListView.SelectedIndices[0] == ministerListView.Items.Count - 1)
            {
                return;
            }

            int selectedIndex = ministerListView.SelectedIndices[0];
            var selectedMinister = ministerListView.SelectedItems[0].Tag as Minister;
            if (selectedMinister == null)
            {
                return;
            }
            var lowerMinister = ministerListView.Items[selectedIndex + 1].Tag as Minister;
            if (lowerMinister == null)
            {
                return;
            }

            int masterSelectedIndex = _masterMinisterList.IndexOf(selectedMinister);
            int masterLowerIndex = _masterMinisterList.IndexOf(lowerMinister);
            _masterMinisterList.Insert(masterSelectedIndex, lowerMinister);
            _masterMinisterList.RemoveAt(masterLowerIndex + 1);

            _narrowedMinisterList.Insert(selectedIndex, lowerMinister);
            _narrowedMinisterList.RemoveAt(selectedIndex + 2);
            InsertMinisterListViewItem(selectedIndex, lowerMinister);
            RemoveMinisterListViewItem(selectedIndex + 2);

            ministerListView.Items[selectedIndex + 1].Focused = true;
            ministerListView.Items[selectedIndex + 1].Selected = true;

            if (selectedMinister.CountryTag != null)
            {
                SetDirtyFlag(selectedMinister.CountryTag.Value);
            }
        }

        /// <summary>
        ///     末尾へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBottomButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }
            int selectedIndex = ministerListView.SelectedIndices[0];
            int bottomIndex = ministerListView.Items.Count - 1;
            // 選択項目がリストの末尾ならば何もしない
            if (ministerListView.SelectedIndices[0] == bottomIndex)
            {
                return;
            }

            var selectedMinister = ministerListView.Items[selectedIndex].Tag as Minister;
            if (selectedMinister == null)
            {
                return;
            }
            var bottomMinister = ministerListView.Items[bottomIndex].Tag as Minister;
            if (bottomMinister == null)
            {
                return;
            }

            int masterSelectedIndex = _masterMinisterList.IndexOf(selectedMinister);
            int masterBottomIndex = _masterMinisterList.IndexOf(bottomMinister);
            _masterMinisterList.Insert(masterBottomIndex + 1, selectedMinister);
            _masterMinisterList.RemoveAt(masterSelectedIndex);

            _narrowedMinisterList.Insert(bottomIndex + 1, selectedMinister);
            _narrowedMinisterList.RemoveAt(selectedIndex);
            InsertMinisterListViewItem(bottomIndex + 1, selectedMinister);
            RemoveMinisterListViewItem(selectedIndex);

            ministerListView.Items[bottomIndex].Focused = true;
            ministerListView.Items[bottomIndex].Selected = true;

            if (selectedMinister.CountryTag != null)
            {
                SetDirtyFlag(selectedMinister.CountryTag.Value);
            }
        }

        /// <summary>
        ///     国家リストボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryListBoxDrawItem(object sender, DrawItemEventArgs e)
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
                    brush = _dirtyFlags[e.Index] ? new SolidBrush(Color.Red) : new SolidBrush(SystemColors.WindowText);
                }
                else
                {
                    brush = new SolidBrush(SystemColors.HighlightText);
                }
                var listbox = sender as ListBox;
                if (listbox != null)
                {
                    string s = listbox.Items[e.Index].ToString();
                    e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
                }
                brush.Dispose();
            }

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
            countryAllButton.Text = countryListBox.SelectedItems.Count <= 1
                                        ? Resources.KeySelectAll
                                        : Resources.KeyUnselectAll;
            newButton.Enabled = countryListBox.SelectedItems.Count > 0;

            NarrowMinisterList();
            UpdateMinisterList();
        }

        /// <summary>
        ///     閣僚リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }
            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }

            if (minister.CountryTag != null)
            {
                if (string.IsNullOrEmpty(countryComboBox.Items[0].ToString()))
                {
                    countryComboBox.Items.RemoveAt(0);
                }
                countryComboBox.SelectedIndex = (int) minister.CountryTag.Value;
            }
            else
            {
                if (!string.IsNullOrEmpty(countryComboBox.Items[0].ToString()))
                {
                    countryComboBox.Items.Insert(0, "");
                }
                countryComboBox.SelectedIndex = 0;
            }

            idNumericUpDown.Value = minister.Id;
            nameTextBox.Text = minister.Name;
            startYearNumericUpDown.Value = minister.StartYear;
            endYearNumericUpDown.Value = minister.EndYear;

            if (minister.Position != null)
            {
                if (string.IsNullOrEmpty(positionComboBox.Items[0].ToString()))
                {
                    positionComboBox.Items.RemoveAt(0);
                }
                positionComboBox.SelectedIndex = (int) minister.Position.Value;
            }
            else
            {
                if (!string.IsNullOrEmpty(positionComboBox.Items[0].ToString()))
                {
                    positionComboBox.Items.Insert(0, "");
                }
                positionComboBox.SelectedIndex = 0;
            }

            if (minister.Personality != null)
            {
                if (string.IsNullOrEmpty(personalityComboBox.Items[0].ToString()))
                {
                    personalityComboBox.Items.RemoveAt(0);
                }
                personalityComboBox.SelectedIndex = (int) minister.Personality;
            }
            else
            {
                if (!string.IsNullOrEmpty(personalityComboBox.Items[0].ToString()))
                {
                    personalityComboBox.Items.Insert(0, "");
                }
                personalityComboBox.SelectedIndex = 0;
            }

            if (minister.Ideology != null)
            {
                if (string.IsNullOrEmpty(ideologyComboBox.Items[0].ToString()))
                {
                    ideologyComboBox.Items.RemoveAt(0);
                }
                ideologyComboBox.SelectedIndex = (int) minister.Ideology;
            }
            else
            {
                if (!string.IsNullOrEmpty(ideologyComboBox.Items[0].ToString()))
                {
                    ideologyComboBox.Items.Insert(0, "");
                }
                ideologyComboBox.SelectedIndex = 0;
            }

            if (minister.Loyalty != null)
            {
                if (string.IsNullOrEmpty(loyaltyComboBox.Items[0].ToString()))
                {
                    loyaltyComboBox.Items.RemoveAt(0);
                }
                loyaltyComboBox.SelectedIndex = (int) minister.Loyalty;
            }
            else
            {
                if (!string.IsNullOrEmpty(loyaltyComboBox.Items[0].ToString()))
                {
                    loyaltyComboBox.Items.Insert(0, "");
                }
                loyaltyComboBox.SelectedIndex = 0;
            }

            pictureNameTextBox.Text = minister.PictureName;
            if (!string.IsNullOrEmpty(minister.PictureName))
            {
                if (Game.IsModActive)
                {
                    string modFileName = Path.Combine(Path.Combine(Game.ModFolderName, Game.PicturePathName),
                                                      Path.ChangeExtension(minister.PictureName, ".bmp"));
                    if (File.Exists(modFileName))
                    {
                        ministerPictureBox.ImageLocation = modFileName;
                    }
                    else
                    {
                        ministerPictureBox.ImageLocation =
                            Path.Combine(Path.Combine(Game.FolderName, Game.PicturePathName),
                                         Path.ChangeExtension(minister.PictureName, ".bmp"));
                    }
                }
                else
                {
                    ministerPictureBox.ImageLocation =
                        Path.Combine(Path.Combine(Game.FolderName, Game.PicturePathName),
                                     Path.ChangeExtension(minister.PictureName, ".bmp"));
                }
            }
        }

        /// <summary>
        ///     国タグ変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryComboBoxSelectionChangeCommitted(object sender, EventArgs e)
        {
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }
            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            CountryTag? newCountryTag = !string.IsNullOrEmpty(countryComboBox.Items[0].ToString())
                                            ? (CountryTag?) countryComboBox.SelectedIndex
                                            : (CountryTag?) (countryComboBox.SelectedIndex - 1);
            if (newCountryTag == minister.CountryTag)
            {
                return;
            }

            if (minister.CountryTag != null)
            {
                SetDirtyFlag(minister.CountryTag.Value);
            }
            minister.CountryTag = newCountryTag;
            ministerListView.SelectedItems[0].Text = Country.CountryTextTable[(int) minister.CountryTag];
            if (minister.CountryTag != null)
            {
                if (string.IsNullOrEmpty(countryComboBox.Items[0].ToString()))
                {
                    countryComboBox.Items.RemoveAt(0);
                }
                countryComboBox.SelectedIndex = (int) minister.CountryTag.Value;
                SetDirtyFlag(minister.CountryTag.Value);
            }

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
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }
            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            var newId = (int) idNumericUpDown.Value;
            if (newId == minister.Id)
            {
                return;
            }
            minister.Id = newId;
            ministerListView.SelectedItems[0].SubItems[1].Text = minister.Id.ToString(CultureInfo.InvariantCulture);

            if (minister.CountryTag != null)
            {
                SetDirtyFlag(minister.CountryTag.Value);
            }
        }

        /// <summary>
        ///     名前文字列変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNameTextBoxTextChanged(object sender, EventArgs e)
        {
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }
            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            string newName = nameTextBox.Text;
            if (newName.Equals(minister.Name))
            {
                return;
            }
            minister.Name = newName;
            ministerListView.SelectedItems[0].SubItems[2].Text = minister.Name;

            if (minister.CountryTag != null)
            {
                SetDirtyFlag(minister.CountryTag.Value);
            }
        }

        /// <summary>
        ///     開始年変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStartYearNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }
            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            var newStartYear = (int) startYearNumericUpDown.Value;
            if (newStartYear == minister.StartYear)
            {
                return;
            }
            minister.StartYear = newStartYear;
            ministerListView.SelectedItems[0].SubItems[3].Text =
                minister.StartYear.ToString(CultureInfo.InvariantCulture);

            if (minister.CountryTag != null)
            {
                SetDirtyFlag(minister.CountryTag.Value);
            }
        }

        /// <summary>
        ///     閣僚地位変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPositionComboBoxSelectionChangeCommitted(object sender, EventArgs e)
        {
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }
            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            MinisterPosition? newPosition = !string.IsNullOrEmpty(positionComboBox.Items[0].ToString())
                                                ? (MinisterPosition?) positionComboBox.SelectedIndex
                                                : (MinisterPosition?) (positionComboBox.SelectedIndex - 1);
            if (newPosition == minister.Position)
            {
                return;
            }

            minister.Position = newPosition;
            ministerListView.SelectedItems[0].SubItems[5].Text =
                minister.Position != null ? Config.Text[Minister.PositionTextTable[(int) minister.Position]] : "";

            if (minister.CountryTag != null)
            {
                if (string.IsNullOrEmpty(positionComboBox.Items[0].ToString()))
                {
                    positionComboBox.Items.RemoveAt(0);
                }
                positionComboBox.SelectedIndex = (int) minister.Position.Value;
                SetDirtyFlag(minister.CountryTag.Value);
            }
        }

        /// <summary>
        ///     閣僚特性変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPersonalityComboBoxSelectionChangeCommitted(object sender, EventArgs e)
        {
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }
            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            MinisterPersonality? newPersonality = !string.IsNullOrEmpty(personalityComboBox.Items[0].ToString())
                                                      ? (MinisterPersonality?) personalityComboBox.SelectedIndex
                                                      : (MinisterPersonality?) (personalityComboBox.SelectedIndex - 1);
            if (newPersonality == minister.Personality)
            {
                return;
            }

            minister.Personality = newPersonality;
            ministerListView.SelectedItems[0].SubItems[6].Text =
                minister.Personality != null
                    ? Config.Text[Minister.PersonalityTextTable[(int) minister.Personality]]
                    : "";

            if (minister.CountryTag != null)
            {
                if (string.IsNullOrEmpty(personalityComboBox.Items[0].ToString()))
                {
                    personalityComboBox.Items.RemoveAt(0);
                }
                personalityComboBox.SelectedIndex = (int) minister.Personality;
                SetDirtyFlag(minister.CountryTag.Value);
            }
        }

        /// <summary>
        ///     イデオロギー変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IdeologyComboBoxSelectionChangeCommitted(object sender, EventArgs e)
        {
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }
            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            MinisterIdeology? newIdeology = !string.IsNullOrEmpty(ideologyComboBox.Items[0].ToString())
                                                ? (MinisterIdeology?) ideologyComboBox.SelectedIndex
                                                : (MinisterIdeology?) (ideologyComboBox.SelectedIndex - 1);
            if (newIdeology == minister.Ideology)
            {
                return;
            }

            minister.Ideology = newIdeology;
            ministerListView.SelectedItems[0].SubItems[7].Text =
                minister.Ideology != null ? Config.Text[Minister.IdeologyTextTable[(int) minister.Ideology]] : "";

            if (minister.CountryTag != null)
            {
                if (string.IsNullOrEmpty(ideologyComboBox.Items[0].ToString()))
                {
                    ideologyComboBox.Items.RemoveAt(0);
                }
                ideologyComboBox.SelectedIndex = (int) minister.Ideology;
                SetDirtyFlag(minister.CountryTag.Value);
            }
        }

        /// <summary>
        ///     忠誠度変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoyaltyComboBoxSelectionChangeCommitted(object sender, EventArgs e)
        {
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }
            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            MinisterLoyalty? newLoyalty = !string.IsNullOrEmpty(loyaltyComboBox.Items[0].ToString())
                                              ? (MinisterLoyalty?) loyaltyComboBox.SelectedIndex
                                              : (MinisterLoyalty?) (loyaltyComboBox.SelectedIndex - 1);
            if (newLoyalty == minister.Loyalty)
            {
                return;
            }

            minister.Loyalty = newLoyalty;

            if (minister.CountryTag != null)
            {
                if (string.IsNullOrEmpty(loyaltyComboBox.Items[0].ToString()))
                {
                    loyaltyComboBox.Items.RemoveAt(0);
                }
                loyaltyComboBox.SelectedIndex = (int) minister.Loyalty;
                SetDirtyFlag(minister.CountryTag.Value);
            }
        }

        /// <summary>
        ///     画像ファイル名変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPictureNameTextBoxTextChanged(object sender, EventArgs e)
        {
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }
            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            string newPictureName = pictureNameTextBox.Text;
            if (newPictureName.Equals(minister.PictureName))
            {
                return;
            }

            minister.PictureName = newPictureName;
            if (!string.IsNullOrEmpty(minister.PictureName))
            {
                if (Game.IsModActive)
                {
                    string modFileName = Path.Combine(Path.Combine(Game.ModFolderName, Game.PicturePathName),
                                                      Path.ChangeExtension(minister.PictureName, ".bmp"));
                    if (File.Exists(modFileName))
                    {
                        ministerPictureBox.ImageLocation = modFileName;
                    }
                    else
                    {
                        ministerPictureBox.ImageLocation =
                            Path.Combine(Path.Combine(Game.FolderName, Game.PicturePathName),
                                         Path.ChangeExtension(minister.PictureName, ".bmp"));
                    }
                }
                else
                {
                    ministerPictureBox.ImageLocation =
                        Path.Combine(Path.Combine(Game.FolderName, Game.PicturePathName),
                                     Path.ChangeExtension(minister.PictureName, ".bmp"));
                }
            }

            if (minister.CountryTag != null)
            {
                SetDirtyFlag(minister.CountryTag.Value);
            }
        }

        /// <summary>
        ///     画像ファイル名参照ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPictureNameReferButtonClick(object sender, EventArgs e)
        {
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }
            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }

            var dialog = new OpenFileDialog
                             {
                                 InitialDirectory = Path.Combine(Game.FolderName, Game.PicturePathName),
                                 FileName = minister.PictureName,
                                 Filter = Resources.OpenBitmapFileDialogFilter
                             };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                pictureNameTextBox.Text = Path.GetFileNameWithoutExtension(dialog.FileName);
            }
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

        /// <summary>
        ///     再読み込みボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReloadButtonClick(object sender, EventArgs e)
        {
            LoadMinisterFiles();
        }

        /// <summary>
        ///     保存ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            Minister.SaveMinisterFiles(_masterMinisterList, _dirtyFlags);
            ClearDirtyFlags();
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
    }
}