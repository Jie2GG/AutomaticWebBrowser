using System.Text.Json.Serialization;

using Serilog.Events;

namespace AutomaticWebBrowser.Wpf.Services.Configuration.Models
{
    /// <summary>
    /// 日志
    /// </summary>
    class AWLog
    {
        /// <summary>
        /// 格式化模板
        /// </summary>
        [JsonPropertyName ("format")]
        public string Format { get; set; } = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        /// <summary>
        /// 日志等级
        /// </summary>
        [JsonPropertyName ("level")]
        [JsonConverter (typeof (JsonStringEnumConverter))]
        public LogEventLevel Level { get; set; } = LogEventLevel.Information;

        /// <summary>
        /// 保存路径
        /// </summary>
        [JsonPropertyName ("save-path")]
        public string SavePath { get; set; } = "./logs";
    }
}
