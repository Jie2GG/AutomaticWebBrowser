using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

using Action = System.Action;

namespace AutomaticWebBrowser.Commands.SearchCommands
{
    [SearchCommand (SearchType.GetChildElementById)]
    public class GetChildElementByIdSearchCommand : SearchCommand
    {
        public GetChildElementByIdSearchCommand (TaskWebBrowser webBrowser, GeckoNode node, Models.Element element)
            : base (webBrowser, node, element)
        { }

        public override void Execute ()
        {
            string elementId = this.Element.Value.Deserialize<string> ();
            IAsyncResult result = this.Browser.BeginInvoke (new Action (() =>
            {
                if (this.Node is GeckoDomDocument domDocument)
                {
                    this.SearchResult = domDocument.GetElementById (elementId).ChildNodes.ToArray ();
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
