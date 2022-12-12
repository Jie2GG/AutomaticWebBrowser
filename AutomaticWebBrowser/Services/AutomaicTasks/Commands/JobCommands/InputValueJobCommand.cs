using System.Text.Json;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Services.Configuration.Models;

using Gecko;
using Gecko.DOM;

using Serilog.Core;

namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands.JobCommands
{
    [JobCommand (JobType.InputValue)]
    class InputValueJobCommand : JobCommand
    {
        public InputValueJobCommand (GeckoWebBrowser webView, GeckoNode node, Job job, Logger log)
            : base (webView, node, job, log)
        { }

        public override bool Execute ()
        {
            if (this.Job.Value.ValueKind == JsonValueKind.String)
            {
                try
                {
                    string inputValue = this.Job.Value.Deserialize<string> (Global.JsonSerializerOptions);

                    if (this.Node is GeckoInputElement inputElement)
                    {
                        bool result = (bool)this.WebView.Invoke (() =>
                        {
                            inputElement.Value = inputValue;
                            this.Log.Information ($"JobCommand executed “inputValue” job of node “{this.Node.NodeName}”, value is “{inputValue}”.");
                            return true;
                        });

                        return result;
                    }
                    else
                    {
                        this.Log.Warning ($"JobCommand executed “inputValue” job of node “{this.NodeName}”, but node is not “InputElement”.");
                    }
                }
                catch (JsonException e)
                {
                    this.Log.Error (e, $"JobCommand executed “inputValue” job of node “{this.NodeName}”, but value deserialize to “String” type fail.");
                }
            }
            else
            {
                this.Log.Warning ($"JobCommand executed “inputValue” job of node “{this.NodeName}”, but job value type is not “String”.");
            }

            return false;
        }
    }
}
