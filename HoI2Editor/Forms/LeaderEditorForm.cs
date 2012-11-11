﻿using System;
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
    /// 指揮官エディタのフォーム
    /// </summary>
    public partial class LeaderEditorForm : Form
    {
        /// <summary>
        /// 指揮官編集フラグ
        /// </summary>
        private readonly bool[] _dirtyFlags = new bool[Enum.GetValues(typeof (CountryTag)).Length];

        /// <summary>
        /// 絞り込み後の指揮官リスト
        /// </summary>
        private readonly List<Leader> _narrowedLeaderList = new List<Leader>();

        /// <summary>
        /// マスター指揮官リスト
        /// </summary>
        private List<Leader> _masterLeaderList = new List<Leader>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LeaderEditorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 指揮官ファイルを読み込む
        /// </summary>
        private void LoadLeaderFiles()
        {
            _masterLeaderList = Leader.LoadLeaderFiles();

            ClearDirtyFlags();

            NarrowLeaderList();
            UpdateLeaderList();
        }

        /// <summary>
        /// 編集フラグをセットする
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
        /// 編集フラグをクリアする
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
        /// 編集フラグを全てクリアする
        /// </summary>
        private void ClearDirtyFlags()
        {
            foreach (CountryTag countryTag in Enum.GetValues(typeof (CountryTag)))
            {
                ClearDirtyFlag(countryTag);
            }
        }

        /// <summary>
        /// 指揮官リストを絞り込む
        /// </summary>
        private void NarrowLeaderList()
        {
            _narrowedLeaderList.Clear();
            uint traitsMask = GetNarrowedLeaderTraits();

            List<CountryTag> selectedTagList;
            if (countryListBox.SelectedItems.Count == 0)
            {
                selectedTagList = new List<CountryTag> {CountryTag.None};
            }
            else
            {
                selectedTagList =
                    (from string countryText in countryListBox.SelectedItems select Country.CountryTextMap[countryText])
                        .ToList();
            }

            foreach (Leader leader in _masterLeaderList)
            {
                // 国タグによる絞り込み
                if (!selectedTagList.Contains(leader.CountryTag))
                {
                    continue;
                }

                // 兵科による絞り込み
                switch (leader.Branch)
                {
                    case LeaderBranch.None:
                        if (!armyNarrowCheckBox.Checked || !navyNarrowCheckBox.Checked ||
                            !airforceNarrowCheckBox.Checked)
                        {
                            continue;
                        }
                        break;
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
                }

                // 指揮官特性による絞り込み
                if (traitsOrNarrowRadioButton.Checked)
                {
                    if ((leader.Traits & traitsMask) == 0)
                    {
                        continue;
                    }
                }
                else if (traitsAndNarrowRadioButton.Checked)
                {
                    if ((leader.Traits & traitsMask) != traitsMask)
                    {
                        continue;
                    }
                }

                _narrowedLeaderList.Add(leader);
            }
        }

        /// <summary>
        /// 指揮官リストの表示を更新する
        /// </summary>
        private void UpdateLeaderList()
        {
            leaderListView.BeginUpdate();
            leaderListView.Items.Clear();

            foreach (Leader leader in _narrowedLeaderList)
            {
                AddLeaderListViewItem(leader);
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
        /// 指揮官特性文字列を更新する
        /// </summary>
        private void InitTraitsText()
        {
            logisticsWizardCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.LogisticsWizard]];
            defensiveDoctrineCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.DefensiveDoctrine]];
            offensiveDoctrineCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.OffensiveDoctrine]];
            winterSpecialistCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.WinterSpecialist]];
            tricksterCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.Trickster]];
            engineerCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.Engineer]];
            fortressBusterCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.FortressBuster]];
            panzerLeaderCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.PanzerLeader]];
            commandoCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.Commando]];
            oldGuardCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.OldGuard]];
            seaWolfCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.SeaWolf]];
            blockadeRunnerCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.BlockadeRunner]];
            superiorTacticianCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.SuperiorTactician]];
            spotterCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.Spotter]];
            tankBusterCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.TankBuster]];
            carpetBomberCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.CarpetBomber]];
            nightFlyerCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.NightFlyer]];
            fleetDestroyerCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.FleetDestroyer]];
            desertFoxCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.DesertFox]];
            jungleRatCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.JungleRat]];
            urbanWarfareSpecialistCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.UrbanWarfareSpecialist]];
            rangerCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.Ranger]];
            mountaineerCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.Mountaineer]];
            hillsFighterCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.HillsFighter]];
            counterAttackerCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.CounterAttacker]];
            assaulterCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.Assaulter]];
            encirclerCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.Encircler]];
            ambusherCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.Ambusher]];
            disciplinedCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.Disciplined]];
            elasticDefenceSpecialistCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.ElasticDefenceSpecialist]];
            blitzerCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.Blitzer]];

            logisticsWizardNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.LogisticsWizard]];
            defensiveDoctrineNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.DefensiveDoctrine]];
            offensiveDoctrineNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.OffensiveDoctrine]];
            winterSpecialistNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.WinterSpecialist]];
            tricksterNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.Trickster]];
            engineerNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.Engineer]];
            fortressBusterNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.FortressBuster]];
            panzerLeaderNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.PanzerLeader]];
            commandoNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.Commando]];
            oldGuardNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.OldGuard]];
            seaWolfNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.SeaWolf]];
            blockadeRunnerNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.BlockadeRunner]];
            superiorTacticianNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.SuperiorTactician]];
            spotterNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.Spotter]];
            tankBusterNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.TankBuster]];
            carpetBomberNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.CarpetBomber]];
            nightFlyerNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.NightFlyer]];
            fleetDestroyerNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.FleetDestroyer]];
            desertFoxNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.DesertFox]];
            jungleRatNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.JungleRat]];
            urbanWarfareSpecialistNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.UrbanWarfareSpecialist]];
            rangerNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.Ranger]];
            mountaineerNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.Mountaineer]];
            hillsFighterNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.HillsFighter]];
            counterAttackerNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.CounterAttacker]];
            assaulterNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.Assaulter]];
            encirclerNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.Encircler]];
            ambusherNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.Ambusher]];
            disciplinedNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.Disciplined]];
            elasticDefenceSpecialistNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.ElasticDefenceSpecialist]];
            blitzerNarrowCheckBox.Text =
                Config.Text[Leader.TraitsTextTable[(int) LeaderTraitsId.Blitzer]];
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

            // 兵科
            foreach (string branchText in Leader.BranchTextTable)
            {
                branchComboBox.Items.Add(branchText);
            }

            // 階級
            foreach (string rankText in Leader.RankTextTable)
            {
                idealRankComboBox.Items.Add(rankText);
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
        private void OnLeaderEditorFormLoad(object sender, EventArgs e)
        {
            InitTraitsText();
            InitEditableItems();
            InitCountryList();
            LoadLeaderFiles();
        }

        /// <summary>
        /// 指揮官リストビューの項目を追加する
        /// </summary>
        /// <param name="leader">追加する項目</param>
        private void AddLeaderListViewItem(Leader leader)
        {
            leaderListView.Items.Add(CreateLeaderListViewItem(leader));
        }

        /// <summary>
        /// 指揮官リストビューの項目を挿入する
        /// </summary>
        /// <param name="index">挿入する位置</param>
        /// <param name="leader">挿入する項目</param>
        private void InsertLeaderListViewItem(int index, Leader leader)
        {
            leaderListView.Items.Insert(index, CreateLeaderListViewItem(leader));
        }

        /// <summary>
        /// 指揮官リストビューの項目を削除する
        /// </summary>
        /// <param name="index">削除する位置</param>
        private void RemoveLeaderListViewItem(int index)
        {
            leaderListView.Items.RemoveAt(index);
        }

        /// <summary>
        /// 指揮官リストビューの項目を作成する
        /// </summary>
        /// <param name="leader">指揮官データ</param>
        /// <returns>指揮官リストビューの項目</returns>
        private ListViewItem CreateLeaderListViewItem(Leader leader)
        {
            if (leader == null)
            {
                return null;
            }

            var item = new ListViewItem
                           {
                               Text =
                                   leader.CountryTag != CountryTag.None
                                       ? Country.CountryTextTable[(int) leader.CountryTag]
                                       : "",
                               Tag = leader
                           };
            item.SubItems.Add(leader.Id.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(leader.Name);
            item.SubItems.Add(Leader.BranchTextTable[(int) leader.Branch]);
            item.SubItems.Add(leader.Skill.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(leader.MaxSkill.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(leader.StartYear.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(leader.EndYear.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(GetLeaderTraitsText(leader.Traits));

            return item;
        }

        /// <summary>
        /// 指揮官特性文字列を取得する
        /// </summary>
        /// <param name="traits">指揮官特性</param>
        /// <returns>指揮官特性文字列</returns>
        private string GetLeaderTraitsText(uint traits)
        {
            string s = "";
            foreach (LeaderTraitsId id in Enum.GetValues(typeof (LeaderTraitsId)))
            {
                if ((traits & Leader.TraitsValueTable[(int) id]) != 0)
                {
                    s += ", ";
                    s += Config.Text[Leader.TraitsTextTable[(int) id]];
                }
            }
            // 先頭項目の", "を削除する
            if (!string.IsNullOrEmpty(s))
            {
                s = s.Substring(2);
            }

            return s;
        }

        /// <summary>
        /// 編集可能な項目を有効化する
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
            countryComboBox.SelectedIndex = 0;
            idNumericUpDown.Value = 0;
            nameTextBox.Text = "";
            branchComboBox.SelectedIndex = 0;
            idealRankComboBox.SelectedIndex = 0;
            skillNumericUpDown.Value = 0;
            maxSkillNumericUpDown.Value = 0;
            experienceNumericUpDown.Value = 0;
            loyaltyNumericUpDown.Value = 0;
            startYearNumericUpDown.Value = 1930;
            endYearNumericUpDown.Value = 1990;
            rankYearNumericUpDown1.Value = 1990;
            rankYearNumericUpDown2.Value = 1990;
            rankYearNumericUpDown3.Value = 1990;
            rankYearNumericUpDown4.Value = 1990;
            pictureNameTextBox.Text = "";
            leaderPictureBox.ImageLocation = "";

            logisticsWizardCheckBox.Checked = false;
            defensiveDoctrineCheckBox.Checked = false;
            offensiveDoctrineCheckBox.Checked = false;
            winterSpecialistCheckBox.Checked = false;
            tricksterCheckBox.Checked = false;
            engineerCheckBox.Checked = false;
            fortressBusterCheckBox.Checked = false;
            panzerLeaderCheckBox.Checked = false;
            commandoCheckBox.Checked = false;
            oldGuardCheckBox.Checked = false;
            seaWolfCheckBox.Checked = false;
            blockadeRunnerCheckBox.Checked = false;
            superiorTacticianCheckBox.Checked = false;
            spotterCheckBox.Checked = false;
            tankBusterCheckBox.Checked = false;
            carpetBomberCheckBox.Checked = false;
            nightFlyerCheckBox.Checked = false;
            fleetDestroyerCheckBox.Checked = false;
            desertFoxCheckBox.Checked = false;
            jungleRatCheckBox.Checked = false;
            urbanWarfareSpecialistCheckBox.Checked = false;
            rangerCheckBox.Checked = false;
            mountaineerCheckBox.Checked = false;
            hillsFighterCheckBox.Checked = false;
            counterAttackerCheckBox.Checked = false;
            assaulterCheckBox.Checked = false;
            encirclerCheckBox.Checked = false;
            ambusherCheckBox.Checked = false;
            disciplinedCheckBox.Checked = false;
            elasticDefenceSpecialistCheckBox.Checked = false;
            blitzerCheckBox.Checked = false;

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
            rankYearNumericUpDown1.Enabled = false;
            rankYearNumericUpDown2.Enabled = false;
            rankYearNumericUpDown3.Enabled = false;
            rankYearNumericUpDown4.Enabled = false;
            pictureNameTextBox.Enabled = false;
            pictureNameReferButton.Enabled = false;
            traitsGroupBox.Enabled = false;

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
            Leader leader;
            if (leaderListView.SelectedItems.Count > 0)
            {
                var selectedLeader = leaderListView.SelectedItems[0].Tag as Leader;
                if (selectedLeader == null)
                {
                    return;
                }
                leader = new Leader
                             {
                                 CountryTag = selectedLeader.CountryTag,
                                 Id = selectedLeader.Id + 1,
                                 Branch = LeaderBranch.None,
                                 IdealRank = LeaderRank.None,
                                 StartYear = 1930,
                                 EndYear = 1990,
                             };
                leader.RankYear[0] = 1930;
                leader.RankYear[1] = 1990;
                leader.RankYear[2] = 1990;
                leader.RankYear[3] = 1990;
                int masterIndex = _masterLeaderList.IndexOf(selectedLeader);
                _masterLeaderList.Insert(masterIndex + 1, leader);
                int narrowedIndex = leaderListView.SelectedIndices[0] + 1;
                _narrowedLeaderList.Insert(narrowedIndex, leader);
                InsertLeaderListViewItem(narrowedIndex, leader);
                leaderListView.Items[narrowedIndex].Focused = true;
                leaderListView.Items[narrowedIndex].Selected = true;
                leaderListView.Items[narrowedIndex].EnsureVisible();
            }
            else
            {
                leader = new Leader
                             {
                                 CountryTag =
                                     countryListBox.SelectedItems.Count > 0
                                         ? (CountryTag) (countryListBox.SelectedIndex + 1)
                                         : CountryTag.None,
                                 Id = 0,
                                 Branch = LeaderBranch.None,
                                 IdealRank = LeaderRank.None,
                                 StartYear = 1930,
                                 EndYear = 1990,
                             };
                leader.RankYear[0] = 1930;
                leader.RankYear[1] = 1990;
                leader.RankYear[2] = 1990;
                leader.RankYear[3] = 1990;
                _masterLeaderList.Add(leader);
                _narrowedLeaderList.Add(leader);
                AddLeaderListViewItem(leader);
                leaderListView.Items[0].Focused = true;
                leaderListView.Items[0].Selected = true;
                EnableEditableItems();
            }
            SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        /// 複製ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloneButtonClick(object sender, EventArgs e)
        {
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }
            var selectedLeader = leaderListView.SelectedItems[0].Tag as Leader;
            if (selectedLeader == null)
            {
                return;
            }
            var leader = new Leader
                             {
                                 CountryTag = selectedLeader.CountryTag,
                                 Id = selectedLeader.Id + 1,
                                 Name = selectedLeader.Name,
                                 Branch = selectedLeader.Branch,
                                 IdealRank = selectedLeader.IdealRank,
                                 Skill = selectedLeader.Skill,
                                 MaxSkill = selectedLeader.MaxSkill,
                                 Experience = selectedLeader.Experience,
                                 Loyalty = selectedLeader.Loyalty,
                                 StartYear = selectedLeader.StartYear,
                                 EndYear = selectedLeader.EndYear,
                                 PictureName = selectedLeader.PictureName
                             };
            leader.RankYear[0] = selectedLeader.RankYear[0];
            leader.RankYear[1] = selectedLeader.RankYear[1];
            leader.RankYear[2] = selectedLeader.RankYear[2];
            leader.RankYear[3] = selectedLeader.RankYear[3];
            int masterIndex = _masterLeaderList.IndexOf(selectedLeader);
            _masterLeaderList.Insert(masterIndex + 1, leader);
            int narrowedIndex = leaderListView.SelectedIndices[0] + 1;
            _narrowedLeaderList.Insert(narrowedIndex, leader);
            InsertLeaderListViewItem(narrowedIndex, leader);
            leaderListView.Items[narrowedIndex].Focused = true;
            leaderListView.Items[narrowedIndex].Selected = true;
            leaderListView.Items[narrowedIndex].EnsureVisible();
            SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        /// 削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDeleteButtonClick(object sender, EventArgs e)
        {
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }
            var selectedLeader = leaderListView.SelectedItems[0].Tag as Leader;
            if (selectedLeader == null)
            {
                return;
            }
            int masterIndex = _masterLeaderList.IndexOf(selectedLeader);
            _masterLeaderList.RemoveAt(masterIndex);
            int narrowedIndex = leaderListView.SelectedIndices[0];
            _narrowedLeaderList.RemoveAt(narrowedIndex);
            RemoveLeaderListViewItem(narrowedIndex);
            if (narrowedIndex < leaderListView.Items.Count)
            {
                leaderListView.Items[narrowedIndex].Focused = true;
                leaderListView.Items[narrowedIndex].Selected = true;
            }
            else if (narrowedIndex - 1 >= 0)
            {
                leaderListView.Items[narrowedIndex - 1].Focused = true;
                leaderListView.Items[narrowedIndex - 1].Selected = true;
            }
            else
            {
                DisableEditableItems();
            }
            SetDirtyFlag(selectedLeader.CountryTag);
        }

        /// <summary>
        /// 先頭へボタン押下時の処理
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
            int selectedIndex = leaderListView.SelectedIndices[0];
            // 選択項目がリストの先頭ならば何もしない
            if (leaderListView.SelectedIndices[0] == 0)
            {
                return;
            }
            var selectedLeader = leaderListView.SelectedItems[0].Tag as Leader;
            if (selectedLeader == null)
            {
                return;
            }
            var topLeader = leaderListView.Items[0].Tag as Leader;
            if (topLeader == null)
            {
                return;
            }
            int masterSelectedIndex = _masterLeaderList.IndexOf(selectedLeader);
            int masterTopIndex = _masterLeaderList.IndexOf(topLeader);
            _masterLeaderList.Insert(masterTopIndex, selectedLeader);
            _masterLeaderList.RemoveAt(masterSelectedIndex + 1);
            _narrowedLeaderList.Insert(0, selectedLeader);
            _narrowedLeaderList.RemoveAt(selectedIndex + 1);
            InsertLeaderListViewItem(0, selectedLeader);
            RemoveLeaderListViewItem(selectedIndex + 1);
            leaderListView.Items[0].Focused = true;
            leaderListView.Items[0].Selected = true;
            SetDirtyFlag(selectedLeader.CountryTag);
        }

        /// <summary>
        /// 上へボタン押下時の処理
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
            if (leaderListView.SelectedIndices[0] == 0)
            {
                return;
            }
            int selectedIndex = leaderListView.SelectedIndices[0];
            var selectedLeader = leaderListView.SelectedItems[0].Tag as Leader;
            if (selectedLeader == null)
            {
                return;
            }
            var upperLeader = leaderListView.Items[selectedIndex - 1].Tag as Leader;
            if (upperLeader == null)
            {
                return;
            }
            int masterSelectedIndex = _masterLeaderList.IndexOf(selectedLeader);
            int masterUpperIndex = _masterLeaderList.IndexOf(upperLeader);
            _masterLeaderList.Insert(masterUpperIndex, selectedLeader);
            _masterLeaderList.RemoveAt(masterSelectedIndex + 1);
            _narrowedLeaderList.Insert(selectedIndex - 1, selectedLeader);
            _narrowedLeaderList.RemoveAt(selectedIndex + 1);
            InsertLeaderListViewItem(selectedIndex - 1, selectedLeader);
            RemoveLeaderListViewItem(selectedIndex + 1);
            leaderListView.Items[selectedIndex - 1].Focused = true;
            leaderListView.Items[selectedIndex - 1].Selected = true;
            SetDirtyFlag(selectedLeader.CountryTag);
        }

        /// <summary>
        /// 下へボタン押下時の処理
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
            if (leaderListView.SelectedIndices[0] == leaderListView.Items.Count - 1)
            {
                return;
            }
            int selectedIndex = leaderListView.SelectedIndices[0];
            var selectedLeader = leaderListView.SelectedItems[0].Tag as Leader;
            if (selectedLeader == null)
            {
                return;
            }
            var lowerLeader = leaderListView.Items[selectedIndex + 1].Tag as Leader;
            if (lowerLeader == null)
            {
                return;
            }
            int masterSelectedIndex = _masterLeaderList.IndexOf(selectedLeader);
            int masterLowerIndex = _masterLeaderList.IndexOf(lowerLeader);
            _masterLeaderList.Insert(masterSelectedIndex, lowerLeader);
            _masterLeaderList.RemoveAt(masterLowerIndex + 1);
            _narrowedLeaderList.Insert(selectedIndex, lowerLeader);
            _narrowedLeaderList.RemoveAt(selectedIndex + 2);
            InsertLeaderListViewItem(selectedIndex, lowerLeader);
            RemoveLeaderListViewItem(selectedIndex + 2);
            leaderListView.Items[selectedIndex + 1].Focused = true;
            leaderListView.Items[selectedIndex + 1].Selected = true;
            SetDirtyFlag(selectedLeader.CountryTag);
        }

        /// <summary>
        /// 末尾へボタン押下時の処理
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
            int selectedIndex = leaderListView.SelectedIndices[0];
            int bottomIndex = leaderListView.Items.Count - 1;
            // 選択項目がリストの末尾ならば何もしない
            if (leaderListView.SelectedIndices[0] == bottomIndex)
            {
                return;
            }
            var selectedLeader = leaderListView.Items[selectedIndex].Tag as Leader;
            if (selectedLeader == null)
            {
                return;
            }
            var bottomLeader = leaderListView.Items[bottomIndex].Tag as Leader;
            if (bottomLeader == null)
            {
                return;
            }
            int masterSelectedIndex = _masterLeaderList.IndexOf(selectedLeader);
            int masterBottomIndex = _masterLeaderList.IndexOf(bottomLeader);
            _masterLeaderList.Insert(masterBottomIndex + 1, selectedLeader);
            _masterLeaderList.RemoveAt(masterSelectedIndex);
            _narrowedLeaderList.Insert(bottomIndex + 1, selectedLeader);
            _narrowedLeaderList.RemoveAt(selectedIndex);
            InsertLeaderListViewItem(bottomIndex + 1, selectedLeader);
            RemoveLeaderListViewItem(selectedIndex);
            leaderListView.Items[bottomIndex].Focused = true;
            leaderListView.Items[bottomIndex].Selected = true;
            SetDirtyFlag(selectedLeader.CountryTag);
        }

        /// <summary>
        /// 兵科チェックボックスのチェック状態変化時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBranchNarrowCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            NarrowLeaderList();
            UpdateLeaderList();
        }

        /// <summary>
        /// 特性絞り込みチェックボックスのチェック状態変化時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTraitsNarrowCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            NarrowLeaderList();
            UpdateLeaderList();
        }

        /// <summary>
        /// 特性絞込み条件ラジオボタンのチェック状態変化時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTraitsNarrowRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            NarrowLeaderList();
            UpdateLeaderList();
        }

        /// <summary>
        /// 選択反転ボタン押下時の処理
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

            NarrowLeaderList();
            UpdateLeaderList();
        }

        /// <summary>
        /// 指揮官リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLeaderListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }
            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }

            countryComboBox.Text = leader.CountryTag != CountryTag.None
                                       ? Country.CountryTextTable[(int) leader.CountryTag]
                                       : "";
            idNumericUpDown.Value = leader.Id;
            nameTextBox.Text = leader.Name;
            branchComboBox.SelectedIndex = (int) leader.Branch;
            idealRankComboBox.SelectedIndex = (int) leader.IdealRank;
            skillNumericUpDown.Value = leader.Skill;
            maxSkillNumericUpDown.Value = leader.MaxSkill;
            experienceNumericUpDown.Value = leader.Experience;
            loyaltyNumericUpDown.Value = leader.Loyalty;
            startYearNumericUpDown.Value = leader.StartYear;
            endYearNumericUpDown.Value = leader.EndYear;
            rankYearNumericUpDown1.Value = leader.RankYear[0];
            rankYearNumericUpDown2.Value = leader.RankYear[1];
            rankYearNumericUpDown3.Value = leader.RankYear[2];
            rankYearNumericUpDown4.Value = leader.RankYear[3];
            pictureNameTextBox.Text = leader.PictureName;
            leaderPictureBox.ImageLocation = Game.GetPictureFileName(leader.PictureName);

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
        }

        /// <summary>
        /// 国タグ変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryComboBoxSelectionChangeCommitted(object sender, EventArgs e)
        {
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }
            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }
            // 値に変化がなければ何もせずに戻る
            var newCountryTag = (CountryTag) countryComboBox.SelectedIndex;
            if (newCountryTag == leader.CountryTag)
            {
                return;
            }
            SetDirtyFlag(leader.CountryTag);
            leader.CountryTag = newCountryTag;
            leaderListView.SelectedItems[0].Text = Country.CountryTextTable[(int) leader.CountryTag];
            SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        /// ID変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIdNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }
            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }
            // 値に変化がなければ何もせずに戻る
            var newId = (int) idNumericUpDown.Value;
            if (newId == leader.Id)
            {
                return;
            }
            leader.Id = newId;
            leaderListView.SelectedItems[0].SubItems[1].Text = leader.Id.ToString(CultureInfo.InvariantCulture);
            SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        /// 名前文字列変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNameTextBoxTextChanged(object sender, EventArgs e)
        {
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }
            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }
            // 値に変化がなければ何もせずに戻る
            string newName = nameTextBox.Text;
            if (newName.Equals(leader.Name))
            {
                return;
            }
            leader.Name = newName;
            leaderListView.SelectedItems[0].SubItems[2].Text = leader.Name;
            SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        /// 兵科変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBranchComboBoxSelectionChangeCommitted(object sender, EventArgs e)
        {
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }
            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }
            // 値に変化がなければ何もせずに戻る
            var newBranch = (LeaderBranch) branchComboBox.SelectedIndex;
            if (newBranch == leader.Branch)
            {
                return;
            }
            leader.Branch = newBranch;
            leaderListView.SelectedItems[0].SubItems[3].Text = Leader.BranchTextTable[(int) leader.Branch];
            SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        /// 理想階級変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIdealRankComboBoxSelectionChangeCommitted(object sender, EventArgs e)
        {
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }
            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }
            // 値に変化がなければ何もせずに戻る
            var newIdealRank = (LeaderRank) idealRankComboBox.SelectedIndex;
            if (newIdealRank == leader.IdealRank)
            {
                return;
            }
            leader.IdealRank = newIdealRank;
            SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        /// スキル変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSkillNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }
            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }
            // 値に変化がなければ何もせずに戻る
            var newSkill = (int) skillNumericUpDown.Value;
            if (newSkill == leader.Skill)
            {
                return;
            }
            leader.Skill = newSkill;
            leaderListView.SelectedItems[0].SubItems[4].Text = leader.Skill.ToString(CultureInfo.InvariantCulture);
            SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        /// 最大スキル変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMaxSkillNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }
            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }
            // 値に変化がなければ何もせずに戻る
            var newMaxSkill = (int) maxSkillNumericUpDown.Value;
            if (newMaxSkill == leader.MaxSkill)
            {
                return;
            }
            leader.MaxSkill = newMaxSkill;
            leaderListView.SelectedItems[0].SubItems[5].Text = leader.MaxSkill.ToString(CultureInfo.InvariantCulture);
            SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        /// 経験値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExperienceNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }
            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }
            // 値に変化がなければ何もせずに戻る
            var newExperience = (int) experienceNumericUpDown.Value;
            if (newExperience == leader.Experience)
            {
                return;
            }
            leader.Experience = newExperience;
            SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        /// 忠誠度変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoyaltyNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }
            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }
            // 値に変化がなければ何もせずに戻る
            var newLoyalty = (int) loyaltyNumericUpDown.Value;
            if (newLoyalty == leader.Loyalty)
            {
                return;
            }
            leader.Loyalty = newLoyalty;
            SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        /// 開始年変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStartYearNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }
            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }
            // 値に変化がなければ何もせずに戻る
            var newStartYear = (int) startYearNumericUpDown.Value;
            if (newStartYear == leader.StartYear)
            {
                return;
            }
            leader.StartYear = newStartYear;
            leaderListView.SelectedItems[0].SubItems[6].Text = leader.StartYear.ToString(CultureInfo.InvariantCulture);
            SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        /// 終了年変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEndYearNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }
            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }
            // 値に変化がなければ何もせずに戻る
            var newEndYear = (int) endYearNumericUpDown.Value;
            if (newEndYear == leader.EndYear)
            {
                return;
            }
            leader.EndYear = newEndYear;
            leaderListView.SelectedItems[0].SubItems[7].Text = leader.EndYear.ToString(CultureInfo.InvariantCulture);
            SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        /// 少将任官年変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRankYearNumericUpDown1ValueChanged(object sender, EventArgs e)
        {
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }
            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }
            // 値に変化がなければ何もせずに戻る
            var newRankYear = (int) rankYearNumericUpDown1.Value;
            if (newRankYear == leader.RankYear[0])
            {
                return;
            }
            leader.RankYear[0] = newRankYear;
            SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        /// 中将任官年変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRankYearNumericUpDown2ValueChanged(object sender, EventArgs e)
        {
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }
            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }
            // 値に変化がなければ何もせずに戻る
            var newRankYear = (int) rankYearNumericUpDown2.Value;
            if (newRankYear == leader.RankYear[1])
            {
                return;
            }
            leader.RankYear[1] = newRankYear;
            SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        /// 大将任官年変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRankYearNumericUpDown3ValueChanged(object sender, EventArgs e)
        {
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }
            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }
            // 値に変化がなければ何もせずに戻る
            var newRankYear = (int) rankYearNumericUpDown3.Value;
            if (newRankYear == leader.RankYear[2])
            {
                return;
            }
            leader.RankYear[2] = newRankYear;
            SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        /// 元帥任官年変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRankYearNumericUpDown4ValueChanged(object sender, EventArgs e)
        {
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }
            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }
            // 値に変化がなければ何もせずに戻る
            var newRankYear = (int) rankYearNumericUpDown1.Value;
            if (newRankYear == leader.RankYear[3])
            {
                return;
            }
            leader.RankYear[3] = newRankYear;
            SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        /// 特性変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTraitsCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }
            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }
            // 値に変化がなければ何もせずに戻る
            uint newTraits = GetCheckedLeaderTraits();
            if (newTraits == leader.Traits)
            {
                return;
            }
            leader.Traits = newTraits;
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);
            SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        /// 選択された指揮官特性を取得する
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

        /// <summary>
        /// 絞り込まれた指揮官特性を取得する
        /// </summary>
        /// <returns>指揮官特性</returns>
        private uint GetNarrowedLeaderTraits()
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

        /// <summary>
        /// 画像ファイル名変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPictureNameTextBoxTextChanged(object sender, EventArgs e)
        {
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }
            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }
            // 値に変化がなければ何もせずに戻る
            string newPictureName = pictureNameTextBox.Text;
            if (newPictureName.Equals(leader.PictureName))
            {
                return;
            }
            leader.PictureName = newPictureName;
            leaderPictureBox.ImageLocation = Game.GetPictureFileName(leader.PictureName);
            SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        /// 画像ファイル名参照ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPictureNameReferButtonClick(object sender, EventArgs e)
        {
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }
            var leader = leaderListView.SelectedItems[0].Tag as Leader;
            if (leader == null)
            {
                return;
            }
            var dialog = new OpenFileDialog
                             {
                                 InitialDirectory = Game.PictureFolderName,
                                 FileName = leader.PictureName,
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
            // 指揮官リスト絞り込みのため、ダミーでイベント発行する
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
            LoadLeaderFiles();
        }

        /// <summary>
        /// 保存ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            Leader.SaveLeaderFiles(_masterLeaderList, _dirtyFlags);
            ClearDirtyFlags();
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