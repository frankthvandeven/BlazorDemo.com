using Kenova.Client.Util;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq.Expressions;

namespace Kenova.Client.Components
{
    public partial class CheckBox : KenovaComponentBase, IRerender, IAsyncDisposable
    {
        private string container_id = KenovaClientConfig.GetUniqueElementID();

        private ComponentWingman<CheckBox> _wingman = new();

        protected FieldLink<bool> CheckedFieldLink { get; private set; }

        /// <summary>
        /// For example "() => this.Model.CurrentCustomer"
        /// </summary>
        [Parameter, EditorRequired]
        public Expression<Func<bool>> CheckedExpression { get; set; }

        [Parameter]
        public bool Indeterminate { get; set; } = false;

        [Parameter]
        public string AdditionalStyle { get; set; } = null;

        protected override void OnInitialized()
        {
            if (LayerComponent == null)
                throw new InvalidOperationException("This component must be placed inside a LayerBaseComponent");

            LayerComponent.RegisterComponent(this);

            if (CheckedExpression == null)
                throw new Exception("Checked parameter must be set");

            CheckedFieldLink = new FieldLink<bool>(this, CheckedExpression, true, null, CheckedChanged);

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await this.Mutex.WaitAsync();

                if (firstRender)
                {
                    await _wingman.InstantiateAsync(this, "SingleButtonComponent");

                    await _wingman.InvokeVoidAsync("Start", container_id);
                    LayerComponent.RegisterFirstRenderComplete(this); // must be at the end of OnAfterRender
                }

            }
            finally
            {
                this.Mutex.Release();
            }
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            await _wingman.InvokeVoidAsync("Stop");
            await _wingman.DisposeAsync();

            CheckedFieldLink.Dispose();

            LayerComponent.UnregisterComponent(this);
        }


        public void Rerender()
        {
            this.StateHasChanged();
        }

    }
}

