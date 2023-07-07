using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

using AutomaticWebBrowser.Wpf.Core;
using AutomaticWebBrowser.Wpf.Services.Configuration.Models;

using Serilog;

namespace AutomaticWebBrowser.Wpf.Services.Automatic.Commands.ElementCommands
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
        public static ElementCommand Dispatcher (IWebView webView, ILogger log, AWElement element)
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

        public static ElementCommand Dispatcher (IWebView webView, ILogger log, AWElement element, string? variableName)
        {
            foreach (Type type in types)
            {
                if (type.GetCustomAttribute<ElementCommandAttribute> ()?.ElementType == element.Type)
                {
                    return (ElementCommand)(Activator.CreateInstance (type, new object?[] { webView, log, element, variableName }) ?? new DefaultElementCommand (webView, log, element, variableName));
                }
            }
            return new DefaultElementCommand (webView, log, element, variableName);
        }
        #endregion
    }
}
