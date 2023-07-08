using Serilog.Events;

namespace AutomaticWebBrowser.Services.Logger.Models
{
    class Log
    {
        public LogEventLevel Level { get; init; }

        public required string Message { get; init; }
    }
}
