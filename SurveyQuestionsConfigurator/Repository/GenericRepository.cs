using SurveyQuestionsConfigurator.CommonLayer;
using SurveyQuestionsConfigurator.DataAccessLayer;
using SurveyQuestionsConfigurator.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
namespace SurveyQuestionsConfigurator.Repository
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

        public int Delete(int id)
        {
            try
            {
                DbConnect dbConnect = new DbConnect();
                return dbConnect.DeleteQuestion(id);
            }
            catch (Exception ex)
            {
                CommonHelpers.Logger(ex);
                return (int)CommonEnums.ErrorType.ERROR;
            }
        }

        public DataTable GetAll()
        {
            try
            {
                DbConnect dbConnect = new DbConnect();
                return dbConnect.GetAllQuestions();
            }
            catch (Exception ex)
            {
                CommonHelpers.Logger(ex);
                return null;
            }
        }
    }
}
