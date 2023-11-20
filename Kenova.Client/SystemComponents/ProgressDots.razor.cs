using Microsoft.AspNetCore.Components;

namespace Kenova.Client.SystemComponents
{
    public partial class ProgressDots : KenovaComponentBase
    {
        [Parameter]
        public string Caption { get; set; } = null;
    }
}
