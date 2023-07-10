using System.ComponentModel;
using System.Text.Json.Serialization;

using Serilog.Events;

namespace AutomaticWebBrowser.Services.Configuration.Models
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
        [Category ("基本")]
        [DisplayName ("输出格式(format)")]
        [Description ("指定运行日志的输出格式化模板, 具体格式参考 Serilog")]
        public string Format { get; set; } = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        /// <summary>
        /// 日志等级
        /// </summary>
        [JsonPropertyName ("level")]
        [JsonConverter (typeof (JsonStringEnumConverter))]
        [Category ("基本")]
        [DisplayName ("日志等级(level)")]
        [Description ("指定运行日志捕获的最低级别")]
        public LogEventLevel Level { get; set; } = LogEventLevel.Information;

        /// <summary>
        /// 保存路径
        /// </summary>
        [JsonPropertyName ("save-path")]
        [DisplayName ("保存位置(save-path)")]
        [Description ("指定运行日志的保存位置")]
        public string SavePath { get; set; } = "./logs";
    }
}
