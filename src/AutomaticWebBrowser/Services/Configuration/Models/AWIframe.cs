using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Wpf.Services.Configuration.Models
{
    /// <summary>
    /// 内嵌框架 iframe 元素查找
    /// </summary>
    class AWIframe : AWElement
    {
        /// <summary>
        /// iframe 的中查找的元素
        /// </summary>
        [JsonPropertyName ("element")]
        public AWElement? Element { get; set; }
    }
}
