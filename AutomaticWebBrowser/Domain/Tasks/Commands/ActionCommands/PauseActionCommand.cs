using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Domain.Configuration.Models;

using Serilog.Core;

namespace AutomaticWebBrowser.Domain.Tasks.Commands.ActionCommands
{
    [ActionCommand (AWActionType.Pause)]
    class PauseActionCommand : ActionCommand
    {
        public PauseActionCommand (IWebView webView, Logger log, AWAction action, string? variableName, int? index) 
            : base (webView, log, action, variableName, index)
        { }

        public override bool Execute ()
        {
            if (this.VariableName is not null)
            {
                string script = $@"
(function () {{
    const log = chrome.webview.hostObjects.log;
    const element = {this.VariableName}[{this.Index}];
    try {{
        element.pause ();
        log.Info (`自动化任务 --> ${{element.nodeName == undefined ? ""WINDOW"" : element.nodeName}} 执行 Action({this.Action.Type}) 命令成功`);
    }} catch (e) {{
        log.Error (`自动化任务 --> ${{element.nodeName == undefined ? ""WINDOW"" : element.nodeName}} 执行 Action({this.Action.Type}) 命令失败, 原因: JavaScript 函数执行发生异常, 异常信息: ${{e.message}}`);
    }}
}}) ();
".Trim ();
                this.WebView.SafeExecuteScriptAsync (script).Wait ();

                return true;
            }

            return false;
        }
    }
}
