using System;
using System.Configuration.Provider;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

using AutomaticWebBrowser.Models;
using AutomaticWebBrowser.Services.Configuration;
using AutomaticWebBrowser.Services.Configuration.Models;
using AutomaticWebBrowser.Services.Loggers;
using AutomaticWebBrowser.Views;

using Gecko;

using Ookii.CommandLine;

using Serilog;
using Serilog.Core;

namespace AutomaticWebBrowser
{
    class Program
    {
        /// <summary>
        /// 应用程序的主要入口点
        /// </summary>
        [STAThread]
        static void Main (params string[] args)
        {
            // 初始化 WinForm
            Application.EnableVisualStyles ();
            Application.SetCompatibleTextRenderingDefault (false);

            // 解析启动命令
            BootParameters @params = CommandLineParser.Parse<BootParameters> (args);
            Config config = null;
            Logger log = null;
            LogForm logForm = null;

            try
            {
                // 读取配置文件
                config = ConfigurationService.LoadFrom (@params.ConfigPath);
                config.Check ();

                #region 初始化日志框架
                if (config.Log != null && config.Log.Window != null)
                {
                    // 创建日志窗体
                    logForm = new LogForm ()
                    {
                        Size = config.Log.Window.Size ?? WindowSize.LogFormSize
                    };

                    // 激活窗体
                    logForm.Activate ();

                    // 初始化框架
                    string logPath = Path.Combine (Path.GetFullPath (config.Log.SavePath), $"{DateTime.Now:yyyyMMdd}.log");
                    log = new LoggerConfiguration ()
#if DEBUG
                        .WriteTo.Debug (Serilog.Events.LogEventLevel.Verbose, config.Log.Format)
#else
                        .WriteTo.Trace (config.Log.Level, config.Log.Format)
#endif
                        .WriteTo.LogForm (logForm, config.Log.Level, config.Log.Format)
                        .WriteTo.File (logPath, config.Log.Level, config.Log.Format)
                        .CreateLogger ();
                }
                else
                {
                    throw new ProviderException ("未找到日志的配置描述信息, 属性名: “log”");
                }
                #endregion

                #region 初始化浏览器内核
                // 初始化浏览器
                if (config.Browser != null && config.Browser.Window != null)
                {
                    // 初始化浏览器内核
                    if (Environment.Is64BitProcess)
                    {
                        Xpcom.Initialize ("lib\\x64");
                        log.Information ($"初始化 --> 浏览器核心: 64bit");
                    }
                    else
                    {
                        Xpcom.Initialize ("lib\\x86");
                        log.Information ($"初始化 --> 浏览器核心: 32bit");
                    }

                    // 配置浏览器缓存
                    string profilePath = Path.GetFullPath (config.Browser.Profile);
                    if (!Directory.Exists (profilePath))
                    {
                        Directory.CreateDirectory (profilePath);
                    }
                    Xpcom.ProfileDirectory = profilePath;
                    log.Information ($"初始化 --> 浏览器配置目录: {profilePath}");

                    // 配置浏览器语言
                    GeckoPreferences.User["intl.accept_languages"] = config.Browser.Language;
                    log.Information ($"初始化 --> 浏览器默认语言: {config.Browser.Language}");
                    // 配置浏览器UA
                    GeckoPreferences.User["general.useragent.override"] = config.Browser.UserAgent;
                    log.Information ($"初始化 --> 浏览器User-Agent: {config.Browser.UserAgent}");
                    // 配置字体渲染
                    GeckoPreferences.User["gfx.font_rendering.graphite.enabled"] = config.Browser.EnableFontRendering;
                    log.Information ($"初始化 --> 浏览器启用字体渲染 {config.Browser.EnableFontRendering}");
                    //配置浏览器不被追踪
                    GeckoPreferences.User["privacy.donottrackheader.enabled"] = config.Browser.EnableDonottrackheader;
                    log.Information ($"初始化 --> 浏览器希望不被追踪: {config.Browser.EnableDonottrackheader}");
                }
                else
                {
                    throw new ProviderException ("未找到浏览器的配置描述信息, 属性名: “browser”");
                }
                #endregion
            }
            catch (Exception e)
            {
                MessageBox.Show (e.Message);
            }

            if (logForm != null)
            {
                logForm.Load += (sender, e) =>
                {
                    Task.Factory.StartNew (() =>
                    {
                        log?.Information ($"任务管理器 --> 开始载入任务列表, 数量: {config.Tasks.Length}");
                        foreach (AutomaticTask task in config.Tasks)
                        {
                            // 创建浏览器窗体
                            BrowserForm browserForm = new BrowserForm (config, log);

                            // 显示窗口
                            browserForm.SafeShow (logForm);

                            // 导航到指定的地址
                            browserForm.SafeNavigate (task.Url);

                            // 执行任务中的所有动作
                            browserForm.RunActions (task.Actions);

                            // 关闭窗口
                            browserForm.SafeClose (logForm);
                        }
                        log?.Information ($"任务管理器 --> 任务执行完毕");
                    });
                };

                Application.Run (logForm);
            }
        }
    }
}