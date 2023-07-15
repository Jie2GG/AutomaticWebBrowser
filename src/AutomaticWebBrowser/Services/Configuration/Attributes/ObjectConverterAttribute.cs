using System;

namespace AutomaticWebBrowser.Services.Configuration.Attributes
{
    [AttributeUsage (AttributeTargets.Field)]
    class ObjectConverterAttribute : Attribute
    {
        public Type ConverterType { get; set; }

        public ObjectConverterAttribute (Type converterType)
        {
            this.ConverterType = converterType;
        }
    }
}
