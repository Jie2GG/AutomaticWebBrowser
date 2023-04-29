using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Domain.Configuration.Models;
using AutomaticWebBrowser.Services;
using AutomaticWebBrowser.Views;

using HandyControl.Controls;
using HandyControl.Tools.Extension;

using Prism.Commands;
using Prism.Mvvm;

using Unity;

namespace AutomaticWebBrowser.ViewModels
{
    /// <summary>
    /// MainView.xaml 的业务逻辑
    /// </summary>
    class MainViewModel : BindableBase
    {
        #region --字段--
        private string title = "自动化浏览器";
        private ObservableCollection<WebTabItem> browserTabs = new ();
        private int browserTabIndex = 0;
        #endregion

        #region --属性--
        /// <summary>
        /// 配置
        /// </summary>
        [Dependency (nameof (AWConfig))]
        public AWConfig? Config { get; set; }

        /// <summary>
        /// IOC容器
        /// </summary>
        [Dependency]
        public UnityContainer? Container { get; set; }

        /// <summary>
        /// 日志服务
        /// </summary>
        [Dependency]
        public LoggerService? LoggerService { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get => this.title;
            set => this.SetProperty (ref this.title, value);
        }

        /// <summary>
        /// 浏览器标签页
        /// </summary>
        public ObservableCollection<WebTabItem> BrowserTabs
        {
            get => this.browserTabs;
            set => this.SetProperty (ref this.browserTabs, value);
        }

        /// <summary>
        /// 标签页索引
        /// </summary>
        public int BrowserTabIndex
        {
            get => this.browserTabIndex;
            set => this.SetProperty (ref this.browserTabIndex, value);
        }
        #endregion

        #region --命令--
        public ICommand InitializeCommand => new DelegateCommand<HandyControl.Controls.Window> (async (window) =>
        {
            if (this.Config != null && this.LoggerService?.Logger != null)
            {
                this.LoggerService?.Logger.Information ($"初始化 --> 跟踪程序启动完毕");
                // 设置窗体属性
                window.WindowState = this.Config.Browser.Window.State;
                this.LoggerService?.Logger.Information ($"初始化 --> 窗体状态: {window.WindowState}");
                window.WindowStartupLocation = this.Config.Browser.Window.StartupLocation;
                this.LoggerService?.Logger.Information ($"初始化 --> 窗体启动位置: {window.WindowStartupLocation}");
                window.Width = this.Config.Browser.Window.Width;
                window.Height = this.Config.Browser.Window.Height;
                if (this.Config.Browser.Window.Left is not null)
                {
                    window.Left = (double)this.Config.Browser.Window.Left;
                }
                if (this.Config.Browser.Window.Top is not null)
                {
                    window.Top = (double)this.Config.Browser.Window.Top;
                }
                this.LoggerService?.Logger.Information ($"初始化 --> 窗体大小: [{window.Width}, {window.Height}]");

                await this.StartTask (window.Dispatcher);
            }
        });


        public static ICommand ShowLog => new DelegateCommand (() =>
        {
            new LogView ().Show ();
        });
        #endregion

        #region --私有方法--
        private async Task StartTask (Dispatcher dispatcher)
        {
            this.LoggerService!.Logger!.Information ($"自动化任务 --> 加载任务清单, 数量: {this.Config!.Tasks.Length}");

            foreach (AWTask task in this.Config.Tasks)
            {
                // 创建标签页
                WebTabItem webTabItem = dispatcher.Invoke<WebTabItem> (() => new (this.Config!, this.LoggerService!.Logger!));
                this.BrowserTabs.Add (webTabItem);

                // 初始化浏览器
                await webTabItem.InitializeWebView ();

                // 投递任务
                await webTabItem.RunTask (task);

                // 关闭标签页
                if (task.AutoClose)
                {
                    dispatcher.Invoke (() => this.BrowserTabs.Remove (webTabItem));
                }

                await Task.Delay (1000);
            }

            this.LoggerService!.Logger!.Information ($"自动化任务 --> 任务执行完毕");
        }
        #endregion


    }
}