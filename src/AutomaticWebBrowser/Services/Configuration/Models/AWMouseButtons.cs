using AutomaticWebBrowser.Services.Configuration.Attributes;

namespace AutomaticWebBrowser.Services.Configuration.Models
{
    /// <summary>
    /// 鼠标按键
    /// </summary>
    enum AWMouseButtons
    {
        /// <summary>
        /// 表示没有按键被按下
        /// </summary>
        [Button (Button = 0)]
        None = 0,
        /// <summary>
        /// 鼠标左键
        /// </summary>
        [Button (Button = 0)]
        Left = 1,
        /// <summary>
        /// 鼠标右键
        /// </summary>
        [Button (Button = 1)]
        Right = 2,
        /// <summary>
        /// 鼠标中键
        /// </summary>
        [Button (Button = 2)]
        Middle = 4
    }
}
