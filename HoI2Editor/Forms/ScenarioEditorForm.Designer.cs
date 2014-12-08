namespace HoI2Editor.Forms
{
    partial class ScenarioEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScenarioEditorForm));
            this.scenarioTabControl = new System.Windows.Forms.TabControl();
            this.mainTabPage = new System.Windows.Forms.TabPage();
            this.optionGroupBox = new System.Windows.Forms.GroupBox();
            this.gameSpeedLabel = new System.Windows.Forms.Label();
            this.difficultyLabel = new System.Windows.Forms.Label();
            this.aiAggressiveLabel = new System.Windows.Forms.Label();
            this.gameSpeedComboBox = new System.Windows.Forms.ComboBox();
            this.difficultyComboBox = new System.Windows.Forms.ComboBox();
            this.aiAggressiveComboBox = new System.Windows.Forms.ComboBox();
            this.allowTechnologyCheckBox = new System.Windows.Forms.CheckBox();
            this.allowProductionCheckBox = new System.Windows.Forms.CheckBox();
            this.allowDiplomacyCheckBox = new System.Windows.Forms.CheckBox();
            this.freeCountryCheckBox = new System.Windows.Forms.CheckBox();
            this.battleScenarioCheckBox = new System.Windows.Forms.CheckBox();
            this.countryGroupBox = new System.Windows.Forms.GroupBox();
            this.propagandaPictureBox = new System.Windows.Forms.PictureBox();
            this.propagandaBrowseButton = new System.Windows.Forms.Button();
            this.propagandaTextBox = new System.Windows.Forms.TextBox();
            this.propagandaLabel = new System.Windows.Forms.Label();
            this.countryDescTextBox = new System.Windows.Forms.TextBox();
            this.countryDescLabel = new System.Windows.Forms.Label();
            this.selectableCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.majorAddButton = new System.Windows.Forms.Button();
            this.majorRemoveButton = new System.Windows.Forms.Button();
            this.majorListBox = new System.Windows.Forms.ListBox();
            this.majorLabel = new System.Windows.Forms.Label();
            this.selectableLabel = new System.Windows.Forms.Label();
            this.majorUpButton = new System.Windows.Forms.Button();
            this.majorDownButton = new System.Windows.Forms.Button();
            this.infoGroupBox = new System.Windows.Forms.GroupBox();
            this.includeFolderBrowseButton = new System.Windows.Forms.Button();
            this.includeFolderTextBox = new System.Windows.Forms.TextBox();
            this.includeFolderLabel = new System.Windows.Forms.Label();
            this.scenarioNameLabel = new System.Windows.Forms.Label();
            this.scenarioNameTextBox = new System.Windows.Forms.TextBox();
            this.panelPictureBox = new System.Windows.Forms.PictureBox();
            this.panelImageLabel = new System.Windows.Forms.Label();
            this.panelImageTextBox = new System.Windows.Forms.TextBox();
            this.panelImageBrowseButton = new System.Windows.Forms.Button();
            this.startDateLabel = new System.Windows.Forms.Label();
            this.startYearTextBox = new System.Windows.Forms.TextBox();
            this.startMonthTextBox = new System.Windows.Forms.TextBox();
            this.startDayTextBox = new System.Windows.Forms.TextBox();
            this.endDateLabel = new System.Windows.Forms.Label();
            this.endYearTextBox = new System.Windows.Forms.TextBox();
            this.endMonthTextBox = new System.Windows.Forms.TextBox();
            this.endDayTextBox = new System.Windows.Forms.TextBox();
            this.loadButton = new System.Windows.Forms.Button();
            this.folderGroupBox = new System.Windows.Forms.GroupBox();
            this.exportRadioButton = new System.Windows.Forms.RadioButton();
            this.modRadioButton = new System.Windows.Forms.RadioButton();
            this.vanillaRadioButton = new System.Windows.Forms.RadioButton();
            this.typeGroupBox = new System.Windows.Forms.GroupBox();
            this.saveGamesRadioButton = new System.Windows.Forms.RadioButton();
            this.scenarioRadioButton = new System.Windows.Forms.RadioButton();
            this.scenarioListBox = new System.Windows.Forms.ListBox();
            this.allianceTabPage = new System.Windows.Forms.TabPage();
            this.allianceParticipantLabel = new System.Windows.Forms.Label();
            this.allianceParticipantListBox = new System.Windows.Forms.ListBox();
            this.allianceCountryListBox = new System.Windows.Forms.ListBox();
            this.allianceListView = new System.Windows.Forms.ListView();
            this.allianceTypeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.allianceParticipantColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.participantRemoveButton = new System.Windows.Forms.Button();
            this.participantAddButton = new System.Windows.Forms.Button();
            this.allianceLeaderButton = new System.Windows.Forms.Button();
            this.allianceUpButton = new System.Windows.Forms.Button();
            this.allianceRemoveButton = new System.Windows.Forms.Button();
            this.allianceDownButton = new System.Windows.Forms.Button();
            this.allianceNewButton = new System.Windows.Forms.Button();
            this.relationTabPage = new System.Windows.Forms.TabPage();
            this.tradeTabPage = new System.Windows.Forms.TabPage();
            this.countryTabPage = new System.Windows.Forms.TabPage();
            this.personTabPage = new System.Windows.Forms.TabPage();
            this.techTabPage = new System.Windows.Forms.TabPage();
            this.provinceTabPage = new System.Windows.Forms.TabPage();
            this.provinceMapPanel = new System.Windows.Forms.Panel();
            this.provinceMapPictureBox = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.closeButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.reloadButton = new System.Windows.Forms.Button();
            this.allianceGroupBox = new System.Windows.Forms.GroupBox();
            this.warDefenderLabel = new System.Windows.Forms.Label();
            this.warListView = new System.Windows.Forms.ListView();
            this.warStartDateColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.warEndDateColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.warAttackerColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.warDefenderColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.warAttackerRemoveButton = new System.Windows.Forms.Button();
            this.warAttackerListBox = new System.Windows.Forms.ListBox();
            this.warAttackerLabel = new System.Windows.Forms.Label();
            this.warDownButton = new System.Windows.Forms.Button();
            this.warAttackerAddButton = new System.Windows.Forms.Button();
            this.warRemoveButton = new System.Windows.Forms.Button();
            this.warDefenderAddButton = new System.Windows.Forms.Button();
            this.warCountryListBox = new System.Windows.Forms.ListBox();
            this.warDefenderListBox = new System.Windows.Forms.ListBox();
            this.warDefenderRemoveButton = new System.Windows.Forms.Button();
            this.warUpButton = new System.Windows.Forms.Button();
            this.warNewButton = new System.Windows.Forms.Button();
            this.warGroupBox = new System.Windows.Forms.GroupBox();
            this.intelligenceGroupBox = new System.Windows.Forms.GroupBox();
            this.spyNumNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.spyNumLabel = new System.Windows.Forms.Label();
            this.diplomacyGroupBox = new System.Windows.Forms.GroupBox();
            this.peaceEndLabel = new System.Windows.Forms.Label();
            this.peaceEndDayTextBox = new System.Windows.Forms.TextBox();
            this.peaceEndMonthTextBox = new System.Windows.Forms.TextBox();
            this.peaceEndYearTextBox = new System.Windows.Forms.TextBox();
            this.peaceStartLabel = new System.Windows.Forms.Label();
            this.peaceStartDayTextBox = new System.Windows.Forms.TextBox();
            this.peaceCheckBox = new System.Windows.Forms.CheckBox();
            this.peaceStartYearTextBox = new System.Windows.Forms.TextBox();
            this.peaceStartMonthTextBox = new System.Windows.Forms.TextBox();
            this.accessCheckBox = new System.Windows.Forms.CheckBox();
            this.nonAggressionEndLabel = new System.Windows.Forms.Label();
            this.guaranteeCheckBox = new System.Windows.Forms.CheckBox();
            this.nonAggressionEndDayTextBox = new System.Windows.Forms.TextBox();
            this.guaranteeYearTextBox = new System.Windows.Forms.TextBox();
            this.nonAggressionEndMonthTextBox = new System.Windows.Forms.TextBox();
            this.guaranteeMonthTextBox = new System.Windows.Forms.TextBox();
            this.nonAggressionEndYearTextBox = new System.Windows.Forms.TextBox();
            this.guaranteeDayTextBox = new System.Windows.Forms.TextBox();
            this.nonAggressionStartLabel = new System.Windows.Forms.Label();
            this.puppetCheckBox = new System.Windows.Forms.CheckBox();
            this.guaraneeEndLabel = new System.Windows.Forms.Label();
            this.controlCheckBox = new System.Windows.Forms.CheckBox();
            this.nonAggressionStartDayTextBox = new System.Windows.Forms.TextBox();
            this.nonAggressionCheckBox = new System.Windows.Forms.CheckBox();
            this.nonAggressionStartYearTextBox = new System.Windows.Forms.TextBox();
            this.nonAggressionStartMonthTextBox = new System.Windows.Forms.TextBox();
            this.relationValueNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.relationValueLabel = new System.Windows.Forms.Label();
            this.relationListView = new System.Windows.Forms.ListView();
            this.relationCountryColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.relationValueColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.relationPuppetColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.relationControlColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.relationAccessColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.relationGuaranteeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.relationNonAggressionColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.relationPeaceColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.relationSpyColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.relationCountryListBox = new System.Windows.Forms.ListBox();
            this.tradeEndDayTextBox = new System.Windows.Forms.TextBox();
            this.tradeEndMonthTextBox = new System.Windows.Forms.TextBox();
            this.tradeListView = new System.Windows.Forms.ListView();
            this.tradeStartDateColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tradeEndDateColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tradeCountryColumnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tradeCountryColumnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tradeDealsColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tradeEndYearTextBox = new System.Windows.Forms.TextBox();
            this.tradeDownButton = new System.Windows.Forms.Button();
            this.tradeStartDayTextBox = new System.Windows.Forms.TextBox();
            this.tradeNewButton = new System.Windows.Forms.Button();
            this.tradeStartMonthTextBox = new System.Windows.Forms.TextBox();
            this.tradeRemoveButton = new System.Windows.Forms.Button();
            this.tradeStartYearTextBox = new System.Windows.Forms.TextBox();
            this.tradeUpButton = new System.Windows.Forms.Button();
            this.tradeCancelCheckBox = new System.Windows.Forms.CheckBox();
            this.tradeStartDateLabel = new System.Windows.Forms.Label();
            this.tradeSwapButton = new System.Windows.Forms.Button();
            this.tradeEndDateLabel = new System.Windows.Forms.Label();
            this.tradeMoneyTextBox2 = new System.Windows.Forms.TextBox();
            this.tradeCountryComboBox1 = new System.Windows.Forms.ComboBox();
            this.tradeMoneyTextBox1 = new System.Windows.Forms.TextBox();
            this.tradeCountryComboBox2 = new System.Windows.Forms.ComboBox();
            this.tradeMoneyLabel = new System.Windows.Forms.Label();
            this.tradeEnergyLabel = new System.Windows.Forms.Label();
            this.tradeSuppliesTextBox2 = new System.Windows.Forms.TextBox();
            this.tradeEnergyTextBox1 = new System.Windows.Forms.TextBox();
            this.tradeSuppliesTextBox1 = new System.Windows.Forms.TextBox();
            this.tradeEnergyTextBox2 = new System.Windows.Forms.TextBox();
            this.tradeSuppliesLabel = new System.Windows.Forms.Label();
            this.tradeMetalLabel = new System.Windows.Forms.Label();
            this.tradeOilTextBox2 = new System.Windows.Forms.TextBox();
            this.tradeMetalTextBox1 = new System.Windows.Forms.TextBox();
            this.tradeOilTextBox1 = new System.Windows.Forms.TextBox();
            this.tradeMetalTextBox2 = new System.Windows.Forms.TextBox();
            this.tradeOilLabel = new System.Windows.Forms.Label();
            this.tradeRareMaterialsLabel = new System.Windows.Forms.Label();
            this.tradeRareMaterialsTextBox2 = new System.Windows.Forms.TextBox();
            this.tradeRareMaterialsTextBox1 = new System.Windows.Forms.TextBox();
            this.nonAggressionGroupBox = new System.Windows.Forms.GroupBox();
            this.nonAggressionIdLabel = new System.Windows.Forms.Label();
            this.nonAggressionTypeTextBox = new System.Windows.Forms.TextBox();
            this.nonAggressionIdTextBox = new System.Windows.Forms.TextBox();
            this.peaceGroupBox = new System.Windows.Forms.GroupBox();
            this.peaceIdTextBox = new System.Windows.Forms.TextBox();
            this.peaceTypeTextBox = new System.Windows.Forms.TextBox();
            this.peaceIdLabel = new System.Windows.Forms.Label();
            this.guaranteeGroupBox = new System.Windows.Forms.GroupBox();
            this.regularTagLabel = new System.Windows.Forms.Label();
            this.regularTagComboBox = new System.Windows.Forms.ComboBox();
            this.tradeIdLabel = new System.Windows.Forms.Label();
            this.tradeIdTextBox = new System.Windows.Forms.TextBox();
            this.tradeTypeTextBox = new System.Windows.Forms.TextBox();
            this.tradeInfoGroupBox = new System.Windows.Forms.GroupBox();
            this.tradeDealGroupBox = new System.Windows.Forms.GroupBox();
            this.scenarioTabControl.SuspendLayout();
            this.mainTabPage.SuspendLayout();
            this.optionGroupBox.SuspendLayout();
            this.countryGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propagandaPictureBox)).BeginInit();
            this.infoGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelPictureBox)).BeginInit();
            this.folderGroupBox.SuspendLayout();
            this.typeGroupBox.SuspendLayout();
            this.allianceTabPage.SuspendLayout();
            this.relationTabPage.SuspendLayout();
            this.tradeTabPage.SuspendLayout();
            this.provinceTabPage.SuspendLayout();
            this.provinceMapPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.provinceMapPictureBox)).BeginInit();
            this.allianceGroupBox.SuspendLayout();
            this.warGroupBox.SuspendLayout();
            this.intelligenceGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spyNumNumericUpDown)).BeginInit();
            this.diplomacyGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.relationValueNumericUpDown)).BeginInit();
            this.nonAggressionGroupBox.SuspendLayout();
            this.peaceGroupBox.SuspendLayout();
            this.guaranteeGroupBox.SuspendLayout();
            this.tradeInfoGroupBox.SuspendLayout();
            this.tradeDealGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // scenarioTabControl
            // 
            resources.ApplyResources(this.scenarioTabControl, "scenarioTabControl");
            this.scenarioTabControl.Controls.Add(this.mainTabPage);
            this.scenarioTabControl.Controls.Add(this.allianceTabPage);
            this.scenarioTabControl.Controls.Add(this.relationTabPage);
            this.scenarioTabControl.Controls.Add(this.tradeTabPage);
            this.scenarioTabControl.Controls.Add(this.countryTabPage);
            this.scenarioTabControl.Controls.Add(this.personTabPage);
            this.scenarioTabControl.Controls.Add(this.techTabPage);
            this.scenarioTabControl.Controls.Add(this.provinceTabPage);
            this.scenarioTabControl.Name = "scenarioTabControl";
            this.scenarioTabControl.SelectedIndex = 0;
            // 
            // mainTabPage
            // 
            this.mainTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.mainTabPage.Controls.Add(this.optionGroupBox);
            this.mainTabPage.Controls.Add(this.countryGroupBox);
            this.mainTabPage.Controls.Add(this.infoGroupBox);
            this.mainTabPage.Controls.Add(this.loadButton);
            this.mainTabPage.Controls.Add(this.folderGroupBox);
            this.mainTabPage.Controls.Add(this.typeGroupBox);
            this.mainTabPage.Controls.Add(this.scenarioListBox);
            resources.ApplyResources(this.mainTabPage, "mainTabPage");
            this.mainTabPage.Name = "mainTabPage";
            // 
            // optionGroupBox
            // 
            this.optionGroupBox.Controls.Add(this.gameSpeedLabel);
            this.optionGroupBox.Controls.Add(this.difficultyLabel);
            this.optionGroupBox.Controls.Add(this.aiAggressiveLabel);
            this.optionGroupBox.Controls.Add(this.gameSpeedComboBox);
            this.optionGroupBox.Controls.Add(this.difficultyComboBox);
            this.optionGroupBox.Controls.Add(this.aiAggressiveComboBox);
            this.optionGroupBox.Controls.Add(this.allowTechnologyCheckBox);
            this.optionGroupBox.Controls.Add(this.allowProductionCheckBox);
            this.optionGroupBox.Controls.Add(this.allowDiplomacyCheckBox);
            this.optionGroupBox.Controls.Add(this.freeCountryCheckBox);
            this.optionGroupBox.Controls.Add(this.battleScenarioCheckBox);
            resources.ApplyResources(this.optionGroupBox, "optionGroupBox");
            this.optionGroupBox.Name = "optionGroupBox";
            this.optionGroupBox.TabStop = false;
            // 
            // gameSpeedLabel
            // 
            resources.ApplyResources(this.gameSpeedLabel, "gameSpeedLabel");
            this.gameSpeedLabel.Name = "gameSpeedLabel";
            // 
            // difficultyLabel
            // 
            resources.ApplyResources(this.difficultyLabel, "difficultyLabel");
            this.difficultyLabel.Name = "difficultyLabel";
            // 
            // aiAggressiveLabel
            // 
            resources.ApplyResources(this.aiAggressiveLabel, "aiAggressiveLabel");
            this.aiAggressiveLabel.Name = "aiAggressiveLabel";
            // 
            // gameSpeedComboBox
            // 
            this.gameSpeedComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.gameSpeedComboBox, "gameSpeedComboBox");
            this.gameSpeedComboBox.Name = "gameSpeedComboBox";
            // 
            // difficultyComboBox
            // 
            this.difficultyComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.difficultyComboBox, "difficultyComboBox");
            this.difficultyComboBox.Name = "difficultyComboBox";
            // 
            // aiAggressiveComboBox
            // 
            this.aiAggressiveComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.aiAggressiveComboBox, "aiAggressiveComboBox");
            this.aiAggressiveComboBox.Name = "aiAggressiveComboBox";
            // 
            // allowTechnologyCheckBox
            // 
            resources.ApplyResources(this.allowTechnologyCheckBox, "allowTechnologyCheckBox");
            this.allowTechnologyCheckBox.Name = "allowTechnologyCheckBox";
            this.allowTechnologyCheckBox.UseVisualStyleBackColor = true;
            // 
            // allowProductionCheckBox
            // 
            resources.ApplyResources(this.allowProductionCheckBox, "allowProductionCheckBox");
            this.allowProductionCheckBox.Name = "allowProductionCheckBox";
            this.allowProductionCheckBox.UseVisualStyleBackColor = true;
            // 
            // allowDiplomacyCheckBox
            // 
            resources.ApplyResources(this.allowDiplomacyCheckBox, "allowDiplomacyCheckBox");
            this.allowDiplomacyCheckBox.Name = "allowDiplomacyCheckBox";
            this.allowDiplomacyCheckBox.UseVisualStyleBackColor = true;
            // 
            // freeCountryCheckBox
            // 
            resources.ApplyResources(this.freeCountryCheckBox, "freeCountryCheckBox");
            this.freeCountryCheckBox.Name = "freeCountryCheckBox";
            this.freeCountryCheckBox.UseVisualStyleBackColor = true;
            // 
            // battleScenarioCheckBox
            // 
            resources.ApplyResources(this.battleScenarioCheckBox, "battleScenarioCheckBox");
            this.battleScenarioCheckBox.Name = "battleScenarioCheckBox";
            this.battleScenarioCheckBox.UseVisualStyleBackColor = true;
            // 
            // countryGroupBox
            // 
            this.countryGroupBox.Controls.Add(this.propagandaPictureBox);
            this.countryGroupBox.Controls.Add(this.propagandaBrowseButton);
            this.countryGroupBox.Controls.Add(this.propagandaTextBox);
            this.countryGroupBox.Controls.Add(this.propagandaLabel);
            this.countryGroupBox.Controls.Add(this.countryDescTextBox);
            this.countryGroupBox.Controls.Add(this.countryDescLabel);
            this.countryGroupBox.Controls.Add(this.selectableCheckedListBox);
            this.countryGroupBox.Controls.Add(this.majorAddButton);
            this.countryGroupBox.Controls.Add(this.majorRemoveButton);
            this.countryGroupBox.Controls.Add(this.majorListBox);
            this.countryGroupBox.Controls.Add(this.majorLabel);
            this.countryGroupBox.Controls.Add(this.selectableLabel);
            this.countryGroupBox.Controls.Add(this.majorUpButton);
            this.countryGroupBox.Controls.Add(this.majorDownButton);
            resources.ApplyResources(this.countryGroupBox, "countryGroupBox");
            this.countryGroupBox.Name = "countryGroupBox";
            this.countryGroupBox.TabStop = false;
            // 
            // propagandaPictureBox
            // 
            resources.ApplyResources(this.propagandaPictureBox, "propagandaPictureBox");
            this.propagandaPictureBox.Name = "propagandaPictureBox";
            this.propagandaPictureBox.TabStop = false;
            // 
            // propagandaBrowseButton
            // 
            resources.ApplyResources(this.propagandaBrowseButton, "propagandaBrowseButton");
            this.propagandaBrowseButton.Name = "propagandaBrowseButton";
            this.propagandaBrowseButton.UseVisualStyleBackColor = true;
            // 
            // propagandaTextBox
            // 
            resources.ApplyResources(this.propagandaTextBox, "propagandaTextBox");
            this.propagandaTextBox.Name = "propagandaTextBox";
            // 
            // propagandaLabel
            // 
            resources.ApplyResources(this.propagandaLabel, "propagandaLabel");
            this.propagandaLabel.Name = "propagandaLabel";
            // 
            // countryDescTextBox
            // 
            resources.ApplyResources(this.countryDescTextBox, "countryDescTextBox");
            this.countryDescTextBox.Name = "countryDescTextBox";
            // 
            // countryDescLabel
            // 
            resources.ApplyResources(this.countryDescLabel, "countryDescLabel");
            this.countryDescLabel.Name = "countryDescLabel";
            // 
            // selectableCheckedListBox
            // 
            resources.ApplyResources(this.selectableCheckedListBox, "selectableCheckedListBox");
            this.selectableCheckedListBox.FormattingEnabled = true;
            this.selectableCheckedListBox.Name = "selectableCheckedListBox";
            // 
            // majorAddButton
            // 
            resources.ApplyResources(this.majorAddButton, "majorAddButton");
            this.majorAddButton.Name = "majorAddButton";
            this.majorAddButton.UseVisualStyleBackColor = true;
            // 
            // majorRemoveButton
            // 
            resources.ApplyResources(this.majorRemoveButton, "majorRemoveButton");
            this.majorRemoveButton.Name = "majorRemoveButton";
            this.majorRemoveButton.UseVisualStyleBackColor = true;
            // 
            // majorListBox
            // 
            resources.ApplyResources(this.majorListBox, "majorListBox");
            this.majorListBox.FormattingEnabled = true;
            this.majorListBox.Name = "majorListBox";
            this.majorListBox.SelectedIndexChanged += new System.EventHandler(this.OnMajorListBoxSelectedIndexChanged);
            // 
            // majorLabel
            // 
            resources.ApplyResources(this.majorLabel, "majorLabel");
            this.majorLabel.Name = "majorLabel";
            // 
            // selectableLabel
            // 
            resources.ApplyResources(this.selectableLabel, "selectableLabel");
            this.selectableLabel.Name = "selectableLabel";
            // 
            // majorUpButton
            // 
            resources.ApplyResources(this.majorUpButton, "majorUpButton");
            this.majorUpButton.Name = "majorUpButton";
            this.majorUpButton.UseVisualStyleBackColor = true;
            // 
            // majorDownButton
            // 
            resources.ApplyResources(this.majorDownButton, "majorDownButton");
            this.majorDownButton.Name = "majorDownButton";
            this.majorDownButton.UseVisualStyleBackColor = true;
            // 
            // infoGroupBox
            // 
            this.infoGroupBox.Controls.Add(this.includeFolderBrowseButton);
            this.infoGroupBox.Controls.Add(this.includeFolderTextBox);
            this.infoGroupBox.Controls.Add(this.includeFolderLabel);
            this.infoGroupBox.Controls.Add(this.scenarioNameLabel);
            this.infoGroupBox.Controls.Add(this.scenarioNameTextBox);
            this.infoGroupBox.Controls.Add(this.panelPictureBox);
            this.infoGroupBox.Controls.Add(this.panelImageLabel);
            this.infoGroupBox.Controls.Add(this.panelImageTextBox);
            this.infoGroupBox.Controls.Add(this.panelImageBrowseButton);
            this.infoGroupBox.Controls.Add(this.startDateLabel);
            this.infoGroupBox.Controls.Add(this.startYearTextBox);
            this.infoGroupBox.Controls.Add(this.startMonthTextBox);
            this.infoGroupBox.Controls.Add(this.startDayTextBox);
            this.infoGroupBox.Controls.Add(this.endDateLabel);
            this.infoGroupBox.Controls.Add(this.endYearTextBox);
            this.infoGroupBox.Controls.Add(this.endMonthTextBox);
            this.infoGroupBox.Controls.Add(this.endDayTextBox);
            resources.ApplyResources(this.infoGroupBox, "infoGroupBox");
            this.infoGroupBox.Name = "infoGroupBox";
            this.infoGroupBox.TabStop = false;
            // 
            // includeFolderBrowseButton
            // 
            resources.ApplyResources(this.includeFolderBrowseButton, "includeFolderBrowseButton");
            this.includeFolderBrowseButton.Name = "includeFolderBrowseButton";
            this.includeFolderBrowseButton.UseVisualStyleBackColor = true;
            // 
            // includeFolderTextBox
            // 
            resources.ApplyResources(this.includeFolderTextBox, "includeFolderTextBox");
            this.includeFolderTextBox.Name = "includeFolderTextBox";
            // 
            // includeFolderLabel
            // 
            resources.ApplyResources(this.includeFolderLabel, "includeFolderLabel");
            this.includeFolderLabel.Name = "includeFolderLabel";
            // 
            // scenarioNameLabel
            // 
            resources.ApplyResources(this.scenarioNameLabel, "scenarioNameLabel");
            this.scenarioNameLabel.Name = "scenarioNameLabel";
            // 
            // scenarioNameTextBox
            // 
            resources.ApplyResources(this.scenarioNameTextBox, "scenarioNameTextBox");
            this.scenarioNameTextBox.Name = "scenarioNameTextBox";
            // 
            // panelPictureBox
            // 
            resources.ApplyResources(this.panelPictureBox, "panelPictureBox");
            this.panelPictureBox.Name = "panelPictureBox";
            this.panelPictureBox.TabStop = false;
            // 
            // panelImageLabel
            // 
            resources.ApplyResources(this.panelImageLabel, "panelImageLabel");
            this.panelImageLabel.Name = "panelImageLabel";
            // 
            // panelImageTextBox
            // 
            resources.ApplyResources(this.panelImageTextBox, "panelImageTextBox");
            this.panelImageTextBox.Name = "panelImageTextBox";
            // 
            // panelImageBrowseButton
            // 
            resources.ApplyResources(this.panelImageBrowseButton, "panelImageBrowseButton");
            this.panelImageBrowseButton.Name = "panelImageBrowseButton";
            this.panelImageBrowseButton.UseVisualStyleBackColor = true;
            // 
            // startDateLabel
            // 
            resources.ApplyResources(this.startDateLabel, "startDateLabel");
            this.startDateLabel.Name = "startDateLabel";
            // 
            // startYearTextBox
            // 
            resources.ApplyResources(this.startYearTextBox, "startYearTextBox");
            this.startYearTextBox.Name = "startYearTextBox";
            // 
            // startMonthTextBox
            // 
            resources.ApplyResources(this.startMonthTextBox, "startMonthTextBox");
            this.startMonthTextBox.Name = "startMonthTextBox";
            // 
            // startDayTextBox
            // 
            resources.ApplyResources(this.startDayTextBox, "startDayTextBox");
            this.startDayTextBox.Name = "startDayTextBox";
            // 
            // endDateLabel
            // 
            resources.ApplyResources(this.endDateLabel, "endDateLabel");
            this.endDateLabel.Name = "endDateLabel";
            // 
            // endYearTextBox
            // 
            resources.ApplyResources(this.endYearTextBox, "endYearTextBox");
            this.endYearTextBox.Name = "endYearTextBox";
            // 
            // endMonthTextBox
            // 
            resources.ApplyResources(this.endMonthTextBox, "endMonthTextBox");
            this.endMonthTextBox.Name = "endMonthTextBox";
            // 
            // endDayTextBox
            // 
            resources.ApplyResources(this.endDayTextBox, "endDayTextBox");
            this.endDayTextBox.Name = "endDayTextBox";
            // 
            // loadButton
            // 
            resources.ApplyResources(this.loadButton, "loadButton");
            this.loadButton.Name = "loadButton";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.OnLoadButtonClick);
            // 
            // folderGroupBox
            // 
            this.folderGroupBox.Controls.Add(this.exportRadioButton);
            this.folderGroupBox.Controls.Add(this.modRadioButton);
            this.folderGroupBox.Controls.Add(this.vanillaRadioButton);
            resources.ApplyResources(this.folderGroupBox, "folderGroupBox");
            this.folderGroupBox.Name = "folderGroupBox";
            this.folderGroupBox.TabStop = false;
            // 
            // exportRadioButton
            // 
            resources.ApplyResources(this.exportRadioButton, "exportRadioButton");
            this.exportRadioButton.Name = "exportRadioButton";
            this.exportRadioButton.TabStop = true;
            this.exportRadioButton.UseVisualStyleBackColor = true;
            this.exportRadioButton.CheckedChanged += new System.EventHandler(this.OnFolderRadioButtonCheckedChanged);
            // 
            // modRadioButton
            // 
            resources.ApplyResources(this.modRadioButton, "modRadioButton");
            this.modRadioButton.Name = "modRadioButton";
            this.modRadioButton.TabStop = true;
            this.modRadioButton.UseVisualStyleBackColor = true;
            this.modRadioButton.CheckedChanged += new System.EventHandler(this.OnFolderRadioButtonCheckedChanged);
            // 
            // vanillaRadioButton
            // 
            resources.ApplyResources(this.vanillaRadioButton, "vanillaRadioButton");
            this.vanillaRadioButton.Name = "vanillaRadioButton";
            this.vanillaRadioButton.TabStop = true;
            this.vanillaRadioButton.UseVisualStyleBackColor = true;
            this.vanillaRadioButton.CheckedChanged += new System.EventHandler(this.OnFolderRadioButtonCheckedChanged);
            // 
            // typeGroupBox
            // 
            this.typeGroupBox.Controls.Add(this.saveGamesRadioButton);
            this.typeGroupBox.Controls.Add(this.scenarioRadioButton);
            resources.ApplyResources(this.typeGroupBox, "typeGroupBox");
            this.typeGroupBox.Name = "typeGroupBox";
            this.typeGroupBox.TabStop = false;
            // 
            // saveGamesRadioButton
            // 
            resources.ApplyResources(this.saveGamesRadioButton, "saveGamesRadioButton");
            this.saveGamesRadioButton.Name = "saveGamesRadioButton";
            this.saveGamesRadioButton.TabStop = true;
            this.saveGamesRadioButton.UseVisualStyleBackColor = true;
            // 
            // scenarioRadioButton
            // 
            resources.ApplyResources(this.scenarioRadioButton, "scenarioRadioButton");
            this.scenarioRadioButton.Checked = true;
            this.scenarioRadioButton.Name = "scenarioRadioButton";
            this.scenarioRadioButton.TabStop = true;
            this.scenarioRadioButton.UseVisualStyleBackColor = true;
            // 
            // scenarioListBox
            // 
            this.scenarioListBox.FormattingEnabled = true;
            resources.ApplyResources(this.scenarioListBox, "scenarioListBox");
            this.scenarioListBox.Name = "scenarioListBox";
            // 
            // allianceTabPage
            // 
            resources.ApplyResources(this.allianceTabPage, "allianceTabPage");
            this.allianceTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.allianceTabPage.Controls.Add(this.warGroupBox);
            this.allianceTabPage.Controls.Add(this.allianceGroupBox);
            this.allianceTabPage.Name = "allianceTabPage";
            // 
            // allianceParticipantLabel
            // 
            resources.ApplyResources(this.allianceParticipantLabel, "allianceParticipantLabel");
            this.allianceParticipantLabel.Name = "allianceParticipantLabel";
            // 
            // allianceParticipantListBox
            // 
            this.allianceParticipantListBox.FormattingEnabled = true;
            resources.ApplyResources(this.allianceParticipantListBox, "allianceParticipantListBox");
            this.allianceParticipantListBox.Name = "allianceParticipantListBox";
            // 
            // allianceCountryListBox
            // 
            this.allianceCountryListBox.FormattingEnabled = true;
            resources.ApplyResources(this.allianceCountryListBox, "allianceCountryListBox");
            this.allianceCountryListBox.Name = "allianceCountryListBox";
            // 
            // allianceListView
            // 
            this.allianceListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.allianceTypeColumnHeader,
            this.allianceParticipantColumnHeader});
            this.allianceListView.FullRowSelect = true;
            this.allianceListView.GridLines = true;
            resources.ApplyResources(this.allianceListView, "allianceListView");
            this.allianceListView.Name = "allianceListView";
            this.allianceListView.UseCompatibleStateImageBehavior = false;
            this.allianceListView.View = System.Windows.Forms.View.Details;
            this.allianceListView.SelectedIndexChanged += new System.EventHandler(this.OnAllianceListViewSelectedIndexChanged);
            // 
            // allianceTypeColumnHeader
            // 
            resources.ApplyResources(this.allianceTypeColumnHeader, "allianceTypeColumnHeader");
            // 
            // allianceParticipantColumnHeader
            // 
            resources.ApplyResources(this.allianceParticipantColumnHeader, "allianceParticipantColumnHeader");
            // 
            // participantRemoveButton
            // 
            resources.ApplyResources(this.participantRemoveButton, "participantRemoveButton");
            this.participantRemoveButton.Name = "participantRemoveButton";
            this.participantRemoveButton.UseVisualStyleBackColor = true;
            // 
            // participantAddButton
            // 
            resources.ApplyResources(this.participantAddButton, "participantAddButton");
            this.participantAddButton.Name = "participantAddButton";
            this.participantAddButton.UseVisualStyleBackColor = true;
            // 
            // allianceLeaderButton
            // 
            resources.ApplyResources(this.allianceLeaderButton, "allianceLeaderButton");
            this.allianceLeaderButton.Name = "allianceLeaderButton";
            this.allianceLeaderButton.UseVisualStyleBackColor = true;
            // 
            // allianceUpButton
            // 
            resources.ApplyResources(this.allianceUpButton, "allianceUpButton");
            this.allianceUpButton.Name = "allianceUpButton";
            this.allianceUpButton.UseVisualStyleBackColor = true;
            // 
            // allianceRemoveButton
            // 
            resources.ApplyResources(this.allianceRemoveButton, "allianceRemoveButton");
            this.allianceRemoveButton.Name = "allianceRemoveButton";
            this.allianceRemoveButton.UseVisualStyleBackColor = true;
            // 
            // allianceDownButton
            // 
            resources.ApplyResources(this.allianceDownButton, "allianceDownButton");
            this.allianceDownButton.Name = "allianceDownButton";
            this.allianceDownButton.UseVisualStyleBackColor = true;
            // 
            // allianceNewButton
            // 
            resources.ApplyResources(this.allianceNewButton, "allianceNewButton");
            this.allianceNewButton.Name = "allianceNewButton";
            this.allianceNewButton.UseVisualStyleBackColor = true;
            // 
            // relationTabPage
            // 
            this.relationTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.relationTabPage.Controls.Add(this.intelligenceGroupBox);
            this.relationTabPage.Controls.Add(this.diplomacyGroupBox);
            this.relationTabPage.Controls.Add(this.relationListView);
            this.relationTabPage.Controls.Add(this.relationCountryListBox);
            resources.ApplyResources(this.relationTabPage, "relationTabPage");
            this.relationTabPage.Name = "relationTabPage";
            // 
            // tradeTabPage
            // 
            this.tradeTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.tradeTabPage.Controls.Add(this.tradeDealGroupBox);
            this.tradeTabPage.Controls.Add(this.tradeInfoGroupBox);
            this.tradeTabPage.Controls.Add(this.tradeListView);
            this.tradeTabPage.Controls.Add(this.tradeDownButton);
            this.tradeTabPage.Controls.Add(this.tradeNewButton);
            this.tradeTabPage.Controls.Add(this.tradeRemoveButton);
            this.tradeTabPage.Controls.Add(this.tradeUpButton);
            resources.ApplyResources(this.tradeTabPage, "tradeTabPage");
            this.tradeTabPage.Name = "tradeTabPage";
            // 
            // countryTabPage
            // 
            this.countryTabPage.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.countryTabPage, "countryTabPage");
            this.countryTabPage.Name = "countryTabPage";
            // 
            // personTabPage
            // 
            this.personTabPage.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.personTabPage, "personTabPage");
            this.personTabPage.Name = "personTabPage";
            // 
            // techTabPage
            // 
            this.techTabPage.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.techTabPage, "techTabPage");
            this.techTabPage.Name = "techTabPage";
            // 
            // provinceTabPage
            // 
            this.provinceTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.provinceTabPage.Controls.Add(this.provinceMapPanel);
            this.provinceTabPage.Controls.Add(this.textBox1);
            resources.ApplyResources(this.provinceTabPage, "provinceTabPage");
            this.provinceTabPage.Name = "provinceTabPage";
            // 
            // provinceMapPanel
            // 
            resources.ApplyResources(this.provinceMapPanel, "provinceMapPanel");
            this.provinceMapPanel.Controls.Add(this.provinceMapPictureBox);
            this.provinceMapPanel.Name = "provinceMapPanel";
            // 
            // provinceMapPictureBox
            // 
            resources.ApplyResources(this.provinceMapPictureBox, "provinceMapPictureBox");
            this.provinceMapPictureBox.Name = "provinceMapPictureBox";
            this.provinceMapPictureBox.TabStop = false;
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            this.textBox1.Validated += new System.EventHandler(this.OnTextBox1Validated);
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
            // allianceGroupBox
            // 
            this.allianceGroupBox.Controls.Add(this.allianceListView);
            this.allianceGroupBox.Controls.Add(this.allianceParticipantLabel);
            this.allianceGroupBox.Controls.Add(this.allianceLeaderButton);
            this.allianceGroupBox.Controls.Add(this.allianceParticipantListBox);
            this.allianceGroupBox.Controls.Add(this.allianceUpButton);
            this.allianceGroupBox.Controls.Add(this.participantAddButton);
            this.allianceGroupBox.Controls.Add(this.allianceCountryListBox);
            this.allianceGroupBox.Controls.Add(this.allianceRemoveButton);
            this.allianceGroupBox.Controls.Add(this.allianceNewButton);
            this.allianceGroupBox.Controls.Add(this.participantRemoveButton);
            this.allianceGroupBox.Controls.Add(this.allianceDownButton);
            resources.ApplyResources(this.allianceGroupBox, "allianceGroupBox");
            this.allianceGroupBox.Name = "allianceGroupBox";
            this.allianceGroupBox.TabStop = false;
            // 
            // warDefenderLabel
            // 
            resources.ApplyResources(this.warDefenderLabel, "warDefenderLabel");
            this.warDefenderLabel.Name = "warDefenderLabel";
            // 
            // warListView
            // 
            this.warListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.warStartDateColumnHeader,
            this.warEndDateColumnHeader,
            this.warAttackerColumnHeader,
            this.warDefenderColumnHeader});
            this.warListView.FullRowSelect = true;
            this.warListView.GridLines = true;
            resources.ApplyResources(this.warListView, "warListView");
            this.warListView.Name = "warListView";
            this.warListView.UseCompatibleStateImageBehavior = false;
            this.warListView.View = System.Windows.Forms.View.Details;
            // 
            // warStartDateColumnHeader
            // 
            resources.ApplyResources(this.warStartDateColumnHeader, "warStartDateColumnHeader");
            // 
            // warEndDateColumnHeader
            // 
            resources.ApplyResources(this.warEndDateColumnHeader, "warEndDateColumnHeader");
            // 
            // warAttackerColumnHeader
            // 
            resources.ApplyResources(this.warAttackerColumnHeader, "warAttackerColumnHeader");
            // 
            // warDefenderColumnHeader
            // 
            resources.ApplyResources(this.warDefenderColumnHeader, "warDefenderColumnHeader");
            // 
            // warAttackerRemoveButton
            // 
            resources.ApplyResources(this.warAttackerRemoveButton, "warAttackerRemoveButton");
            this.warAttackerRemoveButton.Name = "warAttackerRemoveButton";
            this.warAttackerRemoveButton.UseVisualStyleBackColor = true;
            // 
            // warAttackerListBox
            // 
            this.warAttackerListBox.FormattingEnabled = true;
            resources.ApplyResources(this.warAttackerListBox, "warAttackerListBox");
            this.warAttackerListBox.Name = "warAttackerListBox";
            // 
            // warAttackerLabel
            // 
            resources.ApplyResources(this.warAttackerLabel, "warAttackerLabel");
            this.warAttackerLabel.Name = "warAttackerLabel";
            // 
            // warDownButton
            // 
            resources.ApplyResources(this.warDownButton, "warDownButton");
            this.warDownButton.Name = "warDownButton";
            this.warDownButton.UseVisualStyleBackColor = true;
            // 
            // warAttackerAddButton
            // 
            resources.ApplyResources(this.warAttackerAddButton, "warAttackerAddButton");
            this.warAttackerAddButton.Name = "warAttackerAddButton";
            this.warAttackerAddButton.UseVisualStyleBackColor = true;
            // 
            // warRemoveButton
            // 
            resources.ApplyResources(this.warRemoveButton, "warRemoveButton");
            this.warRemoveButton.Name = "warRemoveButton";
            this.warRemoveButton.UseVisualStyleBackColor = true;
            // 
            // warDefenderAddButton
            // 
            resources.ApplyResources(this.warDefenderAddButton, "warDefenderAddButton");
            this.warDefenderAddButton.Name = "warDefenderAddButton";
            this.warDefenderAddButton.UseVisualStyleBackColor = true;
            // 
            // warCountryListBox
            // 
            this.warCountryListBox.FormattingEnabled = true;
            resources.ApplyResources(this.warCountryListBox, "warCountryListBox");
            this.warCountryListBox.Name = "warCountryListBox";
            // 
            // warDefenderListBox
            // 
            this.warDefenderListBox.FormattingEnabled = true;
            resources.ApplyResources(this.warDefenderListBox, "warDefenderListBox");
            this.warDefenderListBox.Name = "warDefenderListBox";
            // 
            // warDefenderRemoveButton
            // 
            resources.ApplyResources(this.warDefenderRemoveButton, "warDefenderRemoveButton");
            this.warDefenderRemoveButton.Name = "warDefenderRemoveButton";
            this.warDefenderRemoveButton.UseVisualStyleBackColor = true;
            // 
            // warUpButton
            // 
            resources.ApplyResources(this.warUpButton, "warUpButton");
            this.warUpButton.Name = "warUpButton";
            this.warUpButton.UseVisualStyleBackColor = true;
            // 
            // warNewButton
            // 
            resources.ApplyResources(this.warNewButton, "warNewButton");
            this.warNewButton.Name = "warNewButton";
            this.warNewButton.UseVisualStyleBackColor = true;
            // 
            // warGroupBox
            // 
            this.warGroupBox.Controls.Add(this.warDefenderLabel);
            this.warGroupBox.Controls.Add(this.warListView);
            this.warGroupBox.Controls.Add(this.warNewButton);
            this.warGroupBox.Controls.Add(this.warAttackerRemoveButton);
            this.warGroupBox.Controls.Add(this.warUpButton);
            this.warGroupBox.Controls.Add(this.warAttackerListBox);
            this.warGroupBox.Controls.Add(this.warDefenderRemoveButton);
            this.warGroupBox.Controls.Add(this.warAttackerLabel);
            this.warGroupBox.Controls.Add(this.warDefenderListBox);
            this.warGroupBox.Controls.Add(this.warDownButton);
            this.warGroupBox.Controls.Add(this.warCountryListBox);
            this.warGroupBox.Controls.Add(this.warAttackerAddButton);
            this.warGroupBox.Controls.Add(this.warDefenderAddButton);
            this.warGroupBox.Controls.Add(this.warRemoveButton);
            resources.ApplyResources(this.warGroupBox, "warGroupBox");
            this.warGroupBox.Name = "warGroupBox";
            this.warGroupBox.TabStop = false;
            // 
            // intelligenceGroupBox
            // 
            this.intelligenceGroupBox.Controls.Add(this.spyNumNumericUpDown);
            this.intelligenceGroupBox.Controls.Add(this.spyNumLabel);
            resources.ApplyResources(this.intelligenceGroupBox, "intelligenceGroupBox");
            this.intelligenceGroupBox.Name = "intelligenceGroupBox";
            this.intelligenceGroupBox.TabStop = false;
            // 
            // spyNumNumericUpDown
            // 
            resources.ApplyResources(this.spyNumNumericUpDown, "spyNumNumericUpDown");
            this.spyNumNumericUpDown.Name = "spyNumNumericUpDown";
            // 
            // spyNumLabel
            // 
            resources.ApplyResources(this.spyNumLabel, "spyNumLabel");
            this.spyNumLabel.Name = "spyNumLabel";
            // 
            // diplomacyGroupBox
            // 
            this.diplomacyGroupBox.Controls.Add(this.regularTagComboBox);
            this.diplomacyGroupBox.Controls.Add(this.regularTagLabel);
            this.diplomacyGroupBox.Controls.Add(this.guaranteeGroupBox);
            this.diplomacyGroupBox.Controls.Add(this.peaceGroupBox);
            this.diplomacyGroupBox.Controls.Add(this.relationValueNumericUpDown);
            this.diplomacyGroupBox.Controls.Add(this.nonAggressionGroupBox);
            this.diplomacyGroupBox.Controls.Add(this.relationValueLabel);
            this.diplomacyGroupBox.Controls.Add(this.accessCheckBox);
            this.diplomacyGroupBox.Controls.Add(this.puppetCheckBox);
            this.diplomacyGroupBox.Controls.Add(this.controlCheckBox);
            resources.ApplyResources(this.diplomacyGroupBox, "diplomacyGroupBox");
            this.diplomacyGroupBox.Name = "diplomacyGroupBox";
            this.diplomacyGroupBox.TabStop = false;
            // 
            // peaceEndLabel
            // 
            resources.ApplyResources(this.peaceEndLabel, "peaceEndLabel");
            this.peaceEndLabel.Name = "peaceEndLabel";
            // 
            // peaceEndDayTextBox
            // 
            resources.ApplyResources(this.peaceEndDayTextBox, "peaceEndDayTextBox");
            this.peaceEndDayTextBox.Name = "peaceEndDayTextBox";
            // 
            // peaceEndMonthTextBox
            // 
            resources.ApplyResources(this.peaceEndMonthTextBox, "peaceEndMonthTextBox");
            this.peaceEndMonthTextBox.Name = "peaceEndMonthTextBox";
            // 
            // peaceEndYearTextBox
            // 
            resources.ApplyResources(this.peaceEndYearTextBox, "peaceEndYearTextBox");
            this.peaceEndYearTextBox.Name = "peaceEndYearTextBox";
            // 
            // peaceStartLabel
            // 
            resources.ApplyResources(this.peaceStartLabel, "peaceStartLabel");
            this.peaceStartLabel.Name = "peaceStartLabel";
            // 
            // peaceStartDayTextBox
            // 
            resources.ApplyResources(this.peaceStartDayTextBox, "peaceStartDayTextBox");
            this.peaceStartDayTextBox.Name = "peaceStartDayTextBox";
            // 
            // peaceCheckBox
            // 
            resources.ApplyResources(this.peaceCheckBox, "peaceCheckBox");
            this.peaceCheckBox.Name = "peaceCheckBox";
            this.peaceCheckBox.UseVisualStyleBackColor = true;
            // 
            // peaceStartYearTextBox
            // 
            resources.ApplyResources(this.peaceStartYearTextBox, "peaceStartYearTextBox");
            this.peaceStartYearTextBox.Name = "peaceStartYearTextBox";
            // 
            // peaceStartMonthTextBox
            // 
            resources.ApplyResources(this.peaceStartMonthTextBox, "peaceStartMonthTextBox");
            this.peaceStartMonthTextBox.Name = "peaceStartMonthTextBox";
            // 
            // accessCheckBox
            // 
            resources.ApplyResources(this.accessCheckBox, "accessCheckBox");
            this.accessCheckBox.Name = "accessCheckBox";
            this.accessCheckBox.UseVisualStyleBackColor = true;
            // 
            // nonAggressionEndLabel
            // 
            resources.ApplyResources(this.nonAggressionEndLabel, "nonAggressionEndLabel");
            this.nonAggressionEndLabel.Name = "nonAggressionEndLabel";
            // 
            // guaranteeCheckBox
            // 
            resources.ApplyResources(this.guaranteeCheckBox, "guaranteeCheckBox");
            this.guaranteeCheckBox.Name = "guaranteeCheckBox";
            this.guaranteeCheckBox.UseVisualStyleBackColor = true;
            // 
            // nonAggressionEndDayTextBox
            // 
            resources.ApplyResources(this.nonAggressionEndDayTextBox, "nonAggressionEndDayTextBox");
            this.nonAggressionEndDayTextBox.Name = "nonAggressionEndDayTextBox";
            // 
            // guaranteeYearTextBox
            // 
            resources.ApplyResources(this.guaranteeYearTextBox, "guaranteeYearTextBox");
            this.guaranteeYearTextBox.Name = "guaranteeYearTextBox";
            // 
            // nonAggressionEndMonthTextBox
            // 
            resources.ApplyResources(this.nonAggressionEndMonthTextBox, "nonAggressionEndMonthTextBox");
            this.nonAggressionEndMonthTextBox.Name = "nonAggressionEndMonthTextBox";
            // 
            // guaranteeMonthTextBox
            // 
            resources.ApplyResources(this.guaranteeMonthTextBox, "guaranteeMonthTextBox");
            this.guaranteeMonthTextBox.Name = "guaranteeMonthTextBox";
            // 
            // nonAggressionEndYearTextBox
            // 
            resources.ApplyResources(this.nonAggressionEndYearTextBox, "nonAggressionEndYearTextBox");
            this.nonAggressionEndYearTextBox.Name = "nonAggressionEndYearTextBox";
            // 
            // guaranteeDayTextBox
            // 
            resources.ApplyResources(this.guaranteeDayTextBox, "guaranteeDayTextBox");
            this.guaranteeDayTextBox.Name = "guaranteeDayTextBox";
            // 
            // nonAggressionStartLabel
            // 
            resources.ApplyResources(this.nonAggressionStartLabel, "nonAggressionStartLabel");
            this.nonAggressionStartLabel.Name = "nonAggressionStartLabel";
            // 
            // puppetCheckBox
            // 
            resources.ApplyResources(this.puppetCheckBox, "puppetCheckBox");
            this.puppetCheckBox.Name = "puppetCheckBox";
            this.puppetCheckBox.UseVisualStyleBackColor = true;
            // 
            // guaraneeEndLabel
            // 
            resources.ApplyResources(this.guaraneeEndLabel, "guaraneeEndLabel");
            this.guaraneeEndLabel.Name = "guaraneeEndLabel";
            // 
            // controlCheckBox
            // 
            resources.ApplyResources(this.controlCheckBox, "controlCheckBox");
            this.controlCheckBox.Name = "controlCheckBox";
            this.controlCheckBox.UseVisualStyleBackColor = true;
            // 
            // nonAggressionStartDayTextBox
            // 
            resources.ApplyResources(this.nonAggressionStartDayTextBox, "nonAggressionStartDayTextBox");
            this.nonAggressionStartDayTextBox.Name = "nonAggressionStartDayTextBox";
            // 
            // nonAggressionCheckBox
            // 
            resources.ApplyResources(this.nonAggressionCheckBox, "nonAggressionCheckBox");
            this.nonAggressionCheckBox.Name = "nonAggressionCheckBox";
            this.nonAggressionCheckBox.UseVisualStyleBackColor = true;
            // 
            // nonAggressionStartYearTextBox
            // 
            resources.ApplyResources(this.nonAggressionStartYearTextBox, "nonAggressionStartYearTextBox");
            this.nonAggressionStartYearTextBox.Name = "nonAggressionStartYearTextBox";
            // 
            // nonAggressionStartMonthTextBox
            // 
            resources.ApplyResources(this.nonAggressionStartMonthTextBox, "nonAggressionStartMonthTextBox");
            this.nonAggressionStartMonthTextBox.Name = "nonAggressionStartMonthTextBox";
            // 
            // relationValueNumericUpDown
            // 
            resources.ApplyResources(this.relationValueNumericUpDown, "relationValueNumericUpDown");
            this.relationValueNumericUpDown.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.relationValueNumericUpDown.Minimum = new decimal(new int[] {
            200,
            0,
            0,
            -2147483648});
            this.relationValueNumericUpDown.Name = "relationValueNumericUpDown";
            // 
            // relationValueLabel
            // 
            resources.ApplyResources(this.relationValueLabel, "relationValueLabel");
            this.relationValueLabel.Name = "relationValueLabel";
            // 
            // relationListView
            // 
            this.relationListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.relationCountryColumnHeader,
            this.relationValueColumnHeader,
            this.relationPuppetColumnHeader,
            this.relationControlColumnHeader,
            this.relationAccessColumnHeader,
            this.relationGuaranteeColumnHeader,
            this.relationNonAggressionColumnHeader,
            this.relationPeaceColumnHeader,
            this.relationSpyColumnHeader});
            this.relationListView.GridLines = true;
            resources.ApplyResources(this.relationListView, "relationListView");
            this.relationListView.Name = "relationListView";
            this.relationListView.UseCompatibleStateImageBehavior = false;
            this.relationListView.View = System.Windows.Forms.View.Details;
            // 
            // relationCountryColumnHeader
            // 
            resources.ApplyResources(this.relationCountryColumnHeader, "relationCountryColumnHeader");
            // 
            // relationValueColumnHeader
            // 
            resources.ApplyResources(this.relationValueColumnHeader, "relationValueColumnHeader");
            // 
            // relationPuppetColumnHeader
            // 
            resources.ApplyResources(this.relationPuppetColumnHeader, "relationPuppetColumnHeader");
            // 
            // relationControlColumnHeader
            // 
            resources.ApplyResources(this.relationControlColumnHeader, "relationControlColumnHeader");
            // 
            // relationAccessColumnHeader
            // 
            resources.ApplyResources(this.relationAccessColumnHeader, "relationAccessColumnHeader");
            // 
            // relationGuaranteeColumnHeader
            // 
            resources.ApplyResources(this.relationGuaranteeColumnHeader, "relationGuaranteeColumnHeader");
            // 
            // relationNonAggressionColumnHeader
            // 
            resources.ApplyResources(this.relationNonAggressionColumnHeader, "relationNonAggressionColumnHeader");
            // 
            // relationPeaceColumnHeader
            // 
            resources.ApplyResources(this.relationPeaceColumnHeader, "relationPeaceColumnHeader");
            // 
            // relationSpyColumnHeader
            // 
            resources.ApplyResources(this.relationSpyColumnHeader, "relationSpyColumnHeader");
            // 
            // relationCountryListBox
            // 
            this.relationCountryListBox.FormattingEnabled = true;
            resources.ApplyResources(this.relationCountryListBox, "relationCountryListBox");
            this.relationCountryListBox.Name = "relationCountryListBox";
            // 
            // tradeEndDayTextBox
            // 
            resources.ApplyResources(this.tradeEndDayTextBox, "tradeEndDayTextBox");
            this.tradeEndDayTextBox.Name = "tradeEndDayTextBox";
            // 
            // tradeEndMonthTextBox
            // 
            resources.ApplyResources(this.tradeEndMonthTextBox, "tradeEndMonthTextBox");
            this.tradeEndMonthTextBox.Name = "tradeEndMonthTextBox";
            // 
            // tradeListView
            // 
            this.tradeListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.tradeStartDateColumnHeader,
            this.tradeEndDateColumnHeader,
            this.tradeCountryColumnHeader1,
            this.tradeCountryColumnHeader2,
            this.tradeDealsColumnHeader});
            this.tradeListView.FullRowSelect = true;
            this.tradeListView.GridLines = true;
            resources.ApplyResources(this.tradeListView, "tradeListView");
            this.tradeListView.Name = "tradeListView";
            this.tradeListView.UseCompatibleStateImageBehavior = false;
            this.tradeListView.View = System.Windows.Forms.View.Details;
            // 
            // tradeStartDateColumnHeader
            // 
            resources.ApplyResources(this.tradeStartDateColumnHeader, "tradeStartDateColumnHeader");
            // 
            // tradeEndDateColumnHeader
            // 
            resources.ApplyResources(this.tradeEndDateColumnHeader, "tradeEndDateColumnHeader");
            // 
            // tradeCountryColumnHeader1
            // 
            resources.ApplyResources(this.tradeCountryColumnHeader1, "tradeCountryColumnHeader1");
            // 
            // tradeCountryColumnHeader2
            // 
            resources.ApplyResources(this.tradeCountryColumnHeader2, "tradeCountryColumnHeader2");
            // 
            // tradeDealsColumnHeader
            // 
            resources.ApplyResources(this.tradeDealsColumnHeader, "tradeDealsColumnHeader");
            // 
            // tradeEndYearTextBox
            // 
            resources.ApplyResources(this.tradeEndYearTextBox, "tradeEndYearTextBox");
            this.tradeEndYearTextBox.Name = "tradeEndYearTextBox";
            // 
            // tradeDownButton
            // 
            resources.ApplyResources(this.tradeDownButton, "tradeDownButton");
            this.tradeDownButton.Name = "tradeDownButton";
            this.tradeDownButton.UseVisualStyleBackColor = true;
            // 
            // tradeStartDayTextBox
            // 
            resources.ApplyResources(this.tradeStartDayTextBox, "tradeStartDayTextBox");
            this.tradeStartDayTextBox.Name = "tradeStartDayTextBox";
            // 
            // tradeNewButton
            // 
            resources.ApplyResources(this.tradeNewButton, "tradeNewButton");
            this.tradeNewButton.Name = "tradeNewButton";
            this.tradeNewButton.UseVisualStyleBackColor = true;
            // 
            // tradeStartMonthTextBox
            // 
            resources.ApplyResources(this.tradeStartMonthTextBox, "tradeStartMonthTextBox");
            this.tradeStartMonthTextBox.Name = "tradeStartMonthTextBox";
            // 
            // tradeRemoveButton
            // 
            resources.ApplyResources(this.tradeRemoveButton, "tradeRemoveButton");
            this.tradeRemoveButton.Name = "tradeRemoveButton";
            this.tradeRemoveButton.UseVisualStyleBackColor = true;
            // 
            // tradeStartYearTextBox
            // 
            resources.ApplyResources(this.tradeStartYearTextBox, "tradeStartYearTextBox");
            this.tradeStartYearTextBox.Name = "tradeStartYearTextBox";
            // 
            // tradeUpButton
            // 
            resources.ApplyResources(this.tradeUpButton, "tradeUpButton");
            this.tradeUpButton.Name = "tradeUpButton";
            this.tradeUpButton.UseVisualStyleBackColor = true;
            // 
            // tradeCancelCheckBox
            // 
            resources.ApplyResources(this.tradeCancelCheckBox, "tradeCancelCheckBox");
            this.tradeCancelCheckBox.Name = "tradeCancelCheckBox";
            this.tradeCancelCheckBox.UseVisualStyleBackColor = true;
            // 
            // tradeStartDateLabel
            // 
            resources.ApplyResources(this.tradeStartDateLabel, "tradeStartDateLabel");
            this.tradeStartDateLabel.Name = "tradeStartDateLabel";
            // 
            // tradeSwapButton
            // 
            resources.ApplyResources(this.tradeSwapButton, "tradeSwapButton");
            this.tradeSwapButton.Name = "tradeSwapButton";
            this.tradeSwapButton.UseVisualStyleBackColor = true;
            // 
            // tradeEndDateLabel
            // 
            resources.ApplyResources(this.tradeEndDateLabel, "tradeEndDateLabel");
            this.tradeEndDateLabel.Name = "tradeEndDateLabel";
            // 
            // tradeMoneyTextBox2
            // 
            resources.ApplyResources(this.tradeMoneyTextBox2, "tradeMoneyTextBox2");
            this.tradeMoneyTextBox2.Name = "tradeMoneyTextBox2";
            // 
            // tradeCountryComboBox1
            // 
            this.tradeCountryComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tradeCountryComboBox1.FormattingEnabled = true;
            resources.ApplyResources(this.tradeCountryComboBox1, "tradeCountryComboBox1");
            this.tradeCountryComboBox1.Name = "tradeCountryComboBox1";
            // 
            // tradeMoneyTextBox1
            // 
            resources.ApplyResources(this.tradeMoneyTextBox1, "tradeMoneyTextBox1");
            this.tradeMoneyTextBox1.Name = "tradeMoneyTextBox1";
            // 
            // tradeCountryComboBox2
            // 
            this.tradeCountryComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tradeCountryComboBox2.FormattingEnabled = true;
            resources.ApplyResources(this.tradeCountryComboBox2, "tradeCountryComboBox2");
            this.tradeCountryComboBox2.Name = "tradeCountryComboBox2";
            // 
            // tradeMoneyLabel
            // 
            resources.ApplyResources(this.tradeMoneyLabel, "tradeMoneyLabel");
            this.tradeMoneyLabel.Name = "tradeMoneyLabel";
            // 
            // tradeEnergyLabel
            // 
            resources.ApplyResources(this.tradeEnergyLabel, "tradeEnergyLabel");
            this.tradeEnergyLabel.Name = "tradeEnergyLabel";
            // 
            // tradeSuppliesTextBox2
            // 
            resources.ApplyResources(this.tradeSuppliesTextBox2, "tradeSuppliesTextBox2");
            this.tradeSuppliesTextBox2.Name = "tradeSuppliesTextBox2";
            // 
            // tradeEnergyTextBox1
            // 
            resources.ApplyResources(this.tradeEnergyTextBox1, "tradeEnergyTextBox1");
            this.tradeEnergyTextBox1.Name = "tradeEnergyTextBox1";
            // 
            // tradeSuppliesTextBox1
            // 
            resources.ApplyResources(this.tradeSuppliesTextBox1, "tradeSuppliesTextBox1");
            this.tradeSuppliesTextBox1.Name = "tradeSuppliesTextBox1";
            // 
            // tradeEnergyTextBox2
            // 
            resources.ApplyResources(this.tradeEnergyTextBox2, "tradeEnergyTextBox2");
            this.tradeEnergyTextBox2.Name = "tradeEnergyTextBox2";
            // 
            // tradeSuppliesLabel
            // 
            resources.ApplyResources(this.tradeSuppliesLabel, "tradeSuppliesLabel");
            this.tradeSuppliesLabel.Name = "tradeSuppliesLabel";
            // 
            // tradeMetalLabel
            // 
            resources.ApplyResources(this.tradeMetalLabel, "tradeMetalLabel");
            this.tradeMetalLabel.Name = "tradeMetalLabel";
            // 
            // tradeOilTextBox2
            // 
            resources.ApplyResources(this.tradeOilTextBox2, "tradeOilTextBox2");
            this.tradeOilTextBox2.Name = "tradeOilTextBox2";
            // 
            // tradeMetalTextBox1
            // 
            resources.ApplyResources(this.tradeMetalTextBox1, "tradeMetalTextBox1");
            this.tradeMetalTextBox1.Name = "tradeMetalTextBox1";
            // 
            // tradeOilTextBox1
            // 
            resources.ApplyResources(this.tradeOilTextBox1, "tradeOilTextBox1");
            this.tradeOilTextBox1.Name = "tradeOilTextBox1";
            // 
            // tradeMetalTextBox2
            // 
            resources.ApplyResources(this.tradeMetalTextBox2, "tradeMetalTextBox2");
            this.tradeMetalTextBox2.Name = "tradeMetalTextBox2";
            // 
            // tradeOilLabel
            // 
            resources.ApplyResources(this.tradeOilLabel, "tradeOilLabel");
            this.tradeOilLabel.Name = "tradeOilLabel";
            // 
            // tradeRareMaterialsLabel
            // 
            resources.ApplyResources(this.tradeRareMaterialsLabel, "tradeRareMaterialsLabel");
            this.tradeRareMaterialsLabel.Name = "tradeRareMaterialsLabel";
            // 
            // tradeRareMaterialsTextBox2
            // 
            resources.ApplyResources(this.tradeRareMaterialsTextBox2, "tradeRareMaterialsTextBox2");
            this.tradeRareMaterialsTextBox2.Name = "tradeRareMaterialsTextBox2";
            // 
            // tradeRareMaterialsTextBox1
            // 
            resources.ApplyResources(this.tradeRareMaterialsTextBox1, "tradeRareMaterialsTextBox1");
            this.tradeRareMaterialsTextBox1.Name = "tradeRareMaterialsTextBox1";
            // 
            // nonAggressionGroupBox
            // 
            this.nonAggressionGroupBox.Controls.Add(this.nonAggressionIdTextBox);
            this.nonAggressionGroupBox.Controls.Add(this.nonAggressionTypeTextBox);
            this.nonAggressionGroupBox.Controls.Add(this.nonAggressionIdLabel);
            this.nonAggressionGroupBox.Controls.Add(this.nonAggressionCheckBox);
            this.nonAggressionGroupBox.Controls.Add(this.nonAggressionStartMonthTextBox);
            this.nonAggressionGroupBox.Controls.Add(this.nonAggressionStartYearTextBox);
            this.nonAggressionGroupBox.Controls.Add(this.nonAggressionStartDayTextBox);
            this.nonAggressionGroupBox.Controls.Add(this.nonAggressionStartLabel);
            this.nonAggressionGroupBox.Controls.Add(this.nonAggressionEndYearTextBox);
            this.nonAggressionGroupBox.Controls.Add(this.nonAggressionEndMonthTextBox);
            this.nonAggressionGroupBox.Controls.Add(this.nonAggressionEndDayTextBox);
            this.nonAggressionGroupBox.Controls.Add(this.nonAggressionEndLabel);
            resources.ApplyResources(this.nonAggressionGroupBox, "nonAggressionGroupBox");
            this.nonAggressionGroupBox.Name = "nonAggressionGroupBox";
            this.nonAggressionGroupBox.TabStop = false;
            // 
            // nonAggressionIdLabel
            // 
            resources.ApplyResources(this.nonAggressionIdLabel, "nonAggressionIdLabel");
            this.nonAggressionIdLabel.Name = "nonAggressionIdLabel";
            // 
            // nonAggressionTypeTextBox
            // 
            resources.ApplyResources(this.nonAggressionTypeTextBox, "nonAggressionTypeTextBox");
            this.nonAggressionTypeTextBox.Name = "nonAggressionTypeTextBox";
            // 
            // nonAggressionIdTextBox
            // 
            resources.ApplyResources(this.nonAggressionIdTextBox, "nonAggressionIdTextBox");
            this.nonAggressionIdTextBox.Name = "nonAggressionIdTextBox";
            // 
            // peaceGroupBox
            // 
            this.peaceGroupBox.Controls.Add(this.peaceIdTextBox);
            this.peaceGroupBox.Controls.Add(this.peaceTypeTextBox);
            this.peaceGroupBox.Controls.Add(this.peaceEndDayTextBox);
            this.peaceGroupBox.Controls.Add(this.peaceEndLabel);
            this.peaceGroupBox.Controls.Add(this.peaceEndMonthTextBox);
            this.peaceGroupBox.Controls.Add(this.peaceIdLabel);
            this.peaceGroupBox.Controls.Add(this.peaceEndYearTextBox);
            this.peaceGroupBox.Controls.Add(this.peaceStartLabel);
            this.peaceGroupBox.Controls.Add(this.peaceStartYearTextBox);
            this.peaceGroupBox.Controls.Add(this.peaceStartDayTextBox);
            this.peaceGroupBox.Controls.Add(this.peaceCheckBox);
            this.peaceGroupBox.Controls.Add(this.peaceStartMonthTextBox);
            resources.ApplyResources(this.peaceGroupBox, "peaceGroupBox");
            this.peaceGroupBox.Name = "peaceGroupBox";
            this.peaceGroupBox.TabStop = false;
            // 
            // peaceIdTextBox
            // 
            resources.ApplyResources(this.peaceIdTextBox, "peaceIdTextBox");
            this.peaceIdTextBox.Name = "peaceIdTextBox";
            // 
            // peaceTypeTextBox
            // 
            resources.ApplyResources(this.peaceTypeTextBox, "peaceTypeTextBox");
            this.peaceTypeTextBox.Name = "peaceTypeTextBox";
            // 
            // peaceIdLabel
            // 
            resources.ApplyResources(this.peaceIdLabel, "peaceIdLabel");
            this.peaceIdLabel.Name = "peaceIdLabel";
            // 
            // guaranteeGroupBox
            // 
            this.guaranteeGroupBox.Controls.Add(this.guaranteeCheckBox);
            this.guaranteeGroupBox.Controls.Add(this.guaranteeYearTextBox);
            this.guaranteeGroupBox.Controls.Add(this.guaranteeMonthTextBox);
            this.guaranteeGroupBox.Controls.Add(this.guaraneeEndLabel);
            this.guaranteeGroupBox.Controls.Add(this.guaranteeDayTextBox);
            resources.ApplyResources(this.guaranteeGroupBox, "guaranteeGroupBox");
            this.guaranteeGroupBox.Name = "guaranteeGroupBox";
            this.guaranteeGroupBox.TabStop = false;
            // 
            // regularTagLabel
            // 
            resources.ApplyResources(this.regularTagLabel, "regularTagLabel");
            this.regularTagLabel.Name = "regularTagLabel";
            // 
            // regularTagComboBox
            // 
            this.regularTagComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.regularTagComboBox, "regularTagComboBox");
            this.regularTagComboBox.Name = "regularTagComboBox";
            // 
            // tradeIdLabel
            // 
            resources.ApplyResources(this.tradeIdLabel, "tradeIdLabel");
            this.tradeIdLabel.Name = "tradeIdLabel";
            // 
            // tradeIdTextBox
            // 
            resources.ApplyResources(this.tradeIdTextBox, "tradeIdTextBox");
            this.tradeIdTextBox.Name = "tradeIdTextBox";
            // 
            // tradeTypeTextBox
            // 
            resources.ApplyResources(this.tradeTypeTextBox, "tradeTypeTextBox");
            this.tradeTypeTextBox.Name = "tradeTypeTextBox";
            // 
            // tradeInfoGroupBox
            // 
            this.tradeInfoGroupBox.Controls.Add(this.tradeStartDateLabel);
            this.tradeInfoGroupBox.Controls.Add(this.tradeIdTextBox);
            this.tradeInfoGroupBox.Controls.Add(this.tradeEndDateLabel);
            this.tradeInfoGroupBox.Controls.Add(this.tradeTypeTextBox);
            this.tradeInfoGroupBox.Controls.Add(this.tradeStartYearTextBox);
            this.tradeInfoGroupBox.Controls.Add(this.tradeIdLabel);
            this.tradeInfoGroupBox.Controls.Add(this.tradeStartMonthTextBox);
            this.tradeInfoGroupBox.Controls.Add(this.tradeCancelCheckBox);
            this.tradeInfoGroupBox.Controls.Add(this.tradeEndDayTextBox);
            this.tradeInfoGroupBox.Controls.Add(this.tradeStartDayTextBox);
            this.tradeInfoGroupBox.Controls.Add(this.tradeEndMonthTextBox);
            this.tradeInfoGroupBox.Controls.Add(this.tradeEndYearTextBox);
            resources.ApplyResources(this.tradeInfoGroupBox, "tradeInfoGroupBox");
            this.tradeInfoGroupBox.Name = "tradeInfoGroupBox";
            this.tradeInfoGroupBox.TabStop = false;
            // 
            // tradeDealGroupBox
            // 
            this.tradeDealGroupBox.Controls.Add(this.tradeCountryComboBox1);
            this.tradeDealGroupBox.Controls.Add(this.tradeRareMaterialsTextBox1);
            this.tradeDealGroupBox.Controls.Add(this.tradeRareMaterialsTextBox2);
            this.tradeDealGroupBox.Controls.Add(this.tradeRareMaterialsLabel);
            this.tradeDealGroupBox.Controls.Add(this.tradeOilLabel);
            this.tradeDealGroupBox.Controls.Add(this.tradeMetalTextBox2);
            this.tradeDealGroupBox.Controls.Add(this.tradeOilTextBox1);
            this.tradeDealGroupBox.Controls.Add(this.tradeMetalTextBox1);
            this.tradeDealGroupBox.Controls.Add(this.tradeSwapButton);
            this.tradeDealGroupBox.Controls.Add(this.tradeOilTextBox2);
            this.tradeDealGroupBox.Controls.Add(this.tradeMoneyTextBox2);
            this.tradeDealGroupBox.Controls.Add(this.tradeMetalLabel);
            this.tradeDealGroupBox.Controls.Add(this.tradeSuppliesLabel);
            this.tradeDealGroupBox.Controls.Add(this.tradeMoneyTextBox1);
            this.tradeDealGroupBox.Controls.Add(this.tradeEnergyTextBox2);
            this.tradeDealGroupBox.Controls.Add(this.tradeCountryComboBox2);
            this.tradeDealGroupBox.Controls.Add(this.tradeSuppliesTextBox1);
            this.tradeDealGroupBox.Controls.Add(this.tradeMoneyLabel);
            this.tradeDealGroupBox.Controls.Add(this.tradeEnergyTextBox1);
            this.tradeDealGroupBox.Controls.Add(this.tradeEnergyLabel);
            this.tradeDealGroupBox.Controls.Add(this.tradeSuppliesTextBox2);
            resources.ApplyResources(this.tradeDealGroupBox, "tradeDealGroupBox");
            this.tradeDealGroupBox.Name = "tradeDealGroupBox";
            this.tradeDealGroupBox.TabStop = false;
            // 
            // ScenarioEditorForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scenarioTabControl);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.reloadButton);
            this.Name = "ScenarioEditorForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.Move += new System.EventHandler(this.OnFormMove);
            this.Resize += new System.EventHandler(this.OnFormResize);
            this.scenarioTabControl.ResumeLayout(false);
            this.mainTabPage.ResumeLayout(false);
            this.optionGroupBox.ResumeLayout(false);
            this.optionGroupBox.PerformLayout();
            this.countryGroupBox.ResumeLayout(false);
            this.countryGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propagandaPictureBox)).EndInit();
            this.infoGroupBox.ResumeLayout(false);
            this.infoGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelPictureBox)).EndInit();
            this.folderGroupBox.ResumeLayout(false);
            this.folderGroupBox.PerformLayout();
            this.typeGroupBox.ResumeLayout(false);
            this.typeGroupBox.PerformLayout();
            this.allianceTabPage.ResumeLayout(false);
            this.relationTabPage.ResumeLayout(false);
            this.tradeTabPage.ResumeLayout(false);
            this.provinceTabPage.ResumeLayout(false);
            this.provinceTabPage.PerformLayout();
            this.provinceMapPanel.ResumeLayout(false);
            this.provinceMapPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.provinceMapPictureBox)).EndInit();
            this.allianceGroupBox.ResumeLayout(false);
            this.allianceGroupBox.PerformLayout();
            this.warGroupBox.ResumeLayout(false);
            this.warGroupBox.PerformLayout();
            this.intelligenceGroupBox.ResumeLayout(false);
            this.intelligenceGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spyNumNumericUpDown)).EndInit();
            this.diplomacyGroupBox.ResumeLayout(false);
            this.diplomacyGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.relationValueNumericUpDown)).EndInit();
            this.nonAggressionGroupBox.ResumeLayout(false);
            this.nonAggressionGroupBox.PerformLayout();
            this.peaceGroupBox.ResumeLayout(false);
            this.peaceGroupBox.PerformLayout();
            this.guaranteeGroupBox.ResumeLayout(false);
            this.guaranteeGroupBox.PerformLayout();
            this.tradeInfoGroupBox.ResumeLayout(false);
            this.tradeInfoGroupBox.PerformLayout();
            this.tradeDealGroupBox.ResumeLayout(false);
            this.tradeDealGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl scenarioTabControl;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button reloadButton;
        private System.Windows.Forms.TabPage mainTabPage;
        private System.Windows.Forms.TextBox scenarioNameTextBox;
        private System.Windows.Forms.Label scenarioNameLabel;
        private System.Windows.Forms.GroupBox typeGroupBox;
        private System.Windows.Forms.RadioButton saveGamesRadioButton;
        private System.Windows.Forms.RadioButton scenarioRadioButton;
        private System.Windows.Forms.ListBox scenarioListBox;
        private System.Windows.Forms.Label panelImageLabel;
        private System.Windows.Forms.Button panelImageBrowseButton;
        private System.Windows.Forms.TextBox panelImageTextBox;
        private System.Windows.Forms.TextBox endDayTextBox;
        private System.Windows.Forms.TextBox endMonthTextBox;
        private System.Windows.Forms.TextBox endYearTextBox;
        private System.Windows.Forms.Label endDateLabel;
        private System.Windows.Forms.TextBox startDayTextBox;
        private System.Windows.Forms.TextBox startMonthTextBox;
        private System.Windows.Forms.TextBox startYearTextBox;
        private System.Windows.Forms.Label startDateLabel;
        private System.Windows.Forms.Label selectableLabel;
        private System.Windows.Forms.Button majorRemoveButton;
        private System.Windows.Forms.Button majorAddButton;
        private System.Windows.Forms.Button majorDownButton;
        private System.Windows.Forms.Button majorUpButton;
        private System.Windows.Forms.ListBox majorListBox;
        private System.Windows.Forms.Label majorLabel;
        private System.Windows.Forms.GroupBox folderGroupBox;
        private System.Windows.Forms.RadioButton modRadioButton;
        private System.Windows.Forms.RadioButton vanillaRadioButton;
        private System.Windows.Forms.RadioButton exportRadioButton;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.PictureBox panelPictureBox;
        private System.Windows.Forms.CheckedListBox selectableCheckedListBox;
        private System.Windows.Forms.TabPage allianceTabPage;
        private System.Windows.Forms.ListView allianceListView;
        private System.Windows.Forms.ColumnHeader allianceTypeColumnHeader;
        private System.Windows.Forms.ColumnHeader allianceParticipantColumnHeader;
        private System.Windows.Forms.Button participantRemoveButton;
        private System.Windows.Forms.Button participantAddButton;
        private System.Windows.Forms.Button allianceLeaderButton;
        private System.Windows.Forms.Button allianceUpButton;
        private System.Windows.Forms.Button allianceRemoveButton;
        private System.Windows.Forms.Button allianceDownButton;
        private System.Windows.Forms.Button allianceNewButton;
        private System.Windows.Forms.TabPage relationTabPage;
        private System.Windows.Forms.TabPage tradeTabPage;
        private System.Windows.Forms.TabPage countryTabPage;
        private System.Windows.Forms.TabPage personTabPage;
        private System.Windows.Forms.TabPage techTabPage;
        private System.Windows.Forms.TabPage provinceTabPage;
        private System.Windows.Forms.GroupBox countryGroupBox;
        private System.Windows.Forms.GroupBox infoGroupBox;
        private System.Windows.Forms.GroupBox optionGroupBox;
        private System.Windows.Forms.Button includeFolderBrowseButton;
        private System.Windows.Forms.TextBox includeFolderTextBox;
        private System.Windows.Forms.Label includeFolderLabel;
        private System.Windows.Forms.CheckBox allowDiplomacyCheckBox;
        private System.Windows.Forms.CheckBox freeCountryCheckBox;
        private System.Windows.Forms.CheckBox battleScenarioCheckBox;
        private System.Windows.Forms.CheckBox allowProductionCheckBox;
        private System.Windows.Forms.CheckBox allowTechnologyCheckBox;
        private System.Windows.Forms.Label aiAggressiveLabel;
        private System.Windows.Forms.ComboBox gameSpeedComboBox;
        private System.Windows.Forms.ComboBox difficultyComboBox;
        private System.Windows.Forms.ComboBox aiAggressiveComboBox;
        private System.Windows.Forms.Label difficultyLabel;
        private System.Windows.Forms.Label gameSpeedLabel;
        private System.Windows.Forms.Button propagandaBrowseButton;
        private System.Windows.Forms.TextBox propagandaTextBox;
        private System.Windows.Forms.Label propagandaLabel;
        private System.Windows.Forms.TextBox countryDescTextBox;
        private System.Windows.Forms.Label countryDescLabel;
        private System.Windows.Forms.PictureBox propagandaPictureBox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PictureBox provinceMapPictureBox;
        private System.Windows.Forms.Panel provinceMapPanel;
        private System.Windows.Forms.ListBox allianceParticipantListBox;
        private System.Windows.Forms.ListBox allianceCountryListBox;
        private System.Windows.Forms.Label allianceParticipantLabel;
        private System.Windows.Forms.GroupBox warGroupBox;
        private System.Windows.Forms.Label warDefenderLabel;
        private System.Windows.Forms.ListView warListView;
        private System.Windows.Forms.ColumnHeader warStartDateColumnHeader;
        private System.Windows.Forms.ColumnHeader warEndDateColumnHeader;
        private System.Windows.Forms.ColumnHeader warAttackerColumnHeader;
        private System.Windows.Forms.ColumnHeader warDefenderColumnHeader;
        private System.Windows.Forms.Button warNewButton;
        private System.Windows.Forms.Button warAttackerRemoveButton;
        private System.Windows.Forms.Button warUpButton;
        private System.Windows.Forms.ListBox warAttackerListBox;
        private System.Windows.Forms.Button warDefenderRemoveButton;
        private System.Windows.Forms.Label warAttackerLabel;
        private System.Windows.Forms.ListBox warDefenderListBox;
        private System.Windows.Forms.Button warDownButton;
        private System.Windows.Forms.ListBox warCountryListBox;
        private System.Windows.Forms.Button warAttackerAddButton;
        private System.Windows.Forms.Button warDefenderAddButton;
        private System.Windows.Forms.Button warRemoveButton;
        private System.Windows.Forms.GroupBox allianceGroupBox;
        private System.Windows.Forms.GroupBox intelligenceGroupBox;
        private System.Windows.Forms.NumericUpDown spyNumNumericUpDown;
        private System.Windows.Forms.Label spyNumLabel;
        private System.Windows.Forms.GroupBox diplomacyGroupBox;
        private System.Windows.Forms.Label peaceEndLabel;
        private System.Windows.Forms.TextBox peaceEndDayTextBox;
        private System.Windows.Forms.TextBox peaceEndMonthTextBox;
        private System.Windows.Forms.TextBox peaceEndYearTextBox;
        private System.Windows.Forms.Label peaceStartLabel;
        private System.Windows.Forms.TextBox peaceStartDayTextBox;
        private System.Windows.Forms.CheckBox peaceCheckBox;
        private System.Windows.Forms.TextBox peaceStartYearTextBox;
        private System.Windows.Forms.TextBox peaceStartMonthTextBox;
        private System.Windows.Forms.CheckBox accessCheckBox;
        private System.Windows.Forms.Label nonAggressionEndLabel;
        private System.Windows.Forms.CheckBox guaranteeCheckBox;
        private System.Windows.Forms.TextBox nonAggressionEndDayTextBox;
        private System.Windows.Forms.TextBox guaranteeYearTextBox;
        private System.Windows.Forms.TextBox nonAggressionEndMonthTextBox;
        private System.Windows.Forms.TextBox guaranteeMonthTextBox;
        private System.Windows.Forms.TextBox nonAggressionEndYearTextBox;
        private System.Windows.Forms.TextBox guaranteeDayTextBox;
        private System.Windows.Forms.Label nonAggressionStartLabel;
        private System.Windows.Forms.CheckBox puppetCheckBox;
        private System.Windows.Forms.Label guaraneeEndLabel;
        private System.Windows.Forms.CheckBox controlCheckBox;
        private System.Windows.Forms.TextBox nonAggressionStartDayTextBox;
        private System.Windows.Forms.CheckBox nonAggressionCheckBox;
        private System.Windows.Forms.TextBox nonAggressionStartYearTextBox;
        private System.Windows.Forms.TextBox nonAggressionStartMonthTextBox;
        private System.Windows.Forms.NumericUpDown relationValueNumericUpDown;
        private System.Windows.Forms.Label relationValueLabel;
        private System.Windows.Forms.ListView relationListView;
        private System.Windows.Forms.ColumnHeader relationCountryColumnHeader;
        private System.Windows.Forms.ColumnHeader relationValueColumnHeader;
        private System.Windows.Forms.ColumnHeader relationPuppetColumnHeader;
        private System.Windows.Forms.ColumnHeader relationControlColumnHeader;
        private System.Windows.Forms.ColumnHeader relationAccessColumnHeader;
        private System.Windows.Forms.ColumnHeader relationGuaranteeColumnHeader;
        private System.Windows.Forms.ColumnHeader relationNonAggressionColumnHeader;
        private System.Windows.Forms.ColumnHeader relationPeaceColumnHeader;
        private System.Windows.Forms.ColumnHeader relationSpyColumnHeader;
        private System.Windows.Forms.ListBox relationCountryListBox;
        private System.Windows.Forms.TextBox tradeEndDayTextBox;
        private System.Windows.Forms.TextBox tradeEndMonthTextBox;
        private System.Windows.Forms.ListView tradeListView;
        private System.Windows.Forms.ColumnHeader tradeStartDateColumnHeader;
        private System.Windows.Forms.ColumnHeader tradeEndDateColumnHeader;
        private System.Windows.Forms.ColumnHeader tradeCountryColumnHeader1;
        private System.Windows.Forms.ColumnHeader tradeCountryColumnHeader2;
        private System.Windows.Forms.ColumnHeader tradeDealsColumnHeader;
        private System.Windows.Forms.TextBox tradeEndYearTextBox;
        private System.Windows.Forms.Button tradeDownButton;
        private System.Windows.Forms.TextBox tradeStartDayTextBox;
        private System.Windows.Forms.Button tradeNewButton;
        private System.Windows.Forms.TextBox tradeStartMonthTextBox;
        private System.Windows.Forms.Button tradeRemoveButton;
        private System.Windows.Forms.TextBox tradeStartYearTextBox;
        private System.Windows.Forms.Button tradeUpButton;
        private System.Windows.Forms.CheckBox tradeCancelCheckBox;
        private System.Windows.Forms.Label tradeStartDateLabel;
        private System.Windows.Forms.Button tradeSwapButton;
        private System.Windows.Forms.Label tradeEndDateLabel;
        private System.Windows.Forms.TextBox tradeMoneyTextBox2;
        private System.Windows.Forms.ComboBox tradeCountryComboBox1;
        private System.Windows.Forms.TextBox tradeMoneyTextBox1;
        private System.Windows.Forms.ComboBox tradeCountryComboBox2;
        private System.Windows.Forms.Label tradeMoneyLabel;
        private System.Windows.Forms.Label tradeEnergyLabel;
        private System.Windows.Forms.TextBox tradeSuppliesTextBox2;
        private System.Windows.Forms.TextBox tradeEnergyTextBox1;
        private System.Windows.Forms.TextBox tradeSuppliesTextBox1;
        private System.Windows.Forms.TextBox tradeEnergyTextBox2;
        private System.Windows.Forms.Label tradeSuppliesLabel;
        private System.Windows.Forms.Label tradeMetalLabel;
        private System.Windows.Forms.TextBox tradeOilTextBox2;
        private System.Windows.Forms.TextBox tradeMetalTextBox1;
        private System.Windows.Forms.TextBox tradeOilTextBox1;
        private System.Windows.Forms.TextBox tradeMetalTextBox2;
        private System.Windows.Forms.Label tradeOilLabel;
        private System.Windows.Forms.Label tradeRareMaterialsLabel;
        private System.Windows.Forms.TextBox tradeRareMaterialsTextBox2;
        private System.Windows.Forms.TextBox tradeRareMaterialsTextBox1;
        private System.Windows.Forms.GroupBox guaranteeGroupBox;
        private System.Windows.Forms.GroupBox peaceGroupBox;
        private System.Windows.Forms.TextBox peaceIdTextBox;
        private System.Windows.Forms.TextBox peaceTypeTextBox;
        private System.Windows.Forms.Label peaceIdLabel;
        private System.Windows.Forms.GroupBox nonAggressionGroupBox;
        private System.Windows.Forms.TextBox nonAggressionIdTextBox;
        private System.Windows.Forms.TextBox nonAggressionTypeTextBox;
        private System.Windows.Forms.Label nonAggressionIdLabel;
        private System.Windows.Forms.ComboBox regularTagComboBox;
        private System.Windows.Forms.Label regularTagLabel;
        private System.Windows.Forms.GroupBox tradeDealGroupBox;
        private System.Windows.Forms.GroupBox tradeInfoGroupBox;
        private System.Windows.Forms.TextBox tradeIdTextBox;
        private System.Windows.Forms.TextBox tradeTypeTextBox;
        private System.Windows.Forms.Label tradeIdLabel;

    }
}