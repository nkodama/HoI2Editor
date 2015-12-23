using System;

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
        public int RowIndex { get; private set; }

        /// <summary>
        ///     リストビュー項目の列インデックス
        /// </summary>
        public int ColumnIndex { get; private set; }

        /// <summary>
        ///     項目編集の種類
        /// </summary>
        public ItemEditType Type { get; set; }

        /// <summary>
        ///     項目編集用のデータ
        /// </summary>
        public object Data { get; set; }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="rowIndex">リストビュー項目の行インデックス</param>
        /// <param name="columnIndex">リストビュー項目の列インデックス</param>
        public QueryListViewItemEditEventArgs(int rowIndex, int columnIndex)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
        }

        #endregion
    }

    /// <summary>
    ///     項目編集の種類
    /// </summary>
    public enum ItemEditType
    {
        None, // 編集なし
        Text, // 文字列編集
        Number, // 数値編集
        List // リストから選択
    }
}