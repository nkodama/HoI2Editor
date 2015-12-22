using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace HoI2Editor.Controls
{
    /// <summary>
    ///     拡張リストビュー
    /// </summary>
    public partial class ExtendedListView : ListView
    {
        #region 公開プロパティ

        /// <summary>
        ///     行の入れ替えをサポートするかどうか
        /// </summary>
        [Category("動作")]
        [DefaultValue(typeof (bool), "false")]
        [Description("ユーザーが項目の順番を再変更できるかどうかを示します。")]
        public bool AllowRowReorder
        {
            get { return _allowRowReorder; }
            set
            {
                _allowRowReorder = value;
                AllowDrop = value;
            }
        }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     行の入れ替えをサポートするかどうか
        /// </summary>
        private bool _allowRowReorder;

        #endregion

        #region 公開イベント

        /// <summary>
        ///     行の入れ替え時の処理
        /// </summary>
        [Category("動作")]
        [Description("項目の順番を再変更したときに発生します。")]
        public event EventHandler<RowReorderedEventArgs> RowReordered;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public ExtendedListView()
        {
            InitializeComponent();
        }

        #endregion

        #region イベントハンドラ

        /// <summary>
        ///     ドラッグ開始時の処理
        /// </summary>
        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            base.OnItemDrag(e);
            if (!AllowRowReorder)
            {
                return;
            }
            if (SelectedItems.Count != 1)
            {
                return;
            }
            DoDragDrop(SelectedItems[0], DragDropEffects.Move);
        }

        /// <summary>
        ///     ドラッグした項目が領域内に移動した時の処理
        /// </summary>
        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
            if (!AllowRowReorder)
            {
                return;
            }
            if (!e.Data.GetDataPresent(typeof (ListViewItem)))
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            e.Effect = e.AllowedEffect;
        }

        /// <summary>
        ///     ドラッグした項目が領域内で移動した時の処理
        /// </summary>
        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);
            if (!AllowRowReorder)
            {
                return;
            }
            if (!e.Data.GetDataPresent(typeof (ListViewItem)))
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            Point p = PointToClient(new Point(e.X, e.Y));
            int index = InsertionMark.NearestIndex(p);
            if (index < 0)
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            e.Effect = e.AllowedEffect;
            Rectangle bounds = GetItemRect(index);
            InsertionMark.AppearsAfterItem = (p.Y > bounds.Top + bounds.Height / 2);
            InsertionMark.Index = index;
            Items[index].EnsureVisible();
        }

        /// <summary>
        ///     ドラッグした項目が領域外へ移動した時の処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDragLeave(EventArgs e)
        {
            base.OnDragLeave(e);
            if (!AllowRowReorder)
            {
                return;
            }
            InsertionMark.Index = -1;
        }

        /// <summary>
        ///     項目をドロップしたときの処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDragDrop(DragEventArgs e)
        {
            base.OnDragDrop(e);
            if (!AllowRowReorder)
            {
                return;
            }
            if (!e.Data.GetDataPresent(typeof (ListViewItem)))
            {
                return;
            }
            int index = InsertionMark.Index;
            if (index < 0)
            {
                return;
            }
            if (InsertionMark.AppearsAfterItem)
            {
                index++;
            }
            ListViewItem item = (ListViewItem) e.Data.GetData(typeof (ListViewItem));

            // イベントハンドラを呼び出す
            RowReorderedEventArgs re = new RowReorderedEventArgs(item.Index, index, item);
            RowReordered?.Invoke(this, re);
            if (re.Cancel)
            {
                return;
            }

            // リストビューの項目を移動する
            Items.Insert(index, (ListViewItem) item.Clone());
            Items.Remove(item);
        }

        /// <summary>
        ///     描画更新時の処理
        /// </summary>
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        #endregion
    }
}