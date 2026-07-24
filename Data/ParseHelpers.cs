using System;
using System.Globalization;
using System.Linq;

namespace AvaloniaEjercicios.Data;

/// <summary>
/// Small parsing helpers shared by every exercise. All numeric input uses
/// InvariantCulture with '.' as decimal separator.
/// </summary>
internal static class ParseHelpers
{
    public static double D(string s, string fieldName = "valor")
    {
        if (string.IsNullOrWhiteSpace(s))
            throw new FormatException($"El campo '{fieldName}' está vacío.");
        if (!double.TryParse(s.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var v))
            throw new FormatException($"'{s}' no es un número válido para '{fieldName}'.");
        return v;
    }

    public static int I(string s, string fieldName = "valor")
    {
        if (string.IsNullOrWhiteSpace(s))
            throw new FormatException($"El campo '{fieldName}' está vacío.");
        if (!int.TryParse(s.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var v))
            throw new FormatException($"'{s}' no es un número entero válido para '{fieldName}'.");
        return v;
    }

    /// <summary>Parses a comma/semicolon separated list of doubles.</summary>
    public static double[] DList(string s, string fieldName = "lista")
    {
        if (string.IsNullOrWhiteSpace(s))
            throw new FormatException($"El campo '{fieldName}' está vacío.");
        return s.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .Where(x => x.Length > 0)
            .Select(x => D(x, fieldName))
            .ToArray();
    }

    public static string Money(double v) => v.ToString("N2", CultureInfo.InvariantCulture);

    public static string Num(double v) => v.ToString("0.####", CultureInfo.InvariantCulture);
}
