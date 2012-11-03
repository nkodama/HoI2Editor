namespace HoI2Editor.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ministerButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ministerButton
            // 
            this.ministerButton.Location = new System.Drawing.Point(49, 56);
            this.ministerButton.Name = "ministerButton";
            this.ministerButton.Size = new System.Drawing.Size(75, 23);
            this.ministerButton.TabIndex = 0;
            this.ministerButton.Text = "閣僚";
            this.ministerButton.UseVisualStyleBackColor = true;
            this.ministerButton.Click += new System.EventHandler(this.OnMinisterButtonClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 281);
            this.Controls.Add(this.ministerButton);
            this.Name = "MainForm";
            this.Text = "Aliternative HoI2 Editor";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ministerButton;
    }
}