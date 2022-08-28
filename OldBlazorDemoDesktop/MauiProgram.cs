using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Hosting;
using System.Net.Http;
using VenturaSQL;
using BlazorDemo.Client;
using System;

namespace BlazorDesktopDemo
{
    public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				});

			builder.Services.AddMauiBlazorWebView();

			builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001") });

			builder.Services.AddOptions(); // Needed for Authorization according to docs. https://docs.microsoft.com/en-us/aspnet/core/blazor/security/?view=aspnetcore-6.0
			builder.Services.AddAuthorizationCore();

			builder.Services.AddKenovaClient<Startup>();

			var host = builder.Build();

			host.Services.KenovaInitialize();

			// VenturaSQL requests HttpClient instances from Blazor.
			VenturaSqlConfig.SetHttpClientFactory(connector => host.Services.GetService(typeof(HttpClient)) as HttpClient);

			// This is for VenturaSQL
			ClientConnector.BikeStores = new HttpConnector("BikeStores", "api/venturasql");

			VenturaSqlConfig.DefaultConnector = ClientConnector.BikeStores;

			return host;

		}

	}
}