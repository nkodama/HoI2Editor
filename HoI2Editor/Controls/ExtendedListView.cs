using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
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
        ///     選択中の項目のインデックス
        /// </summary>
        public int SelectedIndex
        {
            get { return SelectedIndices.Count > 0 ? SelectedIndices[0] : -1; }
            set
            {
                foreach (int index in SelectedIndices.Cast<int>().Where(index => index != value))
                {
                    Items[index].Selected = false;
                    Items[index].Focused = false;
                }
                if ((value < 0) || (value >= Items.Count))
                {
                    return;
                }
                Items[value].Selected = true;
                Items[value].Focused = true;
            }
        }

        /// <summary>
        ///     選択中の項目
        /// </summary>
        public ListViewItem SelectedItem => SelectedItems.Count > 0 ? SelectedItems[0] : null;

        /// <summary>
        ///     項目の入れ替えをサポートするかどうか
        /// </summary>
        [Category("動作")]
        [DefaultValue(typeof(bool), "false")]
        [Description("ユーザーが項目の順番を再変更できるかどうかを示します。")]
        public bool AllowItemReorder
        {
            get { return _allowItemReorder; }
            set
            {
                _allowItemReorder = value;
                AllowDrop = value;
            }
        }

        [Category("動作")]
        [DefaultValue(typeof(bool), "false")]
        [Description("ユーザーによる項目の編集が可能になります。")]
        public bool ItemEdit { get; set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     行の入れ替えをサポートするかどうか
        /// </summary>
        private bool _allowItemReorder;

        /// <summary>
        ///     ドラッグアンドドロップ中の項目のインデックス
        /// </summary>
        private static readonly List<int> DragIndices = new List<int>();

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
        ///     項目入れ替え時の処理
        /// </summary>
        [Category("動作")]
        [Description("項目の順番を再変更したときに発生します。")]
        public event EventHandler<ItemReorderedEventArgs> ItemReordered;

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
        internal ExtendedListView()
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
            if (!AllowItemReorder)
            {
                return;
            }

            // 選択項目がなければ何もしない
            if (SelectedItems.Count == 0)
            {
                return;
            }

            // 項目のインデックスを保存する
            DragIndices.AddRange(SelectedIndices.Cast<int>());

            // ドラッグアンドドロップを開始する
            DoDragDrop(this, DragDropEffects.Move);
        }

        /// <summary>
        ///     ドラッグした項目が領域内に移動した時の処理
        /// </summary>
        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);

            // ドラッグアンドドロップによる項目入れ替えが許可されていなければ何もしない
            if (!AllowItemReorder)
            {
                return;
            }

            // ExtendedListViewの項目でなければドロップを許可しない
            if (!e.Data.GetDataPresent(typeof(ExtendedListView)))
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
            if (!AllowItemReorder)
            {
                return;
            }

            // ExtendedListViewの項目でなければドロップを許可しない
            if (!e.Data.GetDataPresent(typeof(ExtendedListView)))
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
            InsertionMark.AppearsAfterItem = p.Y > bounds.Top + bounds.Height / 2;
            InsertionMark.Index = index;

            // 挿入位置の項目を表示することで自動スクロールする
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
            if (!AllowItemReorder)
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
            if (!AllowItemReorder)
            {
                return;
            }

            // 自分自身の項目でなければドロップを許可しない
            ExtendedListView listView = e.Data.GetData(typeof(ExtendedListView)) as ExtendedListView;
            if (listView != this)
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

            // イベントハンドラを呼び出す
            ItemReorderedEventArgs re = new ItemReorderedEventArgs(DragIndices, index);
            ItemReordered?.Invoke(this, re);
            if (re.Cancel)
            {
                // ドラッグ状態を解除する
                DragIndices.Clear();
                return;
            }

            // リストビューの項目を移動する
            ListViewItem firstItem = null;
            foreach (int dragIndex in DragIndices)
            {
                ListViewItem item = (ListViewItem) Items[dragIndex].Clone();
                if (firstItem == null)
                {
                    firstItem = item;
                }
                Items.Insert(index, item);
                if (index < dragIndex)
                {
                    Items.RemoveAt(dragIndex + 1);
                    index++;
                }
                else
                {
                    Items.RemoveAt(dragIndex);
                }
            }

            // 移動先の項目を選択する
            if (firstItem != null)
            {
                firstItem.Selected = true;
                firstItem.Focused = true;
                EnsureVisible(firstItem.Index);
            }

            // ドラッグ状態を解除する
            DragIndices.Clear();
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

            _editingRowIndex = e.Row;
            _editingColumnIndex = e.Column;

            ListViewItem item = Items[e.Row];
            ListViewItem.ListViewSubItem subItem = item.SubItems[e.Column];

            // 項目編集用コントロールを表示する
            switch (e.Type)
            {
                case ItemEditType.Bool:
                    // 編集用コントロールを表示せず真偽値を反転させる
                    InvertFlag(e.Flag);
                    break;

                case ItemEditType.Text:
                    ShowEditTextBox(e.Text, new Point(subItem.Bounds.Left, subItem.Bounds.Top),
                        new Size(Columns[e.Column].Width, subItem.Bounds.Height));
                    break;

                case ItemEditType.List:
                    ShowEditComboBox(e.Items, e.Index, new Point(subItem.Bounds.Left, subItem.Bounds.Top),
                        new Size(Columns[e.Column].Width, subItem.Bounds.Height), e.DropDownWidth);
                    break;
            }
        }

        /// <summary>
        ///     項目の真偽値を反転させる
        /// </summary>
        /// <param name="flag">初期真偽値</param>
        private void InvertFlag(bool flag)
        {
            ListViewItemEditEventArgs ie = new ListViewItemEditEventArgs(_editingRowIndex, _editingColumnIndex, !flag);
            BeforeItemEdit?.Invoke(this, ie);

            // キャンセルされれば項目を更新しない
            if (ie.Cancel)
            {
                return;
            }

            AfterItemEdit?.Invoke(this, ie);
        }

        /// <summary>
        ///     項目編集用テキストボックスを表示する
        /// </summary>
        /// <param name="text">初期文字列</param>
        /// <param name="location">テキストボックスの位置</param>
        /// <param name="size">テキストボックスのサイズ</param>
        private void ShowEditTextBox(string text, Point location, Size size)
        {
            InlineTextBox textBox = new InlineTextBox(text, location, size, this);
            textBox.FinishEdit += OnTextFinishEdit;
            Controls.Add(textBox);
        }

        /// <summary>
        ///     項目編集用コンボボックスを表示する
        /// </summary>
        /// <param name="items">項目リスト</param>
        /// <param name="index">初期インデックス</param>
        /// <param name="location">コンボボックスの位置</param>
        /// <param name="size">コンボボックスのサイズ</param>
        /// <param name="dropDownWidth">ドロップダウンリストの幅</param>
        private void ShowEditComboBox(IEnumerable<string> items, int index, Point location, Size size, int dropDownWidth)
        {
            InlineComboBox comboBox = new InlineComboBox(items, index, location, size, dropDownWidth, this);
            comboBox.FinishEdit += OnListFinishEdit;
            Controls.Add(comboBox);
        }

        /// <summary>
        ///     文字列編集時の処理
        /// </summary>
        private void OnTextFinishEdit(object sender, CancelEventArgs e)
        {
            InlineTextBox textBox = sender as InlineTextBox;
            if (textBox == null)
            {
                return;
            }
            string text = textBox.Text;

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

            ListViewItemEditEventArgs ie = new ListViewItemEditEventArgs(_editingRowIndex, _editingColumnIndex,
                textBox.Text);
            BeforeItemEdit?.Invoke(this, ie);

            // キャンセルされれば項目を更新しない
            if (ie.Cancel)
            {
                return;
            }

            // 項目の文字列を更新する
            subItem.Text = text;

            AfterItemEdit?.Invoke(this, ie);
        }

        /// <summary>
        ///     リスト編集時の処理
        /// </summary>
        private void OnListFinishEdit(object sender, CancelEventArgs e)
        {
            InlineComboBox comboBox = sender as InlineComboBox;
            if (comboBox == null)
            {
                return;
            }
            string s = comboBox.Text;
            int index = comboBox.SelectedIndex;

            // イベントハンドラを削除する
            comboBox.FinishEdit -= OnListFinishEdit;

            // 編集用コンボボックスを削除する
            Controls.Remove(comboBox);

            // キャンセルされれば項目を更新しない
            if (e.Cancel)
            {
                return;
            }

            ListViewItem item = Items[_editingRowIndex];
            ListViewItem.ListViewSubItem subItem = item.SubItems[_editingColumnIndex];

            ListViewItemEditEventArgs ie = new ListViewItemEditEventArgs(_editingRowIndex, _editingColumnIndex, s, index);
            BeforeItemEdit?.Invoke(this, ie);

            // キャンセルされれば項目を更新しない
            if (ie.Cancel)
            {
                return;
            }

            // 項目の文字列を更新する
            subItem.Text = comboBox.Items[index].ToString();

            AfterItemEdit?.Invoke(this, ie);
        }

        #endregion
    }
}