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
    /// 日志窗口
    /// </summary>
    partial class LogForm : Form, ILogEventSink
    {
        #region --字段--
        private readonly ConcurrentQueue<LogEvent> queue;
        private readonly AutoResetEvent printEvent;
        private readonly CancellationTokenSource printTaskCancellationTokenSource;
        private readonly Task printTask;
        #endregion

        #region --属性--
        internal ITextFormatter TextFormatter { get; set; }
        #endregion

        #region --构造函数--
        public LogForm ()
        {
            this.InitializeComponent ();

            this.queue = new ConcurrentQueue<LogEvent> ();
            this.printEvent = new AutoResetEvent (false);
            this.printTaskCancellationTokenSource = new CancellationTokenSource ();
            this.printTask = new Task (this.PrintLogTask, this.printTaskCancellationTokenSource.Token, TaskCreationOptions.LongRunning);

#if DEBUG
            this.TopMost = false;
#endif
        }

        ~LogForm ()
        {
            this.printTaskCancellationTokenSource.Cancel ();
        }
        #endregion

        #region --公开方法--
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
                        using (StringWriter stringWriter = new StringWriter ())
                        {
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
                }

                this.printEvent.Reset ();
            } while (!this.printTaskCancellationTokenSource.Token.IsCancellationRequested);
        }

        private void AppendText (string text, Color foregroundColor, Color backgroundColor)
        {
            if (!this.printTaskCancellationTokenSource.IsCancellationRequested)
            {
                this.BeginInvoke (new Action (() =>
                {
                    this.logRichTextBox.SelectionStart = this.logRichTextBox.TextLength;
                    this.logRichTextBox.SelectionLength = 0;
                    this.logRichTextBox.SelectionBackColor = backgroundColor;
                    this.logRichTextBox.SelectionColor = foregroundColor;
                    this.logRichTextBox.AppendText ($"{text}{Environment.NewLine}");
                    this.logRichTextBox.SelectionColor = this.logRichTextBox.ForeColor;
                    this.logRichTextBox.SelectionBackColor = this.logRichTextBox.BackColor;
                    this.logRichTextBox.ScrollToCaret ();
                }));
            }
        }
        #endregion

        #region --事件处理--
        protected override void OnShown (EventArgs e)
        {
            // 处理事件
            Application.DoEvents ();

            // 启动打印线程
            this.printTask.Start ();
        }

        protected override void OnClosing (CancelEventArgs e)
        {
            if (MessageBox.Show ($"确定要退出 AutomaticTaskBrowser 吗? 这将会停止所有任务!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                e.Cancel = true;
            }
            else
            {
                this.printTaskCancellationTokenSource.Cancel ();
            }

        }
        #endregion
    }
}
