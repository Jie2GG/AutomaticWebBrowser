using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Services.Configuration.Models;
using AutomaticWebBrowser.Views;

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
        public BrowserForm Form { get; }

        public GeckoWebBrowser Browser { get; }

        public GeckoNode Node { get; }

        public Job Job { get; }

        public Logger Log { get; }
        #endregion

        #region --构造函数--
        protected JobCommand(BrowserForm form, GeckoNode node, Job job, Logger log)
        {
            this.Form = form ?? throw new ArgumentNullException (nameof (form));
            this.Browser = form.AutomaticWebBrowser ?? throw new ArgumentNullException (nameof (form));
            this.Node = node ?? throw new ArgumentNullException (nameof (node));
            this.Job = job ?? throw new ArgumentNullException (nameof (job));
            this.Log = log;
        }
        #endregion

        #region --公开方法--
        public abstract bool Execute ();

        public static JobCommand CreateCommand (BrowserForm form, GeckoNode node, Job job, Logger log)
        {
            foreach (Type commandType in typeEnumerable)
            {
                JobCommandAttribute attribute = commandType.GetCustomAttribute<JobCommandAttribute> ();
                if (attribute != null && attribute.Type == job.Type)
                {
                    return (JobCommand)Activator.CreateInstance (commandType, new object[] { form, node, job, log });
                }
            }
            return new DefaultJobCommand (form, node, job, log);
        }
        #endregion
    }
}
