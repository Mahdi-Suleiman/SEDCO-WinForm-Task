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
    public partial class SurveyQuestionsConfiguratorForm : Form
    {
        /* 
       * Global Variables
       * Access them anywhere
       * */
        //private ConnectionStringSettings cn = ConfigurationManager.ConnectionStrings["SurveyQuestionsConfigurator"]; //get connection string information from App.config
        private ConnectionStringSettings cn = ConfigurationManager.ConnectionStrings[0]; //get connection string information from App.config
        //private SqlConnection conn = null; // Create SqlConnection object to connect to DB

        /*
         * Question Type Enum to reduce any errors 
         */
        public enum QuestionType
        {
            SMILEY,
            SLIDER,
            STAR
        }
        private ListViewColumnSorter lvwColumnSorter;

        public SurveyQuestionsConfiguratorForm()
        {
            InitializeComponent();
            /*
             * Create an instance of a ListView column sorter and assign it
             * to the ListView control.
             */
            lvwColumnSorter = new ListViewColumnSorter();
            this.createdQuestions_ListView.ListViewItemSorter = lvwColumnSorter;
        }

        private void BuildListView()
        {
            ColumnHeader columnheader;// Used for creating column headers.
            ListViewItem listviewitem;// Used for creating listview items.

            createdQuestions_ListView.Clear(); // clear first

            // Ensure that the view is set to show details.
            createdQuestions_ListView.View = View.Details;

            // Create some column headers for the data.
            columnheader = new ColumnHeader();
            columnheader.Text = "Type";
            this.createdQuestions_ListView.Columns.Add(columnheader);

            columnheader = new ColumnHeader();
            columnheader.Text = "ID";
            this.createdQuestions_ListView.Columns.Add(columnheader);

            columnheader = new ColumnHeader();
            columnheader.Text = "Order";
            this.createdQuestions_ListView.Columns.Add(columnheader);

            columnheader = new ColumnHeader();
            columnheader.Text = "Text";
            this.createdQuestions_ListView.Columns.Add(columnheader);


            DataTable dt = new DataTable();
            ///
            /// Connect to every Quesion Type table
            /// And Fill the List View
            ///
            try
            {
                dt = CommDB.RetrieveSmileyQuestions();
                foreach (DataRow row in dt.Rows)
                {
                    listviewitem = new ListViewItem($"{QuestionType.SMILEY}");
                    listviewitem.SubItems.Add($"{row[0]}");
                    listviewitem.SubItems.Add($"{row[1]}");
                    listviewitem.SubItems.Add($"{row[2]}");
                    this.createdQuestions_ListView.Items.Add(listviewitem);
                }

                dt = CommDB.RetrieveSliderQuestions();
                foreach (DataRow row in dt.Rows)
                {
                    listviewitem = new ListViewItem($"{QuestionType.SLIDER}");
                    listviewitem.SubItems.Add($"{row[0]}");
                    listviewitem.SubItems.Add($"{row[1]}");
                    listviewitem.SubItems.Add($"{row[2]}");
                    this.createdQuestions_ListView.Items.Add(listviewitem);
                }

                dt = CommDB.RetrieveStarQuestions();
                foreach (DataRow row in dt.Rows)
                {
                    listviewitem = new ListViewItem($"{QuestionType.STAR}");
                    listviewitem.SubItems.Add($"{row[0]}");
                    listviewitem.SubItems.Add($"{row[1]}");
                    listviewitem.SubItems.Add($"{row[2]}");
                    this.createdQuestions_ListView.Items.Add(listviewitem);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL Error:\n" + ex);
                Trace.TraceError("SQL Error:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong:\n" + ex);
                Trace.TraceError("Something went wrong:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
            }
            finally
            {
                //reader.Close();
                //conn.Close();
            }

            // Loop through and size each column header to fit the column header text.
            foreach (ColumnHeader ch in this.createdQuestions_ListView.Columns)
            {
                ch.Width = -2;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //
            // Check if tables exist
            // If tables do not exit, create them.
            //
            int checkIfTablesExistResult = CommDB.CheckIfTablesExist();
            switch (checkIfTablesExistResult)
            {
                case 1:
                    //MessageBox.Show("Question deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 2:
                    MessageBox.Show("Something wrong happened\nPlease try again or contact your system administrator", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -1:
                    MessageBox.Show("Something wrong happened\nPlease try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }

            /*
            * Build List View on load
            */
            BuildListView();
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                /*
                 * before : SortOrder 
                 * after  : System.Windows.Forms.SortOrder
                 * Slove error :
                 * Error CS0104	'SortOrder' is an ambiguous reference between 'System.Windows.Forms.SortOrder' and 'System.Data.SqlClient.SortOrder'
                 */
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

        private void addQuestionButton_Click(object sender, EventArgs e)
        {
            AddQuestionForm form2 = new AddQuestionForm();
            form2.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void deleteQuestionButton_Click(object sender, EventArgs e)
        {
            if (createdQuestions_ListView.SelectedItems.Count > 0) //If at least one question is selected
            {
                var selectedItem = createdQuestions_ListView.SelectedItems[0]; // save selected item before it is unchecked by dialog box
                //Display confirmation dilaog first
                var confirmResult = MessageBox.Show("Are you sure to delete this item ??", "Confirm Delete!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmResult == DialogResult.Yes)
                {
                    int questionId; // question to be deleted
                    /*
                     * Check the type of the question to be deleted
                     * Choose appropriate table to query
                     */
                    if (selectedItem.Text.ToString() == QuestionType.SMILEY.ToString()) //SMILEY Question
                    {
                        try
                        {
                            questionId = Convert.ToInt32(selectedItem.SubItems[1].Text);
                            int result = CommDB.DeleteSmileyQuestion(questionId);
                            switch (result)
                            {
                                case 1:
                                    BuildListView();
                                    MessageBox.Show("Question deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    break;
                                case 2:
                                    MessageBox.Show("Something wrong happened\nPlease try again or contact your system administrator", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        finally
                        {
                            //conn.Close();
                        }
                        createdQuestions_ListView.SelectedIndices.Clear();

                    }
                    else if (selectedItem.Text.ToString() == QuestionType.SLIDER.ToString()) //Slider Question
                    {
                        try
                        {
                            questionId = Convert.ToInt32(selectedItem.SubItems[1].Text);
                            int result = CommDB.DeleteSliderQuestion(questionId);
                            switch (result)
                            {
                                case 1:
                                    BuildListView();
                                    MessageBox.Show("Question deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    break;
                                case 2:
                                    MessageBox.Show("Something wrong happened\nPlease try again or contact your system administrator", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        finally
                        {
                            //conn.Close();
                        }
                        createdQuestions_ListView.SelectedIndices.Clear();

                    }
                    else if (selectedItem.Text.ToString() == QuestionType.STAR.ToString()) //Star Question
                    {
                        try
                        {
                            questionId = Convert.ToInt32(selectedItem.SubItems[1].Text);
                            int result = CommDB.DeleteStarQuestion(questionId);
                            switch (result)
                            {
                                case 1:
                                    BuildListView();
                                    MessageBox.Show("Question deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    break;
                                case 2:
                                    MessageBox.Show("Something wrong happened\nPlease try again or contact your system administrator", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        finally
                        {
                            //conn.Close();
                        }
                        createdQuestions_ListView.SelectedIndices.Clear();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an item first", "No item selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }//end event 

        private void refreshDataButton_Click(object sender, EventArgs e)
        {
            ///
            /// Rebuild List View when refresh button is pressed
            ///
            BuildListView();
        }

        private void editQuestionButton_Click(object sender, EventArgs e)
        {
            AddQuestionForm form2 = null;
            if (createdQuestions_ListView.SelectedIndices.Count > 0) //If at least one question is selected
            {
                int questionId = Convert.ToInt32(createdQuestions_ListView.SelectedItems[0].SubItems[1].Text);

                if (createdQuestions_ListView.SelectedItems[0].Text == QuestionType.SMILEY.ToString())
                {
                    form2 = new AddQuestionForm(questionId, QuestionType.SMILEY.ToString());
                }
                else if (createdQuestions_ListView.SelectedItems[0].Text == QuestionType.SLIDER.ToString())
                {
                    form2 = new AddQuestionForm(questionId, QuestionType.SLIDER.ToString());
                }
                else if (createdQuestions_ListView.SelectedItems[0].Text == QuestionType.STAR.ToString())
                {
                    form2 = new AddQuestionForm(questionId, QuestionType.STAR.ToString());
                }
                form2.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select an item first", "No selected item", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void closeApplicationButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Leave(object sender, EventArgs e)
        {
            //conn.Close();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            BuildListView();
        }
    }
}
