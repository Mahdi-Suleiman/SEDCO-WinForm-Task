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
    public class SliderQuestionRepository : GenericRepository
    {
        public int Add(SliderQuestion sliderQuestion)
        {
            try
            {
                dbConnect = new DbConnect();
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
                dbConnect = new DbConnect();
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
                dbConnect = new DbConnect();
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
