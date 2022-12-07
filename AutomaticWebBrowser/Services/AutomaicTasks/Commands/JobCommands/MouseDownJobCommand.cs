using System;
using System.Text.Json;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Services.Configuration.Models;

using Gecko;
using Gecko.WebIDL;

namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands.JobCommands
{
    [JobCommand (JobType.MouseDown)]
    class MouseDownJobCommand : JobCommand
    {
        public MouseDownJobCommand (WebView webView, GeckoNode node, Job job)
            : base (webView, node, job)
        { }

        public override bool Execute ()
        {
            IAsyncResult asyncResult = this.WebView.BeginInvoke (new Func<bool> (() =>
            {
                if (this.Job.Value.ValueKind == JsonValueKind.Object)
                {
                    try
                    {
                        Mouse mouse = this.Job.Value.Deserialize<Mouse> (Global.JsonSerializerOptions);

                        // 创建鼠标输入事件
                        DomEventArgs eventArgs = this.WebView.DomDocument.CreateEvent (WebView.DOM_MOUSE_EVENT);

                        // 按键事件
                        MouseEvent mouseEvent = new (this.WebView.Window.DomWindow as mozIDOMWindowProxy, eventArgs.DomEvent as nsISupports);
                        mouseEvent.InitMouseEvent (
                            WebView.EVENT_MOUSE_DOWN,
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
                        this.Log.Information ($"JobCommand executed “mouseDown” job of node “{this.Node.NodeName}”, step: {WebView.EVENT_MOUSE_DOWN}, key: {(mouse.Control ? "ctrl+" : string.Empty)}{(mouse.Alt ? "alt+" : string.Empty)}{(mouse.Shift ? "shift+" : string.Empty)}{(mouse.Meta ? "meta+" : string.Empty)}, mouse: {mouse.Button}, count: {mouse.Count}.");
                        return true;

                    }
                    catch (JsonException e)
                    {
                        this.Log.Error (e, $"JobCommand executed “mouseDown” job of node “{this.Node.NodeName}”, but value deserialize to “Mouse” type fail.");
                    }
                    catch (Exception e)
                    {
                        this.Log.Error (e, $"JobCommand executed “mouseDown” job of node “{this.Node.NodeName}”, but something went error the execution.");
                    }
                }
                else
                {
                    this.Log.Warning ($"JobCommand executed “mouseDown” job of node “{this.Node.NodeName}”, but job value type is not “Mouse”.");
                }

                return false;
            }));
            return this.WebView.EndInvoke (asyncResult) as bool? ?? false;
        }
    }
}
