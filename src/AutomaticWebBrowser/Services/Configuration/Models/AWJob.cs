using System;
using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Wpf.Services.Configuration.Models
{
    /// <summary>
    /// 作业
    /// </summary>
    class AWJob
    {
        /// <summary>
        /// 作业名称
        /// </summary>
        [JsonPropertyName ("name")]
        public string? Name { get; set; }

        /// <summary>
        /// 动作执行条件
        /// </summary>
        [JsonPropertyName ("condition")]
        public AWCondition? Condition { get; set; } = null;

        /// <summary>
        /// 动作执行内嵌框架
        /// </summary>
        [JsonPropertyName ("iframe")]
        public AWIframe? Iframe { get; set; } = null;

        /// <summary>
        /// 动作执行元素
        /// </summary>
        [JsonPropertyName ("element")]
        public AWElement? Element { get; set; } = null;

        /// <summary>
        /// 动作列表
        /// </summary>
        [JsonPropertyName ("action")]
        public AWAction[] Actions { get; set; } = Array.Empty<AWAction> ();
    }
}
