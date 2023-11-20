using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Kenova.Client.Components
{
    public partial class TabPage : KenovaComponentBase
    {
        [CascadingParameter]
        private TabComponent Parent { get; set; }

        private TabItem _tabItem;

        [Parameter]
        public string Identifier { get; set; }

        [Parameter]
        public bool KeepAlive { get; set; } = true;

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        protected override void OnInitialized()
        {
            if (Parent == null)
                throw new ArgumentNullException(nameof(Parent), "TabPage must exist within a TabComponent");

            _tabItem = Parent.GetTabItem(this.Identifier);
        }

    }
}