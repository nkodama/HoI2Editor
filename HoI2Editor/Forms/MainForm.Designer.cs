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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ministerButton = new System.Windows.Forms.Button();
            this.gameFolderLabel = new System.Windows.Forms.Label();
            this.gameFolderTextBox = new System.Windows.Forms.TextBox();
            this.modLabel = new System.Windows.Forms.Label();
            this.modTextBox = new System.Windows.Forms.TextBox();
            this.editGroupBox = new System.Windows.Forms.GroupBox();
            this.techButton = new System.Windows.Forms.Button();
            this.leaderButton = new System.Windows.Forms.Button();
            this.teamButton = new System.Windows.Forms.Button();
            this.gameFolderReferButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.modFolderReferButton = new System.Windows.Forms.Button();
            this.encodingLabel = new System.Windows.Forms.Label();
            this.encodingComboBox = new System.Windows.Forms.ComboBox();
            this.optionGroupBox = new System.Windows.Forms.GroupBox();
            this.logCheckBox = new System.Windows.Forms.CheckBox();
            this.editGroupBox.SuspendLayout();
            this.optionGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ministerButton
            // 
            resources.ApplyResources(this.ministerButton, "ministerButton");
            this.ministerButton.Name = "ministerButton";
            this.ministerButton.UseVisualStyleBackColor = true;
            this.ministerButton.Click += new System.EventHandler(this.OnMinisterButtonClick);
            // 
            // gameFolderLabel
            // 
            resources.ApplyResources(this.gameFolderLabel, "gameFolderLabel");
            this.gameFolderLabel.Name = "gameFolderLabel";
            // 
            // gameFolderTextBox
            // 
            this.gameFolderTextBox.AllowDrop = true;
            resources.ApplyResources(this.gameFolderTextBox, "gameFolderTextBox");
            this.gameFolderTextBox.Name = "gameFolderTextBox";
            this.gameFolderTextBox.TextChanged += new System.EventHandler(this.OnGameFolderTextBoxTextChanged);
            this.gameFolderTextBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnGameFolderTextBoxDragDrop);
            this.gameFolderTextBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnGameFolderTextBoxDragEnter);
            // 
            // modLabel
            // 
            resources.ApplyResources(this.modLabel, "modLabel");
            this.modLabel.Name = "modLabel";
            // 
            // modTextBox
            // 
            this.modTextBox.AllowDrop = true;
            resources.ApplyResources(this.modTextBox, "modTextBox");
            this.modTextBox.Name = "modTextBox";
            this.modTextBox.TextChanged += new System.EventHandler(this.OnModTextBoxTextChanged);
            this.modTextBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnModTextBoxDragDrop);
            this.modTextBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnModTextBoxDragEnter);
            // 
            // editGroupBox
            // 
            this.editGroupBox.Controls.Add(this.techButton);
            this.editGroupBox.Controls.Add(this.leaderButton);
            this.editGroupBox.Controls.Add(this.teamButton);
            this.editGroupBox.Controls.Add(this.ministerButton);
            resources.ApplyResources(this.editGroupBox, "editGroupBox");
            this.editGroupBox.Name = "editGroupBox";
            this.editGroupBox.TabStop = false;
            // 
            // techButton
            // 
            resources.ApplyResources(this.techButton, "techButton");
            this.techButton.Name = "techButton";
            this.techButton.UseVisualStyleBackColor = true;
            this.techButton.Click += new System.EventHandler(this.OnTechButtonClick);
            // 
            // leaderButton
            // 
            resources.ApplyResources(this.leaderButton, "leaderButton");
            this.leaderButton.Name = "leaderButton";
            this.leaderButton.UseVisualStyleBackColor = true;
            this.leaderButton.Click += new System.EventHandler(this.OnLeaderButtonClick);
            // 
            // teamButton
            // 
            resources.ApplyResources(this.teamButton, "teamButton");
            this.teamButton.Name = "teamButton";
            this.teamButton.UseVisualStyleBackColor = true;
            this.teamButton.Click += new System.EventHandler(this.OnTeamButtonClick);
            // 
            // gameFolderReferButton
            // 
            resources.ApplyResources(this.gameFolderReferButton, "gameFolderReferButton");
            this.gameFolderReferButton.Name = "gameFolderReferButton";
            this.gameFolderReferButton.UseVisualStyleBackColor = true;
            this.gameFolderReferButton.Click += new System.EventHandler(this.OnLoadButtonClick);
            // 
            // exitButton
            // 
            resources.ApplyResources(this.exitButton, "exitButton");
            this.exitButton.Name = "exitButton";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.OnExitButtonClick);
            // 
            // modFolderReferButton
            // 
            resources.ApplyResources(this.modFolderReferButton, "modFolderReferButton");
            this.modFolderReferButton.Name = "modFolderReferButton";
            this.modFolderReferButton.UseVisualStyleBackColor = true;
            this.modFolderReferButton.Click += new System.EventHandler(this.OnModFolderReferButtonClick);
            // 
            // encodingLabel
            // 
            resources.ApplyResources(this.encodingLabel, "encodingLabel");
            this.encodingLabel.Name = "encodingLabel";
            // 
            // encodingComboBox
            // 
            this.encodingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.encodingComboBox.FormattingEnabled = true;
            this.encodingComboBox.Items.AddRange(new object[] {
            resources.GetString("encodingComboBox.Items"),
            resources.GetString("encodingComboBox.Items1")});
            resources.ApplyResources(this.encodingComboBox, "encodingComboBox");
            this.encodingComboBox.Name = "encodingComboBox";
            this.encodingComboBox.SelectionChangeCommitted += new System.EventHandler(this.OnEncodingComboBoxSelectionChangeCommitted);
            // 
            // optionGroupBox
            // 
            this.optionGroupBox.Controls.Add(this.logCheckBox);
            this.optionGroupBox.Controls.Add(this.encodingComboBox);
            this.optionGroupBox.Controls.Add(this.encodingLabel);
            resources.ApplyResources(this.optionGroupBox, "optionGroupBox");
            this.optionGroupBox.Name = "optionGroupBox";
            this.optionGroupBox.TabStop = false;
            // 
            // logCheckBox
            // 
            resources.ApplyResources(this.logCheckBox, "logCheckBox");
            this.logCheckBox.Name = "logCheckBox";
            this.logCheckBox.UseVisualStyleBackColor = true;
            this.logCheckBox.CheckedChanged += new System.EventHandler(this.OnLogCheckBoxChekcedChanged);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.optionGroupBox);
            this.Controls.Add(this.modFolderReferButton);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.gameFolderReferButton);
            this.Controls.Add(this.editGroupBox);
            this.Controls.Add(this.modTextBox);
            this.Controls.Add(this.modLabel);
            this.Controls.Add(this.gameFolderTextBox);
            this.Controls.Add(this.gameFolderLabel);
            this.Name = "MainForm";
            this.Load += new System.EventHandler(this.OnMainFormLoad);
            this.editGroupBox.ResumeLayout(false);
            this.optionGroupBox.ResumeLayout(false);
            this.optionGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ministerButton;
        private System.Windows.Forms.Label gameFolderLabel;
        private System.Windows.Forms.TextBox gameFolderTextBox;
        private System.Windows.Forms.Label modLabel;
        private System.Windows.Forms.TextBox modTextBox;
        private System.Windows.Forms.GroupBox editGroupBox;
        private System.Windows.Forms.Button gameFolderReferButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button teamButton;
        private System.Windows.Forms.Button leaderButton;
        private System.Windows.Forms.Button modFolderReferButton;
        private System.Windows.Forms.Label encodingLabel;
        private System.Windows.Forms.ComboBox encodingComboBox;
        private System.Windows.Forms.GroupBox optionGroupBox;
        private System.Windows.Forms.CheckBox logCheckBox;
        private System.Windows.Forms.Button techButton;
    }
}