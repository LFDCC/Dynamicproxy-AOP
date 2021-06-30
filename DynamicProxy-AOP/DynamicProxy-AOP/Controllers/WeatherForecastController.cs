using DynamicProxy_AOP.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicProxy_AOP.Controllers
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

        ITestUser testUser;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ITestUser testUser)
        {
            _logger = logger;
            this.testUser = testUser;
        }

        [HttpGet("run")]
        public void Run(string arg)
        {
            testUser.Run(arg);
        }

        [HttpGet("run1")]
        public string Run1(string arg)
        {
            var val = testUser.Run1(arg);
            return val;
        }

        [HttpGet("run2")]
        public async Task Run2(string arg)
        {
            await testUser.Run2(arg);
        }

        [HttpGet("run3")]
        public async Task<string> Run3(string arg)
        {
            var val = await testUser.Run3(arg);
            return val;
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
    }
}
