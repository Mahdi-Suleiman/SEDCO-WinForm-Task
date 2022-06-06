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
                DbConnect dbConnect = new DbConnect();
                return dbConnect.AddSliderQuestion(sliderQuestion);
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
                return dbConnect.GetSingleSliderQuestion(id);
            }
            catch (Exception ex)
            {
                Helper.Logger(ex);
                return null;
            }
        }

        public int Update(SliderQuestion sliderQuestion)
        {
            try
            {
                DbConnect dbConnect = new DbConnect();
                return dbConnect.EditSliderQuestion(sliderQuestion);
            }
            catch (Exception ex)
            {
                Helper.Logger(ex);
                return (int)Types.Error.ERROR;
            }
        }
    }
}
