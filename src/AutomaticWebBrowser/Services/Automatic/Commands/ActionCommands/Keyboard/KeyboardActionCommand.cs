using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using AutomaticWebBrowser.Core;
using AutomaticWebBrowser.Services.Configuration.Attributes;
using AutomaticWebBrowser.Services.Configuration.Models;

using Serilog;

namespace AutomaticWebBrowser.Services.Automatic.Commands.ActionCommands.Keyboard
{
    /// <summary>
    /// 键盘动作命令
    /// </summary>
    abstract class KeyboardActionCommand : ActionCommand
    {
        #region --属性--
        /// <summary>
        /// 事件类型参数
        /// </summary>
        protected abstract string TypeArg { get; }
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="KeyboardActionCommand"/> 类的新实例
        /// </summary>
        protected KeyboardActionCommand (IWebView webView, ILogger logger, AWAction action, string? variableName, int? index)
            : base (webView, logger, action, variableName, index)
        { }
        #endregion

        #region --公开方法--
        public override bool Execute ()
        {
            if (this.VariableName is not null && this.Action.Value != null)
            {
                try
                {
                    // 获取键盘事件参数
                    AWKeyboard keyboard = this.Action.Value?.Deserialize<AWKeyboard> (Global.DefaultJsonSerializerOptions)!;

                    // 获取 KeyAttribute
                    KeyAttribute? keyAttribute = GetKeyAttribute (keyboard.Code);

                    if (keyAttribute != null)
                    {
                        string script = $@"
(async function () {{
    const log = chrome.webview.hostObjects.log;
    const wait = chrome.webview.hostObjects.wait;
    const sleep = function (seconds) {{
        return new Promise(resolve => {{
            setTimeout(resolve, seconds);
        }});
    }}
    let aw_event = new KeyboardEvent ('{this.TypeArg}', {{
        view: this.window,
        bubbles: true,
        cancelable: true,
        ctrlKey: {keyboard.CtrlKey.ToString ().ToLower ()},
        shiftKey: {keyboard.ShiftKey.ToString ().ToLower ()},
        altKey: {keyboard.AltKey.ToString ().ToLower ()},
        metaKey: {keyboard.MetaKey.ToString ().ToLower ()},
        code: '{keyboard.Code}',
        key: '{(keyboard.ShiftKey == false ? keyAttribute.NormalKey : keyAttribute.ShiftKey)}',
        keyCode: {keyAttribute.KeyCode},
        repeat: false
    }});
    let element = {this.VariableName}[{this.Index}];
    for (let j = 1; j <= {keyboard.Count}; j++) {{
        try {{
            element.dispatchEvent (aw_event);
            if (j === 1) {{
                aw_event.repeat = true;
            }}
            log.Information (`自动化任务({this.WebView.TaskInfo.Name}) --> ${{element.nodeName == undefined ? ""WINDOW"" : element.nodeName}} 执行 Action({this.Action.Type}) 命令成功, 次数: ${{j}}`);
        }} catch (e) {{
            log.Error (`自动化任务({this.WebView.TaskInfo.Name}) --> ${{element.nodeName == undefined ? ""WINDOW"" : element.nodeName}} 执行 Action({this.Action.Type}) 命令失败, 原因: JavaScript 函数执行发生异常, 异常信息: ${{e.message}}`);
        }} finally {{
            await sleep ({keyboard.Delay});
        }}
    }}
    wait.Set ();
}}) ();
".Trim ();
                        this.WebView.ExecuteScriptAsync (script);
                        this.WebView.AsyncWaitHostScript.WaitOne ();
                        return true;
                    }
                    else
                    {
                        this.Logger.Warning ($"自动化任务 --> 执行 Action({this.Action.Type}) 命令失败, 原因: 使用了未定义的键");
                    }
                }
                catch (JsonException e)
                {
                    this.Logger.Error (e, $"自动化任务 --> 执行 Action({this.Action.Type}) 命令失败, 原因: 值反序列化为 {nameof (AWKeyboard)} 类型失败");
                }

            }
            else
            {
                this.Logger.Warning ($"自动化任务 --> 执行 Action({this.Action.Type}) 命令失败, 原因: 值为 null");
            }

            return false;
        }
        #endregion

        #region --私有方法--
        private static KeyAttribute? GetKeyAttribute (AWKeyCode code)
        {
            string value = code.ToString ();
            FieldInfo? field = code.GetType ().GetField (value);
            if (field != null)
            {
                return field.GetCustomAttribute<KeyAttribute> ();
            }
            return null;
        }
        #endregion
    }
}
