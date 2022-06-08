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
        //private SmileyQuestion pSmileyQuestion = null;
        //public SmileyQuestionRepository(SmileyQuestion pSmileyQuestion)
        //{
        //    this.pSmileyQuestion = new SmileyQuestion(pSmileyQuestion);
        //}

        public ErrorCode Add(SmileyQuestion pSmileyQuestion)
        {
            try
            {
                return mDbConnect.InsertSmileyQuestion(pSmileyQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }

        public ErrorCode Get(ref SmileyQuestion pSmileyQuestion)
        {
            try
            {
                return mDbConnect.GetSmileyQuestionByID(ref pSmileyQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }

        public ErrorCode Update(SmileyQuestion pSmileyQuestion)
        {
            try
            {
                return mDbConnect.UpdateSmileyQuestion(pSmileyQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }
    }
}
