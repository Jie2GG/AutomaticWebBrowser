using System.Text.Json;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

using Element = AutomaticWebBrowser.Models.Element;

namespace AutomaticWebBrowser.Commands.OperationCommands
{
    [OperationCommand (OperationType.GetDomElement)]
    public class GetDomElementOperatoinCommand : OperationCommand
    {
        public GetDomElementOperatoinCommand (TaskWebBrowser webBrowser, GeckoNode node, Operation operation)
            : base (webBrowser, node, operation)
        { }

        public override void Execute ()
        {
            // 获取要搜索的节点
            Element element = this.Operation.Value.Deserialize<Element> (GlobalConfig.JsonSerializerOptions);

            //  搜索节点
            SearchCommand command = SearchCommand.CreateCommand (this.Browser, this.Node, element);
            command.Execute ();

            // 执行子操作
            foreach (GeckoNode node in command.SearchResult)
            {
                foreach (Operation operation in this.Operation.SubOperations)
                {
                    CreateCommand (this.Browser, node, operation)
                        .Execute ();
                }
            }
        }
    }
}
