using SurveyQuestionsConfigurator.CommonHelpers;
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
using System.Transactions;
using static SurveyQuestionsConfigurator.Entities.Generic;
namespace SurveyQuestionsConfigurator.DataAccess
{
    public class DbConnect
    {
        private ConnectionStringSettings sqlConnectionectionSetting = ConfigurationManager.ConnectionStrings[0]; //get sqlConnectionection string information from App.config

        #region INSERT Methods

        private ErrorCode InsertQuestion(TransactionScope transactionScope, SqlConnection sqlConnection, Question question)
        {
            try
            {
                ErrorCode returnedErrorCode = ErrorCode.ERROR;
                using (SqlCommand cmd = sqlConnection.CreateCommand())
                {
                    cmd.CommandText = $@"
                            DECLARE @RESULT INT
                            EXECUTE @RESULT = INSERT_QUESTION @Order = @{QuestionColumn.Order}, @Text = @{QuestionColumn.Text}, @Type = @{QuestionColumn.Type}
                            SELECT @RESULT";

                    SqlParameter[] parameters = new SqlParameter[] {
                                new SqlParameter($"{QuestionColumn.Order}", question.Order),
                                new SqlParameter($"{QuestionColumn.Text}", question.Text),
                                new SqlParameter($"{QuestionColumn.Type}", question.Type)};
                    cmd.Parameters.AddRange(parameters);

                    returnedErrorCode = (ErrorCode)cmd.ExecuteScalar();
                    return returnedErrorCode;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); //write error to log file
                return ErrorCode.ERROR;
            }
        }

