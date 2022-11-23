using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

namespace AutomaticWebBrowser.Commands.OperationCommands
{
    [OperationCommand (OperationType.Focus)]
    public class FocusOperationCommand : OperationCommand
    {
        public FocusOperationCommand (TaskWebBrowser webBrowser, GeckoNode node, Operation operation)
            : base (webBrowser, node, operation)
        { }

        public override void Execute ()
        {
            TaskWebBrowser.Option.Focus (this.Browser, this.Node);
        }
    }
}
