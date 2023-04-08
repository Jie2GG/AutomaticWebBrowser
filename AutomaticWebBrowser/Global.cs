using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutomaticWebBrowser
{
    static class Global
    {
        public static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions ()
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

        public static class DOM
        {
            #region --常量--
            public const string DOM_KEY_EVENTS = "KeyEvents";
            public const string DOM_MOUSE_EVENT = "MouseEvent";

            public const string EVENT_FOCUS = "focus";
            public const string EVENT_BLUR = "blur";
            public const string EVENT_KEY_PRESS = "keypress";
            public const string EVENT_KEY_DOWN = "keydown";
            public const string EVENT_KEY_UP = "keyup";
            public const string EVENT_MOUSE_DOWN = "mousedown";
            public const string EVENT_MOUSE_UP = "mouseup";
            public const string EVENT_CLICK = "click";
            public const string EVENT_DBLCLICK = "dblclick";
            #endregion
        }
    }
}
