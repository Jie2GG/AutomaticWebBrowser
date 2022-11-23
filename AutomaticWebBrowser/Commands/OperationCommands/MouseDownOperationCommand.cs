using System.Text.Json;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

namespace AutomaticWebBrowser.Commands.OperationCommands
{

    [OperationCommand (OperationType.MouseDown)]
    public class MouseDownOperationCommand : OperationCommand
    {
        public MouseDownOperationCommand (TaskWebBrowser webBrowser, GeckoElement element, Operation operation)
            : base (webBrowser, element, operation)
        { }

        public override void Execute ()
        {
            ButtonInfo mouseKeyInfo = this.Operation.Value.Deserialize<ButtonInfo> ();
            TaskWebBrowser.Option.MouseDown (this.Browser, this.Node, mouseKeyInfo);
        }
    }
}
