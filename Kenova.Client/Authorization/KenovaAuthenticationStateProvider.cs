using Kenova.Client.Util;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kenova.Client.Authorization
{
    public class KenovaAuthenticationStateProvider : AuthenticationStateProvider
    {

        private const string KEYNAME = "auth_token";

        private bool _isAuthenticated;
        private string _userName;
        private bool _isAdministrator;
        private string _token;
        private AuthenticationState _authenticationState;

        public KenovaAuthenticationStateProvider()
        {
            _isAuthenticated = false;
            _userName = "";
            _isAdministrator = false;
            _token = null;
            _authenticationState = anonymous_authentication_state();
        }

        private AuthenticationState anonymous_authentication_state()
        {
            var identity = new ClaimsIdentity(); // Anonymous and not authenticated.
            var principal = new ClaimsPrincipal(identity);
            return new AuthenticationState(principal);
        }

        public bool IsAuthenticated
        {
            get { return _isAuthenticated; }
        }

        public string UserName
        {
            get { return _userName; }
        }

        public bool IsAdministrator
        {
            get { return _isAdministrator; }
        }

        public string Token
        {
            get { return _token; }
        }


        /// <summary>
        /// Get the token stored in the browser's localstorage.
        /// </summary>
        /// <returns>The token or null if there is no token in localstorage.</returns>
        public ValueTask<string> GetSavedTokenAsync()
        {
            return LocalStorage.GetStringAsync(KEYNAME);
        }


        /// <summary>
        /// Authenticate with the token from the browser's local storage.
        /// If there is no token in localstorage, nothing will happen.
        /// </summary>
        public async ValueTask AuthenticateWithSavedTokenAsync()
        {
            if (KenovaClientConfig.Diagnostics) Console.WriteLine("🔑 AuthenticationStateProvider - AuthenticateWithSavedTokenAsync called.");

            var token = await LocalStorage.GetStringAsync(KEYNAME);

            if (string.IsNullOrEmpty(token))
                return;

            from_token_to_properties(token);

            if (KenovaClientConfig.Diagnostics) Console.WriteLine("🔒 Triggering authentication state event (AuthenticateWithSavedTokenAsync).");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async ValueTask AuthenticateAnonymousAsync()
        {
            if (KenovaClientConfig.Diagnostics) Console.WriteLine("🔑 AuthenticationStateProvider - AuthenticateAnonymousAsync called.");

            _isAuthenticated = false;
            _userName = "";
            _isAdministrator = false;
            _token = null;
            _authenticationState = anonymous_authentication_state();

            await LocalStorage.RemoveAsync(KEYNAME);

            if (KenovaClientConfig.Diagnostics) Console.WriteLine("🔒 Triggering authentication state event (AuthenticateAnonymousAsync).");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        /// <summary>
        /// Locally authenticate and save the token in the browser's local storage.
        /// </summary>
        public async ValueTask AuthenticateWithNewTokenAsync(string token)
        {
            if (KenovaClientConfig.Diagnostics) Console.WriteLine("🔑 AuthenticationStateProvider - AuthenticateWithNewTokenAsync called.");

            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException("token");

            from_token_to_properties(token);

            await LocalStorage.SetStringAsync(KEYNAME, token);

            if (KenovaClientConfig.Diagnostics) Console.WriteLine("🔒 Triggering authentication state event (AuthenticateWithNewTokenAsync).");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        private void from_token_to_properties(string token)
        {
            ClaimsIdentity identity;

            var claims = ClientJwtHelper.ParseClaimsFromJwt(token);

            identity = new ClaimsIdentity(claims, "jwt", ClientJwtHelper.CLAIMTYPE_NAME, ClientJwtHelper.CLAIMTYPE_ROLE);

            //ClaimTypes.Name
            //claims.FirstOrDefault(c => c..Type == )

            var principal = new ClaimsPrincipal(identity);

            _isAuthenticated = true;
            _userName = identity.Name ?? "(null)";
            _isAdministrator = true;
            _token = token;
            _authenticationState = new AuthenticationState(principal);

        }

        /// <summary>
        /// Sign out and delete the token from the browser's local storage.
        /// </summary>
        public async Task SignOutAsync()
        {
            if (KenovaClientConfig.Diagnostics) Console.WriteLine("🔑 AuthenticationStateProvider - SignOutAsync called.");

            _isAuthenticated = false;
            _userName = "";
            _isAdministrator = false;
            _token = null;
            _authenticationState = anonymous_authentication_state();
            
            await LocalStorage.RemoveAsync(KEYNAME);

            if (KenovaClientConfig.Diagnostics) Console.WriteLine("🔒 Triggering authentication state event (SignOutAsync).");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        private int getcount = 1;

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {

#if DEBUG
            var authstatus = "(authorization did not run yet)";

            if (_authenticationState != null)
            {
                authstatus = _authenticationState.User.Identity.IsAuthenticated ? "(authenticated)" : "(not authenticated)";
            }

            if (KenovaClientConfig.Diagnostics) Console.WriteLine($"🔑 AuthenticationStateProvider - GetAuthenticationStateAsync called - call count {getcount++} {authstatus}.");
#endif
            // The returned authenticationstate can never be null!
            // We return a task that already completed.
            return Task.FromResult(_authenticationState);
        }
    }
}
