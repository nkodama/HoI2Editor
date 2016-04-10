using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

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
        public int[] OldDisplayIndices { get; private set; }

        /// <summary>
        ///     新しい表示位置
        /// </summary>
        public int NewDisplayIndex { get; private set; }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="oldDisplayIndices">前の表示位置</param>
        /// <param name="newDisplayIndex">新しい表示位置</param>
        internal ItemReorderedEventArgs(IEnumerable<int> oldDisplayIndices, int newDisplayIndex)
        {
            OldDisplayIndices = oldDisplayIndices.ToArray();
            NewDisplayIndex = newDisplayIndex;
        }

        #endregion
    }
}