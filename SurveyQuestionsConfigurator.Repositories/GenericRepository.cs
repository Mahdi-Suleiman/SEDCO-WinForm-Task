using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.DataAccess;
using SurveyQuestionsConfigurator.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static SurveyQuestionsConfigurator.Entities.Types;

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

        public ErrorCode Delete(int id)
        {
            try
            {
                return dbConnect.DeleteQuestion(id);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Types.ErrorCode.ERROR;
            }
        }

        public ErrorCode GetAll(ref List<Question> questionsList)
        {
            try
            {
                return dbConnect.GetAllQuestions(ref questionsList);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Types.ErrorCode.ERROR;
            }
        }
    }
}
