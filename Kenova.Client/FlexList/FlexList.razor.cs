using Kenova.Client.Util;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kenova.Client.Components
{
    // <button @onclick="btnClick" @onclick:stopPropagation>

    public partial class FlexList<ItemType> : KenovaComponentBase, IAsyncDisposable, IKenovaComponent, IRerender
    {
        [CascadingParameter]
        public KenovaDialogBase LayerComponent { get; set; }

        [Parameter]
        public string AdditionalStyle { get; set; } = null;

        [Parameter]
        public double Width { get; set; } = -1;

        [Parameter]
        public IList<ItemType> Items { get; set; }

        [Parameter]
        public RenderFragment<ItemType> ItemTemplate { get; set; }

        [Parameter]
        public string FocusID { get; set; }

        [Parameter]
        public bool AutoFocus { get; set; } = false;

        [Parameter]
        public int AutoFocusPriority { get; set; } = 100;

        [Parameter]
        public EventCallback<ItemType> RowClicked { get; set; }

        [Parameter]
        public EventCallback<ItemType> RowDoubleClicked { get; set; }

        private StringBuilder _container_style = new StringBuilder(100);
        private readonly string _container_id = KenovaClientConfig.GetUniqueElementID();
        private ComponentWingman<FlexList<ItemType>> _wingman = new();

        protected override void OnInitialized()
        {
            if (LayerComponent == null)
                throw new InvalidOperationException($"The {this.GetType().Name} component must be placed inside a LayerBaseComponent");

            LayerComponent.RegisterComponent(this);

        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            await _wingman.InvokeVoidAsync("Stop");
            await _wingman.DisposeAsync();

            LayerComponent.UnregisterComponent(this);
        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await this.Mutex.WaitAsync();

                if (firstRender)
                {
                    await _wingman.InstantiateAsync(this,"FlexListComponent");

                    await _wingman.InvokeVoidAsync("Start", _container_id);
                }

                await _wingman.InvokeVoidAsync("OnAfterRender");

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

        public void Rerender()
        {
            this.StateHasChanged();
        }

        private void OnRowClick(ItemType item)
        {
            _ = this.RowClicked.InvokeAsync(item);
        }

        private void OnRowDoubleClick(ItemType item)
        {
            _ = this.RowDoubleClicked.InvokeAsync(item);
        }

        [JSInvokable]
        public void OnEnterPressed(int index)
        {
            _ = this.RowClicked.InvokeAsync(this.Items[index]);
        }

        [JSInvokable]
        public void OnSpacePressed(int index)
        {
            _ = this.RowClicked.InvokeAsync(this.Items[index]);
        }


        public ValueTask SetFocusAsync()
        {
            return _wingman.InvokeVoidAsync("SetFocus");
        }

        async ValueTask<bool> IKenovaComponent.PerformFocusAsync(string focusID)
        {
            if (this.FocusID != null && focusID == this.FocusID)
            {
                await SetFocusAsync();
                return true;
            }

            return false;
        }

        ValueTask IKenovaComponent.PerformAutoFocusAsync()
        {
            return this.SetFocusAsync();
        }

        ValueTask<int> IKenovaComponent.MeasureAutoFocusPriorityAsync()
        {
            if (this.AutoFocus)
            {
                return ValueTask.FromResult(this.AutoFocusPriority);
            }

            return ValueTask.FromResult(-1);
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
            return JavaScriptCaller.KNElementHiddenAsync(_container_id);
        }

        ValueTask<bool> IKenovaComponent.IsChildOfAsync(string parent_id)
        {
            return JavaScriptCaller.KNIsChildOfAsync(parent_id, _container_id);
        }

    }
}
