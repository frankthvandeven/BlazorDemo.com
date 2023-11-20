using Kenova.Client.Util;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;

namespace Kenova.Client.Components
{
    public partial class FeatureTiles : KenovaComponentBase, IAsyncDisposable, IKenovaComponent
    {
        private string container_id = KenovaClientConfig.GetUniqueElementID();

        private ComponentWingman<FeatureTiles> _wingman = new();

        [CascadingParameter]
        public KenovaDialogBase LayerComponent { get; set; }

        [Parameter]
        public MenuItemCollection MenuItems { get; set; }

        [Parameter]
        public EventCallback<MenuItem> TileClicked { get; set; }

        protected override void OnInitialized()
        {
            if (LayerComponent == null)
                throw new InvalidOperationException($"The {this.GetType().Name} component must be placed inside a LayerBaseComponent");

            LayerComponent.RegisterComponent(this);

        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (KenovaClientConfig.Diagnostics) Console.WriteLine("FEATURETILES DISPOSEASYNC");

            LayerComponent.UnregisterComponent(this);

            await _wingman.InvokeVoidAsync("Stop");
            await _wingman.DisposeAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await this.Mutex.WaitAsync();

                if (firstRender)
                {
                    await _wingman.InstantiateAsync(this, "ButtonGroupComponent");
                    await _wingman.InvokeVoidAsync("Start", container_id);
                }

                await _wingman.InvokeVoidAsync("CalculateTabindexZero");

                if (firstRender)
                {
                    LayerComponent.RegisterFirstRenderComplete(this); // must be at the end of OnAfterRender
                }

            }
            finally
            {
                this.Mutex.Release();
            }

        }

        private async void Tile_Div_ClickedAsync(MouseEventArgs e, MenuItem item)
        {
            await this.processClickAsync(item);
        }

        [JSInvokable]
        public async Task OnEnterPressed(int index)
        {
            var item = this.MenuItems[index];

            await this.processClickAsync(item);
        }

        [JSInvokable]
        public async Task OnSpacePressed(int index)
        {
            var item = this.MenuItems[index];
            
            await this.processClickAsync(item);
        }

        private async Task processClickAsync(MenuItem item)
        {
            if (item.EnabledExpression() == false)
                return;

            if (item.Identifier.StartsWith('/'))
            {
                await NavigationManager.PortalNavigateToAsync(item.Identifier);
            }
            else
            {
                await TileClicked.InvokeAsync(item);
            }

        }

        async ValueTask<bool> IKenovaComponent.PerformFocusAsync(string identifier)
        {
            for (int index = 0; index < this.MenuItems.Count; index++)
            {
                MenuItem item = this.MenuItems[index];

                if (item.Identifier != null && item.Identifier == identifier)
                {
                    await _wingman.InvokeVoidAsync("FocusByIndex", index);
                    return true;
                }
            }

            return false;
        }

        async ValueTask IKenovaComponent.PerformAutoFocusAsync()
        {
            if (_autofocus_item == null)
                return;

            int index = this.MenuItems.IndexOf(_autofocus_item);

            if (index == -1)
                return;

            await _wingman.InvokeVoidAsync("FocusByIndex", index);

        }

        private MenuItem _autofocus_item = null;

        ValueTask<int> IKenovaComponent.MeasureAutoFocusPriorityAsync()
        {
            foreach (var item in this.MenuItems)
            {
                if (item.AutoFocus && item.EnabledExpression())
                {
                    _autofocus_item = item;
                    return ValueTask.FromResult<int>(this.MenuItems.AutoFocusPriority);
                }
            }

            return ValueTask.FromResult<int>(-1);
        }

        ValueTask<bool> IKenovaComponent.PerformEnterPressedAsync()
        {
            return ValueTask.FromResult(false);
        }

        ValueTask<bool> IKenovaComponent.PerformEscapePressedAsync()
        {
            return ValueTask.FromResult(false);
        }

        ValueTask<bool> IKenovaComponent.ComponentHiddenAsync()
        {
            return JavaScriptCaller.KNElementHiddenAsync(container_id);
        }

        ValueTask<bool> IKenovaComponent.IsChildOfAsync(string parent_id)
        {
            return JavaScriptCaller.KNIsChildOfAsync(parent_id, container_id);
        }

    }
}

