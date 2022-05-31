using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
        /// <param name="questionText"></param>
        /// <returns>
        /// 1 -> Success
        /// 2 -> Unique key violation
        /// -1 -> Error
        /// </returns>
        public static int AddSmileyQuestion(int questionOrder, string questionText, int NumberOfSmilyFaces)
        {
            ///
            /// Try to insert a new question into "Smiley_Questions" table
            ///
            try
            {
                conn = new SqlConnection(cn.ConnectionString);
                SqlCommand cmd = new SqlCommand($@"
USE {cn.Name}
INSERT INTO Questions
(QuestionOrder, QuestionText, QuestionType)
VALUES
({questionOrder}, '{questionText}', 'SMILEY')
INSERT INTO Smiley_Questions
(QuestionID, NumberOfSmileyFaces)
VALUES
((SELECT QuestionID FROM Questions WHERE QuestionOrder = {questionOrder} AND QuestionType = 'SMILEY') ,{NumberOfSmilyFaces})", conn);

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
                    CommonLayer.LogError(ex); //write error to log file
                    return -1;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Something went wrong:\n" + ex.Message);
                CommonLayer.LogError(ex); //write error to log file
                return -1;
            }
            finally
            {
                conn.Close();
            }
        } //end func.

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
            ///
            /// Try to insert a new question into "Slider_Questions" table
            ///
            try
            {
                conn = new SqlConnection(cn.ConnectionString);
                SqlCommand cmd = new SqlCommand($@"
USE {cn.Name}
INSERT INTO Questions
(QuestionOrder, QuestionText, QuestionType)
VALUES
({questionOrder}, '{questionText}', 'SLIDER')

INSERT INTO Slider_Questions
(QuestionID, QuestionStartValue, QuestionEndValue, QuestionStartValueCaption, QuestionEndValueCaption)
VALUES
((SELECT QuestionID FROM Questions WHERE QuestionOrder = {questionOrder} AND QuestionType = 'SLIDER'), {questionStartValue}, {questionEndValue}, '{questionStartValueCaption}', '{questionEndValueCaption}' )",
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
                    CommonLayer.LogError(ex); //write error to log file
                    return -1;
                }
            }
            catch (Exception ex)
            {
                CommonLayer.LogError(ex); //write error to log file
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
INSERT INTO Questions
(QuestionOrder, QuestionText, QuestionType)
VALUES
({questionOrder}, '{questionText}', 'STAR')

INSERT INTO Star_Questions
(QuestionID,  NumberOfStars)
values
((SELECT QuestionID FROM Questions WHERE QuestionOrder = {questionOrder} AND QuestionType = 'STAR'), {numberOfStars})", conn);

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
                    CommonLayer.LogError(ex); //write error to log file
                    return -1;
                }
            }

            catch (Exception ex)
            {
                CommonLayer.LogError(ex); //write error to log file
                return -1;
            }
            finally
            {
                conn.Close();
            }

        }// end of function


        /// <summary>
        /// 
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="questionOrder"></param>
        /// <param name="QuestionText"></param>
        /// <param name="NumberOfSmilyFaces"></param>
        /// <returns>
        /// 1 -> Success
        /// 2 -> Unique key violation
        /// -1 -> Error
        /// </returns>
        public static int EditSmileyQuestion(int questionId, int questionOrder, string QuestionText, int NumberOfSmilyFaces)
        {
            ///
            /// Try to Update a new question into "Smiley_Questions" table
            ///
            try
            {
                QuestionId = questionId;
                using (SqlConnection conn = new SqlConnection(cn.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand($@"
USE {cn.Name}
UPDATE Questions
SET QuestionOrder = {questionOrder}, QuestionText = '{QuestionText}'
WHERE QuestionID = {QuestionId}

UPDATE Smiley_Questions
SET NumberOfSmileyFaces = {NumberOfSmilyFaces}
WHERE QuestionID = {QuestionId}", conn);
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
                    CommonLayer.LogError(ex); //write error to log file
                    return -1;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Something went wrong:\n" + ex.Message);
                CommonLayer.LogError(ex); //write error to log file
                return -1;
            }
            //finally
            //{
            //    conn.Close();
            //}

        }//end of function

        /// <summary>
        /// 
        /// </summary>
        /// <param name="questionId"></param>
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
        public static int EditSliderQuestion(int questionId, int questionOrder, string questionText, int questionStartValue, int questionEndValue, string questionStartValueCaption, string questionEndValueCaption)
        {
            try
            {
                QuestionId = questionId;
                using (SqlConnection conn = new SqlConnection(cn.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand($@"
USE {cn.Name}
UPDATE Questions
SET QuestionOrder = {questionOrder}, QuestionText = '{questionText}'
WHERE QuestionID = {QuestionId}

UPDATE Slider_Questions
SET QuestionStartValue = {questionStartValue}, QuestionEndValue = {questionEndValue}, QuestionStartValueCaption = '{questionStartValueCaption}', QuestionEndValueCaption = '{questionEndValueCaption}'
WHERE QuestionID = {QuestionId}", conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return 1;
                    //MessageBox.Show("Question updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (SqlException ex)
            {
                //2627 -> primary key violation
                if (ex.Number == 2627)
                {
                    //MessageBox.Show("This Question order is already in use\nTry using another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return 2;
                }
                else
                {
                    CommonLayer.LogError(ex); //write error to log file
                    return -1;
                }
            }
            catch (Exception ex)
            {
                CommonLayer.LogError(ex); //write error to log file
                return -1;
            }
        } //end func.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="questionOrder"></param>
        /// <param name="questionText"></param>
        /// <param name="numberOfStars"></param>
        /// <returns>
        /// 1 -> Success
        /// 2 -> Unique key violation
        /// -1 -> Error
        /// </returns>
        public static int EditStarQuestion(int questionId, int questionOrder, string questionText, int numberOfStars)
        {
            try
            {
                QuestionId = questionId;
                using (SqlConnection conn = new SqlConnection(cn.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand($@"
USE {cn.Name}
UPDATE Questions
SET QuestionOrder = {questionOrder}, QuestionText = '{questionText}'
WHERE QuestionID = {QuestionId}

UPDATE Star_Questions
SET NumberOfStars = {numberOfStars}
WHERE QuestionID = {QuestionId}", conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    //MessageBox.Show("Question updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    CommonLayer.LogError(ex); //write error to log file
                    return -1;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Something went wrong:\n" + ex.Message);
                CommonLayer.LogError(ex); //write error to log file
                return -1;
            }
        } //end func.

        public static int DeleteQuestion(int questionId)
        {
            try
            {
                QuestionId = questionId;
                conn = new SqlConnection(cn.ConnectionString);
                SqlCommand cmd = new SqlCommand($@"delete from Questions where QuestionID = {QuestionId};", conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                return 1;
            }
            catch (SqlException ex)
            {
                CommonLayer.LogError(ex); //write error to log file
                return 2;
            }
            catch (Exception ex)
            {
                CommonLayer.LogError(ex); //write error to log file
                return -1;
            }
            finally
            {
                conn.Close();
            }
        } //end func.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns>
        /// 1 -> Success
        /// 2 -> SQL Error
        /// -1 -> Error
        /// </returns>
        public static int DeleteSmileyQuestion(int questionId)
        {
            try
            {
                QuestionId = questionId;
                conn = new SqlConnection(cn.ConnectionString);
                SqlCommand cmd = new SqlCommand($@"delete from Smiley_Questions where QuestionID = {QuestionId};", conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                return 1;
            }
            catch (SqlException ex)
            {
                CommonLayer.LogError(ex); //write error to log file
                return 2;
            }
            catch (Exception ex)
            {
                CommonLayer.LogError(ex); //write error to log file
                return -1;
            }
            finally
            {
                conn.Close();
            }
        } //end func.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns>
        /// 1 -> Success
        /// 2 -> SQL Error
        /// -1 -> Error
        /// </returns>
        public static int DeleteSliderQuestion(int questionId)
        {
            try
            {
                QuestionId = questionId;
                conn = new SqlConnection(cn.ConnectionString);
                SqlCommand cmd = new SqlCommand($@"delete from Slider_Questions where QuestionID = {questionId};", conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                return 1;
                // BuildListView();
            }
            catch (SqlException ex)
            {
                CommonLayer.LogError(ex); //write error to log file
                return 2;
            }
            catch (Exception ex)
            {
                CommonLayer.LogError(ex); //write error to log file
                return -1;
            }
            finally
            {
                conn.Close();
            }
        } //end func.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns>
        /// 1 -> Success
        /// 2 -> SQL Error
        /// -1 -> Error
        /// </returns>
        public static int DeleteStarQuestion(int questionId)
        {
            try
            {
                QuestionId = questionId;
                conn = new SqlConnection(cn.ConnectionString);
                SqlCommand cmd = new SqlCommand($@"delete from Star_Questions where QuestionID = {QuestionId};", conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                return 1;
                // BuildListView();
            }
            catch (SqlException ex)
            {
                CommonLayer.LogError(ex); //write error to log file
                return 2;
            }
            catch (Exception ex)
            {
                CommonLayer.LogError(ex); //write error to log file
                return -1;
            }
            finally
            {
                conn.Close();
            }
        } //end func.

        public static DataTable RetrieveQuestions()
        {

            try
            {
                SqlDataAdapter adapter = null;
                DataTable dt = new DataTable();
                SqlCommand cmd = null;

                conn = new SqlConnection(cn.ConnectionString);
                cmd = new SqlCommand($@"
USE {cn.Name}
SELECT QuestionID, QuestionOrder, QuestionType, QuestionText FROM Questions", conn);
                conn.Open();
                adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                return dt;
            }
            catch (SqlException ex)
            {
                CommonLayer.LogError(ex); //write error to log file
                return null;
            }
            catch (Exception ex)
            {
                CommonLayer.LogError(ex); //write error to log file
                return null;
            }
            finally
            {
                conn.Close();
            }
        } //end func.

        public static DataTable RetrieveSingleSmileyQuestion(int questionId)
        {
            try
            {
                QuestionId = questionId;
                SqlDataAdapter adapter = null;
                DataTable dt = new DataTable();
                SqlCommand cmd = null;


                conn = new SqlConnection(cn.ConnectionString);
                cmd = new SqlCommand($@"
USE {cn.Name}
select Questions.QuestionOrder, Questions.QuestionText, Smiley_Questions.NumberOfSmileyFaces
from Questions
inner join Smiley_Questions
on Questions.QuestionID = Smiley_Questions.QuestionID
where Questions.QuestionID = {QuestionId}", conn);
                conn.Open();
                adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                return dt;
            }
            catch (SqlException ex)
            {
                CommonLayer.LogError(ex); //write error to log file
                return null;
            }
            catch (Exception ex)
            {
                CommonLayer.LogError(ex); //write error to log file
                return null;
            }
            finally
            {
                conn.Close();
            }
        } // end func.

        public static DataTable RetrieveSingleSliderQuestion(int questionId)
        {
            try
            {
                QuestionId = questionId;
                SqlDataAdapter adapter = null;
                DataTable dt = new DataTable();
                SqlCommand cmd = null;

                conn = new SqlConnection(cn.ConnectionString);
                cmd = new SqlCommand($@"
USE {cn.Name}
select Q.QuestionOrder, Q.QuestionText, SQ.QuestionStartValue, SQ.QuestionEndValue, SQ.QuestionStartValueCaption, SQ.QuestionEndValueCaption
from Questions AS Q
inner join Slider_Questions AS SQ
on Q.QuestionID = SQ.QuestionID
where Q.QuestionID = {QuestionId}", conn);
                conn.Open();
                adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                return dt;
            }
            catch (SqlException ex)
            {
                CommonLayer.LogError(ex); //write error to log file
                return null;
            }
            catch (Exception ex)
            {
                CommonLayer.LogError(ex); //write error to log file
                return null;
            }
            finally
            {
                conn.Close();
            }
        } // end func.

        public static DataTable RetrieveSingleStarQuestion(int questionId)
        {
            try
            {
                QuestionId = questionId;
                SqlDataAdapter adapter = null;
                DataTable dt = new DataTable();
                SqlCommand cmd = null;

                conn = new SqlConnection(cn.ConnectionString);
                cmd = new SqlCommand($@"
USE {cn.Name}
select Q.QuestionOrder, Q.QuestionText, StQ.NumberOfStars
from Questions AS Q
inner join Star_Questions AS StQ
on Q.QuestionID = StQ.QuestionID
where Q.QuestionID = {QuestionId}", conn);
                conn.Open();
                adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                return dt;
            }
            catch (SqlException ex)
            {
                CommonLayer.LogError(ex); //write error to log file
                return null;
            }
            catch (Exception ex)
            {
                CommonLayer.LogError(ex); //write error to log file
                return null;
            }
            finally
            {
                conn.Close();
            }
        } // end func.

        public static int CheckIfTablesExist()
        {
            try
            {
                conn = new SqlConnection(cn.ConnectionString);
                SqlCommand cmd = null;


                ///
                /// Create Tables if they do NOT exist
                ///
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
                    CommonLayer.LogError(ex); //write error to log file
                    return -1;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Something went wrong:\n" + ex.Message);
                CommonLayer.LogError(ex); //write error to log file
                return -1;
            }
            finally
            {
                conn.Close();
            }
        } //end func.
    }
}
