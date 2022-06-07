using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.DataAccess;
using SurveyQuestionsConfigurator.Entites;
using SurveyQuestionsConfigurator.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyQuestionsConfigurator.Repositories
{
    public class SliderQuestionRepository : GenericRepository
    {
        public int Add(SliderQuestion sliderQuestion)
        {
            try
            {
                return dbConnect.AddSliderQuestion(sliderQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return (int)Types.ErrorCode.ERROR;
            }
        }

        public int Get(ref SliderQuestion sliderQuestion)
        {
            try
            {
                return dbConnect.GetSliderQuestionByID(ref sliderQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return (int)Types.ErrorCode.ERROR;
            }
        }

        public int Update(SliderQuestion sliderQuestion)
        {
            try
            {
                return dbConnect.EditSliderQuestion(sliderQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return (int)Types.ErrorCode.ERROR;
            }
        }
    }
}
