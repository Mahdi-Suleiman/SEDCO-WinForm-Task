using System;
using System.Collections.Generic;
//using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SurveyQuestionsConfigurator.CommonLayer
{
    public class CommonHelpers
    {
        public static void Logger(Exception ex)
        {
            string exeFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

            // Write each directory name to a file.
            using (StreamWriter sw = new StreamWriter($"{exeFolder}/logs/LogFile.txt", append: true))
            {
                sw.WriteLine($@"
 -------------- ({DateTime.Now}) --------------
 
-Exception Type: {ex.GetType()}
-Exception Call Site: {ex.TargetSite}
-Exception Short Message: {ex.Message}
-Exception Long Message: {ex}
-Exception Stack Trace: {ex.StackTrace}
");
            }
            //            Trace.TraceError($@"
            // -------------- ({DateTime.Now}) --------------

            //-Exception Type: {ex.GetType()}
            //-Exception Call Site: {ex.TargetSite}
            //-Exception Short Message: {ex.Message}
            //-Exception Long Message: {ex}
            //-Exception Stack Trace: {ex.StackTrace}
            //");
        }
    }
}
