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
                        this.Log.Information ($"自动化任务 --> 在节点: {this.SourceNode.NodeName} 执行 “evaluateXPath” 搜索, 搜索值是 “{searchValue}”, 搜索到 {result.Length} 个结果");
                        return result;
                    }
                    else
                    {
                        this.Log.Warning ($"自动化任务 --> 在节点: {this.SourceNode.NodeName} 执行 “evaluateXPath” 搜索, 搜索值是 “{searchValue}”, 但搜索结果不可遍历");
                    }
                }
                else
                {
                    this.Log.Warning ($"自动化任务 --> 在节点: {this.SourceNode.NodeName} 执行 “evaluateXPath” 搜索, 但用于搜索的值不是 string 类型");
                }

                return null;
            }));

            return this.WebView.EndInvoke (asyncResult) as GeckoNode[];
        }
    }
}
