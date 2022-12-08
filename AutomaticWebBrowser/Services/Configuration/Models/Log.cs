﻿using System.Text.Json.Serialization;

using Serilog.Events;

namespace AutomaticWebBrowser.Services.Configuration.Models
{
    /// <summary>
    /// 日志配置
    /// </summary>
    class Log
    {
        /// <summary>
        /// 日志窗口
        /// </summary>
        [JsonPropertyName ("window")]
        public Window Window { get; set; } = new Window ()
        {
            Size = new WindowSize (800, 300),
            Visible = false
        };

        /// <summary>
        /// 日志格式
        /// </summary>
        [JsonPropertyName ("format")]
        public string Format { get; set; } = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        /// <summary>
        /// 日志等级
        /// </summary>
        [JsonPropertyName ("level")]
        [JsonConverter (typeof (JsonStringEnumConverter))]
        public LogEventLevel Level { get; set; } = LogEventLevel.Warning;

        /// <summary>
        /// 日志路径
        /// </summary>
        [JsonPropertyName ("save-path")]
        public string SavePath { get; set; } = "./logs";
    }
}