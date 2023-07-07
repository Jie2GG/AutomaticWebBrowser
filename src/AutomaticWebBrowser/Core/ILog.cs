using Serilog;
using Serilog.Core;
using Serilog.Formatting;

namespace AutomaticWebBrowser.Wpf.Core
{
    /// <summary>
    /// 日志接口
    /// </summary>
    interface ILog : ILogEventSink
    {
        ITextFormatter? TextFormatter { get; set; }
    }
}
