using SurveyQuestionsConfigurator.Entities;
using SurveyQuestionsConfigurator.QuestionLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SurveyQuestionsConfigurator
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Form form = new SurveyQuestionsConfiguratorForm();
            //QuestionMonitor qm = new QuestionMonitor();
            //qm.Subscribe((IObserver<Question>)form);

            Application.Run(new SurveyQuestionsConfiguratorForm());
        }
    }
}
