using Kenova.Client.Components.Panels;
using Kenova.Client.Util;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;

namespace Kenova.Client.Components
{
    public partial class Toolbar : KenovaComponentBase, IAsyncDisposable
    {
        private string container_id = KenovaClientConfig.GetUniqueElementID();
        private string overflow_id = KenovaClientConfig.GetUniqueElementID();

        private ComponentWingman<Toolbar> _wingman = new();

        [CascadingParameter]
        public KenovaDialogBase LayerComponent { get; set; }

        [Parameter]
        public ToolbarItemCollection Buttons { get; set; } = null;

        protected override void OnInitialized()
        {
            if (LayerComponent == null)
                throw new InvalidOperationException($"The {this.GetType().Name} component must be placed inside a LayerBaseComponent");

            LayerComponent.RegisterComponent(this);

            if (Buttons == null)
                throw new InvalidOperationException("Button parameter cannot be null");

        }

        protected override void OnParametersSet()
        {
            if (Buttons.Count == 0)
                throw new InvalidOperationException("There must be at least one toolbar item");
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await _wingman.InstantiateAsync(this,"ToolbarComponent");

                await _wingman.InvokeVoidAsync("Start", container_id);
                LayerComponent.RegisterFirstRenderComplete(this); // must be at the end of OnAfterRender
            }

        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            await _wingman.InvokeVoidAsync("Stop");

            await _wingman.DisposeAsync();

            LayerComponent.UnregisterComponent(this);
        }

        private bool DropdownOpen = false;
        private LayerDefinition<PanelDropdownMenu> _ld = null;


        [JSInvokable]
        public async ValueTask OnOverflowClicked(int visible_count)
        {
            if (this.DropdownOpen == true)
            {
                await _ld.CloseCancelAsync(); // Close the layer
                return;
            }

            var ddItems = new MenuItemCollection();

            int startIndex = visible_count;

            for (int index = startIndex; index < this.Buttons.Count; index++)
            {
                var item = this.Buttons[index];

                ddItems.Add(item.Caption, null, item.EnabledExpression, item.Icon.IconKind, item.Icon.IconData);
                ddItems.Icon.HtmlColor = item.Icon.HtmlColor;
                ddItems.Tag = item;
            }

            this.DropdownOpen = true;

            _ld = new LayerDefinition<PanelDropdownMenu>
            {
                Kind = LayerKind.Dropdown,
                OwnerID = overflow_id,
                AfterClosed = LayerDefinition_DropdownWasClosed,
                [i => i.MenuItems] = ddItems
            };

            _ld.Parameter(p => p.LayerDefinition, _ld); // not possible to refer to self in object initializer syntax

            await _ld.OpenNonBlockingAsync();

        }

        /// <summary>
        /// This method is called by LayerManager after closing the dropdown.
        /// A dropdown is closed by calling LayerManager.Close()
        /// When clicking outside the dropdown element and outside dropdown panel, the 
        /// LayerManager.Close() will be called automatically.
        /// </summary>
        private void LayerDefinition_DropdownWasClosed(LayerResult result)
        {

            this.DropdownOpen = false;

            _ld = null;

            if (result.Data == null)
            {
                this.StateHasChanged();
                return;
            }

            var menuItem = (MenuItem)result.Data;

            var toolbarItem = (ToolbarItem)menuItem.Tag;

            int index = this.Buttons.IndexOf(toolbarItem);

            processToolbarButtonClick(index);

            this.StateHasChanged();
        }

    }
}