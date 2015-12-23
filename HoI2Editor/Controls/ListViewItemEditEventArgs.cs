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
        public bool Cancel { get; set; }

        /// <summary>
        ///     リストビュー項目の行インデックス
        /// </summary>
        public int RowIndex { get; private set; }

        /// <summary>
        ///     リストビュー項目の列インデックス
        /// </summary>
        public int ColumnIndex { get; private set; }

        /// <summary>
        ///     項目編集用のデータ
        /// </summary>
        public object Data { get; private set; }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="rowIndex">リストビュー項目の行インデックス</param>
        /// <param name="columnIndex">リストビュー項目の列インデックス</param>
        /// <param name="data">項目編集用のデータ</param>
        public ListViewItemEditEventArgs(int rowIndex, int columnIndex, object data)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            Data = data;
        }

        #endregion
    }
}