using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using AutomaticWebBrowser.Core;
using AutomaticWebBrowser.Services.Configuration.Models;

using Serilog;

namespace AutomaticWebBrowser.Services.Automatic.Commands.ActionCommands
{
    /// <summary>
    /// 运行子作业动作命令
    /// </summary>
    [ActionCommand (AWActionType.RunSubJob)]
    class RunSubJobActionCommand : ActionCommand
    {
        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="RunSubJobActionCommand"/> 类的新实例
        /// </summary>
        public RunSubJobActionCommand (IWebView webView, ILogger logger, AWAction action, string? variableName, int? index)
            : base (webView, logger, action, variableName, index)
        { }
        #endregion

        #region --公开方法--
        public override bool Execute ()
        {
            if (this.VariableName is not null && this.Action.Value is JsonElement json)
            {
                try
                {
                    // 获取子任务参数
                    AWJob[] subJob = json.Deserialize<AWJob[]> (Global.DefaultJsonSerializerOptions)!;

                    // 等待子页面就绪
                    this.WebView.WebView2CreateNewAsyncWait.WaitOne ();

                    // 执行子作业
                    if (this.WebView.SubTabs.Count > 0)
                    {
                        for (int i = 0; i < subJob.Length; i++)
                        {
                            if (subJob[i].Name is null)
                            {
                                subJob[i].Name = $"Job{i + 1}";
                            }
                        }

                        IWebView subWebView = this.WebView.SubTabs[^1];

                        AWTask subTask = new () { Name = $"{this.WebView.TaskInfo.Name}-SubTask", Jobs = subJob };
                        this.Logger.Information ($"自动化任务({this.WebView.TaskInfo.Name}) --> 执行 Action({this.Action.Type}) 命令, 子任务: {subTask.Name} 准备就绪");

                        // 投递子任务
                        subWebView.PutTask (subTask);
                        this.Logger.Information ($"自动化任务({this.WebView.TaskInfo.Name}) --> 执行 Action({this.Action.Type}) 命令, 子任务: {subTask.Name} 执行开始");

                        // 启动并等待执行结果
                        return subWebView.Start (this.WebView.TaskTokenSource)
                            .ContinueWith (task =>
                            {
                                if (task.IsCanceled)
                                {
                                    this.Logger.Warning ($"自动化任务({this.WebView.TaskInfo.Name}) --> 执行 Action({this.Action.Type}) 命令, 子任务: {subTask.Name} 执行被取消");
                                    return false;
                                }

                                if (task.IsCompletedSuccessfully)
                                {
                                    this.Logger.Information ($"自动化任务({this.WebView.TaskInfo.Name}) --> 执行 Action({this.Action.Type}) 命令, 子任务: {subTask.Name} 执行成功");
                                    return true;
                                }
                                return false;
                            }).Result;
                    }
                    else
                    {
                        this.Logger.Warning ($"自动化任务({this.WebView.TaskInfo.Name}) --> 执行 Action({this.Action.Type}) 命令失败, 原因: 没有可用的子标签页进行任务传递");
                    }

                }
                catch (JsonException e)
                {
                    this.Logger.Error (e, $"自动化任务({this.WebView.TaskInfo.Name}) --> 执行 Action({this.Action.Type}) 命令失败, 原因: 值反序列化为 {nameof (AWJob)} 类型失败");
                }

            }
            else
            {
                this.Logger.Warning ($"自动化任务({this.WebView.TaskInfo.Name}) --> 执行 Action({this.Action.Type}) 命令失败, 原因: 值为 null");
            }

            return false;
        }
        #endregion
    }
}
