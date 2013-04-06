using HoI2Editor.Forms;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     ログの管理
    /// </summary>
    public static class Log
    {
        #region 公開プロパティ

        /// <summary>
        ///     ログ出力が有効か
        /// </summary>
        public static bool Enabled { get; set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     ログ出力フォーム
        /// </summary>
        private static readonly LogForm Form;

        #endregion

        #region 初期化

        /// <summary>
        ///     静的コンストラクタ
        /// </summary>
        static Log()
        {
            Form = new LogForm();
        }

        #endregion

        #region ログ出力

        /// <summary>
        ///     ログを出力する
        /// </summary>
        /// <param name="s">対象文字列</param>
        public static void Write(string s)
        {
            if (Enabled)
            {
                Form.Write(s);
            }
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

        #endregion
    }
}