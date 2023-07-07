﻿using System;

using AutomaticWebBrowser.Wpf.Services.Configuration.Models;

namespace AutomaticWebBrowser.Wpf.Services.Automatic.Commands.ElementCommands
{
    /// <summary>
    /// 元素命令特性
    /// </summary>
    [AttributeUsage (AttributeTargets.Class)]
    class ElementCommandAttribute : Attribute
    {
        /// <summary>
        /// 元素类型
        /// </summary>
        public AWElementType ElementType { get; set; }

        /// <summary>
        /// 初始化 <see cref="ElementCommandAttribute"/> 类的新实例
        /// </summary>
        /// <param name="elementType"></param>
        public ElementCommandAttribute (AWElementType elementType)
        {
            this.ElementType = elementType;
        }
    }
}