using Kenova.Client.Util;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Kenova.Client.Components
{

    public partial class OptionGroup : KenovaComponentBase, IRerender, IAsyncDisposable, IKenovaComponent
    {
        private string container_id = KenovaClientConfig.GetUniqueElementID();
        private ComponentWingman<OptionGroup> _wingman = new();
        private List<OptionButton> Buttons { get; set; } = new();

        internal FieldLink<string> SelectedFieldLink;

        [CascadingParameter]
        public KenovaDialogBase LayerComponent { get; set; }

        /// <summary>
        /// For example "() => this.Model.CurrentCustomer"
        /// </summary>
        [Parameter]
        public Expression<Func<string>> SelectedExpression { get; set; }

        [Parameter]
        public string FocusID { get; set; }

        [Parameter]
        public bool AutoFocus { get; set; } = false;

        [Parameter]
        public int AutoFocusPriority { get; set; } = 100;


        protected override void OnInitialized()
        {
            if (LayerComponent == null)
                throw new InvalidOperationException($"The {this.GetType().Name} component must be placed inside a LayerBaseComponent");

            LayerComponent.RegisterComponent(this);

            if (SelectedExpression == null)
                throw new Exception("SelectedExpression parameter must be set");

            SelectedFieldLink = new FieldLink<string>(this, SelectedExpression, true, null, SelectedIdentifierChanged);

        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            try
            {
                await this.Mutex.WaitAsync();

                LayerComponent.UnregisterComponent(this);

                await _wingman.InvokeVoidAsync("Stop");
                await _wingman.DisposeAsync();

                SelectedFieldLink.Dispose();
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

        internal void Register(OptionButton button)
        {
            this.Buttons.Add(button);
        }

        internal void Unregister(OptionButton button)
        {
            this.Buttons.Remove(button);
        }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public EventCallback<string> SelectedIdentifierChanged { get; set; }

        [JSInvokable]
        public void OnEnterPressed(int index)
        {
            if (this.Buttons[index].Enabled)
            {
                this.SelectedFieldLink.Value = this.Buttons[index].Identifier;
                this.StateHasChanged();
            }
        }

        [JSInvokable]
        public void OnSpacePressed(int index)
        {
            if (this.Buttons[index].Enabled)
            {
                this.SelectedFieldLink.Value = this.Buttons[index].Identifier;
                this.StateHasChanged();
            }
        }

        private OptionButton findSelectedButton()
        {
            if (this.SelectedFieldLink.Value == null)
                return null;

            foreach (var button in this.Buttons)
            {
                if (button.Identifier == this.SelectedFieldLink.Value)
                    return button;
            }

            return null;
        }

        async ValueTask<bool> IKenovaComponent.PerformFocusAsync(string focusID)
        {
            if (this.FocusID != focusID)
                return false;

            var button = findSelectedButton();

            if (button != null)
            {
                await JavaScriptCaller.KNFocusAsync(button.ContainerID);
            }
            else
            {
                await JavaScriptCaller.KNFocusAsync(this.Buttons[0].ContainerID);
            }

            return true;
        }

        async ValueTask IKenovaComponent.PerformAutoFocusAsync()
        {
            var button = findSelectedButton();

            if (button != null)
            {
                await JavaScriptCaller.KNFocusAsync(button.ContainerID);
            }
            else
            {
                await JavaScriptCaller.KNFocusAsync(this.Buttons[0].ContainerID);
            }

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
            return JavaScriptCaller.KNElementHiddenAsync(container_id);
        }

        ValueTask<bool> IKenovaComponent.IsChildOfAsync(string parent_id)
        {
            return JavaScriptCaller.KNIsChildOfAsync(parent_id, container_id);
        }

    }
}

