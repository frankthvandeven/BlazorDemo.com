namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServerServiceExtensions
    {

        public static void AddKenovaServer(this IServiceCollection services)
        {
            // Make the same instance accessible as both AuthenticationStateProvider and TokenAuthenticationStateProvider
            //services.AddScoped<KenovaAuthenticationStateProvider>();
            //services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<KenovaAuthenticationStateProvider>());

        }

    }
}
