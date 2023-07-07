using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Wpf.Services.Configuration.Models
{
    /// <summary>
    /// 键盘
    /// </summary>
    class AWKeyboard
    {
        /// <summary>
        /// 按下 Ctrl 键
        /// </summary>
        [JsonPropertyName ("ctrl-key")]
        public bool CtrlKey { get; set; } = false;

        /// <summary>
        /// 按下 Shift 键
        /// </summary>
        [JsonPropertyName ("shift-key")]
        public bool ShiftKey { get; set; } = false;

        /// <summary>
        /// 按下 Alt 键
        /// </summary>
        [JsonPropertyName ("alt-key")]
        public bool AltKey { get; set; } = false;

        /// <summary>
        /// 按下 Meta 键 (Win键)
        /// </summary>
        [JsonPropertyName ("meta-key")]
        public bool MetaKey { get; set; } = false;

        /// <summary>
        /// 按键代码
        /// </summary>
        [JsonPropertyName ("code")]
        [JsonConverter (typeof (JsonStringEnumConverter))]
        public AWKeyCode Code { get; set; }

        /// <summary>
        /// 触发次数
        /// </summary>
        [JsonPropertyName ("count")]
        public int Count { get; set; } = 1;

        /// <summary>
        /// 每次触发间隔
        /// </summary>
        [JsonPropertyName ("delay")]
        public int Delay { get; set; } = 100;
    }
}
