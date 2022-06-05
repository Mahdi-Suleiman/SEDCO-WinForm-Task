using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.CommonTypes;
using SurveyQuestionsConfigurator.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SurveyQuestionsConfigurator.Repositories
{
    public class SmileyQuestionRepository : GenericRepository
    {
        //private SmileyQuestion smileyQuestion = null;
        //public SmileyQuestionRepository(SmileyQuestion smileyQuestion)
        //{
        //    this.smileyQuestion = new SmileyQuestion(smileyQuestion);
        //}

        public int Add(SmileyQuestion smileyQuestion)
        {
            try
            {
                DbConnect dbConnect = new DbConnect();
                return dbConnect.AddSmileyQuestion(smileyQuestion);
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
                return dbConnect.GetSingleSmileyQuestion(id);
            }
            catch (Exception ex)
            {
                Helper.Logger(ex);
                return null;
            }
        }

        public int Update(SmileyQuestion smileyQuestion)
        {
            try
            {
                DbConnect dbConnect = new DbConnect();
                return dbConnect.EditSmileyQuestion(smileyQuestion);
            }
            catch (Exception ex)
            {
                Helper.Logger(ex);
                return (int)Types.Error.ERROR;
            }
        }
    }
}
