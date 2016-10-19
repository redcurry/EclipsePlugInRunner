using System;
using System.Globalization;
using System.Windows.Data;
using EclipsePlugInRunner.Data;

namespace EclipsePlugInRunner.Views
{
    public class ActivePlanSetupConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "(No active plan.)";
            }
            else if (value is PlanningItem)
            {
                return ((PlanningItem)value).Id;
            }

            return "(Not a plan.)";    // This should never happen
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}