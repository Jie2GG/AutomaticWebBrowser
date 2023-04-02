﻿using System;
using System.Text.Json;
using System.Windows.Forms;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Services.Configuration.Models;

using Gecko;
using Gecko.DOM;
using Gecko.WebIDL;

using Serilog.Core;

namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands.JobCommands
{
    [JobCommand (JobType.InputKeypressValue)]
    class InputKeypressValueJobCommand : JobCommand
    {
        public InputKeypressValueJobCommand (GeckoWebBrowser webView, GeckoNode node, Job job, Logger log)
            : base (webView, node, job, log)
        { }

        public override bool Execute ()
        {
            if (this.Job.Value.ValueKind == JsonValueKind.String)
            {
                try
                {
                    string inputValue = this.Job.Value.Deserialize<string> (Global.JsonSerializerOptions);

                    if (this.Node is GeckoInputElement inputElement)
                    {
                        bool result = (bool)this.WebView.Invoke (() =>
                        {
                            try
                            {
                                // 创建键盘输入事件
                                DomEventArgs eventArgs = this.WebView.DomDocument.CreateEvent (Global.DOM.DOM_KEY_EVENTS);

                                // DOM事件
                                Event domEvent = new (this.WebView.Window.DomWindow as mozIDOMWindowProxy, eventArgs.DomEvent as nsISupports);
                                // 获取焦点
                                domEvent.InitEvent (Global.DOM.EVENT_FOCUS, true, false);
                                inputElement.GetEventTarget ().DispatchEvent (eventArgs);
                                this.Log.Information ($"JobCommand executed “inputKeypressValue” job of node “{this.Node.NodeName}”, node is focus.");

                                // 按键事件
                                KeyEvent keyEvent = new (this.WebView.Window.DomWindow as mozIDOMWindowProxy, eventArgs.DomEvent as nsISupports);
                                foreach (char c in inputValue.ToCharArray ())
                                {
                                    keyEvent.InitKeyEvent (
                                        Global.DOM.EVENT_KEY_PRESS,
                                        true,
                                        false,
                                        this.WebView.Window.DomWindow as nsIDOMWindow,
                                        false,
                                        false,
                                        false,
                                        false,
                                        c,
                                        c
                                    );
                                    inputElement.GetEventTarget ().DispatchEvent (eventArgs);
                                }
                                this.Log.Information ($"JobCommand executed “inputKeypressValue” job of node “{this.Node.NodeName}”, value is “{inputValue}”.");

                                // 失去焦点
                                domEvent.InitEvent (Global.DOM.EVENT_BLUR, true, false);
                                inputElement.GetEventTarget ().DispatchEvent (eventArgs);
                                this.Log.Information ($"JobCommand executed “inputKeypressValue” job of node “{this.Node.NodeName}”, node is blur.");

                                // 处理事件
                                Application.DoEvents ();
                                return true;
                            }
                            catch (Exception e)
                            {
                                this.Log.Error (e, $"JobCommand executed “inputKeypressValue” job of node “{this.NodeName}”, but something went error the execution.");
                                return false;
                            }
                        });

                        return result;
                    }
                    else
                    {
                        this.Log.Warning ($"JobCommand executed “inputKeypressValue” job of node “{this.NodeName}”, but node is not “InputElement”.");
                    }
                }
                catch (JsonException e)
                {
                    this.Log.Error (e, $"JobCommand executed “inputKeypressValue” job of node “{this.NodeName}”, but value deserialize to “String” type fail.");
                }
            }
            else
            {
                this.Log.Warning ($"JobCommand executed “inputKeypressValue” job of node “{this.NodeName}”, but job value type is not “String”.");
            }

            return false;
        }
    }
}
