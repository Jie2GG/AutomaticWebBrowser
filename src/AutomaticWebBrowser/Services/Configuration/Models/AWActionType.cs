﻿namespace AutomaticWebBrowser.Services.Configuration.Models
{
    /// <summary>
    /// 动作类型
    /// </summary>
    enum AWActionType
    {
        /// <summary>
        /// 表示不执行任何操作
        /// </summary>
        None = 0,
        
        #region --软件动作--
        /// <summary>
        /// 表示关闭当前 Tab
        /// </summary>
        CloseTab = 1,
        #endregion

        #region --HTML动作类型--
        /// <summary>
        /// 设置元素 value 属性
        /// </summary>
        SetValue = 10,
        /// <summary>
        /// 设置元素的 value 属性, 并触发元素的 input 事件
        /// </summary>
        InputValue = 11,
        /// <summary>
        /// 调用元素的 click 函数
        /// </summary>
        Click = 12,
        /// <summary>
        /// 触发元素 keydown 事件
        /// </summary>
        KeyDown = 13,
        /// <summary>
        /// 触发元素 keyup 事件
        /// </summary>
        KeyUp = 14,
        /// <summary>
        /// 触发元素 keypress 事件
        /// </summary>
        KeyPress = 15,
        /// <summary>
        /// 触发元素 mousedown 事件
        /// </summary>
        MouseDown = 16,
        /// <summary>
        /// 触发 mouseup 事件
        /// </summary>
        MouseUp = 17,
        /// <summary>
        /// 触发 click 事件
        /// </summary>
        MouseClick = 18,
        /// <summary>
        /// 触发 dbclick 事件
        /// </summary>
        MouseDbClick = 19,
        /// <summary>
        /// 调用元素的 play 函数
        /// </summary>
        Play = 20,
        /// <summary>
        /// 调用元素的 pause 函数
        /// </summary>
        Pause = 21,
        /// <summary>
        /// 等待元素引发 ended 事件
        /// </summary>
        WaitEndedEvent = 22, 
        #endregion
        
        /// <summary>
        /// 运行子作业
        /// </summary>
        RunSubJob = 99,
    }
}
