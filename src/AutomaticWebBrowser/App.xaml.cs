using System;
using System.IO;
using System.Text.Json;
using System.Windows;

using AutomaticWebBrowser.Services.Configuration;
using AutomaticWebBrowser.Services.Configuration.Models;
using AutomaticWebBrowser.Services.Logger;
using AutomaticWebBrowser.Services.Startup;
using AutomaticWebBrowser.Views;

using Ookii.CommandLine;

using Prism.Ioc;
using Prism.Unity;

using Serilog;
using Serilog.Events;

using MessageBox = HandyControl.Controls.MessageBox;

namespace AutomaticWebBrowser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        #region --字段--
        private BootParameters? parameters;
        private AWConfig? config;
        private LoggerSubscriber? loggerSubscriber;
        private ILogger? logger;
        #endregion

        #region --私有方法--
        protected override void OnStartup (StartupEventArgs e)
        {
            this.parameters = CommandLineParser.Parse<BootParameters> (e.Args);
            if (parameters is not null)
            {
                this.InitializeConfig ();
                this.InitializeLogger ();
            }

            base.OnStartup (e);
        }

        protected override void RegisterTypes (IContainerRegistry containerRegistry)
        {
            if (this.parameters is not null)
            {
                containerRegistry.RegisterInstance (this.parameters);
            }
            if (this.config is not null)
            {
                containerRegistry.RegisterInstance (this.config);
            }
            if (this.loggerSubscriber is not null)
            {
                containerRegistry.RegisterInstance (this.loggerSubscriber);
            }
            if (this.logger is not null)
            {
                containerRegistry.RegisterInstance (this.logger);
            }

            containerRegistry.RegisterSingleton<LogView> ();
            containerRegistry.Register<SettingView> ();
        }

        protected override Window CreateShell ()
        {
            return new MainView ();
        }

        /// <summary>
        /// 初始化配置文件
        /// </summary>
        /// <param name="configFileInfo"></param>
        private void InitializeConfig ()
        {
            if (this.parameters is not null)
            {
                try
                {
                    FileInfo configFileInfo = new (Path.GetFullPath (Path.Combine (this.parameters.ConfigDirectory, this.parameters.ConfigName)));
                    this.config = JsonConfiguration.Load (configFileInfo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show (ex.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Shutdown ();
                }
            }
        }

        /// <summary>
        /// 初始化日志框架
        /// </summary>
        private void InitializeLogger ()
        {
            if (this.config is not null)
            {
                string logPath = Path.Combine (Path.GetFullPath (this.config.Log.SavePath), $"{DateTime.Now:yyyyMMdd}.log");

                this.loggerSubscriber = new LoggerSubscriber ();

                this.logger = new LoggerConfiguration ()
#if DEBUG
                    .WriteTo.Debug (LogEventLevel.Debug, this.config.Log.Format)
#else
                    .WriteTo.Trace (this.config.Log.Level, this.config.Log.Format)
#endif
                    .WriteTo.File (logPath, this.config.Log.Level, this.config.Log.Format)
                    .WriteTo.Subscriber (this.loggerSubscriber, this.config.Log.Level, this.config.Log.Format)
                    .CreateLogger ();

                this.logger.Information ($"初始化 --> 跟踪程序启动, 等级: {this.config.Log.Level}");
            }
        }
        #endregion
    }
}
