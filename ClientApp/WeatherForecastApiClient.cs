using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ClientApp.Models;
using Newtonsoft.Json;

namespace ClientApp
{
    public class WeatherForecastApiClient
    {
        private readonly HttpClient _client;
    
        public WeatherForecastApiClient(string baseUri = null)
        {
            _client = new HttpClient { BaseAddress = new Uri(baseUri ?? "https://localhost:44392") };
        }
        
        public async Task<WeatherForecast> GetSomething()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/WeatherForecast");
            request.Headers.Add("Accept", "application/json");

            var response = await _client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            var status = response.StatusCode;

            var reasonPhrase = response.ReasonPhrase;

            request.Dispose();
            response.Dispose();

            if (status == HttpStatusCode.OK)
                return !string.IsNullOrEmpty(content)
                    ? JsonConvert.DeserializeObject<WeatherForecast>(content)
                    : null;

            throw new Exception(reasonPhrase);
        }
    }
}