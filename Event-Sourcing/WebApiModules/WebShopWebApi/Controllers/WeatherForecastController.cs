using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebShopWebApi.Commands;
using WebShopWebApi.DTOs;

namespace WebShopWebApi.Controllers;

public class WeatherForecast
{
    public DateTime Date { get; set; }
    public float TemperatureC { get; set; }
    public string Summary { get; set; } = string.Empty;
}

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : BaseMediaRController
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing",
        "Bracing",
        "Chilly",
        "Cool",
        "Mild",
        "Warm",
        "Balmy",
        "Hot",
        "Sweltering",
        "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator)
        : base(mediator)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable
            .Range(1, 5)
            .Select(
                index =>
                    new WeatherForecast
                    {
                        Date = DateTime.Now.AddDays(index),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                    }
            )
            .ToArray();
    }

    [HttpPost("random-test")]
    public async Task<TestDTO> Post([FromBody] TestCommand testCommand)
    {
        return await Execute<TestDTO>(testCommand);
    }
}
