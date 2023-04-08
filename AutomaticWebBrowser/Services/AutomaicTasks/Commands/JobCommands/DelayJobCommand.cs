using System;
using System.Text.Json;
using System.Threading;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Services.Configuration.Models;
using AutomaticWebBrowser.Views;

using Gecko;

using Serilog.Core;

namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands.JobCommands
{
    [JobCommand (JobType.Delay)]
    class DelayJobCommand : JobCommand
    {
        public DelayJobCommand (BrowserForm form, GeckoNode node, Job job, Logger log)
            : base (form, node, job, log)
        { }

        public override bool Execute ()
        {
            if (this.Job.Value.ValueKind == JsonValueKind.String)
            {
                string timeFormat = this.Job.Value.Deserialize<string> (Global.JsonSerializerOptions);
                if (TimeSpan.TryParse (timeFormat, out TimeSpan duration))
                {
                    Thread.Sleep (duration);
                    this.Log.Information ($"自动化任务 --> 在节点 {this.Node.NodeName} 执行 “delay” 作业, 持续时间: {duration}");
                    return true;
                }
                else
                {
                    this.Log.Warning ($"自动化任务 --> 在节点 {this.Node.NodeName} 执行 “delay” 作业, 但作业值格式错误 (格式: HH:mm:ss)");
                }
            }
            else
            {
                this.Log.Warning ($"自动化任务 --> 在节点 {this.Node.NodeName} 执行 “delay” 作业, 但作业值类型不是 string 类型");
            }

            return false;
        }
    }
}
