using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HoI2Editor.Models;
using HoI2Editor.Properties;

namespace HoI2Editor.Forms
{
    /// <summary>
    /// 閣僚エディタのフォーム
    /// </summary>
    public partial class MinisterEditorForm : Form
    {
        /// <summary>
        /// 絞り込み後の閣僚リスト
        /// </summary>
        private readonly List<Minister> _narrowedMinisterList = new List<Minister>();

        /// <summary>
        /// マスター閣僚リスト
        /// </summary>
        private List<Minister> _masterMinisterList = new List<Minister>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MinisterEditorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 閣僚ファイルを読み込む
        /// </summary>
        private void LoadMinisterFiles()
        {
            _masterMinisterList = Minister.LoadMinisterFiles();

            NarrowMinisterList();
            UpdateMinisterList();
        }

        /// <summary>
        /// 閣僚リストを国タグで絞り込む
        /// </summary>
        private void NarrowMinisterList()
        {
            _narrowedMinisterList.Clear();
            List<CountryTag> selectedTagList;
            if (countryListBox.SelectedItems.Count == 0)
            {
                selectedTagList = new List<CountryTag> {CountryTag.None};
            }
            else
            {
                selectedTagList =
                    (from string countryText in countryListBox.SelectedItems select Country.CountryTextMap[countryText])
                        .
                        ToList();
            }

            foreach (
                Minister minister in
                    _masterMinisterList.Where(minister => selectedTagList.Contains(minister.CountryTag)))
            {
                _narrowedMinisterList.Add(minister);
            }
        }

        /// <summary>
        /// 閣僚リストの表示を更新する
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
        /// 編集項目を初期化する
        /// </summary>
        private void InitEditableItems()
        {
            // 国タグ
            foreach (string countryText in Country.CountryTextTable)
            {
                countryComboBox.Items.Add(countryText);
            }

            // 地位
            foreach (string positionText in Minister.PositionTextTable)
            {
                positionComboBox.Items.Add(!string.IsNullOrEmpty(positionText) ? Config.Text[positionText] : "");
            }

            // 特性
            foreach (string personalityText in Minister.PersonalityTextTable)
            {
                personalityComboBox.Items.Add(!string.IsNullOrEmpty(personalityText) ? Config.Text[personalityText] : "");
            }

            // イデオロギー
            foreach (string ideologyText in Minister.IdeologyTextTable)
            {
                ideologyComboBox.Items.Add(!string.IsNullOrEmpty(ideologyText) ? Config.Text[ideologyText] : "");
            }

            // 忠誠度
            foreach (string loyaltyText in Minister.LoyaltyTextTable)
            {
                loyaltyComboBox.Items.Add(loyaltyText);
            }
        }

        /// <summary>
        /// 国家リストボックスを初期化する
        /// </summary>
        private void InitCountryList()
        {
            foreach (
                string countryText in Country.CountryTextTable.Where(countryText => !string.IsNullOrEmpty(countryText)))
            {
                countryListBox.Items.Add(countryText);
            }
            countryListBox.SelectedIndex = 0;
        }

