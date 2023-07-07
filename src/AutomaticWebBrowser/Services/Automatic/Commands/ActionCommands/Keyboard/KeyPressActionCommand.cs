using System.Threading;

using AutomaticWebBrowser.Wpf.Core;
using AutomaticWebBrowser.Wpf.Services.Configuration.Models;

using Serilog;

namespace AutomaticWebBrowser.Wpf.Services.Automatic.Commands.ActionCommands.Keyboard
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
        public KeyPressActionCommand (IWebView webView, ILogger logger, AWAction action, string? variableName, int? index)
            : base (webView, logger, action, variableName, index)
        { }
        #endregion
    }
}
