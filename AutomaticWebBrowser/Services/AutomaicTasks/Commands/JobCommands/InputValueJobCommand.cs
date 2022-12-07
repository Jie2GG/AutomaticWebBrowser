using System;
using System.Text.Json;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Services.Configuration.Models;

using Gecko;
using Gecko.DOM;

namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands.JobCommands
{
    [JobCommand (JobType.InputValue)]
    class InputValueJobCommand : JobCommand
    {
        public InputValueJobCommand (WebView webView, GeckoNode node, Job job)
            : base (webView, node, job)
        { }

        public override bool Execute ()
        {
            IAsyncResult asyncResult = this.WebView.BeginInvoke (new Func<bool> (() =>
            {
                if (this.Job.Value.ValueKind == JsonValueKind.String)
                {
                    string inputValue = this.Job.Value.Deserialize<string> (Global.JsonSerializerOptions);
                    if (this.Node is GeckoInputElement inputElement)
                    {
                        inputElement.Value = inputValue;
                        this.Log.Information ($"JobCommand executed “inputValue” job of node “{this.Node.NodeName}”, value is “{inputValue}”.");
                        return true;
                    }
                    else
                    {
                        this.Log.Warning ($"JobCommand executed “inputValue” job of node “{this.Node.NodeName}”, but node is not “InputElement”.");
                    }
                }
                else
                {
                    this.Log.Warning ($"JobCommand executed “inputValue” job of node “{this.Node.NodeName}”, but job value type is not string.");
                }

                return false;
            }));
            return this.WebView.EndInvoke (asyncResult) as bool? ?? false;
        }
    }
}
