using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.Entities;
using SurveyQuestionsConfigurator.QuestionLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SurveyQuestionsConfigurator.Entities.Generic;


namespace SurveyQuestionsConfigurator
{
    public partial class SurveyQuestionsConfiguratorForm : Form
    {
        #region Properties & Attributes

        private readonly ResourceManager mLocalResourceManager;
        private readonly CultureInfo mDefaultCulture;
        private readonly ListViewColumnSorter mListViewColumnSorter; /// Used for sorting listview columns on click
        private readonly QuestionManager mQuestionManager;

        /// <summary>
        /// All translatable message box messages in the "AddQuestionFormStrings" resource file
        /// </summary>
        private enum ResourceStrings
        {
            areYouSureToDeleteThisItem,
            confirmDelete,
            connecting,
            contactSystemAdministratorError,
            emptyString,
            error,
            noItemSelected,
            pleaseSelectAnItemFirst,
            youAreOffilneError,
            somethingWrongHappenedError,
            questionDoesNotExist,
            SMILEY,
            SLIDER,
            STAR
        }
        #endregion

        #region Constructor
        public SurveyQuestionsConfiguratorForm()
        {
            try
            {
                mDefaultCulture = new CultureInfo(ConfigurationManager.AppSettings["DefaultCulture"]);
                Thread.CurrentThread.CurrentUICulture = mDefaultCulture;
                mLocalResourceManager = new ResourceManager("SurveyQuestionsConfigurator.SurvayQuestionFormStrings", typeof(SurveyQuestionsConfiguratorForm).Assembly);

                InitializeComponent();

                EnterOfflineMode(mLocalResourceManager.GetString($"{ResourceStrings.connecting}"));

                mQuestionManager = new QuestionManager();

                mListViewColumnSorter = new ListViewColumnSorter(); /// Create an instance of a ListView column sorter and assign it to the ListView control.
                this.createdQuestions_ListView.ListViewItemSorter = mListViewColumnSorter;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); ///write error to log file
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clear list view by deleting all of it's rows
        /// </summary>
        public void ClearListView()
        {
            /// Remove each row
            try
            {
                /// Perform thread safe call to prevent Cross-thread operation exception
                if (createdQuestions_ListView.InvokeRequired)
                {
                    createdQuestions_ListView.Invoke(new Action(() =>
                    {
                        foreach (ListViewItem item in createdQuestions_ListView.Items)
                        {
                            item.Remove();
                        }
                    }));
                }
                else
                {
                    foreach (ListViewItem item in createdQuestions_ListView.Items)
                    {
                        item.Remove();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); ///write error to log file
            }
        }

        /// <summary>
        /// This method gets called from QuestionManager(BLL)
        /// Refill list view with data when needed (on Add, Edit, Refresh, ...etc)
        /// </summary>
        /// <param name="stateInfo">
        /// Essential for allowing threads to call this function
        /// </param>
        public void BuildListView(ErrorCode pErrorCode, List<Question> pQuestionList)
        {
            /// Connect to quesion table and fill the list view
            try
            {


                ErrorCode tResult = ErrorCode.ERROR;

                List<Question> tQuestionsList = (List<Question>)stateInfo;
                if (tQuestionsList != null)
                {
                    /// Show data from current list
                    tResult = ErrorCode.SUCCESS;
                }
                else if (stateInfo == null)
                {
                    /// Show data from DB
                    tQuestionsList = new List<Question>();
                    tResult = mQuestionManager.GetAllQuestions(ref tQuestionsList);
                }

                switch (tResult)
                {
                    ///If connectin to DB is SUCCESS -> Enable buttons and list view
                    case ErrorCode.SUCCESS:
                        {
                            /// Prevent EnterOnlineMode() everytime unless DB connection is down
                            if (addQuestionButton.Enabled == false)
                            {
                                EnterOnlineMode();
                            }


                            /// Perform thread safe call to prevent Cross-thread operation exception
                            if (this.createdQuestions_ListView.InvokeRequired)
                            {
                                this.createdQuestions_ListView.Invoke(new Action(() =>
                               {
                                   FillListView(tQuestionsList);
                               }));
                            }
                            else
                            {
                                FillListView(tQuestionsList);
                            }
                            break;
                        }

                    ///If connectin to DB is NOT SUCCESS -> Disable buttons and list view
                    default:
                        {
                            string tOfflineMessage = mLocalResourceManager.GetString($"{ResourceStrings.youAreOffilneError}");
                            EnterOfflineMode(tOfflineMessage);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); ///write error to log file
            }
        } ///End Function.

        ///<summary>
        /// Fill list view with passed data
        /// </summary>
        private void FillListView(List<Question> pQuestionsList)
        {
            try
            {
                ClearListView();

                ListViewItem tListviewitem;/// Used for creating listview items.

                /// Perform thread safe call to prevent Cross-thread operation exception
                if (this.createdQuestions_ListView.InvokeRequired)
                {
                    this.createdQuestions_ListView.Invoke(new Action(() =>
                    {

                        foreach (Question q in pQuestionsList)
                        {
                            string tQuestionType = mLocalResourceManager.GetString($"{(QuestionType)q.Type}");

                            /// Add id as a tag to Order column -> use it while it's hidden
                            tListviewitem = new ListViewItem($"{q.Order}")
                            {
                                Tag = q.ID /// SubItems[0].Tag
                            };
                            tListviewitem.SubItems.Add($"{tQuestionType}");

                            tListviewitem.SubItems[1].Tag = q.Type; /// Save question type in Type's column tag => Decouple what is shown in UI from actual data
                            tListviewitem.SubItems.Add($"{q.Text}");

                            this.createdQuestions_ListView.Items.Add(tListviewitem); /// add whole row
                        }
                    }));
                }
                else
                {
                    foreach (Question q in pQuestionsList)
                    {
                        string tQuestionType = mLocalResourceManager.GetString($"{(QuestionType)q.Type}");

                        /// Add id as a tag to Order column -> use it while it's hidden
                        tListviewitem = new ListViewItem($"{q.Order}")
                        {
                            Tag = q.ID /// SubItems[0].Tag
                        };
                        tListviewitem.SubItems.Add($"{tQuestionType}");

                        tListviewitem.SubItems[1].Tag = q.Type; /// Save question type in Type's column tag => Decouple what is shown in UI from actual data
                        tListviewitem.SubItems.Add($"{q.Text}");

                        this.createdQuestions_ListView.Items.Add(tListviewitem); /// add whole row
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage.Box($"{ResourceStrings.somethingWrongHappenedError}", $"{ResourceStrings.error}", MessageBoxButtons.OK, MessageBoxIcon.Error, mLocalResourceManager, mDefaultCulture);
                Logger.LogError(ex);
            }
        }

        /// <summary>
        /// Set the form into offline mode where user can't do any action that can interact with DB except changing the connection settings
        /// </summary>
        /// <param name="pOfflineMeesage">
        /// Pass message to be displayed to error label
        /// </param>
        private void EnterOfflineMode(string pOfflineMeesage)
        {
            try
            {
                /// Perform thread safe call to prevent Cross-thread operation exception
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        errorLabel.Text = pOfflineMeesage;

                        addQuestionButton.Enabled = false;
                        editQuestionButton.Enabled = false;
                        deleteQuestionButton.Enabled = false;

                        ClearListView();
                        createdQuestions_ListView.Enabled = false;
                    }));
                }
                else
                {
                    errorLabel.Text = pOfflineMeesage;

                    addQuestionButton.Enabled = false;
                    editQuestionButton.Enabled = false;
                    deleteQuestionButton.Enabled = false;

                    ClearListView();
                    createdQuestions_ListView.Enabled = false;
                }

                /// Move error label 5 pixels to fit all message in a neat way
                if (pOfflineMeesage == mLocalResourceManager.GetString($"{ResourceStrings.youAreOffilneError}"))
                {
                    if (errorLabel.InvokeRequired)
                    {
                        errorLabel.Invoke(new Action(() =>
                        {
                            errorLabel.Top = 309;
                        }
                        ));
                    }
                    else
                    {
                        errorLabel.Top = 309;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw; /// Throw error to show it on a higher level function
            }
        } ///End Function.

        ///<summary>
        /// Reset Form to online mode where the user can interact with it normally
        /// </summary>
        private void EnterOnlineMode()
        {
            try
            {
                /// Perform thread safe call to prevent Cross-thread operation exception
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        errorLabel.Text = "";

                        createdQuestions_ListView.Enabled = true;
                        addQuestionButton.Enabled = true;
                        editQuestionButton.Enabled = true;
                        deleteQuestionButton.Enabled = true;
                    }));
                }
                else
                {
                    errorLabel.Text = "";

                    createdQuestions_ListView.Enabled = true;
                    addQuestionButton.Enabled = true;
                    editQuestionButton.Enabled = true;
                    deleteQuestionButton.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw; /// Throw error to show it on a higher level function
            }
        } ///End Function.

