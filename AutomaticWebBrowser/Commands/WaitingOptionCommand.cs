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
        public WaitingOptionCommand (TaskWebBrowser webBrowser, GeckoNode node, Option option) : base (webBrowser, node, option)
        { }

        public override void Execute ()
        {
            int value = this.Option.Value.Deserialize<int> (GlobalConfig.JsonSerializerOptions);
            Thread.Sleep (value);
        }
    }
}
