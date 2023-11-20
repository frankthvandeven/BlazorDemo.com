using Microsoft.AspNetCore.Components;

namespace Kenova.Client.Components
{
    public partial class DirectIcon : KenovaComponentBase
    {

        [Parameter]
        public bool Enabled { get; set; } = true;

        [Parameter]
        public IconSize Size { get; set; } = IconSize.Regular;

        [Parameter]
        public IconKind Kind { get; set; } = IconKind.FontAwesome;

        [Parameter]
        public string Data { get; set; } = "fal fa-square-full";

        /// <summary>
        /// Html color string in the format #RRGGBB (7 characters).
        /// Value null means that the color is not specified.
        /// </summary>
        public string HtmlColor { get; set; } = null;




    }

}
