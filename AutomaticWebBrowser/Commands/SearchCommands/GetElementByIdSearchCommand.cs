using System;
using System.Text.Json;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

using Action = System.Action;

namespace AutomaticWebBrowser.Commands.SearchCommands
{
    [SearchCommand (SearchType.GetElementById)]
    public class GetElementByIdSearchCommand : SearchCommand
    {
        public GetElementByIdSearchCommand (TaskWebBrowser webBrowser, GeckoNode node, Models.Element element)
            : base (webBrowser, node, element)
        {
        }

        public override void Execute ()
        {
            IAsyncResult result = this.Browser.BeginInvoke (new Action (() =>
            {
                this.Log.Information ($"Search: Node: {this.Node.NodeName}, Type: GetElementById");

                if (this.Node is GeckoDomDocument domDocument)
                {
                    string elementId = this.Element.Value.Deserialize<string> ();
                    this.SearchResult = new GeckoNode[] { domDocument.GetElementById (elementId) };
                }
                else
                {
                    throw new InvalidOperationException ($"GetElementById 不支持在子节点中进行查找操作");
                }
            }));
            this.Browser.EndInvoke (result);
        }
    }
}
