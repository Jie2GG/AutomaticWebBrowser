namespace AutomaticWebBrowser.Models
{
    /// <summary>
    /// 元素搜索类型
    /// </summary>
    public enum SearchType
    {
        /// <summary>
        /// DOM 文档
        /// </summary>
        DomDocument,
        /// <summary>
        /// 评估 XPath
        /// </summary>
        EvaluateXPath,
        /// <summary>
        /// 通过 Id 获取元素
        /// </summary>
        GetElementById,
        /// <summary>
        /// 通过 Id 获取子元素
        /// </summary>
        GetChildElementById,
    }
}
