using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

using AutomaticWebBrowser.Services.AutomaicTasks.Commands.JobCommands;
using AutomaticWebBrowser.Services.AutomaicTasks.Commands.SearchCommands;
using AutomaticWebBrowser.Services.Configuration.Models;

using Gecko;

using Serilog;
using Serilog.Core;

namespace AutomaticWebBrowser.Controls
{
    class WebViewTabPage : TabPage
    {
        #region --字段--
        private readonly ConditionSynchronous conditionSynchronous = new ();
        private Services.Configuration.Models.Action[] runningActions;
        private Task<bool> runningTask;
        #endregion

        #region --属性--
        public Logger Log { get; }
        public GeckoWebBrowser WebView { get; }
        #endregion

        #region --构造函数--
        public WebViewTabPage (Logger log)
            : base ("新标签页")
        {
            this.Log = log ?? throw new ArgumentNullException (nameof (log));
            this.WebView = new GeckoWebBrowser ()
            {
                Dock = DockStyle.Fill,
                Padding = Padding.Empty,
                Margin = Padding.Empty,
            };
            this.WebView.StatusTextChanged += this.WebViewStatusTextChangedEventHandler;
            this.WebView.ReadyStateChange += this.WebViewReadyStateChangeEventHandler;
            this.WebView.DocumentTitleChanged += this.WebViewDocumentTitleChangedEventHandler;
            this.WebView.Navigating += this.WebViewNavigatingEventHandler;
            this.WebView.FrameNavigating += this.WebViewFrameNavigatingEventHandler;
            this.WebView.CreateWindow += this.WebViewCreateWindowEventHandler;
            this.WebView.DOMContentLoaded += this.WebViewDOMContentLoadedEventHandler;
            this.WebView.DomContentChanged += this.WebViewDomContentChangedEventHandler;
            this.WebView.DocumentCompleted += this.WebViewDocumentCompletedEventHandler;

            this.Controls.Add (this.WebView);
        }
        #endregion

        #region --公开方法--
        public void CreateTask (Services.Configuration.Models.Action[] actions)
        {
            this.runningActions = actions;
            this.runningTask = new Task<bool> (this.ExecuteTask, this, TaskCreationOptions.LongRunning);
            this.runningTask.ContinueWith (task =>
            {
                if (task.IsFaulted)
                {
                    this.Log.Error (task.Exception.InnerException, "自动化任务运行错误");
                    ErrorDialog.ShowException (null, task.Exception.InnerException);
                }

                if (task.IsCompleted && task.Result)
                {
                    this.Log.Information ($"AutomaticTask running success.");
                }
                else
                {
                    this.Log.Warning ($"AutomaticTask running fail.");
                }
            }, TaskScheduler.FromCurrentSynchronizationContext ());
            this.Log.Information ($"AutomaticTask running thread in created, id: {this.runningTask.Id}.");
        }

        public void RunTask ()
        {
            this.runningTask.Start ();
            this.Log.Information ($"AutomaticTask running thread in started.");
            this.runningTask.Wait ();
        }
        #endregion

