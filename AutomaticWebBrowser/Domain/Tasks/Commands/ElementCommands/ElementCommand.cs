using System;
using System.Diagnostics;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Domain.Configuration.Models;

using Serilog.Core;

namespace AutomaticWebBrowser.Domain.Tasks.Commands.ElementCommands
{
    /// <summary>
    /// 元素查找命令
    /// </summary>
    abstract class ElementCommand : ICommand<string?>
    {
        #region --属性--
        /// <summary>
        /// 返回值
        /// </summary>
        public string? Result { get; }

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
        {
            this.WebView = webView ?? throw new ArgumentNullException (nameof (webView));
            this.Log = log ?? throw new ArgumentNullException (nameof (log));
            this.Element = element ?? throw new ArgumentNullException (nameof (element));

            // 创建临时变量
            this.Result = $@"aw_{Guid.NewGuid ():N}";
            Debug.WriteLine ($"变量名: {this.Result}");
        }
        #endregion

        #region --公开方法--
        public abstract bool Execute ();
        #endregion
    }
}
