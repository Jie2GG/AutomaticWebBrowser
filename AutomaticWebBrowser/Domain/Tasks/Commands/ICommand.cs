﻿using System.Threading.Tasks;

namespace AutomaticWebBrowser.Domain.Tasks.Commands
{
    /// <summary>
    /// 表示命令的接口
    /// </summary>
    interface ICommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <returns>执行结果</returns>
        bool Execute ();
    }
}
