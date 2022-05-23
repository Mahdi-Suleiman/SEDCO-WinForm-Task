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

            // Create an instance of a ListView column sorter and assign it
            // to the ListView control.
            lvwColumnSorter = new ListViewColumnSorter();
            this.createdQuestions_ListView.ListViewItemSorter = lvwColumnSorter;
        }

        private void BuildListView()
        {
            ColumnHeader columnheader;// Used for creating column headers.
            ListViewItem listviewitem;// Used for creating listview items.
            SqlConnection conn = null;

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

            //columnheader = new ColumnHeader();
            //columnheader.Text = "Number of Smiley Faces";
            //this.listView1.Columns.Add(columnheader);

            //columnheader = new ColumnHeader();
            //columnheader.Text = "Number of stars";
            //this.listView1.Columns.Add(columnheader);

            //columnheader = new ColumnHeader();
            //columnheader.Text = "Start Value";
            //this.listView1.Columns.Add(columnheader);

            //columnheader = new ColumnHeader();
            //columnheader.Text = "Start Value Caption";
            //this.listView1.Columns.Add(columnheader);

            //columnheader = new ColumnHeader();
            //columnheader.Text = "End Value";
            //this.listView1.Columns.Add(columnheader);

            //columnheader = new ColumnHeader();
            //columnheader.Text = "End Value Caption";
            //this.listView1.Columns.Add(columnheader);



            var cn = ConfigurationManager.ConnectionStrings["cn"];
            SqlDataReader reader = null;

            try
            {
                conn = new SqlConnection(cn.ConnectionString);
                SqlCommand cmd = new SqlCommand("select * from Smiley_Questions", conn);
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
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
                while (reader.Read())
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
                while (reader.Read())
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

            // Create some listview items consisting of first and last names.
            //listviewitem = new ListViewItem("John");
            //listviewitem.SubItems.Add("Smith");
            //listviewitem.SubItems.Add("3");
            //this.listView1.Items.Add(listviewitem);

            //listviewitem = new ListViewItem("Bob");
            //listviewitem.SubItems.Add("Taylor");
            //this.listView1.Items.Add(listviewitem);

            //listviewitem = new ListViewItem("Kim");
            //listviewitem.SubItems.Add("Zimmerman");
            //this.listView1.Items.Add(listviewitem);

            //listviewitem = new ListViewItem("Olivia");
            //listviewitem.SubItems.Add("Johnson");
            //this.listView1.Items.Add(listviewitem);

            //this.listView1.Items.Add(`listview`item);


            //this.`listView`1.Columns.Add(columnheader);


            // Loop through and size each column header to fit the column header text.
            foreach (ColumnHeader ch in this.createdQuestions_ListView.Columns)
            {
                ch.Width = -2;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BuildListView();
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                /*
                Error	CS0104	'SortOrder' is an ambiguous reference between 'System.Windows.Forms.SortOrder' and 'System.Data.SqlClient.SortOrder'
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
            if (createdQuestions_ListView.SelectedIndices.Count > 0)
            { // general condition

                //Display confirmation dilaog first
                var confirmResult = MessageBox.Show("Are you sure to delete this item ??", "Confirm Delete!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirmResult == DialogResult.Yes)
                {
                    var cn = ConfigurationManager.ConnectionStrings["cn"]; //get connection string information from App.config

                    SqlConnection conn = null; //create a "method scope" SqlConnection object and reference it to null

                    int questionID = 0; // question to be deleted
                    if (createdQuestions_ListView.SelectedItems[0].Text == QuestionType.SMILEY.ToString())
                    {
                        try
                        {
                            conn = new SqlConnection(cn.ConnectionString);
                            questionID = Convert.ToInt32(createdQuestions_ListView.SelectedItems[0].SubItems[1].Text);
                            SqlCommand cmd = new SqlCommand($@"delete from Smiley_Questions where QuestionID = {questionID};", conn);
                            conn.Open();
                            int affectedRows = cmd.ExecuteNonQuery();
                            if (affectedRows > 0)
                            {
                                //BuildListView();
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
                    else if (createdQuestions_ListView.SelectedItems[0].Text == QuestionType.SLIDER.ToString())
                    {
                        try
                        {
                            conn = new SqlConnection(cn.ConnectionString);
                            questionID = Convert.ToInt32(createdQuestions_ListView.SelectedItems[0].SubItems[1].Text);
                            SqlCommand cmd = new SqlCommand($@"delete from Slider_Questions where QuestionID = {questionID};", conn);
                            conn.Open();
                            int affectedRows = cmd.ExecuteNonQuery();
                            if (affectedRows > 0)
                            {
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
                    else if ((createdQuestions_ListView.SelectedItems[0].Text == QuestionType.STAR.ToString()))
                    {
                        try
                        {
                            conn = new SqlConnection(cn.ConnectionString);
                            questionID = Convert.ToInt32(createdQuestions_ListView.SelectedItems[0].SubItems[1].Text);
                            SqlCommand cmd = new SqlCommand($@"delete from Star_Questions where QuestionID = {questionID};", conn);
                            conn.Open();
                            int affectedRows = cmd.ExecuteNonQuery();
                            if (affectedRows > 0)
                            {
                                //BuildListView();
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
            BuildListView();
        }

        private void editQuestionButton_Click(object sender, EventArgs e)
        {
            AddQuestionForm form2 = null;
            if (createdQuestions_ListView.SelectedIndices.Count > 0)
            {
                int questionId = Convert.ToInt32(createdQuestions_ListView.SelectedItems[0].SubItems[1].Text);
                if (createdQuestions_ListView.SelectedItems[0].Text == QuestionType.SMILEY.ToString())
                {
                    //string questionText = createdQuestions_ListView.SelectedItems[0].SubItems[2].Text;
                    //int numberOfSmileyFaces = Convert.ToInt32(createdQuestions_ListView.SelectedItems[0].SubItems[3].Text);
                    //form2 = new AddQuestionForm((int)QuestionType.SMILEY, questionId, questionText, numberOfSmileyFaces);
                    //MessageBox.Show("" + questionId);
                    form2 = new AddQuestionForm((int)QuestionType.SMILEY, questionId, QuestionType.SMILEY.ToString());

                }
                else if (createdQuestions_ListView.SelectedItems[0].Text == QuestionType.SLIDER.ToString())
                {
                    //int questionId = Convert.ToInt32(createdQuestions_ListView.SelectedItems[0].SubItems[1].Text);
                    //string questionText = createdQuestions_ListView.SelectedItems[0].SubItems[2].Text;
                    //int numberOfSmileyFaces = Convert.ToInt32(createdQuestions_ListView.SelectedItems[0].SubItems[3].Text);
                    //form2 = new AddQuestionForm((int)QuestionType.SMILEY, questionId, questionText, numberOfSmileyFaces);
                    form2 = new AddQuestionForm((int)QuestionType.SLIDER, questionId, QuestionType.SLIDER.ToString());

                }
                else if (createdQuestions_ListView.SelectedItems[0].Text == QuestionType.STAR.ToString())
                {
                    //int questionId = Convert.ToInt32(createdQuestions_ListView.SelectedItems[0].SubItems[1].Text);
                    //string questionText = createdQuestions_ListView.SelectedItems[0].SubItems[2].Text;
                    //int numberOfSmileyFaces = Convert.ToInt32(createdQuestions_ListView.SelectedItems[0].SubItems[3].Text);
                    //form2 = new AddQuestionForm((int)QuestionType.SMILEY, questionId, questionText, numberOfSmileyFaces);
                    form2 = new AddQuestionForm((int)QuestionType.STAR, questionId, QuestionType.STAR.ToString());

                }
                //if (createdQuestions_ListView.SelectedItems[0].Text == QuestionType.SLIDER.ToString())
                //    form2 = new AddQuestionForm((int)QuestionType.SLIDER);
                //if (createdQuestions_ListView.SelectedItems[0].Text == QuestionType.STAR.ToString())
                //    form2 = new AddQuestionForm((int)QuestionType.STAR);
                form2.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select an item first", "No selected item", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }
    }
}
