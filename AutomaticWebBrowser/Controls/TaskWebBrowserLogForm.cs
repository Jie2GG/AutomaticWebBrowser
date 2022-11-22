using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace AutomaticWebBrowser.Controls
{
    /// <summary>
    /// 表示任务Web浏览器日志窗口的类
    /// </summary>
    public class TaskWebBrowserLogForm : Form
    {
        #region --属性--
        public TraceListener LogListener { get; }
        public TextBox TextBox { get; private set; }
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="TaskWebBrowserLogForm"/> 类的新实例
        /// </summary>
        /// <param name="config">配置文件</param>
        public TaskWebBrowserLogForm ()
        {
            this.LogListener = new LogTraceListener (this);

            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.Padding = Padding.Empty;
            this.Size = new Size (800, 300);
            this.Text = "日志";
            this.TopMost = true;
            this.MaximizeBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = Point.Empty;
        }
        #endregion

        #region --公开方法--
        // 窗口加载方法
        protected override void OnLoad (EventArgs e)
        {
            // 创建日志框
            this.TextBox = new TextBox ()
            {
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
                Padding = Padding.Empty,
                Multiline = true,   // 多行
                ReadOnly = true     // 只读
            };

            // 添加到控件
            this.Controls.Add (TextBox);

            // 监听日志
            Trace.Listeners.Add (this.LogListener);
        }

        // 窗口关闭方法
        protected override void OnClosing (CancelEventArgs e)
        {
            e.Cancel = true;
        }
        #endregion

        #region --内部类--
        private class LogTraceListener : TraceListener
        {
            #region --属性--
            public TaskWebBrowserLogForm LogForm { get; }
            #endregion

            #region --构造函数--
            public LogTraceListener (TaskWebBrowserLogForm logForm)
            {
                this.LogForm = logForm;
            }
            #endregion

            public override void Write (string message)
            {
                this.LogForm.TextBox.AppendText (message);
            }

            public override void WriteLine (string message)
            {
                this.LogForm.TextBox.AppendText ($"{message}{Environment.NewLine}");
            }
        }
        #endregion
    }
}
