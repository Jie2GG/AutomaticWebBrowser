using System.Text.Json;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

namespace AutomaticWebBrowser.Commands.OperationCommands
{
    [OperationCommand (OperationType.Click)]
    public class ClickOperationCommand : OperationCommand
    {
        public ClickOperationCommand (TaskWebBrowser webBrowser, GeckoNode node, Operation operation)
            : base (webBrowser, node, operation)
        { }

        public override void Execute ()
        {
            int count = 1;
            if (this.Operation.Value.ValueKind == JsonValueKind.Number)
            {
                count = this.Operation.Value.Deserialize<int> (GlobalConfig.JsonSerializerOptions);
            }
            TaskWebBrowser.Option.Click (this.Browser, this.Node, count);
        }
    }
}
