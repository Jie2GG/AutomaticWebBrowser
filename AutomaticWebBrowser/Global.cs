using System.Text.Json;
using System.Text.Json.Serialization;

using Serilog.Core;

namespace AutomaticWebBrowser
{
    public static class Global
    {
        public static readonly JsonSerializerOptions JsonSerializerOptions = new ()
        {
            // 设置字符编码器
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            // 允许逗号结尾的 Json
            AllowTrailingCommas = true,
            // 允许带注释的 Json
            ReadCommentHandling = JsonCommentHandling.Skip,
            // 处理循环引用
            ReferenceHandler = ReferenceHandler.Preserve,
            // 驼峰命名法
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            // 忽略 Null 值
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }
}
