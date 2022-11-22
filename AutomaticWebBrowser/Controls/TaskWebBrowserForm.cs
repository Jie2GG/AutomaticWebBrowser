﻿using System;
using System.IO;
using System.Windows.Forms;

using AutomaticWebBrowser.Models;

using Gecko;

using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace AutomaticWebBrowser.Controls
{
    /// <summary>
    /// 表示任务Web浏览器窗口的类
    /// </summary>
    public class TaskWebBrowserForm : Form
    {
        #region --属性--
        public Configuration Config { get; }
        public Logger Log { get; private set; }
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="TaskWebBrowserForm"/> 类的新实例
        /// </summary>
        /// <param name="config">配置文件</param>
        public TaskWebBrowserForm (Configuration config)
        {
            this.Config = config;

            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.Padding = Padding.Empty;
            this.ClientSize = config.Setting.Window.Size;
            this.WindowState = config.Setting.Window.Maximize ? FormWindowState.Maximized : FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        #endregion

        #region --公开方法--
        // 窗口加载事件
        protected override void OnLoad (EventArgs e)
        {
            #region 日志处理
            string logPath = Path.Combine (Path.GetFullPath (this.Config.Setting.Log.Path), $"{DateTime.Now:yyyyMMdd}.log");

            // 创建日志
            this.Log = new LoggerConfiguration ()
#if DEBUG
                .WriteTo.Debug (LogEventLevel.Verbose, this.Config.Setting.Log.Format)
#else
                .WriteTo.Trace (this.Config.Setting.Log.Level, this.Config.Setting.Log.Format)
#endif
                .WriteTo.File (logPath, this.Config.Setting.Log.Level, this.Config.Setting.Log.Format)
                .CreateLogger ();

            // 日志窗口
            if (this.Config.Setting.Log.ShowLogs)
            {
                new TaskWebBrowserLogForm ().Show ();
            }
            #endregion

            #region 浏览器处理
            if (Environment.Is64BitProcess)
            {
                Xpcom.Initialize ("lib\\x64");
            }
            else
            {
                Xpcom.Initialize ("lib\\x86");
            }

            // 配置浏览器缓存
            string profilePath = Path.GetFullPath (this.Config.Setting.Browser.Profile);
            if (!Directory.Exists (profilePath))
            {
                Directory.CreateDirectory (profilePath);
            }
            Xpcom.ProfileDirectory = profilePath;

            // 配置浏览器语言
            GeckoPreferences.User["intl.accept_languages"] = this.Config.Setting.Browser.Language;
            // 配置浏览器UA
            GeckoPreferences.User["general.useragent.override"] = this.Config.Setting.Browser.UserAgent;
            // 配置字体渲染
            GeckoPreferences.User["gfx.font_rendering.graphite.enabled"] = this.Config.Setting.Browser.EnableFontRendering;
            //配置浏览器不被追踪
            GeckoPreferences.User["privacy.donottrackheader.enabled"] = this.Config.Setting.Browser.EnableDoNotTrackHeader;

            // 创建浏览器
            TaskWebBrowser webControl = new TaskWebBrowser (this.Config.TaskInfo, this.Log)
            {
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
                Padding = Padding.Empty,
            };
            // 创建浏览器所有子控件
            webControl.CreateControl ();
            // 配置浏览器上下文菜单
            webControl.NoDefaultContextMenu = !this.Config.Setting.Browser.EnableContextMenu;

            // 添加到子控件
            this.Controls.Add (webControl);

            // 开始导航到任务首页
            webControl.Navigate (this.Config.TaskInfo.Url);
            #endregion
        }
        #endregion
    }
}
