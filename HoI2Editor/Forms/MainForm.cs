using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using HoI2Editor.Models;
using HoI2Editor.Properties;
using HoI2Editor.Utilities;

namespace HoI2Editor.Forms
{
    /// <summary>
    ///     メインフォーム
    /// </summary>
    internal partial class MainForm : Form
    {
        #region 内部フィールド

        /// <summary>
        ///     エディタインスタンス
        /// </summary>
        private readonly HoI2EditorInstance _instance;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="instance">エディタインスタンス</param>
        internal MainForm(HoI2EditorInstance instance)
        {
            InitializeComponent();

            _instance = instance;

            // ウィンドウ位置の初期化
            InitPosition();
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMainFormLoad(object sender, EventArgs e)
        {
            // バージョン文字列を更新する
            Text = HoI2EditorController.Version;

            // ログレベルを初期化する
            logLevelComboBox.SelectedIndex = Log.Level;

            // ログ初期化前にゲームの種類が確定している時にはここでログ出力する
            if (Game.Type != GameType.None)
            {
                Game.OutputGameType();
                Game.OutputGameVersion();
            }

            // マップ読み込みの禁止チェックボックスを初期化する
            mapLoadCheckBox.Checked = Maps.ForbidLoad;

            // 初期状態のゲームフォルダ名を設定する
            if (!string.IsNullOrEmpty(Game.FolderName))
            {
                gameFolderTextBox.Text = HoI2EditorController.Settings.Main.GameFolder;
                Log.Error("Game Folder: {0}", HoI2EditorController.Settings.Main.GameFolder);

                if (!string.IsNullOrEmpty(HoI2EditorController.Settings.Main.ModFolder))
                {
                    modTextBox.Text = HoI2EditorController.Settings.Main.ModFolder;
                    Log.Error("MOD Name: {0}", HoI2EditorController.Settings.Main.ModFolder);
                }
                if (!string.IsNullOrEmpty(HoI2EditorController.Settings.Main.ExportFolder))
                {
                    exportFolderTextBox.Text = HoI2EditorController.Settings.Main.ExportFolder;
                    Log.Error("Export Name: {0}", HoI2EditorController.Settings.Main.ExportFolder);
                }

                // 言語リストを更新する
                UpdateLanguage();

                // ゲームフォルダ名が有効でなければデータ編集を無効化する
                if (!Game.IsGameFolderActive)
                {
                    editGroupBox.Enabled = false;
                    return;
                }

                // 他のエディタプロセスで使われていなければ、データ編集を有効化する
                editGroupBox.Enabled = HoI2EditorController.LockMutex(Game.FolderName);
            }
            else
            {
                SetFolderName(Environment.CurrentDirectory);
            }
        }

        #endregion

        #region 終了処理

        /// <summary>
        ///     終了ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExitButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        ///     フォームクローズ時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMainFormClosing(object sender, FormClosingEventArgs e)
        {
            // 編集済みでなければフォームを閉じる
            if (!_instance.IsDirty())
            {
                return;
            }

            // 既に保存をキャンセルしていればフォームを閉じる
            if (_instance.SaveCanceled)
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
                    _instance.Save();
                    break;
            }
        }

        #endregion

        #region ウィンドウ位置

        /// <summary>
        ///     ウィンドウ位置の初期化
        /// </summary>
        private void InitPosition()
        {
            // ウィンドウの位置
            Location = HoI2EditorController.Settings.Main.Location;
            Size = HoI2EditorController.Settings.Main.Size;
        }

