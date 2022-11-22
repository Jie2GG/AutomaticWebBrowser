using System.Text.Json.Serialization;

using Serilog.Events;

namespace AutomaticWebBrowser.Models
{
    /// <summary>
    /// 日志配置
    /// </summary>
    public class Log
    {
        /// <summary>
        /// 日志格式
        /// </summary>
        [JsonPropertyName ("format")]
        public string Format { get; set; }

        /// <summary>
        /// 日志等级
        /// </summary>
        [JsonPropertyName ("level")]
        [JsonConverter (typeof (JsonStringEnumConverter))]
        public LogEventLevel Level { get; set; } = LogEventLevel.Information;

        /// <summary>
        /// 日志路径
        /// </summary>
        [JsonPropertyName ("path")]
        public string Path { get; set; } = "./logs";

        /// <summary>
        /// 是否显示日志
        /// </summary>
        [JsonPropertyName ("show-logs")]
        public bool ShowLogs { get; set; } = false;
    }
}
