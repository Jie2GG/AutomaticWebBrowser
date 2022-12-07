using System;

using AutomaticWebBrowser.Controls;

using Gecko;

namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands.SearchCommands
{
    class DefaultSearchCommand : SearchCommand
    {
        public DefaultSearchCommand (WebView webView, GeckoNode sourceNode, Configuration.Models.Element element)
            : base (webView, sourceNode, element)
        { }

        public override GeckoNode[] Execute ()
        {
            IAsyncResult asyncResult = this.WebView.BeginInvoke (new Func<GeckoNode[]> (() =>
            {
                this.Log.Information ($"SearchCommand executed “default” command of source node “{this.SourceNode.NodeName}”.");
                return null;
            }));
            return this.WebView.EndInvoke (asyncResult) as GeckoNode[];
        }
    }
}
