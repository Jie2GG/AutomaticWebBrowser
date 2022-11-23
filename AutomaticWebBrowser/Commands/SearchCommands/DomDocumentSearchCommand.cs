using System;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

using Action = System.Action;

namespace AutomaticWebBrowser.Commands.DomSearchCommands
{
    [SearchCommand (SearchType.DomDocument)]
    public class DomDocumentSearchCommand : SearchCommand
    {
        public DomDocumentSearchCommand (TaskWebBrowser webBrowser, GeckoNode node, Models.Element element)
            : base (webBrowser, node, element)
        { }

        public override void Execute ()
        {
            IAsyncResult result = this.Browser.BeginInvoke (new Action (() =>
            {
                this.Log.Information ($"Search: Node: {this.Node.NodeName}, Type: DomDocument");
                if (this.Node is GeckoDocument document)
                {
                    this.SearchResult = new GeckoNode[] { document };
                }
                else
                {
                    throw new InvalidOperationException ($"{this.Node.NodeName} 不是 DomDocuemnt");
                }
            }));
            this.Browser.EndInvoke (result);
        }
    }
}
