using System;
using System.Threading.Tasks;
using System.Windows.Forms;

using AutomaticWebBrowser.Services.Configuration.Models;

using Serilog.Core;

namespace AutomaticWebBrowser.Controls
{
    class WebViewTabControls : TabControl
    {
        #region --属性--
        public Logger Log { get; set; }
        #endregion

        #region --公开方法--
        /// <summary>
        /// 运行任务
        /// </summary>
        /// <param name="taskInofs"></param>
        public Task RunTask (params AutomaticTask[] taskInofs)
        {
            return Task.Run (() =>
            {
                foreach (AutomaticTask taskInfo in taskInofs)
                {
                    // 创建新的标签页
                    WebViewTabPage page = new (this.Log);

                    this.Invoke (() =>
                    {
                        // 清空所有标签页
                        this.TabPages.Clear ();
                        this.TabPages.Add (page);

                        // 开始执行任务
                        page.WebView.Navigate (taskInfo.Url);
                    });

                    // 创建任务
                    page.CreateTask (taskInfo.Actions);
                    // 运行任务
                    page.RunTask ();
                }
            });
        }
        #endregion
    }
}
