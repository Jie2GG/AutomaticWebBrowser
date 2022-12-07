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
    /// 主窗口
    /// </summary>
    partial class MainForm : Form
    {
        #region --字段--
        private readonly LogForm logForm;
        #endregion

        #region --属性--
        /// <summary>
        /// 配置项
        /// </summary>
        public Config Config { get; }

        /// <summary>
        /// 日志窗口
        /// </summary>
        public LogForm LogForm => logForm;

        /// <summary>
        /// 日志框架
        /// </summary>
        public Logger Log { get; }
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="MainForm"/> 类的新实例
        /// </summary>
        /// <param name="params"></param>
        public MainForm (BootParameters @params)
        {
            // 初始化控件
            this.InitializeComponent ();

            try
            {
                // 读取配置文件
                this.Config = ConfigurationService.LoadFrom (@params.ConfigPath);
                this.Config.Check ();
            }
            catch (Exception e)
            {
                ErrorDialog.ShowException (null, e);
                return;
            }

            #region 初始化日志框架
            if (this.Config.Log != null && this.Config.Log.Window != null)
            {
                // 创建日志窗口
                this.logForm ??= new LogForm ()
                {
                    // 设置窗口客户区大小
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
                throw new ProviderException ("未找到日志的配置描述信息, 属性名: “log”");
            }
            #endregion

            #region 初始化浏览器核心
            if (this.Config.Browser != null && this.Config.Browser.Window != null)
            {
                // 初始化浏览器内核
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

                // 配置浏览器缓存
                string profilePath = Path.GetFullPath (this.Config.Browser.Profile);
                if (!Directory.Exists (profilePath))
                {
                    Directory.CreateDirectory (profilePath);
                }
                Xpcom.ProfileDirectory = profilePath;
                this.Log.Information ($"Browser profile directory: {profilePath}.");

                // 配置浏览器语言
                GeckoPreferences.User["intl.accept_languages"] = this.Config.Browser.Language;
                this.Log.Information ($"Browser language: {this.Config.Browser.Language}.");
                // 配置浏览器UA
                GeckoPreferences.User["general.useragent.override"] = this.Config.Browser.UserAgent;
                this.Log.Information ($"Browser User-Agent: {this.Config.Browser.UserAgent}.");
                // 配置字体渲染
                GeckoPreferences.User["gfx.font_rendering.graphite.enabled"] = this.Config.Browser.EnableFontRendering;
                this.Log.Information ($"Browser FontRendering: {this.Config.Browser.EnableFontRendering}.");
                //配置浏览器不被追踪
                GeckoPreferences.User["privacy.donottrackheader.enabled"] = this.Config.Browser.EnableDonottrackheader;
                this.Log.Information ($"Browser do not track header: {this.Config.Browser.EnableDonottrackheader}.");
            }
            else
            {
                throw new ProviderException ("未找到浏览器的配置描述信息, 属性名: “browser”");
            }
            #endregion

            // 设置窗体状态
            this.WindowState = this.Config.Browser.Window.State;
            // 设置窗口客户区大小
            this.Size = this.Config.Browser.Window.Size ?? WindowSize.MainFormSize;
            // 设置窗口是否显示
            this.Visible = this.Config.Browser.Window.Visible;
            // 设置浏览器日志
            this.webView.Log = this.Log;
        }
        #endregion

        #region --私有方法--
        // 窗体显示
        protected override void OnShown (EventArgs e)
        {
            // 处理事件
            Application.DoEvents ();

            // 创建任务
            this.webView.CreateTask (this.Config.Task);
            this.webView.RunningTask ();
        }

        // 日志按钮点击事件
        private void LogToolStripButtonClickEventHandler (object sender, EventArgs e)
        {
            this.LogForm.Show ();
        }
        #endregion
    }
}