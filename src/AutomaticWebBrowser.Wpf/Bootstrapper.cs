using System;
using System.Collections.Generic;
using System.Reflection;

using Gemini;

namespace AutomaticWebBrowser
{
    class Bootstrapper : AppBootstrapper
    {
        public override bool IsPublishSingleFileHandled => true;


        protected override IEnumerable<Assembly> PublishSingleFileBypassAssemblies
        {
            get
            {
                yield return Assembly.GetAssembly (typeof (AppBootstrapper)) ?? throw new TypeLoadException (); // GeminiWpf
                // add more assemblies with exports as needed here
            }
        }
    }
}
