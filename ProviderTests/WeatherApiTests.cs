using System;
using System.Collections.Generic;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PactNet;
using PactNet.Infrastructure.Outputters;
using Xunit;
using Xunit.Abstractions;

namespace ProviderTests
{
    public sealed class WeatherApiTests : IDisposable
    {
        private readonly string _providerUri;
        private readonly string _pactServiceUri;
        private readonly IWebHost _webHost;
        private readonly ITestOutputHelper _outputHelper;

        public WeatherApiTests(ITestOutputHelper output)
        {
            _outputHelper = output;
            _providerUri = "http://localhost:61048"; // Weather API base address
            _pactServiceUri = "http://localhost:9009";

            _webHost = WebHost.CreateDefaultBuilder()
                .UseUrls(_pactServiceUri)
                .UseStartup<TestStartup>()
                .Build();

            _webHost.Start();
        }

        [Fact]
        public void EnsureWeatherApiHonoursPactWithWeatherApiConsumer()
        {
            // Arrange
            var pacts = @"..\..\..\..\ClientTests\pacts\weather_api_consumer-weather_api_provider.json";

            var config = new PactVerifierConfig
            {
                Outputters = new List<IOutput>
                                {
                                    new XUnitOutput(_outputHelper)
                                },

                Verbose = true
            };
            
            // Act, Assert
            IPactVerifier pactVerifier = new PactVerifier(config);
            pactVerifier.ProviderState($"{_pactServiceUri}/provider-states")
                .ServiceProvider("Weather API Provider", _providerUri)
                .HonoursPactWith("Weather API Consumer")
                .PactUri(pacts)
                .Verify();
        }

        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _webHost.StopAsync().GetAwaiter().GetResult();
                    _webHost.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}