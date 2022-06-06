using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.CommonTypes;
using SurveyQuestionsConfigurator.Entities;
using SurveyQuestionsConfigurator.QuestionLogic;
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
        #region Properties
        /// 
        /// Global Variables
        /// Access them anywhere
        ///
        public int QuestionId { get; set; } /// create global Question ID property
        private string StateForm { get; set; } /// Decide whether "OK" Form button is used to either ADD or EDIT a question
        private int SelectedQuestionType { get; set; }

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
                StateForm = FormStateType.ADD.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        }

        /// <summary>
        /// Form constructor for "Editing A Question"
        /// </summary>
        public AddQuestionForm(int questionId, int questionType)
        {
            try
            {
                InitializeComponent();
                this.Text = "Edit A Question";

                StateForm = FormStateType.EDIT.ToString();

                QuestionId = questionId; //set QuestionId to access it globally
                                         //Question = questionType;
                questionTypeComboBox.Enabled = false;

                if (questionType == (int)Types.Question.SMILEY)
                {
                    SelectedQuestionType = (int)Types.Question.SMILEY; // 0 
                    InitializeEditingSmileyQuestion(questionId);
                }
                else if (questionType == (int)Types.Question.SLIDER)
                {
                    SelectedQuestionType = (int)Types.Question.SLIDER; // 1
                    InitializeEditingSliderQuestion(questionId);
                }
                else if (questionType == (int)Types.Question.STAR)
                {
                    SelectedQuestionType = (int)Types.Question.STAR; // 2
                    InitializeEditingStarQuestion(questionId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }

        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Selecting correct combo box item
        /// </summary>
        private void AddQuestionForm_Load(object sender, EventArgs e)
        {
            try
            {
                questionTypeComboBox.SelectedIndex = SelectedQuestionType;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                Logger.LogError(ex);
            }
        } // event end

        /// <summary>
        /// Closing the form
        /// </summary>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        } // event end

        private void addQuestionButton_Click(object sender, EventArgs e)
        {
            try
            {
                bool OperationSuccess = false;
                if (StateForm.ToString().ToUpper() == FormStateType.ADD.ToString().ToUpper())
                {
                    try
                    {
                        if (questionTypeComboBox.SelectedIndex == 0)
                        {
                            OperationSuccess = AddSmileyQuestion();
                        }
                        else if (questionTypeComboBox.SelectedIndex == 1)
                        {
                            OperationSuccess = AddSliderQuestion();
                        }
                        else if (questionTypeComboBox.SelectedIndex == 2)
                        {
                            OperationSuccess = AddStarQuestion();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                        Logger.LogError(ex);
                    }
                }
                else if (StateForm.ToString().ToUpper() == FormStateType.EDIT.ToString().ToUpper())
                {
                    try
                    {
                        if (questionTypeComboBox.SelectedIndex == 0)
                        {
                            OperationSuccess = EditSmileyQuestion();
                        }
                        else if (questionTypeComboBox.SelectedIndex == 1)
                        {
                            OperationSuccess = EditSliderQuestion();
                        }
                        else if (questionTypeComboBox.SelectedIndex == 2)
                        {
                            OperationSuccess = EditStarQuestion();
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                QuestionId = questionId;
                DataTable dataTable = new DataTable();
                QuestionManager questionManager = new QuestionManager();

                int result = questionManager.GetSmileyQuestionByID(questionId, ref dataTable);
                switch (result)
                {
                    case (int)Types.ErrorCode.SUCCESS:
                        foreach (DataRow row in dataTable.Rows)
                        {
                            questionOrderNumericUpDown.Value = Convert.ToDecimal(row[0]);
                            questionTextRichTextBox.Text = (string)row[1];
                            genericNumericUpDown1.Value = Convert.ToDecimal(row[2]);
                        }
                        break;
                    case (int)Types.ErrorCode.EMPTY:
                        MessageBox.Show("Question was not found or deleted\nRefresh the list and try again", "ErrorCode", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case (int)Types.ErrorCode.SQLVIOLATION:
                        MessageBox.Show("Something wrong happened\nPlease try again or contact your system administrator", "ErrorCode", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case (int)Types.ErrorCode.ERROR:
                        MessageBox.Show("Something wrong happened\nPlease try again", "ErrorCode", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex); //write error to log file
            }
        } //end function

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
                QuestionId = questionId;
                DataTable dataTable = new DataTable();
                QuestionManager questionManager = new QuestionManager();

                int result = questionManager.GetSliderQuestionByID(questionId, ref dataTable);
                switch (result)
                {
                    case (int)Types.ErrorCode.SUCCESS:
                        foreach (DataRow row in dataTable.Rows)
                        {
                            questionOrderNumericUpDown.Value = Convert.ToDecimal(row[0]);
                            questionTextRichTextBox.Text = (string)row[1];
                            genericNumericUpDown1.Value = Convert.ToDecimal(row[2]);
                            genericNumericUpDown2.Value = Convert.ToDecimal(row[3]);

                            genericTextBox1.Text = (string)row[4];
                            genericTextBox2.Text = (string)row[5];
                        }
                        break;
                    case (int)Types.ErrorCode.EMPTY:
                        MessageBox.Show("Question was not found or deleted\nRefresh the list and try again", "ErrorCode", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case (int)Types.ErrorCode.SQLVIOLATION:
                        MessageBox.Show("Something wrong happened\nPlease try again or contact your system administrator", "ErrorCode", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case (int)Types.ErrorCode.ERROR:
                        MessageBox.Show("Something wrong happened\nPlease try again", "ErrorCode", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex); //write error to log file
            }
        } //end function

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
                QuestionId = questionId;
                DataTable dataTable = new DataTable();

                QuestionManager questionManager = new QuestionManager();

                int result = questionManager.GetStarQuestionByID(questionId, ref dataTable);
                switch (result)
                {
                    case (int)Types.ErrorCode.SUCCESS:
                        foreach (DataRow row in dataTable.Rows)
                        {
                            questionOrderNumericUpDown.Value = Convert.ToDecimal(row[0]);
                            questionTextRichTextBox.Text = (string)row[1];
                            genericNumericUpDown1.Value = Convert.ToDecimal(row[2]);
                        }
                        break;
                    case (int)Types.ErrorCode.EMPTY:
                        MessageBox.Show("Question was not found or deleted\nRefresh the list and try again", "ErrorCode", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case (int)Types.ErrorCode.SQLVIOLATION:
                        MessageBox.Show("Something wrong happened\nPlease try again or contact your system administrator", "ErrorCode", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case (int)Types.ErrorCode.ERROR:
                        MessageBox.Show("Something wrong happened\nPlease try again", "ErrorCode", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex); //write error to log file
            }
        }//end function
        #endregion

        #region Check Input Fields Methods Before Passing Data To Next Layer
        /// <summary>
        /// Check smiley question input fields
        /// </summary>
        private bool CheckSmileyQuestionInputFields()
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(questionTextRichTextBox.Text) && questionTextRichTextBox.TextLength < 8000) //if Question text is not null or empty 
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
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
                if (!String.IsNullOrWhiteSpace(questionTextRichTextBox.Text) && questionTextRichTextBox.TextLength < 8000) //if Question text is not null or empty 
                    if (!String.IsNullOrWhiteSpace(genericTextBox1.Text) && genericTextBox1.TextLength < 100)
                        if (!String.IsNullOrWhiteSpace(genericTextBox2.Text) && genericTextBox2.TextLength < 100)
                            if (genericNumericUpDown1.Value < genericNumericUpDown2.Value)
                                return true;
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
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
                if (!String.IsNullOrWhiteSpace(questionTextRichTextBox.Text) && questionTextRichTextBox.TextLength < 8000) //if Question text is not null or empty 
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
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
                    InitializeLoadingASmileyQuestion();
                }
                else if (questionTypeComboBox.SelectedIndex == 1)
                {
                    InitializeLoadingASliderQuestion();
                }
                else if (questionTypeComboBox.SelectedIndex == 2)
                {
                    InitializeLoadingAStarQuestion();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        }// event end
        private void InitializeLoadingASmileyQuestion()
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
                MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        } // func. end
        private void InitializeLoadingASliderQuestion()
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
                MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        } //func. end
        private void InitializeLoadingAStarQuestion()
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
                MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        } //func. end
        #endregion

        #region Add Question Methods
        /// <summary>
        /// Add a smiley question through DbConnect Class
        /// </summary>
        private bool AddSmileyQuestion()
        {
            try
            {
                ///
                /// Declare variables
                ///Question.Question.Question.
                int questionOrder, numberOfSmilyFaces;
                string questionText;
                int result;

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
                        SmileyQuestion smileyQuestion = new SmileyQuestion(-1, questionOrder, questionText, (int)Types.Question.SMILEY, numberOfSmilyFaces);
                        QuestionManager questionManager = new QuestionManager();
                        result = questionManager.AddSmileyQuestion(smileyQuestion);

                        switch (result)
                        {
                            case (int)Types.ErrorCode.SUCCESS:
                                MessageBox.Show("Question inserted successfully\nPress OK to see changes", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //ClearInputs();
                                return true;
                            case (int)Types.ErrorCode.SQLVIOLATION:
                                MessageBox.Show("This Question order is already in use\nTry using another one", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                break;
                            case (int)Types.ErrorCode.ERROR:
                                MessageBox.Show("Something wrong happened\nPlease try again", "ErrorCode", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                        Logger.LogError(ex); //write error to log file
                    }
                }
                else
                {
                    MessageBox.Show("Question text cant be empty", "ErrorCode", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
                return false;
            }
        }

        /// <summary>
        /// Add a slider question through DbConnect Class
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
                int result;
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


                        SliderQuestion sliderQuestion = new SliderQuestion(-1, questionOrder, questionText, (int)Types.Question.SLIDER, questionStartValue, questionEndValue, questionStartValueCaption, questionEndValueCaption);
                        QuestionManager questionManager = new QuestionManager();
                        result = questionManager.AddSliderQuestion(sliderQuestion);

                        switch (result)
                        {
                            case (int)Types.ErrorCode.SUCCESS:
                                MessageBox.Show("Question inserted successfully\nPress OK to see changes", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //ClearInputs();
                                return true;
                            case (int)Types.ErrorCode.SQLVIOLATION:
                                MessageBox.Show("This Question order is already in use\nTry using another one", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                break;
                            case (int)Types.ErrorCode.ERROR:
                                MessageBox.Show("Something wrong happened\nPlease try again", "ErrorCode", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                        Logger.LogError(ex); //write error to log file
                    }
                }
                else
                {
                    MessageBox.Show("Question text\nStart Value caption\nEnd Value caption\nCan NOT be empty\n\nMake sure that Max Value is larger than Min Value", "ErrorCode", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
                return false;
            }
        } // end of function

        /// <summary>
        /// Add a star question through DbConnect Class
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
                int result;

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
                        StarQuestion starQuestion = new StarQuestion(-1, questionOrder, questionText, (int)Types.Question.STAR, numberOfStars);
                        QuestionManager questionManager = new QuestionManager();
                        result = questionManager.AddStarQuestion(starQuestion);

                        switch (result)
                        {
                            case (int)Types.ErrorCode.SUCCESS:
                                MessageBox.Show("Question inserted successfully\nPress OK to see changes", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //ClearInputs();
                                return true;
                            case (int)Types.ErrorCode.SQLVIOLATION:
                                MessageBox.Show("This Question order is already in use\nTry using another one", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                break;
                            case (int)Types.ErrorCode.ERROR:
                                MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                        Logger.LogError(ex); //write error to log file
                    }
                }
                else
                {
                    MessageBox.Show("Question text cant be empty", "ErrorCode", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
                return false;
            }
        }// end of function
        #endregion

        #region Edit Question Methods
        /// <summary>
        /// Edit a smiley question through DbConnect Class
        /// </summary>
        private bool EditSmileyQuestion()
        {
            try
            {
                int questionId, questionOrder, numberOfSmilyFaces;
                string questionText;
                int result;

                if (CheckSmileyQuestionInputFields())
                {
                    ///
                    /// Try to Update a new question into "Smiley_Questions" table
                    ///
                    try
                    {
                        questionId = QuestionId;
                        questionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                        questionText = questionTextRichTextBox.Text;
                        numberOfSmilyFaces = Convert.ToInt32(genericNumericUpDown1.Value);

                        SmileyQuestion smileyQuestion = new SmileyQuestion(questionId, questionOrder, questionText, (int)Types.Question.SMILEY, numberOfSmilyFaces);
                        QuestionManager questionManager = new QuestionManager();
                        result = questionManager.EditSmileyQuestion(smileyQuestion);

                        switch (result)
                        {
                            case (int)Types.ErrorCode.SUCCESS:
                                MessageBox.Show("Question updated successfully\nPress OK to see changes", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return true;
                            case (int)Types.ErrorCode.SQLVIOLATION:
                                MessageBox.Show("This Question order is already in use\nTry using another one", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                break;
                            case (int)Types.ErrorCode.ERROR:
                                MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                        Logger.LogError(ex); //write error to log file
                    }
                }
                else
                {
                    MessageBox.Show("Question Text Can NOT be empty", "ErrorCode", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
                return false;
            }
        }//end of function

        /// <summary>
        /// Edit a slider question through DbConnect Class
        /// </summary>
        private bool EditSliderQuestion()
        {
            try
            {
                int questionId, questionOrder, questionStartValue, questionEndValue;
                string questionText, questionStartValueCaption, questionEndValueCaption;
                int result;

                if (CheckSliderQuestionInputFields())
                {
                    ///
                    /// Try to Update a new question into "Slider_Questions" table
                    ///
                    try
                    {
                        questionId = QuestionId;
                        questionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                        questionText = (string)questionTextRichTextBox.Text;
                        questionStartValue = Convert.ToInt32(genericNumericUpDown1.Value);
                        questionEndValue = Convert.ToInt32(genericNumericUpDown2.Value);
                        questionStartValueCaption = (string)genericTextBox1.Text;
                        questionEndValueCaption = (string)genericTextBox2.Text;

                        SliderQuestion sliderQuestion = new SliderQuestion(questionId, questionOrder, questionText, (int)Types.Question.SLIDER, questionStartValue, questionEndValue, questionStartValueCaption, questionEndValueCaption);
                        QuestionManager questionManager = new QuestionManager();
                        result = questionManager.EditSliderQuestion(sliderQuestion);

                        switch (result)
                        {
                            case (int)Types.ErrorCode.SUCCESS:
                                MessageBox.Show("Question updated successfully\nPress OK to see changes", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return true;
                            case (int)Types.ErrorCode.SQLVIOLATION:
                                MessageBox.Show("This Question order is already in use\nTry using another one", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                break;
                            case (int)Types.ErrorCode.ERROR:
                                MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                        Logger.LogError(ex); //write error to log file
                    }
                }
                else
                {
                    MessageBox.Show("Question text\nStart Value caption\nEnd Value caption\nCan NOT be empty\n\nMake sure that Max Value is larger than Min Value", "ErrorCode", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
                return false;
            }
        } //end of function

        /// <summary>
        /// Edit a star question through DbConnect Class
        /// </summary>
        private bool EditStarQuestion()
        {
            try
            {
                int questionId, questionOrder, numberOfStars;
                string questionText;
                int result;

                if (CheckStarQuestionInputFields())
                {
                    ///
                    /// Try to Update a new question into "Star_Questions" table
                    ///
                    try
                    {
                        questionId = QuestionId;
                        questionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
                        questionText = questionTextRichTextBox.Text;
                        numberOfStars = Convert.ToInt32(genericNumericUpDown1.Value);

                        StarQuestion starQuestion = new StarQuestion(questionId, questionOrder, questionText, (int)Types.Question.STAR, numberOfStars);
                        QuestionManager questionManager = new QuestionManager();
                        result = questionManager.EditStarQuestion(starQuestion);

                        switch (result)
                        {
                            case (int)Types.ErrorCode.SUCCESS:
                                MessageBox.Show("Question updated successfully\nPress OK to see changes", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return true;
                            case (int)Types.ErrorCode.SQLVIOLATION:
                                MessageBox.Show("This Question order is already in use\nTry using another one", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                break;
                            case (int)Types.ErrorCode.ERROR:
                                MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                        Logger.LogError(ex); //write error to log file
                    }
                }
                else
                {
                    MessageBox.Show("Question Text Can NOT be empty", "ErrorCode", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
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
        //        MessageBox.Show("Something wrong happened, please try again\n", "ErrorCode", MessageBoxButtons.OKCancel, MessageBoxIcon.ErrorCode);
        //        Logger.LogError(ex);
        //    }
        //}

    }
}
