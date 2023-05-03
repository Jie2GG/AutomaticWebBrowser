using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Domain.Configuration.Models;

using Serilog.Core;

namespace AutomaticWebBrowser.Domain.Tasks.Commands.ElementCommands
{
    /// <summary>
    /// 默认元素查找命令
    /// </summary>
    class DefaultElementCommand : ElementCommand
    {
        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="DefaultElementCommand"/> 类的新实例
        /// </summary>
        public DefaultElementCommand (IWebView webView, Logger log, AWElement element)
            : base (webView, log, element)
        { }

        /// <summary>
        /// 初始化 <see cref="DefaultElementCommand"/> 类的新实例
        /// </summary>
        public DefaultElementCommand (IWebView webView, Logger log, AWElement element, string? iframeVariableName)
            : base (webView, log, element, iframeVariableName)
        { }
        #endregion

        #region --公开方法--
        public override bool Execute ()
        {
            this.Log.Warning ($"自动化任务 --> 执行 Element(Default) 命令, 原因: 未找到指定类型的 {nameof (ElementCommand)} 或使用了未知的 {nameof (AWElementType)}");
            return false;
        }
        #endregion
    }
}
