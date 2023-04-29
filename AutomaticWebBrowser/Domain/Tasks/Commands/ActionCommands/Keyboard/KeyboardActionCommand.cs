using System.Reflection;
using System.Text.Json;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Domain.Configuration.Attributes;
using AutomaticWebBrowser.Domain.Configuration.Models;

using Serilog.Core;

namespace AutomaticWebBrowser.Domain.Tasks.Commands.ActionCommands.Keyboard
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
        /// <param name="webView"></param>
        /// <param name="log"></param>
        /// <param name="action"></param>
        /// <param name="variableName"></param>
        protected KeyboardActionCommand (IWebView webView, Logger log, AWAction action, string? variableName)
            : base (webView, log, action, variableName)
        { }
        #endregion

        #region --公开方法--
        public override bool Execute ()
        {
            if (this.VariableName is not null && this.Action.Value != null)
            {
                if (this.Action.Value?.ValueKind == JsonValueKind.Object)
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
async function {this.VariableName}_ActionCommand_Keyboard_{this.TypeArg}_Func () {{
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
        keyCode: {(int)keyAttribute.KeyCode},
        repeat: false
    }});

    for (let i = 0; i <= {this.VariableName}.length; i++) {{
        let element = {this.VariableName}[i];
        for (let j = 1; j <= {keyboard.Count}; j++) {{
            try {{
                element.dispatchEvent (aw_event);
                if (j === 1) {{
                    aw_event.repeat = true;
                }}
                log.Info (`自动化任务 --> ${{element.nodeName == undefined ? ""WINDOW"" : element.nodeName}} 执行 Action({this.Action.Type}) 命令成功, 次数: ${{j}}`);
            }} catch (e) {{
                log.Error (`自动化任务 --> ${{element.nodeName == undefined ? ""WINDOW"" : element.nodeName}} 执行 Action({this.Action.Type}) 命令失败, 原因: JavaScript 函数执行发生异常, 异常信息: ${{e.message}}`);
            }} finally {{
                await sleep ({keyboard.Delay});
            }}
        }}
    }}
    wait.Set ();
}}
{this.VariableName}_ActionCommand_Keyboard_{this.TypeArg}_Func ();
".Trim ();
                            this.WebView.SafeExecuteScriptAsync (script).Wait ();
                            this.WebView.WaitHostScript.WaitOne ();
                        }
                        else
                        {
                            this.Log.Warning ($"自动化任务 --> 执行 Action({this.Action.Type}) 命令失败, 原因: 使用了未定义的键");
                        }
                    }
                    catch (JsonException e)
                    {
                        this.Log.Error (e, $"自动化任务 --> 执行 Action({this.Action.Type}) 命令失败, 原因: 值反序列化为 {nameof (AWKeyboard)} 类型失败");
                    }
                }
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
