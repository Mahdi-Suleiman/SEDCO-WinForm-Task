using SurveyQuestionsConfigurator.CommonLayer;
using SurveyQuestionsConfigurator.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SurveyQuestionsConfigurator
{
    public class BusinessLogic
    {
        public int AddSmileyQuestion(SmileyQuestion smileyQuestion)
        {
            try
            {
                SmileyQuestionRepository smileyQuestionRepository = new SmileyQuestionRepository();
                if (CheckSmileyQuestionInputFields(smileyQuestion))
                {
                    return smileyQuestionRepository.Add(smileyQuestion);
                }
                else
                {
                    return (int)CommonEnums.ErrorType.ERROR;
                }
            }
            catch (Exception ex)
            {
                CommonHelpers.Logger(ex);
                return (int)CommonEnums.ErrorType.ERROR;
            }
        }

        public int EditSmileyQuestion(SmileyQuestion smileyQuestion)
        {
            try
            {
                SmileyQuestionRepository smileyQuestionRepository = new SmileyQuestionRepository();
                if (CheckSmileyQuestionInputFields(smileyQuestion))
                {
                    return smileyQuestionRepository.Update(smileyQuestion);
                }
                else
                {
                    return (int)CommonEnums.ErrorType.ERROR;
                }
            }
            catch (Exception ex)
            {
                CommonHelpers.Logger(ex);
                return (int)CommonEnums.ErrorType.ERROR;
            }
        }

        public DataTable GetSingleSmileyQuestion(int questionId)
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
                CommonHelpers.Logger(ex);
                return null;
            }
        }

        public int AddSliderQuestion(SliderQuestion sliderQuestion)
        {
            try
            {
                SliderQuestionRepository sliderQuestionRepository = new SliderQuestionRepository();
                if (CheckSliderQuestionInputFields(sliderQuestion))
                {
                    return sliderQuestionRepository.Add(sliderQuestion);
                }
                else
                {
                    return (int)CommonEnums.ErrorType.ERROR;
                }
            }
            catch (Exception ex)
            {
                CommonHelpers.Logger(ex);
                return (int)CommonEnums.ErrorType.ERROR;
            }
        }

        public int EditSliderQuestion(SliderQuestion sliderQuestion)
        {
            try
            {
                SliderQuestionRepository sliderQuestionRepository = new SliderQuestionRepository();
                if (CheckSliderQuestionInputFields(sliderQuestion))
                {
                    return sliderQuestionRepository.Update(sliderQuestion);
                }
                else
                {
                    return (int)CommonEnums.ErrorType.ERROR;
                }
            }
            catch (Exception ex)
            {
                CommonHelpers.Logger(ex);
                return (int)CommonEnums.ErrorType.ERROR;
            }
        }

        public DataTable GetSingleSliderQuestion(int questionId)
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
                CommonHelpers.Logger(ex);
                return null;
            }
        }


        public int AddStarQuestion(StarQuestion starQuestion)
        {
            try
            {
                StarQuestionRepository starQuestionRepository = new StarQuestionRepository();
                if (CheckStarQuestionInputFields(starQuestion))
                {
                    return starQuestionRepository.Add(starQuestion);
                }
                else
                {
                    return (int)CommonEnums.ErrorType.ERROR;
                }
            }
            catch (Exception ex)
            {
                CommonHelpers.Logger(ex);
                return (int)CommonEnums.ErrorType.ERROR;
            }
        }

        public int EditStarQuestion(StarQuestion starQuestion)
        {
            try
            {
                StarQuestionRepository starQuestionRepository = new StarQuestionRepository();
                if (CheckStarQuestionInputFields(starQuestion))
                {
                    return starQuestionRepository.Update(starQuestion);
                }
                else
                {
                    return (int)CommonEnums.ErrorType.ERROR;
                }
            }
            catch (Exception ex)
            {
                CommonHelpers.Logger(ex);
                return (int)CommonEnums.ErrorType.ERROR;
            }
        }

        public DataTable GetSingleStarQuestion(int questionId)
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
                CommonHelpers.Logger(ex);
                return null;
            }
        }

        public int DeleteQuestion(int questionId)
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
                    return (int)CommonEnums.ErrorType.ERROR;
                }
            }
            catch (Exception ex)
            {
                CommonHelpers.Logger(ex);
                return (int)CommonEnums.ErrorType.ERROR;
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
                CommonHelpers.Logger(ex);
                return null;
            }
        }


        public bool CheckSmileyQuestionInputFields(SmileyQuestion smileyQuestion)
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
                //MessageBox.Show("Something wrong happened, please try again\n");
                CommonHelpers.Logger(ex);
                return false;
            }
        } // end func.

        public bool CheckSliderQuestionInputFields(SliderQuestion sliderQuestion)
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
                //MessageBox.Show("Something wrong happened, please try again\n");
                CommonHelpers.Logger(ex);
                return false;
            }
        } // end func.

        public bool CheckStarQuestionInputFields(StarQuestion starQuestion)
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
                //MessageBox.Show("Something wrong happened, please try again\n");
                CommonHelpers.Logger(ex);
                return false;
            }
        } // end func.
    }
}
