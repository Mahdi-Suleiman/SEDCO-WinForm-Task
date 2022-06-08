using SurveyQuestionsConfigurator.CommonHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SurveyQuestionsConfigurator.Entities.Generic;

namespace SurveyQuestionsConfigurator.Entities
{
    public class Question
    {
        public int ID { get; set; }
        public int Order { get; set; }
        public string Text { get; set; }
        public QuestionType Type { get; set; }

        public Question(int pID)
        {
            try
            {
                ID = pID;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        public Question(int pID, int pOrder, string pText, QuestionType pType)
        {
            try
            {
                ID = pID;
                Order = pOrder;
                Text = pText;
                Type = pType;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        public Question(Question pQuestion) :
            this(pQuestion.ID, pQuestion.Order, pQuestion.Text, pQuestion.Type)
        { }

    }
}
