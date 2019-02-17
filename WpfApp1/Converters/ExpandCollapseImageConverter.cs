using FontAwesome.WPF;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace WpfApp1.Converters
{
    public class ExpandCollapseImageConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            // ChevronUp and ChevronDown are the IDs of the font awesome icons
            var chevronUp = values[0] as Enum;
            var chevronDown = values[1] as Enum;

            var isChecked = values[2] as bool?;
            var droppedDown = isChecked ?? false;

            if (droppedDown)
                return chevronUp;
            else
                return chevronDown;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
