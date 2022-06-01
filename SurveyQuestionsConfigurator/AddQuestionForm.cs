using SurveyQuestionsConfigurator.CommonLayer;
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
        /// 
        /// Global Variables
        /// Access them anywhere
        ///
        public int QuestionId { get; set; } /// create global Question ID property
        private string FormState { get; set; } /// Decide whether "OK" Form button is used to either ADD or EDIT a question
        private string QuestionType { get; set; }
        private int SelectedQuestionType { get; set; }

        public enum FormStateType
        {
            ADD,
            EDIT
        }

        ///
        /// Form constructor for "Adding Mode" or Editing a question
        ///
        public AddQuestionForm()
        {
            InitializeComponent();
            this.Text = "Add A Question";
            FormState = FormStateType.ADD.ToString();
        }

        ///
        /// Form constructor for "Editing Mode" or Editing a question
        ///
        public AddQuestionForm(int questionId, string questionType)
        {
            InitializeComponent();
            this.Text = "Edit A Question";

            FormState = FormStateType.EDIT.ToString();

            QuestionId = questionId; //set QuestionId to access it globally
            QuestionType = questionType;
            questionTypeComboBox.Enabled = false;

            if (QuestionType.ToUpper() == SurveyQuestionsConfiguratorForm.QuestionType.SMILEY.ToString().ToUpper())
            {
                SelectedQuestionType = (int)SurveyQuestionsConfiguratorForm.QuestionType.SMILEY; // 0 
                InitializeEditingSmileyQuestion(QuestionId);
            }
            else if (QuestionType.ToUpper() == SurveyQuestionsConfiguratorForm.QuestionType.SLIDER.ToString().ToUpper())
            {
                SelectedQuestionType = (int)SurveyQuestionsConfiguratorForm.QuestionType.SLIDER; // 1
                InitializeEditingSlideQuestion(QuestionId);
            }
            else if (QuestionType.ToUpper() == SurveyQuestionsConfiguratorForm.QuestionType.STAR.ToString().ToUpper())
            {
                SelectedQuestionType = (int)SurveyQuestionsConfiguratorForm.QuestionType.STAR; // 2
                InitializeEditingStarQuestion(QuestionId);
            }
        }

        private void AddQuestionForm_Load(object sender, EventArgs e)
        {
            try
            {
                questionTypeComboBox.SelectedIndex = SelectedQuestionType;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n");

                Helper.LogError(ex);
            }
        } // event end

        private void AddQuestionForm_Leave(object sender, EventArgs e)
        {
            //conn.Close();
        } // event end
        private void cancelButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n");
                Helper.LogError(ex);
            }
        }

        private void InitializeEditingSmileyQuestion(int questionId)
        {
            ///
            /// Select wanted question from DB and fill input fields  with its data
            ///
            try
            {
                QuestionId = questionId;
                DataTable dt = new DataTable();
                dt = CommDB.RetrieveSingleSmileyQuestion(QuestionId);
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        questionOrderNumericUpDown.Value = Convert.ToDecimal(row[0]);
                        questionTextRichTextBox.Text = (string)row[1];
                        genericNumericUpDown1.Value = Convert.ToDecimal(row[2]);
                    }
                }
                else
                {
                    MessageBox.Show("Error while retrieveing data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL Error while initializing smiley question form:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Helper.LogError(ex); //write error to log file
            }

            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong while initializing smiley question form:\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Helper.LogError(ex); //write error to log file
            }
        } //end function
        private void InitializeEditingSlideQuestion(int questionId)
        {
            ///
            /// Select wanted question from DB and fill input fields  with its data
            ///
            try
            {
                QuestionId = questionId;
                DataTable dt = new DataTable();
                dt = CommDB.RetrieveSingleSliderQuestion(QuestionId);
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        questionOrderNumericUpDown.Value = Convert.ToDecimal(row[0]);
                        questionTextRichTextBox.Text = (string)row[1];
                        genericNumericUpDown1.Value = Convert.ToDecimal(row[2]);
                        genericNumericUpDown2.Value = Convert.ToDecimal(row[3]);

                        genericTextBox1.Text = (string)row[4];
                        genericTextBox2.Text = (string)row[5];
                    }
                }
                else
                {
                    MessageBox.Show("Error while retrieveing data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL Error while initializing slider question form:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Helper.LogError(ex); //write error to log file
            }

            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong while initializing slider question form:\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Helper.LogError(ex); //write error to log file
            }
        } //end function
        private void InitializeEditingStarQuestion(int questionId)
        {
            ///
            /// Select wanted question from DB and fill input fields  with its data
            ///
            try
            {
                QuestionId = questionId;
                DataTable dt = new DataTable();
                dt = CommDB.RetrieveSingleStarQuestion(QuestionId);
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        questionOrderNumericUpDown.Value = Convert.ToDecimal(row[0]);
                        questionTextRichTextBox.Text = (string)row[1];
                        genericNumericUpDown1.Value = Convert.ToDecimal(row[2]);
                    }
                }
                else
                {
                    MessageBox.Show("Error while retrieveing data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL Error while initializing star question form:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Helper.LogError(ex); //write error to log file
            }

            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong while initializing star question form form:\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Helper.LogError(ex); //write error to log file
            }
        }//end function

        private bool CheckSmileyQuestionInputFields()
        {
            try
            {
                if (BusinessLogic.CheckSmileyQuestionInputFields(questionTextRichTextBox)) //if Question text is not null or empty 
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n");
                Helper.LogError(ex);
                return false;
            }
        }
        private bool CheckSliderQuestionInputFields()
        {
            try
            {
                if (BusinessLogic.CheckSliderQuestionInputFields(questionTextRichTextBox, genericTextBox1, genericTextBox1,
                   genericNumericUpDown1, genericNumericUpDown2))  //if Question text is not null or empty 
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n");
                Helper.LogError(ex);
                return false;
            }
        }
        private bool CheckStarQuestionInputFields()
        {
            try
            {
                if (BusinessLogic.CheckStarQuestionInputFields(questionTextRichTextBox))  //if Question text is not null or empty 
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n");
                Helper.LogError(ex);
                return false;
            }
        }


        private void questionTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n");
                Helper.LogError(ex);
            }
        }// event end
        private void InitializeSmileyQuestionForm()
        {
            try
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n");
                Helper.LogError(ex);
            }
        } // func. end
        private void InitializeSliderQuestionForm()
        {
            try
            {
                genericLabel1.Visible = true;
                genericLabel2.Visible = true;
                genericLabel3.Visible = true;
                genericLabel4.Visible = true;
                genericLabel1.Text = "Slider start Value (1 - 99):";
                genericLabel2.Text = "Slider end Value (2 - 100):";
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n");
                Helper.LogError(ex);
            }
        } //func. end
        private void InitializeStarQuestionForm()
        {
            try
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n");
                Helper.LogError(ex);
            }
        } //func. end

        private void addQuestionButton_Click(object sender, EventArgs e)
        {
            try
            {
                bool success = false;
                if (FormState.ToString().ToUpper() == FormStateType.ADD.ToString().ToUpper())
                {
                    try
                    {
                        if (questionTypeComboBox.SelectedIndex == 0)
                        {
                            success = AddSmileyQuestion();
                        }
                        else if (questionTypeComboBox.SelectedIndex == 1)
                        {
                            success = AddSliderQuestion();
                        }
                        else if (questionTypeComboBox.SelectedIndex == 2)
                        {
                            success = AddStarQuestion();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something wrong happened, please try again\n");
                        Helper.LogError(ex);
                    }
                }
                else if (FormState.ToString().ToUpper() == FormStateType.EDIT.ToString().ToUpper())
                {
                    try
                    {
                        if (questionTypeComboBox.SelectedIndex == 0)
                        {
                            success = EditSmileyQuestion();
                        }
                        else if (questionTypeComboBox.SelectedIndex == 1)
                        {
                            success = EditSliderQuestion();
                        }
                        else if (questionTypeComboBox.SelectedIndex == 2)
                        {
                            success = EditStarQuestion();
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Helper.LogError(ex);
                    }
                }

                if (success)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Helper.LogError(ex);
            }

        }// end of function

        /// <summary>
        /// Add a smiley question through CommDB Class
        /// </summary>
        private bool AddSmileyQuestion()
        {
            try
            {
                ///
                /// Declare variables
                ///
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
                                return true;
                                break;
                            case 2:
                                MessageBox.Show("This Question order is already in use\nTry using another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            case -1:
                                MessageBox.Show("Something wrong happened\nPlease try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something went wrong\nPlease try again\n" + ex.Message);
                        Helper.LogError(ex); //write error to log file
                    }
                }
                else
                {
                    MessageBox.Show("Question text cant be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n");
                Helper.LogError(ex);
                return false;
            }
        }

        /// <summary>
        /// Add a slider question through CommDB Class
        /// </summary>
        private bool AddSliderQuestion()
        {
            try
            {
                ///
                /// Declare variables
                ///
                int questionOrder, questionStartValue, questionEndValue;
                string questionText, questionStartValueCaption, questionEndValueCaption;

                ///
                ///Check if Question input fields are not null or empty 
                ///
                if (CheckSliderQuestionInputFields())
                {

                    questionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                    questionText = (string)questionTextRichTextBox.Text;

                    questionStartValueCaption = (string)genericTextBox1.Text;
                    questionEndValueCaption = (string)genericTextBox2.Text;

                    questionStartValue = Convert.ToInt32(genericNumericUpDown1.Value);
                    questionEndValue = Convert.ToInt32(genericNumericUpDown2.Value);

                    ///
                    /// Try to insert a new question into "Slider_Questions" table
                    ///
                    try
                    {
                        int result = CommDB.AddSliderQuestion(questionOrder, questionText, questionStartValue,
                            questionEndValue, questionStartValueCaption, questionEndValueCaption);
                        switch (result)
                        {
                            case 1:
                                MessageBox.Show("Question inserted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ClearInputs();
                                return true;
                                break;
                            case 2:
                                MessageBox.Show("This Question order is already in use\nTry using another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            case -1:
                                MessageBox.Show("Something wrong happened\nPlease try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something went wrong\nPlease try again\n" + ex.Message);
                        Helper.LogError(ex); //write error to log file
                    }
                }
                else
                {
                    MessageBox.Show("Question text\nStart Value caption\nEnd Value caption\nCan NOT be empty\n\nMake sure that Max Value is larger than Min Value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n");
                Helper.LogError(ex);
                return false;
            }
        } // end of function

        /// <summary>
        /// Add a star question through CommDB Class
        /// </summary>
        private bool AddStarQuestion()
        {
            try
            {
                ///
                /// Declare variables
                ///
                int questionOrder, numberOfStars;
                string questionText;

                if (CheckStarQuestionInputFields()) //if Question input fields are not null or empty 
                {
                    questionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                    questionText = questionTextRichTextBox.Text;
                    numberOfStars = Convert.ToInt32(genericNumericUpDown1.Value);

                    ///
                    /// Try to insert a new question into "Star_Questions" table
                    ///
                    try
                    {
                        int result = CommDB.AddStarQuestion(questionOrder, questionText, numberOfStars);
                        switch (result)
                        {
                            case 1:
                                MessageBox.Show("Question inserted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ClearInputs();
                                return true;
                                break;
                            case 2:
                                MessageBox.Show("This Question order is already in use\nTry using another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            case -1:
                                MessageBox.Show("Something wrong happened\nPlease try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something went wrong\nPlease try again\n" + ex.Message);
                        Helper.LogError(ex); //write error to log file
                    }
                }
                else
                {
                    MessageBox.Show("Question text cant be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n");
                Helper.LogError(ex);
                return false;
            }
        }// end of function

        /// <summary>
        /// Edit a smiley question through CommDB Class
        /// </summary>
        private bool EditSmileyQuestion()
        {
            try
            {
                int questionOrder, numberOfSmilyFaces;
                string questionText;

                if (CheckSmileyQuestionInputFields())
                {
                    questionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                    questionText = questionTextRichTextBox.Text;
                    numberOfSmilyFaces = Convert.ToInt32(genericNumericUpDown1.Value);

                    ///
                    /// Try to Update a new question into "Smiley_Questions" table
                    ///
                    try
                    {
                        int result = CommDB.EditSmileyQuestion(QuestionId, questionOrder, questionText, numberOfSmilyFaces);
                        switch (result)
                        {
                            case 1:
                                MessageBox.Show("Question updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return true;
                                break;
                            case 2:
                                MessageBox.Show("This Question order is already in use\nTry using another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            case -1:
                                MessageBox.Show("Something wrong happened\nPlease try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Something went wrong\nPlease try again\n" + ex.Message);
                        Helper.LogError(ex); //write error to log file
                    }
                }
                else
                {
                    MessageBox.Show("Question Text Can NOT be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n");
                Helper.LogError(ex);
                return false;
            }
        }//end of function

        /// <summary>
        /// Edit a slider question through CommDB Class
        /// </summary>
        private bool EditSliderQuestion()
        {
            try
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

                    ///
                    /// Try to Update a new question into "Slider_Questions" table
                    ///
                    try
                    {
                        int result = CommDB.EditSliderQuestion(QuestionId, questionOrder, questionText, questionStartValue, questionEndValue, questionStartValueCaption, questionEndValueCaption);
                        switch (result)
                        {
                            case 1:
                                MessageBox.Show("Question updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return true;
                                break;
                            case 2:
                                MessageBox.Show("This Question order is already in use\nTry using another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            case -1:
                                MessageBox.Show("Something wrong happened\nPlease try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Something went wrong\nPlease try again\n" + ex.Message);
                        Helper.LogError(ex); //write error to log file
                    }
                }
                else
                {
                    MessageBox.Show("Question text\nStart Value caption\nEnd Value caption\nCan NOT be empty\n\nMake sure that Max Value is larger than Min Value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n");
                Helper.LogError(ex);
                return false;
            }
        } //end of function

        /// <summary>
        /// Edit a star question through CommDB Class
        /// </summary>
        private bool EditStarQuestion()
        {
            try
            {
                int questionOrder, numberOfStars;
                string questionText;
                if (CheckStarQuestionInputFields())
                {
                    questionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                    questionText = questionTextRichTextBox.Text;
                    numberOfStars = Convert.ToInt32(genericNumericUpDown1.Value);

                    ///
                    /// Try to Update a new question into "Star_Questions" table
                    ///
                    try
                    {
                        int result = CommDB.EditStarQuestion(QuestionId, questionOrder, questionText, numberOfStars);
                        switch (result)
                        {
                            case 1:
                                MessageBox.Show("Question updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return true;
                                break;
                            case 2:
                                MessageBox.Show("This Question order is already in use\nTry using another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            case -1:
                                MessageBox.Show("Something wrong happened\nPlease try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Something went wrong\nPlease try again\n" + ex.Message);
                        Helper.LogError(ex); //write error to log file
                    }
                }
                else
                {
                    MessageBox.Show("Question Text Can NOT be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n");
                Helper.LogError(ex);
                return false;
            }
        } //end of function

        private void ClearInputs()
        {
            try
            {
                questionOrderNumericUpDown.Value = questionOrderNumericUpDown.Minimum;
                questionTextRichTextBox.Clear();
                genericNumericUpDown1.Value = genericNumericUpDown1.Minimum;
                genericNumericUpDown2.Value = genericNumericUpDown2.Minimum;
                genericTextBox1.Clear();
                genericTextBox2.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n");
                Helper.LogError(ex);
            }
        }

    }
}
