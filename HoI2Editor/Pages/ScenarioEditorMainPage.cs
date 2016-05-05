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

namespace HoI2Editor.Pages
{
    /// <summary>
    ///     シナリオエディタのメインタブ
    /// </summary>
    [ToolboxItem(false)]
    internal partial class ScenarioEditorMainPage : UserControl
    {
        #region 内部フィールド

        /// <summary>
        ///     シナリオエディタのメインタブのコントローラ
        /// </summary>
        private readonly ScenarioEditorMainController _controller;

        /// <summary>
        ///     編集項目IDとコントロールの関連付け
        /// </summary>
        private readonly Dictionary<ScenarioEditorMainController.ItemId, Control> _itemControls =
            new Dictionary<ScenarioEditorMainController.ItemId, Control>();

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="controller">シナリオエディタのメインタブのコントローラ</param>
        internal ScenarioEditorMainPage(ScenarioEditorMainController controller)
        {
            InitializeComponent();

            _controller = controller;

            // 編集項目を初期化する
            InitScenarioListBox();
            InitScenarioInfoItems();
            InitScenarioOptionItems();
            InitSelectableItems();
        }

        #endregion

        #region 共通

        /// <summary>
        ///     編集項目を有効化する
        /// </summary>
        internal void EnableItems()
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

            // シナリオリストボックスの表示を更新する
            UpdateScenarioListBox();
        }

        /// <summary>
        ///     シナリオリストボックスの表示を更新する
        /// </summary>
        internal void UpdateScenarioListBox()
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
        ///     選択中のシナリオファイル名を取得する
        /// </summary>
        /// <returns></returns>
        private string GetSelectedScenarioFileName()
        {
            if (scenarioListBox.SelectedIndex < 0)
            {
                return string.Empty;
            }

            return scenarioListBox.Items[scenarioListBox.SelectedIndex].ToString();
        }

        /// <summary>
        ///     読み込みボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoadButtonClick(object sender, EventArgs e)
        {
            string fileName = GetSelectedScenarioFileName();
            ScenarioEditorMainController.FileType fileType = saveGamesRadioButton.Checked
                ? ScenarioEditorMainController.FileType.SaveGames
                : ScenarioEditorMainController.FileType.Scenario;
            ScenarioEditorMainController.FolderType folderType = exportRadioButton.Checked
                ? ScenarioEditorMainController.FolderType.Export
                : modRadioButton.Checked
                    ? ScenarioEditorMainController.FolderType.Mod
                    : ScenarioEditorMainController.FolderType.Vanilla;
            _controller.OnLoadButtonClick(fileName, fileType, folderType);
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
            SetItemControl(scenarioNameTextBox, ScenarioEditorMainController.ItemId.ScenarioName);
            SetItemControl(panelImageTextBox, ScenarioEditorMainController.ItemId.ScenarioPanelName);
            SetItemControl(startYearTextBox, ScenarioEditorMainController.ItemId.ScenarioStartYear);
            SetItemControl(startMonthTextBox, ScenarioEditorMainController.ItemId.ScenarioStartMonth);
            SetItemControl(startDayTextBox, ScenarioEditorMainController.ItemId.ScenarioStartDay);
            SetItemControl(endYearTextBox, ScenarioEditorMainController.ItemId.ScenarioEndYear);
            SetItemControl(endMonthTextBox, ScenarioEditorMainController.ItemId.ScenarioEndMonth);
            SetItemControl(endDayTextBox, ScenarioEditorMainController.ItemId.ScenarioEndDay);
            SetItemControl(includeFolderTextBox, ScenarioEditorMainController.ItemId.ScenarioIncludeFolder);
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
            SetItemControl(battleScenarioCheckBox, ScenarioEditorMainController.ItemId.ScenarioBattleScenario);
            SetItemControl(freeCountryCheckBox, ScenarioEditorMainController.ItemId.ScenarioFreeSelection);
            SetItemControl(allowDiplomacyCheckBox, ScenarioEditorMainController.ItemId.ScenarioAllowDiplomacy);
            SetItemControl(allowProductionCheckBox, ScenarioEditorMainController.ItemId.ScenarioAllowProduction);
            SetItemControl(allowTechnologyCheckBox, ScenarioEditorMainController.ItemId.ScenarioAllowTechnology);
            SetItemControl(aiAggressiveComboBox, ScenarioEditorMainController.ItemId.ScenarioAiAggressive);
            SetItemControl(difficultyComboBox, ScenarioEditorMainController.ItemId.ScenarioDifficulty);
            SetItemControl(gameSpeedComboBox, ScenarioEditorMainController.ItemId.ScenarioGameSpeed);
            SetItemControl(aiAggressiveComboBox, ScenarioEditorMainController.ItemId.ScenarioAiAggressive);
            SetItemControl(difficultyComboBox, ScenarioEditorMainController.ItemId.ScenarioDifficulty);
            SetItemControl(gameSpeedComboBox, ScenarioEditorMainController.ItemId.ScenarioGameSpeed);
        }

