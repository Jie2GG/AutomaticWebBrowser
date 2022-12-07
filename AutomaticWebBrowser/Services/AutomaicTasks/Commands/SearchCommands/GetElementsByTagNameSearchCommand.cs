using System;
using System.Linq;
using System.Text.Json;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Services.Configuration.Models;

using Gecko;

namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands.SearchCommands
{
    [SearchCommand (SearchMode.GetElementsByTagName)]
    class GetElementsByTagNameSearchCommand : SearchCommand
    {
        public GetElementsByTagNameSearchCommand (WebView webView, GeckoNode sourceNode, Configuration.Models.Element element)
            : base (webView, sourceNode, element)
        { }

        public override GeckoNode[] Execute ()
        {
            IAsyncResult asyncResult = WebView.BeginInvoke (new Func<GeckoNode[]> (() =>
            {
                if (this.SourceNode is GeckoElement element)
                {
                    if (this.Element.SearchValue.ValueKind == JsonValueKind.String)
                    {
                        string searchValue = this.Element.SearchValue.GetString ();
                        GeckoElement[] result = element.GetElementsByTagName (searchValue).ToArray ();
                        this.Log.Information ($"SearchCommand executed “getElementsByTagName” command of source node “{this.SourceNode.NodeName}”, search value is “{searchValue}”, result count is “{result.Length}”");
                        return result;
                    }
                    else
                    {
                        this.Log.Warning ($"SearchCommand executed “getElementsByTagName” command of source node “{this.SourceNode.NodeName}”, but search value type is not string.");
                    }
                }
                else
                {
                    this.Log.Warning ($"SearchCommand executed “getElementsByTagName” command of source node “{this.SourceNode.NodeName}”, but source node type is not “Element”.");
                }

                return null;
            }));

            return this.WebView.EndInvoke (asyncResult) as GeckoNode[];
        }
    }
}
