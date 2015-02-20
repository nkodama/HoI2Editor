using System;
using System.Windows.Forms;

namespace HoI2Editor.Forms
{
    /// <summary>
    ///     チェック結果出力フォーム
    /// </summary>
    public partial class DataCheckerForm : Form
    {
        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public DataCheckerForm()
        {
            InitializeComponent();
        }

        #endregion

        #region フォーム

        /// <summary>
        ///     クリアボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClearButtonClick(object sender, EventArgs e)
        {
            resultRichTextBox.Clear();
        }

        /// <summary>
        ///     コピーボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCopyButtonClick(object sender, EventArgs e)
        {
            resultRichTextBox.SelectAll();
            resultRichTextBox.Copy();
            resultRichTextBox.DeselectAll();
        }

        /// <summary>
        ///     閉じるボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloseButtonClick(object sender, EventArgs e)
        {
            Hide();
        }

        #endregion

        #region チェック結果出力

        /// <summary>
        ///     チェック結果を出力する
        /// </summary>
        /// <param name="s">対象文字列</param>
        /// <param name="args">パラメータ</param>
        public void Write(string s, params object[] args)
        {
            string t = string.Format(s, args);
            resultRichTextBox.AppendText(t);
        }

        /// <summary>
        ///     チェック結果を出力する
        /// </summary>
        /// <param name="s">対象文字列</param>
        /// <param name="args">パラメータ</param>
        public void WriteLine(string s, params object[] args)
        {
            string t = string.Format(s, args);
            resultRichTextBox.AppendText(t);
            resultRichTextBox.AppendText(Environment.NewLine);
        }

        /// <summary>
        ///     チェック結果を出力する
        /// </summary>
        public void WriteLine()
        {
            resultRichTextBox.AppendText(Environment.NewLine);
        }

        #endregion
    }
}