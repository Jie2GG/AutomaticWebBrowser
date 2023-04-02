using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

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
        public GeckoWebBrowser WebView { get; }

        public GeckoNode Node { get; }

        public string NodeName { get; private set; }

        public Job Job { get; }

        public Logger Log { get; }

        public WebViewTabControl TabConrol { get; private set; }
        #endregion

        #region --构造函数--
        protected JobCommand (GeckoWebBrowser webView, GeckoNode node, Job job, Logger log)
        {
            this.WebView = webView ?? throw new ArgumentNullException (nameof (webView));
            this.Node = node ?? throw new ArgumentNullException (nameof (node));
            this.Job = job ?? throw new ArgumentNullException (nameof (job));
            this.Log = log;

            this.WebView.Invoke (() =>
            {
                this.NodeName = this.Node.NodeName;
            });
        }
        #endregion

        #region --公开方法--
        public JobCommand SetTabControl (WebViewTabControl tabControls)
        {
            this.TabConrol = tabControls ?? throw new ArgumentNullException (nameof (tabControls));
            return this;
        }

        public abstract bool Execute ();

        public static JobCommand CreateCommand (GeckoWebBrowser webView, GeckoNode node, Job job, Logger log)
        {
            foreach (Type commandType in typeEnumerable)
            {
                JobCommandAttribute attribute = commandType.GetCustomAttribute<JobCommandAttribute> ();
                if (attribute != null && attribute.Type == job.Type)
                {
                    return (JobCommand)Activator.CreateInstance (commandType, new object[] { webView, node, job, log });
                }
            }
            return new DefaultJobCommand (webView, node, job, log);
        }
        #endregion
    }
}
