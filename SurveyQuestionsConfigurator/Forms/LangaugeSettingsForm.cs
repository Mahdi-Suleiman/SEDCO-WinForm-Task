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
        private readonly LanguageSettingsManager mLanguageSettingsManager;

        private ComboBoxLanguages mLoadedComboBoxLanguage;
        private readonly string mConfigEnglishLangauge;
        private readonly string mConfigArabicLangauge;
        private string mReturnedLangaugeValue;

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
                mLoadedComboBoxLanguage = ComboBoxLanguages.NoLanguage;
                mConfigEnglishLangauge = "en-US";
                mConfigArabicLangauge = "ar-JO";

                mLanguageSettingsManager = new LanguageSettingsManager();
                mLocalResourceManager = new ResourceManager("SurveyQuestionsConfigurator.LanguageSettingsFormStrings", typeof(LangaugeSettingsForm).Assembly);
                mDefaultCulture = new CultureInfo(ConfigurationManager.AppSettings["DefaultCulture"]);

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
                        ErrorCode tResult = mLanguageSettingsManager.SaveLangaugeSettings(mConfigEnglishLangauge);
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
                        ErrorCode tResult = mLanguageSettingsManager.SaveLangaugeSettings(mConfigArabicLangauge);
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
                ErrorCode result = mLanguageSettingsManager.LoadLangaugeSettings(ref mReturnedLangaugeValue);
                if (result == ErrorCode.SUCCESS)
                {
                    if (mReturnedLangaugeValue == mConfigEnglishLangauge)
                    {
                        languageComboBox.SelectedIndex = 0;
                        mLoadedComboBoxLanguage = (ComboBoxLanguages)languageComboBox.SelectedIndex;
                    }
                    else if (mReturnedLangaugeValue == mConfigArabicLangauge)
                    {
                        languageComboBox.SelectedIndex = 1;
                        mLoadedComboBoxLanguage = (ComboBoxLanguages)languageComboBox.SelectedIndex;
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
    }
}
