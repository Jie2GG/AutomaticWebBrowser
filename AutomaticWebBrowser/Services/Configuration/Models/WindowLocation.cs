using System.Drawing;
using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Services.Configuration.Models
{
    /// <summary>
    /// 窗口位置
    /// </summary>
    class WindowLocation
    {
        #region --常量--
        public static readonly WindowLocation Empty = new WindowLocation (0, 0);
        #endregion

        #region --属性--
        /// <summary>
        /// X 坐标
        /// </summary>
        [JsonPropertyName ("x")]
        public int X { get; }

        /// <summary>
        /// Y 坐标
        /// </summary>
        [JsonPropertyName ("y")]
        public int Y { get; }
        #endregion

        #region --构造函数--
        [JsonConstructor]
        public WindowLocation (int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        #endregion

        #region --隐式转换--
        public static implicit operator Point (WindowLocation size)
        {
            return new Point (size.X, size.Y);
        }
        #endregion
    }
}
