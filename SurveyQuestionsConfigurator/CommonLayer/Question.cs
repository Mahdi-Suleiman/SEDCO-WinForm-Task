using SurveyQuestionsConfigurator.CommonLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyQuestionsConfigurator.CommonLayer
{
    internal class Question
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
        public QuestionType Type { get; set; }

        public Question(int id, int order, string text, int type)
        {
            try
            {
                ID = id;
                Order = order;
                Text = text;
                Type = (QuestionType)type;
            }
            catch (Exception ex)
            {
                Helper.LogError(ex);
            }
        }

    }
}
