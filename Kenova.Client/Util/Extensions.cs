using System;
using System.Globalization;

namespace Kenova.Client
{
    public static class Extensions
    {
        private static IFormatProvider inv = CultureInfo.InvariantCulture.NumberFormat;

        /// <summary>
        /// Converts a double to a 1 decimal string with a '.' as decimal separator and the letters "px" at the end of the string.
        /// For example 170.3234234 is returned as "170.3px"
        /// </summary>
        public static string ToPixels(this double dbl)
        {
            if (dbl == 0.00d)
            {
                return "100%";
            }

            return String.Format(inv, "{0:0.0}px", dbl);
        }

    }
}
