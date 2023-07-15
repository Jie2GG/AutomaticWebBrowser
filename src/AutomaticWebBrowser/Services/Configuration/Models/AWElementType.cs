using AutomaticWebBrowser.Services.Configuration.Attributes;
using AutomaticWebBrowser.Services.Configuration.Converter;

namespace AutomaticWebBrowser.Services.Configuration.Models
{
    /// <summary>
    /// 元素类型
    /// </summary>
    enum AWElementType
    {
        /// <summary>
        /// 表示不执行任何操作
        /// </summary>
        None = 0,
        /// <summary>
        /// 表示使用 window 对象
        /// </summary>
        Window = 1,
        /// <summary>
        /// 表示使用 document 对象
        /// </summary>
        Document = 2,
        /// <summary>
        /// 使用 <c>document.getElementById</c> 函数进行搜索
        /// </summary>
        [ObjectConverter (typeof (StringObjectConverter))]
        GetElementById = 3,
        /// <summary>
        /// 使用 <c>document.getElementsByName</c> 函数进行搜索
        /// </summary>
        [ObjectConverter (typeof (StringObjectConverter))]
        GetElementsByName = 4,
        /// <summary>
        /// 使用 <c>document.getElementsByTagName</c> 函数进行搜索
        /// </summary>
        [ObjectConverter (typeof (StringObjectConverter))]
        GetElementsByTagName = 5,
        /// <summary>
        /// 使用 <c>document.getElementsByTagNameNS</c> 函数进行搜索
        /// </summary>
        [ObjectConverter (typeof (ElementsNameNSObjectConverter))]
        GetElementsByTagNameNS = 6,
        /// <summary>
        /// 使用 <c>document.getElementsByClassName</c> 函数进行搜索
        /// </summary>
        [ObjectConverter (typeof (StringObjectConverter))]
        GetElementsByClassName = 7,
        /// <summary>
        /// 使用 <c>document.evaluate</c> 函数进行搜索
        /// </summary>
        [ObjectConverter (typeof (StringObjectConverter))]
        Evaluate = 8
    }
}
