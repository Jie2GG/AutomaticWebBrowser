using System;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Domain.Configuration.Models;

using Serilog.Core;

namespace AutomaticWebBrowser.Domain.Tasks.Commands.ActionCommands
{
    /// <summary>
    /// 动作命令
    /// </summary>
    abstract class ActionCommand : ICommand
    {
        #region --属性--
        /// <summary>
        /// WebView2
        /// </summary>
        public IWebView WebView { get; }

        /// <summary>
        /// 日志
        /// </summary>
        public Logger Log { get; }

        /// <summary>
        /// 动作
        /// </summary>
        public AWAction Action { get; }

        /// <summary>
        /// 变量名
        /// </summary>
        public string? VariableName { get; }

        /// <summary>
        /// 索引
        /// </summary>
        public int? Index { get; }
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="ActionCommand"/> 类的新实例
        /// </summary>
        protected ActionCommand (IWebView webView, Logger log, AWAction action, string? variableName, int? index)
        {
            this.WebView = webView ?? throw new ArgumentNullException (nameof (webView));
            this.Log = log ?? throw new ArgumentNullException (nameof (log));
            this.Action = action ?? throw new ArgumentNullException (nameof (action));
            this.VariableName = variableName;
            this.Index = index;
        }
        #endregion

        #region --公开方法--
        public abstract bool Execute ();
        #endregion
    }
}
