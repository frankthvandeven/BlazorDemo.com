using BlazorDemo.Server;
using BlazorDemo.Server.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Data.SqlClient;
using System.Text;
using VenturaSQL;
using VenturaSQL.AspNetCore.Server.RequestHandling;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddSignalR();

builder.Services.AddControllers(options =>
{
    options.InputFormatters.Insert(0, new FrameStreamInputFormatter());

});

builder.Services.AddRazorPages(); /* = support for .cshtml pages */

//builder.Services.AddResponseCompression(opts =>
//{
//    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
//        new[] { "application/octet-stream" });
//});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BlazorDemo.Server", Version = "v1" });
});

var app = builder.Build();

//string db_folder = env.ContentRootPath;
//string connection_string = $@"Data Source={db_folder}\VanArsdel.db";
//VenturaConfig.DefaultConnector = new AdoConnector(SqliteFactory.Instance, connection_string);

string cs_bikestores = app.Configuration.GetConnectionString("BikeStores");

ServerConnector.BikeStores = new AdoConnector(SqlClientFactory.Instance, cs_bikestores);

VenturaSqlConfig.DefaultConnector = ServerConnector.BikeStores;

// For Apache on Ubuntu
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();

    // Swagger
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication9 v1"));

}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization(); // Obligated, must be placed between UseRouting() and UseEndpoints()

app.MapRazorPages(); /* = support for .cshtml pages */
app.MapControllers();
//app.MapHub<ChatHub>("/chathub");
app.MapFallbackToFile("index.html");

app.Run();



