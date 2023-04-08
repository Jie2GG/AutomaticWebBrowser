namespace AutomaticWebBrowser.Views
{
    partial class BrowserForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose ();
            }
            base.Dispose (disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            this.automaticWebBrowser = new Gecko.GeckoWebBrowser();
            this.SuspendLayout();
            // 
            // automaticWebBrowser
            // 
            this.automaticWebBrowser.ConsoleMessageEventReceivesConsoleLogCalls = true;
            this.automaticWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.automaticWebBrowser.FrameEventsPropagateToMainWindow = false;
            this.automaticWebBrowser.Location = new System.Drawing.Point(0, 0);
            this.automaticWebBrowser.Margin = new System.Windows.Forms.Padding(0);
            this.automaticWebBrowser.Name = "automaticWebBrowser";
            this.automaticWebBrowser.Size = new System.Drawing.Size(800, 450);
            this.automaticWebBrowser.TabIndex = 0;
            this.automaticWebBrowser.UseHttpActivityObserver = false;
            this.automaticWebBrowser.CreateWindow += new System.EventHandler<Gecko.GeckoCreateWindowEventArgs>(this.AutomaticWebBrowserCreateWindowEventHandler);
            this.automaticWebBrowser.StatusTextChanged += new System.EventHandler(this.AutomaticWebBrowserStatusTextChangedEventHandler);
            this.automaticWebBrowser.ReadyStateChange += new System.EventHandler<Gecko.DomEventArgs>(this.AutomaticWebBrowserReadyStateChangeEventHandler);
            // 
            // BrowserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.automaticWebBrowser);
            this.Name = "BrowserForm";
            this.Text = "新浏览窗";
            this.ResumeLayout(false);

        }

        #endregion

        private Gecko.GeckoWebBrowser automaticWebBrowser;
    }
}