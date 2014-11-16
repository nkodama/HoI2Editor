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
            this.components = new System.ComponentModel.Container();
            this.scenarioTabControl = new System.Windows.Forms.TabControl();
            this.fileTabPage = new System.Windows.Forms.TabPage();
            this.folderTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.modRadioButton = new System.Windows.Forms.RadioButton();
            this.vanillaRadioButton = new System.Windows.Forms.RadioButton();
            this.eventDownButton = new System.Windows.Forms.Button();
            this.eventUpButton = new System.Windows.Forms.Button();
            this.eventRemoveButton = new System.Windows.Forms.Button();
            this.eventAddButton = new System.Windows.Forms.Button();
            this.eventTextBox = new System.Windows.Forms.TextBox();
            this.eventListBox = new System.Windows.Forms.ListBox();
            this.eventLabel = new System.Windows.Forms.Label();
            this.includeDownButton = new System.Windows.Forms.Button();
            this.includeUpButton = new System.Windows.Forms.Button();
            this.includeRemoveButton = new System.Windows.Forms.Button();
            this.includeAddButton = new System.Windows.Forms.Button();
            this.includeTextBox = new System.Windows.Forms.TextBox();
            this.includeListBox = new System.Windows.Forms.ListBox();
            this.includeLabel = new System.Windows.Forms.Label();
            this.selectableListBox = new System.Windows.Forms.ListBox();
            this.selectableLabel = new System.Windows.Forms.Label();
            this.majorRemoveButton = new System.Windows.Forms.Button();
            this.majorAddButton = new System.Windows.Forms.Button();
            this.majorDownButton = new System.Windows.Forms.Button();
            this.majorUpButton = new System.Windows.Forms.Button();
            this.majorListBox = new System.Windows.Forms.ListBox();
            this.majorLabel = new System.Windows.Forms.Label();
            this.endDayTextBox = new System.Windows.Forms.TextBox();
            this.endMonthTextBox = new System.Windows.Forms.TextBox();
            this.endYearTextBox = new System.Windows.Forms.TextBox();
            this.endDateLabel = new System.Windows.Forms.Label();
            this.startDayTextBox = new System.Windows.Forms.TextBox();
            this.startMonthTextBox = new System.Windows.Forms.TextBox();
            this.startYearTextBox = new System.Windows.Forms.TextBox();
            this.startDateLabel = new System.Windows.Forms.Label();
            this.panelImagePanel = new System.Windows.Forms.Panel();
            this.panelImageBrowseButton = new System.Windows.Forms.Button();
            this.panelImageTextBox = new System.Windows.Forms.TextBox();
            this.panelImageLabel = new System.Windows.Forms.Label();
            this.scenarioNameTextBox = new System.Windows.Forms.TextBox();
            this.scenarioNameLabel = new System.Windows.Forms.Label();
            this.fileTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.saveGamesRadioButton = new System.Windows.Forms.RadioButton();
            this.scenarioRadioButton = new System.Windows.Forms.RadioButton();
            this.scenarioListBox = new System.Windows.Forms.ListBox();
            this.miscToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.closeButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.reloadButton = new System.Windows.Forms.Button();
            this.exportRadioButton = new System.Windows.Forms.RadioButton();
            this.loadButton = new System.Windows.Forms.Button();
            this.scenarioTabControl.SuspendLayout();
            this.fileTabPage.SuspendLayout();
            this.folderTypeGroupBox.SuspendLayout();
            this.fileTypeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // scenarioTabControl
            // 
            this.scenarioTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scenarioTabControl.Controls.Add(this.fileTabPage);
            this.scenarioTabControl.Location = new System.Drawing.Point(0, 5);
            this.scenarioTabControl.Name = "scenarioTabControl";
            this.scenarioTabControl.SelectedIndex = 0;
            this.scenarioTabControl.Size = new System.Drawing.Size(984, 585);
            this.scenarioTabControl.TabIndex = 4;
            // 
            // fileTabPage
            // 
            this.fileTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.fileTabPage.Controls.Add(this.loadButton);
            this.fileTabPage.Controls.Add(this.folderTypeGroupBox);
            this.fileTabPage.Controls.Add(this.eventDownButton);
            this.fileTabPage.Controls.Add(this.eventUpButton);
            this.fileTabPage.Controls.Add(this.eventRemoveButton);
            this.fileTabPage.Controls.Add(this.eventAddButton);
            this.fileTabPage.Controls.Add(this.eventTextBox);
            this.fileTabPage.Controls.Add(this.eventListBox);
            this.fileTabPage.Controls.Add(this.eventLabel);
            this.fileTabPage.Controls.Add(this.includeDownButton);
            this.fileTabPage.Controls.Add(this.includeUpButton);
            this.fileTabPage.Controls.Add(this.includeRemoveButton);
            this.fileTabPage.Controls.Add(this.includeAddButton);
            this.fileTabPage.Controls.Add(this.includeTextBox);
            this.fileTabPage.Controls.Add(this.includeListBox);
            this.fileTabPage.Controls.Add(this.includeLabel);
            this.fileTabPage.Controls.Add(this.selectableListBox);
            this.fileTabPage.Controls.Add(this.selectableLabel);
            this.fileTabPage.Controls.Add(this.majorRemoveButton);
            this.fileTabPage.Controls.Add(this.majorAddButton);
            this.fileTabPage.Controls.Add(this.majorDownButton);
            this.fileTabPage.Controls.Add(this.majorUpButton);
            this.fileTabPage.Controls.Add(this.majorListBox);
            this.fileTabPage.Controls.Add(this.majorLabel);
            this.fileTabPage.Controls.Add(this.endDayTextBox);
            this.fileTabPage.Controls.Add(this.endMonthTextBox);
            this.fileTabPage.Controls.Add(this.endYearTextBox);
            this.fileTabPage.Controls.Add(this.endDateLabel);
            this.fileTabPage.Controls.Add(this.startDayTextBox);
            this.fileTabPage.Controls.Add(this.startMonthTextBox);
            this.fileTabPage.Controls.Add(this.startYearTextBox);
            this.fileTabPage.Controls.Add(this.startDateLabel);
            this.fileTabPage.Controls.Add(this.panelImagePanel);
            this.fileTabPage.Controls.Add(this.panelImageBrowseButton);
            this.fileTabPage.Controls.Add(this.panelImageTextBox);
            this.fileTabPage.Controls.Add(this.panelImageLabel);
            this.fileTabPage.Controls.Add(this.scenarioNameTextBox);
            this.fileTabPage.Controls.Add(this.scenarioNameLabel);
            this.fileTabPage.Controls.Add(this.fileTypeGroupBox);
            this.fileTabPage.Controls.Add(this.scenarioListBox);
            this.fileTabPage.Location = new System.Drawing.Point(4, 22);
            this.fileTabPage.Name = "fileTabPage";
            this.fileTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.fileTabPage.Size = new System.Drawing.Size(976, 559);
            this.fileTabPage.TabIndex = 0;
            this.fileTabPage.Text = "Main";
            // 
            // folderTypeGroupBox
            // 
            this.folderTypeGroupBox.Controls.Add(this.exportRadioButton);
            this.folderTypeGroupBox.Controls.Add(this.modRadioButton);
            this.folderTypeGroupBox.Controls.Add(this.vanillaRadioButton);
            this.folderTypeGroupBox.Location = new System.Drawing.Point(12, 458);
            this.folderTypeGroupBox.Name = "folderTypeGroupBox";
            this.folderTypeGroupBox.Size = new System.Drawing.Size(120, 85);
            this.folderTypeGroupBox.TabIndex = 39;
            this.folderTypeGroupBox.TabStop = false;
            // 
            // modRadioButton
            // 
            this.modRadioButton.AutoSize = true;
            this.modRadioButton.Location = new System.Drawing.Point(6, 40);
            this.modRadioButton.Name = "modRadioButton";
            this.modRadioButton.Size = new System.Drawing.Size(48, 16);
            this.modRadioButton.TabIndex = 1;
            this.modRadioButton.TabStop = true;
            this.modRadioButton.Text = "MOD";
            this.modRadioButton.UseVisualStyleBackColor = true;
            this.modRadioButton.CheckedChanged += new System.EventHandler(this.OnFolderRadioButtonCheckedChanged);
            // 
            // vanillaRadioButton
            // 
            this.vanillaRadioButton.AutoSize = true;
            this.vanillaRadioButton.Location = new System.Drawing.Point(6, 18);
            this.vanillaRadioButton.Name = "vanillaRadioButton";
            this.vanillaRadioButton.Size = new System.Drawing.Size(58, 16);
            this.vanillaRadioButton.TabIndex = 0;
            this.vanillaRadioButton.TabStop = true;
            this.vanillaRadioButton.Text = "Vanilla";
            this.vanillaRadioButton.UseVisualStyleBackColor = true;
            this.vanillaRadioButton.CheckedChanged += new System.EventHandler(this.OnFolderRadioButtonCheckedChanged);
            // 
            // eventDownButton
            // 
            this.eventDownButton.Location = new System.Drawing.Point(597, 441);
            this.eventDownButton.Name = "eventDownButton";
            this.eventDownButton.Size = new System.Drawing.Size(75, 23);
            this.eventDownButton.TabIndex = 38;
            this.eventDownButton.Text = "Down";
            this.eventDownButton.UseVisualStyleBackColor = true;
            // 
            // eventUpButton
            // 
            this.eventUpButton.Location = new System.Drawing.Point(597, 412);
            this.eventUpButton.Name = "eventUpButton";
            this.eventUpButton.Size = new System.Drawing.Size(75, 23);
            this.eventUpButton.TabIndex = 37;
            this.eventUpButton.Text = "Up";
            this.eventUpButton.UseVisualStyleBackColor = true;
            // 
            // eventRemoveButton
            // 
            this.eventRemoveButton.Location = new System.Drawing.Point(597, 381);
            this.eventRemoveButton.Name = "eventRemoveButton";
            this.eventRemoveButton.Size = new System.Drawing.Size(75, 23);
            this.eventRemoveButton.TabIndex = 36;
            this.eventRemoveButton.Text = "Remove";
            this.eventRemoveButton.UseVisualStyleBackColor = true;
            // 
            // eventAddButton
            // 
            this.eventAddButton.Location = new System.Drawing.Point(597, 352);
            this.eventAddButton.Name = "eventAddButton";
            this.eventAddButton.Size = new System.Drawing.Size(75, 23);
            this.eventAddButton.TabIndex = 35;
            this.eventAddButton.Text = "Add";
            this.eventAddButton.UseVisualStyleBackColor = true;
            // 
            // eventTextBox
            // 
            this.eventTextBox.Location = new System.Drawing.Point(597, 327);
            this.eventTextBox.Name = "eventTextBox";
            this.eventTextBox.Size = new System.Drawing.Size(100, 19);
            this.eventTextBox.TabIndex = 34;
            // 
            // eventListBox
            // 
            this.eventListBox.FormattingEnabled = true;
            this.eventListBox.ItemHeight = 12;
            this.eventListBox.Location = new System.Drawing.Point(455, 327);
            this.eventListBox.Name = "eventListBox";
            this.eventListBox.Size = new System.Drawing.Size(120, 208);
            this.eventListBox.TabIndex = 33;
            // 
            // eventLabel
            // 
            this.eventLabel.AutoSize = true;
            this.eventLabel.Location = new System.Drawing.Point(443, 312);
            this.eventLabel.Name = "eventLabel";
            this.eventLabel.Size = new System.Drawing.Size(34, 12);
            this.eventLabel.TabIndex = 32;
            this.eventLabel.Text = "Event";
            // 
            // includeDownButton
            // 
            this.includeDownButton.Location = new System.Drawing.Point(301, 441);
            this.includeDownButton.Name = "includeDownButton";
            this.includeDownButton.Size = new System.Drawing.Size(75, 23);
            this.includeDownButton.TabIndex = 31;
            this.includeDownButton.Text = "Down";
            this.includeDownButton.UseVisualStyleBackColor = true;
            // 
            // includeUpButton
            // 
            this.includeUpButton.Location = new System.Drawing.Point(301, 412);
            this.includeUpButton.Name = "includeUpButton";
            this.includeUpButton.Size = new System.Drawing.Size(75, 23);
            this.includeUpButton.TabIndex = 30;
            this.includeUpButton.Text = "Up";
            this.includeUpButton.UseVisualStyleBackColor = true;
            // 
            // includeRemoveButton
            // 
            this.includeRemoveButton.Location = new System.Drawing.Point(301, 381);
            this.includeRemoveButton.Name = "includeRemoveButton";
            this.includeRemoveButton.Size = new System.Drawing.Size(75, 23);
            this.includeRemoveButton.TabIndex = 29;
            this.includeRemoveButton.Text = "Remove";
            this.includeRemoveButton.UseVisualStyleBackColor = true;
            // 
            // includeAddButton
            // 
            this.includeAddButton.Location = new System.Drawing.Point(301, 352);
            this.includeAddButton.Name = "includeAddButton";
            this.includeAddButton.Size = new System.Drawing.Size(75, 23);
            this.includeAddButton.TabIndex = 28;
            this.includeAddButton.Text = "Add";
            this.includeAddButton.UseVisualStyleBackColor = true;
            // 
            // includeTextBox
            // 
            this.includeTextBox.Location = new System.Drawing.Point(301, 327);
            this.includeTextBox.Name = "includeTextBox";
            this.includeTextBox.Size = new System.Drawing.Size(100, 19);
            this.includeTextBox.TabIndex = 27;
            // 
            // includeListBox
            // 
            this.includeListBox.FormattingEnabled = true;
            this.includeListBox.ItemHeight = 12;
            this.includeListBox.Location = new System.Drawing.Point(159, 327);
            this.includeListBox.Name = "includeListBox";
            this.includeListBox.Size = new System.Drawing.Size(120, 208);
            this.includeListBox.TabIndex = 26;
            // 
            // includeLabel
            // 
            this.includeLabel.AutoSize = true;
            this.includeLabel.Location = new System.Drawing.Point(147, 312);
            this.includeLabel.Name = "includeLabel";
            this.includeLabel.Size = new System.Drawing.Size(41, 12);
            this.includeLabel.TabIndex = 25;
            this.includeLabel.Text = "Include";
            // 
            // selectableListBox
            // 
            this.selectableListBox.FormattingEnabled = true;
            this.selectableListBox.ItemHeight = 12;
            this.selectableListBox.Location = new System.Drawing.Point(622, 27);
            this.selectableListBox.Name = "selectableListBox";
            this.selectableListBox.Size = new System.Drawing.Size(120, 196);
            this.selectableListBox.TabIndex = 24;
            // 
            // selectableLabel
            // 
            this.selectableLabel.AutoSize = true;
            this.selectableLabel.Location = new System.Drawing.Point(620, 12);
            this.selectableLabel.Name = "selectableLabel";
            this.selectableLabel.Size = new System.Drawing.Size(111, 12);
            this.selectableLabel.TabIndex = 23;
            this.selectableLabel.Text = "Selectable Countries";
            // 
            // majorRemoveButton
            // 
            this.majorRemoveButton.Location = new System.Drawing.Point(571, 125);
            this.majorRemoveButton.Name = "majorRemoveButton";
            this.majorRemoveButton.Size = new System.Drawing.Size(45, 23);
            this.majorRemoveButton.TabIndex = 22;
            this.majorRemoveButton.Text = "->";
            this.majorRemoveButton.UseVisualStyleBackColor = true;
            // 
            // majorAddButton
            // 
            this.majorAddButton.Location = new System.Drawing.Point(571, 79);
            this.majorAddButton.Name = "majorAddButton";
            this.majorAddButton.Size = new System.Drawing.Size(45, 23);
            this.majorAddButton.TabIndex = 21;
            this.majorAddButton.Text = "<-";
            this.majorAddButton.UseVisualStyleBackColor = true;
            // 
            // majorDownButton
            // 
            this.majorDownButton.Location = new System.Drawing.Point(445, 258);
            this.majorDownButton.Name = "majorDownButton";
            this.majorDownButton.Size = new System.Drawing.Size(75, 23);
            this.majorDownButton.TabIndex = 19;
            this.majorDownButton.Text = "Down";
            this.majorDownButton.UseVisualStyleBackColor = true;
            // 
            // majorUpButton
            // 
            this.majorUpButton.Location = new System.Drawing.Point(445, 229);
            this.majorUpButton.Name = "majorUpButton";
            this.majorUpButton.Size = new System.Drawing.Size(75, 23);
            this.majorUpButton.TabIndex = 18;
            this.majorUpButton.Text = "Up";
            this.majorUpButton.UseVisualStyleBackColor = true;
            // 
            // majorListBox
            // 
            this.majorListBox.FormattingEnabled = true;
            this.majorListBox.ItemHeight = 12;
            this.majorListBox.Location = new System.Drawing.Point(445, 27);
            this.majorListBox.Name = "majorListBox";
            this.majorListBox.Size = new System.Drawing.Size(120, 196);
            this.majorListBox.TabIndex = 17;
            // 
            // majorLabel
            // 
            this.majorLabel.AutoSize = true;
            this.majorLabel.Location = new System.Drawing.Point(443, 12);
            this.majorLabel.Name = "majorLabel";
            this.majorLabel.Size = new System.Drawing.Size(86, 12);
            this.majorLabel.TabIndex = 16;
            this.majorLabel.Text = "Major Countries";
            // 
            // endDayTextBox
            // 
            this.endDayTextBox.Location = new System.Drawing.Point(311, 269);
            this.endDayTextBox.Name = "endDayTextBox";
            this.endDayTextBox.Size = new System.Drawing.Size(70, 19);
            this.endDayTextBox.TabIndex = 15;
            // 
            // endMonthTextBox
            // 
            this.endMonthTextBox.Location = new System.Drawing.Point(235, 269);
            this.endMonthTextBox.Name = "endMonthTextBox";
            this.endMonthTextBox.Size = new System.Drawing.Size(70, 19);
            this.endMonthTextBox.TabIndex = 14;
            // 
            // endYearTextBox
            // 
            this.endYearTextBox.Location = new System.Drawing.Point(159, 269);
            this.endYearTextBox.Name = "endYearTextBox";
            this.endYearTextBox.Size = new System.Drawing.Size(70, 19);
            this.endYearTextBox.TabIndex = 13;
            // 
            // endDateLabel
            // 
            this.endDateLabel.AutoSize = true;
            this.endDateLabel.Location = new System.Drawing.Point(147, 254);
            this.endDateLabel.Name = "endDateLabel";
            this.endDateLabel.Size = new System.Drawing.Size(52, 12);
            this.endDateLabel.TabIndex = 12;
            this.endDateLabel.Text = "End Date";
            // 
            // startDayTextBox
            // 
            this.startDayTextBox.Location = new System.Drawing.Point(311, 223);
            this.startDayTextBox.Name = "startDayTextBox";
            this.startDayTextBox.Size = new System.Drawing.Size(70, 19);
            this.startDayTextBox.TabIndex = 11;
            // 
            // startMonthTextBox
            // 
            this.startMonthTextBox.Location = new System.Drawing.Point(235, 223);
            this.startMonthTextBox.Name = "startMonthTextBox";
            this.startMonthTextBox.Size = new System.Drawing.Size(70, 19);
            this.startMonthTextBox.TabIndex = 10;
            // 
            // startYearTextBox
            // 
            this.startYearTextBox.Location = new System.Drawing.Point(159, 223);
            this.startYearTextBox.Name = "startYearTextBox";
            this.startYearTextBox.Size = new System.Drawing.Size(70, 19);
            this.startYearTextBox.TabIndex = 9;
            // 
            // startDateLabel
            // 
            this.startDateLabel.AutoSize = true;
            this.startDateLabel.Location = new System.Drawing.Point(147, 208);
            this.startDateLabel.Name = "startDateLabel";
            this.startDateLabel.Size = new System.Drawing.Size(58, 12);
            this.startDateLabel.TabIndex = 8;
            this.startDateLabel.Text = "Start Date";
            // 
            // panelImagePanel
            // 
            this.panelImagePanel.Location = new System.Drawing.Point(149, 95);
            this.panelImagePanel.Name = "panelImagePanel";
            this.panelImagePanel.Size = new System.Drawing.Size(200, 91);
            this.panelImagePanel.TabIndex = 7;
            // 
            // panelImageBrowseButton
            // 
            this.panelImageBrowseButton.Location = new System.Drawing.Point(336, 66);
            this.panelImageBrowseButton.Name = "panelImageBrowseButton";
            this.panelImageBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.panelImageBrowseButton.TabIndex = 6;
            this.panelImageBrowseButton.Text = "Browse";
            this.panelImageBrowseButton.UseVisualStyleBackColor = true;
            // 
            // panelImageTextBox
            // 
            this.panelImageTextBox.Location = new System.Drawing.Point(159, 68);
            this.panelImageTextBox.Name = "panelImageTextBox";
            this.panelImageTextBox.Size = new System.Drawing.Size(171, 19);
            this.panelImageTextBox.TabIndex = 5;
            // 
            // panelImageLabel
            // 
            this.panelImageLabel.AutoSize = true;
            this.panelImageLabel.Location = new System.Drawing.Point(147, 53);
            this.panelImageLabel.Name = "panelImageLabel";
            this.panelImageLabel.Size = new System.Drawing.Size(67, 12);
            this.panelImageLabel.TabIndex = 4;
            this.panelImageLabel.Text = "Panel Image";
            // 
            // scenarioNameTextBox
            // 
            this.scenarioNameTextBox.Location = new System.Drawing.Point(159, 27);
            this.scenarioNameTextBox.Name = "scenarioNameTextBox";
            this.scenarioNameTextBox.Size = new System.Drawing.Size(171, 19);
            this.scenarioNameTextBox.TabIndex = 3;
            // 
            // scenarioNameLabel
            // 
            this.scenarioNameLabel.AutoSize = true;
            this.scenarioNameLabel.Location = new System.Drawing.Point(147, 12);
            this.scenarioNameLabel.Name = "scenarioNameLabel";
            this.scenarioNameLabel.Size = new System.Drawing.Size(34, 12);
            this.scenarioNameLabel.TabIndex = 2;
            this.scenarioNameLabel.Text = "Name";
            // 
            // fileTypeGroupBox
            // 
            this.fileTypeGroupBox.Controls.Add(this.saveGamesRadioButton);
            this.fileTypeGroupBox.Controls.Add(this.scenarioRadioButton);
            this.fileTypeGroupBox.Location = new System.Drawing.Point(12, 387);
            this.fileTypeGroupBox.Name = "fileTypeGroupBox";
            this.fileTypeGroupBox.Size = new System.Drawing.Size(120, 65);
            this.fileTypeGroupBox.TabIndex = 1;
            this.fileTypeGroupBox.TabStop = false;
            // 
            // saveGamesRadioButton
            // 
            this.saveGamesRadioButton.AutoSize = true;
            this.saveGamesRadioButton.Location = new System.Drawing.Point(6, 40);
            this.saveGamesRadioButton.Name = "saveGamesRadioButton";
            this.saveGamesRadioButton.Size = new System.Drawing.Size(87, 16);
            this.saveGamesRadioButton.TabIndex = 1;
            this.saveGamesRadioButton.TabStop = true;
            this.saveGamesRadioButton.Text = "Save Games";
            this.saveGamesRadioButton.UseVisualStyleBackColor = true;
            // 
            // scenarioRadioButton
            // 
            this.scenarioRadioButton.AutoSize = true;
            this.scenarioRadioButton.Checked = true;
            this.scenarioRadioButton.Location = new System.Drawing.Point(6, 18);
            this.scenarioRadioButton.Name = "scenarioRadioButton";
            this.scenarioRadioButton.Size = new System.Drawing.Size(67, 16);
            this.scenarioRadioButton.TabIndex = 0;
            this.scenarioRadioButton.TabStop = true;
            this.scenarioRadioButton.Text = "Scenario";
            this.scenarioRadioButton.UseVisualStyleBackColor = true;
            // 
            // scenarioListBox
            // 
            this.scenarioListBox.FormattingEnabled = true;
            this.scenarioListBox.ItemHeight = 12;
            this.scenarioListBox.Location = new System.Drawing.Point(12, 12);
            this.scenarioListBox.Name = "scenarioListBox";
            this.scenarioListBox.Size = new System.Drawing.Size(120, 340);
            this.scenarioListBox.TabIndex = 0;
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.closeButton.Location = new System.Drawing.Point(897, 596);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 7;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.OnCloseButtonClick);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.saveButton.Location = new System.Drawing.Point(816, 596);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 6;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.OnSaveButtonClick);
            // 
            // reloadButton
            // 
            this.reloadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.reloadButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.reloadButton.Location = new System.Drawing.Point(735, 596);
            this.reloadButton.Name = "reloadButton";
            this.reloadButton.Size = new System.Drawing.Size(75, 23);
            this.reloadButton.TabIndex = 5;
            this.reloadButton.Text = "Reload";
            this.reloadButton.UseVisualStyleBackColor = true;
            this.reloadButton.Click += new System.EventHandler(this.OnReloadButtonClick);
            // 
            // exportRadioButton
            // 
            this.exportRadioButton.AutoSize = true;
            this.exportRadioButton.Location = new System.Drawing.Point(6, 62);
            this.exportRadioButton.Name = "exportRadioButton";
            this.exportRadioButton.Size = new System.Drawing.Size(56, 16);
            this.exportRadioButton.TabIndex = 2;
            this.exportRadioButton.TabStop = true;
            this.exportRadioButton.Text = "Export";
            this.exportRadioButton.UseVisualStyleBackColor = true;
            this.exportRadioButton.CheckedChanged += new System.EventHandler(this.OnFolderRadioButtonCheckedChanged);
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(12, 358);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(75, 23);
            this.loadButton.TabIndex = 40;
            this.loadButton.Text = "Load";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.OnLoadButtonClick);
            // 
            // ScenarioEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 631);
            this.Controls.Add(this.scenarioTabControl);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.reloadButton);
            this.Name = "ScenarioEditorForm";
            this.Text = "Scenario Editor";
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.Move += new System.EventHandler(this.OnFormMove);
            this.Resize += new System.EventHandler(this.OnFormResize);
            this.scenarioTabControl.ResumeLayout(false);
            this.fileTabPage.ResumeLayout(false);
            this.fileTabPage.PerformLayout();
            this.folderTypeGroupBox.ResumeLayout(false);
            this.folderTypeGroupBox.PerformLayout();
            this.fileTypeGroupBox.ResumeLayout(false);
            this.fileTypeGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl scenarioTabControl;
        private System.Windows.Forms.ToolTip miscToolTip;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button reloadButton;
        private System.Windows.Forms.TabPage fileTabPage;
        private System.Windows.Forms.TextBox scenarioNameTextBox;
        private System.Windows.Forms.Label scenarioNameLabel;
        private System.Windows.Forms.GroupBox fileTypeGroupBox;
        private System.Windows.Forms.RadioButton saveGamesRadioButton;
        private System.Windows.Forms.RadioButton scenarioRadioButton;
        private System.Windows.Forms.ListBox scenarioListBox;
        private System.Windows.Forms.Label panelImageLabel;
        private System.Windows.Forms.Panel panelImagePanel;
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
        private System.Windows.Forms.ListBox selectableListBox;
        private System.Windows.Forms.Label selectableLabel;
        private System.Windows.Forms.Button majorRemoveButton;
        private System.Windows.Forms.Button majorAddButton;
        private System.Windows.Forms.Button majorDownButton;
        private System.Windows.Forms.Button majorUpButton;
        private System.Windows.Forms.ListBox majorListBox;
        private System.Windows.Forms.Label majorLabel;
        private System.Windows.Forms.TextBox includeTextBox;
        private System.Windows.Forms.ListBox includeListBox;
        private System.Windows.Forms.Label includeLabel;
        private System.Windows.Forms.Button eventDownButton;
        private System.Windows.Forms.Button eventUpButton;
        private System.Windows.Forms.Button eventRemoveButton;
        private System.Windows.Forms.Button eventAddButton;
        private System.Windows.Forms.TextBox eventTextBox;
        private System.Windows.Forms.ListBox eventListBox;
        private System.Windows.Forms.Label eventLabel;
        private System.Windows.Forms.Button includeDownButton;
        private System.Windows.Forms.Button includeUpButton;
        private System.Windows.Forms.Button includeRemoveButton;
        private System.Windows.Forms.Button includeAddButton;
        private System.Windows.Forms.GroupBox folderTypeGroupBox;
        private System.Windows.Forms.RadioButton modRadioButton;
        private System.Windows.Forms.RadioButton vanillaRadioButton;
        private System.Windows.Forms.RadioButton exportRadioButton;
        private System.Windows.Forms.Button loadButton;

    }
}