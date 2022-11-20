using System;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;

using Gecko;

using Ookii.CommandLine;

namespace AutomaticWebBrowser
{
    public static class Program
    {
        [STAThread]
        public static void Main (params string[] args)
        {
            // ��ʼ��WinFormsӦ�ó���
            Application.EnableVisualStyles ();
            Application.SetCompatibleTextRenderingDefault (false);

            try
            {
                // ��ʼ�������ļ�
                StartArguments argsObj = CommandLineParser.Parse<StartArguments> (args);
                Configuration config = InitializeConfig (argsObj);

                // ���������
                Application.Run (new TaskWebBrowserForm (config));
            }
            catch (Exception e)
            {
                TaskMessage.ShowException (null, e);
            }
        }

        private static Configuration InitializeConfig (StartArguments args)
        {
            string fullPath = Path.GetFullPath (args.Config);
            if (!File.Exists (fullPath))
            {
                throw new FileNotFoundException ("�����ļ�������!", fullPath);
            }

            Configuration config = JsonSerializer.Deserialize<Configuration> (File.OpenRead (fullPath), GlobalConfig.JsonSerializerOptions);

            if (config.TaskInfo is null)
            {
                throw new JsonException ("�޷��ҵ������ļ��е� task �ڵ�, �����ļ���������ýڵ�ſ����Զ�ִ������");
            }

            return config;
        }
    }
}
