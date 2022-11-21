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
    }
}
