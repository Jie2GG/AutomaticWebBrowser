using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Services.Configuration.Models
{
    /// <summary>
    /// 条件配置
    /// </summary>
    class Condition
    {
        /// <summary>
        /// 条件类型
        /// </summary>
        [JsonPropertyName ("type")]
        [JsonConverter (typeof (JsonStringEnumConverter))]
        public ConditionType Type { get; set; }

        /// <summary>
        /// 条件值
        /// </summary>
        [JsonPropertyName ("value")]
        public JsonElement Value { get; set; }
    }
}
