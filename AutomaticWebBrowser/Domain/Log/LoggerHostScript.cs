using System.Runtime.InteropServices;

using Serilog.Core;

namespace AutomaticWebBrowser.Domain.Log
{
    [ComVisible (true)]
    public class LoggerHostScript
    {
        #region --字段--
        private readonly Logger logger;
        #endregion

        #region --构造函数--
        public LoggerHostScript (Logger logger)
        {
            this.logger = logger;
        }
        #endregion

        #region --公开方法--
        public void Debug (string message)
        {
            this.logger.Debug (message);
        }

        public void Info (string message)
        {
            this.logger.Information (message);
        }

        public void Warning (string message)
        {
            this.logger.Warning (message);
        }

        public void Error (string message)
        {
            this.logger.Error (message);
        }
        #endregion
    }
}
