using HoI2Editor.Forms;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     ログの管理
    /// </summary>
    public static class Log
    {
        /// <summary>
        ///     ログ出力フォーム
        /// </summary>
        private static readonly LogForm Form;

        /// <summary>
        ///     静的コンストラクタ
        /// </summary>
        static Log()
        {
            Form = new LogForm();
        }

        /// <summary>
        ///     ログを出力する
        /// </summary>
        /// <param name="s">対象文字列</param>
        public static void Write(string s)
        {
            Form.Write(s);
        }

        /// <summary>
        ///     ログ出力フォームを表示する
        /// </summary>
        public static void Show()
        {
            Form.Show();
        }

        /// <summary>
        ///     ログ出力フォームを非表示にする
        /// </summary>
        public static void Hide()
        {
            Form.Hide();
        }
    }
}