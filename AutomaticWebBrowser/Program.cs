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
            // ������������
            BootParameters @params = CommandLineParser.Parse<BootParameters> (args);

            // ��ʼ�� WinForms Ӧ�ó���
            Application.EnableVisualStyles ();
            Application.SetCompatibleTextRenderingDefault (false);
            Application.Run (new MainForm (@params));
        }
    }
}