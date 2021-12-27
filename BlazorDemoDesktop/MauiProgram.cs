using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using BlazorDesktopDemo.Data;
using System.Net.Http;
using System;
using VenturaSQL;
using System.Diagnostics;

namespace BlazorDesktopDemo
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.RegisterBlazorMauiWebView()
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				});

			builder.Services.AddBlazorWebView();

			//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

			builder.Services.AddOptions(); // Needed for Authorization according to docs. https://docs.microsoft.com/en-us/aspnet/core/blazor/security/?view=aspnetcore-6.0

			builder.Services.AddAuthorizationCore();

			builder.Services.AddKenovaClient<Startup>();

			var host = builder.Build();

			host.Services.KenovaInitialize();

			/// VenturaSQL requests HttpClient instances from Blazor WebAssembly.
			VenturaSqlConfig.SetHttpClientFactory(connector => host.Services.GetService(typeof(HttpClient)) as HttpClient);

			SetupVenturaSQL();


			return host;

		}

		private static void SetupVenturaSQL()
		{
			// This is for VenturaSQL
			//ClientConnector.BikeStores = new HttpConnector("BikeStores", "api/venturasql");

			//VenturaSqlConfig.DefaultConnector = ClientConnector.BikeStores;
		}
	}
}