using System;

namespace AutomaticWebBrowser.Domain.Configuration.Attributes
{
    [AttributeUsage (AttributeTargets.Field)]
    class KeyAttribute : Attribute
    {
        #region --属性--
        /// <summary>
        /// 获取或设置正常状态下的 Key 值
        /// </summary>
        public string NormalKey { get; set; }

        /// <summary>
        /// 获取或设置按下 Shift 键后的 Key 值
        /// </summary>
        public string ShiftKey { get; set; }

        /// <summary>
        /// 获取或设置按键按下的键值
        /// </summary>
        public int KeyCode { get; set; }
        #endregion

        #region --构造函数--
        public KeyAttribute (string normalKey)
        {
            this.NormalKey = normalKey;
            this.ShiftKey = normalKey;
        }

        public KeyAttribute (string normalKey, string shiftKey)
        {
            this.NormalKey = normalKey;
            this.ShiftKey = shiftKey;
        }
        #endregion
    }
}