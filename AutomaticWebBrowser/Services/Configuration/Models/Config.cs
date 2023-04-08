using System.Configuration.Provider;
using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Services.Configuration.Models
{
    /// <summary>
    /// 配置文件根
    /// </summary>
    class Config
    {
        /// <summary>
        /// 浏览器配置
        /// </summary>
        [JsonPropertyName ("browser")]
        public Browser Browser { get; set; } = new Browser ();

        /// <summary>
        /// 日志配置
        /// </summary>
        [JsonPropertyName ("log")]
        public Log Log { get; set; } = new Log ();

        /// <summary>
        /// 任务配置
        /// </summary>
        [JsonPropertyName ("task")]
        public AutomaticTask[] Tasks { get; set; }

        /// <summary>
        /// 检查配置文件内容的合理性
        /// </summary>
        public void Check ()
        {
            if (this.Tasks == null)
            {
                throw new ProviderException ("未找到自动化任务的配置描述信息, 属性名: “task”");
            }
        }
    }
}
