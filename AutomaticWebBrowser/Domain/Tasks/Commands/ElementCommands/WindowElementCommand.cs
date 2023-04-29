using System.Threading.Tasks;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Domain.Configuration.Models;

using Microsoft.Web.WebView2.Wpf;

using Serilog.Core;

namespace AutomaticWebBrowser.Domain.Tasks.Commands.ElementCommands
{
    class WindowElementCommand : ElementCommand
    {
        public WindowElementCommand (IWebView webView, Logger log, AWElement element)
            : base (webView, log, element)
        { }

        public override bool Execute ()
        {
            // 合成 javascript 代码
            string script = $@"
const {this.Result} = [ this.window ];
".Trim ();
            // 执行 javascript 代码
            this.WebView.SafeExecuteScriptAsync (script).Wait ();
            this.Log.Information ($"自动化任务 --> 执行 Element({this.Element.Type}) 命令成功");
            return true;
        }
    }
}
