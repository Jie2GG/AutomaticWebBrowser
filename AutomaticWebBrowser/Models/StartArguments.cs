using Ookii.CommandLine;

namespace AutomaticWebBrowser.Models
{
    /// <summary>
    /// 启动参数
    /// </summary>
    public class StartArguments
    {
        /// <summary>
        /// 配置文件位置
        /// </summary>
        [CommandLineArgument ("config", DefaultValue = "config.json")]
        public string Config { get; set; }
    }
}
