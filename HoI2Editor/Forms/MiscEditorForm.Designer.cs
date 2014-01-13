﻿namespace HoI2Editor.Forms
{
    partial class MiscEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MiscEditorForm));
            this.miscTabControl = new System.Windows.Forms.TabControl();
            this.miscToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.closeButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.reloadButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // miscTabControl
            // 
            resources.ApplyResources(this.miscTabControl, "miscTabControl");
            this.miscTabControl.Name = "miscTabControl";
            this.miscTabControl.SelectedIndex = 0;
            this.miscToolTip.SetToolTip(this.miscTabControl, resources.GetString("miscTabControl.ToolTip"));
            this.miscTabControl.SelectedIndexChanged += new System.EventHandler(this.OnMiscTabControlSelectedIndexChanged);
            // 
            // closeButton
            // 
            resources.ApplyResources(this.closeButton, "closeButton");
            this.closeButton.Name = "closeButton";
            this.miscToolTip.SetToolTip(this.closeButton, resources.GetString("closeButton.ToolTip"));
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.OnCloseButtonClick);
            // 
            // saveButton
            // 
            resources.ApplyResources(this.saveButton, "saveButton");
            this.saveButton.Name = "saveButton";
            this.miscToolTip.SetToolTip(this.saveButton, resources.GetString("saveButton.ToolTip"));
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.OnSaveButtonClick);
            // 
            // reloadButton
            // 
            resources.ApplyResources(this.reloadButton, "reloadButton");
            this.reloadButton.Name = "reloadButton";
            this.miscToolTip.SetToolTip(this.reloadButton, resources.GetString("reloadButton.ToolTip"));
            this.reloadButton.UseVisualStyleBackColor = true;
            this.reloadButton.Click += new System.EventHandler(this.OnReloadButtonClick);
            // 
            // MiscEditorForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.reloadButton);
            this.Controls.Add(this.miscTabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MiscEditorForm";
            this.miscToolTip.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnMiscEditorFormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnMiscEditorFormClosed);
            this.Load += new System.EventHandler(this.OnMiscEditorFormLoad);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl miscTabControl;
        private System.Windows.Forms.ToolTip miscToolTip;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button reloadButton;
    }
}