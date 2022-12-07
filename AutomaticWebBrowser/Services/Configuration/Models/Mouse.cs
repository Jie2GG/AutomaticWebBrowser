using System.Text.Json.Serialization;

using Gecko;

namespace AutomaticWebBrowser.Services.Configuration.Models
{
    /// <summary>
    /// 鼠标配置
    /// </summary>
    class Mouse
    {
        /// <summary>
        /// 按下 Control 键
        /// </summary>
        [JsonPropertyName ("ctrl")]
        public bool Control { get; set; } = false;

        /// <summary>
        /// 按下 Alt 键
        /// </summary>
        [JsonPropertyName ("alt")]
        public bool Alt { get; set; } = false;

        /// <summary>
        /// 按下 Shift 键
        /// </summary>
        [JsonPropertyName ("shift")]
        public bool Shift { get; set; } = false;

        /// <summary>
        /// 按下 Meta 键
        /// </summary>
        [JsonPropertyName ("meta")]
        public bool Meta { get; set; } = false;

        /// <summary>
        /// 按下鼠标键的键值
        /// </summary>
        [JsonPropertyName ("button")]
        [JsonConverter (typeof (JsonStringEnumConverter))]
        public GeckoMouseButton Button { get; set; } = GeckoMouseButton.None;

        /// <summary>
        /// 按下键的次数
        /// </summary>
        [JsonPropertyName ("count")]
        public int Count { get; set; } = 1;
    }
}
