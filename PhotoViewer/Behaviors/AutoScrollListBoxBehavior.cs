using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;

namespace PhotoViewer.Behaviors
{
    /// <summary>
    /// A behavior that causes the selected item to scroll into view when it changes.
    /// </summary>
    public class AutoScrollListBoxBehavior : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += OnListBoxSelectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.SelectionChanged -= OnListBoxSelectionChanged;
        }

        private void OnListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox listBox)
            {
                if (listBox.SelectedItem != null)
                {
                    listBox.ScrollIntoView(listBox.SelectedItem);
                }
            }
        }
    }
}
