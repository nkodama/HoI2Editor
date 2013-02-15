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
    ///     指揮官エディタのフォーム
    /// </summary>
    public partial class LeaderEditorForm : Form
    {
        #region フィールド

        /// <summary>
        ///     絞り込み後の指揮官リスト
        /// </summary>
        private readonly List<Leader> _narrowedList = new List<Leader>();

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public LeaderEditorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLeaderEditorFormLoad(object sender, EventArgs e)
        {
            // 国家データを初期化する
            Country.Init();

            // 特性文字列を初期化する
            InitTraitsText();

            // 編集項目を初期化する
            InitEditableItems();

            // 国家リストボックスを初期化する
            InitCountryListBox();

            // 指揮官ファイルを読み込む
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

        #region 指揮官データ処理

        /// <summary>
        ///     再読み込みボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReloadButtonClick(object sender, EventArgs e)
        {
            // 指揮官ファイルの再読み込みを要求する
            Leaders.RequireReload();

            // 指揮官ファイルを読み込む
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
        ///     指揮官ファイルを読み込む
        /// </summary>
        private void LoadFiles()
        {
            // 指揮官ファイルを読み込む
            Leaders.Load();

            // 指揮官リストを絞り込む
            NarrowLeaderList();

            // 指揮官リストの表示を更新する
            UpdateLeaderList();

            // 編集済みフラグがクリアされるため表示を更新する
            countryListBox.Update();
        }

        /// <summary>
        ///     指揮官ファイルを保存する
        /// </summary>
        private void SaveFiles()
        {
            // 指揮官ファイルを保存する
            Leaders.Save();

            // 編集済みフラグがクリアされるため表示を更新する
            countryListBox.Update();
        }

        #endregion

        #region 指揮官リストビュー

        /// <summary>
        ///     指揮官リストの表示を更新する
        /// </summary>
        private void UpdateLeaderList()
        {
            leaderListView.BeginUpdate();
            leaderListView.Items.Clear();

            foreach (Leader leader in _narrowedList)
            {
                leaderListView.Items.Add(CreateLeaderListViewItem(leader));
            }

            if (leaderListView.Items.Count > 0)
            {
                leaderListView.Items[0].Focused = true;
                leaderListView.Items[0].Selected = true;
                EnableEditableItems();
            }
            else
            {
                DisableEditableItems();
            }

            leaderListView.EndUpdate();
        }

        /// <summary>
        ///     指揮官リストを絞り込む
        /// </summary>
        private void NarrowLeaderList()
        {
            _narrowedList.Clear();

            // 選択中の国家がなければ戻る
            if (countryListBox.SelectedIndices.Count == 0)
            {
                return;
            }

            // 特性絞り込みマスクを取得する
            uint traitsMask = GetNarrowedTraits();

            // 選択中の国家リストを作成する
            List<CountryTag> selectedTags =
                (from string country in countryListBox.SelectedItems select Country.StringMap[country]).ToList();

            // 選択中の国家に所属する指揮官を順に絞り込む
            foreach (Leader leader in Leaders.List.Where(leader => selectedTags.Contains(leader.Country)))
            {
                // 兵科による絞り込み
                switch (leader.Branch)
                {
                    case LeaderBranch.Army:
                        if (!armyNarrowCheckBox.Checked)
                        {
                            continue;
                        }
                        break;

                    case LeaderBranch.Navy:
                        if (!navyNarrowCheckBox.Checked)
                        {
                            continue;
                        }
                        break;

                    case LeaderBranch.Airforce:
                        if (!airforceNarrowCheckBox.Checked)
                        {
                            continue;
                        }
                        break;

                    default:
                        if (!armyNarrowCheckBox.Checked ||
                            !navyNarrowCheckBox.Checked ||
                            !airforceNarrowCheckBox.Checked)
                        {
                            continue;
                        }
                        break;
                }

                // 指揮官特性による絞り込み
                if (traitsOrNarrowRadioButton.Checked)
                {
                    // OR条件
                    if ((leader.Traits & traitsMask) == 0)
                    {
                        continue;
                    }
                }
                else if (traitsAndNarrowRadioButton.Checked)
                {
                    // AND条件
                    if ((leader.Traits & traitsMask) != traitsMask)
                    {
                        continue;
                    }
                }

                _narrowedList.Add(leader);
            }
        }

        /// <summary>
        ///     指揮官リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLeaderListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }

            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }

            // 編集項目を更新する
            UpdateCountryComboBox(leader);
            idNumericUpDown.Value = leader.Id;
            nameTextBox.Text = leader.Name;
            UpdateBranchComboBox(leader);
            UpdateIdealRankComboBox(leader);
            skillNumericUpDown.Value = leader.Skill;
            maxSkillNumericUpDown.Value = leader.MaxSkill;
            experienceNumericUpDown.Value = leader.Experience;
            loyaltyNumericUpDown.Value = leader.Loyalty;
            startYearNumericUpDown.Value = leader.StartYear;
            endYearNumericUpDown.Value = leader.EndYear;
            retirementYearNumericUpDown.Value = leader.RetirementYear;
            rankYearNumericUpDown1.Value = leader.RankYear[0];
            rankYearNumericUpDown2.Value = leader.RankYear[1];
            rankYearNumericUpDown3.Value = leader.RankYear[2];
            rankYearNumericUpDown4.Value = leader.RankYear[3];
            pictureNameTextBox.Text = leader.PictureName;
            UpdateLeaderPicture(leader);

            // 中途半端な状態での更新を防ぐため、更新イベントを抑止する
            SetTraitsCheckBoxEvent(false);

            // 特性チェックボックスの状態を更新する
            logisticsWizardCheckBox.Checked = ((leader.Traits & LeaderTraits.LogisticsWizard) != 0);
            defensiveDoctrineCheckBox.Checked = ((leader.Traits & LeaderTraits.DefensiveDoctrine) != 0);
            offensiveDoctrineCheckBox.Checked = ((leader.Traits & LeaderTraits.OffensiveDoctrine) != 0);
            winterSpecialistCheckBox.Checked = ((leader.Traits & LeaderTraits.WinterSpecialist) != 0);
            tricksterCheckBox.Checked = ((leader.Traits & LeaderTraits.Trickster) != 0);
            engineerCheckBox.Checked = ((leader.Traits & LeaderTraits.Engineer) != 0);
            fortressBusterCheckBox.Checked = ((leader.Traits & LeaderTraits.FortressBuster) != 0);
            panzerLeaderCheckBox.Checked = ((leader.Traits & LeaderTraits.PanzerLeader) != 0);
            commandoCheckBox.Checked = ((leader.Traits & LeaderTraits.Commando) != 0);
            oldGuardCheckBox.Checked = ((leader.Traits & LeaderTraits.OldGuard) != 0);
            seaWolfCheckBox.Checked = ((leader.Traits & LeaderTraits.SeaWolf) != 0);
            blockadeRunnerCheckBox.Checked = ((leader.Traits & LeaderTraits.BlockadeRunner) != 0);
            superiorTacticianCheckBox.Checked = ((leader.Traits & LeaderTraits.SuperiorTactician) != 0);
            spotterCheckBox.Checked = ((leader.Traits & LeaderTraits.Spotter) != 0);
            tankBusterCheckBox.Checked = ((leader.Traits & LeaderTraits.TankBuster) != 0);
            carpetBomberCheckBox.Checked = ((leader.Traits & LeaderTraits.CarpetBomber) != 0);
            nightFlyerCheckBox.Checked = ((leader.Traits & LeaderTraits.NightFlyer) != 0);
            fleetDestroyerCheckBox.Checked = ((leader.Traits & LeaderTraits.FleetDestroyer) != 0);
            desertFoxCheckBox.Checked = ((leader.Traits & LeaderTraits.DesertFox) != 0);
            jungleRatCheckBox.Checked = ((leader.Traits & LeaderTraits.JungleRat) != 0);
            urbanWarfareSpecialistCheckBox.Checked = ((leader.Traits & LeaderTraits.UrbanWarfareSpecialist) != 0);
            rangerCheckBox.Checked = ((leader.Traits & LeaderTraits.Ranger) != 0);
            mountaineerCheckBox.Checked = ((leader.Traits & LeaderTraits.Mountaineer) != 0);
            hillsFighterCheckBox.Checked = ((leader.Traits & LeaderTraits.HillsFighter) != 0);
            counterAttackerCheckBox.Checked = ((leader.Traits & LeaderTraits.CounterAttacker) != 0);
            assaulterCheckBox.Checked = ((leader.Traits & LeaderTraits.Assaulter) != 0);
            encirclerCheckBox.Checked = ((leader.Traits & LeaderTraits.Encircler) != 0);
            ambusherCheckBox.Checked = ((leader.Traits & LeaderTraits.Ambusher) != 0);
            disciplinedCheckBox.Checked = ((leader.Traits & LeaderTraits.Disciplined) != 0);
            elasticDefenceSpecialistCheckBox.Checked = ((leader.Traits & LeaderTraits.ElasticDefenceSpecialist) != 0);
            blitzerCheckBox.Checked = ((leader.Traits & LeaderTraits.Blitzer) != 0);

            // 更新イベントを再開する
            SetTraitsCheckBoxEvent(true);

            // 項目移動ボタンの状態更新
            topButton.Enabled = leaderListView.SelectedIndices[0] != 0;
            upButton.Enabled = leaderListView.SelectedIndices[0] != 0;
            downButton.Enabled = leaderListView.SelectedIndices[0] != leaderListView.Items.Count - 1;
            bottomButton.Enabled = leaderListView.SelectedIndices[0] != leaderListView.Items.Count - 1;
        }

        /// <summary>
        ///     新規ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNewButtonClick(object sender, EventArgs e)
        {
            Leader leader;
            if (leaderListView.SelectedItems.Count > 0)
            {
                var selected = leaderListView.SelectedItems[0].Tag as Leader;
                if (selected == null)
                {
                    return;
                }

                // 選択項目がある場合、国タグやIDを引き継いで項目を作成する
                leader = new Leader
                             {
                                 Country = selected.Country,
                                 Id = selected.Id + 1,
                                 Branch = LeaderBranch.None,
                                 IdealRank = LeaderRank.None,
                                 StartYear = 1930,
                                 EndYear = 1990,
                                 RetirementYear = 1999,
                             };
                leader.RankYear[0] = 1930;
                leader.RankYear[1] = 1990;
                leader.RankYear[2] = 1990;
                leader.RankYear[3] = 1990;

                // 指揮官リストに項目を挿入する
                Leaders.InsertItem(leader, selected);
                InsertListItem(leader, leaderListView.SelectedIndices[0] + 1);
            }
            else
            {
                // 新規項目を作成する
                leader = new Leader
                             {
                                 Country =
                                     countryListBox.SelectedItems.Count > 0
                                         ? (CountryTag) (countryListBox.SelectedIndex + 1)
                                         : CountryTag.None,
                                 Id = 0,
                                 Branch = LeaderBranch.None,
                                 IdealRank = LeaderRank.None,
                                 StartYear = 1930,
                                 EndYear = 1990,
                                 RetirementYear = 1999,
                             };
                leader.RankYear[0] = 1930;
                leader.RankYear[1] = 1990;
                leader.RankYear[2] = 1990;
                leader.RankYear[3] = 1990;

                // 指揮官リストに項目を追加する
                Leaders.AddItem(leader);
                AddListItem(leader);

                // 編集項目を有効化する
                EnableEditableItems();
            }

            // 編集済みフラグを設定する
            Leaders.SetDirtyFlag(leader.Country);
        }

        /// <summary>
        ///     複製ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloneButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }
            var selected = leaderListView.SelectedItems[0].Tag as Leader;
            if (selected == null)
            {
                return;
            }

            // 選択項目を引き継いで項目を作成する
            var leader = new Leader
                             {
                                 Country = selected.Country,
                                 Id = selected.Id + 1,
                                 Name = selected.Name,
                                 Branch = selected.Branch,
                                 IdealRank = selected.IdealRank,
                                 Skill = selected.Skill,
                                 MaxSkill = selected.MaxSkill,
                                 Experience = selected.Experience,
                                 Loyalty = selected.Loyalty,
                                 StartYear = selected.StartYear,
                                 EndYear = selected.EndYear,
                                 RetirementYear = selected.RetirementYear,
                                 PictureName = selected.PictureName,
                                 Traits = selected.Traits
                             };
            leader.RankYear[0] = selected.RankYear[0];
            leader.RankYear[1] = selected.RankYear[1];
            leader.RankYear[2] = selected.RankYear[2];
            leader.RankYear[3] = selected.RankYear[3];

            // 指揮官リストに項目を挿入する
            Leaders.InsertItem(leader, selected);
            InsertListItem(leader, leaderListView.SelectedIndices[0] + 1);

            // 編集済みフラグを設定する
            Leaders.SetDirtyFlag(leader.Country);
        }

        /// <summary>
        ///     削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRemoveButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }

            var selected = leaderListView.SelectedItems[0].Tag as Leader;
            if (selected == null)
            {
                return;
            }

            // 指揮官リストから項目を削除する
            Leaders.RemoveItem(selected);
            RemoveItem(leaderListView.SelectedIndices[0]);

            // リストから項目がなくなれば編集項目を無効化する
            if (leaderListView.Items.Count == 0)
            {
                DisableEditableItems();
            }

            // 編集済みフラグを設定する
            Leaders.SetDirtyFlag(selected.Country);
        }

        /// <summary>
        ///     先頭へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTopButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }

            // 選択項目がリストの先頭ならば何もしない
            int index = leaderListView.SelectedIndices[0];
            if (index == 0)
            {
                return;
            }

            var selected = leaderListView.SelectedItems[0].Tag as Leader;
            if (selected == null)
            {
                return;
            }

            var top = leaderListView.Items[0].Tag as Leader;
            if (top == null)
            {
                return;
            }

            // 指揮官リストの項目を移動する
            Leaders.MoveItem(selected, top);
            MoveListItem(index, 0);

            // 編集済みフラグを設定する
            Leaders.SetDirtyFlag(selected.Country);
        }

        /// <summary>
        ///     上へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }

            // 選択項目がリストの先頭ならば何もしない
            int index = leaderListView.SelectedIndices[0];
            if (index == 0)
            {
                return;
            }

            var selected = leaderListView.SelectedItems[0].Tag as Leader;
            if (selected == null)
            {
                return;
            }
            var upper = leaderListView.Items[index - 1].Tag as Leader;
            if (upper == null)
            {
                return;
            }

            // 指揮官リストの項目を移動する
            Leaders.MoveItem(selected, upper);
            MoveListItem(index, index - 1);

            // 編集済みフラグを設定する
            Leaders.SetDirtyFlag(selected.Country);
        }

        /// <summary>
        ///     下へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDownButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }

            // 選択項目がリストの末尾ならば何もしない
            int index = leaderListView.SelectedIndices[0];
            if (index == leaderListView.Items.Count - 1)
            {
                return;
            }

            var selected = leaderListView.SelectedItems[0].Tag as Leader;
            if (selected == null)
            {
                return;
            }

            var lower = leaderListView.Items[index + 1].Tag as Leader;
            if (lower == null)
            {
                return;
            }

            // 指揮官リストの項目を移動する
            Leaders.MoveItem(selected, lower);
            MoveListItem(index, index + 1);

            // 編集済みフラグを設定する
            Leaders.SetDirtyFlag(selected.Country);
        }

        /// <summary>
        ///     末尾へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBottomButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }

            // 選択項目がリストの末尾ならば何もしない
            int index = leaderListView.SelectedIndices[0];
            if (index == leaderListView.Items.Count - 1)
            {
                return;
            }

            var selected = leaderListView.Items[index].Tag as Leader;
            if (selected == null)
            {
                return;
            }

            var bottom = leaderListView.Items[leaderListView.Items.Count - 1].Tag as Leader;
            if (bottom == null)
            {
                return;
            }

            // 指揮官リストの項目を移動する
            Leaders.MoveItem(selected, bottom);
            MoveListItem(index, leaderListView.Items.Count - 1);

            // 編集済みフラグを設定する
            Leaders.SetDirtyFlag(selected.Country);
        }

        /// <summary>
        ///     指揮官リストに項目を追加する
        /// </summary>
        /// <param name="leader">挿入対象の項目</param>
        private void AddListItem(Leader leader)
        {
            // 絞り込みリストに項目を追加する
            _narrowedList.Add(leader);

            // 指揮官リストビューに追加する
            leaderListView.Items.Add(CreateLeaderListViewItem(leader));

            // 追加した項目を選択する
            leaderListView.Items[leaderListView.Items.Count - 1].Focused = true;
            leaderListView.Items[leaderListView.Items.Count - 1].Selected = true;
            leaderListView.Items[leaderListView.Items.Count - 1].EnsureVisible();
        }

        /// <summary>
        ///     指揮官リストに項目を挿入する
        /// </summary>
        /// <param name="leader">挿入対象の項目</param>
        /// <param name="index">挿入先の位置</param>
        private void InsertListItem(Leader leader, int index)
        {
            // 絞り込みリストに項目を挿入する
            _narrowedList.Insert(index, leader);

            // 指揮官リストビューに項目を挿入する
            ListViewItem item = CreateLeaderListViewItem(leader);
            leaderListView.Items.Insert(index, item);

            // 挿入した項目を選択する
            leaderListView.Items[index].Focused = true;
            leaderListView.Items[index].Selected = true;
            leaderListView.Items[index].EnsureVisible();
        }

        /// <summary>
        ///     指揮官リストから項目を削除する
        /// </summary>
        /// <param name="index">削除対象の位置</param>
        private void RemoveItem(int index)
        {
            // 絞り込みリストから項目を削除する
            _narrowedList.RemoveAt(index);

            // 指揮官リストビューから項目を削除する
            leaderListView.Items.RemoveAt(index);

            // 削除した項目の次の項目を選択する
            if (index < leaderListView.Items.Count)
            {
                leaderListView.Items[index].Focused = true;
                leaderListView.Items[index].Selected = true;
            }
            else if (index - 1 >= 0)
            {
                // リストの末尾ならば、削除した項目の前の項目を選択する
                leaderListView.Items[index - 1].Focused = true;
                leaderListView.Items[index - 1].Selected = true;
            }
        }

        /// <summary>
        ///     指揮官リストの項目を移動する
        /// </summary>
        /// <param name="src">移動元の位置</param>
        /// <param name="dest">移動先の位置</param>
        private void MoveListItem(int src, int dest)
        {
            Leader leader = _narrowedList[src];
            ListViewItem item = CreateLeaderListViewItem(leader);

            if (src > dest)
            {
                // 上へ移動する場合
                // 絞り込みリストの項目を移動する
                _narrowedList.Insert(dest, leader);
                _narrowedList.RemoveAt(src + 1);

                // 指揮官リストビューの項目を移動する
                leaderListView.Items.Insert(dest, item);
                leaderListView.Items.RemoveAt(src + 1);
            }
            else
            {
                // 下へ移動する場合
                // 絞り込みリストの項目を移動する
                _narrowedList.Insert(dest + 1, leader);
                _narrowedList.RemoveAt(src);

                // 指揮官リストビューの項目を移動する
                leaderListView.Items.Insert(dest + 1, item);
                leaderListView.Items.RemoveAt(src);
            }

            // 移動先の項目を選択する
            leaderListView.Items[dest].Focused = true;
            leaderListView.Items[dest].Selected = true;
            leaderListView.Items[dest].EnsureVisible();
        }

        /// <summary>
        ///     指揮官リストビューの項目を作成する
        /// </summary>
        /// <param name="leader">指揮官データ</param>
        /// <returns>指揮官リストビューの項目</returns>
        private static ListViewItem CreateLeaderListViewItem(Leader leader)
        {
            var item = new ListViewItem
                           {
                               Text = Country.Strings[(int) leader.Country],
                               Tag = leader
                           };
            item.SubItems.Add(leader.Id.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(leader.Name);
            item.SubItems.Add(Leaders.BranchNames[(int) leader.Branch]);
            item.SubItems.Add(leader.Skill.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(leader.MaxSkill.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(leader.StartYear.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(leader.EndYear.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(GetLeaderTraitsText(leader.Traits));

            return item;
        }

        /// <summary>
        ///     指揮官特性文字列を取得する
        /// </summary>
        /// <param name="traits">指揮官特性</param>
        /// <returns>指揮官特性文字列</returns>
        private static string GetLeaderTraitsText(uint traits)
        {
            string s = Enum.GetValues(typeof (LeaderTraitsId))
                           .Cast<LeaderTraitsId>()
                           .Where(id => (traits & Leaders.TraitsValueTable[(int) id]) != 0)
                           .Aggregate("",
                                      (current, id) =>
                                      string.Format("{0}, {1}", current, Config.GetText(Leaders.TraitsNames[(int) id])));
            // 先頭項目の", "を削除する
            if (!string.IsNullOrEmpty(s))
            {
                s = s.Substring(2);
            }

            return s;
        }

        #endregion

        #region 絞り込み

        /// <summary>
        ///     兵科チェックボックスのチェック状態変化時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBranchNarrowCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 指揮官リストを更新する
            NarrowLeaderList();
            UpdateLeaderList();
        }

        /// <summary>
        ///     特性絞り込みチェックボックスのチェック状態変化時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTraitsNarrowCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            if (GetNarrowedTraits() == 0)
            {
                // 特性に1つもチェックがついていなければフィルターなしに変更する
                if (!traitsNoneNarrowRadioButton.Checked)
                {
                    traitsNoneNarrowRadioButton.Checked = true;
                }
            }
            else
            {
                // フィルターなしで特性にチェックがついていればOR条件に変更する
                if (traitsNoneNarrowRadioButton.Checked)
                {
                    traitsOrNarrowRadioButton.Checked = true;
                }
            }

            // 指揮官リストを更新する
            NarrowLeaderList();
            UpdateLeaderList();
        }

        /// <summary>
        ///     特性絞込み条件ラジオボタンのチェック状態変化時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTraitsNarrowRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            // 指揮官リストを更新する
            NarrowLeaderList();
            UpdateLeaderList();
        }

        /// <summary>
        ///     選択反転ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTraitsNarrowInvertButtonClick(object sender, EventArgs e)
        {
            logisticsWizardNarrowCheckBox.Checked = !logisticsWizardNarrowCheckBox.Checked;
            defensiveDoctrineNarrowCheckBox.Checked = !defensiveDoctrineNarrowCheckBox.Checked;
            offensiveDoctrineNarrowCheckBox.Checked = !offensiveDoctrineNarrowCheckBox.Checked;
            winterSpecialistNarrowCheckBox.Checked = !winterSpecialistNarrowCheckBox.Checked;
            tricksterNarrowCheckBox.Checked = !tricksterNarrowCheckBox.Checked;
            engineerNarrowCheckBox.Checked = !engineerNarrowCheckBox.Checked;
            fortressBusterNarrowCheckBox.Checked = !fortressBusterNarrowCheckBox.Checked;
            panzerLeaderNarrowCheckBox.Checked = !panzerLeaderNarrowCheckBox.Checked;
            commandoNarrowCheckBox.Checked = !commandoNarrowCheckBox.Checked;
            oldGuardNarrowCheckBox.Checked = !oldGuardNarrowCheckBox.Checked;
            seaWolfNarrowCheckBox.Checked = !seaWolfNarrowCheckBox.Checked;
            blockadeRunnerNarrowCheckBox.Checked = !blockadeRunnerNarrowCheckBox.Checked;
            superiorTacticianNarrowCheckBox.Checked = !superiorTacticianNarrowCheckBox.Checked;
            spotterNarrowCheckBox.Checked = !spotterNarrowCheckBox.Checked;
            tankBusterNarrowCheckBox.Checked = !tankBusterNarrowCheckBox.Checked;
            carpetBomberNarrowCheckBox.Checked = !carpetBomberNarrowCheckBox.Checked;
            nightFlyerNarrowCheckBox.Checked = !nightFlyerNarrowCheckBox.Checked;
            fleetDestroyerNarrowCheckBox.Checked = !fleetDestroyerNarrowCheckBox.Checked;
            desertFoxNarrowCheckBox.Checked = !desertFoxNarrowCheckBox.Checked;
            jungleRatNarrowCheckBox.Checked = !jungleRatNarrowCheckBox.Checked;
            urbanWarfareSpecialistNarrowCheckBox.Checked = !urbanWarfareSpecialistNarrowCheckBox.Checked;
            rangerNarrowCheckBox.Checked = !rangerNarrowCheckBox.Checked;
            mountaineerNarrowCheckBox.Checked = !mountaineerNarrowCheckBox.Checked;
            hillsFighterNarrowCheckBox.Checked = !hillsFighterNarrowCheckBox.Checked;
            counterAttackerNarrowCheckBox.Checked = !counterAttackerNarrowCheckBox.Checked;
            assaulterNarrowCheckBox.Checked = !assaulterNarrowCheckBox.Checked;
            encirclerNarrowCheckBox.Checked = !encirclerNarrowCheckBox.Checked;
            ambusherNarrowCheckBox.Checked = !ambusherNarrowCheckBox.Checked;
            disciplinedNarrowCheckBox.Checked = !disciplinedNarrowCheckBox.Checked;
            elasticDefenceSpecialistNarrowCheckBox.Checked = !elasticDefenceSpecialistNarrowCheckBox.Checked;
            blitzerNarrowCheckBox.Checked = !blitzerNarrowCheckBox.Checked;
        }

        /// <summary>
        ///     指揮官特性文字列を初期化する
        /// </summary>
        private void InitTraitsText()
        {
            logisticsWizardCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.LogisticsWizard]);
            defensiveDoctrineCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.DefensiveDoctrine]);
            offensiveDoctrineCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.OffensiveDoctrine]);
            winterSpecialistCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.WinterSpecialist]);
            tricksterCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.Trickster]);
            engineerCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.Engineer]);
            fortressBusterCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.FortressBuster]);
            panzerLeaderCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.PanzerLeader]);
            commandoCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.Commando]);
            oldGuardCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.OldGuard]);
            seaWolfCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.SeaWolf]);
            blockadeRunnerCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.BlockadeRunner]);
            superiorTacticianCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.SuperiorTactician]);
            spotterCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.Spotter]);
            tankBusterCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.TankBuster]);
            carpetBomberCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.CarpetBomber]);
            nightFlyerCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.NightFlyer]);
            fleetDestroyerCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.FleetDestroyer]);
            desertFoxCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.DesertFox]);
            jungleRatCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.JungleRat]);
            urbanWarfareSpecialistCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.UrbanWarfareSpecialist]);
            rangerCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.Ranger]);
            mountaineerCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.Mountaineer]);
            hillsFighterCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.HillsFighter]);
            counterAttackerCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.CounterAttacker]);
            assaulterCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.Assaulter]);
            encirclerCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.Encircler]);
            ambusherCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.Ambusher]);
            disciplinedCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.Disciplined]);
            elasticDefenceSpecialistCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.ElasticDefenceSpecialist]);
            blitzerCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.Blitzer]);

            logisticsWizardNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.LogisticsWizard]);
            defensiveDoctrineNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.DefensiveDoctrine]);
            offensiveDoctrineNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.OffensiveDoctrine]);
            winterSpecialistNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.WinterSpecialist]);
            tricksterNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.Trickster]);
            engineerNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.Engineer]);
            fortressBusterNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.FortressBuster]);
            panzerLeaderNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.PanzerLeader]);
            commandoNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.Commando]);
            oldGuardNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.OldGuard]);
            seaWolfNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.SeaWolf]);
            blockadeRunnerNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.BlockadeRunner]);
            superiorTacticianNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.SuperiorTactician]);
            spotterNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.Spotter]);
            tankBusterNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.TankBuster]);
            carpetBomberNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.CarpetBomber]);
            nightFlyerNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.NightFlyer]);
            fleetDestroyerNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.FleetDestroyer]);
            desertFoxNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.DesertFox]);
            jungleRatNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.JungleRat]);
            urbanWarfareSpecialistNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.UrbanWarfareSpecialist]);
            rangerNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.Ranger]);
            mountaineerNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.Mountaineer]);
            hillsFighterNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.HillsFighter]);
            counterAttackerNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.CounterAttacker]);
            assaulterNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.Assaulter]);
            encirclerNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.Encircler]);
            ambusherNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.Ambusher]);
            disciplinedNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.Disciplined]);
            elasticDefenceSpecialistNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.ElasticDefenceSpecialist]);
            blitzerNarrowCheckBox.Text =
                Config.GetText(Leaders.TraitsNames[(int) LeaderTraitsId.Blitzer]);
        }

        /// <summary>
        ///     絞り込まれた指揮官特性を取得する
        /// </summary>
        /// <returns>指揮官特性</returns>
        private uint GetNarrowedTraits()
        {
            uint traits = 0;

            // 兵站管理
            if (logisticsWizardNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.LogisticsWizard;
            }
            // 防勢ドクトリン
            if (defensiveDoctrineNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.DefensiveDoctrine;
            }
            // 攻勢ドクトリン
            if (offensiveDoctrineNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.OffensiveDoctrine;
            }
            // 冬期戦
            if (winterSpecialistNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.WinterSpecialist;
            }
            // 伏撃
            if (tricksterNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.Trickster;
            }
            // 工兵
            if (engineerNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.Engineer;
            }
            // 要塞攻撃
            if (fortressBusterNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.FortressBuster;
            }
            // 機甲戦
            if (panzerLeaderNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.PanzerLeader;
            }
            // 特殊戦
            if (commandoNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.Commando;
            }
            // 古典派
            if (oldGuardNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.OldGuard;
            }
            // 海狼
            if (seaWolfNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.SeaWolf;
            }
            // 封鎖線突破の達人
            if (blockadeRunnerNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.BlockadeRunner;
            }
            // 卓越した戦術家
            if (superiorTacticianNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.SuperiorTactician;
            }
            // 索敵
            if (spotterNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.Spotter;
            }
            // 対戦車攻撃
            if (tankBusterNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.TankBuster;
            }
            // 絨毯爆撃
            if (carpetBomberNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.CarpetBomber;
            }
            // 夜間航空作戦
            if (nightFlyerNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.NightFlyer;
            }
            // 対艦攻撃
            if (fleetDestroyerNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.FleetDestroyer;
            }
            // 砂漠のキツネ
            if (desertFoxNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.DesertFox;
            }
            // 密林のネズミ
            if (jungleRatNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.JungleRat;
            }
            // 市街戦
            if (urbanWarfareSpecialistNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.UrbanWarfareSpecialist;
            }
            // レンジャー
            if (rangerNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.Ranger;
            }
            // 山岳戦
            if (mountaineerNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.Mountaineer;
            }
            // 高地戦
            if (hillsFighterNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.HillsFighter;
            }
            // 反撃戦
            if (counterAttackerNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.CounterAttacker;
            }
            // 突撃戦
            if (assaulterNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.Assaulter;
            }
            // 包囲戦
            if (encirclerNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.Encircler;
            }
            // 奇襲戦
            if (ambusherNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.Ambusher;
            }
            // 規律
            if (disciplinedNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.Disciplined;
            }
            // 戦術的退却
            if (elasticDefenceSpecialistNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.ElasticDefenceSpecialist;
            }
            // 電撃戦
            if (blitzerNarrowCheckBox.Checked)
            {
                traits |= LeaderTraits.Blitzer;
            }

            return traits;
        }

        #endregion

        #region 国家リストボックス

        /// <summary>
        ///     国家リストボックスを初期化する
        /// </summary>
        private void InitCountryListBox()
        {
            foreach (string name in Country.Tags
                                           .Select(country => Country.Strings[(int) country])
                                           .Where(name => !string.IsNullOrEmpty(name)))
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
            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            if (e.Index != -1)
            {
                Brush brush;
                if ((e.State & DrawItemState.Selected) != DrawItemState.Selected)
                {
                    // 変更ありの項目は文字色を変更する
                    CountryTag country = Country.Tags[e.Index + 1];
                    brush = Leaders.DirtyFlags[(int) country]
                                ? new SolidBrush(Color.Red)
                                : new SolidBrush(SystemColors.WindowText);
                }
                else
                {
                    brush = new SolidBrush(SystemColors.HighlightText);
                }
                string s = countryListBox.Items[e.Index].ToString();
                e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
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
            // 選択数に合わせて全選択/全解除を切り替える
            countryAllButton.Text = countryListBox.SelectedItems.Count <= 1
                                        ? Resources.KeySelectAll
                                        : Resources.KeyUnselectAll;

            // 選択数がゼロの場合は新規追加ボタンを無効化する
            newButton.Enabled = countryListBox.SelectedItems.Count > 0;

            // 指揮官リストを更新する
            NarrowLeaderList();
            UpdateLeaderList();
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

            // 指揮官リスト絞り込みのため、ダミーでイベント発行する
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
            int maxSize = countryComboBox.DropDownWidth;
            foreach (string s in Country.Tags
                                        .Select(country => Country.Strings[(int) country])
                                        .Select(name => Config.ExistsKey(name)
                                                            ? string.Format("{0} {1}", name, Config.GetText(name))
                                                            : name))
            {
                countryComboBox.Items.Add(s);
                maxSize = Math.Max(maxSize,
                                   TextRenderer.MeasureText(s, countryComboBox.Font).Width +
                                   SystemInformation.VerticalScrollBarWidth);
            }
            countryComboBox.DropDownWidth = maxSize;

            // 兵科
            maxSize = branchComboBox.DropDownWidth;
            foreach (string name in Leaders.BranchNames.Where(name => !string.IsNullOrEmpty(name)))
            {
                branchComboBox.Items.Add(name);
                maxSize = Math.Max(maxSize, TextRenderer.MeasureText(name, branchComboBox.Font).Width);
            }
            branchComboBox.DropDownWidth = maxSize;

            // 階級
            maxSize = idealRankComboBox.DropDownWidth;
            foreach (string name in Leaders.RankNames.Where(name => !string.IsNullOrEmpty(name)))
            {
                idealRankComboBox.Items.Add(name);
                maxSize = Math.Max(maxSize, TextRenderer.MeasureText(name, idealRankComboBox.Font).Width);
            }
            idealRankComboBox.DropDownWidth = maxSize;
        }

        /// <summary>
        ///     編集可能な項目を有効化する
        /// </summary>
        private void EnableEditableItems()
        {
            countryComboBox.Enabled = true;
            idNumericUpDown.Enabled = true;
            nameTextBox.Enabled = true;
            branchComboBox.Enabled = true;
            idealRankComboBox.Enabled = true;
            skillNumericUpDown.Enabled = true;
            maxSkillNumericUpDown.Enabled = true;
            experienceNumericUpDown.Enabled = true;
            loyaltyNumericUpDown.Enabled = true;
            startYearNumericUpDown.Enabled = true;
            endYearNumericUpDown.Enabled = true;
            rankYearNumericUpDown1.Enabled = true;
            rankYearNumericUpDown2.Enabled = true;
            rankYearNumericUpDown3.Enabled = true;
            rankYearNumericUpDown4.Enabled = true;
            pictureNameTextBox.Enabled = true;
            pictureNameReferButton.Enabled = true;
            traitsGroupBox.Enabled = true;

            // 無効化時にクリアした文字列を再設定する
            idNumericUpDown.Text = idNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            skillNumericUpDown.Text = skillNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            maxSkillNumericUpDown.Text = maxSkillNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            experienceNumericUpDown.Text = experienceNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            loyaltyNumericUpDown.Text = loyaltyNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            startYearNumericUpDown.Text = startYearNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            endYearNumericUpDown.Text = endYearNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
            rankYearNumericUpDown1.Text = rankYearNumericUpDown1.Value.ToString(CultureInfo.InvariantCulture);
            rankYearNumericUpDown2.Text = rankYearNumericUpDown2.Value.ToString(CultureInfo.InvariantCulture);
            rankYearNumericUpDown3.Text = rankYearNumericUpDown3.Value.ToString(CultureInfo.InvariantCulture);
            rankYearNumericUpDown4.Text = rankYearNumericUpDown4.Value.ToString(CultureInfo.InvariantCulture);

            if (Misc.Mod.RetirementYearLeader)
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
            branchComboBox.SelectedIndex = -1;
            branchComboBox.ResetText();
            idealRankComboBox.SelectedIndex = -1;
            idealRankComboBox.ResetText();
            skillNumericUpDown.ResetText();
            maxSkillNumericUpDown.ResetText();
            experienceNumericUpDown.ResetText();
            loyaltyNumericUpDown.ResetText();
            startYearNumericUpDown.ResetText();
            endYearNumericUpDown.ResetText();
            retirementYearNumericUpDown.ResetText();
            rankYearNumericUpDown1.ResetText();
            rankYearNumericUpDown2.ResetText();
            rankYearNumericUpDown3.ResetText();
            rankYearNumericUpDown4.ResetText();
            pictureNameTextBox.ResetText();
            leaderPictureBox.ImageLocation = "";

            SetTraitsCheckBoxValue(false);

            countryComboBox.Enabled = false;
            idNumericUpDown.Enabled = false;
            nameTextBox.Enabled = false;
            branchComboBox.Enabled = false;
            idealRankComboBox.Enabled = false;
            skillNumericUpDown.Enabled = false;
            maxSkillNumericUpDown.Enabled = false;
            experienceNumericUpDown.Enabled = false;
            loyaltyNumericUpDown.Enabled = false;
            startYearNumericUpDown.Enabled = false;
            endYearNumericUpDown.Enabled = false;
            retirementYearNumericUpDown.Enabled = false;
            rankYearNumericUpDown1.Enabled = false;
            rankYearNumericUpDown2.Enabled = false;
            rankYearNumericUpDown3.Enabled = false;
            rankYearNumericUpDown4.Enabled = false;
            pictureNameTextBox.Enabled = false;
            pictureNameReferButton.Enabled = false;
            traitsGroupBox.Enabled = false;

            cloneButton.Enabled = false;
            removeButton.Enabled = false;
            topButton.Enabled = false;
            upButton.Enabled = false;
            downButton.Enabled = false;
            bottomButton.Enabled = false;
        }

        /// <summary>
        ///     国家コンボボックスの項目を更新する
        /// </summary>
        /// <param name="leader">指揮官データ</param>
        private void UpdateCountryComboBox(Leader leader)
        {
            countryComboBox.BeginUpdate();

            if (leader.Country != CountryTag.None)
            {
                // CountryTag.Noneの分インデックスを-1する
                countryComboBox.SelectedIndex = (int) (leader.Country - 1);
            }
            else
            {
                countryComboBox.SelectedIndex = -1;
            }

            countryComboBox.EndUpdate();
        }

        /// <summary>
        ///     兵科コンボボックスの項目を更新する
        /// </summary>
        /// <param name="leader">指揮官データ</param>
        private void UpdateBranchComboBox(Leader leader)
        {
            branchComboBox.BeginUpdate();

            if (leader.Branch != LeaderBranch.None)
            {
                // LeaderBranch.Noneの分インデックスを-1する
                branchComboBox.SelectedIndex = (int) (leader.Branch - 1);
            }
            else
            {
                branchComboBox.SelectedIndex = -1;
            }

            branchComboBox.EndUpdate();
        }

        /// <summary>
        ///     理想階級コンボボックスの項目を更新する
        /// </summary>
        /// <param name="leader">指揮官データ</param>
        private void UpdateIdealRankComboBox(Leader leader)
        {
            idealRankComboBox.BeginUpdate();

            if (leader.IdealRank != LeaderRank.None)
            {
                // LeaderRank.Noneの分インデックスを-1する
                idealRankComboBox.SelectedIndex = (int) (leader.IdealRank - 1);
            }
            else
            {
                idealRankComboBox.SelectedIndex = -1;
            }

            idealRankComboBox.EndUpdate();
        }

        /// <summary>
        ///     指揮官画像ピクチャーボックスの項目を更新する
        /// </summary>
        /// <param name="leader">指揮官データ</param>
        private void UpdateLeaderPicture(Leader leader)
        {
            if (!string.IsNullOrEmpty(leader.PictureName))
            {
                string fileName =
                    Game.GetReadFileName(Path.Combine(Game.PersonPicturePathName,
                                                      Path.ChangeExtension(leader.PictureName, ".bmp")));
                leaderPictureBox.ImageLocation = File.Exists(fileName) ? fileName : "";
            }
            else
            {
                leaderPictureBox.ImageLocation = "";
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
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }

            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            CountryTag country = Country.Tags[countryComboBox.SelectedIndex];
            if (country == leader.Country)
            {
                return;
            }

            // 変更前の国タグの編集済みフラグを設定する
            Leaders.SetDirtyFlag(leader.Country);

            // 値を更新する
            leader.Country = country;

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].Text = Country.Strings[(int) leader.Country];

            // 変更後の国タグの編集済みフラグを設定する
            Leaders.SetDirtyFlag(leader.Country);

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
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }

            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var id = (int) idNumericUpDown.Value;
            if (id == leader.Id)
            {
                return;
            }

            // 値を更新する
            leader.Id = id;

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[1].Text = leader.Id.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            Leaders.SetDirtyFlag(leader.Country);
        }

        /// <summary>
        ///     名前文字列変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNameTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }

            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            string name = nameTextBox.Text;
            if (name.Equals(leader.Name))
            {
                return;
            }

            // 値を更新する
            leader.Name = name;

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[2].Text = leader.Name;

            // 編集済みフラグを設定する
            Leaders.SetDirtyFlag(leader.Country);
        }

        /// <summary>
        ///     兵科変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBranchComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }

            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var branch = (LeaderBranch) (branchComboBox.SelectedIndex + 1);
            if (branch == leader.Branch)
            {
                return;
            }

            // 値を更新する
            leader.Branch = branch;

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[3].Text = Leaders.BranchNames[(int) leader.Branch];

            // 編集済みフラグを設定する
            Leaders.SetDirtyFlag(leader.Country);
        }

        /// <summary>
        ///     理想階級変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIdealRankComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }

            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var idealRank = (LeaderRank) (idealRankComboBox.SelectedIndex + 1);
            if (idealRank == leader.IdealRank)
            {
                return;
            }

            // 値を更新する
            leader.IdealRank = idealRank;

            // 指揮官リストビューの項目を更新する
            UpdateIdealRankComboBox(leader);

            // 編集済みフラグを設定する
            Leaders.SetDirtyFlag(leader.Country);
        }

        /// <summary>
        ///     スキル変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSkillNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }

            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var skill = (int) skillNumericUpDown.Value;
            if (skill == leader.Skill)
            {
                return;
            }

            // 値を更新する
            leader.Skill = skill;

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[4].Text = leader.Skill.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            Leaders.SetDirtyFlag(leader.Country);
        }

        /// <summary>
        ///     最大スキル変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMaxSkillNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }

            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var maxSkill = (int) maxSkillNumericUpDown.Value;
            if (maxSkill == leader.MaxSkill)
            {
                return;
            }

            // 値を更新する
            leader.MaxSkill = maxSkill;

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[5].Text = leader.MaxSkill.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            Leaders.SetDirtyFlag(leader.Country);
        }

        /// <summary>
        ///     経験値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExperienceNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }

            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var experience = (int) experienceNumericUpDown.Value;
            if (experience == leader.Experience)
            {
                return;
            }

            // 値を更新する
            leader.Experience = experience;

            // 編集済みフラグを設定する
            Leaders.SetDirtyFlag(leader.Country);
        }

        /// <summary>
        ///     忠誠度変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoyaltyNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }

            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }
            // 値に変化がなければ何もしない
            var loyalty = (int) loyaltyNumericUpDown.Value;
            if (loyalty == leader.Loyalty)
            {
                return;
            }

            // 値を更新する
            leader.Loyalty = loyalty;

            // 編集済みフラグを設定する
            Leaders.SetDirtyFlag(leader.Country);
        }

        /// <summary>
        ///     開始年変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStartYearNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }

            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var startYear = (int) startYearNumericUpDown.Value;
            if (startYear == leader.StartYear)
            {
                return;
            }

            // 値を更新する
            leader.StartYear = startYear;

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[6].Text = leader.StartYear.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            Leaders.SetDirtyFlag(leader.Country);
        }

        /// <summary>
        ///     終了年変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEndYearNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }

            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var endYear = (int) endYearNumericUpDown.Value;
            if (endYear == leader.EndYear)
            {
                return;
            }

            // 値を更新する
            leader.EndYear = endYear;

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[7].Text = leader.EndYear.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            Leaders.SetDirtyFlag(leader.Country);
        }

        /// <summary>
        ///     引退年変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRetirementYearNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }

            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var retirementYear = (int) retirementYearNumericUpDown.Value;
            if (retirementYear == leader.RetirementYear)
            {
                return;
            }

            // 値を更新する
            leader.RetirementYear = retirementYear;

            // 編集済みフラグを設定する
            Leaders.SetDirtyFlag(leader.Country);
        }

        /// <summary>
        ///     少将任官年変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRankYearNumericUpDown1ValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }

            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var rankYear = (int) rankYearNumericUpDown1.Value;
            if (rankYear == leader.RankYear[0])
            {
                return;
            }

            // 値を更新する
            leader.RankYear[0] = rankYear;

            // 編集済みフラグを設定する
            Leaders.SetDirtyFlag(leader.Country);
        }

        /// <summary>
        ///     中将任官年変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRankYearNumericUpDown2ValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }

            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var newRankYear = (int) rankYearNumericUpDown2.Value;
            if (newRankYear == leader.RankYear[1])
            {
                return;
            }

            // 値を更新する
            leader.RankYear[1] = newRankYear;

            // 編集済みフラグを設定する
            Leaders.SetDirtyFlag(leader.Country);
        }

        /// <summary>
        ///     大将任官年変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRankYearNumericUpDown3ValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }

            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var newRankYear = (int) rankYearNumericUpDown3.Value;
            if (newRankYear == leader.RankYear[2])
            {
                return;
            }

            // 値を更新する
            leader.RankYear[2] = newRankYear;

            // 編集済みフラグを設定する
            Leaders.SetDirtyFlag(leader.Country);
        }

        /// <summary>
        ///     元帥任官年変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRankYearNumericUpDown4ValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }

            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var newRankYear = (int) rankYearNumericUpDown4.Value;
            if (newRankYear == leader.RankYear[3])
            {
                return;
            }

            // 値を更新する
            leader.RankYear[3] = newRankYear;

            // 編集済みフラグを設定する
            Leaders.SetDirtyFlag(leader.Country);
        }

        /// <summary>
        ///     特性変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTraitsCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }

            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint traits = GetCheckedLeaderTraits();
            if (traits == leader.Traits)
            {
                return;
            }

            // 値を更新する
            leader.Traits = traits;

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            Leaders.SetDirtyFlag(leader.Country);
        }

        /// <summary>
        ///     画像ファイル名変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPictureNameTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }

            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            string pictureName = pictureNameTextBox.Text;
            if (pictureName.Equals(leader.PictureName))
            {
                return;
            }

            // 値を更新する
            leader.PictureName = pictureName;

            // 指揮官画像を更新する
            UpdateLeaderPicture(leader);

            // 編集済みフラグを設定する
            Leaders.SetDirtyFlag(leader.Country);
        }

        /// <summary>
        ///     画像ファイル名参照ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPictureNameReferButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }

            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }

            // ファイル選択ダイアログを開く
            var dialog = new OpenFileDialog
                             {
                                 InitialDirectory = Path.Combine(Game.FolderName, Game.PersonPicturePathName),
                                 FileName = leader.PictureName,
                                 Filter = Resources.OpenBitmapFileDialogFilter
                             };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                pictureNameTextBox.Text = Path.GetFileNameWithoutExtension(dialog.FileName);
            }
        }

        /// <summary>
        ///     指揮官特性チェックボックスの値を一括設定する
        /// </summary>
        /// <param name="flag">一括設定する値</param>
        private void SetTraitsCheckBoxValue(bool flag)
        {
            logisticsWizardCheckBox.Checked = flag;
            defensiveDoctrineCheckBox.Checked = flag;
            offensiveDoctrineCheckBox.Checked = flag;
            winterSpecialistCheckBox.Checked = flag;
            tricksterCheckBox.Checked = flag;
            engineerCheckBox.Checked = flag;
            fortressBusterCheckBox.Checked = flag;
            panzerLeaderCheckBox.Checked = flag;
            commandoCheckBox.Checked = flag;
            oldGuardCheckBox.Checked = flag;
            seaWolfCheckBox.Checked = flag;
            blockadeRunnerCheckBox.Checked = flag;
            superiorTacticianCheckBox.Checked = flag;
            spotterCheckBox.Checked = flag;
            tankBusterCheckBox.Checked = flag;
            carpetBomberCheckBox.Checked = flag;
            nightFlyerCheckBox.Checked = flag;
            fleetDestroyerCheckBox.Checked = flag;
            desertFoxCheckBox.Checked = flag;
            jungleRatCheckBox.Checked = flag;
            urbanWarfareSpecialistCheckBox.Checked = flag;
            rangerCheckBox.Checked = flag;
            mountaineerCheckBox.Checked = flag;
            hillsFighterCheckBox.Checked = flag;
            counterAttackerCheckBox.Checked = flag;
            assaulterCheckBox.Checked = flag;
            encirclerCheckBox.Checked = flag;
            ambusherCheckBox.Checked = flag;
            disciplinedCheckBox.Checked = flag;
            elasticDefenceSpecialistCheckBox.Checked = flag;
            blitzerCheckBox.Checked = flag;
        }

        /// <summary>
        ///     指揮官特性チェックボックスのイベントを一括設定する
        /// </summary>
        /// <param name="flag">イベントの有効/無効</param>
        private void SetTraitsCheckBoxEvent(bool flag)
        {
            if (flag)
            {
                logisticsWizardCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
                defensiveDoctrineCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
                offensiveDoctrineCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
                winterSpecialistCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
                tricksterCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
                engineerCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
                fortressBusterCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
                panzerLeaderCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
                commandoCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
                oldGuardCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
                seaWolfCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
                blockadeRunnerCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
                superiorTacticianCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
                spotterCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
                tankBusterCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
                carpetBomberCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
                nightFlyerCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
                fleetDestroyerCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
                desertFoxCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
                jungleRatCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
                urbanWarfareSpecialistCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
                rangerCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
                mountaineerCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
                hillsFighterCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
                counterAttackerCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
                assaulterCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
                encirclerCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
                ambusherCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
                disciplinedCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
                elasticDefenceSpecialistCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
                blitzerCheckBox.CheckedChanged += OnTraitsCheckBoxCheckedChanged;
            }
            else
            {
                logisticsWizardCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
                defensiveDoctrineCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
                offensiveDoctrineCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
                winterSpecialistCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
                tricksterCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
                engineerCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
                fortressBusterCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
                panzerLeaderCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
                commandoCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
                oldGuardCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
                seaWolfCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
                blockadeRunnerCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
                superiorTacticianCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
                spotterCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
                tankBusterCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
                carpetBomberCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
                nightFlyerCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
                fleetDestroyerCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
                desertFoxCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
                jungleRatCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
                urbanWarfareSpecialistCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
                rangerCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
                mountaineerCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
                hillsFighterCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
                counterAttackerCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
                assaulterCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
                encirclerCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
                ambusherCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
                disciplinedCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
                elasticDefenceSpecialistCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
                blitzerCheckBox.CheckedChanged -= OnTraitsCheckBoxCheckedChanged;
            }
        }

        /// <summary>
        ///     選択された指揮官特性を取得する
        /// </summary>
        /// <returns>指揮官特性</returns>
        private uint GetCheckedLeaderTraits()
        {
            uint traits = 0;

            // 兵站管理
            if (logisticsWizardCheckBox.Checked)
            {
                traits |= LeaderTraits.LogisticsWizard;
            }
            // 防勢ドクトリン
            if (defensiveDoctrineCheckBox.Checked)
            {
                traits |= LeaderTraits.DefensiveDoctrine;
            }
            // 攻勢ドクトリン
            if (offensiveDoctrineCheckBox.Checked)
            {
                traits |= LeaderTraits.OffensiveDoctrine;
            }
            // 冬期戦
            if (winterSpecialistCheckBox.Checked)
            {
                traits |= LeaderTraits.WinterSpecialist;
            }
            // 伏撃
            if (tricksterCheckBox.Checked)
            {
                traits |= LeaderTraits.Trickster;
            }
            // 工兵
            if (engineerCheckBox.Checked)
            {
                traits |= LeaderTraits.Engineer;
            }
            // 要塞攻撃
            if (fortressBusterCheckBox.Checked)
            {
                traits |= LeaderTraits.FortressBuster;
            }
            // 機甲戦
            if (panzerLeaderCheckBox.Checked)
            {
                traits |= LeaderTraits.PanzerLeader;
            }
            // 特殊戦
            if (commandoCheckBox.Checked)
            {
                traits |= LeaderTraits.Commando;
            }
            // 古典派
            if (oldGuardCheckBox.Checked)
            {
                traits |= LeaderTraits.OldGuard;
            }
            // 海狼
            if (seaWolfCheckBox.Checked)
            {
                traits |= LeaderTraits.SeaWolf;
            }
            // 封鎖線突破の達人
            if (blockadeRunnerCheckBox.Checked)
            {
                traits |= LeaderTraits.BlockadeRunner;
            }
            // 卓越した戦術家
            if (superiorTacticianCheckBox.Checked)
            {
                traits |= LeaderTraits.SuperiorTactician;
            }
            // 索敵
            if (spotterCheckBox.Checked)
            {
                traits |= LeaderTraits.Spotter;
            }
            // 対戦車攻撃
            if (tankBusterCheckBox.Checked)
            {
                traits |= LeaderTraits.TankBuster;
            }
            // 絨毯爆撃
            if (carpetBomberCheckBox.Checked)
            {
                traits |= LeaderTraits.CarpetBomber;
            }
            // 夜間航空作戦
            if (nightFlyerCheckBox.Checked)
            {
                traits |= LeaderTraits.NightFlyer;
            }
            // 対艦攻撃
            if (fleetDestroyerCheckBox.Checked)
            {
                traits |= LeaderTraits.FleetDestroyer;
            }
            // 砂漠のキツネ
            if (desertFoxCheckBox.Checked)
            {
                traits |= LeaderTraits.DesertFox;
            }
            // 密林のネズミ
            if (jungleRatCheckBox.Checked)
            {
                traits |= LeaderTraits.JungleRat;
            }
            // 市街戦
            if (urbanWarfareSpecialistCheckBox.Checked)
            {
                traits |= LeaderTraits.UrbanWarfareSpecialist;
            }
            // レンジャー
            if (rangerCheckBox.Checked)
            {
                traits |= LeaderTraits.Ranger;
            }
            // 山岳戦
            if (mountaineerCheckBox.Checked)
            {
                traits |= LeaderTraits.Mountaineer;
            }
            // 高地戦
            if (hillsFighterCheckBox.Checked)
            {
                traits |= LeaderTraits.HillsFighter;
            }
            // 反撃戦
            if (counterAttackerCheckBox.Checked)
            {
                traits |= LeaderTraits.CounterAttacker;
            }
            // 突撃戦
            if (assaulterCheckBox.Checked)
            {
                traits |= LeaderTraits.Assaulter;
            }
            // 包囲戦
            if (encirclerCheckBox.Checked)
            {
                traits |= LeaderTraits.Encircler;
            }
            // 奇襲戦
            if (ambusherCheckBox.Checked)
            {
                traits |= LeaderTraits.Ambusher;
            }
            // 規律
            if (disciplinedCheckBox.Checked)
            {
                traits |= LeaderTraits.Disciplined;
            }
            // 戦術的退却
            if (elasticDefenceSpecialistCheckBox.Checked)
            {
                traits |= LeaderTraits.ElasticDefenceSpecialist;
            }
            // 電撃戦
            if (blitzerCheckBox.Checked)
            {
                traits |= LeaderTraits.Blitzer;
            }

            return traits;
        }

        #endregion
    }
}