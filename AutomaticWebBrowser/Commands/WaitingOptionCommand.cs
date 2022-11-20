using System.Text.Json;
using System.Threading;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

namespace AutomaticWebBrowser.Commands
{
    [OptionCommand (OptionType.Waiting)]
    public class WaitingOptionCommand : OptionCommand
    {
        public WaitingOptionCommand (TaskWebBrowser webBrowser, GeckoElement element, Option option)
            : base (webBrowser, element, option)
        { }

        public override void Execute ()
        {
            int value = this.Option.Value.Deserialize<int> (GlobalConfig.JsonSerializerOptions);
            Thread.Sleep (value);
        }
    }
}
