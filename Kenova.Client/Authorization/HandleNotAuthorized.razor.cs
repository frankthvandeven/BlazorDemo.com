using Kenova.Client.Components;
using Microsoft.AspNetCore.Components;

namespace Kenova.Client.Authorization
{
    public partial class HandleNotAuthorized : KenovaComponentBase
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private bool IsAuthenticated;

        protected override void OnInitialized()
        {
            
            IsAuthenticated = KenovaClientConfig.AuthenticationStateProvider.IsAuthenticated;



        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (IsAuthenticated == false)
                {
                    if (KenovaClientConfig.Diagnostics) Console.WriteLine("🧭🧭🧭🧭🧭🧭🧭 calling NavigateTo for auth/login ");
                    await NavigationManager.PortalNavigateToAsync($"auth/login?returnto={Uri.EscapeDataString(NavigationManager.Uri)}");
                }

            }
        }




    }

}

