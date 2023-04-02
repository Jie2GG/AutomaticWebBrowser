namespace AutomaticWebBrowser
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.mainToolStrip = new System.Windows.Forms.ToolStrip();
            this.logToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.webViewTabControls = new AutomaticWebBrowser.Controls.WebViewTabControl();
            this.mainTableLayoutPanel.SuspendLayout();
            this.mainToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTableLayoutPanel
            // 
            this.mainTableLayoutPanel.ColumnCount = 1;
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.Controls.Add(this.mainToolStrip, 0, 0);
            this.mainTableLayoutPanel.Controls.Add(this.webViewTabControls, 0, 1);
            this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.mainTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            this.mainTableLayoutPanel.RowCount = 2;
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.Size = new System.Drawing.Size(984, 561);
            this.mainTableLayoutPanel.TabIndex = 0;
            // 
            // mainToolStrip
            // 
            this.mainToolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logToolStripButton});
            this.mainToolStrip.Location = new System.Drawing.Point(2, 6);
            this.mainToolStrip.Margin = new System.Windows.Forms.Padding(2, 6, 2, 2);
            this.mainToolStrip.Name = "mainToolStrip";
            this.mainToolStrip.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.mainToolStrip.Size = new System.Drawing.Size(980, 30);
            this.mainToolStrip.TabIndex = 0;
            this.mainToolStrip.Text = "ToolStrip";
            // 
            // logToolStripButton
            // 
            this.logToolStripButton.Image = global::AutomaticWebBrowser.Properties.Resources.icon_logs;
            this.logToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.logToolStripButton.Margin = new System.Windows.Forms.Padding(0);
            this.logToolStripButton.Name = "logToolStripButton";
            this.logToolStripButton.Size = new System.Drawing.Size(60, 28);
            this.logToolStripButton.Text = "日志";
            this.logToolStripButton.Click += new System.EventHandler(this.LogToolStripButtonClickEventHandler);
            // 
            // webViewTabControls
            // 
            this.webViewTabControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webViewTabControls.Location = new System.Drawing.Point(0, 56);
            this.webViewTabControls.Log = null;
            this.webViewTabControls.Margin = new System.Windows.Forms.Padding(0);
            this.webViewTabControls.Name = "webViewTabControls";
            this.webViewTabControls.SelectedIndex = 0;
            this.webViewTabControls.Size = new System.Drawing.Size(984, 505);
            this.webViewTabControls.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.mainTableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.mainTableLayoutPanel.ResumeLayout(false);
            this.mainTableLayoutPanel.PerformLayout();
            this.mainToolStrip.ResumeLayout(false);
            this.mainToolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
        private System.Windows.Forms.ToolStrip mainToolStrip;
        private System.Windows.Forms.ToolStripButton logToolStripButton;
        private Controls.WebViewTabControl webViewTabControls;
    }
}