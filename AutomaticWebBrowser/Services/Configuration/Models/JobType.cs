namespace AutomaticWebBrowser.Services.Configuration.Models
{
    /// <summary>
    /// 作业类型
    /// </summary>
    enum JobType
    {
        /// <summary>
        /// 无操作
        /// </summary>
        None,
        /// <summary>
        /// 延迟
        /// </summary>
        Delay,
        /// <summary>
        /// 对 HTML 元素发送 Click
        /// </summary>
        Click,
        /// <summary>
        /// 对 HTML 元素发送 Focus
        /// </summary>
        Focus,
        /// <summary>
        /// 对 HTML 元素发送 Blur
        /// </summary>
        Blur,
        /// <summary>
        /// 对 INPUT 元素的 Value 属性设置值
        /// </summary>
        InputValue,
        /// <summary>
        /// 对 INPUT 元素的 Value 属性执行按键输入
        /// </summary>
        InputKeypressValue,
        /// <summary>
        /// 对 HTML 元素发送 KeyDown
        /// </summary>
        KeyDown,
        /// <summary>
        /// 对 HTML 元素发送 KeyUp
        /// </summary>
        KeyUp,
        /// <summary>
        /// 对 HTML 元素发送 KeyPress
        /// </summary>
        KeyPress,
        /// <summary>
        /// 对 HTML 元素发送 MouseDown
        /// </summary>
        MouseDown,
        /// <summary>
        /// 对 HTML 元素发送 MouseUp
        /// </summary>
        MouseUp,
        /// <summary>
        /// 对 HTML 元素发送 MouseClick
        /// </summary>
        MouseClick,
        /// <summary>
        /// 对 HTML 元素发送 MouseDoubleClick
        /// </summary>
        MouseDoubleClick,
        /// <summary>
        /// 打开 A 元素中的链接
        /// </summary>
        OpenLink,
    }
}
