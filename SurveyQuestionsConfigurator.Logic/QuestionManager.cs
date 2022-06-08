using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.Entities;
using SurveyQuestionsConfigurator.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SurveyQuestionsConfigurator.Entities.Generic;

namespace SurveyQuestionsConfigurator.QuestionLogic
{
    public class QuestionManager
    {
        SmileyQuestionRepository mSmileyQuestionRepository = new SmileyQuestionRepository();
        SliderQuestionRepository mSliderQuestionRepository = new SliderQuestionRepository();
        StarQuestionRepository mStarQuestionRepository = new StarQuestionRepository();
        GenericRepository mRepository = new GenericRepository();

        #region Add Question Functions
        public ErrorCode InsertSmileyQuestion(SmileyQuestion pSmileyQuestion)
        {
            try
            {
                if (CheckSmileyQuestionValues(pSmileyQuestion) == ErrorCode.SUCCESS)
                {
                    return mSmileyQuestionRepository.Add(pSmileyQuestion);
                }
                else
                {
                    return Generic.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }
        public ErrorCode InsertSliderQuestion(SliderQuestion pSliderQuestion)
        {
            try
            {
                if (CheckSliderQuestionValues(pSliderQuestion) == ErrorCode.SUCCESS)
                {
                    return mSliderQuestionRepository.Add(pSliderQuestion);
                }
                else
                {
                    return Generic.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }
        public ErrorCode InsertStarQuestion(StarQuestion pStarQuestion)
        {
            try
            {
                if (CheckStarQuestionValues(pStarQuestion) == ErrorCode.SUCCESS)
                {
                    return mStarQuestionRepository.Add(pStarQuestion);
                }
                else
                {
                    return Generic.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }

        #endregion

        #region Edit Question Functions
        public ErrorCode UpdateSmileyQuestion(SmileyQuestion pSmileyQuestion)
        {
            try
            {
                if (CheckSmileyQuestionValues(pSmileyQuestion) == ErrorCode.SUCCESS)
                {
                    return mSmileyQuestionRepository.Update(pSmileyQuestion);
                }
                else
                {
                    return Generic.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }
        public ErrorCode UpdateSliderQuestion(SliderQuestion pSliderQuestion)
        {
            try
            {
                if (CheckSliderQuestionValues(pSliderQuestion) == ErrorCode.SUCCESS)
                {
                    return mSliderQuestionRepository.Update(pSliderQuestion);
                }
                else
                {
                    return Generic.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }
        public ErrorCode UpdateStarQuestion(StarQuestion pStarQuestion)
        {
            try
            {
                if (CheckStarQuestionValues(pStarQuestion) == ErrorCode.SUCCESS)
                {
                    return mStarQuestionRepository.Update(pStarQuestion);
                }
                else
                {
                    return Generic.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }

        #endregion

        #region Get Question Functions
        public ErrorCode GetSmileyQuestionByID(ref SmileyQuestion pSmileyQuestion)
        {
            try
            {
                if (pSmileyQuestion.ID > 0)
                {
                    return mSmileyQuestionRepository.Get(ref pSmileyQuestion);
                }
                else
                {
                    return Generic.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }
        public ErrorCode GetSliderQuestionByID(ref SliderQuestion pSliderQuestion)
        {
            try
            {
                if (pSliderQuestion.ID > 0)
                {
                    return mSliderQuestionRepository.Get(ref pSliderQuestion);
                }
                else
                {
                    return Generic.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }
        public ErrorCode GetStarQuestionByID(ref StarQuestion pStarQuestion)
        {
            try
            {
                if (pStarQuestion.ID > 0)
                {
                    return mStarQuestionRepository.Get(ref pStarQuestion);
                }
                else
                {
                    return Generic.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }
        public ErrorCode GetAllQuestions(ref List<Question> questionsList)
        {
            try
            {
                return mRepository.GetAll(ref questionsList);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }

        #endregion

        #region Delete Question Functions
        public ErrorCode DeleteQuestionByID(int questionId)
        {
            try
            {
                Question question = new Question(questionId);
                if (questionId > 0)
                {
                    return mRepository.Delete(questionId);
                }
                else
                {
                    return Generic.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }

        #endregion


        #region Validation Functions

        public ErrorCode CheckSmileyQuestionValues(SmileyQuestion pSmileyQuestion)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(pSmileyQuestion.Text) && pSmileyQuestion.Text.Length < 4000) //if Question text is not null or empty 
                    if (pSmileyQuestion.NumberOfSmileyFaces >= 2 && pSmileyQuestion.NumberOfSmileyFaces <= 5)
                        return ErrorCode.SUCCESS;

                return ErrorCode.ERROR;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        } // end func.

        public ErrorCode CheckSliderQuestionValues(SliderQuestion pSliderQuestion)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(pSliderQuestion.Text) && pSliderQuestion.Text.Length < 4000) //if Question text is not null or empty 
                    if (!String.IsNullOrWhiteSpace(pSliderQuestion.StartValueCaption) && pSliderQuestion.StartValueCaption.Length < 100)
                        if (!String.IsNullOrWhiteSpace(pSliderQuestion.EndValueCaption) && pSliderQuestion.EndValueCaption.Length < 100)
                            if (pSliderQuestion.StartValue < pSliderQuestion.EndValue)
                                return ErrorCode.SUCCESS;

                return ErrorCode.ERROR;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        } // end func.

        public ErrorCode CheckStarQuestionValues(StarQuestion pStarQuestion)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(pStarQuestion.Text) && pStarQuestion.Text.Length < 4000) //if Question text is not null or empty 
                    if (pStarQuestion.NumberOfStars >= 1 && pStarQuestion.NumberOfStars <= 10)
                        return ErrorCode.SUCCESS;

                return ErrorCode.ERROR;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        } // end func.

        #endregion
    }
}
