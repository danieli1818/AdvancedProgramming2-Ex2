using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

/// <summary>
/// FlightSimulator.Converters Namespace of all Converters.
/// </summary>
namespace FlightSimulator.Converters
{
    /// <summary>
    /// StringToBrushConverter Class is the Class of the Converter
    /// from String To Brush if it's LightPink it returns the LightPink Brush
    /// else White.
    /// It implements IValueConverter.
    /// </summary>
    class StringToBrushConverter : IValueConverter
    {
        /// <summary>
        /// Convert Function From Converting From String to Brush.
        /// </summary>
        /// <param name="value">object value the String Value.</param>
        /// <param name="targetType">Type targetType of to what to convert to it must be Brush</param>
        /// <param name="parameter">object parameter parameter for the function. Not using it.</param>
        /// <param name="culture">CultureInfo culture. Not using it.</param>
        /// <returns>The Brush which fits the String value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
            {
                throw new InvalidOperationException("Target Type Must be Brush!!!!");
            }
            string colorStr = value as string;
            if (colorStr == null)
            {
                throw new InvalidOperationException("Value Type Must be String!!!!");
            }
            if (colorStr.Equals("LightPink"))
            {
                return Brushes.LightPink;
            }
            return Brushes.White;
        }

        /// <summary>
        /// ConvertBack function to convert back from Brush to String.
        /// Not Using it therefore it is not implemented.
        /// </summary>
        /// <param name="value">object value Brush value to convert to String.</param>
        /// <param name="targetType">Type targetType , the target type must be String.</param>
        /// <param name="parameter">object parameter of the function.</param>
        /// <param name="culture">CultureInfo culture.</param>
        /// <returns>String which fits the Brush value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
