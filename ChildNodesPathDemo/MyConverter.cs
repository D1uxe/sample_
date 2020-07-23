using DevExpress.Xpf.Grid;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ChildNodesPathDemo
{
    [ValueConversion(typeof(TreeListNode), typeof(string))]
    class MyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TreeListNode Node)) return null;
            return ((EplanNode)Node.Content).Executor ?? "NULL";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
