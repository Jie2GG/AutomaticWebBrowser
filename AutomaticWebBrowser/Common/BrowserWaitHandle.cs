using System.Threading;

using AutomaticWebBrowser.Views;

namespace AutomaticWebBrowser.Common
{
    /// <summary>
    /// 浏览器等待事件
    /// </summary>
    class BrowserWaitHandle : EventWaitHandle
    {
        #region --属性--
        public BrowserForm Form { get; }
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="BrowserWaitHandle"/> 类的新实例
        /// </summary>
        /// <param name="form">由等待的线程创建的浏览器窗体</param>
        public BrowserWaitHandle (BrowserForm form)
            : base (true, EventResetMode.AutoReset)
        {
            this.Form = form ?? throw new System.ArgumentNullException (nameof (form));
            this.Form.Show ();
        }
        #endregion
    }
}
