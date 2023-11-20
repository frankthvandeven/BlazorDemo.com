using Kenova.Client.Util;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;

namespace Kenova.Client.Components
{
    public partial class PortalRoot : KenovaComponentBase, IAsyncDisposable
    {
        //private static int _instance_count = 0;

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        //private readonly string PortalID = Guid.NewGuid().ToString("N");

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool ShowPortalTopbar { get; set; } = true;

        [Parameter]
        public bool ShowPortalMenu { get; set; } = true;

        [Parameter]
        public bool ShowPortalBreadcrumb { get; set; } = true;

        private ComponentWingman<PortalRoot> _wingman = new();

        protected override void OnInitialized()
        {
            if (KenovaClientConfig.Diagnostics) Console.WriteLine("PORTALROOT INIT PORTALROOT INIT PORTALROOT INIT PORTALROOT INIT PORTALROOT INIT PORTALROOT INIT");

            //_instance_count++;

            //if (_instance_count > 1)
            //    throw new ApplicationException($"The Portal component was instantiated {_instance_count} times. Kenova only allows a single instance.");

            Portal.RefreshWasCalled += Portal_RefreshWasCalled;

            // TEST
            //Console.WriteLine($"URI: {NavigationManager.Uri}");
            //Console.WriteLine($"BaseRelative: {NavigationManager.ToBaseRelativePath(NavigationManager.Uri)}");
            //Console.WriteLine($"ESCAPED: {Uri.EscapeDataString(NavigationManager.Uri)}");

            //NavigationManager.PortalNavigateTo("/auth/login");
            //NavigationManager.PortalNavigateTo($"auth/login?returnUrl={Uri.EscapeDataString(NavigationManager.Uri)}");

        }

        private void Portal_RefreshWasCalled()
        {
            this.StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (KenovaClientConfig.Diagnostics) Console.WriteLine("PORTALROOT OnAfterRenderAsync PORTALROOT OnAfterRenderAsync");

            try
            {
                await this.Mutex.WaitAsync();

                if (firstRender)
                {
                    await _wingman.InstantiateAsync(this, "PortalRootComponent");
                    await _wingman.InvokeVoidAsync("Start");
                }
            }
            finally
            {
                this.Mutex.Release();
            }

        }

        public async ValueTask DisposeAsync()
        {
            if (KenovaClientConfig.Diagnostics) Console.WriteLine("💀💀💀 📈 PortalRoot - DisposeAsync - BEGIN");

            try
            {
                await this.Mutex.WaitAsync();

                Portal.RefreshWasCalled -= Portal_RefreshWasCalled;

                //_instance_count--;

                await _wingman.InvokeVoidAsync("Stop");

                await _wingman.DisposeAsync();
            }
            finally
            {
                this.Mutex.Release();
            }

            if (KenovaClientConfig.Diagnostics) Console.WriteLine("💀💀💀 📈 PortalRoot - DisposeAsync - END");
        }

        [JSInvokable]
        public async ValueTask OnEscapePressed()
        {
            if (LayerManager.LayerStack.Count > 0)
            {
                await LayerManager.LayerStack.Peek().ComponentReference.PerformCancelAsync();
                //LayerManager.CloseTopmost();
            }
            else
            {
                Portal.ActivatePortalMenu();
            }
        }


        private RenderFragment LaunchComponent(LayerDefinition layer_definition)
        {
            if (layer_definition.ComponentType == null)
                throw new ArgumentNullException("modal_data.ComponentType");

            var fragment = new RenderFragment(builder =>
            {
                var i = 0;
                builder.OpenComponent(i++, layer_definition.ComponentType);
                builder.AddAttribute(i++, nameof(LayerDefinition), layer_definition);
                foreach (KeyValuePair<string, object> parameter in layer_definition.Attributes)
                {
                    builder.AddAttribute(i++, parameter.Key, parameter.Value);
                }
                builder.SetKey(layer_definition.Key);
                builder.AddComponentReferenceCapture(i++, inst => { layer_definition.ComponentReference = (KenovaDialogBase)inst; }); // call right before closecomponent and not before attibutes!
                builder.CloseComponent();
            });

            return fragment;
        }

    }


}
