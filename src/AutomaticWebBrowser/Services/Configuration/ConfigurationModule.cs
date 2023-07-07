using System.IO;
using System.Text.Json;

using AutomaticWebBrowser.Wpf.Services.Configuration.Models;

namespace AutomaticWebBrowser.Wpf.Services.Configuration
{
    /// <summary>
    /// 配置模块
    /// </summary>
    class ConfigurationModule
    {
        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns>配置对象</returns>
        /// <exception cref="FileNotFoundException">配置文件不存在</exception>
        public static AWBrowser? Load (string fileName)
        {
            string fullPath = Path.GetFullPath (fileName);
            if (!File.Exists (fullPath))
            {
                throw new FileNotFoundException ("配置文件不存在", fullPath);
            }

            return JsonSerializer.Deserialize<AWBrowser> (File.OpenRead (fullPath), Global.DefaultJsonSerializerOptions);
        }
    }
}
