using System;
using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Services.Configuration.Models
{
    /// <summary>
    /// 配置文件
    /// </summary>
    class AWConfig
    {
        /// <summary>
        /// 浏览器
        /// </summary>
        [JsonPropertyName ("browser")]
        public AWBrowser Browser { get; set; } = new AWBrowser ();

        /// <summary>
        /// 日志
        /// </summary>
        [JsonPropertyName ("log")]
        public AWLog Log { get; set; } = new AWLog ();

        /// <summary>
        /// 任务列表
        /// </summary>
        [JsonPropertyName ("task")]
        public AWTask[] Tasks { get; set; } = Array.Empty<AWTask> ();
    }
}
