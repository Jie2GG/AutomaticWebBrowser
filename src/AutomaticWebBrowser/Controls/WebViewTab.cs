using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using AutomaticWebBrowser.Common;
using AutomaticWebBrowser.Core;
using AutomaticWebBrowser.Services.Automatic.Commands.ActionCommands;
using AutomaticWebBrowser.Services.Automatic.Commands.ElementCommands;
using AutomaticWebBrowser.Services.Configuration.Models;
using AutomaticWebBrowser.Services.Logger;
using AutomaticWebBrowser.Services.Thread;

using HandyControl.Controls;

using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

using Serilog;

using static AutomaticWebBrowser.Core.IWebView;

namespace AutomaticWebBrowser.Controls
{
    /// <summary>
    /// Web 标签视图
    /// </summary>
    class WebViewTab : TabItem, IWebView
    {
        #region --字段--
        private readonly ILogger logger;
        private readonly AWBrowser browser;

        private readonly WebView2 webview2;
        private readonly AutoResetEvent webView2AsyncWait;
        private readonly AutoResetEvent webView2CreateNewAsyncWait;
        private readonly Collection<IWebView> subTabs;

        private readonly ConditionSynchronous conditionSynchronous;
        private readonly LoggerHostScript loggerHostScript;
        private readonly AsyncWaitHostScript asyncWaitHostScript;

        private AWTask? taskInfo;
        private bool isRunning;
        private CancellationTokenSource? taskTokenSource;
        #endregion

        #region --属性--
        /// <summary>
        /// Web 视图托管的 <see cref="Microsoft.Web.WebView2.Wpf.WebView2"/>
        /// </summary>
        public WebView2 WebView2 => this.webview2;

        /// <summary>
        /// 当前 Web 视图 <see cref="CreateNew"/> 事件异步等待服务
        /// </summary>
        public AutoResetEvent WebView2CreateNewAsyncWait => this.webView2CreateNewAsyncWait;

        /// <summary>
        /// 由当前 Web 视图打开的逻辑子标签页
        /// </summary>
        public Collection<IWebView> SubTabs => this.subTabs;

        /// <summary>
        /// 当前 Web 视图用于提供给 JavaScript 的异步等待服务
        /// </summary>
        public AsyncWaitHostScript AsyncWaitHostScript => this.asyncWaitHostScript;

        /// <summary>
        /// 获取当前 Web 视图的任务信息
        /// </summary>
        public AWTask TaskInfo => this.taskInfo!;

        /// <summary>
        /// 获取当前 Web 视图的任务取消令牌
        /// </summary>
        public CancellationTokenSource TaskTokenSource => this.taskTokenSource!;
        #endregion

        #region --事件--
        /// <summary>
        /// 创建新 Tab 事件
        /// </summary>
        public event EventHandler<NewWebViewEventArgs>? CreateNew;

        /// <summary>
        /// 销毁自身 Tab 事件
        /// </summary>
        public event EventHandler? DestroyItself;
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="WebViewTab"/> 类的新实例
        /// </summary>
        public WebViewTab (ILogger logger, AWBrowser browser)
        {
            this.logger = logger;
            this.browser = browser;

            // 挂接事件
            this.Closed += this.TabItemClosedEventHandler;

            // 创建浏览器所需的实例
            this.Content = this.webview2 = new WebView2 ();
            this.webView2AsyncWait = new AutoResetEvent (true);
            this.subTabs = new Collection<IWebView> ();

            // 创建自动化任务所需的实例
            this.conditionSynchronous = new ConditionSynchronous ();
            this.loggerHostScript = new LoggerHostScript (this.logger);
            this.asyncWaitHostScript = new AsyncWaitHostScript ();
            this.webView2CreateNewAsyncWait = new AutoResetEvent (true);
            this.isRunning = false;
        }

        #endregion

