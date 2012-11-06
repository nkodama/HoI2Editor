using System;
using System.Windows.Forms;

namespace HoI2Editor.Forms
{
    /// <summary>
    /// ログ出力フォーム
    /// </summary>
    public partial class LogForm : Form
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LogForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ログを出力する
        /// </summary>
        /// <param name="s">対象文字列</param>
        public void Write(string s)
        {
            if (!Visible)
            {
                logRichTextBox.Clear();
                Show();
            }
            logRichTextBox.AppendText(s);
        }

        /// <summary>
        /// コピーボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCopyButtonClick(object sender, EventArgs e)
        {
            logRichTextBox.SelectAll();
            logRichTextBox.Copy();
            logRichTextBox.DeselectAll();
        }

        /// <summary>
        /// クリアボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClearButtonClick(object sender, EventArgs e)
        {
            logRichTextBox.Clear();
        }

        /// <summary>
        /// 閉じるボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloseButtonClick(object sender, EventArgs e)
        {
            Hide();
        }
    }
}