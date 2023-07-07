using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Wpf.Services.Configuration.Models
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
        public JsonElement? Value { get; set; } = null;
    }
}
