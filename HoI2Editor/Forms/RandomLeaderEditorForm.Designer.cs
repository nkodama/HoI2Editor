namespace HoI2Editor.Forms
{
    partial class RandomLeaderEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RandomLeaderEditorForm));
            this.copyButton = new System.Windows.Forms.Button();
            this.pasteButton = new System.Windows.Forms.Button();
            this.cutButton = new System.Windows.Forms.Button();
            this.replaceGroupBox = new System.Windows.Forms.GroupBox();
            this.withLabel = new System.Windows.Forms.Label();
            this.toLabel = new System.Windows.Forms.Label();
            this.replaceButton = new System.Windows.Forms.Button();
            this.toComboBox = new System.Windows.Forms.ComboBox();
            this.withComboBox = new System.Windows.Forms.ComboBox();
            this.undoButton = new System.Windows.Forms.Button();
            this.optionGroupBox = new System.Windows.Forms.GroupBox();
            this.regexCheckBox = new System.Windows.Forms.CheckBox();
            this.allCountryCheckBox = new System.Windows.Forms.CheckBox();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.closeButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.reloadButton = new System.Windows.Forms.Button();
            this.countryListBox = new System.Windows.Forms.ListBox();
            this.replaceGroupBox.SuspendLayout();
            this.optionGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // copyButton
            // 
            resources.ApplyResources(this.copyButton, "copyButton");
            this.copyButton.Name = "copyButton";
            this.copyButton.UseVisualStyleBackColor = true;
            this.copyButton.Click += new System.EventHandler(this.OnCopyButtonClick);
            // 
            // pasteButton
            // 
            resources.ApplyResources(this.pasteButton, "pasteButton");
            this.pasteButton.Name = "pasteButton";
            this.pasteButton.UseVisualStyleBackColor = true;
            this.pasteButton.Click += new System.EventHandler(this.OnPasteButtonClick);
            // 
            // cutButton
            // 
            resources.ApplyResources(this.cutButton, "cutButton");
            this.cutButton.Name = "cutButton";
            this.cutButton.UseVisualStyleBackColor = true;
            this.cutButton.Click += new System.EventHandler(this.OnCutButtonClick);
            // 
            // replaceGroupBox
            // 
            resources.ApplyResources(this.replaceGroupBox, "replaceGroupBox");
            this.replaceGroupBox.Controls.Add(this.withLabel);
            this.replaceGroupBox.Controls.Add(this.toLabel);
            this.replaceGroupBox.Controls.Add(this.replaceButton);
            this.replaceGroupBox.Controls.Add(this.toComboBox);
            this.replaceGroupBox.Controls.Add(this.withComboBox);
            this.replaceGroupBox.Name = "replaceGroupBox";
            this.replaceGroupBox.TabStop = false;
            // 
            // withLabel
            // 
            resources.ApplyResources(this.withLabel, "withLabel");
            this.withLabel.Name = "withLabel";
            // 
            // toLabel
            // 
            resources.ApplyResources(this.toLabel, "toLabel");
            this.toLabel.Name = "toLabel";
            // 
            // replaceButton
            // 
            resources.ApplyResources(this.replaceButton, "replaceButton");
            this.replaceButton.Name = "replaceButton";
            this.replaceButton.UseVisualStyleBackColor = true;
            this.replaceButton.Click += new System.EventHandler(this.OnReplaceButtonClick);
            // 
            // toComboBox
            // 
            this.toComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.toComboBox, "toComboBox");
            this.toComboBox.Name = "toComboBox";
            // 
            // withComboBox
            // 
            this.withComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.withComboBox, "withComboBox");
            this.withComboBox.Name = "withComboBox";
            // 
            // undoButton
            // 
            resources.ApplyResources(this.undoButton, "undoButton");
            this.undoButton.Name = "undoButton";
            this.undoButton.UseVisualStyleBackColor = true;
            this.undoButton.Click += new System.EventHandler(this.OnUndoButtonClick);
            // 
            // optionGroupBox
            // 
            resources.ApplyResources(this.optionGroupBox, "optionGroupBox");
            this.optionGroupBox.Controls.Add(this.regexCheckBox);
            this.optionGroupBox.Controls.Add(this.allCountryCheckBox);
            this.optionGroupBox.Name = "optionGroupBox";
            this.optionGroupBox.TabStop = false;
            // 
            // regexCheckBox
            // 
            resources.ApplyResources(this.regexCheckBox, "regexCheckBox");
            this.regexCheckBox.Name = "regexCheckBox";
            this.regexCheckBox.UseVisualStyleBackColor = true;
            // 
            // allCountryCheckBox
            // 
            resources.ApplyResources(this.allCountryCheckBox, "allCountryCheckBox");
            this.allCountryCheckBox.Name = "allCountryCheckBox";
            this.allCountryCheckBox.UseVisualStyleBackColor = true;
            // 
            // nameTextBox
            // 
            this.nameTextBox.AcceptsReturn = true;
            resources.ApplyResources(this.nameTextBox, "nameTextBox");
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Validated += new System.EventHandler(this.OnNameTextBoxValidated);
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
            // RandomLeaderEditorForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.copyButton);
            this.Controls.Add(this.pasteButton);
            this.Controls.Add(this.cutButton);
            this.Controls.Add(this.replaceGroupBox);
            this.Controls.Add(this.undoButton);
            this.Controls.Add(this.optionGroupBox);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.reloadButton);
            this.Controls.Add(this.countryListBox);
            this.Name = "RandomLeaderEditorForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnRandomLeaderEditorFormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnRandomLeaderEditorFormClosed);
            this.Load += new System.EventHandler(this.OnRandomLeaderEditorFormLoad);
            this.replaceGroupBox.ResumeLayout(false);
            this.replaceGroupBox.PerformLayout();
            this.optionGroupBox.ResumeLayout(false);
            this.optionGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button copyButton;
        private System.Windows.Forms.Button pasteButton;
        private System.Windows.Forms.Button cutButton;
        private System.Windows.Forms.GroupBox replaceGroupBox;
        private System.Windows.Forms.Label withLabel;
        private System.Windows.Forms.Label toLabel;
        private System.Windows.Forms.Button replaceButton;
        private System.Windows.Forms.ComboBox toComboBox;
        private System.Windows.Forms.ComboBox withComboBox;
        private System.Windows.Forms.Button undoButton;
        private System.Windows.Forms.GroupBox optionGroupBox;
        private System.Windows.Forms.CheckBox regexCheckBox;
        private System.Windows.Forms.CheckBox allCountryCheckBox;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button reloadButton;
        private System.Windows.Forms.ListBox countryListBox;
    }
}