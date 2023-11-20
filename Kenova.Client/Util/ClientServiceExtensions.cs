using Kenova.Client;
using Kenova.Client.Authorization;
using Kenova.Client.Localization;
using Microsoft.AspNetCore.Components.Authorization;

namespace Microsoft.Extensions.DependencyInjection;

public static class ClientServiceExtensions
{

    /// <summary>
    /// Add the Kenova Client services.
    /// The services are KenovaLocalizer, KenovaStartup and KenovaAuthenticationStateProvider.
    /// </summary>
    /// <typeparam name="TStartup">Class type inherited from KenovaStartup.</typeparam>
    public static void AddKenovaClient<TStartup>(this IServiceCollection services) where TStartup : KenovaStartup, new()
    {
        if (services == null)
            throw new ArgumentNullException("services");

        services.AddSingleton<KenovaLocalizer>();

        services.AddSingleton<KenovaStartup, TStartup>();

        //services.AddScoped<KenovaAuthenticationStateProvider>();
        //services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<KenovaAuthenticationStateProvider>());
        services.AddSingleton<KenovaAuthenticationStateProvider>();
        services.AddSingleton<AuthenticationStateProvider>(provider => provider.GetRequiredService<KenovaAuthenticationStateProvider>());

    }

    /// <summary>
    /// Kenova Client initializations that run before the Blazor components start running.
    /// </summary>
    public static void KenovaInitialize(this IServiceProvider services)
    {
        KenovaClientConfig.ServiceProvider = services;
    }

}
