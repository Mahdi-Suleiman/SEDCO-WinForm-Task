﻿using SurveyQuestionsConfigurator.CommonHelpers;
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
using static SurveyQuestionsConfigurator.Entities.Generic;

namespace SurveyQuestionsConfigurator.DataAccess
{
    public class DbConnect
    {
        private ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings[0]; //get connection string information from App.config

        #region INSERT Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="questionOrder"></param>
        /// <param name="NumberOfSmilyFaces"></param>
        /// <param name="questionText"></param>
        /// <returns>
        /// 1 -> Success
        /// 2 -> Unique key violation
        /// -1 -> ErrorCode
        /// </returns>
        public ErrorCode AddSmileyQuestion(SmileyQuestion smileyQuestion)
        {
            ///
            /// Try to insert a new question into "Smiley_Questions" table
            ///
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand($@"

                    IF ((SELECT COUNT(ID) FROM Questions WHERE [Order] = @{QuestionColumn.ORDER}) = 0)

                    BEGIN TRANSACTION

                    BEGIN
                         INSERT INTO Questions 
                         ([Order], [Text], [Type])
                         VALUES 
                         (@{QuestionColumn.ORDER}, @{QuestionColumn.TEXT}, @{QuestionColumn.TYPE})
                    END
                    IF (@@IDENTITY IS NOT NULL)
                    BEGIN
                         INSERT INTO Smiley_Questions(ID, NumberOfSmileyFaces )
                         OUTPUT @@IDENTITY
                         VALUES (@@IDENTITY, @{QuestionColumn.NUMBER_OF_SMILEY_FACES})
                    END

                    COMMIT TRANSACTION
", conn);

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter($"{QuestionColumn.ORDER}", smileyQuestion.Order),
                new SqlParameter($"{QuestionColumn.TEXT}", smileyQuestion.Text),
                new SqlParameter($"{QuestionColumn.TYPE}", smileyQuestion.Type),
                new SqlParameter($"{QuestionColumn.NUMBER_OF_SMILEY_FACES}", smileyQuestion.NumberOfSmileyFaces)
                };
                    cmd.Parameters.AddRange(parameters);
                    conn.Open();
                    var result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        return Generic.ErrorCode.SUCCESS;
                    }
                    else if (result == null)
                    {
                        return Generic.ErrorCode.SQL_VIOLATION;
                    }
                    else
                    {
                        return Generic.ErrorCode.ERROR;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); //write error to log file
                return Generic.ErrorCode.ERROR;
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
        /// -1 -> ErrorCode
        /// </returns>
        public ErrorCode AddSliderQuestion(SliderQuestion sliderQuestion)
        {
            ///
            /// Try to insert a new question into "Slider_Questions" table
            ///
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand($@"
                    IF ((SELECT COUNT(ID) FROM Questions WHERE [Order] = @{QuestionColumn.ORDER}) = 0)

                    BEGIN TRANSACTION

                        BEGIN
                            INSERT INTO Questions 
                            ([Order], [Text], [Type])
                            VALUES 
                            (@{QuestionColumn.ORDER}, @{QuestionColumn.TEXT}, @{QuestionColumn.TYPE})
                        END
                    IF (@@IDENTITY IS NOT NULL)
                        BEGIN
                            INSERT INTO Slider_Questions
                            (ID, StartValue, EndValue, StartValueCaption, EndValueCaption)
                            OUTPUT @@IDENTITY
                            VALUES (@@IDENTITY, @{QuestionColumn.START_VALUE}, @{QuestionColumn.END_VALUE}, @{QuestionColumn.START_VALUE_CAPTION}, @{QuestionColumn.END_VALUE_CAPTION})
                        END

                    COMMIT TRANSACTION
", conn);

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter($"{QuestionColumn.ORDER}", sliderQuestion.Order),
                new SqlParameter($"{QuestionColumn.TEXT}", sliderQuestion.Text),
                new SqlParameter($"{QuestionColumn.TYPE}",sliderQuestion.Type),
                new SqlParameter($"{QuestionColumn.START_VALUE}", sliderQuestion.StartValue),
                new SqlParameter($"{QuestionColumn.END_VALUE}", sliderQuestion.EndValue),
                new SqlParameter($"{QuestionColumn.START_VALUE_CAPTION}", sliderQuestion.StartValueCaption),
                new SqlParameter($"{QuestionColumn.END_VALUE_CAPTION}", sliderQuestion.EndValueCaption),
                };
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    var result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        return Generic.ErrorCode.SUCCESS;
                    }
                    else if (result == null)
                    {
                        return Generic.ErrorCode.SQL_VIOLATION;
                    }
                    else
                    {
                        return Generic.ErrorCode.ERROR;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); //write error to log file
                return Generic.ErrorCode.ERROR;
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
        /// -1 -> ErrorCode
        /// </returns>
        public ErrorCode AddStarQuestion(StarQuestion starQuestion)
        {
            ///
            // Try to insert a new question into "Star_Questions" table
            //
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand($@"

                    IF ((SELECT COUNT(ID) FROM Questions WHERE [Order] = @{QuestionColumn.ORDER}) = 0)

                    BEGIN TRANSACTION

                        BEGIN
                        INSERT INTO Questions 
                            ([Order], [Text], [Type])
                            VALUES 
                            (@{QuestionColumn.ORDER}, @{QuestionColumn.TEXT}, @{QuestionColumn.TYPE})
                        END
                    IF (@@IDENTITY IS NOT NULL)
                        BEGIN
                            INSERT INTO Star_Questions(ID,  NumberOfStars)
                            OUTPUT @@IDENTITY
                            VALUES (@@IDENTITY, @{QuestionColumn.NUMBER_OF_STARS})
                        END

                    COMMIT TRANSACTION
", conn);
                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter($"{QuestionColumn.ORDER}", starQuestion.Order),
                new SqlParameter($"{QuestionColumn.TEXT}", starQuestion.Text),
                new SqlParameter($"{QuestionColumn.TYPE}",starQuestion.Type),
                new SqlParameter($"{QuestionColumn.NUMBER_OF_STARS}", starQuestion.NumberOfStars)
                };
                    cmd.Parameters.AddRange(parameters);
                    conn.Open();
                    var result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        return Generic.ErrorCode.SUCCESS;
                    }
                    else if (result == null)
                    {
                        return Generic.ErrorCode.SQL_VIOLATION;
                    }
                    else
                    {
                        return Generic.ErrorCode.ERROR;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); //write error to log file
                return Generic.ErrorCode.ERROR;
            }

        }// end of function

        #endregion

        #region UPDATE Methods

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
        /// -1 -> ErrorCode
        /// </returns>
        public ErrorCode EditSmileyQuestion(SmileyQuestion smileyQuestion)
        {
            ///
            /// Try to Update a new question into "Smiley_Questions" table
            ///
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand($@"

                    DECLARE @@MyOrder as INT
                    SET @@MyOrder = (SELECT [Order] FROM Questions WHERE ID = @{QuestionColumn.ID})

                    IF ((SELECT COUNT(ID) FROM Questions WHERE [Order] = @{QuestionColumn.ORDER}) = 0)

                    BEGIN TRANSACTION

                        BEGIN
	                        UPDATE Questions
	                        SET [Order] = @{QuestionColumn.ORDER}, [Text] = @{QuestionColumn.TEXT}
	                        WHERE ID = @{QuestionColumn.ID}
                        END

                    IF @@ROWCOUNT <> 0
                        BEGIN
	                        UPDATE Smiley_Questions
	                        SET NumberOfSmileyFaces = @{QuestionColumn.NUMBER_OF_SMILEY_FACES}
	                        WHERE ID = @{QuestionColumn.ID}
	                        select @@ROWCOUNT
                        END

                    ELSE
                        BEGIN
		                    IF (@@MyOrder = @{QuestionColumn.ORDER})
		                        BEGIN
	                                UPDATE Questions
	                                SET [Text] = @{QuestionColumn.TEXT}
	                                WHERE ID = @{QuestionColumn.ID}
                                END

                            IF @@ROWCOUNT <> 0
                                BEGIN
	                                UPDATE Smiley_Questions
	                                SET NumberOfSmileyFaces = @{QuestionColumn.NUMBER_OF_SMILEY_FACES}
	                                WHERE ID = @{QuestionColumn.ID}
	                                select @@ROWCOUNT
                                END
                         END

                    COMMIT TRANSACTION
", conn);

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter($"{QuestionColumn.ORDER}", smileyQuestion.Order),
                new SqlParameter($"{QuestionColumn.TEXT}", smileyQuestion.Text),
                new SqlParameter($"{QuestionColumn.ID}", smileyQuestion.ID),
                new SqlParameter($"{QuestionColumn.TYPE}", smileyQuestion.Type),
                new SqlParameter($"{QuestionColumn.NUMBER_OF_SMILEY_FACES}", smileyQuestion.NumberOfSmileyFaces)
                };
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    var result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        return Generic.ErrorCode.SUCCESS;
                    }
                    else if (result == null)
                    {
                        return Generic.ErrorCode.SQL_VIOLATION;
                    }
                    else
                    {
                        return Generic.ErrorCode.ERROR;
                    }
                    //return cmd.ExecuteNonQuery() > 0 ?  CommonEnums.ErrorCode.SUCCESS :  CommonEnums.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Something went wrong:\n" + ex.Message);
                Logger.LogError(ex); //write error to log file
                return Generic.ErrorCode.ERROR;
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
        /// -1 -> ErrorCode
        /// </returns>
        public ErrorCode EditSliderQuestion(SliderQuestion sliderQuestion)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand($@"

                    DECLARE @@MyOrder as INT
                    SET @@MyOrder = (SELECT [Order] FROM Questions WHERE ID = @{QuestionColumn.ID})

                    IF ((SELECT COUNT(ID) FROM Questions WHERE [Order] = @{QuestionColumn.ORDER}) = 0)

                    BEGIN TRANSACTION

                        BEGIN
                            UPDATE Questions
                            SET [Order] = @{QuestionColumn.ORDER}, [Text] = @{QuestionColumn.TEXT}
                            WHERE ID = @{QuestionColumn.ID}
                        END
                    IF @@ROWCOUNT <> 0
                        BEGIN
                            UPDATE Slider_Questions
                            SET StartValue = @{QuestionColumn.START_VALUE}, EndValue = @{QuestionColumn.END_VALUE}, StartValueCaption = @{QuestionColumn.START_VALUE_CAPTION}, EndValueCaption = @{QuestionColumn.END_VALUE_CAPTION}
                            WHERE ID = @{QuestionColumn.ID}
	                        select @@ROWCOUNT
                        END

                    ELSE
                        BEGIN
		                    IF (@@MyOrder = @{QuestionColumn.ORDER})
		                        BEGIN
                                    UPDATE Questions
                                    SET [Order] = @{QuestionColumn.ORDER}, [Text] = @{QuestionColumn.TEXT}
                                    WHERE ID = @{QuestionColumn.ID}
                                END
                            IF @@ROWCOUNT <> 0
                                BEGIN
                                    UPDATE Slider_Questions
                                    SET StartValue = @{QuestionColumn.START_VALUE}, EndValue = @{QuestionColumn.END_VALUE}, StartValueCaption = @{QuestionColumn.START_VALUE_CAPTION}, EndValueCaption = @{QuestionColumn.END_VALUE_CAPTION}
                                    WHERE ID = @{QuestionColumn.ID}
	                                select @@ROWCOUNT
                                END
                         END

                    COMMIT TRANSACTION
", conn);

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter($"{QuestionColumn.ID}", sliderQuestion.ID),
                new SqlParameter($"{QuestionColumn.ORDER}", sliderQuestion.Order),
                new SqlParameter($"{QuestionColumn.TEXT}", sliderQuestion.Text),
                new SqlParameter($"{QuestionColumn.TYPE}", sliderQuestion.Type),
                new SqlParameter($"{QuestionColumn.START_VALUE}", sliderQuestion.StartValue),
                new SqlParameter($"{QuestionColumn.END_VALUE}", sliderQuestion.EndValue),
                new SqlParameter($"{QuestionColumn.START_VALUE_CAPTION}", sliderQuestion.StartValueCaption),
                new SqlParameter($"{QuestionColumn.END_VALUE_CAPTION}", sliderQuestion.EndValueCaption),
                };
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    var result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        return Generic.ErrorCode.SUCCESS;
                    }
                    else if (result == null)
                    {
                        return Generic.ErrorCode.SQL_VIOLATION;
                    }
                    else
                    {
                        return Generic.ErrorCode.ERROR;
                    }
                    //return cmd.ExecuteNonQuery() > 0 ?  CommonEnums.ErrorCode.SUCCESS :  CommonEnums.ErrorCode.ERROR;
                    //MessageBox.Show("Question updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); //write error to log file
                return Generic.ErrorCode.ERROR;
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
        /// -1 -> ErrorCode
        /// </returns>
        public ErrorCode EditStarQuestion(StarQuestion starQuestion)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand($@"

                    DECLARE @@MyOrder as INT
                    SET @@MyOrder = (SELECT [Order] FROM Questions WHERE ID = @{QuestionColumn.ID})

                    IF ((SELECT COUNT(ID) FROM Questions WHERE [Order] = @{QuestionColumn.ORDER}) = 0)

                    BEGIN TRANSACTION

                        BEGIN
	                        UPDATE Questions
	                        SET [Order] = @{QuestionColumn.ORDER}, [Text] = @{QuestionColumn.TEXT}
	                        WHERE ID = @{QuestionColumn.ID}
                        END

                    IF @@ROWCOUNT <> 0
                            BEGIN
                                UPDATE Star_Questions
                                SET NumberOfStars = @{QuestionColumn.NUMBER_OF_STARS}
                                WHERE ID = @{QuestionColumn.ID}
	                            select @@ROWCOUNT
                            END

                    ELSE
                        BEGIN
		                    IF (@@MyOrder = @{QuestionColumn.ORDER})
		                        BEGIN
	                                UPDATE Questions
	                                SET [Text] = @{QuestionColumn.TEXT}
	                                WHERE ID = @{QuestionColumn.ID}
                                END

                            IF @@ROWCOUNT <> 0
                                BEGIN
                                    UPDATE Star_Questions
                                    SET NumberOfStars = @{QuestionColumn.NUMBER_OF_STARS}
                                    WHERE ID = @{QuestionColumn.ID}
	                                select @@ROWCOUNT
                                END
                         END

                    COMMIT TRANSACTION
", conn);
                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter($"{QuestionColumn.ID}", starQuestion.ID),
                new SqlParameter($"{QuestionColumn.ORDER}", starQuestion.Order),
                new SqlParameter($"{QuestionColumn.TEXT}", starQuestion.Text),
                new SqlParameter($"{QuestionColumn.TYPE}", starQuestion.Type),
                new SqlParameter($"{QuestionColumn.NUMBER_OF_STARS}", starQuestion.NumberOfStars)
                };
                    cmd.Parameters.AddRange(parameters);
                    conn.Open();
                    var result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        return Generic.ErrorCode.SUCCESS;
                    }
                    else if (result == null)
                    {
                        return Generic.ErrorCode.SQL_VIOLATION;
                    }
                    else
                    {
                        return Generic.ErrorCode.ERROR;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); //write error to log file
                return Generic.ErrorCode.ERROR;
            }
        } //end func.

        #endregion

        #region DELETE Methods

        public ErrorCode DeleteQuestion(int questionId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand($@"
                    BEGIN TRANSACTION

                    delete from Questions where ID = @{QuestionColumn.ID}

                    COMMIT TRANSACTION
", conn);

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter($"{QuestionColumn.ID}", questionId),
                };
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0 ? Generic.ErrorCode.SUCCESS : Generic.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); //write error to log file
                return Generic.ErrorCode.ERROR;
            }

        } //end func.

        #endregion

        #region GET Methods

        public ErrorCode GetAllQuestions(ref List<Question> questionsList)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString.ConnectionString))
                {
                    SqlDataAdapter adapter = null;
                    DataTable dataTable = new DataTable();
                    SqlCommand cmd = null;

                    cmd = new SqlCommand($@"

                    SELECT [ID], [Order], [Text], [Type] FROM Questions
", conn);
                    conn.Open();
                    adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dataTable);

                    foreach (DataRow row in dataTable.Rows)
                    {
                        Question q = new Question((int)(int)row[$"{QuestionColumn.ID}"], (int)row[$"{QuestionColumn.ORDER}"], (string)row[$"{QuestionColumn.TEXT}"], (QuestionType)row[$"{QuestionColumn.TYPE}"]);
                        questionsList.Add(q);
                    }
                    return questionsList.Count >= 0 ? Generic.ErrorCode.SUCCESS : Generic.ErrorCode.ERROR; // RETURN INT32
                }
            }
            catch (SqlException ex)
            {
                Logger.LogError(ex); //write error to log file
                return Generic.ErrorCode.SQL_VIOLATION;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); //write error to log file
                return Generic.ErrorCode.ERROR;
            }
        } //end func.

        public ErrorCode GetSmileyQuestionByID(ref SmileyQuestion smileyQuestion)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString.ConnectionString))
                {
                    SqlDataAdapter adapter = null;
                    DataTable dataTable = new DataTable();
                    SqlCommand cmd = null;

                    cmd = new SqlCommand($@"

                    select Q.[ID], Q.[Order], Q.[Text], Q.[Type], SmQ.NumberOfSmileyFaces
                    from Questions AS Q
                    inner join Smiley_Questions AS SmQ
                    on Q.ID = SmQ.ID
                    where Q.ID = @{QuestionColumn.ID}
", conn);

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter($"{QuestionColumn.ID}", smileyQuestion.ID),
                };
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dataTable);

                    foreach (DataRow row in dataTable.Rows)
                    {
                        smileyQuestion = new SmileyQuestion((int)row[$"{QuestionColumn.ID}"], (int)row[$"{QuestionColumn.ORDER}"], (string)row[$"{QuestionColumn.TEXT}"], (QuestionType)row[$"{QuestionColumn.TYPE}"], (int)row[$"{QuestionColumn.NUMBER_OF_SMILEY_FACES}"]);
                    }

                    return smileyQuestion.NumberOfSmileyFaces >= 0 ? Generic.ErrorCode.SUCCESS : Generic.ErrorCode.ERROR; // RETURN INT32
                }
            }
            catch (SqlException ex)
            {
                Logger.LogError(ex); //write error to log file
                return Generic.ErrorCode.SQL_VIOLATION;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); //write error to log file
                return Generic.ErrorCode.ERROR;
            }
        } // end func.

        public ErrorCode GetSliderQuestionByID(ref SliderQuestion sliderQuestion)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString.ConnectionString))
                {
                    SqlDataAdapter adapter = null;
                    DataTable dataTable = new DataTable();
                    SqlCommand cmd = null;

                    cmd = new SqlCommand($@"

                    select Q.[ID], Q.[Order], Q.[Text], Q.[Type], SQ.StartValue, SQ.EndValue, SQ.StartValueCaption, SQ.EndValueCaption
                    from Questions AS Q
                    inner join Slider_Questions AS SQ
                    on Q.ID = SQ.ID
                    where Q.ID = @{QuestionColumn.ID}
", conn);

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter($"{QuestionColumn.ID}", sliderQuestion.ID),
                };
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dataTable);

                    foreach (DataRow row in dataTable.Rows)
                    {
                        sliderQuestion = new SliderQuestion((int)row[$"{QuestionColumn.ID}"], (int)row[$"{QuestionColumn.ORDER}"], (string)row[$"{QuestionColumn.TEXT}"], (QuestionType)row[$"{QuestionColumn.TYPE}"],
                            (int)row[$"{QuestionColumn.START_VALUE}"], (int)row[$"{QuestionColumn.END_VALUE}"], (string)row[$"{QuestionColumn.START_VALUE_CAPTION}"], (string)row[$"{QuestionColumn.END_VALUE_CAPTION}"]);
                    }

                    return sliderQuestion.StartValue >= 0 ? Generic.ErrorCode.SUCCESS : Generic.ErrorCode.ERROR; // RETURN INT32
                }
            }
            catch (SqlException ex)
            {
                Logger.LogError(ex); //write error to log file
                return Generic.ErrorCode.SQL_VIOLATION;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); //write error to log file
                return Generic.ErrorCode.ERROR;
            }
        } // end func.

        public ErrorCode GetStarQuestionByID(ref StarQuestion starQuestion)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString.ConnectionString))
                {
                    SqlDataAdapter adapter = null;
                    DataTable dataTable = new DataTable();
                    SqlCommand cmd = null;

                    cmd = new SqlCommand($@"

                    select Q.[ID], Q.[Order], Q.[Text], Q.[Type], StQ.NumberOfStars
                    from Questions AS Q
                    inner join Star_Questions AS StQ
                    on Q.ID = StQ.ID
                    where Q.ID = @{QuestionColumn.ID}
", conn);

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter($"{QuestionColumn.ID}", starQuestion.ID),
                };
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dataTable);

                    foreach (DataRow row in dataTable.Rows)
                    {
                        starQuestion = new StarQuestion((int)row[$"{QuestionColumn.ID}"], (int)row[$"{QuestionColumn.ORDER}"], (string)row[$"{QuestionColumn.TEXT}"], (QuestionType)row[$"{QuestionColumn.TYPE}"], (int)row[$"{QuestionColumn.NUMBER_OF_STARS}"]);
                    }

                    return starQuestion.NumberOfStars >= 0 ? Generic.ErrorCode.SUCCESS : Generic.ErrorCode.ERROR; // RETURN INT32
                }
            }
            catch (SqlException ex)
            {
                Logger.LogError(ex); //write error to log file
                return Generic.ErrorCode.SQL_VIOLATION;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); //write error to log file
                return Generic.ErrorCode.ERROR;
            }

        } // end func.

        #endregion

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
        //	[QuestionID] [int] ID(1,1) NOT NULL,
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
        //                return cmd.ExecuteNonQuery() > 0 ?  CommonEnums.ErrorCode.SUCCESS :  CommonEnums.ErrorCode.ERROR;
        //                conn.Close();


        //                cmd = new SqlCommand($@"
        //USE [{connectionString.Name}]
        //IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Slider_Questions]') AND type in (N'U'))
        //BEGIN
        //CREATE TABLE [dbo].[Slider_Questions](
        //	[QuestionID] [int] ID(1,1) NOT NULL,
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
        //                return cmd.ExecuteNonQuery() > 0 ?  CommonEnums.ErrorCode.SUCCESS :  CommonEnums.ErrorCode.ERROR;
        //                conn.Close();


        //                cmd = new SqlCommand($@"
        //USE [{connectionString.Name}]
        //IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Star_Questions]') AND type in (N'U'))
        //BEGIN
        //CREATE TABLE [dbo].[Star_Questions](
        //	[QuestionID] [int] ID(1,1) NOT NULL,
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
        //                return cmd.ExecuteNonQuery() > 0 ?  CommonEnums.ErrorCode.SUCCESS :  CommonEnums.ErrorCode.ERROR;
        //                conn.Close();

        //                return  CommonEnums.ErrorState.SUCCESS;
        //            }
        //            catch (SqlException ex)
        //            {
        //                //2627 -> unique key violation
        //                // ex.Number
        //                if (ex.Number == 2627)
        //                {
        //                    //MessageBox.Show("This Question order is already in use\nTry using another one", "ErrorCode", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                    return  CommonEnums.ErrorState.SQL_VIOLATION;
        //                }
        //                else
        //                {
        //                    //MessageBox.Show("SQL ErrorCode:\n" + ex.Message);
        //                    Logger.LogError(ex); //write error to log file
        //                    return  CommonEnums.ErrorState.ERROR;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                //MessageBox.Show("Something went wrong:\n" + ex.Message);
        //                Logger.LogError(ex); //write error to log file
        //                return  CommonEnums.ErrorState.ERROR;
        //            }
        //            finally
        //            {
        //                conn.Close();
        //            }
        //        } //end func.
    }
}
