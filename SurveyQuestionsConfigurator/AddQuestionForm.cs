using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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
            this.Text = "Add A Question";
        }

        /*
         * Form constructor for "Editing Mode" or Editing a question
         */
        public AddQuestionForm(int questionId, string questionType/*, string questionText, int numberOfSmileyFaces*/)
        {
            InitializeComponent();
            this.Text = "Edit A Question";

            QuestionId = questionId; //set QuestionId to access it globally
            questionTypeComboBox.Enabled = false;

            if (questionType == SurveyQuestionsConfiguratorForm.QuestionType.SMILEY.ToString())
            {
                SelectedQuestionType = (int)SurveyQuestionsConfiguratorForm.QuestionType.SMILEY; // 0 
                InitializeEditingSmileyQuestion(questionId);
            }
            else if (questionType == SurveyQuestionsConfiguratorForm.QuestionType.SLIDER.ToString())
            {
                SelectedQuestionType = (int)SurveyQuestionsConfiguratorForm.QuestionType.SLIDER; // 1
                InitializeEditingSlideQuestion(questionId);
            }
            else if (questionType == SurveyQuestionsConfiguratorForm.QuestionType.STAR.ToString())
            {
                SelectedQuestionType = (int)SurveyQuestionsConfiguratorForm.QuestionType.STAR; // 2
                InitializeEditingStarQuestion(questionId);
            }
        }

        private void AddQuestionForm_Load(object sender, EventArgs e)
        {
            questionTypeComboBox.SelectedIndex = SelectedQuestionType;
        } // event end

        private void AddQuestionForm_Leave(object sender, EventArgs e)
        {
            conn.Close();
        }
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InitializeEditingSmileyQuestion(int questionId)
        {
            QuestionId = questionId;
            DataTable dt = new DataTable();

            ///
            /// Select wanted question from DB and fill input fields  with its data
            ///
            try
            {
                dt = CommDB.RetrieveSingleSmileyQuestion(QuestionId);
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        questionOrderNumericUpDown.Value = Convert.ToDecimal(row[1]);
                        questionTextRichTextBox.Text = (string)row[2];
                        genericNumericUpDown1.Value = Convert.ToDecimal(row[3]);
                    }
                }
                else
                {
                    MessageBox.Show("Error while retrieveing data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL Error while initializing smiley question form:\n" + ex.Message);
                Trace.TraceError("SQL Error while initializing smiley question form:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
            }

            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong while initializing smiley question form:\n" + ex);
                Trace.TraceError("Something went wrong while initializing smiley question form:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
            }
            finally
            {
                //conn.Close();
            }

            addQuestionButton.Enabled = false;
            addQuestionButton.Visible = false;

            editQuestionButton.Enabled = true;
            editQuestionButton.Visible = true;
        } //end function
        private void InitializeEditingSlideQuestion(int questionId)
        {
            QuestionId = questionId;
            DataTable dt = new DataTable();

            ///
            /// Select wanted question from DB and fill input fields  with its data
            ///
            try
            {
                dt = CommDB.RetrieveSingleSliderQuestion(QuestionId);
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        questionOrderNumericUpDown.Value = Convert.ToDecimal(row[1]);
                        questionTextRichTextBox.Text = (string)row[2];
                        genericNumericUpDown1.Value = Convert.ToDecimal(row[3]);
                        genericNumericUpDown2.Value = Convert.ToDecimal(row[4]);

                        genericTextBox1.Text = (string)row[5];
                        genericTextBox2.Text = (string)row[6];
                    }
                }
                else
                {
                    MessageBox.Show("Error while retrieveing data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL Error while initializing slider question form:\n" + ex.Message);
                Trace.TraceError("SQL Error while initializing  slider question form:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
            }

            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong while initializing  slider question form:\n" + ex);
                Trace.TraceError("Something went wrong while initializing  slider question form:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
            }
            finally
            {
                //conn.Close();
            }

            addQuestionButton.Enabled = false;
            addQuestionButton.Visible = false;

            editQuestionButton.Enabled = true;
            editQuestionButton.Visible = true;
        } //end function
        private void InitializeEditingStarQuestion(int questionId)
        {
            QuestionId = questionId;
            DataTable dt = new DataTable();

            ///
            /// Select wanted question from DB and fill input fields  with its data
            ///
            try
            {
                dt = CommDB.RetrieveSingleStarQuestion(QuestionId);
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        MessageBox.Show(row[3].ToString());
                        questionOrderNumericUpDown.Value = Convert.ToDecimal(row[1]);
                        questionTextRichTextBox.Text = (string)row[2];
                        genericNumericUpDown1.Value = Convert.ToDecimal(row[3]);
                    }
                }
                else
                {
                    MessageBox.Show("Error while retrieveing data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL Error while initializing star question form:\n" + ex.Message);
                Trace.TraceError("SQL Error while initializing star question form form:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
            }

            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong while initializing star question form form:\n" + ex);
                Trace.TraceError("Something went wrong while initializing star question form form:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
            }
            finally
            {
                //conn.Close();
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
                        if (genericNumericUpDown1.Value < genericNumericUpDown2.Value)
                            return true;
            return false;
        }
        private bool CheckStarQuestionInputFields()
        {
            if (!String.IsNullOrWhiteSpace(questionTextRichTextBox.Text)) //if Question text is not null or empty 
                return true;

            return false;
        }


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
            genericLabel1.Text = "Number Of Smiley Faces (2 - 5):";

            genericNumericUpDown1.Visible = true;
            genericNumericUpDown2.Visible = false;
            genericNumericUpDown1.Minimum = 2;
            genericNumericUpDown1.Maximum = 5;
            //genericNumericUpDown1.Value = genericNumericUpDown1.Minimum;

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
            //genericNumericUpDown1.Value = genericNumericUpDown1.Minimum;
            genericNumericUpDown2.Minimum = 2;
            genericNumericUpDown2.Maximum = 100;
            //genericNumericUpDown2.Value = genericNumericUpDown2.Minimum;

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
            //genericNumericUpDown1.Value = genericNumericUpDown1.Minimum;

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

        /// <summary>
        /// Add a smiley question through CommDB Class
        /// </summary>
        private void AddSmileyQuestion()
        {
            /*
             * Declare variables
             */
            int questionOrder, numberOfSmilyFaces;
            string questionText;

            if (CheckSmileyQuestionInputFields()) //if Question text is not null or empty 
            {
                ///
                /// Assign values to variables
                ///
                questionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                questionText = questionTextRichTextBox.Text;
                numberOfSmilyFaces = Convert.ToInt32(genericNumericUpDown1.Value);

                ///
                /// Try to insert a new question into "Smiley_Questions" table
                ///
                try
                {
                    int result = CommDB.AddSmileyQuestion(questionOrder, questionText, numberOfSmilyFaces);
                    switch (result)
                    {
                        case 1:
                            MessageBox.Show("Question inserted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearInputs();
                            break;
                        case 2:
                            MessageBox.Show("This Question order is already in use\nTry using another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case -1:
                            MessageBox.Show("Something wrong happened\nPlease try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong\nPlease try again\n" + ex.Message);
                    Trace.TraceError("Something went wrong:\nShort Message:\n" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
                }
            }
            else
            {
                MessageBox.Show("Question text cant be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Add a slider question through CommDB Class
        /// </summary>
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
                    int result = CommDB.AddSliderQuestion(questionOrder, questionText, questionStartValue,
                        questionEndValue, questionStartValueCaption, questionEndValueCaption);
                    switch (result)
                    {
                        case 1:
                            MessageBox.Show("Question inserted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearInputs();
                            break;
                        case 2:
                            MessageBox.Show("This Question order is already in use\nTry using another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case -1:
                            MessageBox.Show("Something wrong happened\nPlease try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong\nPlease try again\n" + ex.Message);
                    Trace.TraceError("Something went wrong:\nShort Message:\n" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
                }
            }
            else
            {
                MessageBox.Show("Question text\nStart Value caption\nEnd Value caption\nCan NOT be empty\n\nMake sure that Max Value is larger than Min Value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } // end of function

        /// <summary>
        /// Add a star question through CommDB Class
        /// </summary>
        private void AddStarQuestion()
        {
            /*
             * Declare variables
             */
            int questionOrder, numberOfStars;
            string questionText;

            if (CheckStarQuestionInputFields()) //if Question input fields are not null or empty 
            {
                questionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                questionText = questionTextRichTextBox.Text;
                numberOfStars = Convert.ToInt32(genericNumericUpDown1.Value);

                /*
                * Try to insert a new question into "Star_Questions" table
                */
                try
                {
                    int result = CommDB.AddStarQuestion(questionOrder, questionText, numberOfStars);
                    switch (result)
                    {
                        case 1:
                            MessageBox.Show("Question inserted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearInputs();
                            break;
                        case 2:
                            MessageBox.Show("This Question order is already in use\nTry using another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case -1:
                            MessageBox.Show("Something wrong happened\nPlease try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong\nPlease try again\n" + ex.Message);
                    Trace.TraceError("Something went wrong:\nShort Message:\n" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
                }
            }
            else
            {
                MessageBox.Show("Question text cant be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }// end of function

        /// <summary>
        /// Edit a smiley question through CommDB Class
        /// </summary>
        private void EditSmileyQuestion()
        {
            int questionOrder, numberOfSmilyFaces;
            string questionText;

            if (CheckSmileyQuestionInputFields())
            {
                questionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                questionText = questionTextRichTextBox.Text;
                numberOfSmilyFaces = Convert.ToInt32(genericNumericUpDown1.Value);

                /*
                * Try to Update a new question into "Smiley_Questions" table
                */
                try
                {
                    int result = CommDB.EditSmileyQuestion(QuestionId, questionOrder, questionText, numberOfSmilyFaces);
                    switch (result)
                    {
                        case 1:
                            MessageBox.Show("Question updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 2:
                            MessageBox.Show("This Question order is already in use\nTry using another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case -1:
                            MessageBox.Show("Something wrong happened\nPlease try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Something went wrong\nPlease try again\n" + ex.Message);
                    Trace.TraceError("Something went wrong:\nShort Message:\n" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
                }
            }
            else
            {
                MessageBox.Show("Question Text Can NOT be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }//end of function

        /// <summary>
        /// Edit a slider question through CommDB Class
        /// </summary>
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
                    int result = CommDB.EditSliderQuestion(QuestionId, questionOrder, questionText, questionStartValue, questionEndValue, questionStartValueCaption, questionEndValueCaption);
                    switch (result)
                    {
                        case 1:
                            MessageBox.Show("Question updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 2:
                            MessageBox.Show("This Question order is already in use\nTry using another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case -1:
                            MessageBox.Show("Something wrong happened\nPlease try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Something went wrong\nPlease try again\n" + ex.Message);
                    Trace.TraceError("Something went wrong:\nShort Message:\n" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
                }
            }
            else
            {
                MessageBox.Show("Question text\nStart Value caption\nEnd Value caption\nCan NOT be empty\n\nMake sure that Max Value is larger than Min Value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } //end of function

        /// <summary>
        /// Edit a star question through CommDB Class
        /// </summary>
        private void EditStarQuestion()
        {
            int questionOrder, numberOfStars;
            string questionText;
            if (CheckStarQuestionInputFields())
            {
                questionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                questionText = questionTextRichTextBox.Text;
                numberOfStars = Convert.ToInt32(genericNumericUpDown1.Value);

                /*
                * Try to Update a new question into "Star_Questions" table
                */
                try
                {
                    int result = CommDB.EditStarQuestion(QuestionId, questionOrder, questionText, numberOfStars);
                    switch (result)
                    {
                        case 1:
                            MessageBox.Show("Question updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 2:
                            MessageBox.Show("This Question order is already in use\nTry using another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case -1:
                            MessageBox.Show("Something wrong happened\nPlease try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Something went wrong\nPlease try again\n" + ex.Message);
                    Trace.TraceError("Something went wrong:\nShort Message:\n" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
                }
            }
            else
            {
                MessageBox.Show("Question Text Can NOT be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } //end of function

        private void ClearInputs()
        {
            questionOrderNumericUpDown.Value = questionOrderNumericUpDown.Minimum;
            questionTextRichTextBox.Clear();
            genericNumericUpDown1.Value = genericNumericUpDown1.Minimum;
            genericNumericUpDown2.Value = genericNumericUpDown2.Minimum;
            genericTextBox1.Clear();
            genericTextBox2.Clear();
        }

    }
}
