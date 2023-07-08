using System;
using System.IO;
using System.Text.Json;
using System.Windows;

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
        private AWConfig? config;
        private LoggerSubscriber? loggerSubscriber;
        private ILogger? logger;
        #endregion

        #region --公开方法--
        protected override void OnStartup (StartupEventArgs e)
        {
            BootParameters? parameters = CommandLineParser.Parse<BootParameters> (e.Args);
            if (parameters is not null)
            {
                this.InitializeConfig (parameters.ConfigPath);
                this.InitializeLogger ();
            }

            base.OnStartup (e);
        }

        protected override void RegisterTypes (IContainerRegistry containerRegistry)
        {
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

        protected override void InitializeShell (Window shell)
        {
            if (this.config is not null && this.logger is not null)
            {
                shell.WindowState = this.config.Browser.Window.State;
                this.logger.Information ($"初始化 --> 窗体状态: {shell.WindowState}");

                shell.WindowStartupLocation = this.config.Browser.Window.StartupLocation;
                this.logger.Information ($"初始化 --> 窗体启动位置: {shell.WindowStartupLocation}");

                shell.Width = this.config.Browser.Window.Width;
                shell.Height = this.config.Browser.Window.Height;
                this.logger.Information ($"初始化 --> 窗体大小: ({shell.Width},{shell.Height})");

                shell.Left = (double)this.config.Browser.Window.Left;
                shell.Top = (double)this.config.Browser.Window.Top;
                this.logger.Information ($"初始化 --> 窗体位置: ({shell.Left},{shell.Top})");
            }

            base.InitializeShell (shell);
        }
        #endregion

        #region --私有方法--
        /// <summary>
        /// 初始化配置文件
        /// </summary>
        /// <param name="configFileInfo"></param>
        private void InitializeConfig (string configPath)
        {
            FileInfo configFileInfo = new (Path.GetFullPath (configPath));
            if (configFileInfo.Exists)
            {
                try
                {
                    this.config = JsonSerializer.Deserialize<AWConfig> (configFileInfo.OpenRead (), Global.DefaultJsonSerializerOptions);

                    if (this.config is null)
                    {
                        MessageBox.Show ($"配置文件 ({configFileInfo.Name}) 加载失败. {Environment.NewLine} 原因: 未知错误");
                        this.Shutdown ();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show ($"配置文件 ({configFileInfo.Name}) 加载失败. {Environment.NewLine} 原因: {ex.Message}", $"提示", MessageBoxButton.OK, MessageBoxImage.Error);
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
