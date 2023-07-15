using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

using AutomaticWebBrowser.Properties;

using AvalonDock.Properties;

using Gemini.Framework;
using Gemini.Modules.Output;

namespace AutomaticWebBrowser.Modules.Startup
{
    [Export (typeof (IModule))]
    class Module : ModuleBase
    {
        #region --属性--
        /// <summary>
        /// 输出接口
        /// </summary>
        public IOutput Output { get; }
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="Module"/> 类的新实例
        /// </summary>
        [ImportingConstructor]
        public Module (IOutput output)
        {
            this.Output = output;
        }
        #endregion

        #region --公开方法--
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize ()
        {
            // 显示工具栏
            this.Shell.ToolBars.Visible = true;

            // 修改窗体
            this.MainWindow.Title = Resource.StartupMainWindowTitle;
            this.MainWindow.Icon = (DrawingImage)Application.Current.FindResource ("BrowserDrawingImage");
        }
        #endregion
    }
}
