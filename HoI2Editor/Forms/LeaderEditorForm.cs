using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HoI2Editor.Dialogs;
using HoI2Editor.Models;
using HoI2Editor.Properties;
using HoI2Editor.Utilities;

namespace HoI2Editor.Forms
{
    /// <summary>
    ///     指揮官エディタのフォーム
    /// </summary>
    public partial class LeaderEditorForm : Form
    {
        #region 内部フィールド

        /// <summary>
        ///     絞り込み後の指揮官リスト
        /// </summary>
        private readonly List<Leader> _list = new List<Leader>();

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
            Branch,
            Skill,
            MaxSkill,
            StartYear,
            EndYear,
            Traits
        }

        /// <summary>
        ///     ソート順
        /// </summary>
        private enum SortOrder
        {
            Ascendant,
            Decendant
        }

        #endregion

        #region 公開定数

        /// <summary>
        ///     指揮官リストビューの列の数
        /// </summary>
        public const int LeaderListColumnCount = 9;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public LeaderEditorForm()
        {
            InitializeComponent();

            // フォームの初期化
            InitForm();
        }

        #endregion

        #region データ処理

        /// <summary>
        ///     データ読み込み後の処理
        /// </summary>
        public void OnFileLoaded()
        {
            // 指揮官リストを絞り込む
            NarrowLeaderList();

            // 指揮官リストをソートする
            SortLeaderList();

            // 指揮官リストの表示を更新する
            UpdateLeaderList();

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

        #region フォーム

        /// <summary>
        ///     フォームの初期化
        /// </summary>
        private void InitForm()
        {
            // 指揮官リストビュー
            countryColumnHeader.Width = HoI2Editor.Settings.LeaderEditor.ListColumnWidth[0];
            idColumnHeader.Width = HoI2Editor.Settings.LeaderEditor.ListColumnWidth[1];
            nameColumnHeader.Width = HoI2Editor.Settings.LeaderEditor.ListColumnWidth[2];
            branchColumnHeader.Width = HoI2Editor.Settings.LeaderEditor.ListColumnWidth[3];
            skillColumnHeader.Width = HoI2Editor.Settings.LeaderEditor.ListColumnWidth[4];
            maxSkillColumnHeader.Width = HoI2Editor.Settings.LeaderEditor.ListColumnWidth[5];
            startYearColumnHeader.Width = HoI2Editor.Settings.LeaderEditor.ListColumnWidth[6];
            endYearColumnHeader.Width = HoI2Editor.Settings.LeaderEditor.ListColumnWidth[7];
            traitsColumnHeader.Width = HoI2Editor.Settings.LeaderEditor.ListColumnWidth[8];

            // 国家リストボックス
            countryListBox.ColumnWidth = DeviceCaps.GetScaledWidth(countryListBox.ColumnWidth);
            countryListBox.ItemHeight = DeviceCaps.GetScaledHeight(countryListBox.ItemHeight);

            // ウィンドウの位置
            Location = HoI2Editor.Settings.LeaderEditor.Location;
            Size = HoI2Editor.Settings.LeaderEditor.Size;
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormLoad(object sender, EventArgs e)
        {
            // 国家データを初期化する
            Countries.Init();

            // ゲーム設定ファイルを読み込む
            Misc.Load();

            // 文字列定義ファイルを読み込む
            Config.Load();

            // 特性文字列を初期化する
            InitTraitsText();

            // 編集項目を初期化する
            InitEditableItems();

            // 国家リストボックスを初期化する
            InitCountryListBox();

            // 指揮官ファイルを読み込む
            Leaders.Load();

            // データ読み込み後の処理
            OnFileLoaded();
        }

        /// <summary>
        ///     フォームクローズ時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
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
        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            HoI2Editor.OnLeaderEditorFormClosed();
        }

        /// <summary>
        ///     フォーム移動時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormMove(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                HoI2Editor.Settings.LeaderEditor.Location = Location;
            }
        }

        /// <summary>
        ///     フォームリサイズ時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormResize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                HoI2Editor.Settings.LeaderEditor.Size = Size;
            }
        }

        /// <summary>
        ///     一括編集ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBatchButtonClick(object sender, EventArgs e)
        {
            string countryName = countryListBox.SelectedItem as string;
            Country country = !string.IsNullOrEmpty(countryName) ? Countries.StringMap[countryName] : Country.None;
            LeaderBatchDialog dialog = new LeaderBatchDialog(country);
            if (dialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            BatchEdit(dialog.Mode, dialog.SelectedCountry, dialog.BatchItems, dialog.IdealRank, dialog.Skill,
                dialog.MaxSkill, dialog.Experience, dialog.Loyalty, dialog.StartYear, dialog.EndYear,
                dialog.RetirementYear, dialog.RankYear);
        }

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
        ///     閉じるボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloseButtonClick(object sender, EventArgs e)
        {
            Close();
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

            // 項目を順に登録する
            foreach (Leader leader in _list)
            {
                leaderListView.Items.Add(CreateLeaderListViewItem(leader));
            }

            if (leaderListView.Items.Count > 0)
            {
                // 先頭の項目を選択する
                leaderListView.Items[0].Focused = true;
                leaderListView.Items[0].Selected = true;

                // 編集項目を有効化する
                EnableEditableItems();
            }
            else
            {
                // 編集項目を無効化する
                DisableEditableItems();
            }

            leaderListView.EndUpdate();
        }

        /// <summary>
        ///     指揮官リストを絞り込む
        /// </summary>
        private void NarrowLeaderList()
        {
            _list.Clear();

            // 選択中の国家がなければ戻る
            if (countryListBox.SelectedIndices.Count == 0)
            {
                return;
            }

            // 特性絞り込みマスクを取得する
            uint traitsMask = GetNarrowedTraits();

            // 選択中の国家リストを作成する
            List<Country> tags = (from string s in countryListBox.SelectedItems select Countries.StringMap[s]).ToList();

            // 選択中の国家に所属する指揮官を順に絞り込む
            foreach (Leader leader in Leaders.Items.Where(leader => tags.Contains(leader.Country)))
            {
                // 兵科による絞り込み
                switch (leader.Branch)
                {
                    case Branch.Army:
                        if (!armyNarrowCheckBox.Checked)
                        {
                            continue;
                        }
                        break;

                    case Branch.Navy:
                        if (!navyNarrowCheckBox.Checked)
                        {
                            continue;
                        }
                        break;

                    case Branch.Airforce:
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

                _list.Add(leader);
            }
        }

        /// <summary>
        ///     指揮官リストをソートする
        /// </summary>
        private void SortLeaderList()
        {
            switch (_key)
            {
                case SortKey.None: // ソートなし
                    break;

                case SortKey.Tag: // 国タグ
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((leader1, leader2) => leader1.Country - leader2.Country);
                    }
                    else
                    {
                        _list.Sort((leader1, leader2) => leader2.Country - leader1.Country);
                    }
                    break;

                case SortKey.Id: // ID
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((leader1, leader2) => leader1.Id - leader2.Id);
                    }
                    else
                    {
                        _list.Sort((leader1, leader2) => leader2.Id - leader1.Id);
                    }
                    break;

                case SortKey.Name: // 名前
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((leader1, leader2) => string.CompareOrdinal(leader1.Name, leader2.Name));
                    }
                    else
                    {
                        _list.Sort((leader1, leader2) => string.CompareOrdinal(leader2.Name, leader1.Name));
                    }
                    break;

                case SortKey.Branch: // 兵科
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((leader1, leader2) => leader1.Branch - leader2.Branch);
                    }
                    else
                    {
                        _list.Sort((leader1, leader2) => leader2.Branch - leader1.Branch);
                    }
                    break;

                case SortKey.Skill: // スキル
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((leader1, leader2) => leader1.Skill - leader2.Skill);
                    }
                    else
                    {
                        _list.Sort((leader1, leader2) => leader2.Skill - leader1.Skill);
                    }
                    break;

                case SortKey.MaxSkill: // 最大スキル
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((leader1, leader2) => leader1.MaxSkill - leader2.MaxSkill);
                    }
                    else
                    {
                        _list.Sort((leader1, leader2) => leader2.MaxSkill - leader1.MaxSkill);
                    }
                    break;

                case SortKey.StartYear: // 開始年
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((leader1, leader2) => leader1.StartYear - leader2.StartYear);
                    }
                    else
                    {
                        _list.Sort((leader1, leader2) => leader2.StartYear - leader1.StartYear);
                    }
                    break;

                case SortKey.EndYear: // 終了年
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((leader1, leader2) => leader1.EndYear - leader2.EndYear);
                    }
                    else
                    {
                        _list.Sort((leader1, leader2) => leader2.EndYear - leader1.EndYear);
                    }
                    break;

                case SortKey.Traits: // 特性
                    if (_order == SortOrder.Ascendant)
                    {
                        _list.Sort((leader1, leader2) => (int) (leader1.Traits - (long) leader2.Traits));
                    }
                    else
                    {
                        _list.Sort((leader1, leader2) => (int) (leader2.Traits - (long) leader1.Traits));
                    }
                    break;
            }
        }

        /// <summary>
        ///     指揮官リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLeaderListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 編集項目を更新する
            UpdateEditableItems();
        }

        /// <summary>
        ///     指揮官リストビューのカラムクリック時の処理
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

                case 3: // 兵科
                    if (_key == SortKey.Branch)
                    {
                        _order = (_order == SortOrder.Ascendant) ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.Branch;
                    }
                    break;

                case 4: // スキル
                    if (_key == SortKey.Skill)
                    {
                        _order = (_order == SortOrder.Ascendant) ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.Skill;
                    }
                    break;

                case 5: // 最大スキル
                    if (_key == SortKey.MaxSkill)
                    {
                        _order = (_order == SortOrder.Ascendant) ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.MaxSkill;
                    }
                    break;

                case 6: // 開始年
                    if (_key == SortKey.StartYear)
                    {
                        _order = (_order == SortOrder.Ascendant) ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.StartYear;
                    }
                    break;

                case 7: // 終了年
                    if (_key == SortKey.EndYear)
                    {
                        _order = (_order == SortOrder.Ascendant) ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.EndYear;
                    }
                    break;

                case 8: // 特性
                    if (_key == SortKey.Traits)
                    {
                        _order = (_order == SortOrder.Ascendant) ? SortOrder.Decendant : SortOrder.Ascendant;
                    }
                    else
                    {
                        _key = SortKey.Traits;
                    }
                    break;

                default:
                    // 項目のない列をクリックした時には何もしない
                    return;
            }

            // 指揮官リストをソートする
            SortLeaderList();

            // 指揮官リストを更新する
            UpdateLeaderList();
        }

        /// <summary>
        ///     指揮官リストビューの列の幅変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLeaderListViewColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if ((e.ColumnIndex >= 0) && (e.ColumnIndex < LeaderListColumnCount))
            {
                HoI2Editor.Settings.LeaderEditor.ListColumnWidth[e.ColumnIndex] =
                    leaderListView.Columns[e.ColumnIndex].Width;
            }
        }

        /// <summary>
        ///     新規ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNewButtonClick(object sender, EventArgs e)
        {
            Leader leader;
            Leader selected = GetSelectedLeader();
            if (selected != null)
            {
                // 選択項目がある場合、国タグやIDを引き継いで項目を作成する
                leader = new Leader
                {
                    Country = selected.Country,
                    Id = Leaders.GetNewId(selected.Country),
                    Branch = selected.Branch,
                    IdealRank = selected.IdealRank,
                    Skill = selected.Skill,
                    MaxSkill = selected.MaxSkill,
                    Experience = selected.Experience,
                    Loyalty = selected.Loyalty,
                    StartYear = selected.StartYear,
                    EndYear = selected.EndYear,
                    RetirementYear = selected.RetirementYear
                };
                leader.RankYear[0] = selected.RankYear[0];
                leader.RankYear[1] = selected.RankYear[1];
                leader.RankYear[2] = selected.RankYear[2];
                leader.RankYear[3] = selected.RankYear[3];

                // 指揮官ごとの編集済みフラグを設定する
                leader.SetDirtyAll();

                // 指揮官リストに項目を挿入する
                Leaders.InsertItem(leader, selected);
                InsertListItem(leader, leaderListView.SelectedIndices[0] + 1);
            }
            else
            {
                Country country = Countries.Tags[countryListBox.SelectedIndex];
                // 新規項目を作成する
                leader = new Leader
                {
                    Country = country,
                    Id = Leaders.GetNewId(country),
                    Branch = Branch.None,
                    IdealRank = LeaderRank.None,
                    StartYear = 1930,
                    EndYear = 1990,
                    RetirementYear = 1999
                };
                leader.RankYear[0] = 1930;
                leader.RankYear[1] = 1990;
                leader.RankYear[2] = 1990;
                leader.RankYear[3] = 1990;

                // 指揮官ごとの編集済みフラグを設定する
                leader.SetDirtyAll();

                // 指揮官リストに項目を追加する
                Leaders.AddItem(leader);
                AddListItem(leader);

                // 編集項目を有効化する
                EnableEditableItems();
            }

            // 国家ごとの編集済みフラグを設定する
            Leaders.SetDirty(leader.Country);

            // ファイル一覧に存在しなければ追加する
            if (!Leaders.FileNameMap.ContainsKey(leader.Country))
            {
                Leaders.FileNameMap.Add(leader.Country, Game.GetLeaderFileName(leader.Country));
                Leaders.SetDirtyList();
            }
        }

        /// <summary>
        ///     複製ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloneButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader selected = GetSelectedLeader();
            if (selected == null)
            {
                return;
            }

            // 選択項目を引き継いで項目を作成する
            Leader leader = new Leader
            {
                Country = selected.Country,
                Id = Leaders.GetNewId(selected.Country),
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

            // 指揮官ごとの編集済みフラグを設定する
            leader.SetDirtyAll();

            // 指揮官リストに項目を挿入する
            Leaders.InsertItem(leader, selected);
            InsertListItem(leader, leaderListView.SelectedIndices[0] + 1);

            // 国家ごとの編集済みフラグを設定する
            Leaders.SetDirty(leader.Country);
        }

        /// <summary>
        ///     削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRemoveButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader selected = GetSelectedLeader();
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
            Leaders.SetDirty(selected.Country);
        }

        /// <summary>
        ///     先頭へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTopButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader selected = GetSelectedLeader();
            if (selected == null)
            {
                return;
            }

            // 選択項目がリストの先頭ならば何もしない
            int index = leaderListView.SelectedIndices[0];
            if (index == 0)
            {
                return;
            }

            Leader top = leaderListView.Items[0].Tag as Leader;
            if (top == null)
            {
                return;
            }

            // 指揮官リストの項目を移動する
            Leaders.MoveItem(selected, top);
            MoveListItem(index, 0);

            // 編集済みフラグを設定する
            Leaders.SetDirty(selected.Country);
        }

        /// <summary>
        ///     上へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader selected = GetSelectedLeader();
            if (selected == null)
            {
                return;
            }

            // 選択項目がリストの先頭ならば何もしない
            int index = leaderListView.SelectedIndices[0];
            if (index == 0)
            {
                return;
            }

            Leader upper = leaderListView.Items[index - 1].Tag as Leader;
            if (upper == null)
            {
                return;
            }

            // 指揮官リストの項目を移動する
            Leaders.MoveItem(selected, upper);
            MoveListItem(index, index - 1);

            // 編集済みフラグを設定する
            Leaders.SetDirty(selected.Country);
        }

        /// <summary>
        ///     下へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDownButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader selected = GetSelectedLeader();
            if (selected == null)
            {
                return;
            }

            // 選択項目がリストの末尾ならば何もしない
            int index = leaderListView.SelectedIndices[0];
            if (index == leaderListView.Items.Count - 1)
            {
                return;
            }

            Leader lower = leaderListView.Items[index + 1].Tag as Leader;
            if (lower == null)
            {
                return;
            }

            // 指揮官リストの項目を移動する
            Leaders.MoveItem(selected, lower);
            MoveListItem(index, index + 1);

            // 編集済みフラグを設定する
            Leaders.SetDirty(selected.Country);
        }

        /// <summary>
        ///     末尾へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBottomButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader selected = GetSelectedLeader();
            if (selected == null)
            {
                return;
            }

            // 選択項目がリストの末尾ならば何もしない
            int index = leaderListView.SelectedIndices[0];
            if (index == leaderListView.Items.Count - 1)
            {
                return;
            }

            Leader bottom = leaderListView.Items[leaderListView.Items.Count - 1].Tag as Leader;
            if (bottom == null)
            {
                return;
            }

            // 指揮官リストの項目を移動する
            Leaders.MoveItem(selected, bottom);
            MoveListItem(index, leaderListView.Items.Count - 1);

            // 編集済みフラグを設定する
            Leaders.SetDirty(selected.Country);
        }

        /// <summary>
        ///     指揮官リストに項目を追加する
        /// </summary>
        /// <param name="leader">挿入対象の項目</param>
        private void AddListItem(Leader leader)
        {
            // 絞り込みリストに項目を追加する
            _list.Add(leader);

            // 指揮官リストビューに項目を追加する
            leaderListView.Items.Add(CreateLeaderListViewItem(leader));

            // 追加した項目を選択する
            leaderListView.Items[leaderListView.Items.Count - 1].Focused = true;
            leaderListView.Items[leaderListView.Items.Count - 1].Selected = true;
            leaderListView.EnsureVisible(leaderListView.Items.Count - 1);
        }

        /// <summary>
        ///     指揮官リストに項目を挿入する
        /// </summary>
        /// <param name="leader">挿入対象の項目</param>
        /// <param name="index">挿入先の位置</param>
        private void InsertListItem(Leader leader, int index)
        {
            // 絞り込みリストに項目を挿入する
            _list.Insert(index, leader);

            // 指揮官リストビューに項目を挿入する
            ListViewItem item = CreateLeaderListViewItem(leader);
            leaderListView.Items.Insert(index, item);

            // 挿入した項目を選択する
            leaderListView.Items[index].Focused = true;
            leaderListView.Items[index].Selected = true;
            leaderListView.EnsureVisible(index);
        }

        /// <summary>
        ///     指揮官リストから項目を削除する
        /// </summary>
        /// <param name="index">削除対象の位置</param>
        private void RemoveItem(int index)
        {
            // 絞り込みリストから項目を削除する
            _list.RemoveAt(index);

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
            Leader leader = _list[src];
            ListViewItem item = CreateLeaderListViewItem(leader);

            if (src > dest)
            {
                // 上へ移動する場合
                // 絞り込みリストの項目を移動する
                _list.Insert(dest, leader);
                _list.RemoveAt(src + 1);

                // 指揮官リストビューの項目を移動する
                leaderListView.Items.Insert(dest, item);
                leaderListView.Items.RemoveAt(src + 1);
            }
            else
            {
                // 下へ移動する場合
                // 絞り込みリストの項目を移動する
                _list.Insert(dest + 1, leader);
                _list.RemoveAt(src);

                // 指揮官リストビューの項目を移動する
                leaderListView.Items.Insert(dest + 1, item);
                leaderListView.Items.RemoveAt(src);
            }

            // 移動先の項目を選択する
            leaderListView.Items[dest].Focused = true;
            leaderListView.Items[dest].Selected = true;
            leaderListView.EnsureVisible(dest);
        }

        /// <summary>
        ///     指揮官リストビューの項目を作成する
        /// </summary>
        /// <param name="leader">指揮官データ</param>
        /// <returns>指揮官リストビューの項目</returns>
        private static ListViewItem CreateLeaderListViewItem(Leader leader)
        {
            ListViewItem item = new ListViewItem
            {
                Text = Countries.Strings[(int) leader.Country],
                Tag = leader
            };
            item.SubItems.Add(IntHelper.ToString(leader.Id));
            item.SubItems.Add(leader.Name);
            item.SubItems.Add(Branches.GetName(leader.Branch));
            item.SubItems.Add(IntHelper.ToString(leader.Skill));
            item.SubItems.Add(IntHelper.ToString(leader.MaxSkill));
            item.SubItems.Add(IntHelper.ToString(leader.StartYear));
            item.SubItems.Add(IntHelper.ToString(leader.EndYear));
            item.SubItems.Add(GetLeaderTraitsText(leader.Traits));

            return item;
        }

        /// <summary>
        ///     選択中の指揮官データを取得する
        /// </summary>
        /// <returns>選択中の指揮官データ</returns>
        private Leader GetSelectedLeader()
        {
            // 選択項目がない場合
            if (leaderListView.SelectedItems.Count == 0)
            {
                return null;
            }

            return leaderListView.SelectedItems[0].Tag as Leader;
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
                .Where(id => (traits & Leaders.TraitsValues[(int) id]) != 0)
                .Aggregate("",
                    (current, id) =>
                        $"{current}, {Config.GetText(Leaders.TraitsNames[(int) id])}");
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
            countryListBox.BeginUpdate();
            countryListBox.Items.Clear();
            foreach (Country country in Countries.Tags)
            {
                countryListBox.Items.Add(Countries.Strings[(int) country]);
            }

            // 選択イベントを処理すると時間がかかるので、一時的に無効化する
            countryListBox.SelectedIndexChanged -= OnCountryListBoxSelectedIndexChanged;
            // 選択中の国家を反映する
            foreach (Country country in HoI2Editor.Settings.LeaderEditor.Countries)
            {
                int index = Array.IndexOf(Countries.Tags, country);
                if (index >= 0)
                {
                    countryListBox.SetSelected(Array.IndexOf(Countries.Tags, country), true);
                }
            }
            // 選択イベントを元に戻す
            countryListBox.SelectedIndexChanged += OnCountryListBoxSelectedIndexChanged;

            int count = countryListBox.SelectedItems.Count;
            // 選択数に合わせて全選択/全解除を切り替える
            countryAllButton.Text = (count <= 1) ? Resources.KeySelectAll : Resources.KeyUnselectAll;
            // 選択数がゼロの場合は新規追加ボタンを無効化する
            newButton.Enabled = (count > 0);

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
                brush = Leaders.IsDirty(country) ? new SolidBrush(Color.Red) : new SolidBrush(SystemColors.WindowText);
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

            // 選択中の国家を保存する
            HoI2Editor.Settings.LeaderEditor.Countries =
                countryListBox.SelectedIndices.Cast<int>().Select(index => Countries.Tags[index]).ToList();

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
            Graphics g = Graphics.FromHwnd(Handle);
            int margin = DeviceCaps.GetScaledWidth(2) + 1;

            // 国タグ
            countryComboBox.BeginUpdate();
            countryComboBox.Items.Clear();
            int width = countryComboBox.Width;
            foreach (string s in Countries.Tags
                .Select(country => Countries.Strings[(int) country])
                .Select(name => Config.ExistsKey(name)
                    ? $"{name} {Config.GetText(name)}"
                    : name))
            {
                countryComboBox.Items.Add(s);
                width = Math.Max(width,
                    (int) g.MeasureString(s, countryComboBox.Font).Width + SystemInformation.VerticalScrollBarWidth +
                    margin);
            }
            countryComboBox.DropDownWidth = width;
            countryComboBox.EndUpdate();

            // 兵科
            branchComboBox.BeginUpdate();
            branchComboBox.Items.Clear();
            width = branchComboBox.Width;
            foreach (string s in Branches.GetNames())
            {
                branchComboBox.Items.Add(s);
                width = Math.Max(width, (int) g.MeasureString(s, branchComboBox.Font).Width + margin);
            }
            branchComboBox.DropDownWidth = width;
            branchComboBox.EndUpdate();

            armyNarrowCheckBox.Text = Branches.GetName(Branch.Army);
            navyNarrowCheckBox.Text = Branches.GetName(Branch.Navy);
            airforceNarrowCheckBox.Text = Branches.GetName(Branch.Airforce);

            // 階級
            idealRankComboBox.BeginUpdate();
            idealRankComboBox.Items.Clear();
            width = idealRankComboBox.Width;
            foreach (string s in Leaders.RankNames.Where(name => !string.IsNullOrEmpty(name)))
            {
                idealRankComboBox.Items.Add(s);
                width = Math.Max(width, (int) g.MeasureString(s, idealRankComboBox.Font).Width + margin);
            }
            idealRankComboBox.DropDownWidth = width;
            idealRankComboBox.EndUpdate();
        }

        /// <summary>
        ///     編集項目を更新する
        /// </summary>
        private void UpdateEditableItems()
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 編集項目を更新する
            UpdateEditableItemsValue(leader);

            // 編集項目の色を更新する
            UpdateEditableItemsColor(leader);

            // 項目移動ボタンの状態更新
            int index = leaderListView.SelectedIndices[0];
            int bottom = leaderListView.Items.Count - 1;
            topButton.Enabled = (index != 0);
            upButton.Enabled = (index != 0);
            downButton.Enabled = (index != bottom);
            bottomButton.Enabled = (index != bottom);
        }

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="leader">指揮官データ</param>
        private void UpdateEditableItemsValue(Leader leader)
        {
            // 編集項目の値を更新する
            countryComboBox.SelectedIndex = leader.Country != Country.None ? (int) leader.Country - 1 : -1;
            idNumericUpDown.Value = leader.Id;
            nameTextBox.Text = leader.Name;
            branchComboBox.SelectedIndex = leader.Branch != Branch.None ? (int) leader.Branch - 1 : -1;
            idealRankComboBox.SelectedIndex = leader.IdealRank != LeaderRank.None ? (int) leader.IdealRank - 1 : -1;
            skillNumericUpDown.Value = leader.Skill;
            maxSkillNumericUpDown.Value = leader.MaxSkill;
            experienceNumericUpDown.Value = leader.Experience;
            loyaltyNumericUpDown.Value = leader.Loyalty;
            startYearNumericUpDown.Value = leader.StartYear;
            endYearNumericUpDown.Value = leader.EndYear;
            if (Misc.EnableRetirementYearLeaders)
            {
                retirementYearLabel.Enabled = true;
                retirementYearNumericUpDown.Enabled = true;
                retirementYearNumericUpDown.Value = leader.RetirementYear;
                retirementYearNumericUpDown.Text = IntHelper.ToString((int) retirementYearNumericUpDown.Value);
            }
            else
            {
                retirementYearLabel.Enabled = false;
                retirementYearNumericUpDown.Enabled = false;
                retirementYearNumericUpDown.ResetText();
            }
            rankYearNumericUpDown1.Value = leader.RankYear[0];
            rankYearNumericUpDown2.Value = leader.RankYear[1];
            rankYearNumericUpDown3.Value = leader.RankYear[2];
            rankYearNumericUpDown4.Value = leader.RankYear[3];
            pictureNameTextBox.Text = leader.PictureName;
            UpdateLeaderPicture(leader);

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
        }

        /// <summary>
        ///     編集項目の色を更新する
        /// </summary>
        /// <param name="leader"></param>
        private void UpdateEditableItemsColor(Leader leader)
        {
            // コンボボックスの色を更新する
            countryComboBox.Refresh();
            branchComboBox.Refresh();
            idealRankComboBox.Refresh();

            // 編集項目の色を更新する
            idNumericUpDown.ForeColor = leader.IsDirty(LeaderItemId.Id) ? Color.Red : SystemColors.WindowText;
            nameTextBox.ForeColor = leader.IsDirty(LeaderItemId.Name) ? Color.Red : SystemColors.WindowText;
            skillNumericUpDown.ForeColor = leader.IsDirty(LeaderItemId.Skill) ? Color.Red : SystemColors.WindowText;
            maxSkillNumericUpDown.ForeColor = leader.IsDirty(LeaderItemId.MaxSkill)
                ? Color.Red
                : SystemColors.WindowText;
            experienceNumericUpDown.ForeColor = leader.IsDirty(LeaderItemId.Experience)
                ? Color.Red
                : SystemColors.WindowText;
            loyaltyNumericUpDown.ForeColor = leader.IsDirty(LeaderItemId.Loyalty) ? Color.Red : SystemColors.WindowText;
            startYearNumericUpDown.ForeColor = leader.IsDirty(LeaderItemId.StartYear)
                ? Color.Red
                : SystemColors.WindowText;
            endYearNumericUpDown.ForeColor = leader.IsDirty(LeaderItemId.EndYear) ? Color.Red : SystemColors.WindowText;
            retirementYearNumericUpDown.ForeColor = leader.IsDirty(LeaderItemId.RetirementYear)
                ? Color.Red
                : SystemColors.WindowText;
            rankYearNumericUpDown1.ForeColor = leader.IsDirty(LeaderItemId.Rank3Year)
                ? Color.Red
                : SystemColors.WindowText;
            rankYearNumericUpDown2.ForeColor = leader.IsDirty(LeaderItemId.Rank2Year)
                ? Color.Red
                : SystemColors.WindowText;
            rankYearNumericUpDown3.ForeColor = leader.IsDirty(LeaderItemId.Rank1Year)
                ? Color.Red
                : SystemColors.WindowText;
            rankYearNumericUpDown4.ForeColor = leader.IsDirty(LeaderItemId.Rank0Year)
                ? Color.Red
                : SystemColors.WindowText;
            pictureNameTextBox.ForeColor = leader.IsDirty(LeaderItemId.PictureName)
                ? Color.Red
                : SystemColors.WindowText;

            // 特性チェックボックスの項目色を更新する
            logisticsWizardCheckBox.ForeColor = leader.IsDirty(LeaderItemId.LogisticsWizard)
                ? Color.Red
                : SystemColors.WindowText;
            defensiveDoctrineCheckBox.ForeColor = leader.IsDirty(LeaderItemId.DefensiveDoctrine)
                ? Color.Red
                : SystemColors.WindowText;
            offensiveDoctrineCheckBox.ForeColor = leader.IsDirty(LeaderItemId.OffensiveDoctrine)
                ? Color.Red
                : SystemColors.WindowText;
            winterSpecialistCheckBox.ForeColor = leader.IsDirty(LeaderItemId.WinterSpecialist)
                ? Color.Red
                : SystemColors.WindowText;
            tricksterCheckBox.ForeColor = leader.IsDirty(LeaderItemId.Trickster) ? Color.Red : SystemColors.WindowText;
            engineerCheckBox.ForeColor = leader.IsDirty(LeaderItemId.Engineer) ? Color.Red : SystemColors.WindowText;
            fortressBusterCheckBox.ForeColor = leader.IsDirty(LeaderItemId.FortressBuster)
                ? Color.Red
                : SystemColors.WindowText;
            panzerLeaderCheckBox.ForeColor = leader.IsDirty(LeaderItemId.PanzerLeader)
                ? Color.Red
                : SystemColors.WindowText;
            commandoCheckBox.ForeColor = leader.IsDirty(LeaderItemId.Commando) ? Color.Red : SystemColors.WindowText;
            oldGuardCheckBox.ForeColor = leader.IsDirty(LeaderItemId.OldGuard) ? Color.Red : SystemColors.WindowText;
            seaWolfCheckBox.ForeColor = leader.IsDirty(LeaderItemId.SeaWolf) ? Color.Red : SystemColors.WindowText;
            blockadeRunnerCheckBox.ForeColor = leader.IsDirty(LeaderItemId.BlockadeRunner)
                ? Color.Red
                : SystemColors.WindowText;
            superiorTacticianCheckBox.ForeColor = leader.IsDirty(LeaderItemId.SuperiorTactician)
                ? Color.Red
                : SystemColors.WindowText;
            spotterCheckBox.ForeColor = leader.IsDirty(LeaderItemId.Spotter) ? Color.Red : SystemColors.WindowText;
            tankBusterCheckBox.ForeColor = leader.IsDirty(LeaderItemId.TankBuster) ? Color.Red : SystemColors.WindowText;
            carpetBomberCheckBox.ForeColor = leader.IsDirty(LeaderItemId.CarpetBomber)
                ? Color.Red
                : SystemColors.WindowText;
            nightFlyerCheckBox.ForeColor = leader.IsDirty(LeaderItemId.NightFlyer) ? Color.Red : SystemColors.WindowText;
            fleetDestroyerCheckBox.ForeColor = leader.IsDirty(LeaderItemId.FleetDestroyer)
                ? Color.Red
                : SystemColors.WindowText;
            desertFoxCheckBox.ForeColor = leader.IsDirty(LeaderItemId.DesertFox) ? Color.Red : SystemColors.WindowText;
            jungleRatCheckBox.ForeColor = leader.IsDirty(LeaderItemId.JungleRat) ? Color.Red : SystemColors.WindowText;
            urbanWarfareSpecialistCheckBox.ForeColor = leader.IsDirty(LeaderItemId.UrbanWarfareSpecialist)
                ? Color.Red
                : SystemColors.WindowText;
            rangerCheckBox.ForeColor = leader.IsDirty(LeaderItemId.Ranger) ? Color.Red : SystemColors.WindowText;
            mountaineerCheckBox.ForeColor = leader.IsDirty(LeaderItemId.Mountaineer)
                ? Color.Red
                : SystemColors.WindowText;
            hillsFighterCheckBox.ForeColor = leader.IsDirty(LeaderItemId.HillsFighter)
                ? Color.Red
                : SystemColors.WindowText;
            counterAttackerCheckBox.ForeColor = leader.IsDirty(LeaderItemId.CounterAttacker)
                ? Color.Red
                : SystemColors.WindowText;
            assaulterCheckBox.ForeColor = leader.IsDirty(LeaderItemId.Assaulter) ? Color.Red : SystemColors.WindowText;
            encirclerCheckBox.ForeColor = leader.IsDirty(LeaderItemId.Encircler) ? Color.Red : SystemColors.WindowText;
            ambusherCheckBox.ForeColor = leader.IsDirty(LeaderItemId.Ambusher) ? Color.Red : SystemColors.WindowText;
            disciplinedCheckBox.ForeColor = leader.IsDirty(LeaderItemId.Disciplined)
                ? Color.Red
                : SystemColors.WindowText;
            elasticDefenceSpecialistCheckBox.ForeColor = leader.IsDirty(LeaderItemId.ElasticDefenceSpecialist)
                ? Color.Red
                : SystemColors.WindowText;
            blitzerCheckBox.ForeColor = leader.IsDirty(LeaderItemId.Blitzer) ? Color.Red : SystemColors.WindowText;
        }

        /// <summary>
        ///     編集項目を有効化する
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
            pictureNameBrowseButton.Enabled = true;
            traitsGroupBox.Enabled = true;

            // 無効化時にクリアした文字列を再設定する
            idNumericUpDown.Text = IntHelper.ToString((int) idNumericUpDown.Value);
            skillNumericUpDown.Text = IntHelper.ToString((int) skillNumericUpDown.Value);
            maxSkillNumericUpDown.Text = IntHelper.ToString((int) maxSkillNumericUpDown.Value);
            experienceNumericUpDown.Text = IntHelper.ToString((int) experienceNumericUpDown.Value);
            loyaltyNumericUpDown.Text = IntHelper.ToString((int) loyaltyNumericUpDown.Value);
            startYearNumericUpDown.Text = IntHelper.ToString((int) startYearNumericUpDown.Value);
            endYearNumericUpDown.Text = IntHelper.ToString((int) endYearNumericUpDown.Value);
            rankYearNumericUpDown1.Text = IntHelper.ToString((int) rankYearNumericUpDown1.Value);
            rankYearNumericUpDown2.Text = IntHelper.ToString((int) rankYearNumericUpDown2.Value);
            rankYearNumericUpDown3.Text = IntHelper.ToString((int) rankYearNumericUpDown3.Value);
            rankYearNumericUpDown4.Text = IntHelper.ToString((int) rankYearNumericUpDown4.Value);

            if (Misc.EnableRetirementYearLeaders)
            {
                retirementYearNumericUpDown.Enabled = true;
                retirementYearNumericUpDown.Text = IntHelper.ToString((int) retirementYearNumericUpDown.Value);
            }

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

            ResetTraitsCheckBoxValue();

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
            pictureNameBrowseButton.Enabled = false;
            traitsGroupBox.Enabled = false;

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
            Leader leader = GetSelectedLeader();
            if (leader != null)
            {
                Brush brush;
                if ((Countries.Tags[e.Index] == leader.Country) && leader.IsDirty(LeaderItemId.Country))
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
        ///     兵科コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBranchComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Leader leader = GetSelectedLeader();
            if (leader != null)
            {
                Brush brush;
                if ((e.Index == (int) leader.Branch - 1) && leader.IsDirty(LeaderItemId.Branch))
                {
                    brush = new SolidBrush(Color.Red);
                }
                else
                {
                    brush = new SolidBrush(SystemColors.WindowText);
                }
                string s = branchComboBox.Items[e.Index].ToString();
                e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     理想階級コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIdealRankComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Leader leader = GetSelectedLeader();
            if (leader != null)
            {
                Brush brush;
                if ((e.Index == (int) leader.IdealRank - 1) && leader.IsDirty(LeaderItemId.IdealRank))
                {
                    brush = new SolidBrush(Color.Red);
                }
                else
                {
                    brush = new SolidBrush(SystemColors.WindowText);
                }
                string s = idealRankComboBox.Items[e.Index].ToString();
                e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     指揮官画像ピクチャーボックスの項目を更新する
        /// </summary>
        /// <param name="leader">指揮官データ</param>
        private void UpdateLeaderPicture(Leader leader)
        {
            if (!string.IsNullOrEmpty(leader.PictureName))
            {
                string fileName = Game.GetReadFileName(Game.PersonPicturePathName,
                    Path.ChangeExtension(leader.PictureName, ".bmp"));
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
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            Country country = Countries.Tags[countryComboBox.SelectedIndex];
            if (country == leader.Country)
            {
                return;
            }

            // 変更前の国タグの編集済みフラグを設定する
            Leaders.SetDirty(leader.Country);

            Log.Info("[Leader] country: {0} -> {1} ({2}: {3})", Countries.Strings[(int) leader.Country],
                Countries.Strings[(int) country], leader.Id, leader.Name);

            // 値を更新する
            leader.Country = country;

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].Text = Countries.Strings[(int) leader.Country];

            // 指揮官ごとの編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.Country);

            // 変更後の国タグの編集済みフラグを設定する
            Leaders.SetDirty(leader.Country);

            // ファイル一覧に存在しなければ追加する
            if (!Leaders.FileNameMap.ContainsKey(leader.Country))
            {
                Leaders.FileNameMap.Add(leader.Country, Game.GetLeaderFileName(leader.Country));
                Leaders.SetDirtyList();
            }

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
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            int id = (int) idNumericUpDown.Value;
            if (id == leader.Id)
            {
                return;
            }

            Log.Info("[Leader] id: {0} -> {1} ({2})", leader.Id, id, leader.Name);

            // 値を更新する
            leader.Id = id;

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[1].Text = IntHelper.ToString(leader.Id);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.Id);
            Leaders.SetDirty(leader.Country);

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
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            string name = nameTextBox.Text;
            if (string.IsNullOrEmpty(name))
            {
                if (string.IsNullOrEmpty(leader.Name))
                {
                    return;
                }
            }
            else
            {
                if (name.Equals(leader.Name))
                {
                    return;
                }
            }

            Log.Info("[Leader] name: {0} -> {1} ({2})", leader.Name, name, leader.Id);

            // 値を更新する
            leader.Name = name;

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[2].Text = leader.Name;

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.Name);
            Leaders.SetDirty(leader.Country);

            // 文字色を変更する
            nameTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     兵科変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBranchComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            Branch branch = (Branch) (branchComboBox.SelectedIndex + 1);
            if (branch == leader.Branch)
            {
                return;
            }

            Log.Info("[Leader] branch: {0} -> {1} ({2}: {3})", Branches.GetName(leader.Branch), Branches.GetName(branch),
                leader.Id, leader.Name);

            // 値を更新する
            leader.Branch = branch;

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[3].Text = Branches.GetName(leader.Branch);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.Branch);
            Leaders.SetDirty(leader.Country);

            // 兵科コンボボックスの項目色を変更するため描画更新する
            branchComboBox.Refresh();
        }

        /// <summary>
        ///     理想階級変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIdealRankComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            LeaderRank idealRank = (LeaderRank) (idealRankComboBox.SelectedIndex + 1);
            if (idealRank == leader.IdealRank)
            {
                return;
            }

            Log.Info("[Leader] ideak rank: {0} -> {1} ({2}: {3})", Leaders.RankNames[(int) leader.IdealRank],
                Leaders.RankNames[(int) idealRank], leader.Id, leader.Name);

            // 値を更新する
            leader.IdealRank = idealRank;

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.IdealRank);
            Leaders.SetDirty(leader.Country);

            // 理想階級コンボボックスの項目色を変更するため描画更新する
            idealRankComboBox.Refresh();
        }

        /// <summary>
        ///     スキル変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSkillNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            int skill = (int) skillNumericUpDown.Value;
            if (skill == leader.Skill)
            {
                return;
            }

            Log.Info("[Leader] skill: {0} -> {1} ({2}: {3})", leader.Skill, skill, leader.Id, leader.Name);

            // 値を更新する
            leader.Skill = skill;

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[4].Text = IntHelper.ToString(leader.Skill);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.Skill);
            Leaders.SetDirty(leader.Country);

            // 文字色を変更する
            skillNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     最大スキル変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMaxSkillNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            int maxSkill = (int) maxSkillNumericUpDown.Value;
            if (maxSkill == leader.MaxSkill)
            {
                return;
            }

            Log.Info("[Leader] max skill: {0} -> {1} ({2}: {3})", leader.MaxSkill, maxSkill, leader.Id, leader.Name);

            // 値を更新する
            leader.MaxSkill = maxSkill;

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[5].Text = IntHelper.ToString(leader.MaxSkill);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.MaxSkill);
            Leaders.SetDirty(leader.Country);

            // 文字色を変更する
            maxSkillNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     経験値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExperienceNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            int experience = (int) experienceNumericUpDown.Value;
            if (experience == leader.Experience)
            {
                return;
            }

            Log.Info("[Leader] experience: {0} -> {1} ({2}: {3})", leader.Experience, experience, leader.Id, leader.Name);

            // 値を更新する
            leader.Experience = experience;

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.Experience);
            Leaders.SetDirty(leader.Country);

            // 文字色を変更する
            experienceNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     忠誠度変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoyaltyNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }
            // 値に変化がなければ何もしない
            int loyalty = (int) loyaltyNumericUpDown.Value;
            if (loyalty == leader.Loyalty)
            {
                return;
            }

            Log.Info("[Leader] loyalty: {0} -> {1} ({2}: {3})", leader.Loyalty, loyalty, leader.Id, leader.Name);

            // 値を更新する
            leader.Loyalty = loyalty;

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.Loyalty);
            Leaders.SetDirty(leader.Country);

            // 文字色を変更する
            loyaltyNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     開始年変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStartYearNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            int startYear = (int) startYearNumericUpDown.Value;
            if (startYear == leader.StartYear)
            {
                return;
            }

            Log.Info("[Leader] start year: {0} -> {1} ({2}: {3})", leader.StartYear, startYear, leader.Id, leader.Name);

            // 値を更新する
            leader.StartYear = startYear;

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[6].Text = IntHelper.ToString(leader.StartYear);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.StartYear);
            Leaders.SetDirty(leader.Country);

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
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            int endYear = (int) endYearNumericUpDown.Value;
            if (endYear == leader.EndYear)
            {
                return;
            }

            Log.Info("[Leader] end year: {0} -> {1} ({2}: {3})", leader.EndYear, endYear, leader.Id, leader.Name);

            // 値を更新する
            leader.EndYear = endYear;

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[7].Text = IntHelper.ToString(leader.EndYear);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.EndYear);
            Leaders.SetDirty(leader.Country);

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
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            int retirementYear = (int) retirementYearNumericUpDown.Value;
            if (retirementYear == leader.RetirementYear)
            {
                return;
            }

            Log.Info("[Leader] retirement year: {0} -> {1} ({2}: {3})", leader.RetirementYear, retirementYear, leader.Id,
                leader.Name);

            // 値を更新する
            leader.RetirementYear = retirementYear;

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.RetirementYear);
            Leaders.SetDirty(leader.Country);

            // 文字色を変更する
            retirementYearNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     少将任官年変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRankYearNumericUpDown1ValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            int year = (int) rankYearNumericUpDown1.Value;
            if (year == leader.RankYear[0])
            {
                return;
            }

            Log.Info("[Leader] rank3 year: {0} -> {1} ({2}: {3})", leader.RankYear[0], year, leader.Id, leader.Name);

            // 値を更新する
            leader.RankYear[0] = year;

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.Rank3Year);
            Leaders.SetDirty(leader.Country);

            // 文字色を変更する
            rankYearNumericUpDown1.ForeColor = Color.Red;
        }

        /// <summary>
        ///     中将任官年変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRankYearNumericUpDown2ValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            int year = (int) rankYearNumericUpDown2.Value;
            if (year == leader.RankYear[1])
            {
                return;
            }

            Log.Info("[Leader] rank2 year: {0} -> {1} ({2}: {3})", leader.RankYear[1], year, leader.Id, leader.Name);

            // 値を更新する
            leader.RankYear[1] = year;

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.Rank2Year);
            Leaders.SetDirty(leader.Country);

            // 文字色を変更する
            rankYearNumericUpDown2.ForeColor = Color.Red;
        }

        /// <summary>
        ///     大将任官年変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRankYearNumericUpDown3ValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            int year = (int) rankYearNumericUpDown3.Value;
            if (year == leader.RankYear[2])
            {
                return;
            }

            Log.Info("[Leader] rank1 year: {0} -> {1} ({2}: {3})", leader.RankYear[2], year, leader.Id, leader.Name);

            // 値を更新する
            leader.RankYear[2] = year;

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.Rank1Year);
            Leaders.SetDirty(leader.Country);

            // 文字色を変更する
            rankYearNumericUpDown3.ForeColor = Color.Red;
        }

        /// <summary>
        ///     元帥任官年変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRankYearNumericUpDown4ValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            int year = (int) rankYearNumericUpDown4.Value;
            if (year == leader.RankYear[3])
            {
                return;
            }

            Log.Info("[Leader] rank0 year: {0} -> {1} ({2}: {3})", leader.RankYear[3], year, leader.Id, leader.Name);

            // 値を更新する
            leader.RankYear[3] = year;

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.Rank0Year);
            Leaders.SetDirty(leader.Country);

            // 文字色を変更する
            rankYearNumericUpDown4.ForeColor = Color.Red;
        }

        /// <summary>
        ///     画像ファイル名変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPictureNameTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            string pictureName = pictureNameTextBox.Text;
            if (string.IsNullOrEmpty(pictureName))
            {
                if (string.IsNullOrEmpty(leader.PictureName))
                {
                    return;
                }
            }
            else
            {
                if (pictureName.Equals(leader.PictureName))
                {
                    return;
                }
            }

            Log.Info("[Leader] picture name: {0} -> {1} ({2}: {3})", leader.PictureName, pictureName, leader.Id,
                leader.Name);

            // 値を更新する
            leader.PictureName = pictureName;

            // 指揮官画像を更新する
            UpdateLeaderPicture(leader);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.PictureName);
            Leaders.SetDirty(leader.Country);

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
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // ファイル選択ダイアログを開く
            OpenFileDialog dialog = new OpenFileDialog
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

        #endregion

        #region 指揮官特性

        /// <summary>
        ///     指揮官特性チェックボックスの値を一括設定する
        /// </summary>
        private void ResetTraitsCheckBoxValue()
        {
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
        }

        /// <summary>
        ///     兵站管理チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLogisticsWizardCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = logisticsWizardCheckBox.Checked ? LeaderTraits.LogisticsWizard : 0;
            if (((leader.Traits & LeaderTraits.LogisticsWizard) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.LogisticsWizard;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.LogisticsWizard);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            logisticsWizardCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     防勢ドクトリンチェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDefensiveDoctrineCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = defensiveDoctrineCheckBox.Checked ? LeaderTraits.DefensiveDoctrine : 0;
            if (((leader.Traits & LeaderTraits.DefensiveDoctrine) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.DefensiveDoctrine;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.DefensiveDoctrine);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            defensiveDoctrineCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     攻勢ドクトリンチェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOffensiveDoctrineCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = offensiveDoctrineCheckBox.Checked ? LeaderTraits.OffensiveDoctrine : 0;
            if (((leader.Traits & LeaderTraits.OffensiveDoctrine) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.OffensiveDoctrine;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.OffensiveDoctrine);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            offensiveDoctrineCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     冬期戦チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWinterSpecialistCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = winterSpecialistCheckBox.Checked ? LeaderTraits.WinterSpecialist : 0;
            if (((leader.Traits & LeaderTraits.WinterSpecialist) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.WinterSpecialist;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.WinterSpecialist);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            winterSpecialistCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     伏撃チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTricksterCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = tricksterCheckBox.Checked ? LeaderTraits.Trickster : 0;
            if (((leader.Traits & LeaderTraits.Trickster) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.Trickster;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.Trickster);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            tricksterCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     工兵チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEngineerCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = engineerCheckBox.Checked ? LeaderTraits.Engineer : 0;
            if (((leader.Traits & LeaderTraits.Engineer) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.Engineer;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.Engineer);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            engineerCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     要塞攻撃チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFortressBusterCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = fortressBusterCheckBox.Checked ? LeaderTraits.FortressBuster : 0;
            if (((leader.Traits & LeaderTraits.FortressBuster) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.FortressBuster;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.FortressBuster);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            fortressBusterCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     機甲戦チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPanzerLeaderCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = panzerLeaderCheckBox.Checked ? LeaderTraits.PanzerLeader : 0;
            if (((leader.Traits & LeaderTraits.PanzerLeader) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.PanzerLeader;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.PanzerLeader);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            panzerLeaderCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     特殊戦チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCommandoCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = commandoCheckBox.Checked ? LeaderTraits.Commando : 0;
            if (((leader.Traits & LeaderTraits.Commando) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.Commando;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.Commando);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            commandoCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     古典派チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOldGuardCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = oldGuardCheckBox.Checked ? LeaderTraits.OldGuard : 0;
            if (((leader.Traits & LeaderTraits.OldGuard) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.OldGuard;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.OldGuard);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            oldGuardCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     海狼チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSeaWolfCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = seaWolfCheckBox.Checked ? LeaderTraits.SeaWolf : 0;
            if (((leader.Traits & LeaderTraits.SeaWolf) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.SeaWolf;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.SeaWolf);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            seaWolfCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     封鎖線突破の達人チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBlockadeRunnerCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = blockadeRunnerCheckBox.Checked ? LeaderTraits.BlockadeRunner : 0;
            if (((leader.Traits & LeaderTraits.BlockadeRunner) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.BlockadeRunner;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.BlockadeRunner);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            blockadeRunnerCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     卓越した戦術家チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSuperiorTacticianCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = superiorTacticianCheckBox.Checked ? LeaderTraits.SuperiorTactician : 0;
            if (((leader.Traits & LeaderTraits.SuperiorTactician) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.SuperiorTactician;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.SuperiorTactician);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            superiorTacticianCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     索敵チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpotterCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = spotterCheckBox.Checked ? LeaderTraits.Spotter : 0;
            if (((leader.Traits & LeaderTraits.Spotter) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.Spotter;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.Spotter);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            spotterCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     対戦車攻撃チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTankBusterCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = tankBusterCheckBox.Checked ? LeaderTraits.TankBuster : 0;
            if (((leader.Traits & LeaderTraits.TankBuster) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.TankBuster;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.TankBuster);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            tankBusterCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     絨毯爆撃チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCarpetBomberCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = carpetBomberCheckBox.Checked ? LeaderTraits.CarpetBomber : 0;
            if (((leader.Traits & LeaderTraits.CarpetBomber) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.CarpetBomber;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.CarpetBomber);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            carpetBomberCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     夜間航空作戦チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNightFlyerCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = nightFlyerCheckBox.Checked ? LeaderTraits.NightFlyer : 0;
            if (((leader.Traits & LeaderTraits.NightFlyer) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.NightFlyer;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.NightFlyer);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            nightFlyerCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     対艦攻撃チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFleetDestroyerCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = fleetDestroyerCheckBox.Checked ? LeaderTraits.FleetDestroyer : 0;
            if (((leader.Traits & LeaderTraits.FleetDestroyer) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.FleetDestroyer;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.FleetDestroyer);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            fleetDestroyerCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     砂漠のキツネチェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDesertFoxCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = desertFoxCheckBox.Checked ? LeaderTraits.DesertFox : 0;
            if (((leader.Traits & LeaderTraits.DesertFox) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.DesertFox;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.DesertFox);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            desertFoxCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     密林のネズミチェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnJungleRatCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = jungleRatCheckBox.Checked ? LeaderTraits.JungleRat : 0;
            if (((leader.Traits & LeaderTraits.JungleRat) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.JungleRat;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.JungleRat);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            jungleRatCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     市街戦チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUrbanWarfareSpecialistCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = urbanWarfareSpecialistCheckBox.Checked ? LeaderTraits.UrbanWarfareSpecialist : 0;
            if (((leader.Traits & LeaderTraits.UrbanWarfareSpecialist) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.UrbanWarfareSpecialist;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.UrbanWarfareSpecialist);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            urbanWarfareSpecialistCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     レンジャーチェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRangerCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = rangerCheckBox.Checked ? LeaderTraits.Ranger : 0;
            if (((leader.Traits & LeaderTraits.Ranger) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.Ranger;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.Ranger);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            rangerCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     山岳戦チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMountaineerCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = mountaineerCheckBox.Checked ? LeaderTraits.Mountaineer : 0;
            if (((leader.Traits & LeaderTraits.Mountaineer) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.Mountaineer;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.Mountaineer);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            mountaineerCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     高地戦チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHillsFighterCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = hillsFighterCheckBox.Checked ? LeaderTraits.HillsFighter : 0;
            if (((leader.Traits & LeaderTraits.HillsFighter) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.HillsFighter;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.HillsFighter);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            hillsFighterCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     反撃戦チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCounterAttackerCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = counterAttackerCheckBox.Checked ? LeaderTraits.CounterAttacker : 0;
            if (((leader.Traits & LeaderTraits.CounterAttacker) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.CounterAttacker;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.CounterAttacker);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            counterAttackerCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     突撃戦チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAssaulterCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = assaulterCheckBox.Checked ? LeaderTraits.Assaulter : 0;
            if (((leader.Traits & LeaderTraits.Assaulter) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.Assaulter;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.Assaulter);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            assaulterCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     包囲戦チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEncirclerCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = encirclerCheckBox.Checked ? LeaderTraits.Encircler : 0;
            if (((leader.Traits & LeaderTraits.Encircler) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.Encircler;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.Encircler);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            encirclerCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     奇襲戦チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAmbusherCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = ambusherCheckBox.Checked ? LeaderTraits.Ambusher : 0;
            if (((leader.Traits & LeaderTraits.Ambusher) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.Ambusher;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.Ambusher);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            ambusherCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     規律チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDisiplinedCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = disciplinedCheckBox.Checked ? LeaderTraits.Disciplined : 0;
            if (((leader.Traits & LeaderTraits.Disciplined) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.Disciplined;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.Disciplined);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            disciplinedCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     戦術的退却チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnElasticDefenceSpecialistCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = elasticDefenceSpecialistCheckBox.Checked ? LeaderTraits.ElasticDefenceSpecialist : 0;
            if (((leader.Traits & LeaderTraits.ElasticDefenceSpecialist) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.ElasticDefenceSpecialist;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.ElasticDefenceSpecialist);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            elasticDefenceSpecialistCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     電撃戦チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBlitzerCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Leader leader = GetSelectedLeader();
            if (leader == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            uint trait = blitzerCheckBox.Checked ? LeaderTraits.Blitzer : 0;
            if (((leader.Traits & LeaderTraits.Blitzer) ^ trait) == 0)
            {
                return;
            }

            uint old = leader.Traits;

            // 値を更新する
            leader.Traits &= ~LeaderTraits.Blitzer;
            leader.Traits |= trait;

            Log.Info("[Leader] traits: {0} -> {1} ({2}: {3})", old, leader.Traits, leader.Id, leader.Name);

            // 指揮官リストビューの項目を更新する
            leaderListView.SelectedItems[0].SubItems[8].Text = GetLeaderTraitsText(leader.Traits);

            // 編集済みフラグを設定する
            leader.SetDirty(LeaderItemId.Blitzer);
            Leaders.SetDirty(leader.Country);

            // 項目色を変更する
            blitzerCheckBox.ForeColor = Color.Red;
        }

        #endregion

        #region 一括編集

        /// <summary>
        ///     一括編集処理
        /// </summary>
        /// <param name="mode">一括編集モード</param>
        /// <param name="country">指定国</param>
        /// <param name="items">一括編集項目</param>
        /// <param name="idealRank">理想階級</param>
        /// <param name="skill">スキル</param>
        /// <param name="maxSkill">最大スキル</param>
        /// <param name="experience">経験値</param>
        /// <param name="loyalty">忠誠度</param>
        /// <param name="startYear">開始年</param>
        /// <param name="endYear">終了年</param>
        /// <param name="retirementYear">引退年</param>
        /// <param name="rankYear">任官年</param>
        private void BatchEdit(LeaderBatchMode mode, Country country, bool[] items, LeaderRank idealRank, int skill,
            int maxSkill, int experience, int loyalty, int startYear, int endYear, int retirementYear, int[] rankYear)
        {
            switch (mode)
            {
                case LeaderBatchMode.All:
                    BatchEditAll(items, idealRank, skill, maxSkill, experience, loyalty, startYear, endYear,
                        retirementYear, rankYear);
                    break;

                case LeaderBatchMode.Selected:
                    BatchEditSelected(items, idealRank, skill, maxSkill, experience, loyalty, startYear, endYear,
                        retirementYear, rankYear);
                    break;

                case LeaderBatchMode.Specified:
                    BatchEditSpecified(country, items, idealRank, skill, maxSkill, experience, loyalty, startYear,
                        endYear, retirementYear, rankYear);
                    break;
            }

            // 指揮官リストを更新する
            UpdateLeaderList();

            // 国家リストボックスの項目色を変更するため描画更新する
            countryListBox.Refresh();

            // 引退年が未設定ならばMiscの値を変更する
            if (items[(int) LeaderBatchItemId.RetirementYear] && !Misc.EnableRetirementYearLeaders)
            {
                Misc.EnableRetirementYearLeaders = true;
                HoI2Editor.OnItemChanged(EditorItemId.LeaderRetirementYear, this);
            }
        }

        /// <summary>
        ///     全ての指揮官を一括編集する
        /// </summary>
        /// <param name="items">一括編集項目</param>
        /// <param name="idealRank">理想階級</param>
        /// <param name="skill">スキル</param>
        /// <param name="maxSkill">最大スキル</param>
        /// <param name="experience">経験値</param>
        /// <param name="loyalty">忠誠度</param>
        /// <param name="startYear">開始年</param>
        /// <param name="endYear">終了年</param>
        /// <param name="retirementYear">引退年</param>
        /// <param name="rankYear">任官年</param>
        private static void BatchEditAll(bool[] items, LeaderRank idealRank, int skill, int maxSkill, int experience,
            int loyalty, int startYear, int endYear, int retirementYear, int[] rankYear)
        {
            // 一括編集項目が設定されていなければ戻る
            if (!Enum.GetValues(typeof (LeaderBatchItemId)).Cast<LeaderBatchItemId>().Any(id => items[(int) id]))
            {
                return;
            }

            LogBatchEdit("All", items, idealRank, skill, maxSkill, experience, loyalty, startYear, endYear,
                retirementYear, rankYear);

            // 一括編集処理を順に呼び出す
            foreach (Leader leader in Leaders.Items)
            {
                BatchEditLeader(leader, items, idealRank, skill, maxSkill, experience, loyalty, startYear, endYear,
                    retirementYear, rankYear);
            }
        }

        /// <summary>
        ///     選択国の指揮官を一括編集する
        /// </summary>
        /// <param name="items">一括編集項目</param>
        /// <param name="idealRank">理想階級</param>
        /// <param name="skill">スキル</param>
        /// <param name="maxSkill">最大スキル</param>
        /// <param name="experience">経験値</param>
        /// <param name="loyalty">忠誠度</param>
        /// <param name="startYear">開始年</param>
        /// <param name="endYear">終了年</param>
        /// <param name="retirementYear">引退年</param>
        /// <param name="rankYear">任官年</param>
        private void BatchEditSelected(bool[] items, LeaderRank idealRank, int skill, int maxSkill, int experience,
            int loyalty, int startYear, int endYear, int retirementYear, int[] rankYear)
        {
            // 一括編集項目が設定されていなければ戻る
            if (!Enum.GetValues(typeof (LeaderBatchItemId)).Cast<LeaderBatchItemId>().Any(id => items[(int) id]))
            {
                return;
            }

            // 選択中の国家リストを作成する
            Country[] countries =
                (from string s in countryListBox.SelectedItems select Countries.StringMap[s]).ToArray();
            if (countries.Length == 0)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();
            foreach (string s in countries.Select(country => Countries.Strings[(int) country]))
            {
                sb.AppendFormat("{0} ", s);
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            LogBatchEdit(sb.ToString(), items, idealRank, skill, maxSkill, experience, loyalty, startYear, endYear,
                retirementYear, rankYear);

            // 一括編集処理を順に呼び出す
            foreach (Leader leader in Leaders.Items.Where(leader => countries.Contains(leader.Country)))
            {
                BatchEditLeader(leader, items, idealRank, skill, maxSkill, experience, loyalty, startYear, endYear,
                    retirementYear, rankYear);
            }
        }

        /// <summary>
        ///     指定国の指揮官を一括編集する
        /// </summary>
        /// <param name="country">選択国</param>
        /// <param name="items">一括編集項目</param>
        /// <param name="idealRank">理想階級</param>
        /// <param name="skill">スキル</param>
        /// <param name="maxSkill">最大スキル</param>
        /// <param name="experience">経験値</param>
        /// <param name="loyalty">忠誠度</param>
        /// <param name="startYear">開始年</param>
        /// <param name="endYear">終了年</param>
        /// <param name="retirementYear">引退年</param>
        /// <param name="rankYear">任官年</param>
        private static void BatchEditSpecified(Country country, bool[] items, LeaderRank idealRank, int skill,
            int maxSkill,
            int experience, int loyalty, int startYear, int endYear, int retirementYear, int[] rankYear)
        {
            // 一括編集項目が設定されていなければ戻る
            if (!Enum.GetValues(typeof (LeaderBatchItemId)).Cast<LeaderBatchItemId>().Any(id => items[(int) id]))
            {
                return;
            }

            LogBatchEdit(Countries.Strings[(int) country], items, idealRank, skill, maxSkill, experience, loyalty,
                startYear, endYear, retirementYear, rankYear);

            // 一括編集処理を順に呼び出す
            foreach (Leader leader in Leaders.Items.Where(leader => leader.Country == country))
            {
                BatchEditLeader(leader, items, idealRank, skill, maxSkill, experience, loyalty, startYear, endYear,
                    retirementYear, rankYear);
            }
        }

        /// <summary>
        ///     一括編集処理
        /// </summary>
        /// <param name="leader">一括編集対象の指揮官</param>
        /// <param name="items">一括編集項目</param>
        /// <param name="idealRank">理想階級</param>
        /// <param name="skill">スキル</param>
        /// <param name="maxSkill">最大スキル</param>
        /// <param name="experience">経験値</param>
        /// <param name="loyalty">忠誠度</param>
        /// <param name="startYear">開始年</param>
        /// <param name="endYear">終了年</param>
        /// <param name="retirementYear">引退年</param>
        /// <param name="rankYear">任官年</param>
        private static void BatchEditLeader(Leader leader, bool[] items, LeaderRank idealRank, int skill, int maxSkill,
            int experience, int loyalty, int startYear, int endYear, int retirementYear, int[] rankYear)
        {
            // 理想階級
            if (items[(int) LeaderBatchItemId.IdealRank])
            {
                if (idealRank != leader.IdealRank)
                {
                    leader.IdealRank = idealRank;
                    leader.SetDirty(LeaderItemId.IdealRank);
                    Leaders.SetDirty(leader.Country);
                }
            }

            // スキル
            if (items[(int) LeaderBatchItemId.Skill])
            {
                if (skill != leader.Skill)
                {
                    leader.Skill = skill;
                    leader.SetDirty(LeaderItemId.Skill);
                    Leaders.SetDirty(leader.Country);
                }
            }

            // 最大スキル
            if (items[(int) LeaderBatchItemId.MaxSkill])
            {
                if (maxSkill != leader.MaxSkill)
                {
                    leader.MaxSkill = maxSkill;
                    leader.SetDirty(LeaderItemId.MaxSkill);
                    Leaders.SetDirty(leader.Country);
                }
            }

            // 経験値
            if (items[(int) LeaderBatchItemId.Experience])
            {
                if (experience != leader.Experience)
                {
                    leader.Experience = experience;
                    leader.SetDirty(LeaderItemId.Experience);
                    Leaders.SetDirty(leader.Country);
                }
            }

            // 忠誠度
            if (items[(int) LeaderBatchItemId.Loyalty])
            {
                if (loyalty != leader.Loyalty)
                {
                    leader.Loyalty = loyalty;
                    leader.SetDirty(LeaderItemId.Loyalty);
                    Leaders.SetDirty(leader.Country);
                }
            }

            // 開始年
            if (items[(int) LeaderBatchItemId.StartYear])
            {
                if (startYear != leader.StartYear)
                {
                    leader.StartYear = startYear;
                    leader.SetDirty(LeaderItemId.StartYear);
                    Leaders.SetDirty(leader.Country);
                }
            }

            // 終了年
            if (items[(int) LeaderBatchItemId.EndYear])
            {
                if (endYear != leader.EndYear)
                {
                    leader.EndYear = endYear;
                    leader.SetDirty(LeaderItemId.EndYear);
                    Leaders.SetDirty(leader.Country);
                }
            }

            // 引退年
            if (items[(int) LeaderBatchItemId.RetirementYear])
            {
                if (retirementYear != leader.RetirementYear)
                {
                    leader.RetirementYear = retirementYear;
                    leader.SetDirty(LeaderItemId.RetirementYear);
                    Leaders.SetDirty(leader.Country);
                }
            }

            // 少将任官年
            if (items[(int) LeaderBatchItemId.Rank3Year])
            {
                if (rankYear[0] != leader.RankYear[0])
                {
                    leader.RankYear[0] = rankYear[0];
                    leader.SetDirty(LeaderItemId.Rank3Year);
                    Leaders.SetDirty(leader.Country);
                }
            }

            // 中将任官年
            if (items[(int) LeaderBatchItemId.Rank2Year])
            {
                if (rankYear[1] != leader.RankYear[1])
                {
                    leader.RankYear[1] = rankYear[1];
                    leader.SetDirty(LeaderItemId.Rank2Year);
                    Leaders.SetDirty(leader.Country);
                }
            }

            // 大将任官年
            if (items[(int) LeaderBatchItemId.Rank1Year])
            {
                if (rankYear[2] != leader.RankYear[2])
                {
                    leader.RankYear[2] = rankYear[2];
                    leader.SetDirty(LeaderItemId.Rank1Year);
                    Leaders.SetDirty(leader.Country);
                }
            }

            // 元帥任官年
            if (items[(int) LeaderBatchItemId.Rank0Year])
            {
                if (rankYear[3] != leader.RankYear[3])
                {
                    leader.RankYear[3] = rankYear[3];
                    leader.SetDirty(LeaderItemId.Rank0Year);
                    Leaders.SetDirty(leader.Country);
                }
            }
        }

        /// <summary>
        ///     一括編集処理のログ出力
        /// </summary>
        /// <param name="countries">対象国の文字列</param>
        /// <param name="items">一括編集項目</param>
        /// <param name="idealRank">理想階級</param>
        /// <param name="skill">スキル</param>
        /// <param name="maxSkill">最大スキル</param>
        /// <param name="experience">経験値</param>
        /// <param name="loyalty">忠誠度</param>
        /// <param name="startYear">開始年</param>
        /// <param name="endYear">終了年</param>
        /// <param name="retirementYear">引退年</param>
        /// <param name="rankYear">任官年</param>
        private static void LogBatchEdit(string countries, bool[] items, LeaderRank idealRank, int skill, int maxSkill,
            int experience, int loyalty, int startYear, int endYear, int retirementYear, int[] rankYear)
        {
            StringBuilder sb = new StringBuilder();

            if (items[(int) LeaderBatchItemId.IdealRank])
            {
                sb.AppendFormat(" ideal rank: {0}", Config.GetText(Leaders.RankNames[(int) idealRank]));
            }
            if (items[(int) LeaderBatchItemId.Skill])
            {
                sb.AppendFormat(" skill: {0}", skill);
            }
            if (items[(int) LeaderBatchItemId.MaxSkill])
            {
                sb.AppendFormat(" max skill: {0}", maxSkill);
            }
            if (items[(int) LeaderBatchItemId.Experience])
            {
                sb.AppendFormat(" experience: {0}", experience);
            }
            if (items[(int) LeaderBatchItemId.Loyalty])
            {
                sb.AppendFormat(" loyalty: {0}", loyalty);
            }
            if (items[(int) LeaderBatchItemId.StartYear])
            {
                sb.AppendFormat(" start year: {0}", startYear);
            }
            if (items[(int) LeaderBatchItemId.EndYear])
            {
                sb.AppendFormat(" end year: {0}", endYear);
            }
            if (items[(int) LeaderBatchItemId.RetirementYear])
            {
                sb.AppendFormat(" retirement year: {0}", retirementYear);
            }
            if (items[(int) LeaderBatchItemId.Rank3Year])
            {
                sb.AppendFormat(" rank3 year: {0}", rankYear[0]);
            }
            if (items[(int) LeaderBatchItemId.Rank2Year])
            {
                sb.AppendFormat(" rank2 year: {0}", rankYear[1]);
            }
            if (items[(int) LeaderBatchItemId.Rank1Year])
            {
                sb.AppendFormat(" rank1 year: {0}", rankYear[2]);
            }
            if (items[(int) LeaderBatchItemId.Rank0Year])
            {
                sb.AppendFormat(" rank0 year: {0}", rankYear[3]);
            }

            Log.Verbose("[Leader] Batch{0} ({1})", sb, countries);
        }

        #endregion
    }
}