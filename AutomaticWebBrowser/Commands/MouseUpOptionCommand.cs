using System.Text.Json;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

namespace AutomaticWebBrowser.Commands
{
    [OptionCommand (OptionType.MouseUp)]
    public class MouseUpOptionCommand : OptionCommand
    {
        public MouseUpOptionCommand (TaskWebBrowser webBrowser, GeckoElement element, Option option) : base (webBrowser, element, option)
        {
        }

        public override void Execute ()
        {
            TaskWebBrowser.Option.MouseUp (this.Browser, this.Element, this.Option.Value.Deserialize<MouseInfo> ());
        }
    }
}
