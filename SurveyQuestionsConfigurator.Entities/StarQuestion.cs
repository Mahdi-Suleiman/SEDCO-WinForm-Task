using SurveyQuestionsConfigurator.CommonHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SurveyQuestionsConfigurator.Entities.Types;

namespace SurveyQuestionsConfigurator.Entities
{
    public class StarQuestion : Question
    {
        public int NumberOfStars { get; set; }

        public StarQuestion(int id, int order, string text, QuestionType type, int numberOfStars) :
            base(id, order, text, type)
        {
            try
            {
                NumberOfStars = numberOfStars;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        public StarQuestion(int id) :

            base(id)
        { }

        public StarQuestion(StarQuestion starQuestion) :
         this(starQuestion.ID, starQuestion.Order, starQuestion.Text, starQuestion.Type, starQuestion.NumberOfStars)
        { }
    }
}
