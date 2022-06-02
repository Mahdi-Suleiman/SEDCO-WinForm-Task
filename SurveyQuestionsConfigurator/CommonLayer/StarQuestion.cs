using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyQuestionsConfigurator.CommonLayer
{
    public class StarQuestion : Question
    {
        public int NumberOfStars { get; set; }

        public StarQuestion(int id, int order, string text, int type, int numberOfStars) :
            base(id, order, text, type)
        {
            NumberOfStars = numberOfStars;
        }

        public StarQuestion(int id) :

            base(id)
        { }

        public StarQuestion(StarQuestion starQuestion) :
         this(starQuestion.ID, starQuestion.Order, starQuestion.Text, starQuestion.Type, starQuestion.NumberOfStars)
        { }
    }
}
