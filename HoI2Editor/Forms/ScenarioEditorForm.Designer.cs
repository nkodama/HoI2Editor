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
            System.Windows.Forms.Label provinceSyntheticOilLabel;
            System.Windows.Forms.Label provinceSyntheticRaresLabel;
            System.Windows.Forms.Label provinceNuclearPowerLabel;
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
            System.Windows.Forms.Label peacetimeIcModifierLabel;
            System.Windows.Forms.Label wartimeIcModifierLabel;
            System.Windows.Forms.Label groundDefEffLabel;
            System.Windows.Forms.Label relativeManpowerLabel;
            System.Windows.Forms.Label industrialModifierLabel;
            System.Windows.Forms.Label aiFileLabel;
            System.Windows.Forms.Label countryOffmapLabel;
            System.Windows.Forms.Label countryStockpileLabel;
            System.Windows.Forms.Label nukeDateLabel;
            System.Windows.Forms.Label dissentLabel;
            System.Windows.Forms.Label flagExtLabel;
            System.Windows.Forms.Label nukeLabel;
            System.Windows.Forms.Label extraTcLabel;
            System.Windows.Forms.Label belligerenceLabel;
            System.Windows.Forms.Label regularIdLabel;
            System.Windows.Forms.Label countryNameLabel;
            System.Windows.Forms.Label tradeStartDateLabel;
            System.Windows.Forms.Label tradeEndDateLabel;
            System.Windows.Forms.Label tradeIdLabel;
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
            this.revoltRiskTextBox = new System.Windows.Forms.TextBox();
            this.vpTextBox = new System.Windows.Forms.TextBox();
            this.provinceNameTextBox = new System.Windows.Forms.TextBox();
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
            this.cabinetGroupBox = new System.Windows.Forms.GroupBox();
            this.chiefOfAirIdTextBox = new System.Windows.Forms.TextBox();
            this.chiefOfAirTypeTextBox = new System.Windows.Forms.TextBox();
            this.chiefOfAirComboBox = new System.Windows.Forms.ComboBox();
            this.chiefOfAirLabel = new System.Windows.Forms.Label();
            this.chiefOfNavyIdTextBox = new System.Windows.Forms.TextBox();
            this.chiefOfNavyTypeTextBox = new System.Windows.Forms.TextBox();
            this.chiefOfNavyComboBox = new System.Windows.Forms.ComboBox();
            this.chiefOfNavyLabel = new System.Windows.Forms.Label();
            this.chiefOfArmyIdTextBox = new System.Windows.Forms.TextBox();
            this.chiefOfArmyTypeTextBox = new System.Windows.Forms.TextBox();
            this.chiefOfArmyComboBox = new System.Windows.Forms.ComboBox();
            this.chiefOfArmyLabel = new System.Windows.Forms.Label();
            this.chiefOfStaffIdTextBox = new System.Windows.Forms.TextBox();
            this.chiefOfStaffTypeTextBox = new System.Windows.Forms.TextBox();
            this.chiefOfStaffComboBox = new System.Windows.Forms.ComboBox();
            this.chiefOfStaffLabel = new System.Windows.Forms.Label();
            this.ministerOfIntelligenceIdTextBox = new System.Windows.Forms.TextBox();
            this.ministerOfIntelligenceTypeTextBox = new System.Windows.Forms.TextBox();
            this.ministerOfIntelligenceComboBox = new System.Windows.Forms.ComboBox();
            this.ministerOfIntelligenceLabel = new System.Windows.Forms.Label();
            this.ministerOfSecurityIdTextBox = new System.Windows.Forms.TextBox();
            this.ministerOfSecurityTypeTextBox = new System.Windows.Forms.TextBox();
            this.ministerOfSecurityComboBox = new System.Windows.Forms.ComboBox();
            this.ministerOfSecurityLabel = new System.Windows.Forms.Label();
            this.armamentMinisterIdTextBox = new System.Windows.Forms.TextBox();
            this.armamentMinisterTypeTextBox = new System.Windows.Forms.TextBox();
            this.armamentMinisterComboBox = new System.Windows.Forms.ComboBox();
            this.armamentMinisterLabel = new System.Windows.Forms.Label();
            this.foreignMinisterIdTextBox = new System.Windows.Forms.TextBox();
            this.foreignMinisterTypeTextBox = new System.Windows.Forms.TextBox();
            this.foreignMinisterComboBox = new System.Windows.Forms.ComboBox();
            this.foreignMinisterlabel = new System.Windows.Forms.Label();
            this.headOfGovernmentIdTextBox = new System.Windows.Forms.TextBox();
            this.headOfGovernmentTypeTextBox = new System.Windows.Forms.TextBox();
            this.headOfGovernmentComboBox = new System.Windows.Forms.ComboBox();
            this.headOfGovernmentLabel = new System.Windows.Forms.Label();
            this.headOfStateIdTextBox = new System.Windows.Forms.TextBox();
            this.headOfStateTypeTextBox = new System.Windows.Forms.TextBox();
            this.headOfStateComboBox = new System.Windows.Forms.ComboBox();
            this.headOfStateLabel = new System.Windows.Forms.Label();
            this.governmentCountryListBox = new System.Windows.Forms.ListBox();
            this.politicalSliderGroupBox = new System.Windows.Forms.GroupBox();
            this.sliderDateLabel = new System.Windows.Forms.Label();
            this.interventionismTrackBar = new System.Windows.Forms.TrackBar();
            this.isolationismLabel = new System.Windows.Forms.Label();
            this.interventionismLabel = new System.Windows.Forms.Label();
            this.defenseLobbyTrackBar = new System.Windows.Forms.TrackBar();
            this.doveLobbyLabel = new System.Windows.Forms.Label();
            this.hawkLobbyLabel = new System.Windows.Forms.Label();
            this.professionalArmyTrackBar = new System.Windows.Forms.TrackBar();
            this.draftedArmyLabel = new System.Windows.Forms.Label();
            this.standingArmyLabel = new System.Windows.Forms.Label();
            this.freeMarketTrackBar = new System.Windows.Forms.TrackBar();
            this.centralPlanningLabel = new System.Windows.Forms.Label();
            this.freeMarketLabel = new System.Windows.Forms.Label();
            this.freedomTrackBar = new System.Windows.Forms.TrackBar();
            this.closedSocietyLabel = new System.Windows.Forms.Label();
            this.openSocietyLabel = new System.Windows.Forms.Label();
            this.politicalLeftTrackBar = new System.Windows.Forms.TrackBar();
            this.politicalRightLabel = new System.Windows.Forms.Label();
            this.politicalLeftLabel = new System.Windows.Forms.Label();
            this.democraticTrackBar = new System.Windows.Forms.TrackBar();
            this.authoritarianLabel = new System.Windows.Forms.Label();
            this.democraticLabel = new System.Windows.Forms.Label();
            this.sliderDayTextBox = new System.Windows.Forms.TextBox();
            this.sliderMonthTextBox = new System.Windows.Forms.TextBox();
            this.sliderYearTextBox = new System.Windows.Forms.TextBox();
            this.countryTabPage = new System.Windows.Forms.TabPage();
            this.countryModifierGroupBox = new System.Windows.Forms.GroupBox();
            this.groundDefEffTextBox = new System.Windows.Forms.TextBox();
            this.peacetimeIcModifierTextBox = new System.Windows.Forms.TextBox();
            this.relativeManpowerTextBox = new System.Windows.Forms.TextBox();
            this.wartimeIcModifierTextBox = new System.Windows.Forms.TextBox();
            this.industrialModifierTextBox = new System.Windows.Forms.TextBox();
            this.aiGroupBox = new System.Windows.Forms.GroupBox();
            this.aiFileBrowseButton = new System.Windows.Forms.Button();
            this.aiFileNameTextBox = new System.Windows.Forms.TextBox();
            this.countryListBox = new System.Windows.Forms.ListBox();
            this.countryResourceGroupBox = new System.Windows.Forms.GroupBox();
            this.offmapIcTextBox = new System.Windows.Forms.TextBox();
            this.countryIcLabel = new System.Windows.Forms.Label();
            this.offmapManpowerTextBox = new System.Windows.Forms.TextBox();
            this.offmapEscortsTextBox = new System.Windows.Forms.TextBox();
            this.offmapTransportsTextBox = new System.Windows.Forms.TextBox();
            this.offmapMoneyTextBox = new System.Windows.Forms.TextBox();
            this.offmapSuppliesTextBox = new System.Windows.Forms.TextBox();
            this.offmapOilTextBox = new System.Windows.Forms.TextBox();
            this.offmapRareMaterialsTextBox = new System.Windows.Forms.TextBox();
            this.offmapMetalTextBox = new System.Windows.Forms.TextBox();
            this.offmapEnergyTextBox = new System.Windows.Forms.TextBox();
            this.countryManpowerTextBox = new System.Windows.Forms.TextBox();
            this.countryManpowerLabel = new System.Windows.Forms.Label();
            this.countryEscortsTextBox = new System.Windows.Forms.TextBox();
            this.countryEscortsLabel = new System.Windows.Forms.Label();
            this.countryTransportsTextBox = new System.Windows.Forms.TextBox();
            this.countryTransportsLabel = new System.Windows.Forms.Label();
            this.countryMoneyTextBox = new System.Windows.Forms.TextBox();
            this.countryMoneyLabel = new System.Windows.Forms.Label();
            this.countrySuppliesTextBox = new System.Windows.Forms.TextBox();
            this.countrySuppliesLabel = new System.Windows.Forms.Label();
            this.countryOilTextBox = new System.Windows.Forms.TextBox();
            this.countryOilLabel = new System.Windows.Forms.Label();
            this.countryRareMaterialsTextBox = new System.Windows.Forms.TextBox();
            this.countryRareMaterialsLabel = new System.Windows.Forms.Label();
            this.countryMetalTextBox = new System.Windows.Forms.TextBox();
            this.countryMetalLabel = new System.Windows.Forms.Label();
            this.countryEnergyTextBox = new System.Windows.Forms.TextBox();
            this.countryEnergyLabel = new System.Windows.Forms.Label();
            this.countryInfoGroupBox = new System.Windows.Forms.GroupBox();
            this.nukeDayTextBox = new System.Windows.Forms.TextBox();
            this.nukeMonthTextBox = new System.Windows.Forms.TextBox();
            this.nukeYearTextBox = new System.Windows.Forms.TextBox();
            this.dissentTextBox = new System.Windows.Forms.TextBox();
            this.flagExtTextBox = new System.Windows.Forms.TextBox();
            this.nukeTextBox = new System.Windows.Forms.TextBox();
            this.extraTcTextBox = new System.Windows.Forms.TextBox();
            this.belligerenceTextBox = new System.Windows.Forms.TextBox();
            this.regularIdComboBox = new System.Windows.Forms.ComboBox();
            this.countryNameTextBox = new System.Windows.Forms.TextBox();
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
            this.intelligenceGroupBox = new System.Windows.Forms.GroupBox();
            this.spyNumNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.spyNumLabel = new System.Windows.Forms.Label();
            this.diplomacyGroupBox = new System.Windows.Forms.GroupBox();
            this.guaranteeGroupBox = new System.Windows.Forms.GroupBox();
            this.guaranteeCheckBox = new System.Windows.Forms.CheckBox();
            this.guaranteeYearTextBox = new System.Windows.Forms.TextBox();
            this.guaranteeMonthTextBox = new System.Windows.Forms.TextBox();
            this.guaranteeEndLabel = new System.Windows.Forms.Label();
            this.guaranteeDayTextBox = new System.Windows.Forms.TextBox();
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
            this.relationValueNumericUpDown = new System.Windows.Forms.NumericUpDown();
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
            this.relationValueLabel = new System.Windows.Forms.Label();
            this.accessCheckBox = new System.Windows.Forms.CheckBox();
            this.masterCheckBox = new System.Windows.Forms.CheckBox();
            this.controlCheckBox = new System.Windows.Forms.CheckBox();
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
            this.warGroupBox = new System.Windows.Forms.GroupBox();
            this.warDefenderLeaderButton = new System.Windows.Forms.Button();
            this.warAttackerLeaderButton = new System.Windows.Forms.Button();
            this.warDefenderIdLabel = new System.Windows.Forms.Label();
            this.warAttackerIdLabel = new System.Windows.Forms.Label();
            this.warDefenderIdTextBox = new System.Windows.Forms.TextBox();
            this.warDefenderTypeTextBox = new System.Windows.Forms.TextBox();
            this.warAttackerIdTextBox = new System.Windows.Forms.TextBox();
            this.warAttackerTypeTextBox = new System.Windows.Forms.TextBox();
            this.warIdTextBox = new System.Windows.Forms.TextBox();
            this.warTypeTextBox = new System.Windows.Forms.TextBox();
            this.warEndMonthTextBox = new System.Windows.Forms.TextBox();
            this.warIdLabel = new System.Windows.Forms.Label();
            this.warEndYearTextBox = new System.Windows.Forms.TextBox();
            this.warEndDayTextBox = new System.Windows.Forms.TextBox();
            this.warEndDateLabel = new System.Windows.Forms.Label();
            this.warStartMonthTextBox = new System.Windows.Forms.TextBox();
            this.warStartYearTextBox = new System.Windows.Forms.TextBox();
            this.warStartDayTextBox = new System.Windows.Forms.TextBox();
            this.warStartDateLabel = new System.Windows.Forms.Label();
            this.warDefenderLabel = new System.Windows.Forms.Label();
            this.warListView = new System.Windows.Forms.ListView();
            this.warAttackerColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.warDefenderColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.warNewButton = new System.Windows.Forms.Button();
            this.warAttackerRemoveButton = new System.Windows.Forms.Button();
            this.warUpButton = new System.Windows.Forms.Button();
            this.warAttackerListBox = new System.Windows.Forms.ListBox();
            this.warDefenderRemoveButton = new System.Windows.Forms.Button();
            this.warAttackerLabel = new System.Windows.Forms.Label();
            this.warDefenderListBox = new System.Windows.Forms.ListBox();
            this.warDownButton = new System.Windows.Forms.Button();
            this.warCountryListBox = new System.Windows.Forms.ListBox();
            this.warAttackerAddButton = new System.Windows.Forms.Button();
            this.warDefenderAddButton = new System.Windows.Forms.Button();
            this.warRemoveButton = new System.Windows.Forms.Button();
            this.allianceGroupBox = new System.Windows.Forms.GroupBox();
            this.allianceIdTextBox = new System.Windows.Forms.TextBox();
            this.allianceTypeTextBox = new System.Windows.Forms.TextBox();
            this.allianceIdLabel = new System.Windows.Forms.Label();
            this.allianceNameTextBox = new System.Windows.Forms.TextBox();
            this.allianceNameLabel = new System.Windows.Forms.Label();
            this.allianceListView = new System.Windows.Forms.ListView();
            this.allianceNameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.allianceParticipantColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.allianceParticipantLabel = new System.Windows.Forms.Label();
            this.allianceLeaderButton = new System.Windows.Forms.Button();
            this.allianceParticipantListBox = new System.Windows.Forms.ListBox();
            this.allianceUpButton = new System.Windows.Forms.Button();
            this.allianceParticipantAddButton = new System.Windows.Forms.Button();
            this.allianceCountryListBox = new System.Windows.Forms.ListBox();
            this.allianceRemoveButton = new System.Windows.Forms.Button();
            this.allianceNewButton = new System.Windows.Forms.Button();
            this.allianceParticipantRemoveButton = new System.Windows.Forms.Button();
            this.allianceDownButton = new System.Windows.Forms.Button();
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
            this.countrySelectionGroupBox = new System.Windows.Forms.GroupBox();
            this.selectableRemoveButton = new System.Windows.Forms.Button();
            this.selectableAddButton = new System.Windows.Forms.Button();
            this.unselectableListBox = new System.Windows.Forms.ListBox();
            this.selectableListBox = new System.Windows.Forms.ListBox();
            this.propagandaPictureBox = new System.Windows.Forms.PictureBox();
            this.propagandaBrowseButton = new System.Windows.Forms.Button();
            this.propagandaTextBox = new System.Windows.Forms.TextBox();
            this.propagandaLabel = new System.Windows.Forms.Label();
            this.countryDescTextBox = new System.Windows.Forms.TextBox();
            this.countryDescLabel = new System.Windows.Forms.Label();
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
            this.scenarioTabControl = new System.Windows.Forms.TabControl();
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
            provinceSyntheticOilLabel = new System.Windows.Forms.Label();
            provinceSyntheticRaresLabel = new System.Windows.Forms.Label();
            provinceNuclearPowerLabel = new System.Windows.Forms.Label();
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
            peacetimeIcModifierLabel = new System.Windows.Forms.Label();
            wartimeIcModifierLabel = new System.Windows.Forms.Label();
            groundDefEffLabel = new System.Windows.Forms.Label();
            relativeManpowerLabel = new System.Windows.Forms.Label();
            industrialModifierLabel = new System.Windows.Forms.Label();
            aiFileLabel = new System.Windows.Forms.Label();
            countryOffmapLabel = new System.Windows.Forms.Label();
            countryStockpileLabel = new System.Windows.Forms.Label();
            nukeDateLabel = new System.Windows.Forms.Label();
            dissentLabel = new System.Windows.Forms.Label();
            flagExtLabel = new System.Windows.Forms.Label();
            nukeLabel = new System.Windows.Forms.Label();
            extraTcLabel = new System.Windows.Forms.Label();
            belligerenceLabel = new System.Windows.Forms.Label();
            regularIdLabel = new System.Windows.Forms.Label();
            countryNameLabel = new System.Windows.Forms.Label();
            tradeStartDateLabel = new System.Windows.Forms.Label();
            tradeEndDateLabel = new System.Windows.Forms.Label();
            tradeIdLabel = new System.Windows.Forms.Label();
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
            this.governmentTabPage.SuspendLayout();
            this.cabinetGroupBox.SuspendLayout();
            this.politicalSliderGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.interventionismTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.defenseLobbyTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.professionalArmyTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.freeMarketTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.freedomTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.politicalLeftTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.democraticTrackBar)).BeginInit();
            this.countryTabPage.SuspendLayout();
            this.countryModifierGroupBox.SuspendLayout();
            this.aiGroupBox.SuspendLayout();
            this.countryResourceGroupBox.SuspendLayout();
            this.countryInfoGroupBox.SuspendLayout();
            this.tradeTabPage.SuspendLayout();
            this.tradeDealsGroupBox.SuspendLayout();
            this.tradeInfoGroupBox.SuspendLayout();
            this.relationTabPage.SuspendLayout();
            this.intelligenceGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spyNumNumericUpDown)).BeginInit();
            this.diplomacyGroupBox.SuspendLayout();
            this.guaranteeGroupBox.SuspendLayout();
            this.peaceGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.relationValueNumericUpDown)).BeginInit();
            this.nonAggressionGroupBox.SuspendLayout();
            this.allianceTabPage.SuspendLayout();
            this.warGroupBox.SuspendLayout();
            this.allianceGroupBox.SuspendLayout();
            this.mainTabPage.SuspendLayout();
            this.optionGroupBox.SuspendLayout();
            this.countrySelectionGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propagandaPictureBox)).BeginInit();
            this.infoGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelPictureBox)).BeginInit();
            this.folderGroupBox.SuspendLayout();
            this.typeGroupBox.SuspendLayout();
            this.scenarioTabControl.SuspendLayout();
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
            // provinceSyntheticOilLabel
            // 
            resources.ApplyResources(provinceSyntheticOilLabel, "provinceSyntheticOilLabel");
            provinceSyntheticOilLabel.Name = "provinceSyntheticOilLabel";
            // 
            // provinceSyntheticRaresLabel
            // 
            resources.ApplyResources(provinceSyntheticRaresLabel, "provinceSyntheticRaresLabel");
            provinceSyntheticRaresLabel.Name = "provinceSyntheticRaresLabel";
            // 
            // provinceNuclearPowerLabel
            // 
            resources.ApplyResources(provinceNuclearPowerLabel, "provinceNuclearPowerLabel");
            provinceNuclearPowerLabel.Name = "provinceNuclearPowerLabel";
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
            this.inventionsListView.CheckBoxes = true;
            resources.ApplyResources(this.inventionsListView, "inventionsListView");
            this.inventionsListView.MultiSelect = false;
            this.inventionsListView.Name = "inventionsListView";
            this.inventionsListView.UseCompatibleStateImageBehavior = false;
            this.inventionsListView.View = System.Windows.Forms.View.List;
            // 
            // blueprintsListView
            // 
            this.blueprintsListView.CheckBoxes = true;
            resources.ApplyResources(this.blueprintsListView, "blueprintsListView");
            this.blueprintsListView.MultiSelect = false;
            this.blueprintsListView.Name = "blueprintsListView";
            this.blueprintsListView.UseCompatibleStateImageBehavior = false;
            this.blueprintsListView.View = System.Windows.Forms.View.List;
            // 
            // ownedTechsListView
            // 
            this.ownedTechsListView.CheckBoxes = true;
            resources.ApplyResources(this.ownedTechsListView, "ownedTechsListView");
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
            this.techCountryListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            resources.ApplyResources(this.techCountryListBox, "techCountryListBox");
            this.techCountryListBox.FormattingEnabled = true;
            this.techCountryListBox.Name = "techCountryListBox";
            this.techCountryListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnCountryListBoxDrawItem);
            this.techCountryListBox.SelectedIndexChanged += new System.EventHandler(this.OnTechCountryListBoxSelectedIndexChanged);
            // 
            // peacetimeIcModifierLabel
            // 
            resources.ApplyResources(peacetimeIcModifierLabel, "peacetimeIcModifierLabel");
            peacetimeIcModifierLabel.Name = "peacetimeIcModifierLabel";
            // 
            // wartimeIcModifierLabel
            // 
            resources.ApplyResources(wartimeIcModifierLabel, "wartimeIcModifierLabel");
            wartimeIcModifierLabel.Name = "wartimeIcModifierLabel";
            // 
            // groundDefEffLabel
            // 
            resources.ApplyResources(groundDefEffLabel, "groundDefEffLabel");
            groundDefEffLabel.Name = "groundDefEffLabel";
            // 
            // relativeManpowerLabel
            // 
            resources.ApplyResources(relativeManpowerLabel, "relativeManpowerLabel");
            relativeManpowerLabel.Name = "relativeManpowerLabel";
            // 
            // industrialModifierLabel
            // 
            resources.ApplyResources(industrialModifierLabel, "industrialModifierLabel");
            industrialModifierLabel.Name = "industrialModifierLabel";
            // 
            // aiFileLabel
            // 
            resources.ApplyResources(aiFileLabel, "aiFileLabel");
            aiFileLabel.Name = "aiFileLabel";
            // 
            // countryOffmapLabel
            // 
            resources.ApplyResources(countryOffmapLabel, "countryOffmapLabel");
            countryOffmapLabel.Name = "countryOffmapLabel";
            // 
            // countryStockpileLabel
            // 
            resources.ApplyResources(countryStockpileLabel, "countryStockpileLabel");
            countryStockpileLabel.Name = "countryStockpileLabel";
            // 
            // nukeDateLabel
            // 
            resources.ApplyResources(nukeDateLabel, "nukeDateLabel");
            nukeDateLabel.Name = "nukeDateLabel";
            // 
            // dissentLabel
            // 
            resources.ApplyResources(dissentLabel, "dissentLabel");
            dissentLabel.Name = "dissentLabel";
            // 
            // flagExtLabel
            // 
            resources.ApplyResources(flagExtLabel, "flagExtLabel");
            flagExtLabel.Name = "flagExtLabel";
            // 
            // nukeLabel
            // 
            resources.ApplyResources(nukeLabel, "nukeLabel");
            nukeLabel.Name = "nukeLabel";
            // 
            // extraTcLabel
            // 
            resources.ApplyResources(extraTcLabel, "extraTcLabel");
            extraTcLabel.Name = "extraTcLabel";
            // 
            // belligerenceLabel
            // 
            resources.ApplyResources(belligerenceLabel, "belligerenceLabel");
            belligerenceLabel.Name = "belligerenceLabel";
            // 
            // regularIdLabel
            // 
            resources.ApplyResources(regularIdLabel, "regularIdLabel");
            regularIdLabel.Name = "regularIdLabel";
            // 
            // countryNameLabel
            // 
            resources.ApplyResources(countryNameLabel, "countryNameLabel");
            countryNameLabel.Name = "countryNameLabel";
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
            this.provinceCountryGroupBox.Controls.Add(this.claimedProvinceCheckBox);
            this.provinceCountryGroupBox.Controls.Add(this.capitalCheckBox);
            this.provinceCountryGroupBox.Controls.Add(this.controlledProvinceCheckBox);
            this.provinceCountryGroupBox.Controls.Add(this.coreProvinceCheckBox);
            this.provinceCountryGroupBox.Controls.Add(this.ownedProvinceCheckBox);
            resources.ApplyResources(this.provinceCountryGroupBox, "provinceCountryGroupBox");
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
            this.provinceInfoGroupBox.Controls.Add(this.revoltRiskTextBox);
            this.provinceInfoGroupBox.Controls.Add(provinceNameLabel);
            this.provinceInfoGroupBox.Controls.Add(this.vpTextBox);
            this.provinceInfoGroupBox.Controls.Add(this.provinceNameTextBox);
            this.provinceInfoGroupBox.Controls.Add(provinceVpLabel);
            this.provinceInfoGroupBox.Controls.Add(provinceRevoltRiskLabel);
            resources.ApplyResources(this.provinceInfoGroupBox, "provinceInfoGroupBox");
            this.provinceInfoGroupBox.Name = "provinceInfoGroupBox";
            this.provinceInfoGroupBox.TabStop = false;
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
            // provinceNameTextBox
            // 
            resources.ApplyResources(this.provinceNameTextBox, "provinceNameTextBox");
            this.provinceNameTextBox.Name = "provinceNameTextBox";
            this.provinceNameTextBox.TextChanged += new System.EventHandler(this.OnProvinceStringItemTextBoxTextChanged);
            // 
            // provinceResourceGroupBox
            // 
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
            resources.ApplyResources(this.provinceResourceGroupBox, "provinceResourceGroupBox");
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
            this.provinceBuildingGroupBox.Controls.Add(provinceNuclearPowerLabel);
            this.provinceBuildingGroupBox.Controls.Add(provinceRocketTestLabel);
            this.provinceBuildingGroupBox.Controls.Add(provinceSyntheticOilLabel);
            this.provinceBuildingGroupBox.Controls.Add(provinceNuclearReactorLabel);
            this.provinceBuildingGroupBox.Controls.Add(provinceSyntheticRaresLabel);
            resources.ApplyResources(this.provinceBuildingGroupBox, "provinceBuildingGroupBox");
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
            this.provinceListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.provinceIdColumnHeader,
            this.provinceNameColumnHeader,
            this.capitalProvinceColumnHeader,
            this.coreProvinceColumnHeader,
            this.ownedColumnHeader,
            this.controlledProvinceColumnHeader,
            this.claimedProvinceColumnHeader});
            resources.ApplyResources(this.provinceListView, "provinceListView");
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
            this.governmentTabPage.Controls.Add(this.cabinetGroupBox);
            this.governmentTabPage.Controls.Add(this.governmentCountryListBox);
            this.governmentTabPage.Controls.Add(this.politicalSliderGroupBox);
            resources.ApplyResources(this.governmentTabPage, "governmentTabPage");
            this.governmentTabPage.Name = "governmentTabPage";
            // 
            // cabinetGroupBox
            // 
            this.cabinetGroupBox.Controls.Add(this.chiefOfAirIdTextBox);
            this.cabinetGroupBox.Controls.Add(this.chiefOfAirTypeTextBox);
            this.cabinetGroupBox.Controls.Add(this.chiefOfAirComboBox);
            this.cabinetGroupBox.Controls.Add(this.chiefOfAirLabel);
            this.cabinetGroupBox.Controls.Add(this.chiefOfNavyIdTextBox);
            this.cabinetGroupBox.Controls.Add(this.chiefOfNavyTypeTextBox);
            this.cabinetGroupBox.Controls.Add(this.chiefOfNavyComboBox);
            this.cabinetGroupBox.Controls.Add(this.chiefOfNavyLabel);
            this.cabinetGroupBox.Controls.Add(this.chiefOfArmyIdTextBox);
            this.cabinetGroupBox.Controls.Add(this.chiefOfArmyTypeTextBox);
            this.cabinetGroupBox.Controls.Add(this.chiefOfArmyComboBox);
            this.cabinetGroupBox.Controls.Add(this.chiefOfArmyLabel);
            this.cabinetGroupBox.Controls.Add(this.chiefOfStaffIdTextBox);
            this.cabinetGroupBox.Controls.Add(this.chiefOfStaffTypeTextBox);
            this.cabinetGroupBox.Controls.Add(this.chiefOfStaffComboBox);
            this.cabinetGroupBox.Controls.Add(this.chiefOfStaffLabel);
            this.cabinetGroupBox.Controls.Add(this.ministerOfIntelligenceIdTextBox);
            this.cabinetGroupBox.Controls.Add(this.ministerOfIntelligenceTypeTextBox);
            this.cabinetGroupBox.Controls.Add(this.ministerOfIntelligenceComboBox);
            this.cabinetGroupBox.Controls.Add(this.ministerOfIntelligenceLabel);
            this.cabinetGroupBox.Controls.Add(this.ministerOfSecurityIdTextBox);
            this.cabinetGroupBox.Controls.Add(this.ministerOfSecurityTypeTextBox);
            this.cabinetGroupBox.Controls.Add(this.ministerOfSecurityComboBox);
            this.cabinetGroupBox.Controls.Add(this.ministerOfSecurityLabel);
            this.cabinetGroupBox.Controls.Add(this.armamentMinisterIdTextBox);
            this.cabinetGroupBox.Controls.Add(this.armamentMinisterTypeTextBox);
            this.cabinetGroupBox.Controls.Add(this.armamentMinisterComboBox);
            this.cabinetGroupBox.Controls.Add(this.armamentMinisterLabel);
            this.cabinetGroupBox.Controls.Add(this.foreignMinisterIdTextBox);
            this.cabinetGroupBox.Controls.Add(this.foreignMinisterTypeTextBox);
            this.cabinetGroupBox.Controls.Add(this.foreignMinisterComboBox);
            this.cabinetGroupBox.Controls.Add(this.foreignMinisterlabel);
            this.cabinetGroupBox.Controls.Add(this.headOfGovernmentIdTextBox);
            this.cabinetGroupBox.Controls.Add(this.headOfGovernmentTypeTextBox);
            this.cabinetGroupBox.Controls.Add(this.headOfGovernmentComboBox);
            this.cabinetGroupBox.Controls.Add(this.headOfGovernmentLabel);
            this.cabinetGroupBox.Controls.Add(this.headOfStateIdTextBox);
            this.cabinetGroupBox.Controls.Add(this.headOfStateTypeTextBox);
            this.cabinetGroupBox.Controls.Add(this.headOfStateComboBox);
            this.cabinetGroupBox.Controls.Add(this.headOfStateLabel);
            resources.ApplyResources(this.cabinetGroupBox, "cabinetGroupBox");
            this.cabinetGroupBox.Name = "cabinetGroupBox";
            this.cabinetGroupBox.TabStop = false;
            // 
            // chiefOfAirIdTextBox
            // 
            resources.ApplyResources(this.chiefOfAirIdTextBox, "chiefOfAirIdTextBox");
            this.chiefOfAirIdTextBox.Name = "chiefOfAirIdTextBox";
            this.chiefOfAirIdTextBox.Validated += new System.EventHandler(this.OnGovernmentIntItemTextBoxValidated);
            // 
            // chiefOfAirTypeTextBox
            // 
            resources.ApplyResources(this.chiefOfAirTypeTextBox, "chiefOfAirTypeTextBox");
            this.chiefOfAirTypeTextBox.Name = "chiefOfAirTypeTextBox";
            this.chiefOfAirTypeTextBox.Validated += new System.EventHandler(this.OnGovernmentIntItemTextBoxValidated);
            // 
            // chiefOfAirComboBox
            // 
            this.chiefOfAirComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.chiefOfAirComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.chiefOfAirComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.chiefOfAirComboBox, "chiefOfAirComboBox");
            this.chiefOfAirComboBox.Name = "chiefOfAirComboBox";
            this.chiefOfAirComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnCabinetComboBoxDrawItem);
            this.chiefOfAirComboBox.SelectedIndexChanged += new System.EventHandler(this.OnCabinetComboBoxSelectedIndexChanged);
            // 
            // chiefOfAirLabel
            // 
            resources.ApplyResources(this.chiefOfAirLabel, "chiefOfAirLabel");
            this.chiefOfAirLabel.Name = "chiefOfAirLabel";
            // 
            // chiefOfNavyIdTextBox
            // 
            resources.ApplyResources(this.chiefOfNavyIdTextBox, "chiefOfNavyIdTextBox");
            this.chiefOfNavyIdTextBox.Name = "chiefOfNavyIdTextBox";
            this.chiefOfNavyIdTextBox.Validated += new System.EventHandler(this.OnGovernmentIntItemTextBoxValidated);
            // 
            // chiefOfNavyTypeTextBox
            // 
            resources.ApplyResources(this.chiefOfNavyTypeTextBox, "chiefOfNavyTypeTextBox");
            this.chiefOfNavyTypeTextBox.Name = "chiefOfNavyTypeTextBox";
            this.chiefOfNavyTypeTextBox.Validated += new System.EventHandler(this.OnGovernmentIntItemTextBoxValidated);
            // 
            // chiefOfNavyComboBox
            // 
            this.chiefOfNavyComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.chiefOfNavyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.chiefOfNavyComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.chiefOfNavyComboBox, "chiefOfNavyComboBox");
            this.chiefOfNavyComboBox.Name = "chiefOfNavyComboBox";
            this.chiefOfNavyComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnCabinetComboBoxDrawItem);
            this.chiefOfNavyComboBox.SelectedIndexChanged += new System.EventHandler(this.OnCabinetComboBoxSelectedIndexChanged);
            // 
            // chiefOfNavyLabel
            // 
            resources.ApplyResources(this.chiefOfNavyLabel, "chiefOfNavyLabel");
            this.chiefOfNavyLabel.Name = "chiefOfNavyLabel";
            // 
            // chiefOfArmyIdTextBox
            // 
            resources.ApplyResources(this.chiefOfArmyIdTextBox, "chiefOfArmyIdTextBox");
            this.chiefOfArmyIdTextBox.Name = "chiefOfArmyIdTextBox";
            this.chiefOfArmyIdTextBox.Validated += new System.EventHandler(this.OnGovernmentIntItemTextBoxValidated);
            // 
            // chiefOfArmyTypeTextBox
            // 
            resources.ApplyResources(this.chiefOfArmyTypeTextBox, "chiefOfArmyTypeTextBox");
            this.chiefOfArmyTypeTextBox.Name = "chiefOfArmyTypeTextBox";
            this.chiefOfArmyTypeTextBox.Validated += new System.EventHandler(this.OnGovernmentIntItemTextBoxValidated);
            // 
            // chiefOfArmyComboBox
            // 
            this.chiefOfArmyComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.chiefOfArmyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.chiefOfArmyComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.chiefOfArmyComboBox, "chiefOfArmyComboBox");
            this.chiefOfArmyComboBox.Name = "chiefOfArmyComboBox";
            this.chiefOfArmyComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnCabinetComboBoxDrawItem);
            this.chiefOfArmyComboBox.SelectedIndexChanged += new System.EventHandler(this.OnCabinetComboBoxSelectedIndexChanged);
            // 
            // chiefOfArmyLabel
            // 
            resources.ApplyResources(this.chiefOfArmyLabel, "chiefOfArmyLabel");
            this.chiefOfArmyLabel.Name = "chiefOfArmyLabel";
            // 
            // chiefOfStaffIdTextBox
            // 
            resources.ApplyResources(this.chiefOfStaffIdTextBox, "chiefOfStaffIdTextBox");
            this.chiefOfStaffIdTextBox.Name = "chiefOfStaffIdTextBox";
            this.chiefOfStaffIdTextBox.Validated += new System.EventHandler(this.OnGovernmentIntItemTextBoxValidated);
            // 
            // chiefOfStaffTypeTextBox
            // 
            resources.ApplyResources(this.chiefOfStaffTypeTextBox, "chiefOfStaffTypeTextBox");
            this.chiefOfStaffTypeTextBox.Name = "chiefOfStaffTypeTextBox";
            this.chiefOfStaffTypeTextBox.Validated += new System.EventHandler(this.OnGovernmentIntItemTextBoxValidated);
            // 
            // chiefOfStaffComboBox
            // 
            this.chiefOfStaffComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.chiefOfStaffComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.chiefOfStaffComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.chiefOfStaffComboBox, "chiefOfStaffComboBox");
            this.chiefOfStaffComboBox.Name = "chiefOfStaffComboBox";
            this.chiefOfStaffComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnCabinetComboBoxDrawItem);
            this.chiefOfStaffComboBox.SelectedIndexChanged += new System.EventHandler(this.OnCabinetComboBoxSelectedIndexChanged);
            // 
            // chiefOfStaffLabel
            // 
            resources.ApplyResources(this.chiefOfStaffLabel, "chiefOfStaffLabel");
            this.chiefOfStaffLabel.Name = "chiefOfStaffLabel";
            // 
            // ministerOfIntelligenceIdTextBox
            // 
            resources.ApplyResources(this.ministerOfIntelligenceIdTextBox, "ministerOfIntelligenceIdTextBox");
            this.ministerOfIntelligenceIdTextBox.Name = "ministerOfIntelligenceIdTextBox";
            this.ministerOfIntelligenceIdTextBox.Validated += new System.EventHandler(this.OnGovernmentIntItemTextBoxValidated);
            // 
            // ministerOfIntelligenceTypeTextBox
            // 
            resources.ApplyResources(this.ministerOfIntelligenceTypeTextBox, "ministerOfIntelligenceTypeTextBox");
            this.ministerOfIntelligenceTypeTextBox.Name = "ministerOfIntelligenceTypeTextBox";
            this.ministerOfIntelligenceTypeTextBox.Validated += new System.EventHandler(this.OnGovernmentIntItemTextBoxValidated);
            // 
            // ministerOfIntelligenceComboBox
            // 
            this.ministerOfIntelligenceComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ministerOfIntelligenceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ministerOfIntelligenceComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.ministerOfIntelligenceComboBox, "ministerOfIntelligenceComboBox");
            this.ministerOfIntelligenceComboBox.Name = "ministerOfIntelligenceComboBox";
            this.ministerOfIntelligenceComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnCabinetComboBoxDrawItem);
            this.ministerOfIntelligenceComboBox.SelectedIndexChanged += new System.EventHandler(this.OnCabinetComboBoxSelectedIndexChanged);
            // 
            // ministerOfIntelligenceLabel
            // 
            resources.ApplyResources(this.ministerOfIntelligenceLabel, "ministerOfIntelligenceLabel");
            this.ministerOfIntelligenceLabel.Name = "ministerOfIntelligenceLabel";
            // 
            // ministerOfSecurityIdTextBox
            // 
            resources.ApplyResources(this.ministerOfSecurityIdTextBox, "ministerOfSecurityIdTextBox");
            this.ministerOfSecurityIdTextBox.Name = "ministerOfSecurityIdTextBox";
            this.ministerOfSecurityIdTextBox.Validated += new System.EventHandler(this.OnGovernmentIntItemTextBoxValidated);
            // 
            // ministerOfSecurityTypeTextBox
            // 
            resources.ApplyResources(this.ministerOfSecurityTypeTextBox, "ministerOfSecurityTypeTextBox");
            this.ministerOfSecurityTypeTextBox.Name = "ministerOfSecurityTypeTextBox";
            this.ministerOfSecurityTypeTextBox.Validated += new System.EventHandler(this.OnGovernmentIntItemTextBoxValidated);
            // 
            // ministerOfSecurityComboBox
            // 
            this.ministerOfSecurityComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ministerOfSecurityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ministerOfSecurityComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.ministerOfSecurityComboBox, "ministerOfSecurityComboBox");
            this.ministerOfSecurityComboBox.Name = "ministerOfSecurityComboBox";
            this.ministerOfSecurityComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnCabinetComboBoxDrawItem);
            this.ministerOfSecurityComboBox.SelectedIndexChanged += new System.EventHandler(this.OnCabinetComboBoxSelectedIndexChanged);
            // 
            // ministerOfSecurityLabel
            // 
            resources.ApplyResources(this.ministerOfSecurityLabel, "ministerOfSecurityLabel");
            this.ministerOfSecurityLabel.Name = "ministerOfSecurityLabel";
            // 
            // armamentMinisterIdTextBox
            // 
            resources.ApplyResources(this.armamentMinisterIdTextBox, "armamentMinisterIdTextBox");
            this.armamentMinisterIdTextBox.Name = "armamentMinisterIdTextBox";
            this.armamentMinisterIdTextBox.Validated += new System.EventHandler(this.OnGovernmentIntItemTextBoxValidated);
            // 
            // armamentMinisterTypeTextBox
            // 
            resources.ApplyResources(this.armamentMinisterTypeTextBox, "armamentMinisterTypeTextBox");
            this.armamentMinisterTypeTextBox.Name = "armamentMinisterTypeTextBox";
            this.armamentMinisterTypeTextBox.Validated += new System.EventHandler(this.OnGovernmentIntItemTextBoxValidated);
            // 
            // armamentMinisterComboBox
            // 
            this.armamentMinisterComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.armamentMinisterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.armamentMinisterComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.armamentMinisterComboBox, "armamentMinisterComboBox");
            this.armamentMinisterComboBox.Name = "armamentMinisterComboBox";
            this.armamentMinisterComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnCabinetComboBoxDrawItem);
            this.armamentMinisterComboBox.SelectedIndexChanged += new System.EventHandler(this.OnCabinetComboBoxSelectedIndexChanged);
            // 
            // armamentMinisterLabel
            // 
            resources.ApplyResources(this.armamentMinisterLabel, "armamentMinisterLabel");
            this.armamentMinisterLabel.Name = "armamentMinisterLabel";
            // 
            // foreignMinisterIdTextBox
            // 
            resources.ApplyResources(this.foreignMinisterIdTextBox, "foreignMinisterIdTextBox");
            this.foreignMinisterIdTextBox.Name = "foreignMinisterIdTextBox";
            this.foreignMinisterIdTextBox.Validated += new System.EventHandler(this.OnGovernmentIntItemTextBoxValidated);
            // 
            // foreignMinisterTypeTextBox
            // 
            resources.ApplyResources(this.foreignMinisterTypeTextBox, "foreignMinisterTypeTextBox");
            this.foreignMinisterTypeTextBox.Name = "foreignMinisterTypeTextBox";
            this.foreignMinisterTypeTextBox.Validated += new System.EventHandler(this.OnGovernmentIntItemTextBoxValidated);
            // 
            // foreignMinisterComboBox
            // 
            this.foreignMinisterComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.foreignMinisterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.foreignMinisterComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.foreignMinisterComboBox, "foreignMinisterComboBox");
            this.foreignMinisterComboBox.Name = "foreignMinisterComboBox";
            this.foreignMinisterComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnCabinetComboBoxDrawItem);
            this.foreignMinisterComboBox.SelectedIndexChanged += new System.EventHandler(this.OnCabinetComboBoxSelectedIndexChanged);
            // 
            // foreignMinisterlabel
            // 
            resources.ApplyResources(this.foreignMinisterlabel, "foreignMinisterlabel");
            this.foreignMinisterlabel.Name = "foreignMinisterlabel";
            // 
            // headOfGovernmentIdTextBox
            // 
            resources.ApplyResources(this.headOfGovernmentIdTextBox, "headOfGovernmentIdTextBox");
            this.headOfGovernmentIdTextBox.Name = "headOfGovernmentIdTextBox";
            this.headOfGovernmentIdTextBox.Validated += new System.EventHandler(this.OnGovernmentIntItemTextBoxValidated);
            // 
            // headOfGovernmentTypeTextBox
            // 
            resources.ApplyResources(this.headOfGovernmentTypeTextBox, "headOfGovernmentTypeTextBox");
            this.headOfGovernmentTypeTextBox.Name = "headOfGovernmentTypeTextBox";
            this.headOfGovernmentTypeTextBox.Validated += new System.EventHandler(this.OnGovernmentIntItemTextBoxValidated);
            // 
            // headOfGovernmentComboBox
            // 
            this.headOfGovernmentComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.headOfGovernmentComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.headOfGovernmentComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.headOfGovernmentComboBox, "headOfGovernmentComboBox");
            this.headOfGovernmentComboBox.Name = "headOfGovernmentComboBox";
            this.headOfGovernmentComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnCabinetComboBoxDrawItem);
            this.headOfGovernmentComboBox.SelectedIndexChanged += new System.EventHandler(this.OnCabinetComboBoxSelectedIndexChanged);
            // 
            // headOfGovernmentLabel
            // 
            resources.ApplyResources(this.headOfGovernmentLabel, "headOfGovernmentLabel");
            this.headOfGovernmentLabel.Name = "headOfGovernmentLabel";
            // 
            // headOfStateIdTextBox
            // 
            resources.ApplyResources(this.headOfStateIdTextBox, "headOfStateIdTextBox");
            this.headOfStateIdTextBox.Name = "headOfStateIdTextBox";
            this.headOfStateIdTextBox.Validated += new System.EventHandler(this.OnGovernmentIntItemTextBoxValidated);
            // 
            // headOfStateTypeTextBox
            // 
            resources.ApplyResources(this.headOfStateTypeTextBox, "headOfStateTypeTextBox");
            this.headOfStateTypeTextBox.Name = "headOfStateTypeTextBox";
            this.headOfStateTypeTextBox.Validated += new System.EventHandler(this.OnGovernmentIntItemTextBoxValidated);
            // 
            // headOfStateComboBox
            // 
            this.headOfStateComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.headOfStateComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.headOfStateComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.headOfStateComboBox, "headOfStateComboBox");
            this.headOfStateComboBox.Name = "headOfStateComboBox";
            this.headOfStateComboBox.Tag = "";
            this.headOfStateComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnCabinetComboBoxDrawItem);
            this.headOfStateComboBox.SelectedIndexChanged += new System.EventHandler(this.OnCabinetComboBoxSelectedIndexChanged);
            // 
            // headOfStateLabel
            // 
            resources.ApplyResources(this.headOfStateLabel, "headOfStateLabel");
            this.headOfStateLabel.Name = "headOfStateLabel";
            // 
            // governmentCountryListBox
            // 
            this.governmentCountryListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            resources.ApplyResources(this.governmentCountryListBox, "governmentCountryListBox");
            this.governmentCountryListBox.FormattingEnabled = true;
            this.governmentCountryListBox.Name = "governmentCountryListBox";
            this.governmentCountryListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnCountryListBoxDrawItem);
            this.governmentCountryListBox.SelectedIndexChanged += new System.EventHandler(this.OnGovernmentCountryListBoxSelectedIndexChanged);
            // 
            // politicalSliderGroupBox
            // 
            this.politicalSliderGroupBox.Controls.Add(this.sliderDateLabel);
            this.politicalSliderGroupBox.Controls.Add(this.interventionismTrackBar);
            this.politicalSliderGroupBox.Controls.Add(this.isolationismLabel);
            this.politicalSliderGroupBox.Controls.Add(this.interventionismLabel);
            this.politicalSliderGroupBox.Controls.Add(this.defenseLobbyTrackBar);
            this.politicalSliderGroupBox.Controls.Add(this.doveLobbyLabel);
            this.politicalSliderGroupBox.Controls.Add(this.hawkLobbyLabel);
            this.politicalSliderGroupBox.Controls.Add(this.professionalArmyTrackBar);
            this.politicalSliderGroupBox.Controls.Add(this.draftedArmyLabel);
            this.politicalSliderGroupBox.Controls.Add(this.standingArmyLabel);
            this.politicalSliderGroupBox.Controls.Add(this.freeMarketTrackBar);
            this.politicalSliderGroupBox.Controls.Add(this.centralPlanningLabel);
            this.politicalSliderGroupBox.Controls.Add(this.freeMarketLabel);
            this.politicalSliderGroupBox.Controls.Add(this.freedomTrackBar);
            this.politicalSliderGroupBox.Controls.Add(this.closedSocietyLabel);
            this.politicalSliderGroupBox.Controls.Add(this.openSocietyLabel);
            this.politicalSliderGroupBox.Controls.Add(this.politicalLeftTrackBar);
            this.politicalSliderGroupBox.Controls.Add(this.politicalRightLabel);
            this.politicalSliderGroupBox.Controls.Add(this.politicalLeftLabel);
            this.politicalSliderGroupBox.Controls.Add(this.democraticTrackBar);
            this.politicalSliderGroupBox.Controls.Add(this.authoritarianLabel);
            this.politicalSliderGroupBox.Controls.Add(this.democraticLabel);
            this.politicalSliderGroupBox.Controls.Add(this.sliderDayTextBox);
            this.politicalSliderGroupBox.Controls.Add(this.sliderMonthTextBox);
            this.politicalSliderGroupBox.Controls.Add(this.sliderYearTextBox);
            resources.ApplyResources(this.politicalSliderGroupBox, "politicalSliderGroupBox");
            this.politicalSliderGroupBox.Name = "politicalSliderGroupBox";
            this.politicalSliderGroupBox.TabStop = false;
            // 
            // sliderDateLabel
            // 
            resources.ApplyResources(this.sliderDateLabel, "sliderDateLabel");
            this.sliderDateLabel.Name = "sliderDateLabel";
            // 
            // interventionismTrackBar
            // 
            resources.ApplyResources(this.interventionismTrackBar, "interventionismTrackBar");
            this.interventionismTrackBar.LargeChange = 1;
            this.interventionismTrackBar.Minimum = 1;
            this.interventionismTrackBar.Name = "interventionismTrackBar";
            this.interventionismTrackBar.Value = 6;
            this.interventionismTrackBar.Scroll += new System.EventHandler(this.OnPoliticalSliderTrackBarScroll);
            // 
            // isolationismLabel
            // 
            resources.ApplyResources(this.isolationismLabel, "isolationismLabel");
            this.isolationismLabel.Name = "isolationismLabel";
            // 
            // interventionismLabel
            // 
            resources.ApplyResources(this.interventionismLabel, "interventionismLabel");
            this.interventionismLabel.Name = "interventionismLabel";
            // 
            // defenseLobbyTrackBar
            // 
            resources.ApplyResources(this.defenseLobbyTrackBar, "defenseLobbyTrackBar");
            this.defenseLobbyTrackBar.LargeChange = 1;
            this.defenseLobbyTrackBar.Minimum = 1;
            this.defenseLobbyTrackBar.Name = "defenseLobbyTrackBar";
            this.defenseLobbyTrackBar.Value = 6;
            this.defenseLobbyTrackBar.Scroll += new System.EventHandler(this.OnPoliticalSliderTrackBarScroll);
            // 
            // doveLobbyLabel
            // 
            resources.ApplyResources(this.doveLobbyLabel, "doveLobbyLabel");
            this.doveLobbyLabel.Name = "doveLobbyLabel";
            // 
            // hawkLobbyLabel
            // 
            resources.ApplyResources(this.hawkLobbyLabel, "hawkLobbyLabel");
            this.hawkLobbyLabel.Name = "hawkLobbyLabel";
            // 
            // professionalArmyTrackBar
            // 
            resources.ApplyResources(this.professionalArmyTrackBar, "professionalArmyTrackBar");
            this.professionalArmyTrackBar.LargeChange = 1;
            this.professionalArmyTrackBar.Minimum = 1;
            this.professionalArmyTrackBar.Name = "professionalArmyTrackBar";
            this.professionalArmyTrackBar.Value = 6;
            this.professionalArmyTrackBar.Scroll += new System.EventHandler(this.OnPoliticalSliderTrackBarScroll);
            // 
            // draftedArmyLabel
            // 
            resources.ApplyResources(this.draftedArmyLabel, "draftedArmyLabel");
            this.draftedArmyLabel.Name = "draftedArmyLabel";
            // 
            // standingArmyLabel
            // 
            resources.ApplyResources(this.standingArmyLabel, "standingArmyLabel");
            this.standingArmyLabel.Name = "standingArmyLabel";
            // 
            // freeMarketTrackBar
            // 
            resources.ApplyResources(this.freeMarketTrackBar, "freeMarketTrackBar");
            this.freeMarketTrackBar.LargeChange = 1;
            this.freeMarketTrackBar.Minimum = 1;
            this.freeMarketTrackBar.Name = "freeMarketTrackBar";
            this.freeMarketTrackBar.Value = 6;
            this.freeMarketTrackBar.Scroll += new System.EventHandler(this.OnPoliticalSliderTrackBarScroll);
            // 
            // centralPlanningLabel
            // 
            resources.ApplyResources(this.centralPlanningLabel, "centralPlanningLabel");
            this.centralPlanningLabel.Name = "centralPlanningLabel";
            // 
            // freeMarketLabel
            // 
            resources.ApplyResources(this.freeMarketLabel, "freeMarketLabel");
            this.freeMarketLabel.Name = "freeMarketLabel";
            // 
            // freedomTrackBar
            // 
            resources.ApplyResources(this.freedomTrackBar, "freedomTrackBar");
            this.freedomTrackBar.LargeChange = 1;
            this.freedomTrackBar.Minimum = 1;
            this.freedomTrackBar.Name = "freedomTrackBar";
            this.freedomTrackBar.Value = 6;
            this.freedomTrackBar.Scroll += new System.EventHandler(this.OnPoliticalSliderTrackBarScroll);
            // 
            // closedSocietyLabel
            // 
            resources.ApplyResources(this.closedSocietyLabel, "closedSocietyLabel");
            this.closedSocietyLabel.Name = "closedSocietyLabel";
            // 
            // openSocietyLabel
            // 
            resources.ApplyResources(this.openSocietyLabel, "openSocietyLabel");
            this.openSocietyLabel.Name = "openSocietyLabel";
            // 
            // politicalLeftTrackBar
            // 
            resources.ApplyResources(this.politicalLeftTrackBar, "politicalLeftTrackBar");
            this.politicalLeftTrackBar.LargeChange = 1;
            this.politicalLeftTrackBar.Minimum = 1;
            this.politicalLeftTrackBar.Name = "politicalLeftTrackBar";
            this.politicalLeftTrackBar.Value = 6;
            this.politicalLeftTrackBar.Scroll += new System.EventHandler(this.OnPoliticalSliderTrackBarScroll);
            // 
            // politicalRightLabel
            // 
            resources.ApplyResources(this.politicalRightLabel, "politicalRightLabel");
            this.politicalRightLabel.Name = "politicalRightLabel";
            // 
            // politicalLeftLabel
            // 
            resources.ApplyResources(this.politicalLeftLabel, "politicalLeftLabel");
            this.politicalLeftLabel.Name = "politicalLeftLabel";
            // 
            // democraticTrackBar
            // 
            resources.ApplyResources(this.democraticTrackBar, "democraticTrackBar");
            this.democraticTrackBar.LargeChange = 1;
            this.democraticTrackBar.Minimum = 1;
            this.democraticTrackBar.Name = "democraticTrackBar";
            this.democraticTrackBar.Value = 6;
            this.democraticTrackBar.Scroll += new System.EventHandler(this.OnPoliticalSliderTrackBarScroll);
            // 
            // authoritarianLabel
            // 
            resources.ApplyResources(this.authoritarianLabel, "authoritarianLabel");
            this.authoritarianLabel.Name = "authoritarianLabel";
            // 
            // democraticLabel
            // 
            resources.ApplyResources(this.democraticLabel, "democraticLabel");
            this.democraticLabel.Name = "democraticLabel";
            // 
            // sliderDayTextBox
            // 
            resources.ApplyResources(this.sliderDayTextBox, "sliderDayTextBox");
            this.sliderDayTextBox.Name = "sliderDayTextBox";
            this.sliderDayTextBox.Validated += new System.EventHandler(this.OnGovernmentIntItemTextBoxValidated);
            // 
            // sliderMonthTextBox
            // 
            resources.ApplyResources(this.sliderMonthTextBox, "sliderMonthTextBox");
            this.sliderMonthTextBox.Name = "sliderMonthTextBox";
            this.sliderMonthTextBox.Validated += new System.EventHandler(this.OnGovernmentIntItemTextBoxValidated);
            // 
            // sliderYearTextBox
            // 
            resources.ApplyResources(this.sliderYearTextBox, "sliderYearTextBox");
            this.sliderYearTextBox.Name = "sliderYearTextBox";
            this.sliderYearTextBox.Validated += new System.EventHandler(this.OnGovernmentIntItemTextBoxValidated);
            // 
            // countryTabPage
            // 
            this.countryTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.countryTabPage.Controls.Add(this.countryModifierGroupBox);
            this.countryTabPage.Controls.Add(this.aiGroupBox);
            this.countryTabPage.Controls.Add(this.countryListBox);
            this.countryTabPage.Controls.Add(this.countryResourceGroupBox);
            this.countryTabPage.Controls.Add(this.countryInfoGroupBox);
            resources.ApplyResources(this.countryTabPage, "countryTabPage");
            this.countryTabPage.Name = "countryTabPage";
            // 
            // countryModifierGroupBox
            // 
            this.countryModifierGroupBox.Controls.Add(this.groundDefEffTextBox);
            this.countryModifierGroupBox.Controls.Add(peacetimeIcModifierLabel);
            this.countryModifierGroupBox.Controls.Add(this.peacetimeIcModifierTextBox);
            this.countryModifierGroupBox.Controls.Add(wartimeIcModifierLabel);
            this.countryModifierGroupBox.Controls.Add(groundDefEffLabel);
            this.countryModifierGroupBox.Controls.Add(relativeManpowerLabel);
            this.countryModifierGroupBox.Controls.Add(this.relativeManpowerTextBox);
            this.countryModifierGroupBox.Controls.Add(this.wartimeIcModifierTextBox);
            this.countryModifierGroupBox.Controls.Add(industrialModifierLabel);
            this.countryModifierGroupBox.Controls.Add(this.industrialModifierTextBox);
            resources.ApplyResources(this.countryModifierGroupBox, "countryModifierGroupBox");
            this.countryModifierGroupBox.Name = "countryModifierGroupBox";
            this.countryModifierGroupBox.TabStop = false;
            // 
            // groundDefEffTextBox
            // 
            resources.ApplyResources(this.groundDefEffTextBox, "groundDefEffTextBox");
            this.groundDefEffTextBox.Name = "groundDefEffTextBox";
            this.groundDefEffTextBox.Validated += new System.EventHandler(this.OnCountryDoubleItemTextBoxValidated);
            // 
            // peacetimeIcModifierTextBox
            // 
            resources.ApplyResources(this.peacetimeIcModifierTextBox, "peacetimeIcModifierTextBox");
            this.peacetimeIcModifierTextBox.Name = "peacetimeIcModifierTextBox";
            this.peacetimeIcModifierTextBox.Validated += new System.EventHandler(this.OnCountryDoubleItemTextBoxValidated);
            // 
            // relativeManpowerTextBox
            // 
            resources.ApplyResources(this.relativeManpowerTextBox, "relativeManpowerTextBox");
            this.relativeManpowerTextBox.Name = "relativeManpowerTextBox";
            this.relativeManpowerTextBox.Validated += new System.EventHandler(this.OnCountryDoubleItemTextBoxValidated);
            // 
            // wartimeIcModifierTextBox
            // 
            resources.ApplyResources(this.wartimeIcModifierTextBox, "wartimeIcModifierTextBox");
            this.wartimeIcModifierTextBox.Name = "wartimeIcModifierTextBox";
            this.wartimeIcModifierTextBox.Validated += new System.EventHandler(this.OnCountryDoubleItemTextBoxValidated);
            // 
            // industrialModifierTextBox
            // 
            resources.ApplyResources(this.industrialModifierTextBox, "industrialModifierTextBox");
            this.industrialModifierTextBox.Name = "industrialModifierTextBox";
            this.industrialModifierTextBox.Validated += new System.EventHandler(this.OnCountryDoubleItemTextBoxValidated);
            // 
            // aiGroupBox
            // 
            this.aiGroupBox.Controls.Add(aiFileLabel);
            this.aiGroupBox.Controls.Add(this.aiFileBrowseButton);
            this.aiGroupBox.Controls.Add(this.aiFileNameTextBox);
            resources.ApplyResources(this.aiGroupBox, "aiGroupBox");
            this.aiGroupBox.Name = "aiGroupBox";
            this.aiGroupBox.TabStop = false;
            // 
            // aiFileBrowseButton
            // 
            resources.ApplyResources(this.aiFileBrowseButton, "aiFileBrowseButton");
            this.aiFileBrowseButton.Name = "aiFileBrowseButton";
            this.aiFileBrowseButton.UseVisualStyleBackColor = true;
            this.aiFileBrowseButton.Click += new System.EventHandler(this.OnAiFileNameBrowseButtonClick);
            // 
            // aiFileNameTextBox
            // 
            resources.ApplyResources(this.aiFileNameTextBox, "aiFileNameTextBox");
            this.aiFileNameTextBox.Name = "aiFileNameTextBox";
            this.aiFileNameTextBox.TextChanged += new System.EventHandler(this.OnCountryStringItemTextBoxTextChanged);
            // 
            // countryListBox
            // 
            this.countryListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            resources.ApplyResources(this.countryListBox, "countryListBox");
            this.countryListBox.FormattingEnabled = true;
            this.countryListBox.Name = "countryListBox";
            this.countryListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnCountryListBoxDrawItem);
            this.countryListBox.SelectedIndexChanged += new System.EventHandler(this.OnCountryListBoxSelectedIndexChanged);
            // 
            // countryResourceGroupBox
            // 
            this.countryResourceGroupBox.Controls.Add(this.offmapIcTextBox);
            this.countryResourceGroupBox.Controls.Add(this.countryIcLabel);
            this.countryResourceGroupBox.Controls.Add(this.offmapManpowerTextBox);
            this.countryResourceGroupBox.Controls.Add(this.offmapEscortsTextBox);
            this.countryResourceGroupBox.Controls.Add(this.offmapTransportsTextBox);
            this.countryResourceGroupBox.Controls.Add(this.offmapMoneyTextBox);
            this.countryResourceGroupBox.Controls.Add(this.offmapSuppliesTextBox);
            this.countryResourceGroupBox.Controls.Add(this.offmapOilTextBox);
            this.countryResourceGroupBox.Controls.Add(this.offmapRareMaterialsTextBox);
            this.countryResourceGroupBox.Controls.Add(this.offmapMetalTextBox);
            this.countryResourceGroupBox.Controls.Add(this.offmapEnergyTextBox);
            this.countryResourceGroupBox.Controls.Add(countryOffmapLabel);
            this.countryResourceGroupBox.Controls.Add(this.countryManpowerTextBox);
            this.countryResourceGroupBox.Controls.Add(countryStockpileLabel);
            this.countryResourceGroupBox.Controls.Add(this.countryManpowerLabel);
            this.countryResourceGroupBox.Controls.Add(this.countryEscortsTextBox);
            this.countryResourceGroupBox.Controls.Add(this.countryEscortsLabel);
            this.countryResourceGroupBox.Controls.Add(this.countryTransportsTextBox);
            this.countryResourceGroupBox.Controls.Add(this.countryTransportsLabel);
            this.countryResourceGroupBox.Controls.Add(this.countryMoneyTextBox);
            this.countryResourceGroupBox.Controls.Add(this.countryMoneyLabel);
            this.countryResourceGroupBox.Controls.Add(this.countrySuppliesTextBox);
            this.countryResourceGroupBox.Controls.Add(this.countrySuppliesLabel);
            this.countryResourceGroupBox.Controls.Add(this.countryOilTextBox);
            this.countryResourceGroupBox.Controls.Add(this.countryOilLabel);
            this.countryResourceGroupBox.Controls.Add(this.countryRareMaterialsTextBox);
            this.countryResourceGroupBox.Controls.Add(this.countryRareMaterialsLabel);
            this.countryResourceGroupBox.Controls.Add(this.countryMetalTextBox);
            this.countryResourceGroupBox.Controls.Add(this.countryMetalLabel);
            this.countryResourceGroupBox.Controls.Add(this.countryEnergyTextBox);
            this.countryResourceGroupBox.Controls.Add(this.countryEnergyLabel);
            resources.ApplyResources(this.countryResourceGroupBox, "countryResourceGroupBox");
            this.countryResourceGroupBox.Name = "countryResourceGroupBox";
            this.countryResourceGroupBox.TabStop = false;
            // 
            // offmapIcTextBox
            // 
            resources.ApplyResources(this.offmapIcTextBox, "offmapIcTextBox");
            this.offmapIcTextBox.Name = "offmapIcTextBox";
            this.offmapIcTextBox.Validated += new System.EventHandler(this.OnCountryDoubleItemTextBoxValidated);
            // 
            // countryIcLabel
            // 
            resources.ApplyResources(this.countryIcLabel, "countryIcLabel");
            this.countryIcLabel.Name = "countryIcLabel";
            // 
            // offmapManpowerTextBox
            // 
            resources.ApplyResources(this.offmapManpowerTextBox, "offmapManpowerTextBox");
            this.offmapManpowerTextBox.Name = "offmapManpowerTextBox";
            this.offmapManpowerTextBox.Validated += new System.EventHandler(this.OnCountryDoubleItemTextBoxValidated);
            // 
            // offmapEscortsTextBox
            // 
            resources.ApplyResources(this.offmapEscortsTextBox, "offmapEscortsTextBox");
            this.offmapEscortsTextBox.Name = "offmapEscortsTextBox";
            this.offmapEscortsTextBox.Validated += new System.EventHandler(this.OnCountryIntItemTextBoxValidated);
            // 
            // offmapTransportsTextBox
            // 
            resources.ApplyResources(this.offmapTransportsTextBox, "offmapTransportsTextBox");
            this.offmapTransportsTextBox.Name = "offmapTransportsTextBox";
            this.offmapTransportsTextBox.Validated += new System.EventHandler(this.OnCountryIntItemTextBoxValidated);
            // 
            // offmapMoneyTextBox
            // 
            resources.ApplyResources(this.offmapMoneyTextBox, "offmapMoneyTextBox");
            this.offmapMoneyTextBox.Name = "offmapMoneyTextBox";
            this.offmapMoneyTextBox.Validated += new System.EventHandler(this.OnCountryDoubleItemTextBoxValidated);
            // 
            // offmapSuppliesTextBox
            // 
            resources.ApplyResources(this.offmapSuppliesTextBox, "offmapSuppliesTextBox");
            this.offmapSuppliesTextBox.Name = "offmapSuppliesTextBox";
            this.offmapSuppliesTextBox.Validated += new System.EventHandler(this.OnCountryDoubleItemTextBoxValidated);
            // 
            // offmapOilTextBox
            // 
            resources.ApplyResources(this.offmapOilTextBox, "offmapOilTextBox");
            this.offmapOilTextBox.Name = "offmapOilTextBox";
            this.offmapOilTextBox.Validated += new System.EventHandler(this.OnCountryDoubleItemTextBoxValidated);
            // 
            // offmapRareMaterialsTextBox
            // 
            resources.ApplyResources(this.offmapRareMaterialsTextBox, "offmapRareMaterialsTextBox");
            this.offmapRareMaterialsTextBox.Name = "offmapRareMaterialsTextBox";
            this.offmapRareMaterialsTextBox.Validated += new System.EventHandler(this.OnCountryDoubleItemTextBoxValidated);
            // 
            // offmapMetalTextBox
            // 
            resources.ApplyResources(this.offmapMetalTextBox, "offmapMetalTextBox");
            this.offmapMetalTextBox.Name = "offmapMetalTextBox";
            this.offmapMetalTextBox.Validated += new System.EventHandler(this.OnCountryDoubleItemTextBoxValidated);
            // 
            // offmapEnergyTextBox
            // 
            resources.ApplyResources(this.offmapEnergyTextBox, "offmapEnergyTextBox");
            this.offmapEnergyTextBox.Name = "offmapEnergyTextBox";
            this.offmapEnergyTextBox.Validated += new System.EventHandler(this.OnCountryDoubleItemTextBoxValidated);
            // 
            // countryManpowerTextBox
            // 
            resources.ApplyResources(this.countryManpowerTextBox, "countryManpowerTextBox");
            this.countryManpowerTextBox.Name = "countryManpowerTextBox";
            this.countryManpowerTextBox.Validated += new System.EventHandler(this.OnCountryDoubleItemTextBoxValidated);
            // 
            // countryManpowerLabel
            // 
            resources.ApplyResources(this.countryManpowerLabel, "countryManpowerLabel");
            this.countryManpowerLabel.Name = "countryManpowerLabel";
            // 
            // countryEscortsTextBox
            // 
            resources.ApplyResources(this.countryEscortsTextBox, "countryEscortsTextBox");
            this.countryEscortsTextBox.Name = "countryEscortsTextBox";
            this.countryEscortsTextBox.Validated += new System.EventHandler(this.OnCountryIntItemTextBoxValidated);
            // 
            // countryEscortsLabel
            // 
            resources.ApplyResources(this.countryEscortsLabel, "countryEscortsLabel");
            this.countryEscortsLabel.Name = "countryEscortsLabel";
            // 
            // countryTransportsTextBox
            // 
            resources.ApplyResources(this.countryTransportsTextBox, "countryTransportsTextBox");
            this.countryTransportsTextBox.Name = "countryTransportsTextBox";
            this.countryTransportsTextBox.Validated += new System.EventHandler(this.OnCountryIntItemTextBoxValidated);
            // 
            // countryTransportsLabel
            // 
            resources.ApplyResources(this.countryTransportsLabel, "countryTransportsLabel");
            this.countryTransportsLabel.Name = "countryTransportsLabel";
            // 
            // countryMoneyTextBox
            // 
            resources.ApplyResources(this.countryMoneyTextBox, "countryMoneyTextBox");
            this.countryMoneyTextBox.Name = "countryMoneyTextBox";
            this.countryMoneyTextBox.Validated += new System.EventHandler(this.OnCountryDoubleItemTextBoxValidated);
            // 
            // countryMoneyLabel
            // 
            resources.ApplyResources(this.countryMoneyLabel, "countryMoneyLabel");
            this.countryMoneyLabel.Name = "countryMoneyLabel";
            // 
            // countrySuppliesTextBox
            // 
            resources.ApplyResources(this.countrySuppliesTextBox, "countrySuppliesTextBox");
            this.countrySuppliesTextBox.Name = "countrySuppliesTextBox";
            this.countrySuppliesTextBox.Validated += new System.EventHandler(this.OnCountryDoubleItemTextBoxValidated);
            // 
            // countrySuppliesLabel
            // 
            resources.ApplyResources(this.countrySuppliesLabel, "countrySuppliesLabel");
            this.countrySuppliesLabel.Name = "countrySuppliesLabel";
            // 
            // countryOilTextBox
            // 
            resources.ApplyResources(this.countryOilTextBox, "countryOilTextBox");
            this.countryOilTextBox.Name = "countryOilTextBox";
            this.countryOilTextBox.Validated += new System.EventHandler(this.OnCountryDoubleItemTextBoxValidated);
            // 
            // countryOilLabel
            // 
            resources.ApplyResources(this.countryOilLabel, "countryOilLabel");
            this.countryOilLabel.Name = "countryOilLabel";
            // 
            // countryRareMaterialsTextBox
            // 
            resources.ApplyResources(this.countryRareMaterialsTextBox, "countryRareMaterialsTextBox");
            this.countryRareMaterialsTextBox.Name = "countryRareMaterialsTextBox";
            this.countryRareMaterialsTextBox.Validated += new System.EventHandler(this.OnCountryDoubleItemTextBoxValidated);
            // 
            // countryRareMaterialsLabel
            // 
            resources.ApplyResources(this.countryRareMaterialsLabel, "countryRareMaterialsLabel");
            this.countryRareMaterialsLabel.Name = "countryRareMaterialsLabel";
            // 
            // countryMetalTextBox
            // 
            resources.ApplyResources(this.countryMetalTextBox, "countryMetalTextBox");
            this.countryMetalTextBox.Name = "countryMetalTextBox";
            this.countryMetalTextBox.Validated += new System.EventHandler(this.OnCountryDoubleItemTextBoxValidated);
            // 
            // countryMetalLabel
            // 
            resources.ApplyResources(this.countryMetalLabel, "countryMetalLabel");
            this.countryMetalLabel.Name = "countryMetalLabel";
            // 
            // countryEnergyTextBox
            // 
            resources.ApplyResources(this.countryEnergyTextBox, "countryEnergyTextBox");
            this.countryEnergyTextBox.Name = "countryEnergyTextBox";
            this.countryEnergyTextBox.Validated += new System.EventHandler(this.OnCountryDoubleItemTextBoxValidated);
            // 
            // countryEnergyLabel
            // 
            resources.ApplyResources(this.countryEnergyLabel, "countryEnergyLabel");
            this.countryEnergyLabel.Name = "countryEnergyLabel";
            // 
            // countryInfoGroupBox
            // 
            this.countryInfoGroupBox.Controls.Add(this.nukeDayTextBox);
            this.countryInfoGroupBox.Controls.Add(this.nukeMonthTextBox);
            this.countryInfoGroupBox.Controls.Add(nukeDateLabel);
            this.countryInfoGroupBox.Controls.Add(this.nukeYearTextBox);
            this.countryInfoGroupBox.Controls.Add(this.dissentTextBox);
            this.countryInfoGroupBox.Controls.Add(dissentLabel);
            this.countryInfoGroupBox.Controls.Add(this.flagExtTextBox);
            this.countryInfoGroupBox.Controls.Add(flagExtLabel);
            this.countryInfoGroupBox.Controls.Add(this.nukeTextBox);
            this.countryInfoGroupBox.Controls.Add(nukeLabel);
            this.countryInfoGroupBox.Controls.Add(this.extraTcTextBox);
            this.countryInfoGroupBox.Controls.Add(extraTcLabel);
            this.countryInfoGroupBox.Controls.Add(this.belligerenceTextBox);
            this.countryInfoGroupBox.Controls.Add(belligerenceLabel);
            this.countryInfoGroupBox.Controls.Add(this.regularIdComboBox);
            this.countryInfoGroupBox.Controls.Add(regularIdLabel);
            this.countryInfoGroupBox.Controls.Add(this.countryNameTextBox);
            this.countryInfoGroupBox.Controls.Add(countryNameLabel);
            resources.ApplyResources(this.countryInfoGroupBox, "countryInfoGroupBox");
            this.countryInfoGroupBox.Name = "countryInfoGroupBox";
            this.countryInfoGroupBox.TabStop = false;
            // 
            // nukeDayTextBox
            // 
            resources.ApplyResources(this.nukeDayTextBox, "nukeDayTextBox");
            this.nukeDayTextBox.Name = "nukeDayTextBox";
            this.nukeDayTextBox.Validated += new System.EventHandler(this.OnCountryIntItemTextBoxValidated);
            // 
            // nukeMonthTextBox
            // 
            resources.ApplyResources(this.nukeMonthTextBox, "nukeMonthTextBox");
            this.nukeMonthTextBox.Name = "nukeMonthTextBox";
            this.nukeMonthTextBox.Validated += new System.EventHandler(this.OnCountryIntItemTextBoxValidated);
            // 
            // nukeYearTextBox
            // 
            resources.ApplyResources(this.nukeYearTextBox, "nukeYearTextBox");
            this.nukeYearTextBox.Name = "nukeYearTextBox";
            this.nukeYearTextBox.Validated += new System.EventHandler(this.OnCountryIntItemTextBoxValidated);
            // 
            // dissentTextBox
            // 
            resources.ApplyResources(this.dissentTextBox, "dissentTextBox");
            this.dissentTextBox.Name = "dissentTextBox";
            this.dissentTextBox.Validated += new System.EventHandler(this.OnCountryDoubleItemTextBoxValidated);
            // 
            // flagExtTextBox
            // 
            resources.ApplyResources(this.flagExtTextBox, "flagExtTextBox");
            this.flagExtTextBox.Name = "flagExtTextBox";
            this.flagExtTextBox.TextChanged += new System.EventHandler(this.OnCountryStringItemTextBoxTextChanged);
            // 
            // nukeTextBox
            // 
            resources.ApplyResources(this.nukeTextBox, "nukeTextBox");
            this.nukeTextBox.Name = "nukeTextBox";
            this.nukeTextBox.Validated += new System.EventHandler(this.OnCountryIntItemTextBoxValidated);
            // 
            // extraTcTextBox
            // 
            resources.ApplyResources(this.extraTcTextBox, "extraTcTextBox");
            this.extraTcTextBox.Name = "extraTcTextBox";
            this.extraTcTextBox.Validated += new System.EventHandler(this.OnCountryDoubleItemTextBoxValidated);
            // 
            // belligerenceTextBox
            // 
            resources.ApplyResources(this.belligerenceTextBox, "belligerenceTextBox");
            this.belligerenceTextBox.Name = "belligerenceTextBox";
            this.belligerenceTextBox.Validated += new System.EventHandler(this.OnCountryIntItemTextBoxValidated);
            // 
            // regularIdComboBox
            // 
            this.regularIdComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.regularIdComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.regularIdComboBox, "regularIdComboBox");
            this.regularIdComboBox.FormattingEnabled = true;
            this.regularIdComboBox.Name = "regularIdComboBox";
            this.regularIdComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnCountryItemComboBoxDrawItem);
            this.regularIdComboBox.SelectedIndexChanged += new System.EventHandler(this.OnCountryItemComboBoxSelectedIndexChanged);
            // 
            // countryNameTextBox
            // 
            resources.ApplyResources(this.countryNameTextBox, "countryNameTextBox");
            this.countryNameTextBox.Name = "countryNameTextBox";
            this.countryNameTextBox.TextChanged += new System.EventHandler(this.OnCountryNameItemTextBoxTextChanged);
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
            resources.ApplyResources(this.tradeDealsGroupBox, "tradeDealsGroupBox");
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
            resources.ApplyResources(this.tradeInfoGroupBox, "tradeInfoGroupBox");
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
            this.tradeListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.tradeCountryColumnHeader1,
            this.tradeCountryColumnHeader2,
            this.tradeDealsColumnHeader});
            resources.ApplyResources(this.tradeListView, "tradeListView");
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
            this.relationTabPage.Controls.Add(this.intelligenceGroupBox);
            this.relationTabPage.Controls.Add(this.diplomacyGroupBox);
            this.relationTabPage.Controls.Add(this.relationListView);
            this.relationTabPage.Controls.Add(this.relationCountryListBox);
            resources.ApplyResources(this.relationTabPage, "relationTabPage");
            this.relationTabPage.Name = "relationTabPage";
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
            this.spyNumNumericUpDown.ValueChanged += new System.EventHandler(this.OnSpyNumNumericUpDownValueChanged);
            // 
            // spyNumLabel
            // 
            resources.ApplyResources(this.spyNumLabel, "spyNumLabel");
            this.spyNumLabel.Name = "spyNumLabel";
            // 
            // diplomacyGroupBox
            // 
            this.diplomacyGroupBox.Controls.Add(this.guaranteeGroupBox);
            this.diplomacyGroupBox.Controls.Add(this.peaceGroupBox);
            this.diplomacyGroupBox.Controls.Add(this.relationValueNumericUpDown);
            this.diplomacyGroupBox.Controls.Add(this.nonAggressionGroupBox);
            this.diplomacyGroupBox.Controls.Add(this.relationValueLabel);
            this.diplomacyGroupBox.Controls.Add(this.accessCheckBox);
            this.diplomacyGroupBox.Controls.Add(this.masterCheckBox);
            this.diplomacyGroupBox.Controls.Add(this.controlCheckBox);
            resources.ApplyResources(this.diplomacyGroupBox, "diplomacyGroupBox");
            this.diplomacyGroupBox.Name = "diplomacyGroupBox";
            this.diplomacyGroupBox.TabStop = false;
            // 
            // guaranteeGroupBox
            // 
            this.guaranteeGroupBox.Controls.Add(this.guaranteeCheckBox);
            this.guaranteeGroupBox.Controls.Add(this.guaranteeYearTextBox);
            this.guaranteeGroupBox.Controls.Add(this.guaranteeMonthTextBox);
            this.guaranteeGroupBox.Controls.Add(this.guaranteeEndLabel);
            this.guaranteeGroupBox.Controls.Add(this.guaranteeDayTextBox);
            resources.ApplyResources(this.guaranteeGroupBox, "guaranteeGroupBox");
            this.guaranteeGroupBox.Name = "guaranteeGroupBox";
            this.guaranteeGroupBox.TabStop = false;
            // 
            // guaranteeCheckBox
            // 
            resources.ApplyResources(this.guaranteeCheckBox, "guaranteeCheckBox");
            this.guaranteeCheckBox.Name = "guaranteeCheckBox";
            this.guaranteeCheckBox.UseVisualStyleBackColor = true;
            this.guaranteeCheckBox.CheckedChanged += new System.EventHandler(this.OnGuaranteeCheckBoxCheckedChanged);
            // 
            // guaranteeYearTextBox
            // 
            resources.ApplyResources(this.guaranteeYearTextBox, "guaranteeYearTextBox");
            this.guaranteeYearTextBox.Name = "guaranteeYearTextBox";
            this.guaranteeYearTextBox.Validated += new System.EventHandler(this.OnGuaranteeYearTextBoxValidated);
            // 
            // guaranteeMonthTextBox
            // 
            resources.ApplyResources(this.guaranteeMonthTextBox, "guaranteeMonthTextBox");
            this.guaranteeMonthTextBox.Name = "guaranteeMonthTextBox";
            this.guaranteeMonthTextBox.Validated += new System.EventHandler(this.OnGuaranteeMonthTextBoxValidated);
            // 
            // guaranteeEndLabel
            // 
            resources.ApplyResources(this.guaranteeEndLabel, "guaranteeEndLabel");
            this.guaranteeEndLabel.Name = "guaranteeEndLabel";
            // 
            // guaranteeDayTextBox
            // 
            resources.ApplyResources(this.guaranteeDayTextBox, "guaranteeDayTextBox");
            this.guaranteeDayTextBox.Name = "guaranteeDayTextBox";
            this.guaranteeDayTextBox.Validated += new System.EventHandler(this.OnGuaranteeDayTextBoxValidated);
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
            this.peaceIdTextBox.Validated += new System.EventHandler(this.OnPeaceIdTextBoxValidated);
            // 
            // peaceTypeTextBox
            // 
            resources.ApplyResources(this.peaceTypeTextBox, "peaceTypeTextBox");
            this.peaceTypeTextBox.Name = "peaceTypeTextBox";
            this.peaceTypeTextBox.Validated += new System.EventHandler(this.OnPeaceTypeTextBoxValidated);
            // 
            // peaceEndDayTextBox
            // 
            resources.ApplyResources(this.peaceEndDayTextBox, "peaceEndDayTextBox");
            this.peaceEndDayTextBox.Name = "peaceEndDayTextBox";
            this.peaceEndDayTextBox.Validated += new System.EventHandler(this.OnPeaceEndDayTextBoxValidated);
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
            this.peaceEndMonthTextBox.Validated += new System.EventHandler(this.OnPeaceEndMonthTextBoxValidated);
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
            this.peaceEndYearTextBox.Validated += new System.EventHandler(this.OnPeaceEndYearTextBoxValidated);
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
            this.peaceStartYearTextBox.Validated += new System.EventHandler(this.OnPeaceStartYearTextBoxValidated);
            // 
            // peaceStartDayTextBox
            // 
            resources.ApplyResources(this.peaceStartDayTextBox, "peaceStartDayTextBox");
            this.peaceStartDayTextBox.Name = "peaceStartDayTextBox";
            this.peaceStartDayTextBox.Validated += new System.EventHandler(this.OnPeaceStartDayTextBoxValidated);
            // 
            // peaceCheckBox
            // 
            resources.ApplyResources(this.peaceCheckBox, "peaceCheckBox");
            this.peaceCheckBox.Name = "peaceCheckBox";
            this.peaceCheckBox.UseVisualStyleBackColor = true;
            this.peaceCheckBox.CheckedChanged += new System.EventHandler(this.OnPeaceCheckBoxCheckedChanged);
            // 
            // peaceStartMonthTextBox
            // 
            resources.ApplyResources(this.peaceStartMonthTextBox, "peaceStartMonthTextBox");
            this.peaceStartMonthTextBox.Name = "peaceStartMonthTextBox";
            this.peaceStartMonthTextBox.Validated += new System.EventHandler(this.OnPeaceStartMonthTextBoxValidated);
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
            this.relationValueNumericUpDown.ValueChanged += new System.EventHandler(this.OnRelationValueNumericUpDownValueChanged);
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
            // nonAggressionIdTextBox
            // 
            resources.ApplyResources(this.nonAggressionIdTextBox, "nonAggressionIdTextBox");
            this.nonAggressionIdTextBox.Name = "nonAggressionIdTextBox";
            this.nonAggressionIdTextBox.Validated += new System.EventHandler(this.OnNonAggressionIdTextBoxValidated);
            // 
            // nonAggressionTypeTextBox
            // 
            resources.ApplyResources(this.nonAggressionTypeTextBox, "nonAggressionTypeTextBox");
            this.nonAggressionTypeTextBox.Name = "nonAggressionTypeTextBox";
            this.nonAggressionTypeTextBox.Validated += new System.EventHandler(this.OnNonAggressionTypeTextBoxValidated);
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
            this.nonAggressionCheckBox.CheckedChanged += new System.EventHandler(this.OnNonAggressionCheckBoxCheckedChanged);
            // 
            // nonAggressionStartMonthTextBox
            // 
            resources.ApplyResources(this.nonAggressionStartMonthTextBox, "nonAggressionStartMonthTextBox");
            this.nonAggressionStartMonthTextBox.Name = "nonAggressionStartMonthTextBox";
            this.nonAggressionStartMonthTextBox.Validated += new System.EventHandler(this.OnNonAggressionStartMonthTextBoxValidated);
            // 
            // nonAggressionStartYearTextBox
            // 
            resources.ApplyResources(this.nonAggressionStartYearTextBox, "nonAggressionStartYearTextBox");
            this.nonAggressionStartYearTextBox.Name = "nonAggressionStartYearTextBox";
            this.nonAggressionStartYearTextBox.Validated += new System.EventHandler(this.OnNonAggressionStartYearTextBoxValidated);
            // 
            // nonAggressionStartDayTextBox
            // 
            resources.ApplyResources(this.nonAggressionStartDayTextBox, "nonAggressionStartDayTextBox");
            this.nonAggressionStartDayTextBox.Name = "nonAggressionStartDayTextBox";
            this.nonAggressionStartDayTextBox.Validated += new System.EventHandler(this.OnNonAggressionStartDayTextBoxValidated);
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
            this.nonAggressionEndYearTextBox.Validated += new System.EventHandler(this.OnNonAggressionEndYearTextBoxValidated);
            // 
            // nonAggressionEndMonthTextBox
            // 
            resources.ApplyResources(this.nonAggressionEndMonthTextBox, "nonAggressionEndMonthTextBox");
            this.nonAggressionEndMonthTextBox.Name = "nonAggressionEndMonthTextBox";
            this.nonAggressionEndMonthTextBox.Validated += new System.EventHandler(this.OnNonAggressionEndMonthTextBoxValidated);
            // 
            // nonAggressionEndDayTextBox
            // 
            resources.ApplyResources(this.nonAggressionEndDayTextBox, "nonAggressionEndDayTextBox");
            this.nonAggressionEndDayTextBox.Name = "nonAggressionEndDayTextBox";
            this.nonAggressionEndDayTextBox.Validated += new System.EventHandler(this.OnNonAggressionEndDayTextBoxValidated);
            // 
            // nonAggressionEndLabel
            // 
            resources.ApplyResources(this.nonAggressionEndLabel, "nonAggressionEndLabel");
            this.nonAggressionEndLabel.Name = "nonAggressionEndLabel";
            // 
            // relationValueLabel
            // 
            resources.ApplyResources(this.relationValueLabel, "relationValueLabel");
            this.relationValueLabel.Name = "relationValueLabel";
            // 
            // accessCheckBox
            // 
            resources.ApplyResources(this.accessCheckBox, "accessCheckBox");
            this.accessCheckBox.Name = "accessCheckBox";
            this.accessCheckBox.UseVisualStyleBackColor = true;
            this.accessCheckBox.CheckedChanged += new System.EventHandler(this.OnAccessCheckBoxCheckedChanged);
            // 
            // masterCheckBox
            // 
            resources.ApplyResources(this.masterCheckBox, "masterCheckBox");
            this.masterCheckBox.Name = "masterCheckBox";
            this.masterCheckBox.UseVisualStyleBackColor = true;
            this.masterCheckBox.CheckedChanged += new System.EventHandler(this.OnMasterCheckBoxCheckedChanged);
            // 
            // controlCheckBox
            // 
            resources.ApplyResources(this.controlCheckBox, "controlCheckBox");
            this.controlCheckBox.Name = "controlCheckBox";
            this.controlCheckBox.UseVisualStyleBackColor = true;
            this.controlCheckBox.CheckedChanged += new System.EventHandler(this.OnControlCheckBoxCheckedChanged);
            // 
            // relationListView
            // 
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
            resources.ApplyResources(this.relationListView, "relationListView");
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
            this.relationCountryListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            resources.ApplyResources(this.relationCountryListBox, "relationCountryListBox");
            this.relationCountryListBox.FormattingEnabled = true;
            this.relationCountryListBox.Name = "relationCountryListBox";
            this.relationCountryListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnCountryListBoxDrawItem);
            this.relationCountryListBox.SelectedIndexChanged += new System.EventHandler(this.OnRelationCountryListBoxSelectedIndexChanged);
            // 
            // allianceTabPage
            // 
            resources.ApplyResources(this.allianceTabPage, "allianceTabPage");
            this.allianceTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.allianceTabPage.Controls.Add(this.warGroupBox);
            this.allianceTabPage.Controls.Add(this.allianceGroupBox);
            this.allianceTabPage.Name = "allianceTabPage";
            // 
            // warGroupBox
            // 
            this.warGroupBox.Controls.Add(this.warDefenderLeaderButton);
            this.warGroupBox.Controls.Add(this.warAttackerLeaderButton);
            this.warGroupBox.Controls.Add(this.warDefenderIdLabel);
            this.warGroupBox.Controls.Add(this.warAttackerIdLabel);
            this.warGroupBox.Controls.Add(this.warDefenderIdTextBox);
            this.warGroupBox.Controls.Add(this.warDefenderTypeTextBox);
            this.warGroupBox.Controls.Add(this.warAttackerIdTextBox);
            this.warGroupBox.Controls.Add(this.warAttackerTypeTextBox);
            this.warGroupBox.Controls.Add(this.warIdTextBox);
            this.warGroupBox.Controls.Add(this.warTypeTextBox);
            this.warGroupBox.Controls.Add(this.warEndMonthTextBox);
            this.warGroupBox.Controls.Add(this.warIdLabel);
            this.warGroupBox.Controls.Add(this.warEndYearTextBox);
            this.warGroupBox.Controls.Add(this.warEndDayTextBox);
            this.warGroupBox.Controls.Add(this.warEndDateLabel);
            this.warGroupBox.Controls.Add(this.warStartMonthTextBox);
            this.warGroupBox.Controls.Add(this.warStartYearTextBox);
            this.warGroupBox.Controls.Add(this.warStartDayTextBox);
            this.warGroupBox.Controls.Add(this.warStartDateLabel);
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
            // warDefenderLeaderButton
            // 
            resources.ApplyResources(this.warDefenderLeaderButton, "warDefenderLeaderButton");
            this.warDefenderLeaderButton.Name = "warDefenderLeaderButton";
            this.warDefenderLeaderButton.UseVisualStyleBackColor = true;
            this.warDefenderLeaderButton.Click += new System.EventHandler(this.OnWarDefenderLeaderButtonClick);
            // 
            // warAttackerLeaderButton
            // 
            resources.ApplyResources(this.warAttackerLeaderButton, "warAttackerLeaderButton");
            this.warAttackerLeaderButton.Name = "warAttackerLeaderButton";
            this.warAttackerLeaderButton.UseVisualStyleBackColor = true;
            this.warAttackerLeaderButton.Click += new System.EventHandler(this.OnWarAttackerLeaderButtonClick);
            // 
            // warDefenderIdLabel
            // 
            resources.ApplyResources(this.warDefenderIdLabel, "warDefenderIdLabel");
            this.warDefenderIdLabel.Name = "warDefenderIdLabel";
            // 
            // warAttackerIdLabel
            // 
            resources.ApplyResources(this.warAttackerIdLabel, "warAttackerIdLabel");
            this.warAttackerIdLabel.Name = "warAttackerIdLabel";
            // 
            // warDefenderIdTextBox
            // 
            resources.ApplyResources(this.warDefenderIdTextBox, "warDefenderIdTextBox");
            this.warDefenderIdTextBox.Name = "warDefenderIdTextBox";
            this.warDefenderIdTextBox.Validated += new System.EventHandler(this.OnWarDefenderIdTextBoxValidated);
            // 
            // warDefenderTypeTextBox
            // 
            resources.ApplyResources(this.warDefenderTypeTextBox, "warDefenderTypeTextBox");
            this.warDefenderTypeTextBox.Name = "warDefenderTypeTextBox";
            this.warDefenderTypeTextBox.Validated += new System.EventHandler(this.OnWarDefenderTypeTextBoxValidated);
            // 
            // warAttackerIdTextBox
            // 
            resources.ApplyResources(this.warAttackerIdTextBox, "warAttackerIdTextBox");
            this.warAttackerIdTextBox.Name = "warAttackerIdTextBox";
            this.warAttackerIdTextBox.Validated += new System.EventHandler(this.OnWarAttackerIdTextBoxValidated);
            // 
            // warAttackerTypeTextBox
            // 
            resources.ApplyResources(this.warAttackerTypeTextBox, "warAttackerTypeTextBox");
            this.warAttackerTypeTextBox.Name = "warAttackerTypeTextBox";
            this.warAttackerTypeTextBox.Validated += new System.EventHandler(this.OnWarAttackerTypeTextBoxValidated);
            // 
            // warIdTextBox
            // 
            resources.ApplyResources(this.warIdTextBox, "warIdTextBox");
            this.warIdTextBox.Name = "warIdTextBox";
            this.warIdTextBox.Validated += new System.EventHandler(this.OnWarIdTextBoxValidated);
            // 
            // warTypeTextBox
            // 
            resources.ApplyResources(this.warTypeTextBox, "warTypeTextBox");
            this.warTypeTextBox.Name = "warTypeTextBox";
            this.warTypeTextBox.Validated += new System.EventHandler(this.OnWarTypeTextBoxValidated);
            // 
            // warEndMonthTextBox
            // 
            resources.ApplyResources(this.warEndMonthTextBox, "warEndMonthTextBox");
            this.warEndMonthTextBox.Name = "warEndMonthTextBox";
            this.warEndMonthTextBox.Validated += new System.EventHandler(this.OnWarEndMonthTextBoxValidated);
            // 
            // warIdLabel
            // 
            resources.ApplyResources(this.warIdLabel, "warIdLabel");
            this.warIdLabel.Name = "warIdLabel";
            // 
            // warEndYearTextBox
            // 
            resources.ApplyResources(this.warEndYearTextBox, "warEndYearTextBox");
            this.warEndYearTextBox.Name = "warEndYearTextBox";
            this.warEndYearTextBox.Validated += new System.EventHandler(this.OnWarEndYearTextBoxValidated);
            // 
            // warEndDayTextBox
            // 
            resources.ApplyResources(this.warEndDayTextBox, "warEndDayTextBox");
            this.warEndDayTextBox.Name = "warEndDayTextBox";
            this.warEndDayTextBox.Validated += new System.EventHandler(this.OnWarEndDayTextBoxValidated);
            // 
            // warEndDateLabel
            // 
            resources.ApplyResources(this.warEndDateLabel, "warEndDateLabel");
            this.warEndDateLabel.Name = "warEndDateLabel";
            // 
            // warStartMonthTextBox
            // 
            resources.ApplyResources(this.warStartMonthTextBox, "warStartMonthTextBox");
            this.warStartMonthTextBox.Name = "warStartMonthTextBox";
            this.warStartMonthTextBox.Validated += new System.EventHandler(this.OnWarStartMonthTextBoxValidated);
            // 
            // warStartYearTextBox
            // 
            resources.ApplyResources(this.warStartYearTextBox, "warStartYearTextBox");
            this.warStartYearTextBox.Name = "warStartYearTextBox";
            this.warStartYearTextBox.Validated += new System.EventHandler(this.OnWarStartYearTextBoxValidated);
            // 
            // warStartDayTextBox
            // 
            resources.ApplyResources(this.warStartDayTextBox, "warStartDayTextBox");
            this.warStartDayTextBox.Name = "warStartDayTextBox";
            this.warStartDayTextBox.Validated += new System.EventHandler(this.OnWarStartDayTextBoxValidated);
            // 
            // warStartDateLabel
            // 
            resources.ApplyResources(this.warStartDateLabel, "warStartDateLabel");
            this.warStartDateLabel.Name = "warStartDateLabel";
            // 
            // warDefenderLabel
            // 
            resources.ApplyResources(this.warDefenderLabel, "warDefenderLabel");
            this.warDefenderLabel.Name = "warDefenderLabel";
            // 
            // warListView
            // 
            this.warListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.warAttackerColumnHeader,
            this.warDefenderColumnHeader});
            this.warListView.FullRowSelect = true;
            this.warListView.GridLines = true;
            this.warListView.HideSelection = false;
            resources.ApplyResources(this.warListView, "warListView");
            this.warListView.Name = "warListView";
            this.warListView.UseCompatibleStateImageBehavior = false;
            this.warListView.View = System.Windows.Forms.View.Details;
            this.warListView.SelectedIndexChanged += new System.EventHandler(this.OnWarListViewSelectedIndexChanged);
            // 
            // warAttackerColumnHeader
            // 
            resources.ApplyResources(this.warAttackerColumnHeader, "warAttackerColumnHeader");
            // 
            // warDefenderColumnHeader
            // 
            resources.ApplyResources(this.warDefenderColumnHeader, "warDefenderColumnHeader");
            // 
            // warNewButton
            // 
            resources.ApplyResources(this.warNewButton, "warNewButton");
            this.warNewButton.Name = "warNewButton";
            this.warNewButton.UseVisualStyleBackColor = true;
            this.warNewButton.Click += new System.EventHandler(this.OnWarNewButtonClick);
            // 
            // warAttackerRemoveButton
            // 
            resources.ApplyResources(this.warAttackerRemoveButton, "warAttackerRemoveButton");
            this.warAttackerRemoveButton.Name = "warAttackerRemoveButton";
            this.warAttackerRemoveButton.UseVisualStyleBackColor = true;
            this.warAttackerRemoveButton.Click += new System.EventHandler(this.OnWarAttackerRemoveButtonClick);
            // 
            // warUpButton
            // 
            resources.ApplyResources(this.warUpButton, "warUpButton");
            this.warUpButton.Name = "warUpButton";
            this.warUpButton.UseVisualStyleBackColor = true;
            this.warUpButton.Click += new System.EventHandler(this.OnWarUpButtonClick);
            // 
            // warAttackerListBox
            // 
            this.warAttackerListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            resources.ApplyResources(this.warAttackerListBox, "warAttackerListBox");
            this.warAttackerListBox.FormattingEnabled = true;
            this.warAttackerListBox.Name = "warAttackerListBox";
            this.warAttackerListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.warAttackerListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnWarAttackerListBoxDrawItem);
            this.warAttackerListBox.SelectedIndexChanged += new System.EventHandler(this.OnWarAttackerListBoxSelectedIndexChanged);
            // 
            // warDefenderRemoveButton
            // 
            resources.ApplyResources(this.warDefenderRemoveButton, "warDefenderRemoveButton");
            this.warDefenderRemoveButton.Name = "warDefenderRemoveButton";
            this.warDefenderRemoveButton.UseVisualStyleBackColor = true;
            this.warDefenderRemoveButton.Click += new System.EventHandler(this.OnWarDefenderRemoveButtonClick);
            // 
            // warAttackerLabel
            // 
            resources.ApplyResources(this.warAttackerLabel, "warAttackerLabel");
            this.warAttackerLabel.Name = "warAttackerLabel";
            // 
            // warDefenderListBox
            // 
            this.warDefenderListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            resources.ApplyResources(this.warDefenderListBox, "warDefenderListBox");
            this.warDefenderListBox.FormattingEnabled = true;
            this.warDefenderListBox.Name = "warDefenderListBox";
            this.warDefenderListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.warDefenderListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnWarDefenderListBoxDrawItem);
            this.warDefenderListBox.SelectedIndexChanged += new System.EventHandler(this.OnWarDefenderListBoxSelectedIndexChanged);
            // 
            // warDownButton
            // 
            resources.ApplyResources(this.warDownButton, "warDownButton");
            this.warDownButton.Name = "warDownButton";
            this.warDownButton.UseVisualStyleBackColor = true;
            this.warDownButton.Click += new System.EventHandler(this.OnWarDownButtonClick);
            // 
            // warCountryListBox
            // 
            this.warCountryListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            resources.ApplyResources(this.warCountryListBox, "warCountryListBox");
            this.warCountryListBox.FormattingEnabled = true;
            this.warCountryListBox.Name = "warCountryListBox";
            this.warCountryListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.warCountryListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnWarCountryListBoxDrawItem);
            this.warCountryListBox.SelectedIndexChanged += new System.EventHandler(this.OnWarCountryListBoxSelectedIndexChanged);
            // 
            // warAttackerAddButton
            // 
            resources.ApplyResources(this.warAttackerAddButton, "warAttackerAddButton");
            this.warAttackerAddButton.Name = "warAttackerAddButton";
            this.warAttackerAddButton.UseVisualStyleBackColor = true;
            this.warAttackerAddButton.Click += new System.EventHandler(this.OnWarAttackerAddButtonClick);
            // 
            // warDefenderAddButton
            // 
            resources.ApplyResources(this.warDefenderAddButton, "warDefenderAddButton");
            this.warDefenderAddButton.Name = "warDefenderAddButton";
            this.warDefenderAddButton.UseVisualStyleBackColor = true;
            this.warDefenderAddButton.Click += new System.EventHandler(this.OnWarDefenderAddButtonClick);
            // 
            // warRemoveButton
            // 
            resources.ApplyResources(this.warRemoveButton, "warRemoveButton");
            this.warRemoveButton.Name = "warRemoveButton";
            this.warRemoveButton.UseVisualStyleBackColor = true;
            this.warRemoveButton.Click += new System.EventHandler(this.OnWarRemoveButtonClick);
            // 
            // allianceGroupBox
            // 
            this.allianceGroupBox.Controls.Add(this.allianceIdTextBox);
            this.allianceGroupBox.Controls.Add(this.allianceTypeTextBox);
            this.allianceGroupBox.Controls.Add(this.allianceIdLabel);
            this.allianceGroupBox.Controls.Add(this.allianceNameTextBox);
            this.allianceGroupBox.Controls.Add(this.allianceNameLabel);
            this.allianceGroupBox.Controls.Add(this.allianceListView);
            this.allianceGroupBox.Controls.Add(this.allianceParticipantLabel);
            this.allianceGroupBox.Controls.Add(this.allianceLeaderButton);
            this.allianceGroupBox.Controls.Add(this.allianceParticipantListBox);
            this.allianceGroupBox.Controls.Add(this.allianceUpButton);
            this.allianceGroupBox.Controls.Add(this.allianceParticipantAddButton);
            this.allianceGroupBox.Controls.Add(this.allianceCountryListBox);
            this.allianceGroupBox.Controls.Add(this.allianceRemoveButton);
            this.allianceGroupBox.Controls.Add(this.allianceNewButton);
            this.allianceGroupBox.Controls.Add(this.allianceParticipantRemoveButton);
            this.allianceGroupBox.Controls.Add(this.allianceDownButton);
            resources.ApplyResources(this.allianceGroupBox, "allianceGroupBox");
            this.allianceGroupBox.Name = "allianceGroupBox";
            this.allianceGroupBox.TabStop = false;
            // 
            // allianceIdTextBox
            // 
            resources.ApplyResources(this.allianceIdTextBox, "allianceIdTextBox");
            this.allianceIdTextBox.Name = "allianceIdTextBox";
            this.allianceIdTextBox.Validated += new System.EventHandler(this.OnAllianceIdTextBoxValidated);
            // 
            // allianceTypeTextBox
            // 
            resources.ApplyResources(this.allianceTypeTextBox, "allianceTypeTextBox");
            this.allianceTypeTextBox.Name = "allianceTypeTextBox";
            this.allianceTypeTextBox.Validated += new System.EventHandler(this.OnAllianceTypeTextBoxValidated);
            // 
            // allianceIdLabel
            // 
            resources.ApplyResources(this.allianceIdLabel, "allianceIdLabel");
            this.allianceIdLabel.Name = "allianceIdLabel";
            // 
            // allianceNameTextBox
            // 
            resources.ApplyResources(this.allianceNameTextBox, "allianceNameTextBox");
            this.allianceNameTextBox.Name = "allianceNameTextBox";
            this.allianceNameTextBox.TextChanged += new System.EventHandler(this.OnAllianceNameTextBoxTextChanged);
            // 
            // allianceNameLabel
            // 
            resources.ApplyResources(this.allianceNameLabel, "allianceNameLabel");
            this.allianceNameLabel.Name = "allianceNameLabel";
            // 
            // allianceListView
            // 
            this.allianceListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.allianceNameColumnHeader,
            this.allianceParticipantColumnHeader});
            this.allianceListView.FullRowSelect = true;
            this.allianceListView.GridLines = true;
            this.allianceListView.HideSelection = false;
            resources.ApplyResources(this.allianceListView, "allianceListView");
            this.allianceListView.Name = "allianceListView";
            this.allianceListView.UseCompatibleStateImageBehavior = false;
            this.allianceListView.View = System.Windows.Forms.View.Details;
            this.allianceListView.SelectedIndexChanged += new System.EventHandler(this.OnAllianceListViewSelectedIndexChanged);
            // 
            // allianceNameColumnHeader
            // 
            resources.ApplyResources(this.allianceNameColumnHeader, "allianceNameColumnHeader");
            // 
            // allianceParticipantColumnHeader
            // 
            resources.ApplyResources(this.allianceParticipantColumnHeader, "allianceParticipantColumnHeader");
            // 
            // allianceParticipantLabel
            // 
            resources.ApplyResources(this.allianceParticipantLabel, "allianceParticipantLabel");
            this.allianceParticipantLabel.Name = "allianceParticipantLabel";
            // 
            // allianceLeaderButton
            // 
            resources.ApplyResources(this.allianceLeaderButton, "allianceLeaderButton");
            this.allianceLeaderButton.Name = "allianceLeaderButton";
            this.allianceLeaderButton.UseVisualStyleBackColor = true;
            this.allianceLeaderButton.Click += new System.EventHandler(this.OnAllianceLeaderButtonClick);
            // 
            // allianceParticipantListBox
            // 
            this.allianceParticipantListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            resources.ApplyResources(this.allianceParticipantListBox, "allianceParticipantListBox");
            this.allianceParticipantListBox.FormattingEnabled = true;
            this.allianceParticipantListBox.Name = "allianceParticipantListBox";
            this.allianceParticipantListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.allianceParticipantListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnAllianceParticipantListBoxDrawItem);
            this.allianceParticipantListBox.SelectedIndexChanged += new System.EventHandler(this.OnAllianceParticipantListBoxSelectedIndexChanged);
            // 
            // allianceUpButton
            // 
            resources.ApplyResources(this.allianceUpButton, "allianceUpButton");
            this.allianceUpButton.Name = "allianceUpButton";
            this.allianceUpButton.UseVisualStyleBackColor = true;
            this.allianceUpButton.Click += new System.EventHandler(this.OnAllianceUpButtonClick);
            // 
            // allianceParticipantAddButton
            // 
            resources.ApplyResources(this.allianceParticipantAddButton, "allianceParticipantAddButton");
            this.allianceParticipantAddButton.Name = "allianceParticipantAddButton";
            this.allianceParticipantAddButton.UseVisualStyleBackColor = true;
            this.allianceParticipantAddButton.Click += new System.EventHandler(this.OnAllianceParticipantAddButtonClick);
            // 
            // allianceCountryListBox
            // 
            this.allianceCountryListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            resources.ApplyResources(this.allianceCountryListBox, "allianceCountryListBox");
            this.allianceCountryListBox.FormattingEnabled = true;
            this.allianceCountryListBox.Name = "allianceCountryListBox";
            this.allianceCountryListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.allianceCountryListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnAllianceCountryListBoxDrawItem);
            this.allianceCountryListBox.SelectedIndexChanged += new System.EventHandler(this.OnAllianceCountryListBoxSelectedIndexChanged);
            // 
            // allianceRemoveButton
            // 
            resources.ApplyResources(this.allianceRemoveButton, "allianceRemoveButton");
            this.allianceRemoveButton.Name = "allianceRemoveButton";
            this.allianceRemoveButton.UseVisualStyleBackColor = true;
            this.allianceRemoveButton.Click += new System.EventHandler(this.OnAllianceRemoveButtonClick);
            // 
            // allianceNewButton
            // 
            resources.ApplyResources(this.allianceNewButton, "allianceNewButton");
            this.allianceNewButton.Name = "allianceNewButton";
            this.allianceNewButton.UseVisualStyleBackColor = true;
            this.allianceNewButton.Click += new System.EventHandler(this.OnAllianceNewButtonClick);
            // 
            // allianceParticipantRemoveButton
            // 
            resources.ApplyResources(this.allianceParticipantRemoveButton, "allianceParticipantRemoveButton");
            this.allianceParticipantRemoveButton.Name = "allianceParticipantRemoveButton";
            this.allianceParticipantRemoveButton.UseVisualStyleBackColor = true;
            this.allianceParticipantRemoveButton.Click += new System.EventHandler(this.OnAllianceParticipantRemoveButtonClick);
            // 
            // allianceDownButton
            // 
            resources.ApplyResources(this.allianceDownButton, "allianceDownButton");
            this.allianceDownButton.Name = "allianceDownButton";
            this.allianceDownButton.UseVisualStyleBackColor = true;
            this.allianceDownButton.Click += new System.EventHandler(this.OnAllianceDownButtonClick);
            // 
            // mainTabPage
            // 
            this.mainTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.mainTabPage.Controls.Add(this.optionGroupBox);
            this.mainTabPage.Controls.Add(this.countrySelectionGroupBox);
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
            this.gameSpeedComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.gameSpeedComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gameSpeedComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.gameSpeedComboBox, "gameSpeedComboBox");
            this.gameSpeedComboBox.Name = "gameSpeedComboBox";
            this.gameSpeedComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnGameSpeedComboBoxDrawItem);
            this.gameSpeedComboBox.SelectedIndexChanged += new System.EventHandler(this.OnGameSpeedComboBoxSelectedIndexChanged);
            // 
            // difficultyComboBox
            // 
            this.difficultyComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.difficultyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.difficultyComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.difficultyComboBox, "difficultyComboBox");
            this.difficultyComboBox.Name = "difficultyComboBox";
            this.difficultyComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnDifficultyComboBoxDrawItem);
            this.difficultyComboBox.SelectedIndexChanged += new System.EventHandler(this.OnDifficultyComboBoxSelectedIndexChanged);
            // 
            // aiAggressiveComboBox
            // 
            this.aiAggressiveComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.aiAggressiveComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.aiAggressiveComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.aiAggressiveComboBox, "aiAggressiveComboBox");
            this.aiAggressiveComboBox.Name = "aiAggressiveComboBox";
            this.aiAggressiveComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnAiAggressiveComboBoxDrawItem);
            this.aiAggressiveComboBox.SelectedIndexChanged += new System.EventHandler(this.OnAiAggressiveComboBoxSelectedIndexChanged);
            // 
            // allowTechnologyCheckBox
            // 
            resources.ApplyResources(this.allowTechnologyCheckBox, "allowTechnologyCheckBox");
            this.allowTechnologyCheckBox.Name = "allowTechnologyCheckBox";
            this.allowTechnologyCheckBox.UseVisualStyleBackColor = true;
            this.allowTechnologyCheckBox.CheckedChanged += new System.EventHandler(this.OnAllowTechnologyCheckBoxCheckedChanged);
            // 
            // allowProductionCheckBox
            // 
            resources.ApplyResources(this.allowProductionCheckBox, "allowProductionCheckBox");
            this.allowProductionCheckBox.Name = "allowProductionCheckBox";
            this.allowProductionCheckBox.UseVisualStyleBackColor = true;
            this.allowProductionCheckBox.CheckedChanged += new System.EventHandler(this.OnAllowProductionCheckBoxCheckedChanged);
            // 
            // allowDiplomacyCheckBox
            // 
            resources.ApplyResources(this.allowDiplomacyCheckBox, "allowDiplomacyCheckBox");
            this.allowDiplomacyCheckBox.Name = "allowDiplomacyCheckBox";
            this.allowDiplomacyCheckBox.UseVisualStyleBackColor = true;
            this.allowDiplomacyCheckBox.CheckedChanged += new System.EventHandler(this.OnAllowDiplomacyCheckBoxCheckedChanged);
            // 
            // freeCountryCheckBox
            // 
            resources.ApplyResources(this.freeCountryCheckBox, "freeCountryCheckBox");
            this.freeCountryCheckBox.Name = "freeCountryCheckBox";
            this.freeCountryCheckBox.UseVisualStyleBackColor = true;
            this.freeCountryCheckBox.CheckedChanged += new System.EventHandler(this.OnFreeCountryCheckBoxCheckedChanged);
            // 
            // battleScenarioCheckBox
            // 
            resources.ApplyResources(this.battleScenarioCheckBox, "battleScenarioCheckBox");
            this.battleScenarioCheckBox.Name = "battleScenarioCheckBox";
            this.battleScenarioCheckBox.UseVisualStyleBackColor = true;
            this.battleScenarioCheckBox.CheckedChanged += new System.EventHandler(this.OnBattleScenarioCheckBoxCheckedChanged);
            // 
            // countrySelectionGroupBox
            // 
            this.countrySelectionGroupBox.Controls.Add(this.selectableRemoveButton);
            this.countrySelectionGroupBox.Controls.Add(this.selectableAddButton);
            this.countrySelectionGroupBox.Controls.Add(this.unselectableListBox);
            this.countrySelectionGroupBox.Controls.Add(this.selectableListBox);
            this.countrySelectionGroupBox.Controls.Add(this.propagandaPictureBox);
            this.countrySelectionGroupBox.Controls.Add(this.propagandaBrowseButton);
            this.countrySelectionGroupBox.Controls.Add(this.propagandaTextBox);
            this.countrySelectionGroupBox.Controls.Add(this.propagandaLabel);
            this.countrySelectionGroupBox.Controls.Add(this.countryDescTextBox);
            this.countrySelectionGroupBox.Controls.Add(this.countryDescLabel);
            this.countrySelectionGroupBox.Controls.Add(this.majorAddButton);
            this.countrySelectionGroupBox.Controls.Add(this.majorRemoveButton);
            this.countrySelectionGroupBox.Controls.Add(this.majorListBox);
            this.countrySelectionGroupBox.Controls.Add(this.majorLabel);
            this.countrySelectionGroupBox.Controls.Add(this.selectableLabel);
            this.countrySelectionGroupBox.Controls.Add(this.majorUpButton);
            this.countrySelectionGroupBox.Controls.Add(this.majorDownButton);
            resources.ApplyResources(this.countrySelectionGroupBox, "countrySelectionGroupBox");
            this.countrySelectionGroupBox.Name = "countrySelectionGroupBox";
            this.countrySelectionGroupBox.TabStop = false;
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
            this.propagandaTextBox.TextChanged += new System.EventHandler(this.OnPropagandaTextBoxTextChanged);
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
            this.countryDescTextBox.TextChanged += new System.EventHandler(this.OnCountryDescTextBoxTextChanged);
            // 
            // countryDescLabel
            // 
            resources.ApplyResources(this.countryDescLabel, "countryDescLabel");
            this.countryDescLabel.Name = "countryDescLabel";
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
            this.includeFolderBrowseButton.Click += new System.EventHandler(this.OnIncludeFolderBrowseButtonClick);
            // 
            // includeFolderTextBox
            // 
            resources.ApplyResources(this.includeFolderTextBox, "includeFolderTextBox");
            this.includeFolderTextBox.Name = "includeFolderTextBox";
            this.includeFolderTextBox.TextChanged += new System.EventHandler(this.OnIncludeFolderTextBoxTextChanged);
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
            this.scenarioNameTextBox.TextChanged += new System.EventHandler(this.OnScenarioNameTextBoxTextChanged);
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
            this.panelImageTextBox.TextChanged += new System.EventHandler(this.OnPanelImageTextBoxTextChanged);
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
            this.startYearTextBox.Validated += new System.EventHandler(this.OnStartYearTextBoxValidated);
            // 
            // startMonthTextBox
            // 
            resources.ApplyResources(this.startMonthTextBox, "startMonthTextBox");
            this.startMonthTextBox.Name = "startMonthTextBox";
            this.startMonthTextBox.Validated += new System.EventHandler(this.OnStartMonthTextBoxValidated);
            // 
            // startDayTextBox
            // 
            resources.ApplyResources(this.startDayTextBox, "startDayTextBox");
            this.startDayTextBox.Name = "startDayTextBox";
            this.startDayTextBox.Validated += new System.EventHandler(this.OnStartDayTextBoxValidated);
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
            this.endYearTextBox.Validated += new System.EventHandler(this.OnEndYearTextBoxValidated);
            // 
            // endMonthTextBox
            // 
            resources.ApplyResources(this.endMonthTextBox, "endMonthTextBox");
            this.endMonthTextBox.Name = "endMonthTextBox";
            this.endMonthTextBox.Validated += new System.EventHandler(this.OnEndMonthTextBoxValidated);
            // 
            // endDayTextBox
            // 
            resources.ApplyResources(this.endDayTextBox, "endDayTextBox");
            this.endDayTextBox.Name = "endDayTextBox";
            this.endDayTextBox.Validated += new System.EventHandler(this.OnEndDayTextBoxValidated);
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
            this.scenarioTabControl.Name = "scenarioTabControl";
            this.scenarioTabControl.SelectedIndex = 0;
            this.scenarioTabControl.SelectedIndexChanged += new System.EventHandler(this.OnScenarioTabControlSelectedIndexChanged);
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
            this.governmentTabPage.ResumeLayout(false);
            this.cabinetGroupBox.ResumeLayout(false);
            this.cabinetGroupBox.PerformLayout();
            this.politicalSliderGroupBox.ResumeLayout(false);
            this.politicalSliderGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.interventionismTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.defenseLobbyTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.professionalArmyTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.freeMarketTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.freedomTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.politicalLeftTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.democraticTrackBar)).EndInit();
            this.countryTabPage.ResumeLayout(false);
            this.countryModifierGroupBox.ResumeLayout(false);
            this.countryModifierGroupBox.PerformLayout();
            this.aiGroupBox.ResumeLayout(false);
            this.aiGroupBox.PerformLayout();
            this.countryResourceGroupBox.ResumeLayout(false);
            this.countryResourceGroupBox.PerformLayout();
            this.countryInfoGroupBox.ResumeLayout(false);
            this.countryInfoGroupBox.PerformLayout();
            this.tradeTabPage.ResumeLayout(false);
            this.tradeDealsGroupBox.ResumeLayout(false);
            this.tradeDealsGroupBox.PerformLayout();
            this.tradeInfoGroupBox.ResumeLayout(false);
            this.tradeInfoGroupBox.PerformLayout();
            this.relationTabPage.ResumeLayout(false);
            this.intelligenceGroupBox.ResumeLayout(false);
            this.intelligenceGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spyNumNumericUpDown)).EndInit();
            this.diplomacyGroupBox.ResumeLayout(false);
            this.diplomacyGroupBox.PerformLayout();
            this.guaranteeGroupBox.ResumeLayout(false);
            this.guaranteeGroupBox.PerformLayout();
            this.peaceGroupBox.ResumeLayout(false);
            this.peaceGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.relationValueNumericUpDown)).EndInit();
            this.nonAggressionGroupBox.ResumeLayout(false);
            this.nonAggressionGroupBox.PerformLayout();
            this.allianceTabPage.ResumeLayout(false);
            this.warGroupBox.ResumeLayout(false);
            this.warGroupBox.PerformLayout();
            this.allianceGroupBox.ResumeLayout(false);
            this.allianceGroupBox.PerformLayout();
            this.mainTabPage.ResumeLayout(false);
            this.optionGroupBox.ResumeLayout(false);
            this.optionGroupBox.PerformLayout();
            this.countrySelectionGroupBox.ResumeLayout(false);
            this.countrySelectionGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propagandaPictureBox)).EndInit();
            this.infoGroupBox.ResumeLayout(false);
            this.infoGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelPictureBox)).EndInit();
            this.folderGroupBox.ResumeLayout(false);
            this.folderGroupBox.PerformLayout();
            this.typeGroupBox.ResumeLayout(false);
            this.typeGroupBox.PerformLayout();
            this.scenarioTabControl.ResumeLayout(false);
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
        private System.Windows.Forms.GroupBox countryInfoGroupBox;
        private System.Windows.Forms.TextBox flagExtTextBox;
        private System.Windows.Forms.TextBox aiFileNameTextBox;
        private System.Windows.Forms.TextBox countryNameTextBox;
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
        private System.Windows.Forms.Label spyNumLabel;
        private System.Windows.Forms.GroupBox diplomacyGroupBox;
        private System.Windows.Forms.GroupBox guaranteeGroupBox;
        private System.Windows.Forms.CheckBox guaranteeCheckBox;
        private System.Windows.Forms.TextBox guaranteeYearTextBox;
        private System.Windows.Forms.TextBox guaranteeMonthTextBox;
        private System.Windows.Forms.Label guaranteeEndLabel;
        private System.Windows.Forms.TextBox guaranteeDayTextBox;
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
        private System.Windows.Forms.NumericUpDown relationValueNumericUpDown;
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
        private System.Windows.Forms.Label relationValueLabel;
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
        private System.Windows.Forms.GroupBox warGroupBox;
        private System.Windows.Forms.Label warDefenderLabel;
        private System.Windows.Forms.ListView warListView;
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
        private System.Windows.Forms.ListView allianceListView;
        private System.Windows.Forms.ColumnHeader allianceNameColumnHeader;
        private System.Windows.Forms.ColumnHeader allianceParticipantColumnHeader;
        private System.Windows.Forms.Label allianceParticipantLabel;
        private System.Windows.Forms.Button allianceLeaderButton;
        private System.Windows.Forms.ListBox allianceParticipantListBox;
        private System.Windows.Forms.Button allianceUpButton;
        private System.Windows.Forms.Button allianceParticipantAddButton;
        private System.Windows.Forms.ListBox allianceCountryListBox;
        private System.Windows.Forms.Button allianceRemoveButton;
        private System.Windows.Forms.Button allianceNewButton;
        private System.Windows.Forms.Button allianceParticipantRemoveButton;
        private System.Windows.Forms.Button allianceDownButton;
        private System.Windows.Forms.TabPage mainTabPage;
        private System.Windows.Forms.GroupBox optionGroupBox;
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
        private System.Windows.Forms.TextBox countryDescTextBox;
        private System.Windows.Forms.Label countryDescLabel;
        private System.Windows.Forms.Button majorAddButton;
        private System.Windows.Forms.Button majorRemoveButton;
        private System.Windows.Forms.ListBox majorListBox;
        private System.Windows.Forms.Label majorLabel;
        private System.Windows.Forms.Label selectableLabel;
        private System.Windows.Forms.Button majorUpButton;
        private System.Windows.Forms.Button majorDownButton;
        private System.Windows.Forms.GroupBox infoGroupBox;
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
        private System.Windows.Forms.TextBox warEndMonthTextBox;
        private System.Windows.Forms.TextBox warEndYearTextBox;
        private System.Windows.Forms.TextBox warEndDayTextBox;
        private System.Windows.Forms.Label warEndDateLabel;
        private System.Windows.Forms.TextBox warStartMonthTextBox;
        private System.Windows.Forms.TextBox warStartYearTextBox;
        private System.Windows.Forms.TextBox warStartDayTextBox;
        private System.Windows.Forms.Label warStartDateLabel;
        private System.Windows.Forms.TextBox allianceNameTextBox;
        private System.Windows.Forms.Label allianceNameLabel;
        private System.Windows.Forms.TextBox allianceIdTextBox;
        private System.Windows.Forms.TextBox allianceTypeTextBox;
        private System.Windows.Forms.Label allianceIdLabel;
        private System.Windows.Forms.TextBox warIdTextBox;
        private System.Windows.Forms.TextBox warTypeTextBox;
        private System.Windows.Forms.Label warIdLabel;
        private System.Windows.Forms.Button selectableRemoveButton;
        private System.Windows.Forms.Button selectableAddButton;
        private System.Windows.Forms.ListBox unselectableListBox;
        private System.Windows.Forms.ListBox selectableListBox;
        private System.Windows.Forms.Label warDefenderIdLabel;
        private System.Windows.Forms.Label warAttackerIdLabel;
        private System.Windows.Forms.TextBox warDefenderIdTextBox;
        private System.Windows.Forms.TextBox warDefenderTypeTextBox;
        private System.Windows.Forms.TextBox warAttackerIdTextBox;
        private System.Windows.Forms.TextBox warAttackerTypeTextBox;
        private System.Windows.Forms.Button warDefenderLeaderButton;
        private System.Windows.Forms.Button warAttackerLeaderButton;
        private System.Windows.Forms.ComboBox regularIdComboBox;
        private System.Windows.Forms.TextBox belligerenceTextBox;
        private System.Windows.Forms.TextBox extraTcTextBox;
        private System.Windows.Forms.TextBox dissentTextBox;
        private System.Windows.Forms.TextBox peacetimeIcModifierTextBox;
        private System.Windows.Forms.TextBox industrialModifierTextBox;
        private System.Windows.Forms.TextBox wartimeIcModifierTextBox;
        private System.Windows.Forms.TextBox groundDefEffTextBox;
        private System.Windows.Forms.GroupBox countryResourceGroupBox;
        private System.Windows.Forms.TextBox relativeManpowerTextBox;
        private System.Windows.Forms.TextBox countryManpowerTextBox;
        private System.Windows.Forms.Label countryManpowerLabel;
        private System.Windows.Forms.TextBox countryEnergyTextBox;
        private System.Windows.Forms.Label countryEnergyLabel;
        private System.Windows.Forms.TextBox countryMoneyTextBox;
        private System.Windows.Forms.Label countryMoneyLabel;
        private System.Windows.Forms.TextBox countrySuppliesTextBox;
        private System.Windows.Forms.Label countrySuppliesLabel;
        private System.Windows.Forms.TextBox countryOilTextBox;
        private System.Windows.Forms.Label countryOilLabel;
        private System.Windows.Forms.TextBox countryRareMaterialsTextBox;
        private System.Windows.Forms.Label countryRareMaterialsLabel;
        private System.Windows.Forms.TextBox countryMetalTextBox;
        private System.Windows.Forms.Label countryMetalLabel;
        private System.Windows.Forms.TextBox countryEscortsTextBox;
        private System.Windows.Forms.Label countryEscortsLabel;
        private System.Windows.Forms.TextBox countryTransportsTextBox;
        private System.Windows.Forms.Label countryTransportsLabel;
        private System.Windows.Forms.TextBox nukeTextBox;
        private System.Windows.Forms.TextBox offmapIcTextBox;
        private System.Windows.Forms.Label countryIcLabel;
        private System.Windows.Forms.TextBox offmapManpowerTextBox;
        private System.Windows.Forms.TextBox offmapEscortsTextBox;
        private System.Windows.Forms.TextBox offmapTransportsTextBox;
        private System.Windows.Forms.TextBox offmapMoneyTextBox;
        private System.Windows.Forms.TextBox offmapSuppliesTextBox;
        private System.Windows.Forms.TextBox offmapOilTextBox;
        private System.Windows.Forms.TextBox offmapRareMaterialsTextBox;
        private System.Windows.Forms.TextBox offmapMetalTextBox;
        private System.Windows.Forms.TextBox offmapEnergyTextBox;
        private System.Windows.Forms.TextBox nukeDayTextBox;
        private System.Windows.Forms.TextBox nukeMonthTextBox;
        private System.Windows.Forms.TextBox nukeYearTextBox;
        private System.Windows.Forms.GroupBox aiGroupBox;
        private System.Windows.Forms.Button aiFileBrowseButton;
        private System.Windows.Forms.ListBox countryListBox;
        private System.Windows.Forms.GroupBox countryModifierGroupBox;
        private System.Windows.Forms.GroupBox politicalSliderGroupBox;
        private System.Windows.Forms.TrackBar interventionismTrackBar;
        private System.Windows.Forms.Label isolationismLabel;
        private System.Windows.Forms.Label interventionismLabel;
        private System.Windows.Forms.TrackBar defenseLobbyTrackBar;
        private System.Windows.Forms.Label doveLobbyLabel;
        private System.Windows.Forms.Label hawkLobbyLabel;
        private System.Windows.Forms.TrackBar professionalArmyTrackBar;
        private System.Windows.Forms.Label draftedArmyLabel;
        private System.Windows.Forms.Label standingArmyLabel;
        private System.Windows.Forms.TrackBar freeMarketTrackBar;
        private System.Windows.Forms.Label centralPlanningLabel;
        private System.Windows.Forms.Label freeMarketLabel;
        private System.Windows.Forms.TrackBar freedomTrackBar;
        private System.Windows.Forms.Label closedSocietyLabel;
        private System.Windows.Forms.Label openSocietyLabel;
        private System.Windows.Forms.TrackBar politicalLeftTrackBar;
        private System.Windows.Forms.Label politicalRightLabel;
        private System.Windows.Forms.Label politicalLeftLabel;
        private System.Windows.Forms.TrackBar democraticTrackBar;
        private System.Windows.Forms.Label authoritarianLabel;
        private System.Windows.Forms.Label democraticLabel;
        private System.Windows.Forms.TextBox sliderDayTextBox;
        private System.Windows.Forms.TextBox sliderMonthTextBox;
        private System.Windows.Forms.TextBox sliderYearTextBox;
        private System.Windows.Forms.ListBox governmentCountryListBox;
        private System.Windows.Forms.Label sliderDateLabel;
        private System.Windows.Forms.GroupBox cabinetGroupBox;
        private System.Windows.Forms.TextBox chiefOfAirIdTextBox;
        private System.Windows.Forms.TextBox chiefOfAirTypeTextBox;
        private System.Windows.Forms.ComboBox chiefOfAirComboBox;
        private System.Windows.Forms.Label chiefOfAirLabel;
        private System.Windows.Forms.TextBox chiefOfNavyIdTextBox;
        private System.Windows.Forms.TextBox chiefOfNavyTypeTextBox;
        private System.Windows.Forms.ComboBox chiefOfNavyComboBox;
        private System.Windows.Forms.Label chiefOfNavyLabel;
        private System.Windows.Forms.TextBox chiefOfArmyIdTextBox;
        private System.Windows.Forms.TextBox chiefOfArmyTypeTextBox;
        private System.Windows.Forms.ComboBox chiefOfArmyComboBox;
        private System.Windows.Forms.Label chiefOfArmyLabel;
        private System.Windows.Forms.TextBox chiefOfStaffIdTextBox;
        private System.Windows.Forms.TextBox chiefOfStaffTypeTextBox;
        private System.Windows.Forms.ComboBox chiefOfStaffComboBox;
        private System.Windows.Forms.Label chiefOfStaffLabel;
        private System.Windows.Forms.TextBox ministerOfIntelligenceIdTextBox;
        private System.Windows.Forms.TextBox ministerOfIntelligenceTypeTextBox;
        private System.Windows.Forms.ComboBox ministerOfIntelligenceComboBox;
        private System.Windows.Forms.Label ministerOfIntelligenceLabel;
        private System.Windows.Forms.TextBox ministerOfSecurityIdTextBox;
        private System.Windows.Forms.TextBox ministerOfSecurityTypeTextBox;
        private System.Windows.Forms.ComboBox ministerOfSecurityComboBox;
        private System.Windows.Forms.Label ministerOfSecurityLabel;
        private System.Windows.Forms.TextBox armamentMinisterIdTextBox;
        private System.Windows.Forms.TextBox armamentMinisterTypeTextBox;
        private System.Windows.Forms.ComboBox armamentMinisterComboBox;
        private System.Windows.Forms.Label armamentMinisterLabel;
        private System.Windows.Forms.TextBox foreignMinisterIdTextBox;
        private System.Windows.Forms.TextBox foreignMinisterTypeTextBox;
        private System.Windows.Forms.ComboBox foreignMinisterComboBox;
        private System.Windows.Forms.Label foreignMinisterlabel;
        private System.Windows.Forms.TextBox headOfGovernmentIdTextBox;
        private System.Windows.Forms.TextBox headOfGovernmentTypeTextBox;
        private System.Windows.Forms.ComboBox headOfGovernmentComboBox;
        private System.Windows.Forms.Label headOfGovernmentLabel;
        private System.Windows.Forms.TextBox headOfStateIdTextBox;
        private System.Windows.Forms.TextBox headOfStateTypeTextBox;
        private System.Windows.Forms.ComboBox headOfStateComboBox;
        private System.Windows.Forms.Label headOfStateLabel;
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
        private System.Windows.Forms.TextBox provinceNameTextBox;
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

    }
}