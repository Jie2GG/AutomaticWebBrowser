using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Windows;

namespace AutomaticWebBrowser.Services.Configuration.Models
{
    /// <summary>
    /// 浏览器配置
    /// </summary>
    class AWBrowser
    {
        /// <summary>
        /// 数据路径
        /// </summary>
        [JsonPropertyName ("data-path")]
        [Category ("其他")]
        [DisplayName ("用户数据保存路径(data-path)")]
        [Description ("指定浏览器的缓存数据的存储路径")]
        public string DataPath { get; internal set; } = "./data";

        /// <summary>
        /// 启用跟踪防护功能
        /// </summary>
        [JsonPropertyName ("enable-tracking-prevention")]
        [Category ("安全")]
        [DisplayName ("跟踪防护(enable-tracking-prevention)")]
        [Description ("网站会使用跟踪器收集你的浏览信息, 此信息将用于改进网站服务并向你显示个性化广告等内容, 某些跟踪器会收集你的信息并将其发送到你未访问过的网站.")]
        public bool EnableTrackingPrevention { get; set; } = true;

        /// <summary>
        /// 启用密码自动保存
        /// </summary>
        [JsonPropertyName ("enable-password-autosave")]
        [Category ("安全")]
        [DisplayName ("自动填充密码(enable-password-autosave)")]
        [Description ("允许 Edge 自动保存密码并帮助确保密码安全")]
        public bool EnablePasswordAutosave { get; set; } = false;

        /// <summary>
        /// 启用开发者工具
        /// </summary>
        [JsonPropertyName ("enable-dev-tools")]
        [Category ("高级")]
        [DisplayName ("开发者工具(enable-dev-tools)")]
        [Description ("开启或关闭 \"开发人员工具(F12)\" 功能")]
        public bool EnableDevTools { get; set; } = false;

        /// <summary>
        /// 启用上下文菜单
        /// </summary>
        [JsonPropertyName ("enable-context-menu")]
        [Category ("高级")]
        [DisplayName ("上下文菜单(enable-context-menu)")]
        [Description ("开启或关闭浏览器的右键菜单")]
        public bool EnableContextMenu { get; set; } = true;
    }
}
