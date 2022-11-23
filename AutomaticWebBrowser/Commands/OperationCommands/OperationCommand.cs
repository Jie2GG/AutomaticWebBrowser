using System;
using System.Linq;
using System.Reflection;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

using Serilog;

namespace AutomaticWebBrowser.Commands.OperationCommands
{
    /// <summary>
    /// 操作命令抽象类
    /// </summary>
    public abstract class OperationCommand : ICommand
    {
        #region --字段--
        private static readonly Type[] operationTypes = Assembly.GetExecutingAssembly ()
            .GetTypes ()
            .Where (p => p.IsSubclassOf (typeof (OperationCommand)) && !p.IsAbstract && !p.IsInterface)
            .ToArray ();
        #endregion

        #region --属性--
        public TaskWebBrowser Browser { get; }
        public GeckoNode Node { get; }
        public Operation Operation { get; }
        public ILogger Log => this.Browser.Log;
        #endregion

        #region --构造函数--
        protected OperationCommand (TaskWebBrowser webBrowser, GeckoNode node, Operation option)
        {
            this.Browser = webBrowser;
            this.Node = node;
            this.Operation = option;
        }
        #endregion

        #region --公开方法--
        public abstract void Execute ();

        public static OperationCommand CreateCommand (TaskWebBrowser browser, GeckoNode node, Operation option)
        {
            foreach (Type optionType in operationTypes)
            {
                OperationCommandAttribute attribute = optionType.GetCustomAttribute<OperationCommandAttribute> ();
                if (attribute != null && attribute.OptionType == option.Type)
                {
                    return (OperationCommand)Activator.CreateInstance (optionType, new object[] { browser, node, option });
                }
            }
            return new DefaultOperationCommand (browser, node, option);
        }
        #endregion
    }
}
