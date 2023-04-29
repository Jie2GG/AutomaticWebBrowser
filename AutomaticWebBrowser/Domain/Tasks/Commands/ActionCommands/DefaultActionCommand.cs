using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Domain.Configuration.Models;

using Serilog.Core;

namespace AutomaticWebBrowser.Domain.Tasks.Commands.ActionCommands
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
        /// <param name="webView"></param>
        /// <param name="log"></param>
        /// <param name="action"></param>
        /// <param name="variableName"></param>
        public DefaultActionCommand (IWebView webView, Logger log, AWAction action, string? variableName)
            : base (webView, log, action, variableName)
        { }
        #endregion

        #region --公开方法--
        public override bool Execute ()
        {
            if (this.VariableName is not null)
            {
                string script = $@"
function {this.VariableName}_ActionCommand_Default_Func () {{
    const log = chrome.webview.hostObjects.log;
    {this.VariableName}.forEach(element => {{
        log.Warning (`自动化任务 --> ${{element.nodeName == undefined ? ""WINDOW"" : element.nodeName}} 执行 Action({this.Action.Type}) 命令, 原因: 未找到指定类型的 {nameof (ActionCommand)} 或使用了未知的 {nameof (AWActionType)}`);
    }});
}}
{this.VariableName}_ActionCommand_Default_Func ();
".Trim ();
                this.WebView.SafeExecuteScriptAsync (script).Wait ();

                return true;
            }

            return false;
        } 
        #endregion
    }
}
