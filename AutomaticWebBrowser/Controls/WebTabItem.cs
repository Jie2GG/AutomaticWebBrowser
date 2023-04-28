using System;
using System.IO;
using System.Threading.Tasks;

using AutomaticWebBrowser.Commons;
using AutomaticWebBrowser.Domain.Configuration.Models;
using AutomaticWebBrowser.Domain.Log;
using AutomaticWebBrowser.Domain.Tasks.Commands.ActionCommands;
using AutomaticWebBrowser.Domain.Tasks.Commands.ElementCommands;

using HandyControl.Controls;

using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

using Serilog.Core;

namespace AutomaticWebBrowser.Controls
{
    class WebTabItem : TabItem, IWebView
    {
        #region --字段--
        private readonly ConditionSynchronous conditionSynchronous;
        #endregion

        #region --属性--
        public AWConfig Config { get; }

        public Logger Log { get; }

        public WebView2 WebView { get; }

        AsyncWaitHostScript IWebView.WaitHostScript => new ();
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="WebTabItem"/> 类的新实例
        /// </summary>
        /// <param name="config">配置项</param>
        /// <param name="log">日志框架</param>
        public WebTabItem (AWConfig config, Logger log)
        {
            this.Header = "新标签页";
            this.Config = config;
            this.Log = log;

            // 创建条件同步器
            this.conditionSynchronous = new ConditionSynchronous ();

            // 创建 WebView2
            this.Content = this.WebView = new WebView2 ();
        }

        /// <summary>
        /// 释放 <see cref="WebTabItem"/> 类的实例
        /// </summary>
        ~WebTabItem ()
        {
            this.WebView.Dispose ();
        }
        #endregion

        #region --公开方法--
        public async Task InitializeWebView ()
        {
            CoreWebView2EnvironmentOptions environmentOptions = new ()
            {
                EnableTrackingPrevention = this.Config.Browser.EnableTrackingPrevention
            };
            this.Log.Information ($"浏览器 --> 启用跟踪防护功能: {environmentOptions.EnableTrackingPrevention}");

            CoreWebView2Environment environment = await CoreWebView2Environment.CreateAsync (userDataFolder: Path.GetFullPath (this.Config.Browser.DataPath), options: environmentOptions);
            this.Log.Information ($"浏览器 --> 数据路径: {environment.UserDataFolder}");

            await this.WebView.EnsureCoreWebView2Async (environment);
            if (this.WebView.CoreWebView2 != null)
            {
                this.Log.Information ($"浏览器 --> 浏览器初始化完毕, PID: {this.WebView.CoreWebView2.BrowserProcessId}");

                // 显示状态条
                this.WebView.CoreWebView2.Settings.IsStatusBarEnabled = true;

                this.WebView.CoreWebView2.Settings.IsScriptEnabled = this.Config.Browser.EnableJavaScript;
                this.Log.Information ($"浏览器 --> 启用 JavaScript 代码: {this.WebView.CoreWebView2.Settings.IsScriptEnabled}");

                this.WebView.CoreWebView2.Settings.IsPasswordAutosaveEnabled = this.Config.Browser.EnablePasswordAutosave;
                this.Log.Information ($"浏览器 --> 启用密码自动保存: {this.WebView.CoreWebView2.Settings.IsPasswordAutosaveEnabled}");

                this.WebView.CoreWebView2.Settings.AreDevToolsEnabled = this.Config.Browser.EnableDevTools;
                this.Log.Information ($"浏览器 --> 启用开发者工具: {this.WebView.CoreWebView2.Settings.AreDevToolsEnabled}");

                this.WebView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = this.Config.Browser.EnableContextMenu;
                this.Log.Information ($"浏览器 --> 启用上下文菜单: {this.WebView.CoreWebView2.Settings.AreDefaultContextMenusEnabled}");

                // 绑定事件
                this.WebView.CoreWebView2.NavigationStarting += this.CoreWebView2NavigationStartingEventHandler;
                this.WebView.CoreWebView2.NavigationCompleted += this.CoreWebView2NavigationCompletedEventHandler;
                this.WebView.CoreWebView2.FaviconChanged += this.CoreWebView2FaviconChangedEventHandler;
                this.WebView.CoreWebView2.DocumentTitleChanged += this.CoreWebView2DocumentTitleChangedEventHandler;
                this.WebView.CoreWebView2.NewWindowRequested += this.CoreWebView2NewWindowRequestedEventHandler;
            }
        }

