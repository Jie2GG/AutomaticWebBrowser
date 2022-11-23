using System;
using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Models
{
    /// <summary>
    /// 动作类
    /// </summary>
    public class Action
    {
        /// <summary>
        /// 动作名字
        /// </summary>
        [JsonPropertyName ("name")]
        public string Name { get; set; } = null;

        /// <summary>
        /// 执行条件
        /// </summary>
        [JsonPropertyName ("condition")]
        public Condition Condition { get; set; } = null;

        /// <summary>
        /// 元素
        /// </summary>
        [JsonPropertyName ("element")]
        public Element Element { get; set; }

        /// <summary>
        /// 操作信息组
        /// </summary>
        [JsonPropertyName ("operations")]
        public Operation[] Operations { get; set; } = Array.Empty<Operation> ();
    }
}
