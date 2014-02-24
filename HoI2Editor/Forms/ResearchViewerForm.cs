using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using HoI2Editor.Models;
using HoI2Editor.Utilities;

namespace HoI2Editor.Forms
{
    /// <summary>
    ///     研究速度ビューアのフォーム
    /// </summary>
    public partial class ResearchViewerForm : Form
    {
        #region 内部フィールド

        /// <summary>
        ///     絞り込み後の研究機関リスト
        /// </summary>
        private readonly List<Team> _teamList = new List<Team>();

        /// <summary>
        ///     絞り込み後の技術リスト
        /// </summary>
        private readonly List<TechItem> _techList = new List<TechItem>();

        /// <summary>
        ///     研究特性オーバーレイアイコン
        /// </summary>
        private Bitmap _techOverlayIcon;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public ResearchViewerForm()
        {
            InitializeComponent();

            // 自動スケーリングを考慮した初期化
            InitScaling();
        }

        /// <summary>
        ///     自動スケーリングを考慮した初期化
        /// </summary>
        private void InitScaling()
        {
            // 技術リストビュー
            techNameColumnHeader.Width = DeviceCaps.GetScaledWidth(techNameColumnHeader.Width);
            techIdColumnHeader.Width = DeviceCaps.GetScaledWidth(techIdColumnHeader.Width);
            techYearColumnHeader.Width = DeviceCaps.GetScaledWidth(techYearColumnHeader.Width);
            techComponentsColumnHeader.Width = DeviceCaps.GetScaledWidth(techComponentsColumnHeader.Width);

            // 国家リストボックス
            countryListBox.ColumnWidth = DeviceCaps.GetScaledWidth(countryListBox.ColumnWidth);
            countryListBox.ItemHeight = DeviceCaps.GetScaledHeight(countryListBox.ItemHeight);

            // 研究機関リストビュー
            teamNameColumnHeader.Width = DeviceCaps.GetScaledWidth(teamNameColumnHeader.Width);
            teamIdColumnHeader.Width = DeviceCaps.GetScaledWidth(teamIdColumnHeader.Width);
            teamSkillColumnHeader.Width = DeviceCaps.GetScaledWidth(teamSkillColumnHeader.Width);
            teamSpecialityColumnHeader.Width = DeviceCaps.GetScaledWidth(teamSpecialityColumnHeader.Width);
            teamRankColumnHeader.Width = DeviceCaps.GetScaledWidth(teamRankColumnHeader.Width);
            teamDaysColumnHeader.Width = DeviceCaps.GetScaledWidth(teamDaysColumnHeader.Width);
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResearchViewerFormLoad(object sender, EventArgs e)
        {
            // 国家データを初期化する
            Countries.Init();

            // 研究特性を初期化する
            Techs.InitSpecialities();

            // ゲーム設定ファイルを読み込む
            Misc.Load();

            // 文字列定義ファイルを読み込む
            Config.Load();

            // 技術リストビューの高さを設定するためにダミーのイメージリストを作成する
            techListView.SmallImageList = new ImageList {ImageSize = new Size(1, DeviceCaps.GetScaledHeight(18))};

            // 研究機関リストビューの高さを設定するためにダミーのイメージリストを作成する
            teamListView.SmallImageList = new ImageList {ImageSize = new Size(1, DeviceCaps.GetScaledHeight(18))};

            // 研究特性オーバーレイアイコンを初期化する
            InitOverlayIcon();

            // オプション項目を初期化する
            InitOptionItems();

            // 国家リストボックスを初期化する
            InitCountryListBox();

            // 技術定義ファイルを読み込む
            Techs.Load();

            // 研究機関ファイルを読み込む
            Teams.Load();

            // データ読み込み後の処理
            OnFileLoaded();
        }

        /// <summary>
        ///     研究特性オーバーレイアイコンを初期化する
        /// </summary>
        private void InitOverlayIcon()
        {
            _techOverlayIcon = new Bitmap(Game.GetReadFileName(Game.TechIconOverlayPathName));
            _techOverlayIcon.MakeTransparent(Color.Lime);
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
        ///     フォームクローズ後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResearchViewerFormClosed(object sender, FormClosedEventArgs e)
        {
            HoI2Editor.OnResearchViewerFormClosed();
        }

        #endregion

        #region データ処理

        /// <summary>
        ///     データ読み込み後の処理
        /// </summary>
        public void OnFileLoaded()
        {
            // カテゴリリストボックスを初期化する
            InitCategoryList();

            // 技術リストの表示を更新する
            UpdateTechList();

            // 編集済みフラグがクリアされるため表示を更新する
            countryListBox.Refresh();
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

        #region 技術カテゴリリスト

        /// <summary>
        ///     カテゴリリストボックスを初期化する
        /// </summary>
        private void InitCategoryList()
        {
            categoryListBox.Items.Clear();
            foreach (TechGroup grp in Techs.Groups)
            {
                categoryListBox.Items.Add(grp);
            }
            categoryListBox.SelectedIndex = 0;
        }

        /// <summary>
        ///     カテゴリリストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCategoryListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 技術リストを更新する
            UpdateTechList();
        }

        #endregion

        #region 技術リストビュー

        /// <summary>
        ///     技術リストの表示を更新する
        /// </summary>
        private void UpdateTechList()
        {
            // 技術リストを絞り込む
            _techList.Clear();
            _techList.AddRange(categoryListBox.SelectedIndices.Cast<int>()
                .SelectMany(index => Techs.Groups[index].Items.Where(item => item is TechItem).Cast<TechItem>()));

            techListView.BeginUpdate();
            techListView.Items.Clear();

            // 項目を順に登録する
            foreach (TechItem tech in _techList)
            {
                techListView.Items.Add(CreateTechListViewItem(tech));
            }

            if (techListView.Items.Count > 0)
            {
                // 先頭の項目を選択する
                techListView.Items[0].Focused = true;
                techListView.Items[0].Selected = true;
            }

            techListView.EndUpdate();
        }

        /// <summary>
        ///     技術リストビューの項目を作成する
        /// </summary>
        /// <param name="tech">技術データ</param>
        /// <returns>技術リストビューの項目</returns>
        private static ListViewItem CreateTechListViewItem(TechItem tech)
        {
            if (tech == null)
            {
                return null;
            }

            var item = new ListViewItem
            {
                Text = Config.GetText(tech.Name),
                Tag = tech
            };
            item.SubItems.Add(tech.Id.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(tech.Year.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add("");

            return item;
        }

        /// <summary>
        ///     技術リストビューのサブ項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechListViewDrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case 3: // 小研究
                    e.Graphics.FillRectangle(
                        techListView.SelectedIndices.Count > 0 && e.ItemIndex == techListView.SelectedIndices[0]
                            ? (techListView.Focused ? SystemBrushes.Highlight : SystemBrushes.Control)
                            : SystemBrushes.Window, e.Bounds);
                    var tech = techListView.Items[e.ItemIndex].Tag as TechItem;
                    if (tech == null)
                    {
                        break;
                    }
                    DrawTechSpecialityItems(e, tech);
                    break;

                default:
                    e.DrawDefault = true;
                    break;
            }
        }

        /// <summary>
        ///     技術リストビューの研究特性項目描画処理
        /// </summary>
        /// <param name="e"></param>
        /// <param name="tech">技術項目</param>
        private void DrawTechSpecialityItems(DrawListViewSubItemEventArgs e, TechItem tech)
        {
            if (tech == null)
            {
                e.DrawDefault = true;
                return;
            }

            var gr = new Rectangle(e.Bounds.X + 4, e.Bounds.Y + 1, DeviceCaps.GetScaledWidth(16),
                DeviceCaps.GetScaledHeight(16));
            var tr = new Rectangle(e.Bounds.X + DeviceCaps.GetScaledWidth(16) + 3, e.Bounds.Y + 3,
                e.Bounds.Width - DeviceCaps.GetScaledWidth(16) - 3, e.Bounds.Height);
            Brush brush = new SolidBrush(
                ((techListView.SelectedIndices.Count > 0) && (e.ItemIndex == techListView.SelectedIndices[0]))
                    ? (techListView.Focused ? SystemColors.HighlightText : SystemColors.ControlText)
                    : SystemColors.WindowText);

            foreach (TechComponent component in tech.Components)
            {
                // 研究特性アイコンを描画する
                if (component.Speciality != TechSpeciality.None)
                {
                    e.Graphics.DrawImage(
                        Techs.SpecialityImages.Images[Array.IndexOf(Techs.Specialities, component.Speciality) - 1], gr);
                }

                // 研究難易度を描画する
                e.Graphics.DrawString(component.Difficulty.ToString(CultureInfo.InvariantCulture), techListView.Font,
                    brush, tr);

                // 次の項目の開始位置を計算する
                int offset = DeviceCaps.GetScaledWidth(32);
                gr.X += offset;
                tr.X += offset;
            }

            brush.Dispose();
        }

        /// <summary>
        ///     研究機関リストビューの列ヘッダ描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechListViewDrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        /// <summary>
        ///     技術リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (techListView.SelectedIndices.Count == 0)
            {
                return;
            }

            // 研究機関リストを更新する
            UpdateTeamList();
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
        ///     国家リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 研究機関リストを更新する
            NarrowTeamList();
            UpdateTeamList();
        }

        #endregion

        #region 研究機関リスト

        /// <summary>
        ///     研究機関リストを更新する
        /// </summary>
        private void UpdateTeamList()
        {
            // 技術リストビューの選択項目がなければ研究機関リストをクリアする
            if (techListView.SelectedItems.Count == 0)
            {
                teamListView.BeginUpdate();
                teamListView.Items.Clear();
                teamListView.EndUpdate();
                return;
            }

            var tech = techListView.SelectedItems[0].Tag as TechItem;
            if (tech == null)
            {
                return;
            }

            // 研究速度リストを更新する
            Researches.UpdateResearchList(tech, _teamList);

            teamListView.BeginUpdate();
            teamListView.Items.Clear();

            // 項目を順に登録する
            int rank = 1;
            foreach (Research research in Researches.Items)
            {
                teamListView.Items.Add(CreateTeamListViewItem(research, rank));
                rank++;
            }

            teamListView.EndUpdate();
        }

        /// <summary>
        ///     研究機関リストを絞り込む
        /// </summary>
        private void NarrowTeamList()
        {
            _teamList.Clear();

            // 選択中の国家リストを作成する
            List<Country> tags =
                countryListBox.SelectedItems.Cast<string>().Select(s => Countries.StringMap[s]).ToList();

            // 選択中の国家に所属する研究機関を順に絞り込む
            _teamList.AddRange(Teams.Items.Where(team => tags.Contains(team.Country)));
        }

        /// <summary>
        ///     研究機関リストビューの項目を作成する
        /// </summary>
        /// <param name="research">研究速度データ</param>
        /// <param name="rank">研究速度順位</param>
        /// <returns>研究機関リストビューの項目</returns>
        private ListViewItem CreateTeamListViewItem(Research research, int rank)
        {
            if (research == null)
            {
                return null;
            }

            var item = new ListViewItem
            {
                Tag = research
            };
            item.SubItems.Add(rank.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(research.Days.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(research.Team.Name);
            item.SubItems.Add(research.Team.Id.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(research.Team.Skill.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add("");

            return item;
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
                    var research = teamListView.Items[e.ItemIndex].Tag as Research;
                    if (research == null)
                    {
                        break;
                    }
                    DrawTeamSpecialityIcon(e, research);
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
        /// <param name="research">研究速度データ</param>
        private void DrawTeamSpecialityIcon(DrawListViewSubItemEventArgs e, Research research)
        {
            if (research == null)
            {
                return;
            }

            var rect = new Rectangle(e.Bounds.X + 4, e.Bounds.Y + 1, DeviceCaps.GetScaledWidth(16),
                DeviceCaps.GetScaledHeight(16));
            for (int i = 0; i < Team.SpecialityLength; i++)
            {
                // 研究特性なしならば何もしない
                if (research.Team.Specialities[i] == TechSpeciality.None)
                {
                    continue;
                }

                // 研究特性アイコンを描画する
                e.Graphics.DrawImage(
                    Techs.SpecialityImages.Images[Array.IndexOf(Techs.Specialities, research.Team.Specialities[i]) - 1],
                    rect);

                // 研究特性オーバーレイアイコンを描画する
                if (research.Tech.Components.Any(component => component.Speciality == research.Team.Specialities[i]))
                {
                    e.Graphics.DrawImage(_techOverlayIcon, rect);
                }

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

        #endregion

        #region オプション項目

        /// <summary>
        ///     オプション項目を初期化する
        /// </summary>
        private void InitOptionItems()
        {
            if (Researches.YearMode == ResearchYearMode.Historical)
            {
                historicalRadioButton.Checked = true;
                yearNumericUpDown.Enabled = false;
            }
            else
            {
                specifiedRadioButton.Checked = true;
                yearNumericUpDown.Enabled = true;
            }
            yearNumericUpDown.Value = Researches.SpecifiedYear;
            rocketNumericUpDown.Value = Researches.RocketTestingSites;
            nuclearNumericUpDown.Value = Researches.NuclearReactors;
            blueprintCheckBox.Checked = Researches.Blueprint;
            modifierTextBox.Text = Researches.Modifier.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     史実年度を使用チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHistoricalRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            // 値を更新する
            Researches.YearMode = historicalRadioButton.Checked
                ? ResearchYearMode.Historical
                : ResearchYearMode.Specified;

            // 指定年度を使用の時だけ年度の編集を許可する
            yearNumericUpDown.Enabled = (Researches.YearMode == ResearchYearMode.Specified);

            // 研究機関リストを更新する
            UpdateTeamList();
        }

        /// <summary>
        ///     指定年度変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnYearNumericUpDownValueChanged(object sender, EventArgs e)
        {
            Researches.SpecifiedYear = (int) yearNumericUpDown.Value;

            // 研究機関リストを更新する
            UpdateTeamList();
        }

        /// <summary>
        ///     ロケット試験場の規模変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRocketNumericUpDownValueChanged(object sender, EventArgs e)
        {
            Researches.RocketTestingSites = (int) rocketNumericUpDown.Value;

            // 研究機関リストを更新する
            UpdateTeamList();
        }

        /// <summary>
        ///     原子炉の規模変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNuclearNumericUpDownValueChanged(object sender, EventArgs e)
        {
            Researches.NuclearReactors = (int) nuclearNumericUpDown.Value;

            // 研究機関リストを更新する
            UpdateTeamList();
        }

        /// <summary>
        ///     青写真チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBlueprintCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            Researches.Blueprint = blueprintCheckBox.Checked;

            // 研究機関リストを更新する
            UpdateTeamList();
        }

        /// <summary>
        ///     研究速度補正の値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnModifierTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double modifier;
            if (!double.TryParse(modifierTextBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out modifier))
            {
                modifierTextBox.Text = Researches.Modifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(modifier - Researches.Modifier) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Researches.Modifier = modifier;

            // 研究機関リストを更新する
            UpdateTeamList();
        }

        #endregion
    }
}