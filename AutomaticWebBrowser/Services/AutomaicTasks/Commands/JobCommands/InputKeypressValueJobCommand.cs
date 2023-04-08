using System;
using System.Text.Json;
using System.Windows.Forms;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Services.Configuration.Models;
using AutomaticWebBrowser.Views;

using Gecko;
using Gecko.DOM;
using Gecko.Interop;
using Gecko.WebIDL;

using Serilog.Core;

namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands.JobCommands
{
    [JobCommand (JobType.InputKeypressValue)]
    class InputKeypressValueJobCommand : JobCommand
    {
        public InputKeypressValueJobCommand (BrowserForm form, GeckoNode node, Job job, Logger log)
            : base (form, node, job, log)
        { }

        public override bool Execute ()
        {
            if (this.Job.Value.ValueKind == JsonValueKind.String)
            {
                string inputValue = this.Job.Value.Deserialize<string> (Global.JsonSerializerOptions);

                if (this.Node is GeckoInputElement inputElement)
                {
                    IAsyncResult asyncResult = this.Browser.BeginInvoke (new Func<bool> (() =>
                    {
                        try
                        {
                            // 创建键盘输入事件
                            DomEventArgs eventArgs = this.Browser.DomDocument.CreateEvent (Global.DOM.DOM_KEY_EVENTS);

                            // DOM事件
                            Event domEvent = new Event (this.Browser.Window.DomWindow as mozIDOMWindowProxy, eventArgs.DomEvent as nsISupports);
                            // 获取焦点
                            domEvent.InitEvent (Global.DOM.EVENT_FOCUS, true, false);
                            inputElement.GetEventTarget ().DispatchEvent (eventArgs);
                            this.Log.Information ($"自动化任务 --> 在节点 {this.Node.NodeName} 执行 “inputKeypressValue” 作业, 节点触发 focus");

                            // 按键事件
                            KeyEvent keyEvent = new KeyEvent (this.Browser.Window.DomWindow as mozIDOMWindowProxy, eventArgs.DomEvent as nsISupports);
                            foreach (char c in inputValue.ToCharArray ())
                            {
                                keyEvent.InitKeyEvent (
                                    Global.DOM.EVENT_KEY_PRESS,
                                    true,
                                    false,
                                    this.Browser.Window.DomWindow as nsIDOMWindow,
                                    false,
                                    false,
                                    false,
                                    false,
                                    c,
                                    c
                                );
                                inputElement.GetEventTarget ().DispatchEvent (eventArgs);
                            }
                            this.Log.Information ($"自动化任务 --> 在节点 {this.Node.NodeName} 执行 “inputKeypressValue” 作业, 节点输入值是: {inputValue}");

                            // 失去焦点
                            domEvent.InitEvent (Global.DOM.EVENT_BLUR, true, false);
                            inputElement.GetEventTarget ().DispatchEvent (eventArgs);
                            this.Log.Information ($"自动化任务 --> 在节点 {this.Node.NodeName} 执行 “inputKeypressValue” 作业, 节点触发 blur");

                            // 处理事件
                            Application.DoEvents ();
                            return true;
                        }
                        catch (Exception e)
                        {
                            this.Log.Error (e, $"自动化任务 --> 在节点 {this.Node.NodeName} 执行 “inputKeypressValue” 作业, 但在执行过程中出现了异常");
                            return false;
                        }
                    }));

                    return this.Browser.EndInvoke (asyncResult) as bool? ?? false;
                }
                else
                {
                    this.Log.Warning ($"自动化任务 --> 在节点 {this.Node.NodeName} 执行 “inputKeypressValue” 作业, 但节点不是 InputElement 类型");
                }
            }
            else
            {
                this.Log.Warning ($"自动化任务 --> 在节点 {this.Node.NodeName} 执行 “inputKeypressValue” 作业, 但作业值不是 string 类型");
            }

            return false;
        }
    }
}
