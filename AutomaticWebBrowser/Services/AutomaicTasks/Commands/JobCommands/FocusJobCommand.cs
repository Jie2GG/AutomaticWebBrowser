using System;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Services.Configuration.Models;

using Gecko;

namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands.JobCommands
{
    [JobCommand (JobType.Focus)]
    class FocusJobCommand : JobCommand
    {
        public FocusJobCommand (WebView webView, GeckoNode node, Job job)
            : base (webView, node, job)
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
