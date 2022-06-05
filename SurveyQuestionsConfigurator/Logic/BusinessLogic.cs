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
using System.Windows.Forms;

namespace SurveyQuestionsConfigurator.Logic
{
    public class BusinessLogic
    {
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
                    return (int)Types.Error.ERROR;
                }
            }
            catch (Exception ex)
            {
                Helper.Logger(ex);
                return (int)Types.Error.ERROR;
            }
        }

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
                    return (int)Types.Error.ERROR;
                }
            }
            catch (Exception ex)
            {
                Helper.Logger(ex);
                return (int)Types.Error.ERROR;
            }
        }

        public DataTable GetSmileyQuestionByID(int questionId)
        {
            try
            {
                SmileyQuestionRepository smileyQuestionRepository = new SmileyQuestionRepository();
                if (questionId > 0)
                {
                    return smileyQuestionRepository.Get(questionId);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Helper.Logger(ex);
                return null;
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
                    return (int)Types.Error.ERROR;
                }
            }
            catch (Exception ex)
            {
                Helper.Logger(ex);
                return (int)Types.Error.ERROR;
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
                    return (int)Types.Error.ERROR;
                }
            }
            catch (Exception ex)
            {
                Helper.Logger(ex);
                return (int)Types.Error.ERROR;
            }
        }

        public DataTable GetSliderQuestionByID(int questionId)
        {
            try
            {
                SliderQuestionRepository sliderQuestionRepository = new SliderQuestionRepository();
                if (questionId > 0)
                {
                    return sliderQuestionRepository.Get(questionId);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Helper.Logger(ex);
                return null;
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
                    return (int)Types.Error.ERROR;
                }
            }
            catch (Exception ex)
            {
                Helper.Logger(ex);
                return (int)Types.Error.ERROR;
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
                    return (int)Types.Error.ERROR;
                }
            }
            catch (Exception ex)
            {
                Helper.Logger(ex);
                return (int)Types.Error.ERROR;
            }
        }

        public DataTable GetStarQuestionByID(int questionId)
        {
            try
            {
                StarQuestionRepository starQuestionRepository = new StarQuestionRepository();
                if (questionId > 0)
                {
                    return starQuestionRepository.Get(questionId);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Helper.Logger(ex);
                return null;
            }
        }

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
                    return (int)Types.Error.ERROR;
                }
            }
            catch (Exception ex)
            {
                Helper.Logger(ex);
                return (int)Types.Error.ERROR;
            }
        }

        public DataTable GetAllQuestions()
        {
            try
            {
                GenericRepository repository = new GenericRepository();
                return repository.GetAll();
            }
            catch (Exception ex)
            {
                Helper.Logger(ex);
                return null;
            }
        }


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
                Helper.Logger(ex);
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
                Helper.Logger(ex);
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
                Helper.Logger(ex);
                return false;
            }
        } // end func.
    }
}
