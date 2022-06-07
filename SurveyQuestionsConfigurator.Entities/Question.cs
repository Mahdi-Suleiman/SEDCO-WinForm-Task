using SurveyQuestionsConfigurator.CommonHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SurveyQuestionsConfigurator.Entities.Types;

namespace SurveyQuestionsConfigurator.Entities
{
    public class Question
    {
        public int ID { get; set; }
        public int Order { get; set; }
        public string Text { get; set; }
        public QuestionType Type { get; set; }

        public Question(int id)
        {
            try
            {
                ID = id;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        public Question(int id, int order, string text, QuestionType type)
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
                Logger.LogError(ex);
            }
        }

        public Question(Question question) :
            this(question.ID, question.Order, question.Text, question.Type)
        { }

    }
}
