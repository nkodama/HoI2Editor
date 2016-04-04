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
            System.Windows.Forms.Label provinceIcLabel;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScenarioEditorForm));
            System.Windows.Forms.Label provinceInfrastructureLabel;
            System.Windows.Forms.Label provinceLandFortLabel;
            System.Windows.Forms.Label provinceCoastalFortLabel;
            System.Windows.Forms.Label provinceNavalBaseLabel;
            System.Windows.Forms.Label provinceAirBaseLabel;
            System.Windows.Forms.Label provinceAntiAirLabel;
            System.Windows.Forms.Label provinceNuclearReactorLabel;
            System.Windows.Forms.Label provinceRocketTestLabel;
            System.Windows.Forms.Label provinceRadarStationLbel;
            System.Windows.Forms.Label buildingCurrentLabel1;
            System.Windows.Forms.Label buildingMaxLabel1;
            System.Windows.Forms.Label buildingRelativeLabel1;
            System.Windows.Forms.Label provinceNameLabel;
            System.Windows.Forms.Label provinceVpLabel;
            System.Windows.Forms.Label provinceRevoltRiskLabel;
            System.Windows.Forms.Label resourceMaxLabel;
            System.Windows.Forms.Label resourceCurrentLabel;
            System.Windows.Forms.Label resourcePoolLabel;
            System.Windows.Forms.TabPage technologyTabPage;
            System.Windows.Forms.Label tradeStartDateLabel;
            System.Windows.Forms.Label tradeEndDateLabel;
            System.Windows.Forms.Label tradeIdLabel;
            System.Windows.Forms.Label relationValueLabel;
            System.Windows.Forms.Label spyNumLabel;
            this.techTreePanel = new System.Windows.Forms.Panel();
            this.techTreePictureBox = new System.Windows.Forms.PictureBox();
            this.inventionsListView = new System.Windows.Forms.ListView();
            this.blueprintsListView = new System.Windows.Forms.ListView();
            this.ownedTechsListView = new System.Windows.Forms.ListView();
            this.ownedTechsLabel = new System.Windows.Forms.Label();
            this.techCategoryListBox = new System.Windows.Forms.ListBox();
            this.blueprintsLabel = new System.Windows.Forms.Label();
            this.inventionsLabel = new System.Windows.Forms.Label();
            this.techCountryListBox = new System.Windows.Forms.ListBox();
            this.provinceSyntheticOilLabel = new System.Windows.Forms.Label();
            this.provinceSyntheticRaresLabel = new System.Windows.Forms.Label();
            this.provinceNuclearPowerLabel = new System.Windows.Forms.Label();
            this.provinceIdLabel = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.reloadButton = new System.Windows.Forms.Button();
            this.provinceTabPage = new System.Windows.Forms.TabPage();
            this.provinceIdTextBox = new System.Windows.Forms.TextBox();
            this.provinceCountryFilterLabel = new System.Windows.Forms.Label();
            this.provinceCountryFilterComboBox = new System.Windows.Forms.ComboBox();
            this.provinceCountryGroupBox = new System.Windows.Forms.GroupBox();
            this.claimedProvinceCheckBox = new System.Windows.Forms.CheckBox();
            this.capitalCheckBox = new System.Windows.Forms.CheckBox();
            this.controlledProvinceCheckBox = new System.Windows.Forms.CheckBox();
            this.coreProvinceCheckBox = new System.Windows.Forms.CheckBox();
            this.ownedProvinceCheckBox = new System.Windows.Forms.CheckBox();
            this.mapFilterGroupBox = new System.Windows.Forms.GroupBox();
            this.mapFilterClaimedRadioButton = new System.Windows.Forms.RadioButton();
            this.mapFilterControlledRadioButton = new System.Windows.Forms.RadioButton();
            this.mapFilterOwnedRadioButton = new System.Windows.Forms.RadioButton();
            this.mapFilterCoreRadioButton = new System.Windows.Forms.RadioButton();
            this.mapFilterNoneRadioButton = new System.Windows.Forms.RadioButton();
            this.provinceInfoGroupBox = new System.Windows.Forms.GroupBox();
            this.provinceNameStringTextBox = new System.Windows.Forms.TextBox();
            this.revoltRiskTextBox = new System.Windows.Forms.TextBox();
            this.vpTextBox = new System.Windows.Forms.TextBox();
            this.provinceNameKeyTextBox = new System.Windows.Forms.TextBox();
            this.provinceResourceGroupBox = new System.Windows.Forms.GroupBox();
            this.manpowerCurrentTextBox = new System.Windows.Forms.TextBox();
            this.suppliesPoolTextBox = new System.Windows.Forms.TextBox();
            this.oilMaxTextBox = new System.Windows.Forms.TextBox();
            this.manpowerMaxTextBox = new System.Windows.Forms.TextBox();
            this.oilCurrentTextBox = new System.Windows.Forms.TextBox();
            this.oilPoolTextBox = new System.Windows.Forms.TextBox();
            this.rareMaterialsMaxTextBox = new System.Windows.Forms.TextBox();
            this.rareMaterialsCurrentTextBox = new System.Windows.Forms.TextBox();
            this.rareMaterialsPoolTextBox = new System.Windows.Forms.TextBox();
            this.metalMaxTextBox = new System.Windows.Forms.TextBox();
            this.metalCurrentTextBox = new System.Windows.Forms.TextBox();
            this.metalPoolTextBox = new System.Windows.Forms.TextBox();
            this.energyMaxTextBox = new System.Windows.Forms.TextBox();
            this.energyCurrentTextBox = new System.Windows.Forms.TextBox();
            this.provinceSuppliesLabel = new System.Windows.Forms.Label();
            this.energyPoolTextBox = new System.Windows.Forms.TextBox();
            this.provinceEnergyLabel = new System.Windows.Forms.Label();
            this.provinceOilLabel = new System.Windows.Forms.Label();
            this.provinceMetalLabel = new System.Windows.Forms.Label();
            this.provinceRareMaterialsLabel = new System.Windows.Forms.Label();
            this.provinceManpowerLabel = new System.Windows.Forms.Label();
            this.provinceBuildingGroupBox = new System.Windows.Forms.GroupBox();
            this.nuclearPowerRelativeTextBox = new System.Windows.Forms.TextBox();
            this.nuclearPowerMaxTextBox = new System.Windows.Forms.TextBox();
            this.nuclearPowerCurrentTextBox = new System.Windows.Forms.TextBox();
            this.syntheticRaresRelativeTextBox = new System.Windows.Forms.TextBox();
            this.syntheticRaresMaxTextBox = new System.Windows.Forms.TextBox();
            this.syntheticRaresCurrentTextBox = new System.Windows.Forms.TextBox();
            this.syntheticOilRelativeTextBox = new System.Windows.Forms.TextBox();
            this.syntheticOilMaxTextBox = new System.Windows.Forms.TextBox();
            this.syntheticOilCurrentTextBox = new System.Windows.Forms.TextBox();
            this.rocketTestRelativeTextBox = new System.Windows.Forms.TextBox();
            this.rocketTestMaxTextBox = new System.Windows.Forms.TextBox();
            this.rocketTestCurrentTextBox = new System.Windows.Forms.TextBox();
            this.infrastructureRelativeTextBox = new System.Windows.Forms.TextBox();
            this.infrastructureMaxTextBox = new System.Windows.Forms.TextBox();
            this.infrastructureCurrentTextBox = new System.Windows.Forms.TextBox();
            this.icRelativeTextBox = new System.Windows.Forms.TextBox();
            this.icMaxTextBox = new System.Windows.Forms.TextBox();
            this.icCurrentTextBox = new System.Windows.Forms.TextBox();
            this.nuclearReactorRelativeTextBox = new System.Windows.Forms.TextBox();
            this.nuclearReactorMaxTextBox = new System.Windows.Forms.TextBox();
            this.nuclearReactorCurrentTextBox = new System.Windows.Forms.TextBox();
            this.radarStationRelativeTextBox = new System.Windows.Forms.TextBox();
            this.radarStationMaxTextBox = new System.Windows.Forms.TextBox();
            this.radarStationCurrentTextBox = new System.Windows.Forms.TextBox();
            this.navalBaseRelativeTextBox = new System.Windows.Forms.TextBox();
            this.navalBaseMaxTextBox = new System.Windows.Forms.TextBox();
            this.navalBaseCurrentTextBox = new System.Windows.Forms.TextBox();
            this.airBaseRelativeTextBox = new System.Windows.Forms.TextBox();
            this.airBaseMaxTextBox = new System.Windows.Forms.TextBox();
            this.airBaseCurrentTextBox = new System.Windows.Forms.TextBox();
            this.antiAirRelativeTextBox = new System.Windows.Forms.TextBox();
            this.antiAirMaxTextBox = new System.Windows.Forms.TextBox();
            this.antiAirCurrentTextBox = new System.Windows.Forms.TextBox();
            this.coastalFortRelativeTextBox = new System.Windows.Forms.TextBox();
            this.coastalFortMaxTextBox = new System.Windows.Forms.TextBox();
            this.coastalFortCurrentTextBox = new System.Windows.Forms.TextBox();
            this.landFortRelativeTextBox = new System.Windows.Forms.TextBox();
            this.landFortMaxTextBox = new System.Windows.Forms.TextBox();
            this.landFortCurrentTextBox = new System.Windows.Forms.TextBox();
            this.provinceMapPanel = new System.Windows.Forms.Panel();
            this.provinceMapPictureBox = new System.Windows.Forms.PictureBox();
            this.provinceListView = new System.Windows.Forms.ListView();
            this.provinceIdColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.provinceNameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.capitalProvinceColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.coreProvinceColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ownedColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.controlledProvinceColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.claimedProvinceColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.governmentTabPage = new System.Windows.Forms.TabPage();
            this.countryTabPage = new System.Windows.Forms.TabPage();
            this.tradeTabPage = new System.Windows.Forms.TabPage();
            this.tradeDealsGroupBox = new System.Windows.Forms.GroupBox();
            this.tradeCountryComboBox1 = new System.Windows.Forms.ComboBox();
            this.tradeRareMaterialsTextBox1 = new System.Windows.Forms.TextBox();
            this.tradeRareMaterialsTextBox2 = new System.Windows.Forms.TextBox();
            this.tradeRareMaterialsLabel = new System.Windows.Forms.Label();
            this.tradeOilLabel = new System.Windows.Forms.Label();
            this.tradeMetalTextBox2 = new System.Windows.Forms.TextBox();
            this.tradeOilTextBox1 = new System.Windows.Forms.TextBox();
            this.tradeMetalTextBox1 = new System.Windows.Forms.TextBox();
            this.tradeSwapButton = new System.Windows.Forms.Button();
            this.tradeOilTextBox2 = new System.Windows.Forms.TextBox();
            this.tradeMoneyTextBox2 = new System.Windows.Forms.TextBox();
            this.tradeMetalLabel = new System.Windows.Forms.Label();
            this.tradeSuppliesLabel = new System.Windows.Forms.Label();
            this.tradeMoneyTextBox1 = new System.Windows.Forms.TextBox();
            this.tradeEnergyTextBox2 = new System.Windows.Forms.TextBox();
            this.tradeCountryComboBox2 = new System.Windows.Forms.ComboBox();
            this.tradeSuppliesTextBox1 = new System.Windows.Forms.TextBox();
            this.tradeMoneyLabel = new System.Windows.Forms.Label();
            this.tradeEnergyTextBox1 = new System.Windows.Forms.TextBox();
            this.tradeEnergyLabel = new System.Windows.Forms.Label();
            this.tradeSuppliesTextBox2 = new System.Windows.Forms.TextBox();
            this.tradeInfoGroupBox = new System.Windows.Forms.GroupBox();
            this.tradeIdTextBox = new System.Windows.Forms.TextBox();
            this.tradeTypeTextBox = new System.Windows.Forms.TextBox();
            this.tradeStartYearTextBox = new System.Windows.Forms.TextBox();
            this.tradeStartMonthTextBox = new System.Windows.Forms.TextBox();
            this.tradeCancelCheckBox = new System.Windows.Forms.CheckBox();
            this.tradeEndDayTextBox = new System.Windows.Forms.TextBox();
            this.tradeStartDayTextBox = new System.Windows.Forms.TextBox();
            this.tradeEndMonthTextBox = new System.Windows.Forms.TextBox();
            this.tradeEndYearTextBox = new System.Windows.Forms.TextBox();
            this.tradeListView = new System.Windows.Forms.ListView();
            this.tradeCountryColumnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tradeCountryColumnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tradeDealsColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tradeDownButton = new System.Windows.Forms.Button();
            this.tradeNewButton = new System.Windows.Forms.Button();
            this.tradeRemoveButton = new System.Windows.Forms.Button();
            this.tradeUpButton = new System.Windows.Forms.Button();
            this.relationTabPage = new System.Windows.Forms.TabPage();
            this.peaceGroupBox = new System.Windows.Forms.GroupBox();
            this.peaceIdTextBox = new System.Windows.Forms.TextBox();
            this.peaceTypeTextBox = new System.Windows.Forms.TextBox();
            this.peaceEndDayTextBox = new System.Windows.Forms.TextBox();
            this.peaceEndLabel = new System.Windows.Forms.Label();
            this.peaceEndMonthTextBox = new System.Windows.Forms.TextBox();
            this.peaceIdLabel = new System.Windows.Forms.Label();
            this.peaceEndYearTextBox = new System.Windows.Forms.TextBox();
            this.peaceStartLabel = new System.Windows.Forms.Label();
            this.peaceStartYearTextBox = new System.Windows.Forms.TextBox();
            this.peaceStartDayTextBox = new System.Windows.Forms.TextBox();
            this.peaceCheckBox = new System.Windows.Forms.CheckBox();
            this.peaceStartMonthTextBox = new System.Windows.Forms.TextBox();
            this.guaranteedGroupBox = new System.Windows.Forms.GroupBox();
            this.guaranteedCheckBox = new System.Windows.Forms.CheckBox();
            this.guaranteedYearTextBox = new System.Windows.Forms.TextBox();
            this.guaranteedMonthTextBox = new System.Windows.Forms.TextBox();
            this.guaranteedEndLabel = new System.Windows.Forms.Label();
            this.guaranteedDayTextBox = new System.Windows.Forms.TextBox();
            this.nonAggressionGroupBox = new System.Windows.Forms.GroupBox();
            this.nonAggressionIdTextBox = new System.Windows.Forms.TextBox();
            this.nonAggressionTypeTextBox = new System.Windows.Forms.TextBox();
            this.nonAggressionIdLabel = new System.Windows.Forms.Label();
            this.nonAggressionCheckBox = new System.Windows.Forms.CheckBox();
            this.nonAggressionStartMonthTextBox = new System.Windows.Forms.TextBox();
            this.nonAggressionStartYearTextBox = new System.Windows.Forms.TextBox();
            this.nonAggressionStartDayTextBox = new System.Windows.Forms.TextBox();
            this.nonAggressionStartLabel = new System.Windows.Forms.Label();
            this.nonAggressionEndYearTextBox = new System.Windows.Forms.TextBox();
            this.nonAggressionEndMonthTextBox = new System.Windows.Forms.TextBox();
            this.nonAggressionEndDayTextBox = new System.Windows.Forms.TextBox();
            this.nonAggressionEndLabel = new System.Windows.Forms.Label();
            this.relationGroupBox = new System.Windows.Forms.GroupBox();
            this.relationValueTextBox = new System.Windows.Forms.TextBox();
            this.controlCheckBox = new System.Windows.Forms.CheckBox();
            this.masterCheckBox = new System.Windows.Forms.CheckBox();
            this.accessCheckBox = new System.Windows.Forms.CheckBox();
            this.intelligenceGroupBox = new System.Windows.Forms.GroupBox();
            this.spyNumNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.relationListView = new System.Windows.Forms.ListView();
            this.relationCountryColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.relationValueColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.relationMasterColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.relationControlColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.relationAccessColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.relationGuaranteeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.relationNonAggressionColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.relationPeaceColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.relationSpyColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.relationCountryListBox = new System.Windows.Forms.ListBox();
            this.allianceTabPage = new System.Windows.Forms.TabPage();
            this.mainTabPage = new System.Windows.Forms.TabPage();
            this.scenarioOptionGroupBox = new System.Windows.Forms.GroupBox();
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
            this.countrySelectionGroupBox = new System.Windows.Forms.GroupBox();
            this.countryDescKeyTextBox = new System.Windows.Forms.TextBox();
            this.majorCountryNameStringTextBox = new System.Windows.Forms.TextBox();
            this.majorFlagExtTextBox = new System.Windows.Forms.TextBox();
            this.majorFlagExtLabel = new System.Windows.Forms.Label();
            this.majorCountryNameKeyTextBox = new System.Windows.Forms.TextBox();
            this.majorCountryNameLabel = new System.Windows.Forms.Label();
            this.selectableRemoveButton = new System.Windows.Forms.Button();
            this.selectableAddButton = new System.Windows.Forms.Button();
            this.unselectableListBox = new System.Windows.Forms.ListBox();
            this.selectableListBox = new System.Windows.Forms.ListBox();
            this.propagandaPictureBox = new System.Windows.Forms.PictureBox();
            this.propagandaBrowseButton = new System.Windows.Forms.Button();
            this.propagandaTextBox = new System.Windows.Forms.TextBox();
            this.propagandaLabel = new System.Windows.Forms.Label();
            this.countryDescStringTextBox = new System.Windows.Forms.TextBox();
            this.majorAddButton = new System.Windows.Forms.Button();
            this.majorRemoveButton = new System.Windows.Forms.Button();
            this.countryDescLabel = new System.Windows.Forms.Label();
            this.majorListBox = new System.Windows.Forms.ListBox();
            this.majorLabel = new System.Windows.Forms.Label();
            this.selectableLabel = new System.Windows.Forms.Label();
            this.majorUpButton = new System.Windows.Forms.Button();
            this.majorDownButton = new System.Windows.Forms.Button();
            this.scenarioInfoGroupBox = new System.Windows.Forms.GroupBox();
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
            this.scenarioTabControl = new System.Windows.Forms.TabControl();
            this.oobTabPage = new System.Windows.Forms.TabPage();
            this.oobBottomButton = new System.Windows.Forms.Button();
            this.oobDownButton = new System.Windows.Forms.Button();
            this.oobUpButton = new System.Windows.Forms.Button();
            this.oobTopButton = new System.Windows.Forms.Button();
            this.oobRemoveButton = new System.Windows.Forms.Button();
            this.oobCloneButton = new System.Windows.Forms.Button();
            this.oobAddDivisionButton = new System.Windows.Forms.Button();
            this.oobAddUnitButton = new System.Windows.Forms.Button();
            this.divisionGroupBox = new System.Windows.Forms.GroupBox();
            this.dormantCheckBox = new System.Windows.Forms.CheckBox();
            this.lockedCheckBox = new System.Windows.Forms.CheckBox();
            this.experienceTextBox = new System.Windows.Forms.TextBox();
            this.experienceLabel = new System.Windows.Forms.Label();
            this.divisionMoraleTextBox = new System.Windows.Forms.TextBox();
            this.divisionMoraleLabel = new System.Windows.Forms.Label();
            this.maxOrganisationTextBox = new System.Windows.Forms.TextBox();
            this.organisationTextBox = new System.Windows.Forms.TextBox();
            this.organisationLabel = new System.Windows.Forms.Label();
            this.maxStrengthTextBox = new System.Windows.Forms.TextBox();
            this.strengthTextBox = new System.Windows.Forms.TextBox();
            this.strengthLabel = new System.Windows.Forms.Label();
            this.brigadeModelComboBox5 = new System.Windows.Forms.ComboBox();
            this.brigadeTypeComboBox5 = new System.Windows.Forms.ComboBox();
            this.brigadeModelComboBox4 = new System.Windows.Forms.ComboBox();
            this.brigadeTypeComboBox4 = new System.Windows.Forms.ComboBox();
            this.brigadeModelComboBox3 = new System.Windows.Forms.ComboBox();
            this.brigadeTypeComboBox3 = new System.Windows.Forms.ComboBox();
            this.brigadeModelComboBox2 = new System.Windows.Forms.ComboBox();
            this.brigadeTypeComboBox2 = new System.Windows.Forms.ComboBox();
            this.brigadeModelComboBox1 = new System.Windows.Forms.ComboBox();
            this.brigadesLabel = new System.Windows.Forms.Label();
            this.brigadeTypeComboBox1 = new System.Windows.Forms.ComboBox();
            this.unitModelComboBox = new System.Windows.Forms.ComboBox();
            this.unitTypeLabel = new System.Windows.Forms.Label();
            this.unitTypeComboBox = new System.Windows.Forms.ComboBox();
            this.divisionNameTextBox = new System.Windows.Forms.TextBox();
            this.divisionNameLabel = new System.Windows.Forms.Label();
            this.divisionIdTextBox = new System.Windows.Forms.TextBox();
            this.divisionTypeTextBox = new System.Windows.Forms.TextBox();
            this.divisionIdLabel = new System.Windows.Forms.Label();
            this.unitGroupBox = new System.Windows.Forms.GroupBox();
            this.leaderComboBox = new System.Windows.Forms.ComboBox();
            this.baseComboBox = new System.Windows.Forms.ComboBox();
            this.locationComboBox = new System.Windows.Forms.ComboBox();
            this.digInTextBox = new System.Windows.Forms.TextBox();
            this.digInLabel = new System.Windows.Forms.Label();
            this.leaderTextBox = new System.Windows.Forms.TextBox();
            this.leaderLabel = new System.Windows.Forms.Label();
            this.unitMoraleTextBox = new System.Windows.Forms.TextBox();
            this.unitMoraleLabel = new System.Windows.Forms.Label();
            this.baseTextBox = new System.Windows.Forms.TextBox();
            this.baseLabel = new System.Windows.Forms.Label();
            this.locationTextBox = new System.Windows.Forms.TextBox();
            this.locationLabel = new System.Windows.Forms.Label();
            this.unitNameTextBox = new System.Windows.Forms.TextBox();
            this.unitNameLabel = new System.Windows.Forms.Label();
            this.unitIdTextBox = new System.Windows.Forms.TextBox();
            this.unitTypeTextBox = new System.Windows.Forms.TextBox();
            this.unitIdLabel = new System.Windows.Forms.Label();
            this.unitTreeView = new System.Windows.Forms.TreeView();
            this.oobCountryListBox = new System.Windows.Forms.ListBox();
            this.checkButton = new System.Windows.Forms.Button();
            provinceIcLabel = new System.Windows.Forms.Label();
            provinceInfrastructureLabel = new System.Windows.Forms.Label();
            provinceLandFortLabel = new System.Windows.Forms.Label();
            provinceCoastalFortLabel = new System.Windows.Forms.Label();
            provinceNavalBaseLabel = new System.Windows.Forms.Label();
            provinceAirBaseLabel = new System.Windows.Forms.Label();
            provinceAntiAirLabel = new System.Windows.Forms.Label();
            provinceNuclearReactorLabel = new System.Windows.Forms.Label();
            provinceRocketTestLabel = new System.Windows.Forms.Label();
            provinceRadarStationLbel = new System.Windows.Forms.Label();
            buildingCurrentLabel1 = new System.Windows.Forms.Label();
            buildingMaxLabel1 = new System.Windows.Forms.Label();
            buildingRelativeLabel1 = new System.Windows.Forms.Label();
            provinceNameLabel = new System.Windows.Forms.Label();
            provinceVpLabel = new System.Windows.Forms.Label();
            provinceRevoltRiskLabel = new System.Windows.Forms.Label();
            resourceMaxLabel = new System.Windows.Forms.Label();
            resourceCurrentLabel = new System.Windows.Forms.Label();
            resourcePoolLabel = new System.Windows.Forms.Label();
            technologyTabPage = new System.Windows.Forms.TabPage();
            tradeStartDateLabel = new System.Windows.Forms.Label();
            tradeEndDateLabel = new System.Windows.Forms.Label();
            tradeIdLabel = new System.Windows.Forms.Label();
            relationValueLabel = new System.Windows.Forms.Label();
            spyNumLabel = new System.Windows.Forms.Label();
            technologyTabPage.SuspendLayout();
            this.techTreePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.techTreePictureBox)).BeginInit();
            this.provinceTabPage.SuspendLayout();
            this.provinceCountryGroupBox.SuspendLayout();
            this.mapFilterGroupBox.SuspendLayout();
            this.provinceInfoGroupBox.SuspendLayout();
            this.provinceResourceGroupBox.SuspendLayout();
            this.provinceBuildingGroupBox.SuspendLayout();
            this.provinceMapPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.provinceMapPictureBox)).BeginInit();
            this.tradeTabPage.SuspendLayout();
            this.tradeDealsGroupBox.SuspendLayout();
            this.tradeInfoGroupBox.SuspendLayout();
            this.relationTabPage.SuspendLayout();
            this.peaceGroupBox.SuspendLayout();
            this.guaranteedGroupBox.SuspendLayout();
            this.nonAggressionGroupBox.SuspendLayout();
            this.relationGroupBox.SuspendLayout();
            this.intelligenceGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spyNumNumericUpDown)).BeginInit();
            this.mainTabPage.SuspendLayout();
            this.scenarioOptionGroupBox.SuspendLayout();
            this.countrySelectionGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propagandaPictureBox)).BeginInit();
            this.scenarioInfoGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelPictureBox)).BeginInit();
            this.folderGroupBox.SuspendLayout();
            this.typeGroupBox.SuspendLayout();
            this.scenarioTabControl.SuspendLayout();
            this.oobTabPage.SuspendLayout();
            this.divisionGroupBox.SuspendLayout();
            this.unitGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // provinceIcLabel
            // 
            resources.ApplyResources(provinceIcLabel, "provinceIcLabel");
            provinceIcLabel.Name = "provinceIcLabel";
            // 
            // provinceInfrastructureLabel
            // 
            resources.ApplyResources(provinceInfrastructureLabel, "provinceInfrastructureLabel");
            provinceInfrastructureLabel.Name = "provinceInfrastructureLabel";
            // 
            // provinceLandFortLabel
            // 
            resources.ApplyResources(provinceLandFortLabel, "provinceLandFortLabel");
            provinceLandFortLabel.Name = "provinceLandFortLabel";
            // 
            // provinceCoastalFortLabel
            // 
            resources.ApplyResources(provinceCoastalFortLabel, "provinceCoastalFortLabel");
            provinceCoastalFortLabel.Name = "provinceCoastalFortLabel";
            // 
            // provinceNavalBaseLabel
            // 
            resources.ApplyResources(provinceNavalBaseLabel, "provinceNavalBaseLabel");
            provinceNavalBaseLabel.Name = "provinceNavalBaseLabel";
            // 
            // provinceAirBaseLabel
            // 
            resources.ApplyResources(provinceAirBaseLabel, "provinceAirBaseLabel");
            provinceAirBaseLabel.Name = "provinceAirBaseLabel";
            // 
            // provinceAntiAirLabel
            // 
            resources.ApplyResources(provinceAntiAirLabel, "provinceAntiAirLabel");
            provinceAntiAirLabel.Name = "provinceAntiAirLabel";
            // 
            // provinceNuclearReactorLabel
            // 
            resources.ApplyResources(provinceNuclearReactorLabel, "provinceNuclearReactorLabel");
            provinceNuclearReactorLabel.Name = "provinceNuclearReactorLabel";
            // 
            // provinceRocketTestLabel
            // 
            resources.ApplyResources(provinceRocketTestLabel, "provinceRocketTestLabel");
            provinceRocketTestLabel.Name = "provinceRocketTestLabel";
            // 
            // provinceRadarStationLbel
            // 
            resources.ApplyResources(provinceRadarStationLbel, "provinceRadarStationLbel");
            provinceRadarStationLbel.Name = "provinceRadarStationLbel";
            // 
            // buildingCurrentLabel1
            // 
            resources.ApplyResources(buildingCurrentLabel1, "buildingCurrentLabel1");
            buildingCurrentLabel1.Name = "buildingCurrentLabel1";
            // 
            // buildingMaxLabel1
            // 
            resources.ApplyResources(buildingMaxLabel1, "buildingMaxLabel1");
            buildingMaxLabel1.Name = "buildingMaxLabel1";
            // 
            // buildingRelativeLabel1
            // 
            resources.ApplyResources(buildingRelativeLabel1, "buildingRelativeLabel1");
            buildingRelativeLabel1.Name = "buildingRelativeLabel1";
            // 
            // provinceNameLabel
            // 
            resources.ApplyResources(provinceNameLabel, "provinceNameLabel");
            provinceNameLabel.Name = "provinceNameLabel";
            // 
            // provinceVpLabel
            // 
            resources.ApplyResources(provinceVpLabel, "provinceVpLabel");
            provinceVpLabel.Name = "provinceVpLabel";
            // 
            // provinceRevoltRiskLabel
            // 
            resources.ApplyResources(provinceRevoltRiskLabel, "provinceRevoltRiskLabel");
            provinceRevoltRiskLabel.Name = "provinceRevoltRiskLabel";
            // 
            // resourceMaxLabel
            // 
            resources.ApplyResources(resourceMaxLabel, "resourceMaxLabel");
            resourceMaxLabel.Name = "resourceMaxLabel";
            // 
            // resourceCurrentLabel
            // 
            resources.ApplyResources(resourceCurrentLabel, "resourceCurrentLabel");
            resourceCurrentLabel.Name = "resourceCurrentLabel";
            // 
            // resourcePoolLabel
            // 
            resources.ApplyResources(resourcePoolLabel, "resourcePoolLabel");
            resourcePoolLabel.Name = "resourcePoolLabel";
            // 
            // technologyTabPage
            // 
            technologyTabPage.BackColor = System.Drawing.SystemColors.Control;
            technologyTabPage.Controls.Add(this.techTreePanel);
            technologyTabPage.Controls.Add(this.inventionsListView);
            technologyTabPage.Controls.Add(this.blueprintsListView);
            technologyTabPage.Controls.Add(this.ownedTechsListView);
            technologyTabPage.Controls.Add(this.ownedTechsLabel);
            technologyTabPage.Controls.Add(this.techCategoryListBox);
            technologyTabPage.Controls.Add(this.blueprintsLabel);
            technologyTabPage.Controls.Add(this.inventionsLabel);
            technologyTabPage.Controls.Add(this.techCountryListBox);
            resources.ApplyResources(technologyTabPage, "technologyTabPage");
            technologyTabPage.Name = "technologyTabPage";
            // 
            // techTreePanel
            // 
            resources.ApplyResources(this.techTreePanel, "techTreePanel");
            this.techTreePanel.Controls.Add(this.techTreePictureBox);
            this.techTreePanel.Name = "techTreePanel";
            // 
            // techTreePictureBox
            // 
            resources.ApplyResources(this.techTreePictureBox, "techTreePictureBox");
            this.techTreePictureBox.Name = "techTreePictureBox";
            this.techTreePictureBox.TabStop = false;
            // 
            // inventionsListView
            // 
            resources.ApplyResources(this.inventionsListView, "inventionsListView");
            this.inventionsListView.CheckBoxes = true;
            this.inventionsListView.MultiSelect = false;
            this.inventionsListView.Name = "inventionsListView";
            this.inventionsListView.UseCompatibleStateImageBehavior = false;
            this.inventionsListView.View = System.Windows.Forms.View.List;
            // 
            // blueprintsListView
            // 
            resources.ApplyResources(this.blueprintsListView, "blueprintsListView");
            this.blueprintsListView.CheckBoxes = true;
            this.blueprintsListView.MultiSelect = false;
            this.blueprintsListView.Name = "blueprintsListView";
            this.blueprintsListView.UseCompatibleStateImageBehavior = false;
            this.blueprintsListView.View = System.Windows.Forms.View.List;
            // 
            // ownedTechsListView
            // 
            resources.ApplyResources(this.ownedTechsListView, "ownedTechsListView");
            this.ownedTechsListView.CheckBoxes = true;
            this.ownedTechsListView.MultiSelect = false;
            this.ownedTechsListView.Name = "ownedTechsListView";
            this.ownedTechsListView.UseCompatibleStateImageBehavior = false;
            this.ownedTechsListView.View = System.Windows.Forms.View.List;
            // 
            // ownedTechsLabel
            // 
            resources.ApplyResources(this.ownedTechsLabel, "ownedTechsLabel");
            this.ownedTechsLabel.Name = "ownedTechsLabel";
            // 
            // techCategoryListBox
            // 
            resources.ApplyResources(this.techCategoryListBox, "techCategoryListBox");
            this.techCategoryListBox.FormattingEnabled = true;
            this.techCategoryListBox.Name = "techCategoryListBox";
            this.techCategoryListBox.SelectedIndexChanged += new System.EventHandler(this.OnTechCategoryListBoxSelectedIndexChanged);
            // 
            // blueprintsLabel
            // 
            resources.ApplyResources(this.blueprintsLabel, "blueprintsLabel");
            this.blueprintsLabel.Name = "blueprintsLabel";
            // 
            // inventionsLabel
            // 
            resources.ApplyResources(this.inventionsLabel, "inventionsLabel");
            this.inventionsLabel.Name = "inventionsLabel";
            // 
            // techCountryListBox
            // 
            resources.ApplyResources(this.techCountryListBox, "techCountryListBox");
            this.techCountryListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.techCountryListBox.FormattingEnabled = true;
            this.techCountryListBox.Name = "techCountryListBox";
            this.techCountryListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnCountryListBoxDrawItem);
            this.techCountryListBox.SelectedIndexChanged += new System.EventHandler(this.OnTechCountryListBoxSelectedIndexChanged);
            // 
            // tradeStartDateLabel
            // 
            resources.ApplyResources(tradeStartDateLabel, "tradeStartDateLabel");
            tradeStartDateLabel.Name = "tradeStartDateLabel";
            // 
            // tradeEndDateLabel
            // 
            resources.ApplyResources(tradeEndDateLabel, "tradeEndDateLabel");
            tradeEndDateLabel.Name = "tradeEndDateLabel";
            // 
            // tradeIdLabel
            // 
            resources.ApplyResources(tradeIdLabel, "tradeIdLabel");
            tradeIdLabel.Name = "tradeIdLabel";
            // 
            // relationValueLabel
            // 
            resources.ApplyResources(relationValueLabel, "relationValueLabel");
            relationValueLabel.Name = "relationValueLabel";
            // 
            // spyNumLabel
            // 
            resources.ApplyResources(spyNumLabel, "spyNumLabel");
            spyNumLabel.Name = "spyNumLabel";
            // 
            // provinceSyntheticOilLabel
            // 
            resources.ApplyResources(this.provinceSyntheticOilLabel, "provinceSyntheticOilLabel");
            this.provinceSyntheticOilLabel.Name = "provinceSyntheticOilLabel";
            // 
            // provinceSyntheticRaresLabel
            // 
            resources.ApplyResources(this.provinceSyntheticRaresLabel, "provinceSyntheticRaresLabel");
            this.provinceSyntheticRaresLabel.Name = "provinceSyntheticRaresLabel";
            // 
            // provinceNuclearPowerLabel
            // 
            resources.ApplyResources(this.provinceNuclearPowerLabel, "provinceNuclearPowerLabel");
            this.provinceNuclearPowerLabel.Name = "provinceNuclearPowerLabel";
            // 
            // provinceIdLabel
            // 
            resources.ApplyResources(this.provinceIdLabel, "provinceIdLabel");
            this.provinceIdLabel.Name = "provinceIdLabel";
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
            // provinceTabPage
            // 
            this.provinceTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.provinceTabPage.Controls.Add(this.provinceIdTextBox);
            this.provinceTabPage.Controls.Add(this.provinceCountryFilterLabel);
            this.provinceTabPage.Controls.Add(this.provinceCountryFilterComboBox);
            this.provinceTabPage.Controls.Add(this.provinceCountryGroupBox);
            this.provinceTabPage.Controls.Add(this.mapFilterGroupBox);
            this.provinceTabPage.Controls.Add(this.provinceInfoGroupBox);
            this.provinceTabPage.Controls.Add(this.provinceIdLabel);
            this.provinceTabPage.Controls.Add(this.provinceResourceGroupBox);
            this.provinceTabPage.Controls.Add(this.provinceBuildingGroupBox);
            this.provinceTabPage.Controls.Add(this.provinceMapPanel);
            this.provinceTabPage.Controls.Add(this.provinceListView);
            resources.ApplyResources(this.provinceTabPage, "provinceTabPage");
            this.provinceTabPage.Name = "provinceTabPage";
            // 
            // provinceIdTextBox
            // 
            resources.ApplyResources(this.provinceIdTextBox, "provinceIdTextBox");
            this.provinceIdTextBox.Name = "provinceIdTextBox";
            this.provinceIdTextBox.Validated += new System.EventHandler(this.OnProvinceIdTextBoxValidated);
            // 
            // provinceCountryFilterLabel
            // 
            resources.ApplyResources(this.provinceCountryFilterLabel, "provinceCountryFilterLabel");
            this.provinceCountryFilterLabel.Name = "provinceCountryFilterLabel";
            // 
            // provinceCountryFilterComboBox
            // 
            resources.ApplyResources(this.provinceCountryFilterComboBox, "provinceCountryFilterComboBox");
            this.provinceCountryFilterComboBox.FormattingEnabled = true;
            this.provinceCountryFilterComboBox.Name = "provinceCountryFilterComboBox";
            this.provinceCountryFilterComboBox.SelectedIndexChanged += new System.EventHandler(this.OnProvinceCountryFilterComboBoxSelectedIndexChanged);
            // 
            // provinceCountryGroupBox
            // 
            resources.ApplyResources(this.provinceCountryGroupBox, "provinceCountryGroupBox");
            this.provinceCountryGroupBox.Controls.Add(this.claimedProvinceCheckBox);
            this.provinceCountryGroupBox.Controls.Add(this.capitalCheckBox);
            this.provinceCountryGroupBox.Controls.Add(this.controlledProvinceCheckBox);
            this.provinceCountryGroupBox.Controls.Add(this.coreProvinceCheckBox);
            this.provinceCountryGroupBox.Controls.Add(this.ownedProvinceCheckBox);
            this.provinceCountryGroupBox.Name = "provinceCountryGroupBox";
            this.provinceCountryGroupBox.TabStop = false;
            // 
            // claimedProvinceCheckBox
            // 
            resources.ApplyResources(this.claimedProvinceCheckBox, "claimedProvinceCheckBox");
            this.claimedProvinceCheckBox.Name = "claimedProvinceCheckBox";
            this.claimedProvinceCheckBox.UseVisualStyleBackColor = true;
            this.claimedProvinceCheckBox.CheckedChanged += new System.EventHandler(this.OnProvinceCheckBoxCheckedChanged);
            // 
            // capitalCheckBox
            // 
            resources.ApplyResources(this.capitalCheckBox, "capitalCheckBox");
            this.capitalCheckBox.Name = "capitalCheckBox";
            this.capitalCheckBox.UseVisualStyleBackColor = true;
            this.capitalCheckBox.CheckedChanged += new System.EventHandler(this.OnProvinceCheckBoxCheckedChanged);
            // 
            // controlledProvinceCheckBox
            // 
            resources.ApplyResources(this.controlledProvinceCheckBox, "controlledProvinceCheckBox");
            this.controlledProvinceCheckBox.Name = "controlledProvinceCheckBox";
            this.controlledProvinceCheckBox.UseVisualStyleBackColor = true;
            this.controlledProvinceCheckBox.CheckedChanged += new System.EventHandler(this.OnProvinceCheckBoxCheckedChanged);
            // 
            // coreProvinceCheckBox
            // 
            resources.ApplyResources(this.coreProvinceCheckBox, "coreProvinceCheckBox");
            this.coreProvinceCheckBox.Name = "coreProvinceCheckBox";
            this.coreProvinceCheckBox.UseVisualStyleBackColor = true;
            this.coreProvinceCheckBox.CheckedChanged += new System.EventHandler(this.OnProvinceCheckBoxCheckedChanged);
            // 
            // ownedProvinceCheckBox
            // 
            resources.ApplyResources(this.ownedProvinceCheckBox, "ownedProvinceCheckBox");
            this.ownedProvinceCheckBox.Name = "ownedProvinceCheckBox";
            this.ownedProvinceCheckBox.UseVisualStyleBackColor = true;
            this.ownedProvinceCheckBox.CheckedChanged += new System.EventHandler(this.OnProvinceCheckBoxCheckedChanged);
            // 
            // mapFilterGroupBox
            // 
            this.mapFilterGroupBox.Controls.Add(this.mapFilterClaimedRadioButton);
            this.mapFilterGroupBox.Controls.Add(this.mapFilterControlledRadioButton);
            this.mapFilterGroupBox.Controls.Add(this.mapFilterOwnedRadioButton);
            this.mapFilterGroupBox.Controls.Add(this.mapFilterCoreRadioButton);
            this.mapFilterGroupBox.Controls.Add(this.mapFilterNoneRadioButton);
            resources.ApplyResources(this.mapFilterGroupBox, "mapFilterGroupBox");
            this.mapFilterGroupBox.Name = "mapFilterGroupBox";
            this.mapFilterGroupBox.TabStop = false;
            // 
            // mapFilterClaimedRadioButton
            // 
            resources.ApplyResources(this.mapFilterClaimedRadioButton, "mapFilterClaimedRadioButton");
            this.mapFilterClaimedRadioButton.Name = "mapFilterClaimedRadioButton";
            this.mapFilterClaimedRadioButton.UseVisualStyleBackColor = true;
            this.mapFilterClaimedRadioButton.CheckedChanged += new System.EventHandler(this.OnMapFilterRadioButtonCheckedChanged);
            // 
            // mapFilterControlledRadioButton
            // 
            resources.ApplyResources(this.mapFilterControlledRadioButton, "mapFilterControlledRadioButton");
            this.mapFilterControlledRadioButton.Name = "mapFilterControlledRadioButton";
            this.mapFilterControlledRadioButton.UseVisualStyleBackColor = true;
            this.mapFilterControlledRadioButton.CheckedChanged += new System.EventHandler(this.OnMapFilterRadioButtonCheckedChanged);
            // 
            // mapFilterOwnedRadioButton
            // 
            resources.ApplyResources(this.mapFilterOwnedRadioButton, "mapFilterOwnedRadioButton");
            this.mapFilterOwnedRadioButton.Name = "mapFilterOwnedRadioButton";
            this.mapFilterOwnedRadioButton.UseVisualStyleBackColor = true;
            this.mapFilterOwnedRadioButton.CheckedChanged += new System.EventHandler(this.OnMapFilterRadioButtonCheckedChanged);
            // 
            // mapFilterCoreRadioButton
            // 
            resources.ApplyResources(this.mapFilterCoreRadioButton, "mapFilterCoreRadioButton");
            this.mapFilterCoreRadioButton.Name = "mapFilterCoreRadioButton";
            this.mapFilterCoreRadioButton.UseVisualStyleBackColor = true;
            this.mapFilterCoreRadioButton.CheckedChanged += new System.EventHandler(this.OnMapFilterRadioButtonCheckedChanged);
            // 
            // mapFilterNoneRadioButton
            // 
            resources.ApplyResources(this.mapFilterNoneRadioButton, "mapFilterNoneRadioButton");
            this.mapFilterNoneRadioButton.Checked = true;
            this.mapFilterNoneRadioButton.Name = "mapFilterNoneRadioButton";
            this.mapFilterNoneRadioButton.TabStop = true;
            this.mapFilterNoneRadioButton.UseVisualStyleBackColor = true;
            this.mapFilterNoneRadioButton.CheckedChanged += new System.EventHandler(this.OnMapFilterRadioButtonCheckedChanged);
            // 
            // provinceInfoGroupBox
            // 
            resources.ApplyResources(this.provinceInfoGroupBox, "provinceInfoGroupBox");
            this.provinceInfoGroupBox.Controls.Add(this.provinceNameStringTextBox);
            this.provinceInfoGroupBox.Controls.Add(this.revoltRiskTextBox);
            this.provinceInfoGroupBox.Controls.Add(provinceNameLabel);
            this.provinceInfoGroupBox.Controls.Add(this.vpTextBox);
            this.provinceInfoGroupBox.Controls.Add(this.provinceNameKeyTextBox);
            this.provinceInfoGroupBox.Controls.Add(provinceVpLabel);
            this.provinceInfoGroupBox.Controls.Add(provinceRevoltRiskLabel);
            this.provinceInfoGroupBox.Name = "provinceInfoGroupBox";
            this.provinceInfoGroupBox.TabStop = false;
            // 
            // provinceNameStringTextBox
            // 
            resources.ApplyResources(this.provinceNameStringTextBox, "provinceNameStringTextBox");
            this.provinceNameStringTextBox.Name = "provinceNameStringTextBox";
            this.provinceNameStringTextBox.TextChanged += new System.EventHandler(this.OnProvinceStringItemTextBoxTextChanged);
            // 
            // revoltRiskTextBox
            // 
            resources.ApplyResources(this.revoltRiskTextBox, "revoltRiskTextBox");
            this.revoltRiskTextBox.Name = "revoltRiskTextBox";
            this.revoltRiskTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // vpTextBox
            // 
            resources.ApplyResources(this.vpTextBox, "vpTextBox");
            this.vpTextBox.Name = "vpTextBox";
            this.vpTextBox.Validated += new System.EventHandler(this.OnProvinceIntItemTextBoxValidated);
            // 
            // provinceNameKeyTextBox
            // 
            resources.ApplyResources(this.provinceNameKeyTextBox, "provinceNameKeyTextBox");
            this.provinceNameKeyTextBox.Name = "provinceNameKeyTextBox";
            this.provinceNameKeyTextBox.TextChanged += new System.EventHandler(this.OnProvinceStringItemTextBoxTextChanged);
            // 
            // provinceResourceGroupBox
            // 
            resources.ApplyResources(this.provinceResourceGroupBox, "provinceResourceGroupBox");
            this.provinceResourceGroupBox.Controls.Add(this.manpowerCurrentTextBox);
            this.provinceResourceGroupBox.Controls.Add(this.suppliesPoolTextBox);
            this.provinceResourceGroupBox.Controls.Add(this.oilMaxTextBox);
            this.provinceResourceGroupBox.Controls.Add(this.manpowerMaxTextBox);
            this.provinceResourceGroupBox.Controls.Add(this.oilCurrentTextBox);
            this.provinceResourceGroupBox.Controls.Add(this.oilPoolTextBox);
            this.provinceResourceGroupBox.Controls.Add(this.rareMaterialsMaxTextBox);
            this.provinceResourceGroupBox.Controls.Add(this.rareMaterialsCurrentTextBox);
            this.provinceResourceGroupBox.Controls.Add(this.rareMaterialsPoolTextBox);
            this.provinceResourceGroupBox.Controls.Add(this.metalMaxTextBox);
            this.provinceResourceGroupBox.Controls.Add(this.metalCurrentTextBox);
            this.provinceResourceGroupBox.Controls.Add(this.metalPoolTextBox);
            this.provinceResourceGroupBox.Controls.Add(this.energyMaxTextBox);
            this.provinceResourceGroupBox.Controls.Add(this.energyCurrentTextBox);
            this.provinceResourceGroupBox.Controls.Add(this.provinceSuppliesLabel);
            this.provinceResourceGroupBox.Controls.Add(this.energyPoolTextBox);
            this.provinceResourceGroupBox.Controls.Add(resourceMaxLabel);
            this.provinceResourceGroupBox.Controls.Add(resourceCurrentLabel);
            this.provinceResourceGroupBox.Controls.Add(resourcePoolLabel);
            this.provinceResourceGroupBox.Controls.Add(this.provinceEnergyLabel);
            this.provinceResourceGroupBox.Controls.Add(this.provinceOilLabel);
            this.provinceResourceGroupBox.Controls.Add(this.provinceMetalLabel);
            this.provinceResourceGroupBox.Controls.Add(this.provinceRareMaterialsLabel);
            this.provinceResourceGroupBox.Controls.Add(this.provinceManpowerLabel);
            this.provinceResourceGroupBox.Name = "provinceResourceGroupBox";
            this.provinceResourceGroupBox.TabStop = false;
            // 
            // manpowerCurrentTextBox
            // 
            resources.ApplyResources(this.manpowerCurrentTextBox, "manpowerCurrentTextBox");
            this.manpowerCurrentTextBox.Name = "manpowerCurrentTextBox";
            this.manpowerCurrentTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // suppliesPoolTextBox
            // 
            resources.ApplyResources(this.suppliesPoolTextBox, "suppliesPoolTextBox");
            this.suppliesPoolTextBox.Name = "suppliesPoolTextBox";
            this.suppliesPoolTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // oilMaxTextBox
            // 
            resources.ApplyResources(this.oilMaxTextBox, "oilMaxTextBox");
            this.oilMaxTextBox.Name = "oilMaxTextBox";
            this.oilMaxTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // manpowerMaxTextBox
            // 
            resources.ApplyResources(this.manpowerMaxTextBox, "manpowerMaxTextBox");
            this.manpowerMaxTextBox.Name = "manpowerMaxTextBox";
            this.manpowerMaxTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // oilCurrentTextBox
            // 
            resources.ApplyResources(this.oilCurrentTextBox, "oilCurrentTextBox");
            this.oilCurrentTextBox.Name = "oilCurrentTextBox";
            this.oilCurrentTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // oilPoolTextBox
            // 
            resources.ApplyResources(this.oilPoolTextBox, "oilPoolTextBox");
            this.oilPoolTextBox.Name = "oilPoolTextBox";
            this.oilPoolTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // rareMaterialsMaxTextBox
            // 
            resources.ApplyResources(this.rareMaterialsMaxTextBox, "rareMaterialsMaxTextBox");
            this.rareMaterialsMaxTextBox.Name = "rareMaterialsMaxTextBox";
            this.rareMaterialsMaxTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // rareMaterialsCurrentTextBox
            // 
            resources.ApplyResources(this.rareMaterialsCurrentTextBox, "rareMaterialsCurrentTextBox");
            this.rareMaterialsCurrentTextBox.Name = "rareMaterialsCurrentTextBox";
            this.rareMaterialsCurrentTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // rareMaterialsPoolTextBox
            // 
            resources.ApplyResources(this.rareMaterialsPoolTextBox, "rareMaterialsPoolTextBox");
            this.rareMaterialsPoolTextBox.Name = "rareMaterialsPoolTextBox";
            this.rareMaterialsPoolTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // metalMaxTextBox
            // 
            resources.ApplyResources(this.metalMaxTextBox, "metalMaxTextBox");
            this.metalMaxTextBox.Name = "metalMaxTextBox";
            this.metalMaxTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // metalCurrentTextBox
            // 
            resources.ApplyResources(this.metalCurrentTextBox, "metalCurrentTextBox");
            this.metalCurrentTextBox.Name = "metalCurrentTextBox";
            this.metalCurrentTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // metalPoolTextBox
            // 
            resources.ApplyResources(this.metalPoolTextBox, "metalPoolTextBox");
            this.metalPoolTextBox.Name = "metalPoolTextBox";
            this.metalPoolTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // energyMaxTextBox
            // 
            resources.ApplyResources(this.energyMaxTextBox, "energyMaxTextBox");
            this.energyMaxTextBox.Name = "energyMaxTextBox";
            this.energyMaxTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // energyCurrentTextBox
            // 
            resources.ApplyResources(this.energyCurrentTextBox, "energyCurrentTextBox");
            this.energyCurrentTextBox.Name = "energyCurrentTextBox";
            this.energyCurrentTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // provinceSuppliesLabel
            // 
            resources.ApplyResources(this.provinceSuppliesLabel, "provinceSuppliesLabel");
            this.provinceSuppliesLabel.Name = "provinceSuppliesLabel";
            // 
            // energyPoolTextBox
            // 
            resources.ApplyResources(this.energyPoolTextBox, "energyPoolTextBox");
            this.energyPoolTextBox.Name = "energyPoolTextBox";
            this.energyPoolTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // provinceEnergyLabel
            // 
            resources.ApplyResources(this.provinceEnergyLabel, "provinceEnergyLabel");
            this.provinceEnergyLabel.Name = "provinceEnergyLabel";
            // 
            // provinceOilLabel
            // 
            resources.ApplyResources(this.provinceOilLabel, "provinceOilLabel");
            this.provinceOilLabel.Name = "provinceOilLabel";
            // 
            // provinceMetalLabel
            // 
            resources.ApplyResources(this.provinceMetalLabel, "provinceMetalLabel");
            this.provinceMetalLabel.Name = "provinceMetalLabel";
            // 
            // provinceRareMaterialsLabel
            // 
            resources.ApplyResources(this.provinceRareMaterialsLabel, "provinceRareMaterialsLabel");
            this.provinceRareMaterialsLabel.Name = "provinceRareMaterialsLabel";
            // 
            // provinceManpowerLabel
            // 
            resources.ApplyResources(this.provinceManpowerLabel, "provinceManpowerLabel");
            this.provinceManpowerLabel.Name = "provinceManpowerLabel";
            // 
            // provinceBuildingGroupBox
            // 
            resources.ApplyResources(this.provinceBuildingGroupBox, "provinceBuildingGroupBox");
            this.provinceBuildingGroupBox.Controls.Add(this.nuclearPowerRelativeTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.nuclearPowerMaxTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.nuclearPowerCurrentTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.syntheticRaresRelativeTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.syntheticRaresMaxTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.syntheticRaresCurrentTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.syntheticOilRelativeTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.syntheticOilMaxTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.syntheticOilCurrentTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.rocketTestRelativeTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.rocketTestMaxTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.rocketTestCurrentTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.infrastructureRelativeTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.infrastructureMaxTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.infrastructureCurrentTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.icRelativeTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.icMaxTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.icCurrentTextBox);
            this.provinceBuildingGroupBox.Controls.Add(provinceInfrastructureLabel);
            this.provinceBuildingGroupBox.Controls.Add(this.nuclearReactorRelativeTextBox);
            this.provinceBuildingGroupBox.Controls.Add(provinceIcLabel);
            this.provinceBuildingGroupBox.Controls.Add(this.nuclearReactorMaxTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.nuclearReactorCurrentTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.radarStationRelativeTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.radarStationMaxTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.radarStationCurrentTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.navalBaseRelativeTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.navalBaseMaxTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.navalBaseCurrentTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.airBaseRelativeTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.airBaseMaxTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.airBaseCurrentTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.antiAirRelativeTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.antiAirMaxTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.antiAirCurrentTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.coastalFortRelativeTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.coastalFortMaxTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.coastalFortCurrentTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.landFortRelativeTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.landFortMaxTextBox);
            this.provinceBuildingGroupBox.Controls.Add(this.landFortCurrentTextBox);
            this.provinceBuildingGroupBox.Controls.Add(buildingRelativeLabel1);
            this.provinceBuildingGroupBox.Controls.Add(buildingMaxLabel1);
            this.provinceBuildingGroupBox.Controls.Add(buildingCurrentLabel1);
            this.provinceBuildingGroupBox.Controls.Add(provinceLandFortLabel);
            this.provinceBuildingGroupBox.Controls.Add(provinceCoastalFortLabel);
            this.provinceBuildingGroupBox.Controls.Add(provinceAntiAirLabel);
            this.provinceBuildingGroupBox.Controls.Add(provinceAirBaseLabel);
            this.provinceBuildingGroupBox.Controls.Add(provinceNavalBaseLabel);
            this.provinceBuildingGroupBox.Controls.Add(provinceRadarStationLbel);
            this.provinceBuildingGroupBox.Controls.Add(this.provinceNuclearPowerLabel);
            this.provinceBuildingGroupBox.Controls.Add(provinceRocketTestLabel);
            this.provinceBuildingGroupBox.Controls.Add(this.provinceSyntheticOilLabel);
            this.provinceBuildingGroupBox.Controls.Add(provinceNuclearReactorLabel);
            this.provinceBuildingGroupBox.Controls.Add(this.provinceSyntheticRaresLabel);
            this.provinceBuildingGroupBox.Name = "provinceBuildingGroupBox";
            this.provinceBuildingGroupBox.TabStop = false;
            // 
            // nuclearPowerRelativeTextBox
            // 
            resources.ApplyResources(this.nuclearPowerRelativeTextBox, "nuclearPowerRelativeTextBox");
            this.nuclearPowerRelativeTextBox.Name = "nuclearPowerRelativeTextBox";
            this.nuclearPowerRelativeTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // nuclearPowerMaxTextBox
            // 
            resources.ApplyResources(this.nuclearPowerMaxTextBox, "nuclearPowerMaxTextBox");
            this.nuclearPowerMaxTextBox.Name = "nuclearPowerMaxTextBox";
            this.nuclearPowerMaxTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // nuclearPowerCurrentTextBox
            // 
            resources.ApplyResources(this.nuclearPowerCurrentTextBox, "nuclearPowerCurrentTextBox");
            this.nuclearPowerCurrentTextBox.Name = "nuclearPowerCurrentTextBox";
            this.nuclearPowerCurrentTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // syntheticRaresRelativeTextBox
            // 
            resources.ApplyResources(this.syntheticRaresRelativeTextBox, "syntheticRaresRelativeTextBox");
            this.syntheticRaresRelativeTextBox.Name = "syntheticRaresRelativeTextBox";
            this.syntheticRaresRelativeTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // syntheticRaresMaxTextBox
            // 
            resources.ApplyResources(this.syntheticRaresMaxTextBox, "syntheticRaresMaxTextBox");
            this.syntheticRaresMaxTextBox.Name = "syntheticRaresMaxTextBox";
            this.syntheticRaresMaxTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // syntheticRaresCurrentTextBox
            // 
            resources.ApplyResources(this.syntheticRaresCurrentTextBox, "syntheticRaresCurrentTextBox");
            this.syntheticRaresCurrentTextBox.Name = "syntheticRaresCurrentTextBox";
            this.syntheticRaresCurrentTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // syntheticOilRelativeTextBox
            // 
            resources.ApplyResources(this.syntheticOilRelativeTextBox, "syntheticOilRelativeTextBox");
            this.syntheticOilRelativeTextBox.Name = "syntheticOilRelativeTextBox";
            this.syntheticOilRelativeTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // syntheticOilMaxTextBox
            // 
            resources.ApplyResources(this.syntheticOilMaxTextBox, "syntheticOilMaxTextBox");
            this.syntheticOilMaxTextBox.Name = "syntheticOilMaxTextBox";
            this.syntheticOilMaxTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // syntheticOilCurrentTextBox
            // 
            resources.ApplyResources(this.syntheticOilCurrentTextBox, "syntheticOilCurrentTextBox");
            this.syntheticOilCurrentTextBox.Name = "syntheticOilCurrentTextBox";
            this.syntheticOilCurrentTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // rocketTestRelativeTextBox
            // 
            resources.ApplyResources(this.rocketTestRelativeTextBox, "rocketTestRelativeTextBox");
            this.rocketTestRelativeTextBox.Name = "rocketTestRelativeTextBox";
            this.rocketTestRelativeTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // rocketTestMaxTextBox
            // 
            resources.ApplyResources(this.rocketTestMaxTextBox, "rocketTestMaxTextBox");
            this.rocketTestMaxTextBox.Name = "rocketTestMaxTextBox";
            this.rocketTestMaxTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // rocketTestCurrentTextBox
            // 
            resources.ApplyResources(this.rocketTestCurrentTextBox, "rocketTestCurrentTextBox");
            this.rocketTestCurrentTextBox.Name = "rocketTestCurrentTextBox";
            this.rocketTestCurrentTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // infrastructureRelativeTextBox
            // 
            resources.ApplyResources(this.infrastructureRelativeTextBox, "infrastructureRelativeTextBox");
            this.infrastructureRelativeTextBox.Name = "infrastructureRelativeTextBox";
            this.infrastructureRelativeTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // infrastructureMaxTextBox
            // 
            resources.ApplyResources(this.infrastructureMaxTextBox, "infrastructureMaxTextBox");
            this.infrastructureMaxTextBox.Name = "infrastructureMaxTextBox";
            this.infrastructureMaxTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // infrastructureCurrentTextBox
            // 
            resources.ApplyResources(this.infrastructureCurrentTextBox, "infrastructureCurrentTextBox");
            this.infrastructureCurrentTextBox.Name = "infrastructureCurrentTextBox";
            this.infrastructureCurrentTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // icRelativeTextBox
            // 
            resources.ApplyResources(this.icRelativeTextBox, "icRelativeTextBox");
            this.icRelativeTextBox.Name = "icRelativeTextBox";
            this.icRelativeTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // icMaxTextBox
            // 
            resources.ApplyResources(this.icMaxTextBox, "icMaxTextBox");
            this.icMaxTextBox.Name = "icMaxTextBox";
            this.icMaxTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // icCurrentTextBox
            // 
            resources.ApplyResources(this.icCurrentTextBox, "icCurrentTextBox");
            this.icCurrentTextBox.Name = "icCurrentTextBox";
            this.icCurrentTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // nuclearReactorRelativeTextBox
            // 
            resources.ApplyResources(this.nuclearReactorRelativeTextBox, "nuclearReactorRelativeTextBox");
            this.nuclearReactorRelativeTextBox.Name = "nuclearReactorRelativeTextBox";
            this.nuclearReactorRelativeTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // nuclearReactorMaxTextBox
            // 
            resources.ApplyResources(this.nuclearReactorMaxTextBox, "nuclearReactorMaxTextBox");
            this.nuclearReactorMaxTextBox.Name = "nuclearReactorMaxTextBox";
            this.nuclearReactorMaxTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // nuclearReactorCurrentTextBox
            // 
            resources.ApplyResources(this.nuclearReactorCurrentTextBox, "nuclearReactorCurrentTextBox");
            this.nuclearReactorCurrentTextBox.Name = "nuclearReactorCurrentTextBox";
            this.nuclearReactorCurrentTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // radarStationRelativeTextBox
            // 
            resources.ApplyResources(this.radarStationRelativeTextBox, "radarStationRelativeTextBox");
            this.radarStationRelativeTextBox.Name = "radarStationRelativeTextBox";
            this.radarStationRelativeTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // radarStationMaxTextBox
            // 
            resources.ApplyResources(this.radarStationMaxTextBox, "radarStationMaxTextBox");
            this.radarStationMaxTextBox.Name = "radarStationMaxTextBox";
            this.radarStationMaxTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // radarStationCurrentTextBox
            // 
            resources.ApplyResources(this.radarStationCurrentTextBox, "radarStationCurrentTextBox");
            this.radarStationCurrentTextBox.Name = "radarStationCurrentTextBox";
            this.radarStationCurrentTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // navalBaseRelativeTextBox
            // 
            resources.ApplyResources(this.navalBaseRelativeTextBox, "navalBaseRelativeTextBox");
            this.navalBaseRelativeTextBox.Name = "navalBaseRelativeTextBox";
            this.navalBaseRelativeTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // navalBaseMaxTextBox
            // 
            resources.ApplyResources(this.navalBaseMaxTextBox, "navalBaseMaxTextBox");
            this.navalBaseMaxTextBox.Name = "navalBaseMaxTextBox";
            this.navalBaseMaxTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // navalBaseCurrentTextBox
            // 
            resources.ApplyResources(this.navalBaseCurrentTextBox, "navalBaseCurrentTextBox");
            this.navalBaseCurrentTextBox.Name = "navalBaseCurrentTextBox";
            this.navalBaseCurrentTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // airBaseRelativeTextBox
            // 
            resources.ApplyResources(this.airBaseRelativeTextBox, "airBaseRelativeTextBox");
            this.airBaseRelativeTextBox.Name = "airBaseRelativeTextBox";
            this.airBaseRelativeTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // airBaseMaxTextBox
            // 
            resources.ApplyResources(this.airBaseMaxTextBox, "airBaseMaxTextBox");
            this.airBaseMaxTextBox.Name = "airBaseMaxTextBox";
            this.airBaseMaxTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // airBaseCurrentTextBox
            // 
            resources.ApplyResources(this.airBaseCurrentTextBox, "airBaseCurrentTextBox");
            this.airBaseCurrentTextBox.Name = "airBaseCurrentTextBox";
            this.airBaseCurrentTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // antiAirRelativeTextBox
            // 
            resources.ApplyResources(this.antiAirRelativeTextBox, "antiAirRelativeTextBox");
            this.antiAirRelativeTextBox.Name = "antiAirRelativeTextBox";
            this.antiAirRelativeTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // antiAirMaxTextBox
            // 
            resources.ApplyResources(this.antiAirMaxTextBox, "antiAirMaxTextBox");
            this.antiAirMaxTextBox.Name = "antiAirMaxTextBox";
            this.antiAirMaxTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // antiAirCurrentTextBox
            // 
            resources.ApplyResources(this.antiAirCurrentTextBox, "antiAirCurrentTextBox");
            this.antiAirCurrentTextBox.Name = "antiAirCurrentTextBox";
            this.antiAirCurrentTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // coastalFortRelativeTextBox
            // 
            resources.ApplyResources(this.coastalFortRelativeTextBox, "coastalFortRelativeTextBox");
            this.coastalFortRelativeTextBox.Name = "coastalFortRelativeTextBox";
            this.coastalFortRelativeTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // coastalFortMaxTextBox
            // 
            resources.ApplyResources(this.coastalFortMaxTextBox, "coastalFortMaxTextBox");
            this.coastalFortMaxTextBox.Name = "coastalFortMaxTextBox";
            this.coastalFortMaxTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // coastalFortCurrentTextBox
            // 
            resources.ApplyResources(this.coastalFortCurrentTextBox, "coastalFortCurrentTextBox");
            this.coastalFortCurrentTextBox.Name = "coastalFortCurrentTextBox";
            this.coastalFortCurrentTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // landFortRelativeTextBox
            // 
            resources.ApplyResources(this.landFortRelativeTextBox, "landFortRelativeTextBox");
            this.landFortRelativeTextBox.Name = "landFortRelativeTextBox";
            this.landFortRelativeTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // landFortMaxTextBox
            // 
            resources.ApplyResources(this.landFortMaxTextBox, "landFortMaxTextBox");
            this.landFortMaxTextBox.Name = "landFortMaxTextBox";
            this.landFortMaxTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // landFortCurrentTextBox
            // 
            resources.ApplyResources(this.landFortCurrentTextBox, "landFortCurrentTextBox");
            this.landFortCurrentTextBox.Name = "landFortCurrentTextBox";
            this.landFortCurrentTextBox.Validated += new System.EventHandler(this.OnProvinceDoubleItemTextBoxValidated);
            // 
            // provinceMapPanel
            // 
            this.provinceMapPanel.AllowDrop = true;
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
            // provinceListView
            // 
            resources.ApplyResources(this.provinceListView, "provinceListView");
            this.provinceListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.provinceIdColumnHeader,
            this.provinceNameColumnHeader,
            this.capitalProvinceColumnHeader,
            this.coreProvinceColumnHeader,
            this.ownedColumnHeader,
            this.controlledProvinceColumnHeader,
            this.claimedProvinceColumnHeader});
            this.provinceListView.FullRowSelect = true;
            this.provinceListView.GridLines = true;
            this.provinceListView.HideSelection = false;
            this.provinceListView.MultiSelect = false;
            this.provinceListView.Name = "provinceListView";
            this.provinceListView.UseCompatibleStateImageBehavior = false;
            this.provinceListView.View = System.Windows.Forms.View.Details;
            this.provinceListView.SelectedIndexChanged += new System.EventHandler(this.OnProvinceListViewSelectedIndexChanged);
            // 
            // provinceIdColumnHeader
            // 
            resources.ApplyResources(this.provinceIdColumnHeader, "provinceIdColumnHeader");
            // 
            // provinceNameColumnHeader
            // 
            resources.ApplyResources(this.provinceNameColumnHeader, "provinceNameColumnHeader");
            // 
            // capitalProvinceColumnHeader
            // 
            resources.ApplyResources(this.capitalProvinceColumnHeader, "capitalProvinceColumnHeader");
            // 
            // coreProvinceColumnHeader
            // 
            resources.ApplyResources(this.coreProvinceColumnHeader, "coreProvinceColumnHeader");
            // 
            // ownedColumnHeader
            // 
            resources.ApplyResources(this.ownedColumnHeader, "ownedColumnHeader");
            // 
            // controlledProvinceColumnHeader
            // 
            resources.ApplyResources(this.controlledProvinceColumnHeader, "controlledProvinceColumnHeader");
            // 
            // claimedProvinceColumnHeader
            // 
            resources.ApplyResources(this.claimedProvinceColumnHeader, "claimedProvinceColumnHeader");
            // 
            // governmentTabPage
            // 
            this.governmentTabPage.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.governmentTabPage, "governmentTabPage");
            this.governmentTabPage.Name = "governmentTabPage";
            // 
            // countryTabPage
            // 
            this.countryTabPage.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.countryTabPage, "countryTabPage");
            this.countryTabPage.Name = "countryTabPage";
            // 
            // tradeTabPage
            // 
            this.tradeTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.tradeTabPage.Controls.Add(this.tradeDealsGroupBox);
            this.tradeTabPage.Controls.Add(this.tradeInfoGroupBox);
            this.tradeTabPage.Controls.Add(this.tradeListView);
            this.tradeTabPage.Controls.Add(this.tradeDownButton);
            this.tradeTabPage.Controls.Add(this.tradeNewButton);
            this.tradeTabPage.Controls.Add(this.tradeRemoveButton);
            this.tradeTabPage.Controls.Add(this.tradeUpButton);
            resources.ApplyResources(this.tradeTabPage, "tradeTabPage");
            this.tradeTabPage.Name = "tradeTabPage";
            // 
            // tradeDealsGroupBox
            // 
            resources.ApplyResources(this.tradeDealsGroupBox, "tradeDealsGroupBox");
            this.tradeDealsGroupBox.Controls.Add(this.tradeCountryComboBox1);
            this.tradeDealsGroupBox.Controls.Add(this.tradeRareMaterialsTextBox1);
            this.tradeDealsGroupBox.Controls.Add(this.tradeRareMaterialsTextBox2);
            this.tradeDealsGroupBox.Controls.Add(this.tradeRareMaterialsLabel);
            this.tradeDealsGroupBox.Controls.Add(this.tradeOilLabel);
            this.tradeDealsGroupBox.Controls.Add(this.tradeMetalTextBox2);
            this.tradeDealsGroupBox.Controls.Add(this.tradeOilTextBox1);
            this.tradeDealsGroupBox.Controls.Add(this.tradeMetalTextBox1);
            this.tradeDealsGroupBox.Controls.Add(this.tradeSwapButton);
            this.tradeDealsGroupBox.Controls.Add(this.tradeOilTextBox2);
            this.tradeDealsGroupBox.Controls.Add(this.tradeMoneyTextBox2);
            this.tradeDealsGroupBox.Controls.Add(this.tradeMetalLabel);
            this.tradeDealsGroupBox.Controls.Add(this.tradeSuppliesLabel);
            this.tradeDealsGroupBox.Controls.Add(this.tradeMoneyTextBox1);
            this.tradeDealsGroupBox.Controls.Add(this.tradeEnergyTextBox2);
            this.tradeDealsGroupBox.Controls.Add(this.tradeCountryComboBox2);
            this.tradeDealsGroupBox.Controls.Add(this.tradeSuppliesTextBox1);
            this.tradeDealsGroupBox.Controls.Add(this.tradeMoneyLabel);
            this.tradeDealsGroupBox.Controls.Add(this.tradeEnergyTextBox1);
            this.tradeDealsGroupBox.Controls.Add(this.tradeEnergyLabel);
            this.tradeDealsGroupBox.Controls.Add(this.tradeSuppliesTextBox2);
            this.tradeDealsGroupBox.Name = "tradeDealsGroupBox";
            this.tradeDealsGroupBox.TabStop = false;
            // 
            // tradeCountryComboBox1
            // 
            this.tradeCountryComboBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.tradeCountryComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.tradeCountryComboBox1, "tradeCountryComboBox1");
            this.tradeCountryComboBox1.FormattingEnabled = true;
            this.tradeCountryComboBox1.Name = "tradeCountryComboBox1";
            this.tradeCountryComboBox1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnTradeCountryItemComboBoxDrawItem);
            this.tradeCountryComboBox1.SelectedIndexChanged += new System.EventHandler(this.OnTradeCountryItemComboBoxSelectedIndexChanged);
            // 
            // tradeRareMaterialsTextBox1
            // 
            resources.ApplyResources(this.tradeRareMaterialsTextBox1, "tradeRareMaterialsTextBox1");
            this.tradeRareMaterialsTextBox1.Name = "tradeRareMaterialsTextBox1";
            this.tradeRareMaterialsTextBox1.Validated += new System.EventHandler(this.OnTradeDoubleItemTextBoxValidated);
            // 
            // tradeRareMaterialsTextBox2
            // 
            resources.ApplyResources(this.tradeRareMaterialsTextBox2, "tradeRareMaterialsTextBox2");
            this.tradeRareMaterialsTextBox2.Name = "tradeRareMaterialsTextBox2";
            this.tradeRareMaterialsTextBox2.Validated += new System.EventHandler(this.OnTradeDoubleItemTextBoxValidated);
            // 
            // tradeRareMaterialsLabel
            // 
            resources.ApplyResources(this.tradeRareMaterialsLabel, "tradeRareMaterialsLabel");
            this.tradeRareMaterialsLabel.Name = "tradeRareMaterialsLabel";
            // 
            // tradeOilLabel
            // 
            resources.ApplyResources(this.tradeOilLabel, "tradeOilLabel");
            this.tradeOilLabel.Name = "tradeOilLabel";
            // 
            // tradeMetalTextBox2
            // 
            resources.ApplyResources(this.tradeMetalTextBox2, "tradeMetalTextBox2");
            this.tradeMetalTextBox2.Name = "tradeMetalTextBox2";
            this.tradeMetalTextBox2.Validated += new System.EventHandler(this.OnTradeDoubleItemTextBoxValidated);
            // 
            // tradeOilTextBox1
            // 
            resources.ApplyResources(this.tradeOilTextBox1, "tradeOilTextBox1");
            this.tradeOilTextBox1.Name = "tradeOilTextBox1";
            this.tradeOilTextBox1.Validated += new System.EventHandler(this.OnTradeDoubleItemTextBoxValidated);
            // 
            // tradeMetalTextBox1
            // 
            resources.ApplyResources(this.tradeMetalTextBox1, "tradeMetalTextBox1");
            this.tradeMetalTextBox1.Name = "tradeMetalTextBox1";
            this.tradeMetalTextBox1.Validated += new System.EventHandler(this.OnTradeDoubleItemTextBoxValidated);
            // 
            // tradeSwapButton
            // 
            resources.ApplyResources(this.tradeSwapButton, "tradeSwapButton");
            this.tradeSwapButton.Name = "tradeSwapButton";
            this.tradeSwapButton.UseVisualStyleBackColor = true;
            this.tradeSwapButton.Click += new System.EventHandler(this.OnTradeSwapButtonClick);
            // 
            // tradeOilTextBox2
            // 
            resources.ApplyResources(this.tradeOilTextBox2, "tradeOilTextBox2");
            this.tradeOilTextBox2.Name = "tradeOilTextBox2";
            this.tradeOilTextBox2.Validated += new System.EventHandler(this.OnTradeDoubleItemTextBoxValidated);
            // 
            // tradeMoneyTextBox2
            // 
            resources.ApplyResources(this.tradeMoneyTextBox2, "tradeMoneyTextBox2");
            this.tradeMoneyTextBox2.Name = "tradeMoneyTextBox2";
            this.tradeMoneyTextBox2.Validated += new System.EventHandler(this.OnTradeDoubleItemTextBoxValidated);
            // 
            // tradeMetalLabel
            // 
            resources.ApplyResources(this.tradeMetalLabel, "tradeMetalLabel");
            this.tradeMetalLabel.Name = "tradeMetalLabel";
            // 
            // tradeSuppliesLabel
            // 
            resources.ApplyResources(this.tradeSuppliesLabel, "tradeSuppliesLabel");
            this.tradeSuppliesLabel.Name = "tradeSuppliesLabel";
            // 
            // tradeMoneyTextBox1
            // 
            resources.ApplyResources(this.tradeMoneyTextBox1, "tradeMoneyTextBox1");
            this.tradeMoneyTextBox1.Name = "tradeMoneyTextBox1";
            this.tradeMoneyTextBox1.Validated += new System.EventHandler(this.OnTradeDoubleItemTextBoxValidated);
            // 
            // tradeEnergyTextBox2
            // 
            resources.ApplyResources(this.tradeEnergyTextBox2, "tradeEnergyTextBox2");
            this.tradeEnergyTextBox2.Name = "tradeEnergyTextBox2";
            this.tradeEnergyTextBox2.Validated += new System.EventHandler(this.OnTradeDoubleItemTextBoxValidated);
            // 
            // tradeCountryComboBox2
            // 
            this.tradeCountryComboBox2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.tradeCountryComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.tradeCountryComboBox2, "tradeCountryComboBox2");
            this.tradeCountryComboBox2.FormattingEnabled = true;
            this.tradeCountryComboBox2.Name = "tradeCountryComboBox2";
            this.tradeCountryComboBox2.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnTradeCountryItemComboBoxDrawItem);
            this.tradeCountryComboBox2.SelectedIndexChanged += new System.EventHandler(this.OnTradeCountryItemComboBoxSelectedIndexChanged);
            // 
            // tradeSuppliesTextBox1
            // 
            resources.ApplyResources(this.tradeSuppliesTextBox1, "tradeSuppliesTextBox1");
            this.tradeSuppliesTextBox1.Name = "tradeSuppliesTextBox1";
            this.tradeSuppliesTextBox1.Validated += new System.EventHandler(this.OnTradeDoubleItemTextBoxValidated);
            // 
            // tradeMoneyLabel
            // 
            resources.ApplyResources(this.tradeMoneyLabel, "tradeMoneyLabel");
            this.tradeMoneyLabel.Name = "tradeMoneyLabel";
            // 
            // tradeEnergyTextBox1
            // 
            resources.ApplyResources(this.tradeEnergyTextBox1, "tradeEnergyTextBox1");
            this.tradeEnergyTextBox1.Name = "tradeEnergyTextBox1";
            this.tradeEnergyTextBox1.Validated += new System.EventHandler(this.OnTradeDoubleItemTextBoxValidated);
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
            this.tradeSuppliesTextBox2.Validated += new System.EventHandler(this.OnTradeDoubleItemTextBoxValidated);
            // 
            // tradeInfoGroupBox
            // 
            resources.ApplyResources(this.tradeInfoGroupBox, "tradeInfoGroupBox");
            this.tradeInfoGroupBox.Controls.Add(tradeStartDateLabel);
            this.tradeInfoGroupBox.Controls.Add(this.tradeIdTextBox);
            this.tradeInfoGroupBox.Controls.Add(tradeEndDateLabel);
            this.tradeInfoGroupBox.Controls.Add(this.tradeTypeTextBox);
            this.tradeInfoGroupBox.Controls.Add(this.tradeStartYearTextBox);
            this.tradeInfoGroupBox.Controls.Add(tradeIdLabel);
            this.tradeInfoGroupBox.Controls.Add(this.tradeStartMonthTextBox);
            this.tradeInfoGroupBox.Controls.Add(this.tradeCancelCheckBox);
            this.tradeInfoGroupBox.Controls.Add(this.tradeEndDayTextBox);
            this.tradeInfoGroupBox.Controls.Add(this.tradeStartDayTextBox);
            this.tradeInfoGroupBox.Controls.Add(this.tradeEndMonthTextBox);
            this.tradeInfoGroupBox.Controls.Add(this.tradeEndYearTextBox);
            this.tradeInfoGroupBox.Name = "tradeInfoGroupBox";
            this.tradeInfoGroupBox.TabStop = false;
            // 
            // tradeIdTextBox
            // 
            resources.ApplyResources(this.tradeIdTextBox, "tradeIdTextBox");
            this.tradeIdTextBox.Name = "tradeIdTextBox";
            this.tradeIdTextBox.Validated += new System.EventHandler(this.OnTradeIntItemTextBoxValidated);
            // 
            // tradeTypeTextBox
            // 
            resources.ApplyResources(this.tradeTypeTextBox, "tradeTypeTextBox");
            this.tradeTypeTextBox.Name = "tradeTypeTextBox";
            this.tradeTypeTextBox.Validated += new System.EventHandler(this.OnTradeIntItemTextBoxValidated);
            // 
            // tradeStartYearTextBox
            // 
            resources.ApplyResources(this.tradeStartYearTextBox, "tradeStartYearTextBox");
            this.tradeStartYearTextBox.Name = "tradeStartYearTextBox";
            this.tradeStartYearTextBox.Validated += new System.EventHandler(this.OnTradeIntItemTextBoxValidated);
            // 
            // tradeStartMonthTextBox
            // 
            resources.ApplyResources(this.tradeStartMonthTextBox, "tradeStartMonthTextBox");
            this.tradeStartMonthTextBox.Name = "tradeStartMonthTextBox";
            this.tradeStartMonthTextBox.Validated += new System.EventHandler(this.OnTradeIntItemTextBoxValidated);
            // 
            // tradeCancelCheckBox
            // 
            resources.ApplyResources(this.tradeCancelCheckBox, "tradeCancelCheckBox");
            this.tradeCancelCheckBox.Name = "tradeCancelCheckBox";
            this.tradeCancelCheckBox.UseVisualStyleBackColor = true;
            this.tradeCancelCheckBox.CheckedChanged += new System.EventHandler(this.OnTradeItemCheckBoxCheckedChanged);
            // 
            // tradeEndDayTextBox
            // 
            resources.ApplyResources(this.tradeEndDayTextBox, "tradeEndDayTextBox");
            this.tradeEndDayTextBox.Name = "tradeEndDayTextBox";
            this.tradeEndDayTextBox.Validated += new System.EventHandler(this.OnTradeIntItemTextBoxValidated);
            // 
            // tradeStartDayTextBox
            // 
            resources.ApplyResources(this.tradeStartDayTextBox, "tradeStartDayTextBox");
            this.tradeStartDayTextBox.Name = "tradeStartDayTextBox";
            this.tradeStartDayTextBox.Validated += new System.EventHandler(this.OnTradeIntItemTextBoxValidated);
            // 
            // tradeEndMonthTextBox
            // 
            resources.ApplyResources(this.tradeEndMonthTextBox, "tradeEndMonthTextBox");
            this.tradeEndMonthTextBox.Name = "tradeEndMonthTextBox";
            this.tradeEndMonthTextBox.Validated += new System.EventHandler(this.OnTradeIntItemTextBoxValidated);
            // 
            // tradeEndYearTextBox
            // 
            resources.ApplyResources(this.tradeEndYearTextBox, "tradeEndYearTextBox");
            this.tradeEndYearTextBox.Name = "tradeEndYearTextBox";
            this.tradeEndYearTextBox.Validated += new System.EventHandler(this.OnTradeIntItemTextBoxValidated);
            // 
            // tradeListView
            // 
            resources.ApplyResources(this.tradeListView, "tradeListView");
            this.tradeListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.tradeCountryColumnHeader1,
            this.tradeCountryColumnHeader2,
            this.tradeDealsColumnHeader});
            this.tradeListView.FullRowSelect = true;
            this.tradeListView.GridLines = true;
            this.tradeListView.HideSelection = false;
            this.tradeListView.MultiSelect = false;
            this.tradeListView.Name = "tradeListView";
            this.tradeListView.UseCompatibleStateImageBehavior = false;
            this.tradeListView.View = System.Windows.Forms.View.Details;
            this.tradeListView.SelectedIndexChanged += new System.EventHandler(this.OnTradeListViewSelectedIndexChanged);
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
            // tradeDownButton
            // 
            resources.ApplyResources(this.tradeDownButton, "tradeDownButton");
            this.tradeDownButton.Name = "tradeDownButton";
            this.tradeDownButton.UseVisualStyleBackColor = true;
            this.tradeDownButton.Click += new System.EventHandler(this.OnTradeDownButtonClick);
            // 
            // tradeNewButton
            // 
            resources.ApplyResources(this.tradeNewButton, "tradeNewButton");
            this.tradeNewButton.Name = "tradeNewButton";
            this.tradeNewButton.UseVisualStyleBackColor = true;
            this.tradeNewButton.Click += new System.EventHandler(this.OnTradeNewButtonClick);
            // 
            // tradeRemoveButton
            // 
            resources.ApplyResources(this.tradeRemoveButton, "tradeRemoveButton");
            this.tradeRemoveButton.Name = "tradeRemoveButton";
            this.tradeRemoveButton.UseVisualStyleBackColor = true;
            this.tradeRemoveButton.Click += new System.EventHandler(this.OnTradeRemoveButtonClick);
            // 
            // tradeUpButton
            // 
            resources.ApplyResources(this.tradeUpButton, "tradeUpButton");
            this.tradeUpButton.Name = "tradeUpButton";
            this.tradeUpButton.UseVisualStyleBackColor = true;
            this.tradeUpButton.Click += new System.EventHandler(this.OnTradeUpButtonClick);
            // 
            // relationTabPage
            // 
            this.relationTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.relationTabPage.Controls.Add(this.peaceGroupBox);
            this.relationTabPage.Controls.Add(this.guaranteedGroupBox);
            this.relationTabPage.Controls.Add(this.nonAggressionGroupBox);
            this.relationTabPage.Controls.Add(this.relationGroupBox);
            this.relationTabPage.Controls.Add(this.intelligenceGroupBox);
            this.relationTabPage.Controls.Add(this.relationListView);
            this.relationTabPage.Controls.Add(this.relationCountryListBox);
            resources.ApplyResources(this.relationTabPage, "relationTabPage");
            this.relationTabPage.Name = "relationTabPage";
            // 
            // peaceGroupBox
            // 
            resources.ApplyResources(this.peaceGroupBox, "peaceGroupBox");
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
            this.peaceGroupBox.Name = "peaceGroupBox";
            this.peaceGroupBox.TabStop = false;
            // 
            // peaceIdTextBox
            // 
            resources.ApplyResources(this.peaceIdTextBox, "peaceIdTextBox");
            this.peaceIdTextBox.Name = "peaceIdTextBox";
            this.peaceIdTextBox.Validated += new System.EventHandler(this.OnRelationPeaceItemTextBoxValidated);
            // 
            // peaceTypeTextBox
            // 
            resources.ApplyResources(this.peaceTypeTextBox, "peaceTypeTextBox");
            this.peaceTypeTextBox.Name = "peaceTypeTextBox";
            this.peaceTypeTextBox.Validated += new System.EventHandler(this.OnRelationPeaceItemTextBoxValidated);
            // 
            // peaceEndDayTextBox
            // 
            resources.ApplyResources(this.peaceEndDayTextBox, "peaceEndDayTextBox");
            this.peaceEndDayTextBox.Name = "peaceEndDayTextBox";
            this.peaceEndDayTextBox.Validated += new System.EventHandler(this.OnRelationPeaceItemTextBoxValidated);
            // 
            // peaceEndLabel
            // 
            resources.ApplyResources(this.peaceEndLabel, "peaceEndLabel");
            this.peaceEndLabel.Name = "peaceEndLabel";
            // 
            // peaceEndMonthTextBox
            // 
            resources.ApplyResources(this.peaceEndMonthTextBox, "peaceEndMonthTextBox");
            this.peaceEndMonthTextBox.Name = "peaceEndMonthTextBox";
            this.peaceEndMonthTextBox.Validated += new System.EventHandler(this.OnRelationPeaceItemTextBoxValidated);
            // 
            // peaceIdLabel
            // 
            resources.ApplyResources(this.peaceIdLabel, "peaceIdLabel");
            this.peaceIdLabel.Name = "peaceIdLabel";
            // 
            // peaceEndYearTextBox
            // 
            resources.ApplyResources(this.peaceEndYearTextBox, "peaceEndYearTextBox");
            this.peaceEndYearTextBox.Name = "peaceEndYearTextBox";
            this.peaceEndYearTextBox.Validated += new System.EventHandler(this.OnRelationPeaceItemTextBoxValidated);
            // 
            // peaceStartLabel
            // 
            resources.ApplyResources(this.peaceStartLabel, "peaceStartLabel");
            this.peaceStartLabel.Name = "peaceStartLabel";
            // 
            // peaceStartYearTextBox
            // 
            resources.ApplyResources(this.peaceStartYearTextBox, "peaceStartYearTextBox");
            this.peaceStartYearTextBox.Name = "peaceStartYearTextBox";
            this.peaceStartYearTextBox.Validated += new System.EventHandler(this.OnRelationPeaceItemTextBoxValidated);
            // 
            // peaceStartDayTextBox
            // 
            resources.ApplyResources(this.peaceStartDayTextBox, "peaceStartDayTextBox");
            this.peaceStartDayTextBox.Name = "peaceStartDayTextBox";
            this.peaceStartDayTextBox.Validated += new System.EventHandler(this.OnRelationPeaceItemTextBoxValidated);
            // 
            // peaceCheckBox
            // 
            resources.ApplyResources(this.peaceCheckBox, "peaceCheckBox");
            this.peaceCheckBox.Name = "peaceCheckBox";
            this.peaceCheckBox.UseVisualStyleBackColor = true;
            this.peaceCheckBox.CheckedChanged += new System.EventHandler(this.OnRelationPeaceItemCheckBoxCheckedChanged);
            // 
            // peaceStartMonthTextBox
            // 
            resources.ApplyResources(this.peaceStartMonthTextBox, "peaceStartMonthTextBox");
            this.peaceStartMonthTextBox.Name = "peaceStartMonthTextBox";
            this.peaceStartMonthTextBox.Validated += new System.EventHandler(this.OnRelationPeaceItemTextBoxValidated);
            // 
            // guaranteedGroupBox
            // 
            resources.ApplyResources(this.guaranteedGroupBox, "guaranteedGroupBox");
            this.guaranteedGroupBox.Controls.Add(this.guaranteedCheckBox);
            this.guaranteedGroupBox.Controls.Add(this.guaranteedYearTextBox);
            this.guaranteedGroupBox.Controls.Add(this.guaranteedMonthTextBox);
            this.guaranteedGroupBox.Controls.Add(this.guaranteedEndLabel);
            this.guaranteedGroupBox.Controls.Add(this.guaranteedDayTextBox);
            this.guaranteedGroupBox.Name = "guaranteedGroupBox";
            this.guaranteedGroupBox.TabStop = false;
            // 
            // guaranteedCheckBox
            // 
            resources.ApplyResources(this.guaranteedCheckBox, "guaranteedCheckBox");
            this.guaranteedCheckBox.Name = "guaranteedCheckBox";
            this.guaranteedCheckBox.UseVisualStyleBackColor = true;
            this.guaranteedCheckBox.CheckedChanged += new System.EventHandler(this.OnRelationItemCheckBoxCheckedChanged);
            // 
            // guaranteedYearTextBox
            // 
            resources.ApplyResources(this.guaranteedYearTextBox, "guaranteedYearTextBox");
            this.guaranteedYearTextBox.Name = "guaranteedYearTextBox";
            this.guaranteedYearTextBox.Validated += new System.EventHandler(this.OnRelationIntItemTextBoxValidated);
            // 
            // guaranteedMonthTextBox
            // 
            resources.ApplyResources(this.guaranteedMonthTextBox, "guaranteedMonthTextBox");
            this.guaranteedMonthTextBox.Name = "guaranteedMonthTextBox";
            this.guaranteedMonthTextBox.Validated += new System.EventHandler(this.OnRelationIntItemTextBoxValidated);
            // 
            // guaranteedEndLabel
            // 
            resources.ApplyResources(this.guaranteedEndLabel, "guaranteedEndLabel");
            this.guaranteedEndLabel.Name = "guaranteedEndLabel";
            // 
            // guaranteedDayTextBox
            // 
            resources.ApplyResources(this.guaranteedDayTextBox, "guaranteedDayTextBox");
            this.guaranteedDayTextBox.Name = "guaranteedDayTextBox";
            this.guaranteedDayTextBox.Validated += new System.EventHandler(this.OnRelationIntItemTextBoxValidated);
            // 
            // nonAggressionGroupBox
            // 
            resources.ApplyResources(this.nonAggressionGroupBox, "nonAggressionGroupBox");
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
            this.nonAggressionGroupBox.Name = "nonAggressionGroupBox";
            this.nonAggressionGroupBox.TabStop = false;
            // 
            // nonAggressionIdTextBox
            // 
            resources.ApplyResources(this.nonAggressionIdTextBox, "nonAggressionIdTextBox");
            this.nonAggressionIdTextBox.Name = "nonAggressionIdTextBox";
            this.nonAggressionIdTextBox.Validated += new System.EventHandler(this.OnRelationNonAggressionItemTextBoxValidated);
            // 
            // nonAggressionTypeTextBox
            // 
            resources.ApplyResources(this.nonAggressionTypeTextBox, "nonAggressionTypeTextBox");
            this.nonAggressionTypeTextBox.Name = "nonAggressionTypeTextBox";
            this.nonAggressionTypeTextBox.Validated += new System.EventHandler(this.OnRelationNonAggressionItemTextBoxValidated);
            // 
            // nonAggressionIdLabel
            // 
            resources.ApplyResources(this.nonAggressionIdLabel, "nonAggressionIdLabel");
            this.nonAggressionIdLabel.Name = "nonAggressionIdLabel";
            // 
            // nonAggressionCheckBox
            // 
            resources.ApplyResources(this.nonAggressionCheckBox, "nonAggressionCheckBox");
            this.nonAggressionCheckBox.Name = "nonAggressionCheckBox";
            this.nonAggressionCheckBox.UseVisualStyleBackColor = true;
            this.nonAggressionCheckBox.CheckedChanged += new System.EventHandler(this.OnRelationNonAggressionItemCheckBoxCheckedChanged);
            // 
            // nonAggressionStartMonthTextBox
            // 
            resources.ApplyResources(this.nonAggressionStartMonthTextBox, "nonAggressionStartMonthTextBox");
            this.nonAggressionStartMonthTextBox.Name = "nonAggressionStartMonthTextBox";
            this.nonAggressionStartMonthTextBox.Validated += new System.EventHandler(this.OnRelationNonAggressionItemTextBoxValidated);
            // 
            // nonAggressionStartYearTextBox
            // 
            resources.ApplyResources(this.nonAggressionStartYearTextBox, "nonAggressionStartYearTextBox");
            this.nonAggressionStartYearTextBox.Name = "nonAggressionStartYearTextBox";
            this.nonAggressionStartYearTextBox.Validated += new System.EventHandler(this.OnRelationNonAggressionItemTextBoxValidated);
            // 
            // nonAggressionStartDayTextBox
            // 
            resources.ApplyResources(this.nonAggressionStartDayTextBox, "nonAggressionStartDayTextBox");
            this.nonAggressionStartDayTextBox.Name = "nonAggressionStartDayTextBox";
            this.nonAggressionStartDayTextBox.Validated += new System.EventHandler(this.OnRelationNonAggressionItemTextBoxValidated);
            // 
            // nonAggressionStartLabel
            // 
            resources.ApplyResources(this.nonAggressionStartLabel, "nonAggressionStartLabel");
            this.nonAggressionStartLabel.Name = "nonAggressionStartLabel";
            // 
            // nonAggressionEndYearTextBox
            // 
            resources.ApplyResources(this.nonAggressionEndYearTextBox, "nonAggressionEndYearTextBox");
            this.nonAggressionEndYearTextBox.Name = "nonAggressionEndYearTextBox";
            this.nonAggressionEndYearTextBox.Validated += new System.EventHandler(this.OnRelationNonAggressionItemTextBoxValidated);
            // 
            // nonAggressionEndMonthTextBox
            // 
            resources.ApplyResources(this.nonAggressionEndMonthTextBox, "nonAggressionEndMonthTextBox");
            this.nonAggressionEndMonthTextBox.Name = "nonAggressionEndMonthTextBox";
            this.nonAggressionEndMonthTextBox.Validated += new System.EventHandler(this.OnRelationNonAggressionItemTextBoxValidated);
            // 
            // nonAggressionEndDayTextBox
            // 
            resources.ApplyResources(this.nonAggressionEndDayTextBox, "nonAggressionEndDayTextBox");
            this.nonAggressionEndDayTextBox.Name = "nonAggressionEndDayTextBox";
            this.nonAggressionEndDayTextBox.Validated += new System.EventHandler(this.OnRelationNonAggressionItemTextBoxValidated);
            // 
            // nonAggressionEndLabel
            // 
            resources.ApplyResources(this.nonAggressionEndLabel, "nonAggressionEndLabel");
            this.nonAggressionEndLabel.Name = "nonAggressionEndLabel";
            // 
            // relationGroupBox
            // 
            resources.ApplyResources(this.relationGroupBox, "relationGroupBox");
            this.relationGroupBox.Controls.Add(this.relationValueTextBox);
            this.relationGroupBox.Controls.Add(relationValueLabel);
            this.relationGroupBox.Controls.Add(this.controlCheckBox);
            this.relationGroupBox.Controls.Add(this.masterCheckBox);
            this.relationGroupBox.Controls.Add(this.accessCheckBox);
            this.relationGroupBox.Name = "relationGroupBox";
            this.relationGroupBox.TabStop = false;
            // 
            // relationValueTextBox
            // 
            resources.ApplyResources(this.relationValueTextBox, "relationValueTextBox");
            this.relationValueTextBox.Name = "relationValueTextBox";
            this.relationValueTextBox.Validated += new System.EventHandler(this.OnRelationDoubleItemTextBoxValidated);
            // 
            // controlCheckBox
            // 
            resources.ApplyResources(this.controlCheckBox, "controlCheckBox");
            this.controlCheckBox.Name = "controlCheckBox";
            this.controlCheckBox.UseVisualStyleBackColor = true;
            this.controlCheckBox.CheckedChanged += new System.EventHandler(this.OnRelationCountryItemCheckBoxCheckedChanged);
            // 
            // masterCheckBox
            // 
            resources.ApplyResources(this.masterCheckBox, "masterCheckBox");
            this.masterCheckBox.Name = "masterCheckBox";
            this.masterCheckBox.UseVisualStyleBackColor = true;
            this.masterCheckBox.CheckedChanged += new System.EventHandler(this.OnRelationCountryItemCheckBoxCheckedChanged);
            // 
            // accessCheckBox
            // 
            resources.ApplyResources(this.accessCheckBox, "accessCheckBox");
            this.accessCheckBox.Name = "accessCheckBox";
            this.accessCheckBox.UseVisualStyleBackColor = true;
            this.accessCheckBox.CheckedChanged += new System.EventHandler(this.OnRelationItemCheckBoxCheckedChanged);
            // 
            // intelligenceGroupBox
            // 
            resources.ApplyResources(this.intelligenceGroupBox, "intelligenceGroupBox");
            this.intelligenceGroupBox.Controls.Add(this.spyNumNumericUpDown);
            this.intelligenceGroupBox.Controls.Add(spyNumLabel);
            this.intelligenceGroupBox.Name = "intelligenceGroupBox";
            this.intelligenceGroupBox.TabStop = false;
            // 
            // spyNumNumericUpDown
            // 
            resources.ApplyResources(this.spyNumNumericUpDown, "spyNumNumericUpDown");
            this.spyNumNumericUpDown.Name = "spyNumNumericUpDown";
            this.spyNumNumericUpDown.ValueChanged += new System.EventHandler(this.OnRelationIntelligenceItemNumericUpDownValueChanged);
            // 
            // relationListView
            // 
            resources.ApplyResources(this.relationListView, "relationListView");
            this.relationListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.relationCountryColumnHeader,
            this.relationValueColumnHeader,
            this.relationMasterColumnHeader,
            this.relationControlColumnHeader,
            this.relationAccessColumnHeader,
            this.relationGuaranteeColumnHeader,
            this.relationNonAggressionColumnHeader,
            this.relationPeaceColumnHeader,
            this.relationSpyColumnHeader});
            this.relationListView.FullRowSelect = true;
            this.relationListView.GridLines = true;
            this.relationListView.HideSelection = false;
            this.relationListView.MultiSelect = false;
            this.relationListView.Name = "relationListView";
            this.relationListView.UseCompatibleStateImageBehavior = false;
            this.relationListView.View = System.Windows.Forms.View.Details;
            this.relationListView.SelectedIndexChanged += new System.EventHandler(this.OnRelationListViewSelectedIndexChanged);
            // 
            // relationCountryColumnHeader
            // 
            resources.ApplyResources(this.relationCountryColumnHeader, "relationCountryColumnHeader");
            // 
            // relationValueColumnHeader
            // 
            resources.ApplyResources(this.relationValueColumnHeader, "relationValueColumnHeader");
            // 
            // relationMasterColumnHeader
            // 
            resources.ApplyResources(this.relationMasterColumnHeader, "relationMasterColumnHeader");
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
            resources.ApplyResources(this.relationCountryListBox, "relationCountryListBox");
            this.relationCountryListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.relationCountryListBox.FormattingEnabled = true;
            this.relationCountryListBox.Name = "relationCountryListBox";
            this.relationCountryListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnCountryListBoxDrawItem);
            this.relationCountryListBox.SelectedIndexChanged += new System.EventHandler(this.OnRelationCountryListBoxSelectedIndexChanged);
            // 
            // allianceTabPage
            // 
            resources.ApplyResources(this.allianceTabPage, "allianceTabPage");
            this.allianceTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.allianceTabPage.Name = "allianceTabPage";
            // 
            // mainTabPage
            // 
            this.mainTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.mainTabPage.Controls.Add(this.scenarioOptionGroupBox);
            this.mainTabPage.Controls.Add(this.countrySelectionGroupBox);
            this.mainTabPage.Controls.Add(this.scenarioInfoGroupBox);
            this.mainTabPage.Controls.Add(this.loadButton);
            this.mainTabPage.Controls.Add(this.folderGroupBox);
            this.mainTabPage.Controls.Add(this.typeGroupBox);
            this.mainTabPage.Controls.Add(this.scenarioListBox);
            resources.ApplyResources(this.mainTabPage, "mainTabPage");
            this.mainTabPage.Name = "mainTabPage";
            // 
            // scenarioOptionGroupBox
            // 
            resources.ApplyResources(this.scenarioOptionGroupBox, "scenarioOptionGroupBox");
            this.scenarioOptionGroupBox.Controls.Add(this.gameSpeedLabel);
            this.scenarioOptionGroupBox.Controls.Add(this.difficultyLabel);
            this.scenarioOptionGroupBox.Controls.Add(this.aiAggressiveLabel);
            this.scenarioOptionGroupBox.Controls.Add(this.gameSpeedComboBox);
            this.scenarioOptionGroupBox.Controls.Add(this.difficultyComboBox);
            this.scenarioOptionGroupBox.Controls.Add(this.aiAggressiveComboBox);
            this.scenarioOptionGroupBox.Controls.Add(this.allowTechnologyCheckBox);
            this.scenarioOptionGroupBox.Controls.Add(this.allowProductionCheckBox);
            this.scenarioOptionGroupBox.Controls.Add(this.allowDiplomacyCheckBox);
            this.scenarioOptionGroupBox.Controls.Add(this.freeCountryCheckBox);
            this.scenarioOptionGroupBox.Controls.Add(this.battleScenarioCheckBox);
            this.scenarioOptionGroupBox.Name = "scenarioOptionGroupBox";
            this.scenarioOptionGroupBox.TabStop = false;
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
            this.gameSpeedComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.gameSpeedComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gameSpeedComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.gameSpeedComboBox, "gameSpeedComboBox");
            this.gameSpeedComboBox.Name = "gameSpeedComboBox";
            this.gameSpeedComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnScenarioItemComboBoxDrawItem);
            this.gameSpeedComboBox.SelectedIndexChanged += new System.EventHandler(this.OnScenarioItemComboBoxSelectedIndexChanged);
            // 
            // difficultyComboBox
            // 
            this.difficultyComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.difficultyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.difficultyComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.difficultyComboBox, "difficultyComboBox");
            this.difficultyComboBox.Name = "difficultyComboBox";
            this.difficultyComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnScenarioItemComboBoxDrawItem);
            this.difficultyComboBox.SelectedIndexChanged += new System.EventHandler(this.OnScenarioItemComboBoxSelectedIndexChanged);
            // 
            // aiAggressiveComboBox
            // 
            this.aiAggressiveComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.aiAggressiveComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.aiAggressiveComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.aiAggressiveComboBox, "aiAggressiveComboBox");
            this.aiAggressiveComboBox.Name = "aiAggressiveComboBox";
            this.aiAggressiveComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnScenarioItemComboBoxDrawItem);
            this.aiAggressiveComboBox.SelectedIndexChanged += new System.EventHandler(this.OnScenarioItemComboBoxSelectedIndexChanged);
            // 
            // allowTechnologyCheckBox
            // 
            resources.ApplyResources(this.allowTechnologyCheckBox, "allowTechnologyCheckBox");
            this.allowTechnologyCheckBox.Name = "allowTechnologyCheckBox";
            this.allowTechnologyCheckBox.UseVisualStyleBackColor = true;
            this.allowTechnologyCheckBox.CheckedChanged += new System.EventHandler(this.OnScenarioItemCheckBoxCheckedChanged);
            // 
            // allowProductionCheckBox
            // 
            resources.ApplyResources(this.allowProductionCheckBox, "allowProductionCheckBox");
            this.allowProductionCheckBox.Name = "allowProductionCheckBox";
            this.allowProductionCheckBox.UseVisualStyleBackColor = true;
            this.allowProductionCheckBox.CheckedChanged += new System.EventHandler(this.OnScenarioItemCheckBoxCheckedChanged);
            // 
            // allowDiplomacyCheckBox
            // 
            resources.ApplyResources(this.allowDiplomacyCheckBox, "allowDiplomacyCheckBox");
            this.allowDiplomacyCheckBox.Name = "allowDiplomacyCheckBox";
            this.allowDiplomacyCheckBox.UseVisualStyleBackColor = true;
            this.allowDiplomacyCheckBox.CheckedChanged += new System.EventHandler(this.OnScenarioItemCheckBoxCheckedChanged);
            // 
            // freeCountryCheckBox
            // 
            resources.ApplyResources(this.freeCountryCheckBox, "freeCountryCheckBox");
            this.freeCountryCheckBox.Name = "freeCountryCheckBox";
            this.freeCountryCheckBox.UseVisualStyleBackColor = true;
            this.freeCountryCheckBox.CheckedChanged += new System.EventHandler(this.OnScenarioItemCheckBoxCheckedChanged);
            // 
            // battleScenarioCheckBox
            // 
            resources.ApplyResources(this.battleScenarioCheckBox, "battleScenarioCheckBox");
            this.battleScenarioCheckBox.Name = "battleScenarioCheckBox";
            this.battleScenarioCheckBox.UseVisualStyleBackColor = true;
            this.battleScenarioCheckBox.CheckedChanged += new System.EventHandler(this.OnScenarioItemCheckBoxCheckedChanged);
            // 
            // countrySelectionGroupBox
            // 
            resources.ApplyResources(this.countrySelectionGroupBox, "countrySelectionGroupBox");
            this.countrySelectionGroupBox.Controls.Add(this.countryDescKeyTextBox);
            this.countrySelectionGroupBox.Controls.Add(this.majorCountryNameStringTextBox);
            this.countrySelectionGroupBox.Controls.Add(this.majorFlagExtTextBox);
            this.countrySelectionGroupBox.Controls.Add(this.majorFlagExtLabel);
            this.countrySelectionGroupBox.Controls.Add(this.majorCountryNameKeyTextBox);
            this.countrySelectionGroupBox.Controls.Add(this.majorCountryNameLabel);
            this.countrySelectionGroupBox.Controls.Add(this.selectableRemoveButton);
            this.countrySelectionGroupBox.Controls.Add(this.selectableAddButton);
            this.countrySelectionGroupBox.Controls.Add(this.unselectableListBox);
            this.countrySelectionGroupBox.Controls.Add(this.selectableListBox);
            this.countrySelectionGroupBox.Controls.Add(this.propagandaPictureBox);
            this.countrySelectionGroupBox.Controls.Add(this.propagandaBrowseButton);
            this.countrySelectionGroupBox.Controls.Add(this.propagandaTextBox);
            this.countrySelectionGroupBox.Controls.Add(this.propagandaLabel);
            this.countrySelectionGroupBox.Controls.Add(this.countryDescStringTextBox);
            this.countrySelectionGroupBox.Controls.Add(this.majorAddButton);
            this.countrySelectionGroupBox.Controls.Add(this.majorRemoveButton);
            this.countrySelectionGroupBox.Controls.Add(this.countryDescLabel);
            this.countrySelectionGroupBox.Controls.Add(this.majorListBox);
            this.countrySelectionGroupBox.Controls.Add(this.majorLabel);
            this.countrySelectionGroupBox.Controls.Add(this.selectableLabel);
            this.countrySelectionGroupBox.Controls.Add(this.majorUpButton);
            this.countrySelectionGroupBox.Controls.Add(this.majorDownButton);
            this.countrySelectionGroupBox.Name = "countrySelectionGroupBox";
            this.countrySelectionGroupBox.TabStop = false;
            // 
            // countryDescKeyTextBox
            // 
            resources.ApplyResources(this.countryDescKeyTextBox, "countryDescKeyTextBox");
            this.countryDescKeyTextBox.Name = "countryDescKeyTextBox";
            this.countryDescKeyTextBox.TextChanged += new System.EventHandler(this.OnSelectableStringItemTextBoxTextChanged);
            // 
            // majorCountryNameStringTextBox
            // 
            resources.ApplyResources(this.majorCountryNameStringTextBox, "majorCountryNameStringTextBox");
            this.majorCountryNameStringTextBox.Name = "majorCountryNameStringTextBox";
            this.majorCountryNameStringTextBox.TextChanged += new System.EventHandler(this.OnSelectableStringItemTextBoxTextChanged);
            // 
            // majorFlagExtTextBox
            // 
            resources.ApplyResources(this.majorFlagExtTextBox, "majorFlagExtTextBox");
            this.majorFlagExtTextBox.Name = "majorFlagExtTextBox";
            this.majorFlagExtTextBox.TextChanged += new System.EventHandler(this.OnSelectableStringItemTextBoxTextChanged);
            // 
            // majorFlagExtLabel
            // 
            resources.ApplyResources(this.majorFlagExtLabel, "majorFlagExtLabel");
            this.majorFlagExtLabel.Name = "majorFlagExtLabel";
            // 
            // majorCountryNameKeyTextBox
            // 
            resources.ApplyResources(this.majorCountryNameKeyTextBox, "majorCountryNameKeyTextBox");
            this.majorCountryNameKeyTextBox.Name = "majorCountryNameKeyTextBox";
            this.majorCountryNameKeyTextBox.TextChanged += new System.EventHandler(this.OnSelectableStringItemTextBoxTextChanged);
            // 
            // majorCountryNameLabel
            // 
            resources.ApplyResources(this.majorCountryNameLabel, "majorCountryNameLabel");
            this.majorCountryNameLabel.Name = "majorCountryNameLabel";
            // 
            // selectableRemoveButton
            // 
            resources.ApplyResources(this.selectableRemoveButton, "selectableRemoveButton");
            this.selectableRemoveButton.Name = "selectableRemoveButton";
            this.selectableRemoveButton.UseVisualStyleBackColor = true;
            this.selectableRemoveButton.Click += new System.EventHandler(this.OnSelectableRemoveButtonClick);
            // 
            // selectableAddButton
            // 
            resources.ApplyResources(this.selectableAddButton, "selectableAddButton");
            this.selectableAddButton.Name = "selectableAddButton";
            this.selectableAddButton.UseVisualStyleBackColor = true;
            this.selectableAddButton.Click += new System.EventHandler(this.OnSelectableAddButtonClick);
            // 
            // unselectableListBox
            // 
            this.unselectableListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            resources.ApplyResources(this.unselectableListBox, "unselectableListBox");
            this.unselectableListBox.FormattingEnabled = true;
            this.unselectableListBox.Name = "unselectableListBox";
            this.unselectableListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.unselectableListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnUnselectableListBoxDrawItem);
            this.unselectableListBox.SelectedIndexChanged += new System.EventHandler(this.OnUnselectableListBoxSelectedIndexChanged);
            // 
            // selectableListBox
            // 
            this.selectableListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            resources.ApplyResources(this.selectableListBox, "selectableListBox");
            this.selectableListBox.FormattingEnabled = true;
            this.selectableListBox.Name = "selectableListBox";
            this.selectableListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.selectableListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnSelectableListBoxDrawItem);
            this.selectableListBox.SelectedIndexChanged += new System.EventHandler(this.OnSelectableListBoxSelectedIndexChanged);
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
            this.propagandaBrowseButton.Click += new System.EventHandler(this.OnPropagandaBrowseButtonClick);
            // 
            // propagandaTextBox
            // 
            resources.ApplyResources(this.propagandaTextBox, "propagandaTextBox");
            this.propagandaTextBox.Name = "propagandaTextBox";
            this.propagandaTextBox.TextChanged += new System.EventHandler(this.OnSelectableStringItemTextBoxTextChanged);
            // 
            // propagandaLabel
            // 
            resources.ApplyResources(this.propagandaLabel, "propagandaLabel");
            this.propagandaLabel.Name = "propagandaLabel";
            // 
            // countryDescStringTextBox
            // 
            resources.ApplyResources(this.countryDescStringTextBox, "countryDescStringTextBox");
            this.countryDescStringTextBox.Name = "countryDescStringTextBox";
            this.countryDescStringTextBox.TextChanged += new System.EventHandler(this.OnSelectableStringItemTextBoxTextChanged);
            // 
            // majorAddButton
            // 
            resources.ApplyResources(this.majorAddButton, "majorAddButton");
            this.majorAddButton.Name = "majorAddButton";
            this.majorAddButton.UseVisualStyleBackColor = true;
            this.majorAddButton.Click += new System.EventHandler(this.OnMajorAddButtonClick);
            // 
            // majorRemoveButton
            // 
            resources.ApplyResources(this.majorRemoveButton, "majorRemoveButton");
            this.majorRemoveButton.Name = "majorRemoveButton";
            this.majorRemoveButton.UseVisualStyleBackColor = true;
            this.majorRemoveButton.Click += new System.EventHandler(this.OnMajorRemoveButtonClick);
            // 
            // countryDescLabel
            // 
            resources.ApplyResources(this.countryDescLabel, "countryDescLabel");
            this.countryDescLabel.Name = "countryDescLabel";
            // 
            // majorListBox
            // 
            this.majorListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            resources.ApplyResources(this.majorListBox, "majorListBox");
            this.majorListBox.FormattingEnabled = true;
            this.majorListBox.Name = "majorListBox";
            this.majorListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnMajorListBoxDrawItem);
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
            this.majorUpButton.Click += new System.EventHandler(this.OnMajorUpButtonClick);
            // 
            // majorDownButton
            // 
            resources.ApplyResources(this.majorDownButton, "majorDownButton");
            this.majorDownButton.Name = "majorDownButton";
            this.majorDownButton.UseVisualStyleBackColor = true;
            this.majorDownButton.Click += new System.EventHandler(this.OnMajorDownButtonClick);
            // 
            // scenarioInfoGroupBox
            // 
            this.scenarioInfoGroupBox.Controls.Add(this.includeFolderBrowseButton);
            this.scenarioInfoGroupBox.Controls.Add(this.includeFolderTextBox);
            this.scenarioInfoGroupBox.Controls.Add(this.includeFolderLabel);
            this.scenarioInfoGroupBox.Controls.Add(this.scenarioNameLabel);
            this.scenarioInfoGroupBox.Controls.Add(this.scenarioNameTextBox);
            this.scenarioInfoGroupBox.Controls.Add(this.panelPictureBox);
            this.scenarioInfoGroupBox.Controls.Add(this.panelImageLabel);
            this.scenarioInfoGroupBox.Controls.Add(this.panelImageTextBox);
            this.scenarioInfoGroupBox.Controls.Add(this.panelImageBrowseButton);
            this.scenarioInfoGroupBox.Controls.Add(this.startDateLabel);
            this.scenarioInfoGroupBox.Controls.Add(this.startYearTextBox);
            this.scenarioInfoGroupBox.Controls.Add(this.startMonthTextBox);
            this.scenarioInfoGroupBox.Controls.Add(this.startDayTextBox);
            this.scenarioInfoGroupBox.Controls.Add(this.endDateLabel);
            this.scenarioInfoGroupBox.Controls.Add(this.endYearTextBox);
            this.scenarioInfoGroupBox.Controls.Add(this.endMonthTextBox);
            this.scenarioInfoGroupBox.Controls.Add(this.endDayTextBox);
            resources.ApplyResources(this.scenarioInfoGroupBox, "scenarioInfoGroupBox");
            this.scenarioInfoGroupBox.Name = "scenarioInfoGroupBox";
            this.scenarioInfoGroupBox.TabStop = false;
            // 
            // includeFolderBrowseButton
            // 
            resources.ApplyResources(this.includeFolderBrowseButton, "includeFolderBrowseButton");
            this.includeFolderBrowseButton.Name = "includeFolderBrowseButton";
            this.includeFolderBrowseButton.UseVisualStyleBackColor = true;
            this.includeFolderBrowseButton.Click += new System.EventHandler(this.OnIncludeFolderBrowseButtonClick);
            // 
            // includeFolderTextBox
            // 
            resources.ApplyResources(this.includeFolderTextBox, "includeFolderTextBox");
            this.includeFolderTextBox.Name = "includeFolderTextBox";
            this.includeFolderTextBox.TextChanged += new System.EventHandler(this.OnScenarioStringItemTextBoxTextChanged);
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
            this.scenarioNameTextBox.TextChanged += new System.EventHandler(this.OnScenarioStringItemTextBoxTextChanged);
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
            this.panelImageTextBox.TextChanged += new System.EventHandler(this.OnScenarioStringItemTextBoxTextChanged);
            // 
            // panelImageBrowseButton
            // 
            resources.ApplyResources(this.panelImageBrowseButton, "panelImageBrowseButton");
            this.panelImageBrowseButton.Name = "panelImageBrowseButton";
            this.panelImageBrowseButton.UseVisualStyleBackColor = true;
            this.panelImageBrowseButton.Click += new System.EventHandler(this.OnPanelImageBrowseButtonClick);
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
            this.startYearTextBox.Validated += new System.EventHandler(this.OnScenarioIntItemTextBoxValidated);
            // 
            // startMonthTextBox
            // 
            resources.ApplyResources(this.startMonthTextBox, "startMonthTextBox");
            this.startMonthTextBox.Name = "startMonthTextBox";
            this.startMonthTextBox.Validated += new System.EventHandler(this.OnScenarioIntItemTextBoxValidated);
            // 
            // startDayTextBox
            // 
            resources.ApplyResources(this.startDayTextBox, "startDayTextBox");
            this.startDayTextBox.Name = "startDayTextBox";
            this.startDayTextBox.Validated += new System.EventHandler(this.OnScenarioIntItemTextBoxValidated);
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
            this.endYearTextBox.Validated += new System.EventHandler(this.OnScenarioIntItemTextBoxValidated);
            // 
            // endMonthTextBox
            // 
            resources.ApplyResources(this.endMonthTextBox, "endMonthTextBox");
            this.endMonthTextBox.Name = "endMonthTextBox";
            this.endMonthTextBox.Validated += new System.EventHandler(this.OnScenarioIntItemTextBoxValidated);
            // 
            // endDayTextBox
            // 
            resources.ApplyResources(this.endDayTextBox, "endDayTextBox");
            this.endDayTextBox.Name = "endDayTextBox";
            this.endDayTextBox.Validated += new System.EventHandler(this.OnScenarioIntItemTextBoxValidated);
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
            resources.ApplyResources(this.folderGroupBox, "folderGroupBox");
            this.folderGroupBox.Controls.Add(this.exportRadioButton);
            this.folderGroupBox.Controls.Add(this.modRadioButton);
            this.folderGroupBox.Controls.Add(this.vanillaRadioButton);
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
            resources.ApplyResources(this.typeGroupBox, "typeGroupBox");
            this.typeGroupBox.Controls.Add(this.saveGamesRadioButton);
            this.typeGroupBox.Controls.Add(this.scenarioRadioButton);
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
            resources.ApplyResources(this.scenarioListBox, "scenarioListBox");
            this.scenarioListBox.FormattingEnabled = true;
            this.scenarioListBox.Name = "scenarioListBox";
            // 
            // scenarioTabControl
            // 
            resources.ApplyResources(this.scenarioTabControl, "scenarioTabControl");
            this.scenarioTabControl.Controls.Add(this.mainTabPage);
            this.scenarioTabControl.Controls.Add(this.allianceTabPage);
            this.scenarioTabControl.Controls.Add(this.relationTabPage);
            this.scenarioTabControl.Controls.Add(this.tradeTabPage);
            this.scenarioTabControl.Controls.Add(this.countryTabPage);
            this.scenarioTabControl.Controls.Add(this.governmentTabPage);
            this.scenarioTabControl.Controls.Add(technologyTabPage);
            this.scenarioTabControl.Controls.Add(this.provinceTabPage);
            this.scenarioTabControl.Controls.Add(this.oobTabPage);
            this.scenarioTabControl.Name = "scenarioTabControl";
            this.scenarioTabControl.SelectedIndex = 0;
            this.scenarioTabControl.SelectedIndexChanged += new System.EventHandler(this.OnScenarioTabControlSelectedIndexChanged);
            // 
            // oobTabPage
            // 
            this.oobTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.oobTabPage.Controls.Add(this.oobBottomButton);
            this.oobTabPage.Controls.Add(this.oobDownButton);
            this.oobTabPage.Controls.Add(this.oobUpButton);
            this.oobTabPage.Controls.Add(this.oobTopButton);
            this.oobTabPage.Controls.Add(this.oobRemoveButton);
            this.oobTabPage.Controls.Add(this.oobCloneButton);
            this.oobTabPage.Controls.Add(this.oobAddDivisionButton);
            this.oobTabPage.Controls.Add(this.oobAddUnitButton);
            this.oobTabPage.Controls.Add(this.divisionGroupBox);
            this.oobTabPage.Controls.Add(this.unitGroupBox);
            this.oobTabPage.Controls.Add(this.unitTreeView);
            this.oobTabPage.Controls.Add(this.oobCountryListBox);
            resources.ApplyResources(this.oobTabPage, "oobTabPage");
            this.oobTabPage.Name = "oobTabPage";
            // 
            // oobBottomButton
            // 
            resources.ApplyResources(this.oobBottomButton, "oobBottomButton");
            this.oobBottomButton.Name = "oobBottomButton";
            this.oobBottomButton.UseVisualStyleBackColor = true;
            this.oobBottomButton.Click += new System.EventHandler(this.OnOobBottomButtonClick);
            // 
            // oobDownButton
            // 
            resources.ApplyResources(this.oobDownButton, "oobDownButton");
            this.oobDownButton.Name = "oobDownButton";
            this.oobDownButton.UseVisualStyleBackColor = true;
            this.oobDownButton.Click += new System.EventHandler(this.OnOobDownButtonClick);
            // 
            // oobUpButton
            // 
            resources.ApplyResources(this.oobUpButton, "oobUpButton");
            this.oobUpButton.Name = "oobUpButton";
            this.oobUpButton.UseVisualStyleBackColor = true;
            this.oobUpButton.Click += new System.EventHandler(this.OnOobUpButtonClick);
            // 
            // oobTopButton
            // 
            resources.ApplyResources(this.oobTopButton, "oobTopButton");
            this.oobTopButton.Name = "oobTopButton";
            this.oobTopButton.UseVisualStyleBackColor = true;
            this.oobTopButton.Click += new System.EventHandler(this.OnOobTopButtonClick);
            // 
            // oobRemoveButton
            // 
            resources.ApplyResources(this.oobRemoveButton, "oobRemoveButton");
            this.oobRemoveButton.Name = "oobRemoveButton";
            this.oobRemoveButton.UseVisualStyleBackColor = true;
            this.oobRemoveButton.Click += new System.EventHandler(this.OnOobRemoveButtonClick);
            // 
            // oobCloneButton
            // 
            resources.ApplyResources(this.oobCloneButton, "oobCloneButton");
            this.oobCloneButton.Name = "oobCloneButton";
            this.oobCloneButton.UseVisualStyleBackColor = true;
            this.oobCloneButton.Click += new System.EventHandler(this.OnOobCloneButtonClick);
            // 
            // oobAddDivisionButton
            // 
            resources.ApplyResources(this.oobAddDivisionButton, "oobAddDivisionButton");
            this.oobAddDivisionButton.Name = "oobAddDivisionButton";
            this.oobAddDivisionButton.UseVisualStyleBackColor = true;
            this.oobAddDivisionButton.Click += new System.EventHandler(this.OnOobAddDivisionButtonClick);
            // 
            // oobAddUnitButton
            // 
            resources.ApplyResources(this.oobAddUnitButton, "oobAddUnitButton");
            this.oobAddUnitButton.Name = "oobAddUnitButton";
            this.oobAddUnitButton.UseVisualStyleBackColor = true;
            this.oobAddUnitButton.Click += new System.EventHandler(this.OnOobAddUnitButtonClick);
            // 
            // divisionGroupBox
            // 
            this.divisionGroupBox.Controls.Add(this.dormantCheckBox);
            this.divisionGroupBox.Controls.Add(this.lockedCheckBox);
            this.divisionGroupBox.Controls.Add(this.experienceTextBox);
            this.divisionGroupBox.Controls.Add(this.experienceLabel);
            this.divisionGroupBox.Controls.Add(this.divisionMoraleTextBox);
            this.divisionGroupBox.Controls.Add(this.divisionMoraleLabel);
            this.divisionGroupBox.Controls.Add(this.maxOrganisationTextBox);
            this.divisionGroupBox.Controls.Add(this.organisationTextBox);
            this.divisionGroupBox.Controls.Add(this.organisationLabel);
            this.divisionGroupBox.Controls.Add(this.maxStrengthTextBox);
            this.divisionGroupBox.Controls.Add(this.strengthTextBox);
            this.divisionGroupBox.Controls.Add(this.strengthLabel);
            this.divisionGroupBox.Controls.Add(this.brigadeModelComboBox5);
            this.divisionGroupBox.Controls.Add(this.brigadeTypeComboBox5);
            this.divisionGroupBox.Controls.Add(this.brigadeModelComboBox4);
            this.divisionGroupBox.Controls.Add(this.brigadeTypeComboBox4);
            this.divisionGroupBox.Controls.Add(this.brigadeModelComboBox3);
            this.divisionGroupBox.Controls.Add(this.brigadeTypeComboBox3);
            this.divisionGroupBox.Controls.Add(this.brigadeModelComboBox2);
            this.divisionGroupBox.Controls.Add(this.brigadeTypeComboBox2);
            this.divisionGroupBox.Controls.Add(this.brigadeModelComboBox1);
            this.divisionGroupBox.Controls.Add(this.brigadesLabel);
            this.divisionGroupBox.Controls.Add(this.brigadeTypeComboBox1);
            this.divisionGroupBox.Controls.Add(this.unitModelComboBox);
            this.divisionGroupBox.Controls.Add(this.unitTypeLabel);
            this.divisionGroupBox.Controls.Add(this.unitTypeComboBox);
            this.divisionGroupBox.Controls.Add(this.divisionNameTextBox);
            this.divisionGroupBox.Controls.Add(this.divisionNameLabel);
            this.divisionGroupBox.Controls.Add(this.divisionIdTextBox);
            this.divisionGroupBox.Controls.Add(this.divisionTypeTextBox);
            this.divisionGroupBox.Controls.Add(this.divisionIdLabel);
            resources.ApplyResources(this.divisionGroupBox, "divisionGroupBox");
            this.divisionGroupBox.Name = "divisionGroupBox";
            this.divisionGroupBox.TabStop = false;
            // 
            // dormantCheckBox
            // 
            resources.ApplyResources(this.dormantCheckBox, "dormantCheckBox");
            this.dormantCheckBox.Name = "dormantCheckBox";
            this.dormantCheckBox.UseVisualStyleBackColor = true;
            this.dormantCheckBox.CheckedChanged += new System.EventHandler(this.OnDivisionCheckBoxCheckedChanged);
            // 
            // lockedCheckBox
            // 
            resources.ApplyResources(this.lockedCheckBox, "lockedCheckBox");
            this.lockedCheckBox.Name = "lockedCheckBox";
            this.lockedCheckBox.UseVisualStyleBackColor = true;
            this.lockedCheckBox.CheckedChanged += new System.EventHandler(this.OnDivisionCheckBoxCheckedChanged);
            // 
            // experienceTextBox
            // 
            resources.ApplyResources(this.experienceTextBox, "experienceTextBox");
            this.experienceTextBox.Name = "experienceTextBox";
            this.experienceTextBox.Validated += new System.EventHandler(this.OnDivisionDoubleItemTextBoxValidated);
            // 
            // experienceLabel
            // 
            resources.ApplyResources(this.experienceLabel, "experienceLabel");
            this.experienceLabel.Name = "experienceLabel";
            // 
            // divisionMoraleTextBox
            // 
            resources.ApplyResources(this.divisionMoraleTextBox, "divisionMoraleTextBox");
            this.divisionMoraleTextBox.Name = "divisionMoraleTextBox";
            this.divisionMoraleTextBox.Validated += new System.EventHandler(this.OnDivisionDoubleItemTextBoxValidated);
            // 
            // divisionMoraleLabel
            // 
            resources.ApplyResources(this.divisionMoraleLabel, "divisionMoraleLabel");
            this.divisionMoraleLabel.Name = "divisionMoraleLabel";
            // 
            // maxOrganisationTextBox
            // 
            resources.ApplyResources(this.maxOrganisationTextBox, "maxOrganisationTextBox");
            this.maxOrganisationTextBox.Name = "maxOrganisationTextBox";
            this.maxOrganisationTextBox.Validated += new System.EventHandler(this.OnDivisionDoubleItemTextBoxValidated);
            // 
            // organisationTextBox
            // 
            resources.ApplyResources(this.organisationTextBox, "organisationTextBox");
            this.organisationTextBox.Name = "organisationTextBox";
            this.organisationTextBox.Validated += new System.EventHandler(this.OnDivisionDoubleItemTextBoxValidated);
            // 
            // organisationLabel
            // 
            resources.ApplyResources(this.organisationLabel, "organisationLabel");
            this.organisationLabel.Name = "organisationLabel";
            // 
            // maxStrengthTextBox
            // 
            resources.ApplyResources(this.maxStrengthTextBox, "maxStrengthTextBox");
            this.maxStrengthTextBox.Name = "maxStrengthTextBox";
            this.maxStrengthTextBox.Validated += new System.EventHandler(this.OnDivisionDoubleItemTextBoxValidated);
            // 
            // strengthTextBox
            // 
            resources.ApplyResources(this.strengthTextBox, "strengthTextBox");
            this.strengthTextBox.Name = "strengthTextBox";
            this.strengthTextBox.Validated += new System.EventHandler(this.OnDivisionDoubleItemTextBoxValidated);
            // 
            // strengthLabel
            // 
            resources.ApplyResources(this.strengthLabel, "strengthLabel");
            this.strengthLabel.Name = "strengthLabel";
            // 
            // brigadeModelComboBox5
            // 
            this.brigadeModelComboBox5.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.brigadeModelComboBox5.FormattingEnabled = true;
            resources.ApplyResources(this.brigadeModelComboBox5, "brigadeModelComboBox5");
            this.brigadeModelComboBox5.Name = "brigadeModelComboBox5";
            this.brigadeModelComboBox5.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnDivisionComboBoxDrawItem);
            this.brigadeModelComboBox5.SelectedIndexChanged += new System.EventHandler(this.OnDivisionComboBoxSelectedIndexChanged);
            this.brigadeModelComboBox5.Validated += new System.EventHandler(this.OnDivisionComboBoxValidated);
            // 
            // brigadeTypeComboBox5
            // 
            this.brigadeTypeComboBox5.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.brigadeTypeComboBox5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.brigadeTypeComboBox5.FormattingEnabled = true;
            resources.ApplyResources(this.brigadeTypeComboBox5, "brigadeTypeComboBox5");
            this.brigadeTypeComboBox5.Name = "brigadeTypeComboBox5";
            this.brigadeTypeComboBox5.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnDivisionComboBoxDrawItem);
            this.brigadeTypeComboBox5.SelectedIndexChanged += new System.EventHandler(this.OnDivisionComboBoxSelectedIndexChanged);
            // 
            // brigadeModelComboBox4
            // 
            this.brigadeModelComboBox4.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.brigadeModelComboBox4.FormattingEnabled = true;
            resources.ApplyResources(this.brigadeModelComboBox4, "brigadeModelComboBox4");
            this.brigadeModelComboBox4.Name = "brigadeModelComboBox4";
            this.brigadeModelComboBox4.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnDivisionComboBoxDrawItem);
            this.brigadeModelComboBox4.SelectedIndexChanged += new System.EventHandler(this.OnDivisionComboBoxSelectedIndexChanged);
            this.brigadeModelComboBox4.Validated += new System.EventHandler(this.OnDivisionComboBoxValidated);
            // 
            // brigadeTypeComboBox4
            // 
            this.brigadeTypeComboBox4.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.brigadeTypeComboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.brigadeTypeComboBox4.FormattingEnabled = true;
            resources.ApplyResources(this.brigadeTypeComboBox4, "brigadeTypeComboBox4");
            this.brigadeTypeComboBox4.Name = "brigadeTypeComboBox4";
            this.brigadeTypeComboBox4.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnDivisionComboBoxDrawItem);
            this.brigadeTypeComboBox4.SelectedIndexChanged += new System.EventHandler(this.OnDivisionComboBoxSelectedIndexChanged);
            // 
            // brigadeModelComboBox3
            // 
            this.brigadeModelComboBox3.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.brigadeModelComboBox3.FormattingEnabled = true;
            resources.ApplyResources(this.brigadeModelComboBox3, "brigadeModelComboBox3");
            this.brigadeModelComboBox3.Name = "brigadeModelComboBox3";
            this.brigadeModelComboBox3.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnDivisionComboBoxDrawItem);
            this.brigadeModelComboBox3.SelectedIndexChanged += new System.EventHandler(this.OnDivisionComboBoxSelectedIndexChanged);
            this.brigadeModelComboBox3.Validated += new System.EventHandler(this.OnDivisionComboBoxValidated);
            // 
            // brigadeTypeComboBox3
            // 
            this.brigadeTypeComboBox3.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.brigadeTypeComboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.brigadeTypeComboBox3.FormattingEnabled = true;
            resources.ApplyResources(this.brigadeTypeComboBox3, "brigadeTypeComboBox3");
            this.brigadeTypeComboBox3.Name = "brigadeTypeComboBox3";
            this.brigadeTypeComboBox3.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnDivisionComboBoxDrawItem);
            this.brigadeTypeComboBox3.SelectedIndexChanged += new System.EventHandler(this.OnDivisionComboBoxSelectedIndexChanged);
            // 
            // brigadeModelComboBox2
            // 
            this.brigadeModelComboBox2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.brigadeModelComboBox2.FormattingEnabled = true;
            resources.ApplyResources(this.brigadeModelComboBox2, "brigadeModelComboBox2");
            this.brigadeModelComboBox2.Name = "brigadeModelComboBox2";
            this.brigadeModelComboBox2.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnDivisionComboBoxDrawItem);
            this.brigadeModelComboBox2.SelectedIndexChanged += new System.EventHandler(this.OnDivisionComboBoxSelectedIndexChanged);
            this.brigadeModelComboBox2.Validated += new System.EventHandler(this.OnDivisionComboBoxValidated);
            // 
            // brigadeTypeComboBox2
            // 
            this.brigadeTypeComboBox2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.brigadeTypeComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.brigadeTypeComboBox2.FormattingEnabled = true;
            resources.ApplyResources(this.brigadeTypeComboBox2, "brigadeTypeComboBox2");
            this.brigadeTypeComboBox2.Name = "brigadeTypeComboBox2";
            this.brigadeTypeComboBox2.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnDivisionComboBoxDrawItem);
            this.brigadeTypeComboBox2.SelectedIndexChanged += new System.EventHandler(this.OnDivisionComboBoxSelectedIndexChanged);
            // 
            // brigadeModelComboBox1
            // 
            this.brigadeModelComboBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.brigadeModelComboBox1.FormattingEnabled = true;
            resources.ApplyResources(this.brigadeModelComboBox1, "brigadeModelComboBox1");
            this.brigadeModelComboBox1.Name = "brigadeModelComboBox1";
            this.brigadeModelComboBox1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnDivisionComboBoxDrawItem);
            this.brigadeModelComboBox1.SelectedIndexChanged += new System.EventHandler(this.OnDivisionComboBoxSelectedIndexChanged);
            this.brigadeModelComboBox1.Validated += new System.EventHandler(this.OnDivisionComboBoxValidated);
            // 
            // brigadesLabel
            // 
            resources.ApplyResources(this.brigadesLabel, "brigadesLabel");
            this.brigadesLabel.Name = "brigadesLabel";
            // 
            // brigadeTypeComboBox1
            // 
            this.brigadeTypeComboBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.brigadeTypeComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.brigadeTypeComboBox1.FormattingEnabled = true;
            resources.ApplyResources(this.brigadeTypeComboBox1, "brigadeTypeComboBox1");
            this.brigadeTypeComboBox1.Name = "brigadeTypeComboBox1";
            this.brigadeTypeComboBox1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnDivisionComboBoxDrawItem);
            this.brigadeTypeComboBox1.SelectedIndexChanged += new System.EventHandler(this.OnDivisionComboBoxSelectedIndexChanged);
            // 
            // unitModelComboBox
            // 
            this.unitModelComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.unitModelComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.unitModelComboBox, "unitModelComboBox");
            this.unitModelComboBox.Name = "unitModelComboBox";
            this.unitModelComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnDivisionComboBoxDrawItem);
            this.unitModelComboBox.SelectedIndexChanged += new System.EventHandler(this.OnDivisionComboBoxSelectedIndexChanged);
            this.unitModelComboBox.Validated += new System.EventHandler(this.OnDivisionComboBoxValidated);
            // 
            // unitTypeLabel
            // 
            resources.ApplyResources(this.unitTypeLabel, "unitTypeLabel");
            this.unitTypeLabel.Name = "unitTypeLabel";
            // 
            // unitTypeComboBox
            // 
            this.unitTypeComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.unitTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.unitTypeComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.unitTypeComboBox, "unitTypeComboBox");
            this.unitTypeComboBox.Name = "unitTypeComboBox";
            this.unitTypeComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnDivisionComboBoxDrawItem);
            this.unitTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.OnDivisionComboBoxSelectedIndexChanged);
            // 
            // divisionNameTextBox
            // 
            resources.ApplyResources(this.divisionNameTextBox, "divisionNameTextBox");
            this.divisionNameTextBox.Name = "divisionNameTextBox";
            this.divisionNameTextBox.TextChanged += new System.EventHandler(this.OnDivisionStringItemTextBoxTextChanged);
            // 
            // divisionNameLabel
            // 
            resources.ApplyResources(this.divisionNameLabel, "divisionNameLabel");
            this.divisionNameLabel.Name = "divisionNameLabel";
            // 
            // divisionIdTextBox
            // 
            resources.ApplyResources(this.divisionIdTextBox, "divisionIdTextBox");
            this.divisionIdTextBox.Name = "divisionIdTextBox";
            this.divisionIdTextBox.Validated += new System.EventHandler(this.OnDivisionIntItemTextBoxValidated);
            // 
            // divisionTypeTextBox
            // 
            resources.ApplyResources(this.divisionTypeTextBox, "divisionTypeTextBox");
            this.divisionTypeTextBox.Name = "divisionTypeTextBox";
            this.divisionTypeTextBox.Validated += new System.EventHandler(this.OnDivisionIntItemTextBoxValidated);
            // 
            // divisionIdLabel
            // 
            resources.ApplyResources(this.divisionIdLabel, "divisionIdLabel");
            this.divisionIdLabel.Name = "divisionIdLabel";
            // 
            // unitGroupBox
            // 
            this.unitGroupBox.Controls.Add(this.leaderComboBox);
            this.unitGroupBox.Controls.Add(this.baseComboBox);
            this.unitGroupBox.Controls.Add(this.locationComboBox);
            this.unitGroupBox.Controls.Add(this.digInTextBox);
            this.unitGroupBox.Controls.Add(this.digInLabel);
            this.unitGroupBox.Controls.Add(this.leaderTextBox);
            this.unitGroupBox.Controls.Add(this.leaderLabel);
            this.unitGroupBox.Controls.Add(this.unitMoraleTextBox);
            this.unitGroupBox.Controls.Add(this.unitMoraleLabel);
            this.unitGroupBox.Controls.Add(this.baseTextBox);
            this.unitGroupBox.Controls.Add(this.baseLabel);
            this.unitGroupBox.Controls.Add(this.locationTextBox);
            this.unitGroupBox.Controls.Add(this.locationLabel);
            this.unitGroupBox.Controls.Add(this.unitNameTextBox);
            this.unitGroupBox.Controls.Add(this.unitNameLabel);
            this.unitGroupBox.Controls.Add(this.unitIdTextBox);
            this.unitGroupBox.Controls.Add(this.unitTypeTextBox);
            this.unitGroupBox.Controls.Add(this.unitIdLabel);
            resources.ApplyResources(this.unitGroupBox, "unitGroupBox");
            this.unitGroupBox.Name = "unitGroupBox";
            this.unitGroupBox.TabStop = false;
            // 
            // leaderComboBox
            // 
            this.leaderComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.leaderComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.leaderComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.leaderComboBox, "leaderComboBox");
            this.leaderComboBox.Name = "leaderComboBox";
            this.leaderComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnUnitComboBoxDrawItem);
            this.leaderComboBox.SelectedIndexChanged += new System.EventHandler(this.OnUnitComboBoxSelectedIndexChanged);
            // 
            // baseComboBox
            // 
            this.baseComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.baseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.baseComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.baseComboBox, "baseComboBox");
            this.baseComboBox.Name = "baseComboBox";
            this.baseComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnUnitComboBoxDrawItem);
            this.baseComboBox.SelectedIndexChanged += new System.EventHandler(this.OnUnitComboBoxSelectedIndexChanged);
            // 
            // locationComboBox
            // 
            this.locationComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.locationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.locationComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.locationComboBox, "locationComboBox");
            this.locationComboBox.Name = "locationComboBox";
            this.locationComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnUnitComboBoxDrawItem);
            this.locationComboBox.SelectedIndexChanged += new System.EventHandler(this.OnUnitComboBoxSelectedIndexChanged);
            // 
            // digInTextBox
            // 
            resources.ApplyResources(this.digInTextBox, "digInTextBox");
            this.digInTextBox.Name = "digInTextBox";
            this.digInTextBox.Validated += new System.EventHandler(this.OnUnitDoubleItemTextBoxValidated);
            // 
            // digInLabel
            // 
            resources.ApplyResources(this.digInLabel, "digInLabel");
            this.digInLabel.Name = "digInLabel";
            // 
            // leaderTextBox
            // 
            resources.ApplyResources(this.leaderTextBox, "leaderTextBox");
            this.leaderTextBox.Name = "leaderTextBox";
            this.leaderTextBox.Validated += new System.EventHandler(this.OnUnitIntItemTextBoxValidated);
            // 
            // leaderLabel
            // 
            resources.ApplyResources(this.leaderLabel, "leaderLabel");
            this.leaderLabel.Name = "leaderLabel";
            // 
            // unitMoraleTextBox
            // 
            resources.ApplyResources(this.unitMoraleTextBox, "unitMoraleTextBox");
            this.unitMoraleTextBox.Name = "unitMoraleTextBox";
            this.unitMoraleTextBox.Validated += new System.EventHandler(this.OnUnitDoubleItemTextBoxValidated);
            // 
            // unitMoraleLabel
            // 
            resources.ApplyResources(this.unitMoraleLabel, "unitMoraleLabel");
            this.unitMoraleLabel.Name = "unitMoraleLabel";
            // 
            // baseTextBox
            // 
            resources.ApplyResources(this.baseTextBox, "baseTextBox");
            this.baseTextBox.Name = "baseTextBox";
            this.baseTextBox.Validated += new System.EventHandler(this.OnUnitIntItemTextBoxValidated);
            // 
            // baseLabel
            // 
            resources.ApplyResources(this.baseLabel, "baseLabel");
            this.baseLabel.Name = "baseLabel";
            // 
            // locationTextBox
            // 
            resources.ApplyResources(this.locationTextBox, "locationTextBox");
            this.locationTextBox.Name = "locationTextBox";
            this.locationTextBox.Validated += new System.EventHandler(this.OnUnitIntItemTextBoxValidated);
            // 
            // locationLabel
            // 
            resources.ApplyResources(this.locationLabel, "locationLabel");
            this.locationLabel.Name = "locationLabel";
            // 
            // unitNameTextBox
            // 
            resources.ApplyResources(this.unitNameTextBox, "unitNameTextBox");
            this.unitNameTextBox.Name = "unitNameTextBox";
            this.unitNameTextBox.TextChanged += new System.EventHandler(this.OnUnitStringItemTextBoxTextChanged);
            // 
            // unitNameLabel
            // 
            resources.ApplyResources(this.unitNameLabel, "unitNameLabel");
            this.unitNameLabel.Name = "unitNameLabel";
            // 
            // unitIdTextBox
            // 
            resources.ApplyResources(this.unitIdTextBox, "unitIdTextBox");
            this.unitIdTextBox.Name = "unitIdTextBox";
            this.unitIdTextBox.Validated += new System.EventHandler(this.OnUnitIntItemTextBoxValidated);
            // 
            // unitTypeTextBox
            // 
            resources.ApplyResources(this.unitTypeTextBox, "unitTypeTextBox");
            this.unitTypeTextBox.Name = "unitTypeTextBox";
            this.unitTypeTextBox.Validated += new System.EventHandler(this.OnUnitIntItemTextBoxValidated);
            // 
            // unitIdLabel
            // 
            resources.ApplyResources(this.unitIdLabel, "unitIdLabel");
            this.unitIdLabel.Name = "unitIdLabel";
            // 
            // unitTreeView
            // 
            resources.ApplyResources(this.unitTreeView, "unitTreeView");
            this.unitTreeView.FullRowSelect = true;
            this.unitTreeView.HideSelection = false;
            this.unitTreeView.Name = "unitTreeView";
            // 
            // oobCountryListBox
            // 
            resources.ApplyResources(this.oobCountryListBox, "oobCountryListBox");
            this.oobCountryListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.oobCountryListBox.FormattingEnabled = true;
            this.oobCountryListBox.Name = "oobCountryListBox";
            this.oobCountryListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnCountryListBoxDrawItem);
            this.oobCountryListBox.SelectedIndexChanged += new System.EventHandler(this.OnOobCountryListBoxSelectedIndexChanged);
            // 
            // checkButton
            // 
            resources.ApplyResources(this.checkButton, "checkButton");
            this.checkButton.Name = "checkButton";
            this.checkButton.UseVisualStyleBackColor = true;
            this.checkButton.Click += new System.EventHandler(this.OnCheckButtonClick);
            // 
            // ScenarioEditorForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.checkButton);
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
            technologyTabPage.ResumeLayout(false);
            technologyTabPage.PerformLayout();
            this.techTreePanel.ResumeLayout(false);
            this.techTreePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.techTreePictureBox)).EndInit();
            this.provinceTabPage.ResumeLayout(false);
            this.provinceTabPage.PerformLayout();
            this.provinceCountryGroupBox.ResumeLayout(false);
            this.provinceCountryGroupBox.PerformLayout();
            this.mapFilterGroupBox.ResumeLayout(false);
            this.mapFilterGroupBox.PerformLayout();
            this.provinceInfoGroupBox.ResumeLayout(false);
            this.provinceInfoGroupBox.PerformLayout();
            this.provinceResourceGroupBox.ResumeLayout(false);
            this.provinceResourceGroupBox.PerformLayout();
            this.provinceBuildingGroupBox.ResumeLayout(false);
            this.provinceBuildingGroupBox.PerformLayout();
            this.provinceMapPanel.ResumeLayout(false);
            this.provinceMapPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.provinceMapPictureBox)).EndInit();
            this.tradeTabPage.ResumeLayout(false);
            this.tradeDealsGroupBox.ResumeLayout(false);
            this.tradeDealsGroupBox.PerformLayout();
            this.tradeInfoGroupBox.ResumeLayout(false);
            this.tradeInfoGroupBox.PerformLayout();
            this.relationTabPage.ResumeLayout(false);
            this.peaceGroupBox.ResumeLayout(false);
            this.peaceGroupBox.PerformLayout();
            this.guaranteedGroupBox.ResumeLayout(false);
            this.guaranteedGroupBox.PerformLayout();
            this.nonAggressionGroupBox.ResumeLayout(false);
            this.nonAggressionGroupBox.PerformLayout();
            this.relationGroupBox.ResumeLayout(false);
            this.relationGroupBox.PerformLayout();
            this.intelligenceGroupBox.ResumeLayout(false);
            this.intelligenceGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spyNumNumericUpDown)).EndInit();
            this.mainTabPage.ResumeLayout(false);
            this.scenarioOptionGroupBox.ResumeLayout(false);
            this.scenarioOptionGroupBox.PerformLayout();
            this.countrySelectionGroupBox.ResumeLayout(false);
            this.countrySelectionGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propagandaPictureBox)).EndInit();
            this.scenarioInfoGroupBox.ResumeLayout(false);
            this.scenarioInfoGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelPictureBox)).EndInit();
            this.folderGroupBox.ResumeLayout(false);
            this.folderGroupBox.PerformLayout();
            this.typeGroupBox.ResumeLayout(false);
            this.typeGroupBox.PerformLayout();
            this.scenarioTabControl.ResumeLayout(false);
            this.oobTabPage.ResumeLayout(false);
            this.divisionGroupBox.ResumeLayout(false);
            this.divisionGroupBox.PerformLayout();
            this.unitGroupBox.ResumeLayout(false);
            this.unitGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button reloadButton;
        private System.Windows.Forms.TabPage provinceTabPage;
        private System.Windows.Forms.Panel provinceMapPanel;
        private System.Windows.Forms.PictureBox provinceMapPictureBox;
        private System.Windows.Forms.TabPage governmentTabPage;
        private System.Windows.Forms.TabPage countryTabPage;
        private System.Windows.Forms.TabPage tradeTabPage;
        private System.Windows.Forms.GroupBox tradeDealsGroupBox;
        private System.Windows.Forms.ComboBox tradeCountryComboBox1;
        private System.Windows.Forms.TextBox tradeRareMaterialsTextBox1;
        private System.Windows.Forms.TextBox tradeRareMaterialsTextBox2;
        private System.Windows.Forms.Label tradeRareMaterialsLabel;
        private System.Windows.Forms.Label tradeOilLabel;
        private System.Windows.Forms.TextBox tradeMetalTextBox2;
        private System.Windows.Forms.TextBox tradeOilTextBox1;
        private System.Windows.Forms.TextBox tradeMetalTextBox1;
        private System.Windows.Forms.Button tradeSwapButton;
        private System.Windows.Forms.TextBox tradeOilTextBox2;
        private System.Windows.Forms.TextBox tradeMoneyTextBox2;
        private System.Windows.Forms.Label tradeMetalLabel;
        private System.Windows.Forms.Label tradeSuppliesLabel;
        private System.Windows.Forms.TextBox tradeMoneyTextBox1;
        private System.Windows.Forms.TextBox tradeEnergyTextBox2;
        private System.Windows.Forms.TextBox tradeSuppliesTextBox1;
        private System.Windows.Forms.Label tradeMoneyLabel;
        private System.Windows.Forms.TextBox tradeEnergyTextBox1;
        private System.Windows.Forms.Label tradeEnergyLabel;
        private System.Windows.Forms.TextBox tradeSuppliesTextBox2;
        private System.Windows.Forms.GroupBox tradeInfoGroupBox;
        private System.Windows.Forms.TextBox tradeIdTextBox;
        private System.Windows.Forms.TextBox tradeTypeTextBox;
        private System.Windows.Forms.TextBox tradeStartYearTextBox;
        private System.Windows.Forms.TextBox tradeStartMonthTextBox;
        private System.Windows.Forms.CheckBox tradeCancelCheckBox;
        private System.Windows.Forms.TextBox tradeEndDayTextBox;
        private System.Windows.Forms.TextBox tradeStartDayTextBox;
        private System.Windows.Forms.TextBox tradeEndMonthTextBox;
        private System.Windows.Forms.TextBox tradeEndYearTextBox;
        private System.Windows.Forms.ListView tradeListView;
        private System.Windows.Forms.ColumnHeader tradeCountryColumnHeader1;
        private System.Windows.Forms.ColumnHeader tradeCountryColumnHeader2;
        private System.Windows.Forms.ColumnHeader tradeDealsColumnHeader;
        private System.Windows.Forms.Button tradeDownButton;
        private System.Windows.Forms.Button tradeNewButton;
        private System.Windows.Forms.Button tradeRemoveButton;
        private System.Windows.Forms.Button tradeUpButton;
        private System.Windows.Forms.TabPage relationTabPage;
        private System.Windows.Forms.GroupBox intelligenceGroupBox;
        private System.Windows.Forms.NumericUpDown spyNumNumericUpDown;
        private System.Windows.Forms.GroupBox guaranteedGroupBox;
        private System.Windows.Forms.CheckBox guaranteedCheckBox;
        private System.Windows.Forms.TextBox guaranteedYearTextBox;
        private System.Windows.Forms.TextBox guaranteedMonthTextBox;
        private System.Windows.Forms.Label guaranteedEndLabel;
        private System.Windows.Forms.TextBox guaranteedDayTextBox;
        private System.Windows.Forms.GroupBox peaceGroupBox;
        private System.Windows.Forms.TextBox peaceIdTextBox;
        private System.Windows.Forms.TextBox peaceTypeTextBox;
        private System.Windows.Forms.TextBox peaceEndDayTextBox;
        private System.Windows.Forms.Label peaceEndLabel;
        private System.Windows.Forms.TextBox peaceEndMonthTextBox;
        private System.Windows.Forms.Label peaceIdLabel;
        private System.Windows.Forms.TextBox peaceEndYearTextBox;
        private System.Windows.Forms.Label peaceStartLabel;
        private System.Windows.Forms.TextBox peaceStartYearTextBox;
        private System.Windows.Forms.TextBox peaceStartDayTextBox;
        private System.Windows.Forms.CheckBox peaceCheckBox;
        private System.Windows.Forms.TextBox peaceStartMonthTextBox;
        private System.Windows.Forms.GroupBox nonAggressionGroupBox;
        private System.Windows.Forms.TextBox nonAggressionIdTextBox;
        private System.Windows.Forms.TextBox nonAggressionTypeTextBox;
        private System.Windows.Forms.Label nonAggressionIdLabel;
        private System.Windows.Forms.CheckBox nonAggressionCheckBox;
        private System.Windows.Forms.TextBox nonAggressionStartMonthTextBox;
        private System.Windows.Forms.TextBox nonAggressionStartYearTextBox;
        private System.Windows.Forms.TextBox nonAggressionStartDayTextBox;
        private System.Windows.Forms.Label nonAggressionStartLabel;
        private System.Windows.Forms.TextBox nonAggressionEndYearTextBox;
        private System.Windows.Forms.TextBox nonAggressionEndMonthTextBox;
        private System.Windows.Forms.TextBox nonAggressionEndDayTextBox;
        private System.Windows.Forms.Label nonAggressionEndLabel;
        private System.Windows.Forms.CheckBox accessCheckBox;
        private System.Windows.Forms.CheckBox masterCheckBox;
        private System.Windows.Forms.CheckBox controlCheckBox;
        private System.Windows.Forms.ListView relationListView;
        private System.Windows.Forms.ColumnHeader relationCountryColumnHeader;
        private System.Windows.Forms.ColumnHeader relationValueColumnHeader;
        private System.Windows.Forms.ColumnHeader relationMasterColumnHeader;
        private System.Windows.Forms.ColumnHeader relationControlColumnHeader;
        private System.Windows.Forms.ColumnHeader relationAccessColumnHeader;
        private System.Windows.Forms.ColumnHeader relationGuaranteeColumnHeader;
        private System.Windows.Forms.ColumnHeader relationNonAggressionColumnHeader;
        private System.Windows.Forms.ColumnHeader relationPeaceColumnHeader;
        private System.Windows.Forms.ColumnHeader relationSpyColumnHeader;
        private System.Windows.Forms.ListBox relationCountryListBox;
        private System.Windows.Forms.TabPage allianceTabPage;
        private System.Windows.Forms.TabPage mainTabPage;
        private System.Windows.Forms.GroupBox scenarioOptionGroupBox;
        private System.Windows.Forms.Label gameSpeedLabel;
        private System.Windows.Forms.Label difficultyLabel;
        private System.Windows.Forms.Label aiAggressiveLabel;
        private System.Windows.Forms.ComboBox gameSpeedComboBox;
        private System.Windows.Forms.ComboBox difficultyComboBox;
        private System.Windows.Forms.ComboBox aiAggressiveComboBox;
        private System.Windows.Forms.CheckBox allowTechnologyCheckBox;
        private System.Windows.Forms.CheckBox allowProductionCheckBox;
        private System.Windows.Forms.CheckBox allowDiplomacyCheckBox;
        private System.Windows.Forms.CheckBox freeCountryCheckBox;
        private System.Windows.Forms.CheckBox battleScenarioCheckBox;
        private System.Windows.Forms.GroupBox countrySelectionGroupBox;
        private System.Windows.Forms.PictureBox propagandaPictureBox;
        private System.Windows.Forms.Button propagandaBrowseButton;
        private System.Windows.Forms.TextBox propagandaTextBox;
        private System.Windows.Forms.Label propagandaLabel;
        private System.Windows.Forms.TextBox countryDescStringTextBox;
        private System.Windows.Forms.Label countryDescLabel;
        private System.Windows.Forms.Button majorAddButton;
        private System.Windows.Forms.Button majorRemoveButton;
        private System.Windows.Forms.ListBox majorListBox;
        private System.Windows.Forms.Label majorLabel;
        private System.Windows.Forms.Label selectableLabel;
        private System.Windows.Forms.Button majorUpButton;
        private System.Windows.Forms.Button majorDownButton;
        private System.Windows.Forms.GroupBox scenarioInfoGroupBox;
        private System.Windows.Forms.Button includeFolderBrowseButton;
        private System.Windows.Forms.TextBox includeFolderTextBox;
        private System.Windows.Forms.Label includeFolderLabel;
        private System.Windows.Forms.Label scenarioNameLabel;
        private System.Windows.Forms.TextBox scenarioNameTextBox;
        private System.Windows.Forms.PictureBox panelPictureBox;
        private System.Windows.Forms.Label panelImageLabel;
        private System.Windows.Forms.TextBox panelImageTextBox;
        private System.Windows.Forms.Button panelImageBrowseButton;
        private System.Windows.Forms.Label startDateLabel;
        private System.Windows.Forms.TextBox startYearTextBox;
        private System.Windows.Forms.TextBox startMonthTextBox;
        private System.Windows.Forms.TextBox startDayTextBox;
        private System.Windows.Forms.Label endDateLabel;
        private System.Windows.Forms.TextBox endYearTextBox;
        private System.Windows.Forms.TextBox endMonthTextBox;
        private System.Windows.Forms.TextBox endDayTextBox;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.GroupBox folderGroupBox;
        private System.Windows.Forms.RadioButton exportRadioButton;
        private System.Windows.Forms.RadioButton modRadioButton;
        private System.Windows.Forms.RadioButton vanillaRadioButton;
        private System.Windows.Forms.GroupBox typeGroupBox;
        private System.Windows.Forms.RadioButton saveGamesRadioButton;
        private System.Windows.Forms.RadioButton scenarioRadioButton;
        private System.Windows.Forms.ListBox scenarioListBox;
        private System.Windows.Forms.TabControl scenarioTabControl;
        private System.Windows.Forms.Button selectableRemoveButton;
        private System.Windows.Forms.Button selectableAddButton;
        private System.Windows.Forms.ListBox unselectableListBox;
        private System.Windows.Forms.ListBox selectableListBox;
        private System.Windows.Forms.ListBox techCategoryListBox;
        private System.Windows.Forms.ListBox techCountryListBox;
        private System.Windows.Forms.ListView ownedTechsListView;
        private System.Windows.Forms.ListView inventionsListView;
        private System.Windows.Forms.ListView blueprintsListView;
        private System.Windows.Forms.GroupBox mapFilterGroupBox;
        private System.Windows.Forms.RadioButton mapFilterClaimedRadioButton;
        private System.Windows.Forms.RadioButton mapFilterControlledRadioButton;
        private System.Windows.Forms.RadioButton mapFilterOwnedRadioButton;
        private System.Windows.Forms.RadioButton mapFilterCoreRadioButton;
        private System.Windows.Forms.RadioButton mapFilterNoneRadioButton;
        private System.Windows.Forms.CheckBox claimedProvinceCheckBox;
        private System.Windows.Forms.CheckBox controlledProvinceCheckBox;
        private System.Windows.Forms.CheckBox ownedProvinceCheckBox;
        private System.Windows.Forms.CheckBox coreProvinceCheckBox;
        private System.Windows.Forms.ListView provinceListView;
        private System.Windows.Forms.TextBox provinceIdTextBox;
        private System.Windows.Forms.TextBox provinceNameKeyTextBox;
        private System.Windows.Forms.Label provinceMetalLabel;
        private System.Windows.Forms.Label provinceRareMaterialsLabel;
        private System.Windows.Forms.Label provinceOilLabel;
        private System.Windows.Forms.Label provinceEnergyLabel;
        private System.Windows.Forms.Label provinceSuppliesLabel;
        private System.Windows.Forms.Label provinceManpowerLabel;
        private System.Windows.Forms.GroupBox provinceResourceGroupBox;
        private System.Windows.Forms.TextBox nuclearPowerRelativeTextBox;
        private System.Windows.Forms.TextBox nuclearPowerMaxTextBox;
        private System.Windows.Forms.TextBox nuclearPowerCurrentTextBox;
        private System.Windows.Forms.TextBox syntheticRaresRelativeTextBox;
        private System.Windows.Forms.TextBox syntheticRaresMaxTextBox;
        private System.Windows.Forms.TextBox syntheticRaresCurrentTextBox;
        private System.Windows.Forms.TextBox syntheticOilRelativeTextBox;
        private System.Windows.Forms.TextBox syntheticOilMaxTextBox;
        private System.Windows.Forms.TextBox syntheticOilCurrentTextBox;
        private System.Windows.Forms.TextBox rocketTestRelativeTextBox;
        private System.Windows.Forms.TextBox rocketTestMaxTextBox;
        private System.Windows.Forms.TextBox rocketTestCurrentTextBox;
        private System.Windows.Forms.TextBox infrastructureRelativeTextBox;
        private System.Windows.Forms.TextBox infrastructureMaxTextBox;
        private System.Windows.Forms.TextBox infrastructureCurrentTextBox;
        private System.Windows.Forms.TextBox icRelativeTextBox;
        private System.Windows.Forms.TextBox icMaxTextBox;
        private System.Windows.Forms.TextBox icCurrentTextBox;
        private System.Windows.Forms.TextBox nuclearReactorRelativeTextBox;
        private System.Windows.Forms.TextBox nuclearReactorMaxTextBox;
        private System.Windows.Forms.TextBox nuclearReactorCurrentTextBox;
        private System.Windows.Forms.TextBox radarStationRelativeTextBox;
        private System.Windows.Forms.TextBox radarStationMaxTextBox;
        private System.Windows.Forms.TextBox radarStationCurrentTextBox;
        private System.Windows.Forms.TextBox navalBaseRelativeTextBox;
        private System.Windows.Forms.TextBox navalBaseMaxTextBox;
        private System.Windows.Forms.TextBox navalBaseCurrentTextBox;
        private System.Windows.Forms.TextBox airBaseRelativeTextBox;
        private System.Windows.Forms.TextBox airBaseMaxTextBox;
        private System.Windows.Forms.TextBox airBaseCurrentTextBox;
        private System.Windows.Forms.TextBox antiAirRelativeTextBox;
        private System.Windows.Forms.TextBox antiAirMaxTextBox;
        private System.Windows.Forms.TextBox antiAirCurrentTextBox;
        private System.Windows.Forms.TextBox coastalFortRelativeTextBox;
        private System.Windows.Forms.TextBox coastalFortMaxTextBox;
        private System.Windows.Forms.TextBox coastalFortCurrentTextBox;
        private System.Windows.Forms.TextBox landFortRelativeTextBox;
        private System.Windows.Forms.TextBox landFortMaxTextBox;
        private System.Windows.Forms.TextBox landFortCurrentTextBox;
        private System.Windows.Forms.TextBox rareMaterialsMaxTextBox;
        private System.Windows.Forms.TextBox rareMaterialsCurrentTextBox;
        private System.Windows.Forms.TextBox rareMaterialsPoolTextBox;
        private System.Windows.Forms.TextBox metalMaxTextBox;
        private System.Windows.Forms.TextBox metalCurrentTextBox;
        private System.Windows.Forms.TextBox metalPoolTextBox;
        private System.Windows.Forms.TextBox energyMaxTextBox;
        private System.Windows.Forms.TextBox energyCurrentTextBox;
        private System.Windows.Forms.TextBox energyPoolTextBox;
        private System.Windows.Forms.GroupBox provinceInfoGroupBox;
        private System.Windows.Forms.TextBox revoltRiskTextBox;
        private System.Windows.Forms.TextBox vpTextBox;
        private System.Windows.Forms.TextBox manpowerCurrentTextBox;
        private System.Windows.Forms.TextBox suppliesPoolTextBox;
        private System.Windows.Forms.TextBox oilMaxTextBox;
        private System.Windows.Forms.TextBox manpowerMaxTextBox;
        private System.Windows.Forms.TextBox oilCurrentTextBox;
        private System.Windows.Forms.TextBox oilPoolTextBox;
        private System.Windows.Forms.ComboBox provinceCountryFilterComboBox;
        private System.Windows.Forms.CheckBox capitalCheckBox;
        private System.Windows.Forms.GroupBox provinceCountryGroupBox;
        private System.Windows.Forms.ColumnHeader provinceIdColumnHeader;
        private System.Windows.Forms.ColumnHeader provinceNameColumnHeader;
        private System.Windows.Forms.ColumnHeader capitalProvinceColumnHeader;
        private System.Windows.Forms.ColumnHeader coreProvinceColumnHeader;
        private System.Windows.Forms.ColumnHeader ownedColumnHeader;
        private System.Windows.Forms.ColumnHeader controlledProvinceColumnHeader;
        private System.Windows.Forms.ColumnHeader claimedProvinceColumnHeader;
        private System.Windows.Forms.GroupBox provinceBuildingGroupBox;
        private System.Windows.Forms.Label provinceCountryFilterLabel;
        private System.Windows.Forms.Label provinceIdLabel;
        private System.Windows.Forms.Panel techTreePanel;
        private System.Windows.Forms.PictureBox techTreePictureBox;
        private System.Windows.Forms.Label ownedTechsLabel;
        private System.Windows.Forms.Label blueprintsLabel;
        private System.Windows.Forms.Label inventionsLabel;
        private System.Windows.Forms.ComboBox tradeCountryComboBox2;
        private System.Windows.Forms.GroupBox relationGroupBox;
        private System.Windows.Forms.TextBox relationValueTextBox;
        private System.Windows.Forms.TextBox majorFlagExtTextBox;
        private System.Windows.Forms.Label majorFlagExtLabel;
        private System.Windows.Forms.TextBox majorCountryNameKeyTextBox;
        private System.Windows.Forms.Label majorCountryNameLabel;
        private System.Windows.Forms.TextBox majorCountryNameStringTextBox;
        private System.Windows.Forms.TextBox countryDescKeyTextBox;
        private System.Windows.Forms.TextBox provinceNameStringTextBox;
        private System.Windows.Forms.Label provinceSyntheticOilLabel;
        private System.Windows.Forms.Label provinceSyntheticRaresLabel;
        private System.Windows.Forms.Label provinceNuclearPowerLabel;
        private System.Windows.Forms.Button checkButton;
        private System.Windows.Forms.TabPage oobTabPage;
        private System.Windows.Forms.GroupBox divisionGroupBox;
        private System.Windows.Forms.TextBox experienceTextBox;
        private System.Windows.Forms.Label experienceLabel;
        private System.Windows.Forms.TextBox divisionMoraleTextBox;
        private System.Windows.Forms.Label divisionMoraleLabel;
        private System.Windows.Forms.TextBox maxOrganisationTextBox;
        private System.Windows.Forms.TextBox organisationTextBox;
        private System.Windows.Forms.Label organisationLabel;
        private System.Windows.Forms.TextBox maxStrengthTextBox;
        private System.Windows.Forms.TextBox strengthTextBox;
        private System.Windows.Forms.Label strengthLabel;
        private System.Windows.Forms.ComboBox brigadeModelComboBox5;
        private System.Windows.Forms.ComboBox brigadeTypeComboBox5;
        private System.Windows.Forms.ComboBox brigadeModelComboBox4;
        private System.Windows.Forms.ComboBox brigadeTypeComboBox4;
        private System.Windows.Forms.ComboBox brigadeModelComboBox3;
        private System.Windows.Forms.ComboBox brigadeTypeComboBox3;
        private System.Windows.Forms.ComboBox brigadeModelComboBox2;
        private System.Windows.Forms.ComboBox brigadeTypeComboBox2;
        private System.Windows.Forms.ComboBox brigadeModelComboBox1;
        private System.Windows.Forms.Label brigadesLabel;
        private System.Windows.Forms.ComboBox brigadeTypeComboBox1;
        private System.Windows.Forms.ComboBox unitModelComboBox;
        private System.Windows.Forms.Label unitTypeLabel;
        private System.Windows.Forms.ComboBox unitTypeComboBox;
        private System.Windows.Forms.TextBox divisionNameTextBox;
        private System.Windows.Forms.Label divisionNameLabel;
        private System.Windows.Forms.TextBox divisionIdTextBox;
        private System.Windows.Forms.TextBox divisionTypeTextBox;
        private System.Windows.Forms.Label divisionIdLabel;
        private System.Windows.Forms.GroupBox unitGroupBox;
        private System.Windows.Forms.ComboBox leaderComboBox;
        private System.Windows.Forms.ComboBox baseComboBox;
        private System.Windows.Forms.ComboBox locationComboBox;
        private System.Windows.Forms.TextBox digInTextBox;
        private System.Windows.Forms.Label digInLabel;
        private System.Windows.Forms.TextBox leaderTextBox;
        private System.Windows.Forms.Label leaderLabel;
        private System.Windows.Forms.TextBox unitMoraleTextBox;
        private System.Windows.Forms.Label unitMoraleLabel;
        private System.Windows.Forms.TextBox baseTextBox;
        private System.Windows.Forms.Label baseLabel;
        private System.Windows.Forms.TextBox locationTextBox;
        private System.Windows.Forms.Label locationLabel;
        private System.Windows.Forms.TextBox unitNameTextBox;
        private System.Windows.Forms.Label unitNameLabel;
        private System.Windows.Forms.TextBox unitIdTextBox;
        private System.Windows.Forms.TextBox unitTypeTextBox;
        private System.Windows.Forms.Label unitIdLabel;
        private System.Windows.Forms.TreeView unitTreeView;
        private System.Windows.Forms.ListBox oobCountryListBox;
        private System.Windows.Forms.CheckBox dormantCheckBox;
        private System.Windows.Forms.CheckBox lockedCheckBox;
        private System.Windows.Forms.Button oobBottomButton;
        private System.Windows.Forms.Button oobDownButton;
        private System.Windows.Forms.Button oobUpButton;
        private System.Windows.Forms.Button oobTopButton;
        private System.Windows.Forms.Button oobRemoveButton;
        private System.Windows.Forms.Button oobCloneButton;
        private System.Windows.Forms.Button oobAddDivisionButton;
        private System.Windows.Forms.Button oobAddUnitButton;

    }
}