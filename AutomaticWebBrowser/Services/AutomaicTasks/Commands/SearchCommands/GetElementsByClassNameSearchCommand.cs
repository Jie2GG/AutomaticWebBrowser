using System;
using System.Linq;
using System.Text.Json;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Services.Configuration.Models;

using Gecko;

using Serilog.Core;

namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands.SearchCommands
{
    [SearchCommand (SearchMode.GetElementsByClassName)]
    class GetElementsByClassNameSearchCommand : SearchCommand
    {
        public GetElementsByClassNameSearchCommand (GeckoWebBrowser webView, GeckoNode sourceNode, Configuration.Models.Element element, Logger log)
            : base (webView, sourceNode, element, log)
        { }

        public override GeckoNode[] Execute ()
        {
            IAsyncResult asyncResult = WebView.BeginInvoke (new Func<GeckoNode[]> (() =>
            {
                if (this.SourceNode is GeckoDocument domDocument)
                {
                    if (this.Element.SearchValue.ValueKind == JsonValueKind.String)
                    {
                        string searchValue = this.Element.SearchValue.GetString ();
                        GeckoNode[] result = domDocument.GetElementsByClassName (searchValue).ToArray ();
                        this.Log.Information ($"SearchCommand executed “getElementsByClassName” command of source node “{this.SourceNode.NodeName}”, search value is “{searchValue}”, result count is “{result.Length}”");
                        return result;
                    }
                    else
                    {
                        this.Log.Warning ($"SearchCommand executed “getElementsByClassName” command of source node “{this.SourceNode.NodeName}”, but search value type is not string.");
                    }
                }
                else
                {
                    this.Log.Warning ($"SearchCommand executed “getElementsByClassName” command of source node “{this.SourceNode.NodeName}”, but source node type is not “Document”.");
                }

                return null;
            }));

            return this.WebView.EndInvoke (asyncResult) as GeckoNode[];
        }
    }
}
