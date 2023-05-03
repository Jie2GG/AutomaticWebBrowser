using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Domain.Configuration.Models;

using Serilog.Core;

namespace AutomaticWebBrowser.Domain.Tasks.Commands.ActionCommands
{
    /// <summary>
    /// <see cref="ActionCommand"/> 命令调度程序
    /// </summary>
    static class ActionCommandDispatcher
    {
        #region --字段--
        private static readonly IEnumerable<Type> types = Assembly.GetExecutingAssembly ()
            .GetTypes ()
            .Where (p => p.IsSubclassOf (typeof (ActionCommand)) && !p.IsAbstract && !p.IsInterface);
        #endregion

        #region --公开方法--
        public static ActionCommand Dispatcher (IWebView webView, Logger log, AWAction action, string? variableName, int? index)
        {
            foreach (Type type in types)
            {
                if (type.GetCustomAttribute<ActionCommandAttribute> ()?.ActionType == action.Type)
                {
                    return (ActionCommand)(Activator.CreateInstance (type, new object?[] { webView, log, action, variableName, index }) ?? new DefaultActionCommand (webView, log, action, variableName, index));
                }
            }

            return new DefaultActionCommand (webView, log, action, variableName, index);
        }
        #endregion
    }
}
