using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Services.Configuration.Models
{
    /// <summary>
    /// 动作
    /// </summary>
    class AWAction
    {
        /// <summary>
        /// 动作类型
        /// </summary>
        [JsonPropertyName ("type")]
        [JsonConverter (typeof (JsonStringEnumConverter))]
        public AWActionType Type { get; set; } = AWActionType.None;

        /// <summary>
        /// 动作附件值
        /// </summary>
        [JsonPropertyName ("value")]
        public object? Value { get; set; } = null;
    }
}
