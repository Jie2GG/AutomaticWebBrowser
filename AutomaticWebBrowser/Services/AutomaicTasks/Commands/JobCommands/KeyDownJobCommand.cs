﻿using System;
using System.Text.Json;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Services.Configuration.Models;

using Gecko;
using Gecko.WebIDL;

namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands.JobCommands
{
    [JobCommand (JobType.KeyDown)]
    class KeyDownJobCommand : JobCommand
    {
        public KeyDownJobCommand (WebView webView, GeckoNode node, Job job)
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
                        Keyboard keyboard = this.Job.Value.Deserialize<Keyboard> (Global.JsonSerializerOptions);

                        // 创建键盘输入事件
                        DomEventArgs eventArgs = this.WebView.DomDocument.CreateEvent (WebView.DOM_KEY_EVENTS);

                        // 按键事件
                        KeyEvent keyEvent = new (this.WebView.Window.DomWindow as mozIDOMWindowProxy, eventArgs.DomEvent as nsISupports);
                        keyEvent.InitKeyEvent (
                            WebView.EVENT_KEY_DOWN,
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
                        this.Log.Information ($"JobCommand executed “keyDown” job of node “{this.Node.NodeName}”, step: {WebView.EVENT_KEY_DOWN}, key: {(keyboard.Control ? "ctrl+" : string.Empty)}{(keyboard.Alt ? "alt+" : string.Empty)}{(keyboard.Shift ? "shift+" : string.Empty)}{(keyboard.Meta ? "meta+" : string.Empty)}{keyboard.Key}, count: {keyboard.Count}.");
                        return true;

                    }
                    catch (JsonException e)
                    {
                        this.Log.Error (e, $"JobCommand executed “keyDown” job of node “{this.Node.NodeName}”, but value deserialize to “Keyboard” type fail.");
                    }
                    catch (Exception e)
                    {
                        this.Log.Error (e, $"JobCommand executed “keyDown” job of node “{this.Node.NodeName}”, but something went error the execution.");
                    }
                }
                else
                {
                    this.Log.Warning ($"JobCommand executed “keyDown” job of node “{this.Node.NodeName}”, but job value type is not “Keyboard”.");
                }

                return false;
            }));
            return this.WebView.EndInvoke (asyncResult) as bool? ?? false;
        }
    }
}