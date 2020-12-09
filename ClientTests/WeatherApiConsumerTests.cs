using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClientApp;
using ClientApp.Models;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Xunit;

namespace ClientTests
{
    public class WeatherApiConsumerTests : IClassFixture<ConsumerMyApiPact>
    {
        private IMockProviderService _mockProviderService;
        private string _mockProviderServiceBaseUri;

        public WeatherApiConsumerTests(ConsumerMyApiPact data)
        {
            _mockProviderService = data.MockProviderService;
            _mockProviderService.ClearInteractions(); //NOTE: Clears any previously registered interactions before the test is run
            _mockProviderServiceBaseUri = ConsumerMyApiPact.MockProviderServiceBaseUri;
        }

        [Fact]
        public async Task Get_FromWeatherAPI_ReturnsOneWeatherForecast()
        {
            //Arrange
            _mockProviderService
                .Given("WeatherForecast api returns 1 forecast")
                .UponReceiving("A GET request to retrieve the forecast")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/WeatherForecast",
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
                    Body = new 
                    {
                        summary = "Scorching",
                        temperatureC = 12,
                        temperatureF = 53
                    } //NOTE: Note the case sensitivity here, the body will be serialised as per the casing defined
                    
                }); //NOTE: WillRespondWith call must come last as it will register the interaction

            var consumer = new WeatherForecastApiClient(_mockProviderServiceBaseUri);

            //Act
            var result = await consumer.GetSomething();

            //Assert
            Assert.Equal(12, result.TemperatureC);

            _mockProviderService.VerifyInteractions(); //NOTE: Verifies that interactions registered on the mock provider are called at least once
        }
    }
}