namespace HoI2Editor.Dialogs
{
    partial class MinisterBatchDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MinisterBatchDialog));
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.specifiedRadioButton = new System.Windows.Forms.RadioButton();
            this.selectedRadioButton = new System.Windows.Forms.RadioButton();
            this.allRadioButton = new System.Windows.Forms.RadioButton();
            this.startYearCheckBox = new System.Windows.Forms.CheckBox();
            this.endYearCheckBox = new System.Windows.Forms.CheckBox();
            this.startYearNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.endYearNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.retirementYearNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.retirementYearCheckBox = new System.Windows.Forms.CheckBox();
            this.ideologyCheckBox = new System.Windows.Forms.CheckBox();
            this.ideologyComboBox = new System.Windows.Forms.ComboBox();
            this.loyaltyCheckBox = new System.Windows.Forms.CheckBox();
            this.loyaltyComboBox = new System.Windows.Forms.ComboBox();
            this.srcComboBox = new System.Windows.Forms.ComboBox();
            this.countryGroupBox = new System.Windows.Forms.GroupBox();
            this.modifyGroupBox = new System.Windows.Forms.GroupBox();
            this.actionGroupBox = new System.Windows.Forms.GroupBox();
            this.destLabel = new System.Windows.Forms.Label();
            this.moveRadioButton = new System.Windows.Forms.RadioButton();
            this.copyRadioButton = new System.Windows.Forms.RadioButton();
            this.modifyRadioButton = new System.Windows.Forms.RadioButton();
            this.idNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.idLabel = new System.Windows.Forms.Label();
            this.destComboBox = new System.Windows.Forms.ComboBox();
            this.positionGroupBox = new System.Windows.Forms.GroupBox();
            this.coafCheckBox = new System.Windows.Forms.CheckBox();
            this.conCheckBox = new System.Windows.Forms.CheckBox();
            this.coaCheckBox = new System.Windows.Forms.CheckBox();
            this.cosCheckBox = new System.Windows.Forms.CheckBox();
            this.moiCheckBox = new System.Windows.Forms.CheckBox();
            this.mosCheckBox = new System.Windows.Forms.CheckBox();
            this.moaCheckBox = new System.Windows.Forms.CheckBox();
            this.mofCheckBox = new System.Windows.Forms.CheckBox();
            this.hogCheckBox = new System.Windows.Forms.CheckBox();
            this.hosCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.startYearNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endYearNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.retirementYearNumericUpDown)).BeginInit();
            this.countryGroupBox.SuspendLayout();
            this.modifyGroupBox.SuspendLayout();
            this.actionGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.idNumericUpDown)).BeginInit();
            this.positionGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OnOkButtonClick);
            // 
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // specifiedRadioButton
            // 
            resources.ApplyResources(this.specifiedRadioButton, "specifiedRadioButton");
            this.specifiedRadioButton.Name = "specifiedRadioButton";
            this.specifiedRadioButton.UseVisualStyleBackColor = true;
            // 
            // selectedRadioButton
            // 
            resources.ApplyResources(this.selectedRadioButton, "selectedRadioButton");
            this.selectedRadioButton.Checked = true;
            this.selectedRadioButton.Name = "selectedRadioButton";
            this.selectedRadioButton.TabStop = true;
            this.selectedRadioButton.UseVisualStyleBackColor = true;
            // 
            // allRadioButton
            // 
            resources.ApplyResources(this.allRadioButton, "allRadioButton");
            this.allRadioButton.Name = "allRadioButton";
            this.allRadioButton.UseVisualStyleBackColor = true;
            // 
            // startYearCheckBox
            // 
            resources.ApplyResources(this.startYearCheckBox, "startYearCheckBox");
            this.startYearCheckBox.Name = "startYearCheckBox";
            this.startYearCheckBox.UseVisualStyleBackColor = true;
            this.startYearCheckBox.CheckedChanged += new System.EventHandler(this.OnStartYearCheckBoxCheckedChanged);
            // 
            // endYearCheckBox
            // 
            resources.ApplyResources(this.endYearCheckBox, "endYearCheckBox");
            this.endYearCheckBox.Name = "endYearCheckBox";
            this.endYearCheckBox.UseVisualStyleBackColor = true;
            this.endYearCheckBox.CheckedChanged += new System.EventHandler(this.OnEndYearCheckBoxCheckedChanged);
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
            1936,
            0,
            0,
            0});
            this.startYearNumericUpDown.ValueChanged += new System.EventHandler(this.OnStartYearNumericUpDownValueChanged);
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
            1964,
            0,
            0,
            0});
            this.endYearNumericUpDown.ValueChanged += new System.EventHandler(this.OnEndYearNumericUpDownValueChanged);
            // 
            // retirementYearNumericUpDown
            // 
            resources.ApplyResources(this.retirementYearNumericUpDown, "retirementYearNumericUpDown");
            this.retirementYearNumericUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.retirementYearNumericUpDown.Name = "retirementYearNumericUpDown";
            this.retirementYearNumericUpDown.Value = new decimal(new int[] {
            1999,
            0,
            0,
            0});
            this.retirementYearNumericUpDown.ValueChanged += new System.EventHandler(this.OnRetirementYearNumericUpDownValueChanged);
            // 
            // retirementYearCheckBox
            // 
            resources.ApplyResources(this.retirementYearCheckBox, "retirementYearCheckBox");
            this.retirementYearCheckBox.Name = "retirementYearCheckBox";
            this.retirementYearCheckBox.UseVisualStyleBackColor = true;
            this.retirementYearCheckBox.CheckedChanged += new System.EventHandler(this.OnRetirementYearCheckBoxCheckedChanged);
            // 
            // ideologyCheckBox
            // 
            resources.ApplyResources(this.ideologyCheckBox, "ideologyCheckBox");
            this.ideologyCheckBox.Name = "ideologyCheckBox";
            this.ideologyCheckBox.UseVisualStyleBackColor = true;
            this.ideologyCheckBox.CheckedChanged += new System.EventHandler(this.OnIdeologyCheckBoxCheckedChanged);
            // 
            // ideologyComboBox
            // 
            resources.ApplyResources(this.ideologyComboBox, "ideologyComboBox");
            this.ideologyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ideologyComboBox.FormattingEnabled = true;
            this.ideologyComboBox.Name = "ideologyComboBox";
            // 
            // loyaltyCheckBox
            // 
            resources.ApplyResources(this.loyaltyCheckBox, "loyaltyCheckBox");
            this.loyaltyCheckBox.Name = "loyaltyCheckBox";
            this.loyaltyCheckBox.UseVisualStyleBackColor = true;
            this.loyaltyCheckBox.CheckedChanged += new System.EventHandler(this.OnLoyaltyCheckBoxCheckedChanged);
            // 
            // loyaltyComboBox
            // 
            resources.ApplyResources(this.loyaltyComboBox, "loyaltyComboBox");
            this.loyaltyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.loyaltyComboBox.FormattingEnabled = true;
            this.loyaltyComboBox.Name = "loyaltyComboBox";
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
            // modifyGroupBox
            // 
            resources.ApplyResources(this.modifyGroupBox, "modifyGroupBox");
            this.modifyGroupBox.Controls.Add(this.startYearCheckBox);
            this.modifyGroupBox.Controls.Add(this.loyaltyComboBox);
            this.modifyGroupBox.Controls.Add(this.startYearNumericUpDown);
            this.modifyGroupBox.Controls.Add(this.loyaltyCheckBox);
            this.modifyGroupBox.Controls.Add(this.endYearCheckBox);
            this.modifyGroupBox.Controls.Add(this.ideologyComboBox);
            this.modifyGroupBox.Controls.Add(this.ideologyCheckBox);
            this.modifyGroupBox.Controls.Add(this.endYearNumericUpDown);
            this.modifyGroupBox.Controls.Add(this.retirementYearCheckBox);
            this.modifyGroupBox.Controls.Add(this.retirementYearNumericUpDown);
            this.modifyGroupBox.Name = "modifyGroupBox";
            this.modifyGroupBox.TabStop = false;
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
            // positionGroupBox
            // 
            resources.ApplyResources(this.positionGroupBox, "positionGroupBox");
            this.positionGroupBox.Controls.Add(this.coafCheckBox);
            this.positionGroupBox.Controls.Add(this.conCheckBox);
            this.positionGroupBox.Controls.Add(this.coaCheckBox);
            this.positionGroupBox.Controls.Add(this.cosCheckBox);
            this.positionGroupBox.Controls.Add(this.moiCheckBox);
            this.positionGroupBox.Controls.Add(this.mosCheckBox);
            this.positionGroupBox.Controls.Add(this.moaCheckBox);
            this.positionGroupBox.Controls.Add(this.mofCheckBox);
            this.positionGroupBox.Controls.Add(this.hogCheckBox);
            this.positionGroupBox.Controls.Add(this.hosCheckBox);
            this.positionGroupBox.Name = "positionGroupBox";
            this.positionGroupBox.TabStop = false;
            // 
            // coafCheckBox
            // 
            resources.ApplyResources(this.coafCheckBox, "coafCheckBox");
            this.coafCheckBox.Checked = true;
            this.coafCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.coafCheckBox.Name = "coafCheckBox";
            this.coafCheckBox.UseVisualStyleBackColor = true;
            // 
            // conCheckBox
            // 
            resources.ApplyResources(this.conCheckBox, "conCheckBox");
            this.conCheckBox.Checked = true;
            this.conCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.conCheckBox.Name = "conCheckBox";
            this.conCheckBox.UseVisualStyleBackColor = true;
            // 
            // coaCheckBox
            // 
            resources.ApplyResources(this.coaCheckBox, "coaCheckBox");
            this.coaCheckBox.Checked = true;
            this.coaCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.coaCheckBox.Name = "coaCheckBox";
            this.coaCheckBox.UseVisualStyleBackColor = true;
            // 
            // cosCheckBox
            // 
            resources.ApplyResources(this.cosCheckBox, "cosCheckBox");
            this.cosCheckBox.Checked = true;
            this.cosCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cosCheckBox.Name = "cosCheckBox";
            this.cosCheckBox.UseVisualStyleBackColor = true;
            // 
            // moiCheckBox
            // 
            resources.ApplyResources(this.moiCheckBox, "moiCheckBox");
            this.moiCheckBox.Checked = true;
            this.moiCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.moiCheckBox.Name = "moiCheckBox";
            this.moiCheckBox.UseVisualStyleBackColor = true;
            // 
            // mosCheckBox
            // 
            resources.ApplyResources(this.mosCheckBox, "mosCheckBox");
            this.mosCheckBox.Checked = true;
            this.mosCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mosCheckBox.Name = "mosCheckBox";
            this.mosCheckBox.UseVisualStyleBackColor = true;
            // 
            // moaCheckBox
            // 
            resources.ApplyResources(this.moaCheckBox, "moaCheckBox");
            this.moaCheckBox.Checked = true;
            this.moaCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.moaCheckBox.Name = "moaCheckBox";
            this.moaCheckBox.UseVisualStyleBackColor = true;
            // 
            // mofCheckBox
            // 
            resources.ApplyResources(this.mofCheckBox, "mofCheckBox");
            this.mofCheckBox.Checked = true;
            this.mofCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mofCheckBox.Name = "mofCheckBox";
            this.mofCheckBox.UseVisualStyleBackColor = true;
            // 
            // hogCheckBox
            // 
            resources.ApplyResources(this.hogCheckBox, "hogCheckBox");
            this.hogCheckBox.Checked = true;
            this.hogCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hogCheckBox.Name = "hogCheckBox";
            this.hogCheckBox.UseVisualStyleBackColor = true;
            // 
            // hosCheckBox
            // 
            resources.ApplyResources(this.hosCheckBox, "hosCheckBox");
            this.hosCheckBox.Checked = true;
            this.hosCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hosCheckBox.Name = "hosCheckBox";
            this.hosCheckBox.UseVisualStyleBackColor = true;
            // 
            // MinisterBatchDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.positionGroupBox);
            this.Controls.Add(this.actionGroupBox);
            this.Controls.Add(this.modifyGroupBox);
            this.Controls.Add(this.countryGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MinisterBatchDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Load += new System.EventHandler(this.OnFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.startYearNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.endYearNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.retirementYearNumericUpDown)).EndInit();
            this.countryGroupBox.ResumeLayout(false);
            this.countryGroupBox.PerformLayout();
            this.modifyGroupBox.ResumeLayout(false);
            this.modifyGroupBox.PerformLayout();
            this.actionGroupBox.ResumeLayout(false);
            this.actionGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.idNumericUpDown)).EndInit();
            this.positionGroupBox.ResumeLayout(false);
            this.positionGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.RadioButton allRadioButton;
        private System.Windows.Forms.RadioButton specifiedRadioButton;
        private System.Windows.Forms.RadioButton selectedRadioButton;
        private System.Windows.Forms.CheckBox startYearCheckBox;
        private System.Windows.Forms.CheckBox endYearCheckBox;
        private System.Windows.Forms.NumericUpDown startYearNumericUpDown;
        private System.Windows.Forms.NumericUpDown endYearNumericUpDown;
        private System.Windows.Forms.NumericUpDown retirementYearNumericUpDown;
        private System.Windows.Forms.CheckBox retirementYearCheckBox;
        private System.Windows.Forms.CheckBox ideologyCheckBox;
        private System.Windows.Forms.ComboBox ideologyComboBox;
        private System.Windows.Forms.CheckBox loyaltyCheckBox;
        private System.Windows.Forms.ComboBox loyaltyComboBox;
        private System.Windows.Forms.ComboBox srcComboBox;
        private System.Windows.Forms.GroupBox countryGroupBox;
        private System.Windows.Forms.GroupBox modifyGroupBox;
        private System.Windows.Forms.GroupBox actionGroupBox;
        private System.Windows.Forms.Label destLabel;
        private System.Windows.Forms.RadioButton moveRadioButton;
        private System.Windows.Forms.RadioButton copyRadioButton;
        private System.Windows.Forms.RadioButton modifyRadioButton;
        private System.Windows.Forms.ComboBox destComboBox;
        private System.Windows.Forms.Label idLabel;
        private System.Windows.Forms.NumericUpDown idNumericUpDown;
        private System.Windows.Forms.GroupBox positionGroupBox;
        private System.Windows.Forms.CheckBox coafCheckBox;
        private System.Windows.Forms.CheckBox conCheckBox;
        private System.Windows.Forms.CheckBox coaCheckBox;
        private System.Windows.Forms.CheckBox cosCheckBox;
        private System.Windows.Forms.CheckBox moiCheckBox;
        private System.Windows.Forms.CheckBox mosCheckBox;
        private System.Windows.Forms.CheckBox moaCheckBox;
        private System.Windows.Forms.CheckBox mofCheckBox;
        private System.Windows.Forms.CheckBox hogCheckBox;
        private System.Windows.Forms.CheckBox hosCheckBox;
    }
}