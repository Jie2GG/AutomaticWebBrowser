using Ookii.CommandLine;

namespace AutomaticWebBrowser.Models
{
    /// <summary>
    /// 启动参数
    /// </summary>
    internal class BootParameters
    {
        /// <summary>
        /// 配置文件位置
        /// </summary>
        [CommandLineArgument ("config", DefaultValue = "config.json")]
        public string ConfigPath { get; set; }
    }
}
