using System.Text.Json;

using AutomaticWebBrowser.Services.Configuration.Models;

namespace AutomaticWebBrowser.Services.Configuration.Converter
{
    class KeyboardObjectConverter : ObjectConverter
    {
        public override bool Convert (object sourceObj, out object? targetObj)
        {
            targetObj = null;
            if (sourceObj is JsonElement json)
            {
                targetObj = json.Deserialize<AWKeyboard> (Global.DefaultJsonSerializerOptions);
                return true;
            }
            return false;
        }
    }
}
