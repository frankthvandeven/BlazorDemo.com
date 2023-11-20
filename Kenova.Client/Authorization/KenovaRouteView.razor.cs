using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Kenova.Client.Authorization
{
    public partial class KenovaRouteView : KenovaComponentBase
    {
        private static bool RefreshTokenMode = true;

        // AttributeDataCache.cs is the base of page authorization

        /// <summary>
        /// Gets or sets the route data. This determines the page that will be
        /// displayed and the parameter values that will be supplied to the page.
        /// </summary>
        [Parameter]
        public RouteData RouteData { get; set; }

        protected override void OnParametersSet()
        {
            if (KenovaClientConfig.Diagnostics) Console.WriteLine("🛤 <KenovaRouteView> - OnParametersSet - ENTER");

            if (RouteData == null)
                throw new InvalidOperationException("RouteData parameter must be set");

            if (RouteData.PageType.IsSubclassOf(typeof(KenovaDialogBase)) == false)
                throw new InvalidOperationException($"{nameof(KenovaRouteView)} can only route to a type that inherits from {nameof(KenovaDialogBase)}");

            // Set the IsOpenedAsRoutedPage parameter.

            Dictionary<string, object> dict = RouteData.RouteValues as Dictionary<string, object>;

            if (dict == null)
                throw new InvalidOperationException("RouteValues is not the correct type");

            dict["IsOpenedAsRoutedPage"] = true;

            //AuthenticationState state = await TaskAuth;

            if (KenovaClientConfig.Diagnostics) Console.WriteLine("🛤 <KenovaRouteView> - OnParametersSet - EXIT");
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (KenovaClientConfig.Diagnostics) Console.WriteLine("🛤 <KenovaRouteView> - OnAfterRenderAsync - ENTRY");

                await Startup.RefreshTokenAsync();

                RefreshTokenMode = false;

                if (KenovaClientConfig.Diagnostics) Console.WriteLine("🛤 <KenovaRouteView> is calling StateHasChanged()");

                this.StateHasChanged();

                if (KenovaClientConfig.Diagnostics) Console.WriteLine("🛤 <KenovaRouteView> - OnAfterRenderAsync - EXIT");
            }
        }


    }

}
