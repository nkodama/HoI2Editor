using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using HoI2Editor.Models;
using HoI2Editor.Properties;

namespace HoI2Editor.Forms
{
    /// <summary>
    ///     メインフォーム
    /// </summary>
    public partial class MainForm : Form
    {
        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMainFormLoad(object sender, EventArgs e)
        {
            // バージョン文字列を更新する
            Text = HoI2Editor.Version;

            // 言語を初期化する
            Config.LangMode = Thread.CurrentThread.CurrentUICulture.Equals(CultureInfo.GetCultureInfo("ja-JP"))
                ? LanguageMode.Japanese
                : LanguageMode.English;

            // 初期状態のゲームフォルダ名を設定する
            gameFolderTextBox.Text = Environment.CurrentDirectory;

            // 言語リストを更新する
            UpdateLanguage();
        }

        /// <summary>
        ///     終了ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExitButtonClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion

        #region 個別エディタ呼び出し

        /// <summary>
        ///     指揮官ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLeaderButtonClick(object sender, EventArgs e)
        {
            HoI2Editor.LaunchLeaderEditorForm();
        }

        /// <summary>
        ///     閣僚ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterButtonClick(object sender, EventArgs e)
        {
            HoI2Editor.LaunchMinisterEditorForm();
        }

        /// <summary>
        ///     研究機関ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTeamButtonClick(object sender, EventArgs e)
        {
            HoI2Editor.LaunchTeamEditorForm();
        }

        /// <summary>
        ///     プロヴィンスボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProvinceButtonClick(object sender, EventArgs e)
        {
            HoI2Editor.LaunchProvinceEditorForm();
        }

        /// <summary>
        ///     技術ツリーボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechButtonClick(object sender, EventArgs e)
        {
            HoI2Editor.LaunchTechEditorForm();
        }

        /// <summary>
        ///     ユニットモデルボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnitButtonClick(object sender, EventArgs e)
        {
            HoI2Editor.LaunchUnitEditorForm();
        }

        /// <summary>
        ///     ゲーム設定ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMiscButtonClick(object sender, EventArgs e)
        {
            HoI2Editor.LaunchMiscEditorForm();
        }

        /// <summary>
        ///     ユニット名ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnitNameButtonClick(object sender, EventArgs e)
        {
            HoI2Editor.LaunchUnitNameEditorForm();
        }

        /// <summary>
        ///     師団名ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDivisionNameButtonClick(object sender, EventArgs e)
        {
            HoI2Editor.LaunchDivisionNameEditorForm();
        }

        /// <summary>
        ///     ランダム指揮官名ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRandomLeaderButtonClick(object sender, EventArgs e)
        {
            HoI2Editor.LaunchRandomLeaderEditorForm();
        }

        #endregion

        #region ゲームフォルダ/MOD名

        /// <summary>
        ///     ゲームフォルダ参照ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoadButtonClick(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog
            {
                SelectedPath = Game.FolderName,
                ShowNewFolderButton = false,
                Description = Resources.OpenGameFolderDialogDescription
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                gameFolderTextBox.Text = dialog.SelectedPath;
            }
        }

        /// <summary>
        ///     ゲームフォルダ名文字列変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGameFolderTextBoxTextChanged(object sender, EventArgs e)
        {
            // ゲームフォルダ名が変更なければ戻る
            if (gameFolderTextBox.Text.Equals(Game.FolderName))
            {
                return;
            }

            // 言語モードを記憶する
            LanguageMode prev = Config.LangMode;

            // ゲームフォルダ名を変更する
            Game.FolderName = gameFolderTextBox.Text;

            // 言語モードが変更されたら言語リストを更新する
            if (Config.LangMode != prev)
            {
                UpdateLanguage();
            }

            // ゲームフォルダ名が有効ならばデータ編集を有効化する
            editGroupBox.Enabled = Game.IsGameFolderActive;
        }

