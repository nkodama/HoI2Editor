namespace HoI2Editor.Forms
{
    partial class UnitEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UnitEditorForm));
            this.bottomButton = new System.Windows.Forms.Button();
            this.downButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.topButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.cloneButton = new System.Windows.Forms.Button();
            this.newButton = new System.Windows.Forms.Button();
            this.modelListView = new System.Windows.Forms.ListView();
            this.noColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.classColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.branchColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.costColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buildTimeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.manPowerSkillColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.supplyColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fuelColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.unitListBox = new System.Windows.Forms.ListBox();
            this.editTabControl = new System.Windows.Forms.TabControl();
            this.classTabPage = new System.Windows.Forms.TabPage();
            this.listPrioNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.listPrioLabel = new System.Windows.Forms.Label();
            this.upgradeGroupBox = new System.Windows.Forms.GroupBox();
            this.upgradeTimeTextBox = new System.Windows.Forms.TextBox();
            this.upgradeTimeLabel = new System.Windows.Forms.Label();
            this.upgradeCostTextBox = new System.Windows.Forms.TextBox();
            this.upgradeCostLabel = new System.Windows.Forms.Label();
            this.upgradeTypeComboBox = new System.Windows.Forms.ComboBox();
            this.upgradeTypeLabel = new System.Windows.Forms.Label();
            this.upgradeRemoveButton = new System.Windows.Forms.Button();
            this.upgradeAddButton = new System.Windows.Forms.Button();
            this.upgradeListView = new System.Windows.Forms.ListView();
            this.upgradeTypeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.upgradeCostColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.upgradeTimeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.militaryValueNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.militaryValueLabel = new System.Windows.Forms.Label();
            this.transmuteComboBox = new System.Windows.Forms.ComboBox();
            this.transmuteLabel = new System.Windows.Forms.Label();
            this.spriteTypeComboBox = new System.Windows.Forms.ComboBox();
            this.spriteTypeLabel = new System.Windows.Forms.Label();
            this.realUnitTypeComboBox = new System.Windows.Forms.ComboBox();
            this.realUnitTypeLabel = new System.Windows.Forms.Label();
            this.productableCheckBox = new System.Windows.Forms.CheckBox();
            this.gfxPrioNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.gfxPrioLabel = new System.Windows.Forms.Label();
            this.detachableCheckBox = new System.Windows.Forms.CheckBox();
            this.allowedBrigadesLabel = new System.Windows.Forms.Label();
            this.branchLabel = new System.Windows.Forms.Label();
            this.branchComboBox = new System.Windows.Forms.ComboBox();
            this.allowedBrigadesCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.classShortDescTextBox = new System.Windows.Forms.TextBox();
            this.classShortDescLabel = new System.Windows.Forms.Label();
            this.classDescTextBox = new System.Windows.Forms.TextBox();
            this.classDescLabel = new System.Windows.Forms.Label();
            this.classShortNameTextBox = new System.Windows.Forms.TextBox();
            this.classShortNameLabel = new System.Windows.Forms.Label();
            this.classNameTextBox = new System.Windows.Forms.TextBox();
            this.classNameLabel = new System.Windows.Forms.Label();
            this.modelTabPage = new System.Windows.Forms.TabPage();
            this.modelNameTextBox = new System.Windows.Forms.TextBox();
            this.equipmentGroupBox = new System.Windows.Forms.GroupBox();
            this.quantityTextBox = new System.Windows.Forms.TextBox();
            this.equipmentDownButton = new System.Windows.Forms.Button();
            this.equipmentUpButton = new System.Windows.Forms.Button();
            this.quantityLabel = new System.Windows.Forms.Label();
            this.resourceComboBox = new System.Windows.Forms.ComboBox();
            this.resourceLabel = new System.Windows.Forms.Label();
            this.equipmentRemoveButton = new System.Windows.Forms.Button();
            this.equipmentAddButton = new System.Windows.Forms.Button();
            this.equipmentListView = new System.Windows.Forms.ListView();
            this.resourceColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.quantityColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.productionGroupBox = new System.Windows.Forms.GroupBox();
            this.reinforceCostTextBox = new System.Windows.Forms.TextBox();
            this.costTextBox = new System.Windows.Forms.TextBox();
            this.reinforceCostLabel = new System.Windows.Forms.Label();
            this.costLabel = new System.Windows.Forms.Label();
            this.reinforceTimeTextBox = new System.Windows.Forms.TextBox();
            this.buildTimeLabel = new System.Windows.Forms.Label();
            this.reinforceTimeLabel = new System.Windows.Forms.Label();
            this.buildTimeTextBox = new System.Windows.Forms.TextBox();
            this.manPowerTextBox = new System.Windows.Forms.TextBox();
            this.manPowerLabel = new System.Windows.Forms.Label();
            this.upgradeTimeFactorTextBox = new System.Windows.Forms.TextBox();
            this.upgradeTimeFactorLabel = new System.Windows.Forms.Label();
            this.upgradeCostFactorLabel = new System.Windows.Forms.Label();
            this.upgradeCostFactorTextBox = new System.Windows.Forms.TextBox();
            this.speedGroupBox = new System.Windows.Forms.GroupBox();
            this.speedCapAaTextBox = new System.Windows.Forms.TextBox();
            this.maxSpeedTextBox = new System.Windows.Forms.TextBox();
            this.speedCapAaLabel = new System.Windows.Forms.Label();
            this.maxSpeedLabel = new System.Windows.Forms.Label();
            this.speedCapAtTextBox = new System.Windows.Forms.TextBox();
            this.speedCapAtLabel = new System.Windows.Forms.Label();
            this.speedCapEngTextBox = new System.Windows.Forms.TextBox();
            this.speedCapAllTextBox = new System.Windows.Forms.TextBox();
            this.speedCapEngLabel = new System.Windows.Forms.Label();
            this.speedCapAllLabel = new System.Windows.Forms.Label();
            this.speedCapArtTextBox = new System.Windows.Forms.TextBox();
            this.speedCapArtLabel = new System.Windows.Forms.Label();
            this.battleGroupBox = new System.Windows.Forms.GroupBox();
            this.artilleryBombardmentTextBox = new System.Windows.Forms.TextBox();
            this.artilleryBombardmentLabel = new System.Windows.Forms.Label();
            this.visibilityTextBox = new System.Windows.Forms.TextBox();
            this.visibilityLabel = new System.Windows.Forms.Label();
            this.noFuelCombatModTextBox = new System.Windows.Forms.TextBox();
            this.noFuelCombatModLabel = new System.Windows.Forms.Label();
            this.airDetectionCapabilityTextBox = new System.Windows.Forms.TextBox();
            this.airDetectionCapabilityLabel = new System.Windows.Forms.Label();
            this.subDetectionCapabilityTextBox = new System.Windows.Forms.TextBox();
            this.subDetectionCapabilityLabel = new System.Windows.Forms.Label();
            this.surfaceDetectionCapabilityTextBox = new System.Windows.Forms.TextBox();
            this.surfaceDetectionCapabilityLabel = new System.Windows.Forms.Label();
            this.distanceTextBox = new System.Windows.Forms.TextBox();
            this.distanceLabel = new System.Windows.Forms.Label();
            this.strategicAttackTextBox = new System.Windows.Forms.TextBox();
            this.strategicAttackLabel = new System.Windows.Forms.Label();
            this.navalAttackTextBox = new System.Windows.Forms.TextBox();
            this.navalAttackLabel = new System.Windows.Forms.Label();
            this.airAttackTextBox = new System.Windows.Forms.TextBox();
            this.airAttackLabel = new System.Windows.Forms.Label();
            this.shoreBombardmentTextBox = new System.Windows.Forms.TextBox();
            this.shoreBombardmentLabel = new System.Windows.Forms.Label();
            this.convoyAttackTextBox = new System.Windows.Forms.TextBox();
            this.convoyAttackLabel = new System.Windows.Forms.Label();
            this.subAttackTextBox = new System.Windows.Forms.TextBox();
            this.subAttackLabel = new System.Windows.Forms.Label();
            this.seaAttackTextBox = new System.Windows.Forms.TextBox();
            this.seaAttackLabel = new System.Windows.Forms.Label();
            this.hardAttackTextBox = new System.Windows.Forms.TextBox();
            this.hardAttackLabel = new System.Windows.Forms.Label();
            this.softAttackTextBox = new System.Windows.Forms.TextBox();
            this.softAttackLabel = new System.Windows.Forms.Label();
            this.softnessTextBox = new System.Windows.Forms.TextBox();
            this.softnessLabel = new System.Windows.Forms.Label();
            this.toughnessTextBox = new System.Windows.Forms.TextBox();
            this.toughnessLabel = new System.Windows.Forms.Label();
            this.surfaceDefenceTextBox = new System.Windows.Forms.TextBox();
            this.surfaceDefenceLabel = new System.Windows.Forms.Label();
            this.airDefenceTextBox = new System.Windows.Forms.TextBox();
            this.airDefenceLabel = new System.Windows.Forms.Label();
            this.seaDefenceTextBox = new System.Windows.Forms.TextBox();
            this.seaDefenceLabel = new System.Windows.Forms.Label();
            this.defensivenessTextBox = new System.Windows.Forms.TextBox();
            this.defensivenessLabel = new System.Windows.Forms.Label();
            this.basicGroupBox = new System.Windows.Forms.GroupBox();
            this.transportWeightTextBox = new System.Windows.Forms.TextBox();
            this.rangeLabel = new System.Windows.Forms.Label();
            this.transportWeightLabel = new System.Windows.Forms.Label();
            this.maxOilStockTextBox = new System.Windows.Forms.TextBox();
            this.rangeTextBox = new System.Windows.Forms.TextBox();
            this.transportCapabilityLabel = new System.Windows.Forms.Label();
            this.maxOilStockLabel = new System.Windows.Forms.Label();
            this.transportCapabilityTextBox = new System.Windows.Forms.TextBox();
            this.maxSupplyStockTextBox = new System.Windows.Forms.TextBox();
            this.maxSupplyStockLabel = new System.Windows.Forms.Label();
            this.fuelConsumptionTextBox = new System.Windows.Forms.TextBox();
            this.fuelConsumptionLabel = new System.Windows.Forms.Label();
            this.supplyConsumptionTextBox = new System.Windows.Forms.TextBox();
            this.supplyConsumptionLabel = new System.Windows.Forms.Label();
            this.suppressionTextBox = new System.Windows.Forms.TextBox();
            this.suppressionLabel = new System.Windows.Forms.Label();
            this.moraleTextBox = new System.Windows.Forms.TextBox();
            this.moraleLabel = new System.Windows.Forms.Label();
            this.defaultOrganisationTextBox = new System.Windows.Forms.TextBox();
            this.defaultOrganisationLabel = new System.Windows.Forms.Label();
            this.modelIconPictureBox = new System.Windows.Forms.PictureBox();
            this.modelImagePictureBox = new System.Windows.Forms.PictureBox();
            this.closeButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.reloadButton = new System.Windows.Forms.Button();
            this.countryListView = new System.Windows.Forms.ListView();
            this.dummyColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.editTabControl.SuspendLayout();
            this.classTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listPrioNumericUpDown)).BeginInit();
            this.upgradeGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.militaryValueNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gfxPrioNumericUpDown)).BeginInit();
            this.modelTabPage.SuspendLayout();
            this.equipmentGroupBox.SuspendLayout();
            this.productionGroupBox.SuspendLayout();
            this.speedGroupBox.SuspendLayout();
            this.battleGroupBox.SuspendLayout();
            this.basicGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.modelIconPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.modelImagePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // bottomButton
            // 
            resources.ApplyResources(this.bottomButton, "bottomButton");
            this.bottomButton.Name = "bottomButton";
            this.bottomButton.UseVisualStyleBackColor = true;
            this.bottomButton.Click += new System.EventHandler(this.OnBottonButtonClick);
            // 
            // downButton
            // 
            resources.ApplyResources(this.downButton, "downButton");
            this.downButton.Name = "downButton";
            this.downButton.UseVisualStyleBackColor = true;
            this.downButton.Click += new System.EventHandler(this.OnDownButtonClick);
            // 
            // upButton
            // 
            resources.ApplyResources(this.upButton, "upButton");
            this.upButton.Name = "upButton";
            this.upButton.UseVisualStyleBackColor = true;
            this.upButton.Click += new System.EventHandler(this.OnUpButtonClick);
            // 
            // topButton
            // 
            resources.ApplyResources(this.topButton, "topButton");
            this.topButton.Name = "topButton";
            this.topButton.UseVisualStyleBackColor = true;
            this.topButton.Click += new System.EventHandler(this.OnTopButtonClick);
            // 
            // removeButton
            // 
            resources.ApplyResources(this.removeButton, "removeButton");
            this.removeButton.Name = "removeButton";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.OnRemoveButtonClick);
            // 
            // cloneButton
            // 
            resources.ApplyResources(this.cloneButton, "cloneButton");
            this.cloneButton.Name = "cloneButton";
            this.cloneButton.UseVisualStyleBackColor = true;
            this.cloneButton.Click += new System.EventHandler(this.OnCloneButtonClick);
            // 
            // newButton
            // 
            resources.ApplyResources(this.newButton, "newButton");
            this.newButton.Name = "newButton";
            this.newButton.UseVisualStyleBackColor = true;
            this.newButton.Click += new System.EventHandler(this.OnNewButtonClick);
            // 
            // modelListView
            // 
            resources.ApplyResources(this.modelListView, "modelListView");
            this.modelListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.noColumnHeader,
            this.nameColumnHeader,
            this.classColumnHeader,
            this.branchColumnHeader,
            this.costColumnHeader,
            this.buildTimeColumnHeader,
            this.manPowerSkillColumnHeader,
            this.supplyColumnHeader,
            this.fuelColumnHeader});
            this.modelListView.FullRowSelect = true;
            this.modelListView.GridLines = true;
            this.modelListView.HideSelection = false;
            this.modelListView.MultiSelect = false;
            this.modelListView.Name = "modelListView";
            this.modelListView.UseCompatibleStateImageBehavior = false;
            this.modelListView.View = System.Windows.Forms.View.Details;
            this.modelListView.SelectedIndexChanged += new System.EventHandler(this.OnModelListViewSelectedIndexChanged);
            // 
            // noColumnHeader
            // 
            resources.ApplyResources(this.noColumnHeader, "noColumnHeader");
            // 
            // nameColumnHeader
            // 
            resources.ApplyResources(this.nameColumnHeader, "nameColumnHeader");
            // 
            // classColumnHeader
            // 
            resources.ApplyResources(this.classColumnHeader, "classColumnHeader");
            // 
            // branchColumnHeader
            // 
            resources.ApplyResources(this.branchColumnHeader, "branchColumnHeader");
            // 
            // costColumnHeader
            // 
            resources.ApplyResources(this.costColumnHeader, "costColumnHeader");
            // 
            // buildTimeColumnHeader
            // 
            resources.ApplyResources(this.buildTimeColumnHeader, "buildTimeColumnHeader");
            // 
            // manPowerSkillColumnHeader
            // 
            resources.ApplyResources(this.manPowerSkillColumnHeader, "manPowerSkillColumnHeader");
            // 
            // supplyColumnHeader
            // 
            resources.ApplyResources(this.supplyColumnHeader, "supplyColumnHeader");
            // 
            // fuelColumnHeader
            // 
            resources.ApplyResources(this.fuelColumnHeader, "fuelColumnHeader");
            // 
            // unitListBox
            // 
            resources.ApplyResources(this.unitListBox, "unitListBox");
            this.unitListBox.Name = "unitListBox";
            this.unitListBox.SelectedIndexChanged += new System.EventHandler(this.OnUnitListBoxSelectedIndexChanged);
            // 
            // editTabControl
            // 
            this.editTabControl.Controls.Add(this.classTabPage);
            this.editTabControl.Controls.Add(this.modelTabPage);
            resources.ApplyResources(this.editTabControl, "editTabControl");
            this.editTabControl.Name = "editTabControl";
            this.editTabControl.SelectedIndex = 0;
            // 
            // classTabPage
            // 
            this.classTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.classTabPage.Controls.Add(this.listPrioNumericUpDown);
            this.classTabPage.Controls.Add(this.listPrioLabel);
            this.classTabPage.Controls.Add(this.upgradeGroupBox);
            this.classTabPage.Controls.Add(this.militaryValueNumericUpDown);
            this.classTabPage.Controls.Add(this.militaryValueLabel);
            this.classTabPage.Controls.Add(this.transmuteComboBox);
            this.classTabPage.Controls.Add(this.transmuteLabel);
            this.classTabPage.Controls.Add(this.spriteTypeComboBox);
            this.classTabPage.Controls.Add(this.spriteTypeLabel);
            this.classTabPage.Controls.Add(this.realUnitTypeComboBox);
            this.classTabPage.Controls.Add(this.realUnitTypeLabel);
            this.classTabPage.Controls.Add(this.productableCheckBox);
            this.classTabPage.Controls.Add(this.gfxPrioNumericUpDown);
            this.classTabPage.Controls.Add(this.gfxPrioLabel);
            this.classTabPage.Controls.Add(this.detachableCheckBox);
            this.classTabPage.Controls.Add(this.allowedBrigadesLabel);
            this.classTabPage.Controls.Add(this.branchLabel);
            this.classTabPage.Controls.Add(this.branchComboBox);
            this.classTabPage.Controls.Add(this.allowedBrigadesCheckedListBox);
            this.classTabPage.Controls.Add(this.classShortDescTextBox);
            this.classTabPage.Controls.Add(this.classShortDescLabel);
            this.classTabPage.Controls.Add(this.classDescTextBox);
            this.classTabPage.Controls.Add(this.classDescLabel);
            this.classTabPage.Controls.Add(this.classShortNameTextBox);
            this.classTabPage.Controls.Add(this.classShortNameLabel);
            this.classTabPage.Controls.Add(this.classNameTextBox);
            this.classTabPage.Controls.Add(this.classNameLabel);
            resources.ApplyResources(this.classTabPage, "classTabPage");
            this.classTabPage.Name = "classTabPage";
            // 
            // listPrioNumericUpDown
            // 
            resources.ApplyResources(this.listPrioNumericUpDown, "listPrioNumericUpDown");
            this.listPrioNumericUpDown.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.listPrioNumericUpDown.Name = "listPrioNumericUpDown";
            // 
            // listPrioLabel
            // 
            resources.ApplyResources(this.listPrioLabel, "listPrioLabel");
            this.listPrioLabel.Name = "listPrioLabel";
            // 
            // upgradeGroupBox
            // 
            this.upgradeGroupBox.Controls.Add(this.upgradeTimeTextBox);
            this.upgradeGroupBox.Controls.Add(this.upgradeTimeLabel);
            this.upgradeGroupBox.Controls.Add(this.upgradeCostTextBox);
            this.upgradeGroupBox.Controls.Add(this.upgradeCostLabel);
            this.upgradeGroupBox.Controls.Add(this.upgradeTypeComboBox);
            this.upgradeGroupBox.Controls.Add(this.upgradeTypeLabel);
            this.upgradeGroupBox.Controls.Add(this.upgradeRemoveButton);
            this.upgradeGroupBox.Controls.Add(this.upgradeAddButton);
            this.upgradeGroupBox.Controls.Add(this.upgradeListView);
            resources.ApplyResources(this.upgradeGroupBox, "upgradeGroupBox");
            this.upgradeGroupBox.Name = "upgradeGroupBox";
            this.upgradeGroupBox.TabStop = false;
            // 
            // upgradeTimeTextBox
            // 
            resources.ApplyResources(this.upgradeTimeTextBox, "upgradeTimeTextBox");
            this.upgradeTimeTextBox.Name = "upgradeTimeTextBox";
            // 
            // upgradeTimeLabel
            // 
            resources.ApplyResources(this.upgradeTimeLabel, "upgradeTimeLabel");
            this.upgradeTimeLabel.Name = "upgradeTimeLabel";
            // 
            // upgradeCostTextBox
            // 
            resources.ApplyResources(this.upgradeCostTextBox, "upgradeCostTextBox");
            this.upgradeCostTextBox.Name = "upgradeCostTextBox";
            // 
            // upgradeCostLabel
            // 
            resources.ApplyResources(this.upgradeCostLabel, "upgradeCostLabel");
            this.upgradeCostLabel.Name = "upgradeCostLabel";
            // 
            // upgradeTypeComboBox
            // 
            this.upgradeTypeComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.upgradeTypeComboBox, "upgradeTypeComboBox");
            this.upgradeTypeComboBox.Name = "upgradeTypeComboBox";
            // 
            // upgradeTypeLabel
            // 
            resources.ApplyResources(this.upgradeTypeLabel, "upgradeTypeLabel");
            this.upgradeTypeLabel.Name = "upgradeTypeLabel";
            // 
            // upgradeRemoveButton
            // 
            resources.ApplyResources(this.upgradeRemoveButton, "upgradeRemoveButton");
            this.upgradeRemoveButton.Name = "upgradeRemoveButton";
            this.upgradeRemoveButton.UseVisualStyleBackColor = true;
            // 
            // upgradeAddButton
            // 
            resources.ApplyResources(this.upgradeAddButton, "upgradeAddButton");
            this.upgradeAddButton.Name = "upgradeAddButton";
            this.upgradeAddButton.UseVisualStyleBackColor = true;
            // 
            // upgradeListView
            // 
            this.upgradeListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.upgradeTypeColumnHeader,
            this.upgradeCostColumnHeader,
            this.upgradeTimeColumnHeader});
            resources.ApplyResources(this.upgradeListView, "upgradeListView");
            this.upgradeListView.Name = "upgradeListView";
            this.upgradeListView.UseCompatibleStateImageBehavior = false;
            this.upgradeListView.View = System.Windows.Forms.View.Details;
            // 
            // upgradeTypeColumnHeader
            // 
            resources.ApplyResources(this.upgradeTypeColumnHeader, "upgradeTypeColumnHeader");
            // 
            // upgradeCostColumnHeader
            // 
            resources.ApplyResources(this.upgradeCostColumnHeader, "upgradeCostColumnHeader");
            // 
            // upgradeTimeColumnHeader
            // 
            resources.ApplyResources(this.upgradeTimeColumnHeader, "upgradeTimeColumnHeader");
            // 
            // militaryValueNumericUpDown
            // 
            resources.ApplyResources(this.militaryValueNumericUpDown, "militaryValueNumericUpDown");
            this.militaryValueNumericUpDown.Name = "militaryValueNumericUpDown";
            // 
            // militaryValueLabel
            // 
            resources.ApplyResources(this.militaryValueLabel, "militaryValueLabel");
            this.militaryValueLabel.Name = "militaryValueLabel";
            // 
            // transmuteComboBox
            // 
            this.transmuteComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.transmuteComboBox, "transmuteComboBox");
            this.transmuteComboBox.Name = "transmuteComboBox";
            // 
            // transmuteLabel
            // 
            resources.ApplyResources(this.transmuteLabel, "transmuteLabel");
            this.transmuteLabel.Name = "transmuteLabel";
            // 
            // spriteTypeComboBox
            // 
            this.spriteTypeComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.spriteTypeComboBox, "spriteTypeComboBox");
            this.spriteTypeComboBox.Name = "spriteTypeComboBox";
            // 
            // spriteTypeLabel
            // 
            resources.ApplyResources(this.spriteTypeLabel, "spriteTypeLabel");
            this.spriteTypeLabel.Name = "spriteTypeLabel";
            // 
            // realUnitTypeComboBox
            // 
            this.realUnitTypeComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.realUnitTypeComboBox, "realUnitTypeComboBox");
            this.realUnitTypeComboBox.Name = "realUnitTypeComboBox";
            // 
            // realUnitTypeLabel
            // 
            resources.ApplyResources(this.realUnitTypeLabel, "realUnitTypeLabel");
            this.realUnitTypeLabel.Name = "realUnitTypeLabel";
            // 
            // productableCheckBox
            // 
            resources.ApplyResources(this.productableCheckBox, "productableCheckBox");
            this.productableCheckBox.Name = "productableCheckBox";
            this.productableCheckBox.UseVisualStyleBackColor = true;
            // 
            // gfxPrioNumericUpDown
            // 
            resources.ApplyResources(this.gfxPrioNumericUpDown, "gfxPrioNumericUpDown");
            this.gfxPrioNumericUpDown.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.gfxPrioNumericUpDown.Name = "gfxPrioNumericUpDown";
            // 
            // gfxPrioLabel
            // 
            resources.ApplyResources(this.gfxPrioLabel, "gfxPrioLabel");
            this.gfxPrioLabel.Name = "gfxPrioLabel";
            // 
            // detachableCheckBox
            // 
            resources.ApplyResources(this.detachableCheckBox, "detachableCheckBox");
            this.detachableCheckBox.Name = "detachableCheckBox";
            this.detachableCheckBox.UseVisualStyleBackColor = true;
            // 
            // allowedBrigadesLabel
            // 
            resources.ApplyResources(this.allowedBrigadesLabel, "allowedBrigadesLabel");
            this.allowedBrigadesLabel.Name = "allowedBrigadesLabel";
            // 
            // branchLabel
            // 
            resources.ApplyResources(this.branchLabel, "branchLabel");
            this.branchLabel.Name = "branchLabel";
            // 
            // branchComboBox
            // 
            this.branchComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.branchComboBox, "branchComboBox");
            this.branchComboBox.Name = "branchComboBox";
            // 
            // allowedBrigadesCheckedListBox
            // 
            this.allowedBrigadesCheckedListBox.FormattingEnabled = true;
            resources.ApplyResources(this.allowedBrigadesCheckedListBox, "allowedBrigadesCheckedListBox");
            this.allowedBrigadesCheckedListBox.MultiColumn = true;
            this.allowedBrigadesCheckedListBox.Name = "allowedBrigadesCheckedListBox";
            // 
            // classShortDescTextBox
            // 
            resources.ApplyResources(this.classShortDescTextBox, "classShortDescTextBox");
            this.classShortDescTextBox.Name = "classShortDescTextBox";
            // 
            // classShortDescLabel
            // 
            resources.ApplyResources(this.classShortDescLabel, "classShortDescLabel");
            this.classShortDescLabel.Name = "classShortDescLabel";
            // 
            // classDescTextBox
            // 
            resources.ApplyResources(this.classDescTextBox, "classDescTextBox");
            this.classDescTextBox.Name = "classDescTextBox";
            // 
            // classDescLabel
            // 
            resources.ApplyResources(this.classDescLabel, "classDescLabel");
            this.classDescLabel.Name = "classDescLabel";
            // 
            // classShortNameTextBox
            // 
            resources.ApplyResources(this.classShortNameTextBox, "classShortNameTextBox");
            this.classShortNameTextBox.Name = "classShortNameTextBox";
            // 
            // classShortNameLabel
            // 
            resources.ApplyResources(this.classShortNameLabel, "classShortNameLabel");
            this.classShortNameLabel.Name = "classShortNameLabel";
            // 
            // classNameTextBox
            // 
            resources.ApplyResources(this.classNameTextBox, "classNameTextBox");
            this.classNameTextBox.Name = "classNameTextBox";
            // 
            // classNameLabel
            // 
            resources.ApplyResources(this.classNameLabel, "classNameLabel");
            this.classNameLabel.Name = "classNameLabel";
            // 
            // modelTabPage
            // 
            this.modelTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.modelTabPage.Controls.Add(this.modelNameTextBox);
            this.modelTabPage.Controls.Add(this.equipmentGroupBox);
            this.modelTabPage.Controls.Add(this.productionGroupBox);
            this.modelTabPage.Controls.Add(this.speedGroupBox);
            this.modelTabPage.Controls.Add(this.battleGroupBox);
            this.modelTabPage.Controls.Add(this.basicGroupBox);
            this.modelTabPage.Controls.Add(this.modelIconPictureBox);
            this.modelTabPage.Controls.Add(this.modelImagePictureBox);
            resources.ApplyResources(this.modelTabPage, "modelTabPage");
            this.modelTabPage.Name = "modelTabPage";
            // 
            // modelNameTextBox
            // 
            resources.ApplyResources(this.modelNameTextBox, "modelNameTextBox");
            this.modelNameTextBox.Name = "modelNameTextBox";
            this.modelNameTextBox.TextChanged += new System.EventHandler(this.OnModelNameTextBoxTextChanged);
            // 
            // equipmentGroupBox
            // 
            this.equipmentGroupBox.Controls.Add(this.quantityTextBox);
            this.equipmentGroupBox.Controls.Add(this.equipmentDownButton);
            this.equipmentGroupBox.Controls.Add(this.equipmentUpButton);
            this.equipmentGroupBox.Controls.Add(this.quantityLabel);
            this.equipmentGroupBox.Controls.Add(this.resourceComboBox);
            this.equipmentGroupBox.Controls.Add(this.resourceLabel);
            this.equipmentGroupBox.Controls.Add(this.equipmentRemoveButton);
            this.equipmentGroupBox.Controls.Add(this.equipmentAddButton);
            this.equipmentGroupBox.Controls.Add(this.equipmentListView);
            resources.ApplyResources(this.equipmentGroupBox, "equipmentGroupBox");
            this.equipmentGroupBox.Name = "equipmentGroupBox";
            this.equipmentGroupBox.TabStop = false;
            // 
            // quantityTextBox
            // 
            resources.ApplyResources(this.quantityTextBox, "quantityTextBox");
            this.quantityTextBox.Name = "quantityTextBox";
            this.quantityTextBox.Validated += new System.EventHandler(this.OnQuantityTextBoxValidated);
            // 
            // equipmentDownButton
            // 
            resources.ApplyResources(this.equipmentDownButton, "equipmentDownButton");
            this.equipmentDownButton.Name = "equipmentDownButton";
            this.equipmentDownButton.UseVisualStyleBackColor = true;
            this.equipmentDownButton.Click += new System.EventHandler(this.OnEquipmentDownButtonClick);
            // 
            // equipmentUpButton
            // 
            resources.ApplyResources(this.equipmentUpButton, "equipmentUpButton");
            this.equipmentUpButton.Name = "equipmentUpButton";
            this.equipmentUpButton.UseVisualStyleBackColor = true;
            this.equipmentUpButton.Click += new System.EventHandler(this.OnEquipmentUpButtonClick);
            // 
            // quantityLabel
            // 
            resources.ApplyResources(this.quantityLabel, "quantityLabel");
            this.quantityLabel.Name = "quantityLabel";
            // 
            // resourceComboBox
            // 
            this.resourceComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.resourceComboBox, "resourceComboBox");
            this.resourceComboBox.Name = "resourceComboBox";
            this.resourceComboBox.Validated += new System.EventHandler(this.OnResourceComboBoxValidated);
            // 
            // resourceLabel
            // 
            resources.ApplyResources(this.resourceLabel, "resourceLabel");
            this.resourceLabel.Name = "resourceLabel";
            // 
            // equipmentRemoveButton
            // 
            resources.ApplyResources(this.equipmentRemoveButton, "equipmentRemoveButton");
            this.equipmentRemoveButton.Name = "equipmentRemoveButton";
            this.equipmentRemoveButton.UseVisualStyleBackColor = true;
            this.equipmentRemoveButton.Click += new System.EventHandler(this.OnEquipmentRemoveButtonClick);
            // 
            // equipmentAddButton
            // 
            resources.ApplyResources(this.equipmentAddButton, "equipmentAddButton");
            this.equipmentAddButton.Name = "equipmentAddButton";
            this.equipmentAddButton.UseVisualStyleBackColor = true;
            this.equipmentAddButton.Click += new System.EventHandler(this.OnEquipmentAddButtonClick);
            // 
            // equipmentListView
            // 
            this.equipmentListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.resourceColumnHeader,
            this.quantityColumnHeader});
            resources.ApplyResources(this.equipmentListView, "equipmentListView");
            this.equipmentListView.Name = "equipmentListView";
            this.equipmentListView.UseCompatibleStateImageBehavior = false;
            this.equipmentListView.View = System.Windows.Forms.View.Details;
            this.equipmentListView.SelectedIndexChanged += new System.EventHandler(this.OnEquipmentListViewSelectedIndexChanged);
            // 
            // resourceColumnHeader
            // 
            resources.ApplyResources(this.resourceColumnHeader, "resourceColumnHeader");
            // 
            // quantityColumnHeader
            // 
            resources.ApplyResources(this.quantityColumnHeader, "quantityColumnHeader");
            // 
            // productionGroupBox
            // 
            this.productionGroupBox.Controls.Add(this.reinforceCostTextBox);
            this.productionGroupBox.Controls.Add(this.costTextBox);
            this.productionGroupBox.Controls.Add(this.reinforceCostLabel);
            this.productionGroupBox.Controls.Add(this.costLabel);
            this.productionGroupBox.Controls.Add(this.reinforceTimeTextBox);
            this.productionGroupBox.Controls.Add(this.buildTimeLabel);
            this.productionGroupBox.Controls.Add(this.reinforceTimeLabel);
            this.productionGroupBox.Controls.Add(this.buildTimeTextBox);
            this.productionGroupBox.Controls.Add(this.manPowerTextBox);
            this.productionGroupBox.Controls.Add(this.manPowerLabel);
            this.productionGroupBox.Controls.Add(this.upgradeTimeFactorTextBox);
            this.productionGroupBox.Controls.Add(this.upgradeTimeFactorLabel);
            this.productionGroupBox.Controls.Add(this.upgradeCostFactorLabel);
            this.productionGroupBox.Controls.Add(this.upgradeCostFactorTextBox);
            resources.ApplyResources(this.productionGroupBox, "productionGroupBox");
            this.productionGroupBox.Name = "productionGroupBox";
            this.productionGroupBox.TabStop = false;
            // 
            // reinforceCostTextBox
            // 
            resources.ApplyResources(this.reinforceCostTextBox, "reinforceCostTextBox");
            this.reinforceCostTextBox.Name = "reinforceCostTextBox";
            this.reinforceCostTextBox.Validated += new System.EventHandler(this.OnReinforceCostTextBoxValidated);
            // 
            // costTextBox
            // 
            resources.ApplyResources(this.costTextBox, "costTextBox");
            this.costTextBox.Name = "costTextBox";
            this.costTextBox.Validated += new System.EventHandler(this.OnCostTextBoxValidated);
            // 
            // reinforceCostLabel
            // 
            resources.ApplyResources(this.reinforceCostLabel, "reinforceCostLabel");
            this.reinforceCostLabel.Name = "reinforceCostLabel";
            // 
            // costLabel
            // 
            resources.ApplyResources(this.costLabel, "costLabel");
            this.costLabel.Name = "costLabel";
            // 
            // reinforceTimeTextBox
            // 
            resources.ApplyResources(this.reinforceTimeTextBox, "reinforceTimeTextBox");
            this.reinforceTimeTextBox.Name = "reinforceTimeTextBox";
            this.reinforceTimeTextBox.Validated += new System.EventHandler(this.OnReinforceTimeTextBoxValidated);
            // 
            // buildTimeLabel
            // 
            resources.ApplyResources(this.buildTimeLabel, "buildTimeLabel");
            this.buildTimeLabel.Name = "buildTimeLabel";
            // 
            // reinforceTimeLabel
            // 
            resources.ApplyResources(this.reinforceTimeLabel, "reinforceTimeLabel");
            this.reinforceTimeLabel.Name = "reinforceTimeLabel";
            // 
            // buildTimeTextBox
            // 
            resources.ApplyResources(this.buildTimeTextBox, "buildTimeTextBox");
            this.buildTimeTextBox.Name = "buildTimeTextBox";
            this.buildTimeTextBox.Validated += new System.EventHandler(this.OnBuildTimeTextBoxValidated);
            // 
            // manPowerTextBox
            // 
            resources.ApplyResources(this.manPowerTextBox, "manPowerTextBox");
            this.manPowerTextBox.Name = "manPowerTextBox";
            this.manPowerTextBox.Validated += new System.EventHandler(this.OnManPowerTextBoxValidated);
            // 
            // manPowerLabel
            // 
            resources.ApplyResources(this.manPowerLabel, "manPowerLabel");
            this.manPowerLabel.Name = "manPowerLabel";
            // 
            // upgradeTimeFactorTextBox
            // 
            resources.ApplyResources(this.upgradeTimeFactorTextBox, "upgradeTimeFactorTextBox");
            this.upgradeTimeFactorTextBox.Name = "upgradeTimeFactorTextBox";
            this.upgradeTimeFactorTextBox.Validated += new System.EventHandler(this.OnUpgradeTimeFactorTextBoxValidated);
            // 
            // upgradeTimeFactorLabel
            // 
            resources.ApplyResources(this.upgradeTimeFactorLabel, "upgradeTimeFactorLabel");
            this.upgradeTimeFactorLabel.Name = "upgradeTimeFactorLabel";
            // 
            // upgradeCostFactorLabel
            // 
            resources.ApplyResources(this.upgradeCostFactorLabel, "upgradeCostFactorLabel");
            this.upgradeCostFactorLabel.Name = "upgradeCostFactorLabel";
            // 
            // upgradeCostFactorTextBox
            // 
            resources.ApplyResources(this.upgradeCostFactorTextBox, "upgradeCostFactorTextBox");
            this.upgradeCostFactorTextBox.Name = "upgradeCostFactorTextBox";
            this.upgradeCostFactorTextBox.Validated += new System.EventHandler(this.OnUpgradeCostFactorTextBoxValidated);
            // 
            // speedGroupBox
            // 
            this.speedGroupBox.Controls.Add(this.speedCapAaTextBox);
            this.speedGroupBox.Controls.Add(this.maxSpeedTextBox);
            this.speedGroupBox.Controls.Add(this.speedCapAaLabel);
            this.speedGroupBox.Controls.Add(this.maxSpeedLabel);
            this.speedGroupBox.Controls.Add(this.speedCapAtTextBox);
            this.speedGroupBox.Controls.Add(this.speedCapAtLabel);
            this.speedGroupBox.Controls.Add(this.speedCapEngTextBox);
            this.speedGroupBox.Controls.Add(this.speedCapAllTextBox);
            this.speedGroupBox.Controls.Add(this.speedCapEngLabel);
            this.speedGroupBox.Controls.Add(this.speedCapAllLabel);
            this.speedGroupBox.Controls.Add(this.speedCapArtTextBox);
            this.speedGroupBox.Controls.Add(this.speedCapArtLabel);
            resources.ApplyResources(this.speedGroupBox, "speedGroupBox");
            this.speedGroupBox.Name = "speedGroupBox";
            this.speedGroupBox.TabStop = false;
            // 
            // speedCapAaTextBox
            // 
            resources.ApplyResources(this.speedCapAaTextBox, "speedCapAaTextBox");
            this.speedCapAaTextBox.Name = "speedCapAaTextBox";
            this.speedCapAaTextBox.Validated += new System.EventHandler(this.OnSpeedCapAaTextBoxValidated);
            // 
            // maxSpeedTextBox
            // 
            resources.ApplyResources(this.maxSpeedTextBox, "maxSpeedTextBox");
            this.maxSpeedTextBox.Name = "maxSpeedTextBox";
            this.maxSpeedTextBox.Validated += new System.EventHandler(this.OnMaxSpeedTextBoxValidated);
            // 
            // speedCapAaLabel
            // 
            resources.ApplyResources(this.speedCapAaLabel, "speedCapAaLabel");
            this.speedCapAaLabel.Name = "speedCapAaLabel";
            // 
            // maxSpeedLabel
            // 
            resources.ApplyResources(this.maxSpeedLabel, "maxSpeedLabel");
            this.maxSpeedLabel.Name = "maxSpeedLabel";
            // 
            // speedCapAtTextBox
            // 
            resources.ApplyResources(this.speedCapAtTextBox, "speedCapAtTextBox");
            this.speedCapAtTextBox.Name = "speedCapAtTextBox";
            this.speedCapAtTextBox.Validated += new System.EventHandler(this.OnSpeedCapAtTextBoxValidated);
            // 
            // speedCapAtLabel
            // 
            resources.ApplyResources(this.speedCapAtLabel, "speedCapAtLabel");
            this.speedCapAtLabel.Name = "speedCapAtLabel";
            // 
            // speedCapEngTextBox
            // 
            resources.ApplyResources(this.speedCapEngTextBox, "speedCapEngTextBox");
            this.speedCapEngTextBox.Name = "speedCapEngTextBox";
            this.speedCapEngTextBox.Validated += new System.EventHandler(this.OnSpeedCapEngTextBoxValidated);
            // 
            // speedCapAllTextBox
            // 
            resources.ApplyResources(this.speedCapAllTextBox, "speedCapAllTextBox");
            this.speedCapAllTextBox.Name = "speedCapAllTextBox";
            this.speedCapAllTextBox.Validated += new System.EventHandler(this.OnSpeedCapTextBoxValidated);
            // 
            // speedCapEngLabel
            // 
            resources.ApplyResources(this.speedCapEngLabel, "speedCapEngLabel");
            this.speedCapEngLabel.Name = "speedCapEngLabel";
            // 
            // speedCapAllLabel
            // 
            resources.ApplyResources(this.speedCapAllLabel, "speedCapAllLabel");
            this.speedCapAllLabel.Name = "speedCapAllLabel";
            // 
            // speedCapArtTextBox
            // 
            resources.ApplyResources(this.speedCapArtTextBox, "speedCapArtTextBox");
            this.speedCapArtTextBox.Name = "speedCapArtTextBox";
            this.speedCapArtTextBox.Validated += new System.EventHandler(this.OnSpeedCapArtTextBox);
            // 
            // speedCapArtLabel
            // 
            resources.ApplyResources(this.speedCapArtLabel, "speedCapArtLabel");
            this.speedCapArtLabel.Name = "speedCapArtLabel";
            // 
            // battleGroupBox
            // 
            this.battleGroupBox.Controls.Add(this.artilleryBombardmentTextBox);
            this.battleGroupBox.Controls.Add(this.artilleryBombardmentLabel);
            this.battleGroupBox.Controls.Add(this.visibilityTextBox);
            this.battleGroupBox.Controls.Add(this.visibilityLabel);
            this.battleGroupBox.Controls.Add(this.noFuelCombatModTextBox);
            this.battleGroupBox.Controls.Add(this.noFuelCombatModLabel);
            this.battleGroupBox.Controls.Add(this.airDetectionCapabilityTextBox);
            this.battleGroupBox.Controls.Add(this.airDetectionCapabilityLabel);
            this.battleGroupBox.Controls.Add(this.subDetectionCapabilityTextBox);
            this.battleGroupBox.Controls.Add(this.subDetectionCapabilityLabel);
            this.battleGroupBox.Controls.Add(this.surfaceDetectionCapabilityTextBox);
            this.battleGroupBox.Controls.Add(this.surfaceDetectionCapabilityLabel);
            this.battleGroupBox.Controls.Add(this.distanceTextBox);
            this.battleGroupBox.Controls.Add(this.distanceLabel);
            this.battleGroupBox.Controls.Add(this.strategicAttackTextBox);
            this.battleGroupBox.Controls.Add(this.strategicAttackLabel);
            this.battleGroupBox.Controls.Add(this.navalAttackTextBox);
            this.battleGroupBox.Controls.Add(this.navalAttackLabel);
            this.battleGroupBox.Controls.Add(this.airAttackTextBox);
            this.battleGroupBox.Controls.Add(this.airAttackLabel);
            this.battleGroupBox.Controls.Add(this.shoreBombardmentTextBox);
            this.battleGroupBox.Controls.Add(this.shoreBombardmentLabel);
            this.battleGroupBox.Controls.Add(this.convoyAttackTextBox);
            this.battleGroupBox.Controls.Add(this.convoyAttackLabel);
            this.battleGroupBox.Controls.Add(this.subAttackTextBox);
            this.battleGroupBox.Controls.Add(this.subAttackLabel);
            this.battleGroupBox.Controls.Add(this.seaAttackTextBox);
            this.battleGroupBox.Controls.Add(this.seaAttackLabel);
            this.battleGroupBox.Controls.Add(this.hardAttackTextBox);
            this.battleGroupBox.Controls.Add(this.hardAttackLabel);
            this.battleGroupBox.Controls.Add(this.softAttackTextBox);
            this.battleGroupBox.Controls.Add(this.softAttackLabel);
            this.battleGroupBox.Controls.Add(this.softnessTextBox);
            this.battleGroupBox.Controls.Add(this.softnessLabel);
            this.battleGroupBox.Controls.Add(this.toughnessTextBox);
            this.battleGroupBox.Controls.Add(this.toughnessLabel);
            this.battleGroupBox.Controls.Add(this.surfaceDefenceTextBox);
            this.battleGroupBox.Controls.Add(this.surfaceDefenceLabel);
            this.battleGroupBox.Controls.Add(this.airDefenceTextBox);
            this.battleGroupBox.Controls.Add(this.airDefenceLabel);
            this.battleGroupBox.Controls.Add(this.seaDefenceTextBox);
            this.battleGroupBox.Controls.Add(this.seaDefenceLabel);
            this.battleGroupBox.Controls.Add(this.defensivenessTextBox);
            this.battleGroupBox.Controls.Add(this.defensivenessLabel);
            resources.ApplyResources(this.battleGroupBox, "battleGroupBox");
            this.battleGroupBox.Name = "battleGroupBox";
            this.battleGroupBox.TabStop = false;
            // 
            // artilleryBombardmentTextBox
            // 
            resources.ApplyResources(this.artilleryBombardmentTextBox, "artilleryBombardmentTextBox");
            this.artilleryBombardmentTextBox.Name = "artilleryBombardmentTextBox";
            this.artilleryBombardmentTextBox.Validated += new System.EventHandler(this.OnArtilleryBombardmentTextBoxValidated);
            // 
            // artilleryBombardmentLabel
            // 
            resources.ApplyResources(this.artilleryBombardmentLabel, "artilleryBombardmentLabel");
            this.artilleryBombardmentLabel.Name = "artilleryBombardmentLabel";
            // 
            // visibilityTextBox
            // 
            resources.ApplyResources(this.visibilityTextBox, "visibilityTextBox");
            this.visibilityTextBox.Name = "visibilityTextBox";
            this.visibilityTextBox.Validated += new System.EventHandler(this.OnVisibilityTextBoxValidated);
            // 
            // visibilityLabel
            // 
            resources.ApplyResources(this.visibilityLabel, "visibilityLabel");
            this.visibilityLabel.Name = "visibilityLabel";
            // 
            // noFuelCombatModTextBox
            // 
            resources.ApplyResources(this.noFuelCombatModTextBox, "noFuelCombatModTextBox");
            this.noFuelCombatModTextBox.Name = "noFuelCombatModTextBox";
            this.noFuelCombatModTextBox.Validated += new System.EventHandler(this.OnNoFuelCombatModTextBoxValidated);
            // 
            // noFuelCombatModLabel
            // 
            resources.ApplyResources(this.noFuelCombatModLabel, "noFuelCombatModLabel");
            this.noFuelCombatModLabel.Name = "noFuelCombatModLabel";
            // 
            // airDetectionCapabilityTextBox
            // 
            resources.ApplyResources(this.airDetectionCapabilityTextBox, "airDetectionCapabilityTextBox");
            this.airDetectionCapabilityTextBox.Name = "airDetectionCapabilityTextBox";
            this.airDetectionCapabilityTextBox.Validated += new System.EventHandler(this.OnAirDetectionCapabilityTextBoxValidated);
            // 
            // airDetectionCapabilityLabel
            // 
            resources.ApplyResources(this.airDetectionCapabilityLabel, "airDetectionCapabilityLabel");
            this.airDetectionCapabilityLabel.Name = "airDetectionCapabilityLabel";
            // 
            // subDetectionCapabilityTextBox
            // 
            resources.ApplyResources(this.subDetectionCapabilityTextBox, "subDetectionCapabilityTextBox");
            this.subDetectionCapabilityTextBox.Name = "subDetectionCapabilityTextBox";
            this.subDetectionCapabilityTextBox.Validated += new System.EventHandler(this.OnSubDetectionCapabilityTextBoxValidated);
            // 
            // subDetectionCapabilityLabel
            // 
            resources.ApplyResources(this.subDetectionCapabilityLabel, "subDetectionCapabilityLabel");
            this.subDetectionCapabilityLabel.Name = "subDetectionCapabilityLabel";
            // 
            // surfaceDetectionCapabilityTextBox
            // 
            resources.ApplyResources(this.surfaceDetectionCapabilityTextBox, "surfaceDetectionCapabilityTextBox");
            this.surfaceDetectionCapabilityTextBox.Name = "surfaceDetectionCapabilityTextBox";
            this.surfaceDetectionCapabilityTextBox.Validated += new System.EventHandler(this.OnSurfaceDetectionCapabilityTextBoxValidated);
            // 
            // surfaceDetectionCapabilityLabel
            // 
            resources.ApplyResources(this.surfaceDetectionCapabilityLabel, "surfaceDetectionCapabilityLabel");
            this.surfaceDetectionCapabilityLabel.Name = "surfaceDetectionCapabilityLabel";
            // 
            // distanceTextBox
            // 
            resources.ApplyResources(this.distanceTextBox, "distanceTextBox");
            this.distanceTextBox.Name = "distanceTextBox";
            this.distanceTextBox.Validated += new System.EventHandler(this.OnDistanceTextBoxValidated);
            // 
            // distanceLabel
            // 
            resources.ApplyResources(this.distanceLabel, "distanceLabel");
            this.distanceLabel.Name = "distanceLabel";
            // 
            // strategicAttackTextBox
            // 
            resources.ApplyResources(this.strategicAttackTextBox, "strategicAttackTextBox");
            this.strategicAttackTextBox.Name = "strategicAttackTextBox";
            this.strategicAttackTextBox.Validated += new System.EventHandler(this.OnStrategicAttackTextBoxValidated);
            // 
            // strategicAttackLabel
            // 
            resources.ApplyResources(this.strategicAttackLabel, "strategicAttackLabel");
            this.strategicAttackLabel.Name = "strategicAttackLabel";
            // 
            // navalAttackTextBox
            // 
            resources.ApplyResources(this.navalAttackTextBox, "navalAttackTextBox");
            this.navalAttackTextBox.Name = "navalAttackTextBox";
            this.navalAttackTextBox.Validated += new System.EventHandler(this.OnNavalAttackTextBoxValidated);
            // 
            // navalAttackLabel
            // 
            resources.ApplyResources(this.navalAttackLabel, "navalAttackLabel");
            this.navalAttackLabel.Name = "navalAttackLabel";
            // 
            // airAttackTextBox
            // 
            resources.ApplyResources(this.airAttackTextBox, "airAttackTextBox");
            this.airAttackTextBox.Name = "airAttackTextBox";
            this.airAttackTextBox.Validated += new System.EventHandler(this.OnAirAttackTextBoxValidated);
            // 
            // airAttackLabel
            // 
            resources.ApplyResources(this.airAttackLabel, "airAttackLabel");
            this.airAttackLabel.Name = "airAttackLabel";
            // 
            // shoreBombardmentTextBox
            // 
            resources.ApplyResources(this.shoreBombardmentTextBox, "shoreBombardmentTextBox");
            this.shoreBombardmentTextBox.Name = "shoreBombardmentTextBox";
            this.shoreBombardmentTextBox.Validated += new System.EventHandler(this.OnShoreBombardmentTextBoxValidated);
            // 
            // shoreBombardmentLabel
            // 
            resources.ApplyResources(this.shoreBombardmentLabel, "shoreBombardmentLabel");
            this.shoreBombardmentLabel.Name = "shoreBombardmentLabel";
            // 
            // convoyAttackTextBox
            // 
            resources.ApplyResources(this.convoyAttackTextBox, "convoyAttackTextBox");
            this.convoyAttackTextBox.Name = "convoyAttackTextBox";
            // 
            // convoyAttackLabel
            // 
            resources.ApplyResources(this.convoyAttackLabel, "convoyAttackLabel");
            this.convoyAttackLabel.Name = "convoyAttackLabel";
            // 
            // subAttackTextBox
            // 
            resources.ApplyResources(this.subAttackTextBox, "subAttackTextBox");
            this.subAttackTextBox.Name = "subAttackTextBox";
            this.subAttackTextBox.Validated += new System.EventHandler(this.OnSubAttackTextBoxValidated);
            // 
            // subAttackLabel
            // 
            resources.ApplyResources(this.subAttackLabel, "subAttackLabel");
            this.subAttackLabel.Name = "subAttackLabel";
            // 
            // seaAttackTextBox
            // 
            resources.ApplyResources(this.seaAttackTextBox, "seaAttackTextBox");
            this.seaAttackTextBox.Name = "seaAttackTextBox";
            this.seaAttackTextBox.Validated += new System.EventHandler(this.OnSeaAttackTextBoxValidated);
            // 
            // seaAttackLabel
            // 
            resources.ApplyResources(this.seaAttackLabel, "seaAttackLabel");
            this.seaAttackLabel.Name = "seaAttackLabel";
            // 
            // hardAttackTextBox
            // 
            resources.ApplyResources(this.hardAttackTextBox, "hardAttackTextBox");
            this.hardAttackTextBox.Name = "hardAttackTextBox";
            this.hardAttackTextBox.Validated += new System.EventHandler(this.OnHardAttackTextBoxValidated);
            // 
            // hardAttackLabel
            // 
            resources.ApplyResources(this.hardAttackLabel, "hardAttackLabel");
            this.hardAttackLabel.Name = "hardAttackLabel";
            // 
            // softAttackTextBox
            // 
            resources.ApplyResources(this.softAttackTextBox, "softAttackTextBox");
            this.softAttackTextBox.Name = "softAttackTextBox";
            this.softAttackTextBox.Validated += new System.EventHandler(this.OnSoftAttackTextBoxValidated);
            // 
            // softAttackLabel
            // 
            resources.ApplyResources(this.softAttackLabel, "softAttackLabel");
            this.softAttackLabel.Name = "softAttackLabel";
            // 
            // softnessTextBox
            // 
            resources.ApplyResources(this.softnessTextBox, "softnessTextBox");
            this.softnessTextBox.Name = "softnessTextBox";
            this.softnessTextBox.Validated += new System.EventHandler(this.OnSoftnessTextBoxValidated);
            // 
            // softnessLabel
            // 
            resources.ApplyResources(this.softnessLabel, "softnessLabel");
            this.softnessLabel.Name = "softnessLabel";
            // 
            // toughnessTextBox
            // 
            resources.ApplyResources(this.toughnessTextBox, "toughnessTextBox");
            this.toughnessTextBox.Name = "toughnessTextBox";
            this.toughnessTextBox.Validated += new System.EventHandler(this.OnToughnessTextBoxValidated);
            // 
            // toughnessLabel
            // 
            resources.ApplyResources(this.toughnessLabel, "toughnessLabel");
            this.toughnessLabel.Name = "toughnessLabel";
            // 
            // surfaceDefenceTextBox
            // 
            resources.ApplyResources(this.surfaceDefenceTextBox, "surfaceDefenceTextBox");
            this.surfaceDefenceTextBox.Name = "surfaceDefenceTextBox";
            // 
            // surfaceDefenceLabel
            // 
            resources.ApplyResources(this.surfaceDefenceLabel, "surfaceDefenceLabel");
            this.surfaceDefenceLabel.Name = "surfaceDefenceLabel";
            this.surfaceDefenceLabel.Validated += new System.EventHandler(this.OnSurfaceDefenceTextBoxValidated);
            // 
            // airDefenceTextBox
            // 
            resources.ApplyResources(this.airDefenceTextBox, "airDefenceTextBox");
            this.airDefenceTextBox.Name = "airDefenceTextBox";
            this.airDefenceTextBox.Validated += new System.EventHandler(this.OnAirDefenceTextBoxValidated);
            // 
            // airDefenceLabel
            // 
            resources.ApplyResources(this.airDefenceLabel, "airDefenceLabel");
            this.airDefenceLabel.Name = "airDefenceLabel";
            // 
            // seaDefenceTextBox
            // 
            resources.ApplyResources(this.seaDefenceTextBox, "seaDefenceTextBox");
            this.seaDefenceTextBox.Name = "seaDefenceTextBox";
            this.seaDefenceTextBox.Validated += new System.EventHandler(this.OnSeaDefenceTextBoxValidated);
            // 
            // seaDefenceLabel
            // 
            resources.ApplyResources(this.seaDefenceLabel, "seaDefenceLabel");
            this.seaDefenceLabel.Name = "seaDefenceLabel";
            // 
            // defensivenessTextBox
            // 
            resources.ApplyResources(this.defensivenessTextBox, "defensivenessTextBox");
            this.defensivenessTextBox.Name = "defensivenessTextBox";
            this.defensivenessTextBox.Validated += new System.EventHandler(this.OnDefensivenessTextBoxValidated);
            // 
            // defensivenessLabel
            // 
            resources.ApplyResources(this.defensivenessLabel, "defensivenessLabel");
            this.defensivenessLabel.Name = "defensivenessLabel";
            // 
            // basicGroupBox
            // 
            this.basicGroupBox.Controls.Add(this.transportWeightTextBox);
            this.basicGroupBox.Controls.Add(this.rangeLabel);
            this.basicGroupBox.Controls.Add(this.transportWeightLabel);
            this.basicGroupBox.Controls.Add(this.maxOilStockTextBox);
            this.basicGroupBox.Controls.Add(this.rangeTextBox);
            this.basicGroupBox.Controls.Add(this.transportCapabilityLabel);
            this.basicGroupBox.Controls.Add(this.maxOilStockLabel);
            this.basicGroupBox.Controls.Add(this.transportCapabilityTextBox);
            this.basicGroupBox.Controls.Add(this.maxSupplyStockTextBox);
            this.basicGroupBox.Controls.Add(this.maxSupplyStockLabel);
            this.basicGroupBox.Controls.Add(this.fuelConsumptionTextBox);
            this.basicGroupBox.Controls.Add(this.fuelConsumptionLabel);
            this.basicGroupBox.Controls.Add(this.supplyConsumptionTextBox);
            this.basicGroupBox.Controls.Add(this.supplyConsumptionLabel);
            this.basicGroupBox.Controls.Add(this.suppressionTextBox);
            this.basicGroupBox.Controls.Add(this.suppressionLabel);
            this.basicGroupBox.Controls.Add(this.moraleTextBox);
            this.basicGroupBox.Controls.Add(this.moraleLabel);
            this.basicGroupBox.Controls.Add(this.defaultOrganisationTextBox);
            this.basicGroupBox.Controls.Add(this.defaultOrganisationLabel);
            resources.ApplyResources(this.basicGroupBox, "basicGroupBox");
            this.basicGroupBox.Name = "basicGroupBox";
            this.basicGroupBox.TabStop = false;
            // 
            // transportWeightTextBox
            // 
            resources.ApplyResources(this.transportWeightTextBox, "transportWeightTextBox");
            this.transportWeightTextBox.Name = "transportWeightTextBox";
            this.transportWeightTextBox.Validated += new System.EventHandler(this.OnTransportWeightTextBoxValidated);
            // 
            // rangeLabel
            // 
            resources.ApplyResources(this.rangeLabel, "rangeLabel");
            this.rangeLabel.Name = "rangeLabel";
            // 
            // transportWeightLabel
            // 
            resources.ApplyResources(this.transportWeightLabel, "transportWeightLabel");
            this.transportWeightLabel.Name = "transportWeightLabel";
            // 
            // maxOilStockTextBox
            // 
            resources.ApplyResources(this.maxOilStockTextBox, "maxOilStockTextBox");
            this.maxOilStockTextBox.Name = "maxOilStockTextBox";
            this.maxOilStockTextBox.Validated += new System.EventHandler(this.OnMaxOilStockTextBoxValidated);
            // 
            // rangeTextBox
            // 
            resources.ApplyResources(this.rangeTextBox, "rangeTextBox");
            this.rangeTextBox.Name = "rangeTextBox";
            this.rangeTextBox.Validated += new System.EventHandler(this.OnRangeTextBoxValidated);
            // 
            // transportCapabilityLabel
            // 
            resources.ApplyResources(this.transportCapabilityLabel, "transportCapabilityLabel");
            this.transportCapabilityLabel.Name = "transportCapabilityLabel";
            // 
            // maxOilStockLabel
            // 
            resources.ApplyResources(this.maxOilStockLabel, "maxOilStockLabel");
            this.maxOilStockLabel.Name = "maxOilStockLabel";
            // 
            // transportCapabilityTextBox
            // 
            resources.ApplyResources(this.transportCapabilityTextBox, "transportCapabilityTextBox");
            this.transportCapabilityTextBox.Name = "transportCapabilityTextBox";
            this.transportCapabilityTextBox.Validated += new System.EventHandler(this.OnTransportCapabilityTextBoxTextChanged);
            // 
            // maxSupplyStockTextBox
            // 
            resources.ApplyResources(this.maxSupplyStockTextBox, "maxSupplyStockTextBox");
            this.maxSupplyStockTextBox.Name = "maxSupplyStockTextBox";
            this.maxSupplyStockTextBox.Validated += new System.EventHandler(this.OnMaxSupplyStockTextBoxValidated);
            // 
            // maxSupplyStockLabel
            // 
            resources.ApplyResources(this.maxSupplyStockLabel, "maxSupplyStockLabel");
            this.maxSupplyStockLabel.Name = "maxSupplyStockLabel";
            // 
            // fuelConsumptionTextBox
            // 
            resources.ApplyResources(this.fuelConsumptionTextBox, "fuelConsumptionTextBox");
            this.fuelConsumptionTextBox.Name = "fuelConsumptionTextBox";
            this.fuelConsumptionTextBox.Validated += new System.EventHandler(this.OnFuelConsumptionTextBox);
            // 
            // fuelConsumptionLabel
            // 
            resources.ApplyResources(this.fuelConsumptionLabel, "fuelConsumptionLabel");
            this.fuelConsumptionLabel.Name = "fuelConsumptionLabel";
            // 
            // supplyConsumptionTextBox
            // 
            resources.ApplyResources(this.supplyConsumptionTextBox, "supplyConsumptionTextBox");
            this.supplyConsumptionTextBox.Name = "supplyConsumptionTextBox";
            this.supplyConsumptionTextBox.Validated += new System.EventHandler(this.OnSupplyConsumptionTextBoxValidated);
            // 
            // supplyConsumptionLabel
            // 
            resources.ApplyResources(this.supplyConsumptionLabel, "supplyConsumptionLabel");
            this.supplyConsumptionLabel.Name = "supplyConsumptionLabel";
            // 
            // suppressionTextBox
            // 
            resources.ApplyResources(this.suppressionTextBox, "suppressionTextBox");
            this.suppressionTextBox.Name = "suppressionTextBox";
            this.suppressionTextBox.Validated += new System.EventHandler(this.OnSuppressionTextBoxValidated);
            // 
            // suppressionLabel
            // 
            resources.ApplyResources(this.suppressionLabel, "suppressionLabel");
            this.suppressionLabel.Name = "suppressionLabel";
            // 
            // moraleTextBox
            // 
            resources.ApplyResources(this.moraleTextBox, "moraleTextBox");
            this.moraleTextBox.Name = "moraleTextBox";
            this.moraleTextBox.Validated += new System.EventHandler(this.OnMoraleTextBoxValidated);
            // 
            // moraleLabel
            // 
            resources.ApplyResources(this.moraleLabel, "moraleLabel");
            this.moraleLabel.Name = "moraleLabel";
            // 
            // defaultOrganisationTextBox
            // 
            resources.ApplyResources(this.defaultOrganisationTextBox, "defaultOrganisationTextBox");
            this.defaultOrganisationTextBox.Name = "defaultOrganisationTextBox";
            this.defaultOrganisationTextBox.Validated += new System.EventHandler(this.OnDefaultOrganizationTextBoxValidated);
            // 
            // defaultOrganisationLabel
            // 
            resources.ApplyResources(this.defaultOrganisationLabel, "defaultOrganisationLabel");
            this.defaultOrganisationLabel.Name = "defaultOrganisationLabel";
            // 
            // modelIconPictureBox
            // 
            this.modelIconPictureBox.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.modelIconPictureBox, "modelIconPictureBox");
            this.modelIconPictureBox.Name = "modelIconPictureBox";
            this.modelIconPictureBox.TabStop = false;
            // 
            // modelImagePictureBox
            // 
            resources.ApplyResources(this.modelImagePictureBox, "modelImagePictureBox");
            this.modelImagePictureBox.Name = "modelImagePictureBox";
            this.modelImagePictureBox.TabStop = false;
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
            // countryListView
            // 
            this.countryListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.dummyColumnHeader});
            this.countryListView.HideSelection = false;
            resources.ApplyResources(this.countryListView, "countryListView");
            this.countryListView.MultiSelect = false;
            this.countryListView.Name = "countryListView";
            this.countryListView.UseCompatibleStateImageBehavior = false;
            this.countryListView.View = System.Windows.Forms.View.List;
            this.countryListView.SelectedIndexChanged += new System.EventHandler(this.OnCountryListViewSelectedIndexChanged);
            // 
            // dummyColumnHeader
            // 
            resources.ApplyResources(this.dummyColumnHeader, "dummyColumnHeader");
            // 
            // UnitEditorForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.countryListView);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.reloadButton);
            this.Controls.Add(this.editTabControl);
            this.Controls.Add(this.unitListBox);
            this.Controls.Add(this.bottomButton);
            this.Controls.Add(this.downButton);
            this.Controls.Add(this.upButton);
            this.Controls.Add(this.topButton);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.cloneButton);
            this.Controls.Add(this.newButton);
            this.Controls.Add(this.modelListView);
            this.Name = "UnitEditorForm";
            this.Load += new System.EventHandler(this.OnUnitModelEditorFormLoad);
            this.editTabControl.ResumeLayout(false);
            this.classTabPage.ResumeLayout(false);
            this.classTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listPrioNumericUpDown)).EndInit();
            this.upgradeGroupBox.ResumeLayout(false);
            this.upgradeGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.militaryValueNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gfxPrioNumericUpDown)).EndInit();
            this.modelTabPage.ResumeLayout(false);
            this.modelTabPage.PerformLayout();
            this.equipmentGroupBox.ResumeLayout(false);
            this.equipmentGroupBox.PerformLayout();
            this.productionGroupBox.ResumeLayout(false);
            this.productionGroupBox.PerformLayout();
            this.speedGroupBox.ResumeLayout(false);
            this.speedGroupBox.PerformLayout();
            this.battleGroupBox.ResumeLayout(false);
            this.battleGroupBox.PerformLayout();
            this.basicGroupBox.ResumeLayout(false);
            this.basicGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.modelIconPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.modelImagePictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bottomButton;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Button topButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.Button cloneButton;
        private System.Windows.Forms.Button newButton;
        private System.Windows.Forms.ListView modelListView;
        private System.Windows.Forms.ColumnHeader classColumnHeader;
        private System.Windows.Forms.ColumnHeader branchColumnHeader;
        private System.Windows.Forms.ColumnHeader nameColumnHeader;
        private System.Windows.Forms.ColumnHeader costColumnHeader;
        private System.Windows.Forms.ColumnHeader buildTimeColumnHeader;
        private System.Windows.Forms.ColumnHeader manPowerSkillColumnHeader;
        private System.Windows.Forms.ColumnHeader supplyColumnHeader;
        private System.Windows.Forms.ColumnHeader fuelColumnHeader;
        private System.Windows.Forms.ColumnHeader noColumnHeader;
        private System.Windows.Forms.ListBox unitListBox;
        private System.Windows.Forms.TabControl editTabControl;
        private System.Windows.Forms.TabPage classTabPage;
        private System.Windows.Forms.TabPage modelTabPage;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button reloadButton;
        private System.Windows.Forms.PictureBox modelIconPictureBox;
        private System.Windows.Forms.PictureBox modelImagePictureBox;
        private System.Windows.Forms.GroupBox basicGroupBox;
        private System.Windows.Forms.TextBox rangeTextBox;
        private System.Windows.Forms.Label rangeLabel;
        private System.Windows.Forms.TextBox maxSpeedTextBox;
        private System.Windows.Forms.Label maxSpeedLabel;
        private System.Windows.Forms.TextBox manPowerTextBox;
        private System.Windows.Forms.Label manPowerLabel;
        private System.Windows.Forms.TextBox buildTimeTextBox;
        private System.Windows.Forms.Label buildTimeLabel;
        private System.Windows.Forms.TextBox costTextBox;
        private System.Windows.Forms.Label costLabel;
        private System.Windows.Forms.TextBox speedCapAaTextBox;
        private System.Windows.Forms.Label speedCapAaLabel;
        private System.Windows.Forms.TextBox speedCapAtTextBox;
        private System.Windows.Forms.Label speedCapAtLabel;
        private System.Windows.Forms.TextBox speedCapEngTextBox;
        private System.Windows.Forms.Label speedCapEngLabel;
        private System.Windows.Forms.TextBox speedCapArtTextBox;
        private System.Windows.Forms.Label speedCapArtLabel;
        private System.Windows.Forms.TextBox speedCapAllTextBox;
        private System.Windows.Forms.Label speedCapAllLabel;
        private System.Windows.Forms.TextBox moraleTextBox;
        private System.Windows.Forms.Label moraleLabel;
        private System.Windows.Forms.TextBox defaultOrganisationTextBox;
        private System.Windows.Forms.Label defaultOrganisationLabel;
        private System.Windows.Forms.GroupBox battleGroupBox;
        private System.Windows.Forms.TextBox softAttackTextBox;
        private System.Windows.Forms.Label softAttackLabel;
        private System.Windows.Forms.TextBox softnessTextBox;
        private System.Windows.Forms.Label softnessLabel;
        private System.Windows.Forms.TextBox toughnessTextBox;
        private System.Windows.Forms.Label toughnessLabel;
        private System.Windows.Forms.TextBox surfaceDefenceTextBox;
        private System.Windows.Forms.Label surfaceDefenceLabel;
        private System.Windows.Forms.TextBox airDefenceTextBox;
        private System.Windows.Forms.Label airDefenceLabel;
        private System.Windows.Forms.TextBox seaDefenceTextBox;
        private System.Windows.Forms.Label seaDefenceLabel;
        private System.Windows.Forms.TextBox defensivenessTextBox;
        private System.Windows.Forms.Label defensivenessLabel;
        private System.Windows.Forms.TextBox suppressionTextBox;
        private System.Windows.Forms.Label suppressionLabel;
        private System.Windows.Forms.TextBox hardAttackTextBox;
        private System.Windows.Forms.Label hardAttackLabel;
        private System.Windows.Forms.TextBox subAttackTextBox;
        private System.Windows.Forms.Label subAttackLabel;
        private System.Windows.Forms.TextBox seaAttackTextBox;
        private System.Windows.Forms.Label seaAttackLabel;
        private System.Windows.Forms.TextBox convoyAttackTextBox;
        private System.Windows.Forms.Label convoyAttackLabel;
        private System.Windows.Forms.TextBox shoreBombardmentTextBox;
        private System.Windows.Forms.Label shoreBombardmentLabel;
        private System.Windows.Forms.TextBox navalAttackTextBox;
        private System.Windows.Forms.Label navalAttackLabel;
        private System.Windows.Forms.TextBox airAttackTextBox;
        private System.Windows.Forms.Label airAttackLabel;
        private System.Windows.Forms.TextBox strategicAttackTextBox;
        private System.Windows.Forms.Label strategicAttackLabel;
        private System.Windows.Forms.TextBox distanceTextBox;
        private System.Windows.Forms.Label distanceLabel;
        private System.Windows.Forms.TextBox surfaceDetectionCapabilityTextBox;
        private System.Windows.Forms.Label surfaceDetectionCapabilityLabel;
        private System.Windows.Forms.TextBox airDetectionCapabilityTextBox;
        private System.Windows.Forms.Label airDetectionCapabilityLabel;
        private System.Windows.Forms.TextBox subDetectionCapabilityTextBox;
        private System.Windows.Forms.Label subDetectionCapabilityLabel;
        private System.Windows.Forms.TextBox visibilityTextBox;
        private System.Windows.Forms.Label visibilityLabel;
        private System.Windows.Forms.TextBox transportWeightTextBox;
        private System.Windows.Forms.Label transportWeightLabel;
        private System.Windows.Forms.TextBox transportCapabilityTextBox;
        private System.Windows.Forms.Label transportCapabilityLabel;
        private System.Windows.Forms.TextBox fuelConsumptionTextBox;
        private System.Windows.Forms.Label fuelConsumptionLabel;
        private System.Windows.Forms.TextBox supplyConsumptionTextBox;
        private System.Windows.Forms.Label supplyConsumptionLabel;
        private System.Windows.Forms.TextBox upgradeCostFactorTextBox;
        private System.Windows.Forms.Label upgradeCostFactorLabel;
        private System.Windows.Forms.TextBox upgradeTimeFactorTextBox;
        private System.Windows.Forms.Label upgradeTimeFactorLabel;
        private System.Windows.Forms.TextBox artilleryBombardmentTextBox;
        private System.Windows.Forms.Label artilleryBombardmentLabel;
        private System.Windows.Forms.TextBox maxOilStockTextBox;
        private System.Windows.Forms.Label maxOilStockLabel;
        private System.Windows.Forms.TextBox maxSupplyStockTextBox;
        private System.Windows.Forms.Label maxSupplyStockLabel;
        private System.Windows.Forms.TextBox noFuelCombatModTextBox;
        private System.Windows.Forms.Label noFuelCombatModLabel;
        private System.Windows.Forms.TextBox reinforceCostTextBox;
        private System.Windows.Forms.Label reinforceCostLabel;
        private System.Windows.Forms.TextBox reinforceTimeTextBox;
        private System.Windows.Forms.Label reinforceTimeLabel;
        private System.Windows.Forms.GroupBox equipmentGroupBox;
        private System.Windows.Forms.ListView equipmentListView;
        private System.Windows.Forms.ColumnHeader resourceColumnHeader;
        private System.Windows.Forms.ColumnHeader quantityColumnHeader;
        private System.Windows.Forms.GroupBox productionGroupBox;
        private System.Windows.Forms.GroupBox speedGroupBox;
        private System.Windows.Forms.Button equipmentRemoveButton;
        private System.Windows.Forms.Button equipmentAddButton;
        private System.Windows.Forms.Label quantityLabel;
        private System.Windows.Forms.ComboBox resourceComboBox;
        private System.Windows.Forms.Label resourceLabel;
        private System.Windows.Forms.TextBox modelNameTextBox;
        private System.Windows.Forms.Label classNameLabel;
        private System.Windows.Forms.TextBox classShortDescTextBox;
        private System.Windows.Forms.Label classShortDescLabel;
        private System.Windows.Forms.TextBox classDescTextBox;
        private System.Windows.Forms.Label classDescLabel;
        private System.Windows.Forms.TextBox classShortNameTextBox;
        private System.Windows.Forms.Label classShortNameLabel;
        private System.Windows.Forms.TextBox classNameTextBox;
        private System.Windows.Forms.Button equipmentDownButton;
        private System.Windows.Forms.Button equipmentUpButton;
        private System.Windows.Forms.CheckedListBox allowedBrigadesCheckedListBox;
        private System.Windows.Forms.Label allowedBrigadesLabel;
        private System.Windows.Forms.Label branchLabel;
        private System.Windows.Forms.ComboBox branchComboBox;
        private System.Windows.Forms.CheckBox detachableCheckBox;
        private System.Windows.Forms.NumericUpDown gfxPrioNumericUpDown;
        private System.Windows.Forms.Label gfxPrioLabel;
        private System.Windows.Forms.CheckBox productableCheckBox;
        private System.Windows.Forms.ComboBox realUnitTypeComboBox;
        private System.Windows.Forms.Label realUnitTypeLabel;
        private System.Windows.Forms.ComboBox spriteTypeComboBox;
        private System.Windows.Forms.Label spriteTypeLabel;
        private System.Windows.Forms.ComboBox transmuteComboBox;
        private System.Windows.Forms.Label transmuteLabel;
        private System.Windows.Forms.ListView upgradeListView;
        private System.Windows.Forms.NumericUpDown militaryValueNumericUpDown;
        private System.Windows.Forms.Label militaryValueLabel;
        private System.Windows.Forms.GroupBox upgradeGroupBox;
        private System.Windows.Forms.ColumnHeader upgradeTypeColumnHeader;
        private System.Windows.Forms.ColumnHeader upgradeCostColumnHeader;
        private System.Windows.Forms.ColumnHeader upgradeTimeColumnHeader;
        private System.Windows.Forms.TextBox upgradeTimeTextBox;
        private System.Windows.Forms.Label upgradeTimeLabel;
        private System.Windows.Forms.TextBox upgradeCostTextBox;
        private System.Windows.Forms.Label upgradeCostLabel;
        private System.Windows.Forms.ComboBox upgradeTypeComboBox;
        private System.Windows.Forms.Label upgradeTypeLabel;
        private System.Windows.Forms.Button upgradeRemoveButton;
        private System.Windows.Forms.Button upgradeAddButton;
        private System.Windows.Forms.NumericUpDown listPrioNumericUpDown;
        private System.Windows.Forms.Label listPrioLabel;
        private System.Windows.Forms.TextBox quantityTextBox;
        private System.Windows.Forms.ListView countryListView;
        private System.Windows.Forms.ColumnHeader dummyColumnHeader;
    }
}