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
        ///get sqlConnectionection string information from App.config
        private ConnectionStringSettings mSqlConnectionectionSetting = ConfigurationManager.ConnectionStrings[0];

        #region INSERT Methods
        /// <summary>
        /// General question insert method that calls a stored proceedure
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.SQL_VIOLATION
        /// </returns>
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

        /// <summary>
        /// General question update method that calls a stored proceedure
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.SQL_VIOLATION
        /// </returns>
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
        /// 1) Open transaction
        /// 2) Perform insert stored procedure -> get the result
        /// 3) uery a second insert and get rows affected number
        /// 4) return error code based on the results
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.SQL_VIOLATION
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
                        sqlConnection.ConnectionString = mSqlConnectionectionSetting.ConnectionString;
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
        } /// Function end

        /// <summary>
        /// 1) Open transaction
        /// 2) Perform insert stored procedure -> get the result
        /// 3) uery a second insert and get rows affected number
        /// 4) return error code based on the results
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.SQL_VIOLATION
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
                        sqlConnection.ConnectionString = mSqlConnectionectionSetting.ConnectionString;
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
        /// 1) Open transaction
        /// 2) Perform insert stored procedure -> get the result
        /// 3) uery a second insert and get rows affected number
        /// 4) return error code based on the results
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.SQL_VIOLATION
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
                        sqlConnection.ConnectionString = mSqlConnectionectionSetting.ConnectionString;
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
        /// 1) Open transaction
        /// 2) Perform update stored procedure -> get the result
        /// 3) uery a second insert and get rows affected number
        /// 4) return error code based on the results
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.SQL_VIOLATION
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
                        sqlConnection.ConnectionString = mSqlConnectionectionSetting.ConnectionString;
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
        /// 1) Open transaction
        /// 2) Perform update stored procedure -> get the result
        /// 3) uery a second insert and get rows affected number
        /// 4) return error code based on the results
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.SQL_VIOLATION
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
                        sqlConnection.ConnectionString = mSqlConnectionectionSetting.ConnectionString;
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
        } /// Function end

        /// <summary>
        /// 1) Open transaction
        /// 2) Perform update stored procedure -> get the result
        /// 3) uery a second insert and get rows affected number
        /// 4) return error code based on the results
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.SQL_VIOLATION
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
                        sqlConnection.ConnectionString = mSqlConnectionectionSetting.ConnectionString;
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
        } /// Function end

        #endregion

        #region DELETE Methods

        /// <summary>
        /// 1) Open transaction
        /// 2) Perform update stored procedure -> get the result
        /// 3) uery a second insert and get rows affected number
        /// 4) return error code based on the results
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// </returns>
        public ErrorCode DeleteQuestion(int questionId)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(mSqlConnectionectionSetting.ConnectionString))
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

        } /// Function end

        #endregion

        #region GET Methods

        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// </returns>
        public ErrorCode GetAllQuestions(ref List<Question> questionsList)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(mSqlConnectionectionSetting.ConnectionString))
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
        } /// Function end

        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// </returns>
        public ErrorCode GetSmileyQuestionByID(ref SmileyQuestion smileyQuestion)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(mSqlConnectionectionSetting.ConnectionString))
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
        } /// Function end

        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// </returns>
        public ErrorCode GetSliderQuestionByID(ref SliderQuestion sliderQuestion)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(mSqlConnectionectionSetting.ConnectionString))
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
        } /// Function end

          /// <returns>
          /// ErrorCode.SUCCESS
          /// ErrorCode.ERROR
          /// </returns>
        public ErrorCode GetStarQuestionByID(ref StarQuestion starQuestion)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(mSqlConnectionectionSetting.ConnectionString))
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

        } /// Function end

        #endregion

    }
}
