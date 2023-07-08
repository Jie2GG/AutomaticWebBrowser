using System;

using AutomaticWebBrowser.Core;
using AutomaticWebBrowser.Services.Configuration.Models;

using Serilog;

namespace AutomaticWebBrowser.Services.Automatic.Commands.ElementCommands
{
    /// <summary>
    /// 元素查找命令
    /// </summary>
    abstract class ElementCommand : ICommand<int>
    {
        #region --属性--
        /// <summary>
        /// 当前命令的元素查找结果
        /// </summary>
        public int Result { get; protected set; }

        /// <summary>
        /// 当前命令搜索目标 iframe 变量名
        /// </summary>
        public string? IframeVariableName { get; }

        /// <summary>
        /// 当前命令查找结果存储的变量名
        /// </summary>
        public string ResultVariableName { get; }

        /// <summary>
        /// 当前命令关联的 Web 视图
        /// </summary>
        public IWebView WebView { get; }

        /// <summary>
        /// 当前命令关联的跟踪程序
        /// </summary>
        public ILogger Logger { get; }

        /// <summary>
        /// 当前命令待查找的元素信息
        /// </summary>
        public AWElement Element { get; }
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="ElementCommand"/> 类的新实例
        /// </summary>
        protected ElementCommand (IWebView webView, ILogger logger, AWElement element)
            : this (webView, logger, element, null)
        { }

        /// <summary>
        /// 初始化 <see cref="ElementCommand"/> 类的新实例
        /// </summary>
        protected ElementCommand (IWebView webView, ILogger logger, AWElement element, string? iframeVariableName)
        {
            this.WebView = webView ?? throw new ArgumentNullException (nameof (webView));
            this.Logger = logger ?? throw new ArgumentNullException (nameof (logger));
            this.Element = element ?? throw new ArgumentNullException (nameof (element));
            this.IframeVariableName = iframeVariableName;

            this.Result = 0;
            // 创建临时变量
            this.ResultVariableName = $@"aw_{Guid.NewGuid ():N}";
        }
        #endregion

        #region --公开方法--
        public abstract bool Execute ();
        #endregion
    }
}
