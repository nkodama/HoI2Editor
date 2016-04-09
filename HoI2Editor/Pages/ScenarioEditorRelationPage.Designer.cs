namespace HoI2Editor.Pages
{
    partial class ScenarioEditorRelationPage
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label relationValueLabel;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScenarioEditorRelationPage));
            System.Windows.Forms.Label spyNumLabel;
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
            this.countryListBox = new System.Windows.Forms.ListBox();
            relationValueLabel = new System.Windows.Forms.Label();
            spyNumLabel = new System.Windows.Forms.Label();
            this.peaceGroupBox.SuspendLayout();
            this.guaranteedGroupBox.SuspendLayout();
            this.nonAggressionGroupBox.SuspendLayout();
            this.relationGroupBox.SuspendLayout();
            this.intelligenceGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spyNumNumericUpDown)).BeginInit();
            this.SuspendLayout();
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
            // countryListBox
            // 
            resources.ApplyResources(this.countryListBox, "countryListBox");
            this.countryListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.countryListBox.FormattingEnabled = true;
            this.countryListBox.Name = "countryListBox";
            this.countryListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnCountryListBoxDrawItem);
            this.countryListBox.SelectedIndexChanged += new System.EventHandler(this.OnCountryListBoxSelectedIndexChanged);
            // 
            // ScenarioEditorRelationPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.peaceGroupBox);
            this.Controls.Add(this.guaranteedGroupBox);
            this.Controls.Add(this.nonAggressionGroupBox);
            this.Controls.Add(this.relationGroupBox);
            this.Controls.Add(this.intelligenceGroupBox);
            this.Controls.Add(this.relationListView);
            this.Controls.Add(this.countryListBox);
            this.Name = "ScenarioEditorRelationPage";
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
            this.ResumeLayout(false);

        }

        #endregion

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
        private System.Windows.Forms.GroupBox guaranteedGroupBox;
        private System.Windows.Forms.CheckBox guaranteedCheckBox;
        private System.Windows.Forms.TextBox guaranteedYearTextBox;
        private System.Windows.Forms.TextBox guaranteedMonthTextBox;
        private System.Windows.Forms.Label guaranteedEndLabel;
        private System.Windows.Forms.TextBox guaranteedDayTextBox;
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
        private System.Windows.Forms.GroupBox relationGroupBox;
        private System.Windows.Forms.TextBox relationValueTextBox;
        private System.Windows.Forms.CheckBox controlCheckBox;
        private System.Windows.Forms.CheckBox masterCheckBox;
        private System.Windows.Forms.CheckBox accessCheckBox;
        private System.Windows.Forms.GroupBox intelligenceGroupBox;
        private System.Windows.Forms.NumericUpDown spyNumNumericUpDown;
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
        private System.Windows.Forms.ListBox countryListBox;
    }
}
