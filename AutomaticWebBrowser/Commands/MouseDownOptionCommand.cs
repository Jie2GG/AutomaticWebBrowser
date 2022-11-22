﻿using System.Text.Json;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

namespace AutomaticWebBrowser.Commands
{

    [OptionCommand (OptionType.MouseDown)]
    public class MouseDownOptionCommand : OptionCommand
    {
        public MouseDownOptionCommand (TaskWebBrowser webBrowser, GeckoElement element, Option option) : base (webBrowser, element, option)
        {
        }

        public override void Execute ()
        {
            ButtonInfo mouseKeyInfo = this.Option.Value.Deserialize<ButtonInfo> ();
            TaskWebBrowser.Option.MouseDown (this.Browser, this.Node, mouseKeyInfo);
        }
    }
}
