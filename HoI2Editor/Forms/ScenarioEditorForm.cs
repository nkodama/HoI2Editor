using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HoI2Editor.Controls;
using HoI2Editor.Models;
using HoI2Editor.Properties;
using HoI2Editor.Utilities;

namespace HoI2Editor.Forms
{
    /// <summary>
    ///     シナリオエディタのフォーム
    /// </summary>
    public partial class ScenarioEditorForm : Form
    {
        #region 内部フィールド

        #region 選択可能国リスト

        /// <summary>
        ///     主要国以外の選択可能国リスト
        /// </summary>
        private List<Country> _majorFreeCountries;

        /// <summary>
        ///     選択可能国以外の国家リスト
        /// </summary>
        private List<Country> _selectableFreeCountries;

        #endregion

        #region 同盟国リスト

        /// <summary>
        ///     同盟国以外の国家リスト
        /// </summary>
        private List<Country> _allianceFreeCountries;

        /// <summary>
        ///     戦争国以外の国家リスト
        /// </summary>
        private List<Country> _warFreeCountries;

        #endregion

        #region 閣僚候補リスト

        /// <summary>
        ///     国家元首リスト
        /// </summary>
        private List<Minister> _headOfStateList;

        /// <summary>
        ///     政府首班リスト
        /// </summary>
        private List<Minister> _headOfGovernmentList;

        /// <summary>
        ///     外務大臣リスト
        /// </summary>
        private List<Minister> _foreignMinisterList;

        /// <summary>
        ///     軍需大臣リスト
        /// </summary>
        private List<Minister> _armamentMinisterList;

        /// <summary>
        ///     内務大臣リスト
        /// </summary>
        private List<Minister> _ministerOfSecurityList;

        /// <summary>
        ///     情報大臣リスト
        /// </summary>
        private List<Minister> _ministerOfIntelligenceList;

        /// <summary>
        ///     統合参謀総長リスト
        /// </summary>
        private List<Minister> _chiefOfStaffList;

        /// <summary>
        ///     陸軍総司令官リスト
        /// </summary>
        private List<Minister> _chiefOfArmyList;

        /// <summary>
        ///     海軍総司令官リスト
        /// </summary>
        private List<Minister> _chiefOfNavyList;

        /// <summary>
        ///     空軍総司令官リスト
        /// </summary>
        private List<Minister> _chiefOfAirList;

        #endregion

        #region 技術リスト

        /// <summary>
        ///     技術項目リスト
        /// </summary>
        private List<TechItem> _techs;

        /// <summary>
        ///     発明イベントリスト
        /// </summary>
        private List<TechEvent> _inventions;

        #endregion

        #region 技術ツリー

        /// <summary>
        ///     技術ツリーパネル
        /// </summary>
        private TechTreePanel _techTreePanel;

        #endregion

        #region データ遅延読み込み

        /// <summary>
        ///     閣僚データロード用
        /// </summary>
        private readonly BackgroundWorker _ministerWorker = new BackgroundWorker();

        /// <summary>
        ///     技術データロード用
        /// </summary>
        private readonly BackgroundWorker _techWorker = new BackgroundWorker();

        /// <summary>
        ///     マップデータロード用
        /// </summary>
        private readonly BackgroundWorker _mapWorker = new BackgroundWorker();

        #endregion

        private ushort _prevId;

        #endregion

        #region 内部定数

        /// <summary>
        ///     AIの攻撃性の文字列
        /// </summary>
        private readonly TextId[] _aiAggressiveStrings =
        {
            TextId.OptionAiAggressiveness1,
            TextId.OptionAiAggressiveness2,
            TextId.OptionAiAggressiveness3,
            TextId.OptionAiAggressiveness4,
            TextId.OptionAiAggressiveness5
        };

        /// <summary>
        ///     難易度の文字列
        /// </summary>
        private readonly TextId[] _difficultyStrings =
        {
            TextId.OptionDifficulty1,
            TextId.OptionDifficulty2,
            TextId.OptionDifficulty3,
            TextId.OptionDifficulty4,
            TextId.OptionDifficulty5
        };

        /// <summary>
        ///     ゲームスピードの文字列
        /// </summary>
        private readonly TextId[] _gameSpeedStrings =
        {
            TextId.OptionGameSpeed0,
            TextId.OptionGameSpeed1,
            TextId.OptionGameSpeed2,
            TextId.OptionGameSpeed3,
            TextId.OptionGameSpeed4,
            TextId.OptionGameSpeed5,
            TextId.OptionGameSpeed6,
            TextId.OptionGameSpeed7
        };

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public ScenarioEditorForm()
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
            // 国家関係を初期化する
            Scenarios.Init();

            // 編集項目を更新する
            UpdateEditableItems();
        }

        /// <summary>
        ///     データ保存後の処理
        /// </summary>
        public void OnFileSaved()
        {
            // 編集済みフラグがクリアされるため表示を更新する
        }

        /// <summary>
        ///     編集項目変更後の処理
        /// </summary>
        /// <param name="id">編集項目ID</param>
        public void OnItemChanged(EditorItemId id)
        {
            // 何もしない
        }

        /// <summary>
        ///     閣僚データを遅延読み込みする
        /// </summary>
        private void LoadMinisters()
        {
            _ministerWorker.DoWork += OnMinisterWorkerDoWork;
            _ministerWorker.RunWorkerCompleted += OnMinisterWorkerRunWorkerCompleted;
            _ministerWorker.RunWorkerAsync();
        }

        /// <summary>
        ///     閣僚データの読み込み完了まで待機する
        /// </summary>
        private void WaitLoadingMinisters()
        {
            while (_ministerWorker.IsBusy)
            {
                Application.DoEvents();
            }
        }

        /// <summary>
        ///     閣僚データを読み込む
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            // 閣僚データを読み込む
            Ministers.Load();

            Log.Info("[Scenario] Load ministers");
        }

        /// <summary>
        ///     閣僚データ読み込み完了時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }

            if (e.Cancelled)
            {
                return;
            }

            // 政府タブの編集項目を初期化する
            InitGovernmentTab();
        }

        /// <summary>
        ///     技術データを遅延読み込みする
        /// </summary>
        private void LoadTechs()
        {
            _techWorker.DoWork += OnTechWorkerDoWork;
            _techWorker.RunWorkerCompleted += OnTechWorkerRunWorkerCompleted;
            _techWorker.RunWorkerAsync();
        }

        /// <summary>
        ///     技術データの読み込み完了まで待機する
        /// </summary>
        private void WaitLoadingTechs()
        {
            while (_techWorker.IsBusy)
            {
                Application.DoEvents();
            }
        }

        /// <summary>
        ///     技術データを読み込む
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            // 技術定義ファイルを読み込む
            Techs.Load();

            Log.Info("[Scenario] Load techs");
        }

        /// <summary>
        ///     技術データ読み込み完了時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }

            if (e.Cancelled)
            {
                return;
            }

            // 技術タブの編集項目を初期化する
            InitTechTab();
        }

        /// <summary>
        ///     マップを遅延読み込みする
        /// </summary>
        private void LoadMaps()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += OnMapWorkerDoWork;
            worker.RunWorkerCompleted += OnMapWorkerRunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        /// <summary>
        ///     マップの読み込み完了まで待機する
        /// </summary>
        private void WaitLoadingMaps()
        {
            while (_mapWorker.IsBusy)
            {
                Application.DoEvents();
            }
        }

        /// <summary>
        ///     マップを読み込む
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMapWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            Maps.Load(MapLevel.Level4);

            Log.Info("[Scenario] Load level-4 map");
            
            Maps.Load(MapLevel.Level2);

            Log.Info("[Scenario] Load level-2 map");
        }

        /// <summary>
        ///     マップ読み込み完了時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMapWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }

            if (e.Cancelled)
            {
                return;
            }

            Map map = Maps.Data[(int) MapLevel.Level2];
            Bitmap bitmap = map.GetImage();
            map.SetMaskColor(bitmap, Color.LightSteelBlue);
            provinceMapPictureBox.Image = bitmap;
        }

        #endregion

        #region フォーム

        /// <summary>
        ///     フォームの初期化
        /// </summary>
        private void InitForm()
        {
            // ウィンドウの位置
            Location = HoI2Editor.Settings.ScenarioEditor.Location;
            Size = HoI2Editor.Settings.ScenarioEditor.Size;

            // 技術ツリーパネル
            _techTreePanel = new TechTreePanel
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Location = new Point(404, 12),
                Size = new Size(560, 535),
                ApplyItemStatus = true
            };
            _techTreePanel.ItemMouseClick += OnTechTreeItemMouseClick;
            _techTreePanel.QueryItemStatus += OnQueryTechTreeItemStatus;
            technologyTabPage.Controls.Add(_techTreePanel);
        }

        /// <summary>
        ///     編集項目を初期化する
        /// </summary>
        private void InitEditableItems()
        {
            InitMainTab();
            InitAllianceTab();
            InitRelationTab();
            InitTradeTab();
            InitCountryTab();
            //InitGovernmentTab();
            //InitTechTab();
        }

        /// <summary>
        ///     編集項目を更新する
        /// </summary>
        private void UpdateEditableItems()
        {
            UpdateMainTab();
            UpdateAllianceTab();
            UpdateRelationTab();
            UpdateTradeTab();
            UpdateCountryTab();
            UpdateGovernmentTab();
            UpdateTechTab();
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormLoad(object sender, EventArgs e)
        {
            // マップを遅延読み込みする
            LoadMaps();

            // 国家データを初期化する
            Countries.Init();

            // 閣僚特性を初期化する
            Ministers.InitPersonality();

            // ユニットデータを初期化する
            Units.Init();

            // ゲーム設定ファイルを読み込む
            Misc.Load();

            // 文字列定義ファイルを読み込む
            Config.Load();

            // 閣僚データを遅延読込する
            LoadMinisters();

            // 技術データを遅延読み込みする
            LoadTechs();

            // 表示項目を初期化する
            InitEditableItems();

            // シナリオファイル読み込み済みなら編集項目を更新する
            if (Scenarios.IsLoaded())
            {
                OnFileLoaded();
            }
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
            HoI2Editor.OnScenarioEditorFormClosed();
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
                HoI2Editor.Settings.ScenarioEditor.Location = Location;
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
                HoI2Editor.Settings.ScenarioEditor.Size = Size;
            }
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

        /// <summary>
        ///     選択タブ変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnScenarioTabControlSelectedIndexChanged(object sender, EventArgs e)
        {
            switch (scenarioTabControl.SelectedIndex)
            {
                case 5: // 政府タブ
                    OnGovernmentTabSelected();
                    break;

                case 6: // 技術タブ
                    OnTechTabPageSelected();
                    break;
            }
        }

        #endregion

        #region メインタブ

        #region メインタブ - 共通

        /// <summary>
        ///     メインタブの編集項目を初期化する
        /// </summary>
        private void InitMainTab()
        {
            // シナリオリストボックス
            InitScenarioListBox();

            // AIの攻撃性コンボボックス
            aiAggressiveComboBox.BeginUpdate();
            aiAggressiveComboBox.Items.Clear();
            for (int i = 0; i < ScenarioHeader.AiAggressiveCount; i++)
            {
                aiAggressiveComboBox.Items.Add(Config.GetText(_aiAggressiveStrings[i]));
            }
            aiAggressiveComboBox.EndUpdate();

            // 難易度コンボボックス
            difficultyComboBox.BeginUpdate();
            difficultyComboBox.Items.Clear();
            for (int i = 0; i < ScenarioHeader.DifficultyCount; i++)
            {
                difficultyComboBox.Items.Add(Config.GetText(_difficultyStrings[i]));
            }
            difficultyComboBox.EndUpdate();

            // ゲームスピードコンボボックス
            gameSpeedComboBox.BeginUpdate();
            gameSpeedComboBox.Items.Clear();
            for (int i = 0; i < ScenarioHeader.GameSpeedCount; i++)
            {
                gameSpeedComboBox.Items.Add(Config.GetText(_gameSpeedStrings[i]));
            }
            gameSpeedComboBox.EndUpdate();
        }

        /// <summary>
        ///     メインタブの編集項目を更新する
        /// </summary>
        private void UpdateMainTab()
        {
            // 編集項目を更新する
            UpdateMainItems();
        }

        /// <summary>
        ///     メインタブの編集項目を更新する
        /// </summary>
        private void UpdateMainItems()
        {
            Scenario scenario = Scenarios.Data;
            ScenarioHeader header = scenario.Header;
            ScenarioGlobalData data = scenario.GlobalData;

            scenarioNameTextBox.Text = Config.GetText(scenario.Name);
            scenarioNameTextBox.ForeColor = scenario.IsDirty(ScenarioItemId.Name) ? Color.Red : SystemColors.WindowText;
            panelImageTextBox.Text = scenario.PanelName;
            panelImageTextBox.ForeColor = scenario.IsDirty(ScenarioItemId.PanelName)
                ? Color.Red
                : SystemColors.WindowText;
            UpdatePanelImage(scenario.PanelName);

            bool flag = (data.StartDate != null);
            startYearTextBox.Text = flag ? IntHelper.ToString(data.StartDate.Year) : "";
            startMonthTextBox.Text = flag ? IntHelper.ToString(data.StartDate.Month) : "";
            startDayTextBox.Text = flag ? IntHelper.ToString(data.StartDate.Day) : "";

            startYearTextBox.ForeColor = scenario.IsDirty(ScenarioItemId.StartYear)
                ? Color.Red
                : SystemColors.WindowText;
            startMonthTextBox.ForeColor = scenario.IsDirty(ScenarioItemId.StartMonth)
                ? Color.Red
                : SystemColors.WindowText;
            startDayTextBox.ForeColor = scenario.IsDirty(ScenarioItemId.StartDay) ? Color.Red : SystemColors.WindowText;

            flag = (data.EndDate != null);
            endYearTextBox.Text = flag ? IntHelper.ToString(data.EndDate.Year) : "";
            endMonthTextBox.Text = flag ? IntHelper.ToString(data.EndDate.Month) : "";
            endDayTextBox.Text = flag ? IntHelper.ToString(data.EndDate.Day) : "";

            endYearTextBox.ForeColor = scenario.IsDirty(ScenarioItemId.EndYear) ? Color.Red : SystemColors.WindowText;
            endMonthTextBox.ForeColor = scenario.IsDirty(ScenarioItemId.EndMonth) ? Color.Red : SystemColors.WindowText;
            endDayTextBox.ForeColor = scenario.IsDirty(ScenarioItemId.EndDay) ? Color.Red : SystemColors.WindowText;

            includeFolderTextBox.Text = scenario.IncludeFolder;
            includeFolderTextBox.ForeColor = scenario.IsDirty(ScenarioItemId.IncludeFolder)
                ? Color.Red
                : SystemColors.WindowText;

            battleScenarioCheckBox.Checked = header.IsCombatScenario;
            battleScenarioCheckBox.ForeColor = scenario.IsDirty(ScenarioItemId.BattleScenario)
                ? Color.Red
                : SystemColors.WindowText;
            freeCountryCheckBox.Checked = header.IsFreeSelection;
            freeCountryCheckBox.ForeColor = scenario.IsDirty(ScenarioItemId.FreeSelection)
                ? Color.Red
                : SystemColors.WindowText;

            flag = (data.Rules == null);
            allowDiplomacyCheckBox.Checked = (flag || data.Rules.AllowDiplomacy);
            allowDiplomacyCheckBox.ForeColor = scenario.IsDirty(ScenarioItemId.AllowDiplomacy)
                ? Color.Red
                : SystemColors.WindowText;
            allowProductionCheckBox.Checked = (flag || data.Rules.AllowProduction);
            allowProductionCheckBox.ForeColor = scenario.IsDirty(ScenarioItemId.AllowProduction)
                ? Color.Red
                : SystemColors.WindowText;
            allowTechnologyCheckBox.Checked = (flag || data.Rules.AllowTechnology);
            allowTechnologyCheckBox.ForeColor = scenario.IsDirty(ScenarioItemId.AllowTechnology)
                ? Color.Red
                : SystemColors.WindowText;

            aiAggressiveComboBox.SelectedIndex = header.AiAggressive;
            difficultyComboBox.SelectedIndex = header.Difficulty;
            gameSpeedComboBox.SelectedIndex = header.GameSpeed;

            List<Country> majors = header.MajorCountries.Select(major => major.Country).ToList();
            majorListBox.BeginUpdate();
            majorListBox.Items.Clear();
            foreach (Country country in majors)
            {
                majorListBox.Items.Add(Countries.GetTagName(country));
            }
            majorListBox.EndUpdate();

            _majorFreeCountries = header.SelectableCountries.Where(country => !majors.Contains(country)).ToList();
            selectableListBox.BeginUpdate();
            selectableListBox.Items.Clear();
            foreach (Country country in _majorFreeCountries)
            {
                selectableListBox.Items.Add(Countries.GetTagName(country));
            }
            selectableListBox.EndUpdate();

            _selectableFreeCountries =
                Countries.Tags.Where(country => !header.SelectableCountries.Contains(country)).ToList();
            unselectableListBox.BeginUpdate();
            unselectableListBox.Items.Clear();
            foreach (Country country in _selectableFreeCountries)
            {
                unselectableListBox.Items.Add(Countries.GetTagName(country));
            }
            unselectableListBox.EndUpdate();

            // 編集項目を有効化する
            infoGroupBox.Enabled = true;
            optionGroupBox.Enabled = true;
            countrySelectionGroupBox.Enabled = true;
        }

        #endregion

        #region メインタブ - シナリオ読み込み

        /// <summary>
        ///     シナリオリストボックスを初期化する
        /// </summary>
        private void InitScenarioListBox()
        {
            // フォルダグループボックスのどれかのラジオボタンを有効にする
            if (Game.IsExportFolderActive && Directory.Exists(Game.GetExportFileName(Game.ScenarioPathName)))
            {
                exportRadioButton.Checked = true;
            }
            else if (Game.IsModActive && Directory.Exists(Game.GetModFileName(Game.ScenarioPathName)))
            {
                modRadioButton.Checked = true;
            }
            else
            {
                vanillaRadioButton.Checked = true;
            }
        }

        /// <summary>
        ///     シナリオリストボックスの表示を更新する
        /// </summary>
        private void UpdateScenarioListBox()
        {
            scenarioListBox.Items.Clear();

            string folderName;
            if (exportRadioButton.Checked)
            {
                folderName = Game.GetExportFileName(Game.ScenarioPathName);
            }
            else if (modRadioButton.Checked)
            {
                folderName = Game.GetModFileName(Game.ScenarioPathName);
            }
            else
            {
                folderName = Game.GetVanillaFileName(Game.ScenarioPathName);
            }

            // シナリオフォルダがなければ戻る
            if (!Directory.Exists(folderName))
            {
                return;
            }

            // eugファイルを順に追加する
            string[] fileNames = Directory.GetFiles(folderName, "*.eug");
            foreach (string fileName in fileNames)
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    scenarioListBox.Items.Add(Path.GetFileName(fileName));
                }
            }

            // 先頭の項目を選択する
            if (scenarioListBox.Items.Count > 0)
            {
                scenarioListBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        ///     読み込みボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoadButtonClick(object sender, EventArgs e)
        {
            // シナリオリストボックスの選択項目がなければ何もしない
            if (scenarioListBox.SelectedIndex < 0)
            {
                return;
            }

            string fileName = scenarioListBox.Items[scenarioListBox.SelectedIndex].ToString();
            string pathName;
            if (exportRadioButton.Checked)
            {
                pathName = Game.GetExportFileName(Game.ScenarioPathName, fileName);
            }
            else if (modRadioButton.Checked)
            {
                pathName = Game.GetModFileName(Game.ScenarioPathName, fileName);
            }
            else
            {
                pathName = Game.GetVanillaFileName(Game.ScenarioPathName, fileName);
            }

            // シナリオファイルを読み込む
            if (File.Exists(pathName))
            {
                Scenarios.Load(pathName);
            }

            // データ読み込み後の処理
            OnFileLoaded();
        }

        /// <summary>
        ///     フォルダラジオボタンのチェック状態変化時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFolderRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            RadioButton button = sender as RadioButton;
            if (button != null && button.Checked)
            {
                UpdateScenarioListBox();
            }
        }

        #endregion

        #region メインタブ - シナリオ情報

        /// <summary>
        ///     シナリオ名テキストボックスの文字列変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnScenarioNameTextBoxTextChanged(object sender, EventArgs e)
        {
            // 値に変化がなければ何もしない
            Scenario scenario = Scenarios.Data;
            string val = scenarioNameTextBox.Text;
            string name = Config.ExistsKey(scenario.Name) ? Config.GetText(scenario.Name) : "";
            if (val.Equals(name))
            {
                return;
            }

            Log.Info("[Scenario] scenario name: {0} -> {1}", name, val);

            // 値を更新する
            Config.SetText(scenario.Name, val, Game.ScenarioTextFileName);

            // 編集済みフラグを設定する
            scenario.SetDirty(ScenarioItemId.Name);

            // 文字色を変更する
            scenarioNameTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     パネル画像名テキストボックスの文字列変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPanelImageTextBoxTextChanged(object sender, EventArgs e)
        {
            // 値に変化がなければ何もしない
            Scenario scenario = Scenarios.Data;
            string val = panelImageTextBox.Text;
            if (val.Equals(scenario.PanelName))
            {
                return;
            }

            Log.Info("[Scenario] panel image: {0} -> {1}", scenario.PanelName, val);

            // 値を更新する
            scenario.PanelName = val;

            // 編集済みフラグを設定する
            scenario.SetDirty(ScenarioItemId.PanelName);
            Scenarios.SetDirty();

            // 文字色を変更する
            panelImageTextBox.ForeColor = Color.Red;

            // パネル画像を更新する
            UpdatePanelImage(scenario.PanelName);
        }

        /// <summary>
        ///     パネル画像名参照ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPanelImageBrowseButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;

            OpenFileDialog dialog = new OpenFileDialog
            {
                InitialDirectory = Game.GetReadFileName(Game.ScenarioDataPathName),
                FileName = scenario.PanelName,
                Filter = Resources.OpenBitmapFileDialogFilter
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                panelImageTextBox.Text = Game.GetRelativePathName(dialog.FileName);
            }
        }

        /// <summary>
        ///     開始年テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStartYearTextBoxValidated(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            ScenarioGlobalData data = scenario.GlobalData;

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(startYearTextBox.Text, out val))
            {
                startYearTextBox.Text = (data.StartDate != null) ? IntHelper.ToString(data.StartDate.Year) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((data.StartDate != null) && (val == data.StartDate.Year))
            {
                return;
            }

            Log.Info("[Scenario] start year: {0} -> {1}",
                (data.StartDate != null) ? IntHelper.ToString(data.StartDate.Year) : "", val);

            if (data.StartDate == null)
            {
                data.StartDate = new GameDate();

                // 編集済みフラグを設定する
                scenario.SetDirty(ScenarioItemId.StartMonth);
                scenario.SetDirty(ScenarioItemId.StartDay);

                // 編集項目を更新する
                startMonthTextBox.Text = IntHelper.ToString(data.StartDate.Month);
                startDayTextBox.Text = IntHelper.ToString(data.StartDate.Day);

                // 文字色を変更する
                startMonthTextBox.ForeColor = Color.Red;
                startDayTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            data.StartDate.Year = val;

            // 編集済みフラグを設定する
            scenario.SetDirty(ScenarioItemId.StartYear);
            Scenarios.SetDirty();

            // 文字色を変更する
            startYearTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     開始月テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStartMonthTextBoxValidated(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            ScenarioGlobalData data = scenario.GlobalData;

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(startMonthTextBox.Text, out val))
            {
                startMonthTextBox.Text = (data.StartDate != null) ? IntHelper.ToString(data.StartDate.Month) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((data.StartDate != null) && (val == data.StartDate.Month))
            {
                return;
            }

            Log.Info("[Scenario] start month: {0} -> {1}",
                (data.StartDate != null) ? IntHelper.ToString(data.StartDate.Month) : "", val);

            if (data.StartDate == null)
            {
                data.StartDate = new GameDate();

                // 編集済みフラグを設定する
                scenario.SetDirty(ScenarioItemId.StartYear);
                scenario.SetDirty(ScenarioItemId.StartDay);

                // 編集項目を更新する
                startYearTextBox.Text = IntHelper.ToString(data.StartDate.Year);
                startDayTextBox.Text = IntHelper.ToString(data.StartDate.Day);

                // 文字色を変更する
                startYearTextBox.ForeColor = Color.Red;
                startDayTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            data.StartDate.Month = val;

            // 編集済みフラグを設定する
            scenario.SetDirty(ScenarioItemId.StartMonth);
            Scenarios.SetDirty();

            // 文字色を変更する
            startMonthTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     開始日テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStartDayTextBoxValidated(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            ScenarioGlobalData data = scenario.GlobalData;

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(startDayTextBox.Text, out val))
            {
                startDayTextBox.Text = (data.StartDate != null) ? IntHelper.ToString(data.StartDate.Day) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((data.StartDate != null) && (val == data.StartDate.Day))
            {
                return;
            }

            Log.Info("[Scenario] start day: {0} -> {1}",
                (data.StartDate != null) ? IntHelper.ToString(data.StartDate.Day) : "", val);

            if (data.StartDate == null)
            {
                data.StartDate = new GameDate();

                // 編集済みフラグを設定する
                scenario.SetDirty(ScenarioItemId.StartYear);
                scenario.SetDirty(ScenarioItemId.StartMonth);

                // 編集項目を更新する
                startYearTextBox.Text = IntHelper.ToString(data.StartDate.Year);
                startMonthTextBox.Text = IntHelper.ToString(data.StartDate.Month);

                // 文字色を変更する
                startYearTextBox.ForeColor = Color.Red;
                startMonthTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            data.StartDate.Day = val;

            // 編集済みフラグを設定する
            scenario.SetDirty(ScenarioItemId.StartDay);
            Scenarios.SetDirty();

            // 文字色を変更する
            startDayTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     終了年テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEndYearTextBoxValidated(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            ScenarioGlobalData data = scenario.GlobalData;

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(endYearTextBox.Text, out val))
            {
                endYearTextBox.Text = (data.EndDate != null) ? IntHelper.ToString(data.EndDate.Year) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((data.EndDate != null) && (val == data.EndDate.Year))
            {
                return;
            }

            Log.Info("[Scenario] end year: {0} -> {1}",
                (data.EndDate != null) ? IntHelper.ToString(data.EndDate.Year) : "", val);

            if (data.EndDate == null)
            {
                data.EndDate = new GameDate();

                // 編集済みフラグを設定する
                scenario.SetDirty(ScenarioItemId.EndMonth);
                scenario.SetDirty(ScenarioItemId.EndDay);

                // 編集項目を更新する
                endMonthTextBox.Text = IntHelper.ToString(data.EndDate.Month);
                endDayTextBox.Text = IntHelper.ToString(data.EndDate.Day);

                // 文字色を変更する
                endMonthTextBox.ForeColor = Color.Red;
                endDayTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            data.EndDate.Year = val;

            // 編集済みフラグを設定する
            scenario.SetDirty(ScenarioItemId.EndYear);
            Scenarios.SetDirty();

            // 文字色を変更する
            endYearTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     終了月テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEndMonthTextBoxValidated(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            ScenarioGlobalData data = scenario.GlobalData;

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(endMonthTextBox.Text, out val))
            {
                endMonthTextBox.Text = (data.EndDate != null) ? IntHelper.ToString(data.EndDate.Month) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((data.EndDate != null) && (val == data.EndDate.Month))
            {
                return;
            }

            Log.Info("[Scenario] end month: {0} -> {1}",
                (data.EndDate != null) ? IntHelper.ToString(data.EndDate.Month) : "", val);

            if (data.EndDate == null)
            {
                data.EndDate = new GameDate();

                // 編集済みフラグを設定する
                scenario.SetDirty(ScenarioItemId.EndYear);
                scenario.SetDirty(ScenarioItemId.EndDay);

                // 編集項目を更新する
                endYearTextBox.Text = IntHelper.ToString(data.EndDate.Year);
                endDayTextBox.Text = IntHelper.ToString(data.EndDate.Day);

                // 文字色を変更する
                endYearTextBox.ForeColor = Color.Red;
                endDayTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            data.EndDate.Month = val;

            // 編集済みフラグを設定する
            scenario.SetDirty(ScenarioItemId.EndMonth);
            Scenarios.SetDirty();

            // 文字色を変更する
            endMonthTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     終了日テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEndDayTextBoxValidated(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            ScenarioGlobalData data = scenario.GlobalData;

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(endDayTextBox.Text, out val))
            {
                endDayTextBox.Text = (data.EndDate != null) ? IntHelper.ToString(data.EndDate.Day) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((data.EndDate != null) && (val == data.EndDate.Day))
            {
                return;
            }

            Log.Info("[Scenario] end day: {0} -> {1}",
                (data.EndDate != null) ? IntHelper.ToString(data.EndDate.Day) : "", val);

            if (data.EndDate == null)
            {
                data.EndDate = new GameDate();

                // 編集済みフラグを設定する
                scenario.SetDirty(ScenarioItemId.EndYear);
                scenario.SetDirty(ScenarioItemId.EndMonth);

                // 編集項目を更新する
                endYearTextBox.Text = IntHelper.ToString(data.EndDate.Year);
                endMonthTextBox.Text = IntHelper.ToString(data.EndDate.Month);

                // 文字色を変更する
                endYearTextBox.ForeColor = Color.Red;
                endMonthTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            data.EndDate.Day = val;

            // 編集済みフラグを設定する
            scenario.SetDirty(ScenarioItemId.EndDay);
            Scenarios.SetDirty();

            // 文字色を変更する
            endDayTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     インクルードフォルダテキストボックスの文字列変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIncludeFolderTextBoxTextChanged(object sender, EventArgs e)
        {
            // 値に変化がなければ何もしない
            Scenario scenario = Scenarios.Data;
            string val = includeFolderTextBox.Text;
            if (val.Equals(scenario.IncludeFolder))
            {
                return;
            }

            Log.Info("[Scenario] include folder: {0} -> {1}", scenario.IncludeFolder, val);

            // 値を更新する
            scenario.IncludeFolder = val;

            // 編集済みフラグを設定する
            scenario.SetDirty(ScenarioItemId.IncludeFolder);
            Scenarios.SetDirty();

            // 文字色を変更する
            includeFolderTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     インクルードフォルダ参照ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIncludeFolderBrowseButtonClick(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                SelectedPath = Game.GetReadFileName(Game.ScenarioDataPathName),
                ShowNewFolderButton = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                includeFolderTextBox.Text = Game.GetRelativePathName(dialog.SelectedPath, Game.ScenarioPathName);
            }
        }

        /// <summary>
        ///     パネル画像を更新する
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        private void UpdatePanelImage(string fileName)
        {
            Image prev = panelPictureBox.Image;
            string pathName = Game.GetReadFileName(fileName);
            if (File.Exists(pathName))
            {
                Bitmap bitmap = new Bitmap(pathName);
                bitmap.MakeTransparent(Color.Lime);
                panelPictureBox.Image = bitmap;
            }
            else
            {
                panelPictureBox.Image = null;
            }
            if (prev != null)
            {
                prev.Dispose();
            }
        }

        #endregion

        #region メインタブ - オプション

        /// <summary>
        ///     AIの攻撃性コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAiAggressiveComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            ScenarioHeader header = Scenarios.Data.Header;
            bool dirty = ((e.Index == header.AiAggressive) && Scenarios.Data.IsDirty(ScenarioItemId.AiAggressive));
            Brush brush = new SolidBrush(dirty ? Color.Red : comboBox.ForeColor);
            string s = comboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     難易度コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDifficultyComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            ScenarioHeader header = Scenarios.Data.Header;
            bool dirty = ((e.Index == header.Difficulty) && Scenarios.Data.IsDirty(ScenarioItemId.Difficulty));
            Brush brush = new SolidBrush(dirty ? Color.Red : comboBox.ForeColor);
            string s = comboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     ゲームスピードコンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGameSpeedComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            ScenarioHeader header = Scenarios.Data.Header;
            bool dirty = ((e.Index == header.GameSpeed) && Scenarios.Data.IsDirty(ScenarioItemId.GameSpeed));
            Brush brush = new SolidBrush(dirty ? Color.Red : comboBox.ForeColor);
            string s = comboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     ショートシナリオチェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBattleScenarioCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            ScenarioHeader header = scenario.Header;

            // 値に変化がなければ何もしない
            if (battleScenarioCheckBox.Checked == header.IsCombatScenario)
            {
                return;
            }

            Log.Info("[Scenario] battle scenario: {0} -> {1}", BoolHelper.ToYesNo(header.IsCombatScenario),
                BoolHelper.ToYesNo(battleScenarioCheckBox.Checked));

            // 値を更新する
            header.IsCombatScenario = battleScenarioCheckBox.Checked;

            // 編集済みフラグを設定する
            scenario.SetDirty(ScenarioItemId.BattleScenario);

            // 文字色を変更する
            battleScenarioCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     国家の自由選択チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFreeCountryCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            ScenarioHeader header = scenario.Header;

            // 値に変化がなければ何もしない
            bool val = freeCountryCheckBox.Checked;
            if (val == header.IsFreeSelection)
            {
                return;
            }

            Log.Info("[Scenario] free country selection: {0} -> {1}", BoolHelper.ToYesNo(header.IsFreeSelection),
                BoolHelper.ToYesNo(val));

            // 値を更新する
            header.IsFreeSelection = val;

            // 編集済みフラグを設定する
            scenario.SetDirty(ScenarioItemId.FreeSelection);

            // 文字色を変更する
            freeCountryCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     外交を許可チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllowDiplomacyCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            ScenarioGlobalData data = scenario.GlobalData;

            // 値に変化がなければ何もしない
            bool val = allowDiplomacyCheckBox.Checked;
            if ((data.Rules != null) && (val == data.Rules.AllowDiplomacy))
            {
                return;
            }

            Log.Info("[Scenario] allow diplomacy: {0} -> {1}",
                (data.Rules != null) ? BoolHelper.ToYesNo(data.Rules.AllowDiplomacy) : "", BoolHelper.ToYesNo(val));

            if (data.Rules == null)
            {
                data.Rules = new ScenarioRules();
            }

            // 値を更新する
            data.Rules.AllowDiplomacy = val;

            // 編集済みフラグを設定する
            scenario.SetDirty(ScenarioItemId.AllowDiplomacy);

            // 文字色を変更する
            allowDiplomacyCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     生産を許可チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllowProductionCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            ScenarioGlobalData data = scenario.GlobalData;

            // 値に変化がなければ何もしない
            bool val = allowProductionCheckBox.Checked;
            if ((data.Rules != null) && (val == data.Rules.AllowProduction))
            {
                return;
            }

            Log.Info("[Scenario] allow production: {0} -> {1}",
                (data.Rules != null) ? BoolHelper.ToYesNo(data.Rules.AllowProduction) : "", BoolHelper.ToYesNo(val));

            if (data.Rules == null)
            {
                data.Rules = new ScenarioRules();
            }

            // 値を更新する
            data.Rules.AllowProduction = val;

            // 編集済みフラグを設定する
            scenario.SetDirty(ScenarioItemId.AllowProduction);

            // 文字色を変更する
            allowProductionCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     技術開発を許可チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllowTechnologyCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            ScenarioGlobalData data = scenario.GlobalData;

            // 値に変化がなければ何もしない
            bool val = allowTechnologyCheckBox.Checked;
            if ((data.Rules != null) && (val == data.Rules.AllowTechnology))
            {
                return;
            }

            Log.Info("[Scenario] allow technology: {0} -> {1}",
                (data.Rules != null) ? BoolHelper.ToYesNo(data.Rules.AllowTechnology) : "", BoolHelper.ToYesNo(val));

            if (data.Rules == null)
            {
                data.Rules = new ScenarioRules();
            }

            // 値を更新する
            data.Rules.AllowTechnology = val;

            // 編集済みフラグを設定する
            scenario.SetDirty(ScenarioItemId.AllowTechnology);

            // 文字色を変更する
            allowTechnologyCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     AIの攻撃性コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAiAggressiveComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 非選択になった時には何もしない
            if (aiAggressiveComboBox.SelectedIndex == -1)
            {
                return;
            }

            Scenario scenario = Scenarios.Data;
            ScenarioHeader header = scenario.Header;

            // 値に変化がなければ何もしない
            int val = aiAggressiveComboBox.SelectedIndex;
            if (val == header.AiAggressive)
            {
                return;
            }

            Log.Info("[Scenario] ai aggressive: {0} -> {1}", header.AiAggressive, val);

            // 値を更新する
            header.AiAggressive = val;

            // 編集済みフラグを設定する
            scenario.SetDirty(ScenarioItemId.AiAggressive);

            // 項目色を変更するために描画更新する
            aiAggressiveComboBox.Refresh();
        }

        /// <summary>
        ///     難易度コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDifficultyComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 非選択になった時には何もしない
            if (difficultyComboBox.SelectedIndex == -1)
            {
                return;
            }

            Scenario scenario = Scenarios.Data;
            ScenarioHeader header = scenario.Header;

            // 値に変化がなければ何もしない
            int val = difficultyComboBox.SelectedIndex;
            if (val == header.Difficulty)
            {
                return;
            }

            Log.Info("[Scenario] difficulty: {0} -> {1}", header.Difficulty, val);

            // 値を更新する
            header.Difficulty = val;

            // 編集済みフラグを設定する
            scenario.SetDirty(ScenarioItemId.Difficulty);

            // 項目色を変更するために描画更新する
            difficultyComboBox.Refresh();
        }

        /// <summary>
        ///     ゲームスピードコンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGameSpeedComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 非選択になった時には何もしない
            if (gameSpeedComboBox.SelectedIndex == -1)
            {
                return;
            }

            Scenario scenario = Scenarios.Data;
            ScenarioHeader header = scenario.Header;

            // 値に変化がなければ何もしない
            int val = gameSpeedComboBox.SelectedIndex;
            if (val == header.GameSpeed)
            {
                return;
            }

            Log.Info("[Scenario] game speed: {0} -> {1}", header.GameSpeed, val);

            // 値を更新する
            header.GameSpeed = val;

            // 編集済みフラグを設定する
            scenario.SetDirty(ScenarioItemId.GameSpeed);

            // 項目色を変更するために描画更新する
            gameSpeedComboBox.Refresh();
        }

        #endregion

        #region メインタブ - 国家選択

        /// <summary>
        ///     主要国の編集項目を更新する
        /// </summary>
        private void UpdateMajorItems()
        {
            int index = majorListBox.SelectedIndex;
            ScenarioHeader header = Scenarios.Data.Header;
            MajorCountrySettings major = header.MajorCountries[index];

            // 編集項目を更新する
            int year = (header.StartDate != null) ? header.StartDate.Year : header.StartYear;
            countryDescTextBox.Text = GetCountryDesc(major.Country, year, major.Desc);
            countryDescTextBox.ForeColor = major.IsDirty(MajorCountrySettingsItemId.Desc)
                ? Color.Red
                : SystemColors.WindowText;

            propagandaPictureBox.Text = major.PictureName;
            propagandaPictureBox.ForeColor = major.IsDirty(MajorCountrySettingsItemId.PictureName)
                ? Color.Red
                : SystemColors.WindowText;
            UpdatePropagandaImage(major.Country, major.PictureName);

            // 編集項目を有効化する
            countryDescLabel.Enabled = true;
            countryDescTextBox.Enabled = true;
            propagandaLabel.Enabled = true;
            propagandaPictureBox.Enabled = true;
            propagandaBrowseButton.Enabled = true;

            majorRemoveButton.Enabled = true;
            majorUpButton.Enabled = (index > 0);
            majorDownButton.Enabled = (index < header.MajorCountries.Count - 1);
        }

        /// <summary>
        ///     主要国の編集項目をクリアする
        /// </summary>
        private void ResetMajorItems()
        {
            // 編集項目を無効化する
            countryDescLabel.Enabled = false;
            countryDescTextBox.Enabled = false;
            propagandaLabel.Enabled = false;
            propagandaPictureBox.Enabled = false;
            propagandaBrowseButton.Enabled = false;

            majorRemoveButton.Enabled = false;
            majorUpButton.Enabled = false;
            majorDownButton.Enabled = false;

            // 編集項目をクリアする
            countryDescTextBox.Text = "";
            propagandaPictureBox.Text = "";
            Image prev = propagandaPictureBox.Image;
            propagandaPictureBox.Image = null;
            if (prev != null)
            {
                prev.Dispose();
            }
        }

        /// <summary>
        ///     主要国リストボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMajorListBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index < 0)
            {
                return;
            }
            ListBox listBox = sender as ListBox;
            if (listBox == null)
            {
                return;
            }

            Scenario scenario = Scenarios.Data;
            List<MajorCountrySettings> majors = scenario.Header.MajorCountries;

            // 背景を描画する
            e.DrawBackground();

            // 項目を描画する
            Brush brush;
            if ((e.State & DrawItemState.Selected) == 0)
            {
                // 変更ありの項目は文字色を変更する
                bool dirty = scenario.IsDirtySelectableCountry(majors[e.Index].Country);
                brush = new SolidBrush(dirty ? Color.Red : listBox.ForeColor);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            string s = listBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     選択可能国リストボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectableListBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index < 0)
            {
                return;
            }
            ListBox listBox = sender as ListBox;
            if (listBox == null)
            {
                return;
            }

            Scenario scenario = Scenarios.Data;

            // 背景を描画する
            e.DrawBackground();

            // 項目を描画する
            Brush brush;
            if ((e.State & DrawItemState.Selected) == 0)
            {
                // 変更ありの項目は文字色を変更する
                bool dirty = scenario.IsDirtySelectableCountry(_majorFreeCountries[e.Index]);
                brush = new SolidBrush(dirty ? Color.Red : listBox.ForeColor);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            string s = listBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     非選択国リストボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnselectableListBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index < 0)
            {
                return;
            }
            ListBox listBox = sender as ListBox;
            if (listBox == null)
            {
                return;
            }

            Scenario scenario = Scenarios.Data;

            // 背景を描画する
            e.DrawBackground();

            // 項目を描画する
            Brush brush;
            if ((e.State & DrawItemState.Selected) == 0)
            {
                // 変更ありの項目は文字色を変更する
                bool dirty = scenario.IsDirtySelectableCountry(_selectableFreeCountries[e.Index]);
                brush = new SolidBrush(dirty ? Color.Red : listBox.ForeColor);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            string s = listBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     主要国リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMajorListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (majorListBox.SelectedIndex < 0)
            {
                // 選択項目がなければ編集項目をクリアする
                ResetMajorItems();
                return;
            }

            // 編集項目を更新する
            UpdateMajorItems();
        }

        /// <summary>
        ///     選択可能国リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectableListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            bool flag = (selectableListBox.SelectedItems.Count > 0);
            majorAddButton.Enabled = flag;
            selectableRemoveButton.Enabled = flag;
        }

        /// <summary>
        ///     非選択国リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnselectableListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            selectableAddButton.Enabled = (unselectableListBox.SelectedItems.Count > 0);
        }

        /// <summary>
        ///     主要国の上へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMajorUpButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<MajorCountrySettings> majors = scenario.Header.MajorCountries;

            // 主要国リストの項目を移動する
            int index = majorListBox.SelectedIndex;
            MajorCountrySettings major = majors[index];
            majors.RemoveAt(index);
            majors.Insert(index - 1, major);

            // 主要国リストボックスの項目を移動する
            majorListBox.SelectedIndexChanged -= OnMajorListBoxSelectedIndexChanged;
            majorListBox.Items.RemoveAt(index);
            majorListBox.Items.Insert(index - 1, Countries.GetTagName(major.Country));
            majorListBox.SelectedIndexChanged += OnMajorListBoxSelectedIndexChanged;
            majorListBox.SelectedIndex = index - 1;

            // 編集済みフラグを設定する
            scenario.SetDirtySelectableCountry(major.Country);
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     主要国の下へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMajorDownButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<MajorCountrySettings> majors = scenario.Header.MajorCountries;

            // 主要国リストの項目を移動する
            int index = majorListBox.SelectedIndex;
            MajorCountrySettings major = majors[index];
            majors.RemoveAt(index);
            majors.Insert(index + 1, major);

            // 主要国リストボックスの項目を移動する
            majorListBox.SelectedIndexChanged -= OnMajorListBoxSelectedIndexChanged;
            majorListBox.Items.RemoveAt(index);
            majorListBox.Items.Insert(index + 1, Countries.GetTagName(major.Country));
            majorListBox.SelectedIndexChanged += OnMajorListBoxSelectedIndexChanged;
            majorListBox.SelectedIndex = index + 1;

            // 編集済みフラグを設定する
            scenario.SetDirtySelectableCountry(major.Country);
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     主要国追加ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMajorAddButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            ScenarioHeader header = scenario.Header;

            List<Country> countries =
                (from int index in selectableListBox.SelectedIndices select _majorFreeCountries[index]).ToList();
            majorListBox.BeginUpdate();
            selectableListBox.BeginUpdate();
            foreach (Country country in countries)
            {
                // 主要国リストボックスに追加する
                majorListBox.Items.Add(Countries.GetTagName(country));

                // 主要国リストに追加する
                MajorCountrySettings major = new MajorCountrySettings { Country = country };
                header.MajorCountries.Add(major);

                // 選択可能国リストボックスから削除する
                int index = _majorFreeCountries.IndexOf(country);
                selectableListBox.Items.RemoveAt(index);
                _majorFreeCountries.RemoveAt(index);

                // 編集済みフラグを設定する
                scenario.SetDirtySelectableCountry(country);
                Scenarios.SetDirty();

                Log.Info("[Scenario] major country: +{0}", Countries.Strings[(int) country]);
            }
            majorListBox.EndUpdate();
            selectableListBox.EndUpdate();

            // 主要国リストボックスに追加した項目を選択する
            majorListBox.SelectedIndex = majorListBox.Items.Count - 1;
        }

        /// <summary>
        ///     主要国削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMajorRemoveButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            ScenarioHeader header = scenario.Header;
            int index = majorListBox.SelectedIndex;
            Country country = header.MajorCountries[index].Country;

            // 編集済みフラグを設定する
            scenario.SetDirtySelectableCountry(country);
            Scenarios.SetDirty();

            // 主要国リストボックスから削除する
            majorListBox.SelectedIndexChanged -= OnMajorListBoxSelectedIndexChanged;
            majorListBox.Items.RemoveAt(index);

            // 主要国リストボックスの次の項目を選択する
            if (majorListBox.Items.Count > 0)
            {
                majorListBox.SelectedIndex = (index < majorListBox.Items.Count) ? index : index - 1;
            }

            majorListBox.SelectedIndexChanged += OnMajorListBoxSelectedIndexChanged;

            // 主要国リストから削除する
            header.MajorCountries.RemoveAt(index);

            // 選択項目を更新するためにイベントハンドラを呼び出す
            OnMajorListBoxSelectedIndexChanged(sender, e);

            // 選択可能国リストボックスに追加する
            index = _majorFreeCountries.FindIndex(c => c > country);
            if (index < 0)
            {
                index = _majorFreeCountries.Count;
            }
            selectableListBox.Items.Insert(index, Countries.GetTagName(country));
            _majorFreeCountries.Insert(index, country);

            Log.Info("[Scenario] major country: -{0}", Countries.Strings[(int) country]);

            // ボタン状態を更新する
            if (majorListBox.Items.Count == 0)
            {
                majorRemoveButton.Enabled = false;
            }
        }

        /// <summary>
        ///     選択可能国追加ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectableAddButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            ScenarioHeader header = scenario.Header;

            List<Country> countries =
                (from int index in unselectableListBox.SelectedIndices select _selectableFreeCountries[index]).ToList();
            selectableListBox.BeginUpdate();
            unselectableListBox.BeginUpdate();
            foreach (Country country in countries)
            {
                // 選択可能国リストボックスに追加する
                int index = _majorFreeCountries.FindIndex(c => c > country);
                if (index < 0)
                {
                    index = _majorFreeCountries.Count;
                }
                selectableListBox.Items.Insert(index, Countries.GetTagName(country));
                _majorFreeCountries.Insert(index, country);

                // 選択可能国リストに追加する
                index = header.SelectableCountries.FindIndex(c => c > country);
                if (index < 0)
                {
                    index = header.SelectableCountries.Count;
                }
                header.SelectableCountries.Insert(index, country);

                // 非選択国リストボックスから削除する
                index = _selectableFreeCountries.IndexOf(country);
                unselectableListBox.Items.RemoveAt(index);
                _selectableFreeCountries.RemoveAt(index);

                // 編集済みフラグを設定する
                scenario.SetDirtySelectableCountry(country);
                Scenarios.SetDirty();

                Log.Info("[Scenario] selectable country: +{0}", Countries.Strings[(int) country]);
            }
            selectableListBox.EndUpdate();
            unselectableListBox.EndUpdate();
        }

        /// <summary>
        ///     選択可能国削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectableRemoveButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            ScenarioHeader header = scenario.Header;

            List<Country> countries =
                (from int index in selectableListBox.SelectedIndices select _majorFreeCountries[index]).ToList();
            selectableListBox.BeginUpdate();
            unselectableListBox.BeginUpdate();
            foreach (Country country in countries)
            {
                // 非選択国リストボックスに追加する
                int index = _selectableFreeCountries.FindIndex(c => c > country);
                if (index < 0)
                {
                    index = _selectableFreeCountries.Count;
                }
                unselectableListBox.Items.Insert(index, Countries.GetTagName(country));
                _selectableFreeCountries.Insert(index, country);

                // 選択可能国リストボックスから削除する
                index = _majorFreeCountries.IndexOf(country);
                selectableListBox.Items.RemoveAt(index);
                _majorFreeCountries.RemoveAt(index);

                // 選択可能国リストから削除する
                header.SelectableCountries.Remove(country);

                // 編集済みフラグを設定する
                scenario.SetDirtySelectableCountry(country);
                Scenarios.SetDirty();

                Log.Info("[Scenario] selectable country: -{0}", Countries.Strings[(int) country]);
            }
            selectableListBox.EndUpdate();
            unselectableListBox.EndUpdate();
        }

        /// <summary>
        ///     プロパガンダ画像名テキストボックスの文字列変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPropagandaTextBoxTextChanged(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<MajorCountrySettings> majors = scenario.Header.MajorCountries;
            MajorCountrySettings major = majors[majorListBox.SelectedIndex];

            // 初期値から変更されていなければ何もしない
            string val = propagandaPictureBox.Text;
            if ((major.PictureName == null) && string.IsNullOrEmpty(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (val.Equals(major.PictureName))
            {
                return;
            }

            Log.Info("[Scenario] propaganda image: {0} -> {1} ({2})", major.PictureName, val, major.Country);

            // 値を更新する
            major.PictureName = val;

            // 編集済みフラグを設定する
            major.SetDirty(MajorCountrySettingsItemId.PictureName);
            Scenarios.SetDirty();

            // 文字色を変更する
            propagandaTextBox.ForeColor = Color.Red;

            // プロパガンダ画像を更新する
            UpdatePropagandaImage(major.Country, val);
        }

        /// <summary>
        ///     プロパガンダ画像名参照ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPropagandaBrowseButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<MajorCountrySettings> majors = scenario.Header.MajorCountries;
            MajorCountrySettings major = majors[majorListBox.SelectedIndex];

            OpenFileDialog dialog = new OpenFileDialog
            {
                InitialDirectory = Game.GetReadFileName(Game.ScenarioDataPathName),
                FileName = major.PictureName,
                Filter = Resources.OpenBitmapFileDialogFilter
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                propagandaPictureBox.Text = Game.GetRelativePathName(dialog.FileName);
            }
        }

        /// <summary>
        ///     国家説明テキストボックスの文字列変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryDescTextBoxTextChanged(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            ScenarioHeader header = scenario.Header;
            List<MajorCountrySettings> majors = header.MajorCountries;
            MajorCountrySettings major = majors[majorListBox.SelectedIndex];

            // 値に変化がなければ何もしない
            string val = countryDescTextBox.Text;
            int year = (header.StartDate != null) ? header.StartDate.Year : header.StartYear;
            string name = GetCountryDesc(major.Country, year, major.Desc);
            if (val.Equals(name))
            {
                return;
            }

            Log.Info("[Scenario] country desc: {0} -> {1} ({2})", name, val, major.Country);

            // 値を更新する
            Config.SetText(major.Desc, val, Game.ScenarioTextFileName);

            // 文字色を変更する
            countryDescTextBox.ForeColor = Color.Red;

            // 編集済みフラグを設定する
            major.SetDirty(MajorCountrySettingsItemId.Desc);
        }

        /// <summary>
        ///     国家の説明文字列を取得する
        /// </summary>
        /// <param name="tag">国タグ</param>
        /// <param name="year">開始年</param>
        /// <param name="desc">説明文字列</param>
        /// <returns>説明文字列</returns>
        private static string GetCountryDesc(Country tag, int year, string desc)
        {
            if (!string.IsNullOrEmpty(desc) && Config.ExistsKey(desc))
            {
                return Config.GetText(desc);
            }

            string country = Countries.Strings[(int) tag];

            // 年数の下2桁のみ使用する
            year = year % 100;

            string key = string.Format("{0}_{1}_DESC", country, year);
            if (Config.ExistsKey(key))
            {
                return Config.GetText(key);
            }

            key = string.Format("{0}_DESC", country);
            return Config.GetText(key);
        }

        /// <summary>
        ///     プロパガンダ画像を更新する
        /// </summary>
        /// <param name="country">国タグ</param>
        /// <param name="fileName">プロパガンダ画像名</param>
        private void UpdatePropagandaImage(Country country, string fileName)
        {
            Image prev = propagandaPictureBox.Image;
            propagandaPictureBox.Image = GetPropagandaImage(country, fileName);
            if (prev != null)
            {
                prev.Dispose();
            }
        }

        /// <summary>
        ///     国家のプロパガンダ画像を取得する
        /// </summary>
        /// <param name="country">国タグ</param>
        /// <param name="fileName">プロパガンダ画像名</param>
        /// <returns>プロパガンダ画像</returns>
        private static Image GetPropagandaImage(Country country, string fileName)
        {
            Bitmap bitmap;
            string pathName;
            if (!string.IsNullOrEmpty(fileName))
            {
                pathName = Game.GetReadFileName(fileName);
                if (File.Exists(pathName))
                {
                    bitmap = new Bitmap(pathName);
                    bitmap.MakeTransparent(bitmap.GetPixel(0, 0));
                    return bitmap;
                }
            }

            pathName = Game.GetReadFileName(Game.ScenarioDataPathName,
                string.Format("propaganda_{0}.bmp", Countries.Strings[(int) country]));
            if (!File.Exists(pathName))
            {
                return null;
            }

            bitmap = new Bitmap(pathName);
            bitmap.MakeTransparent(bitmap.GetPixel(0, 0));
            return bitmap;
        }

        #endregion

        #endregion

        #region 同盟タブ

        #region 同盟タブ - 共通

        /// <summary>
        ///     同盟タブの編集項目を初期化する
        /// </summary>
        private void InitAllianceTab()
        {
            // 何もしない
        }

        /// <summary>
        ///     同盟タブの編集項目を更新する
        /// </summary>
        private void UpdateAllianceTab()
        {
            // 同盟情報の編集項目を無効化する
            DisableAllianceItems();

            // 戦争情報の編集項目を無効化する
            DisableWarItems();

            // 同盟情報の新規ボタンを有効化する
            allianceNewButton.Enabled = true;

            // 戦争情報の新規ボタンを有効化する
            warNewButton.Enabled = true;

            // 同盟リストビューを更新する
            UpdateAllianceListView();

            // 戦争リストビューを更新する
            UpdateWarListView();

            // 同盟グループボックスを有効化する
            allianceGroupBox.Enabled = true;

            // 戦争グループボックスを有効化する
            warGroupBox.Enabled = true;
        }

        #endregion

        #region 同盟タブ - 同盟

        /// <summary>
        ///     同盟リストビューを更新する
        /// </summary>
        private void UpdateAllianceListView()
        {
            ScenarioGlobalData data = Scenarios.Data.GlobalData;

            allianceListView.BeginUpdate();
            allianceListView.Items.Clear();

            // 枢軸国
            ListViewItem item = new ListViewItem { Text = GetAllianceName(data.Axis), Tag = data.Axis };
            item.SubItems.Add((data.Axis != null)
                ? Countries.GetNameList(data.Axis.Participant)
                : Config.GetText(TextId.AllianceAxis));
            allianceListView.Items.Add(item);

            // 連合国
            item = new ListViewItem { Text = GetAllianceName(data.Allies), Tag = data.Allies };
            item.SubItems.Add((data.Allies != null)
                ? Countries.GetNameList(data.Allies.Participant)
                : Config.GetText(TextId.AllianceAllies));
            allianceListView.Items.Add(item);

            // 共産国
            item = new ListViewItem { Text = GetAllianceName(data.Comintern), Tag = data.Comintern };
            item.SubItems.Add((data.Comintern != null)
                ? Countries.GetNameList(data.Comintern.Participant)
                : Config.GetText(TextId.AllianceComintern));
            allianceListView.Items.Add(item);

            // その他の同盟
            foreach (Alliance alliance in data.Alliances)
            {
                item = new ListViewItem
                {
                    Text = Resources.Alliance,
                    Tag = alliance
                };
                item.SubItems.Add(Countries.GetNameList(alliance.Participant));
                allianceListView.Items.Add(item);
            }

            allianceListView.EndUpdate();
        }

        /// <summary>
        ///     同盟の編集項目を無効化する
        /// </summary>
        private void DisableAllianceItems()
        {
            // 編集項目を無効化する
            allianceUpButton.Enabled = false;
            allianceDownButton.Enabled = false;
            allianceRemoveButton.Enabled = false;

            allianceNameLabel.Enabled = false;
            allianceNameTextBox.Enabled = false;
            allianceIdLabel.Enabled = false;
            allianceTypeTextBox.Enabled = false;
            allianceIdTextBox.Enabled = false;
            allianceParticipantLabel.Enabled = false;
            allianceParticipantListBox.Enabled = false;
            allianceCountryListBox.Enabled = false;

            allianceParticipantAddButton.Enabled = false;
            allianceParticipantRemoveButton.Enabled = false;
            allianceLeaderButton.Enabled = false;

            // 編集項目をクリアする
            allianceNameTextBox.Text = "";
            allianceTypeTextBox.Text = "";
            allianceIdTextBox.Text = "";

            allianceParticipantListBox.Items.Clear();
            allianceCountryListBox.Items.Clear();
        }

        /// <summary>
        ///     同盟の編集項目を更新する
        /// </summary>
        private void UpdateAllianceItems()
        {
            // 選択項目がなければ何もしない
            Alliance alliance = GetSelectedAlliance();
            if (alliance == null)
            {
                return;
            }

            // 同盟名
            allianceNameTextBox.Text = GetAllianceName(alliance);
            allianceNameTextBox.ForeColor = alliance.IsDirty(AllianceItemId.Name)
                ? Color.Red
                : SystemColors.WindowText;

            // 同盟ID
            bool flag = (alliance.Id != null);
            allianceTypeTextBox.Text = flag ? IntHelper.ToString(alliance.Id.Type) : "";
            allianceIdTextBox.Text = flag ? IntHelper.ToString(alliance.Id.Id) : "";

            allianceTypeTextBox.ForeColor = alliance.IsDirty(AllianceItemId.Type) ? Color.Red : SystemColors.WindowText;
            allianceIdTextBox.ForeColor = alliance.IsDirty(AllianceItemId.Id) ? Color.Red : SystemColors.WindowText;

            // 参加国リストボックス
            allianceParticipantListBox.BeginUpdate();
            allianceParticipantListBox.Items.Clear();
            foreach (Country country in alliance.Participant)
            {
                allianceParticipantListBox.Items.Add(Countries.GetTagName(country));
            }
            allianceParticipantListBox.EndUpdate();

            // 国家リストボックス
            _allianceFreeCountries = Countries.Tags.Where(country => !alliance.Participant.Contains(country)).ToList();
            allianceCountryListBox.BeginUpdate();
            allianceCountryListBox.Items.Clear();
            foreach (Country country in _allianceFreeCountries)
            {
                allianceCountryListBox.Items.Add(Countries.GetTagName(country));
            }
            allianceCountryListBox.EndUpdate();

            int count = allianceListView.Items.Count;
            int index = allianceListView.SelectedIndices[0];

            // 枢軸国/連合国/共産国は順番変更/削除できない
            allianceUpButton.Enabled = (index > 3);
            allianceDownButton.Enabled = ((index < count - 1) && (index >= 3));
            allianceRemoveButton.Enabled = (index >= 3);

            // 編集項目を有効化する
            allianceNameLabel.Enabled = (index < 3);
            allianceNameTextBox.Enabled = (index < 3);
            allianceIdLabel.Enabled = true;
            allianceTypeTextBox.Enabled = true;
            allianceIdTextBox.Enabled = true;
            allianceParticipantLabel.Enabled = true;
            allianceParticipantListBox.Enabled = true;
            allianceCountryListBox.Enabled = true;
        }

        /// <summary>
        ///     同盟リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ編集項目を無効化する
            if (allianceListView.SelectedItems.Count == 0)
            {
                DisableAllianceItems();
                return;
            }

            // 編集項目を更新する
            UpdateAllianceItems();

            // 同盟参加国の追加ボタンを無効化する
            allianceParticipantAddButton.Enabled = false;

            // 同盟参加国の削除ボタンを無効化する
            allianceParticipantRemoveButton.Enabled = false;

            // リーダーに設定ボタンを無効化する
            allianceLeaderButton.Enabled = false;
        }

        /// <summary>
        ///     同盟の上へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceUpButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<Alliance> alliances = scenario.GlobalData.Alliances;

            // 同盟リストビューの項目を移動する
            int index = allianceListView.SelectedIndices[0];
            ListViewItem item = allianceListView.Items[index];
            allianceListView.Items.RemoveAt(index);
            allianceListView.Items.Insert(index - 1, item);
            allianceListView.Items[index - 1].Focused = true;
            allianceListView.Items[index - 1].Selected = true;
            allianceListView.EnsureVisible(index - 1);

            // 同盟リストの項目を移動する
            index -= 3; // -3は枢軸国/連合国/共産国の分
            Alliance alliance = alliances[index];
            alliances.RemoveAt(index);
            alliances.Insert(index - 1, alliance);

            // 編集済みフラグを設定する
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     同盟の下へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceDownButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<Alliance> alliances = scenario.GlobalData.Alliances;

            // 同盟リストビューの項目を移動する
            int index = allianceListView.SelectedIndices[0];
            ListViewItem item = allianceListView.Items[index];
            allianceListView.Items.RemoveAt(index);
            allianceListView.Items.Insert(index + 1, item);
            allianceListView.Items[index + 1].Focused = true;
            allianceListView.Items[index + 1].Selected = true;
            allianceListView.EnsureVisible(index + 1);

            // 同盟リストの項目を移動する
            index -= 3; // -3は枢軸国/連合国/共産国の分
            Alliance alliance = alliances[index];
            alliances.RemoveAt(index);
            alliances.Insert(index + 1, alliance);

            // 編集済みフラグを設定する
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     同盟の新規ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceNewButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<Alliance> alliances = scenario.GlobalData.Alliances;

            // 同盟リストに項目を追加する
            Alliance alliance = new Alliance { Id = Scenarios.GetNewTypeId(Scenarios.DefaultAllianceType, 1) };
            alliances.Add(alliance);

            // 同盟リストビューに項目を追加する
            ListViewItem item = new ListViewItem { Text = Resources.Alliance, Tag = alliance };
            item.SubItems.Add("");
            allianceListView.Items.Add(item);

            // 編集済みフラグを設定する
            alliance.SetDirty(AllianceItemId.Type);
            alliance.SetDirty(AllianceItemId.Id);
            Scenarios.SetDirty();

            // 追加した項目を選択する
            if (allianceListView.SelectedIndices.Count > 0)
            {
                ListViewItem prev = allianceListView.SelectedItems[0];
                prev.Focused = false;
                prev.Selected = false;
            }
            item.Focused = true;
            item.Selected = true;
        }

        /// <summary>
        ///     同盟の削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceRemoveButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<Alliance> alliances = scenario.GlobalData.Alliances;

            // 枢軸国/連合国/共産国は削除できない
            int index = allianceListView.SelectedIndices[0] - 3;
            if (index < 0)
            {
                return;
            }

            Alliance alliance = alliances[index];

            // typeとidの組を削除する
            Scenarios.RemoveTypeId(alliance.Id);

            // 同盟リストから項目を削除する
            alliances.RemoveAt(index);

            // 同盟リストビューから項目を削除する
            allianceListView.Items.RemoveAt(index + 3);

            // 編集済みフラグを設定する
            Scenarios.SetDirty();

            // 削除した項目の次を選択する
            index += (index < alliances.Count) ? 3 : (3 - 1);
            allianceListView.Items[index].Focused = true;
            allianceListView.Items[index].Selected = true;
        }

        /// <summary>
        ///     同盟名テキストボックスの文字列変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceNameTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Alliance alliance = GetSelectedAlliance();
            if (alliance == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            string name = GetAllianceName(alliance);
            string val = allianceNameTextBox.Text;
            if (val.Equals(name))
            {
                return;
            }

            Log.Info("[Scenario] alliance name: {0} -> {1}", name, val);

            // 値を更新する
            string s = val;
            ScenarioGlobalData data = Scenarios.Data.GlobalData;
            if (!string.IsNullOrEmpty(alliance.Name))
            {
                Config.SetText("ALLIANCE_" + alliance.Name, s, Game.ScenarioTextFileName);
            }
            else
            {
                if (alliance == data.Axis)
                {
                    Config.SetText("EYR_AXIS", s, Game.ScenarioTextFileName);
                }
                else if (alliance == data.Allies)
                {
                    Config.SetText("EYR_ALLIES", s, Game.ScenarioTextFileName);
                }
                else if (alliance == data.Comintern)
                {
                    Config.SetText("EYR_COM", s, Game.ScenarioTextFileName);
                }
            }

            // 同盟リストビューの項目を更新する
            allianceListView.SelectedItems[0].Text = GetAllianceName(alliance);

            // 文字色を変更する
            allianceNameTextBox.ForeColor = Color.Red;

            // 編集済みフラグを設定する
            alliance.SetDirty(AllianceItemId.Name);
        }

        /// <summary>
        ///     同盟のtypeテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceTypeTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Alliance alliance = GetSelectedAlliance();
            if (alliance == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(allianceTypeTextBox.Text, out val))
            {
                allianceTypeTextBox.Text = (alliance.Id != null) ? IntHelper.ToString(alliance.Id.Type) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((alliance.Id != null) && (val == alliance.Id.Type))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if ((alliance.Id != null) && Scenarios.ExistsTypeId(val, alliance.Id.Id))
            {
                allianceTypeTextBox.Text = IntHelper.ToString(alliance.Id.Type);
                return;
            }

            Log.Info("[Scenario] alliance type: {0} -> {1} ({2})",
                (alliance.Id != null) ? IntHelper.ToString(alliance.Id.Type) : "", val,
                allianceListView.SelectedIndices[0]);

            if (alliance.Id != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(alliance.Id);

                // 値を更新する
                alliance.Id.Type = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(alliance.Id);
            }
            else
            {
                // 値を更新する
                alliance.Id = Scenarios.GetNewTypeId(val, 1);

                // 編集済みフラグを設定する
                alliance.SetDirty(AllianceItemId.Id);

                // 編集項目を更新する
                allianceIdTextBox.Text = IntHelper.ToString(alliance.Id.Id);

                // 文字色を変更する
                allianceIdTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            alliance.SetDirty(AllianceItemId.Type);
            Scenarios.SetDirty();

            // 文字色を変更する
            allianceTypeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     同盟のidテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceIdTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Alliance alliance = GetSelectedAlliance();
            if (alliance == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(allianceIdTextBox.Text, out val))
            {
                allianceIdTextBox.Text = (alliance.Id != null) ? IntHelper.ToString(alliance.Id.Id) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((alliance.Id != null) && (val == alliance.Id.Id))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if (Scenarios.ExistsTypeId((alliance.Id != null) ? alliance.Id.Type : Scenarios.DefaultAllianceType, val))
            {
                allianceIdTextBox.Text = (alliance.Id != null) ? IntHelper.ToString(alliance.Id.Id) : "";
            }

            Log.Info("[Scenario] alliance id: {0} -> {1} ({2})",
                (alliance.Id != null) ? IntHelper.ToString(alliance.Id.Id) : "", val,
                allianceListView.SelectedIndices[0]);

            if (alliance.Id != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(alliance.Id);

                // 値を更新する
                alliance.Id.Id = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(alliance.Id);
            }
            else
            {
                // 値を更新する
                alliance.Id = Scenarios.GetNewTypeId(Scenarios.DefaultAllianceType, val);

                // 編集済みフラグを設定する
                alliance.SetDirty(AllianceItemId.Type);

                // 編集項目を更新する
                allianceTypeTextBox.Text = IntHelper.ToString(alliance.Id.Type);

                // 文字色を変更する
                allianceTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            alliance.SetDirty(AllianceItemId.Id);
            Scenarios.SetDirty();

            // 文字色を変更する
            allianceIdTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     同盟参加国リストボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceParticipantListBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index < 0)
            {
                return;
            }
            ListBox listBox = sender as ListBox;
            if (listBox == null)
            {
                return;
            }

            // 選択項目がなければ何もしない
            Alliance alliance = GetSelectedAlliance();
            if (alliance == null)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目を描画する
            Brush brush;
            if ((e.State & DrawItemState.Selected) == 0)
            {
                // 変更ありの項目は文字色を変更する
                bool dirty = alliance.IsDirtyCountry(alliance.Participant[e.Index]);
                brush = new SolidBrush(dirty ? Color.Red : listBox.ForeColor);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            string s = listBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     同盟非参加国リストボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceCountryListBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index < 0)
            {
                return;
            }
            ListBox listBox = sender as ListBox;
            if (listBox == null)
            {
                return;
            }

            // 選択項目がなければ何もしない
            Alliance alliance = GetSelectedAlliance();
            if (alliance == null)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目を描画する
            Brush brush;
            if ((e.State & DrawItemState.Selected) == 0)
            {
                // 変更ありの項目は文字色を変更する
                bool dirty = alliance.IsDirtyCountry(_allianceFreeCountries[e.Index]);
                brush = new SolidBrush(dirty ? Color.Red : listBox.ForeColor);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            string s = listBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     同盟参加国リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceParticipantListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            int count = allianceParticipantListBox.SelectedIndices.Count;
            int index = allianceParticipantListBox.SelectedIndex;
            allianceParticipantRemoveButton.Enabled = (count > 0);
            allianceLeaderButton.Enabled = ((count == 1) && (index > 0));
        }

        /// <summary>
        ///     同盟非参加国リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceCountryListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            int count = allianceCountryListBox.SelectedIndices.Count;
            allianceParticipantAddButton.Enabled = (count > 0);
        }

        /// <summary>
        ///     同盟参加国の追加ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceParticipantAddButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Alliance alliance = GetSelectedAlliance();
            if (alliance == null)
            {
                return;
            }

            List<Country> countries =
                (from int index in allianceCountryListBox.SelectedIndices select _allianceFreeCountries[index]).ToList();
            allianceParticipantListBox.BeginUpdate();
            allianceCountryListBox.BeginUpdate();
            foreach (Country country in countries)
            {
                // 同盟参加国リストボックスに追加する
                allianceParticipantListBox.Items.Add(Countries.GetTagName(country));

                // 同盟参加国リストに追加する
                alliance.Participant.Add(country);

                // 同盟非参加国リストボックスから削除する
                int index = _allianceFreeCountries.IndexOf(country);
                allianceCountryListBox.Items.RemoveAt(index);
                _allianceFreeCountries.RemoveAt(index);

                // 編集済みフラグを設定する
                alliance.SetDirtyCountry(country);
                Scenarios.SetDirty();

                // 同盟リストビューの項目を更新する
                allianceListView.SelectedItems[0].SubItems[1].Text = Countries.GetNameList(alliance.Participant);

                Log.Info("[Scenario] alliance participant: +{0} ({1})", Countries.Strings[(int) country],
                    allianceListView.SelectedIndices[0]);
            }
            allianceParticipantListBox.EndUpdate();
            allianceCountryListBox.EndUpdate();
        }

        /// <summary>
        ///     同盟参加国の削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceParticipantRemoveButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Alliance alliance = GetSelectedAlliance();
            if (alliance == null)
            {
                return;
            }

            List<Country> countries =
                (from int index in allianceParticipantListBox.SelectedIndices select alliance.Participant[index])
                    .ToList();
            allianceParticipantListBox.BeginUpdate();
            allianceCountryListBox.BeginUpdate();
            foreach (Country country in countries)
            {
                // 同盟非参加国リストボックスに追加する
                int index = _allianceFreeCountries.FindIndex(c => c > country);
                if (index < 0)
                {
                    index = _allianceFreeCountries.Count;
                }
                allianceCountryListBox.Items.Insert(index, Countries.GetTagName(country));
                _allianceFreeCountries.Insert(index, country);

                // 同盟参加国リストボックスから削除する
                index = alliance.Participant.IndexOf(country);
                allianceParticipantListBox.Items.RemoveAt(index);

                // 同盟参加国リストから削除する
                alliance.Participant.Remove(country);

                // 編集済みフラグを設定する
                alliance.SetDirtyCountry(country);
                Scenarios.SetDirty();

                // 同盟リストビューの項目を更新する
                allianceListView.SelectedItems[0].SubItems[1].Text = Countries.GetNameList(alliance.Participant);

                Log.Info("[Scenario] alliance participant: -{0} ({1})", Countries.Strings[(int) country],
                    allianceListView.SelectedIndices[0]);
            }
            allianceParticipantListBox.EndUpdate();
            allianceCountryListBox.EndUpdate();
        }

        /// <summary>
        ///     同盟のリーダーに設定ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceLeaderButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Alliance alliance = GetSelectedAlliance();
            if (alliance == null)
            {
                return;
            }

            int index = allianceParticipantListBox.SelectedIndex;
            Country country = alliance.Participant[index];

            // 同盟参加国リストボックスの先頭に移動する
            allianceParticipantListBox.BeginUpdate();
            allianceParticipantListBox.Items.RemoveAt(index);
            allianceParticipantListBox.Items.Insert(0, Countries.GetTagName(country));
            allianceParticipantListBox.EndUpdate();

            // 同盟参加国リストの先頭に移動する
            alliance.Participant.RemoveAt(index);
            alliance.Participant.Insert(0, country);

            // 編集済みフラグを設定する
            alliance.SetDirtyCountry(country);
            Scenarios.SetDirty();

            // 同盟リストビューの項目を更新する
            allianceListView.SelectedItems[0].SubItems[1].Text = Countries.GetNameList(alliance.Participant);

            Log.Info("[Scenario] alliance leader: {0} ({1})", Countries.Strings[(int) country],
                allianceListView.SelectedIndices[0]);
        }

        /// <summary>
        ///     選択中の同盟情報を取得する
        /// </summary>
        /// <returns>選択中の同盟情報</returns>
        private Alliance GetSelectedAlliance()
        {
            return (allianceListView.SelectedItems.Count > 0) ? allianceListView.SelectedItems[0].Tag as Alliance : null;
        }

        /// <summary>
        ///     同盟名を取得する
        /// </summary>
        /// <param name="alliance">同盟情報</param>
        /// <returns>同盟名</returns>
        private static string GetAllianceName(Alliance alliance)
        {
            if (!string.IsNullOrEmpty(alliance.Name))
            {
                string key = "ALLIANCE_" + alliance.Name;
                return Config.ExistsKey(key) ? Config.GetText(key) : "";
            }
            ScenarioGlobalData data = Scenarios.Data.GlobalData;
            if (alliance == data.Axis)
            {
                return Config.GetText(TextId.AllianceAxis);
            }
            if (alliance == data.Allies)
            {
                return Config.GetText(TextId.AllianceAllies);
            }
            if (alliance == data.Comintern)
            {
                return Config.GetText(TextId.AllianceComintern);
            }
            return "";
        }

        #endregion

        #region 同盟タブ - 戦争

        /// <summary>
        ///     戦争リストビューを更新する
        /// </summary>
        private void UpdateWarListView()
        {
            ScenarioGlobalData data = Scenarios.Data.GlobalData;

            warListView.BeginUpdate();
            warListView.Items.Clear();
            foreach (War war in data.Wars)
            {
                ListViewItem item = new ListViewItem
                {
                    Text = Countries.GetNameList(war.Attackers.Participant),
                    Tag = war
                };
                item.SubItems.Add(Countries.GetNameList(war.Defenders.Participant));
                warListView.Items.Add(item);
            }
            warListView.EndUpdate();
        }

        /// <summary>
        ///     戦争の編集項目を無効化する
        /// </summary>
        private void DisableWarItems()
        {
            // 編集項目を無効化する
            warUpButton.Enabled = false;
            warDownButton.Enabled = false;
            warRemoveButton.Enabled = false;

            warStartDateLabel.Enabled = false;
            warStartYearTextBox.Enabled = false;
            warStartMonthTextBox.Enabled = false;
            warStartDayTextBox.Enabled = false;
            warEndDateLabel.Enabled = false;
            warEndYearTextBox.Enabled = false;
            warEndMonthTextBox.Enabled = false;
            warEndDayTextBox.Enabled = false;
            warIdLabel.Enabled = false;
            warTypeTextBox.Enabled = false;
            warIdTextBox.Enabled = false;
            warAttackerLabel.Enabled = false;
            warAttackerListBox.Enabled = false;
            warAttackerIdLabel.Enabled = false;
            warAttackerTypeTextBox.Enabled = false;
            warAttackerIdTextBox.Enabled = false;
            warDefenderLabel.Enabled = false;
            warDefenderListBox.Enabled = false;
            warDefenderIdLabel.Enabled = false;
            warDefenderTypeTextBox.Enabled = false;
            warDefenderIdTextBox.Enabled = false;
            warCountryListBox.Enabled = false;

            warAttackerAddButton.Enabled = false;
            warAttackerRemoveButton.Enabled = false;
            warAttackerLeaderButton.Enabled = false;
            warDefenderAddButton.Enabled = false;
            warDefenderRemoveButton.Enabled = false;
            warDefenderLeaderButton.Enabled = false;

            // 編集項目をクリアする
            warStartYearTextBox.Text = "";
            warStartMonthTextBox.Text = "";
            warStartDayTextBox.Text = "";
            warEndYearTextBox.Text = "";
            warEndMonthTextBox.Text = "";
            warEndYearTextBox.Text = "";
            warTypeTextBox.Text = "";
            warIdTextBox.Text = "";
            warAttackerTypeTextBox.Text = "";
            warAttackerIdTextBox.Text = "";
            warDefenderTypeTextBox.Text = "";
            warDefenderIdTextBox.Text = "";

            warAttackerListBox.Items.Clear();
            warDefenderListBox.Items.Clear();
            warCountryListBox.Items.Clear();
        }

        /// <summary>
        ///     戦争の編集項目を更新する
        /// </summary>
        private void UpdateWarItems()
        {
            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            // 開始日時
            bool flag = (war.StartDate != null);
            warStartYearTextBox.Text = flag ? IntHelper.ToString(war.StartDate.Year) : "";
            warStartMonthTextBox.Text = flag ? IntHelper.ToString(war.StartDate.Month) : "";
            warStartDayTextBox.Text = flag ? IntHelper.ToString(war.StartDate.Day) : "";

            warStartYearTextBox.ForeColor = war.IsDirty(WarItemId.StartYear) ? Color.Red : SystemColors.WindowText;
            warStartMonthTextBox.ForeColor = war.IsDirty(WarItemId.StartMonth) ? Color.Red : SystemColors.WindowText;
            warStartDayTextBox.ForeColor = war.IsDirty(WarItemId.StartDay) ? Color.Red : SystemColors.WindowText;

            // 終了日時
            flag = (war.EndDate != null);
            warEndYearTextBox.Text = flag ? IntHelper.ToString(war.EndDate.Year) : "";
            warEndMonthTextBox.Text = flag ? IntHelper.ToString(war.EndDate.Month) : "";
            warEndDayTextBox.Text = flag ? IntHelper.ToString(war.EndDate.Day) : "";

            warEndYearTextBox.ForeColor = war.IsDirty(WarItemId.EndYear) ? Color.Red : SystemColors.WindowText;
            warEndMonthTextBox.ForeColor = war.IsDirty(WarItemId.EndMonth) ? Color.Red : SystemColors.WindowText;
            warEndDayTextBox.ForeColor = war.IsDirty(WarItemId.EndDay) ? Color.Red : SystemColors.WindowText;

            // 戦争ID
            flag = (war.Id != null);
            warTypeTextBox.Text = flag ? IntHelper.ToString(war.Id.Type) : "";
            warIdTextBox.Text = flag ? IntHelper.ToString(war.Id.Id) : "";

            warTypeTextBox.ForeColor = war.IsDirty(WarItemId.Type) ? Color.Red : SystemColors.WindowText;
            warIdTextBox.ForeColor = war.IsDirty(WarItemId.Id) ? Color.Red : SystemColors.WindowText;

            // 攻撃側ID
            flag = ((war.Attackers != null) && (war.Attackers.Id != null));
            warAttackerTypeTextBox.Text = flag ? IntHelper.ToString(war.Attackers.Id.Type) : "";
            warAttackerIdTextBox.Text = flag ? IntHelper.ToString(war.Attackers.Id.Id) : "";

            warAttackerTypeTextBox.ForeColor = war.IsDirty(WarItemId.AttackerType)
                ? Color.Red
                : SystemColors.WindowText;
            warAttackerIdTextBox.ForeColor = war.IsDirty(WarItemId.AttackerId) ? Color.Red : SystemColors.WindowText;

            // 防御側ID
            flag = ((war.Defenders != null) && (war.Defenders.Id != null));
            warDefenderTypeTextBox.Text = flag ? IntHelper.ToString(war.Defenders.Id.Type) : "";
            warDefenderIdTextBox.Text = flag ? IntHelper.ToString(war.Defenders.Id.Id) : "";
            warDefenderTypeTextBox.ForeColor = war.IsDirty(WarItemId.DefenderType)
                ? Color.Red
                : SystemColors.WindowText;
            warDefenderIdTextBox.ForeColor = war.IsDirty(WarItemId.DefenderId) ? Color.Red : SystemColors.WindowText;

            IEnumerable<Country> countries = Countries.Tags;

            // 攻撃側リストボックス
            warAttackerListBox.BeginUpdate();
            warAttackerListBox.Items.Clear();
            if ((war.Attackers != null) && (war.Attackers.Participant != null))
            {
                foreach (Country country in war.Attackers.Participant)
                {
                    warAttackerListBox.Items.Add(Countries.GetTagName(country));
                }
                countries = countries.Where(country => !war.Attackers.Participant.Contains(country));
            }
            warAttackerListBox.EndUpdate();

            // 防御側リストボックス
            warDefenderListBox.BeginUpdate();
            warDefenderListBox.Items.Clear();
            if ((war.Defenders != null) && (war.Defenders.Participant != null))
            {
                foreach (Country country in war.Defenders.Participant)
                {
                    warDefenderListBox.Items.Add(Countries.GetTagName(country));
                }
                countries = countries.Where(country => !war.Defenders.Participant.Contains(country));
            }
            warDefenderListBox.EndUpdate();

            // 国家リストボックス
            _warFreeCountries = countries.ToList();
            warCountryListBox.BeginUpdate();
            warCountryListBox.Items.Clear();
            foreach (Country country in _warFreeCountries)
            {
                warCountryListBox.Items.Add(Countries.GetTagName(country));
            }
            warCountryListBox.EndUpdate();

            int count = warListView.Items.Count;
            int index = warListView.SelectedIndices[0];

            warUpButton.Enabled = (index > 0);
            warDownButton.Enabled = (index < count - 1);
            warRemoveButton.Enabled = true;

            // 編集項目を有効化する
            warStartDateLabel.Enabled = true;
            warStartYearTextBox.Enabled = true;
            warStartMonthTextBox.Enabled = true;
            warStartDayTextBox.Enabled = true;
            warEndDateLabel.Enabled = true;
            warEndYearTextBox.Enabled = true;
            warEndMonthTextBox.Enabled = true;
            warEndDayTextBox.Enabled = true;
            warIdLabel.Enabled = true;
            warTypeTextBox.Enabled = true;
            warIdTextBox.Enabled = true;
            warAttackerLabel.Enabled = true;
            warAttackerListBox.Enabled = true;
            warAttackerIdLabel.Enabled = true;
            warAttackerTypeTextBox.Enabled = true;
            warAttackerIdTextBox.Enabled = true;
            warDefenderLabel.Enabled = true;
            warDefenderListBox.Enabled = true;
            warDefenderIdLabel.Enabled = true;
            warDefenderTypeTextBox.Enabled = true;
            warDefenderIdTextBox.Enabled = true;
            warCountryListBox.Enabled = true;
        }

        /// <summary>
        ///     戦争リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ編集項目を無効化する
            if (warListView.SelectedItems.Count == 0)
            {
                DisableWarItems();
                return;
            }

            // 編集項目を更新する
            UpdateWarItems();

            // 攻撃側参加国の追加ボタンを無効化する
            warAttackerAddButton.Enabled = false;

            // 攻撃側参加国の削除ボタンを無効化する
            warAttackerRemoveButton.Enabled = false;

            // 戦争攻撃側リーダーに設定ボタンを無効化する
            warAttackerLeaderButton.Enabled = false;

            // 防御側参加国の追加ボタンを無効化する
            warDefenderAddButton.Enabled = false;

            // 防御側参加国の削除ボタンを無効化する
            warDefenderRemoveButton.Enabled = false;

            // 戦争防御側リーダーに設定ボタンを無効化する
            warAttackerLeaderButton.Enabled = false;
        }

        /// <summary>
        ///     戦争の上へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarUpButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<War> wars = scenario.GlobalData.Wars;

            // 戦争リストビューの項目を移動する
            int index = warListView.SelectedIndices[0];
            ListViewItem item = warListView.Items[index];
            warListView.Items.RemoveAt(index);
            warListView.Items.Insert(index - 1, item);
            warListView.Items[index - 1].Focused = true;
            warListView.Items[index - 1].Selected = true;
            warListView.EnsureVisible(index - 1);

            // 戦争リストの項目を移動する
            War war = wars[index];
            wars.RemoveAt(index);
            wars.Insert(index - 1, war);

            // 編集済みフラグを設定する
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     戦争の下へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarDownButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<War> wars = scenario.GlobalData.Wars;

            // 戦争リストビューの項目を移動する
            int index = warListView.SelectedIndices[0];
            ListViewItem item = warListView.Items[index];
            warListView.Items.RemoveAt(index);
            warListView.Items.Insert(index + 1, item);
            warListView.Items[index + 1].Focused = true;
            warListView.Items[index + 1].Selected = true;
            warListView.EnsureVisible(index + 1);

            // 戦争リストの項目を移動する
            War war = wars[index];
            wars.RemoveAt(index);
            wars.Insert(index + 1, war);

            // 編集済みフラグを設定する
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     戦争の新規ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarNewButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<War> wars = scenario.GlobalData.Wars;

            // 戦争リストに項目を追加する
            War war = new War
            {
                StartDate = new GameDate(),
                EndDate = new GameDate(),
                Id = Scenarios.GetNewTypeId(Scenarios.DefaultWarType, 1),
                Attackers = new Alliance { Id = Scenarios.GetNewTypeId(Scenarios.DefaultWarType, 1) },
                Defenders = new Alliance { Id = Scenarios.GetNewTypeId(Scenarios.DefaultWarType, 1) }
            };
            wars.Add(war);

            // 戦争リストビューに項目を追加する
            ListViewItem item = new ListViewItem { Tag = war };
            item.SubItems.Add("");
            warListView.Items.Add(item);

            // 編集済みフラグを設定する
            war.SetDirty(WarItemId.StartYear);
            war.SetDirty(WarItemId.StartMonth);
            war.SetDirty(WarItemId.StartDay);
            war.SetDirty(WarItemId.EndYear);
            war.SetDirty(WarItemId.EndMonth);
            war.SetDirty(WarItemId.EndDay);
            war.SetDirty(WarItemId.Type);
            war.SetDirty(WarItemId.Id);
            war.SetDirty(WarItemId.AttackerType);
            war.SetDirty(WarItemId.AttackerId);
            war.SetDirty(WarItemId.DefenderType);
            war.SetDirty(WarItemId.DefenderId);
            Scenarios.SetDirty();

            // 追加した項目を選択する
            if (warListView.SelectedIndices.Count > 0)
            {
                ListViewItem prev = warListView.SelectedItems[0];
                prev.Focused = false;
                prev.Selected = false;
            }
            item.Focused = true;
            item.Selected = true;
        }

        /// <summary>
        ///     戦争の削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarRemoveButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<War> wars = scenario.GlobalData.Wars;

            // 枢軸国/連合国/共産国は削除できない
            int index = warListView.SelectedIndices[0];
            if (index < 0)
            {
                return;
            }

            War war = wars[index];

            // typeとidの組を削除する
            Scenarios.RemoveTypeId(war.Id);

            // 戦争リストから項目を削除する
            wars.RemoveAt(index);

            // 戦争リストビューから項目を削除する
            warListView.Items.RemoveAt(index);

            // 編集済みフラグを設定する
            Scenarios.SetDirty();

            // 削除した項目の次を選択する
            if (index >= wars.Count)
            {
                index --;
            }
            warListView.Items[index].Focused = true;
            warListView.Items[index].Selected = true;
        }

        /// <summary>
        ///     戦争開始年テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarStartYearTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(warStartYearTextBox.Text, out val))
            {
                warStartYearTextBox.Text = (war.Id != null) ? IntHelper.ToString(war.Id.Id) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((war.StartDate != null) && (val == war.StartDate.Year))
            {
                return;
            }

            Log.Info("[Scenario] war start year: {0} -> {1} ({2})",
                (war.StartDate != null) ? IntHelper.ToString(war.StartDate.Year) : "", val,
                warListView.SelectedIndices[0]);

            if (war.StartDate == null)
            {
                war.StartDate = new GameDate();

                // 編集済みフラグを設定する
                war.SetDirty(WarItemId.StartMonth);
                war.SetDirty(WarItemId.StartDay);

                // 編集項目を更新する
                warStartMonthTextBox.Text = IntHelper.ToString(war.StartDate.Month);
                warStartDayTextBox.Text = IntHelper.ToString(war.StartDate.Day);

                // 文字色を変更する
                warStartMonthTextBox.ForeColor = Color.Red;
                warStartDayTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            war.StartDate.Year = val;

            // 編集済みフラグを設定する
            war.SetDirty(WarItemId.StartYear);
            Scenarios.SetDirty();

            // 文字色を変更する
            warStartYearTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     戦争開始月テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarStartMonthTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(warStartMonthTextBox.Text, out val))
            {
                warStartMonthTextBox.Text = (war.Id != null) ? IntHelper.ToString(war.Id.Id) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((war.StartDate != null) && (val == war.StartDate.Month))
            {
                return;
            }

            Log.Info("[Scenario] war start month: {0} -> {1} ({2})",
                (war.StartDate != null) ? IntHelper.ToString(war.StartDate.Month) : "", val,
                warListView.SelectedIndices[0]);

            if (war.StartDate == null)
            {
                war.StartDate = new GameDate();

                // 編集済みフラグを設定する
                war.SetDirty(WarItemId.StartYear);
                war.SetDirty(WarItemId.StartDay);

                // 編集項目を更新する
                warStartYearTextBox.Text = IntHelper.ToString(war.StartDate.Year);
                warStartDayTextBox.Text = IntHelper.ToString(war.StartDate.Day);

                // 文字色を変更する
                warStartYearTextBox.ForeColor = Color.Red;
                warStartDayTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            war.StartDate.Month = val;

            // 編集済みフラグを設定する
            war.SetDirty(WarItemId.StartMonth);
            Scenarios.SetDirty();

            // 文字色を変更する
            warStartMonthTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     戦争開始日テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarStartDayTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(warStartDayTextBox.Text, out val))
            {
                warStartDayTextBox.Text = (war.Id != null) ? IntHelper.ToString(war.Id.Id) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((war.StartDate != null) && (val == war.StartDate.Day))
            {
                return;
            }

            Log.Info("[Scenario] war start day: {0} -> {1} ({2})",
                (war.StartDate != null) ? IntHelper.ToString(war.StartDate.Day) : "", val,
                warListView.SelectedIndices[0]);

            if (war.StartDate == null)
            {
                war.StartDate = new GameDate();

                // 編集済みフラグを設定する
                war.SetDirty(WarItemId.StartYear);
                war.SetDirty(WarItemId.StartMonth);

                // 編集項目を更新する
                warStartYearTextBox.Text = IntHelper.ToString(war.StartDate.Year);
                warStartMonthTextBox.Text = IntHelper.ToString(war.StartDate.Month);

                // 文字色を変更する
                warStartYearTextBox.ForeColor = Color.Red;
                warStartMonthTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            war.StartDate.Day = val;

            // 編集済みフラグを設定する
            war.SetDirty(WarItemId.StartDay);
            Scenarios.SetDirty();

            // 文字色を変更する
            warStartDayTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     戦争終了年テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarEndYearTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(warEndYearTextBox.Text, out val))
            {
                warEndYearTextBox.Text = (war.Id != null) ? IntHelper.ToString(war.Id.Id) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((war.EndDate != null) && (val == war.EndDate.Year))
            {
                return;
            }

            Log.Info("[Scenario] war end year: {0} -> {1} ({2})",
                (war.EndDate != null) ? IntHelper.ToString(war.EndDate.Year) : "", val,
                warListView.SelectedIndices[0]);

            if (war.EndDate == null)
            {
                war.EndDate = new GameDate();

                // 編集済みフラグを設定する
                war.SetDirty(WarItemId.EndMonth);
                war.SetDirty(WarItemId.EndDay);

                // 編集項目を更新する
                warEndMonthTextBox.Text = IntHelper.ToString(war.EndDate.Month);
                warEndDayTextBox.Text = IntHelper.ToString(war.EndDate.Day);

                // 文字色を変更する
                warEndMonthTextBox.ForeColor = Color.Red;
                warEndDayTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            war.EndDate.Year = val;

            // 編集済みフラグを設定する
            war.SetDirty(WarItemId.EndYear);
            Scenarios.SetDirty();

            // 文字色を変更する
            warEndYearTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     戦争終了月テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarEndMonthTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(warEndMonthTextBox.Text, out val))
            {
                warEndMonthTextBox.Text = (war.Id != null) ? IntHelper.ToString(war.Id.Id) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((war.EndDate != null) && (val == war.EndDate.Month))
            {
                return;
            }

            Log.Info("[Scenario] war end month: {0} -> {1} ({2})",
                (war.EndDate != null) ? IntHelper.ToString(war.EndDate.Month) : "", val,
                warListView.SelectedIndices[0]);

            if (war.EndDate == null)
            {
                war.EndDate = new GameDate();

                // 編集済みフラグを設定する
                war.SetDirty(WarItemId.EndYear);
                war.SetDirty(WarItemId.EndDay);

                // 編集項目を更新する
                warEndYearTextBox.Text = IntHelper.ToString(war.EndDate.Year);
                warEndDayTextBox.Text = IntHelper.ToString(war.EndDate.Day);

                // 文字色を変更する
                warEndYearTextBox.ForeColor = Color.Red;
                warEndDayTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            war.EndDate.Month = val;

            // 編集済みフラグを設定する
            war.SetDirty(WarItemId.EndMonth);
            Scenarios.SetDirty();

            // 文字色を変更する
            warEndMonthTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     戦争終了日テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarEndDayTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(warEndDayTextBox.Text, out val))
            {
                warEndDayTextBox.Text = (war.Id != null) ? IntHelper.ToString(war.Id.Id) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((war.EndDate != null) && (val == war.EndDate.Day))
            {
                return;
            }

            Log.Info("[Scenario] war end day: {0} -> {1} ({2})",
                (war.EndDate != null) ? IntHelper.ToString(war.EndDate.Day) : "", val,
                warListView.SelectedIndices[0]);

            if (war.EndDate == null)
            {
                war.EndDate = new GameDate();

                // 編集済みフラグを設定する
                war.SetDirty(WarItemId.EndYear);
                war.SetDirty(WarItemId.EndMonth);

                // 編集項目を更新する
                warEndYearTextBox.Text = IntHelper.ToString(war.EndDate.Year);
                warEndMonthTextBox.Text = IntHelper.ToString(war.EndDate.Month);

                // 文字色を変更する
                warEndYearTextBox.ForeColor = Color.Red;
                warEndMonthTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            war.EndDate.Day = val;

            // 編集済みフラグを設定する
            war.SetDirty(WarItemId.EndDay);
            Scenarios.SetDirty();

            // 文字色を変更する
            warEndDayTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     戦争のtypeテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarTypeTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(warTypeTextBox.Text, out val))
            {
                warTypeTextBox.Text = (war.Id != null) ? IntHelper.ToString(war.Id.Type) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((war.Id != null) && (val == war.Id.Type))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if ((war.Id != null) && Scenarios.ExistsTypeId(val, war.Id.Id))
            {
                warTypeTextBox.Text = IntHelper.ToString(war.Id.Type);
                return;
            }

            Log.Info("[Scenario] war type: {0} -> {1} ({2})", (war.Id != null) ? IntHelper.ToString(war.Id.Type) : "",
                val, warListView.SelectedIndices[0]);

            if (war.Id != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(war.Id);

                // 値を更新する
                war.Id.Type = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(war.Id);
            }
            else
            {
                // 値を更新する
                war.Id = Scenarios.GetNewTypeId(Scenarios.DefaultWarType, 1);

                // 編集済みフラグを設定する
                war.SetDirty(WarItemId.Id);

                // 編集項目を更新する
                warIdTextBox.Text = IntHelper.ToString(war.Id.Id);

                // 文字色を変更する
                warIdTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            war.SetDirty(WarItemId.Type);
            Scenarios.SetDirty();

            // 文字色を変更する
            warTypeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     戦争のidテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarIdTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(warIdTextBox.Text, out val))
            {
                warIdTextBox.Text = (war.Id != null) ? IntHelper.ToString(war.Id.Id) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((war.Id != null) && (val == war.Id.Id))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if (Scenarios.ExistsTypeId((war.Id != null) ? war.Id.Type : Scenarios.DefaultWarType, val))
            {
                warIdTextBox.Text = (war.Id != null) ? IntHelper.ToString(war.Id.Id) : "";
                return;
            }

            Log.Info("[Scenario] war id: {0} -> {1} ({2})", (war.Id != null) ? IntHelper.ToString(war.Id.Id) : "", val,
                warListView.SelectedIndices[0]);

            if (war.Id != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(war.Id);

                // 値を更新する
                war.Id.Id = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(war.Id);
            }
            else
            {
                // 値を更新する
                war.Id = Scenarios.GetNewTypeId(Scenarios.DefaultWarType, val);

                // 編集済みフラグを設定する
                war.SetDirty(WarItemId.Type);

                // 編集項目を更新する
                warTypeTextBox.Text = IntHelper.ToString(war.Id.Type);

                // 文字色を変更する
                warTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            war.SetDirty(WarItemId.Id);
            Scenarios.SetDirty();

            // 文字色を変更する
            warIdTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     戦争の攻撃側typeテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarAttackerTypeTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(warAttackerTypeTextBox.Text, out val))
            {
                warAttackerTypeTextBox.Text = ((war.Attackers != null) && (war.Attackers.Id != null))
                    ? IntHelper.ToString(war.Attackers.Id.Type)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((war.Attackers != null) && (war.Attackers.Id != null) && (val == war.Attackers.Id.Type))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if ((war.Attackers != null) && (war.Attackers.Id != null) &&
                Scenarios.ExistsTypeId(val, war.Attackers.Id.Id))
            {
                warAttackerTypeTextBox.Text = ((war.Attackers != null) && (war.Attackers.Id != null))
                    ? IntHelper.ToString(war.Attackers.Id.Type)
                    : "";
                return;
            }

            Log.Info("[Scenario] war attacker type: {0} -> {1} ({2})",
                ((war.Attackers != null) && (war.Attackers.Id != null)) ? IntHelper.ToString(war.Attackers.Id.Type) : "",
                val, warListView.SelectedIndices[0]);

            if (war.Attackers == null)
            {
                war.Attackers = new Alliance();
            }
            if (war.Attackers.Id != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(war.Attackers.Id);

                // 値を更新する
                war.Attackers.Id.Type = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(war.Attackers.Id);
            }
            else
            {
                // 値を更新する
                war.Attackers.Id = Scenarios.GetNewTypeId(Scenarios.DefaultWarType, 1);

                // 編集済みフラグを設定する
                war.SetDirty(WarItemId.AttackerId);

                // 編集項目を更新する
                warAttackerIdTextBox.Text = IntHelper.ToString(war.Attackers.Id.Id);

                // 文字色を変更する
                warAttackerIdTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            war.SetDirty(WarItemId.AttackerType);
            Scenarios.SetDirty();

            // 文字色を変更する
            warAttackerTypeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     戦争の攻撃側idテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarAttackerIdTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(warAttackerIdTextBox.Text, out val))
            {
                warAttackerIdTextBox.Text = ((war.Attackers != null) && (war.Attackers.Id != null))
                    ? IntHelper.ToString(war.Attackers.Id.Id)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((war.Attackers != null) && (war.Attackers.Id != null) && (val == war.Attackers.Id.Id))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if (Scenarios.ExistsTypeId((war.Attackers != null) && (war.Attackers.Id != null)
                ? war.Attackers.Id.Type
                : Scenarios.DefaultWarType, val))
            {
                warAttackerIdTextBox.Text = ((war.Attackers != null) && (war.Attackers.Id != null))
                    ? IntHelper.ToString(war.Attackers.Id.Id)
                    : "";
                return;
            }

            Log.Info("[Scenario] war attacker id: {0} -> {1} ({2})",
                ((war.Attackers != null) && (war.Attackers.Id != null)) ? IntHelper.ToString(war.Attackers.Id.Id) : "",
                val, warListView.SelectedIndices[0]);

            if (war.Attackers == null)
            {
                war.Attackers = new Alliance();
            }
            if (war.Attackers.Id != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(war.Attackers.Id);

                // 値を更新する
                war.Attackers.Id.Id = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(war.Attackers.Id);
            }
            else
            {
                // 値を更新する
                war.Attackers.Id = Scenarios.GetNewTypeId(Scenarios.DefaultWarType, val);

                // 編集済みフラグを設定する
                war.SetDirty(WarItemId.AttackerType);

                // 編集項目を更新する
                warAttackerTypeTextBox.Text = IntHelper.ToString(war.Attackers.Id.Type);

                // 文字色を変更する
                warAttackerTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            war.SetDirty(WarItemId.AttackerId);
            Scenarios.SetDirty();

            // 文字色を変更する
            warAttackerIdTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     戦争の防御側typeテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarDefenderTypeTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(warDefenderTypeTextBox.Text, out val))
            {
                warDefenderTypeTextBox.Text = ((war.Defenders != null) && (war.Defenders.Id != null))
                    ? IntHelper.ToString(war.Defenders.Id.Type)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((war.Defenders != null) && (war.Defenders.Id != null) && (val == war.Defenders.Id.Type))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if ((war.Defenders != null) && (war.Defenders.Id != null) &&
                Scenarios.ExistsTypeId(val, war.Defenders.Id.Id))
            {
                warDefenderTypeTextBox.Text = ((war.Defenders != null) && (war.Defenders.Id != null))
                    ? IntHelper.ToString(war.Defenders.Id.Type)
                    : "";
                return;
            }

            Log.Info("[Scenario] war defender type: {0} -> {1} ({2})",
                ((war.Defenders != null) && (war.Defenders.Id != null)) ? IntHelper.ToString(war.Defenders.Id.Type) : "",
                val, warListView.SelectedIndices[0]);

            if (war.Defenders == null)
            {
                war.Defenders = new Alliance();
            }
            if (war.Defenders.Id != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(war.Defenders.Id);

                // 値を更新する
                war.Defenders.Id.Type = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(war.Defenders.Id);
            }
            else
            {
                // 値を更新する
                war.Defenders.Id = Scenarios.GetNewTypeId(Scenarios.DefaultWarType, 1);

                // 編集済みフラグを設定する
                war.SetDirty(WarItemId.DefenderId);

                // 編集項目を更新する
                warDefenderIdTextBox.Text = IntHelper.ToString(war.Defenders.Id.Id);

                // 文字色を変更する
                warDefenderIdTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            war.SetDirty(WarItemId.DefenderType);
            Scenarios.SetDirty();

            // 文字色を変更する
            warDefenderTypeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     戦争の防御側idテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarDefenderIdTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(warDefenderIdTextBox.Text, out val))
            {
                warDefenderIdTextBox.Text = ((war.Defenders != null) && (war.Defenders.Id != null))
                    ? IntHelper.ToString(war.Defenders.Id.Id)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((war.Defenders != null) && (war.Defenders.Id != null) && (val == war.Defenders.Id.Id))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if (Scenarios.ExistsTypeId((war.Defenders != null) && (war.Defenders.Id != null)
                ? war.Defenders.Id.Type
                : Scenarios.DefaultWarType, val))
            {
                warDefenderIdTextBox.Text = ((war.Defenders != null) && (war.Defenders.Id != null))
                    ? IntHelper.ToString(war.Defenders.Id.Id)
                    : "";
                return;
            }

            Log.Info("[Scenario] war defender id: {0} -> {1} ({2})",
                ((war.Defenders != null) && (war.Defenders.Id != null)) ? IntHelper.ToString(war.Defenders.Id.Id) : "",
                val, warListView.SelectedIndices[0]);

            if (war.Defenders == null)
            {
                war.Defenders = new Alliance();
            }
            if (war.Defenders.Id != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(war.Defenders.Id);

                // 値を更新する
                war.Defenders.Id.Id = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(war.Defenders.Id);
            }
            else
            {
                // 値を更新する
                war.Defenders.Id = Scenarios.GetNewTypeId(Scenarios.DefaultWarType, val);

                // 編集済みフラグを設定する
                war.SetDirty(WarItemId.DefenderType);

                // 編集項目を更新する
                warDefenderTypeTextBox.Text = IntHelper.ToString(war.Defenders.Id.Type);

                // 文字色を変更する
                warDefenderTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            war.SetDirty(WarItemId.DefenderId);
            Scenarios.SetDirty();

            // 文字色を変更する
            warDefenderIdTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     戦争攻撃側参加国リストボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarAttackerListBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index < 0)
            {
                return;
            }
            ListBox listBox = sender as ListBox;
            if (listBox == null)
            {
                return;
            }

            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目を描画する
            Brush brush;
            if ((e.State & DrawItemState.Selected) == 0)
            {
                // 変更ありの項目は文字色を変更する
                bool dirty = war.IsDirtyCountry(war.Attackers.Participant[e.Index]);
                brush = new SolidBrush(dirty ? Color.Red : listBox.ForeColor);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            string s = listBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     戦争防御側参加国リストボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarDefenderListBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index < 0)
            {
                return;
            }
            ListBox listBox = sender as ListBox;
            if (listBox == null)
            {
                return;
            }

            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目を描画する
            Brush brush;
            if ((e.State & DrawItemState.Selected) == 0)
            {
                // 変更ありの項目は文字色を変更する
                bool dirty = war.IsDirtyCountry(war.Defenders.Participant[e.Index]);
                brush = new SolidBrush(dirty ? Color.Red : listBox.ForeColor);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            string s = listBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     戦争非参加国リストボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarCountryListBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index < 0)
            {
                return;
            }
            ListBox listBox = sender as ListBox;
            if (listBox == null)
            {
                return;
            }

            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目を描画する
            Brush brush;
            if ((e.State & DrawItemState.Selected) == 0)
            {
                // 変更ありの項目は文字色を変更する
                bool dirty = war.IsDirtyCountry(_warFreeCountries[e.Index]);
                brush = new SolidBrush(dirty ? Color.Red : listBox.ForeColor);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            string s = listBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     戦争攻撃側参加国リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarAttackerListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            int count = warAttackerListBox.SelectedIndices.Count;
            int index = warAttackerListBox.SelectedIndex;
            warAttackerRemoveButton.Enabled = (count > 0);
            warAttackerLeaderButton.Enabled = ((count == 1) && (index > 0));
        }

        /// <summary>
        ///     戦争防御側参加国リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarDefenderListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            int count = warDefenderListBox.SelectedIndices.Count;
            int index = warDefenderListBox.SelectedIndex;
            warDefenderRemoveButton.Enabled = (count > 0);
            warDefenderLeaderButton.Enabled = ((count == 1) && (index > 0));
        }

        /// <summary>
        ///     戦争非参加国リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarCountryListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            int count = warCountryListBox.SelectedIndices.Count;
            warAttackerAddButton.Enabled = (count > 0);
            warDefenderAddButton.Enabled = (count > 0);
        }

        /// <summary>
        ///     戦争攻撃側参加国の追加ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarAttackerAddButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            List<Country> countries =
                (from int index in warCountryListBox.SelectedIndices select _warFreeCountries[index]).ToList();
            warAttackerListBox.BeginUpdate();
            warCountryListBox.BeginUpdate();
            foreach (Country country in countries)
            {
                // 戦争攻撃側参加国リストボックスに追加する
                warAttackerListBox.Items.Add(Countries.GetTagName(country));

                // 戦争攻撃側参加国リストに追加する
                war.Attackers.Participant.Add(country);

                // 戦争非参加国リストボックスから削除する
                int index = _warFreeCountries.IndexOf(country);
                warCountryListBox.Items.RemoveAt(index);
                _warFreeCountries.RemoveAt(index);

                // 編集済みフラグを設定する
                war.SetDirtyCountry(country);
                Scenarios.SetDirty();

                // 戦争リストビューの項目を更新する
                warListView.SelectedItems[0].SubItems[2].Text = Countries.GetNameList(war.Attackers.Participant);

                Log.Info("[Scenario] war attacker: +{0} ({1})", Countries.Strings[(int) country],
                    warListView.SelectedIndices[0]);
            }
            warAttackerListBox.EndUpdate();
            warCountryListBox.EndUpdate();
        }

        /// <summary>
        ///     戦争攻撃側参加国の削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarAttackerRemoveButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            List<Country> countries =
                (from int index in warAttackerListBox.SelectedIndices select war.Attackers.Participant[index]).ToList();
            warAttackerListBox.BeginUpdate();
            warCountryListBox.BeginUpdate();
            foreach (Country country in countries)
            {
                // 戦争非参加国リストボックスに追加する
                int index = _warFreeCountries.FindIndex(c => c > country);
                if (index < 0)
                {
                    index = _warFreeCountries.Count;
                }
                warCountryListBox.Items.Insert(index, Countries.GetTagName(country));
                _warFreeCountries.Insert(index, country);

                // 戦争攻撃側参加国リストボックスから削除する
                index = war.Attackers.Participant.IndexOf(country);
                warAttackerListBox.Items.RemoveAt(index);

                // 戦争攻撃側参加国リストから削除する
                war.Attackers.Participant.Remove(country);

                // 編集済みフラグを設定する
                war.SetDirtyCountry(country);
                Scenarios.SetDirty();

                // 戦争リストビューの項目を更新する
                warListView.SelectedItems[0].SubItems[2].Text = Countries.GetNameList(war.Attackers.Participant);

                Log.Info("[Scenario] war attacker: -{0} ({1})", Countries.Strings[(int) country],
                    warListView.SelectedIndices[0]);
            }
            warAttackerListBox.EndUpdate();
            warCountryListBox.EndUpdate();
        }

        /// <summary>
        ///     戦争防御側参加国の追加ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarDefenderAddButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            List<Country> countries =
                (from int index in warCountryListBox.SelectedIndices select _warFreeCountries[index]).ToList();
            warDefenderListBox.BeginUpdate();
            warCountryListBox.BeginUpdate();
            foreach (Country country in countries)
            {
                // 戦争防御側参加国リストボックスに追加する
                warDefenderListBox.Items.Add(Countries.GetTagName(country));

                // 戦争防御側参加国リストに追加する
                war.Defenders.Participant.Add(country);

                // 戦争非参加国リストボックスから削除する
                int index = _warFreeCountries.IndexOf(country);
                warCountryListBox.Items.RemoveAt(index);
                _warFreeCountries.RemoveAt(index);

                // 編集済みフラグを設定する
                war.SetDirtyCountry(country);
                Scenarios.SetDirty();

                // 戦争リストビューの項目を更新する
                warListView.SelectedItems[0].SubItems[3].Text = Countries.GetNameList(war.Defenders.Participant);

                Log.Info("[Scenario] war defender: +{0} ({1})", Countries.Strings[(int) country],
                    warListView.SelectedIndices[0]);
            }
            warDefenderListBox.EndUpdate();
            warCountryListBox.EndUpdate();
        }

        /// <summary>
        ///     戦争防御側参加国の削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarDefenderRemoveButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            List<Country> countries =
                (from int index in warDefenderListBox.SelectedIndices select war.Defenders.Participant[index]).ToList();
            warDefenderListBox.BeginUpdate();
            warCountryListBox.BeginUpdate();
            foreach (Country country in countries)
            {
                // 戦争非参加国リストボックスに追加する
                int index = _warFreeCountries.FindIndex(c => c > country);
                if (index < 0)
                {
                    index = _warFreeCountries.Count;
                }
                warCountryListBox.Items.Insert(index, Countries.GetTagName(country));
                _warFreeCountries.Insert(index, country);

                // 戦争防御側参加国リストボックスから削除する
                index = war.Defenders.Participant.IndexOf(country);
                warDefenderListBox.Items.RemoveAt(index);

                // 戦争防御側参加国リストから削除する
                war.Defenders.Participant.Remove(country);

                // 編集済みフラグを設定する
                war.SetDirtyCountry(country);
                Scenarios.SetDirty();

                // 戦争リストビューの項目を更新する
                warListView.SelectedItems[0].SubItems[3].Text = Countries.GetNameList(war.Defenders.Participant);

                Log.Info("[Scenario] war defender: -{0} ({1})", Countries.Strings[(int) country],
                    warListView.SelectedIndices[0]);
            }
            warDefenderListBox.EndUpdate();
            warCountryListBox.EndUpdate();
        }

        /// <summary>
        ///     戦争攻撃側のリーダーに設定ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarAttackerLeaderButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            int index = warAttackerListBox.SelectedIndex;
            Country country = war.Attackers.Participant[index];

            // 戦争攻撃側参加国リストボックスの先頭に移動する
            warAttackerListBox.BeginUpdate();
            warAttackerListBox.Items.RemoveAt(index);
            warAttackerListBox.Items.Insert(0, Countries.GetTagName(country));
            warAttackerListBox.EndUpdate();

            // 戦争攻撃側参加国リストの先頭に移動する
            war.Attackers.Participant.RemoveAt(index);
            war.Attackers.Participant.Insert(0, country);

            // 編集済みフラグを設定する
            war.SetDirtyCountry(country);
            Scenarios.SetDirty();

            // 戦争リストビューの項目を更新する
            warListView.SelectedItems[0].SubItems[2].Text = Countries.GetNameList(war.Attackers.Participant);

            Log.Info("[Scenario] war attacker leader: {0} ({1})", Countries.Strings[(int) country],
                warListView.SelectedIndices[0]);
        }

        /// <summary>
        ///     戦争防御側のリーダーに設定ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarDefenderLeaderButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            int index = warDefenderListBox.SelectedIndex;
            Country country = war.Defenders.Participant[index];

            // 戦争防御側参加国リストボックスの先頭に移動する
            warDefenderListBox.BeginUpdate();
            warDefenderListBox.Items.RemoveAt(index);
            warDefenderListBox.Items.Insert(0, Countries.GetTagName(country));
            warDefenderListBox.EndUpdate();

            // 戦争防御側参加国リストの先頭に移動する
            war.Defenders.Participant.RemoveAt(index);
            war.Defenders.Participant.Insert(0, country);

            // 編集済みフラグを設定する
            war.SetDirtyCountry(country);
            Scenarios.SetDirty();

            // 戦争リストビューの項目を更新する
            warListView.SelectedItems[0].SubItems[3].Text = Countries.GetNameList(war.Defenders.Participant);

            Log.Info("[Scenario] war defender leader: {0} ({1})", Countries.Strings[(int) country],
                warListView.SelectedIndices[0]);
        }

        /// <summary>
        ///     選択中の戦争情報を取得する
        /// </summary>
        /// <returns>選択中の戦争情報</returns>
        private War GetSelectedWar()
        {
            return (warListView.SelectedItems.Count > 0) ? warListView.SelectedItems[0].Tag as War : null;
        }

        #endregion

        #endregion

        #region 関係タブ

        #region 関係タブ - 共通

        /// <summary>
        ///     関係タブの編集項目を初期化する
        /// </summary>
        private void InitRelationTab()
        {
            // 選択国リストボックス
            relationCountryListBox.BeginUpdate();
            relationCountryListBox.Items.Clear();
            foreach (string s in Countries.Tags.Select(Countries.GetTagName))
            {
                relationCountryListBox.Items.Add(s);
            }
            relationCountryListBox.EndUpdate();
        }

        /// <summary>
        ///     関係タブの編集項目を更新する
        /// </summary>
        private void UpdateRelationTab()
        {
            // 編集項目を無効化する
            DisableRelationItems();

            // 国家リストボックスを有効化する
            relationCountryListBox.Enabled = true;

            // 関係リストビューを有効化する
            relationListView.Enabled = true;
        }

        /// <summary>
        ///     国家リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRelationCountryListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (relationCountryListBox.SelectedIndex < 0)
            {
                return;
            }

            // 関係リストビューを更新する
            UpdateRelationListView();
        }

        /// <summary>
        ///     関係リストビューを更新する
        /// </summary>
        private void UpdateRelationListView()
        {
            Country self = GetSelectedRelationCountry();
            CountrySettings settings = Scenarios.GetCountrySettings(self);

            relationListView.BeginUpdate();
            relationListView.Items.Clear();
            foreach (Country target in Countries.Tags)
            {
                ListViewItem item = new ListViewItem(Countries.GetTagName(target));
                Relation relation = Scenarios.GetCountryRelation(self, target);
                Treaty nonAggression = Scenarios.GetNonAggression(self, target);
                Treaty peace = Scenarios.GetPeace(self, target);
                SpySettings spy = Scenarios.GetCountryIntelligence(self, target);
                item.SubItems.Add((relation != null) ? DoubleHelper.ToString(relation.Value) : "0");
                bool flag = (settings != null);
                item.SubItems.Add((flag && (settings.Master == target)) ? Resources.Yes : "");
                item.SubItems.Add((flag && (settings.Control == target)) ? Resources.Yes : "");
                flag = (relation != null);
                item.SubItems.Add((flag && relation.Access) ? Resources.Yes : "");
                item.SubItems.Add((flag && (relation.Guaranteed != null)) ? Resources.Yes : "");
                item.SubItems.Add((nonAggression != null) ? Resources.Yes : "");
                item.SubItems.Add((peace != null) ? Resources.Yes : "");
                item.SubItems.Add((spy != null) ? IntHelper.ToString(spy.Spies) : "");
                relationListView.Items.Add(item);
            }
            relationListView.EndUpdate();
        }

        /// <summary>
        ///     関係リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRelationListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ編集項目を無効化する
            if ((relationListView.SelectedIndices.Count == 0) ||
                (relationCountryListBox.SelectedIndex < 0))
            {
                DisableRelationItems();
                return;
            }

            // 編集項目を更新する
            UpdateRelationItems();
        }

        /// <summary>
        ///     関係タブの編集項目を無効化する
        /// </summary>
        private void DisableRelationItems()
        {
            // 編集項目を無効化する
            diplomacyGroupBox.Enabled = false;
            intelligenceGroupBox.Enabled = false;

            // 編集項目をクリアする
            relationValueNumericUpDown.Value = 0;
            masterCheckBox.Checked = false;
            controlCheckBox.Checked = false;
            accessCheckBox.Checked = false;

            guaranteeCheckBox.Checked = false;
            guaranteeYearTextBox.Text = "";
            guaranteeMonthTextBox.Text = "";
            guaranteeDayTextBox.Text = "";

            nonAggressionCheckBox.Checked = false;
            nonAggressionStartYearTextBox.Text = "";
            nonAggressionStartMonthTextBox.Text = "";
            nonAggressionStartDayTextBox.Text = "";
            nonAggressionEndYearTextBox.Text = "";
            nonAggressionEndMonthTextBox.Text = "";
            nonAggressionEndDayTextBox.Text = "";
            nonAggressionTypeTextBox.Text = "";
            nonAggressionIdTextBox.Text = "";

            peaceCheckBox.Checked = false;
            peaceStartYearTextBox.Text = "";
            peaceStartMonthTextBox.Text = "";
            peaceStartDayTextBox.Text = "";
            peaceEndYearTextBox.Text = "";
            peaceEndMonthTextBox.Text = "";
            peaceEndDayTextBox.Text = "";
            peaceTypeTextBox.Text = "";
            peaceIdTextBox.Text = "";

            spyNumNumericUpDown.Value = 0;
        }

        /// <summary>
        ///     関係タブの編集項目を更新する
        /// </summary>
        private void UpdateRelationItems()
        {
            // 外交情報の編集項目を更新する
            UpdateDiplomacyItems();

            // 諜報情報の編集項目を更新する
            UpdateIntelligenceItems();

            // 編集項目を有効化する
            diplomacyGroupBox.Enabled = true;
            intelligenceGroupBox.Enabled = true;
        }

        /// <summary>
        ///     外交情報の編集項目を更新する
        /// </summary>
        private void UpdateDiplomacyItems()
        {
            Country self = GetSelectedRelationCountry();
            Country target = GetSelectedRelationTarget();
            CountrySettings settings = Scenarios.GetCountrySettings(self);
            Relation relation = Scenarios.GetCountryRelation(self, target);
            Treaty nonAggression = Scenarios.GetNonAggression(self, target);
            Treaty peace = Scenarios.GetPeace(self, target);

            bool flag = (relation != null);
            relationValueNumericUpDown.Value = flag ? (decimal) relation.Value : 0;
            relationValueNumericUpDown.ForeColor = (flag && relation.IsDirty(RelationItemId.Value))
                ? Color.Red
                : SystemColors.WindowText;

            flag = (settings != null);
            masterCheckBox.Checked = flag && (settings.Master == target);
            controlCheckBox.Checked = flag && (settings.Control == target);
            masterCheckBox.ForeColor = (flag && settings.IsDirty(CountrySettingsItemId.Master))
                ? Color.Red
                : SystemColors.WindowText;
            controlCheckBox.ForeColor = (flag && settings.IsDirty(CountrySettingsItemId.Control))
                ? Color.Red
                : SystemColors.WindowText;

            flag = (relation != null);
            accessCheckBox.Checked = flag && relation.Access;

            accessCheckBox.ForeColor = (flag && relation.IsDirty(RelationItemId.Access))
                ? Color.Red
                : SystemColors.WindowText;

            UpdateGuaranteeItems(relation);
            UpdateNonAggressionItems(nonAggression);
            UpdatePeaceItems(peace);
        }

        /// <summary>
        ///     関係タブで選択中の対象国を取得する
        /// </summary>
        /// <returns>対象国</returns>
        private Country GetSelectedRelationCountry()
        {
            return (relationCountryListBox.SelectedIndex >= 0)
                ? Countries.Tags[relationCountryListBox.SelectedIndex]
                : Country.None;
        }

        /// <summary>
        ///     関係タブで選択中の相手国を取得する
        /// </summary>
        /// <returns>相手国</returns>
        private Country GetSelectedRelationTarget()
        {
            return (relationListView.SelectedItems.Count > 0)
                ? Countries.Tags[relationListView.SelectedIndices[0]]
                : Country.None;
        }

        #endregion

        #region 関係タブ - 外交

        /// <summary>
        ///     独立保障グループボックスの編集項目を更新する
        /// </summary>
        /// <param name="relation">国家関係</param>
        private void UpdateGuaranteeItems(Relation relation)
        {
            bool flag = (relation != null) && (relation.Guaranteed != null);
            guaranteeCheckBox.Checked = flag;
            guaranteeYearTextBox.Text = flag ? IntHelper.ToString(relation.Guaranteed.Year) : "";
            guaranteeMonthTextBox.Text = flag ? IntHelper.ToString(relation.Guaranteed.Month) : "";
            guaranteeDayTextBox.Text = flag ? IntHelper.ToString(relation.Guaranteed.Day) : "";

            guaranteeCheckBox.ForeColor = ((relation != null) && relation.IsDirty(RelationItemId.Guaranteed))
                ? Color.Red
                : SystemColors.WindowText;
            guaranteeYearTextBox.ForeColor = (flag && relation.IsDirty(RelationItemId.GuaranteedYear))
                ? Color.Red
                : SystemColors.WindowText;
            guaranteeMonthTextBox.ForeColor = (flag && relation.IsDirty(RelationItemId.GuaranteedMonth))
                ? Color.Red
                : SystemColors.WindowText;
            guaranteeDayTextBox.ForeColor = (flag && relation.IsDirty(RelationItemId.GuaranteedDay))
                ? Color.Red
                : SystemColors.WindowText;

            guaranteeEndLabel.Enabled = flag;
            guaranteeYearTextBox.Enabled = flag;
            guaranteeMonthTextBox.Enabled = flag;
            guaranteeDayTextBox.Enabled = flag;
        }

        /// <summary>
        ///     不可侵条約グループボックスの編集項目を更新する
        /// </summary>
        /// <param name="nonAggression">不可侵条約</param>
        private void UpdateNonAggressionItems(Treaty nonAggression)
        {
            bool flag = (nonAggression != null);
            nonAggressionCheckBox.Checked = flag;
            nonAggressionCheckBox.ForeColor = (flag && nonAggression.IsDirty()) ? Color.Red : SystemColors.WindowText;

            flag = (nonAggression != null) && (nonAggression.StartDate != null);
            nonAggressionStartYearTextBox.Text = flag ? IntHelper.ToString(nonAggression.StartDate.Year) : "";
            nonAggressionStartMonthTextBox.Text = flag ? IntHelper.ToString(nonAggression.StartDate.Month) : "";
            nonAggressionStartDayTextBox.Text = flag ? IntHelper.ToString(nonAggression.StartDate.Day) : "";

            nonAggressionStartYearTextBox.ForeColor = (flag && nonAggression.IsDirty(TreatyItemId.StartYear))
                ? Color.Red
                : SystemColors.WindowText;
            nonAggressionStartMonthTextBox.ForeColor = (flag && nonAggression.IsDirty(TreatyItemId.StartMonth))
                ? Color.Red
                : SystemColors.WindowText;
            nonAggressionStartDayTextBox.ForeColor = (flag && nonAggression.IsDirty(TreatyItemId.StartDay))
                ? Color.Red
                : SystemColors.WindowText;

            nonAggressionStartLabel.Enabled = flag;
            nonAggressionStartYearTextBox.Enabled = flag;
            nonAggressionStartMonthTextBox.Enabled = flag;
            nonAggressionStartDayTextBox.Enabled = flag;

            flag = (nonAggression != null) && (nonAggression.EndDate != null);
            nonAggressionEndYearTextBox.Text = flag ? IntHelper.ToString(nonAggression.EndDate.Year) : "";
            nonAggressionEndMonthTextBox.Text = flag ? IntHelper.ToString(nonAggression.EndDate.Month) : "";
            nonAggressionEndDayTextBox.Text = flag ? IntHelper.ToString(nonAggression.EndDate.Day) : "";

            nonAggressionEndYearTextBox.ForeColor = (flag && nonAggression.IsDirty(TreatyItemId.EndYear))
                ? Color.Red
                : SystemColors.WindowText;
            nonAggressionEndMonthTextBox.ForeColor = (flag && nonAggression.IsDirty(TreatyItemId.EndMonth))
                ? Color.Red
                : SystemColors.WindowText;
            nonAggressionEndDayTextBox.ForeColor = (flag && nonAggression.IsDirty(TreatyItemId.EndDay))
                ? Color.Red
                : SystemColors.WindowText;

            nonAggressionEndLabel.Enabled = flag;
            nonAggressionEndYearTextBox.Enabled = flag;
            nonAggressionEndMonthTextBox.Enabled = flag;
            nonAggressionEndDayTextBox.Enabled = flag;

            flag = (nonAggression != null) && (nonAggression.Id != null);
            nonAggressionTypeTextBox.Text = flag ? IntHelper.ToString(nonAggression.Id.Type) : "";
            nonAggressionIdTextBox.Text = flag ? IntHelper.ToString(nonAggression.Id.Id) : "";

            nonAggressionTypeTextBox.ForeColor = (flag && nonAggression.IsDirty(TreatyItemId.Type))
                ? Color.Red
                : SystemColors.WindowText;
            nonAggressionIdTextBox.ForeColor = (flag && nonAggression.IsDirty(TreatyItemId.Id))
                ? Color.Red
                : SystemColors.WindowText;

            nonAggressionIdLabel.Enabled = flag;
            nonAggressionTypeTextBox.Enabled = flag;
            nonAggressionIdTextBox.Enabled = flag;
        }

        /// <summary>
        ///     講和条約グループボックスの編集項目を更新する
        /// </summary>
        /// <param name="peace">講和条約</param>
        private void UpdatePeaceItems(Treaty peace)
        {
            bool flag = (peace != null);
            peaceCheckBox.Checked = flag;
            peaceCheckBox.ForeColor = (flag && peace.IsDirty()) ? Color.Red : SystemColors.WindowText;

            flag = (peace != null) && (peace.StartDate != null);
            peaceStartYearTextBox.Text = flag ? IntHelper.ToString(peace.StartDate.Year) : "";
            peaceStartMonthTextBox.Text = flag ? IntHelper.ToString(peace.StartDate.Month) : "";
            peaceStartDayTextBox.Text = flag ? IntHelper.ToString(peace.StartDate.Day) : "";

            peaceStartYearTextBox.ForeColor = (flag && peace.IsDirty(TreatyItemId.StartYear))
                ? Color.Red
                : SystemColors.WindowText;
            peaceStartMonthTextBox.ForeColor = (flag && peace.IsDirty(TreatyItemId.StartMonth))
                ? Color.Red
                : SystemColors.WindowText;
            peaceStartDayTextBox.ForeColor = (flag && peace.IsDirty(TreatyItemId.StartDay))
                ? Color.Red
                : SystemColors.WindowText;

            peaceStartLabel.Enabled = flag;
            peaceStartYearTextBox.Enabled = flag;
            peaceStartMonthTextBox.Enabled = flag;
            peaceStartDayTextBox.Enabled = flag;

            flag = (peace != null) && (peace.EndDate != null);
            peaceEndYearTextBox.Text = flag ? IntHelper.ToString(peace.EndDate.Year) : "";
            peaceEndMonthTextBox.Text = flag ? IntHelper.ToString(peace.EndDate.Month) : "";
            peaceEndDayTextBox.Text = flag ? IntHelper.ToString(peace.EndDate.Day) : "";

            peaceEndYearTextBox.ForeColor = (flag && peace.IsDirty(TreatyItemId.EndYear))
                ? Color.Red
                : SystemColors.WindowText;
            peaceEndMonthTextBox.ForeColor = (flag && peace.IsDirty(TreatyItemId.EndMonth))
                ? Color.Red
                : SystemColors.WindowText;
            peaceEndDayTextBox.ForeColor = (flag && peace.IsDirty(TreatyItemId.EndDay))
                ? Color.Red
                : SystemColors.WindowText;

            peaceEndLabel.Enabled = flag;
            peaceEndYearTextBox.Enabled = flag;
            peaceEndMonthTextBox.Enabled = flag;
            peaceEndDayTextBox.Enabled = flag;

            flag = (peace != null) && (peace.Id != null);
            peaceTypeTextBox.Text = flag ? IntHelper.ToString(peace.Id.Type) : "";
            peaceIdTextBox.Text = flag ? IntHelper.ToString(peace.Id.Id) : "";

            peaceTypeTextBox.ForeColor = (flag && peace.IsDirty(TreatyItemId.Type))
                ? Color.Red
                : SystemColors.WindowText;
            peaceIdTextBox.ForeColor = (flag && peace.IsDirty(TreatyItemId.Id))
                ? Color.Red
                : SystemColors.WindowText;

            peaceIdLabel.Enabled = flag;
            peaceTypeTextBox.Enabled = flag;
            peaceIdTextBox.Enabled = flag;
        }

        /// <summary>
        ///     関係値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRelationValueNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country self = GetSelectedRelationCountry();
            if (self == Country.None)
            {
                return;
            }
            Country target = GetSelectedRelationTarget();
            if (target == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(self);
            Relation relation = Scenarios.GetCountryRelation(self, target);

            // 値に変化がなければ何もしない
            double val = (double) relationValueNumericUpDown.Value;
            if ((relation != null) && DoubleHelper.IsEqual(val, relation.Value))
            {
                return;
            }

            Log.Info("[Scenario] relation value: {0} -> {1} ({2} > {3})",
                (relation != null) ? DoubleHelper.ToString(relation.Value) : "", DoubleHelper.ToString(val),
                Countries.Strings[(int) self], Countries.Strings[(int) target]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = self };
                Scenarios.SetCountrySettings(settings);
            }
            if (relation == null)
            {
                relation = new Relation { Country = target };
                settings.Relations.Add(relation);
                Scenarios.SetCountryRelation(self, relation);
            }

            // 値を更新する
            relation.Value = val;

            // 編集済みフラグを設定する
            relation.SetDirty(RelationItemId.Value);
            settings.SetDirty();
            Scenarios.SetDirty();

            // 関係リストビューの表示を更新する
            relationListView.SelectedItems[0].SubItems[1].Text = DoubleHelper.ToString(val);

            // 文字色を変更する
            relationValueNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     宗主国チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMasterCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country self = GetSelectedRelationCountry();
            if (self == Country.None)
            {
                return;
            }
            Country target = GetSelectedRelationTarget();
            if (target == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(self);

            // 値に変化がなければ何もしない
            bool val = masterCheckBox.Checked;
            if ((settings != null) && (val == (settings.Master == target)))
            {
                return;
            }

            Log.Info("[Scenario] master: {0} -> {1} ({2} > {3})",
                (settings != null) ? BoolHelper.ToYesNo(settings.Master == target) : "", BoolHelper.ToYesNo(val),
                Countries.Strings[(int) self], Countries.Strings[(int) target]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = self };
                Scenarios.SetCountrySettings(settings);
            }

            // 関係リストビューの表示を更新する
            if (settings.Master != target)
            {
                int index = Array.IndexOf(Countries.Tags, settings.Master);
                if (index >= 0)
                {
                    relationListView.Items[index].SubItems[2].Text = "";
                }
            }
            relationListView.SelectedItems[0].SubItems[2].Text = masterCheckBox.Checked ? Resources.Yes : "";

            // 値を更新する
            settings.Master = val ? target : Country.None;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.Master);
            Scenarios.SetDirty();

            // 文字色を変更する
            masterCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     統帥権取得国チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnControlCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country self = GetSelectedRelationCountry();
            if (self == Country.None)
            {
                return;
            }
            Country target = GetSelectedRelationTarget();
            if (target == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(self);

            // 値に変化がなければ何もしない
            bool val = controlCheckBox.Checked;
            if ((settings != null) && (val == (settings.Control == target)))
            {
                return;
            }

            Log.Info("[Scenario] military control: {0} -> {1} ({2} > {3})",
                (settings != null) ? BoolHelper.ToYesNo(settings.Control == target) : "", BoolHelper.ToYesNo(val),
                Countries.Strings[(int) self], Countries.Strings[(int) target]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = self };
                Scenarios.SetCountrySettings(settings);
            }

            // 関係リストビューの表示を更新する
            if (settings.Control != target)
            {
                int index = Array.IndexOf(Countries.Tags, settings.Control);
                if (index >= 0)
                {
                    relationListView.Items[index].SubItems[3].Text = "";
                }
            }
            relationListView.SelectedItems[0].SubItems[3].Text = controlCheckBox.Checked ? Resources.Yes : "";

            // 値を更新する
            settings.Control = val ? target : Country.None;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.Control);
            Scenarios.SetDirty();

            // 文字色を変更する
            controlCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     軍事通行許可国チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAccessCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country self = GetSelectedRelationCountry();
            if (self == Country.None)
            {
                return;
            }
            Country target = GetSelectedRelationTarget();
            if (target == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(self);
            Relation relation = Scenarios.GetCountryRelation(self, target);

            // 値に変化がなければ何もしない
            bool val = accessCheckBox.Checked;
            if ((relation != null) && (val == relation.Access))
            {
                return;
            }

            Log.Info("[Scenario] military access: {0} -> {1} ({2} > {3})",
                (relation != null) ? BoolHelper.ToYesNo(relation.Access) : "", BoolHelper.ToYesNo(val),
                Countries.Strings[(int) self], Countries.Strings[(int) target]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = self };
                Scenarios.SetCountrySettings(settings);
            }
            if (relation == null)
            {
                relation = new Relation { Country = target };
                settings.Relations.Add(relation);
                Scenarios.SetCountryRelation(self, relation);
            }

            // 値を更新する
            relation.Access = val;

            // 編集済みフラグを設定する
            relation.SetDirty(RelationItemId.Access);
            settings.SetDirty();
            Scenarios.SetDirty();

            // 関係リストビューの表示を更新する
            relationListView.SelectedItems[0].SubItems[4].Text = relation.Access ? Resources.Yes : "";

            // 文字色を変更する
            accessCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     独立保障チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGuaranteeCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country self = GetSelectedRelationCountry();
            if (self == Country.None)
            {
                return;
            }
            Country target = GetSelectedRelationTarget();
            if (target == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(self);
            Relation relation = Scenarios.GetCountryRelation(self, target);

            // 値に変化がなければ何もしない
            bool val = guaranteeCheckBox.Checked;
            if ((relation != null) && (val == (relation.Guaranteed != null)))
            {
                return;
            }

            Log.Info("[Scenario] guarantee: {0} -> {1} ({2} > {3})",
                (relation != null) ? BoolHelper.ToYesNo(relation.Guaranteed != null) : "", BoolHelper.ToYesNo(val),
                Countries.Strings[(int) self], Countries.Strings[(int) target]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = self };
                Scenarios.SetCountrySettings(settings);
            }
            if (relation == null)
            {
                relation = new Relation { Country = target };
                settings.Relations.Add(relation);
                Scenarios.SetCountryRelation(self, relation);
            }

            // 値を更新する
            relation.Guaranteed = val ? new GameDate() : null;

            // 編集済みフラグを設定する
            relation.SetDirty(RelationItemId.Guaranteed);
            relation.SetDirty(RelationItemId.GuaranteedYear);
            relation.SetDirty(RelationItemId.GuaranteedMonth);
            relation.SetDirty(RelationItemId.GuaranteedDay);
            settings.SetDirty();
            Scenarios.SetDirty();

            // 関係リストビューの表示を更新する
            relationListView.SelectedItems[0].SubItems[5].Text = val ? Resources.Yes : "";

            // 編集項目を更新する
            guaranteeYearTextBox.Text = val ? IntHelper.ToString(relation.Guaranteed.Year) : "";
            guaranteeMonthTextBox.Text = val ? IntHelper.ToString(relation.Guaranteed.Month) : "";
            guaranteeDayTextBox.Text = val ? IntHelper.ToString(relation.Guaranteed.Day) : "";

            guaranteeEndLabel.Enabled = val;
            guaranteeYearTextBox.Enabled = val;
            guaranteeMonthTextBox.Enabled = val;
            guaranteeDayTextBox.Enabled = val;

            // 文字色を変更する
            guaranteeCheckBox.ForeColor = Color.Red;
            guaranteeYearTextBox.ForeColor = Color.Red;
            guaranteeMonthTextBox.ForeColor = Color.Red;
            guaranteeDayTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     独立保証期限年テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGuaranteeYearTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country self = GetSelectedRelationCountry();
            if (self == Country.None)
            {
                return;
            }
            Country target = GetSelectedRelationTarget();
            if (target == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(self);
            Relation relation = Scenarios.GetCountryRelation(self, target);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(guaranteeYearTextBox.Text, out val))
            {
                guaranteeYearTextBox.Text = (relation.Guaranteed != null)
                    ? IntHelper.ToString(relation.Guaranteed.Year)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((relation.Guaranteed != null) && (val == relation.Guaranteed.Year))
            {
                return;
            }

            Log.Info("[Scenario] guarantee year: {0} -> {1} ({2} > {3})",
                (relation.Guaranteed != null) ? IntHelper.ToString(relation.Guaranteed.Year) : "", val,
                Countries.Strings[(int) self], Countries.Strings[(int) target]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = self };
                Scenarios.SetCountrySettings(settings);
            }
            if (relation.Guaranteed == null)
            {
                relation.Guaranteed = new GameDate();

                // 編集済みフラグを設定する
                relation.SetDirty(RelationItemId.GuaranteedMonth);
                relation.SetDirty(RelationItemId.GuaranteedDay);

                // 編集項目を更新する
                guaranteeMonthTextBox.Text = IntHelper.ToString(relation.Guaranteed.Month);
                guaranteeDayTextBox.Text = IntHelper.ToString(relation.Guaranteed.Day);

                // 文字色を変更する
                guaranteeMonthTextBox.ForeColor = Color.Red;
                guaranteeDayTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            relation.Guaranteed.Year = val;

            // 編集済みフラグを設定する
            relation.SetDirty(RelationItemId.GuaranteedYear);
            settings.SetDirty();
            Scenarios.SetDirty();

            // 文字色を変更する
            guaranteeYearTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     独立保証期限月テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGuaranteeMonthTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country self = GetSelectedRelationCountry();
            if (self == Country.None)
            {
                return;
            }
            Country target = GetSelectedRelationTarget();
            if (target == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(self);
            Relation relation = Scenarios.GetCountryRelation(self, target);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(guaranteeMonthTextBox.Text, out val))
            {
                guaranteeMonthTextBox.Text = (relation.Guaranteed != null)
                    ? IntHelper.ToString(relation.Guaranteed.Month)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((relation.Guaranteed != null) && (val == relation.Guaranteed.Month))
            {
                return;
            }

            Log.Info("[Scenario] guarantee month: {0} -> {1} ({2} > {3})",
                (relation.Guaranteed != null) ? IntHelper.ToString(relation.Guaranteed.Month) : "", val,
                Countries.Strings[(int) self], Countries.Strings[(int) target]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = self };
                Scenarios.SetCountrySettings(settings);
            }
            if (relation.Guaranteed == null)
            {
                relation.Guaranteed = new GameDate();

                // 編集済みフラグを設定する
                relation.SetDirty(RelationItemId.GuaranteedYear);
                relation.SetDirty(RelationItemId.GuaranteedDay);

                // 編集項目を更新する
                guaranteeYearTextBox.Text = IntHelper.ToString(relation.Guaranteed.Year);
                guaranteeDayTextBox.Text = IntHelper.ToString(relation.Guaranteed.Day);

                // 文字色を変更する
                guaranteeYearTextBox.ForeColor = Color.Red;
                guaranteeDayTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            relation.Guaranteed.Month = val;

            // 編集済みフラグを設定する
            relation.SetDirty(RelationItemId.GuaranteedMonth);
            settings.SetDirty();
            Scenarios.SetDirty();

            // 文字色を変更する
            guaranteeMonthTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     独立保証期限日テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGuaranteeDayTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country self = GetSelectedRelationCountry();
            if (self == Country.None)
            {
                return;
            }
            Country target = GetSelectedRelationTarget();
            if (target == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(self);
            Relation relation = Scenarios.GetCountryRelation(self, target);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(guaranteeDayTextBox.Text, out val))
            {
                guaranteeDayTextBox.Text = (relation.Guaranteed != null)
                    ? IntHelper.ToString(relation.Guaranteed.Day)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((relation.Guaranteed != null) && (val == relation.Guaranteed.Day))
            {
                return;
            }

            Log.Info("[Scenario] guarantee day: {0} -> {1} ({2} > {3})",
                (relation.Guaranteed != null) ? IntHelper.ToString(relation.Guaranteed.Day) : "", val,
                Countries.Strings[(int) self], Countries.Strings[(int) target]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = self };
                Scenarios.SetCountrySettings(settings);
            }
            if (relation.Guaranteed == null)
            {
                relation.Guaranteed = new GameDate();

                // 編集済みフラグを設定する
                relation.SetDirty(RelationItemId.GuaranteedYear);
                relation.SetDirty(RelationItemId.GuaranteedMonth);

                // 編集項目を更新する
                guaranteeYearTextBox.Text = IntHelper.ToString(relation.Guaranteed.Year);
                guaranteeMonthTextBox.Text = IntHelper.ToString(relation.Guaranteed.Month);

                // 文字色を変更する
                guaranteeYearTextBox.ForeColor = Color.Red;
                guaranteeMonthTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            relation.Guaranteed.Day = val;

            // 編集済みフラグを設定する
            relation.SetDirty(RelationItemId.GuaranteedDay);
            settings.SetDirty();
            Scenarios.SetDirty();

            // 文字色を変更する
            guaranteeDayTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     不可侵条約チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNonAggressionCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country self = GetSelectedRelationCountry();
            if (self == Country.None)
            {
                return;
            }
            Country target = GetSelectedRelationTarget();
            if (target == Country.None)
            {
                return;
            }

            List<Treaty> nonAggressions = Scenarios.Data.GlobalData.NonAggressions;
            Treaty nonAggression = Scenarios.GetNonAggression(self, target);

            // 値に変化がなければ何もしない
            bool val = nonAggressionCheckBox.Checked;
            if (val == (nonAggression != null))
            {
                return;
            }

            Log.Info("[Scenario] non aggression: {0} -> {1} ({2} > {3})", BoolHelper.ToYesNo(nonAggression != null),
                BoolHelper.ToYesNo(val), Countries.Strings[(int) self], Countries.Strings[(int) target]);

            // 値を更新する
            if (val)
            {
                nonAggression = new Treaty
                {
                    Type = TreatyType.NonAggression,
                    Country1 = self,
                    Country2 = target,
                    StartDate = new GameDate(),
                    EndDate = new GameDate(),
                    Id = Scenarios.GetNewTypeId(Scenarios.DefaultTreatyType, 1)
                };
                nonAggressions.Add(nonAggression);
                Scenarios.SetNonAggression(nonAggression);
            }
            else
            {
                nonAggressions.Remove(nonAggression);
                Scenarios.RemoveNonAggression(nonAggression);
            }

            // 編集済みフラグを設定する
            nonAggression.SetDirty(TreatyItemId.StartYear);
            nonAggression.SetDirty(TreatyItemId.StartMonth);
            nonAggression.SetDirty(TreatyItemId.StartDay);
            nonAggression.SetDirty(TreatyItemId.EndYear);
            nonAggression.SetDirty(TreatyItemId.EndMonth);
            nonAggression.SetDirty(TreatyItemId.EndDay);
            nonAggression.SetDirty(TreatyItemId.Type);
            nonAggression.SetDirty(TreatyItemId.Id);
            nonAggression.SetDirty();
            Scenarios.SetDirty();

            // 関係リストビューの表示を更新する
            relationListView.SelectedItems[0].SubItems[6].Text = nonAggressionCheckBox.Checked ? Resources.Yes : "";

            // 編集項目を更新する
            bool flag = val && (nonAggression.StartDate != null);
            nonAggressionStartYearTextBox.Text = flag ? IntHelper.ToString(nonAggression.StartDate.Year) : "";
            nonAggressionStartMonthTextBox.Text = flag ? IntHelper.ToString(nonAggression.StartDate.Month) : "";
            nonAggressionStartDayTextBox.Text = flag ? IntHelper.ToString(nonAggression.StartDate.Day) : "";

            flag = val && (nonAggression.EndDate != null);
            nonAggressionEndYearTextBox.Text = flag ? IntHelper.ToString(nonAggression.EndDate.Year) : "";
            nonAggressionEndMonthTextBox.Text = flag ? IntHelper.ToString(nonAggression.EndDate.Month) : "";
            nonAggressionEndDayTextBox.Text = flag ? IntHelper.ToString(nonAggression.EndDate.Day) : "";

            flag = val && (nonAggression.Id != null);
            nonAggressionTypeTextBox.Text = flag ? IntHelper.ToString(nonAggression.Id.Type) : "";
            nonAggressionIdTextBox.Text = flag ? IntHelper.ToString(nonAggression.Id.Id) : "";

            nonAggressionStartLabel.Enabled = val;
            nonAggressionStartYearTextBox.Enabled = val;
            nonAggressionStartMonthTextBox.Enabled = val;
            nonAggressionStartDayTextBox.Enabled = val;
            nonAggressionEndLabel.Enabled = val;
            nonAggressionEndYearTextBox.Enabled = val;
            nonAggressionEndMonthTextBox.Enabled = val;
            nonAggressionEndDayTextBox.Enabled = val;
            nonAggressionTypeTextBox.Enabled = val;
            nonAggressionIdTextBox.Enabled = val;

            // 文字色を変更する
            nonAggressionCheckBox.ForeColor = Color.Red;
            nonAggressionStartYearTextBox.ForeColor = Color.Red;
            nonAggressionStartMonthTextBox.ForeColor = Color.Red;
            nonAggressionStartDayTextBox.ForeColor = Color.Red;
            nonAggressionEndYearTextBox.ForeColor = Color.Red;
            nonAggressionEndMonthTextBox.ForeColor = Color.Red;
            nonAggressionEndDayTextBox.ForeColor = Color.Red;
            nonAggressionTypeTextBox.ForeColor = Color.Red;
            nonAggressionIdTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     不可侵条約開始年テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNonAggressionStartYearTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country self = GetSelectedRelationCountry();
            if (self == Country.None)
            {
                return;
            }
            Country target = GetSelectedRelationTarget();
            if (target == Country.None)
            {
                return;
            }

            Treaty nonAggression = Scenarios.GetNonAggression(self, target);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(nonAggressionStartYearTextBox.Text, out val))
            {
                nonAggressionStartYearTextBox.Text = (nonAggression.StartDate != null)
                    ? IntHelper.ToString(nonAggression.StartDate.Year)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((nonAggression.StartDate != null) && (val == nonAggression.StartDate.Year))
            {
                return;
            }

            Log.Info("[Scenario] non aggression start year: {0} -> {1} ({2} > {3})",
                (nonAggression.StartDate != null) ? IntHelper.ToString(nonAggression.StartDate.Year) : "", val,
                Countries.Strings[(int) self], Countries.Strings[(int) target]);

            if (nonAggression.StartDate == null)
            {
                nonAggression.StartDate = new GameDate();

                // 編集済みフラグを設定する
                nonAggression.SetDirty(TreatyItemId.StartMonth);
                nonAggression.SetDirty(TreatyItemId.StartDay);

                // 編集項目を更新する
                nonAggressionStartMonthTextBox.Text = IntHelper.ToString(nonAggression.StartDate.Month);
                nonAggressionStartDayTextBox.Text = IntHelper.ToString(nonAggression.StartDate.Day);

                // 文字色を変更する
                nonAggressionStartMonthTextBox.ForeColor = Color.Red;
                nonAggressionStartDayTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            nonAggression.StartDate.Year = val;

            // 編集済みフラグを設定する
            nonAggression.SetDirty(TreatyItemId.StartYear);
            Scenarios.SetDirty();

            // 文字色を変更する
            nonAggressionStartYearTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     不可侵条約開始月テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNonAggressionStartMonthTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country self = GetSelectedRelationCountry();
            if (self == Country.None)
            {
                return;
            }
            Country target = GetSelectedRelationTarget();
            if (target == Country.None)
            {
                return;
            }

            Treaty nonAggression = Scenarios.GetNonAggression(self, target);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(nonAggressionStartMonthTextBox.Text, out val))
            {
                nonAggressionStartMonthTextBox.Text = (nonAggression.StartDate != null)
                    ? IntHelper.ToString(nonAggression.StartDate.Month)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((nonAggression.StartDate != null) && (val == nonAggression.StartDate.Month))
            {
                return;
            }

            Log.Info("[Scenario] non aggression start month: {0} -> {1} ({2} > {3})",
                (nonAggression.StartDate != null) ? IntHelper.ToString(nonAggression.StartDate.Month) : "", val,
                Countries.Strings[(int) self], Countries.Strings[(int) target]);

            if (nonAggression.StartDate == null)
            {
                nonAggression.StartDate = new GameDate();

                // 編集済みフラグを設定する
                nonAggression.SetDirty(TreatyItemId.StartYear);
                nonAggression.SetDirty(TreatyItemId.StartDay);

                // 編集項目を更新する
                nonAggressionStartYearTextBox.Text = IntHelper.ToString(nonAggression.StartDate.Year);
                nonAggressionStartDayTextBox.Text = IntHelper.ToString(nonAggression.StartDate.Day);

                // 文字色を変更する
                nonAggressionStartYearTextBox.ForeColor = Color.Red;
                nonAggressionStartDayTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            nonAggression.StartDate.Month = val;

            // 編集済みフラグを設定する
            nonAggression.SetDirty(TreatyItemId.StartMonth);
            Scenarios.SetDirty();

            // 文字色を変更する
            nonAggressionStartMonthTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     不可侵条約開始日テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNonAggressionStartDayTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country self = GetSelectedRelationCountry();
            if (self == Country.None)
            {
                return;
            }
            Country target = GetSelectedRelationTarget();
            if (target == Country.None)
            {
                return;
            }

            Treaty nonAggression = Scenarios.GetNonAggression(self, target);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(nonAggressionStartDayTextBox.Text, out val))
            {
                nonAggressionStartDayTextBox.Text = (nonAggression.StartDate != null)
                    ? IntHelper.ToString(nonAggression.StartDate.Day)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((nonAggression.StartDate != null) && (val == nonAggression.StartDate.Day))
            {
                return;
            }

            Log.Info("[Scenario] non aggression start day: {0} -> {1} ({2} > {3})",
                (nonAggression.StartDate != null) ? IntHelper.ToString(nonAggression.StartDate.Day) : "", val,
                Countries.Strings[(int) self], Countries.Strings[(int) target]);

            if (nonAggression.StartDate == null)
            {
                nonAggression.StartDate = new GameDate();

                // 編集済みフラグを設定する
                nonAggression.SetDirty(TreatyItemId.StartYear);
                nonAggression.SetDirty(TreatyItemId.StartMonth);

                // 編集項目を更新する
                nonAggressionStartYearTextBox.Text = IntHelper.ToString(nonAggression.StartDate.Year);
                nonAggressionStartMonthTextBox.Text = IntHelper.ToString(nonAggression.StartDate.Month);

                // 文字色を変更する
                nonAggressionStartYearTextBox.ForeColor = Color.Red;
                nonAggressionStartMonthTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            nonAggression.StartDate.Day = val;

            // 編集済みフラグを設定する
            nonAggression.SetDirty(TreatyItemId.StartDay);
            Scenarios.SetDirty();

            // 文字色を変更する
            nonAggressionStartDayTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     不可侵条約終了年テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNonAggressionEndYearTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country self = GetSelectedRelationCountry();
            if (self == Country.None)
            {
                return;
            }
            Country target = GetSelectedRelationTarget();
            if (target == Country.None)
            {
                return;
            }

            Treaty nonAggression = Scenarios.GetNonAggression(self, target);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(nonAggressionEndYearTextBox.Text, out val))
            {
                nonAggressionEndYearTextBox.Text = (nonAggression.EndDate != null)
                    ? IntHelper.ToString(nonAggression.EndDate.Year)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((nonAggression.EndDate != null) && (val == nonAggression.EndDate.Year))
            {
                return;
            }

            Log.Info("[Scenario] non aggression end year: {0} -> {1} ({2} > {3})",
                (nonAggression.EndDate != null) ? IntHelper.ToString(nonAggression.EndDate.Year) : "", val,
                Countries.Strings[(int) self], Countries.Strings[(int) target]);

            if (nonAggression.EndDate == null)
            {
                nonAggression.EndDate = new GameDate();

                // 編集済みフラグを設定する
                nonAggression.SetDirty(TreatyItemId.EndMonth);
                nonAggression.SetDirty(TreatyItemId.EndDay);

                // 編集項目を更新する
                nonAggressionEndMonthTextBox.Text = IntHelper.ToString(nonAggression.EndDate.Month);
                nonAggressionEndDayTextBox.Text = IntHelper.ToString(nonAggression.EndDate.Day);

                // 文字色を変更する
                nonAggressionEndMonthTextBox.ForeColor = Color.Red;
                nonAggressionEndDayTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            nonAggression.EndDate.Year = val;

            // 編集済みフラグを設定する
            nonAggression.SetDirty(TreatyItemId.EndYear);
            Scenarios.SetDirty();

            // 文字色を変更する
            nonAggressionEndYearTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     不可侵条約終了月テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNonAggressionEndMonthTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country self = GetSelectedRelationCountry();
            if (self == Country.None)
            {
                return;
            }
            Country target = GetSelectedRelationTarget();
            if (target == Country.None)
            {
                return;
            }

            Treaty nonAggression = Scenarios.GetNonAggression(self, target);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(nonAggressionEndMonthTextBox.Text, out val))
            {
                nonAggressionEndMonthTextBox.Text = (nonAggression.EndDate != null)
                    ? IntHelper.ToString(nonAggression.EndDate.Month)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((nonAggression.EndDate != null) && (val == nonAggression.EndDate.Month))
            {
                return;
            }

            Log.Info("[Scenario] non aggression end month: {0} -> {1} ({2} > {3})",
                (nonAggression.EndDate != null) ? IntHelper.ToString(nonAggression.EndDate.Month) : "", val,
                Countries.Strings[(int) self], Countries.Strings[(int) target]);

            if (nonAggression.EndDate == null)
            {
                nonAggression.EndDate = new GameDate();

                // 編集済みフラグを設定する
                nonAggression.SetDirty(TreatyItemId.EndYear);
                nonAggression.SetDirty(TreatyItemId.EndDay);

                // 編集項目を更新する
                nonAggressionEndYearTextBox.Text = IntHelper.ToString(nonAggression.EndDate.Year);
                nonAggressionEndDayTextBox.Text = IntHelper.ToString(nonAggression.EndDate.Day);

                // 文字色を変更する
                nonAggressionEndYearTextBox.ForeColor = Color.Red;
                nonAggressionEndDayTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            nonAggression.EndDate.Month = val;

            // 編集済みフラグを設定する
            nonAggression.SetDirty(TreatyItemId.EndMonth);
            Scenarios.SetDirty();

            // 文字色を変更する
            nonAggressionEndMonthTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     不可侵条約終了日テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNonAggressionEndDayTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country self = GetSelectedRelationCountry();
            if (self == Country.None)
            {
                return;
            }
            Country target = GetSelectedRelationTarget();
            if (target == Country.None)
            {
                return;
            }

            Treaty nonAggression = Scenarios.GetNonAggression(self, target);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(nonAggressionEndDayTextBox.Text, out val))
            {
                nonAggressionEndDayTextBox.Text = (nonAggression.EndDate != null)
                    ? IntHelper.ToString(nonAggression.EndDate.Day)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((nonAggression.EndDate != null) && (val == nonAggression.EndDate.Day))
            {
                return;
            }

            Log.Info("[Scenario] non aggression end day: {0} -> {1} ({2} > {3})",
                (nonAggression.EndDate != null) ? IntHelper.ToString(nonAggression.EndDate.Day) : "", val,
                Countries.Strings[(int) self], Countries.Strings[(int) target]);

            if (nonAggression.EndDate == null)
            {
                nonAggression.EndDate = new GameDate();

                // 編集済みフラグを設定する
                nonAggression.SetDirty(TreatyItemId.EndYear);
                nonAggression.SetDirty(TreatyItemId.EndMonth);

                // 編集項目を更新する
                nonAggressionEndYearTextBox.Text = IntHelper.ToString(nonAggression.EndDate.Year);
                nonAggressionEndMonthTextBox.Text = IntHelper.ToString(nonAggression.EndDate.Month);

                // 文字色を変更する
                nonAggressionEndYearTextBox.ForeColor = Color.Red;
                nonAggressionEndMonthTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            nonAggression.EndDate.Day = val;

            // 編集済みフラグを設定する
            nonAggression.SetDirty(TreatyItemId.EndDay);
            Scenarios.SetDirty();

            // 文字色を変更する
            nonAggressionEndDayTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     不可侵条約のtypeテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNonAggressionTypeTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country self = GetSelectedRelationCountry();
            if (self == Country.None)
            {
                return;
            }
            Country target = GetSelectedRelationTarget();
            if (target == Country.None)
            {
                return;
            }

            Treaty nonAggression = Scenarios.GetNonAggression(self, target);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(nonAggressionTypeTextBox.Text, out val))
            {
                nonAggressionTypeTextBox.Text = (nonAggression.Id != null)
                    ? IntHelper.ToString(nonAggression.Id.Type)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((nonAggression.Id != null) && (val == nonAggression.Id.Type))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if ((nonAggression.Id != null) && Scenarios.ExistsTypeId(val, nonAggression.Id.Id))
            {
                nonAggressionTypeTextBox.Text = IntHelper.ToString(nonAggression.Id.Type);
                return;
            }

            Log.Info("[Scenario] non aggression type: {0} -> {1} ({2} > {3})",
                (nonAggression.Id != null) ? IntHelper.ToString(nonAggression.Id.Type) : "", val,
                Countries.Strings[(int) self], Countries.Strings[(int) target]);

            if (nonAggression.Id != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(nonAggression.Id);

                // 値を更新する
                nonAggression.Id.Type = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(nonAggression.Id);
            }
            else
            {
                // 値を更新する
                nonAggression.Id = Scenarios.GetNewTypeId(Scenarios.DefaultTreatyType, 1);

                // 編集済みフラグを設定する
                nonAggression.SetDirty(TreatyItemId.Id);

                // 編集項目を更新する
                nonAggressionIdTextBox.Text = IntHelper.ToString(nonAggression.Id.Id);

                // 文字色を変更する
                nonAggressionIdTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            nonAggression.SetDirty(TreatyItemId.Type);
            Scenarios.SetDirty();

            // 文字色を変更する
            nonAggressionTypeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     不可侵条約のidテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNonAggressionIdTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country self = GetSelectedRelationCountry();
            if (self == Country.None)
            {
                return;
            }
            Country target = GetSelectedRelationTarget();
            if (target == Country.None)
            {
                return;
            }

            Treaty nonAggression = Scenarios.GetNonAggression(self, target);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(nonAggressionIdTextBox.Text, out val))
            {
                nonAggressionIdTextBox.Text = (nonAggression.Id != null) ? IntHelper.ToString(nonAggression.Id.Id) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((nonAggression.Id != null) && (val == nonAggression.Id.Id))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if (Scenarios.ExistsTypeId(
                (nonAggression.Id != null) ? nonAggression.Id.Type : Scenarios.DefaultTreatyType, val))
            {
                nonAggressionIdTextBox.Text = (nonAggression.Id != null) ? IntHelper.ToString(nonAggression.Id.Id) : "";
                return;
            }

            Log.Info("[Scenario] non aggression type: {0} -> {1} ({2} > {3})",
                (nonAggression.Id != null) ? IntHelper.ToString(nonAggression.Id.Id) : "", val,
                Countries.Strings[(int) self], Countries.Strings[(int) target]);

            if (nonAggression.Id != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(nonAggression.Id);

                // 値を更新する
                nonAggression.Id.Id = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(nonAggression.Id);
            }
            else
            {
                // 値を更新する
                nonAggression.Id = Scenarios.GetNewTypeId(Scenarios.DefaultTreatyType, val);

                // 編集済みフラグを設定する
                nonAggression.SetDirty(TreatyItemId.Type);

                // 編集項目を更新する
                nonAggressionTypeTextBox.Text = IntHelper.ToString(nonAggression.Id.Type);

                // 文字色を変更する
                nonAggressionTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            nonAggression.SetDirty(TreatyItemId.Id);
            Scenarios.SetDirty();

            // 文字色を変更する
            nonAggressionIdTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     講和条約チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPeaceCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country self = GetSelectedRelationCountry();
            if (self == Country.None)
            {
                return;
            }
            Country target = GetSelectedRelationTarget();
            if (target == Country.None)
            {
                return;
            }

            List<Treaty> peaces = Scenarios.Data.GlobalData.Peaces;
            Treaty peace = Scenarios.GetPeace(self, target);

            // 値に変化がなければ何もしない
            bool val = (peaceCheckBox.Checked);
            if (val == (peace != null))
            {
                return;
            }

            Log.Info("[Scenario] peace: {0} -> {1} ({2} > {3})", BoolHelper.ToYesNo(peace != null),
                BoolHelper.ToYesNo(val), Countries.Strings[(int) self], Countries.Strings[(int) target]);

            // 値を更新する
            if (val)
            {
                peace = new Treaty
                {
                    Type = TreatyType.Peace,
                    Country1 = self,
                    Country2 = target,
                    StartDate = new GameDate(),
                    EndDate = new GameDate(),
                    Id = Scenarios.GetNewTypeId(Scenarios.DefaultTreatyType, 1)
                };
                peaces.Add(peace);
                Scenarios.SetPeace(peace);
            }
            else
            {
                peaces.Remove(peace);
                Scenarios.RemovePeace(peace);
            }

            // 編集済みフラグを設定する
            peace.SetDirty(TreatyItemId.StartYear);
            peace.SetDirty(TreatyItemId.StartMonth);
            peace.SetDirty(TreatyItemId.StartDay);
            peace.SetDirty(TreatyItemId.EndYear);
            peace.SetDirty(TreatyItemId.EndMonth);
            peace.SetDirty(TreatyItemId.EndDay);
            peace.SetDirty(TreatyItemId.Type);
            peace.SetDirty(TreatyItemId.Id);
            peace.SetDirty();
            Scenarios.SetDirty();

            // 関係リストビューの表示を更新する
            relationListView.SelectedItems[0].SubItems[7].Text = peaceCheckBox.Checked ? Resources.Yes : "";

            // 編集項目を更新する
            bool flag = val && (peace.StartDate != null);
            peaceStartYearTextBox.Text = flag ? IntHelper.ToString(peace.StartDate.Year) : "";
            peaceStartMonthTextBox.Text = flag ? IntHelper.ToString(peace.StartDate.Month) : "";
            peaceStartDayTextBox.Text = flag ? IntHelper.ToString(peace.StartDate.Day) : "";

            flag = val && (peace.EndDate != null);
            peaceEndYearTextBox.Text = flag ? IntHelper.ToString(peace.EndDate.Year) : "";
            peaceEndMonthTextBox.Text = flag ? IntHelper.ToString(peace.EndDate.Month) : "";
            peaceEndDayTextBox.Text = flag ? IntHelper.ToString(peace.EndDate.Day) : "";

            flag = val && (peace.Id != null);
            peaceTypeTextBox.Text = flag ? IntHelper.ToString(peace.Id.Type) : "";
            peaceIdTextBox.Text = flag ? IntHelper.ToString(peace.Id.Id) : "";

            peaceStartLabel.Enabled = val;
            peaceStartYearTextBox.Enabled = val;
            peaceStartMonthTextBox.Enabled = val;
            peaceStartDayTextBox.Enabled = val;
            peaceEndLabel.Enabled = val;
            peaceEndYearTextBox.Enabled = val;
            peaceEndMonthTextBox.Enabled = val;
            peaceEndDayTextBox.Enabled = val;
            peaceTypeTextBox.Enabled = val;
            peaceIdTextBox.Enabled = val;

            // 文字色を変更する
            peaceCheckBox.ForeColor = Color.Red;
            peaceStartYearTextBox.ForeColor = Color.Red;
            peaceStartMonthTextBox.ForeColor = Color.Red;
            peaceStartDayTextBox.ForeColor = Color.Red;
            peaceEndYearTextBox.ForeColor = Color.Red;
            peaceEndMonthTextBox.ForeColor = Color.Red;
            peaceEndDayTextBox.ForeColor = Color.Red;
            peaceTypeTextBox.ForeColor = Color.Red;
            peaceIdTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     講和条約開始年テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPeaceStartYearTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country self = GetSelectedRelationCountry();
            if (self == Country.None)
            {
                return;
            }
            Country target = GetSelectedRelationTarget();
            if (target == Country.None)
            {
                return;
            }

            Treaty peace = Scenarios.GetPeace(self, target);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(peaceStartYearTextBox.Text, out val))
            {
                peaceStartYearTextBox.Text = (peace.StartDate != null)
                    ? IntHelper.ToString(peace.StartDate.Year)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((peace.StartDate != null) && (val == peace.StartDate.Year))
            {
                return;
            }

            Log.Info("[Scenario] peace start year: {0} -> {1} ({2} > {3})",
                (peace.StartDate != null) ? IntHelper.ToString(peace.StartDate.Year) : "", val,
                Countries.Strings[(int) self], Countries.Strings[(int) target]);

            if (peace.StartDate == null)
            {
                peace.StartDate = new GameDate();

                // 編集済みフラグを設定する
                peace.SetDirty(TreatyItemId.StartMonth);
                peace.SetDirty(TreatyItemId.StartDay);

                // 編集項目を更新する
                peaceStartMonthTextBox.Text = IntHelper.ToString(peace.StartDate.Month);
                peaceStartDayTextBox.Text = IntHelper.ToString(peace.StartDate.Day);

                // 文字色を変更する
                peaceStartMonthTextBox.ForeColor = Color.Red;
                peaceStartDayTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            peace.StartDate.Year = val;

            // 編集済みフラグを設定する
            peace.SetDirty(TreatyItemId.StartYear);
            Scenarios.SetDirty();

            // 文字色を変更する
            peaceStartYearTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     講和条約開始月テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPeaceStartMonthTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country self = GetSelectedRelationCountry();
            if (self == Country.None)
            {
                return;
            }
            Country target = GetSelectedRelationTarget();
            if (target == Country.None)
            {
                return;
            }

            Treaty peace = Scenarios.GetPeace(self, target);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(peaceStartMonthTextBox.Text, out val))
            {
                peaceStartMonthTextBox.Text = (peace.StartDate != null)
                    ? IntHelper.ToString(peace.StartDate.Month)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((peace.StartDate != null) && (val == peace.StartDate.Month))
            {
                return;
            }

            Log.Info("[Scenario] peace start month: {0} -> {1} ({2} > {3})",
                (peace.StartDate != null) ? IntHelper.ToString(peace.StartDate.Month) : "", val,
                Countries.Strings[(int) self], Countries.Strings[(int) target]);

            if (peace.StartDate == null)
            {
                peace.StartDate = new GameDate();

                // 編集済みフラグを設定する
                peace.SetDirty(TreatyItemId.StartYear);
                peace.SetDirty(TreatyItemId.StartDay);

                // 編集項目を更新する
                peaceStartYearTextBox.Text = IntHelper.ToString(peace.StartDate.Year);
                peaceStartDayTextBox.Text = IntHelper.ToString(peace.StartDate.Day);

                // 文字色を変更する
                peaceStartYearTextBox.ForeColor = Color.Red;
                peaceStartDayTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            peace.StartDate.Month = val;

            // 編集済みフラグを設定する
            peace.SetDirty(TreatyItemId.StartMonth);
            Scenarios.SetDirty();

            // 文字色を変更する
            peaceStartMonthTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     講和条約開始日テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPeaceStartDayTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country self = GetSelectedRelationCountry();
            if (self == Country.None)
            {
                return;
            }
            Country target = GetSelectedRelationTarget();
            if (target == Country.None)
            {
                return;
            }

            Treaty peace = Scenarios.GetPeace(self, target);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(peaceStartDayTextBox.Text, out val))
            {
                peaceStartDayTextBox.Text = (peace.StartDate != null)
                    ? IntHelper.ToString(peace.StartDate.Day)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((peace.StartDate != null) && (val == peace.StartDate.Day))
            {
                return;
            }

            Log.Info("[Scenario] peace start day: {0} -> {1} ({2} > {3})",
                (peace.StartDate != null) ? IntHelper.ToString(peace.StartDate.Day) : "", val,
                Countries.Strings[(int) self], Countries.Strings[(int) target]);

            if (peace.StartDate == null)
            {
                peace.StartDate = new GameDate();

                // 編集済みフラグを設定する
                peace.SetDirty(TreatyItemId.StartYear);
                peace.SetDirty(TreatyItemId.StartMonth);

                // 編集項目を更新する
                peaceStartYearTextBox.Text = IntHelper.ToString(peace.StartDate.Year);
                peaceStartMonthTextBox.Text = IntHelper.ToString(peace.StartDate.Month);

                // 文字色を変更する
                peaceStartYearTextBox.ForeColor = Color.Red;
                peaceStartMonthTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            peace.StartDate.Day = val;

            // 編集済みフラグを設定する
            peace.SetDirty(TreatyItemId.StartDay);
            Scenarios.SetDirty();

            // 文字色を変更する
            peaceStartDayTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     講和条約終了年テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPeaceEndYearTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country self = GetSelectedRelationCountry();
            if (self == Country.None)
            {
                return;
            }
            Country target = GetSelectedRelationTarget();
            if (target == Country.None)
            {
                return;
            }

            Treaty peace = Scenarios.GetPeace(self, target);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(peaceEndYearTextBox.Text, out val))
            {
                peaceEndYearTextBox.Text = (peace.EndDate != null)
                    ? IntHelper.ToString(peace.EndDate.Year)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((peace.EndDate != null) && (val == peace.EndDate.Year))
            {
                return;
            }

            Log.Info("[Scenario] peace end year: {0} -> {1} ({2} > {3})",
                (peace.EndDate != null) ? IntHelper.ToString(peace.EndDate.Year) : "", val,
                Countries.Strings[(int) self], Countries.Strings[(int) target]);

            if (peace.EndDate == null)
            {
                peace.EndDate = new GameDate();

                // 編集済みフラグを設定する
                peace.SetDirty(TreatyItemId.EndMonth);
                peace.SetDirty(TreatyItemId.EndDay);

                // 編集項目を更新する
                peaceEndMonthTextBox.Text = IntHelper.ToString(peace.EndDate.Month);
                peaceEndDayTextBox.Text = IntHelper.ToString(peace.EndDate.Day);

                // 文字色を変更する
                peaceEndMonthTextBox.ForeColor = Color.Red;
                peaceEndDayTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            peace.EndDate.Year = val;

            // 編集済みフラグを設定する
            peace.SetDirty(TreatyItemId.EndYear);
            Scenarios.SetDirty();

            // 文字色を変更する
            peaceEndYearTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     講和条約終了月テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPeaceEndMonthTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country self = GetSelectedRelationCountry();
            if (self == Country.None)
            {
                return;
            }
            Country target = GetSelectedRelationTarget();
            if (target == Country.None)
            {
                return;
            }

            Treaty peace = Scenarios.GetPeace(self, target);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(peaceEndMonthTextBox.Text, out val))
            {
                peaceEndMonthTextBox.Text = (peace.EndDate != null)
                    ? IntHelper.ToString(peace.EndDate.Month)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((peace.EndDate != null) && (val == peace.EndDate.Month))
            {
                return;
            }

            Log.Info("[Scenario] peace end month: {0} -> {1} ({2} > {3})",
                (peace.EndDate != null) ? IntHelper.ToString(peace.EndDate.Month) : "", val,
                Countries.Strings[(int) self], Countries.Strings[(int) target]);

            if (peace.EndDate == null)
            {
                peace.EndDate = new GameDate();

                // 編集済みフラグを設定する
                peace.SetDirty(TreatyItemId.EndYear);
                peace.SetDirty(TreatyItemId.EndDay);

                // 編集項目を更新する
                peaceEndYearTextBox.Text = IntHelper.ToString(peace.EndDate.Year);
                peaceEndDayTextBox.Text = IntHelper.ToString(peace.EndDate.Day);

                // 文字色を変更する
                peaceEndYearTextBox.ForeColor = Color.Red;
                peaceEndDayTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            peace.EndDate.Month = val;

            // 編集済みフラグを設定する
            peace.SetDirty(TreatyItemId.EndMonth);
            Scenarios.SetDirty();

            // 文字色を変更する
            peaceEndMonthTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     講和条約終了日テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPeaceEndDayTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country self = GetSelectedRelationCountry();
            if (self == Country.None)
            {
                return;
            }
            Country target = GetSelectedRelationTarget();
            if (target == Country.None)
            {
                return;
            }

            Treaty peace = Scenarios.GetPeace(self, target);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(peaceEndDayTextBox.Text, out val))
            {
                peaceEndDayTextBox.Text = (peace.EndDate != null)
                    ? IntHelper.ToString(peace.EndDate.Day)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((peace.EndDate != null) && (val == peace.EndDate.Day))
            {
                return;
            }

            Log.Info("[Scenario] peace end day: {0} -> {1} ({2} > {3})",
                (peace.EndDate != null) ? IntHelper.ToString(peace.EndDate.Day) : "", val,
                Countries.Strings[(int) self], Countries.Strings[(int) target]);

            if (peace.EndDate == null)
            {
                peace.EndDate = new GameDate();

                // 編集済みフラグを設定する
                peace.SetDirty(TreatyItemId.EndYear);
                peace.SetDirty(TreatyItemId.EndMonth);

                // 編集項目を更新する
                peaceEndYearTextBox.Text = IntHelper.ToString(peace.EndDate.Year);
                peaceEndMonthTextBox.Text = IntHelper.ToString(peace.EndDate.Month);

                // 文字色を変更する
                peaceEndYearTextBox.ForeColor = Color.Red;
                peaceEndMonthTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            peace.EndDate.Day = val;

            // 編集済みフラグを設定する
            peace.SetDirty(TreatyItemId.EndDay);
            Scenarios.SetDirty();

            // 文字色を変更する
            peaceEndDayTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     講和条約のtypeテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPeaceTypeTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country self = GetSelectedRelationCountry();
            if (self == Country.None)
            {
                return;
            }
            Country target = GetSelectedRelationTarget();
            if (target == Country.None)
            {
                return;
            }

            Treaty peace = Scenarios.GetPeace(self, target);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(peaceTypeTextBox.Text, out val))
            {
                peaceTypeTextBox.Text = (peace.Id != null)
                    ? IntHelper.ToString(peace.Id.Type)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((peace.Id != null) && (val == peace.Id.Type))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if ((peace.Id != null) && Scenarios.ExistsTypeId(val, peace.Id.Id))
            {
                peaceTypeTextBox.Text = IntHelper.ToString(peace.Id.Type);
                return;
            }

            Log.Info("[Scenario] peace type: {0} -> {1} ({2} > {3})",
                (peace.Id != null) ? IntHelper.ToString(peace.Id.Type) : "", val,
                Countries.Strings[(int) self], Countries.Strings[(int) target]);

            if (peace.Id != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(peace.Id);

                // 値を更新する
                peace.Id.Type = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(peace.Id);
            }
            else
            {
                // 値を更新する
                peace.Id = Scenarios.GetNewTypeId(Scenarios.DefaultTreatyType, 1);

                // 編集済みフラグを設定する
                peace.SetDirty(TreatyItemId.Id);

                // 編集項目を更新する
                peaceIdTextBox.Text = IntHelper.ToString(peace.Id.Id);

                // 文字色を変更する
                peaceIdTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            peace.SetDirty(TreatyItemId.Type);
            Scenarios.SetDirty();

            // 文字色を変更する
            peaceTypeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     講和条約のidテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPeaceIdTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country self = GetSelectedRelationCountry();
            if (self == Country.None)
            {
                return;
            }
            Country target = GetSelectedRelationTarget();
            if (target == Country.None)
            {
                return;
            }

            Treaty peace = Scenarios.GetPeace(self, target);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(peaceIdTextBox.Text, out val))
            {
                peaceIdTextBox.Text = (peace.Id != null) ? IntHelper.ToString(peace.Id.Id) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((peace.Id != null) && (val == peace.Id.Id))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if (Scenarios.ExistsTypeId(
                (peace.Id != null) ? peace.Id.Type : Scenarios.DefaultTreatyType, val))
            {
                peaceIdTextBox.Text = (peace.Id != null) ? IntHelper.ToString(peace.Id.Id) : "";
                return;
            }

            Log.Info("[Scenario] peace type: {0} -> {1} ({2} > {3})",
                (peace.Id != null) ? IntHelper.ToString(peace.Id.Id) : "", val,
                Countries.Strings[(int) self], Countries.Strings[(int) target]);

            if (peace.Id != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(peace.Id);

                // 値を更新する
                peace.Id.Id = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(peace.Id);
            }
            else
            {
                // 値を更新する
                peace.Id = Scenarios.GetNewTypeId(Scenarios.DefaultTreatyType, val);

                // 編集済みフラグを設定する
                peace.SetDirty(TreatyItemId.Type);

                // 編集項目を更新する
                peaceTypeTextBox.Text = IntHelper.ToString(peace.Id.Type);

                // 文字色を変更する
                peaceTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            peace.SetDirty(TreatyItemId.Id);
            Scenarios.SetDirty();

            // 文字色を変更する
            peaceIdTextBox.ForeColor = Color.Red;
        }

        #endregion

        #region 関係タブ - 諜報

        /// <summary>
        ///     諜報情報の編集項目を更新する
        /// </summary>
        private void UpdateIntelligenceItems()
        {
            Country self = GetSelectedRelationCountry();
            Country target = GetSelectedRelationTarget();
            SpySettings spy = Scenarios.GetCountryIntelligence(self, target);

            bool flag = (spy != null);
            spyNumNumericUpDown.Value = flag ? spy.Spies : 0;
            spyNumNumericUpDown.ForeColor = (flag && spy.IsDirty(SpySettingsItemId.Spies))
                ? Color.Red
                : SystemColors.WindowText;
        }

        /// <summary>
        ///     スパイの数変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpyNumNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country self = GetSelectedRelationCountry();
            if (self == Country.None)
            {
                return;
            }
            Country target = GetSelectedRelationTarget();
            if (target == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(self);
            SpySettings spy = Scenarios.GetCountryIntelligence(self, target);

            // 値に変化がなければ何もしない
            int val = (int) spyNumNumericUpDown.Value;
            if ((spy != null) && (val == spy.Spies))
            {
                return;
            }

            Log.Info("[Scenario] num of spies: {0} -> {1} ({2} > {3})", (spy != null) ? spy.Spies : 0, val,
                Countries.Strings[(int) self], Countries.Strings[(int) target]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = self };
                Scenarios.SetCountrySettings(settings);
            }
            if (spy == null)
            {
                spy = new SpySettings { Country = target };
                Scenarios.SetCountryIntelligence(self, spy);
            }

            // 値を更新する
            spy.Spies = val;

            // 編集済みフラグを設定する
            spy.SetDirty(SpySettingsItemId.Spies);
            settings.SetDirty();
            Scenarios.SetDirty();

            // 関係リストビューの表示を更新する
            relationListView.SelectedItems[0].SubItems[8].Text = IntHelper.ToString(val);

            // 文字色を変更する
            spyNumNumericUpDown.ForeColor = Color.Red;
        }

        #endregion

        #endregion

        #region 貿易タブ

        #region 貿易タブ - 共通

        /// <summary>
        ///     貿易タブの編集項目を初期化する
        /// </summary>
        private void InitTradeTab()
        {
            Graphics g = Graphics.FromHwnd(Handle);
            int margin = DeviceCaps.GetScaledWidth(2) + 1;

            // 貿易国家コンボボックス
            tradeCountryComboBox1.BeginUpdate();
            tradeCountryComboBox2.BeginUpdate();
            tradeCountryComboBox1.Items.Clear();
            tradeCountryComboBox2.Items.Clear();
            int width = tradeCountryComboBox1.Width;
            foreach (Country country in Countries.Tags)
            {
                string s = Countries.GetTagName(country);
                tradeCountryComboBox1.Items.Add(s);
                tradeCountryComboBox2.Items.Add(s);
                width = Math.Max(width,
                    (int) g.MeasureString(s, tradeCountryComboBox1.Font).Width +
                    SystemInformation.VerticalScrollBarWidth + margin);
            }
            tradeCountryComboBox1.DropDownWidth = width;
            tradeCountryComboBox2.DropDownWidth = width;
            tradeCountryComboBox1.EndUpdate();
            tradeCountryComboBox2.EndUpdate();

            // 貿易資源ラベル
            tradeEnergyLabel.Text = Config.GetText(TextId.ResourceEnergy);
            tradeMetalLabel.Text = Config.GetText(TextId.ResourceMetal);
            tradeRareMaterialsLabel.Text = Config.GetText(TextId.ResourceRareMaterials);
            tradeOilLabel.Text = Config.GetText(TextId.ResourceOil);
            tradeSuppliesLabel.Text = Config.GetText(TextId.ResourceSupplies);
            tradeMoneyLabel.Text = Config.GetText(TextId.ResourceMoney);
        }

        /// <summary>
        ///     貿易タブの編集項目を更新する
        /// </summary>
        private void UpdateTradeTab()
        {
            // 編集項目を無効化する
            DisableTradeItems();

            // 貿易リストビュー
            List<Treaty> trades = Scenarios.Data.GlobalData.Trades;
            tradeListView.BeginUpdate();
            tradeListView.Items.Clear();
            foreach (Treaty trade in trades)
            {
                tradeListView.Items.Add(CreateTradeListViewItem(trade));
            }
            tradeListView.EndUpdate();

            // 貿易リストビューを有効化する
            tradeListView.Enabled = true;

            // 新規ボタンを有効化する
            tradeNewButton.Enabled = true;
        }

        /// <summary>
        ///     貿易タブの編集項目を無効化する
        /// </summary>
        private void DisableTradeItems()
        {
            // 編集項目を無効化する
            tradeInfoGroupBox.Enabled = false;
            tradeDealGroupBox.Enabled = false;

            tradeUpButton.Enabled = false;
            tradeDownButton.Enabled = false;
            tradeRemoveButton.Enabled = false;

            // 編集項目をクリアする
            tradeStartYearTextBox.Text = "";
            tradeStartMonthTextBox.Text = "";
            tradeStartDayTextBox.Text = "";
            tradeEndYearTextBox.Text = "";
            tradeEndMonthTextBox.Text = "";
            tradeEndDayTextBox.Text = "";
            tradeTypeTextBox.Text = "";
            tradeIdTextBox.Text = "";
            tradeCancelCheckBox.Checked = false;

            tradeCountryComboBox1.SelectedIndex = -1;
            tradeCountryComboBox2.SelectedIndex = -1;
            tradeEnergyTextBox1.Text = "";
            tradeEnergyTextBox2.Text = "";
            tradeMetalTextBox1.Text = "";
            tradeMetalTextBox2.Text = "";
            tradeRareMaterialsTextBox1.Text = "";
            tradeRareMaterialsTextBox2.Text = "";
            tradeOilTextBox1.Text = "";
            tradeOilTextBox2.Text = "";
            tradeSuppliesTextBox1.Text = "";
            tradeSuppliesTextBox2.Text = "";
            tradeMoneyTextBox1.Text = "";
            tradeMoneyTextBox2.Text = "";
        }

        /// <summary>
        ///     貿易タブの編集項目を更新する
        /// </summary>
        private void UpdateTradeItems()
        {
            Treaty trade = GetSelectedTrade();
            if (trade == null)
            {
                return;
            }

            // 開始日時
            bool flag = (trade.StartDate != null);
            tradeStartYearTextBox.Text = flag ? IntHelper.ToString(trade.StartDate.Year) : "";
            tradeStartMonthTextBox.Text = flag ? IntHelper.ToString(trade.StartDate.Month) : "";
            tradeStartDayTextBox.Text = flag ? IntHelper.ToString(trade.StartDate.Day) : "";

            tradeStartYearTextBox.ForeColor = trade.IsDirty(TreatyItemId.StartYear)
                ? Color.Red
                : SystemColors.WindowText;
            tradeStartMonthTextBox.ForeColor = trade.IsDirty(TreatyItemId.StartMonth)
                ? Color.Red
                : SystemColors.WindowText;
            tradeStartDayTextBox.ForeColor = trade.IsDirty(TreatyItemId.StartDay) ? Color.Red : SystemColors.WindowText;

            // 終了日時
            flag = (trade.EndDate != null);
            tradeEndYearTextBox.Text = flag ? IntHelper.ToString(trade.EndDate.Year) : "";
            tradeEndMonthTextBox.Text = flag ? IntHelper.ToString(trade.EndDate.Month) : "";
            tradeEndDayTextBox.Text = flag ? IntHelper.ToString(trade.EndDate.Day) : "";

            tradeEndYearTextBox.ForeColor = trade.IsDirty(TreatyItemId.EndYear) ? Color.Red : SystemColors.WindowText;
            tradeEndMonthTextBox.ForeColor = trade.IsDirty(TreatyItemId.EndMonth) ? Color.Red : SystemColors.WindowText;
            tradeEndDayTextBox.ForeColor = trade.IsDirty(TreatyItemId.EndDay) ? Color.Red : SystemColors.WindowText;

            // ID
            flag = (trade.Id != null);
            tradeTypeTextBox.Text = flag ? IntHelper.ToString(trade.Id.Type) : "";
            tradeIdTextBox.Text = flag ? IntHelper.ToString(trade.Id.Id) : "";

            tradeTypeTextBox.ForeColor = trade.IsDirty(TreatyItemId.Type) ? Color.Red : SystemColors.WindowText;
            tradeIdTextBox.ForeColor = trade.IsDirty(TreatyItemId.Id) ? Color.Red : SystemColors.WindowText;

            // キャンセルを許可
            tradeCancelCheckBox.Checked = trade.Cancel;
            tradeCancelCheckBox.ForeColor = trade.IsDirty(TreatyItemId.Cancel) ? Color.Red : SystemColors.WindowText;

            // 貿易国家コンボボックス
            if (Countries.Tags.Contains(trade.Country1))
            {
                tradeCountryComboBox1.SelectedIndex = Array.IndexOf(Countries.Tags, trade.Country1);
            }
            if (Countries.Tags.Contains(trade.Country2))
            {
                tradeCountryComboBox2.SelectedIndex = Array.IndexOf(Countries.Tags, trade.Country2);
            }

            // 貿易量
            tradeEnergyTextBox1.Text = (trade.Energy < 0) ? DoubleHelper.ToString(-trade.Energy) : "";
            tradeEnergyTextBox2.Text = (trade.Energy > 0) ? DoubleHelper.ToString(trade.Energy) : "";
            tradeMetalTextBox1.Text = (trade.Metal < 0) ? DoubleHelper.ToString(-trade.Metal) : "";
            tradeMetalTextBox2.Text = (trade.Metal > 0) ? DoubleHelper.ToString(trade.Metal) : "";
            tradeRareMaterialsTextBox1.Text = (trade.RareMaterials < 0)
                ? DoubleHelper.ToString(-trade.RareMaterials)
                : "";
            tradeRareMaterialsTextBox2.Text = (trade.RareMaterials > 0)
                ? DoubleHelper.ToString(trade.RareMaterials)
                : "";
            tradeOilTextBox1.Text = (trade.Oil < 0) ? DoubleHelper.ToString(-trade.Oil) : "";
            tradeOilTextBox2.Text = (trade.Oil > 0) ? DoubleHelper.ToString(trade.Oil) : "";
            tradeSuppliesTextBox1.Text = (trade.Supplies < 0) ? DoubleHelper.ToString(-trade.Supplies) : "";
            tradeSuppliesTextBox2.Text = (trade.Supplies > 0) ? DoubleHelper.ToString(trade.Supplies) : "";
            tradeMoneyTextBox1.Text = (trade.Money < 0) ? DoubleHelper.ToString(-trade.Money) : "";
            tradeMoneyTextBox2.Text = (trade.Money > 0) ? DoubleHelper.ToString(trade.Money) : "";

            tradeEnergyTextBox1.ForeColor = trade.IsDirty(TreatyItemId.Energy) ? Color.Red : SystemColors.WindowText;
            tradeEnergyTextBox2.ForeColor = trade.IsDirty(TreatyItemId.Energy) ? Color.Red : SystemColors.WindowText;
            tradeMetalTextBox1.ForeColor = trade.IsDirty(TreatyItemId.Metal) ? Color.Red : SystemColors.WindowText;
            tradeMetalTextBox2.ForeColor = trade.IsDirty(TreatyItemId.Metal) ? Color.Red : SystemColors.WindowText;
            tradeRareMaterialsTextBox1.ForeColor = trade.IsDirty(TreatyItemId.RareMaterials)
                ? Color.Red
                : SystemColors.WindowText;
            tradeRareMaterialsTextBox2.ForeColor = trade.IsDirty(TreatyItemId.RareMaterials)
                ? Color.Red
                : SystemColors.WindowText;
            tradeOilTextBox1.ForeColor = trade.IsDirty(TreatyItemId.Oil) ? Color.Red : SystemColors.WindowText;
            tradeOilTextBox2.ForeColor = trade.IsDirty(TreatyItemId.Oil) ? Color.Red : SystemColors.WindowText;
            tradeSuppliesTextBox1.ForeColor = trade.IsDirty(TreatyItemId.Supplies) ? Color.Red : SystemColors.WindowText;
            tradeSuppliesTextBox2.ForeColor = trade.IsDirty(TreatyItemId.Supplies) ? Color.Red : SystemColors.WindowText;
            tradeMoneyTextBox1.ForeColor = trade.IsDirty(TreatyItemId.Money) ? Color.Red : SystemColors.WindowText;
            tradeMoneyTextBox2.ForeColor = trade.IsDirty(TreatyItemId.Money) ? Color.Red : SystemColors.WindowText;

            // 編集項目を有効化する
            tradeInfoGroupBox.Enabled = true;
            tradeDealGroupBox.Enabled = true;

            int index = tradeListView.SelectedIndices[0];
            int count = tradeListView.Items.Count;
            tradeUpButton.Enabled = (index > 0);
            tradeDownButton.Enabled = (index < count - 1);
            tradeRemoveButton.Enabled = true;
        }

        /// <summary>
        ///     貿易リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ編集項目を無効化する
            if (tradeListView.SelectedIndices.Count == 0)
            {
                DisableTradeItems();
                return;
            }

            // 編集項目を更新する
            UpdateTradeItems();
        }

        /// <summary>
        ///     貿易の上へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeUpButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<Treaty> trades = scenario.GlobalData.Trades;

            // 貿易リストビューの項目を移動する
            int index = tradeListView.SelectedIndices[0];
            ListViewItem item = tradeListView.Items[index];
            tradeListView.Items.RemoveAt(index);
            tradeListView.Items.Insert(index - 1, item);
            tradeListView.Items[index - 1].Focused = true;
            tradeListView.Items[index - 1].Selected = true;
            tradeListView.EnsureVisible(index - 1);

            // 貿易リストの項目を移動する
            Treaty trade = trades[index];
            trades.RemoveAt(index);
            trades.Insert(index - 1, trade);

            // 編集済みフラグを設定する
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     貿易の下へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeDownButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<Treaty> trades = scenario.GlobalData.Trades;

            // 貿易リストビューの項目を移動する
            int index = tradeListView.SelectedIndices[0];
            ListViewItem item = tradeListView.Items[index];
            tradeListView.Items.RemoveAt(index);
            tradeListView.Items.Insert(index + 1, item);
            tradeListView.Items[index + 1].Focused = true;
            tradeListView.Items[index + 1].Selected = true;
            tradeListView.EnsureVisible(index + 1);

            // 貿易リストの項目を移動する
            Treaty trade = trades[index];
            trades.RemoveAt(index);
            trades.Insert(index + 1, trade);

            // 編集済みフラグを設定する
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     貿易の新規ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeNewButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<Treaty> trades = scenario.GlobalData.Trades;

            // 貿易リストに項目を追加する
            Treaty trade = new Treaty
            {
                StartDate = new GameDate(),
                EndDate = new GameDate(),
                Id = Scenarios.GetNewTypeId(Scenarios.DefaultTreatyType, 1)
            };
            trades.Add(trade);

            // 貿易リストビューに項目を追加する
            ListViewItem item = new ListViewItem { Tag = trade };
            item.SubItems.Add("");
            item.SubItems.Add("");
            tradeListView.Items.Add(item);

            // 編集済みフラグを設定する
            trade.SetDirty(TreatyItemId.StartYear);
            trade.SetDirty(TreatyItemId.StartMonth);
            trade.SetDirty(TreatyItemId.StartDay);
            trade.SetDirty(TreatyItemId.EndYear);
            trade.SetDirty(TreatyItemId.EndMonth);
            trade.SetDirty(TreatyItemId.EndDay);
            trade.SetDirty(TreatyItemId.Type);
            trade.SetDirty(TreatyItemId.Id);
            trade.SetDirty(TreatyItemId.Cancel);
            Scenarios.SetDirty();

            // 追加した項目を選択する
            if (tradeListView.SelectedIndices.Count > 0)
            {
                ListViewItem prev = tradeListView.SelectedItems[0];
                prev.Focused = false;
                prev.Selected = false;
            }
            item.Focused = true;
            item.Selected = true;
        }

        /// <summary>
        ///     貿易の削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeRemoveButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<Treaty> trades = scenario.GlobalData.Trades;

            int index = tradeListView.SelectedIndices[0];
            Treaty trade = trades[index];

            // typeとidの組を削除する
            Scenarios.RemoveTypeId(trade.Id);

            // 貿易リストから項目を削除する
            trades.RemoveAt(index);

            // 貿易リストビューから項目を削除する
            tradeListView.Items.RemoveAt(index);

            // 編集済みフラグを設定する
            Scenarios.SetDirty();

            // 削除した項目の次を選択する
            if (index == trades.Count)
            {
                index--;
            }
            tradeListView.Items[index].Focused = true;
            tradeListView.Items[index].Selected = true;
        }

        /// <summary>
        ///     選択中の貿易情報を取得する
        /// </summary>
        /// <returns>選択中の貿易情報</returns>
        private Treaty GetSelectedTrade()
        {
            return (tradeListView.SelectedItems.Count > 0) ? tradeListView.SelectedItems[0].Tag as Treaty : null;
        }

        /// <summary>
        ///     貿易リストビューの項目を作成する
        /// </summary>
        /// <param name="trade">貿易情報</param>
        /// <returns>貿易リストビューの項目</returns>
        private ListViewItem CreateTradeListViewItem(Treaty trade)
        {
            ListViewItem item = new ListViewItem
            {
                Text = Countries.GetName(trade.Country1),
                Tag = trade
            };
            item.SubItems.Add(Countries.GetName(trade.Country2));
            item.SubItems.Add(GetTradeString(trade));

            return item;
        }

        /// <summary>
        ///     貿易内容の文字列を取得する
        /// </summary>
        /// <param name="trade">外交協定情報</param>
        /// <returns>貿易内容の文字列</returns>
        private static string GetTradeString(Treaty trade)
        {
            StringBuilder sb = new StringBuilder();
            if (!DoubleHelper.IsZero(trade.Energy))
            {
                sb.AppendFormat("{0}:{1}, ", Config.GetText(TextId.ResourceEnergy), DoubleHelper.ToString1(trade.Energy));
            }
            if (!DoubleHelper.IsZero(trade.Metal))
            {
                sb.AppendFormat("{0}:{1}, ", Config.GetText(TextId.ResourceMetal), DoubleHelper.ToString1(trade.Metal));
            }
            if (!DoubleHelper.IsZero(trade.RareMaterials))
            {
                sb.AppendFormat("{0}:{1}, ", Config.GetText(TextId.ResourceRareMaterials),
                    DoubleHelper.ToString1(trade.RareMaterials));
            }
            if (!DoubleHelper.IsZero(trade.Oil))
            {
                sb.AppendFormat("{0}:{1}, ", Config.GetText(TextId.ResourceOil), DoubleHelper.ToString1(trade.Oil));
            }
            if (!DoubleHelper.IsZero(trade.Supplies))
            {
                sb.AppendFormat("{0}:{1}, ", Config.GetText(TextId.ResourceSupplies),
                    DoubleHelper.ToString1(trade.Supplies));
            }
            if (!DoubleHelper.IsZero(trade.Money))
            {
                sb.AppendFormat("{0}:{1}, ", Config.GetText(TextId.ResourceMoney), DoubleHelper.ToString1(trade.Money));
            }
            int len = sb.Length;
            return (len > 0) ? sb.ToString(0, len - 2) : "";
        }

        #endregion

        #region 貿易タブ - 貿易情報

        /// <summary>
        ///     貿易開始年テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeStartYearTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty trade = GetSelectedTrade();
            if (trade == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(tradeStartYearTextBox.Text, out val))
            {
                tradeStartYearTextBox.Text = (trade.Id != null) ? IntHelper.ToString(trade.Id.Id) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((trade.StartDate != null) && (val == trade.StartDate.Year))
            {
                return;
            }

            Log.Info("[Scenario] trade start year: {0} -> {1} ({2})",
                (trade.StartDate != null) ? IntHelper.ToString(trade.StartDate.Year) : "", val,
                tradeListView.SelectedIndices[0]);

            if (trade.StartDate == null)
            {
                trade.StartDate = new GameDate();

                // 編集済みフラグを設定する
                trade.SetDirty(TreatyItemId.StartMonth);
                trade.SetDirty(TreatyItemId.StartDay);

                // 編集項目を更新する
                tradeStartMonthTextBox.Text = IntHelper.ToString(trade.StartDate.Month);
                tradeStartDayTextBox.Text = IntHelper.ToString(trade.StartDate.Day);

                // 文字色を変更する
                tradeStartMonthTextBox.ForeColor = Color.Red;
                tradeStartDayTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            trade.StartDate.Year = val;

            // 編集済みフラグを設定する
            trade.SetDirty(TreatyItemId.StartYear);
            Scenarios.SetDirty();

            // 文字色を変更する
            tradeStartYearTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     貿易開始月テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeStartMonthTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty trade = GetSelectedTrade();
            if (trade == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(tradeStartMonthTextBox.Text, out val))
            {
                tradeStartMonthTextBox.Text = (trade.Id != null) ? IntHelper.ToString(trade.Id.Id) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((trade.StartDate != null) && (val == trade.StartDate.Month))
            {
                return;
            }

            Log.Info("[Scenario] trade start month: {0} -> {1} ({2})",
                (trade.StartDate != null) ? IntHelper.ToString(trade.StartDate.Month) : "", val,
                tradeListView.SelectedIndices[0]);

            if (trade.StartDate == null)
            {
                trade.StartDate = new GameDate();

                // 編集済みフラグを設定する
                trade.SetDirty(TreatyItemId.StartYear);
                trade.SetDirty(TreatyItemId.StartDay);

                // 編集項目を更新する
                tradeStartYearTextBox.Text = IntHelper.ToString(trade.StartDate.Year);
                tradeStartDayTextBox.Text = IntHelper.ToString(trade.StartDate.Day);

                // 文字色を変更する
                tradeStartYearTextBox.ForeColor = Color.Red;
                tradeStartDayTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            trade.StartDate.Month = val;

            // 編集済みフラグを設定する
            trade.SetDirty(TreatyItemId.StartMonth);
            Scenarios.SetDirty();

            // 文字色を変更する
            tradeStartMonthTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     貿易開始日テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeStartDayTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty trade = GetSelectedTrade();
            if (trade == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(tradeStartDayTextBox.Text, out val))
            {
                tradeStartDayTextBox.Text = (trade.Id != null) ? IntHelper.ToString(trade.Id.Id) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((trade.StartDate != null) && (val == trade.StartDate.Day))
            {
                return;
            }

            Log.Info("[Scenario] trade start day: {0} -> {1} ({2})",
                (trade.StartDate != null) ? IntHelper.ToString(trade.StartDate.Day) : "", val,
                tradeListView.SelectedIndices[0]);

            if (trade.StartDate == null)
            {
                trade.StartDate = new GameDate();

                // 編集済みフラグを設定する
                trade.SetDirty(TreatyItemId.StartYear);
                trade.SetDirty(TreatyItemId.StartMonth);

                // 編集項目を更新する
                tradeStartYearTextBox.Text = IntHelper.ToString(trade.StartDate.Year);
                tradeStartMonthTextBox.Text = IntHelper.ToString(trade.StartDate.Month);

                // 文字色を変更する
                tradeStartYearTextBox.ForeColor = Color.Red;
                tradeStartMonthTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            trade.StartDate.Day = val;

            // 編集済みフラグを設定する
            trade.SetDirty(TreatyItemId.StartDay);
            Scenarios.SetDirty();

            // 文字色を変更する
            tradeStartDayTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     貿易終了年テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeEndYearTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty trade = GetSelectedTrade();
            if (trade == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(tradeEndYearTextBox.Text, out val))
            {
                tradeEndYearTextBox.Text = (trade.Id != null) ? IntHelper.ToString(trade.Id.Id) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((trade.EndDate != null) && (val == trade.EndDate.Year))
            {
                return;
            }

            Log.Info("[Scenario] trade end year: {0} -> {1} ({2})",
                (trade.EndDate != null) ? IntHelper.ToString(trade.EndDate.Year) : "", val,
                tradeListView.SelectedIndices[0]);

            if (trade.EndDate == null)
            {
                trade.EndDate = new GameDate();

                // 編集済みフラグを設定する
                trade.SetDirty(TreatyItemId.EndMonth);
                trade.SetDirty(TreatyItemId.EndDay);

                // 編集項目を更新する
                tradeEndMonthTextBox.Text = IntHelper.ToString(trade.EndDate.Month);
                tradeEndDayTextBox.Text = IntHelper.ToString(trade.EndDate.Day);

                // 文字色を変更する
                tradeEndMonthTextBox.ForeColor = Color.Red;
                tradeEndDayTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            trade.EndDate.Year = val;

            // 編集済みフラグを設定する
            trade.SetDirty(TreatyItemId.EndYear);
            Scenarios.SetDirty();

            // 文字色を変更する
            tradeEndYearTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     貿易終了月テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeEndMonthTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty trade = GetSelectedTrade();
            if (trade == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(tradeEndMonthTextBox.Text, out val))
            {
                tradeEndMonthTextBox.Text = (trade.Id != null) ? IntHelper.ToString(trade.Id.Id) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((trade.EndDate != null) && (val == trade.EndDate.Month))
            {
                return;
            }

            Log.Info("[Scenario] trade end month: {0} -> {1} ({2})",
                (trade.EndDate != null) ? IntHelper.ToString(trade.EndDate.Month) : "", val,
                tradeListView.SelectedIndices[0]);

            if (trade.EndDate == null)
            {
                trade.EndDate = new GameDate();

                // 編集済みフラグを設定する
                trade.SetDirty(TreatyItemId.EndYear);
                trade.SetDirty(TreatyItemId.EndDay);

                // 編集項目を更新する
                tradeEndYearTextBox.Text = IntHelper.ToString(trade.EndDate.Year);
                tradeEndDayTextBox.Text = IntHelper.ToString(trade.EndDate.Day);

                // 文字色を変更する
                tradeEndYearTextBox.ForeColor = Color.Red;
                tradeEndDayTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            trade.EndDate.Month = val;

            // 編集済みフラグを設定する
            trade.SetDirty(TreatyItemId.EndMonth);
            Scenarios.SetDirty();

            // 文字色を変更する
            tradeEndMonthTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     貿易終了日テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeEndDayTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty trade = GetSelectedTrade();
            if (trade == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(tradeEndDayTextBox.Text, out val))
            {
                tradeEndDayTextBox.Text = (trade.Id != null) ? IntHelper.ToString(trade.Id.Id) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((trade.EndDate != null) && (val == trade.EndDate.Day))
            {
                return;
            }

            Log.Info("[Scenario] trade end day: {0} -> {1} ({2})",
                (trade.EndDate != null) ? IntHelper.ToString(trade.EndDate.Day) : "", val,
                tradeListView.SelectedIndices[0]);

            if (trade.EndDate == null)
            {
                trade.EndDate = new GameDate();

                // 編集済みフラグを設定する
                trade.SetDirty(TreatyItemId.EndYear);
                trade.SetDirty(TreatyItemId.EndMonth);

                // 編集項目を更新する
                tradeEndYearTextBox.Text = IntHelper.ToString(trade.EndDate.Year);
                tradeEndMonthTextBox.Text = IntHelper.ToString(trade.EndDate.Month);

                // 文字色を変更する
                tradeEndYearTextBox.ForeColor = Color.Red;
                tradeEndMonthTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            trade.EndDate.Day = val;

            // 編集済みフラグを設定する
            trade.SetDirty(TreatyItemId.EndDay);
            Scenarios.SetDirty();

            // 文字色を変更する
            tradeEndDayTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     貿易のtypeテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeTypeTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty trade = GetSelectedTrade();
            if (trade == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(tradeTypeTextBox.Text, out val))
            {
                tradeTypeTextBox.Text = (trade.Id != null) ? IntHelper.ToString(trade.Id.Type) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((trade.Id != null) && (val == trade.Id.Type))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if ((trade.Id != null) && Scenarios.ExistsTypeId(val, trade.Id.Id))
            {
                tradeTypeTextBox.Text = IntHelper.ToString(trade.Id.Type);
                return;
            }

            Log.Info("[Scenario] trade type: {0} -> {1} ({2})",
                (trade.Id != null) ? IntHelper.ToString(trade.Id.Type) : "",
                val, tradeListView.SelectedIndices[0]);

            if (trade.Id != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(trade.Id);

                // 値を更新する
                trade.Id.Type = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(trade.Id);
            }
            else
            {
                // 値を更新する
                trade.Id = Scenarios.GetNewTypeId(Scenarios.DefaultTreatyType, 1);

                // 編集済みフラグを設定する
                trade.SetDirty(TreatyItemId.Id);

                // 編集項目を更新する
                tradeIdTextBox.Text = IntHelper.ToString(trade.Id.Id);

                // 文字色を変更する
                tradeIdTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            trade.SetDirty(TreatyItemId.Type);
            Scenarios.SetDirty();

            // 文字色を変更する
            tradeTypeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     貿易のidテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeIdTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty trade = GetSelectedTrade();
            if (trade == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(tradeIdTextBox.Text, out val))
            {
                tradeIdTextBox.Text = (trade.Id != null) ? IntHelper.ToString(trade.Id.Id) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((trade.Id != null) && (val == trade.Id.Id))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if (Scenarios.ExistsTypeId((trade.Id != null) ? trade.Id.Type : Scenarios.DefaultTreatyType, val))
            {
                tradeIdTextBox.Text = (trade.Id != null) ? IntHelper.ToString(trade.Id.Id) : "";
                return;
            }

            Log.Info("[Scenario] trade id: {0} -> {1} ({2})", (trade.Id != null) ? IntHelper.ToString(trade.Id.Id) : "",
                val,
                tradeListView.SelectedIndices[0]);

            if (trade.Id != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(trade.Id);

                // 値を更新する
                trade.Id.Id = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(trade.Id);
            }
            else
            {
                // 値を更新する
                trade.Id = Scenarios.GetNewTypeId(Scenarios.DefaultTreatyType, val);

                // 編集済みフラグを設定する
                trade.SetDirty(TreatyItemId.Type);

                // 編集項目を更新する
                tradeTypeTextBox.Text = IntHelper.ToString(trade.Id.Type);

                // 文字色を変更する
                tradeTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            trade.SetDirty(TreatyItemId.Id);
            Scenarios.SetDirty();

            // 文字色を変更する
            tradeIdTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     貿易のキャンセルを許可チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeCancelCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty trade = GetSelectedTrade();
            if (trade == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            bool val = tradeCancelCheckBox.Checked;
            if (val == trade.Cancel)
            {
                return;
            }

            Log.Info("[Scenario] trade cancel: {0} -> {1} ({2})", BoolHelper.ToYesNo(trade.Cancel),
                BoolHelper.ToYesNo(val), tradeListView.SelectedIndices[0]);

            // 値を更新する
            trade.Cancel = val;

            // 編集済みフラグを設定する
            trade.SetDirty(TreatyItemId.Cancel);
            Scenarios.SetDirty();

            // 文字色を変更する
            tradeCancelCheckBox.ForeColor = Color.Red;
        }

        #endregion

        #region 貿易タブ - 貿易内容

        /// <summary>
        ///     貿易国コンボボックス1の項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeCountryComboBox1DrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Treaty trade = GetSelectedTrade();
            if (trade != null)
            {
                bool dirty = ((Countries.Tags[e.Index] == trade.Country1) && trade.IsDirty(TreatyItemId.Country1));
                Brush brush = new SolidBrush(dirty ? Color.Red : comboBox.ForeColor);
                string s = comboBox.Items[e.Index].ToString();
                e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     貿易国コンボボックス2の項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeCountryComboBox2DrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Treaty trade = GetSelectedTrade();
            if (trade != null)
            {
                bool dirty = ((Countries.Tags[e.Index] == trade.Country2) && trade.IsDirty(TreatyItemId.Country2));
                Brush brush = new SolidBrush(dirty ? Color.Red : comboBox.ForeColor);
                string s = comboBox.Items[e.Index].ToString();
                e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     貿易国コンボボックス1の選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeCountryComboBox1SelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (tradeCountryComboBox1.SelectedIndex < 0)
            {
                return;
            }
            Treaty trade = GetSelectedTrade();
            if (trade == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            Country country = Countries.Tags[tradeCountryComboBox1.SelectedIndex];
            if (country == trade.Country1)
            {
                return;
            }

            Log.Info("[Scenario] trade country1: {0} -> {1} ({2})", Countries.Strings[(int) trade.Country1],
                Countries.Strings[(int) country], tradeListView.SelectedIndices[0]);

            // 値を更新する
            trade.Country1 = country;

            // 編集済みフラグを設定する
            trade.SetDirty(TreatyItemId.Country1);
            Scenarios.SetDirty();

            // 貿易リストビューの項目を更新する
            tradeListView.SelectedItems[0].Text = Countries.GetName(country);

            // 項目色を変更するため描画更新する
            tradeCountryComboBox1.Refresh();
        }

        /// <summary>
        ///     貿易国コンボボックス2の選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeCountryComboBox2SelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (tradeCountryComboBox2.SelectedIndex < 0)
            {
                return;
            }
            Treaty trade = GetSelectedTrade();
            if (trade == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            Country country = Countries.Tags[tradeCountryComboBox2.SelectedIndex];
            if (country == trade.Country2)
            {
                return;
            }

            Log.Info("[Scenario] trade country2: {0} -> {1} ({2})", Countries.Strings[(int) trade.Country2],
                Countries.Strings[(int) country], tradeListView.SelectedIndices[0]);

            // 値を更新する
            trade.Country2 = country;

            // 編集済みフラグを設定する
            trade.SetDirty(TreatyItemId.Country2);
            Scenarios.SetDirty();

            // 貿易リストビューの項目を更新する
            tradeListView.SelectedItems[0].SubItems[1].Text = Countries.GetName(country);

            // 項目色を変更するため描画更新する
            tradeCountryComboBox2.Refresh();
        }

        /// <summary>
        ///     貿易国入れ替えボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeSwapButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty trade = GetSelectedTrade();
            if (trade == null)
            {
                return;
            }

            // 値を入れ替える
            Country country = trade.Country1;
            trade.Country1 = trade.Country2;
            trade.Country2 = country;

            trade.Energy = -trade.Energy;
            trade.Metal = -trade.Metal;
            trade.RareMaterials = -trade.RareMaterials;
            trade.Oil = -trade.Oil;
            trade.Supplies = -trade.Supplies;
            trade.Money = -trade.Money;

            // 編集済みフラグを設定する
            trade.SetDirty(TreatyItemId.Country1);
            trade.SetDirty(TreatyItemId.Country2);
            trade.SetDirty(TreatyItemId.Energy);
            trade.SetDirty(TreatyItemId.Metal);
            trade.SetDirty(TreatyItemId.RareMaterials);
            trade.SetDirty(TreatyItemId.Oil);
            trade.SetDirty(TreatyItemId.Supplies);
            trade.SetDirty(TreatyItemId.Money);
            Scenarios.SetDirty();

            // 貿易リストビューの項目を更新する
            ListViewItem item = tradeListView.SelectedItems[0];
            item.Text = Countries.GetName(trade.Country1);
            item.SubItems[1].Text = Countries.GetName(trade.Country2);
            item.SubItems[2].Text = GetTradeString(trade);

            // 編集項目を更新する
            UpdateTradeItems();
        }

        /// <summary>
        ///     エネルギー貿易量テキストボックス1のフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeEnergyTextBox1Validated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty trade = GetSelectedTrade();
            if (trade == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(tradeEnergyTextBox1.Text, out val))
            {
                tradeEnergyTextBox1.Text = (trade.Energy < 0) ? DoubleHelper.ToString(-trade.Energy) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(-val, trade.Energy))
            {
                return;
            }

            Log.Info("[Scenario] trade energy: {0} -> {1} ({2})", DoubleHelper.ToString(trade.Energy), -val,
                tradeListView.SelectedIndices[0]);

            // 値を更新する
            trade.Energy = -val;

            // 編集済みフラグを設定する
            trade.SetDirty(TreatyItemId.Energy);
            Scenarios.SetDirty();

            // 編集項目を更新する
            tradeEnergyTextBox1.Text = (trade.Energy < 0) ? DoubleHelper.ToString(-trade.Energy) : "";
            tradeEnergyTextBox2.Text = (trade.Energy > 0) ? DoubleHelper.ToString(trade.Energy) : "";

            // 文字色を変更する
            tradeEnergyTextBox1.ForeColor = Color.Red;
            tradeEnergyTextBox2.ForeColor = Color.Red;

            // 貿易リストビューの項目を更新する
            tradeListView.SelectedItems[0].SubItems[2].Text = GetTradeString(trade);
        }

        /// <summary>
        ///     エネルギー貿易量テキストボックス2のフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeEnergyTextBox2Validated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty trade = GetSelectedTrade();
            if (trade == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(tradeEnergyTextBox2.Text, out val))
            {
                tradeEnergyTextBox2.Text = (trade.Energy > 0) ? DoubleHelper.ToString(trade.Energy) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, trade.Energy))
            {
                return;
            }

            Log.Info("[Scenario] trade energy: {0} -> {1} ({2})", DoubleHelper.ToString(trade.Energy), val,
                tradeListView.SelectedIndices[0]);

            // 値を更新する
            trade.Energy = val;

            // 編集済みフラグを設定する
            trade.SetDirty(TreatyItemId.Energy);
            Scenarios.SetDirty();

            // 編集項目を更新する
            tradeEnergyTextBox1.Text = (trade.Energy < 0) ? DoubleHelper.ToString(-trade.Energy) : "";
            tradeEnergyTextBox2.Text = (trade.Energy > 0) ? DoubleHelper.ToString(trade.Energy) : "";

            // 文字色を変更する
            tradeEnergyTextBox1.ForeColor = Color.Red;
            tradeEnergyTextBox2.ForeColor = Color.Red;

            // 貿易リストビューの項目を更新する
            tradeListView.SelectedItems[0].SubItems[2].Text = GetTradeString(trade);
        }

        /// <summary>
        ///     金属貿易量テキストボックス1のフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeMetalTextBox1Validated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty trade = GetSelectedTrade();
            if (trade == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(tradeMetalTextBox1.Text, out val))
            {
                tradeMetalTextBox1.Text = (trade.Metal < 0) ? DoubleHelper.ToString(-trade.Metal) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(-val, trade.Metal))
            {
                return;
            }

            Log.Info("[Scenario] trade metal: {0} -> {1} ({2})", DoubleHelper.ToString(trade.Metal), -val,
                tradeListView.SelectedIndices[0]);

            // 値を更新する
            trade.Metal = -val;

            // 編集済みフラグを設定する
            trade.SetDirty(TreatyItemId.Metal);
            Scenarios.SetDirty();

            // 編集項目を更新する
            tradeMetalTextBox1.Text = (trade.Metal < 0) ? DoubleHelper.ToString(-trade.Metal) : "";
            tradeMetalTextBox2.Text = (trade.Metal > 0) ? DoubleHelper.ToString(trade.Metal) : "";

            // 文字色を変更する
            tradeMetalTextBox1.ForeColor = Color.Red;
            tradeMetalTextBox2.ForeColor = Color.Red;

            // 貿易リストビューの項目を更新する
            tradeListView.SelectedItems[0].SubItems[2].Text = GetTradeString(trade);
        }

        /// <summary>
        ///     金属貿易量テキストボックス2のフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeMetalTextBox2Validated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty trade = GetSelectedTrade();
            if (trade == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(tradeMetalTextBox2.Text, out val))
            {
                tradeMetalTextBox2.Text = (trade.Metal > 0) ? DoubleHelper.ToString(trade.Metal) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, trade.Metal))
            {
                return;
            }

            Log.Info("[Scenario] trade metal: {0} -> {1} ({2})", DoubleHelper.ToString(trade.Metal), val,
                tradeListView.SelectedIndices[0]);

            // 値を更新する
            trade.Metal = val;

            // 編集済みフラグを設定する
            trade.SetDirty(TreatyItemId.Metal);
            Scenarios.SetDirty();

            // 編集項目を更新する
            tradeMetalTextBox1.Text = (trade.Metal < 0) ? DoubleHelper.ToString(-trade.Metal) : "";
            tradeMetalTextBox2.Text = (trade.Metal > 0) ? DoubleHelper.ToString(trade.Metal) : "";

            // 文字色を変更する
            tradeMetalTextBox1.ForeColor = Color.Red;
            tradeMetalTextBox2.ForeColor = Color.Red;

            // 貿易リストビューの項目を更新する
            tradeListView.SelectedItems[0].SubItems[2].Text = GetTradeString(trade);
        }

        /// <summary>
        ///     希少資源貿易量テキストボックス1のフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeRareMaterialsTextBox1Validated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty trade = GetSelectedTrade();
            if (trade == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(tradeRareMaterialsTextBox1.Text, out val))
            {
                tradeRareMaterialsTextBox1.Text = (trade.RareMaterials < 0)
                    ? DoubleHelper.ToString(-trade.RareMaterials)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(-val, trade.RareMaterials))
            {
                return;
            }

            Log.Info("[Scenario] trade rare materials: {0} -> {1} ({2})", DoubleHelper.ToString(trade.RareMaterials),
                -val, tradeListView.SelectedIndices[0]);

            // 値を更新する
            trade.RareMaterials = -val;

            // 編集済みフラグを設定する
            trade.SetDirty(TreatyItemId.RareMaterials);
            Scenarios.SetDirty();

            // 編集項目を更新する
            tradeRareMaterialsTextBox1.Text = (trade.RareMaterials < 0)
                ? DoubleHelper.ToString(-trade.RareMaterials)
                : "";
            tradeRareMaterialsTextBox2.Text = (trade.RareMaterials > 0)
                ? DoubleHelper.ToString(trade.RareMaterials)
                : "";

            // 文字色を変更する
            tradeRareMaterialsTextBox1.ForeColor = Color.Red;
            tradeRareMaterialsTextBox2.ForeColor = Color.Red;

            // 貿易リストビューの項目を更新する
            tradeListView.SelectedItems[0].SubItems[2].Text = GetTradeString(trade);
        }

        /// <summary>
        ///     希少資源貿易量テキストボックス2のフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeRareMaterialsTextBox2Validated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty trade = GetSelectedTrade();
            if (trade == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(tradeRareMaterialsTextBox2.Text, out val))
            {
                tradeRareMaterialsTextBox2.Text = (trade.RareMaterials > 0)
                    ? DoubleHelper.ToString(trade.RareMaterials)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, trade.RareMaterials))
            {
                return;
            }

            Log.Info("[Scenario] trade rare materials: {0} -> {1} ({2})", DoubleHelper.ToString(trade.RareMaterials),
                val, tradeListView.SelectedIndices[0]);

            // 値を更新する
            trade.RareMaterials = val;

            // 編集済みフラグを設定する
            trade.SetDirty(TreatyItemId.RareMaterials);
            Scenarios.SetDirty();

            // 編集項目を更新する
            tradeRareMaterialsTextBox1.Text = (trade.RareMaterials < 0)
                ? DoubleHelper.ToString(-trade.RareMaterials)
                : "";
            tradeRareMaterialsTextBox2.Text = (trade.RareMaterials > 0)
                ? DoubleHelper.ToString(trade.RareMaterials)
                : "";

            // 文字色を変更する
            tradeRareMaterialsTextBox1.ForeColor = Color.Red;
            tradeRareMaterialsTextBox2.ForeColor = Color.Red;

            // 貿易リストビューの項目を更新する
            tradeListView.SelectedItems[0].SubItems[2].Text = GetTradeString(trade);
        }

        /// <summary>
        ///     石油貿易量テキストボックス1のフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeOilTextBox1Validated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty trade = GetSelectedTrade();
            if (trade == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(tradeOilTextBox1.Text, out val))
            {
                tradeOilTextBox1.Text = (trade.Oil < 0) ? DoubleHelper.ToString(-trade.Oil) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(-val, trade.Oil))
            {
                return;
            }

            Log.Info("[Scenario] trade oil: {0} -> {1} ({2})", DoubleHelper.ToString(trade.Oil), -val,
                tradeListView.SelectedIndices[0]);

            // 値を更新する
            trade.Oil = -val;

            // 編集済みフラグを設定する
            trade.SetDirty(TreatyItemId.Oil);
            Scenarios.SetDirty();

            // 編集項目を更新する
            tradeOilTextBox1.Text = (trade.Oil < 0) ? DoubleHelper.ToString(-trade.Oil) : "";
            tradeOilTextBox2.Text = (trade.Oil > 0) ? DoubleHelper.ToString(trade.Oil) : "";

            // 文字色を変更する
            tradeOilTextBox1.ForeColor = Color.Red;
            tradeOilTextBox2.ForeColor = Color.Red;

            // 貿易リストビューの項目を更新する
            tradeListView.SelectedItems[0].SubItems[2].Text = GetTradeString(trade);
        }

        /// <summary>
        ///     石油貿易量テキストボックス2のフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeOilTextBox2Validated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty trade = GetSelectedTrade();
            if (trade == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(tradeOilTextBox2.Text, out val))
            {
                tradeOilTextBox2.Text = (trade.Oil > 0) ? DoubleHelper.ToString(trade.Oil) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, trade.Oil))
            {
                return;
            }

            Log.Info("[Scenario] trade oil: {0} -> {1} ({2})", DoubleHelper.ToString(trade.Oil), val,
                tradeListView.SelectedIndices[0]);

            // 値を更新する
            trade.Oil = val;

            // 編集済みフラグを設定する
            trade.SetDirty(TreatyItemId.Oil);
            Scenarios.SetDirty();

            // 編集項目を更新する
            tradeOilTextBox1.Text = (trade.Oil < 0) ? DoubleHelper.ToString(-trade.Oil) : "";
            tradeOilTextBox2.Text = (trade.Oil > 0) ? DoubleHelper.ToString(trade.Oil) : "";

            // 文字色を変更する
            tradeOilTextBox1.ForeColor = Color.Red;
            tradeOilTextBox2.ForeColor = Color.Red;

            // 貿易リストビューの項目を更新する
            tradeListView.SelectedItems[0].SubItems[2].Text = GetTradeString(trade);
        }

        /// <summary>
        ///     物資貿易量テキストボックス1のフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeSuppliesTextBox1Validated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty trade = GetSelectedTrade();
            if (trade == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(tradeSuppliesTextBox1.Text, out val))
            {
                tradeSuppliesTextBox1.Text = (trade.Supplies < 0) ? DoubleHelper.ToString(-trade.Supplies) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(-val, trade.Supplies))
            {
                return;
            }

            Log.Info("[Scenario] trade supplies: {0} -> {1} ({2})", DoubleHelper.ToString(trade.Supplies), -val,
                tradeListView.SelectedIndices[0]);

            // 値を更新する
            trade.Supplies = -val;

            // 編集済みフラグを設定する
            trade.SetDirty(TreatyItemId.Supplies);
            Scenarios.SetDirty();

            // 編集項目を更新する
            tradeSuppliesTextBox1.Text = (trade.Supplies < 0) ? DoubleHelper.ToString(-trade.Supplies) : "";
            tradeSuppliesTextBox2.Text = (trade.Supplies > 0) ? DoubleHelper.ToString(trade.Supplies) : "";

            // 文字色を変更する
            tradeSuppliesTextBox1.ForeColor = Color.Red;
            tradeSuppliesTextBox2.ForeColor = Color.Red;

            // 貿易リストビューの項目を更新する
            tradeListView.SelectedItems[0].SubItems[2].Text = GetTradeString(trade);
        }

        /// <summary>
        ///     物資貿易量テキストボックス2のフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeSuppliesTextBox2Validated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty trade = GetSelectedTrade();
            if (trade == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(tradeSuppliesTextBox2.Text, out val))
            {
                tradeSuppliesTextBox2.Text = (trade.Supplies > 0) ? DoubleHelper.ToString(trade.Supplies) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, trade.Supplies))
            {
                return;
            }

            Log.Info("[Scenario] trade supplies: {0} -> {1} ({2})", DoubleHelper.ToString(trade.Supplies), val,
                tradeListView.SelectedIndices[0]);

            // 値を更新する
            trade.Supplies = val;

            // 編集済みフラグを設定する
            trade.SetDirty(TreatyItemId.Supplies);
            Scenarios.SetDirty();

            // 編集項目を更新する
            tradeSuppliesTextBox1.Text = (trade.Supplies < 0) ? DoubleHelper.ToString(-trade.Supplies) : "";
            tradeSuppliesTextBox2.Text = (trade.Supplies > 0) ? DoubleHelper.ToString(trade.Supplies) : "";

            // 文字色を変更する
            tradeSuppliesTextBox1.ForeColor = Color.Red;
            tradeSuppliesTextBox2.ForeColor = Color.Red;

            // 貿易リストビューの項目を更新する
            tradeListView.SelectedItems[0].SubItems[2].Text = GetTradeString(trade);
        }

        /// <summary>
        ///     資金貿易量テキストボックス1のフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeMoneyTextBox1Validated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty trade = GetSelectedTrade();
            if (trade == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(tradeMoneyTextBox1.Text, out val))
            {
                tradeMoneyTextBox1.Text = (trade.Money < 0) ? DoubleHelper.ToString(-trade.Money) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(-val, trade.Money))
            {
                return;
            }

            Log.Info("[Scenario] trade money: {0} -> {1} ({2})", DoubleHelper.ToString(trade.Money), -val,
                tradeListView.SelectedIndices[0]);

            // 値を更新する
            trade.Money = -val;

            // 編集済みフラグを設定する
            trade.SetDirty(TreatyItemId.Money);
            Scenarios.SetDirty();

            // 編集項目を更新する
            tradeMoneyTextBox1.Text = (trade.Money < 0) ? DoubleHelper.ToString(-trade.Money) : "";
            tradeMoneyTextBox2.Text = (trade.Money > 0) ? DoubleHelper.ToString(trade.Money) : "";

            // 文字色を変更する
            tradeMoneyTextBox1.ForeColor = Color.Red;
            tradeMoneyTextBox2.ForeColor = Color.Red;

            // 貿易リストビューの項目を更新する
            tradeListView.SelectedItems[0].SubItems[2].Text = GetTradeString(trade);
        }

        /// <summary>
        ///     資金貿易量テキストボックス2のフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeMoneyTextBox2Validated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty trade = GetSelectedTrade();
            if (trade == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(tradeMoneyTextBox2.Text, out val))
            {
                tradeMoneyTextBox2.Text = (trade.Money > 0) ? DoubleHelper.ToString(trade.Money) : "";
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, trade.Money))
            {
                return;
            }

            Log.Info("[Scenario] trade money: {0} -> {1} ({2})", DoubleHelper.ToString(trade.Money), val,
                tradeListView.SelectedIndices[0]);

            // 値を更新する
            trade.Money = val;

            // 編集済みフラグを設定する
            trade.SetDirty(TreatyItemId.Money);
            Scenarios.SetDirty();

            // 編集項目を更新する
            tradeMoneyTextBox1.Text = (trade.Money < 0) ? DoubleHelper.ToString(-trade.Money) : "";
            tradeMoneyTextBox2.Text = (trade.Money > 0) ? DoubleHelper.ToString(trade.Money) : "";

            // 文字色を変更する
            tradeMoneyTextBox1.ForeColor = Color.Red;
            tradeMoneyTextBox2.ForeColor = Color.Red;

            // 貿易リストビューの項目を更新する
            tradeListView.SelectedItems[0].SubItems[2].Text = GetTradeString(trade);
        }

        #endregion

        #endregion

        #region 国家タブ

        #region 国家タブ- 共通

        /// <summary>
        ///     国家タブの編集項目を初期化する
        /// </summary>
        private void InitCountryTab()
        {
            Graphics g = Graphics.FromHwnd(Handle);
            int margin = DeviceCaps.GetScaledWidth(2) + 1;

            // 国家リストボックス
            countryListBox.BeginUpdate();
            countryListBox.Items.Clear();
            foreach (Country country in Countries.Tags)
            {
                countryListBox.Items.Add(Countries.GetTagName(country));
            }
            countryListBox.EndUpdate();

            // 兄弟国コンボボックス
            int width = regularIdComboBox.Width;
            regularIdComboBox.BeginUpdate();
            regularIdComboBox.Items.Clear();
            regularIdComboBox.Items.Add("");
            foreach (Country country in Countries.Tags)
            {
                string s = Countries.GetTagName(country);
                regularIdComboBox.Items.Add(s);
                width = Math.Max(width,
                    (int) g.MeasureString(s, regularIdComboBox.Font).Width +
                    SystemInformation.VerticalScrollBarWidth + margin);
            }
            regularIdComboBox.DropDownWidth = width;
            regularIdComboBox.EndUpdate();

            // 国家資源ラベル
            countryEnergyLabel.Text = Config.GetText(TextId.ResourceEnergy);
            countryMetalLabel.Text = Config.GetText(TextId.ResourceMetal);
            countryRareMaterialsLabel.Text = Config.GetText(TextId.ResourceRareMaterials);
            countryOilLabel.Text = Config.GetText(TextId.ResourceOil);
            countrySuppliesLabel.Text = Config.GetText(TextId.ResourceSupplies);
            countryMoneyLabel.Text = Config.GetText(TextId.ResourceMoney);
            countryTransportsLabel.Text = Config.GetText(TextId.ResourceTransports);
            countryEscortsLabel.Text = Config.GetText(TextId.ResourceEscorts);
            countryManpowerLabel.Text = Config.GetText(TextId.ResourceManpower);
            countryIcLabel.Text = Config.GetText(TextId.ResourceIc);
        }

        /// <summary>
        ///     国家タブの編集項目を更新する
        /// </summary>
        private void UpdateCountryTab()
        {
            // 編集項目を無効化する
            DisableCountryItems();

            // 国家リストボックスを有効化する
            countryListBox.Enabled = true;
        }

        /// <summary>
        ///     国家タブの編集項目を無効化する
        /// </summary>
        private void DisableCountryItems()
        {
            // 編集項目を無効化する
            countryInfoGroupBox.Enabled = false;
            countryModifierGroupBox.Enabled = false;
            countryResourceGroupBox.Enabled = false;
            aiGroupBox.Enabled = false;

            // 編集項目をクリアする
            countryNameTextBox.Text = "";
            regularIdComboBox.SelectedIndex = -1;
            flagExtTextBox.Text = "";
            belligerenceTextBox.Text = "";
            dissentTextBox.Text = "";
            extraTcTextBox.Text = "";
            nukeTextBox.Text = "";
            nukeYearTextBox.Text = "";
            nukeMonthTextBox.Text = "";
            nukeDayTextBox.Text = "";

            groundDefEffTextBox.Text = "";
            peacetimeIcModifierTextBox.Text = "";
            wartimeIcModifierTextBox.Text = "";
            industrialModifierTextBox.Text = "";
            relativeManpowerTextBox.Text = "";

            countryEnergyTextBox.Text = "";
            countryMetalTextBox.Text = "";
            countryRareMaterialsTextBox.Text = "";
            countryOilTextBox.Text = "";
            countrySuppliesTextBox.Text = "";
            countryMoneyTextBox.Text = "";
            countryTransportsTextBox.Text = "";
            countryEscortsTextBox.Text = "";
            countryManpowerTextBox.Text = "";
            offmapEnergyTextBox.Text = "";
            offmapMetalTextBox.Text = "";
            offmapRareMaterialsTextBox.Text = "";
            offmapOilTextBox.Text = "";
            offmapSuppliesTextBox.Text = "";
            offmapMoneyTextBox.Text = "";
            offmapTransportsTextBox.Text = "";
            offmapEscortsTextBox.Text = "";
            offmapManpowerTextBox.Text = "";
            offmapIcTextBox.Text = "";

            aiFileNameTextBox.Text = "";
        }

        /// <summary>
        ///     国家タブの編集項目を更新する
        /// </summary>
        private void UpdateCountryItems()
        {
            // 選択中の国家がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            countryNameTextBox.Text = GetCountryName(country);

            bool flag = (settings != null);
            regularIdComboBox.SelectedIndex = (flag && settings.RegularId != Country.None)
                ? Array.IndexOf(Countries.Tags, settings.RegularId) + 1
                : 0;
            flagExtTextBox.Text = flag ? settings.FlagExt : "";

            belligerenceTextBox.Text = flag ? IntHelper.ToString(settings.Belligerence) : "";
            dissentTextBox.Text = flag ? DoubleHelper.ToString(settings.Dissent) : "";
            extraTcTextBox.Text = flag ? DoubleHelper.ToString(settings.ExtraTc) : "";
            nukeTextBox.Text = flag ? IntHelper.ToString(settings.Nuke) : "";

            groundDefEffTextBox.Text = flag ? DoubleHelper.ToString(settings.GroundDefEff) : "";
            peacetimeIcModifierTextBox.Text = flag ? DoubleHelper.ToString(settings.PeacetimeIcModifier) : "";
            wartimeIcModifierTextBox.Text = flag ? DoubleHelper.ToString(settings.WartimeIcModifier) : "";
            industrialModifierTextBox.Text = flag ? DoubleHelper.ToString(settings.IndustrialModifier) : "";
            relativeManpowerTextBox.Text = flag ? DoubleHelper.ToString(settings.RelativeManpower) : "";

            countryEnergyTextBox.Text = flag ? DoubleHelper.ToString(settings.Energy) : "0";
            countryMetalTextBox.Text = flag ? DoubleHelper.ToString(settings.Metal) : "0";
            countryRareMaterialsTextBox.Text = flag ? DoubleHelper.ToString(settings.RareMaterials) : "0";
            countryOilTextBox.Text = flag ? DoubleHelper.ToString(settings.Oil) : "0";
            countrySuppliesTextBox.Text = flag ? DoubleHelper.ToString(settings.Supplies) : "0";
            countryMoneyTextBox.Text = flag ? DoubleHelper.ToString(settings.Money) : "0";
            countryTransportsTextBox.Text = flag ? DoubleHelper.ToString(settings.Transports) : "0";
            countryEscortsTextBox.Text = flag ? DoubleHelper.ToString(settings.Escorts) : "0";
            countryManpowerTextBox.Text = flag ? DoubleHelper.ToString(settings.Manpower) : "0";

            aiFileNameTextBox.Text = flag ? settings.AiFileName : "";

            flag = ((settings != null) && (settings.NukeDate != null));
            nukeYearTextBox.Text = flag ? IntHelper.ToString(settings.NukeDate.Year) : "";
            nukeMonthTextBox.Text = flag ? IntHelper.ToString(settings.NukeDate.Month) : "";
            nukeDayTextBox.Text = flag ? IntHelper.ToString(settings.NukeDate.Day) : "";

            flag = ((settings != null) && (settings.Offmap != null));
            offmapEnergyTextBox.Text = flag ? DoubleHelper.ToString(settings.Offmap.Energy) : "";
            offmapMetalTextBox.Text = flag ? DoubleHelper.ToString(settings.Offmap.Metal) : "";
            offmapRareMaterialsTextBox.Text = flag ? DoubleHelper.ToString(settings.Offmap.RareMaterials) : "";
            offmapOilTextBox.Text = flag ? DoubleHelper.ToString(settings.Offmap.Oil) : "";
            offmapSuppliesTextBox.Text = flag ? DoubleHelper.ToString(settings.Offmap.Supplies) : "";
            offmapMoneyTextBox.Text = flag ? DoubleHelper.ToString(settings.Offmap.Money) : "";
            offmapTransportsTextBox.Text = flag ? DoubleHelper.ToString(settings.Offmap.Transports) : "";
            offmapEscortsTextBox.Text = flag ? DoubleHelper.ToString(settings.Offmap.Escorts) : "";
            offmapManpowerTextBox.Text = flag ? DoubleHelper.ToString(settings.Offmap.Manpower) : "";
            offmapIcTextBox.Text = flag ? DoubleHelper.ToString(settings.Offmap.Ic) : "";

            // 編集項目を有効化する
            countryInfoGroupBox.Enabled = true;
            countryModifierGroupBox.Enabled = true;
            countryResourceGroupBox.Enabled = true;
            aiGroupBox.Enabled = true;
        }

        /// <summary>
        ///     国家リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ編集項目を無効化する
            if (countryListBox.SelectedIndex < 0)
            {
                DisableCountryItems();
                return;
            }

            // 編集項目を更新する
            UpdateCountryItems();
        }

        /// <summary>
        ///     選択中の国家を取得する
        /// </summary>
        /// <returns>選択中の国家</returns>
        private Country GetSelectedCountry()
        {
            return (countryListBox.SelectedIndex >= 0) ? Countries.Tags[countryListBox.SelectedIndex] : Country.None;
        }

        /// <summary>
        ///     国名を取得する
        /// </summary>
        /// <param name="country">国家</param>
        /// <returns>国名</returns>
        private static string GetCountryName(Country country)
        {
            CountrySettings settings = Scenarios.GetCountrySettings(country);
            return ((settings != null) && !string.IsNullOrEmpty(settings.Name))
                ? Config.GetText(settings.Name)
                : Countries.GetName(country);
        }

        #endregion

        #region 国家タブ - 国家情報

        /// <summary>
        ///     国名テキストボックスの文字列変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryNameTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 値に変化がなければ何もしない
            string name = GetCountryName(country);
            string val = countryNameTextBox.Text;
            if (val.Equals(name))
            {
                return;
            }

            Log.Info("[Scenario] country name: {0} -> {1} ({2})", name, val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            // 値を更新する
            Config.SetText(!string.IsNullOrEmpty(settings.Name) ? settings.Name : Countries.Strings[(int) country], val,
                Game.WorldTextFileName);

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.Name);
            Scenarios.SetDirty();

            // 国家リストボックスの項目を更新する
            countryListBox.SelectedItem = val;

            // 文字色を変更する
            countryNameTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     国旗接尾辞テキストボックスの文字列変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFlagExtTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 初期値から変更されていなければ何もしない
            if (((settings == null) || string.IsNullOrEmpty(settings.FlagExt)) &&
                string.IsNullOrEmpty(flagExtTextBox.Text))
            {
                return;
            }

            // 値に変化がなければ何もしない
            string val = flagExtTextBox.Text;
            if ((settings != null) && val.Equals(settings.FlagExt))
            {
                return;
            }

            Log.Info("[Scenario] flag ext: {0} -> {1} ({2})", (settings != null) ? settings.FlagExt : "", val,
                Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            // 値を更新する
            settings.FlagExt = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.FlagExt);
            Scenarios.SetDirty();

            // 文字色を変更する
            flagExtTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     兄弟国コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRegularIdComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            if (e.Index > 0)
            {
                Country country = Countries.Tags[countryListBox.SelectedIndex];
                CountrySettings settings = Scenarios.GetCountrySettings(country);
                Brush brush = ((settings != null) &&
                               (Countries.Tags[e.Index - 1] == settings.RegularId) &&
                               settings.IsDirty(CountrySettingsItemId.RegularId))
                    ? new SolidBrush(Color.Red)
                    : new SolidBrush(SystemColors.WindowText);
                string s = regularIdComboBox.Items[e.Index].ToString();
                e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     兄弟国コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRegularIdComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 初期値から変更されていなければ何もしない
            if ((settings == null) && (regularIdComboBox.SelectedIndex == 0))
            {
                return;
            }

            // 値に変化がなければ何もしない
            Country val = (regularIdComboBox.SelectedIndex > 0)
                ? Countries.Tags[regularIdComboBox.SelectedIndex - 1]
                : Country.None;
            if ((settings != null) && (val == settings.RegularId))
            {
                return;
            }

            Log.Info("[Scenario] regular id: {0} -> {1} ({2})",
                (settings != null) ? Countries.Strings[(int) settings.RegularId] : "", Countries.Strings[(int) val],
                Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            // 値を更新する
            settings.RegularId = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.RegularId);
            Scenarios.SetDirty();

            // 兄弟国コンボボックスの項目色を変更するために描画更新する
            regularIdComboBox.Refresh();
        }

        /// <summary>
        ///     好戦性テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBelligerenceTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(belligerenceTextBox.Text, out val))
            {
                belligerenceTextBox.Text = (settings != null) ? IntHelper.ToString(settings.Belligerence) : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if ((settings == null) && (val == 0))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (val == settings.Belligerence))
            {
                return;
            }

            Log.Info("[Scenario] belligerence: {0} -> {1} ({2})",
                (settings != null) ? IntHelper.ToString(settings.Belligerence) : "", val,
                Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            // 値を更新する
            settings.Belligerence = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.Belligerence);
            Scenarios.SetDirty();

            // 文字色を変更する
            belligerenceTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     国民不満度テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDissentTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(dissentTextBox.Text, out val))
            {
                dissentTextBox.Text = (settings != null) ? DoubleHelper.ToString(settings.Dissent) : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if ((settings == null) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && DoubleHelper.IsEqual(val, settings.Dissent))
            {
                return;
            }

            Log.Info("[Scenario] dissent: {0} -> {1} ({2})",
                (settings != null) ? DoubleHelper.ToString(settings.Dissent) : "", val,
                Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            // 値を更新する
            settings.Dissent = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.Dissent);
            Scenarios.SetDirty();

            // 文字色を変更する
            dissentTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     輸送力補正テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExtraTcTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(extraTcTextBox.Text, out val))
            {
                extraTcTextBox.Text = (settings != null) ? DoubleHelper.ToString(settings.ExtraTc) : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if ((settings == null) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && DoubleHelper.IsEqual(val, settings.ExtraTc))
            {
                return;
            }

            Log.Info("[Scenario] extra tc: {0} -> {1} ({2})",
                (settings != null) ? DoubleHelper.ToString(settings.ExtraTc) : "", val,
                Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            // 値を更新する
            settings.ExtraTc = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.ExtraTc);
            Scenarios.SetDirty();

            // 文字色を変更する
            extraTcTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     核兵器テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNukeTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(nukeTextBox.Text, out val))
            {
                nukeTextBox.Text = (settings != null) ? IntHelper.ToString(settings.Nuke) : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if ((settings == null) && (val == 0))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (val == settings.Nuke))
            {
                return;
            }

            Log.Info("[Scenario] nuke: {0} -> {1} ({2})",
                (settings != null) ? IntHelper.ToString(settings.Nuke) : "", val,
                Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            // 値を更新する
            settings.Nuke = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.Nuke);
            Scenarios.SetDirty();

            // 文字色を変更する
            nukeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     核兵器生産年テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNukeYearTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(nukeYearTextBox.Text, out val))
            {
                nukeYearTextBox.Text = ((settings != null) && (settings.NukeDate != null))
                    ? IntHelper.ToString(settings.NukeDate.Year)
                    : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if (((settings == null) || (settings.NukeDate == null)) && (val == 0))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.NukeDate != null) && (val == settings.NukeDate.Year))
            {
                return;
            }

            Log.Info("[Scenario] nuke year: {0} -> {1} ({2})",
                ((settings != null) && (settings.NukeDate != null)) ? IntHelper.ToString(settings.NukeDate.Year) : "",
                val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }
            if (settings.NukeDate == null)
            {
                settings.NukeDate = new GameDate();

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.NukeMonth);
                settings.SetDirty(CountrySettingsItemId.NukeDay);

                // 編集項目を更新する
                nukeMonthTextBox.Text = IntHelper.ToString(settings.NukeDate.Month);
                nukeDayTextBox.Text = IntHelper.ToString(settings.NukeDate.Day);

                // 文字色を変更する
                nukeMonthTextBox.ForeColor = Color.Red;
                nukeDayTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            settings.NukeDate.Year = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.NukeYear);
            Scenarios.SetDirty();

            // 文字色を変更する
            nukeYearTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     核兵器生産月テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNukeMonthTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(nukeMonthTextBox.Text, out val))
            {
                nukeMonthTextBox.Text = ((settings != null) && (settings.NukeDate != null))
                    ? IntHelper.ToString(settings.NukeDate.Month)
                    : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if (((settings == null) || (settings.NukeDate == null)) && (val == 0))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.NukeDate != null) && (val == settings.NukeDate.Month))
            {
                return;
            }

            Log.Info("[Scenario] nuke month: {0} -> {1} ({2})",
                ((settings != null) && (settings.NukeDate != null)) ? IntHelper.ToString(settings.NukeDate.Month) : "",
                val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }
            if (settings.NukeDate == null)
            {
                settings.NukeDate = new GameDate();

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.NukeYear);
                settings.SetDirty(CountrySettingsItemId.NukeDay);

                // 編集項目を更新する
                nukeYearTextBox.Text = IntHelper.ToString(settings.NukeDate.Year);
                nukeDayTextBox.Text = IntHelper.ToString(settings.NukeDate.Day);

                // 文字色を変更する
                nukeYearTextBox.ForeColor = Color.Red;
                nukeDayTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            settings.NukeDate.Month = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.NukeMonth);
            Scenarios.SetDirty();

            // 文字色を変更する
            nukeMonthTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     核兵器生産日テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNukeDayTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(nukeDayTextBox.Text, out val))
            {
                nukeDayTextBox.Text = ((settings != null) && (settings.NukeDate != null))
                    ? IntHelper.ToString(settings.NukeDate.Day)
                    : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if (((settings == null) || (settings.NukeDate == null)) && (val == 0))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.NukeDate != null) && (val == settings.NukeDate.Day))
            {
                return;
            }

            Log.Info("[Scenario] nuke day: {0} -> {1} ({2})",
                ((settings != null) && (settings.NukeDate != null)) ? IntHelper.ToString(settings.NukeDate.Day) : "",
                val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }
            if (settings.NukeDate == null)
            {
                settings.NukeDate = new GameDate();

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.NukeYear);
                settings.SetDirty(CountrySettingsItemId.NukeMonth);

                // 編集項目を更新する
                nukeYearTextBox.Text = IntHelper.ToString(settings.NukeDate.Year);
                nukeMonthTextBox.Text = IntHelper.ToString(settings.NukeDate.Month);

                // 文字色を変更する
                nukeYearTextBox.ForeColor = Color.Red;
                nukeMonthTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            settings.NukeDate.Day = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.NukeDay);
            Scenarios.SetDirty();

            // 文字色を変更する
            nukeDayTextBox.ForeColor = Color.Red;
        }

        #endregion

        #region 国家タブ - 補正値

        /// <summary>
        ///     対地防御補正テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGroundDefEffTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(groundDefEffTextBox.Text, out val))
            {
                groundDefEffTextBox.Text = (settings != null) ? DoubleHelper.ToString(settings.GroundDefEff) : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if ((settings == null) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && DoubleHelper.IsEqual(val, settings.GroundDefEff))
            {
                return;
            }

            Log.Info("[Scenario] ground def eff: {0} -> {1} ({2})",
                (settings != null) ? DoubleHelper.ToString(settings.GroundDefEff) : "", val,
                Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            // 値を更新する
            settings.GroundDefEff = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.GroundDefEff);
            Scenarios.SetDirty();

            // 文字色を変更する
            groundDefEffTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     平時IC補正テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPeacetimeIcModifierTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(peacetimeIcModifierTextBox.Text, out val))
            {
                peacetimeIcModifierTextBox.Text = (settings != null)
                    ? DoubleHelper.ToString(settings.PeacetimeIcModifier)
                    : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if ((settings == null) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && DoubleHelper.IsEqual(val, settings.PeacetimeIcModifier))
            {
                return;
            }

            Log.Info("[Scenario] peacetime ic modifier: {0} -> {1} ({2})",
                (settings != null) ? DoubleHelper.ToString(settings.PeacetimeIcModifier) : "", val,
                Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            // 値を更新する
            settings.PeacetimeIcModifier = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.PeacetimeIcModifier);
            Scenarios.SetDirty();

            // 文字色を変更する
            peacetimeIcModifierTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     戦時IC補正テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWartimeIcModifierTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(wartimeIcModifierTextBox.Text, out val))
            {
                wartimeIcModifierTextBox.Text = (settings != null)
                    ? DoubleHelper.ToString(settings.WartimeIcModifier)
                    : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if ((settings == null) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && DoubleHelper.IsEqual(val, settings.WartimeIcModifier))
            {
                return;
            }

            Log.Info("[Scenario] wartime ic modifier: {0} -> {1} ({2})",
                (settings != null) ? DoubleHelper.ToString(settings.WartimeIcModifier) : "", val,
                Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            // 値を更新する
            settings.WartimeIcModifier = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.WartimeIcModifier);
            Scenarios.SetDirty();

            // 文字色を変更する
            wartimeIcModifierTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     工業力補正テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIndustrialModifierTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(industrialModifierTextBox.Text, out val))
            {
                industrialModifierTextBox.Text = (settings != null)
                    ? DoubleHelper.ToString(settings.IndustrialModifier)
                    : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if ((settings == null) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && DoubleHelper.IsEqual(val, settings.IndustrialModifier))
            {
                return;
            }

            Log.Info("[Scenario] industrial modifier: {0} -> {1} ({2})",
                (settings != null) ? DoubleHelper.ToString(settings.IndustrialModifier) : "", val,
                Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            // 値を更新する
            settings.IndustrialModifier = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.IndustrialModifier);
            Scenarios.SetDirty();

            // 文字色を変更する
            industrialModifierTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     人的資源補正テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRelativeManpowerTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(relativeManpowerTextBox.Text, out val))
            {
                relativeManpowerTextBox.Text = (settings != null)
                    ? DoubleHelper.ToString(settings.RelativeManpower)
                    : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if ((settings == null) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && DoubleHelper.IsEqual(val, settings.RelativeManpower))
            {
                return;
            }

            Log.Info("[Scenario] relative manpower: {0} -> {1} ({2})",
                (settings != null) ? DoubleHelper.ToString(settings.RelativeManpower) : "", val,
                Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            // 値を更新する
            settings.RelativeManpower = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.RelativeManpower);
            Scenarios.SetDirty();

            // 文字色を変更する
            relativeManpowerTextBox.ForeColor = Color.Red;
        }

        #endregion

        #region 国家タブ - 資源

        /// <summary>
        ///     エネルギー備蓄量テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryEnergyTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(countryEnergyTextBox.Text, out val))
            {
                countryEnergyTextBox.Text = (settings != null) ? DoubleHelper.ToString(settings.Energy) : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if ((settings == null) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && DoubleHelper.IsEqual(val, settings.Energy))
            {
                return;
            }

            Log.Info("[Scenario] energy: {0} -> {1} ({2})",
                (settings != null) ? DoubleHelper.ToString(settings.Energy) : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            // 値を更新する
            settings.Energy = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.Energy);
            Scenarios.SetDirty();

            // 文字色を変更する
            countryEnergyTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     金属備蓄量テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryMetalTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(countryMetalTextBox.Text, out val))
            {
                countryMetalTextBox.Text = (settings != null) ? DoubleHelper.ToString(settings.Metal) : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if ((settings == null) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && DoubleHelper.IsEqual(val, settings.Metal))
            {
                return;
            }

            Log.Info("[Scenario] metal: {0} -> {1} ({2})",
                (settings != null) ? DoubleHelper.ToString(settings.Metal) : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            // 値を更新する
            settings.Metal = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.Metal);
            Scenarios.SetDirty();

            // 文字色を変更する
            countryMetalTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     希少資源備蓄量テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryRareMaterialsTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(countryRareMaterialsTextBox.Text, out val))
            {
                countryRareMaterialsTextBox.Text = (settings != null)
                    ? DoubleHelper.ToString(settings.RareMaterials)
                    : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if ((settings == null) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && DoubleHelper.IsEqual(val, settings.RareMaterials))
            {
                return;
            }

            Log.Info("[Scenario] rare materials: {0} -> {1} ({2})",
                (settings != null) ? DoubleHelper.ToString(settings.RareMaterials) : "", val,
                Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            // 値を更新する
            settings.RareMaterials = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.RareMaterials);
            Scenarios.SetDirty();

            // 文字色を変更する
            countryRareMaterialsTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     石油備蓄量テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryOilTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(countryOilTextBox.Text, out val))
            {
                countryOilTextBox.Text = (settings != null) ? DoubleHelper.ToString(settings.Oil) : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if ((settings == null) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && DoubleHelper.IsEqual(val, settings.Oil))
            {
                return;
            }

            Log.Info("[Scenario] oil: {0} -> {1} ({2})",
                (settings != null) ? DoubleHelper.ToString(settings.Oil) : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            // 値を更新する
            settings.Oil = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.Oil);
            Scenarios.SetDirty();

            // 文字色を変更する
            countryOilTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     物資備蓄量テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountrySuppliesTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(countrySuppliesTextBox.Text, out val))
            {
                countrySuppliesTextBox.Text = (settings != null) ? DoubleHelper.ToString(settings.Supplies) : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if ((settings == null) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && DoubleHelper.IsEqual(val, settings.Supplies))
            {
                return;
            }

            Log.Info("[Scenario] supplies: {0} -> {1} ({2})",
                (settings != null) ? DoubleHelper.ToString(settings.Supplies) : "", val,
                Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            // 値を更新する
            settings.Supplies = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.Supplies);
            Scenarios.SetDirty();

            // 文字色を変更する
            countrySuppliesTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     資金備蓄量テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryMoneyTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(countryMoneyTextBox.Text, out val))
            {
                countryMoneyTextBox.Text = (settings != null) ? DoubleHelper.ToString(settings.Money) : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if ((settings == null) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && DoubleHelper.IsEqual(val, settings.Money))
            {
                return;
            }

            Log.Info("[Scenario] money: {0} -> {1} ({2})",
                (settings != null) ? DoubleHelper.ToString(settings.Money) : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            // 値を更新する
            settings.Money = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.Money);
            Scenarios.SetDirty();

            // 文字色を変更する
            countryMoneyTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     輸送船団保有数テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryTransportsTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(countryTransportsTextBox.Text, out val))
            {
                countryTransportsTextBox.Text = (settings != null) ? IntHelper.ToString(settings.Transports) : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if ((settings == null) && (val == 0))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (val == settings.Transports))
            {
                return;
            }

            Log.Info("[Scenario] transports: {0} -> {1} ({2})",
                (settings != null) ? IntHelper.ToString(settings.Transports) : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            // 値を更新する
            settings.Transports = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.Transports);
            Scenarios.SetDirty();

            // 文字色を変更する
            countryTransportsTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     護衛船保有数テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryEscortsTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(countryEscortsTextBox.Text, out val))
            {
                countryEscortsTextBox.Text = (settings != null) ? IntHelper.ToString(settings.Escorts) : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if ((settings == null) && (val == 0))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (val == settings.Escorts))
            {
                return;
            }

            Log.Info("[Scenario] transports: {0} -> {1} ({2})",
                (settings != null) ? IntHelper.ToString(settings.Escorts) : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            // 値を更新する
            settings.Escorts = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.Escorts);
            Scenarios.SetDirty();

            // 文字色を変更する
            countryEscortsTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     人的資源テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryManpowerTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(countryManpowerTextBox.Text, out val))
            {
                countryManpowerTextBox.Text = (settings != null) ? DoubleHelper.ToString(settings.Manpower) : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if ((settings == null) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && DoubleHelper.IsEqual(val, settings.Manpower))
            {
                return;
            }

            Log.Info("[Scenario] money: {0} -> {1} ({2})",
                (settings != null) ? DoubleHelper.ToString(settings.Manpower) : "", val,
                Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            // 値を更新する
            settings.Manpower = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.Manpower);
            Scenarios.SetDirty();

            // 文字色を変更する
            countryManpowerTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     マップ外エネルギーテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOffmapEnergyTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(offmapEnergyTextBox.Text, out val))
            {
                offmapEnergyTextBox.Text = ((settings != null) && (settings.Offmap != null))
                    ? DoubleHelper.ToString(settings.Offmap.Energy)
                    : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if (((settings == null) || (settings.Offmap == null)) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (((settings != null) && (settings.Offmap != null)) && DoubleHelper.IsEqual(val, settings.Offmap.Energy))
            {
                return;
            }

            Log.Info("[Scenario] offmap energy: {0} -> {1} ({2})",
                ((settings != null) && (settings.Offmap != null)) ? DoubleHelper.ToString(settings.Offmap.Energy) : "",
                val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }
            if (settings.Offmap == null)
            {
                settings.Offmap = new ResourceSettings();
            }

            // 値を更新する
            settings.Offmap.Energy = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.OffmapEnergy);
            Scenarios.SetDirty();

            // 文字色を変更する
            offmapEnergyTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     マップ外金属テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOffmapMetalTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(offmapMetalTextBox.Text, out val))
            {
                offmapMetalTextBox.Text = ((settings != null) && (settings.Offmap != null))
                    ? DoubleHelper.ToString(settings.Offmap.Metal)
                    : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if (((settings == null) || (settings.Offmap == null)) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (((settings != null) && (settings.Offmap != null)) && DoubleHelper.IsEqual(val, settings.Offmap.Metal))
            {
                return;
            }

            Log.Info("[Scenario] offmap metal: {0} -> {1} ({2})",
                ((settings != null) && (settings.Offmap != null)) ? DoubleHelper.ToString(settings.Offmap.Metal) : "",
                val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }
            if (settings.Offmap == null)
            {
                settings.Offmap = new ResourceSettings();
            }

            // 値を更新する
            settings.Offmap.Metal = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.OffmapMetal);
            Scenarios.SetDirty();

            // 文字色を変更する
            offmapMetalTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     マップ外希少資源テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOffmapRareMaterialsTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(offmapRareMaterialsTextBox.Text, out val))
            {
                offmapRareMaterialsTextBox.Text = ((settings != null) && (settings.Offmap != null))
                    ? DoubleHelper.ToString(settings.Offmap.RareMaterials)
                    : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if (((settings == null) || (settings.Offmap == null)) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (((settings != null) && (settings.Offmap != null)) &&
                DoubleHelper.IsEqual(val, settings.Offmap.RareMaterials))
            {
                return;
            }

            Log.Info("[Scenario] offmap rare materials: {0} -> {1} ({2})",
                ((settings != null) && (settings.Offmap != null))
                    ? DoubleHelper.ToString(settings.Offmap.RareMaterials)
                    : "",
                val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }
            if (settings.Offmap == null)
            {
                settings.Offmap = new ResourceSettings();
            }

            // 値を更新する
            settings.Offmap.RareMaterials = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.OffmapRareMaterials);
            Scenarios.SetDirty();

            // 文字色を変更する
            offmapRareMaterialsTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     マップ外石油テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOffmapOilTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(offmapOilTextBox.Text, out val))
            {
                offmapOilTextBox.Text = ((settings != null) && (settings.Offmap != null))
                    ? DoubleHelper.ToString(settings.Offmap.Oil)
                    : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if (((settings == null) || (settings.Offmap == null)) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (((settings != null) && (settings.Offmap != null)) && DoubleHelper.IsEqual(val, settings.Offmap.Oil))
            {
                return;
            }

            Log.Info("[Scenario] offmap oil: {0} -> {1} ({2})",
                ((settings != null) && (settings.Offmap != null)) ? DoubleHelper.ToString(settings.Offmap.Oil) : "",
                val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }
            if (settings.Offmap == null)
            {
                settings.Offmap = new ResourceSettings();
            }

            // 値を更新する
            settings.Offmap.Oil = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.OffmapOil);
            Scenarios.SetDirty();

            // 文字色を変更する
            offmapOilTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     マップ外物資テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOffmapSuppliesTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(offmapSuppliesTextBox.Text, out val))
            {
                offmapSuppliesTextBox.Text = ((settings != null) && (settings.Offmap != null))
                    ? DoubleHelper.ToString(settings.Offmap.Supplies)
                    : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if (((settings == null) || (settings.Offmap == null)) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (((settings != null) && (settings.Offmap != null)) && DoubleHelper.IsEqual(val, settings.Offmap.Supplies))
            {
                return;
            }

            Log.Info("[Scenario] offmap supplies: {0} -> {1} ({2})",
                ((settings != null) && (settings.Offmap != null)) ? DoubleHelper.ToString(settings.Offmap.Supplies) : "",
                val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }
            if (settings.Offmap == null)
            {
                settings.Offmap = new ResourceSettings();
            }

            // 値を更新する
            settings.Offmap.Supplies = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.OffmapSupplies);
            Scenarios.SetDirty();

            // 文字色を変更する
            offmapSuppliesTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     マップ外資金テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOffmapMoneyTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(offmapMoneyTextBox.Text, out val))
            {
                offmapMoneyTextBox.Text = ((settings != null) && (settings.Offmap != null))
                    ? DoubleHelper.ToString(settings.Offmap.Money)
                    : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if (((settings == null) || (settings.Offmap == null)) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (((settings != null) && (settings.Offmap != null)) && DoubleHelper.IsEqual(val, settings.Offmap.Money))
            {
                return;
            }

            Log.Info("[Scenario] offmap money: {0} -> {1} ({2})",
                ((settings != null) && (settings.Offmap != null)) ? DoubleHelper.ToString(settings.Offmap.Money) : "",
                val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }
            if (settings.Offmap == null)
            {
                settings.Offmap = new ResourceSettings();
            }

            // 値を更新する
            settings.Offmap.Money = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.OffmapMoney);
            Scenarios.SetDirty();

            // 文字色を変更する
            offmapMoneyTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     マップ外輸送船団テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOffmapTransportsTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(offmapTransportsTextBox.Text, out val))
            {
                offmapTransportsTextBox.Text = ((settings != null) && (settings.Offmap != null))
                    ? IntHelper.ToString(settings.Offmap.Transports)
                    : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if (((settings == null) || (settings.Offmap == null)) && (val == 0))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (((settings != null) && (settings.Offmap != null)) && (val == settings.Offmap.Transports))
            {
                return;
            }

            Log.Info("[Scenario] offmap transports: {0} -> {1} ({2})",
                ((settings != null) && (settings.Offmap != null)) ? IntHelper.ToString(settings.Offmap.Transports) : "",
                val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }
            if (settings.Offmap == null)
            {
                settings.Offmap = new ResourceSettings();
            }

            // 値を更新する
            settings.Offmap.Transports = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.OffmapTransports);
            Scenarios.SetDirty();

            // 文字色を変更する
            offmapTransportsTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     マップ外護衛艦テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOffmapEscortsTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(offmapEscortsTextBox.Text, out val))
            {
                offmapEscortsTextBox.Text = ((settings != null) && (settings.Offmap != null))
                    ? IntHelper.ToString(settings.Offmap.Escorts)
                    : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if (((settings == null) || (settings.Offmap == null)) && (val == 0))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (((settings != null) && (settings.Offmap != null)) && (val == settings.Offmap.Escorts))
            {
                return;
            }

            Log.Info("[Scenario] offmap escorts: {0} -> {1} ({2})",
                ((settings != null) && (settings.Offmap != null)) ? IntHelper.ToString(settings.Offmap.Escorts) : "",
                val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }
            if (settings.Offmap == null)
            {
                settings.Offmap = new ResourceSettings();
            }

            // 値を更新する
            settings.Offmap.Escorts = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.OffmapEscorts);
            Scenarios.SetDirty();

            // 文字色を変更する
            offmapEscortsTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     マップ外人的資源テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOffmapManpowerTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(offmapManpowerTextBox.Text, out val))
            {
                offmapManpowerTextBox.Text = ((settings != null) && (settings.Offmap != null))
                    ? DoubleHelper.ToString(settings.Offmap.Manpower)
                    : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if (((settings == null) || (settings.Offmap == null)) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (((settings != null) && (settings.Offmap != null)) && DoubleHelper.IsEqual(val, settings.Offmap.Manpower))
            {
                return;
            }

            Log.Info("[Scenario] offmap manpower: {0} -> {1} ({2})",
                ((settings != null) && (settings.Offmap != null)) ? DoubleHelper.ToString(settings.Offmap.Manpower) : "",
                val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }
            if (settings.Offmap == null)
            {
                settings.Offmap = new ResourceSettings();
            }

            // 値を更新する
            settings.Offmap.Manpower = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.OffmapManpower);
            Scenarios.SetDirty();

            // 文字色を変更する
            offmapManpowerTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     マップ外ICテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOffmapIcTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(offmapIcTextBox.Text, out val))
            {
                offmapIcTextBox.Text = ((settings != null) && (settings.Offmap != null))
                    ? DoubleHelper.ToString(settings.Offmap.Ic)
                    : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if (((settings == null) || (settings.Offmap == null)) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (((settings != null) && (settings.Offmap != null)) && DoubleHelper.IsEqual(val, settings.Offmap.Ic))
            {
                return;
            }

            Log.Info("[Scenario] offmap ic: {0} -> {1} ({2})",
                ((settings != null) && (settings.Offmap != null)) ? DoubleHelper.ToString(settings.Offmap.Ic) : "",
                val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }
            if (settings.Offmap == null)
            {
                settings.Offmap = new ResourceSettings();
            }

            // 値を更新する
            settings.Offmap.Ic = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.OffmapIc);
            Scenarios.SetDirty();

            // 文字色を変更する
            offmapIcTextBox.ForeColor = Color.Red;
        }

        #endregion

        #region 国家タブ - AI

        /// <summary>
        ///     AIファイル名テキストボックスの文字列変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAiFileNameTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 初期値から変更されていなければ何もしない
            if (((settings == null) || string.IsNullOrEmpty(settings.AiFileName)) &&
                string.IsNullOrEmpty(aiFileNameTextBox.Text))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && aiFileNameTextBox.Text.Equals(settings.AiFileName))
            {
                return;
            }

            Log.Info("[Scenario] ai file name: {0} -> {1} ({2})", (settings != null) ? settings.AiFileName : "",
                aiFileNameTextBox.Text, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            // 値を更新する
            settings.AiFileName = aiFileNameTextBox.Text;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.AiFileName);
            Scenarios.SetDirty();

            // 文字色を変更する
            aiFileNameTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     AIファイル名参照ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAiFileNameBrowseButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            OpenFileDialog dialog = new OpenFileDialog
            {
                InitialDirectory = Game.GetReadFileName(Game.AiPathName),
                FileName = settings.AiFileName,
                Filter = Resources.OpenAiFileDialogFilter
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string fileName;
                if (Game.IsExportFolderActive)
                {
                    fileName = PathHelper.GetRelativePathName(dialog.FileName, Game.ExportFolderName);
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        aiFileNameTextBox.Text = fileName;
                        return;
                    }
                }
                if (Game.IsModActive)
                {
                    fileName = PathHelper.GetRelativePathName(dialog.FileName, Game.ModFolderName);
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        aiFileNameTextBox.Text = fileName;
                        return;
                    }
                }
                fileName = PathHelper.GetRelativePathName(dialog.FileName, Game.FolderName);
                if (!string.IsNullOrEmpty(fileName))
                {
                    aiFileNameTextBox.Text = fileName;
                    return;
                }
                aiFileNameTextBox.Text = dialog.FileName;
            }
        }

        #endregion

        #endregion

        #region 政府タブ

        #region 政府タブ - 共通

        /// <summary>
        ///     政府タブを初期化する
        /// </summary>
        private void InitGovernmentTab()
        {
            // 国家リストボックス
            governmentCountryListBox.BeginUpdate();
            governmentCountryListBox.Items.Clear();
            foreach (Country country in Countries.Tags)
            {
                governmentCountryListBox.Items.Add(Countries.GetTagName(country));
            }
            governmentCountryListBox.EndUpdate();

            // 政策スライダーラベル
            democraticLabel.Text = Config.GetText(TextId.SliderDemocratic);
            authoritarianLabel.Text = Config.GetText(TextId.SliderAuthoritarian);
            politicalLeftLabel.Text = Config.GetText(TextId.SliderPoliticalLeft);
            politicalRightLabel.Text = Config.GetText(TextId.SliderPoliticalRight);
            openSocietyLabel.Text = Config.GetText(TextId.SliderOpenSociety);
            closedSocietyLabel.Text = Config.GetText(TextId.SliderClosedSociety);
            freeMarketLabel.Text = Config.GetText(TextId.SliderFreeMarket);
            centralPlanningLabel.Text = Config.GetText(TextId.SliderCentralPlanning);
            standingArmyLabel.Text = Config.GetText(TextId.SliderStandingArmy);
            draftedArmyLabel.Text = Config.GetText(TextId.SliderDraftedArmy);
            hawkLobbyLabel.Text = Config.GetText(TextId.SliderHawkLobby);
            doveLobbyLabel.Text = Config.GetText(TextId.SliderDoveLobby);
            interventionismLabel.Text = Config.GetText(TextId.SliderInterventionism);
            isolationismLabel.Text = Config.GetText(TextId.SlidlaIsolationism);

            authoritarianLabel.Left = democraticTrackBar.Left + democraticTrackBar.Width - authoritarianLabel.Width;
            politicalRightLabel.Left = politicalLeftTrackBar.Left + politicalLeftTrackBar.Width -
                                       politicalRightLabel.Width;
            closedSocietyLabel.Left = freedomTrackBar.Left + freedomTrackBar.Width - closedSocietyLabel.Width;
            centralPlanningLabel.Left = freeMarketTrackBar.Left + freeMarketTrackBar.Width - centralPlanningLabel.Width;
            draftedArmyLabel.Left = professionalArmyTrackBar.Left + professionalArmyTrackBar.Width -
                                    draftedArmyLabel.Width;
            doveLobbyLabel.Left = defenseLobbyTrackBar.Left + defenseLobbyTrackBar.Width - doveLobbyLabel.Width;
            isolationismLabel.Left = interventionismTrackBar.Left + interventionismTrackBar.Width -
                                     isolationismLabel.Width;

            // 閣僚地位ラベル
            headOfStateLabel.Text = Config.GetText(TextId.MinisterHeadOfState);
            headOfGovernmentLabel.Text = Config.GetText(TextId.MinisterHeadOfGovernment);
            foreignMinisterlabel.Text = Config.GetText(TextId.MinisterForeignMinister);
            armamentMinisterLabel.Text = Config.GetText(TextId.MinisterArmamentMinister);
            ministerOfSecurityLabel.Text = Config.GetText(TextId.MinisterMinisterOfSecurity);
            ministerOfIntelligenceLabel.Text = Config.GetText(TextId.MinisterMinisterOfIntelligence);
            chiefOfStaffLabel.Text = Config.GetText(TextId.MinisterChiefOfStaff);
            chiefOfArmyLabel.Text = Config.GetText(TextId.MinisterChiefOfArmy);
            chiefOfNavyLabel.Text = Config.GetText(TextId.MinisterChiefOfNavy);
            chiefOfAirLabel.Text = Config.GetText(TextId.MinisterChiefOfAir);
        }

        /// <summary>
        ///     政府タブを更新する
        /// </summary>
        private void UpdateGovernmentTab()
        {
            // 編集項目を無効化する
            DisableGovernmentItems();

            // 国家リストボックスを有効化する
            governmentCountryListBox.Enabled = true;
        }

        /// <summary>
        ///     政府タブの編集項目を無効化する
        /// </summary>
        private void DisableGovernmentItems()
        {
            // 編集項目を無効化する
            politicalSliderGroupBox.Enabled = false;
            cabinetGroupBox.Enabled = false;

            // 編集項目をクリアする
            sliderYearTextBox.Text = "";
            sliderMonthTextBox.Text = "";
            sliderDayTextBox.Text = "";

            democraticTrackBar.Value = 6;
            politicalLeftTrackBar.Value = 6;
            freedomTrackBar.Value = 6;
            freeMarketTrackBar.Value = 6;
            professionalArmyTrackBar.Value = 6;
            defenseLobbyTrackBar.Value = 6;
            interventionismTrackBar.Value = 6;

            headOfStateComboBox.SelectedIndex = -1;
            headOfGovernmentComboBox.SelectedIndex = -1;
            foreignMinisterComboBox.SelectedIndex = -1;
            armamentMinisterComboBox.SelectedIndex = -1;
            ministerOfSecurityComboBox.SelectedIndex = -1;
            ministerOfIntelligenceComboBox.SelectedIndex = -1;
            chiefOfStaffComboBox.SelectedIndex = -1;
            chiefOfArmyComboBox.SelectedIndex = -1;
            chiefOfNavyComboBox.SelectedIndex = -1;
            chiefOfAirComboBox.SelectedIndex = -1;

            headOfStateTypeTextBox.Text = "";
            headOfStateIdTextBox.Text = "";
            headOfGovernmentTypeTextBox.Text = "";
            headOfGovernmentIdTextBox.Text = "";
            foreignMinisterTypeTextBox.Text = "";
            foreignMinisterIdTextBox.Text = "";
            armamentMinisterTypeTextBox.Text = "";
            armamentMinisterIdTextBox.Text = "";
            ministerOfSecurityTypeTextBox.Text = "";
            ministerOfSecurityIdTextBox.Text = "";
            ministerOfIntelligenceTypeTextBox.Text = "";
            ministerOfIntelligenceIdTextBox.Text = "";
            chiefOfStaffTypeTextBox.Text = "";
            chiefOfStaffIdTextBox.Text = "";
            chiefOfArmyTypeTextBox.Text = "";
            chiefOfArmyIdTextBox.Text = "";
            chiefOfNavyTypeTextBox.Text = "";
            chiefOfNavyIdTextBox.Text = "";
            chiefOfAirTypeTextBox.Text = "";
            chiefOfAirIdTextBox.Text = "";
        }

        /// <summary>
        ///     政府タブの編集項目を更新する
        /// </summary>
        private void UpdateGovernmentItems()
        {
            // 選択中の国家がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 編集項目を更新する
            UpdatePoliticalSliderItems(settings);
            UpdateCabinetItems(settings);

            // 編集項目を有効化する
            politicalSliderGroupBox.Enabled = true;
            cabinetGroupBox.Enabled = true;
        }

        /// <summary>
        ///     政府タブ選択時の処理
        /// </summary>
        private void OnGovernmentTabSelected()
        {
            // 閣僚データの読み込み完了まで待機する
            WaitLoadingMinisters();
        }

        /// <summary>
        ///     国家リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGovernmentCountryListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ編集項目を無効化する
            if (governmentCountryListBox.SelectedIndex < 0)
            {
                DisableGovernmentItems();
                return;
            }

            // 編集項目を更新する
            UpdateGovernmentItems();
        }

        /// <summary>
        ///     選択中の国家を取得する
        /// </summary>
        /// <returns>選択中の国家</returns>
        private Country GetSelectedGovernmentCountry()
        {
            if (governmentCountryListBox.SelectedIndex < 0)
            {
                return Country.None;
            }
            return Countries.Tags[governmentCountryListBox.SelectedIndex];
        }

        #endregion

        #region 政府タブ - 政策スライダー

        /// <summary>
        ///     政策スライダーの編集項目を更新する
        /// </summary>
        /// <param name="settings">国家設定</param>
        private void UpdatePoliticalSliderItems(CountrySettings settings)
        {
            bool flag = ((settings != null) && (settings.Policy != null) && (settings.Policy.Date != null));
            sliderYearTextBox.Text = flag ? IntHelper.ToString(settings.Policy.Date.Year) : "";
            sliderMonthTextBox.Text = flag ? IntHelper.ToString(settings.Policy.Date.Month) : "";
            sliderDayTextBox.Text = flag ? IntHelper.ToString(settings.Policy.Date.Day) : "";

            sliderYearTextBox.ForeColor = (flag && settings.IsDirty(CountrySettingsItemId.SliderYear))
                ? Color.Red
                : SystemColors.WindowText;
            sliderMonthTextBox.ForeColor = (flag && settings.IsDirty(CountrySettingsItemId.SliderMonth))
                ? Color.Red
                : SystemColors.WindowText;
            sliderDayTextBox.ForeColor = (flag && settings.IsDirty(CountrySettingsItemId.SliderDay))
                ? Color.Red
                : SystemColors.WindowText;

            if ((settings == null) || (settings.Policy == null))
            {
                democraticTrackBar.Value = 6;
                politicalLeftTrackBar.Value = 6;
                freedomTrackBar.Value = 6;
                freeMarketTrackBar.Value = 6;
                professionalArmyTrackBar.Value = 6;
                defenseLobbyTrackBar.Value = 6;
                interventionismTrackBar.Value = 6;
                return;
            }

            int val = settings.Policy.Democratic;
            if (val < 1)
            {
                val = 1;
            }
            else if (val > 10)
            {
                val = 10;
            }
            democraticTrackBar.Value = 11 - val;

            val = settings.Policy.PoliticalLeft;
            if (val < 1)
            {
                val = 1;
            }
            else if (val > 10)
            {
                val = 10;
            }
            politicalLeftTrackBar.Value = 11 - val;

            val = settings.Policy.Freedom;
            if (val < 1)
            {
                val = 1;
            }
            else if (val > 10)
            {
                val = 10;
            }
            freedomTrackBar.Value = 11 - val;

            val = settings.Policy.FreeMarket;
            if (val < 1)
            {
                val = 1;
            }
            else if (val > 10)
            {
                val = 10;
            }
            freeMarketTrackBar.Value = 11 - val;

            val = settings.Policy.ProfessionalArmy;
            if (val < 1)
            {
                val = 1;
            }
            else if (val > 10)
            {
                val = 10;
            }
            professionalArmyTrackBar.Value = 11 - val;

            val = settings.Policy.DefenseLobby;
            if (val < 1)
            {
                val = 1;
            }
            else if (val > 10)
            {
                val = 10;
            }
            defenseLobbyTrackBar.Value = 11 - val;

            val = settings.Policy.Interventionism;
            if (val < 1)
            {
                val = 1;
            }
            else if (val > 10)
            {
                val = 10;
            }
            interventionismTrackBar.Value = 11 - val;
        }

        /// <summary>
        ///     スライダー移動可能年テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSliderYearTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(sliderYearTextBox.Text, out val))
            {
                sliderYearTextBox.Text = ((settings != null) && (settings.Policy != null) &&
                                          (settings.Policy.Date != null))
                    ? IntHelper.ToString(settings.Policy.Date.Year)
                    : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if (((settings == null) || (settings.Policy == null) || (settings.Policy.Date == null)) && (val == 5))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.Policy != null) && (settings.Policy.Date != null) &&
                (val == settings.Policy.Date.Year))
            {
                return;
            }

            Log.Info("[Scenario] slider year: {0} -> {1} ({2})",
                ((settings != null) && (settings.Policy != null) && (settings.Policy.Date != null))
                    ? IntHelper.ToString(settings.Policy.Date.Year)
                    : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }
            if (settings.Policy == null)
            {
                settings.Policy = new CountryPolicy
                {
                    Democratic = 5,
                    PoliticalLeft = 5,
                    Freedom = 5,
                    FreeMarket = 5,
                    ProfessionalArmy = 5,
                    DefenseLobby = 5,
                    Interventionism = 5
                };
            }
            if (settings.Policy.Date == null)
            {
                settings.Policy.Date = new GameDate();

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.SliderMonth);
                settings.SetDirty(CountrySettingsItemId.SliderDay);

                // 編集項目を更新する
                sliderMonthTextBox.Text = IntHelper.ToString(settings.Policy.Date.Month);
                sliderDayTextBox.Text = IntHelper.ToString(settings.Policy.Date.Day);

                // 文字色を変更する
                sliderMonthTextBox.ForeColor = Color.Red;
                sliderDayTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            settings.Policy.Date.Year = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.SliderYear);
            Scenarios.SetDirty();

            // 文字色を変更する
            sliderYearTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     スライダー移動可能月テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSliderMonthTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(sliderMonthTextBox.Text, out val))
            {
                sliderMonthTextBox.Text = ((settings != null) && (settings.Policy != null) &&
                                           (settings.Policy.Date != null))
                    ? IntHelper.ToString(settings.Policy.Date.Month)
                    : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if (((settings == null) || (settings.Policy == null) || (settings.Policy.Date == null)) && (val == 5))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.Policy != null) && (settings.Policy.Date != null) &&
                (val == settings.Policy.Date.Month))
            {
                return;
            }

            Log.Info("[Scenario] slider month: {0} -> {1} ({2})",
                ((settings != null) && (settings.Policy != null) && (settings.Policy.Date != null))
                    ? IntHelper.ToString(settings.Policy.Date.Month)
                    : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }
            if (settings.Policy == null)
            {
                settings.Policy = new CountryPolicy
                {
                    Democratic = 5,
                    PoliticalLeft = 5,
                    Freedom = 5,
                    FreeMarket = 5,
                    ProfessionalArmy = 5,
                    DefenseLobby = 5,
                    Interventionism = 5
                };
            }
            if (settings.Policy.Date == null)
            {
                settings.Policy.Date = new GameDate();

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.SliderYear);
                settings.SetDirty(CountrySettingsItemId.SliderDay);

                // 編集項目を更新する
                sliderYearTextBox.Text = IntHelper.ToString(settings.Policy.Date.Year);
                sliderDayTextBox.Text = IntHelper.ToString(settings.Policy.Date.Day);

                // 文字色を変更する
                sliderYearTextBox.ForeColor = Color.Red;
                sliderDayTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            settings.Policy.Date.Month = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.SliderMonth);
            Scenarios.SetDirty();

            // 文字色を変更する
            sliderMonthTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     スライダー移動可能日テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSliderDayTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(sliderDayTextBox.Text, out val))
            {
                sliderDayTextBox.Text = ((settings != null) && (settings.Policy != null) &&
                                         (settings.Policy.Date != null))
                    ? IntHelper.ToString(settings.Policy.Date.Day)
                    : "";
                return;
            }

            // 初期値から変更されていなければ何もしない
            if (((settings == null) || (settings.Policy == null) || (settings.Policy.Date == null)) && (val == 5))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.Policy != null) && (settings.Policy.Date != null) &&
                (val == settings.Policy.Date.Day))
            {
                return;
            }

            Log.Info("[Scenario] slider day: {0} -> {1} ({2})",
                ((settings != null) && (settings.Policy != null) && (settings.Policy.Date != null))
                    ? IntHelper.ToString(settings.Policy.Date.Day)
                    : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }
            if (settings.Policy == null)
            {
                settings.Policy = new CountryPolicy
                {
                    Democratic = 5,
                    PoliticalLeft = 5,
                    Freedom = 5,
                    FreeMarket = 5,
                    ProfessionalArmy = 5,
                    DefenseLobby = 5,
                    Interventionism = 5
                };
            }
            if (settings.Policy.Date == null)
            {
                settings.Policy.Date = new GameDate();

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.SliderYear);
                settings.SetDirty(CountrySettingsItemId.SliderMonth);

                // 編集項目を更新する
                sliderYearTextBox.Text = IntHelper.ToString(settings.Policy.Date.Year);
                sliderMonthTextBox.Text = IntHelper.ToString(settings.Policy.Date.Month);

                // 文字色を変更する
                sliderYearTextBox.ForeColor = Color.Red;
                sliderMonthTextBox.ForeColor = Color.Red;
            }

            // 値を更新する
            settings.Policy.Date.Day = val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.SliderDay);
            Scenarios.SetDirty();

            // 文字色を変更する
            sliderDayTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     民主的 - 独裁的スライドバーの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDemocraticTrackBarScroll(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 初期値から変更されていなければ何もしない
            int val = democraticTrackBar.Value;
            if (((settings == null) || (settings.Policy == null)) && (val == 6))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.Policy != null) && (val == settings.Policy.Democratic))
            {
                return;
            }

            Log.Info("[Scenario] democratic: {0} -> {1} ({2})",
                ((settings != null) && (settings.Policy != null)) ? IntHelper.ToString(settings.Policy.Democratic) : "",
                val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }
            if (settings.Policy == null)
            {
                settings.Policy = new CountryPolicy
                {
                    PoliticalLeft = 5,
                    Freedom = 5,
                    FreeMarket = 5,
                    ProfessionalArmy = 5,
                    DefenseLobby = 5,
                    Interventionism = 5
                };
            }

            // 値を更新する
            settings.Policy.Democratic = 11 - val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.Democratic);
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     政治的左派 - 政治的右派スライドバーの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPoliticalLeftTrackBarScroll(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 初期値から変更されていなければ何もしない
            int val = politicalLeftTrackBar.Value;
            if (((settings == null) || (settings.Policy == null)) && (val == 6))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.Policy != null) && (val == settings.Policy.PoliticalLeft))
            {
                return;
            }

            Log.Info("[Scenario] political left: {0} -> {1} ({2})",
                ((settings != null) && (settings.Policy != null))
                    ? IntHelper.ToString(settings.Policy.PoliticalLeft)
                    : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }
            if (settings.Policy == null)
            {
                settings.Policy = new CountryPolicy
                {
                    Democratic = 5,
                    Freedom = 5,
                    FreeMarket = 5,
                    ProfessionalArmy = 5,
                    DefenseLobby = 5,
                    Interventionism = 5
                };
            }

            // 値を更新する
            settings.Policy.PoliticalLeft = 11 - val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.PoliticalLeft);
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     開放社会 - 閉鎖社会スライドバーの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFreedomTrackBarScroll(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 初期値から変更されていなければ何もしない
            int val = freedomTrackBar.Value;
            if (((settings == null) || (settings.Policy == null)) && (val == 6))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.Policy != null) && (val == settings.Policy.Freedom))
            {
                return;
            }

            Log.Info("[Scenario] freedom: {0} -> {1} ({2})",
                ((settings != null) && (settings.Policy != null)) ? IntHelper.ToString(settings.Policy.Freedom) : "",
                val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }
            if (settings.Policy == null)
            {
                settings.Policy = new CountryPolicy
                {
                    Democratic = 5,
                    PoliticalLeft = 5,
                    FreeMarket = 5,
                    ProfessionalArmy = 5,
                    DefenseLobby = 5,
                    Interventionism = 5
                };
            }

            // 値を更新する
            settings.Policy.Freedom = 11 - val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.Freedom);
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     自由経済 - 中央計画経済スライドバーの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFreeMarketTrackBarScroll(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 初期値から変更されていなければ何もしない
            int val = freeMarketTrackBar.Value;
            if (((settings == null) || (settings.Policy == null)) && (val == 6))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.Policy != null) && (val == settings.Policy.FreeMarket))
            {
                return;
            }

            Log.Info("[Scenario] free market: {0} -> {1} ({2})",
                ((settings != null) && (settings.Policy != null)) ? IntHelper.ToString(settings.Policy.FreeMarket) : "",
                val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }
            if (settings.Policy == null)
            {
                settings.Policy = new CountryPolicy
                {
                    Democratic = 5,
                    PoliticalLeft = 5,
                    Freedom = 5,
                    ProfessionalArmy = 5,
                    DefenseLobby = 5,
                    Interventionism = 5
                };
            }

            // 値を更新する
            settings.Policy.FreeMarket = 11 - val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.FreeMarket);
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     常備軍 - 徴兵軍スライドバーの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProfessionalArmyTrackBarScroll(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 初期値から変更されていなければ何もしない
            int val = professionalArmyTrackBar.Value;
            if (((settings == null) || (settings.Policy == null)) && (val == 6))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.Policy != null) && (val == settings.Policy.ProfessionalArmy))
            {
                return;
            }

            Log.Info("[Scenario] professional army: {0} -> {1} ({2})",
                ((settings != null) && (settings.Policy != null))
                    ? IntHelper.ToString(settings.Policy.ProfessionalArmy)
                    : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }
            if (settings.Policy == null)
            {
                settings.Policy = new CountryPolicy
                {
                    Democratic = 5,
                    PoliticalLeft = 5,
                    Freedom = 5,
                    FreeMarket = 5,
                    DefenseLobby = 5,
                    Interventionism = 5
                };
            }

            // 値を更新する
            settings.Policy.ProfessionalArmy = 11 - val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.ProfessionalArmy);
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     タカ派 - ハト派スライドバーの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDefenseLobbyTrackBarScroll(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 初期値から変更されていなければ何もしない
            int val = defenseLobbyTrackBar.Value;
            if (((settings == null) || (settings.Policy == null)) && (val == 6))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.Policy != null) && (val == settings.Policy.DefenseLobby))
            {
                return;
            }

            Log.Info("[Scenario] defense lobby: {0} -> {1} ({2})",
                ((settings != null) && (settings.Policy != null))
                    ? IntHelper.ToString(settings.Policy.DefenseLobby)
                    : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }
            if (settings.Policy == null)
            {
                settings.Policy = new CountryPolicy
                {
                    Democratic = 5,
                    PoliticalLeft = 5,
                    Freedom = 5,
                    FreeMarket = 5,
                    ProfessionalArmy = 5,
                    Interventionism = 5
                };
            }

            // 値を更新する
            settings.Policy.DefenseLobby = 11 - val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.DefenseLobby);
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     介入主義 - 孤立主義スライドバーの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIntervensionismTrackBarScroll(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 初期値から変更されていなければ何もしない
            int val = interventionismTrackBar.Value;
            if (((settings == null) || (settings.Policy == null)) && (val == 6))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.Policy != null) && (val == settings.Policy.Interventionism))
            {
                return;
            }

            Log.Info("[Scenario] interventionism: {0} -> {1} ({2})",
                ((settings != null) && (settings.Policy != null))
                    ? IntHelper.ToString(settings.Policy.Interventionism)
                    : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }
            if (settings.Policy == null)
            {
                settings.Policy = new CountryPolicy
                {
                    Democratic = 5,
                    PoliticalLeft = 5,
                    Freedom = 5,
                    FreeMarket = 5,
                    ProfessionalArmy = 5,
                    DefenseLobby = 5
                };
            }

            // 値を更新する
            settings.Policy.Interventionism = 11 - val;

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.Interventionism);
            Scenarios.SetDirty();
        }

        #endregion

        #region 政府タブ - 閣僚

        /// <summary>
        ///     閣僚の編集項目を更新する
        /// </summary>
        /// <param name="settings"></param>
        private void UpdateCabinetItems(CountrySettings settings)
        {
            ScenarioHeader header = Scenarios.Data.Header;
            int year = (header.StartDate != null) ? header.StartDate.Year : header.StartYear;
            Country country = GetSelectedGovernmentCountry();
            List<Minister> ministers = Misc.EnableRetirementYearMinisters
                ? Ministers.Items.Where(
                    minister => (minister.Country == country) &&
                                (year >= minister.StartYear) &&
                                (year < minister.EndYear) &&
                                (year < minister.RetirementYear)).ToList()
                : Ministers.Items.Where(
                    minister => (minister.Country == country) &&
                                (year >= minister.StartYear) &&
                                (year < minister.EndYear)).ToList();

            bool flag = ((settings != null) && (settings.HeadOfState != null));
            _headOfStateList = ministers.Where(minister => minister.Position == MinisterPosition.HeadOfState).ToList();
            headOfStateComboBox.BeginUpdate();
            headOfStateComboBox.Items.Clear();
            foreach (Minister minister in _headOfStateList)
            {
                headOfStateComboBox.Items.Add(minister.Name);
            }
            headOfStateComboBox.SelectedIndex = flag
                ? _headOfStateList.FindIndex(minister => minister.Id == settings.HeadOfState.Id)
                : -1;
            headOfStateComboBox.EndUpdate();
            headOfStateTypeTextBox.Text = flag ? IntHelper.ToString(settings.HeadOfState.Type) : "";
            headOfStateIdTextBox.Text = flag ? IntHelper.ToString(settings.HeadOfState.Id) : "";

            flag = ((settings != null) && (settings.HeadOfGovernment != null));
            _headOfGovernmentList =
                ministers.Where(minister => minister.Position == MinisterPosition.HeadOfGovernment).ToList();
            headOfGovernmentComboBox.BeginUpdate();
            headOfGovernmentComboBox.Items.Clear();
            foreach (Minister minister in _headOfGovernmentList)
            {
                headOfGovernmentComboBox.Items.Add(minister.Name);
            }
            headOfGovernmentComboBox.SelectedIndex = flag
                ? _headOfGovernmentList.FindIndex(minister => minister.Id == settings.HeadOfGovernment.Id)
                : -1;
            headOfGovernmentComboBox.EndUpdate();
            headOfGovernmentTypeTextBox.Text = flag ? IntHelper.ToString(settings.HeadOfGovernment.Type) : "";
            headOfGovernmentIdTextBox.Text = flag ? IntHelper.ToString(settings.HeadOfGovernment.Id) : "";

            flag = ((settings != null) && (settings.ForeignMinister != null));
            _foreignMinisterList =
                ministers.Where(minister => minister.Position == MinisterPosition.ForeignMinister).ToList();
            foreignMinisterComboBox.BeginUpdate();
            foreignMinisterComboBox.Items.Clear();
            foreach (Minister minister in _foreignMinisterList)
            {
                foreignMinisterComboBox.Items.Add(minister.Name);
            }
            foreignMinisterComboBox.SelectedIndex = flag
                ? _foreignMinisterList.FindIndex(minister => minister.Id == settings.ForeignMinister.Id)
                : -1;
            foreignMinisterComboBox.EndUpdate();
            foreignMinisterTypeTextBox.Text = flag ? IntHelper.ToString(settings.ForeignMinister.Type) : "";
            foreignMinisterIdTextBox.Text = flag ? IntHelper.ToString(settings.ForeignMinister.Id) : "";

            flag = ((settings != null) && (settings.ArmamentMinister != null));
            _armamentMinisterList =
                ministers.Where(minister => minister.Position == MinisterPosition.MinisterOfArmament).ToList();
            armamentMinisterComboBox.BeginUpdate();
            armamentMinisterComboBox.Items.Clear();
            foreach (Minister minister in _armamentMinisterList)
            {
                armamentMinisterComboBox.Items.Add(minister.Name);
            }
            armamentMinisterComboBox.SelectedIndex = flag
                ? _armamentMinisterList.FindIndex(minister => minister.Id == settings.ArmamentMinister.Id)
                : -1;
            armamentMinisterComboBox.EndUpdate();
            armamentMinisterTypeTextBox.Text = flag ? IntHelper.ToString(settings.ArmamentMinister.Type) : "";
            armamentMinisterIdTextBox.Text = flag ? IntHelper.ToString(settings.ArmamentMinister.Id) : "";

            flag = ((settings != null) && (settings.MinisterOfSecurity != null));
            _ministerOfSecurityList =
                ministers.Where(minister => minister.Position == MinisterPosition.MinisterOfSecurity).ToList();
            ministerOfSecurityComboBox.BeginUpdate();
            ministerOfSecurityComboBox.Items.Clear();
            foreach (Minister minister in _ministerOfSecurityList)
            {
                ministerOfSecurityComboBox.Items.Add(minister.Name);
            }
            ministerOfSecurityComboBox.SelectedIndex = flag
                ? _ministerOfSecurityList.FindIndex(minister => minister.Id == settings.MinisterOfSecurity.Id)
                : -1;
            ministerOfSecurityComboBox.EndUpdate();
            ministerOfSecurityTypeTextBox.Text = flag ? IntHelper.ToString(settings.MinisterOfSecurity.Type) : "";
            ministerOfSecurityIdTextBox.Text = flag ? IntHelper.ToString(settings.MinisterOfSecurity.Id) : "";

            flag = ((settings != null) && (settings.MinisterOfIntelligence != null));
            _ministerOfIntelligenceList =
                ministers.Where(minister => minister.Position == MinisterPosition.HeadOfMilitaryIntelligence).ToList();
            ministerOfIntelligenceComboBox.BeginUpdate();
            ministerOfIntelligenceComboBox.Items.Clear();
            foreach (Minister minister in _ministerOfIntelligenceList)
            {
                ministerOfIntelligenceComboBox.Items.Add(minister.Name);
            }
            ministerOfIntelligenceComboBox.SelectedIndex = flag
                ? _ministerOfIntelligenceList.FindIndex(minister => minister.Id == settings.MinisterOfIntelligence.Id)
                : -1;
            ministerOfIntelligenceComboBox.EndUpdate();
            ministerOfIntelligenceTypeTextBox.Text = flag
                ? IntHelper.ToString(settings.MinisterOfIntelligence.Type)
                : "";
            ministerOfIntelligenceIdTextBox.Text = flag ? IntHelper.ToString(settings.MinisterOfIntelligence.Id) : "";

            flag = ((settings != null) && (settings.ChiefOfStaff != null));
            _chiefOfStaffList = ministers.Where(minister => minister.Position == MinisterPosition.ChiefOfStaff).ToList();
            chiefOfStaffComboBox.BeginUpdate();
            chiefOfStaffComboBox.Items.Clear();
            foreach (Minister minister in _chiefOfStaffList)
            {
                chiefOfStaffComboBox.Items.Add(minister.Name);
            }
            chiefOfStaffComboBox.SelectedIndex = flag
                ? _chiefOfStaffList.FindIndex(minister => minister.Id == settings.ChiefOfStaff.Id)
                : -1;
            chiefOfStaffComboBox.EndUpdate();
            chiefOfStaffTypeTextBox.Text = flag ? IntHelper.ToString(settings.ChiefOfStaff.Type) : "";
            chiefOfStaffIdTextBox.Text = flag ? IntHelper.ToString(settings.ChiefOfStaff.Id) : "";

            flag = ((settings != null) && (settings.ChiefOfArmy != null));
            _chiefOfArmyList = ministers.Where(minister => minister.Position == MinisterPosition.ChiefOfArmy).ToList();
            chiefOfArmyComboBox.BeginUpdate();
            chiefOfArmyComboBox.Items.Clear();
            foreach (Minister minister in _chiefOfArmyList)
            {
                chiefOfArmyComboBox.Items.Add(minister.Name);
            }
            chiefOfArmyComboBox.SelectedIndex = flag
                ? _chiefOfArmyList.FindIndex(minister => minister.Id == settings.ChiefOfArmy.Id)
                : -1;
            chiefOfArmyComboBox.EndUpdate();
            chiefOfArmyTypeTextBox.Text = flag ? IntHelper.ToString(settings.ChiefOfArmy.Type) : "";
            chiefOfArmyIdTextBox.Text = flag ? IntHelper.ToString(settings.ChiefOfArmy.Id) : "";

            flag = ((settings != null) && (settings.ChiefOfNavy != null));
            _chiefOfNavyList = ministers.Where(minister => minister.Position == MinisterPosition.ChiefOfNavy).ToList();
            chiefOfNavyComboBox.BeginUpdate();
            chiefOfNavyComboBox.Items.Clear();
            foreach (Minister minister in _chiefOfNavyList)
            {
                chiefOfNavyComboBox.Items.Add(minister.Name);
            }
            chiefOfNavyComboBox.SelectedIndex = flag
                ? _chiefOfNavyList.FindIndex(minister => minister.Id == settings.ChiefOfNavy.Id)
                : -1;
            chiefOfNavyComboBox.EndUpdate();
            chiefOfNavyTypeTextBox.Text = flag ? IntHelper.ToString(settings.ChiefOfNavy.Type) : "";
            chiefOfNavyIdTextBox.Text = flag ? IntHelper.ToString(settings.ChiefOfNavy.Id) : "";

            flag = ((settings != null) && (settings.ChiefOfAir != null));
            _chiefOfAirList =
                ministers.Where(minister => minister.Position == MinisterPosition.ChiefOfAirForce).ToList();
            chiefOfAirComboBox.BeginUpdate();
            chiefOfAirComboBox.Items.Clear();
            foreach (Minister minister in _chiefOfAirList)
            {
                chiefOfAirComboBox.Items.Add(minister.Name);
            }
            chiefOfAirComboBox.SelectedIndex = flag
                ? _chiefOfAirList.FindIndex(minister => minister.Id == settings.ChiefOfAir.Id)
                : -1;
            chiefOfAirComboBox.EndUpdate();
            chiefOfAirTypeTextBox.Text = flag ? IntHelper.ToString(settings.ChiefOfAir.Type) : "";
            chiefOfAirIdTextBox.Text = flag ? IntHelper.ToString(settings.ChiefOfAir.Id) : "";
        }

        /// <summary>
        ///     国家元首コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHeadOfStateComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Country country = GetSelectedGovernmentCountry();
            CountrySettings settings = Scenarios.GetCountrySettings(country);
            Brush brush = ((settings != null) && (settings.HeadOfState != null) &&
                           (_headOfStateList[e.Index].Id == settings.HeadOfState.Id) &&
                           settings.IsDirty(CountrySettingsItemId.HeadOfStateId))
                ? new SolidBrush(Color.Red)
                : new SolidBrush(SystemColors.WindowText);
            string s = headOfStateComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     政府首班コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHeadOfGovernmentComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Country country = GetSelectedGovernmentCountry();
            CountrySettings settings = Scenarios.GetCountrySettings(country);
            Brush brush = ((settings != null) && (settings.HeadOfGovernment != null) &&
                           (_headOfGovernmentList[e.Index].Id == settings.HeadOfGovernment.Id) &&
                           settings.IsDirty(CountrySettingsItemId.HeadOfGovernmentId))
                ? new SolidBrush(Color.Red)
                : new SolidBrush(SystemColors.WindowText);
            string s = headOfGovernmentComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     外務大臣コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnForeignMinisterComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Country country = GetSelectedGovernmentCountry();
            CountrySettings settings = Scenarios.GetCountrySettings(country);
            Brush brush = ((settings != null) && (settings.ForeignMinister != null) &&
                           (_foreignMinisterList[e.Index].Id == settings.ForeignMinister.Id) &&
                           settings.IsDirty(CountrySettingsItemId.ForeignMinisterId))
                ? new SolidBrush(Color.Red)
                : new SolidBrush(SystemColors.WindowText);
            string s = foreignMinisterComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     軍需大臣コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnArmamentMinisterComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Country country = GetSelectedGovernmentCountry();
            CountrySettings settings = Scenarios.GetCountrySettings(country);
            Brush brush = ((settings != null) && (settings.ArmamentMinister != null) &&
                           (_armamentMinisterList[e.Index].Id == settings.ArmamentMinister.Id) &&
                           settings.IsDirty(CountrySettingsItemId.ArmamentMinisterId))
                ? new SolidBrush(Color.Red)
                : new SolidBrush(SystemColors.WindowText);
            string s = armamentMinisterComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     内務大臣コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterOfSecurityComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Country country = GetSelectedGovernmentCountry();
            CountrySettings settings = Scenarios.GetCountrySettings(country);
            Brush brush = ((settings != null) && (settings.MinisterOfSecurity != null) &&
                           (_ministerOfSecurityList[e.Index].Id == settings.MinisterOfSecurity.Id) &&
                           settings.IsDirty(CountrySettingsItemId.MinisterOfSecurityId))
                ? new SolidBrush(Color.Red)
                : new SolidBrush(SystemColors.WindowText);
            string s = ministerOfSecurityComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     情報大臣コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterOfIntelligenceComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Country country = GetSelectedGovernmentCountry();
            CountrySettings settings = Scenarios.GetCountrySettings(country);
            Brush brush = ((settings != null) && (settings.MinisterOfIntelligence != null) &&
                           (_ministerOfIntelligenceList[e.Index].Id == settings.MinisterOfIntelligence.Id) &&
                           settings.IsDirty(CountrySettingsItemId.MinisterOfIntelligenceId))
                ? new SolidBrush(Color.Red)
                : new SolidBrush(SystemColors.WindowText);
            string s = ministerOfIntelligenceComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     統合参謀総長コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChiefOfStaffComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Country country = GetSelectedGovernmentCountry();
            CountrySettings settings = Scenarios.GetCountrySettings(country);
            Brush brush = ((settings != null) && (settings.ChiefOfStaff != null) &&
                           (_chiefOfStaffList[e.Index].Id == settings.ChiefOfStaff.Id) &&
                           settings.IsDirty(CountrySettingsItemId.ChiefOfStaffId))
                ? new SolidBrush(Color.Red)
                : new SolidBrush(SystemColors.WindowText);
            string s = chiefOfStaffComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     陸軍総司令官コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChiefOfArmyComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Country country = GetSelectedGovernmentCountry();
            CountrySettings settings = Scenarios.GetCountrySettings(country);
            Brush brush = ((settings != null) && (settings.ChiefOfArmy != null) &&
                           (_chiefOfArmyList[e.Index].Id == settings.ChiefOfArmy.Id) &&
                           settings.IsDirty(CountrySettingsItemId.ChiefOfArmyId))
                ? new SolidBrush(Color.Red)
                : new SolidBrush(SystemColors.WindowText);
            string s = chiefOfArmyComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     海軍総司令官コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChiefOfNavyComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Country country = GetSelectedGovernmentCountry();
            CountrySettings settings = Scenarios.GetCountrySettings(country);
            Brush brush = ((settings != null) && (settings.ChiefOfNavy != null) &&
                           (_chiefOfNavyList[e.Index].Id == settings.ChiefOfNavy.Id) &&
                           settings.IsDirty(CountrySettingsItemId.ChiefOfNavyId))
                ? new SolidBrush(Color.Red)
                : new SolidBrush(SystemColors.WindowText);
            string s = chiefOfNavyComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     空軍総司令官コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChiefOfAirComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Country country = GetSelectedGovernmentCountry();
            CountrySettings settings = Scenarios.GetCountrySettings(country);
            Brush brush = ((settings != null) && (settings.ChiefOfAir != null) &&
                           (_chiefOfAirList[e.Index].Id == settings.ChiefOfAir.Id) &&
                           settings.IsDirty(CountrySettingsItemId.ChiefOfAirId))
                ? new SolidBrush(Color.Red)
                : new SolidBrush(SystemColors.WindowText);
            string s = chiefOfAirComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     国家元首コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHeadOfStateComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            int index = headOfStateComboBox.SelectedIndex;
            if (index < 0)
            {
                return;
            }
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 値に変化がなければ何もしない
            int val = _headOfStateList[index].Id;
            if ((settings != null) && (settings.HeadOfState != null) &&
                (val == settings.HeadOfState.Id))
            {
                return;
            }

            Log.Info("[Scenario] head of state id: {0} -> {1} ({2})",
                (settings != null) && (settings.HeadOfState != null) ? IntHelper.ToString(settings.HeadOfState.Id) : "",
                val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (settings.HeadOfState != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(settings.HeadOfState);

                // 値を更新する
                settings.HeadOfState.Id = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(settings.HeadOfState);
            }
            else
            {
                // 値を更新する
                int type = 1;
                while (Scenarios.ExistsTypeId(type, val))
                {
                    type++;
                }
                settings.HeadOfState = Scenarios.GetNewTypeId(type, val);

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.HeadOfStateType);

                // 編集項目を更新する
                headOfStateTypeTextBox.Text = IntHelper.ToString(type);

                // 文字色を変更する
                headOfStateTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.HeadOfStateId);
            Scenarios.SetDirty();

            // 項目色を変更するために描画更新する
            headOfStateComboBox.Refresh();

            // テキストボックスの表示を変更する
            headOfStateIdTextBox.Text = IntHelper.ToString(settings.HeadOfState.Id);

            // 文字色を変更する
            headOfStateIdTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     政府首班コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHeadOfGovernmentComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            int index = headOfGovernmentComboBox.SelectedIndex;
            if (index < 0)
            {
                return;
            }
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 値に変化がなければ何もしない
            int val = _headOfGovernmentList[index].Id;
            if ((settings != null) && (settings.HeadOfGovernment != null) &&
                (val == settings.HeadOfGovernment.Id))
            {
                return;
            }

            Log.Info("[Scenario] head of government id: {0} -> {1} ({2})",
                (settings != null) && (settings.HeadOfGovernment != null)
                    ? IntHelper.ToString(settings.HeadOfGovernment.Id)
                    : "",
                val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (settings.HeadOfGovernment != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(settings.HeadOfGovernment);

                // 値を更新する
                settings.HeadOfGovernment.Id = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(settings.HeadOfGovernment);
            }
            else
            {
                // 値を更新する
                int type = (settings.HeadOfGovernment != null) ? settings.HeadOfGovernment.Type : 1;
                while (Scenarios.ExistsTypeId(type, val))
                {
                    type++;
                }
                settings.HeadOfGovernment = Scenarios.GetNewTypeId(type, val);

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.HeadOfGovernmentType);

                // 編集項目を更新する
                headOfGovernmentTypeTextBox.Text = IntHelper.ToString(type);

                // 文字色を変更する
                headOfGovernmentTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.HeadOfGovernmentId);
            Scenarios.SetDirty();

            // 項目色を変更するために描画更新する
            headOfGovernmentComboBox.Refresh();

            // テキストボックスの表示を変更する
            headOfGovernmentIdTextBox.Text = IntHelper.ToString(settings.HeadOfGovernment.Id);

            // 文字色を変更する
            headOfGovernmentIdTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     外務大臣コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnForeignMinisterComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            int index = foreignMinisterComboBox.SelectedIndex;
            if (index < 0)
            {
                return;
            }
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 値に変化がなければ何もしない
            int val = _foreignMinisterList[index].Id;
            if ((settings != null) && (settings.ForeignMinister != null) &&
                (val == settings.ForeignMinister.Id))
            {
                return;
            }

            Log.Info("[Scenario] foreign minister id: {0} -> {1} ({2})",
                (settings != null) && (settings.ForeignMinister != null)
                    ? IntHelper.ToString(settings.ForeignMinister.Id)
                    : "",
                val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (settings.ForeignMinister != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(settings.ForeignMinister);

                // 値を更新する
                settings.ForeignMinister.Id = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(settings.ForeignMinister);
            }
            else
            {
                // 値を更新する
                int type = (settings.ForeignMinister != null) ? settings.ForeignMinister.Type : 1;
                while (Scenarios.ExistsTypeId(type, val))
                {
                    type++;
                }
                settings.ForeignMinister = Scenarios.GetNewTypeId(type, val);

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.ForeignMinisterType);

                // 編集項目を更新する
                foreignMinisterTypeTextBox.Text = IntHelper.ToString(type);

                // 文字色を変更する
                foreignMinisterTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.ForeignMinisterId);
            Scenarios.SetDirty();

            // 項目色を変更するために描画更新する
            foreignMinisterComboBox.Refresh();

            // テキストボックスの表示を変更する
            foreignMinisterIdTextBox.Text = IntHelper.ToString(settings.ForeignMinister.Id);

            // 文字色を変更する
            foreignMinisterIdTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     軍需大臣コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnArmamentMinisterComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            int index = armamentMinisterComboBox.SelectedIndex;
            if (index < 0)
            {
                return;
            }
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 値に変化がなければ何もしない
            int val = _armamentMinisterList[index].Id;
            if ((settings != null) && (settings.ArmamentMinister != null) &&
                (val == settings.ArmamentMinister.Id))
            {
                return;
            }

            Log.Info("[Scenario] armament minister id: {0} -> {1} ({2})",
                (settings != null) && (settings.ArmamentMinister != null)
                    ? IntHelper.ToString(settings.ArmamentMinister.Id)
                    : "",
                val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (settings.ArmamentMinister != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(settings.ArmamentMinister);

                // 値を更新する
                settings.ArmamentMinister.Id = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(settings.ArmamentMinister);
            }
            else
            {
                // 値を更新する
                int type = (settings.ArmamentMinister != null) ? settings.ArmamentMinister.Type : 1;
                while (Scenarios.ExistsTypeId(type, val))
                {
                    type++;
                }
                settings.ArmamentMinister = Scenarios.GetNewTypeId(type, val);

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.ArmamentMinisterType);

                // 編集項目を更新する
                armamentMinisterTypeTextBox.Text = IntHelper.ToString(type);

                // 文字色を変更する
                armamentMinisterTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.ArmamentMinisterId);
            Scenarios.SetDirty();

            // 項目色を変更するために描画更新する
            armamentMinisterComboBox.Refresh();

            // テキストボックスの表示を変更する
            armamentMinisterIdTextBox.Text = IntHelper.ToString(settings.ArmamentMinister.Id);

            // 文字色を変更する
            armamentMinisterIdTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     内務大臣コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterOfSecurityComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            int index = ministerOfSecurityComboBox.SelectedIndex;
            if (index < 0)
            {
                return;
            }
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 値に変化がなければ何もしない
            int val = _ministerOfSecurityList[index].Id;
            if ((settings != null) && (settings.MinisterOfSecurity != null) &&
                (val == settings.MinisterOfSecurity.Id))
            {
                return;
            }

            Log.Info("[Scenario] minister of security id: {0} -> {1} ({2})",
                (settings != null) && (settings.MinisterOfSecurity != null)
                    ? IntHelper.ToString(settings.MinisterOfSecurity.Id)
                    : "",
                val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (settings.MinisterOfSecurity != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(settings.MinisterOfSecurity);

                // 値を更新する
                settings.MinisterOfSecurity.Id = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(settings.MinisterOfSecurity);
            }
            else
            {
                // 値を更新する
                int type = (settings.MinisterOfSecurity != null) ? settings.MinisterOfSecurity.Type : 1;
                while (Scenarios.ExistsTypeId(type, val))
                {
                    type++;
                }
                settings.MinisterOfSecurity = Scenarios.GetNewTypeId(type, val);

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.MinisterOfSecurityType);

                // 編集項目を更新する
                ministerOfSecurityTypeTextBox.Text = IntHelper.ToString(type);

                // 文字色を変更する
                ministerOfSecurityTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.MinisterOfSecurityId);
            Scenarios.SetDirty();

            // 項目色を変更するために描画更新する
            ministerOfSecurityComboBox.Refresh();

            // テキストボックスの表示を変更する
            ministerOfSecurityIdTextBox.Text = IntHelper.ToString(settings.MinisterOfSecurity.Id);

            // 文字色を変更する
            ministerOfSecurityIdTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     情報大臣コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterOfIntelligenceComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            int index = ministerOfIntelligenceComboBox.SelectedIndex;
            if (index < 0)
            {
                return;
            }
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 値に変化がなければ何もしない
            int val = _ministerOfIntelligenceList[index].Id;
            if ((settings != null) && (settings.MinisterOfIntelligence != null) &&
                (val == settings.MinisterOfIntelligence.Id))
            {
                return;
            }

            Log.Info("[Scenario] minister of intelligence id: {0} -> {1} ({2})",
                (settings != null) && (settings.MinisterOfIntelligence != null)
                    ? IntHelper.ToString(settings.MinisterOfIntelligence.Id)
                    : "",
                val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (settings.MinisterOfIntelligence != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(settings.MinisterOfIntelligence);

                // 値を更新する
                settings.MinisterOfIntelligence.Id = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(settings.MinisterOfIntelligence);
            }
            else
            {
                // 値を更新する
                int type = (settings.MinisterOfIntelligence != null) ? settings.MinisterOfIntelligence.Type : 1;
                while (Scenarios.ExistsTypeId(type, val))
                {
                    type++;
                }
                settings.MinisterOfIntelligence = Scenarios.GetNewTypeId(type, val);

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.MinisterOfIntelligenceType);

                // 編集項目を更新する
                ministerOfIntelligenceTypeTextBox.Text = IntHelper.ToString(type);

                // 文字色を変更する
                ministerOfIntelligenceTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.MinisterOfIntelligenceId);
            Scenarios.SetDirty();

            // 項目色を変更するために描画更新する
            ministerOfIntelligenceComboBox.Refresh();

            // テキストボックスの表示を変更する
            ministerOfIntelligenceIdTextBox.Text = IntHelper.ToString(settings.MinisterOfIntelligence.Id);

            // 文字色を変更する
            ministerOfIntelligenceIdTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     統合参謀総長コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChiefOfStaffComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            int index = chiefOfStaffComboBox.SelectedIndex;
            if (index < 0)
            {
                return;
            }
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 値に変化がなければ何もしない
            int val = _chiefOfStaffList[index].Id;
            if ((settings != null) && (settings.ChiefOfStaff != null) &&
                (val == settings.ChiefOfStaff.Id))
            {
                return;
            }

            Log.Info("[Scenario] chief of staff id: {0} -> {1} ({2})",
                (settings != null) && (settings.ChiefOfStaff != null)
                    ? IntHelper.ToString(settings.ChiefOfStaff.Id)
                    : "",
                val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (settings.ChiefOfStaff != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(settings.ChiefOfStaff);

                // 値を更新する
                settings.ChiefOfStaff.Id = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(settings.ChiefOfStaff);
            }
            else
            {
                // 値を更新する
                int type = (settings.ChiefOfStaff != null) ? settings.ChiefOfStaff.Type : 1;
                while (Scenarios.ExistsTypeId(type, val))
                {
                    type++;
                }
                settings.ChiefOfStaff = Scenarios.GetNewTypeId(type, val);

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.ChiefOfStaffType);

                // 編集項目を更新する
                chiefOfStaffTypeTextBox.Text = IntHelper.ToString(type);

                // 文字色を変更する
                chiefOfStaffTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.ChiefOfStaffId);
            Scenarios.SetDirty();

            // 項目色を変更するために描画更新する
            chiefOfStaffComboBox.Refresh();

            // テキストボックスの表示を変更する
            chiefOfStaffIdTextBox.Text = IntHelper.ToString(settings.ChiefOfStaff.Id);

            // 文字色を変更する
            chiefOfStaffIdTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     陸軍総司令官コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChiefOfArmyComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            int index = chiefOfArmyComboBox.SelectedIndex;
            if (index < 0)
            {
                return;
            }
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 値に変化がなければ何もしない
            int val = _chiefOfArmyList[index].Id;
            if ((settings != null) && (settings.ChiefOfArmy != null) &&
                (val == settings.ChiefOfArmy.Id))
            {
                return;
            }

            Log.Info("[Scenario] chief of army id: {0} -> {1} ({2})",
                (settings != null) && (settings.ChiefOfArmy != null) ? IntHelper.ToString(settings.ChiefOfArmy.Id) : "",
                val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (settings.ChiefOfArmy != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(settings.ChiefOfArmy);

                // 値を更新する
                settings.ChiefOfArmy.Id = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(settings.ChiefOfArmy);
            }
            else
            {
                // 値を更新する
                int type = (settings.ChiefOfArmy != null) ? settings.ChiefOfArmy.Type : 1;
                while (Scenarios.ExistsTypeId(type, val))
                {
                    type++;
                }
                settings.ChiefOfArmy = Scenarios.GetNewTypeId(type, val);

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.ChiefOfArmyType);

                // 編集項目を更新する
                chiefOfArmyTypeTextBox.Text = IntHelper.ToString(type);

                // 文字色を変更する
                chiefOfArmyTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.ChiefOfArmyId);
            Scenarios.SetDirty();

            // 項目色を変更するために描画更新する
            chiefOfArmyComboBox.Refresh();

            // テキストボックスの表示を変更する
            chiefOfArmyIdTextBox.Text = IntHelper.ToString(settings.ChiefOfArmy.Id);

            // 文字色を変更する
            chiefOfArmyIdTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     海軍総司令官コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChiefOfNavyComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            int index = chiefOfNavyComboBox.SelectedIndex;
            if (index < 0)
            {
                return;
            }
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 値に変化がなければ何もしない
            int val = _chiefOfNavyList[index].Id;
            if ((settings != null) && (settings.ChiefOfNavy != null) &&
                (val == settings.ChiefOfNavy.Id))
            {
                return;
            }

            Log.Info("[Scenario] chief of navy id: {0} -> {1} ({2})",
                (settings != null) && (settings.ChiefOfNavy != null) ? IntHelper.ToString(settings.ChiefOfNavy.Id) : "",
                val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (settings.ChiefOfNavy != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(settings.ChiefOfNavy);

                // 値を更新する
                settings.ChiefOfNavy.Id = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(settings.ChiefOfNavy);
            }
            else
            {
                // 値を更新する
                int type = (settings.ChiefOfNavy != null) ? settings.ChiefOfNavy.Type : 1;
                while (Scenarios.ExistsTypeId(type, val))
                {
                    type++;
                }
                settings.ChiefOfNavy = Scenarios.GetNewTypeId(type, val);

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.ChiefOfNavyType);

                // 編集項目を更新する
                chiefOfNavyTypeTextBox.Text = IntHelper.ToString(type);

                // 文字色を変更する
                chiefOfNavyTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.ChiefOfNavyId);
            Scenarios.SetDirty();

            // 項目色を変更するために描画更新する
            chiefOfNavyComboBox.Refresh();

            // テキストボックスの表示を変更する
            chiefOfNavyIdTextBox.Text = IntHelper.ToString(settings.ChiefOfNavy.Id);

            // 文字色を変更する
            chiefOfNavyIdTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     空軍総司令官コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChiefOfAirComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            int index = chiefOfAirComboBox.SelectedIndex;
            if (index < 0)
            {
                return;
            }
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 値に変化がなければ何もしない
            int val = _chiefOfAirList[index].Id;
            if ((settings != null) && (settings.ChiefOfAir != null) &&
                (val == settings.ChiefOfAir.Id))
            {
                return;
            }

            Log.Info("[Scenario] chief of air id: {0} -> {1} ({2})",
                (settings != null) && (settings.ChiefOfAir != null) ? IntHelper.ToString(settings.ChiefOfAir.Id) : "",
                val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (settings.ChiefOfAir != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(settings.ChiefOfAir);

                // 値を更新する
                settings.ChiefOfAir.Id = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(settings.ChiefOfAir);
            }
            else
            {
                // 値を更新する
                int type = (settings.ChiefOfAir != null) ? settings.ChiefOfAir.Type : 1;
                while (Scenarios.ExistsTypeId(type, val))
                {
                    type++;
                }
                settings.ChiefOfAir = Scenarios.GetNewTypeId(type, val);

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.ChiefOfAirType);

                // 編集項目を更新する
                chiefOfAirTypeTextBox.Text = IntHelper.ToString(type);

                // 文字色を変更する
                chiefOfAirTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.ChiefOfAirId);
            Scenarios.SetDirty();

            // 項目色を変更するために描画更新する
            chiefOfAirComboBox.Refresh();

            // テキストボックスの表示を変更する
            chiefOfAirIdTextBox.Text = IntHelper.ToString(settings.ChiefOfAir.Id);

            // 文字色を変更する
            chiefOfAirIdTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     国家元首のtypeテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHeadOfStateTypeTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(headOfStateTypeTextBox.Text, out val))
            {
                headOfStateTypeTextBox.Text = ((settings != null) && (settings.HeadOfState != null))
                    ? IntHelper.ToString(settings.HeadOfState.Type)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.HeadOfState != null) && (val == settings.HeadOfState.Type))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if ((settings != null) && (settings.HeadOfState != null) &&
                Scenarios.ExistsTypeId(val, settings.HeadOfState.Id))
            {
                headOfStateTypeTextBox.Text = IntHelper.ToString(settings.HeadOfState.Type);
                return;
            }

            Log.Info("[Scenario] head of state type: {0} -> {1} ({2})",
                ((settings != null) && (settings.HeadOfState != null))
                    ? IntHelper.ToString(settings.HeadOfState.Type)
                    : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (settings.HeadOfState != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(settings.HeadOfState);

                // 値を更新する
                settings.HeadOfState.Type = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(settings.HeadOfState);
            }
            else
            {
                // 値を更新する
                settings.HeadOfState = Scenarios.GetNewTypeId(val, 1);

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.HeadOfStateId);

                // 編集項目を更新する
                headOfStateIdTextBox.Text = IntHelper.ToString(settings.HeadOfState.Id);

                // 文字色を変更する
                headOfStateIdTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.HeadOfStateType);
            Scenarios.SetDirty();

            // 文字色を変更する
            headOfStateTypeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     国家元首のidテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHeadOfStateIdTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(headOfStateIdTextBox.Text, out val))
            {
                headOfStateIdTextBox.Text = ((settings != null) && (settings.HeadOfState != null))
                    ? IntHelper.ToString(settings.HeadOfState.Id)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.HeadOfState != null) && (val == settings.HeadOfState.Id))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if (
                Scenarios.ExistsTypeId(
                    ((settings != null) && (settings.HeadOfState != null))
                        ? settings.HeadOfState.Type
                        : Scenarios.DefaultWarType, val))
            {
                headOfStateIdTextBox.Text = ((settings != null) && (settings.HeadOfState != null))
                    ? IntHelper.ToString(settings.HeadOfState.Id)
                    : "";
            }

            Log.Info("[Scenario] head of state id: {0} -> {1} ({2})",
                ((settings != null) && (settings.HeadOfState != null))
                    ? IntHelper.ToString(settings.HeadOfState.Id)
                    : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (settings.HeadOfState != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(settings.HeadOfState);

                // 値を更新する
                settings.HeadOfState.Id = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(settings.HeadOfState);
            }
            else
            {
                // 値を更新する
                int type = 1;
                while (Scenarios.ExistsTypeId(type, val))
                {
                    type++;
                }
                settings.HeadOfState = Scenarios.GetNewTypeId(type, val);

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.HeadOfStateType);

                // 編集項目を更新する
                headOfStateTypeTextBox.Text = IntHelper.ToString(type);

                // 文字色を変更する
                headOfStateTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.HeadOfStateId);
            Scenarios.SetDirty();

            // 文字色を変更する
            headOfStateIdTextBox.ForeColor = Color.Red;

            // コンボボックスの選択項目を変更する
            headOfStateComboBox.SelectedIndex =
                _headOfStateList.FindIndex(minister => minister.Id == settings.HeadOfState.Id);
        }

        /// <summary>
        ///     政府首班のtypeテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHeadOfGovernmentTypeTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(headOfGovernmentTypeTextBox.Text, out val))
            {
                headOfGovernmentTypeTextBox.Text = ((settings != null) && (settings.HeadOfGovernment != null))
                    ? IntHelper.ToString(settings.HeadOfGovernment.Type)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.HeadOfGovernment != null) && (val == settings.HeadOfGovernment.Type))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if ((settings != null) && (settings.HeadOfGovernment != null) &&
                Scenarios.ExistsTypeId(val, settings.HeadOfGovernment.Id))
            {
                headOfGovernmentTypeTextBox.Text = IntHelper.ToString(settings.HeadOfGovernment.Type);
                return;
            }

            Log.Info("[Scenario] head of government type: {0} -> {1} ({2})",
                ((settings != null) && (settings.HeadOfGovernment != null))
                    ? IntHelper.ToString(settings.HeadOfGovernment.Type)
                    : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (settings.HeadOfGovernment != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(settings.HeadOfGovernment);

                // 値を更新する
                settings.HeadOfGovernment.Type = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(settings.HeadOfGovernment);
            }
            else
            {
                // 値を更新する
                settings.HeadOfGovernment = Scenarios.GetNewTypeId(val, 1);

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.HeadOfGovernmentId);

                // 編集項目を更新する
                headOfGovernmentIdTextBox.Text = IntHelper.ToString(settings.HeadOfGovernment.Id);

                // 文字色を変更する
                headOfGovernmentIdTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.HeadOfGovernmentType);
            Scenarios.SetDirty();

            // 文字色を変更する
            headOfGovernmentTypeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     政府首班のidテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHeadOfGovernmentIdTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(headOfGovernmentIdTextBox.Text, out val))
            {
                headOfGovernmentIdTextBox.Text = ((settings != null) && (settings.HeadOfGovernment != null))
                    ? IntHelper.ToString(settings.HeadOfGovernment.Id)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.HeadOfGovernment != null) && (val == settings.HeadOfGovernment.Id))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if (Scenarios.ExistsTypeId(
                ((settings != null) && (settings.HeadOfGovernment != null))
                    ? settings.HeadOfGovernment.Type
                    : Scenarios.DefaultWarType, val))
            {
                headOfGovernmentIdTextBox.Text = ((settings != null) && (settings.HeadOfGovernment != null))
                    ? IntHelper.ToString(settings.HeadOfGovernment.Id)
                    : "";
            }

            Log.Info("[Scenario] head of government id: {0} -> {1} ({2})",
                ((settings != null) && (settings.HeadOfGovernment != null))
                    ? IntHelper.ToString(settings.HeadOfGovernment.Id)
                    : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (settings.HeadOfGovernment != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(settings.HeadOfGovernment);

                // 値を更新する
                settings.HeadOfGovernment.Id = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(settings.HeadOfGovernment);
            }
            else
            {
                // 値を更新する
                int type = (settings.HeadOfGovernment != null) ? settings.HeadOfGovernment.Type : 1;
                while (Scenarios.ExistsTypeId(type, val))
                {
                    type++;
                }
                settings.HeadOfGovernment = Scenarios.GetNewTypeId(type, val);

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.HeadOfGovernmentType);

                // 編集項目を更新する
                headOfGovernmentTypeTextBox.Text = IntHelper.ToString(type);

                // 文字色を変更する
                headOfGovernmentTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.HeadOfGovernmentId);
            Scenarios.SetDirty();

            // 文字色を変更する
            headOfGovernmentIdTextBox.ForeColor = Color.Red;

            // コンボボックスの選択項目を変更する
            headOfGovernmentComboBox.SelectedIndex =
                _headOfGovernmentList.FindIndex(minister => minister.Id == settings.HeadOfGovernment.Id);
        }

        /// <summary>
        ///     外務大臣のtypeテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnForeignMinisterTypeTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(foreignMinisterTypeTextBox.Text, out val))
            {
                foreignMinisterTypeTextBox.Text = ((settings != null) && (settings.ForeignMinister != null))
                    ? IntHelper.ToString(settings.ForeignMinister.Type)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.ForeignMinister != null) && (val == settings.ForeignMinister.Type))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if ((settings != null) && (settings.ForeignMinister != null) &&
                Scenarios.ExistsTypeId(val, settings.ForeignMinister.Id))
            {
                foreignMinisterTypeTextBox.Text = IntHelper.ToString(settings.ForeignMinister.Type);
                return;
            }

            Log.Info("[Scenario] foreign minister type: {0} -> {1} ({2})",
                ((settings != null) && (settings.ForeignMinister != null))
                    ? IntHelper.ToString(settings.ForeignMinister.Type)
                    : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (settings.ForeignMinister != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(settings.ForeignMinister);

                // 値を更新する
                settings.ForeignMinister.Type = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(settings.ForeignMinister);
            }
            else
            {
                // 値を更新する
                settings.ForeignMinister = Scenarios.GetNewTypeId(val, 1);

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.ForeignMinisterId);

                // 編集項目を更新する
                foreignMinisterIdTextBox.Text = IntHelper.ToString(settings.ForeignMinister.Id);

                // 文字色を変更する
                foreignMinisterIdTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.ForeignMinisterType);
            Scenarios.SetDirty();

            // 文字色を変更する
            foreignMinisterTypeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     外務大臣のidテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnForeignMinisterIdTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(foreignMinisterIdTextBox.Text, out val))
            {
                foreignMinisterIdTextBox.Text = ((settings != null) && (settings.ForeignMinister != null))
                    ? IntHelper.ToString(settings.ForeignMinister.Id)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.ForeignMinister != null) && (val == settings.ForeignMinister.Id))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if (Scenarios.ExistsTypeId(
                ((settings != null) && (settings.ForeignMinister != null))
                    ? settings.ForeignMinister.Type
                    : Scenarios.DefaultWarType, val))
            {
                foreignMinisterIdTextBox.Text = ((settings != null) && (settings.ForeignMinister != null))
                    ? IntHelper.ToString(settings.ForeignMinister.Id)
                    : "";
            }

            Log.Info("[Scenario] foreign minister id: {0} -> {1} ({2})",
                ((settings != null) && (settings.ForeignMinister != null))
                    ? IntHelper.ToString(settings.ForeignMinister.Id)
                    : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (settings.ForeignMinister != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(settings.ForeignMinister);

                // 値を更新する
                settings.ForeignMinister.Id = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(settings.ForeignMinister);
            }
            else
            {
                // 値を更新する
                int type = (settings.ForeignMinister != null) ? settings.ForeignMinister.Type : 1;
                while (Scenarios.ExistsTypeId(type, val))
                {
                    type++;
                }
                settings.ForeignMinister = Scenarios.GetNewTypeId(type, val);

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.ForeignMinisterType);

                // 編集項目を更新する
                foreignMinisterTypeTextBox.Text = IntHelper.ToString(type);

                // 文字色を変更する
                foreignMinisterTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.ForeignMinisterId);
            Scenarios.SetDirty();

            // 文字色を変更する
            foreignMinisterIdTextBox.ForeColor = Color.Red;

            // コンボボックスの選択項目を変更する
            foreignMinisterComboBox.SelectedIndex =
                _foreignMinisterList.FindIndex(minister => minister.Id == settings.ForeignMinister.Id);
        }

        /// <summary>
        ///     軍需大臣のtypeテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnArmamentMinisterTypeTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(armamentMinisterTypeTextBox.Text, out val))
            {
                armamentMinisterTypeTextBox.Text = ((settings != null) && (settings.ArmamentMinister != null))
                    ? IntHelper.ToString(settings.ArmamentMinister.Type)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.ArmamentMinister != null) && (val == settings.ArmamentMinister.Type))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if ((settings != null) && (settings.ArmamentMinister != null) &&
                Scenarios.ExistsTypeId(val, settings.ArmamentMinister.Id))
            {
                armamentMinisterTypeTextBox.Text = IntHelper.ToString(settings.ArmamentMinister.Type);
                return;
            }

            Log.Info("[Scenario] armament minister type: {0} -> {1} ({2})",
                ((settings != null) && (settings.ArmamentMinister != null))
                    ? IntHelper.ToString(settings.ArmamentMinister.Type)
                    : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (settings.ArmamentMinister != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(settings.ArmamentMinister);

                // 値を更新する
                settings.ArmamentMinister.Type = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(settings.ArmamentMinister);
            }
            else
            {
                // 値を更新する
                settings.ArmamentMinister = Scenarios.GetNewTypeId(val, 1);

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.ArmamentMinisterId);

                // 編集項目を更新する
                armamentMinisterIdTextBox.Text = IntHelper.ToString(settings.ArmamentMinister.Id);

                // 文字色を変更する
                armamentMinisterIdTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.ArmamentMinisterType);
            Scenarios.SetDirty();

            // 文字色を変更する
            armamentMinisterTypeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     軍需大臣のidテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnArmamentMinisterIdTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(armamentMinisterIdTextBox.Text, out val))
            {
                armamentMinisterIdTextBox.Text = ((settings != null) && (settings.ArmamentMinister != null))
                    ? IntHelper.ToString(settings.ArmamentMinister.Id)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.ArmamentMinister != null) && (val == settings.ArmamentMinister.Id))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if (Scenarios.ExistsTypeId(
                ((settings != null) && (settings.ArmamentMinister != null))
                    ? settings.ArmamentMinister.Type
                    : Scenarios.DefaultWarType, val))
            {
                armamentMinisterIdTextBox.Text = ((settings != null) && (settings.ArmamentMinister != null))
                    ? IntHelper.ToString(settings.ArmamentMinister.Id)
                    : "";
            }

            Log.Info("[Scenario] armament minister id: {0} -> {1} ({2})",
                ((settings != null) && (settings.ArmamentMinister != null))
                    ? IntHelper.ToString(settings.ArmamentMinister.Id)
                    : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (settings.ArmamentMinister != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(settings.ArmamentMinister);

                // 値を更新する
                settings.ArmamentMinister.Id = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(settings.ArmamentMinister);
            }
            else
            {
                // 値を更新する
                int type = (settings.ArmamentMinister != null) ? settings.ArmamentMinister.Type : 1;
                while (Scenarios.ExistsTypeId(type, val))
                {
                    type++;
                }
                settings.ArmamentMinister = Scenarios.GetNewTypeId(type, val);

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.ArmamentMinisterType);

                // 編集項目を更新する
                armamentMinisterTypeTextBox.Text = IntHelper.ToString(type);

                // 文字色を変更する
                armamentMinisterTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.ArmamentMinisterId);
            Scenarios.SetDirty();

            // 文字色を変更する
            armamentMinisterIdTextBox.ForeColor = Color.Red;

            // コンボボックスの選択項目を変更する
            armamentMinisterComboBox.SelectedIndex =
                _armamentMinisterList.FindIndex(minister => minister.Id == settings.ArmamentMinister.Id);
        }

        /// <summary>
        ///     内務大臣のtypeテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterOfSecurityTypeTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(ministerOfSecurityTypeTextBox.Text, out val))
            {
                ministerOfSecurityTypeTextBox.Text = ((settings != null) && (settings.MinisterOfSecurity != null))
                    ? IntHelper.ToString(settings.MinisterOfSecurity.Type)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.MinisterOfSecurity != null) && (val == settings.MinisterOfSecurity.Type))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if ((settings != null) && (settings.MinisterOfSecurity != null) &&
                Scenarios.ExistsTypeId(val, settings.MinisterOfSecurity.Id))
            {
                ministerOfSecurityTypeTextBox.Text = IntHelper.ToString(settings.MinisterOfSecurity.Type);
                return;
            }

            Log.Info("[Scenario] minister of security type: {0} -> {1} ({2})",
                ((settings != null) && (settings.MinisterOfSecurity != null))
                    ? IntHelper.ToString(settings.MinisterOfSecurity.Type)
                    : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (settings.MinisterOfSecurity != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(settings.MinisterOfSecurity);

                // 値を更新する
                settings.MinisterOfSecurity.Type = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(settings.MinisterOfSecurity);
            }
            else
            {
                // 値を更新する
                settings.MinisterOfSecurity = Scenarios.GetNewTypeId(val, 1);

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.MinisterOfSecurityId);

                // 編集項目を更新する
                ministerOfSecurityIdTextBox.Text = IntHelper.ToString(settings.MinisterOfSecurity.Id);

                // 文字色を変更する
                ministerOfSecurityIdTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.MinisterOfSecurityType);
            Scenarios.SetDirty();

            // 文字色を変更する
            ministerOfSecurityTypeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     内務大臣のidテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterOfSecurityIdTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(ministerOfSecurityIdTextBox.Text, out val))
            {
                ministerOfSecurityIdTextBox.Text = ((settings != null) && (settings.MinisterOfSecurity != null))
                    ? IntHelper.ToString(settings.MinisterOfSecurity.Id)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.MinisterOfSecurity != null) && (val == settings.MinisterOfSecurity.Id))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if (Scenarios.ExistsTypeId(
                ((settings != null) && (settings.MinisterOfSecurity != null))
                    ? settings.MinisterOfSecurity.Type
                    : Scenarios.DefaultWarType, val))
            {
                ministerOfSecurityIdTextBox.Text = ((settings != null) && (settings.MinisterOfSecurity != null))
                    ? IntHelper.ToString(settings.MinisterOfSecurity.Id)
                    : "";
            }

            Log.Info("[Scenario] minister of security id: {0} -> {1} ({2})",
                ((settings != null) && (settings.MinisterOfSecurity != null))
                    ? IntHelper.ToString(settings.MinisterOfSecurity.Id)
                    : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (settings.MinisterOfSecurity != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(settings.MinisterOfSecurity);

                // 値を更新する
                settings.MinisterOfSecurity.Id = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(settings.MinisterOfSecurity);
            }
            else
            {
                // 値を更新する
                int type = (settings.MinisterOfSecurity != null) ? settings.MinisterOfSecurity.Type : 1;
                while (Scenarios.ExistsTypeId(type, val))
                {
                    type++;
                }
                settings.MinisterOfSecurity = Scenarios.GetNewTypeId(type, val);

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.MinisterOfSecurityType);

                // 編集項目を更新する
                ministerOfSecurityTypeTextBox.Text = IntHelper.ToString(type);

                // 文字色を変更する
                ministerOfSecurityTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.MinisterOfSecurityId);
            Scenarios.SetDirty();

            // 文字色を変更する
            ministerOfSecurityIdTextBox.ForeColor = Color.Red;

            // コンボボックスの選択項目を変更する
            ministerOfSecurityComboBox.SelectedIndex =
                _ministerOfSecurityList.FindIndex(minister => minister.Id == settings.MinisterOfSecurity.Id);
        }

        /// <summary>
        ///     情報大臣のtypeテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterOfIntelligenceTypeTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(ministerOfIntelligenceTypeTextBox.Text, out val))
            {
                ministerOfIntelligenceTypeTextBox.Text = ((settings != null) &&
                                                          (settings.MinisterOfIntelligence != null))
                    ? IntHelper.ToString(settings.MinisterOfIntelligence.Type)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.MinisterOfIntelligence != null) &&
                (val == settings.MinisterOfIntelligence.Type))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if ((settings != null) && (settings.MinisterOfIntelligence != null) &&
                Scenarios.ExistsTypeId(val, settings.MinisterOfIntelligence.Id))
            {
                ministerOfIntelligenceTypeTextBox.Text = IntHelper.ToString(settings.MinisterOfIntelligence.Type);
                return;
            }

            Log.Info("[Scenario] minister of intelligence type: {0} -> {1} ({2})",
                ((settings != null) && (settings.MinisterOfIntelligence != null))
                    ? IntHelper.ToString(settings.MinisterOfIntelligence.Type)
                    : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (settings.MinisterOfIntelligence != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(settings.MinisterOfIntelligence);

                // 値を更新する
                settings.MinisterOfIntelligence.Type = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(settings.MinisterOfIntelligence);
            }
            else
            {
                // 値を更新する
                settings.MinisterOfIntelligence = Scenarios.GetNewTypeId(val, 1);

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.MinisterOfIntelligenceId);

                // 編集項目を更新する
                ministerOfIntelligenceIdTextBox.Text = IntHelper.ToString(settings.MinisterOfIntelligence.Id);

                // 文字色を変更する
                ministerOfIntelligenceIdTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.MinisterOfIntelligenceType);
            Scenarios.SetDirty();

            // 文字色を変更する
            ministerOfIntelligenceTypeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     情報大臣のidテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterOfIntelligenceIdTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(ministerOfIntelligenceIdTextBox.Text, out val))
            {
                ministerOfIntelligenceIdTextBox.Text = ((settings != null) && (settings.MinisterOfIntelligence != null))
                    ? IntHelper.ToString(settings.MinisterOfIntelligence.Id)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.MinisterOfIntelligence != null) &&
                (val == settings.MinisterOfIntelligence.Id))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if (Scenarios.ExistsTypeId(
                ((settings != null) && (settings.MinisterOfIntelligence != null))
                    ? settings.MinisterOfIntelligence.Type
                    : Scenarios.DefaultWarType, val))
            {
                ministerOfIntelligenceIdTextBox.Text = ((settings != null) && (settings.MinisterOfIntelligence != null))
                    ? IntHelper.ToString(settings.MinisterOfIntelligence.Id)
                    : "";
            }

            Log.Info("[Scenario] minister of intelligence id: {0} -> {1} ({2})",
                ((settings != null) && (settings.MinisterOfIntelligence != null))
                    ? IntHelper.ToString(settings.MinisterOfIntelligence.Id)
                    : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (settings.MinisterOfIntelligence != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(settings.MinisterOfIntelligence);

                // 値を更新する
                settings.MinisterOfIntelligence.Id = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(settings.MinisterOfIntelligence);
            }
            else
            {
                // 値を更新する
                int type = (settings.MinisterOfIntelligence != null) ? settings.MinisterOfIntelligence.Type : 1;
                while (Scenarios.ExistsTypeId(type, val))
                {
                    type++;
                }
                settings.MinisterOfIntelligence = Scenarios.GetNewTypeId(type, val);

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.MinisterOfIntelligenceType);

                // 編集項目を更新する
                ministerOfIntelligenceTypeTextBox.Text = IntHelper.ToString(type);

                // 文字色を変更する
                ministerOfIntelligenceTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.MinisterOfIntelligenceId);
            Scenarios.SetDirty();

            // 文字色を変更する
            ministerOfIntelligenceIdTextBox.ForeColor = Color.Red;

            // コンボボックスの選択項目を変更する
            ministerOfIntelligenceComboBox.SelectedIndex =
                _ministerOfIntelligenceList.FindIndex(minister => minister.Id == settings.MinisterOfIntelligence.Id);
        }

        /// <summary>
        ///     統合参謀総長のtypeテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChiefOfStaffTypeTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(chiefOfStaffTypeTextBox.Text, out val))
            {
                chiefOfStaffTypeTextBox.Text = ((settings != null) && (settings.ChiefOfStaff != null))
                    ? IntHelper.ToString(settings.ChiefOfStaff.Type)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.ChiefOfStaff != null) && (val == settings.ChiefOfStaff.Type))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if ((settings != null) && (settings.ChiefOfStaff != null) &&
                Scenarios.ExistsTypeId(val, settings.ChiefOfStaff.Id))
            {
                chiefOfStaffTypeTextBox.Text = IntHelper.ToString(settings.ChiefOfStaff.Type);
                return;
            }

            Log.Info("[Scenario] chief of staff type: {0} -> {1} ({2})",
                ((settings != null) && (settings.ChiefOfStaff != null))
                    ? IntHelper.ToString(settings.ChiefOfStaff.Type)
                    : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (settings.ChiefOfStaff != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(settings.ChiefOfStaff);

                // 値を更新する
                settings.ChiefOfStaff.Type = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(settings.ChiefOfStaff);
            }
            else
            {
                // 値を更新する
                settings.ChiefOfStaff = Scenarios.GetNewTypeId(val, 1);

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.ChiefOfStaffId);

                // 編集項目を更新する
                chiefOfStaffIdTextBox.Text = IntHelper.ToString(settings.ChiefOfStaff.Id);

                // 文字色を変更する
                chiefOfStaffIdTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.ChiefOfStaffType);
            Scenarios.SetDirty();

            // 文字色を変更する
            chiefOfStaffTypeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     統合参謀総長のidテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChiefOfStaffIdTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(chiefOfStaffIdTextBox.Text, out val))
            {
                chiefOfStaffIdTextBox.Text = ((settings != null) && (settings.ChiefOfStaff != null))
                    ? IntHelper.ToString(settings.ChiefOfStaff.Id)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.ChiefOfStaff != null) && (val == settings.ChiefOfStaff.Id))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if (Scenarios.ExistsTypeId(
                ((settings != null) && (settings.ChiefOfStaff != null))
                    ? settings.ChiefOfStaff.Type
                    : Scenarios.DefaultWarType, val))
            {
                chiefOfStaffIdTextBox.Text = ((settings != null) && (settings.ChiefOfStaff != null))
                    ? IntHelper.ToString(settings.ChiefOfStaff.Id)
                    : "";
            }

            Log.Info("[Scenario] chief of staff id: {0} -> {1} ({2})",
                ((settings != null) && (settings.ChiefOfStaff != null))
                    ? IntHelper.ToString(settings.ChiefOfStaff.Id)
                    : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (settings.ChiefOfStaff != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(settings.ChiefOfStaff);

                // 値を更新する
                settings.ChiefOfStaff.Id = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(settings.ChiefOfStaff);
            }
            else
            {
                // 値を更新する
                int type = (settings.ChiefOfStaff != null) ? settings.ChiefOfStaff.Type : 1;
                while (Scenarios.ExistsTypeId(type, val))
                {
                    type++;
                }
                settings.ChiefOfStaff = Scenarios.GetNewTypeId(type, val);

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.ChiefOfStaffType);

                // 編集項目を更新する
                chiefOfStaffTypeTextBox.Text = IntHelper.ToString(type);

                // 文字色を変更する
                chiefOfStaffTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.ChiefOfStaffId);
            Scenarios.SetDirty();

            // 文字色を変更する
            chiefOfStaffIdTextBox.ForeColor = Color.Red;

            // コンボボックスの選択項目を変更する
            chiefOfStaffComboBox.SelectedIndex =
                _chiefOfStaffList.FindIndex(minister => minister.Id == settings.ChiefOfStaff.Id);
        }

        /// <summary>
        ///     陸軍総司令官のtypeテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChiefOfArmyTypeTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(chiefOfArmyTypeTextBox.Text, out val))
            {
                chiefOfArmyTypeTextBox.Text = ((settings != null) && (settings.ChiefOfArmy != null))
                    ? IntHelper.ToString(settings.ChiefOfArmy.Type)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.ChiefOfArmy != null) && (val == settings.ChiefOfArmy.Type))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if ((settings != null) && (settings.ChiefOfArmy != null) &&
                Scenarios.ExistsTypeId(val, settings.ChiefOfArmy.Id))
            {
                chiefOfArmyTypeTextBox.Text = IntHelper.ToString(settings.ChiefOfArmy.Type);
                return;
            }

            Log.Info("[Scenario] chief of army type: {0} -> {1} ({2})",
                ((settings != null) && (settings.ChiefOfArmy != null))
                    ? IntHelper.ToString(settings.ChiefOfArmy.Type)
                    : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (settings.ChiefOfArmy != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(settings.ChiefOfArmy);

                // 値を更新する
                settings.ChiefOfArmy.Type = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(settings.ChiefOfArmy);
            }
            else
            {
                // 値を更新する
                settings.ChiefOfArmy = Scenarios.GetNewTypeId(val, 1);

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.ChiefOfArmyId);

                // 編集項目を更新する
                chiefOfArmyIdTextBox.Text = IntHelper.ToString(settings.ChiefOfArmy.Id);

                // 文字色を変更する
                chiefOfArmyIdTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.ChiefOfArmyType);
            Scenarios.SetDirty();

            // 文字色を変更する
            chiefOfArmyTypeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     陸軍総司令官のidテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChiefOfArmyIdTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(chiefOfArmyIdTextBox.Text, out val))
            {
                chiefOfArmyIdTextBox.Text = ((settings != null) && (settings.ChiefOfArmy != null))
                    ? IntHelper.ToString(settings.ChiefOfArmy.Id)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.ChiefOfArmy != null) && (val == settings.ChiefOfArmy.Id))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if (Scenarios.ExistsTypeId(
                ((settings != null) && (settings.ChiefOfArmy != null))
                    ? settings.ChiefOfArmy.Type
                    : Scenarios.DefaultWarType, val))
            {
                chiefOfArmyIdTextBox.Text = ((settings != null) && (settings.ChiefOfArmy != null))
                    ? IntHelper.ToString(settings.ChiefOfArmy.Id)
                    : "";
            }

            Log.Info("[Scenario] chief of army id: {0} -> {1} ({2})",
                ((settings != null) && (settings.ChiefOfArmy != null))
                    ? IntHelper.ToString(settings.ChiefOfArmy.Id)
                    : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (settings.ChiefOfArmy != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(settings.ChiefOfArmy);

                // 値を更新する
                settings.ChiefOfArmy.Id = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(settings.ChiefOfArmy);
            }
            else
            {
                // 値を更新する
                int type = (settings.ChiefOfArmy != null) ? settings.ChiefOfArmy.Type : 1;
                while (Scenarios.ExistsTypeId(type, val))
                {
                    type++;
                }
                settings.ChiefOfArmy = Scenarios.GetNewTypeId(type, val);

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.ChiefOfArmyType);

                // 編集項目を更新する
                chiefOfArmyTypeTextBox.Text = IntHelper.ToString(type);

                // 文字色を変更する
                chiefOfArmyTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.ChiefOfArmyId);
            Scenarios.SetDirty();

            // 文字色を変更する
            chiefOfArmyIdTextBox.ForeColor = Color.Red;

            // コンボボックスの選択項目を変更する
            chiefOfArmyComboBox.SelectedIndex =
                _chiefOfArmyList.FindIndex(minister => minister.Id == settings.ChiefOfArmy.Id);
        }

        /// <summary>
        ///     海軍総司令官のtypeテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChiefOfNavyTypeTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(chiefOfNavyTypeTextBox.Text, out val))
            {
                chiefOfNavyTypeTextBox.Text = ((settings != null) && (settings.ChiefOfNavy != null))
                    ? IntHelper.ToString(settings.ChiefOfNavy.Type)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.ChiefOfNavy != null) && (val == settings.ChiefOfNavy.Type))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if ((settings != null) && (settings.ChiefOfNavy != null) &&
                Scenarios.ExistsTypeId(val, settings.ChiefOfNavy.Id))
            {
                chiefOfNavyTypeTextBox.Text = IntHelper.ToString(settings.ChiefOfNavy.Type);
                return;
            }

            Log.Info("[Scenario] chief of navy type: {0} -> {1} ({2})",
                ((settings != null) && (settings.ChiefOfNavy != null))
                    ? IntHelper.ToString(settings.ChiefOfNavy.Type)
                    : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (settings.ChiefOfNavy != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(settings.ChiefOfNavy);

                // 値を更新する
                settings.ChiefOfNavy.Type = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(settings.ChiefOfNavy);
            }
            else
            {
                // 値を更新する
                settings.ChiefOfNavy = Scenarios.GetNewTypeId(val, 1);

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.ChiefOfNavyId);

                // 編集項目を更新する
                chiefOfNavyIdTextBox.Text = IntHelper.ToString(settings.ChiefOfNavy.Id);

                // 文字色を変更する
                chiefOfNavyIdTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.ChiefOfNavyType);
            Scenarios.SetDirty();

            // 文字色を変更する
            chiefOfNavyTypeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     海軍総司令官のidテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChiefOfNavyIdTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(chiefOfNavyIdTextBox.Text, out val))
            {
                chiefOfNavyIdTextBox.Text = ((settings != null) && (settings.ChiefOfNavy != null))
                    ? IntHelper.ToString(settings.ChiefOfNavy.Id)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.ChiefOfNavy != null) && (val == settings.ChiefOfNavy.Id))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if (Scenarios.ExistsTypeId(
                ((settings != null) && (settings.ChiefOfNavy != null))
                    ? settings.ChiefOfNavy.Type
                    : Scenarios.DefaultWarType, val))
            {
                chiefOfNavyIdTextBox.Text = ((settings != null) && (settings.ChiefOfNavy != null))
                    ? IntHelper.ToString(settings.ChiefOfNavy.Id)
                    : "";
            }

            Log.Info("[Scenario] chief of navy id: {0} -> {1} ({2})",
                ((settings != null) && (settings.ChiefOfNavy != null))
                    ? IntHelper.ToString(settings.ChiefOfNavy.Id)
                    : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (settings.ChiefOfNavy != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(settings.ChiefOfNavy);

                // 値を更新する
                settings.ChiefOfNavy.Id = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(settings.ChiefOfNavy);
            }
            else
            {
                // 値を更新する
                int type = (settings.ChiefOfNavy != null) ? settings.ChiefOfNavy.Type : 1;
                while (Scenarios.ExistsTypeId(type, val))
                {
                    type++;
                }
                settings.ChiefOfNavy = Scenarios.GetNewTypeId(type, val);

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.ChiefOfNavyType);

                // 編集項目を更新する
                chiefOfNavyTypeTextBox.Text = IntHelper.ToString(type);

                // 文字色を変更する
                chiefOfNavyTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.ChiefOfNavyId);
            Scenarios.SetDirty();

            // 文字色を変更する
            chiefOfNavyIdTextBox.ForeColor = Color.Red;

            // コンボボックスの選択項目を変更する
            chiefOfNavyComboBox.SelectedIndex =
                _chiefOfNavyList.FindIndex(minister => minister.Id == settings.ChiefOfNavy.Id);
        }

        /// <summary>
        ///     空軍総司令官のtypeテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChiefOfAirTypeTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(chiefOfAirTypeTextBox.Text, out val))
            {
                chiefOfAirTypeTextBox.Text = ((settings != null) && (settings.ChiefOfAir != null))
                    ? IntHelper.ToString(settings.ChiefOfAir.Type)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.ChiefOfAir != null) && (val == settings.ChiefOfAir.Type))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if ((settings != null) && (settings.ChiefOfAir != null) &&
                Scenarios.ExistsTypeId(val, settings.ChiefOfAir.Id))
            {
                chiefOfAirTypeTextBox.Text = IntHelper.ToString(settings.ChiefOfAir.Type);
                return;
            }

            Log.Info("[Scenario] chief of air type: {0} -> {1} ({2})",
                ((settings != null) && (settings.ChiefOfAir != null))
                    ? IntHelper.ToString(settings.ChiefOfAir.Type)
                    : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (settings.ChiefOfAir != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(settings.ChiefOfAir);

                // 値を更新する
                settings.ChiefOfAir.Type = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(settings.ChiefOfAir);
            }
            else
            {
                // 値を更新する
                settings.ChiefOfAir = Scenarios.GetNewTypeId(val, 1);

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.ChiefOfAirId);

                // 編集項目を更新する
                chiefOfAirIdTextBox.Text = IntHelper.ToString(settings.ChiefOfAir.Id);

                // 文字色を変更する
                chiefOfAirIdTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.ChiefOfAirType);
            Scenarios.SetDirty();

            // 文字色を変更する
            chiefOfAirTypeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     空軍総司令官のidテキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChiefOfAirIdTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(chiefOfAirIdTextBox.Text, out val))
            {
                chiefOfAirIdTextBox.Text = ((settings != null) && (settings.ChiefOfAir != null))
                    ? IntHelper.ToString(settings.ChiefOfAir.Id)
                    : "";
                return;
            }

            // 値に変化がなければ何もしない
            if ((settings != null) && (settings.ChiefOfAir != null) && (val == settings.ChiefOfAir.Id))
            {
                return;
            }

            // 変更後のtypeとidの組が存在すれば元に戻す
            if (Scenarios.ExistsTypeId(
                ((settings != null) && (settings.ChiefOfAir != null))
                    ? settings.ChiefOfAir.Type
                    : Scenarios.DefaultWarType, val))
            {
                chiefOfAirIdTextBox.Text = ((settings != null) && (settings.ChiefOfAir != null))
                    ? IntHelper.ToString(settings.ChiefOfAir.Id)
                    : "";
            }

            Log.Info("[Scenario] chief of air id: {0} -> {1} ({2})",
                ((settings != null) && (settings.ChiefOfAir != null))
                    ? IntHelper.ToString(settings.ChiefOfAir.Id)
                    : "", val, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (settings.ChiefOfAir != null)
            {
                // 変更前のtypeとidの組を削除する
                Scenarios.RemoveTypeId(settings.ChiefOfAir);

                // 値を更新する
                settings.ChiefOfAir.Id = val;

                // 変更後のtypeとidの組を登録する
                Scenarios.AddTypeId(settings.ChiefOfAir);
            }
            else
            {
                // 値を更新する
                int type = (settings.ChiefOfAir != null) ? settings.ChiefOfAir.Type : 1;
                while (Scenarios.ExistsTypeId(type, val))
                {
                    type++;
                }
                settings.ChiefOfAir = Scenarios.GetNewTypeId(type, val);

                // 編集済みフラグを設定する
                settings.SetDirty(CountrySettingsItemId.ChiefOfAirType);

                // 編集項目を更新する
                chiefOfAirTypeTextBox.Text = IntHelper.ToString(type);

                // 文字色を変更する
                chiefOfAirTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            settings.SetDirty(CountrySettingsItemId.ChiefOfAirId);
            Scenarios.SetDirty();

            // 文字色を変更する
            chiefOfAirIdTextBox.ForeColor = Color.Red;

            // コンボボックスの選択項目を変更する
            chiefOfAirComboBox.SelectedIndex =
                _chiefOfAirList.FindIndex(minister => minister.Id == settings.ChiefOfAir.Id);
        }

        #endregion

        #endregion

        #region 技術タブ

        #region 技術タブ - 共通

        /// <summary>
        ///     技術タブの編集項目を初期化する
        /// </summary>
        private void InitTechTab()
        {
            // 技術カテゴリリスト
            techCategoryListBox.BeginUpdate();
            techCategoryListBox.Items.Clear();
            foreach (TechGroup grp in Techs.Groups)
            {
                techCategoryListBox.Items.Add(grp);
            }
            techCategoryListBox.SelectedIndex = 0;
            techCategoryListBox.EndUpdate();

            // 国家リストボックス
            techCountryListBox.BeginUpdate();
            techCountryListBox.Items.Clear();
            foreach (Country country in Countries.Tags)
            {
                techCountryListBox.Items.Add(Countries.GetTagName(country));
            }
            techCountryListBox.EndUpdate();
        }

        /// <summary>
        ///     技術タブの編集項目を更新する
        /// </summary>
        private void UpdateTechTab()
        {
            // 編集項目を無効化する
            DisableTechItems();

            // 技術カテゴリリストボックスを有効化する
            techCategoryListBox.Enabled = true;

            // 国家リストボックスを有効化する
            techCountryListBox.Enabled = true;
        }

        /// <summary>
        ///     技術の編集項目を無効化する
        /// </summary>
        private void DisableTechItems()
        {
            // 編集項目を無効化する
            ownedTechsLabel.Enabled = false;
            ownedTechsListView.Enabled = false;
            blueprintsLabel.Enabled = false;
            blueprintsListView.Enabled = false;
            inventionsLabel.Enabled = false;
            inventionsListView.Enabled = false;

            // 技術ツリーをクリアする
            _techTreePanel.Clear();

            // 編集項目をクリアする
            ownedTechsListView.Items.Clear();
            blueprintsListView.Items.Clear();
            inventionsListView.Items.Clear();
        }

        /// <summary>
        ///     技術の編集項目を更新する
        /// </summary>
        private void UpdateTechItems()
        {
            Country country = GetSelectedTechCountry();
            CountrySettings settings = Scenarios.GetCountrySettings(country);
            TechGroup grp = GetSelectedTechGroup();

            // 保有技術リスト/青写真リスト
            _techs = grp.Items.OfType<TechItem>().ToList();
            ownedTechsListView.ItemChecked -= OnOwnedTechsListViewItemChecked;
            blueprintsListView.ItemChecked -= OnBlueprintsListViewItemChecked;
            ownedTechsListView.BeginUpdate();
            blueprintsListView.BeginUpdate();
            ownedTechsListView.Items.Clear();
            blueprintsListView.Items.Clear();
            if (settings != null)
            {
                foreach (TechItem item in _techs)
                {
                    string name = item.ToString();
                    ownedTechsListView.Items.Add(new ListViewItem
                    {
                        Text = name,
                        Checked = settings.TechApps.Contains(item.Id),
                        ForeColor = settings.IsDirtyOwnedTech(item.Id) ? Color.Red : ownedTechsListView.ForeColor,
                        Tag = item
                    });
                    blueprintsListView.Items.Add(new ListViewItem
                    {
                        Text = name,
                        Checked = settings.BluePrints.Contains(item.Id),
                        ForeColor = settings.IsDirtyBlueprint(item.Id) ? Color.Red : ownedTechsListView.ForeColor,
                        Tag = item
                    });
                }
            }
            else
            {
                foreach (TechItem item in _techs)
                {
                    string name = item.ToString();
                    ownedTechsListView.Items.Add(new ListViewItem { Text = name, Tag = item });
                    blueprintsListView.Items.Add(new ListViewItem { Text = name, Tag = item });
                }
            }
            ownedTechsListView.EndUpdate();
            blueprintsListView.EndUpdate();
            ownedTechsListView.ItemChecked += OnOwnedTechsListViewItemChecked;
            blueprintsListView.ItemChecked += OnBlueprintsListViewItemChecked;

            // 発明イベントリスト
            _inventions = Techs.Groups.SelectMany(g => g.Items.OfType<TechEvent>()).ToList();
            inventionsListView.ItemChecked -= OnInveitionsListViewItemChecked;
            inventionsListView.BeginUpdate();
            inventionsListView.Items.Clear();
            if (settings != null)
            {
                foreach (TechEvent ev in _inventions)
                {
                    inventionsListView.Items.Add(new ListViewItem
                    {
                        Text = ev.ToString(),
                        Checked = settings.Inventions.Contains(ev.Id),
                        ForeColor = settings.IsDirtyInvention(ev.Id) ? Color.Red : inventionsListView.ForeColor,
                        Tag = ev
                    });
                }
            }
            else
            {
                foreach (TechEvent ev in _inventions)
                {
                    inventionsListView.Items.Add(new ListViewItem { Text = ev.ToString(), Tag = ev });
                }
            }
            inventionsListView.EndUpdate();
            inventionsListView.ItemChecked += OnInveitionsListViewItemChecked;

            // 技術ツリーを更新する
            _techTreePanel.Category = grp.Category;
            _techTreePanel.UpdateTechTree();

            // 編集項目を有効化する
            ownedTechsLabel.Enabled = true;
            ownedTechsListView.Enabled = true;
            blueprintsLabel.Enabled = true;
            blueprintsListView.Enabled = true;
            inventionsLabel.Enabled = true;
            inventionsListView.Enabled = true;
        }

        /// <summary>
        ///     技術タブ選択時の処理
        /// </summary>
        private void OnTechTabPageSelected()
        {
            // 技術データの読み込み完了まで待機する
            WaitLoadingTechs();
        }

        /// <summary>
        ///     技術カテゴリリストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechCategoryListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択中の国家がなければ編集項目を無効化する
            Country country = GetSelectedTechCountry();
            if (country == Country.None)
            {
                DisableTechItems();
                return;
            }

            // 編集項目を更新する
            UpdateTechItems();
        }

        /// <summary>
        ///     国家リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechCountryListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ編集項目を無効化する
            if (techCountryListBox.SelectedIndex < 0)
            {
                DisableTechItems();
                return;
            }

            // 編集項目を更新する
            UpdateTechItems();
        }

        /// <summary>
        ///     選択中の技術グループを取得する
        /// </summary>
        /// <returns>選択中の技術グループ</returns>
        private TechGroup GetSelectedTechGroup()
        {
            if (techCategoryListBox.SelectedIndex < 0)
            {
                return null;
            }
            return Techs.Groups[techCategoryListBox.SelectedIndex];
        }

        /// <summary>
        ///     選択中の国家を取得する
        /// </summary>
        /// <returns>選択中の国家</returns>
        private Country GetSelectedTechCountry()
        {
            if (techCountryListBox.SelectedIndex < 0)
            {
                return Country.None;
            }
            return Countries.Tags[techCountryListBox.SelectedIndex];
        }

        #endregion

        #region 技術タブ - 編集項目

        /// <summary>
        ///     保有技術リストビューのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOwnedTechsListViewItemChecked(object sender, ItemCheckedEventArgs e)
        {
            // 選択中の国家がなければ何もしない
            Country country = GetSelectedTechCountry();
            if (country == Country.None)
            {
                return;
            }

            TechItem item = e.Item.Tag as TechItem;
            if (item == null)
            {
                return;
            }
            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 値に変化がなければ何もしない
            bool val = e.Item.Checked;
            if ((settings != null) && (val == (settings.TechApps.Contains(item.Id))))
            {
                return;
            }

            Log.Info("[Scenario] owned techs: {0}{1} ({2})", val ? '+' : '-', item.Id, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            // 値を更新する
            if (val)
            {
                settings.TechApps.Add(item.Id);
            }
            else
            {
                settings.TechApps.Remove(item.Id);
            }

            // 編集済みフラグを設定する
            settings.SetDirtyOwnedTech(item.Id);
            Scenarios.SetDirty();

            // 文字色を変更する
            e.Item.ForeColor = Color.Red;

            // 技術ツリーの項目ラベルを更新する
            _techTreePanel.UpdateTechTreeItem(item);
        }

        /// <summary>
        ///     青写真リストビューのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBlueprintsListViewItemChecked(object sender, ItemCheckedEventArgs e)
        {
            // 選択中の国家がなければ何もしない
            Country country = GetSelectedTechCountry();
            if (country == Country.None)
            {
                return;
            }

            TechItem item = e.Item.Tag as TechItem;
            if (item == null)
            {
                return;
            }
            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 値に変化がなければ何もしない
            bool val = e.Item.Checked;
            if ((settings != null) && (val == (settings.BluePrints.Contains(item.Id))))
            {
                return;
            }

            Log.Info("[Scenario] blurprints: {0}{1} ({2})", val ? '+' : '-', item.Id, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            // 値を更新する
            if (val)
            {
                settings.BluePrints.Add(item.Id);
            }
            else
            {
                settings.BluePrints.Remove(item.Id);
            }

            // 編集済みフラグを設定する
            settings.SetDirtyBlueprint(item.Id);
            Scenarios.SetDirty();

            // 文字色を変更する
            e.Item.ForeColor = Color.Red;

            // 技術ツリーの項目ラベルを更新する
            _techTreePanel.UpdateTechTreeItem(item);
        }

        /// <summary>
        ///     発明イベントリストビューのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnInveitionsListViewItemChecked(object sender, ItemCheckedEventArgs e)
        {
            // 選択中の国家がなければ何もしない
            Country country = GetSelectedTechCountry();
            if (country == Country.None)
            {
                return;
            }

            TechEvent ev = e.Item.Tag as TechEvent;
            if (ev == null)
            {
                return;
            }
            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 値に変化がなければ何もしない
            bool val = e.Item.Checked;
            if ((settings != null) && (val == (settings.Inventions.Contains(ev.Id))))
            {
                return;
            }

            Log.Info("[Scenario] inventions: {0}{1} ({2})", val ? '+' : '-', ev.Id, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            // 値を更新する
            if (val)
            {
                settings.Inventions.Add(ev.Id);
            }
            else
            {
                settings.Inventions.Remove(ev.Id);
            }

            // 編集済みフラグを設定する
            settings.SetDirtyInvention(ev.Id);
            Scenarios.SetDirty();

            // 文字色を変更する
            e.Item.ForeColor = Color.Red;

            // 技術ツリーの項目ラベルを更新する
            _techTreePanel.UpdateTechTreeItem(ev);
        }

        #endregion

        #region 技術タブ - 技術ツリー

        /// <summary>
        ///     項目ラベルマウスクリック時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechTreeItemMouseClick(object sender, TechTreePanel.ItemMouseEventArgs e)
        {
            // 選択中の国家がなければ何もしない
            Country country = GetSelectedTechCountry();
            if (country == Country.None)
            {
                return;
            }

            TechItem tech = e.Item as TechItem;
            if (tech != null)
            {
                // 左クリックで保有技術の有無を切り替える
                if (e.MouseEvent.Button == MouseButtons.Left)
                {
                    ToggleOwnedTech(tech, country);
                }
                // 右クリックで青写真の有無を切り替える
                else if (e.MouseEvent.Button == MouseButtons.Right)
                {
                    ToggleBlueprint(tech, country);
                }
                return;
            }

            TechEvent ev = e.Item as TechEvent;
            if (ev != null)
            {
                // 左クリックで保有技術の有無を切り替える
                if (e.MouseEvent.Button == MouseButtons.Left)
                {
                    ToggleInvention(ev, country);
                }
            }
        }

        /// <summary>
        ///     保有技術の有無を切り替える
        /// </summary>
        /// <param name="item">対象技術</param>
        /// <param name="country">対象国</param>
        private void ToggleOwnedTech(TechItem item, Country country)
        {
            CountrySettings settings = Scenarios.GetCountrySettings(country);
            bool val = (settings == null) || !settings.TechApps.Contains(item.Id);

            Log.Info("[Scenario] owned techs: {0}{1} ({2})", val ? '+' : '-', item.Id, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            // 値を更新する
            if (val)
            {
                settings.TechApps.Add(item.Id);
            }
            else
            {
                settings.TechApps.Remove(item.Id);
            }

            // 編集済みフラグを設定する
            settings.SetDirtyOwnedTech(item.Id);
            Scenarios.SetDirty();

            // 技術ツリーの項目ラベルを更新する
            _techTreePanel.UpdateTechTreeItem(item);

            // 保有技術リストビューの表示を更新する
            int index = _techs.IndexOf(item);
            if (index >= 0)
            {
                ListViewItem li = ownedTechsListView.Items[index];
                li.Checked = val;
                li.ForeColor = Color.Red;
                li.EnsureVisible();
            }
        }

        /// <summary>
        ///     保有技術の有無を切り替える
        /// </summary>
        /// <param name="item">対象技術</param>
        /// <param name="country">対象国</param>
        private void ToggleBlueprint(TechItem item, Country country)
        {
            CountrySettings settings = Scenarios.GetCountrySettings(country);
            bool val = (settings == null) || !settings.BluePrints.Contains(item.Id);

            Log.Info("[Scenario] blueprints: {0}{1} ({2})", val ? '+' : '-', item.Id, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (val)
            {
                settings.BluePrints.Add(item.Id);
            }
            else
            {
                settings.BluePrints.Remove(item.Id);
            }

            // 編集済みフラグを設定する
            settings.SetDirtyBlueprint(item.Id);
            Scenarios.SetDirty();

            // 技術ツリーの項目ラベルを更新する
            _techTreePanel.UpdateTechTreeItem(item);

            // 保有技術リストビューの表示を更新する
            int index = _techs.IndexOf(item);
            if (index >= 0)
            {
                ListViewItem li = blueprintsListView.Items[index];
                li.Checked = val;
                li.ForeColor = Color.Red;
                li.EnsureVisible();
            }
        }

        /// <summary>
        ///     発明イベントの有無を切り替える
        /// </summary>
        /// <param name="item">対象発明イベント</param>
        /// <param name="country">対象国</param>
        private void ToggleInvention(TechEvent item, Country country)
        {
            CountrySettings settings = Scenarios.GetCountrySettings(country);
            bool val = (settings == null) || !settings.Inventions.Contains(item.Id);

            Log.Info("[Scenario] inventions: {0}{1} ({2})", val ? '+' : '-', item.Id, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            if (val)
            {
                settings.Inventions.Add(item.Id);
            }
            else
            {
                settings.Inventions.Remove(item.Id);
            }

            // 編集済みフラグを設定する
            settings.SetDirtyInvention(item.Id);
            Scenarios.SetDirty();

            // 技術ツリーの項目ラベルを更新する
            _techTreePanel.UpdateTechTreeItem(item);

            // 保有技術リストビューの表示を更新する
            int index = _inventions.IndexOf(item);
            if (index >= 0)
            {
                ListViewItem li = inventionsListView.Items[index];
                li.Checked = val;
                li.ForeColor = Color.Red;
                li.EnsureVisible();
            }
        }

        /// <summary>
        ///     技術項目の状態を返す
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnQueryTechTreeItemStatus(object sender, TechTreePanel.QueryItemStatusEventArgs e)
        {
            // 選択中の国家がなければ何もしない
            Country country = GetSelectedTechCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);
            if (settings == null)
            {
                return;
            }

            TechItem tech = e.Item as TechItem;
            if (tech != null)
            {
                e.Done = settings.TechApps.Contains(tech.Id);
                e.Blueprint = settings.BluePrints.Contains(tech.Id);
                return;
            }

            TechEvent ev = e.Item as TechEvent;
            if (ev != null)
            {
                e.Done = settings.Inventions.Contains(ev.Id);
            }
        }

        #endregion

        #endregion

        #region 汎用

        /// <summary>
        ///     国家リストボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryListBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index < 0)
            {
                return;
            }
            ListBox listBox = sender as ListBox;
            if (listBox == null)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目を描画する
            Brush brush;
            if ((e.State & DrawItemState.Selected) == 0)
            {
                // 変更ありの項目は文字色を変更する
                CountrySettings settings = Scenarios.GetCountrySettings(Countries.Tags[e.Index]);
                brush = new SolidBrush((settings != null)
                    ? (settings.IsDirty() ? Color.Red : listBox.ForeColor)
                    : Color.LightGray);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            string s = listBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        #endregion

        private void OnTextBox1Validated(object sender, EventArgs e)
        {
            ushort id;
            if (!ushort.TryParse(testProvinceIdTextBox.Text, out id))
            {
                return;
            }
            if (id > 0 && id < 10000)
            {
                Map map = Maps.Data[(int) MapLevel.Level1];
                Bitmap bitmap = provinceMapPictureBox.Image as Bitmap;
                if (_prevId != 0)
                {
                    map.ResetProvinceMask(bitmap, _prevId);
                }
                map.SetProvinceMask(bitmap, id);
                provinceMapPictureBox.Refresh();

                _prevId = id;
            }
        }
    }
}