using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Domain.Configuration.Models;

using Serilog.Core;

namespace AutomaticWebBrowser.Domain.Tasks.Commands.ElementCommands
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
        /// <param name="webView"></param>
        /// <param name="log"></param>
        /// <param name="element"></param>
        public WindowElementCommand (IWebView webView, Logger log, AWElement element)
            : base (webView, log, element)
        { }
        #endregion

        #region --公开方法--
        public override bool Execute ()
        {
            // 合成 javascript 代码
            string script = $@"
const {this.Result} = [];
function {this.Result}_ElementCommand_{this.Element.Type}_Func () {{
    {this.Result}.push (this.window);
    return {this.Result}.length;
}}
{this.Result}_ElementCommand_{this.Element.Type}_Func ();
".Trim ();
            // 执行 javascript 代码
            string result = this.WebView.SafeExecuteScriptAsync (script).Result;
            if (int.TryParse (result, out int count) && count > 0)
            {
                this.Result = count;
                this.Log.Information ($"自动化任务 --> 执行 Element({this.Element.Type}) 命令成功");
                return true;
            }
            return false;
        } 
        #endregion
    }
}
