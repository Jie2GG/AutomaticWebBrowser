using System;
using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Wpf.Services.Configuration.Models
{
    /// <summary>
    /// 任务
    /// </summary>
    class AWTask
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        [JsonPropertyName ("name")]
        public string? Name { get; set; } = null;

        /// <summary>
        /// URL地址
        /// </summary>
        [JsonPropertyName ("url")]
        public string Url { get; set; } = "about:blank";

        /// <summary>
        /// 作业列表
        /// </summary>
        [JsonPropertyName ("job")]
        public AWJob[] Jobs { get; set; } = Array.Empty<AWJob> ();
    }
}
