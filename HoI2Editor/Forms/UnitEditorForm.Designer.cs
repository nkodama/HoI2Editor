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
            this.bottomButton = new System.Windows.Forms.Button();
            this.downButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.topButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.cloneButton = new System.Windows.Forms.Button();
            this.newButton = new System.Windows.Forms.Button();
            this.leaderListView = new System.Windows.Forms.ListView();
            this.classColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.branchColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.noColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.costColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buildTimeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.manPowerSkillColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.supplyColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fuelColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.countryListBox = new System.Windows.Forms.ListBox();
            this.classListBox = new System.Windows.Forms.ListBox();
            this.editTabControl = new System.Windows.Forms.TabControl();
            this.classTabPage = new System.Windows.Forms.TabPage();
            this.modelTabPage = new System.Windows.Forms.TabPage();
            this.modelNameTextBox = new System.Windows.Forms.TextBox();
            this.equipmentGroupBox = new System.Windows.Forms.GroupBox();
            this.quantityNumericUpDown = new System.Windows.Forms.NumericUpDown();
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
            this.classNameLabel = new System.Windows.Forms.Label();
            this.classNameTextBox = new System.Windows.Forms.TextBox();
            this.classShortNameTextBox = new System.Windows.Forms.TextBox();
            this.classShortNameLabel = new System.Windows.Forms.Label();
            this.classDescTextBox = new System.Windows.Forms.TextBox();
            this.classDescLabel = new System.Windows.Forms.Label();
            this.classShortDescTextBox = new System.Windows.Forms.TextBox();
            this.classShortDescLabel = new System.Windows.Forms.Label();
            this.equipmentUpButton = new System.Windows.Forms.Button();
            this.equipmentDownButton = new System.Windows.Forms.Button();
            this.allowedBrigadesCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.branchComboBox = new System.Windows.Forms.ComboBox();
            this.branchLabel = new System.Windows.Forms.Label();
            this.allowedBrigadesLabel = new System.Windows.Forms.Label();
            this.detachableCheckBox = new System.Windows.Forms.CheckBox();
            this.gfxPrioLabel = new System.Windows.Forms.Label();
            this.gfxPrioNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.productableCheckBox = new System.Windows.Forms.CheckBox();
            this.realUnitTypeLabel = new System.Windows.Forms.Label();
            this.realUnitTypeComboBox = new System.Windows.Forms.ComboBox();
            this.spriteTypeLabel = new System.Windows.Forms.Label();
            this.spriteTypeComboBox = new System.Windows.Forms.ComboBox();
            this.transmuteLabel = new System.Windows.Forms.Label();
            this.transmuteComboBox = new System.Windows.Forms.ComboBox();
            this.upgradeListView = new System.Windows.Forms.ListView();
            this.militaryValueLabel = new System.Windows.Forms.Label();
            this.militaryValueNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.upgradeGroupBox = new System.Windows.Forms.GroupBox();
            this.upgradeTypeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.upgradeCostColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.upgradeTimeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.upgradeAddButton = new System.Windows.Forms.Button();
            this.upgradeRemoveButton = new System.Windows.Forms.Button();
            this.upgradeTypeLabel = new System.Windows.Forms.Label();
            this.upgradeTypeComboBox = new System.Windows.Forms.ComboBox();
            this.upgradeCostLabel = new System.Windows.Forms.Label();
            this.upgradeCostTextBox = new System.Windows.Forms.TextBox();
            this.upgradeTimeLabel = new System.Windows.Forms.Label();
            this.upgradeTimeTextBox = new System.Windows.Forms.TextBox();
            this.listPrioLabel = new System.Windows.Forms.Label();
            this.listPrioNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.editTabControl.SuspendLayout();
            this.classTabPage.SuspendLayout();
            this.modelTabPage.SuspendLayout();
            this.equipmentGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.quantityNumericUpDown)).BeginInit();
            this.productionGroupBox.SuspendLayout();
            this.speedGroupBox.SuspendLayout();
            this.battleGroupBox.SuspendLayout();
            this.basicGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.modelIconPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.modelImagePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gfxPrioNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.militaryValueNumericUpDown)).BeginInit();
            this.upgradeGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listPrioNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // bottomButton
            // 
            this.bottomButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bottomButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bottomButton.Location = new System.Drawing.Point(921, 208);
            this.bottomButton.Name = "bottomButton";
            this.bottomButton.Size = new System.Drawing.Size(75, 23);
            this.bottomButton.TabIndex = 15;
            this.bottomButton.Text = "Bottom";
            this.bottomButton.UseVisualStyleBackColor = true;
            // 
            // downButton
            // 
            this.downButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.downButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.downButton.Location = new System.Drawing.Point(840, 208);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(75, 23);
            this.downButton.TabIndex = 14;
            this.downButton.Text = "Down";
            this.downButton.UseVisualStyleBackColor = true;
            // 
            // upButton
            // 
            this.upButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.upButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.upButton.Location = new System.Drawing.Point(759, 208);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(75, 23);
            this.upButton.TabIndex = 13;
            this.upButton.Text = "Up";
            this.upButton.UseVisualStyleBackColor = true;
            // 
            // topButton
            // 
            this.topButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.topButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.topButton.Location = new System.Drawing.Point(678, 208);
            this.topButton.Name = "topButton";
            this.topButton.Size = new System.Drawing.Size(75, 23);
            this.topButton.TabIndex = 12;
            this.topButton.Text = "Top";
            this.topButton.UseVisualStyleBackColor = true;
            // 
            // removeButton
            // 
            this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.removeButton.Location = new System.Drawing.Point(384, 208);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(75, 23);
            this.removeButton.TabIndex = 11;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            // 
            // cloneButton
            // 
            this.cloneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cloneButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cloneButton.Location = new System.Drawing.Point(303, 208);
            this.cloneButton.Name = "cloneButton";
            this.cloneButton.Size = new System.Drawing.Size(75, 23);
            this.cloneButton.TabIndex = 10;
            this.cloneButton.Text = "Clone";
            this.cloneButton.UseVisualStyleBackColor = true;
            // 
            // newButton
            // 
            this.newButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.newButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.newButton.Location = new System.Drawing.Point(222, 208);
            this.newButton.Name = "newButton";
            this.newButton.Size = new System.Drawing.Size(75, 23);
            this.newButton.TabIndex = 9;
            this.newButton.Text = "New";
            this.newButton.UseVisualStyleBackColor = true;
            // 
            // leaderListView
            // 
            this.leaderListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.leaderListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.classColumnHeader,
            this.branchColumnHeader,
            this.noColumnHeader,
            this.nameColumnHeader,
            this.costColumnHeader,
            this.buildTimeColumnHeader,
            this.manPowerSkillColumnHeader,
            this.supplyColumnHeader,
            this.fuelColumnHeader});
            this.leaderListView.FullRowSelect = true;
            this.leaderListView.GridLines = true;
            this.leaderListView.HideSelection = false;
            this.leaderListView.Location = new System.Drawing.Point(222, 12);
            this.leaderListView.MultiSelect = false;
            this.leaderListView.Name = "leaderListView";
            this.leaderListView.Size = new System.Drawing.Size(774, 190);
            this.leaderListView.TabIndex = 8;
            this.leaderListView.UseCompatibleStateImageBehavior = false;
            this.leaderListView.View = System.Windows.Forms.View.Details;
            // 
            // classColumnHeader
            // 
            this.classColumnHeader.Text = "Class";
            this.classColumnHeader.Width = 150;
            // 
            // branchColumnHeader
            // 
            this.branchColumnHeader.Text = "Branch";
            // 
            // noColumnHeader
            // 
            this.noColumnHeader.Text = "No.";
            this.noColumnHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.noColumnHeader.Width = 40;
            // 
            // nameColumnHeader
            // 
            this.nameColumnHeader.Text = "Name";
            this.nameColumnHeader.Width = 240;
            // 
            // costColumnHeader
            // 
            this.costColumnHeader.Text = "Cost";
            this.costColumnHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.costColumnHeader.Width = 50;
            // 
            // buildTimeColumnHeader
            // 
            this.buildTimeColumnHeader.Text = "Time";
            this.buildTimeColumnHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.buildTimeColumnHeader.Width = 50;
            // 
            // manPowerSkillColumnHeader
            // 
            this.manPowerSkillColumnHeader.Text = "MP";
            this.manPowerSkillColumnHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.manPowerSkillColumnHeader.Width = 50;
            // 
            // supplyColumnHeader
            // 
            this.supplyColumnHeader.Text = "Supply";
            this.supplyColumnHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.supplyColumnHeader.Width = 50;
            // 
            // fuelColumnHeader
            // 
            this.fuelColumnHeader.Text = "Fuel";
            this.fuelColumnHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.fuelColumnHeader.Width = 50;
            // 
            // countryListBox
            // 
            this.countryListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.countryListBox.ColumnWidth = 40;
            this.countryListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.countryListBox.FormattingEnabled = true;
            this.countryListBox.ItemHeight = 12;
            this.countryListBox.Location = new System.Drawing.Point(12, 437);
            this.countryListBox.MultiColumn = true;
            this.countryListBox.Name = "countryListBox";
            this.countryListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.countryListBox.Size = new System.Drawing.Size(192, 280);
            this.countryListBox.TabIndex = 16;
            // 
            // classListBox
            // 
            this.classListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.classListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.classListBox.FormattingEnabled = true;
            this.classListBox.ItemHeight = 12;
            this.classListBox.Location = new System.Drawing.Point(12, 12);
            this.classListBox.Name = "classListBox";
            this.classListBox.Size = new System.Drawing.Size(192, 412);
            this.classListBox.TabIndex = 17;
            // 
            // editTabControl
            // 
            this.editTabControl.Controls.Add(this.classTabPage);
            this.editTabControl.Controls.Add(this.modelTabPage);
            this.editTabControl.Location = new System.Drawing.Point(222, 237);
            this.editTabControl.Name = "editTabControl";
            this.editTabControl.SelectedIndex = 0;
            this.editTabControl.Size = new System.Drawing.Size(774, 451);
            this.editTabControl.TabIndex = 18;
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
            this.classTabPage.Location = new System.Drawing.Point(4, 22);
            this.classTabPage.Name = "classTabPage";
            this.classTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.classTabPage.Size = new System.Drawing.Size(766, 425);
            this.classTabPage.TabIndex = 0;
            this.classTabPage.Text = "Unit Class";
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
            this.modelTabPage.Location = new System.Drawing.Point(4, 22);
            this.modelTabPage.Name = "modelTabPage";
            this.modelTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.modelTabPage.Size = new System.Drawing.Size(766, 425);
            this.modelTabPage.TabIndex = 1;
            this.modelTabPage.Text = "Unit Model";
            // 
            // modelNameTextBox
            // 
            this.modelNameTextBox.Location = new System.Drawing.Point(9, 116);
            this.modelNameTextBox.Name = "modelNameTextBox";
            this.modelNameTextBox.Size = new System.Drawing.Size(291, 19);
            this.modelNameTextBox.TabIndex = 33;
            // 
            // equipmentGroupBox
            // 
            this.equipmentGroupBox.Controls.Add(this.equipmentDownButton);
            this.equipmentGroupBox.Controls.Add(this.equipmentUpButton);
            this.equipmentGroupBox.Controls.Add(this.quantityNumericUpDown);
            this.equipmentGroupBox.Controls.Add(this.quantityLabel);
            this.equipmentGroupBox.Controls.Add(this.resourceComboBox);
            this.equipmentGroupBox.Controls.Add(this.resourceLabel);
            this.equipmentGroupBox.Controls.Add(this.equipmentRemoveButton);
            this.equipmentGroupBox.Controls.Add(this.equipmentAddButton);
            this.equipmentGroupBox.Controls.Add(this.equipmentListView);
            this.equipmentGroupBox.Location = new System.Drawing.Point(306, 312);
            this.equipmentGroupBox.Name = "equipmentGroupBox";
            this.equipmentGroupBox.Size = new System.Drawing.Size(448, 107);
            this.equipmentGroupBox.TabIndex = 17;
            this.equipmentGroupBox.TabStop = false;
            this.equipmentGroupBox.Text = "Equipment";
            // 
            // quantityNumericUpDown
            // 
            this.quantityNumericUpDown.Location = new System.Drawing.Point(342, 79);
            this.quantityNumericUpDown.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.quantityNumericUpDown.Name = "quantityNumericUpDown";
            this.quantityNumericUpDown.Size = new System.Drawing.Size(95, 19);
            this.quantityNumericUpDown.TabIndex = 6;
            this.quantityNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // quantityLabel
            // 
            this.quantityLabel.AutoSize = true;
            this.quantityLabel.Location = new System.Drawing.Point(279, 81);
            this.quantityLabel.Name = "quantityLabel";
            this.quantityLabel.Size = new System.Drawing.Size(48, 12);
            this.quantityLabel.TabIndex = 5;
            this.quantityLabel.Text = "Quantity";
            // 
            // resourceComboBox
            // 
            this.resourceComboBox.FormattingEnabled = true;
            this.resourceComboBox.Location = new System.Drawing.Point(342, 49);
            this.resourceComboBox.Name = "resourceComboBox";
            this.resourceComboBox.Size = new System.Drawing.Size(95, 20);
            this.resourceComboBox.TabIndex = 4;
            // 
            // resourceLabel
            // 
            this.resourceLabel.AutoSize = true;
            this.resourceLabel.Location = new System.Drawing.Point(279, 52);
            this.resourceLabel.Name = "resourceLabel";
            this.resourceLabel.Size = new System.Drawing.Size(53, 12);
            this.resourceLabel.TabIndex = 3;
            this.resourceLabel.Text = "Resource";
            // 
            // equipmentRemoveButton
            // 
            this.equipmentRemoveButton.Location = new System.Drawing.Point(362, 20);
            this.equipmentRemoveButton.Name = "equipmentRemoveButton";
            this.equipmentRemoveButton.Size = new System.Drawing.Size(75, 23);
            this.equipmentRemoveButton.TabIndex = 2;
            this.equipmentRemoveButton.Text = "Remove";
            this.equipmentRemoveButton.UseVisualStyleBackColor = true;
            // 
            // equipmentAddButton
            // 
            this.equipmentAddButton.Location = new System.Drawing.Point(281, 20);
            this.equipmentAddButton.Name = "equipmentAddButton";
            this.equipmentAddButton.Size = new System.Drawing.Size(75, 23);
            this.equipmentAddButton.TabIndex = 1;
            this.equipmentAddButton.Text = "Add";
            this.equipmentAddButton.UseVisualStyleBackColor = true;
            // 
            // equipmentListView
            // 
            this.equipmentListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.resourceColumnHeader,
            this.quantityColumnHeader});
            this.equipmentListView.Location = new System.Drawing.Point(8, 18);
            this.equipmentListView.Name = "equipmentListView";
            this.equipmentListView.Size = new System.Drawing.Size(184, 83);
            this.equipmentListView.TabIndex = 0;
            this.equipmentListView.UseCompatibleStateImageBehavior = false;
            this.equipmentListView.View = System.Windows.Forms.View.Details;
            // 
            // resourceColumnHeader
            // 
            this.resourceColumnHeader.Text = "Resource";
            this.resourceColumnHeader.Width = 100;
            // 
            // quantityColumnHeader
            // 
            this.quantityColumnHeader.Text = "Quantity";
            this.quantityColumnHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
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
            this.productionGroupBox.Location = new System.Drawing.Point(9, 290);
            this.productionGroupBox.Name = "productionGroupBox";
            this.productionGroupBox.Size = new System.Drawing.Size(291, 129);
            this.productionGroupBox.TabIndex = 16;
            this.productionGroupBox.TabStop = false;
            this.productionGroupBox.Text = "Production Status";
            // 
            // reinforceCostTextBox
            // 
            this.reinforceCostTextBox.Location = new System.Drawing.Point(231, 93);
            this.reinforceCostTextBox.Name = "reinforceCostTextBox";
            this.reinforceCostTextBox.Size = new System.Drawing.Size(50, 19);
            this.reinforceCostTextBox.TabIndex = 37;
            // 
            // costTextBox
            // 
            this.costTextBox.Location = new System.Drawing.Point(87, 18);
            this.costTextBox.Name = "costTextBox";
            this.costTextBox.Size = new System.Drawing.Size(50, 19);
            this.costTextBox.TabIndex = 1;
            // 
            // reinforceCostLabel
            // 
            this.reinforceCostLabel.AutoSize = true;
            this.reinforceCostLabel.Location = new System.Drawing.Point(142, 96);
            this.reinforceCostLabel.Name = "reinforceCostLabel";
            this.reinforceCostLabel.Size = new System.Drawing.Size(82, 12);
            this.reinforceCostLabel.TabIndex = 36;
            this.reinforceCostLabel.Text = "Reinforce Cost";
            // 
            // costLabel
            // 
            this.costLabel.AutoSize = true;
            this.costLabel.Location = new System.Drawing.Point(7, 21);
            this.costLabel.Name = "costLabel";
            this.costLabel.Size = new System.Drawing.Size(59, 12);
            this.costLabel.TabIndex = 0;
            this.costLabel.Text = "Build Cost";
            // 
            // reinforceTimeTextBox
            // 
            this.reinforceTimeTextBox.Location = new System.Drawing.Point(231, 68);
            this.reinforceTimeTextBox.Name = "reinforceTimeTextBox";
            this.reinforceTimeTextBox.Size = new System.Drawing.Size(50, 19);
            this.reinforceTimeTextBox.TabIndex = 35;
            // 
            // buildTimeLabel
            // 
            this.buildTimeLabel.AutoSize = true;
            this.buildTimeLabel.Location = new System.Drawing.Point(7, 45);
            this.buildTimeLabel.Name = "buildTimeLabel";
            this.buildTimeLabel.Size = new System.Drawing.Size(60, 12);
            this.buildTimeLabel.TabIndex = 2;
            this.buildTimeLabel.Text = "Build Time";
            // 
            // reinforceTimeLabel
            // 
            this.reinforceTimeLabel.AutoSize = true;
            this.reinforceTimeLabel.Location = new System.Drawing.Point(142, 71);
            this.reinforceTimeLabel.Name = "reinforceTimeLabel";
            this.reinforceTimeLabel.Size = new System.Drawing.Size(83, 12);
            this.reinforceTimeLabel.TabIndex = 34;
            this.reinforceTimeLabel.Text = "Reinforce Time";
            // 
            // buildTimeTextBox
            // 
            this.buildTimeTextBox.Location = new System.Drawing.Point(86, 43);
            this.buildTimeTextBox.Name = "buildTimeTextBox";
            this.buildTimeTextBox.Size = new System.Drawing.Size(50, 19);
            this.buildTimeTextBox.TabIndex = 3;
            // 
            // manPowerTextBox
            // 
            this.manPowerTextBox.Location = new System.Drawing.Point(86, 68);
            this.manPowerTextBox.Name = "manPowerTextBox";
            this.manPowerTextBox.Size = new System.Drawing.Size(50, 19);
            this.manPowerTextBox.TabIndex = 5;
            // 
            // manPowerLabel
            // 
            this.manPowerLabel.AutoSize = true;
            this.manPowerLabel.Location = new System.Drawing.Point(6, 70);
            this.manPowerLabel.Name = "manPowerLabel";
            this.manPowerLabel.Size = new System.Drawing.Size(61, 12);
            this.manPowerLabel.TabIndex = 4;
            this.manPowerLabel.Text = "Man Power";
            // 
            // upgradeTimeFactorTextBox
            // 
            this.upgradeTimeFactorTextBox.Location = new System.Drawing.Point(231, 18);
            this.upgradeTimeFactorTextBox.Name = "upgradeTimeFactorTextBox";
            this.upgradeTimeFactorTextBox.Size = new System.Drawing.Size(50, 19);
            this.upgradeTimeFactorTextBox.TabIndex = 25;
            // 
            // upgradeTimeFactorLabel
            // 
            this.upgradeTimeFactorLabel.AutoSize = true;
            this.upgradeTimeFactorLabel.Location = new System.Drawing.Point(142, 21);
            this.upgradeTimeFactorLabel.Name = "upgradeTimeFactorLabel";
            this.upgradeTimeFactorLabel.Size = new System.Drawing.Size(76, 12);
            this.upgradeTimeFactorLabel.TabIndex = 24;
            this.upgradeTimeFactorLabel.Text = "Upgrade Time";
            // 
            // upgradeCostFactorLabel
            // 
            this.upgradeCostFactorLabel.AutoSize = true;
            this.upgradeCostFactorLabel.Location = new System.Drawing.Point(142, 46);
            this.upgradeCostFactorLabel.Name = "upgradeCostFactorLabel";
            this.upgradeCostFactorLabel.Size = new System.Drawing.Size(75, 12);
            this.upgradeCostFactorLabel.TabIndex = 26;
            this.upgradeCostFactorLabel.Text = "Upgrade Cost";
            // 
            // upgradeCostFactorTextBox
            // 
            this.upgradeCostFactorTextBox.Location = new System.Drawing.Point(231, 43);
            this.upgradeCostFactorTextBox.Name = "upgradeCostFactorTextBox";
            this.upgradeCostFactorTextBox.Size = new System.Drawing.Size(50, 19);
            this.upgradeCostFactorTextBox.TabIndex = 27;
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
            this.speedGroupBox.Location = new System.Drawing.Point(306, 6);
            this.speedGroupBox.Name = "speedGroupBox";
            this.speedGroupBox.Size = new System.Drawing.Size(448, 74);
            this.speedGroupBox.TabIndex = 15;
            this.speedGroupBox.TabStop = false;
            this.speedGroupBox.Text = "Speed Status";
            // 
            // speedCapAaTextBox
            // 
            this.speedCapAaTextBox.Location = new System.Drawing.Point(387, 43);
            this.speedCapAaTextBox.Name = "speedCapAaTextBox";
            this.speedCapAaTextBox.Size = new System.Drawing.Size(50, 19);
            this.speedCapAaTextBox.TabIndex = 11;
            // 
            // maxSpeedTextBox
            // 
            this.maxSpeedTextBox.Location = new System.Drawing.Point(93, 18);
            this.maxSpeedTextBox.Name = "maxSpeedTextBox";
            this.maxSpeedTextBox.Size = new System.Drawing.Size(50, 19);
            this.maxSpeedTextBox.TabIndex = 7;
            // 
            // speedCapAaLabel
            // 
            this.speedCapAaLabel.AutoSize = true;
            this.speedCapAaLabel.Location = new System.Drawing.Point(300, 46);
            this.speedCapAaLabel.Name = "speedCapAaLabel";
            this.speedCapAaLabel.Size = new System.Drawing.Size(45, 12);
            this.speedCapAaLabel.TabIndex = 10;
            this.speedCapAaLabel.Text = "AA Cap";
            // 
            // maxSpeedLabel
            // 
            this.maxSpeedLabel.AutoSize = true;
            this.maxSpeedLabel.Location = new System.Drawing.Point(6, 21);
            this.maxSpeedLabel.Name = "maxSpeedLabel";
            this.maxSpeedLabel.Size = new System.Drawing.Size(61, 12);
            this.maxSpeedLabel.TabIndex = 6;
            this.maxSpeedLabel.Text = "Max Speed";
            // 
            // speedCapAtTextBox
            // 
            this.speedCapAtTextBox.Location = new System.Drawing.Point(387, 18);
            this.speedCapAtTextBox.Name = "speedCapAtTextBox";
            this.speedCapAtTextBox.Size = new System.Drawing.Size(50, 19);
            this.speedCapAtTextBox.TabIndex = 9;
            // 
            // speedCapAtLabel
            // 
            this.speedCapAtLabel.AutoSize = true;
            this.speedCapAtLabel.Location = new System.Drawing.Point(300, 21);
            this.speedCapAtLabel.Name = "speedCapAtLabel";
            this.speedCapAtLabel.Size = new System.Drawing.Size(44, 12);
            this.speedCapAtLabel.TabIndex = 8;
            this.speedCapAtLabel.Text = "AT Cap";
            // 
            // speedCapEngTextBox
            // 
            this.speedCapEngTextBox.Location = new System.Drawing.Point(244, 43);
            this.speedCapEngTextBox.Name = "speedCapEngTextBox";
            this.speedCapEngTextBox.Size = new System.Drawing.Size(50, 19);
            this.speedCapEngTextBox.TabIndex = 7;
            // 
            // speedCapAllTextBox
            // 
            this.speedCapAllTextBox.Location = new System.Drawing.Point(93, 43);
            this.speedCapAllTextBox.Name = "speedCapAllTextBox";
            this.speedCapAllTextBox.Size = new System.Drawing.Size(50, 19);
            this.speedCapAllTextBox.TabIndex = 3;
            // 
            // speedCapEngLabel
            // 
            this.speedCapEngLabel.AutoSize = true;
            this.speedCapEngLabel.Location = new System.Drawing.Point(149, 46);
            this.speedCapEngLabel.Name = "speedCapEngLabel";
            this.speedCapEngLabel.Size = new System.Drawing.Size(50, 12);
            this.speedCapEngLabel.TabIndex = 6;
            this.speedCapEngLabel.Text = "Eng. Cap";
            // 
            // speedCapAllLabel
            // 
            this.speedCapAllLabel.AutoSize = true;
            this.speedCapAllLabel.Location = new System.Drawing.Point(6, 46);
            this.speedCapAllLabel.Name = "speedCapAllLabel";
            this.speedCapAllLabel.Size = new System.Drawing.Size(60, 12);
            this.speedCapAllLabel.TabIndex = 2;
            this.speedCapAllLabel.Text = "Speed Cap";
            // 
            // speedCapArtTextBox
            // 
            this.speedCapArtTextBox.Location = new System.Drawing.Point(244, 17);
            this.speedCapArtTextBox.Name = "speedCapArtTextBox";
            this.speedCapArtTextBox.Size = new System.Drawing.Size(50, 19);
            this.speedCapArtTextBox.TabIndex = 5;
            // 
            // speedCapArtLabel
            // 
            this.speedCapArtLabel.AutoSize = true;
            this.speedCapArtLabel.Location = new System.Drawing.Point(149, 21);
            this.speedCapArtLabel.Name = "speedCapArtLabel";
            this.speedCapArtLabel.Size = new System.Drawing.Size(47, 12);
            this.speedCapArtLabel.TabIndex = 4;
            this.speedCapArtLabel.Text = "Art. Cap";
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
            this.battleGroupBox.Location = new System.Drawing.Point(306, 86);
            this.battleGroupBox.Name = "battleGroupBox";
            this.battleGroupBox.Size = new System.Drawing.Size(448, 220);
            this.battleGroupBox.TabIndex = 14;
            this.battleGroupBox.TabStop = false;
            this.battleGroupBox.Text = "Battle Status";
            // 
            // artilleryBombardmentTextBox
            // 
            this.artilleryBombardmentTextBox.Location = new System.Drawing.Point(244, 193);
            this.artilleryBombardmentTextBox.Name = "artilleryBombardmentTextBox";
            this.artilleryBombardmentTextBox.Size = new System.Drawing.Size(50, 19);
            this.artilleryBombardmentTextBox.TabIndex = 41;
            // 
            // artilleryBombardmentLabel
            // 
            this.artilleryBombardmentLabel.AutoSize = true;
            this.artilleryBombardmentLabel.Location = new System.Drawing.Point(149, 196);
            this.artilleryBombardmentLabel.Name = "artilleryBombardmentLabel";
            this.artilleryBombardmentLabel.Size = new System.Drawing.Size(58, 12);
            this.artilleryBombardmentLabel.TabIndex = 40;
            this.artilleryBombardmentLabel.Text = "Art. Bomb.";
            // 
            // visibilityTextBox
            // 
            this.visibilityTextBox.Location = new System.Drawing.Point(387, 43);
            this.visibilityTextBox.Name = "visibilityTextBox";
            this.visibilityTextBox.Size = new System.Drawing.Size(50, 19);
            this.visibilityTextBox.TabIndex = 39;
            // 
            // visibilityLabel
            // 
            this.visibilityLabel.AutoSize = true;
            this.visibilityLabel.Location = new System.Drawing.Point(300, 46);
            this.visibilityLabel.Name = "visibilityLabel";
            this.visibilityLabel.Size = new System.Drawing.Size(50, 12);
            this.visibilityLabel.TabIndex = 38;
            this.visibilityLabel.Text = "Visibility";
            // 
            // noFuelCombatModTextBox
            // 
            this.noFuelCombatModTextBox.Location = new System.Drawing.Point(387, 143);
            this.noFuelCombatModTextBox.Name = "noFuelCombatModTextBox";
            this.noFuelCombatModTextBox.Size = new System.Drawing.Size(50, 19);
            this.noFuelCombatModTextBox.TabIndex = 33;
            // 
            // noFuelCombatModLabel
            // 
            this.noFuelCombatModLabel.AutoSize = true;
            this.noFuelCombatModLabel.Location = new System.Drawing.Point(300, 146);
            this.noFuelCombatModLabel.Name = "noFuelCombatModLabel";
            this.noFuelCombatModLabel.Size = new System.Drawing.Size(72, 12);
            this.noFuelCombatModLabel.TabIndex = 32;
            this.noFuelCombatModLabel.Text = "No Fuel Mod.";
            // 
            // airDetectionCapabilityTextBox
            // 
            this.airDetectionCapabilityTextBox.Location = new System.Drawing.Point(387, 118);
            this.airDetectionCapabilityTextBox.Name = "airDetectionCapabilityTextBox";
            this.airDetectionCapabilityTextBox.Size = new System.Drawing.Size(50, 19);
            this.airDetectionCapabilityTextBox.TabIndex = 37;
            // 
            // airDetectionCapabilityLabel
            // 
            this.airDetectionCapabilityLabel.AutoSize = true;
            this.airDetectionCapabilityLabel.Location = new System.Drawing.Point(300, 121);
            this.airDetectionCapabilityLabel.Name = "airDetectionCapabilityLabel";
            this.airDetectionCapabilityLabel.Size = new System.Drawing.Size(73, 12);
            this.airDetectionCapabilityLabel.TabIndex = 36;
            this.airDetectionCapabilityLabel.Text = "Air Detection";
            // 
            // subDetectionCapabilityTextBox
            // 
            this.subDetectionCapabilityTextBox.Location = new System.Drawing.Point(387, 93);
            this.subDetectionCapabilityTextBox.Name = "subDetectionCapabilityTextBox";
            this.subDetectionCapabilityTextBox.Size = new System.Drawing.Size(50, 19);
            this.subDetectionCapabilityTextBox.TabIndex = 35;
            // 
            // subDetectionCapabilityLabel
            // 
            this.subDetectionCapabilityLabel.AutoSize = true;
            this.subDetectionCapabilityLabel.Location = new System.Drawing.Point(300, 96);
            this.subDetectionCapabilityLabel.Name = "subDetectionCapabilityLabel";
            this.subDetectionCapabilityLabel.Size = new System.Drawing.Size(77, 12);
            this.subDetectionCapabilityLabel.TabIndex = 34;
            this.subDetectionCapabilityLabel.Text = "Sub Detection";
            // 
            // surfaceDetectionCapabilityTextBox
            // 
            this.surfaceDetectionCapabilityTextBox.Location = new System.Drawing.Point(387, 68);
            this.surfaceDetectionCapabilityTextBox.Name = "surfaceDetectionCapabilityTextBox";
            this.surfaceDetectionCapabilityTextBox.Size = new System.Drawing.Size(50, 19);
            this.surfaceDetectionCapabilityTextBox.TabIndex = 33;
            // 
            // surfaceDetectionCapabilityLabel
            // 
            this.surfaceDetectionCapabilityLabel.AutoSize = true;
            this.surfaceDetectionCapabilityLabel.Location = new System.Drawing.Point(300, 71);
            this.surfaceDetectionCapabilityLabel.Name = "surfaceDetectionCapabilityLabel";
            this.surfaceDetectionCapabilityLabel.Size = new System.Drawing.Size(81, 12);
            this.surfaceDetectionCapabilityLabel.TabIndex = 32;
            this.surfaceDetectionCapabilityLabel.Text = "Surf. Detection";
            // 
            // distanceTextBox
            // 
            this.distanceTextBox.Location = new System.Drawing.Point(387, 18);
            this.distanceTextBox.Name = "distanceTextBox";
            this.distanceTextBox.Size = new System.Drawing.Size(50, 19);
            this.distanceTextBox.TabIndex = 31;
            // 
            // distanceLabel
            // 
            this.distanceLabel.AutoSize = true;
            this.distanceLabel.Location = new System.Drawing.Point(300, 21);
            this.distanceLabel.Name = "distanceLabel";
            this.distanceLabel.Size = new System.Drawing.Size(50, 12);
            this.distanceLabel.TabIndex = 30;
            this.distanceLabel.Text = "Distance";
            // 
            // strategicAttackTextBox
            // 
            this.strategicAttackTextBox.Location = new System.Drawing.Point(244, 168);
            this.strategicAttackTextBox.Name = "strategicAttackTextBox";
            this.strategicAttackTextBox.Size = new System.Drawing.Size(50, 19);
            this.strategicAttackTextBox.TabIndex = 29;
            // 
            // strategicAttackLabel
            // 
            this.strategicAttackLabel.AutoSize = true;
            this.strategicAttackLabel.Location = new System.Drawing.Point(149, 171);
            this.strategicAttackLabel.Name = "strategicAttackLabel";
            this.strategicAttackLabel.Size = new System.Drawing.Size(89, 12);
            this.strategicAttackLabel.TabIndex = 28;
            this.strategicAttackLabel.Text = "Strategic Attack";
            // 
            // navalAttackTextBox
            // 
            this.navalAttackTextBox.Location = new System.Drawing.Point(244, 143);
            this.navalAttackTextBox.Name = "navalAttackTextBox";
            this.navalAttackTextBox.Size = new System.Drawing.Size(50, 19);
            this.navalAttackTextBox.TabIndex = 27;
            // 
            // navalAttackLabel
            // 
            this.navalAttackLabel.AutoSize = true;
            this.navalAttackLabel.Location = new System.Drawing.Point(149, 146);
            this.navalAttackLabel.Name = "navalAttackLabel";
            this.navalAttackLabel.Size = new System.Drawing.Size(72, 12);
            this.navalAttackLabel.TabIndex = 26;
            this.navalAttackLabel.Text = "Naval Attack";
            // 
            // airAttackTextBox
            // 
            this.airAttackTextBox.Location = new System.Drawing.Point(244, 118);
            this.airAttackTextBox.Name = "airAttackTextBox";
            this.airAttackTextBox.Size = new System.Drawing.Size(50, 19);
            this.airAttackTextBox.TabIndex = 25;
            // 
            // airAttackLabel
            // 
            this.airAttackLabel.AutoSize = true;
            this.airAttackLabel.Location = new System.Drawing.Point(149, 121);
            this.airAttackLabel.Name = "airAttackLabel";
            this.airAttackLabel.Size = new System.Drawing.Size(58, 12);
            this.airAttackLabel.TabIndex = 24;
            this.airAttackLabel.Text = "Air Attack";
            // 
            // shoreBombardmentTextBox
            // 
            this.shoreBombardmentTextBox.Location = new System.Drawing.Point(244, 93);
            this.shoreBombardmentTextBox.Name = "shoreBombardmentTextBox";
            this.shoreBombardmentTextBox.Size = new System.Drawing.Size(50, 19);
            this.shoreBombardmentTextBox.TabIndex = 23;
            // 
            // shoreBombardmentLabel
            // 
            this.shoreBombardmentLabel.AutoSize = true;
            this.shoreBombardmentLabel.Location = new System.Drawing.Point(149, 96);
            this.shoreBombardmentLabel.Name = "shoreBombardmentLabel";
            this.shoreBombardmentLabel.Size = new System.Drawing.Size(69, 12);
            this.shoreBombardmentLabel.TabIndex = 22;
            this.shoreBombardmentLabel.Text = "Shore Bomb.";
            // 
            // convoyAttackTextBox
            // 
            this.convoyAttackTextBox.Location = new System.Drawing.Point(244, 68);
            this.convoyAttackTextBox.Name = "convoyAttackTextBox";
            this.convoyAttackTextBox.Size = new System.Drawing.Size(50, 19);
            this.convoyAttackTextBox.TabIndex = 21;
            // 
            // convoyAttackLabel
            // 
            this.convoyAttackLabel.AutoSize = true;
            this.convoyAttackLabel.Location = new System.Drawing.Point(149, 71);
            this.convoyAttackLabel.Name = "convoyAttackLabel";
            this.convoyAttackLabel.Size = new System.Drawing.Size(81, 12);
            this.convoyAttackLabel.TabIndex = 20;
            this.convoyAttackLabel.Text = "Convoy Attack";
            // 
            // subAttackTextBox
            // 
            this.subAttackTextBox.Location = new System.Drawing.Point(244, 43);
            this.subAttackTextBox.Name = "subAttackTextBox";
            this.subAttackTextBox.Size = new System.Drawing.Size(50, 19);
            this.subAttackTextBox.TabIndex = 19;
            // 
            // subAttackLabel
            // 
            this.subAttackLabel.AutoSize = true;
            this.subAttackLabel.Location = new System.Drawing.Point(149, 46);
            this.subAttackLabel.Name = "subAttackLabel";
            this.subAttackLabel.Size = new System.Drawing.Size(64, 12);
            this.subAttackLabel.TabIndex = 18;
            this.subAttackLabel.Text = "Sub. Attack";
            // 
            // seaAttackTextBox
            // 
            this.seaAttackTextBox.Location = new System.Drawing.Point(244, 18);
            this.seaAttackTextBox.Name = "seaAttackTextBox";
            this.seaAttackTextBox.Size = new System.Drawing.Size(50, 19);
            this.seaAttackTextBox.TabIndex = 17;
            // 
            // seaAttackLabel
            // 
            this.seaAttackLabel.AutoSize = true;
            this.seaAttackLabel.Location = new System.Drawing.Point(149, 21);
            this.seaAttackLabel.Name = "seaAttackLabel";
            this.seaAttackLabel.Size = new System.Drawing.Size(62, 12);
            this.seaAttackLabel.TabIndex = 16;
            this.seaAttackLabel.Text = "Sea Attack";
            // 
            // hardAttackTextBox
            // 
            this.hardAttackTextBox.Location = new System.Drawing.Point(93, 193);
            this.hardAttackTextBox.Name = "hardAttackTextBox";
            this.hardAttackTextBox.Size = new System.Drawing.Size(50, 19);
            this.hardAttackTextBox.TabIndex = 15;
            // 
            // hardAttackLabel
            // 
            this.hardAttackLabel.AutoSize = true;
            this.hardAttackLabel.Location = new System.Drawing.Point(6, 196);
            this.hardAttackLabel.Name = "hardAttackLabel";
            this.hardAttackLabel.Size = new System.Drawing.Size(67, 12);
            this.hardAttackLabel.TabIndex = 14;
            this.hardAttackLabel.Text = "Hard Attack";
            // 
            // softAttackTextBox
            // 
            this.softAttackTextBox.Location = new System.Drawing.Point(93, 168);
            this.softAttackTextBox.Name = "softAttackTextBox";
            this.softAttackTextBox.Size = new System.Drawing.Size(50, 19);
            this.softAttackTextBox.TabIndex = 13;
            // 
            // softAttackLabel
            // 
            this.softAttackLabel.AutoSize = true;
            this.softAttackLabel.Location = new System.Drawing.Point(6, 171);
            this.softAttackLabel.Name = "softAttackLabel";
            this.softAttackLabel.Size = new System.Drawing.Size(64, 12);
            this.softAttackLabel.TabIndex = 12;
            this.softAttackLabel.Text = "Soft Attack";
            // 
            // softnessTextBox
            // 
            this.softnessTextBox.Location = new System.Drawing.Point(93, 143);
            this.softnessTextBox.Name = "softnessTextBox";
            this.softnessTextBox.Size = new System.Drawing.Size(50, 19);
            this.softnessTextBox.TabIndex = 11;
            // 
            // softnessLabel
            // 
            this.softnessLabel.AutoSize = true;
            this.softnessLabel.Location = new System.Drawing.Point(6, 146);
            this.softnessLabel.Name = "softnessLabel";
            this.softnessLabel.Size = new System.Drawing.Size(50, 12);
            this.softnessLabel.TabIndex = 10;
            this.softnessLabel.Text = "Softness";
            // 
            // toughnessTextBox
            // 
            this.toughnessTextBox.Location = new System.Drawing.Point(93, 118);
            this.toughnessTextBox.Name = "toughnessTextBox";
            this.toughnessTextBox.Size = new System.Drawing.Size(50, 19);
            this.toughnessTextBox.TabIndex = 9;
            // 
            // toughnessLabel
            // 
            this.toughnessLabel.AutoSize = true;
            this.toughnessLabel.Location = new System.Drawing.Point(6, 121);
            this.toughnessLabel.Name = "toughnessLabel";
            this.toughnessLabel.Size = new System.Drawing.Size(60, 12);
            this.toughnessLabel.TabIndex = 8;
            this.toughnessLabel.Text = "Toughness";
            // 
            // surfaceDefenceTextBox
            // 
            this.surfaceDefenceTextBox.Location = new System.Drawing.Point(93, 93);
            this.surfaceDefenceTextBox.Name = "surfaceDefenceTextBox";
            this.surfaceDefenceTextBox.Size = new System.Drawing.Size(50, 19);
            this.surfaceDefenceTextBox.TabIndex = 7;
            // 
            // surfaceDefenceLabel
            // 
            this.surfaceDefenceLabel.AutoSize = true;
            this.surfaceDefenceLabel.Location = new System.Drawing.Point(6, 96);
            this.surfaceDefenceLabel.Name = "surfaceDefenceLabel";
            this.surfaceDefenceLabel.Size = new System.Drawing.Size(74, 12);
            this.surfaceDefenceLabel.TabIndex = 6;
            this.surfaceDefenceLabel.Text = "Surf. Defence";
            // 
            // airDefenceTextBox
            // 
            this.airDefenceTextBox.Location = new System.Drawing.Point(93, 68);
            this.airDefenceTextBox.Name = "airDefenceTextBox";
            this.airDefenceTextBox.Size = new System.Drawing.Size(50, 19);
            this.airDefenceTextBox.TabIndex = 5;
            // 
            // airDefenceLabel
            // 
            this.airDefenceLabel.AutoSize = true;
            this.airDefenceLabel.Location = new System.Drawing.Point(6, 71);
            this.airDefenceLabel.Name = "airDefenceLabel";
            this.airDefenceLabel.Size = new System.Drawing.Size(66, 12);
            this.airDefenceLabel.TabIndex = 4;
            this.airDefenceLabel.Text = "Air Defence";
            // 
            // seaDefenceTextBox
            // 
            this.seaDefenceTextBox.Location = new System.Drawing.Point(93, 43);
            this.seaDefenceTextBox.Name = "seaDefenceTextBox";
            this.seaDefenceTextBox.Size = new System.Drawing.Size(50, 19);
            this.seaDefenceTextBox.TabIndex = 3;
            // 
            // seaDefenceLabel
            // 
            this.seaDefenceLabel.AutoSize = true;
            this.seaDefenceLabel.Location = new System.Drawing.Point(6, 46);
            this.seaDefenceLabel.Name = "seaDefenceLabel";
            this.seaDefenceLabel.Size = new System.Drawing.Size(70, 12);
            this.seaDefenceLabel.TabIndex = 2;
            this.seaDefenceLabel.Text = "Sea Defence";
            // 
            // defensivenessTextBox
            // 
            this.defensivenessTextBox.Location = new System.Drawing.Point(93, 18);
            this.defensivenessTextBox.Name = "defensivenessTextBox";
            this.defensivenessTextBox.Size = new System.Drawing.Size(50, 19);
            this.defensivenessTextBox.TabIndex = 1;
            // 
            // defensivenessLabel
            // 
            this.defensivenessLabel.AutoSize = true;
            this.defensivenessLabel.Location = new System.Drawing.Point(6, 21);
            this.defensivenessLabel.Name = "defensivenessLabel";
            this.defensivenessLabel.Size = new System.Drawing.Size(80, 12);
            this.defensivenessLabel.TabIndex = 0;
            this.defensivenessLabel.Text = "Defensiveness";
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
            this.basicGroupBox.Location = new System.Drawing.Point(9, 141);
            this.basicGroupBox.Name = "basicGroupBox";
            this.basicGroupBox.Size = new System.Drawing.Size(291, 143);
            this.basicGroupBox.TabIndex = 2;
            this.basicGroupBox.TabStop = false;
            this.basicGroupBox.Text = "Basic Status";
            // 
            // transportWeightTextBox
            // 
            this.transportWeightTextBox.Location = new System.Drawing.Point(88, 93);
            this.transportWeightTextBox.Name = "transportWeightTextBox";
            this.transportWeightTextBox.Size = new System.Drawing.Size(50, 19);
            this.transportWeightTextBox.TabIndex = 17;
            // 
            // rangeLabel
            // 
            this.rangeLabel.AutoSize = true;
            this.rangeLabel.Location = new System.Drawing.Point(8, 71);
            this.rangeLabel.Name = "rangeLabel";
            this.rangeLabel.Size = new System.Drawing.Size(37, 12);
            this.rangeLabel.TabIndex = 8;
            this.rangeLabel.Text = "Range";
            // 
            // transportWeightLabel
            // 
            this.transportWeightLabel.AutoSize = true;
            this.transportWeightLabel.Location = new System.Drawing.Point(8, 96);
            this.transportWeightLabel.Name = "transportWeightLabel";
            this.transportWeightLabel.Size = new System.Drawing.Size(74, 12);
            this.transportWeightLabel.TabIndex = 16;
            this.transportWeightLabel.Text = "Trans. Weight";
            // 
            // maxOilStockTextBox
            // 
            this.maxOilStockTextBox.Location = new System.Drawing.Point(231, 118);
            this.maxOilStockTextBox.Name = "maxOilStockTextBox";
            this.maxOilStockTextBox.Size = new System.Drawing.Size(50, 19);
            this.maxOilStockTextBox.TabIndex = 31;
            // 
            // rangeTextBox
            // 
            this.rangeTextBox.Location = new System.Drawing.Point(87, 68);
            this.rangeTextBox.Name = "rangeTextBox";
            this.rangeTextBox.Size = new System.Drawing.Size(50, 19);
            this.rangeTextBox.TabIndex = 9;
            // 
            // transportCapabilityLabel
            // 
            this.transportCapabilityLabel.AutoSize = true;
            this.transportCapabilityLabel.Location = new System.Drawing.Point(8, 121);
            this.transportCapabilityLabel.Name = "transportCapabilityLabel";
            this.transportCapabilityLabel.Size = new System.Drawing.Size(62, 12);
            this.transportCapabilityLabel.TabIndex = 18;
            this.transportCapabilityLabel.Text = "Trans. Cap.";
            // 
            // maxOilStockLabel
            // 
            this.maxOilStockLabel.AutoSize = true;
            this.maxOilStockLabel.Location = new System.Drawing.Point(142, 121);
            this.maxOilStockLabel.Name = "maxOilStockLabel";
            this.maxOilStockLabel.Size = new System.Drawing.Size(52, 12);
            this.maxOilStockLabel.TabIndex = 30;
            this.maxOilStockLabel.Text = "Max Fuel";
            // 
            // transportCapabilityTextBox
            // 
            this.transportCapabilityTextBox.Location = new System.Drawing.Point(87, 118);
            this.transportCapabilityTextBox.Name = "transportCapabilityTextBox";
            this.transportCapabilityTextBox.Size = new System.Drawing.Size(50, 19);
            this.transportCapabilityTextBox.TabIndex = 19;
            // 
            // maxSupplyStockTextBox
            // 
            this.maxSupplyStockTextBox.Location = new System.Drawing.Point(231, 93);
            this.maxSupplyStockTextBox.Name = "maxSupplyStockTextBox";
            this.maxSupplyStockTextBox.Size = new System.Drawing.Size(50, 19);
            this.maxSupplyStockTextBox.TabIndex = 29;
            // 
            // maxSupplyStockLabel
            // 
            this.maxSupplyStockLabel.AutoSize = true;
            this.maxSupplyStockLabel.Location = new System.Drawing.Point(142, 96);
            this.maxSupplyStockLabel.Name = "maxSupplyStockLabel";
            this.maxSupplyStockLabel.Size = new System.Drawing.Size(64, 12);
            this.maxSupplyStockLabel.TabIndex = 28;
            this.maxSupplyStockLabel.Text = "Max Supply";
            // 
            // fuelConsumptionTextBox
            // 
            this.fuelConsumptionTextBox.Location = new System.Drawing.Point(231, 68);
            this.fuelConsumptionTextBox.Name = "fuelConsumptionTextBox";
            this.fuelConsumptionTextBox.Size = new System.Drawing.Size(50, 19);
            this.fuelConsumptionTextBox.TabIndex = 23;
            // 
            // fuelConsumptionLabel
            // 
            this.fuelConsumptionLabel.AutoSize = true;
            this.fuelConsumptionLabel.Location = new System.Drawing.Point(142, 71);
            this.fuelConsumptionLabel.Name = "fuelConsumptionLabel";
            this.fuelConsumptionLabel.Size = new System.Drawing.Size(59, 12);
            this.fuelConsumptionLabel.TabIndex = 22;
            this.fuelConsumptionLabel.Text = "Fuel Cons.";
            // 
            // supplyConsumptionTextBox
            // 
            this.supplyConsumptionTextBox.Location = new System.Drawing.Point(231, 43);
            this.supplyConsumptionTextBox.Name = "supplyConsumptionTextBox";
            this.supplyConsumptionTextBox.Size = new System.Drawing.Size(50, 19);
            this.supplyConsumptionTextBox.TabIndex = 21;
            // 
            // supplyConsumptionLabel
            // 
            this.supplyConsumptionLabel.AutoSize = true;
            this.supplyConsumptionLabel.Location = new System.Drawing.Point(142, 46);
            this.supplyConsumptionLabel.Name = "supplyConsumptionLabel";
            this.supplyConsumptionLabel.Size = new System.Drawing.Size(71, 12);
            this.supplyConsumptionLabel.TabIndex = 20;
            this.supplyConsumptionLabel.Text = "Supply Cons.";
            // 
            // suppressionTextBox
            // 
            this.suppressionTextBox.Location = new System.Drawing.Point(231, 18);
            this.suppressionTextBox.Name = "suppressionTextBox";
            this.suppressionTextBox.Size = new System.Drawing.Size(50, 19);
            this.suppressionTextBox.TabIndex = 15;
            // 
            // suppressionLabel
            // 
            this.suppressionLabel.AutoSize = true;
            this.suppressionLabel.Location = new System.Drawing.Point(142, 21);
            this.suppressionLabel.Name = "suppressionLabel";
            this.suppressionLabel.Size = new System.Drawing.Size(67, 12);
            this.suppressionLabel.TabIndex = 14;
            this.suppressionLabel.Text = "Suppression";
            // 
            // moraleTextBox
            // 
            this.moraleTextBox.Location = new System.Drawing.Point(87, 43);
            this.moraleTextBox.Name = "moraleTextBox";
            this.moraleTextBox.Size = new System.Drawing.Size(50, 19);
            this.moraleTextBox.TabIndex = 13;
            // 
            // moraleLabel
            // 
            this.moraleLabel.AutoSize = true;
            this.moraleLabel.Location = new System.Drawing.Point(7, 46);
            this.moraleLabel.Name = "moraleLabel";
            this.moraleLabel.Size = new System.Drawing.Size(39, 12);
            this.moraleLabel.TabIndex = 12;
            this.moraleLabel.Text = "Morale";
            // 
            // defaultOrganisationTextBox
            // 
            this.defaultOrganisationTextBox.Location = new System.Drawing.Point(87, 18);
            this.defaultOrganisationTextBox.Name = "defaultOrganisationTextBox";
            this.defaultOrganisationTextBox.Size = new System.Drawing.Size(50, 19);
            this.defaultOrganisationTextBox.TabIndex = 11;
            // 
            // defaultOrganisationLabel
            // 
            this.defaultOrganisationLabel.AutoSize = true;
            this.defaultOrganisationLabel.Location = new System.Drawing.Point(7, 21);
            this.defaultOrganisationLabel.Name = "defaultOrganisationLabel";
            this.defaultOrganisationLabel.Size = new System.Drawing.Size(69, 12);
            this.defaultOrganisationLabel.TabIndex = 10;
            this.defaultOrganisationLabel.Text = "Organisation";
            // 
            // modelIconPictureBox
            // 
            this.modelIconPictureBox.Location = new System.Drawing.Point(207, 6);
            this.modelIconPictureBox.Name = "modelIconPictureBox";
            this.modelIconPictureBox.Size = new System.Drawing.Size(44, 44);
            this.modelIconPictureBox.TabIndex = 1;
            this.modelIconPictureBox.TabStop = false;
            // 
            // modelImagePictureBox
            // 
            this.modelImagePictureBox.Location = new System.Drawing.Point(9, 6);
            this.modelImagePictureBox.Name = "modelImagePictureBox";
            this.modelImagePictureBox.Size = new System.Drawing.Size(192, 104);
            this.modelImagePictureBox.TabIndex = 0;
            this.modelImagePictureBox.TabStop = false;
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.closeButton.Location = new System.Drawing.Point(921, 694);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 53;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.saveButton.Location = new System.Drawing.Point(840, 694);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 52;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // reloadButton
            // 
            this.reloadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.reloadButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.reloadButton.Location = new System.Drawing.Point(759, 694);
            this.reloadButton.Name = "reloadButton";
            this.reloadButton.Size = new System.Drawing.Size(75, 23);
            this.reloadButton.TabIndex = 51;
            this.reloadButton.Text = "Reload";
            this.reloadButton.UseVisualStyleBackColor = true;
            // 
            // classNameLabel
            // 
            this.classNameLabel.AutoSize = true;
            this.classNameLabel.Location = new System.Drawing.Point(15, 13);
            this.classNameLabel.Name = "classNameLabel";
            this.classNameLabel.Size = new System.Drawing.Size(34, 12);
            this.classNameLabel.TabIndex = 0;
            this.classNameLabel.Text = "Name";
            // 
            // classNameTextBox
            // 
            this.classNameTextBox.Location = new System.Drawing.Point(86, 10);
            this.classNameTextBox.Name = "classNameTextBox";
            this.classNameTextBox.Size = new System.Drawing.Size(178, 19);
            this.classNameTextBox.TabIndex = 1;
            // 
            // classShortNameTextBox
            // 
            this.classShortNameTextBox.Location = new System.Drawing.Point(86, 38);
            this.classShortNameTextBox.Name = "classShortNameTextBox";
            this.classShortNameTextBox.Size = new System.Drawing.Size(178, 19);
            this.classShortNameTextBox.TabIndex = 3;
            // 
            // classShortNameLabel
            // 
            this.classShortNameLabel.AutoSize = true;
            this.classShortNameLabel.Location = new System.Drawing.Point(15, 41);
            this.classShortNameLabel.Name = "classShortNameLabel";
            this.classShortNameLabel.Size = new System.Drawing.Size(65, 12);
            this.classShortNameLabel.TabIndex = 2;
            this.classShortNameLabel.Text = "Short Name";
            // 
            // classDescTextBox
            // 
            this.classDescTextBox.Location = new System.Drawing.Point(86, 63);
            this.classDescTextBox.Multiline = true;
            this.classDescTextBox.Name = "classDescTextBox";
            this.classDescTextBox.Size = new System.Drawing.Size(178, 120);
            this.classDescTextBox.TabIndex = 5;
            // 
            // classDescLabel
            // 
            this.classDescLabel.AutoSize = true;
            this.classDescLabel.Location = new System.Drawing.Point(15, 66);
            this.classDescLabel.Name = "classDescLabel";
            this.classDescLabel.Size = new System.Drawing.Size(63, 12);
            this.classDescLabel.TabIndex = 4;
            this.classDescLabel.Text = "Description";
            // 
            // classShortDescTextBox
            // 
            this.classShortDescTextBox.Location = new System.Drawing.Point(86, 189);
            this.classShortDescTextBox.Name = "classShortDescTextBox";
            this.classShortDescTextBox.Size = new System.Drawing.Size(178, 19);
            this.classShortDescTextBox.TabIndex = 7;
            // 
            // classShortDescLabel
            // 
            this.classShortDescLabel.AutoSize = true;
            this.classShortDescLabel.Location = new System.Drawing.Point(15, 192);
            this.classShortDescLabel.Name = "classShortDescLabel";
            this.classShortDescLabel.Size = new System.Drawing.Size(62, 12);
            this.classShortDescLabel.TabIndex = 6;
            this.classShortDescLabel.Text = "Short Desc";
            // 
            // equipmentUpButton
            // 
            this.equipmentUpButton.Location = new System.Drawing.Point(198, 47);
            this.equipmentUpButton.Name = "equipmentUpButton";
            this.equipmentUpButton.Size = new System.Drawing.Size(75, 23);
            this.equipmentUpButton.TabIndex = 7;
            this.equipmentUpButton.Text = "Up";
            this.equipmentUpButton.UseVisualStyleBackColor = true;
            // 
            // equipmentDownButton
            // 
            this.equipmentDownButton.Location = new System.Drawing.Point(198, 76);
            this.equipmentDownButton.Name = "equipmentDownButton";
            this.equipmentDownButton.Size = new System.Drawing.Size(75, 23);
            this.equipmentDownButton.TabIndex = 8;
            this.equipmentDownButton.Text = "Down";
            this.equipmentDownButton.UseVisualStyleBackColor = true;
            // 
            // allowedBrigadesCheckedListBox
            // 
            this.allowedBrigadesCheckedListBox.FormattingEnabled = true;
            this.allowedBrigadesCheckedListBox.Location = new System.Drawing.Point(281, 28);
            this.allowedBrigadesCheckedListBox.MultiColumn = true;
            this.allowedBrigadesCheckedListBox.Name = "allowedBrigadesCheckedListBox";
            this.allowedBrigadesCheckedListBox.Size = new System.Drawing.Size(468, 228);
            this.allowedBrigadesCheckedListBox.TabIndex = 8;
            // 
            // branchComboBox
            // 
            this.branchComboBox.FormattingEnabled = true;
            this.branchComboBox.Location = new System.Drawing.Point(144, 214);
            this.branchComboBox.Name = "branchComboBox";
            this.branchComboBox.Size = new System.Drawing.Size(120, 20);
            this.branchComboBox.TabIndex = 9;
            // 
            // branchLabel
            // 
            this.branchLabel.AutoSize = true;
            this.branchLabel.Location = new System.Drawing.Point(15, 217);
            this.branchLabel.Name = "branchLabel";
            this.branchLabel.Size = new System.Drawing.Size(41, 12);
            this.branchLabel.TabIndex = 10;
            this.branchLabel.Text = "Branch";
            // 
            // allowedBrigadesLabel
            // 
            this.allowedBrigadesLabel.AutoSize = true;
            this.allowedBrigadesLabel.Location = new System.Drawing.Point(279, 13);
            this.allowedBrigadesLabel.Name = "allowedBrigadesLabel";
            this.allowedBrigadesLabel.Size = new System.Drawing.Size(94, 12);
            this.allowedBrigadesLabel.TabIndex = 11;
            this.allowedBrigadesLabel.Text = "Allowed Brigades";
            // 
            // detachableCheckBox
            // 
            this.detachableCheckBox.AutoSize = true;
            this.detachableCheckBox.Location = new System.Drawing.Point(183, 240);
            this.detachableCheckBox.Name = "detachableCheckBox";
            this.detachableCheckBox.Size = new System.Drawing.Size(81, 16);
            this.detachableCheckBox.TabIndex = 12;
            this.detachableCheckBox.Text = "Detachable";
            this.detachableCheckBox.UseVisualStyleBackColor = true;
            // 
            // gfxPrioLabel
            // 
            this.gfxPrioLabel.AutoSize = true;
            this.gfxPrioLabel.Location = new System.Drawing.Point(15, 265);
            this.gfxPrioLabel.Name = "gfxPrioLabel";
            this.gfxPrioLabel.Size = new System.Drawing.Size(91, 12);
            this.gfxPrioLabel.TabIndex = 13;
            this.gfxPrioLabel.Text = "Graphics Priority";
            // 
            // gfxPrioNumericUpDown
            // 
            this.gfxPrioNumericUpDown.Location = new System.Drawing.Point(144, 263);
            this.gfxPrioNumericUpDown.Name = "gfxPrioNumericUpDown";
            this.gfxPrioNumericUpDown.Size = new System.Drawing.Size(120, 19);
            this.gfxPrioNumericUpDown.TabIndex = 14;
            // 
            // productableCheckBox
            // 
            this.productableCheckBox.AutoSize = true;
            this.productableCheckBox.Location = new System.Drawing.Point(17, 240);
            this.productableCheckBox.Name = "productableCheckBox";
            this.productableCheckBox.Size = new System.Drawing.Size(84, 16);
            this.productableCheckBox.TabIndex = 15;
            this.productableCheckBox.Text = "Productable";
            this.productableCheckBox.UseVisualStyleBackColor = true;
            // 
            // realUnitTypeLabel
            // 
            this.realUnitTypeLabel.AutoSize = true;
            this.realUnitTypeLabel.Location = new System.Drawing.Point(15, 316);
            this.realUnitTypeLabel.Name = "realUnitTypeLabel";
            this.realUnitTypeLabel.Size = new System.Drawing.Size(82, 12);
            this.realUnitTypeLabel.TabIndex = 16;
            this.realUnitTypeLabel.Text = "Real Unit Type";
            // 
            // realUnitTypeComboBox
            // 
            this.realUnitTypeComboBox.FormattingEnabled = true;
            this.realUnitTypeComboBox.Location = new System.Drawing.Point(144, 313);
            this.realUnitTypeComboBox.Name = "realUnitTypeComboBox";
            this.realUnitTypeComboBox.Size = new System.Drawing.Size(120, 20);
            this.realUnitTypeComboBox.TabIndex = 17;
            // 
            // spriteTypeLabel
            // 
            this.spriteTypeLabel.AutoSize = true;
            this.spriteTypeLabel.Location = new System.Drawing.Point(15, 342);
            this.spriteTypeLabel.Name = "spriteTypeLabel";
            this.spriteTypeLabel.Size = new System.Drawing.Size(64, 12);
            this.spriteTypeLabel.TabIndex = 18;
            this.spriteTypeLabel.Text = "Sprite Type";
            // 
            // spriteTypeComboBox
            // 
            this.spriteTypeComboBox.FormattingEnabled = true;
            this.spriteTypeComboBox.Location = new System.Drawing.Point(144, 339);
            this.spriteTypeComboBox.Name = "spriteTypeComboBox";
            this.spriteTypeComboBox.Size = new System.Drawing.Size(120, 20);
            this.spriteTypeComboBox.TabIndex = 19;
            // 
            // transmuteLabel
            // 
            this.transmuteLabel.AutoSize = true;
            this.transmuteLabel.Location = new System.Drawing.Point(15, 368);
            this.transmuteLabel.Name = "transmuteLabel";
            this.transmuteLabel.Size = new System.Drawing.Size(84, 12);
            this.transmuteLabel.TabIndex = 20;
            this.transmuteLabel.Text = "Transmute Unit";
            // 
            // transmuteComboBox
            // 
            this.transmuteComboBox.FormattingEnabled = true;
            this.transmuteComboBox.Location = new System.Drawing.Point(144, 365);
            this.transmuteComboBox.Name = "transmuteComboBox";
            this.transmuteComboBox.Size = new System.Drawing.Size(120, 20);
            this.transmuteComboBox.TabIndex = 21;
            // 
            // upgradeListView
            // 
            this.upgradeListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.upgradeTypeColumnHeader,
            this.upgradeCostColumnHeader,
            this.upgradeTimeColumnHeader});
            this.upgradeListView.Location = new System.Drawing.Point(15, 23);
            this.upgradeListView.Name = "upgradeListView";
            this.upgradeListView.Size = new System.Drawing.Size(253, 103);
            this.upgradeListView.TabIndex = 22;
            this.upgradeListView.UseCompatibleStateImageBehavior = false;
            this.upgradeListView.View = System.Windows.Forms.View.Details;
            // 
            // militaryValueLabel
            // 
            this.militaryValueLabel.AutoSize = true;
            this.militaryValueLabel.Location = new System.Drawing.Point(15, 393);
            this.militaryValueLabel.Name = "militaryValueLabel";
            this.militaryValueLabel.Size = new System.Drawing.Size(76, 12);
            this.militaryValueLabel.TabIndex = 23;
            this.militaryValueLabel.Text = "Military Value";
            // 
            // militaryValueNumericUpDown
            // 
            this.militaryValueNumericUpDown.Location = new System.Drawing.Point(144, 391);
            this.militaryValueNumericUpDown.Name = "militaryValueNumericUpDown";
            this.militaryValueNumericUpDown.Size = new System.Drawing.Size(120, 19);
            this.militaryValueNumericUpDown.TabIndex = 24;
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
            this.upgradeGroupBox.Location = new System.Drawing.Point(281, 271);
            this.upgradeGroupBox.Name = "upgradeGroupBox";
            this.upgradeGroupBox.Size = new System.Drawing.Size(468, 139);
            this.upgradeGroupBox.TabIndex = 25;
            this.upgradeGroupBox.TabStop = false;
            this.upgradeGroupBox.Text = "Upgrade";
            // 
            // upgradeTypeColumnHeader
            // 
            this.upgradeTypeColumnHeader.Text = "Unit Type";
            this.upgradeTypeColumnHeader.Width = 150;
            // 
            // upgradeCostColumnHeader
            // 
            this.upgradeCostColumnHeader.Text = "Cost";
            this.upgradeCostColumnHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.upgradeCostColumnHeader.Width = 40;
            // 
            // upgradeTimeColumnHeader
            // 
            this.upgradeTimeColumnHeader.Text = "Time";
            this.upgradeTimeColumnHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.upgradeTimeColumnHeader.Width = 40;
            // 
            // upgradeAddButton
            // 
            this.upgradeAddButton.Location = new System.Drawing.Point(299, 23);
            this.upgradeAddButton.Name = "upgradeAddButton";
            this.upgradeAddButton.Size = new System.Drawing.Size(75, 23);
            this.upgradeAddButton.TabIndex = 25;
            this.upgradeAddButton.Text = "Add";
            this.upgradeAddButton.UseVisualStyleBackColor = true;
            // 
            // upgradeRemoveButton
            // 
            this.upgradeRemoveButton.Location = new System.Drawing.Point(380, 23);
            this.upgradeRemoveButton.Name = "upgradeRemoveButton";
            this.upgradeRemoveButton.Size = new System.Drawing.Size(75, 23);
            this.upgradeRemoveButton.TabIndex = 26;
            this.upgradeRemoveButton.Text = "Remove";
            this.upgradeRemoveButton.UseVisualStyleBackColor = true;
            // 
            // upgradeTypeLabel
            // 
            this.upgradeTypeLabel.AutoSize = true;
            this.upgradeTypeLabel.Location = new System.Drawing.Point(274, 57);
            this.upgradeTypeLabel.Name = "upgradeTypeLabel";
            this.upgradeTypeLabel.Size = new System.Drawing.Size(55, 12);
            this.upgradeTypeLabel.TabIndex = 27;
            this.upgradeTypeLabel.Text = "Unit Type";
            // 
            // upgradeTypeComboBox
            // 
            this.upgradeTypeComboBox.FormattingEnabled = true;
            this.upgradeTypeComboBox.Location = new System.Drawing.Point(335, 54);
            this.upgradeTypeComboBox.Name = "upgradeTypeComboBox";
            this.upgradeTypeComboBox.Size = new System.Drawing.Size(120, 20);
            this.upgradeTypeComboBox.TabIndex = 28;
            // 
            // upgradeCostLabel
            // 
            this.upgradeCostLabel.AutoSize = true;
            this.upgradeCostLabel.Location = new System.Drawing.Point(274, 83);
            this.upgradeCostLabel.Name = "upgradeCostLabel";
            this.upgradeCostLabel.Size = new System.Drawing.Size(29, 12);
            this.upgradeCostLabel.TabIndex = 29;
            this.upgradeCostLabel.Text = "Cost";
            // 
            // upgradeCostTextBox
            // 
            this.upgradeCostTextBox.Location = new System.Drawing.Point(335, 80);
            this.upgradeCostTextBox.Name = "upgradeCostTextBox";
            this.upgradeCostTextBox.Size = new System.Drawing.Size(120, 19);
            this.upgradeCostTextBox.TabIndex = 30;
            // 
            // upgradeTimeLabel
            // 
            this.upgradeTimeLabel.AutoSize = true;
            this.upgradeTimeLabel.Location = new System.Drawing.Point(274, 108);
            this.upgradeTimeLabel.Name = "upgradeTimeLabel";
            this.upgradeTimeLabel.Size = new System.Drawing.Size(30, 12);
            this.upgradeTimeLabel.TabIndex = 31;
            this.upgradeTimeLabel.Text = "Time";
            // 
            // upgradeTimeTextBox
            // 
            this.upgradeTimeTextBox.Location = new System.Drawing.Point(335, 105);
            this.upgradeTimeTextBox.Name = "upgradeTimeTextBox";
            this.upgradeTimeTextBox.Size = new System.Drawing.Size(120, 19);
            this.upgradeTimeTextBox.TabIndex = 32;
            // 
            // listPrioLabel
            // 
            this.listPrioLabel.AutoSize = true;
            this.listPrioLabel.Location = new System.Drawing.Point(15, 290);
            this.listPrioLabel.Name = "listPrioLabel";
            this.listPrioLabel.Size = new System.Drawing.Size(65, 12);
            this.listPrioLabel.TabIndex = 26;
            this.listPrioLabel.Text = "List Priority";
            // 
            // listPrioNumericUpDown
            // 
            this.listPrioNumericUpDown.Location = new System.Drawing.Point(144, 288);
            this.listPrioNumericUpDown.Name = "listPrioNumericUpDown";
            this.listPrioNumericUpDown.Size = new System.Drawing.Size(120, 19);
            this.listPrioNumericUpDown.TabIndex = 27;
            // 
            // UnitEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.reloadButton);
            this.Controls.Add(this.editTabControl);
            this.Controls.Add(this.classListBox);
            this.Controls.Add(this.countryListBox);
            this.Controls.Add(this.bottomButton);
            this.Controls.Add(this.downButton);
            this.Controls.Add(this.upButton);
            this.Controls.Add(this.topButton);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.cloneButton);
            this.Controls.Add(this.newButton);
            this.Controls.Add(this.leaderListView);
            this.Name = "UnitEditorForm";
            this.Text = "Unit Model Editor";
            this.editTabControl.ResumeLayout(false);
            this.classTabPage.ResumeLayout(false);
            this.classTabPage.PerformLayout();
            this.modelTabPage.ResumeLayout(false);
            this.modelTabPage.PerformLayout();
            this.equipmentGroupBox.ResumeLayout(false);
            this.equipmentGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.quantityNumericUpDown)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.gfxPrioNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.militaryValueNumericUpDown)).EndInit();
            this.upgradeGroupBox.ResumeLayout(false);
            this.upgradeGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listPrioNumericUpDown)).EndInit();
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
        private System.Windows.Forms.ListView leaderListView;
        private System.Windows.Forms.ColumnHeader classColumnHeader;
        private System.Windows.Forms.ColumnHeader branchColumnHeader;
        private System.Windows.Forms.ColumnHeader nameColumnHeader;
        private System.Windows.Forms.ColumnHeader costColumnHeader;
        private System.Windows.Forms.ColumnHeader buildTimeColumnHeader;
        private System.Windows.Forms.ColumnHeader manPowerSkillColumnHeader;
        private System.Windows.Forms.ColumnHeader supplyColumnHeader;
        private System.Windows.Forms.ColumnHeader fuelColumnHeader;
        private System.Windows.Forms.ColumnHeader noColumnHeader;
        private System.Windows.Forms.ListBox countryListBox;
        private System.Windows.Forms.ListBox classListBox;
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
        private System.Windows.Forms.NumericUpDown quantityNumericUpDown;
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
    }
}