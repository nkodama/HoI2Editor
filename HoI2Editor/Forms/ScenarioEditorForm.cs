using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HoI2Editor.Controllers;
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

        /// <summary>
        ///     主要国以外の選択可能国リスト
        /// </summary>
        private List<Country> _majorFreeCountries;

        /// <summary>
        ///     選択可能国以外の国家リスト
        /// </summary>
        private List<Country> _selectableFreeCountries;

        /// <summary>
        ///     同盟国以外の国家リスト
        /// </summary>
        private List<Country> _allianceFreeCountries;

        /// <summary>
        ///     戦争国以外の国家リスト
        /// </summary>
        private List<Country> _warFreeCountries;

        /// <summary>
        ///     技術項目リスト
        /// </summary>
        private List<TechItem> _techs;

        /// <summary>
        ///     発明イベントリスト
        /// </summary>
        private List<TechEvent> _inventions;

        /// <summary>
        ///     技術ツリーパネルのコントローラ
        /// </summary>
        private TechTreePanelController _techTreePanelController;

        /// <summary>
        ///     マップパネルのコントローラ
        /// </summary>
        private MapPanelController _mapPanelController;

        /// <summary>
        ///     マップパネルの初期化フラグ
        /// </summary>
        private bool _mapPanelInitialized;

        /// <summary>
        ///     ユニットツリーのコントローラ
        /// </summary>
        private UnitTreeController _unitTreeController;

        /// <summary>
        ///     選択中の国家
        /// </summary>
        private Country _selectedCountry;

        /// <summary>
        ///     最終のユニットの兵科
        /// </summary>
        private Branch _lastUnitBranch;

        /// <summary>
        ///     最終の師団の兵科
        /// </summary>
        private Branch _lastDivisionBranch;

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
            Province, // プロヴィンス
            Oob // 初期部隊
        }

        /// <summary>
        ///     AIの攻撃性の文字列名
        /// </summary>
        private readonly TextId[] _aiAggressiveNames =
        {
            TextId.OptionAiAggressiveness1,
            TextId.OptionAiAggressiveness2,
            TextId.OptionAiAggressiveness3,
            TextId.OptionAiAggressiveness4,
            TextId.OptionAiAggressiveness5
        };

        /// <summary>
        ///     難易度の文字列名
        /// </summary>
        private readonly TextId[] _difficultyNames =
        {
            TextId.OptionDifficulty1,
            TextId.OptionDifficulty2,
            TextId.OptionDifficulty3,
            TextId.OptionDifficulty4,
            TextId.OptionDifficulty5
        };

        /// <summary>
        ///     ゲームスピードの文字列名
        /// </summary>
        private readonly TextId[] _gameSpeedNames =
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
            // 読み込み前ならば何もしない
            if (!Scenarios.IsLoaded())
            {
                return;
            }

            // シナリオ関連情報を初期化する
            Scenarios.Init();

            // 各タブページの初期化済み状態をクリアする
            foreach (TabPageNo page in Enum.GetValues(typeof (TabPageNo)))
            {
                _tabPageInitialized[(int) page] = false;
            }

            // 編集項目を更新する
            OnMainTabPageFileLoad();
            OnAllianceTabPageFileLoad();
            OnRelationTabPageFileLoad();
            OnTradeTabPageFileLoad();
            OnCountryTabPageFileLoad();
            OnGovernmentTabPageFileLoad();
            OnTechTabPageFileLoad();
            OnProvinceTabPageFileLoad();
            OnOobTabPageFileLoad();
        }

        /// <summary>
        ///     データ保存後の処理
        /// </summary>
        public void OnFileSaved()
        {
            // 各タブページの初期化済み状態をクリアする
            foreach (TabPageNo page in Enum.GetValues(typeof (TabPageNo)))
            {
                _tabPageInitialized[(int) page] = false;
            }

            // 強制的に選択タブの表示を更新する
            OnScenarioTabControlSelectedIndexChanged(null, null);
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
        ///     マップ読み込み完了時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMapFileLoad(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }

            if (e.Cancelled)
            {
                return;
            }

            // プロヴィンスタブ選択中でなければ何もしない
            if (_tabPageNo != TabPageNo.Province)
            {
                return;
            }

            // マップパネルを更新する
            UpdateMapPanel();
        }

        #endregion

        #region フォーム

        /// <summary>
        ///     フォームの初期化
        /// </summary>
        private void InitForm()
        {
            // ウィンドウの位置
            Location = HoI2EditorController.Settings.ScenarioEditor.Location;
            Size = HoI2EditorController.Settings.ScenarioEditor.Size;

            // 技術ツリーパネル
            _techTreePanelController = new TechTreePanelController(techTreePictureBox) { ApplyItemStatus = true };
            _techTreePanelController.ItemMouseClick += OnTechTreeItemMouseClick;
            _techTreePanelController.QueryItemStatus += OnQueryTechTreeItemStatus;

            // マップパネル
            _mapPanelController = new MapPanelController(provinceMapPanel, provinceMapPictureBox);

            // ユニットツリー
            _unitTreeController = new UnitTreeController(unitTreeView);

            // コントローラ
            _controller = new ScenarioEditorController(this, _mapPanelController, _unitTreeController);
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
            Maps.LoadAsync(MapLevel.Level2, OnMapFileLoad);

            // 指揮官データを遅延読み込みする
            Leaders.LoadAsync(null);

            // 閣僚データを遅延読込する
            Ministers.LoadAsync(null);

            // 技術データを遅延読み込みする
            Techs.LoadAsync(null);

            // プロヴィンスデータを遅延読み込みする
            Provinces.LoadAsync(null);

            // ユニットデータを遅延読み込みする
            Units.LoadAsync(null);

            // 表示項目を初期化する
            OnMainTabPageFormLoad();
            OnAllianceTabPageFormLoad();
            OnRelationTabPageFormLoad();
            OnTradeTabPageFormLoad();
            OnCountryTabPageFormLoad();
            OnGovernmentTabPageFormLoad();
            OnTechTabPageFormLoad();
            OnProvinceTabPageFormLoad();
            OnOobTabPageFormLoad();

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
            if (!HoI2EditorController.IsDirty())
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
                    HoI2EditorController.Save();
                    break;
                case DialogResult.No:
                    HoI2EditorController.SaveCanceled = true;
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
            HoI2EditorController.OnScenarioEditorFormClosed();
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
                HoI2EditorController.Settings.ScenarioEditor.Location = Location;
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
                HoI2EditorController.Settings.ScenarioEditor.Size = Size;
            }
        }

        /// <summary>
        ///     チェックボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCheckButtonClick(object sender, EventArgs e)
        {
            // プロヴィンスデータ読み込み完了まで待つ
            Provinces.WaitLoading();

            DataChecker.CheckScenario();
        }

        /// <summary>
        ///     再読み込みボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReloadButtonClick(object sender, EventArgs e)
        {
            // 編集済みならば保存するかを問い合わせる
            if (HoI2EditorController.IsDirty())
            {
                DialogResult result = MessageBox.Show(Resources.ConfirmSaveMessage, Text, MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);
                switch (result)
                {
                    case DialogResult.Cancel:
                        return;
                    case DialogResult.Yes:
                        HoI2EditorController.Save();
                        break;
                }
            }

            HoI2EditorController.Reload();
        }

        /// <summary>
        ///     保存ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            HoI2EditorController.Save();
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
                case TabPageNo.Main:
                    OnMainTabPageSelected();
                    break;

                case TabPageNo.Alliance:
                    OnAllianceTabPageSelected();
                    break;

                case TabPageNo.Relation:
                    OnRelationTabPageSelected();
                    break;

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

                case TabPageNo.Oob:
                    OnOobTabPageSelected();
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
            InitScenarioListBox();
            InitScenarioInfoItems();
            InitScenarioOptionItems();
            InitSelectableItems();
        }

        /// <summary>
        ///     メインタブの編集項目を更新する
        /// </summary>
        private void UpdateMainTab()
        {
            // 初期化済みであれば何もしない
            if (_tabPageInitialized[(int) TabPageNo.Main])
            {
                return;
            }

            // 編集項目を更新する
            UpdateScenarioInfoItems();
            UpdateScenarioOptionItems();

            // 選択可能国リストを更新する
            UpdateSelectableList();

            // メインタブの編集項目を有効化する
            EnableMainItems();

            // 初期化済みフラグをセットする
            _tabPageInitialized[(int) TabPageNo.Main] = true;
        }

        /// <summary>
        ///     メインタブの編集項目を有効化する
        /// </summary>
        private void EnableMainItems()
        {
            scenarioInfoGroupBox.Enabled = true;
            scenarioOptionGroupBox.Enabled = true;
            countrySelectionGroupBox.Enabled = true;
        }

        /// <summary>
        ///     メインタブのフォーム読み込み時の処理
        /// </summary>
        private void OnMainTabPageFormLoad()
        {
            // メインタブを初期化する
            InitMainTab();
        }

        /// <summary>
        ///     メインタブのファイル読み込み時の処理
        /// </summary>
        private void OnMainTabPageFileLoad()
        {
            // メインタブ選択中でなければ何もしない
            if (_tabPageNo != TabPageNo.Main)
            {
                return;
            }

            // 初回遷移時には表示を更新する
            UpdateMainTab();
        }

        /// <summary>
        ///     メインタブ選択時の処理
        /// </summary>
        private void OnMainTabPageSelected()
        {
            // シナリオ未読み込みならば何もしない
            if (!Scenarios.IsLoaded())
            {
                return;
            }

            // 初回遷移時には表示を更新する
            UpdateMainTab();
        }

        #endregion

        #region メインタブ - シナリオ読み込み

        /// <summary>
        ///     シナリオリストボックスを初期化する
        /// </summary>
        private void InitScenarioListBox()
        {
            // フォルダグループボックスのどれかのラジオボタンを有効にする
            vanillaRadioButton.Checked = true;
            if (Game.IsModActive && Directory.Exists(Game.GetModFileName(Game.ScenarioPathName)))
            {
                modRadioButton.Checked = true;
            }
            else
            {
                modRadioButton.Enabled = false;
            }
            if (Game.IsExportFolderActive && Directory.Exists(Game.GetExportFileName(Game.ScenarioPathName)))
            {
                exportRadioButton.Checked = true;
            }
            else
            {
                exportRadioButton.Enabled = false;
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

            // 編集済みならば保存するかを問い合わせる
            if (Scenarios.IsLoaded() && HoI2EditorController.IsDirty())
            {
                DialogResult result = MessageBox.Show(Resources.ConfirmSaveMessage, Text, MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);
                switch (result)
                {
                    case DialogResult.Cancel:
                        return;
                    case DialogResult.Yes:
                        HoI2EditorController.Save();
                        break;
                }
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
        ///     シナリオ情報の編集項目を初期化する
        /// </summary>
        private void InitScenarioInfoItems()
        {
            _itemControls.Add(ScenarioEditorItemId.ScenarioName, scenarioNameTextBox);
            _itemControls.Add(ScenarioEditorItemId.ScenarioPanelName, panelImageTextBox);
            _itemControls.Add(ScenarioEditorItemId.ScenarioStartYear, startYearTextBox);
            _itemControls.Add(ScenarioEditorItemId.ScenarioStartMonth, startMonthTextBox);
            _itemControls.Add(ScenarioEditorItemId.ScenarioStartDay, startDayTextBox);
            _itemControls.Add(ScenarioEditorItemId.ScenarioEndYear, endYearTextBox);
            _itemControls.Add(ScenarioEditorItemId.ScenarioEndMonth, endMonthTextBox);
            _itemControls.Add(ScenarioEditorItemId.ScenarioEndDay, endDayTextBox);
            _itemControls.Add(ScenarioEditorItemId.ScenarioIncludeFolder, includeFolderTextBox);

            scenarioNameTextBox.Tag = ScenarioEditorItemId.ScenarioName;
            panelImageTextBox.Tag = ScenarioEditorItemId.ScenarioPanelName;
            startYearTextBox.Tag = ScenarioEditorItemId.ScenarioStartYear;
            startMonthTextBox.Tag = ScenarioEditorItemId.ScenarioStartMonth;
            startDayTextBox.Tag = ScenarioEditorItemId.ScenarioStartDay;
            endYearTextBox.Tag = ScenarioEditorItemId.ScenarioEndYear;
            endMonthTextBox.Tag = ScenarioEditorItemId.ScenarioEndMonth;
            endDayTextBox.Tag = ScenarioEditorItemId.ScenarioEndDay;
            includeFolderTextBox.Tag = ScenarioEditorItemId.ScenarioIncludeFolder;
        }

        /// <summary>
        ///     シナリオ情報の編集項目を更新する
        /// </summary>
        private void UpdateScenarioInfoItems()
        {
            _controller.UpdateItemValue(scenarioNameTextBox);
            _controller.UpdateItemValue(panelImageTextBox);
            _controller.UpdateItemValue(startYearTextBox);
            _controller.UpdateItemValue(startMonthTextBox);
            _controller.UpdateItemValue(startDayTextBox);
            _controller.UpdateItemValue(endYearTextBox);
            _controller.UpdateItemValue(endMonthTextBox);
            _controller.UpdateItemValue(endDayTextBox);
            _controller.UpdateItemValue(includeFolderTextBox);

            _controller.UpdateItemColor(scenarioNameTextBox);
            _controller.UpdateItemColor(panelImageTextBox);
            _controller.UpdateItemColor(startYearTextBox);
            _controller.UpdateItemColor(startMonthTextBox);
            _controller.UpdateItemColor(startDayTextBox);
            _controller.UpdateItemColor(endYearTextBox);
            _controller.UpdateItemColor(endMonthTextBox);
            _controller.UpdateItemColor(endDayTextBox);
            _controller.UpdateItemColor(includeFolderTextBox);

            UpdatePanelImage(Scenarios.Data.PanelName);
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
        public void UpdatePanelImage(string fileName)
        {
            Image prev = panelPictureBox.Image;
            if (!string.IsNullOrEmpty(fileName) &&
                (fileName.IndexOfAny(Path.GetInvalidPathChars()) < 0))
            {
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
            }
            else
            {
                panelPictureBox.Image = null;
            }
            prev?.Dispose();
        }

        #endregion

        #region メインタブ - オプション

        /// <summary>
        ///     シナリオオプションの編集項目を初期化する
        /// </summary>
        private void InitScenarioOptionItems()
        {
            _itemControls.Add(ScenarioEditorItemId.ScenarioBattleScenario, battleScenarioCheckBox);
            _itemControls.Add(ScenarioEditorItemId.ScenarioFreeSelection, freeCountryCheckBox);
            _itemControls.Add(ScenarioEditorItemId.ScenarioAllowDiplomacy, allowDiplomacyCheckBox);
            _itemControls.Add(ScenarioEditorItemId.ScenarioAllowProduction, allowProductionCheckBox);
            _itemControls.Add(ScenarioEditorItemId.ScenarioAllowTechnology, allowTechnologyCheckBox);
            _itemControls.Add(ScenarioEditorItemId.ScenarioAiAggressive, aiAggressiveComboBox);
            _itemControls.Add(ScenarioEditorItemId.ScenarioDifficulty, difficultyComboBox);
            _itemControls.Add(ScenarioEditorItemId.ScenarioGameSpeed, gameSpeedComboBox);

            battleScenarioCheckBox.Tag = ScenarioEditorItemId.ScenarioBattleScenario;
            freeCountryCheckBox.Tag = ScenarioEditorItemId.ScenarioFreeSelection;
            allowDiplomacyCheckBox.Tag = ScenarioEditorItemId.ScenarioAllowDiplomacy;
            allowProductionCheckBox.Tag = ScenarioEditorItemId.ScenarioAllowProduction;
            allowTechnologyCheckBox.Tag = ScenarioEditorItemId.ScenarioAllowTechnology;
            aiAggressiveComboBox.Tag = ScenarioEditorItemId.ScenarioAiAggressive;
            difficultyComboBox.Tag = ScenarioEditorItemId.ScenarioDifficulty;
            gameSpeedComboBox.Tag = ScenarioEditorItemId.ScenarioGameSpeed;

            // AIの攻撃性コンボボックス
            aiAggressiveComboBox.BeginUpdate();
            aiAggressiveComboBox.Items.Clear();
            for (int i = 0; i < ScenarioHeader.AiAggressiveCount; i++)
            {
                aiAggressiveComboBox.Items.Add(Config.GetText(_aiAggressiveNames[i]));
            }
            aiAggressiveComboBox.EndUpdate();

            // 難易度コンボボックス
            difficultyComboBox.BeginUpdate();
            difficultyComboBox.Items.Clear();
            for (int i = 0; i < ScenarioHeader.DifficultyCount; i++)
            {
                difficultyComboBox.Items.Add(Config.GetText(_difficultyNames[i]));
            }
            difficultyComboBox.EndUpdate();

            // ゲームスピードコンボボックス
            gameSpeedComboBox.BeginUpdate();
            gameSpeedComboBox.Items.Clear();
            for (int i = 0; i < ScenarioHeader.GameSpeedCount; i++)
            {
                gameSpeedComboBox.Items.Add(Config.GetText(_gameSpeedNames[i]));
            }
            gameSpeedComboBox.EndUpdate();
        }

        /// <summary>
        ///     シナリオオプションの編集項目を更新する
        /// </summary>
        private void UpdateScenarioOptionItems()
        {
            _controller.UpdateItemValue(battleScenarioCheckBox);
            _controller.UpdateItemValue(freeCountryCheckBox);
            _controller.UpdateItemValue(allowDiplomacyCheckBox);
            _controller.UpdateItemValue(allowProductionCheckBox);
            _controller.UpdateItemValue(allowTechnologyCheckBox);
            _controller.UpdateItemValue(aiAggressiveComboBox);
            _controller.UpdateItemValue(difficultyComboBox);
            _controller.UpdateItemValue(gameSpeedComboBox);

            _controller.UpdateItemColor(battleScenarioCheckBox);
            _controller.UpdateItemColor(freeCountryCheckBox);
            _controller.UpdateItemColor(allowDiplomacyCheckBox);
            _controller.UpdateItemColor(allowProductionCheckBox);
            _controller.UpdateItemColor(allowTechnologyCheckBox);
            _controller.UpdateItemColor(aiAggressiveComboBox);
            _controller.UpdateItemColor(difficultyComboBox);
            _controller.UpdateItemColor(gameSpeedComboBox);

            aiAggressiveComboBox.Refresh();
            difficultyComboBox.Refresh();
            gameSpeedComboBox.Refresh();
        }

        #endregion

        #region メインタブ - 国家選択

        /// <summary>
        ///     選択可能国リストを更新する
        /// </summary>
        private void UpdateSelectableList()
        {
            List<Country> majors = Scenarios.Data.Header.MajorCountries.Select(major => major.Country).ToList();
            majorListBox.BeginUpdate();
            majorListBox.Items.Clear();
            foreach (Country country in majors)
            {
                majorListBox.Items.Add(Countries.GetTagName(country));
            }
            majorListBox.EndUpdate();

            _majorFreeCountries =
                Scenarios.Data.Header.SelectableCountries.Where(country => !majors.Contains(country)).ToList();
            selectableListBox.BeginUpdate();
            selectableListBox.Items.Clear();
            foreach (Country country in _majorFreeCountries)
            {
                selectableListBox.Items.Add(Countries.GetTagName(country));
            }
            selectableListBox.EndUpdate();

            _selectableFreeCountries =
                Countries.Tags.Where(country => !Scenarios.Data.Header.SelectableCountries.Contains(country)).ToList();
            unselectableListBox.BeginUpdate();
            unselectableListBox.Items.Clear();
            foreach (Country country in _selectableFreeCountries)
            {
                unselectableListBox.Items.Add(Countries.GetTagName(country));
            }
            unselectableListBox.EndUpdate();

            // 主要国の操作ボタンを無効化する
            DisableMajorButtons();

            // 編集項目を無効化する
            DisableSelectableItems();

            // 編集項目をクリアする
            ClearSelectableItems();
        }

        /// <summary>
        ///     選択可能国の編集項目を初期化する
        /// </summary>
        private void InitSelectableItems()
        {
            _itemControls.Add(ScenarioEditorItemId.MajorCountryNameKey, majorCountryNameKeyTextBox);
            _itemControls.Add(ScenarioEditorItemId.MajorCountryNameString, majorCountryNameStringTextBox);
            _itemControls.Add(ScenarioEditorItemId.MajorFlagExt, majorFlagExtTextBox);
            _itemControls.Add(ScenarioEditorItemId.MajorCountryDescKey, countryDescKeyTextBox);
            _itemControls.Add(ScenarioEditorItemId.MajorCountryDescString, countryDescStringTextBox);
            _itemControls.Add(ScenarioEditorItemId.MajorPropaganada, propagandaTextBox);

            majorCountryNameKeyTextBox.Tag = ScenarioEditorItemId.MajorCountryNameKey;
            majorCountryNameStringTextBox.Tag = ScenarioEditorItemId.MajorCountryNameString;
            majorFlagExtTextBox.Tag = ScenarioEditorItemId.MajorFlagExt;
            countryDescKeyTextBox.Tag = ScenarioEditorItemId.MajorCountryDescKey;
            countryDescStringTextBox.Tag = ScenarioEditorItemId.MajorCountryDescString;
            propagandaTextBox.Tag = ScenarioEditorItemId.MajorPropaganada;
        }

        /// <summary>
        ///     選択可能国の編集項目を更新する
        /// </summary>
        /// <param name="major">主要国設定</param>
        private void UpdateSelectableItems(MajorCountrySettings major)
        {
            _controller.UpdateItemValue(majorCountryNameKeyTextBox, major);
            _controller.UpdateItemValue(majorCountryNameStringTextBox, major);
            _controller.UpdateItemValue(majorFlagExtTextBox, major);
            _controller.UpdateItemValue(countryDescKeyTextBox, major);
            _controller.UpdateItemValue(countryDescStringTextBox, major);
            _controller.UpdateItemValue(propagandaTextBox, major);

            _controller.UpdateItemColor(majorCountryNameKeyTextBox, major);
            _controller.UpdateItemColor(majorCountryNameStringTextBox, major);
            _controller.UpdateItemColor(majorFlagExtTextBox, major);
            _controller.UpdateItemColor(countryDescKeyTextBox, major);
            _controller.UpdateItemColor(countryDescStringTextBox, major);
            _controller.UpdateItemColor(propagandaTextBox, major);

            UpdatePropagandaImage(major.Country, major.PictureName);
        }

        /// <summary>
        ///     選択可能国の編集項目をクリアする
        /// </summary>
        private void ClearSelectableItems()
        {
            majorCountryNameKeyTextBox.Text = "";
            majorCountryNameStringTextBox.Text = "";
            majorFlagExtTextBox.Text = "";
            countryDescKeyTextBox.Text = "";
            countryDescStringTextBox.Text = "";
            propagandaTextBox.Text = "";
            Image prev = propagandaPictureBox.Image;
            propagandaPictureBox.Image = null;
            prev?.Dispose();
        }

        /// <summary>
        ///     選択可能国の編集項目を有効化する
        /// </summary>
        private void EnableSelectableItems()
        {
            majorCountryNameLabel.Enabled = true;
            majorCountryNameKeyTextBox.Enabled = (Game.Type == GameType.DarkestHour) && (Game.Version >= 104);
            majorCountryNameStringTextBox.Enabled = true;
            majorFlagExtLabel.Enabled = (Game.Type == GameType.DarkestHour) && (Game.Version >= 104);
            majorFlagExtTextBox.Enabled = (Game.Type == GameType.DarkestHour) && (Game.Version >= 104);
            countryDescLabel.Enabled = true;
            countryDescKeyTextBox.Enabled = true;
            countryDescStringTextBox.Enabled = true;
            propagandaLabel.Enabled = true;
            propagandaTextBox.Enabled = true;
            propagandaBrowseButton.Enabled = true;
        }

        /// <summary>
        ///     選択可能国の編集項目を無効化する
        /// </summary>
        private void DisableSelectableItems()
        {
            majorCountryNameLabel.Enabled = false;
            majorCountryNameKeyTextBox.Enabled = false;
            majorCountryNameStringTextBox.Enabled = false;
            majorFlagExtLabel.Enabled = false;
            majorFlagExtTextBox.Enabled = false;
            countryDescLabel.Enabled = false;
            countryDescKeyTextBox.Enabled = false;
            countryDescStringTextBox.Enabled = false;
            propagandaLabel.Enabled = false;
            propagandaTextBox.Enabled = false;
            propagandaBrowseButton.Enabled = false;
        }

        /// <summary>
        ///     主要国の操作ボタンを有効化する
        /// </summary>
        private void EnableMajorButtons()
        {
            int index = majorListBox.SelectedIndex;
            int count = majorListBox.Items.Count;

            majorRemoveButton.Enabled = true;
            majorUpButton.Enabled = index > 0;
            majorDownButton.Enabled = index < count - 1;
        }

        /// <summary>
        ///     主要国の操作ボタンを無効化する
        /// </summary>
        private void DisableMajorButtons()
        {
            majorRemoveButton.Enabled = false;
            majorUpButton.Enabled = false;
            majorDownButton.Enabled = false;
        }

        /// <summary>
        ///     選択国の操作ボタンを有効化する
        /// </summary>
        private void EnableSelectableButtons()
        {
            majorAddButton.Enabled = true;
            selectableRemoveButton.Enabled = true;
        }

        /// <summary>
        ///     選択国の操作ボタンを無効化する
        /// </summary>
        private void DisableSelectableButtons()
        {
            majorAddButton.Enabled = false;
            selectableRemoveButton.Enabled = false;
        }

        /// <summary>
        ///     非選択国の操作ボタンを有効化する
        /// </summary>
        private void EnableUnselectableButtons()
        {
            selectableAddButton.Enabled = true;
        }

        /// <summary>
        ///     非選択国の操作ボタンを無効化する
        /// </summary>
        private void DisableUnselectableButtons()
        {
            selectableAddButton.Enabled = false;
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

            ListBox control = sender as ListBox;
            if (control == null)
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
                brush = new SolidBrush(dirty ? Color.Red : control.ForeColor);
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

            ListBox control = sender as ListBox;
            if (control == null)
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
                brush = new SolidBrush(dirty ? Color.Red : control.ForeColor);
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

            ListBox control = sender as ListBox;
            if (control == null)
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
                brush = new SolidBrush(dirty ? Color.Red : control.ForeColor);
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

        /// <summary>
        ///     主要国リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMajorListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ編集項目を無効化する
            if (majorListBox.SelectedIndex < 0)
            {
                // 主要国の操作ボタンを無効化する
                DisableMajorButtons();

                // 編集項目を無効化する
                DisableSelectableItems();

                // 編集項目をクリアする
                ClearSelectableItems();
                return;
            }

            MajorCountrySettings major = GetSelectedMajorCountry();

            // 編集項目を更新する
            UpdateSelectableItems(major);

            // 編集項目を有効化する
            EnableSelectableItems();

            // 主要国の操作ボタンを有効化する
            EnableMajorButtons();
        }

        /// <summary>
        ///     選択可能国リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectableListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectableListBox.SelectedItems.Count == 0)
            {
                // 選択国の操作ボタンを無効化する
                DisableSelectableButtons();
                return;
            }

            // 選択国の操作ボタンを有効化する
            EnableSelectableButtons();
        }

        /// <summary>
        ///     非選択国リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnselectableListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (unselectableListBox.SelectedItems.Count == 0)
            {
                // 非選択国の操作ボタンを無効化する
                DisableUnselectableButtons();
                return;
            }

            // 非選択国の操作ボタンを有効化する
            EnableUnselectableButtons();
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
                majorListBox.SelectedIndex = index < majorListBox.Items.Count ? index : index - 1;
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
                propagandaTextBox.Text = Game.GetRelativePathName(dialog.FileName);
            }
        }

        /// <summary>
        ///     選択中の主要国設定を取得する
        /// </summary>
        /// <returns>選択中の主要国設定</returns>
        private MajorCountrySettings GetSelectedMajorCountry()
        {
            if (majorListBox.SelectedIndex < 0)
            {
                return null;
            }
            return Scenarios.Data.Header.MajorCountries[majorListBox.SelectedIndex];
        }

        /// <summary>
        ///     プロパガンダ画像を更新する
        /// </summary>
        /// <param name="country">国タグ</param>
        /// <param name="fileName">プロパガンダ画像名</param>
        public void UpdatePropagandaImage(Country country, string fileName)
        {
            Image prev = propagandaPictureBox.Image;
            propagandaPictureBox.Image = GetPropagandaImage(country, fileName);
            prev?.Dispose();
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
            if (!string.IsNullOrEmpty(fileName) &&
                (fileName.IndexOfAny(Path.GetInvalidPathChars()) < 0))
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
                $"propaganda_{Countries.Strings[(int) country]}.bmp");
            if (!File.Exists(pathName))
            {
                return null;
            }

            bitmap = new Bitmap(pathName);
            bitmap.MakeTransparent(bitmap.GetPixel(0, 0));
            return bitmap;
        }

        #endregion

        #region メインタブ - 編集項目

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnScenarioIntItemTextBoxValidated(object sender, EventArgs e)
        {
            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            // 文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control);
                return;
            }

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId);
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
            if (!_controller.IsItemValueValid(itemId, val))
            {
                _controller.UpdateItemValue(control);
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val);

            // 値を更新する
            _controller.SetItemValue(itemId, val);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val);
        }

        /// <summary>
        ///     テキストボックスの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnScenarioStringItemTextBoxTextChanged(object sender, EventArgs e)
        {
            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            // 値に変化がなければ何もしない
            string val = control.Text;
            if (val.Equals((string) _controller.GetItemValue(itemId)))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val);

            // 値を更新する
            _controller.SetItemValue(itemId, val);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val);
        }

        /// <summary>
        ///     コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnScenarioItemComboBoxDrawItem(object sender, DrawItemEventArgs e)
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
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            int val = (int) _controller.GetItemValue(itemId);
            bool dirty = (e.Index == val) && _controller.IsItemDirty(itemId);
            Brush brush = new SolidBrush(dirty ? Color.Red : control.ForeColor);
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
        private void OnScenarioItemComboBoxSelectedIndexChanged(object sender, EventArgs e)
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
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            // 値に変化がなければ何もしない
            int val = control.SelectedIndex;
            if (val == (int) _controller.GetItemValue(itemId))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val);

            // 値を更新する
            _controller.SetItemValue(itemId, val);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId);

            // 項目色を変更するため描画更新する
            control.Refresh();

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val);
        }

        /// <summary>
        ///     チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnScenarioItemCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            CheckBox control = sender as CheckBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            // 値に変化がなければ何もしない
            bool val = control.Checked;
            object prev = _controller.GetItemValue(itemId);
            if ((prev != null) && (val == (bool) prev))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val);

            // 値を更新する
            _controller.SetItemValue(itemId, val);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val);
        }

        /// <summary>
        ///     テキストボックスの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectableStringItemTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            MajorCountrySettings major = GetSelectedMajorCountry();
            if (major == null)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            // 値に変化がなければ何もしない
            string val = control.Text;
            if (val.Equals((string) _controller.GetItemValue(itemId, major)))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, major);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, major);

            // 値を更新する
            _controller.SetItemValue(itemId, val, major);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, major);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, major);
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
            InitAllianceItems();
            InitWarItems();
        }

        /// <summary>
        ///     同盟タブの編集項目を更新する
        /// </summary>
        private void UpdateAllianceTab()
        {
            // 初期化済みであれば何もしない
            if (_tabPageInitialized[(int) TabPageNo.Alliance])
            {
                return;
            }

            // 同盟リストを更新する
            UpdateAllianceList();

            // 戦争リストを更新する
            UpdateWarList();

            // 同盟リストを有効化する
            EnableAllianceList();

            // 戦争リストを有効化する
            EnableWarList();

            // 初期化済みフラグをセットする
            _tabPageInitialized[(int) TabPageNo.Alliance] = true;
        }

        /// <summary>
        ///     同盟タブのフォーム読み込み時の処理
        /// </summary>
        private void OnAllianceTabPageFormLoad()
        {
            // 同盟タブを初期化する
            InitAllianceTab();
        }

        /// <summary>
        ///     同盟タブのファイル読み込み時の処理
        /// </summary>
        private void OnAllianceTabPageFileLoad()
        {
            // 同盟タブ選択中でなければ何もしない
            if (_tabPageNo != TabPageNo.Alliance)
            {
                return;
            }

            // 初回遷移時には表示を更新する
            UpdateAllianceTab();
        }

        /// <summary>
        ///     同盟タブ選択時の処理
        /// </summary>
        private void OnAllianceTabPageSelected()
        {
            // シナリオ未読み込みならば何もしない
            if (!Scenarios.IsLoaded())
            {
                return;
            }

            // 初回遷移時には表示を更新する
            UpdateAllianceTab();
        }

        #endregion

        #region 同盟タブ - 同盟

        /// <summary>
        ///     同盟の編集項目を初期化する
        /// </summary>
        private void InitAllianceItems()
        {
            _itemControls.Add(ScenarioEditorItemId.AllianceName, allianceNameTextBox);
            _itemControls.Add(ScenarioEditorItemId.AllianceType, allianceTypeTextBox);
            _itemControls.Add(ScenarioEditorItemId.AllianceId, allianceIdTextBox);

            allianceNameTextBox.Tag = ScenarioEditorItemId.AllianceName;
            allianceTypeTextBox.Tag = ScenarioEditorItemId.AllianceType;
            allianceIdTextBox.Tag = ScenarioEditorItemId.AllianceId;
        }

        /// <summary>
        ///     同盟の編集項目を更新する
        /// </summary>
        /// <param name="alliance">同盟</param>
        private void UpdateAllianceItems(Alliance alliance)
        {
            // 編集項目の値を更新する
            _controller.UpdateItemValue(allianceNameTextBox, alliance);
            _controller.UpdateItemValue(allianceTypeTextBox, alliance);
            _controller.UpdateItemValue(allianceIdTextBox, alliance);

            // 編集項目の色を更新する
            _controller.UpdateItemColor(allianceNameTextBox, alliance);
            _controller.UpdateItemColor(allianceTypeTextBox, alliance);
            _controller.UpdateItemColor(allianceIdTextBox, alliance);

            // 同盟参加国リストを更新する
            UpdateAllianceParticipant(alliance);
        }

        /// <summary>
        ///     同盟の編集項目をクリアする
        /// </summary>
        private void ClearAllianceItems()
        {
            // 編集項目をクリアする
            allianceNameTextBox.Text = "";
            allianceTypeTextBox.Text = "";
            allianceIdTextBox.Text = "";

            // 同盟参加国リストをクリアする
            ClearAllianceParticipant();
        }

        /// <summary>
        ///     同盟の編集項目を有効化する
        /// </summary>
        private void EnableAllianceItems()
        {
            int index = allianceListView.SelectedIndices[0];

            // 枢軸国/連合国/共産国以外は名前変更できない
            allianceNameLabel.Enabled = index < 3;
            allianceNameTextBox.Enabled = index < 3;
            allianceIdLabel.Enabled = true;
            allianceTypeTextBox.Enabled = true;
            allianceIdTextBox.Enabled = true;

            // 同盟参加国リストを有効化する
            EnableAllianceParticipant();
        }

        /// <summary>
        ///     同盟の編集項目を無効化する
        /// </summary>
        private void DisableAllianceItems()
        {
            allianceNameLabel.Enabled = false;
            allianceNameTextBox.Enabled = false;
            allianceIdLabel.Enabled = false;
            allianceTypeTextBox.Enabled = false;
            allianceIdTextBox.Enabled = false;

            // 同盟参加国リストを無効化する
            DisableAllianceParticipant();
        }

        #endregion

        #region 同盟タブ - 同盟リスト

        /// <summary>
        ///     同盟リストを更新する
        /// </summary>
        private void UpdateAllianceList()
        {
            ScenarioGlobalData data = Scenarios.Data.GlobalData;

            allianceListView.BeginUpdate();
            allianceListView.Items.Clear();

            // 枢軸国
            ListViewItem item = new ListViewItem
            {
                Text = (string) _controller.GetItemValue(ScenarioEditorItemId.AllianceName, data.Axis),
                Tag = data.Axis
            };
            item.SubItems.Add(data.Axis != null ? Countries.GetNameList(data.Axis.Participant) : "");
            allianceListView.Items.Add(item);

            // 連合国
            item = new ListViewItem
            {
                Text = (string) _controller.GetItemValue(ScenarioEditorItemId.AllianceName, data.Allies),
                Tag = data.Allies
            };
            item.SubItems.Add(data.Allies != null ? Countries.GetNameList(data.Allies.Participant) : "");
            allianceListView.Items.Add(item);

            // 共産国
            item = new ListViewItem
            {
                Text = (string) _controller.GetItemValue(ScenarioEditorItemId.AllianceName, data.Comintern),
                Tag = data.Comintern
            };
            item.SubItems.Add(data.Comintern != null ? Countries.GetNameList(data.Comintern.Participant) : "");
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

            // 同盟操作ボタンを無効化する
            DisableAllianceItemButtons();

            // 同盟参加国操作ボタンを無効化する
            DisableAllianceParticipantButtons();

            // 編集項目を無効化する
            DisableAllianceItems();

            // 編集項目をクリアする
            ClearAllianceItems();
        }

        /// <summary>
        ///     同盟リストを有効化する
        /// </summary>
        private void EnableAllianceList()
        {
            allianceGroupBox.Enabled = true;
        }

        /// <summary>
        ///     同盟操作ボタンを有効化する
        /// </summary>
        private void EnableAllianceItemButtons()
        {
            int count = allianceListView.Items.Count;
            int index = allianceListView.SelectedIndices[0];

            // 枢軸国/連合国/共産国は順番変更/削除できない
            allianceUpButton.Enabled = index > 3;
            allianceDownButton.Enabled = (index < count - 1) && (index >= 3);
            allianceRemoveButton.Enabled = index >= 3;
        }

        /// <summary>
        ///     同盟操作ボタンを無効化する
        /// </summary>
        private void DisableAllianceItemButtons()
        {
            allianceUpButton.Enabled = false;
            allianceDownButton.Enabled = false;
            allianceRemoveButton.Enabled = false;
        }

        /// <summary>
        ///     同盟リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 同盟参加国操作ボタンを無効化する
            DisableAllianceParticipantButtons();

            // 選択項目がなければ編集項目を無効化する
            if (allianceListView.SelectedItems.Count == 0)
            {
                // 同盟操作ボタンを無効化する
                DisableAllianceItemButtons();

                // 編集項目を無効化する
                DisableAllianceItems();

                // 編集項目をクリアする
                ClearAllianceItems();
                return;
            }

            Alliance alliance = GetSelectedAlliance();

            // 編集項目を更新する
            UpdateAllianceItems(alliance);

            // 編集項目を有効化する
            EnableAllianceItems();

            // 同盟操作ボタンを有効化する
            EnableAllianceItemButtons();
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

            Log.Info("[Scenario] alliance added ({0})", allianceListView.Items.Count - 4);

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

            Log.Info("[Scenario] alliance removed ({0})", index);

            // 編集済みフラグを設定する
            Scenarios.SetDirty();

            // 削除した項目の次を選択する
            index += index < alliances.Count ? 3 : 3 - 1;
            allianceListView.Items[index].Focused = true;
            allianceListView.Items[index].Selected = true;
        }


        /// <summary>
        ///     同盟リストビューの項目文字列を設定する
        /// </summary>
        /// <param name="no">項目番号</param>
        /// <param name="s">文字列</param>
        public void SetAllianceListItemText(int no, string s)
        {
            allianceListView.SelectedItems[0].SubItems[no].Text = s;
        }

        /// <summary>
        ///     選択中の同盟情報を取得する
        /// </summary>
        /// <returns>選択中の同盟情報</returns>
        private Alliance GetSelectedAlliance()
        {
            return allianceListView.SelectedItems.Count > 0 ? allianceListView.SelectedItems[0].Tag as Alliance : null;
        }

        #endregion

        #region 同盟タブ - 同盟参加国

        /// <summary>
        ///     同盟参加国リストを更新する
        /// </summary>
        /// <param name="alliance">同盟</param>
        private void UpdateAllianceParticipant(Alliance alliance)
        {
            // 同盟参加国
            allianceParticipantListBox.BeginUpdate();
            allianceParticipantListBox.Items.Clear();
            foreach (Country country in alliance.Participant)
            {
                allianceParticipantListBox.Items.Add(Countries.GetTagName(country));
            }
            allianceParticipantListBox.EndUpdate();

            // 同盟非参加国
            _allianceFreeCountries = Countries.Tags.Where(country => !alliance.Participant.Contains(country)).ToList();
            allianceFreeCountryListBox.BeginUpdate();
            allianceFreeCountryListBox.Items.Clear();
            foreach (Country country in _allianceFreeCountries)
            {
                allianceFreeCountryListBox.Items.Add(Countries.GetTagName(country));
            }
            allianceFreeCountryListBox.EndUpdate();
        }

        /// <summary>
        ///     同盟参加国リストをクリアする
        /// </summary>
        private void ClearAllianceParticipant()
        {
            allianceParticipantListBox.Items.Clear();
            allianceFreeCountryListBox.Items.Clear();
        }

        /// <summary>
        ///     同盟参加国リストを有効化する
        /// </summary>
        private void EnableAllianceParticipant()
        {
            allianceParticipantLabel.Enabled = true;
            allianceParticipantListBox.Enabled = true;
            allianceFreeCountryListBox.Enabled = true;
        }

        /// <summary>
        ///     同盟参加国リストを無効化する
        /// </summary>
        private void DisableAllianceParticipant()
        {
            allianceParticipantLabel.Enabled = false;
            allianceParticipantListBox.Enabled = false;
            allianceFreeCountryListBox.Enabled = false;
        }

        /// <summary>
        ///     同盟参加国操作ボタンを無効化する
        /// </summary>
        private void DisableAllianceParticipantButtons()
        {
            allianceParticipantAddButton.Enabled = false;
            allianceParticipantRemoveButton.Enabled = false;
            allianceLeaderButton.Enabled = false;
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

            ListBox control = sender as ListBox;
            if (control == null)
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
                brush = new SolidBrush(dirty ? Color.Red : control.ForeColor);
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

            ListBox control = sender as ListBox;
            if (control == null)
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
                brush = new SolidBrush(dirty ? Color.Red : control.ForeColor);
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

        /// <summary>
        ///     同盟参加国リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceParticipantListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            int count = allianceParticipantListBox.SelectedIndices.Count;
            int index = allianceParticipantListBox.SelectedIndex;

            allianceParticipantRemoveButton.Enabled = count > 0;
            allianceLeaderButton.Enabled = (count == 1) && (index > 0);
        }

        /// <summary>
        ///     同盟非参加国リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceCountryListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            int count = allianceFreeCountryListBox.SelectedIndices.Count;

            allianceParticipantAddButton.Enabled = count > 0;
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
                (from int index in allianceFreeCountryListBox.SelectedIndices select _allianceFreeCountries[index])
                    .ToList();
            allianceParticipantListBox.BeginUpdate();
            allianceFreeCountryListBox.BeginUpdate();
            foreach (Country country in countries)
            {
                // 同盟参加国リストボックスに追加する
                allianceParticipantListBox.Items.Add(Countries.GetTagName(country));

                // 同盟参加国リストに追加する
                alliance.Participant.Add(country);

                // 同盟非参加国リストボックスから削除する
                int index = _allianceFreeCountries.IndexOf(country);
                allianceFreeCountryListBox.Items.RemoveAt(index);
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
            allianceFreeCountryListBox.EndUpdate();
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
            allianceFreeCountryListBox.BeginUpdate();
            foreach (Country country in countries)
            {
                // 同盟非参加国リストボックスに追加する
                int index = _allianceFreeCountries.FindIndex(c => c > country);
                if (index < 0)
                {
                    index = _allianceFreeCountries.Count;
                }
                allianceFreeCountryListBox.Items.Insert(index, Countries.GetTagName(country));
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
            allianceFreeCountryListBox.EndUpdate();
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

        #endregion

        #region 同盟タブ - 戦争

        /// <summary>
        ///     戦争の編集項目を初期化する
        /// </summary>
        private void InitWarItems()
        {
            _itemControls.Add(ScenarioEditorItemId.WarStartYear, warStartYearTextBox);
            _itemControls.Add(ScenarioEditorItemId.WarStartMonth, warStartMonthTextBox);
            _itemControls.Add(ScenarioEditorItemId.WarStartDay, warStartDayTextBox);
            _itemControls.Add(ScenarioEditorItemId.WarEndYear, warEndYearTextBox);
            _itemControls.Add(ScenarioEditorItemId.WarEndMonth, warEndMonthTextBox);
            _itemControls.Add(ScenarioEditorItemId.WarEndDay, warEndDayTextBox);
            _itemControls.Add(ScenarioEditorItemId.WarType, warTypeTextBox);
            _itemControls.Add(ScenarioEditorItemId.WarId, warIdTextBox);
            _itemControls.Add(ScenarioEditorItemId.WarAttackerType, warAttackerTypeTextBox);
            _itemControls.Add(ScenarioEditorItemId.WarAttackerId, warAttackerIdTextBox);
            _itemControls.Add(ScenarioEditorItemId.WarDefenderType, warDefenderTypeTextBox);
            _itemControls.Add(ScenarioEditorItemId.WarDefenderId, warDefenderIdTextBox);

            warStartYearTextBox.Tag = ScenarioEditorItemId.WarStartYear;
            warStartMonthTextBox.Tag = ScenarioEditorItemId.WarStartMonth;
            warStartDayTextBox.Tag = ScenarioEditorItemId.WarStartDay;
            warEndYearTextBox.Tag = ScenarioEditorItemId.WarEndYear;
            warEndMonthTextBox.Tag = ScenarioEditorItemId.WarEndMonth;
            warEndDayTextBox.Tag = ScenarioEditorItemId.WarEndDay;
            warTypeTextBox.Tag = ScenarioEditorItemId.WarType;
            warIdTextBox.Tag = ScenarioEditorItemId.WarId;
            warAttackerTypeTextBox.Tag = ScenarioEditorItemId.WarAttackerType;
            warAttackerIdTextBox.Tag = ScenarioEditorItemId.WarAttackerId;
            warDefenderTypeTextBox.Tag = ScenarioEditorItemId.WarDefenderType;
            warDefenderIdTextBox.Tag = ScenarioEditorItemId.WarDefenderId;
        }

        /// <summary>
        ///     戦争の編集項目を更新する
        /// </summary>
        /// <param name="war">戦争</param>
        private void UpdateWarItems(War war)
        {
            // 編集項目の値を更新する
            _controller.UpdateItemValue(warStartYearTextBox, war);
            _controller.UpdateItemValue(warStartMonthTextBox, war);
            _controller.UpdateItemValue(warStartDayTextBox, war);
            _controller.UpdateItemValue(warEndYearTextBox, war);
            _controller.UpdateItemValue(warEndMonthTextBox, war);
            _controller.UpdateItemValue(warEndDayTextBox, war);
            _controller.UpdateItemValue(warTypeTextBox, war);
            _controller.UpdateItemValue(warIdTextBox, war);
            _controller.UpdateItemValue(warAttackerTypeTextBox, war);
            _controller.UpdateItemValue(warAttackerIdTextBox, war);
            _controller.UpdateItemValue(warDefenderTypeTextBox, war);
            _controller.UpdateItemValue(warDefenderIdTextBox, war);

            // 編集項目の色を更新する
            _controller.UpdateItemColor(warStartYearTextBox, war);
            _controller.UpdateItemColor(warStartMonthTextBox, war);
            _controller.UpdateItemColor(warStartDayTextBox, war);
            _controller.UpdateItemColor(warEndYearTextBox, war);
            _controller.UpdateItemColor(warEndMonthTextBox, war);
            _controller.UpdateItemColor(warEndDayTextBox, war);
            _controller.UpdateItemColor(warTypeTextBox, war);
            _controller.UpdateItemColor(warIdTextBox, war);
            _controller.UpdateItemColor(warAttackerTypeTextBox, war);
            _controller.UpdateItemColor(warAttackerIdTextBox, war);
            _controller.UpdateItemColor(warDefenderTypeTextBox, war);
            _controller.UpdateItemColor(warDefenderIdTextBox, war);

            // 戦争参加国リストを更新する
            UpdateWarParticipant(war);
        }

        /// <summary>
        ///     戦争の編集項目をクリアする
        /// </summary>
        private void ClearWarItems()
        {
            // 編集項目をクリアする
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

            // 戦争参加国リストをクリアする
            ClearWarParticipant();
        }

        /// <summary>
        ///     戦争の編集項目を有効化する
        /// </summary>
        private void EnableWarItems()
        {
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
            warAttackerIdLabel.Enabled = true;
            warAttackerTypeTextBox.Enabled = true;
            warAttackerIdTextBox.Enabled = true;
            warDefenderIdLabel.Enabled = true;
            warDefenderTypeTextBox.Enabled = true;
            warDefenderIdTextBox.Enabled = true;

            // 戦争参加国リストを有効化する
            EnableWarParticipant();
        }

        /// <summary>
        ///     戦争の編集項目を無効化する
        /// </summary>
        private void DisableWarItems()
        {
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
            warAttackerIdLabel.Enabled = false;
            warAttackerTypeTextBox.Enabled = false;
            warAttackerIdTextBox.Enabled = false;
            warDefenderIdLabel.Enabled = false;
            warDefenderTypeTextBox.Enabled = false;
            warDefenderIdTextBox.Enabled = false;

            // 戦争参加国リストを無効化する
            DisableWarParticipant();
        }

        #endregion

        #region 同盟タブ - 戦争リスト

        /// <summary>
        ///     戦争リストビューを更新する
        /// </summary>
        private void UpdateWarList()
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

            // 戦争操作ボタンを無効化する
            DisableWarItemButtons();

            // 戦争参加国操作ボタンを無効化する
            DisableWarParticipantButtons();

            // 編集項目を無効化する
            DisableWarItems();

            // 編集項目をクリアする
            ClearWarItems();
        }

        /// <summary>
        ///     戦争リストを有効化する
        /// </summary>
        private void EnableWarList()
        {
            warGroupBox.Enabled = true;
        }

        /// <summary>
        ///     戦争操作ボタンを有効化する
        /// </summary>
        private void EnableWarItemButtons()
        {
            int count = warListView.Items.Count;
            int index = warListView.SelectedIndices[0];

            warUpButton.Enabled = index > 0;
            warDownButton.Enabled = index < count - 1;
            warRemoveButton.Enabled = true;
        }

        /// <summary>
        ///     戦争操作ボタンを有効化する
        /// </summary>
        private void DisableWarItemButtons()
        {
            warUpButton.Enabled = false;
            warDownButton.Enabled = false;
            warRemoveButton.Enabled = false;
        }

        /// <summary>
        ///     戦争リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 戦争参加国操作ボタンを無効化する
            DisableWarParticipantButtons();

            // 選択項目がなければ編集項目を無効化する
            if (warListView.SelectedItems.Count == 0)
            {
                // 戦争操作ボタンを無効化する
                DisableWarItemButtons();

                // 編集項目を無効化する
                DisableWarItems();

                // 編集項目をクリアする
                ClearWarItems();
                return;
            }

            War war = GetSelectedWar();

            // 編集項目を更新する
            UpdateWarItems(war);

            // 編集項目を有効化する
            EnableWarItems();

            // 戦争操作ボタンを有効化する
            EnableWarItemButtons();
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

            Log.Info("[Scenario] war added ({0})", warListView.Items.Count - 1);

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

            Log.Info("[Scenario] war removed ({0})", index);

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
            if (index >= 0)
            {
                warListView.Items[index].Focused = true;
                warListView.Items[index].Selected = true;
            }
        }

        /// <summary>
        ///     選択中の戦争情報を取得する
        /// </summary>
        /// <returns>選択中の戦争情報</returns>
        private War GetSelectedWar()
        {
            return warListView.SelectedItems.Count > 0 ? warListView.SelectedItems[0].Tag as War : null;
        }

        #endregion

        #region 同盟タブ - 戦争参加国

        /// <summary>
        ///     戦争参加国リストを更新する
        /// </summary>
        /// <param name="war"></param>
        private void UpdateWarParticipant(War war)
        {
            IEnumerable<Country> countries = Countries.Tags;

            // 攻撃側参加国
            warAttackerListBox.BeginUpdate();
            warAttackerListBox.Items.Clear();
            if (war.Attackers?.Participant != null)
            {
                foreach (Country country in war.Attackers.Participant)
                {
                    warAttackerListBox.Items.Add(Countries.GetTagName(country));
                }
                countries = countries.Where(country => !war.Attackers.Participant.Contains(country));
            }
            warAttackerListBox.EndUpdate();

            // 防御側参加国
            warDefenderListBox.BeginUpdate();
            warDefenderListBox.Items.Clear();
            if (war.Defenders?.Participant != null)
            {
                foreach (Country country in war.Defenders.Participant)
                {
                    warDefenderListBox.Items.Add(Countries.GetTagName(country));
                }
                countries = countries.Where(country => !war.Defenders.Participant.Contains(country));
            }
            warDefenderListBox.EndUpdate();

            // 戦争非参加国
            _warFreeCountries = countries.ToList();
            warFreeCountryListBox.BeginUpdate();
            warFreeCountryListBox.Items.Clear();
            foreach (Country country in _warFreeCountries)
            {
                warFreeCountryListBox.Items.Add(Countries.GetTagName(country));
            }
            warFreeCountryListBox.EndUpdate();
        }

        /// <summary>
        ///     戦争参加国リストをクリアする
        /// </summary>
        private void ClearWarParticipant()
        {
            warAttackerListBox.Items.Clear();
            warDefenderListBox.Items.Clear();
            warFreeCountryListBox.Items.Clear();
        }

        /// <summary>
        ///     戦争参加国リストを有効化する
        /// </summary>
        private void EnableWarParticipant()
        {
            warAttackerLabel.Enabled = true;
            warAttackerListBox.Enabled = true;
            warDefenderLabel.Enabled = true;
            warDefenderListBox.Enabled = true;
            warFreeCountryListBox.Enabled = true;
        }

        /// <summary>
        ///     戦争参加国リストを無効化する
        /// </summary>
        private void DisableWarParticipant()
        {
            warAttackerLabel.Enabled = false;
            warAttackerListBox.Enabled = false;
            warDefenderLabel.Enabled = false;
            warDefenderListBox.Enabled = false;
            warFreeCountryListBox.Enabled = false;
        }

        /// <summary>
        ///     戦争参加国操作ボタンを無効化する
        /// </summary>
        private void DisableWarParticipantButtons()
        {
            warAttackerAddButton.Enabled = false;
            warAttackerRemoveButton.Enabled = false;
            warAttackerLeaderButton.Enabled = false;
            warDefenderAddButton.Enabled = false;
            warDefenderRemoveButton.Enabled = false;
            warDefenderLeaderButton.Enabled = false;
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

            ListBox control = sender as ListBox;
            if (control == null)
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
                brush = new SolidBrush(dirty ? Color.Red : control.ForeColor);
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
            ListBox control = sender as ListBox;
            if (control == null)
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
                brush = new SolidBrush(dirty ? Color.Red : control.ForeColor);
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

            ListBox control = sender as ListBox;
            if (control == null)
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
                brush = new SolidBrush(dirty ? Color.Red : control.ForeColor);
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

        /// <summary>
        ///     戦争攻撃側参加国リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarAttackerListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            int count = warAttackerListBox.SelectedIndices.Count;
            int index = warAttackerListBox.SelectedIndex;

            warAttackerRemoveButton.Enabled = count > 0;
            warAttackerLeaderButton.Enabled = (count == 1) && (index > 0);
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

            warDefenderRemoveButton.Enabled = count > 0;
            warDefenderLeaderButton.Enabled = (count == 1) && (index > 0);
        }

        /// <summary>
        ///     戦争非参加国リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarCountryListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            int count = warFreeCountryListBox.SelectedIndices.Count;

            warAttackerAddButton.Enabled = count > 0;
            warDefenderAddButton.Enabled = count > 0;
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
                (from int index in warFreeCountryListBox.SelectedIndices select _warFreeCountries[index]).ToList();
            warAttackerListBox.BeginUpdate();
            warFreeCountryListBox.BeginUpdate();
            foreach (Country country in countries)
            {
                // 戦争攻撃側参加国リストボックスに追加する
                warAttackerListBox.Items.Add(Countries.GetTagName(country));

                // 戦争攻撃側参加国リストに追加する
                war.Attackers.Participant.Add(country);

                // 戦争非参加国リストボックスから削除する
                int index = _warFreeCountries.IndexOf(country);
                warFreeCountryListBox.Items.RemoveAt(index);
                _warFreeCountries.RemoveAt(index);

                // 編集済みフラグを設定する
                war.SetDirtyCountry(country);
                Scenarios.SetDirty();

                // 戦争リストビューの項目を更新する
                warListView.SelectedItems[0].SubItems[0].Text = Countries.GetNameList(war.Attackers.Participant);

                Log.Info("[Scenario] war attacker: +{0} ({1})", Countries.Strings[(int) country],
                    warListView.SelectedIndices[0]);
            }
            warAttackerListBox.EndUpdate();
            warFreeCountryListBox.EndUpdate();
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
            warFreeCountryListBox.BeginUpdate();
            foreach (Country country in countries)
            {
                // 戦争非参加国リストボックスに追加する
                int index = _warFreeCountries.FindIndex(c => c > country);
                if (index < 0)
                {
                    index = _warFreeCountries.Count;
                }
                warFreeCountryListBox.Items.Insert(index, Countries.GetTagName(country));
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
                warListView.SelectedItems[0].SubItems[0].Text = Countries.GetNameList(war.Attackers.Participant);

                Log.Info("[Scenario] war attacker: -{0} ({1})", Countries.Strings[(int) country],
                    warListView.SelectedIndices[0]);
            }
            warAttackerListBox.EndUpdate();
            warFreeCountryListBox.EndUpdate();
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
                (from int index in warFreeCountryListBox.SelectedIndices select _warFreeCountries[index]).ToList();
            warDefenderListBox.BeginUpdate();
            warFreeCountryListBox.BeginUpdate();
            foreach (Country country in countries)
            {
                // 戦争防御側参加国リストボックスに追加する
                warDefenderListBox.Items.Add(Countries.GetTagName(country));

                // 戦争防御側参加国リストに追加する
                war.Defenders.Participant.Add(country);

                // 戦争非参加国リストボックスから削除する
                int index = _warFreeCountries.IndexOf(country);
                warFreeCountryListBox.Items.RemoveAt(index);
                _warFreeCountries.RemoveAt(index);

                // 編集済みフラグを設定する
                war.SetDirtyCountry(country);
                Scenarios.SetDirty();

                // 戦争リストビューの項目を更新する
                warListView.SelectedItems[0].SubItems[1].Text = Countries.GetNameList(war.Defenders.Participant);

                Log.Info("[Scenario] war defender: +{0} ({1})", Countries.Strings[(int) country],
                    warListView.SelectedIndices[0]);
            }
            warDefenderListBox.EndUpdate();
            warFreeCountryListBox.EndUpdate();
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
            warFreeCountryListBox.BeginUpdate();
            foreach (Country country in countries)
            {
                // 戦争非参加国リストボックスに追加する
                int index = _warFreeCountries.FindIndex(c => c > country);
                if (index < 0)
                {
                    index = _warFreeCountries.Count;
                }
                warFreeCountryListBox.Items.Insert(index, Countries.GetTagName(country));
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
                warListView.SelectedItems[0].SubItems[1].Text = Countries.GetNameList(war.Defenders.Participant);

                Log.Info("[Scenario] war defender: -{0} ({1})", Countries.Strings[(int) country],
                    warListView.SelectedIndices[0]);
            }
            warDefenderListBox.EndUpdate();
            warFreeCountryListBox.EndUpdate();
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
            warListView.SelectedItems[0].SubItems[0].Text = Countries.GetNameList(war.Attackers.Participant);

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
            warListView.SelectedItems[0].SubItems[1].Text = Countries.GetNameList(war.Defenders.Participant);

            Log.Info("[Scenario] war defender leader: {0} ({1})", Countries.Strings[(int) country],
                warListView.SelectedIndices[0]);
        }

        #endregion

        #region 同盟タブ - 編集項目

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceIntItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Alliance alliance = GetSelectedAlliance();
            if (alliance == null)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            // 文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, alliance);
                return;
            }

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, alliance);
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
            if (!_controller.IsItemValueValid(itemId, val))
            {
                _controller.UpdateItemValue(control, alliance);
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, alliance, allianceListView.SelectedIndices[0]);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, alliance);

            // 値を更新する
            _controller.SetItemValue(itemId, val, alliance);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, alliance);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, alliance);
        }

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarIntItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            // 文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, war);
                return;
            }

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, war);
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
            if (!_controller.IsItemValueValid(itemId, val))
            {
                _controller.UpdateItemValue(control, war);
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, war, warListView.SelectedIndices[0]);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, war);

            // 値を更新する
            _controller.SetItemValue(itemId, val, war);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, war);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, war);
        }

        /// <summary>
        ///     テキストボックスの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceStringItemTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Alliance alliance = GetSelectedAlliance();
            if (alliance == null)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            // 値に変化がなければ何もしない
            string val = control.Text;
            if (val.Equals((string) _controller.GetItemValue(itemId, alliance)))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, alliance, allianceListView.SelectedIndices[0]);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, alliance);

            // 値を更新する
            _controller.SetItemValue(itemId, val, alliance);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, alliance);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, alliance);
        }

        #endregion

        #endregion

        #region 関係タブ

        #region 関係タブ - 共通

        /// <summary>
        ///     関係タブを初期化する
        /// </summary>
        private void InitRelationTab()
        {
            InitRelationItems();
            InitGuaranteedItems();
            InitNonAggressionItems();
            InitPeaceItems();
            InitIntelligenceItems();
        }

        /// <summary>
        ///     関係タブを更新する
        /// </summary>
        private void UpdateRelationTab()
        {
            // 初期化済みであれば何もしない
            if (_tabPageInitialized[(int) TabPageNo.Relation])
            {
                return;
            }

            // 選択国リストを更新する
            UpdateCountryListBox(relationCountryListBox);

            // 選択国リストを有効化する
            EnableRelationCountryList();

            // 国家関係リストをクリアする
            ClearRelationList();

            // 編集項目を無効化する
            DisableRelationItems();
            DisableGuaranteedItems();
            DisableNonAggressionItems();
            DisablePeaceItems();
            DisableIntelligenceItems();

            // 編集項目をクリアする
            ClearRelationItems();
            ClearGuaranteedItems();
            ClearNonAggressionItems();
            ClearPeaceItems();
            ClearIntelligenceItems();

            // 国家関係リストを有効化する
            EnableRelationList();

            // 初期化済みフラグをセットする
            _tabPageInitialized[(int) TabPageNo.Relation] = true;
        }

        /// <summary>
        ///     関係タブのフォーム読み込み時の処理
        /// </summary>
        private void OnRelationTabPageFormLoad()
        {
            // 関係タブを初期化する
            InitRelationTab();
        }

        /// <summary>
        ///     関係タブのファイル読み込み時の処理
        /// </summary>
        private void OnRelationTabPageFileLoad()
        {
            // 関係タブ選択中でなければ何もしない
            if (_tabPageNo != TabPageNo.Relation)
            {
                return;
            }

            // 初回遷移時には表示を更新する
            UpdateRelationTab();
        }

        /// <summary>
        ///     関係タブ選択時の処理
        /// </summary>
        private void OnRelationTabPageSelected()
        {
            // シナリオ未読み込みならば何もしない
            if (!Scenarios.IsLoaded())
            {
                return;
            }

            // 初回遷移時には表示を更新する
            UpdateRelationTab();
        }

        #endregion

        #region 国家リスト

        /// <summary>
        ///     国家リストボックスを有効化する
        /// </summary>
        private void EnableRelationCountryList()
        {
            relationCountryListBox.Enabled = true;
        }

        /// <summary>
        ///     国家リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRelationCountryListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ国家関係リストをクリアする
            if (relationCountryListBox.SelectedIndex < 0)
            {
                // 国家関係リストをクリアする
                ClearRelationList();
                return;
            }

            // 国家関係リストを更新する
            UpdateRelationList();
        }

        /// <summary>
        ///     国家関係の選択国を取得する
        /// </summary>
        /// <returns>国家関係の選択国</returns>
        private Country GetSelectedRelationCountry()
        {
            return relationCountryListBox.SelectedIndex >= 0
                ? Countries.Tags[relationCountryListBox.SelectedIndex]
                : Country.None;
        }

        #endregion

        #region 関係タブ - 国家関係リスト

        /// <summary>
        ///     国家関係リストを更新する
        /// </summary>
        private void UpdateRelationList()
        {
            Country selected = GetSelectedRelationCountry();
            CountrySettings settings = Scenarios.GetCountrySettings(selected);

            relationListView.BeginUpdate();
            relationListView.Items.Clear();
            foreach (Country target in Countries.Tags)
            {
                relationListView.Items.Add(CreateRelationListItem(selected, target, settings));
            }
            relationListView.EndUpdate();
        }

        /// <summary>
        ///     国家関係リストをクリアする
        /// </summary>
        private void ClearRelationList()
        {
            relationListView.BeginUpdate();
            relationListView.Items.Clear();
            relationListView.EndUpdate();
        }

        /// <summary>
        ///     国家関係リストを有効化する
        /// </summary>
        private void EnableRelationList()
        {
            relationListView.Enabled = true;
        }

        /// <summary>
        ///     国家関係リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRelationListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ編集項目を無効化する
            if (relationListView.SelectedIndices.Count == 0)
            {
                // 編集項目を無効化する
                DisableRelationItems();
                DisableGuaranteedItems();
                DisableNonAggressionItems();
                DisablePeaceItems();
                DisableIntelligenceItems();

                // 編集項目をクリアする
                ClearRelationItems();
                ClearGuaranteedItems();
                ClearNonAggressionItems();
                ClearPeaceItems();
                ClearIntelligenceItems();
                return;
            }

            Country selected = GetSelectedRelationCountry();
            Country target = GetTargetRelationCountry();
            CountrySettings settings = Scenarios.GetCountrySettings(selected);
            Relation relation = Scenarios.GetCountryRelation(selected, target);
            Treaty nonAggression = Scenarios.GetNonAggression(selected, target);
            Treaty peace = Scenarios.GetPeace(selected, target);
            SpySettings spy = Scenarios.GetCountryIntelligence(selected, target);

            // 編集項目を更新する
            UpdateRelationItems(relation, target, settings);
            UpdateGuaranteedItems(relation);
            UpdateNonAggressionItems(nonAggression);
            UpdatePeaceItems(peace);
            UpdateIntelligenceItems(spy);

            // 編集項目を有効化する
            EnableRelationItems();
            EnableGuaranteedItems();
            EnableNonAggressionItems();
            EnablePeaceItems();
            EnableIntelligenceItems();
        }

        /// <summary>
        ///     貿易リストビューの項目文字列を設定する
        /// </summary>
        /// <param name="index">項目のインデックス</param>
        /// <param name="no">項目番号</param>
        /// <param name="s">文字列</param>
        public void SetRelationListItemText(int index, int no, string s)
        {
            relationListView.Items[index].SubItems[no].Text = s;
        }

        /// <summary>
        ///     貿易リストビューの選択項目の文字列を設定する
        /// </summary>
        /// <param name="no">項目番号</param>
        /// <param name="s">文字列</param>
        public void SetRelationListItemText(int no, string s)
        {
            relationListView.SelectedItems[0].SubItems[no].Text = s;
        }

        /// <summary>
        ///     国家関係リストビューの項目を作成する
        /// </summary>
        /// <param name="selected">選択国</param>
        /// <param name="target">対象国</param>
        /// <param name="settings">国家設定</param>
        /// <returns>国家関係リストビューの項目</returns>
        private ListViewItem CreateRelationListItem(Country selected, Country target, CountrySettings settings)
        {
            ListViewItem item = new ListViewItem(Countries.GetTagName(target));
            Relation relation = Scenarios.GetCountryRelation(selected, target);
            Treaty nonAggression = Scenarios.GetNonAggression(selected, target);
            Treaty peace = Scenarios.GetPeace(selected, target);
            SpySettings spy = Scenarios.GetCountryIntelligence(selected, target);

            item.SubItems.Add(
                ObjectHelper.ToString(_controller.GetItemValue(ScenarioEditorItemId.DiplomacyRelationValue, relation)));
            item.SubItems.Add(
                (Country) _controller.GetItemValue(ScenarioEditorItemId.DiplomacyMaster, settings) == target
                    ? Resources.Yes
                    : "");
            item.SubItems.Add(
                (Country) _controller.GetItemValue(ScenarioEditorItemId.DiplomacyMilitaryControl, settings) == target
                    ? Resources.Yes
                    : "");
            item.SubItems.Add(
                (bool) _controller.GetItemValue(ScenarioEditorItemId.DiplomacyMilitaryAccess, relation)
                    ? Resources.Yes
                    : "");
            item.SubItems.Add(
                (bool) _controller.GetItemValue(ScenarioEditorItemId.DiplomacyGuaranteed, relation) ? Resources.Yes : "");
            item.SubItems.Add(
                (bool) _controller.GetItemValue(ScenarioEditorItemId.DiplomacyNonAggression, nonAggression)
                    ? Resources.Yes
                    : "");
            item.SubItems.Add(
                (bool) _controller.GetItemValue(ScenarioEditorItemId.DiplomacyPeace, peace) ? Resources.Yes : "");
            item.SubItems.Add(
                ObjectHelper.ToString(_controller.GetItemValue(ScenarioEditorItemId.IntelligenceSpies, spy)));

            return item;
        }

        /// <summary>
        ///     国家関係の対象国を取得する
        /// </summary>
        /// <returns>国家関係の対象国</returns>
        private Country GetTargetRelationCountry()
        {
            return relationListView.SelectedItems.Count > 0
                ? Countries.Tags[relationListView.SelectedIndices[0]]
                : Country.None;
        }

        #endregion

        #region 関係タブ - 国家関係

        /// <summary>
        ///     国家関係の編集項目を初期化する
        /// </summary>
        private void InitRelationItems()
        {
            _itemControls.Add(ScenarioEditorItemId.DiplomacyRelationValue, relationValueTextBox);
            _itemControls.Add(ScenarioEditorItemId.DiplomacyMaster, masterCheckBox);
            _itemControls.Add(ScenarioEditorItemId.DiplomacyMilitaryControl, controlCheckBox);
            _itemControls.Add(ScenarioEditorItemId.DiplomacyMilitaryAccess, accessCheckBox);

            relationValueTextBox.Tag = ScenarioEditorItemId.DiplomacyRelationValue;
            masterCheckBox.Tag = ScenarioEditorItemId.DiplomacyMaster;
            controlCheckBox.Tag = ScenarioEditorItemId.DiplomacyMilitaryControl;
            accessCheckBox.Tag = ScenarioEditorItemId.DiplomacyMilitaryAccess;
        }

        /// <summary>
        ///     国家関係の編集項目を更新する
        /// </summary>
        /// <param name="relation">国家関係</param>
        /// <param name="target">相手国</param>
        /// <param name="settings">国家設定</param>
        private void UpdateRelationItems(Relation relation, Country target, CountrySettings settings)
        {
            _controller.UpdateItemValue(relationValueTextBox, relation);
            _controller.UpdateItemValue(masterCheckBox, target, settings);
            _controller.UpdateItemValue(controlCheckBox, target, settings);
            _controller.UpdateItemValue(accessCheckBox, relation);

            _controller.UpdateItemColor(relationValueTextBox, relation);
            _controller.UpdateItemColor(masterCheckBox, settings);
            _controller.UpdateItemColor(controlCheckBox, settings);
            _controller.UpdateItemColor(accessCheckBox, relation);
        }

        /// <summary>
        ///     国家関係の編集項目をクリアする
        /// </summary>
        private void ClearRelationItems()
        {
            relationValueTextBox.Text = "";
            masterCheckBox.Checked = false;
            controlCheckBox.Checked = false;
            accessCheckBox.Checked = false;
        }

        /// <summary>
        ///     国家関係の編集項目を有効化する
        /// </summary>
        private void EnableRelationItems()
        {
            relationGroupBox.Enabled = true;
        }

        /// <summary>
        ///     国家関係の編集項目を無効化する
        /// </summary>
        private void DisableRelationItems()
        {
            relationGroupBox.Enabled = false;
        }

        #endregion

        #region 関係タブ - 独立保障

        /// <summary>
        ///     独立保障の編集項目を初期化する
        /// </summary>
        private void InitGuaranteedItems()
        {
            _itemControls.Add(ScenarioEditorItemId.DiplomacyGuaranteed, guaranteedCheckBox);
            _itemControls.Add(ScenarioEditorItemId.DiplomacyGuaranteedEndYear, guaranteedYearTextBox);
            _itemControls.Add(ScenarioEditorItemId.DiplomacyGuaranteedEndMonth, guaranteedMonthTextBox);
            _itemControls.Add(ScenarioEditorItemId.DiplomacyGuaranteedEndDay, guaranteedDayTextBox);

            guaranteedCheckBox.Tag = ScenarioEditorItemId.DiplomacyGuaranteed;
            guaranteedYearTextBox.Tag = ScenarioEditorItemId.DiplomacyGuaranteedEndYear;
            guaranteedMonthTextBox.Tag = ScenarioEditorItemId.DiplomacyGuaranteedEndMonth;
            guaranteedDayTextBox.Tag = ScenarioEditorItemId.DiplomacyGuaranteedEndDay;
        }

        /// <summary>
        ///     独立保障の編集項目を更新する
        /// </summary>
        /// <param name="relation">国家関係</param>
        private void UpdateGuaranteedItems(Relation relation)
        {
            _controller.UpdateItemValue(guaranteedCheckBox, relation);
            _controller.UpdateItemValue(guaranteedYearTextBox, relation);
            _controller.UpdateItemValue(guaranteedMonthTextBox, relation);
            _controller.UpdateItemValue(guaranteedDayTextBox, relation);

            _controller.UpdateItemColor(guaranteedCheckBox, relation);
            _controller.UpdateItemColor(guaranteedYearTextBox, relation);
            _controller.UpdateItemColor(guaranteedMonthTextBox, relation);
            _controller.UpdateItemColor(guaranteedDayTextBox, relation);

            bool flag = (bool) _controller.GetItemValue(ScenarioEditorItemId.DiplomacyGuaranteed, relation);
            guaranteedYearTextBox.Enabled = flag;
            guaranteedMonthTextBox.Enabled = flag;
            guaranteedDayTextBox.Enabled = flag;
        }

        /// <summary>
        ///     独立保障の編集項目をクリアする
        /// </summary>
        private void ClearGuaranteedItems()
        {
            guaranteedCheckBox.Checked = false;
            guaranteedYearTextBox.Text = "";
            guaranteedMonthTextBox.Text = "";
            guaranteedDayTextBox.Text = "";

            guaranteedYearTextBox.Enabled = false;
            guaranteedMonthTextBox.Enabled = false;
            guaranteedDayTextBox.Enabled = false;
        }

        /// <summary>
        ///     独立保障の編集項目を有効化する
        /// </summary>
        private void EnableGuaranteedItems()
        {
            guaranteedGroupBox.Enabled = true;
        }

        /// <summary>
        ///     独立保障の編集項目を無効化する
        /// </summary>
        private void DisableGuaranteedItems()
        {
            guaranteedGroupBox.Enabled = false;
        }

        #endregion

        #region 関係タブ - 不可侵条約

        /// <summary>
        ///     不可侵条約の編集項目を初期化する
        /// </summary>
        private void InitNonAggressionItems()
        {
            _itemControls.Add(ScenarioEditorItemId.DiplomacyNonAggression, nonAggressionCheckBox);
            _itemControls.Add(ScenarioEditorItemId.DiplomacyNonAggressionStartYear, nonAggressionStartYearTextBox);
            _itemControls.Add(ScenarioEditorItemId.DiplomacyNonAggressionStartMonth, nonAggressionStartMonthTextBox);
            _itemControls.Add(ScenarioEditorItemId.DiplomacyNonAggressionStartDay, nonAggressionStartDayTextBox);
            _itemControls.Add(ScenarioEditorItemId.DiplomacyNonAggressionEndYear, nonAggressionEndYearTextBox);
            _itemControls.Add(ScenarioEditorItemId.DiplomacyNonAggressionEndMonth, nonAggressionEndMonthTextBox);
            _itemControls.Add(ScenarioEditorItemId.DiplomacyNonAggressionEndDay, nonAggressionEndDayTextBox);
            _itemControls.Add(ScenarioEditorItemId.DiplomacyNonAggressionType, nonAggressionTypeTextBox);
            _itemControls.Add(ScenarioEditorItemId.DiplomacyNonAggressionId, nonAggressionIdTextBox);

            nonAggressionCheckBox.Tag = ScenarioEditorItemId.DiplomacyNonAggression;
            nonAggressionStartYearTextBox.Tag = ScenarioEditorItemId.DiplomacyNonAggressionStartYear;
            nonAggressionStartMonthTextBox.Tag = ScenarioEditorItemId.DiplomacyNonAggressionStartMonth;
            nonAggressionStartDayTextBox.Tag = ScenarioEditorItemId.DiplomacyNonAggressionStartDay;
            nonAggressionEndYearTextBox.Tag = ScenarioEditorItemId.DiplomacyNonAggressionEndYear;
            nonAggressionEndMonthTextBox.Tag = ScenarioEditorItemId.DiplomacyNonAggressionEndMonth;
            nonAggressionEndDayTextBox.Tag = ScenarioEditorItemId.DiplomacyNonAggressionEndDay;
            nonAggressionTypeTextBox.Tag = ScenarioEditorItemId.DiplomacyNonAggressionType;
            nonAggressionIdTextBox.Tag = ScenarioEditorItemId.DiplomacyNonAggressionId;
        }

        /// <summary>
        ///     不可侵条約の編集項目を更新する
        /// </summary>
        /// <param name="treaty">協定</param>
        private void UpdateNonAggressionItems(Treaty treaty)
        {
            _controller.UpdateItemValue(nonAggressionCheckBox, treaty);
            _controller.UpdateItemValue(nonAggressionStartYearTextBox, treaty);
            _controller.UpdateItemValue(nonAggressionStartMonthTextBox, treaty);
            _controller.UpdateItemValue(nonAggressionStartDayTextBox, treaty);
            _controller.UpdateItemValue(nonAggressionEndYearTextBox, treaty);
            _controller.UpdateItemValue(nonAggressionEndMonthTextBox, treaty);
            _controller.UpdateItemValue(nonAggressionEndDayTextBox, treaty);
            _controller.UpdateItemValue(nonAggressionTypeTextBox, treaty);
            _controller.UpdateItemValue(nonAggressionIdTextBox, treaty);

            _controller.UpdateItemColor(nonAggressionCheckBox, treaty);
            _controller.UpdateItemColor(nonAggressionStartYearTextBox, treaty);
            _controller.UpdateItemColor(nonAggressionStartMonthTextBox, treaty);
            _controller.UpdateItemColor(nonAggressionStartDayTextBox, treaty);
            _controller.UpdateItemColor(nonAggressionEndYearTextBox, treaty);
            _controller.UpdateItemColor(nonAggressionEndMonthTextBox, treaty);
            _controller.UpdateItemColor(nonAggressionEndDayTextBox, treaty);
            _controller.UpdateItemColor(nonAggressionTypeTextBox, treaty);
            _controller.UpdateItemColor(nonAggressionIdTextBox, treaty);

            bool flag = (bool) _controller.GetItemValue(ScenarioEditorItemId.DiplomacyNonAggression, treaty);
            nonAggressionStartLabel.Enabled = flag;
            nonAggressionStartYearTextBox.Enabled = flag;
            nonAggressionStartMonthTextBox.Enabled = flag;
            nonAggressionStartDayTextBox.Enabled = flag;
            nonAggressionEndLabel.Enabled = flag;
            nonAggressionEndYearTextBox.Enabled = flag;
            nonAggressionEndMonthTextBox.Enabled = flag;
            nonAggressionEndDayTextBox.Enabled = flag;
            nonAggressionIdLabel.Enabled = flag;
            nonAggressionTypeTextBox.Enabled = flag;
            nonAggressionIdTextBox.Enabled = flag;
        }

        /// <summary>
        ///     不可侵条約の編集項目をクリアする
        /// </summary>
        private void ClearNonAggressionItems()
        {
            nonAggressionCheckBox.Checked = false;
            nonAggressionStartYearTextBox.Text = "";
            nonAggressionStartMonthTextBox.Text = "";
            nonAggressionStartDayTextBox.Text = "";
            nonAggressionEndYearTextBox.Text = "";
            nonAggressionEndMonthTextBox.Text = "";
            nonAggressionEndDayTextBox.Text = "";
            nonAggressionTypeTextBox.Text = "";
            nonAggressionIdTextBox.Text = "";

            nonAggressionStartLabel.Enabled = false;
            nonAggressionStartYearTextBox.Enabled = false;
            nonAggressionStartMonthTextBox.Enabled = false;
            nonAggressionStartDayTextBox.Enabled = false;
            nonAggressionEndLabel.Enabled = false;
            nonAggressionEndYearTextBox.Enabled = false;
            nonAggressionEndMonthTextBox.Enabled = false;
            nonAggressionEndDayTextBox.Enabled = false;
            nonAggressionIdLabel.Enabled = false;
            nonAggressionTypeTextBox.Enabled = false;
            nonAggressionIdTextBox.Enabled = false;
        }

        /// <summary>
        ///     不可侵条約の編集項目を有効化する
        /// </summary>
        private void EnableNonAggressionItems()
        {
            nonAggressionGroupBox.Enabled = true;
        }

        /// <summary>
        ///     不可侵条約の編集項目を無効化する
        /// </summary>
        private void DisableNonAggressionItems()
        {
            nonAggressionGroupBox.Enabled = false;
        }

        #endregion

        #region 関係タブ - 講和条約

        /// <summary>
        ///     講和条約の編集項目を初期化する
        /// </summary>
        private void InitPeaceItems()
        {
            _itemControls.Add(ScenarioEditorItemId.DiplomacyPeace, peaceCheckBox);
            _itemControls.Add(ScenarioEditorItemId.DiplomacyPeaceStartYear, peaceStartYearTextBox);
            _itemControls.Add(ScenarioEditorItemId.DiplomacyPeaceStartMonth, peaceStartMonthTextBox);
            _itemControls.Add(ScenarioEditorItemId.DiplomacyPeaceStartDay, peaceStartDayTextBox);
            _itemControls.Add(ScenarioEditorItemId.DiplomacyPeaceEndYear, peaceEndYearTextBox);
            _itemControls.Add(ScenarioEditorItemId.DiplomacyPeaceEndMonth, peaceEndMonthTextBox);
            _itemControls.Add(ScenarioEditorItemId.DiplomacyPeaceEndDay, peaceEndDayTextBox);
            _itemControls.Add(ScenarioEditorItemId.DiplomacyPeaceType, peaceTypeTextBox);
            _itemControls.Add(ScenarioEditorItemId.DiplomacyPeaceId, peaceIdTextBox);

            peaceCheckBox.Tag = ScenarioEditorItemId.DiplomacyPeace;
            peaceStartYearTextBox.Tag = ScenarioEditorItemId.DiplomacyPeaceStartYear;
            peaceStartMonthTextBox.Tag = ScenarioEditorItemId.DiplomacyPeaceStartMonth;
            peaceStartDayTextBox.Tag = ScenarioEditorItemId.DiplomacyPeaceStartDay;
            peaceEndYearTextBox.Tag = ScenarioEditorItemId.DiplomacyPeaceEndYear;
            peaceEndMonthTextBox.Tag = ScenarioEditorItemId.DiplomacyPeaceEndMonth;
            peaceEndDayTextBox.Tag = ScenarioEditorItemId.DiplomacyPeaceEndDay;
            peaceTypeTextBox.Tag = ScenarioEditorItemId.DiplomacyPeaceType;
            peaceIdTextBox.Tag = ScenarioEditorItemId.DiplomacyPeaceId;
        }

        /// <summary>
        ///     講和条約の編集項目を更新する
        /// </summary>
        /// <param name="treaty">協定</param>
        private void UpdatePeaceItems(Treaty treaty)
        {
            _controller.UpdateItemValue(peaceCheckBox, treaty);
            _controller.UpdateItemValue(peaceStartYearTextBox, treaty);
            _controller.UpdateItemValue(peaceStartMonthTextBox, treaty);
            _controller.UpdateItemValue(peaceStartDayTextBox, treaty);
            _controller.UpdateItemValue(peaceEndYearTextBox, treaty);
            _controller.UpdateItemValue(peaceEndMonthTextBox, treaty);
            _controller.UpdateItemValue(peaceEndDayTextBox, treaty);
            _controller.UpdateItemValue(peaceTypeTextBox, treaty);
            _controller.UpdateItemValue(peaceIdTextBox, treaty);

            _controller.UpdateItemColor(peaceCheckBox, treaty);
            _controller.UpdateItemColor(peaceStartYearTextBox, treaty);
            _controller.UpdateItemColor(peaceStartMonthTextBox, treaty);
            _controller.UpdateItemColor(peaceStartDayTextBox, treaty);
            _controller.UpdateItemColor(peaceEndYearTextBox, treaty);
            _controller.UpdateItemColor(peaceEndMonthTextBox, treaty);
            _controller.UpdateItemColor(peaceEndDayTextBox, treaty);
            _controller.UpdateItemColor(peaceTypeTextBox, treaty);
            _controller.UpdateItemColor(peaceIdTextBox, treaty);

            bool flag = (bool) _controller.GetItemValue(ScenarioEditorItemId.DiplomacyPeace, treaty);
            peaceStartLabel.Enabled = flag;
            peaceStartYearTextBox.Enabled = flag;
            peaceStartMonthTextBox.Enabled = flag;
            peaceStartDayTextBox.Enabled = flag;
            peaceEndLabel.Enabled = flag;
            peaceEndYearTextBox.Enabled = flag;
            peaceEndMonthTextBox.Enabled = flag;
            peaceEndDayTextBox.Enabled = flag;
            peaceIdLabel.Enabled = flag;
            peaceTypeTextBox.Enabled = flag;
            peaceIdTextBox.Enabled = flag;
        }

        /// <summary>
        ///     講和条約の編集項目をクリアする
        /// </summary>
        private void ClearPeaceItems()
        {
            peaceCheckBox.Checked = false;
            peaceStartYearTextBox.Text = "";
            peaceStartMonthTextBox.Text = "";
            peaceStartDayTextBox.Text = "";
            peaceEndYearTextBox.Text = "";
            peaceEndMonthTextBox.Text = "";
            peaceEndDayTextBox.Text = "";
            peaceTypeTextBox.Text = "";
            peaceIdTextBox.Text = "";

            peaceStartLabel.Enabled = false;
            peaceStartYearTextBox.Enabled = false;
            peaceStartMonthTextBox.Enabled = false;
            peaceStartDayTextBox.Enabled = false;
            peaceEndLabel.Enabled = false;
            peaceEndYearTextBox.Enabled = false;
            peaceEndMonthTextBox.Enabled = false;
            peaceEndDayTextBox.Enabled = false;
            peaceIdLabel.Enabled = false;
            peaceTypeTextBox.Enabled = false;
            peaceIdTextBox.Enabled = false;
        }

        /// <summary>
        ///     講和条約の編集項目を有効化する
        /// </summary>
        private void EnablePeaceItems()
        {
            peaceGroupBox.Enabled = true;
        }

        /// <summary>
        ///     講和条約の編集項目を無効化する
        /// </summary>
        private void DisablePeaceItems()
        {
            peaceGroupBox.Enabled = false;
        }

        #endregion

        #region 関係タブ - 諜報

        /// <summary>
        ///     諜報情報の編集項目を更新する
        /// </summary>
        private void InitIntelligenceItems()
        {
            _itemControls.Add(ScenarioEditorItemId.IntelligenceSpies, spyNumNumericUpDown);

            spyNumNumericUpDown.Tag = ScenarioEditorItemId.IntelligenceSpies;
        }

        /// <summary>
        ///     諜報情報の編集項目を更新する
        /// </summary>
        /// <param name="spy">諜報設定</param>
        private void UpdateIntelligenceItems(SpySettings spy)
        {
            _controller.UpdateItemValue(spyNumNumericUpDown, spy);

            _controller.UpdateItemColor(spyNumNumericUpDown, spy);
        }

        /// <summary>
        ///     諜報情報の編集項目をクリアする
        /// </summary>
        private void ClearIntelligenceItems()
        {
            spyNumNumericUpDown.Value = 0;
        }

        /// <summary>
        ///     諜報情報の編集項目を有効化する
        /// </summary>
        private void EnableIntelligenceItems()
        {
            intelligenceGroupBox.Enabled = true;
        }

        /// <summary>
        ///     諜報情報の編集項目を無効化する
        /// </summary>
        private void DisableIntelligenceItems()
        {
            intelligenceGroupBox.Enabled = false;
        }

        #endregion

        #region 関係タブ - 編集項目

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRelationIntItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country selected = GetSelectedRelationCountry();
            if (selected == Country.None)
            {
                return;
            }
            Country target = GetTargetRelationCountry();
            if (target == Country.None)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(selected);
            Relation relation = Scenarios.GetCountryRelation(selected, target);

            // 文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, relation);
                return;
            }

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, relation);
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
            if (!_controller.IsItemValueValid(itemId, val))
            {
                _controller.UpdateItemValue(control, relation);
                return;
            }

            if (settings == null)
            {
                settings = Scenarios.CreateCountrySettings(selected);
            }

            if (relation == null)
            {
                relation = new Relation { Country = target };
                settings.Relations.Add(relation);
                Scenarios.SetCountryRelation(selected, relation);
            }

            _controller.OutputItemValueChangedLog(itemId, val, selected, relation);

            // 値を更新する
            _controller.SetItemValue(itemId, val, relation);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, relation, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, relation);
        }

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRelationDoubleItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country selected = GetSelectedRelationCountry();
            if (selected == Country.None)
            {
                return;
            }
            Country target = GetTargetRelationCountry();
            if (target == Country.None)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(selected);
            Relation relation = Scenarios.GetCountryRelation(selected, target);

            // 文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, relation);
                return;
            }

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, relation);
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
            if (!_controller.IsItemValueValid(itemId, val))
            {
                _controller.UpdateItemValue(control, relation);
                return;
            }

            if (settings == null)
            {
                settings = Scenarios.CreateCountrySettings(selected);
            }

            if (relation == null)
            {
                relation = new Relation { Country = target };
                settings.Relations.Add(relation);
                Scenarios.SetCountryRelation(selected, relation);
            }

            _controller.OutputItemValueChangedLog(itemId, val, selected, relation);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, relation);

            // 値を更新する
            _controller.SetItemValue(itemId, val, relation);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, relation, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, relation);
        }

        /// <summary>
        ///     チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRelationItemCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country selected = GetSelectedRelationCountry();
            if (selected == Country.None)
            {
                return;
            }
            Country target = GetTargetRelationCountry();
            if (target == Country.None)
            {
                return;
            }

            CheckBox control = sender as CheckBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(selected);
            Relation relation = Scenarios.GetCountryRelation(selected, target);

            // 初期値から変更されていなければ何もしない
            bool val = control.Checked;
            object prev = _controller.GetItemValue(itemId, relation);
            if ((prev == null) && !val)
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((prev != null) && (val == (bool) prev))
            {
                return;
            }

            if (settings == null)
            {
                settings = Scenarios.CreateCountrySettings(selected);
            }

            if (relation == null)
            {
                relation = new Relation { Country = target };
                settings.Relations.Add(relation);
                Scenarios.SetCountryRelation(selected, relation);
            }

            _controller.OutputItemValueChangedLog(itemId, val, selected, relation);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, relation);

            // 値を更新する
            _controller.SetItemValue(itemId, val, relation);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, relation, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, relation, settings);
        }

        /// <summary>
        ///     チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRelationCountryItemCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country selected = GetSelectedRelationCountry();
            if (selected == Country.None)
            {
                return;
            }
            Country target = GetTargetRelationCountry();
            if (target == Country.None)
            {
                return;
            }

            CheckBox control = sender as CheckBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(selected);

            // 初期値から変更されていなければ何もしない
            Country val = control.Checked ? target : Country.None;
            object prev = _controller.GetItemValue(itemId, settings);
            if ((prev == null) && (val == Country.None))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((prev != null) && (val == (Country) prev))
            {
                return;
            }

            if (settings == null)
            {
                settings = Scenarios.CreateCountrySettings(selected);
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
        private void OnRelationNonAggressionItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country selected = GetSelectedRelationCountry();
            if (selected == Country.None)
            {
                return;
            }
            Country target = GetTargetRelationCountry();
            if (target == Country.None)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            Treaty treaty = Scenarios.GetNonAggression(selected, target);

            // 文字列を数値に変換できなければ値を戻す
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
            if (!_controller.IsItemValueValid(itemId, val))
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
        private void OnRelationNonAggressionItemCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country selected = GetSelectedRelationCountry();
            if (selected == Country.None)
            {
                return;
            }
            Country target = GetTargetRelationCountry();
            if (target == Country.None)
            {
                return;
            }

            CheckBox control = sender as CheckBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            Treaty treaty = Scenarios.GetNonAggression(selected, target);

            // 初期値から変更されていなければ何もしない
            bool val = control.Checked;
            object prev = _controller.GetItemValue(itemId, treaty);
            if ((prev == null) && !val)
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((prev != null) && (val == (bool) prev))
            {
                return;
            }

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, treaty);

            // 値を更新する
            if (val)
            {
                treaty = new Treaty
                {
                    Type = TreatyType.NonAggression,
                    Country1 = selected,
                    Country2 = target,
                    StartDate = new GameDate(),
                    EndDate = new GameDate(),
                    Id = Scenarios.GetNewTypeId(Scenarios.DefaultTreatyType, 1)
                };
                Scenarios.Data.GlobalData.NonAggressions.Add(treaty);
                Scenarios.SetNonAggression(treaty);
            }
            else
            {
                Scenarios.Data.GlobalData.NonAggressions.Remove(treaty);
                Scenarios.RemoveNonAggression(treaty);
            }

            _controller.OutputItemValueChangedLog(itemId, val, !val, treaty);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, treaty);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, val ? treaty : null);
        }

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRelationPeaceItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country selected = GetSelectedRelationCountry();
            if (selected == Country.None)
            {
                return;
            }
            Country target = GetTargetRelationCountry();
            if (target == Country.None)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            Treaty treaty = Scenarios.GetPeace(selected, target);

            // 文字列を数値に変換できなければ値を戻す
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
            if (!_controller.IsItemValueValid(itemId, val))
            {
                _controller.UpdateItemValue(control, treaty);
                return;
            }

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, treaty);

            // 値を更新する
            _controller.SetItemValue(itemId, val, treaty);

            _controller.OutputItemValueChangedLog(itemId, val, treaty);

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
        private void OnRelationPeaceItemCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country selected = GetSelectedRelationCountry();
            if (selected == Country.None)
            {
                return;
            }
            Country target = GetTargetRelationCountry();
            if (target == Country.None)
            {
                return;
            }

            CheckBox control = sender as CheckBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            Treaty treaty = Scenarios.GetPeace(selected, target);

            // 初期値から変更されていなければ何もしない
            bool val = control.Checked;
            object prev = _controller.GetItemValue(itemId, treaty);
            if ((prev == null) && !val)
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((prev != null) && (val == (bool) prev))
            {
                return;
            }

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, treaty);

            // 値を更新する
            if (val)
            {
                treaty = new Treaty
                {
                    Type = TreatyType.Peace,
                    Country1 = selected,
                    Country2 = target,
                    StartDate = new GameDate(),
                    EndDate = new GameDate(),
                    Id = Scenarios.GetNewTypeId(Scenarios.DefaultTreatyType, 1)
                };
                Scenarios.Data.GlobalData.Peaces.Add(treaty);
                Scenarios.SetPeace(treaty);
            }
            else
            {
                Scenarios.Data.GlobalData.Peaces.Remove(treaty);
                Scenarios.RemovePeace(treaty);
            }

            _controller.OutputItemValueChangedLog(itemId, val, !val, treaty);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, treaty);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, val ? treaty : null);
        }

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRelationIntelligenceItemNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country selected = GetSelectedRelationCountry();
            if (selected == Country.None)
            {
                return;
            }
            Country target = GetTargetRelationCountry();
            if (target == Country.None)
            {
                return;
            }

            NumericUpDown control = sender as NumericUpDown;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(selected);
            SpySettings spy = Scenarios.GetCountryIntelligence(selected, target);

            // 初期値から変更されていなければ何もしない
            int val = (int) control.Value;
            object prev = _controller.GetItemValue(itemId, spy);
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
            if (!_controller.IsItemValueValid(itemId, val))
            {
                _controller.UpdateItemValue(control, spy);
                return;
            }

            if (settings == null)
            {
                settings = Scenarios.CreateCountrySettings(selected);
            }

            if (spy == null)
            {
                spy = new SpySettings { Country = target };
                Scenarios.SetCountryIntelligence(selected, spy);
            }

            _controller.OutputItemValueChangedLog(itemId, val, selected, spy);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, spy);

            // 値を更新する
            _controller.SetItemValue(itemId, val, spy);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, spy, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, spy);
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

            // 編集項目を無効化する
            DisableTradeInfoItems();
            DisableTradeDealsItems();
            DisableTradeButtons();

            // 編集項目をクリアする
            ClearTradeInfoItems();
            ClearTradeDealsItems();
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
            tradeUpButton.Enabled = index > 0;
            tradeDownButton.Enabled = index < count - 1;
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
            if (index >= 0)
            {
                tradeListView.Items[index].Focused = true;
                tradeListView.Items[index].Selected = true;
            }
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
        ///     選択中の貿易情報を取得する
        /// </summary>
        /// <returns>選択中の貿易情報</returns>
        private Treaty GetSelectedTrade()
        {
            return tradeListView.SelectedItems.Count > 0 ? tradeListView.SelectedItems[0].Tag as Treaty : null;
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
        ///     貿易情報の編集項目を更新する
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
        ///     貿易内容の編集項目を更新する
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

            // 文字列を数値に変換できなければ値を戻す
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

            // 文字列を数値に変換できなければ値を戻す
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
            Brush brush = (val == sel) && _controller.IsItemDirty(itemId, treaty)
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
            return countryListBox.SelectedIndex >= 0 ? Countries.Tags[countryListBox.SelectedIndex] : Country.None;
        }

        #endregion

        #region 国家タブ - 国家情報

        /// <summary>
        ///     国家情報の編集項目を初期化する
        /// </summary>
        private void InitCountryInfoItems()
        {
            _itemControls.Add(ScenarioEditorItemId.CountryNameKey, countryNameKeyTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryNameString, countryNameStringTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryFlagExt, flagExtTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryRegularId, regularIdComboBox);
            _itemControls.Add(ScenarioEditorItemId.CountryBelligerence, belligerenceTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryDissent, dissentTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryExtraTc, extraTcTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryNuke, nukeTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryNukeYear, nukeYearTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryNukeMonth, nukeMonthTextBox);
            _itemControls.Add(ScenarioEditorItemId.CountryNukeDay, nukeDayTextBox);

            countryNameKeyTextBox.Tag = ScenarioEditorItemId.CountryNameKey;
            countryNameStringTextBox.Tag = ScenarioEditorItemId.CountryNameString;
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
        ///     国家情報の編集項目を更新する
        /// </summary>
        /// <param name="country">選択国</param>
        /// <param name="settings">国家設定</param>
        private void UpdateCountryInfoItems(Country country, CountrySettings settings)
        {
            // 編集項目の表示を更新する
            _controller.UpdateItemValue(countryNameKeyTextBox, settings);
            _controller.UpdateItemValue(countryNameStringTextBox, country, settings);
            _controller.UpdateItemValue(flagExtTextBox, settings);
            _controller.UpdateItemValue(regularIdComboBox, settings);
            _controller.UpdateItemValue(belligerenceTextBox, settings);
            _controller.UpdateItemValue(dissentTextBox, settings);
            _controller.UpdateItemValue(extraTcTextBox, settings);
            _controller.UpdateItemValue(nukeTextBox, settings);
            _controller.UpdateItemValue(nukeYearTextBox, settings);
            _controller.UpdateItemValue(nukeMonthTextBox, settings);
            _controller.UpdateItemValue(nukeDayTextBox, settings);

            // 編集項目の色を更新する
            _controller.UpdateItemColor(countryNameKeyTextBox, settings);
            _controller.UpdateItemColor(countryNameStringTextBox, country, settings);
            _controller.UpdateItemColor(flagExtTextBox, settings);
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
            countryNameKeyTextBox.Text = "";
            countryNameStringTextBox.Text = "";
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
        ///     国家補正値の編集項目を更新する
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
        ///     国家資源情報の編集項目を更新する
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
        ///     国家AI情報の編集項目を更新する
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

            // 文字列を数値に変換できなければ値を戻す
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
                settings = Scenarios.CreateCountrySettings(country);
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

            // 文字列を数値に変換できなければ値を戻す
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
                settings = Scenarios.CreateCountrySettings(country);
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
            object prev = _controller.GetItemValue(itemId, country, settings);
            string val = control.Text;
            if ((prev == null) && string.IsNullOrEmpty(val))
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
                settings = Scenarios.CreateCountrySettings(country);
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
            object prev = _controller.GetItemValue(itemId, country, settings);
            string val = control.Text;
            if ((prev == null) && string.IsNullOrEmpty(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (val.Equals(_controller.GetItemValue(itemId, country, settings)))
            {
                return;
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
                Brush brush = (val != null) && ((Country) val == (Country) sel) &&
                              _controller.IsItemDirty(itemId, settings)
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
            Country val = control.SelectedIndex > 0 ? Countries.Tags[control.SelectedIndex - 1] : Country.None;
            if ((settings != null) && (val == (Country) _controller.GetItemValue(itemId, settings)))
            {
                return;
            }

            if (settings == null)
            {
                settings = Scenarios.CreateCountrySettings(country);
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

            // 編集項目を無効化する
            DisablePoliticalSliderItems();
            DisableCabinetItems();

            // 編集項目をクリアする
            ClearPoliticalSliderItems();
            ClearCabinetItems();

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
            Ministers.WaitLoading();

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
            Ministers.WaitLoading();

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
            int year = header.StartDate?.Year ?? header.StartYear;

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
        ///     政策スライダーの編集項目を更新する
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
        ///     閣僚の編集項目を更新する
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

            // 文字列を数値に変換できなければ値を戻す
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
                settings = Scenarios.CreateCountrySettings(country);
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
                settings = Scenarios.CreateCountrySettings(country);
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
            Brush brush = (val != null) && (sel != null) && ((int) val == (int) sel) &&
                          _controller.IsItemDirty(itemId, settings)
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
                settings = Scenarios.CreateCountrySettings(country);
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

            // 編集項目を無効化する
            DisableTechItems();

            // 編集項目をクリアする
            ClearTechItems();

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
            Techs.WaitLoading();

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
            Techs.WaitLoading();

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
        ///     技術の編集項目を更新する
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
            if ((settings != null) && (val == settings.TechApps.Contains(item.Id)))
            {
                return;
            }

            Log.Info("[Scenario] owned techs: {0}{1} ({2})", val ? '+' : '-', item.Id, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = Scenarios.CreateCountrySettings(country);
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
            _techTreePanelController.UpdateItem(item);
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
            if ((settings != null) && (val == settings.BluePrints.Contains(item.Id)))
            {
                return;
            }

            Log.Info("[Scenario] blurprints: {0}{1} ({2})", val ? '+' : '-', item.Id, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = Scenarios.CreateCountrySettings(country);
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
            _techTreePanelController.UpdateItem(item);
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
            if ((settings != null) && (val == settings.Inventions.Contains(ev.Id)))
            {
                return;
            }

            Log.Info("[Scenario] inventions: {0}{1} ({2})", val ? '+' : '-', ev.Id, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = Scenarios.CreateCountrySettings(country);
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
            _techTreePanelController.UpdateItem(ev);
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
                if (e.Button == MouseButtons.Left)
                {
                    ToggleOwnedTech(tech, country);
                }
                // 右クリックで青写真の有無を切り替える
                else if (e.Button == MouseButtons.Right)
                {
                    ToggleBlueprint(tech, country);
                }
                return;
            }

            TechEvent ev = e.Item as TechEvent;
            if (ev != null)
            {
                // 左クリックで保有技術の有無を切り替える
                if (e.Button == MouseButtons.Left)
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
                settings = Scenarios.CreateCountrySettings(country);
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
            _techTreePanelController.UpdateItem(item);

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
                settings = Scenarios.CreateCountrySettings(country);
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
            _techTreePanelController.UpdateItem(item);

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
                settings = Scenarios.CreateCountrySettings(country);
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
            _techTreePanelController.UpdateItem(item);

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

            // 陸地プロヴィンスリストを初期化する
            _controller.InitProvinceList();

            // プロヴィンスリストを初期化する
            InitProvinceList();

            // 国家フィルターを更新する
            UpdateProvinceCountryFilter();

            // プロヴィンスリストを有効化する
            EnableProvinceList();

            // 国家フィルターを有効化する
            EnableProvinceCountryFilter();

            // IDテキストボックスを有効化する
            EnableProvinceIdTextBox();

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
            Provinces.WaitLoading();

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
            Provinces.WaitLoading();

            // 初回遷移時には表示を更新する
            UpdateProvinceTab();

            // 読み込み済みで未初期化ならばマップパネルを更新する
            UpdateMapPanel();
        }

        #endregion

        #region プロヴィンスタブ - マップ

        /// <summary>
        ///     マップパネルを更新する
        /// </summary>
        private void UpdateMapPanel()
        {
            // マップ読み込み前ならば何もしない
            if (!Maps.IsLoaded[(int) MapLevel.Level2])
            {
                return;
            }

            // 初期化済みであれば何もしない
            if (_mapPanelInitialized)
            {
                return;
            }

            // 初期化済みフラグを設定する
            _mapPanelInitialized = true;

            // マップパネルを有効化する
            _mapPanelController.ProvinceMouseClick += OnMapPanelMouseClick;
            _mapPanelController.Show();

            // マップフィルターを有効化する
            EnableMapFilter();

            // 選択プロヴィンスが表示されるようにスクロールする
            Province province = GetSelectedProvince();
            if (province != null)
            {
                _mapPanelController.ScrollToProvince(province.Id);
            }
        }

        /// <summary>
        ///     マップパネルのマウスクリック時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMapPanelMouseClick(object sender, MapPanelController.ProvinceEventArgs e)
        {
            // 左クリック以外では何もしない
            if (e.Button != MouseButtons.Left)
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
            switch (_mapPanelController.FilterMode)
            {
                case MapPanelController.MapFilterMode.None:
                    Country target = (from settings in Scenarios.Data.Countries
                        where settings.ControlledProvinces.Contains(e.Id)
                        select settings.Country).FirstOrDefault();
                    provinceCountryFilterComboBox.SelectedIndex = Array.IndexOf(Countries.Tags, target) + 1;
                    break;

                case MapPanelController.MapFilterMode.Core:
                    if (country != Country.None)
                    {
                        coreProvinceCheckBox.Checked = !coreProvinceCheckBox.Checked;
                    }
                    break;

                case MapPanelController.MapFilterMode.Owned:
                    if (country != Country.None && ownedProvinceCheckBox.Enabled)
                    {
                        ownedProvinceCheckBox.Checked = !ownedProvinceCheckBox.Checked;
                    }
                    break;

                case MapPanelController.MapFilterMode.Controlled:
                    if (country != Country.None && controlledProvinceCheckBox.Enabled)
                    {
                        controlledProvinceCheckBox.Checked = !controlledProvinceCheckBox.Checked;
                    }
                    break;

                case MapPanelController.MapFilterMode.Claimed:
                    if (country != Country.None)
                    {
                        claimedProvinceCheckBox.Checked = !claimedProvinceCheckBox.Checked;
                    }
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
        ///     国家フィルターを更新する
        /// </summary>
        private void UpdateProvinceCountryFilter()
        {
            provinceCountryFilterComboBox.BeginUpdate();
            provinceCountryFilterComboBox.Items.Clear();
            provinceCountryFilterComboBox.Items.Add("");
            foreach (Country country in Countries.Tags)
            {
                provinceCountryFilterComboBox.Items.Add(Scenarios.GetCountryTagName(country));
            }
            provinceCountryFilterComboBox.EndUpdate();
        }

        /// <summary>
        ///     国家フィルターを有効化する
        /// </summary>
        private void EnableProvinceCountryFilter()
        {
            provinceCountryFilterLabel.Enabled = true;
            provinceCountryFilterComboBox.Enabled = true;
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
            UpdateProvinceList(country);

            // マップフィルターを更新する
            _mapPanelController.SelectedCountry = country;

            // プロヴィンス国家グループボックスの編集項目を更新する
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

            // 文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(provinceIdTextBox.Text, out val))
            {
                if (province != null)
                {
                    provinceIdTextBox.Text = IntHelper.ToString(province.Id);
                }
                return;
            }

            // 初期値から変更されていなければ何もしない
            if ((province == null) && (val == 0))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((province != null) && (val == province.Id))
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
        ///     陸地プロヴィンスリストを初期化する
        /// </summary>
        private void InitProvinceList()
        {
            provinceListView.BeginUpdate();
            provinceListView.Items.Clear();
            foreach (Province province in _controller.GetLandProvinces())
            {
                ListViewItem item = CreateProvinceListItem(province);
                provinceListView.Items.Add(item);
            }
            provinceListView.EndUpdate();
        }

        /// <summary>
        ///     プロヴィンスリストを更新する
        /// </summary>
        /// <param name="country">選択国</param>
        private void UpdateProvinceList(Country country)
        {
            CountrySettings settings = Scenarios.GetCountrySettings(country);

            provinceListView.BeginUpdate();
            if (settings != null)
            {
                foreach (ListViewItem item in provinceListView.Items)
                {
                    Province province = (Province) item.Tag;
                    item.SubItems[2].Text = province.Id == settings.Capital ? Resources.Yes : "";
                    item.SubItems[3].Text = settings.NationalProvinces.Contains(province.Id) ? Resources.Yes : "";
                    item.SubItems[4].Text = settings.OwnedProvinces.Contains(province.Id) ? Resources.Yes : "";
                    item.SubItems[5].Text = settings.ControlledProvinces.Contains(province.Id) ? Resources.Yes : "";
                    item.SubItems[6].Text = settings.ClaimedProvinces.Contains(province.Id) ? Resources.Yes : "";
                }
            }
            else
            {
                foreach (ListViewItem item in provinceListView.Items)
                {
                    item.SubItems[2].Text = "";
                    item.SubItems[3].Text = "";
                    item.SubItems[4].Text = "";
                    item.SubItems[5].Text = "";
                    item.SubItems[6].Text = "";
                }
            }
            provinceListView.EndUpdate();
        }

        /// <summary>
        ///     プロヴィンスリストを有効化する
        /// </summary>
        private void EnableProvinceList()
        {
            provinceListView.Enabled = true;
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
        ///     プロヴィンスリストビューの項目を作成する
        /// </summary>
        /// <param name="province">プロヴィンスデータ</param>
        /// <returns>プロヴィンスリストビューの項目</returns>
        private static ListViewItem CreateProvinceListItem(Province province)
        {
            ProvinceSettings settings = Scenarios.GetProvinceSettings(province.Id);

            ListViewItem item = new ListViewItem { Text = IntHelper.ToString(province.Id), Tag = province };
            item.SubItems.Add(Scenarios.GetProvinceName(province, settings));
            item.SubItems.Add("");
            item.SubItems.Add("");
            item.SubItems.Add("");
            item.SubItems.Add("");
            item.SubItems.Add("");

            return item;
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
        ///     プロヴィンス国家情報の編集項目を更新する
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

        #endregion

        #region プロヴィンスタブ - プロヴィンス情報

        /// <summary>
        ///     プロヴィンス情報の編集項目を初期化する
        /// </summary>
        private void InitProvinceInfoItems()
        {
            _itemControls.Add(ScenarioEditorItemId.ProvinceNameKey, provinceNameKeyTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceNameString, provinceNameStringTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceVp, vpTextBox);
            _itemControls.Add(ScenarioEditorItemId.ProvinceRevoltRisk, revoltRiskTextBox);

            provinceNameKeyTextBox.Tag = ScenarioEditorItemId.ProvinceNameKey;
            provinceNameStringTextBox.Tag = ScenarioEditorItemId.ProvinceNameString;
            vpTextBox.Tag = ScenarioEditorItemId.ProvinceVp;
            revoltRiskTextBox.Tag = ScenarioEditorItemId.ProvinceRevoltRisk;
        }

        /// <summary>
        ///     プロヴィンス情報の編集項目を更新する
        /// </summary>
        /// <param name="province">プロヴィンス</param>
        /// <param name="settings">プロヴィンス設定</param>
        private void UpdateProvinceInfoItems(Province province, ProvinceSettings settings)
        {
            _controller.UpdateItemValue(provinceIdTextBox, province);
            _controller.UpdateItemValue(provinceNameKeyTextBox, province, settings);
            _controller.UpdateItemValue(provinceNameStringTextBox, province, settings);
            _controller.UpdateItemValue(vpTextBox, settings);
            _controller.UpdateItemValue(revoltRiskTextBox, settings);

            _controller.UpdateItemColor(provinceNameKeyTextBox, settings);
            _controller.UpdateItemColor(provinceNameStringTextBox, settings);
            _controller.UpdateItemColor(vpTextBox, settings);
            _controller.UpdateItemColor(revoltRiskTextBox, settings);
        }

        /// <summary>
        ///     プロヴィンス情報の編集項目の表示をクリアする
        /// </summary>
        private void ClearProvinceInfoItems()
        {
            provinceIdTextBox.Text = "";
            provinceNameKeyTextBox.Text = "";
            provinceNameStringTextBox.Text = "";
            vpTextBox.Text = "";
            revoltRiskTextBox.Text = "";
        }

        /// <summary>
        ///     プロヴィンス情報の編集項目を有効化する
        /// </summary>
        private void EnableProvinceInfoItems()
        {
            provinceInfoGroupBox.Enabled = true;
            provinceNameKeyTextBox.Enabled = Game.Type == GameType.DarkestHour;
        }

        /// <summary>
        ///     プロヴィンス情報の編集項目を無効化する
        /// </summary>
        private void DisableProvinceInfoItems()
        {
            provinceInfoGroupBox.Enabled = false;
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
        ///     プロヴィンス資源情報の編集項目を更新する
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
        ///     プロヴィンス建物情報の編集項目を更新する
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

        /// <summary>
        ///     プロヴィンス建物情報の編集項目を有効化する
        /// </summary>
        private void EnableProvinceBuildingItems()
        {
            provinceBuildingGroupBox.Enabled = true;

            bool flag = Game.Type == GameType.ArsenalOfDemocracy;
            provinceSyntheticOilLabel.Enabled = flag;
            syntheticOilCurrentTextBox.Enabled = flag;
            syntheticOilMaxTextBox.Enabled = flag;
            syntheticOilRelativeTextBox.Enabled = flag;
            provinceSyntheticRaresLabel.Enabled = flag;
            syntheticRaresCurrentTextBox.Enabled = flag;
            syntheticRaresMaxTextBox.Enabled = flag;
            syntheticRaresRelativeTextBox.Enabled = flag;
            provinceNuclearPowerLabel.Enabled = flag;
            nuclearPowerCurrentTextBox.Enabled = flag;
            nuclearPowerMaxTextBox.Enabled = flag;
            nuclearPowerRelativeTextBox.Enabled = flag;
        }

        /// <summary>
        ///     プロヴィンス建物情報の編集項目を無効化する
        /// </summary>
        private void DisableProvinceBuildingItems()
        {
            provinceBuildingGroupBox.Enabled = false;
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

            // 文字列を数値に変換できなければ値を戻す
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

            // 文字列を数値に変換できなければ値を戻す
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
            object prev = _controller.GetItemValue(itemId, province, settings);
            string val = control.Text;
            if ((prev == null) && string.IsNullOrEmpty(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (val.Equals(prev))
            {
                return;
            }

            if (settings == null)
            {
                settings = new ProvinceSettings { Id = province.Id };
                Scenarios.AddProvinceSettings(settings);
            }

            _controller.OutputItemValueChangedLog(itemId, val, province, settings);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, settings);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, province, settings);
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
                settings = Scenarios.CreateCountrySettings(country);
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
            _controller.PostItemChanged(itemId, val, province, settings);
        }

        #endregion

        #endregion

        #region 初期部隊タブ

        #region 初期部隊タブ - 共通

        /// <summary>
        ///     初期部隊タブを初期化する
        /// </summary>
        private void InitOobTab()
        {
            InitUnitTree();
            InitOobUnitItems();
            InitOobDivisionItems();
        }

        /// <summary>
        ///     初期部隊タブの表示を更新する
        /// </summary>
        private void UpdateOobTab()
        {
            // 初期化済みであれば何もしない
            if (_tabPageInitialized[(int) TabPageNo.Oob])
            {
                return;
            }

            // プロヴィンスリストを初期化する
            _controller.InitProvinceList();

            // ユニット種類リストを初期化する
            _controller.InitUnitTypeList();

            // ユニットツリーコントローラの選択国を解除する
            _unitTreeController.Country = Country.None;

            // 国家リストボックスを更新する
            UpdateCountryListBox(oobCountryListBox);

            // 国家リストボックスを有効化する
            EnableOobCountryListBox();

            // ユニットツリーを有効化する
            EnableUnitTree();

            // 編集項目を無効化する
            DisableOobUnitItems();
            DisableOobDivisionItems();

            // 編集項目をクリアする
            ClearOobUnitItems();
            ClearOobDivisionItems();

            // 初期化済みフラグをセットする
            _tabPageInitialized[(int) TabPageNo.Oob] = true;
        }

        /// <summary>
        ///     初期部隊タブのフォーム読み込み時の処理
        /// </summary>
        private void OnOobTabPageFormLoad()
        {
            // 初期部隊タブを初期化する
            InitOobTab();
        }

        /// <summary>
        ///     初期部隊タブのファイル読み込み時の処理
        /// </summary>
        private void OnOobTabPageFileLoad()
        {
            // 初期部隊タブ選択中でなければ何もしない
            if (_tabPageNo != TabPageNo.Oob)
            {
                return;
            }

            // 指揮官データの読み込み完了まで待機する
            Leaders.WaitLoading();

            // プロヴィンスデータの読み込み完了まで待機する
            Provinces.WaitLoading();

            // ユニットデータの読み込み完了まで待機する
            Units.WaitLoading();

            // 初回遷移時には表示を更新する
            UpdateOobTab();
        }

        /// <summary>
        ///     初期部隊タブ選択時の処理
        /// </summary>
        private void OnOobTabPageSelected()
        {
            // シナリオ未読み込みならば何もしない
            if (!Scenarios.IsLoaded())
            {
                return;
            }

            // 指揮官データの読み込み完了まで待機する
            Leaders.WaitLoading();

            // プロヴィンスデータの読み込み完了まで待機する
            Provinces.WaitLoading();

            // ユニットデータの読み込み完了まで待機する
            Units.WaitLoading();

            // 初回遷移時には表示を更新する
            UpdateOobTab();
        }

        #endregion

        #region 初期部隊タブ - 国家

        /// <summary>
        ///     国家リストボックスを有効化する
        /// </summary>
        private void EnableOobCountryListBox()
        {
            oobCountryListBox.Enabled = true;
        }

        /// <summary>
        ///     国家リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOobCountryListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ編集項目を無効化する
            if (oobCountryListBox.SelectedIndex < 0)
            {
                // 編集項目を無効化する
                DisableOobUnitItems();
                DisableOobDivisionItems();

                // 編集項目をクリアする
                ClearOobUnitItems();
                ClearOobDivisionItems();
                return;
            }

            _selectedCountry = Countries.Tags[oobCountryListBox.SelectedIndex];

            // 指揮官リストを初期化する
            ScenarioHeader header = Scenarios.Data.Header;
            int year = header.StartDate?.Year ?? header.StartYear;
            _controller.UpdateLeaderList(_selectedCountry, year);

            // ユニットツリーを更新する
            _unitTreeController.Country = _selectedCountry;
        }

        #endregion

        #region 初期部隊タブ - ユニットツリー

        /// <summary>
        ///     ユニットツリーを初期化する
        /// </summary>
        private void InitUnitTree()
        {
            _unitTreeController.AfterSelect += OnUnitTreeAfterSelect;
        }

        /// <summary>
        ///     ユニットツリーを有効化する
        /// </summary>
        private void EnableUnitTree()
        {
            unitTreeView.Enabled = true;

            // ツリー操作ボタンを無効化する
            oobAddUnitButton.Enabled = false;
            oobAddDivisionButton.Enabled = false;
            oobCloneButton.Enabled = false;
            oobRemoveButton.Enabled = false;
            oobTopButton.Enabled = false;
            oobUpButton.Enabled = false;
            oobDownButton.Enabled = false;
            oobBottomButton.Enabled = false;
        }

        /// <summary>
        ///     ユニットツリーのノード選択時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnitTreeAfterSelect(object sender, UnitTreeController.UnitTreeViewEventArgs e)
        {
            // ボタンの状態を更新する
            oobAddUnitButton.Enabled = e.CanAddUnit;
            oobAddDivisionButton.Enabled = e.CanAddDivision;
            bool selected = (e.Unit != null) || (e.Division != null);
            oobCloneButton.Enabled = selected;
            oobRemoveButton.Enabled = selected;
            TreeNode parent = e.Node.Parent;
            if (selected && (parent != null))
            {
                int index = parent.Nodes.IndexOf(e.Node);
                int bottom = parent.Nodes.Count - 1;
                oobTopButton.Enabled = index > 0;
                oobUpButton.Enabled = index > 0;
                oobDownButton.Enabled = index < bottom;
                oobBottomButton.Enabled = index < bottom;
            }
            else
            {
                oobTopButton.Enabled = false;
                oobUpButton.Enabled = false;
                oobDownButton.Enabled = false;
                oobBottomButton.Enabled = false;
            }

            if (e.Unit != null)
            {
                UpdateOobUnitItems(e.Unit);
                EnableOobUnitItems();
            }
            else
            {
                DisableOobUnitItems();
                ClearOobUnitItems();
            }

            if (e.Division != null)
            {
                UpdateOobDivisionItems(e.Division);
                EnableOobDivisionItems();
            }
            else
            {
                DisableOobDivisionItems();
                ClearOobDivisionItems();
            }
        }

        /// <summary>
        ///     先頭へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOobTopButtonClick(object sender, EventArgs e)
        {
            _unitTreeController.MoveTop();
        }

        /// <summary>
        ///     上へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOobUpButtonClick(object sender, EventArgs e)
        {
            _unitTreeController.MoveUp();
        }

        /// <summary>
        ///     下へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOobDownButtonClick(object sender, EventArgs e)
        {
            _unitTreeController.MoveDown();
        }

        /// <summary>
        ///     末尾へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOobBottomButtonClick(object sender, EventArgs e)
        {
            _unitTreeController.MoveBottom();
        }

        /// <summary>
        ///     新規ユニットボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOobAddUnitButtonClick(object sender, EventArgs e)
        {
            _unitTreeController.AddUnit();
        }

        /// <summary>
        ///     新規師団ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOobAddDivisionButtonClick(object sender, EventArgs e)
        {
            _unitTreeController.AddDivision();
        }

        /// <summary>
        ///     複製ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOobCloneButtonClick(object sender, EventArgs e)
        {
            _unitTreeController.Clone();
        }

        /// <summary>
        ///     削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOobRemoveButtonClick(object sender, EventArgs e)
        {
            _unitTreeController.Remove();
        }

        #endregion

        #region 初期部隊タブ - ユニット情報

        /// <summary>
        ///     ユニット情報の編集項目を初期化する
        /// </summary>
        private void InitOobUnitItems()
        {
            _itemControls.Add(ScenarioEditorItemId.UnitType, unitTypeTextBox);
            _itemControls.Add(ScenarioEditorItemId.UnitId, unitIdTextBox);
            _itemControls.Add(ScenarioEditorItemId.UnitName, unitNameTextBox);
            _itemControls.Add(ScenarioEditorItemId.UnitLocationId, locationTextBox);
            _itemControls.Add(ScenarioEditorItemId.UnitLocation, locationComboBox);
            _itemControls.Add(ScenarioEditorItemId.UnitBaseId, baseTextBox);
            _itemControls.Add(ScenarioEditorItemId.UnitBase, baseComboBox);
            _itemControls.Add(ScenarioEditorItemId.UnitLeaderId, leaderTextBox);
            _itemControls.Add(ScenarioEditorItemId.UnitLeader, leaderComboBox);
            _itemControls.Add(ScenarioEditorItemId.UnitMorale, unitMoraleTextBox);
            _itemControls.Add(ScenarioEditorItemId.UnitDigIn, digInTextBox);

            unitTypeTextBox.Tag = ScenarioEditorItemId.UnitType;
            unitIdTextBox.Tag = ScenarioEditorItemId.UnitId;
            unitNameTextBox.Tag = ScenarioEditorItemId.UnitName;
            locationTextBox.Tag = ScenarioEditorItemId.UnitLocationId;
            locationComboBox.Tag = ScenarioEditorItemId.UnitLocation;
            baseTextBox.Tag = ScenarioEditorItemId.UnitBaseId;
            baseComboBox.Tag = ScenarioEditorItemId.UnitBase;
            leaderTextBox.Tag = ScenarioEditorItemId.UnitLeaderId;
            leaderComboBox.Tag = ScenarioEditorItemId.UnitLeader;
            unitMoraleTextBox.Tag = ScenarioEditorItemId.UnitMorale;
            digInTextBox.Tag = ScenarioEditorItemId.UnitDigIn;
        }

        /// <summary>
        ///     ユニット情報の編集項目を更新する
        /// </summary>
        /// <param name="unit">ユニット</param>
        private void UpdateOobUnitItems(Unit unit)
        {
            _controller.UpdateItemValue(unitTypeTextBox, unit);
            _controller.UpdateItemValue(unitIdTextBox, unit);
            _controller.UpdateItemValue(unitNameTextBox, unit);
            _controller.UpdateItemValue(locationTextBox, unit);
            _controller.UpdateItemValue(baseTextBox, unit);
            _controller.UpdateItemValue(leaderTextBox, unit);
            _controller.UpdateItemValue(unitMoraleTextBox, unit);
            _controller.UpdateItemValue(digInTextBox, unit);

            _controller.UpdateItemColor(unitTypeTextBox, unit);
            _controller.UpdateItemColor(unitIdTextBox, unit);
            _controller.UpdateItemColor(unitNameTextBox, unit);
            _controller.UpdateItemColor(locationTextBox, unit);
            _controller.UpdateItemColor(baseTextBox, unit);
            _controller.UpdateItemColor(leaderTextBox, unit);
            _controller.UpdateItemColor(unitMoraleTextBox, unit);
            _controller.UpdateItemColor(digInTextBox, unit);

            // ユニットの兵科が変更された場合
            if (unit.Branch != _lastUnitBranch)
            {
                _lastUnitBranch = unit.Branch;

                // リストの選択肢を変更する
                _controller.UpdateListItems(locationComboBox, unit);
                _controller.UpdateListItems(baseComboBox, unit);
                _controller.UpdateListItems(leaderComboBox, unit);

                // 兵科による編集制限
                switch (unit.Branch)
                {
                    case Branch.Army:
                        baseLabel.Enabled = false;
                        baseTextBox.Enabled = false;
                        baseComboBox.Enabled = false;
                        digInLabel.Enabled = true;
                        digInTextBox.Enabled = true;
                        break;

                    case Branch.Navy:
                    case Branch.Airforce:
                        baseLabel.Enabled = true;
                        baseTextBox.Enabled = true;
                        baseComboBox.Enabled = true;
                        digInLabel.Enabled = false;
                        digInTextBox.Enabled = false;
                        break;
                }
            }

            _controller.UpdateItemValue(locationComboBox, unit);
            _controller.UpdateItemValue(baseComboBox, unit);
            _controller.UpdateItemValue(leaderComboBox, unit);

            _controller.UpdateItemColor(locationComboBox, unit);
            _controller.UpdateItemColor(baseComboBox, unit);
            _controller.UpdateItemColor(leaderComboBox, unit);
        }

        /// <summary>
        ///     ユニット情報の編集項目をクリアする
        /// </summary>
        private void ClearOobUnitItems()
        {
            unitTypeTextBox.Text = "";
            unitIdTextBox.Text = "";
            unitNameTextBox.Text = "";
            locationTextBox.Text = "";
            locationComboBox.SelectedIndex = -1;
            baseTextBox.Text = "";
            baseComboBox.SelectedIndex = -1;
            leaderTextBox.Text = "";
            leaderComboBox.SelectedIndex = -1;
            unitMoraleTextBox.Text = "";
            digInTextBox.Text = "";
        }

        /// <summary>
        ///     ユニット情報の編集項目を有効化する
        /// </summary>
        private void EnableOobUnitItems()
        {
            unitGroupBox.Enabled = true;
        }

        /// <summary>
        ///     ユニット情報の編集項目を無効化する
        /// </summary>
        private void DisableOobUnitItems()
        {
            unitGroupBox.Enabled = false;
        }

        #endregion

        #region 初期部隊タブ - 師団情報

        /// <summary>
        ///     師団情報の編集項目を初期化する
        /// </summary>
        private void InitOobDivisionItems()
        {
            _itemControls.Add(ScenarioEditorItemId.DivisionType, divisionTypeTextBox);
            _itemControls.Add(ScenarioEditorItemId.DivisionId, divisionIdTextBox);
            _itemControls.Add(ScenarioEditorItemId.DivisionName, divisionNameTextBox);
            _itemControls.Add(ScenarioEditorItemId.DivisionUnitType, unitTypeComboBox);
            _itemControls.Add(ScenarioEditorItemId.DivisionModel, unitModelComboBox);
            _itemControls.Add(ScenarioEditorItemId.DivisionBrigadeType1, brigadeTypeComboBox1);
            _itemControls.Add(ScenarioEditorItemId.DivisionBrigadeType2, brigadeTypeComboBox2);
            _itemControls.Add(ScenarioEditorItemId.DivisionBrigadeType3, brigadeTypeComboBox3);
            _itemControls.Add(ScenarioEditorItemId.DivisionBrigadeType4, brigadeTypeComboBox4);
            _itemControls.Add(ScenarioEditorItemId.DivisionBrigadeType5, brigadeTypeComboBox5);
            _itemControls.Add(ScenarioEditorItemId.DivisionBrigadeModel1, brigadeModelComboBox1);
            _itemControls.Add(ScenarioEditorItemId.DivisionBrigadeModel2, brigadeModelComboBox2);
            _itemControls.Add(ScenarioEditorItemId.DivisionBrigadeModel3, brigadeModelComboBox3);
            _itemControls.Add(ScenarioEditorItemId.DivisionBrigadeModel4, brigadeModelComboBox4);
            _itemControls.Add(ScenarioEditorItemId.DivisionBrigadeModel5, brigadeModelComboBox5);
            _itemControls.Add(ScenarioEditorItemId.DivisionStrength, strengthTextBox);
            _itemControls.Add(ScenarioEditorItemId.DivisionMaxStrength, maxStrengthTextBox);
            _itemControls.Add(ScenarioEditorItemId.DivisionOrganisation, organisationTextBox);
            _itemControls.Add(ScenarioEditorItemId.DivisionMaxOrganisation, maxOrganisationTextBox);
            _itemControls.Add(ScenarioEditorItemId.DivisionMorale, divisionMoraleTextBox);
            _itemControls.Add(ScenarioEditorItemId.DivisionExperience, experienceTextBox);
            _itemControls.Add(ScenarioEditorItemId.DivisionLocked, lockedCheckBox);
            _itemControls.Add(ScenarioEditorItemId.DivisionDormant, dormantCheckBox);

            divisionTypeTextBox.Tag = ScenarioEditorItemId.DivisionType;
            divisionIdTextBox.Tag = ScenarioEditorItemId.DivisionId;
            divisionNameTextBox.Tag = ScenarioEditorItemId.DivisionName;
            unitTypeComboBox.Tag = ScenarioEditorItemId.DivisionUnitType;
            unitModelComboBox.Tag = ScenarioEditorItemId.DivisionModel;
            brigadeTypeComboBox1.Tag = ScenarioEditorItemId.DivisionBrigadeType1;
            brigadeTypeComboBox2.Tag = ScenarioEditorItemId.DivisionBrigadeType2;
            brigadeTypeComboBox3.Tag = ScenarioEditorItemId.DivisionBrigadeType3;
            brigadeTypeComboBox4.Tag = ScenarioEditorItemId.DivisionBrigadeType4;
            brigadeTypeComboBox5.Tag = ScenarioEditorItemId.DivisionBrigadeType5;
            brigadeModelComboBox1.Tag = ScenarioEditorItemId.DivisionBrigadeModel1;
            brigadeModelComboBox2.Tag = ScenarioEditorItemId.DivisionBrigadeModel2;
            brigadeModelComboBox3.Tag = ScenarioEditorItemId.DivisionBrigadeModel3;
            brigadeModelComboBox4.Tag = ScenarioEditorItemId.DivisionBrigadeModel4;
            brigadeModelComboBox5.Tag = ScenarioEditorItemId.DivisionBrigadeModel5;
            strengthTextBox.Tag = ScenarioEditorItemId.DivisionStrength;
            maxStrengthTextBox.Tag = ScenarioEditorItemId.DivisionMaxStrength;
            organisationTextBox.Tag = ScenarioEditorItemId.DivisionOrganisation;
            maxOrganisationTextBox.Tag = ScenarioEditorItemId.DivisionMaxOrganisation;
            divisionMoraleTextBox.Tag = ScenarioEditorItemId.DivisionMorale;
            experienceTextBox.Tag = ScenarioEditorItemId.DivisionExperience;
            lockedCheckBox.Tag = ScenarioEditorItemId.DivisionLocked;
            dormantCheckBox.Tag = ScenarioEditorItemId.DivisionDormant;
        }

        /// <summary>
        ///     師団情報の編集項目を更新する
        /// </summary>
        /// <param name="division">師団</param>
        private void UpdateOobDivisionItems(Division division)
        {
            _controller.UpdateItemValue(divisionTypeTextBox, division);
            _controller.UpdateItemValue(divisionIdTextBox, division);
            _controller.UpdateItemValue(divisionNameTextBox, division);
            _controller.UpdateItemValue(strengthTextBox, division);
            _controller.UpdateItemValue(maxStrengthTextBox, division);
            _controller.UpdateItemValue(organisationTextBox, division);
            _controller.UpdateItemValue(maxOrganisationTextBox, division);
            _controller.UpdateItemValue(divisionMoraleTextBox, division);
            _controller.UpdateItemValue(experienceTextBox, division);
            _controller.UpdateItemValue(lockedCheckBox, division);
            _controller.UpdateItemValue(dormantCheckBox, division);

            _controller.UpdateItemColor(divisionTypeTextBox, division);
            _controller.UpdateItemColor(divisionIdTextBox, division);
            _controller.UpdateItemColor(divisionNameTextBox, division);
            _controller.UpdateItemColor(strengthTextBox, division);
            _controller.UpdateItemColor(maxStrengthTextBox, division);
            _controller.UpdateItemColor(organisationTextBox, division);
            _controller.UpdateItemColor(maxOrganisationTextBox, division);
            _controller.UpdateItemColor(divisionMoraleTextBox, division);
            _controller.UpdateItemColor(experienceTextBox, division);
            _controller.UpdateItemColor(lockedCheckBox, division);
            _controller.UpdateItemColor(dormantCheckBox, division);

            CountrySettings settings = Scenarios.GetCountrySettings(_selectedCountry);

            // 師団の兵科が変更された場合、リストの選択肢も変更する
            if (division.Branch != _lastDivisionBranch)
            {
                _lastDivisionBranch = division.Branch;
                _controller.UpdateListItems(unitTypeComboBox, division, settings);
                _controller.UpdateListItems(brigadeTypeComboBox1, division, settings);
                _controller.UpdateListItems(brigadeTypeComboBox2, division, settings);
                _controller.UpdateListItems(brigadeTypeComboBox3, division, settings);
                _controller.UpdateListItems(brigadeTypeComboBox4, division, settings);
                _controller.UpdateListItems(brigadeTypeComboBox5, division, settings);
            }

            _controller.UpdateListItems(unitModelComboBox, division, settings);
            _controller.UpdateListItems(brigadeModelComboBox1, division, settings);
            _controller.UpdateListItems(brigadeModelComboBox2, division, settings);
            _controller.UpdateListItems(brigadeModelComboBox3, division, settings);
            _controller.UpdateListItems(brigadeModelComboBox4, division, settings);
            _controller.UpdateListItems(brigadeModelComboBox5, division, settings);

            _controller.UpdateItemValue(unitTypeComboBox, division);
            _controller.UpdateItemValue(brigadeTypeComboBox1, division);
            _controller.UpdateItemValue(brigadeTypeComboBox2, division);
            _controller.UpdateItemValue(brigadeTypeComboBox3, division);
            _controller.UpdateItemValue(brigadeTypeComboBox4, division);
            _controller.UpdateItemValue(brigadeTypeComboBox5, division);
            _controller.UpdateItemValue(unitModelComboBox, division);
            _controller.UpdateItemValue(brigadeModelComboBox1, division);
            _controller.UpdateItemValue(brigadeModelComboBox2, division);
            _controller.UpdateItemValue(brigadeModelComboBox3, division);
            _controller.UpdateItemValue(brigadeModelComboBox4, division);
            _controller.UpdateItemValue(brigadeModelComboBox5, division);

            _controller.UpdateItemColor(unitTypeComboBox, division);
            _controller.UpdateItemColor(brigadeTypeComboBox1, division);
            _controller.UpdateItemColor(brigadeTypeComboBox2, division);
            _controller.UpdateItemColor(brigadeTypeComboBox3, division);
            _controller.UpdateItemColor(brigadeTypeComboBox4, division);
            _controller.UpdateItemColor(brigadeTypeComboBox5, division);
            _controller.UpdateItemColor(unitModelComboBox, division);
            _controller.UpdateItemColor(brigadeModelComboBox1, division);
            _controller.UpdateItemColor(brigadeModelComboBox2, division);
            _controller.UpdateItemColor(brigadeModelComboBox3, division);
            _controller.UpdateItemColor(brigadeModelComboBox4, division);
            _controller.UpdateItemColor(brigadeModelComboBox5, division);
        }

        /// <summary>
        ///     師団情報の編集項目をクリアする
        /// </summary>
        private void ClearOobDivisionItems()
        {
            divisionTypeTextBox.Text = "";
            divisionIdTextBox.Text = "";
            divisionNameTextBox.Text = "";
            unitTypeComboBox.SelectedIndex = -1;
            unitModelComboBox.SelectedIndex = -1;
            brigadeTypeComboBox1.SelectedIndex = -1;
            brigadeTypeComboBox2.SelectedIndex = -1;
            brigadeTypeComboBox3.SelectedIndex = -1;
            brigadeTypeComboBox4.SelectedIndex = -1;
            brigadeTypeComboBox5.SelectedIndex = -1;
            brigadeModelComboBox1.SelectedIndex = -1;
            brigadeModelComboBox2.SelectedIndex = -1;
            brigadeModelComboBox3.SelectedIndex = -1;
            brigadeModelComboBox4.SelectedIndex = -1;
            brigadeModelComboBox5.SelectedIndex = -1;
            strengthTextBox.Text = "";
            maxStrengthTextBox.Text = "";
            organisationTextBox.Text = "";
            maxOrganisationTextBox.Text = "";
            divisionMoraleTextBox.Text = "";
            experienceTextBox.Text = "";
            lockedCheckBox.Checked = false;
            dormantCheckBox.Checked = false;
        }

        /// <summary>
        ///     師団情報の編集項目を有効化する
        /// </summary>
        private void EnableOobDivisionItems()
        {
            divisionGroupBox.Enabled = true;
        }

        /// <summary>
        ///     師団情報の編集項目を無効化する
        /// </summary>
        private void DisableOobDivisionItems()
        {
            divisionGroupBox.Enabled = false;
        }

        #endregion

        #region 初期部隊タブ - 編集項目

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnitIntItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Unit unit = _unitTreeController.GetSelectedUnit();
            if (unit == null)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(_selectedCountry);

            // 文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, unit);
                return;
            }

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, unit);
            if ((prev == null) && (val == 0))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((prev != null) && (val == (int) prev))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, unit);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, unit);

            // 値を更新する
            _controller.SetItemValue(itemId, val, unit);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, unit, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, unit);
        }

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnitDoubleItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Unit unit = _unitTreeController.GetSelectedUnit();
            if (unit == null)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(_selectedCountry);

            // 文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, unit);
                return;
            }

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, settings);
            if ((prev == null) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((prev != null) && DoubleHelper.IsEqual(val, (double) prev))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, unit);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, unit);

            // 値を更新する
            _controller.SetItemValue(itemId, val, unit);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, unit, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, unit);
        }

        /// <summary>
        ///     テキストボックスの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnitStringItemTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Unit unit = _unitTreeController.GetSelectedUnit();
            if (unit == null)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(_selectedCountry);

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, unit);
            string val = control.Text;
            if ((prev == null) && string.IsNullOrEmpty(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (val.Equals(prev))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, unit);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, unit);

            // 値を更新する
            _controller.SetItemValue(itemId, val, unit);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, unit, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, unit);
        }

        /// <summary>
        ///     コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnitComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            Unit unit = _unitTreeController.GetSelectedUnit();
            if (unit == null)
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
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            object val = _controller.GetItemValue(itemId, unit);
            object sel = _controller.GetListItemValue(itemId, e.Index, unit);
            Brush brush = ((int) val == (int) sel) && _controller.IsItemDirty(itemId, unit)
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
        private void OnUnitComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            ComboBox control = (ComboBox) sender;
            int index = control.SelectedIndex;
            if (index < 0)
            {
                return;
            }

            Unit unit = _unitTreeController.GetSelectedUnit();
            if (unit == null)
            {
                return;
            }

            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            CountrySettings settings = Scenarios.GetCountrySettings(_selectedCountry);

            // 初期値から変更されていなければ何もしない
            object val = _controller.GetListItemValue(itemId, index, unit);
            if (val == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            object prev = _controller.GetItemValue(itemId, unit);
            if ((prev != null) && ((int) val == (int) prev))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, unit);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, unit, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, unit);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, unit, settings);

            // 文字色変更のため描画更新する
            control.Refresh();

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, unit);
        }

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDivisionIntItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Division division = _unitTreeController.GetSelectedDivision();
            if (division == null)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(_selectedCountry);

            // 文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, division);
                return;
            }

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, division);
            if ((prev == null) && (val == 0))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((prev != null) && (val == (int) prev))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, division);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, division);

            // 値を更新する
            _controller.SetItemValue(itemId, val, division);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, division, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, division, settings);
        }

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDivisionDoubleItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Division division = _unitTreeController.GetSelectedDivision();
            if (division == null)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(_selectedCountry);

            // 文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, division);
                return;
            }

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, division);
            if ((prev == null) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((prev != null) && DoubleHelper.IsEqual(val, (double) prev))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, division);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, division);

            // 値を更新する
            _controller.SetItemValue(itemId, val, division);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, division, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, division, settings);
        }

        /// <summary>
        ///     テキストボックスの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDivisionStringItemTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Division division = _unitTreeController.GetSelectedDivision();
            if (division == null)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(_selectedCountry);

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, division);
            string val = control.Text;
            if ((prev == null) && string.IsNullOrEmpty(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (val.Equals(prev))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, division);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, division);

            // 値を更新する
            _controller.SetItemValue(itemId, val, division);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, division, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, division, settings);
        }

        /// <summary>
        ///     コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDivisionComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            Division division = _unitTreeController.GetSelectedDivision();
            if (division == null)
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
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            object val = _controller.GetItemValue(itemId, division);
            object sel = _controller.GetListItemValue(itemId, e.Index, division);
            Brush brush = ((int) val == (int) sel) && _controller.IsItemDirty(itemId, division)
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
        private void OnDivisionComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            ComboBox control = (ComboBox) sender;
            int index = control.SelectedIndex;
            if (index < 0)
            {
                return;
            }

            Division division = _unitTreeController.GetSelectedDivision();
            if (division == null)
            {
                return;
            }

            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            CountrySettings settings = Scenarios.GetCountrySettings(_selectedCountry);

            // 初期値から変更されていなければ何もしない
            object val = _controller.GetListItemValue(itemId, index, division);
            if (val == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            object prev = _controller.GetItemValue(itemId, division);
            if ((prev != null) && ((int) val == (int) prev))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, division);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, division, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, division);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, division, settings);

            // 文字色変更のため描画更新する
            control.Refresh();

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, division, settings);
        }

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDivisionComboBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Division division = _unitTreeController.GetSelectedDivision();
            if (division == null)
            {
                return;
            }

            ComboBox control = sender as ComboBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(_selectedCountry);

            // 文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, division);
                return;
            }

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, division);
            if ((prev == null) && (val == 0))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((prev != null) && (val == (int) prev))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, division);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, division);

            // 値を更新する
            _controller.SetItemValue(itemId, val, division);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, division, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, division, settings);
        }

        /// <summary>
        ///     チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDivisionCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Division division = _unitTreeController.GetSelectedDivision();
            if (division == null)
            {
                return;
            }

            CheckBox control = sender as CheckBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(_selectedCountry);

            // 初期値から変更されていなければ何もしない
            bool val = control.Checked;
            object prev = _controller.GetItemValue(itemId, division);
            if ((prev == null) && !val)
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((prev != null) && (val == (bool) prev))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, division);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, division, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, division);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, division, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, division, settings);
        }

        #endregion

        #endregion

        #region 共通

        #region 共通 - 国家

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
                control.Items.Add(Scenarios.GetCountryTagName(country));
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
                brush = new SolidBrush(settings != null
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