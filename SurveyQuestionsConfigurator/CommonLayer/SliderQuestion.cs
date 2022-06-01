using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyQuestionsConfigurator.CommonLayer
{
    internal class SliderQuestion : Question
    {
        public int StartValue { get; set; }
        public int EndValue { get; set; }
        public string StartValueCaption { get; set; }
        public string EndValueCaption { get; set; }
        public SliderQuestion(int id, int order, string text, int type, int startValue, int endValue, string startValueCaption, string endValueCaption) :
      base(id, order, text, type)
        {
            StartValue = startValue;
            EndValue = endValue;
            StartValueCaption = startValueCaption;
            EndValueCaption = endValueCaption;
        }
    }
}
