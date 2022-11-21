﻿using System.Text.Json;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

namespace AutomaticWebBrowser.Commands
{
    [OptionCommand( OptionType.MouseClick)]
    public class MouseClickOptionCommand : OptionCommand
    {
        public MouseClickOptionCommand (TaskWebBrowser webBrowser, GeckoNode node, Option option) : base (webBrowser, node, option)
        {
        }

        public override void Execute ()
        {
            MouseKeyInfo mouseKeyInfo = this.Option.Value.Deserialize<MouseKeyInfo> (GlobalConfig.JsonSerializerOptions);
            TaskWebBrowser.Option.MouseClick (this.Browser, this.Node, mouseKeyInfo);
        }
    }
}
