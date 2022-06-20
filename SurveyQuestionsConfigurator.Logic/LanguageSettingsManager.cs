using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SurveyQuestionsConfigurator.Entities.Generic;

namespace SurveyQuestionsConfigurator.QuestionLogic
{
    public class LanguageSettingsManager
    {

        //public LanguageSettingsManager()
        //{
        //    try
        //    {
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogError(ex); /// write error to log file
        //    }
        //}
        public ErrorCode LoadLangaugeSettings(ref string pReturnedLangaugeString)
        {
            try
            {
                var tConfigFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var tSettings = tConfigFile.AppSettings.Settings;
                pReturnedLangaugeString = tSettings["DefaultCulture"].Value;

                //tSettings["DefaultCulture"].Value = "en-US";
                //tConfigFile.Save(ConfigurationSaveMode.Modified);
                //ConfigurationManager.RefreshSection(tConfigFile.AppSettings.SectionInformation.Name);

                return ErrorCode.SUCCESS;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); /// write error to log file
                pReturnedLangaugeString = null;
                return ErrorCode.ERROR;
            }
        }

        public ErrorCode SaveLangaugeSettings(string pLangaugeToSave)
        {
            try
            {
                var tConfigFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var tSettings = tConfigFile.AppSettings.Settings;

                tSettings["DefaultCulture"].Value = pLangaugeToSave;
                tConfigFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(tConfigFile.AppSettings.SectionInformation.Name);

                return ErrorCode.SUCCESS;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); /// write error to log file
                return ErrorCode.ERROR;
            }
        }
    }
}
