using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace HoI2Editor.Controls
{
    /// <summary>
    ///     項目編集用コンボボックス
    /// </summary>
    [ToolboxItem(false)]
    public partial class InlineComboBox : ComboBox
    {
        #region 公開イベント

        /// <summary>
        ///     項目編集完了時の処理
        /// </summary>
        [Category("動作")]
        [Description("項目の編集を完了したときに発生します。")]
        public event EventHandler<CancelEventArgs> FinishEdit;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="items">項目リスト</param>
        /// <param name="index">初期インデックス</param>
        /// <param name="location">座標</param>
        /// <param name="size">サイズ</param>
        /// <param name="parent">親コントロール</param>
        /// <param name="dropDownWidth">ドロップダウンリストの幅</param>
        public InlineComboBox(IEnumerable<string> items, int index, Point location, Size size, int dropDownWidth,
            Control parent)
        {
            InitializeComponent();

            Init(items, index, location, size, dropDownWidth, parent);
        }

        /// <summary>
        ///     初期化処理
        /// </summary>
        /// <param name="items">項目リスト</param>
        /// <param name="index">初期インデックス</param>
        /// <param name="location">座標</param>
        /// <param name="size">サイズ</param>
        /// <param name="parent">親コントロール</param>
        /// <param name="dropDownWidth">ドロップダウンリストの幅</param>
        private void Init(IEnumerable<string> items, int index, Point location, Size size, int dropDownWidth,
            Control parent)
        {
            Parent = parent;
            Location = location;
            Size = size;
            foreach (string s in items)
            {
                Items.Add(s);
            }
            SelectedIndex = index;
            DropDownStyle = ComboBoxStyle.DropDownList;
            DropDownWidth = dropDownWidth > size.Width ? dropDownWidth : size.Width;

            // ドロップダウンリストを開く
            DroppedDown = true;
        }

        #endregion

        #region イベントハンドラ

        /// <summary>
        ///     キー押下時の処理
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Escape)
            {
                Finish(true);
            }
        }

        /// <summary>
        ///     フォーカス解除時の処理
        /// </summary>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Finish(true);
        }

        /// <summary>
        ///     選択項目変更時の処理
        /// </summary>
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            Finish(false);
        }

        /// <summary>
        ///     ドロップダウンリストが閉じられた時の処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDropDownClosed(EventArgs e)
        {
            base.OnDropDownClosed(e);

            // 領域外クリックと項目選択を区別する方法が思いつかないので常に更新ありと見なす
            Finish(false);
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
        ///     編集完了時の処理
        /// </summary>
        /// <param name="cancel">キャンセルされたかどうか</param>
        private void Finish(bool cancel)
        {
            CancelEventArgs e = new CancelEventArgs(cancel);
            FinishEdit?.Invoke(this, e);
        }

        #endregion
    }
}