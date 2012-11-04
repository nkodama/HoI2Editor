using System;
using System.IO;
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
                Config.LoadConfigFiles();
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
            var dialog = new FolderBrowserDialog { SelectedPath = Game.FolderName };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Game.FolderName = dialog.SelectedPath;
                gameFolderTextBox.Text = Game.FolderName;

                if (Game.IsValidFolderName())
                {
                    Config.LoadConfigFiles();
                    ministerButton.Enabled = true;
                }
                else
                {
                    ministerButton.Enabled = false;
                }
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
        /// 閣僚ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterButtonClick(object sender, EventArgs e)
        {
            var form = new MinisterEditorForm();
            form.Show();
        }
    }
}