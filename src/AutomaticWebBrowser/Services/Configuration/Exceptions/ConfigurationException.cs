using System;

namespace AutomaticWebBrowser.Services.Configuration.Exceptions
{
    class ConfigurationException : Exception
    {
        public ConfigurationException (string? message)
            : base (message)
        { }
    }
}
