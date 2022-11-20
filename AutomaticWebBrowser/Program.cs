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
            // 初始化WinForms应用程序
            Application.EnableVisualStyles ();
            Application.SetCompatibleTextRenderingDefault (false);

            try
            {
                // 初始化配置文件
                StartArguments argsObj = CommandLineParser.Parse<StartArguments> (args);
                Configuration config = InitializeConfig (argsObj);

                // 运行浏览器
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
                throw new FileNotFoundException ("配置文件不存在!", fullPath);
            }

            Configuration config = JsonSerializer.Deserialize<Configuration> (File.OpenRead (fullPath), GlobalConfig.JsonSerializerOptions);

            if (config.TaskInfo is null)
            {
                throw new JsonException ("无法找到配置文件中的 task 节点, 配置文件必须包含该节点才可以自动执行任务");
            }

            return config;
        }
    }
}
