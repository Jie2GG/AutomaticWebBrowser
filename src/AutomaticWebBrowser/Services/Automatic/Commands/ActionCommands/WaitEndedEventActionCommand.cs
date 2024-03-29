﻿using System.Threading;
using System.Threading.Tasks;

using AutomaticWebBrowser.Core;
using AutomaticWebBrowser.Services.Configuration.Models;

using Serilog;

namespace AutomaticWebBrowser.Services.Automatic.Commands.ActionCommands
{
    /// <summary>
    /// 等待引发 ended 事件动作命令
    /// </summary>
    [ActionCommand (AWActionType.WaitEndedEvent)]
    class WaitEndedEventActionCommand : ActionCommand
    {
        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="WaitEndedEventActionCommand"/> 类的新实例
        /// </summary>
        public WaitEndedEventActionCommand (IWebView webView, ILogger logger, AWAction action, string? variableName, int? index)
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
    const wait = chrome.webview.hostObjects.wait;
    const element = {this.VariableName}[{this.Index}];
    const eventCallback = function (e) {{
        log.Info (`自动化任务 --> ${{element.nodeName == undefined ? ""WINDOW"" : element.nodeName}} 执行 Action({this.Action.Type}) 命令成功, ended 事件已触发`);
        element.removeEventListener ('ended', eventCallback);          
        wait.Set ();
    }}
    try {{
        element.addEventListener ('ended', eventCallback);
        log.Information (`自动化任务({this.WebView.TaskInfo.Name}) --> ${{element.nodeName == undefined ? ""WINDOW"" : element.nodeName}} 执行 Action({this.Action.Type}) 命令, 正在等待 ended 事件触发`);
    }} catch (e) {{
        log.Error (`自动化任务({this.WebView.TaskInfo.Name}) --> ${{element.nodeName == undefined ? ""WINDOW"" : element.nodeName}} 执行 Action({this.Action.Type}) 命令失败, 原因: JavaScript 函数执行发生异常, 异常信息: ${{e.message}}`);
    }}
}}) ();
".Trim ();
                this.WebView.ExecuteScriptAsync (script);
                this.WebView.AsyncWaitHostScript.WaitOne ();
                return true;
            }
            return false;
        }
        #endregion
    }
}
