using BlazorDemo.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using VenturaSQL;

// *never* call builder.Services.BuildServiceProvider() to get a service reference.
// Use host.Services.GetService() instead.

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddOptions(); // Needed for Authorization according to docs. https://docs.microsoft.com/en-us/aspnet/core/blazor/security/?view=aspnetcore-6.0
builder.Services.AddAuthorizationCore();

builder.Services.AddKenovaClient<Startup>();

//builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var host = builder.Build();

host.Services.KenovaInitialize();

/// VenturaSQL requests HttpClient instances from Blazor WebAssembly.
VenturaSqlConfig.SetHttpClientFactory(connector => host.Services.GetService(typeof(HttpClient)) as HttpClient);

// This is for VenturaSQL
ClientConnector.BikeStores = new HttpConnector("BikeStores", "api/venturasql");

VenturaSqlConfig.DefaultConnector = ClientConnector.BikeStores;

await host.RunAsync();