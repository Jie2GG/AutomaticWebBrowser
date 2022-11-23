using System;

using AutomaticWebBrowser.Models;

namespace AutomaticWebBrowser.Commands.DomSearchCommands
{
    public class SearchCommandAttribute : Attribute
    {
        public SearchType SearchType { get; set; }

        public SearchCommandAttribute (SearchType searchType)
        {
            this.SearchType = searchType;
        }
    }
}
