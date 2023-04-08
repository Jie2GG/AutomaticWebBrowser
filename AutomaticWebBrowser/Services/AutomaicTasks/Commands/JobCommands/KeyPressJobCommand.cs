using System;
using System.Text.Json;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Services.Configuration.Models;
using AutomaticWebBrowser.Views;

using Gecko;
using Gecko.WebIDL;

using Serilog.Core;

namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands.JobCommands
{
    [JobCommand (JobType.KeyPress)]
    class KeyPressJobCommand : JobCommand
    {
        public KeyPressJobCommand (BrowserForm form, GeckoNode node, Job job, Logger log)
            : base (form, node, job, log)
        { }

        public override bool Execute ()
        {
            if (this.Job.Value.ValueKind == JsonValueKind.Object)
            {
                try
                {
                    Keyboard keyboard = this.Job.Value.Deserialize<Keyboard> (Global.JsonSerializerOptions);

                    IAsyncResult asyncResult = this.Browser.BeginInvoke (new Func<bool> (() =>
                    {
                        try
                        {
                            // 创建键盘输入事件
                            DomEventArgs eventArgs = this.Browser.DomDocument.CreateEvent (Global.DOM.DOM_KEY_EVENTS);

                            // 按键事件
                            KeyEvent keyEvent = new KeyEvent (this.Browser.Window.DomWindow as mozIDOMWindowProxy, eventArgs.DomEvent as nsISupports);
                            keyEvent.InitKeyEvent (
                                Global.DOM.EVENT_KEY_PRESS,
                                true,
                                false,
                                this.Browser.Window.DomWindow as nsIDOMWindow,
                                keyboard.Control,
                                keyboard.Alt,
                                keyboard.Shift,
                                keyboard.Meta,
                                (uint)keyboard.Key
                            );
                            int count = keyboard.Count;
                            while (count-- > 0)
                            {
                                this.Node.GetEventTarget ().DispatchEvent (eventArgs);
                            }
                            this.Log.Information ($"自动化任务 --> 在节点 {this.Node.NodeName} 执行 “keyPress” 作业, 键: {(keyboard.Control ? "ctrl+" : string.Empty)}{(keyboard.Alt ? "alt+" : string.Empty)}{(keyboard.Shift ? "shift+" : string.Empty)}{(keyboard.Meta ? "meta+" : string.Empty)}{keyboard.Key}, 次数: {keyboard.Count}");
                            return true;
                        }
                        catch (Exception e)
                        {
                            this.Log.Error (e, $"自动化任务 --> 在节点 {this.Node.NodeName} 执行 “keyPress” 作业, 但在执行过程中出现了异常");
                            return false;
                        }
                    }));

                    return this.Browser.EndInvoke (asyncResult) as bool? ?? false;
                }
                catch (JsonException e)
                {
                    this.Log.Error (e, $"自动化任务 --> 在节点 {this.Node.NodeName} 执行 “keyPress” 作业, 但作业值反序列化为 Keyboard 类型时失败");
                }
            }
            else
            {
                this.Log.Warning ($"自动化任务 --> 在节点 {this.Node.NodeName} 执行 “keyPress” 作业, 但作业值不是 Keyboard 类型");
            }

            return false;
        }
    }
}
