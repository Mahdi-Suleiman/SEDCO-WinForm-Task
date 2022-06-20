using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SurveyQuestionsConfigurator
{
    public class ShowMessage
    {
        public DialogResult Box(ResourceManager pLocalResourceManager, CultureInfo pDefaultCulture, string pText, string pCaption, MessageBoxButtons pButton, MessageBoxIcon pIcon)
        {
            return MessageBox.Show(pLocalResourceManager.GetString(pText), pLocalResourceManager.GetString(pCaption), pButton, pIcon, MessageBoxDefaultButton.Button1, pDefaultCulture.ToString() == "ar-JO" ? MessageBoxOptions.RightAlign : MessageBoxOptions.DefaultDesktopOnly);
        }
    }
}
