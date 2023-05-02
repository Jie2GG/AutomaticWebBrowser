using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Domain.Configuration.Models;

using Serilog.Core;

namespace AutomaticWebBrowser.Domain.Tasks.Commands.ElementCommands
{
    /// <summary>
    /// document 对象元素查找命令
    /// </summary>
    [ElementCommand (AWElementType.Document)]
    class DocumentElementCommand : ElementCommand
    {
        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="DocumentElementCommand"/> 类的新实例
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="log"></param>
        /// <param name="element"></param>
        public DocumentElementCommand (IWebView webView, Logger log, AWElement element)
            : base (webView, log, element)
        { }
        #endregion

        #region --公开方法--
        public override bool Execute ()
        {
            string script;
            if (this.IframeVariableName is null)
            {
                // 合成 javascript 代码
                script = $@"
const {this.ResultVariableName} = [];
(function () {{
    {this.ResultVariableName}.push (this.document);
    return {this.ResultVariableName}.length;
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
                {this.ResultVariableName}.push (iframe_window.document);  
            }}
        }}
        return {this.ResultVariableName}.length;
    }} catch (e) {{
        log.Error (`自动化任务 --> 执行 Element({this.Element.Type}) 命令失败, 原因: JavaScript 函数执行发生异常, 异常信息: ${{e.message}}`);
        return 0;
    }}
}}) ();
".Trim ();
            }

            // 执行 javascript 代码
            string result = this.WebView.SafeExecuteScriptAsync (script).Result;
            if (int.TryParse (result, out int count) && count > 0)
            {
                this.Result = count;
                this.Log.Information ($"自动化任务 --> 执行 Element({this.Element.Type}) 命令成功");
                return true;
            }
            return false;
        }
        #endregion
    }
}
