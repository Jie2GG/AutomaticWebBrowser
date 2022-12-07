using System;
using System.Configuration.Provider;
using System.IO;
using System.Windows.Forms;

using AutomaticWebBrowser.Controls;
using AutomaticWebBrowser.Models;
using AutomaticWebBrowser.Services.Configuration;
using AutomaticWebBrowser.Services.Configuration.Models;
using AutomaticWebBrowser.Services.Loggers;
using AutomaticWebBrowser.Views;

using Gecko;

using Serilog;
using Serilog.Core;

namespace AutomaticWebBrowser
{
    /// <summary>
    /// ������
    /// </summary>
    partial class MainForm : Form
    {
        #region --�ֶ�--
        private readonly LogForm logForm;
        #endregion

        #region --����--
        /// <summary>
        /// ������
        /// </summary>
        public Config Config { get; }

        /// <summary>
        /// ��־����
        /// </summary>
        public LogForm LogForm => logForm;

        /// <summary>
        /// ��־���
        /// </summary>
        public Logger Log { get; }
        #endregion

        #region --���캯��--
        /// <summary>
        /// ��ʼ�� <see cref="MainForm"/> �����ʵ��
        /// </summary>
        /// <param name="params"></param>
        public MainForm (BootParameters @params)
        {
            // ��ʼ���ؼ�
            this.InitializeComponent ();

            try
            {
                // ��ȡ�����ļ�
                this.Config = ConfigurationService.LoadFrom (@params.ConfigPath);
                this.Config.Check ();
            }
            catch (Exception e)
            {
                ErrorDialog.ShowException (null, e);
                return;
            }

            #region ��ʼ����־���
            if (this.Config.Log != null && this.Config.Log.Window != null)
            {
                // ������־����
                this.logForm ??= new LogForm ()
                {
                    // ���ô��ڿͻ�����С
                    Size = this.Config.Log.Window.Size ?? WindowSize.LogFormSize
                };
                this.LogForm.Activate ();
                if (this.Config.Log.Window.Visible)
                {
                    this.LogForm.Show ();
                }

                string logPath = Path.Combine (Path.GetFullPath (this.Config.Log.SavePath), $"{DateTime.Now:yyyyMMdd}.log");
                this.Log = new LoggerConfiguration ()
#if DEBUG
                    .WriteTo.Debug (Serilog.Events.LogEventLevel.Verbose, this.Config.Log.Format)
#else
                    .WriteTo.Trace (this.Config.Log.Level, this.Config.Log.Format)
#endif
                    .WriteTo.LogForm (this.LogForm, this.Config.Log.Level, this.Config.Log.Format)
                    .WriteTo.File (logPath, this.Config.Log.Level, this.Config.Log.Format)
                    .CreateLogger ();
            }
            else
            {
                throw new ProviderException ("δ�ҵ���־������������Ϣ, ������: ��log��");
            }
            #endregion

            #region ��ʼ�����������
            if (this.Config.Browser != null && this.Config.Browser.Window != null)
            {
                // ��ʼ��������ں�
                if (Environment.Is64BitProcess)
                {
                    Xpcom.Initialize ("lib\\x64");
                    this.Log.Information ($"Browser cores: x64.");
                }
                else
                {
                    Xpcom.Initialize ("lib\\x86");
                    this.Log.Information ($"Browser cores: x86.");
                }

                // �������������
                string profilePath = Path.GetFullPath (this.Config.Browser.Profile);
                if (!Directory.Exists (profilePath))
                {
                    Directory.CreateDirectory (profilePath);
                }
                Xpcom.ProfileDirectory = profilePath;
                this.Log.Information ($"Browser profile directory: {profilePath}.");

                // �������������
                GeckoPreferences.User["intl.accept_languages"] = this.Config.Browser.Language;
                this.Log.Information ($"Browser language: {this.Config.Browser.Language}.");
                // ���������UA
                GeckoPreferences.User["general.useragent.override"] = this.Config.Browser.UserAgent;
                this.Log.Information ($"Browser User-Agent: {this.Config.Browser.UserAgent}.");
                // ����������Ⱦ
                GeckoPreferences.User["gfx.font_rendering.graphite.enabled"] = this.Config.Browser.EnableFontRendering;
                this.Log.Information ($"Browser FontRendering: {this.Config.Browser.EnableFontRendering}.");
                //�������������׷��
                GeckoPreferences.User["privacy.donottrackheader.enabled"] = this.Config.Browser.EnableDonottrackheader;
                this.Log.Information ($"Browser do not track header: {this.Config.Browser.EnableDonottrackheader}.");
            }
            else
            {
                throw new ProviderException ("δ�ҵ������������������Ϣ, ������: ��browser��");
            }
            #endregion

            // ���ô���״̬
            this.WindowState = this.Config.Browser.Window.State;
            // ���ô��ڿͻ�����С
            this.Size = this.Config.Browser.Window.Size ?? WindowSize.MainFormSize;
            // ���ô����Ƿ���ʾ
            this.Visible = this.Config.Browser.Window.Visible;
            // �����������־
            this.webView.Log = this.Log;
        }
        #endregion

        #region --˽�з���--
        // ������ʾ
        protected override void OnShown (EventArgs e)
        {
            // �����¼�
            Application.DoEvents ();

            // ��������
            this.webView.CreateTask (this.Config.Task);
            this.webView.RunningTask ();
        }

        // ��־��ť����¼�
        private void LogToolStripButtonClickEventHandler (object sender, EventArgs e)
        {
            this.LogForm.Show ();
        }
        #endregion
    }
}