using System;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Services.Configuration.Models;

using Gecko;

namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands.JobCommands
{
    [JobCommand (JobType.Click)]
    class ClickJobCommand : JobCommand
    {
        public ClickJobCommand (WebView webView, GeckoNode node, Job job)
            : base (webView, node, job)
        { }

        public override bool Execute ()
        {
            IAsyncResult asyncResult = this.WebView.BeginInvoke (new Func<bool> (() =>
            {
                if (this.Node is GeckoHtmlElement htmlElement)
                {
                    htmlElement.Click ();
                    this.Log.Information ($"JobCommand executed “click” job of node “{this.Node.NodeName}”.");
                    return true;
                }
                else
                {
                    this.Log.Warning ($"JobCommand executed “click” job of node “{this.Node.NodeName}”, but node is not “HtmlElement”.");
                }

                return false;
            }));
            return this.WebView.EndInvoke (asyncResult) as bool? ?? false;
        }
    }
}
