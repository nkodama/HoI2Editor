using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace HoI2Editor.Controls
{
    /// <summary>
    ///     項目編集用テキストボックス
    /// </summary>
    [ToolboxItem(false)]
    public partial class InlineTextBox : TextBox
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
        /// <param name="text">初期文字列</param>
        /// <param name="location">座標</param>
        /// <param name="size">サイズ</param>
        /// <param name="parent">親コントロール</param>
        public InlineTextBox(string text, Point location, Size size, Control parent)
        {
            InitializeComponent();

            Init(text, location, size, parent);
        }

        /// <summary>
        ///     初期化処理
        /// </summary>
        /// <param name="text">初期文字列</param>
        /// <param name="location">座標</param>
        /// <param name="size">サイズ</param>
        /// <param name="parent">親コントロール</param>
        private void Init(string text, Point location, Size size, Control parent)
        {
            Parent = parent;
            Location = location;
            Size = size;
            Text = text;
            Multiline = false;

            // 文字列を全選択する
            SelectAll();

            // フォーカスを設定する
            Focus();
        }

        #endregion

        #region イベントハンドラ

        /// <summary>
        ///     キー押下時の処理
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Enter)
            {
                Finish(false);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Finish(true);
                e.Handled = true;
            }
        }

        /// <summary>
        ///     フォーカス解除時の処理
        /// </summary>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
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