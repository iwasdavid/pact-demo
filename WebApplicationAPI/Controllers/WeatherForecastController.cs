using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        
        [HttpGet("{count}")]
        public WeatherForecast[] Get(int count)
        {
            var forecasts = new List<WeatherForecast>();
            
            for (var i = 0; i < count; i++)
            {
                var summariesIndex = i < 10 ? i : 0;
                    
                forecasts.Add(new WeatherForecast
                {
                    TemperatureC = i,
                    Summary = Summaries[summariesIndex]
                });
            }

            return forecasts.ToArray();
        }
    }
}