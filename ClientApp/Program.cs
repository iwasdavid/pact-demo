using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClientApp
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var api = new WeatherForecastApiClient();
            
            var result = await api.GetSomething();

            Console.WriteLine(result);
            Console.WriteLine();
        }
    }
}