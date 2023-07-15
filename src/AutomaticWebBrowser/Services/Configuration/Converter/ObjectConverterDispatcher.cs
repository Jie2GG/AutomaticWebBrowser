using System;

namespace AutomaticWebBrowser.Services.Configuration.Converter
{
    class ObjectConverterDispatcher
    {
        public static bool IfConveter (Type converterType, object? sourceObj, out object? targetObj)
        {
            targetObj = null;
            if (converterType is not null && converterType.IsSubclassOf (typeof (ObjectConverter)))
            {
                object? instance = Activator.CreateInstance (converterType);
                if (instance is ObjectConverter converter && sourceObj is not null)
                {
                    return converter.Convert (sourceObj, out targetObj);
                }
            }
            return false;
        }
    }
}
