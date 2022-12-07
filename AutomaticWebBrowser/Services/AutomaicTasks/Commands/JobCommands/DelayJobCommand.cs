using System;
using System.Text.Json;
using System.Threading;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Services.Configuration.Models;

using Gecko;

namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands.JobCommands
{
    [JobCommand (JobType.Delay)]
    class DelayJobCommand : JobCommand
    {
        public DelayJobCommand (WebView webView, GeckoNode node, Job job)
            : base (webView, node, job)
        { }

        public override bool Execute ()
        {
            IAsyncResult asyncResult = this.WebView.BeginInvoke (new Func<bool> (() =>
            {
                if (this.Job.Value.ValueKind == JsonValueKind.String)
                {
                    string timeFormat = this.Job.Value.Deserialize<string> (Global.JsonSerializerOptions);
                    if (TimeSpan.TryParse (timeFormat, out TimeSpan duration))
                    {
                        Thread.Sleep (duration);
                        this.Log.Information ($"JobCommand executed “delay” job of node “{this.Node.NodeName}”, duration: {duration}.");
                        return true;
                    }
                    else
                    {
                        this.Log.Warning ($"JobCommand execute “dealy” job of node “{this.Node.NodeName}”, but job value format error(delay job value format is HH:mm:ss).");
                    }
                }
                else
                {
                    this.Log.Warning ($"JobCommand execute “dealy” job of node “{this.Node.NodeName}”, but job value type is not string.");
                }

                return false;
            }));
            return this.WebView.EndInvoke (asyncResult) as bool? ?? false;
        }
    }
}
