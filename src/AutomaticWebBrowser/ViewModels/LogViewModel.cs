using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

using AutomaticWebBrowser.Models;
using AutomaticWebBrowser.Services;

using HandyControl.Controls;

using Prism.Commands;
using Prism.Mvvm;

using Unity;

namespace AutomaticWebBrowser.ViewModels
{
    class LogViewModel : BindableBase, IObserver<LogItem>
    {
        #region --字段--
        private IDisposable? unsubscriber;
        private ObservableCollection<LogItem> logs = new ();
        #endregion

        #region --属性--
        /// <summary>
        /// 日志服务
        /// </summary>
        [Dependency]
        public LoggerService? LoggerService { get; set; }

        /// <summary>
        /// 日志项
        /// </summary>
        public ObservableCollection<LogItem> Logs
        {
            get => this.logs;
            set => this.SetProperty (ref this.logs, value);
        }
        #endregion

        #region --命令--
        public ICommand InitializeCommand => new DelegateCommand<HandyControl.Controls.Window> ((window) =>
        {
            // 订阅日志回调
            this.unsubscriber = this.LoggerService?.Subscribe (this);
        });

        public ICommand UninitializeCommand => new DelegateCommand (() =>
        {
            this.unsubscriber?.Dispose ();
        });
        #endregion

        #region --私有方法--
        void IObserver<LogItem>.OnCompleted ()
        {
            Application.Current.Dispatcher.Invoke (() =>
            {
                while (this.Logs.Count > 1000)
                {
                    this.Logs.RemoveAt (0);
                }
            });
        }

        void IObserver<LogItem>.OnNext (LogItem value)
        {
            Application.Current.Dispatcher.Invoke (() =>
            {
                this.Logs.Add (value);
            });
        }

        void IObserver<LogItem>.OnError (Exception error)
        {

        }
        #endregion
    }
}
