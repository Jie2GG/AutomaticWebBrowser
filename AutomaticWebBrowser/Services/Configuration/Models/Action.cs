using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Services.Configuration.Models
{
    /// <summary>
    /// 操作配置
    /// </summary>
    class Action
    {
        /// <summary>
        /// 操作名称
        /// </summary>
        [JsonPropertyName ("name")]
        public string Name { get; set; } = null;

        /// <summary>
        /// 执行条件
        /// </summary>
        [JsonPropertyName ("condition")]
        public Condition Condition { get; set; }

        /// <summary>
        /// 操作的元素
        /// </summary>
        [JsonPropertyName ("element")]
        public Element Element { get; set; }

        /// <summary>
        /// 工作列表
        /// </summary>
        [JsonPropertyName ("jobs")]
        public Job[] Jobs { get; set; }
    }
}
