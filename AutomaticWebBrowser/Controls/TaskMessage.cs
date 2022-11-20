using System;
using System.Windows.Forms;

using Ookii.Dialogs.WinForms;

namespace AutomaticWebBrowser.Controls
{
    public static class TaskMessage
    {
        public static void ShowException (Form form, Exception exception)
        {
            if (TaskDialog.OSSupportsTaskDialogs)
            {
                using (TaskDialog dialog = new TaskDialog ())
                {
                    dialog.Width = 240;
                    dialog.WindowTitle = "Error";
                    dialog.MainIcon = (TaskDialogIcon)0xFFF9;
                    dialog.MainInstruction = "自动化浏览器在运行过程中发生了一个错误";
                    dialog.Content = $"错误信息: {exception.Message}";
                    dialog.ButtonStyle = TaskDialogButtonStyle.CommandLinks;
                    dialog.ExpandedInformation = exception.StackTrace;
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
            }
            else
            {
                MessageBox.Show (exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
