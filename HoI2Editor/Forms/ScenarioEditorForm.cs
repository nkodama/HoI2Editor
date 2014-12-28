using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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

        /// <summary>
        ///     同盟国以外の国家リスト
        /// </summary>
        private List<Country> _allianceFreeCountries;

        /// <summary>
        ///     主要国以外の選択可能国リスト
        /// </summary>
        private List<Country> _majorFreeCountries;

        private ushort _prevId;

        /// <summary>
        ///     選択可能国以外の国家リスト
        /// </summary>
        private List<Country> _selectableFreeCountries;

        /// <summary>
        ///     戦争国以外の国家リスト
        /// </summary>
        private List<Country> _warFreeCountries;

        #endregion

        #region 内部定数

        /// <summary>
        ///     AIの攻撃性の文字列
        /// </summary>
        private readonly string[] _aiAggressiveStrings =
        {
            "FEOPT_AI_LEVEL1", // 臆病
            "FEOPT_AI_LEVEL2", // 弱気
            "FEOPT_AI_LEVEL3", // 標準
            "FEOPT_AI_LEVEL4", // 攻撃的
            "FEOPT_AI_LEVEL5" // 過激
        };

        /// <summary>
        ///     難易度の文字列
        /// </summary>
        private readonly string[] _difficultyStrings =
        {
            "FE_DIFFI1", // 非常に難しい
            "FE_DIFFI2", // 難しい
            "FE_DIFFI3", // 標準
            "FE_DIFFI4", // 簡単
            "FE_DIFFI5" // 非常に簡単
        };

        /// <summary>
        ///     ゲームスピードの文字列
        /// </summary>
        private readonly string[] _gameSpeedStrings =
        {
            "FEOPT_GAMESPEED0", // 非常に遅い
            "FEOPT_GAMESPEED1", // 遅い
            "FEOPT_GAMESPEED2", // やや遅い
            "FEOPT_GAMESPEED3", // 標準
            "FEOPT_GAMESPEED4", // やや速い
            "FEOPT_GAMESPEED5", // 速い
            "FEOPT_GAMESPEED6", // 非常に速い
            "FEOPT_GAMESPEED7" // きわめて速い
        };

        /// <summary>
        ///     エネルギーの文字列名
        /// </summary>
        private const string EnergyName = "RESOURCE_ENERGY";

        /// <summary>
        ///     金属の文字列名
        /// </summary>
        private const string MetalName = "RESOURCE_METAL";

        /// <summary>
        ///     希少資源の文字列名
        /// </summary>
        private const string RareMaterialsName = "RESOURCE_RARE_MATERIALS";

        /// <summary>
        ///     石油の文字列名
        /// </summary>
        private const string OilName = "RESOURCE_OIL";

        /// <summary>
        ///     物資の文字列名
        /// </summary>
        private const string SuppliesName = "RESOURCE_SUPPLY";

        /// <summary>
        ///     資金の文字列名
        /// </summary>
        private const string MoneyName = "RESOURCE_MONEY";

        /// <summary>
        ///     輸送船団の文字列名
        /// </summary>
        private const string TransportsName = "CIW_TRANSPORTS";

        /// <summary>
        ///     護衛艦の文字列名
        /// </summary>
        private const string EscortsName = "CIW_ESCORTS";

        /// <summary>
        ///     ICの文字列名
        /// </summary>
        private const string IcName = "RESOURCE_IC";

        /// <summary>
        ///     人的資源の文字列名
        /// </summary>
        private const string ManpowerName = "RESOURCE_MANPOWER";

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
        ///     マップを読み込む
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMapWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            Maps.Load(MapLevel.Level2);
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

            // ユニットデータを初期化する
            Units.Init();

            // ゲーム設定ファイルを読み込む
            Misc.Load();

            // 文字列定義ファイルを読み込む
            Config.Load();

            // 表示項目を初期化する
            InitEditableItems();
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

        #endregion

        #region メインタブ

        #region メインタブ - 共通

        /// <summary>
        ///     メインタブの項目を初期化する
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
        ///     メインタブの項目を更新する
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
            Image old = panelPictureBox.Image;
            panelPictureBox.Image = GetPanelImage(scenario.PanelName);
            if (old != null)
            {
                old.Dispose();
            }

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
            string name = Config.ExistsKey(scenario.Name) ? Config.GetText(scenario.Name) : "";
            if (scenarioNameTextBox.Text.Equals(name))
            {
                return;
            }

            Log.Info("[Scenario] scenario name: {0} -> {1}", name, scenarioNameTextBox.Text);

            // 値を更新する
            Config.SetText(scenario.Name, scenarioNameTextBox.Text, Game.ScenarioTextFileName);

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
            if (panelImageTextBox.Text.Equals(scenario.PanelName))
            {
                return;
            }

            Log.Info("[Scenario] panel image: {0} -> {1}", scenario.PanelName, panelImageTextBox.Text);

            // 値を更新する
            scenario.PanelName = panelImageTextBox.Text;

            // 編集済みフラグを設定する
            scenario.SetDirty(ScenarioItemId.PanelName);
            Scenarios.SetDirty();

            // 文字色を変更する
            panelImageTextBox.ForeColor = Color.Red;

            // パネル画像を更新する
            Image old = panelPictureBox.Image;
            panelPictureBox.Image = GetPanelImage(scenario.PanelName);
            if (old != null)
            {
                old.Dispose();
            }
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
                string fileName;
                if (Game.IsExportFolderActive)
                {
                    fileName = PathHelper.GetRelativePathName(dialog.FileName, Game.ExportFolderName);
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        panelImageTextBox.Text = fileName;
                        return;
                    }
                }
                if (Game.IsModActive)
                {
                    fileName = PathHelper.GetRelativePathName(dialog.FileName, Game.ModFolderName);
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        panelImageTextBox.Text = fileName;
                        return;
                    }
                }
                fileName = PathHelper.GetRelativePathName(dialog.FileName, Game.FolderName);
                if (!string.IsNullOrEmpty(fileName))
                {
                    panelImageTextBox.Text = fileName;
                    return;
                }
                panelImageTextBox.Text = dialog.FileName;
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
            if (includeFolderTextBox.Text.Equals(scenario.IncludeFolder))
            {
                return;
            }

            Log.Info("[Scenario] include folder: {0} -> {1}", scenario.IncludeFolder, includeFolderTextBox.Text);

            // 値を更新する
            scenario.IncludeFolder = includeFolderTextBox.Text;

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
                string folderName;
                if (Game.IsExportFolderActive)
                {
                    folderName = PathHelper.GetRelativePathName(dialog.SelectedPath,
                        Path.Combine(Game.ExportFolderName, Game.ScenarioPathName));
                    if (!string.IsNullOrEmpty(folderName))
                    {
                        includeFolderTextBox.Text = folderName;
                        return;
                    }
                }
                if (Game.IsModActive)
                {
                    folderName = PathHelper.GetRelativePathName(dialog.SelectedPath,
                        Path.Combine(Game.ModFolderName, Game.ScenarioPathName));
                    if (!string.IsNullOrEmpty(folderName))
                    {
                        includeFolderTextBox.Text = folderName;
                        return;
                    }
                }
                folderName = PathHelper.GetRelativePathName(dialog.SelectedPath,
                    Path.Combine(Game.FolderName, Game.ScenarioPathName));
                if (!string.IsNullOrEmpty(folderName))
                {
                    includeFolderTextBox.Text = folderName;
                    return;
                }
                includeFolderTextBox.Text = dialog.SelectedPath;
            }
        }

        /// <summary>
        ///     パネル画像を取得する
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <returns>パネル画像</returns>
        private static Bitmap GetPanelImage(string fileName)
        {
            string pathName = Game.GetReadFileName(fileName);
            if (!File.Exists(pathName))
            {
                return null;
            }

            Bitmap bitmap = new Bitmap(pathName);
            bitmap.MakeTransparent(Color.Lime);
            return bitmap;
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

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            ScenarioHeader header = Scenarios.Data.Header;
            Brush brush = ((e.Index == header.AiAggressive) && Scenarios.Data.IsDirty(ScenarioItemId.AiAggressive))
                ? new SolidBrush(Color.Red)
                : new SolidBrush(SystemColors.WindowText);
            string s = aiAggressiveComboBox.Items[e.Index].ToString();
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

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            ScenarioHeader header = Scenarios.Data.Header;
            Brush brush = ((e.Index == header.Difficulty) && Scenarios.Data.IsDirty(ScenarioItemId.Difficulty))
                ? new SolidBrush(Color.Red)
                : new SolidBrush(SystemColors.WindowText);
            string s = difficultyComboBox.Items[e.Index].ToString();
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

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            ScenarioHeader header = Scenarios.Data.Header;
            Brush brush = ((e.Index == header.GameSpeed) && Scenarios.Data.IsDirty(ScenarioItemId.GameSpeed))
                ? new SolidBrush(Color.Red)
                : new SolidBrush(SystemColors.WindowText);
            string s = gameSpeedComboBox.Items[e.Index].ToString();
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
            if (freeCountryCheckBox.Checked == header.IsFreeSelection)
            {
                return;
            }

            Log.Info("[Scenario] free country selection: {0} -> {1}", BoolHelper.ToYesNo(header.IsFreeSelection),
                BoolHelper.ToYesNo(freeCountryCheckBox.Checked));

            // 値を更新する
            header.IsFreeSelection = freeCountryCheckBox.Checked;

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
            if ((data.Rules != null) && (allowDiplomacyCheckBox.Checked == data.Rules.AllowDiplomacy))
            {
                return;
            }

            Log.Info("[Scenario] allow diplomacy: {0} -> {1}",
                (data.Rules != null) ? BoolHelper.ToYesNo(data.Rules.AllowDiplomacy) : "",
                BoolHelper.ToYesNo(allowDiplomacyCheckBox.Checked));

            if (data.Rules == null)
            {
                data.Rules = new ScenarioRules();
            }

            // 値を更新する
            data.Rules.AllowDiplomacy = allowDiplomacyCheckBox.Checked;

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
            if ((data.Rules != null) && (allowProductionCheckBox.Checked == data.Rules.AllowProduction))
            {
                return;
            }

            Log.Info("[Scenario] allow production: {0} -> {1}",
                (data.Rules != null) ? BoolHelper.ToYesNo(data.Rules.AllowProduction) : "",
                BoolHelper.ToYesNo(allowProductionCheckBox.Checked));

            if (data.Rules == null)
            {
                data.Rules = new ScenarioRules();
            }

            // 値を更新する
            data.Rules.AllowProduction = allowProductionCheckBox.Checked;

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
            if ((data.Rules != null) && (allowTechnologyCheckBox.Checked == data.Rules.AllowTechnology))
            {
                return;
            }

            Log.Info("[Scenario] allow technology: {0} -> {1}",
                (data.Rules != null) ? BoolHelper.ToYesNo(data.Rules.AllowTechnology) : "",
                BoolHelper.ToYesNo(allowTechnologyCheckBox.Checked));

            if (data.Rules == null)
            {
                data.Rules = new ScenarioRules();
            }

            // 値を更新する
            data.Rules.AllowTechnology = allowTechnologyCheckBox.Checked;

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

            // AIの攻撃性コンボボックスの項目色を変更するために描画更新する
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

            // 難易度コンボボックスの項目色を変更するために描画更新する
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

            // ゲームスピードコンボボックスの項目色を変更するために描画更新する
            gameSpeedComboBox.Refresh();
        }

        #endregion

        #region メインタブ - 国家選択

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

            Scenario scenario = Scenarios.Data;
            List<MajorCountrySettings> majors = scenario.Header.MajorCountries;

            // 背景を描画する
            e.DrawBackground();

            // 項目を描画する
            Brush brush;
            if ((e.State & DrawItemState.Selected) == 0)
            {
                // 変更ありの項目は文字色を変更する
                brush = scenario.IsDirtySelectableCountry(majors[e.Index].Country)
                    ? new SolidBrush(Color.Red)
                    : new SolidBrush(majorListBox.ForeColor);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            string s = majorListBox.Items[e.Index].ToString();
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

            Scenario scenario = Scenarios.Data;

            // 背景を描画する
            e.DrawBackground();

            // 項目を描画する
            Brush brush;
            if ((e.State & DrawItemState.Selected) == 0)
            {
                // 変更ありの項目は文字色を変更する
                brush = scenario.IsDirtySelectableCountry(_majorFreeCountries[e.Index])
                    ? new SolidBrush(Color.Red)
                    : new SolidBrush(selectableListBox.ForeColor);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            string s = selectableListBox.Items[e.Index].ToString();
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

            Scenario scenario = Scenarios.Data;

            // 背景を描画する
            e.DrawBackground();

            // 項目を描画する
            Brush brush;
            if ((e.State & DrawItemState.Selected) == 0)
            {
                // 変更ありの項目は文字色を変更する
                brush = scenario.IsDirtySelectableCountry(_selectableFreeCountries[e.Index])
                    ? new SolidBrush(Color.Red)
                    : new SolidBrush(unselectableListBox.ForeColor);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            string s = unselectableListBox.Items[e.Index].ToString();
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
            Image image;
            int index = majorListBox.SelectedIndex;
            if (index < 0)
            {
                // 選択項目がなければ表示をクリアする
                countryDescTextBox.Text = "";
                propagandaTextBox.Text = "";
                image = null;

                // 設定項目を無効化する
                countryDescLabel.Enabled = false;
                countryDescTextBox.Enabled = false;
                propagandaLabel.Enabled = false;
                propagandaTextBox.Enabled = false;
                propagandaBrowseButton.Enabled = false;

                majorRemoveButton.Enabled = false;
                majorUpButton.Enabled = false;
                majorDownButton.Enabled = false;
            }
            else
            {
                ScenarioHeader header = Scenarios.Data.Header;
                MajorCountrySettings major = header.MajorCountries[index];
                int year = (header.StartDate != null) ? header.StartDate.Year : header.StartYear;
                countryDescTextBox.Text = GetCountryDesc(major.Country, year, major.Desc);
                countryDescTextBox.ForeColor = major.IsDirty(MajorCountrySettingsItemId.Desc)
                    ? Color.Red
                    : SystemColors.WindowText;

                propagandaTextBox.Text = major.PictureName;
                propagandaTextBox.ForeColor = major.IsDirty(MajorCountrySettingsItemId.PictureName)
                    ? Color.Red
                    : SystemColors.WindowText;
                image = GetCountryPropagandaImage(major.Country, major.PictureName);

                // 設定項目を有効化する
                countryDescLabel.Enabled = true;
                countryDescTextBox.Enabled = true;
                propagandaLabel.Enabled = true;
                propagandaTextBox.Enabled = true;
                propagandaBrowseButton.Enabled = true;

                majorRemoveButton.Enabled = true;
                majorUpButton.Enabled = (index > 0);
                majorDownButton.Enabled = (index < header.MajorCountries.Count - 1);
            }

            Image old = propagandaPictureBox.Image;
            propagandaPictureBox.Image = image;
            if (old != null)
            {
                old.Dispose();
            }
        }

        /// <summary>
        ///     選択可能国リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectableListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectableListBox.SelectedItems.Count > 0)
            {
                majorAddButton.Enabled = true;
                selectableRemoveButton.Enabled = true;
            }
            else
            {
                majorAddButton.Enabled = false;
                selectableRemoveButton.Enabled = false;
            }
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
            majorListBox.SelectedIndex = -1;
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
            majorListBox.SelectedIndex = -1;
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

            // 値に変化がなければ何もしない
            if (propagandaTextBox.Text.Equals(major.PictureName))
            {
                return;
            }

            Log.Info("[Scenario] propaganda image: {0} -> {1} ({2})", major.PictureName, propagandaTextBox.Text,
                major.Country);

            // 値を更新する
            major.PictureName = propagandaTextBox.Text;

            // 編集済みフラグを設定する
            major.SetDirty(MajorCountrySettingsItemId.PictureName);
            Scenarios.SetDirty();

            // 文字色を変更する
            propagandaTextBox.ForeColor = Color.Red;

            // プロパガンダ画像を更新する
            Image old = propagandaPictureBox.Image;
            propagandaPictureBox.Image = GetCountryPropagandaImage(major.Country, major.PictureName);
            if (old != null)
            {
                old.Dispose();
            }
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
                string fileName;
                if (Game.IsExportFolderActive)
                {
                    fileName = PathHelper.GetRelativePathName(dialog.FileName, Game.ExportFolderName);
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        propagandaTextBox.Text = fileName;
                        return;
                    }
                }
                if (Game.IsModActive)
                {
                    fileName = PathHelper.GetRelativePathName(dialog.FileName, Game.ModFolderName);
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        propagandaTextBox.Text = fileName;
                        return;
                    }
                }
                fileName = PathHelper.GetRelativePathName(dialog.FileName, Game.FolderName);
                if (!string.IsNullOrEmpty(fileName))
                {
                    propagandaTextBox.Text = fileName;
                    return;
                }
                propagandaTextBox.Text = dialog.FileName;
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
            int year = (header.StartDate != null) ? header.StartDate.Year : header.StartYear;
            string name = GetCountryDesc(major.Country, year, major.Desc);
            if (countryDescTextBox.Text.Equals(name))
            {
                return;
            }

            Log.Info("[Scenario] country desc: {0} -> {1} ({2})", name, countryDescTextBox.Text, major.Country);

            // 値を更新する
            Config.SetText(major.Desc, countryDescTextBox.Text, Game.ScenarioTextFileName);

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

            if (tag == Country.None)
            {
                return "";
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
        ///     国家のプロパガンダ画像を取得する
        /// </summary>
        /// <param name="country">国タグ</param>
        /// <param name="fileName">プロパガンダ画像名</param>
        /// <returns>プロパガンダ画像</returns>
        private static Image GetCountryPropagandaImage(Country country, string fileName)
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

            if (country == Country.None)
            {
                return null;
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
        ///     同盟タブの項目を初期化する
        /// </summary>
        private void InitAllianceTab()
        {
            // 何もしない
        }

        /// <summary>
        ///     同盟タブの項目を更新する
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
            ListViewItem item = new ListViewItem();
            if (data.Axis != null)
            {
                item.Text = GetAllianceName(data.Axis);
                item.Tag = data.Axis;
                item.SubItems.Add(Countries.GetNameList(data.Axis.Participant));
            }
            else
            {
                item.Text = Config.GetText("EYR_AXIS");
            }
            allianceListView.Items.Add(item);

            // 連合国
            item = new ListViewItem();
            if (data.Allies != null)
            {
                item.Text = GetAllianceName(data.Allies);
                item.Tag = data.Allies;
                item.SubItems.Add(Countries.GetNameList(data.Allies.Participant));
            }
            else
            {
                item.Text = Config.GetText("EYR_ALLIES");
            }
            allianceListView.Items.Add(item);

            // 共産国
            item = new ListViewItem();
            if (data.Comintern != null)
            {
                item.Text = GetAllianceName(data.Comintern);
                item.Tag = data.Comintern;
                item.SubItems.Add(Countries.GetNameList(data.Comintern.Participant));
            }
            else
            {
                item.Text = Config.GetText("EYR_COM");
            }
            allianceListView.Items.Add(item);

            // その他の同盟
            foreach (Alliance alliance in data.Alliances)
            {
                string name = GetAllianceName(alliance);
                item = new ListViewItem
                {
                    Text = !string.IsNullOrEmpty(name) ? name : Resources.Alliance,
                    Tag = alliance
                };
                item.SubItems.Add(Countries.GetNameList(alliance.Participant));
                allianceListView.Items.Add(item);
            }

            allianceListView.EndUpdate();
        }

        /// <summary>
        ///     同盟情報の編集項目を無効化する
        /// </summary>
        private void DisableAllianceItems()
        {
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

            allianceNameTextBox.Text = "";
            allianceTypeTextBox.Text = "";
            allianceIdTextBox.Text = "";

            allianceParticipantListBox.Items.Clear();
            allianceCountryListBox.Items.Clear();
        }

        /// <summary>
        ///     同盟情報の編集項目を更新する
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

            IEnumerable<Country> countries = Countries.Tags;

            // 参加国リストボックス
            allianceParticipantListBox.BeginUpdate();
            allianceParticipantListBox.Items.Clear();
            foreach (Country country in alliance.Participant)
            {
                allianceParticipantListBox.Items.Add(Countries.GetTagName(country));
            }
            countries = countries.Where(country => !alliance.Participant.Contains(country));
            allianceParticipantListBox.EndUpdate();

            // 国家リストボックス
            _allianceFreeCountries = countries.ToList();
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
            if (allianceNameTextBox.Text.Equals(name))
            {
                return;
            }

            Log.Info("[Scenario] alliance name: {0} -> {1}", name, allianceNameTextBox.Text);

            // 値を更新する
            string s = allianceNameTextBox.Text;
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

            string name = GetAllianceName(alliance);
            if (string.IsNullOrEmpty(name))
            {
                name = IntHelper.ToString(allianceListView.SelectedIndices[0]);
            }
            Log.Info("[Scenario] alliance type: {0} -> {1} ({2})",
                (alliance.Id != null) ? IntHelper.ToString(alliance.Id.Type) : "", val, name);

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
            if (Scenarios.ExistsTypeId((alliance.Id != null) ? alliance.Id.Type : Scenarios.DefaultWarType, val))
            {
                allianceIdTextBox.Text = (alliance.Id != null) ? IntHelper.ToString(alliance.Id.Id) : "";
            }

            string name = GetAllianceName(alliance);
            if (string.IsNullOrEmpty(name))
            {
                name = IntHelper.ToString(allianceListView.SelectedIndices[0]);
            }
            Log.Info("[Scenario] alliance id: {0} -> {1} ({2})",
                (alliance.Id != null) ? IntHelper.ToString(alliance.Id.Id) : "", val, name);

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
                brush = alliance.IsDirtyCountry(alliance.Participant[e.Index])
                    ? new SolidBrush(Color.Red)
                    : new SolidBrush(allianceParticipantListBox.ForeColor);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            string s = allianceParticipantListBox.Items[e.Index].ToString();
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
                brush = alliance.IsDirtyCountry(_allianceFreeCountries[e.Index])
                    ? new SolidBrush(Color.Red)
                    : new SolidBrush(allianceCountryListBox.ForeColor);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            string s = allianceCountryListBox.Items[e.Index].ToString();
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

                string name = GetAllianceName(alliance);
                if (string.IsNullOrEmpty(name))
                {
                    name = IntHelper.ToString(allianceListView.SelectedIndices[0]);
                }
                Log.Info("[Scenario] alliance participant: +{0} ({1})", Countries.Strings[(int) country], name);
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

                string name = GetAllianceName(alliance);
                if (string.IsNullOrEmpty(name))
                {
                    name = IntHelper.ToString(allianceListView.SelectedIndices[0]);
                }
                Log.Info("[Scenario] alliance participant: -{0} ({1})", Countries.Strings[(int) country], name);
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

            string name = GetAllianceName(alliance);
            if (string.IsNullOrEmpty(name))
            {
                name = IntHelper.ToString(allianceListView.SelectedIndices[0]);
            }
            Log.Info("[Scenario] alliance leader: {0} ({1})", Countries.Strings[(int) country], name);
        }

        /// <summary>
        ///     選択中の同盟情報を取得する
        /// </summary>
        /// <returns>選択中の同盟情報</returns>
        private Alliance GetSelectedAlliance()
        {
            if (allianceListView.SelectedItems.Count == 0)
            {
                return null;
            }

            return allianceListView.SelectedItems[0].Tag as Alliance;
        }

        /// <summary>
        ///     同盟名を取得する
        /// </summary>
        /// <param name="alliance">同盟</param>
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
                return Config.GetText("EYR_AXIS");
            }
            if (alliance == data.Allies)
            {
                return Config.GetText("EYR_ALLIES");
            }
            if (alliance == data.Comintern)
            {
                return Config.GetText("EYR_COM");
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
        ///     戦争情報の編集項目を有効化する
        /// </summary>
        private void EnableWarItems()
        {
            int count = warListView.Items.Count;
            int index = warListView.SelectedIndices[0];
            warUpButton.Enabled = (index > 0);
            warDownButton.Enabled = (index < count - 1);
            warRemoveButton.Enabled = true;

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
        ///     戦争情報の編集項目を無効化する
        /// </summary>
        private void DisableWarItems()
        {
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
        }

        /// <summary>
        ///     戦争情報の編集項目を更新する
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
        ///     戦争情報の編集項目をクリアする
        /// </summary>
        private void ClearWarItems()
        {
            warStartYearTextBox.Text = "";
            warStartMonthTextBox.Text = "";
            warStartDayTextBox.Text = "";
            warEndYearTextBox.Text = "";
            warEndMonthTextBox.Text = "";
            warEndDayTextBox.Text = "";
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
        ///     戦争リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がない場合
            if (warListView.SelectedItems.Count == 0)
            {
                // 編集項目を無効化する
                DisableWarItems();

                // 編集項目をクリアする
                ClearWarItems();

                return;
            }

            // 編集項目を更新する
            UpdateWarItems();

            // 編集項目を有効化する
            EnableWarItems();

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
                brush = war.IsDirtyCountry(war.Attackers.Participant[e.Index])
                    ? new SolidBrush(Color.Red)
                    : new SolidBrush(warAttackerListBox.ForeColor);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            string s = warAttackerListBox.Items[e.Index].ToString();
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
                brush = war.IsDirtyCountry(war.Defenders.Participant[e.Index])
                    ? new SolidBrush(Color.Red)
                    : new SolidBrush(warDefenderListBox.ForeColor);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            string s = warDefenderListBox.Items[e.Index].ToString();
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
                brush = war.IsDirtyCountry(_warFreeCountries[e.Index])
                    ? new SolidBrush(Color.Red)
                    : new SolidBrush(warCountryListBox.ForeColor);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            string s = warCountryListBox.Items[e.Index].ToString();
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
            if (warListView.SelectedItems.Count == 0)
            {
                return null;
            }

            return warListView.SelectedItems[0].Tag as War;
        }

        #endregion

        #endregion

        #region 関係タブ

        #region 関係タブ - 共通

        /// <summary>
        ///     関係タブの項目を初期化する
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
        ///     関係タブの項目を更新する
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
        ///     選択国リストボックスの選択項目変更時の処理
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
                item.SubItems.Add(((settings != null) && (settings.Master == target)) ? Resources.Yes : "");
                item.SubItems.Add(((settings != null) && (settings.Control == target)) ? Resources.Yes : "");
                item.SubItems.Add(((relation != null) && relation.Access) ? Resources.Yes : "");
                item.SubItems.Add(((relation != null) && (relation.Guaranteed != null)) ? Resources.Yes : "");
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
            diplomacyGroupBox.Enabled = false;
            intelligenceGroupBox.Enabled = false;

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
        ///     関係タブで選択中の対象国を取得する
        /// </summary>
        /// <returns>対象国</returns>
        private Country GetSelectedRelationCountry()
        {
            if (relationCountryListBox.SelectedIndex < 0)
            {
                return Country.None;
            }

            return Countries.Tags[relationCountryListBox.SelectedIndex];
        }

        /// <summary>
        ///     関係タブで選択中の相手国を取得する
        /// </summary>
        /// <returns>相手国</returns>
        private Country GetSelectedRelationTarget()
        {
            if (relationListView.SelectedItems.Count == 0)
            {
                return Country.None;
            }

            return Countries.Tags[relationListView.SelectedIndices[0]];
        }

        #endregion

        #region 関係タブ - 外交

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
            if ((settings != null) && (masterCheckBox.Checked == (settings.Master == target)))
            {
                return;
            }

            Log.Info("[Scenario] master: {0} -> {1} ({2} > {3})",
                (settings != null) ? BoolHelper.ToYesNo(settings.Master == target) : "",
                BoolHelper.ToYesNo(masterCheckBox.Checked), Countries.Strings[(int) self],
                Countries.Strings[(int) target]);

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
            settings.Master = masterCheckBox.Checked ? target : Country.None;

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
            if ((settings != null) && (controlCheckBox.Checked == (settings.Control == target)))
            {
                return;
            }

            Log.Info("[Scenario] military control: {0} -> {1} ({2} > {3})",
                (settings != null) ? BoolHelper.ToYesNo(settings.Control == target) : "",
                BoolHelper.ToYesNo(controlCheckBox.Checked), Countries.Strings[(int) self],
                Countries.Strings[(int) target]);

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
            settings.Control = controlCheckBox.Checked ? target : Country.None;

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
            if ((relation != null) && (accessCheckBox.Checked == relation.Access))
            {
                return;
            }

            Log.Info("[Scenario] military access: {0} -> {1} ({2} > {3})",
                (relation != null) ? BoolHelper.ToYesNo(relation.Access) : "",
                BoolHelper.ToYesNo(accessCheckBox.Checked), Countries.Strings[(int) self],
                Countries.Strings[(int) target]);

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
            relation.Access = accessCheckBox.Checked;

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
            if ((relation != null) && (guaranteeCheckBox.Checked == (relation.Guaranteed != null)))
            {
                return;
            }

            Log.Info("[Scenario] guarantee: {0} -> {1} ({2} > {3})",
                (relation != null) ? BoolHelper.ToYesNo(relation.Guaranteed == null) : "",
                BoolHelper.ToYesNo(guaranteeCheckBox.Checked), Countries.Strings[(int) self],
                Countries.Strings[(int) target]);

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
            relation.Guaranteed = guaranteeCheckBox.Checked ? new GameDate() : null;

            // 編集済みフラグを設定する
            relation.SetDirty(RelationItemId.Guaranteed);
            relation.SetDirty(RelationItemId.GuaranteedYear);
            relation.SetDirty(RelationItemId.GuaranteedMonth);
            relation.SetDirty(RelationItemId.GuaranteedDay);
            settings.SetDirty();
            Scenarios.SetDirty();

            // 関係リストビューの表示を更新する
            relationListView.SelectedItems[0].SubItems[5].Text = guaranteeCheckBox.Checked ? Resources.Yes : "";

            // 編集項目を更新する
            bool flag = guaranteeCheckBox.Checked;
            guaranteeYearTextBox.Text = flag ? IntHelper.ToString(relation.Guaranteed.Year) : "";
            guaranteeMonthTextBox.Text = flag ? IntHelper.ToString(relation.Guaranteed.Month) : "";
            guaranteeDayTextBox.Text = flag ? IntHelper.ToString(relation.Guaranteed.Day) : "";

            guaranteeEndLabel.Enabled = flag;
            guaranteeYearTextBox.Enabled = flag;
            guaranteeMonthTextBox.Enabled = flag;
            guaranteeDayTextBox.Enabled = flag;

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
            if (nonAggressionCheckBox.Checked == (nonAggression != null))
            {
                return;
            }

            Log.Info("[Scenario] non aggression: {0} -> {1} ({2} > {3})", BoolHelper.ToYesNo(nonAggression != null),
                BoolHelper.ToYesNo(nonAggressionCheckBox.Checked), Countries.Strings[(int) self],
                Countries.Strings[(int) target]);

            // 値を更新する
            if (nonAggression == null)
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
            bool flag = nonAggressionCheckBox.Checked && (nonAggression.StartDate != null);
            nonAggressionStartYearTextBox.Text = flag ? IntHelper.ToString(nonAggression.StartDate.Year) : "";
            nonAggressionStartMonthTextBox.Text = flag ? IntHelper.ToString(nonAggression.StartDate.Month) : "";
            nonAggressionStartDayTextBox.Text = flag ? IntHelper.ToString(nonAggression.StartDate.Day) : "";

            flag = nonAggressionCheckBox.Checked && (nonAggression.EndDate != null);
            nonAggressionEndYearTextBox.Text = flag ? IntHelper.ToString(nonAggression.EndDate.Year) : "";
            nonAggressionEndMonthTextBox.Text = flag ? IntHelper.ToString(nonAggression.EndDate.Month) : "";
            nonAggressionEndDayTextBox.Text = flag ? IntHelper.ToString(nonAggression.EndDate.Day) : "";

            flag = nonAggressionCheckBox.Checked && (nonAggression.Id != null);
            nonAggressionTypeTextBox.Text = flag ? IntHelper.ToString(nonAggression.Id.Type) : "";
            nonAggressionIdTextBox.Text = flag ? IntHelper.ToString(nonAggression.Id.Id) : "";

            flag = nonAggressionCheckBox.Checked;
            nonAggressionStartLabel.Enabled = flag;
            nonAggressionStartYearTextBox.Enabled = flag;
            nonAggressionStartMonthTextBox.Enabled = flag;
            nonAggressionStartDayTextBox.Enabled = flag;
            nonAggressionEndLabel.Enabled = flag;
            nonAggressionEndYearTextBox.Enabled = flag;
            nonAggressionEndMonthTextBox.Enabled = flag;
            nonAggressionEndDayTextBox.Enabled = flag;
            nonAggressionTypeTextBox.Enabled = flag;
            nonAggressionIdTextBox.Enabled = flag;

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
            if (peaceCheckBox.Checked == (peace != null))
            {
                return;
            }

            Log.Info("[Scenario] peace: {0} -> {1} ({2} > {3})", BoolHelper.ToYesNo(peace != null),
                BoolHelper.ToYesNo(peaceCheckBox.Checked), Countries.Strings[(int) self],
                Countries.Strings[(int) target]);

            // 値を更新する
            if (peace == null)
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
            bool flag = peaceCheckBox.Checked && (peace.StartDate != null);
            peaceStartYearTextBox.Text = flag ? IntHelper.ToString(peace.StartDate.Year) : "";
            peaceStartMonthTextBox.Text = flag ? IntHelper.ToString(peace.StartDate.Month) : "";
            peaceStartDayTextBox.Text = flag ? IntHelper.ToString(peace.StartDate.Day) : "";

            flag = peaceCheckBox.Checked && (peace.EndDate != null);
            peaceEndYearTextBox.Text = flag ? IntHelper.ToString(peace.EndDate.Year) : "";
            peaceEndMonthTextBox.Text = flag ? IntHelper.ToString(peace.EndDate.Month) : "";
            peaceEndDayTextBox.Text = flag ? IntHelper.ToString(peace.EndDate.Day) : "";

            flag = peaceCheckBox.Checked && (peace.Id != null);
            peaceTypeTextBox.Text = flag ? IntHelper.ToString(peace.Id.Type) : "";
            peaceIdTextBox.Text = flag ? IntHelper.ToString(peace.Id.Id) : "";

            flag = peaceCheckBox.Checked;
            peaceStartLabel.Enabled = flag;
            peaceStartYearTextBox.Enabled = flag;
            peaceStartMonthTextBox.Enabled = flag;
            peaceStartDayTextBox.Enabled = flag;
            peaceEndLabel.Enabled = flag;
            peaceEndYearTextBox.Enabled = flag;
            peaceEndMonthTextBox.Enabled = flag;
            peaceEndDayTextBox.Enabled = flag;
            peaceTypeTextBox.Enabled = flag;
            peaceIdTextBox.Enabled = flag;

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
        ///     貿易タブを初期化する
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
            tradeEnergyLabel.Text = Config.GetText(EnergyName);
            tradeMetalLabel.Text = Config.GetText(MetalName);
            tradeRareMaterialsLabel.Text = Config.GetText(RareMaterialsName);
            tradeOilLabel.Text = Config.GetText(OilName);
            tradeSuppliesLabel.Text = Config.GetText(SuppliesName);
            tradeMoneyLabel.Text = Config.GetText(MoneyName);
        }

        /// <summary>
        ///     貿易タブを更新する
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
            tradeInfoGroupBox.Enabled = false;
            tradeDealGroupBox.Enabled = false;

            tradeUpButton.Enabled = false;
            tradeDownButton.Enabled = false;
            tradeRemoveButton.Enabled = false;

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
            if (tradeListView.SelectedItems.Count == 0)
            {
                return null;
            }

            return tradeListView.SelectedItems[0].Tag as Treaty;
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
                sb.AppendFormat("{0}:{1}, ", Config.GetText(EnergyName), DoubleHelper.ToString1(trade.Energy));
            }
            if (!DoubleHelper.IsZero(trade.Metal))
            {
                sb.AppendFormat("{0}:{1}, ", Config.GetText(MetalName), DoubleHelper.ToString1(trade.Metal));
            }
            if (!DoubleHelper.IsZero(trade.RareMaterials))
            {
                sb.AppendFormat("{0}:{1}, ", Config.GetText(RareMaterialsName),
                    DoubleHelper.ToString1(trade.RareMaterials));
            }
            if (!DoubleHelper.IsZero(trade.Oil))
            {
                sb.AppendFormat("{0}:{1}, ", Config.GetText(OilName), DoubleHelper.ToString1(trade.Oil));
            }
            if (!DoubleHelper.IsZero(trade.Supplies))
            {
                sb.AppendFormat("{0}:{1}, ", Config.GetText(SuppliesName), DoubleHelper.ToString1(trade.Supplies));
            }
            if (!DoubleHelper.IsZero(trade.Money))
            {
                sb.AppendFormat("{0}:{1}, ", Config.GetText(MoneyName), DoubleHelper.ToString1(trade.Money));
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
            if (tradeCancelCheckBox.Checked == trade.Cancel)
            {
                return;
            }

            Log.Info("[Scenario] trade cancel: {0} -> {1} ({2})", BoolHelper.ToYesNo(trade.Cancel),
                BoolHelper.ToYesNo(tradeCancelCheckBox.Checked), tradeListView.SelectedIndices[0]);

            // 値を更新する
            trade.Cancel = tradeCancelCheckBox.Checked;

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

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Treaty trade = GetSelectedTrade();
            if (trade != null)
            {
                Brush brush = ((Countries.Tags[e.Index] == trade.Country1) && trade.IsDirty(TreatyItemId.Country1))
                    ? new SolidBrush(Color.Red)
                    : new SolidBrush(SystemColors.WindowText);
                string s = tradeCountryComboBox1.Items[e.Index].ToString();
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

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Treaty trade = GetSelectedTrade();
            if (trade != null)
            {
                Brush brush = ((Countries.Tags[e.Index] == trade.Country2) && trade.IsDirty(TreatyItemId.Country2))
                    ? new SolidBrush(Color.Red)
                    : new SolidBrush(SystemColors.WindowText);
                string s = tradeCountryComboBox2.Items[e.Index].ToString();
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

            // 編集項目を更新する
            int index = tradeCountryComboBox1.SelectedIndex;
            tradeCountryComboBox1.SelectedIndex = tradeCountryComboBox2.SelectedIndex;
            tradeCountryComboBox2.SelectedIndex = index;

            string s = tradeEnergyTextBox1.Text;
            tradeEnergyTextBox1.Text = tradeEnergyTextBox2.Text;
            tradeEnergyTextBox2.Text = s;
            s = tradeMetalTextBox1.Text;
            tradeMetalTextBox1.Text = tradeMetalTextBox2.Text;
            tradeMetalTextBox2.Text = s;
            s = tradeRareMaterialsTextBox1.Text;
            tradeRareMaterialsTextBox1.Text = tradeRareMaterialsTextBox2.Text;
            tradeRareMaterialsTextBox2.Text = s;
            s = tradeOilTextBox1.Text;
            tradeOilTextBox1.Text = tradeOilTextBox2.Text;
            tradeOilTextBox2.Text = s;
            s = tradeSuppliesTextBox1.Text;
            tradeSuppliesTextBox1.Text = tradeSuppliesTextBox2.Text;
            tradeSuppliesTextBox2.Text = s;
            s = tradeMoneyTextBox1.Text;
            tradeMoneyTextBox1.Text = tradeMoneyTextBox2.Text;
            tradeMoneyTextBox2.Text = s;

            // 文字色を変更する
            tradeEnergyTextBox1.ForeColor = Color.Red;
            tradeEnergyTextBox2.ForeColor = Color.Red;
            tradeMetalTextBox1.ForeColor = Color.Red;
            tradeMetalTextBox2.ForeColor = Color.Red;
            tradeRareMaterialsTextBox1.ForeColor = Color.Red;
            tradeRareMaterialsTextBox2.ForeColor = Color.Red;
            tradeOilTextBox1.ForeColor = Color.Red;
            tradeOilTextBox2.ForeColor = Color.Red;
            tradeSuppliesTextBox1.ForeColor = Color.Red;
            tradeSuppliesTextBox2.ForeColor = Color.Red;
            tradeMoneyTextBox1.ForeColor = Color.Red;
            tradeMoneyTextBox2.ForeColor = Color.Red;

            // 項目色を変更するため描画更新する
            tradeCountryComboBox1.Refresh();
            tradeCountryComboBox2.Refresh();

            // 貿易リストビューの項目を更新する
            ListViewItem item = tradeListView.SelectedItems[0];
            item.Text = Countries.GetName(trade.Country1);
            item.SubItems[1].Text = Countries.GetName(trade.Country2);
            item.SubItems[2].Text = GetTradeString(trade);
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
        ///     国家タブを初期化する
        /// </summary>
        private void InitCountryTab()
        {
            // 国家リストボックス
            countryListBox.BeginUpdate();
            countryListBox.Items.Clear();
            foreach (Country country in Countries.Tags)
            {
                countryListBox.Items.Add(Countries.GetTagName(country));
            }
            countryListBox.EndUpdate();

            // 兄弟国コンボボックス
            regularIdComboBox.BeginUpdate();
            regularIdComboBox.Items.Clear();
            regularIdComboBox.Items.Add("");
            foreach (Country country in Countries.Tags)
            {
                regularIdComboBox.Items.Add(Countries.GetTagName(country));
            }
            regularIdComboBox.EndUpdate();

            // 国家資源ラベル
            countryEnergyLabel.Text = Config.GetText(EnergyName);
            countryMetalLabel.Text = Config.GetText(MetalName);
            countryRareMaterialsLabel.Text = Config.GetText(RareMaterialsName);
            countryOilLabel.Text = Config.GetText(OilName);
            countrySuppliesLabel.Text = Config.GetText(SuppliesName);
            countryMoneyLabel.Text = Config.GetText(MoneyName);
            countryTransportsLabel.Text = Config.GetText(TransportsName);
            countryEscortsLabel.Text = Config.GetText(EscortsName);
            countryManpowerLabel.Text = Config.GetText(ManpowerName);
            countryIcLabel.Text = Config.GetText(IcName);
        }

        /// <summary>
        ///     国家タブを更新する
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
            countryInfoGroupBox.Enabled = false;
            countryModifierGroupBox.Enabled = false;
            countryResourceGroupBox.Enabled = false;
            productionGroupBox.Enabled = false;
            aiGroupBox.Enabled = false;

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

            consumerSliderTextBox.Text = "";
            supplySliderTextBox.Text = "";
            productionSliderTextBox.Text = "";
            reinforcementSliderTextBox.Text = "";

            aiFileTextBox.Text = "";
            aiFlagsListView.Items.Clear();
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
            bool flag = (settings != null);

            countryNameTextBox.Text = (flag && !string.IsNullOrEmpty(settings.Name)
                ? Config.GetText(settings.Name)
                : Countries.GetName(country));
            regularIdComboBox.SelectedIndex = (flag && settings.Country != Country.None)
                ? Array.IndexOf(Countries.Tags, settings.Country) + 1
                : 0;
            flagExtTextBox.Text = (flag && !string.IsNullOrEmpty(settings.FlagExt)) ? settings.FlagExt : "";

            belligerenceTextBox.Text = (flag && (settings.Belligerence != 0))
                ? IntHelper.ToString(settings.Belligerence)
                : "";
            dissentTextBox.Text = (flag && !DoubleHelper.IsZero(settings.Dissent))
                ? DoubleHelper.ToString(settings.Dissent)
                : "";
            extraTcTextBox.Text = (flag && !DoubleHelper.IsZero(settings.ExtraTc))
                ? DoubleHelper.ToString(settings.ExtraTc)
                : "";
            nukeTextBox.Text = flag ? IntHelper.ToString(settings.Nuke) : "";

            groundDefEffTextBox.Text = (flag && !DoubleHelper.IsZero(settings.GroundDefEff))
                ? DoubleHelper.ToString(settings.GroundDefEff)
                : "";
            peacetimeIcModifierTextBox.Text = (flag && !DoubleHelper.IsZero(settings.PeacetimeIcModifier))
                ? DoubleHelper.ToString(settings.PeacetimeIcModifier)
                : "";
            wartimeIcModifierTextBox.Text = (flag && !DoubleHelper.IsZero(settings.WartimeIcModifier))
                ? DoubleHelper.ToString(settings.WartimeIcModifier)
                : "";
            industrialModifierTextBox.Text = (flag && !DoubleHelper.IsZero(settings.IndustrialModifier))
                ? DoubleHelper.ToString(settings.IndustrialModifier)
                : "";
            relativeManpowerTextBox.Text = (flag && !DoubleHelper.IsZero(settings.RelativeManpower))
                ? DoubleHelper.ToString(settings.RelativeManpower)
                : "";

            countryEnergyTextBox.Text = flag ? DoubleHelper.ToString(settings.Energy) : "0";
            countryMetalTextBox.Text = flag ? DoubleHelper.ToString(settings.Metal) : "0";
            countryRareMaterialsTextBox.Text = flag ? DoubleHelper.ToString(settings.RareMaterials) : "0";
            countryOilTextBox.Text = flag ? DoubleHelper.ToString(settings.Oil) : "0";
            countrySuppliesTextBox.Text = flag ? DoubleHelper.ToString(settings.Supplies) : "0";
            countryMoneyTextBox.Text = flag ? DoubleHelper.ToString(settings.Money) : "0";
            countryTransportsTextBox.Text = flag ? DoubleHelper.ToString(settings.Transports) : "0";
            countryEscortsTextBox.Text = flag ? DoubleHelper.ToString(settings.Escorts) : "0";
            countryManpowerTextBox.Text = flag ? DoubleHelper.ToString(settings.Manpower) : "0";

            consumerSliderTextBox.Text = (flag && !DoubleHelper.IsZero(settings.ConsumerSlider))
                ? DoubleHelper.ToString(settings.ConsumerSlider)
                : "";

            supplySliderTextBox.Text = (flag && !DoubleHelper.IsZero(settings.SupplySlider))
                ? DoubleHelper.ToString(settings.SupplySlider)
                : "";

            productionSliderTextBox.Text = (flag && !DoubleHelper.IsZero(settings.ProductionSlider))
                ? DoubleHelper.ToString(settings.ProductionSlider)
                : "";

            reinforcementSliderTextBox.Text = (flag && !DoubleHelper.IsZero(settings.ReinforcementSlider))
                ? DoubleHelper.ToString(settings.ReinforcementSlider)
                : "";

            aiFileTextBox.Text = (flag && !string.IsNullOrEmpty(settings.Ai)) ? settings.Ai : "";
            aiFlagsListView.BeginUpdate();
            aiFlagsListView.Items.Clear();
            if (flag && (settings.AiSettings != null) && (settings.AiSettings.Flags != null))
            {
                foreach (KeyValuePair<string, string> pair in settings.AiSettings.Flags)
                {
                    ListViewItem item = new ListViewItem { Text = pair.Key };
                    item.SubItems[1].Text = pair.Value;
                }
            }
            aiFlagsListView.EndUpdate();

            flag = ((settings != null) && (settings.NukeDate != null));
            nukeYearTextBox.Text = flag ? IntHelper.ToString(settings.NukeDate.Year) : "";
            nukeMonthTextBox.Text = flag ? IntHelper.ToString(settings.NukeDate.Month) : "";
            nukeDayTextBox.Text = flag ? IntHelper.ToString(settings.NukeDate.Day) : "";

            flag = ((settings != null) && (settings.Offmap != null));
            offmapEnergyTextBox.Text = (flag && !DoubleHelper.IsZero(settings.Offmap.Energy))
                ? DoubleHelper.ToString(settings.Offmap.Energy)
                : "";
            offmapMetalTextBox.Text = (flag && !DoubleHelper.IsZero(settings.Offmap.Metal))
                ? DoubleHelper.ToString(settings.Offmap.Metal)
                : "";
            offmapRareMaterialsTextBox.Text = (flag && !DoubleHelper.IsZero(settings.Offmap.RareMaterials))
                ? DoubleHelper.ToString(settings.Offmap.RareMaterials)
                : "";
            offmapOilTextBox.Text = (flag && !DoubleHelper.IsZero(settings.Offmap.Oil))
                ? DoubleHelper.ToString(settings.Offmap.Oil)
                : "";
            offmapSuppliesTextBox.Text = (flag && !DoubleHelper.IsZero(settings.Offmap.Supplies))
                ? DoubleHelper.ToString(settings.Offmap.Supplies)
                : "";
            offmapMoneyTextBox.Text = (flag && !DoubleHelper.IsZero(settings.Offmap.Money))
                ? DoubleHelper.ToString(settings.Offmap.Money)
                : "";
            offmapTransportsTextBox.Text = (flag && !DoubleHelper.IsZero(settings.Offmap.Transports))
                ? DoubleHelper.ToString(settings.Offmap.Transports)
                : "";
            offmapEscortsTextBox.Text = (flag && !DoubleHelper.IsZero(settings.Offmap.Escorts))
                ? DoubleHelper.ToString(settings.Offmap.Escorts)
                : "";
            offmapManpowerTextBox.Text = (flag && !DoubleHelper.IsZero(settings.Offmap.Manpower))
                ? DoubleHelper.ToString(settings.Offmap.Manpower)
                : "";
            offmapIcTextBox.Text = (flag && !DoubleHelper.IsZero(settings.Offmap.Ic))
                ? DoubleHelper.ToString(settings.Offmap.Ic)
                : "";

            // 編集項目を有効化する
            countryInfoGroupBox.Enabled = true;
            countryModifierGroupBox.Enabled = true;
            countryResourceGroupBox.Enabled = true;
            productionGroupBox.Enabled = true;
            aiGroupBox.Enabled = true;
        }

        /// <summary>
        ///     選択中の国家を取得する
        /// </summary>
        /// <returns></returns>
        private Country GetSelectedCountry()
        {
            if (countryListBox.SelectedIndex < 0)
            {
                return Country.None;
            }
            return Countries.Tags[countryListBox.SelectedIndex];
        }

        #endregion

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