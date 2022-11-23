using System.Text.Json;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

namespace AutomaticWebBrowser.Commands.OperationCommands
{
    [OperationCommand (OperationType.Waiting)]
    public class WaitingOperationCommand : OperationCommand
    {
        public WaitingOperationCommand (TaskWebBrowser webBrowser, GeckoNode node, Operation operation)
            : base (webBrowser, node, operation)
        { }

        public override void Execute ()
        {
            int value = this.Operation.Value.Deserialize<int> (GlobalConfig.JsonSerializerOptions);
            TaskWebBrowser.Option.Waiting (this.Browser, value);
        }
    }
}
