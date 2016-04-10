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
            this.closeButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.reloadButton = new System.Windows.Forms.Button();
            this.provinceTabPage = new System.Windows.Forms.TabPage();
            this.governmentTabPage = new System.Windows.Forms.TabPage();
            this.countryTabPage = new System.Windows.Forms.TabPage();
            this.tradeTabPage = new System.Windows.Forms.TabPage();
            this.relationTabPage = new System.Windows.Forms.TabPage();
            this.allianceTabPage = new System.Windows.Forms.TabPage();
            this.mainTabPage = new System.Windows.Forms.TabPage();
            this.scenarioTabControl = new System.Windows.Forms.TabControl();
            this.oobTabPage = new System.Windows.Forms.TabPage();
            this.checkButton = new System.Windows.Forms.Button();
            technologyTabPage = new System.Windows.Forms.TabPage();
            this.scenarioTabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // technologyTabPage
            // 
            technologyTabPage.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(technologyTabPage, "technologyTabPage");
            technologyTabPage.Name = "technologyTabPage";
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
            resources.ApplyResources(this.tradeTabPage, "tradeTabPage");
            this.tradeTabPage.Name = "tradeTabPage";
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
        private System.Windows.Forms.TabPage relationTabPage;
        private System.Windows.Forms.TabPage allianceTabPage;
        private System.Windows.Forms.TabPage mainTabPage;
        private System.Windows.Forms.TabControl scenarioTabControl;
        private System.Windows.Forms.Button checkButton;
        private System.Windows.Forms.TabPage oobTabPage;

    }
}