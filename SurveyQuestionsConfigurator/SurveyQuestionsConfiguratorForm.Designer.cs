﻿namespace SurveyQuestionsConfigurator
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
            this.questionsTabPage = new System.Windows.Forms.TabPage();
            this.createdQuestionsGroupBox = new System.Windows.Forms.GroupBox();
            this.refreshDataButton = new System.Windows.Forms.Button();
            this.createdQuestions_ListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.deleteQuestionButton = new System.Windows.Forms.Button();
            this.editQuestionButton = new System.Windows.Forms.Button();
            this.addQuestionButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.closeApplicationButton = new System.Windows.Forms.Button();
            this.questionsTabPage.SuspendLayout();
            this.createdQuestionsGroupBox.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // questionsTabPage
            // 
            this.questionsTabPage.Controls.Add(this.createdQuestionsGroupBox);
            this.questionsTabPage.Location = new System.Drawing.Point(4, 22);
            this.questionsTabPage.Name = "questionsTabPage";
            this.questionsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.questionsTabPage.Size = new System.Drawing.Size(612, 353);
            this.questionsTabPage.TabIndex = 0;
            this.questionsTabPage.Text = "Questions";
            this.questionsTabPage.UseVisualStyleBackColor = true;
            // 
            // createdQuestionsGroupBox
            // 
            this.createdQuestionsGroupBox.Controls.Add(this.refreshDataButton);
            this.createdQuestionsGroupBox.Controls.Add(this.createdQuestions_ListView);
            this.createdQuestionsGroupBox.Controls.Add(this.deleteQuestionButton);
            this.createdQuestionsGroupBox.Controls.Add(this.editQuestionButton);
            this.createdQuestionsGroupBox.Controls.Add(this.addQuestionButton);
            this.createdQuestionsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.createdQuestionsGroupBox.Location = new System.Drawing.Point(3, 3);
            this.createdQuestionsGroupBox.Name = "createdQuestionsGroupBox";
            this.createdQuestionsGroupBox.Size = new System.Drawing.Size(606, 347);
            this.createdQuestionsGroupBox.TabIndex = 5;
            this.createdQuestionsGroupBox.TabStop = false;
            this.createdQuestionsGroupBox.Text = "Created Questions";
            // 
            // refreshDataButton
            // 
            this.refreshDataButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.refreshDataButton.BackgroundImage = global::SurveyQuestionsConfigurator.Properties.Resources.refresh_icon;
            this.refreshDataButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.refreshDataButton.Location = new System.Drawing.Point(6, 311);
            this.refreshDataButton.Margin = new System.Windows.Forms.Padding(5);
            this.refreshDataButton.Name = "refreshDataButton";
            this.refreshDataButton.Padding = new System.Windows.Forms.Padding(15);
            this.refreshDataButton.Size = new System.Drawing.Size(29, 29);
            this.refreshDataButton.TabIndex = 8;
            this.toolTip1.SetToolTip(this.refreshDataButton, "Refresh List");
            this.refreshDataButton.UseVisualStyleBackColor = true;
            this.refreshDataButton.Click += new System.EventHandler(this.refreshDataButton_Click);
            // 
            // createdQuestions_ListView
            // 
            this.createdQuestions_ListView.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.createdQuestions_ListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.createdQuestions_ListView.Dock = System.Windows.Forms.DockStyle.Top;
            this.createdQuestions_ListView.FullRowSelect = true;
            this.createdQuestions_ListView.GridLines = true;
            this.createdQuestions_ListView.HideSelection = false;
            this.createdQuestions_ListView.Location = new System.Drawing.Point(3, 16);
            this.createdQuestions_ListView.Name = "createdQuestions_ListView";
            this.createdQuestions_ListView.Size = new System.Drawing.Size(600, 207);
            this.createdQuestions_ListView.TabIndex = 1;
            this.createdQuestions_ListView.TileSize = new System.Drawing.Size(170, 60);
            this.createdQuestions_ListView.UseCompatibleStateImageBehavior = false;
            this.createdQuestions_ListView.View = System.Windows.Forms.View.Details;
            this.createdQuestions_ListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            this.createdQuestions_ListView.DoubleClick += new System.EventHandler(this.editQuestionButton_Click);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID";
            this.columnHeader1.Width = 75;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Order";
            this.columnHeader2.Width = 75;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Type";
            this.columnHeader3.Width = 80;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Text";
            this.columnHeader4.Width = 250;
            // 
            // deleteQuestionButton
            // 
            this.deleteQuestionButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.deleteQuestionButton.Location = new System.Drawing.Point(508, 313);
            this.deleteQuestionButton.Margin = new System.Windows.Forms.Padding(10);
            this.deleteQuestionButton.Name = "deleteQuestionButton";
            this.deleteQuestionButton.Size = new System.Drawing.Size(91, 25);
            this.deleteQuestionButton.TabIndex = 4;
            this.deleteQuestionButton.Text = "Delete";
            this.deleteQuestionButton.UseVisualStyleBackColor = true;
            this.deleteQuestionButton.Click += new System.EventHandler(this.deleteQuestionButton_Click);
            // 
            // editQuestionButton
            // 
            this.editQuestionButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.editQuestionButton.Location = new System.Drawing.Point(397, 313);
            this.editQuestionButton.Margin = new System.Windows.Forms.Padding(10);
            this.editQuestionButton.Name = "editQuestionButton";
            this.editQuestionButton.Size = new System.Drawing.Size(91, 25);
            this.editQuestionButton.TabIndex = 3;
            this.editQuestionButton.Text = "Edit";
            this.editQuestionButton.UseVisualStyleBackColor = true;
            this.editQuestionButton.Click += new System.EventHandler(this.editQuestionButton_Click);
            // 
            // addQuestionButton
            // 
            this.addQuestionButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.addQuestionButton.Location = new System.Drawing.Point(286, 313);
            this.addQuestionButton.Margin = new System.Windows.Forms.Padding(10);
            this.addQuestionButton.Name = "addQuestionButton";
            this.addQuestionButton.Size = new System.Drawing.Size(91, 25);
            this.addQuestionButton.TabIndex = 2;
            this.addQuestionButton.Text = "Add";
            this.addQuestionButton.UseVisualStyleBackColor = true;
            this.addQuestionButton.Click += new System.EventHandler(this.addQuestionButton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.questionsTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(620, 379);
            this.tabControl1.TabIndex = 6;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(620, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // closeApplicationButton
            // 
            this.closeApplicationButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.closeApplicationButton.Location = new System.Drawing.Point(525, 409);
            this.closeApplicationButton.Margin = new System.Windows.Forms.Padding(10);
            this.closeApplicationButton.Name = "closeApplicationButton";
            this.closeApplicationButton.Size = new System.Drawing.Size(91, 25);
            this.closeApplicationButton.TabIndex = 9;
            this.closeApplicationButton.Text = "Close";
            this.closeApplicationButton.UseVisualStyleBackColor = true;
            this.closeApplicationButton.Click += new System.EventHandler(this.closeApplicationButton_Click);
            // 
            // SurveyQuestionsConfiguratorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 445);
            this.Controls.Add(this.closeApplicationButton);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1000, 484);
            this.MinimumSize = new System.Drawing.Size(636, 484);
            this.Name = "SurveyQuestionsConfiguratorForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Survey Questions Configurator";
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Leave += new System.EventHandler(this.Form1_Leave);
            this.questionsTabPage.ResumeLayout(false);
            this.createdQuestionsGroupBox.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage questionsTabPage;
        private System.Windows.Forms.GroupBox createdQuestionsGroupBox;
        private System.Windows.Forms.ListView createdQuestions_ListView;
        private System.Windows.Forms.Button deleteQuestionButton;
        private System.Windows.Forms.Button editQuestionButton;
        private System.Windows.Forms.Button addQuestionButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button refreshDataButton;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button closeApplicationButton;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
    }
}

