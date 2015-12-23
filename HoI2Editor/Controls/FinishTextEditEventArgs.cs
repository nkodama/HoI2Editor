using System;

namespace HoI2Editor.Controls
{
    /// <summary>
    ///     文字列編集完了イベントのパラメータ
    /// </summary>
    public class FinishTextEditEventArgs : EventArgs
    {
        #region 公開プロパティ

        /// <summary>
        ///     文字列
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        ///     キャンセルされたかどうか
        /// </summary>
        public bool Cancel { get; private set; }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="text">文字列</param>
        /// <param name="cancel">キャンセルされたかどうか</param>
        public FinishTextEditEventArgs(string text, bool cancel)
        {
            Text = text;
            Cancel = cancel;
        }

        #endregion
    }
}