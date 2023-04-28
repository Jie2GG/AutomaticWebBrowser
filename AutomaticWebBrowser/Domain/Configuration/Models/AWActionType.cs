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
        SetValue,
        /// <summary>
        /// 设置元素的 value 属性, 并触发元素的 input 事件
        /// </summary>
        InputValue,
        /// <summary>
        /// 调用元素的 click 函数
        /// </summary>
        Click,
        /// <summary>
        /// 触发元素 keydown 事件
        /// </summary>
        KeyDown,
    }
}
