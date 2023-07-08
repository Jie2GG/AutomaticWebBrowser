using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

using AutomaticWebBrowser.Controls.InputValueDialog.ViewModels;
using AutomaticWebBrowser.Controls.InputValueDialog.Views;
using AutomaticWebBrowser.Services.Startup;
using AutomaticWebBrowser.Views;

using HandyControl.Controls;
using HandyControl.Tools.Extension;

using Prism.Commands;
using Prism.Mvvm;

using Unity;

namespace AutomaticWebBrowser.ViewModels
{
    class SettingViewModel : BindableBase
    {
        #region --字段--
        private ObservableCollection<FileInfo> configSources;
        private int configSelectedIndex;
        #endregion

        #region --属性--
        /// <summary>
        /// 配置文件源
        /// </summary>
        public ObservableCollection<FileInfo> ConfigSource
        {
            get => this.configSources;
            set => this.SetProperty (ref this.configSources, value);
        }

        /// <summary>
        /// 配置文件选择项
        /// </summary>
        public int ConfigSelectedIndex
        {
            get => this.configSelectedIndex;
            set => this.SetProperty (ref this.configSelectedIndex, value);
        }

        /// <summary>
        /// 启动参数
        /// </summary>
        public BootParameters BootParameters { get; }
        #endregion

        #region --命令--
        /// <summary>
        /// 初始化命令
        /// </summary>
        public ICommand InitializeCommand => new DelegateCommand (() =>
        {
            DirectoryInfo dir = new (Path.GetFullPath (this.BootParameters.ConfigDirectory));
            if (dir.Exists)
            {
                FileInfo[] files = dir.GetFiles ("*.json");
                for (int i = 0; i < files.Length; i++)
                {
                    this.ConfigSource.Add (files[i]);
                    if (string.Compare (files[i].Name, this.BootParameters.ConfigName, true) == 0)
                    {
                        this.ConfigSelectedIndex = i;
                    }
                }
            }

        });

        /// <summary>
        /// 创建新配置命令
        /// </summary>
        public ICommand CreateNewCommand => new DelegateCommand (() =>
        {
            /*
             DialogResult = await Dialog.Show<InteractiveDialog>()
                .Initialize<InteractiveDialogViewModel>(vm => vm.Message = DialogResult)
                .GetResultAsync<string>();
             */
            Dialog.Show<InputValueDialogView> (nameof (SettingView))
                .GetResultAsync<string> ()
                .ContinueWith (task =>
                {
                    // 创建文件

                    // 加入配置列表
                });
        });
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="SettingViewModel"/> 类的新实例
        /// </summary>
        public SettingViewModel ([Dependency] BootParameters bootParameters)
        {
            this.BootParameters = bootParameters;

            this.configSources = new ObservableCollection<FileInfo> ();
        }
        #endregion
    }
}
