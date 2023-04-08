using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Services.Configuration.Models
{
    /// <summary>
    /// 子任务配置
    /// </summary>
    class SubTask
    {
        /// <summary>
        /// 子任务的动作
        /// </summary>
        [JsonPropertyName ("actions")]
        public Action[] Actions { get; set; }
    }
}
