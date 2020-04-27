using BlazorApp1.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorApp1.Client.Services
{
    public class WeatherForecastsService : ServiceBase
    {
        public WeatherForecastsService(
            HttpClient client) : base(client)
        {

        }

        public async Task GetAllAsync(
            Action<IEnumerable<WeatherForecast>> actionOnSuccess,
            Action<ProblemDetails> actionOnFailure,
            CancellationToken cancellationToken = default)
        {
            await GetManyAsync("weatherforecast",
                actionOnSuccess,
                actionOnFailure,
                cancellationToken);
        }
    }
}
