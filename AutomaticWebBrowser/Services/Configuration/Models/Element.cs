using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Services.Configuration.Models
{
    /// <summary>
    /// 元素配置
    /// </summary>
    class Element
    {
        /// <summary>
        /// 元素搜索模式
        /// </summary>
        [JsonPropertyName ("search-mode")]
        [JsonConverter (typeof (JsonStringEnumConverter))]
        public SearchMode SearchMode { get; set; }

        /// <summary>
        /// 用于搜索的值
        /// </summary>
        [JsonPropertyName ("search-value")]
        public JsonElement SearchValue { get; set; }
    }
}