        private ErrorCode UpdateQuestion(TransactionScope transactionScope, SqlConnection sqlConnection, Question question)
        {
            try
            {
                ErrorCode returnedErrorCode = ErrorCode.ERROR;
                using (SqlCommand cmd = sqlConnection.CreateCommand())
                {
                    cmd.CommandText = $@"
                            DECLARE @RESULT INT
                            EXECUTE @RESULT = UPDATE_QUESTION @ID = @{QuestionColumn.ID}, @ORDER =  @{QuestionColumn.Order}, @Text = @{QuestionColumn.Text}
                            SELECT @RESULT";

                    SqlParameter[] parameters = new SqlParameter[] {
                                new SqlParameter($"{QuestionColumn.Order}", question.Order),
                                new SqlParameter($"{QuestionColumn.Text}", question.Text),
                                new SqlParameter($"{QuestionColumn.ID}", question.ID)};
                    cmd.Parameters.AddRange(parameters);

                    returnedErrorCode = (ErrorCode)cmd.ExecuteScalar();
                    return returnedErrorCode;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); //write error to log file
                return ErrorCode.ERROR;
            }
        }


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
        public ErrorCode InsertSmileyQuestion(SmileyQuestion smileyQuestion)
        {
            ///
            /// Try to insert a new question into "Smiley_Questions" table
            ///
            try
            {
                ErrorCode returnedErrorCode = ErrorCode.ERROR;
                int rowsAffected = 0;

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    using (SqlConnection sqlConnection = new SqlConnection())
                    {
                        sqlConnection.ConnectionString = sqlConnectionectionSetting.ConnectionString;
                        sqlConnection.Open();

                        returnedErrorCode = InsertQuestion(transactionScope, sqlConnection, smileyQuestion);

                        if (returnedErrorCode == ErrorCode.SQL_VIOLATION)
                        {
                            return ErrorCode.SQL_VIOLATION;
                        }
                        else if (returnedErrorCode == ErrorCode.ERROR)
                        {
                            return ErrorCode.ERROR;
                        }

                        using (SqlCommand cmd = sqlConnection.CreateCommand())
                        {
                            cmd.CommandText = $@"
                            INSERT INTO Smiley_Questions(ID, NumberOfSmileyFaces )
                            VALUES (@@IDENTITY, @{QuestionColumn.NumberOfSmileyFaces})";

                            SqlParameter[] parameters = new SqlParameter[] {
                                new SqlParameter($"{QuestionColumn.NumberOfSmileyFaces}", smileyQuestion.NumberOfSmileyFaces)};
                            cmd.Parameters.AddRange(parameters);

                            rowsAffected = cmd.ExecuteNonQuery();
                        }

                        if (returnedErrorCode == ErrorCode.SUCCESS && rowsAffected != 0)
                        {
                            transactionScope.Complete();
                            return ErrorCode.SUCCESS;
                        }

                        return ErrorCode.ERROR;
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
        /// Try to insert a new question into "Slider_Questions" table
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
        public ErrorCode InsertSliderQuestion(SliderQuestion sliderQuestion)
        {

            try
            {
                ErrorCode returnedErrorCode = ErrorCode.ERROR;
                int rowsAffected = 0;

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    using (SqlConnection sqlConnection = new SqlConnection())
                    {
                        sqlConnection.ConnectionString = sqlConnectionectionSetting.ConnectionString;
                        sqlConnection.Open();

                        returnedErrorCode = InsertQuestion(transactionScope, sqlConnection, sliderQuestion);

                        if (returnedErrorCode == ErrorCode.SQL_VIOLATION || returnedErrorCode == ErrorCode.ERROR)
                        {
                            return returnedErrorCode;
                        }

                        using (SqlCommand cmd = sqlConnection.CreateCommand())
                        {
                            cmd.CommandText = $@"
                            INSERT INTO Slider_Questions
                            (ID, StartValue, EndValue, StartValueCaption, EndValueCaption)
                            VALUES (@@IDENTITY, @{QuestionColumn.StartValue}, @{QuestionColumn.EndValue},
                                    @{QuestionColumn.StartValueCaption}, @{QuestionColumn.EndValueCaption})";

                            SqlParameter[] parameters = new SqlParameter[] {
                                new SqlParameter($"{QuestionColumn.StartValue}", sliderQuestion.StartValue),
                                new SqlParameter($"{QuestionColumn.EndValue}", sliderQuestion.EndValue),
                                new SqlParameter($"{QuestionColumn.StartValueCaption}", sliderQuestion.StartValueCaption),
                                new SqlParameter($"{QuestionColumn.EndValueCaption}", sliderQuestion.EndValueCaption)};
                            cmd.Parameters.AddRange(parameters);

                            rowsAffected = cmd.ExecuteNonQuery();
                        }

                        if (returnedErrorCode == ErrorCode.SUCCESS && rowsAffected != 0)
                        {
                            transactionScope.Complete();
                            return ErrorCode.SUCCESS;
                        }

                        return ErrorCode.ERROR;
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
        /// Try to insert a new question into "Star_Questions" table
        /// </summary>
        /// <param name="questionOrder"></param>
        /// <param name="questionText"></param>
        /// <param name="numberOfStars"></param>
        /// <returns>
        /// 1 -> Success
        /// 2 -> Unique key violation
        /// -1 -> ErrorCode
        /// </returns>
        public ErrorCode InsertStarQuestion(StarQuestion starQuestion)
        {
            try
            {
                ErrorCode returnedErrorCode = ErrorCode.ERROR;
                int rowsAffected = 0;

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    using (SqlConnection sqlConnection = new SqlConnection())
                    {
                        sqlConnection.ConnectionString = sqlConnectionectionSetting.ConnectionString;
                        sqlConnection.Open();

                        returnedErrorCode = InsertQuestion(transactionScope, sqlConnection, starQuestion);

                        if (returnedErrorCode == ErrorCode.SQL_VIOLATION || returnedErrorCode == ErrorCode.ERROR)
                        {
                            return returnedErrorCode;
                        }

                        using (SqlCommand cmd = sqlConnection.CreateCommand())
                        {
                            cmd.CommandText = $@"
                            INSERT INTO Star_Questions(ID,  NumberOfStars)
                            VALUES (@@IDENTITY, @{QuestionColumn.NumberOfStars})";

                            SqlParameter[] parameters = new SqlParameter[] {
                                 new SqlParameter($"{QuestionColumn.NumberOfStars}", starQuestion.NumberOfStars)
                            };
                            cmd.Parameters.AddRange(parameters);

                            rowsAffected = cmd.ExecuteNonQuery();
                        }

                        if (returnedErrorCode == ErrorCode.SUCCESS && rowsAffected != 0)
                        {
                            transactionScope.Complete();
                            return ErrorCode.SUCCESS;
                        }

                        return ErrorCode.ERROR;
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
        public ErrorCode UpdateSmileyQuestion(SmileyQuestion smileyQuestion)
        {
            ///
            /// Try to Update a new question into "Smiley_Questions" table
            ///
            try
            {
                ErrorCode returnedErrorCode = ErrorCode.ERROR;
                int rowsAffected = 0;

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    using (SqlConnection sqlConnection = new SqlConnection())
                    {
                        sqlConnection.ConnectionString = sqlConnectionectionSetting.ConnectionString;
                        sqlConnection.Open();

                        returnedErrorCode = UpdateQuestion(transactionScope, sqlConnection, smileyQuestion);

                        if (returnedErrorCode == ErrorCode.SQL_VIOLATION || returnedErrorCode == ErrorCode.ERROR)
                        {
                            return returnedErrorCode;
                        }

                        using (SqlCommand cmd = sqlConnection.CreateCommand())
                        {
                            cmd.CommandText = $@"
	                        UPDATE Smiley_Questions
	                        SET NumberOfSmileyFaces = @{QuestionColumn.NumberOfSmileyFaces}
	                        WHERE ID = @{QuestionColumn.ID}";

                            SqlParameter[] parameters = new SqlParameter[] {
                                new SqlParameter($"{QuestionColumn.ID}", smileyQuestion.ID),
                                new SqlParameter($"{QuestionColumn.NumberOfSmileyFaces}", smileyQuestion.NumberOfSmileyFaces)
                            };
                            cmd.Parameters.AddRange(parameters);

                            rowsAffected = cmd.ExecuteNonQuery();
                        }

                        if (returnedErrorCode == ErrorCode.SUCCESS && rowsAffected != 0)
                        {
                            transactionScope.Complete();
                            return ErrorCode.SUCCESS;
                        }

                        return ErrorCode.ERROR;

                    }
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
        public ErrorCode UpdateSliderQuestion(SliderQuestion sliderQuestion)
        {
            try
            {
                ErrorCode returnedErrorCode = ErrorCode.ERROR;
                int rowsAffected = 0;

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    using (SqlConnection sqlConnection = new SqlConnection())
                    {
                        sqlConnection.ConnectionString = sqlConnectionectionSetting.ConnectionString;
                        sqlConnection.Open();

                        returnedErrorCode = UpdateQuestion(transactionScope, sqlConnection, sliderQuestion);

                        if (returnedErrorCode == ErrorCode.SQL_VIOLATION || returnedErrorCode == ErrorCode.ERROR)
                        {
                            return returnedErrorCode;
                        }

                        using (SqlCommand cmd = sqlConnection.CreateCommand())
                        {
                            cmd.CommandText = $@"
                            UPDATE Slider_Questions
                            SET StartValue = @{QuestionColumn.StartValue}, EndValue = @{QuestionColumn.EndValue}, StartValueCaption = @{QuestionColumn.StartValueCaption}, EndValueCaption = @{QuestionColumn.EndValueCaption}
                            WHERE ID = @{QuestionColumn.ID}";

                            SqlParameter[] parameters = new SqlParameter[] {
                                new SqlParameter($"{QuestionColumn.ID}", sliderQuestion.ID),
                                new SqlParameter($"{QuestionColumn.StartValue}", sliderQuestion.StartValue),
                                new SqlParameter($"{QuestionColumn.EndValue}", sliderQuestion.EndValue),
                                new SqlParameter($"{QuestionColumn.StartValueCaption}", sliderQuestion.StartValueCaption),
                                new SqlParameter($"{QuestionColumn.EndValueCaption}", sliderQuestion.EndValueCaption),
                            };
                            cmd.Parameters.AddRange(parameters);

                            rowsAffected = cmd.ExecuteNonQuery();
                        }

                        if (returnedErrorCode == ErrorCode.SUCCESS && rowsAffected != 0)
                        {
                            transactionScope.Complete();
                            return ErrorCode.SUCCESS;
                        }

                        return ErrorCode.ERROR;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Something went wrong:\n" + ex.Message);
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
        public ErrorCode UpdateStarQuestion(StarQuestion starQuestion)
        {
            try
            {
                ErrorCode returnedErrorCode = ErrorCode.ERROR;
                int rowsAffected = 0;

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    using (SqlConnection sqlConnection = new SqlConnection())
                    {
                        sqlConnection.ConnectionString = sqlConnectionectionSetting.ConnectionString;
                        sqlConnection.Open();

                        returnedErrorCode = UpdateQuestion(transactionScope, sqlConnection, starQuestion);

                        if (returnedErrorCode == ErrorCode.SQL_VIOLATION || returnedErrorCode == ErrorCode.ERROR)
                        {
                            return returnedErrorCode;
                        }

                        using (SqlCommand cmd = sqlConnection.CreateCommand())
                        {
                            cmd.CommandText = $@"
                                UPDATE Star_Questions
                                SET NumberOfStars = @{QuestionColumn.NumberOfStars}
                                WHERE ID = @{QuestionColumn.ID}";

                            SqlParameter[] parameters = new SqlParameter[] {
                                new SqlParameter($"{QuestionColumn.ID}", starQuestion.ID),
                                new SqlParameter($"{QuestionColumn.NumberOfStars}", starQuestion.NumberOfStars)
                            };
                            cmd.Parameters.AddRange(parameters);

                            rowsAffected = cmd.ExecuteNonQuery();
                        }

                        if (returnedErrorCode == ErrorCode.SUCCESS && rowsAffected != 0)
                        {
                            transactionScope.Complete();
                            return ErrorCode.SUCCESS;
                        }

                        return ErrorCode.ERROR;
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
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionectionSetting.ConnectionString))
                {
                    sqlConnection.Open();
                    using (SqlCommand cmd = sqlConnection.CreateCommand())
                    {
                        cmd.CommandText = $@"
                            delete from Questions where ID = @{QuestionColumn.ID}";

                        SqlParameter[] parameters = new SqlParameter[] {
                            new SqlParameter($"{QuestionColumn.ID}", questionId),
                        };
                        cmd.Parameters.AddRange(parameters);

                        return cmd.ExecuteNonQuery() > 0 ? Generic.ErrorCode.SUCCESS : Generic.ErrorCode.ERROR;

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

        #region GET Methods

        public ErrorCode GetAllQuestions(ref List<Question> questionsList)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionectionSetting.ConnectionString))
                {
                    sqlConnection.Open();
                    using (SqlCommand cmd = sqlConnection.CreateCommand())
                    {
                        cmd.CommandText = $@"
                            SELECT [ID], [Order], [Text], [Type] FROM Questions ORDER BY [ORDER] ASC;";

                        DataTable dataTable = new DataTable();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(dataTable);

                        foreach (DataRow row in dataTable.Rows)
                        {
                            Question q = new Question((int)row[$"{QuestionColumn.ID}"], (int)row[$"{QuestionColumn.Order}"], (string)row[$"{QuestionColumn.Text}"], (QuestionType)row[$"{QuestionColumn.Type}"]);
                            questionsList.Add(q);
                        }
                        return questionsList.Count >= 0 ? Generic.ErrorCode.SUCCESS : Generic.ErrorCode.ERROR; // RETURN INT32
                    }
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
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionectionSetting.ConnectionString))
                {
                    sqlConnection.Open();
                    using (SqlCommand cmd = sqlConnection.CreateCommand())
                    {
                        cmd.CommandText = $@"
                            select Q.[ID], Q.[Order], Q.[Text], Q.[Type], SmQ.NumberOfSmileyFaces
                            from Questions AS Q
                            inner join Smiley_Questions AS SmQ
                            on Q.ID = SmQ.ID
                            where Q.ID = @{QuestionColumn.ID}
                            ORDER BY [ORDER] ASC";
                        SqlParameter[] parameters = new SqlParameter[] {
                            new SqlParameter($"{QuestionColumn.ID}", smileyQuestion.ID),
                        };
                        cmd.Parameters.AddRange(parameters);

                        DataTable dataTable = new DataTable();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(dataTable);

                        foreach (DataRow row in dataTable.Rows)
                        {
                            smileyQuestion = new SmileyQuestion((int)row[$"{QuestionColumn.ID}"], (int)row[$"{QuestionColumn.Order}"], (string)row[$"{QuestionColumn.Text}"], (QuestionType)row[$"{QuestionColumn.Type}"], (int)row[$"{QuestionColumn.NumberOfSmileyFaces}"]);
                        }

                        return smileyQuestion.NumberOfSmileyFaces >= 0 ? Generic.ErrorCode.SUCCESS : Generic.ErrorCode.ERROR; // RETURN INT32
                    }
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
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionectionSetting.ConnectionString))
                {
                    sqlConnection.Open();
                    using (SqlCommand cmd = sqlConnection.CreateCommand())
                    {
                        cmd.CommandText = $@"
                            select Q.[ID], Q.[Order], Q.[Text], Q.[Type], SQ.StartValue, SQ.EndValue, SQ.StartValueCaption, SQ.EndValueCaption
                            from Questions AS Q
                            inner join Slider_Questions AS SQ
                            on Q.ID = SQ.ID
                            where Q.ID = @{QuestionColumn.ID}
                            ORDER BY [ORDER] ASC";

                        SqlParameter[] parameters = new SqlParameter[] {
                            new SqlParameter($"{QuestionColumn.ID}", sliderQuestion.ID),
                        };
                        cmd.Parameters.AddRange(parameters);

                        DataTable dataTable = new DataTable();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(dataTable);

                        foreach (DataRow row in dataTable.Rows)
                        {
                            sliderQuestion = new SliderQuestion((int)row[$"{QuestionColumn.ID}"], (int)row[$"{QuestionColumn.Order}"], (string)row[$"{QuestionColumn.Text}"], (QuestionType)row[$"{QuestionColumn.Type}"],
                                (int)row[$"{QuestionColumn.StartValue}"], (int)row[$"{QuestionColumn.EndValue}"], (string)row[$"{QuestionColumn.StartValueCaption}"], (string)row[$"{QuestionColumn.EndValueCaption}"]);
                        }

                        return sliderQuestion.StartValue >= 0 ? Generic.ErrorCode.SUCCESS : Generic.ErrorCode.ERROR; // RETURN INT32
                    }
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
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionectionSetting.ConnectionString))
                {
                    sqlConnection.Open();
                    using (SqlCommand cmd = sqlConnection.CreateCommand())
                    {
                        cmd.CommandText = $@"
                            select Q.[ID], Q.[Order], Q.[Text], Q.[Type], StQ.NumberOfStars
                            from Questions AS Q
                            inner join Star_Questions AS StQ
                            on Q.ID = StQ.ID
                            where Q.ID = @{QuestionColumn.ID}
                            ORDER BY [ORDER] ASC";

                        SqlParameter[] parameters = new SqlParameter[] {
                            new SqlParameter($"{QuestionColumn.ID}", starQuestion.ID),
                        };
                        cmd.Parameters.AddRange(parameters);

                        DataTable dataTable = new DataTable();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(dataTable);

                        foreach (DataRow row in dataTable.Rows)
                        {
                            starQuestion = new StarQuestion((int)row[$"{QuestionColumn.ID}"], (int)row[$"{QuestionColumn.Order}"], (string)row[$"{QuestionColumn.Text}"], (QuestionType)row[$"{QuestionColumn.Type}"], (int)row[$"{QuestionColumn.NumberOfStars}"]);
                        }

                        return starQuestion.NumberOfStars >= 0 ? Generic.ErrorCode.SUCCESS : Generic.ErrorCode.ERROR; // RETURN INT32
                    }
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
        //                sqlConnection = new SqlConnection(sqlConnectionectionSetting.ConnectionString);
        //                SqlCommand cmd = null;


        //                ///
        //                /// Create Tables if they do NOT exist
        //                ///
        //                cmd = new SqlCommand($@"
        //USE [{sqlConnectionectionSetting.Name}]
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
        //", sqlConnection);
        //                sqlConnection.Open();
        //                return cmd.ExecuteNonQuery() > 0 ?  CommonEnums.ErrorCode.SUCCESS :  CommonEnums.ErrorCode.ERROR;
        //                sqlConnection.Close();


        //                cmd = new SqlCommand($@"
        //USE [{sqlConnectionectionSetting.Name}]
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
        //", sqlConnection);
        //                sqlConnection.Open();
        //                return cmd.ExecuteNonQuery() > 0 ?  CommonEnums.ErrorCode.SUCCESS :  CommonEnums.ErrorCode.ERROR;
        //                sqlConnection.Close();


        //                cmd = new SqlCommand($@"
        //USE [{sqlConnectionectionSetting.Name}]
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
        //", sqlConnection);
        //                sqlConnection.Open();
        //                return cmd.ExecuteNonQuery() > 0 ?  CommonEnums.ErrorCode.SUCCESS :  CommonEnums.ErrorCode.ERROR;
        //                sqlConnection.Close();

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
        //                sqlConnection.Close();
        //            }
        //        } //end func.
    }
}
