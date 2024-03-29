﻿using BlazorDemo.Client;
using Kenova.Client.Components;
using System.Threading.Tasks;

namespace Kenova.Client.Pages
{
    [ViewModel]
    public partial class LoginModel
    {
        private string __Username;
        private string __Password;

        public LoginModel()
        {
            Username = "demo";
            Password = "demo";

            Register(m => m.Username);
            Register(m => m.Password);
        }

        protected override async Task ValidateEventAsync(ValidateEventArgs<LoginModel> e)
        {
            if (e.IsMember(m => m.Username))
            {
                await Task.CompletedTask;
            }
            else if (e.IsMember(m => m.Password))
            {
            }

        }

        public async Task<LoginResult> LoginTask()
        {
            var credentials = new LoginCredentials();

            credentials.UserName = this.Username;
            credentials.Password = this.Password;

            LoginResult result = await KenovaHttp.PostJsonAsync<LoginResult>("api/user/login", credentials);

            var loginFailed = result.Token == null;

            if (!loginFailed)
            {
                // Success! Store token in underlying auth state service
                await KenovaClientConfig.AuthenticationStateProvider.AuthenticateWithNewTokenAsync(result.Token);

                KenovaClientConfig.Labels.UserName = result.DisplayName;
                KenovaClientConfig.Labels.DirectoryInfo = "Database: BikeStores";
            }

            return result;
        }


    }
}
