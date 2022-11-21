﻿using System.Text.Json;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

namespace AutomaticWebBrowser.Commands
{
    [OptionCommand (OptionType.Input)]
    public class InputOptionCommand : OptionCommand
    {
        public InputOptionCommand (TaskWebBrowser webBrowser, GeckoNode node, Option option) : base (webBrowser, node, option)
        {
        }

        public override void Execute ()
        {
            string value = this.Option.Value.Deserialize<string> (GlobalConfig.JsonSerializerOptions);
            TaskWebBrowser.Option.Input (this.Browser, this.Node, value);
        }
    }
}
