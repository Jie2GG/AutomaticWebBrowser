using System.ComponentModel.Composition;

using Gemini.Framework;
using Gemini.Framework.ToolBars;

namespace AutomaticWebBrowser.Modules.WebView
{
    [Export (typeof (IModule))]
    class Module : ModuleBase
    {
        [Export]
        public static readonly ToolBarDefinition TaskToolBar =
            new (1, "Task");

        [Export]
        public static readonly ToolBarItemGroupDefinition TaskToolBarItemGroup =
            new (TaskToolBar, 1);

        //[Export]
        //public static readonly ToolBarItemDefinition RunToolBarItem = 
        //    new ;
    }
}