        #endregion

        #region 国家選択

        /// <summary>
        ///     選択可能国の編集項目を初期化する
        /// </summary>
        private void InitSelectableItems()
        {
            SetItemControl(majorListBox, ScenarioEditorMainController.ItemId.MajorCountries);
            SetItemControl(selectableListBox, ScenarioEditorMainController.ItemId.SelectableCountries);
            SetItemControl(unselectableListBox, ScenarioEditorMainController.ItemId.UnselectableCountries);
            SetItemControl(majorCountryNameKeyTextBox, ScenarioEditorMainController.ItemId.MajorCountryNameKey);
            SetItemControl(majorCountryNameStringTextBox, ScenarioEditorMainController.ItemId.MajorCountryNameString);
            SetItemControl(majorFlagExtTextBox, ScenarioEditorMainController.ItemId.MajorFlagExt);
            SetItemControl(countryDescKeyTextBox, ScenarioEditorMainController.ItemId.MajorCountryDescKey);
            SetItemControl(countryDescStringTextBox, ScenarioEditorMainController.ItemId.MajorCountryDescString);
            SetItemControl(propagandaTextBox, ScenarioEditorMainController.ItemId.MajorPropaganada);
        }

        /// <summary>
        ///     選択可能国の編集項目をクリアする
        /// </summary>
        internal void ClearSelectableItems()
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
        internal void EnableSelectableItems()
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
        internal void DisableSelectableItems()
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
        internal void EnableMajorButtons()
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
        internal void DisableMajorButtons()
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
        ///     主要国リストの項目を追加する
        /// </summary>
        /// <param name="country">対象国</param>
        internal void AddMajorListItem(Country country)
        {
            majorListBox.Items.Add(Countries.GetTagName(country));
        }

        /// <summary>
        ///     主要国リストの項目を削除する
        /// </summary>
        /// <param name="index">削除対象のインデックス</param>
        internal void RemoveMajorListItem(int index)
        {
            majorListBox.Items.RemoveAt(index);
        }

        /// <summary>
        ///     主要国リストの項目を移動する
        /// </summary>
        /// <param name="src">移動元のインデックス</param>
        /// <param name="dest">移動先のインデックス</param>
        internal void MoveMajorListItem(int src, int dest)
        {
            string s = (string) majorListBox.Items[src];
            majorListBox.SelectedIndexChanged -= OnMajorListBoxSelectedIndexChanged;
            majorListBox.Items.RemoveAt(src);
            majorListBox.Items.Insert(dest, s);
            majorListBox.SelectedIndex = dest;
            majorListBox.SelectedIndexChanged += OnMajorListBoxSelectedIndexChanged;
        }

        /// <summary>
        ///     主要国リストの項目を選択する
        /// </summary>
        /// <param name="index">対象のインデックス</param>
        internal void SelectMajorListItem(int index)
        {
            majorListBox.SelectedIndex = index;
        }

        /// <summary>
        ///     選択可能国リストの項目を挿入する
        /// </summary>
        /// <param name="index">挿入位置のインデックス</param>
        /// <param name="country">対象国</param>
        internal void InsertSelectableListItem(int index, Country country)
        {
            selectableListBox.Items.Insert(index, Countries.GetTagName(country));
        }

        /// <summary>
        ///     選択可能国リストの項目を削除する
        /// </summary>
        /// <param name="index">削除対象のインデックス</param>
        internal void RemoveSelectableListItem(int index)
        {
            majorListBox.SelectedIndexChanged -= OnMajorListBoxSelectedIndexChanged;
            selectableListBox.Items.RemoveAt(index);
            majorListBox.SelectedIndexChanged += OnMajorListBoxSelectedIndexChanged;
        }

