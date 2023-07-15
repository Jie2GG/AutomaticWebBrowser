using System;
using System.Windows.Input;

using Gemini.Framework.Commands;
using Gemini.Framework.ToolBars;

namespace AutomaticWebBrowser.Modules.WebView.Toolbars
{
    class ToolBarItemRunDefinition : ToolBarItemDefinition
    {


        public override string Text => throw new NotImplementedException ();

        public override Uri IconSource => throw new NotImplementedException ();

        public override KeyGesture KeyGesture => throw new NotImplementedException ();

        public override CommandDefinitionBase CommandDefinition => throw new NotImplementedException ();

        public ToolBarItemRunDefinition (ToolBarItemGroupDefinition group, int sortOrder)
            : base (group, sortOrder, ToolBarItemDisplay.IconAndText)
        { }
    }
}
