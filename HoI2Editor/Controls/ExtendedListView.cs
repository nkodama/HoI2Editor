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

        [Category("動作")]
        [DefaultValue(typeof (bool), "false")]
        [Description("ユーザーによるサブ項目の編集が可能になります。")]
        public bool SubItemEdit { get; set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     行の入れ替えをサポートするかどうか
        /// </summary>
        private bool _allowRowReorder;

        /// <summary>
        ///     編集中の行インデックス
        /// </summary>
        private int _editingRowIndex;

        /// <summary>
        ///     編集中の列インデックス
        /// </summary>
        private int _editingColumnIndex;

        #endregion

        #region 公開イベント

        /// <summary>
        ///     行の入れ替え時の処理
        /// </summary>
        [Category("動作")]
        [Description("項目の順番を再変更したときに発生します。")]
        public event EventHandler<RowReorderedEventArgs> RowReordered;

        [Category("動作")]
        [Description("ユーザーが項目の編集を始めたときに発生します。")]
        public event EventHandler<QueryListViewItemEditEventArgs> QueryItemEdit;

        [Category("動作")]
        [Description("ユーザーが項目を編集しようとしているときに発生します。")]
        public event EventHandler<ListViewItemEditEventArgs> BeforeItemEdit;


        [Category("動作")]
        [Description("ユーザーが項目を編集したときに発生します。")]
        public event EventHandler<ListViewItemEditEventArgs> AfterItemEdit;

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
        ///     マウスダブルクリック時の処理
        /// </summary>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            // クリック位置が項目の上でなければ何もしない
            ListViewHitTestInfo ht = HitTest(e.X, e.Y);
            if (ht.SubItem == null)
            {
                return;
            }

            int rowIndex = ht.Item.Index;
            int columnIndex = ht.Item.SubItems.IndexOf(ht.SubItem);

            // 編集項目の種類を問い合わせる
            QueryListViewItemEditEventArgs qe = new QueryListViewItemEditEventArgs(rowIndex, columnIndex);
            QueryItemEdit?.Invoke(this, qe);

            // 編集用のコントロールを表示する
            ShowEditControl(qe);
        }

        /// <summary>
        ///     ドラッグ開始時の処理
        /// </summary>
        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            base.OnItemDrag(e);

            // ドラッグアンドドロップによる項目入れ替えが許可されていなければ何もしない
            if (!AllowRowReorder)
            {
                return;
            }

            // 項目入れ替えは1項目選択時のみ
            if (SelectedItems.Count != 1)
            {
                return;
            }

            // ドラッグアンドドロップを開始する
            DoDragDrop(SelectedItems[0], DragDropEffects.Move);
        }

        /// <summary>
        ///     ドラッグした項目が領域内に移動した時の処理
        /// </summary>
        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);

            // ドラッグアンドドロップによる項目入れ替えが許可されていなければ何もしない
            if (!AllowRowReorder)
            {
                return;
            }

            // ドラッグした項目がリストビューの項目でなければドロップを許可しない
            if (!e.Data.GetDataPresent(typeof (ListViewItem)))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            // ドロップを許可する
            e.Effect = e.AllowedEffect;
        }

        /// <summary>
        ///     ドラッグした項目が領域内で移動した時の処理
        /// </summary>
        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);

            // ドラッグアンドドロップによる項目入れ替えが許可されていなければ何もしない
            if (!AllowRowReorder)
            {
                return;
            }

            // ドラッグした項目がリストビューの項目でなければドロップを許可しない
            if (!e.Data.GetDataPresent(typeof (ListViewItem)))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            // 挿入マークを表示する
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

            // 挿入位置の項目を表示することでスクロールする
            Items[index].EnsureVisible();
        }

        /// <summary>
        ///     ドラッグした項目が領域外へ移動した時の処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDragLeave(EventArgs e)
        {
            base.OnDragLeave(e);

            // ドラッグアンドドロップによる項目入れ替えが許可されていなければ何もしない
            if (!AllowRowReorder)
            {
                return;
            }

            // 挿入マークを非表示にする
            InsertionMark.Index = -1;
        }

        /// <summary>
        ///     項目をドロップしたときの処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDragDrop(DragEventArgs e)
        {
            base.OnDragDrop(e);

            // ドラッグアンドドロップによる項目入れ替えが許可されていなければ何もしない
            if (!AllowRowReorder)
            {
                return;
            }

            // ドラッグした項目がリストビューの項目でなければドロップを許可しない
            if (!e.Data.GetDataPresent(typeof (ListViewItem)))
            {
                return;
            }

            // 挿入位置を計算する
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

        #region 内部メソッド

        /// <summary>
        ///     項目編集用コントロールを表示する
        /// </summary>
        /// <param name="e">項目編集前イベントのパラメータ</param>
        private void ShowEditControl(QueryListViewItemEditEventArgs e)
        {
            // 項目編集なしの場合は何もしない
            if (e.Type == ItemEditType.None)
            {
                return;
            }

            _editingRowIndex = e.RowIndex;
            _editingColumnIndex = e.ColumnIndex;

            ListViewItem item = Items[e.RowIndex];
            ListViewItem.ListViewSubItem subItem = item.SubItems[e.ColumnIndex];

            // 項目編集用コントロールを表示する
            switch (e.Type)
            {
                case ItemEditType.Text:
                    ShowEditTextBox(subItem.Text, new Point(subItem.Bounds.Left, subItem.Bounds.Top),
                        new Size(subItem.Bounds.Width, subItem.Bounds.Height));
                    break;
            }
        }

        /// <summary>
        ///     項目編集用テキストボックスを表示する
        /// </summary>
        /// <param name="text">元の文字列</param>
        /// <param name="location">テキストボックスの位置</param>
        /// <param name="size">テキストボックスのサイズ</param>
        private void ShowEditTextBox(string text, Point location, Size size)
        {
            InlineTextBox textBox = new InlineTextBox(text, location, size, this);
            textBox.FinishEdit += OnTextFinishEdit;
            Controls.Add(textBox);

            // 文字列を全選択する
            textBox.SelectAll();

            // テキストボックスにフォーカスを設定する
            textBox.Focus();
        }

        /// <summary>
        ///     文字列編集時の処理
        /// </summary>
        private void OnTextFinishEdit(object sender, FinishTextEditEventArgs e)
        {
            InlineTextBox textBox = sender as InlineTextBox;
            if (textBox == null)
            {
                return;
            }

            // イベントハンドラを削除する
            textBox.FinishEdit -= OnTextFinishEdit;

            // 編集用テキストボックスを削除する
            Controls.Remove(textBox);

            // キャンセルされれば項目を更新しない
            if (e.Cancel)
            {
                return;
            }

            ListViewItem item = Items[_editingRowIndex];
            ListViewItem.ListViewSubItem subItem = item.SubItems[_editingColumnIndex];

            ListViewItemEditEventArgs ie = new ListViewItemEditEventArgs(_editingRowIndex, _editingColumnIndex, e.Text);
            BeforeItemEdit?.Invoke(this, ie);

            // キャンセルされれば項目を更新しない
            if (ie.Cancel)
            {
                return;
            }

            // 項目の文字列を更新する
            subItem.Text = e.Text;

            AfterItemEdit?.Invoke(this, ie);
        }

        #endregion
    }
}