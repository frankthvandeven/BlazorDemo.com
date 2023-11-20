using Microsoft.AspNetCore.Components;
using System;

namespace Kenova.Client.Components
{
    public partial class MenuPage : KenovaComponentBase, IDisposable
    {

        [CascadingParameter]
        private MainLayout Parent { get; set; }

        private MenuItem _menuItem;

        [Parameter]
        public string Identifier { get; set; } = null;

        [Parameter]
        public bool KeepAlive { get; set; } = false;

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        protected override void OnInitialized()
        {
            if (Parent == null)
                throw new ArgumentNullException(nameof(Parent), "A MenuPage must exist within a MainLayout component");

            _menuItem = Parent.GetMenuItemForIdentifier(this.Identifier);

        }

        public void Dispose()
        {
        }
    }
}