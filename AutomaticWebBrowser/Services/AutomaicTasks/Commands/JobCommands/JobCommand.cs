using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Services.Configuration.Models;

using Gecko;

using Serilog.Core;

namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands.JobCommands
{
    abstract class JobCommand : ICommand<bool>
    {
        #region --字段--
        private static readonly IEnumerable<Type> typeEnumerable = Assembly.GetExecutingAssembly ()
            .GetTypes ()
            .Where (p => p.IsSubclassOf (typeof (JobCommand)) && !p.IsAbstract && !p.IsInterface);
        #endregion

        #region --属性--
        public WebView WebView { get; }

        public GeckoNode Node { get; }

        public Job Job { get; }

        public Logger Log => this.WebView.Log;
        #endregion

        #region --构造函数--
        protected JobCommand (WebView webView, GeckoNode node, Job job)
        {
            this.WebView = webView ?? throw new ArgumentNullException (nameof (webView));
            this.Node = node ?? throw new ArgumentNullException (nameof (node));
            this.Job = job ?? throw new ArgumentNullException (nameof (job));
        }
        #endregion

        #region --公开方法--
        public abstract bool Execute ();

        public static JobCommand CreateCommand (WebView webView, GeckoNode node, Job job)
        {
            foreach (Type commandType in typeEnumerable)
            {
                JobCommandAttribute attribute = commandType.GetCustomAttribute<JobCommandAttribute> ();
                if (attribute != null && attribute.Type == job.Type)
                {
                    return (JobCommand)Activator.CreateInstance (commandType, new object[] { webView, node, job });
                }
            }
            return new DefaultJobCommand (webView, node, job);
        }
        #endregion
    }
}
