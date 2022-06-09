﻿using SurveyQuestionsConfigurator.CommonHelpers;
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
        private void InitializeEditingSmileyQuestion(int pQuestionId)
        {
            /// Get selected question from DB and fill input fields with its data
            try
            {
                mQuestionId = pQuestionId;
                QuestionManager tQuestionManager = new QuestionManager();
                SmileyQuestion tSmileyQuestion = new SmileyQuestion(pQuestionId);

                ErrorCode tResult = tQuestionManager.GetSmileyQuestionByID(ref tSmileyQuestion);
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
        private void InitializeEditingSliderQuestion(int pQuestionId)
        {
            ///
            /// Get selected question from DB and fill input fields  with its data
            ///
            try
            {
                mQuestionId = pQuestionId;
                QuestionManager tQuestionManager = new QuestionManager();
                SliderQuestion tSliderQuestion = new SliderQuestion(pQuestionId);

                ErrorCode result = tQuestionManager.GetSliderQuestionByID(ref tSliderQuestion);
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
        private void InitializeEditingStarQuestion(int pQuestionId)
        {
            ///
            /// Select wanted question from DB and fill input fields  with its data
            ///
            try
            {
                mQuestionId = pQuestionId;
                QuestionManager tQuestionManager = new QuestionManager();
                StarQuestion tStarQuestion = new StarQuestion(pQuestionId);

                ErrorCode result = tQuestionManager.GetStarQuestionByID(ref tStarQuestion);
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

        #region Validation Methods
        ///<summary>
        /// Check common question input fields for all tyes of questions
        ///</summary>
        private ErrorCode CheckQuestionInputFields()
        {
            if (!String.IsNullOrWhiteSpace(questionTextRichTextBox.Text) && questionTextRichTextBox.TextLength < 4000) //if Question text is not null or empty 
                return ErrorCode.SUCCESS;

            return ErrorCode.ERROR;
        }

        /// <summary>
        /// Check smiley question input fields
        /// </summary>
        private ErrorCode CheckSmileyQuestionInputFields()
        {
            try
            {
                if (CheckQuestionInputFields() == ErrorCode.SUCCESS) //if Question text is not null or empty 
                    if (genericNumericUpDown1.Value >= 2 && genericNumericUpDown1.Value <= 5)
                        return ErrorCode.SUCCESS;

                return ErrorCode.ERROR;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }

        /// <summary>
        /// Check slider question input fields
        /// </summary>
        private ErrorCode CheckSliderQuestionInputFields()
        {
            try
            {
                if (CheckQuestionInputFields() == ErrorCode.SUCCESS) //if Question text is not null or empty 
                    if (!String.IsNullOrWhiteSpace(genericTextBox1.Text) && genericTextBox1.TextLength < 100)
                        if (!String.IsNullOrWhiteSpace(genericTextBox2.Text) && genericTextBox2.TextLength < 100)
                            if (genericNumericUpDown1.Value < genericNumericUpDown2.Value)
                                return ErrorCode.SUCCESS;
                return ErrorCode.ERROR;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }

        /// <summary>
        /// Check star question input fields
        /// </summary>
        private ErrorCode CheckStarQuestionInputFields()
        {
            try
            {
                if (CheckQuestionInputFields() == ErrorCode.SUCCESS) //if Question text is not null or empty 
                    if (genericNumericUpDown1.Value >= 1 && genericNumericUpDown1.Value <= 10)
                        return ErrorCode.SUCCESS;

                return ErrorCode.ERROR;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }
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
        /// Create a question object and pass to QuestionManager (Busineess Logic Layer)
        /// </summary>
        private ErrorCode InsertSmileyQuestion()
        {
            try
            {
                int tQuestionOrder, tNumberOfSmilyFaces;
                string tQuestionText;
                ErrorCode tResult;

                if (CheckSmileyQuestionInputFields() == ErrorCode.SUCCESS) //if Question text is not null or empty 
                {
                    tQuestionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                    tQuestionText = questionTextRichTextBox.Text;
                    tNumberOfSmilyFaces = Convert.ToInt32(genericNumericUpDown1.Value);

                    /// Try to insert a new question into "Questions" and "Smiley_Questions" tables in DB

                    /// Pass -1 as ID because ID is not needed here & there is no constructor that do not accept ID
                    SmileyQuestion tSmileyQuestion = new SmileyQuestion(-1, tQuestionOrder, tQuestionText, QuestionType.SMILEY, tNumberOfSmilyFaces);
                    QuestionManager tQuestionManager = new QuestionManager();
                    tResult = tQuestionManager.InsertSmileyQuestion(tSmileyQuestion);

                    switch (tResult)
                    {
                        case ErrorCode.SUCCESS:
                            orderLabel.Text = "";
                            return ErrorCode.SUCCESS;
                        case ErrorCode.SQL_VIOLATION:
                            orderLabel.Text = "Question order already in use\nTry using another one";
                            break;
                        case ErrorCode.ERROR:
                            orderLabel.Text = "";
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
        }

        /// <summary>
        /// Handle adding a question
        /// Create a question object and pass to QuestionManager (Busineess Logic Layer)
        /// </summary>
        private ErrorCode InsertSliderQuestion()
        {
            try
            {
                int tQuestionOrder, tQuestionStartValue, tQuestionEndValue;
                string tQuestionText, tQuestionStartValueCaption, tQuestionEndValueCaption;
                ErrorCode tResult;
                if (CheckSliderQuestionInputFields() == ErrorCode.SUCCESS)
                {
                    /// Try to insert a new question into "Questions" and "Slider_Questions" tables in DB
                    tQuestionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                    tQuestionText = (string)questionTextRichTextBox.Text;
                    tQuestionStartValueCaption = (string)genericTextBox1.Text;
                    tQuestionEndValueCaption = (string)genericTextBox2.Text;
                    tQuestionStartValue = Convert.ToInt32(genericNumericUpDown1.Value);
                    tQuestionEndValue = Convert.ToInt32(genericNumericUpDown2.Value);

                    /// Pass -1 as ID because ID is not needed here & there is no constructor that do not accept ID
                    SliderQuestion tSliderQuestion = new SliderQuestion(-1, tQuestionOrder, tQuestionText, QuestionType.SLIDER, tQuestionStartValue, tQuestionEndValue, tQuestionStartValueCaption, tQuestionEndValueCaption);
                    QuestionManager tQuestionManager = new QuestionManager();
                    tResult = tQuestionManager.InsertSliderQuestion(tSliderQuestion);

                    switch (tResult)
                    {
                        case ErrorCode.SUCCESS:
                            orderLabel.Text = "";
                            return ErrorCode.SUCCESS;
                        case ErrorCode.SQL_VIOLATION:
                            orderLabel.Text = "Question order already in use\nTry using another one";
                            break;
                        case ErrorCode.ERROR:
                            orderLabel.Text = "";
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
        /// Create a question object and pass to QuestionManager (Busineess Logic Layer)
        /// </summary>
        private ErrorCode InsertStarQuestion()
        {
            try
            {
                int tQuestionOrder, tNumberOfStars;
                string tQuestionText;
                ErrorCode tResult;

                if (CheckStarQuestionInputFields() == ErrorCode.SUCCESS) /// If question input fields are not null or empty 
                {
                    tQuestionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                    tQuestionText = questionTextRichTextBox.Text;
                    tNumberOfStars = Convert.ToInt32(genericNumericUpDown1.Value);

                    /// Try to insert a new question into "Questions" and "Star_Questions" tables in DB

                    /// Pass -1 as ID because ID is not needed here & there is no constructor that do not accept ID
                    StarQuestion tStarQuestion = new StarQuestion(-1, tQuestionOrder, tQuestionText, QuestionType.STAR, tNumberOfStars);
                    QuestionManager tQuestionManager = new QuestionManager();
                    tResult = tQuestionManager.InsertStarQuestion(tStarQuestion);

                    switch (tResult)
                    {
                        case ErrorCode.SUCCESS:
                            orderLabel.Text = "";
                            return ErrorCode.SUCCESS;
                        case ErrorCode.SQL_VIOLATION:
                            orderLabel.Text = "Question order already in use\nTry using another one";
                            break;
                        case ErrorCode.ERROR:
                            orderLabel.Text = "";
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
        /// Create a question object and pass to QuestionManager (Busineess Logic Layer)
        /// </summary>
        private ErrorCode UpdateSmileyQuestion()
        {
            try
            {
                int tQuestionId, tQuestionOrder, tNumberOfSmilyFaces;
                string tQuestionText;
                ErrorCode tResult;

                if (CheckSmileyQuestionInputFields() == ErrorCode.SUCCESS)
                {
                    /// Try to Update a new question into "Questions" and "Smiley_Questions" tables in DB
                    tQuestionId = mQuestionId;
                    tQuestionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                    tQuestionText = questionTextRichTextBox.Text;
                    tNumberOfSmilyFaces = Convert.ToInt32(genericNumericUpDown1.Value);

                    SmileyQuestion tSmileyQuestion = new SmileyQuestion(tQuestionId, tQuestionOrder, tQuestionText, QuestionType.SMILEY, tNumberOfSmilyFaces);
                    QuestionManager tQuestionManager = new QuestionManager();
                    tResult = tQuestionManager.UpdateSmileyQuestion(tSmileyQuestion);

                    switch (tResult)
                    {
                        case ErrorCode.SUCCESS:
                            orderLabel.Text = "";
                            return ErrorCode.SUCCESS;
                        case ErrorCode.SQL_VIOLATION:
                            orderLabel.Text = "Order already in use\nTry using another one";
                            break;
                        case ErrorCode.ERROR:
                            orderLabel.Text = "";
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
        /// Create a question object and pass to QuestionManager (Busineess Logic Layer)
        /// </summary>
        private ErrorCode UpdateSliderQuestion()
        {
            try
            {
                int tQuestionId, tQuestionOrder, tQuestionStartValue, tQuestionEndValue;
                string tQuestionText, tQuestionStartValueCaption, tQuestionEndValueCaption;
                ErrorCode tResult;

                if (CheckSliderQuestionInputFields() == ErrorCode.SUCCESS)
                {
                    /// Try to Update a new question into "Questions" and "Slider_Questions" tables in DB
                    try
                    {
                        tQuestionId = mQuestionId;
                        tQuestionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                        tQuestionText = (string)questionTextRichTextBox.Text;
                        tQuestionStartValue = Convert.ToInt32(genericNumericUpDown1.Value);
                        tQuestionEndValue = Convert.ToInt32(genericNumericUpDown2.Value);
                        tQuestionStartValueCaption = (string)genericTextBox1.Text;
                        tQuestionEndValueCaption = (string)genericTextBox2.Text;

                        SliderQuestion tSliderQuestion = new SliderQuestion(tQuestionId, tQuestionOrder, tQuestionText, QuestionType.SLIDER, tQuestionStartValue, tQuestionEndValue, tQuestionStartValueCaption, tQuestionEndValueCaption);
                        QuestionManager tQuestionManager = new QuestionManager();
                        tResult = tQuestionManager.UpdateSliderQuestion(tSliderQuestion);

                        switch (tResult)
                        {
                            case ErrorCode.SUCCESS:
                                orderLabel.Text = "";
                                return ErrorCode.SUCCESS;
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
        /// Create a question object and pass to QuestionManager (Busineess Logic Layer)
        /// </summary>
        private ErrorCode UpdateStarQuestion()
        {
            try
            {
                int tQuestionId, tQuestionOrder, tNumberOfStars;
                string tQuestionText;
                ErrorCode result;

                if (CheckStarQuestionInputFields() == ErrorCode.SUCCESS)
                {
                    /// Try to Update a new question into "Questions" and "Star_Questions" tables in DB
                    try
                    {
                        tQuestionId = mQuestionId;
                        tQuestionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                        tQuestionText = questionTextRichTextBox.Text;
                        tNumberOfStars = Convert.ToInt32(genericNumericUpDown1.Value);

                        StarQuestion tStarQuestion = new StarQuestion(tQuestionId, tQuestionOrder, tQuestionText, QuestionType.STAR, tNumberOfStars);
                        QuestionManager tQuestionManager = new QuestionManager();
                        result = tQuestionManager.UpdateStarQuestion(tStarQuestion);

                        switch (result)
                        {
                            case ErrorCode.SUCCESS:
                                orderLabel.Text = "";
                                return ErrorCode.SUCCESS;
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
    }
}
