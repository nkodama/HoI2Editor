using System;
using System.IO;
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
        ///     ゲーム種類ラジオボタンの状態を設定する
        /// </summary>
        private void SetGameTypeRadioButton()
        {
            switch (Game.Type)
            {
                case GameType.HeartsOfIron2:
                    hoi2RadioButton.Checked = true;
                    break;

                case GameType.ArsenalOfDemocracy:
                    aodRadioButton.Checked = true;
                    break;

                case GameType.DarkestHour:
                    dhRadioButton.Checked = true;
                    break;
            }
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMainFormLoad(object sender, EventArgs e)
        {
            gameFolderTextBox.Text = Game.FolderName;
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
        ///     ゲーム種類ラジオボタンのチェック状態変化時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGameTypeRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            if (hoi2RadioButton.Checked)
            {
                Game.Type = GameType.HeartsOfIron2;
            }
            else if (aodRadioButton.Checked)
            {
                Game.Type = GameType.ArsenalOfDemocracy;
            }
            else if (dhRadioButton.Checked)
            {
                Game.Type = GameType.DarkestHour;
            }
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
                                 SelectedPath = gameFolderTextBox.Text,
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

            Game.DistinguishGameType();
            SetGameTypeRadioButton();

            leaderButton.Enabled = Game.IsGameFolderActive;
            ministerButton.Enabled = Game.IsGameFolderActive;
            teamButton.Enabled = Game.IsGameFolderActive;
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
            gameFolderTextBox.Text = string.Equals(Path.GetFileName(folderName), "Mods")
                                         ? Path.GetDirectoryName(folderName)
                                         : folderName;
        }

        /// <summary>
        ///     指揮官ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLeaderButtonClick(object sender, EventArgs e)
        {
            Misc.LoadMiscFile();
            Config.LoadConfigFiles();
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
            Misc.LoadMiscFile();
            Config.LoadConfigFiles();
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
            Misc.LoadMiscFile();
            Config.LoadConfigFiles();
            var form = new TeamEditorForm();
            form.Show();
        }
    }
}