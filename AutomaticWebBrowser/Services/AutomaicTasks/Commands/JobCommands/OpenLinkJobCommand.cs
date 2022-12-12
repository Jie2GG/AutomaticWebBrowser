using System;
using System.Text.Json;

using AutomaticWebBrowser.Commands.DomSearchCommands;
using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Services.Configuration.Models;

using Gecko;
using Gecko.DOM;

using Serilog.Core;

namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands.JobCommands
{
    [JobCommand (JobType.OpenLink)]
    class OpenLinkJobCommand : JobCommand
    {
        public OpenLinkJobCommand (GeckoWebBrowser webView, GeckoNode node, Job job, Logger log)
            : base (webView, node, job, log)
        { }

        public override bool Execute ()
        {
            if (this.Job.Value.ValueKind == JsonValueKind.Array)
            {
                try
                {
                    Configuration.Models.Action[] actions = this.Job.Value.Deserialize<Configuration.Models.Action[]> (Global.JsonSerializerOptions);

                    if (this.Node is GeckoAnchorElement anchorElement)
                    {
                        // �����±�ǩ
                        WebViewTabPage page = new (this.Log);

                        // ��ʾ��ǩҳ
                        this.WebView.Invoke (() =>
                        {
                            this.TabConrol.TabPages.Add (page);
                            this.TabConrol.SelectTab (page);

                            page.WebView.Navigate (anchorElement.Href);
                            this.Log.Information ($"JobCommand executed ��openLink�� job of node ��{this.NodeName}��, new tab navigate to {anchorElement.Href}");
                        });

                        // ����������
                        page.CreateTask (actions);
                        page.RunTask ();

                        // �Ƴ���ǩҳ
                        this.WebView.Invoke (() =>
                        {
                            this.TabConrol.SelectTab (0);
                            this.TabConrol.TabPages.Remove (page);
                            this.Log.Information ($"JobCommand executed ��openLink�� job of node ��{this.NodeName}��, new tab navigate to {anchorElement.Href}");

                            // �ͷ������, ��ֹ��ס����
                            page.Dispose ();
                        });
                    }
                    else
                    {
                        this.Log.Warning ($"JobCommand executed ��openLink�� job of node ��{this.NodeName}��, but node is not ��AnchorElement��.");
                    }

                    return true;
                }
                catch (JsonException e)
                {
                    this.Log.Error (e, $"JobCommand executed ��openLink�� job of node ��{this.NodeName}��, but value deserialize to ��Action[]�� type fail.");
                }
            }
            else
            {
                this.Log.Warning ($"JobCommand executed ��openLink�� job of node ��{this.NodeName}��, but job value type is not ��Array��.");
            }

            return false;
        }
    }
}