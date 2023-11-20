using Microsoft.AspNetCore.Components;
using Kenova.Client.Localization;
using System.Reflection;
using Kenova.Client.Authorization;
using Microsoft.JSInterop;

namespace Kenova.Client.Components
{
    public partial class PortalApp : KenovaComponentBase, IAsyncDisposable
    {
        private bool _initialized = false;
        private string StartingCaption;

        /// <summary>
        /// Gets or sets the assembly that should be searched for components matching the URI.
        /// </summary>
        [Parameter, EditorRequired] public Assembly AppAssembly { get; set; }

        /// <summary>
        /// Gets or sets a collection of additional assemblies that should be searched for components
        /// that can match URIs.
        /// </summary>
        [Parameter, EditorRequired] public IEnumerable<Assembly> AdditionalAssemblies { get; set; }

        protected override async Task OnInitializedAsync()
        {
            // The localizer is not loaded yet. Retrieve translation caption from JS
            StartingCaption = await JSRuntime.InvokeAsync<string>("KNGetStartingCaption");
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            if (firstRender)
            {
                var services = KenovaClientConfig.ServiceProvider;

                //foreach (var service in services)
                //{
                //    Trace.WriteLine($"Service: {service}");
                //}

                KenovaClientConfig.JSRuntime = JSRuntime;
                KenovaClientConfig.AuthenticationStateProvider = services.GetService(typeof(KenovaAuthenticationStateProvider)) as KenovaAuthenticationStateProvider;
                KenovaClientConfig.Localizer = services.GetService(typeof(KenovaLocalizer)) as KenovaLocalizer;

                if (KenovaClientConfig.JSRuntime == null)
                    throw new InvalidOperationException("Getting service IJSRuntime failed.");

                if (KenovaClientConfig.AuthenticationStateProvider == null)
                    throw new InvalidOperationException("Getting service AuthenticationStateProvider failed.");

                if (KenovaClientConfig.Localizer == null)
                    throw new InvalidOperationException("Getting service KenovaLocalizer failed.");

                await Startup.SetupPortalAsync();

                //await Portal.Modules.LoadModules();

                _initialized = true;
                this.StateHasChanged();
            }

        }
        public async ValueTask DisposeAsync()
        {
            //if (Portal.Modules is not null)
            //{
            //    await Portal.Modules.UnloadModules();
            //}
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }
    }
}