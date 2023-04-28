using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Domain.Configuration.Models
{
    /// <summary>
    /// 动作执行元素
    /// </summary>
    class AWElement
    {
        /// <summary>
        /// 元素类型
        /// </summary>
        [JsonPropertyName ("type")]
        [JsonConverter (typeof (JsonStringEnumConverter))]
        public AWElementType Type { get; set; } = AWElementType.None;

        /// <summary>
        /// 元素值
        /// </summary>
        [JsonPropertyName ("value")]
        public string? Value { get; set; } = null;
    }
}
