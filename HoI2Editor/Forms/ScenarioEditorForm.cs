using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
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

        private ushort _prevId;

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
            var worker = new BackgroundWorker();
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
            // 編集項目を更新する
            UpdateEditableItems();
        }

        /// <summary>
        ///     編集項目を初期化する
        /// </summary>
        private void InitEditableItems()
        {
            InitMainItems();
            InitAllianceItems();
            InitTradeItems();
        }

        /// <summary>
        ///     編集項目を更新する
        /// </summary>
        private void UpdateEditableItems()
        {
            UpdateMainItems();
            UpdateAllianceItems();
            UpdateTradeItems();
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

        /// <summary>
        ///     メインタブの項目を初期化する
        /// </summary>
        private void InitMainItems()
        {
            // シナリオリストボックス
            InitScenarioListBox();

            // AIの攻撃性コンボボックス
            aiAggressiveComboBox.Items.Clear();
            for (int i = 0; i < ScenarioHeader.AiAggressiveCount; i++)
            {
                aiAggressiveComboBox.Items.Add(Config.GetText(_aiAggressiveStrings[i]));
            }

            // 難易度コンボボックス
            difficultyComboBox.Items.Clear();
            for (int i = 0; i < ScenarioHeader.DifficultyCount; i++)
            {
                difficultyComboBox.Items.Add(Config.GetText(_difficultyStrings[i]));
            }

            // ゲームスピードコンボボックス
            gameSpeedComboBox.Items.Clear();
            for (int i = 0; i < ScenarioHeader.GameSpeedCount; i++)
            {
                gameSpeedComboBox.Items.Add(Config.GetText(_gameSpeedStrings[i]));
            }
        }

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
        ///     メインタブの項目を更新する
        /// </summary>
        private void UpdateMainItems()
        {
            Scenario scenario = Scenarios.Data;

            scenarioNameTextBox.Text = Config.GetText(scenario.Name);
            panelImageTextBox.Text = scenario.PanelName;
            Image old = panelPictureBox.Image;
            panelPictureBox.Image = GetPanelImage(scenario.PanelName);
            if (old != null)
            {
                old.Dispose();
            }

            startYearTextBox.Text = scenario.GlobalData.StartDate.Year.ToString(CultureInfo.InvariantCulture);
            startMonthTextBox.Text = scenario.GlobalData.StartDate.Month.ToString(CultureInfo.InvariantCulture);
            startDayTextBox.Text = scenario.GlobalData.StartDate.Day.ToString(CultureInfo.InvariantCulture);
            endYearTextBox.Text = scenario.GlobalData.EndDate.Year.ToString(CultureInfo.InvariantCulture);
            endMonthTextBox.Text = scenario.GlobalData.EndDate.Month.ToString(CultureInfo.InvariantCulture);
            endDayTextBox.Text = scenario.GlobalData.EndDate.Day.ToString(CultureInfo.InvariantCulture);

            includeFolderTextBox.Text = scenario.IncludeFolder;

            battleScenarioCheckBox.Checked = scenario.Header.IsCombatScenario;
            freeCountryCheckBox.Checked = scenario.Header.IsFreeSelection;

            allowDiplomacyCheckBox.Checked = scenario.GlobalData.Rules.AllowDiplomacy;
            allowProductionCheckBox.Checked = scenario.GlobalData.Rules.AllowProduction;
            allowTechnologyCheckBox.Checked = scenario.GlobalData.Rules.AllowTechnology;

            aiAggressiveComboBox.SelectedIndex = scenario.Header.AiAggressive;
            difficultyComboBox.SelectedIndex = scenario.Header.Difficulty;
            gameSpeedComboBox.SelectedIndex = scenario.Header.GameSpeed;

            var major = new List<Country>();
            majorListBox.Items.Clear();
            foreach (MajorCountrySettings majorCountry in scenario.Header.MajorCountries)
            {
                string tag = Countries.Strings[(int) majorCountry.Country];
                string name = Config.ExistsKey(tag)
                    ? string.Format("{0} {1}", tag, Config.GetText(tag))
                    : tag;
                majorListBox.Items.Add(name);
                major.Add(majorCountry.Country);
            }

            selectableCheckedListBox.Items.Clear();
            foreach (Country country in Countries.Tags)
            {
                // 主要国リストに表示されている国は追加しない
                if (major.Contains(country))
                {
                    continue;
                }

                string tag = Countries.Strings[(int) country];
                string name = Config.ExistsKey(tag)
                    ? string.Format("{0} {1}", tag, Config.GetText(tag))
                    : tag;
                bool check = scenario.Header.SelectableCountries.Contains(country);
                selectableCheckedListBox.Items.Add(name, check);
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
            var button = sender as RadioButton;
            if (button != null && button.Checked)
            {
                UpdateScenarioListBox();
            }
        }

        /// <summary>
        ///     主要国リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMajorListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            int index = majorListBox.SelectedIndex;
            Image image;

            if (index < 0)
            {
                // 選択項目がなければ表示をクリアする
                countryDescTextBox.Text = "";
                propagandaTextBox.Text = "";
                image = null;
            }
            else
            {
                ScenarioHeader header = Scenarios.Data.Header;
                MajorCountrySettings major = header.MajorCountries[index];
                int year = header.StartDate != null ? header.StartDate.Year : header.StartYear;
                countryDescTextBox.Text = GetCountryDesc(major.Country, year, major.Desc);

                propagandaTextBox.Text = major.PictureName;
                image = GetCountryPropagandaImage(major.Country, major.PictureName);
            }

            Image old = propagandaPictureBox.Image;
            propagandaPictureBox.Image = image;
            if (old != null)
            {
                old.Dispose();
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

            var bitmap = new Bitmap(pathName);
            bitmap.MakeTransparent(Color.Lime);
            return bitmap;
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

        #region 同盟タブ

        /// <summary>
        ///     同盟タブの項目を初期化する
        /// </summary>
        private void InitAllianceItems()
        {
            // 何もしない
        }

        /// <summary>
        ///     同盟タブの項目を更新する
        /// </summary>
        private void UpdateAllianceItems()
        {
            ScenarioGlobalData data = Scenarios.Data.GlobalData;

            // 同盟リストビュー
            allianceListView.BeginUpdate();
            allianceListView.Items.Clear();
            if (data.Axis != null)
            {
                // 枢軸国
                var item = new ListViewItem {Text = Config.GetText("EYR_AXIS"), Tag = data.Axis};
                item.SubItems.Add(GetCountryListString(data.Axis.Participant));
                allianceListView.Items.Add(item);
            }
            if (data.Allies != null)
            {
                // 連合国
                var item = new ListViewItem {Text = Config.GetText("EYR_ALLIES"), Tag = data.Allies};
                item.SubItems.Add(GetCountryListString(data.Allies.Participant));
                allianceListView.Items.Add(item);
            }
            if (data.Allies != null)
            {
                // 共産国
                var item = new ListViewItem {Text = Config.GetText("EYR_COM"), Tag = data.Comintern};
                item.SubItems.Add(GetCountryListString(data.Comintern.Participant));
                allianceListView.Items.Add(item);
            }
            foreach (Alliance alliance in data.Alliances)
            {
                // その他の同盟
                var item = new ListViewItem {Text = Resources.Alliance, Tag = alliance};
                item.SubItems.Add(GetCountryListString(alliance.Participant));
                allianceListView.Items.Add(item);
            }
            allianceListView.EndUpdate();

            // 戦争リストビュー
            warListView.BeginUpdate();
            warListView.Items.Clear();
            foreach (War war in data.Wars)
            {
                var item = new ListViewItem {Text = war.StartDate.ToString(), Tag = war};
                item.SubItems.Add(war.EndDate.ToString());
                item.SubItems.Add(GetCountryListString(war.Attackers.Participant));
                item.SubItems.Add(GetCountryListString(war.Defenders.Participant));
                warListView.Items.Add(item);
            }
            warListView.EndUpdate();
        }

        /// <summary>
        ///     同盟リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (allianceListView.SelectedItems.Count == 0)
            {
                return;
            }

            var alliance = allianceListView.SelectedItems[0].Tag as Alliance;
            if (alliance == null)
            {
                return;
            }

            // 同盟国家リストボックス
            allianceCountryListBox.BeginUpdate();
            allianceCountryListBox.Items.Clear();
            foreach (Country country in Countries.Tags)
            {
                if (alliance.Participant.Contains(country))
                {
                    continue;
                }
                allianceCountryListBox.Items.Add(GetCountryTagName(country));
            }
            allianceCountryListBox.EndUpdate();

            // 同盟参加国リストボックス
            allianceParticipantListBox.BeginUpdate();
            allianceParticipantListBox.Items.Clear();
            foreach (Country country in alliance.Participant)
            {
                allianceParticipantListBox.Items.Add(GetCountryTagName(country));
            }
            allianceParticipantListBox.EndUpdate();
        }

        /// <summary>
        ///     戦争リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (warListView.SelectedItems.Count == 0)
            {
                return;
            }

            var war = warListView.SelectedItems[0].Tag as War;
            if (war == null)
            {
                return;
            }

            // 戦争国家リストボックス
            warCountryListBox.BeginUpdate();
            warCountryListBox.Items.Clear();
            foreach (Country country in Countries.Tags)
            {
                if (war.Attackers.Participant.Contains(country) || war.Defenders.Participant.Contains(country))
                {
                    continue;
                }
                warCountryListBox.Items.Add(GetCountryTagName(country));
            }
            warCountryListBox.EndUpdate();

            // 攻撃側リストボックス
            warAttackerListBox.BeginUpdate();
            warAttackerListBox.Items.Clear();
            foreach (Country country in war.Attackers.Participant)
            {
                warAttackerListBox.Items.Add(GetCountryTagName(country));
            }
            warAttackerListBox.EndUpdate();

            // 防御側リストボックス
            warDefenderListBox.BeginUpdate();
            warDefenderListBox.Items.Clear();
            foreach (Country country in war.Defenders.Participant)
            {
                warDefenderListBox.Items.Add(GetCountryTagName(country));
            }
            warDefenderListBox.EndUpdate();
        }

        #endregion

        #region 関係タブ

        #endregion

        #region 貿易タブ

        /// <summary>
        ///     貿易タブの項目を初期化する
        /// </summary>
        private void InitTradeItems()
        {
            // 貿易国家コンボボックス
            tradeCountryComboBox1.BeginUpdate();
            tradeCountryComboBox2.BeginUpdate();
            tradeCountryComboBox1.Items.Clear();
            tradeCountryComboBox2.Items.Clear();
            foreach (Country country in Countries.Tags)
            {
                string s = GetCountryTagName(country);
                tradeCountryComboBox1.Items.Add(s);
                tradeCountryComboBox2.Items.Add(s);
            }
            tradeCountryComboBox1.EndUpdate();
            tradeCountryComboBox2.EndUpdate();

            // 貿易資源ラベル
            tradeEnergyLabel.Text = Config.GetText("RESOURCE_ENERGY");
            tradeMetalLabel.Text = Config.GetText("RESOURCE_METAL");
            tradeRareMaterialsLabel.Text = Config.GetText("RESOURCE_RARE_MATERIALS");
            tradeOilLabel.Text = Config.GetText("RESOURCE_OIL");
            tradeSuppliesLabel.Text = Config.GetText("RESOURCE_SUPPLY");
            tradeMoneyLabel.Text = Config.GetText("RESOURCE_MONEY");
        }

        /// <summary>
        ///     貿易タブの項目を更新する
        /// </summary>
        private void UpdateTradeItems()
        {
            ScenarioGlobalData data = Scenarios.Data.GlobalData;

            // 貿易リストビュー
            tradeListView.BeginUpdate();
            tradeListView.Items.Clear();
            foreach (Treaty treaty in data.Treaties.Where(treaty => treaty.Type == TreatyType.Trade))
            {
                var item = new ListViewItem {Text = treaty.StartDate.ToString(), Tag = treaty};
                item.SubItems.Add(treaty.EndDate.ToString());
                item.SubItems.Add(GetCountryName(treaty.Country1));
                item.SubItems.Add(GetCountryName(treaty.Country2));
                item.SubItems.Add(GetTradeString(treaty));
                tradeListView.Items.Add(item);
            }
            tradeListView.EndUpdate();
        }

        /// <summary>
        ///     貿易リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (tradeListView.SelectedItems.Count == 0)
            {
                return;
            }

            var treaty = tradeListView.SelectedItems[0].Tag as Treaty;
            if (treaty == null)
            {
                return;
            }

            // 開始日時
            tradeStartYearTextBox.Text = treaty.StartDate.Year.ToString(CultureInfo.InvariantCulture);
            tradeStartMonthTextBox.Text = treaty.StartDate.Month.ToString(CultureInfo.InvariantCulture);
            tradeStartDayTextBox.Text = treaty.StartDate.Day.ToString(CultureInfo.InvariantCulture);

            // 終了日時
            tradeEndYearTextBox.Text = treaty.EndDate.Year.ToString(CultureInfo.InvariantCulture);
            tradeEndMonthTextBox.Text = treaty.EndDate.Month.ToString(CultureInfo.InvariantCulture);
            tradeEndDayTextBox.Text = treaty.EndDate.Day.ToString(CultureInfo.InvariantCulture);

            // 貿易国家コンボボックス
            if (Countries.Tags.Contains(treaty.Country1))
            {
                tradeCountryComboBox1.SelectedIndex = Array.IndexOf(Countries.Tags, treaty.Country1);
            }
            if (Countries.Tags.Contains(treaty.Country2))
            {
                tradeCountryComboBox2.SelectedIndex = Array.IndexOf(Countries.Tags, treaty.Country2);
            }

            // 貿易量
            if (!DoubleHelper.IsZero(treaty.Energy))
            {
                if (treaty.Energy < 0)
                {
                    tradeEnergyTextBox1.Text = DoubleHelper.ToString1(Math.Abs(treaty.Energy));
                    tradeEnergyTextBox2.Text = "";
                }
                else
                {
                    tradeEnergyTextBox1.Text = "";
                    tradeEnergyTextBox2.Text = DoubleHelper.ToString1(treaty.Energy);
                }
            }
            else
            {
                tradeEnergyTextBox1.Text = "";
                tradeEnergyTextBox2.Text = "";
            }
            if (!DoubleHelper.IsZero(treaty.Metal))
            {
                if (treaty.Metal < 0)
                {
                    tradeMetalTextBox1.Text = DoubleHelper.ToString1(Math.Abs(treaty.Metal));
                    tradeMetalTextBox2.Text = "";
                }
                else
                {
                    tradeMetalTextBox1.Text = "";
                    tradeMetalTextBox2.Text = DoubleHelper.ToString1(treaty.Metal);
                }
            }
            else
            {
                tradeMetalTextBox1.Text = "";
                tradeMetalTextBox2.Text = "";
            }
            if (!DoubleHelper.IsZero(treaty.RareMaterials))
            {
                if (treaty.RareMaterials < 0)
                {
                    tradeRareMaterialsTextBox1.Text = DoubleHelper.ToString1(Math.Abs(treaty.RareMaterials));
                    tradeRareMaterialsTextBox2.Text = "";
                }
                else
                {
                    tradeRareMaterialsTextBox1.Text = "";
                    tradeRareMaterialsTextBox2.Text = DoubleHelper.ToString1(treaty.RareMaterials);
                }
            }
            else
            {
                tradeRareMaterialsTextBox1.Text = "";
                tradeRareMaterialsTextBox2.Text = "";
            }
            if (!DoubleHelper.IsZero(treaty.Oil))
            {
                if (treaty.Oil < 0)
                {
                    tradeOilTextBox1.Text = DoubleHelper.ToString1(Math.Abs(treaty.Oil));
                    tradeOilTextBox2.Text = "";
                }
                else
                {
                    tradeOilTextBox1.Text = "";
                    tradeOilTextBox2.Text = DoubleHelper.ToString1(treaty.Oil);
                }
            }
            else
            {
                tradeOilTextBox1.Text = "";
                tradeOilTextBox2.Text = "";
            }
            if (!DoubleHelper.IsZero(treaty.Supplies))
            {
                if (treaty.Supplies < 0)
                {
                    tradeSuppliesTextBox1.Text = DoubleHelper.ToString1(Math.Abs(treaty.Supplies));
                    tradeSuppliesTextBox2.Text = "";
                }
                else
                {
                    tradeSuppliesTextBox1.Text = "";
                    tradeSuppliesTextBox2.Text = DoubleHelper.ToString1(treaty.Supplies);
                }
            }
            else
            {
                tradeSuppliesTextBox1.Text = "";
                tradeSuppliesTextBox2.Text = "";
            }
            if (!DoubleHelper.IsZero(treaty.Money))
            {
                if (treaty.Money < 0)
                {
                    tradeMoneyTextBox1.Text = DoubleHelper.ToString1(Math.Abs(treaty.Money));
                    tradeMoneyTextBox2.Text = "";
                }
                else
                {
                    tradeMoneyTextBox1.Text = "";
                    tradeMoneyTextBox2.Text = DoubleHelper.ToString1(treaty.Money);
                }
            }
            else
            {
                tradeMoneyTextBox1.Text = "";
                tradeMoneyTextBox2.Text = "";
            }

            // キャンセルを許可
            tradeCancelCheckBox.Checked = treaty.Cancel;
        }

        #endregion

        #region 文字列操作

        /// <summary>
        ///     国名を取得する
        /// </summary>
        /// <param name="country">国タグ</param>
        /// <returns>国名</returns>
        private static string GetCountryName(Country country)
        {
            if (country == Country.None)
            {
                return "";
            }
            string tag = Countries.Strings[(int) country];
            return Config.ExistsKey(tag) ? Config.GetText(tag) : tag;
        }

        /// <summary>
        ///     国タグ名と国名を取得する
        /// </summary>
        /// <param name="country">国タグ</param>
        /// <returns>国タグ名と国名</returns>
        private static string GetCountryTagName(Country country)
        {
            if (country == Country.None)
            {
                return "";
            }
            string tag = Countries.Strings[(int) country];
            return Config.ExistsKey(tag)
                ? string.Format("{0} {1}", tag, Config.GetText(tag))
                : tag;
        }

        /// <summary>
        ///     国タグリストの文字列を取得する
        /// </summary>
        /// <param name="countries">国タグリスト</param>
        /// <returns>国タグリストの文字列</returns>
        private static string GetCountryListString(IEnumerable<Country> countries)
        {
            var sb = new StringBuilder();
            foreach (Country country in countries)
            {
                sb.AppendFormat("{0}, ", GetCountryName(country));
            }
            int len = sb.Length;
            return (len > 0) ? sb.ToString(0, len - 2) : "";
        }

        /// <summary>
        ///     貿易内容の文字列を取得する
        /// </summary>
        /// <param name="treaty">外交協定情報</param>
        /// <returns>貿易内容の文字列</returns>
        private static string GetTradeString(Treaty treaty)
        {
            var sb = new StringBuilder();
            if (!DoubleHelper.IsZero(treaty.Energy))
            {
                sb.AppendFormat("{0}:{1}, ", Config.GetText("RESOURCE_ENERGY"), DoubleHelper.ToString1(treaty.Energy));
            }
            if (!DoubleHelper.IsZero(treaty.Metal))
            {
                sb.AppendFormat("{0}:{1}, ", Config.GetText("RESOURCE_METAL"), DoubleHelper.ToString1(treaty.Metal));
            }
            if (!DoubleHelper.IsZero(treaty.RareMaterials))
            {
                sb.AppendFormat("{0}:{1}, ", Config.GetText("RESOURCE_RARE_MATERIALS"),
                    DoubleHelper.ToString1(treaty.RareMaterials));
            }
            if (!DoubleHelper.IsZero(treaty.Oil))
            {
                sb.AppendFormat("{0}:{1}, ", Config.GetText("RESOURCE_OIL"), DoubleHelper.ToString1(treaty.Oil));
            }
            if (!DoubleHelper.IsZero(treaty.Supplies))
            {
                sb.AppendFormat("{0}:{1}, ", Config.GetText("RESOURCE_SUPPLY"), DoubleHelper.ToString1(treaty.Supplies));
            }
            if (!DoubleHelper.IsZero(treaty.Money))
            {
                sb.AppendFormat("{0}:{1}, ", Config.GetText("RESOURCE_MONEY"), DoubleHelper.ToString1(treaty.Money));
            }
            int len = sb.Length;
            return (len > 0) ? sb.ToString(0, len - 2) : "";
        }

        #endregion

        private void OnTextBox1Validated(object sender, EventArgs e)
        {
            ushort id;
            if (!ushort.TryParse(textBox1.Text, out id))
            {
                return;
            }
            if (id > 0 && id < 10000)
            {
                Map map = Maps.Data[(int) MapLevel.Level1];
                var bitmap = provinceMapPictureBox.Image as Bitmap;
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