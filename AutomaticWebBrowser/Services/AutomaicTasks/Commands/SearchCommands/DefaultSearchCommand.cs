using System;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Services.Configuration.Models;

using Gecko;

using Serilog.Core;

namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands.SearchCommands
{
    [SearchCommand (SearchMode.None)]
    class DefaultSearchCommand : SearchCommand
    {
        public DefaultSearchCommand (GeckoWebBrowser webView, GeckoNode sourceNode, Configuration.Models.Element element, Logger log)
            : base (webView, sourceNode, element, log)
        { }

        public override GeckoNode[] Execute ()
        {
            IAsyncResult asyncResult = this.WebView.BeginInvoke (new Func<GeckoNode[]> (() =>
            {
                this.Log.Information ($"自动化任务 --> 在节点: {this.SourceNode.NodeName} 执行 “default” 搜索");
                return null;
            }));
            return this.WebView.EndInvoke (asyncResult) as GeckoNode[];
        }
    }
}
