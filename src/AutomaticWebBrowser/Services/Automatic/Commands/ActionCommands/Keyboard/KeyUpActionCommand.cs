using System.Threading;

using AutomaticWebBrowser.Core;
using AutomaticWebBrowser.Services.Configuration.Models;

using Serilog;

namespace AutomaticWebBrowser.Services.Automatic.Commands.ActionCommands.Keyboard
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

        public KeyUpActionCommand (IWebView webView, ILogger logger, AWAction action, string? variableName, int? index)
            : base (webView, logger, action, variableName, index)
        { }
        #endregion
    }
}
