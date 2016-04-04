using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HoI2Editor.Controllers;
using HoI2Editor.Forms;
using HoI2Editor.Models;
using HoI2Editor.Properties;
using HoI2Editor.Utilities;

namespace HoI2Editor.Pages
{
    /// <summary>
    ///     シナリオエディタのメインタブ
    /// </summary>
    internal partial class ScenarioEditorMainPage : UserControl
    {
        #region 内部フィールド

        /// <summary>
        ///     シナリオエディタコントローラ
        /// </summary>
        private readonly ScenarioEditorController _controller;

        /// <summary>
        ///     シナリオエディタのフォーム
        /// </summary>
        private readonly ScenarioEditorForm _form;

        /// <summary>
        ///     主要国以外の選択可能国リスト
        /// </summary>
        private List<Country> _majorFreeCountries;

        /// <summary>
        ///     選択可能国以外の国家リスト
        /// </summary>
        private List<Country> _selectableFreeCountries;

        #endregion

        #region 内部定数

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
        /// <param name="controller">シナリオエディタコントローラ</param>
        /// <param name="form">シナリオエディタのフォーム</param>
        internal ScenarioEditorMainPage(ScenarioEditorController controller, ScenarioEditorForm form)
        {
            InitializeComponent();

            _controller = controller;
            _form = form;

            // 編集項目を初期化する
            InitScenarioListBox();
            InitScenarioInfoItems();
            InitScenarioOptionItems();
            InitSelectableItems();
        }

        /// <summary>
        ///     編集項目を更新する
        /// </summary>
        internal void UpdateItems()
        {
            // 編集項目を更新する
            UpdateScenarioInfoItems();
            UpdateScenarioOptionItems();

            // 選択可能国リストを更新する
            UpdateSelectableList();

            // メインタブの編集項目を有効化する
            EnableMainItems();
        }

        #endregion

        #region 共通

        /// <summary>
        ///     メインタブの編集項目を有効化する
        /// </summary>
        private void EnableMainItems()
        {
            scenarioInfoGroupBox.Enabled = true;
            scenarioOptionGroupBox.Enabled = true;
            countrySelectionGroupBox.Enabled = true;
        }

        #endregion

        #region シナリオ読み込み

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
            if (Scenarios.IsLoaded() && _controller.IsDirty())
            {
                DialogResult result = MessageBox.Show(Resources.ConfirmSaveMessage, Text, MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);
                switch (result)
                {
                    case DialogResult.Cancel:
                        return;

                    case DialogResult.Yes:
                        _controller.Save();
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
            _form.OnFileLoaded();
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

        #region シナリオ情報

        /// <summary>
        ///     シナリオ情報の編集項目を初期化する
        /// </summary>
        private void InitScenarioInfoItems()
        {
            _form._itemControls.Add(ScenarioEditorItemId.ScenarioName, scenarioNameTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ScenarioPanelName, panelImageTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ScenarioStartYear, startYearTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ScenarioStartMonth, startMonthTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ScenarioStartDay, startDayTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ScenarioEndYear, endYearTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ScenarioEndMonth, endMonthTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ScenarioEndDay, endDayTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ScenarioIncludeFolder, includeFolderTextBox);

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
        internal void UpdatePanelImage(string fileName)
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

        #region オプション

        /// <summary>
        ///     シナリオオプションの編集項目を初期化する
        /// </summary>
        private void InitScenarioOptionItems()
        {
            _form._itemControls.Add(ScenarioEditorItemId.ScenarioBattleScenario, battleScenarioCheckBox);
            _form._itemControls.Add(ScenarioEditorItemId.ScenarioFreeSelection, freeCountryCheckBox);
            _form._itemControls.Add(ScenarioEditorItemId.ScenarioAllowDiplomacy, allowDiplomacyCheckBox);
            _form._itemControls.Add(ScenarioEditorItemId.ScenarioAllowProduction, allowProductionCheckBox);
            _form._itemControls.Add(ScenarioEditorItemId.ScenarioAllowTechnology, allowTechnologyCheckBox);
            _form._itemControls.Add(ScenarioEditorItemId.ScenarioAiAggressive, aiAggressiveComboBox);
            _form._itemControls.Add(ScenarioEditorItemId.ScenarioDifficulty, difficultyComboBox);
            _form._itemControls.Add(ScenarioEditorItemId.ScenarioGameSpeed, gameSpeedComboBox);

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

        #region 国家選択

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
            _form._itemControls.Add(ScenarioEditorItemId.MajorCountryNameKey, majorCountryNameKeyTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.MajorCountryNameString, majorCountryNameStringTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.MajorFlagExt, majorFlagExtTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.MajorCountryDescKey, countryDescKeyTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.MajorCountryDescString, countryDescStringTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.MajorPropaganada, propagandaTextBox);

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
        internal void UpdatePropagandaImage(Country country, string fileName)
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

        #region 編集項目

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
    }
}