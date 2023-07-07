using System;
using System.Text.Json;
using System.Threading;

using AutomaticWebBrowser.Wpf.Core;
using AutomaticWebBrowser.Wpf.Services.Configuration.Models;

using Serilog;

namespace AutomaticWebBrowser.Wpf.Services.Automatic.Commands.ActionCommands
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
            if (this.VariableName is not null && this.Action.Value != null)
            {
                try
                {
                    // 获取子任务参数
                    AWJob[] subJob = this.Action.Value?.Deserialize<AWJob[]> (Global.DefaultJsonSerializerOptions)!;

                    // 等待子页面就绪
                    this.WebView.WebView2CreateNewAsyncWait.WaitOne ();

                    // 执行子作业
                    if (this.WebView.SubTabs.Count > 0)
                    {
                        IWebView subWebView = this.WebView.SubTabs[^1];

                        AWTask subTask = new () { Name = $"{this.WebView.TaskInfo.Name}-SubTask", Jobs = subJob };
                        this.Logger.Information ($"自动化任务({this.WebView.TaskInfo.Name}) --> 执行 Action({this.Action.Type}) 命令, 子任务: {subTask.Name} 准备就绪");

                        // 投递子任务
                        subWebView.PutTask (subTask);
                        this.Logger.Information ($"自动化任务({this.WebView.TaskInfo.Name}) --> 执行 Action({this.Action.Type}) 命令, 子任务: {subTask.Name} 执行开始");

                        // 运行并等待子任务
                        try
                        {
                            subWebView.Run ().Wait ();
                        }
                        catch (AggregateException)
                        {
                            this.Logger.Warning ($"自动化任务({this.WebView.TaskInfo.Name}) --> 执行 Action({this.Action.Type}) 命令, 子任务: {subTask.Name} 执行被中断");
                            return false;
                        }

                        this.Logger.Information ($"自动化任务({this.WebView.TaskInfo.Name}) --> 执行 Action({this.Action.Type}) 命令, 子任务: {subTask.Name} 执行结束");
                        return true;
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
