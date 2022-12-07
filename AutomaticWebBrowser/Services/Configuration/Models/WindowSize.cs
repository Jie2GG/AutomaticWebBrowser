using System.Drawing;
using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Services.Configuration.Models
{
    /// <summary>
    /// 窗口大小
    /// </summary>
    class WindowSize
    {
        #region --常量--
        public static readonly WindowSize MainFormSize = new (1200, 900);
        public static readonly WindowSize LogFormSize = new (800, 300);
        #endregion

        #region --属性--
        /// <summary>
        /// 宽度
        /// </summary>
        [JsonPropertyName ("width")]
        public int Width { get; }

        /// <summary>
        /// 高度
        /// </summary>
        [JsonPropertyName ("height")]
        public int Height { get; }
        #endregion

        #region --构造函数--
        [JsonConstructor]
        public WindowSize (int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }
        #endregion

        #region --隐式转换--
        public static implicit operator Size (WindowSize size)
        {
            return new Size (size.Width, size.Height);
        }
        #endregion
    }
}
