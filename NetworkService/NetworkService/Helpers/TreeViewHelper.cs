using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace NetworkService.Helpers
{
    public static class TreeViewHelper
    {
        public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.RegisterAttached(
            "SelectedItem",
            typeof(object),
            typeof(TreeViewHelper),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemChanged));

        public static object GetSelectedItem(TreeView treeView)
        {
            return treeView.GetValue(SelectedItemProperty);
        }

        public static void SetSelectedItem(TreeView treeView, object value)
        {
            treeView.SetValue(SelectedItemProperty, value);
        }

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var treeView = d as TreeView;
            if (treeView != null)
            {
                treeView.SelectedItemChanged += (sender, args) =>
                {
                    treeView.SetValue(SelectedItemProperty, args.NewValue);
                };
            }
        }
    }
}
