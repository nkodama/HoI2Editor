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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScenarioEditorTechnologyPage));
            this.techTreePanel = new System.Windows.Forms.Panel();
            this.techTreePictureBox = new System.Windows.Forms.PictureBox();
            this.inventionsListView = new System.Windows.Forms.ListView();
            this.blueprintsListView = new System.Windows.Forms.ListView();
            this.ownedTechsListView = new System.Windows.Forms.ListView();
            this.ownedTechsLabel = new System.Windows.Forms.Label();
            this.techCategoryListBox = new System.Windows.Forms.ListBox();
            this.blueprintsLabel = new System.Windows.Forms.Label();
            this.inventionsLabel = new System.Windows.Forms.Label();
            this.countryListBox = new System.Windows.Forms.ListBox();
            this.techTreePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.techTreePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // techTreePanel
            // 
            resources.ApplyResources(this.techTreePanel, "techTreePanel");
            this.techTreePanel.Controls.Add(this.techTreePictureBox);
            this.techTreePanel.Name = "techTreePanel";
            // 
            // techTreePictureBox
            // 
            resources.ApplyResources(this.techTreePictureBox, "techTreePictureBox");
            this.techTreePictureBox.Name = "techTreePictureBox";
            this.techTreePictureBox.TabStop = false;
            // 
            // inventionsListView
            // 
            resources.ApplyResources(this.inventionsListView, "inventionsListView");
            this.inventionsListView.CheckBoxes = true;
            this.inventionsListView.MultiSelect = false;
            this.inventionsListView.Name = "inventionsListView";
            this.inventionsListView.UseCompatibleStateImageBehavior = false;
            this.inventionsListView.View = System.Windows.Forms.View.List;
            // 
            // blueprintsListView
            // 
            resources.ApplyResources(this.blueprintsListView, "blueprintsListView");
            this.blueprintsListView.CheckBoxes = true;
            this.blueprintsListView.MultiSelect = false;
            this.blueprintsListView.Name = "blueprintsListView";
            this.blueprintsListView.UseCompatibleStateImageBehavior = false;
            this.blueprintsListView.View = System.Windows.Forms.View.List;
            // 
            // ownedTechsListView
            // 
            resources.ApplyResources(this.ownedTechsListView, "ownedTechsListView");
            this.ownedTechsListView.CheckBoxes = true;
            this.ownedTechsListView.MultiSelect = false;
            this.ownedTechsListView.Name = "ownedTechsListView";
            this.ownedTechsListView.UseCompatibleStateImageBehavior = false;
            this.ownedTechsListView.View = System.Windows.Forms.View.List;
            // 
            // ownedTechsLabel
            // 
            resources.ApplyResources(this.ownedTechsLabel, "ownedTechsLabel");
            this.ownedTechsLabel.Name = "ownedTechsLabel";
            // 
            // techCategoryListBox
            // 
            resources.ApplyResources(this.techCategoryListBox, "techCategoryListBox");
            this.techCategoryListBox.FormattingEnabled = true;
            this.techCategoryListBox.Name = "techCategoryListBox";
            this.techCategoryListBox.SelectedIndexChanged += new System.EventHandler(this.OnTechCategoryListBoxSelectedIndexChanged);
            // 
            // blueprintsLabel
            // 
            resources.ApplyResources(this.blueprintsLabel, "blueprintsLabel");
            this.blueprintsLabel.Name = "blueprintsLabel";
            // 
            // inventionsLabel
            // 
            resources.ApplyResources(this.inventionsLabel, "inventionsLabel");
            this.inventionsLabel.Name = "inventionsLabel";
            // 
            // countryListBox
            // 
            resources.ApplyResources(this.countryListBox, "countryListBox");
            this.countryListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.countryListBox.FormattingEnabled = true;
            this.countryListBox.Name = "countryListBox";
            this.countryListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnCountryListBoxDrawItem);
            this.countryListBox.SelectedIndexChanged += new System.EventHandler(this.OnCountryListBoxSelectedIndexChanged);
            // 
            // ScenarioEditorTechnologyPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.techTreePanel);
            this.Controls.Add(this.inventionsListView);
            this.Controls.Add(this.blueprintsListView);
            this.Controls.Add(this.ownedTechsListView);
            this.Controls.Add(this.ownedTechsLabel);
            this.Controls.Add(this.techCategoryListBox);
            this.Controls.Add(this.blueprintsLabel);
            this.Controls.Add(this.inventionsLabel);
            this.Controls.Add(this.countryListBox);
            this.Name = "ScenarioEditorTechnologyPage";
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
        private System.Windows.Forms.ListBox countryListBox;
    }
}