        #region --公开方法--
        /// <summary>
        /// 初始化当前 Web 视图
        /// </summary>
        /// <returns></returns>
        public async Task InitializeAsync ()
        {
            if (this.webview2.CoreWebView2 is null)
            {
                CoreWebView2EnvironmentOptions environmentOptions = new ()
                {
                    EnableTrackingPrevention = this.browser.EnableTrackingPrevention
                };
                this.logger.Information ($"浏览器 --> 跟踪防护: {environmentOptions.EnableTrackingPrevention}");

                CoreWebView2Environment environment = await CoreWebView2Environment.CreateAsync (userDataFolder: System.IO.Path.GetFullPath (this.browser.DataPath), options: environmentOptions);
                this.logger.Information ($"浏览器 --> 数据路径: {environment.UserDataFolder}");

                await this.webview2.EnsureCoreWebView2Async (environment);
                if (this.webview2.CoreWebView2 != null)
                {
                    // 显示状态条
                    this.webview2.CoreWebView2.Settings.IsStatusBarEnabled = true;
                    // 启用 Javascript
                    this.webview2.CoreWebView2.Settings.IsScriptEnabled = true;

                    this.webview2.CoreWebView2.Settings.IsPasswordAutosaveEnabled = this.browser.EnablePasswordAutosave;
                    this.logger.Information ($"浏览器 --> 启用密码自动保存: {this.webview2.CoreWebView2.Settings.IsPasswordAutosaveEnabled}");

                    this.webview2.CoreWebView2.Settings.AreDevToolsEnabled = this.browser.EnableDevTools;
                    this.logger.Information ($"浏览器 --> 启用开发者工具: {this.webview2.CoreWebView2.Settings.AreDevToolsEnabled}");

                    this.webview2.CoreWebView2.Settings.AreDefaultContextMenusEnabled = this.browser.EnableContextMenu;
                    this.logger.Information ($"浏览器 --> 启用上下文菜单: {this.webview2.CoreWebView2.Settings.AreDefaultContextMenusEnabled}");

                    // 绑定浏览器事件
                    this.webview2.CoreWebView2.NavigationStarting += this.CoreWebView2NavigationStartingEventHandler;
                    this.webview2.CoreWebView2.NavigationCompleted += this.CoreWebView2NavigationCompletedEventHandler;
                    this.webview2.CoreWebView2.FaviconChanged += this.CoreWebView2FaviconChangedEventHandler;
                    this.webview2.CoreWebView2.DocumentTitleChanged += this.CoreWebView2DocumentTitleChangedEventHandler;
                    this.webview2.CoreWebView2.NewWindowRequested += this.CoreWebView2NewWindowRequestedEventHandler;

                    this.logger.Information ($"浏览器 --> 启动完毕, PID: {this.webview2.CoreWebView2.BrowserProcessId}");
                }
            }
        }

        /// <summary>
        /// 导航到指定的地址
        /// </summary>
        /// <param name="url">目标网址</param>
        /// <returns></returns>
        public async Task NavigateAsync (string url)
        {
            await Task.Run (() =>
            {
                this.webView2AsyncWait.Reset ();
                this.webview2.Dispatcher.Invoke (() => this.webview2.CoreWebView2.Navigate (url));
                this.webView2AsyncWait.WaitOne ();
            }, CancellationToken.None);
        }

        /// <summary>
        /// 在当前 Web 视图中执行 JavaScript 代码
        /// </summary>
        /// <param name="script">被执行的 JavaScript 代码</param>
        /// <returns>被执行的代码返回的结果</returns>
        public async Task<string> ExecuteScriptAsync (string script)
        {
            return await this.webview2.Dispatcher.Invoke (() => this.webview2.CoreWebView2.ExecuteScriptAsync (script));
        }

        /// <summary>
        /// 将本机对象作为 JavaScript 远程调用终结点
        /// </summary>
        /// <param name="name">JavaScript 对象名</param>
        /// <param name="rawObject">实际终结点对象</param>
        public void AddHostObjectToScript (string name, object rawObject)
        {
            this.webview2.Dispatcher.Invoke (() => this.webview2.CoreWebView2.AddHostObjectToScript (name, rawObject));
        }

        /// <summary>
        /// 向当前 Web 视图投递任务信息
        /// </summary>
        /// <param name="taskInfo">任务信息</param>
        public void PutTask (AWTask taskInfo)
        {
            this.taskInfo = taskInfo ?? throw new ArgumentNullException (nameof (taskInfo));
        }

