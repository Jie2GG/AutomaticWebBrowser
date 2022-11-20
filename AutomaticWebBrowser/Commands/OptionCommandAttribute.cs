using System;

using AutomaticWebBrowser.Models;

namespace AutomaticWebBrowser.Commands
{
    public class OptionCommandAttribute : Attribute
    {
        public OptionType OptionType { get; set; }

        public OptionCommandAttribute (OptionType optionType)
        {
            OptionType = optionType;
        }
    }
}
