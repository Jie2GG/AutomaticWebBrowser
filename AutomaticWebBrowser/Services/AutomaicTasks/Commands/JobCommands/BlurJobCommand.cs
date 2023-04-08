using System;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Services.Configuration.Models;
using AutomaticWebBrowser.Views;

using Gecko;

using Serilog.Core;

namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands.JobCommands
{
    [JobCommand (JobType.Blur)]
    class BlurJobCommand : JobCommand
    {
        public BlurJobCommand (BrowserForm form, GeckoNode node, Job job, Logger log)
            : base (form, node, job, log)
        { }

        public override bool Execute ()
        {
            IAsyncResult asyncResult = this.Browser.BeginInvoke (new Func<bool> (() =>
            {
                if (this.Node is GeckoHtmlElement htmlElement)
                {
                    htmlElement.Blur ();
                    this.Log.Information ($"自动化任务 --> 在节点: {this.Node.NodeName} 执行 “blur” 作业");
                    return true;
                }
                else
                {
                    this.Log.Warning ($"自动化任务 --> 在节点: {this.Node.NodeName} 执行 “blur” 作业, 但节点不是 Htmlelement 类型");
                }

                return false;
            }));
            return this.Browser.EndInvoke (asyncResult) as bool? ?? false;
        }
    }
}
