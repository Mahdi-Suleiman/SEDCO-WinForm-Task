using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyQuestionsConfigurator.CommonLayer
{
    internal class StarQuestion : Question
    {
        public int NumberOfStars { get; set; }

        public StarQuestion(int id, int order, string text, int type, int numberOfStars) :
            base(id, order, text, type)
        {
            NumberOfStars = numberOfStars;
        }
    }
}
