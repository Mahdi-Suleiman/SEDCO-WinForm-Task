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
        public static void LogError(Exception pEx)
        {
            try
            {
                string exeFolder = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                using (StreamWriter sw = new StreamWriter($"{exeFolder}/logs/LogFile.txt", append: true))
                {
                    sw.WriteLine($@"
 -------------- ({DateTime.Now}) --------------

-Exception Type: {pEx.GetType()}
-Exception Call Site: {pEx.TargetSite}
-Exception Short Message: {pEx.Message}
-Exception Long Message: {pEx}
-Exception Stack Trace: {pEx.StackTrace}
");
                }
            }
            catch (Exception ex2)
            {
                BackUpLogger(ex2);
            }
        }

        //Log error in case main "LogError" faced an exception
        public static void BackUpLogger(Exception pEx)
        {
            try
            {
                string exeFolder = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                using (StreamWriter sw = new StreamWriter($"{exeFolder}/logs/LogFile.txt", append: true))
                {
                    sw.WriteLine($@"
 -------------- ({DateTime.Now}) --------------

-Exception Type: {pEx.GetType()}
-Exception Call Site: {pEx.TargetSite}
-Exception Short Message: {pEx.Message}
-Exception Long Message: {pEx}
-Exception Stack Trace: {pEx.StackTrace}
");
                }
            }
            catch
            {

            }
        }
    }
}
