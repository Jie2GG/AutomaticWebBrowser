using System;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

namespace AutomaticWebBrowser.Commands.OperationCommands
{
    public class DefaultOperationCommand : OperationCommand
    {
        public DefaultOperationCommand (TaskWebBrowser webBrowser, GeckoNode element, Operation operation)
            : base (webBrowser, element, operation)
        { }

        public override void Execute ()
        {
            throw new InvalidOperationException ("不支持的操作");
        }
    }
}
