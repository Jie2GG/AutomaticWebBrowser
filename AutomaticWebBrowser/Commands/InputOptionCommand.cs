using System.Text.Json;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

namespace AutomaticWebBrowser.Commands
{
    [OptionCommand (OptionType.Input)]
    public class InputOptionCommand : OptionCommand
    {
        public InputOptionCommand (TaskWebBrowser webBrowser, GeckoElement element, Option option) : base (webBrowser, element, option)
        {
        }

        public override void Execute ()
        {
            string value = this.Option.Value.Deserialize<string> (GlobalConfig.JsonSerializerOptions);
            TaskWebBrowser.Option.Input (this.Browser, this.Element, value);
        }
    }
}
