using System;

namespace HoI2Editor.Controls
{
    /// <summary>
    ///     リストビューの項目編集時イベントのパラメータ
    /// </summary>
    public class ListViewItemEditEventArgs : EventArgs
    {
        #region 公開プロパティ

        /// <summary>
        ///     キャンセルされたかどうか
        /// </summary>
        internal bool Cancel { get; set; }

        /// <summary>
        ///     リストビュー項目の行インデックス
        /// </summary>
        internal int Row { get; private set; }

        /// <summary>
        ///     リストビュー項目の列インデックス
        /// </summary>
        internal int Column { get; private set; }

        /// <summary>
        ///     編集後の真偽値
        /// </summary>
        internal bool Flag { get; private set; }

        /// <summary>
        ///     編集後の文字列
        /// </summary>
        internal string Text { get; private set; }

        /// <summary>
        ///     選択後のインデックス
        /// </summary>
        internal int Index { get; private set; }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="row">リストビュー項目の行インデックス</param>
        /// <param name="column">リストビュー項目の列インデックス</param>
        /// <param name="flag">編集後の真偽値</param>
        internal ListViewItemEditEventArgs(int row, int column, bool flag)
        {
            Row = row;
            Column = column;
            Flag = flag;
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="row">リストビュー項目の行インデックス</param>
        /// <param name="column">リストビュー項目の列インデックス</param>
        /// <param name="text">編集後の文字列</param>
        internal ListViewItemEditEventArgs(int row, int column, string text)
        {
            Row = row;
            Column = column;
            Text = text;
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="row">リストビュー項目の行インデックス</param>
        /// <param name="column">リストビュー項目の列インデックス</param>
        /// <param name="text">編集後の文字列</param>
        /// <param name="index">選択後のインデックス</param>
        internal ListViewItemEditEventArgs(int row, int column, string text, int index)
        {
            Row = row;
            Column = column;
            Text = text;
            Index = index;
        }

        #endregion
    }
}