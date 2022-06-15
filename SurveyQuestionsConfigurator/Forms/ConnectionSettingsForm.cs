using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.QuestionLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SurveyQuestionsConfigurator.Entities.Generic;

namespace SurveyQuestionsConfigurator
{
    public partial class ConnectionSettingsForm : Form
    {
        #region Properties

        private SqlConnectionStringBuilder mBuilder;
        private readonly SettingsManager mSettingsManager;
        #endregion

        #region Constructor
        public ConnectionSettingsForm()
        {
            try
            {
                InitializeComponent();
                mBuilder = new SqlConnectionStringBuilder();
                mSettingsManager = new SettingsManager();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        }

        #endregion

        #region Event Handlers
        private void ConnectionSettingsForm_Load(object sender, EventArgs e)
        {
            try
            {
                mBuilder = mSettingsManager.GetConnectionString();

                dataSourceTextBox.Text = mBuilder.DataSource;
                initialCatalogTextBox.Text = mBuilder.InitialCatalog;
                userIDTextBox.Text = mBuilder.UserID;
                passwordTextBox.Text = mBuilder.Password;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        }

        private void testConnectionButton_Click(object sender, EventArgs e)
        {
            try
            {
                mBuilder.Clear();
                mBuilder.DataSource = dataSourceTextBox.Text.ToString();
                mBuilder.InitialCatalog = initialCatalogTextBox.Text.ToString();
                mBuilder.UserID = userIDTextBox.Text.ToString();
                mBuilder.Password = passwordTextBox.Text.ToString();

                if (CheckConnectionStringInputFields(mBuilder) == ErrorCode.SUCCESS)
                {
                    if (mSettingsManager.CheckConnectivity(mBuilder) == ErrorCode.SUCCESS)
                    {
                        MessageBox.Show("Test connection succeeded.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Test connection failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("None of the input fields above can be empty\nNor be more than 128 character max", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                mBuilder.DataSource = dataSourceTextBox.Text.ToString();
                mBuilder.InitialCatalog = initialCatalogTextBox.Text.ToString();
                mBuilder.UserID = userIDTextBox.Text.ToString();
                mBuilder.Password = passwordTextBox.Text.ToString();

                ErrorCode isSaved = mSettingsManager.SaveConnectionString(mBuilder);
                if (isSaved == ErrorCode.SUCCESS)
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Couldn't save settings, please try again\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }

        #endregion
    }
}
