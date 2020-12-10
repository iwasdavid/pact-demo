using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClientApp
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var numberOfForecasts = int.Parse(Console.ReadLine() ?? "10");
            var api = new WeatherForecastApiClient();
            
            var result = await api.GetForecasts(numberOfForecasts);

            Console.WriteLine($"You have received {result.Length} forecasts");
            Console.WriteLine();
        }
    }
}