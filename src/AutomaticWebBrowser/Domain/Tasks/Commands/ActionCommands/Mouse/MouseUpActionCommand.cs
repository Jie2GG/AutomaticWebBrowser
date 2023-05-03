using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Domain.Configuration.Attributes;
using AutomaticWebBrowser.Domain.Configuration.Models;

using Serilog.Core;

namespace AutomaticWebBrowser.Domain.Tasks.Commands.ActionCommands.Mouse
{
    /// <summary>
    /// 触发 mouseup 事件动作命令
    /// </summary>
    [ActionCommand (AWActionType.MouseUp)]
    class MouseUpActionCommand : MouseActionCommand
    {
        #region --属性--
        protected override string TypeArg => "mouseup";
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="MouseUpActionCommand"/> 类的新实例
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="log"></param>
        /// <param name="action"></param>
        /// <param name="variableName"></param>
        public MouseUpActionCommand (IWebView webView, Logger log, AWAction action, string? variableName, int? index) 
            : base (webView, log, action, variableName, index)
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
                this.Log.Warning ($"自动化任务 --> 执行 Action({this.Action.Type}) 命令失败, 原因: 使用了未定义的键");
            }

            return 0;
        }

        protected override int GetButtons (AWMouseButtons buttons)
        {
            return 0;
        }
        #endregion
    }
}
