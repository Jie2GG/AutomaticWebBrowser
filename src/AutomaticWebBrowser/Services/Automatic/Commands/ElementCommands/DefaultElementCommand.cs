using AutomaticWebBrowser.Core;
using AutomaticWebBrowser.Services.Configuration.Models;

using Serilog;

namespace AutomaticWebBrowser.Services.Automatic.Commands.ElementCommands
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
        public DefaultElementCommand (IWebView webView, ILogger logger, AWElement element)
            : base (webView, logger, element)
        { }

        /// <summary>
        /// 初始化 <see cref="DefaultElementCommand"/> 类的新实例
        /// </summary>
        public DefaultElementCommand (IWebView webView, ILogger logger, AWElement element, string? iframeVariableName)
            : base (webView, logger, element, iframeVariableName)
        { }
        #endregion

        #region --公开方法--
        public override bool Execute ()
        {
            this.Logger.Warning ($"自动化任务({this.WebView.TaskInfo.Name}) --> 执行 Element(Default) 命令, 原因: 未找到指定类型的 {nameof (ElementCommand)} 或使用了未知的 {nameof (AWElementType)}");
            return false;
        }
        #endregion
    }
}
