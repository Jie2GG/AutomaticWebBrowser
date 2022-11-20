using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Models
{
    /// <summary>
    /// 操作类
    /// </summary>
    public class Option
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        [JsonPropertyName ("type")]
        [JsonConverter (typeof (JsonStringEnumConverter))]
        public OptionType Type { get; set; }

        /// <summary>
        /// 操作附加的值
        /// </summary>
        [JsonPropertyName ("value")]
        public JsonElement Value { get; set; }
    }
}
