using System;
using System.Text.Json;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Services.Configuration.Models;

using Gecko;
using Gecko.DOM;

using Serilog.Core;

namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands.JobCommands
{
    [JobCommand (JobType.OpenLink)]
    class OpenLinkJobCommand : JobCommand
    {
        public OpenLinkJobCommand (GeckoWebBrowser webView, GeckoNode node, Job job, Logger log)
            : base (webView, node, job, log)
        { }

        public override bool Execute ()
        {
            //// 创建新标签
            //WebViewTabPage page = new (this.Log);

            //IAsyncResult asyncResult = this.WebView.BeginInvoke (new Func<bool> (() =>
            //{
            //    if (this.Node is GeckoAnchorElement anchorElement)
            //    {
            //        this.TabConrol.TabPages.Add (page);
            //        this.TabConrol.SelectTab (page);

            //        page.WebView.Navigate (anchorElement.Href);
            //    }
            //    else
            //    {
            //        this.Log.Warning ($"JobCommand executed “openLink” job of node “{this.Node.NodeName}”, but node is not “AnchorElement”.");
            //    }

            //    if (this.Job.Value.ValueKind == JsonValueKind.Array)
            //    {
            //        try
            //        {
            //            Configuration.Models.Action[] actions = this.Job.Value.Deserialize<Configuration.Models.Action[]> (Global.JsonSerializerOptions);

            //        }
            //        catch (JsonException e)
            //        {
            //            this.Log.Error (e, $"JobCommand executed “openLink” job of node “{this.Node.NodeName}”, but value deserialize to “Action[]” type fail.");
            //        }
            //    }
            //    else
            //    {
            //        this.Log.Warning ($"JobCommand executed “openLink” job of node “{this.Node.NodeName}”, but job value type is not “Array”.");
            //    }

            //    return false;
            //}));
            //bool result = this.WebView.EndInvoke (asyncResult) as bool? ?? false;

            return false;
        }
    }
}