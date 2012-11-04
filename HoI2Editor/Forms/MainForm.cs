using System;
using System.Windows.Forms;
using HoI2Editor.Models;

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
        /// 読み込みボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoadButtonClick(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog {SelectedPath = gameFolderTextBox.Text, ShowNewFolderButton = false};
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Game.FolderName = dialog.SelectedPath;
                gameFolderTextBox.Text = Game.FolderName;

                ministerButton.Enabled = Game.IsValidFolderName();
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
        /// ゲームフォルダ文字列変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGameFolderTextBoxTextChanged(object sender, EventArgs e)
        {
            Game.FolderName = gameFolderTextBox.Text;

            ministerButton.Enabled = Game.IsValidFolderName();
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
    }
}