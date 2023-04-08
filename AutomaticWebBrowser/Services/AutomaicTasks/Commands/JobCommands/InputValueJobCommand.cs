using System;
using System.Text.Json;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Services.Configuration.Models;
using AutomaticWebBrowser.Views;

using Gecko;
using Gecko.DOM;

using Serilog.Core;

namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands.JobCommands
{
    [JobCommand (JobType.InputValue)]
    class InputValueJobCommand : JobCommand
    {
        public InputValueJobCommand (BrowserForm form, GeckoNode node, Job job, Logger log)
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
                        inputElement.Value = inputValue;
                        this.Log.Information ($"自动化任务 --> 在节点 {this.Node.NodeName} 执行 “inputValue” 作业, 节点输入值是: {inputValue}");
                        return true;
                    }));

                    return this.Browser.EndInvoke (asyncResult) as bool? ?? false;
                }
                else
                {
                    this.Log.Warning ($"自动化任务 --> 在节点 {this.Node.NodeName} 执行 “inputValue” 作业, 但节点不是 InputElement 类型");
                }

            }
            else
            {
                this.Log.Warning ($"自动化任务 --> 在节点 {this.Node.NodeName} 执行 “inputValue” 作业, 但作业值不是 string 类型");
            }

            return false;
        }
    }
}
