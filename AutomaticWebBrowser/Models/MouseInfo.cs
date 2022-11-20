﻿using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace AutomaticWebBrowser.Models
{
    /// <summary>
    /// 鼠标信息
    /// </summary>
    public class MouseInfo
    {
        /// <summary>
        /// 是否按下 Ctrl 键
        /// </summary>
        [JsonPropertyName ("ctrl")]
        public bool CtrlKey { get; set; }

        /// <summary>
        /// 是否按下 Alt 键
        /// </summary>
        [JsonPropertyName ("alt")]
        public bool AltKey { get; set; }

        /// <summary>
        /// 是否按下 Shift 键
        /// </summary>
        [JsonPropertyName ("shift")]
        public bool ShiftKey { get; set; }

        /// <summary>
        /// 是否按下 Meta 键
        /// </summary>
        [JsonPropertyName ("meta")]
        public bool MetaKey { get; set; }

        /// <summary>
        /// 按键码
        /// </summary>
        [JsonPropertyName ("key-code")]
        [JsonConverter (typeof (JsonStringEnumConverter))]
        public MouseKeys KeyCode { get; set; }

        /// <summary>
        /// 触发次数
        /// </summary>
        [JsonPropertyName ("count")]
        public int Count { get; set; } = 1;
    }
}
