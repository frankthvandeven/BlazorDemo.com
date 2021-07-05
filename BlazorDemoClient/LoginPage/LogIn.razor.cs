using BlazorDemo.Shared;
using Kenova.WebAssembly.Client.Components;
using Kenova.WebAssembly.Client.Util;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Kenova.WebAssembly.Client.Pages
{
    public partial class LogIn : LayerComponentBase
    {
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public LoginModel Model { get; set; } = new LoginModel();

        protected override void OnLayerInitialized()
        {
            if (Model == null)
                throw new ArgumentNullException("model");

        }

        private async void LoginClicked()
        {
            LoginResult result = await LongRunningTask.SimpleRun("Logging in", Model.LoginTask);

            if (result.Authorized == false)
            {
                await MessageBox.ShowAsync("Sign in failed", $"Message from server: {result.Message}");
                return;
            }

            var state = await KenovaClientConfig.AuthenticationStateProvider.GetAuthenticationStateAsync();

            var navManager = KenovaClientConfig.NavigationManager;

            //Microsoft.Extensions.Primitives.StringValues initCount;
            //var uri = navManager.ToAbsoluteUri(navManager.Uri);

            navManager.NavigateTo("/");

            //if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("initialCount", out initCount))
            //{
            //    currentCount = Convert.ToInt32(initCount);
            //}
        }





    }
}
