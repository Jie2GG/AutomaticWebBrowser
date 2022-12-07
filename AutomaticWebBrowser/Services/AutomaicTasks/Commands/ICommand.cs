namespace AutomaticWebBrowser.Services.AutomaicTasks.Commands
{
    /// <summary>
    /// 命令执行接口
    /// </summary>
    interface ICommand<T>
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <returns></returns>
        T Execute ();
    }
}
