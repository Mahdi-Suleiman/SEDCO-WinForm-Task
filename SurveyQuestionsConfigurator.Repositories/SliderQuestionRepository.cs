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
        public ErrorCode Add(SliderQuestion sliderQuestion)
        {
            try
            {
                return dbConnect.AddSliderQuestion(sliderQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }

        public ErrorCode Get(ref SliderQuestion sliderQuestion)
        {
            try
            {
                return dbConnect.GetSliderQuestionByID(ref sliderQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }

        public ErrorCode Update(SliderQuestion sliderQuestion)
        {
            try
            {
                return dbConnect.EditSliderQuestion(sliderQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }
    }
}
