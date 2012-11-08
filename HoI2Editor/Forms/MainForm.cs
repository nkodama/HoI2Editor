using System;
using System.Windows.Forms;
using HoI2Editor.Models;
using HoI2Editor.Properties;

namespace HoI2Editor.Forms
{
    /// <summary>
    /// メインフォーム
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMainFormLoad(object sender, EventArgs e)
        {
            gameFolderTextBox.Text = Game.FolderName;

            if (Game.IsValidFolderName())
            {
                ministerButton.Enabled = true;
            }
        }

        /// <summary>
        /// 終了ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExitButtonClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// ゲームフォルダ参照ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoadButtonClick(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog
                             {
                                 SelectedPath = gameFolderTextBox.Text,
                                 ShowNewFolderButton = false,
                                 Description = Resources.OpenGameFolderDialogDescription
                             };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Game.FolderName = dialog.SelectedPath;
                gameFolderTextBox.Text = Game.FolderName;

                ministerButton.Enabled = Game.IsValidFolderName();
            }
        }

        /// <summary>
        /// ゲームフォルダ文字列変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGameFolderTextBoxTextChanged(object sender, EventArgs e)
        {
            Game.FolderName = gameFolderTextBox.Text;

            if (Game.IsValidFolderName())
            {
                ministerButton.Enabled = true;
                teamButton.Enabled = true;
            }
            else
            {
                ministerButton.Enabled = false;
                teamButton.Enabled = false;
            }
        }

        /// <summary>
        /// ゲームフォルダ名テキストボックスにドラッグした時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGameFolderTextBoxDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = (e.Data.GetDataPresent(DataFormats.FileDrop)) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        /// <summary>
        /// ゲームフォルダ名テキストボックスにドロップした時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGameFolderTextBoxDragDrop(object sender, DragEventArgs e)
        {
            var fileNames = (string[]) e.Data.GetData(DataFormats.FileDrop, false);
            gameFolderTextBox.Text = fileNames[0];
        }

        /// <summary>
        /// 閣僚ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterButtonClick(object sender, EventArgs e)
        {
            Config.LoadConfigFiles();
            var form = new MinisterEditorForm();
            form.Show();
        }

        /// <summary>
        /// 研究機関押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTeamButtonClick(object sender, EventArgs e)
        {
            Config.LoadConfigFiles();
            var form = new TeamEditorForm();
            form.Show();
        }
    }
}