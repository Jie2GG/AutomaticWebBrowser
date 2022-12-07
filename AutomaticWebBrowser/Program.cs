using System;
using System.Windows.Forms;

using AutomaticWebBrowser.Models;

using Ookii.CommandLine;

namespace AutomaticWebBrowser
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main (params string[] args)
        {
            // 解析启动命令
            BootParameters @params = CommandLineParser.Parse<BootParameters> (args);

            // 初始化 WinForms 应用程序
            Application.EnableVisualStyles ();
            Application.SetCompatibleTextRenderingDefault (false);
            Application.Run (new MainForm (@params));
        }
    }
}