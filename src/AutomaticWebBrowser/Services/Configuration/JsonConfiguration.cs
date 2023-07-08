using System;
using System.IO;
using System.Text.Json;

using AutomaticWebBrowser.Services.Configuration.Exceptions;
using AutomaticWebBrowser.Services.Configuration.Models;

namespace AutomaticWebBrowser.Services.Configuration
{
    static class JsonConfiguration
    {
        /// <summary>
        /// 从文件中加载配置
        /// </summary>
        public static AWConfig LoadFile (FileInfo fileInfo)
        {
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException ($"配置文件 ({fileInfo.Name}) 加载失败. 原因: 文件不存在", fileInfo.Name);
            }
            return JsonSerializer.Deserialize<AWConfig> (fileInfo.OpenRead (), Global.DefaultJsonSerializerOptions) ?? throw new ConfigurationException ($"配置文件 ({fileInfo.Name}) 加载失败. {Environment.NewLine} 原因: 未知错误");
        }
    }
}
