﻿using System;
using System.Text.Json;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Services.Configuration.Models;

using Gecko;
using Gecko.WebIDL;

using Serilog.Core;

namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands.JobCommands
{
    [JobCommand (JobType.KeyDown)]
    class KeyDownJobCommand : JobCommand
    {
        public KeyDownJobCommand (GeckoWebBrowser webView, GeckoNode node, Job job, Logger log)
            : base (webView, node, job, log)
        { }

        public override bool Execute ()
        {
            if (this.Job.Value.ValueKind == JsonValueKind.Object)
            {
                try
                {
                    Keyboard keyboard = this.Job.Value.Deserialize<Keyboard> (Global.JsonSerializerOptions);

                    bool result = (bool)this.WebView.Invoke (() =>
                    {
                        try
                        {
                            // 创建键盘输入事件
                            DomEventArgs eventArgs = this.WebView.DomDocument.CreateEvent (Global.DOM.DOM_KEY_EVENTS);

                            // 按键事件
                            KeyEvent keyEvent = new (this.WebView.Window.DomWindow as mozIDOMWindowProxy, eventArgs.DomEvent as nsISupports);
                            keyEvent.InitKeyEvent (
                                Global.DOM.EVENT_KEY_DOWN,
                                true,
                                false,
                                this.WebView.Window.DomWindow as nsIDOMWindow,
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
                            this.Log.Information ($"JobCommand executed “keyDown” job of node “{this.NodeName}”, key: {(keyboard.Control ? "ctrl+" : string.Empty)}{(keyboard.Alt ? "alt+" : string.Empty)}{(keyboard.Shift ? "shift+" : string.Empty)}{(keyboard.Meta ? "meta+" : string.Empty)}{keyboard.Key}, count: {keyboard.Count}.");
                            return true;
                        }
                        catch (Exception e)
                        {
                            this.Log.Error (e, $"JobCommand executed “keyDown” job of node “{this.NodeName}”, but something went error the execution.");
                            return false;
                        }
                    });

                    return result;
                }
                catch (JsonException e)
                {
                    this.Log.Error (e, $"JobCommand executed “keyDown” job of node “{this.NodeName}”, but value deserialize to “Keyboard” type fail.");
                }
            }
            else
            {
                this.Log.Warning ($"JobCommand executed “keyDown” job of node “{this.NodeName}”, but job value type is not “Keyboard”.");
            }

            return false;
        }
    }
}
