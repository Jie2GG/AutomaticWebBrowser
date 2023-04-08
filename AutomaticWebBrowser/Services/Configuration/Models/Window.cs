using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace AutomaticWebBrowser.Services.Configuration.Models
{
    /// <summary>
    /// 窗口配置
    /// </summary>
    class Window
    {
        /// <summary>
        /// 窗口状态
        /// </summary>
        [JsonPropertyName ("state")]
        [JsonConverter (typeof (JsonStringEnumConverter))]
        public FormWindowState State { get; set; } = FormWindowState.Normal;

        /// <summary>
        /// 窗口位置
        /// </summary>
        [JsonPropertyName ("location")]
        public WindowLocation Location { get; set; }

        /// <summary>
        /// 窗口大小
        /// </summary>
        [JsonPropertyName ("size")]
        public WindowSize Size { get; set; }

        /// <summary>
        /// 窗口初始位置
        /// </summary>
        [JsonPropertyName ("start-position")]
        [JsonConverter (typeof (JsonStringEnumConverter))]
        public FormStartPosition StartPosition { get; set; } = FormStartPosition.CenterParent;
    }
}
