﻿using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Domain.Configuration.Models;

using Serilog.Core;

namespace AutomaticWebBrowser.Domain.Tasks.Commands.ElementCommands
{
    /// <summary>
    /// documnet.getElementById 元素查找命令
    /// </summary>
    [ElementCommand (AWElementType.GetElementById)]
    class GetElementByIdElementCommand : ElementCommand
    {
        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="GetElementByIdElementCommand"/> 类的新实例
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="log"></param>
        /// <param name="element"></param>
        public GetElementByIdElementCommand (IWebView webView, Logger log, AWElement element)
            : base (webView, log, element)
        { }
        #endregion

        #region --公开方法--
        public override bool Execute ()
        {
            string script = $@"
const {this.Result} = [];
function {this.Result}_ElementCommand_GetElementById_Func () {{
    const log = chrome.webview.hostObjects.log;
    try {{
        let result = this.document.getElementById ('{this.Element.Value}');
        if (result != null && result != undefined) {{
            {this.Result}.push (result);
        }}
        return {this.Result}.length > 0;
    }} catch (e) {{
        log.Error (`自动化任务 --> 执行 Element({this.Element.Type}) 命令失败, 原因: JavaScript 函数执行发生异常, 异常信息: ${{e.message}}`);
        return false;
    }}
}}
{this.Result}_ElementCommand_GetElementById_Func ();
".Trim ();

            // 执行 javascript 代码
            string result = this.WebView.SafeExecuteScriptAsync (script).Result;
            if ("true".Equals (result))
            {
                this.Log.Information ($"自动化任务 --> 执行 Element({this.Element.Type}) 命令成功, 值: {this.Element.Value}");
                return true;
            }
            else
            {
                this.Log.Information ($"自动化任务 --> 执行 Element({this.Element.Type}) 命令失败, 原因: 未搜索到指定的元素, 值: {this.Element.Value}");
            }

            return false;
        } 
        #endregion
    }
}