using System;
using System.Text.Json;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Services.Configuration.Models;

using Gecko;
using Gecko.WebIDL;

using Serilog.Core;

namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands.JobCommands
{
    [JobCommand (JobType.MouseDown)]
    class MouseDownJobCommand : JobCommand
    {
        public MouseDownJobCommand (GeckoWebBrowser webView, GeckoNode node, Job job, Logger log)
            : base (webView, node, job, log)
        { }

        public override bool Execute ()
        {
            if (this.Job.Value.ValueKind == JsonValueKind.Object)
            {
                try
                {
                    Mouse mouse = this.Job.Value.Deserialize<Mouse> (Global.JsonSerializerOptions);

                    bool result = (bool)this.WebView.Invoke (() =>
                    {
                        try
                        {
                            // 创建鼠标输入事件
                            DomEventArgs eventArgs = this.WebView.DomDocument.CreateEvent (Global.DOM.DOM_MOUSE_EVENT);

                            // 按键事件
                            MouseEvent mouseEvent = new (this.WebView.Window.DomWindow as mozIDOMWindowProxy, eventArgs.DomEvent as nsISupports);
                            mouseEvent.InitMouseEvent (
                                Global.DOM.EVENT_MOUSE_DOWN,
                                true,
                                false,
                                this.WebView.Window.DomWindow as nsIDOMWindow,
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
                            this.Log.Information ($"JobCommand executed “mouseDown” job of node “{this.NodeName}”, key: {(mouse.Control ? "ctrl+" : string.Empty)}{(mouse.Alt ? "alt+" : string.Empty)}{(mouse.Shift ? "shift+" : string.Empty)}{(mouse.Meta ? "meta+" : string.Empty)}, mouse: {mouse.Button}, count: {mouse.Count}.");
                            return true;
                        }
                        catch (Exception e)
                        {
                            this.Log.Error (e, $"JobCommand executed “mouseDown” job of node “{this.NodeName}”, but something went error the execution.");
                            return false;
                        }
                    });
                    return result;
                }
                catch (JsonException e)
                {
                    this.Log.Error (e, $"JobCommand executed “mouseDown” job of node “{this.NodeName}”, but value deserialize to “Mouse” type fail.");
                }
            }
            else
            {
                this.Log.Warning ($"JobCommand executed “mouseDown” job of node “{this.NodeName}”, but job value type is not “Mouse”.");
            }

            return false;
        }
    }
}
