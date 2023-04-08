using System;
using System.Text.Json;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Common;
using AutomaticWebBrowser.Services.Configuration.Models;
using AutomaticWebBrowser.Views;

using Gecko;
using Gecko.DOM;

using Serilog.Core;

namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands.JobCommands
{
    [JobCommand (JobType.ExecuteSubTask)]
    class ExecuteSubTaskJobCommand : JobCommand
    {
        public ExecuteSubTaskJobCommand (BrowserForm form, GeckoNode node, Job job, Logger log)
            : base (form, node, job, log)
        { }

        public override bool Execute ()
        {
            if (this.Job.Value.ValueKind == JsonValueKind.Object)
            {
                try
                {
                    SubTask subTask = this.Job.Value.Deserialize<SubTask> (Global.JsonSerializerOptions);

                    // 
                    this.Browser.Invoke (new System.Action (() =>
                    {
                        if (this.Node is GeckoAnchorElement anchorElement)
                        {
                            // 将A标签的 target 设置为新窗体打开
                            anchorElement.Target = "_blank";

                            // 创建浏览器窗体
                            this.Form.BrowserWaitHandle = new BrowserWaitHandle (new BrowserForm (this.Log));
                            this.Form.BrowserWaitHandle.Reset ();

                            // 点击链接触发新窗体事件
                            anchorElement.Click ();
                        }
                        else
                        {
                            this.Log.Warning ($"自动化任务 --> 在节点 {this.Node.NodeName} 执行 “executeSubTask” 作业, 但节点不是 AnchorElement 类型");
                        }
                    }));
                    //this.Log.Error (e, $"自动化任务 --> 在节点 {this.Node.NodeName} 执行 “executeSubTask” 作业, 但在执行过程中出现了异常");

                    // 等待窗体创建完毕
                    this.Form.BrowserWaitHandle.WaitOne ();

                    // 开始执行子任务
                    this.Form.BrowserWaitHandle.Form.RunActions (subTask.Actions);

                    // 执行完毕后关闭窗体
                    this.Form.BrowserWaitHandle.Form.SafeClose (this.Form);

                }
                catch (JsonException e)
                {
                    this.Log.Error (e, $"自动化任务 --> 在节点 {this.Node.NodeName} 执行 “executeSubTask” 作业, 但作业值反序列化为 SubTask 类型时失败");
                }
            }
            else
            {
                this.Log.Warning ($"自动化任务 --> 在节点 {this.Node.NodeName} 执行 “executeSubTask” 作业, 但作业值不是 SubTask 类型");
            }

            return false;
        }
    }
}
