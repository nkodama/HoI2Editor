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
        /// <summary>
        ///     絞り込み後の指揮官リスト
        /// </summary>
        private readonly List<Leader> _narrowedList = new List<Leader>();

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public LeaderEditorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     指揮官ファイルを読み込む
        /// </summary>
        private void LoadLeaderFiles()
        {
            // 指揮官ファイルを読み込む
            Leaders.LoadLeaderFiles();

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
        private void SaveLeaderFiles()
        {
            // 指揮官ファイルを保存する
            Leaders.SaveLeaderFiles();

            // 編集済みフラグがクリアされるため表示を更新する
            countryListBox.Update();
        }

        /// <summary>
        ///     指揮官リストを絞り込む
        /// </summary>
        private void NarrowLeaderList()
        {
            _narrowedList.Clear();
            uint traitsMask = GetNarrowedTraits();

            // 選択中の国家リストを作成する
            List<CountryTag> selectedTagList = countryListBox.SelectedItems.Count == 0
                                                   ? new List<CountryTag>()
                                                   : (from string country in countryListBox.SelectedItems
                                                      select Country.CountryStringMap[country]).ToList();

            foreach (
                Leader leader in Leaders.List.Where(leader => selectedTagList.Contains(leader.CountryTag)))
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
                        if (!armyNarrowCheckBox.Checked || !navyNarrowCheckBox.Checked ||
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
        ///     指揮官特性文字列を更新する
        /// </summary>
        private void InitTraitsText()
        {
            logisticsWizardCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.LogisticsWizard]);
            defensiveDoctrineCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.DefensiveDoctrine]);
            offensiveDoctrineCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.OffensiveDoctrine]);
            winterSpecialistCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.WinterSpecialist]);
            tricksterCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.Trickster]);
            engineerCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.Engineer]);
            fortressBusterCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.FortressBuster]);
            panzerLeaderCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.PanzerLeader]);
            commandoCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.Commando]);
            oldGuardCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.OldGuard]);
            seaWolfCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.SeaWolf]);
            blockadeRunnerCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.BlockadeRunner]);
            superiorTacticianCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.SuperiorTactician]);
            spotterCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.Spotter]);
            tankBusterCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.TankBuster]);
            carpetBomberCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.CarpetBomber]);
            nightFlyerCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.NightFlyer]);
            fleetDestroyerCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.FleetDestroyer]);
            desertFoxCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.DesertFox]);
            jungleRatCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.JungleRat]);
            urbanWarfareSpecialistCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.UrbanWarfareSpecialist]);
            rangerCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.Ranger]);
            mountaineerCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.Mountaineer]);
            hillsFighterCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.HillsFighter]);
            counterAttackerCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.CounterAttacker]);
            assaulterCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.Assaulter]);
            encirclerCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.Encircler]);
            ambusherCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.Ambusher]);
            disciplinedCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.Disciplined]);
            elasticDefenceSpecialistCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.ElasticDefenceSpecialist]);
            blitzerCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.Blitzer]);

            logisticsWizardNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.LogisticsWizard]);
            defensiveDoctrineNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.DefensiveDoctrine]);
            offensiveDoctrineNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.OffensiveDoctrine]);
            winterSpecialistNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.WinterSpecialist]);
            tricksterNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.Trickster]);
            engineerNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.Engineer]);
            fortressBusterNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.FortressBuster]);
            panzerLeaderNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.PanzerLeader]);
            commandoNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.Commando]);
            oldGuardNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.OldGuard]);
            seaWolfNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.SeaWolf]);
            blockadeRunnerNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.BlockadeRunner]);
            superiorTacticianNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.SuperiorTactician]);
            spotterNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.Spotter]);
            tankBusterNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.TankBuster]);
            carpetBomberNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.CarpetBomber]);
            nightFlyerNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.NightFlyer]);
            fleetDestroyerNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.FleetDestroyer]);
            desertFoxNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.DesertFox]);
            jungleRatNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.JungleRat]);
            urbanWarfareSpecialistNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.UrbanWarfareSpecialist]);
            rangerNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.Ranger]);
            mountaineerNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.Mountaineer]);
            hillsFighterNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.HillsFighter]);
            counterAttackerNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.CounterAttacker]);
            assaulterNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.Assaulter]);
            encirclerNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.Encircler]);
            ambusherNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.Ambusher]);
            disciplinedNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.Disciplined]);
            elasticDefenceSpecialistNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.ElasticDefenceSpecialist]);
            blitzerNarrowCheckBox.Text =
                Config.GetText(Leader.TraitsNameTable[(int) LeaderTraitsId.Blitzer]);
        }

        /// <summary>
        ///     国家コンボボックスの項目を更新する
        /// </summary>
        /// <param name="leader">指揮官データ</param>
        private void UpdateCountryComboBox(Leader leader)
        {
            countryComboBox.BeginUpdate();

            if (leader.CountryTag != CountryTag.None)
            {
                if (string.IsNullOrEmpty(countryComboBox.Items[0].ToString()))
                {
                    countryComboBox.Items.RemoveAt(0);
                }
                countryComboBox.SelectedIndex = (int) (leader.CountryTag - 1);
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
        ///     編集項目を初期化する
        /// </summary>
        private void InitEditableItems()
        {
            // 国タグ
            int maxSize = countryComboBox.DropDownWidth;
            foreach (string s in Country.CountryTextTable.Select(
                country =>
                Config.ExistsKey(country) ? string.Format("{0} {1}", country, Config.GetText(country)) : country))
            {
                countryComboBox.Items.Add(s);
                maxSize = Math.Max(maxSize,
                                   TextRenderer.MeasureText(s, countryComboBox.Font).Width +
                                   SystemInformation.VerticalScrollBarWidth);
            }
            countryComboBox.DropDownWidth = maxSize;

            // 兵科
            maxSize = branchComboBox.DropDownWidth;
            foreach (string name in Leaders.BranchNameTable)
            {
                branchComboBox.Items.Add(name);
                maxSize = Math.Max(maxSize, TextRenderer.MeasureText(name, branchComboBox.Font).Width);
            }
            branchComboBox.DropDownWidth = maxSize;

            // 階級
            maxSize = idealRankComboBox.DropDownWidth;
            foreach (string name in Leaders.RankNameTable)
            {
                idealRankComboBox.Items.Add(name);
                maxSize = Math.Max(maxSize, TextRenderer.MeasureText(name, idealRankComboBox.Font).Width);
            }
            idealRankComboBox.DropDownWidth = maxSize;
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
                if (string.IsNullOrEmpty(branchComboBox.Items[0].ToString()))
                {
                    branchComboBox.Items.RemoveAt(0);
                }
                branchComboBox.SelectedIndex = (int) (leader.Branch - 1);
            }
            else
            {
                if (!string.IsNullOrEmpty(branchComboBox.Items[0].ToString()))
                {
                    branchComboBox.Items.Insert(0, "");
                }
                branchComboBox.SelectedIndex = 0;
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
                if (string.IsNullOrEmpty(idealRankComboBox.Items[0].ToString()))
                {
                    idealRankComboBox.Items.RemoveAt(0);
                }
                idealRankComboBox.SelectedIndex = (int) (leader.IdealRank - 1);
            }
            else
            {
                if (!string.IsNullOrEmpty(idealRankComboBox.Items[0].ToString()))
                {
                    idealRankComboBox.Items.Insert(0, "");
                }
                idealRankComboBox.SelectedIndex = 0;
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
                    Game.GetFileName(Path.Combine(Game.PersonPicturePathName,
                                                  Path.ChangeExtension(leader.PictureName, ".bmp")));
                leaderPictureBox.ImageLocation = File.Exists(fileName) ? fileName : "";
            }
            else
            {
                leaderPictureBox.ImageLocation = "";
            }
        }

        /// <summary>
        ///     国家リストボックスを初期化する
        /// </summary>
        private void InitCountryListBox()
        {
            foreach (string country in Country.CountryTextTable.Where(country => !string.IsNullOrEmpty(country)))
            {
                countryListBox.Items.Add(country);
            }
            countryListBox.SelectedIndex = 0;
        }

        /// <summary>
        ///     指揮官リストに項目を追加する
        /// </summary>
        /// <param name="target">挿入対象の項目</param>
        private void AddListItem(Leader target)
        {
            _narrowedList.Add(target);

            leaderListView.Items.Add(CreateLeaderListViewItem(target));

            leaderListView.Items[0].Focused = true;
            leaderListView.Items[0].Selected = true;
        }

        /// <summary>
        ///     指揮官リストに項目を挿入する
        /// </summary>
        /// <param name="target">挿入対象の項目</param>
        /// <param name="index">挿入先の位置</param>
        private void InsertListItem(Leader target, int index)
        {
            _narrowedList.Insert(index, target);

            leaderListView.Items.Insert(index, CreateLeaderListViewItem(target));

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
            _narrowedList.RemoveAt(index);

            leaderListView.Items.RemoveAt(index);

            if (index < leaderListView.Items.Count)
            {
                leaderListView.Items[index].Focused = true;
                leaderListView.Items[index].Selected = true;
            }
            else if (index - 1 >= 0)
            {
                leaderListView.Items[index - 1].Focused = true;
                leaderListView.Items[index - 1].Selected = true;
            }
        }

        /// <summary>
        ///     指揮官リストの項目を移動する
        /// </summary>
        /// <param name="target">移動対象の位置</param>
        /// <param name="position">移動先の位置</param>
        private void MoveListItem(int target, int position)
        {
            Leader leader = _narrowedList[target];

            if (target > position)
            {
                // 上へ移動する場合
                _narrowedList.Insert(position, leader);
                _narrowedList.RemoveAt(target + 1);

                leaderListView.Items.Insert(position, CreateLeaderListViewItem(leader));
                leaderListView.Items.RemoveAt(target + 1);
            }
            else
            {
                // 下へ移動する場合
                _narrowedList.Insert(position + 1, leader);
                _narrowedList.RemoveAt(target);

                leaderListView.Items.Insert(position + 1, CreateLeaderListViewItem(leader));
                leaderListView.Items.RemoveAt(target);
            }

            leaderListView.Items[position].Focused = true;
            leaderListView.Items[position].Selected = true;
            leaderListView.Items[position].EnsureVisible();
        }

        /// <summary>
        ///     指揮官リストビューの項目を作成する
        /// </summary>
        /// <param name="leader">指揮官データ</param>
        /// <returns>指揮官リストビューの項目</returns>
        private static ListViewItem CreateLeaderListViewItem(Leader leader)
        {
            if (leader == null)
            {
                return null;
            }

            var item = new ListViewItem
                           {
                               Text = Country.CountryTextTable[(int) leader.CountryTag],
                               Tag = leader
                           };
            item.SubItems.Add(leader.Id.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(leader.Name);
            item.SubItems.Add(Leaders.BranchNameTable[(int) leader.Branch]);
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
            string s =
                Enum.GetValues(typeof (LeaderTraitsId))
                    .Cast<LeaderTraitsId>()
                    .Where(id => (traits & Leader.TraitsValueTable[(int) id]) != 0)
                    .Aggregate("", (current, id) => current + (", " + Config.GetText(Leader.TraitsNameTable[(int) id])));
            // 先頭項目の", "を削除する
            if (!string.IsNullOrEmpty(s))
            {
                s = s.Substring(2);
            }

            return s;
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
            retirementYearNumericUpDown.Enabled = Misc.Mod.RetirementYearLeader;
            rankYearNumericUpDown1.Enabled = true;
            rankYearNumericUpDown2.Enabled = true;
            rankYearNumericUpDown3.Enabled = true;
            rankYearNumericUpDown4.Enabled = true;
            pictureNameTextBox.Enabled = true;
            pictureNameReferButton.Enabled = true;
            traitsGroupBox.Enabled = true;

            cloneButton.Enabled = true;
            removeButton.Enabled = true;
        }

        /// <summary>
        ///     編集可能な項目を無効化する
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
            retirementYearNumericUpDown.Value = 1999;
            rankYearNumericUpDown1.Value = 1990;
            rankYearNumericUpDown2.Value = 1990;
            rankYearNumericUpDown3.Value = 1990;
            rankYearNumericUpDown4.Value = 1990;
            pictureNameTextBox.Text = "";
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
        ///     指揮官特性チェックボックスの値を一括設定する
        /// </summary>
        /// <param name="flag">設定する値</param>
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

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLeaderEditorFormLoad(object sender, EventArgs e)
        {
            InitTraitsText();
            InitEditableItems();
            InitCountryListBox();
            LoadLeaderFiles();
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

                leader = new Leader
                             {
                                 CountryTag = selected.CountryTag,
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

                Leaders.InsertItemNext(leader, selected);

                InsertListItem(leader, leaderListView.SelectedIndices[0] + 1);
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
                                 RetirementYear = 1999,
                             };
                leader.RankYear[0] = 1930;
                leader.RankYear[1] = 1990;
                leader.RankYear[2] = 1990;
                leader.RankYear[3] = 1990;

                Leaders.AddItem(leader);

                AddListItem(leader);

                EnableEditableItems();
            }

            Leaders.SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        ///     複製ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloneButtonClick(object sender, EventArgs e)
        {
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }

            var selected = leaderListView.SelectedItems[0].Tag as Leader;
            if (selected == null)
            {
                return;
            }

            var leader = new Leader
                             {
                                 CountryTag = selected.CountryTag,
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

            Leaders.InsertItemNext(leader, selected);

            InsertListItem(leader, leaderListView.SelectedIndices[0] + 1);

            Leaders.SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        ///     削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRemoveButtonClick(object sender, EventArgs e)
        {
            if (leaderListView.SelectedItems.Count == 0)
            {
                return;
            }

            var selected = leaderListView.SelectedItems[0].Tag as Leader;
            if (selected == null)
            {
                return;
            }

            Leaders.RemoveItem(selected);

            RemoveItem(leaderListView.SelectedIndices[0]);

            if (leaderListView.Items.Count == 0)
            {
                DisableEditableItems();
            }

            Leaders.SetDirtyFlag(selected.CountryTag);
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

            Leaders.MoveItem(selected, top);

            MoveListItem(index, 0);

            Leaders.SetDirtyFlag(selected.CountryTag);
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

            Leaders.MoveItem(selected, upper);

            MoveListItem(index, index - 1);

            Leaders.SetDirtyFlag(selected.CountryTag);
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

            Leaders.MoveItem(selected, lower);

            MoveListItem(index, index + 1);

            Leaders.SetDirtyFlag(selected.CountryTag);
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

            Leaders.MoveItem(selected, bottom);

            MoveListItem(index, leaderListView.Items.Count - 1);

            Leaders.SetDirtyFlag(selected.CountryTag);
        }

        /// <summary>
        ///     兵科チェックボックスのチェック状態変化時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBranchNarrowCheckBoxCheckedChanged(object sender, EventArgs e)
        {
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
                if (!traitsNoneNarrowRadioButton.Checked)
                {
                    traitsNoneNarrowRadioButton.Checked = true;
                }
            }
            else
            {
                if (traitsNoneNarrowRadioButton.Checked)
                {
                    traitsOrNarrowRadioButton.Checked = true;
                }
            }

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
                    brush = Leaders.DirtyFlags[e.Index + 1]
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

            NarrowLeaderList();
            UpdateLeaderList();
        }

        /// <summary>
        ///     指揮官リストビューの選択項目変更時の処理
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
        ///     国タグ変更時の処理
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
            CountryTag newCountryTag = !string.IsNullOrEmpty(countryComboBox.Items[0].ToString())
                                           ? (CountryTag) (countryComboBox.SelectedIndex + 1)
                                           : (CountryTag) countryComboBox.SelectedIndex;
            if (newCountryTag == leader.CountryTag)
            {
                return;
            }

            Leaders.SetDirtyFlag(leader.CountryTag);

            leader.CountryTag = newCountryTag;
            leaderListView.SelectedItems[0].Text = Country.CountryTextTable[(int) leader.CountryTag];

            UpdateCountryComboBox(leader);

            Leaders.SetDirtyFlag(leader.CountryTag);

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

            Leaders.SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        ///     名前文字列変更時の処理
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

            Leaders.SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        ///     兵科変更時の処理
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
            LeaderBranch newBranch = !string.IsNullOrEmpty(branchComboBox.Items[0].ToString())
                                         ? (LeaderBranch) (branchComboBox.SelectedIndex + 1)
                                         : (LeaderBranch) branchComboBox.SelectedIndex;
            if (newBranch == leader.Branch)
            {
                return;
            }

            leader.Branch = newBranch;
            leaderListView.SelectedItems[0].SubItems[3].Text = Leaders.BranchNameTable[(int) leader.Branch];

            UpdateBranchComboBox(leader);

            Leaders.SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        ///     理想階級変更時の処理
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
            LeaderRank newIdealRank = !string.IsNullOrEmpty(idealRankComboBox.Items[0].ToString())
                                          ? (LeaderRank) (idealRankComboBox.SelectedIndex + 1)
                                          : (LeaderRank) idealRankComboBox.SelectedIndex;
            if (newIdealRank == leader.IdealRank)
            {
                return;
            }

            leader.IdealRank = newIdealRank;

            UpdateIdealRankComboBox(leader);

            Leaders.SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        ///     スキル変更時の処理
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

            Leaders.SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        ///     最大スキル変更時の処理
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

            Leaders.SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        ///     経験値変更時の処理
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

            Leaders.SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        ///     忠誠度変更時の処理
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

            Leaders.SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        ///     開始年変更時の処理
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

            Leaders.SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        ///     終了年変更時の処理
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

            Leaders.SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        ///     引退年変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRetirementYearNumericUpDownValueChanged(object sender, EventArgs e)
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
            var newRetirementYear = (int) retirementYearNumericUpDown.Value;
            if (newRetirementYear == leader.RetirementYear)
            {
                return;
            }

            leader.RetirementYear = newRetirementYear;

            Leaders.SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        ///     少将任官年変更時の処理
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

            Leaders.SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        ///     中将任官年変更時の処理
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

            Leaders.SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        ///     大将任官年変更時の処理
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

            Leaders.SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        ///     元帥任官年変更時の処理
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
            var newRankYear = (int) rankYearNumericUpDown4.Value;
            if (newRankYear == leader.RankYear[3])
            {
                return;
            }

            leader.RankYear[3] = newRankYear;

            Leaders.SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        ///     特性変更時の処理
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

            Leaders.SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        ///     画像ファイル名変更時の処理
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
            UpdateLeaderPicture(leader);

            Leaders.SetDirtyFlag(leader.CountryTag);
        }

        /// <summary>
        ///     画像ファイル名参照ボタン押下時の処理
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

        /// <summary>
        ///     再読み込みボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReloadButtonClick(object sender, EventArgs e)
        {
            LoadLeaderFiles();
        }

        /// <summary>
        ///     保存ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            SaveLeaderFiles();
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