using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Models
{
    /// <summary>
    /// 任务类
    /// </summary>
    public class TaskInfo
    {
        /// <summary>
        /// 任务地址
        /// </summary>
        [JsonPropertyName ("url")]
        public string Url { get; set; }

        /// <summary>
        /// 任务动作组
        /// </summary>
        [JsonPropertyName ("actions")]
        public Action[] Actions { get; set; }
    }
}
