using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

namespace AutomaticWebBrowser.Views
{
    /// <summary>
    /// 日志窗体
    /// </summary>
    partial class LogForm : Form, ILogEventSink
    {
        private readonly ConcurrentQueue<LogEvent> queue;
        private readonly AutoResetEvent printEvent;
        private readonly CancellationTokenSource printTaskCancellationTokenSource;
        private readonly Task printTask;

        #region --属性--
        /// <summary>
        /// 文本格式化工具
        /// </summary>
        public ITextFormatter TextFormatter { get; set; }
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="LogForm"/> 类的新实例
        /// </summary>
        public LogForm ()
        {
            this.InitializeComponent ();

            this.queue = new ConcurrentQueue<LogEvent> ();
            this.printEvent = new AutoResetEvent (false);
            this.printTaskCancellationTokenSource = new CancellationTokenSource ();
            this.printTask = new Task (this.PrintLogTask, this.printTaskCancellationTokenSource.Token, TaskCreationOptions.LongRunning);
        }

        ~LogForm ()
        {
            this.printTaskCancellationTokenSource.Cancel ();
        }
        #endregion

        #region --公开方法--
        /// <summary>
        /// 日志写入
        /// </summary>
        /// <param name="logEvent"></param>
        void ILogEventSink.Emit (LogEvent logEvent)
        {
            this.queue.Enqueue (logEvent);
            this.printEvent.Set ();
        }
        #endregion

        #region --私有方法--
        private void PrintLogTask ()
        {
            do
            {
                while (!this.queue.IsEmpty)
                {
                    if (this.queue.TryDequeue (out LogEvent result))
                    {
                        using StringWriter stringWriter = new ();
                        this.TextFormatter.Format (result, stringWriter);
                        string text = stringWriter.ToString ().Trim ();

                        switch (result.Level)
                        {
                            case LogEventLevel.Verbose: this.AppendText (text, Color.Black, Color.White); break;
                            case LogEventLevel.Debug: this.AppendText (text, Color.Green, Color.White); break;
                            case LogEventLevel.Information: this.AppendText (text, Color.Blue, Color.White); break;
                            case LogEventLevel.Warning: this.AppendText (text, Color.OrangeRed, Color.White); break;
                            case LogEventLevel.Error: this.AppendText (text, Color.Red, Color.White); break;
                            case LogEventLevel.Fatal: this.AppendText (text, Color.Red, Color.Black); break;
                        }
                    }
                }

                this.printEvent.Reset ();
            } while (!this.printTaskCancellationTokenSource.Token.IsCancellationRequested);
        }

        private void AppendText (string text, Color foregroundColor, Color backgroundColor)
        {
            this.BeginInvoke (() =>
            {
                this.logRichTextBox.SelectionStart = this.logRichTextBox.TextLength;
                this.logRichTextBox.SelectionLength = 0;
                this.logRichTextBox.SelectionBackColor = backgroundColor;
                this.logRichTextBox.SelectionColor = foregroundColor;
                this.logRichTextBox.AppendText ($"{text}{Environment.NewLine}");
                this.logRichTextBox.SelectionColor = this.logRichTextBox.ForeColor;
                this.logRichTextBox.SelectionBackColor = this.logRichTextBox.BackColor;
                this.logRichTextBox.ScrollToCaret ();
            });
        }
        #endregion

        #region --事件处理--
        protected override void OnShown (EventArgs e)
        {
            // 处理事件
            Application.DoEvents ();

            this.printTask.Start ();
        }

        protected override void OnClosing (CancelEventArgs e)
        {
            this.Visible = false;
            e.Cancel = true;
        }
        #endregion
    }
}
