using System;
using System.Collections.Generic;
using System.IO;

using AutomaticWebBrowser.Wpf.Core;
using AutomaticWebBrowser.Wpf.Services.Logger.Models;

using Serilog.Events;
using Serilog.Formatting;

namespace AutomaticWebBrowser.Wpf.Services.Logger
{
    class LoggerSubscriber : ILog, IObservable<Log>
    {
        #region --字段--
        private readonly List<IObserver<Log>> observers;
        private readonly List<Log> caches;
        #endregion

        #region --属性--
        public ITextFormatter? TextFormatter { get; set; }
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="LoggerSubscriber"/> 类的新实例
        /// </summary>
        public LoggerSubscriber ()
        {
            this.observers = new List<IObserver<Log>> ();
            this.caches = new List<Log> ();
        }
        #endregion

        #region --公开方法--
        public void Emit (LogEvent logEvent)
        {
            using StringWriter stringWriter = new ();
            this.TextFormatter!.Format (logEvent, stringWriter);
            string text = stringWriter.ToString ().Trim ();

            Log log = new ()
            {
                Level = logEvent.Level,
                Message = text
            };

            while (this.caches.Count > 1000)
            {
                this.caches.RemoveAt (0);
            }
            this.caches.Add (log);

            foreach (IObserver<Log> observer in this.observers)
            {
                observer.OnCompleted ();
                observer.OnNext (log);
            }
        }

        public IDisposable Subscribe (IObserver<Log> observer)
        {
            if (observer is null)
            {
                throw new ArgumentNullException (nameof (observer));
            }

            if (this.observers.Contains (observer) == false)
            {
                this.observers.Add (observer);
            }

            // 推送缓存中的日志, 达到日志同步效果
            for (int i = 0; i < this.caches.Count; i++)
            {
                observer.OnCompleted ();
                observer.OnNext (this.caches[i]);
            }

            return new Unsubscriber (this.observers, observer);
        }
        #endregion

        #region --内部类--
        private class Unsubscriber : IDisposable
        {
            #region --字段--
            private readonly List<IObserver<Log>> observers;
            private readonly IObserver<Log> observer;
            #endregion

            #region --构造函数--
            public Unsubscriber (List<IObserver<Log>> observers, IObserver<Log> observer)
            {
                this.observers = observers;
                this.observer = observer;
            }
            #endregion

            #region --公开方法--
            public void Dispose ()
            {
                if (this.observer != null && this.observers.Contains (this.observer))
                {
                    this.observers.Remove (this.observer);
                }
            }
            #endregion
        }
        #endregion
    }
}