        /// <summary>
        /// 运行自动化任务
        /// </summary>
        public void Start ()
        {
            if (!this.isRunning)
            {
                // 启动任务
                this.Start (new CancellationTokenSource ())
                    .ContinueWith (task =>
                    {
                        if (this.taskInfo is not null)
                        {
                            if (task.IsCanceled)
                            {
                                this.logger.Warning ($"自动化任务({this.taskInfo.Name}) --> 任务执行被用户取消");
                            }

                            if (task.IsCompletedSuccessfully)
                            {
                                this.logger.Information ($"自动化任务({this.taskInfo.Name}) --> 任务执行成功");
                                this.Reset ();
                            }
                        }
                        else
                        {
                            if (task.IsCanceled)
                            {
                                this.logger.Warning ($"自动化任务 --> 任务执行被用户取消");
                            }

                            if (task.IsCompletedSuccessfully)
                            {
                                this.logger.Information ($"自动化任务 --> 任务执行成功");
                                this.Reset ();
                            }
                        }

                        // 重置任务取消令牌
                        this.taskTokenSource = null;
                    });
            }
            else
            {
                this.logger.Warning ($"自动化任务 --> 任务正在执行");
            }
        }

        /// <summary>
        /// 运行自动化任务
        /// </summary>
        /// <param name="tokenSource">线程取消令牌</param>
        public Task Start (CancellationTokenSource tokenSource)
        {
            if (tokenSource is null)
            {
                throw new ArgumentNullException (nameof (tokenSource));
            }

            if (this.taskInfo is null)
            {
                this.logger.Error ($"自动化任务 --> 任务停止失败, 原因: 未投递任务作业或作业为空");
            }
            else if (!this.isRunning)
            {
                this.isRunning = true;

                this.taskTokenSource ??= tokenSource;
                this.taskTokenSource.Token.Register (this.Reset);

                // 创建任务
                return Task.Run (() =>
                {
                    this.logger.Information ($"自动化任务({this.taskInfo.Name}) --> 任务执行开始");

                    // 注册 javascript 远程对象
                    this.AddHostObjectToScript ("log", this.loggerHostScript);
                    this.AddHostObjectToScript ("wait", this.asyncWaitHostScript);
                    this.logger.Information ($"自动化任务({this.taskInfo.Name}) --> 注入 JavaScript 依赖对象");

                    // 开始执行作业
                    foreach (AWJob job in this.taskInfo.Jobs)
                    {
                        // 检查是否停止任务
                        tokenSource.Token.ThrowIfCancellationRequested ();

                        // 处理作业执行条件
                        if (job.Condition != null)
                        {
                            if (this.conditionSynchronous.Reset (job.Condition))
                            {
                                this.logger.Information ($"自动化任务({this.taskInfo.Name}) --> {job.Name} 等待浏览器同步条件 {job.Condition.Type}");
                            }
                            else
                            {
                                this.logger.Information ($"自动化任务({this.taskInfo.Name}) --> {job.Name} 浏览器已同步条件 {job.Condition.Type}");
                            }

                            this.conditionSynchronous.WaitOneEx ();
                        }

                        // 检查是否停止任务
                        tokenSource.Token.ThrowIfCancellationRequested ();

                        // 进行元素查找
                        if (job.Element != null)
                        {
                            ElementCommand elementCommand = ElementCommandDispatcher.Dispatcher (this, this.logger, job.Element);
                            if (!tokenSource.Token.IsCancellationRequested && elementCommand.Execute ())
                            {
                                for (int i = 0; i < elementCommand.Result; i++)
                                {
                                    if (!RunActions (elementCommand.ResultVariableName, i, job.Actions))
                                    {
                                        break;
                                    }

                                    // 检查是否停止任务
                                    tokenSource.Token.ThrowIfCancellationRequested ();
                                }
                            }
                        }
                        // 内嵌框架元素查找
                        else if (job.Iframe != null)
                        {
                            ElementCommand iframeCommand = ElementCommandDispatcher.Dispatcher (this, this.logger, job.Iframe);
                            if (!tokenSource.Token.IsCancellationRequested && iframeCommand.Execute () && job.Iframe.Element is not null)
                            {
                                ElementCommand elementCommand = ElementCommandDispatcher.Dispatcher (this, this.logger, job.Iframe.Element, iframeCommand.ResultVariableName);
                                if (!tokenSource.Token.IsCancellationRequested && elementCommand.Execute ())
                                {
                                    for (int i = 0; i < elementCommand.Result; i++)
                                    {
                                        if (!RunActions (elementCommand.ResultVariableName, i, job.Actions))
                                        {
                                            break;
                                        }

                                        // 检查是否停止任务
                                        tokenSource.Token.ThrowIfCancellationRequested ();
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!RunActions (null, null, job.Actions))
                            {
                                break;
                            }
                        }
                    }
                }, this.taskTokenSource.Token);
            }

            return Task.FromResult (false);

            bool RunActions (string? variableName, int? index, AWAction[] actions)
            {
                foreach (AWAction action in actions)
                {
                    // 检查是否停止任务
                    this.taskTokenSource.Token.ThrowIfCancellationRequested ();

                    ActionCommand actionCommand = ActionCommandDispatcher.Dispatcher (this, this.logger, action, variableName, index);
                    if (!actionCommand.Execute ())
                    {
                        this.logger.Warning ($"自动化任务({this.taskInfo.Name}) --> 执行 Action({action.Type}) 命令失败, 任务取消执行");
                        return false;
                    }

                    Thread.Sleep (100);
                }

                return true;
            }
        }

        /// <summary>
        /// 停止自动化任务
        /// </summary>
        public void Stop ()
        {
            if (this.taskInfo is null)
            {
                this.logger.Error ($"自动化任务 --> 任务停止失败, 原因: 未投递任务作业或作业为空");
                return;
            }

            if (this.isRunning && this.taskTokenSource is not null)
            {
                this.taskTokenSource.Cancel ();
            }
            else
            {
                this.logger.Warning ($"自动化任务({this.taskInfo.Name}) --> 任务未在运行");
            }
        }

        /// <summary>
        /// 关闭当前 Web 视图
        /// </summary>
        public void Close ()
        {
            this.webview2.Dispatcher.Invoke (() =>
            {
                this.DestroyItself?.Invoke (this, new EventArgs ());
                // 释放浏览器资源
                this.webview2.Dispose ();
            });
        }
        #endregion

        #region --私有方法--
        /// <summary>
        /// 重置任务状态
        /// </summary>
        private void Reset ()
        {
            if (this.isRunning && this.taskInfo is not null)
            {
                this.logger.Information ($"自动化任务({this.taskInfo.Name}) --> 任务正在停止");
                this.conditionSynchronous.Set ();
                this.asyncWaitHostScript.Set ();
                this.webView2CreateNewAsyncWait.Set ();
                this.webView2AsyncWait.Set ();

                this.isRunning = false;
            }
        }

        /// <summary>
        /// 标签页关闭事件处理函数
        /// </summary>
        private void TabItemClosedEventHandler (object? sender, EventArgs e)
        {
            // 释放浏览器资源
            this.webview2.Dispose ();
        }

        /// <summary>
        /// 核心浏览器开始导航事件处理函数
        /// </summary>
        private void CoreWebView2NavigationStartingEventHandler (object? sender, CoreWebView2NavigationStartingEventArgs e)
        {
            this.logger.Information ($"浏览器 --> 正在导航到: {e.Uri}");
        }

        /// <summary>
        /// 核心浏览器文档标题改变事件处理函数
        /// </summary>
        private void CoreWebView2DocumentTitleChangedEventHandler (object? sender, object e)
        {
            this.Header = this.webview2.CoreWebView2.DocumentTitle;
            this.logger.Information ($"浏览器 --> 修改标题, Title: {this.webview2.CoreWebView2.DocumentTitle}");
        }

        /// <summary>
        /// 核心浏览器网站图标改变事件处理函数
        /// </summary>
        private void CoreWebView2FaviconChangedEventHandler (object? sender, object e)
        {
            FaviconElement.SetWidth (this, 16);
            FaviconElement.SetHeight (this, 16);
            FaviconElement.SetFaviconUri (this, this.webview2.CoreWebView2.FaviconUri);
            this.logger.Information ($"浏览器 --> 修改 Favicon, Uri: {this.webview2.CoreWebView2.FaviconUri}");
        }

        /// <summary>
        /// 核心浏览器导航结束事件处理函数
        /// </summary>
        private void CoreWebView2NavigationCompletedEventHandler (object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                this.logger.Information ($"浏览器 --> 导航成功, 状态码: {e.HttpStatusCode}");

                // 同步线程
                AWCondition aWCondition = new () { Type = AWConditionType.Ready };
                this.conditionSynchronous.Set (aWCondition);
                this.logger.Information ($"浏览器 --> 同步条件 {aWCondition.Type}");
            }
            else
            {
                this.logger.Warning ($"浏览器 --> 导航失败, 状态码: {e.WebErrorStatus}({e.HttpStatusCode})");
            }

            this.webView2AsyncWait.Set ();
        }

        /// <summary>
        /// 核心浏览器创建新窗体事件处理函数
        /// </summary>
        private async void CoreWebView2NewWindowRequestedEventHandler (object? sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            // 让子任务等待页面创建完毕
            this.webView2CreateNewAsyncWait.Reset ();
            this.logger.Information ($"浏览器 --> 创建新窗体");
            CoreWebView2Deferral coreWebView2Deferral = e.GetDeferral ();

            // 触发新标签创建事件
            NewWebViewEventArgs args = new ();
            this.CreateNew?.Invoke (this, args);

            // 接管事件返回的 Web 浏览器承载新页面
            if (args.NewWebView is not null)
            {
                // 初始化浏览器
                await args.NewWebView.InitializeAsync ();

                // 承载新页面
                e.NewWindow = args.NewWebView.WebView2.CoreWebView2;
                e.Handled = true;

                // 保存子标签, 用于处理子任务
                this.subTabs.Add (args.NewWebView);
            }

            coreWebView2Deferral.Complete ();
            this.webView2CreateNewAsyncWait.Set ();
        }
        #endregion

        #region --内部类--
        /// <summary>
        /// 表示条件同步服务的类
        /// </summary>
        class ConditionSynchronous : EventWaitHandle
        {
            #region --字段--
            private AWCondition? condition1;
            private AWCondition? condition2;
            #endregion

            #region --构造函数--
            /// <summary>
            /// 初始化 <see cref="ConditionSynchronous"/> 类的新实例
            /// </summary>
            public ConditionSynchronous ()
                : base (true, EventResetMode.AutoReset)
            { }
            #endregion

            #region --公开方法--
            /// <summary>
            /// 将事件状态设置为非终止，从而导致线程受阻
            /// </summary>
            /// <param name="condition">设置事件解锁条件</param>
            /// <exception cref="ArgumentNullException"><paramref name="condition"/> 为 <see langword="null"/></exception>
            public bool Reset (AWCondition condition)
            {
                lock (this)
                {
                    if (condition != null)
                    {
                        this.condition1 = condition;

                        if (this.condition1.Equals (this.condition2) == false)
                        {
                            base.Reset ();
                            return true;
                        }
                    }
                }

                return false;
            }

            /// <summary>
            /// 将事件状态设置为有信号，从而允许一个或多个等待线程继续执行
            /// </summary>
            /// <param name="condition">用来比对解锁条件的实例</param>
            /// <returns>如果该操作成功，则为 <see langword="true"/>；否则为 <see langword="false"/></returns>
            public bool Set (AWCondition condition)
            {
                lock (this)
                {
                    if (condition != null)
                    {
                        this.condition2 = condition;
                    }

                    if (this.condition2?.Equals (this.condition1) == true)
                    {
                        return base.Set ();
                    }
                }

                return false;
            }

            /// <summary>
            /// 阻止当前线程，直到当前 <see cref="ConditionSynchronous"/> 收到信号, 或被任务被取消
            /// </summary>
            /// <param name="cancellationTokenSource">任务取消令牌</param>
            public void WaitOneEx ()
            {
                base.WaitOne ();
                this.condition1 = this.condition2 = null;
            }
            #endregion
        }
        #endregion
    }
}
