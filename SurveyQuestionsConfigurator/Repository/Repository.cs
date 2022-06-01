using SurveyQuestionsConfigurator.CommonLayer;
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
    public class Repository
    {
        ///<>
        /// Add
        /// Update
        /// Get
        /// GetAll
        /// Find
        ///</>
        ///

        Question question = null;
        public Repository(Question q)
        {
            question = new Question(q);
        }

        //public DataTable Add(int id)
        //{

        //}
    }
}
