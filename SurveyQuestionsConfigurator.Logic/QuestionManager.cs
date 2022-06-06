using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.CommonTypes;
using SurveyQuestionsConfigurator.Entities;
using SurveyQuestionsConfigurator.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyQuestionsConfigurator.QuestionLogic
{
    public class QuestionManager
    {
        #region Add Question Functions
        public int AddSmileyQuestion(SmileyQuestion smileyQuestion)
        {
            try
            {
                SmileyQuestionRepository smileyQuestionRepository = new SmileyQuestionRepository();
                if (CheckSmileyQuestionValues(smileyQuestion))
                {
                    return smileyQuestionRepository.Add(smileyQuestion);
                }
                else
                {
                    return (int)Types.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return (int)Types.ErrorCode.ERROR;
            }
        }
        public int AddSliderQuestion(SliderQuestion sliderQuestion)
        {
            try
            {
                SliderQuestionRepository sliderQuestionRepository = new SliderQuestionRepository();
                if (CheckSliderQuestionValues(sliderQuestion))
                {
                    return sliderQuestionRepository.Add(sliderQuestion);
                }
                else
                {
                    return (int)Types.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return (int)Types.ErrorCode.ERROR;
            }
        }
        public int AddStarQuestion(StarQuestion starQuestion)
        {
            try
            {
                StarQuestionRepository starQuestionRepository = new StarQuestionRepository();
                if (CheckStarQuestionValues(starQuestion))
                {
                    return starQuestionRepository.Add(starQuestion);
                }
                else
                {
                    return (int)Types.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return (int)Types.ErrorCode.ERROR;
            }
        }

        #endregion

        #region Edit Question Functions
        public int EditSmileyQuestion(SmileyQuestion smileyQuestion)
        {
            try
            {
                SmileyQuestionRepository smileyQuestionRepository = new SmileyQuestionRepository();
                if (CheckSmileyQuestionValues(smileyQuestion))
                {
                    return smileyQuestionRepository.Update(smileyQuestion);
                }
                else
                {
                    return (int)Types.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return (int)Types.ErrorCode.ERROR;
            }
        }
        public int EditSliderQuestion(SliderQuestion sliderQuestion)
        {
            try
            {
                SliderQuestionRepository sliderQuestionRepository = new SliderQuestionRepository();
                if (CheckSliderQuestionValues(sliderQuestion))
                {
                    return sliderQuestionRepository.Update(sliderQuestion);
                }
                else
                {
                    return (int)Types.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return (int)Types.ErrorCode.ERROR;
            }
        }
        public int EditStarQuestion(StarQuestion starQuestion)
        {
            try
            {
                StarQuestionRepository starQuestionRepository = new StarQuestionRepository();
                if (CheckStarQuestionValues(starQuestion))
                {
                    return starQuestionRepository.Update(starQuestion);
                }
                else
                {
                    return (int)Types.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return (int)Types.ErrorCode.ERROR;
            }
        }

        #endregion

        #region Get Question Functions
        public int GetSmileyQuestionByID(int questionId, ref DataTable dataTable)
        {
            try
            {
                SmileyQuestionRepository smileyQuestionRepository = new SmileyQuestionRepository();
                if (questionId > 0)
                {
                    return smileyQuestionRepository.Get(questionId, ref dataTable);
                }
                else
                {
                    return (int)Types.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return (int)Types.ErrorCode.ERROR;
            }
        }
        public int GetSliderQuestionByID(int questionId, ref DataTable dataTable)
        {
            try
            {
                SliderQuestionRepository sliderQuestionRepository = new SliderQuestionRepository();
                if (questionId > 0)
                {
                    return sliderQuestionRepository.Get(questionId, ref dataTable);
                }
                else
                {
                    return (int)Types.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return (int)Types.ErrorCode.ERROR;
            }
        }
        public int GetStarQuestionByID(int questionId, ref DataTable dataTable)
        {
            try
            {
                StarQuestionRepository starQuestionRepository = new StarQuestionRepository();
                if (questionId > 0)
                {
                    return starQuestionRepository.Get(questionId, ref dataTable);
                }
                else
                {
                    return (int)Types.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return (int)Types.ErrorCode.ERROR;
            }
        }
        public int GetAllQuestions(ref DataTable dataTable)
        {
            try
            {
                GenericRepository repository = new GenericRepository();
                return repository.GetAll(ref dataTable);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return (int)Types.ErrorCode.ERROR;
            }
        }

        #endregion

        #region Delete Question Functions
        public int DeleteQuestionByID(int questionId)
        {
            try
            {
                Question question = new Question(questionId);
                GenericRepository repository = new GenericRepository();
                if (questionId > 0)
                {
                    return repository.Delete(questionId);
                }
                else
                {
                    return (int)Types.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return (int)Types.ErrorCode.ERROR;
            }
        }

        #endregion


        #region Validation Functions

        public bool CheckSmileyQuestionValues(SmileyQuestion smileyQuestion)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(smileyQuestion.Text) && smileyQuestion.Text.Length < 8000) //if Question text is not null or empty 
                    if (smileyQuestion.NumberOfSmileyFaces >= 2 && smileyQuestion.NumberOfSmileyFaces <= 5)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return false;
            }
        } // end func.

        public bool CheckSliderQuestionValues(SliderQuestion sliderQuestion)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(sliderQuestion.Text)) //if Question text is not null or empty 
                    if (!String.IsNullOrWhiteSpace(sliderQuestion.StartValueCaption) && sliderQuestion.StartValueCaption.Length < 100)
                        if (!String.IsNullOrWhiteSpace(sliderQuestion.EndValueCaption) && sliderQuestion.EndValueCaption.Length < 100)
                            if (sliderQuestion.StartValue < sliderQuestion.EndValue)
                                return true;
                return false;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return false;
            }
        } // end func.

        public bool CheckStarQuestionValues(StarQuestion starQuestion)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(starQuestion.Text) && starQuestion.Text.Length < 8000) //if Question text is not null or empty 
                    if (starQuestion.NumberOfStars >= 1 && starQuestion.NumberOfStars <= 10)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return false;
            }
        } // end func.

        #endregion
    }
}
