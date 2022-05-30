﻿using System;
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
        /// 
        /// Global Variables
        /// Access them anywhere
        ///

        /// 
        ///get connection string information from App.config
        ///
        private ConnectionStringSettings cn = ConfigurationManager.ConnectionStrings[0];

        ///
        /// Question Type Enum to reduce errors
        ///
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
            ///
            /// Create an instance of a ListView column sorter and assign it
            /// to the ListView control.
            ///
            lvwColumnSorter = new ListViewColumnSorter();
            this.createdQuestions_ListView.ListViewItemSorter = lvwColumnSorter;
        }

        public void BuildListView()
        {
            try
            {
                ListViewItem listviewitem;// Used for creating listview items.

                ///
                ///remove all rows -> refresh
                ///
                foreach (ListViewItem item in createdQuestions_ListView.Items)
                {
                    item.Remove();
                }


                DataTable dt = new DataTable();
                ///
                /// Connect to every Quesion Type table
                /// And Fill the List View
                ///
                try
                {
                    dt = CommDB.RetrieveQuestions();
                    foreach (DataRow row in dt.Rows)
                    {
                        //listviewitem = new ListViewItem($"{QuestionType.SMILEY}");
                        listviewitem = new ListViewItem($"{row[0]}");
                        listviewitem.SubItems.Add($"{row[1]}");
                        listviewitem.SubItems.Add($"{row[2]}");
                        listviewitem.SubItems.Add($"{row[3].ToString()}");
                        this.createdQuestions_ListView.Items.Add(listviewitem);
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("SQL Error:\n" + ex);
                    CommonLayer.LogError(ex); //write error to log file
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Something went wrong:\n" + ex);
                    CommonLayer.LogError(ex); //write error to log file
                }

                // Loop through and size each column header to fit the column header text.
                //foreach (ColumnHeader ch in this.createdQuestions_ListView.Columns)
                //{
                //    ch.Width = -2;
                //}
                this.createdQuestions_ListView.Columns[3].Width = -2;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n");
                CommonLayer.LogError(ex);
            }
        } // end func.

        private void Form1_Load(object sender, EventArgs e)
        {
            try
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

                ///
                /// Build List View on load
                ///
                BuildListView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n");
                CommonLayer.LogError(ex);
            }
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                // Determine if clicked column is already the column that is being sorted.
                if (e.Column == lvwColumnSorter.SortColumn)
                {
                    // Reverse the current sort direction for this column.
                    ///
                    /// before : SortOrder 
                    /// after  : System.Windows.Forms.SortOrder
                    /// Slove error :
                    /// Error CS0104	'SortOrder' is an ambiguous reference between 'System.Windows.Forms.SortOrder' and 'System.Data.SqlClient.SortOrder'
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
                CommonLayer.LogError(ex);
            }
        }

        private void addQuestionButton_Click(object sender, EventArgs e)
        {
            try
            {
                AddQuestionForm form2 = new AddQuestionForm();
                form2.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n");
                CommonLayer.LogError(ex);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n");
                CommonLayer.LogError(ex);
            }
        }

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
                        ///
                        /// Check the type of the question to be deleted
                        /// Choose appropriate table to query
                        ///
                        try
                        {
                            questionId = Convert.ToInt32(selectedItem.SubItems[0].Text);
                            int result = CommDB.DeleteQuestion(questionId);
                            createdQuestions_ListView.SelectedIndices.Clear(); /// unselect item -> avoid errors
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
                            CommonLayer.LogError(ex); //write error to log file
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
                MessageBox.Show("Something wrong happened, please try again\n");
                CommonLayer.LogError(ex);
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
                MessageBox.Show("Something wrong happened, please try again\n");
                CommonLayer.LogError(ex);
            }
        }

        private void editQuestionButton_Click(object sender, EventArgs e)
        {
            try
            {
                AddQuestionForm form2 = null;
                if (createdQuestions_ListView.SelectedIndices.Count > 0) //If at least one question is selected
                {
                    int questionId = Convert.ToInt32(createdQuestions_ListView.SelectedItems[0].SubItems[0].Text);
                    //MessageBox.Show(createdQuestions_ListView.SelectedItems[0].SubItems[3].Text.ToLower());
                    if (createdQuestions_ListView.SelectedItems[0].SubItems[2].Text.ToLower() == QuestionType.SMILEY.ToString().ToLower())
                    {
                        form2 = new AddQuestionForm(questionId, QuestionType.SMILEY.ToString());
                    }
                    else if (createdQuestions_ListView.SelectedItems[0].SubItems[2].Text.ToLower() == QuestionType.SLIDER.ToString().ToLower())
                    {
                        form2 = new AddQuestionForm(questionId, QuestionType.SLIDER.ToString());
                    }
                    else if (createdQuestions_ListView.SelectedItems[0].SubItems[2].Text.ToLower() == QuestionType.STAR.ToString().ToLower())
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
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n");
                CommonLayer.LogError(ex);
            }
        }

        private void closeApplicationButton_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n");
                CommonLayer.LogError(ex);
            }
        }

        private void Form1_Leave(object sender, EventArgs e)
        {
            //conn.Close();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            try
            {
                BuildListView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n");
                CommonLayer.LogError(ex);
            }
        }
    }
}
