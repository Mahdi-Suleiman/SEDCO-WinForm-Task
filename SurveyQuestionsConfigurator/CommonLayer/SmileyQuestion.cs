using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyQuestionsConfigurator.CommonLayer
{
    public class SmileyQuestion : Question
    {
        public int NumberOfSmileyFaces { get; set; }

        public SmileyQuestion(int id, int order, string text, int type, int numberOfSmileyFaces) :
            base(id, order, text, type)
        {
            NumberOfSmileyFaces = numberOfSmileyFaces;
        }

        public SmileyQuestion(int id) :

            base(id)
        { }

        public SmileyQuestion(SmileyQuestion smileyQuestion) :
            this(smileyQuestion.ID, smileyQuestion.Order, smileyQuestion.Text, smileyQuestion.Type, smileyQuestion.NumberOfSmileyFaces)
        { }
    }
}
