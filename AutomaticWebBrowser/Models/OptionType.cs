namespace AutomaticWebBrowser.Models
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OptionType
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
        /// 输入
        /// </summary>
        Input,
        /// <summary>
        /// 按键点击
        /// </summary>
        KeyPress,
        /// <summary>
        /// 按键按下
        /// </summary>
        KeyDown,
        /// <summary>
        /// 按键弹起
        /// </summary>
        KeyUp,
        /// <summary>
        /// 点击元素
        /// </summary>
        Click,
        /// <summary>
        /// 鼠标按键按下
        /// </summary>
        MouseDown,
        /// <summary>
        /// 鼠标按键弹起
        /// </summary>
        MouseUp,
    }
}
