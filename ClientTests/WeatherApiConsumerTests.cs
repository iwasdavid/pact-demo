using System.Collections.Generic;
using System.Threading.Tasks;
using ClientApp;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Xunit;

namespace ClientTests
{
    public class WeatherApiConsumerTests : IClassFixture<ConsumerMyApiPact>
    {
        private readonly IMockProviderService _mockProviderService;
        private readonly string _mockProviderServiceBaseUri;

        public WeatherApiConsumerTests(ConsumerMyApiPact data)
        {
            _mockProviderService = data.MockProviderService;
            _mockProviderService.ClearInteractions(); //NOTE: Clears any previously registered interactions before the test is run
            _mockProviderServiceBaseUri = ConsumerMyApiPact.MockProviderServiceBaseUri;
        }

        [Fact]
        public async Task Get_FromWeatherAPI_ReturnsTwoWeatherForecasts()
        {
            //Arrange
            _mockProviderService
                .Given("WeatherForecast api returns 2 forecast")
                .UponReceiving("A GET request to retrieve the forecast")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/WeatherForecast/2",
                    Headers = new Dictionary<string, object>
                    {
                        { "Accept", "application/json" }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new List<dynamic>
                    {
                        new {
                            summary = "Freezing",
                            temperatureC = 0,
                            temperatureF = 32
                        },
                        new {
                            summary = "Bracing",
                            temperatureC = 1,
                            temperatureF = 33
                        }
                    } //NOTE: Note the case sensitivity here, the body will be serialised as per the casing defined
                    
                }); //NOTE: WillRespondWith call must come last as it will register the interaction

            var consumer = new WeatherForecastApiClient(_mockProviderServiceBaseUri);
            const int forecastCount = 2;

            //Act
            var result = await consumer.GetForecasts(forecastCount);

            //Assert
            Assert.Equal(forecastCount, result.Length);

            _mockProviderService.VerifyInteractions(); //NOTE: Verifies that interactions registered on the mock provider are called at least once
        }
    }
}