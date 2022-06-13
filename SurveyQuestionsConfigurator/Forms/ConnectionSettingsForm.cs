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
            InitializeComponent();
            mBuilder = new SqlConnectionStringBuilder();
            mSettingsManager = new SettingsManager();
        }

        private void ConnectionSettingsForm_Load(object sender, EventArgs e)
        {

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
                mBuilder.InitialCatalog = databaseTextBox.Text.ToString();
                mBuilder.UserID = userIDTextBox.Text.ToString();
                mBuilder.Password = passwordTextBox.Text.ToString();
                if (CheckConnectionStringInputFields(mBuilder) == ErrorCode.SUCCESS)
                {
                    MessageBox.Show(mBuilder.ConnectionString);
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
    }
}
