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
        
        public async Task<WeatherForecast[]> GetForecasts(int count)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"/WeatherForecast/{count}");
            request.Headers.Add("Accept", "application/json");

            using var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
                return !string.IsNullOrEmpty(content)
                    ? JsonConvert.DeserializeObject<WeatherForecast[]>(content)
                    : null;

            throw new Exception(response.ReasonPhrase);
        }
    }
}