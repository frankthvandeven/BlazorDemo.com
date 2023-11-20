using Kenova.Client;

namespace Microsoft.Extensions.DependencyInjection;

public abstract class KenovaStartup
{

    public abstract Task SetupPortalAsync();

    /// <summary>
    /// The "Authorizing..." message is displayed on screen while the token is 
    /// being refreshed. If not set, the saved login token from Local Storage is loaded. If that fails,
    /// the login dialog will be displayed.
    /// </summary>
    public abstract Task RefreshTokenAsync();

    // await KenovaClientConfig.AuthenticationStateProvider.AuthenticateWithSavedTokenAsync();

}
