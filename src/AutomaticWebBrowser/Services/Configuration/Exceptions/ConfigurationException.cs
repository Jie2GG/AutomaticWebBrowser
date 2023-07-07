using System;

namespace AutomaticWebBrowser.Wpf.Services.Configuration.Exceptions
{
    class ConfigurationException : Exception
    {
        public ConfigurationException (string? message)
            : base (message)
        { }
    }
}
