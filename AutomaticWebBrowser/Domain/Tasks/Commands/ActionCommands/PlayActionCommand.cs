﻿using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Domain.Configuration.Models;

using Serilog.Core;

namespace AutomaticWebBrowser.Domain.Tasks.Commands.ActionCommands
{
    [ActionCommand (AWActionType.Play)]
    class PlayActionCommand : ActionCommand
    {
        public PlayActionCommand (IWebView webView, Logger log, AWAction action, string? variableName)
            : base (webView, log, action, variableName)
        { }

        public override bool Execute ()
        {
            if (this.VariableName is not null)
            {
                string script = $@"
function {this.VariableName}_ActionCommand_Play_Func () {{
    const log = chrome.webview.hostObjects.log;
    {this.VariableName}.forEach(element => {{
        try {{
            element.play ();
            log.Info (`自动化任务 --> ${{element.nodeName == undefined ? ""WINDOW"" : element.nodeName}} 执行 Action({this.Action.Type}) 命令成功`);
        }} catch (e) {{
            log.Error (`自动化任务 --> ${{element.nodeName == undefined ? ""WINDOW"" : element.nodeName}} 执行 Action({this.Action.Type}) 命令失败, 原因: JavaScript 函数执行发生异常, 异常信息: ${{e.message}}`);
        }}
    }});
}}
{this.VariableName}_ActionCommand_Play_Func ();
".Trim ();
                this.WebView.SafeExecuteScriptAsync (script).Wait ();

                return true;
            }

            return false;
        }
    }
}