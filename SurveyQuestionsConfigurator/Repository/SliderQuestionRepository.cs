using SurveyQuestionsConfigurator.CommonLayer;
using SurveyQuestionsConfigurator.DataAccessLayer;
using SurveyQuestionsConfigurator.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyQuestionsConfigurator.Repository
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
                CommonHelpers.Logger(ex);
                return (int)CommonEnums.ErrorType.ERROR;
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
                CommonHelpers.Logger(ex);
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
                CommonHelpers.Logger(ex);
                return (int)CommonEnums.ErrorType.ERROR;
            }
        }
    }
}
