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
        private Question mGenericQuestion { get; set; } /// create global Question ID property

        private readonly QuestionManager mGeneralQuestionManager;

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
                mGeneralQuestionManager = new QuestionManager();
                this.Text = "Add A Question";
                cStateForm = FormStateType.ADD;
                errorLabel.Text = "";
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
        public AddQuestionForm(Question pQuestion)
        {
            try
            {
                InitializeComponent();
                mGeneralQuestionManager = new QuestionManager();
                this.Text = "Edit A Question";
                cStateForm = FormStateType.EDIT;
                errorLabel.Text = "";
                mGenericQuestion = pQuestion;
                questionTypeComboBox.Enabled = false;

                if (pQuestion.Type == QuestionType.SMILEY)
                {
                    cSelectedQuestionType = QuestionType.SMILEY; // 0 
                    InitializeEditingSmileyQuestion(pQuestion);
                }
                else if (pQuestion.Type == QuestionType.SLIDER)
                {
                    cSelectedQuestionType = QuestionType.SLIDER; // 1
                    InitializeEditingSliderQuestion(pQuestion);
                }
                else if (pQuestion.Type == QuestionType.STAR)
                {
                    cSelectedQuestionType = QuestionType.STAR; // 2
                    InitializeEditingStarQuestion(pQuestion);
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
        } /// End event

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
        } /// End event

        /// <summary>
        /// Handle OK button click
        /// Based on what type of form is loaded (Add or Edit)
        /// </summary>
        private void addQuestionButton_Click(object sender, EventArgs e)
        {
            try
            {
                /// Close form if operation was successful
                ErrorCode OperationSuccess = ErrorCode.ERROR;
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

                if (OperationSuccess == ErrorCode.SUCCESS)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }

        } /// End event

        #endregion

        #region Editng Form Intializers

        /// <summary>
        /// Initialize editing smiley question on combobox index change event
        /// </summary>
        private void InitializeEditingSmileyQuestion(Question pQuestion)
        {
            /// Get selected question from DB and fill input fields with its data
            try
            {
                mGenericQuestion = pQuestion;
                SmileyQuestion tSmileyQuestion = new SmileyQuestion(pQuestion.ID);

                ErrorCode tResult = mGeneralQuestionManager.GetSmileyQuestionByID(ref tSmileyQuestion);
                switch (tResult)
                {
                    case ErrorCode.SUCCESS:
                        questionOrderNumericUpDown.Value = Convert.ToDecimal(tSmileyQuestion.Order);
                        questionTextRichTextBox.Text = tSmileyQuestion.Text.ToString();
                        genericNumericUpDown1.Value = Convert.ToDecimal(tSmileyQuestion.NumberOfSmileyFaces);
                        break;
                    case ErrorCode.EMPTY:
                        MessageBox.Show("Question was not found or deleted\nRefresh the list and try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        break;
                    case ErrorCode.SQL_VIOLATION:
                        MessageBox.Show("Something wrong happened\nPlease try again or contact your system administrator", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        break;
                    case ErrorCode.ERROR:
                        MessageBox.Show("Something wrong happened\nPlease try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex); //write error to log file
            }
        } /// End function

        /// <summary>
        /// Initialize editing slider question on combobox index change event
        /// </summary>
        private void InitializeEditingSliderQuestion(Question pQuestion)
        {
            ///
            /// Get selected question from DB and fill input fields  with its data
            ///
            try
            {
                mGenericQuestion = pQuestion;
                SliderQuestion tSliderQuestion = new SliderQuestion(pQuestion.ID);

                ErrorCode result = mGeneralQuestionManager.GetSliderQuestionByID(ref tSliderQuestion);
                switch (result)
                {
                    case ErrorCode.SUCCESS:
                        questionOrderNumericUpDown.Value = Convert.ToDecimal(tSliderQuestion.Order);
                        questionTextRichTextBox.Text = tSliderQuestion.Text.ToString();
                        genericNumericUpDown1.Value = Convert.ToDecimal(tSliderQuestion.StartValue);
                        genericNumericUpDown2.Value = Convert.ToDecimal(tSliderQuestion.EndValue);
                        genericTextBox1.Text = tSliderQuestion.StartValueCaption.ToString();
                        genericTextBox2.Text = tSliderQuestion.EndValueCaption.ToString();
                        break;
                    case ErrorCode.EMPTY:
                        MessageBox.Show("Question was not found or deleted\nRefresh the list and try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        break;
                    case ErrorCode.SQL_VIOLATION:
                        MessageBox.Show("Something wrong happened\nPlease try again or contact your system administrator", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        break;
                    case ErrorCode.ERROR:
                        MessageBox.Show("Something wrong happened\nPlease try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex); //write error to log file
            }
        } /// End function

        /// <summary>
        /// Initialize editing star question on combobox change
        /// </summary>
        private void InitializeEditingStarQuestion(Question pQuestion)
        {
            ///
            /// Select wanted question from DB and fill input fields  with its data
            ///
            try
            {
                mGenericQuestion = pQuestion;
                StarQuestion tStarQuestion = new StarQuestion(pQuestion.ID);

                ErrorCode result = mGeneralQuestionManager.GetStarQuestionByID(ref tStarQuestion);
                switch (result)
                {
                    case ErrorCode.SUCCESS:
                        questionOrderNumericUpDown.Value = Convert.ToDecimal(tStarQuestion.Order);
                        questionTextRichTextBox.Text = tStarQuestion.Text.ToString();
                        genericNumericUpDown1.Value = Convert.ToDecimal(tStarQuestion.NumberOfStars);
                        break;
                    case ErrorCode.EMPTY:
                        MessageBox.Show("Question was not found or deleted\nRefresh the list and try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        break;
                    case ErrorCode.SQL_VIOLATION:
                        MessageBox.Show("Something wrong happened\nPlease try again or contact your system administrator", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        break;
                    case ErrorCode.ERROR:
                        MessageBox.Show("Something wrong happened\nPlease try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex); //write error to log file
            }
        }/// End function
        #endregion


        #region ComboBox Selected Index Change Methods
        /// <summary>
        /// Handle combo box selected index changed
        /// </summary>
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
        }/// End event

         ///<summary>
         /// Initialize smiley question based on combo box changed value
         ///</summary>
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

                genericTextBox1.Visible = false;
                genericTextBox2.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        } /// End function

        ///<summary>
        /// Initialize slider question based on combo box changed value
        ///</summary>
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
                genericNumericUpDown2.Minimum = 2;
                genericNumericUpDown2.Maximum = 100;

                genericTextBox1.Visible = true;
                genericTextBox2.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        } /// End function

        ///<summary>
        /// Initialize star question based on combo box changed value
        ///</summary>
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

                genericTextBox1.Visible = false;
                genericTextBox2.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        } /// End function
        #endregion

        #region Add Question Methods
        /// <summary>
        /// Handle adding a question
        /// Create a question object and pass to h5h (Busineess Logic Layer)
        /// </summary>
        private ErrorCode InsertSmileyQuestion()
        {
            try
            {
                int tQuestionOrder, tNumberOfSmilyFaces;
                string tQuestionText;
                ErrorCode tResult;


                tQuestionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                tQuestionText = questionTextRichTextBox.Text;
                tNumberOfSmilyFaces = Convert.ToInt32(genericNumericUpDown1.Value);
                /// Pass -1 as ID because ID is not needed here & there is no constructor that do not accept ID
                SmileyQuestion tSmileyQuestion = new SmileyQuestion(-1, tQuestionOrder, tQuestionText, QuestionType.SMILEY, tNumberOfSmilyFaces);

                /// Try to insert a new question into "Questions" and "Smiley_Questions" tables in DB
                if (CheckSmileyQuestionInputFields(tSmileyQuestion) == ErrorCode.SUCCESS) //if Question text is not null or empty 
                {
                    tResult = mGeneralQuestionManager.InsertSmileyQuestion(tSmileyQuestion);

                    switch (tResult)
                    {
                        case ErrorCode.SUCCESS:
                            errorLabel.Text = "";
                            return ErrorCode.SUCCESS;
                        case ErrorCode.SQL_VIOLATION:
                            errorLabel.Text = "Question order already in use\nTry using another one";
                            break;
                        case ErrorCode.ERROR:
                            errorLabel.Text = "";
                            MessageBox.Show("Something wrong happened\nPlease try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Question text cant be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return ErrorCode.ERROR;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }/// Function end

        /// <summary>
        /// Handle adding a question
        /// Create a question object and pass to h5h (Busineess Logic Layer)
        /// </summary>
        private ErrorCode InsertSliderQuestion()
        {
            try
            {
                int tQuestionOrder, tQuestionStartValue, tQuestionEndValue;
                string tQuestionText, tQuestionStartValueCaption, tQuestionEndValueCaption;
                ErrorCode tResult;
                tQuestionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                tQuestionText = (string)questionTextRichTextBox.Text;
                tQuestionStartValueCaption = (string)genericTextBox1.Text;
                tQuestionEndValueCaption = (string)genericTextBox2.Text;
                tQuestionStartValue = Convert.ToInt32(genericNumericUpDown1.Value);
                tQuestionEndValue = Convert.ToInt32(genericNumericUpDown2.Value);

                /// Pass -1 as ID because ID is not needed here & there is no constructor that do not accept ID
                SliderQuestion tSliderQuestion = new SliderQuestion(-1, tQuestionOrder, tQuestionText, QuestionType.SLIDER, tQuestionStartValue, tQuestionEndValue, tQuestionStartValueCaption, tQuestionEndValueCaption);

                if (CheckSliderQuestionInputFields(tSliderQuestion) == ErrorCode.SUCCESS)
                {
                    /// Try to insert a new question into "Questions" and "Slider_Questions" tables in DB
                    tResult = mGeneralQuestionManager.InsertSliderQuestion(tSliderQuestion);

                    switch (tResult)
                    {
                        case ErrorCode.SUCCESS:
                            errorLabel.Text = "";
                            return ErrorCode.SUCCESS;
                        case ErrorCode.SQL_VIOLATION:
                            errorLabel.Text = "Question order already in use\nTry using another one";
                            break;
                        case ErrorCode.ERROR:
                            errorLabel.Text = "";
                            MessageBox.Show("Something wrong happened\nPlease try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Question text\nStart Value caption\nEnd Value caption\nCan NOT be empty\n\nMake sure that Max Value is larger than Min Value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return ErrorCode.ERROR;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        } /// End function

        /// <summary>
        /// Handle adding a question
        /// Create a question object and pass to h5h (Busineess Logic Layer)
        /// </summary>
        private ErrorCode InsertStarQuestion()
        {
            try
            {
                int tQuestionOrder, tNumberOfStars;
                string tQuestionText;
                ErrorCode tResult;

                tQuestionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                tQuestionText = questionTextRichTextBox.Text;
                tNumberOfStars = Convert.ToInt32(genericNumericUpDown1.Value);


                /// Pass -1 as ID because ID is not needed here & there is no constructor that do not accept ID
                StarQuestion tStarQuestion = new StarQuestion(-1, tQuestionOrder, tQuestionText, QuestionType.STAR, tNumberOfStars);

                /// Try to insert a new question into "Questions" and "Star_Questions" tables in DB
                if (CheckStarQuestionInputFields(tStarQuestion) == ErrorCode.SUCCESS) /// If question input fields are not null or empty 
                {
                    tResult = mGeneralQuestionManager.InsertStarQuestion(tStarQuestion);

                    switch (tResult)
                    {
                        case ErrorCode.SUCCESS:
                            errorLabel.Text = "";
                            return ErrorCode.SUCCESS;
                        case ErrorCode.SQL_VIOLATION:
                            errorLabel.Text = "Question order already in use\nTry using another one";
                            break;
                        case ErrorCode.ERROR:
                            errorLabel.Text = "";
                            MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Question text cant be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return ErrorCode.ERROR;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }/// End function
        #endregion

        #region Edit Question Methods
        /// <summary>
        /// Handle editing a question
        /// Create a question object and pass to h5h (Busineess Logic Layer)
        /// </summary>
        private ErrorCode UpdateSmileyQuestion()
        {
            try
            {
                int tQuestionId, tQuestionOrder, tNumberOfSmilyFaces;
                string tQuestionText;
                ErrorCode tResult;

                tQuestionId = mGenericQuestion.ID;
                tQuestionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                tQuestionText = questionTextRichTextBox.Text;
                tNumberOfSmilyFaces = Convert.ToInt32(genericNumericUpDown1.Value);

                SmileyQuestion tSmileyQuestion = new SmileyQuestion(tQuestionId, tQuestionOrder, tQuestionText, QuestionType.SMILEY, tNumberOfSmilyFaces);

                if (CheckSmileyQuestionInputFields(tSmileyQuestion) == ErrorCode.SUCCESS)
                {
                    /// Try to Update a new question into "Questions" and "Smiley_Questions" tables in DB
                    tResult = mGeneralQuestionManager.UpdateSmileyQuestion(tSmileyQuestion);

                    switch (tResult)
                    {
                        case ErrorCode.SUCCESS:
                            errorLabel.Text = "";
                            return ErrorCode.SUCCESS;
                        case ErrorCode.SQL_VIOLATION:
                            errorLabel.Text = "Order already in use\nTry using another one";
                            break;
                        case ErrorCode.ERROR:
                            errorLabel.Text = "";
                            MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Question Text Can NOT be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return ErrorCode.ERROR;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }/// Function end

        /// <summary>
        /// Handle editing a question
        /// Create a question object and pass to h5h (Busineess Logic Layer)
        /// </summary>
        private ErrorCode UpdateSliderQuestion()
        {
            try
            {
                int tQuestionId, tQuestionOrder, tQuestionStartValue, tQuestionEndValue;
                string tQuestionText, tQuestionStartValueCaption, tQuestionEndValueCaption;
                ErrorCode tResult;

                tQuestionId = mGenericQuestion.ID;
                tQuestionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                tQuestionText = (string)questionTextRichTextBox.Text;
                tQuestionStartValue = Convert.ToInt32(genericNumericUpDown1.Value);
                tQuestionEndValue = Convert.ToInt32(genericNumericUpDown2.Value);
                tQuestionStartValueCaption = (string)genericTextBox1.Text;
                tQuestionEndValueCaption = (string)genericTextBox2.Text;

                SliderQuestion tSliderQuestion = new SliderQuestion(tQuestionId, tQuestionOrder, tQuestionText, QuestionType.SLIDER, tQuestionStartValue, tQuestionEndValue, tQuestionStartValueCaption, tQuestionEndValueCaption);

                if (CheckSliderQuestionInputFields(tSliderQuestion) == ErrorCode.SUCCESS)
                {
                    /// Try to Update a new question into "Questions" and "Slider_Questions" tables in DB
                    try
                    {
                        tResult = mGeneralQuestionManager.UpdateSliderQuestion(tSliderQuestion);

                        switch (tResult)
                        {
                            case ErrorCode.SUCCESS:
                                errorLabel.Text = "";
                                return ErrorCode.SUCCESS;
                            case ErrorCode.SQL_VIOLATION:
                                errorLabel.Text = "Question order already in use\nTry using another one";
                                break;
                            case ErrorCode.ERROR:
                                errorLabel.Text = "";
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
                return ErrorCode.ERROR;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        } /// Function end

        /// <summary>
        /// Handle editing a question
        /// Create a question object and pass to h5h (Busineess Logic Layer)
        /// </summary>
        private ErrorCode UpdateStarQuestion()
        {
            try
            {
                int tQuestionId, tQuestionOrder, tNumberOfStars;
                string tQuestionText;
                ErrorCode result;

                tQuestionId = mGenericQuestion.ID;
                tQuestionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                tQuestionText = questionTextRichTextBox.Text;
                tNumberOfStars = Convert.ToInt32(genericNumericUpDown1.Value);

                StarQuestion tStarQuestion = new StarQuestion(tQuestionId, tQuestionOrder, tQuestionText, QuestionType.STAR, tNumberOfStars);

                if (CheckStarQuestionInputFields(tStarQuestion) == ErrorCode.SUCCESS)
                {
                    /// Try to Update a new question into "Questions" and "Star_Questions" tables in DB
                    result = mGeneralQuestionManager.UpdateStarQuestion(tStarQuestion);

                    switch (result)
                    {
                        case ErrorCode.SUCCESS:
                            errorLabel.Text = "";
                            return ErrorCode.SUCCESS;
                        case ErrorCode.SQL_VIOLATION:
                            errorLabel.Text = "Question order already in use\nTry using another one";
                            break;
                        case ErrorCode.ERROR:
                            errorLabel.Text = "";
                            MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Question Text Can NOT be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return ErrorCode.ERROR;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        } /// Function end
        #endregion

        #region Validation Methods

        /// <summary>
        /// Check smiley question input fields
        /// </summary>
        private ErrorCode CheckSmileyQuestionInputFields(SmileyQuestion pSmileyQuestion)
        {
            try
            {
                return mGeneralQuestionManager.CheckSmileyQuestionValues(pSmileyQuestion);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }/// Function end

        /// <summary>
        /// Check slider question input fields
        /// </summary>
        private ErrorCode CheckSliderQuestionInputFields(SliderQuestion pSliderQuestion)
        {
            try
            {
                return mGeneralQuestionManager.CheckSliderQuestionValues(pSliderQuestion);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }/// Function end

        /// <summary>
        /// Check star question input fields
        /// </summary>
        private ErrorCode CheckStarQuestionInputFields(StarQuestion pStarQuestion)
        {
            try
            {
                return mGeneralQuestionManager.CheckStarQuestionValues(pStarQuestion);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }/// Function end
        #endregion
    }
}
