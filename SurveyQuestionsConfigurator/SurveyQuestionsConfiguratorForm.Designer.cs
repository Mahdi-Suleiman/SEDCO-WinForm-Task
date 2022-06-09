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
            this.refreshListButton = new System.Windows.Forms.Button();
            this.createdQuestions_ListView = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.deleteQuestionButton = new System.Windows.Forms.Button();
            this.editQuestionButton = new System.Windows.Forms.Button();
            this.addQuestionButton = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.closeApplicationButton = new System.Windows.Forms.Button();
            this.createdQuestionsGroupBox = new System.Windows.Forms.GroupBox();
            this.errorLabel = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.createdQuestionsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // refreshListButton
            // 
            this.refreshListButton.Location = new System.Drawing.Point(6, 309);
            this.refreshListButton.Name = "refreshListButton";
            this.refreshListButton.Size = new System.Drawing.Size(75, 23);
            this.refreshListButton.TabIndex = 8;
            this.refreshListButton.Text = "Refresh";
            this.toolTip1.SetToolTip(this.refreshListButton, "Refresh List");
            this.refreshListButton.UseVisualStyleBackColor = true;
            this.refreshListButton.Click += new System.EventHandler(this.refreshDataButton_Click);
            // 
            // createdQuestions_ListView
            // 
            this.createdQuestions_ListView.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.createdQuestions_ListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.createdQuestions_ListView.FullRowSelect = true;
            this.createdQuestions_ListView.GridLines = true;
            this.createdQuestions_ListView.HideSelection = false;
            this.createdQuestions_ListView.Location = new System.Drawing.Point(6, 19);
            this.createdQuestions_ListView.MultiSelect = false;
            this.createdQuestions_ListView.Name = "createdQuestions_ListView";
            this.createdQuestions_ListView.Size = new System.Drawing.Size(600, 284);
            this.createdQuestions_ListView.TabIndex = 1;
            this.createdQuestions_ListView.TileSize = new System.Drawing.Size(170, 60);
            this.createdQuestions_ListView.UseCompatibleStateImageBehavior = false;
            this.createdQuestions_ListView.View = System.Windows.Forms.View.Details;
            this.createdQuestions_ListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.createdQuestions_ListView_ColumnClick);
            this.createdQuestions_ListView.DoubleClick += new System.EventHandler(this.editQuestionButton_Click);
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
            this.columnHeader4.Width = 440;
            // 
            // deleteQuestionButton
            // 
            this.deleteQuestionButton.Location = new System.Drawing.Point(525, 309);
            this.deleteQuestionButton.Name = "deleteQuestionButton";
            this.deleteQuestionButton.Size = new System.Drawing.Size(75, 23);
            this.deleteQuestionButton.TabIndex = 4;
            this.deleteQuestionButton.Text = "&Delete";
            this.deleteQuestionButton.UseVisualStyleBackColor = true;
            this.deleteQuestionButton.Click += new System.EventHandler(this.deleteQuestionButton_Click);
            // 
            // editQuestionButton
            // 
            this.editQuestionButton.Location = new System.Drawing.Point(444, 309);
            this.editQuestionButton.Name = "editQuestionButton";
            this.editQuestionButton.Size = new System.Drawing.Size(75, 23);
            this.editQuestionButton.TabIndex = 3;
            this.editQuestionButton.Text = "&Edit";
            this.editQuestionButton.UseVisualStyleBackColor = true;
            this.editQuestionButton.Click += new System.EventHandler(this.editQuestionButton_Click);
            // 
            // addQuestionButton
            // 
            this.addQuestionButton.Location = new System.Drawing.Point(363, 309);
            this.addQuestionButton.Name = "addQuestionButton";
            this.addQuestionButton.Size = new System.Drawing.Size(75, 23);
            this.addQuestionButton.TabIndex = 2;
            this.addQuestionButton.Text = "&Add";
            this.addQuestionButton.UseVisualStyleBackColor = true;
            this.addQuestionButton.Click += new System.EventHandler(this.addQuestionButton_Click);
            // 
            // closeApplicationButton
            // 
            this.closeApplicationButton.Location = new System.Drawing.Point(537, 381);
            this.closeApplicationButton.Name = "closeApplicationButton";
            this.closeApplicationButton.Size = new System.Drawing.Size(75, 23);
            this.closeApplicationButton.TabIndex = 9;
            this.closeApplicationButton.Text = "Close";
            this.closeApplicationButton.UseVisualStyleBackColor = true;
            this.closeApplicationButton.Click += new System.EventHandler(this.closeApplicationButton_Click);
            // 
            // createdQuestionsGroupBox
            // 
            this.createdQuestionsGroupBox.Controls.Add(this.errorLabel);
            this.createdQuestionsGroupBox.Controls.Add(this.createdQuestions_ListView);
            this.createdQuestionsGroupBox.Controls.Add(this.refreshListButton);
            this.createdQuestionsGroupBox.Controls.Add(this.addQuestionButton);
            this.createdQuestionsGroupBox.Controls.Add(this.editQuestionButton);
            this.createdQuestionsGroupBox.Controls.Add(this.deleteQuestionButton);
            this.createdQuestionsGroupBox.Controls.Add(this.menuStrip1);
            this.createdQuestionsGroupBox.Location = new System.Drawing.Point(6, 36);
            this.createdQuestionsGroupBox.Name = "createdQuestionsGroupBox";
            this.createdQuestionsGroupBox.Size = new System.Drawing.Size(606, 339);
            this.createdQuestionsGroupBox.TabIndex = 10;
            this.createdQuestionsGroupBox.TabStop = false;
            this.createdQuestionsGroupBox.Text = "Questions";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.BackColor = System.Drawing.Color.Transparent;
            this.errorLabel.ForeColor = System.Drawing.Color.Red;
            this.errorLabel.Location = new System.Drawing.Point(87, 314);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(58, 13);
            this.errorLabel.TabIndex = 11;
            this.errorLabel.Text = "Error Label";
            this.errorLabel.Visible = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(3, 16);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(600, 24);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // SurveyQuestionsConfiguratorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(621, 410);
            this.Controls.Add(this.createdQuestionsGroupBox);
            this.Controls.Add(this.closeApplicationButton);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(637, 449);
            this.MinimumSize = new System.Drawing.Size(637, 449);
            this.Name = "SurveyQuestionsConfiguratorForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Survey Questions Configurator";
            this.Activated += new System.EventHandler(this.SurveyQuestionsConfiguratorForm_Activated);
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
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button closeApplicationButton;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.GroupBox createdQuestionsGroupBox;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.MenuStrip menuStrip1;
    }
}

