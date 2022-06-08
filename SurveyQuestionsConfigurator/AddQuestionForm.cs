using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.Entities;
using SurveyQuestionsConfigurator.QuestionLogic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Forms;
using static SurveyQuestionsConfigurator.Entities.Generic;

namespace SurveyQuestionsConfigurator
{
    public partial class AddQuestionForm : Form
    {
        #region Properties
        public int mQuestionId { get; set; } /// create global Question ID property
        private FormStateType cStateForm { get; set; } /// Decide whether "OK" Form button is used to either ADD or EDIT a question
        private QuestionType cSelectedQuestionType { get; set; }

        public enum FormStateType
        {
            ADD,
            EDIT
        }

        #endregion

        #region Form Constructors
        /// <summary>
        /// Form constructor for "Adding A Question"
        /// </summary>
        public AddQuestionForm()
        {
            try
            {
                InitializeComponent();
                this.Text = "Add A Question";
                cStateForm = FormStateType.ADD;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        }

        /// <summary>
        /// Form constructor for "Editing A Question"
        /// </summary>
        public AddQuestionForm(int questionId, QuestionType pQuestionType)
        {
            try
            {
                InitializeComponent();
                this.Text = "Edit A Question";

                cStateForm = FormStateType.EDIT;

                mQuestionId = questionId;

                questionTypeComboBox.Enabled = false;

                if (pQuestionType == QuestionType.SMILEY)
                {
                    cSelectedQuestionType = QuestionType.SMILEY; // 0 
                    InitializeEditingSmileyQuestion(questionId);
                }
                else if (pQuestionType == QuestionType.SLIDER)
                {
                    cSelectedQuestionType = QuestionType.SLIDER; // 1
                    InitializeEditingSliderQuestion(questionId);
                }
                else if (pQuestionType == QuestionType.STAR)
                {
                    cSelectedQuestionType = QuestionType.STAR; // 2
                    InitializeEditingStarQuestion(questionId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Handle form's load and load correct question type based what question type is passed to the constructor
        /// </summary>
        private void AddQuestionForm_Load(object sender, EventArgs e)
        {
            try
            {
                questionTypeComboBox.SelectedIndex = (int)cSelectedQuestionType;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                Logger.LogError(ex);
            }
        } // event end

        /// <summary>
        /// Handle close click
        /// </summary>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        } // event end

        /// <summary>
        /// Handle OK button click
        /// Based on what type of form is loaded (Add or Edit)
        /// </summary>
        private void addQuestionButton_Click(object sender, EventArgs e)
        {
            try
            {
                bool OperationSuccess = false;
                if (cStateForm == FormStateType.ADD)
                {
                    try
                    {
                        if (questionTypeComboBox.SelectedIndex == 0)
                        {
                            OperationSuccess = InsertSmileyQuestion();
                        }
                        else if (questionTypeComboBox.SelectedIndex == 1)
                        {
                            OperationSuccess = InsertSliderQuestion();
                        }
                        else if (questionTypeComboBox.SelectedIndex == 2)
                        {
                            OperationSuccess = InsertStarQuestion();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                        Logger.LogError(ex);
                    }
                }
                else if (cStateForm == FormStateType.EDIT)
                {
                    try
                    {
                        if (questionTypeComboBox.SelectedIndex == 0)
                        {
                            OperationSuccess = UpdateSmileyQuestion();
                        }
                        else if (questionTypeComboBox.SelectedIndex == 1)
                        {
                            OperationSuccess = UpdateSliderQuestion();
                        }
                        else if (questionTypeComboBox.SelectedIndex == 2)
                        {
                            OperationSuccess = UpdateStarQuestion();
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Logger.LogError(ex);
                    }
                }

                if (OperationSuccess)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }

        } // event end

        #endregion

        #region Editng Form Intializers
        /// <summary>
        /// Initialize editing smiley question on combobox change
        /// </summary>
        private void InitializeEditingSmileyQuestion(int questionId)
        {
            ///
            /// Select wanted question from DB and fill input fields with its data
            ///
            try
            {
                mQuestionId = questionId;
                QuestionManager questionManager = new QuestionManager();
                SmileyQuestion smileyQuestion = new SmileyQuestion(questionId);

                ErrorCode result = questionManager.GetSmileyQuestionByID(ref smileyQuestion);
                switch (result)
                {
                    case ErrorCode.SUCCESS:
                        questionOrderNumericUpDown.Value = Convert.ToDecimal(smileyQuestion.Order);
                        questionTextRichTextBox.Text = smileyQuestion.Text.ToString();
                        genericNumericUpDown1.Value = Convert.ToDecimal(smileyQuestion.NumberOfSmileyFaces);
                        break;
                    case ErrorCode.EMPTY:
                        MessageBox.Show("Question was not found or deleted\nRefresh the list and try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        break;
                    case ErrorCode.SQL_VIOLATION:
                        MessageBox.Show("Something wrong happened\nPlease try again or contact your system administrator", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case ErrorCode.ERROR:
                        MessageBox.Show("Something wrong happened\nPlease try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex); //write error to log file
            }
        } //End Functiontion

        /// <summary>
        /// Initialize editing slider question on combobox change
        /// </summary>
        private void InitializeEditingSliderQuestion(int questionId)
        {
            ///
            /// Select wanted question from DB and fill input fields  with its data
            ///
            try
            {
                mQuestionId = questionId;
                QuestionManager questionManager = new QuestionManager();
                SliderQuestion sliderQuestion = new SliderQuestion(questionId);

                ErrorCode result = questionManager.GetSliderQuestionByID(ref sliderQuestion);
                switch (result)
                {
                    case ErrorCode.SUCCESS:
                        questionOrderNumericUpDown.Value = Convert.ToDecimal(sliderQuestion.Order);
                        questionTextRichTextBox.Text = sliderQuestion.Text.ToString();
                        genericNumericUpDown1.Value = Convert.ToDecimal(sliderQuestion.StartValue);
                        genericNumericUpDown2.Value = Convert.ToDecimal(sliderQuestion.EndValue);
                        genericTextBox1.Text = sliderQuestion.StartValueCaption.ToString();
                        genericTextBox2.Text = sliderQuestion.EndValueCaption.ToString();
                        break;
                    case ErrorCode.EMPTY:
                        MessageBox.Show("Question was not found or deleted\nRefresh the list and try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        break;
                    case ErrorCode.SQL_VIOLATION:
                        MessageBox.Show("Something wrong happened\nPlease try again or contact your system administrator", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case ErrorCode.ERROR:
                        MessageBox.Show("Something wrong happened\nPlease try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex); //write error to log file
            }
        } //End Functiontion

        /// <summary>
        /// Initialize editing star question on combobox change
        /// </summary>
        private void InitializeEditingStarQuestion(int questionId)
        {
            ///
            /// Select wanted question from DB and fill input fields  with its data
            ///
            try
            {
                mQuestionId = questionId;
                DataTable dataTable = new DataTable();
                QuestionManager questionManager = new QuestionManager();
                StarQuestion starQuestion = new StarQuestion(questionId);

                ErrorCode result = questionManager.GetStarQuestionByID(ref starQuestion);
                switch (result)
                {
                    case ErrorCode.SUCCESS:
                        questionOrderNumericUpDown.Value = Convert.ToDecimal(starQuestion.Order);
                        questionTextRichTextBox.Text = starQuestion.Text.ToString();
                        genericNumericUpDown1.Value = Convert.ToDecimal(starQuestion.NumberOfStars);
                        break;
                    case ErrorCode.EMPTY:
                        MessageBox.Show("Question was not found or deleted\nRefresh the list and try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case ErrorCode.SQL_VIOLATION:
                        MessageBox.Show("Something wrong happened\nPlease try again or contact your system administrator", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case ErrorCode.ERROR:
                        MessageBox.Show("Something wrong happened\nPlease try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex); //write error to log file
            }
        }//End Functiontion
        #endregion

        #region Validation Methods
        private bool QuestionInputFields()
        {
            if (!String.IsNullOrWhiteSpace(questionTextRichTextBox.Text) && questionTextRichTextBox.TextLength < 4000) //if Question text is not null or empty 
                return true;

            return false;
        }


        /// <summary>
        /// Check smiley question input fields
        /// </summary>
        private bool CheckSmileyQuestionInputFields()
        {
            try
            {
                if (QuestionInputFields()) //if Question text is not null or empty 
                    if (genericNumericUpDown1.Value >= 2 && genericNumericUpDown1.Value <= 5)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
                return false;
            }
        }

        /// <summary>
        /// Check slider question input fields
        /// </summary>
        private bool CheckSliderQuestionInputFields()
        {
            try
            {
                if (QuestionInputFields()) //if Question text is not null or empty 
                    if (!String.IsNullOrWhiteSpace(genericTextBox1.Text) && genericTextBox1.TextLength < 100)
                        if (!String.IsNullOrWhiteSpace(genericTextBox2.Text) && genericTextBox2.TextLength < 100)
                            if (genericNumericUpDown1.Value < genericNumericUpDown2.Value)
                                return true;
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
                return false;
            }
        }

        /// <summary>
        /// Check star question input fields
        /// </summary>
        private bool CheckStarQuestionInputFields()
        {
            try
            {
                if (QuestionInputFields()) //if Question text is not null or empty 
                    if (genericNumericUpDown1.Value >= 1 && genericNumericUpDown1.Value <= 10)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
                return false;
            }
        }
        #endregion

        #region ComboBox Selected Index Change Methods
        private void questionTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (questionTypeComboBox.SelectedIndex == 0)
                {
                    InitializeSmileyQuestion();
                }
                else if (questionTypeComboBox.SelectedIndex == 1)
                {
                    InitializeSliderQuestion();
                }
                else if (questionTypeComboBox.SelectedIndex == 2)
                {
                    InitializeStarQuestion();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        }// event end
        private void InitializeSmileyQuestion()
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
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        } // func. end
        private void InitializeSliderQuestion()
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
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        } //func. end
        private void InitializeStarQuestion()
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
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        } //func. end
        #endregion

        #region Add Question Methods
        /// <summary>
        /// Add a smiley question through DbConnect Class
        /// </summary>
        private bool InsertSmileyQuestion()
        {
            try
            {
                ///
                /// Declare variables
                ///
                int questionOrder, numberOfSmilyFaces;
                string questionText;
                ErrorCode result;

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
                        SmileyQuestion smileyQuestion = new SmileyQuestion(-1, questionOrder, questionText, QuestionType.SMILEY, numberOfSmilyFaces);
                        QuestionManager questionManager = new QuestionManager();
                        result = questionManager.InsertSmileyQuestion(smileyQuestion);

                        switch (result)
                        {
                            case ErrorCode.SUCCESS:
                                orderLabel.Text = "";
                                return true;
                            case ErrorCode.SQL_VIOLATION:
                                orderLabel.Text = "Question order already in use\nTry using another one";
                                //MessageBox.Show("This Question order is already in use\nTry using another one", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning );
                                break;
                            case ErrorCode.ERROR:
                                orderLabel.Text = "";
                                MessageBox.Show("Something wrong happened\nPlease try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                        Logger.LogError(ex); //write error to log file
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
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
                return false;
            }
        }

        /// <summary>
        /// Add a slider question through DbConnect Class
        /// </summary>
        private bool InsertSliderQuestion()
        {
            try
            {
                ///
                /// Declare variables
                ///
                int questionOrder, questionStartValue, questionEndValue;
                string questionText, questionStartValueCaption, questionEndValueCaption;
                ErrorCode result;
                ///
                ///Check if Question input fields are not null or empty 
                ///
                if (CheckSliderQuestionInputFields())
                {
                    ///
                    /// Try to insert a new question into "Slider_Questions" table
                    ///
                    try
                    {
                        questionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                        questionText = (string)questionTextRichTextBox.Text;
                        questionStartValueCaption = (string)genericTextBox1.Text;
                        questionEndValueCaption = (string)genericTextBox2.Text;
                        questionStartValue = Convert.ToInt32(genericNumericUpDown1.Value);
                        questionEndValue = Convert.ToInt32(genericNumericUpDown2.Value);


                        SliderQuestion sliderQuestion = new SliderQuestion(-1, questionOrder, questionText, QuestionType.SLIDER, questionStartValue, questionEndValue, questionStartValueCaption, questionEndValueCaption);
                        QuestionManager questionManager = new QuestionManager();
                        result = questionManager.InsertSliderQuestion(sliderQuestion);

                        switch (result)
                        {
                            case ErrorCode.SUCCESS:
                                orderLabel.Text = "";
                                return true;
                            case ErrorCode.SQL_VIOLATION:
                                orderLabel.Text = "Question order already in use\nTry using another one";
                                break;
                            case ErrorCode.ERROR:
                                orderLabel.Text = "";
                                MessageBox.Show("Something wrong happened\nPlease try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                        Logger.LogError(ex); //write error to log file
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
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
                return false;
            }
        } // end of function

        /// <summary>
        /// Add a star question through DbConnect Class
        /// </summary>
        private bool InsertStarQuestion()
        {
            try
            {
                ///
                /// Declare variables
                ///
                int questionOrder, numberOfStars;
                string questionText;
                ErrorCode result;

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
                        StarQuestion starQuestion = new StarQuestion(-1, questionOrder, questionText, QuestionType.STAR, numberOfStars);
                        QuestionManager questionManager = new QuestionManager();
                        result = questionManager.InsertStarQuestion(starQuestion);

                        switch (result)
                        {
                            case ErrorCode.SUCCESS:
                                orderLabel.Text = "";
                                return true;
                            case ErrorCode.SQL_VIOLATION:
                                orderLabel.Text = "Question order already in use\nTry using another one";
                                break;
                            case ErrorCode.ERROR:
                                orderLabel.Text = "";
                                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                        Logger.LogError(ex); //write error to log file
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
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
                return false;
            }
        }// end of function
        #endregion

        #region Edit Question Methods
        /// <summary>
        /// Edit a smiley question through DbConnect Class
        /// </summary>
        private bool UpdateSmileyQuestion()
        {
            try
            {
                int questionId, questionOrder, numberOfSmilyFaces;
                string questionText;
                ErrorCode result;

                if (CheckSmileyQuestionInputFields())
                {
                    ///
                    /// Try to Update a new question into "Smiley_Questions" table
                    ///
                    try
                    {
                        questionId = mQuestionId;
                        questionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                        questionText = questionTextRichTextBox.Text;
                        numberOfSmilyFaces = Convert.ToInt32(genericNumericUpDown1.Value);

                        SmileyQuestion smileyQuestion = new SmileyQuestion(questionId, questionOrder, questionText, QuestionType.SMILEY, numberOfSmilyFaces);
                        QuestionManager questionManager = new QuestionManager();
                        result = questionManager.UpdateSmileyQuestion(smileyQuestion);

                        switch (result)
                        {
                            case ErrorCode.SUCCESS:
                                orderLabel.Text = "";
                                return true;
                            case ErrorCode.SQL_VIOLATION:
                                orderLabel.Text = "Order already in use\nTry using another one";
                                break;
                            case ErrorCode.ERROR:
                                orderLabel.Text = "";
                                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                        Logger.LogError(ex); //write error to log file
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
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
                return false;
            }
        }//end of function

        /// <summary>
        /// Edit a slider question through DbConnect Class
        /// </summary>
        private bool UpdateSliderQuestion()
        {
            try
            {
                int questionId, questionOrder, questionStartValue, questionEndValue;
                string questionText, questionStartValueCaption, questionEndValueCaption;
                ErrorCode result;

                if (CheckSliderQuestionInputFields())
                {
                    ///
                    /// Try to Update a new question into "Slider_Questions" table
                    ///
                    try
                    {
                        questionId = mQuestionId;
                        questionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                        questionText = (string)questionTextRichTextBox.Text;
                        questionStartValue = Convert.ToInt32(genericNumericUpDown1.Value);
                        questionEndValue = Convert.ToInt32(genericNumericUpDown2.Value);
                        questionStartValueCaption = (string)genericTextBox1.Text;
                        questionEndValueCaption = (string)genericTextBox2.Text;

                        SliderQuestion sliderQuestion = new SliderQuestion(questionId, questionOrder, questionText, QuestionType.SLIDER, questionStartValue, questionEndValue, questionStartValueCaption, questionEndValueCaption);
                        QuestionManager questionManager = new QuestionManager();
                        result = questionManager.UpdateSliderQuestion(sliderQuestion);

                        switch (result)
                        {
                            case ErrorCode.SUCCESS:
                                orderLabel.Text = "";
                                return true;
                            case ErrorCode.SQL_VIOLATION:
                                orderLabel.Text = "Question order already in use\nTry using another one";
                                break;
                            case ErrorCode.ERROR:
                                orderLabel.Text = "";
                                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                        Logger.LogError(ex); //write error to log file
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
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
                return false;
            }
        } //end of function

        /// <summary>
        /// Edit a star question through DbConnect Class
        /// </summary>
        private bool UpdateStarQuestion()
        {
            try
            {
                int questionId, questionOrder, numberOfStars;
                string questionText;
                ErrorCode result;

                if (CheckStarQuestionInputFields())
                {
                    ///
                    /// Try to Update a new question into "Star_Questions" table
                    ///
                    try
                    {
                        questionId = mQuestionId;
                        questionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                        questionText = questionTextRichTextBox.Text;
                        numberOfStars = Convert.ToInt32(genericNumericUpDown1.Value);

                        StarQuestion starQuestion = new StarQuestion(questionId, questionOrder, questionText, QuestionType.STAR, numberOfStars);
                        QuestionManager questionManager = new QuestionManager();
                        result = questionManager.UpdateStarQuestion(starQuestion);

                        switch (result)
                        {
                            case ErrorCode.SUCCESS:
                                orderLabel.Text = "";
                                return true;
                            case ErrorCode.SQL_VIOLATION:
                                orderLabel.Text = "Question order already in use\nTry using another one";
                                break;
                            case ErrorCode.ERROR:
                                orderLabel.Text = "";
                                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                        Logger.LogError(ex); //write error to log file
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
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
                return false;
            }
        } //end of function
        #endregion


        //private void ClearInputs()
        //{
        //    try
        //    {
        //        questionOrderNumericUpDown.Value = questionOrderNumericUpDown.Minimum;
        //        questionTextRichTextBox.Clear();
        //        genericNumericUpDown1.Value = genericNumericUpDown1.Minimum;
        //        genericNumericUpDown2.Value = genericNumericUpDown2.Minimum;
        //        genericTextBox1.Clear();
        //        genericTextBox2.Clear();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.ErrorCode);
        //        Logger.LogError(ex);
        //    }
        //}

    }
}
