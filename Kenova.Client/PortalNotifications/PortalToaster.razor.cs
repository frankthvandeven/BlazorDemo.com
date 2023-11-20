using Kenova.Client.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Kenova.Client.SystemComponents
{
    public partial class PortalToaster : KenovaComponentBase, IRerender
    {
        [Parameter]
        public List<NotificationItem> Items { get; set; }

        public void Rerender()
        {
            this.StateHasChanged();
        }

        protected override void OnInitialized()
        {
            if (Items == null)
                throw new InvalidOperationException("Parameter Items not set");
        }


    }
}
