using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Services.Configuration.Models
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
        [Category ("其他")]
        [DisplayName ("名称(name)")]
        [Description ("指定作业的名称 (可选)")]
        public string? Name { get; set; }

        /// <summary>
        /// 动作执行条件
        /// </summary>
        [JsonPropertyName ("condition")]
        [Browsable (false)]
        public AWCondition? Condition { get; set; } = null;

        /// <summary>
        /// 动作执行内嵌框架
        /// </summary>
        [JsonPropertyName ("iframe")]
        [Browsable (false)]
        public AWIframe? Iframe { get; set; } = null;

        /// <summary>
        /// 动作执行元素
        /// </summary>
        [JsonPropertyName ("element")]
        [Browsable (false)]
        public AWElement? Element { get; set; } = null;

        /// <summary>
        /// 动作列表
        /// </summary>
        [JsonPropertyName ("action")]
        [Browsable (false)]
        public AWAction[] Actions { get; set; } = Array.Empty<AWAction> ();
    }
}
