using System;

namespace HoI2Editor.Controls
{
    /// <summary>
    ///     リスト編集完了イベントのパラメータ
    /// </summary>
    public class FinishListEditEventArgs : EventArgs
    {
        #region 公開プロパティ

        /// <summary>
        ///     選択項目のインデックス
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        ///     キャンセルされたかどうか
        /// </summary>
        public bool Cancel { get; private set; }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="index">選択項目のインデックス</param>
        /// <param name="cancel">キャンセルされたかどうか</param>
        public FinishListEditEventArgs(int index, bool cancel)
        {
            Index = index;
            Cancel = cancel;
        }

        #endregion
    }
}