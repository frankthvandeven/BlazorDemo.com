using BlazorDemo.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VenturaSQL;
using VenturaSQL.AspNetCore.Server.RequestHandling;

namespace BlazorDemo.Server.Controllers
{
    [ApiController]
    [AllowAnonymous] // Replace with [Authorize] in production environments
    public class VenturaSqlController : ControllerBase
    {
        [Route("api/venturasql")]
        [HttpPost]
        public Task Index(byte[] requestData)
        {
            var processor = new VenturaSqlServerEngine();

            processor.RequestData = requestData;
            processor.CallBacks.LookupAdoConnector = LookupAdoConnector;

            processor.Exec();

            return Response.Body.WriteAsync(processor.ResponseBuffer, 0, processor.ResponseLength);
        }

        private AdoConnector LookupAdoConnector(string requestedName)
        {
            if (requestedName == "BikeStores")
                return ServerConnector.BikeStores;

            throw new Exception($"Requested connector name {requestedName} is unknown on server.");
        }
    }
}


// "application/octet-stream";

//
//            HttpResponseException()

//    catch (HttpException ex)
//        return result = Request.CreateResponse(HttpStatusCode.BadGateway, "Http Exception Come" + ex.Message);

// API error handling: https://stackify.com/web-api-error-handling/
// https://stackoverflow.com/questions/41757245/request-createresponse-in-asp-net-core
// https://code-maze.com/global-error-handling-aspnetcore/


//return this.StatusCode(500, "meriaty");

