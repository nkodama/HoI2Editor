namespace HoI2Editor.Forms
{
    partial class MinisterEditorForm
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
            this.ministerListView = new System.Windows.Forms.ListView();
            this.countryColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.idColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.startYearColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.endYearColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.positionColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.personalityColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ideologyColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.newButton = new System.Windows.Forms.Button();
            this.cloneButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.topButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.downButton = new System.Windows.Forms.Button();
            this.bottomButton = new System.Windows.Forms.Button();
            this.countryListBox = new System.Windows.Forms.ListBox();
            this.countryAllButton = new System.Windows.Forms.Button();
            this.countryLabel = new System.Windows.Forms.Label();
            this.countryComboBox = new System.Windows.Forms.ComboBox();
            this.idLabel = new System.Windows.Forms.Label();
            this.idNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.nameLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.startYearLabel = new System.Windows.Forms.Label();
            this.startYearNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.endYearLabel = new System.Windows.Forms.Label();
            this.endYearNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.loyaltyComboBox = new System.Windows.Forms.ComboBox();
            this.loyaltyLabel = new System.Windows.Forms.Label();
            this.ideologyComboBox = new System.Windows.Forms.ComboBox();
            this.ideologyLabel = new System.Windows.Forms.Label();
            this.personalityComboBox = new System.Windows.Forms.ComboBox();
            this.personalityLabel = new System.Windows.Forms.Label();
            this.positionComboBox = new System.Windows.Forms.ComboBox();
            this.positionLabel = new System.Windows.Forms.Label();
            this.pictureNameLabel = new System.Windows.Forms.Label();
            this.pictureNameTextBox = new System.Windows.Forms.TextBox();
            this.pictureNameReferButton = new System.Windows.Forms.Button();
            this.ministerPictureBox = new System.Windows.Forms.PictureBox();
            this.reloadButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.idNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startYearNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endYearNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ministerPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ministerListView
            // 
            this.ministerListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ministerListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.countryColumnHeader,
            this.idColumnHeader,
            this.nameColumnHeader,
            this.startYearColumnHeader,
            this.endYearColumnHeader,
            this.positionColumnHeader,
            this.personalityColumnHeader,
            this.ideologyColumnHeader});
            this.ministerListView.FullRowSelect = true;
            this.ministerListView.GridLines = true;
            this.ministerListView.HideSelection = false;
            this.ministerListView.Location = new System.Drawing.Point(12, 12);
            this.ministerListView.MultiSelect = false;
            this.ministerListView.Name = "ministerListView";
            this.ministerListView.Size = new System.Drawing.Size(760, 188);
            this.ministerListView.TabIndex = 0;
            this.ministerListView.UseCompatibleStateImageBehavior = false;
            this.ministerListView.View = System.Windows.Forms.View.Details;
            this.ministerListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.OnMinisterListViewItemSelectionChanged);
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
            // positionColumnHeader
            // 
            this.positionColumnHeader.Text = "地位";
            this.positionColumnHeader.Width = 90;
            // 
            // personalityColumnHeader
            // 
            this.personalityColumnHeader.Text = "特性";
            this.personalityColumnHeader.Width = 160;
            // 
            // ideologyColumnHeader
            // 
            this.ideologyColumnHeader.Text = "イデオロギー";
            this.ideologyColumnHeader.Width = 100;
            // 
            // newButton
            // 
            this.newButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.newButton.Location = new System.Drawing.Point(12, 206);
            this.newButton.Name = "newButton";
            this.newButton.Size = new System.Drawing.Size(75, 23);
            this.newButton.TabIndex = 1;
            this.newButton.Text = "新規";
            this.newButton.UseVisualStyleBackColor = true;
            // 
            // cloneButton
            // 
            this.cloneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cloneButton.Location = new System.Drawing.Point(93, 206);
            this.cloneButton.Name = "cloneButton";
            this.cloneButton.Size = new System.Drawing.Size(75, 23);
            this.cloneButton.TabIndex = 2;
            this.cloneButton.Text = "複製";
            this.cloneButton.UseVisualStyleBackColor = true;
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Location = new System.Drawing.Point(174, 206);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 3;
            this.deleteButton.Text = "削除";
            this.deleteButton.UseVisualStyleBackColor = true;
            // 
            // topButton
            // 
            this.topButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.topButton.Location = new System.Drawing.Point(454, 206);
            this.topButton.Name = "topButton";
            this.topButton.Size = new System.Drawing.Size(75, 23);
            this.topButton.TabIndex = 4;
            this.topButton.Text = "先頭へ";
            this.topButton.UseVisualStyleBackColor = true;
            // 
            // upButton
            // 
            this.upButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.upButton.Location = new System.Drawing.Point(535, 206);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(75, 23);
            this.upButton.TabIndex = 5;
            this.upButton.Text = "上へ";
            this.upButton.UseVisualStyleBackColor = true;
            // 
            // downButton
            // 
            this.downButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.downButton.Location = new System.Drawing.Point(616, 206);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(75, 23);
            this.downButton.TabIndex = 6;
            this.downButton.Text = "下へ";
            this.downButton.UseVisualStyleBackColor = true;
            // 
            // bottomButton
            // 
            this.bottomButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bottomButton.Location = new System.Drawing.Point(697, 206);
            this.bottomButton.Name = "bottomButton";
            this.bottomButton.Size = new System.Drawing.Size(75, 23);
            this.bottomButton.TabIndex = 7;
            this.bottomButton.Text = "末尾へ";
            this.bottomButton.UseVisualStyleBackColor = true;
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
            this.countryListBox.TabIndex = 8;
            this.countryListBox.SelectedIndexChanged += new System.EventHandler(this.OnCountryListBoxSelectedIndexChanged);
            // 
            // countryAllButton
            // 
            this.countryAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.countryAllButton.Location = new System.Drawing.Point(174, 526);
            this.countryAllButton.Name = "countryAllButton";
            this.countryAllButton.Size = new System.Drawing.Size(75, 23);
            this.countryAllButton.TabIndex = 9;
            this.countryAllButton.Text = "全選択";
            this.countryAllButton.UseVisualStyleBackColor = true;
            // 
            // countryLabel
            // 
            this.countryLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.countryLabel.AutoSize = true;
            this.countryLabel.Location = new System.Drawing.Point(278, 255);
            this.countryLabel.Name = "countryLabel";
            this.countryLabel.Size = new System.Drawing.Size(29, 12);
            this.countryLabel.TabIndex = 10;
            this.countryLabel.Text = "国家";
            // 
            // countryComboBox
            // 
            this.countryComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.countryComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.countryComboBox.FormattingEnabled = true;
            this.countryComboBox.Location = new System.Drawing.Point(359, 252);
            this.countryComboBox.Name = "countryComboBox";
            this.countryComboBox.Size = new System.Drawing.Size(160, 20);
            this.countryComboBox.TabIndex = 11;
            // 
            // idLabel
            // 
            this.idLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.idLabel.AutoSize = true;
            this.idLabel.Location = new System.Drawing.Point(557, 255);
            this.idLabel.Name = "idLabel";
            this.idLabel.Size = new System.Drawing.Size(16, 12);
            this.idLabel.TabIndex = 12;
            this.idLabel.Text = "ID";
            // 
            // idNumericUpDown
            // 
            this.idNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.idNumericUpDown.Location = new System.Drawing.Point(612, 253);
            this.idNumericUpDown.Maximum = new decimal(new int[] {
            16777215,
            0,
            0,
            0});
            this.idNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.idNumericUpDown.Name = "idNumericUpDown";
            this.idNumericUpDown.Size = new System.Drawing.Size(159, 19);
            this.idNumericUpDown.TabIndex = 13;
            this.idNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.idNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.idNumericUpDown.ValueChanged += new System.EventHandler(this.OnIdNumericUpDownValueChanged);
            // 
            // nameLabel
            // 
            this.nameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(278, 287);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(29, 12);
            this.nameLabel.TabIndex = 14;
            this.nameLabel.Text = "名前";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTextBox.Location = new System.Drawing.Point(359, 284);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(412, 19);
            this.nameTextBox.TabIndex = 15;
            this.nameTextBox.TextChanged += new System.EventHandler(this.OnNameTextBoxTextChanged);
            // 
            // startYearLabel
            // 
            this.startYearLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startYearLabel.AutoSize = true;
            this.startYearLabel.Location = new System.Drawing.Point(278, 318);
            this.startYearLabel.Name = "startYearLabel";
            this.startYearLabel.Size = new System.Drawing.Size(41, 12);
            this.startYearLabel.TabIndex = 16;
            this.startYearLabel.Text = "開始年";
            // 
            // startYearNumericUpDown
            // 
            this.startYearNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startYearNumericUpDown.Location = new System.Drawing.Point(359, 316);
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
            this.startYearNumericUpDown.Size = new System.Drawing.Size(160, 19);
            this.startYearNumericUpDown.TabIndex = 17;
            this.startYearNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.startYearNumericUpDown.Value = new decimal(new int[] {
            1900,
            0,
            0,
            0});
            this.startYearNumericUpDown.ValueChanged += new System.EventHandler(this.OnStartYearNumericUpDownValueChanged);
            // 
            // endYearLabel
            // 
            this.endYearLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.endYearLabel.AutoSize = true;
            this.endYearLabel.Location = new System.Drawing.Point(557, 318);
            this.endYearLabel.Name = "endYearLabel";
            this.endYearLabel.Size = new System.Drawing.Size(41, 12);
            this.endYearLabel.TabIndex = 18;
            this.endYearLabel.Text = "終了年";
            // 
            // endYearNumericUpDown
            // 
            this.endYearNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.endYearNumericUpDown.Enabled = false;
            this.endYearNumericUpDown.Location = new System.Drawing.Point(612, 316);
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
            this.endYearNumericUpDown.Size = new System.Drawing.Size(160, 19);
            this.endYearNumericUpDown.TabIndex = 19;
            this.endYearNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.endYearNumericUpDown.Value = new decimal(new int[] {
            1970,
            0,
            0,
            0});
            // 
            // loyaltyComboBox
            // 
            this.loyaltyComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.loyaltyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.loyaltyComboBox.FormattingEnabled = true;
            this.loyaltyComboBox.Location = new System.Drawing.Point(610, 380);
            this.loyaltyComboBox.Name = "loyaltyComboBox";
            this.loyaltyComboBox.Size = new System.Drawing.Size(161, 20);
            this.loyaltyComboBox.TabIndex = 31;
            this.loyaltyComboBox.SelectedIndexChanged += new System.EventHandler(this.OnLoyaltyComboBoxSelectedIndexChanged);
            // 
            // loyaltyLabel
            // 
            this.loyaltyLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.loyaltyLabel.AutoSize = true;
            this.loyaltyLabel.Location = new System.Drawing.Point(557, 383);
            this.loyaltyLabel.Name = "loyaltyLabel";
            this.loyaltyLabel.Size = new System.Drawing.Size(41, 12);
            this.loyaltyLabel.TabIndex = 30;
            this.loyaltyLabel.Text = "忠誠度";
            // 
            // ideologyComboBox
            // 
            this.ideologyComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ideologyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ideologyComboBox.FormattingEnabled = true;
            this.ideologyComboBox.Location = new System.Drawing.Point(359, 380);
            this.ideologyComboBox.Name = "ideologyComboBox";
            this.ideologyComboBox.Size = new System.Drawing.Size(160, 20);
            this.ideologyComboBox.TabIndex = 29;
            this.ideologyComboBox.SelectedIndexChanged += new System.EventHandler(this.IdeologyComboBoxSelectedIndexChanged);
            // 
            // ideologyLabel
            // 
            this.ideologyLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ideologyLabel.AutoSize = true;
            this.ideologyLabel.Location = new System.Drawing.Point(278, 383);
            this.ideologyLabel.Name = "ideologyLabel";
            this.ideologyLabel.Size = new System.Drawing.Size(62, 12);
            this.ideologyLabel.TabIndex = 28;
            this.ideologyLabel.Text = "イデオロギー";
            // 
            // personalityComboBox
            // 
            this.personalityComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.personalityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.personalityComboBox.FormattingEnabled = true;
            this.personalityComboBox.Location = new System.Drawing.Point(611, 348);
            this.personalityComboBox.Name = "personalityComboBox";
            this.personalityComboBox.Size = new System.Drawing.Size(160, 20);
            this.personalityComboBox.TabIndex = 27;
            this.personalityComboBox.SelectedIndexChanged += new System.EventHandler(this.OnPersonalityComboBoxSelectedIndexChanged);
            // 
            // personalityLabel
            // 
            this.personalityLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.personalityLabel.AutoSize = true;
            this.personalityLabel.Location = new System.Drawing.Point(557, 351);
            this.personalityLabel.Name = "personalityLabel";
            this.personalityLabel.Size = new System.Drawing.Size(29, 12);
            this.personalityLabel.TabIndex = 26;
            this.personalityLabel.Text = "特性";
            // 
            // positionComboBox
            // 
            this.positionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.positionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.positionComboBox.FormattingEnabled = true;
            this.positionComboBox.Location = new System.Drawing.Point(359, 348);
            this.positionComboBox.Name = "positionComboBox";
            this.positionComboBox.Size = new System.Drawing.Size(160, 20);
            this.positionComboBox.TabIndex = 25;
            this.positionComboBox.SelectedIndexChanged += new System.EventHandler(this.OnPositionComboBoxSelectedIndexChanged);
            // 
            // positionLabel
            // 
            this.positionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.positionLabel.AutoSize = true;
            this.positionLabel.Location = new System.Drawing.Point(278, 351);
            this.positionLabel.Name = "positionLabel";
            this.positionLabel.Size = new System.Drawing.Size(29, 12);
            this.positionLabel.TabIndex = 24;
            this.positionLabel.Text = "地位";
            // 
            // pictureNameLabel
            // 
            this.pictureNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureNameLabel.AutoSize = true;
            this.pictureNameLabel.Location = new System.Drawing.Point(278, 416);
            this.pictureNameLabel.Name = "pictureNameLabel";
            this.pictureNameLabel.Size = new System.Drawing.Size(75, 12);
            this.pictureNameLabel.TabIndex = 32;
            this.pictureNameLabel.Text = "画像ファイル名";
            // 
            // pictureNameTextBox
            // 
            this.pictureNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureNameTextBox.Location = new System.Drawing.Point(359, 413);
            this.pictureNameTextBox.Name = "pictureNameTextBox";
            this.pictureNameTextBox.Size = new System.Drawing.Size(332, 19);
            this.pictureNameTextBox.TabIndex = 33;
            this.pictureNameTextBox.TextChanged += new System.EventHandler(this.OnPictureNameTextBoxTextChanged);
            // 
            // pictureNameReferButton
            // 
            this.pictureNameReferButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureNameReferButton.Location = new System.Drawing.Point(697, 411);
            this.pictureNameReferButton.Name = "pictureNameReferButton";
            this.pictureNameReferButton.Size = new System.Drawing.Size(75, 23);
            this.pictureNameReferButton.TabIndex = 34;
            this.pictureNameReferButton.Text = "参照";
            this.pictureNameReferButton.UseVisualStyleBackColor = true;
            this.pictureNameReferButton.Click += new System.EventHandler(this.OnPictureNameReferButtonClick);
            // 
            // ministerPictureBox
            // 
            this.ministerPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ministerPictureBox.Location = new System.Drawing.Point(294, 448);
            this.ministerPictureBox.Name = "ministerPictureBox";
            this.ministerPictureBox.Size = new System.Drawing.Size(36, 50);
            this.ministerPictureBox.TabIndex = 35;
            this.ministerPictureBox.TabStop = false;
            // 
            // reloadButton
            // 
            this.reloadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.reloadButton.Location = new System.Drawing.Point(535, 526);
            this.reloadButton.Name = "reloadButton";
            this.reloadButton.Size = new System.Drawing.Size(75, 23);
            this.reloadButton.TabIndex = 37;
            this.reloadButton.Text = "再読み込み";
            this.reloadButton.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(616, 526);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 38;
            this.saveButton.Text = "保存";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(697, 526);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 39;
            this.closeButton.Text = "閉じる";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.OnCloseButtonClick);
            // 
            // MinisterEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.reloadButton);
            this.Controls.Add(this.ministerPictureBox);
            this.Controls.Add(this.pictureNameReferButton);
            this.Controls.Add(this.pictureNameTextBox);
            this.Controls.Add(this.pictureNameLabel);
            this.Controls.Add(this.loyaltyComboBox);
            this.Controls.Add(this.loyaltyLabel);
            this.Controls.Add(this.ideologyComboBox);
            this.Controls.Add(this.ideologyLabel);
            this.Controls.Add(this.personalityComboBox);
            this.Controls.Add(this.personalityLabel);
            this.Controls.Add(this.positionComboBox);
            this.Controls.Add(this.positionLabel);
            this.Controls.Add(this.endYearNumericUpDown);
            this.Controls.Add(this.endYearLabel);
            this.Controls.Add(this.startYearNumericUpDown);
            this.Controls.Add(this.startYearLabel);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.idNumericUpDown);
            this.Controls.Add(this.idLabel);
            this.Controls.Add(this.countryComboBox);
            this.Controls.Add(this.countryLabel);
            this.Controls.Add(this.countryAllButton);
            this.Controls.Add(this.countryListBox);
            this.Controls.Add(this.bottomButton);
            this.Controls.Add(this.downButton);
            this.Controls.Add(this.upButton);
            this.Controls.Add(this.topButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.cloneButton);
            this.Controls.Add(this.newButton);
            this.Controls.Add(this.ministerListView);
            this.Name = "MinisterEditorForm";
            this.Text = "Minister Editor";
            this.Load += new System.EventHandler(this.OnMinisterEditorFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.idNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startYearNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.endYearNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ministerPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView ministerListView;
        private System.Windows.Forms.ColumnHeader countryColumnHeader;
        private System.Windows.Forms.ColumnHeader idColumnHeader;
        private System.Windows.Forms.ColumnHeader nameColumnHeader;
        private System.Windows.Forms.ColumnHeader startYearColumnHeader;
        private System.Windows.Forms.ColumnHeader endYearColumnHeader;
        private System.Windows.Forms.ColumnHeader positionColumnHeader;
        private System.Windows.Forms.ColumnHeader personalityColumnHeader;
        private System.Windows.Forms.Button newButton;
        private System.Windows.Forms.Button cloneButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button topButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button bottomButton;
        private System.Windows.Forms.ListBox countryListBox;
        private System.Windows.Forms.Button countryAllButton;
        private System.Windows.Forms.Label countryLabel;
        private System.Windows.Forms.ComboBox countryComboBox;
        private System.Windows.Forms.Label idLabel;
        private System.Windows.Forms.NumericUpDown idNumericUpDown;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label startYearLabel;
        private System.Windows.Forms.NumericUpDown startYearNumericUpDown;
        private System.Windows.Forms.Label endYearLabel;
        private System.Windows.Forms.NumericUpDown endYearNumericUpDown;
        private System.Windows.Forms.ComboBox loyaltyComboBox;
        private System.Windows.Forms.Label loyaltyLabel;
        private System.Windows.Forms.ComboBox ideologyComboBox;
        private System.Windows.Forms.Label ideologyLabel;
        private System.Windows.Forms.ComboBox personalityComboBox;
        private System.Windows.Forms.Label personalityLabel;
        private System.Windows.Forms.ComboBox positionComboBox;
        private System.Windows.Forms.Label positionLabel;
        private System.Windows.Forms.Label pictureNameLabel;
        private System.Windows.Forms.TextBox pictureNameTextBox;
        private System.Windows.Forms.Button pictureNameReferButton;
        private System.Windows.Forms.PictureBox ministerPictureBox;
        private System.Windows.Forms.Button reloadButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.ColumnHeader ideologyColumnHeader;
    }
}