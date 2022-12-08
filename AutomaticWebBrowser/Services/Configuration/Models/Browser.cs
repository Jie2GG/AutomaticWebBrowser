﻿using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace AutomaticWebBrowser.Services.Configuration.Models
{
    /// <summary>
    /// 配置文件浏览器
    /// </summary>
    class Browser
    {
        /// <summary>
        /// 窗口配置
        /// </summary>
        [JsonPropertyName ("window")]
        public Window Window { get; set; } = new Window ()
        {
            State = FormWindowState.Normal,
            Size = new WindowSize (1200, 900),
            Visible = true,
        };

        /// <summary>
        /// 配置缓存路径
        /// </summary>
        [JsonPropertyName ("profile")]
        public string Profile { get; set; } = "./profile";

        /// <summary>
        /// 默认语言
        /// </summary>
        [JsonPropertyName ("language")]
        public string Language { get; set; } = "zh-CN,zh;q=0.9,en;q=0.8";

        /// <summary>
        /// 用户表示
        /// </summary>
        [JsonPropertyName ("user-agent")]
        public string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:60.0) Gecko/20100101 Firefox/60.0";

        /// <summary>
        /// 启用字体渲染
        /// </summary>
        [JsonPropertyName ("enable-font-rendering")]
        public bool EnableFontRendering { get; set; } = true;

        /// <summary>
        /// 启用不被追踪
        /// </summary>
        [JsonPropertyName ("enable-donottrackheader")]
        public bool EnableDonottrackheader { get; set; } = true;

        /// <summary>
        /// 启用上下文菜单
        /// </summary>
        [JsonPropertyName ("enable-context-menu")]
        public bool EnableContextMenu { get; set; } = true;
    }
}