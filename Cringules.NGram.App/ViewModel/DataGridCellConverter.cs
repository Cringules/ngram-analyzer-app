using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using Cringules.NGram.App.Resources;

namespace Cringules.NGram.App.ViewModel;

public class DataGridCellConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException(Strings.ConvertingValueToCellNotSupportedError);
    }

    public object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not DataGridCellInfo cell)
        {
            return null;
        }
        
        return (cell.Column.GetCellContent(cell.Item) as TextBlock)?.Text;
    }
}
