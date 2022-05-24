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
    public partial class Form1 : Form
    {
        /* 
       * Global Variables
       * Access them anywhere
       * */
        //private ConnectionStringSettings cn = ConfigurationManager.ConnectionStrings["SurveyQuestionsConfigurator"]; //get connection string information from App.config
        private ConnectionStringSettings cn = ConfigurationManager.ConnectionStrings[0]; //get connection string information from App.config
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
                MessageBox.Show("SQL Error:\n" + ex.Message);
                Trace.TraceError("SQL Error:\n" + ex.Message + "\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong:\n" + ex.Message);
                Trace.TraceError("Something went wrong:\n" + ex.Message + "\n");
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
            //MessageBox.Show(cn.Name);
            /*
             * If tables do not exit, create them
             */
            CheckIfTablesExist();
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
                            conn = new SqlConnection(cn.ConnectionString);
                            questionId = Convert.ToInt32(selectedItem.SubItems[1].Text);
                            SqlCommand cmd = new SqlCommand($@"delete from Smiley_Questions where QuestionID = {questionId};", conn);
                            conn.Open();
                            int affectedRows = cmd.ExecuteNonQuery();
                            if (affectedRows > 0)
                            {
                                /*
                                 * Rebuild List View after each deletion
                                 */
                                BuildListView();
                                MessageBox.Show("Question deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("SQL Error\n\n" + ex.Message);
                            Trace.TraceError("SQL Error:\n" + ex.Message + "\n");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Somthing went wrong:\n\n" + ex.Message);
                            Trace.TraceError("Something went wrong:\n" + ex.Message + "\n");
                        }
                        finally
                        {
                            conn.Close();
                        }
                        createdQuestions_ListView.SelectedIndices.Clear();

                    }
                    else if (selectedItem.Text.ToString() == QuestionType.SLIDER.ToString()) //Slider Question
                    {
                        try
                        {
                            conn = new SqlConnection(cn.ConnectionString);
                            questionId = Convert.ToInt32(selectedItem.SubItems[1].Text);
                            SqlCommand cmd = new SqlCommand($@"delete from Slider_Questions where QuestionID = {questionId};", conn);
                            conn.Open();
                            int affectedRows = cmd.ExecuteNonQuery();
                            if (affectedRows > 0)
                            {
                                /*
                                 * Rebuild List View after each deletion
                                 */
                                BuildListView();
                                MessageBox.Show("Question deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("SQL Error\n\n" + ex.Message);
                            Trace.TraceError("SQL Error:\n" + ex.Message + "\n");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Somthing went wrong:\n\n" + ex.Message);
                            Trace.TraceError("Something went wrong:\n" + ex.Message + "\n");
                        }
                        finally
                        {
                            conn.Close();
                        }
                        createdQuestions_ListView.SelectedIndices.Clear();

                    }
                    else if (selectedItem.Text.ToString() == QuestionType.STAR.ToString()) //Star Question
                    {
                        try
                        {
                            conn = new SqlConnection(cn.ConnectionString);
                            questionId = Convert.ToInt32(selectedItem.SubItems[1].Text);
                            SqlCommand cmd = new SqlCommand($@"delete from Star_Questions where QuestionID = {questionId};", conn);
                            conn.Open();
                            int affectedRows = cmd.ExecuteNonQuery();
                            if (affectedRows > 0)
                            {
                                /*
                                 * Rebuild List View after each deletion
                                 */
                                BuildListView();
                                MessageBox.Show("Question deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("SQL Error\n\n" + ex.Message);
                            Trace.TraceError("SQL Error:\n" + ex.Message + "\n");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Somthing went wrong:\n\n" + ex.Message);
                            Trace.TraceError("Something went wrong:\n" + ex.Message + "\n");
                        }
                        finally
                        {
                            conn.Close();
                        }
                        createdQuestions_ListView.SelectedIndices.Clear();

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
            conn.Close();
        }

        private void CheckIfTablesExist()
        {
            /*
             * GET DATABASE NAME FROM APP.CONFIG FILE
             */

            try
            {
                conn = new SqlConnection(cn.ConnectionString);
                SqlCommand cmd = null;

                /*
                 * Failed attempt at creating a database that does not exist before
                 */
                cmd = new SqlCommand($@"
USE [master]
IF DB_ID(N'{cn.Name}') IS NULL
BEGIN
/****** Object:  Database [{cn.Name}]    Script Date: 24/05/2022 13:06:25 ******/
CREATE DATABASE [{cn.Name}]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'{cn.Name}', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\{cn.Name}.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'{cn.Name}_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\{cn.Name}_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [{cn.Name}].[dbo].[sp_fulltext_database] @action = 'enable'
end
ALTER DATABASE [{cn.Name}] SET ANSI_NULL_DEFAULT OFF 
ALTER DATABASE [{cn.Name}] SET ANSI_NULLS OFF 
ALTER DATABASE [{cn.Name}] SET ANSI_PADDING OFF 
ALTER DATABASE [{cn.Name}] SET ANSI_WARNINGS OFF 
ALTER DATABASE [{cn.Name}] SET ARITHABORT OFF 
ALTER DATABASE [{cn.Name}] SET AUTO_CLOSE OFF 
ALTER DATABASE [{cn.Name}] SET AUTO_SHRINK OFF 
ALTER DATABASE [{cn.Name}] SET AUTO_UPDATE_STATISTICS ON 
ALTER DATABASE [{cn.Name}] SET CURSOR_CLOSE_ON_COMMIT OFF 
ALTER DATABASE [{cn.Name}] SET CURSOR_DEFAULT  GLOBAL 
ALTER DATABASE [{cn.Name}] SET CONCAT_NULL_YIELDS_NULL OFF 
ALTER DATABASE [{cn.Name}] SET NUMERIC_ROUNDABORT OFF 
ALTER DATABASE [{cn.Name}] SET QUOTED_IDENTIFIER OFF 
ALTER DATABASE [{cn.Name}] SET RECURSIVE_TRIGGERS OFF 
ALTER DATABASE [{cn.Name}] SET  DISABLE_BROKER 
ALTER DATABASE [{cn.Name}] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
ALTER DATABASE [{cn.Name}] SET DATE_CORRELATION_OPTIMIZATION OFF 
ALTER DATABASE [{cn.Name}] SET TRUSTWORTHY OFF 
ALTER DATABASE [{cn.Name}] SET ALLOW_SNAPSHOT_ISOLATION OFF 
ALTER DATABASE [{cn.Name}] SET PARAMETERIZATION SIMPLE 
ALTER DATABASE [{cn.Name}] SET READ_COMMITTED_SNAPSHOT OFF 
ALTER DATABASE [{cn.Name}] SET HONOR_BROKER_PRIORITY OFF 
ALTER DATABASE [{cn.Name}] SET RECOVERY FULL 
ALTER DATABASE [{cn.Name}] SET  MULTI_USER 
ALTER DATABASE [{cn.Name}] SET PAGE_VERIFY CHECKSUM  
ALTER DATABASE [{cn.Name}] SET DB_CHAINING OFF 
ALTER DATABASE [{cn.Name}] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
ALTER DATABASE [{cn.Name}] SET TARGET_RECOVERY_TIME = 60 SECONDS 
ALTER DATABASE [{cn.Name}] SET DELAYED_DURABILITY = DISABLED 
ALTER DATABASE [{cn.Name}] SET ACCELERATED_DATABASE_RECOVERY = OFF  
ALTER DATABASE [{cn.Name}] SET QUERY_STORE = OFF
ALTER DATABASE [{cn.Name}] SET  READ_WRITE 
END
", conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();


                /*
                 * Create Tables if they do NOT exist
                 */
                cmd = new SqlCommand($@"
USE [{cn.Name}]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Smiley_Questions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Smiley_Questions](
	[QuestionID] [int] IDENTITY(1,1) NOT NULL,
	[QuestionOrder] [int] NOT NULL,
	[QuestionText] [text] NOT NULL,
	[NumberOfSmileyFaces] [int] NOT NULL,
 CONSTRAINT [PK_Smiley_Faces] PRIMARY KEY CLUSTERED 
(
	[QuestionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
", conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();


                cmd = new SqlCommand($@"
USE [{cn.Name}]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Slider_Questions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Slider_Questions](
	[QuestionID] [int] IDENTITY(1,1) NOT NULL,
	[QuestionOrder] [int] NOT NULL,
	[QuestionText] [text] NOT NULL,
	[QuestionStartValue] [int] NOT NULL,
	[QuestionEndValue] [int] NOT NULL,
	[QuestionStartValueCaption] [varchar](100) NOT NULL,
	[QuestionEndValueCaption] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Slider_Questions] PRIMARY KEY CLUSTERED 
(
	[QuestionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_Slider_Questions] UNIQUE NONCLUSTERED 
(
	[QuestionOrder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
", conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();


                cmd = new SqlCommand($@"
USE [{cn.Name}]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Star_Questions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Star_Questions](
	[QuestionID] [int] IDENTITY(1,1) NOT NULL,
	[QuestionOrder] [int] NOT NULL,
	[QuestionText] [text] NOT NULL,
	[NumberOfStars] [int] NOT NULL,
 CONSTRAINT [PK_Stars_Questions] PRIMARY KEY CLUSTERED 
(
	[QuestionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_Stars_Questions] UNIQUE NONCLUSTERED 
(
	[QuestionOrder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
", conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL Error:\n" + ex.Message);
                Trace.TraceError("SQL Error:\n" + ex.Message + "\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong:\n" + ex.Message);
                Trace.TraceError("Something went wrong:\n" + ex.Message + "\n");
            }
            finally
            {
                conn.Close();
            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            BuildListView();
        }
    }
}
