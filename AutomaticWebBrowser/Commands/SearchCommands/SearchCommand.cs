using System;
using System.Linq;
using System.Reflection;

using AutomaticWebBrowser.Commands.OperationCommands;
using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

using Serilog;

namespace AutomaticWebBrowser.Commands.DomSearchCommands
{
    /// <summary>
    /// DOM 搜索命令抽象类
    /// </summary>
    public abstract class SearchCommand : ICommand
    {
        #region --字段--
        private static readonly Type[] domSearchTypes = Assembly.GetExecutingAssembly ()
            .GetTypes ()
            .Where (p => p.IsSubclassOf (typeof (SearchCommand)) && !p.IsAbstract && !p.IsInterface)
            .ToArray ();
        #endregion

        #region --属性--
        public TaskWebBrowser Browser { get; }
        public GeckoNode Node { get; }
        public Models.Element Element { get; }
        public ILogger Log => this.Browser.Log;

        public GeckoNode[] SearchResult { get; protected set; }
        #endregion

        #region --构造函数--
        protected SearchCommand (TaskWebBrowser webBrowser, GeckoNode node, Models.Element element)
        {
            this.Browser = webBrowser;
            this.Node = node;
            this.Element = element;
        }
        #endregion

        #region --公开方法--
        public abstract void Execute ();

        public static SearchCommand CreateCommand (TaskWebBrowser browser, GeckoNode node, Models.Element element)
        {
            foreach (Type optionType in domSearchTypes)
            {
                SearchCommandAttribute attribute = optionType.GetCustomAttribute<SearchCommandAttribute> ();
                if (attribute != null && attribute.SearchType == element.SearchType)
                {
                    return (SearchCommand)Activator.CreateInstance (optionType, new object[] { browser, node, element });
                }
            }
            return new DefaultSearchCommand (browser, node, element);
        }
        #endregion
    }
}
