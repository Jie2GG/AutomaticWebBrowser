using System;
using System.Linq;
using System.Reflection;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

using Serilog;

namespace AutomaticWebBrowser.Commands
{
    /// <summary>
    /// 操作命令抽象类
    /// </summary>
    public abstract class OptionCommand : ICommand
    {
        #region --字段--
        private static readonly Type[] optionTypes = Assembly.GetExecutingAssembly ()
            .GetTypes ()
            .Where (p => p.IsSubclassOf (typeof (OptionCommand)) && !p.IsAbstract && !p.IsInterface)
            .ToArray ();
        #endregion

        #region --属性--
        public TaskWebBrowser Browser { get; }
        public GeckoNode Node { get; }
        public Option Option { get; }
        public ILogger Log => this.Browser.Log;
        #endregion

        #region --构造函数--
        protected OptionCommand (TaskWebBrowser webBrowser, GeckoNode node, Option option)
        {
            this.Browser = webBrowser;
            this.Node = node;
            this.Option = option;
        }
        #endregion

        #region --公开方法--
        public abstract void Execute ();

        public static OptionCommand CreateCommand (TaskWebBrowser browser, GeckoNode node, Option option)
        {
            foreach (Type optionType in optionTypes)
            {
                OptionCommandAttribute attribute = optionType.GetCustomAttribute<OptionCommandAttribute> ();
                if (attribute != null && attribute.OptionType == option.Type)
                {
                    return (OptionCommand)Activator.CreateInstance (optionType, new object[] { browser, node, option });
                }
            }
            return new DefaultOptionCommand (browser, node, option);
        }
        #endregion
    }
}
