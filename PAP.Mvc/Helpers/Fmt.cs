using System;
using System.Globalization;

namespace PAP.Mvc.Helpers
{
    /// <summary>
    /// Formatos de fecha/texto usados por las vistas Index (siempre en cultura invariante
    /// para que los inputs date / datetime-local / number reciban valores validos).
    /// </summary>
    public static class Fmt
    {
        public static string Date(DateTime? value)
        {
            return value.HasValue ? value.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : string.Empty;
        }

        public static string DateTimeLocal(DateTime? value)
        {
            return value.HasValue ? value.Value.ToString("yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture) : string.Empty;
        }

        public static string Pretty(DateTime? value)
        {
            return value.HasValue ? value.Value.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture) : "-";
        }

        public static string Dash(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "-" : value;
        }

        public static string Truncate(string value, int max)
        {
            if (string.IsNullOrEmpty(value) || value.Length <= max)
                return value;
            return value.Substring(0, max) + "...";
        }
    }
}