        /// <summary>
        /// フォーム読み込み時の処理
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
        /// 閣僚リストビューの項目を追加する
        /// </summary>
        /// <param name="minister">追加する項目</param>
        private void AddMinisterListViewItem(Minister minister)
        {
            var item = new ListViewItem
                           {
                               Text =
                                   minister.CountryTag != CountryTag.None
                                       ? Country.CountryTextTable[(int) minister.CountryTag]
                                       : "",
                               Tag = minister
                           };
            item.SubItems.Add(minister.Id.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(minister.Name);
            item.SubItems.Add(minister.StartYear.ToString(CultureInfo.InvariantCulture));
            //item.SubItems.Add(minister.EndYear.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add("");
            item.SubItems.Add(minister.Position != MinisterPosition.None
                                  ? Config.Text[Minister.PositionTextTable[(int) minister.Position]]
                                  : "");
            item.SubItems.Add(minister.Personality != MinisterPersonality.None
                                  ? Config.Text[Minister.PersonalityTextTable[(int) minister.Personality]]
                                  : "");
            item.SubItems.Add(minister.Ideology != MinisterIdeology.None
                                  ? Config.Text[Minister.IdeologyTextTable[(int) minister.Ideology]]
                                  : "");

            ministerListView.Items.Add(item);
        }

        /// <summary>
        /// 閣僚リストビューの項目を挿入する
        /// </summary>
        /// <param name="index">挿入する位置</param>
        /// <param name="minister">挿入する項目</param>
        private void InsertMinisterListViewItem(int index, Minister minister)
        {
            var item = new ListViewItem
                           {
                               Text =
                                   minister.CountryTag != CountryTag.None
                                       ? Country.CountryTextTable[(int) minister.CountryTag]
                                       : "",
                               Tag = minister
                           };
            item.SubItems.Add(minister.Id.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(minister.Name);
            item.SubItems.Add(minister.StartYear.ToString(CultureInfo.InvariantCulture));
            //item.SubItems.Add(minister.EndYear.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add("");
            item.SubItems.Add(minister.Position != MinisterPosition.None
                                  ? Config.Text[Minister.PositionTextTable[(int) minister.Position]]
                                  : "");
            item.SubItems.Add(minister.Personality != MinisterPersonality.None
                                  ? Config.Text[Minister.PersonalityTextTable[(int) minister.Personality]]
                                  : "");
            item.SubItems.Add(minister.Ideology != MinisterIdeology.None
                                  ? Config.Text[Minister.IdeologyTextTable[(int) minister.Ideology]]
                                  : "");

            ministerListView.Items.Insert(index, item);
        }

        /// <summary>
        /// 閣僚リストビューの項目を削除する
        /// </summary>
        /// <param name="index"></param>
        private void RemoveMinisterListViewItem(int index)
        {
            ministerListView.Items.RemoveAt(index);
        }

        /// <summary>
        /// 編集可能な項目を有効化する
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
        /// 編集可能な項目を無効化する
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
        /// 新規ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNewButtonClick(object sender, EventArgs e)
        {
            if (ministerListView.SelectedItems.Count > 0)
            {
                var selectedMinister = ministerListView.SelectedItems[0].Tag as Minister;
                var minister = new Minister
                                   {
                                       CountryTag =
                                           selectedMinister != null ? selectedMinister.CountryTag : CountryTag.None,
                                       Id = selectedMinister != null ? selectedMinister.Id + 1 : 0,
                                       StartYear = 1930,
                                       EndYear = 1970,
                                       Position = MinisterPosition.None,
                                       Personality = MinisterPersonality.None,
                                       Ideology = MinisterIdeology.None,
                                       Loyalty = MinisterLoyalty.None
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
                if (countryListBox.SelectedItems.Count == 0)
                {
                    return;
                }
                var minister = new Minister
                                   {
                                       CountryTag = (CountryTag) (countryListBox.SelectedIndex + 1),
                                       Id = 0,
                                       StartYear = 1930,
                                       EndYear = 1970,
                                       Position = MinisterPosition.None,
                                       Personality = MinisterPersonality.None,
                                       Ideology = MinisterIdeology.None,
                                       Loyalty = MinisterLoyalty.None
                                   };
                _masterMinisterList.Add(minister);
                _narrowedMinisterList.Add(minister);
                AddMinisterListViewItem(minister);
                ministerListView.Items[0].Focused = true;
                ministerListView.Items[0].Selected = true;
                EnableEditableItems();
            }
        }

        /// <summary>
        /// 複製ボタン押下時の処理
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
        }

        /// <summary>
        /// 削除ボタン押下時の処理
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
        }

        /// <summary>
        /// 先頭へボタン押下時の処理
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
        }

        /// <summary>
        /// 上へボタン押下時の処理
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
        }

