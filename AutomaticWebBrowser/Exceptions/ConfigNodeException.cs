using System;

namespace AutomaticWebBrowser.Exceptions
{
    public class ConfigNodeException : Exception
    {
        public ConfigNodeException (string message)
            : base (message)
        {
        }
    }
}
