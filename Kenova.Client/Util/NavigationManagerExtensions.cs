using Kenova.Client.Components;

namespace Microsoft.AspNetCore.Components;

public static class NavigationManagerExtensions
{

    public static async Task PortalNavigateToAsync(this NavigationManager manager, string uri, bool forceLoad)
    {
        await LayerManager.AbortAllAsync();

        manager.NavigateTo(uri, forceLoad, replace: false);
    }

    public static async Task PortalNavigateToAsync(this NavigationManager manager, string uri, bool forceLoad = false, bool replace = false)
    {
        await LayerManager.AbortAllAsync();

        manager.NavigateTo(uri, forceLoad, replace);
    }

    public static async Task PortalNavigateToAsync(this NavigationManager manager, string uri, NavigationOptions options)
    {
        await LayerManager.AbortAllAsync();

        manager.NavigateTo(uri, options);
    }

}
