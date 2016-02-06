namespace HoI2Editor.Forms
{
    partial class ResearchViewerForm
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
            if (disposing && _techOverlayIcon != null)
            {
                _techOverlayIcon.Dispose();
                _techOverlayIcon = null;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResearchViewerForm));
            this.categoryListBox = new System.Windows.Forms.ListBox();
            this.techListView = new System.Windows.Forms.ListView();
            this.techNameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.techIdColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.techYearColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.techComponentsColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.techGroupBox = new System.Windows.Forms.GroupBox();
            this.optionGroupBox = new System.Windows.Forms.GroupBox();
            this.dayNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.monthNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.specifiedRadioButton = new System.Windows.Forms.RadioButton();
            this.historicalRadioButton = new System.Windows.Forms.RadioButton();
            this.blueprintCheckBox = new System.Windows.Forms.CheckBox();
            this.modifierTextBox = new System.Windows.Forms.TextBox();
            this.modifierLabel = new System.Windows.Forms.Label();
            this.nuclearNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.nuclearLabel = new System.Windows.Forms.Label();
            this.rocketNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.rocketLabel = new System.Windows.Forms.Label();
            this.yearNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.teamGroupBox = new System.Windows.Forms.GroupBox();
            this.teamListView = new System.Windows.Forms.ListView();
            this.teamDummyColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.teamRankColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.teamDaysColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.teamEndDateColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.teamNameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.teamIdColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.teamSkillColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.teamSpecialityColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.countryListBox = new System.Windows.Forms.ListBox();
            this.closeButton = new System.Windows.Forms.Button();
            this.techGroupBox.SuspendLayout();
            this.optionGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dayNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.monthNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuclearNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rocketNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yearNumericUpDown)).BeginInit();
            this.teamGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // categoryListBox
            // 
            this.categoryListBox.FormattingEnabled = true;
            resources.ApplyResources(this.categoryListBox, "categoryListBox");
            this.categoryListBox.Name = "categoryListBox";
            this.categoryListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.categoryListBox.SelectedIndexChanged += new System.EventHandler(this.OnCategoryListBoxSelectedIndexChanged);
            // 
            // techListView
            // 
            resources.ApplyResources(this.techListView, "techListView");
            this.techListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.techNameColumnHeader,
            this.techIdColumnHeader,
            this.techYearColumnHeader,
            this.techComponentsColumnHeader});
            this.techListView.FullRowSelect = true;
            this.techListView.GridLines = true;
            this.techListView.HideSelection = false;
            this.techListView.MultiSelect = false;
            this.techListView.Name = "techListView";
            this.techListView.OwnerDraw = true;
            this.techListView.UseCompatibleStateImageBehavior = false;
            this.techListView.View = System.Windows.Forms.View.Details;
            this.techListView.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.OnTechListViewColumnWidthChanged);
            this.techListView.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.OnTechListViewDrawColumnHeader);
            this.techListView.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.OnTechListViewDrawSubItem);
            this.techListView.SelectedIndexChanged += new System.EventHandler(this.OnTechListViewSelectedIndexChanged);
            // 
            // techNameColumnHeader
            // 
            resources.ApplyResources(this.techNameColumnHeader, "techNameColumnHeader");
            // 
            // techIdColumnHeader
            // 
            resources.ApplyResources(this.techIdColumnHeader, "techIdColumnHeader");
            // 
            // techYearColumnHeader
            // 
            resources.ApplyResources(this.techYearColumnHeader, "techYearColumnHeader");
            // 
            // techComponentsColumnHeader
            // 
            resources.ApplyResources(this.techComponentsColumnHeader, "techComponentsColumnHeader");
            // 
            // techGroupBox
            // 
            resources.ApplyResources(this.techGroupBox, "techGroupBox");
            this.techGroupBox.Controls.Add(this.categoryListBox);
            this.techGroupBox.Controls.Add(this.techListView);
            this.techGroupBox.Name = "techGroupBox";
            this.techGroupBox.TabStop = false;
            // 
            // optionGroupBox
            // 
            resources.ApplyResources(this.optionGroupBox, "optionGroupBox");
            this.optionGroupBox.Controls.Add(this.dayNumericUpDown);
            this.optionGroupBox.Controls.Add(this.monthNumericUpDown);
            this.optionGroupBox.Controls.Add(this.specifiedRadioButton);
            this.optionGroupBox.Controls.Add(this.historicalRadioButton);
            this.optionGroupBox.Controls.Add(this.blueprintCheckBox);
            this.optionGroupBox.Controls.Add(this.modifierTextBox);
            this.optionGroupBox.Controls.Add(this.modifierLabel);
            this.optionGroupBox.Controls.Add(this.nuclearNumericUpDown);
            this.optionGroupBox.Controls.Add(this.nuclearLabel);
            this.optionGroupBox.Controls.Add(this.rocketNumericUpDown);
            this.optionGroupBox.Controls.Add(this.rocketLabel);
            this.optionGroupBox.Controls.Add(this.yearNumericUpDown);
            this.optionGroupBox.Name = "optionGroupBox";
            this.optionGroupBox.TabStop = false;
            // 
            // dayNumericUpDown
            // 
            resources.ApplyResources(this.dayNumericUpDown, "dayNumericUpDown");
            this.dayNumericUpDown.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.dayNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.dayNumericUpDown.Name = "dayNumericUpDown";
            this.dayNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.dayNumericUpDown.ValueChanged += new System.EventHandler(this.OnDayNumericUpDownValueChanged);
            // 
            // monthNumericUpDown
            // 
            resources.ApplyResources(this.monthNumericUpDown, "monthNumericUpDown");
            this.monthNumericUpDown.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.monthNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.monthNumericUpDown.Name = "monthNumericUpDown";
            this.monthNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.monthNumericUpDown.ValueChanged += new System.EventHandler(this.OnMonthNumericUpDownValueChanged);
            // 
            // specifiedRadioButton
            // 
            resources.ApplyResources(this.specifiedRadioButton, "specifiedRadioButton");
            this.specifiedRadioButton.Name = "specifiedRadioButton";
            this.specifiedRadioButton.UseVisualStyleBackColor = true;
            // 
            // historicalRadioButton
            // 
            resources.ApplyResources(this.historicalRadioButton, "historicalRadioButton");
            this.historicalRadioButton.Checked = true;
            this.historicalRadioButton.Name = "historicalRadioButton";
            this.historicalRadioButton.TabStop = true;
            this.historicalRadioButton.UseVisualStyleBackColor = true;
            this.historicalRadioButton.CheckedChanged += new System.EventHandler(this.OnHistoricalRadioButtonCheckedChanged);
            // 
            // blueprintCheckBox
            // 
            resources.ApplyResources(this.blueprintCheckBox, "blueprintCheckBox");
            this.blueprintCheckBox.Name = "blueprintCheckBox";
            this.blueprintCheckBox.UseVisualStyleBackColor = true;
            this.blueprintCheckBox.CheckedChanged += new System.EventHandler(this.OnBlueprintCheckBoxCheckedChanged);
            // 
            // modifierTextBox
            // 
            resources.ApplyResources(this.modifierTextBox, "modifierTextBox");
            this.modifierTextBox.Name = "modifierTextBox";
            this.modifierTextBox.Validated += new System.EventHandler(this.OnModifierTextBoxValidated);
            // 
            // modifierLabel
            // 
            resources.ApplyResources(this.modifierLabel, "modifierLabel");
            this.modifierLabel.Name = "modifierLabel";
            // 
            // nuclearNumericUpDown
            // 
            resources.ApplyResources(this.nuclearNumericUpDown, "nuclearNumericUpDown");
            this.nuclearNumericUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nuclearNumericUpDown.Name = "nuclearNumericUpDown";
            this.nuclearNumericUpDown.ValueChanged += new System.EventHandler(this.OnNuclearNumericUpDownValueChanged);
            // 
            // nuclearLabel
            // 
            resources.ApplyResources(this.nuclearLabel, "nuclearLabel");
            this.nuclearLabel.Name = "nuclearLabel";
            // 
            // rocketNumericUpDown
            // 
            resources.ApplyResources(this.rocketNumericUpDown, "rocketNumericUpDown");
            this.rocketNumericUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.rocketNumericUpDown.Name = "rocketNumericUpDown";
            this.rocketNumericUpDown.ValueChanged += new System.EventHandler(this.OnRocketNumericUpDownValueChanged);
            // 
            // rocketLabel
            // 
            resources.ApplyResources(this.rocketLabel, "rocketLabel");
            this.rocketLabel.Name = "rocketLabel";
            // 
            // yearNumericUpDown
            // 
            resources.ApplyResources(this.yearNumericUpDown, "yearNumericUpDown");
            this.yearNumericUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.yearNumericUpDown.Name = "yearNumericUpDown";
            this.yearNumericUpDown.Value = new decimal(new int[] {
            1936,
            0,
            0,
            0});
            this.yearNumericUpDown.ValueChanged += new System.EventHandler(this.OnYearNumericUpDownValueChanged);
            // 
            // teamGroupBox
            // 
            resources.ApplyResources(this.teamGroupBox, "teamGroupBox");
            this.teamGroupBox.Controls.Add(this.teamListView);
            this.teamGroupBox.Controls.Add(this.countryListBox);
            this.teamGroupBox.Name = "teamGroupBox";
            this.teamGroupBox.TabStop = false;
            // 
            // teamListView
            // 
            resources.ApplyResources(this.teamListView, "teamListView");
            this.teamListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.teamDummyColumnHeader,
            this.teamRankColumnHeader,
            this.teamDaysColumnHeader,
            this.teamEndDateColumnHeader,
            this.teamNameColumnHeader,
            this.teamIdColumnHeader,
            this.teamSkillColumnHeader,
            this.teamSpecialityColumnHeader});
            this.teamListView.FullRowSelect = true;
            this.teamListView.GridLines = true;
            this.teamListView.MultiSelect = false;
            this.teamListView.Name = "teamListView";
            this.teamListView.OwnerDraw = true;
            this.teamListView.UseCompatibleStateImageBehavior = false;
            this.teamListView.View = System.Windows.Forms.View.Details;
            this.teamListView.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.OnTeamListViewColumnWidthChanged);
            this.teamListView.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.OnTeamListViewDrawColumnHeader);
            this.teamListView.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.OnTeamListViewDrawSubItem);
            // 
            // teamDummyColumnHeader
            // 
            resources.ApplyResources(this.teamDummyColumnHeader, "teamDummyColumnHeader");
            // 
            // teamRankColumnHeader
            // 
            resources.ApplyResources(this.teamRankColumnHeader, "teamRankColumnHeader");
            // 
            // teamDaysColumnHeader
            // 
            resources.ApplyResources(this.teamDaysColumnHeader, "teamDaysColumnHeader");
            // 
            // teamEndDateColumnHeader
            // 
            resources.ApplyResources(this.teamEndDateColumnHeader, "teamEndDateColumnHeader");
            // 
            // teamNameColumnHeader
            // 
            resources.ApplyResources(this.teamNameColumnHeader, "teamNameColumnHeader");
            // 
            // teamIdColumnHeader
            // 
            resources.ApplyResources(this.teamIdColumnHeader, "teamIdColumnHeader");
            // 
            // teamSkillColumnHeader
            // 
            resources.ApplyResources(this.teamSkillColumnHeader, "teamSkillColumnHeader");
            // 
            // teamSpecialityColumnHeader
            // 
            resources.ApplyResources(this.teamSpecialityColumnHeader, "teamSpecialityColumnHeader");
            // 
            // countryListBox
            // 
            resources.ApplyResources(this.countryListBox, "countryListBox");
            this.countryListBox.FormattingEnabled = true;
            this.countryListBox.MultiColumn = true;
            this.countryListBox.Name = "countryListBox";
            this.countryListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.countryListBox.SelectedIndexChanged += new System.EventHandler(this.OnCountryListBoxSelectedIndexChanged);
            // 
            // closeButton
            // 
            resources.ApplyResources(this.closeButton, "closeButton");
            this.closeButton.Name = "closeButton";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.OnCloseButtonClick);
            // 
            // ResearchViewerForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.teamGroupBox);
            this.Controls.Add(this.optionGroupBox);
            this.Controls.Add(this.techGroupBox);
            this.Name = "ResearchViewerForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.Move += new System.EventHandler(this.OnFormMove);
            this.Resize += new System.EventHandler(this.OnFormResize);
            this.techGroupBox.ResumeLayout(false);
            this.optionGroupBox.ResumeLayout(false);
            this.optionGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dayNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.monthNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuclearNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rocketNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yearNumericUpDown)).EndInit();
            this.teamGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox categoryListBox;
        private System.Windows.Forms.ListView techListView;
        private System.Windows.Forms.GroupBox techGroupBox;
        private System.Windows.Forms.GroupBox optionGroupBox;
        private System.Windows.Forms.TextBox modifierTextBox;
        private System.Windows.Forms.Label modifierLabel;
        private System.Windows.Forms.NumericUpDown nuclearNumericUpDown;
        private System.Windows.Forms.Label nuclearLabel;
        private System.Windows.Forms.NumericUpDown rocketNumericUpDown;
        private System.Windows.Forms.Label rocketLabel;
        private System.Windows.Forms.NumericUpDown yearNumericUpDown;
        private System.Windows.Forms.GroupBox teamGroupBox;
        private System.Windows.Forms.ListView teamListView;
        private System.Windows.Forms.ListBox countryListBox;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.ColumnHeader techNameColumnHeader;
        private System.Windows.Forms.ColumnHeader techIdColumnHeader;
        private System.Windows.Forms.ColumnHeader techYearColumnHeader;
        private System.Windows.Forms.ColumnHeader techComponentsColumnHeader;
        private System.Windows.Forms.RadioButton specifiedRadioButton;
        private System.Windows.Forms.RadioButton historicalRadioButton;
        private System.Windows.Forms.CheckBox blueprintCheckBox;
        private System.Windows.Forms.ColumnHeader teamNameColumnHeader;
        private System.Windows.Forms.ColumnHeader teamIdColumnHeader;
        private System.Windows.Forms.ColumnHeader teamSkillColumnHeader;
        private System.Windows.Forms.ColumnHeader teamSpecialityColumnHeader;
        private System.Windows.Forms.ColumnHeader teamRankColumnHeader;
        private System.Windows.Forms.ColumnHeader teamDaysColumnHeader;
        private System.Windows.Forms.ColumnHeader teamDummyColumnHeader;
        private System.Windows.Forms.ColumnHeader teamEndDateColumnHeader;
        private System.Windows.Forms.NumericUpDown dayNumericUpDown;
        private System.Windows.Forms.NumericUpDown monthNumericUpDown;
    }
}