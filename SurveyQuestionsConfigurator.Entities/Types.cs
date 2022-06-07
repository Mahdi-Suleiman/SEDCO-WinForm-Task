using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyQuestionsConfigurator.Entities
{
    public class Types
    {
        public enum ErrorCode
        {
            SUCCESS,
            ERROR,
            SQL_VIOLATION,
            EMPTY
        }

        public enum QuestionType
        {
            SMILEY = 0,
            SLIDER = 1,
            STAR = 2
        }
        public enum QuestionColumn
        {
            ID,
            ORDER,
            TEXT,
            TYPE,
            NUMBER_OF_SMILEY_FACES,
            NUMBER_OF_STARS,
            START_VALUE,
            END_VALUE,
            START_VALUE_CAPTION,
            END_VALUE_CAPTION
        }
    }
}
