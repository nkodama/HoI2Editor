using System;
using System.Collections.Generic;

namespace HoI2Editor.Controls
{
    /// <summary>
    ///     リストビューの項目編集前イベントのパラメータ
    /// </summary>
    public class QueryListViewItemEditEventArgs : EventArgs
    {
        #region 公開プロパティ

        /// <summary>
        ///     リストビュー項目の行インデックス
        /// </summary>
        internal int Row { get; private set; }

        /// <summary>
        ///     リストビュー項目の列インデックス
        /// </summary>
        internal int Column { get; private set; }

        /// <summary>
        ///     項目編集の種類
        /// </summary>
        internal ItemEditType Type { get; set; }

        /// <summary>
        ///     初期真偽値
        /// </summary>
        internal bool Flag { get; set; }

        /// <summary>
        ///     初期文字列
        /// </summary>
        internal string Text { get; set; }

        /// <summary>
        ///     初期インデックス
        /// </summary>
        internal int Index { get; set; }

        /// <summary>
        ///     リスト選択用項目リスト
        /// </summary>
        internal IEnumerable<string> Items { get; set; }

        /// <summary>
        ///     ドロップダウンリストの幅
        /// </summary>
        internal int DropDownWidth { get; set; }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="row">リストビュー項目の行インデックス</param>
        /// <param name="column">リストビュー項目の列インデックス</param>
        internal QueryListViewItemEditEventArgs(int row, int column)
        {
            Row = row;
            Column = column;
        }

        #endregion
    }

    /// <summary>
    ///     項目編集の種類
    /// </summary>
    internal enum ItemEditType
    {
        None, // 編集なし
        Bool, // 真偽値
        Text, // 文字列編集
        List // リスト選択
    }
}