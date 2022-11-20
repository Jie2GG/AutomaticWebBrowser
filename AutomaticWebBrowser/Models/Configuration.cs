using System;
using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Models
{
    /// <summary>
    /// 配置
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// 版本
        /// </summary>
        [JsonPropertyName ("ver")]
        public string Version { get; set; }

        /// <summary>
        /// 设置
        /// </summary>
        [JsonPropertyName ("setting")]
        public Setting Setting { get; set; } = new Setting ();

        /// <summary>
        /// 任务信息
        /// </summary>
        [JsonPropertyName ("task-info")]
        public TaskInfo TaskInfo { get; set; }
    }
}
