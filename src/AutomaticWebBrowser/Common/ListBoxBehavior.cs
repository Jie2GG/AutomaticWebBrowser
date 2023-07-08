using System;
using System.Collections.Specialized;
using System.Windows.Controls;

using Microsoft.Xaml.Behaviors;

namespace AutomaticWebBrowser.Common
{
    class ListBoxBehavior : Behavior<ListBox>
    {
        protected override void OnAttached ()
        {
            base.OnAttached ();

            if (this.AssociatedObject.ItemsSource is INotifyCollectionChanged collectionChanged)
            {
                collectionChanged.CollectionChanged += this.ItemsSourceCollectionChangedEventHandler;
            }
        }

        private void ItemsSourceCollectionChangedEventHandler (object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems is not null)
            {
                this.AssociatedObject.ScrollIntoView (e.NewItems[^1]);
            }
        }
    }
}
