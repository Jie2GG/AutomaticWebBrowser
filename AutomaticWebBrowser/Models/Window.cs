using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Models
{
    /// <summary>
    /// 窗体配置类
    /// </summary>
    public class Window
    {
        /// <summary>
        /// 是否最大化窗口
        /// </summary>
        [JsonPropertyName ("maximize")]
        public bool Maximize { get; set; } = false;

        /// <summary>
        /// 客户区大小
        /// </summary>
        [JsonPropertyName ("size")]
        public ClientSize Size { get; set; } = new ClientSize ()
        {
            Width = 800,
            Height = 500
        };
    }
}
