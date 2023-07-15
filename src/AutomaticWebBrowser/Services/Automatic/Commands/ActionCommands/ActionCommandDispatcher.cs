using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AutomaticWebBrowser.Core;
using AutomaticWebBrowser.Services.Configuration.Models;

using Serilog;

namespace AutomaticWebBrowser.Services.Automatic.Commands.ActionCommands
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
        public static ActionCommand Dispatcher (IWebView webView, ILogger logger, AWAction action, string? variableName, int? index)
        {
            foreach (Type type in types)
            {
                if (type.GetCustomAttribute<ActionCommandAttribute> ()?.ActionType == action.Type)
                {
                    return (ActionCommand)(Activator.CreateInstance (type, new object?[] { webView, logger, action, variableName, index }) ?? new DefaultActionCommand (webView, logger, action, variableName, index));
                }
            }

            return new DefaultActionCommand (webView, logger, action, variableName, index);
        }
        #endregion
    }
}
