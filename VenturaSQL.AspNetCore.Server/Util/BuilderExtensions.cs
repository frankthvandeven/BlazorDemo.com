using VenturaSQL.AspNetCore.Server.RequestHandling;

namespace Microsoft.AspNetCore.Builder
{
    public static class VenturaSQLBuilderExtensions
    {

        /// <summary>
        /// Map the "/Ventura.FSPRO" request path to the VenturaSQL framestream processor middleware.
        /// </summary>
        //public static IApplicationBuilder UseVenturaSqlListener(this IApplicationBuilder app)
        //{
        //    app.Map("/Ventura.FSPRO", z => z.UseMiddleware<FrameStreamProcessor>());
        //    return app;
        //}

    }
}
