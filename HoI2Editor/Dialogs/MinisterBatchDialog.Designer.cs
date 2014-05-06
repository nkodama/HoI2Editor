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
            this.countryComboBox = new System.Windows.Forms.ComboBox();
            this.targetGroupBox = new System.Windows.Forms.GroupBox();
            this.editGroupBox = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.startYearNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endYearNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.retirementYearNumericUpDown)).BeginInit();
            this.targetGroupBox.SuspendLayout();
            this.editGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OnOkButtonClick);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.cancelButton, "cancelButton");
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
            // 
            // endYearCheckBox
            // 
            resources.ApplyResources(this.endYearCheckBox, "endYearCheckBox");
            this.endYearCheckBox.Name = "endYearCheckBox";
            this.endYearCheckBox.UseVisualStyleBackColor = true;
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
            // 
            // ideologyCheckBox
            // 
            resources.ApplyResources(this.ideologyCheckBox, "ideologyCheckBox");
            this.ideologyCheckBox.Name = "ideologyCheckBox";
            this.ideologyCheckBox.UseVisualStyleBackColor = true;
            // 
            // ideologyComboBox
            // 
            this.ideologyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ideologyComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.ideologyComboBox, "ideologyComboBox");
            this.ideologyComboBox.Name = "ideologyComboBox";
            // 
            // loyaltyCheckBox
            // 
            resources.ApplyResources(this.loyaltyCheckBox, "loyaltyCheckBox");
            this.loyaltyCheckBox.Name = "loyaltyCheckBox";
            this.loyaltyCheckBox.UseVisualStyleBackColor = true;
            // 
            // loyaltyComboBox
            // 
            this.loyaltyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.loyaltyComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.loyaltyComboBox, "loyaltyComboBox");
            this.loyaltyComboBox.Name = "loyaltyComboBox";
            // 
            // countryComboBox
            // 
            this.countryComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.countryComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.countryComboBox, "countryComboBox");
            this.countryComboBox.Name = "countryComboBox";
            // 
            // targetGroupBox
            // 
            this.targetGroupBox.Controls.Add(this.allRadioButton);
            this.targetGroupBox.Controls.Add(this.selectedRadioButton);
            this.targetGroupBox.Controls.Add(this.countryComboBox);
            this.targetGroupBox.Controls.Add(this.specifiedRadioButton);
            resources.ApplyResources(this.targetGroupBox, "targetGroupBox");
            this.targetGroupBox.Name = "targetGroupBox";
            this.targetGroupBox.TabStop = false;
            // 
            // editGroupBox
            // 
            this.editGroupBox.Controls.Add(this.startYearCheckBox);
            this.editGroupBox.Controls.Add(this.loyaltyComboBox);
            this.editGroupBox.Controls.Add(this.startYearNumericUpDown);
            this.editGroupBox.Controls.Add(this.loyaltyCheckBox);
            this.editGroupBox.Controls.Add(this.endYearCheckBox);
            this.editGroupBox.Controls.Add(this.ideologyComboBox);
            this.editGroupBox.Controls.Add(this.ideologyCheckBox);
            this.editGroupBox.Controls.Add(this.endYearNumericUpDown);
            this.editGroupBox.Controls.Add(this.retirementYearCheckBox);
            this.editGroupBox.Controls.Add(this.retirementYearNumericUpDown);
            resources.ApplyResources(this.editGroupBox, "editGroupBox");
            this.editGroupBox.Name = "editGroupBox";
            this.editGroupBox.TabStop = false;
            // 
            // MinisterBatchDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.editGroupBox);
            this.Controls.Add(this.targetGroupBox);
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
            this.targetGroupBox.ResumeLayout(false);
            this.targetGroupBox.PerformLayout();
            this.editGroupBox.ResumeLayout(false);
            this.editGroupBox.PerformLayout();
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
        private System.Windows.Forms.ComboBox countryComboBox;
        private System.Windows.Forms.GroupBox targetGroupBox;
        private System.Windows.Forms.GroupBox editGroupBox;
    }
}