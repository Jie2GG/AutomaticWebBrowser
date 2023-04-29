using System.Text.Json.Serialization;

namespace AutomaticWebBrowser.Domain.Configuration.Models
{
    /// <summary>
    /// 窗体位置
    /// </summary>
    readonly struct AWWindowLocation
    {
        #region --常量--
        public static readonly AWWindowLocation Empty = new (0, 0);
        #endregion

        #region --属性--
        [JsonPropertyName ("x")]
        public int X { get; }

        [JsonPropertyName ("y")]
        public int Y { get; }
        #endregion

        #region --构造函数--
        [JsonConstructor]
        public AWWindowLocation (int x, int y)
            : this ()
        {
            this.X = x;
            this.Y = y;
        }
        #endregion
    }
}
