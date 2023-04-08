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
        /// Ӧ�ó������Ҫ��ڵ�
        /// </summary>
        [STAThread]
        static void Main (params string[] args)
        {
            // ��ʼ�� WinForm
            Application.EnableVisualStyles ();
            Application.SetCompatibleTextRenderingDefault (false);

            // ������������
            BootParameters @params = CommandLineParser.Parse<BootParameters> (args);
            Config config = null;
            Logger log = null;
            LogForm logForm = null;

            try
            {
                // ��ȡ�����ļ�
                config = ConfigurationService.LoadFrom (@params.ConfigPath);
                config.Check ();

                #region ��ʼ����־���
                if (config.Log != null && config.Log.Window != null)
                {
                    // ������־����
                    logForm = new LogForm ()
                    {
                        Size = config.Log.Window.Size ?? WindowSize.LogFormSize
                    };

                    // �����
                    logForm.Activate ();

                    // ��ʼ�����
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
                    throw new ProviderException ("δ�ҵ���־������������Ϣ, ������: ��log��");
                }
                #endregion

                #region ��ʼ��������ں�
                // ��ʼ�������
                if (config.Browser != null && config.Browser.Window != null)
                {
                    // ��ʼ��������ں�
                    if (Environment.Is64BitProcess)
                    {
                        Xpcom.Initialize ("lib\\x64");
                        log.Information ($"��ʼ�� --> ���������: 64bit");
                    }
                    else
                    {
                        Xpcom.Initialize ("lib\\x86");
                        log.Information ($"��ʼ�� --> ���������: 32bit");
                    }

                    // �������������
                    string profilePath = Path.GetFullPath (config.Browser.Profile);
                    if (!Directory.Exists (profilePath))
                    {
                        Directory.CreateDirectory (profilePath);
                    }
                    Xpcom.ProfileDirectory = profilePath;
                    log.Information ($"��ʼ�� --> ���������Ŀ¼: {profilePath}");

                    // �������������
                    GeckoPreferences.User["intl.accept_languages"] = config.Browser.Language;
                    log.Information ($"��ʼ�� --> �����Ĭ������: {config.Browser.Language}");
                    // ���������UA
                    GeckoPreferences.User["general.useragent.override"] = config.Browser.UserAgent;
                    log.Information ($"��ʼ�� --> �����User-Agent: {config.Browser.UserAgent}");
                    // ����������Ⱦ
                    GeckoPreferences.User["gfx.font_rendering.graphite.enabled"] = config.Browser.EnableFontRendering;
                    log.Information ($"��ʼ�� --> ���������������Ⱦ {config.Browser.EnableFontRendering}");
                    //�������������׷��
                    GeckoPreferences.User["privacy.donottrackheader.enabled"] = config.Browser.EnableDonottrackheader;
                    log.Information ($"��ʼ�� --> �����ϣ������׷��: {config.Browser.EnableDonottrackheader}");
                }
                else
                {
                    throw new ProviderException ("δ�ҵ������������������Ϣ, ������: ��browser��");
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
                        log?.Information ($"��������� --> ��ʼ���������б�, ����: {config.Tasks.Length}");
                        foreach (AutomaticTask task in config.Tasks)
                        {
                            // �������������
                            BrowserForm browserForm = new BrowserForm (config, log);

                            // ��ʾ����
                            browserForm.SafeShow (logForm);

                            // ������ָ���ĵ�ַ
                            browserForm.SafeNavigate (task.Url);

                            // ִ�������е����ж���
                            browserForm.RunActions (task.Actions);

                            // �رմ���
                            browserForm.SafeClose (logForm);
                        }
                        log?.Information ($"��������� --> ����ִ�����");
                    });
                };

                Application.Run (logForm);
            }
        }
    }
}