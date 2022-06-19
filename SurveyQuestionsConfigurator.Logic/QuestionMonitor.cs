//using SurveyQuestionsConfigurator.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SurveyQuestionsConfigurator.QuestionLogic
//{
//    public class QuestionMonitor : IObservable<Question>
//    {
//        List<IObserver<Question>> mObservers;
//        //List<Question> mCachedList = new List<Question>();
//        public QuestionMonitor()
//        {
//            mObservers = new List<IObserver<Question>>();
//        }
//        private class Unsubscriber : IDisposable
//        {
//            private List<IObserver<Question>> tObservers;
//            private IObserver<Question> tObserver;

//            public Unsubscriber(List<IObserver<Question>> pObservers, IObserver<Question> pObserver)
//            {
//                this.tObservers = pObservers;
//                this.tObserver = pObserver;
//            }

//            public void Dispose()
//            {
//                if (!(tObserver == null)) tObservers.Remove(tObserver);
//            }
//        }
//        public IDisposable Subscribe(IObserver<Question> pObserver)
//        {
//            if (!mObservers.Contains(pObserver))
//                mObservers.Add(pObserver);

//            return new Unsubscriber(mObservers, pObserver);
//        }

//        //public void Refresh(List<Question> pQuestion)
//        /*
//         *         public void Refresh(Question pQuestion)
//        {
//            foreach (var tObserver in mObservers)
//            {
//                if (!mCachedList.Contains(pQuestion))
//                {
//                    tObserver.OnNext(pQuestion);
//                }
//                //tObserver.OnError(new ArgumentNullException());
//            }
//        }
//         */
//        public void Refresh(List<Question> pQuestionList, List<Question> pChachedList)
//        {
//            foreach (var tObserver in mObservers)
//            {
//                if (pQuestionList.Count == 0)
//                {
//                    tObserver.OnError(new ArgumentNullException());
//                    tObserver.OnNext(new Question(-1));
//                }
//                tObserver.OnNext(pQuestionList[0]);

//            }
//        }
//    }
//}
