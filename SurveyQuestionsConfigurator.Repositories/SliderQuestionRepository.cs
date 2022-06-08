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
    public class SliderQuestionRepository : GenericRepository
    {
        public ErrorCode Add(SliderQuestion pSliderQuestion)
        {
            try
            {
                return mDbConnect.InsertSliderQuestion(pSliderQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }

        public ErrorCode Get(ref SliderQuestion pSliderQuestion)
        {
            try
            {
                return mDbConnect.GetSliderQuestionByID(ref pSliderQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }

        public ErrorCode Update(SliderQuestion pSliderQuestion)
        {
            try
            {
                return mDbConnect.UpdateSliderQuestion(pSliderQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }
    }
}
