namespace AutomaticWebBrowser.Services.Automatic.Commands
{
    /// <summary>
    /// 带返回值的命令接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    interface ICommand<T> : ICommand
    {
        /// <summary>
        /// 命令执行后的返回值
        /// </summary>
        T Result { get; }
    }
}
