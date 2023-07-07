using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using AutomaticWebBrowser.Wpf.Core;
using AutomaticWebBrowser.Wpf.Services.Configuration.Models;

using Serilog;

namespace AutomaticWebBrowser.Wpf.Services.Automatic.Commands.ActionCommands
{
    /// <summary>
    /// 模拟 input 输入值命令
    /// </summary>
    [ActionCommand (AWActionType.InputValue)]
    class InputValueActionCommand : ActionCommand
    {
        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="InputValueActionCommand"/> 类的新实例
        /// </summary>
        public InputValueActionCommand (IWebView webView, ILogger logger, AWAction action, string? variableName, int? index)
            : base (webView, logger, action, variableName, index)
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
(function () {{
    const log = chrome.webview.hostObjects.log;
    let aw_event = new InputEvent('input', {{
        inputType: 'insertText',
        data: '{value}', 
        dataTransfer: null, 
        isComposing: false
    }});
    const element = {this.VariableName}[{this.Index}];
    try {{
        element.value = '{value}';
        element.dispatchEvent (aw_event);
        log.Information (`自动化任务({this.WebView.TaskInfo.Name}) --> ${{element.nodeName == undefined ? ""WINDOW"" : element.nodeName}} 执行 Action({this.Action.Type}) 命令成功, 值: {value}`);
    }} catch (e) {{
        log.Error (`自动化任务({this.WebView.TaskInfo.Name}) --> ${{element.nodeName == undefined ? ""WINDOW"" : element.nodeName}} 执行 Action({this.Action.Type}) 命令失败, 原因: JavaScript 函数执行发生异常, 异常信息: ${{e.message}}`);
    }}
}}) ();
".Trim ();
                    this.WebView.ExecuteScriptAsync (script);
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}
