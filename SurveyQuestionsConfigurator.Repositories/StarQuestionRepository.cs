﻿using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SurveyQuestionsConfigurator.Entities.Types;

namespace SurveyQuestionsConfigurator.Repositories
{
    public class StarQuestionRepository : GenericRepository
    {
        public ErrorCode Add(StarQuestion starQuestion)
        {
            try
            {
                return dbConnect.AddStarQuestion(starQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Types.ErrorCode.ERROR;
            }
        }

        public ErrorCode Get(ref StarQuestion starQuestion)
        {
            try
            {
                return dbConnect.GetStarQuestionByID(ref starQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Types.ErrorCode.ERROR;
            }
        }

        public ErrorCode Update(StarQuestion starQuestion)
        {
            try
            {
                return dbConnect.EditStarQuestion(starQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Types.ErrorCode.ERROR;
            }
        }
    }
}
