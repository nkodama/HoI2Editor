namespace HoI2Editor.Dialogs
{
    partial class TeamBatchDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TeamBatchDialog));
            this.modifyGroupBox = new System.Windows.Forms.GroupBox();
            this.startYearCheckBox = new System.Windows.Forms.CheckBox();
            this.startYearNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.endYearCheckBox = new System.Windows.Forms.CheckBox();
            this.endYearNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.skillCheckBox = new System.Windows.Forms.CheckBox();
            this.skillNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.srcComboBox = new System.Windows.Forms.ComboBox();
            this.countryGroupBox = new System.Windows.Forms.GroupBox();
            this.allRadioButton = new System.Windows.Forms.RadioButton();
            this.selectedRadioButton = new System.Windows.Forms.RadioButton();
            this.specifiedRadioButton = new System.Windows.Forms.RadioButton();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.actionGroupBox = new System.Windows.Forms.GroupBox();
            this.destLabel = new System.Windows.Forms.Label();
            this.moveRadioButton = new System.Windows.Forms.RadioButton();
            this.copyRadioButton = new System.Windows.Forms.RadioButton();
            this.modifyRadioButton = new System.Windows.Forms.RadioButton();
            this.idNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.idLabel = new System.Windows.Forms.Label();
            this.destComboBox = new System.Windows.Forms.ComboBox();
            this.modifyGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startYearNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endYearNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.skillNumericUpDown)).BeginInit();
            this.countryGroupBox.SuspendLayout();
            this.actionGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.idNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // modifyGroupBox
            // 
            resources.ApplyResources(this.modifyGroupBox, "modifyGroupBox");
            this.modifyGroupBox.Controls.Add(this.startYearCheckBox);
            this.modifyGroupBox.Controls.Add(this.startYearNumericUpDown);
            this.modifyGroupBox.Controls.Add(this.endYearCheckBox);
            this.modifyGroupBox.Controls.Add(this.endYearNumericUpDown);
            this.modifyGroupBox.Controls.Add(this.skillCheckBox);
            this.modifyGroupBox.Controls.Add(this.skillNumericUpDown);
            this.modifyGroupBox.Name = "modifyGroupBox";
            this.modifyGroupBox.TabStop = false;
            // 
            // startYearCheckBox
            // 
            resources.ApplyResources(this.startYearCheckBox, "startYearCheckBox");
            this.startYearCheckBox.Name = "startYearCheckBox";
            this.startYearCheckBox.UseVisualStyleBackColor = true;
            this.startYearCheckBox.CheckedChanged += new System.EventHandler(this.OnStartYearCheckBoxCheckedChanged);
            // 
            // startYearNumericUpDown
            // 
            resources.ApplyResources(this.startYearNumericUpDown, "startYearNumericUpDown");
            this.startYearNumericUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.startYearNumericUpDown.Name = "startYearNumericUpDown";
            this.startYearNumericUpDown.Value = new decimal(new int[] {
            1930,
            0,
            0,
            0});
            this.startYearNumericUpDown.ValueChanged += new System.EventHandler(this.OnStartYearNumericUpDownValueChanged);
            // 
            // endYearCheckBox
            // 
            resources.ApplyResources(this.endYearCheckBox, "endYearCheckBox");
            this.endYearCheckBox.Name = "endYearCheckBox";
            this.endYearCheckBox.UseVisualStyleBackColor = true;
            this.endYearCheckBox.CheckedChanged += new System.EventHandler(this.OnEndYearCheckBoxCheckedChanged);
            // 
            // endYearNumericUpDown
            // 
            resources.ApplyResources(this.endYearNumericUpDown, "endYearNumericUpDown");
            this.endYearNumericUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.endYearNumericUpDown.Name = "endYearNumericUpDown";
            this.endYearNumericUpDown.Value = new decimal(new int[] {
            1970,
            0,
            0,
            0});
            this.endYearNumericUpDown.ValueChanged += new System.EventHandler(this.OnEndYearNumericUpDownValueChanged);
            // 
            // skillCheckBox
            // 
            resources.ApplyResources(this.skillCheckBox, "skillCheckBox");
            this.skillCheckBox.Name = "skillCheckBox";
            this.skillCheckBox.UseVisualStyleBackColor = true;
            this.skillCheckBox.CheckedChanged += new System.EventHandler(this.OnSkillCheckBoxCheckedChanged);
            // 
            // skillNumericUpDown
            // 
            resources.ApplyResources(this.skillNumericUpDown, "skillNumericUpDown");
            this.skillNumericUpDown.Name = "skillNumericUpDown";
            this.skillNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.skillNumericUpDown.ValueChanged += new System.EventHandler(this.OnSkillNumericUpDownValueChanged);
            // 
            // srcComboBox
            // 
            resources.ApplyResources(this.srcComboBox, "srcComboBox");
            this.srcComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.srcComboBox.FormattingEnabled = true;
            this.srcComboBox.Name = "srcComboBox";
            // 
            // countryGroupBox
            // 
            resources.ApplyResources(this.countryGroupBox, "countryGroupBox");
            this.countryGroupBox.Controls.Add(this.allRadioButton);
            this.countryGroupBox.Controls.Add(this.selectedRadioButton);
            this.countryGroupBox.Controls.Add(this.srcComboBox);
            this.countryGroupBox.Controls.Add(this.specifiedRadioButton);
            this.countryGroupBox.Name = "countryGroupBox";
            this.countryGroupBox.TabStop = false;
            // 
            // allRadioButton
            // 
            resources.ApplyResources(this.allRadioButton, "allRadioButton");
            this.allRadioButton.Name = "allRadioButton";
            this.allRadioButton.UseVisualStyleBackColor = true;
            // 
            // selectedRadioButton
            // 
            resources.ApplyResources(this.selectedRadioButton, "selectedRadioButton");
            this.selectedRadioButton.Checked = true;
            this.selectedRadioButton.Name = "selectedRadioButton";
            this.selectedRadioButton.TabStop = true;
            this.selectedRadioButton.UseVisualStyleBackColor = true;
            // 
            // specifiedRadioButton
            // 
            resources.ApplyResources(this.specifiedRadioButton, "specifiedRadioButton");
            this.specifiedRadioButton.Name = "specifiedRadioButton";
            this.specifiedRadioButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OnOkButtonClick);
            // 
            // actionGroupBox
            // 
            resources.ApplyResources(this.actionGroupBox, "actionGroupBox");
            this.actionGroupBox.Controls.Add(this.destLabel);
            this.actionGroupBox.Controls.Add(this.moveRadioButton);
            this.actionGroupBox.Controls.Add(this.copyRadioButton);
            this.actionGroupBox.Controls.Add(this.modifyRadioButton);
            this.actionGroupBox.Controls.Add(this.idNumericUpDown);
            this.actionGroupBox.Controls.Add(this.idLabel);
            this.actionGroupBox.Controls.Add(this.destComboBox);
            this.actionGroupBox.Name = "actionGroupBox";
            this.actionGroupBox.TabStop = false;
            // 
            // destLabel
            // 
            resources.ApplyResources(this.destLabel, "destLabel");
            this.destLabel.Name = "destLabel";
            // 
            // moveRadioButton
            // 
            resources.ApplyResources(this.moveRadioButton, "moveRadioButton");
            this.moveRadioButton.Name = "moveRadioButton";
            this.moveRadioButton.TabStop = true;
            this.moveRadioButton.UseVisualStyleBackColor = true;
            this.moveRadioButton.CheckedChanged += new System.EventHandler(this.OnMoveRadioButtonCheckedChanged);
            // 
            // copyRadioButton
            // 
            resources.ApplyResources(this.copyRadioButton, "copyRadioButton");
            this.copyRadioButton.Name = "copyRadioButton";
            this.copyRadioButton.TabStop = true;
            this.copyRadioButton.UseVisualStyleBackColor = true;
            this.copyRadioButton.CheckedChanged += new System.EventHandler(this.OnCopyRadioButtonCheckedChanged);
            // 
            // modifyRadioButton
            // 
            resources.ApplyResources(this.modifyRadioButton, "modifyRadioButton");
            this.modifyRadioButton.Checked = true;
            this.modifyRadioButton.Name = "modifyRadioButton";
            this.modifyRadioButton.TabStop = true;
            this.modifyRadioButton.UseVisualStyleBackColor = true;
            // 
            // idNumericUpDown
            // 
            resources.ApplyResources(this.idNumericUpDown, "idNumericUpDown");
            this.idNumericUpDown.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.idNumericUpDown.Name = "idNumericUpDown";
            this.idNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // idLabel
            // 
            resources.ApplyResources(this.idLabel, "idLabel");
            this.idLabel.Name = "idLabel";
            // 
            // destComboBox
            // 
            resources.ApplyResources(this.destComboBox, "destComboBox");
            this.destComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.destComboBox.FormattingEnabled = true;
            this.destComboBox.Name = "destComboBox";
            // 
            // TeamBatchDialog
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.actionGroupBox);
            this.Controls.Add(this.modifyGroupBox);
            this.Controls.Add(this.countryGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TeamBatchDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.modifyGroupBox.ResumeLayout(false);
            this.modifyGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startYearNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.endYearNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.skillNumericUpDown)).EndInit();
            this.countryGroupBox.ResumeLayout(false);
            this.countryGroupBox.PerformLayout();
            this.actionGroupBox.ResumeLayout(false);
            this.actionGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.idNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox modifyGroupBox;
        private System.Windows.Forms.CheckBox startYearCheckBox;
        private System.Windows.Forms.NumericUpDown startYearNumericUpDown;
        private System.Windows.Forms.CheckBox endYearCheckBox;
        private System.Windows.Forms.NumericUpDown endYearNumericUpDown;
        private System.Windows.Forms.CheckBox skillCheckBox;
        private System.Windows.Forms.NumericUpDown skillNumericUpDown;
        private System.Windows.Forms.ComboBox srcComboBox;
        private System.Windows.Forms.GroupBox countryGroupBox;
        private System.Windows.Forms.RadioButton allRadioButton;
        private System.Windows.Forms.RadioButton selectedRadioButton;
        private System.Windows.Forms.RadioButton specifiedRadioButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.GroupBox actionGroupBox;
        private System.Windows.Forms.Label destLabel;
        private System.Windows.Forms.RadioButton moveRadioButton;
        private System.Windows.Forms.RadioButton copyRadioButton;
        private System.Windows.Forms.RadioButton modifyRadioButton;
        private System.Windows.Forms.NumericUpDown idNumericUpDown;
        private System.Windows.Forms.Label idLabel;
        private System.Windows.Forms.ComboBox destComboBox;
    }
}