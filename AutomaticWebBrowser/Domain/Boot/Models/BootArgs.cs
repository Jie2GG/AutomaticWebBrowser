using Ookii.CommandLine;

namespace AutomaticWebBrowser.Domain.Boot.Models
{
    /// <summary>
    /// 启动参数模块类
    /// </summary>
    class BootArgs
    {
        /// <summary>
        /// 配置文件位置
        /// </summary>
        [CommandLineArgument ("config", DefaultValue = "config.json")]
        public string ConfigPath { get; set; } = "config.json";
    }
}
