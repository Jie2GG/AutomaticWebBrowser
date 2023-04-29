﻿using System.Text.Json;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Domain.Configuration.Models;

using Serilog.Core;

namespace AutomaticWebBrowser.Domain.Tasks.Commands.ActionCommands
{
    /// <summary>
    /// 设置 input 元素值命令
    /// </summary>
    [ActionCommand (AWActionType.SetValue)]
    class SetValueActionCommand : ActionCommand
    {
        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="SetValueActionCommand"/> 类的新实例
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="log"></param>
        /// <param name="action"></param>
        /// <param name="variableName"></param>
        public SetValueActionCommand (IWebView webView, Logger log, AWAction action, string? variableName)
            : base (webView, log, action, variableName)
        { }
        #endregion

        #region --公开方法--
        public override bool Execute ()
        {
            if (this.VariableName is not null && this.Action.Value != null)
            {
                if (this.Action.Value?.ValueKind == JsonValueKind.String)
                {
                    string value = this.Action.Value?.Deserialize<string> (Global.DefaultJsonSerializerOptions)!;

                    string script = $@"
function {this.VariableName}_ActionCommand_SetValue_Func () {{
    const log = chrome.webview.hostObjects.log;
    {this.VariableName}.forEach(element => {{
        try {{
            element.value = '{value}';
            log.Info (`自动化任务 --> ${{element.nodeName == undefined ? ""WINDOW"" : element.nodeName}} 执行 Action({this.Action.Type}) 命令成功, 值: {value}`);
        }} catch (e) {{
            log.Error (`自动化任务 --> ${{element.nodeName == undefined ? ""WINDOW"" : element.nodeName}} 执行 Action({this.Action.Type}) 命令失败, 原因: JavaScript 函数执行发生异常, 异常信息: ${{e.message}}`);
        }}
    }});
}}
{this.VariableName}_ActionCommand_SetValue_Func ();
".Trim ();
                    this.WebView.SafeExecuteScriptAsync (script).Wait ();
                }

                return true;
            }

            return false;
        } 
        #endregion
    }
}