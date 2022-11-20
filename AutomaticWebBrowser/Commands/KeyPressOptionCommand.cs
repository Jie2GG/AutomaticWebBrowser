using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

namespace AutomaticWebBrowser.Commands
{
    [OptionCommand (OptionType.KeyPress)]
    public class KeyPressOptionCommand : OptionCommand
    {
        public KeyPressOptionCommand (TaskWebBrowser webBrowser, GeckoElement element, Option option) : base (webBrowser, element, option)
        { }

        public override void Execute ()
        {

            KeyInfo keyInfo = this.Option.Value.Deserialize<KeyInfo> (GlobalConfig.JsonSerializerOptions);
            TaskWebBrowser.Option.KeyPress (this.Browser, this.Element, keyInfo);
        }
    }
}
