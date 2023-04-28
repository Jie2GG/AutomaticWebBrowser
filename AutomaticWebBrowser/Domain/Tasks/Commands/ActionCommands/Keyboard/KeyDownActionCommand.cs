using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Domain.Configuration.Models;

using Serilog.Core;

namespace AutomaticWebBrowser.Domain.Tasks.Commands.ActionCommands.Keyboard
{
    [ActionCommand (AWActionType.KeyDown)]
    class KeyDownActionCommand : KeyboardActionCommand
    {
        protected override string TypeArg => "keydown";

        public KeyDownActionCommand (IWebView webView, Logger log, AWAction action, string? variableName)
            : base (webView, log, action, variableName)
        { }
    }
}
