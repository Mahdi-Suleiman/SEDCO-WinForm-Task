using SurveyQuestionsConfigurator.CommonHelpers;
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

namespace SurveyQuestionsConfigurator
{
    public partial class LangaugeSettingsForm : Form
    {
        private readonly ResourceManager mLocalResourceManager;
        private readonly CultureInfo mDefaultCulture;

        private enum ResourceStrings
        {
            somethingWrongHappenedError,
            error,
            success
        }
        public LangaugeSettingsForm()
        {
            try
            {
                mLocalResourceManager = new ResourceManager("SurveyQuestionsConfigurator.ConnectinSettingsFormStrings", typeof(SurveyQuestionsConfiguratorForm).Assembly);
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
    }
}
