using System;

namespace AutomaticWebBrowser.Services.Configuration.Attributes
{
    [AttributeUsage (AttributeTargets.Field)]
    class ButtonAttribute : Attribute
    {
        /// <summary>
        /// MouseEvent 事件的 button 值, 按键按下
        /// </summary>
        public int Button { get; set; }
    }
}
