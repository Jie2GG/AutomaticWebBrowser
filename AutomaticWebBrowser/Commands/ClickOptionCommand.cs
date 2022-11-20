using System;
using System.Text.Json;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

namespace AutomaticWebBrowser.Commands
{
    [OptionCommand (OptionType.Click)]
    public class ClickOptionCommand : OptionCommand
    {
        public ClickOptionCommand (TaskWebBrowser webBrowser, GeckoElement element, Option option) : base (webBrowser, element, option)
        {
        }

        public override void Execute ()
        {
            int count = 1;
            if (this.Option.Value.ValueKind == JsonValueKind.Number)
            {
                count = this.Option.Value.Deserialize<int> (GlobalConfig.JsonSerializerOptions);
            }
            TaskWebBrowser.Option.Click (this.Browser, this.Element, count);
        }
    }
}
