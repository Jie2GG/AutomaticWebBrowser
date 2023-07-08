using System;
using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Services.Configuration.Models
{
    /// <summary>
    /// 动作执行条件
    /// </summary>
    class AWCondition : IEquatable<AWCondition>
    {
        /// <summary>
        /// 条件类型
        /// </summary>
        [JsonPropertyName ("type")]
        [JsonConverter (typeof (JsonStringEnumConverter))]
        public AWConditionType Type { get; set; } = AWConditionType.None;

        public bool Equals (AWCondition? other)
        {
            if (this == other)
            {
                return true;
            }

            return other != null && other.Type == this.Type;
        }

        public override bool Equals (object? obj)
        {
            return this.Equals (obj as AWCondition);
        }

        public override int GetHashCode ()
        {
            return HashCode.Combine (this);
        }
    }
}
