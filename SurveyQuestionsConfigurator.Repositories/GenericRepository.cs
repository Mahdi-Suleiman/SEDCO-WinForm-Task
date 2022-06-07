using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.DataAccess;
using SurveyQuestionsConfigurator.Entites;
using SurveyQuestionsConfigurator.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SurveyQuestionsConfigurator.Repositories
{
    public class GenericRepository
    {
        ///<>
        /// Add
        /// Update
        /// Get
        /// GetAll
        /// Find
        ///</>
        ///
        protected DbConnect dbConnect = new DbConnect();

        public int Delete(int id)
        {
            try
            {
                return dbConnect.DeleteQuestion(id);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return (int)Types.ErrorCode.ERROR;
            }
        }

        public int GetAll(ref List<Question> questionsList)
        {
            try
            {
                return dbConnect.GetAllQuestions(ref questionsList);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return (int)Types.ErrorCode.ERROR;
            }
        }
    }
}
