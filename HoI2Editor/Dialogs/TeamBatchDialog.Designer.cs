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
            this.editGroupBox = new System.Windows.Forms.GroupBox();
            this.startYearCheckBox = new System.Windows.Forms.CheckBox();
            this.startYearNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.endYearCheckBox = new System.Windows.Forms.CheckBox();
            this.endYearNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.skillCheckBox = new System.Windows.Forms.CheckBox();
            this.skillNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.countryComboBox = new System.Windows.Forms.ComboBox();
            this.targetGroupBox = new System.Windows.Forms.GroupBox();
            this.allRadioButton = new System.Windows.Forms.RadioButton();
            this.selectedRadioButton = new System.Windows.Forms.RadioButton();
            this.specifiedRadioButton = new System.Windows.Forms.RadioButton();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.editGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startYearNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endYearNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.skillNumericUpDown)).BeginInit();
            this.targetGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // editGroupBox
            // 
            this.editGroupBox.Controls.Add(this.startYearCheckBox);
            this.editGroupBox.Controls.Add(this.startYearNumericUpDown);
            this.editGroupBox.Controls.Add(this.endYearCheckBox);
            this.editGroupBox.Controls.Add(this.endYearNumericUpDown);
            this.editGroupBox.Controls.Add(this.skillCheckBox);
            this.editGroupBox.Controls.Add(this.skillNumericUpDown);
            resources.ApplyResources(this.editGroupBox, "editGroupBox");
            this.editGroupBox.Name = "editGroupBox";
            this.editGroupBox.TabStop = false;
            // 
            // startYearCheckBox
            // 
            resources.ApplyResources(this.startYearCheckBox, "startYearCheckBox");
            this.startYearCheckBox.Name = "startYearCheckBox";
            this.startYearCheckBox.UseVisualStyleBackColor = true;
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
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OnOkButtonClick);
            // 
            // TeamBatchDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.editGroupBox);
            this.Controls.Add(this.targetGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TeamBatchDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.editGroupBox.ResumeLayout(false);
            this.editGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startYearNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.endYearNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.skillNumericUpDown)).EndInit();
            this.targetGroupBox.ResumeLayout(false);
            this.targetGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox editGroupBox;
        private System.Windows.Forms.CheckBox startYearCheckBox;
        private System.Windows.Forms.NumericUpDown startYearNumericUpDown;
        private System.Windows.Forms.CheckBox endYearCheckBox;
        private System.Windows.Forms.NumericUpDown endYearNumericUpDown;
        private System.Windows.Forms.CheckBox skillCheckBox;
        private System.Windows.Forms.NumericUpDown skillNumericUpDown;
        private System.Windows.Forms.ComboBox countryComboBox;
        private System.Windows.Forms.GroupBox targetGroupBox;
        private System.Windows.Forms.RadioButton allRadioButton;
        private System.Windows.Forms.RadioButton selectedRadioButton;
        private System.Windows.Forms.RadioButton specifiedRadioButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
    }
}