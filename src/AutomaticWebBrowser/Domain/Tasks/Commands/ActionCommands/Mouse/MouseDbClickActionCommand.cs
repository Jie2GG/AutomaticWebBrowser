﻿using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Domain.Configuration.Models;

using Serilog.Core;

namespace AutomaticWebBrowser.Domain.Tasks.Commands.ActionCommands.Mouse
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
        /// <param name="webView"></param>
        /// <param name="log"></param>
        /// <param name="action"></param>
        /// <param name="variableName"></param>
        public MouseDbClickActionCommand (IWebView webView, Logger log, AWAction action, string? variableName, int? index) 
            : base (webView, log, action, variableName, index)
        { }
        #endregion

        #region --私有方法--
        protected override int GetButton (AWMouseButtons buttons)
        {
            return 0;
        }

        protected override int GetButtons (AWMouseButtons buttons)
        {
            return 0;
        }
        #endregion
    }
}