namespace AutomaticWebBrowser.Domain.Configuration.Models
{
    /// <summary>
    /// 动作类型
    /// </summary>
    enum AWActionType
    {
        /// <summary>
        /// 表示不执行任何操作
        /// </summary>
        None = 0,
        /// <summary>
        /// 设置元素 value 属性
        /// </summary>
        SetValue = 1,
        /// <summary>
        /// 设置元素的 value 属性, 并触发元素的 input 事件
        /// </summary>
        InputValue = 2,
        /// <summary>
        /// 调用元素的 click 函数
        /// </summary>
        Click = 3,
        /// <summary>
        /// 触发元素 keydown 事件
        /// </summary>
        KeyDown = 4,
        /// <summary>
        /// 触发元素 keyup 事件
        /// </summary>
        KeyUp = 5,
        /// <summary>
        /// 触发元素 keypress 事件
        /// </summary>
        KeyPress = 6,
        /// <summary>
        /// 触发元素 mousedown 事件
        /// </summary>
        MouseDown = 7,
        /// <summary>
        /// 触发 mouseup 事件
        /// </summary>
        MouseUp = 8,
        /// <summary>
        /// 触发 click 事件
        /// </summary>
        MouseClick = 9,
        /// <summary>
        /// 触发 dbclick 事件
        /// </summary>
        MouseDbClick = 10,
    }
}
