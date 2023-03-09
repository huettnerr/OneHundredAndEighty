using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace OneHundredAndEighty.Controls
{
    #region Markup-Extension

    public abstract class ConverterMarkupExtension<T> : MarkupExtension, IValueConverter where T : class, new()
    {
        private static T _converter = null;

        public ConverterMarkupExtension()
        {
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new T());
        }

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }

    public abstract class MultiConverterMarkupExtension<T> : MarkupExtension, IMultiValueConverter where T : class, new()
    {
        private static T _converter = null;

        public MultiConverterMarkupExtension()
        {
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new T());
        }

        public abstract object Convert(object[] value, Type targetType, object parameter, CultureInfo culture);
        public abstract object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture);
    }

    #endregion

    #region InverseBoolean

    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBooleanConverter : ConverterMarkupExtension<InverseBooleanConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    #endregion

    #region Multi-Boolean

    public class BooleanANDMultiConverter : MultiConverterMarkupExtension<BooleanANDMultiConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return values.OfType<IConvertible>().All(System.Convert.ToBoolean);
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class BooleanORMultiConverter : MultiConverterMarkupExtension<BooleanORMultiConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return values.OfType<IConvertible>().Any(System.Convert.ToBoolean);
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    #endregion

    #region BoolToVisibility

    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : ConverterMarkupExtension<BoolToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(Visibility) && !(value is bool))
                throw new InvalidOperationException("The target must be a boolean");

            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public override object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool) && !(value is Visibility))
                throw new InvalidOperationException("The target must be a bool");

            return (value.Equals(Visibility.Visible)) ? true : false;
        }
    }

    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class InverseBoolToVisibilityConverter : ConverterMarkupExtension<InverseBoolToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(Visibility) && !(value is bool))
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public override object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool) && !(value is Visibility))
                throw new InvalidOperationException("The target must be a bool");

            return (value.Equals(Visibility.Visible)) ? false : true;
        }
    }

    #endregion

    #region TimespanToDouble

    [ValueConversion(typeof(TimeSpan), typeof(double))]
    public class TimespanToDoubleMillisecondsConverter : ConverterMarkupExtension<TimespanToDoubleMillisecondsConverter>
    {
        public override object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(double) && !(value is TimeSpan))
                throw new InvalidOperationException("The target must be a double");

            return ((TimeSpan)value).TotalMilliseconds;
        }

        public override object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(TimeSpan) && !(value is double))
                throw new InvalidOperationException("The target must be a TimeSpan");

            return TimeSpan.FromMilliseconds((double)value);
        }
    }

    [ValueConversion(typeof(TimeSpan), typeof(double))]
    public class TimespanToDoubleSecondsConverter : ConverterMarkupExtension<TimespanToDoubleSecondsConverter>
    {
        public override object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(double) && !(value is TimeSpan))
                throw new InvalidOperationException("The target must be a double");

            return ((TimeSpan)value).TotalSeconds;
        }

        public override object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(TimeSpan) && !(value is double))
                throw new InvalidOperationException("The target must be a TimeSpan");

            return TimeSpan.FromSeconds((double)value);
        }
    }

    #endregion

    #region DoubleToPercent

    [ValueConversion(typeof(double), typeof(double))]
    public class DoubleToPercentConverter : ConverterMarkupExtension<DoubleToPercentConverter>
    {
        public override object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(double) && !(value is double))
                throw new InvalidOperationException("The target must be a double");

            return (double)value * 100.0;
        }

        public override object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(double) && !(value is double))
                throw new InvalidOperationException("The target must be a double");

            return (double)value / 100.0;
        }
    }

    #endregion

    #region RadToDegree

    [ValueConversion(typeof(double), typeof(double))]
    public class RadToDegreeConverter : ConverterMarkupExtension<RadToDegreeConverter>
    {
        public override object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(double) && !(value is double))
                throw new InvalidOperationException("The target must be a double");

            return (double)value / Math.PI * 180.0;
        }

        public override object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(double) && !(value is double))
                throw new InvalidOperationException("The target must be a double");

            return (double)value / 180.0 * Math.PI;
        }
    }

    #endregion

    #region StatusToColor

    [ValueConversion(typeof(StatusEnum), typeof(SolidColorBrush))]
    public class StatusToColorConverter : ConverterMarkupExtension<StatusToColorConverter>
    {
        public enum StatusEnum { Initial, Active, ResultOk, ResultNotOk }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is StatusEnum && value != null)
            {
                StatusEnum status = (StatusEnum)value;
                var color = new SolidColorBrush(Colors.White);

                switch (status)
                {
                    case StatusEnum.Active:
                        color = new SolidColorBrush(Colors.Beige);
                        //color = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffaacc");
                        break;
                    case StatusEnum.ResultOk:
                        color = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF1B8D0B");
                        break;
                    case StatusEnum.ResultNotOk:
                        color = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFB90E0E");
                        break;
                    default:
                        color = new SolidColorBrush(Colors.Transparent);
                        break;
                }

                return color;
            }

            return Binding.DoNothing;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
