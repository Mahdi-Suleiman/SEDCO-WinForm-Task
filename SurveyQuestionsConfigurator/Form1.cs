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
        private ListViewColumnSorter lvwColumnSorter;

        public Form1()
        {
            InitializeComponent();

            // Create an instance of a ListView column sorter and assign it
            // to the ListView control.
            lvwColumnSorter = new ListViewColumnSorter();
            this.listView1.ListViewItemSorter = lvwColumnSorter;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ColumnHeader columnheader;// Used for creating column headers.
            ListViewItem listviewitem;// Used for creating listview items.
            SqlConnection conn = null;


            // Ensure that the view is set to show details.
            listView1.View = View.Details;

            // Create some column headers for the data.
            columnheader = new ColumnHeader();
            columnheader.Text = "ID";
            this.listView1.Columns.Add(columnheader);

            columnheader = new ColumnHeader();
            columnheader.Text = "Order";
            this.listView1.Columns.Add(columnheader);

            columnheader = new ColumnHeader();
            columnheader.Text = "Text";
            this.listView1.Columns.Add(columnheader);

            columnheader = new ColumnHeader();
            columnheader.Text = "Number of Smiley Faces";
            this.listView1.Columns.Add(columnheader);

            columnheader = new ColumnHeader();
            columnheader.Text = "Number of stars";
            this.listView1.Columns.Add(columnheader);

            columnheader = new ColumnHeader();
            columnheader.Text = "Start Value";
            this.listView1.Columns.Add(columnheader);

            columnheader = new ColumnHeader();
            columnheader.Text = "Start Value Caption";
            this.listView1.Columns.Add(columnheader);

            columnheader = new ColumnHeader();
            columnheader.Text = "End Value";
            this.listView1.Columns.Add(columnheader);

            columnheader = new ColumnHeader();
            columnheader.Text = "End Value Caption";
            this.listView1.Columns.Add(columnheader);



            var cn = ConfigurationManager.ConnectionStrings["cn"];
            SqlDataReader reader = null;

            try
            {
                conn = new SqlConnection(cn.ConnectionString);
                SqlCommand cmd = new SqlCommand("select * from Smiley_Questions", conn);
                conn.Open();
                reader = cmd.ExecuteReader();
                //var data = cmd.ExecuteReader();
                while (reader.Read())
                {
                    MessageBox.Show($@"{reader[0]} + {reader[1]}");
                }
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
                conn.Close();
            }

            // Create some listview items consisting of first and last names.
            listviewitem = new ListViewItem("John");
            listviewitem.SubItems.Add("Smith");
            listviewitem.SubItems.Add("3");
            this.listView1.Items.Add(listviewitem);

            listviewitem = new ListViewItem("Bob");
            listviewitem.SubItems.Add("Taylor");
            this.listView1.Items.Add(listviewitem);

            listviewitem = new ListViewItem("Kim");
            listviewitem.SubItems.Add("Zimmerman");
            this.listView1.Items.Add(listviewitem);

            listviewitem = new ListViewItem("Olivia");
            listviewitem.SubItems.Add("Johnson");
            this.listView1.Items.Add(listviewitem);

            //this.listView1.Items.Add(`listview`item);


            //this.`listView`1.Columns.Add(columnheader);


            // Loop through and size each column header to fit the column header text.
            foreach (ColumnHeader ch in this.listView1.Columns)
            {
                ch.Width = -2;
            }
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
            this.listView1.Sort();
        }

        private void addQuestionButton_Click(object sender, EventArgs e)
        {
            AddQuestionForm form = new AddQuestionForm();
            form.ShowDialog();
        }
    }
}
