using System.IO;
using System.Text.Json;

using AutomaticWebBrowser.Services.Configuration.Models;

namespace AutomaticWebBrowser.Services.Configuration
{
    /// <summary>
    /// 配置项服务
    /// </summary>
    internal static class ConfigurationService
    {
        /// <summary>
        /// 从指定的文件加载
        /// </summary>
        /// <param name="fileName"></param>
        /// <exception cref="FileNotFoundException"></exception>
        public static Config LoadFrom (string fileName)
        {
            string fullPath = Path.GetFullPath (fileName);
            if (!File.Exists (fullPath))
            {
                throw new FileNotFoundException ("配置文件不存在", fullPath);
            }

            return JsonSerializer.Deserialize<Config> (File.OpenRead (fullPath), Global.JsonSerializerOptions);
        }
    }
}
