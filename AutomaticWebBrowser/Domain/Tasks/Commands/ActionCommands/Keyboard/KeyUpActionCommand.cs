using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Domain.Configuration.Models;

using Serilog.Core;

namespace AutomaticWebBrowser.Domain.Tasks.Commands.ActionCommands.Keyboard
{
    /// <summary>
    /// 触发 keyup 事件动作命令
    /// </summary>
    [ActionCommand (AWActionType.KeyUp)]
    class KeyUpActionCommand : KeyboardActionCommand
    {
        #region --属性--
        protected override string TypeArg => "keyup";
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="KeyUpActionCommand"/> 类的新实例
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="log"></param>
        /// <param name="action"></param>
        /// <param name="variableName"></param>
        public KeyUpActionCommand (IWebView webView, Logger log, AWAction action, string? variableName)
            : base (webView, log, action, variableName)
        { } 
        #endregion
    }
}
