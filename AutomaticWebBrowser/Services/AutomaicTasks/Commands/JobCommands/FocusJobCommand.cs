using System;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Services.Configuration.Models;

using Gecko;

using Serilog.Core;

namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands.JobCommands
{
    [JobCommand (JobType.Focus)]
    class FocusJobCommand : JobCommand
    {
        public FocusJobCommand (GeckoWebBrowser webView, GeckoNode node, Job job, Logger log)
            : base (webView, node, job, log)
        { }

        public override bool Execute ()
        {
            IAsyncResult asyncResult = this.WebView.BeginInvoke (new Func<bool> (() =>
            {
                if (this.Node is GeckoHtmlElement htmlElement)
                {
                    htmlElement.Focus ();
                    this.Log.Information ($"JobCommand executed “focus” job of node “{this.Node.NodeName}”.");
                    return true;
                }
                else
                {
                    this.Log.Warning ($"JobCommand executed “focus” job of node “{this.Node.NodeName}”, but node is not “HtmlDocument”.");
                }

                return false;
            }));
            return this.WebView.EndInvoke (asyncResult) as bool? ?? false;
        }
    }
}
