using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Models
{
    /// <summary>
    /// 设置
    /// </summary>
    public class Setting
    {
        /// <summary>
        /// 窗口设置
        /// </summary>
        [JsonPropertyName ("window")]
        public Window Window { get; set; } = new Window ();

        /// <summary>
        /// 浏览器设置
        /// </summary>
        [JsonPropertyName ("browser")]
        public Browser Browser { get; set; } = new Browser ();
    }
}
