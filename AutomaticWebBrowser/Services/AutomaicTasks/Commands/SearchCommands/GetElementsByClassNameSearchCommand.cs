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
                        this.Log.Information ($"自动化任务 --> 在节点: {this.SourceNode.NodeName} 执行 “getElementsByClassName” 搜索, 搜索值是 {searchValue}, 返回数量: {result.Length}");
                        return result;
                    }
                    else
                    {
                        this.Log.Warning ($"自动化任务 --> 在节点: {this.SourceNode.NodeName} 执行 “getElementsByClassName” 搜索, 但搜索值不是 string 类型");
                    }
                }
                else
                {
                    this.Log.Warning ($"自动化任务 --> 在节点: {this.SourceNode.NodeName} 执行 “getElementsByClassName” 搜索, 但源节点不是 Document 类型");
                }

                return null;
            }));

            return this.WebView.EndInvoke (asyncResult) as GeckoNode[];
        }
    }
}
