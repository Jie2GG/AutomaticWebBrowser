using System;
using System.Linq;
using System.Text.Json;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Services.Configuration.Models;

using Gecko;

using Serilog.Core;

namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands.SearchCommands
{
    [SearchCommand (SearchMode.EvaluateXPath)]
    class EvaluateXPathSearchCommand : SearchCommand
    {
        public EvaluateXPathSearchCommand (GeckoWebBrowser webView, GeckoNode sourceNode, Configuration.Models.Element element, Logger log)
            : base (webView, sourceNode, element, log)
        { }

        public override GeckoNode[] Execute ()
        {
            IAsyncResult asyncResult = WebView.BeginInvoke (new Func<GeckoNode[]> (() =>
            {
                if (this.Element.SearchValue.ValueKind == JsonValueKind.String)
                {
                    string searchValue = this.Element.SearchValue.GetString ();
                    Gecko.DOM.XPathResult xPathResult = this.SourceNode.EvaluateXPath (searchValue);
                    if (xPathResult.GetResultType () == Gecko.DOM.XPathResultType.UnorderedNodeIterator || xPathResult.GetResultType () == Gecko.DOM.XPathResultType.OrderedNodeIterator)
                    {
                        GeckoNode[] result = xPathResult.GetNodes ().ToArray ();
                        this.Log.Information ($"SearchCommand executed “evaluate” command of source node “{this.SourceNode.NodeName}”, search value is “{searchValue}”, result count is “{result.Length}”.");
                        return result;
                    }
                    else
                    {
                        this.Log.Warning ($"SearchCommand executed “evaluate” command of source node “{this.SourceNode.NodeName}”, search value is “{searchValue}”, but result type is not node iterator.");
                    }
                }
                else
                {
                    this.Log.Warning ($"SearchCommand executed “evaluate” command of source node “{this.SourceNode.NodeName}”, but search value type is not string.");
                }

                return null;
            }));

            return this.WebView.EndInvoke (asyncResult) as GeckoNode[];
        }
    }
}
