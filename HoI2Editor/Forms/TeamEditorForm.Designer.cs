namespace HoI2Editor.Forms
{
    partial class TeamEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TeamEditorForm));
            this.closeButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.reloadButton = new System.Windows.Forms.Button();
            this.teamPictureBox = new System.Windows.Forms.PictureBox();
            this.pictureNameReferButton = new System.Windows.Forms.Button();
            this.pictureNameTextBox = new System.Windows.Forms.TextBox();
            this.pictureNameLabel = new System.Windows.Forms.Label();
            this.endYearNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.endYearLabel = new System.Windows.Forms.Label();
            this.startYearNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.startYearLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.idNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.countryListBox = new System.Windows.Forms.ListBox();
            this.teamListView = new System.Windows.Forms.ListView();
            this.countryColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.idColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.skillColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.startYearColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.endYearColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.specialityColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.idLabel = new System.Windows.Forms.Label();
            this.countryComboBox = new System.Windows.Forms.ComboBox();
            this.countryLabel = new System.Windows.Forms.Label();
            this.countryAllButton = new System.Windows.Forms.Button();
            this.bottomButton = new System.Windows.Forms.Button();
            this.downButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.topButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.cloneButton = new System.Windows.Forms.Button();
            this.newButton = new System.Windows.Forms.Button();
            this.skillNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.skillLabel = new System.Windows.Forms.Label();
            this.specialityGroupBox = new System.Windows.Forms.GroupBox();
            this.sortAbcButton = new System.Windows.Forms.Button();
            this.sortIdButton = new System.Windows.Forms.Button();
            this.specialityComboBox7 = new System.Windows.Forms.ComboBox();
            this.specialityComboBox6 = new System.Windows.Forms.ComboBox();
            this.specialityComboBox5 = new System.Windows.Forms.ComboBox();
            this.specialityComboBox4 = new System.Windows.Forms.ComboBox();
            this.specialityComboBox3 = new System.Windows.Forms.ComboBox();
            this.specialityComboBox2 = new System.Windows.Forms.ComboBox();
            this.specialityComboBox1 = new System.Windows.Forms.ComboBox();
            this.batchButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.teamPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endYearNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startYearNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.idNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.skillNumericUpDown)).BeginInit();
            this.specialityGroupBox.SuspendLayout();
            this.SuspendLayout();
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
            // teamPictureBox
            // 
            resources.ApplyResources(this.teamPictureBox, "teamPictureBox");
            this.teamPictureBox.Name = "teamPictureBox";
            this.teamPictureBox.TabStop = false;
            // 
            // pictureNameReferButton
            // 
            resources.ApplyResources(this.pictureNameReferButton, "pictureNameReferButton");
            this.pictureNameReferButton.Name = "pictureNameReferButton";
            this.pictureNameReferButton.UseVisualStyleBackColor = true;
            this.pictureNameReferButton.Click += new System.EventHandler(this.OnPictureNameReferButtonClick);
            // 
            // pictureNameTextBox
            // 
            resources.ApplyResources(this.pictureNameTextBox, "pictureNameTextBox");
            this.pictureNameTextBox.Name = "pictureNameTextBox";
            this.pictureNameTextBox.TextChanged += new System.EventHandler(this.OnPictureNameTextBoxTextChanged);
            // 
            // pictureNameLabel
            // 
            resources.ApplyResources(this.pictureNameLabel, "pictureNameLabel");
            this.pictureNameLabel.Name = "pictureNameLabel";
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
            // endYearLabel
            // 
            resources.ApplyResources(this.endYearLabel, "endYearLabel");
            this.endYearLabel.Name = "endYearLabel";
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
            // startYearLabel
            // 
            resources.ApplyResources(this.startYearLabel, "startYearLabel");
            this.startYearLabel.Name = "startYearLabel";
            // 
            // nameTextBox
            // 
            resources.ApplyResources(this.nameTextBox, "nameTextBox");
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.TextChanged += new System.EventHandler(this.OnNameTextBoxTextChanged);
            // 
            // nameLabel
            // 
            resources.ApplyResources(this.nameLabel, "nameLabel");
            this.nameLabel.Name = "nameLabel";
            // 
            // idNumericUpDown
            // 
            resources.ApplyResources(this.idNumericUpDown, "idNumericUpDown");
            this.idNumericUpDown.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.idNumericUpDown.Name = "idNumericUpDown";
            this.idNumericUpDown.ValueChanged += new System.EventHandler(this.OnIdNumericUpDownValueChanged);
            // 
            // countryListBox
            // 
            resources.ApplyResources(this.countryListBox, "countryListBox");
            this.countryListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.countryListBox.FormattingEnabled = true;
            this.countryListBox.MultiColumn = true;
            this.countryListBox.Name = "countryListBox";
            this.countryListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.countryListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnCountryListBoxDrawItem);
            this.countryListBox.SelectedIndexChanged += new System.EventHandler(this.OnCountryListBoxSelectedIndexChanged);
            // 
            // teamListView
            // 
            resources.ApplyResources(this.teamListView, "teamListView");
            this.teamListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.countryColumnHeader,
            this.idColumnHeader,
            this.nameColumnHeader,
            this.skillColumnHeader,
            this.startYearColumnHeader,
            this.endYearColumnHeader,
            this.specialityColumnHeader});
            this.teamListView.FullRowSelect = true;
            this.teamListView.GridLines = true;
            this.teamListView.HideSelection = false;
            this.teamListView.MultiSelect = false;
            this.teamListView.Name = "teamListView";
            this.teamListView.OwnerDraw = true;
            this.teamListView.UseCompatibleStateImageBehavior = false;
            this.teamListView.View = System.Windows.Forms.View.Details;
            this.teamListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.OnLeaderListViewColumnClick);
            this.teamListView.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.OnTeamListViewColumnWidthChanged);
            this.teamListView.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.OnTeamListViewDrawColumnHeader);
            this.teamListView.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.OnTeamListViewDrawSubItem);
            this.teamListView.SelectedIndexChanged += new System.EventHandler(this.OnTeamListViewSelectedIndexChanged);
            // 
            // countryColumnHeader
            // 
            resources.ApplyResources(this.countryColumnHeader, "countryColumnHeader");
            // 
            // idColumnHeader
            // 
            resources.ApplyResources(this.idColumnHeader, "idColumnHeader");
            // 
            // nameColumnHeader
            // 
            resources.ApplyResources(this.nameColumnHeader, "nameColumnHeader");
            // 
            // skillColumnHeader
            // 
            resources.ApplyResources(this.skillColumnHeader, "skillColumnHeader");
            // 
            // startYearColumnHeader
            // 
            resources.ApplyResources(this.startYearColumnHeader, "startYearColumnHeader");
            // 
            // endYearColumnHeader
            // 
            resources.ApplyResources(this.endYearColumnHeader, "endYearColumnHeader");
            // 
            // specialityColumnHeader
            // 
            resources.ApplyResources(this.specialityColumnHeader, "specialityColumnHeader");
            // 
            // idLabel
            // 
            resources.ApplyResources(this.idLabel, "idLabel");
            this.idLabel.Name = "idLabel";
            // 
            // countryComboBox
            // 
            resources.ApplyResources(this.countryComboBox, "countryComboBox");
            this.countryComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.countryComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.countryComboBox.FormattingEnabled = true;
            this.countryComboBox.Name = "countryComboBox";
            this.countryComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnCountryComboBoxDrawItem);
            this.countryComboBox.SelectedIndexChanged += new System.EventHandler(this.OnCountryComboBoxSelectedIndexChanged);
            // 
            // countryLabel
            // 
            resources.ApplyResources(this.countryLabel, "countryLabel");
            this.countryLabel.Name = "countryLabel";
            // 
            // countryAllButton
            // 
            resources.ApplyResources(this.countryAllButton, "countryAllButton");
            this.countryAllButton.Name = "countryAllButton";
            this.countryAllButton.UseVisualStyleBackColor = true;
            this.countryAllButton.Click += new System.EventHandler(this.OnCountryAllButtonClick);
            // 
            // bottomButton
            // 
            resources.ApplyResources(this.bottomButton, "bottomButton");
            this.bottomButton.Name = "bottomButton";
            this.bottomButton.UseVisualStyleBackColor = true;
            this.bottomButton.Click += new System.EventHandler(this.OnBottomButtonClick);
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
            // skillNumericUpDown
            // 
            resources.ApplyResources(this.skillNumericUpDown, "skillNumericUpDown");
            this.skillNumericUpDown.Name = "skillNumericUpDown";
            this.skillNumericUpDown.ValueChanged += new System.EventHandler(this.OnSkillNumericUpDownValueChanged);
            // 
            // skillLabel
            // 
            resources.ApplyResources(this.skillLabel, "skillLabel");
            this.skillLabel.Name = "skillLabel";
            // 
            // specialityGroupBox
            // 
            resources.ApplyResources(this.specialityGroupBox, "specialityGroupBox");
            this.specialityGroupBox.Controls.Add(this.sortAbcButton);
            this.specialityGroupBox.Controls.Add(this.sortIdButton);
            this.specialityGroupBox.Controls.Add(this.specialityComboBox7);
            this.specialityGroupBox.Controls.Add(this.specialityComboBox6);
            this.specialityGroupBox.Controls.Add(this.specialityComboBox5);
            this.specialityGroupBox.Controls.Add(this.specialityComboBox4);
            this.specialityGroupBox.Controls.Add(this.specialityComboBox3);
            this.specialityGroupBox.Controls.Add(this.specialityComboBox2);
            this.specialityGroupBox.Controls.Add(this.specialityComboBox1);
            this.specialityGroupBox.Name = "specialityGroupBox";
            this.specialityGroupBox.TabStop = false;
            // 
            // sortAbcButton
            // 
            resources.ApplyResources(this.sortAbcButton, "sortAbcButton");
            this.sortAbcButton.Name = "sortAbcButton";
            this.sortAbcButton.UseVisualStyleBackColor = true;
            this.sortAbcButton.Click += new System.EventHandler(this.OnSortAbcButtonClick);
            // 
            // sortIdButton
            // 
            resources.ApplyResources(this.sortIdButton, "sortIdButton");
            this.sortIdButton.Name = "sortIdButton";
            this.sortIdButton.UseVisualStyleBackColor = true;
            this.sortIdButton.Click += new System.EventHandler(this.OnSortIdButtonClick);
            // 
            // specialityComboBox7
            // 
            this.specialityComboBox7.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.specialityComboBox7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.specialityComboBox7.FormattingEnabled = true;
            resources.ApplyResources(this.specialityComboBox7, "specialityComboBox7");
            this.specialityComboBox7.Name = "specialityComboBox7";
            this.specialityComboBox7.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnSpecialityComboBoxDrawItem);
            this.specialityComboBox7.SelectedIndexChanged += new System.EventHandler(this.OnSpecialityComboBoxSelectedIndexChanged);
            // 
            // specialityComboBox6
            // 
            this.specialityComboBox6.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.specialityComboBox6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.specialityComboBox6.FormattingEnabled = true;
            resources.ApplyResources(this.specialityComboBox6, "specialityComboBox6");
            this.specialityComboBox6.Name = "specialityComboBox6";
            this.specialityComboBox6.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnSpecialityComboBoxDrawItem);
            this.specialityComboBox6.SelectedIndexChanged += new System.EventHandler(this.OnSpecialityComboBoxSelectedIndexChanged);
            // 
            // specialityComboBox5
            // 
            this.specialityComboBox5.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.specialityComboBox5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.specialityComboBox5.FormattingEnabled = true;
            resources.ApplyResources(this.specialityComboBox5, "specialityComboBox5");
            this.specialityComboBox5.Name = "specialityComboBox5";
            this.specialityComboBox5.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnSpecialityComboBoxDrawItem);
            this.specialityComboBox5.SelectedIndexChanged += new System.EventHandler(this.OnSpecialityComboBoxSelectedIndexChanged);
            // 
            // specialityComboBox4
            // 
            this.specialityComboBox4.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.specialityComboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.specialityComboBox4.FormattingEnabled = true;
            resources.ApplyResources(this.specialityComboBox4, "specialityComboBox4");
            this.specialityComboBox4.Name = "specialityComboBox4";
            this.specialityComboBox4.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnSpecialityComboBoxDrawItem);
            this.specialityComboBox4.SelectedIndexChanged += new System.EventHandler(this.OnSpecialityComboBoxSelectedIndexChanged);
            // 
            // specialityComboBox3
            // 
            this.specialityComboBox3.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.specialityComboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.specialityComboBox3.FormattingEnabled = true;
            resources.ApplyResources(this.specialityComboBox3, "specialityComboBox3");
            this.specialityComboBox3.Name = "specialityComboBox3";
            this.specialityComboBox3.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnSpecialityComboBoxDrawItem);
            this.specialityComboBox3.SelectedIndexChanged += new System.EventHandler(this.OnSpecialityComboBoxSelectedIndexChanged);
            // 
            // specialityComboBox2
            // 
            this.specialityComboBox2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.specialityComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.specialityComboBox2.FormattingEnabled = true;
            resources.ApplyResources(this.specialityComboBox2, "specialityComboBox2");
            this.specialityComboBox2.Name = "specialityComboBox2";
            this.specialityComboBox2.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnSpecialityComboBoxDrawItem);
            this.specialityComboBox2.SelectedIndexChanged += new System.EventHandler(this.OnSpecialityComboBoxSelectedIndexChanged);
            // 
            // specialityComboBox1
            // 
            this.specialityComboBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.specialityComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.specialityComboBox1.FormattingEnabled = true;
            resources.ApplyResources(this.specialityComboBox1, "specialityComboBox1");
            this.specialityComboBox1.Name = "specialityComboBox1";
            this.specialityComboBox1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnSpecialityComboBoxDrawItem);
            this.specialityComboBox1.SelectedIndexChanged += new System.EventHandler(this.OnSpecialityComboBoxSelectedIndexChanged);
            // 
            // batchButton
            // 
            resources.ApplyResources(this.batchButton, "batchButton");
            this.batchButton.Name = "batchButton";
            this.batchButton.UseVisualStyleBackColor = true;
            this.batchButton.Click += new System.EventHandler(this.OnBatchButtonClick);
            // 
            // TeamEditorForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.batchButton);
            this.Controls.Add(this.specialityGroupBox);
            this.Controls.Add(this.skillNumericUpDown);
            this.Controls.Add(this.skillLabel);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.reloadButton);
            this.Controls.Add(this.teamPictureBox);
            this.Controls.Add(this.pictureNameReferButton);
            this.Controls.Add(this.pictureNameTextBox);
            this.Controls.Add(this.pictureNameLabel);
            this.Controls.Add(this.endYearNumericUpDown);
            this.Controls.Add(this.endYearLabel);
            this.Controls.Add(this.startYearNumericUpDown);
            this.Controls.Add(this.startYearLabel);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.idNumericUpDown);
            this.Controls.Add(this.countryListBox);
            this.Controls.Add(this.teamListView);
            this.Controls.Add(this.idLabel);
            this.Controls.Add(this.countryComboBox);
            this.Controls.Add(this.countryLabel);
            this.Controls.Add(this.countryAllButton);
            this.Controls.Add(this.bottomButton);
            this.Controls.Add(this.downButton);
            this.Controls.Add(this.upButton);
            this.Controls.Add(this.topButton);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.cloneButton);
            this.Controls.Add(this.newButton);
            this.Name = "TeamEditorForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.Move += new System.EventHandler(this.OnFormMove);
            this.Resize += new System.EventHandler(this.OnFormResize);
            ((System.ComponentModel.ISupportInitialize)(this.teamPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.endYearNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startYearNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.idNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.skillNumericUpDown)).EndInit();
            this.specialityGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button reloadButton;
        private System.Windows.Forms.PictureBox teamPictureBox;
        private System.Windows.Forms.Button pictureNameReferButton;
        private System.Windows.Forms.TextBox pictureNameTextBox;
        private System.Windows.Forms.Label pictureNameLabel;
        private System.Windows.Forms.NumericUpDown endYearNumericUpDown;
        private System.Windows.Forms.Label endYearLabel;
        private System.Windows.Forms.NumericUpDown startYearNumericUpDown;
        private System.Windows.Forms.Label startYearLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.NumericUpDown idNumericUpDown;
        private System.Windows.Forms.ListBox countryListBox;
        private System.Windows.Forms.ListView teamListView;
        private System.Windows.Forms.ColumnHeader countryColumnHeader;
        private System.Windows.Forms.ColumnHeader idColumnHeader;
        private System.Windows.Forms.ColumnHeader nameColumnHeader;
        private System.Windows.Forms.ColumnHeader startYearColumnHeader;
        private System.Windows.Forms.ColumnHeader endYearColumnHeader;
        private System.Windows.Forms.ColumnHeader specialityColumnHeader;
        private System.Windows.Forms.Label idLabel;
        private System.Windows.Forms.ComboBox countryComboBox;
        private System.Windows.Forms.Label countryLabel;
        private System.Windows.Forms.Button countryAllButton;
        private System.Windows.Forms.Button bottomButton;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Button topButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.Button cloneButton;
        private System.Windows.Forms.Button newButton;
        private System.Windows.Forms.NumericUpDown skillNumericUpDown;
        private System.Windows.Forms.Label skillLabel;
        private System.Windows.Forms.GroupBox specialityGroupBox;
        private System.Windows.Forms.ComboBox specialityComboBox6;
        private System.Windows.Forms.ComboBox specialityComboBox5;
        private System.Windows.Forms.ComboBox specialityComboBox4;
        private System.Windows.Forms.ComboBox specialityComboBox3;
        private System.Windows.Forms.ComboBox specialityComboBox2;
        private System.Windows.Forms.ComboBox specialityComboBox1;
        private System.Windows.Forms.ColumnHeader skillColumnHeader;
        private System.Windows.Forms.ComboBox specialityComboBox7;
        private System.Windows.Forms.Button sortAbcButton;
        private System.Windows.Forms.Button sortIdButton;
        private System.Windows.Forms.Button batchButton;
    }
}