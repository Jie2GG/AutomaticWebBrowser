using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using AutomaticWebBrowser.Services.AutomaicTasks.Commands.JobCommands;
using AutomaticWebBrowser.Services.AutomaicTasks.Commands.SearchCommands;
using AutomaticWebBrowser.Services.Configuration.Models;

using Gecko;
using Gecko.Events;

using Serilog.Core;

using Action = System.Action;

namespace AutomaticWebBrowser.Controls
{
    class WebView1 : GeckoWebBrowser
    {
        #region --常量--
        public const string DOM_KEY_EVENTS = "KeyEvents";
        public const string DOM_MOUSE_EVENT = "MouseEvent";

        public const string EVENT_FOCUS = "focus";
        public const string EVENT_BLUR = "blur";
        public const string EVENT_KEY_PRESS = "keypress";
        public const string EVENT_KEY_DOWN = "keydown";
        public const string EVENT_KEY_UP = "keyup";
        public const string EVENT_MOUSE_DOWN = "mousedown";
        public const string EVENT_MOUSE_UP = "mouseup";
        public const string EVENT_CLICK = "click";
        public const string EVENT_DBLCLICK = "dblclick";
        #endregion

        #region --字段--
        private readonly ConditionSynchronous conditionSynchronous;
        private Task<bool> runningTask;
        private AutomaticTask[] tasks;
        #endregion

        #region --属性--
        public Logger Log { get; set; }

        public Form Form { get; set; }
        #endregion

        #region --构造函数--
        public WebView1 ()
        {
            this.conditionSynchronous = new ConditionSynchronous ();
        }
        #endregion

        #region --公开方法--
        public void CreateTask (params AutomaticTask[] tasks)
        {
            // 赋值
            this.tasks = tasks ?? throw new ArgumentNullException (nameof (tasks));

            // 创建任务
            this.runningTask = new Task<bool> (this.ExecuteTask, this, TaskCreationOptions.LongRunning);
            this.Log.Information ($"AutomaticTask running thread in created, id: {this.runningTask.Id}.");
            this.runningTask.ContinueWith (task =>
            {
                if (task.IsFaulted)
                {
                    this.Log.Error (task.Exception.InnerException, "自动化任务运行错误");
                    ErrorDialog.ShowException ((Form)this.Parent, task.Exception.InnerException);
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
        }

        public void RunningTask ()
        {
            // 启动任务
            this.runningTask.Start ();
        }
        #endregion

        #region --私有方法--
        private bool ExecuteTask (object obj)
        {
            if (obj is WebView1 webView)
            {
                foreach (AutomaticTask task in webView.tasks)
                {
                    // 导航到指定的 URL
                    webView.Invoke (new Action (() => this.Navigate (task.Url)));

                    // 执行任务
                    int actionSerialNumber = 0;
                    foreach (Services.Configuration.Models.Action action in task.Actions)
                    {
                        actionSerialNumber += 1;

                        // 元素查找器判断
                        if (action.Element == null)
                        {
                            webView.Log.Warning ($"AutomaticTask config in action “{actionSerialNumber}” not found “element” field.");
                            return false;
                        }

                        // 条件同步器
                        if (action.Condition != null)
                        {
                            this.conditionSynchronous.SetCondition (action.Condition);
                            this.conditionSynchronous.WaitOne ();
                        }

                        // 执行元素查找
                        IAsyncResult asyncResult = webView.BeginInvoke (new Func<Services.Configuration.Models.Element, GeckoNode[]> ((element) =>
                        {
                            return SearchCommand.CreateCommand (webView, webView.DomDocument, element, webView.Log).Execute ();
                        }), action.Element);

                        webView.Log.Information ($"AutomaticTask starts action “{actionSerialNumber}”");

                        // 执行操作
                        if (webView.EndInvoke (asyncResult) is GeckoNode[] nodes)
                        {
                            foreach (GeckoNode node in nodes)
                            {
                                int jobSerialNumber = 0;
                                foreach (Job job in action.Jobs)
                                {
                                    jobSerialNumber += 1;

                                    if (!JobCommand.CreateCommand (webView, node, job, webView.Log).Execute ())
                                    {
                                        IAsyncResult asyncResult1 = webView.BeginInvoke (() =>
                                        {
                                            webView.Log.Warning ($"AutomaticTask action “{actionSerialNumber}” the job “{jobSerialNumber}” on node “{node.NodeName}” a fails to be executed.");
                                        });
                                        webView.EndInvoke (asyncResult1);
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        #region 状态类事件
        // 状态文本改变事件
        protected override void OnStatusTextChanged (EventArgs e)
        {
            this.Form.BeginInvoke (new Action (() => this.Form.Text = this.StatusText));
        }

        // 就绪状态改变事件
        protected override void OnReadyStateChange (DomEventArgs e)
        {
            this.Log.Information ($"Browser ready state: {this.Document.ReadyState}.");

            // 文档加载完毕进行一次重绘, 防止网页错位
            if (this.Document.ReadyState == "complete")
            {
                this.Invalidate ();
            }

            // 尝试同步执行线程
            this.conditionSynchronous.TryUnsetCondition (ConditionType.Ready, this.Document.ReadyState);
        }

        // 文档标题改变事件
        protected override void OnDocumentTitleChanged (EventArgs e)
        {
            this.Log.Information ($"Browser document title: {this.Document.Title}.");
        }
        #endregion

        #region 导航类事件
        
        protected override void OnNavigating (GeckoNavigatingEventArgs e)
        {
            this.Log.Information ($"Browser navigating: {e.Uri}.");
        }

        // 浏览器内部 iframe 导航事件
        protected override void OnFrameNavigating (GeckoNavigatingEventArgs e)
        {
            this.Log.Information ($"Browser frame navigating: {e.Uri}.");
        }

        // 创建窗口事件
        protected override void OnCreateWindow (GeckoCreateWindowEventArgs e)
        {
            this.Log.Information ($"Browser create new window redirection to oneself.");

            e.WebBrowser = this;
        }
        #endregion

        #region 文档类事件
        // 文档内容加载事件
        protected override void OnDOMContentLoaded (DomEventArgs e)
        {
            this.Log.Information ($"Browser docuemnt content loaded.");
        }

        // 文档内容改变事件
        protected override void OnDomContentChanged (DomEventArgs e)
        {
            this.Log.Information ($"Browser document content changed.");
        }

        // 文档加载完成事件
        protected override void OnDocumentCompleted (GeckoDocumentCompletedEventArgs e)
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
                    if (this.condition.Type == ConditionType.Delay && this.condition.Value.ValueKind == JsonValueKind.Number)
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
