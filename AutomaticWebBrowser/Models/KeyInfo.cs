using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace AutomaticWebBrowser.Models
{
    /// <summary>
    /// 按键信息
    /// </summary>
    public class KeyInfo
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
        /// 按下键的键值
        /// </summary>
        [JsonPropertyName ("key")]
        [JsonConverter (typeof (JsonStringEnumConverter))]
        public Keys Key { get; set; }

        /// <summary>
        /// 按下键的次数
        /// </summary>
        [JsonPropertyName ("count")]
        public int Count { get; set; } = 1;
    }
}
