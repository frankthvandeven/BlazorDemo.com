using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kenova.Server.Controllers
{
    [ApiController]
    [Route("api/test/[controller]")]
    public class ModernController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ModernController> _logger;

        public ModernController(ILogger<ModernController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Simdata> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new Simdata
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }

    public class Simdata
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}
