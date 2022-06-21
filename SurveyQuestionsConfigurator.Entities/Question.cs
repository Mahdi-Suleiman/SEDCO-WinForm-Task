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

        public Question(int pID, QuestionType pType)
        {
            try
            {
                ID = pID;
                Type = pType;
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


        public override bool Equals(object pObject)
        {
            if (pObject == null ||
                this.GetType() != pObject.GetType()
                )
            {
                return false;
            }

            Question q = (Question)pObject;
            if (
                q.ID == this.ID &&
                q.Order == this.Order &&
                q.Text == this.Text
                )
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
