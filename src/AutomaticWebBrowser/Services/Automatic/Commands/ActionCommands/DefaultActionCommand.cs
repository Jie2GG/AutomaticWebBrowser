using System.Threading;
using System.Threading.Tasks;

using AutomaticWebBrowser.Wpf.Core;
using AutomaticWebBrowser.Wpf.Services.Configuration.Models;

using Serilog;

namespace AutomaticWebBrowser.Wpf.Services.Automatic.Commands.ActionCommands
{
    /// <summary>
    /// 默认动作命令
    /// </summary>
    class DefaultActionCommand : ActionCommand
    {
        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="DefaultActionCommand"/> 类的新实例
        /// </summary>
        public DefaultActionCommand (IWebView webView, ILogger logger, AWAction action, string? variableName, int? index)
            : base (webView, logger, action, variableName, index)
        { }
        #endregion

        #region --公开方法--
        public override bool Execute ()
        {
            if (this.VariableName is not null)
            {
                string script = $@"
(function () {{
    const log = chrome.webview.hostObjects.log;
    const element = {this.VariableName}[{this.Index}];
    log.Warning (`自动化任务({this.WebView.TaskInfo.Name}) --> ${{element.nodeName == undefined ? ""WINDOW"" : element.nodeName}} 执行 Action({this.Action.Type}) 命令, 原因: 未找到指定类型的 {nameof (ActionCommand)} 或使用了未知的 {nameof (AWActionType)}`);
}}) ();
".Trim ();
                this.WebView.ExecuteScriptAsync (script);
            }

            return false;
        }
        #endregion
    }
}
