using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Kenova.Client.Components
{
    public partial class LinkButton : KenovaComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public EventCallback OnClick { get; set; }

        [Parameter]
        public bool TabStop { get; set; } = true;

        //[Parameter]
        //public EventCallback OnClick { get; set; }

        [Parameter]
        public bool Enabled { get; set; } = true;

        private void Item_Div_Clicked(MouseEventArgs e)
        {
            if (!Enabled)
                return;

            _ = OnClick.InvokeAsync();
        }

    }
}
