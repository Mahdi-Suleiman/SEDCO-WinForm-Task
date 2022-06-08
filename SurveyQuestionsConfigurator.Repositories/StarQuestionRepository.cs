using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SurveyQuestionsConfigurator.Entities.Generic;

namespace SurveyQuestionsConfigurator.Repositories
{
    public class StarQuestionRepository : GenericRepository
    {
        public ErrorCode Add(StarQuestion pStarQuestion)
        {
            try
            {
                return mDbConnect.InsertStarQuestion(pStarQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }

        public ErrorCode Get(ref StarQuestion pStarQuestion)
        {
            try
            {
                return mDbConnect.GetStarQuestionByID(ref pStarQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }

        public ErrorCode Update(StarQuestion pStarQuestion)
        {
            try
            {
                return mDbConnect.UpdateStarQuestion(pStarQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }
    }
}