        #endregion

        #region Event Handlers
        private void SurveyQuestionsConfiguratorForm_Load(object sender, EventArgs e)
        {
            try
            {
                QuestionManager.refreshDataEvent += BuildListView; /// Add BuildListView() refernce to event
                //mQuestionManager.InstantlyRefreshList(); /// Get show data from DB at load
                mQuestionManager.WatchForChanges(); /// Subscribe to data changes event

                //Thread th = new Thread(() =>
                //{
                //    mQuestionManager.WatchForChanges();
                //});
                //th.Start();
                //mQuestionManager.WatchForChanges();
                //ThreadPool.QueueUserWorkItem(mQuestionManager.WatchForChanges);
                /// Build List View on load
                //BuildListView();
                //ThreadPool.QueueUserWorkItem(BuildListView);
            }
            catch (Exception ex)
            {
                ShowMessage.Box($"{ResourceStrings.somethingWrongHappenedError}", $"{ResourceStrings.error}", MessageBoxButtons.OK, MessageBoxIcon.Error, mLocalResourceManager, mDefaultCulture);
                Logger.LogError(ex);
            }
        }///End event 

        /// <summary>
        /// Refresh when add question form is closed
        /// Or when this form is activated
        /// </summary>
        private void SurveyQuestionsConfiguratorForm_Activated(object sender, EventArgs e)
        {
            try
            {
                //Thread thr = new Thread(new ThreadStart(BuildListView));
                //thr.Start();
                //BuildListView();
                //ThreadPool.QueueUserWorkItem(BuildListView);
            }
            catch (Exception ex)
            {
                ShowMessage.Box($"{ResourceStrings.somethingWrongHappenedError}", $"{ResourceStrings.error}", MessageBoxButtons.OK, MessageBoxIcon.Error, mLocalResourceManager, mDefaultCulture);
                Logger.LogError(ex);
            }
        }///End event 

