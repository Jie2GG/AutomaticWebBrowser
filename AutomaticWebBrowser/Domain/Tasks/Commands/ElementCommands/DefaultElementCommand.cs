using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Domain.Configuration.Models;

using Serilog.Core;

namespace AutomaticWebBrowser.Domain.Tasks.Commands.ElementCommands
{
    /// <summary>
    /// 默认元素查找命令
    /// </summary>
    class DefaultElementCommand : ElementCommand
    {
        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="DefaultElementCommand"/> 类的新实例
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="log"></param>
        /// <param name="element"></param>
        public DefaultElementCommand (IWebView webView, Logger log, AWElement element)
            : base (webView, log, element)
        { }
        #endregion

        #region --公开方法--
        public override bool Execute ()
        {
            // 合成 javascript 代码
            string script = $@"
const {this.VariableName} = [];
(function () {{
    return {this.VariableName}.length;
}}) ();
".Trim ();

            // 执行 javascript 代码
            string result = this.WebView.SafeExecuteScriptAsync (script).Result;
            if (int.TryParse (result, out int count) && count > 0)
            {
                this.Result = count;
                this.Log.Warning ($"自动化任务 --> 执行 Element(Default) 命令, 原因: 未找到指定类型的 {nameof (ElementCommand)} 或使用了未知的 {nameof (AWElementType)}");
                return true;
            }
            return false;
        }
        #endregion
    }
}
