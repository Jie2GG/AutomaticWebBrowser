using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Domain.Configuration.Models;

using Serilog.Core;

namespace AutomaticWebBrowser.Domain.Tasks.Commands.ElementCommands
{
    /// <summary>
    /// <see cref="ElementCommand"/> 命令调度程序
    /// </summary>
    static class ElementCommandDispatcher
    {
        #region --字段--
        private static readonly IEnumerable<Type> types = Assembly.GetExecutingAssembly ()
            .GetTypes ()
            .Where (p => p.IsSubclassOf (typeof (ElementCommand)) && !p.IsAbstract && !p.IsInterface);
        #endregion

        #region --公开方法--
        public static ElementCommand Dispatcher (IWebView webView, Logger log, AWElement element)
        {
            foreach (Type type in types)
            {
                if (type.GetCustomAttribute<ElementCommandAttribute> ()?.ElementType == element.Type)
                {
                    return (ElementCommand)(Activator.CreateInstance (type, new object?[] { webView, log, element }) ?? new DefaultElementCommand (webView, log, element));
                }
            }
            return new DefaultElementCommand (webView, log, element);
        }
        #endregion
    }
}
