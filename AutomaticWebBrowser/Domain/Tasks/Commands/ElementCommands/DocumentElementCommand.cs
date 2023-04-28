using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Domain.Configuration.Models;

using Serilog.Core;

namespace AutomaticWebBrowser.Domain.Tasks.Commands.ElementCommands
{
    /// <summary>
    /// document 对象元素查找命令
    /// </summary>
    [ElementCommand (AWElementType.Document)]
    class DocumentElementCommand : ElementCommand
    {
        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="DocumentElementCommand"/> 类的新实例
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="log"></param>
        /// <param name="element"></param>
        public DocumentElementCommand (IWebView webView, Logger log, AWElement element)
            : base (webView, log, element)
        { }
        #endregion

        #region --公开方法--
        public override bool Execute ()
        {
            // 合成 javascript 代码
            string script = $@"
const {this.Result} = [ this.document ];
".Trim ();
            // 执行 javascript 代码
            this.WebView.SafeExecuteScriptAsync (script).Wait ();
            this.Log.Information ($"自动化任务 --> 执行 Element({this.Element.Type}) 命令成功");
            return true;
        } 
        #endregion
    }
}
