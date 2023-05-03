using System.Runtime.InteropServices;

using Serilog.Core;

namespace AutomaticWebBrowser.Domain.Log
{
    /// <summary>
    /// 日志宿主脚本对象
    /// </summary>
    [ComVisible (true)]
    public class LoggerHostScript
    {
        #region --字段--
        private readonly Logger logger;
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="LoggerHostScript"/> 类的新实例
        /// </summary>
        /// <param name="logger"></param>
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
