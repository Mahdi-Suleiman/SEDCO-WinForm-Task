using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyQuestionsConfigurator
{
    public class CommonLayer
    {
        public static void LogError(Exception ex)
        {
            Trace.TraceError($@"
 -------------- ({DateTime.Now}) --------------
 
-Exception Type: {ex.GetType()}
-Exception Call Site: {ex.TargetSite}
-Exception Stack Trace: {ex.StackTrace}
-Exception Short Message: {ex.Message}
-Exception Long Message:
 {ex}
");
        }
    }
}
