using System.Text.Json;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

namespace AutomaticWebBrowser.Commands.OperationCommands
{
    [OperationCommand (OperationType.MouseClick)]
    public class MouseClickOperationCommand : OperationCommand
    {
        public MouseClickOperationCommand (TaskWebBrowser webBrowser, GeckoNode node, Operation operation)
            : base (webBrowser, node, operation)
        { }

        public override void Execute ()
        {
            ButtonInfo mouseKeyInfo = this.Operation.Value.Deserialize<ButtonInfo> (GlobalConfig.JsonSerializerOptions);
            TaskWebBrowser.Option.MouseClick (this.Browser, this.Node, mouseKeyInfo);
        }
    }
}
