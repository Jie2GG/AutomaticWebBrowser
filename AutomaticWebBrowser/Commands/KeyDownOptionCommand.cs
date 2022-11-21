using System.Text.Json;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

namespace AutomaticWebBrowser.Commands
{
    [OptionCommand (OptionType.KeyDown)]
    public class KeyDownOptionCommand : OptionCommand
    {
        public KeyDownOptionCommand (TaskWebBrowser webBrowser, GeckoNode node, Option option) : base (webBrowser, node, option)
        {
        }

        public override void Execute ()
        {
            KeyInfo keyInfo = this.Option.Value.Deserialize<KeyInfo> (GlobalConfig.JsonSerializerOptions);
            TaskWebBrowser.Option.KeyDown (this.Browser, this.Node, keyInfo);
        }
    }
}
