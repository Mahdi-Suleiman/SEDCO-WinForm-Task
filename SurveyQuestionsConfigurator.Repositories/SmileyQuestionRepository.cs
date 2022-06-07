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
    public class SmileyQuestionRepository : GenericRepository
    {
        //private SmileyQuestion smileyQuestion = null;
        //public SmileyQuestionRepository(SmileyQuestion smileyQuestion)
        //{
        //    this.smileyQuestion = new SmileyQuestion(smileyQuestion);
        //}

        public ErrorCode Add(SmileyQuestion smileyQuestion)
        {
            try
            {
                return dbConnect.AddSmileyQuestion(smileyQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }

        public ErrorCode Get(ref SmileyQuestion smileyQuestion)
        {
            try
            {
                return dbConnect.GetSmileyQuestionByID(ref smileyQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }

        public ErrorCode Update(SmileyQuestion smileyQuestion)
        {
            try
            {
                return dbConnect.EditSmileyQuestion(smileyQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }
    }
}