        /// <summary>
        ///     非選択可能国リストの項目を挿入する
        /// </summary>
        /// <param name="index">挿入位置のインデックス</param>
        /// <param name="country">対象国</param>
        internal void InsertUnselectableListItem(int index, Country country)
        {
            unselectableListBox.Items.Insert(index, Countries.GetTagName(country));
        }

        /// <summary>
        ///     非選択可能国リストの項目を削除する
        /// </summary>
        /// <param name="index">削除対象のインデックス</param>
        internal void RemoveUnselectableListItem(int index)
        {
            unselectableListBox.Items.RemoveAt(index);
        }

        /// <summary>
        ///     選択国リストボックスの項目描画処理
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
            ScenarioEditorMainController.ItemId itemId = (ScenarioEditorMainController.ItemId) control.Tag;

            // 背景を描画する
            e.DrawBackground();

            // 項目を描画する
            Brush brush;
            if ((e.State & DrawItemState.Selected) == 0)
            {
                // 変更ありの項目は文字色を変更する
                bool dirty = _controller.IsDirtyCountry(itemId, e.Index);
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
            _controller.OnSelectedMajorCountryChanged(majorListBox.SelectedIndex);
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
            _controller.MoveUpMajorCountry(majorListBox.SelectedIndex);
        }

        /// <summary>
        ///     主要国の下へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMajorDownButtonClick(object sender, EventArgs e)
        {
            _controller.MoveDownMajorCountry(majorListBox.SelectedIndex);
        }

        /// <summary>
        ///     主要国追加ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMajorAddButtonClick(object sender, EventArgs e)
        {
            _controller.AddMajorCountries(selectableListBox.SelectedIndices.Cast<int>().ToList());
        }

        /// <summary>
        ///     主要国削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMajorRemoveButtonClick(object sender, EventArgs e)
        {
            _controller.RemoveMajorCountry(majorListBox.SelectedIndex);
        }

        /// <summary>
        ///     選択可能国追加ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectableAddButtonClick(object sender, EventArgs e)
        {
            _controller.AddSelectableCountries(unselectableListBox.SelectedIndices.Cast<int>().ToList());
        }

        /// <summary>
        ///     選択可能国削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectableRemoveButtonClick(object sender, EventArgs e)
        {
            _controller.RemoveSelectableCountries(selectableListBox.SelectedIndices.Cast<int>().ToList());
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
        ///     選択中の主要国設定のインデックスを取得する
        /// </summary>
        /// <returns></returns>
        internal int GetSelectedMajorCountryIndex()
        {
            return majorListBox.SelectedIndex;
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
        private void OnItemTextBoxValidated(object sender, EventArgs e)
        {
            _controller.OnItemValueChanged(sender as TextBox);
        }

        /// <summary>
        ///     テキストボックスの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemTextBoxTextChanged(object sender, EventArgs e)
        {
            _controller.OnItemValueChanged(sender as TextBox);
        }

        /// <summary>
        ///     コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemComboBoxDrawItem(object sender, DrawItemEventArgs e)
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
            ScenarioEditorMainController.ItemId itemId = (ScenarioEditorMainController.ItemId) control.Tag;
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
        private void OnItemComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            _controller.OnItemValueChanged(sender as ComboBox);
        }

        /// <summary>
        ///     チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            _controller.OnItemValueChanged(sender as CheckBox);
        }

        /// <summary>
        ///     コントロールと編集項目IDを関連付ける
        /// </summary>
        /// <param name="control"></param>
        /// <param name="itemId"></param>
        private void SetItemControl(Control control, ScenarioEditorMainController.ItemId itemId)
        {
            control.Tag = itemId;
            _itemControls[itemId] = control;
        }

        /// <summary>
        ///     編集項目IDに関連付けられたコントロールを取得する
        /// </summary>
        /// <param name="itemId">編集項目ID</param>
        /// <returns>関連付けられたコントロール</returns>
        internal Control GetItemControl(ScenarioEditorMainController.ItemId itemId)
        {
            return _itemControls[itemId];
        }

        #endregion
    }
}