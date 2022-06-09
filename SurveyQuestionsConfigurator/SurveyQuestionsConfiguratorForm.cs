using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.Entities;
using SurveyQuestionsConfigurator.QuestionLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SurveyQuestionsConfigurator.Entities.Generic;

namespace SurveyQuestionsConfigurator
{
    public partial class SurveyQuestionsConfiguratorForm : Form
    {
        #region Properties & Attributes

        /// <summary>
        /// Used for sorting listview columns on click
        /// </summary>
        private ListViewColumnSorter mListViewColumnSorter;
        private QuestionManager mGeneralQuestionManager = new QuestionManager();

        #endregion

        #region Constructor
        public SurveyQuestionsConfiguratorForm()
        {
            InitializeComponent();


            /// <summary>
            /// Create an instance of a ListView column sorter and assign it
            /// to the ListView control.
            /// </summary>
            mListViewColumnSorter = new ListViewColumnSorter();
            this.createdQuestions_ListView.ListViewItemSorter = mListViewColumnSorter;

        }

        #endregion

        #region Methods

        public void ClearViewList()
        {
            /// Remove each row
            foreach (ListViewItem item in createdQuestions_ListView.Items)
            {
                item.Remove();
            }
        }

        /// Build list view when needed (on ADD, EDIT, ...etc)
        public void BuildListView()
        {
            /// Connect to quesion table and fill the list view
            try
            {
                ListViewItem listviewitem;// Used for creating listview items.
                List<Question> questionsList = new List<Question>();

                ///Size text column header to fit the text.
                this.createdQuestions_ListView.Columns[2].Width = -2;

                ErrorCode result = mGeneralQuestionManager.GetAllQuestions(ref questionsList);
                switch (result)
                {
                    case ErrorCode.SUCCESS:
                        {
                            ///If connectin to DB is SUCCESS -> Enable buttons and list view
                            if (!addQuestionButton.Enabled)
                            {
                                createdQuestions_ListView.Enabled = true;
                                addQuestionButton.Enabled = true;
                                editQuestionButton.Enabled = true;
                                deleteQuestionButton.Enabled = true;

                                errorLabel.Visible = false;
                            }

                            ClearViewList();

                            ///Fill the list view
                            foreach (Question q in questionsList)
                            {
                                /// Add id as a tag to Order column -> use it while it's hidden
                                listviewitem = new ListViewItem($"{q.Order}");
                                listviewitem.Tag = q.ID;
                                listviewitem.SubItems.Add($"{(QuestionType)q.Type}");
                                listviewitem.SubItems.Add($"{q.Text}");
                                this.createdQuestions_ListView.Items.Add(listviewitem);
                            }
                        }
                        break;
                    default:
                        ///If connectin to DB is ERROR -> Disable buttons and list view
                        if (addQuestionButton.Enabled)
                        {
                            addQuestionButton.Enabled = false;
                            editQuestionButton.Enabled = false;
                            deleteQuestionButton.Enabled = false;

                            errorLabel.Visible = true;
                            errorLabel.Text = "You're Offilne, Please Try Againt Later";
                            createdQuestions_ListView.Enabled = false;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); ///write error to log file
            }
        } ///End Function.

        #endregion

        #region Event Handlers
        private void SurveyQuestionsConfiguratorForm_Load(object sender, EventArgs e)
        {
            try
            {
                /// Build List View on load
                BuildListView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        }//End event 

        /// <summary>
        /// Refresh when add question form is closed
        /// Or when this form is activated
        /// </summary>
        private void SurveyQuestionsConfiguratorForm_Activated(object sender, EventArgs e)
        {
            try
            {
                BuildListView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        }//End event 

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
                MessageBox.Show("Something wrong happened, please try again\n");
                Logger.LogError(ex);
            }
        }//End event 

        /// <summary>
        /// Handle add question button click
        /// </summary>
        private void addQuestionButton_Click(object sender, EventArgs e)
        {
            try
            {
                AddQuestionForm addQuestionForm = new AddQuestionForm();
                addQuestionForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        }//End event 

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
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        }//End event 

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
                    var confirmResult = MessageBox.Show("Are you sure to delete this item ??", "Confirm Delete!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (confirmResult == DialogResult.Yes)
                    {
                        int tQuestionId; /// question to be deleted
                        ErrorCode result;

                        /// Get ID from hidden ID column
                        tQuestionId = Convert.ToInt32(selectedItem.Tag);
                        result = mGeneralQuestionManager.DeleteQuestionByID(tQuestionId);

                        /// unselect item -> avoid errors
                        createdQuestions_ListView.SelectedIndices.Clear();
                        switch (result)
                        {
                            case ErrorCode.SUCCESS:
                                BuildListView();
                                break;
                            case ErrorCode.SQL_VIOLATION:
                                MessageBox.Show("Something wrong happened\nPlease try again or contact your system administrator", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            case ErrorCode.ERROR:
                                MessageBox.Show("Something wrong happened\nPlease try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select an item first", "No item selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        }//End event 

        /// <summary>
        /// Handle reresh button click
        /// </summary>
        private void refreshDataButton_Click(object sender, EventArgs e)
        {
            try
            {
                /// Rebuild List View when refresh button is pressed
                BuildListView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        }//End event 

        /// <summary>
        /// Handle edit question button click
        /// </summary>
        private void editQuestionButton_Click(object sender, EventArgs e)
        {
            try
            {
                AddQuestionForm tAddQuestionForm = null; /// Accept question ID & question type
                if (createdQuestions_ListView.SelectedIndices.Count > 0) //If at least one question is selected
                {
                    int tQuestionId = Convert.ToInt32(createdQuestions_ListView.SelectedItems[0].Tag);
                    if (createdQuestions_ListView.SelectedItems[0].SubItems[1].Text == QuestionType.SMILEY.ToString())
                    {
                        tAddQuestionForm = new AddQuestionForm(tQuestionId, QuestionType.SMILEY);
                        tAddQuestionForm.ShowDialog();
                    }
                    else if (createdQuestions_ListView.SelectedItems[0].SubItems[1].Text.ToString() == QuestionType.SLIDER.ToString())
                    {
                        tAddQuestionForm = new AddQuestionForm(tQuestionId, QuestionType.SLIDER);
                        tAddQuestionForm.ShowDialog();
                    }
                    else if (createdQuestions_ListView.SelectedItems[0].SubItems[1].Text.ToString() == QuestionType.STAR.ToString())
                    {
                        tAddQuestionForm = new AddQuestionForm(tQuestionId, QuestionType.STAR);
                        tAddQuestionForm.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("Please select an item first", "No selected item", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        }//End event 

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
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        }//End event 

        #endregion
    }
}
