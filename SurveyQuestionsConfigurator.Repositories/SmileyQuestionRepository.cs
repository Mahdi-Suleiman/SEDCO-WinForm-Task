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
                dbConnect = new DbConnect();
                return dbConnect.AddSmileyQuestion(smileyQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return (int)Types.ErrorCode.ERROR;
            }
        }

        public int Get(ref SmileyQuestion smileyQuestion)
        {
            try
            {
                dbConnect = new DbConnect();
                return dbConnect.GetSmileyQuestionByID(ref smileyQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return (int)Types.ErrorCode.ERROR;
            }
        }

        public int Update(SmileyQuestion smileyQuestion)
        {
            try
            {
                dbConnect = new DbConnect();
                return dbConnect.EditSmileyQuestion(smileyQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return (int)Types.ErrorCode.ERROR;
            }
        }
    }
}
