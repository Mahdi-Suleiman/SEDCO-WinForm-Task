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
        SqlConnectionStringBuilder mBuilder;
        SettingsManager mSettingsManager;
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
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        }

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
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
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
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
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
                        MessageBox.Show("Test connection succeeded.\nYou can save settings now", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        saveButton.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("Test connection failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        saveButton.Enabled = false;
                    }
                }
                else
                {
                    MessageBox.Show("None of the input fields above can be empty\nNor be more than 128 character max", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        }

        private ErrorCode CheckConnectionStringInputFields(SqlConnectionStringBuilder pBuilder)
        {
            try
            {
                return mSettingsManager.CheckConnectionStringInputFields(pBuilder);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (mSettingsManager.SaveConnectionString(mBuilder) == ErrorCode.SUCCESS)
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Couldn't save settings, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        }

        private void dataSourceTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                saveButton.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Logger.LogError(ex);
            }
        }
    }
}
