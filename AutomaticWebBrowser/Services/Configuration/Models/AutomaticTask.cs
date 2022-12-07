using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Services.Configuration.Models
{
    /// <summary>
    /// 自动化任务配置
    /// </summary>
    class AutomaticTask
    {
        /// <summary>
        /// 链接地址
        /// </summary>
        [JsonPropertyName ("url")]
        public string Url { get; set; }

        /// <summary>
        /// 操作列表
        /// </summary>
        [JsonPropertyName ("actions")]
        public Action[] Actions { get; set; }
    }
}
