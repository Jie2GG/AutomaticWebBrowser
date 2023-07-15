using System.Text.Json;

namespace AutomaticWebBrowser.Services.Configuration.Converter
{
    class StringObjectConverter : ObjectConverter
    {
        public override bool Convert (object sourceObj, out object? targetObj)
        {
            targetObj = null;
            if (sourceObj is JsonElement json)
            {
                targetObj = json.Deserialize<string> (Global.DefaultJsonSerializerOptions);
                return true;
            }
            return false;
        }
    }
}
