using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SurveyQuestionsConfigurator
{
    public partial class AddQuestionForm : Form
    {
        public int QuestionIdForEditing { get; set; }
        //private ConnectionStringSettings = ConfigurationManager.
        ConnectionStringSettings cn = ConfigurationManager.ConnectionStrings["cn"];
        public AddQuestionForm()
        {
            InitializeComponent();
        }
        public AddQuestionForm(int activeTab, int questionId, string questionType/*, string questionText, int numberOfSmileyFaces*/)
        {
            InitializeComponent();

            QuestionIdForEditing = questionId;

            foreach (TabPage tab in tabControl1.TabPages)
            {
                if (tab.TabIndex == activeTab)
                {
                    tabControl1.SelectedTab = tabControl1.TabPages[activeTab];
                    tab.Enabled = true;
                }
                else
                {
                    tab.Enabled = false;
                }
            }

            if (questionType == Form1.QuestionType.SMILEY.ToString())
            {
                InitializeEditingSmileyQuestion(questionId);
            }
            else if (questionType == Form1.QuestionType.SLIDER.ToString())
            {
                InitializeEditingSlideQuestion(questionId);
            }
            else if (questionType == Form1.QuestionType.STAR.ToString())
            {
                InitializeEditingStarQuestion(questionId);
            }



            //var cn = ConfigurationManager.ConnectionStrings["cn"];
            //SqlDataReader reader = null;
            //SqlConnection conn = null;
            //try
            //{
            //    conn = new SqlConnection(cn.ConnectionString);
            //    SqlCommand cmd = new SqlCommand($"select * from Smiley_Questions where QuestionID = {questionId}", conn);
            //    conn.Open();
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {

            //    }
            //    reader.Close();
            //}
            //catch (SqlException ex)
            //{
            //    MessageBox.Show("SQL Error:\n" + ex);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Something went wrong:\n" + ex);
            //}
            //finally
            //{
            //    reader.Close();
            //    conn.Close();
            //}
            //smileyQuestion_OrderNumericUpDown.Value = questionOrder;
            //smileyQuestion_TextRichTextBox.Text = questionText;
            //smileyQuestion_NumberOfSmileyFacesNumericUpDown.Value = numberOfSmileyFaces;

        }

        //Smiley question logic
        private void smileyQuestionAddQuestionButton_Click(object sender, EventArgs e)
        {

            //SqlDataReader reader; // get row count
            var cn = ConfigurationManager.ConnectionStrings["cn"]; //get connection string information from App.config

            SqlConnection conn = null; //create a "method scope" SqlConnection object and reference it to null

            //declare variables
            int questionOrder, NumberOfSmilyFaces;
            string QuestionText;

            if (!String.IsNullOrWhiteSpace(smileyQuestion_TextRichTextBox.Text)) //if Question text is not null or empty 
            {
                questionOrder = Convert.ToInt32(smileyQuestion_OrderNumericUpDown.Value);
                QuestionText = smileyQuestion_TextRichTextBox.Text;
                NumberOfSmilyFaces = Convert.ToInt32(smileyQuestion_NumberOfSmileyFacesNumericUpDown.Value);
                try
                {
                    conn = new SqlConnection(cn.ConnectionString);
                    SqlCommand cmd = new SqlCommand($@"
USE SurveyQuestionsConfigurator
INSERT INTO Smiley_Questions
(QuestionOrder, QuestionText, NumberOfSmileyFaces)
VALUES
({questionOrder},'{QuestionText}',{NumberOfSmilyFaces})", conn);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Question inserted successfully");

                }
                catch (SqlException ex)
                {

                    //2627 -> primary key violation
                    //2601 -> unique key violation
                    // ex.Number
                    if (ex.Number == 2601)
                    {
                        MessageBox.Show("This Question order is already in use\nTry using another one");
                    }
                    else
                    {
                        MessageBox.Show("SQL Error:\n" + ex);
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong:\n" + ex);
                }
                finally
                {
                    conn.Close();
                }
            }
            else
            {
                MessageBox.Show("Question text cant be empty", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }// end of event

        private void sliderQuestionAddQuestionButton_Click(object sender, EventArgs e)
        {
            int questionOrder, questionStartValue, questionEndValue;
            string questionText, questionStartValueCaption, questionEndValueCaption;


            if (!String.IsNullOrWhiteSpace(sliderQuestion_QuestionTextRichTextBox.Text)) //if Question text is not null or empty 
            {
                var cn = ConfigurationManager.ConnectionStrings["cn"]; //get connection string information from App.config

                SqlConnection conn = null;

                questionOrder = Convert.ToInt32(sliderQuestion_QuestionOrderNumericUpDown.Value);

                questionText = (string)sliderQuestion_QuestionTextRichTextBox.Text;




                if (!String.IsNullOrWhiteSpace(sliderQuestion_StartValueCaptionTextBox.Text) &&
                    !String.IsNullOrWhiteSpace(sliderQuestion_EndValueCaptionTextBox.Text))
                {
                    questionStartValueCaption = (string)sliderQuestion_StartValueCaptionTextBox.Text;
                    questionEndValueCaption = (string)sliderQuestion_EndValueCaptionTextBox.Text;

                    questionStartValue = Convert.ToInt32(sliderQuestion_StartValueNumericUpDown.Value);
                    questionEndValue = Convert.ToInt32(sliderQuestion_EndValueNumericUpDown.Value);

                    try
                    {
                        conn = new SqlConnection(cn.ConnectionString);
                        SqlCommand cmd = new SqlCommand($@"
USE SurveyQuestionsConfigurator
INSERT INTO Slider_Questions
(QuestionOrder, QuestionText, QuestionStartValue, QuestionEndValue, QuestionStartValueCaption, QuestionEndValueCaption)
VALUES
({questionOrder},'{questionText}',{questionStartValue}, {questionEndValue}, '{questionStartValueCaption}', '{questionEndValueCaption}' )",
conn);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Question added successfully");


                    }
                    catch (SqlException ex)
                    {

                        //2627 -> primary key violation
                        if (ex.Number == 2627)
                        {
                            MessageBox.Show("This Question order is already in use\nTry using another one");
                        }
                        else
                        {
                            MessageBox.Show("SQL Error:\n" + ex);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something went wrong:\n" + ex);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Start Value caption and End Value caption can NOT be empty", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Question text cant be empty", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } // end of event

        private void starQuestionAddQuestionButton_Click(object sender, EventArgs e)
        {
            var cn = ConfigurationManager.ConnectionStrings["cn"]; //get connection string information from App.config

            SqlConnection conn = null; //create a "method scope" SqlConnection object and reference it to null

            //declare variables
            int questionOrder, NumberOfStars;
            string questionText;
            if (!String.IsNullOrWhiteSpace(starQuestion_TextRichTextBox.Text)) //if Question text is not null or empty 
            {
                questionOrder = Convert.ToInt32(starQuestion_QuestionOrderNumericUpDown.Value);
                questionText = starQuestion_TextRichTextBox.Text;
                NumberOfStars = Convert.ToInt32(starQuestion_NumberOfStarsNumericUpDown.Value);
                try
                {
                    conn = new SqlConnection(cn.ConnectionString);
                    SqlCommand cmd = new SqlCommand($@"
USE SurveyQuestionsConfigurator
INSERT INTO Star_Questions
(QuestionOrder, QuestionText, NumberOfStars)
values
({questionOrder}, '{questionText}', {NumberOfStars})", conn);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Question inserted successfully");

                }
                catch (SqlException ex)
                {

                    //2627 -> primary key violation
                    //2601 -> unique key violation
                    // ex.Number
                    if (ex.Number == 2627)
                    {
                        MessageBox.Show("This Question order is already in use\nTry using another one");
                    }
                    else
                    {
                        MessageBox.Show("SQL Error:\n" + ex);
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong:\n" + ex);
                }
                finally
                {
                    conn.Close();
                }
            }
            else
            {
                MessageBox.Show("Question text cant be empty", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }// end of event

        private void AddQuestionForm_Leave(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void smileyQuestion_EditQuestionButton_Click(object sender, EventArgs e)
        {
            int questionOrder, NumberOfSmilyFaces;
            string QuestionText;

            questionOrder = Convert.ToInt32(smileyQuestion_OrderNumericUpDown.Value);
            QuestionText = smileyQuestion_TextRichTextBox.Text;
            NumberOfSmilyFaces = Convert.ToInt32(smileyQuestion_NumberOfSmileyFacesNumericUpDown.Value);
            if (CheckSmileyQuestionInputFields())
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(cn.ConnectionString))
                    {
                        SqlCommand cmd = new SqlCommand($@"
USE SurveyQuestionsConfigurator
 UPDATE Smiley_Questions
 SET QuestionOrder = {questionOrder}, QuestionText = '{QuestionText}', NumberOfSmileyFaces ={NumberOfSmilyFaces}
 WHERE QuestionID = {QuestionIdForEditing};
", conn);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Question edited successfully");
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("SQL Error:\n" + ex);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong:\n" + ex);
                }
            }
            else
            {
                MessageBox.Show("Question Text Can NOT be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeEditingSmileyQuestion(int questionId)
        {

            SqlConnection conn = null;
            var cn = ConfigurationManager.ConnectionStrings["cn"];
            SqlDataReader reader = null;

            try
            {
                conn = new SqlConnection(cn.ConnectionString);
                SqlCommand cmd = new SqlCommand($"select * from Smiley_Questions where QuestionID = {questionId}", conn);
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    smileyQuestion_OrderNumericUpDown.Value = Convert.ToDecimal(reader[1]);
                    smileyQuestion_TextRichTextBox.Text = (string)reader[2];
                    smileyQuestion_NumberOfSmileyFacesNumericUpDown.Value = Convert.ToDecimal(reader[3]);
                }
            }
            catch (SqlException ex)
            {

                //2627 -> primary key violation
                //2601 -> unique key violation
                // ex.Number
                if (ex.Number == 2601)
                {
                    MessageBox.Show("This Question order is already in use\nTry using another one");
                }
                else
                {
                    MessageBox.Show("SQL Error:\n" + ex);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong:\n" + ex);
            }
            finally
            {
                conn.Close();
            }

            smileyQuestion_AddQuestionButton.Enabled = false;
            smileyQuestion_AddQuestionButton.Visible = false;

            smileyQuestion_EditQuestionButton.Enabled = true;
            smileyQuestion_EditQuestionButton.Visible = true;
        } //end function

        private void InitializeEditingSlideQuestion(int questionId)
        {

            SqlConnection conn = null;
            var cn = ConfigurationManager.ConnectionStrings["cn"];
            SqlDataReader reader = null;

            try
            {
                conn = new SqlConnection(cn.ConnectionString);
                SqlCommand cmd = new SqlCommand($"select * from Slider_Questions where QuestionID = {questionId}", conn);
                conn.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    sliderQuestion_QuestionOrderNumericUpDown.Value = Convert.ToDecimal(reader[1]);
                    sliderQuestion_QuestionTextRichTextBox.Text = (string)reader[2];
                    sliderQuestion_StartValueNumericUpDown.Value = Convert.ToDecimal(reader[3]);
                    sliderQuestion_EndValueNumericUpDown.Value = Convert.ToDecimal(reader[4]);

                    sliderQuestion_StartValueCaptionTextBox.Text = (string)reader[5];
                    sliderQuestion_EndValueCaptionTextBox.Text = (string)reader[6];
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL Error:\n" + ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong:\n" + ex);
            }
            finally
            {
                conn.Close();
            }

            sliderQuestion_AddQuestionButton.Enabled = false;
            sliderQuestion_AddQuestionButton.Visible = false;

            sliderQuestion_EditQuestionButton.Enabled = true;
            sliderQuestion_EditQuestionButton.Visible = true;
        } //end function

        private void InitializeEditingStarQuestion(int questionId)
        {

            SqlConnection conn = null;
            var cn = ConfigurationManager.ConnectionStrings["cn"];
            SqlDataReader reader = null;

            try
            {
                conn = new SqlConnection(cn.ConnectionString);
                SqlCommand cmd = new SqlCommand($@"select * from Star_Questions where QuestionID = {questionId}", conn);
                conn.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    starQuestion_QuestionOrderNumericUpDown.Value = Convert.ToDecimal(reader[1]);
                    starQuestion_TextRichTextBox.Text = (string)reader[2];
                    starQuestion_NumberOfStarsNumericUpDown.Value = Convert.ToDecimal(reader[3]);
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL Error:\n" + ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong:\n" + ex);
            }
            finally
            {
                conn.Close();
            }

            starQuestion_AddQuestionButton.Enabled = false;
            starQuestion_AddQuestionButton.Visible = false;

            starQuestion_EditQuestionButton.Enabled = true;
            starQuestion_EditQuestionButton.Visible = true;
        }//end function

        private void starQuestion_EditQuestionButton_Click(object sender, EventArgs e)
        {
            int questionOrder, NumberOfStars;
            string questionText;
            if (CheckStarQuestionInputFields())
            {
                questionOrder = Convert.ToInt32(starQuestion_QuestionOrderNumericUpDown.Value);
                questionText = starQuestion_TextRichTextBox.Text;
                NumberOfStars = Convert.ToInt32(starQuestion_NumberOfStarsNumericUpDown.Value);

                try
                {
                    using (SqlConnection conn = new SqlConnection(cn.ConnectionString))
                    {
                        SqlCommand cmd = new SqlCommand($@"
USE SurveyQuestionsConfigurator
 UPDATE Star_Questions
 SET QuestionOrder = {questionOrder}, QuestionText = '{questionText}', NumberOfStars = {NumberOfStars}
 WHERE QuestionID = {QuestionIdForEditing};
", conn);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Question edited successfully");
                    }
                }
                catch (SqlException ex)
                {

                    //2627 -> primary key violation
                    //2601 -> unique key violation
                    // ex.Number
                    if (ex.Number == 2627)
                    {
                        MessageBox.Show("This Question order is already in use\nTry using another one");
                    }
                    else
                    {
                        MessageBox.Show("SQL Error:\n" + ex);
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong:\n" + ex);
                }
            }
            else
            {
                MessageBox.Show("Question Text Can NOT be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }




        private void sliderQuestion_EditQuestionButton_Click(object sender, EventArgs e)
        {
            int questionOrder, questionStartValue, questionEndValue;
            string questionText, questionStartValueCaption, questionEndValueCaption;

            if (CheckSliderQuestionInputFields())
            {
                //assign variables

                questionOrder = Convert.ToInt32(sliderQuestion_QuestionOrderNumericUpDown.Value);
                questionText = (string)sliderQuestion_QuestionTextRichTextBox.Text;
                questionStartValueCaption = (string)sliderQuestion_StartValueCaptionTextBox.Text;
                questionEndValueCaption = (string)sliderQuestion_EndValueCaptionTextBox.Text;
                questionStartValue = Convert.ToInt32(sliderQuestion_StartValueNumericUpDown.Value);
                questionEndValue = Convert.ToInt32(sliderQuestion_EndValueNumericUpDown.Value);


                try
                {
                    using (SqlConnection conn = new SqlConnection(cn.ConnectionString))
                    {
                        SqlCommand cmd = new SqlCommand($@"
USE SurveyQuestionsConfigurator
 UPDATE Slider_Questions
 SET QuestionOrder = {questionOrder}, QuestionText = '{questionText}', QuestionStartValue = {questionStartValue}, QuestionEndValue = {questionEndValue}, QuestionStartValueCaption = '{questionStartValueCaption}', QuestionEndValueCaption = '{questionEndValueCaption}'
 WHERE QuestionID = {QuestionIdForEditing};
", conn);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Question edited successfully");
                    }
                }
                catch (SqlException ex)
                {

                    //2627 -> primary key violation
                    if (ex.Number == 2627)
                    {
                        MessageBox.Show("This Question order is already in use\nTry using another one");
                    }
                    else
                    {
                        MessageBox.Show("SQL Error:\n" + ex);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong:\n" + ex);
                }


            }

            else
            {
                MessageBox.Show("Question text, Start value Caption and End value caption can NOT be empty", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CheckSmileyQuestionInputFields()
        {
            if (!String.IsNullOrWhiteSpace(smileyQuestion_TextRichTextBox.Text)) //if Question text is not null or empty 
                return true;

            return false;
        }

        private bool CheckSliderQuestionInputFields()
        {
            if (!String.IsNullOrWhiteSpace(sliderQuestion_QuestionTextRichTextBox.Text)) //if Question text is not null or empty 
                if (!String.IsNullOrWhiteSpace(sliderQuestion_StartValueCaptionTextBox.Text))
                    if (!String.IsNullOrWhiteSpace(sliderQuestion_EndValueCaptionTextBox.Text))
                        return true;
            return false;
        }

        private bool CheckStarQuestionInputFields()
        {
            if (!String.IsNullOrWhiteSpace(starQuestion_TextRichTextBox.Text)) //if Question text is not null or empty 
                return true;

            return false;
        }
    }
}
