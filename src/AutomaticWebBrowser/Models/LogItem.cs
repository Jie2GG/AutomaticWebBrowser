using System.Windows.Media;

namespace AutomaticWebBrowser.Models
{
    class LogItem
    {
        public string Text { get; set; } = string.Empty;
        public Brush Foreground { get; set; } = Brushes.Black;
        public Brush Background { get; set; } = Brushes.Transparent;
    }
}
