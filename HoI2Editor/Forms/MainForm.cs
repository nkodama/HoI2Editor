using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
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
            FileVersionInfo info = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            char privateVersion;
            if (info.FilePrivatePart > 0 && info.FilePrivatePart <= 26)
            {
                privateVersion = (char) ('`' + info.FilePrivatePart);
            }
            else
            {
                privateVersion = '\0';
            }
            Text = string.Format("Alternative HoI2 Editor Ver {0}.{1}{2}{3}", info.FileMajorPart, info.FileMinorPart,
                                 info.FileBuildPart, privateVersion);
            gameFolderTextBox.Text = Game.FolderName;
            encodingComboBox.SelectedIndex =
                Thread.CurrentThread.CurrentUICulture.Equals(CultureInfo.GetCultureInfo("ja-JP")) ? 1 : 0;
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
            Game.FolderName = gameFolderTextBox.Text;

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
                string folderName = Path.GetDirectoryName(Path.GetFileName(dialog.SelectedPath));
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
        ///     エンコード変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEncodingComboBoxSelectionChangeCommitted(object sender, EventArgs e)
        {
            switch (encodingComboBox.SelectedIndex)
            {
                case 0:
                    Game.CodePage = 1252;
                    break;

                case 1:
                    Game.CodePage = 932;
                    break;
            }
        }

        /// <summary>
        ///     エラーログ出力チェックボックスのチェック状態変化時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLogCheckBoxChekcedChanged(object sender, EventArgs e)
        {
            Log.Enabled = logCheckBox.Checked;
        }

        /// <summary>
        ///     指揮官ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLeaderButtonClick(object sender, EventArgs e)
        {
            LoadCommonFiles();
            var form = new LeaderEditorForm();
            form.Show();
        }

        /// <summary>
        ///     閣僚ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterButtonClick(object sender, EventArgs e)
        {
            LoadCommonFiles();
            var form = new MinisterEditorForm();
            form.Show();
        }

        /// <summary>
        ///     研究機関ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTeamButtonClick(object sender, EventArgs e)
        {
            LoadCommonFiles();
            var form = new TeamEditorForm();
            form.Show();
        }

        /// <summary>
        ///     技術ツリーボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechButtonClick(object sender, EventArgs e)
        {
            LoadCommonFiles();
            var form = new TechEditorForm();
            form.Show();
        }

        /// <summary>
        ///     ユニットモデルボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnitButtonClick(object sender, EventArgs e)
        {
            LoadCommonFiles();
            var form = new UnitEditorForm();
            form.Show();
        }

        /// <summary>
        ///     共通ファイルを読み込む
        /// </summary>
        private static void LoadCommonFiles()
        {
            Misc.LoadMiscFile();
            Config.LoadConfigFiles();
        }
    }
}