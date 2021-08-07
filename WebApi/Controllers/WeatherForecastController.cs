using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [HttpGet("Get1")]
        public  async Task<IActionResult> Get1()
        {
            await Task.Delay(TimeSpan.FromSeconds(2));

            return Ok(new {Data=20000});
        }

        [HttpGet("Get2")]
        public async Task<IActionResult> Get2()
        {
            await Task.Delay(TimeSpan.FromSeconds(2));

            return Ok(new { Data = 20000 });
        }


        [HttpGet("Get3")]
        public async Task<IActionResult> Get3()
        {
            await Task.Delay(TimeSpan.FromSeconds(3));

            return Ok(new { Data = 30000 });
        }
    }
}
