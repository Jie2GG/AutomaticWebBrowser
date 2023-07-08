using System.ComponentModel;

using HandyControl.Controls;

namespace AutomaticWebBrowser.Views
{
    /// <summary>
    /// LogView.xaml 的交互逻辑
    /// </summary>
    public partial class LogView : Window
    {
        #region --构造函数--
        public LogView ()
        {
            this.InitializeComponent ();
        } 
        #endregion

        #region --私有方法--
        /// <summary>
        /// 窗口即将关闭事件
        /// </summary>
        protected override void OnClosing (CancelEventArgs e)
        {
            this.Hide ();
            e.Cancel = true;
        } 
        #endregion
    }
}
