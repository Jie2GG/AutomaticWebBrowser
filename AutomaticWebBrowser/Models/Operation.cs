using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Models
{
    /// <summary>
    /// 操作类
    /// </summary>
    public class Operation
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        [JsonPropertyName ("type")]
        [JsonConverter (typeof (JsonStringEnumConverter))]
        public OperationType Type { get; set; }

        /// <summary>
        /// 相关的值
        /// </summary>
        [JsonPropertyName ("value")]
        public JsonElement Value { get; set; }

        /// <summary>
        /// 子操作列表
        /// </summary>
        [JsonPropertyName ("sub-operations")]
        public Operation[] SubOperations { get; set; } = null;
    }
}
