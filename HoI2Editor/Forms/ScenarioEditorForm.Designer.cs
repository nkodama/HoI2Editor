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
            System.Windows.Forms.TabPage technologyTabPage;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScenarioEditorForm));
            System.Windows.Forms.Label tradeStartDateLabel;
            System.Windows.Forms.Label tradeEndDateLabel;
            System.Windows.Forms.Label tradeIdLabel;
            this.closeButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.reloadButton = new System.Windows.Forms.Button();
            this.provinceTabPage = new System.Windows.Forms.TabPage();
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
            this.allianceTabPage = new System.Windows.Forms.TabPage();
            this.mainTabPage = new System.Windows.Forms.TabPage();
            this.scenarioTabControl = new System.Windows.Forms.TabControl();
            this.oobTabPage = new System.Windows.Forms.TabPage();
            this.checkButton = new System.Windows.Forms.Button();
            technologyTabPage = new System.Windows.Forms.TabPage();
            tradeStartDateLabel = new System.Windows.Forms.Label();
            tradeEndDateLabel = new System.Windows.Forms.Label();
            tradeIdLabel = new System.Windows.Forms.Label();
            this.tradeTabPage.SuspendLayout();
            this.tradeDealsGroupBox.SuspendLayout();
            this.tradeInfoGroupBox.SuspendLayout();
            this.scenarioTabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // technologyTabPage
            // 
            technologyTabPage.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(technologyTabPage, "technologyTabPage");
            technologyTabPage.Name = "technologyTabPage";
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
            resources.ApplyResources(this.provinceTabPage, "provinceTabPage");
            this.provinceTabPage.Name = "provinceTabPage";
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
            resources.ApplyResources(this.relationTabPage, "relationTabPage");
            this.relationTabPage.Name = "relationTabPage";
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
            resources.ApplyResources(this.mainTabPage, "mainTabPage");
            this.mainTabPage.Name = "mainTabPage";
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
            resources.ApplyResources(this.oobTabPage, "oobTabPage");
            this.oobTabPage.Name = "oobTabPage";
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
            this.tradeTabPage.ResumeLayout(false);
            this.tradeDealsGroupBox.ResumeLayout(false);
            this.tradeDealsGroupBox.PerformLayout();
            this.tradeInfoGroupBox.ResumeLayout(false);
            this.tradeInfoGroupBox.PerformLayout();
            this.scenarioTabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button reloadButton;
        private System.Windows.Forms.TabPage provinceTabPage;
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
        private System.Windows.Forms.TabPage allianceTabPage;
        private System.Windows.Forms.TabPage mainTabPage;
        private System.Windows.Forms.TabControl scenarioTabControl;
        private System.Windows.Forms.ComboBox tradeCountryComboBox2;
        private System.Windows.Forms.Button checkButton;
        private System.Windows.Forms.TabPage oobTabPage;

    }
}