using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SurveyQuestionsConfigurator
{
    public partial class AddQuestionForm : Form
    {
        /* 
         * Global Variables
         * Access them anywhere
         * */
        public int QuestionId { get; set; } // create global Question ID property
        private ConnectionStringSettings cn = ConfigurationManager.ConnectionStrings[0]; //get connection string information from App.config
        private SqlConnection conn = null; // Create SqlConnection object to connect to DB
        private int SelectedQuestionType = 0;

        public AddQuestionForm()
        {
            InitializeComponent();
        }

        /*
         * Form constructor for "Editing Mode" or Editing a question
         */
        public AddQuestionForm(int activeTab, int questionId, string questionType/*, string questionText, int numberOfSmileyFaces*/)
        {
            InitializeComponent();

            QuestionId = questionId; //set QuestionId to access it globally
            questionTypeComboBox.Enabled = false;


            if (questionType == Form1.QuestionType.SMILEY.ToString())
            {
                SelectedQuestionType = 0;
                InitializeEditingSmileyQuestion(questionId);
            }
            else if (questionType == Form1.QuestionType.SLIDER.ToString())
            {
                SelectedQuestionType = 1;
                InitializeEditingSlideQuestion(questionId);
            }
            else if (questionType == Form1.QuestionType.STAR.ToString())
            {
                SelectedQuestionType = 2;
                InitializeEditingStarQuestion(questionId);
            }
        }


        /*
         * Smiley question logic
         */
        private void smileyQuestionAddQuestionButton_Click(object sender, EventArgs e)
        {

            /*
             * Declare variables
             */
            int questionOrder, NumberOfSmilyFaces;
            string QuestionText;

            if (!String.IsNullOrWhiteSpace(questionTextRichTextBox.Text)) //if Question text is not null or empty 
            {
                /*
                * Assign values to variables
                */
                questionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                QuestionText = questionTextRichTextBox.Text;
                NumberOfSmilyFaces = Convert.ToInt32(genericNumericUpDown1.Value);

                /*
                * Try to insert a new question into "Smiley_Questions" table
                */
                try
                {
                    conn = new SqlConnection(cn.ConnectionString);
                    SqlCommand cmd = new SqlCommand($@"
USE {cn.Name}
INSERT INTO Smiley_Questions
(QuestionOrder, QuestionText, NumberOfSmileyFaces)
VALUES
({questionOrder},'{QuestionText}',{NumberOfSmilyFaces})", conn);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Question inserted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (SqlException ex)
                {

                    //2627 -> primary key violation
                    //2601 -> unique key violation
                    // ex.Number
                    if (ex.Number == 2601)
                    {
                        MessageBox.Show("This Question order is already in use\nTry using another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("SQL Error:\n" + ex.Message);
                        Trace.TraceError("SQL Error:\n" + ex.Message + "\n");

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong:\n" + ex);
                    Trace.TraceError("Something went wrong:\n" + ex.Message + "\n");
                }
                finally
                {
                    conn.Close();
                }
            }
            else
            {
                MessageBox.Show("Question text cant be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }// end of event

        private void sliderQuestionAddQuestionButton_Click(object sender, EventArgs e)
        {
            /*
             * Declare variables
             */
            int questionOrder, questionStartValue, questionEndValue;
            string questionText, questionStartValueCaption, questionEndValueCaption;


            if (CheckSliderQuestionInputFields()) //if Question input fields are not null or empty 
            {

                questionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                questionText = (string)questionTextRichTextBox.Text;

                questionStartValueCaption = (string)genericTextBox1.Text;
                questionEndValueCaption = (string)genericTextBox2.Text;

                questionStartValue = Convert.ToInt32(genericNumericUpDown1.Value);
                questionEndValue = Convert.ToInt32(genericNumericUpDown2.Value);

                /*
                * Try to insert a new question into "Slider_Questions" table
                */
                try
                {
                    conn = new SqlConnection(cn.ConnectionString);
                    SqlCommand cmd = new SqlCommand($@"
USE {cn.Name}
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
                        MessageBox.Show("This Question order is already in use\nTry using another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("SQL Error:\n" + ex.Message);
                        Trace.TraceError("SQL Error:\n" + ex.Message + "\n");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong:\n" + ex);
                    Trace.TraceError("Something went wrong:\n" + ex.Message + "\n");
                }
                finally
                {
                    conn.Close();
                }
            }
            else
            {
                MessageBox.Show("Question text\nStart Value caption\nEnd Value caption\ncan NOT be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } // end of event

        private void starQuestionAddQuestionButton_Click(object sender, EventArgs e)
        {
            /*
             * Declare variables
             */
            int questionOrder, NumberOfStars;
            string questionText;

            if (CheckStarQuestionInputFields()) //if Question input fields are not null or empty 
            {
                questionOrder = Convert.ToInt32(starQuestion_QuestionOrderNumericUpDown.Value);
                questionText = starQuestion_TextRichTextBox.Text;
                NumberOfStars = Convert.ToInt32(starQuestion_NumberOfStarsNumericUpDown.Value);

                /*
                * Try to insert a new question into "Star_Questions" table
                */
                try
                {
                    conn = new SqlConnection(cn.ConnectionString);
                    SqlCommand cmd = new SqlCommand($@"
USE {cn.Name}
INSERT INTO Star_Questions
(QuestionOrder, QuestionText, NumberOfStars)
values
({questionOrder}, '{questionText}', {NumberOfStars})", conn);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Question inserted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (SqlException ex)
                {

                    //2627 -> primary key violation
                    //2601 -> unique key violation
                    // ex.Number
                    if (ex.Number == 2627)
                    {
                        MessageBox.Show("This Question order is already in use\nTry using another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("SQL Error:\n" + ex.Message);
                        Trace.TraceError("SQL Error:\n" + ex.Message + "\n");
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong:\n" + ex);
                    Trace.TraceError("Something went wrong:\n" + ex.Message + "\n");
                }
                finally
                {
                    conn.Close();
                }
            }
            else
            {
                MessageBox.Show("Question text cant be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }// end of event

        private void AddQuestionForm_Leave(object sender, EventArgs e)
        {
            conn.Close();
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

            if (CheckSmileyQuestionInputFields())
            {
                questionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                QuestionText = questionTextRichTextBox.Text;
                NumberOfSmilyFaces = Convert.ToInt32(genericNumericUpDown1.Value);

                /*
                * Try to Update a new question into "Smiley_Questions" table
                */
                try
                {
                    using (SqlConnection conn = new SqlConnection(cn.ConnectionString))
                    {
                        SqlCommand cmd = new SqlCommand($@"
USE {cn.Name}
 UPDATE Smiley_Questions
 SET QuestionOrder = {questionOrder}, QuestionText = '{QuestionText}', NumberOfSmileyFaces ={NumberOfSmilyFaces}
 WHERE QuestionID = {QuestionId};
", conn);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Question updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("SQL Error:\n" + ex.Message);
                    Trace.TraceError("SQL Error:\n" + ex.Message + "\n");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong:\n" + ex);
                    Trace.TraceError("Something went wrong:\n" + ex.Message + "\n");
                }
            }
            else
            {
                MessageBox.Show("Question Text Can NOT be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void starQuestion_EditQuestionButton_Click(object sender, EventArgs e)
        {
            int questionOrder, NumberOfStars;
            string questionText;
            if (CheckStarQuestionInputFields())
            {
                questionOrder = Convert.ToInt32(starQuestion_QuestionOrderNumericUpDown.Value);
                questionText = starQuestion_TextRichTextBox.Text;
                NumberOfStars = Convert.ToInt32(starQuestion_NumberOfStarsNumericUpDown.Value);

                /*
                * Try to Update a new question into "Star_Questions" table
                */
                try
                {
                    using (SqlConnection conn = new SqlConnection(cn.ConnectionString))
                    {
                        SqlCommand cmd = new SqlCommand($@"
USE {cn.Name}
 UPDATE Star_Questions
 SET QuestionOrder = {questionOrder}, QuestionText = '{questionText}', NumberOfStars = {NumberOfStars}
 WHERE QuestionID = {QuestionId};
", conn);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Question updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    }
                }
                catch (SqlException ex)
                {
                    //2627 -> primary key violation
                    //2601 -> unique key violation
                    // ex.Number
                    if (ex.Number == 2627)
                    {
                        MessageBox.Show("This Question order is already in use\nTry using another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("SQL Error:\n" + ex.Message);
                        Trace.TraceError("SQL Error:\n" + ex.Message + "\n");


                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong:\n" + ex);
                    Trace.TraceError("Something went wrong:\n" + ex.Message + "\n");
                }
            }
            else
            {
                MessageBox.Show("Question Text Can NOT be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } //end of function

        private void sliderQuestion_EditQuestionButton_Click(object sender, EventArgs e)
        {
            int questionOrder, questionStartValue, questionEndValue;
            string questionText, questionStartValueCaption, questionEndValueCaption;

            if (CheckSliderQuestionInputFields())
            {
                //assign variables
                questionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                questionText = (string)questionTextRichTextBox.Text;
                questionStartValueCaption = (string)genericTextBox1.Text;
                questionEndValueCaption = (string)genericTextBox2.Text;
                questionStartValue = Convert.ToInt32(genericNumericUpDown1.Value);
                questionEndValue = Convert.ToInt32(genericNumericUpDown2.Value);

                /*
                * Try to Update a new question into "Slider_Questions" table
                */
                try
                {
                    using (SqlConnection conn = new SqlConnection(cn.ConnectionString))
                    {
                        SqlCommand cmd = new SqlCommand($@"
USE {cn.Name}
 UPDATE Slider_Questions
 SET QuestionOrder = {questionOrder}, QuestionText = '{questionText}', QuestionStartValue = {questionStartValue}, QuestionEndValue = {questionEndValue}, QuestionStartValueCaption = '{questionStartValueCaption}', QuestionEndValueCaption = '{questionEndValueCaption}'
 WHERE QuestionID = {QuestionId};
", conn);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Question updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (SqlException ex)
                {
                    //2627 -> primary key violation
                    if (ex.Number == 2627)
                    {
                        MessageBox.Show("This Question order is already in use\nTry using another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("SQL Error:\n" + ex.Message);
                        Trace.TraceError("SQL Error:\n" + ex.Message + "\n");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong:\n" + ex);
                    Trace.TraceError("Something went wrong:\n" + ex.Message + "\n");
                }
            }

            else
            {
                MessageBox.Show("Question text, Start value Caption and End value caption can NOT be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } //end of function

        private void InitializeEditingSmileyQuestion(int questionId)
        {
            SqlDataReader reader = null;

            /*
             * Select wanted question from DB and fill input fields  with its data
             */
            try
            {
                conn = new SqlConnection(cn.ConnectionString);
                SqlCommand cmd = new SqlCommand($"select * from Smiley_Questions where QuestionID = {questionId}", conn);
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    questionOrderNumericUpDown.Value = Convert.ToDecimal(reader[1]);
                    questionTextRichTextBox.Text = (string)reader[2];
                    genericNumericUpDown1.Value = Convert.ToDecimal(reader[3]);
                }
            }
            catch (SqlException ex)
            {

                //2627 -> primary key violation
                //2601 -> unique key violation
                if (ex.Number == 2601)
                {
                    MessageBox.Show("This Question order is already in use\nTry using another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("SQL Error:\n" + ex.Message);
                    Trace.TraceError("SQL Error:\n" + ex.Message + "\n");
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong:\n" + ex);
                Trace.TraceError("Something went wrong:\n" + ex.Message + "\n");
            }
            finally
            {
                conn.Close();
            }

            addQuestionButton.Enabled = false;
            addQuestionButton.Visible = false;

            editQuestionButton.Enabled = true;
            editQuestionButton.Visible = true;
        } //end function

        private void InitializeEditingSlideQuestion(int questionId)
        {
            SqlDataReader reader = null;

            /*
             * Select wanted question from DB and fill input fields  with its data
             */
            try
            {
                conn = new SqlConnection(cn.ConnectionString);
                SqlCommand cmd = new SqlCommand($"select * from Slider_Questions where QuestionID = {questionId}", conn);
                conn.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    questionOrderNumericUpDown.Value = Convert.ToDecimal(reader[1]);
                    questionTextRichTextBox.Text = (string)reader[2];
                    genericNumericUpDown1.Value = Convert.ToDecimal(reader[3]);
                    genericNumericUpDown2.Value = Convert.ToDecimal(reader[4]);

                    genericTextBox1.Text = (string)reader[5];
                    genericTextBox2.Text = (string)reader[6];
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL Error:\n" + ex.Message);
                Trace.TraceError("SQL Error:\n" + ex.Message + "\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong:\n" + ex.Message);
                Trace.TraceError("Something went wrong:\n" + ex.Message + "\n");
            }
            finally
            {
                conn.Close();
            }

            addQuestionButton.Enabled = false;
            addQuestionButton.Visible = false;

            editQuestionButton.Enabled = true;
            editQuestionButton.Visible = true;
        } //end function

        private void InitializeEditingStarQuestion(int questionId)
        {
            SqlDataReader reader = null;

            /*
             * Select wanted question from DB and fill input fields  with its data
             */
            try
            {
                conn = new SqlConnection(cn.ConnectionString);
                SqlCommand cmd = new SqlCommand($@"select * from Star_Questions where QuestionID = {questionId}", conn);
                conn.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    questionOrderNumericUpDown.Value = Convert.ToDecimal(reader[1]);
                    questionTextRichTextBox.Text = (string)reader[2];
                    genericNumericUpDown1.Value = Convert.ToDecimal(reader[3].ToString());
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL Error:\n" + ex.Message);
                Trace.TraceError("SQL Error:\n" + ex.Message + "\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong:\n" + ex.Message);
                Trace.TraceError("Something went wrong:\n" + ex.Message + "\n");
            }
            finally
            {
                conn.Close();
            }

            addQuestionButton.Enabled = false;
            addQuestionButton.Visible = false;

            editQuestionButton.Enabled = true;
            editQuestionButton.Visible = true;
        }//end function





        private bool CheckSmileyQuestionInputFields()
        {
            if (!String.IsNullOrWhiteSpace(questionTextRichTextBox.Text)) //if Question text is not null or empty 
                return true;

            return false;
        }

        private bool CheckSliderQuestionInputFields()
        {
            if (!String.IsNullOrWhiteSpace(questionTextRichTextBox.Text)) //if Question text is not null or empty 
                if (!String.IsNullOrWhiteSpace(genericTextBox1.Text))
                    if (!String.IsNullOrWhiteSpace(genericTextBox2.Text))
                        return true;
            return false;
        }

        private bool CheckStarQuestionInputFields()
        {
            if (!String.IsNullOrWhiteSpace(questionTextRichTextBox.Text)) //if Question text is not null or empty 
                return true;

            return false;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddQuestionForm_Load(object sender, EventArgs e)
        {
            questionTypeComboBox.SelectedIndex = SelectedQuestionType;
        } // event end

        private void questionTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (questionTypeComboBox.SelectedIndex == 0)
            {
                InitializeSmileyQuestionForm();
            }
            else if (questionTypeComboBox.SelectedIndex == 1)
            {
                InitializeSliderQuestionForm();
            }
            else if (questionTypeComboBox.SelectedIndex == 2)
            {
                InitializeStarQuestionForm();
            }
        }// event end

        private void InitializeSmileyQuestionForm()
        {
            genericLabel1.Visible = true;
            genericLabel2.Visible = false;
            genericLabel3.Visible = false;
            genericLabel4.Visible = false;
            genericLabel1.Text = "Number Of Smiley Faces (2 -5):";

            genericNumericUpDown1.Visible = true;
            genericNumericUpDown2.Visible = false;
            genericNumericUpDown1.Minimum = 2;
            genericNumericUpDown1.Maximum = 5;

            genericTextBox1.Visible = false;
            genericTextBox2.Visible = false;
        } // func. end
        private void InitializeSliderQuestionForm()
        {
            genericLabel1.Visible = true;
            genericLabel2.Visible = true;
            genericLabel3.Visible = true;
            genericLabel4.Visible = true;
            genericLabel1.Text = "Start Value:";
            genericLabel2.Text = "End Value:";
            genericLabel3.Text = "Start Value Caption:";
            genericLabel4.Text = "End Value Caption:";

            genericNumericUpDown1.Visible = true;
            genericNumericUpDown2.Visible = true;
            genericNumericUpDown1.Minimum = 1;
            genericNumericUpDown1.Maximum = 99;
            genericNumericUpDown2.Minimum = 2;
            genericNumericUpDown2.Maximum = 100;

            genericTextBox1.Visible = true;
            genericTextBox2.Visible = true;
        } //func. end
        private void InitializeStarQuestionForm()
        {
            genericLabel1.Visible = true;
            genericLabel2.Visible = false;
            genericLabel3.Visible = false;
            genericLabel4.Visible = false;
            genericLabel1.Text = "Number Of Stars (1 - 10):";

            genericNumericUpDown1.Visible = true;
            genericNumericUpDown1.Enabled = true;
            genericNumericUpDown2.Visible = false;
            genericNumericUpDown1.Minimum = 1;
            genericNumericUpDown1.Maximum = 10;

            genericTextBox1.Visible = false;
            genericTextBox2.Visible = false;
        } //func. end

        private void addQuestionButton_Click(object sender, EventArgs e)
        {
            if (questionTypeComboBox.SelectedIndex == 0)
            {
                AddSmileyQuestion();
            }
            else if (questionTypeComboBox.SelectedIndex == 1)
            {
                AddSliderQuestion();
            }
            else if (questionTypeComboBox.SelectedIndex == 2)
            {
                AddStarQuestion();
            }
        }// end of function

        private void editQuestionButton_Click(object sender, EventArgs e)
        {
            if (questionTypeComboBox.SelectedIndex == 0)
            {
                EditSmileyQuestion();
            }
            else if (questionTypeComboBox.SelectedIndex == 1)
            {
                EditSliderQuestion();
            }
            else if (questionTypeComboBox.SelectedIndex == 2)
            {
                EditStarQuestion();
            }
        }// end of function

        private void AddSmileyQuestion()
        {
            /*
             * Declare variables
             */
            int questionOrder, NumberOfSmilyFaces;
            string QuestionText;

            if (CheckSmileyQuestionInputFields()) //if Question text is not null or empty 
            {
                /*
                * Assign values to variables
                */
                questionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                QuestionText = questionTextRichTextBox.Text;
                NumberOfSmilyFaces = Convert.ToInt32(genericNumericUpDown1.Value);

                /*
                * Try to insert a new question into "Smiley_Questions" table
                */
                try
                {
                    conn = new SqlConnection(cn.ConnectionString);
                    SqlCommand cmd = new SqlCommand($@"
USE {cn.Name}
INSERT INTO Smiley_Questions
(QuestionOrder, QuestionText, NumberOfSmileyFaces)
VALUES
({questionOrder},'{QuestionText}',{NumberOfSmilyFaces})", conn);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Question inserted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (SqlException ex)
                {
                    //2627 -> primary key violation
                    //2601 -> unique key violation
                    // ex.Number
                    if (ex.Number == 2601)
                    {
                        MessageBox.Show("This Question order is already in use\nTry using another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("SQL Error:\n" + ex.Message);
                        Trace.TraceError("SQL Error:\n" + ex.Message + "\n"); //write error to log file
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong:\n" + ex.Message);
                    Trace.TraceError("Something went wrong:\n" + ex.Message + "\n"); //write error to log file
                }
                finally
                {
                    conn.Close();
                }
            }
            else
            {
                MessageBox.Show("Question text cant be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AddSliderQuestion()
        {
            /*
             * Declare variables
             */
            int questionOrder, questionStartValue, questionEndValue;
            string questionText, questionStartValueCaption, questionEndValueCaption;


            if (CheckSliderQuestionInputFields()) //if Question input fields are not null or empty 
            {

                questionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                questionText = (string)questionTextRichTextBox.Text;

                questionStartValueCaption = (string)genericTextBox1.Text;
                questionEndValueCaption = (string)genericTextBox2.Text;

                questionStartValue = Convert.ToInt32(genericNumericUpDown1.Value);
                questionEndValue = Convert.ToInt32(genericNumericUpDown2.Value);

                /*
                * Try to insert a new question into "Slider_Questions" table
                */
                try
                {
                    conn = new SqlConnection(cn.ConnectionString);
                    SqlCommand cmd = new SqlCommand($@"
USE {cn.Name}
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
                        MessageBox.Show("This Question order is already in use\nTry using another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("SQL Error:\n" + ex.Message);
                        Trace.TraceError("SQL Error:\n" + ex.Message + "\n");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong:\n" + ex);
                    Trace.TraceError("Something went wrong:\n" + ex.Message + "\n");
                }
                finally
                {
                    conn.Close();
                }
            }
            else
            {
                MessageBox.Show("Question text\nStart Value caption\nEnd Value caption\ncan NOT be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } // end of function
        private void AddStarQuestion()
        {
            /*
             * Declare variables
             */
            int questionOrder, NumberOfStars;
            string questionText;

            if (CheckStarQuestionInputFields()) //if Question input fields are not null or empty 
            {
                questionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                questionText = questionTextRichTextBox.Text;
                NumberOfStars = Convert.ToInt32(genericNumericUpDown1.Value);

                /*
                * Try to insert a new question into "Star_Questions" table
                */
                try
                {
                    conn = new SqlConnection(cn.ConnectionString);
                    SqlCommand cmd = new SqlCommand($@"
USE {cn.Name}
INSERT INTO Star_Questions
(QuestionOrder, QuestionText, NumberOfStars)
values
({questionOrder}, '{questionText}', {NumberOfStars})", conn);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Question inserted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (SqlException ex)
                {

                    //2627 -> primary key violation
                    //2601 -> unique key violation
                    // ex.Number
                    if (ex.Number == 2627)
                    {
                        MessageBox.Show("This Question order is already in use\nTry using another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("SQL Error:\n" + ex.Message);
                        Trace.TraceError("SQL Error:\n" + ex.Message + "\n");
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong:\n" + ex);
                    Trace.TraceError("Something went wrong:\n" + ex.Message + "\n");
                }
                finally
                {
                    conn.Close();
                }
            }
            else
            {
                MessageBox.Show("Question text cant be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }// end of function

        private void EditSmileyQuestion()
        {
            int questionOrder, NumberOfSmilyFaces;
            string QuestionText;

            if (CheckSmileyQuestionInputFields())
            {
                questionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                QuestionText = questionTextRichTextBox.Text;
                NumberOfSmilyFaces = Convert.ToInt32(genericNumericUpDown1.Value);

                /*
                * Try to Update a new question into "Smiley_Questions" table
                */
                try
                {
                    using (SqlConnection conn = new SqlConnection(cn.ConnectionString))
                    {
                        SqlCommand cmd = new SqlCommand($@"
USE {cn.Name}
 UPDATE Smiley_Questions
 SET QuestionOrder = {questionOrder}, QuestionText = '{QuestionText}', NumberOfSmileyFaces ={NumberOfSmilyFaces}
 WHERE QuestionID = {QuestionId};
", conn);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Question updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("SQL Error:\n" + ex.Message);
                    Trace.TraceError("SQL Error:\n" + ex.Message + "\n");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong:\n" + ex);
                    Trace.TraceError("Something went wrong:\n" + ex.Message + "\n");
                }
            }
            else
            {
                MessageBox.Show("Question Text Can NOT be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }//end of function

        private void EditSliderQuestion()
        {
            int questionOrder, questionStartValue, questionEndValue;
            string questionText, questionStartValueCaption, questionEndValueCaption;

            if (CheckSliderQuestionInputFields())
            {
                //assign variables
                questionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                questionText = (string)questionTextRichTextBox.Text;
                questionStartValueCaption = (string)genericTextBox1.Text;
                questionEndValueCaption = (string)genericTextBox2.Text;
                questionStartValue = Convert.ToInt32(genericNumericUpDown1.Value);
                questionEndValue = Convert.ToInt32(genericNumericUpDown2.Value);

                /*
                * Try to Update a new question into "Slider_Questions" table
                */
                try
                {
                    using (SqlConnection conn = new SqlConnection(cn.ConnectionString))
                    {
                        SqlCommand cmd = new SqlCommand($@"
USE {cn.Name}
 UPDATE Slider_Questions
 SET QuestionOrder = {questionOrder}, QuestionText = '{questionText}', QuestionStartValue = {questionStartValue}, QuestionEndValue = {questionEndValue}, QuestionStartValueCaption = '{questionStartValueCaption}', QuestionEndValueCaption = '{questionEndValueCaption}'
 WHERE QuestionID = {QuestionId};
", conn);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Question updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (SqlException ex)
                {
                    //2627 -> primary key violation
                    if (ex.Number == 2627)
                    {
                        MessageBox.Show("This Question order is already in use\nTry using another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("SQL Error:\n" + ex.Message);
                        Trace.TraceError("SQL Error:\n" + ex.Message + "\n");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong:\n" + ex);
                    Trace.TraceError("Something went wrong:\n" + ex.Message + "\n");
                }
            }

            else
            {
                MessageBox.Show("Question text, Start value Caption and End value caption can NOT be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } //end of function

        private void EditStarQuestion()
        {
            int questionOrder, NumberOfStars;
            string questionText;
            if (CheckStarQuestionInputFields())
            {
                questionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                questionText = questionTextRichTextBox.Text;
                NumberOfStars = Convert.ToInt32(genericNumericUpDown1.Value);

                /*
                * Try to Update a new question into "Star_Questions" table
                */
                try
                {
                    using (SqlConnection conn = new SqlConnection(cn.ConnectionString))
                    {
                        SqlCommand cmd = new SqlCommand($@"
USE {cn.Name}
 UPDATE Star_Questions
 SET QuestionOrder = {questionOrder}, QuestionText = '{questionText}', NumberOfStars = {NumberOfStars}
 WHERE QuestionID = {QuestionId};
", conn);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Question updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    }
                }
                catch (SqlException ex)
                {
                    //2627 -> primary key violation
                    //2601 -> unique key violation
                    // ex.Number
                    if (ex.Number == 2627)
                    {
                        MessageBox.Show("This Question order is already in use\nTry using another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("SQL Error:\n" + ex.Message);
                        Trace.TraceError("SQL Error:\n" + ex.Message + "\n");


                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong:\n" + ex);
                    Trace.TraceError("Something went wrong:\n" + ex.Message + "\n");
                }
            }
            else
            {
                MessageBox.Show("Question Text Can NOT be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } //end of function


    }
}
