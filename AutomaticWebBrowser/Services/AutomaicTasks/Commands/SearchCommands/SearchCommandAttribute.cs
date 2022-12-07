using System;

using AutomaticWebBrowser.Services.Configuration.Models;

namespace AutomaticWebBrowser.Commands.DomSearchCommands
{
    class SearchCommandAttribute : Attribute
    {
        public SearchMode Mode { get; set; }

        public SearchCommandAttribute (SearchMode mode)
        {
            this.Mode = mode;
        }
    }
}
