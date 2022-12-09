using System;

using AutomaticWebBrowser.Services.Configuration.Models;

using Gecko;

using Serilog.Core;

namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands.JobCommands
{
    class DefaultJobCommand : JobCommand
    {
        public DefaultJobCommand (GeckoWebBrowser webView, GeckoNode node, Job job, Logger log)
            : base (webView, node, job, log)
        { }

        public override bool Execute ()
        {
            IAsyncResult asyncResult = this.WebView.BeginInvoke (new Func<bool> (() =>
            {
                this.Log.Information ($"JobCommand executed “default” job of node “{this.Node.NodeName}”.");
                return true;
            }));
            return this.WebView.EndInvoke (asyncResult) as bool? ?? false;
        }
    }
}
