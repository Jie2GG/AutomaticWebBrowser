namespace AutomaticWebBrowser.Services.Configuration.Converter
{
    abstract class ObjectConverter
    {
        public abstract bool Convert (object sourceObj, out object? targetObj);
    }
}
