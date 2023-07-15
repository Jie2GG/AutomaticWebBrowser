using System.Reflection;
using System.Windows.Controls;

using HandyControl.Controls;

using Microsoft.Xaml.Behaviors;

namespace AutomaticWebBrowser.Common
{
    class PropertyGridBehavior : Behavior<PropertyGrid>
    {
        protected override void OnAttached ()
        {
            FieldInfo? itemsControlField = this.AssociatedType.GetField ("_itemsControl", BindingFlags.NonPublic | BindingFlags.Instance);
            this.AssociatedObject.SelectedObjectChanged += (sender, e) =>
            {
                if (this.AssociatedObject.SelectedObject is null && itemsControlField is not null)
                {
                    if (itemsControlField.GetValue (this.AssociatedObject) is ItemsControl itemsControl)
                    {
                        itemsControl.ItemsSource = null;
                    }
                }
            };

        }
    }
}
