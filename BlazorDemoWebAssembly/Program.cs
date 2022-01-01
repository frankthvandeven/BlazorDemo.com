using BlazorDemo.Client;
using BlazorDemoWebAssembly;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using VenturaSQL;

// *never* call builder.Services.BuildServiceProvider() to get a service reference.
// Use host.Services.GetService() instead.

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");

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


// Snippets:
//var factory = builder.Services.GetService<IHttpClientFactory>();
//builder.Services.AddCors(c => c.AddDefaultPolicy(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
//
//  foreach(ServiceDescriptor srv in builder.Services)
//    {
//         Console.WriteLine("SERVICE " + srv.ServiceType.ToString()  + "  " + srv.Lifetime.ToString() );
//    }

// Using IHttpClientFactory with Blazor WebAssembly
// https://medium.com/@marcodesanctis2/using-ihttpclientfactory-with-blazor-webassembly-7cc702f5e9f8

// About IHttpClientFactory
// https://davemateer.com/2019/11/16/IHttpClientFactory

// Using HttpClient the wrong way:
// https://josef.codes/you-are-probably-still-using-httpclient-wrong-and-it-is-destabilizing-your-software/

// David Fowler on how to do ASYNC right
// https://github.com/davidfowl/AspNetCoreDiagnosticScenarios/blob/master/AsyncGuidance.md

// ASP.Net CORE ScopedService dep injection
// https://volosoft.com/blog/ASP.NET-Core-Dependency-Injection
