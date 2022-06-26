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
        #region Properties & Attributes

        private readonly ResourceManager mLocalResourceManager;
        private readonly CultureInfo mDefaultCulture;
        private SqlConnectionStringBuilder mBuilder; /// passed to ConnectionSettingsManager (Busniess Logic Layer)
        private readonly ConnectionSettingsManager mConnectionSettingsManager;

        /// <summary>
        /// All translatable message box messages in the "ConnectionSettingsFormStrings" resource file
        /// </summary>
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
                mBuilder = new SqlConnectionStringBuilder();
                mConnectionSettingsManager = new ConnectionSettingsManager();
                mLocalResourceManager = new ResourceManager("SurveyQuestionsConfigurator.ConnectinSettingsFormStrings", typeof(ConnectionSettingsForm).Assembly);
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
        /// Get current connecion string details from config file, save it to the generic mBuilder and fill the form with it
        /// </summary>
        private void ConnectionSettingsForm_Load(object sender, EventArgs e)
        {
            try
            {
                mBuilder = mConnectionSettingsManager.GetConnectionString();

                LoadCurrentConnectionStringSettings();
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

        /// <summary>
        /// Tests the connection of the inputs 
        /// </summary>
        private void testConnectionButton_Click(object sender, EventArgs e)
        {
            try
            {
                mBuilder.Clear();
                FillConnectionStringBuilderFields();

                if (CheckConnectionStringInputFields(mBuilder) == ErrorCode.SUCCESS) /// if the connection string fields are valid
                {
                    if (mConnectionSettingsManager.CheckConnectivity(mBuilder) == ErrorCode.SUCCESS) /// check actual connectivity
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

        /// <summary>
        /// Saves the current inputs to the config file under "connectionStrings" section 
        /// </summary>
        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                FillConnectionStringBuilderFields();

                ErrorCode isSaved = mConnectionSettingsManager.SaveConnectionString(mBuilder);
                if (isSaved == ErrorCode.SUCCESS)
                {
                    this.Close();
                    this.DialogResult = DialogResult.OK;
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
                return mConnectionSettingsManager.CheckConnectionStringInputFields(pBuilder);
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

        public void LoadCurrentConnectionStringSettings()
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
