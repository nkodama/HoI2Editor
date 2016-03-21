namespace HoI2Editor.Pages
{
    partial class ScenarioEditorTechnologyPage
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
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
            this.techTreePanel = new System.Windows.Forms.Panel();
            this.techTreePictureBox = new System.Windows.Forms.PictureBox();
            this.inventionsListView = new System.Windows.Forms.ListView();
            this.blueprintsListView = new System.Windows.Forms.ListView();
            this.ownedTechsListView = new System.Windows.Forms.ListView();
            this.ownedTechsLabel = new System.Windows.Forms.Label();
            this.techCategoryListBox = new System.Windows.Forms.ListBox();
            this.blueprintsLabel = new System.Windows.Forms.Label();
            this.inventionsLabel = new System.Windows.Forms.Label();
            this.techCountryListBox = new System.Windows.Forms.ListBox();
            this.techTreePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.techTreePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // techTreePanel
            // 
            this.techTreePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.techTreePanel.AutoScroll = true;
            this.techTreePanel.Controls.Add(this.techTreePictureBox);
            this.techTreePanel.Location = new System.Drawing.Point(404, 12);
            this.techTreePanel.Name = "techTreePanel";
            this.techTreePanel.Size = new System.Drawing.Size(560, 535);
            this.techTreePanel.TabIndex = 17;
            // 
            // techTreePictureBox
            // 
            this.techTreePictureBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.techTreePictureBox.Location = new System.Drawing.Point(0, 0);
            this.techTreePictureBox.Name = "techTreePictureBox";
            this.techTreePictureBox.Size = new System.Drawing.Size(0, 0);
            this.techTreePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.techTreePictureBox.TabIndex = 0;
            this.techTreePictureBox.TabStop = false;
            // 
            // inventionsListView
            // 
            this.inventionsListView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.inventionsListView.CheckBoxes = true;
            this.inventionsListView.Enabled = false;
            this.inventionsListView.Location = new System.Drawing.Point(144, 451);
            this.inventionsListView.MultiSelect = false;
            this.inventionsListView.Name = "inventionsListView";
            this.inventionsListView.Size = new System.Drawing.Size(250, 91);
            this.inventionsListView.TabIndex = 16;
            this.inventionsListView.UseCompatibleStateImageBehavior = false;
            this.inventionsListView.View = System.Windows.Forms.View.List;
            // 
            // blueprintsListView
            // 
            this.blueprintsListView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.blueprintsListView.CheckBoxes = true;
            this.blueprintsListView.Enabled = false;
            this.blueprintsListView.Location = new System.Drawing.Point(144, 268);
            this.blueprintsListView.MultiSelect = false;
            this.blueprintsListView.Name = "blueprintsListView";
            this.blueprintsListView.Size = new System.Drawing.Size(250, 161);
            this.blueprintsListView.TabIndex = 14;
            this.blueprintsListView.UseCompatibleStateImageBehavior = false;
            this.blueprintsListView.View = System.Windows.Forms.View.List;
            // 
            // ownedTechsListView
            // 
            this.ownedTechsListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ownedTechsListView.CheckBoxes = true;
            this.ownedTechsListView.Enabled = false;
            this.ownedTechsListView.Location = new System.Drawing.Point(144, 29);
            this.ownedTechsListView.MultiSelect = false;
            this.ownedTechsListView.Name = "ownedTechsListView";
            this.ownedTechsListView.Size = new System.Drawing.Size(250, 217);
            this.ownedTechsListView.TabIndex = 12;
            this.ownedTechsListView.UseCompatibleStateImageBehavior = false;
            this.ownedTechsListView.View = System.Windows.Forms.View.List;
            // 
            // ownedTechsLabel
            // 
            this.ownedTechsLabel.AutoSize = true;
            this.ownedTechsLabel.Enabled = false;
            this.ownedTechsLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ownedTechsLabel.Location = new System.Drawing.Point(142, 14);
            this.ownedTechsLabel.Name = "ownedTechsLabel";
            this.ownedTechsLabel.Size = new System.Drawing.Size(74, 12);
            this.ownedTechsLabel.TabIndex = 11;
            this.ownedTechsLabel.Text = "Owned Techs";
            // 
            // techCategoryListBox
            // 
            this.techCategoryListBox.Enabled = false;
            this.techCategoryListBox.FormattingEnabled = true;
            this.techCategoryListBox.ItemHeight = 12;
            this.techCategoryListBox.Location = new System.Drawing.Point(12, 14);
            this.techCategoryListBox.Name = "techCategoryListBox";
            this.techCategoryListBox.Size = new System.Drawing.Size(120, 112);
            this.techCategoryListBox.TabIndex = 9;
            // 
            // blueprintsLabel
            // 
            this.blueprintsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.blueprintsLabel.AutoSize = true;
            this.blueprintsLabel.Enabled = false;
            this.blueprintsLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.blueprintsLabel.Location = new System.Drawing.Point(142, 253);
            this.blueprintsLabel.Name = "blueprintsLabel";
            this.blueprintsLabel.Size = new System.Drawing.Size(57, 12);
            this.blueprintsLabel.TabIndex = 13;
            this.blueprintsLabel.Text = "Blueprints";
            // 
            // inventionsLabel
            // 
            this.inventionsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.inventionsLabel.AutoSize = true;
            this.inventionsLabel.Enabled = false;
            this.inventionsLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.inventionsLabel.Location = new System.Drawing.Point(142, 436);
            this.inventionsLabel.Name = "inventionsLabel";
            this.inventionsLabel.Size = new System.Drawing.Size(57, 12);
            this.inventionsLabel.TabIndex = 15;
            this.inventionsLabel.Text = "Inventions";
            // 
            // techCountryListBox
            // 
            this.techCountryListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.techCountryListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.techCountryListBox.Enabled = false;
            this.techCountryListBox.Font = new System.Drawing.Font("Lucida Console", 9F);
            this.techCountryListBox.FormattingEnabled = true;
            this.techCountryListBox.Location = new System.Drawing.Point(12, 143);
            this.techCountryListBox.Name = "techCountryListBox";
            this.techCountryListBox.Size = new System.Drawing.Size(120, 394);
            this.techCountryListBox.TabIndex = 10;
            // 
            // ScenarioTechnologyPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.techTreePanel);
            this.Controls.Add(this.inventionsListView);
            this.Controls.Add(this.blueprintsListView);
            this.Controls.Add(this.ownedTechsListView);
            this.Controls.Add(this.ownedTechsLabel);
            this.Controls.Add(this.techCategoryListBox);
            this.Controls.Add(this.blueprintsLabel);
            this.Controls.Add(this.inventionsLabel);
            this.Controls.Add(this.techCountryListBox);
            this.Name = "ScenarioEditorTechnologyPage";
            this.Size = new System.Drawing.Size(976, 559);
            this.techTreePanel.ResumeLayout(false);
            this.techTreePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.techTreePictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel techTreePanel;
        private System.Windows.Forms.PictureBox techTreePictureBox;
        private System.Windows.Forms.ListView inventionsListView;
        private System.Windows.Forms.ListView blueprintsListView;
        private System.Windows.Forms.ListView ownedTechsListView;
        private System.Windows.Forms.Label ownedTechsLabel;
        private System.Windows.Forms.ListBox techCategoryListBox;
        private System.Windows.Forms.Label blueprintsLabel;
        private System.Windows.Forms.Label inventionsLabel;
        private System.Windows.Forms.ListBox techCountryListBox;
    }
}
