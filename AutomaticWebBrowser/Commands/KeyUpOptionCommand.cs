using System.Text.Json;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

namespace AutomaticWebBrowser.Commands
{
    [OptionCommand (OptionType.KeyUp)]
    public class KeyUpOptionCommand : OptionCommand
    {
        public KeyUpOptionCommand (TaskWebBrowser webBrowser, GeckoNode node, Option option) : base (webBrowser, node, option)
        {
        }

        public override void Execute ()
        {
            KeyInfo keyInfo = this.Option.Value.Deserialize<KeyInfo> (GlobalConfig.JsonSerializerOptions);
            TaskWebBrowser.Option.KeyUp (this.Browser, this.Node, keyInfo);
        }
    }
}
