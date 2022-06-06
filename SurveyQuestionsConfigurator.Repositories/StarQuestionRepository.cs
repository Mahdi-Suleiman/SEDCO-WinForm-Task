using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.CommonTypes;
using SurveyQuestionsConfigurator.DataAccess;
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
                dbConnect = new DbConnect();
                return dbConnect.AddStarQuestion(starQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return (int)Types.ErrorCode.ERROR;
            }
        }

        public int Get(int id, ref DataTable dataTable)
        {
            try
            {
                dbConnect = new DbConnect();
                return dbConnect.GetSingleStarQuestion(id, ref dataTable);
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
                dbConnect = new DbConnect();
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
