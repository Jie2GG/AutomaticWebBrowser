using System.Threading.Tasks;

using AutomaticWebBrowser.Commons;

namespace AutomaticWebBrowser.Controls
{
    /// <summary>
    /// Web视图接口
    /// </summary>
    interface IWebView
    {
        AsyncWaitHostScript WaitHostScript { get; }

        /// <summary>
        /// 导航到指定地址 (该方法是线程安全的)
        /// </summary>
        /// <param name="url"></param>
        void SafeNavigate (string url);

        /// <summary>
        /// 异步执行 Javascript 代码 (该方法是线程安全的)
        /// </summary>
        /// <param name="javaScript"></param>
        /// <returns></returns>
        Task<string> SafeExecuteScriptAsync (string javaScript);
    }
}
