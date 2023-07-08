using System;
using System.Windows.Input;

using HandyControl.Tools.Extension;

using Prism.Commands;
using Prism.Mvvm;

namespace AutomaticWebBrowser.Controls.InputValueDialog.ViewModels
{
    class InputValueDialogViewModel : BindableBase, IDialogResultable<string>
    {
        #region --字段--
        private string result;
        #endregion

        #region --属性--
        /// <summary>
        /// 返回值
        /// </summary>
        public string Result
        {
            get => this.result;
            set => this.SetProperty (ref this.result, value);
        }
        #endregion

        #region --事件--
        /// <summary>
        /// 关闭对话框事件
        /// </summary>
        public Action? CloseAction { get; set; }
        #endregion

        #region --命令--
        /// <summary>
        /// 确认命令
        /// </summary>
        public ICommand ConfirmCommand => new DelegateCommand (() =>
        {
            this.CloseAction?.Invoke ();
        });

        /// <summary>
        /// 关闭命令
        /// </summary>
        public ICommand CloseCommand => new DelegateCommand (() =>
        {
            this.Result = string.Empty;
            this.CloseAction?.Invoke ();
        });
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="InputValueDialogViewModel"/> 类的新实例
        /// </summary>
        public InputValueDialogViewModel ()
        {
            this.result = string.Empty;
        }
        #endregion
    }
}
