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
        private ListViewColumnSorter lvwColumnSorter;

        #endregion

        #region Constructor
        public SurveyQuestionsConfiguratorForm()
        {
            InitializeComponent();


            /// <summary>
            /// Create an instance of a ListView column sorter and assign it
            /// to the ListView control.
            /// </summary>
            lvwColumnSorter = new ListViewColumnSorter();
            this.createdQuestions_ListView.ListViewItemSorter = lvwColumnSorter;
        }

        #endregion

        #region Functions

        public void ClearViewList()
        {
            foreach (ListViewItem item in createdQuestions_ListView.Items)
            {
                item.Remove();
            }
        }


        /// <summary>
        /// Build List View When Needed (On ADD, EDIT, ...etc)
        /// </summary>
        public void BuildListView()
        {
            /// <summary>
            /// Connect to Quesion table
            /// And Fill the List View
            /// </summary>
            try
            {
                ListViewItem listviewitem;// Used for creating listview items.
                List<Question> questionsList = new List<Question>();
                QuestionManager questionManager = new QuestionManager();

                ///Hide ID Column
                createdQuestions_ListView.Columns[0].Width = 0;

                ///size Text column header to fit the column header text.
                this.createdQuestions_ListView.Columns[3].Width = -2;

                ErrorCode result = questionManager.GetAllQuestions(ref questionsList);
                switch (result)
                {
                    case Generic.ErrorCode.SUCCESS:
                        {
                            if (!addQuestionButton.Enabled)
                            {
                                createdQuestions_ListView.Enabled = true;
                                addQuestionButton.Enabled = true;
                                editQuestionButton.Enabled = true;
                                deleteQuestionButton.Enabled = true;

                                errorLabel.Visible = false;
                            }

                            ClearViewList();

                            foreach (Question q in questionsList)
                            {
                                //listviewitem = new ListViewItem($"{Question.Question.SMILEY}");
                                listviewitem = new ListViewItem($"{q.ID}");
                                listviewitem.SubItems.Add($"{q.Order}");
                                listviewitem.SubItems.Add($"{(Generic.QuestionType)q.Type}");
                                listviewitem.SubItems.Add($"{q.Text}");
                                this.createdQuestions_ListView.Items.Add(listviewitem);
                            }
                        }
                        break;
                    case Generic.ErrorCode.SQL_VIOLATION:
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

                    case Generic.ErrorCode.ERROR:
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); //write error to log file
            }
        } //end func.

        #endregion

        #region Event Handlers
        private void SurveyQuestionsConfiguratorForm_Load(object sender, EventArgs e)
        {
            try
            {
                ///
                /// Build List View on load
                ///
                BuildListView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        }//end event 

        /// <summary>
        /// Refresh When Add Question Form Is Closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        }//end event 

        /// <summary>
        /// Sort List For Each Column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createdQuestions_ListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                /// Determine if clicked column is already the column that is being sorted.
                if (e.Column == lvwColumnSorter.SortColumn)
                {
                    // Reverse the current sort direction for this column.
                    ///
                    /// before : SortOrder 
                    /// after  : System.Windows.Forms.SortOrder
                    /// Slove error :
                    /// ErrorCode CS0104	'SortOrder' is an ambiguous reference between 'System.Windows.Forms.SortOrder' and 'System.Data.SqlClient.SortOrder'
                    ///
                    if (lvwColumnSorter.Order == System.Windows.Forms.SortOrder.Ascending)
                    {
                        lvwColumnSorter.Order = System.Windows.Forms.SortOrder.Descending;
                    }
                    else
                    {
                        lvwColumnSorter.Order = System.Windows.Forms.SortOrder.Ascending;
                    }
                }
                else
                {
                    // Set the column number that is to be sorted; default to ascending.
                    lvwColumnSorter.SortColumn = e.Column;
                    lvwColumnSorter.Order = System.Windows.Forms.SortOrder.Ascending;
                }

                // Perform the sort with these new sort options.
                this.createdQuestions_ListView.Sort();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n");
                Logger.LogError(ex);
            }
        }//end event 

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
        }//end event 

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
        }//end event 

        private void deleteQuestionButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (createdQuestions_ListView.SelectedItems.Count > 0) //If at least one question is selected
                {
                    var selectedItem = createdQuestions_ListView.SelectedItems[0]; // save selected item before it is unchecked by dialog box
                    ///
                    ///Display confirmation dilaog first
                    ///
                    var confirmResult = MessageBox.Show("Are you sure to delete this item ??", "Confirm Delete!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (confirmResult == DialogResult.Yes)
                    {
                        int questionId; // question to be deleted
                        ErrorCode result;
                        ///
                        /// Check the type of the question to be deleted
                        /// Choose appropriate table to query
                        ///
                        questionId = Convert.ToInt32(selectedItem.SubItems[0].Text);
                        QuestionManager questionManager = new QuestionManager();
                        result = questionManager.DeleteQuestionByID(questionId);

                        createdQuestions_ListView.SelectedIndices.Clear(); /// unselect item -> avoid errors
                        switch (result)
                        {
                            case Generic.ErrorCode.SUCCESS:
                                BuildListView();
                                break;
                            case Generic.ErrorCode.SQL_VIOLATION:
                                MessageBox.Show("Something wrong happened\nPlease try again or contact your system administrator", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            case Generic.ErrorCode.ERROR:
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
        }//end event 

        private void refreshDataButton_Click(object sender, EventArgs e)
        {
            try
            {
                ///
                /// Rebuild List View when refresh button is pressed
                ///
                BuildListView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        }//end event 

        private void editQuestionButton_Click(object sender, EventArgs e)
        {
            try
            {
                AddQuestionForm addQuestionForm = null;
                if (createdQuestions_ListView.SelectedIndices.Count > 0) //If at least one question is selected
                {
                    int questionId = Convert.ToInt32(createdQuestions_ListView.SelectedItems[0].SubItems[0].Text);
                    if (createdQuestions_ListView.SelectedItems[0].SubItems[2].Text == Generic.QuestionType.SMILEY.ToString())
                    {
                        addQuestionForm = new AddQuestionForm(questionId, Generic.QuestionType.SMILEY);
                        addQuestionForm.ShowDialog();
                    }
                    else if (createdQuestions_ListView.SelectedItems[0].SubItems[2].Text.ToString() == Generic.QuestionType.SLIDER.ToString())
                    {
                        addQuestionForm = new AddQuestionForm(questionId, Generic.QuestionType.SLIDER);
                        addQuestionForm.ShowDialog();
                    }
                    else if (createdQuestions_ListView.SelectedItems[0].SubItems[2].Text.ToString() == Generic.QuestionType.STAR.ToString())
                    {
                        addQuestionForm = new AddQuestionForm(questionId, Generic.QuestionType.STAR);
                        addQuestionForm.ShowDialog();
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
        }//end event 

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
        }//end event 

        #endregion
    }
}
