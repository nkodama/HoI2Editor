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
            this.gameFolderLabel = new System.Windows.Forms.Label();
            this.gameFolderTextBox = new System.Windows.Forms.TextBox();
            this.modLabel = new System.Windows.Forms.Label();
            this.modTextBox = new System.Windows.Forms.TextBox();
            this.gameTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.dhRadioButton = new System.Windows.Forms.RadioButton();
            this.aodRadioButton = new System.Windows.Forms.RadioButton();
            this.hoi2RadioButton = new System.Windows.Forms.RadioButton();
            this.editGroupBox = new System.Windows.Forms.GroupBox();
            this.leaderButton = new System.Windows.Forms.Button();
            this.teamButton = new System.Windows.Forms.Button();
            this.gameFolderReferButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.gameTypeGroupBox.SuspendLayout();
            this.editGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ministerButton
            // 
            this.ministerButton.Enabled = false;
            this.ministerButton.Location = new System.Drawing.Point(88, 18);
            this.ministerButton.Name = "ministerButton";
            this.ministerButton.Size = new System.Drawing.Size(75, 23);
            this.ministerButton.TabIndex = 0;
            this.ministerButton.Text = "閣僚";
            this.ministerButton.UseVisualStyleBackColor = true;
            this.ministerButton.Click += new System.EventHandler(this.OnMinisterButtonClick);
            // 
            // gameFolderLabel
            // 
            this.gameFolderLabel.AutoSize = true;
            this.gameFolderLabel.Location = new System.Drawing.Point(12, 69);
            this.gameFolderLabel.Name = "gameFolderLabel";
            this.gameFolderLabel.Size = new System.Drawing.Size(82, 12);
            this.gameFolderLabel.TabIndex = 1;
            this.gameFolderLabel.Text = "ゲームフォルダ名";
            // 
            // gameFolderTextBox
            // 
            this.gameFolderTextBox.AllowDrop = true;
            this.gameFolderTextBox.Location = new System.Drawing.Point(32, 84);
            this.gameFolderTextBox.Name = "gameFolderTextBox";
            this.gameFolderTextBox.Size = new System.Drawing.Size(339, 19);
            this.gameFolderTextBox.TabIndex = 2;
            this.gameFolderTextBox.TextChanged += new System.EventHandler(this.OnGameFolderTextBoxTextChanged);
            this.gameFolderTextBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnGameFolderTextBoxDragDrop);
            this.gameFolderTextBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnGameFolderTextBoxDragEnter);
            // 
            // modLabel
            // 
            this.modLabel.AutoSize = true;
            this.modLabel.Location = new System.Drawing.Point(12, 116);
            this.modLabel.Name = "modLabel";
            this.modLabel.Size = new System.Drawing.Size(42, 12);
            this.modLabel.TabIndex = 4;
            this.modLabel.Text = "MOD名";
            // 
            // modTextBox
            // 
            this.modTextBox.Location = new System.Drawing.Point(32, 131);
            this.modTextBox.Name = "modTextBox";
            this.modTextBox.Size = new System.Drawing.Size(339, 19);
            this.modTextBox.TabIndex = 5;
            this.modTextBox.TextChanged += new System.EventHandler(this.OnModTextBoxTextChanged);
            // 
            // gameTypeGroupBox
            // 
            this.gameTypeGroupBox.Controls.Add(this.dhRadioButton);
            this.gameTypeGroupBox.Controls.Add(this.aodRadioButton);
            this.gameTypeGroupBox.Controls.Add(this.hoi2RadioButton);
            this.gameTypeGroupBox.Location = new System.Drawing.Point(12, 12);
            this.gameTypeGroupBox.Name = "gameTypeGroupBox";
            this.gameTypeGroupBox.Size = new System.Drawing.Size(204, 44);
            this.gameTypeGroupBox.TabIndex = 6;
            this.gameTypeGroupBox.TabStop = false;
            this.gameTypeGroupBox.Text = "ゲームの種類";
            // 
            // dhRadioButton
            // 
            this.dhRadioButton.AutoSize = true;
            this.dhRadioButton.Location = new System.Drawing.Point(149, 18);
            this.dhRadioButton.Name = "dhRadioButton";
            this.dhRadioButton.Size = new System.Drawing.Size(39, 16);
            this.dhRadioButton.TabIndex = 2;
            this.dhRadioButton.Text = "DH";
            this.dhRadioButton.UseVisualStyleBackColor = true;
            this.dhRadioButton.CheckedChanged += new System.EventHandler(this.OnGameTypeRadioButtonCheckedChanged);
            // 
            // aodRadioButton
            // 
            this.aodRadioButton.AutoSize = true;
            this.aodRadioButton.Checked = true;
            this.aodRadioButton.Location = new System.Drawing.Point(98, 18);
            this.aodRadioButton.Name = "aodRadioButton";
            this.aodRadioButton.Size = new System.Drawing.Size(45, 16);
            this.aodRadioButton.TabIndex = 1;
            this.aodRadioButton.TabStop = true;
            this.aodRadioButton.Text = "AoD";
            this.aodRadioButton.UseVisualStyleBackColor = true;
            this.aodRadioButton.CheckedChanged += new System.EventHandler(this.OnGameTypeRadioButtonCheckedChanged);
            // 
            // hoi2RadioButton
            // 
            this.hoi2RadioButton.AutoSize = true;
            this.hoi2RadioButton.Location = new System.Drawing.Point(18, 18);
            this.hoi2RadioButton.Name = "hoi2RadioButton";
            this.hoi2RadioButton.Size = new System.Drawing.Size(74, 16);
            this.hoi2RadioButton.TabIndex = 0;
            this.hoi2RadioButton.Text = "HoI2 DDA";
            this.hoi2RadioButton.UseVisualStyleBackColor = true;
            this.hoi2RadioButton.CheckedChanged += new System.EventHandler(this.OnGameTypeRadioButtonCheckedChanged);
            // 
            // editGroupBox
            // 
            this.editGroupBox.Controls.Add(this.leaderButton);
            this.editGroupBox.Controls.Add(this.teamButton);
            this.editGroupBox.Controls.Add(this.ministerButton);
            this.editGroupBox.Location = new System.Drawing.Point(12, 160);
            this.editGroupBox.Name = "editGroupBox";
            this.editGroupBox.Size = new System.Drawing.Size(440, 109);
            this.editGroupBox.TabIndex = 7;
            this.editGroupBox.TabStop = false;
            this.editGroupBox.Text = "編集";
            // 
            // leaderButton
            // 
            this.leaderButton.Enabled = false;
            this.leaderButton.Location = new System.Drawing.Point(7, 18);
            this.leaderButton.Name = "leaderButton";
            this.leaderButton.Size = new System.Drawing.Size(75, 23);
            this.leaderButton.TabIndex = 2;
            this.leaderButton.Text = "指揮官";
            this.leaderButton.UseVisualStyleBackColor = true;
            this.leaderButton.Click += new System.EventHandler(this.OnLeaderButtonClick);
            // 
            // teamButton
            // 
            this.teamButton.Enabled = false;
            this.teamButton.Location = new System.Drawing.Point(169, 18);
            this.teamButton.Name = "teamButton";
            this.teamButton.Size = new System.Drawing.Size(75, 23);
            this.teamButton.TabIndex = 1;
            this.teamButton.Text = "研究機関";
            this.teamButton.UseVisualStyleBackColor = true;
            this.teamButton.Click += new System.EventHandler(this.OnTeamButtonClick);
            // 
            // gameFolderReferButton
            // 
            this.gameFolderReferButton.Location = new System.Drawing.Point(377, 82);
            this.gameFolderReferButton.Name = "gameFolderReferButton";
            this.gameFolderReferButton.Size = new System.Drawing.Size(75, 23);
            this.gameFolderReferButton.TabIndex = 8;
            this.gameFolderReferButton.Text = "参照";
            this.gameFolderReferButton.UseVisualStyleBackColor = true;
            this.gameFolderReferButton.Click += new System.EventHandler(this.OnLoadButtonClick);
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(377, 27);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(75, 23);
            this.exitButton.TabIndex = 9;
            this.exitButton.Text = "終了";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.OnExitButtonClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 281);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.gameFolderReferButton);
            this.Controls.Add(this.editGroupBox);
            this.Controls.Add(this.gameTypeGroupBox);
            this.Controls.Add(this.modTextBox);
            this.Controls.Add(this.modLabel);
            this.Controls.Add(this.gameFolderTextBox);
            this.Controls.Add(this.gameFolderLabel);
            this.Name = "MainForm";
            this.Text = "Alternative HoI2 Editor";
            this.Load += new System.EventHandler(this.OnMainFormLoad);
            this.gameTypeGroupBox.ResumeLayout(false);
            this.gameTypeGroupBox.PerformLayout();
            this.editGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ministerButton;
        private System.Windows.Forms.Label gameFolderLabel;
        private System.Windows.Forms.TextBox gameFolderTextBox;
        private System.Windows.Forms.Label modLabel;
        private System.Windows.Forms.TextBox modTextBox;
        private System.Windows.Forms.GroupBox gameTypeGroupBox;
        private System.Windows.Forms.RadioButton dhRadioButton;
        private System.Windows.Forms.RadioButton aodRadioButton;
        private System.Windows.Forms.RadioButton hoi2RadioButton;
        private System.Windows.Forms.GroupBox editGroupBox;
        private System.Windows.Forms.Button gameFolderReferButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button teamButton;
        private System.Windows.Forms.Button leaderButton;
    }
}