        public Task RunTask (AWTask task)
        {
            return Task.Factory.StartNew (() =>
            {
                #region 任务作业预处理
                for (int i = 0; i < task.Jobs.Length; i++)
                {
                    if (task.Jobs[i].Name is null)
                    {
                        task.Jobs[i].Name = $"Job{i + 1}";
                    }
                }
                this.Log.Information ($"自动化任务 --> 处理作业列表, 数量: {task.Jobs.Length}");
                #endregion

                // 导航到指定 URL
                ((IWebView)this).SafeNavigate (task.Url);

                // 开始执行作业
                foreach (AWJob job in task.Jobs)
                {
                    // 处理作业执行条件
                    if (job.Condition != null)
                    {
                        this.conditionSynchronous.Reset (job.Condition);
                        this.Log.Information ($"自动化任务 --> {job.Name} 等待同步条件 {job.Condition.Type}");
                        this.conditionSynchronous.WaitOneEx ();
                    }

                    // 处理作业执行元素
                    if (job.Element != null)
                    {
                        ElementCommand elementCommand = ElementCommandDispatcher.Dispatcher (this, this.Log, job.Element);
                        if (elementCommand.Execute ())
                        {
                            RunAction (elementCommand.Result, job.Actions);
                        }
                        else
                        {
                            // TODO: 任务中断
                        }
                    }
                    else
                    {
                        RunAction (null, job.Actions);
                    }

                }
            });

            void RunAction (string? variableName, AWAction[] actions)
            {
                foreach (AWAction action in actions)
                {
                    ActionCommand actionCommand = ActionCommandDispatcher.Dispatcher (this, this.Log, action, variableName);
                    if (!actionCommand.Execute ())
                    {
                        // TODO: 任务中断
                        break;
                    }
                }
            }
        }
        #endregion

        #region --私有方法--
        void IWebView.SafeNavigate (string url)
        {
            this.WebView.Dispatcher.Invoke (() => this.WebView.CoreWebView2?.Navigate (url));
        }

        async Task<string> IWebView.SafeExecuteScriptAsync (string javaScript)
        {
            return await this.WebView.Dispatcher.Invoke (() => this.WebView.CoreWebView2.ExecuteScriptAsync (javaScript));
        }
        #endregion

        #region --事件处理--
        // 文档图标改变
        private void CoreWebView2FaviconChangedEventHandler (object? sender, object e)
        {

        }

        // 文档标题改变
        private void CoreWebView2DocumentTitleChangedEventHandler (object? sender, object e)
        {
            this.Header = this.WebView.CoreWebView2.DocumentTitle;
        }

        // 导航开始
        private void CoreWebView2NavigationStartingEventHandler (object? sender, CoreWebView2NavigationStartingEventArgs e)
        {
            this.Log.Information ($"浏览器 --> 导航开始: {e.Uri}");

            // 注册 JavaSciprt 回调对象
            this.WebView.CoreWebView2.AddHostObjectToScript ("log", new LoggerHostScript (this.Log));
            this.Log.Information ($"浏览器 --> 注入 log 宿主对象");

            // 注册 JavaScript 异步等待对象
            this.WebView.CoreWebView2.AddHostObjectToScript ("wait", ((IWebView)this).WaitHostScript);
            this.Log.Information ($"浏览器 --> 注入 wait 宿主对象");
        }

        // 浏览器导航完成
        private void CoreWebView2NavigationCompletedEventHandler (object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                // 同步线程
                this.conditionSynchronous.Set (new AWCondition () { Type = AWConditionType.Ready });
                this.Log.Information ($"浏览器 --> 导航成功");
            }
            else
            {
                this.Log.Warning ($"浏览器 --> 导航失败, 原因: {e.WebErrorStatus}(CODE: {e.HttpStatusCode})");
            }
        }

        // 新窗体请求
        private void CoreWebView2NewWindowRequestedEventHandler (object? sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            CoreWebView2Deferral coreWebView2Deferral = e.GetDeferral ();

            // 处理新标签创建

            // 赋值给 e.WebView2

            coreWebView2Deferral.Complete ();
        }
        #endregion
    }
}
