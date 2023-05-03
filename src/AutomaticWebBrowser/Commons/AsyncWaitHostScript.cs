using System.Runtime.InteropServices;
using System.Threading;

namespace AutomaticWebBrowser.Commons
{
    /// <summary>
    /// 异步等待宿主脚本对象
    /// </summary>
    [ComVisible (true)]
    public class AsyncWaitHostScript
    {
        #region --字段--
        private readonly AutoResetEvent waitEvent;
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="AsyncWaitHostScript"/> 类的新实例
        /// </summary>
        public AsyncWaitHostScript ()
        {
            this.waitEvent = new AutoResetEvent (false);
        }
        #endregion

        #region --公开方法--
        /// <summary>
        /// 等待异步执行完成
        /// </summary>
        public void WaitOne ()
        {
            this.waitEvent.Reset ();
            this.waitEvent.WaitOne ();
        }

        /// <summary>
        /// 设置等待为有信号状态
        /// </summary>
        public void Set ()
        {
            this.waitEvent.Set ();
        }
        #endregion
    }
}
