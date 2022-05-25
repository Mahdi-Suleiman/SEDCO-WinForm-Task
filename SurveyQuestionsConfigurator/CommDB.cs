using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyQuestionsConfigurator
{
    public class CommDB
    {
        public static int QuestionId { get; set; } // create global Question ID property
        private static ConnectionStringSettings cn = ConfigurationManager.ConnectionStrings[0]; //get connection string information from App.config
        private static SqlConnection conn = null; // Create SqlConnection object to connect to DB

        /// <summary>
        /// 
        /// </summary>
        /// <param name="questionOrder"></param>
        /// <param name="NumberOfSmilyFaces"></param>
        /// <param name="QuestionText"></param>
        /// <returns>
        /// 1 -> Success
        /// 2 -> Unique key violation
        /// -1 -> Error
        /// </returns>
        public static int AddSmileyQuestion(int questionOrder, string QuestionText, int NumberOfSmilyFaces)
        {
            /*
            * Assign values to variables
            */

            //questionOrder = Convert.ToInt32(questionOrderNumericUpDown.Value);
            //QuestionText = questionTextRichTextBox.Text;
            //NumberOfSmilyFaces = Convert.ToInt32(genericNumericUpDown1.Value);

            /*
            * Try to insert a new question into "Smiley_Questions" table
            */
            try
            {
                conn = new SqlConnection(cn.ConnectionString);
                SqlCommand cmd = new SqlCommand($@"
USE {cn.Name}
INSERT INTO Smiley_Questions
(QuestionOrder, QuestionText, NumberOfSmileyFaces)
VALUES
({questionOrder},'{QuestionText}',{NumberOfSmilyFaces})", conn);

                conn.Open();
                cmd.ExecuteNonQuery();

                //MessageBox.Show("Question inserted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //ClearInputs();
                return 1;
            }
            catch (SqlException ex)
            {
                //2627 -> unique key violation
                // ex.Number
                if (ex.Number == 2627)
                {
                    //MessageBox.Show("This Question order is already in use\nTry using another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return 2;
                }
                else
                {
                    //MessageBox.Show("SQL Error:\n" + ex.Message);
                    Trace.TraceError("SQL Error:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
                    return -1;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Something went wrong:\n" + ex.Message);
                Trace.TraceError("Something went wrong:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
                return -1;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="questionOrder"></param>
        /// <param name="questionText"></param>
        /// <param name="questionStartValue"></param>
        /// <param name="questionEndValue"></param>
        /// <param name="questionStartValueCaption"></param>
        /// <param name="questionEndValueCaption"></param>
        /// <returns>
        /// 1 -> Success
        /// 2 -> Unique key violation
        /// -1 -> Error
        /// </returns>
        public static int AddSliderQuestion(int questionOrder, string questionText, int questionStartValue,
            int questionEndValue, string questionStartValueCaption, string questionEndValueCaption)
        {
            /*
            * Try to insert a new question into "Slider_Questions" table
            */
            try
            {
                conn = new SqlConnection(cn.ConnectionString);
                SqlCommand cmd = new SqlCommand($@"
USE {cn.Name}
INSERT INTO Slider_Questions
(QuestionOrder, QuestionText, QuestionStartValue, QuestionEndValue, QuestionStartValueCaption, QuestionEndValueCaption)
VALUES
({questionOrder},'{questionText}',{questionStartValue}, {questionEndValue}, '{questionStartValueCaption}', '{questionEndValueCaption}' )",
        conn);

                conn.Open();
                cmd.ExecuteNonQuery();
                return 1;
            }
            catch (SqlException ex)
            {
                //2627 -> unique key violation
                if (ex.Number == 2627)
                {
                    return 2;
                }
                else
                {
                    Trace.TraceError("SQL Error:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
                    return -1;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Something went wrong:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
                return -1;
            }
            finally
            {
                conn.Close();
            }

        } // end of function


        /// <summary>
        /// 
        /// </summary>
        /// <param name="questionOrder"></param>
        /// <param name="questionText"></param>
        /// <param name="numberOfStars"></param>
        /// <returns>
        /// 1 -> Success
        /// 2 -> Unique key violation
        /// -1 -> Error
        /// </returns>
        public static int AddStarQuestion(int questionOrder, string questionText, int numberOfStars)
        {
            ///
            // Try to insert a new question into "Star_Questions" table
            //
            try
            {
                conn = new SqlConnection(cn.ConnectionString);
                SqlCommand cmd = new SqlCommand($@"
USE {cn.Name}
INSERT INTO Star_Questions
(QuestionOrder, QuestionText, NumberOfStars)
values
({questionOrder}, '{questionText}', {numberOfStars})", conn);

                conn.Open();
                cmd.ExecuteNonQuery();
                return 1;
            }
            catch (SqlException ex)
            {
                //2627 -> unique key violation
                if (ex.Number == 2627)
                {
                    return 2;
                }
                else
                {
                    Trace.TraceError("SQL Error:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
                    return -1;
                }
            }

            catch (Exception ex)
            {
                Trace.TraceError("Something went wrong:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
                return -1;
            }
            finally
            {
                conn.Close();
            }

        }// end of function



        public static int EditSmileyQuestion(int questionOrder, string QuestionText, int NumberOfSmilyFaces)
        {
            ///
            /// Try to Update a new question into "Smiley_Questions" table
            ///
            try
            {
                using (SqlConnection conn = new SqlConnection(cn.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand($@"
USE {cn.Name}
 UPDATE Smiley_Questions
 SET QuestionOrder = {questionOrder}, QuestionText = '{QuestionText}', NumberOfSmileyFaces ={NumberOfSmilyFaces}
 WHERE QuestionID = {QuestionId};
", conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return 1;
                }
            }
            catch (SqlException ex)
            {
                //2627 -> unique key violation
                // ex.Number
                if (ex.Number == 2627)
                {
                    //MessageBox.Show("This Question order is already in use\nTry using another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return 2;
                }
                else
                {
                    //MessageBox.Show("SQL Error:\n" + ex.Message);
                    Trace.TraceError("SQL Error:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
                    return -1;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Something went wrong:\n" + ex.Message);
                Trace.TraceError("Something went wrong:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
                return -1;
            }
            //finally
            //{
            //    conn.Close();
            //}

        }//end of function


    }
}
