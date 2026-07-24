using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace AvaloniaEjercicios.ViewModels;

public class ErrorToBrushConverter : IValueConverter
{
    public static readonly ErrorToBrushConverter Instance = new();

    private static readonly IBrush ErrorBrush = new SolidColorBrush(Color.Parse("#FEE2E2"));
    private static readonly IBrush NormalBrush = new SolidColorBrush(Color.Parse("#F1F5F9"));

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is true ? ErrorBrush : NormalBrush;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
