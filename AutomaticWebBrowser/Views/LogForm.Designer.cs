namespace AutomaticWebBrowser.Views
{
    partial class LogForm
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
            this.logRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // logRichTextBox
            // 
            this.logRichTextBox.AcceptsTab = true;
            this.logRichTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.logRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.logRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logRichTextBox.Location = new System.Drawing.Point(0, 0);
            this.logRichTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.logRichTextBox.Name = "logRichTextBox";
            this.logRichTextBox.ReadOnly = true;
            this.logRichTextBox.ShortcutsEnabled = false;
            this.logRichTextBox.Size = new System.Drawing.Size(1568, 522);
            this.logRichTextBox.TabIndex = 0;
            this.logRichTextBox.TabStop = false;
            this.logRichTextBox.Text = "";
            this.logRichTextBox.WordWrap = false;
            // 
            // LogForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1568, 522);
            this.Controls.Add(this.logRichTextBox);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.Name = "LogForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "日志";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox logRichTextBox;
    }
}