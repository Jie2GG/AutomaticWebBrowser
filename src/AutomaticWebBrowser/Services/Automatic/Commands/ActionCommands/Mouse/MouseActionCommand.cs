﻿using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using AutomaticWebBrowser.Core;
using AutomaticWebBrowser.Services.Configuration.Attributes;
using AutomaticWebBrowser.Services.Configuration.Models;

using Serilog;

namespace AutomaticWebBrowser.Services.Automatic.Commands.ActionCommands.Mouse
{
    /// <summary>
    /// 鼠标动作命令
    /// </summary>
    abstract class MouseActionCommand : ActionCommand
    {
        #region --属性--
        /// <summary>
        /// 事件类型参数
        /// </summary>
        protected abstract string TypeArg { get; }
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="MouseActionCommand"/> 类的新实例
        /// </summary>
        protected MouseActionCommand (IWebView webView, ILogger logger, AWAction action, string? variableName, int? index)
            : base (webView, logger, action, variableName, index)
        { }
        #endregion

        #region --公开方法--
        public override bool Execute ()
        {
            if (this.VariableName is not null && this.Action.Value is JsonElement json)
            {
                try
                {
                    // 获取鼠标事件参数
                    AWMouse mouse = json.Deserialize<AWMouse> ()!;

                    string script = $@"
(async function () {{
    const log = chrome.webview.hostObjects.log;
    const wait = chrome.webview.hostObjects.wait;
    const sleep = function (seconds) {{
        return new Promise(resolve => {{
            setTimeout(resolve, seconds);
        }});
    }}
    const getX = function (element) {{
        let actualLeft = element.offsetLeft;
        let current = element.offsetParent;
        while (current !== null) {{
            actualLeft += current.offsetLeft;
            current = current.offsetParent;
        }}
        if (this.document.compatMode == ""BackCompat"") {{
            return actualLeft - this.document.body.scrollLeft;
        }} else {{
            return actualLeft - this.document.documentElement.scrollLeft;
        }}
    }}
    const getY = function (element) {{
        let actualTop = element.offsetTop;
        let current = element.offsetParent;
        while (current !== null) {{
            actualTop += current.offsetTop;
            current = current.offsetParent;
        }}
        if (this.document.compatMode == ""BackCompat"") {{
            return actualTop - this.document.body.scrollTop;
        }} else {{
            return actualTop - this.document.documentElement.scrollTop;
        }}
    }}

    let element = {this.VariableName}[{this.Index}];
    let aw_event = new MouseEvent ('{this.TypeArg}', {{
        view: this.window,
        bubbles: true,
        cancelable: true,
        ctrlKey: {mouse.CtrlKey.ToString ().ToLower ()},
        shiftKey: {mouse.ShiftKey.ToString ().ToLower ()},
        altKey: {mouse.AltKey.ToString ().ToLower ()},
        metaKey: {mouse.MetaKey.ToString ().ToLower ()},
        screenX: this.window.screenX + getX (element),
        screenY: this.window.screenY + getY (element),
        clientX: getX (element),
        clientY: getY (element),
        button: {this.GetButton (mouse.Buttons)},
        buttons: {this.GetButtons (mouse.Buttons)}
    }});
    for (let j = 1; j <= {mouse.Count}; j++) {{
        try {{
            element.dispatchEvent (aw_event);
            if (j === 1) {{
                aw_event.repeat = true;
            }}
            log.Information (`自动化任务({this.WebView.TaskInfo.Name}) --> ${{element.nodeName == undefined ? ""WINDOW"" : element.nodeName}} 执行 Action({this.Action.Type}) 命令成功, 次数: ${{j}}`);
            await sleep ({mouse.Delay});
        }} catch (e) {{
            log.Error (`自动化任务({this.WebView.TaskInfo.Name}) --> ${{element.nodeName == undefined ? ""WINDOW"" : element.nodeName}} 执行 Action({this.Action.Type}) 命令失败, 原因: JavaScript 函数执行发生异常, 异常信息: ${{e.message}}`);
            break;
        }}
    }}
    wait.Set ();
}}) ();
".Trim ();
                    this.WebView.ExecuteScriptAsync (script);
                    this.WebView.AsyncWaitHostScript.WaitOne ();
                    return true;
                }
                catch (JsonException e)
                {
                    this.Logger.Error (e, $"自动化任务({this.WebView.TaskInfo.Name}) --> 执行 Action({this.Action.Type}) 命令失败, 原因: 值反序列化为 {nameof (AWMouse)} 类型失败");
                }

            }
            else
            {
                this.Logger.Warning ($"自动化任务({this.WebView.TaskInfo.Name}) --> 执行 Action({this.Action.Type}) 命令失败, 原因: 值为 null");
            }
            return false;
        }
        #endregion

        #region --私有方法--
        protected abstract int GetButton (AWMouseButtons buttons);

        protected abstract int GetButtons (AWMouseButtons buttons);

        protected static ButtonAttribute? GetButtonAttribute (AWMouseButtons buttons)
        {
            string value = buttons.ToString ();
            FieldInfo? field = buttons.GetType ().GetField (value);
            if (field != null)
            {
                return field.GetCustomAttribute<ButtonAttribute> ();
            }
            return null;
        }
        #endregion
    }
}
