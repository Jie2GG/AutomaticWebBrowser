using Ookii.CommandLine;

namespace AutomaticWebBrowser.Services.Startup
{
    /// <summary>
    /// 启动参数模块类
    /// </summary>
    class BootParameters
    {
        /// <summary>
        /// 配置文件名称
        /// </summary>
        [CommandLineArgument ("config", DefaultValue = "config.json")]
        public string ConfigName { get; set; } = "config.json";

        /// <summary>
        /// 配置文件路径
        /// </summary>
        [CommandLineArgument ("dir", DefaultValue = "Config")]
        public string ConfigDirectory { get; set; } = "Config";
    }
}
