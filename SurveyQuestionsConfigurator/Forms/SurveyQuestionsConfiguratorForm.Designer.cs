namespace SurveyQuestionsConfigurator
{
    partial class SurveyQuestionsConfiguratorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SurveyQuestionsConfiguratorForm));
            this.refreshListButton = new System.Windows.Forms.Button();
            this.createdQuestions_ListView = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.deleteQuestionButton = new System.Windows.Forms.Button();
            this.editQuestionButton = new System.Windows.Forms.Button();
            this.addQuestionButton = new System.Windows.Forms.Button();
            this.connectionSettingsToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.settingsButton = new System.Windows.Forms.Button();
            this.languageSettingsButton = new System.Windows.Forms.Button();
            this.closeApplicationButton = new System.Windows.Forms.Button();
            this.createdQuestionsGroupBox = new System.Windows.Forms.GroupBox();
            this.errorLabel = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.label5 = new System.Windows.Forms.Label();
            this.langaugeSettingsToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.createdQuestionsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // refreshListButton
            // 
            resources.ApplyResources(this.refreshListButton, "refreshListButton");
            this.refreshListButton.Name = "refreshListButton";
            this.connectionSettingsToolTip.SetToolTip(this.refreshListButton, resources.GetString("refreshListButton.ToolTip"));
            this.langaugeSettingsToolTip.SetToolTip(this.refreshListButton, resources.GetString("refreshListButton.ToolTip1"));
            this.refreshListButton.UseVisualStyleBackColor = true;
            this.refreshListButton.Click += new System.EventHandler(this.refreshDataButton_Click);
            // 
            // createdQuestions_ListView
            // 
            resources.ApplyResources(this.createdQuestions_ListView, "createdQuestions_ListView");
            this.createdQuestions_ListView.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.createdQuestions_ListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.createdQuestions_ListView.FullRowSelect = true;
            this.createdQuestions_ListView.GridLines = true;
            this.createdQuestions_ListView.HideSelection = false;
            this.createdQuestions_ListView.MultiSelect = false;
            this.createdQuestions_ListView.Name = "createdQuestions_ListView";
            this.createdQuestions_ListView.TileSize = new System.Drawing.Size(170, 60);
            this.connectionSettingsToolTip.SetToolTip(this.createdQuestions_ListView, resources.GetString("createdQuestions_ListView.ToolTip"));
            this.langaugeSettingsToolTip.SetToolTip(this.createdQuestions_ListView, resources.GetString("createdQuestions_ListView.ToolTip1"));
            this.createdQuestions_ListView.UseCompatibleStateImageBehavior = false;
            this.createdQuestions_ListView.View = System.Windows.Forms.View.Details;
            this.createdQuestions_ListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.createdQuestions_ListView_ColumnClick);
            this.createdQuestions_ListView.DoubleClick += new System.EventHandler(this.editQuestionButton_Click);
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // columnHeader4
            // 
            resources.ApplyResources(this.columnHeader4, "columnHeader4");
            // 
            // deleteQuestionButton
            // 
            resources.ApplyResources(this.deleteQuestionButton, "deleteQuestionButton");
            this.deleteQuestionButton.Name = "deleteQuestionButton";
            this.connectionSettingsToolTip.SetToolTip(this.deleteQuestionButton, resources.GetString("deleteQuestionButton.ToolTip"));
            this.langaugeSettingsToolTip.SetToolTip(this.deleteQuestionButton, resources.GetString("deleteQuestionButton.ToolTip1"));
            this.deleteQuestionButton.UseVisualStyleBackColor = true;
            this.deleteQuestionButton.Click += new System.EventHandler(this.deleteQuestionButton_Click);
            // 
            // editQuestionButton
            // 
            resources.ApplyResources(this.editQuestionButton, "editQuestionButton");
            this.editQuestionButton.Name = "editQuestionButton";
            this.connectionSettingsToolTip.SetToolTip(this.editQuestionButton, resources.GetString("editQuestionButton.ToolTip"));
            this.langaugeSettingsToolTip.SetToolTip(this.editQuestionButton, resources.GetString("editQuestionButton.ToolTip1"));
            this.editQuestionButton.UseVisualStyleBackColor = true;
            this.editQuestionButton.Click += new System.EventHandler(this.editQuestionButton_Click);
            // 
            // addQuestionButton
            // 
            resources.ApplyResources(this.addQuestionButton, "addQuestionButton");
            this.addQuestionButton.Name = "addQuestionButton";
            this.connectionSettingsToolTip.SetToolTip(this.addQuestionButton, resources.GetString("addQuestionButton.ToolTip"));
            this.langaugeSettingsToolTip.SetToolTip(this.addQuestionButton, resources.GetString("addQuestionButton.ToolTip1"));
            this.addQuestionButton.UseVisualStyleBackColor = true;
            this.addQuestionButton.Click += new System.EventHandler(this.addQuestionButton_Click);
            // 
            // settingsButton
            // 
            resources.ApplyResources(this.settingsButton, "settingsButton");
            this.settingsButton.BackgroundImage = global::SurveyQuestionsConfigurator.Properties.Resources.gear;
            this.settingsButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.settingsButton.ForeColor = System.Drawing.Color.Transparent;
            this.settingsButton.Name = "settingsButton";
            this.connectionSettingsToolTip.SetToolTip(this.settingsButton, resources.GetString("settingsButton.ToolTip"));
            this.langaugeSettingsToolTip.SetToolTip(this.settingsButton, resources.GetString("settingsButton.ToolTip1"));
            this.settingsButton.UseVisualStyleBackColor = true;
            this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);
            // 
            // languageSettingsButton
            // 
            resources.ApplyResources(this.languageSettingsButton, "languageSettingsButton");
            this.languageSettingsButton.BackgroundImage = global::SurveyQuestionsConfigurator.Properties.Resources.language;
            this.languageSettingsButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.languageSettingsButton.ForeColor = System.Drawing.Color.Transparent;
            this.languageSettingsButton.Name = "languageSettingsButton";
            this.connectionSettingsToolTip.SetToolTip(this.languageSettingsButton, resources.GetString("languageSettingsButton.ToolTip"));
            this.langaugeSettingsToolTip.SetToolTip(this.languageSettingsButton, resources.GetString("languageSettingsButton.ToolTip1"));
            this.languageSettingsButton.UseVisualStyleBackColor = true;
            this.languageSettingsButton.Click += new System.EventHandler(this.languageSettingsButton_Click);
            // 
            // closeApplicationButton
            // 
            resources.ApplyResources(this.closeApplicationButton, "closeApplicationButton");
            this.closeApplicationButton.Name = "closeApplicationButton";
            this.connectionSettingsToolTip.SetToolTip(this.closeApplicationButton, resources.GetString("closeApplicationButton.ToolTip"));
            this.langaugeSettingsToolTip.SetToolTip(this.closeApplicationButton, resources.GetString("closeApplicationButton.ToolTip1"));
            this.closeApplicationButton.UseVisualStyleBackColor = true;
            this.closeApplicationButton.Click += new System.EventHandler(this.closeApplicationButton_Click);
            // 
            // createdQuestionsGroupBox
            // 
            resources.ApplyResources(this.createdQuestionsGroupBox, "createdQuestionsGroupBox");
            this.createdQuestionsGroupBox.Controls.Add(this.errorLabel);
            this.createdQuestionsGroupBox.Controls.Add(this.createdQuestions_ListView);
            this.createdQuestionsGroupBox.Controls.Add(this.addQuestionButton);
            this.createdQuestionsGroupBox.Controls.Add(this.editQuestionButton);
            this.createdQuestionsGroupBox.Controls.Add(this.refreshListButton);
            this.createdQuestionsGroupBox.Controls.Add(this.deleteQuestionButton);
            this.createdQuestionsGroupBox.Controls.Add(this.menuStrip1);
            this.createdQuestionsGroupBox.Name = "createdQuestionsGroupBox";
            this.createdQuestionsGroupBox.TabStop = false;
            this.connectionSettingsToolTip.SetToolTip(this.createdQuestionsGroupBox, resources.GetString("createdQuestionsGroupBox.ToolTip"));
            this.langaugeSettingsToolTip.SetToolTip(this.createdQuestionsGroupBox, resources.GetString("createdQuestionsGroupBox.ToolTip1"));
            // 
            // errorLabel
            // 
            resources.ApplyResources(this.errorLabel, "errorLabel");
            this.errorLabel.BackColor = System.Drawing.Color.Transparent;
            this.errorLabel.ForeColor = System.Drawing.Color.Red;
            this.errorLabel.Name = "errorLabel";
            this.connectionSettingsToolTip.SetToolTip(this.errorLabel, resources.GetString("errorLabel.ToolTip"));
            this.langaugeSettingsToolTip.SetToolTip(this.errorLabel, resources.GetString("errorLabel.ToolTip1"));
            // 
            // menuStrip1
            // 
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            this.langaugeSettingsToolTip.SetToolTip(this.menuStrip1, resources.GetString("menuStrip1.ToolTip"));
            this.connectionSettingsToolTip.SetToolTip(this.menuStrip1, resources.GetString("menuStrip1.ToolTip1"));
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label5.Cursor = System.Windows.Forms.Cursors.Default;
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label5.Name = "label5";
            this.connectionSettingsToolTip.SetToolTip(this.label5, resources.GetString("label5.ToolTip"));
            this.langaugeSettingsToolTip.SetToolTip(this.label5, resources.GetString("label5.ToolTip1"));
            // 
            // SurveyQuestionsConfiguratorForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.languageSettingsButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.settingsButton);
            this.Controls.Add(this.createdQuestionsGroupBox);
            this.Controls.Add(this.closeApplicationButton);
            this.MaximizeBox = false;
            this.Name = "SurveyQuestionsConfiguratorForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.connectionSettingsToolTip.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.langaugeSettingsToolTip.SetToolTip(this, resources.GetString("$this.ToolTip1"));
            this.Load += new System.EventHandler(this.SurveyQuestionsConfiguratorForm_Load);
            this.createdQuestionsGroupBox.ResumeLayout(false);
            this.createdQuestionsGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListView createdQuestions_ListView;
        private System.Windows.Forms.Button deleteQuestionButton;
        private System.Windows.Forms.Button editQuestionButton;
        private System.Windows.Forms.Button addQuestionButton;
        private System.Windows.Forms.Button refreshListButton;
        private System.Windows.Forms.ToolTip connectionSettingsToolTip;
        private System.Windows.Forms.Button closeApplicationButton;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.GroupBox createdQuestionsGroupBox;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Button settingsButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button languageSettingsButton;
        private System.Windows.Forms.ToolTip langaugeSettingsToolTip;
    }
}

