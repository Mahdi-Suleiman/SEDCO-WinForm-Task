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
        /// General pQuestion insert method that calls a stored proceedure
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.SQL_VIOLATION
        /// </returns>
        private ErrorCode InsertQuestion(TransactionScope pTransactionScope, SqlConnection pSqlConnection, Question pQuestion)
        {
            try
            {
                ErrorCode returnedErrorCode = ErrorCode.ERROR;
                using (SqlCommand cmd = pSqlConnection.CreateCommand())
                {
                    cmd.CommandText = $@"
                            DECLARE @RESULT INT
                            EXECUTE @RESULT = INSERT_QUESTION @Order = @{QuestionColumn.Order}, @Text = @{QuestionColumn.Text}, @Type = @{QuestionColumn.Type}
                            SELECT @RESULT";

                    SqlParameter[] parameters = new SqlParameter[] {
                                new SqlParameter($"{QuestionColumn.Order}", pQuestion.Order),
                                new SqlParameter($"{QuestionColumn.Text}", pQuestion.Text),
                                new SqlParameter($"{QuestionColumn.Type}", pQuestion.Type)};
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
        /// General pQuestion update method that calls a stored proceedure
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.SQL_VIOLATION
        /// </returns>
        private ErrorCode UpdateQuestion(TransactionScope pTransactionScope, SqlConnection pSqlConnection, Question pQuestion)
        {
            try
            {
                ErrorCode returnedErrorCode = ErrorCode.ERROR;
                using (SqlCommand cmd = pSqlConnection.CreateCommand())
                {
                    cmd.CommandText = $@"
                            DECLARE @RESULT INT
                            EXECUTE @RESULT = UPDATE_QUESTION @ID = @{QuestionColumn.ID}, @ORDER =  @{QuestionColumn.Order}, @Text = @{QuestionColumn.Text}
                            SELECT @RESULT";

                    SqlParameter[] parameters = new SqlParameter[] {
                                new SqlParameter($"{QuestionColumn.Order}", pQuestion.Order),
                                new SqlParameter($"{QuestionColumn.Text}", pQuestion.Text),
                                new SqlParameter($"{QuestionColumn.ID}", pQuestion.ID)};
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
        public ErrorCode InsertSmileyQuestion(SmileyQuestion pSmileyQuestion)
        {
            ///
            /// Try to insert a new pQuestion into "Smiley_Questions" table
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

                        returnedErrorCode = InsertQuestion(transactionScope, sqlConnection, pSmileyQuestion);

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
                                new SqlParameter($"{QuestionColumn.NumberOfSmileyFaces}", pSmileyQuestion.NumberOfSmileyFaces)};
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
        public ErrorCode InsertSliderQuestion(SliderQuestion pSliderQuestion)
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

                        returnedErrorCode = InsertQuestion(transactionScope, sqlConnection, pSliderQuestion);

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
                                new SqlParameter($"{QuestionColumn.StartValue}", pSliderQuestion.StartValue),
                                new SqlParameter($"{QuestionColumn.EndValue}", pSliderQuestion.EndValue),
                                new SqlParameter($"{QuestionColumn.StartValueCaption}", pSliderQuestion.StartValueCaption),
                                new SqlParameter($"{QuestionColumn.EndValueCaption}", pSliderQuestion.EndValueCaption)};
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
        public ErrorCode InsertStarQuestion(StarQuestion pStarQuestion)
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

                        returnedErrorCode = InsertQuestion(transactionScope, sqlConnection, pStarQuestion);

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
                                 new SqlParameter($"{QuestionColumn.NumberOfStars}", pStarQuestion.NumberOfStars)
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



        }/// Function end

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
        public ErrorCode UpdateSmileyQuestion(SmileyQuestion pSmileyQuestion)
        {
            ///
            /// Try to Update a new pQuestion into "Smiley_Questions" table
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

                        returnedErrorCode = UpdateQuestion(transactionScope, sqlConnection, pSmileyQuestion);

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
                                new SqlParameter($"{QuestionColumn.ID}", pSmileyQuestion.ID),
                                new SqlParameter($"{QuestionColumn.NumberOfSmileyFaces}", pSmileyQuestion.NumberOfSmileyFaces)
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

        }/// Function end

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
        public ErrorCode UpdateSliderQuestion(SliderQuestion pSliderQuestion)
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

                        returnedErrorCode = UpdateQuestion(transactionScope, sqlConnection, pSliderQuestion);

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
                                new SqlParameter($"{QuestionColumn.ID}", pSliderQuestion.ID),
                                new SqlParameter($"{QuestionColumn.StartValue}", pSliderQuestion.StartValue),
                                new SqlParameter($"{QuestionColumn.EndValue}", pSliderQuestion.EndValue),
                                new SqlParameter($"{QuestionColumn.StartValueCaption}", pSliderQuestion.StartValueCaption),
                                new SqlParameter($"{QuestionColumn.EndValueCaption}", pSliderQuestion.EndValueCaption),
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
        public ErrorCode UpdateStarQuestion(StarQuestion pStarQuestion)
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

                        returnedErrorCode = UpdateQuestion(transactionScope, sqlConnection, pStarQuestion);

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
                                new SqlParameter($"{QuestionColumn.ID}", pStarQuestion.ID),
                                new SqlParameter($"{QuestionColumn.NumberOfStars}", pStarQuestion.NumberOfStars)
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
        public ErrorCode DeleteQuestion(int pQuestionId)
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
                            new SqlParameter($"{QuestionColumn.ID}", pQuestionId),
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
        public ErrorCode GetAllQuestions(ref List<Question> pQuestionsList)
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
                            pQuestionsList.Add(q);
                        }
                        return pQuestionsList.Count >= 0 ? Generic.ErrorCode.SUCCESS : Generic.ErrorCode.ERROR; // RETURN INT32
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
        public ErrorCode GetSmileyQuestionByID(ref SmileyQuestion pSmileyQuestion)
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
                            new SqlParameter($"{QuestionColumn.ID}", pSmileyQuestion.ID),
                        };
                        cmd.Parameters.AddRange(parameters);

                        DataTable dataTable = new DataTable();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(dataTable);

                        foreach (DataRow row in dataTable.Rows)
                        {
                            pSmileyQuestion = new SmileyQuestion((int)row[$"{QuestionColumn.ID}"], (int)row[$"{QuestionColumn.Order}"], (string)row[$"{QuestionColumn.Text}"], (QuestionType)row[$"{QuestionColumn.Type}"], (int)row[$"{QuestionColumn.NumberOfSmileyFaces}"]);
                        }

                        return pSmileyQuestion.NumberOfSmileyFaces >= 0 ? Generic.ErrorCode.SUCCESS : Generic.ErrorCode.ERROR; // RETURN INT32
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
        public ErrorCode GetSliderQuestionByID(ref SliderQuestion pSliderQuestion)
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
                            new SqlParameter($"{QuestionColumn.ID}", pSliderQuestion.ID),
                        };
                        cmd.Parameters.AddRange(parameters);

                        DataTable dataTable = new DataTable();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(dataTable);

                        foreach (DataRow row in dataTable.Rows)
                        {
                            pSliderQuestion = new SliderQuestion((int)row[$"{QuestionColumn.ID}"], (int)row[$"{QuestionColumn.Order}"], (string)row[$"{QuestionColumn.Text}"], (QuestionType)row[$"{QuestionColumn.Type}"],
                                (int)row[$"{QuestionColumn.StartValue}"], (int)row[$"{QuestionColumn.EndValue}"], (string)row[$"{QuestionColumn.StartValueCaption}"], (string)row[$"{QuestionColumn.EndValueCaption}"]);
                        }

                        return pSliderQuestion.StartValue >= 0 ? Generic.ErrorCode.SUCCESS : Generic.ErrorCode.ERROR; // RETURN INT32
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
        public ErrorCode GetStarQuestionByID(ref StarQuestion pStarQuestion)
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
                            new SqlParameter($"{QuestionColumn.ID}", pStarQuestion.ID),
                        };
                        cmd.Parameters.AddRange(parameters);

                        DataTable dataTable = new DataTable();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(dataTable);

                        foreach (DataRow row in dataTable.Rows)
                        {
                            pStarQuestion = new StarQuestion((int)row[$"{QuestionColumn.ID}"], (int)row[$"{QuestionColumn.Order}"], (string)row[$"{QuestionColumn.Text}"], (QuestionType)row[$"{QuestionColumn.Type}"], (int)row[$"{QuestionColumn.NumberOfStars}"]);
                        }

                        return pStarQuestion.NumberOfStars >= 0 ? Generic.ErrorCode.SUCCESS : Generic.ErrorCode.ERROR; // RETURN INT32
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
