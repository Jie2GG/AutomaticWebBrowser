using System;
using System.Threading;
using System.Threading.Tasks;

using AutomaticWebBrowser.Domain.Configuration.Models;

namespace AutomaticWebBrowser.Commons
{
    /// <summary>
    /// 表示条件同步服务的类
    /// </summary>
    class ConditionSynchronous : EventWaitHandle
    {
        #region --字段--
        private AWCondition? condition;
        #endregion

        #region --构造函数--
        /// <summary>
        /// 初始化 <see cref="ConditionSynchronous"/> 类的新实例
        /// </summary>
        public ConditionSynchronous ()
            : base (false, EventResetMode.AutoReset)
        { }
        #endregion

        #region --公开方法--
        /// <summary>
        /// 将事件状态设置为非终止，从而导致线程受阻
        /// </summary>
        /// <param name="condition">设置事件解锁条件</param>
        /// <exception cref="ArgumentNullException"><paramref name="condition"/> 为 <see langword="null"/></exception>
        public void Reset (AWCondition condition)
        {
            lock (this)
            {
                if (condition != null)
                {
                    this.condition = condition;
                    base.Reset ();
                }
                else
                {
                    base.Set ();
                }
            }
        }

        /// <summary>
        /// 将事件状态设置为有信号，从而允许一个或多个等待线程继续执行
        /// </summary>
        /// <param name="condition">用来比对解锁条件的实例</param>
        /// <returns>如果该操作成功，则为 <see langword="true"/>；否则为 <see langword="false"/></returns>
        public bool Set (AWCondition condition)
        {
            lock (this)
            {
                if (this.condition?.Equals (condition) == true)
                {
                    this.condition = null;
                    return base.Set ();
                }
            }

            return false;
        }

        /// <summary>
        /// 阻止当前线程，直到当前 <see cref="ConditionSynchronous"/> 收到信号
        /// </summary>
        public void WaitOneEx ()
        {
            base.WaitOne ();
        }
        #endregion
    }
}
