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
        public ErrorCode Add(StarQuestion starQuestion)
        {
            try
            {
                return dbConnect.InsertStarQuestion(starQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }

        public ErrorCode Get(ref StarQuestion starQuestion)
        {
            try
            {
                return dbConnect.GetStarQuestionByID(ref starQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }

        public ErrorCode Update(StarQuestion starQuestion)
        {
            try
            {
                return dbConnect.EditStarQuestion(starQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }
    }
}
