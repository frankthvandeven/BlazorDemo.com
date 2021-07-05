using Blazor1.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlazorDemo.Server.VenturaAutoCreate;
using BlazorDemo.Server.VenturaRecordsets;

namespace Blazor1.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<TestController> logger;

        public TestController(ILogger<TestController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public IEnumerable<Cust_Record> Get()
        {
            Cust_Recordset rs = new Cust_Recordset();

            rs.ExecSql();

            return rs.ToArray();
        }
    }
}
