﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Input;

using AutomaticWebBrowser.Wpf.Controls;
using AutomaticWebBrowser.Wpf.Core;
using AutomaticWebBrowser.Wpf.Services.Configuration.Models;

using HandyControl.Controls;

using Prism.Commands;
using Prism.Mvvm;

using Serilog;

using Unity;

namespace AutomaticWebBrowser.Wpf.ViewModels
{
    class MainViewModel : BindableBase
    {
        #region --字段--
        private ObservableCollection<IWebView> webViewTabs;
        private int webViewTabCurrentIndex;
        #endregion

        #region --属性--
        /// <summary>
        /// 日志
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// 配置
        /// </summary>
        public AWConfig Config { get; set; }

        /// <summary>
        /// WebView 标签页集合
        /// </summary>
        public ObservableCollection<IWebView> WebViewTabs
        {
            get => this.webViewTabs;
            set => this.SetProperty (ref this.webViewTabs, value);
        }

        /// <summary>
        /// WebView 标签页当前索引
        /// </summary>
        public int WebViewTabCurrentIndex
        {
            get => this.webViewTabCurrentIndex;
            set => this.SetProperty (ref this.webViewTabCurrentIndex, value);
        }
        #endregion

        #region --命令--
        /// <summary>
        /// 初始化命令
        /// </summary>
        public ICommand InitializeCommand => new DelegateCommand (() =>
        {
            this.LoadTasks ();
        });

        /// <summary>
        /// 显示关于命令
        /// </summary>
        public static ICommand ShowAboutCommand => new DelegateCommand (() =>
        {
            Growl.InfoGlobal ($"正在开发中");
        });

        /// <summary>
        /// 运行命令, 开始自动化任务的命令
        /// </summary>
        public ICommand RunCommand => new DelegateCommand (() =>
        {
            this.WebViewTabs[this.WebViewTabCurrentIndex].Run ();
        });

        /// <summary>
        /// 停止命令, 停止自动化任务的命令
        /// </summary>
        public ICommand StopCommand => new DelegateCommand (() =>
        {
            this.WebViewTabs[this.WebViewTabCurrentIndex].Stop ();
        });

        /// <summary>
        /// 显示日志命令
        /// </summary>
        public static ICommand ShowLogCommand => new DelegateCommand (() =>
        {
            Growl.InfoGlobal ($"正在开发中");
        });

        /// <summary>
        /// 显示设置命令
        /// </summary>
        public static ICommand ShowSettingCommand => new DelegateCommand (() =>
        {
            Growl.InfoGlobal ($"正在开发中");
        });
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="MainViewModel"/> 类的新实例
        /// </summary>
        /// <param name="logger"></param>
        public MainViewModel ([Dependency] ILogger logger, [Dependency] AWConfig config)
        {
            this.Logger = logger;
            this.Config = config;

            // 设定默认值
            this.webViewTabs = new ObservableCollection<IWebView> ();
            this.webViewTabCurrentIndex = 0;
        }
        #endregion

        #region --私有方法--
        private async void LoadTasks ()
        {
            this.Logger.Information ($"自动化任务 --> 任务加载开始, 数量: {this.Config.Tasks.Length}");

            #region 任务处理
            for (int i = 0; i < this.Config.Tasks.Length; i++)
            {
                int taskNumber = i + 1;

                if (this.Config.Tasks[i].Name is null)
                {
                    this.Config.Tasks[i].Name = $"Task: {taskNumber}";
                }

                for (int j = 0; j < this.Config.Tasks[i].Jobs.Length; j++)
                {
                    int jobNumber = j + 1;

                    if (this.Config.Tasks[i].Jobs[j].Name is null)
                    {
                        this.Config.Tasks[i].Jobs[j].Name = $"Job: {taskNumber}-{jobNumber}";
                    }
                }
            }
            #endregion

            #region 任务加载
            foreach (AWTask task in this.Config.Tasks)
            {
                this.Logger.Information ($"自动化任务 --> 任务 ({task.Name}) 正在加载");

                try
                {
                    // 创建浏览器
                    IWebView webView = new WebViewTab (this.Logger, this.Config.Browser);
                    this.WebViewTabs.Add (webView);
                    this.WebViewTabCurrentIndex = this.WebViewTabs.Count - 1;

                    // 挂载浏览器事件
                    webView.CreateNew += this.WebViewCreateNewTabEventHandler;
                    webView.DestroyItself += this.WebViewDestroyItselfEventHandler;

                    // 投递任务
                    webView.PutTask (task);

                    // 初始化浏览器
                    await webView.InitializeAsync ();

                    // 导航到网页
                    await webView.NavigateAsync (task.Url);
                }
                catch (Exception ex)
                {
                    this.Logger.Error (ex, $"自动化任务 --> 任务 ({task.Name}) 加载失败");
                }
                this.Logger.Information ($"自动化任务 --> 任务 ({task.Name}) 加载完成");

            }
            #endregion

            this.Logger.Information ($"自动化任务 --> 任务加载结束");
        }

        /// <summary>
        /// Web 视图创建新标签页事件处理函数
        /// </summary>
        private void WebViewCreateNewTabEventHandler (object? sender, IWebView.NewWebViewEventArgs e)
        {
            this.Logger.Information ($"浏览器 --> 重定向到新标签页");

            // 创建标签页
            IWebView webview = new WebViewTab (this.Logger, this.Config.Browser);
            this.WebViewTabs.Add (webview);
            this.WebViewTabCurrentIndex = this.WebViewTabs.Count - 1;

            // 挂载新 Tab 事件
            webview.CreateNew += this.WebViewCreateNewTabEventHandler;
            webview.DestroyItself += this.WebViewDestroyItselfEventHandler;

            // 放入事件参数
            e.NewWebView = webview;
        }

        private void WebViewDestroyItselfEventHandler (object? sender, EventArgs e)
        {
            if (sender is IWebView webView)
            {
                this.WebViewTabs.Remove (webView);
            }
        }
        #endregion
    }
}
