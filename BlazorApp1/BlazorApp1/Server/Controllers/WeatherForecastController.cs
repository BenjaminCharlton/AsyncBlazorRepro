using BlazorApp1.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyConstitution.WebApi.Controllers
{
    [Route("[controller]", Name = nameof(WeatherForecastController))]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var rng = new Random();

            // Create a boolean flag from a random 1 or 0.
            //For demonstration, this will decide if we return a result or a problem.
            bool makeTrouble = rng.Next(0, 2) == 1;

            if (makeTrouble)
            {
                return BadRequest(new ProblemDetails
                {
                    Detail = "Something troublesome happened"
                });
            }
            else
            {
                var model = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                });

                return Ok(model);
            }
        }
    }
}