        #region --私有方法--
        private bool ExecuteTask (object obj)
        {
            if (obj is WebViewTabPage page)
            {
                // 执行任务
                int actionSerialNumber = 0;
                foreach (Services.Configuration.Models.Action action in page.runningActions)
                {
                    actionSerialNumber += 1;

                    // 元素查找器判断
                    if (action.Element == null)
                    {
                        page.Log.Warning ($"AutomaticTask config in action “{actionSerialNumber}” not found “element” field.");
                        return false;
                    }

                    // 条件同步器
                    if (action.Condition != null)
                    {
                        this.conditionSynchronous.SetCondition (action.Condition);
                        this.conditionSynchronous.WaitOne ();
                    }

                    // 执行元素查找
                    IAsyncResult asyncResult = page.WebView.BeginInvoke (new Func<Services.Configuration.Models.Element, GeckoNode[]> ((element) =>
                    {
                        return SearchCommand.CreateCommand (page.WebView, page.WebView.DomDocument, element, page.Log).Execute ();
                    }), action.Element);

                    page.Log.Information ($"AutomaticTask starts action “{actionSerialNumber}”");

                    // 执行操作
                    if (page.WebView.EndInvoke (asyncResult) is GeckoNode[] nodes)
                    {
                        foreach (GeckoNode node in nodes)
                        {
                            int jobSerialNumber = 0;
                            foreach (Job job in action.Jobs)
                            {
                                jobSerialNumber += 1;

                                if (!JobCommand.CreateCommand (page.WebView, node, job, page.Log).Execute ())
                                {
                                    IAsyncResult asyncResult1 = page.WebView.BeginInvoke (() =>
                                    {
                                        page.Log.Warning ($"AutomaticTask action “{actionSerialNumber}” the job “{jobSerialNumber}” on node “{node.NodeName}” a fails to be executed.");
                                    });
                                    page.WebView.EndInvoke (asyncResult1);
                                    return false;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        #region 状态类事件
        // 状态文本改变事件
        private void WebViewStatusTextChangedEventHandler (object sender, EventArgs e)
        {
            if (Application.OpenForms[0] is MainForm form)
            {
                form.Text = HttpUtility.UrlDecode (this.WebView.StatusText);
            }
        }

        // 就绪状态改变事件
        private void WebViewReadyStateChangeEventHandler (object sender, DomEventArgs e)
        {
            this.Log.Information ($"Browser ready state: {this.WebView.Document.ReadyState}.");

            // 文档加载完毕进行一次重绘, 防止网页错位
            if (this.WebView.Document.ReadyState == "complete")
            {
                this.WebView.Invalidate ();
            }

            // 尝试同步执行线程
            this.conditionSynchronous.TryUnsetCondition (ConditionType.Ready, this.WebView.Document.ReadyState);
        }

        // 文档标题改变事件
        private void WebViewDocumentTitleChangedEventHandler (object sender, EventArgs e)
        {
            this.Log.Information ($"Browser document title: {this.WebView.Document.Title}.");

            this.Text = this.WebView.Document.Title;
        }
        #endregion

        #region 导航类事件
        // 浏览器内部 iframe 导航事件
        private void WebViewFrameNavigatingEventHandler (object sender, Gecko.Events.GeckoNavigatingEventArgs e)
        {
            this.Log.Information ($"Browser frame navigating: {e.Uri}.");
        }

        // 浏览器地址导航事件
        private void WebViewNavigatingEventHandler (object sender, Gecko.Events.GeckoNavigatingEventArgs e)
        {
            this.Log.Information ($"Browser navigating: {e.Uri}.");
        }

        // 创建窗口事件
        private void WebViewCreateWindowEventHandler (object sender, GeckoCreateWindowEventArgs e)
        {
            this.Log.Information ($"Browser create new window redirection to new tab.");

            if (this.Parent is WebViewTabControls tabControls)
            {
                // 创建新的标签
                WebViewTabPage page = new (this.Log);
                tabControls.TabPages.Add (page);
                tabControls.SelectTab (page);

                // 将浏览器导航到新的标签页
                e.WebBrowser = page.WebView;

                // TODO: 继续处理任务
            }
        }
        #endregion

        #region 文档类事件
        // 文档内容加载事件
        private void WebViewDOMContentLoadedEventHandler (object sender, DomEventArgs e)
        {
            this.Log.Information ($"Browser docuemnt content loaded.");
        }

        // 文档内容改变事件
        private void WebViewDomContentChangedEventHandler (object sender, DomEventArgs e)
        {
            this.Log.Information ($"Browser document content changed.");
        }

        // 文档加载完成事件
        private void WebViewDocumentCompletedEventHandler (object sender, Gecko.Events.GeckoDocumentCompletedEventArgs e)
        {
            this.Log.Information ($"Browser document completed.");
        }
        #endregion
        #endregion

        #region --内部类--
        private class ConditionSynchronous
        {
            private readonly AutoResetEvent synchronousEvent = new (true);
            private readonly object lockObject = new ();
            private Condition condition;

            public void WaitOne ()
            {
                // 有条件才进行阻塞
                if (this.condition != null)
                {
                    if (this.condition.Type == ConditionType.Timeout && this.condition.Value.ValueKind == JsonValueKind.Number)
                    {
                        // 获取等待的时间
                        int time = this.condition.Value.Deserialize<int> ();

                        // 阻塞线程一定的时长
                        this.synchronousEvent.WaitOne (time);
                    }
                    else
                    {
                        this.synchronousEvent.WaitOne ();
                    }
                }
            }

            public void SetCondition (Condition condition)
            {
                lock (this.lockObject)
                {
                    this.condition = condition;
                }
                this.synchronousEvent.Reset ();
            }

            public void TryUnsetCondition (ConditionType condition, object value)
            {
                lock (this.lockObject)
                {
                    // 有条件才进行尝试解除阻塞
                    if (this.condition != null)
                    {
                        // 尝试解除阻塞的条件类型必须一致
                        if (this.condition.Type == condition)
                        {
                            bool unset = false;
                            switch (this.condition.Type)
                            {
                                case ConditionType.Ready: unset = "complete".Equals (value); break;
                            }

                            if (unset)
                            {
                                this.synchronousEvent.Set ();
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
