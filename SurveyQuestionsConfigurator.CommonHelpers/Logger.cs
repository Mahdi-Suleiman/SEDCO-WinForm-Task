using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SurveyQuestionsConfigurator.CommonHelpers
{
    public class Logger
    {
        public static void LogError(Exception ex)
        {
            try
            {
                string exeFolder = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
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
            }
            catch (Exception ex2)
            {
                BackUpLogger(ex2);
            }
        }

        //Log error in case main "LogError" faced an exception
        public static void BackUpLogger(Exception ex)
        {
            try
            {
                string exeFolder = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
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
            }
            catch (Exception ex2)
            {

            }
        }
    }
}
