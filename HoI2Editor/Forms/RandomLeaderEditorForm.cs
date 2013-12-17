using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HoI2Editor.Models;
using HoI2Editor.Properties;
using HoI2Editor.Utilities;

namespace HoI2Editor.Forms
{
    /// <summary>
    ///     ランダム指揮官名エディタのフォーム
    /// </summary>
    public partial class RandomLeaderEditorForm : Form
    {
        #region 内部フィールド

        /// <summary>
        ///     置換元の履歴
        /// </summary>
        private readonly History _toHistory = new History(HistorySize);

        /// <summary>
        ///     置換先の履歴
        /// </summary>
        private readonly History _withHistory = new History(HistorySize);

        #endregion

        #region 内部定数

        /// <summary>
        ///     履歴の最大数
        /// </summary>
        private const int HistorySize = 10;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public RandomLeaderEditorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRandomLeaderEditorFormLoad(object sender, EventArgs e)
        {
            // 国家データを初期化する
            Countries.Init();

            // 文字列定義ファイルを読み込む
            Config.Load();

            // 国家リストボックスを初期化する
            InitCountryListBox();

            // ランダム指揮官名定義ファイルを読み込む
            LoadFile();
        }

        /// <summary>
        ///     フォームクローズ後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRandomLeaderEditorFormClosed(object sender, FormClosedEventArgs e)
        {
            HoI2EditorApplication.OnRandomLeaderEditorFormClosed();
        }

        #endregion

        #region 終了処理

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
        ///     フォームクローズ時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRandomLeaderEditorFormClosing(object sender, FormClosingEventArgs e)
        {
            // 編集済みでなければフォームを閉じる
            if (!RandomLeaders.IsDirty())
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
                    SaveFile();
                    break;
            }
        }

        #endregion

        #region データ処理

        /// <summary>
        ///     再読み込みボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReloadButtonClick(object sender, EventArgs e)
        {
            // ランダム指揮官名定義ファイルの再読み込みを要求する
            RandomLeaders.RequireReload();

            // ランダム指揮官名定義ファイルを読み込む
            LoadFile();
        }

        /// <summary>
        ///     保存ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            SaveFile();
        }

        /// <summary>
        ///     ランダム指揮官名定義ファイルを読み込む
        /// </summary>
        private void LoadFile()
        {
            // ランダム指揮官名定義ファイルを読み込む
            RandomLeaders.Load();

            // ランダム指揮官名リストの表示を更新する
            UpdateNameList();

            // 編集済みフラグがクリアされるため表示を更新する
            countryListBox.Refresh();
        }

        /// <summary>
        ///     ランダム指揮官名定義ファイルを保存する
        /// </summary>
        private void SaveFile()
        {
            // ランダム指揮官名定義ファイルを保存する
            RandomLeaders.Save();

            // 編集済みフラグがクリアされるため表示を更新する
            countryListBox.Refresh();
        }

        #endregion

        #region 国家リストボックス

        /// <summary>
        ///     国家リストボックスを初期化する
        /// </summary>
        private void InitCountryListBox()
        {
            foreach (string s in Countries.Tags
                .Select(country => Countries.Strings[(int) country])
                .Select(name => Config.ExistsKey(name)
                    ? string.Format("{0} {1}", name, Config.GetText(name))
                    : name))
            {
                countryListBox.Items.Add(s);
            }
            countryListBox.SelectedIndex = 0;
        }

        /// <summary>
        ///     国家リストボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryListBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            if ((e.State & DrawItemState.Selected) != DrawItemState.Selected)
            {
                // 変更ありの項目は文字色を変更する
                Country country = Countries.Tags[e.Index];
                brush = RandomLeaders.IsDirty(country)
                    ? new SolidBrush(Color.Red)
                    : new SolidBrush(SystemColors.WindowText);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            string s = countryListBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     国家リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // ランダム指揮官名リストの表示を更新する
            UpdateNameList();
        }

        #endregion

        #region ランダム指揮官名リスト

        /// <summary>
        ///     ランダム指揮官名リストを更新する
        /// </summary>
        private void UpdateNameList()
        {
            nameTextBox.Clear();

            // 選択中の国家がなければ戻る
            if (countryListBox.SelectedIndex < 0)
            {
                return;
            }
            Country country = Countries.Tags[countryListBox.SelectedIndex];

            // ランダム指揮官名を順に追加する
            var sb = new StringBuilder();
            foreach (string name in RandomLeaders.GetNames(country))
            {
                sb.AppendLine(name);
            }

            nameTextBox.Text = sb.ToString();
        }

        /// <summary>
        ///     ランダム指揮官名リスト変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNameTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中の国家がなければ戻る
            if (countryListBox.SelectedIndex < 0)
            {
                return;
            }
            Country country = Countries.Tags[countryListBox.SelectedIndex];

            // ランダム指揮官名リストを更新する
            RandomLeaders.SetNames(nameTextBox.Lines.Where(line => !string.IsNullOrEmpty(line)).ToList(), country);

            // 編集済みフラグが更新されるため表示を更新する
            countryListBox.Refresh();
        }

        #endregion

        #region 編集機能

        /// <summary>
        ///     元に戻すボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUndoButtonClick(object sender, EventArgs e)
        {
            nameTextBox.Undo();
        }

        /// <summary>
        ///     切り取りボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCutButtonClick(object sender, EventArgs e)
        {
            nameTextBox.Cut();
        }

        /// <summary>
        ///     コピーボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCopyButtonClick(object sender, EventArgs e)
        {
            nameTextBox.Copy();
        }

        /// <summary>
        ///     貼り付けボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPasteButtonClick(object sender, EventArgs e)
        {
            nameTextBox.Paste();
        }

        /// <summary>
        ///     置換ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReplaceButtonClick(object sender, EventArgs e)
        {
            string to = toComboBox.Text;
            string with = withComboBox.Text;

            if (allCountryCheckBox.Checked)
            {
                // 全ての国のランダム指揮官名を置換する
                RandomLeaders.ReplaceAll(to, with, regexCheckBox.Checked);
            }
            else
            {
                // 国家リストボックスの選択項目がなければ戻る
                if (countryListBox.SelectedIndex < 0)
                {
                    return;
                }
                Country country = Countries.Tags[countryListBox.SelectedIndex];

                // ランダム指揮官名を置換する
                RandomLeaders.Replace(to, with, country, regexCheckBox.Checked);
            }

            // ランダム指揮官名リストの表示を更新する
            UpdateNameList();

            // 編集済みフラグが更新されるため表示を更新する
            countryListBox.Refresh();

            // 履歴を更新する
            _toHistory.Add(to);
            toComboBox.Items.Clear();
            foreach (string s in _toHistory.Get())
            {
                toComboBox.Items.Add(s);
            }
            _withHistory.Add(with);
            withComboBox.Items.Clear();
            foreach (string s in _withHistory.Get())
            {
                withComboBox.Items.Add(s);
            }
        }

        #endregion
    }
}