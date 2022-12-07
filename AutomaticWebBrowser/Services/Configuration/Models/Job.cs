using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Services.Configuration.Models
{
    /// <summary>
    /// 作业配置
    /// </summary>
    class Job
    {
        /// <summary>
        /// 作业类型
        /// </summary>
        [JsonPropertyName ("type")]
        [JsonConverter (typeof (JsonStringEnumConverter))]
        public JobType Type { get; set; }

        /// <summary>
        /// 作业相关信息
        /// </summary>
        [JsonPropertyName ("value")]
        public JsonElement Value { get; set; }

        /// <summary>
        /// 作业延迟
        /// </summary>
        [JsonPropertyName ("delay")]
        public int Delay { get; set; }
    }
}
