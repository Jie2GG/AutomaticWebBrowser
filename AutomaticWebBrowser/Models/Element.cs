using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Models
{
    /// <summary>
    /// 元素
    /// </summary>
    public class Element
    {
        /// <summary>
        /// 搜索类型
        /// </summary>
        [JsonPropertyName ("type")]
        [JsonConverter (typeof (JsonStringEnumConverter))]
        public SearchType SearchType { get; set; }

        /// <summary>
        /// 用来搜索的值
        /// </summary>
        [JsonPropertyName ("value")]
        public JsonElement Value { get; set; }
    }
}
