using System.Drawing;
using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Models
{
    /// <summary>
    /// 浏览器大小结构
    /// </summary>
    public class ClientSize
    {
        /// <summary>
        /// 宽度
        /// </summary>
        [JsonPropertyName ("width")]
        public int Width { get; set; }

        /// <summary>
        /// 高度
        /// </summary>
        [JsonPropertyName ("height")]
        public int Height { get; set; }


        public static implicit operator Size (ClientSize size)
        {
            return new Size (size.Width, size.Height);
        }
    }
}
