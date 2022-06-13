using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SurveyQuestionsConfigurator.Entities.Generic;

namespace SurveyQuestionsConfigurator.QuestionLogic
{
    public class SettingsManager
    {
        public ErrorCode CheckConnectionStringInputFields(SqlConnectionStringBuilder pBuilder)
        {
            if (!String.IsNullOrWhiteSpace(pBuilder.DataSource.ToString()) && pBuilder.DataSource.Length <= 128)
                if (!String.IsNullOrWhiteSpace(pBuilder.InitialCatalog.ToString()) && pBuilder.InitialCatalog.Length <= 128)
                    if (!String.IsNullOrWhiteSpace(pBuilder.UserID.ToString()) && pBuilder.UserID.Length <= 128)
                        if (!String.IsNullOrWhiteSpace(pBuilder.Password.ToString()) && pBuilder.Password.Length <= 128)
                            return ErrorCode.SUCCESS;
            return ErrorCode.ERROR;
        }
    }
}
