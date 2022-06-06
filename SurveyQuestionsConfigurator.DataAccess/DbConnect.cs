using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.CommonTypes;
using SurveyQuestionsConfigurator.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;

namespace SurveyQuestionsConfigurator.DataAccess
{
    public class DbConnect
    {
        private ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings[0]; //get connection string information from App.config

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
        public int AddSmileyQuestion(SmileyQuestion smileyQuestion)
        {
            ///
            /// Try to insert a new question into "Smiley_Questions" table
            ///
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand($@"
USE {connectionString.Name}

   IF NOT EXISTS (SELECT * FROM Questions 
                   WHERE [Order] = @Order
                   AND [Type] = @Type)
   BEGIN
       INSERT INTO Questions 
        ([Order], [Text], [Type])
        VALUES 
        (@Order, @Text, @Type)
   END
   IF (@@IDENTITY IS NOT NULL)
    BEGIN
       INSERT INTO Smiley_Questions(ID, NumberOfSmileyFaces )
       OUTPUT @@IDENTITY
       VALUES (@@IDENTITY, @NumberOfSmileyFaces)
   END
", conn);

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@Order", smileyQuestion.Order),
                new SqlParameter("@Text", smileyQuestion.Text),
                new SqlParameter("@Type", smileyQuestion.Type),
                new SqlParameter("@NumberOfSmileyFaces", smileyQuestion.NumberOfSmileyFaces)
                };
                    cmd.Parameters.AddRange(parameters);
                    conn.Open();
                    var result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        return (int)Types.Error.SUCCESS;
                    }
                    else if (result == null)
                    {
                        return (int)Types.Error.SQLVIOLATION;
                    }
                    else
                    {
                        return (int)Types.Error.ERROR;
                    }
                }
            }
            catch (SqlException ex)
            {
                Helper.Logger(ex); //write error to log file
                return (int)Types.Error.ERROR;
            }
            catch (Exception ex)
            {
                Helper.Logger(ex); //write error to log file
                return (int)Types.Error.ERROR;
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
        public int AddSliderQuestion(SliderQuestion sliderQuestion)
        {
            ///
            /// Try to insert a new question into "Slider_Questions" table
            ///
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand($@"
USE {connectionString.Name}
   IF NOT EXISTS (SELECT * FROM Questions 
                   WHERE [Order] = @Order
                   AND [Type] = @Type)
   BEGIN
       INSERT INTO Questions 
        ([Order], [Text], [Type])
        VALUES 
        (@Order, @Text, @Type)
   END
   IF (@@IDENTITY IS NOT NULL)
    BEGIN
       INSERT INTO Slider_Questions
       (ID, StartValue, EndValue, StartValueCaption, EndValueCaption)
       OUTPUT @@IDENTITY
       VALUES (@@IDENTITY, @StartValue, @EndValue, @StartValueCaption, @EndValueCaption)
   END
", conn);

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@Order", sliderQuestion.Order),
                new SqlParameter("@Text", sliderQuestion.Text),
                new SqlParameter("@Type",sliderQuestion.Type),
                new SqlParameter("@StartValue", sliderQuestion.StartValue),
                new SqlParameter("@EndValue", sliderQuestion.EndValue),
                new SqlParameter("@StartValueCaption", sliderQuestion.StartValueCaption),
                new SqlParameter("@EndValueCaption", sliderQuestion.EndValueCaption),
                };
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    var result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        return (int)Types.Error.SUCCESS;
                    }
                    else if (result == null)
                    {
                        return (int)Types.Error.SQLVIOLATION;
                    }
                    else
                    {
                        return (int)Types.Error.ERROR;
                    }
                }
            }
            catch (SqlException ex)
            {
                Helper.Logger(ex); //write error to log file
                return (int)Types.Error.ERROR;
            }
            catch (Exception ex)
            {
                Helper.Logger(ex); //write error to log file
                return (int)Types.Error.ERROR;
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
        public int AddStarQuestion(StarQuestion starQuestion)
        {
            ///
            // Try to insert a new question into "Star_Questions" table
            //
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand($@"
USE {connectionString.Name}

   IF NOT EXISTS (SELECT * FROM Questions 
                   WHERE [Order] = @Order
                   AND [Type] = @Type)
   BEGIN
       INSERT INTO Questions 
        ([Order], [Text], [Type])
        VALUES 
        (@Order, @Text, @Type)
   END
   IF (@@IDENTITY IS NOT NULL)
    BEGIN
       INSERT INTO Star_Questions(ID,  NumberOfStars)
       OUTPUT @@IDENTITY
       VALUES (@@IDENTITY, @NumberOfStars)
   END

", conn);
                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@Order", starQuestion.Order),
                new SqlParameter("@Text", starQuestion.Text),
                new SqlParameter("@Type",starQuestion.Type),
                new SqlParameter("@NumberOfStars", starQuestion.NumberOfStars)
                };
                    cmd.Parameters.AddRange(parameters);
                    conn.Open();
                    var result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        return (int)Types.Error.SUCCESS;
                    }
                    else if (result == null)
                    {
                        return (int)Types.Error.SQLVIOLATION;
                    }
                    else
                    {
                        return (int)Types.Error.ERROR;
                    }
                }
            }
            catch (SqlException ex)
            {
                Helper.Logger(ex); //write error to log file
                return (int)Types.Error.ERROR;
            }

            catch (Exception ex)
            {
                Helper.Logger(ex); //write error to log file
                return (int)Types.Error.ERROR;
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
        public int EditSmileyQuestion(SmileyQuestion smileyQuestion)
        {
            ///
            /// Try to Update a new question into "Smiley_Questions" table
            ///
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand($@"
USE {connectionString.Name}

DECLARE @@MyOrder as INT
SET @@MyOrder = (SELECT [Order] FROM Questions WHERE ID = @ID)

IF NOT EXISTS (SELECT * FROM Questions 
                WHERE [Order] = @Order
                AND [Type] = @Type)
    BEGIN
	    UPDATE Questions
	    SET [Order] = @Order, [Text] = @Text
	    WHERE ID = @ID
    END

    if @@ROWCOUNT <> 0
    BEGIN
	    UPDATE Smiley_Questions
	    SET NumberOfSmileyFaces = @NumberOfSmileyFaces
	    WHERE ID = @ID
	    select @@ROWCOUNT
    END

ELSE
    BEGIN
		IF (@@MyOrder = @order)
		    BEGIN
	            UPDATE Questions
	            SET [Text] = @Text
	            WHERE ID = @ID
            END

            if @@ROWCOUNT <> 0
            BEGIN
	            UPDATE Smiley_Questions
	            SET NumberOfSmileyFaces = @NumberOfSmileyFaces
	            WHERE ID = @ID
	            select @@ROWCOUNT
            END
     END
", conn);

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@Order", smileyQuestion.Order),
                new SqlParameter("@Text", smileyQuestion.Text),
                new SqlParameter("@ID", smileyQuestion.ID),
                new SqlParameter("@Type", smileyQuestion.Type),
                new SqlParameter("@NumberOfSmileyFaces", smileyQuestion.NumberOfSmileyFaces)
                };
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    var result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        return (int)Types.Error.SUCCESS;
                    }
                    else if (result == null)
                    {
                        return (int)Types.Error.SQLVIOLATION;
                    }
                    else
                    {
                        return (int)Types.Error.ERROR;
                    }
                    //return cmd.ExecuteNonQuery() > 0 ? (int)CommonEnums.Error.SUCCESS : (int)CommonEnums.Error.ERROR;
                }
            }
            catch (SqlException ex)
            {
                //MessageBox.Show("SQL Error:\n" + ex.Message);
                Helper.Logger(ex); //write error to log file
                return (int)Types.Error.ERROR;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Something went wrong:\n" + ex.Message);
                Helper.Logger(ex); //write error to log file
                return (int)Types.Error.ERROR;
            }

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
        public int EditSliderQuestion(SliderQuestion sliderQuestion)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand($@"
USE {connectionString.Name}
DECLARE @@MyOrder as INT
SET @@MyOrder = (SELECT [Order] FROM Questions WHERE ID = @ID)

IF NOT EXISTS (SELECT * FROM Questions 
                WHERE [Order] = @Order
                AND [Type] = @Type)
    BEGIN
        UPDATE Questions
        SET [Order] = @Order, [Text] = @Text
        WHERE ID = @ID
    END
    if @@ROWCOUNT <> 0
    BEGIN
        UPDATE Slider_Questions
        SET StartValue = @StartValue, EndValue = @EndValue, StartValueCaption = @StartValueCaption, EndValueCaption = @EndValueCaption
        WHERE ID = @ID
	    select @@ROWCOUNT
    END

ELSE
    BEGIN
		IF (@@MyOrder = @order)
		    BEGIN
                UPDATE Questions
                SET [Order] = @Order, [Text] = @Text
                WHERE ID = @ID
            END
        if @@ROWCOUNT <> 0
            BEGIN
                UPDATE Slider_Questions
                SET StartValue = @StartValue, EndValue = @EndValue, StartValueCaption = @StartValueCaption, EndValueCaption = @EndValueCaption
                WHERE ID = @ID
	            select @@ROWCOUNT
            END
     END

", conn);

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@ID", sliderQuestion.ID),
                new SqlParameter("@Order", sliderQuestion.Order),
                new SqlParameter("@Text", sliderQuestion.Text),
                new SqlParameter("@Type", sliderQuestion.Type),
                new SqlParameter("@StartValue", sliderQuestion.StartValue),
                new SqlParameter("@EndValue", sliderQuestion.EndValue),
                new SqlParameter("@StartValueCaption", sliderQuestion.StartValueCaption),
                new SqlParameter("@EndValueCaption", sliderQuestion.EndValueCaption),
                };
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    var result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        return (int)Types.Error.SUCCESS;
                    }
                    else if (result == null)
                    {
                        return (int)Types.Error.SQLVIOLATION;
                    }
                    else
                    {
                        return (int)Types.Error.ERROR;
                    }
                    //return cmd.ExecuteNonQuery() > 0 ? (int)CommonEnums.Error.SUCCESS : (int)CommonEnums.Error.ERROR;
                    //MessageBox.Show("Question updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (SqlException ex)
            {
                Helper.Logger(ex); //write error to log file
                return (int)Types.Error.ERROR;
            }
            catch (Exception ex)
            {
                Helper.Logger(ex); //write error to log file
                return (int)Types.Error.ERROR;
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
        public int EditStarQuestion(StarQuestion starQuestion)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand($@"
USE {connectionString.Name}

DECLARE @@MyOrder as INT
SET @@MyOrder = (SELECT [Order] FROM Questions WHERE ID = @ID)

IF NOT EXISTS (SELECT * FROM Questions 
                WHERE [Order] = @Order
                AND [Type] = @Type)
    BEGIN
	    UPDATE Questions
	    SET [Order] = @Order, [Text] = @Text
	    WHERE ID = @ID
    END

    if @@ROWCOUNT <> 0
        BEGIN
            UPDATE Star_Questions
            SET NumberOfStars = @NumberOfStars
            WHERE ID = @ID
	        select @@ROWCOUNT
        END

ELSE
    BEGIN
		IF (@@MyOrder = @order)
		    BEGIN
	            UPDATE Questions
	            SET [Text] = @Text
	            WHERE ID = @ID
            END

            if @@ROWCOUNT <> 0
                BEGIN
                    UPDATE Star_Questions
                    SET NumberOfStars = @NumberOfStars
                    WHERE ID = @ID
	                select @@ROWCOUNT
                END
     END
", conn);

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@ID", starQuestion.ID),
                new SqlParameter("@Order", starQuestion.Order),
                new SqlParameter("@Text", starQuestion.Text),
                new SqlParameter("@Type", starQuestion.Type),
                new SqlParameter("@NumberOfStars", starQuestion.NumberOfStars)
                };
                    cmd.Parameters.AddRange(parameters);
                    conn.Open();
                    var result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        return (int)Types.Error.SUCCESS;
                    }
                    else if (result == null)
                    {
                        return (int)Types.Error.SQLVIOLATION;
                    }
                    else
                    {
                        return (int)Types.Error.ERROR;
                    }
                }
            }
            catch (SqlException ex)
            {
                Helper.Logger(ex); //write error to log file
                return (int)Types.Error.ERROR;
            }
            catch (Exception ex)
            {
                Helper.Logger(ex); //write error to log file
                return (int)Types.Error.ERROR;
            }
        } //end func.

        public int DeleteQuestion(int questionId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand($@"delete from Questions where ID = @ID", conn);

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@ID", questionId),
                };
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0 ? (int)Types.Error.SUCCESS : (int)Types.Error.ERROR;
                }
            }
            catch (SqlException ex)
            {
                Helper.Logger(ex); //write error to log file
                return (int)Types.Error.SQLVIOLATION;
            }
            catch (Exception ex)
            {
                Helper.Logger(ex); //write error to log file
                return (int)Types.Error.ERROR;
            }

        } //end func.

        public DataTable GetAllQuestions()
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString.ConnectionString))
                {
                    SqlDataAdapter adapter = null;
                    DataTable dt = new DataTable();
                    SqlCommand cmd = null;

                    cmd = new SqlCommand($@"
USE {connectionString.Name}
SELECT ID, [Order], [Type], [Text] FROM Questions", conn);
                    conn.Open();
                    adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                    return dt;
                }
            }
            catch (SqlException ex)
            {
                Helper.Logger(ex); //write error to log file
                return null;
            }
            catch (Exception ex)
            {
                Helper.Logger(ex); //write error to log file
                return null;
            }
        } //end func.

        public DataTable GetSingleSmileyQuestion(int questionId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString.ConnectionString))
                {
                    SqlDataAdapter adapter = null;
                    DataTable dt = new DataTable();
                    SqlCommand cmd = null;

                    cmd = new SqlCommand($@"
USE {connectionString.Name}
select Questions.[Order], Questions.[Text], Smiley_Questions.NumberOfSmileyFaces
from Questions
inner join Smiley_Questions
on Questions.ID = Smiley_Questions.ID
where Questions.ID = @ID", conn);

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@ID", questionId),
                };
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                    return dt;
                }
            }
            catch (SqlException ex)
            {
                Helper.Logger(ex); //write error to log file
                return null;
            }
            catch (Exception ex)
            {
                Helper.Logger(ex); //write error to log file
                return null;
            }
        } // end func.

        public DataTable GetSingleSliderQuestion(int questionId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString.ConnectionString))
                {
                    SqlDataAdapter adapter = null;
                    DataTable dt = new DataTable();
                    SqlCommand cmd = null;

                    cmd = new SqlCommand($@"
USE {connectionString.Name}
select Q.[Order], Q.[Text], SQ.StartValue, SQ.EndValue, SQ.StartValueCaption, SQ.EndValueCaption
from Questions AS Q
inner join Slider_Questions AS SQ
on Q.ID = SQ.ID
where Q.ID = @ID", conn);

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@ID", questionId),
                };
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                    return dt;
                }
            }
            catch (SqlException ex)
            {
                Helper.Logger(ex); //write error to log file
                return null;
            }
            catch (Exception ex)
            {
                Helper.Logger(ex); //write error to log file
                return null;
            }
        } // end func.

        public DataTable GetSingleStarQuestion(int questionId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString.ConnectionString))
                {
                    SqlDataAdapter adapter = null;
                    DataTable dt = new DataTable();
                    SqlCommand cmd = null;

                    cmd = new SqlCommand($@"
USE {connectionString.Name}
select Q.[Order], Q.[Text], StQ.NumberOfStars
from Questions AS Q
inner join Star_Questions AS StQ
on Q.ID = StQ.ID
where Q.ID = @ID", conn);

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@ID", questionId),
                };
                    cmd.Parameters.AddRange(parameters);


                    conn.Open();
                    adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                    return dt;
                }
            }
            catch (SqlException ex)
            {
                Helper.Logger(ex); //write error to log file
                return null;
            }
            catch (Exception ex)
            {
                Helper.Logger(ex); //write error to log file
                return null;
            }

        } // end func.

        //        public   int CheckIfTablesExist()
        //        {
        //            try
        //            {
        //                conn = new SqlConnection(connectionString.ConnectionString);
        //                SqlCommand cmd = null;


        //                ///
        //                /// Create Tables if they do NOT exist
        //                ///
        //                cmd = new SqlCommand($@"
        //USE [{connectionString.Name}]
        //IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Smiley_Questions]') AND type in (N'U'))
        //BEGIN
        //CREATE TABLE [dbo].[Smiley_Questions](
        //	[QuestionID] [int] IDENTITY(1,1) NOT NULL,
        //	[QuestionOrder] [int] NOT NULL,
        //	[QuestionText] [text] NOT NULL,
        //	[NumberOfSmileyFaces] [int] NOT NULL,
        // CONSTRAINT [PK_Smiley_Faces] PRIMARY KEY CLUSTERED 
        //(
        //	[QuestionID] ASC
        //)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
        //) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
        //END
        //", conn);
        //                conn.Open();
        //                return cmd.ExecuteNonQuery() > 0 ? (int)CommonEnums.Error.SUCCESS : (int)CommonEnums.Error.ERROR;
        //                conn.Close();


        //                cmd = new SqlCommand($@"
        //USE [{connectionString.Name}]
        //IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Slider_Questions]') AND type in (N'U'))
        //BEGIN
        //CREATE TABLE [dbo].[Slider_Questions](
        //	[QuestionID] [int] IDENTITY(1,1) NOT NULL,
        //	[QuestionOrder] [int] NOT NULL,
        //	[QuestionText] [text] NOT NULL,
        //	[QuestionStartValue] [int] NOT NULL,
        //	[QuestionEndValue] [int] NOT NULL,
        //	[StartValueCaption] [varchar](100) NOT NULL,
        //	[QuestionEndValueCaption] [varchar](100) NOT NULL,
        // CONSTRAINT [PK_Slider_Questions] PRIMARY KEY CLUSTERED 
        //(
        //	[QuestionID] ASC
        //)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
        // CONSTRAINT [IX_Slider_Questions] UNIQUE NONCLUSTERED 
        //(
        //	[QuestionOrder] ASC
        //)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
        //) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
        //END
        //", conn);
        //                conn.Open();
        //                return cmd.ExecuteNonQuery() > 0 ? (int)CommonEnums.Error.SUCCESS : (int)CommonEnums.Error.ERROR;
        //                conn.Close();


        //                cmd = new SqlCommand($@"
        //USE [{connectionString.Name}]
        //IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Star_Questions]') AND type in (N'U'))
        //BEGIN
        //CREATE TABLE [dbo].[Star_Questions](
        //	[QuestionID] [int] IDENTITY(1,1) NOT NULL,
        //	[QuestionOrder] [int] NOT NULL,
        //	[QuestionText] [text] NOT NULL,
        //	[NumberOfStars] [int] NOT NULL,
        // CONSTRAINT [PK_Stars_Questions] PRIMARY KEY CLUSTERED 
        //(
        //	[QuestionID] ASC
        //)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
        // CONSTRAINT [IX_Stars_Questions] UNIQUE NONCLUSTERED 
        //(
        //	[QuestionOrder] ASC
        //)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
        //) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
        //END
        //", conn);
        //                conn.Open();
        //                return cmd.ExecuteNonQuery() > 0 ? (int)CommonEnums.Error.SUCCESS : (int)CommonEnums.Error.ERROR;
        //                conn.Close();

        //                return (int)CommonEnums.ErrorState.SUCCESS;
        //            }
        //            catch (SqlException ex)
        //            {
        //                //2627 -> unique key violation
        //                // ex.Number
        //                if (ex.Number == 2627)
        //                {
        //                    //MessageBox.Show("This Question order is already in use\nTry using another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                    return (int)CommonEnums.ErrorState.SQLVIOLATION;
        //                }
        //                else
        //                {
        //                    //MessageBox.Show("SQL Error:\n" + ex.Message);
        //                    Helper.Logger(ex); //write error to log file
        //                    return (int)CommonEnums.ErrorState.ERROR;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                //MessageBox.Show("Something went wrong:\n" + ex.Message);
        //                Helper.Logger(ex); //write error to log file
        //                return (int)CommonEnums.ErrorState.ERROR;
        //            }
        //            finally
        //            {
        //                conn.Close();
        //            }
        //        } //end func.
    }
}
