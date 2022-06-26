using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.QuestionLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SurveyQuestionsConfigurator.Entities.Generic;

namespace SurveyQuestionsConfigurator
{
    public partial class LangaugeSettingsForm : Form
    {

        #region Properties & Attributes

        private readonly ResourceManager mLocalResourceManager;
        private readonly CultureInfo mDefaultCulture;

        private ComboBoxLanguages mLoadedComboBoxLanguage; /// used to check if selected langauge is changed
        private readonly string mConfigEnglishLangauge;
        private readonly string mConfigArabicLangauge;

        private readonly Configuration tConfigFile;
        private readonly KeyValueConfigurationCollection tSettings;

        private readonly string mDefaultCultureString;

        /// <summary>
        /// All translatable message box messages in the "LanguageSettingsFormStrings" resource file
        /// </summary>
        private enum ResourceStrings
        {
            somethingWrongHappenedError,
            error,
            success,
            errorSavingSettings
        }

        /// <summary>
        /// Langauges to be set to or get from combo box
        /// </summary>
        private enum ComboBoxLanguages
        {
            English = 0,
            Arabic = 1,
            NoLanguage = -1
        }

        #endregion

        #region Constructor
        public LangaugeSettingsForm()
        {
            try
            {
                mLocalResourceManager = new ResourceManager("SurveyQuestionsConfigurator.LanguageSettingsFormStrings", typeof(LangaugeSettingsForm).Assembly);
                mDefaultCulture = new CultureInfo(ConfigurationManager.AppSettings["DefaultCulture"]);

                mLoadedComboBoxLanguage = ComboBoxLanguages.NoLanguage;
                mConfigEnglishLangauge = "en-US";
                mConfigArabicLangauge = "ar-JO";

                tConfigFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                tSettings = tConfigFile.AppSettings.Settings;
                mDefaultCultureString = "DefaultCulture";

                Thread.CurrentThread.CurrentUICulture = mDefaultCulture;

                InitializeComponent();
            }
            catch (Exception ex)
            {
                ShowMessage.Box($"{ResourceStrings.somethingWrongHappenedError}", $"{ResourceStrings.error}", MessageBoxButtons.OK, MessageBoxIcon.Error, mLocalResourceManager, mDefaultCulture);
                Logger.LogError(ex);
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Close this form
        /// </summary>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                ShowMessage.Box($"{ResourceStrings.somethingWrongHappenedError}", $"{ResourceStrings.error}", MessageBoxButtons.OK, MessageBoxIcon.Error, mLocalResourceManager, mDefaultCulture);
                Logger.LogError(ex);
            }
        }

        /// <summary>
        /// Load current langauge settings to form
        /// </summary>
        private void LangaugeSettingsForm_Load(object sender, EventArgs e)
        {
            try
            {
                LoadLangaugeSettings();
            }
            catch (Exception ex)
            {
                ShowMessage.Box($"{ResourceStrings.somethingWrongHappenedError}", $"{ResourceStrings.error}", MessageBoxButtons.OK, MessageBoxIcon.Error, mLocalResourceManager, mDefaultCulture);
                Logger.LogError(ex);
            }
        }

        /// <summary>
        /// Handle saving new langauge settings
        /// Restart application if new langauge is saved other than current application's language
        /// </summary>
        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                ComboBoxLanguages tCurrentSelectedIndex = (ComboBoxLanguages)languageComboBox.SelectedIndex;

                /// close the form if the selected language didn't change
                if (tCurrentSelectedIndex == mLoadedComboBoxLanguage)
                {
                    this.Close();
                }
                else
                {
                    /// Saving english langauge
                    if (tCurrentSelectedIndex == ComboBoxLanguages.English)
                    {
                        ErrorCode tResult = SaveLangaugeSettings(mConfigEnglishLangauge);
                        if (tResult == ErrorCode.SUCCESS)
                        {
                            Application.Restart();
                        }
                        else
                        {
                            ShowMessage.Box($"{ResourceStrings.errorSavingSettings}", $"{ResourceStrings.error}", MessageBoxButtons.OK, MessageBoxIcon.Error, mLocalResourceManager, mDefaultCulture);
                        }
                    }
                    /// Saving arabic langauge
                    else if (tCurrentSelectedIndex == ComboBoxLanguages.Arabic)
                    {
                        ErrorCode tResult = SaveLangaugeSettings(mConfigArabicLangauge);
                        if (tResult == ErrorCode.SUCCESS)
                        {
                            Application.Restart();
                        }
                        else
                        {
                            ShowMessage.Box($"{ResourceStrings.errorSavingSettings}", $"{ResourceStrings.error}", MessageBoxButtons.OK, MessageBoxIcon.Error, mLocalResourceManager, mDefaultCulture);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage.Box($"{ResourceStrings.somethingWrongHappenedError}", $"{ResourceStrings.error}", MessageBoxButtons.OK, MessageBoxIcon.Error, mLocalResourceManager, mDefaultCulture);
                Logger.LogError(ex);
            }
        }

        #endregion

        #region Generic Methods

        /// <summary>
        /// Load combo box with the current language of the application
        /// </summary>
        private void LoadLangaugeSettings()
        {
            try
            {
                string tReturnedLangaugeValue = tSettings[mDefaultCultureString].Value;


                if (tReturnedLangaugeValue == mConfigEnglishLangauge)
                {
                    languageComboBox.SelectedIndex = (int)ComboBoxLanguages.English;
                    mLoadedComboBoxLanguage = (ComboBoxLanguages)languageComboBox.SelectedIndex;
                }
                else if (tReturnedLangaugeValue == mConfigArabicLangauge)
                {
                    languageComboBox.SelectedIndex = (int)ComboBoxLanguages.Arabic;
                    mLoadedComboBoxLanguage = (ComboBoxLanguages)languageComboBox.SelectedIndex;
                }
                else
                {
                    languageComboBox.SelectedIndex = (int)ComboBoxLanguages.NoLanguage;
                }
            }
            catch (Exception ex)
            {
                ShowMessage.Box($"{ResourceStrings.somethingWrongHappenedError}", $"{ResourceStrings.error}", MessageBoxButtons.OK, MessageBoxIcon.Error, mLocalResourceManager, mDefaultCulture);
                Logger.LogError(ex);
            }
        }

        /// <summary>
        /// Saves new settings to config file
        /// </summary>
        /// <returns>
        /// ErrorCode
        /// </returns>
        private ErrorCode SaveLangaugeSettings(string pLangaugeToSave)
        {
            try
            {
                tSettings[mDefaultCultureString].Value = pLangaugeToSave;
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

        #endregion
    }
}
