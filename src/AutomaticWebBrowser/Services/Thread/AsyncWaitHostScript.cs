﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutomaticWebBrowser.Services.Thread
{
    /// <summary>
    /// 线程等待 JavaScript 主机服务类
    /// </summary>
    [ComVisible (true)]
    public class AsyncWaitHostScript
    {
        #region --字段--
        private readonly AutoResetEvent waitEvent;
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="AsyncWaitHostScript"/> 类的新实例
        /// </summary>
        public AsyncWaitHostScript ()
        {
            this.waitEvent = new AutoResetEvent (false);
        }
        #endregion

        #region --公开方法--
        /// <summary>
        /// 等待异步执行完成
        /// </summary>
        public void WaitOne ()
        {
            this.waitEvent.Reset ();
            this.waitEvent.WaitOne ();
        }

        /// <summary>
        /// 设置等待为有信号状态
        /// </summary>
        public void Set ()
        {
            this.waitEvent.Set ();
        }
        #endregion
    }
}
