using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Models
{
    /// <summary>
    /// 条件
    /// </summary>
    public class Condition
    {
        /// <summary>
        /// 条件判别类型
        /// </summary>
        [JsonPropertyName ("type")]
        [JsonConverter (typeof (JsonStringEnumConverter))]
        public ConditionType Type { get; set; }

        /// <summary>
        /// 判别的值
        /// </summary>
        [JsonPropertyName ("value")]
        public JsonElement Value { get; set; }
    }
}
