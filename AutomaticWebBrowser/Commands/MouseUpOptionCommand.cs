﻿using System.Text.Json;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

namespace AutomaticWebBrowser.Commands
{
    [OptionCommand (OptionType.MouseUp)]
    public class MouseUpOptionCommand : OptionCommand
    {
        public MouseUpOptionCommand (TaskWebBrowser webBrowser, GeckoNode node, Option option) : base (webBrowser, node, option)
        {
        }

        public override void Execute ()
        {
            MouseKeyInfo mouseKeyInfo = this.Option.Value.Deserialize<MouseKeyInfo> (GlobalConfig.JsonSerializerOptions);
            TaskWebBrowser.Option.MouseUp (this.Browser, this.Node, mouseKeyInfo);
        }
    }
}
