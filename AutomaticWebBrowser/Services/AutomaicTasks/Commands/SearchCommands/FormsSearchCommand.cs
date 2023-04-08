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
                            this.Log.Information ($"自动化任务 --> 在节点: {this.SourceNode.NodeName} 执行 “forms” 搜索, 表单数量: {document.Forms.Length}, 返回第{index}个");
                            return new GeckoNode[] { document.Forms[index] };
                        }
                    }
                    else if (this.Element.SearchValue.ValueKind == JsonValueKind.Undefined || this.Element.SearchValue.ValueKind == JsonValueKind.Null)
                    {
                        // 获取所有表单
                        this.Log.Information ($"自动化任务 --> 在节点: {this.SourceNode.NodeName} 执行 “forms” 搜索, 表单数量: {document.Forms.Length}, 返回所有");
                        return document.Forms.ToArray ();
                    }
                    else
                    {
                        this.Log.Warning ($"自动化任务 --> 在节点: {this.SourceNode.NodeName} 执行 “forms” 搜索, 但搜索值参数不是 integer 或 nullable 类型");
                    }
                }
                else
                {
                    this.Log.Warning ($"自动化任务 --> 在节点: {this.SourceNode.NodeName} 执行 “forms” 搜索, 但源节点不是 Document 类型");
                }

                return null;
            }));

            return this.WebView.EndInvoke (asyncResult) as GeckoNode[];
        }
    }
}
