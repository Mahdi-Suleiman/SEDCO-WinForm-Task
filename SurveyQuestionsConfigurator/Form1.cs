using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

            // Ensure that the view is set to show details.
            listView1.View = View.Details;

            // Create some listview items consisting of first and last names.
            listviewitem = new ListViewItem("John");
            listviewitem.SubItems.Add("Smith");
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

            // Create some column headers for the data.
            columnheader = new ColumnHeader();
            columnheader.Text = "First Name";
            this.listView1.Columns.Add(columnheader);
            //this.`listView`1.Columns.Add(columnheader);

            columnheader = new ColumnHeader();
            columnheader.Text = "Last Name";
            this.listView1.Columns.Add(columnheader);

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
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
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
