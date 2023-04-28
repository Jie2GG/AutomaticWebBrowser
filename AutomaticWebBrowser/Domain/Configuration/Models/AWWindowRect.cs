using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Domain.Configuration.Models
{
    /// <summary>
    /// 窗体大小
    /// </summary>
    readonly struct AWWindowRect
    {
        #region --常量--
        public static readonly AWWindowRect Empty = new (0, 0);
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
        public AWWindowRect (int width, int height)
            : this ()
        {
            this.Width = width;
            this.Height = height;
        }
        #endregion
    }
}
