namespace HoI2Editor.Controls
{
    partial class TechTreePanel
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.techTreePictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.techTreePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // techTreePictureBox
            // 
            this.techTreePictureBox.Location = new System.Drawing.Point(0, 0);
            this.techTreePictureBox.Name = "techTreePictureBox";
            this.techTreePictureBox.Size = new System.Drawing.Size(100, 50);
            this.techTreePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.techTreePictureBox.TabIndex = 0;
            this.techTreePictureBox.TabStop = false;
            this.techTreePictureBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnTreePictureBoxDragDrop);
            this.techTreePictureBox.DragOver += new System.Windows.Forms.DragEventHandler(this.OnTreePictureBoxDragOver);
            // 
            // TechTreePanel
            // 
            this.AutoScroll = true;
            ((System.ComponentModel.ISupportInitialize)(this.techTreePictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox techTreePictureBox;


    }
}
