using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using AutomaticWebBrowser.Domain.Configuration.Models;
using AutomaticWebBrowser.Domain.Log;
using AutomaticWebBrowser.Models;

using Prism.Ioc;
using Prism.Modularity;

using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

using Unity;

namespace AutomaticWebBrowser.Services
{
    /// <summary>
    /// 日志服务
    /// </summary>
    class LoggerService : ILoggerFormatter, IObservable<LogItem>
    {
        #region --字段--
        private readonly List<IObserver<LogItem>> observers;
        private readonly List<LogItem> logsCache;
        private readonly Logger? logger;
        #endregion

        #region --属性--
        /// <summary>
        /// 日志接口
        /// </summary>
        public Logger? Logger { get => this.logger; }

        ITextFormatter? ILoggerFormatter.TextFormatter { get; set; }
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="LoggerService"/> 类的新实例
        /// </summary>
        public LoggerService ([Dependency (nameof (AWConfig))] AWConfig config)
        {
            // 观察者列表
            this.observers = new List<IObserver<LogItem>> ();

            // 日志缓存
            this.logsCache = new List<LogItem> ();

            // 配置日志框架
            string logFilePath = Path.Combine (Path.GetFullPath (config.Log.SavePath), $"{DateTime.Now:yyyyMMdd}.log");
            this.logger = new LoggerConfiguration ()
#if DEBUG
                .WriteTo.Debug (LogEventLevel.Debug, config.Log.Format)
#else
                .WriteTo.Trace (config.Log.Level, config.Log.Format)
#endif
                .WriteTo.File (logFilePath, config.Log.Level, config.Log.Format)
                .WriteTo.View (this, config.Log.Level, config.Log.Format)
                .CreateLogger ();
        }
        #endregion

        #region --公开方法--
        public IDisposable Subscribe (IObserver<LogItem> observer)
        {
            if (!this.observers.Contains (observer))
            {
                this.observers.Add (observer);
            }

            for (int i = 0; i < this.logsCache.Count; i++)
            {
                observer.OnCompleted ();
                observer.OnNext (logsCache[i]);
            }

            return new Unsubscriber (observers, observer);
        }
        #endregion

        #region --私有方法--
        void ILogEventSink.Emit (LogEvent logEvent)
        {
            using StringWriter stringWriter = new ();
            ((ILoggerFormatter)this).TextFormatter!.Format (logEvent, stringWriter);
            string text = stringWriter.ToString ().Trim ();

            LogItem logItem = logEvent.Level switch
            {
                LogEventLevel.Verbose => Create (text, Brushes.Black, Brushes.White),
                LogEventLevel.Debug => Create (text, Brushes.Green, Brushes.White),
                LogEventLevel.Information => Create (text, Brushes.Blue, Brushes.White),
                LogEventLevel.Warning => Create (text, Brushes.OrangeRed, Brushes.White),
                LogEventLevel.Error => Create (text, Brushes.Red, Brushes.White),
                LogEventLevel.Fatal => Create (text, Brushes.Red, Brushes.Black),
                _ => Create (text, Brushes.Blue, Brushes.White),
            };

            while (this.logsCache.Count > 1000)
            {
                this.logsCache.RemoveAt (0);
            }
            this.logsCache.Add (logItem);

            foreach (IObserver<LogItem> observer in this.observers)
            {
                observer.OnCompleted ();
                observer.OnNext (logItem);
            }

            static LogItem Create (string text, Brush foregroud, Brush background)
            {
                return new LogItem ()
                {
                    Text = text,
                    Foreground = foregroud,
                    Background = background
                };
            }
        }
        #endregion

        #region --内部类--
        /// <summary>
        /// 取消订阅内部类
        /// </summary>
        private class Unsubscriber : IDisposable
        {
            #region --字段--
            private readonly List<IObserver<LogItem>> observers;
            private readonly IObserver<LogItem> observer;
            #endregion

            #region --构造函数--
            public Unsubscriber (List<IObserver<LogItem>> observers, IObserver<LogItem> observer)
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
