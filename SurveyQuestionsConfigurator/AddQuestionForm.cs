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
    public partial class AddQuestionForm : Form
    {
        public AddQuestionForm()
        {
            InitializeComponent();
        }

        private void smileyQuestionAddQuestionButton_Click(object sender, EventArgs e)
        {
            SqlConnection conn = null;
            var cn = ConfigurationManager.ConnectionStrings["cn"];

            int questionOrder, NumberOfSmilyFaces;
            string QuestionText;




            if (!String.IsNullOrEmpty(smileyQuestionTextRichTextBox.Text))
            {
                questionOrder = Convert.ToInt32(smileyQuestionOrderNumericUpDown.Value);
                QuestionText = smileyQuestionTextRichTextBox.Text;
                NumberOfSmilyFaces = Convert.ToInt32(smileyQuestionNumberOfSmileyFacesNumericUpDown.Value);
                try
                {
                    conn = new SqlConnection(cn.ConnectionString);
                    SqlCommand cmd = new SqlCommand($@"
USE SurveyQuestionsConfigurator
INSERT INTO Smiley_Questions
(QuestionOrder, QuestionText, NumberOfSmileyFaces)
VALUES
({questionOrder},'{QuestionText}',{NumberOfSmilyFaces})", conn);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Question inserted successfully");

                }
                catch (SqlException ex)
                {
                    //2627
                    //2601
                    if (ex.Number == 2601)
                    {
                        MessageBox.Show("This Question order is already in use\nTry using another one");
                    }
                    else
                    {
                        MessageBox.Show("Something went wrong:\n" + ex);
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong:\n" + ex);
                }
                finally
                {
                    conn.Close();
                }
            }
            else
            {
                MessageBox.Show("Question text cant be empty", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
