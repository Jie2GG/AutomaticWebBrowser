using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Services.Configuration.Models
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
        [Category ("其他")]
        [DisplayName ("名称(name)")]
        [Description ("指定任务的名称 (可选)")]
        public string? Name { get; set; } = null;

        /// <summary>
        /// URL地址
        /// </summary>
        [JsonPropertyName ("url")]
        [Category ("基本")]
        [DisplayName ("地址(url)")]
        [Description ("在指定的网站执行任务")]
        public string Url { get; set; } = "about:blank";

        /// <summary>
        /// 作业列表
        /// </summary>
        [JsonPropertyName ("job")]
        [Browsable (false)]
        public AWJob[] Jobs { get; set; } = Array.Empty<AWJob> ();
    }
}
