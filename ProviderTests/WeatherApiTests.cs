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
    public class WeatherApiTests : IDisposable
    {
        private readonly string _providerUri;
        private readonly string _pactServiceUri;
        private readonly IWebHost _webHost;
        private readonly ITestOutputHelper _outputHelper;

        public WeatherApiTests(ITestOutputHelper output)
        {
            _outputHelper = output;
            _providerUri = "http://localhost:61048";
            _pactServiceUri = "http://localhost:9009";

            _webHost = WebHost.CreateDefaultBuilder()
                .UseUrls(_pactServiceUri)
                .UseStartup<TestStartup>()
                .Build();

            _webHost.Start();
        }

        [Fact]
        public void EnsureProviderApiHonoursPactWithConsumer()
        {
            // Arrange
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
                .ServiceProvider("Weather API", _providerUri)
                .HonoursPactWith("Consumer")
                .PactUri(@"C:\Users\adavidl\RiderProjects\PactDemo\ClientTests\pacts\consumer-weather_api.json")
                .Verify();
        }

        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
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