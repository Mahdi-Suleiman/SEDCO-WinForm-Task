//using SurveyQuestionsConfigurator.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace SurveyQuestionsConfigurator.QuestionLogic
//{
//    internal class QuestionReporter : IObserver<Question>
//    {
//        private IDisposable mUnsubscriber;

//        public virtual void Subscribe(IObservable<Question> provider)
//        {
//            mUnsubscriber = provider.Subscribe(this);
//        }
//        public virtual void Unsubscribe()
//        {
//            mUnsubscriber.Dispose();
//        }

//        public virtual void OnCompleted()
//        {
//            //Console.WriteLine("Additional temperature data will not be transmitted.");
//            this.Unsubscribe();
//        }

//        public virtual void OnError(Exception error)
//        {
//            // Do nothing.
//        }

//        public virtual void OnNext(Question value)
//        {
//            ThreadPool.QueueUserWorkItem(BuildListView);

//            //Console.WriteLine("The temperature is {0}°C at {1:g}", value.Degrees, value.Date);
//            //if (first)
//            //{
//            //    last = value;
//            //    first = false;
//            //}
//            //else
//            //{
//            //    Console.WriteLine("   Change: {0}° in {1:g}", value.Degrees - last.Degrees,
//            //                                                  value.Date.ToUniversalTime() - last.Date.ToUniversalTime());
//            //}
//        }

//    }
//}
