using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using EclipsePlugInRunner.Data;

namespace EclipsePlugInRunner
{
    internal class PlanningItemsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<PlanningItem>)
            {
                var planningItems = (IEnumerable<PlanningItem>)value;
                return string.Join(", ", planningItems.Select(p => p.Id));
            };

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
