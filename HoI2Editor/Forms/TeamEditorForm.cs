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
    ///     研究機関エディタのフォーム
    /// </summary>
    public partial class TeamEditorForm : Form
    {
        #region 内部フィールド

        /// <summary>
        ///     絞り込み後の研究機関リスト
        /// </summary>
        private readonly List<Team> _list = new List<Team>();

        /// <summary>
        ///     研究特性コンボボックスの配列
        /// </summary>
        private readonly ComboBox[] _specialityComboBoxes;

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

        #region 内部定数

        /// <summary>
        ///     編集可能な特性の数
        /// </summary>
        private const int MaxEditableSpecialities = 7;

        /// <summary>
        ///     研究特性の項目ID
        /// </summary>
        private static readonly TeamItemId[] SpecialityItemIds =
        {
            TeamItemId.Speciality1,
            TeamItemId.Speciality2,
            TeamItemId.Speciality3,
            TeamItemId.Speciality4,
            TeamItemId.Speciality5,
            TeamItemId.Speciality6,
            TeamItemId.Speciality7
        };

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public TeamEditorForm()
        {
            InitializeComponent();

            // 研究特性コンボボックスの配列を初期化する
            _specialityComboBoxes = new[]
            {
                specialityComboBox1,
                specialityComboBox2,
                specialityComboBox3,
                specialityComboBox4,
                specialityComboBox5,
                specialityComboBox6,
                specialityComboBox7
            };

            // 自動スケーリングを考慮した初期化
            InitScaling();
        }

        /// <summary>
        ///     自動スケーリングを考慮した初期化
        /// </summary>
        private void InitScaling()
        {
            // 研究機関リストビュー
            countryColumnHeader.Width = DeviceCaps.GetScaledWidth(countryColumnHeader.Width);
            idColumnHeader.Width = DeviceCaps.GetScaledWidth(idColumnHeader.Width);
            nameColumnHeader.Width = DeviceCaps.GetScaledWidth(nameColumnHeader.Width);
            skillColumnHeader.Width = DeviceCaps.GetScaledWidth(skillColumnHeader.Width);
            startYearColumnHeader.Width = DeviceCaps.GetScaledWidth(startYearColumnHeader.Width);
            endYearColumnHeader.Width = DeviceCaps.GetScaledWidth(endYearColumnHeader.Width);
            specialityColumnHeader.Width = DeviceCaps.GetScaledWidth(specialityColumnHeader.Width);

            // 国家リストボックス
            countryListBox.ColumnWidth = DeviceCaps.GetScaledWidth(countryListBox.ColumnWidth);
            countryListBox.ItemHeight = DeviceCaps.GetScaledHeight(countryListBox.ItemHeight);

            // 特性コンボボックス
            specialityComboBox1.ItemHeight = DeviceCaps.GetScaledHeight(specialityComboBox1.ItemHeight);
            specialityComboBox2.ItemHeight = DeviceCaps.GetScaledHeight(specialityComboBox2.ItemHeight);
            specialityComboBox3.ItemHeight = DeviceCaps.GetScaledHeight(specialityComboBox3.ItemHeight);
            specialityComboBox4.ItemHeight = DeviceCaps.GetScaledHeight(specialityComboBox4.ItemHeight);
            specialityComboBox5.ItemHeight = DeviceCaps.GetScaledHeight(specialityComboBox5.ItemHeight);
            specialityComboBox6.ItemHeight = DeviceCaps.GetScaledHeight(specialityComboBox6.ItemHeight);
            specialityComboBox7.ItemHeight = DeviceCaps.GetScaledHeight(specialityComboBox7.ItemHeight);
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTeamEditorFormLoad(object sender, EventArgs e)
        {
            // 国家データを初期化する
            Countries.Init();

            // 研究特性を初期化する
            Techs.InitSpecialities();

            // ゲーム設定ファイルを読み込む
            Misc.Load();

            // 文字列定義ファイルを読み込む
            Config.Load();

            // 研究機関リストビューの高さを設定するためにダミーのイメージリストを作成する
            teamListView.SmallImageList = new ImageList {ImageSize = new Size(1, DeviceCaps.GetScaledHeight(18))};

            // 編集項目を初期化する
            InitEditableItems();

            // 国家リストボックスを初期化する
            InitCountryListBox();

            // 研究機関ファイルを読み込む
            Teams.Load();

            // データ読み込み後の処理
            OnFileLoaded();
        }

        /// <summary>
        ///     最適組み合わせボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMatchingButtonClick(object sender, EventArgs e)
        {
            HoI2Editor.LaunchResearchViewerForm();
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
            if (!HoI2Editor.IsDirty())
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
                    HoI2Editor.Save();
                    break;
                case DialogResult.No:
                    HoI2Editor.SaveCanceled = true;
                    break;
            }
        }

        /// <summary>
        ///     フォームクローズ後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTeamEditorFormClosed(object sender, FormClosedEventArgs e)
        {
            HoI2Editor.OnTeamEditorFormClosed();
        }

        #endregion

        #region データ処理

        /// <summary>
        ///     再読み込みボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReloadButtonClick(object sender, EventArgs e)
        {
            // 編集済みならば保存するかを問い合わせる
            if (HoI2Editor.IsDirty())
            {
                DialogResult result = MessageBox.Show(Resources.ConfirmSaveMessage, Text, MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);
                switch (result)
                {
                    case DialogResult.Cancel:
                        return;
                    case DialogResult.Yes:
                        HoI2Editor.Save();
                        break;
                }
            }

            HoI2Editor.Reload();
        }

        /// <summary>
        ///     保存ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            HoI2Editor.Save();
        }

        /// <summary>
        ///     データ読み込み後の処理
        /// </summary>
        public void OnFileLoaded()
        {
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
            List<Country> tags = (from string s in countryListBox.SelectedItems select Countries.StringMap[s]).ToList();

            // 選択中の国家に所属する研究機関を順に絞り込む
            _list.AddRange(Teams.Items.Where(team => tags.Contains(team.Country)));
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

            var rect = new Rectangle(e.Bounds.X + 4, e.Bounds.Y + 1, DeviceCaps.GetScaledWidth(16),
                DeviceCaps.GetScaledHeight(16));
            for (int i = 0; i < Team.SpecialityLength; i++)
            {
                // 研究特性なしならば何もしない
                if (team.Specialities[i] == TechSpeciality.None)
                {
                    continue;
                }

                // 研究特性アイコンを描画する
                e.Graphics.DrawImage(
                    Techs.SpecialityImages.Images[Array.IndexOf(Techs.Specialities, team.Specialities[i]) - 1],
                    rect);
                rect.X += DeviceCaps.GetScaledWidth(16) + 3;
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
                    Id = Teams.GetNewId(selected.Country),
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
                Country country = Countries.Tags[countryListBox.SelectedIndex];
                // 新規項目を作成する
                team = new Team
                {
                    Country = country,
                    Id = Teams.GetNewId(country),
                    Skill = 1,
                    StartYear = 1930,
                    EndYear = 1970,
                };

                // 研究機関ごとの編集済みフラグを設定する
                team.SetDirtyAll();

                // 研究機関リストに項目を追加する
                Teams.AddItem(team);
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
                Id = Teams.GetNewId(selected.Country),
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
                Text = Countries.Strings[(int) team.Country],
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
            countryListBox.BeginUpdate();
            countryListBox.Items.Clear();
            foreach (Country country in Countries.Tags)
            {
                countryListBox.Items.Add(Countries.Strings[(int) country]);
            }
            countryListBox.SelectedIndex = 0;
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
            int count = countryListBox.SelectedItems.Count;

            // 選択数に合わせて全選択/全解除を切り替える
            countryAllButton.Text = (count <= 1) ? Resources.KeySelectAll : Resources.KeyUnselectAll;

            // 選択数がゼロの場合は新規追加ボタンを無効化する
            newButton.Enabled = (count > 0);

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
            Graphics g = Graphics.FromHwnd(Handle);
            int margin = DeviceCaps.GetScaledWidth(2) + 1;

            // 国タグ
            countryComboBox.BeginUpdate();
            countryComboBox.Items.Clear();
            int width = countryComboBox.Width;
            foreach (string s in Countries.Tags
                .Select(country => Countries.Strings[(int) country])
                .Select(name => Config.ExistsKey(name)
                    ? string.Format("{0} {1}", name, Config.GetText(name))
                    : name))
            {
                countryComboBox.Items.Add(s);
                width = Math.Max(width,
                    (int) g.MeasureString(s, countryComboBox.Font).Width + SystemInformation.VerticalScrollBarWidth +
                    margin);
            }
            countryComboBox.DropDownWidth = width;
            countryComboBox.EndUpdate();

            // 研究特性
            for (int i = 0; i < MaxEditableSpecialities; i++)
            {
                _specialityComboBoxes[i].Tag = i;
                _specialityComboBoxes[i].Items.Clear();
            }
            width = specialityComboBox1.Width;
            int additional = DeviceCaps.GetScaledWidth(16) + 3 + SystemInformation.VerticalScrollBarWidth;
            foreach (string s in Techs.Specialities.Select(Techs.GetSpecialityName))
            {
                for (int i = 0; i < MaxEditableSpecialities; i++)
                {
                    _specialityComboBoxes[i].Items.Add(s);
                }
                // 研究特性アイコンの幅を追加している
                width = Math.Max(width,
                    (int) g.MeasureString(s, specialityComboBox1.Font).Width + additional);
            }
            for (int i = 0; i < MaxEditableSpecialities; i++)
            {
                _specialityComboBoxes[i].DropDownWidth = width;
            }
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
            countryComboBox.SelectedIndex = team.Country != Country.None ? (int) team.Country - 1 : -1;
            idNumericUpDown.Value = team.Id;
            nameTextBox.Text = team.Name;
            skillNumericUpDown.Value = team.Skill;
            startYearNumericUpDown.Value = team.StartYear;
            endYearNumericUpDown.Value = team.EndYear;
            for (int i = 0; i < MaxEditableSpecialities; i++)
            {
                _specialityComboBoxes[i].SelectedIndex = Array.IndexOf(Techs.Specialities, team.Specialities[i]);
            }
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
            for (int i = 0; i < MaxEditableSpecialities; i++)
            {
                _specialityComboBoxes[i].Refresh();
            }

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
            for (int i = 0; i < MaxEditableSpecialities; i++)
            {
                _specialityComboBoxes[i].Enabled = true;
            }

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
            for (int i = 0; i < MaxEditableSpecialities; i++)
            {
                _specialityComboBoxes[i].Enabled = false;
            }

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
                if ((Countries.Tags[e.Index] == team.Country) && team.IsDirty(TeamItemId.Country))
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
        ///     研究特性コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpecialityComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            var comboBox = sender as ComboBox;
            if (comboBox == null)
            {
                return;
            }
            var no = (int) comboBox.Tag;

            // 背景を描画する
            e.DrawBackground();

            Team team = GetSelectedTeam();
            if (team != null)
            {
                // アイコンを描画する
                if (e.Index > 0 && e.Index - 1 < Techs.SpecialityImages.Images.Count)
                {
                    var gr = new Rectangle(e.Bounds.X + 1, e.Bounds.Y + 1, DeviceCaps.GetScaledWidth(16),
                        DeviceCaps.GetScaledHeight(16));
                    e.Graphics.DrawImage(Techs.SpecialityImages.Images[e.Index - 1], gr);
                }

                // 項目の文字列を描画する
                Brush brush;
                if ((Techs.Specialities[e.Index] == team.Specialities[no]) && team.IsDirty(SpecialityItemIds[no]))
                {
                    brush = new SolidBrush(Color.Red);
                }
                else
                {
                    brush = new SolidBrush(SystemColors.WindowText);
                }
                string s = comboBox.Items[e.Index].ToString();
                var tr = new Rectangle(e.Bounds.X + DeviceCaps.GetScaledWidth(16) + 3, e.Bounds.Y + 3,
                    e.Bounds.Width - DeviceCaps.GetScaledWidth(16) - 3, e.Bounds.Height);
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
            Country country = Countries.Tags[countryComboBox.SelectedIndex];
            if (country == team.Country)
            {
                return;
            }

            // 変更前の国タグの編集済みフラグを設定する
            Teams.SetDirty(team.Country);

            // 値を更新する
            team.Country = country;

            // 研究機関リストビューの項目を更新する
            teamListView.SelectedItems[0].Text = Countries.Strings[(int) team.Country];

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
        ///     研究特性変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpecialityComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Team team = GetSelectedTeam();
            if (team == null)
            {
                return;
            }

            var comboBox = sender as ComboBox;
            if (comboBox == null)
            {
                return;
            }
            var no = (int) comboBox.Tag;

            // 値に変化がなければ何もしない
            TechSpeciality speciality = Techs.Specialities[comboBox.SelectedIndex];
            if (speciality == team.Specialities[no])
            {
                return;
            }

            // 研究特性を変更する
            ChangeTechSpeciality(team, no, speciality);

            // 研究機関リストビューの項目を更新する
            teamListView.Refresh();

            // 編集項目を更新する
            UpdateEditableItemsValue(team);

            // 編集項目の色を更新する
            UpdateEditableItemsColor(team);
        }

        /// <summary>
        ///     研究特性を変更する
        /// </summary>
        /// <param name="team">対象の研究機関</param>
        /// <param name="no">研究特性の番号</param>
        /// <param name="speciality">研究特性</param>
        private void ChangeTechSpeciality(Team team, int no, TechSpeciality speciality)
        {
            if (speciality == TechSpeciality.None)
            {
                // 特性なしに変更された場合、後ろの項目を詰める
                for (int i = no; i < MaxEditableSpecialities; i++)
                {
                    if (team.Specialities[i] != TechSpeciality.None || team.Specialities[i + 1] != TechSpeciality.None)
                    {
                        // 1つ前に詰める
                        team.Specialities[i] = team.Specialities[i + 1];
                        // 編集済みフラグを設定する
                        team.SetDirty(SpecialityItemIds[i]);
                    }
                }
            }
            else
            {
                // 変更した箇所よりも前に空きがあれば詰める
                for (int i = 0; i < no; i++)
                {
                    if (team.Specialities[i] == TechSpeciality.None)
                    {
                        no = i;
                        break;
                    }
                }
                // 重複項目を検索する
                for (int i = 0; i < MaxEditableSpecialities; i++)
                {
                    if (i == no)
                    {
                        continue;
                    }

                    if (speciality == team.Specialities[i])
                    {
                        // 他の項目と重複していてかつ元が特性なしならば何もしない
                        if (team.Specialities[no] == TechSpeciality.None)
                        {
                            return;
                        }
                        // 重複している項目と特性を交換する
                        team.Specialities[i] = team.Specialities[no];
                        // 交換対象の編集済みフラグを設定する
                        team.SetDirty(SpecialityItemIds[i]);
                        break;
                    }
                }
                // 対象項目の値を更新する
                team.Specialities[no] = speciality;
                // 対象項目の編集済みフラグを設定する
                team.SetDirty(SpecialityItemIds[no]);
            }

            // 国別の編集済みフラグを設定する
            Teams.SetDirty(team.Country);
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

        /// <summary>
        ///     ID順ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSortIdButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Team team = GetSelectedTeam();
            if (team == null)
            {
                return;
            }

            // 研究特性をID順にソートする
            SortSpeciality(team, new IdComparer());
        }

        /// <summary>
        ///     ABC順ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSortAbcButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Team team = GetSelectedTeam();
            if (team == null)
            {
                return;
            }

            // 研究特性をABC順にソートする
            SortSpeciality(team, new AbcComparer());
        }

        /// <summary>
        ///     研究特性をソートする
        /// </summary>
        /// <param name="team">ソート対象の研究機関</param>
        /// <param name="comparer">ソート用</param>
        private void SortSpeciality(Team team, IComparer<TechSpeciality> comparer)
        {
            // ソート前の項目を退避する
            const int max = 7;
            var old = new TechSpeciality[max];
            for (int i = 0; i < max; i++)
            {
                old[i] = team.Specialities[i];
            }

            // ソートする
            Array.Sort(team.Specialities, 0, max, comparer);

            // 研究特性1の更新チェック
            if (team.Specialities[0] != old[0])
            {
                // 編集済みフラグを設定する
                team.SetDirty(TeamItemId.Speciality1);
                Teams.SetDirty(team.Country);
            }
            // 研究特性2の更新チェック
            if (team.Specialities[1] != old[1])
            {
                // 編集済みフラグを設定する
                team.SetDirty(TeamItemId.Speciality2);
                Teams.SetDirty(team.Country);
            }
            // 研究特性3の更新チェック
            if (team.Specialities[2] != old[2])
            {
                // 編集済みフラグを設定する
                team.SetDirty(TeamItemId.Speciality3);
                Teams.SetDirty(team.Country);
            }
            // 研究特性4の更新チェック
            if (team.Specialities[3] != old[3])
            {
                // 編集済みフラグを設定する
                team.SetDirty(TeamItemId.Speciality4);
                Teams.SetDirty(team.Country);
            }
            // 研究特性5の更新チェック
            if (team.Specialities[4] != old[4])
            {
                // 編集済みフラグを設定する
                team.SetDirty(TeamItemId.Speciality5);
                Teams.SetDirty(team.Country);
            }
            // 研究特性6の更新チェック
            if (team.Specialities[5] != old[5])
            {
                // 編集済みフラグを設定する
                team.SetDirty(TeamItemId.Speciality6);
                Teams.SetDirty(team.Country);
            }
            // 研究特性7の更新チェック
            if (team.Specialities[6] != old[6])
            {
                // 編集済みフラグを設定する
                team.SetDirty(TeamItemId.Speciality7);
                Teams.SetDirty(team.Country);
            }

            // 研究機関リストビューの項目を更新する
            teamListView.Refresh();

            // 編集項目を更新する
            UpdateEditableItemsValue(team);

            // 編集項目の色を更新する
            UpdateEditableItemsColor(team);
        }

        /// <summary>
        ///     研究特性のABC順ソート用
        /// </summary>
        private class AbcComparer : IComparer<TechSpeciality>
        {
            /// <summary>
            ///     ABC順優先度
            /// </summary>
            private static readonly int[] Priorities =
            {
                109,
                3,
                29,
                15,
                12,
                47,
                18,
                39,
                34,
                0,
                37,
                36,
                25,
                20,
                28,
                42,
                24,
                11,
                14,
                46,
                19,
                21,
                13,
                23,
                33,
                35,
                2,
                17,
                7,
                9,
                45,
                22,
                41,
                40,
                38,
                4,
                32,
                48,
                8,
                44,
                16,
                6,
                31,
                1,
                27,
                26,
                5,
                43,
                30,
                10,
                49,
                50,
                51,
                52,
                53,
                54,
                55,
                56,
                57,
                58,
                59,
                60,
                61,
                62,
                63,
                64,
                65,
                66,
                67,
                68,
                69,
                70,
                71,
                72,
                73,
                74,
                75,
                76,
                77,
                78,
                79,
                80,
                81,
                82,
                83,
                84,
                85,
                86,
                87,
                88,
                89,
                90,
                91,
                92,
                93,
                94,
                95,
                96,
                97,
                98,
                99,
                100,
                101,
                102,
                103,
                104,
                105,
                106,
                107,
                108
            };

            /// <summary>
            ///     研究特性を比較する
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public int Compare(TechSpeciality x, TechSpeciality y)
            {
                return Priorities[(int) x] - Priorities[(int) y];
            }
        }

        /// <summary>
        ///     研究特性のID順ソート用
        /// </summary>
        private class IdComparer : IComparer<TechSpeciality>
        {
            /// <summary>
            ///     研究特性を比較する
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public int Compare(TechSpeciality x, TechSpeciality y)
            {
                // 指定なしの場合は後ろへ移動する
                if (x == TechSpeciality.None)
                {
                    return 1;
                }
                if (y == TechSpeciality.None)
                {
                    return -1;
                }
                return (int) x - (int) y;
            }
        }

        #endregion
    }
}