        /// <summary>
        /// 下へボタン押下時の処理
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
        }

        /// <summary>
        /// 末尾へボタン押下時の処理
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
        }

        /// <summary>
        /// 国家リストボックスの選択項目変更時の処理
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
        /// 閣僚リストビューの選択項目変更時の処理
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

            countryComboBox.Text = minister.CountryTag != CountryTag.None
                                       ? Country.CountryTextTable[(int) minister.CountryTag]
                                       : "";
            idNumericUpDown.Value = minister.Id;
            nameTextBox.Text = minister.Name;
            startYearNumericUpDown.Value = minister.StartYear;
            endYearNumericUpDown.Value = minister.EndYear;
            positionComboBox.Text = minister.Position != MinisterPosition.None
                                        ? Config.Text[Minister.PositionTextTable[(int) minister.Position]]
                                        : "";
            personalityComboBox.Text = minister.Personality != MinisterPersonality.None
                                           ? Config.Text[Minister.PersonalityTextTable[(int) minister.Personality]]
                                           : "";
            ideologyComboBox.Text = minister.Ideology != MinisterIdeology.None
                                        ? Config.Text[Minister.IdeologyTextTable[(int) minister.Ideology]]
                                        : "";
            loyaltyComboBox.Text = minister.Loyalty != MinisterLoyalty.None
                                       ? Minister.LoyaltyTextTable[(int) minister.Loyalty]
                                       : "";
            pictureNameTextBox.Text = minister.PictureName;
            ministerPictureBox.ImageLocation = Game.GetPictureFileName(minister.PictureName);
        }

        /// <summary>
        /// 国タグ変更時の処理
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
            minister.CountryTag = (CountryTag) countryComboBox.SelectedIndex;
            ministerListView.SelectedItems[0].Text = Country.CountryTextTable[(int) minister.CountryTag];
        }

        /// <summary>
        /// ID変更時の処理
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
            minister.Id = (int) idNumericUpDown.Value;
            ministerListView.SelectedItems[0].SubItems[1].Text = minister.Id.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 名前文字列変更時の処理
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
            minister.Name = nameTextBox.Text;
            ministerListView.SelectedItems[0].SubItems[2].Text = minister.Name;
        }

        /// <summary>
        /// 開始年変更時の処理
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
            minister.StartYear = (int) startYearNumericUpDown.Value;
            ministerListView.SelectedItems[0].SubItems[3].Text =
                minister.StartYear.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 閣僚地位変更時の処理
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
            minister.Position = (MinisterPosition) positionComboBox.SelectedIndex;
            ministerListView.SelectedItems[0].SubItems[5].Text = minister.Position != MinisterPosition.None
                                                                     ? Config.Text[
                                                                         Minister.PositionTextTable[
                                                                             (int) minister.Position]]
                                                                     : "";
        }

        /// <summary>
        /// 閣僚特性変更時の処理
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
            minister.Personality = (MinisterPersonality) personalityComboBox.SelectedIndex;
            ministerListView.SelectedItems[0].SubItems[6].Text = minister.Personality != MinisterPersonality.None
                                                                     ? Config.Text[
                                                                         Minister.PersonalityTextTable[
                                                                             (int) minister.Personality]]
                                                                     : "";
        }

        /// <summary>
        /// イデオロギー変更時の処理
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
            minister.Ideology = (MinisterIdeology) ideologyComboBox.SelectedIndex;
            ministerListView.SelectedItems[0].SubItems[7].Text = minister.Ideology != MinisterIdeology.None
                                                                     ? Config.Text[
                                                                         Minister.IdeologyTextTable[
                                                                             (int) minister.Ideology]]
                                                                     : "";
        }

        /// <summary>
        /// 忠誠度変更時の処理
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
            minister.Loyalty = (MinisterLoyalty) loyaltyComboBox.SelectedIndex;
        }

        /// <summary>
        /// 画像ファイル名変更時の処理
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
            minister.PictureName = pictureNameTextBox.Text;
            ministerPictureBox.ImageLocation = Game.GetPictureFileName(minister.PictureName);
        }

        /// <summary>
        /// 画像ファイル名参照ボタン押下時の処理
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
                                 InitialDirectory = Game.PictureFolderName,
                                 FileName = minister.PictureName,
                                 Filter = Resources.OpenBitmapFileDialogFilter
                             };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                pictureNameTextBox.Text = Path.GetFileNameWithoutExtension(dialog.FileName);
            }
        }

        /// <summary>
        /// 国家リストボックスの全選択/全解除ボタン押下時の処理
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
        /// 再読み込みボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReloadButtonClick(object sender, EventArgs e)
        {
            LoadMinisterFiles();
        }

        /// <summary>
        /// 閉じるボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloseButtonClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}