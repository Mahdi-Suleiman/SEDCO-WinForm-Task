using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.Entities;
using SurveyQuestionsConfigurator.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SurveyQuestionsConfigurator.Entities.Types;

namespace SurveyQuestionsConfigurator.QuestionLogic
{
    public class QuestionManager
    {
        SmileyQuestionRepository smileyQuestionRepository = new SmileyQuestionRepository();
        SliderQuestionRepository sliderQuestionRepository = new SliderQuestionRepository();
        StarQuestionRepository starQuestionRepository = new StarQuestionRepository();
        GenericRepository repository = new GenericRepository();

        #region Add Question Functions
        public ErrorCode AddSmileyQuestion(SmileyQuestion smileyQuestion)
        {
            try
            {
                if (CheckSmileyQuestionValues(smileyQuestion))
                {
                    return smileyQuestionRepository.Add(smileyQuestion);
                }
                else
                {
                    return Types.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Types.ErrorCode.ERROR;
            }
        }
        public ErrorCode AddSliderQuestion(SliderQuestion sliderQuestion)
        {
            try
            {
                if (CheckSliderQuestionValues(sliderQuestion))
                {
                    return sliderQuestionRepository.Add(sliderQuestion);
                }
                else
                {
                    return Types.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Types.ErrorCode.ERROR;
            }
        }
        public ErrorCode AddStarQuestion(StarQuestion starQuestion)
        {
            try
            {
                if (CheckStarQuestionValues(starQuestion))
                {
                    return starQuestionRepository.Add(starQuestion);
                }
                else
                {
                    return Types.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Types.ErrorCode.ERROR;
            }
        }

        #endregion

        #region Edit Question Functions
        public ErrorCode EditSmileyQuestion(SmileyQuestion smileyQuestion)
        {
            try
            {
                if (CheckSmileyQuestionValues(smileyQuestion))
                {
                    return smileyQuestionRepository.Update(smileyQuestion);
                }
                else
                {
                    return Types.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Types.ErrorCode.ERROR;
            }
        }
        public ErrorCode EditSliderQuestion(SliderQuestion sliderQuestion)
        {
            try
            {
                if (CheckSliderQuestionValues(sliderQuestion))
                {
                    return sliderQuestionRepository.Update(sliderQuestion);
                }
                else
                {
                    return Types.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Types.ErrorCode.ERROR;
            }
        }
        public ErrorCode EditStarQuestion(StarQuestion starQuestion)
        {
            try
            {
                if (CheckStarQuestionValues(starQuestion))
                {
                    return starQuestionRepository.Update(starQuestion);
                }
                else
                {
                    return Types.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Types.ErrorCode.ERROR;
            }
        }

        #endregion

        #region Get Question Functions
        public ErrorCode GetSmileyQuestionByID(ref SmileyQuestion smileyQuestion)
        {
            try
            {
                if (smileyQuestion.ID > 0)
                {
                    return smileyQuestionRepository.Get(ref smileyQuestion);
                }
                else
                {
                    return Types.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Types.ErrorCode.ERROR;
            }
        }
        public ErrorCode GetSliderQuestionByID(ref SliderQuestion sliderQuestion)
        {
            try
            {
                if (sliderQuestion.ID > 0)
                {
                    return sliderQuestionRepository.Get(ref sliderQuestion);
                }
                else
                {
                    return Types.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Types.ErrorCode.ERROR;
            }
        }
        public ErrorCode GetStarQuestionByID(ref StarQuestion starQuestion)
        {
            try
            {
                if (starQuestion.ID > 0)
                {
                    return starQuestionRepository.Get(ref starQuestion);
                }
                else
                {
                    return Types.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Types.ErrorCode.ERROR;
            }
        }
        public ErrorCode GetAllQuestions(ref List<Question> questionsList)
        {
            try
            {
                return repository.GetAll(ref questionsList);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Types.ErrorCode.ERROR;
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
                    return repository.Delete(questionId);
                }
                else
                {
                    return Types.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Types.ErrorCode.ERROR;
            }
        }

        #endregion


        #region Validation Functions

        public bool CheckSmileyQuestionValues(SmileyQuestion smileyQuestion)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(smileyQuestion.Text) && smileyQuestion.Text.Length < 4000) //if Question text is not null or empty 
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
                if (!String.IsNullOrWhiteSpace(sliderQuestion.Text) && sliderQuestion.Text.Length < 4000) //if Question text is not null or empty 
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
                if (!String.IsNullOrWhiteSpace(starQuestion.Text) && starQuestion.Text.Length < 4000) //if Question text is not null or empty 
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
