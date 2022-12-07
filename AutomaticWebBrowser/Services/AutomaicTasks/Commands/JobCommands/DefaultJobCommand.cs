using System;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Services.Configuration.Models;

using Gecko;

namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands.JobCommands
{
    class DefaultJobCommand : JobCommand
    {
        public DefaultJobCommand (WebView webView, GeckoNode node, Job job)
            : base (webView, node, job)
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
