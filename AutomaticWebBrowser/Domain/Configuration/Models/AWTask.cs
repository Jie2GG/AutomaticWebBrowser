using System;
using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Domain.Configuration.Models
{
    /// <summary>
    /// 任务
    /// </summary>
    class AWTask
    {
        /// <summary>
        /// URL地址
        /// </summary>
        [JsonPropertyName ("url")]
        public string Url { get; set; } = "about:blank";

        /// <summary>
        /// URL失败重载次数
        /// </summary>
        [JsonPropertyName ("url-reload-count")]
        public int UrlReloadCount { get; set; } = 0;

        /// <summary>
        /// URL失败重载延迟
        /// </summary>
        [JsonPropertyName ("url-reload-delay")]
        public int UrlReloadDelay { get; set; } = 3000;

        /// <summary>
        /// 自动关闭
        /// </summary>
        [JsonPropertyName ("auto-close")]
        public bool AutoClose { get; set; } = true;

        /// <summary>
        /// 作业列表
        /// </summary>
        [JsonPropertyName ("jobs")]
        public AWJob[] Jobs { get; set; } = Array.Empty<AWJob> ();
    }
}
