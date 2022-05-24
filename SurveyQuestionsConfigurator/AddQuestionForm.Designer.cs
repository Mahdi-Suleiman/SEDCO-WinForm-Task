namespace SurveyQuestionsConfigurator
{
    partial class AddQuestionForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.smileyQuestionTabPage = new System.Windows.Forms.TabPage();
            this.questionOrderNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.questionTextRichTextBox = new System.Windows.Forms.RichTextBox();
            this.genericNumericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.genericLabel1 = new System.Windows.Forms.Label();
            this.questionTextLabel = new System.Windows.Forms.Label();
            this.questionOrderLabel = new System.Windows.Forms.Label();
            this.genericTextBox2 = new System.Windows.Forms.TextBox();
            this.genericTextBox1 = new System.Windows.Forms.TextBox();
            this.genericLabel4 = new System.Windows.Forms.Label();
            this.genericLabel3 = new System.Windows.Forms.Label();
            this.genericNumericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.genericLabel2 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeButton = new System.Windows.Forms.Button();
            this.smilyQuestionDetailsGroupBox = new System.Windows.Forms.GroupBox();
            this.addQuestionButton = new System.Windows.Forms.Button();
            this.typeOfQuestionGroupBox = new System.Windows.Forms.GroupBox();
            this.questionTypeComboBox = new System.Windows.Forms.ComboBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.editQuestionButton = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.smileyQuestionTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.questionOrderNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.genericNumericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.genericNumericUpDown2)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.smilyQuestionDetailsGroupBox.SuspendLayout();
            this.typeOfQuestionGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.smileyQuestionTabPage);
            this.tabControl1.Location = new System.Drawing.Point(12, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(380, 468);
            this.tabControl1.TabIndex = 0;
            // 
            // smileyQuestionTabPage
            // 
            this.smileyQuestionTabPage.Controls.Add(this.pictureBox3);
            this.smileyQuestionTabPage.Controls.Add(this.smilyQuestionDetailsGroupBox);
            this.smileyQuestionTabPage.Controls.Add(this.typeOfQuestionGroupBox);
            this.smileyQuestionTabPage.Location = new System.Drawing.Point(4, 22);
            this.smileyQuestionTabPage.Name = "smileyQuestionTabPage";
            this.smileyQuestionTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.smileyQuestionTabPage.Size = new System.Drawing.Size(372, 442);
            this.smileyQuestionTabPage.TabIndex = 0;
            this.smileyQuestionTabPage.Text = "Question Details";
            this.smileyQuestionTabPage.UseVisualStyleBackColor = true;
            // 
            // questionOrderNumericUpDown
            // 
            this.questionOrderNumericUpDown.Location = new System.Drawing.Point(9, 39);
            this.questionOrderNumericUpDown.Margin = new System.Windows.Forms.Padding(5);
            this.questionOrderNumericUpDown.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.questionOrderNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.questionOrderNumericUpDown.Name = "questionOrderNumericUpDown";
            this.questionOrderNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.questionOrderNumericUpDown.TabIndex = 1;
            this.questionOrderNumericUpDown.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.questionOrderNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // questionTextRichTextBox
            // 
            this.questionTextRichTextBox.Location = new System.Drawing.Point(9, 90);
            this.questionTextRichTextBox.Margin = new System.Windows.Forms.Padding(5);
            this.questionTextRichTextBox.Name = "questionTextRichTextBox";
            this.questionTextRichTextBox.Size = new System.Drawing.Size(340, 77);
            this.questionTextRichTextBox.TabIndex = 2;
            this.questionTextRichTextBox.Text = "";
            // 
            // genericNumericUpDown1
            // 
            this.genericNumericUpDown1.Location = new System.Drawing.Point(9, 201);
            this.genericNumericUpDown1.Margin = new System.Windows.Forms.Padding(5);
            this.genericNumericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.genericNumericUpDown1.Name = "genericNumericUpDown1";
            this.genericNumericUpDown1.Size = new System.Drawing.Size(120, 20);
            this.genericNumericUpDown1.TabIndex = 1;
            this.genericNumericUpDown1.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.genericNumericUpDown1.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // genericLabel1
            // 
            this.genericLabel1.AutoSize = true;
            this.genericLabel1.Location = new System.Drawing.Point(6, 183);
            this.genericLabel1.Name = "genericLabel1";
            this.genericLabel1.Size = new System.Drawing.Size(156, 13);
            this.genericLabel1.TabIndex = 2;
            this.genericLabel1.Text = "Number Of Smiley Faces (2 -5 ):";
            // 
            // questionTextLabel
            // 
            this.questionTextLabel.AutoSize = true;
            this.questionTextLabel.Location = new System.Drawing.Point(6, 72);
            this.questionTextLabel.Name = "questionTextLabel";
            this.questionTextLabel.Size = new System.Drawing.Size(76, 13);
            this.questionTextLabel.TabIndex = 1;
            this.questionTextLabel.Text = "Question Text:";
            // 
            // questionOrderLabel
            // 
            this.questionOrderLabel.AutoSize = true;
            this.questionOrderLabel.Location = new System.Drawing.Point(6, 21);
            this.questionOrderLabel.Name = "questionOrderLabel";
            this.questionOrderLabel.Size = new System.Drawing.Size(81, 13);
            this.questionOrderLabel.TabIndex = 0;
            this.questionOrderLabel.Text = "Question Order:";
            // 
            // genericTextBox2
            // 
            this.genericTextBox2.Location = new System.Drawing.Point(213, 267);
            this.genericTextBox2.Margin = new System.Windows.Forms.Padding(5);
            this.genericTextBox2.Name = "genericTextBox2";
            this.genericTextBox2.Size = new System.Drawing.Size(136, 20);
            this.genericTextBox2.TabIndex = 6;
            // 
            // genericTextBox1
            // 
            this.genericTextBox1.Location = new System.Drawing.Point(213, 201);
            this.genericTextBox1.Margin = new System.Windows.Forms.Padding(5);
            this.genericTextBox1.Name = "genericTextBox1";
            this.genericTextBox1.Size = new System.Drawing.Size(136, 20);
            this.genericTextBox1.TabIndex = 4;
            // 
            // genericLabel4
            // 
            this.genericLabel4.AutoSize = true;
            this.genericLabel4.Location = new System.Drawing.Point(210, 250);
            this.genericLabel4.Name = "genericLabel4";
            this.genericLabel4.Size = new System.Drawing.Size(95, 13);
            this.genericLabel4.TabIndex = 19;
            this.genericLabel4.Text = "End Value Caption";
            // 
            // genericLabel3
            // 
            this.genericLabel3.AutoSize = true;
            this.genericLabel3.Location = new System.Drawing.Point(211, 183);
            this.genericLabel3.Name = "genericLabel3";
            this.genericLabel3.Size = new System.Drawing.Size(98, 13);
            this.genericLabel3.TabIndex = 18;
            this.genericLabel3.Text = "Start Value Caption";
            // 
            // genericNumericUpDown2
            // 
            this.genericNumericUpDown2.Location = new System.Drawing.Point(9, 268);
            this.genericNumericUpDown2.Margin = new System.Windows.Forms.Padding(5);
            this.genericNumericUpDown2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.genericNumericUpDown2.Name = "genericNumericUpDown2";
            this.genericNumericUpDown2.Size = new System.Drawing.Size(120, 20);
            this.genericNumericUpDown2.TabIndex = 5;
            this.genericNumericUpDown2.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.genericNumericUpDown2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // genericLabel2
            // 
            this.genericLabel2.AutoSize = true;
            this.genericLabel2.Location = new System.Drawing.Point(6, 250);
            this.genericLabel2.Name = "genericLabel2";
            this.genericLabel2.Size = new System.Drawing.Size(59, 13);
            this.genericLabel2.TabIndex = 14;
            this.genericLabel2.Text = "Start Value";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(398, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            this.fileToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // closeButton
            // 
            this.closeButton.BackColor = System.Drawing.Color.Transparent;
            this.closeButton.Location = new System.Drawing.Point(314, 497);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(74, 23);
            this.closeButton.TabIndex = 9;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = false;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // smilyQuestionDetailsGroupBox
            // 
            this.smilyQuestionDetailsGroupBox.Controls.Add(this.addQuestionButton);
            this.smilyQuestionDetailsGroupBox.Controls.Add(this.questionTextRichTextBox);
            this.smilyQuestionDetailsGroupBox.Controls.Add(this.questionOrderLabel);
            this.smilyQuestionDetailsGroupBox.Controls.Add(this.editQuestionButton);
            this.smilyQuestionDetailsGroupBox.Controls.Add(this.questionTextLabel);
            this.smilyQuestionDetailsGroupBox.Controls.Add(this.questionOrderNumericUpDown);
            this.smilyQuestionDetailsGroupBox.Controls.Add(this.genericTextBox2);
            this.smilyQuestionDetailsGroupBox.Controls.Add(this.genericLabel1);
            this.smilyQuestionDetailsGroupBox.Controls.Add(this.genericLabel4);
            this.smilyQuestionDetailsGroupBox.Controls.Add(this.genericNumericUpDown1);
            this.smilyQuestionDetailsGroupBox.Controls.Add(this.genericLabel3);
            this.smilyQuestionDetailsGroupBox.Controls.Add(this.genericTextBox1);
            this.smilyQuestionDetailsGroupBox.Controls.Add(this.genericNumericUpDown2);
            this.smilyQuestionDetailsGroupBox.Controls.Add(this.genericLabel2);
            this.smilyQuestionDetailsGroupBox.Location = new System.Drawing.Point(6, 92);
            this.smilyQuestionDetailsGroupBox.Name = "smilyQuestionDetailsGroupBox";
            this.smilyQuestionDetailsGroupBox.Size = new System.Drawing.Size(362, 349);
            this.smilyQuestionDetailsGroupBox.TabIndex = 7;
            this.smilyQuestionDetailsGroupBox.TabStop = false;
            this.smilyQuestionDetailsGroupBox.Text = "Question Details";
            // 
            // addQuestionButton
            // 
            this.addQuestionButton.Location = new System.Drawing.Point(270, 315);
            this.addQuestionButton.Name = "addQuestionButton";
            this.addQuestionButton.Size = new System.Drawing.Size(79, 23);
            this.addQuestionButton.TabIndex = 7;
            this.addQuestionButton.Text = "Add Question";
            this.addQuestionButton.UseVisualStyleBackColor = true;
            this.addQuestionButton.Click += new System.EventHandler(this.addQuestionButton_Click);
            // 
            // typeOfQuestionGroupBox
            // 
            this.typeOfQuestionGroupBox.Controls.Add(this.questionTypeComboBox);
            this.typeOfQuestionGroupBox.Location = new System.Drawing.Point(6, 6);
            this.typeOfQuestionGroupBox.Name = "typeOfQuestionGroupBox";
            this.typeOfQuestionGroupBox.Size = new System.Drawing.Size(269, 70);
            this.typeOfQuestionGroupBox.TabIndex = 16;
            this.typeOfQuestionGroupBox.TabStop = false;
            this.typeOfQuestionGroupBox.Text = "Type Of Question";
            // 
            // questionTypeComboBox
            // 
            this.questionTypeComboBox.FormattingEnabled = true;
            this.questionTypeComboBox.Items.AddRange(new object[] {
            "Smiley Question",
            "Slider Question",
            "Star Question"});
            this.questionTypeComboBox.Location = new System.Drawing.Point(6, 19);
            this.questionTypeComboBox.Name = "questionTypeComboBox";
            this.questionTypeComboBox.Size = new System.Drawing.Size(250, 21);
            this.questionTypeComboBox.TabIndex = 0;
            this.questionTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.questionTypeComboBox_SelectedIndexChanged);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::SurveyQuestionsConfigurator.Properties.Resources.questionMark;
            this.pictureBox3.Location = new System.Drawing.Point(281, 11);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Padding = new System.Windows.Forms.Padding(15, 5, 15, 5);
            this.pictureBox3.Size = new System.Drawing.Size(87, 65);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 1;
            this.pictureBox3.TabStop = false;
            // 
            // editQuestionButton
            // 
            this.editQuestionButton.Location = new System.Drawing.Point(270, 315);
            this.editQuestionButton.Name = "editQuestionButton";
            this.editQuestionButton.Size = new System.Drawing.Size(79, 23);
            this.editQuestionButton.TabIndex = 8;
            this.editQuestionButton.Text = "Edit Question";
            this.editQuestionButton.UseVisualStyleBackColor = true;
            this.editQuestionButton.Click += new System.EventHandler(this.editQuestionButton_Click);
            // 
            // AddQuestionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 529);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "AddQuestionForm";
            this.Text = "Add a question";
            this.Load += new System.EventHandler(this.AddQuestionForm_Load);
            this.Leave += new System.EventHandler(this.AddQuestionForm_Leave);
            this.tabControl1.ResumeLayout(false);
            this.smileyQuestionTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.questionOrderNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.genericNumericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.genericNumericUpDown2)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.smilyQuestionDetailsGroupBox.ResumeLayout(false);
            this.smilyQuestionDetailsGroupBox.PerformLayout();
            this.typeOfQuestionGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage smileyQuestionTabPage;
        private System.Windows.Forms.NumericUpDown questionOrderNumericUpDown;
        private System.Windows.Forms.RichTextBox questionTextRichTextBox;
        private System.Windows.Forms.NumericUpDown genericNumericUpDown1;
        private System.Windows.Forms.Label genericLabel1;
        private System.Windows.Forms.Label questionTextLabel;
        private System.Windows.Forms.Label questionOrderLabel;
        private System.Windows.Forms.TextBox genericTextBox2;
        private System.Windows.Forms.TextBox genericTextBox1;
        private System.Windows.Forms.Label genericLabel4;
        private System.Windows.Forms.Label genericLabel3;
        private System.Windows.Forms.NumericUpDown genericNumericUpDown2;
        private System.Windows.Forms.Label genericLabel2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.GroupBox smilyQuestionDetailsGroupBox;
        private System.Windows.Forms.GroupBox typeOfQuestionGroupBox;
        private System.Windows.Forms.ComboBox questionTypeComboBox;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Button addQuestionButton;
        private System.Windows.Forms.Button editQuestionButton;
    }
}