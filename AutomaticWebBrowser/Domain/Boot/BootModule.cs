using AutomaticWebBrowser.Domain.Boot.Models;

using Ookii.CommandLine;

namespace AutomaticWebBrowser.Domain.Boot
{
    /// <summary>
    /// 启动模块
    /// </summary>
    class BootModule
    {
        /// <summary>
        /// 尝试解析启动参数
        /// </summary>
        /// <param name="args">启动参数</param>
        /// <param name="bootArgs"><see cref="BootArgs"/> 实例</param>
        /// <returns>转换成功为 <see langword="true"/>, 否则为 <see langword="false"/></returns>
        public static bool TryParse (string[] args, out BootArgs? bootArgs)
        {
            try
            {
                CommandLineParser<BootArgs> commandLineParser = new ();
                bootArgs = commandLineParser.Parse (args);
                return commandLineParser.ParseResult.Status == ParseStatus.Success;
            }
            catch
            {
                bootArgs = null;
                return false;
            }
        }
    }
}
