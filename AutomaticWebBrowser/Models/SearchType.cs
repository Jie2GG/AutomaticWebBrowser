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
        /// 使用 XPath
        /// </summary>
        XPath,
        /// <summary>
        /// 使用元素 Id
        /// </summary>
        ElementId,
        /// <summary>
        /// 使用元素 Id 下的所有子节点
        /// </summary>
        ElementIdAllChild,
    }
}
