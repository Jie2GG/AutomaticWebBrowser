using System;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Services.Configuration.Models;
using AutomaticWebBrowser.Views;

using Gecko;

using Serilog.Core;

namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands.JobCommands
{
    [JobCommand (JobType.None)]
    class DefaultJobCommand : JobCommand
    {
        public DefaultJobCommand (BrowserForm form, GeckoNode node, Job job, Logger log)
            : base (form, node, job, log)
        { }

        public override bool Execute ()
        {
            IAsyncResult asyncResult = this.Browser.BeginInvoke (new Func<bool> (() =>
            {
                this.Log.Information ($"自动化任务 --> 在节点 {this.Node.NodeName} 执行 “default” 作业");
                return true;
            }));
            return this.Browser.EndInvoke (asyncResult) as bool? ?? false;
        }
    }
}
