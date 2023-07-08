using HandyControl.Controls;

namespace AutomaticWebBrowser.Views
{
    /// <summary>
    /// SettingView.xaml 的交互逻辑
    /// </summary>
    public partial class SettingView : Window
    {
        public SettingView ()
        {
            this.InitializeComponent ();
            Dialog.SetToken (this, nameof (SettingView));
        }
    }
}
