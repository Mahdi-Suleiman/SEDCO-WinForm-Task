using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.QuestionLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
    public partial class ConnectionSettingsForm : Form
    {
        #region Properties

        private readonly ResourceManager mLocalResourceManager;
        private readonly CultureInfo mDefaultCulture;
        private SqlConnectionStringBuilder mBuilder;
        private readonly SettingsManager mSettingsManager;

        private enum ResourceStrings
        {
            somethingWrongHappenedError,
            couldNotSaveSettingsError,
            testConnectionSucceeded,
            testConnectionFailedError,
            emptyInputFieldsError,
            error,
            success
        }
        #endregion

        #region Constructor
        public ConnectionSettingsForm()
        {
            try
            {
                mLocalResourceManager = new ResourceManager("SurveyQuestionsConfigurator.ConnectinSettingsFormStrings", typeof(SurveyQuestionsConfiguratorForm).Assembly);
                mDefaultCulture = new CultureInfo(ConfigurationManager.AppSettings["DefaultCulture"]);
                Thread.CurrentThread.CurrentUICulture = mDefaultCulture;

                InitializeComponent();
                mBuilder = new SqlConnectionStringBuilder();
                mSettingsManager = new SettingsManager();
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
        /// Get current connecion string details and fill the form with it
        /// </summary>
        private void ConnectionSettingsForm_Load(object sender, EventArgs e)
        {
            try
            {
                mBuilder = mSettingsManager.GetConnectionString();

                FillFormWithConnectionStringBuilderData();
            }
            catch (Exception ex)
            {
                ShowMessage.Box($"{ResourceStrings.somethingWrongHappenedError}", $"{ResourceStrings.error}", MessageBoxButtons.OK, MessageBoxIcon.Error, mLocalResourceManager, mDefaultCulture);
                Logger.LogError(ex);
            }
        }

        /// <summary>
        /// Close the form
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

        private void testConnectionButton_Click(object sender, EventArgs e)
        {
            try
            {
                mBuilder.Clear();
                FillConnectionStringBuilderFields();

                if (CheckConnectionStringInputFields(mBuilder) == ErrorCode.SUCCESS)
                {
                    if (mSettingsManager.CheckConnectivity(mBuilder) == ErrorCode.SUCCESS)
                    {
                        ShowMessage.Box($"{ResourceStrings.testConnectionSucceeded}", $"{ResourceStrings.success}", MessageBoxButtons.OK, MessageBoxIcon.Information, mLocalResourceManager, mDefaultCulture);
                    }
                    else
                    {
                        ShowMessage.Box($"{ResourceStrings.testConnectionFailedError}", $"{ResourceStrings.error}", MessageBoxButtons.OK, MessageBoxIcon.Error, mLocalResourceManager, mDefaultCulture);
                    }
                }
                else
                {
                    ShowMessage.Box($"{ResourceStrings.emptyInputFieldsError}", $"{ResourceStrings.error}", MessageBoxButtons.OK, MessageBoxIcon.Error, mLocalResourceManager, mDefaultCulture);
                }

            }
            catch (Exception ex)
            {
                ShowMessage.Box($"{ResourceStrings.somethingWrongHappenedError}", $"{ResourceStrings.error}", MessageBoxButtons.OK, MessageBoxIcon.Error, mLocalResourceManager, mDefaultCulture);
                Logger.LogError(ex);
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                FillConnectionStringBuilderFields();

                ErrorCode isSaved = mSettingsManager.SaveConnectionString(mBuilder);
                if (isSaved == ErrorCode.SUCCESS)
                {
                    this.Close();
                }
                else
                {
                    ShowMessage.Box($"{ResourceStrings.couldNotSaveSettingsError}", $"{ResourceStrings.error}", MessageBoxButtons.OK, MessageBoxIcon.Error, mLocalResourceManager, mDefaultCulture);
                }
            }
            catch (Exception ex)
            {
                ShowMessage.Box($"{ResourceStrings.somethingWrongHappenedError}", $"{ResourceStrings.error}", MessageBoxButtons.OK, MessageBoxIcon.Error, mLocalResourceManager, mDefaultCulture);
                Logger.LogError(ex);
            }
        }

        #endregion

        #region Validation methods

        /// <summary>
        /// Check connection string fields
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// </returns>
        private ErrorCode CheckConnectionStringInputFields(SqlConnectionStringBuilder pBuilder)
        {
            try
            {
                return mSettingsManager.CheckConnectionStringInputFields(pBuilder);
            }
            catch (Exception ex)
            {
                ShowMessage.Box($"{ResourceStrings.somethingWrongHappenedError}", $"{ResourceStrings.error}", MessageBoxButtons.OK, MessageBoxIcon.Error, mLocalResourceManager, mDefaultCulture);
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }

        #endregion

        #region General Methods

        public void FillConnectionStringBuilderFields()
        {
            try
            {
                mBuilder.DataSource = dataSourceTextBox.Text.ToString();
                mBuilder.InitialCatalog = initialCatalogTextBox.Text.ToString();
                mBuilder.UserID = userIDTextBox.Text.ToString();
                mBuilder.Password = passwordTextBox.Text.ToString();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        public void FillFormWithConnectionStringBuilderData()
        {
            try
            {
                dataSourceTextBox.Text = mBuilder.DataSource;
                initialCatalogTextBox.Text = mBuilder.InitialCatalog;
                userIDTextBox.Text = mBuilder.UserID;
                passwordTextBox.Text = mBuilder.Password;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        #endregion
    }
}
