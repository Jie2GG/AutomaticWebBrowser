using System.Text.Json.Serialization;
using System.Windows;

namespace AutomaticWebBrowser.Wpf.Services.Configuration.Models
{
    /// <summary>
    /// 浏览器配置
    /// </summary>
    class AWBrowser
    {
        /// <summary>
        /// 窗体
        /// </summary>
        [JsonPropertyName ("window")]
        public AWWindow Window { get; set; } = new AWWindow ()
        {
            State = WindowState.Normal,
            StartupLocation = WindowStartupLocation.CenterScreen,
            Width = 1200D,
            Height = 900D,
        };

        /// <summary>
        /// 数据路径
        /// </summary>
        [JsonPropertyName ("data-path")]
        public string DataPath { get; internal set; } = "./data";

        /// <summary>
        /// 启用跟踪防护功能
        /// </summary>
        [JsonPropertyName ("enable-tracking-prevention")]
        public bool EnableTrackingPrevention { get; set; } = true;

        /// <summary>
        /// 启用密码自动保存
        /// </summary>
        [JsonPropertyName ("enable-password-autosave")]
        public bool EnablePasswordAutosave { get; set; } = false;

        /// <summary>
        /// 启用开发者工具
        /// </summary>
        [JsonPropertyName ("enable-dev-tools")]
        public bool EnableDevTools { get; set; } = false;

        /// <summary>
        /// 启用上下文菜单
        /// </summary>
        [JsonPropertyName ("enable-context-menu")]
        public bool EnableContextMenu { get; set; } = true;
    }
}
