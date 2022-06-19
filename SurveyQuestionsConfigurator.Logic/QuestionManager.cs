using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.Entities;
using SurveyQuestionsConfigurator.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SurveyQuestionsConfigurator.Entities.Generic;

namespace SurveyQuestionsConfigurator.QuestionLogic
{
    public class QuestionManager : IObservable<Question>
    {

        #region Observable

        List<IObserver<Question>> mObservers;

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<Question>> tObservers;
            private IObserver<Question> tObserver;

            public Unsubscriber(List<IObserver<Question>> pObservers, IObserver<Question> pObserver)
            {
                this.tObservers = pObservers;
                this.tObserver = pObserver;
            }

            public void Dispose()
            {
                if (!(tObserver == null)) tObservers.Remove(tObserver);
            }
        }
        public IDisposable Subscribe(IObserver<Question> pObserver)
        {
            if (!mObservers.Contains(pObserver))
                mObservers.Add(pObserver);

            return new Unsubscriber(mObservers, pObserver);
        }

        public void Refresh(List<Question> pQuestionList, List<Question> pChachedList)
        {
            foreach (var tObserver in mObservers)
            {
                tObserver.OnNext(new Question(-1));
            }
        }
        #endregion

        private SmileyQuestionRepository mSmileyQuestionRepository;
        private SliderQuestionRepository mSliderQuestionRepository;
        private StarQuestionRepository mStarQuestionRepository;
        private GenericRepository mRepository;
        //private static QuestionMonitor mQuestionMonitor;
        private static List<Question> mCachedList;
        //private static ObservableCollection<Question> mCachedList;

        private void listChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            foreach (var tObserver in mObservers)
            {
                tObserver.OnNext(new Question(-1));
            }
        }


        public QuestionManager()
        {
            try
            {
                mSmileyQuestionRepository = new SmileyQuestionRepository();
                mSliderQuestionRepository = new SliderQuestionRepository();
                mStarQuestionRepository = new StarQuestionRepository();
                mRepository = new GenericRepository();
                //mQuestionMonitor = new QuestionMonitor();
                mCachedList = new List<Question>();
                mObservers = new List<IObserver<Question>>();

            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        #region Add Question Functions
        /// <summary>
        /// Check values and pass it to repository layer
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.SQL_VIOLATION
        /// </returns>
        public ErrorCode InsertSmileyQuestion(SmileyQuestion pSmileyQuestion)
        {
            try
            {
                if (CheckSmileyQuestionValues(pSmileyQuestion) == ErrorCode.SUCCESS)
                {
                    var returnValue = mSmileyQuestionRepository.Add(pSmileyQuestion);

                    mCachedList.Clear();
                    mCachedList.Add(pSmileyQuestion);

                    return returnValue;
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

        /// <summary>
        /// Check values and pass it to repository layer
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.SQL_VIOLATION
        /// </returns>
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

        /// <summary>
        /// Check values and pass it to repository layer
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.SQL_VIOLATION
        /// </returns>
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

        /// <summary>
        /// Check values and pass it to repository layer
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.SQL_VIOLATION
        /// </returns>
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

        /// <summary>
        /// Check values and pass it to repository layer
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.SQL_VIOLATION
        /// </returns>
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

        /// <summary>
        /// Check values and pass it to repository layer
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.SQL_VIOLATION
        /// </returns>
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

        /// <summary>
        /// Get a question from DB and check it values
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.SQL_VIOLATION
        /// </returns>
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

        /// <summary>
        /// Get a question from DB and check it values
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.SQL_VIOLATION
        /// </returns>
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

        /// <summary>
        /// Get a question from DB and check it values
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.SQL_VIOLATION
        /// </returns>
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

        /// <summary>
        /// Get all questions from DB and check it values
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.SQL_VIOLATION
        /// </returns>
        public ErrorCode GetAllQuestions(ref List<Question> pQuestionsList)
        {
            try
            {
                var returnValue = mRepository.GetAll(ref pQuestionsList);
                lock (mCachedList)
                {
                    //if (!pQuestionsList.SequenceEqual(mCachedList))
                    if (pQuestionsList.Count != mCachedList.Count)
                    {
                        //mCachedList.Clear();
                        //mCachedList.
                        //mCachedList.AddRange((ObservableCollection<Question>)pQuestionsList);
                        Refresh(pQuestionsList, mCachedList);
                    }
                    return returnValue;
                }
                //return mRepository.GetAll(ref pQuestionsList);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }

        #endregion

        #region Delete Question Functions

        /// <summary>
        /// Pass ID to the question of which to be deleted
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.SQL_VIOLATION
        /// </returns>
        public ErrorCode DeleteQuestionByID(int pQuestionId)
        {
            try
            {
                Question question = new Question(pQuestionId);
                if (pQuestionId > 0)
                {
                    return mRepository.Delete(pQuestionId);
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

        /// <summary>
        /// Check coommon question values and validate them
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// </returns>
        private ErrorCode CheckCommonQuestionInputFields(Question pQuestion)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(pQuestion.Text) && pQuestion.Text.Length < 4000) //if Question text is not null or empty 
                    return ErrorCode.SUCCESS;

                return ErrorCode.ERROR;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }

        /// <summary>
        /// Check smiley question values and validate them
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// </returns>
        public ErrorCode CheckSmileyQuestionValues(SmileyQuestion pSmileyQuestion)
        {
            try
            {
                if (CheckCommonQuestionInputFields(pSmileyQuestion) == ErrorCode.SUCCESS)
                    if (pSmileyQuestion.NumberOfSmileyFaces >= 2 && pSmileyQuestion.NumberOfSmileyFaces <= 5)
                        return ErrorCode.SUCCESS;

                return ErrorCode.ERROR;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        } /// Function end

        /// <summary>
        /// Check slider question values and validate them
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// </returns>
        public ErrorCode CheckSliderQuestionValues(SliderQuestion pSliderQuestion)
        {
            try
            {
                if (CheckCommonQuestionInputFields(pSliderQuestion) == ErrorCode.SUCCESS)
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
        } /// Function end

        /// <summary>
        /// Check star question values and validate them
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// </returns>
        public ErrorCode CheckStarQuestionValues(StarQuestion pStarQuestion)
        {
            try
            {
                if (CheckCommonQuestionInputFields(pStarQuestion) == ErrorCode.SUCCESS)
                    if (pStarQuestion.NumberOfStars >= 1 && pStarQuestion.NumberOfStars <= 10)
                        return ErrorCode.SUCCESS;

                return ErrorCode.ERROR;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        } /// Function end

        #endregion

    }

    class ChachedListClass
    {
        private static List<Question> mChachedList;

        public ChachedListClass()
        {
            mChachedList = new List<Question>();
        }
        public ChachedListClass(List<Question> pQuestionList)
        {
            mChachedList = new List<Question>();
            mChachedList.AddRange(pQuestionList);
        }

        public void Add(Question pQuestion)
        {
            mChachedList.Add(pQuestion);
            ThresholdReached?.Invoke(this, EventArgs.Empty);
        }

        public void AddRange(List<Question> pSourceList)
        {
            mChachedList.AddRange(pSourceList);
        }
        public static event EventHandler ThresholdReached;
    }
}
