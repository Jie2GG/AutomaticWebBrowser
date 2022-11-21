using System;
using System.Diagnostics;
using System.Drawing;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

using AutomaticWebBrowser.Commands;
using AutomaticWebBrowser.Exceptions;
using AutomaticWebBrowser.Models;

using Gecko;
using Gecko.DOM;
using Gecko.WebIDL;

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
        public TaskWebBrowser (TaskInfo taskInfo)
        {
            this.TaskInfo = taskInfo;
            this.UseHttpActivityObserver = true;

            this.conditionSynchronous = new ConditionSynchronous ();
            this.RunningTask = Task.Factory.StartNew (this.ExecuteTask, this, TaskCreationOptions.LongRunning);
            this.RunningTask.ContinueWith (task =>
            {
                if (task.IsFaulted)
                {
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
            this.conditionSynchronous.Status (ConditionType.ReadyState, this.Document.ReadyState);
            Debug.WriteLine ($"就绪: {this.Document.ReadyState}");
        }

        // DOM文档加载完成事件
        protected override void OnDOMContentLoaded (DomEventArgs e)
        {
            base.OnDOMContentLoaded (e);

            Debug.WriteLine ("DOM 加载完成");
        }

        // DOM文档被改变事件
        protected override void OnDomContentChanged (DomEventArgs e)
        {
            base.OnDomContentChanged (e);

            Debug.WriteLine ("DOM 内容改变");
        }

        // 监听HTTP请求事件
        protected override void OnObserveHttpModifyRequest (GeckoObserveHttpModifyRequestEventArgs e)
        {
            base.OnObserveHttpModifyRequest (e);

            //Debug.WriteLine ($"HTTP: {e.RequestMethod},  {e.Uri}");
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
                    IAsyncResult result = webBrowser.BeginInvoke (new Func<GeckoNode> (() =>
                    {
                        switch (action.Element.SearchType)
                        {
                            case SearchType.DomDocument:
                                {
                                    return webBrowser.Document;
                                }
                            case SearchType.XPath:
                                {
                                    string xpath = action.Element.Value.Deserialize<string> (GlobalConfig.JsonSerializerOptions);
                                    return webBrowser.Document.SelectFirst (xpath);
                                }
                            case SearchType.ElementId:
                                {
                                    string elementId = action.Element.Value.Deserialize<string> (GlobalConfig.JsonSerializerOptions);
                                    return webBrowser.Document.GetElementById (elementId);
                                }
                            default:
                                throw new ConfigNodeException ($"Action: {actionName} 的 Node 中没有设置正确的元素搜索方式");
                        }
                    }));
                    if (webBrowser.EndInvoke (result) is GeckoNode element)
                    {
                        // 对元素的执行操作
                        foreach (Models.Option option in action.Options)
                        {
                            OptionCommand.CreateCommand (webBrowser, element, option)
                                .Execute ();

                            // 延迟 1ms 作为缓冲
                            Thread.Sleep (1);
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
        public static class Option
        {
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
            public static void SimulateInput (TaskWebBrowser browser, GeckoNode node, string value)
            {
                // 输入字符
                IAsyncResult result = browser.BeginInvoke (new Action (() =>
                {
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
            public static void MouseDown (TaskWebBrowser browser, GeckoNode node, MouseKeyInfo mouseKey)
            {
                // 按下按键
                IAsyncResult result = browser.BeginInvoke (new Action (() =>
                {
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
                        mouseKey.Control,
                        mouseKey.Alt,
                        mouseKey.Shift,
                        mouseKey.Meta,
                        (short)mouseKey.Button);

                    while (mouseKey.Count-- > 0)
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
            /// <param name="mouseKey"></param>
            public static void MouseUp (TaskWebBrowser browser, GeckoNode node, MouseKeyInfo mouseKey)
            {
                // 按下按键
                IAsyncResult result = browser.BeginInvoke (new Action (() =>
                {
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
                        mouseKey.Control,
                        mouseKey.Alt,
                        mouseKey.Shift,
                        mouseKey.Meta,
                        (short)mouseKey.Button);

                    while (mouseKey.Count-- > 0)
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
            /// <param name="mouseKey"></param>
            public static void MouseClick (TaskWebBrowser browser, GeckoNode node, MouseKeyInfo mouseKey)
            {
                // 按下按键
                IAsyncResult result = browser.BeginInvoke (new Action (() =>
                {
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
                        mouseKey.Control,
                        mouseKey.Alt,
                        mouseKey.Shift,
                        mouseKey.Meta,
                        (short)mouseKey.Button);

                    while (mouseKey.Count-- > 0)
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
