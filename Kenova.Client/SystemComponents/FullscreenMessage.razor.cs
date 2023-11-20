using Microsoft.AspNetCore.Components;

namespace Kenova.Client.SystemComponents
{
    public partial class FullscreenMessage : KenovaComponentBase
    {

        [Parameter]
        public string Message { get; set; }


    }
}