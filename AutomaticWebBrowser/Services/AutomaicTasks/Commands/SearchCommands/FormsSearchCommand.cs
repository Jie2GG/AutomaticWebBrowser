using System;
using System.Linq;
using System.Text.Json;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Services.Configuration.Models;

using Gecko;

using Serilog.Core;

namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands.SearchCommands
{
    [SearchCommand (SearchMode.Forms)]
    internal class FormsSearchCommand : SearchCommand
    {
        public FormsSearchCommand (GeckoWebBrowser webView, GeckoNode sourceNode, Configuration.Models.Element element, Logger log)
            : base (webView, sourceNode, element, log)
        { }

        public override GeckoNode[] Execute ()
        {
            IAsyncResult asyncResult = WebView.BeginInvoke (new Func<GeckoNode[]> (() =>
            {
                if (this.SourceNode is GeckoDocument document)
                {
                    if (this.Element.SearchValue.ValueKind == JsonValueKind.Number)
                    {
                        // 获取指定位置的表单
                        uint index = this.Element.SearchValue.GetUInt32 ();
                        if (document.Forms.Length > 0)
                        {
                            this.Log.Information ($"SearchCommand executed “forms” command of source node “{this.SourceNode.NodeName}”, forms count is “{document.Forms.Length}”, return to the “{index}th”.");
                            return new GeckoNode[] { document.Forms[index] };
                        }
                    }
                    else if (this.Element.SearchValue.ValueKind == JsonValueKind.Undefined || this.Element.SearchValue.ValueKind == JsonValueKind.Null)
                    {
                        // 获取所有表单
                        this.Log.Information ($"SearchCommand executed “forms” command of source node “{this.SourceNode.NodeName}”, document forms count is “{document.Forms.Length}”, return all.");
                        return document.Forms.ToArray ();
                    }
                    else
                    {
                        this.Log.Warning ($"SearchCommand executed “forms” command of source node “{this.SourceNode.NodeName}”, but search value type is not integer or nullable.");
                    }
                }
                else
                {
                    this.Log.Warning ($"SearchCommand executed “forms” command of source node “{this.SourceNode.NodeName}”, but source node type is not “Document”.");
                }

                return null;
            }));

            return this.WebView.EndInvoke (asyncResult) as GeckoNode[];
        }
    }
}
