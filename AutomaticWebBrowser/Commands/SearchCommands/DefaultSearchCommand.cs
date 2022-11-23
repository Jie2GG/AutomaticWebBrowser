using System;

using AutomaticWebBrowser.Controls;

using Gecko;

namespace AutomaticWebBrowser.Commands.DomSearchCommands
{
    public class DefaultSearchCommand : SearchCommand
    {
        public DefaultSearchCommand (TaskWebBrowser webBrowser, GeckoNode node, Models.Element element)
            : base (webBrowser, node, element)
        { }

        public override void Execute ()
        {
            throw new InvalidOperationException ("不支持的操作");
        }
    }
}
