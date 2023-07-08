using System.Text.Json.Serialization;
using System.Windows;

namespace AutomaticWebBrowser.Services.Configuration.Models
{
    /// <summary>
    /// 窗体配置
    /// </summary>
    class AWWindow
    {
        /// <summary>
        /// 窗体状态
        /// </summary>
        [JsonPropertyName ("state")]
        [JsonConverter (typeof (JsonStringEnumConverter))]
        public WindowState State { get; set; } = WindowState.Normal;

        /// <summary>
        /// 窗体启动位置
        /// </summary>
        [JsonPropertyName ("startup-location")]
        [JsonConverter (typeof (JsonStringEnumConverter))]
        public WindowStartupLocation StartupLocation { get; set; }

        /// <summary>
        /// 窗体左边
        /// </summary>
        [JsonPropertyName ("left")]
        public double Left { get; set; } = double.NaN;

        /// <summary>
        /// 窗体顶边
        /// </summary>
        [JsonPropertyName ("top")]
        public double Top { get; set; } = double.NaN;

        /// <summary>
        /// 窗体宽度
        /// </summary>
        [JsonPropertyName ("width")]
        public double Width { get; set; }

        /// <summary>
        /// 窗体高度
        /// </summary>
        [JsonPropertyName ("height")]
        public double Height { get; set; }
    }
}
