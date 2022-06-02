using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyQuestionsConfigurator.CommonLayer
{
    public class CommonHelpers
    {
        public static void Logger(Exception ex)
        {
            Trace.TraceError($@"
 -------------- ({DateTime.Now}) --------------
 
-Exception Type: {ex.GetType()}
-Exception Call Site: {ex.TargetSite}
-Exception Short Message: {ex.Message}
-Exception Long Message: {ex}
-Exception Stack Trace: {ex.StackTrace}
");
        }
    }
}
