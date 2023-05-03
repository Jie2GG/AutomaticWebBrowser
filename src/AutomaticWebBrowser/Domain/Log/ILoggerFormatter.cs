using Serilog.Core;
using Serilog.Formatting;

namespace AutomaticWebBrowser.Domain.Log
{
    /// <summary>
    /// 日志格式化接口
    /// </summary>
    interface ILoggerFormatter : ILogEventSink
    {
        ITextFormatter? TextFormatter { get; set; }
    }
}
