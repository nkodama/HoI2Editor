namespace HoI2Editor.Forms
{
    partial class ModelNameEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModelNameEditorForm));
            this.typeListBox = new System.Windows.Forms.ListBox();
            this.closeButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.reloadButton = new System.Windows.Forms.Button();
            this.countryListBox = new System.Windows.Forms.ListBox();
            this.itemPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // typeListBox
            // 
            resources.ApplyResources(this.typeListBox, "typeListBox");
            this.typeListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.typeListBox.FormattingEnabled = true;
            this.typeListBox.Name = "typeListBox";
            this.typeListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnTypeListBoxDrawItem);
            this.typeListBox.SelectedIndexChanged += new System.EventHandler(this.OnTypeListBoxSelectedIndexChanged);
            // 
            // closeButton
            // 
            resources.ApplyResources(this.closeButton, "closeButton");
            this.closeButton.Name = "closeButton";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.OnCloseButtonClick);
            // 
            // saveButton
            // 
            resources.ApplyResources(this.saveButton, "saveButton");
            this.saveButton.Name = "saveButton";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.OnSaveButtonClick);
            // 
            // reloadButton
            // 
            resources.ApplyResources(this.reloadButton, "reloadButton");
            this.reloadButton.Name = "reloadButton";
            this.reloadButton.UseVisualStyleBackColor = true;
            this.reloadButton.Click += new System.EventHandler(this.OnReloadButtonClick);
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
            // itemPanel
            // 
            resources.ApplyResources(this.itemPanel, "itemPanel");
            this.itemPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.itemPanel.Name = "itemPanel";
            // 
            // ModelNameEditorForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.itemPanel);
            this.Controls.Add(this.typeListBox);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.reloadButton);
            this.Controls.Add(this.countryListBox);
            this.Name = "ModelNameEditorForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnModelNameEditorFormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnModelNameEditorFormClosed);
            this.Load += new System.EventHandler(this.OnModelNameEditorFormLoad);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox typeListBox;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button reloadButton;
        private System.Windows.Forms.ListBox countryListBox;
        private System.Windows.Forms.Panel itemPanel;
    }
}