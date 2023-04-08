using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

using AutomaticWebBrowser.Common;
using AutomaticWebBrowser.Services.AutomaicTasks.Commands.JobCommands;
using AutomaticWebBrowser.Services.AutomaicTasks.Commands.SearchCommands;
using AutomaticWebBrowser.Services.Configuration.Models;
using AutomaticWebBrowser.Services.Loggers;

using Gecko;

using Serilog.Core;

using Action = AutomaticWebBrowser.Services.Configuration.Models.Action;
using Element = AutomaticWebBrowser.Services.Configuration.Models.Element;

namespace AutomaticWebBrowser.Views
{
    /// <summary>
    /// 浏览器窗口
    /// </summary>
    partial class BrowserForm : Form
    {
        #region --字段--
        private readonly ConditionSynchronous conditionSynchronous;
        #endregion

        #region --属性--
        /// <summary>
        /// 配置
        /// </summary>
        public Config Config { get; }

        /// <summary>
        /// 日志
        /// </summary>
        public Logger Log { get; }

        /// <summary>
        /// Web浏览器
        /// </summary>
        public GeckoWebBrowser AutomaticWebBrowser => this.automaticWebBrowser;

        /// <summary>
        /// 浏览器等待处理事件
        /// </summary>
        public BrowserWaitHandle BrowserWaitHandle { get; set; }
        #endregion

        #region --构造函数--
        public BrowserForm (Config config, Logger log, bool childForm = false)
        {
            // 赋值
            this.conditionSynchronous = new ConditionSynchronous ();
            this.Config = config ?? throw new ArgumentNullException (nameof (config));
            this.Log = log ?? throw new ArgumentNullException (nameof (log));

            // 初始化控件
            this.InitializeComponent ();
            if (!(childForm && !this.Config.Browser.ApplyToChildWindow))
            {
                // 设置窗体状态
                this.WindowState = this.Config.Browser.Window.State;
                // 设置窗体位置
                this.Location = this.Config.Browser.Window.Location ?? WindowLocation.Empty;
                // 设置窗体客户区大小
                this.Size = this.Config.Browser.Window.Size ?? WindowSize.MainFormSize;
                // 设置窗体初始位置
                this.StartPosition = this.Config.Browser.Window.StartPosition;
            }
        }
        #endregion

        #region --公开方法--
        /// <summary>
        /// 使用线程安全的方式显示窗体
        /// </summary>
        public void SafeShow (Control safeControl)
        {
            safeControl.Invoke (new System.Action (() => this.Show ()));
            this.Log.Information ($"浏览器 --> 打开");
        }

        /// <summary>
        /// 使用线程安全的方式关闭窗体
        /// </summary>
        public void SafeClose (Control safeControl)
        {
            safeControl.Invoke (new System.Action (() =>
            {
                this.Close ();
                this.Dispose ();
            }));
            this.Log.Information ($"浏览器 --> 关闭");
        }

        /// <summary>
        /// 使用线程安全的方式导航到指定的地址
        /// </summary>
        /// <param name="url">地址</param>
        public void SafeNavigate (string url)
        {
            this.AutomaticWebBrowser.Invoke (new System.Action (() => this.AutomaticWebBrowser.Navigate (url)));
            this.Log.Information ($"浏览器 --> 打开网址: {url}");
        }

        /// <summary>
        /// 运行动作
        /// </summary>
        /// <param name="actions"></param>
        public void RunActions (params Action[] actions)
        {
            #region 预处理动作
            this.Log.Information ($"任务管理器 --> 预处理任务动作");
            for (int i = 0; i < actions.Length; i++)
            {
                // 处理每个动作的名称
                if (actions[i].Name == null)
                {
                    actions[i].Name = $"Action: {i + 1}";
                }

                // 处理每个作业的名称
                for (int j = 0; j < actions[i].Jobs.Length; j++)
                {
                    if (actions[i].Jobs[j].Name == null)
                    {
                        actions[i].Jobs[j].Name = $"Job: {j + 1}";
                    }
                }
            }
            #endregion

            #region 开始执行动作
            foreach (Action action in actions)
            {
                // 处理动作的执行条件
                if (action.Condition != null)
                {
                    this.conditionSynchronous.SetCondition (action.Condition);
                    this.Log.Information ($"自动化任务 --> 处理任务条件: {action.Condition.Type}");
                    this.conditionSynchronous.WaitOne ();
                }

                // 查找网页元素
                if (action.Element != null)
                {
                    GeckoNode[] nodes = this.SafeSearchNodes (action.Element);
                    foreach (GeckoNode node in nodes)
                    {
                        foreach (Job job in action.Jobs)
                        {
                            if (JobCommand.CreateCommand (this, node, job, this.Log).Execute ())
                            {
                                this.AutomaticWebBrowser.Invoke (new System.Action (() =>
                                {
                                    this.Log.Information ($"自动化任务 --> 在节点 {node.NodeName} 执行动作 {action.Name} 的作业 {job.Name} 成功");
                                }));
                            }
                            else
                            {
                                this.AutomaticWebBrowser.Invoke (new System.Action (() =>
                                {
                                    this.Log.Warning ($"自动化任务 --> 在节点 {node.NodeName} 执行动作 {action.Name} 的作业 {job.Name} 失败");
                                }));
                            }
                        }
                    }
                }
            }
            #endregion
        }
        #endregion

        #region --私有方法--
        private GeckoNode[] SafeSearchNodes (Element element)
        {
            IAsyncResult asyncResult = this.AutomaticWebBrowser.BeginInvoke (new Func<GeckoNode[]> (() =>
            {
                return SearchCommand.CreateCommand (this.AutomaticWebBrowser, this.AutomaticWebBrowser.DomDocument, element, this.Log).Execute ();
            }));

            if (this.AutomaticWebBrowser.EndInvoke (asyncResult) is GeckoNode[] nodes)
            {
                return nodes;
            }

            return Array.Empty<GeckoNode> ();
        }
        #endregion

        #region --事件处理--
        // 状态文本改变事件
        private void AutomaticWebBrowserStatusTextChangedEventHandler (object sender, EventArgs e)
        {
            this.Invoke (new System.Action (() =>
            {
                this.Text = HttpUtility.UrlDecode (this.automaticWebBrowser.StatusText);
            }));
        }

        // 就绪状态改变事件
        private void AutomaticWebBrowserReadyStateChangeEventHandler (object sender, DomEventArgs e)
        {
            this.Log.Debug ($"浏览器 --> 就绪状态: {this.automaticWebBrowser.Document.ReadyState}");

            // 文档加载完毕进行一次重绘, 防止网页错位
            if (this.automaticWebBrowser.Document.ReadyState == "complete")
            {
                this.automaticWebBrowser.Invalidate ();
            }

            // 执行线程同步
            this.conditionSynchronous.TryUnsetCondition (ConditionType.Ready, this.automaticWebBrowser.Document.ReadyState);
        }

        // 创建新窗体事件
        private void AutomaticWebBrowserCreateWindowEventHandler (object sender, GeckoCreateWindowEventArgs e)
        {
            this.Log.Information ($"浏览器 --> 创建新窗体");

            // 将窗体浏览器作为新地址的承载浏览器
            e.WebBrowser = this.BrowserWaitHandle.Form.AutomaticWebBrowser;

            // 发送信号
            this.BrowserWaitHandle.Set ();

        }
        #endregion

        #region --内部类--
        private class ConditionSynchronous
        {
            private readonly AutoResetEvent synchronousEvent = new AutoResetEvent (true);
            private readonly object lockObject = new object ();
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
