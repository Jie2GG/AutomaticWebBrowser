using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Controls;

using Gecko;

using Serilog.Core;

namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands.SearchCommands
{
    abstract class SearchCommand : ICommand<GeckoNode[]>
    {
        #region --字段--
        private static readonly IEnumerable<Type> typeEnumerable = Assembly.GetExecutingAssembly ()
            .GetTypes ()
            .Where (p => p.IsSubclassOf (typeof (SearchCommand)) && !p.IsAbstract && !p.IsInterface);
        #endregion

        #region --属性--
        /// <summary>
        /// Web浏览器
        /// </summary>
        public WebView WebView { get; }

        /// <summary>
        /// 来源节点, 使用此节点进行操作
        /// </summary>
        public GeckoNode SourceNode { get; }

        /// <summary>
        /// 要查找的元素
        /// </summary>
        public Configuration.Models.Element Element { get; }

        /// <summary>
        /// 日志
        /// </summary>
        public Logger Log => this.WebView.Log;
        #endregion

        #region --构造函数--
        protected SearchCommand (WebView webView, GeckoNode sourceNode, Configuration.Models.Element element)
        {
            this.WebView = webView ?? throw new ArgumentNullException (nameof (webView));
            this.SourceNode = sourceNode ?? throw new ArgumentNullException (nameof (sourceNode));
            this.Element = element ?? throw new ArgumentNullException (nameof (element));
        }
        #endregion

        #region --公开方法--
        public abstract GeckoNode[] Execute ();

        public static SearchCommand CreateCommand (WebView webView, GeckoNode node, Configuration.Models.Element element)
        {
            foreach (Type commandType in typeEnumerable)
            {
                SearchCommandAttribute attribute = commandType.GetCustomAttribute<SearchCommandAttribute> ();
                if (attribute != null && attribute.Mode == element.SearchMode)
                {
                    return (SearchCommand)Activator.CreateInstance (commandType, new object[] { webView, node, element });
                }
            }
            return new DefaultSearchCommand (webView, node, element);
        }
        #endregion
    }
}
