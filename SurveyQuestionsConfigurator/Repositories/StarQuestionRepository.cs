using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.CommonTypes;
using SurveyQuestionsConfigurator.DataAccess;
using SurveyQuestionsConfigurator.Entities;
using SurveyQuestionsConfigurator.Interfaces;
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
                DbConnect dbConnect = new DbConnect();
                return dbConnect.AddStarQuestion(starQuestion);
            }
            catch (Exception ex)
            {
                Helper.Logger(ex);
                return (int)Types.Error.ERROR;
            }
        }

        public DataTable Get(int id)
        {
            try
            {
                DbConnect dbConnect = new DbConnect();
                return dbConnect.GetSingleStarQuestion(id);
            }
            catch (Exception ex)
            {
                Helper.Logger(ex);
                return null;
            }
        }

        public int Update(StarQuestion starQuestion)
        {
            try
            {
                DbConnect dbConnect = new DbConnect();
                return dbConnect.EditStarQuestion(starQuestion);
            }
            catch (Exception ex)
            {
                Helper.Logger(ex);
                return (int)Types.Error.ERROR;
            }
        }
    }
}
