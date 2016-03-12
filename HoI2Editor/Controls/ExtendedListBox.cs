using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HoI2Editor.Controls
{
    internal partial class ExtendedListBox : ListBox
    {
        #region 公開プロパティ

        /// <summary>
        ///     項目の入れ替えをサポートするかどうか
        /// </summary>
        [Category("動作")]
        [DefaultValue(typeof (bool), "false")]
        [Description("ユーザーが項目の順番を再変更できるかどうかを示します。")]
        internal bool AllowItemReorder
        {
            get { return _allowItemReorder; }
            set
            {
                _allowItemReorder = value;
                AllowDrop = value;
            }
        }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     行の入れ替えをサポートするかどうか
        /// </summary>
        private bool _allowItemReorder;

        /// <summary>
        ///     ドラッグアンドドロップの開始位置
        /// </summary>
        private static Point _dragPoint = Point.Empty;

        /// <summary>
        ///     ドラッグアンドドロップ中の項目のインデックス
        /// </summary>
        private static readonly List<int> DragIndices = new List<int>();

        #endregion

        #region 公開イベント

        /// <summary>
        ///     項目の入れ替え時の処理
        /// </summary>
        [Category("動作")]
        [Description("項目の順番を再変更したときに発生します。")]
        internal event EventHandler<ItemReorderedEventArgs> ItemReordered;

        #endregion

        #region 初期化

        /// <summary>
        ///     拡張リストボックス
        /// </summary>
        internal ExtendedListBox()
        {
            InitializeComponent();
        }

        #endregion

        #region イベントハンドラ

        /// <summary>
        ///     マウスダウン時の処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            // ドラッグアンドドロップによる項目入れ替えが許可されていなければ何もしない
            if (!AllowItemReorder)
            {
                return;
            }

            // カーソル位置の項目が選択されていなければ何もしない
            int index = IndexFromPoint(e.X, e.Y);
            if (index < 0)
            {
                return;
            }
            if (!SelectedIndices.Contains(index))
            {
                return;
            }

            // 左ボタンダウンでなければドラッグ状態を解除する
            if (e.Button != MouseButtons.Left)
            {
                _dragPoint = Point.Empty;
                DragIndices.Clear();
                return;
            }

            // ドラッグ開始位置を設定する
            _dragPoint = new Point(e.X, e.Y);

            // 項目のインデックスを保存する
            DragIndices.AddRange(SelectedIndices.Cast<int>());
        }

        /// <summary>
        ///     マウスアップ時の処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            // ドラッグアンドドロップによる項目入れ替えが許可されていなければ何もしない
            if (!AllowItemReorder)
            {
                return;
            }

            // ドラッグ状態を解除する
            _dragPoint = Point.Empty;
            DragIndices.Clear();
        }

        /// <summary>
        ///     マウス移動時の処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // ドラッグアンドドロップによる項目入れ替えが許可されていなければ何もしない
            if (!AllowItemReorder)
            {
                return;
            }

            // ドラッグ中でなければ何もしない
            if (_dragPoint == Point.Empty)
            {
                return;
            }

            // ドラッグ判定サイズを超えていなければ何もしない
            Size dragSize = SystemInformation.DragSize;
            Rectangle dragRect = new Rectangle(_dragPoint.X - dragSize.Width / 2, _dragPoint.Y - dragSize.Height / 2,
                dragSize.Width, dragSize.Height);
            if (dragRect.Contains(e.X, e.Y))
            {
                return;
            }

            // ドラッグアンドドロップを開始する
            DoDragDrop(this, DragDropEffects.Move);
        }

        /// <summary>
        ///     ドラッグした項目が領域内に移動した時の処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);

            // ドラッグアンドドロップによる項目入れ替えが許可されていなければ何もしない
            if (!AllowItemReorder)
            {
                return;
            }

            // ExtendedListBoxの項目でなければドロップを許可しない
            if (!e.Data.GetDataPresent(typeof (ExtendedListBox)))
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

            // ExtendedListBoxの項目でなければドロップを許可しない
            if (!e.Data.GetDataPresent(typeof (ExtendedListBox)))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            // 挿入位置に項目がなければドロップを許可しない
            Point p = PointToClient(new Point(e.X, e.Y));
            int index = IndexFromPoint(p);
            if (index < 0)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            // 表示領域の端までドラッグしたら自動スクロールする
            if (index > 0 && p.Y < ItemHeight)
            {
                TopIndex = index - 1;
            }
            else if (index < Items.Count - 1 && p.Y > ClientRectangle.Height - ItemHeight)
            {
                if (TopIndex + ClientRectangle.Height / ItemHeight < Items.Count)
                {
                    TopIndex = TopIndex + 1;
                }
            }

            // 挿入位置の項目がドラッグ対象ならばドロップを許可しない
            if (DragIndices.Contains(index))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            e.Effect = e.AllowedEffect;
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
            ExtendedListBox listBox = e.Data.GetData(typeof (ExtendedListBox)) as ExtendedListBox;
            if (listBox != this)
            {
                return;
            }

            // 挿入位置に項目がなければドロップを許可しない
            Point p = PointToClient(new Point(e.X, e.Y));
            int index = IndexFromPoint(p);
            if (index < 0)
            {
                _dragPoint = Point.Empty;
                DragIndices.Clear();
                return;
            }

            // 挿入位置の項目がドラッグ対象ならばドロップを許可しない
            if (DragIndices.Contains(index))
            {
                return;
            }

            // イベントハンドラを呼び出す
            ItemReorderedEventArgs re = new ItemReorderedEventArgs(DragIndices, index);
            ItemReordered?.Invoke(this, re);
            if (re.Cancel)
            {
                // ドラッグ状態を解除する
                _dragPoint = Point.Empty;
                DragIndices.Clear();
                return;
            }

            // リストボックスの項目を移動する
            foreach (int dragIndex in DragIndices)
            {
                Items.Insert(index, Items[dragIndex]);
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

            // ドラッグ状態を解除する
            _dragPoint = Point.Empty;
            DragIndices.Clear();
        }

        /// <summary>
        ///     描画更新時の処理
        /// </summary>
        /// <param name="pe"></param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        #endregion
    }
}