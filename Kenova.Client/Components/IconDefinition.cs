using Kenova.Client.Util;
using System.Drawing;

namespace Kenova.Client.Components
{
    public class IconDefinition
    {
        public IconKind IconKind { get; set; } = IconKind.FontAwesome;

        public string IconData { get; set; } = "fal fa-square-full";

        /// <summary>
        /// Html color string in the format #RRGGBB (7 characters).
        /// Value null means that the color is not specified.
        /// </summary>
        public string HtmlColor { get; set; } = null;

        /// <summary>
        /// https://developer.mozilla.org/en-US/docs/Web/CSS/color_value
        /// http://www.flounder.com/csharp_color_table.htm
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            HtmlColor = ColorHelper.ColorToHtml(color);
        }

        public void SetColor(KnownColor known_color)
        {
            Color c = Color.FromKnownColor(known_color);
            HtmlColor = ColorHelper.ColorToHtml(c);
        }

        public void SetColor(int alpha, int red, int green, int blue)
        {
            Color c = Color.FromArgb(alpha, red, green, blue);
            HtmlColor = ColorHelper.ColorToHtml(c);
        }

        public void SetColor(int red, int green, int blue)
        {
            Color c = Color.FromArgb(red, green, blue);
            HtmlColor = ColorHelper.ColorToHtml(c);
        }


    }

    public enum IconKind
    {
        None,
        FontAwesome,
        Vector
    }

}
