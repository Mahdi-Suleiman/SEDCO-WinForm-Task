using SurveyQuestionsConfigurator.CommonLayer;
using SurveyQuestionsConfigurator.DataAccessLayer;
using SurveyQuestionsConfigurator.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyQuestionsConfigurator.Repository
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
                CommonHelpers.Logger(ex);
                return (int)CommonEnums.ErrorType.ERROR;
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
                CommonHelpers.Logger(ex);
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
                CommonHelpers.Logger(ex);
                return (int)CommonEnums.ErrorType.ERROR;
            }
        }
    }
}
