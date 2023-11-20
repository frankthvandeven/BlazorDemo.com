using System;
using System.Drawing;

// https://github.com/dotnet/runtime/blob/6072e4d3a7a2a1493f514cdf4be75a3d56580e84/src/libraries/Common/src/System/Drawing/ColorTranslator.cs#L248-L252

namespace Kenova.Client.Util
{

    public static class ColorHelper
    {

        /// <summary>
        /// Translates the specified Color to a Html string color representation in the format #RRGGBB
        /// </summary>
        public static string ColorToHtml(Color c)
        {
            if (c.IsEmpty)
                return string.Empty;

            return "#" + c.R.ToString("X2", null) + c.G.ToString("X2", null) + c.B.ToString("X2", null);
        }

        /// <summary>
        /// Translates an Html color representation in the format #RRGGBB to a System.Drawing.Color.
        /// </summary>
        private static Color HtmlToColor(string htmlColor)
        {
            if ((htmlColor == null) || (htmlColor.Length != 7) || (htmlColor[0] != '#'))
                return Color.Empty;

            // #RRGGBB
            return Color.FromArgb(Convert.ToInt32(htmlColor.Substring(1, 2), 16),
                               Convert.ToInt32(htmlColor.Substring(3, 2), 16),
                               Convert.ToInt32(htmlColor.Substring(5, 2), 16));

        }

    }
}
