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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MinisterEditorForm));
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
            this.removeButton = new System.Windows.Forms.Button();
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
            this.retirementYearNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.retirementYearLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.idNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startYearNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endYearNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ministerPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.retirementYearNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // ministerListView
            // 
            resources.ApplyResources(this.ministerListView, "ministerListView");
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
            this.ministerListView.MultiSelect = false;
            this.ministerListView.Name = "ministerListView";
            this.ministerListView.UseCompatibleStateImageBehavior = false;
            this.ministerListView.View = System.Windows.Forms.View.Details;
            this.ministerListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.OnLeaderListViewColumnClick);
            this.ministerListView.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.OnMinisterListViewColumnWidthChanged);
            this.ministerListView.SelectedIndexChanged += new System.EventHandler(this.OnMinisterListViewSelectedIndexChanged);
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
            // startYearColumnHeader
            // 
            resources.ApplyResources(this.startYearColumnHeader, "startYearColumnHeader");
            // 
            // endYearColumnHeader
            // 
            resources.ApplyResources(this.endYearColumnHeader, "endYearColumnHeader");
            // 
            // positionColumnHeader
            // 
            resources.ApplyResources(this.positionColumnHeader, "positionColumnHeader");
            // 
            // personalityColumnHeader
            // 
            resources.ApplyResources(this.personalityColumnHeader, "personalityColumnHeader");
            // 
            // ideologyColumnHeader
            // 
            resources.ApplyResources(this.ideologyColumnHeader, "ideologyColumnHeader");
            // 
            // newButton
            // 
            resources.ApplyResources(this.newButton, "newButton");
            this.newButton.Name = "newButton";
            this.newButton.UseVisualStyleBackColor = true;
            this.newButton.Click += new System.EventHandler(this.OnNewButtonClick);
            // 
            // cloneButton
            // 
            resources.ApplyResources(this.cloneButton, "cloneButton");
            this.cloneButton.Name = "cloneButton";
            this.cloneButton.UseVisualStyleBackColor = true;
            this.cloneButton.Click += new System.EventHandler(this.OnCloneButtonClick);
            // 
            // removeButton
            // 
            resources.ApplyResources(this.removeButton, "removeButton");
            this.removeButton.Name = "removeButton";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.OnRemoveButtonClick);
            // 
            // topButton
            // 
            resources.ApplyResources(this.topButton, "topButton");
            this.topButton.Name = "topButton";
            this.topButton.UseVisualStyleBackColor = true;
            this.topButton.Click += new System.EventHandler(this.OnTopButtonClick);
            // 
            // upButton
            // 
            resources.ApplyResources(this.upButton, "upButton");
            this.upButton.Name = "upButton";
            this.upButton.UseVisualStyleBackColor = true;
            this.upButton.Click += new System.EventHandler(this.OnUpButtonClick);
            // 
            // downButton
            // 
            resources.ApplyResources(this.downButton, "downButton");
            this.downButton.Name = "downButton";
            this.downButton.UseVisualStyleBackColor = true;
            this.downButton.Click += new System.EventHandler(this.OnDownButtonClick);
            // 
            // bottomButton
            // 
            resources.ApplyResources(this.bottomButton, "bottomButton");
            this.bottomButton.Name = "bottomButton";
            this.bottomButton.UseVisualStyleBackColor = true;
            this.bottomButton.Click += new System.EventHandler(this.OnBottomButtonClick);
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
            // countryAllButton
            // 
            resources.ApplyResources(this.countryAllButton, "countryAllButton");
            this.countryAllButton.Name = "countryAllButton";
            this.countryAllButton.UseVisualStyleBackColor = true;
            this.countryAllButton.Click += new System.EventHandler(this.OnCountryAllButtonClick);
            // 
            // countryLabel
            // 
            resources.ApplyResources(this.countryLabel, "countryLabel");
            this.countryLabel.Name = "countryLabel";
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
            // idLabel
            // 
            resources.ApplyResources(this.idLabel, "idLabel");
            this.idLabel.Name = "idLabel";
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
            // nameLabel
            // 
            resources.ApplyResources(this.nameLabel, "nameLabel");
            this.nameLabel.Name = "nameLabel";
            // 
            // nameTextBox
            // 
            resources.ApplyResources(this.nameTextBox, "nameTextBox");
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.TextChanged += new System.EventHandler(this.OnNameTextBoxTextChanged);
            // 
            // startYearLabel
            // 
            resources.ApplyResources(this.startYearLabel, "startYearLabel");
            this.startYearLabel.Name = "startYearLabel";
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
            1900,
            0,
            0,
            0});
            this.startYearNumericUpDown.ValueChanged += new System.EventHandler(this.OnStartYearNumericUpDownValueChanged);
            // 
            // endYearLabel
            // 
            resources.ApplyResources(this.endYearLabel, "endYearLabel");
            this.endYearLabel.Name = "endYearLabel";
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
            // loyaltyComboBox
            // 
            resources.ApplyResources(this.loyaltyComboBox, "loyaltyComboBox");
            this.loyaltyComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.loyaltyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.loyaltyComboBox.FormattingEnabled = true;
            this.loyaltyComboBox.Name = "loyaltyComboBox";
            this.loyaltyComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnLoyaltyComboBoxDrawItem);
            this.loyaltyComboBox.SelectedIndexChanged += new System.EventHandler(this.OnLoyaltyComboBoxSelectedIndexChanged);
            // 
            // loyaltyLabel
            // 
            resources.ApplyResources(this.loyaltyLabel, "loyaltyLabel");
            this.loyaltyLabel.Name = "loyaltyLabel";
            // 
            // ideologyComboBox
            // 
            resources.ApplyResources(this.ideologyComboBox, "ideologyComboBox");
            this.ideologyComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ideologyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ideologyComboBox.FormattingEnabled = true;
            this.ideologyComboBox.Name = "ideologyComboBox";
            this.ideologyComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnIdeologyComboBoxDrawItem);
            this.ideologyComboBox.SelectedIndexChanged += new System.EventHandler(this.OnIdeologyComboBoxSelectedIndexChanged);
            // 
            // ideologyLabel
            // 
            resources.ApplyResources(this.ideologyLabel, "ideologyLabel");
            this.ideologyLabel.Name = "ideologyLabel";
            // 
            // personalityComboBox
            // 
            resources.ApplyResources(this.personalityComboBox, "personalityComboBox");
            this.personalityComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.personalityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.personalityComboBox.FormattingEnabled = true;
            this.personalityComboBox.Name = "personalityComboBox";
            this.personalityComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnPersonalityComboBoxDrawItem);
            this.personalityComboBox.SelectedIndexChanged += new System.EventHandler(this.OnPersonalityComboBoxSelectedIndexChanged);
            // 
            // personalityLabel
            // 
            resources.ApplyResources(this.personalityLabel, "personalityLabel");
            this.personalityLabel.Name = "personalityLabel";
            // 
            // positionComboBox
            // 
            resources.ApplyResources(this.positionComboBox, "positionComboBox");
            this.positionComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.positionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.positionComboBox.FormattingEnabled = true;
            this.positionComboBox.Name = "positionComboBox";
            this.positionComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnPositionComboBoxDrawItem);
            this.positionComboBox.SelectedIndexChanged += new System.EventHandler(this.OnPositionComboBoxSelectedIndexChanged);
            // 
            // positionLabel
            // 
            resources.ApplyResources(this.positionLabel, "positionLabel");
            this.positionLabel.Name = "positionLabel";
            // 
            // pictureNameLabel
            // 
            resources.ApplyResources(this.pictureNameLabel, "pictureNameLabel");
            this.pictureNameLabel.Name = "pictureNameLabel";
            // 
            // pictureNameTextBox
            // 
            resources.ApplyResources(this.pictureNameTextBox, "pictureNameTextBox");
            this.pictureNameTextBox.Name = "pictureNameTextBox";
            this.pictureNameTextBox.TextChanged += new System.EventHandler(this.OnPictureNameTextBoxTextChanged);
            // 
            // pictureNameReferButton
            // 
            resources.ApplyResources(this.pictureNameReferButton, "pictureNameReferButton");
            this.pictureNameReferButton.Name = "pictureNameReferButton";
            this.pictureNameReferButton.UseVisualStyleBackColor = true;
            this.pictureNameReferButton.Click += new System.EventHandler(this.OnPictureNameReferButtonClick);
            // 
            // ministerPictureBox
            // 
            resources.ApplyResources(this.ministerPictureBox, "ministerPictureBox");
            this.ministerPictureBox.Name = "ministerPictureBox";
            this.ministerPictureBox.TabStop = false;
            // 
            // reloadButton
            // 
            resources.ApplyResources(this.reloadButton, "reloadButton");
            this.reloadButton.Name = "reloadButton";
            this.reloadButton.UseVisualStyleBackColor = true;
            this.reloadButton.Click += new System.EventHandler(this.OnReloadButtonClick);
            // 
            // saveButton
            // 
            resources.ApplyResources(this.saveButton, "saveButton");
            this.saveButton.Name = "saveButton";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.OnSaveButtonClick);
            // 
            // closeButton
            // 
            resources.ApplyResources(this.closeButton, "closeButton");
            this.closeButton.Name = "closeButton";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.OnCloseButtonClick);
            // 
            // retirementYearNumericUpDown
            // 
            resources.ApplyResources(this.retirementYearNumericUpDown, "retirementYearNumericUpDown");
            this.retirementYearNumericUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.retirementYearNumericUpDown.Name = "retirementYearNumericUpDown";
            this.retirementYearNumericUpDown.Value = new decimal(new int[] {
            1999,
            0,
            0,
            0});
            this.retirementYearNumericUpDown.ValueChanged += new System.EventHandler(this.OnRetirementYearNumericUpDownValueChanged);
            // 
            // retirementYearLabel
            // 
            resources.ApplyResources(this.retirementYearLabel, "retirementYearLabel");
            this.retirementYearLabel.Name = "retirementYearLabel";
            // 
            // MinisterEditorForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.retirementYearNumericUpDown);
            this.Controls.Add(this.retirementYearLabel);
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
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.cloneButton);
            this.Controls.Add(this.newButton);
            this.Controls.Add(this.ministerListView);
            this.Name = "MinisterEditorForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnMinisterEditorFormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnMinisterEditorFormClosed);
            this.Load += new System.EventHandler(this.OnMinisterEditorFormLoad);
            this.Move += new System.EventHandler(this.OnMinisterEditorFormMove);
            this.Resize += new System.EventHandler(this.OnMinisterEditorFormResize);
            ((System.ComponentModel.ISupportInitialize)(this.idNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startYearNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.endYearNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ministerPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.retirementYearNumericUpDown)).EndInit();
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
        private System.Windows.Forms.Button removeButton;
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
        private System.Windows.Forms.NumericUpDown retirementYearNumericUpDown;
        private System.Windows.Forms.Label retirementYearLabel;
    }
}