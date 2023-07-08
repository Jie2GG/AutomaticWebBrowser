using System;

using AutomaticWebBrowser.Services.Configuration.Models;

namespace AutomaticWebBrowser.Services.Automatic.Commands.ActionCommands
{
    /// <summary>
    /// 动作命令特性
    /// </summary>
    [AttributeUsage (AttributeTargets.Class)]
    class ActionCommandAttribute : Attribute
    {
        #region --属性--
        /// <summary>
        /// 动作类型
        /// </summary>
        public AWActionType ActionType { get; set; }
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="ActionCommandAttribute"/> 类的新实例
        /// </summary>
        /// <param name="type"></param>
        public ActionCommandAttribute (AWActionType type)
        {
            this.ActionType = type;
        }
        #endregion
    }
}
