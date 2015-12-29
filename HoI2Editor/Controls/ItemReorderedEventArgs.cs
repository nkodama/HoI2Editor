using System.ComponentModel;

namespace HoI2Editor.Controls
{
    /// <summary>
    ///     項目並び替えイベントのパラメータ
    /// </summary>
    public class ItemReorderedEventArgs : CancelEventArgs
    {
        #region 公開プロパティ

        /// <summary>
        ///     前の表示位置
        /// </summary>
        public int OldDisplayIndex { get; private set; }

        /// <summary>
        ///     新しい表示位置
        /// </summary>
        public int NewDisplayIndex { get; private set; }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="oldDisplayIndex">前の表示位置</param>
        /// <param name="newDisplayIndex">新しい表示位置</param>
        public ItemReorderedEventArgs(int oldDisplayIndex, int newDisplayIndex)
        {
            OldDisplayIndex = oldDisplayIndex;
            NewDisplayIndex = newDisplayIndex;
        }

        #endregion
    }
}