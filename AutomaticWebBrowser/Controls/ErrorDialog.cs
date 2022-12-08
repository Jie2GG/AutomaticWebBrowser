﻿using System;
using System.Windows.Forms;

using Ookii.Dialogs.WinForms;

namespace AutomaticWebBrowser.Controls
{
    public static class ErrorDialog
    {
        public static void ShowException (Form form, Exception exception)
        {
            if (TaskDialog.OSSupportsTaskDialogs)
            {
                using TaskDialog dialog = new ();
                dialog.Width = 240;
                dialog.WindowTitle = "Error";
                dialog.MainIcon = (TaskDialogIcon)0xFFF9;
                dialog.MainInstruction = "很抱歉, 程序运行过程中出现了一个错误";
                dialog.Content = exception.Message;
                dialog.ButtonStyle = TaskDialogButtonStyle.CommandLinks;
                dialog.ExpandedInformation = exception.ToString ();
                dialog.ExpandedControlText = "详细信息";
                dialog.Footer = $"时间: {DateTime.Now}";
                dialog.FooterIcon = TaskDialogIcon.Information;
                dialog.EnableHyperlinks = true;
                dialog.Buttons.Add (new TaskDialogButton (ButtonType.Custom)
                {
                    Text = "确定",
                });
                dialog.ShowDialog (form);
            }
            else
            {
                MessageBox.Show (exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}