using SurveyQuestionsConfigurator.CommonHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyQuestionsConfigurator.Entities
{
    public class Question
    {
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
                Helper.Logger(ex);
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
                Helper.Logger(ex);
            }
        }

        public Question(Question question) :
            this(question.ID, question.Order, question.Text, question.Type)
        { }

    }
}
