using System;

using AutomaticWebBrowser.Services.Configuration.Models;

namespace AutomaticWebBrowser.Commands.DomSearchCommands
{
    class JobCommandAttribute : Attribute
    {
        public JobType Type { get; set; }

        public JobCommandAttribute (JobType type)
        {
            this.Type = type;
        }
    }
}
