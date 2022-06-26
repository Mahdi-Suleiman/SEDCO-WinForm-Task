using SurveyQuestionsConfigurator.CommonHelpers;
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
        ///<summary>
        /// Custom methods to show message box dialog
        /// </summary>
        /// <returns>
        /// DialogResult
        /// </returns>
        public static DialogResult Box(string pText, string pCaption, MessageBoxButtons pButton, MessageBoxIcon pIcon,
            ResourceManager pLocalResourceManager = null, CultureInfo pDefaultCulture = null)
        {
            try
            {
                if (pDefaultCulture.ToString() == "ar-JO")
                {
                    return MessageBox.Show(pLocalResourceManager.GetString(pText), pLocalResourceManager.GetString(pCaption),
                        pButton, pIcon, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign); /// Handle RTL Message Box 
                }
                return MessageBox.Show(pLocalResourceManager.GetString(pText), pLocalResourceManager.GetString(pCaption),
                    pButton, pIcon); /// Handle LTR Mssage box
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }
    }
}
