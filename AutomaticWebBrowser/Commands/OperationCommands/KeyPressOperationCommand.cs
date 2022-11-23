using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

namespace AutomaticWebBrowser.Commands.OperationCommands
{
    [OperationCommand (OperationType.KeyPress)]
    public class KeyPressOperationCommand : OperationCommand
    {
        public KeyPressOperationCommand (TaskWebBrowser webBrowser, GeckoNode node, Operation operation)
            : base (webBrowser, node, operation)
        { }

        public override void Execute ()
        {

            KeyInfo keyInfo = this.Operation.Value.Deserialize<KeyInfo> (GlobalConfig.JsonSerializerOptions);
            TaskWebBrowser.Option.KeyPress (this.Browser, this.Node, keyInfo);
        }
    }
}
