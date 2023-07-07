using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

using AutomaticWebBrowser.Wpf.Services.Configuration.Models;
using AutomaticWebBrowser.Wpf.Services.Logger;
using AutomaticWebBrowser.Wpf.Services.Thread;

using Microsoft.Web.WebView2.Wpf;

namespace AutomaticWebBrowser.Wpf.Core
{
    /// <summary>
    /// Web 视图接口
    /// </summary>
    interface IWebView
    {
        #region --属性--
        /// <summary>
        /// Web 视图接口托管的 <see cref="Microsoft.Web.WebView2.Wpf.WebView2"/>
        /// </summary>
        WebView2 WebView2 { get; }

        /// <summary>
        /// 当前 Web 视图 <see cref="CreateNew"/> 事件异步等待服务
        /// </summary>
        AutoResetEvent WebView2CreateNewAsyncWait { get; }

        /// <summary>
        /// 由当前 Web 视图打开的逻辑子标签页
        /// </summary>
        Collection<IWebView> SubTabs { get; }

        /// <summary>
        /// 当前 Web 视图用于提供给 JavaScript 的异步等待服务
        /// </summary>
        AsyncWaitHostScript AsyncWaitHostScript { get; }

        /// <summary>
        /// 获取当前 Web 视图的任务信息
        /// </summary>
        AWTask TaskInfo { get; }

        /// <summary>
        /// 获取当前 Web 视图的任务取消令牌
        /// </summary>
        CancellationTokenSource TaskTokenSource { get; }
        #endregion

        #region --事件--
        /// <summary>
        /// 创建新 Tab 事件
        /// </summary>
        event EventHandler<NewWebViewEventArgs> CreateNew;

        /// <summary>
        /// 销毁自身 Tab 事件
        /// </summary>
        event EventHandler DestroyItself;
        #endregion

        #region --方法--
        /// <summary>
        /// 初始化当前 Web 视图
        /// </summary>
        Task InitializeAsync ();

        /// <summary>
        /// 导航到指定的地址
        /// </summary>
        /// <param name="url">目标网址</param>
        Task NavigateAsync (string url);

        /// <summary>
        /// 在当前 Web 视图中执行 JavaScript 代码
        /// </summary>
        /// <param name="script">被执行的 JavaScript 代码</param>
        /// <returns>被执行的代码返回的结果</returns>
        Task<string> ExecuteScriptAsync (string script);

        /// <summary>
        /// 将本机对象作为 JavaScript 远程调用终结点
        /// </summary>
        /// <param name="name">JavaScript 对象名</param>
        /// <param name="rawObject">实际终结点对象</param>
        void AddHostObjectToScript (string name, object rawObject);

        /// <summary>
        /// 向当前 Web 视图投递任务信息
        /// </summary>
        /// <param name="taskInfo">任务信息</param>
        void PutTask (AWTask taskInfo);

        /// <summary>
        /// 运行自动化任务
        /// </summary>
        Task Run ();

        /// <summary>
        /// 停止自动化任务
        /// </summary>
        /// <returns></returns>
        void Stop ();
        
        /// <summary>
        /// 关闭当前 Web 视图
        /// </summary>
        void Close ();
        #endregion

        #region --内部类--
        /// <summary>
        /// 创建新 WebView 事件参数
        /// </summary>
        public class NewWebViewEventArgs : EventArgs
        {
            public IWebView? NewWebView { get; set; }
        }
        #endregion
    }
}
