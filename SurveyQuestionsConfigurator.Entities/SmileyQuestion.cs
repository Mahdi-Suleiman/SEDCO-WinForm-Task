using SurveyQuestionsConfigurator.CommonHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SurveyQuestionsConfigurator.Entities.Types;

namespace SurveyQuestionsConfigurator.Entities
{
    public class SmileyQuestion : Question
    {
        public int NumberOfSmileyFaces { get; set; }

        public SmileyQuestion(int id, int order, string text, QuestionType type, int numberOfSmileyFaces) :
            base(id, order, text, type)
        {
            try
            {
                NumberOfSmileyFaces = numberOfSmileyFaces;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }

        }

        public SmileyQuestion(int id) :

            base(id)
        { }

        public SmileyQuestion(SmileyQuestion smileyQuestion) :
            this(smileyQuestion.ID, smileyQuestion.Order, smileyQuestion.Text, smileyQuestion.Type, smileyQuestion.NumberOfSmileyFaces)
        { }
    }
}