        /// <summary>
        /// Sort list for each column
        /// </summary>
        private void createdQuestions_ListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                /// Determine if clicked column is already the column that is being sorted.
                if (e.Column == mListViewColumnSorter.SortColumn)
                {
                    /// Reverse the current sort direction for this column.
                    if (mListViewColumnSorter.Order == System.Windows.Forms.SortOrder.Ascending)
                    {
                        mListViewColumnSorter.Order = System.Windows.Forms.SortOrder.Descending;
                    }
                    else
                    {
                        mListViewColumnSorter.Order = System.Windows.Forms.SortOrder.Ascending;
                    }
                }
                else
                {
                    /// Set the column number that is to be sorted; default to ascending.
                    mListViewColumnSorter.SortColumn = e.Column;
                    mListViewColumnSorter.Order = System.Windows.Forms.SortOrder.Ascending;
                }

                /// Perform the sort with these new sort options.
                this.createdQuestions_ListView.Sort();
            }
            catch (Exception ex)
            {
                ShowMessage.Box($"{ResourceStrings.somethingWrongHappenedError}", $"{ResourceStrings.error}", MessageBoxButtons.OK, MessageBoxIcon.Error, mLocalResourceManager, mDefaultCulture);
                Logger.LogError(ex);
            }
        }///End event 

        /// <summary>
        /// Handle add question button click
        /// </summary>
        private void addQuestionButton_Click(object sender, EventArgs e)
        {
            try
            {
                AddQuestionForm addQuestionForm = new AddQuestionForm();
                DialogResult tResult = addQuestionForm.ShowDialog(this);
                if (tResult == DialogResult.OK)
                {
                    mQuestionManager.InstantlyRefreshList();
                }
            }
            catch (Exception ex)
            {
                ShowMessage.Box($"{ResourceStrings.somethingWrongHappenedError}", $"{ResourceStrings.error}", MessageBoxButtons.OK, MessageBoxIcon.Error, mLocalResourceManager, mDefaultCulture);
                Logger.LogError(ex);
            }
        }///End event 

        /// <summary>
        /// Handle exit click
        /// </summary>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Exit();
            }
            catch (Exception ex)
            {
                ShowMessage.Box($"{ResourceStrings.somethingWrongHappenedError}", $"{ResourceStrings.error}", MessageBoxButtons.OK, MessageBoxIcon.Error, mLocalResourceManager, mDefaultCulture);
                Logger.LogError(ex);
            }
        }///End event 

        /// <summary>
        /// Handle delete question click
        /// </summary>
        private void deleteQuestionButton_Click(object sender, EventArgs e)
        {
            try
            {
                ///If at least one question is selected
                if (createdQuestions_ListView.SelectedItems.Count > 0)
                {
                    /// Save selected item before it is unchecked by dialog box (prevent error)
                    var selectedItem = createdQuestions_ListView.SelectedItems[0];

                    ///Display confirmation dilaog first

                    ///
                    var confirmResult = ShowMessage.Box($"{ResourceStrings.areYouSureToDeleteThisItem}", $"{ResourceStrings.confirmDelete}", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, mLocalResourceManager, mDefaultCulture);
                    if (confirmResult == DialogResult.Yes)
                    {
                        int tQuestionId; /// question to be deleted
                        ErrorCode result;

                        /// Get ID from hidden ID column
                        tQuestionId = Convert.ToInt32(selectedItem.Tag);
                        result = mQuestionManager.DeleteQuestionByID(tQuestionId);

                        /// unselect item -> avoid errors
                        createdQuestions_ListView.SelectedIndices.Clear();
                        switch (result)
                        {
                            case ErrorCode.SUCCESS:
                                mQuestionManager.InstantlyRefreshList();
                                break;

                            case ErrorCode.EMPTY:
                                ShowMessage.Box($"{ResourceStrings.questionDoesNotExist}", $"{ResourceStrings.error}", MessageBoxButtons.OK, MessageBoxIcon.Error, mLocalResourceManager, mDefaultCulture);
                                break;

                            default:
                                ShowMessage.Box($"{ResourceStrings.contactSystemAdministratorError}", $"{ResourceStrings.error}", MessageBoxButtons.OK, MessageBoxIcon.Error, mLocalResourceManager, mDefaultCulture);
                                break;
                        }
                    }
                }
                else
                {
                    ShowMessage.Box($"{ResourceStrings.pleaseSelectAnItemFirst}", $"{ResourceStrings.noItemSelected}", MessageBoxButtons.OK, MessageBoxIcon.Warning, mLocalResourceManager, mDefaultCulture);
                }
            }
            catch (Exception ex)
            {
                ShowMessage.Box($"{ResourceStrings.somethingWrongHappenedError}", $"{ResourceStrings.error}", MessageBoxButtons.OK, MessageBoxIcon.Error, mLocalResourceManager, mDefaultCulture);
                Logger.LogError(ex);
            }
        }///End event 

        /// <summary>
        /// Handle reresh button click
        /// </summary>
        private void refreshDataButton_Click(object sender, EventArgs e)
        {
            try
            {
                /// Rebuild List View when refresh button is pressed
                //BuildListView();
                mQuestionManager.InstantlyRefreshList();
                //Thread t = new Thread(BuildListView)
                //{
                //    IsBackground = true
                //};
                //t.Start();
            }
            catch (Exception ex)
            {
                ShowMessage.Box($"{ResourceStrings.somethingWrongHappenedError}", $"{ResourceStrings.error}", MessageBoxButtons.OK, MessageBoxIcon.Error, mLocalResourceManager, mDefaultCulture);
                Logger.LogError(ex);
            }
        }///End event 

        /// <summary>
        /// Handle edit question button click
        /// </summary>
        private void editQuestionButton_Click(object sender, EventArgs e)
        {
            try
            {
                AddQuestionForm tAddQuestionForm = null; /// Accept Qustion object
                DialogResult tFormDialogResult = DialogResult.None; /// Used to instantly refresh list if a question was added or edited successfuly
                if (createdQuestions_ListView.SelectedIndices.Count > 0) /// If at least one question is selected
                {
                    int tQuestionId = Convert.ToInt32(createdQuestions_ListView.SelectedItems[0].Tag);
                    Question tQuestion = null;
                    if (createdQuestions_ListView.SelectedItems[0].SubItems[1].Tag.ToString() == QuestionType.SMILEY.ToString())
                    {
                        tQuestion = new Question(tQuestionId, QuestionType.SMILEY);

                        tAddQuestionForm = new AddQuestionForm(tQuestion);
                        tFormDialogResult = tAddQuestionForm.ShowDialog();
                    }
                    else if (createdQuestions_ListView.SelectedItems[0].SubItems[1].Tag.ToString() == QuestionType.SLIDER.ToString())
                    {
                        tQuestion = new Question(tQuestionId, QuestionType.SLIDER);

                        tAddQuestionForm = new AddQuestionForm(tQuestion);
                        tFormDialogResult = tAddQuestionForm.ShowDialog();
                    }
                    else if (createdQuestions_ListView.SelectedItems[0].SubItems[1].Tag.ToString() == QuestionType.STAR.ToString())
                    {
                        tQuestion = new Question(tQuestionId, QuestionType.STAR);

                        tAddQuestionForm = new AddQuestionForm(tQuestion);
                        tFormDialogResult = tAddQuestionForm.ShowDialog();
                    }

                    if (tFormDialogResult == DialogResult.OK)
                    {
                        mQuestionManager.InstantlyRefreshList();
                    }
                }
                else
                {
                    ShowMessage.Box($"{ResourceStrings.pleaseSelectAnItemFirst}", $"{ResourceStrings.noItemSelected}", MessageBoxButtons.OK, MessageBoxIcon.Warning, mLocalResourceManager, mDefaultCulture);
                }
            }
            catch (Exception ex)
            {
                ShowMessage.Box($"{ResourceStrings.somethingWrongHappenedError}", $"{ResourceStrings.error}", MessageBoxButtons.OK, MessageBoxIcon.Error, mLocalResourceManager, mDefaultCulture);
                Logger.LogError(ex);
            }
        }///End event 

        /// <summary>
        /// Show change connection string settings form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void settingsButton_Click(object sender, EventArgs e)
        {
            try
            {
                ConnectionSettingsForm connectionSettingsForm = new ConnectionSettingsForm();
                DialogResult tResult = connectionSettingsForm.ShowDialog();
                if (tResult == DialogResult.OK)
                {
                    mQuestionManager.InstantlyRefreshList();
                }
            }
            catch (Exception ex)
            {
                ShowMessage.Box($"{ResourceStrings.somethingWrongHappenedError}", $"{ResourceStrings.error}", MessageBoxButtons.OK, MessageBoxIcon.Error, mLocalResourceManager, mDefaultCulture);
                Logger.LogError(ex);
            }
        }

        /// <summary>
        /// Handle close button click
        /// </summary>
        private void closeApplicationButton_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Exit();
            }
            catch (Exception ex)
            {
                ShowMessage.Box($"{ResourceStrings.somethingWrongHappenedError}", $"{ResourceStrings.error}", MessageBoxButtons.OK, MessageBoxIcon.Error, mLocalResourceManager, mDefaultCulture);
                Logger.LogError(ex);
            }
        }///End event 
        #endregion

        private void languageSettingsButton_Click(object sender, EventArgs e)
        {
            try
            {
                LangaugeSettingsForm langaugeSettingsForm = new LangaugeSettingsForm();
                langaugeSettingsForm.ShowDialog();
            }
            catch (Exception ex)
            {
                ShowMessage.Box($"{ResourceStrings.somethingWrongHappenedError}", $"{ResourceStrings.error}", MessageBoxButtons.OK, MessageBoxIcon.Error, mLocalResourceManager, mDefaultCulture);
                Logger.LogError(ex);
            }
        }
    }
}
