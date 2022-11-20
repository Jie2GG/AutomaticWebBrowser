namespace AutomaticWebBrowser.Models
{
    /// <summary>
    /// 元素搜索类型
    /// </summary>
    public enum SearchType
    {
        /// <summary>
        /// DOM文档
        /// </summary>
        DomDocument,
        /// <summary>
        /// 使用 XPath
        /// </summary>
        XPath,
        /// <summary>
        /// 使用 ID 查找
        /// </summary>
        GetById
    }
}
