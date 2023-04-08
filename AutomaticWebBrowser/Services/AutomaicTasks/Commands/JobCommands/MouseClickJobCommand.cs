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
    [JobCommand (JobType.MouseClick)]
    class MouseClickJobCommand : JobCommand
    {
        public MouseClickJobCommand (BrowserForm form, GeckoNode node, Job job, Logger log)
            : base (form, node, job, log)
        { }

        public override bool Execute ()
        {
            if (this.Job.Value.ValueKind == JsonValueKind.Object)
            {
                try
                {
                    Mouse mouse = this.Job.Value.Deserialize<Mouse> (Global.JsonSerializerOptions);

                    IAsyncResult asyncResult = this.Browser.BeginInvoke (new Func<bool> (() =>
                    {
                        try
                        {
                            // 创建鼠标输入事件
                            DomEventArgs eventArgs = this.Browser.DomDocument.CreateEvent (Global.DOM.DOM_MOUSE_EVENT);

                            // 按键事件
                            MouseEvent mouseEvent = new MouseEvent (this.Browser.Window.DomWindow as mozIDOMWindowProxy, eventArgs.DomEvent as nsISupports);
                            mouseEvent.InitMouseEvent (
                                Global.DOM.EVENT_CLICK,
                                true,
                                false,
                                this.Browser.Window.DomWindow as nsIDOMWindow,
                                1,
                                0,
                                0,
                                0,
                                0,
                                mouse.Control,
                                mouse.Alt,
                                mouse.Shift,
                                mouse.Meta,
                                (short)mouse.Button
                            );
                            int count = mouse.Count;
                            while (count-- > 0)
                            {
                                this.Node.GetEventTarget ().DispatchEvent (eventArgs);
                            }
                            this.Log.Information ($"自动化任务 --> 在节点 {this.Node.NodeName} 执行 “mouseClick” 作业, 键: {(mouse.Control ? "ctrl+" : string.Empty)}{(mouse.Alt ? "alt+" : string.Empty)}{(mouse.Shift ? "shift+" : string.Empty)}{(mouse.Meta ? "meta+" : string.Empty)}, mouse: {mouse.Button}, 次数: {mouse.Count}");
                            return true;
                        }
                        catch (Exception e)
                        {
                            this.Log.Error (e, $"自动化任务 --> 在节点 {this.Node.NodeName} 执行 “mouseClick” 作业, 但在执行过程中出现了异常");
                            return false;
                        }
                    }));
                    return this.Browser.EndInvoke (asyncResult) as bool? ?? false;
                }
                catch (JsonException e)
                {
                    this.Log.Error (e, $"自动化任务 --> 在节点 {this.Node.NodeName} 执行 “mouseClick” 作业, 但作业值反序列化为 Mouse 类型时失败");
                }
            }
            else
            {
                this.Log.Warning ($"自动化任务 --> 在节点 {this.Node.NodeName} 执行 “mouseClick” 作业, 但作业值不是 Mouse 类型");
            }

            return false;
        }
    }
}
