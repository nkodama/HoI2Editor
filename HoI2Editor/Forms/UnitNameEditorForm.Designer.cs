namespace HoI2Editor.Forms
{
    partial class UnitNameEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UnitNameEditorForm));
            this.countryListBox = new System.Windows.Forms.ListBox();
            this.closeButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.reloadButton = new System.Windows.Forms.Button();
            this.unitTypeListBox = new System.Windows.Forms.ListBox();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.targetGroupBox = new System.Windows.Forms.GroupBox();
            this.escortFighterCheckBox = new System.Windows.Forms.CheckBox();
            this.allUnitTypeCheckBox = new System.Windows.Forms.CheckBox();
            this.allCountryCheckBox = new System.Windows.Forms.CheckBox();
            this.regexpcheckBox = new System.Windows.Forms.CheckBox();
            this.replaceButton = new System.Windows.Forms.Button();
            this.findComboBox = new System.Windows.Forms.ComboBox();
            this.replaceComboBox = new System.Windows.Forms.ComboBox();
            this.startNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.endNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.interpolateButton = new System.Windows.Forms.Button();
            this.interpolateComboBox = new System.Windows.Forms.ComboBox();
            this.addButton = new System.Windows.Forms.Button();
            this.replaceGroupBox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.findLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.targetGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endNumericUpDown)).BeginInit();
            this.replaceGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // countryListBox
            // 
            resources.ApplyResources(this.countryListBox, "countryListBox");
            this.countryListBox.FormattingEnabled = true;
            this.countryListBox.Name = "countryListBox";
            // 
            // closeButton
            // 
            resources.ApplyResources(this.closeButton, "closeButton");
            this.closeButton.Name = "closeButton";
            this.closeButton.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            resources.ApplyResources(this.saveButton, "saveButton");
            this.saveButton.Name = "saveButton";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // reloadButton
            // 
            resources.ApplyResources(this.reloadButton, "reloadButton");
            this.reloadButton.Name = "reloadButton";
            this.reloadButton.UseVisualStyleBackColor = true;
            // 
            // unitTypeListBox
            // 
            resources.ApplyResources(this.unitTypeListBox, "unitTypeListBox");
            this.unitTypeListBox.FormattingEnabled = true;
            this.unitTypeListBox.Name = "unitTypeListBox";
            // 
            // nameTextBox
            // 
            resources.ApplyResources(this.nameTextBox, "nameTextBox");
            this.nameTextBox.Name = "nameTextBox";
            // 
            // targetGroupBox
            // 
            resources.ApplyResources(this.targetGroupBox, "targetGroupBox");
            this.targetGroupBox.Controls.Add(this.escortFighterCheckBox);
            this.targetGroupBox.Controls.Add(this.allUnitTypeCheckBox);
            this.targetGroupBox.Controls.Add(this.allCountryCheckBox);
            this.targetGroupBox.Name = "targetGroupBox";
            this.targetGroupBox.TabStop = false;
            // 
            // escortFighterCheckBox
            // 
            resources.ApplyResources(this.escortFighterCheckBox, "escortFighterCheckBox");
            this.escortFighterCheckBox.Name = "escortFighterCheckBox";
            this.escortFighterCheckBox.UseVisualStyleBackColor = true;
            // 
            // allUnitTypeCheckBox
            // 
            resources.ApplyResources(this.allUnitTypeCheckBox, "allUnitTypeCheckBox");
            this.allUnitTypeCheckBox.Name = "allUnitTypeCheckBox";
            this.allUnitTypeCheckBox.UseVisualStyleBackColor = true;
            // 
            // allCountryCheckBox
            // 
            resources.ApplyResources(this.allCountryCheckBox, "allCountryCheckBox");
            this.allCountryCheckBox.Name = "allCountryCheckBox";
            this.allCountryCheckBox.UseVisualStyleBackColor = true;
            // 
            // regexpcheckBox
            // 
            resources.ApplyResources(this.regexpcheckBox, "regexpcheckBox");
            this.regexpcheckBox.Name = "regexpcheckBox";
            this.regexpcheckBox.UseVisualStyleBackColor = true;
            // 
            // replaceButton
            // 
            resources.ApplyResources(this.replaceButton, "replaceButton");
            this.replaceButton.Name = "replaceButton";
            this.replaceButton.UseVisualStyleBackColor = true;
            // 
            // findComboBox
            // 
            resources.ApplyResources(this.findComboBox, "findComboBox");
            this.findComboBox.FormattingEnabled = true;
            this.findComboBox.Name = "findComboBox";
            // 
            // replaceComboBox
            // 
            resources.ApplyResources(this.replaceComboBox, "replaceComboBox");
            this.replaceComboBox.FormattingEnabled = true;
            this.replaceComboBox.Name = "replaceComboBox";
            // 
            // startNumericUpDown
            // 
            resources.ApplyResources(this.startNumericUpDown, "startNumericUpDown");
            this.startNumericUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.startNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.startNumericUpDown.Name = "startNumericUpDown";
            this.startNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // endNumericUpDown
            // 
            resources.ApplyResources(this.endNumericUpDown, "endNumericUpDown");
            this.endNumericUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.endNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.endNumericUpDown.Name = "endNumericUpDown";
            this.endNumericUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // interpolateButton
            // 
            resources.ApplyResources(this.interpolateButton, "interpolateButton");
            this.interpolateButton.Name = "interpolateButton";
            this.interpolateButton.UseVisualStyleBackColor = true;
            // 
            // interpolateComboBox
            // 
            resources.ApplyResources(this.interpolateComboBox, "interpolateComboBox");
            this.interpolateComboBox.FormattingEnabled = true;
            this.interpolateComboBox.Name = "interpolateComboBox";
            // 
            // addButton
            // 
            resources.ApplyResources(this.addButton, "addButton");
            this.addButton.Name = "addButton";
            this.addButton.UseVisualStyleBackColor = true;
            // 
            // replaceGroupBox
            // 
            resources.ApplyResources(this.replaceGroupBox, "replaceGroupBox");
            this.replaceGroupBox.Controls.Add(this.label1);
            this.replaceGroupBox.Controls.Add(this.findLabel);
            this.replaceGroupBox.Controls.Add(this.regexpcheckBox);
            this.replaceGroupBox.Controls.Add(this.replaceButton);
            this.replaceGroupBox.Controls.Add(this.findComboBox);
            this.replaceGroupBox.Controls.Add(this.replaceComboBox);
            this.replaceGroupBox.Name = "replaceGroupBox";
            this.replaceGroupBox.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // findLabel
            // 
            resources.ApplyResources(this.findLabel, "findLabel");
            this.findLabel.Name = "findLabel";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.addButton);
            this.groupBox1.Controls.Add(this.interpolateComboBox);
            this.groupBox1.Controls.Add(this.startNumericUpDown);
            this.groupBox1.Controls.Add(this.interpolateButton);
            this.groupBox1.Controls.Add(this.endNumericUpDown);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // UnitNameEditorForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.replaceGroupBox);
            this.Controls.Add(this.targetGroupBox);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.unitTypeListBox);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.reloadButton);
            this.Controls.Add(this.countryListBox);
            this.Name = "UnitNameEditorForm";
            this.targetGroupBox.ResumeLayout(false);
            this.targetGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.endNumericUpDown)).EndInit();
            this.replaceGroupBox.ResumeLayout(false);
            this.replaceGroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox countryListBox;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button reloadButton;
        private System.Windows.Forms.ListBox unitTypeListBox;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.GroupBox targetGroupBox;
        private System.Windows.Forms.CheckBox escortFighterCheckBox;
        private System.Windows.Forms.CheckBox allUnitTypeCheckBox;
        private System.Windows.Forms.CheckBox allCountryCheckBox;
        private System.Windows.Forms.CheckBox regexpcheckBox;
        private System.Windows.Forms.Button replaceButton;
        private System.Windows.Forms.ComboBox findComboBox;
        private System.Windows.Forms.ComboBox replaceComboBox;
        private System.Windows.Forms.NumericUpDown startNumericUpDown;
        private System.Windows.Forms.NumericUpDown endNumericUpDown;
        private System.Windows.Forms.Button interpolateButton;
        private System.Windows.Forms.ComboBox interpolateComboBox;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.GroupBox replaceGroupBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label findLabel;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}