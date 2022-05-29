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
 UPDATE Slider_Questions
 SET QuestionOrder = {questionOrder}, QuestionText = '{questionText}', QuestionStartValue = {questionStartValue}, QuestionEndValue = {questionEndValue}, QuestionStartValueCaption = '{questionStartValueCaption}', QuestionEndValueCaption = '{questionEndValueCaption}'
 WHERE QuestionID = {QuestionId};
", conn);
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
                    Trace.TraceError("SQL Error:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
                    return -1;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Something went wrong:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
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
 UPDATE Star_Questions
 SET QuestionOrder = {questionOrder}, QuestionText = '{questionText}', NumberOfStars = {numberOfStars}
 WHERE QuestionID = {QuestionId};
", conn);
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
                Trace.TraceError("SQL Error:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
                return 2;
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
                Trace.TraceError("SQL Error:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
                return 2;
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
                Trace.TraceError("SQL Error:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
                return 2;
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
        } //end func.


        public static DataTable RetrieveSmileyQuestions()
        {
            SqlDataAdapter adapter = null;
            DataTable dt = new DataTable();
            SqlCommand cmd = null;

            try
            {
                conn = new SqlConnection(cn.ConnectionString);
                cmd = new SqlCommand("select * from Smiley_Questions", conn);
                conn.Open();
                adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                return dt;
            }
            catch (SqlException ex)
            {
                Trace.TraceError("SQL Error:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
                return null;
            }
            catch (Exception ex)
            {
                Trace.TraceError("Something went wrong:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
                return null;
            }
            finally
            {
                conn.Close();
            }
        } //end func.

        public static DataTable RetrieveSliderQuestions()
        {
            SqlDataAdapter adapter = null;
            DataTable dt = new DataTable();
            SqlCommand cmd = null;

            try
            {
                conn = new SqlConnection(cn.ConnectionString);
                cmd = new SqlCommand("select * from Slider_Questions", conn);
                conn.Open();
                adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                return dt;
            }
            catch (SqlException ex)
            {
                Trace.TraceError("SQL Error:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
                return null;
            }
            catch (Exception ex)
            {
                Trace.TraceError("Something went wrong:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
                return null;
            }
            finally
            {
                conn.Close();
            }
        } //end func.

        public static DataTable RetrieveStarQuestions()
        {
            SqlDataAdapter adapter = null;
            DataTable dt = new DataTable();
            SqlCommand cmd = null;

            try
            {
                conn = new SqlConnection(cn.ConnectionString);
                cmd = new SqlCommand("select * from Star_Questions", conn);
                conn.Open();
                adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                return dt;
            }
            catch (SqlException ex)
            {
                Trace.TraceError("SQL Error:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
                return null;
            }
            catch (Exception ex)
            {
                Trace.TraceError("Something went wrong:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
                return null;
            }
            finally
            {
                conn.Close();
            }
        } //end func.

        public static int CheckIfTablesExist()
        {
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
        } //end func.

        public static DataTable RetrieveSingleSmileyQuestion(int questionId)
        {
            QuestionId = questionId;
            SqlDataAdapter adapter = null;
            DataTable dt = new DataTable();
            SqlCommand cmd = null;

            try
            {
                conn = new SqlConnection(cn.ConnectionString);
                cmd = new SqlCommand($"select * from Smiley_Questions where QuestionID = {QuestionId}", conn);
                conn.Open();
                adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                return dt;
            }
            catch (SqlException ex)
            {
                Trace.TraceError("SQL Error:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
                return null;
            }
            catch (Exception ex)
            {
                Trace.TraceError("Something went wrong:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
                return null;
            }
            finally
            {
                conn.Close();
            }
        } // end func.

        public static DataTable RetrieveSingleSliderQuestion(int questionId)
        {
            QuestionId = questionId;
            SqlDataAdapter adapter = null;
            DataTable dt = new DataTable();
            SqlCommand cmd = null;

            try
            {
                conn = new SqlConnection(cn.ConnectionString);
                cmd = new SqlCommand($"select * from Slider_Questions where QuestionID = {QuestionId}", conn);
                conn.Open();
                adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                return dt;
            }
            catch (SqlException ex)
            {
                Trace.TraceError("SQL Error:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
                return null;
            }
            catch (Exception ex)
            {
                Trace.TraceError("Something went wrong:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
                return null;
            }
            finally
            {
                conn.Close();
            }
        } // end func.

        public static DataTable RetrieveSingleStarQuestion(int questionId)
        {
            QuestionId = questionId;
            SqlDataAdapter adapter = null;
            DataTable dt = new DataTable();
            SqlCommand cmd = null;

            try
            {
                conn = new SqlConnection(cn.ConnectionString);
                cmd = new SqlCommand($@"select * from Star_Questions where QuestionID = {QuestionId}", conn);
                conn.Open();
                adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                return dt;
            }
            catch (SqlException ex)
            {
                Trace.TraceError("SQL Error:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
                return null;
            }
            catch (Exception ex)
            {
                Trace.TraceError("Something went wrong:\nShort Message:" + ex.Message + "Long Message:\n" + ex + "\n"); //write error to log file
                return null;
            }
            finally
            {
                conn.Close();
            }
        } // end func.

    }
}
