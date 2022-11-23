using System.Text.Json;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

namespace AutomaticWebBrowser.Commands.OperationCommands
{
    [OperationCommand (OperationType.KeypressInput)]
    public class SimulateInputOperationCommand : OperationCommand
    {
        public SimulateInputOperationCommand (TaskWebBrowser webBrowser, GeckoNode node, Operation operation)
            : base (webBrowser, node, operation)
        { }

        public override void Execute ()
        {
            string value = this.Operation.Value.Deserialize<string> (GlobalConfig.JsonSerializerOptions);
            TaskWebBrowser.Option.KeypressInput (this.Browser, this.Node, value);
        }
    }
}
