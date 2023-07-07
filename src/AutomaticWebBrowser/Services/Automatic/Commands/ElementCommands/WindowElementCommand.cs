using AutomaticWebBrowser.Wpf.Core;
using AutomaticWebBrowser.Wpf.Services.Configuration.Models;

using Serilog;

namespace AutomaticWebBrowser.Wpf.Services.Automatic.Commands.ElementCommands
{
    /// <summary>
    /// window 对象元素查找命令
    /// </summary>
    [ElementCommand (AWElementType.Window)]
    class WindowElementCommand : ElementCommand
    {
        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="WindowElementCommand"/> 类的新实例
        /// </summary>
        public WindowElementCommand (IWebView webView, ILogger logger, AWElement element)
            : base (webView, logger, element)
        { }
        #endregion

        #region --公开方法--
        public override bool Execute ()
        {
            // 合成 javascript 代码
            string script = $@"
const {this.ResultVariableName} = [];
(function () {{
    {this.ResultVariableName}.push (this.window);
    return {this.ResultVariableName}.length;
}}) ();
".Trim ();
            // 执行 javascript 代码
            string result = this.WebView.ExecuteScriptAsync (script).Result;
            if (int.TryParse (result, out int count) && count > 0)
            {
                this.Result = count;
                this.Logger.Information ($"自动化任务 --> 执行 Element({this.Element.Type}) 命令成功");
                return true;
            }
            else
            {
                this.Logger.Warning ($"自动化任务 --> 执行 Element({this.Element.Type}) 命令失败, 原因: 不存在 window");
            }
            return false;
        }
        #endregion
    }
}
