using System.Threading;

using AutomaticWebBrowser.Core;
using AutomaticWebBrowser.Services.Configuration.Attributes;
using AutomaticWebBrowser.Services.Configuration.Models;

using Serilog;

namespace AutomaticWebBrowser.Services.Automatic.Commands.ActionCommands.Mouse
{
    /// <summary>
    /// 触发 dbclick 事件动作命令
    /// </summary>
    [ActionCommand (AWActionType.MouseDbClick)]
    class MouseDbClickActionCommand : MouseActionCommand
    {
        #region --属性--
        protected override string TypeArg => "dbclick";
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="MouseDbClickActionCommand"/> 类的新实例
        /// </summary>
        public MouseDbClickActionCommand (IWebView webView, ILogger logger, AWAction action, string? variableName, int? index)
            : base (webView, logger, action, variableName, index)
        { }
        #endregion

        #region --私有方法--
        protected override int GetButton (AWMouseButtons buttons)
        {
            ButtonAttribute? buttonAttribute = GetButtonAttribute (buttons);
            if (buttonAttribute != null)
            {
                return buttonAttribute.Button;
            }
            else
            {
                this.Logger.Warning ($"自动化任务 --> 执行 Action({this.Action.Type}) 命令失败, 原因: 使用了未定义的键");
            }

            return 0;
        }

        protected override int GetButtons (AWMouseButtons buttons)
        {
            return (int)buttons;
        }
        #endregion
    }
}
