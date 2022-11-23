namespace AutomaticWebBrowser.Models
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// 等待
        /// </summary>
        Waiting,
        /// <summary>
        /// 获取焦点
        /// </summary>
        Focus,
        /// <summary>
        /// 点击
        /// </summary>
        Click,
        /// <summary>
        /// 输入
        /// </summary>
        Input,
        /// <summary>
        /// 按键输入
        /// </summary>
        KeypressInput,
        /// <summary>
        /// 按键按下
        /// </summary>
        KeyDown,
        /// <summary>
        /// 按键松开
        /// </summary>
        KeyUp,
        /// <summary>
        /// 按键按下并松开
        /// </summary>
        KeyPress,
        /// <summary>
        /// 鼠标按键按下
        /// </summary>
        MouseDown,
        /// <summary>
        /// 鼠标按键松开
        /// </summary>
        MouseUp,
        /// <summary>
        /// 鼠标按键按下并松开
        /// </summary>
        MouseClick,
        /// <summary>
        /// 获取 DOM 元素
        /// </summary>
        GetDomElement,
    }
}
