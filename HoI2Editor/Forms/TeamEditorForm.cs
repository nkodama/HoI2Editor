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
    ///     研究機関エディタのフォーム
    /// </summary>
    public partial class TeamEditorForm : Form
    {
        /// <summary>
        ///     研究機関編集フラグ
        /// </summary>
        private readonly bool[] _dirtyFlags = new bool[Enum.GetValues(typeof (CountryTag)).Length];

        /// <summary>
        ///     絞り込み後の閣僚リスト
        /// </summary>
        private readonly List<Team> _narrowedTeamList = new List<Team>();

        /// <summary>
        ///     マスター閣僚リスト
        /// </summary>
        private List<Team> _masterTeamList = new List<Team>();

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public TeamEditorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     研究機関ファイルを読み込む
        /// </summary>
        private void LoadTeamFiles()
        {
            _masterTeamList = Team.LoadTeamFiles();

            ClearDirtyFlags();

            NarrowTeamList();
            UpdateTeamList();
        }

        /// <summary>
        ///     編集フラグをセットする
        /// </summary>
        /// <param name="countryTag">国タグ</param>
        private void SetDirtyFlag(CountryTag countryTag)
        {
            if (countryTag == CountryTag.None)
            {
                return;
            }
            _dirtyFlags[(int) countryTag] = true;
        }

        /// <summary>
        ///     編集フラグをクリアする
        /// </summary>
        /// <param name="countryTag">国タグ</param>
        private void ClearDirtyFlag(CountryTag countryTag)
        {
            if (countryTag == CountryTag.None)
            {
                return;
            }
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
        ///     研究機関リストを国タグで絞り込む
        /// </summary>
        private void NarrowTeamList()
        {
            _narrowedTeamList.Clear();
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
                Team team in
                    _masterTeamList.Where(team => selectedTagList.Contains(team.CountryTag)))
            {
                _narrowedTeamList.Add(team);
            }
        }

        /// <summary>
        ///     研究機関リストの表示を更新する
        /// </summary>
        private void UpdateTeamList()
        {
            teamListView.BeginUpdate();
            teamListView.Items.Clear();

            foreach (Team team in _narrowedTeamList)
            {
                AddTeamListViewItem(team);
            }

            if (teamListView.Items.Count > 0)
            {
                teamListView.Items[0].Focused = true;
                teamListView.Items[0].Selected = true;
                EnableEditableItems();
            }
            else
            {
                DisableEditableItems();
            }

            teamListView.EndUpdate();
        }

        /// <summary>
        ///     編集項目を初期化する
        /// </summary>
        private void InitEditableItems()
        {
            // 国タグ
            foreach (string countryText in Country.CountryTextTable)
            {
                countryComboBox.Items.Add(countryText);
            }

            // 特性
            foreach (string specialityText in Team.SpecialityTextTable)
            {
                string text = !string.IsNullOrEmpty(specialityText) ? Config.Text[specialityText] : "";
                specialityComboBox1.Items.Add(text);
                specialityComboBox2.Items.Add(text);
                specialityComboBox3.Items.Add(text);
                specialityComboBox4.Items.Add(text);
                specialityComboBox5.Items.Add(text);
                specialityComboBox6.Items.Add(text);
            }
        }

        /// <summary>
        ///     国家リストボックスを初期化する
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
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTeamEditorFormLoad(object sender, EventArgs e)
        {
            InitEditableItems();
            InitCountryList();
            LoadTeamFiles();
        }

        /// <summary>
        ///     研究機関リストビューの項目を追加する
        /// </summary>
        /// <param name="team">追加する項目</param>
        private void AddTeamListViewItem(Team team)
        {
            teamListView.Items.Add(CreateTeamListViewItem(team));
        }

        /// <summary>
        ///     研究機関リストビューの項目を挿入する
        /// </summary>
        /// <param name="index">挿入する位置</param>
        /// <param name="team">挿入する項目</param>
        private void InsertTeamListViewItem(int index, Team team)
        {
            teamListView.Items.Insert(index, CreateTeamListViewItem(team));
        }

        /// <summary>
        ///     研究機関リストビューの項目を削除する
        /// </summary>
        /// <param name="index">削除する位置</param>
        private void RemoveTeamListViewItem(int index)
        {
            teamListView.Items.RemoveAt(index);
        }

        /// <summary>
        ///     研究機関リストビューの項目を作成する
        /// </summary>
        /// <param name="team">研究機関データ</param>
        /// <returns>研究機関リストビューの項目</returns>
        private ListViewItem CreateTeamListViewItem(Team team)
        {
            if (team == null)
            {
                return null;
            }

            var item = new ListViewItem
                           {
                               Text =
                                   team.CountryTag != CountryTag.None
                                       ? Country.CountryTextTable[(int) team.CountryTag]
                                       : "",
                               Tag = team
                           };
            item.SubItems.Add(team.Id.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(team.Name);
            item.SubItems.Add(team.Skill.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(team.StartYear.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(team.EndYear.ToString(CultureInfo.InvariantCulture));
            for (int i = 0; i < 6; i++)
            {
                item.SubItems.Add(team.Specialities[i] != TechSpeciality.None
                                      ? Config.Text[Team.SpecialityTextTable[(int) team.Specialities[i]]]
                                      : "");
            }

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
            skillNumericUpDown.Enabled = true;
            startYearNumericUpDown.Enabled = true;
            endYearNumericUpDown.Enabled = true;
            pictureNameTextBox.Enabled = true;
            pictureNameReferButton.Enabled = true;
            specialityComboBox1.Enabled = true;
            specialityComboBox2.Enabled = true;
            specialityComboBox3.Enabled = true;
            specialityComboBox4.Enabled = true;
            specialityComboBox5.Enabled = true;
            specialityComboBox6.Enabled = true;

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
            skillNumericUpDown.Value = 0;
            startYearNumericUpDown.Value = 1930;
            endYearNumericUpDown.Value = 1970;
            pictureNameTextBox.Text = "";
            teamPictureBox.ImageLocation = "";

            countryComboBox.Enabled = false;
            idNumericUpDown.Enabled = false;
            nameTextBox.Enabled = false;
            skillNumericUpDown.Enabled = false;
            startYearNumericUpDown.Enabled = false;
            endYearNumericUpDown.Enabled = false;
            pictureNameTextBox.Enabled = false;
            pictureNameReferButton.Enabled = false;
            specialityComboBox1.Enabled = false;
            specialityComboBox2.Enabled = false;
            specialityComboBox3.Enabled = false;
            specialityComboBox4.Enabled = false;
            specialityComboBox5.Enabled = false;
            specialityComboBox6.Enabled = false;

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
            Team team;
            if (teamListView.SelectedItems.Count > 0)
            {
                var selectedTeam = teamListView.SelectedItems[0].Tag as Team;
                if (selectedTeam == null)
                {
                    return;
                }
                team = new Team
                           {
                               CountryTag = selectedTeam.CountryTag,
                               Id = selectedTeam.Id + 1,
                               Skill = 1,
                               StartYear = 1930,
                               EndYear = 1970,
                           };
                int masterIndex = _masterTeamList.IndexOf(selectedTeam);
                _masterTeamList.Insert(masterIndex + 1, team);
                int narrowedIndex = teamListView.SelectedIndices[0] + 1;
                _narrowedTeamList.Insert(narrowedIndex, team);
                InsertTeamListViewItem(narrowedIndex, team);
                teamListView.Items[narrowedIndex].Focused = true;
                teamListView.Items[narrowedIndex].Selected = true;
                teamListView.Items[narrowedIndex].EnsureVisible();
            }
            else
            {
                team = new Team
                           {
                               CountryTag =
                                   countryListBox.SelectedItems.Count > 0
                                       ? (CountryTag) (countryListBox.SelectedIndex + 1)
                                       : CountryTag.None,
                               Id = 0,
                               Skill = 1,
                               StartYear = 1930,
                               EndYear = 1970,
                           };
                _masterTeamList.Add(team);
                _narrowedTeamList.Add(team);
                AddTeamListViewItem(team);
                teamListView.Items[0].Focused = true;
                teamListView.Items[0].Selected = true;
                EnableEditableItems();
            }
            SetDirtyFlag(team.CountryTag);
        }

        /// <summary>
        ///     複製ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloneButtonClick(object sender, EventArgs e)
        {
            if (teamListView.SelectedItems.Count == 0)
            {
                return;
            }
            var selectedTeam = teamListView.SelectedItems[0].Tag as Team;
            if (selectedTeam == null)
            {
                return;
            }
            var team = new Team
                           {
                               CountryTag = selectedTeam.CountryTag,
                               Id = selectedTeam.Id + 1,
                               Name = selectedTeam.Name,
                               StartYear = selectedTeam.StartYear,
                               EndYear = selectedTeam.EndYear,
                               PictureName = selectedTeam.PictureName,
                           };
            for (int i = 0; i < Team.SpecialityLength; i++)
            {
                team.Specialities[i] = selectedTeam.Specialities[i];
            }
            int masterIndex = _masterTeamList.IndexOf(selectedTeam);
            _masterTeamList.Insert(masterIndex + 1, team);
            int narrowedIndex = teamListView.SelectedIndices[0] + 1;
            _narrowedTeamList.Insert(narrowedIndex, team);
            InsertTeamListViewItem(narrowedIndex, team);
            teamListView.Items[narrowedIndex].Focused = true;
            teamListView.Items[narrowedIndex].Selected = true;
            teamListView.Items[narrowedIndex].EnsureVisible();
            SetDirtyFlag(team.CountryTag);
        }

        /// <summary>
        ///     削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDeleteButtonClick(object sender, EventArgs e)
        {
            if (teamListView.SelectedItems.Count == 0)
            {
                return;
            }
            var selectedTeam = teamListView.SelectedItems[0].Tag as Team;
            if (selectedTeam == null)
            {
                return;
            }
            int masterIndex = _masterTeamList.IndexOf(selectedTeam);
            _masterTeamList.RemoveAt(masterIndex);
            int narrowedIndex = teamListView.SelectedIndices[0];
            _narrowedTeamList.RemoveAt(narrowedIndex);
            RemoveTeamListViewItem(narrowedIndex);
            if (narrowedIndex < teamListView.Items.Count)
            {
                teamListView.Items[narrowedIndex].Focused = true;
                teamListView.Items[narrowedIndex].Selected = true;
            }
            else if (narrowedIndex - 1 >= 0)
            {
                teamListView.Items[narrowedIndex - 1].Focused = true;
                teamListView.Items[narrowedIndex - 1].Selected = true;
            }
            else
            {
                DisableEditableItems();
            }
            SetDirtyFlag(selectedTeam.CountryTag);
        }

        /// <summary>
        ///     先頭へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTopButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (teamListView.SelectedItems.Count == 0)
            {
                return;
            }
            int selectedIndex = teamListView.SelectedIndices[0];
            // 選択項目がリストの先頭ならば何もしない
            if (teamListView.SelectedIndices[0] == 0)
            {
                return;
            }
            var selectedTeam = teamListView.SelectedItems[0].Tag as Team;
            if (selectedTeam == null)
            {
                return;
            }
            var topTeam = teamListView.Items[0].Tag as Team;
            if (topTeam == null)
            {
                return;
            }
            int masterSelectedIndex = _masterTeamList.IndexOf(selectedTeam);
            int masterTopIndex = _masterTeamList.IndexOf(topTeam);
            _masterTeamList.Insert(masterTopIndex, selectedTeam);
            _masterTeamList.RemoveAt(masterSelectedIndex + 1);
            _narrowedTeamList.Insert(0, selectedTeam);
            _narrowedTeamList.RemoveAt(selectedIndex + 1);
            InsertTeamListViewItem(0, selectedTeam);
            RemoveTeamListViewItem(selectedIndex + 1);
            teamListView.Items[0].Focused = true;
            teamListView.Items[0].Selected = true;
            SetDirtyFlag(selectedTeam.CountryTag);
        }

        /// <summary>
        ///     上へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (teamListView.SelectedItems.Count == 0)
            {
                return;
            }
            // 選択項目がリストの先頭ならば何もしない
            if (teamListView.SelectedIndices[0] == 0)
            {
                return;
            }
            int selectedIndex = teamListView.SelectedIndices[0];
            var selectedTeam = teamListView.SelectedItems[0].Tag as Team;
            if (selectedTeam == null)
            {
                return;
            }
            var upperTeam = teamListView.Items[selectedIndex - 1].Tag as Team;
            if (upperTeam == null)
            {
                return;
            }
            int masterSelectedIndex = _masterTeamList.IndexOf(selectedTeam);
            int masterUpperIndex = _masterTeamList.IndexOf(upperTeam);
            _masterTeamList.Insert(masterUpperIndex, selectedTeam);
            _masterTeamList.RemoveAt(masterSelectedIndex + 1);
            _narrowedTeamList.Insert(selectedIndex - 1, selectedTeam);
            _narrowedTeamList.RemoveAt(selectedIndex + 1);
            InsertTeamListViewItem(selectedIndex - 1, selectedTeam);
            RemoveTeamListViewItem(selectedIndex + 1);
            teamListView.Items[selectedIndex - 1].Focused = true;
            teamListView.Items[selectedIndex - 1].Selected = true;
            SetDirtyFlag(selectedTeam.CountryTag);
        }

        /// <summary>
        ///     下へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDownButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (teamListView.SelectedItems.Count == 0)
            {
                return;
            }
            // 選択項目がリストの末尾ならば何もしない
            if (teamListView.SelectedIndices[0] == teamListView.Items.Count - 1)
            {
                return;
            }
            int selectedIndex = teamListView.SelectedIndices[0];
            var selectedTeam = teamListView.SelectedItems[0].Tag as Team;
            if (selectedTeam == null)
            {
                return;
            }
            var lowerTeam = teamListView.Items[selectedIndex + 1].Tag as Team;
            if (lowerTeam == null)
            {
                return;
            }
            int masterSelectedIndex = _masterTeamList.IndexOf(selectedTeam);
            int masterLowerIndex = _masterTeamList.IndexOf(lowerTeam);
            _masterTeamList.Insert(masterSelectedIndex, lowerTeam);
            _masterTeamList.RemoveAt(masterLowerIndex + 1);
            _narrowedTeamList.Insert(selectedIndex, lowerTeam);
            _narrowedTeamList.RemoveAt(selectedIndex + 2);
            InsertTeamListViewItem(selectedIndex, lowerTeam);
            RemoveTeamListViewItem(selectedIndex + 2);
            teamListView.Items[selectedIndex + 1].Focused = true;
            teamListView.Items[selectedIndex + 1].Selected = true;
            SetDirtyFlag(selectedTeam.CountryTag);
        }

        /// <summary>
        ///     末尾へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBottomButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (teamListView.SelectedItems.Count == 0)
            {
                return;
            }
            int selectedIndex = teamListView.SelectedIndices[0];
            int bottomIndex = teamListView.Items.Count - 1;
            // 選択項目がリストの末尾ならば何もしない
            if (teamListView.SelectedIndices[0] == bottomIndex)
            {
                return;
            }
            var selectedTeam = teamListView.Items[selectedIndex].Tag as Team;
            if (selectedTeam == null)
            {
                return;
            }
            var bottomTeam = teamListView.Items[bottomIndex].Tag as Team;
            if (bottomTeam == null)
            {
                return;
            }
            int masterSelectedIndex = _masterTeamList.IndexOf(selectedTeam);
            int masterBottomIndex = _masterTeamList.IndexOf(bottomTeam);
            _masterTeamList.Insert(masterBottomIndex + 1, selectedTeam);
            _masterTeamList.RemoveAt(masterSelectedIndex);
            _narrowedTeamList.Insert(bottomIndex + 1, selectedTeam);
            _narrowedTeamList.RemoveAt(selectedIndex);
            InsertTeamListViewItem(bottomIndex + 1, selectedTeam);
            RemoveTeamListViewItem(selectedIndex);
            teamListView.Items[bottomIndex].Focused = true;
            teamListView.Items[bottomIndex].Selected = true;
            SetDirtyFlag(selectedTeam.CountryTag);
        }

        /// <summary>
        /// 国家リストボックスの項目描画処理
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
                    brush = _dirtyFlags[e.Index + 1]
                                ? new SolidBrush(Color.Red)
                                : new SolidBrush(SystemColors.WindowText);
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

            NarrowTeamList();
            UpdateTeamList();
        }

        /// <summary>
        ///     研究機関リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTeamListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            if (teamListView.SelectedItems.Count == 0)
            {
                return;
            }
            var team = teamListView.SelectedItems[0].Tag as Team;
            if (team == null)
            {
                return;
            }

            countryComboBox.SelectedIndex = (int) team.CountryTag;
            idNumericUpDown.Value = team.Id;
            nameTextBox.Text = team.Name;
            skillNumericUpDown.Value = team.Skill;
            startYearNumericUpDown.Value = team.StartYear;
            endYearNumericUpDown.Value = team.EndYear;
            specialityComboBox1.SelectedIndex = (int) team.Specialities[0];
            specialityComboBox2.SelectedIndex = (int) team.Specialities[1];
            specialityComboBox3.SelectedIndex = (int) team.Specialities[2];
            specialityComboBox4.SelectedIndex = (int) team.Specialities[3];
            specialityComboBox5.SelectedIndex = (int) team.Specialities[4];
            specialityComboBox6.SelectedIndex = (int) team.Specialities[5];

            pictureNameTextBox.Text = team.PictureName;
            if (!string.IsNullOrEmpty(team.PictureName))
            {
                if (Game.IsModActive)
                {
                    string modFileName = Path.Combine(Path.Combine(Game.ModFolderName, "gfx\\interface\\pics"),
                                                      Path.ChangeExtension(team.PictureName, ".bmp"));
                    if (File.Exists(modFileName))
                    {
                        teamPictureBox.ImageLocation = modFileName;
                    }
                    else
                    {
                        teamPictureBox.ImageLocation =
                            Path.Combine(Path.Combine(Game.FolderName, "gfx\\interface\\pics"),
                                         Path.ChangeExtension(team.PictureName, ".bmp"));
                    }
                }
                else
                {
                    teamPictureBox.ImageLocation =
                        Path.Combine(Path.Combine(Game.FolderName, "gfx\\interface\\pics"),
                                     Path.ChangeExtension(team.PictureName, ".bmp"));
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
            if (teamListView.SelectedItems.Count == 0)
            {
                return;
            }
            var team = teamListView.SelectedItems[0].Tag as Team;
            if (team == null)
            {
                return;
            }
            // 値に変化がなければ何もせずに戻る
            var newCountryTag = (CountryTag) countryComboBox.SelectedIndex;
            if (newCountryTag == team.CountryTag)
            {
                return;
            }
            SetDirtyFlag(team.CountryTag);
            team.CountryTag = newCountryTag;
            teamListView.SelectedItems[0].Text = Country.CountryTextTable[(int) team.CountryTag];
            SetDirtyFlag(team.CountryTag);
            
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
            if (teamListView.SelectedItems.Count == 0)
            {
                return;
            }
            var team = teamListView.SelectedItems[0].Tag as Team;
            if (team == null)
            {
                return;
            }
            // 値に変化がなければ何もせずに戻る
            var newId = (int) idNumericUpDown.Value;
            if (newId == team.Id)
            {
                return;
            }
            team.Id = newId;
            teamListView.SelectedItems[0].SubItems[1].Text = team.Id.ToString(CultureInfo.InvariantCulture);
            SetDirtyFlag(team.CountryTag);
        }

        /// <summary>
        ///     名前文字列変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNameTextBoxTextChanged(object sender, EventArgs e)
        {
            if (teamListView.SelectedItems.Count == 0)
            {
                return;
            }
            var team = teamListView.SelectedItems[0].Tag as Team;
            if (team == null)
            {
                return;
            }
            // 値に変化がなければ何もせずに戻る
            string newName = nameTextBox.Text;
            if (newName.Equals(team.Name))
            {
                return;
            }
            team.Name = newName;
            teamListView.SelectedItems[0].SubItems[2].Text = team.Name;
            SetDirtyFlag(team.CountryTag);
        }

        /// <summary>
        ///     スキル変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSkillNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (teamListView.SelectedItems.Count == 0)
            {
                return;
            }
            var team = teamListView.SelectedItems[0].Tag as Team;
            if (team == null)
            {
                return;
            }
            // 値に変化がなければ何もせずに戻る
            var newSkill = (int) skillNumericUpDown.Value;
            if (newSkill == team.Skill)
            {
                return;
            }
            team.Skill = newSkill;
            teamListView.SelectedItems[0].SubItems[3].Text =
                team.Skill.ToString(CultureInfo.InvariantCulture);
            SetDirtyFlag(team.CountryTag);
        }

        /// <summary>
        ///     開始年変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStartYearNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (teamListView.SelectedItems.Count == 0)
            {
                return;
            }
            var team = teamListView.SelectedItems[0].Tag as Team;
            if (team == null)
            {
                return;
            }
            // 値に変化がなければ何もせずに戻る
            var newStartYear = (int) startYearNumericUpDown.Value;
            if (newStartYear == team.StartYear)
            {
                return;
            }
            team.StartYear = newStartYear;
            teamListView.SelectedItems[0].SubItems[4].Text =
                team.StartYear.ToString(CultureInfo.InvariantCulture);
            SetDirtyFlag(team.CountryTag);
        }

        /// <summary>
        ///     終了年変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEndYearNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (teamListView.SelectedItems.Count == 0)
            {
                return;
            }
            var team = teamListView.SelectedItems[0].Tag as Team;
            if (team == null)
            {
                return;
            }
            // 値に変化がなければ何もせずに戻る
            var newEndYear = (int) endYearNumericUpDown.Value;
            if (newEndYear == team.EndYear)
            {
                return;
            }
            team.EndYear = newEndYear;
            teamListView.SelectedItems[0].SubItems[5].Text =
                team.EndYear.ToString(CultureInfo.InvariantCulture);
            SetDirtyFlag(team.CountryTag);
        }

        /// <summary>
        ///     特性1変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpecialityComboBox1SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (teamListView.SelectedItems.Count == 0)
            {
                return;
            }
            var team = teamListView.SelectedItems[0].Tag as Team;
            if (team == null)
            {
                return;
            }
            // 値に変化がなければ何もせずに戻る
            var newSpeciality = (TechSpeciality) specialityComboBox1.SelectedIndex;
            if (newSpeciality == team.Specialities[0])
            {
                return;
            }
            team.Specialities[0] = newSpeciality;
            teamListView.SelectedItems[0].SubItems[6].Text =
                team.Specialities[0] != TechSpeciality.None
                    ? Config.Text[Team.SpecialityTextTable[(int) team.Specialities[0]]]
                    : "";
            SetDirtyFlag(team.CountryTag);
        }

        /// <summary>
        ///     特性2変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpecialityComboBox2SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (teamListView.SelectedItems.Count == 0)
            {
                return;
            }
            var team = teamListView.SelectedItems[0].Tag as Team;
            if (team == null)
            {
                return;
            }
            // 値に変化がなければ何もせずに戻る
            var newSpeciality = (TechSpeciality) specialityComboBox2.SelectedIndex;
            if (newSpeciality == team.Specialities[1])
            {
                return;
            }
            team.Specialities[1] = newSpeciality;
            teamListView.SelectedItems[0].SubItems[7].Text =
                team.Specialities[1] != TechSpeciality.None
                    ? Config.Text[Team.SpecialityTextTable[(int) team.Specialities[1]]]
                    : "";
            SetDirtyFlag(team.CountryTag);
        }

        /// <summary>
        ///     特性3変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpecialityComboBox3SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (teamListView.SelectedItems.Count == 0)
            {
                return;
            }
            var team = teamListView.SelectedItems[0].Tag as Team;
            if (team == null)
            {
                return;
            }
            // 値に変化がなければ何もせずに戻る
            var newSpeciality = (TechSpeciality) specialityComboBox3.SelectedIndex;
            if (newSpeciality == team.Specialities[2])
            {
                return;
            }
            team.Specialities[2] = newSpeciality;
            teamListView.SelectedItems[0].SubItems[8].Text =
                team.Specialities[2] != TechSpeciality.None
                    ? Config.Text[Team.SpecialityTextTable[(int) team.Specialities[2]]]
                    : "";
            SetDirtyFlag(team.CountryTag);
        }

        /// <summary>
        ///     特性4変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpecialityComboBox4SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (teamListView.SelectedItems.Count == 0)
            {
                return;
            }
            var team = teamListView.SelectedItems[0].Tag as Team;
            if (team == null)
            {
                return;
            }
            // 値に変化がなければ何もせずに戻る
            var newSpeciality = (TechSpeciality) specialityComboBox4.SelectedIndex;
            if (newSpeciality == team.Specialities[3])
            {
                return;
            }
            team.Specialities[3] = newSpeciality;
            teamListView.SelectedItems[0].SubItems[9].Text =
                team.Specialities[3] != TechSpeciality.None
                    ? Config.Text[Team.SpecialityTextTable[(int) team.Specialities[3]]]
                    : "";
            SetDirtyFlag(team.CountryTag);
        }

        /// <summary>
        ///     特性5変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpecialityComboBox5SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (teamListView.SelectedItems.Count == 0)
            {
                return;
            }
            var team = teamListView.SelectedItems[0].Tag as Team;
            if (team == null)
            {
                return;
            }
            // 値に変化がなければ何もせずに戻る
            var newSpeciality = (TechSpeciality) specialityComboBox5.SelectedIndex;
            if (newSpeciality == team.Specialities[4])
            {
                return;
            }
            team.Specialities[4] = newSpeciality;
            teamListView.SelectedItems[0].SubItems[10].Text =
                team.Specialities[4] != TechSpeciality.None
                    ? Config.Text[Team.SpecialityTextTable[(int) team.Specialities[4]]]
                    : "";
            SetDirtyFlag(team.CountryTag);
        }

        /// <summary>
        ///     特性6変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpecialityComboBox6SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (teamListView.SelectedItems.Count == 0)
            {
                return;
            }
            var team = teamListView.SelectedItems[0].Tag as Team;
            if (team == null)
            {
                return;
            }
            // 値に変化がなければ何もせずに戻る
            var newSpeciality = (TechSpeciality) specialityComboBox6.SelectedIndex;
            if (newSpeciality == team.Specialities[5])
            {
                return;
            }
            team.Specialities[5] = newSpeciality;
            teamListView.SelectedItems[0].SubItems[11].Text =
                team.Specialities[5] != TechSpeciality.None
                    ? Config.Text[Team.SpecialityTextTable[(int) team.Specialities[5]]]
                    : "";
            SetDirtyFlag(team.CountryTag);
        }

        /// <summary>
        ///     画像ファイル名変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPictureNameTextBoxTextChanged(object sender, EventArgs e)
        {
            if (teamListView.SelectedItems.Count == 0)
            {
                return;
            }
            var team = teamListView.SelectedItems[0].Tag as Team;
            if (team == null)
            {
                return;
            }
            // 値に変化がなければ何もせずに戻る
            string newPictureName = pictureNameTextBox.Text;
            if (newPictureName.Equals(team.PictureName))
            {
                return;
            }
            team.PictureName = newPictureName;
            if (!string.IsNullOrEmpty(team.PictureName))
            {
                if (Game.IsModActive)
                {
                    string modFileName = Path.Combine(Path.Combine(Game.ModFolderName, "gfx\\interface\\pics"),
                                                      Path.ChangeExtension(team.PictureName, ".bmp"));
                    if (File.Exists(modFileName))
                    {
                        teamPictureBox.ImageLocation = modFileName;
                    }
                    else
                    {
                        teamPictureBox.ImageLocation =
                            Path.Combine(Path.Combine(Game.FolderName, "gfx\\interface\\pics"),
                                         Path.ChangeExtension(team.PictureName, ".bmp"));
                    }
                }
                else
                {
                    teamPictureBox.ImageLocation =
                        Path.Combine(Path.Combine(Game.FolderName, "gfx\\interface\\pics"),
                                     Path.ChangeExtension(team.PictureName, ".bmp"));
                }
            }
            SetDirtyFlag(team.CountryTag);
        }

        /// <summary>
        ///     画像ファイル名参照ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPictureNameReferButtonClick(object sender, EventArgs e)
        {
            if (teamListView.SelectedItems.Count == 0)
            {
                return;
            }
            var team = teamListView.SelectedItems[0].Tag as Team;
            if (team == null)
            {
                return;
            }
            var dialog = new OpenFileDialog
                             {
                                 InitialDirectory = Path.Combine(Game.FolderName, "gfx\\interface\\pics"),
                                 FileName = team.PictureName,
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
            // 研究機関リスト絞り込みのため、ダミーでイベント発行する
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
            LoadTeamFiles();
        }

        /// <summary>
        ///     保存ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            Team.SaveTeamFiles(_masterTeamList, _dirtyFlags);
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