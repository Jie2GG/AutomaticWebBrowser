using System.Runtime.InteropServices;

using Serilog;
using Serilog.Events;

namespace AutomaticWebBrowser.Services.Logger
{
    /// <summary>
    /// <see cref="ILogger"/> 的 JavaScript 日志主机服务类
    /// </summary>
    [ComVisible (true)]
    public class LoggerHostScript : ILogger
    {
        #region --字段--
        private readonly ILogger target;
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="LoggerHostScript"/> 类的新实例
        /// </summary>
        /// <param name="target">目标日志接口</param>
        public LoggerHostScript (ILogger target)
        {
            this.target = target;
        }
        #endregion

        #region --公开方法--
        public void Debug (string messageTemplate)
        {
            this.target.Debug (messageTemplate);
        }

        public void Error (string messagetemplate)
        {
            this.target.Error (messagetemplate);
        }

        public void Fatal (string messageTemplate)
        {
            this.target.Fatal (messageTemplate);
        }

        public void Information (string messageTemplate)
        {
            this.target.Information (messageTemplate);
        }

        public void Verbose (string messageTemplate)
        {
            this.target.Verbose (messageTemplate);
        }

        public void Warning (string messageTemplate)
        {
            this.target.Warning (messageTemplate);
        }

        public void Write (LogEvent logEvent)
        {
            this.target.Write (logEvent);
        }
        #endregion
    }
}
