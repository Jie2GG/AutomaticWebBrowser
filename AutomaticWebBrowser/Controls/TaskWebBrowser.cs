using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Commands.OperationCommands;
using AutomaticWebBrowser.Exceptions;
using AutomaticWebBrowser.Models;

using Gecko;
using Gecko.DOM;
using Gecko.Events;
using Gecko.WebIDL;

using Serilog.Core;

using Action = System.Action;

namespace AutomaticWebBrowser.Controls
{
    /// <summary>
    /// 表示任务Web浏览器的类
    /// </summary>
    public class TaskWebBrowser : GeckoWebBrowser
    {
        #region --字段--
        private readonly ConditionSynchronous conditionSynchronous;
        #endregion

        #region --属性--
        /// <summary>
        /// 获取要执行的任务
        /// </summary>
        public TaskInfo TaskInfo { get; }
        public Logger Log { get; }

        /// <summary>
        /// 获取正在运行的任务
        /// </summary>
        public Task RunningTask { get; }
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="TaskWebBrowser"/> 类的新实例
        /// </summary>
        /// <param name="taskInfo"></param>
        public TaskWebBrowser (TaskInfo taskInfo, Logger log)
        {
            this.TaskInfo = taskInfo;
            this.Log = log;
            this.UseHttpActivityObserver = true;

            this.conditionSynchronous = new ConditionSynchronous ();
            this.RunningTask = Task.Factory.StartNew (this.ExecuteTask, this, TaskCreationOptions.LongRunning);
            this.RunningTask.ContinueWith (task =>
            {
                if (task.IsFaulted)
                {
                    this.Log.Error (task.Exception.InnerException, "自动化任务运行错误");
                    TaskMessage.ShowException ((Form)this.Parent, task.Exception.InnerException);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext ());
        }
        #endregion

        #region --公开方法--
        // 创建新窗口事件
        protected override void OnCreateWindow (GeckoCreateWindowEventArgs e)
        {
            base.OnCreateWindow (e);
            e.WebBrowser = this;    // 将所有窗体重定向到自己

            this.Log.Information ($"Window: Create new window redirec to this");
        }

        // 状态文本改变事件
        protected override void OnStatusTextChanged (EventArgs e)
        {
            base.OnStatusTextChanged (e);
            this.BeginInvoke (new System.Action (() =>
            {
                if (this.Parent is Form form)
                {
                    form.Text = this.StatusText;
                }
            }));
        }

        // 就绪状态改变事件
        protected override void OnReadyStateChange (DomEventArgs e)
        {
            base.OnReadyStateChange (e);

            this.Log.Information ($"ReadyState: {this.Document.ReadyState}");
            this.conditionSynchronous.Status (ConditionType.ReadyState, this.Document.ReadyState);
        }

        // 文档标题改变事件
        protected override void OnDocumentTitleChanged (EventArgs e)
        {
            base.OnDocumentTitleChanged (e);

            this.Log.Information ($"DocumentTitle: {this.DocumentTitle}");
            this.conditionSynchronous.Status (ConditionType.DocumentTitle, this.DocumentTitle);
        }
        #endregion

        #region --私有方法--
        private void ExecuteTask (object obj)
        {
            if (obj is TaskWebBrowser webBrowser)
            {
                int count = 0;
                foreach (Models.Action action in webBrowser.TaskInfo.Actions)
                {
                    count += 1;

                    string actionName = action.Name ?? $"No name {count}";

                    if (action.Element == null)
                    {
                        throw new ConfigNodeException ($"Action: {actionName} 没有指定 Node 节点");
                    }

                    // 处理执行条件
                    if (action.Condition != null)
                    {
                        // 设置新的条件
                        webBrowser.conditionSynchronous.Condition (action.Condition);

                        // 等待状态同步
                        webBrowser.conditionSynchronous.WaitOne ();
                    }

                    // 查找 DOM 元素
                    IAsyncResult result = webBrowser.BeginInvoke (new Func<GeckoNode[]> (() =>
                    {
                        SearchCommand searchCommand = SearchCommand.CreateCommand (webBrowser, webBrowser.Document, action.Element);
                        searchCommand.Execute ();
                        return searchCommand.SearchResult;
                    }));
                    if (webBrowser.EndInvoke (result) is GeckoNode[] elements)
                    {
                        // 遍历所有节点
                        foreach (GeckoNode element in elements)
                        {
                            // 对节点的执行操作
                            foreach (Models.Operation option in action.Operations)
                            {
                                OperationCommand.CreateCommand (webBrowser, element, option)
                                    .Execute ();

                                // 延迟 1ms 作为缓冲
                                Thread.Sleep (1);
                            }
                        }
                    }

                    // 重置条件同步
                    if (action.Condition != null)
                    {
                        webBrowser.conditionSynchronous.Reset ();
                    }
                }
            }
        }
        #endregion

        #region --内部类--
        /// <summary>
        /// 浏览器操作类
        /// </summary>
        public static class Option
        {
            /// <summary>
            /// 浏览器等待
            /// </summary>
            /// <param name="browser"></param>
            /// <param name="value"></param>
            public static void Waiting (TaskWebBrowser browser, int value)
            {
                browser.Log.Information ($"Option: Waiting {value} ms");
                Thread.Sleep (value);
            }

            /// <summary>
            /// 元素获取焦点
            /// </summary>
            /// <param name="browser"></param>
            /// <param name="node"></param>
            public static void Focus (TaskWebBrowser browser, GeckoNode node)
            {
                IAsyncResult result = browser.BeginInvoke (new Action (() =>
                {
                    // 获取焦点
                    if (node is GeckoHtmlElement htmlElement)
                    {
                        browser.Log.Information ($"Option: HtmlElement {htmlElement.NodeName} focus");
                        htmlElement.Focus ();
                    }
                }));

                browser.EndInvoke (result);
            }

            /// <summary>
            /// 元素点击
            /// </summary>
            /// <param name="browser"></param>
            /// <param name="node"></param>
            /// <returns></returns>
            public static void Click (TaskWebBrowser browser, GeckoNode node, int count)
            {
                IAsyncResult result = browser.BeginInvoke (new Action (() =>
                {
                    // 点击元素
                    if (node is GeckoHtmlElement htmlElement)
                    {
                        browser.Log.Information ($"Option: HtmlElement {htmlElement.NodeName} clicks {count}");
                        while (count-- > 0)
                        {
                            htmlElement.Click ();
                        }
                    }
                }));

                browser.EndInvoke (result);
            }

            /// <summary>
            /// 元素输入
            /// </summary>
            /// <param name="browser"></param>
            /// <param name="node"></param>
            /// <param name="value"></param>
            public static void Input (TaskWebBrowser browser, GeckoNode node, string value)
            {
                // 输入字符
                IAsyncResult result = browser.BeginInvoke (new Action (() =>
                {
                    if (node is GeckoInputElement inputElement)
                    {
                        browser.Log.Information ($"Option: HtmlInputElement {inputElement.NodeName} set value {value}");
                        inputElement.Value = value;
                    }
                }));

                browser.EndInvoke (result);
            }

            /// <summary>
            /// 元素模拟输入
            /// </summary>
            /// <param name="browser"></param>
            /// <param name="node"></param>
            /// <param name="value"></param>
            public static void KeypressInput (TaskWebBrowser browser, GeckoNode node, string value)
            {
                // 输入字符
                IAsyncResult result = browser.BeginInvoke (new Action (() =>
                {
                    browser.Log.Information ($"Option: Node {node.NodeName} keypress input value {value}");

                    // 获取文档事件
                    DomUIEventArgs args = GetKeyboardEvents (browser);

                    // 创建按键事件
                    KeyEvent @event = new KeyEvent ((mozIDOMWindowProxy)browser.Window.DomWindow, args.DomEvent as nsISupports);
                    foreach (char c in value.ToCharArray ())
                    {
                        @event.InitKeyEvent (
                            "keypress",
                            true,
                            true,
                            (nsIDOMWindow)browser.Window.DomWindow,
                            false,
                            false,
                            false,
                            false,
                            c,
                            c
                        );
                        node.GetEventTarget ().DispatchEvent (args);
                    }
                }));

                browser.EndInvoke (result);
            }

            /// <summary>
            /// 元素模拟按键按下
            /// </summary>
            /// <param name="browser"></param>
            /// <param name="node"></param>
            /// <param name="key"></param>
            public static void KeyDown (TaskWebBrowser browser, GeckoNode node, KeyInfo key)
            {
                // 按下按键
                IAsyncResult result = browser.BeginInvoke (new Action (() =>
                {
                    browser.Log.Information ($"Option: Node {node.NodeName} key [{key.Key}] down {key.Count} count, control: {key.Control}, alt: {key.Alt}, shift: {key.Shift}, meta: {key.Meta}");

                    // 获取文档事件
                    DomUIEventArgs args = GetKeyboardEvents (browser);

                    // 创建按键事件
                    KeyEvent @event = new KeyEvent ((mozIDOMWindowProxy)browser.Window.DomWindow, args.DomEvent as nsISupports);
                    @event.InitKeyEvent (
                            "keydown",
                            true,
                            false,
                            (nsIDOMWindow)browser.Window.DomWindow,
                            key.Control,
                            key.Alt,
                            key.Shift,
                            key.Meta,
                            (uint)key.Key
                    );
                    while (key.Count-- > 0)
                    {
                        node.GetEventTarget ().DispatchEvent (args);
                    }
                }));

                browser.EndInvoke (result);
            }

            /// <summary>
            /// 元素模拟按键松开
            /// </summary>
            /// <param name="browser"></param>
            /// <param name="node"></param>
            /// <param name="key"></param>
            public static void KeyUp (TaskWebBrowser browser, GeckoNode node, KeyInfo key)
            {
                // 按下按键
                IAsyncResult result = browser.BeginInvoke (new Action (() =>
                {
                    browser.Log.Information ($"Option: Node {node.NodeName} key [{key.Key}] up {key.Count} count, control: {key.Control}, alt: {key.Alt}, shift: {key.Shift}, meta: {key.Meta}");

                    // 获取文档事件
                    DomUIEventArgs args = GetKeyboardEvents (browser);

                    // 创建按键事件
                    KeyEvent @event = new KeyEvent ((mozIDOMWindowProxy)browser.Window.DomWindow, args.DomEvent as nsISupports);
                    @event.InitKeyEvent (
                            "keyup",
                            true,
                            false,
                            (nsIDOMWindow)browser.Window.DomWindow,
                            key.Control,
                            key.Alt,
                            key.Shift,
                            key.Meta,
                            (uint)key.Key
                    );
                    while (key.Count-- > 0)
                    {
                        node.GetEventTarget ().DispatchEvent (args);
                    }
                }));

                browser.EndInvoke (result);
            }

            /// <summary>
            /// 元素模拟按键按下并松开
            /// </summary>
            /// <param name="browser"></param>
            /// <param name="element"></param>
            /// <param name="keyInfo"></param>
            /// <returns></returns>
            public static void KeyPress (TaskWebBrowser browser, GeckoNode node, KeyInfo key)
            {
                // 按下按键
                IAsyncResult result = browser.BeginInvoke (new Action (() =>
                {
                    browser.Log.Information ($"Option: Node {node.NodeName} key [{key.Key}] press {key.Count} count, control: {key.Control}, alt: {key.Alt}, shift: {key.Shift}, meta: {key.Meta}");

                    // 获取文档事件
                    DomUIEventArgs args = GetKeyboardEvents (browser);

                    // 创建按键事件
                    KeyEvent @event = new KeyEvent ((mozIDOMWindowProxy)browser.Window.DomWindow, args.DomEvent as nsISupports);
                    @event.InitKeyEvent (
                            "keypress",
                            true,
                            false,
                            (nsIDOMWindow)browser.Window.DomWindow,
                            key.Control,
                            key.Alt,
                            key.Shift,
                            key.Meta,
                            (uint)key.Key
                    );
                    while (key.Count-- > 0)
                    {
                        node.GetEventTarget ().DispatchEvent (args);
                    }
                }));

                browser.EndInvoke (result);
            }

            /// <summary>
            /// 元素模拟鼠标按键按下
            /// </summary>
            public static void MouseDown (TaskWebBrowser browser, GeckoNode node, ButtonInfo button)
            {
                // 按下按键
                IAsyncResult result = browser.BeginInvoke (new Action (() =>
                {
                    browser.Log.Information ($"Option: Node {node.NodeName} mouse [{button.Button}] button down {button.Count} count, control: {button.Control}, alt: {button.Alt}, shift: {button.Shift}, meta: {button.Meta}");

                    // 获取文档事件
                    DomMouseEventArgs args = GetMouseEvents (browser);

                    // 创建按键事件
                    MouseEvent @event = new MouseEvent ((mozIDOMWindowProxy)browser.Window.DomWindow, args.DomEvent as nsISupports);
                    @event.InitMouseEvent (
                        "mousedown",
                        true,
                        true,
                        (nsIDOMWindow)browser.Window.DomWindow,
                        1,
                        0,
                        0,
                        0,
                        0,
                        button.Control,
                        button.Alt,
                        button.Shift,
                        button.Meta,
                        (short)button.Button);

                    while (button.Count-- > 0)
                    {
                        node.GetEventTarget ().DispatchEvent (args);
                    }

                }));

                browser.EndInvoke (result);
            }

            /// <summary>
            /// 元素模拟鼠标按键松开
            /// </summary>
            /// <param name="browser"></param>
            /// <param name="node"></param>
            /// <param name="button"></param>
            public static void MouseUp (TaskWebBrowser browser, GeckoNode node, ButtonInfo button)
            {
                // 按下按键
                IAsyncResult result = browser.BeginInvoke (new Action (() =>
                {
                    browser.Log.Information ($"Option: Node {node.NodeName} mouse [{button.Button}] button up {button.Count} count, control: {button.Control}, alt: {button.Alt}, shift: {button.Shift}, meta: {button.Meta}");

                    // 获取文档事件
                    DomMouseEventArgs args = GetMouseEvents (browser);

                    // 创建按键事件
                    MouseEvent @event = new MouseEvent ((mozIDOMWindowProxy)browser.Window.DomWindow, args.DomEvent as nsISupports);
                    @event.InitMouseEvent (
                        "mouseup",
                        true,
                        true,
                        (nsIDOMWindow)browser.Window.DomWindow,
                        1,
                        0,
                        0,
                        0,
                        0,
                        button.Control,
                        button.Alt,
                        button.Shift,
                        button.Meta,
                        (short)button.Button);

                    while (button.Count-- > 0)
                    {
                        node.GetEventTarget ().DispatchEvent (args);
                    }

                }));

                browser.EndInvoke (result);
            }

            /// <summary>
            /// 元素模拟鼠标点击
            /// </summary>
            /// <param name="browser"></param>
            /// <param name="node"></param>
            /// <param name="button"></param>
            public static void MouseClick (TaskWebBrowser browser, GeckoNode node, ButtonInfo button)
            {
                // 按下按键
                IAsyncResult result = browser.BeginInvoke (new Action (() =>
                {
                    browser.Log.Information ($"Option: Node {node.NodeName} mouse [{button.Button}] button click {button.Count} count, control: {button.Control}, alt: {button.Alt}, shift: {button.Shift}, meta: {button.Meta}");

                    // 获取文档事件
                    DomMouseEventArgs args = GetMouseEvents (browser);

                    // 创建按键事件
                    MouseEvent @event = new MouseEvent ((mozIDOMWindowProxy)browser.Window.DomWindow, args.DomEvent as nsISupports);
                    @event.InitMouseEvent (
                        "click",
                        true,
                        true,
                        (nsIDOMWindow)browser.Window.DomWindow,
                        1,
                        0,
                        0,
                        0,
                        0,
                        button.Control,
                        button.Alt,
                        button.Shift,
                        button.Meta,
                        (short)button.Button);

                    while (button.Count-- > 0)
                    {
                        node.GetEventTarget ().DispatchEvent (args);
                    }

                }));

                browser.EndInvoke (result);
            }

            /// <summary>
            /// 获取键盘事件
            /// </summary>
            /// <param name="browser"></param>
            /// <returns></returns>
            private static DomUIEventArgs GetKeyboardEvents (TaskWebBrowser browser)
            {
                return browser.Document.CreateEvent ("KeyEvents") as DomUIEventArgs;
            }

            /// <summary>
            /// 获取鼠标事件
            /// </summary>
            /// <param name="browser"></param>
            /// <returns></returns>
            private static DomMouseEventArgs GetMouseEvents (TaskWebBrowser browser)
            {
                return browser.Document.CreateEvent ("MouseEvent") as DomMouseEventArgs;
            }
        }

        /// <summary>
        /// 条件检查同步类
        /// </summary>
        private class ConditionSynchronous
        {
            private readonly AutoResetEvent waitChagneEvent = new AutoResetEvent (true);
            private readonly AutoResetEvent waitEvent = new AutoResetEvent (true);
            private Condition condition;

            /// <summary>
            /// 等待状态同步
            /// </summary>
            public void WaitOne ()
            {
                this.waitEvent.WaitOne ();
            }

            /// <summary>
            /// 设置同步条件
            /// </summary>
            /// <param name="condition"></param>
            public void Condition (Condition condition)
            {
                this.waitChagneEvent.WaitOne ();
                this.waitChagneEvent.Reset ();

                this.condition = condition;
                this.waitEvent.Reset ();

                this.waitChagneEvent.Set ();
            }

            /// <summary>
            /// 更新条件状态
            /// </summary>
            /// <param name="type"></param>
            /// <param name="value"></param>
            public void Status (ConditionType type, object value)
            {
                this.waitChagneEvent.WaitOne ();
                this.waitChagneEvent.Reset ();

                if (this.condition != null)
                {
                    if (this.condition.Type == type)
                    {
                        bool result = false;
                        switch (this.condition.Type)
                        {
                            // 就绪状态的比较
                            case ConditionType.ReadyState:
                                result = string.Equals (this.condition.Value.GetString (), (string)value);
                                break;
                            case ConditionType.DocumentTitle:
                                result = string.Equals (this.condition.Value.GetString (), (string)value);
                                break;
                        }

                        if (result)
                        {
                            this.waitEvent.Set ();
                        }
                    }
                }

                this.waitChagneEvent.Set ();
            }

            /// <summary>
            /// 重置条件
            /// </summary>
            public void Reset ()
            {
                this.condition = null;
                this.waitEvent.Set ();
            }
        }
        #endregion
    }
}
