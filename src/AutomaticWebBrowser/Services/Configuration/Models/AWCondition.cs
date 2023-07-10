using System;
using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Services.Configuration.Models
{
    /// <summary>
    /// 动作执行条件
    /// </summary>
    class AWCondition : IEquatable<AWCondition>
    {
        #region --属性--
        /// <summary>
        /// 条件类型
        /// </summary>
        [JsonPropertyName ("type")]
        [JsonConverter (typeof (JsonStringEnumConverter))]
        public AWConditionType Type { get; set; } = AWConditionType.None;

        /// <summary>
        /// 条件值
        /// </summary>
        [JsonPropertyName ("value")]
        public object? Value { get; set; } = null; 
        #endregion

        #region --公开方法--
        /// <summary>
        /// 表示当前对象是否等于同一类型的另一个对象
        /// </summary>
        /// <param name="other">一个用来与当前对象比较的对象</param>
        /// <returns>如果当前对象等于另一个形参，返回 <see langword="true"/>; 否则, <see langword="false"/></returns>
        public bool Equals (AWCondition? other)
        {
            if (this == other)
            {
                return true;
            }

            return other != null && other.Type == this.Type;
        }

        /// <summary>
        /// 表示当前对象是否等于同一类型的另一个对象
        /// </summary>
        /// <param name="other">一个用来与当前对象比较的对象</param>
        /// <returns>如果当前对象等于另一个形参，返回 <see langword="true"/>; 否则, <see langword="false"/></returns>
        public override bool Equals (object? obj)
        {
            return this.Equals (obj as AWCondition);
        }

        /// <summary>
        /// 返回此实例值的哈希码
        /// </summary>
        /// <returns>32位有符号整数哈希码</returns>
        public override int GetHashCode ()
        {
            return this.Type.GetHashCode ();
        }
        #endregion
    }
}
