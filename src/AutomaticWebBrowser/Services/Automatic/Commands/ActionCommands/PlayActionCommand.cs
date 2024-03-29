﻿using System.Threading;
using System.Threading.Tasks;

using AutomaticWebBrowser.Core;
using AutomaticWebBrowser.Services.Configuration.Models;

using Serilog;

namespace AutomaticWebBrowser.Services.Automatic.Commands.ActionCommands
{
    /// <summary>
    /// 调用元素 play 函数动作命令
    /// </summary>
    [ActionCommand (AWActionType.Play)]
    class PlayActionCommand : ActionCommand
    {
        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="PlayActionCommand"/> 类的新实例
        /// </summary>
        public PlayActionCommand (IWebView webView, ILogger logger, AWAction action, string? variableName, int? index)
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
    try {{
        element.play ();
        log.Information (`自动化任务({this.WebView.TaskInfo.Name}) --> ${{element.nodeName == undefined ? ""WINDOW"" : element.nodeName}} 执行 Action({this.Action.Type}) 命令成功`);
    }} catch (e) {{
        log.Error (`自动化任务({this.WebView.TaskInfo.Name}) --> ${{element.nodeName == undefined ? ""WINDOW"" : element.nodeName}} 执行 Action({this.Action.Type}) 命令失败, 原因: JavaScript 函数执行发生异常, 异常信息: ${{e.message}}`);
    }}
}}) ();
".Trim ();
                this.WebView.ExecuteScriptAsync (script);
                return true;
            }
            return false;
        }
        #endregion
    }
}
