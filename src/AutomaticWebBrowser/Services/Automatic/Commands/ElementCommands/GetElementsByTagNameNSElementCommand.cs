using System;
using System.Text.Json;

using AutomaticWebBrowser.Wpf.Core;
using AutomaticWebBrowser.Wpf.Services.Configuration.Models;

using Serilog;

namespace AutomaticWebBrowser.Wpf.Services.Automatic.Commands.ElementCommands
{
    /// <summary>
    /// document.getElementsByTagNameNS 元素查找命令
    /// </summary>
    [ElementCommand (AWElementType.GetElementsByTagNameNS)]
    class GetElementsByTagNameNSElementCommand : ElementCommand
    {
        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="GetElementsByTagNameNSElementCommand"/> 类的新实例
        /// </summary>
        public GetElementsByTagNameNSElementCommand (IWebView webView, ILogger logger, AWElement element)
            : base (webView, logger, element)
        { }

        /// <summary>
        /// 初始化 <see cref="GetElementsByTagNameNSElementCommand"/> 类的新实例
        /// </summary>
        public GetElementsByTagNameNSElementCommand (IWebView webView, ILogger logger, AWElement element, string? iframeVariableName)
            : base (webView, logger, element, iframeVariableName)
        { }
        #endregion

        #region --公开方法--
        public override bool Execute ()
        {
            if (this.Element.Value is not null)
            {
                try
                {
                    // 获取搜索值
                    AWElementsNameNS value = this.Element.Value?.Deserialize<AWElementsNameNS> (Global.DefaultJsonSerializerOptions)!;

                    // 组合脚本代码
                    string script;
                    if (this.IframeVariableName is null)
                    {
                        script = $@"
const {this.ResultVariableName} = [];
(function () {{
    const log = chrome.webview.hostObjects.log;
    try {{
        let result = Array.from (this.document.getElementsByTagNameNS ('{value.Ns}', '{value.Name}'));
        if (result != null && result != undefined) {{
            result.forEach (element => {{
                {this.ResultVariableName}.push (element);
            }})
        }}
        return {this.ResultVariableName}.length;
    }} catch (e) {{
        log.Error (`自动化任务({this.WebView.TaskInfo.Name}) --> 执行 Element({this.Element.Type}) 命令失败, 原因: JavaScript 函数执行发生异常, 异常信息: ${{e.message}}`);
        return false;
    }}
}}) ();
".Trim ();
                    }
                    else
                    {
                        script = $@"
const {this.ResultVariableName} = [];
(function () {{
    const log = chrome.webview.hostObjects.log;
    try {{
        for (let i = 0; i < {this.IframeVariableName}.length; i++) {{
            let iframe_window = {this.IframeVariableName}[i].contentWindow;
            if (iframe_window != null || iframe_window != undefined){{
                let result = Array.from (iframe_window.document.getElementsByTagNameNS ('{value.Ns}', '{value.Name}'));
                if (result != null && result != undefined) {{
                    result.forEach (element => {{
                        {this.ResultVariableName}.push (element);
                    }})
                }}
            }}
        }}
        return {this.ResultVariableName}.length;
    }} catch (e) {{
        log.Error (`自动化任务({this.WebView.TaskInfo.Name}) --> 执行 Element({this.Element.Type}) 命令失败, 原因: JavaScript 函数执行发生异常, 异常信息: ${{e.message}}`);
        return 0;
    }}
}}) ();
".Trim ();
                    }

                    // 执行 javascript 代码
                    string result = this.WebView.ExecuteScriptAsync (script).Result;
                    if (int.TryParse (result, out int count) && count > 0)
                    {
                        this.Result = count;
                        this.Logger.Information ($"自动化任务({this.WebView.TaskInfo.Name}) --> 执行 Element({this.Element.Type}) 命令成功, 值: (ns={value.Ns},name={value.Name}), 结果: {count}");
                        return true;
                    }
                    else
                    {
                        this.Logger.Information ($"自动化任务({this.WebView.TaskInfo.Name}) --> 执行 Element({this.Element.Type}) 命令失败, 原因: 未搜索到指定的元素, 值: (ns={value.Ns},name={value.Name})");
                    }
                }
                catch (JsonException e)
                {
                    this.Logger.Error (e, $"自动化任务({this.WebView.TaskInfo.Name}) --> 执行 Element({this.Element.Type}) 命令失败, 原因: 值反序列化为 {nameof (String)} 类型失败");
                }
            }
            else
            {
                this.Logger.Warning ($"自动化任务({this.WebView.TaskInfo.Name}) --> 执行 Element({this.Element.Type}) 命令失败, 原因: 值为 null");
            }
            return false;
        }
        #endregion
    }
}
