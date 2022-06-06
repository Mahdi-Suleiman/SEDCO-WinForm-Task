using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyQuestionsConfigurator.CommonTypes
{
    public class Types
    {
        public enum ErrorCode
        {
            SUCCESS,
            ERROR,
            SQLVIOLATION,
            EMPTY
        }

        public enum Question
        {
            SMILEY = 0,
            SLIDER = 1,
            STAR = 2
        }
    }
}
