using System;
using System.Threading;

using AutomaticWebBrowser.Core;
using AutomaticWebBrowser.Services.Configuration.Models;

using Serilog;

namespace AutomaticWebBrowser.Services.Automatic.Commands.ActionCommands
{
    /// <summary>
    /// 动作命令
    /// </summary>
    abstract class ActionCommand : ICommand
    {
        #region --属性--
        /// <summary>
        /// 当前命令关联的 Web 视图
        /// </summary>
        public IWebView WebView { get; }

        /// <summary>
        /// 当前命令关联的跟踪程序
        /// </summary>
        public ILogger Logger { get; }

        /// <summary>
        /// 当前命令待执行的动作信息
        /// </summary>
        public AWAction Action { get; }

        /// <summary>
        /// 当前命令执行的目标元素变量名
        /// </summary>
        public string? VariableName { get; }

        /// <summary>
        /// 当前命令执行的目标元素索引
        /// </summary>
        public int? Index { get; }
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="ActionCommand"/> 类的新实例
        /// </summary>
        protected ActionCommand (IWebView webView, ILogger logger, AWAction action, string? variableName, int? index)
        {
            this.WebView = webView ?? throw new ArgumentNullException (nameof (webView));
            this.Logger = logger ?? throw new ArgumentNullException (nameof (logger));
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
