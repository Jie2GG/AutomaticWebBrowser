using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Domain.Configuration.Models;

using Serilog.Core;

namespace AutomaticWebBrowser.Domain.Tasks.Commands.ActionCommands.Keyboard
{
    /// <summary>
    /// 触发 keypress 事件动作命令
    /// </summary>
    [ActionCommand (AWActionType.KeyPress)]
    class KeyPressActionCommand : KeyboardActionCommand
    {
        #region --属性--
        protected override string TypeArg => "keypress";
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="KeyPressActionCommand"/> 类的新实例
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="log"></param>
        /// <param name="action"></param>
        /// <param name="variableName"></param>
        public KeyPressActionCommand (IWebView webView, Logger log, AWAction action, string? variableName)
            : base (webView, log, action, variableName)
        { }
        #endregion
    }
}
