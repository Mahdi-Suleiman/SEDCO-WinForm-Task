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
using System.Transactions;
using static SurveyQuestionsConfigurator.Entities.Generic;
namespace SurveyQuestionsConfigurator.DataAccess
{
    public class DbConnect
    {
        ///get tSqlConnection string information from App.config
        private ConnectionStringSettings mSqlConnectionSettings;
        public DbConnect()
        {
            try
            {
                mSqlConnectionSettings = ConfigurationManager.ConnectionStrings[0];
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); /// write error to log file
            }
        }


        #region Common Methods

        /// <summary>
        /// Return SUCCESS if order is not already in use
        /// </summary>
        /// <param name="pSqlConnection"></param>
        /// <param name="pOrder"></param>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.SQL_VIOLATION
        /// </returns>
        private ErrorCode CheckIfOrderExist(SqlConnection pSqlConnection, int pOrder)
        {
            try
            {
                using (SqlCommand cmd = pSqlConnection.CreateCommand())
                {
                    cmd.CommandText = "dbo.CheckIfOrderExist";
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] parameters = new SqlParameter[] {
                                new SqlParameter($"{QuestionColumn.ReturnValue}", SqlDbType.Int),
                                new SqlParameter($"{QuestionColumn.Order}", pOrder),
                            };
                    parameters[0].Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.AddRange(parameters);

                    cmd.ExecuteNonQuery();
                    return (ErrorCode)parameters[0].Value;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); /// write error to log file
                return ErrorCode.ERROR;
            }
        }

        /// <summary>
        /// Return the ID of question based on it's order
        /// </summary>
        /// <param name="pSqlConnection"></param>
        /// <param name="pOrder"></param>
        /// <returns>
        /// ID if found
        /// 0 if no row is found
        /// </returns>
        private ErrorCode GetIDFromOrder(SqlConnection pSqlConnection, int pOrder, ref int pQuestionID)
        {
            try
            {
                using (SqlCommand cmd = pSqlConnection.CreateCommand())
                {
                    cmd.CommandText = "dbo.GetIDFromOrder";
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] parameters = new SqlParameter[] {
                                new SqlParameter($"{QuestionColumn.ReturnValue}", SqlDbType.Int),
                                new SqlParameter($"{QuestionColumn.Order}", pOrder),
                            };
                    parameters[0].Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.AddRange(parameters);

                    cmd.ExecuteNonQuery();

                    pQuestionID = (int)parameters[0].Value;

                    if (pQuestionID == -1)
                        return ErrorCode.ERROR;

                    return ErrorCode.SUCCESS;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); /// write error to log file
                return ErrorCode.ERROR;
            }
        }

        /// <summary>
        /// Test connecting string
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// </returns>
        public ErrorCode CheckConnectivity(SqlConnectionStringBuilder pBuilder)
        {
            try
            {
                using (SqlConnection tSqlConnection = new SqlConnection())
                {
                    tSqlConnection.ConnectionString = pBuilder.ConnectionString;
                    tSqlConnection.Open();
                    if (tSqlConnection.State == ConnectionState.Open)
                    {
                        return ErrorCode.SUCCESS;
                    }
                    return ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); /// write error to log file
                return ErrorCode.ERROR;
            }
        }

        /// <summary>
        /// Save new conncection string to config file
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// </returns>
        public ErrorCode SaveConnectionString(SqlConnectionStringBuilder pBuilder)
        {
            try
            {
                string tProviderName = "System.Data.SqlClient";
                string tSectionName = "connectionStrings";

                Configuration tConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                tConfig.ConnectionStrings.ConnectionStrings[0].ConnectionString = pBuilder.ConnectionString;
                tConfig.ConnectionStrings.ConnectionStrings[0].ProviderName = tProviderName;
                tConfig.Save(ConfigurationSaveMode.Minimal);

                //mSqlConnectionSettings = tConfig.ConnectionStrings.ConnectionStrings[0];
                ConfigurationManager.RefreshSection(tSectionName);
                SetConnectionSettings();
                return ErrorCode.SUCCESS;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); /// write error to log file
                return ErrorCode.ERROR;
            }
        }

        /// <summary>
        /// Get used connection string from config file
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// </returns>
        public SqlConnectionStringBuilder GetConnectionString()
        {
            try
            {
                SqlConnectionStringBuilder tbuilder = new SqlConnectionStringBuilder();
                tbuilder.ConnectionString = mSqlConnectionSettings.ConnectionString;
                return tbuilder;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); /// write error to log file
                return null;
            }
        }

        /// <summary>
        /// Sets mSqlConnectionSettings to newest data
        /// </summary>
        private void SetConnectionSettings()
        {
            try
            {
                mSqlConnectionSettings = ConfigurationManager.ConnectionStrings[0];
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); /// write error to log file
            }
        }

        #endregion

        #region INSERT Methods

        /// <summary>
        /// 1) Open a transaction
        /// 2) Check if desired order is already in use before inserting (SQL Function)
        /// 3) Perform insert stored procedure -> get the result as ErrorCode
        /// 4) return return from function with the corresponding ErrorCode
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.SQL_VIOLATION
        /// </returns>
        public ErrorCode InsertSmileyQuestion(SmileyQuestion pSmileyQuestion)
        {
            /// Try to insert a new smiley question into "Smiley_Questions" table
            try
            {
                /// ErrorCode to be returned
                ErrorCode tIsOrderTaken = ErrorCode.ERROR;

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    using (SqlConnection sqlConnection = new SqlConnection())
                    {
                        sqlConnection.ConnectionString = mSqlConnectionSettings.ConnectionString;
                        sqlConnection.Open();

                        /// Check if order is already in use
                        tIsOrderTaken = CheckIfOrderExist(sqlConnection, pSmileyQuestion.Order);

                        /// return if order already exist
                        if (tIsOrderTaken == ErrorCode.SQL_VIOLATION)
                            return ErrorCode.SQL_VIOLATION;

                        /// if order is not in use -> insert a question with the same order 
                        using (SqlCommand cmd = sqlConnection.CreateCommand())
                        {
                            cmd.CommandText = "dbo.INSERT_SMILEY_QUESTION";
                            cmd.CommandType = CommandType.StoredProcedure;

                            SqlParameter[] parameters = new SqlParameter[] {
                                new SqlParameter($"{QuestionColumn.Order}", pSmileyQuestion.Order),
                                new SqlParameter($"{QuestionColumn.Text}", pSmileyQuestion.Text),
                                new SqlParameter($"{QuestionColumn.Type}", pSmileyQuestion.Type),
                                new SqlParameter($"{QuestionColumn.NumberOfSmileyFaces}", pSmileyQuestion.NumberOfSmileyFaces)
                            };
                            cmd.Parameters.AddRange(parameters);
                            tIsOrderTaken = (ErrorCode)cmd.ExecuteScalar();
                        }

                        if (tIsOrderTaken == ErrorCode.SUCCESS)
                        {
                            /// If everything is okay -> COMMIT Transaction
                            transactionScope.Complete();
                            return ErrorCode.SUCCESS;
                        }

                        return ErrorCode.ERROR;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); /// write error to log file
                return Generic.ErrorCode.ERROR;
            }
        } /// Function end

        /// <summary>
        /// 1) Open a transaction
        /// 2) Check if desired order is already in use before inserting (SQL Function)
        /// 3) Perform insert stored procedure -> get the result as ErrorCode
        /// 4) return return from function with the corresponding ErrorCode
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
                /// ErrorCode to be returned
                ErrorCode tIsOrderTaken = ErrorCode.ERROR;

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    using (SqlConnection sqlConnection = new SqlConnection())
                    {
                        sqlConnection.ConnectionString = mSqlConnectionSettings.ConnectionString;
                        sqlConnection.Open();

                        /// Check if order is already in use
                        tIsOrderTaken = CheckIfOrderExist(sqlConnection, pSliderQuestion.Order);

                        /// return if order already exist
                        if (tIsOrderTaken == ErrorCode.SQL_VIOLATION)
                            return ErrorCode.SQL_VIOLATION;

                        /// if order is not in use -> insert a question with the same order 
                        using (SqlCommand cmd = sqlConnection.CreateCommand())
                        {
                            cmd.CommandText = $@"dbo.INSERT_SLIDER_QUESTION";
                            cmd.CommandType = CommandType.StoredProcedure;

                            SqlParameter[] parameters = new SqlParameter[] {
                                new SqlParameter($"{QuestionColumn.Text}", pSliderQuestion.Text),
                                new SqlParameter($"{QuestionColumn.Order}", pSliderQuestion.Order),
                                new SqlParameter($"{QuestionColumn.Type}", pSliderQuestion.Type),
                                new SqlParameter($"{QuestionColumn.StartValue}", pSliderQuestion.StartValue),
                                new SqlParameter($"{QuestionColumn.EndValue}", pSliderQuestion.EndValue),
                                new SqlParameter($"{QuestionColumn.StartValueCaption}", pSliderQuestion.StartValueCaption),
                                new SqlParameter($"{QuestionColumn.EndValueCaption}", pSliderQuestion.EndValueCaption)
                            };
                            cmd.Parameters.AddRange(parameters);

                            tIsOrderTaken = (ErrorCode)cmd.ExecuteScalar();
                        }

                        if (tIsOrderTaken == ErrorCode.SUCCESS)
                        {
                            /// If everything is okay -> COMMIT Transaction
                            transactionScope.Complete();
                            return ErrorCode.SUCCESS;
                        }

                        return ErrorCode.ERROR;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); /// write error to log file
                return Generic.ErrorCode.ERROR;
            }
        } /// Function end

        /// <summary>
        /// 1) Open a transaction
        /// 2) Check if desired order is already in use before inserting (SQL Function)
        /// 3) Perform insert stored procedure -> get the result as ErrorCode
        /// 4) return return from function with the corresponding ErrorCode
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
                /// ErrorCode to be returned
                ErrorCode tIsOrderTaken = ErrorCode.ERROR;

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    using (SqlConnection sqlConnection = new SqlConnection())
                    {
                        sqlConnection.ConnectionString = mSqlConnectionSettings.ConnectionString;
                        sqlConnection.Open();

                        /// Check if order is already in use
                        tIsOrderTaken = CheckIfOrderExist(sqlConnection, pStarQuestion.Order);

                        /// return if order already exist
                        if (tIsOrderTaken == ErrorCode.SQL_VIOLATION)
                            return ErrorCode.SQL_VIOLATION;


                        /// if order is not in use -> insert a question with the same order 
                        using (SqlCommand cmd = sqlConnection.CreateCommand())
                        {
                            cmd.CommandText = "dbo.INSERT_STAR_QUESTION";
                            cmd.CommandType = CommandType.StoredProcedure;

                            SqlParameter[] parameters = new SqlParameter[] {
                                new SqlParameter($"{QuestionColumn.Order}", pStarQuestion.Order),
                                new SqlParameter($"{QuestionColumn.Text}", pStarQuestion.Text),
                                new SqlParameter($"{QuestionColumn.Type}", pStarQuestion.Type),
                                new SqlParameter($"{QuestionColumn.NumberOfStars}", pStarQuestion.NumberOfStars)

                            };
                            cmd.Parameters.AddRange(parameters);
                            tIsOrderTaken = (ErrorCode)cmd.ExecuteScalar();
                        }

                        if (tIsOrderTaken == ErrorCode.SUCCESS)
                        {
                            /// If everything is okay -> COMMIT Transaction
                            transactionScope.Complete();
                            return ErrorCode.SUCCESS;
                        }

                        return ErrorCode.ERROR;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); /// write error to log file
                return Generic.ErrorCode.ERROR;
            }
        }/// Function end

        #endregion

        #region UPDATE Methods

        /// <summary>
        /// 1) Open a transaction
        /// 2) Check if desired order is already in use before inserting (SQL Function)
        /// 3) Get ID of the desired order you want to insert
        /// 4) if order already in use and the ID of that order is not the same ID the question that I am already updating
        /// 5) -> Perform update stored procedure -> get the result as ErrorCode
        /// 6) return from function with the corresponding ErrorCode
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.SQL_VIOLATION
        /// </returns>
        public ErrorCode UpdateSmileyQuestion(SmileyQuestion pSmileyQuestion)
        {
            /// Try to Update a new pQuestion into "Smiley_Questions" table
            try
            {
                ErrorCode tIsOrderTaken = ErrorCode.ERROR;
                ErrorCode tIsIDTaken = ErrorCode.ERROR;
                int tReturnedID = -1;

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    using (SqlConnection sqlConnection = new SqlConnection())
                    {
                        sqlConnection.ConnectionString = mSqlConnectionSettings.ConnectionString;
                        sqlConnection.Open();

                        /// Check if order is already in use
                        tIsOrderTaken = CheckIfOrderExist(sqlConnection, pSmileyQuestion.Order);

                        /// get question ID form it's unique order
                        tIsIDTaken = GetIDFromOrder(sqlConnection, pSmileyQuestion.Order, ref tReturnedID);

                        /// return if order already exist && the order is taken by another questionID
                        /// if ediitng the same question -> order is already taken BUT the same question is being edited which is OKAY
                        if (tIsOrderTaken == ErrorCode.SQL_VIOLATION && tIsIDTaken == ErrorCode.SUCCESS)
                            return ErrorCode.SQL_VIOLATION;

                        using (SqlCommand cmd = sqlConnection.CreateCommand())
                        {
                            cmd.CommandText = "dbo.UPDATE_SMILEY_QUESTION";
                            cmd.CommandType = CommandType.StoredProcedure;

                            SqlParameter[] parameters = new SqlParameter[] {
                                new SqlParameter($"{QuestionColumn.ID}", pSmileyQuestion.ID),
                                new SqlParameter($"{QuestionColumn.Order}", pSmileyQuestion.Order),
                                new SqlParameter($"{QuestionColumn.Text}", pSmileyQuestion.Text),
                                new SqlParameter($"{QuestionColumn.Type}", pSmileyQuestion.Type),
                                new SqlParameter($"{QuestionColumn.NumberOfSmileyFaces}", pSmileyQuestion.NumberOfSmileyFaces)
                            };
                            cmd.Parameters.AddRange(parameters);
                            tIsOrderTaken = (ErrorCode)cmd.ExecuteScalar();
                        }

                        if (tIsOrderTaken == ErrorCode.SUCCESS)
                        {
                            /// If everything is okay -> COMMIT Transaction
                            transactionScope.Complete();
                            return ErrorCode.SUCCESS;
                        }
                        return ErrorCode.ERROR;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); /// write error to log file
                return Generic.ErrorCode.ERROR;
            }

        }/// Function end

        /// <summary>
        /// 1) Open a transaction
        /// 2) Check if desired order is already in use before inserting (SQL Function)
        /// 3) Get ID of the desired order you want to insert
        /// 4) if order already in use and the ID of that order is not the same ID the question that I am already updating
        /// 5) -> Perform update stored procedure -> get the result as ErrorCode
        /// 6) return from function with the corresponding ErrorCode
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
                ErrorCode tIsOrderTaken = ErrorCode.ERROR;
                ErrorCode tIsIDTaken = ErrorCode.ERROR;
                int tReturnedID = -1;

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    using (SqlConnection sqlConnection = new SqlConnection())
                    {
                        sqlConnection.ConnectionString = mSqlConnectionSettings.ConnectionString;
                        sqlConnection.Open();

                        /// Check if order is already in use
                        tIsOrderTaken = CheckIfOrderExist(sqlConnection, pSliderQuestion.Order);

                        /// get question ID form it's unique order
                        tIsIDTaken = GetIDFromOrder(sqlConnection, pSliderQuestion.Order, ref tReturnedID);

                        /// return if order already exist && the order is taken by another questionID
                        /// if ediitng the same question -> order is already taken BUT the same question is being edited which is OKAY
                        if (tIsOrderTaken == ErrorCode.SQL_VIOLATION && tIsIDTaken == ErrorCode.SUCCESS)
                            return ErrorCode.SQL_VIOLATION;

                        using (SqlCommand cmd = sqlConnection.CreateCommand())
                        {
                            cmd.CommandText = "dbo.UPDATE_SLIDER_QUESTION";
                            cmd.CommandType = CommandType.StoredProcedure;

                            SqlParameter[] parameters = new SqlParameter[] {
                                new SqlParameter($"{QuestionColumn.ID}", pSliderQuestion.ID),
                                new SqlParameter($"{QuestionColumn.Order}", pSliderQuestion.Order),
                                new SqlParameter($"{QuestionColumn.Text}", pSliderQuestion.Text),
                                new SqlParameter($"{QuestionColumn.Type}", pSliderQuestion.Type),
                                new SqlParameter($"{QuestionColumn.StartValue}", pSliderQuestion.StartValue),
                                new SqlParameter($"{QuestionColumn.EndValue}", pSliderQuestion.EndValue),
                                new SqlParameter($"{QuestionColumn.StartValueCaption}", pSliderQuestion.StartValueCaption),
                                new SqlParameter($"{QuestionColumn.EndValueCaption}", pSliderQuestion.EndValueCaption),
                            };
                            cmd.Parameters.AddRange(parameters);
                            tIsOrderTaken = (ErrorCode)cmd.ExecuteScalar();
                        }

                        if (tIsOrderTaken == ErrorCode.SUCCESS)
                        {
                            /// If everything is okay -> COMMIT Transaction
                            transactionScope.Complete();
                            return ErrorCode.SUCCESS;
                        }
                        return ErrorCode.ERROR;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); /// write error to log file
                return Generic.ErrorCode.ERROR;
            }
        } /// Function end

        /// <summary>
        /// 1) Open a transaction
        /// 2) Check if desired order is already in use before inserting (SQL Function)
        /// 3) Get ID of the desired order you want to insert
        /// 4) if order already in use and the ID of that order is not the same ID the question that I am already updating
        /// 5) -> Perform update stored procedure -> get the result as ErrorCode
        /// 6) return from function with the corresponding ErrorCode
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
                ErrorCode tIsOrderTaken = ErrorCode.ERROR;
                ErrorCode tIsIDTaken = ErrorCode.ERROR;
                int tReturnedID = -1;

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    using (SqlConnection sqlConnection = new SqlConnection())
                    {
                        sqlConnection.ConnectionString = mSqlConnectionSettings.ConnectionString;
                        sqlConnection.Open();

                        /// Check if order is already in use
                        tIsOrderTaken = CheckIfOrderExist(sqlConnection, pStarQuestion.Order);

                        /// get question ID form it's unique order
                        tIsIDTaken = GetIDFromOrder(sqlConnection, pStarQuestion.Order, ref tReturnedID);

                        /// return if order already exist && the order is taken by another questionID
                        /// if ediitng the same question -> order is already taken BUT the same question is being edited which is OKAY
                        if (tIsOrderTaken == ErrorCode.SQL_VIOLATION && tIsIDTaken == ErrorCode.SUCCESS)
                            return ErrorCode.SQL_VIOLATION;

                        using (SqlCommand cmd = sqlConnection.CreateCommand())
                        {
                            cmd.CommandText = "dbo.UPDATE_STAR_QUESTION";
                            cmd.CommandType = CommandType.StoredProcedure;

                            SqlParameter[] parameters = new SqlParameter[] {
                                new SqlParameter($"{QuestionColumn.ID}", pStarQuestion.ID),
                                new SqlParameter($"{QuestionColumn.Order}", pStarQuestion.Order),
                                new SqlParameter($"{QuestionColumn.Text}", pStarQuestion.Text),
                                new SqlParameter($"{QuestionColumn.Type}", pStarQuestion.Type),
                                new SqlParameter($"{QuestionColumn.NumberOfStars}", pStarQuestion.NumberOfStars)
                            };
                            cmd.Parameters.AddRange(parameters);
                            tIsOrderTaken = (ErrorCode)cmd.ExecuteScalar();
                        }

                        if (tIsOrderTaken == ErrorCode.SUCCESS)
                        {
                            /// If everything is okay -> COMMIT Transaction
                            transactionScope.Complete();
                            return ErrorCode.SUCCESS;
                        }
                        return ErrorCode.ERROR;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); /// write error to log file
                return Generic.ErrorCode.ERROR;
            }
        } /// Function end

        #endregion

        #region DELETE Methods

        /// <summary>
        /// 1) Open an SQL connection
        /// 2) Perform a delete query based on passed ID
        /// 3) return error code based on affected rows number
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// </returns>
        public ErrorCode DeleteQuestionByID(int pQuestionId)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(mSqlConnectionSettings.ConnectionString))
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
                Logger.LogError(ex); /// write error to log file
                return Generic.ErrorCode.ERROR;
            }

        } /// Function end

        #endregion

        #region GET Methods

        /// <summary>
        /// 1) Open an SQL connection
        /// 2) Get all questions from DB
        /// 3) Create corresponding question objects and fill the passed question list with them
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// </returns>
        public ErrorCode GetAllQuestions(ref List<Question> pQuestionsList)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection())
                {
                    SetConnectionSettings();
                    sqlConnection.ConnectionString = mSqlConnectionSettings.ConnectionString;
                    sqlConnection.Open();

                    using (SqlCommand cmd = sqlConnection.CreateCommand())
                    {
                        cmd.CommandText = $@"
                            SELECT [ID], [Order], [Text], [Type] FROM Questions ORDER BY [ORDER] ASC;";

                        DataTable dataTable = new DataTable();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(dataTable);

                        int tID, tOrder;
                        string tText;
                        QuestionType tType;
                        foreach (DataRow row in dataTable.Rows)
                        {
                            tID = (int)row[$"{QuestionColumn.ID}"];
                            tOrder = (int)row[$"{QuestionColumn.Order}"];
                            tText = (string)row[$"{QuestionColumn.Text}"];
                            tType = (QuestionType)row[$"{QuestionColumn.Type}"];

                            Question q = new Question(tID, tOrder, tText, tType);
                            pQuestionsList.Add(q);
                        }
                        return pQuestionsList.Count >= 0 ? Generic.ErrorCode.SUCCESS : Generic.ErrorCode.ERROR; /// RETURN INT32
                    }
                }
            }
            catch (SqlException ex)
            {
                Logger.LogError(ex); /// write error to log file
                return Generic.ErrorCode.SQL_VIOLATION;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); /// write error to log file
                return Generic.ErrorCode.ERROR;
            }
        } /// Function end

        /// <summary>
        /// 1) Open an SQL connection
        /// 2) Get corresponding question details from DB
        /// 3) Fill the passed question object properties
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// </returns>
        public ErrorCode GetSmileyQuestionByID(ref SmileyQuestion pSmileyQuestion)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection())
                {
                    sqlConnection.ConnectionString = mSqlConnectionSettings.ConnectionString;
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

                        int tID, tOrder, tNumberOfSmileyFaces;
                        string tText;
                        QuestionType tType;
                        foreach (DataRow row in dataTable.Rows)
                        {
                            tID = (int)row[$"{QuestionColumn.ID}"];
                            tOrder = (int)row[$"{QuestionColumn.Order}"];
                            tText = (string)row[$"{QuestionColumn.Text}"];
                            tType = (QuestionType)row[$"{QuestionColumn.Type}"];
                            tNumberOfSmileyFaces = (int)row[$"{QuestionColumn.NumberOfSmileyFaces}"];
                            pSmileyQuestion = new SmileyQuestion(tID, tOrder, tText, tType, tNumberOfSmileyFaces);
                        }

                        return pSmileyQuestion.NumberOfSmileyFaces >= 0 ? Generic.ErrorCode.SUCCESS : Generic.ErrorCode.ERROR; /// RETURN INT32
                    }
                }
            }
            catch (SqlException ex)
            {
                Logger.LogError(ex); /// write error to log file
                return Generic.ErrorCode.SQL_VIOLATION;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); /// write error to log file
                return Generic.ErrorCode.ERROR;
            }
        } /// Function end

        /// <summary>
        /// 1) Open an SQL connection
        /// 2) Get corresponding question details from DB
        /// 3) Fill the passed question object properties
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// </returns>
        public ErrorCode GetSliderQuestionByID(ref SliderQuestion pSliderQuestion)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection())
                {
                    sqlConnection.ConnectionString = mSqlConnectionSettings.ConnectionString;
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


                        int tID, tOrder, tStartValue, tEndValue;
                        string tText, tStartValueCaption, tEndValueCaption;
                        QuestionType tType;
                        foreach (DataRow row in dataTable.Rows)
                        {
                            tID = (int)row[$"{QuestionColumn.ID}"];
                            tOrder = (int)row[$"{QuestionColumn.Order}"];
                            tText = (string)row[$"{QuestionColumn.Text}"];
                            tType = (QuestionType)row[$"{QuestionColumn.Type}"];
                            tStartValue = (int)row[$"{QuestionColumn.StartValue}"];
                            tEndValue = (int)row[$"{QuestionColumn.EndValue}"];
                            tStartValueCaption = (string)row[$"{QuestionColumn.StartValueCaption}"];
                            tEndValueCaption = (string)row[$"{QuestionColumn.EndValueCaption}"];

                            pSliderQuestion = new SliderQuestion(tID, tOrder, tText, tType,
                                tStartValue, tEndValue, tStartValueCaption, tEndValueCaption);
                        }

                        return pSliderQuestion.StartValue >= 0 ? Generic.ErrorCode.SUCCESS : Generic.ErrorCode.ERROR; /// RETURN INT32
                    }
                }
            }
            catch (SqlException ex)
            {
                Logger.LogError(ex); /// write error to log file
                return Generic.ErrorCode.SQL_VIOLATION;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); /// write error to log file
                return Generic.ErrorCode.ERROR;
            }
        } /// Function end

        /// <summary>
        /// 1) Open an SQL connection
        /// 2) Get corresponding question details from DB
        /// 3) Fill the passed question object properties
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// </returns>
        public ErrorCode GetStarQuestionByID(ref StarQuestion pStarQuestion)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection())
                {
                    sqlConnection.ConnectionString = mSqlConnectionSettings.ConnectionString;
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

                        int tID, tOrder, tNumberOfStars;
                        string tText;
                        QuestionType tType;

                        foreach (DataRow row in dataTable.Rows)
                        {
                            tID = (int)row[$"{QuestionColumn.ID}"];
                            tOrder = (int)row[$"{QuestionColumn.Order}"];
                            tText = (string)row[$"{QuestionColumn.Text}"];
                            tType = (QuestionType)row[$"{QuestionColumn.Type}"];
                            tNumberOfStars = (int)row[$"{QuestionColumn.NumberOfStars}"];

                            pStarQuestion = new StarQuestion(tID, tOrder, tText, tType, tNumberOfStars);
                        }

                        return pStarQuestion.NumberOfStars >= 0 ? Generic.ErrorCode.SUCCESS : Generic.ErrorCode.ERROR; /// RETURN INT32
                    }
                }
            }
            catch (SqlException ex)
            {
                Logger.LogError(ex); /// write error to log file
                return Generic.ErrorCode.SQL_VIOLATION;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); /// write error to log file
                return Generic.ErrorCode.ERROR;
            }

        } /// Function end

        #endregion

    }
}
