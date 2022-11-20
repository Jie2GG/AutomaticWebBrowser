using System.Text.Json;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

namespace AutomaticWebBrowser.Commands
{
    [OptionCommand (OptionType.KeyDown)]
    public class KeyDownOptionCommand : OptionCommand
    {
        public KeyDownOptionCommand (TaskWebBrowser webBrowser, GeckoElement element, Option option) : base (webBrowser, element, option)
        {
        }

        public override void Execute ()
        {
            KeyInfo keyInfo = Option.Value.Deserialize<KeyInfo> (GlobalConfig.JsonSerializerOptions);
            TaskWebBrowser.Option.KeyDown (Browser, Element, keyInfo);
        }
    }
}
