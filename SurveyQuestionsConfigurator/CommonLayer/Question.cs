using SurveyQuestionsConfigurator.CommonLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyQuestionsConfigurator.CommonLayer
{
    public class Question
    {
        public enum QuestionType
        {
            SMILEY = 0,
            SLIDER = 1,
            STAR = 2
        }
        public int ID { get; set; }
        public int Order { get; set; }
        public string Text { get; set; }
        public int Type { get; set; }

        public Question(int id)
        {
            try
            {
                ID = id;
            }
            catch (Exception ex)
            {
                CommonHelpers.Logger(ex);
            }
        }

        public Question(int id, int order, string text, int type)
        {
            try
            {
                ID = id;
                Order = order;
                Text = text;
                Type = type;
            }
            catch (Exception ex)
            {
                CommonHelpers.Logger(ex);
            }
        }

        public Question(Question question) :
            this(question.ID, question.Order, question.Text, question.Type)
        { }

    }
}
