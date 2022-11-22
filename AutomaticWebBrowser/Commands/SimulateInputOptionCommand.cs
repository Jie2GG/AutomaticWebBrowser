using System.Text.Json;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

namespace AutomaticWebBrowser.Commands
{
    [OptionCommand (OptionType.KeypressInput)]
    public class SimulateInputOptionCommand : OptionCommand
    {
        public SimulateInputOptionCommand (TaskWebBrowser webBrowser, GeckoNode node, Option option) : base (webBrowser, node, option)
        {
        }

        public override void Execute ()
        {
            string value = this.Option.Value.Deserialize<string> (GlobalConfig.JsonSerializerOptions);
            TaskWebBrowser.Option.KeypressInput (this.Browser, this.Node, value);
        }
    }
}
