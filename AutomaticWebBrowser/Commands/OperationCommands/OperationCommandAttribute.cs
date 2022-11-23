using System;

using AutomaticWebBrowser.Models;

namespace AutomaticWebBrowser.Commands.OperationCommands
{
    public class OperationCommandAttribute : Attribute
    {
        public OperationType OptionType { get; set; }

        public OperationCommandAttribute (OperationType optionType)
        {
            OptionType = optionType;
        }
    }
}
