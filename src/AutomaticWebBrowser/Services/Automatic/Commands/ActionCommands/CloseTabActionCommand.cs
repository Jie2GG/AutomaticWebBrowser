using AutomaticWebBrowser.Wpf.Core;
using AutomaticWebBrowser.Wpf.Services.Configuration.Models;

using Serilog;

namespace AutomaticWebBrowser.Wpf.Services.Automatic.Commands.ActionCommands
{
    /// <summary>
    /// 关闭标签页动作命令
    /// </summary>
    [ActionCommand (AWActionType.CloseTab)]
    class CloseTabActionCommand : ActionCommand
    {
        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="CloseTabActionCommand"/> 类的新实例
        /// </summary>
        public CloseTabActionCommand (IWebView webView, ILogger logger, AWAction action, string? variableName, int? index)
            : base (webView, logger, action, variableName, index)
        { }
        #endregion

        #region --公开方法--
        public override bool Execute ()
        {
            this.WebView.Close ();
            this.Logger.Information ($"自动化任务({this.WebView.TaskInfo.Name}) --> 执行 Action({this.Action.Type}) 命令成功");
            return true;
        }
        #endregion
    }
}
