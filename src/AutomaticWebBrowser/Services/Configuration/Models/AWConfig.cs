using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Services.Configuration.Models
{
    /// <summary>
    /// 配置文件
    /// </summary>
    class AWConfig
    {
        /// <summary>
        /// 日志
        /// </summary>
        [JsonPropertyName ("log")]
        [Browsable (false)]
        public AWLog Log { get; set; } = new AWLog ();

        /// <summary>
        /// 浏览器
        /// </summary>
        [JsonPropertyName ("browser")]
        [Browsable (false)]
        public AWBrowser Browser { get; set; } = new AWBrowser ();

        /// <summary>
        /// 任务列表
        /// </summary>
        [JsonPropertyName ("task")]
        [Browsable (false)]
        public AWTask[] Task { get; set; } = Array.Empty<AWTask> ();
    }
}
