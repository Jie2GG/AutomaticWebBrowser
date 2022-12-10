namespace AutomaticWebBrowser.Services.Configuration.Models
{
    /// <summary>
    /// 元素类型
    /// </summary>
    enum SearchMode
    {
        /// <summary>
        /// 不搜索元素
        /// </summary>
        None,
        /// <summary>
        /// 通过 id 查找 HTML 元素
        /// </summary>
        GetElementById,
        /// <summary>
        /// 通过标签名查找 HTML 元素
        /// </summary>
        GetElementsByTagName,
        /// <summary>
        /// 通过类名查找 HTML 元素
        /// </summary>
        GetElementsByClassName,
        /// <summary>
        /// 通过 XPath 查找
        /// </summary>
        EvaluateXPath,
        /// <summary>
        /// 通过查找 form 标签
        /// </summary>
        Forms,
    }
}
