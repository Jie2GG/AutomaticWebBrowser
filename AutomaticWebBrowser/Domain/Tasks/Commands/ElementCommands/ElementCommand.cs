using System;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Domain.Configuration.Models;

using Serilog.Core;

namespace AutomaticWebBrowser.Domain.Tasks.Commands.ElementCommands
{
    /// <summary>
    /// 元素查找命令
    /// </summary>
    abstract class ElementCommand : ICommand<int>
    {
        #region --属性--
        /// <summary>
        /// 返回值
        /// </summary>
        public int Result { get; protected set; }

        /// <summary>
        /// 内嵌框架变量名
        /// </summary>
        public string? IframeVariableName { get; }

        /// <summary>
        /// 查询结果变量名
        /// </summary>
        public string ResultVariableName { get; }

        /// <summary>
        /// WebView2
        /// </summary>
        public IWebView WebView { get; }

        /// <summary>
        /// 日志
        /// </summary>
        public Logger Log { get; }

        /// <summary>
        /// 元素
        /// </summary>
        public AWElement Element { get; }
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="ElementCommand"/> 类的新实例
        /// </summary>
        protected ElementCommand (IWebView webView, Logger log, AWElement element)
            : this (webView, log, element, null)
        { }

        /// <summary>
        /// 初始化 <see cref="ElementCommand"/> 类的新实例
        /// </summary>
        protected ElementCommand (IWebView webView, Logger log, AWElement element, string? iframeVariableName)
        {
            this.WebView = webView ?? throw new ArgumentNullException (nameof (webView));
            this.Log = log ?? throw new ArgumentNullException (nameof (log));
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
