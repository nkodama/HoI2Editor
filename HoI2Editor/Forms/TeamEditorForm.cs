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
        ///     絞り込み後の閣僚リスト
        /// </summary>
        private readonly List<Team> _narrowedList = new List<Team>();

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
            // 研究機関ファイルを読み込む
            Teams.LoadTeamFiles();

            // 研究機関リストを絞り込む
            NarrowTeamList();

            // 研究機関リストの表示を更新する
            UpdateTeamList();

            // 編集済みフラグがクリアされるため表示を更新する
            countryListBox.Update();
        }

        /// <summary>
        ///     研究機関ファイルを保存する
        /// </summary>
        private void SaveTeamFiles()
        {
            // 研究機関ファイルを保存する
            Teams.SaveTeamFiles();

            // 編集済みフラグがクリアされるため表示を更新する
            countryListBox.Update();
        }

        /// <summary>
        ///     研究機関リストを国タグで絞り込む
        /// </summary>
        private void NarrowTeamList()
        {
            _narrowedList.Clear();
            List<CountryTag> selectedTagList = countryListBox.SelectedItems.Count == 0
                                                   ? new List<CountryTag>()
                                                   : (from string countryText in countryListBox.SelectedItems
                                                      select Country.CountryStringMap[countryText]).ToList();

            foreach (Team team in Teams.List.Where(team => selectedTagList.Contains(team.CountryTag)))
            {
                _narrowedList.Add(team);
            }
        }

        /// <summary>
        ///     研究機関リストの表示を更新する
        /// </summary>
        private void UpdateTeamList()
        {
            teamListView.BeginUpdate();
            teamListView.Items.Clear();

            foreach (Team team in _narrowedList)
            {
                teamListView.Items.Add(CreateTeamListViewItem(team));
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
        ///     国家コンボボックスの項目を更新する
        /// </summary>
        /// <param name="team">研究機関データ</param>
        private void UpdateCountryComboBox(Team team)
        {
            countryComboBox.BeginUpdate();

            if (team.CountryTag != CountryTag.None)
            {
                if (string.IsNullOrEmpty(countryComboBox.Items[0].ToString()))
                {
                    countryComboBox.Items.RemoveAt(0);
                }
                countryComboBox.SelectedIndex = (int) (team.CountryTag - 1);
            }
            else
            {
                if (!string.IsNullOrEmpty(countryComboBox.Items[0].ToString()))
                {
                    countryComboBox.Items.Insert(0, "");
                }
                countryComboBox.SelectedIndex = 0;
            }

            countryComboBox.EndUpdate();
        }

        /// <summary>
        ///     研究機関画像ピクチャーボックスの項目を更新する
        /// </summary>
        /// <param name="team">研究機関データ</param>
        private void UpdateTeamPicture(Team team)
        {
            if (!string.IsNullOrEmpty(team.PictureName))
            {
                string fileName =
                    Game.GetReadFileName(Path.Combine(Game.PersonPicturePathName,
                                                      Path.ChangeExtension(team.PictureName, ".bmp")));
                teamPictureBox.ImageLocation = File.Exists(fileName) ? fileName : "";
            }
            else
            {
                teamPictureBox.ImageLocation = "";
            }
        }

        /// <summary>
        ///     研究特性を初期化する
        /// </summary>
        private static void InitSpecialities()
        {
            // ゲームの種類に合わせて研究特性を初期化する
            Techs.InitSpecialities();

            // 研究特性画像リストを初期化する
            Techs.InitSpecialityImages();
        }

        /// <summary>
        ///     編集項目を初期化する
        /// </summary>
        private void InitEditableItems()
        {
            // 国タグ
            int maxWidth = countryComboBox.DropDownWidth;
            foreach (string s in Country.CountryTextTable.Select(
                country =>
                Config.ExistsKey(country) ? string.Format("{0} {1}", country, Config.GetText(country)) : country))
            {
                countryComboBox.Items.Add(s);
                maxWidth = Math.Max(maxWidth,
                                    TextRenderer.MeasureText(s, countryComboBox.Font).Width +
                                    SystemInformation.VerticalScrollBarWidth);
            }
            countryComboBox.DropDownWidth = maxWidth;

            // 特性
            maxWidth = specialityComboBox1.DropDownWidth;
            foreach (
                string name in
                    Techs.SpecialityTable.Select(
                        speciality => Config.GetText(Tech.SpecialityNameTable[(int) speciality])))
            {
                specialityComboBox1.Items.Add(name);
                specialityComboBox2.Items.Add(name);
                specialityComboBox3.Items.Add(name);
                specialityComboBox4.Items.Add(name);
                specialityComboBox5.Items.Add(name);
                specialityComboBox6.Items.Add(name);
                // +24は特性アイコンの分
                maxWidth = Math.Max(maxWidth,
                                    TextRenderer.MeasureText(name, specialityComboBox1.Font).Width +
                                    SystemInformation.VerticalScrollBarWidth + 24);
            }
            specialityComboBox1.DropDownWidth = maxWidth;
            specialityComboBox2.DropDownWidth = maxWidth;
            specialityComboBox3.DropDownWidth = maxWidth;
            specialityComboBox4.DropDownWidth = maxWidth;
            specialityComboBox5.DropDownWidth = maxWidth;
            specialityComboBox6.DropDownWidth = maxWidth;
        }

        /// <summary>
        ///     国家リストボックスを初期化する
        /// </summary>
        private void InitCountryList()
        {
            foreach (string country in Country.CountryTextTable.Where(country => !string.IsNullOrEmpty(country)))
            {
                countryListBox.Items.Add(country);
            }
            countryListBox.SelectedIndex = 0;
        }

        /// <summary>
        ///     研究機関リストに項目を追加する
        /// </summary>
        /// <param name="target">追加対象の項目</param>
        private void AddListItem(Team target)
        {
            _narrowedList.Add(target);

            teamListView.Items.Add(CreateTeamListViewItem(target));

            teamListView.Items[0].Focused = true;
            teamListView.Items[0].Selected = true;
        }

        /// <summary>
        ///     研究機関リストに項目を挿入する
        /// </summary>
        /// <param name="target">挿入対象の項目</param>
        /// <param name="index">挿入先の位置</param>
        private void InsertListItem(Team target, int index)
        {
            _narrowedList.Insert(index, target);

            teamListView.Items.Insert(index, CreateTeamListViewItem(target));

            teamListView.Items[index].Focused = true;
            teamListView.Items[index].Selected = true;
            teamListView.Items[index].EnsureVisible();
        }

        /// <summary>
        ///     研究機関リストから項目を削除する
        /// </summary>
        /// <param name="index">削除対象の位置</param>
        private void RemoveItem(int index)
        {
            _narrowedList.RemoveAt(index);

            teamListView.Items.RemoveAt(index);

            if (index < teamListView.Items.Count)
            {
                teamListView.Items[index].Focused = true;
                teamListView.Items[index].Selected = true;
            }
            else if (index - 1 >= 0)
            {
                teamListView.Items[index - 1].Focused = true;
                teamListView.Items[index - 1].Selected = true;
            }
        }

        /// <summary>
        ///     研究機関リストの項目を移動する
        /// </summary>
        /// <param name="target">移動対象の位置</param>
        /// <param name="position">移動先の位置</param>
        private void MoveListItem(int target, int position)
        {
            Team team = _narrowedList[target];

            if (target > position)
            {
                // 上へ移動する場合
                _narrowedList.Insert(position, team);
                _narrowedList.RemoveAt(target + 1);

                teamListView.Items.Insert(position, CreateTeamListViewItem(team));
                teamListView.Items.RemoveAt(target + 1);
            }
            else
            {
                // 下へ移動する場合
                _narrowedList.Insert(position + 1, team);
                _narrowedList.RemoveAt(target);

                teamListView.Items.Insert(position + 1, CreateTeamListViewItem(team));
                teamListView.Items.RemoveAt(target);
            }

            teamListView.Items[position].Focused = true;
            teamListView.Items[position].Selected = true;
            teamListView.Items[position].EnsureVisible();
        }

        /// <summary>
        ///     研究機関リストビューの項目を作成する
        /// </summary>
        /// <param name="team">研究機関データ</param>
        /// <returns>研究機関リストビューの項目</returns>
        private static ListViewItem CreateTeamListViewItem(Team team)
        {
            if (team == null)
            {
                return null;
            }

            var item = new ListViewItem
                           {
                               Text = Country.CountryTextTable[(int) team.CountryTag],
                               Tag = team
                           };
            item.SubItems.Add(team.Id.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(team.Name);
            item.SubItems.Add(team.Skill.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(team.StartYear.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(team.EndYear.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add("");

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
            removeButton.Enabled = true;
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
            removeButton.Enabled = false;
            topButton.Enabled = false;
            upButton.Enabled = false;
            downButton.Enabled = false;
            bottomButton.Enabled = false;
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTeamEditorFormLoad(object sender, EventArgs e)
        {
            // 研究機関リストビューの高さを設定するためにダミーのイメージリストを作成する
            teamListView.SmallImageList = new ImageList {ImageSize = new Size(1, 18)};

            InitSpecialities();
            InitEditableItems();
            InitCountryList();
            LoadTeamFiles();
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
                var selected = teamListView.SelectedItems[0].Tag as Team;
                if (selected == null)
                {
                    return;
                }

                team = new Team
                           {
                               CountryTag = selected.CountryTag,
                               Id = selected.Id + 1,
                               Skill = 1,
                               StartYear = 1930,
                               EndYear = 1970,
                           };

                Teams.InsertItemNext(team, selected);

                InsertListItem(team, teamListView.SelectedIndices[0] + 1);
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

                Teams.AddItem(team);

                AddListItem(team);

                EnableEditableItems();
            }

            Teams.SetDirtyFlag(team.CountryTag);
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

            var selected = teamListView.SelectedItems[0].Tag as Team;
            if (selected == null)
            {
                return;
            }

            var team = new Team
                           {
                               CountryTag = selected.CountryTag,
                               Id = selected.Id + 1,
                               Name = selected.Name,
                               StartYear = selected.StartYear,
                               EndYear = selected.EndYear,
                               PictureName = selected.PictureName,
                           };
            for (int i = 0; i < Team.SpecialityLength; i++)
            {
                team.Specialities[i] = selected.Specialities[i];
            }

            Teams.InsertItemNext(team, selected);

            InsertListItem(team, teamListView.SelectedIndices[0] + 1);

            Teams.SetDirtyFlag(team.CountryTag);
        }

        /// <summary>
        ///     削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRemoveButtonClick(object sender, EventArgs e)
        {
            if (teamListView.SelectedItems.Count == 0)
            {
                return;
            }

            var selected = teamListView.SelectedItems[0].Tag as Team;
            if (selected == null)
            {
                return;
            }

            Teams.RemoveItem(selected);

            RemoveItem(teamListView.SelectedIndices[0]);

            if (teamListView.Items.Count == 0)
            {
                DisableEditableItems();
            }

            Teams.SetDirtyFlag(selected.CountryTag);
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

            // 選択項目がリストの先頭ならば何もしない
            int index = teamListView.SelectedIndices[0];
            if (index == 0)
            {
                return;
            }

            var selected = teamListView.SelectedItems[0].Tag as Team;
            if (selected == null)
            {
                return;
            }

            var top = teamListView.Items[0].Tag as Team;
            if (top == null)
            {
                return;
            }

            Teams.MoveItem(selected, top);

            MoveListItem(index, 0);

            Teams.SetDirtyFlag(selected.CountryTag);
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
            int index = teamListView.SelectedIndices[0];
            if (index == 0)
            {
                return;
            }

            var selected = teamListView.SelectedItems[0].Tag as Team;
            if (selected == null)
            {
                return;
            }

            var upper = teamListView.Items[index - 1].Tag as Team;
            if (upper == null)
            {
                return;
            }

            Teams.MoveItem(selected, upper);

            MoveListItem(index, index - 1);

            Teams.SetDirtyFlag(selected.CountryTag);
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
            int index = teamListView.SelectedIndices[0];
            if (index == teamListView.Items.Count - 1)
            {
                return;
            }

            var selected = teamListView.SelectedItems[0].Tag as Team;
            if (selected == null)
            {
                return;
            }

            var lower = teamListView.Items[index + 1].Tag as Team;
            if (lower == null)
            {
                return;
            }

            Teams.MoveItem(selected, lower);

            MoveListItem(index, index + 1);

            Teams.SetDirtyFlag(selected.CountryTag);
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

            // 選択項目がリストの末尾ならば何もしない
            int index = teamListView.SelectedIndices[0];
            if (teamListView.SelectedIndices[0] == teamListView.Items.Count - 1)
            {
                return;
            }

            var selected = teamListView.Items[index].Tag as Team;
            if (selected == null)
            {
                return;
            }

            var bottom = teamListView.Items[teamListView.Items.Count - 1].Tag as Team;
            if (bottom == null)
            {
                return;
            }

            Teams.MoveItem(selected, bottom);

            MoveListItem(index, teamListView.Items.Count - 1);

            Teams.SetDirtyFlag(selected.CountryTag);
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
                    brush = Teams.DirtyFlags[e.Index + 1]
                                ? new SolidBrush(Color.Red)
                                : new SolidBrush(countryListBox.ForeColor);
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
        ///     研究特性コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpecialityComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 背景を描画する
            e.DrawBackground();

            var combobox = sender as ComboBox;
            if (combobox != null && e.Index > 0)
            {
                if (e.Index - 1 < Techs.SpecialityImages.Images.Count)
                {
                    var gr = new Rectangle(e.Bounds.X + 1, e.Bounds.Y + 1, 16, 16);
                    e.Graphics.DrawImage(Techs.SpecialityImages.Images[e.Index - 1], gr);
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
        ///     研究機関リストビューのサブ項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTeamListViewDrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case 6:
                    e.Graphics.FillRectangle(
                        teamListView.SelectedIndices.Count > 0 && e.ItemIndex == teamListView.SelectedIndices[0]
                            ? (teamListView.Focused ? SystemBrushes.Highlight : SystemBrushes.Control)
                            : SystemBrushes.Window, e.Bounds);
                    DrawTechSpecialityIcon(e, teamListView.Items[e.ItemIndex].Tag as Team);
                    break;

                default:
                    e.DrawDefault = true;
                    break;
            }
        }

        /// <summary>
        ///     研究機関リストビューの研究特性アイコン描画処理
        /// </summary>
        /// <param name="e"></param>
        /// <param name="team">研究機関データ</param>
        private static void DrawTechSpecialityIcon(DrawListViewSubItemEventArgs e, Team team)
        {
            if (team == null)
            {
                return;
            }

            var rect = new Rectangle(e.Bounds.X + 4, e.Bounds.Y + 1, 16, 16);
            for (int i = 0; i < Team.SpecialityLength; i++)
            {
                if (team.Specialities[i] != TechSpeciality.None)
                {
                    e.Graphics.DrawImage(Techs.SpecialityImages.Images[(int) team.Specialities[i] - 1], rect);
                    rect.X += 19;
                }
            }
        }

        /// <summary>
        ///     研究機関リストビューの列ヘッダ描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTeamListViewDrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
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

            UpdateCountryComboBox(team);
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
            UpdateTeamPicture(team);

            // 項目移動ボタンの状態更新
            topButton.Enabled = teamListView.SelectedIndices[0] != 0;
            upButton.Enabled = teamListView.SelectedIndices[0] != 0;
            downButton.Enabled = teamListView.SelectedIndices[0] != teamListView.Items.Count - 1;
            bottomButton.Enabled = teamListView.SelectedIndices[0] != teamListView.Items.Count - 1;
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
            CountryTag newCountryTag = !string.IsNullOrEmpty(countryComboBox.Items[0].ToString())
                                           ? (CountryTag) (countryComboBox.SelectedIndex + 1)
                                           : (CountryTag) countryComboBox.SelectedIndex;
            if (newCountryTag == team.CountryTag)
            {
                return;
            }

            Teams.SetDirtyFlag(team.CountryTag);

            team.CountryTag = newCountryTag;
            teamListView.SelectedItems[0].Text = Country.CountryTextTable[(int) team.CountryTag];

            UpdateCountryComboBox(team);

            Teams.SetDirtyFlag(team.CountryTag);

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

            Teams.SetDirtyFlag(team.CountryTag);
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

            Teams.SetDirtyFlag(team.CountryTag);
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

            Teams.SetDirtyFlag(team.CountryTag);
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

            Teams.SetDirtyFlag(team.CountryTag);
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

            Teams.SetDirtyFlag(team.CountryTag);
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

            UpdateTeamList();

            Teams.SetDirtyFlag(team.CountryTag);
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

            UpdateTeamList();

            Teams.SetDirtyFlag(team.CountryTag);
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

            UpdateTeamList();

            Teams.SetDirtyFlag(team.CountryTag);
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

            UpdateTeamList();

            Teams.SetDirtyFlag(team.CountryTag);
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

            UpdateTeamList();

            Teams.SetDirtyFlag(team.CountryTag);
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

            UpdateTeamList();

            Teams.SetDirtyFlag(team.CountryTag);
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
            UpdateTeamPicture(team);

            Teams.SetDirtyFlag(team.CountryTag);
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
                                 InitialDirectory = Path.Combine(Game.FolderName, Game.PersonPicturePathName),
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
            Teams.RequireReload();
            LoadTeamFiles();
        }

        /// <summary>
        ///     保存ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            SaveTeamFiles();
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