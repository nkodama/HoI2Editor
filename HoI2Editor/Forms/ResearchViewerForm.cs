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

        #region 公開定数

        /// <summary>
        ///     技術リストビューの列の数
        /// </summary>
        public const int TechListColumnCount = 4;

        /// <summary>
        ///     研究機関リストビューの列の数
        /// </summary>
        public const int TeamListColumnCount = 8;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public ResearchViewerForm()
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
            // 研究機関リストを絞り込む
            NarrowTeamList();

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
            switch (id)
            {
                case EditorItemId.TeamList:
                    Log.Verbose("[Research] Changed team list");
                    // 研究機関リストを絞り込む
                    NarrowTeamList();
                    // 研究機関リストを更新する
                    UpdateTeamList();
                    break;

                case EditorItemId.TeamCountry:
                    Log.Verbose("[Research] Changed team country");
                    // 研究機関リストを絞り込む
                    NarrowTeamList();
                    // 研究機関リストを更新する
                    UpdateTeamList();
                    break;

                case EditorItemId.TeamName:
                    Log.Verbose("[Research] Changed team name");
                    // 研究機関リストを更新する
                    UpdateTeamList();
                    break;

                case EditorItemId.TeamId:
                    Log.Verbose("[Research] Changed team id");
                    // 研究機関リストを更新する
                    UpdateTeamList();
                    break;

                case EditorItemId.TeamSkill:
                    Log.Verbose("[Research] Changed team skill");
                    // 研究機関リストを更新する
                    UpdateTeamList();
                    break;

                case EditorItemId.TeamSpeciality:
                    Log.Verbose("[Research] Changed team speciality");
                    // 研究機関リストを更新する
                    UpdateTeamList();
                    break;

                case EditorItemId.TechItemList:
                    Log.Verbose("[Research] Changed tech item list");
                    // 技術リストを更新する
                    UpdateTechList();
                    break;

                case EditorItemId.TechItemName:
                    Log.Verbose("[Research] Changed tech item name");
                    // 技術リストを更新する
                    UpdateTechList();
                    break;

                case EditorItemId.TechItemId:
                    Log.Verbose("[Research] Changed tech item id");
                    // 技術リストを更新する
                    UpdateTechList();
                    break;

                case EditorItemId.TechItemYear:
                    Log.Verbose("[Research] Changed tech item year");
                    // 技術リストを更新する
                    UpdateTechList();
                    break;

                case EditorItemId.TechComponentList:
                    Log.Verbose("[Research] Changed tech component list");
                    // 技術リストを更新する
                    UpdateTechList();
                    break;

                case EditorItemId.TechComponentSpeciality:
                    Log.Verbose("[Research] Changed tech component speciality");
                    // 技術リストを更新する
                    UpdateTechList();
                    break;

                case EditorItemId.TechComponentDifficulty:
                    Log.Verbose("[Research] Changed tech component difficulty");
                    // 技術リストを更新する
                    UpdateTechList();
                    break;

                case EditorItemId.TechComponentDoubleTime:
                    Log.Verbose("[Research] Changed tech component double time");
                    // 技術リストを更新する
                    UpdateTechList();
                    break;
            }
        }

        #endregion

        #region フォーム

        /// <summary>
        ///     フォームの初期化
        /// </summary>
        private void InitForm()
        {
            // 技術リストビュー
            techNameColumnHeader.Width = HoI2Editor.Settings.ResearchViewer.TechListColumnWidth[0];
            techIdColumnHeader.Width = HoI2Editor.Settings.ResearchViewer.TechListColumnWidth[1];
            techYearColumnHeader.Width = HoI2Editor.Settings.ResearchViewer.TechListColumnWidth[2];
            techComponentsColumnHeader.Width = HoI2Editor.Settings.ResearchViewer.TechListColumnWidth[3];

            // 国家リストボックス
            countryListBox.ColumnWidth = DeviceCaps.GetScaledWidth(countryListBox.ColumnWidth);
            countryListBox.ItemHeight = DeviceCaps.GetScaledHeight(countryListBox.ItemHeight);

            // 研究機関リストビュー
            teamRankColumnHeader.Width = HoI2Editor.Settings.ResearchViewer.TeamListColumnWidth[1];
            teamDaysColumnHeader.Width = HoI2Editor.Settings.ResearchViewer.TeamListColumnWidth[2];
            teamEndDateColumnHeader.Width = HoI2Editor.Settings.ResearchViewer.TeamListColumnWidth[3];
            teamNameColumnHeader.Width = HoI2Editor.Settings.ResearchViewer.TeamListColumnWidth[4];
            teamIdColumnHeader.Width = HoI2Editor.Settings.ResearchViewer.TeamListColumnWidth[5];
            teamSkillColumnHeader.Width = HoI2Editor.Settings.ResearchViewer.TeamListColumnWidth[6];
            teamSpecialityColumnHeader.Width = HoI2Editor.Settings.ResearchViewer.TeamListColumnWidth[7];

            // ウィンドウの位置
            Location = HoI2Editor.Settings.ResearchViewer.Location;
            Size = HoI2Editor.Settings.ResearchViewer.Size;
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

            // 研究特性を初期化する
            Techs.InitSpecialities();

            // ゲーム設定ファイルを読み込む
            Misc.Load();

            // 文字列定義ファイルを読み込む
            Config.Load();

            // 技術リストビューの高さを設定するためにダミーのイメージリストを作成する
            techListView.SmallImageList = new ImageList { ImageSize = new Size(1, DeviceCaps.GetScaledHeight(18)) };

            // 研究機関リストビューの高さを設定するためにダミーのイメージリストを作成する
            teamListView.SmallImageList = new ImageList { ImageSize = new Size(1, DeviceCaps.GetScaledHeight(18)) };

            // 研究特性オーバーレイアイコンを初期化する
            _techOverlayIcon = new Bitmap(Game.GetReadFileName(Game.TechIconOverlayPathName));
            _techOverlayIcon.MakeTransparent(Color.Lime);

            // オプション項目を初期化する
            InitOptionItems();

            // 技術定義ファイルを読み込む
            Techs.Load();

            // 研究機関ファイルを読み込む
            Teams.Load();

            // 国家リストボックスを初期化する
            InitCountryListBox();

            // データ読み込み後の処理
            OnFileLoaded();
        }

        /// <summary>
        ///     フォームクローズ後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            HoI2Editor.OnResearchViewerFormClosed();
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
                HoI2Editor.Settings.ResearchViewer.Location = Location;
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
                HoI2Editor.Settings.ResearchViewer.Size = Size;
            }
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

            // 選択中のカテゴリを反映する
            int index = HoI2Editor.Settings.ResearchViewer.Category;
            if ((index < 0) || (index >= categoryListBox.Items.Count))
            {
                index = 0;
            }
            categoryListBox.SelectedIndex = index;
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

            // 選択中のカテゴリを保存する
            HoI2Editor.Settings.ResearchViewer.Category = categoryListBox.SelectedIndex;
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
                if ((component.Speciality != TechSpeciality.None) &&
                    ((int) component.Speciality - 1 < Techs.SpecialityImages.Images.Count))
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

        /// <summary>
        ///     技術リストビューの列の幅変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechListViewColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if ((e.ColumnIndex >= 0) && (e.ColumnIndex < TechListColumnCount))
            {
                HoI2Editor.Settings.ResearchViewer.TechListColumnWidth[e.ColumnIndex] =
                    techListView.Columns[e.ColumnIndex].Width;
            }
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
            foreach (Country country in HoI2Editor.Settings.ResearchViewer.Countries)
            {
                int index = Array.IndexOf(Countries.Tags, country);
                if (index >= 0)
                {
                    countryListBox.SetSelected(Array.IndexOf(Countries.Tags, country), true);
                }
            }
            // 選択イベントを元に戻す
            countryListBox.SelectedIndexChanged += OnCountryListBoxSelectedIndexChanged;

            countryListBox.EndUpdate();
        }

        /// <summary>
        ///     国家リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択中の国家を保存する
            HoI2Editor.Settings.ResearchViewer.Countries =
                countryListBox.SelectedIndices.Cast<int>().Select(index => Countries.Tags[index]).ToList();

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
            item.SubItems.Add(research.EndDate.ToString());
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
                case 7: // 研究特性
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
                if ((int) research.Team.Specialities[i] - 1 < Techs.SpecialityImages.Images.Count)
                {
                    e.Graphics.DrawImage(
                        Techs.SpecialityImages.Images[
                            Array.IndexOf(Techs.Specialities, research.Team.Specialities[i]) - 1], rect);
                }

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

        /// <summary>
        ///     研究機関リストビューの列の幅変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTeamListViewColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if ((e.ColumnIndex >= 0) && (e.ColumnIndex < TeamListColumnCount))
            {
                HoI2Editor.Settings.ResearchViewer.TeamListColumnWidth[e.ColumnIndex] =
                    teamListView.Columns[e.ColumnIndex].Width;
            }
        }

        #endregion

        #region オプション項目

        /// <summary>
        ///     オプション項目を初期化する
        /// </summary>
        private void InitOptionItems()
        {
            if (Researches.DateMode == ResearchDateMode.Historical)
            {
                historicalRadioButton.Checked = true;
                yearNumericUpDown.Enabled = false;
                monthNumericUpDown.Enabled = false;
                dayNumericUpDown.Enabled = false;
            }
            else
            {
                specifiedRadioButton.Checked = true;
                yearNumericUpDown.Enabled = true;
                monthNumericUpDown.Enabled = true;
                dayNumericUpDown.Enabled = true;
            }
            yearNumericUpDown.Value = Researches.SpecifiedDate.Year;
            monthNumericUpDown.Value = Researches.SpecifiedDate.Month;
            dayNumericUpDown.Value = Researches.SpecifiedDate.Day;
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
            Researches.DateMode = historicalRadioButton.Checked
                ? ResearchDateMode.Historical
                : ResearchDateMode.Specified;

            // 指定日付を使用の時だけ日付の編集を許可する
            bool flag = (Researches.DateMode == ResearchDateMode.Specified);
            yearNumericUpDown.Enabled = flag;
            monthNumericUpDown.Enabled = flag;
            dayNumericUpDown.Enabled = flag;

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
            Researches.SpecifiedDate.Year = (int) yearNumericUpDown.Value;

            // 研究機関リストを更新する
            UpdateTeamList();
        }

        /// <summary>
        ///     指定月変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMonthNumericUpDownValueChanged(object sender, EventArgs e)
        {
            Researches.SpecifiedDate.Month = (int) monthNumericUpDown.Value;

            // 研究機関リストを更新する
            UpdateTeamList();
        }

        /// <summary>
        ///     指定日変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDayNumericUpDownValueChanged(object sender, EventArgs e)
        {
            Researches.SpecifiedDate.Day = (int) dayNumericUpDown.Value;

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

            // 0以下の値だとまともに計算できなくなるので保険
            if (modifier <= 0.00005)
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