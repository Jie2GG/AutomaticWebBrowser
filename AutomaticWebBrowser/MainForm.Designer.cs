﻿namespace AutomaticWebBrowser
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
            this.webView = new AutomaticWebBrowser.Controls.WebView();
            this.mainTableLayoutPanel.SuspendLayout();
            this.mainToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTableLayoutPanel
            // 
            this.mainTableLayoutPanel.ColumnCount = 1;
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.Controls.Add(this.mainToolStrip, 0, 0);
            this.mainTableLayoutPanel.Controls.Add(this.webView, 0, 1);
            this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.mainTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            this.mainTableLayoutPanel.RowCount = 2;
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.Size = new System.Drawing.Size(800, 450);
            this.mainTableLayoutPanel.TabIndex = 0;
            // 
            // mainToolStrip
            // 
            this.mainToolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logToolStripButton});
            this.mainToolStrip.Location = new System.Drawing.Point(1, 3);
            this.mainToolStrip.Margin = new System.Windows.Forms.Padding(1, 3, 1, 1);
            this.mainToolStrip.Name = "mainToolStrip";
            this.mainToolStrip.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.mainToolStrip.Size = new System.Drawing.Size(798, 24);
            this.mainToolStrip.TabIndex = 0;
            this.mainToolStrip.Text = "ToolStrip";
            // 
            // logToolStripButton
            // 
            this.logToolStripButton.AutoSize = false;
            this.logToolStripButton.Image = global::AutomaticWebBrowser.Properties.Resources.icon_logs;
            this.logToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.logToolStripButton.Margin = new System.Windows.Forms.Padding(0);
            this.logToolStripButton.Name = "logToolStripButton";
            this.logToolStripButton.Size = new System.Drawing.Size(56, 23);
            this.logToolStripButton.Text = "日志";
            this.logToolStripButton.Click += new System.EventHandler(this.LogToolStripButtonClickEventHandler);
            // 
            // webView
            // 
            this.webView.ConsoleMessageEventReceivesConsoleLogCalls = true;
            this.webView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webView.FrameEventsPropagateToMainWindow = false;
            this.webView.Location = new System.Drawing.Point(3, 31);
            this.webView.Log = null;
            this.webView.Name = "webView";
            this.webView.Size = new System.Drawing.Size(794, 416);
            this.webView.TabIndex = 1;
            this.webView.UseHttpActivityObserver = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.mainTableLayoutPanel);
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
        private Controls.WebView webView;
    }
}