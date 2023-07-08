using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

using AutomaticWebBrowser.Services.Logger;
using AutomaticWebBrowser.Services.Logger.Models;

using Prism.Commands;
using Prism.Mvvm;

using Unity;

namespace AutomaticWebBrowser.ViewModels
{
    class LogViewModel : BindableBase, IObserver<Log>
    {
        #region --字段--
        private ObservableCollection<Log> logs;
        #endregion

        #region --属性--
        public ObservableCollection<Log> Logs
        {
            get => this.logs;
            set => this.SetProperty (ref this.logs, value);
        }

        public LoggerSubscriber LoggerSubscriber { get; }
        #endregion

        #region --命令--
        /// <summary>
        /// 初始化命令
        /// </summary>
        public ICommand InitializeCommand => new DelegateCommand (() =>
        {
            this.LoggerSubscriber.Subscribe (this);
        });

        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="LogViewModel"/> 类的新实例
        /// </summary>
        /// <param name="loggerSubscriber"></param>
        public LogViewModel ([Dependency] LoggerSubscriber loggerSubscriber)
        {
            this.logs = new ObservableCollection<Log> ();

            this.LoggerSubscriber = loggerSubscriber;
        }
        #endregion

        #region --私有方法--
        void IObserver<Log>.OnCompleted ()
        { }

        void IObserver<Log>.OnNext (Log value)
        {
            while (this.logs.Count > 1000)
            {
                this.logs.RemoveAt (0);
            }
            Application.Current.Dispatcher.Invoke (() => this.logs.Add (value));
        }

        void IObserver<Log>.OnError (Exception error)
        { }
        #endregion

    }
}
