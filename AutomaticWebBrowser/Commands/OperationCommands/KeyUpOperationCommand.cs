using System.Text.Json;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

namespace AutomaticWebBrowser.Commands.OperationCommands
{
    [OperationCommand (OperationType.KeyUp)]
    public class KeyUpOperationCommand : OperationCommand
    {
        public KeyUpOperationCommand (TaskWebBrowser webBrowser, GeckoNode node, Operation operation)
            : base (webBrowser, node, operation)
        { }

        public override void Execute ()
        {
            KeyInfo keyInfo = this.Operation.Value.Deserialize<KeyInfo> (GlobalConfig.JsonSerializerOptions);
            TaskWebBrowser.Option.KeyUp (this.Browser, this.Node, keyInfo);
        }
    }
}
