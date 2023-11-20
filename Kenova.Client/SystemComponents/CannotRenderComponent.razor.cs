using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Kenova.Client.SystemComponents
{
    public partial class CannotRenderComponent : KenovaComponentBase
    {

        [Parameter]
        public List<string> Messages { get; set; }

    }

}
