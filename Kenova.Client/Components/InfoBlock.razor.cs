using Microsoft.AspNetCore.Components;

namespace Kenova.Client.Components
{
    public partial class InfoBlock : KenovaComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        //public string Text { get; set; }

        [Parameter]
        public InfoKind Kind { get; set; } = InfoKind.Information;


    }

    public enum InfoKind
    {
        Information,
        Warning,
        Error
    }

}
