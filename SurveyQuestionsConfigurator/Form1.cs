using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SurveyQuestionsConfigurator
{
    public partial class Form1 : Form
    {
        /* 
       * Global Variables
       * Access them anywhere
       * */
        private ConnectionStringSettings cn = ConfigurationManager.ConnectionStrings["cn"]; //get connection string information from App.config
        private SqlConnection conn = null; // Create SqlConnection object to connect to DB

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

        public Form1()
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

            SqlDataReader reader = null;

            /*
             * Connect to every Quesion Type table
             * And Fill the List View
             */
            try
            {
                conn = new SqlConnection(cn.ConnectionString);
                SqlCommand cmd = new SqlCommand("select * from Smiley_Questions", conn);
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read()) //Read every retrieved record
                {
                    listviewitem = new ListViewItem($"{QuestionType.SMILEY}");
                    listviewitem.SubItems.Add($"{reader[0]}");
                    listviewitem.SubItems.Add($"{reader[1]}");
                    listviewitem.SubItems.Add($"{reader[2]}");
                    listviewitem.SubItems.Add($"{reader[3]}");
                    this.createdQuestions_ListView.Items.Add(listviewitem);
                }
                reader.Close();

                cmd = new SqlCommand("select * from Slider_Questions", conn);
                reader = cmd.ExecuteReader();
                while (reader.Read()) //Read every retrieved record
                {
                    listviewitem = new ListViewItem($"{QuestionType.SLIDER}");
                    listviewitem.SubItems.Add($"{reader[0]}");
                    listviewitem.SubItems.Add($"{reader[1]}");
                    listviewitem.SubItems.Add($"{reader[2]}");
                    listviewitem.SubItems.Add($"{reader[3]}");
                    this.createdQuestions_ListView.Items.Add(listviewitem);
                }
                reader.Close();


                cmd = new SqlCommand("select * from Star_Questions", conn);
                reader = cmd.ExecuteReader();
                while (reader.Read()) //Read every retrieved record
                {

                    listviewitem = new ListViewItem($"{QuestionType.STAR}");
                    listviewitem.SubItems.Add($"{reader[0]}");
                    listviewitem.SubItems.Add($"{reader[1]}");
                    listviewitem.SubItems.Add($"{reader[2]}");
                    listviewitem.SubItems.Add($"{reader[3]}");
                    this.createdQuestions_ListView.Items.Add(listviewitem);
                }
                reader.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL Error:\n" + ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong:\n" + ex);
            }
            finally
            {
                reader.Close();
                conn.Close();
            }

            // Loop through and size each column header to fit the column header text.
            foreach (ColumnHeader ch in this.createdQuestions_ListView.Columns)
            {
                ch.Width = -2;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
            if (createdQuestions_ListView.SelectedIndices.Count > 0) //If at least one question is selected
            { // general condition

                //Display confirmation dilaog first
                var confirmResult = MessageBox.Show("Are you sure to delete this item ??", "Confirm Delete!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirmResult == DialogResult.Yes)
                {
                    int questionId = 0; // question to be deleted

                    /*
                     * Check the type of the question to be deleted
                     * Choose appropriate table to query
                     */
                    if (createdQuestions_ListView.SelectedItems[0].Text == QuestionType.SMILEY.ToString()) //SMILEY Question
                    {
                        try
                        {
                            conn = new SqlConnection(cn.ConnectionString);
                            questionId = Convert.ToInt32(createdQuestions_ListView.SelectedItems[0].SubItems[1].Text);
                            SqlCommand cmd = new SqlCommand($@"delete from Smiley_Questions where QuestionID = {questionId};", conn);
                            conn.Open();
                            int affectedRows = cmd.ExecuteNonQuery();
                            if (affectedRows > 0)
                            {
                                /*
                                 * Rebuild List View after each deletion
                                 */
                                BuildListView();
                                MessageBox.Show("Question Deleted");
                            }
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("SQL Error\n\n" + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Somthing went wrong:\n\n" + ex.Message);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                    else if (createdQuestions_ListView.SelectedItems[0].Text == QuestionType.SLIDER.ToString()) //Slider Question
                    {
                        try
                        {
                            conn = new SqlConnection(cn.ConnectionString);
                            questionId = Convert.ToInt32(createdQuestions_ListView.SelectedItems[0].SubItems[1].Text);
                            SqlCommand cmd = new SqlCommand($@"delete from Slider_Questions where QuestionID = {questionId};", conn);
                            conn.Open();
                            int affectedRows = cmd.ExecuteNonQuery();
                            if (affectedRows > 0)
                            {
                                /*
                                 * Rebuild List View after each deletion
                                 */
                                BuildListView();
                                MessageBox.Show("Question Deleted");
                            }
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("SQL Error\n\n" + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Somthing went wrong:\n\n" + ex.Message);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                    else if ((createdQuestions_ListView.SelectedItems[0].Text == QuestionType.STAR.ToString())) //Star Question
                    {
                        try
                        {
                            conn = new SqlConnection(cn.ConnectionString);
                            questionId = Convert.ToInt32(createdQuestions_ListView.SelectedItems[0].SubItems[1].Text);
                            SqlCommand cmd = new SqlCommand($@"delete from Star_Questions where QuestionID = {questionId};", conn);
                            conn.Open();
                            int affectedRows = cmd.ExecuteNonQuery();
                            if (affectedRows > 0)
                            {
                                /*
                                 * Rebuild List View after each deletion
                                 */
                                BuildListView();
                                MessageBox.Show("Question Deleted");
                            }
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("SQL Error\n\n" + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Somthing went wrong:\n\n" + ex.Message);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an item first", "No selected item", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }//end event 

        private void refreshDataButton_Click(object sender, EventArgs e)
        {
            /*
            * Rebuild List View when refresh button is pressed
            */
            BuildListView();
        }

        private void editQuestionButton_Click(object sender, EventArgs e)
        {
            AddQuestionForm form2 = null;
            if (createdQuestions_ListView.SelectedIndices.Count > 0) //If at least one question is selected
            {
                int questionId = Convert.ToInt32(createdQuestions_ListView.SelectedItems[0].SubItems[1].Text);
                /*
                 * Choose Editing Form constructor based on Question's Type
                 */
                if (createdQuestions_ListView.SelectedItems[0].Text == QuestionType.SMILEY.ToString())
                {
                    form2 = new AddQuestionForm((int)QuestionType.SMILEY, questionId, QuestionType.SMILEY.ToString());
                }
                else if (createdQuestions_ListView.SelectedItems[0].Text == QuestionType.SLIDER.ToString())
                {
                    form2 = new AddQuestionForm((int)QuestionType.SLIDER, questionId, QuestionType.SLIDER.ToString());
                }
                else if (createdQuestions_ListView.SelectedItems[0].Text == QuestionType.STAR.ToString())
                {
                    form2 = new AddQuestionForm((int)QuestionType.STAR, questionId, QuestionType.STAR.ToString());
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
            conn.Close();
        }
    }
}
