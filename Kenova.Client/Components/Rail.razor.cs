using Microsoft.AspNetCore.Components;
using System.Text;

namespace Kenova.Client.Components
{
    public partial class Rail : KenovaComponentBase
    {
        private StringBuilder _container_style = new StringBuilder(100);

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public RailAlign Align { get; set; } = RailAlign.Left;

        [Parameter]
        public RailItemsAlign AlignItems { get; set; } = RailItemsAlign.Top;

    }


    public enum RailAlign
    {
        Left,
        Center,
        Right
    }

    public enum RailItemsAlign
    {
        Top,
        Center,
        Bottom
    }

}
