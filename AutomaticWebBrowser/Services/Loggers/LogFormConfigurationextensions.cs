using System;

using AutomaticWebBrowser.Views;

using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;

namespace AutomaticWebBrowser.Services.Loggers
{
    static class LogFormConfigurationextensions
    {
        const string DefaultOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        public static LoggerConfiguration LogForm (this LoggerSinkConfiguration sinkConfiguration, LogForm logForm, LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum, string outputTemplate = DefaultOutputTemplate, IFormatProvider formatProvider = null, LoggingLevelSwitch levelSwitch = null)
        {
            if (sinkConfiguration is null)
            {
                throw new ArgumentNullException (nameof (sinkConfiguration));
            }

            if (logForm is null)
            {
                throw new ArgumentNullException (nameof (logForm));
            }

            MessageTemplateTextFormatter formatter = new (outputTemplate, formatProvider);
            return LogForm (sinkConfiguration, logForm, formatter, restrictedToMinimumLevel, levelSwitch);
        }

        public static LoggerConfiguration LogForm (this LoggerSinkConfiguration sinkConfiguration, LogForm logForm, ITextFormatter formatter, LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum, LoggingLevelSwitch levelSwitch = null)
        {
            if (sinkConfiguration is null)
            {
                throw new ArgumentNullException (nameof (sinkConfiguration));
            }

            if (logForm is null)
            {
                throw new ArgumentNullException (nameof (logForm));
            }

            if (formatter is null)
            {
                throw new ArgumentNullException (nameof (formatter));
            }

            logForm.TextFormatter = formatter;
            return sinkConfiguration.Sink (logForm, restrictedToMinimumLevel, levelSwitch);
        }
    }
}
