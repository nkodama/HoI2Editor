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
        #region 内部フィールド

        /// <summary>
        ///     絞り込み後の閣僚リスト
        /// </summary>
        private readonly List<Team> _list = new List<Team>();

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
            Skill,
            StartYear,
            EndYear,
            Speciality,
        }

        /// <summary>
        ///     ソート順
        /// </summary>
        private enum SortOrder
        {
            Ascendant,
            Decendant,
        }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public TeamEditorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTeamEditorFormLoad(object sender, EventArgs e)
        {
            // 国家データを初期化する
            Country.Init();

            // 研究特性を初期化する
            Techs.InitSpecialities();

            // ゲーム設定ファイルを読み込む
            Misc.Load();

            // 文字列定義ファイルを読み込む
            Config.Load();

            // 研究機関リストビューの高さを設定するためにダミーのイメージリストを作成する
            teamListView.SmallImageList = new ImageList {ImageSize = new Size(1, 18)};

            // 編集項目を初期化する
            InitEditableItems();

            // 国家リストボックスを初期化する
            InitCountryListBox();

            // 研究機関ファイルを読み込む
            LoadFiles();
        }

        #endregion

        #region 終了処理

        /// <summary>
        ///     閉じるボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloseButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        ///     フォームクローズ時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTeamEditorFormClosing(object sender, FormClosingEventArgs e)
        {
            // 編集済みでなければフォームを閉じる
            if (!Teams.IsDirty())
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
                    SaveFiles();
                    break;
            }
        }

        #endregion

        #region 研究機関データ処理

        /// <summary>
        ///     再読み込みボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReloadButtonClick(object sender, EventArgs e)
        {
            // 研究機関ファイルの再読み込みを要求する
            Teams.RequireReload();

            // 研究機関ファイルを読み込む
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
        ///     研究機関ファイルを読み込む
        /// </summary>
        private void LoadFiles()
        {
            // 研究機関ファイルを読み込む
            Teams.Load();

            // 研究機関リストを絞り込む
            NarrowTeamList();

            // 研究機関リストをソートする
            SortTeamList();

            // 研究機関リストの表示を更新する
            UpdateTeamList();

            // 編集済みフラグがクリアされるため表示を更新する
            countryListBox.Refresh();
        }

        /// <summary>
        ///     研究機関ファイルを保存する
        /// </summary>
        private void SaveFiles()
        {
            // 研究機関ファイルを保存する
            Teams.Save();

            // 編集済みフラグがクリアされるため表示を更新する
            countryListBox.Refresh();
            UpdateEditableItems();
        }

        #endregion

        #region 研究機関リストビュー

        /// <summary>
        ///     研究機関リストの表示を更新する
        /// </summary>
        private void UpdateTeamList()
        {
            teamListView.BeginUpdate();
            teamListView.Items.Clear();

            // 項目を順に登録する
            foreach (Team team in _list)
            {
                teamListView.Items.Add(CreateTeamListViewItem(team));
            }

            if (teamListView.Items.Count > 0)
            {
                // 先頭の項目を選択する
                teamListView.Items[0].Focused = true;
                teamListView.Items[0].Selected = true;

                // 編集項目を有効化する
                EnableEditableItems();
            }
            else
            {
                // 編集項目を無効化する
                DisableEditableItems();
            }

            teamListView.EndUpdate();
        }

        /// <summary>
        ///     研究機関リストを国タグで絞り込む
        /// </summary>
        private void NarrowTeamList()
        {
            _list.Clear();

            // 選択中の国家リストを作成する
            List<CountryTag> selectedTagList = countryListBox.SelectedItems.Count == 0
                                                   ? new List<CountryTag>()
                                                   : (from string countryText in countryListBox.SelectedItems
                                                      select Country.StringMap[countryText]).ToList();

            // 選択中の国家に所属する指揮官を順に絞り込む
            foreach (Team team in Teams.Items.Where(team => selectedTagList.Contains(team.Country)))
            {
                _list.Add(team);
            }
        }

        /// <summary>
        ///     研究機関リストをソートする
        /// </summary>
        private void SortTeamList()
        {
            switch (_key)
            {
                case SortKey.None: // ソートなし
                    break;

                case SortKey.Tag: // 国タグ
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((team1, team2) => team1.Country - team2.Country);
                    }
                    else
                    {
                        _list.Sort((team1, team2) => team2.Country - team1.Country);
                    }
                    break;

                case SortKey.Id: // ID
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((team1, team2) => team1.Id - team2.Id);
                    }
                    else
                    {
                        _list.Sort((team1, team2) => team2.Id - team1.Id);
                    }
                    break;

                case SortKey.Name: // 名前
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((team1, team2) => string.CompareOrdinal(team1.Name, team2.Name));
                    }
                    else
                    {
                        _list.Sort((team1, team2) => string.CompareOrdinal(team2.Name, team1.Name));
                    }
                    break;

                case SortKey.Skill: // スキル
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((team1, team2) => team1.Skill - team2.Skill);
                    }
                    else
                    {
                        _list.Sort((team1, team2) => team2.Skill - team1.Skill);
                    }
                    break;

                case SortKey.StartYear: // 開始年
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((team1, team2) => team1.StartYear - team2.StartYear);
                    }
                    else
                    {
                        _list.Sort((team1, team2) => team2.StartYear - team1.StartYear);
                    }
                    break;

                case SortKey.EndYear: // 終了年
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((team1, team2) => team1.EndYear - team2.EndYear);
                    }
                    else
                    {
                        _list.Sort((team1, team2) => team2.EndYear - team1.EndYear);
                    }
                    break;

                case SortKey.Speciality: // 特性
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((team1, team2) =>
                            {
                                if (team1.Specialities[0] > team2.Specialities[0])
                                {
                                    return 1;
                                }
                                if (team1.Specialities[0] < team2.Specialities[0])
                                {
                                    return -1;
                                }
                                if (team1.Specialities[1] > team2.Specialities[1])
                                {
                                    return 1;
                                }
                                if (team1.Specialities[1] < team2.Specialities[1])
                                {
                                    return -1;
                                }
                                if (team1.Specialities[2] > team2.Specialities[2])
                                {
                                    return 1;
                                }
                                if (team1.Specialities[2] < team2.Specialities[2])
                                {
                                    return -1;
                                }
                                if (team1.Specialities[3] > team2.Specialities[3])
                                {
                                    return 1;
                                }
                                if (team1.Specialities[3] < team2.Specialities[3])
                                {
                                    return -1;
                                }
                                if (team1.Specialities[4] > team2.Specialities[4])
                                {
                                    return 1;
                                }
                                if (team1.Specialities[4] < team2.Specialities[4])
                                {
                                    return -1;
                                }
                                if (team1.Specialities[5] > team2.Specialities[5])
                                {
                                    return 1;
                                }
                                if (team1.Specialities[5] < team2.Specialities[5])
                                {
                                    return -1;
                                }
                                if (team1.Specialities[6] > team2.Specialities[6])
                                {
                                    return 1;
                                }
                                if (team1.Specialities[6] < team2.Specialities[6])
                                {
                                    return -1;
                                }
                                return 0;
                            });
                    }
                    else
                    {
                        _list.Sort((team1, team2) =>
                            {
                                if (team1.Specialities[0] < team2.Specialities[0])
                                {
                                    return 1;
                                }
                                if (team1.Specialities[0] > team2.Specialities[0])
                                {
                                    return -1;
                                }
                                if (team1.Specialities[1] < team2.Specialities[1])
                                {
                                    return 1;
                                }
                                if (team1.Specialities[1] > team2.Specialities[1])
                                {
                                    return -1;
                                }
                                if (team1.Specialities[2] < team2.Specialities[2])
                                {
                                    return 1;
                                }
                                if (team1.Specialities[2] > team2.Specialities[2])
                                {
                                    return -1;
                                }
                                if (team1.Specialities[3] < team2.Specialities[3])
                                {
                                    return 1;
                                }
                                if (team1.Specialities[3] > team2.Specialities[3])
                                {
                                    return -1;
                                }
                                if (team1.Specialities[4] < team2.Specialities[4])
                                {
                                    return 1;
                                }
                                if (team1.Specialities[4] > team2.Specialities[4])
                                {
                                    return -1;
                                }
                                if (team1.Specialities[5] < team2.Specialities[5])
                                {
                                    return 1;
                                }
                                if (team1.Specialities[5] > team2.Specialities[5])
                                {
                                    return -1;
                                }
                                if (team1.Specialities[6] < team2.Specialities[6])
                                {
                                    return 1;
                                }
                                if (team1.Specialities[6] > team2.Specialities[6])
                                {
                                    return -1;
                                }
                                return 0;
                            });
                    }
                    break;
            }
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
                case 6: // 研究特性
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
                // 研究特性なしならば何もしない
                if (team.Specialities[i] == TechSpeciality.None)
                {
                    continue;
                }

                // 研究特性アイコンを描画する
                e.Graphics.DrawImage(Techs.SpecialityImages.Images[(int) team.Specialities[i] - 1], rect);
                rect.X += 19;
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
        ///     研究機関リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTeamListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 編集項目を更新する
            UpdateEditableItems();
        }

        /// <summary>
        ///     研究機関リストビューのカラムクリック時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLeaderListViewColumnClick(object sender, ColumnClickEventArgs e)
        {
            switch (e.Column)
            {
                case 0: // 国タグ
                    if (_key == SortKey.Tag)
                    {
                        _order = (_order == SortOrder.Ascendant) ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.Tag;
                    }
                    break;

                case 1: // ID
                    if (_key == SortKey.Id)
                    {
                        _order = (_order == SortOrder.Ascendant) ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.Id;
                    }
                    break;

                case 2: // 名前
                    if (_key == SortKey.Name)
                    {
                        _order = (_order == SortOrder.Ascendant) ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.Name;
                    }
                    break;

                case 3: // スキル
                    if (_key == SortKey.Skill)
                    {
                        _order = (_order == SortOrder.Ascendant) ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.Skill;
                    }
                    break;

                case 4: // 開始年
                    if (_key == SortKey.StartYear)
                    {
                        _order = (_order == SortOrder.Ascendant) ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.StartYear;
                    }
                    break;

                case 5: // 終了年
                    if (_key == SortKey.EndYear)
                    {
                        _order = (_order == SortOrder.Ascendant) ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.EndYear;
                    }
                    break;

                case 6: // 特性
                    if (_key == SortKey.Speciality)
                    {
                        _order = (_order == SortOrder.Ascendant) ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.Speciality;
                    }
                    break;

                default:
                    // 項目のない列をクリックした時には何もしない
                    return;
            }

            // 研究機関リストをソートする
            SortTeamList();

            // 研究機関リストを更新する
            UpdateTeamList();
        }

        /// <summary>
        ///     新規ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNewButtonClick(object sender, EventArgs e)
        {
            Team team;
            Team selected = GetSelectedTeam();
            if (selected != null)
            {
                // 選択項目がある場合、国タグやIDを引き継いで項目を作成する
                team = new Team
                    {
                        Country = selected.Country,
                        Id = selected.Id + 1,
                        Skill = 1,
                        StartYear = 1930,
                        EndYear = 1970,
                    };

                // 研究機関ごとの編集済みフラグを設定する
                team.SetDirtyAll();

                // 研究機関リストに項目を挿入する
                Teams.InsertItem(team, selected);
                InsertListItem(team, teamListView.SelectedIndices[0] + 1);
            }
            else
            {
                // 新規項目を作成する
                team = new Team
                    {
                        Country =
                            countryListBox.SelectedItems.Count > 0
                                ? (CountryTag) (countryListBox.SelectedIndex + 1)
                                : CountryTag.None,
                        Id = 0,
                        Skill = 1,
                        StartYear = 1930,
                        EndYear = 1970,
                    };

                // 研究機関ごとの編集済みフラグを設定する
                team.SetDirtyAll();

                // 研究機関リストに項目を追加する
                AddListItem(team);

                // 編集項目を有効化する
                EnableEditableItems();
            }

            // 国家ごとの編集済みフラグを設定する
            Teams.SetDirty(team.Country);
        }

        /// <summary>
        ///     複製ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloneButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Team selected = GetSelectedTeam();
            if (selected == null)
            {
                return;
            }

            // 選択項目を引き継いで項目を作成する
            var team = new Team
                {
                    Country = selected.Country,
                    Id = selected.Id + 1,
                    Name = selected.Name,
                    Skill = selected.Skill,
                    StartYear = selected.StartYear,
                    EndYear = selected.EndYear,
                    PictureName = selected.PictureName,
                };
            for (int i = 0; i < Team.SpecialityLength; i++)
            {
                team.Specialities[i] = selected.Specialities[i];
            }

            // 研究機関ごとの編集済みフラグを設定する
            team.SetDirtyAll();

            // 研究機関リストに項目を挿入する
            Teams.InsertItem(team, selected);
            InsertListItem(team, teamListView.SelectedIndices[0] + 1);

            // 国家ごとの編集済みフラグを設定する
            Teams.SetDirty(team.Country);
        }

        /// <summary>
        ///     削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRemoveButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Team selected = GetSelectedTeam();
            if (selected == null)
            {
                return;
            }

            // 研究機関リストから項目を削除する
            Teams.RemoveItem(selected);
            RemoveItem(teamListView.SelectedIndices[0]);

            // リストから項目がなくなれば編集項目を無効化する
            if (teamListView.Items.Count == 0)
            {
                DisableEditableItems();
            }

            // 編集済みフラグを設定する
            Teams.SetDirty(selected.Country);
        }

        /// <summary>
        ///     先頭へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTopButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Team selected = GetSelectedTeam();
            if (selected == null)
            {
                return;
            }

            // 選択項目がリストの先頭ならば何もしない
            int index = teamListView.SelectedIndices[0];
            if (index == 0)
            {
                return;
            }

            var top = teamListView.Items[0].Tag as Team;
            if (top == null)
            {
                return;
            }

            // 研究機関リストの項目を移動する
            Teams.MoveItem(selected, top);
            MoveListItem(index, 0);

            // 編集済みフラグを設定する
            Teams.SetDirty(selected.Country);
        }

        /// <summary>
        ///     上へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Team selected = GetSelectedTeam();
            if (selected == null)
            {
                return;
            }

            // 選択項目がリストの先頭ならば何もしない
            int index = teamListView.SelectedIndices[0];
            if (index == 0)
            {
                return;
            }

            var upper = teamListView.Items[index - 1].Tag as Team;
            if (upper == null)
            {
                return;
            }

            // 研究機関リストの項目を移動する
            Teams.MoveItem(selected, upper);
            MoveListItem(index, index - 1);

            // 編集済みフラグを設定する
            Teams.SetDirty(selected.Country);
        }

        /// <summary>
        ///     下へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDownButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Team selected = GetSelectedTeam();
            if (selected == null)
            {
                return;
            }

            // 選択項目がリストの末尾ならば何もしない
            int index = teamListView.SelectedIndices[0];
            if (index == teamListView.Items.Count - 1)
            {
                return;
            }

            var lower = teamListView.Items[index + 1].Tag as Team;
            if (lower == null)
            {
                return;
            }

            // 研究機関リストの項目を移動する
            Teams.MoveItem(selected, lower);
            MoveListItem(index, index + 1);

            // 編集済みフラグを設定する
            Teams.SetDirty(selected.Country);
        }

        /// <summary>
        ///     末尾へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBottomButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Team selected = GetSelectedTeam();
            if (selected == null)
            {
                return;
            }

            // 選択項目がリストの末尾ならば何もしない
            int index = teamListView.SelectedIndices[0];
            if (teamListView.SelectedIndices[0] == teamListView.Items.Count - 1)
            {
                return;
            }

            var bottom = teamListView.Items[teamListView.Items.Count - 1].Tag as Team;
            if (bottom == null)
            {
                return;
            }

            // 研究機関リストの項目を移動する
            Teams.MoveItem(selected, bottom);
            MoveListItem(index, teamListView.Items.Count - 1);

            // 編集済みフラグを設定する
            Teams.SetDirty(selected.Country);
        }

        /// <summary>
        ///     研究機関リストに項目を追加する
        /// </summary>
        /// <param name="team">追加対象の項目</param>
        private void AddListItem(Team team)
        {
            // 絞り込みリストに項目を追加する
            _list.Add(team);

            // 研究機関リストビューに項目を追加する
            teamListView.Items.Add(CreateTeamListViewItem(team));

            // 追加した項目を選択する
            teamListView.Items[teamListView.Items.Count - 1].Focused = true;
            teamListView.Items[teamListView.Items.Count - 1].Selected = true;
            teamListView.EnsureVisible(teamListView.Items.Count - 1);
        }

        /// <summary>
        ///     研究機関リストに項目を挿入する
        /// </summary>
        /// <param name="team">挿入対象の項目</param>
        /// <param name="index">挿入先の位置</param>
        private void InsertListItem(Team team, int index)
        {
            // 絞り込みリストに項目を挿入する
            _list.Insert(index, team);

            // 研究機関リストビューに項目を挿入する
            teamListView.Items.Insert(index, CreateTeamListViewItem(team));

            // 挿入した項目を選択する
            teamListView.Items[index].Focused = true;
            teamListView.Items[index].Selected = true;
            teamListView.EnsureVisible(index);
        }

        /// <summary>
        ///     研究機関リストから項目を削除する
        /// </summary>
        /// <param name="index">削除対象の位置</param>
        private void RemoveItem(int index)
        {
            // 絞り込みリストから項目を削除する
            _list.RemoveAt(index);

            // 閣僚リストビューから項目を削除する
            teamListView.Items.RemoveAt(index);

            // 削除した項目の次の項目を選択する
            if (index < teamListView.Items.Count)
            {
                teamListView.Items[index].Focused = true;
                teamListView.Items[index].Selected = true;
            }
            else if (index - 1 >= 0)
            {
                // リストの末尾ならば、削除した項目の前の項目を選択する
                teamListView.Items[index - 1].Focused = true;
                teamListView.Items[index - 1].Selected = true;
            }
        }

        /// <summary>
        ///     研究機関リストの項目を移動する
        /// </summary>
        /// <param name="src">移動元の位置</param>
        /// <param name="dest">移動先の位置</param>
        private void MoveListItem(int src, int dest)
        {
            Team team = _list[src];

            if (src > dest)
            {
                // 上へ移動する場合
                // 絞り込みリストの項目を移動する
                _list.Insert(dest, team);
                _list.RemoveAt(src + 1);

                // 閣僚リストビューの項目を移動する
                teamListView.Items.Insert(dest, CreateTeamListViewItem(team));
                teamListView.Items.RemoveAt(src + 1);
            }
            else
            {
                // 下へ移動する場合
                // 絞り込みリストの項目を移動する
                _list.Insert(dest + 1, team);
                _list.RemoveAt(src);

                // 閣僚リストビューの項目を移動する
                teamListView.Items.Insert(dest + 1, CreateTeamListViewItem(team));
                teamListView.Items.RemoveAt(src);
            }

            // 移動先の項目を選択する
            teamListView.Items[dest].Focused = true;
            teamListView.Items[dest].Selected = true;
            teamListView.EnsureVisible(dest);
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
                    Text = Country.Strings[(int) team.Country],
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
        ///     選択中の研究機関データを取得する
        /// </summary>
        /// <returns>選択中の研究機関データ</returns>
        private Team GetSelectedTeam()
        {
            // 選択項目がない場合
            if (teamListView.SelectedItems.Count == 0)
            {
                return null;
            }

            return teamListView.SelectedItems[0].Tag as Team;
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
                brush = Teams.IsDirty(country)
                            ? new SolidBrush(Color.Red)
                            : new SolidBrush(countryListBox.ForeColor);
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

            // 研究機関リストを更新する
            NarrowTeamList();
            UpdateTeamList();
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

            // 研究特性
            specialityComboBox1.Items.Clear();
            specialityComboBox2.Items.Clear();
            specialityComboBox3.Items.Clear();
            specialityComboBox4.Items.Clear();
            specialityComboBox5.Items.Clear();
            maxWidth = specialityComboBox1.DropDownWidth;
            foreach (string s in Techs.Specialities.Select(Techs.GetSpecialityName))
            {
                specialityComboBox1.Items.Add(s);
                specialityComboBox2.Items.Add(s);
                specialityComboBox3.Items.Add(s);
                specialityComboBox4.Items.Add(s);
                specialityComboBox5.Items.Add(s);
                specialityComboBox6.Items.Add(s);
                // +24は研究特性アイコンの分
                maxWidth = Math.Max(maxWidth,
                                    TextRenderer.MeasureText(s, specialityComboBox1.Font).Width +
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
        ///     編集項目を更新する
        /// </summary>
        private void UpdateEditableItems()
        {
            // 選択項目がなければ何もしない
            Team team = GetSelectedTeam();
            if (team == null)
            {
                return;
            }

            // 編集項目を更新する
            UpdateEditableItemsValue(team);

            // 編集項目の色を更新する
            UpdateEditableItemsColor(team);

            // 項目移動ボタンの状態更新
            topButton.Enabled = teamListView.SelectedIndices[0] != 0;
            upButton.Enabled = teamListView.SelectedIndices[0] != 0;
            downButton.Enabled = teamListView.SelectedIndices[0] != teamListView.Items.Count - 1;
            bottomButton.Enabled = teamListView.SelectedIndices[0] != teamListView.Items.Count - 1;
        }

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="team">研究機関データ</param>
        private void UpdateEditableItemsValue(Team team)
        {
            countryComboBox.SelectedIndex = team.Country != CountryTag.None ? (int) team.Country - 1 : -1;
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
        }

        /// <summary>
        ///     編集項目の色を更新する
        /// </summary>
        /// <param name="team">研究機関データ</param>
        private void UpdateEditableItemsColor(Team team)
        {
            // コンボボックスの色を更新する
            countryComboBox.Refresh();
            specialityComboBox1.Refresh();
            specialityComboBox2.Refresh();
            specialityComboBox3.Refresh();
            specialityComboBox4.Refresh();
            specialityComboBox5.Refresh();
            specialityComboBox6.Refresh();

            // 編集項目の色を更新する
            idNumericUpDown.ForeColor = team.IsDirty(TeamItemId.Id) ? Color.Red : SystemColors.WindowText;
            nameTextBox.ForeColor = team.IsDirty(TeamItemId.Name) ? Color.Red : SystemColors.WindowText;
            skillNumericUpDown.ForeColor = team.IsDirty(TeamItemId.Skill) ? Color.Red : SystemColors.WindowText;
            startYearNumericUpDown.ForeColor = team.IsDirty(TeamItemId.StartYear) ? Color.Red : SystemColors.WindowText;
            endYearNumericUpDown.ForeColor = team.IsDirty(TeamItemId.EndYear) ? Color.Red : SystemColors.WindowText;
            pictureNameTextBox.ForeColor = team.IsDirty(TeamItemId.PictureName) ? Color.Red : SystemColors.WindowText;
        }

        /// <summary>
        ///     編集項目を有効化する
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

            // 無効化時にクリアした文字列を再設定する
            idNumericUpDown.Text = idNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            skillNumericUpDown.Text = skillNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            startYearNumericUpDown.Text = startYearNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            endYearNumericUpDown.Text = endYearNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);

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
            skillNumericUpDown.ResetText();
            startYearNumericUpDown.ResetText();
            endYearNumericUpDown.ResetText();
            pictureNameTextBox.ResetText();
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
            Team team = GetSelectedTeam();
            if (team != null)
            {
                Brush brush;
                if ((Country.Tags[e.Index] == team.Country) && team.IsDirty(TeamItemId.Country))
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
        ///     研究特性コンボボックス1の項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpecialityComboBox1DrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            Team team = GetSelectedTeam();
            if (team != null)
            {
                // アイコンを描画する
                if (e.Index > 0 && e.Index - 1 < Techs.SpecialityImages.Images.Count)
                {
                    var gr = new Rectangle(e.Bounds.X + 1, e.Bounds.Y + 1, 16, 16);
                    e.Graphics.DrawImage(Techs.SpecialityImages.Images[e.Index - 1], gr);
                }

                // 項目の文字列を描画する
                Brush brush;
                if ((Techs.Specialities[e.Index] == team.Specialities[0]) && team.IsDirty(TeamItemId.Speciality1))
                {
                    brush = new SolidBrush(Color.Red);
                }
                else
                {
                    brush = new SolidBrush(SystemColors.WindowText);
                }
                string s = specialityComboBox1.Items[e.Index].ToString();
                var tr = new Rectangle(e.Bounds.X + 19, e.Bounds.Y + 3, e.Bounds.Width - 19, e.Bounds.Height);
                e.Graphics.DrawString(s, e.Font, brush, tr);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     研究特性コンボボックス2の項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpecialityComboBox2DrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            Team team = GetSelectedTeam();
            if (team != null)
            {
                // アイコンを描画する
                if (e.Index > 0 && e.Index - 1 < Techs.SpecialityImages.Images.Count)
                {
                    var gr = new Rectangle(e.Bounds.X + 1, e.Bounds.Y + 1, 16, 16);
                    e.Graphics.DrawImage(Techs.SpecialityImages.Images[e.Index - 1], gr);
                }

                // 項目の文字列を描画する
                Brush brush;
                if ((Techs.Specialities[e.Index] == team.Specialities[1]) && team.IsDirty(TeamItemId.Speciality2))
                {
                    brush = new SolidBrush(Color.Red);
                }
                else
                {
                    brush = new SolidBrush(SystemColors.WindowText);
                }
                string s = specialityComboBox2.Items[e.Index].ToString();
                var tr = new Rectangle(e.Bounds.X + 19, e.Bounds.Y + 3, e.Bounds.Width - 19, e.Bounds.Height);
                e.Graphics.DrawString(s, e.Font, brush, tr);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     研究特性コンボボックス3の項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpecialityComboBox3DrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            Team team = GetSelectedTeam();
            if (team != null)
            {
                // アイコンを描画する
                if (e.Index > 0 && e.Index - 1 < Techs.SpecialityImages.Images.Count)
                {
                    var gr = new Rectangle(e.Bounds.X + 1, e.Bounds.Y + 1, 16, 16);
                    e.Graphics.DrawImage(Techs.SpecialityImages.Images[e.Index - 1], gr);
                }

                // 項目の文字列を描画する
                Brush brush;
                if ((Techs.Specialities[e.Index] == team.Specialities[2]) && team.IsDirty(TeamItemId.Speciality3))
                {
                    brush = new SolidBrush(Color.Red);
                }
                else
                {
                    brush = new SolidBrush(SystemColors.WindowText);
                }
                string s = specialityComboBox3.Items[e.Index].ToString();
                var tr = new Rectangle(e.Bounds.X + 19, e.Bounds.Y + 3, e.Bounds.Width - 19, e.Bounds.Height);
                e.Graphics.DrawString(s, e.Font, brush, tr);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     研究特性コンボボックス4の項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpecialityComboBox4DrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            Team team = GetSelectedTeam();
            if (team != null)
            {
                // アイコンを描画する
                if (e.Index > 0 && e.Index - 1 < Techs.SpecialityImages.Images.Count)
                {
                    var gr = new Rectangle(e.Bounds.X + 1, e.Bounds.Y + 1, 16, 16);
                    e.Graphics.DrawImage(Techs.SpecialityImages.Images[e.Index - 1], gr);
                }

                // 項目の文字列を描画する
                Brush brush;
                if ((Techs.Specialities[e.Index] == team.Specialities[3]) && team.IsDirty(TeamItemId.Speciality4))
                {
                    brush = new SolidBrush(Color.Red);
                }
                else
                {
                    brush = new SolidBrush(SystemColors.WindowText);
                }
                string s = specialityComboBox4.Items[e.Index].ToString();
                var tr = new Rectangle(e.Bounds.X + 19, e.Bounds.Y + 3, e.Bounds.Width - 19, e.Bounds.Height);
                e.Graphics.DrawString(s, e.Font, brush, tr);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     研究特性コンボボックス5の項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpecialityComboBox5DrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            Team team = GetSelectedTeam();
            if (team != null)
            {
                // アイコンを描画する
                if (e.Index > 0 && e.Index - 1 < Techs.SpecialityImages.Images.Count)
                {
                    var gr = new Rectangle(e.Bounds.X + 1, e.Bounds.Y + 1, 16, 16);
                    e.Graphics.DrawImage(Techs.SpecialityImages.Images[e.Index - 1], gr);
                }

                // 項目の文字列を描画する
                Brush brush;
                if ((Techs.Specialities[e.Index] == team.Specialities[4]) && team.IsDirty(TeamItemId.Speciality5))
                {
                    brush = new SolidBrush(Color.Red);
                }
                else
                {
                    brush = new SolidBrush(SystemColors.WindowText);
                }
                string s = specialityComboBox5.Items[e.Index].ToString();
                var tr = new Rectangle(e.Bounds.X + 19, e.Bounds.Y + 3, e.Bounds.Width - 19, e.Bounds.Height);
                e.Graphics.DrawString(s, e.Font, brush, tr);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     研究特性コンボボックス6の項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpecialityComboBox6DrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            Team team = GetSelectedTeam();
            if (team != null)
            {
                // アイコンを描画する
                if (e.Index > 0 && e.Index - 1 < Techs.SpecialityImages.Images.Count)
                {
                    var gr = new Rectangle(e.Bounds.X + 1, e.Bounds.Y + 1, 16, 16);
                    e.Graphics.DrawImage(Techs.SpecialityImages.Images[e.Index - 1], gr);
                }

                // 項目の文字列を描画する
                Brush brush;
                if ((Techs.Specialities[e.Index] == team.Specialities[5]) && team.IsDirty(TeamItemId.Speciality6))
                {
                    brush = new SolidBrush(Color.Red);
                }
                else
                {
                    brush = new SolidBrush(SystemColors.WindowText);
                }
                string s = specialityComboBox6.Items[e.Index].ToString();
                var tr = new Rectangle(e.Bounds.X + 19, e.Bounds.Y + 3, e.Bounds.Width - 19, e.Bounds.Height);
                e.Graphics.DrawString(s, e.Font, brush, tr);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     研究機関画像ピクチャーボックスの項目を更新する
        /// </summary>
        /// <param name="team">研究機関データ</param>
        private void UpdateTeamPicture(Team team)
        {
            if (!string.IsNullOrEmpty(team.PictureName))
            {
                string fileName = Game.GetReadFileName(Game.PersonPicturePathName,
                                                       Path.ChangeExtension(team.PictureName, ".bmp"));
                teamPictureBox.ImageLocation = File.Exists(fileName) ? fileName : "";
            }
            else
            {
                teamPictureBox.ImageLocation = "";
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
            Team team = GetSelectedTeam();
            if (team == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            CountryTag country = Country.Tags[countryComboBox.SelectedIndex];
            if (country == team.Country)
            {
                return;
            }

            // 変更前の国タグの編集済みフラグを設定する
            Teams.SetDirty(team.Country);

            // 値を更新する
            team.Country = country;

            // 研究機関リストビューの項目を更新する
            teamListView.SelectedItems[0].Text = Country.Strings[(int) team.Country];

            // 研究機関ごとの編集済みフラグを設定する
            team.SetDirty(TeamItemId.Country);

            // 変更後の国タグの編集済みフラグを設定する
            Teams.SetDirty(team.Country);

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
            Team team = GetSelectedTeam();
            if (team == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var id = (int) idNumericUpDown.Value;
            if (id == team.Id)
            {
                return;
            }

            // 値を更新する
            team.Id = id;

            // 研究機関リストビューの項目を更新する
            teamListView.SelectedItems[0].SubItems[1].Text = team.Id.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            team.SetDirty(TeamItemId.Id);
            Teams.SetDirty(team.Country);

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
            Team team = GetSelectedTeam();
            if (team == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            string name = nameTextBox.Text;
            if (name.Equals(team.Name))
            {
                return;
            }

            // 値を更新する
            team.Name = name;

            // 研究機関リストビューの項目を更新する
            teamListView.SelectedItems[0].SubItems[2].Text = team.Name;

            // 編集済みフラグを設定する
            team.SetDirty(TeamItemId.Name);
            Teams.SetDirty(team.Country);

            // 文字色を変更する
            nameTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     スキル変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSkillNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Team team = GetSelectedTeam();
            if (team == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var skill = (int) skillNumericUpDown.Value;
            if (skill == team.Skill)
            {
                return;
            }

            // 値を更新する
            team.Skill = skill;

            // 研究機関リストビューの項目を更新する
            teamListView.SelectedItems[0].SubItems[3].Text =
                team.Skill.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            team.SetDirty(TeamItemId.Skill);
            Teams.SetDirty(team.Country);

            // 文字色を変更する
            skillNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     開始年変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStartYearNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Team team = GetSelectedTeam();
            if (team == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var startYear = (int) startYearNumericUpDown.Value;
            if (startYear == team.StartYear)
            {
                return;
            }

            // 値を更新する
            team.StartYear = startYear;

            // 研究機関リストビューの項目を更新する
            teamListView.SelectedItems[0].SubItems[4].Text =
                team.StartYear.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            team.SetDirty(TeamItemId.StartYear);
            Teams.SetDirty(team.Country);

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
            Team team = GetSelectedTeam();
            if (team == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var endYear = (int) endYearNumericUpDown.Value;
            if (endYear == team.EndYear)
            {
                return;
            }

            // 値を更新する
            team.EndYear = endYear;

            // 研究機関リストビューの項目を更新する
            teamListView.SelectedItems[0].SubItems[5].Text =
                team.EndYear.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            team.SetDirty(TeamItemId.EndYear);
            Teams.SetDirty(team.Country);

            // 文字色を変更する
            endYearNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     研究特性1変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpecialityComboBox1SelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Team team = GetSelectedTeam();
            if (team == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var speciality = (TechSpeciality) specialityComboBox1.SelectedIndex;
            if (speciality == team.Specialities[0])
            {
                return;
            }

            // 値を更新する
            team.Specialities[0] = speciality;

            // 編集済みフラグを設定する
            team.SetDirty(TeamItemId.Speciality1);
            Teams.SetDirty(team.Country);

            // 研究機関リストビューの項目を更新する
            teamListView.Refresh();

            // 研究特性ボックス1の項目色を変更するため描画更新する
            specialityComboBox1.Refresh();
        }

        /// <summary>
        ///     特性2変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpecialityComboBox2SelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Team team = GetSelectedTeam();
            if (team == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var speciality = (TechSpeciality) specialityComboBox2.SelectedIndex;
            if (speciality == team.Specialities[1])
            {
                return;
            }

            // 値を更新する
            team.Specialities[1] = speciality;

            // 編集済みフラグを設定する
            team.SetDirty(TeamItemId.Speciality2);
            Teams.SetDirty(team.Country);

            // 研究機関リストビューの項目を更新する
            teamListView.Refresh();

            // 研究特性ボックス2の項目色を変更するため描画更新する
            specialityComboBox2.Refresh();
        }

        /// <summary>
        ///     特性3変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpecialityComboBox3SelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Team team = GetSelectedTeam();
            if (team == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var speciality = (TechSpeciality) specialityComboBox3.SelectedIndex;
            if (speciality == team.Specialities[2])
            {
                return;
            }

            // 値を更新する
            team.Specialities[2] = speciality;

            // 編集済みフラグを設定する
            team.SetDirty(TeamItemId.Speciality3);
            Teams.SetDirty(team.Country);

            // 研究機関リストビューの項目を更新する
            teamListView.Refresh();

            // 研究特性ボックス3の項目色を変更するため描画更新する
            specialityComboBox3.Refresh();
        }

        /// <summary>
        ///     特性4変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpecialityComboBox4SelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Team team = GetSelectedTeam();
            if (team == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var speciality = (TechSpeciality) specialityComboBox4.SelectedIndex;
            if (speciality == team.Specialities[3])
            {
                return;
            }

            // 値を更新する
            team.Specialities[3] = speciality;

            // 編集済みフラグを設定する
            team.SetDirty(TeamItemId.Speciality4);
            Teams.SetDirty(team.Country);

            // 研究機関リストビューの項目を更新する
            teamListView.Refresh();

            // 研究特性ボックス4の項目色を変更するため描画更新する
            specialityComboBox4.Refresh();
        }

        /// <summary>
        ///     特性5変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpecialityComboBox5SelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Team team = GetSelectedTeam();
            if (team == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var speciality = (TechSpeciality) specialityComboBox5.SelectedIndex;
            if (speciality == team.Specialities[4])
            {
                return;
            }

            // 値を更新する
            team.Specialities[4] = speciality;

            // 編集済みフラグを設定する
            team.SetDirty(TeamItemId.Speciality5);
            Teams.SetDirty(team.Country);

            // 研究機関リストビューの項目を更新する
            teamListView.Refresh();

            // 研究特性ボックス5の項目色を変更するため描画更新する
            specialityComboBox5.Refresh();
        }

        /// <summary>
        ///     特性6変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpecialityComboBox6SelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Team team = GetSelectedTeam();
            if (team == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var speciality = (TechSpeciality) specialityComboBox6.SelectedIndex;
            if (speciality == team.Specialities[5])
            {
                return;
            }

            // 値を更新する
            team.Specialities[5] = speciality;

            // 編集済みフラグを設定する
            team.SetDirty(TeamItemId.Speciality6);
            Teams.SetDirty(team.Country);

            // 研究機関リストビューの項目を更新する
            teamListView.Refresh();

            // 研究特性ボックス6の項目色を変更するため描画更新する
            specialityComboBox6.Refresh();
        }

        /// <summary>
        ///     画像ファイル名変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPictureNameTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Team team = GetSelectedTeam();
            if (team == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            string pictureName = pictureNameTextBox.Text;
            if (pictureName.Equals(team.PictureName))
            {
                return;
            }

            // 値を更新する
            team.PictureName = pictureName;

            // 画像ファイルを更新する
            UpdateTeamPicture(team);

            // 編集済みフラグを設定する
            team.SetDirty(TeamItemId.PictureName);
            Teams.SetDirty(team.Country);

            // 文字色を変更する
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
            Team team = GetSelectedTeam();
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

        #endregion
    }
}