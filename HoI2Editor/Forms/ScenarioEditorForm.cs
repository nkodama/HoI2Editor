using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HoI2Editor.Controller;
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

        #region 共通

        /// <summary>
        ///     シナリオエディタのコントローラ
        /// </summary>
        private ScenarioEditorController _controller;

        /// <summary>
        ///     タブページ番号
        /// </summary>
        private TabPageNo _tabPageNo;

        /// <summary>
        ///     タブページの初期化フラグ
        /// </summary>
        private readonly bool[] _tabPageInitialized = new bool[Enum.GetValues(typeof (TabPageNo)).Length];

        /// <summary>
        ///     編集項目IDとコントロールの関連付け
        /// </summary>
        private readonly Dictionary<ScenarioEditorItemId, Control> _itemControls =
            new Dictionary<ScenarioEditorItemId, Control>();

        #endregion

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

        #region 技術ツリーパネル

        /// <summary>
        ///     技術ツリーパネルのコントローラ
        /// </summary>
        private TechTreePanelController _techTreePanelController;

        #endregion

        #region マップパネル

        /// <summary>
        ///     マップパネルのコントローラ
        /// </summary>
        private MapPanelController _mapPanelController;

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
        ///     プロヴィンスデータロード用
        /// </summary>
        private readonly BackgroundWorker _provinceWorker = new BackgroundWorker();

        /// <summary>
        ///     マップデータロード用
        /// </summary>
        private readonly BackgroundWorker _mapWorker = new BackgroundWorker();

        #endregion

        #endregion

        #region 内部定数

        /// <summary>
        ///     タブページ番号
        /// </summary>
        private enum TabPageNo
        {
            Main, // メイン
            Alliance, // 同盟
            Relation, // 関係
            Trade, // 貿易
            Country, // 国家
            Government, // 政府
            Technology, // 技術
            Province // プロヴィンス
        }

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
            // シナリオ関連情報を初期化する
            Scenarios.Init();

            // 編集項目を更新する
            UpdateEditableItems();
            OnTradeTabPageFileLoad();
            OnCountryTabPageFileLoad();
            OnGovernmentTabPageFileLoad();
            OnTechTabPageFileLoad();
            OnProvinceTabPageFileLoad();
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
        private void DelayLoadMinisters()
        {
            _ministerWorker.DoWork += OnMinisterWorkerDoWork;
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
        private static void OnMinisterWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            // 閣僚データを読み込む
            Ministers.Load();

            Log.Info("[Scenario] Load ministers");
        }

        /// <summary>
        ///     技術データを遅延読み込みする
        /// </summary>
        private void DelayLoadTechs()
        {
            _techWorker.DoWork += OnTechWorkerDoWork;
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
        private static void OnTechWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            // 技術定義ファイルを読み込む
            Techs.Load();

            Log.Info("[Scenario] Load techs");
        }

        /// <summary>
        ///     プロヴィンスデータを遅延読み込みする
        /// </summary>
        private void DelayLoadProvinces()
        {
            _provinceWorker.DoWork += OnProvinceWorkerDoWork;
            _provinceWorker.RunWorkerAsync();
        }

        /// <summary>
        ///     プロヴィンスデータの読み込み完了まで待機する
        /// </summary>
        private void WaitLoadingProvinces()
        {
            while (_provinceWorker.IsBusy)
            {
                Application.DoEvents();
            }
        }

        /// <summary>
        ///     プロヴィンスデータを読み込む
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnProvinceWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            // プロヴィンス定義ファイルを読み込む
            Provinces.Load();

            Log.Info("[Scenario] Load provinces");
        }

        /// <summary>
        ///     マップを遅延読み込みする
        /// </summary>
        private void DelayLoadMaps()
        {
            _mapWorker.DoWork += OnMapWorkerDoWork;
            _mapWorker.RunWorkerCompleted += OnMapWorkerRunWorkerCompleted;
            _mapWorker.RunWorkerAsync();
        }

        /// <summary>
        ///     マップを読み込む
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnMapWorkerDoWork(object sender, DoWorkEventArgs e)
        {
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

            // マップパネルを初期化する
            InitMapPanel();

            // マップフィルターを有効化する
            EnableMapFilter();

            // 選択プロヴィンスが表示されるようにスクロールする
            Province province = GetSelectedProvince();
            if (province != null)
            {
                _mapPanelController.ScrollToProvince(province.Id);
            }
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
            _techTreePanelController = new TechTreePanelController(techTreePictureBox) { ApplyItemStatus = true };
            _techTreePanelController.ItemMouseClick += OnTechTreeItemMouseClick;
            _techTreePanelController.QueryItemStatus += OnQueryTechTreeItemStatus;

            // マップパネル
            _mapPanelController = new MapPanelController(provinceMapPanel, provinceMapPictureBox);

            // コントローラ
            _controller = new ScenarioEditorController(this, _mapPanelController);
        }

        /// <summary>
        ///     編集項目を初期化する
        /// </summary>
        private void InitEditableItems()
        {
            InitMainTab();
            InitAllianceTab();
            InitRelationTab();
        }

        /// <summary>
        ///     編集項目を更新する
        /// </summary>
        private void UpdateEditableItems()
        {
            UpdateMainTab();
            UpdateAllianceTab();
            UpdateRelationTab();
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

            // 閣僚特性を初期化する
            Ministers.InitPersonality();

            // ユニットデータを初期化する
            Units.Init();

            // プロヴィンスデータを初期化する
            Provinces.Init();

            // ゲーム設定ファイルを読み込む
            Misc.Load();

            // 文字列定義ファイルを読み込む
            Config.Load();

            // マップを遅延読み込みする
            DelayLoadMaps();

            // 閣僚データを遅延読込する
            DelayLoadMinisters();

            // 技術データを遅延読み込みする
            DelayLoadTechs();

            // プロヴィンスデータを遅延読み込みする
            DelayLoadProvinces();

            // 表示項目を初期化する
            InitEditableItems();
            OnTradeTabPageFormLoad();
            OnCountryTabPageFormLoad();
            OnGovernmentTabPageFormLoad();
            OnTechTabPageFormLoad();
            OnProvinceTabPageFormLoad();

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
            _tabPageNo = (TabPageNo) scenarioTabControl.SelectedIndex;

            switch (_tabPageNo)
            {
                case TabPageNo.Trade:
                    OnTradeTabPageSelected();
                    break;

                case TabPageNo.Country:
                    OnCountryTabPageSelected();
                    break;

                case TabPageNo.Government:
                    OnGovernmentTabPageSelected();
                    break;

                case TabPageNo.Technology:
                    OnTechTabPageSelected();
                    break;

                case TabPageNo.Province:
                    OnProvinceTabPageSelected();
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
            scenarioNameTextBox.ForeColor = scenario.IsDirty(Scenario.ItemId.Name) ? Color.Red : SystemColors.WindowText;
            panelImageTextBox.Text = scenario.PanelName;
            panelImageTextBox.ForeColor = scenario.IsDirty(Scenario.ItemId.PanelName)
                ? Color.Red
                : SystemColors.WindowText;
            UpdatePanelImage(scenario.PanelName);

            bool flag = (data.StartDate != null);
            startYearTextBox.Text = flag ? IntHelper.ToString(data.StartDate.Year) : "";
            startMonthTextBox.Text = flag ? IntHelper.ToString(data.StartDate.Month) : "";
            startDayTextBox.Text = flag ? IntHelper.ToString(data.StartDate.Day) : "";

            startYearTextBox.ForeColor = scenario.IsDirty(Scenario.ItemId.StartYear)
                ? Color.Red
                : SystemColors.WindowText;
            startMonthTextBox.ForeColor = scenario.IsDirty(Scenario.ItemId.StartMonth)
                ? Color.Red
                : SystemColors.WindowText;
            startDayTextBox.ForeColor = scenario.IsDirty(Scenario.ItemId.StartDay) ? Color.Red : SystemColors.WindowText;

            flag = (data.EndDate != null);
            endYearTextBox.Text = flag ? IntHelper.ToString(data.EndDate.Year) : "";
            endMonthTextBox.Text = flag ? IntHelper.ToString(data.EndDate.Month) : "";
            endDayTextBox.Text = flag ? IntHelper.ToString(data.EndDate.Day) : "";

            endYearTextBox.ForeColor = scenario.IsDirty(Scenario.ItemId.EndYear) ? Color.Red : SystemColors.WindowText;
            endMonthTextBox.ForeColor = scenario.IsDirty(Scenario.ItemId.EndMonth) ? Color.Red : SystemColors.WindowText;
            endDayTextBox.ForeColor = scenario.IsDirty(Scenario.ItemId.EndDay) ? Color.Red : SystemColors.WindowText;

            includeFolderTextBox.Text = scenario.IncludeFolder;
            includeFolderTextBox.ForeColor = scenario.IsDirty(Scenario.ItemId.IncludeFolder)
                ? Color.Red
                : SystemColors.WindowText;

            battleScenarioCheckBox.Checked = header.IsCombatScenario;
            battleScenarioCheckBox.ForeColor = scenario.IsDirty(Scenario.ItemId.BattleScenario)
                ? Color.Red
                : SystemColors.WindowText;
            freeCountryCheckBox.Checked = header.IsFreeSelection;
            freeCountryCheckBox.ForeColor = scenario.IsDirty(Scenario.ItemId.FreeSelection)
                ? Color.Red
                : SystemColors.WindowText;

            flag = (data.Rules == null);
            allowDiplomacyCheckBox.Checked = (flag || data.Rules.AllowDiplomacy);
            allowDiplomacyCheckBox.ForeColor = scenario.IsDirty(Scenario.ItemId.AllowDiplomacy)
                ? Color.Red
                : SystemColors.WindowText;
            allowProductionCheckBox.Checked = (flag || data.Rules.AllowProduction);
            allowProductionCheckBox.ForeColor = scenario.IsDirty(Scenario.ItemId.AllowProduction)
                ? Color.Red
                : SystemColors.WindowText;
            allowTechnologyCheckBox.Checked = (flag || data.Rules.AllowTechnology);
            allowTechnologyCheckBox.ForeColor = scenario.IsDirty(Scenario.ItemId.AllowTechnology)
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
            scenario.SetDirty(Scenario.ItemId.Name);

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
            scenario.SetDirty(Scenario.ItemId.PanelName);
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
                scenario.SetDirty(Scenario.ItemId.StartMonth);
                scenario.SetDirty(Scenario.ItemId.StartDay);

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
            scenario.SetDirty(Scenario.ItemId.StartYear);
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
                scenario.SetDirty(Scenario.ItemId.StartYear);
                scenario.SetDirty(Scenario.ItemId.StartDay);

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
            scenario.SetDirty(Scenario.ItemId.StartMonth);
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
                scenario.SetDirty(Scenario.ItemId.StartYear);
                scenario.SetDirty(Scenario.ItemId.StartMonth);

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
            scenario.SetDirty(Scenario.ItemId.StartDay);
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
                scenario.SetDirty(Scenario.ItemId.EndMonth);
                scenario.SetDirty(Scenario.ItemId.EndDay);

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
            scenario.SetDirty(Scenario.ItemId.EndYear);
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
                scenario.SetDirty(Scenario.ItemId.EndYear);
                scenario.SetDirty(Scenario.ItemId.EndDay);

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
            scenario.SetDirty(Scenario.ItemId.EndMonth);
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
                scenario.SetDirty(Scenario.ItemId.EndYear);
                scenario.SetDirty(Scenario.ItemId.EndMonth);

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
            scenario.SetDirty(Scenario.ItemId.EndDay);
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
            scenario.SetDirty(Scenario.ItemId.IncludeFolder);
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
            bool dirty = ((e.Index == header.AiAggressive) && Scenarios.Data.IsDirty(Scenario.ItemId.AiAggressive));
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
            bool dirty = ((e.Index == header.Difficulty) && Scenarios.Data.IsDirty(Scenario.ItemId.Difficulty));
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
            bool dirty = ((e.Index == header.GameSpeed) && Scenarios.Data.IsDirty(Scenario.ItemId.GameSpeed));
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
            scenario.SetDirty(Scenario.ItemId.BattleScenario);

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
            scenario.SetDirty(Scenario.ItemId.FreeSelection);

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
            scenario.SetDirty(Scenario.ItemId.AllowDiplomacy);

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
            scenario.SetDirty(Scenario.ItemId.AllowProduction);

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
            scenario.SetDirty(Scenario.ItemId.AllowTechnology);

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
            scenario.SetDirty(Scenario.ItemId.AiAggressive);

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
            scenario.SetDirty(Scenario.ItemId.Difficulty);

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
            scenario.SetDirty(Scenario.ItemId.GameSpeed);

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
            countryDescTextBox.ForeColor = major.IsDirty(MajorCountrySettings.ItemId.Desc)
                ? Color.Red
                : SystemColors.WindowText;

            propagandaPictureBox.Text = major.PictureName;
            propagandaPictureBox.ForeColor = major.IsDirty(MajorCountrySettings.ItemId.PictureName)
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
            major.SetDirty(MajorCountrySettings.ItemId.PictureName);
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
            major.SetDirty(MajorCountrySettings.ItemId.Desc);
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
        private static void InitAllianceTab()
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
            allianceNameTextBox.ForeColor = alliance.IsDirty(Alliance.ItemId.Name)
                ? Color.Red
                : SystemColors.WindowText;

            // 同盟ID
            bool flag = (alliance.Id != null);
            allianceTypeTextBox.Text = flag ? IntHelper.ToString(alliance.Id.Type) : "";
            allianceIdTextBox.Text = flag ? IntHelper.ToString(alliance.Id.Id) : "";

            allianceTypeTextBox.ForeColor = alliance.IsDirty(Alliance.ItemId.Type) ? Color.Red : SystemColors.WindowText;
            allianceIdTextBox.ForeColor = alliance.IsDirty(Alliance.ItemId.Id) ? Color.Red : SystemColors.WindowText;

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
            alliance.SetDirty(Alliance.ItemId.Type);
            alliance.SetDirty(Alliance.ItemId.Id);
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
            alliance.SetDirty(Alliance.ItemId.Name);
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
                alliance.SetDirty(Alliance.ItemId.Id);

                // 編集項目を更新する
                allianceIdTextBox.Text = IntHelper.ToString(alliance.Id.Id);

                // 文字色を変更する
                allianceIdTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            alliance.SetDirty(Alliance.ItemId.Type);
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
                alliance.SetDirty(Alliance.ItemId.Type);

                // 編集項目を更新する
                allianceTypeTextBox.Text = IntHelper.ToString(alliance.Id.Type);

                // 文字色を変更する
                allianceTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            alliance.SetDirty(Alliance.ItemId.Id);
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

            warStartYearTextBox.ForeColor = war.IsDirty(War.ItemId.StartYear) ? Color.Red : SystemColors.WindowText;
            warStartMonthTextBox.ForeColor = war.IsDirty(War.ItemId.StartMonth) ? Color.Red : SystemColors.WindowText;
            warStartDayTextBox.ForeColor = war.IsDirty(War.ItemId.StartDay) ? Color.Red : SystemColors.WindowText;

            // 終了日時
            flag = (war.EndDate != null);
            warEndYearTextBox.Text = flag ? IntHelper.ToString(war.EndDate.Year) : "";
            warEndMonthTextBox.Text = flag ? IntHelper.ToString(war.EndDate.Month) : "";
            warEndDayTextBox.Text = flag ? IntHelper.ToString(war.EndDate.Day) : "";

            warEndYearTextBox.ForeColor = war.IsDirty(War.ItemId.EndYear) ? Color.Red : SystemColors.WindowText;
            warEndMonthTextBox.ForeColor = war.IsDirty(War.ItemId.EndMonth) ? Color.Red : SystemColors.WindowText;
            warEndDayTextBox.ForeColor = war.IsDirty(War.ItemId.EndDay) ? Color.Red : SystemColors.WindowText;

            // 戦争ID
            flag = (war.Id != null);
            warTypeTextBox.Text = flag ? IntHelper.ToString(war.Id.Type) : "";
            warIdTextBox.Text = flag ? IntHelper.ToString(war.Id.Id) : "";

            warTypeTextBox.ForeColor = war.IsDirty(War.ItemId.Type) ? Color.Red : SystemColors.WindowText;
            warIdTextBox.ForeColor = war.IsDirty(War.ItemId.Id) ? Color.Red : SystemColors.WindowText;

            // 攻撃側ID
            flag = ((war.Attackers != null) && (war.Attackers.Id != null));
            warAttackerTypeTextBox.Text = flag ? IntHelper.ToString(war.Attackers.Id.Type) : "";
            warAttackerIdTextBox.Text = flag ? IntHelper.ToString(war.Attackers.Id.Id) : "";

            warAttackerTypeTextBox.ForeColor = war.IsDirty(War.ItemId.AttackerType)
                ? Color.Red
                : SystemColors.WindowText;
            warAttackerIdTextBox.ForeColor = war.IsDirty(War.ItemId.AttackerId) ? Color.Red : SystemColors.WindowText;

            // 防御側ID
            flag = ((war.Defenders != null) && (war.Defenders.Id != null));
            warDefenderTypeTextBox.Text = flag ? IntHelper.ToString(war.Defenders.Id.Type) : "";
            warDefenderIdTextBox.Text = flag ? IntHelper.ToString(war.Defenders.Id.Id) : "";
            warDefenderTypeTextBox.ForeColor = war.IsDirty(War.ItemId.DefenderType)
                ? Color.Red
                : SystemColors.WindowText;
            warDefenderIdTextBox.ForeColor = war.IsDirty(War.ItemId.DefenderId) ? Color.Red : SystemColors.WindowText;

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
            war.SetDirty(War.ItemId.StartYear);
            war.SetDirty(War.ItemId.StartMonth);
            war.SetDirty(War.ItemId.StartDay);
            war.SetDirty(War.ItemId.EndYear);
            war.SetDirty(War.ItemId.EndMonth);
            war.SetDirty(War.ItemId.EndDay);
            war.SetDirty(War.ItemId.Type);
            war.SetDirty(War.ItemId.Id);
            war.SetDirty(War.ItemId.AttackerType);
            war.SetDirty(War.ItemId.AttackerId);
            war.SetDirty(War.ItemId.DefenderType);
            war.SetDirty(War.ItemId.DefenderId);
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
                war.SetDirty(War.ItemId.StartMonth);
                war.SetDirty(War.ItemId.StartDay);

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
            war.SetDirty(War.ItemId.StartYear);
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
                war.SetDirty(War.ItemId.StartYear);
                war.SetDirty(War.ItemId.StartDay);

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
            war.SetDirty(War.ItemId.StartMonth);
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
                war.SetDirty(War.ItemId.StartYear);
                war.SetDirty(War.ItemId.StartMonth);

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
            war.SetDirty(War.ItemId.StartDay);
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
                war.SetDirty(War.ItemId.EndMonth);
                war.SetDirty(War.ItemId.EndDay);

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
            war.SetDirty(War.ItemId.EndYear);
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
                war.SetDirty(War.ItemId.EndYear);
                war.SetDirty(War.ItemId.EndDay);

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
            war.SetDirty(War.ItemId.EndMonth);
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
                war.SetDirty(War.ItemId.EndYear);
                war.SetDirty(War.ItemId.EndMonth);

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
            war.SetDirty(War.ItemId.EndDay);
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
                war.SetDirty(War.ItemId.Id);

                // 編集項目を更新する
                warIdTextBox.Text = IntHelper.ToString(war.Id.Id);

                // 文字色を変更する
                warIdTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            war.SetDirty(War.ItemId.Type);
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
                war.SetDirty(War.ItemId.Type);

                // 編集項目を更新する
                warTypeTextBox.Text = IntHelper.ToString(war.Id.Type);

                // 文字色を変更する
                warTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            war.SetDirty(War.ItemId.Id);
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
                war.SetDirty(War.ItemId.AttackerId);

                // 編集項目を更新する
                warAttackerIdTextBox.Text = IntHelper.ToString(war.Attackers.Id.Id);

                // 文字色を変更する
                warAttackerIdTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            war.SetDirty(War.ItemId.AttackerType);
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
                war.SetDirty(War.ItemId.AttackerType);

                // 編集項目を更新する
                warAttackerTypeTextBox.Text = IntHelper.ToString(war.Attackers.Id.Type);

                // 文字色を変更する
                warAttackerTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            war.SetDirty(War.ItemId.AttackerId);
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
                war.SetDirty(War.ItemId.DefenderId);

                // 編集項目を更新する
                warDefenderIdTextBox.Text = IntHelper.ToString(war.Defenders.Id.Id);

                // 文字色を変更する
                warDefenderIdTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            war.SetDirty(War.ItemId.DefenderType);
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
                war.SetDirty(War.ItemId.DefenderType);

                // 編集項目を更新する
                warDefenderTypeTextBox.Text = IntHelper.ToString(war.Defenders.Id.Type);

                // 文字色を変更する
                warDefenderTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            war.SetDirty(War.ItemId.DefenderId);
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
            relationValueNumericUpDown.ForeColor = (flag && relation.IsDirty(Relation.ItemId.Value))
                ? Color.Red
                : SystemColors.WindowText;

            flag = (settings != null);
            masterCheckBox.Checked = flag && (settings.Master == target);
            controlCheckBox.Checked = flag && (settings.Control == target);
            masterCheckBox.ForeColor = (flag && settings.IsDirty(CountrySettings.ItemId.Master))
                ? Color.Red
                : SystemColors.WindowText;
            controlCheckBox.ForeColor = (flag && settings.IsDirty(CountrySettings.ItemId.Control))
                ? Color.Red
                : SystemColors.WindowText;

            flag = (relation != null);
            accessCheckBox.Checked = flag && relation.Access;

            accessCheckBox.ForeColor = (flag && relation.IsDirty(Relation.ItemId.Access))
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

            guaranteeCheckBox.ForeColor = ((relation != null) && relation.IsDirty(Relation.ItemId.Guaranteed))
                ? Color.Red
                : SystemColors.WindowText;
            guaranteeYearTextBox.ForeColor = (flag && relation.IsDirty(Relation.ItemId.GuaranteedYear))
                ? Color.Red
                : SystemColors.WindowText;
            guaranteeMonthTextBox.ForeColor = (flag && relation.IsDirty(Relation.ItemId.GuaranteedMonth))
                ? Color.Red
                : SystemColors.WindowText;
            guaranteeDayTextBox.ForeColor = (flag && relation.IsDirty(Relation.ItemId.GuaranteedDay))
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

            nonAggressionStartYearTextBox.ForeColor = (flag && nonAggression.IsDirty(Treaty.ItemId.StartYear))
                ? Color.Red
                : SystemColors.WindowText;
            nonAggressionStartMonthTextBox.ForeColor = (flag && nonAggression.IsDirty(Treaty.ItemId.StartMonth))
                ? Color.Red
                : SystemColors.WindowText;
            nonAggressionStartDayTextBox.ForeColor = (flag && nonAggression.IsDirty(Treaty.ItemId.StartDay))
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

            nonAggressionEndYearTextBox.ForeColor = (flag && nonAggression.IsDirty(Treaty.ItemId.EndYear))
                ? Color.Red
                : SystemColors.WindowText;
            nonAggressionEndMonthTextBox.ForeColor = (flag && nonAggression.IsDirty(Treaty.ItemId.EndMonth))
                ? Color.Red
                : SystemColors.WindowText;
            nonAggressionEndDayTextBox.ForeColor = (flag && nonAggression.IsDirty(Treaty.ItemId.EndDay))
                ? Color.Red
                : SystemColors.WindowText;

            nonAggressionEndLabel.Enabled = flag;
            nonAggressionEndYearTextBox.Enabled = flag;
            nonAggressionEndMonthTextBox.Enabled = flag;
            nonAggressionEndDayTextBox.Enabled = flag;

            flag = (nonAggression != null) && (nonAggression.Id != null);
            nonAggressionTypeTextBox.Text = flag ? IntHelper.ToString(nonAggression.Id.Type) : "";
            nonAggressionIdTextBox.Text = flag ? IntHelper.ToString(nonAggression.Id.Id) : "";

            nonAggressionTypeTextBox.ForeColor = (flag && nonAggression.IsDirty(Treaty.ItemId.Type))
                ? Color.Red
                : SystemColors.WindowText;
            nonAggressionIdTextBox.ForeColor = (flag && nonAggression.IsDirty(Treaty.ItemId.Id))
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

            peaceStartYearTextBox.ForeColor = (flag && peace.IsDirty(Treaty.ItemId.StartYear))
                ? Color.Red
                : SystemColors.WindowText;
            peaceStartMonthTextBox.ForeColor = (flag && peace.IsDirty(Treaty.ItemId.StartMonth))
                ? Color.Red
                : SystemColors.WindowText;
            peaceStartDayTextBox.ForeColor = (flag && peace.IsDirty(Treaty.ItemId.StartDay))
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

            peaceEndYearTextBox.ForeColor = (flag && peace.IsDirty(Treaty.ItemId.EndYear))
                ? Color.Red
                : SystemColors.WindowText;
            peaceEndMonthTextBox.ForeColor = (flag && peace.IsDirty(Treaty.ItemId.EndMonth))
                ? Color.Red
                : SystemColors.WindowText;
            peaceEndDayTextBox.ForeColor = (flag && peace.IsDirty(Treaty.ItemId.EndDay))
                ? Color.Red
                : SystemColors.WindowText;

            peaceEndLabel.Enabled = flag;
            peaceEndYearTextBox.Enabled = flag;
            peaceEndMonthTextBox.Enabled = flag;
            peaceEndDayTextBox.Enabled = flag;

            flag = (peace != null) && (peace.Id != null);
            peaceTypeTextBox.Text = flag ? IntHelper.ToString(peace.Id.Type) : "";
            peaceIdTextBox.Text = flag ? IntHelper.ToString(peace.Id.Id) : "";

            peaceTypeTextBox.ForeColor = (flag && peace.IsDirty(Treaty.ItemId.Type))
                ? Color.Red
                : SystemColors.WindowText;
            peaceIdTextBox.ForeColor = (flag && peace.IsDirty(Treaty.ItemId.Id))
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
            relation.SetDirty(Relation.ItemId.Value);
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
            settings.SetDirty(CountrySettings.ItemId.Master);
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
            settings.SetDirty(CountrySettings.ItemId.Control);
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
            relation.SetDirty(Relation.ItemId.Access);
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
            relation.SetDirty(Relation.ItemId.Guaranteed);
            relation.SetDirty(Relation.ItemId.GuaranteedYear);
            relation.SetDirty(Relation.ItemId.GuaranteedMonth);
            relation.SetDirty(Relation.ItemId.GuaranteedDay);
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
                relation.SetDirty(Relation.ItemId.GuaranteedMonth);
                relation.SetDirty(Relation.ItemId.GuaranteedDay);

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
            relation.SetDirty(Relation.ItemId.GuaranteedYear);
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
                relation.SetDirty(Relation.ItemId.GuaranteedYear);
                relation.SetDirty(Relation.ItemId.GuaranteedDay);

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
            relation.SetDirty(Relation.ItemId.GuaranteedMonth);
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
                relation.SetDirty(Relation.ItemId.GuaranteedYear);
                relation.SetDirty(Relation.ItemId.GuaranteedMonth);

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
            relation.SetDirty(Relation.ItemId.GuaranteedDay);
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
            nonAggression.SetDirty(Treaty.ItemId.StartYear);
            nonAggression.SetDirty(Treaty.ItemId.StartMonth);
            nonAggression.SetDirty(Treaty.ItemId.StartDay);
            nonAggression.SetDirty(Treaty.ItemId.EndYear);
            nonAggression.SetDirty(Treaty.ItemId.EndMonth);
            nonAggression.SetDirty(Treaty.ItemId.EndDay);
            nonAggression.SetDirty(Treaty.ItemId.Type);
            nonAggression.SetDirty(Treaty.ItemId.Id);
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
                nonAggression.SetDirty(Treaty.ItemId.StartMonth);
                nonAggression.SetDirty(Treaty.ItemId.StartDay);

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
            nonAggression.SetDirty(Treaty.ItemId.StartYear);
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
                nonAggression.SetDirty(Treaty.ItemId.StartYear);
                nonAggression.SetDirty(Treaty.ItemId.StartDay);

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
            nonAggression.SetDirty(Treaty.ItemId.StartMonth);
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
                nonAggression.SetDirty(Treaty.ItemId.StartYear);
                nonAggression.SetDirty(Treaty.ItemId.StartMonth);

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
            nonAggression.SetDirty(Treaty.ItemId.StartDay);
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
                nonAggression.SetDirty(Treaty.ItemId.EndMonth);
                nonAggression.SetDirty(Treaty.ItemId.EndDay);

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
            nonAggression.SetDirty(Treaty.ItemId.EndYear);
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
                nonAggression.SetDirty(Treaty.ItemId.EndYear);
                nonAggression.SetDirty(Treaty.ItemId.EndDay);

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
            nonAggression.SetDirty(Treaty.ItemId.EndMonth);
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
                nonAggression.SetDirty(Treaty.ItemId.EndYear);
                nonAggression.SetDirty(Treaty.ItemId.EndMonth);

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
            nonAggression.SetDirty(Treaty.ItemId.EndDay);
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
                nonAggression.SetDirty(Treaty.ItemId.Id);

                // 編集項目を更新する
                nonAggressionIdTextBox.Text = IntHelper.ToString(nonAggression.Id.Id);

                // 文字色を変更する
                nonAggressionIdTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            nonAggression.SetDirty(Treaty.ItemId.Type);
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
                nonAggression.SetDirty(Treaty.ItemId.Type);

                // 編集項目を更新する
                nonAggressionTypeTextBox.Text = IntHelper.ToString(nonAggression.Id.Type);

                // 文字色を変更する
                nonAggressionTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            nonAggression.SetDirty(Treaty.ItemId.Id);
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
            peace.SetDirty(Treaty.ItemId.StartYear);
            peace.SetDirty(Treaty.ItemId.StartMonth);
            peace.SetDirty(Treaty.ItemId.StartDay);
            peace.SetDirty(Treaty.ItemId.EndYear);
            peace.SetDirty(Treaty.ItemId.EndMonth);
            peace.SetDirty(Treaty.ItemId.EndDay);
            peace.SetDirty(Treaty.ItemId.Type);
            peace.SetDirty(Treaty.ItemId.Id);
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
                peace.SetDirty(Treaty.ItemId.StartMonth);
                peace.SetDirty(Treaty.ItemId.StartDay);

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
            peace.SetDirty(Treaty.ItemId.StartYear);
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
                peace.SetDirty(Treaty.ItemId.StartYear);
                peace.SetDirty(Treaty.ItemId.StartDay);

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
            peace.SetDirty(Treaty.ItemId.StartMonth);
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
                peace.SetDirty(Treaty.ItemId.StartYear);
                peace.SetDirty(Treaty.ItemId.StartMonth);

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
            peace.SetDirty(Treaty.ItemId.StartDay);
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
                peace.SetDirty(Treaty.ItemId.EndMonth);
                peace.SetDirty(Treaty.ItemId.EndDay);

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
            peace.SetDirty(Treaty.ItemId.EndYear);
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
                peace.SetDirty(Treaty.ItemId.EndYear);
                peace.SetDirty(Treaty.ItemId.EndDay);

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
            peace.SetDirty(Treaty.ItemId.EndMonth);
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
                peace.SetDirty(Treaty.ItemId.EndYear);
                peace.SetDirty(Treaty.ItemId.EndMonth);

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
            peace.SetDirty(Treaty.ItemId.EndDay);
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
                peace.SetDirty(Treaty.ItemId.Id);

                // 編集項目を更新する
                peaceIdTextBox.Text = IntHelper.ToString(peace.Id.Id);

                // 文字色を変更する
                peaceIdTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            peace.SetDirty(Treaty.ItemId.Type);
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
                peace.SetDirty(Treaty.ItemId.Type);

                // 編集項目を更新する
                peaceTypeTextBox.Text = IntHelper.ToString(peace.Id.Type);

                // 文字色を変更する
                peaceTypeTextBox.ForeColor = Color.Red;
            }

            // 編集済みフラグを設定する
            peace.SetDirty(Treaty.ItemId.Id);
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
            spyNumNumericUpDown.ForeColor = (flag && spy.IsDirty(SpySettings.ItemId.Spies))
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
            spy.SetDirty(SpySettings.ItemId.Spies);
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
            InitTradeInfoItems();
            InitTradeDealsItems();
        }

        /// <summary>
        ///     貿易タブの編集項目を更新する
        /// </summary>
        private void UpdateTradeTab()
        {
            // 初期化済みであれば何もしない
            if (_tabPageInitialized[(int) TabPageNo.Trade])
            {
                return;
            }

            // 貿易国コンボボックスを更新する
            UpdateCountryComboBox(tradeCountryComboBox1, false);
            UpdateCountryComboBox(tradeCountryComboBox2, false);

            // 貿易リストを更新する
            UpdateTradeList();

            // 貿易リストを有効化する
            EnableTradeList();

            // 新規ボタンを有効化する
            EnableTradeNewButton();

            // 初期化済みフラグをセットする
            _tabPageInitialized[(int) TabPageNo.Trade] = true;
        }

        /// <summary>
        ///     貿易タブのフォーム読み込み時の処理
        /// </summary>
        private void OnTradeTabPageFormLoad()
        {
            // 貿易タブを初期化する
            InitTradeTab();
        }

        /// <summary>
        ///     貿易タブのファイル読み込み時の処理
        /// </summary>
        private void OnTradeTabPageFileLoad()
        {
            // 貿易タブ選択中でなければ何もしない
            if (_tabPageNo != TabPageNo.Trade)
            {
                return;
            }

            // 初回遷移時には表示を更新する
            UpdateTradeTab();
        }

        /// <summary>
        ///     貿易タブ選択時の処理
        /// </summary>
        private void OnTradeTabPageSelected()
        {
            // シナリオ未読み込みならば何もしない
            if (!Scenarios.IsLoaded())
            {
                return;
            }

            // 初回遷移時には表示を更新する
            UpdateTradeTab();
        }

        #endregion

        #region 貿易タブ - 貿易リスト

        /// <summary>
        ///     貿易リストの表示を更新する
        /// </summary>
        private void UpdateTradeList()
        {
            List<Treaty> trades = Scenarios.Data.GlobalData.Trades;
            tradeListView.BeginUpdate();
            tradeListView.Items.Clear();
            foreach (Treaty treaty in trades)
            {
                tradeListView.Items.Add(CreateTradeListViewItem(treaty));
            }
            tradeListView.EndUpdate();
        }

        /// <summary>
        ///     貿易リストの項目を作成する
        /// </summary>
        /// <param name="treaty">貿易情報</param>
        /// <returns>貿易リストの項目</returns>
        private static ListViewItem CreateTradeListViewItem(Treaty treaty)
        {
            ListViewItem item = new ListViewItem
            {
                Text = Countries.GetName(treaty.Country1),
                Tag = treaty
            };
            item.SubItems.Add(Countries.GetName(treaty.Country2));
            item.SubItems.Add(treaty.GetTradeString());

            return item;
        }

        /// <summary>
        ///     貿易リストを有効化する
        /// </summary>
        private void EnableTradeList()
        {
            tradeListView.Enabled = true;
        }

        /// <summary>
        ///     新規ボタンを有効化する
        /// </summary>
        private void EnableTradeNewButton()
        {
            tradeNewButton.Enabled = true;
        }

        /// <summary>
        ///     削除/上へ/下へボタンを有効化する
        /// </summary>
        private void EnableTradeButtons()
        {
            int index = tradeListView.SelectedIndices[0];
            int count = tradeListView.Items.Count;
            tradeUpButton.Enabled = (index > 0);
            tradeDownButton.Enabled = (index < count - 1);
            tradeRemoveButton.Enabled = true;
        }

        /// <summary>
        ///     削除/上へ/下へボタンを無効化する
        /// </summary>
        private void DisableTradeButtons()
        {
            tradeUpButton.Enabled = false;
            tradeDownButton.Enabled = false;
            tradeRemoveButton.Enabled = false;
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
                // 編集項目を無効化する
                DisableTradeInfoItems();
                DisableTradeDealsItems();
                DisableTradeButtons();

                // 編集項目をクリアする
                ClearTradeInfoItems();
                ClearTradeDealsItems();
                return;
            }

            Treaty treaty = GetSelectedTrade();

            // 編集項目を更新する
            UpdateTradeInfoItems(treaty);
            UpdateTradeDealsItems(treaty);

            // 編集項目を有効化する
            EnableTradeInfoItems();
            EnableTradeDealsItems();
            EnableTradeButtons();
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
            Scenarios.Data.SetDirty();
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
            Scenarios.Data.SetDirty();
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
            trade.SetDirty(Treaty.ItemId.StartYear);
            trade.SetDirty(Treaty.ItemId.StartMonth);
            trade.SetDirty(Treaty.ItemId.StartDay);
            trade.SetDirty(Treaty.ItemId.EndYear);
            trade.SetDirty(Treaty.ItemId.EndMonth);
            trade.SetDirty(Treaty.ItemId.EndDay);
            trade.SetDirty(Treaty.ItemId.Type);
            trade.SetDirty(Treaty.ItemId.Id);
            trade.SetDirty(Treaty.ItemId.Cancel);
            Scenarios.Data.SetDirty();
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
            Scenarios.Data.SetDirty();
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
        ///     貿易リストビューの選択項目の文字列を設定する
        /// </summary>
        /// <param name="no">項目番号</param>
        /// <param name="s">文字列</param>
        public void SetTradeListItemText(int no, string s)
        {
            tradeListView.SelectedItems[0].SubItems[no].Text = s;
        }

        /// <summary>
        ///     選択中の貿易情報を取得する
        /// </summary>
        /// <returns>選択中の貿易情報</returns>
        private Treaty GetSelectedTrade()
        {
            return (tradeListView.SelectedItems.Count > 0) ? tradeListView.SelectedItems[0].Tag as Treaty : null;
        }

        #endregion

        #region 貿易タブ - 貿易情報

        /// <summary>
        ///     貿易情報の編集項目を初期化する
        /// </summary>
        private void InitTradeInfoItems()
        {
            _itemControls.Add(ScenarioEditorItemId.TradeStartYear, tradeStartYearTextBox);
            _itemControls.Add(ScenarioEditorItemId.TradeStartMonth, tradeStartMonthTextBox);
            _itemControls.Add(ScenarioEditorItemId.TradeStartDay, tradeStartDayTextBox);
            _itemControls.Add(ScenarioEditorItemId.TradeEndYear, tradeEndYearTextBox);
            _itemControls.Add(ScenarioEditorItemId.TradeEndMonth, tradeEndMonthTextBox);
            _itemControls.Add(ScenarioEditorItemId.TradeEndDay, tradeEndDayTextBox);
            _itemControls.Add(ScenarioEditorItemId.TradeType, tradeTypeTextBox);
            _itemControls.Add(ScenarioEditorItemId.TradeId, tradeIdTextBox);
            _itemControls.Add(ScenarioEditorItemId.TradeCancel, tradeCancelCheckBox);

            tradeStartYearTextBox.Tag = ScenarioEditorItemId.TradeStartYear;
            tradeStartMonthTextBox.Tag = ScenarioEditorItemId.TradeStartMonth;
            tradeStartDayTextBox.Tag = ScenarioEditorItemId.TradeStartDay;
            tradeEndYearTextBox.Tag = ScenarioEditorItemId.TradeEndYear;
            tradeEndMonthTextBox.Tag = ScenarioEditorItemId.TradeEndMonth;
            tradeEndDayTextBox.Tag = ScenarioEditorItemId.TradeEndDay;
            tradeTypeTextBox.Tag = ScenarioEditorItemId.TradeType;
            tradeIdTextBox.Tag = ScenarioEditorItemId.TradeId;
            tradeCancelCheckBox.Tag = ScenarioEditorItemId.TradeCancel;
        }

        /// <summary>
        ///     貿易情報の編集項目を有効化する
        /// </summary>
        private void EnableTradeInfoItems()
        {
            tradeInfoGroupBox.Enabled = true;
        }

        /// <summary>
        ///     貿易情報の編集項目を無効化する
        /// </summary>
        private void DisableTradeInfoItems()
        {
            tradeInfoGroupBox.Enabled = false;
        }

        /// <summary>
        ///     貿易情報の編集項目の表示を更新する
        /// </summary>
        /// <param name="treaty">協定</param>
        private void UpdateTradeInfoItems(Treaty treaty)
        {
            // 編集項目の表示を更新する
            _controller.UpdateItemValue(tradeStartYearTextBox, treaty);
            _controller.UpdateItemValue(tradeStartMonthTextBox, treaty);
            _controller.UpdateItemValue(tradeStartDayTextBox, treaty);
            _controller.UpdateItemValue(tradeEndYearTextBox, treaty);
            _controller.UpdateItemValue(tradeEndMonthTextBox, treaty);
            _controller.UpdateItemValue(tradeEndDayTextBox, treaty);
            _controller.UpdateItemValue(tradeTypeTextBox, treaty);
            _controller.UpdateItemValue(tradeIdTextBox, treaty);
            _controller.UpdateItemValue(tradeCancelCheckBox, treaty);

            // 編集項目の色を更新する
            _controller.UpdateItemColor(tradeStartYearTextBox, treaty);
            _controller.UpdateItemColor(tradeStartMonthTextBox, treaty);
            _controller.UpdateItemColor(tradeStartDayTextBox, treaty);
            _controller.UpdateItemColor(tradeEndYearTextBox, treaty);
            _controller.UpdateItemColor(tradeEndMonthTextBox, treaty);
            _controller.UpdateItemColor(tradeEndDayTextBox, treaty);
            _controller.UpdateItemColor(tradeTypeTextBox, treaty);
            _controller.UpdateItemColor(tradeIdTextBox, treaty);
            _controller.UpdateItemColor(tradeCancelCheckBox, treaty);
        }

        /// <summary>
        ///     貿易情報の編集項目の表示をクリアする
        /// </summary>
        private void ClearTradeInfoItems()
        {
            tradeStartYearTextBox.Text = "";
            tradeStartMonthTextBox.Text = "";
            tradeStartDayTextBox.Text = "";
            tradeEndYearTextBox.Text = "";
            tradeEndMonthTextBox.Text = "";
            tradeEndDayTextBox.Text = "";
            tradeTypeTextBox.Text = "";
            tradeIdTextBox.Text = "";
            tradeCancelCheckBox.Checked = false;
        }

        #endregion

        #region 貿易タブ - 貿易内容

        /// <summary>
        ///     貿易内容の編集項目を初期化する
        /// </summary>
        private void InitTradeDealsItems()
        {
            _itemControls.Add(ScenarioEditorItemId.TradeCountry1, tradeCountryComboBox1);
            _itemControls.Add(ScenarioEditorItemId.TradeCountry2, tradeCountryComboBox2);
            _itemControls.Add(ScenarioEditorItemId.TradeEnergy1, tradeEnergyTextBox1);
            _itemControls.Add(ScenarioEditorItemId.TradeEnergy2, tradeEnergyTextBox2);
            _itemControls.Add(ScenarioEditorItemId.TradeMetal1, tradeMetalTextBox1);
            _itemControls.Add(ScenarioEditorItemId.TradeMetal2, tradeMetalTextBox2);
            _itemControls.Add(ScenarioEditorItemId.TradeRareMaterials1, tradeRareMaterialsTextBox1);
            _itemControls.Add(ScenarioEditorItemId.TradeRareMaterials2, tradeRareMaterialsTextBox2);
            _itemControls.Add(ScenarioEditorItemId.TradeOil1, tradeOilTextBox1);
            _itemControls.Add(ScenarioEditorItemId.TradeOil2, tradeOilTextBox2);
            _itemControls.Add(ScenarioEditorItemId.TradeSupplies1, tradeSuppliesTextBox1);
            _itemControls.Add(ScenarioEditorItemId.TradeSupplies2, tradeSuppliesTextBox2);
            _itemControls.Add(ScenarioEditorItemId.TradeMoney1, tradeMoneyTextBox1);
            _itemControls.Add(ScenarioEditorItemId.TradeMoney2, tradeMoneyTextBox2);

            tradeCountryComboBox1.Tag = ScenarioEditorItemId.TradeCountry1;
            tradeCountryComboBox2.Tag = ScenarioEditorItemId.TradeCountry2;
            tradeEnergyTextBox1.Tag = ScenarioEditorItemId.TradeEnergy1;
            tradeEnergyTextBox2.Tag = ScenarioEditorItemId.TradeEnergy2;
            tradeMetalTextBox1.Tag = ScenarioEditorItemId.TradeMetal1;
            tradeMetalTextBox2.Tag = ScenarioEditorItemId.TradeMetal2;
            tradeRareMaterialsTextBox1.Tag = ScenarioEditorItemId.TradeRareMaterials1;
            tradeRareMaterialsTextBox2.Tag = ScenarioEditorItemId.TradeRareMaterials2;
            tradeOilTextBox1.Tag = ScenarioEditorItemId.TradeOil1;
            tradeOilTextBox2.Tag = ScenarioEditorItemId.TradeOil2;
            tradeSuppliesTextBox1.Tag = ScenarioEditorItemId.TradeSupplies1;
            tradeSuppliesTextBox2.Tag = ScenarioEditorItemId.TradeSupplies2;
            tradeMoneyTextBox1.Tag = ScenarioEditorItemId.TradeMoney1;
            tradeMoneyTextBox2.Tag = ScenarioEditorItemId.TradeMoney2;

            // 貿易資源ラベル
            tradeEnergyLabel.Text = Config.GetText(TextId.ResourceEnergy);
            tradeMetalLabel.Text = Config.GetText(TextId.ResourceMetal);
            tradeRareMaterialsLabel.Text = Config.GetText(TextId.ResourceRareMaterials);
            tradeOilLabel.Text = Config.GetText(TextId.ResourceOil);
            tradeSuppliesLabel.Text = Config.GetText(TextId.ResourceSupplies);
            tradeMoneyLabel.Text = Config.GetText(TextId.ResourceMoney);
        }

        /// <summary>
        ///     貿易内容の編集項目を有効化する
        /// </summary>
        private void EnableTradeDealsItems()
        {
            tradeDealsGroupBox.Enabled = true;
        }

        /// <summary>
        ///     貿易内容の編集項目を無効化する
        /// </summary>
        private void DisableTradeDealsItems()
        {
            tradeDealsGroupBox.Enabled = false;
        }

        /// <summary>
        ///     貿易内容の編集項目の表示を更新する
        /// </summary>
        /// <param name="treaty">協定</param>
        private void UpdateTradeDealsItems(Treaty treaty)
        {
            // 編集項目の表示を更新する
            _controller.UpdateItemValue(tradeCountryComboBox1, treaty);
            _controller.UpdateItemValue(tradeCountryComboBox2, treaty);
            _controller.UpdateItemValue(tradeEnergyTextBox1, treaty);
            _controller.UpdateItemValue(tradeEnergyTextBox2, treaty);
            _controller.UpdateItemValue(tradeMetalTextBox1, treaty);
            _controller.UpdateItemValue(tradeMetalTextBox2, treaty);
            _controller.UpdateItemValue(tradeRareMaterialsTextBox1, treaty);
            _controller.UpdateItemValue(tradeRareMaterialsTextBox2, treaty);
            _controller.UpdateItemValue(tradeOilTextBox1, treaty);
            _controller.UpdateItemValue(tradeOilTextBox2, treaty);
            _controller.UpdateItemValue(tradeSuppliesTextBox1, treaty);
            _controller.UpdateItemValue(tradeSuppliesTextBox2, treaty);
            _controller.UpdateItemValue(tradeMoneyTextBox1, treaty);
            _controller.UpdateItemValue(tradeMoneyTextBox2, treaty);

            // 編集項目の色を更新する
            _controller.UpdateItemColor(tradeEnergyTextBox1, treaty);
            _controller.UpdateItemColor(tradeEnergyTextBox2, treaty);
            _controller.UpdateItemColor(tradeMetalTextBox1, treaty);
            _controller.UpdateItemColor(tradeMetalTextBox2, treaty);
            _controller.UpdateItemColor(tradeRareMaterialsTextBox1, treaty);
            _controller.UpdateItemColor(tradeRareMaterialsTextBox2, treaty);
            _controller.UpdateItemColor(tradeOilTextBox1, treaty);
            _controller.UpdateItemColor(tradeOilTextBox2, treaty);
            _controller.UpdateItemColor(tradeSuppliesTextBox1, treaty);
            _controller.UpdateItemColor(tradeSuppliesTextBox2, treaty);
            _controller.UpdateItemColor(tradeMoneyTextBox1, treaty);
            _controller.UpdateItemColor(tradeMoneyTextBox2, treaty);
        }

        /// <summary>
        ///     貿易内容の編集項目の表示をクリアする
        /// </summary>
        private void ClearTradeDealsItems()
        {
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
        ///     貿易国入れ替えボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeSwapButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty treaty = GetSelectedTrade();
            if (treaty == null)
            {
                return;
            }

            // 値を入れ替える
            Country country = treaty.Country1;
            treaty.Country1 = treaty.Country2;
            treaty.Country2 = country;

            treaty.Energy = -treaty.Energy;
            treaty.Metal = -treaty.Metal;
            treaty.RareMaterials = -treaty.RareMaterials;
            treaty.Oil = -treaty.Oil;
            treaty.Supplies = -treaty.Supplies;
            treaty.Money = -treaty.Money;

            // 編集済みフラグを設定する
            treaty.SetDirty(Treaty.ItemId.Country1);
            treaty.SetDirty(Treaty.ItemId.Country2);
            treaty.SetDirty(Treaty.ItemId.Energy);
            treaty.SetDirty(Treaty.ItemId.Metal);
            treaty.SetDirty(Treaty.ItemId.RareMaterials);
            treaty.SetDirty(Treaty.ItemId.Oil);
            treaty.SetDirty(Treaty.ItemId.Supplies);
            treaty.SetDirty(Treaty.ItemId.Money);
            Scenarios.SetDirty();

            // 貿易リストビューの項目を更新する
            ListViewItem item = tradeListView.SelectedItems[0];
            item.Text = Countries.GetName(treaty.Country1);
            item.SubItems[1].Text = Countries.GetName(treaty.Country2);
            item.SubItems[2].Text = treaty.GetTradeString();

            // 編集項目を更新する
            UpdateTradeDealsItems(treaty);
        }

        #endregion

        #region 貿易タブ - 編集項目

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeIntItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty treaty = GetSelectedTrade();
            if (treaty == null)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, treaty);
                return;
            }

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, treaty);
            if ((prev == null) && (val == 0))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((prev != null) && (val == (int) prev))
            {
                return;
            }

            // 無効な値ならば値を戻す
            if (!_controller.IsItemValueValid(itemId, val, treaty))
            {
                _controller.UpdateItemValue(control, treaty);
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, treaty);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, treaty);

            // 値を更新する
            _controller.SetItemValue(itemId, val, treaty);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, treaty);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, treaty);
        }

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeDoubleItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty treaty = GetSelectedTrade();
            if (treaty == null)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, treaty);
                return;
            }

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, treaty);
            if ((prev == null) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((prev != null) && DoubleHelper.IsEqual(val, (double) prev))
            {
                return;
            }

            // 無効な値ならば値を戻す
            if (!_controller.IsItemValueValid(itemId, val, treaty))
            {
                _controller.UpdateItemValue(control, treaty);
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, treaty);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, treaty);

            // 値を更新する
            _controller.SetItemValue(itemId, val, treaty);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, treaty);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, treaty);
        }

        /// <summary>
        ///     チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeItemCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty treaty = GetSelectedTrade();
            if (treaty == null)
            {
                return;
            }

            CheckBox control = sender as CheckBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            // 値に変化がなければ何もしない
            bool val = control.Checked;
            if (val == (bool) _controller.GetItemValue(itemId, treaty))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, treaty);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, treaty);

            // 値を更新する
            _controller.SetItemValue(itemId, val, treaty);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, treaty);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, treaty);
        }

        /// <summary>
        ///     コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeCountryItemComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index < 0)
            {
                return;
            }

            ComboBox control = sender as ComboBox;
            if (control == null)
            {
                return;
            }

            Treaty treaty = GetSelectedTrade();
            if (treaty == null)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            Country val = (Country) _controller.GetItemValue(itemId, treaty);
            Country sel = (Country) _controller.GetListItemValue(itemId, e.Index);
            Brush brush = ((val == sel) && _controller.IsItemDirty(itemId, treaty))
                ? new SolidBrush(Color.Red)
                : new SolidBrush(SystemColors.WindowText);
            string s = control.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeCountryItemComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox control = sender as ComboBox;
            if (control == null)
            {
                return;
            }

            // 選択項目がなければ何もしない
            if (control.SelectedIndex < 0)
            {
                return;
            }

            Treaty treaty = GetSelectedTrade();
            if (treaty == null)
            {
                return;
            }

            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            // 値に変化がなければ何もしない
            Country val = Countries.Tags[control.SelectedIndex];
            if (val == (Country) _controller.GetItemValue(itemId, treaty))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, treaty);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, treaty);

            // 値を更新する
            _controller.SetItemValue(itemId, val, treaty);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, treaty);

            // 項目色を変更するため描画更新する
            control.Refresh();

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, treaty);
        }

        #endregion

        #endregion

        #region 国家タブ

        #region 国家タブ- 共通

        /// <summary>
        ///     国家タブを初期化する
        /// </summary>
        private void InitCountryTab()
        {
            InitCountryInfoItems();
            InitCountryModifierItems();
            InitCountryResourceItems();
            InitCountryAiItems();
        }

        /// <summary>
        ///     国家タブの編集項目を更新する
        /// </summary>
        private void UpdateCountryTab()
        {
            // 初期化済みであれば何もしない
            if (_tabPageInitialized[(int) TabPageNo.Country])
            {
                return;
            }

            // 兄弟国コンボボックスを更新する
            UpdateCountryComboBox(regularIdComboBox, true);

            // 国家リストボックスを更新する
            UpdateCountryListBox(countryListBox);

            // 国家リストボックスを有効化する
            EnableCountryListBox();

            // 初期化済みフラグをセットする
            _tabPageInitialized[(int) TabPageNo.Country] = true;
        }

        /// <summary>
        ///     国家タブのフォーム読み込み時の処理
        /// </summary>
        private void OnCountryTabPageFormLoad()
        {
            // 国家タブを初期化する
            InitCountryTab();
        }

        /// <summary>
        ///     国家タブのファイル読み込み時の処理
        /// </summary>
        private void OnCountryTabPageFileLoad()
        {
            // 国家タブ選択中でなければ何もしない
            if (_tabPageNo != TabPageNo.Country)
            {
                return;
            }

            // 初回遷移時には表示を更新する
            UpdateCountryTab();
        }

        /// <summary>
        ///     国家タブ選択時の処理
        /// </summary>
        private void OnCountryTabPageSelected()
        {
            // シナリオ未読み込みならば何もしない
            if (!Scenarios.IsLoaded())
            {
                return;
            }

            // 初回遷移時には表示を更新する
            UpdateCountryTab();
        }

        #endregion

        #region 国家タブ - 国家

        /// <summary>
        ///     国家リストボックスを有効化する
        /// </summary>
        private void EnableCountryListBox()
        {
            countryListBox.Enabled = true;
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
                // 編集項目を無効化する
                DisableCountryInfoItems();
                DisableCountryModifierItems();
                DisableCountryResourceItems();
                DisableCountryAiItems();

                // 編集項目をクリアする
                ClearCountryInfoItems();
                ClearCountryModifierItems();
                ClearCountryResourceItems();
                ClearCountryAiItems();
                return;
            }

            Country country = GetSelectedCountry();
            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 編集項目を更新する
            UpdateCountryInfoItems(country, settings);
            UpdateCountryModifierItems(settings);
            UpdateCountryResourceItems(settings);
            UpdateCountryAiItems(settings);

            // 編集項目を有効化する
            EnableCountryInfoItems();
            EnableCountryModifierItems();
            EnableCountryResourceItems();
            EnableCountryAiItems();
        }

        /// <summary>
        ///     選択中の国家を取得する
        /// </summary>
        /// <returns>選択中の国家</returns>
        private Country GetSelectedCountry()
        {
            return (countryListBox.SelectedIndex >= 0) ? Countries.Tags[countryListBox.SelectedIndex] : Country.None;
        }

        #endregion

        #region 国家タブ - 国家情報

        /// <summary>
        ///     国家情報の編集項目を初期化する
        /// </summary>
        private void InitCountryInfoItems()
        {
            _itemControls.Add(ScenarioEditorItemId.CountryName, countryNameTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryFlagExt, flagExtTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryRegularId, regularIdComboBox);
            _itemControls.Add(ScenarioEditorItemId.CountryBelligerence, belligerenceTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryDissent, dissentTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryExtraTc, extraTcTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryNuke, nukeTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryNukeYear, nukeYearTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryNukeMonth, nukeMonthTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryNukeDay, nukeDayTextBox);

            countryNameTextBox.Tag = ScenarioEditorItemId.CountryName;
            flagExtTextBox.Tag = ScenarioEditorItemId.CountryFlagExt;
            regularIdComboBox.Tag = ScenarioEditorItemId.CountryRegularId;
            belligerenceTextBox.Tag = ScenarioEditorItemId.CountryBelligerence;
            dissentTextBox.Tag = ScenarioEditorItemId.CountryDissent;
            extraTcTextBox.Tag = ScenarioEditorItemId.CountryExtraTc;
            nukeTextBox.Tag = ScenarioEditorItemId.CountryNuke;
            nukeYearTextBox.Tag = ScenarioEditorItemId.CountryNukeYear;
            nukeMonthTextBox.Tag = ScenarioEditorItemId.CountryNukeMonth;
            nukeDayTextBox.Tag = ScenarioEditorItemId.CountryNukeDay;
        }

        /// <summary>
        ///     国家情報の編集項目を有効化する
        /// </summary>
        private void EnableCountryInfoItems()
        {
            countryInfoGroupBox.Enabled = true;
        }

        /// <summary>
        ///     国家情報の編集項目を無効化する
        /// </summary>
        private void DisableCountryInfoItems()
        {
            countryInfoGroupBox.Enabled = false;
        }

        /// <summary>
        ///     国家情報の編集項目の表示を更新する
        /// </summary>
        /// <param name="country">選択国</param>
        /// <param name="settings">国家設定</param>
        private void UpdateCountryInfoItems(Country country, CountrySettings settings)
        {
            // 編集項目の表示を更新する
            _controller.UpdateItemValue(countryNameTextBox, country, settings);
            _controller.UpdateItemValue(flagExtTextBox, country, settings);
            _controller.UpdateItemValue(regularIdComboBox, settings);
            _controller.UpdateItemValue(belligerenceTextBox, settings);
            _controller.UpdateItemValue(dissentTextBox, settings);
            _controller.UpdateItemValue(extraTcTextBox, settings);
            _controller.UpdateItemValue(nukeTextBox, settings);
            _controller.UpdateItemValue(nukeYearTextBox, settings);
            _controller.UpdateItemValue(nukeMonthTextBox, settings);
            _controller.UpdateItemValue(nukeDayTextBox, settings);

            // 編集項目の色を更新する
            _controller.UpdateItemColor(countryNameTextBox, country, settings);
            _controller.UpdateItemColor(flagExtTextBox, country, settings);
            _controller.UpdateItemColor(belligerenceTextBox, settings);
            _controller.UpdateItemColor(dissentTextBox, settings);
            _controller.UpdateItemColor(extraTcTextBox, settings);
            _controller.UpdateItemColor(nukeTextBox, settings);
            _controller.UpdateItemColor(nukeYearTextBox, settings);
            _controller.UpdateItemColor(nukeMonthTextBox, settings);
            _controller.UpdateItemColor(nukeDayTextBox, settings);
        }

        /// <summary>
        ///     国家情報の編集項目の表示をクリアする
        /// </summary>
        private void ClearCountryInfoItems()
        {
            countryNameTextBox.Text = "";
            flagExtTextBox.Text = "";
            regularIdComboBox.SelectedIndex = -1;
            belligerenceTextBox.Text = "";
            dissentTextBox.Text = "";
            extraTcTextBox.Text = "";
            nukeTextBox.Text = "";
            nukeYearTextBox.Text = "";
            nukeMonthTextBox.Text = "";
            nukeDayTextBox.Text = "";
        }

        #endregion

        #region 国家タブ - 補正値

        /// <summary>
        ///     国家補正値の編集項目を初期化する
        /// </summary>
        private void InitCountryModifierItems()
        {
            _itemControls.Add(ScenarioEditorItemId.CountryGroundDefEff, groundDefEffTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryPeacetimeIcModifier, peacetimeIcModifierTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryWartimeIcModifier, wartimeIcModifierTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryIndustrialModifier, industrialModifierTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryRelativeManpower, relativeManpowerTextBox);

            groundDefEffTextBox.Tag = ScenarioEditorItemId.CountryGroundDefEff;
            peacetimeIcModifierTextBox.Tag = ScenarioEditorItemId.CountryPeacetimeIcModifier;
            wartimeIcModifierTextBox.Tag = ScenarioEditorItemId.CountryWartimeIcModifier;
            industrialModifierTextBox.Tag = ScenarioEditorItemId.CountryIndustrialModifier;
            relativeManpowerTextBox.Tag = ScenarioEditorItemId.CountryRelativeManpower;
        }

        /// <summary>
        ///     国家補正値の編集項目を有効化する
        /// </summary>
        private void EnableCountryModifierItems()
        {
            countryModifierGroupBox.Enabled = true;
        }

        /// <summary>
        ///     国家補正値の編集項目を無効化する
        /// </summary>
        private void DisableCountryModifierItems()
        {
            countryModifierGroupBox.Enabled = false;
        }

        /// <summary>
        ///     国家補正値の編集項目の表示を更新する
        /// </summary>
        /// <param name="settings">国家設定</param>
        private void UpdateCountryModifierItems(CountrySettings settings)
        {
            // 編集項目の表示を更新する
            _controller.UpdateItemValue(groundDefEffTextBox, settings);
            _controller.UpdateItemValue(peacetimeIcModifierTextBox, settings);
            _controller.UpdateItemValue(wartimeIcModifierTextBox, settings);
            _controller.UpdateItemValue(industrialModifierTextBox, settings);
            _controller.UpdateItemValue(relativeManpowerTextBox, settings);

            // 編集項目の色を更新する
            _controller.UpdateItemColor(groundDefEffTextBox, settings);
            _controller.UpdateItemColor(peacetimeIcModifierTextBox, settings);
            _controller.UpdateItemColor(wartimeIcModifierTextBox, settings);
            _controller.UpdateItemColor(industrialModifierTextBox, settings);
            _controller.UpdateItemColor(relativeManpowerTextBox, settings);
        }

        /// <summary>
        ///     国家補正値の編集項目の表示をクリアする
        /// </summary>
        private void ClearCountryModifierItems()
        {
            groundDefEffTextBox.Text = "";
            peacetimeIcModifierTextBox.Text = "";
            wartimeIcModifierTextBox.Text = "";
            industrialModifierTextBox.Text = "";
            relativeManpowerTextBox.Text = "";
        }

        #endregion

        #region 国家タブ - 資源情報

        /// <summary>
        ///     国家資源情報の編集項目を初期化する
        /// </summary>
        private void InitCountryResourceItems()
        {
            _itemControls.Add(ScenarioEditorItemId.CountryEnergy, countryEnergyTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryMetal, countryMetalTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryRareMaterials, countryRareMaterialsTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryOil, countryOilTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountrySupplies, countrySuppliesTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryMoney, countryMoneyTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryTransports, countryTransportsTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryEscorts, countryEscortsTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryManpower, countryManpowerTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryOffmapEnergy, offmapEnergyTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryOffmapMetal, offmapMetalTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryOffmapRareMaterials, offmapRareMaterialsTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryOffmapOil, offmapOilTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryOffmapSupplies, offmapSuppliesTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryOffmapMoney, offmapMoneyTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryOffmapTransports, offmapTransportsTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryOffmapEscorts, offmapEscortsTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryOffmapManpower, offmapManpowerTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryOffmapIc, offmapIcTextBox);

            countryEnergyTextBox.Tag = ScenarioEditorItemId.CountryEnergy;
            countryMetalTextBox.Tag = ScenarioEditorItemId.CountryMetal;
            countryRareMaterialsTextBox.Tag = ScenarioEditorItemId.CountryRareMaterials;
            countryOilTextBox.Tag = ScenarioEditorItemId.CountryOil;
            countrySuppliesTextBox.Tag = ScenarioEditorItemId.CountrySupplies;
            countryMoneyTextBox.Tag = ScenarioEditorItemId.CountryMoney;
            countryTransportsTextBox.Tag = ScenarioEditorItemId.CountryTransports;
            countryEscortsTextBox.Tag = ScenarioEditorItemId.CountryEscorts;
            countryManpowerTextBox.Tag = ScenarioEditorItemId.CountryManpower;
            offmapEnergyTextBox.Tag = ScenarioEditorItemId.CountryOffmapEnergy;
            offmapMetalTextBox.Tag = ScenarioEditorItemId.CountryOffmapMetal;
            offmapRareMaterialsTextBox.Tag = ScenarioEditorItemId.CountryOffmapRareMaterials;
            offmapOilTextBox.Tag = ScenarioEditorItemId.CountryOffmapOil;
            offmapSuppliesTextBox.Tag = ScenarioEditorItemId.CountryOffmapSupplies;
            offmapMoneyTextBox.Tag = ScenarioEditorItemId.CountryOffmapMoney;
            offmapTransportsTextBox.Tag = ScenarioEditorItemId.CountryOffmapTransports;
            offmapEscortsTextBox.Tag = ScenarioEditorItemId.CountryOffmapEscorts;
            offmapManpowerTextBox.Tag = ScenarioEditorItemId.CountryOffmapManpower;
            offmapIcTextBox.Tag = ScenarioEditorItemId.CountryOffmapIc;

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
        ///     国家資源情報の編集項目を有効化する
        /// </summary>
        private void EnableCountryResourceItems()
        {
            countryResourceGroupBox.Enabled = true;
        }

        /// <summary>
        ///     国家資源情報の編集項目を無効化する
        /// </summary>
        private void DisableCountryResourceItems()
        {
            countryResourceGroupBox.Enabled = false;
        }

        /// <summary>
        ///     国家資源情報の編集項目の表示を更新する
        /// </summary>
        /// <param name="settings">国家設定</param>
        private void UpdateCountryResourceItems(CountrySettings settings)
        {
            // 編集項目の表示を更新する
            _controller.UpdateItemValue(countryEnergyTextBox, settings);
            _controller.UpdateItemValue(countryMetalTextBox, settings);
            _controller.UpdateItemValue(countryRareMaterialsTextBox, settings);
            _controller.UpdateItemValue(countryOilTextBox, settings);
            _controller.UpdateItemValue(countrySuppliesTextBox, settings);
            _controller.UpdateItemValue(countryMoneyTextBox, settings);
            _controller.UpdateItemValue(countryTransportsTextBox, settings);
            _controller.UpdateItemValue(countryEscortsTextBox, settings);
            _controller.UpdateItemValue(countryManpowerTextBox, settings);
            _controller.UpdateItemValue(offmapEnergyTextBox, settings);
            _controller.UpdateItemValue(offmapMetalTextBox, settings);
            _controller.UpdateItemValue(offmapRareMaterialsTextBox, settings);
            _controller.UpdateItemValue(offmapOilTextBox, settings);
            _controller.UpdateItemValue(offmapSuppliesTextBox, settings);
            _controller.UpdateItemValue(offmapMoneyTextBox, settings);
            _controller.UpdateItemValue(offmapTransportsTextBox, settings);
            _controller.UpdateItemValue(offmapEscortsTextBox, settings);
            _controller.UpdateItemValue(offmapManpowerTextBox, settings);
            _controller.UpdateItemValue(offmapIcTextBox, settings);

            // 編集項目の色を更新する
            _controller.UpdateItemColor(countryEnergyTextBox, settings);
            _controller.UpdateItemColor(countryMetalTextBox, settings);
            _controller.UpdateItemColor(countryRareMaterialsTextBox, settings);
            _controller.UpdateItemColor(countryOilTextBox, settings);
            _controller.UpdateItemColor(countrySuppliesTextBox, settings);
            _controller.UpdateItemColor(countryMoneyTextBox, settings);
            _controller.UpdateItemColor(countryTransportsTextBox, settings);
            _controller.UpdateItemColor(countryEscortsTextBox, settings);
            _controller.UpdateItemColor(countryManpowerTextBox, settings);
            _controller.UpdateItemColor(offmapEnergyTextBox, settings);
            _controller.UpdateItemColor(offmapMetalTextBox, settings);
            _controller.UpdateItemColor(offmapRareMaterialsTextBox, settings);
            _controller.UpdateItemColor(offmapOilTextBox, settings);
            _controller.UpdateItemColor(offmapSuppliesTextBox, settings);
            _controller.UpdateItemColor(offmapMoneyTextBox, settings);
            _controller.UpdateItemColor(offmapTransportsTextBox, settings);
            _controller.UpdateItemColor(offmapEscortsTextBox, settings);
            _controller.UpdateItemColor(offmapManpowerTextBox, settings);
            _controller.UpdateItemColor(offmapIcTextBox, settings);
        }

        /// <summary>
        ///     国家資源情報の編集項目の表示をクリアする
        /// </summary>
        private void ClearCountryResourceItems()
        {
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
        }

        #endregion

        #region 国家タブ - AI情報

        /// <summary>
        ///     国家AI情報の編集項目を初期化する
        /// </summary>
        private void InitCountryAiItems()
        {
            _itemControls.Add(ScenarioEditorItemId.CountryAiFileName, aiFileNameTextBox);

            aiFileNameTextBox.Tag = ScenarioEditorItemId.CountryAiFileName;
        }

        /// <summary>
        ///     国家AI情報の編集項目を有効化する
        /// </summary>
        private void EnableCountryAiItems()
        {
            aiGroupBox.Enabled = true;
        }

        /// <summary>
        ///     国家AI情報の編集項目を無効化する
        /// </summary>
        private void DisableCountryAiItems()
        {
            aiGroupBox.Enabled = false;
        }

        /// <summary>
        ///     国家AI情報の編集項目の表示を更新する
        /// </summary>
        /// <param name="settings">国家設定</param>
        private void UpdateCountryAiItems(CountrySettings settings)
        {
            // 編集項目の表示を更新する
            _controller.UpdateItemValue(aiFileNameTextBox, settings);

            // 編集項目の色を更新する
            _controller.UpdateItemColor(aiFileNameTextBox, settings);
        }

        /// <summary>
        ///     国家AI情報の編集項目の表示をクリアする
        /// </summary>
        private void ClearCountryAiItems()
        {
            aiFileNameTextBox.Text = "";
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

        #region 国家タブ - 編集項目

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryIntItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中の国家がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, settings);
                return;
            }

            // 初期値から変更されていなければ何もしない
            if ((settings == null) && (val == 0))
            {
                return;
            }

            // 値に変化がなければ何もしない
            object prev = _controller.GetItemValue(itemId, settings);
            if ((prev != null) && (val == (int) prev))
            {
                return;
            }

            // 無効な値ならば値を戻す
            if (!_controller.IsItemValueValid(itemId, val, settings))
            {
                _controller.UpdateItemValue(control, settings);
                return;
            }

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            _controller.OutputItemValueChangedLog(itemId, val, settings);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, settings);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, settings);
        }

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryDoubleItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中の国家がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, settings);
                return;
            }

            // 初期値から変更されていなければ何もしない
            if ((settings == null) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            object prev = _controller.GetItemValue(itemId, settings);
            if ((prev != null) && DoubleHelper.IsEqual(val, (double) prev))
            {
                return;
            }

            // 無効な値ならば値を戻す
            if (!_controller.IsItemValueValid(itemId, val, settings))
            {
                _controller.UpdateItemValue(control, settings);
                return;
            }

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            _controller.OutputItemValueChangedLog(itemId, val, settings);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, settings);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, settings);
        }

        /// <summary>
        ///     テキストボックスの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryStringItemTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択中の国家がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 初期値から変更されていなければ何もしない
            string val = control.Text;
            if ((settings == null) && string.IsNullOrEmpty(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (val.Equals(_controller.GetItemValue(itemId, settings)))
            {
                return;
            }

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            _controller.OutputItemValueChangedLog(itemId, val, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, settings);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;
        }

        /// <summary>
        ///     テキストボックスの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryNameItemTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択中の国家がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 初期値から変更されていなければ何もしない
            string val = control.Text;
            if ((settings == null) && string.IsNullOrEmpty(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (val.Equals(_controller.GetItemValue(itemId, country, settings)))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, country, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, country, settings);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, country, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;
        }

        /// <summary>
        ///     コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryItemComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index < 0)
            {
                return;
            }

            ComboBox control = sender as ComboBox;
            if (control == null)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            if (e.Index > 0)
            {
                Country country = GetSelectedCountry();
                CountrySettings settings = Scenarios.GetCountrySettings(country);
                ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
                object val = _controller.GetItemValue(itemId, settings);
                object sel = _controller.GetListItemValue(itemId, e.Index);
                Brush brush = ((val != null) && ((Country) val == (Country) sel) &&
                               _controller.IsItemDirty(itemId, settings))
                    ? new SolidBrush(Color.Red)
                    : new SolidBrush(SystemColors.WindowText);
                string s = control.Items[e.Index].ToString();
                e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryItemComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            ComboBox control = sender as ComboBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 初期値から変更されていなければ何もしない
            if ((settings == null) && (control.SelectedIndex == 0))
            {
                return;
            }

            // 値に変化がなければ何もしない
            Country val = (control.SelectedIndex > 0) ? Countries.Tags[control.SelectedIndex - 1] : Country.None;
            if ((settings != null) && (val == (Country) _controller.GetItemValue(itemId, settings)))
            {
                return;
            }

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            _controller.OutputItemValueChangedLog(itemId, val, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, settings);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, settings);

            // コンボボックスの項目色を変更するために描画更新する
            control.Refresh();
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
            InitPoliticalSliderItems();
            InitCabinetItems();
        }

        /// <summary>
        ///     政府タブの表示を更新する
        /// </summary>
        private void UpdateGovernmentTab()
        {
            // 初期化済みであれば何もしない
            if (_tabPageInitialized[(int) TabPageNo.Government])
            {
                return;
            }

            // 国家リストボックスを更新する
            UpdateCountryListBox(governmentCountryListBox);

            // 国家リストボックスを有効化する
            EnableGovernmentCountryListBox();

            // 初期化済みフラグをセットする
            _tabPageInitialized[(int) TabPageNo.Government] = true;
        }

        /// <summary>
        ///     政府タブのフォーム読み込み時の処理
        /// </summary>
        private void OnGovernmentTabPageFormLoad()
        {
            // 政府タブを初期化する
            InitGovernmentTab();
        }

        /// <summary>
        ///     政府タブのファイル読み込み時の処理
        /// </summary>
        private void OnGovernmentTabPageFileLoad()
        {
            // 政府タブ選択中でなければ何もしない
            if (_tabPageNo != TabPageNo.Government)
            {
                return;
            }

            // 閣僚データの読み込み完了まで待機する
            WaitLoadingMinisters();

            // 初回遷移時には表示を更新する
            UpdateGovernmentTab();
        }

        /// <summary>
        ///     政府タブ選択時の処理
        /// </summary>
        private void OnGovernmentTabPageSelected()
        {
            // シナリオ未読み込みならば何もしない
            if (!Scenarios.IsLoaded())
            {
                return;
            }

            // 閣僚データの読み込み完了まで待機する
            WaitLoadingMinisters();

            // 初回遷移時には表示を更新する
            UpdateGovernmentTab();
        }

        #endregion

        #region 政府タブ - 国家

        /// <summary>
        ///     国家リストボックスを有効化する
        /// </summary>
        private void EnableGovernmentCountryListBox()
        {
            governmentCountryListBox.Enabled = true;
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
                // 編集項目を無効化する
                DisablePoliticalSliderItems();
                DisableCabinetItems();

                // 編集項目をクリアする
                ClearPoliticalSliderItems();
                ClearCabinetItems();
                return;
            }

            Country country = GetSelectedGovernmentCountry();
            CountrySettings settings = Scenarios.GetCountrySettings(country);
            ScenarioHeader header = Scenarios.Data.Header;
            int year = (header.StartDate != null) ? header.StartDate.Year : header.StartYear;

            // 編集項目を更新する
            UpdatePoliticalSliderItems(settings);
            UpdateCabinetItems(country, settings, year);

            // 編集項目を有効化する
            EnablePoliticalSliderItems();
            EnableCabinetItems();
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
        ///     政策スライダーの編集項目を初期化する
        /// </summary>
        private void InitPoliticalSliderItems()
        {
            _itemControls.Add(ScenarioEditorItemId.SliderYear, sliderYearTextBox);
            _itemControls.Add(ScenarioEditorItemId.SliderMonth, sliderMonthTextBox);
            _itemControls.Add(ScenarioEditorItemId.SliderDay, sliderDayTextBox);

            _itemControls.Add(ScenarioEditorItemId.SliderDemocratic, democraticTrackBar);
            _itemControls.Add(ScenarioEditorItemId.SliderPoliticalLeft, politicalLeftTrackBar);
            _itemControls.Add(ScenarioEditorItemId.SliderFreedom, freedomTrackBar);
            _itemControls.Add(ScenarioEditorItemId.SliderFreeMarket, freeMarketTrackBar);
            _itemControls.Add(ScenarioEditorItemId.SliderProfessionalArmy, professionalArmyTrackBar);
            _itemControls.Add(ScenarioEditorItemId.SliderDefenseLobby, defenseLobbyTrackBar);
            _itemControls.Add(ScenarioEditorItemId.SliderInterventionism, interventionismTrackBar);

            sliderYearTextBox.Tag = ScenarioEditorItemId.SliderYear;
            sliderMonthTextBox.Tag = ScenarioEditorItemId.SliderMonth;
            sliderDayTextBox.Tag = ScenarioEditorItemId.SliderDay;

            democraticTrackBar.Tag = ScenarioEditorItemId.SliderDemocratic;
            politicalLeftTrackBar.Tag = ScenarioEditorItemId.SliderPoliticalLeft;
            freedomTrackBar.Tag = ScenarioEditorItemId.SliderFreedom;
            freeMarketTrackBar.Tag = ScenarioEditorItemId.SliderFreeMarket;
            professionalArmyTrackBar.Tag = ScenarioEditorItemId.SliderProfessionalArmy;
            defenseLobbyTrackBar.Tag = ScenarioEditorItemId.SliderDefenseLobby;
            interventionismTrackBar.Tag = ScenarioEditorItemId.SliderInterventionism;

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
        }

        /// <summary>
        ///     政策スライダーの編集項目を有効化する
        /// </summary>
        private void EnablePoliticalSliderItems()
        {
            politicalSliderGroupBox.Enabled = true;
        }

        /// <summary>
        ///     政策スライダーの編集項目を無効化する
        /// </summary>
        private void DisablePoliticalSliderItems()
        {
            politicalSliderGroupBox.Enabled = false;
        }

        /// <summary>
        ///     政策スライダーの編集項目の表示を更新する
        /// </summary>
        /// <param name="settings">国家設定</param>
        private void UpdatePoliticalSliderItems(CountrySettings settings)
        {
            _controller.UpdateItemValue(sliderYearTextBox, settings);
            _controller.UpdateItemValue(sliderMonthTextBox, settings);
            _controller.UpdateItemValue(sliderDayTextBox, settings);

            _controller.UpdateItemColor(sliderYearTextBox, settings);
            _controller.UpdateItemColor(sliderMonthTextBox, settings);
            _controller.UpdateItemColor(sliderDayTextBox, settings);

            _controller.UpdateItemValue(democraticTrackBar, settings);
            _controller.UpdateItemValue(politicalLeftTrackBar, settings);
            _controller.UpdateItemValue(freedomTrackBar, settings);
            _controller.UpdateItemValue(freeMarketTrackBar, settings);
            _controller.UpdateItemValue(professionalArmyTrackBar, settings);
            _controller.UpdateItemValue(defenseLobbyTrackBar, settings);
            _controller.UpdateItemValue(interventionismTrackBar, settings);
        }

        /// <summary>
        ///     政策スライダーの編集項目の表示をクリアする
        /// </summary>
        private void ClearPoliticalSliderItems()
        {
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
        }

        #endregion

        #region 政府タブ - 閣僚

        /// <summary>
        ///     閣僚の編集項目を初期化する
        /// </summary>
        private void InitCabinetItems()
        {
            _itemControls.Add(ScenarioEditorItemId.CabinetHeadOfState, headOfStateComboBox);
            _itemControls.Add(ScenarioEditorItemId.CabinetHeadOfStateType, headOfStateTypeTextBox);
            _itemControls.Add(ScenarioEditorItemId.CabinetHeadOfStateId, headOfStateIdTextBox);
            _itemControls.Add(ScenarioEditorItemId.CabinetHeadOfGovernment, headOfGovernmentComboBox);
            _itemControls.Add(ScenarioEditorItemId.CabinetHeadOfGovernmentType, headOfGovernmentTypeTextBox);
            _itemControls.Add(ScenarioEditorItemId.CabinetHeadOfGovernmentId, headOfGovernmentIdTextBox);
            _itemControls.Add(ScenarioEditorItemId.CabinetForeignMinister, foreignMinisterComboBox);
            _itemControls.Add(ScenarioEditorItemId.CabinetForeignMinisterType, foreignMinisterTypeTextBox);
            _itemControls.Add(ScenarioEditorItemId.CabinetForeignMinisterId, foreignMinisterIdTextBox);
            _itemControls.Add(ScenarioEditorItemId.CabinetArmamentMinister, armamentMinisterComboBox);
            _itemControls.Add(ScenarioEditorItemId.CabinetArmamentMinisterType, armamentMinisterTypeTextBox);
            _itemControls.Add(ScenarioEditorItemId.CabinetArmamentMinisterId, armamentMinisterIdTextBox);
            _itemControls.Add(ScenarioEditorItemId.CabinetMinisterOfSecurity, ministerOfSecurityComboBox);
            _itemControls.Add(ScenarioEditorItemId.CabinetMinisterOfSecurityType, ministerOfSecurityTypeTextBox);
            _itemControls.Add(ScenarioEditorItemId.CabinetMinisterOfSecurityId, ministerOfSecurityIdTextBox);
            _itemControls.Add(ScenarioEditorItemId.CabinetMinisterOfIntelligence, ministerOfIntelligenceComboBox);
            _itemControls.Add(ScenarioEditorItemId.CabinetMinisterOfIntelligenceType, ministerOfIntelligenceTypeTextBox);
            _itemControls.Add(ScenarioEditorItemId.CabinetMinisterOfIntelligenceId, ministerOfIntelligenceIdTextBox);
            _itemControls.Add(ScenarioEditorItemId.CabinetChiefOfStaff, chiefOfStaffComboBox);
            _itemControls.Add(ScenarioEditorItemId.CabinetChiefOfStaffType, chiefOfStaffTypeTextBox);
            _itemControls.Add(ScenarioEditorItemId.CabinetChiefOfStaffId, chiefOfStaffIdTextBox);
            _itemControls.Add(ScenarioEditorItemId.CabinetChiefOfArmy, chiefOfArmyComboBox);
            _itemControls.Add(ScenarioEditorItemId.CabinetChiefOfArmyType, chiefOfArmyTypeTextBox);
            _itemControls.Add(ScenarioEditorItemId.CabinetChiefOfArmyId, chiefOfArmyIdTextBox);
            _itemControls.Add(ScenarioEditorItemId.CabinetChiefOfNavy, chiefOfNavyComboBox);
            _itemControls.Add(ScenarioEditorItemId.CabinetChiefOfNavyType, chiefOfNavyTypeTextBox);
            _itemControls.Add(ScenarioEditorItemId.CabinetChiefOfNavyId, chiefOfNavyIdTextBox);
            _itemControls.Add(ScenarioEditorItemId.CabinetChiefOfAir, chiefOfAirComboBox);
            _itemControls.Add(ScenarioEditorItemId.CabinetChiefOfAirType, chiefOfAirTypeTextBox);
            _itemControls.Add(ScenarioEditorItemId.CabinetChiefOfAirId, chiefOfAirIdTextBox);

            headOfStateComboBox.Tag = ScenarioEditorItemId.CabinetHeadOfState;
            headOfStateTypeTextBox.Tag = ScenarioEditorItemId.CabinetHeadOfStateType;
            headOfStateIdTextBox.Tag = ScenarioEditorItemId.CabinetHeadOfStateId;
            headOfGovernmentComboBox.Tag = ScenarioEditorItemId.CabinetHeadOfGovernment;
            headOfGovernmentTypeTextBox.Tag = ScenarioEditorItemId.CabinetHeadOfGovernmentType;
            headOfGovernmentIdTextBox.Tag = ScenarioEditorItemId.CabinetHeadOfGovernmentId;
            foreignMinisterComboBox.Tag = ScenarioEditorItemId.CabinetForeignMinister;
            foreignMinisterTypeTextBox.Tag = ScenarioEditorItemId.CabinetForeignMinisterType;
            foreignMinisterIdTextBox.Tag = ScenarioEditorItemId.CabinetForeignMinisterId;
            armamentMinisterComboBox.Tag = ScenarioEditorItemId.CabinetArmamentMinister;
            armamentMinisterTypeTextBox.Tag = ScenarioEditorItemId.CabinetArmamentMinisterType;
            armamentMinisterIdTextBox.Tag = ScenarioEditorItemId.CabinetArmamentMinisterId;
            ministerOfSecurityComboBox.Tag = ScenarioEditorItemId.CabinetMinisterOfSecurity;
            ministerOfSecurityTypeTextBox.Tag = ScenarioEditorItemId.CabinetMinisterOfSecurityType;
            ministerOfSecurityIdTextBox.Tag = ScenarioEditorItemId.CabinetMinisterOfSecurityId;
            ministerOfIntelligenceComboBox.Tag = ScenarioEditorItemId.CabinetMinisterOfIntelligence;
            ministerOfIntelligenceTypeTextBox.Tag = ScenarioEditorItemId.CabinetMinisterOfIntelligenceType;
            ministerOfIntelligenceIdTextBox.Tag = ScenarioEditorItemId.CabinetMinisterOfIntelligenceId;
            chiefOfStaffComboBox.Tag = ScenarioEditorItemId.CabinetChiefOfStaff;
            chiefOfStaffTypeTextBox.Tag = ScenarioEditorItemId.CabinetChiefOfStaffType;
            chiefOfStaffIdTextBox.Tag = ScenarioEditorItemId.CabinetChiefOfStaffId;
            chiefOfArmyComboBox.Tag = ScenarioEditorItemId.CabinetChiefOfArmy;
            chiefOfArmyTypeTextBox.Tag = ScenarioEditorItemId.CabinetChiefOfArmyType;
            chiefOfArmyIdTextBox.Tag = ScenarioEditorItemId.CabinetChiefOfArmyId;
            chiefOfNavyComboBox.Tag = ScenarioEditorItemId.CabinetChiefOfNavy;
            chiefOfNavyTypeTextBox.Tag = ScenarioEditorItemId.CabinetChiefOfNavyType;
            chiefOfNavyIdTextBox.Tag = ScenarioEditorItemId.CabinetChiefOfNavyId;
            chiefOfAirComboBox.Tag = ScenarioEditorItemId.CabinetChiefOfAir;
            chiefOfAirTypeTextBox.Tag = ScenarioEditorItemId.CabinetChiefOfAirType;
            chiefOfAirIdTextBox.Tag = ScenarioEditorItemId.CabinetChiefOfAirId;

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
        ///     閣僚の編集項目を有効化する
        /// </summary>
        private void EnableCabinetItems()
        {
            cabinetGroupBox.Enabled = true;
        }

        /// <summary>
        ///     閣僚の編集項目を無効化する
        /// </summary>
        private void DisableCabinetItems()
        {
            cabinetGroupBox.Enabled = false;
        }

        /// <summary>
        ///     閣僚の編集項目の表示を更新する
        /// </summary>
        /// <param name="country">選択国</param>
        /// <param name="settings">国家設定</param>
        /// <param name="year">対象年次</param>
        private void UpdateCabinetItems(Country country, CountrySettings settings, int year)
        {
            // 閣僚候補リストを更新する
            _controller.UpdateMinisterList(country, year);

            // 閣僚コンボボックスの表示を更新する
            _controller.UpdateItemValue(headOfStateComboBox, settings);
            _controller.UpdateItemValue(headOfGovernmentComboBox, settings);
            _controller.UpdateItemValue(foreignMinisterComboBox, settings);
            _controller.UpdateItemValue(armamentMinisterComboBox, settings);
            _controller.UpdateItemValue(ministerOfSecurityComboBox, settings);
            _controller.UpdateItemValue(ministerOfIntelligenceComboBox, settings);
            _controller.UpdateItemValue(chiefOfStaffComboBox, settings);
            _controller.UpdateItemValue(chiefOfArmyComboBox, settings);
            _controller.UpdateItemValue(chiefOfNavyComboBox, settings);
            _controller.UpdateItemValue(chiefOfAirComboBox, settings);

            // 閣僚type/idテキストボックスの表示を更新する
            _controller.UpdateItemValue(headOfStateTypeTextBox, settings);
            _controller.UpdateItemValue(headOfStateIdTextBox, settings);
            _controller.UpdateItemValue(headOfGovernmentTypeTextBox, settings);
            _controller.UpdateItemValue(headOfGovernmentIdTextBox, settings);
            _controller.UpdateItemValue(foreignMinisterTypeTextBox, settings);
            _controller.UpdateItemValue(foreignMinisterIdTextBox, settings);
            _controller.UpdateItemValue(armamentMinisterTypeTextBox, settings);
            _controller.UpdateItemValue(armamentMinisterIdTextBox, settings);
            _controller.UpdateItemValue(ministerOfSecurityTypeTextBox, settings);
            _controller.UpdateItemValue(ministerOfSecurityIdTextBox, settings);
            _controller.UpdateItemValue(ministerOfIntelligenceTypeTextBox, settings);
            _controller.UpdateItemValue(ministerOfIntelligenceIdTextBox, settings);
            _controller.UpdateItemValue(chiefOfStaffTypeTextBox, settings);
            _controller.UpdateItemValue(chiefOfStaffIdTextBox, settings);
            _controller.UpdateItemValue(chiefOfArmyTypeTextBox, settings);
            _controller.UpdateItemValue(chiefOfArmyIdTextBox, settings);
            _controller.UpdateItemValue(chiefOfNavyTypeTextBox, settings);
            _controller.UpdateItemValue(chiefOfNavyIdTextBox, settings);
            _controller.UpdateItemValue(chiefOfAirTypeTextBox, settings);
            _controller.UpdateItemValue(chiefOfAirIdTextBox, settings);

            // 閣僚type/idテキストボックスの色を更新する
            _controller.UpdateItemColor(headOfStateTypeTextBox, settings);
            _controller.UpdateItemColor(headOfStateIdTextBox, settings);
            _controller.UpdateItemColor(headOfGovernmentTypeTextBox, settings);
            _controller.UpdateItemColor(headOfGovernmentIdTextBox, settings);
            _controller.UpdateItemColor(foreignMinisterTypeTextBox, settings);
            _controller.UpdateItemColor(foreignMinisterIdTextBox, settings);
            _controller.UpdateItemColor(armamentMinisterTypeTextBox, settings);
            _controller.UpdateItemColor(armamentMinisterIdTextBox, settings);
            _controller.UpdateItemColor(ministerOfSecurityTypeTextBox, settings);
            _controller.UpdateItemColor(ministerOfSecurityIdTextBox, settings);
            _controller.UpdateItemColor(ministerOfIntelligenceTypeTextBox, settings);
            _controller.UpdateItemColor(ministerOfIntelligenceIdTextBox, settings);
            _controller.UpdateItemColor(chiefOfStaffTypeTextBox, settings);
            _controller.UpdateItemColor(chiefOfStaffIdTextBox, settings);
            _controller.UpdateItemColor(chiefOfArmyTypeTextBox, settings);
            _controller.UpdateItemColor(chiefOfArmyIdTextBox, settings);
            _controller.UpdateItemColor(chiefOfNavyTypeTextBox, settings);
            _controller.UpdateItemColor(chiefOfNavyIdTextBox, settings);
            _controller.UpdateItemColor(chiefOfAirTypeTextBox, settings);
            _controller.UpdateItemColor(chiefOfAirIdTextBox, settings);
        }

        /// <summary>
        ///     閣僚の編集項目の表示をクリアする
        /// </summary>
        private void ClearCabinetItems()
        {
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

        #endregion

        #region 政府タブ - 編集項目

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGovernmentIntItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中の国家がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, settings);
                return;
            }

            // 初期値から変更されていなければ何もしない
            if ((settings == null) && (val == 0))
            {
                return;
            }

            // 値に変化がなければ何もしない
            object prev = _controller.GetItemValue(itemId, settings);
            if ((prev != null) && (val == (int) prev))
            {
                return;
            }

            // 無効な値ならば値を戻す
            if (!_controller.IsItemValueValid(itemId, val, settings))
            {
                _controller.UpdateItemValue(control, settings);
                return;
            }

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            _controller.OutputItemValueChangedLog(itemId, val, settings);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, settings);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, settings);
        }

        /// <summary>
        ///     政策スライダースライドバーの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPoliticalSliderTrackBarScroll(object sender, EventArgs e)
        {
            // 選択中の国家がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            TrackBar control = (TrackBar) sender;
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            // 初期値から変更されていなければ何もしない
            int val = 11 - control.Value;
            object prev = _controller.GetItemValue(itemId, settings);
            if ((prev == null) && (val == 5))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((prev != null) && (val == (int) prev))
            {
                return;
            }

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            _controller.OutputItemValueChangedLog(itemId, val, settings);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, settings);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, settings);
        }

        /// <summary>
        ///     閣僚コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCabinetComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            ComboBox control = sender as ComboBox;
            if (control == null)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Country country = GetSelectedGovernmentCountry();
            CountrySettings settings = Scenarios.GetCountrySettings(country);
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            object val = _controller.GetItemValue(itemId, settings);
            object sel = _controller.GetListItemValue(itemId, e.Index);
            Brush brush = ((val != null) && (sel != null) && ((int) val == (int) sel) &&
                           _controller.IsItemDirty(itemId, settings))
                ? new SolidBrush(Color.Red)
                : new SolidBrush(SystemColors.WindowText);
            string s = control.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     閣僚コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCabinetComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            ComboBox control = (ComboBox) sender;
            int index = control.SelectedIndex;
            if (index < 0)
            {
                return;
            }

            // 選択中の国家がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }
            CountrySettings settings = Scenarios.GetCountrySettings(country);

            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            // 初期値から変更されていなければ何もしない
            object val = _controller.GetListItemValue(itemId, index);
            if (val == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            object prev = _controller.GetItemValue(itemId, settings);
            if ((prev != null) && ((int) val == (int) prev))
            {
                return;
            }

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            _controller.OutputItemValueChangedLog(itemId, val, settings);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, settings);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, settings);
        }

        #endregion

        #endregion

        #region 技術タブ

        #region 技術タブ - 共通

        /// <summary>
        ///     技術タブを初期化する
        /// </summary>
        private static void InitTechTab()
        {
            // 何もしない
        }

        /// <summary>
        ///     技術タブを更新する
        /// </summary>
        private void UpdateTechTab()
        {
            // 初期化済みであれば何もしない
            if (_tabPageInitialized[(int) TabPageNo.Technology])
            {
                return;
            }

            // 技術カテゴリリストボックスを初期化する
            InitTechCategoryListBox();

            // 技術カテゴリリストボックスを有効化する
            EnableTechCategoryListBox();

            // 国家リストボックスを更新する
            UpdateCountryListBox(techCountryListBox);

            // 国家リストボックスを有効化する
            EnableTechCountryListBox();

            // 初期化済みフラグをセットする
            _tabPageInitialized[(int) TabPageNo.Technology] = true;
        }

        /// <summary>
        ///     技術タブのフォーム読み込み時の処理
        /// </summary>
        private static void OnTechTabPageFormLoad()
        {
            // 技術タブを初期化する
            InitTechTab();
        }

        /// <summary>
        ///     技術タブのファイル読み込み時の処理
        /// </summary>
        private void OnTechTabPageFileLoad()
        {
            // 政府タブ選択中でなければ何もしない
            if (_tabPageNo != TabPageNo.Technology)
            {
                return;
            }

            // 技術データの読み込み完了まで待機する
            WaitLoadingTechs();

            // 初回遷移時には表示を更新する
            UpdateTechTab();
        }

        /// <summary>
        ///     技術タブ選択時の処理
        /// </summary>
        private void OnTechTabPageSelected()
        {
            // シナリオ未読み込みならば何もしない
            if (!Scenarios.IsLoaded())
            {
                return;
            }

            // 技術データの読み込み完了まで待機する
            WaitLoadingMinisters();

            // 初回遷移時には表示を更新する
            UpdateTechTab();
        }

        #endregion

        #region 技術タブ - 技術カテゴリ

        /// <summary>
        ///     技術カテゴリリストボックスを初期化する
        /// </summary>
        private void InitTechCategoryListBox()
        {
            techCategoryListBox.BeginUpdate();
            techCategoryListBox.Items.Clear();
            foreach (TechGroup grp in Techs.Groups)
            {
                techCategoryListBox.Items.Add(grp);
            }
            techCategoryListBox.SelectedIndex = 0;
            techCategoryListBox.EndUpdate();
        }

        /// <summary>
        ///     技術カテゴリリストボックスを有効化する
        /// </summary>
        private void EnableTechCategoryListBox()
        {
            techCategoryListBox.Enabled = true;
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
                // 編集項目を無効化する
                DisableTechItems();

                // 編集項目をクリアする
                ClearTechItems();
                return;
            }

            // 編集項目を更新する
            UpdateTechItems();

            // 編集項目を有効化する
            EnableTechItems();
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

        #endregion

        #region 技術タブ - 国家

        /// <summary>
        ///     国家リストボックスを有効化する
        /// </summary>
        private void EnableTechCountryListBox()
        {
            techCountryListBox.Enabled = true;
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
                // 編集項目を無効化する
                DisableTechItems();

                // 編集項目をクリアする
                ClearTechItems();
                return;
            }

            // 編集項目を更新する
            UpdateTechItems();

            // 編集項目を有効化する
            EnableTechItems();
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
        ///     技術の編集項目を有効化する
        /// </summary>
        private void EnableTechItems()
        {
            ownedTechsLabel.Enabled = true;
            ownedTechsListView.Enabled = true;
            blueprintsLabel.Enabled = true;
            blueprintsListView.Enabled = true;
            inventionsLabel.Enabled = true;
            inventionsListView.Enabled = true;
        }

        /// <summary>
        ///     技術の編集項目を無効化する
        /// </summary>
        private void DisableTechItems()
        {
            ownedTechsLabel.Enabled = false;
            ownedTechsListView.Enabled = false;
            blueprintsLabel.Enabled = false;
            blueprintsListView.Enabled = false;
            inventionsLabel.Enabled = false;
            inventionsListView.Enabled = false;
        }

        /// <summary>
        ///     技術の編集項目の表示を更新する
        /// </summary>
        private void UpdateTechItems()
        {
            Country country = GetSelectedTechCountry();
            CountrySettings settings = Scenarios.GetCountrySettings(country);
            TechGroup grp = GetSelectedTechGroup();

            // 保有技術リスト
            _techs = grp.Items.OfType<TechItem>().ToList();
            UpdateOwnedTechList(settings);

            // 青写真リスト
            UpdateBlueprintList(settings);

            // 発明イベントリスト
            _inventions = Techs.Groups.SelectMany(g => g.Items.OfType<TechEvent>()).ToList();
            UpdateInventionList(settings);

            // 技術ツリーを更新する
            _techTreePanelController.Category = grp.Category;
            _techTreePanelController.Update();
        }

        /// <summary>
        ///     技術の編集項目の表示をクリアする
        /// </summary>
        private void ClearTechItems()
        {
            // 技術ツリーをクリアする
            _techTreePanelController.Clear();

            // 編集項目をクリアする
            ownedTechsListView.Items.Clear();
            blueprintsListView.Items.Clear();
            inventionsListView.Items.Clear();
        }

        /// <summary>
        ///     保有技術リストの表示を更新する
        /// </summary>
        /// <param name="settings">国家設定</param>
        private void UpdateOwnedTechList(CountrySettings settings)
        {
            ownedTechsListView.ItemChecked -= OnOwnedTechsListViewItemChecked;
            ownedTechsListView.BeginUpdate();
            ownedTechsListView.Items.Clear();
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
                }
            }
            else
            {
                foreach (TechItem item in _techs)
                {
                    string name = item.ToString();
                    ownedTechsListView.Items.Add(new ListViewItem { Text = name, Tag = item });
                }
            }
            ownedTechsListView.EndUpdate();
            ownedTechsListView.ItemChecked += OnOwnedTechsListViewItemChecked;
        }

        /// <summary>
        ///     青写真リストの表示を更新する
        /// </summary>
        /// <param name="settings">国家設定</param>
        private void UpdateBlueprintList(CountrySettings settings)
        {
            blueprintsListView.ItemChecked -= OnBlueprintsListViewItemChecked;
            blueprintsListView.BeginUpdate();
            blueprintsListView.Items.Clear();
            if (settings != null)
            {
                foreach (TechItem item in _techs)
                {
                    string name = item.ToString();
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
                    blueprintsListView.Items.Add(new ListViewItem { Text = name, Tag = item });
                }
            }
            blueprintsListView.EndUpdate();
            blueprintsListView.ItemChecked += OnBlueprintsListViewItemChecked;
        }

        /// <summary>
        ///     発明イベントリストの表示を更新する
        /// </summary>
        /// <param name="settings">国家設定</param>
        private void UpdateInventionList(CountrySettings settings)
        {
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
        }

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
            _techTreePanelController.UpdateTechTreeItem(item);
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
            _techTreePanelController.UpdateTechTreeItem(item);
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
            _techTreePanelController.UpdateTechTreeItem(ev);
        }

        #endregion

        #region 技術タブ - 技術ツリー

        /// <summary>
        ///     項目ラベルマウスクリック時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechTreeItemMouseClick(object sender, TechTreePanelController.ItemMouseEventArgs e)
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
            _techTreePanelController.UpdateTechTreeItem(item);

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
            _techTreePanelController.UpdateTechTreeItem(item);

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
            _techTreePanelController.UpdateTechTreeItem(item);

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
        private void OnQueryTechTreeItemStatus(object sender, TechTreePanelController.QueryItemStatusEventArgs e)
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

        #region プロヴィンスタブ

        #region プロヴィンスタブ - 共通

        /// <summary>
        ///     プロヴィンスタブを初期化する
        /// </summary>
        private void InitProvinceTab()
        {
            InitMapFilter();
            InitProvinceIdTextBox();
            InitProvinceCountryItems();
            InitProvinceInfoItems();
            InitProvinceResourceItems();
            InitProvinceBuildingItems();
        }

        /// <summary>
        ///     プロヴィンスタブの表示を更新する
        /// </summary>
        private void UpdateProvinceTab()
        {
            // 初期化済みであれば何もしない
            if (_tabPageInitialized[(int) TabPageNo.Province])
            {
                return;
            }

            // プロヴィンスリストを初期化する
            _controller.InitProvinceList(provinceListView);

            // 国家フィルターを更新する
            UpdateProvinceCountryFilter();

            // プロヴィンスリストを有効化する
            EnableProvinceList();

            // 国家フィルターを有効化する
            EnableProvinceCountryFilter();

            // IDテキストボックスを有効化する
            EnableProvinceIdTextBox();

            // 初期化済みフラグをセットする
            _tabPageInitialized[(int) TabPageNo.Province] = true;
        }

        /// <summary>
        ///     プロヴィンスタブのフォーム読み込み時の処理
        /// </summary>
        private void OnProvinceTabPageFormLoad()
        {
            // プロヴィンスタブを初期化する
            InitProvinceTab();
        }

        /// <summary>
        ///     プロヴィンスタブのファイル読み込み時の処理
        /// </summary>
        private void OnProvinceTabPageFileLoad()
        {
            // プロヴィンスタブ選択中でなければ何もしない
            if (_tabPageNo != TabPageNo.Province)
            {
                return;
            }

            // プロヴィンスデータの読み込み完了まで待機する
            WaitLoadingProvinces();

            // 初回遷移時には表示を更新する
            UpdateProvinceTab();
        }

        /// <summary>
        ///     プロヴィンスタブ選択時の処理
        /// </summary>
        private void OnProvinceTabPageSelected()
        {
            // シナリオ未読み込みならば何もしない
            if (!Scenarios.IsLoaded())
            {
                return;
            }

            // プロヴィンスデータの読み込み完了まで待機する
            WaitLoadingProvinces();

            // 初回遷移時には表示を更新する
            UpdateProvinceTab();
        }

        #endregion

        #region プロヴィンスタブ - マップ

        /// <summary>
        ///     マップパネルを初期化する
        /// </summary>
        private void InitMapPanel()
        {
            _mapPanelController.ProvinceMouseClick += OnMapPanelMouseClick;
            _mapPanelController.Show();
        }

        /// <summary>
        ///     マップパネルのマウスクリック時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMapPanelMouseClick(object sender, MapPanelController.ProvinceEventArgs e)
        {
            // 左クリック以外では何もしない
            if (e.MouseEvent.Button != MouseButtons.Left)
            {
                return;
            }

            // 陸地プロヴィンスでなければ何もしない
            if (_controller.GetLandProvinceIndex(e.Id) < 0)
            {
                return;
            }

            // 選択中のプロヴィンスIDを更新する
            provinceIdTextBox.Text = IntHelper.ToString(e.Id);

            // プロヴィンスを選択する
            SelectProvince(e.Id);

            Country country = GetSelectedProvinceCountry();
            if (country == Country.None)
            {
                return;
            }

            switch (_mapPanelController.FilterMode)
            {
                case MapPanelController.MapFilterMode.Core:
                    coreProvinceCheckBox.Checked = !coreProvinceCheckBox.Checked;
                    break;

                case MapPanelController.MapFilterMode.Owned:
                    ownedProvinceCheckBox.Checked = !ownedProvinceCheckBox.Checked;
                    break;

                case MapPanelController.MapFilterMode.Controlled:
                    controlledProvinceCheckBox.Checked = !controlledProvinceCheckBox.Checked;
                    break;

                case MapPanelController.MapFilterMode.Claimed:
                    claimedProvinceCheckBox.Checked = !claimedProvinceCheckBox.Checked;
                    break;
            }
        }

        #endregion

        #region プロヴィンスタブ - マップフィルター

        /// <summary>
        ///     マップフィルターを初期化する
        /// </summary>
        private void InitMapFilter()
        {
            mapFilterNoneRadioButton.Tag = MapPanelController.MapFilterMode.None;
            mapFilterCoreRadioButton.Tag = MapPanelController.MapFilterMode.Core;
            mapFilterOwnedRadioButton.Tag = MapPanelController.MapFilterMode.Owned;
            mapFilterControlledRadioButton.Tag = MapPanelController.MapFilterMode.Controlled;
            mapFilterClaimedRadioButton.Tag = MapPanelController.MapFilterMode.Claimed;
        }

        /// <summary>
        ///     マップフィルターを有効化する
        /// </summary>
        private void EnableMapFilter()
        {
            mapFilterGroupBox.Enabled = true;
        }

        /// <summary>
        ///     マップフィルターラジオボタンのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMapFilterRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton == null)
            {
                return;
            }

            // チェックなしの時には他の項目にチェックがついているので処理しない
            if (!radioButton.Checked)
            {
                return;
            }

            // フィルターモードを更新する
            _mapPanelController.FilterMode = (MapPanelController.MapFilterMode) radioButton.Tag;
        }

        #endregion

        #region プロヴィンスタブ - 国家フィルター

        /// <summary>
        ///     国家フィルターを有効化する
        /// </summary>
        private void EnableProvinceCountryFilter()
        {
            provinceCountryFilterLabel.Enabled = true;
            provinceCountryFilterComboBox.Enabled = true;
        }

        /// <summary>
        ///     国家フィルターを更新する
        /// </summary>
        private void UpdateProvinceCountryFilter()
        {
            provinceCountryFilterComboBox.BeginUpdate();
            provinceCountryFilterComboBox.Items.Clear();
            provinceCountryFilterComboBox.Items.Add("");
            foreach (Country country in Countries.Tags)
            {
                provinceCountryFilterComboBox.Items.Add(GetCountryTagName(country));
            }
            provinceCountryFilterComboBox.EndUpdate();
        }

        /// <summary>
        ///     国家フィルターの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProvinceCountryFilterComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            Country country = GetSelectedProvinceCountry();

            // プロヴィンスリストを更新する
            _controller.UpdateProvinceList(provinceListView, country);

            // マップフィルターを更新する
            _mapPanelController.SelectedCountry = country;

            // プロヴィンス国家グループボックスの編集項目の表示を更新する
            Province province = GetSelectedProvince();
            if ((country != Country.None) && (province != null))
            {
                CountrySettings settings = Scenarios.GetCountrySettings(country);
                UpdateProvinceCountryItems(province, settings);
                EnableProvinceCountryItems();
            }
            else
            {
                DisableProvinceCountryItems();
                ClearProvinceCountryItems();
            }
        }

        /// <summary>
        ///     選択国を取得する
        /// </summary>
        /// <returns>選択国</returns>
        private Country GetSelectedProvinceCountry()
        {
            if (provinceCountryFilterComboBox.SelectedIndex <= 0)
            {
                return Country.None;
            }
            return Countries.Tags[provinceCountryFilterComboBox.SelectedIndex - 1];
        }

        #endregion

        #region プロヴィンスタブ - プロヴィンスID

        /// <summary>
        ///     プロヴィンスIDテキストボックスを初期化する
        /// </summary>
        private void InitProvinceIdTextBox()
        {
            _itemControls.Add(ScenarioEditorItemId.ProvinceId, provinceIdTextBox);

            provinceIdTextBox.Tag = ScenarioEditorItemId.ProvinceId;
        }

        /// <summary>
        ///     プロヴィンスIDテキストボックスを有効化する
        /// </summary>
        private void EnableProvinceIdTextBox()
        {
            provinceIdLabel.Enabled = true;
            provinceIdTextBox.Enabled = true;
        }

        /// <summary>
        ///     プロヴィンスIDテキストボックスのID変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProvinceIdTextBoxValidated(object sender, EventArgs e)
        {
            Province province = GetSelectedProvince();

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(provinceIdTextBox.Text, out val))
            {
                if (province != null)
                {
                    provinceIdTextBox.Text = IntHelper.ToString(province.Id);
                }
                return;
            }

            // 値に変化がなければ何もしない
            if (val == province.Id)
            {
                return;
            }

            // プロヴィンスを選択する
            SelectProvince(val);
        }

        /// <summary>
        ///     プロヴィンスを選択する
        /// </summary>
        /// <param name="id">プロヴィンスID</param>
        private void SelectProvince(int id)
        {
            // プロヴィンスリストビューの選択項目を変更する
            int index = _controller.GetLandProvinceIndex(id);
            if (index >= 0)
            {
                ListViewItem item = provinceListView.Items[index];
                item.Focused = true;
                item.Selected = true;
                item.EnsureVisible();
            }
        }

        #endregion

        #region プロヴィンスタブ - プロヴィンスリスト

        /// <summary>
        ///     プロヴィンスリストを有効化する
        /// </summary>
        private void EnableProvinceList()
        {
            provinceListView.Enabled = true;
        }

        /// <summary>
        ///     プロヴィンスリストビューの項目を追加する
        /// </summary>
        /// <param name="item">追加する項目</param>
        public void AddProvinceListItem(ListViewItem item)
        {
            provinceListView.Items.Add(item);
        }

        /// <summary>
        ///     プロヴィンスリストビューの項目文字列を設定する
        /// </summary>
        /// <param name="index">プロヴィンスリストビューのインデックス</param>
        /// <param name="no">項目番号</param>
        /// <param name="s">文字列</param>
        public void SetProvinceListItemText(int index, int no, string s)
        {
            provinceListView.Items[index].SubItems[no].Text = s;
        }

        /// <summary>
        ///     選択中のプロヴィンスを取得する
        /// </summary>
        /// <returns></returns>
        private Province GetSelectedProvince()
        {
            if (provinceListView.SelectedIndices.Count == 0)
            {
                return null;
            }
            return provinceListView.SelectedItems[0].Tag as Province;
        }

        /// <summary>
        ///     プロヴィンスリストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProvinceListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ編集項目を無効化する
            Province province = GetSelectedProvince();
            if (province == null)
            {
                // 編集項目を無効化する
                DisableProvinceCountryItems();
                DisableProvinceInfoItems();
                DisableProvinceResourceItems();
                DisableProvinceBuildingItems();

                // 編集項目の表示をクリアする
                ClearProvinceCountryItems();
                ClearProvinceInfoItems();
                ClearProvinceResourceItems();
                ClearProvinceBuildingItems();
                return;
            }

            ProvinceSettings settings = Scenarios.GetProvinceSettings(province.Id);
            Country country = GetSelectedProvinceCountry();
            CountrySettings countrySettings = Scenarios.GetCountrySettings(country);

            // 編集項目の表示を更新する
            UpdateProvinceCountryItems(province, countrySettings);
            UpdateProvinceInfoItems(province, settings);
            UpdateProvinceResourceItems(settings);
            UpdateProvinceBuildingItems(settings);

            // 編集項目を有効化する
            if (country != Country.None)
            {
                EnableProvinceCountryItems();
            }
            EnableProvinceInfoItems();
            EnableProvinceResourceItems();
            EnableProvinceBuildingItems();

            // マップをスクロールさせる
            _mapPanelController.ScrollToProvince(province.Id);
        }

        #endregion

        #region プロヴィンスタブ - 国家情報

        /// <summary>
        ///     プロヴィンス国家情報の編集項目を初期化する
        /// </summary>
        private void InitProvinceCountryItems()
        {
            _itemControls.Add(ScenarioEditorItemId.CountryCapital, capitalCheckBox);
            _itemControls.Add(ScenarioEditorItemId.CountryCoreProvinces, coreProvinceCheckBox);
            _itemControls.Add(ScenarioEditorItemId.CountryOwnedProvinces, ownedProvinceCheckBox);
            _itemControls.Add(ScenarioEditorItemId.CountryControlledProvinces, controlledProvinceCheckBox);
            _itemControls.Add(ScenarioEditorItemId.CountryClaimedProvinces, claimedProvinceCheckBox);

            capitalCheckBox.Tag = ScenarioEditorItemId.CountryCapital;
            coreProvinceCheckBox.Tag = ScenarioEditorItemId.CountryCoreProvinces;
            ownedProvinceCheckBox.Tag = ScenarioEditorItemId.CountryOwnedProvinces;
            controlledProvinceCheckBox.Tag = ScenarioEditorItemId.CountryControlledProvinces;
            claimedProvinceCheckBox.Tag = ScenarioEditorItemId.CountryClaimedProvinces;
        }

        /// <summary>
        ///     プロヴィンス国家情報の編集項目を有効化する
        /// </summary>
        private void EnableProvinceCountryItems()
        {
            provinceCountryGroupBox.Enabled = true;
        }

        /// <summary>
        ///     プロヴィンス国家情報の編集項目を無効化する
        /// </summary>
        private void DisableProvinceCountryItems()
        {
            provinceCountryGroupBox.Enabled = false;
        }

        /// <summary>
        ///     プロヴィンス国家情報の編集項目の表示を更新する
        /// </summary>
        /// <param name="province">プロヴィンス</param>
        /// <param name="settings">国家設定</param>
        private void UpdateProvinceCountryItems(Province province, CountrySettings settings)
        {
            _controller.UpdateItemValue(capitalCheckBox, province, settings);
            _controller.UpdateItemValue(coreProvinceCheckBox, province, settings);
            _controller.UpdateItemValue(ownedProvinceCheckBox, province, settings);
            _controller.UpdateItemValue(controlledProvinceCheckBox, province, settings);
            _controller.UpdateItemValue(claimedProvinceCheckBox, province, settings);

            _controller.UpdateItemColor(capitalCheckBox, province, settings);
            _controller.UpdateItemColor(coreProvinceCheckBox, province, settings);
            _controller.UpdateItemColor(ownedProvinceCheckBox, province, settings);
            _controller.UpdateItemColor(controlledProvinceCheckBox, province, settings);
            _controller.UpdateItemColor(claimedProvinceCheckBox, province, settings);
        }

        /// <summary>
        ///     プロヴィンス国家情報の編集項目の表示をクリアする
        /// </summary>
        private void ClearProvinceCountryItems()
        {
            capitalCheckBox.Checked = false;
            coreProvinceCheckBox.Checked = false;
            ownedProvinceCheckBox.Checked = false;
            controlledProvinceCheckBox.Checked = false;
            claimedProvinceCheckBox.Checked = false;
        }

        #endregion

        #region プロヴィンスタブ - プロヴィンス情報

        /// <summary>
        ///     プロヴィンス情報の編集項目を初期化する
        /// </summary>
        private void InitProvinceInfoItems()
        {
            _itemControls.Add(ScenarioEditorItemId.ProvinceName, provinceNameTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceVp, vpTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceRevoltRisk, revoltRiskTextBox);

            provinceNameTextBox.Tag = ScenarioEditorItemId.ProvinceName;
            vpTextBox.Tag = ScenarioEditorItemId.ProvinceVp;
            revoltRiskTextBox.Tag = ScenarioEditorItemId.ProvinceRevoltRisk;
        }

        /// <summary>
        ///     プロヴィンス情報の編集項目を有効化する
        /// </summary>
        private void EnableProvinceInfoItems()
        {
            provinceInfoGroupBox.Enabled = true;
        }

        /// <summary>
        ///     プロヴィンス情報の編集項目を無効化する
        /// </summary>
        private void DisableProvinceInfoItems()
        {
            provinceInfoGroupBox.Enabled = false;
        }

        /// <summary>
        ///     プロヴィンス情報の編集項目の表示を更新する
        /// </summary>
        /// <param name="province">プロヴィンス</param>
        /// <param name="settings">プロヴィンス設定</param>
        private void UpdateProvinceInfoItems(Province province, ProvinceSettings settings)
        {
            _controller.UpdateItemValue(provinceIdTextBox, province);
            _controller.UpdateItemValue(provinceNameTextBox, province, settings);
            _controller.UpdateItemValue(vpTextBox, settings);
            _controller.UpdateItemValue(revoltRiskTextBox, settings);

            _controller.UpdateItemColor(provinceNameTextBox, province, settings);
            _controller.UpdateItemColor(vpTextBox, settings);
            _controller.UpdateItemColor(revoltRiskTextBox, settings);
        }

        /// <summary>
        ///     プロヴィンス情報の編集項目の表示をクリアする
        /// </summary>
        private void ClearProvinceInfoItems()
        {
            provinceIdTextBox.Text = "";
            provinceNameTextBox.Text = "";
            vpTextBox.Text = "";
            revoltRiskTextBox.Text = "";
        }

        #endregion

        #region プロヴィンスタブ - 資源情報

        /// <summary>
        ///     プロヴィンス資源情報の編集項目を初期化する
        /// </summary>
        private void InitProvinceResourceItems()
        {
            // 資源名ラベル
            provinceManpowerLabel.Text = Config.GetText(TextId.ResourceManpower);
            provinceEnergyLabel.Text = Config.GetText(TextId.ResourceEnergy);
            provinceMetalLabel.Text = Config.GetText(TextId.ResourceMetal);
            provinceRareMaterialsLabel.Text = Config.GetText(TextId.ResourceRareMaterials);
            provinceOilLabel.Text = Config.GetText(TextId.ResourceOil);
            provinceSuppliesLabel.Text = Config.GetText(TextId.ResourceSupplies);

            // 編集項目
            _itemControls.Add(ScenarioEditorItemId.ProvinceManpowerCurrent, manpowerCurrentTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceManpowerMax, manpowerMaxTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceEnergyPool, energyPoolTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceEnergyCurrent, energyCurrentTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceEnergyMax, energyMaxTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceMetalPool, metalPoolTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceMetalCurrent, metalCurrentTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceMetalMax, metalMaxTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceRareMaterialsPool, rareMaterialsPoolTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceRareMaterialsCurrent, rareMaterialsCurrentTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceRareMaterialsMax, rareMaterialsMaxTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceOilPool, oilPoolTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceOilCurrent, oilCurrentTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceOilMax, oilMaxTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceSupplyPool, suppliesPoolTextBox);

            manpowerCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceManpowerCurrent;
            manpowerMaxTextBox.Tag = ScenarioEditorItemId.ProvinceManpowerMax;
            energyPoolTextBox.Tag = ScenarioEditorItemId.ProvinceEnergyPool;
            energyCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceEnergyCurrent;
            energyMaxTextBox.Tag = ScenarioEditorItemId.ProvinceEnergyMax;
            metalPoolTextBox.Tag = ScenarioEditorItemId.ProvinceMetalPool;
            metalCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceMetalCurrent;
            metalMaxTextBox.Tag = ScenarioEditorItemId.ProvinceMetalMax;
            rareMaterialsPoolTextBox.Tag = ScenarioEditorItemId.ProvinceRareMaterialsPool;
            rareMaterialsCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceRareMaterialsCurrent;
            rareMaterialsMaxTextBox.Tag = ScenarioEditorItemId.ProvinceRareMaterialsMax;
            oilPoolTextBox.Tag = ScenarioEditorItemId.ProvinceOilPool;
            oilCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceOilCurrent;
            oilMaxTextBox.Tag = ScenarioEditorItemId.ProvinceOilMax;
            suppliesPoolTextBox.Tag = ScenarioEditorItemId.ProvinceSupplyPool;
        }

        /// <summary>
        ///     プロヴィンス資源情報の編集項目を有効化する
        /// </summary>
        private void EnableProvinceResourceItems()
        {
            provinceResourceGroupBox.Enabled = true;
        }

        /// <summary>
        ///     プロヴィンス資源情報の編集項目を無効化する
        /// </summary>
        private void DisableProvinceResourceItems()
        {
            provinceResourceGroupBox.Enabled = false;
        }

        /// <summary>
        ///     プロヴィンス資源情報の編集項目の表示を更新する
        /// </summary>
        /// <param name="settings">プロヴィンス設定</param>
        private void UpdateProvinceResourceItems(ProvinceSettings settings)
        {
            _controller.UpdateItemValue(manpowerCurrentTextBox, settings);
            _controller.UpdateItemValue(manpowerMaxTextBox, settings);
            _controller.UpdateItemValue(energyPoolTextBox, settings);
            _controller.UpdateItemValue(energyCurrentTextBox, settings);
            _controller.UpdateItemValue(energyMaxTextBox, settings);
            _controller.UpdateItemValue(metalPoolTextBox, settings);
            _controller.UpdateItemValue(metalCurrentTextBox, settings);
            _controller.UpdateItemValue(metalMaxTextBox, settings);
            _controller.UpdateItemValue(rareMaterialsPoolTextBox, settings);
            _controller.UpdateItemValue(rareMaterialsCurrentTextBox, settings);
            _controller.UpdateItemValue(rareMaterialsMaxTextBox, settings);
            _controller.UpdateItemValue(oilPoolTextBox, settings);
            _controller.UpdateItemValue(oilCurrentTextBox, settings);
            _controller.UpdateItemValue(oilMaxTextBox, settings);
            _controller.UpdateItemValue(suppliesPoolTextBox, settings);

            _controller.UpdateItemColor(manpowerCurrentTextBox, settings);
            _controller.UpdateItemColor(manpowerMaxTextBox, settings);
            _controller.UpdateItemColor(energyPoolTextBox, settings);
            _controller.UpdateItemColor(energyCurrentTextBox, settings);
            _controller.UpdateItemColor(energyMaxTextBox, settings);
            _controller.UpdateItemColor(metalPoolTextBox, settings);
            _controller.UpdateItemColor(metalCurrentTextBox, settings);
            _controller.UpdateItemColor(metalMaxTextBox, settings);
            _controller.UpdateItemColor(rareMaterialsPoolTextBox, settings);
            _controller.UpdateItemColor(rareMaterialsCurrentTextBox, settings);
            _controller.UpdateItemColor(rareMaterialsMaxTextBox, settings);
            _controller.UpdateItemColor(oilPoolTextBox, settings);
            _controller.UpdateItemColor(oilCurrentTextBox, settings);
            _controller.UpdateItemColor(oilMaxTextBox, settings);
            _controller.UpdateItemColor(suppliesPoolTextBox, settings);
        }

        /// <summary>
        ///     プロヴィンス資源情報の編集項目の表示をクリアする
        /// </summary>
        private void ClearProvinceResourceItems()
        {
            manpowerCurrentTextBox.Text = "";
            manpowerMaxTextBox.Text = "";
            energyPoolTextBox.Text = "";
            energyCurrentTextBox.Text = "";
            energyMaxTextBox.Text = "";
            metalPoolTextBox.Text = "";
            metalCurrentTextBox.Text = "";
            metalMaxTextBox.Text = "";
            rareMaterialsPoolTextBox.Text = "";
            rareMaterialsCurrentTextBox.Text = "";
            rareMaterialsMaxTextBox.Text = "";
            oilPoolTextBox.Text = "";
            oilCurrentTextBox.Text = "";
            oilMaxTextBox.Text = "";
            suppliesPoolTextBox.Text = "";
        }

        #endregion

        #region プロヴィンスタブ - 建物情報

        /// <summary>
        ///     プロヴィンス建物情報の編集項目を初期化する
        /// </summary>
        private void InitProvinceBuildingItems()
        {
            _itemControls.Add(ScenarioEditorItemId.ProvinceIcCurrent, icCurrentTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceIcMax, icMaxTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceIcRelative, icRelativeTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceInfrastructureCurrent, infrastructureCurrentTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceInfrastructureMax, infrastructureMaxTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceInfrastructureRelative, infrastructureRelativeTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceLandFortCurrent, landFortCurrentTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceLandFortMax, landFortMaxTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceLandFortRelative, landFortRelativeTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceCoastalFortCurrent, coastalFortCurrentTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceCoastalFortMax, coastalFortMaxTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceCoastalFortRelative, coastalFortRelativeTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceAntiAirCurrent, antiAirCurrentTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceAntiAirMax, antiAirMaxTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceAntiAirRelative, antiAirRelativeTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceAirBaseCurrent, airBaseCurrentTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceAirBaseMax, airBaseMaxTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceAirBaseRelative, airBaseRelativeTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceNavalBaseCurrent, navalBaseCurrentTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceNavalBaseMax, navalBaseMaxTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceNavalBaseRelative, navalBaseRelativeTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceRadarStationCurrent, radarStationCurrentTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceRadarStationMax, radarStationMaxTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceRadarStationRelative, radarStationRelativeTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceNuclearReactorCurrent, nuclearReactorCurrentTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceNuclearReactorMax, nuclearReactorMaxTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceNuclearReactorRelative, nuclearReactorRelativeTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceRocketTestCurrent, rocketTestCurrentTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceRocketTestMax, rocketTestMaxTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceRocketTestRelative, rocketTestRelativeTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceSyntheticOilCurrent, syntheticOilCurrentTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceSyntheticOilMax, syntheticOilMaxTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceSyntheticOilRelative, syntheticOilRelativeTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceSyntheticRaresCurrent, syntheticRaresCurrentTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceSyntheticRaresMax, syntheticRaresMaxTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceSyntheticRaresRelative, syntheticRaresRelativeTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceNuclearPowerCurrent, nuclearPowerCurrentTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceNuclearPowerMax, nuclearPowerMaxTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceNuclearPowerRelative, nuclearPowerRelativeTextBox);

            icCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceIcCurrent;
            icMaxTextBox.Tag = ScenarioEditorItemId.ProvinceIcMax;
            icRelativeTextBox.Tag = ScenarioEditorItemId.ProvinceIcRelative;
            infrastructureCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceInfrastructureCurrent;
            infrastructureMaxTextBox.Tag = ScenarioEditorItemId.ProvinceInfrastructureMax;
            infrastructureRelativeTextBox.Tag = ScenarioEditorItemId.ProvinceInfrastructureRelative;
            landFortCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceLandFortCurrent;
            landFortMaxTextBox.Tag = ScenarioEditorItemId.ProvinceLandFortMax;
            landFortRelativeTextBox.Tag = ScenarioEditorItemId.ProvinceLandFortRelative;
            coastalFortCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceCoastalFortCurrent;
            coastalFortMaxTextBox.Tag = ScenarioEditorItemId.ProvinceCoastalFortMax;
            coastalFortRelativeTextBox.Tag = ScenarioEditorItemId.ProvinceCoastalFortRelative;
            antiAirCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceAntiAirCurrent;
            antiAirMaxTextBox.Tag = ScenarioEditorItemId.ProvinceAntiAirMax;
            antiAirRelativeTextBox.Tag = ScenarioEditorItemId.ProvinceAntiAirRelative;
            airBaseCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceAirBaseCurrent;
            airBaseMaxTextBox.Tag = ScenarioEditorItemId.ProvinceAirBaseMax;
            airBaseRelativeTextBox.Tag = ScenarioEditorItemId.ProvinceAirBaseRelative;
            navalBaseCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceNavalBaseCurrent;
            navalBaseMaxTextBox.Tag = ScenarioEditorItemId.ProvinceNavalBaseMax;
            navalBaseRelativeTextBox.Tag = ScenarioEditorItemId.ProvinceNavalBaseRelative;
            radarStationCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceRadarStationCurrent;
            radarStationMaxTextBox.Tag = ScenarioEditorItemId.ProvinceRadarStationMax;
            radarStationRelativeTextBox.Tag = ScenarioEditorItemId.ProvinceRadarStationRelative;
            nuclearReactorCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceNuclearReactorCurrent;
            nuclearReactorMaxTextBox.Tag = ScenarioEditorItemId.ProvinceNuclearReactorMax;
            nuclearReactorRelativeTextBox.Tag = ScenarioEditorItemId.ProvinceNuclearReactorRelative;
            rocketTestCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceRocketTestCurrent;
            rocketTestMaxTextBox.Tag = ScenarioEditorItemId.ProvinceRocketTestMax;
            rocketTestRelativeTextBox.Tag = ScenarioEditorItemId.ProvinceRocketTestRelative;
            syntheticOilCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceSyntheticOilCurrent;
            syntheticOilMaxTextBox.Tag = ScenarioEditorItemId.ProvinceSyntheticOilMax;
            syntheticOilRelativeTextBox.Tag = ScenarioEditorItemId.ProvinceSyntheticOilRelative;
            syntheticRaresCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceSyntheticRaresCurrent;
            syntheticRaresMaxTextBox.Tag = ScenarioEditorItemId.ProvinceSyntheticRaresMax;
            syntheticRaresRelativeTextBox.Tag = ScenarioEditorItemId.ProvinceSyntheticRaresRelative;
            nuclearPowerCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceNuclearPowerCurrent;
            nuclearPowerMaxTextBox.Tag = ScenarioEditorItemId.ProvinceNuclearPowerMax;
            nuclearPowerRelativeTextBox.Tag = ScenarioEditorItemId.ProvinceNuclearPowerRelative;
        }

        /// <summary>
        ///     プロヴィンス建物情報の編集項目を有効化する
        /// </summary>
        private void EnableProvinceBuildingItems()
        {
            provinceBuildingGroupBox.Enabled = true;
        }

        /// <summary>
        ///     プロヴィンス建物情報の編集項目を無効化する
        /// </summary>
        private void DisableProvinceBuildingItems()
        {
            provinceBuildingGroupBox.Enabled = false;
        }

        /// <summary>
        ///     プロヴィンス建物情報の編集項目の表示を更新する
        /// </summary>
        /// <param name="settings">プロヴィンス設定</param>
        private void UpdateProvinceBuildingItems(ProvinceSettings settings)
        {
            _controller.UpdateItemValue(icCurrentTextBox, settings);
            _controller.UpdateItemValue(icMaxTextBox, settings);
            _controller.UpdateItemValue(icRelativeTextBox, settings);
            _controller.UpdateItemValue(infrastructureCurrentTextBox, settings);
            _controller.UpdateItemValue(infrastructureMaxTextBox, settings);
            _controller.UpdateItemValue(infrastructureRelativeTextBox, settings);
            _controller.UpdateItemValue(landFortCurrentTextBox, settings);
            _controller.UpdateItemValue(landFortMaxTextBox, settings);
            _controller.UpdateItemValue(landFortRelativeTextBox, settings);
            _controller.UpdateItemValue(coastalFortCurrentTextBox, settings);
            _controller.UpdateItemValue(coastalFortMaxTextBox, settings);
            _controller.UpdateItemValue(coastalFortRelativeTextBox, settings);
            _controller.UpdateItemValue(antiAirCurrentTextBox, settings);
            _controller.UpdateItemValue(antiAirMaxTextBox, settings);
            _controller.UpdateItemValue(antiAirRelativeTextBox, settings);
            _controller.UpdateItemValue(airBaseCurrentTextBox, settings);
            _controller.UpdateItemValue(airBaseMaxTextBox, settings);
            _controller.UpdateItemValue(airBaseRelativeTextBox, settings);
            _controller.UpdateItemValue(navalBaseCurrentTextBox, settings);
            _controller.UpdateItemValue(navalBaseMaxTextBox, settings);
            _controller.UpdateItemValue(navalBaseRelativeTextBox, settings);
            _controller.UpdateItemValue(radarStationCurrentTextBox, settings);
            _controller.UpdateItemValue(radarStationMaxTextBox, settings);
            _controller.UpdateItemValue(radarStationRelativeTextBox, settings);
            _controller.UpdateItemValue(nuclearReactorCurrentTextBox, settings);
            _controller.UpdateItemValue(nuclearReactorMaxTextBox, settings);
            _controller.UpdateItemValue(nuclearReactorRelativeTextBox, settings);
            _controller.UpdateItemValue(rocketTestCurrentTextBox, settings);
            _controller.UpdateItemValue(rocketTestMaxTextBox, settings);
            _controller.UpdateItemValue(rocketTestRelativeTextBox, settings);
            _controller.UpdateItemValue(syntheticOilCurrentTextBox, settings);
            _controller.UpdateItemValue(syntheticOilMaxTextBox, settings);
            _controller.UpdateItemValue(syntheticOilRelativeTextBox, settings);
            _controller.UpdateItemValue(syntheticRaresCurrentTextBox, settings);
            _controller.UpdateItemValue(syntheticRaresMaxTextBox, settings);
            _controller.UpdateItemValue(syntheticRaresRelativeTextBox, settings);
            _controller.UpdateItemValue(nuclearPowerCurrentTextBox, settings);
            _controller.UpdateItemValue(nuclearPowerMaxTextBox, settings);
            _controller.UpdateItemValue(nuclearPowerRelativeTextBox, settings);

            _controller.UpdateItemColor(icCurrentTextBox, settings);
            _controller.UpdateItemColor(icMaxTextBox, settings);
            _controller.UpdateItemColor(icRelativeTextBox, settings);
            _controller.UpdateItemColor(infrastructureCurrentTextBox, settings);
            _controller.UpdateItemColor(infrastructureMaxTextBox, settings);
            _controller.UpdateItemColor(infrastructureRelativeTextBox, settings);
            _controller.UpdateItemColor(landFortCurrentTextBox, settings);
            _controller.UpdateItemColor(landFortMaxTextBox, settings);
            _controller.UpdateItemColor(landFortRelativeTextBox, settings);
            _controller.UpdateItemColor(coastalFortCurrentTextBox, settings);
            _controller.UpdateItemColor(coastalFortMaxTextBox, settings);
            _controller.UpdateItemColor(coastalFortRelativeTextBox, settings);
            _controller.UpdateItemColor(antiAirCurrentTextBox, settings);
            _controller.UpdateItemColor(antiAirMaxTextBox, settings);
            _controller.UpdateItemColor(antiAirRelativeTextBox, settings);
            _controller.UpdateItemColor(airBaseCurrentTextBox, settings);
            _controller.UpdateItemColor(airBaseMaxTextBox, settings);
            _controller.UpdateItemColor(airBaseRelativeTextBox, settings);
            _controller.UpdateItemColor(navalBaseCurrentTextBox, settings);
            _controller.UpdateItemColor(navalBaseMaxTextBox, settings);
            _controller.UpdateItemColor(navalBaseRelativeTextBox, settings);
            _controller.UpdateItemColor(radarStationCurrentTextBox, settings);
            _controller.UpdateItemColor(radarStationMaxTextBox, settings);
            _controller.UpdateItemColor(radarStationRelativeTextBox, settings);
            _controller.UpdateItemColor(nuclearReactorCurrentTextBox, settings);
            _controller.UpdateItemColor(nuclearReactorMaxTextBox, settings);
            _controller.UpdateItemColor(nuclearReactorRelativeTextBox, settings);
            _controller.UpdateItemColor(rocketTestCurrentTextBox, settings);
            _controller.UpdateItemColor(rocketTestMaxTextBox, settings);
            _controller.UpdateItemColor(rocketTestRelativeTextBox, settings);
            _controller.UpdateItemColor(syntheticOilCurrentTextBox, settings);
            _controller.UpdateItemColor(syntheticOilMaxTextBox, settings);
            _controller.UpdateItemColor(syntheticOilRelativeTextBox, settings);
            _controller.UpdateItemColor(syntheticRaresCurrentTextBox, settings);
            _controller.UpdateItemColor(syntheticRaresMaxTextBox, settings);
            _controller.UpdateItemColor(syntheticRaresRelativeTextBox, settings);
            _controller.UpdateItemColor(nuclearPowerCurrentTextBox, settings);
            _controller.UpdateItemColor(nuclearPowerMaxTextBox, settings);
            _controller.UpdateItemColor(nuclearPowerRelativeTextBox, settings);
        }

        /// <summary>
        ///     プロヴィンス建物情報の編集項目の表示をクリアする
        /// </summary>
        private void ClearProvinceBuildingItems()
        {
            icCurrentTextBox.Text = "";
            icMaxTextBox.Text = "";
            icRelativeTextBox.Text = "";
            infrastructureCurrentTextBox.Text = "";
            infrastructureMaxTextBox.Text = "";
            infrastructureRelativeTextBox.Text = "";
            landFortCurrentTextBox.Text = "";
            landFortMaxTextBox.Text = "";
            landFortRelativeTextBox.Text = "";
            coastalFortCurrentTextBox.Text = "";
            coastalFortMaxTextBox.Text = "";
            coastalFortRelativeTextBox.Text = "";
            antiAirCurrentTextBox.Text = "";
            antiAirMaxTextBox.Text = "";
            antiAirRelativeTextBox.Text = "";
            airBaseCurrentTextBox.Text = "";
            airBaseMaxTextBox.Text = "";
            airBaseRelativeTextBox.Text = "";
            navalBaseCurrentTextBox.Text = "";
            navalBaseMaxTextBox.Text = "";
            navalBaseRelativeTextBox.Text = "";
            radarStationCurrentTextBox.Text = "";
            radarStationMaxTextBox.Text = "";
            radarStationRelativeTextBox.Text = "";
            nuclearReactorCurrentTextBox.Text = "";
            nuclearReactorMaxTextBox.Text = "";
            nuclearReactorRelativeTextBox.Text = "";
            rocketTestCurrentTextBox.Text = "";
            rocketTestMaxTextBox.Text = "";
            rocketTestRelativeTextBox.Text = "";
            syntheticOilCurrentTextBox.Text = "";
            syntheticOilMaxTextBox.Text = "";
            syntheticOilRelativeTextBox.Text = "";
            syntheticRaresCurrentTextBox.Text = "";
            syntheticRaresMaxTextBox.Text = "";
            syntheticRaresRelativeTextBox.Text = "";
            nuclearPowerCurrentTextBox.Text = "";
            nuclearPowerMaxTextBox.Text = "";
            nuclearPowerRelativeTextBox.Text = "";
        }

        #endregion

        #region プロヴィンスタブ - 編集項目

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProvinceIntItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            ProvinceSettings settings = Scenarios.GetProvinceSettings(province.Id);

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, settings);
                return;
            }

            // 初期値から変更されていなければ何もしない
            if ((settings == null) && (val == 0))
            {
                return;
            }

            // 値に変化がなければ何もしない
            object prev = _controller.GetItemValue(itemId, settings);
            if ((prev != null) && (val == (int) prev))
            {
                return;
            }

            if (settings == null)
            {
                settings = new ProvinceSettings { Id = province.Id };
                Scenarios.AddProvinceSettings(settings);
            }

            _controller.OutputItemValueChangedLog(itemId, val, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, settings);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, settings);
        }

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProvinceDoubleItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            ProvinceSettings settings = Scenarios.GetProvinceSettings(province.Id);

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, settings);
                return;
            }

            // 初期値から変更されていなければ何もしない
            if ((settings == null) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            object prev = _controller.GetItemValue(itemId, settings);
            if ((prev != null) && DoubleHelper.IsEqual(val, (double) prev))
            {
                return;
            }

            if (settings == null)
            {
                settings = new ProvinceSettings { Id = province.Id };
                Scenarios.AddProvinceSettings(settings);
            }

            _controller.OutputItemValueChangedLog(itemId, val, settings);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, settings);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, settings);
        }

        /// <summary>
        ///     テキストボックスの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProvinceStringItemTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            ProvinceSettings settings = Scenarios.GetProvinceSettings(province.Id);

            // 初期値から変更されていなければ何もしない
            string val = control.Text;
            if ((settings == null) && string.IsNullOrEmpty(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (val.Equals(_controller.GetItemValue(itemId, province, settings)))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, province, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, province, settings);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, province, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;
        }

        /// <summary>
        ///     チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProvinceCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }
            Country country = GetSelectedProvinceCountry();
            if (country == Country.None)
            {
                return;
            }

            CheckBox control = sender as CheckBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 初期値から変更されていなければ何もしない
            bool val = control.Checked;
            if ((settings == null) && !val)
            {
                return;
            }

            // 値に変化がなければ何もしない
            object prev = _controller.GetItemValue(itemId, province, settings);
            if ((prev != null) && (val == (bool) prev))
            {
                return;
            }

            if (settings == null)
            {
                settings = new CountrySettings { Country = country };
                Scenarios.SetCountrySettings(settings);
            }

            _controller.OutputItemValueChangedLog(itemId, val, province, settings);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, province, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, province, settings);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, province, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, province);
        }

        #endregion

        #endregion

        #region 共通

        #region 共通 - 国家

        /// <summary>
        ///     国タグと国名の文字列を取得する
        /// </summary>
        /// <param name="country">国家</param>
        /// <returns>国タグと国名の文字列</returns>
        private static string GetCountryTagName(Country country)
        {
            CountrySettings settings = Scenarios.GetCountrySettings(country);
            return string.Format("{0} {1}", Countries.Strings[(int) country],
                ((settings != null) && !string.IsNullOrEmpty(settings.Name))
                    ? Config.GetText(settings.Name)
                    : Countries.GetName(country));
        }

        /// <summary>
        ///     国家リストボックスを更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        private static void UpdateCountryListBox(ListBox control)
        {
            control.BeginUpdate();
            control.Items.Clear();
            foreach (Country country in Countries.Tags)
            {
                control.Items.Add(Countries.GetTagName(country));
            }
            control.EndUpdate();
        }

        /// <summary>
        ///     国家コンボボックスを更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="allowEmpty">空項目を許可するかどうか</param>
        private void UpdateCountryComboBox(ComboBox control, bool allowEmpty)
        {
            Graphics g = Graphics.FromHwnd(Handle);
            int margin = DeviceCaps.GetScaledWidth(2) + 1;

            int width = control.Width;
            control.BeginUpdate();
            control.Items.Clear();
            if (allowEmpty)
            {
                control.Items.Add("");
            }
            foreach (Country country in Countries.Tags)
            {
                string s = Countries.GetTagName(country);
                control.Items.Add(s);
                width = Math.Max(width,
                    (int) g.MeasureString(s, control.Font).Width + SystemInformation.VerticalScrollBarWidth + margin);
            }
            control.DropDownWidth = width;
            control.EndUpdate();
        }

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

            ListBox control = sender as ListBox;
            if (control == null)
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
                    ? (settings.IsDirty() ? Color.Red : control.ForeColor)
                    : Color.LightGray);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            string s = control.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        #endregion

        #region 共通 - 編集項目

        /// <summary>
        ///     編集項目IDに関連付けられたコントロールを取得する
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public Control GetItemControl(ScenarioEditorItemId itemId)
        {
            return _itemControls.ContainsKey(itemId) ? _itemControls[itemId] : null;
        }

        #endregion

        #endregion
    }
}