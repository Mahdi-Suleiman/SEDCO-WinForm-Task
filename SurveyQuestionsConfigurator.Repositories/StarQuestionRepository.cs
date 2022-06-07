using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.DataAccess;
using SurveyQuestionsConfigurator.Entites;
using SurveyQuestionsConfigurator.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyQuestionsConfigurator.Repositories
{
    public class StarQuestionRepository : GenericRepository
    {
        public int Add(StarQuestion starQuestion)
        {
            try
            {
                return dbConnect.AddStarQuestion(starQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return (int)Types.ErrorCode.ERROR;
            }
        }

        public int Get(ref StarQuestion starQuestion)
        {
            try
            {
                return dbConnect.GetStarQuestionByID(ref starQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return (int)Types.ErrorCode.ERROR;
            }
        }

        public int Update(StarQuestion starQuestion)
        {
            try
            {
                return dbConnect.EditStarQuestion(starQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return (int)Types.ErrorCode.ERROR;
            }
        }
    }
}
