using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyQuestionsConfigurator.CommonLayer
{
    public class Helper
    {
        public static void LogError(Exception ex)
        {
            Trace.TraceError($@"
 -------------- ({DateTime.Now}) --------------
 
-Exception Type: {ex.GetType()}
-Exception Call Site: {ex.TargetSite}
-Exception Short Message: {ex.Message}
-Exception Stack Trace: {ex.StackTrace}
");
        }
    }
}
