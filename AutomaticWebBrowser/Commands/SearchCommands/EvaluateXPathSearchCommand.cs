using System;
using System.Linq;
using System.Text.Json;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

using Action = System.Action;

namespace AutomaticWebBrowser.Commands.DomSearchCommands
{
    [SearchCommand (SearchType.EvaluateXPath)]
    public class EvaluateXPathSearchCommand : SearchCommand
    {
        public EvaluateXPathSearchCommand (TaskWebBrowser webBrowser, GeckoNode node, Models.Element element)
            : base (webBrowser, node, element)
        { }

        public override void Execute ()
        {
            string xpath = this.Element.Value.Deserialize<string> ();
            IAsyncResult result = this.Browser.BeginInvoke (new Action (() =>
            {
                this.Log.Information ($"Search: Node: {this.Node.NodeName}, Type: EvaluateXPath");

                Gecko.DOM.XPathResult xPathResult = this.Node.EvaluateXPath (xpath);
                if (xPathResult.GetResultType () == Gecko.DOM.XPathResultType.UnorderedNodeIterator)
                {
                    this.SearchResult = xPathResult.GetNodes ().ToArray ();
                }
            }));
            this.Browser.EndInvoke (result);
        }
    }
}
