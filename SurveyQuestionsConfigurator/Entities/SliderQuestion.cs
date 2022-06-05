using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyQuestionsConfigurator.Entities
{
    public class SliderQuestion : Question
    {
        #region Attributes
        public int StartValue { get; set; }
        public int EndValue { get; set; }
        public string StartValueCaption { get; set; }
        public string EndValueCaption { get; set; }
        #endregion

        public SliderQuestion(int id, int order, string text, int type, int startValue, int endValue, string startValueCaption, string endValueCaption) :
      base(id, order, text, type)
        {
            StartValue = startValue;
            EndValue = endValue;
            StartValueCaption = startValueCaption;
            EndValueCaption = endValueCaption;
        }

        public SliderQuestion(int id) :

           base(id)
        { }

        public SliderQuestion(SliderQuestion sliderQuestion) :
          this(sliderQuestion.ID, sliderQuestion.Order, sliderQuestion.Text, sliderQuestion.Type, sliderQuestion.StartValue, sliderQuestion.EndValue, sliderQuestion.StartValueCaption, sliderQuestion.EndValueCaption)
        { }
    }
}
