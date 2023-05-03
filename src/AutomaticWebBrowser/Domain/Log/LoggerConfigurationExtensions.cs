using System;

using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;

namespace AutomaticWebBrowser.Domain.Log
{
    static class LoggerConfigurationExtensions
    {
        const string DefaultOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        public static LoggerConfiguration View (this LoggerSinkConfiguration sinkConfiguration, ILoggerFormatter loggerFormatter, LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum, string outputTemplate = DefaultOutputTemplate, IFormatProvider? formatProvider = null, LoggingLevelSwitch? levelSwitch = null)
        {
            if (sinkConfiguration is null)
            {
                throw new ArgumentNullException (nameof (sinkConfiguration));
            }

            if (loggerFormatter is null)
            {
                throw new ArgumentNullException (nameof (loggerFormatter));
            }

            MessageTemplateTextFormatter formatter = new (outputTemplate, formatProvider);
            return View (sinkConfiguration, loggerFormatter, formatter, restrictedToMinimumLevel, levelSwitch);
        }

        public static LoggerConfiguration View (this LoggerSinkConfiguration sinkConfiguration, ILoggerFormatter loggerFormatter, ITextFormatter formatter, LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum, LoggingLevelSwitch? levelSwitch = null)
        {
            if (sinkConfiguration is null)
            {
                throw new ArgumentNullException (nameof (sinkConfiguration));
            }

            if (loggerFormatter is null)
            {
                throw new ArgumentNullException (nameof (loggerFormatter));
            }

            if (formatter is null)
            {
                throw new ArgumentNullException (nameof (formatter));
            }

            loggerFormatter.TextFormatter = formatter;
            return sinkConfiguration.Sink (loggerFormatter, restrictedToMinimumLevel, levelSwitch);
        }
    }
}