        /// <summary>
        ///     MODフォルダ参照ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnModFolderReferButtonClick(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog
            {
                SelectedPath = Game.IsModActive ? Game.ModFolderName : Game.FolderName,
                ShowNewFolderButton = false,
                Description = Resources.OpenModFolderDialogDescription
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                modTextBox.Text = Path.GetFileName(dialog.SelectedPath);
                string folderName = Path.GetDirectoryName(dialog.SelectedPath);
                gameFolderTextBox.Text = string.Equals(Path.GetFileName(folderName), Game.ModPathNameDh)
                    ? Path.GetDirectoryName(folderName)
                    : folderName;
            }
        }

        /// <summary>
        ///     MOD名文字列変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnModTextBoxTextChanged(object sender, EventArgs e)
        {
            Game.ModName = modTextBox.Text;
        }

        /// <summary>
        ///     ゲームフォルダ名テキストボックスにドラッグした時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGameFolderTextBoxDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = (e.Data.GetDataPresent(DataFormats.FileDrop)) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        /// <summary>
        ///     ゲームフォルダ名テキストボックスにドロップした時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGameFolderTextBoxDragDrop(object sender, DragEventArgs e)
        {
            var fileNames = (string[]) e.Data.GetData(DataFormats.FileDrop, false);
            modTextBox.Text = "";
            gameFolderTextBox.Text = fileNames[0];
        }

        /// <summary>
        ///     MOD名テキストボックスにドラッグした時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnModTextBoxDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = (e.Data.GetDataPresent(DataFormats.FileDrop)) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        /// <summary>
        ///     MOD名テキストボックスにドロップした時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnModTextBoxDragDrop(object sender, DragEventArgs e)
        {
            var fileNames = (string[]) e.Data.GetData(DataFormats.FileDrop, false);
            modTextBox.Text = Path.GetFileName(fileNames[0]);
            string folderName = Path.GetDirectoryName(fileNames[0]);
            gameFolderTextBox.Text = string.Equals(Path.GetFileName(folderName), Game.ModPathNameDh)
                ? Path.GetDirectoryName(folderName)
                : folderName;
        }

        /// <summary>
        /// ゲームフォルダ名/MOD名の変更を許可する
        /// </summary>
        public void EnableFolderChange()
        {
            gameFolderTextBox.Enabled = true;
            gameFolderReferButton.Enabled = true;
            modTextBox.Enabled = true;
            modFolderReferButton.Enabled = true;
        }

        /// <summary>
        /// ゲームフォルダ名/MOD名の変更を禁止する
        /// </summary>
        public void DisableFolderChange()
        {
            gameFolderTextBox.Enabled = false;
            gameFolderReferButton.Enabled = false;
            modTextBox.Enabled = false;
            modFolderReferButton.Enabled = false;
        }

        #endregion

        #region 言語

        /// <summary>
        ///     言語リストを更新する
        /// </summary>
        private void UpdateLanguage()
        {
            // 言語文字列を登録する
            languageComboBox.BeginUpdate();
            languageComboBox.Items.Clear();
            foreach (string s in Config.LanguageStrings[(int) Config.LangMode])
            {
                languageComboBox.Items.Add(s);
            }
            languageComboBox.EndUpdate();
            languageComboBox.SelectedIndex = 0;
        }

        /// <summary>
        ///     言語変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLanguageComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 言語インデックスを更新する
            Config.LangIndex = languageComboBox.SelectedIndex;

            // コードページを設定する
            switch (Config.LangMode)
            {
                case LanguageMode.Japanese:
                    // 日本語は932
                    Game.CodePage = 932;
                    break;

                case LanguageMode.English:
                    // ロシア語は1251/それ以外は1252
                    Game.CodePage = (languageComboBox.SelectedIndex == 7) ? 1251 : 1252;
                    break;

                case LanguageMode.PatchedJapanese:
                    // 日本語は932/それ以外は1252
                    Game.CodePage = (languageComboBox.SelectedIndex == 0) ? 932 : 1252;
                    break;

                case LanguageMode.PatchedKorean:
                    // 韓国語は949
                    Game.CodePage = 949;
                    break;

                case LanguageMode.PatchedTraditionalChinese:
                    // 繁体字中国語は950
                    Game.CodePage = 950;
                    break;

                case LanguageMode.PatchedSimplifiedChinese:
                    // 簡体字中国語は936
                    Game.CodePage = 936;
                    break;
            }
        }

        #endregion

        #region オプション設定

        /// <summary>
        ///     エラーログ出力チェックボックスのチェック状態変化時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLogCheckBoxChekcedChanged(object sender, EventArgs e)
        {
            Log.Enabled = logCheckBox.Checked;
        }

        #endregion
    }
}