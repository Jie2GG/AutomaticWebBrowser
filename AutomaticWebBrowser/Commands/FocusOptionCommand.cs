using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

namespace AutomaticWebBrowser.Commands
{
    [OptionCommand (OptionType.Focus)]
    public class FocusOptionCommand : OptionCommand
    {
        public FocusOptionCommand (TaskWebBrowser webBrowser, GeckoNode node, Option option) : base (webBrowser, node, option)
        {
        }

        public override void Execute ()
        {
            TaskWebBrowser.Option.Focus (this.Browser, this.Node);
        }
    }
}
