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
using static SurveyQuestionsConfigurator.Entities.Generic;

namespace SurveyQuestionsConfigurator.Repositories
{
    public class GenericRepository
    {

        protected DbConnect mDbConnect = new DbConnect();

        public ErrorCode Delete(int pID)
        {
            try
            {
                return mDbConnect.DeleteQuestionByID(pID);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }

        public ErrorCode GetAll(ref List<Question> pQuestionsList)
        {
            try
            {
                return mDbConnect.GetAllQuestions(ref pQuestionsList);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }
    }
}
