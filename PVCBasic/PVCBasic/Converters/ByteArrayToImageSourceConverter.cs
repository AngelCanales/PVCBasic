using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace PVCBasic.Converters
{
    public class ByteArrayToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ImageSource imgSource = null;
            if (value != null && !(value.ToString() == "0"))
            {
                byte[] imageAsBytes = (byte[])value;
                imgSource = ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
            }
            return imgSource;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
