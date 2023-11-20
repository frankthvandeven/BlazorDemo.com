using Kenova.Client.Util;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Kenova.Client.Components
{

    public partial class ButtonBar : KenovaComponentBase, IRerender, IAsyncDisposable
    {
        private string container_id = KenovaClientConfig.GetUniqueElementID();

        private List<Button> Buttons { get; set; } = new();

        private ComponentWingman<ButtonBar> _wingman = new();

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public async ValueTask DisposeAsync()
        {
            try
            {
                await this.Mutex.WaitAsync();

                await _wingman.InvokeVoidAsync("Stop");
                await _wingman.DisposeAsync();
            }
            finally
            {
                this.Mutex.Release();
            }
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
            }
            finally
            {
                this.Mutex.Release();
            }
        }

        internal void Register(Button button)
        {
            this.Buttons.Add(button);
        }

        internal void Unregister(Button button)
        {
            this.Buttons.Remove(button);
        }

        public void Rerender()
        {
            this.StateHasChanged();
        }

        [JSInvokable]
        public void OnEnterPressed(int index)
        {
            if (this.Buttons[index].Enabled)
            {
                _ = this.Buttons[index].ButtonClicked.InvokeAsync();
            }

        }

        [JSInvokable]
        public void OnSpacePressed(int index)
        {
            if (this.Buttons[index].Enabled)
            {
                _ = this.Buttons[index].ButtonClicked.InvokeAsync();
            }
        }

    }
}