        /// <summary>
        ///     フォーム移動時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMainFormMove(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                HoI2EditorController.Settings.Main.Location = Location;
            }
        }

        /// <summary>
        ///     フォームリサイズ時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMainFormResize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                HoI2EditorController.Settings.Main.Size = Size;
            }
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
            _instance.LaunchLeaderEditorForm();
        }

        /// <summary>
        ///     閣僚ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterButtonClick(object sender, EventArgs e)
        {
            _instance.LaunchMinisterEditorForm();
        }

        /// <summary>
        ///     研究機関ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTeamButtonClick(object sender, EventArgs e)
        {
            _instance.LaunchTeamEditorForm();
        }

        /// <summary>
        ///     プロヴィンスボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProvinceButtonClick(object sender, EventArgs e)
        {
            _instance.LaunchProvinceEditorForm();
        }

        /// <summary>
        ///     技術ツリーボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechButtonClick(object sender, EventArgs e)
        {
            _instance.LaunchTechEditorForm();
        }

        /// <summary>
        ///     ユニットモデルボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnitButtonClick(object sender, EventArgs e)
        {
            _instance.LaunchUnitEditorForm();
        }

        /// <summary>
        ///     ゲーム設定ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMiscButtonClick(object sender, EventArgs e)
        {
            _instance.LaunchMiscEditorForm();
        }

        /// <summary>
        ///     軍団名ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCorpsNameButtonClick(object sender, EventArgs e)
        {
            _instance.LaunchCorpsNameEditorForm();
        }

        /// <summary>
        ///     ユニット名ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnitNameButtonClick(object sender, EventArgs e)
        {
            _instance.LaunchUnitNameEditorForm();
        }

        /// <summary>
        ///     モデル名ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnModelNameButtonClick(object sender, EventArgs e)
        {
            _instance.LaunchModelNameEditorForm();
        }

        /// <summary>
        ///     ランダム指揮官名ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRandomLeaderButtonClick(object sender, EventArgs e)
        {
            _instance.LaunchRandomLeaderEditorForm();
        }

        /// <summary>
        ///     研究速度ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResearchButtonClick(object sender, EventArgs e)
        {
            _instance.LaunchResearchViewerForm();
        }

        /// <summary>
        ///     シナリオボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnScenarioButtonClick(object sender, EventArgs e)
        {
            _instance.LaunchScenarioEditorForm();
        }

        #endregion

        #region フォルダ名

        /// <summary>
        ///     ゲームフォルダ参照ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGameFolderBrowseButtonClick(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
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

            // ゲームフォルダ名が有効でなければデータ編集を無効化する
            if (!Game.IsGameFolderActive)
            {
                editGroupBox.Enabled = false;
                return;
            }

            // 他のエディタプロセスで使われていなければ、データ編集を有効化する
            editGroupBox.Enabled = HoI2EditorController.LockMutex(Game.FolderName);
        }

        /// <summary>
        ///     MODフォルダ参照ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnModFolderBrowseButtonClick(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                SelectedPath = Game.IsModActive ? Game.ModFolderName : Game.FolderName,
                ShowNewFolderButton = false,
                Description = Resources.OpenModFolderDialogDescription
            };
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string modName = Path.GetFileName(dialog.SelectedPath);
            string folderName = Path.GetDirectoryName(dialog.SelectedPath);
            if (string.Equals(Path.GetFileName(folderName), Game.ModPathNameDh))
            {
                folderName = Path.GetDirectoryName(folderName);
            }

            // MODのゲームフォルダと保存フォルダのゲームフォルダが一致しない場合は保存フォルダをクリアする
            if (Game.IsExportFolderActive && !string.Equals(folderName, Game.FolderName))
            {
                exportFolderTextBox.Text = "";
            }

            gameFolderTextBox.Text = folderName;
            modTextBox.Text = modName;
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
        ///     保存フォルダ名参照ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExportFolderBrowseButtonClick(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                SelectedPath = Game.IsExportFolderActive ? Game.ExportFolderName : Game.FolderName,
                ShowNewFolderButton = false,
                Description = Resources.OpenExportFolderDialogDescription
            };
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string exportName = Path.GetFileName(dialog.SelectedPath);
            string folderName = Path.GetDirectoryName(dialog.SelectedPath);
            if (string.Equals(Path.GetFileName(folderName), Game.ModPathNameDh))
            {
                folderName = Path.GetDirectoryName(folderName);
            }

            // 保存フォルダのゲームフォルダとMODのゲームフォルダが一致しない場合はMODフォルダをクリアする
            if (Game.IsModActive && !string.Equals(folderName, Game.FolderName))
            {
                modTextBox.Text = "";
            }

            gameFolderTextBox.Text = folderName;
            exportFolderTextBox.Text = exportName;
        }

        /// <summary>
        ///     保存フォルダ名文字列変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExportFolderTextBoxTextChanged(object sender, EventArgs e)
        {
            Game.ExportName = exportFolderTextBox.Text;
        }

        /// <summary>
        ///     ゲームフォルダ名テキストボックスにドラッグした時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGameFolderTextBoxDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        /// <summary>
        ///     ゲームフォルダ名テキストボックスにドロップした時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGameFolderTextBoxDragDrop(object sender, DragEventArgs e)
        {
            string[] fileNames = (string[]) e.Data.GetData(DataFormats.FileDrop, false);
            gameFolderTextBox.Text = fileNames[0];
            modTextBox.Text = "";
            exportFolderTextBox.Text = "";
        }

        /// <summary>
        ///     MOD名テキストボックスにドラッグした時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnModTextBoxDragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            string[] fileNames = (string[]) e.Data.GetData(DataFormats.FileDrop, false);
            string folderName = Path.GetDirectoryName(fileNames[0]);
            if (string.Equals(Path.GetFileName(folderName), Game.ModPathNameDh))
            {
                folderName = Path.GetDirectoryName(folderName);
            }

            // MODのゲームフォルダと保存フォルダのゲームフォルダが一致しない場合はドロップを許可しない
            if (Game.IsExportFolderActive && !string.Equals(folderName, Game.FolderName))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            e.Effect = DragDropEffects.Copy;
        }

        /// <summary>
        ///     MOD名テキストボックスにドロップした時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnModTextBoxDragDrop(object sender, DragEventArgs e)
        {
            string[] fileNames = (string[]) e.Data.GetData(DataFormats.FileDrop, false);
            string modName = Path.GetFileName(fileNames[0]);
            string folderName = Path.GetDirectoryName(fileNames[0]);
            if (string.Equals(Path.GetFileName(folderName), Game.ModPathNameDh))
            {
                folderName = Path.GetDirectoryName(folderName);
            }

            // ゲームフォルダが有効でない時に保存フォルダ名が設定されていればクリアする
            if (!Game.IsGameFolderActive && !string.IsNullOrEmpty(Game.ExportName))
            {
                exportFolderTextBox.Text = "";
            }

            gameFolderTextBox.Text = folderName;
            modTextBox.Text = modName;
        }

        /// <summary>
        ///     保存フォルダ名テキストボックスにドラッグした時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExportFolderTextBoxDragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            string[] fileNames = (string[]) e.Data.GetData(DataFormats.FileDrop, false);
            string folderName = Path.GetDirectoryName(fileNames[0]);
            if (string.Equals(Path.GetFileName(folderName), Game.ModPathNameDh))
            {
                folderName = Path.GetDirectoryName(folderName);
            }

            // 保存フォルダのゲームフォルダとMODのゲームフォルダが一致しない場合はドロップを許可しない
            if (Game.IsModActive && !string.Equals(folderName, Game.FolderName))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            e.Effect = DragDropEffects.Copy;
        }

        /// <summary>
        ///     保存フォルダ名テキストボックスにドロップした時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExportFolderTextBoxDragDrop(object sender, DragEventArgs e)
        {
            string[] fileNames = (string[]) e.Data.GetData(DataFormats.FileDrop, false);
            string exportName = Path.GetFileName(fileNames[0]);
            string folderName = Path.GetDirectoryName(fileNames[0]);
            if (string.Equals(Path.GetFileName(folderName), Game.ModPathNameDh))
            {
                folderName = Path.GetDirectoryName(folderName);
            }

            // ゲームフォルダが有効でない時にMOD名が設定されていればクリアする
            if (!Game.IsGameFolderActive && !string.IsNullOrEmpty(Game.ModName))
            {
                modTextBox.Text = "";
            }

            gameFolderTextBox.Text = folderName;
            exportFolderTextBox.Text = exportName;
        }

        /// <summary>
        ///     メインフォームにドラッグした時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMainFormDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        /// <summary>
        ///     メインフォームにドロップした時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMainFormDragDrop(object sender, DragEventArgs e)
        {
            string[] fileNames = (string[]) e.Data.GetData(DataFormats.FileDrop, false);
            SetFolderName(fileNames[0]);
        }

        /// <summary>
        ///     ゲームフォルダ/MOD名/保存フォルダ名を設定する
        /// </summary>
        /// <param name="folderName">対象フォルダ名</param>
        private void SetFolderName(string folderName)
        {
            if (!IsGameFolder(folderName))
            {
                string modName = Path.GetFileName(folderName);
                string subFolderName = Path.GetDirectoryName(folderName);
                if (string.Equals(Path.GetFileName(subFolderName), Game.ModPathNameDh))
                {
                    subFolderName = Path.GetDirectoryName(subFolderName);
                }
                if (IsGameFolder(subFolderName))
                {
                    gameFolderTextBox.Text = subFolderName;
                    modTextBox.Text = modName;
                    exportFolderTextBox.Text = "";
                    return;
                }
            }
            gameFolderTextBox.Text = folderName;
            modTextBox.Text = "";
            exportFolderTextBox.Text = "";
        }

        /// <summary>
        ///     指定したフォルダがゲームフォルダかどうかを判定する
        /// </summary>
        /// <param name="folderName">対象フォルダ名</param>
        /// <returns>ゲームフォルダならばtrueを返す</returns>
        private static bool IsGameFolder(string folderName)
        {
            if (string.IsNullOrEmpty(folderName))
            {
                return false;
            }
            // Hearts of Iron 2 日本語版
            if (File.Exists(Path.Combine(folderName, "DoomsdayJP.exe")))
            {
                return true;
            }
            // Hearts of Iron 2 英語版
            if (File.Exists(Path.Combine(folderName, "Hoi2.exe")))
            {
                return true;
            }
            // Arsenal of Democracy 日本語版/英語版
            if (File.Exists(Path.Combine(folderName, "AODGame.exe")))
            {
                return true;
            }
            // Darkest Hour 英語版
            if (File.Exists(Path.Combine(folderName, "Darkest Hour.exe")))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     ゲームフォルダ名/MOD名/保存フォルダ名の変更を許可する
        /// </summary>
        internal void EnableFolderChange()
        {
            gameFolderTextBox.Enabled = true;
            gameFolderBrowseButton.Enabled = true;
            modTextBox.Enabled = true;
            modFolderBrowseButton.Enabled = true;
            exportFolderTextBox.Enabled = true;
            exportFolderBrowseButton.Enabled = true;
        }

        /// <summary>
        ///     ゲームフォルダ名/MOD名/保存フォルダ名の変更を禁止する
        /// </summary>
        internal void DisableFolderChange()
        {
            gameFolderTextBox.Enabled = false;
            gameFolderBrowseButton.Enabled = false;
            modTextBox.Enabled = false;
            modFolderBrowseButton.Enabled = false;
            exportFolderTextBox.Enabled = false;
            exportFolderBrowseButton.Enabled = false;
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
                    Game.CodePage = languageComboBox.SelectedIndex == 7 ? 1251 : 1252;
                    break;

                case LanguageMode.PatchedJapanese:
                    // 日本語は932/それ以外は1252
                    Game.CodePage = languageComboBox.SelectedIndex == 0 ? 932 : 1252;
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
        ///     ログレベルコンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLogLevelComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            Log.Level = logLevelComboBox.SelectedIndex;
            Log.Error("[Log] Level: {0}", (TraceLevel) Log.Level);
        }

        /// <summary>
        ///     マップ読み込み禁止チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMapLoadCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            Maps.ForbidLoad = mapLoadCheckBox.Checked;
        }

        #endregion
    }
}