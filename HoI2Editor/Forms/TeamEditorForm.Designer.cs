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
            this.specialityColumnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.specialityColumnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.specialityColumnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.specialityColumnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.specialityColumnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.specialityColumnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.idLabel = new System.Windows.Forms.Label();
            this.countryComboBox = new System.Windows.Forms.ComboBox();
            this.countryLabel = new System.Windows.Forms.Label();
            this.countryAllButton = new System.Windows.Forms.Button();
            this.bottomButton = new System.Windows.Forms.Button();
            this.downButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.topButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.cloneButton = new System.Windows.Forms.Button();
            this.newButton = new System.Windows.Forms.Button();
            this.skillNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.skillLabel = new System.Windows.Forms.Label();
            this.specialityGroupBox = new System.Windows.Forms.GroupBox();
            this.specialityComboBox6 = new System.Windows.Forms.ComboBox();
            this.specialityComboBox5 = new System.Windows.Forms.ComboBox();
            this.specialityComboBox4 = new System.Windows.Forms.ComboBox();
            this.specialityComboBox3 = new System.Windows.Forms.ComboBox();
            this.specialityComboBox2 = new System.Windows.Forms.ComboBox();
            this.specialityComboBox1 = new System.Windows.Forms.ComboBox();
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
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(697, 526);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 74;
            this.closeButton.Text = "閉じる";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.OnCloseButtonClick);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(616, 526);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 73;
            this.saveButton.Text = "保存";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.OnSaveButtonClick);
            // 
            // reloadButton
            // 
            this.reloadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.reloadButton.Location = new System.Drawing.Point(535, 526);
            this.reloadButton.Name = "reloadButton";
            this.reloadButton.Size = new System.Drawing.Size(75, 23);
            this.reloadButton.TabIndex = 72;
            this.reloadButton.Text = "再読み込み";
            this.reloadButton.UseVisualStyleBackColor = true;
            this.reloadButton.Click += new System.EventHandler(this.OnReloadButtonClick);
            // 
            // teamPictureBox
            // 
            this.teamPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.teamPictureBox.Location = new System.Drawing.Point(360, 430);
            this.teamPictureBox.Name = "teamPictureBox";
            this.teamPictureBox.Size = new System.Drawing.Size(96, 96);
            this.teamPictureBox.TabIndex = 71;
            this.teamPictureBox.TabStop = false;
            // 
            // pictureNameReferButton
            // 
            this.pictureNameReferButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureNameReferButton.Location = new System.Drawing.Point(466, 428);
            this.pictureNameReferButton.Name = "pictureNameReferButton";
            this.pictureNameReferButton.Size = new System.Drawing.Size(75, 23);
            this.pictureNameReferButton.TabIndex = 70;
            this.pictureNameReferButton.Text = "参照";
            this.pictureNameReferButton.UseVisualStyleBackColor = true;
            this.pictureNameReferButton.Click += new System.EventHandler(this.OnPictureNameReferButtonClick);
            // 
            // pictureNameTextBox
            // 
            this.pictureNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureNameTextBox.Location = new System.Drawing.Point(360, 403);
            this.pictureNameTextBox.Name = "pictureNameTextBox";
            this.pictureNameTextBox.Size = new System.Drawing.Size(181, 19);
            this.pictureNameTextBox.TabIndex = 69;
            this.pictureNameTextBox.TextChanged += new System.EventHandler(this.OnPictureNameTextBoxTextChanged);
            // 
            // pictureNameLabel
            // 
            this.pictureNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureNameLabel.AutoSize = true;
            this.pictureNameLabel.Location = new System.Drawing.Point(278, 406);
            this.pictureNameLabel.Name = "pictureNameLabel";
            this.pictureNameLabel.Size = new System.Drawing.Size(75, 12);
            this.pictureNameLabel.TabIndex = 68;
            this.pictureNameLabel.Text = "画像ファイル名";
            // 
            // endYearNumericUpDown
            // 
            this.endYearNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.endYearNumericUpDown.Enabled = false;
            this.endYearNumericUpDown.Location = new System.Drawing.Point(360, 378);
            this.endYearNumericUpDown.Maximum = new decimal(new int[] {
            1999,
            0,
            0,
            0});
            this.endYearNumericUpDown.Minimum = new decimal(new int[] {
            1900,
            0,
            0,
            0});
            this.endYearNumericUpDown.Name = "endYearNumericUpDown";
            this.endYearNumericUpDown.Size = new System.Drawing.Size(181, 19);
            this.endYearNumericUpDown.TabIndex = 59;
            this.endYearNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.endYearNumericUpDown.Value = new decimal(new int[] {
            1970,
            0,
            0,
            0});
            this.endYearNumericUpDown.ValueChanged += new System.EventHandler(this.OnEndYearNumericUpDownValueChanged);
            // 
            // endYearLabel
            // 
            this.endYearLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.endYearLabel.AutoSize = true;
            this.endYearLabel.Location = new System.Drawing.Point(279, 380);
            this.endYearLabel.Name = "endYearLabel";
            this.endYearLabel.Size = new System.Drawing.Size(41, 12);
            this.endYearLabel.TabIndex = 58;
            this.endYearLabel.Text = "終了年";
            // 
            // startYearNumericUpDown
            // 
            this.startYearNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startYearNumericUpDown.Location = new System.Drawing.Point(360, 353);
            this.startYearNumericUpDown.Maximum = new decimal(new int[] {
            1999,
            0,
            0,
            0});
            this.startYearNumericUpDown.Minimum = new decimal(new int[] {
            1900,
            0,
            0,
            0});
            this.startYearNumericUpDown.Name = "startYearNumericUpDown";
            this.startYearNumericUpDown.Size = new System.Drawing.Size(181, 19);
            this.startYearNumericUpDown.TabIndex = 57;
            this.startYearNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.startYearNumericUpDown.Value = new decimal(new int[] {
            1900,
            0,
            0,
            0});
            this.startYearNumericUpDown.ValueChanged += new System.EventHandler(this.OnStartYearNumericUpDownValueChanged);
            // 
            // startYearLabel
            // 
            this.startYearLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startYearLabel.AutoSize = true;
            this.startYearLabel.Location = new System.Drawing.Point(279, 355);
            this.startYearLabel.Name = "startYearLabel";
            this.startYearLabel.Size = new System.Drawing.Size(41, 12);
            this.startYearLabel.TabIndex = 56;
            this.startYearLabel.Text = "開始年";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTextBox.Location = new System.Drawing.Point(360, 303);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(181, 19);
            this.nameTextBox.TabIndex = 55;
            this.nameTextBox.TextChanged += new System.EventHandler(this.OnNameTextBoxTextChanged);
            // 
            // nameLabel
            // 
            this.nameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(278, 306);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(29, 12);
            this.nameLabel.TabIndex = 54;
            this.nameLabel.Text = "名前";
            // 
            // idNumericUpDown
            // 
            this.idNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.idNumericUpDown.Location = new System.Drawing.Point(360, 278);
            this.idNumericUpDown.Maximum = new decimal(new int[] {
            16777215,
            0,
            0,
            0});
            this.idNumericUpDown.Name = "idNumericUpDown";
            this.idNumericUpDown.Size = new System.Drawing.Size(181, 19);
            this.idNumericUpDown.TabIndex = 53;
            this.idNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.idNumericUpDown.ValueChanged += new System.EventHandler(this.OnIdNumericUpDownValueChanged);
            // 
            // countryListBox
            // 
            this.countryListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.countryListBox.ColumnWidth = 40;
            this.countryListBox.FormattingEnabled = true;
            this.countryListBox.ItemHeight = 12;
            this.countryListBox.Location = new System.Drawing.Point(12, 252);
            this.countryListBox.MultiColumn = true;
            this.countryListBox.Name = "countryListBox";
            this.countryListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.countryListBox.Size = new System.Drawing.Size(237, 268);
            this.countryListBox.TabIndex = 48;
            this.countryListBox.SelectedIndexChanged += new System.EventHandler(this.OnCountryListBoxSelectedIndexChanged);
            // 
            // teamListView
            // 
            this.teamListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.teamListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.countryColumnHeader,
            this.idColumnHeader,
            this.nameColumnHeader,
            this.skillColumnHeader,
            this.startYearColumnHeader,
            this.endYearColumnHeader,
            this.specialityColumnHeader1,
            this.specialityColumnHeader2,
            this.specialityColumnHeader3,
            this.specialityColumnHeader4,
            this.specialityColumnHeader5,
            this.specialityColumnHeader6});
            this.teamListView.FullRowSelect = true;
            this.teamListView.GridLines = true;
            this.teamListView.HideSelection = false;
            this.teamListView.Location = new System.Drawing.Point(12, 12);
            this.teamListView.MultiSelect = false;
            this.teamListView.Name = "teamListView";
            this.teamListView.Size = new System.Drawing.Size(760, 188);
            this.teamListView.TabIndex = 40;
            this.teamListView.UseCompatibleStateImageBehavior = false;
            this.teamListView.View = System.Windows.Forms.View.Details;
            this.teamListView.SelectedIndexChanged += new System.EventHandler(this.OnTeamListViewSelectedIndexChanged);
            // 
            // countryColumnHeader
            // 
            this.countryColumnHeader.Text = "国家";
            this.countryColumnHeader.Width = 40;
            // 
            // idColumnHeader
            // 
            this.idColumnHeader.Text = "ID";
            this.idColumnHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nameColumnHeader
            // 
            this.nameColumnHeader.Text = "名前";
            this.nameColumnHeader.Width = 180;
            // 
            // skillColumnHeader
            // 
            this.skillColumnHeader.Text = "スキル";
            this.skillColumnHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.skillColumnHeader.Width = 50;
            // 
            // startYearColumnHeader
            // 
            this.startYearColumnHeader.Text = "開始年";
            this.startYearColumnHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.startYearColumnHeader.Width = 50;
            // 
            // endYearColumnHeader
            // 
            this.endYearColumnHeader.Text = "終了年";
            this.endYearColumnHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.endYearColumnHeader.Width = 50;
            // 
            // specialityColumnHeader1
            // 
            this.specialityColumnHeader1.Text = "特性1";
            this.specialityColumnHeader1.Width = 50;
            // 
            // specialityColumnHeader2
            // 
            this.specialityColumnHeader2.Text = "特性2";
            this.specialityColumnHeader2.Width = 50;
            // 
            // specialityColumnHeader3
            // 
            this.specialityColumnHeader3.Text = "特性3";
            this.specialityColumnHeader3.Width = 50;
            // 
            // specialityColumnHeader4
            // 
            this.specialityColumnHeader4.Text = "特性4";
            this.specialityColumnHeader4.Width = 50;
            // 
            // specialityColumnHeader5
            // 
            this.specialityColumnHeader5.Text = "特性5";
            this.specialityColumnHeader5.Width = 50;
            // 
            // specialityColumnHeader6
            // 
            this.specialityColumnHeader6.Text = " 特性6";
            this.specialityColumnHeader6.Width = 50;
            // 
            // idLabel
            // 
            this.idLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.idLabel.AutoSize = true;
            this.idLabel.Location = new System.Drawing.Point(278, 280);
            this.idLabel.Name = "idLabel";
            this.idLabel.Size = new System.Drawing.Size(16, 12);
            this.idLabel.TabIndex = 52;
            this.idLabel.Text = "ID";
            // 
            // countryComboBox
            // 
            this.countryComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.countryComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.countryComboBox.FormattingEnabled = true;
            this.countryComboBox.Location = new System.Drawing.Point(360, 252);
            this.countryComboBox.Name = "countryComboBox";
            this.countryComboBox.Size = new System.Drawing.Size(181, 20);
            this.countryComboBox.TabIndex = 51;
            this.countryComboBox.SelectionChangeCommitted += new System.EventHandler(this.OnCountryComboBoxSelectionChangeCommitted);
            // 
            // countryLabel
            // 
            this.countryLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.countryLabel.AutoSize = true;
            this.countryLabel.Location = new System.Drawing.Point(278, 255);
            this.countryLabel.Name = "countryLabel";
            this.countryLabel.Size = new System.Drawing.Size(29, 12);
            this.countryLabel.TabIndex = 50;
            this.countryLabel.Text = "国家";
            // 
            // countryAllButton
            // 
            this.countryAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.countryAllButton.Location = new System.Drawing.Point(174, 526);
            this.countryAllButton.Name = "countryAllButton";
            this.countryAllButton.Size = new System.Drawing.Size(75, 23);
            this.countryAllButton.TabIndex = 49;
            this.countryAllButton.Text = "全選択";
            this.countryAllButton.UseVisualStyleBackColor = true;
            this.countryAllButton.Click += new System.EventHandler(this.OnCountryAllButtonClick);
            // 
            // bottomButton
            // 
            this.bottomButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bottomButton.Location = new System.Drawing.Point(697, 206);
            this.bottomButton.Name = "bottomButton";
            this.bottomButton.Size = new System.Drawing.Size(75, 23);
            this.bottomButton.TabIndex = 47;
            this.bottomButton.Text = "末尾へ";
            this.bottomButton.UseVisualStyleBackColor = true;
            this.bottomButton.Click += new System.EventHandler(this.OnBottomButtonClick);
            // 
            // downButton
            // 
            this.downButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.downButton.Location = new System.Drawing.Point(616, 206);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(75, 23);
            this.downButton.TabIndex = 46;
            this.downButton.Text = "下へ";
            this.downButton.UseVisualStyleBackColor = true;
            this.downButton.Click += new System.EventHandler(this.OnDownButtonClick);
            // 
            // upButton
            // 
            this.upButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.upButton.Location = new System.Drawing.Point(535, 206);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(75, 23);
            this.upButton.TabIndex = 45;
            this.upButton.Text = "上へ";
            this.upButton.UseVisualStyleBackColor = true;
            this.upButton.Click += new System.EventHandler(this.OnUpButtonClick);
            // 
            // topButton
            // 
            this.topButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.topButton.Location = new System.Drawing.Point(454, 206);
            this.topButton.Name = "topButton";
            this.topButton.Size = new System.Drawing.Size(75, 23);
            this.topButton.TabIndex = 44;
            this.topButton.Text = "先頭へ";
            this.topButton.UseVisualStyleBackColor = true;
            this.topButton.Click += new System.EventHandler(this.OnTopButtonClick);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Location = new System.Drawing.Point(174, 206);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 43;
            this.deleteButton.Text = "削除";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.OnDeleteButtonClick);
            // 
            // cloneButton
            // 
            this.cloneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cloneButton.Location = new System.Drawing.Point(93, 206);
            this.cloneButton.Name = "cloneButton";
            this.cloneButton.Size = new System.Drawing.Size(75, 23);
            this.cloneButton.TabIndex = 42;
            this.cloneButton.Text = "複製";
            this.cloneButton.UseVisualStyleBackColor = true;
            this.cloneButton.Click += new System.EventHandler(this.OnCloneButtonClick);
            // 
            // newButton
            // 
            this.newButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.newButton.Location = new System.Drawing.Point(12, 206);
            this.newButton.Name = "newButton";
            this.newButton.Size = new System.Drawing.Size(75, 23);
            this.newButton.TabIndex = 41;
            this.newButton.Text = "新規";
            this.newButton.UseVisualStyleBackColor = true;
            this.newButton.Click += new System.EventHandler(this.OnNewButtonClick);
            // 
            // skillNumericUpDown
            // 
            this.skillNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.skillNumericUpDown.Location = new System.Drawing.Point(360, 328);
            this.skillNumericUpDown.Name = "skillNumericUpDown";
            this.skillNumericUpDown.Size = new System.Drawing.Size(181, 19);
            this.skillNumericUpDown.TabIndex = 76;
            this.skillNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.skillNumericUpDown.ValueChanged += new System.EventHandler(this.OnSkillNumericUpDownValueChanged);
            // 
            // skillLabel
            // 
            this.skillLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.skillLabel.AutoSize = true;
            this.skillLabel.Location = new System.Drawing.Point(278, 330);
            this.skillLabel.Name = "skillLabel";
            this.skillLabel.Size = new System.Drawing.Size(34, 12);
            this.skillLabel.TabIndex = 75;
            this.skillLabel.Text = "スキル";
            // 
            // specialityGroupBox
            // 
            this.specialityGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.specialityGroupBox.Controls.Add(this.specialityComboBox6);
            this.specialityGroupBox.Controls.Add(this.specialityComboBox5);
            this.specialityGroupBox.Controls.Add(this.specialityComboBox4);
            this.specialityGroupBox.Controls.Add(this.specialityComboBox3);
            this.specialityGroupBox.Controls.Add(this.specialityComboBox2);
            this.specialityGroupBox.Controls.Add(this.specialityComboBox1);
            this.specialityGroupBox.Location = new System.Drawing.Point(568, 252);
            this.specialityGroupBox.Name = "specialityGroupBox";
            this.specialityGroupBox.Size = new System.Drawing.Size(204, 185);
            this.specialityGroupBox.TabIndex = 77;
            this.specialityGroupBox.TabStop = false;
            this.specialityGroupBox.Text = "研究特性";
            // 
            // specialityComboBox6
            // 
            this.specialityComboBox6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.specialityComboBox6.FormattingEnabled = true;
            this.specialityComboBox6.Location = new System.Drawing.Point(21, 149);
            this.specialityComboBox6.Name = "specialityComboBox6";
            this.specialityComboBox6.Size = new System.Drawing.Size(160, 20);
            this.specialityComboBox6.TabIndex = 83;
            this.specialityComboBox6.SelectionChangeCommitted += new System.EventHandler(this.OnSpecialityComboBox6SelectionChangeCommitted);
            // 
            // specialityComboBox5
            // 
            this.specialityComboBox5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.specialityComboBox5.FormattingEnabled = true;
            this.specialityComboBox5.Location = new System.Drawing.Point(21, 123);
            this.specialityComboBox5.Name = "specialityComboBox5";
            this.specialityComboBox5.Size = new System.Drawing.Size(160, 20);
            this.specialityComboBox5.TabIndex = 82;
            this.specialityComboBox5.SelectionChangeCommitted += new System.EventHandler(this.OnSpecialityComboBox5SelectionChangeCommitted);
            // 
            // specialityComboBox4
            // 
            this.specialityComboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.specialityComboBox4.FormattingEnabled = true;
            this.specialityComboBox4.Location = new System.Drawing.Point(21, 97);
            this.specialityComboBox4.Name = "specialityComboBox4";
            this.specialityComboBox4.Size = new System.Drawing.Size(160, 20);
            this.specialityComboBox4.TabIndex = 81;
            this.specialityComboBox4.SelectionChangeCommitted += new System.EventHandler(this.OnSpecialityComboBox4SelectionChangeCommitted);
            // 
            // specialityComboBox3
            // 
            this.specialityComboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.specialityComboBox3.FormattingEnabled = true;
            this.specialityComboBox3.Location = new System.Drawing.Point(21, 70);
            this.specialityComboBox3.Name = "specialityComboBox3";
            this.specialityComboBox3.Size = new System.Drawing.Size(160, 20);
            this.specialityComboBox3.TabIndex = 80;
            this.specialityComboBox3.SelectionChangeCommitted += new System.EventHandler(this.OnSpecialityComboBox3SelectionChangeCommitted);
            // 
            // specialityComboBox2
            // 
            this.specialityComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.specialityComboBox2.FormattingEnabled = true;
            this.specialityComboBox2.Location = new System.Drawing.Point(21, 44);
            this.specialityComboBox2.Name = "specialityComboBox2";
            this.specialityComboBox2.Size = new System.Drawing.Size(160, 20);
            this.specialityComboBox2.TabIndex = 79;
            this.specialityComboBox2.SelectionChangeCommitted += new System.EventHandler(this.OnSpecialityComboBox2SelectionChangeCommitted);
            // 
            // specialityComboBox1
            // 
            this.specialityComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.specialityComboBox1.FormattingEnabled = true;
            this.specialityComboBox1.Location = new System.Drawing.Point(21, 18);
            this.specialityComboBox1.Name = "specialityComboBox1";
            this.specialityComboBox1.Size = new System.Drawing.Size(160, 20);
            this.specialityComboBox1.TabIndex = 78;
            this.specialityComboBox1.SelectionChangeCommitted += new System.EventHandler(this.OnSpecialityComboBox1SelectionChangeCommitted);
            // 
            // TeamEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
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
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.cloneButton);
            this.Controls.Add(this.newButton);
            this.Name = "TeamEditorForm";
            this.Text = "Team Editor";
            this.Load += new System.EventHandler(this.OnTeamEditorFormLoad);
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
        private System.Windows.Forms.ColumnHeader specialityColumnHeader1;
        private System.Windows.Forms.Label idLabel;
        private System.Windows.Forms.ComboBox countryComboBox;
        private System.Windows.Forms.Label countryLabel;
        private System.Windows.Forms.Button countryAllButton;
        private System.Windows.Forms.Button bottomButton;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Button topButton;
        private System.Windows.Forms.Button deleteButton;
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
        private System.Windows.Forms.ColumnHeader specialityColumnHeader2;
        private System.Windows.Forms.ColumnHeader specialityColumnHeader3;
        private System.Windows.Forms.ColumnHeader specialityColumnHeader4;
        private System.Windows.Forms.ColumnHeader specialityColumnHeader5;
        private System.Windows.Forms.ColumnHeader specialityColumnHeader6;
    }
}