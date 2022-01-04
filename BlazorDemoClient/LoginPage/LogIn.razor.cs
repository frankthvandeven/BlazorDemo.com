using BlazorDemo.Shared;
using Kenova.Client.Components;
using Microsoft.AspNetCore.Components;

namespace Kenova.Client.Pages
{
    public partial class LogIn : KenovaDialogBase
    {

        [Parameter]
        [SupplyParameterFromQuery]
        public string ReturnTo { get; set; }

        private LoginModel Model = new LoginModel();

        private async void LoginClicked()
        {
            LoginResult result = await LongRunningTask.SimpleRun("Logging in", Model.LoginTask);

            if (result.Authorized == false)
            {
                await MessageBox.ShowAsync("Sign in failed", $"Message from server: {result.Message}");
                return;
            }

            await NavigationManager.PortalNavigateToAsync(this.ReturnTo??"/");

        }

    }
}

//var state = await KenovaClientConfig.AuthenticationStateProvider.GetAuthenticationStateAsync();
//var navManager = KenovaClientConfig.NavigationManager;
//Microsoft.Extensions.Primitives.StringValues initCount;
//var uri = navManager.ToAbsoluteUri(navManager.Uri);
//if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("initialCount", out initCount))
//{
//    currentCount = Convert.ToInt32(initCount);
//}
