using System.ComponentModel;
using System.Windows.Forms;

namespace HoI2Editor.Controls
{
    /// <summary>
    ///     行並び替えイベントのパラメータ
    /// </summary>
    public class RowReorderedEventArgs : CancelEventArgs
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

        /// <summary>
        ///     並び替えられる項目
        /// </summary>
        public ListViewItem Item { get; private set; }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="oldDisplayIndex">前の表示位置</param>
        /// <param name="newDisplayIndex">新しい表示位置</param>
        /// <param name="item">並べ替えられる項目</param>
        public RowReorderedEventArgs(int oldDisplayIndex, int newDisplayIndex, ListViewItem item)
        {
            OldDisplayIndex = oldDisplayIndex;
            NewDisplayIndex = newDisplayIndex;
            Item = item;
        }

        #endregion
    }
}