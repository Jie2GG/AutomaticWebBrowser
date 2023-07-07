using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Wpf.Services.Configuration.Models
{
    /// <summary>
    /// <c>getElementsByNameNS 搜索元素的参数</c>
    /// </summary>
    class AWElementsNameNS
    {
        /// <summary>
        /// 规定要搜索的命名空间名称
        /// </summary>
        [JsonPropertyName ("ns")]
        public string Ns { get; set; } = "*";

        /// <summary>
        /// 规定要搜索的标签名
        /// </summary>
        [JsonPropertyName ("name")]
        public string Name { get; set; } = "*";

    }
}
