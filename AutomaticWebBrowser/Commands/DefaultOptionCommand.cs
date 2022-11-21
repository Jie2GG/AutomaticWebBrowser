using System;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

namespace AutomaticWebBrowser.Commands
{
    public class DefaultOptionCommand : OptionCommand
    {
        public DefaultOptionCommand (TaskWebBrowser webBrowser, GeckoNode element, Option option)
            : base (webBrowser, element, option)
        { }

        public override void Execute ()
        {
            throw new InvalidOperationException ("不支持的操作");
        }
    }
}
